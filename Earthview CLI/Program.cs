using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EarthView;
using Microsoft.Win32;
using System.Net;
using System.Diagnostics;

namespace Earthview_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Registry.CurrentUser.GetValue("EarthViewEndpoint") == null)
            {
                Registry.CurrentUser.SetValue("EarthViewEndpoint", "/_api/hamburg-germany-5646.json");
            }

            try
            {
                var seed = Registry.CurrentUser.GetValue("EarthViewEndpoint");
                var view = new EarthView.ImageViewer(seed.ToString());
                view.Next();
                Registry.CurrentUser.SetValue("EarthViewEndpoint", view.GetNextEndpoint());
                view.SetWallpaper();
            }

            catch (WebException e)
            {
                Debug.WriteLine(e.ToString());
            }

        }
    }
}
