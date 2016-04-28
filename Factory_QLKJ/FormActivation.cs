using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using Lines.Com;
using log4net;
using LinesDevicesManager;
using Devices.MCCB;

namespace Factory_KRAO
{
    public partial class FormActivation : Form
    {
        public FormActivation()
        {
            InitializeComponent();
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            ushort value = 0;

            if (rdobtnKRAO.Checked)
            {
                value = 0x2130;
            }
            else if (rdobtnQLKJ.Checked)
            {
                value = 0x3256;
            }
            else if (rdobtnSHRM.Checked)
            {
                value = 0x2000;
            }

            CKM55LC_M ck = (CKM55LC_M)ComHelper.GetComLines()[0].Devices[0];
            ck.CalibrationValues.License = value;  
        }


    }
}
