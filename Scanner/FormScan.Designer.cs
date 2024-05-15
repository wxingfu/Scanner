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
            this.SuspendLayout();
            // 
            // cbbDataSource
            // 
            this.cbbDataSource.FormattingEnabled = true;
            this.cbbDataSource.Location = new System.Drawing.Point(17, 15);
            this.cbbDataSource.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbbDataSource.Name = "cbbDataSource";
            this.cbbDataSource.Size = new System.Drawing.Size(337, 23);
            this.cbbDataSource.TabIndex = 1;
            // 
            // bntScan
            // 
            this.bntScan.Location = new System.Drawing.Point(404, 15);
            this.bntScan.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bntScan.Name = "bntScan";
            this.bntScan.Size = new System.Drawing.Size(80, 25);
            this.bntScan.TabIndex = 2;
            this.bntScan.Text = "扫描";
            this.bntScan.UseVisualStyleBackColor = true;
            this.bntScan.Click += new System.EventHandler(this.bntScan_Click);
            // 
            // FormScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1246, 721);
            this.Controls.Add(this.bntScan);
            this.Controls.Add(this.cbbDataSource);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormScan";
            this.Text = "FormScan";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox cbbDataSource;
        private System.Windows.Forms.Button bntScan;
    }
}

