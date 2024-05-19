namespace Scanner.Visual
{
    partial class PageControl
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

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnFrist = new System.Windows.Forms.Button();
            this.btnPreviou = new System.Windows.Forms.Button();
            this.lblPage = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnFrist
            // 
            this.btnFrist.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnFrist.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFrist.Location = new System.Drawing.Point(3, 3);
            this.btnFrist.Name = "btnFrist";
            this.btnFrist.Size = new System.Drawing.Size(39, 22);
            this.btnFrist.TabIndex = 0;
            this.btnFrist.Text = "<<";
            this.btnFrist.UseVisualStyleBackColor = true;
            // 
            // btnPreviou
            // 
            this.btnPreviou.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPreviou.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPreviou.Location = new System.Drawing.Point(54, 3);
            this.btnPreviou.Name = "btnPreviou";
            this.btnPreviou.Size = new System.Drawing.Size(39, 22);
            this.btnPreviou.TabIndex = 1;
            this.btnPreviou.Text = "<";
            this.btnPreviou.UseVisualStyleBackColor = true;
            // 
            // lblPage
            // 
            this.lblPage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblPage.AutoSize = true;
            this.lblPage.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPage.Location = new System.Drawing.Point(105, 8);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(52, 12);
            this.lblPage.TabIndex = 2;
            this.lblPage.Text = "第1/1页";
            // 
            // btnNext
            // 
            this.btnNext.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnNext.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnNext.Location = new System.Drawing.Point(169, 3);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(39, 22);
            this.btnNext.TabIndex = 3;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // btnLast
            // 
            this.btnLast.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnLast.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLast.Location = new System.Drawing.Point(220, 3);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(39, 22);
            this.btnLast.TabIndex = 4;
            this.btnLast.Text = ">>";
            this.btnLast.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnLast);
            this.panel1.Controls.Add(this.btnFrist);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnPreviou);
            this.panel1.Controls.Add(this.lblPage);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(265, 30);
            this.panel1.TabIndex = 5;
            // 
            // PageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "PageControl";
            this.Size = new System.Drawing.Size(265, 30);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnFrist;
        private System.Windows.Forms.Button btnPreviou;
        private System.Windows.Forms.Label lblPage;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Panel panel1;
    }
}
