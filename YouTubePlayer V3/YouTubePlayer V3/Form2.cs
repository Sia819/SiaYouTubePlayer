﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using CefSharp;
using CefSharp.WinForms;
//using AutoItX3Lib;
using System.Threading;

namespace YouTubePlayer_V3
{
    public partial class Form2 : Form
    {
        //AutoItX3 Auto = new AutoItX3();

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int WM_NCLBUTTONUP = 0xA2;
        public const int HT_CAPTION = 0x2;
        public const int WM_NCACTIVATE = 0x2;
        public const int WM_LBUTTONUP = 0x0202;

        public bool move = new bool();
        private int timerLevel = 0;
        
        public ChromiumWebBrowser chrome;// = new ChromiumWebBrowser("about:blank");
        public CefSettings settings = new CefSettings();

        public Form1 form1;


        public Form2()
        {
            InitializeComponent();
            Text = "Player";
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (form1 != null)
            {
                form1.checkBox1.Enabled = true;
                form1.checkBox2.Enabled = true;
                form1.checkBox3.Enabled = true;
            }
        }
        int i = 0;
        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            i++;
            form1.Text = i.ToString();
            ////timerLevel = 1;
            ////timer1.Enabled = true;
            //FullScreen();
            Thread t1 = new Thread(new ThreadStart(FullScreen));
            t1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (timerLevel == 1)
            {
                FullScreen();
            }
            else if (timerLevel == 2)
            {
                FormResize();
            }
        }

        public void FullScreen()
        {
            while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
            ElementsByClass(webBrowser1.Document, "ytp-fullscreen-button ytp-button").First().Focus();
            //Auto.ControlSend(Text, "", "Internet Explorer_Server1", "{Enter}");
            timerLevel2 = 1;
            timer2.Enabled = true;
        }

        int timerLevel2 = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false; //한번만 틱해주고 멈추는 타이머
            if (timerLevel2 == 1)
            {
                FormResize();
            }
            else
            {
                Size = new Size(Size.Width, Size.Height - 1);
            }
        }

        void FormResize()
        {
            Size = new Size(Size.Width, Size.Height + 1);
            timerLevel2 = 2;
            timer2.Enabled = true;
        }

        private IEnumerable<HtmlElement> ElementsByClass(HtmlDocument doc, string className)
        {
            foreach (HtmlElement e in doc.All)
                if (e.GetAttribute("className") == className)
                    yield return e;
        }

        public class BrowserLifeSpanHandler : ILifeSpanHandler
        {
            public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName,
                WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo,
                IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
            {
                string url = targetUrl;
                url = UrlSplit(url);

                if (url != "1" && url != "2")
                {
                    browserControl.Load("https://www.youtube.com/embed/" + url + "?autoplay=1");
                }
                
                newBrowser = null;
                return true;
            }

            public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
            {
                //
            }

            public bool DoClose(IWebBrowser browserControl, IBrowser browser)
            {
                return false;
            }

            public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
            {
                //nothing
            }
            public string UrlSplit(string url)
            {
                if (url.Contains("continue="))
                    url = url.Split(new string[] { "&v=" }, StringSplitOptions.None)[1];    //https://www.youtube.com/watch?time_continue=110&v=fOzBVy-sDbM
                else if (url.Contains("&feature="))
                    url = url.Split(new string[] { "v=", "&feature=" }, StringSplitOptions.None)[1];
                else if (url.Contains("list=RD"))
                    url = url.Split(new string[] { "list=RD" }, StringSplitOptions.None)[1];
                else if (url.Contains("watch?v="))
                    url = url.Split(new string[] { "watch?v=" }, StringSplitOptions.None)[1];
                else if (url.Contains("youtu.be/"))
                    url = url.Split(new string[] { "youtu.be/" }, StringSplitOptions.None)[1];
                else if (url.Contains("embed/"))
                    url = url.Split(new string[] { "embed/" }, StringSplitOptions.None)[1];
                else if (url.Contains(".com/"))
                    url = url.Split(new string[] { ".com/" }, StringSplitOptions.None)[1];
                else if (url.Contains("https://"))
                    url = "1";
                else
                    url = "2";
                return url;
            }
        }

        private void Form2_SizeChanged(object sender, EventArgs e)
        {
            //Text = webBrowser1.Size.ToString();
        }

        public void InitChrome(string url)
        {
            try
            {
                Cef.Initialize(settings);
                chrome = new ChromiumWebBrowser(url);
                //chrome.MouseClick += new MouseEventHandler(Chrme_MouseMove);
                chrome.LifeSpanHandler = new BrowserLifeSpanHandler();
                this.Controls.Add(chrome);
                chrome.Dock = DockStyle.Fill;
            }
            catch
            {
                try
                {
                    chrome.Load(url);
                }
                catch
                {
                    chrome = new ChromiumWebBrowser(url);
                    chrome.LifeSpanHandler = new BrowserLifeSpanHandler();
                    this.Controls.Add(chrome);
                    chrome.Dock = DockStyle.Fill;
                    chrome.Load(url);
                }

            }
        }

        

        void Chrme_MouseMove(object sender, MouseEventArgs e)
        {
            //구현은 되어있지만 실질적으로 작동하지않는 버그가있음 ...
            if (e.Button == MouseButtons.Left && move)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        //webBrowser은 테두리없음시 화면이 이동가능합니다.
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.Document != null)
            {
                webBrowser1.Document.MouseMove += new HtmlElementEventHandler(myMouseMove);
                //webBrowser1.Document.MouseLeave += new HtmlElementEventHandler(myMouseLeave);
            }
        }
        void myMouseMove(object sender, HtmlElementEventArgs e)
        {
            if (e.MouseButtonsPressed == MouseButtons.Left && move)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                //count++;
                //Text = count.ToString();
                //webBrowser1.Document.GetElementById("movie_player").InvokeMember("MouseUp");
                //webBrowser1.Document.GetElementById("movie_player").InvokeMember("Click");
                //SendMessage(Handle, WM_NCLBUTTONUP, HT_CAPTION, 0);
                //SendMessage(webBrowser1.Handle, WM_LBUTTONUP, HT_CAPTION, 0);
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (form1 != null)
            {
                form1.checkBox1.Enabled = false;
                form1.checkBox2.Enabled = false;
                form1.checkBox3.Enabled = false;
            }
        }

        

        






        /*
int count = 0;
void myMouseLeave(object sender, HtmlElementEventArgs e)
{
//if (e.MouseButtonsPressed == MouseButtons.Left && e.AltKeyPressed)
//{
//ReleaseCapture();
//SendMessage(Handle, WM_NCLBUTTONUP, HT_CAPTION, 0);
//SendMessage(Handle, WM_LBUTTONUP, 0, 0);
SendMessage(webBrowser1.Handle, WM_LBUTTONUP, 0, 0);
count++;
Text = count.ToString();

Focus();
//}
}

*/
    }
}
