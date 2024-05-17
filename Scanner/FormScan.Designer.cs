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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.panelSacnMenu = new System.Windows.Forms.Panel();
            this.tabControlPreview = new System.Windows.Forms.TabControl();
            this.tabPageSmallPreview = new System.Windows.Forms.TabPage();
            this.tabPageVerticalPreview = new System.Windows.Forms.TabPage();
            this.tabPageHorizontalPreView = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.panelSacnMenu.SuspendLayout();
            this.tabControlPreview.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbbDataSource
            // 
            this.cbbDataSource.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbbDataSource.FormattingEnabled = true;
            this.cbbDataSource.Location = new System.Drawing.Point(12, 12);
            this.cbbDataSource.Margin = new System.Windows.Forms.Padding(4);
            this.cbbDataSource.Name = "cbbDataSource";
            this.cbbDataSource.Size = new System.Drawing.Size(226, 23);
            this.cbbDataSource.TabIndex = 1;
            // 
            // bntScan
            // 
            this.bntScan.Location = new System.Drawing.Point(12, 58);
            this.bntScan.Margin = new System.Windows.Forms.Padding(4);
            this.bntScan.Name = "bntScan";
            this.bntScan.Size = new System.Drawing.Size(80, 25);
            this.bntScan.TabIndex = 2;
            this.bntScan.Text = "扫描";
            this.bntScan.UseVisualStyleBackColor = true;
            this.bntScan.Click += new System.EventHandler(this.bntScan_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer.Panel1.Controls.Add(this.panelSacnMenu);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.tabControlPreview);
            this.splitContainer.Size = new System.Drawing.Size(1411, 698);
            this.splitContainer.SplitterDistance = 286;
            this.splitContainer.TabIndex = 3;
            // 
            // panelSacnMenu
            // 
            this.panelSacnMenu.BackColor = System.Drawing.SystemColors.Control;
            this.panelSacnMenu.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelSacnMenu.Controls.Add(this.bntScan);
            this.panelSacnMenu.Controls.Add(this.cbbDataSource);
            this.panelSacnMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSacnMenu.Location = new System.Drawing.Point(0, 0);
            this.panelSacnMenu.Name = "panelSacnMenu";
            this.panelSacnMenu.Size = new System.Drawing.Size(286, 698);
            this.panelSacnMenu.TabIndex = 0;
            // 
            // tabControlPreview
            // 
            this.tabControlPreview.Controls.Add(this.tabPageSmallPreview);
            this.tabControlPreview.Controls.Add(this.tabPageVerticalPreview);
            this.tabControlPreview.Controls.Add(this.tabPageHorizontalPreView);
            this.tabControlPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPreview.Location = new System.Drawing.Point(0, 0);
            this.tabControlPreview.Name = "tabControlPreview";
            this.tabControlPreview.SelectedIndex = 0;
            this.tabControlPreview.Size = new System.Drawing.Size(1121, 698);
            this.tabControlPreview.TabIndex = 0;
            // 
            // tabPageSmallPreview
            // 
            this.tabPageSmallPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPageSmallPreview.Location = new System.Drawing.Point(4, 25);
            this.tabPageSmallPreview.Name = "tabPageSmallPreview";
            this.tabPageSmallPreview.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSmallPreview.Size = new System.Drawing.Size(1113, 669);
            this.tabPageSmallPreview.TabIndex = 0;
            this.tabPageSmallPreview.Text = "微缩视图";
            this.tabPageSmallPreview.UseVisualStyleBackColor = true;
            // 
            // tabPageVerticalPreview
            // 
            this.tabPageVerticalPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPageVerticalPreview.Location = new System.Drawing.Point(4, 25);
            this.tabPageVerticalPreview.Name = "tabPageVerticalPreview";
            this.tabPageVerticalPreview.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageVerticalPreview.Size = new System.Drawing.Size(1113, 669);
            this.tabPageVerticalPreview.TabIndex = 1;
            this.tabPageVerticalPreview.Text = "垂直视图";
            this.tabPageVerticalPreview.UseVisualStyleBackColor = true;
            // 
            // tabPageHorizontalPreView
            // 
            this.tabPageHorizontalPreView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPageHorizontalPreView.Location = new System.Drawing.Point(4, 25);
            this.tabPageHorizontalPreView.Name = "tabPageHorizontalPreView";
            this.tabPageHorizontalPreView.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHorizontalPreView.Size = new System.Drawing.Size(1113, 669);
            this.tabPageHorizontalPreView.TabIndex = 2;
            this.tabPageHorizontalPreView.Text = "水平视图";
            this.tabPageHorizontalPreView.UseVisualStyleBackColor = true;
            // 
            // FormScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Highlight;
            this.ClientSize = new System.Drawing.Size(1411, 698);
            this.Controls.Add(this.splitContainer);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormScan";
            this.Text = "FormScan";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.panelSacnMenu.ResumeLayout(false);
            this.tabControlPreview.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox cbbDataSource;
        private System.Windows.Forms.Button bntScan;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Panel panelSacnMenu;
        private System.Windows.Forms.TabControl tabControlPreview;
        private System.Windows.Forms.TabPage tabPageSmallPreview;
        private System.Windows.Forms.TabPage tabPageVerticalPreview;
        private System.Windows.Forms.TabPage tabPageHorizontalPreView;
    }
}

