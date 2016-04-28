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
using Common;
using Lines.Com;
using Devices.MCCB;
using log4net;
using LinesDevicesManager;
using System.Threading;

namespace Factory_KRAO
{
    public partial class FormCalVoltage : Form
    {
        /// <summary>
        /// 日志
        /// </summary>
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(FormCalVoltage));

        /// <summary>
        /// 消息传递事件处理委托
        /// </summary>
        public delegate void DelegateMessage(object sender, string message);

        // 函数 y * 1024 = (kx + b) * 1024
        private class Fun
        {
            public double x1 = 0;
            public double y1 = 0;
            public double x2 = 0;
            public double y2 = 0;
            public double k = 0;
            public double b = 0;
        }

        private Fun A = new Fun();
        private Fun B = new Fun();
        private Fun C = new Fun();

        private bool retry = false;

        public FormCalVoltage()
        {
            InitializeComponent();
        }

        private void FormCalResidual_Load(object sender, EventArgs e)
        {
            btnStep1.Enabled = true;
            btnStep2.Enabled = false;
            btnStep3.Enabled = false;
            btnStep4.Enabled = false;
            lblbStep5.Text = "";
        }

        private void btnStep1_Click(object sender, EventArgs e)
        {
            try
            {
                A.y1 = double.Parse(txtY1.Text);
                B.y1 = A.y1;
                C.y1 = A.y1;
            }
            catch (System.Exception)
            {
                MessageBox.Show("错误：电压输入错误!");
                return;
            }

            CKM55LC_M ck = (CKM55LC_M)ComHelper.GetComLines()[0].Devices[0];

            int sumA = 0;
            int sumB = 0;
            int sumC = 0;
            for (int i = 0; i < 4; i++)
            {
                Thread.Sleep(500);      // wait for refresh device
                sumA += ck.CalibrationValues.Smp_VoltageA;
                sumB += ck.CalibrationValues.Smp_VoltageB;
                sumC += ck.CalibrationValues.Smp_VoltageC;
            }

            A.x1 = sumA >> 2;
            B.x1 = sumB >> 2;
            C.x1 = sumC >> 2;

            txtSmp1.Text = string.Format("A={0},B={1},C={2}", A.x1, B.x1, C.x1);

            btnStep2.Enabled = true;
            btnStep1.Enabled = false;
        }

        private void btnStep2_Click(object sender, EventArgs e)
        {
            btnStep3.Enabled = true;
            btnStep2.Enabled = false;
        }

        private void btnStep3_Click(object sender, EventArgs e)
        {
            try
            {
                A.y2 = double.Parse(txtY2.Text);
                B.y2 = A.y2;
                C.y2 = A.y2;
            }
            catch (System.Exception)
            {
                MessageBox.Show("错误：电压输入错误!");
                return;
            }

            CKM55LC_M ck = (CKM55LC_M)ComHelper.GetComLines()[0].Devices[0];

            int sumA = 0;
            int sumB = 0;
            int sumC = 0;
            for (int i = 0; i < 4; i++)
            {
                Thread.Sleep(500);      // wait for refresh device
                sumA += ck.CalibrationValues.Smp_VoltageA;
                sumB += ck.CalibrationValues.Smp_VoltageB;
                sumC += ck.CalibrationValues.Smp_VoltageC;
            }

            A.x2 = sumA >> 2;
            B.x2 = sumB >> 2;
            C.x2 = sumC >> 2;

            txtSmp2.Text = string.Format("A={0},B={1},C={2}", A.x2, B.x2, C.x2);

            btnStep4.Enabled = true;
            btnStep3.Enabled = false;
        }

        private void btnStep4_Click(object sender, EventArgs e)
        {
            try
            {
                string str = "";

                if (chkA.Checked)
                {
                    calcKB(ref A);
                    str += string.Format("A.K={0},A.B={1},", A.k, A.b);
                }

                if (chkB.Checked)
                {
                    calcKB(ref B);
                    str += string.Format("B.K={0},B.B={1},", B.k, B.b);
                }

                if (chkC.Checked)
                {
                    calcKB(ref C);
                    str += string.Format("C.K={0},C.B={1},", C.k, C.b);
                }

                txtKB.Text = str;
                btnStep4.Enabled = false;

                setKB();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnStep5_Click(object sender, EventArgs e)
        {
            if (retry)
            {
                setKB();
            }
            else
            {
                Close();
            }
        }

        private void setKB()
        {
            CKM55LC_M ck = (CKM55LC_M)ComHelper.GetComLines()[0].Devices[0];

            lblbStep5.Text = "正在设置...";

            if (chkA.Checked)
            {
                ck.CalibrationValues.VoltageA_K = (short)A.k;
                ck.CalibrationValues.VoltageA_B = (short)A.b;
            }

            if (chkB.Checked)
            {
                ck.CalibrationValues.VoltageB_K = (short)B.k;
                ck.CalibrationValues.VoltageB_B = (short)B.b;
            }
            
            if (chkC.Checked)
            {
                ck.CalibrationValues.VoltageC_K = (short)C.k;
                ck.CalibrationValues.VoltageC_B = (short)C.b;
            }

            for (int i = 0; i < 5; i++)
            {
                int err = 0;

                Thread.Sleep(300);

                if (chkA.Checked)
                {
                    if (ck.CalibrationValues.VoltageA_K != (short)A.k ||
                        ck.CalibrationValues.VoltageA_B != (short)A.b ||
                        ck.IsOffline == true)
                    {
                        err = 1;
                    }
                }

                if (chkB.Checked)
                {
                    if (ck.CalibrationValues.VoltageB_K != (short)B.k ||
                        ck.CalibrationValues.VoltageB_B != (short)B.b ||
                        ck.IsOffline == true)
                    {
                        err = 1;
                    }
                }

                if (chkC.Checked)
                {
                    if (ck.CalibrationValues.VoltageC_K != (short)C.k ||
                        ck.CalibrationValues.VoltageC_B != (short)C.b ||
                        ck.IsOffline == true)
                    {
                        err = 1;
                    }
                }

                if (err == 0)
                {
                    lblbStep5.Text = "校准完成.";
                    btnStep5.Text = "关闭";
                    retry = false;
                    return;
                }
            }

            retry = true;

            lblbStep5.Text = "设置失败，请重试!";
            btnStep5.Text = "重试";
        }

        private void calcKB(ref Fun f)
        {
            if (Math.Abs(f.x2 - f.x1) <= 30)
            {
                throw new Exception("错误：两次采样值差小于30！");
            }

            f.k = (f.y2 - f.y1) / (f.x2 - f.x1);
            f.b = f.y2 - f.k * f.x2;

            f.k = Math.Round(f.k * 16384);
            f.b = Math.Round(f.b * 16384);
        }

        private void EnterToTab_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keys)e.KeyValue == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }
    }
}
