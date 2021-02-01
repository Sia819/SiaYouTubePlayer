using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CefSharp;
using CefSharp.Wpf;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace YouTubePlayer.Chrome
{
    public class ChromeBrowserViewModel : ViewModelBase
    {
        private string address;
        public bool IsInitialized = false;
        public string Address
        {
            get { return address; }
            set { Set(ref address, value); }
        }

        private string addressEditable;
        public string AddressEditable
        {
            get { return addressEditable; }
            set { Set(ref addressEditable, value); }
        }

        private string outputMessage;
        public string OutputMessage
        {
            get { return outputMessage; }
            set { Set(ref outputMessage, value); }
        }

        private string statusMessage;
        public string StatusMessage
        {
            get { return statusMessage; }
            set { Set(ref statusMessage, value); }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { Set(ref title, value); }
        }

        private IWpfWebBrowser webBrowser;
        public IWpfWebBrowser WebBrowser
        {
            get { return webBrowser; }
            set { Set(ref webBrowser, value); }
        }

        private object evaluateJavaScriptResult;
        public object EvaluateJavaScriptResult
        {
            get { return evaluateJavaScriptResult; }
            set { Set(ref evaluateJavaScriptResult, value); }
        }

        private bool showSidebar;
        public bool ShowSidebar
        {
            get { return showSidebar; }
            set { Set(ref showSidebar, value); }
        }

        private bool showDownloadInfo;
        public bool ShowDownloadInfo
        {
            get { return showDownloadInfo; }
            set { Set(ref showDownloadInfo, value); }
        }

        private string lastDownloadAction;
        public string LastDownloadAction
        {
            get { return lastDownloadAction; }
            set { Set(ref lastDownloadAction, value); }
        }

        private DownloadItem downloadItem;
        public DownloadItem DownloadItem
        {
            get { return downloadItem; }
            set { Set(ref downloadItem, value); }
        }

        public ICommand GoCommand { get; private set; }
        public ICommand HomeCommand { get; private set; }
        public ICommand ExecuteJavaScriptCommand { get; private set; }
        public ICommand EvaluateJavaScriptCommand { get; private set; }
        public ICommand ShowDevToolsCommand { get; private set; }
        public ICommand CloseDevToolsCommand { get; private set; }
        public ICommand JavascriptBindingStressTest { get; private set; }

        public ChromeBrowserViewModel(string address)
        {
            string DefaultUrl = "https://www.google.com/";
            //InitializeChrome(address);
            Address = address;
            AddressEditable = Address;

            GoCommand = new RelayCommand(Go, () => !String.IsNullOrWhiteSpace(Address));
            HomeCommand = new RelayCommand(() => AddressEditable = Address = DefaultUrl);
            ExecuteJavaScriptCommand = new RelayCommand<string>(ExecuteJavaScript, s => !String.IsNullOrWhiteSpace(s));
            EvaluateJavaScriptCommand = new RelayCommand<string>(EvaluateJavaScript, s => !String.IsNullOrWhiteSpace(s));
            ShowDevToolsCommand = new RelayCommand(() => webBrowser.ShowDevTools());
            CloseDevToolsCommand = new RelayCommand(() => webBrowser.CloseDevTools());
            JavascriptBindingStressTest = new RelayCommand(() =>
            {
                WebBrowser.Load("https://cefsharp.example/BindingTest.html");//CefExample.BindingTestUrl);
                WebBrowser.LoadingStateChanged += (e, args) =>
                {
                    if (args.IsLoading == false)
                    {
                        Task.Delay(10000).ContinueWith(t =>
                        {
                            if (WebBrowser != null)
                            {
                                WebBrowser.Reload();
                            }
                        });
                    }
                };
            });

            PropertyChanged += OnPropertyChanged;

            var version = string.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);
            OutputMessage = version;
        }

        void InitializeChrome(string Address)
        {
            if (!IsInitialized)
            {
                Address = address;
                AddressEditable = Address;

                GoCommand = new RelayCommand(Go, () => !String.IsNullOrWhiteSpace(Address));
                HomeCommand = new RelayCommand(() => AddressEditable = this.Address = Address);
                ExecuteJavaScriptCommand = new RelayCommand<string>(ExecuteJavaScript, s => !String.IsNullOrWhiteSpace(s));
                EvaluateJavaScriptCommand = new RelayCommand<string>(EvaluateJavaScript, s => !String.IsNullOrWhiteSpace(s));
                ShowDevToolsCommand = new RelayCommand(() => webBrowser.ShowDevTools());
                CloseDevToolsCommand = new RelayCommand(() => webBrowser.CloseDevTools());
                JavascriptBindingStressTest = new RelayCommand(() =>
                {
                    //WebBrowser.Load(CefExample.BindingTestUrl);//();
                    WebBrowser.LoadingStateChanged += (e, args) =>
                    {
                        if (args.IsLoading == false)
                        {
                            Task.Delay(10000).ContinueWith(t =>
                            {
                                if (WebBrowser != null)
                                {
                                    WebBrowser.Reload();
                                }
                            });
                        }
                    };
                });
                PropertyChanged += OnPropertyChanged;
                var version = string.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);
                OutputMessage = version;
                IsInitialized = true;
            }
        }

        private async void EvaluateJavaScript(string s)
        {
            try
            {
                var response = await webBrowser.EvaluateScriptAsync(s);
                if (response.Success && response.Result is IJavascriptCallback)
                {
                    response = await ((IJavascriptCallback)response.Result).ExecuteAsync("This is a callback from EvaluateJavaScript");
                }

                EvaluateJavaScriptResult = response.Success ? (response.Result ?? "null") : response.Message;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while evaluating Javascript: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteJavaScript(string s)
        {
            try
            {
                webBrowser.ExecuteScriptAsync(s);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while executing Javascript: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Address":
                    AddressEditable = Address;
                    break;

                case "WebBrowser":
                    if (WebBrowser != null)
                    {
                        WebBrowser.ConsoleMessage += OnWebBrowserConsoleMessage;
                        WebBrowser.StatusMessage += OnWebBrowserStatusMessage;

                        // TODO: This is a bit of a hack. It would be nicer/cleaner to give the webBrowser focus in the Go()
                        // TODO: method, but it seems like "something" gets messed up (= doesn't work correctly) if we give it
                        // TODO: focus "too early" in the loading process...
                        WebBrowser.FrameLoadEnd += (s, args) =>
                        {
                            //Sender is the ChromiumWebBrowser object 
                            var browser = s as ChromiumWebBrowser;
                            if (browser != null && !browser.IsDisposed)
                            {
                                browser.Dispatcher.BeginInvoke((Action)(() => browser.Focus()));
                            }
                        };
                    }

                    break;
            }
        }

        private void OnWebBrowserConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            OutputMessage = e.Message;
        }

        private void OnWebBrowserStatusMessage(object sender, StatusMessageEventArgs e)
        {
            StatusMessage = e.Value;
        }

        private void Go()
        {
            Address = AddressEditable;

            // Part of the Focus hack further described in the OnPropertyChanged() method...
            Keyboard.ClearFocus();
        }

        public void LoadCustomRequestExample()
        {
            var postData = System.Text.Encoding.Default.GetBytes("test=123&data=456");

            WebBrowser.LoadUrlWithPostData("https://cefsharp.com/PostDataTest.html", postData);
        }
    }
}






















