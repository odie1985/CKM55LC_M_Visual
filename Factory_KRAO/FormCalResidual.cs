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
    public partial class FormCalResidual : Form
    {
        /// <summary>
        /// 日志
        /// </summary>
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(FormCalResidual));

        /// <summary>
        /// 消息传递事件处理委托
        /// </summary>
        public delegate void DelegateMessage(object sender, string message);

        // 函数 y * 1024 = (kx + b) * 1024
        private double x1 = 0;
        private double y1 = 0;
        private double x2 = 0;
        private double y2 = 0;
        private double k = 0;
        private double b = 0;

        private bool retry = false;

        public FormCalResidual()
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
                y1 = double.Parse(txtY1.Text);
            }
            catch (System.Exception)
            {
                MessageBox.Show("错误：电流输入错误!");
                return;
            }

            CKM55LC_M ck = (CKM55LC_M)ComHelper.GetComLines()[0].Devices[0];

            int sum = 0;
            for (int i = 0; i < 4; i++)
            {
                Thread.Sleep(500);      // wait for refresh device
                sum += ck.CalibrationValues.Smp_Residual;
            }

            x1 = sum >> 2;

            txtSmp1.Text = x1.ToString();

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
                y2 = double.Parse(txtY2.Text);
            }
            catch (System.Exception)
            {
                MessageBox.Show("错误：电流输入错误!");
                return;
            }

            CKM55LC_M ck = (CKM55LC_M)ComHelper.GetComLines()[0].Devices[0];

            int sum = 0;
            for (int i = 0; i < 4; i++)
            {
                Thread.Sleep(500);      // wait for refresh device
                sum += ck.CalibrationValues.Smp_Residual;
            }

            x2 = sum >> 2;

            txtSmp2.Text = x2.ToString();

            btnStep4.Enabled = true;
            btnStep3.Enabled = false;
        }

        private void btnStep4_Click(object sender, EventArgs e)
        {
            btnStep4.Enabled = false;

            try
            {
                if ((x2 - x1) == 0)
                {
                    throw new Exception("错误：两次电流采样值相等！");
                }

                k = (y2 - y1) / (x2 - x1);
                b = y2 - k * x2;

                k = Math.Round(k * 1024);
                b = Math.Round(b * 1024);

                txtKB.Text = string.Format("K={0},B={1}", k, b);

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
            ck.CalibrationValues.Residual_K = (short)k;
            ck.CalibrationValues.Residual_B = (short)b;

            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(300);

                if ((ck.CalibrationValues.Residual_K == k) &&
                    (ck.CalibrationValues.Residual_B == b) &&
                    ck.IsOffline == false)
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

        private void EnterToTab_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keys)e.KeyValue == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }
    }
}
