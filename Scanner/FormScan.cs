using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection.Emit;
using System.Windows.Forms;
using log4net;
using Scanner.Toolkit;
using Scanner.Twain;

namespace Scanner
{
    public partial class FormScan : Form
    {

        static ILog log = LogManager.GetLogger(typeof(FormScan));

        private static string fileBasePath = FilePath.PathCombine(FilePath.GetAppPath(), "\\image");

        private static string imageSuffix = FilePath.GetImageFormatExtension(ImageFormat.Tiff);

        private List<string> imagePathList = new List<string>();

        private static AreaSettings AreaSettings = new AreaSettings(Units.Centimeters, 0.1f, 5.7f, 0.1F + 2.6f, 5.7f + 2.6f);

        TwainControl _twain;

        ScanSettings _settings;

        public FormScan()
        {
            InitializeComponent();

            #region   初始化控件缩放
            x = Width;
            y = Height;
            SetTag(this);
            this.Resize += FormScan_Resize;
            #endregion

            Init();
        }

        #region 扫描处理

        /// <summary>
        /// 
        /// </summary>
        private void Init()
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

                    // 记录图像地址
                    imagePathList.Add(imagePath);
                }
            };

            _twain.ScanningComplete += delegate
            {
                Enabled = true;
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
        /// 
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

            _settings = new ScanSettings();
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