namespace YouTubePlayer.Chrome
{
    public class BrowserTabViewModel : ViewModelBase
    {
        public const string DefaultUrl = "https://www.google.com/";
        private string address;
        public string Address
        {
            get { return address; }
            set { Set(ref address, value); }
        }

        private string addressEditable;
        public string AddressEditable
        {
            get { return addressEditable; }
            set { Set(ref addressEditable, value); }
        }

        private string outputMessage;
        public string OutputMessage
        {
            get { return outputMessage; }
            set { Set(ref outputMessage, value); }
        }

        private string statusMessage;
        public string StatusMessage
        {
            get { return statusMessage; }
            set { Set(ref statusMessage, value); }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { Set(ref title, value); }
        }

        private IWpfWebBrowser webBrowser;
        public IWpfWebBrowser WebBrowser
        {
            get { return webBrowser; }
            set { Set(ref webBrowser, value); }
        }

        private object evaluateJavaScriptResult;
        public object EvaluateJavaScriptResult
        {
            get { return evaluateJavaScriptResult; }
            set { Set(ref evaluateJavaScriptResult, value); }
        }

        private bool showSidebar;
        public bool ShowSidebar
        {
            get { return showSidebar; }
            set { Set(ref showSidebar, value); }
        }

        private bool showDownloadInfo;
        public bool ShowDownloadInfo
        {
            get { return showDownloadInfo; }
            set { Set(ref showDownloadInfo, value); }
        }

