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
using System.Diagnostics;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace EarthView
{
    public class ImageViewer
    {
        private string nextEndpoint;
        private BitmapImage image;

        public ImageViewer(string startingPoint)
        {
            this.nextEndpoint = startingPoint;
            this.Next();
        }

        public void Next()
        {
            var httpClient = new WebClient();
            var endpoint = new
            {
                PhotoUrl = "",
                NextApi = "",
            };

            var apiResponse = Encoding.UTF8.GetString(httpClient.DownloadData("https://earthview.withgoogle.com" + this.nextEndpoint));
            var photoInfo = JsonConvert.DeserializeAnonymousType(apiResponse, endpoint);

            this.nextEndpoint = photoInfo.NextApi;
            this.UpdateImage(photoInfo.PhotoUrl);
            Debug.WriteLine(photoInfo.NextApi);
        }


        // Required for setting the wallpaper, there is currently no way to do this in managed code
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public void SetWallpaper()
        {
            var encoder = new BmpBitmapEncoder();
            string tempPath = Path.Combine(Path.GetTempPath(), "earthview_wall.bmp");

            encoder.Frames.Add(BitmapFrame.Create(this.image));

            using (var fileStream = new FileStream(tempPath, FileMode.Create))
            {
                encoder.Save(fileStream);
            }

            const int SPI_SETDESKWALLPAPER = 20;
            const int SPIF_UPDATEINIFILE = 0x01;
            const int SPIF_SENDWININICHANGE = 0x02;

            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, tempPath, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }

        private void UpdateImage(string url)
        {
            var webClient = new WebClient();
            var bytes = webClient.DownloadData(url);

            MemoryStream ms = new MemoryStream(bytes);

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = ms;
            bitmap.EndInit();

            this.image = bitmap;
        }

        public ImageSource GetImage()
        {
            return this.image as ImageSource;
        }

        public string GetNextEndpoint()
        {
            return this.nextEndpoint;
        }
    }
}
