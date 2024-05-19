using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using log4net;
using Scanner.Toolkit;
using Scanner.Twain;

namespace Scanner
{
    public partial class FormScan : Form
    {

        static ILog log = LogManager.GetLogger(typeof(FormScan));

        // 图片存放文件夹
        private static string fileBasePath = FilePath.PathCombine(FilePath.GetAppPath(), "\\image");

        // 图片后缀
        private static string imageSuffix = FilePath.GetImageFormatExtension(ImageFormat.Tiff);

        // 扫描到的图片路径
        private List<string> imagePathList = new List<string>();

        // 分页图片路径
        private Dictionary<int, List<string>> currentImagePageList = new Dictionary<int, List<string>>();

        // 分页页大小
        private int pageSize = 20;

        //private static AreaSettings AreaSettings = new AreaSettings(Units.Centimeters, 0.1f, 5.7f, 0.1F + 2.6f, 5.7f + 2.6f);

        // Twain SDK
        TwainControl _twain;

        public FormScan()
        {

            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;


            # region 初始化翻页控件

            this.DefaultPageControl.CurrentPage = 1;
            this.DefaultPageControl.PageSize = 10;
            this.DefaultPageControl.TotalPages = 1;
            this.DefaultPageControl.ClickPageButtonEvent += DefaultPageControl_ClickPageButtonEvent;

            # endregion


            # region 初始化控件缩放

            x = Width;
            y = Height;
            SetTag(this);
            this.Resize += FormScan_Resize;

            #endregion


            # region 加载扫描仪

            LoadScanner();

            # endregion


            // 清空临时目录
            DirectoryInfo directoryInfo = new DirectoryInfo(fileBasePath);
            foreach (var item in directoryInfo.GetFiles())
            {
                FilePath.DeleteFile(item.FullName);
            }

        }


        #region 扫描处理

        /// <summary>
        /// 初始化扫描仪加载
        /// </summary>
        private void LoadScanner()
        {
            try
            {
                _twain = new TwainControl(new WinFormsWindowMessageHook(this));
            }
            catch (Exception ex)
            {
                //MessageBox.Show("未连接到扫描仪");
                return;
            }

            _twain.TransferImage += delegate (Object sender, TransferImageEventArgs args)
            {
                Bitmap image = args.Image;
                if (image != null)
                {
                    string imageName = Guid.NewGuid().ToString() + imageSuffix;
                    string imagePath = fileBasePath + imageName;
                    log.Info("basePath: " + fileBasePath + ", imageName: " + imageName + ", imagePath: " + imagePath);

                    image.Save(imagePath);
                }
            };

            _twain.ScanningComplete += delegate
            {
                Enabled = true;

                // 初始化图片显示
                DefaultShowImage();
            };


            List<string> sourceNames = _twain.GetSourceNames();
            if (sourceNames != null && sourceNames.Count > 0)
            {
                //下拉框绑定数据源并默认选中第一个
                cbbDataSource.DataSource = sourceNames;
                cbbDataSource.SelectedIndex = 0;
            }

        }


        /// <summary>
        /// 扫描按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntScan_Click(object sender, EventArgs e)
        {
            Enabled = false;

            //获取选中的数据源
            object item = cbbDataSource.SelectedItem;
            string sourceName = cbbDataSource.GetItemText(item);
            if (string.IsNullOrEmpty(sourceName))
            {
                MessageBox.Show("没有可用的扫描仪");
                return;
            }
            log.Info("选中的数据源是: " + sourceName);

            _twain.SelectSource(sourceName);

            ScanSettings _settings = new ScanSettings();
            _settings.UseDocumentFeeder = true;
            _settings.ShowTwainUI = false;
            _settings.ShowProgressIndicatorUI = false;
            _settings.UseDuplex = true;
            _settings.Resolution = ResolutionSettings.ColourPhotocopier;
            _settings.Area = null;
            _settings.ShouldTransferAllPages = true;

            _settings.Rotation = new RotationSettings()
            {
                AutomaticRotate = false,
                AutomaticBorderDetection = false
            };

            try
            {
                log.Info("ScanSettings: " + _settings.ToString());
                _twain.StartScanning(_settings);
            }
            catch (TwainException ex)
            {
                MessageBox.Show(ex.Message);
                Enabled = true;
            }
        }

        # endregion


        # region 图像显示和翻页

