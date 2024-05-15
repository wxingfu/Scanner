namespace Scanner
{
    partial class FormScan
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.cbbDataSource = new System.Windows.Forms.ComboBox();
            this.bntScan = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // cbbDataSource
            // 
            this.cbbDataSource.FormattingEnabled = true;
            this.cbbDataSource.Location = new System.Drawing.Point(13, 12);
            this.cbbDataSource.Name = "cbbDataSource";
            this.cbbDataSource.Size = new System.Drawing.Size(254, 20);
            this.cbbDataSource.TabIndex = 1;
            // 
            // bntScan
            // 
            this.bntScan.Location = new System.Drawing.Point(303, 12);
            this.bntScan.Name = "bntScan";
            this.bntScan.Size = new System.Drawing.Size(60, 20);
            this.bntScan.TabIndex = 2;
            this.bntScan.Text = "扫描";
            this.bntScan.UseVisualStyleBackColor = true;
            this.bntScan.Click += new System.EventHandler(this.bntScan_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Location = new System.Drawing.Point(13, 78);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(779, 437);
            this.panel1.TabIndex = 3;
            // 
            // FormScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 577);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.bntScan);
            this.Controls.Add(this.cbbDataSource);
            this.Name = "FormScan";
            this.Text = "FormScan";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox cbbDataSource;
        private System.Windows.Forms.Button bntScan;
        private System.Windows.Forms.FlowLayoutPanel panel1;
    }
}

