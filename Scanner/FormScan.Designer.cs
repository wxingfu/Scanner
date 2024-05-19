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
            this.components = new System.ComponentModel.Container();
            this.cbbDataSource = new System.Windows.Forms.ComboBox();
            this.bntScan = new System.Windows.Forms.Button();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.panelSacnMenu = new System.Windows.Forms.Panel();
            this.tabControlPreview = new System.Windows.Forms.TabControl();
            this.tabPageSmallPreview = new System.Windows.Forms.TabPage();
            this.DefaultPageControl = new Scanner.Visual.PageControl();
            this.DefaultListView = new System.Windows.Forms.ListView();
            this.DefaultImageList = new System.Windows.Forms.ImageList(this.components);
            this.tabPageVerticalPreview = new System.Windows.Forms.TabPage();
            this.tabPageHorizontalPreView = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.panelSacnMenu.SuspendLayout();
            this.tabControlPreview.SuspendLayout();
            this.tabPageSmallPreview.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbbDataSource
            // 
            this.cbbDataSource.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbbDataSource.FormattingEnabled = true;
            this.cbbDataSource.Location = new System.Drawing.Point(9, 10);
            this.cbbDataSource.Name = "cbbDataSource";
            this.cbbDataSource.Size = new System.Drawing.Size(146, 20);
            this.cbbDataSource.TabIndex = 1;
            // 
            // bntScan
            // 
            this.bntScan.Location = new System.Drawing.Point(9, 46);
            this.bntScan.Name = "bntScan";
            this.bntScan.Size = new System.Drawing.Size(60, 20);
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
            this.splitContainer.Margin = new System.Windows.Forms.Padding(2);
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
            this.splitContainer.Size = new System.Drawing.Size(1002, 773);
            this.splitContainer.SplitterDistance = 166;
            this.splitContainer.SplitterWidth = 3;
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
            this.panelSacnMenu.Margin = new System.Windows.Forms.Padding(2);
            this.panelSacnMenu.Name = "panelSacnMenu";
            this.panelSacnMenu.Size = new System.Drawing.Size(166, 773);
            this.panelSacnMenu.TabIndex = 0;
            // 
            // tabControlPreview
            // 
            this.tabControlPreview.Controls.Add(this.tabPageSmallPreview);
            this.tabControlPreview.Controls.Add(this.tabPageVerticalPreview);
            this.tabControlPreview.Controls.Add(this.tabPageHorizontalPreView);
            this.tabControlPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPreview.Location = new System.Drawing.Point(0, 0);
            this.tabControlPreview.Margin = new System.Windows.Forms.Padding(2);
            this.tabControlPreview.Name = "tabControlPreview";
            this.tabControlPreview.SelectedIndex = 0;
            this.tabControlPreview.Size = new System.Drawing.Size(833, 773);
            this.tabControlPreview.TabIndex = 0;
            // 
            // tabPageSmallPreview
            // 
            this.tabPageSmallPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPageSmallPreview.Controls.Add(this.DefaultPageControl);
            this.tabPageSmallPreview.Controls.Add(this.DefaultListView);
            this.tabPageSmallPreview.Location = new System.Drawing.Point(4, 22);
            this.tabPageSmallPreview.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageSmallPreview.Name = "tabPageSmallPreview";
            this.tabPageSmallPreview.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageSmallPreview.Size = new System.Drawing.Size(825, 747);
            this.tabPageSmallPreview.TabIndex = 0;
            this.tabPageSmallPreview.Text = "微缩视图";
            this.tabPageSmallPreview.UseVisualStyleBackColor = true;
            // 
            // DefaultPageControl
            // 
            this.DefaultPageControl.CurrentPage = 0;
            this.DefaultPageControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DefaultPageControl.Location = new System.Drawing.Point(2, 711);
            this.DefaultPageControl.Name = "DefaultPageControl";
            this.DefaultPageControl.PageSize = 0;
            this.DefaultPageControl.Size = new System.Drawing.Size(817, 30);
            this.DefaultPageControl.TabIndex = 1;
            this.DefaultPageControl.TotalPages = 0;
            // 
            // DefaultListView
            // 
            this.DefaultListView.Dock = System.Windows.Forms.DockStyle.Top;
            this.DefaultListView.HideSelection = false;
            this.DefaultListView.LargeImageList = this.DefaultImageList;
            this.DefaultListView.Location = new System.Drawing.Point(2, 2);
            this.DefaultListView.Name = "DefaultListView";
            this.DefaultListView.Size = new System.Drawing.Size(817, 703);
            this.DefaultListView.TabIndex = 0;
            this.DefaultListView.UseCompatibleStateImageBehavior = false;
            // 
            // DefaultImageList
            // 
            this.DefaultImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
            this.DefaultImageList.ImageSize = new System.Drawing.Size(100, 100);
            this.DefaultImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tabPageVerticalPreview
            // 
            this.tabPageVerticalPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPageVerticalPreview.Location = new System.Drawing.Point(4, 22);
            this.tabPageVerticalPreview.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageVerticalPreview.Name = "tabPageVerticalPreview";
            this.tabPageVerticalPreview.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageVerticalPreview.Size = new System.Drawing.Size(671, 615);
            this.tabPageVerticalPreview.TabIndex = 1;
            this.tabPageVerticalPreview.Text = "垂直视图";
            this.tabPageVerticalPreview.UseVisualStyleBackColor = true;
            // 
            // tabPageHorizontalPreView
            // 
            this.tabPageHorizontalPreView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPageHorizontalPreView.Location = new System.Drawing.Point(4, 22);
            this.tabPageHorizontalPreView.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageHorizontalPreView.Name = "tabPageHorizontalPreView";
            this.tabPageHorizontalPreView.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageHorizontalPreView.Size = new System.Drawing.Size(671, 615);
            this.tabPageHorizontalPreView.TabIndex = 2;
            this.tabPageHorizontalPreView.Text = "水平视图";
            this.tabPageHorizontalPreView.UseVisualStyleBackColor = true;
            // 
            // FormScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Highlight;
            this.ClientSize = new System.Drawing.Size(1002, 773);
            this.Controls.Add(this.splitContainer);
            this.Name = "FormScan";
            this.Text = "FormScan";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.panelSacnMenu.ResumeLayout(false);
            this.tabControlPreview.ResumeLayout(false);
            this.tabPageSmallPreview.ResumeLayout(false);
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
        private System.Windows.Forms.ListView DefaultListView;
        private System.Windows.Forms.ImageList DefaultImageList;
        private Visual.PageControl DefaultPageControl;
    }
}

