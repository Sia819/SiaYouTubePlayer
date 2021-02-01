using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace YouTubePlayer_V3
{
    public partial class Form3 : Form
    {
        public ChromiumWebBrowser chrome;

        public Form1 form1;

        public Form3()
        {
            InitializeComponent();
            //Form LostFocus EventHandler
            LostFocus += new EventHandler(Form1_LostFocus);
        }

        void Form1_LostFocus(object sender, EventArgs e)
        {
            //MessageBox.Show("포커스를 잃었습니다.");
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            Text = "YouTube 검색";
            ChromeStart();
        }

        //Chromium
        public void ChromeStart()
        {
            string url = "https://www.youtube.com/music";
            try
            {
                CefSettings settings = new CefSettings();
                Cef.Initialize(settings);
                chrome = new ChromiumWebBrowser(url);
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
                    this.Controls.Add(chrome);
                    chrome.Dock = DockStyle.Fill;
                    chrome.Load(url);
                }

            }
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (form1 != null)
                form1.button3.Enabled = false;
        }
    }
}
