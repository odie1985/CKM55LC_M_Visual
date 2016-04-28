using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Timers;
using System.Threading;
using Common;
using Lines.Com;
using log4net;
using LinesDevicesManager;
using Devices.MCCB;

namespace Factory_KRAO
{
    public partial class FormCalibration : Form
    {
        /// <summary>
        /// 日志
        /// </summary>
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(FormCalibration));

        /// <summary>
        /// 消息传递事件处理委托
        /// </summary>
        public delegate void DelegateMessage(object sender, string message);

        /// <summary>
        /// 刷新数据表
        /// </summary>
        private DataTable dtSmp = new DataTable();
        private DataTable dtRunValue = new DataTable();
        private DataTable dtCalibrationValue = new DataTable();

        public FormCalibration()
        {
            InitializeComponent();
        }

        private void FormCalibration_Load(object sender, EventArgs e)
        {
            grdSmp_init();
            grdRunValue_init();
            grdCalibrationValue_init();

            device_init();
        }

        private void ShowSetResult(object sender, string message)
        {
            if (this.InvokeRequired)
            {
                DelegateMessage df = new DelegateMessage(ShowSetResult);
                this.BeginInvoke(df, sender, message);
            }
            else
            {
                stalabMBIsOffline.Text = message;
            }
        }

