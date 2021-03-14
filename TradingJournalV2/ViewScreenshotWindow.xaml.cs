using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TradingJournalV2
{
    /// <summary>
    /// Interaction logic for ViewScreenshotWindow.xaml
    /// </summary>
    public partial class ViewScreenshotWindow : Window
    {
        private byte[] image { get; set; }
        public ViewScreenshotWindow(byte[] image)
        {
            InitializeComponent();
            this.image = image;

            ScreenshotImage.Source = LoadImage(image);
        }

        private BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var memoryStream =  new MemoryStream(imageData))
            {
                memoryStream.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = memoryStream;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}
