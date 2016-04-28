namespace Factory_KRAO
{
    partial class FormActivation
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rdobtnKRAO = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.rdobtnQLKJ = new System.Windows.Forms.RadioButton();
            this.rdobtnSHRM = new System.Windows.Forms.RadioButton();
            this.btnActivate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rdobtnKRAO
            // 
            this.rdobtnKRAO.AutoSize = true;
            this.rdobtnKRAO.Checked = true;
            this.rdobtnKRAO.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rdobtnKRAO.Location = new System.Drawing.Point(15, 42);
            this.rdobtnKRAO.Name = "rdobtnKRAO";
            this.rdobtnKRAO.Size = new System.Drawing.Size(109, 18);
            this.rdobtnKRAO.TabIndex = 0;
            this.rdobtnKRAO.TabStop = true;
            this.rdobtnKRAO.Text = "江苏凯隆电器";
            this.rdobtnKRAO.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "选择激活厂商：";
            // 
            // rdobtnQLKJ
            // 
            this.rdobtnQLKJ.AutoSize = true;
            this.rdobtnQLKJ.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rdobtnQLKJ.Location = new System.Drawing.Point(15, 79);
            this.rdobtnQLKJ.Name = "rdobtnQLKJ";
            this.rdobtnQLKJ.Size = new System.Drawing.Size(109, 18);
            this.rdobtnQLKJ.TabIndex = 2;
            this.rdobtnQLKJ.Text = "浙江乾龙科技";
            this.rdobtnQLKJ.UseVisualStyleBackColor = true;
            // 
            // rdobtnSHRM
            // 
            this.rdobtnSHRM.AutoSize = true;
            this.rdobtnSHRM.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rdobtnSHRM.Location = new System.Drawing.Point(15, 117);
            this.rdobtnSHRM.Name = "rdobtnSHRM";
            this.rdobtnSHRM.Size = new System.Drawing.Size(95, 18);
            this.rdobtnSHRM.TabIndex = 3;
            this.rdobtnSHRM.Text = "上海人民厂";
            this.rdobtnSHRM.UseVisualStyleBackColor = true;
            // 
            // btnActivate
            // 
            this.btnActivate.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnActivate.Location = new System.Drawing.Point(206, 66);
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new System.Drawing.Size(80, 45);
            this.btnActivate.TabIndex = 4;
            this.btnActivate.Text = "激活";
            this.btnActivate.UseVisualStyleBackColor = true;
            this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
            // 
            // FormActivation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 176);
            this.Controls.Add(this.btnActivate);
            this.Controls.Add(this.rdobtnSHRM);
            this.Controls.Add(this.rdobtnQLKJ);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rdobtnKRAO);
            this.Name = "FormActivation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "漏电重合闸线路板激活";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdobtnKRAO;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdobtnQLKJ;
        private System.Windows.Forms.RadioButton rdobtnSHRM;
        private System.Windows.Forms.Button btnActivate;

    }
}