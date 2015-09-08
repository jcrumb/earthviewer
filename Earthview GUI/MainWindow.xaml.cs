using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using EarthView;

namespace Earthview_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private EarthView.ImageViewer view;
        public MainWindow()
        {
            InitializeComponent();

            view = new ImageViewer("/_api/hamburg-germany-5646.json");
            this.currentImage.Source = view.GetImage();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.view.Next();
            this.currentImage.Source = view.GetImage();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.view.SetWallpaper();
        }

    }
}
