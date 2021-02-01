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
using CefSharp;
using CefSharp.Wpf;
using YouTubePlayer.Chrome;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;

namespace YouTubePlayer
{
    /// <summary>
    /// Player.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChromeWebPannel : Window
    {
        const string DefaultUrlForAddedTabs = "https://www.google.com/";
        public ChromeBrowserViewModel CBVM;
        public ChromeWebPannel()
        {
            InitializeComponent();

            // Initialize this ViewModel using the constructor
            CBVM = new ChromeBrowserViewModel(DefaultUrlForAddedTabs)
            {
                ShowSidebar = false
            };
            DataContext = CBVM;
            Loaded += MainWindowLoaded;
        }
        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            CreateNewTab(DefaultUrlForAddedTabs, true);
        }
        private void CreateNewTab(string url = DefaultUrlForAddedTabs, bool showSideBar = false)
        {
            CBVM = new ChromeBrowserViewModel(url) { ShowSidebar = showSideBar };
        }
    }
}
