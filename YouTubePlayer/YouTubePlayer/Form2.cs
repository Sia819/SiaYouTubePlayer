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

namespace YouTubePlayer
{
    public partial class Form2 : Form
    {
        //Form1 form1 = new Form1();

        public ChromiumWebBrowser chrome;

        public Form2()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            panel1.Visible = false;
            panel1.Dock = DockStyle.Fill;
            webBrowser1.Location = new Point(0, 0);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }

        public void AlwaysOnTop(bool b)
        {
            if (b)
            {
                TopMost = true;
                MessageBox.Show("변경성공");
            }
            else
            {
                TopMost = false;
                MessageBox.Show("취소성공");
            }
        }

        CefSettings settings = new CefSettings();
        
        public void InitChrome(string url)
        {
            try
            {
                Cef.Initialize(settings);
                chrome = new ChromiumWebBrowser(url);
                this.Controls.Add(chrome);
                chrome.Dock = DockStyle.Fill;
                //chrome.Size = new Size(400, 200);
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
                    //chrome.Size = new Size(400, 200);
                    //chrome.Dock = DockStyle.Fill;
                    chrome.Load(url);
                }

            }
        }

        public void killapp()   //form1 에서 죽이는 명령어 실행 //form1 새 스레드로 실행된경우 안죽음
        {
            Application.Exit();
        }

        private void Form2_SizeChanged(object sender, EventArgs e)
        {
            //Text = Size.Width.ToString() + ", " + Size.Height.ToString();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