        /// <summary>
        /// 初始化扫描到的图像显示
        /// </summary>
        private void DefaultShowImage()
        {
            this.imagePathList = FilePath.GetPictureList(fileBasePath);
            this.currentImagePageList.Clear();
            List<string> currentImagePage = new List<string>();
            int pageNo = 1;
            foreach (string imagePath in imagePathList)
            {
                currentImagePage.Add(imagePath);
                // 每达到10条数据，就添加到字典中并重置当前组
                if (currentImagePage.Count == pageSize)
                {
                    this.currentImagePageList.Add(pageNo, currentImagePage);
                    pageNo++;
                    // 重置当前组以准备下一轮数据
                    currentImagePage = new List<string>();
                }
            }

            // 处理最后一组数据，如果它的大小小于pageSize
            if (currentImagePage.Count > 0)
            {
                this.currentImagePageList.Add(pageNo, currentImagePage);
            }

            DefaultShowImage(1);
        }

        /// <summary>
        /// 根据当前页码显示
        /// </summary>
        /// <param name="currentPage"></param>
        private void DefaultShowImage(int currentPage)
        {
            this.DefaultImageList.Images.Clear();

            // 没数据时，点翻页无用
            if (this.currentImagePageList.Count == 0)
            {
                return;
            }

            // 总页数小于当前页，则取总页数
            if (this.currentImagePageList.Count <= currentPage)
            {
                currentPage = this.currentImagePageList.Count;
            }

            List<string> pageImageList = this.currentImagePageList[currentPage];
            pageImageList.ForEach(image =>
            {
                Image img = Image.FromFile(image);
                this.DefaultImageList.Images.Add(img);
                // 释放内存，防止溢出
                img.Dispose();
            });

            int startIndex = 0;
            if (currentPage == 1)
            {
                // 第一页编号从1开始
                startIndex = 1;
            }
            else
            {
                // 编号从前一组最大号开始
                startIndex = (currentPage - 1) * this.currentImagePageList[currentPage - 1].Count + 1;
            }

            this.DefaultListView.Items.Clear();
            this.DefaultListView.LargeImageList = this.DefaultImageList;
            this.DefaultListView.View = View.LargeIcon;
            //开始绑定
            this.DefaultListView.BeginUpdate();
            //增加图片至ListView控件中
            for (int i = 0; i < this.DefaultImageList.Images.Count; i++)
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.ImageIndex = i;
                listViewItem.Text = "" + startIndex;
                this.DefaultListView.Items.Add(listViewItem);
                startIndex++;
            }
            this.DefaultListView.EndUpdate();

            // 设置翻页参数
            this.DefaultPageControl.PageInfo.Text = string.Format("第{0}/{1}页", currentPage, this.currentImagePageList.Count);
            this.DefaultPageControl.CurrentPage = currentPage;
            this.DefaultPageControl.TotalPages = this.currentImagePageList.Count;
        }

        /// <summary>
        /// 页数改变按钮(最前页, 上一页, 最后页,下一页)
        /// </summary>
        /// <param name="current">当前页码</param>
        void DefaultPageControl_ClickPageButtonEvent(int current)
        {
            this.DefaultShowImage(current);
        }

        #endregion


        #region 控件大小随窗体大小等比例缩放

        //定义当前窗体的宽度
        private readonly float x;

        //定义当前窗体的高度
        private readonly float y;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cons"></param>
        private void SetTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ";" + con.Height + ";" + con.Left + ";" + con.Top + ";" + con.Font.Size;
                if (con.Controls.Count > 0)
                {
                    SetTag(con);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newx"></param>
        /// <param name="newy"></param>
        /// <param name="cons"></param>
        private void SetControls(float newx, float newy, Control cons)
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (Control con in cons.Controls)
                //获取控件的Tag属性值，并分割后存储字符串数组
                if (con.Tag != null)
                {
                    var mytag = con.Tag.ToString().Split(';');
                    //根据窗体缩放的比例确定控件的值
                    //宽度
                    con.Width = Convert.ToInt32(Convert.ToSingle(mytag[0]) * newx);
                    //高度
                    con.Height = Convert.ToInt32(Convert.ToSingle(mytag[1]) * newy);
                    //左边距
                    con.Left = Convert.ToInt32(Convert.ToSingle(mytag[2]) * newx);
                    //顶边距
                    con.Top = Convert.ToInt32(Convert.ToSingle(mytag[3]) * newy);
                    //字体大小
                    var currentSize = Convert.ToSingle(mytag[4]) * newy;

                    if (currentSize > 0)
                    {
                        con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    }

                    con.Focus();

                    if (con.Controls.Count > 0)
                    {
                        SetControls(newx, newy, con);
                    }
                }
        }


        /// <summary>
        /// 重置窗体布局
        /// </summary>
        private void ReWinformLayout()
        {
            var newx = Width / x;
            var newy = Height / y;
            SetControls(newx, newy, this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormScan_Resize(object sender, EventArgs e)
        {
            //重置窗口布局
            ReWinformLayout();
        }

        #endregion


    }
}
