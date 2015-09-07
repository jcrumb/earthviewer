using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Media.Imaging;

namespace EarthView
{
    public class EarthView
    {
        public string URL { get; set; }

        public EarthView(string url)
        {
            this.URL = url;
        }

        public ImageSource GetImage()
        {
            var webClient = new WebClient();
            var bytes = webClient.DownloadData(this.URL);

            MemoryStream ms = new MemoryStream(bytes);

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = ms;
            bitmap.EndInit();

            return bitmap as ImageSource;
        }
    }
}
