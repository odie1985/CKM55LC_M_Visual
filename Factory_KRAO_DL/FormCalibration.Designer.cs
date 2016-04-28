namespace Factory_KRAO_DL
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
            this.components = new System.ComponentModel.Container();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.stalabMBPortName = new System.Windows.Forms.ToolStripStatusLabel();
            this.stalabMBBaudRate = new System.Windows.Forms.ToolStripStatusLabel();
            this.stalabMBIsOffline = new System.Windows.Forms.ToolStripStatusLabel();
            this.stalabMBStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.grdRunValue = new System.Windows.Forms.DataGridView();
            this.btnTest = new System.Windows.Forms.Button();
            this.label25 = new System.Windows.Forms.Label();
            this.txtDetect1 = new System.Windows.Forms.TextBox();
            this.cbxBaudRate = new System.Windows.Forms.ComboBox();
            this.cbxCom = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.grdSmp = new System.Windows.Forms.DataGridView();
            this.grdResidualLimit = new System.Windows.Forms.DataGridView();
            this.grdTrip = new System.Windows.Forms.DataGridView();
            this.grdRatedResidual = new System.Windows.Forms.DataGridView();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.txtProductDate = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAssetCode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDeviceNum = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSetBaudrate = new System.Windows.Forms.Button();
            this.btnReadBaudrate = new System.Windows.Forms.Button();
            this.btnSetAddr = new System.Windows.Forms.Button();
            this.btnReadAddr = new System.Windows.Forms.Button();
            this.btnWriteDeviceNum = new System.Windows.Forms.Button();
            this.btnWriteProductDate = new System.Windows.Forms.Button();
            this.btnWriteAssetCode = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnReadDeviceNum = new System.Windows.Forms.Button();
            this.btnReadProductDate = new System.Windows.Forms.Button();
            this.btnReadAssetCode = new System.Windows.Forms.Button();
            this.btnReadFactoryValue = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdRunValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSmp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdResidualLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTrip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdRatedResidual)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOpen.Location = new System.Drawing.Point(388, 20);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(71, 41);
            this.btnOpen.TabIndex = 92;
            this.btnOpen.Tag = "";
            this.btnOpen.Text = "分闸";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.Location = new System.Drawing.Point(388, 91);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(71, 41);
            this.btnClose.TabIndex = 93;
            this.btnClose.Tag = "";
            this.btnClose.Text = "合闸";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stalabMBPortName,
            this.stalabMBBaudRate,
            this.stalabMBIsOffline,
            this.stalabMBStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 695);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.ShowItemToolTips = true;
            this.statusStrip1.Size = new System.Drawing.Size(873, 26);
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
            // grdRunValue
            // 
            this.grdRunValue.AllowUserToAddRows = false;
            this.grdRunValue.AllowUserToDeleteRows = false;
            this.grdRunValue.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.grdRunValue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdRunValue.Location = new System.Drawing.Point(590, 26);
            this.grdRunValue.Name = "grdRunValue";
            this.grdRunValue.ReadOnly = true;
            this.grdRunValue.RowHeadersVisible = false;
            this.grdRunValue.RowTemplate.Height = 23;
            this.grdRunValue.Size = new System.Drawing.Size(200, 344);
            this.grdRunValue.TabIndex = 85;
            // 
            // btnTest
            // 
            this.btnTest.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTest.Location = new System.Drawing.Point(388, 162);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(71, 41);
            this.btnTest.TabIndex = 104;
            this.btnTest.Tag = "";
            this.btnTest.Text = "试验";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label25.Location = new System.Drawing.Point(6, 112);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(70, 14);
            this.label25.TabIndex = 108;
            this.label25.Text = "通讯地址:";
            // 
            // txtDetect1
            // 
            this.txtDetect1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDetect1.Location = new System.Drawing.Point(82, 109);
            this.txtDetect1.MaxLength = 6;
            this.txtDetect1.Name = "txtDetect1";
            this.txtDetect1.Size = new System.Drawing.Size(63, 23);
            this.txtDetect1.TabIndex = 107;
            this.txtDetect1.Text = "1";
            this.txtDetect1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cbxBaudRate
            // 
            this.cbxBaudRate.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbxBaudRate.FormattingEnabled = true;
            this.cbxBaudRate.Items.AddRange(new object[] {
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "19200"});
            this.cbxBaudRate.Location = new System.Drawing.Point(82, 68);
            this.cbxBaudRate.Name = "cbxBaudRate";
            this.cbxBaudRate.Size = new System.Drawing.Size(63, 22);
            this.cbxBaudRate.TabIndex = 49;
            this.cbxBaudRate.Text = "2400";
            // 
            // cbxCom
            // 
            this.cbxCom.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbxCom.FormattingEnabled = true;
            this.cbxCom.Location = new System.Drawing.Point(82, 27);
            this.cbxCom.Name = "cbxCom";
            this.cbxCom.Size = new System.Drawing.Size(63, 22);
            this.cbxCom.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(6, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 14);
            this.label1.TabIndex = 48;
            this.label1.Text = "通讯速率:";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label26.Location = new System.Drawing.Point(6, 30);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(42, 14);
            this.label26.TabIndex = 90;
            this.label26.Text = "串口:";
            // 
            // btnConnect
            // 
            this.btnConnect.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConnect.Location = new System.Drawing.Point(136, 150);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(81, 35);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "连接串口";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // grdSmp
            // 
            this.grdSmp.AllowUserToAddRows = false;
            this.grdSmp.AllowUserToDeleteRows = false;
            this.grdSmp.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.grdSmp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdSmp.Location = new System.Drawing.Point(14, 38);
            this.grdSmp.Name = "grdSmp";
            this.grdSmp.ReadOnly = true;
            this.grdSmp.RowHeadersVisible = false;
            this.grdSmp.RowTemplate.Height = 23;
            this.grdSmp.Size = new System.Drawing.Size(224, 160);
            this.grdSmp.TabIndex = 106;
            // 
            // grdResidualLimit
            // 
            this.grdResidualLimit.AllowUserToAddRows = false;
            this.grdResidualLimit.AllowUserToDeleteRows = false;
            this.grdResidualLimit.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.grdResidualLimit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdResidualLimit.Location = new System.Drawing.Point(623, 38);
            this.grdResidualLimit.Name = "grdResidualLimit";
            this.grdResidualLimit.ReadOnly = true;
            this.grdResidualLimit.RowHeadersVisible = false;
            this.grdResidualLimit.RowTemplate.Height = 23;
            this.grdResidualLimit.Size = new System.Drawing.Size(201, 129);
            this.grdResidualLimit.TabIndex = 107;
            // 
            // grdTrip
            // 
            this.grdTrip.AllowUserToAddRows = false;
            this.grdTrip.AllowUserToDeleteRows = false;
            this.grdTrip.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdTrip.Location = new System.Drawing.Point(416, 38);
            this.grdTrip.Name = "grdTrip";
            this.grdTrip.ReadOnly = true;
            this.grdTrip.RowHeadersVisible = false;
            this.grdTrip.RowTemplate.Height = 23;
            this.grdTrip.Size = new System.Drawing.Size(201, 246);
            this.grdTrip.TabIndex = 108;
            // 
            // grdRatedResidual
            // 
            this.grdRatedResidual.AllowUserToAddRows = false;
            this.grdRatedResidual.AllowUserToDeleteRows = false;
            this.grdRatedResidual.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.grdRatedResidual.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdRatedResidual.Location = new System.Drawing.Point(244, 38);
            this.grdRatedResidual.Name = "grdRatedResidual";
            this.grdRatedResidual.ReadOnly = true;
            this.grdRatedResidual.RowHeadersVisible = false;
            this.grdRatedResidual.RowTemplate.Height = 23;
            this.grdRatedResidual.Size = new System.Drawing.Size(166, 206);
            this.grdRatedResidual.TabIndex = 109;
            // 
            // txtProductDate
            // 
            this.txtProductDate.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtProductDate.Location = new System.Drawing.Point(105, 295);
            this.txtProductDate.MaxLength = 10;
            this.txtProductDate.Name = "txtProductDate";
            this.txtProductDate.Size = new System.Drawing.Size(242, 23);
            this.txtProductDate.TabIndex = 110;
            this.txtProductDate.Text = "20160418";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(29, 298);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 14);
            this.label2.TabIndex = 111;
            this.label2.Text = "生产日期:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(29, 354);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 14);
            this.label3.TabIndex = 112;
            this.label3.Text = "资产编码:";
            // 
            // txtAssetCode
            // 
            this.txtAssetCode.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtAssetCode.Location = new System.Drawing.Point(105, 351);
            this.txtAssetCode.MaxLength = 32;
            this.txtAssetCode.Name = "txtAssetCode";
            this.txtAssetCode.Size = new System.Drawing.Size(242, 23);
            this.txtAssetCode.TabIndex = 113;
            this.txtAssetCode.Text = "66666666666666666666666666666666";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(43, 245);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 14);
            this.label4.TabIndex = 115;
            this.label4.Text = "设备号:";
            // 
            // txtDeviceNum
            // 
            this.txtDeviceNum.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDeviceNum.Location = new System.Drawing.Point(105, 242);
            this.txtDeviceNum.MaxLength = 12;
            this.txtDeviceNum.Name = "txtDeviceNum";
            this.txtDeviceNum.Size = new System.Drawing.Size(242, 23);
            this.txtDeviceNum.TabIndex = 114;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSetBaudrate);
            this.groupBox1.Controls.Add(this.btnReadBaudrate);
            this.groupBox1.Controls.Add(this.btnSetAddr);
            this.groupBox1.Controls.Add(this.btnReadAddr);
            this.groupBox1.Controls.Add(this.label26);
            this.groupBox1.Controls.Add(this.cbxCom);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cbxBaudRate);
            this.groupBox1.Controls.Add(this.label25);
            this.groupBox1.Controls.Add(this.txtDetect1);
            this.groupBox1.Controls.Add(this.btnConnect);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(370, 192);
            this.groupBox1.TabIndex = 116;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "串口连接";
            // 
            // btnSetBaudrate
            // 
            this.btnSetBaudrate.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSetBaudrate.Location = new System.Drawing.Point(293, 60);
            this.btnSetBaudrate.Name = "btnSetBaudrate";
            this.btnSetBaudrate.Size = new System.Drawing.Size(64, 35);
            this.btnSetBaudrate.TabIndex = 114;
            this.btnSetBaudrate.Text = "写";
            this.btnSetBaudrate.UseVisualStyleBackColor = true;
            this.btnSetBaudrate.Click += new System.EventHandler(this.btnSetBaudrate_Click);
            // 
            // btnReadBaudrate
            // 
            this.btnReadBaudrate.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReadBaudrate.Location = new System.Drawing.Point(223, 60);
            this.btnReadBaudrate.Name = "btnReadBaudrate";
            this.btnReadBaudrate.Size = new System.Drawing.Size(64, 35);
            this.btnReadBaudrate.TabIndex = 113;
            this.btnReadBaudrate.Text = "读";
            this.btnReadBaudrate.UseVisualStyleBackColor = true;
            this.btnReadBaudrate.Click += new System.EventHandler(this.btnReadBaudrate_Click);
            // 
            // btnSetAddr
            // 
            this.btnSetAddr.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSetAddr.Location = new System.Drawing.Point(293, 101);
            this.btnSetAddr.Name = "btnSetAddr";
            this.btnSetAddr.Size = new System.Drawing.Size(64, 35);
            this.btnSetAddr.TabIndex = 112;
            this.btnSetAddr.Text = "写";
            this.btnSetAddr.UseVisualStyleBackColor = true;
            this.btnSetAddr.Click += new System.EventHandler(this.btnSetAddr_Click);
            // 
            // btnReadAddr
            // 
            this.btnReadAddr.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReadAddr.Location = new System.Drawing.Point(223, 101);
            this.btnReadAddr.Name = "btnReadAddr";
            this.btnReadAddr.Size = new System.Drawing.Size(64, 35);
            this.btnReadAddr.TabIndex = 111;
            this.btnReadAddr.Text = "读";
            this.btnReadAddr.UseVisualStyleBackColor = true;
            this.btnReadAddr.Click += new System.EventHandler(this.btnReadAddr_Click);
            // 
            // btnWriteDeviceNum
            // 
            this.btnWriteDeviceNum.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWriteDeviceNum.Location = new System.Drawing.Point(409, 232);
            this.btnWriteDeviceNum.Name = "btnWriteDeviceNum";
            this.btnWriteDeviceNum.Size = new System.Drawing.Size(50, 40);
            this.btnWriteDeviceNum.TabIndex = 117;
            this.btnWriteDeviceNum.Text = "写";
            this.btnWriteDeviceNum.UseVisualStyleBackColor = true;
            this.btnWriteDeviceNum.Click += new System.EventHandler(this.btnWriteDeviceNum_Click);
            // 
            // btnWriteProductDate
            // 
            this.btnWriteProductDate.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWriteProductDate.Location = new System.Drawing.Point(409, 286);
            this.btnWriteProductDate.Name = "btnWriteProductDate";
            this.btnWriteProductDate.Size = new System.Drawing.Size(50, 39);
            this.btnWriteProductDate.TabIndex = 118;
            this.btnWriteProductDate.Text = "写";
            this.btnWriteProductDate.UseVisualStyleBackColor = true;
            this.btnWriteProductDate.Click += new System.EventHandler(this.btnWriteProductDate_Click);
            // 
            // btnWriteAssetCode
            // 
            this.btnWriteAssetCode.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWriteAssetCode.Location = new System.Drawing.Point(409, 340);
            this.btnWriteAssetCode.Name = "btnWriteAssetCode";
            this.btnWriteAssetCode.Size = new System.Drawing.Size(50, 40);
            this.btnWriteAssetCode.TabIndex = 119;
            this.btnWriteAssetCode.Text = "写";
            this.btnWriteAssetCode.UseVisualStyleBackColor = true;
            this.btnWriteAssetCode.Click += new System.EventHandler(this.btnWriteAssetCode_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(661, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 120;
            this.label5.Text = "采集数据";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(93, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 14);
            this.label6.TabIndex = 121;
            this.label6.Text = "额定值";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(450, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(133, 14);
            this.label7.TabIndex = 122;
            this.label7.Text = "保护器跳闸事件记录";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(240, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(175, 14);
            this.label8.TabIndex = 123;
            this.label8.Text = "额定剩余电流动作值参数组";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(649, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(147, 14);
            this.label9.TabIndex = 124;
            this.label9.Text = "剩余电流超限事件记录";
            // 
            // btnReadDeviceNum
            // 
            this.btnReadDeviceNum.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReadDeviceNum.Location = new System.Drawing.Point(353, 232);
            this.btnReadDeviceNum.Name = "btnReadDeviceNum";
            this.btnReadDeviceNum.Size = new System.Drawing.Size(50, 40);
            this.btnReadDeviceNum.TabIndex = 125;
            this.btnReadDeviceNum.Text = "读";
            this.btnReadDeviceNum.UseVisualStyleBackColor = true;
            this.btnReadDeviceNum.Click += new System.EventHandler(this.btnReadDeviceNum_Click);
            // 
            // btnReadProductDate
            // 
            this.btnReadProductDate.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReadProductDate.Location = new System.Drawing.Point(353, 286);
            this.btnReadProductDate.Name = "btnReadProductDate";
            this.btnReadProductDate.Size = new System.Drawing.Size(50, 40);
            this.btnReadProductDate.TabIndex = 126;
            this.btnReadProductDate.Text = "读";
            this.btnReadProductDate.UseVisualStyleBackColor = true;
            this.btnReadProductDate.Click += new System.EventHandler(this.btnReadProductDate_Click);
            // 
            // btnReadAssetCode
            // 
            this.btnReadAssetCode.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReadAssetCode.Location = new System.Drawing.Point(353, 341);
            this.btnReadAssetCode.Name = "btnReadAssetCode";
            this.btnReadAssetCode.Size = new System.Drawing.Size(50, 40);
            this.btnReadAssetCode.TabIndex = 127;
            this.btnReadAssetCode.Text = "读";
            this.btnReadAssetCode.UseVisualStyleBackColor = true;
            this.btnReadAssetCode.Click += new System.EventHandler(this.btnReadAssetCode_Click);
            // 
            // btnReadFactoryValue
            // 
            this.btnReadFactoryValue.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReadFactoryValue.Location = new System.Drawing.Point(720, 230);
            this.btnReadFactoryValue.Name = "btnReadFactoryValue";
            this.btnReadFactoryValue.Size = new System.Drawing.Size(86, 41);
            this.btnReadFactoryValue.TabIndex = 129;
            this.btnReadFactoryValue.Tag = "";
            this.btnReadFactoryValue.Text = "读取出厂值";
            this.btnReadFactoryValue.UseVisualStyleBackColor = true;
            this.btnReadFactoryValue.Click += new System.EventHandler(this.btnReadFactoryValue_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.grdTrip);
            this.groupBox2.Controls.Add(this.btnReadFactoryValue);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.grdResidualLimit);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.grdSmp);
            this.groupBox2.Controls.Add(this.grdRatedResidual);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(12, 387);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(838, 301);
            this.groupBox2.TabIndex = 130;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "出厂数据";
            // 
            // FormCalibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(873, 721);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnReadAssetCode);
            this.Controls.Add(this.btnReadProductDate);
            this.Controls.Add(this.btnReadDeviceNum);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnWriteAssetCode);
            this.Controls.Add(this.btnWriteProductDate);
            this.Controls.Add(this.btnWriteDeviceNum);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtDeviceNum);
            this.Controls.Add(this.txtAssetCode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtProductDate);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.grdRunValue);
            this.Name = "FormCalibration";
            this.Text = "CKM55LC-M-DL《规约》通讯测试程序-江苏凯隆";
            this.Load += new System.EventHandler(this.FormCalibration_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdRunValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSmp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdResidualLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTrip)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdRatedResidual)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel stalabMBPortName;
        private System.Windows.Forms.ToolStripStatusLabel stalabMBBaudRate;
        private System.Windows.Forms.ToolStripStatusLabel stalabMBIsOffline;
        private System.Windows.Forms.ToolStripStatusLabel stalabMBStatus;
        private System.Windows.Forms.DataGridView grdRunValue;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.ComboBox cbxBaudRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ComboBox cbxCom;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.DataGridView grdSmp;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox txtDetect1;
        private System.Windows.Forms.DataGridView grdResidualLimit;
        private System.Windows.Forms.DataGridView grdTrip;
        private System.Windows.Forms.DataGridView grdRatedResidual;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.TextBox txtProductDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAssetCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDeviceNum;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnWriteDeviceNum;
        private System.Windows.Forms.Button btnWriteProductDate;
        private System.Windows.Forms.Button btnWriteAssetCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnReadDeviceNum;
        private System.Windows.Forms.Button btnReadProductDate;
        private System.Windows.Forms.Button btnReadAssetCode;
        private System.Windows.Forms.Button btnReadFactoryValue;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSetAddr;
        private System.Windows.Forms.Button btnReadAddr;
        private System.Windows.Forms.Button btnSetBaudrate;
        private System.Windows.Forms.Button btnReadBaudrate;
    }
}

