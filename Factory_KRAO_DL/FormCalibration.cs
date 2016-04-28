using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Timers;
using System.Text;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace Factory_KRAO_DL
{
    public partial class FormCalibration : Form
    {
        /// <summary>
        /// 消息传递事件处理委托
        /// </summary>
        public delegate void DelegateMessage(object sender, string message);

        /// <summary>
        /// 刷新数据表
        /// </summary>
        private DataTable _dtSmp = new DataTable();
        private DataTable _dtRunValue = new DataTable();
        private DataTable _dtResidualLimit = new DataTable();
        private DataTable _dtTrip = new DataTable();
        private DataTable _dtRatedResidual = new DataTable();

        //private SerialPort _com = new SerialPort();
        private byte[] _address = new byte[6];
        private bool _isBusy = false;
        private bool _isOffline = true;
        private object _lock = new object();
        private Timer _timer = new Timer();

        private byte[] _rcvBuf = new byte[250];

        //private int _residualShift = 0;

        public FormCalibration()
        {
            InitializeComponent();
        }

        private void FormCalibration_Load(object sender, EventArgs e)
        {
            btnOpen.Enabled = btnClose.Enabled = btnTest.Enabled = false;
            btnWriteAssetCode.Enabled = btnWriteDeviceNum.Enabled = btnWriteProductDate.Enabled = false;
            btnReadFactoryValue.Enabled = false;
            btnReadAssetCode.Enabled = btnReadDeviceNum.Enabled = btnReadProductDate.Enabled = false;
            comPort_Init();
            grdRunValue_init();
            grdSmp_init();
            grdResidualLimit_init();
            grdRatedResidual_init();
            grdTrip_init();
            
            btnReadAssetCode.Enabled = btnReadDeviceNum.Enabled
                = btnReadProductDate.Enabled = btnReadFactoryValue.Enabled = false;
            btnWriteAssetCode.Enabled = btnWriteDeviceNum.Enabled = btnWriteProductDate.Enabled = false;
        }

        /// <summary>
        /// 初始化端口
        /// </summary>
        private void comPort_Init()
        {
            int num;
            string[] portlist = SerialPort.GetPortNames();
            this.cbxCom.Items.Clear();
            for (num = 0; num < portlist.Length; num++)
            {
                this.cbxCom.Items.Add(portlist[num]);
            }
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
            for (num = 0; num < portlist.Length; num++)
            {
                this.stalabMBStatus.Text = "COM未连接";
                this.stalabMBPortName.Text = portlist[num];
                this.Refresh();
                cbxCom.SelectedIndex = this.cbxCom.Items.IndexOf(portlist[num]);
            }
            for (int i = 0; i < 6; i++)
            {
                _address[i] = byte.Parse(txtDetect1.Text.Substring(5 - i, 1));
            }
            CheckForIllegalCrossThreadCalls = false;
        }

        /// <summary>
        /// 连接串口
        /// </summary>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            lock (_lock)
            {
                try
                {
                    if (btnConnect.Text == "关闭串口") //关闭串口
                    {
                        btnConnect.Text = "连接串口";
                        serialPort1.DiscardOutBuffer();
                        serialPort1.DiscardInBuffer();
                        if (serialPort1.IsOpen == true)
                        {
                            serialPort1.Close();
                        }
                        stalabMBStatus.Text = "串口未连接";
                    }
                    else if (btnConnect.Text == "连接串口") //打开串口
                    {
                        serialPort1.PortName = cbxCom.Text;
                        serialPort1.BaudRate = int.Parse(cbxBaudRate.Text);
                        serialPort1.Parity = System.IO.Ports.Parity.Even; //偶校验
                        serialPort1.DataBits = 8;
                        serialPort1.StopBits = StopBits.One;

                        if (serialPort1.IsOpen == false)
                        {
                            serialPort1.Open();
                        }

                        serialPort1.DiscardOutBuffer();
                        serialPort1.DiscardInBuffer();
                        serialPort1.ReadTimeout = 500;
                        stalabMBPortName.Text = serialPort1.PortName;
                        stalabMBBaudRate.Text = serialPort1.BaudRate.ToString();

                        _timer.Enabled = true;
                        _timer.Interval = 1500;
                        _timer.AutoReset = true;
                        _timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
                        
                        btnConnect.Text = "关闭串口";
                    }
                }
                catch (Exception ex)
                {
                    offline_process(ex.Message);
                }
            }
        }

        private void ShowIsOffine(object sender, string message)
        {
            if (this.InvokeRequired)
            {
                DelegateMessage df = new DelegateMessage(ShowIsOffine);
                this.BeginInvoke(df, sender, message);
            }
            else
            {
                stalabMBIsOffline.Text = message;

                if (_isOffline)
                {
                    btnOpen.Enabled = btnClose.Enabled = btnTest.Enabled = false;
                    btnWriteAssetCode.Enabled = btnWriteDeviceNum.Enabled = btnWriteProductDate.Enabled = false;
                    btnReadFactoryValue.Enabled = false;
                    btnReadAssetCode.Enabled = btnReadDeviceNum.Enabled = btnReadProductDate.Enabled = false;
                    
                    grdRunValue.Enabled = false;
                    grdSmp.Enabled = false;
                    grdRatedResidual.Enabled = false;
                    grdTrip.Enabled = false;
                    grdResidualLimit.Enabled = false;
                }
                else
                {
                    btnOpen.Enabled = btnClose.Enabled = btnTest.Enabled = true;
                    btnWriteAssetCode.Enabled = btnWriteDeviceNum.Enabled = btnWriteProductDate.Enabled = true;
                    btnReadFactoryValue.Enabled = true;
                    btnReadAssetCode.Enabled = btnReadDeviceNum.Enabled = btnReadProductDate.Enabled = true;

                    grdRunValue.Enabled = true;
                    grdSmp.Enabled = true;
                    grdRatedResidual.Enabled = true;
                    grdTrip.Enabled = true;
                    grdResidualLimit.Enabled = true;
                }
            }
        }

        private void ShowMessage(object sender, string message)
        {
            if (this.InvokeRequired)
            {
                DelegateMessage df = new DelegateMessage(ShowMessage);
                this.BeginInvoke(df, sender, message);
            }
            else
            {
                stalabMBStatus.Text = message;
            }
        }

        #region 读取参数

        /// <summary>
        /// 读取运行参数
        /// </summary>
        private bool fill_datatable_RunValue()
        {
            if (serialPort1.IsOpen)
            {
                RunValue rv = new RunValue();
                _dtRunValue.Rows[0][1] = rv.Voltage_All(_address, serialPort1)[0];
                _dtRunValue.Rows[1][1] = rv.Voltage_All(_address, serialPort1)[1];
                _dtRunValue.Rows[2][1] = rv.Voltage_All(_address, serialPort1)[2];
                _dtRunValue.Rows[3][1] = rv.Residual_All(_address, serialPort1)[0];
                _dtRunValue.Rows[4][1] = rv.Residual_All(_address, serialPort1)[1];
                _dtRunValue.Rows[5][1] = rv.Current_All(_address, serialPort1)[0];
                _dtRunValue.Rows[6][1] = rv.Current_All(_address, serialPort1)[1];
                _dtRunValue.Rows[7][1] = rv.Current_All(_address, serialPort1)[2];
                _dtRunValue.Rows[8][1] = rv.Running_Status_1(_address, serialPort1)[0];
                _dtRunValue.Rows[9][1] = rv.Running_Status_1(_address, serialPort1)[1];
                _dtRunValue.Rows[10][1] = rv.Running_Status_1(_address, serialPort1)[2];
                _dtRunValue.Rows[11][1] = rv.Control_Word_4(_address, serialPort1)[0];
                _dtRunValue.Rows[12][1] = rv.Control_Word_4(_address, serialPort1)[1];
                _dtRunValue.Rows[13][1] = rv.Control_Word_4(_address, serialPort1)[2];

                return true;
            }
            else
            {
                return false;
            }

        }
        
        /// <summary>
        /// 保护器跳闸事件记录
        /// </summary>
        /// <param name="rcv"></param>
        private void fill_datatable_Trip(ref byte[] rcv)
        {
            try
            {
                string str = "";
                //故障原因
                _dtTrip.Rows[0][1] = _dtRunValue.Rows[10][1];

                //故障相别
                _dtTrip.Rows[1][1] = rcv[15].ToString("X2");  

                //跳闸发生时刻
                for (int i = 0; i < 6; i++)
                {
                    str += rcv[16 + i].ToString("X2");
                }
                _dtTrip.Rows[2][1] = str;

                //跳闸前剩余电流
                str = rcv[22].ToString("X2");  
                str += rcv[23].ToString("X2");
                _dtTrip.Rows[3][1] = str;

                //跳闸前A相电压
                str = rcv[24].ToString("X2");
                str += rcv[25].ToString("X2");
                _dtTrip.Rows[4][1] = str;

                //跳闸前B相电压
                str = rcv[26].ToString("X2");
                str += rcv[27].ToString("X2");
                _dtTrip.Rows[5][1] = str;

                //跳闸前C相电压
                str = rcv[28].ToString("X2");
                str += rcv[29].ToString("X2");
                _dtTrip.Rows[6][1] = str;

                // 跳闸前A相电流
                str = rcv[30].ToString("X2");
                str += rcv[31].ToString("X2");
                str += rcv[32].ToString("X2");
                _dtTrip.Rows[7][1] = str;

                // 跳闸前B相电流
                str = rcv[33].ToString("X2");
                str += rcv[34].ToString("X2");
                str += rcv[35].ToString("X2");
                _dtTrip.Rows[8][1] = str;

                // 跳闸前B相电流
                str = rcv[36].ToString("X2");
                str += rcv[37].ToString("X2");
                str += rcv[38].ToString("X2");
                _dtTrip.Rows[9][1] = str;
            }
            catch (Exception)
            {
                stalabMBStatus.Text = "fill_datatable error";
            }
        }

        /// <summary>
        /// 剩余电流超限事件记录
        /// </summary>
        /// <param name="rcv"></param>
        private void fill_datatable_ResidualLimit(ref byte[] rcv)
        {
            try
            {
                string str = "";
                //剩余电流最大相
                _dtResidualLimit.Rows[0][1] = rcv[14].ToString("X2");  

                //剩余电流值
                str = rcv[15].ToString("X2");
                str = str + rcv[16].ToString("X2");  
                _dtResidualLimit.Rows[1][1] = str;
                
                //开始时间
                str = "";
                for (int i = 0; i < 6; i++)
                {
                    str += rcv[16 + i].ToString("X2");
                }
                _dtResidualLimit.Rows[2][1] = str;

                //报警结束时间
                str = "";
                for (int i = 0; i < 6; i++)
                {
                    str += rcv[22 + i].ToString("X2");
                }
                _dtResidualLimit.Rows[3][1] = str;
            }
            catch (Exception)
            {
                stalabMBStatus.Text = "fill_datatable error";
            }
        }

        private void grdRunValue_init()
        {
            _dtRunValue.Clear();

            DataColumn name = new DataColumn("name", typeof(string));
            name.ReadOnly = true;

            DataColumn value = new DataColumn("value", typeof(string));

            _dtRunValue.Columns.Add(name);
            _dtRunValue.Columns.Add(value);

            DataRow dr1 = _dtRunValue.NewRow();
            dr1["name"] = "A相电流(A):";
            dr1["value"] = 0;
            _dtRunValue.Rows.Add(dr1);

            DataRow dr2 = _dtRunValue.NewRow();
            dr2["name"] = "B相电流(A):";
            dr2["value"] = 0;
            _dtRunValue.Rows.Add(dr2);

            DataRow dr3 = _dtRunValue.NewRow();
            dr3["name"] = "C相电流(A):";
            dr3["value"] = 0;
            _dtRunValue.Rows.Add(dr3);

            DataRow dr4 = _dtRunValue.NewRow();
            dr4["name"] = "剩余电流最大相:";
            dr4["value"] = 0;
            _dtRunValue.Rows.Add(dr4);

            DataRow dr5 = _dtRunValue.NewRow();
            dr5["name"] = "剩余电流(mA):";
            dr5["value"] = 0;
            _dtRunValue.Rows.Add(dr5);

            DataRow dr6 = _dtRunValue.NewRow();
            dr6["name"] = "A相电压(V):";
            dr6["value"] = 0;
            _dtRunValue.Rows.Add(dr6);

            DataRow dr7 = _dtRunValue.NewRow();
            dr7["name"] = "B相电压(V):";
            dr7["value"] = 0;
            _dtRunValue.Rows.Add(dr7);

            DataRow dr8 = _dtRunValue.NewRow();
            dr8["name"] = "C相电压(V):";
            dr8["value"] = 0;
            _dtRunValue.Rows.Add(dr8);

            DataRow dr9 = _dtRunValue.NewRow();
            dr9["name"] = "告警:";
            dr9["value"] = 0;
            _dtRunValue.Rows.Add(dr9);

            DataRow dr10 = _dtRunValue.NewRow();
            dr10["name"] = "状态:";
            dr10["value"] = 0;
            _dtRunValue.Rows.Add(dr10);

            DataRow dr11 = _dtRunValue.NewRow();
            dr11["name"] = "原因:";
            dr11["value"] = 0;
            _dtRunValue.Rows.Add(dr11);

            DataRow dr12 = _dtRunValue.NewRow();
            dr12["name"] = "额定剩余电流动作值:";
            dr12["value"] = 0;
            _dtRunValue.Rows.Add(dr12);

            DataRow dr13 = _dtRunValue.NewRow();
            dr13["name"] = "额定极限不驱动时间:";
            dr13["value"] = 0;
            _dtRunValue.Rows.Add(dr13);

            DataRow dr14 = _dtRunValue.NewRow();
            dr14["name"] = "剩余电流报警时间:";
            dr14["value"] = 0;
            _dtRunValue.Rows.Add(dr14);

            grdRunValue.DataSource = _dtRunValue;

            grdRunValue.Columns[0].HeaderText = "名称";
            grdRunValue.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grdRunValue.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            grdRunValue.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            grdRunValue.Columns[1].HeaderText = "实测值";
            grdRunValue.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdRunValue.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

            grdRunValue.AllowUserToResizeColumns = false;
            grdRunValue.AllowUserToResizeRows = false;
            grdRunValue.AllowUserToOrderColumns = false;
            grdRunValue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //grdRunValue.Height = grdRunValue.ColumnHeadersHeight  +
                                 //grdRunValue.RowTemplate.Height * _dtRunValue.Rows.Count;
        }

        private void grdSmp_init()
        {
            _dtSmp.Clear();

            DataColumn name = new DataColumn("name", typeof(string));
            name.ReadOnly = true;

            DataColumn value = new DataColumn("value", typeof(string));

            _dtSmp.Columns.Add(name);
            _dtSmp.Columns.Add(value);

            DataRow dr1 = _dtSmp.NewRow();
            dr1["name"] = "额定电压:";
            dr1["value"] = 0;
            _dtSmp.Rows.Add(dr1);

            DataRow dr2 = _dtSmp.NewRow();
            dr2["name"] = "额定电流:";
            dr2["value"] = 0;
            _dtSmp.Rows.Add(dr2);

            DataRow dr3 = _dtSmp.NewRow();
            dr3["name"] = "额定电流等级:";
            dr3["value"] = 0;
            _dtSmp.Rows.Add(dr3);

            DataRow dr4 = _dtSmp.NewRow();
            dr4["name"] = "工厂代码:";
            dr4["value"] = 0;
            _dtSmp.Rows.Add(dr4);

            DataRow dr5 = _dtSmp.NewRow();
            dr5["name"] = "固件版本号:";
            dr5["value"] = 0;
            _dtSmp.Rows.Add(dr5);


            DataRow dr6 = _dtSmp.NewRow();
            dr6["name"] = "硬件版本号:";
            dr6["value"] = 0;
            _dtSmp.Rows.Add(dr6);

            DataRow dr7 = _dtSmp.NewRow();
            dr7["name"] = "设备型号:";
            dr7["value"] = 0;
            _dtSmp.Rows.Add(dr7);

            grdSmp.DataSource = _dtSmp;

            grdSmp.Columns[0].HeaderText = "名称";
            grdSmp.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grdSmp.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            grdSmp.Columns[1].HeaderText = "返回值";
            grdSmp.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdSmp.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

            grdSmp.AllowUserToResizeColumns = false;
            grdSmp.AllowUserToResizeRows = false;
            grdSmp.AllowUserToOrderColumns = false;
            grdSmp.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grdSmp.Height = grdSmp.ColumnHeadersHeight + 2 +
                            grdSmp.RowTemplate.Height * _dtSmp.Rows.Count;
        }

        private void grdRatedResidual_init()
        {
            _dtRatedResidual.Clear();

            DataColumn name = new DataColumn("name", typeof(string));
            name.ReadOnly = true;

            DataColumn value = new DataColumn("value", typeof(int));

            _dtRatedResidual.Columns.Add(name);
            _dtRatedResidual.Columns.Add(value);

            DataRow dr1 = _dtRatedResidual.NewRow();
            dr1["name"] = "档位1:";
            dr1["value"] = 0;
            _dtRatedResidual.Rows.Add(dr1);

            DataRow dr2 = _dtRatedResidual.NewRow();
            dr2["name"] = "档位2:";
            dr2["value"] = 0;
            _dtRatedResidual.Rows.Add(dr2);

            DataRow dr3 = _dtRatedResidual.NewRow();
            dr3["name"] = "档位3:";
            dr3["value"] = 0;
            _dtRatedResidual.Rows.Add(dr3);

            DataRow dr4 = _dtRatedResidual.NewRow();
            dr4["name"] = "档位4:";
            dr4["value"] = 0;
            _dtRatedResidual.Rows.Add(dr4);

            DataRow dr5 = _dtRatedResidual.NewRow();
            dr5["name"] = "档位5:";
            dr5["value"] = 0;
            _dtRatedResidual.Rows.Add(dr5);


            DataRow dr6 = _dtRatedResidual.NewRow();
            dr6["name"] = "档位6:";
            dr6["value"] = 0;
            _dtRatedResidual.Rows.Add(dr6);

            DataRow dr7 = _dtRatedResidual.NewRow();
            dr7["name"] = "档位7:";
            dr7["value"] = 0;
            _dtRatedResidual.Rows.Add(dr7);

            DataRow dr8 = _dtRatedResidual.NewRow();
            dr8["name"] = "档位8:";
            dr8["value"] = 0;
            _dtRatedResidual.Rows.Add(dr8);

            grdRatedResidual.DataSource = _dtRatedResidual;

            grdRatedResidual.Columns[0].HeaderText = "名称";
            grdRatedResidual.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grdRatedResidual.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            grdRatedResidual.Columns[1].HeaderText = "返回值";
            grdRatedResidual.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdRatedResidual.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

            grdRatedResidual.AllowUserToResizeColumns = false;
            grdRatedResidual.AllowUserToResizeRows = false;
            grdRatedResidual.AllowUserToOrderColumns = false;
            grdRatedResidual.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grdRatedResidual.Height = grdRatedResidual.ColumnHeadersHeight + 2 +
                            grdRatedResidual.RowTemplate.Height * _dtRatedResidual.Rows.Count;
        }

        private void grdTrip_init()
        {
            _dtTrip.Clear();

            DataColumn name = new DataColumn("name", typeof(string));
            name.ReadOnly = true;

            DataColumn value = new DataColumn("value", typeof(string));

            _dtTrip.Columns.Add(name);
            _dtTrip.Columns.Add(value);

            DataRow dr1 = _dtTrip.NewRow();
            dr1["name"] = "故障原因:";
            dr1["value"] = 0;
            _dtTrip.Rows.Add(dr1);

            DataRow dr2 = _dtTrip.NewRow();
            dr2["name"] = "故障相别:";
            dr2["value"] = 0;
            _dtTrip.Rows.Add(dr2);

            DataRow dr3 = _dtTrip.NewRow();
            dr3["name"] = "跳闸发生时刻:";
            dr3["value"] = 0;
            _dtTrip.Rows.Add(dr3);

            DataRow dr4 = _dtTrip.NewRow();
            dr4["name"] = "跳闸前剩余电流:";
            dr4["value"] = 0;
            _dtTrip.Rows.Add(dr4);

            DataRow dr5 = _dtTrip.NewRow();
            dr5["name"] = "跳闸前A相电压:";
            dr5["value"] = 0;
            _dtTrip.Rows.Add(dr5);

            DataRow dr6 = _dtTrip.NewRow();
            dr6["name"] = "跳闸前B相电压:";
            dr6["value"] = 0;
            _dtTrip.Rows.Add(dr6);

            DataRow dr7 = _dtTrip.NewRow();
            dr7["name"] = "跳闸前C相电压:";
            dr7["value"] = 0;
            _dtTrip.Rows.Add(dr7);

            DataRow dr8 = _dtTrip.NewRow();
            dr8["name"] = "跳闸前A相电流:";
            dr8["value"] = 0;
            _dtTrip.Rows.Add(dr8);

            DataRow dr9 = _dtTrip.NewRow();
            dr9["name"] = "跳闸前B相电流:";
            dr9["value"] = 0;
            _dtTrip.Rows.Add(dr9);

            DataRow dr10 = _dtTrip.NewRow();
            dr10["name"] = "跳闸前C相电流:";
            dr10["value"] = 0;
            _dtTrip.Rows.Add(dr10);

            grdTrip.DataSource = _dtTrip;

            grdTrip.Columns[0].HeaderText = "名称";
            grdTrip.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grdTrip.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            grdTrip.Columns[1].HeaderText = "返回值";
            grdTrip.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdTrip.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

            grdTrip.AllowUserToResizeColumns = false;
            grdTrip.AllowUserToResizeRows = false;
            grdTrip.AllowUserToOrderColumns = false;
            grdTrip.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grdTrip.Height = grdTrip.ColumnHeadersHeight + 2 +
                            grdTrip.RowTemplate.Height * _dtTrip.Rows.Count;
        }

        private void grdResidualLimit_init()
        {
            _dtResidualLimit.Clear();

            DataColumn name = new DataColumn("name", typeof(string));
            name.ReadOnly = true;

            DataColumn value = new DataColumn("value", typeof(string));

            _dtResidualLimit.Columns.Add(name);
            _dtResidualLimit.Columns.Add(value);

            DataRow dr1 = _dtResidualLimit.NewRow();
            dr1["name"] = "剩余电流最大相:";
            dr1["value"] = 0;
            _dtResidualLimit.Rows.Add(dr1);

            DataRow dr2 = _dtResidualLimit.NewRow();
            dr2["name"] = "剩余电流值:";
            dr2["value"] = 0;
            _dtResidualLimit.Rows.Add(dr2);

            DataRow dr3 = _dtResidualLimit.NewRow();
            dr3["name"] = "开始时刻:";
            dr3["value"] = 0;
            _dtResidualLimit.Rows.Add(dr3);

            DataRow dr4 = _dtResidualLimit.NewRow();
            dr4["name"] = "结束时刻:";
            dr4["value"] = 0;
            _dtResidualLimit.Rows.Add(dr4);

            grdResidualLimit.DataSource = _dtResidualLimit;

            grdResidualLimit.Columns[0].HeaderText = "名称";
            grdResidualLimit.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grdResidualLimit.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            grdResidualLimit.Columns[1].HeaderText = "返回值";
            grdResidualLimit.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdResidualLimit.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

            grdResidualLimit.AllowUserToResizeColumns = false;
            grdResidualLimit.AllowUserToResizeRows = false;
            grdResidualLimit.AllowUserToOrderColumns = false;
            grdResidualLimit.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grdResidualLimit.Height = grdResidualLimit.ColumnHeadersHeight + 2 +
                            grdResidualLimit.RowTemplate.Height * _dtResidualLimit.Rows.Count;
        }

        private void get_trip()
        {
            Dictionary<string, string> dict = DID();
            CommandMsg dlt = new CommandMsg();

            byte[] _Trip = dlt.readData(_address, (byte)CommandMsg.ControlCode.ReadData, dict["跳闸事件记录"]);
            serialPort1.Write(_Trip, 0, _Trip.Length);

            if (receive_frame(ref _rcvBuf) == true)
            {
                fill_datatable_Trip(ref _rcvBuf);
            }
            else
            {
                stalabMBStatus.Text = "跳闸事件记录读取错误";
            }
        }

        

        private void get_residualLimit()
        {
            Dictionary<string, string> dict = DID();
            CommandMsg dlt = new CommandMsg();

            byte[] _ResidualLimit = dlt.readData(_address, (byte)CommandMsg.ControlCode.ReadData, dict["剩余电流超限事件"]);
            serialPort1.Write(_ResidualLimit, 0, _ResidualLimit.Length);

            if (receive_frame(ref _rcvBuf) == true)
            {
                fill_datatable_ResidualLimit(ref _rcvBuf);
            }
            else
            {
                stalabMBStatus.Text = "剩余电流超限事件读取错误";
            }
        }

        private void timer_Elapsed(object sender, EventArgs e)
        {
            if (_isBusy == false)
            {
                _isBusy = true;

                lock (_lock)
                {
                    try
                    {
                        if (fill_datatable_RunValue())
                        {
                            online_process();
                        }
                    }
                    catch (Exception ex)
                    {
                        offline_process(ex.Message);
                    }
                }

                _isBusy = false;
            }
        }

        private void get_Rated_value()
        {
            get_RatedVoltage();
            get_In();
            get_Inm();
            get_FactoryCode();
            get_FW_Version();
            get_HW_Version();
            get_ProductModel();
            get_trip();
            get_ratedResidual();
            get_residualLimit();
            online_process();
        }

        private void get_RatedVoltage()
        {
            IdentityValue iv = new IdentityValue();
            byte[] tmp = new byte[6];
            for (int i = 0; i < 6; i++)
            {
                tmp[i] = (byte)iv.Device_RatedVoltage(_address, serialPort1)[i];
            }
            _dtSmp.Rows[0][1] = Encoding.ASCII.GetString(tmp);
        }

        private void get_In()
        {
            IdentityValue iv = new IdentityValue();
            byte[] tmp = new byte[6];
            for (int i = 0; i < 6; i++)
            {
                tmp[i] = (byte)iv.In(_address, serialPort1)[i];
            }
            _dtSmp.Rows[1][1] = Encoding.ASCII.GetString(tmp);
        }

        private void get_Inm()
        {
            IdentityValue iv = new IdentityValue();
            byte[] tmp = new byte[6];
            for (int i = 0; i < 6; i++)
            {
                tmp[i] = (byte)iv.Inm(_address, serialPort1)[i];
            }
            _dtSmp.Rows[2][1] = Encoding.ASCII.GetString(tmp);
        }

        private void get_FactoryCode()
        {
            IdentityValue iv = new IdentityValue();
            byte[] tmp = new byte[24];
            for (int i = 0; i < 24; i++)
            {
                tmp[i] = (byte)iv.Device_FactoryCode(_address, serialPort1)[i];
            }
            _dtSmp.Rows[3][1] = Encoding.ASCII.GetString(tmp);
        }

        private void get_FW_Version()
        {
            IdentityValue iv = new IdentityValue();
            byte[] tmp = new byte[32];
            for (int i = 0; i < 32; i++)
            {
                tmp[i] = (byte)iv.Device_FW_Version(_address, serialPort1)[i];
            }
            _dtSmp.Rows[4][1] = Encoding.ASCII.GetString(tmp);
        }

        private void get_HW_Version()
        {
            IdentityValue iv = new IdentityValue();
            byte[] tmp = new byte[32];
            for (int i = 0; i < 32; i++)
            {
                tmp[i] = (byte)iv.Device_HW_Version(_address, serialPort1)[i];
            }
            _dtSmp.Rows[5][1] = Encoding.ASCII.GetString(tmp);
        }

        private void get_ProductModel()
        {
            IdentityValue iv = new IdentityValue();
            byte[] tmp = new byte[10];
            for (int i = 0; i < 10; i++)
            {
                tmp[i] = (byte)iv.Device_ProductModel(_address, serialPort1)[i];
            }
            _dtSmp.Rows[5][1] = Encoding.ASCII.GetString(tmp);
        }

        private void get_ratedResidual()
        {
            IdentityValue iv = new IdentityValue();
            byte[] tmp = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                tmp[i] = (byte)iv.Rated_Residual_Parameter_Group(_address, serialPort1)[i];
                _dtRatedResidual.Rows[i][1] = tmp[i];
            }
        }
        #endregion

        private void offline_process(string msg)
        {
            _isOffline = true;
            ShowIsOffine(this, "CKM55LC 未连接");
            ShowMessage(this, msg);
            btnOpen.Enabled = btnClose.Enabled = btnTest.Enabled = false;
            this.BackColor = Color.Red;
        }

        private void online_process()
        {
            _isOffline = false;
            ShowIsOffine(this, "CKM55LC 已连接");
            ShowMessage(this, "通讯正常");
            btnOpen.Enabled = btnClose.Enabled = btnTest.Enabled = true; ;
            this.BackColor = Color.Green;
        }
        
        private Dictionary<string, string> DID()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            dict.Add("A相电压", "0x02010100");
            dict.Add("B相电压", "0x02010200");
            dict.Add("C相电压", "0x02010300");
            dict.Add("三相电压", "0x0201FF00");
            dict.Add("A相电流", "0x02020100");
            dict.Add("B相电流", "0x02020100");
            dict.Add("C相电流", "0x02020300");
            dict.Add("三相电流", "0x0202FF00");
            dict.Add("剩余电流最大相","0x02900000");
            dict.Add("剩余电流值", "0x02900100");
            dict.Add("剩余电流", "0x0290FF00");
            dict.Add("额定剩余电流值", "0x02910100");
            dict.Add("额定电压", "0x04000404");
            dict.Add("额定电流", "0x04000405");
            dict.Add("额定电流等级", "0x04000406");
            dict.Add("工厂代码", "0x0400040E");
            dict.Add("固件版本号", "0x0400040F");
            dict.Add("硬件版本号", "0x04000410");
            dict.Add("额定剩余电流参数组", "0x04000411");
            dict.Add("跳闸事件记录", "0x038E0001");
            dict.Add("剩余电流超限事件", "0x03880001");
            dict.Add("运行状态字", "0x04000501");
            dict.Add("控制字4", "0x04001004");
            dict.Add("跳闸", "0x06010101");
            dict.Add("合闸", "0x06010201");
            dict.Add("试验", "0x06010301");
            dict.Add("设备号", "0x04000402");
            dict.Add("资产管理编码", "0x04000403");
            dict.Add("生产日期", "0x0400040C");
            dict.Add("设备型号", "0x0400040B");
            dict.Add("通讯地址", "0x04000401");
            
            return dict;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            lock (_lock)
            {
                try
                {
                    Dictionary<string, string> dict = DID();
                    CommandMsg dlt = new CommandMsg();

                    byte[] _cmdOpen = dlt.controlCommand(_address, (byte)CommandMsg.ControlCode.Control, (byte)CommandMsg.DataFieldLength.Control, dict["跳闸"]);

                    serialPort1.Write(_cmdOpen, 0, _cmdOpen.Length);
                    receive_frame(ref _rcvBuf);
                }
                catch (Exception ex)
                {
                    offline_process(ex.Message);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            lock (_lock)
            {
                try
                {
                    Dictionary<string, string> dict = DID();
                    CommandMsg dlt = new CommandMsg();

                    byte[] _cmdClose = dlt.controlCommand(_address, (byte)CommandMsg.ControlCode.Control, (byte)CommandMsg.DataFieldLength.Control, dict["合闸"]);

                    serialPort1.Write(_cmdClose, 0, _cmdClose.Length);
                    receive_frame(ref _rcvBuf);
                }
                catch (Exception ex)
                {
                    offline_process(ex.Message);
                }
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            lock (_lock)
            {
                try
                {
                    Dictionary<string, string> dict = DID();
                    CommandMsg dlt = new CommandMsg();

                    byte[] _cmdTest = dlt.controlCommand(_address, (byte)CommandMsg.ControlCode.Control, (byte)CommandMsg.DataFieldLength.Control, dict["试验"]);

                    serialPort1.Write(_cmdTest, 0, _cmdTest.Length);
                    receive_frame(ref _rcvBuf);
                }
                catch (Exception ex)
                {
                    offline_process(ex.Message);
                }
            }
        }

        private void btnReadDeviceNum_Click(object sender, EventArgs e)
        {
            IdentityValue iv = new IdentityValue();

            byte[] tmp = new byte[6];
            for (int i = 0; i < 6; i++)
            {
                tmp[i] = (byte) iv.Read_Device_Number(_address, serialPort1)[i];
                txtDeviceNum.Text += tmp[i].ToString();
            }
        }

        private void btnReadProductDate_Click(object sender, EventArgs e)
        {
            IdentityValue iv = new IdentityValue();

            byte[] tmp = new byte[10];
            for (int i = 0; i < 10; i++)
            {
                tmp[i] = (byte)iv.Read_Device_ProductDate(_address, serialPort1)[i];
            }
            txtAssetCode.Text = Encoding.ASCII.GetString(tmp);
        }

        private void btnReadAssetCode_Click(object sender, EventArgs e)
        {
           IdentityValue iv = new IdentityValue();

            byte[] tmp = new byte[32];
            for (int i = 0; i < 32; i++)
            {
                tmp[i] = (byte)iv.Read_Device_AssetCode(_address, serialPort1)[i];
            }
           txtAssetCode.Text = Encoding.ASCII.GetString(tmp);
        }

        private void btnWriteDeviceNum_Click(object sender, EventArgs e)
        {
            IdentityValue iv = new IdentityValue();
            byte[] tmp = new byte[6];
            for (int i = 0; i < txtDeviceNum.TextLength; i++)
            {
                tmp[i] = byte.Parse(txtDeviceNum.Text.Substring(i, 1));
            }
            if (iv.Write_Device_Number(_address, serialPort1, tmp))
            {
                stalabMBStatus.Text = "写入设备号成功";
            }
        }

        private void btnWriteProductDate_Click(object sender, EventArgs e)
        {
            IdentityValue iv = new IdentityValue();
            byte[] tmp = Encoding.ASCII.GetBytes(txtProductDate.Text);
            //byte[] array = new byte[10];
            //for (int i = 0; i < tmp.Length; i++)
            //{
            //    array[tmp.Length - 1 - i] = tmp[i];
            //}
            if (iv.Write_Device_ProductDate(_address, serialPort1, tmp))
            {
                stalabMBStatus.Text = "写入生产日期成功";
            }
        }

        private void btnWriteAssetCode_Click(object sender, EventArgs e)
        {
            IdentityValue iv = new IdentityValue();
            byte[] tmp = Encoding.ASCII.GetBytes(txtAssetCode.Text);
            if (iv.Write_Device_AssetCode(_address, serialPort1, tmp))
            {
                stalabMBStatus.Text = "写入资产编码成功";
            }
        }
        
        private void btnReadFactoryValue_Click(object sender, EventArgs e)
        {
            lock (_lock)
            {
                try
                {
                    get_Rated_value();
                }
                catch (Exception ex)
                {
                    offline_process(ex.Message);
                }
            }
        }

        private void btnReadAddr_Click(object sender, EventArgs e)
        {
            lock (_lock)
            {
                try
                {
                    IdentityValue iv = new IdentityValue();
                    for (int i = 0; i < 6; i++)
                    {
                        txtDetect1.Text += iv.Read_Device_Address(serialPort1)[6 - i - 1].ToString();
                    }
                }
                catch (Exception ex)
                {
                    offline_process(ex.Message);
                }
            }
        }

        private void btnSetAddr_Click(object sender, EventArgs e)
        {
            lock (_lock)
            {
                try
                {
                    IdentityValue iv = new IdentityValue();
                    int a, b;
                    byte tmp = byte.Parse(txtDetect1.Text);
                    a = (tmp / 10) << 4;
                    b = (tmp % 10) & 0x0F;
                    _address[0] = (byte)(a + b);
                    if(iv.Write_Device_Address(_address,serialPort1))
                    {
                        stalabMBStatus.Text = "修改通讯地址成功";
                    }
                }
                catch (Exception ex)
                {
                    offline_process(ex.Message);
                }
            }
        }

        private void btnReadBaudrate_Click(object sender, EventArgs e)
        {
            lock (_lock)
            {
                try
                {
                    IdentityValue iv = new IdentityValue();
                    int tmp = iv.Device_BaudRate(_address, serialPort1);
                    if (tmp == 1)
                    {
                        cbxBaudRate.Text = "600";
                    }
                    else if (tmp == 2)
                    {
                        cbxBaudRate.Text = "1200";
                    }
                    else if (tmp == 3)
                    {
                        cbxBaudRate.Text = "2400";
                    }
                    else if (tmp == 4)
                    {
                        cbxBaudRate.Text = "4800";
                    }
                    else if (tmp == 5)
                    {
                        cbxBaudRate.Text = "9600";
                    }
                    else if (tmp == 6)
                    {
                        cbxBaudRate.Text = "19200";
                    }
                }
                catch (Exception ex)
                {
                    offline_process(ex.Message);
                }
            }
        }

        private void btnSetBaudrate_Click(object sender, EventArgs e)
        {
            lock (_lock)
            {
                try
                {
                    IdentityValue iv = new IdentityValue();
                    if (iv.Write_Device_BaudRate(_address, serialPort1, int.Parse(cbxBaudRate.Text)))
                    {
                        stalabMBStatus.Text = "修改通讯速率成功";
                    }
                }
                catch (Exception ex)
                {
                    offline_process(ex.Message);
                }
            }
        }

    }
}
