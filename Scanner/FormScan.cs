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

        List<PictureBox> pictureBoxes = new List<PictureBox>();

        private static AreaSettings AreaSettings = new AreaSettings(Units.Centimeters, 0.1f, 5.7f, 0.1F + 2.6f, 5.7f + 2.6f);

        TwainControl _twain;

        ScanSettings _settings;

        public FormScan()
        {
            InitializeComponent();

            Init();
        }


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
                    //string imageName = Guid.NewGuid().ToString() + imageSuffix;
                    //string imagePath = fileBasePath + imageName;
                    //log.Info("basePath: " + fileBasePath + ", imageName: " + imageName + ", imagePath: " + imagePath);

                    //image.Save(imagePath);
                    PictureBox picture = new PictureBox();
                    picture.Image = image;
                    pictureBoxes.Add(picture);
                }
            };

            _twain.ScanningComplete += delegate
            {
                Enabled = true;

                pictureBoxes.ForEach(pictureBox =>
                {
                    Image thumbnailImage = pictureBox.Image.GetThumbnailImage(200, 200, new Image.GetThumbnailImageAbort(GetThumbnailImageCallBack), new IntPtr());
                    PictureBox thumbnailBox = new PictureBox();
                    thumbnailBox.Image = thumbnailImage;
                    thumbnailBox.SizeMode = PictureBoxSizeMode.AutoSize;
                    panel1.Controls.Add(thumbnailBox);
                });
            };


            List<string> sourceNames = _twain.GetSourceNames();
            if (sourceNames != null && sourceNames.Count > 0)
            {
                cbbDataSource.DataSource = sourceNames;
                cbbDataSource.SelectedIndex = 0;

                log.Info("DataSource: " + cbbDataSource.SelectedItem.ToString());

                _twain.SelectSource(cbbDataSource.SelectedItem.ToString());
            }

        }

        private bool GetThumbnailImageCallBack()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntScan_Click(object sender, EventArgs e)
        {
            Enabled = false;

            pictureBoxes.Clear();
            panel1.Controls.Clear();

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


    }
}
