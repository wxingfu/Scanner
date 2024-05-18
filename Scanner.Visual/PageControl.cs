using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scanner.Visual
{
    public partial class PageControl : UserControl
    {
        public delegate void ClickPageButton(int current);

        public event ClickPageButton ClickPageButtonEvent;

        public delegate void ChangedPageSize();
        public event ChangedPageSize ChangedPageSizeEvent;

        public int TotalPages { get; set; }

        private int currentPage;
        public int CurrentPage
        {
            get { return this.currentPage; }
            set { this.currentPage = value; }
        }

        private int pageSize;
        public int PageSize
        {
            get { return this.pageSize; }
            set { this.pageSize = value; }
        }

        public Label PageInfo
        {
            set { this.lblPage = value; }
            get { return this.lblPage; }
        }


        public PageControl()
        {
            InitializeComponent();

            //this.btnFrist.Tag = "F";
            //this.btnPreviou.Tag = "P";
            //this.btnNext.Tag = "N";
            //this.btnLast.Tag = "L";

            this.btnFrist.Click += btn_Click;
            this.btnPreviou.Click += btn_Click;
            this.btnNext.Click += btn_Click;
            this.btnLast.Click += btn_Click;
        }


        void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (this.ClickPageButtonEvent != null)
            {
                if (null != btn)
                {
                    switch (btn.Text)
                    {
                        case "<<":
                            this.CurrentPage = 1;
                            break;
                        case "<":
                            this.CurrentPage = this.CurrentPage <= 1 ? 1 : this.CurrentPage - 1;
                            break;
                        case ">":
                            this.CurrentPage = this.CurrentPage + 1;
                            break;
                        case ">>":
                            this.CurrentPage = this.TotalPages;
                            break;
                        default:
                            this.CurrentPage = 1;
                            break;
                    }
                    this.ClickPageButtonEvent(this.CurrentPage);
                }
            }
        }
    }
}
