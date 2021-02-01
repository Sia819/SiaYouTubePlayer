
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace YouTubePlayer
{
    /// <summary>
    /// chrome.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChromeBrowser : UserControl
    {
        
        //const string DefaultUrlForAddedTabs = "https://www.google.com/";

        //ChromeBrowserViewModel ChromeViewModel;

        public ChromeBrowser()
        {
            InitializeComponent();
            //ChromeViewModel = new ChromeBrowserViewModel("https://www.google.com/");
            // this.DataContext = ChromeViewModel;
        }
    }

    
}
