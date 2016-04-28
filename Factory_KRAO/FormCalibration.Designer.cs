namespace Factory_KRAO
{
    partial class FormCalibration
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.grdCalibrationValue = new System.Windows.Forms.DataGridView();
            this.grdRunValue = new System.Windows.Forms.DataGridView();
            this.grdSmp = new System.Windows.Forms.DataGridView();
            this.gbxCurrent = new System.Windows.Forms.GroupBox();
            this.btnSetFullload = new System.Windows.Forms.Button();
            this.btnCalResidual = new System.Windows.Forms.Button();
            this.btnCalVoltage = new System.Windows.Forms.Button();
            this.btnCalCurrent = new System.Windows.Forms.Button();
            this.cmbFullload = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbxIdentity = new System.Windows.Forms.GroupBox();
            this.labSW = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labManufacture = new System.Windows.Forms.Label();
            this.btnSetPN = new System.Windows.Forms.Button();
            this.labHW = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labProfile = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPN = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.gbxProtect = new System.Windows.Forms.GroupBox();
            this.btnProtectDisable = new System.Windows.Forms.Button();
            this.btnProtectEnable = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.stalabMBPortName = new System.Windows.Forms.ToolStripStatusLabel();
            this.stalabMBBaudRate = new System.Windows.Forms.ToolStripStatusLabel();
            this.stalabMBIsOffline = new System.Windows.Forms.ToolStripStatusLabel();
            this.stalabMBStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnActivation = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdCalibrationValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdRunValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSmp)).BeginInit();
            this.gbxCurrent.SuspendLayout();
            this.gbxIdentity.SuspendLayout();
            this.gbxProtect.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdCalibrationValue
            // 
            this.grdCalibrationValue.AllowUserToAddRows = false;
            this.grdCalibrationValue.AllowUserToDeleteRows = false;
            this.grdCalibrationValue.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.grdCalibrationValue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdCalibrationValue.Location = new System.Drawing.Point(184, 51);
            this.grdCalibrationValue.Name = "grdCalibrationValue";
            this.grdCalibrationValue.ReadOnly = true;
            this.grdCalibrationValue.RowHeadersVisible = false;
            this.grdCalibrationValue.RowTemplate.Height = 23;
            this.grdCalibrationValue.Size = new System.Drawing.Size(166, 404);
            this.grdCalibrationValue.TabIndex = 86;
            // 
            // grdRunValue
            // 
            this.grdRunValue.AllowUserToAddRows = false;
            this.grdRunValue.AllowUserToDeleteRows = false;
            this.grdRunValue.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.grdRunValue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdRunValue.Location = new System.Drawing.Point(356, 51);
            this.grdRunValue.Name = "grdRunValue";
            this.grdRunValue.ReadOnly = true;
            this.grdRunValue.RowHeadersVisible = false;
            this.grdRunValue.RowTemplate.Height = 23;
            this.grdRunValue.Size = new System.Drawing.Size(166, 404);
            this.grdRunValue.TabIndex = 85;
            // 
            // grdSmp
            // 
            this.grdSmp.AllowUserToAddRows = false;
            this.grdSmp.AllowUserToDeleteRows = false;
            this.grdSmp.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.grdSmp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdSmp.Location = new System.Drawing.Point(12, 51);
            this.grdSmp.Name = "grdSmp";
            this.grdSmp.ReadOnly = true;
            this.grdSmp.RowHeadersVisible = false;
            this.grdSmp.RowTemplate.Height = 23;
            this.grdSmp.Size = new System.Drawing.Size(166, 233);
            this.grdSmp.TabIndex = 84;
            // 
            // gbxCurrent
            // 
            this.gbxCurrent.Controls.Add(this.btnSetFullload);
            this.gbxCurrent.Controls.Add(this.btnCalResidual);
            this.gbxCurrent.Controls.Add(this.btnCalVoltage);
            this.gbxCurrent.Controls.Add(this.btnCalCurrent);
            this.gbxCurrent.Controls.Add(this.cmbFullload);
            this.gbxCurrent.Controls.Add(this.label1);
            this.gbxCurrent.Enabled = false;
            this.gbxCurrent.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbxCurrent.Location = new System.Drawing.Point(528, 51);
            this.gbxCurrent.Name = "gbxCurrent";
            this.gbxCurrent.Size = new System.Drawing.Size(288, 145);
            this.gbxCurrent.TabIndex = 87;
            this.gbxCurrent.TabStop = false;
            this.gbxCurrent.Text = "电流校准";
            // 
            // btnSetFullload
            // 
            this.btnSetFullload.Location = new System.Drawing.Point(202, 19);
            this.btnSetFullload.Name = "btnSetFullload";
            this.btnSetFullload.Size = new System.Drawing.Size(80, 44);
            this.btnSetFullload.TabIndex = 5;
            this.btnSetFullload.Text = "设定额定\r\n电流规格";
            this.btnSetFullload.UseVisualStyleBackColor = true;
            this.btnSetFullload.Click += new System.EventHandler(this.btnSetFullload_Click);
            // 
            // btnCalResidual
            // 
            this.btnCalResidual.Location = new System.Drawing.Point(202, 85);
            this.btnCalResidual.Name = "btnCalResidual";
            this.btnCalResidual.Size = new System.Drawing.Size(80, 45);
            this.btnCalResidual.TabIndex = 4;
            this.btnCalResidual.Text = "校准\r\n剩余电流";
            this.btnCalResidual.UseVisualStyleBackColor = true;
            this.btnCalResidual.Click += new System.EventHandler(this.btnCalResidual_Click);
            // 
            // btnCalVoltage
            // 
            this.btnCalVoltage.Location = new System.Drawing.Point(104, 85);
            this.btnCalVoltage.Name = "btnCalVoltage";
            this.btnCalVoltage.Size = new System.Drawing.Size(80, 45);
            this.btnCalVoltage.TabIndex = 3;
            this.btnCalVoltage.Text = "校准\r\n三相电压";
            this.btnCalVoltage.UseVisualStyleBackColor = true;
            this.btnCalVoltage.Click += new System.EventHandler(this.btnCalVoltage_Click);
            // 
            // btnCalCurrent
            // 
            this.btnCalCurrent.Location = new System.Drawing.Point(6, 85);
            this.btnCalCurrent.Name = "btnCalCurrent";
            this.btnCalCurrent.Size = new System.Drawing.Size(80, 45);
            this.btnCalCurrent.TabIndex = 2;
            this.btnCalCurrent.Text = "校准\r\n三相电流";
            this.btnCalCurrent.UseVisualStyleBackColor = true;
            this.btnCalCurrent.Click += new System.EventHandler(this.btnCalCurrent_Click);
            // 
            // cmbFullload
            // 
            this.cmbFullload.FormattingEnabled = true;
            this.cmbFullload.Items.AddRange(new object[] {
            "100A",
            "250A",
            "400A",
            "630A",
            "800A"});
            this.cmbFullload.Location = new System.Drawing.Point(103, 41);
            this.cmbFullload.Name = "cmbFullload";
            this.cmbFullload.Size = new System.Drawing.Size(93, 22);
            this.cmbFullload.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "额定电流规格";
            // 
            // gbxIdentity
            // 
            this.gbxIdentity.Controls.Add(this.labSW);
            this.gbxIdentity.Controls.Add(this.label3);
            this.gbxIdentity.Controls.Add(this.labManufacture);
            this.gbxIdentity.Controls.Add(this.btnSetPN);
            this.gbxIdentity.Controls.Add(this.labHW);
            this.gbxIdentity.Controls.Add(this.label4);
            this.gbxIdentity.Controls.Add(this.labProfile);
            this.gbxIdentity.Controls.Add(this.label5);
            this.gbxIdentity.Controls.Add(this.txtPN);
            this.gbxIdentity.Controls.Add(this.label6);
            this.gbxIdentity.Controls.Add(this.label7);
            this.gbxIdentity.Enabled = false;
            this.gbxIdentity.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbxIdentity.Location = new System.Drawing.Point(528, 290);
            this.gbxIdentity.Name = "gbxIdentity";
            this.gbxIdentity.Size = new System.Drawing.Size(297, 165);
            this.gbxIdentity.TabIndex = 88;
            this.gbxIdentity.TabStop = false;
            this.gbxIdentity.Text = "设备描述";
            this.gbxIdentity.EnabledChanged += new System.EventHandler(this.gbxIndentity_EnabledChanged);
            // 
            // labSW
            // 
            this.labSW.AutoSize = true;
            this.labSW.Location = new System.Drawing.Point(109, 133);
            this.labSW.Name = "labSW";
            this.labSW.Size = new System.Drawing.Size(63, 14);
            this.labSW.TabIndex = 112;
            this.labSW.Text = "软件版本";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 14);
            this.label3.TabIndex = 103;
            this.label3.Text = "产品型号：";
            // 
            // labManufacture
            // 
            this.labManufacture.AutoSize = true;
            this.labManufacture.Location = new System.Drawing.Point(106, 58);
            this.labManufacture.Name = "labManufacture";
            this.labManufacture.Size = new System.Drawing.Size(49, 14);
            this.labManufacture.TabIndex = 111;
            this.labManufacture.Text = "制造商";
            // 
            // btnSetPN
            // 
            this.btnSetPN.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSetPN.Location = new System.Drawing.Point(219, 113);
            this.btnSetPN.Name = "btnSetPN";
            this.btnSetPN.Size = new System.Drawing.Size(71, 34);
            this.btnSetPN.TabIndex = 102;
            this.btnSetPN.Tag = "";
            this.btnSetPN.Text = "设置编号";
            this.btnSetPN.UseVisualStyleBackColor = true;
            this.btnSetPN.Click += new System.EventHandler(this.btnSetPN_Click);
            // 
            // labHW
            // 
            this.labHW.AutoSize = true;
            this.labHW.Location = new System.Drawing.Point(109, 108);
            this.labHW.Name = "labHW";
            this.labHW.Size = new System.Drawing.Size(63, 14);
            this.labHW.TabIndex = 110;
            this.labHW.Text = "硬件版本";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 104;
            this.label4.Text = "制造商：";
            // 
            // labProfile
            // 
            this.labProfile.AutoSize = true;
            this.labProfile.Location = new System.Drawing.Point(106, 33);
            this.labProfile.Name = "labProfile";
            this.labProfile.Size = new System.Drawing.Size(63, 14);
            this.labProfile.TabIndex = 109;
            this.labProfile.Text = "产品型号";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 14);
            this.label5.TabIndex = 105;
            this.label5.Text = "产品编号：";
            // 
            // txtPN
            // 
            this.txtPN.Location = new System.Drawing.Point(109, 80);
            this.txtPN.Multiline = true;
            this.txtPN.Name = "txtPN";
            this.txtPN.Size = new System.Drawing.Size(181, 23);
            this.txtPN.TabIndex = 108;
            this.txtPN.Text = "0123456789ABCDEF";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 108);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 14);
            this.label6.TabIndex = 106;
            this.label6.Text = "硬件版本号：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 133);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 14);
            this.label7.TabIndex = 107;
            this.label7.Text = "软件版本号：";
            // 
            // gbxProtect
            // 
            this.gbxProtect.Controls.Add(this.btnProtectDisable);
            this.gbxProtect.Controls.Add(this.btnProtectEnable);
            this.gbxProtect.Enabled = false;
            this.gbxProtect.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbxProtect.Location = new System.Drawing.Point(528, 202);
            this.gbxProtect.Name = "gbxProtect";
            this.gbxProtect.Size = new System.Drawing.Size(196, 82);
            this.gbxProtect.TabIndex = 96;
            this.gbxProtect.TabStop = false;
            this.gbxProtect.Text = "保护设定";
            // 
            // btnProtectDisable
            // 
            this.btnProtectDisable.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnProtectDisable.Location = new System.Drawing.Point(6, 31);
            this.btnProtectDisable.Name = "btnProtectDisable";
            this.btnProtectDisable.Size = new System.Drawing.Size(71, 41);
            this.btnProtectDisable.TabIndex = 92;
            this.btnProtectDisable.Tag = "";
            this.btnProtectDisable.Text = "关闭所有保护";
            this.btnProtectDisable.UseVisualStyleBackColor = true;
            this.btnProtectDisable.Click += new System.EventHandler(this.btnProtectDisable_Click);
            // 
            // btnProtectEnable
            // 
            this.btnProtectEnable.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnProtectEnable.Location = new System.Drawing.Point(104, 31);
            this.btnProtectEnable.Name = "btnProtectEnable";
            this.btnProtectEnable.Size = new System.Drawing.Size(71, 41);
            this.btnProtectEnable.TabIndex = 93;
            this.btnProtectEnable.Tag = "";
            this.btnProtectEnable.Text = "打开默认保护";
            this.btnProtectEnable.UseVisualStyleBackColor = true;
            this.btnProtectEnable.Click += new System.EventHandler(this.btnProtectEnable_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stalabMBPortName,
            this.stalabMBBaudRate,
            this.stalabMBIsOffline,
            this.stalabMBStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 469);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.ShowItemToolTips = true;
            this.statusStrip1.Size = new System.Drawing.Size(834, 26);
            this.statusStrip1.TabIndex = 99;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // stalabMBPortName
            // 
            this.stalabMBPortName.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.stalabMBPortName.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedOuter;
            this.stalabMBPortName.Margin = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.stalabMBPortName.Name = "stalabMBPortName";
            this.stalabMBPortName.Size = new System.Drawing.Size(19, 21);
            this.stalabMBPortName.Text = "0";
            this.stalabMBPortName.ToolTipText = "Modbus端口号";
            // 
            // stalabMBBaudRate
            // 
            this.stalabMBBaudRate.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.stalabMBBaudRate.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedOuter;
            this.stalabMBBaudRate.Margin = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.stalabMBBaudRate.Name = "stalabMBBaudRate";
            this.stalabMBBaudRate.Size = new System.Drawing.Size(19, 21);
            this.stalabMBBaudRate.Text = "0";
            this.stalabMBBaudRate.ToolTipText = "Modbus比特率";
            // 
            // stalabMBIsOffline
            // 
            this.stalabMBIsOffline.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.stalabMBIsOffline.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedOuter;
            this.stalabMBIsOffline.Margin = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.stalabMBIsOffline.Name = "stalabMBIsOffline";
            this.stalabMBIsOffline.Size = new System.Drawing.Size(121, 21);
            this.stalabMBIsOffline.Text = "CKM55LC-M未连接";
            // 
            // stalabMBStatus
            // 
            this.stalabMBStatus.Name = "stalabMBStatus";
            this.stalabMBStatus.Size = new System.Drawing.Size(39, 21);
            this.stalabMBStatus.Text = "NULL";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(358, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 12);
            this.label9.TabIndex = 103;
            this.label9.Text = "实际测量值：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(184, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 102;
            this.label8.Text = "校准系数：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 101;
            this.label2.Text = "采样值：";
            // 
            // btnActivation
            // 
            this.btnActivation.BackColor = System.Drawing.SystemColors.Control;
            this.btnActivation.Enabled = false;
            this.btnActivation.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnActivation.Location = new System.Drawing.Point(41, 342);
            this.btnActivation.Name = "btnActivation";
            this.btnActivation.Size = new System.Drawing.Size(96, 45);
            this.btnActivation.TabIndex = 104;
            this.btnActivation.Text = "线路板激活";
            this.btnActivation.UseVisualStyleBackColor = true;
            this.btnActivation.Click += new System.EventHandler(this.btnActivation_Click);
            // 
            // FormCalibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 495);
            this.Controls.Add(this.btnActivation);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.gbxProtect);
            this.Controls.Add(this.gbxIdentity);
            this.Controls.Add(this.gbxCurrent);
            this.Controls.Add(this.grdCalibrationValue);
            this.Controls.Add(this.grdRunValue);
            this.Controls.Add(this.grdSmp);
            this.Name = "FormCalibration";
            this.Text = "CKM55LC-M校准配置程序-江苏凯隆";
            this.Load += new System.EventHandler(this.FormCalibration_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdCalibrationValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdRunValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSmp)).EndInit();
            this.gbxCurrent.ResumeLayout(false);
            this.gbxCurrent.PerformLayout();
            this.gbxIdentity.ResumeLayout(false);
            this.gbxIdentity.PerformLayout();
            this.gbxProtect.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grdCalibrationValue;
        private System.Windows.Forms.DataGridView grdRunValue;
        private System.Windows.Forms.DataGridView grdSmp;
        private System.Windows.Forms.GroupBox gbxCurrent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbxIdentity;
        private System.Windows.Forms.Button btnSetFullload;
        private System.Windows.Forms.Button btnCalResidual;
        private System.Windows.Forms.Button btnCalVoltage;
        private System.Windows.Forms.Button btnCalCurrent;
        private System.Windows.Forms.ComboBox cmbFullload;
        private System.Windows.Forms.GroupBox gbxProtect;
        private System.Windows.Forms.Button btnProtectDisable;
        private System.Windows.Forms.Button btnProtectEnable;
        private System.Windows.Forms.Label labSW;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labManufacture;
        private System.Windows.Forms.Button btnSetPN;
        private System.Windows.Forms.Label labHW;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labProfile;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPN;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel stalabMBPortName;
        private System.Windows.Forms.ToolStripStatusLabel stalabMBBaudRate;
        private System.Windows.Forms.ToolStripStatusLabel stalabMBIsOffline;
        private System.Windows.Forms.ToolStripStatusLabel stalabMBStatus;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnActivation;
    }
}