        private void device_init()
        {
            try
            {
                ComHelper.InitComLinesandDevices();

                ComLine cl = ComHelper.GetComLines()[0];

                stalabMBPortName.Text = cl.COM.PortName;
                stalabMBBaudRate.Text = cl.COM.BaudRate.ToString();
                CKM55LC_M ck = (CKM55LC_M)cl.Devices[0];

                ck.SetResult += ShowSetResult;
                cl.AddTimer(ComHelper.GetComConfig()[0].RefreshPeriod, true, ck.FactoryRefresh);
                cl.AddTimer(ComHelper.GetComConfig()[0].RefreshPeriod, true, fill_datatable);

                ck.Online += new BaseDevice.DeviceEventHandler(show_device_online);
                ck.Offline += new BaseDevice.DeviceEventHandler(show_device_offline);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fill_datatable(object sender, ElapsedEventArgs e)
        {
            try
            {
                CKM55LC_M ck = (CKM55LC_M)ComHelper.GetComLines()[0].Devices[0];

                if (ck.IsOffline)
                {
                    return;
                }

                dtSmp.Rows[0][1] = ck.CalibrationValues.Smp_CurrentA;
                dtSmp.Rows[1][1] = ck.CalibrationValues.Smp_CurrentB;
                dtSmp.Rows[2][1] = ck.CalibrationValues.Smp_CurrentC;
                dtSmp.Rows[3][1] = ck.CalibrationValues.Smp_Residual;
                dtSmp.Rows[4][1] = ck.CalibrationValues.Smp_VoltageA;
                dtSmp.Rows[5][1] = ck.CalibrationValues.Smp_VoltageB;
                dtSmp.Rows[6][1] = ck.CalibrationValues.Smp_VoltageC;

                dtRunValue.Rows[0][1] = ck.RunValues.Current_A;
                dtRunValue.Rows[1][1] = ck.RunValues.Current_B;
                dtRunValue.Rows[2][1] = ck.RunValues.Current_C;
                dtRunValue.Rows[3][1] = ck.RunValues.Current_Residual;
                dtRunValue.Rows[4][1] = ck.RunValues.Ratio_Current_Imbalance;

                dtRunValue.Rows[5][1] = ck.RunValues.Voltage_A;
                dtRunValue.Rows[6][1] = ck.RunValues.Voltage_B;
                dtRunValue.Rows[7][1] = ck.RunValues.Voltage_C;
                dtRunValue.Rows[8][1] = ck.RunValues.Temperature;

                dtRunValue.Rows[9][1] = ck.MonitorValues.Status_DO;
                dtRunValue.Rows[10][1] = ck.MonitorValues.Status_MC;

                dtCalibrationValue.Rows[0][1] = ck.CalibrationValues.CurrentA_K;
                dtCalibrationValue.Rows[1][1] = ck.CalibrationValues.CurrentA_B;
                dtCalibrationValue.Rows[2][1] = ck.CalibrationValues.CurrentB_K;
                dtCalibrationValue.Rows[3][1] = ck.CalibrationValues.CurrentB_B;
                dtCalibrationValue.Rows[4][1] = ck.CalibrationValues.CurrentC_K;
                dtCalibrationValue.Rows[5][1] = ck.CalibrationValues.CurrentC_B;
                dtCalibrationValue.Rows[6][1] = ck.CalibrationValues.Residual_K;
                dtCalibrationValue.Rows[7][1] = ck.CalibrationValues.Residual_B;
                dtCalibrationValue.Rows[8][1] = ck.CalibrationValues.VoltageA_K;
                dtCalibrationValue.Rows[9][1] = ck.CalibrationValues.VoltageA_B;
                dtCalibrationValue.Rows[10][1] = ck.CalibrationValues.VoltageB_K;
                dtCalibrationValue.Rows[11][1] = ck.CalibrationValues.VoltageB_B;
                dtCalibrationValue.Rows[12][1] = ck.CalibrationValues.VoltageC_K;
                dtCalibrationValue.Rows[13][1] = ck.CalibrationValues.VoltageC_B;
            }
            catch (System.Exception)
            {
                stalabMBStatus.Text = "fill_datatable error";
            }
        }

        private void grdSmp_init()
        {
            dtSmp.Clear();

            DataColumn name = new DataColumn("name", typeof(string));
            name.ReadOnly = true;

            DataColumn value = new DataColumn("value", typeof(int));

            dtSmp.Columns.Add(name);
            dtSmp.Columns.Add(value);

            DataRow dr1 = dtSmp.NewRow();
            dr1["name"] = "A相电流:";
            dr1["value"] = 0;
            dtSmp.Rows.Add(dr1);

            DataRow dr2 = dtSmp.NewRow();
            dr2["name"] = "B相电流:";
            dr2["value"] = 0;
            dtSmp.Rows.Add(dr2);

            DataRow dr3 = dtSmp.NewRow();
            dr3["name"] = "C相电流:";
            dr3["value"] = 0;
            dtSmp.Rows.Add(dr3);

            DataRow dr4 = dtSmp.NewRow();
            dr4["name"] = "剩余电流:";
            dr4["value"] = 0;
            dtSmp.Rows.Add(dr4);

            DataRow dr5 = dtSmp.NewRow();
            dr5["name"] = "A相电压:";
            dr5["value"] = 0;
            dtSmp.Rows.Add(dr5);


            DataRow dr6 = dtSmp.NewRow();
            dr6["name"] = "B相电压:";
            dr6["value"] = 0;
            dtSmp.Rows.Add(dr6);

            DataRow dr7 = dtSmp.NewRow();
            dr7["name"] = "C相电压:";
            dr7["value"] = 0;
            dtSmp.Rows.Add(dr7);

            grdSmp.DataSource = dtSmp;

            grdSmp.Columns[0].HeaderText = "名称";
            grdSmp.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grdSmp.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            grdSmp.Columns[1].HeaderText = "采样值";
            grdSmp.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdSmp.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

            grdSmp.AllowUserToResizeColumns = false;
            grdSmp.AllowUserToResizeRows = false;
            grdSmp.AllowUserToOrderColumns = false;
            grdSmp.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grdSmp.Height = grdSmp.ColumnHeadersHeight + 2 +
                            grdSmp.RowTemplate.Height * dtSmp.Rows.Count;
        }

        private void grdRunValue_init()
        {
            dtRunValue.Clear();

            DataColumn name = new DataColumn("name", typeof(string));
            name.ReadOnly = true;

            DataColumn value = new DataColumn("value", typeof(float));

            dtRunValue.Columns.Add(name);
            dtRunValue.Columns.Add(value);

            DataRow dr1 = dtRunValue.NewRow();
            dr1["name"] = "A相电流(A):";
            dr1["value"] = 0;
            dtRunValue.Rows.Add(dr1);

            DataRow dr2 = dtRunValue.NewRow();
            dr2["name"] = "B相电流(A):";
            dr2["value"] = 0;
            dtRunValue.Rows.Add(dr2);

            DataRow dr3 = dtRunValue.NewRow();
            dr3["name"] = "C相电流(A):";
            dr3["value"] = 0;
            dtRunValue.Rows.Add(dr3);

            DataRow dr4 = dtRunValue.NewRow();
            dr4["name"] = "剩余电流(mA):";
            dr4["value"] = 0;
            dtRunValue.Rows.Add(dr4);

            DataRow dr5 = dtRunValue.NewRow();
            dr5["name"] = "不平衡电流(%):";
            dr5["value"] = 0;
            dtRunValue.Rows.Add(dr5);

            DataRow dr6 = dtRunValue.NewRow();
            dr6["name"] = "A相电压(V):";
            dr6["value"] = 0;
            dtRunValue.Rows.Add(dr6);

            DataRow dr7 = dtRunValue.NewRow();
            dr7["name"] = "B相电压(V):";
            dr7["value"] = 0;
            dtRunValue.Rows.Add(dr7);

            DataRow dr8 = dtRunValue.NewRow();
            dr8["name"] = "C相电压(V):";
            dr8["value"] = 0;
            dtRunValue.Rows.Add(dr8);

            DataRow dr9 = dtRunValue.NewRow();
            dr9["name"] = "温度(℃):";
            dr9["value"] = 0;
            dtRunValue.Rows.Add(dr9);

            DataRow dr10 = dtRunValue.NewRow();
            dr10["name"] = "DO:";
            dr10["value"] = 0;
            dtRunValue.Rows.Add(dr10);

            DataRow dr11 = dtRunValue.NewRow();
            dr11["name"] = "主触头:";
            dr11["value"] = 0;
            dtRunValue.Rows.Add(dr11);

            grdRunValue.DataSource = dtRunValue;

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
            grdRunValue.Height = grdRunValue.ColumnHeadersHeight + 2 +
                                 grdRunValue.RowTemplate.Height * dtRunValue.Rows.Count;
        }

        private void grdCalibrationValue_init()
        {
            dtCalibrationValue.Clear();

            DataColumn name = new DataColumn("name", typeof(string));
            name.ReadOnly = true;

            DataColumn value = new DataColumn("value", typeof(short));

            dtCalibrationValue.Columns.Add(name);
            dtCalibrationValue.Columns.Add(value);

            DataRow dr1 = dtCalibrationValue.NewRow();
            dr1["name"] = "A相电流K:";
            dr1["value"] = 0;
            dtCalibrationValue.Rows.Add(dr1);

            DataRow dr2 = dtCalibrationValue.NewRow();
            dr2["name"] = "A相电流B:";
            dr2["value"] = 0;
            dtCalibrationValue.Rows.Add(dr2);

            DataRow dr3 = dtCalibrationValue.NewRow();
            dr3["name"] = "B相电流K:";
            dr3["value"] = 0;
            dtCalibrationValue.Rows.Add(dr3);

            DataRow dr4 = dtCalibrationValue.NewRow();
            dr4["name"] = "B相电流B:";
            dr4["value"] = 0;
            dtCalibrationValue.Rows.Add(dr4);

            DataRow dr5 = dtCalibrationValue.NewRow();
            dr5["name"] = "C相电流K:";
            dr5["value"] = 0;
            dtCalibrationValue.Rows.Add(dr5);

            DataRow dr6 = dtCalibrationValue.NewRow();
            dr6["name"] = "C相电流B:";
            dr6["value"] = 0;
            dtCalibrationValue.Rows.Add(dr6);

            DataRow dr7 = dtCalibrationValue.NewRow();
            dr7["name"] = "剩余电流K:";
            dr7["value"] = 0;
            dtCalibrationValue.Rows.Add(dr7);

            DataRow dr8 = dtCalibrationValue.NewRow();
            dr8["name"] = "剩余电流B:";
            dr8["value"] = 0;
            dtCalibrationValue.Rows.Add(dr8);

            DataRow dr9 = dtCalibrationValue.NewRow();
            dr9["name"] = "A相电压K:";
            dr9["value"] = 0;
            dtCalibrationValue.Rows.Add(dr9);

            DataRow dr10 = dtCalibrationValue.NewRow();
            dr10["name"] = "A相电压B:";
            dr10["value"] = 0;
            dtCalibrationValue.Rows.Add(dr10);

            DataRow dr11 = dtCalibrationValue.NewRow();
            dr11["name"] = "B相电压K:";
            dr11["value"] = 0;
            dtCalibrationValue.Rows.Add(dr11);

            DataRow dr12 = dtCalibrationValue.NewRow();
            dr12["name"] = "B相电压B:";
            dr12["value"] = 0;
            dtCalibrationValue.Rows.Add(dr12);

            DataRow dr13 = dtCalibrationValue.NewRow();
            dr13["name"] = "C相电压K:";
            dr13["value"] = 0;
            dtCalibrationValue.Rows.Add(dr13);

            DataRow dr14 = dtCalibrationValue.NewRow();
            dr14["name"] = "C相电压B:";
            dr14["value"] = 0;
            dtCalibrationValue.Rows.Add(dr14);

            grdCalibrationValue.DataSource = dtCalibrationValue;

            grdCalibrationValue.Columns[0].HeaderText = "名称";
            grdCalibrationValue.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grdCalibrationValue.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;

            grdCalibrationValue.Columns[1].HeaderText = "函数值";
            grdCalibrationValue.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdCalibrationValue.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

            grdCalibrationValue.AllowUserToResizeColumns = false;
            grdCalibrationValue.AllowUserToResizeRows = false;
            grdCalibrationValue.AllowUserToOrderColumns = false;
            grdCalibrationValue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grdCalibrationValue.Height = grdCalibrationValue.ColumnHeadersHeight + 2 +
                                         grdCalibrationValue.RowTemplate.Height * dtCalibrationValue.Rows.Count;

        }

        private void show_device_online(object sender, string message)
        {
            if (this.InvokeRequired)
            {
                DelegateMessage df = new DelegateMessage(show_device_online);
                this.BeginInvoke(df, sender, message);
            }
            else
            {
                CKM55LC_M ck = (CKM55LC_M)ComHelper.GetComLines()[0].Devices[0];
                stalabMBStatus.Text = message;
                stalabMBIsOffline.Text = "QKM3L已连接";
                gbxCurrent.Enabled = true;
                gbxProtect.Enabled = true;
                gbxIdentity.Enabled = true;

                try
                {
                    int i = 0;

                    while (ck.IdentityValues.In == 0)
                    {
                        Thread.Sleep(100);

                        if (++i >= 50)
                        {
                            throw new Exception();
                        }
                    }
                
                    cmbFullload.Text = ck.IdentityValues.In.ToString() + "A";
                }
                catch (System.Exception)
                {
                    MessageBox.Show("额定电流规格读取错误");
                }

            }
        }

        private void show_device_offline(object sender, string message)
        {
            if (this.InvokeRequired)
            {
                DelegateMessage df = new DelegateMessage(show_device_offline);
                this.BeginInvoke(df, sender, message);
            }
            else
            {
                stalabMBStatus.Text = message;
                stalabMBIsOffline.Text = "QKM3L已断开";
                cmbFullload.SelectedIndex = -1;
                gbxCurrent.Enabled = false;
                gbxProtect.Enabled = false;
                gbxIdentity.Enabled = false;
            }
        }

        private void btnSetFullload_Click(object sender, EventArgs e)
        {
            CKM55LC_M ck = (CKM55LC_M)ComHelper.GetComLines()[0].Devices[0];
            string sc = cmbFullload.SelectedItem.ToString();
            if (sc != "")
            {
                if (MessageBox.Show("是否确认修改额定电流规格？", "额定电流规格修改", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    ck.IdentityValues.In = ushort.Parse(sc.Substring(0, 3));
                }
                else
                {
                    try
                    {
                        int i = 0;

                        while (ck.IdentityValues.In == 0)
                        {
                            Thread.Sleep(100);

                            if (++i >= 50)
                            {
                                throw new Exception();
                            }
                        }

                        cmbFullload.Text = ck.IdentityValues.In.ToString() + "A";
                    }
                    catch (System.Exception)
                    {
                        MessageBox.Show("额定电流规格读取错误");
                    }
                }
            }
            else
            {
                MessageBox.Show("请选择额定电流规格");
            }
        }

        private void btnCalCurrent_Click(object sender, EventArgs e)
        {
            FormCalCurrent frmCalCurrent = new FormCalCurrent();
            frmCalCurrent.ShowDialog();
        }

        private void btnCalResidual_Click(object sender, EventArgs e)
        {
            FormCalResidual frmCalResidual = new FormCalResidual();
            frmCalResidual.ShowDialog();
        }

        private void btnProtectDisable_Click(object sender, EventArgs e)
        {
            CKM55LC_M ck = (CKM55LC_M)ComHelper.GetComLines()[0].Devices[0];
            List<string> lst = new List<string>();
            ck.ProtectSets.Protect_Enable = lst;
        }

        private void btnProtectEnable_Click(object sender, EventArgs e)
        {
            CKM55LC_M ck = (CKM55LC_M)ComHelper.GetComLines()[0].Devices[0];
            List<string> lst = new List<string>();
            lst.Add("过流故障");
            lst.Add("过载故障");
            ck.ProtectSets.Protect_Enable = lst;
        }

        private void btnCalVoltage_Click(object sender, EventArgs e)
        {
            FormCalVoltage frmCalVoltage = new FormCalVoltage();
            frmCalVoltage.ShowDialog();
        }

        private void gbxIndentity_EnabledChanged(object sender, EventArgs e)
        {
            if (gbxIdentity.Enabled == true)
            {
                CKM55LC_M ck = (CKM55LC_M)ComHelper.GetComLines()[0].Devices[0];
                ck.IdentityValues.GetRegisters((int)Devices.MCCB.Timeout.Never);
                labProfile.Text = ck.IdentityValues.Device_ProductModel;
                labManufacture.Text = ck.IdentityValues.Device_Manufacture;
                labHW.Text = ck.IdentityValues.Device_HW_Version;
                labSW.Text = ck.IdentityValues.Device_SW_Version;
                txtPN.Text = ck.IdentityValues.Device_ProductNumber;
            }
        }

        private void btnSetPN_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认修改产品编号？", "产品编号修改", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                CKM55LC_M ck = (CKM55LC_M)ComHelper.GetComLines()[0].Devices[0];
                txtPN.Text = ck.IdentityValues.Device_ProductNumber = txtPN.Text;
            }
        }

    }
}