        private string lastDownloadAction;
        public string LastDownloadAction
        {
            get { return lastDownloadAction; }
            set { Set(ref lastDownloadAction, value); }
        }

        private DownloadItem downloadItem;
        public DownloadItem DownloadItem
        {
            get { return downloadItem; }
            set { Set(ref downloadItem, value); }
        }

        public ICommand GoCommand { get; private set; }
        public ICommand HomeCommand { get; private set; }
        public ICommand ExecuteJavaScriptCommand { get; private set; }
        public ICommand EvaluateJavaScriptCommand { get; private set; }
        public ICommand ShowDevToolsCommand { get; private set; }
        public ICommand CloseDevToolsCommand { get; private set; }
        public ICommand JavascriptBindingStressTest { get; private set; }

        public BrowserTabViewModel(string address)
        {
            Address = address;
            AddressEditable = Address;

            GoCommand = new RelayCommand(Go, () => !String.IsNullOrWhiteSpace(Address));    // 주목할점이 ICommand 인터페이스 구현을 한줄만에 끝냄.
            HomeCommand = new RelayCommand(() => AddressEditable = Address = DefaultUrl);
            ExecuteJavaScriptCommand = new RelayCommand<string>(ExecuteJavaScript, s => !String.IsNullOrWhiteSpace(s));
            EvaluateJavaScriptCommand = new RelayCommand<string>(EvaluateJavaScript, s => !String.IsNullOrWhiteSpace(s));
            ShowDevToolsCommand = new RelayCommand(() => WebBrowser.ShowDevTools());
            CloseDevToolsCommand = new RelayCommand(() => WebBrowser.CloseDevTools());
            JavascriptBindingStressTest = new RelayCommand(() =>
            {
                WebBrowser.Load(DefaultUrl);
                WebBrowser.LoadingStateChanged += (e, args) =>
                {
                    if (args.IsLoading == false)
                    {
                        Task.Delay(10000).ContinueWith(t =>
                        {
                            if (WebBrowser != null)
                            {
                                WebBrowser.Reload();
                            }
                        });
                    }
                };
            });

            PropertyChanged += OnPropertyChanged;

            var version = string.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);
            OutputMessage = version;
        }

        private async void EvaluateJavaScript(string s)
        {
            try
            {
                var response = await WebBrowser.EvaluateScriptAsync(s);
                if (response.Success && response.Result is IJavascriptCallback)
                {
                    response = await ((IJavascriptCallback)response.Result).ExecuteAsync("This is a callback from EvaluateJavaScript");
                }

                EvaluateJavaScriptResult = response.Success ? (response.Result ?? "null") : response.Message;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while evaluating Javascript: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteJavaScript(string s)
        {
            try
            {
                WebBrowser.ExecuteScriptAsync(s);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while executing Javascript: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Address":
                    AddressEditable = Address;
                    break;

                case "WebBrowser":
                    if (WebBrowser != null)
                    {
                        WebBrowser.ConsoleMessage += OnWebBrowserConsoleMessage;
                        WebBrowser.StatusMessage += OnWebBrowserStatusMessage;

                        // TODO: This is a bit of a hack. It would be nicer/cleaner to give the webBrowser focus in the Go()
                        // TODO: method, but it seems like "something" gets messed up (= doesn't work correctly) if we give it
                        // TODO: focus "too early" in the loading process...
                        WebBrowser.FrameLoadEnd += (s, args) =>
                        {
                        //Sender is the ChromiumWebBrowser object 
                        var browser = s as ChromiumWebBrowser;
                            if (browser != null && !browser.IsDisposed)
                            {
                                browser.Dispatcher.BeginInvoke((Action)(() => browser.Focus()));
                            }
                        };
                    }

                    break;
            }
        }

        private void OnWebBrowserConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            OutputMessage = e.Message;
        }

        private void OnWebBrowserStatusMessage(object sender, StatusMessageEventArgs e)
        {
            StatusMessage = e.Value;
        }

        private void Go()
        {
            Address = AddressEditable;

            // Part of the Focus hack further described in the OnPropertyChanged() method...
            Keyboard.ClearFocus();
        }

        public void LoadCustomRequestExample()
        {
            var postData = System.Text.Encoding.Default.GetBytes("test=123&data=456");

            WebBrowser.LoadUrlWithPostData("https://cefsharp.com/PostDataTest.html", postData);
        }
    }
}