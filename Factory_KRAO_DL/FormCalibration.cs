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
        /// ��Ϣ�����¼�����ί��
        /// </summary>
        public delegate void DelegateMessage(object sender, string message);

        /// <summary>
        /// ˢ�����ݱ�
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
        /// ��ʼ���˿�
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
                this.stalabMBStatus.Text = "COMδ����";
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
        /// ���Ӵ���
        /// </summary>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            lock (_lock)
            {
                try
                {
                    if (btnConnect.Text == "�رմ���") //�رմ���
                    {
                        btnConnect.Text = "���Ӵ���";
                        serialPort1.DiscardOutBuffer();
                        serialPort1.DiscardInBuffer();
                        if (serialPort1.IsOpen == true)
                        {
                            serialPort1.Close();
                        }
                        stalabMBStatus.Text = "����δ����";
                    }
                    else if (btnConnect.Text == "���Ӵ���") //�򿪴���
                    {
                        serialPort1.PortName = cbxCom.Text;
                        serialPort1.BaudRate = int.Parse(cbxBaudRate.Text);
                        serialPort1.Parity = System.IO.Ports.Parity.Even; //żУ��
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
                        
                        btnConnect.Text = "�رմ���";
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

        #region ��ȡ����

        /// <summary>
        /// ��ȡ���в���
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
        /// ��������բ�¼���¼
        /// </summary>
        /// <param name="rcv"></param>
        private void fill_datatable_Trip(ref byte[] rcv)
        {
            try
            {
                string str = "";
                //����ԭ��
                _dtTrip.Rows[0][1] = _dtRunValue.Rows[10][1];

                //�������
                _dtTrip.Rows[1][1] = rcv[15].ToString("X2");  

                //��բ����ʱ��
                for (int i = 0; i < 6; i++)
                {
                    str += rcv[16 + i].ToString("X2");
                }
                _dtTrip.Rows[2][1] = str;

                //��բǰʣ�����
                str = rcv[22].ToString("X2");  
                str += rcv[23].ToString("X2");
                _dtTrip.Rows[3][1] = str;

                //��բǰA���ѹ
                str = rcv[24].ToString("X2");
                str += rcv[25].ToString("X2");
                _dtTrip.Rows[4][1] = str;

                //��բǰB���ѹ
                str = rcv[26].ToString("X2");
                str += rcv[27].ToString("X2");
                _dtTrip.Rows[5][1] = str;

                //��բǰC���ѹ
                str = rcv[28].ToString("X2");
                str += rcv[29].ToString("X2");
                _dtTrip.Rows[6][1] = str;

                // ��բǰA�����
                str = rcv[30].ToString("X2");
                str += rcv[31].ToString("X2");
                str += rcv[32].ToString("X2");
                _dtTrip.Rows[7][1] = str;

                // ��բǰB�����
                str = rcv[33].ToString("X2");
                str += rcv[34].ToString("X2");
                str += rcv[35].ToString("X2");
                _dtTrip.Rows[8][1] = str;

                // ��բǰB�����
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
        /// ʣ����������¼���¼
        /// </summary>
        /// <param name="rcv"></param>
        private void fill_datatable_ResidualLimit(ref byte[] rcv)
        {
            try
            {
                string str = "";
                //ʣ����������
                _dtResidualLimit.Rows[0][1] = rcv[14].ToString("X2");  

                //ʣ�����ֵ
                str = rcv[15].ToString("X2");
                str = str + rcv[16].ToString("X2");  
                _dtResidualLimit.Rows[1][1] = str;
                
                //��ʼʱ��
                str = "";
                for (int i = 0; i < 6; i++)
                {
                    str += rcv[16 + i].ToString("X2");
                }
                _dtResidualLimit.Rows[2][1] = str;

                //��������ʱ��
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
            dr1["name"] = "A�����(A):";
            dr1["value"] = 0;
            _dtRunValue.Rows.Add(dr1);

            DataRow dr2 = _dtRunValue.NewRow();
            dr2["name"] = "B�����(A):";
            dr2["value"] = 0;
            _dtRunValue.Rows.Add(dr2);

            DataRow dr3 = _dtRunValue.NewRow();
            dr3["name"] = "C�����(A):";
            dr3["value"] = 0;
            _dtRunValue.Rows.Add(dr3);

            DataRow dr4 = _dtRunValue.NewRow();
            dr4["name"] = "ʣ����������:";
            dr4["value"] = 0;
            _dtRunValue.Rows.Add(dr4);

            DataRow dr5 = _dtRunValue.NewRow();
            dr5["name"] = "ʣ�����(mA):";
            dr5["value"] = 0;
            _dtRunValue.Rows.Add(dr5);

            DataRow dr6 = _dtRunValue.NewRow();
            dr6["name"] = "A���ѹ(V):";
            dr6["value"] = 0;
            _dtRunValue.Rows.Add(dr6);

            DataRow dr7 = _dtRunValue.NewRow();
            dr7["name"] = "B���ѹ(V):";
            dr7["value"] = 0;
            _dtRunValue.Rows.Add(dr7);

            DataRow dr8 = _dtRunValue.NewRow();
            dr8["name"] = "C���ѹ(V):";
            dr8["value"] = 0;
            _dtRunValue.Rows.Add(dr8);

            DataRow dr9 = _dtRunValue.NewRow();
            dr9["name"] = "�澯:";
            dr9["value"] = 0;
            _dtRunValue.Rows.Add(dr9);

            DataRow dr10 = _dtRunValue.NewRow();
            dr10["name"] = "״̬:";
            dr10["value"] = 0;
            _dtRunValue.Rows.Add(dr10);

            DataRow dr11 = _dtRunValue.NewRow();
            dr11["name"] = "ԭ��:";
            dr11["value"] = 0;
            _dtRunValue.Rows.Add(dr11);

            DataRow dr12 = _dtRunValue.NewRow();
            dr12["name"] = "�ʣ���������ֵ:";
            dr12["value"] = 0;
            _dtRunValue.Rows.Add(dr12);

            DataRow dr13 = _dtRunValue.NewRow();
            dr13["name"] = "����޲�����ʱ��:";
            dr13["value"] = 0;
            _dtRunValue.Rows.Add(dr13);

            DataRow dr14 = _dtRunValue.NewRow();
            dr14["name"] = "ʣ���������ʱ��:";
            dr14["value"] = 0;
            _dtRunValue.Rows.Add(dr14);

            grdRunValue.DataSource = _dtRunValue;

            grdRunValue.Columns[0].HeaderText = "����";
            grdRunValue.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grdRunValue.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            grdRunValue.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            grdRunValue.Columns[1].HeaderText = "ʵ��ֵ";
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
            dr1["name"] = "���ѹ:";
            dr1["value"] = 0;
            _dtSmp.Rows.Add(dr1);

            DataRow dr2 = _dtSmp.NewRow();
            dr2["name"] = "�����:";
            dr2["value"] = 0;
            _dtSmp.Rows.Add(dr2);

            DataRow dr3 = _dtSmp.NewRow();
            dr3["name"] = "������ȼ�:";
            dr3["value"] = 0;
            _dtSmp.Rows.Add(dr3);

            DataRow dr4 = _dtSmp.NewRow();
            dr4["name"] = "��������:";
            dr4["value"] = 0;
            _dtSmp.Rows.Add(dr4);

            DataRow dr5 = _dtSmp.NewRow();
            dr5["name"] = "�̼��汾��:";
            dr5["value"] = 0;
            _dtSmp.Rows.Add(dr5);


            DataRow dr6 = _dtSmp.NewRow();
            dr6["name"] = "Ӳ���汾��:";
            dr6["value"] = 0;
            _dtSmp.Rows.Add(dr6);

            DataRow dr7 = _dtSmp.NewRow();
            dr7["name"] = "�豸�ͺ�:";
            dr7["value"] = 0;
            _dtSmp.Rows.Add(dr7);

            grdSmp.DataSource = _dtSmp;

            grdSmp.Columns[0].HeaderText = "����";
            grdSmp.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grdSmp.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            grdSmp.Columns[1].HeaderText = "����ֵ";
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
            dr1["name"] = "��λ1:";
            dr1["value"] = 0;
            _dtRatedResidual.Rows.Add(dr1);

            DataRow dr2 = _dtRatedResidual.NewRow();
            dr2["name"] = "��λ2:";
            dr2["value"] = 0;
            _dtRatedResidual.Rows.Add(dr2);

            DataRow dr3 = _dtRatedResidual.NewRow();
            dr3["name"] = "��λ3:";
            dr3["value"] = 0;
            _dtRatedResidual.Rows.Add(dr3);

            DataRow dr4 = _dtRatedResidual.NewRow();
            dr4["name"] = "��λ4:";
            dr4["value"] = 0;
            _dtRatedResidual.Rows.Add(dr4);

            DataRow dr5 = _dtRatedResidual.NewRow();
            dr5["name"] = "��λ5:";
            dr5["value"] = 0;
            _dtRatedResidual.Rows.Add(dr5);


            DataRow dr6 = _dtRatedResidual.NewRow();
            dr6["name"] = "��λ6:";
            dr6["value"] = 0;
            _dtRatedResidual.Rows.Add(dr6);

            DataRow dr7 = _dtRatedResidual.NewRow();
            dr7["name"] = "��λ7:";
            dr7["value"] = 0;
            _dtRatedResidual.Rows.Add(dr7);

            DataRow dr8 = _dtRatedResidual.NewRow();
            dr8["name"] = "��λ8:";
            dr8["value"] = 0;
            _dtRatedResidual.Rows.Add(dr8);

            grdRatedResidual.DataSource = _dtRatedResidual;

            grdRatedResidual.Columns[0].HeaderText = "����";
            grdRatedResidual.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grdRatedResidual.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            grdRatedResidual.Columns[1].HeaderText = "����ֵ";
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
            dr1["name"] = "����ԭ��:";
            dr1["value"] = 0;
            _dtTrip.Rows.Add(dr1);

            DataRow dr2 = _dtTrip.NewRow();
            dr2["name"] = "�������:";
            dr2["value"] = 0;
            _dtTrip.Rows.Add(dr2);

            DataRow dr3 = _dtTrip.NewRow();
            dr3["name"] = "��բ����ʱ��:";
            dr3["value"] = 0;
            _dtTrip.Rows.Add(dr3);

            DataRow dr4 = _dtTrip.NewRow();
            dr4["name"] = "��բǰʣ�����:";
            dr4["value"] = 0;
            _dtTrip.Rows.Add(dr4);

            DataRow dr5 = _dtTrip.NewRow();
            dr5["name"] = "��բǰA���ѹ:";
            dr5["value"] = 0;
            _dtTrip.Rows.Add(dr5);

            DataRow dr6 = _dtTrip.NewRow();
            dr6["name"] = "��բǰB���ѹ:";
            dr6["value"] = 0;
            _dtTrip.Rows.Add(dr6);

            DataRow dr7 = _dtTrip.NewRow();
            dr7["name"] = "��բǰC���ѹ:";
            dr7["value"] = 0;
            _dtTrip.Rows.Add(dr7);

            DataRow dr8 = _dtTrip.NewRow();
            dr8["name"] = "��բǰA�����:";
            dr8["value"] = 0;
            _dtTrip.Rows.Add(dr8);

            DataRow dr9 = _dtTrip.NewRow();
            dr9["name"] = "��բǰB�����:";
            dr9["value"] = 0;
            _dtTrip.Rows.Add(dr9);

            DataRow dr10 = _dtTrip.NewRow();
            dr10["name"] = "��բǰC�����:";
            dr10["value"] = 0;
            _dtTrip.Rows.Add(dr10);

            grdTrip.DataSource = _dtTrip;

            grdTrip.Columns[0].HeaderText = "����";
            grdTrip.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grdTrip.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            grdTrip.Columns[1].HeaderText = "����ֵ";
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
            dr1["name"] = "ʣ����������:";
            dr1["value"] = 0;
            _dtResidualLimit.Rows.Add(dr1);

            DataRow dr2 = _dtResidualLimit.NewRow();
            dr2["name"] = "ʣ�����ֵ:";
            dr2["value"] = 0;
            _dtResidualLimit.Rows.Add(dr2);

            DataRow dr3 = _dtResidualLimit.NewRow();
            dr3["name"] = "��ʼʱ��:";
            dr3["value"] = 0;
            _dtResidualLimit.Rows.Add(dr3);

            DataRow dr4 = _dtResidualLimit.NewRow();
            dr4["name"] = "����ʱ��:";
            dr4["value"] = 0;
            _dtResidualLimit.Rows.Add(dr4);

            grdResidualLimit.DataSource = _dtResidualLimit;

            grdResidualLimit.Columns[0].HeaderText = "����";
            grdResidualLimit.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grdResidualLimit.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            grdResidualLimit.Columns[1].HeaderText = "����ֵ";
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

            byte[] _Trip = dlt.readData(_address, (byte)CommandMsg.ControlCode.ReadData, dict["��բ�¼���¼"]);
            serialPort1.Write(_Trip, 0, _Trip.Length);

            if (receive_frame(ref _rcvBuf) == true)
            {
                fill_datatable_Trip(ref _rcvBuf);
            }
            else
            {
                stalabMBStatus.Text = "��բ�¼���¼��ȡ����";
            }
        }

        

        private void get_residualLimit()
        {
            Dictionary<string, string> dict = DID();
            CommandMsg dlt = new CommandMsg();

            byte[] _ResidualLimit = dlt.readData(_address, (byte)CommandMsg.ControlCode.ReadData, dict["ʣ����������¼�"]);
            serialPort1.Write(_ResidualLimit, 0, _ResidualLimit.Length);

            if (receive_frame(ref _rcvBuf) == true)
            {
                fill_datatable_ResidualLimit(ref _rcvBuf);
            }
            else
            {
                stalabMBStatus.Text = "ʣ����������¼���ȡ����";
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
            ShowIsOffine(this, "CKM55LC δ����");
            ShowMessage(this, msg);
            btnOpen.Enabled = btnClose.Enabled = btnTest.Enabled = false;
            this.BackColor = Color.Red;
        }

        private void online_process()
        {
            _isOffline = false;
            ShowIsOffine(this, "CKM55LC ������");
            ShowMessage(this, "ͨѶ����");
            btnOpen.Enabled = btnClose.Enabled = btnTest.Enabled = true; ;
            this.BackColor = Color.Green;
        }
        
        private Dictionary<string, string> DID()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            dict.Add("A���ѹ", "0x02010100");
            dict.Add("B���ѹ", "0x02010200");
            dict.Add("C���ѹ", "0x02010300");
            dict.Add("�����ѹ", "0x0201FF00");
            dict.Add("A�����", "0x02020100");
            dict.Add("B�����", "0x02020100");
            dict.Add("C�����", "0x02020300");
            dict.Add("�������", "0x0202FF00");
            dict.Add("ʣ����������","0x02900000");
            dict.Add("ʣ�����ֵ", "0x02900100");
            dict.Add("ʣ�����", "0x0290FF00");
            dict.Add("�ʣ�����ֵ", "0x02910100");
            dict.Add("���ѹ", "0x04000404");
            dict.Add("�����", "0x04000405");
            dict.Add("������ȼ�", "0x04000406");
            dict.Add("��������", "0x0400040E");
            dict.Add("�̼��汾��", "0x0400040F");
            dict.Add("Ӳ���汾��", "0x04000410");
            dict.Add("�ʣ�����������", "0x04000411");
            dict.Add("��բ�¼���¼", "0x038E0001");
            dict.Add("ʣ����������¼�", "0x03880001");
            dict.Add("����״̬��", "0x04000501");
            dict.Add("������4", "0x04001004");
            dict.Add("��բ", "0x06010101");
            dict.Add("��բ", "0x06010201");
            dict.Add("����", "0x06010301");
            dict.Add("�豸��", "0x04000402");
            dict.Add("�ʲ��������", "0x04000403");
            dict.Add("��������", "0x0400040C");
            dict.Add("�豸�ͺ�", "0x0400040B");
            dict.Add("ͨѶ��ַ", "0x04000401");
            
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

                    byte[] _cmdOpen = dlt.controlCommand(_address, (byte)CommandMsg.ControlCode.Control, (byte)CommandMsg.DataFieldLength.Control, dict["��բ"]);

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

                    byte[] _cmdClose = dlt.controlCommand(_address, (byte)CommandMsg.ControlCode.Control, (byte)CommandMsg.DataFieldLength.Control, dict["��բ"]);

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

                    byte[] _cmdTest = dlt.controlCommand(_address, (byte)CommandMsg.ControlCode.Control, (byte)CommandMsg.DataFieldLength.Control, dict["����"]);

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
                stalabMBStatus.Text = "д���豸�ųɹ�";
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
                stalabMBStatus.Text = "д���������ڳɹ�";
            }
        }

        private void btnWriteAssetCode_Click(object sender, EventArgs e)
        {
            IdentityValue iv = new IdentityValue();
            byte[] tmp = Encoding.ASCII.GetBytes(txtAssetCode.Text);
            if (iv.Write_Device_AssetCode(_address, serialPort1, tmp))
            {
                stalabMBStatus.Text = "д���ʲ�����ɹ�";
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
                        stalabMBStatus.Text = "�޸�ͨѶ��ַ�ɹ�";
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
                        stalabMBStatus.Text = "�޸�ͨѶ���ʳɹ�";
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
