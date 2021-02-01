using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace YouTubePlayer
{
    public partial class Form1 : Form
    {
        [DllImport("user32")]
        public static extern Int32 SetWindowLong(int hWnd, Int32 nIndex, int dwNewLong);
        [DllImport("user32")]
        public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);
        [DllImport("user32")]
        public static extern IntPtr SetParent(int hWndChild, IntPtr hWndNewParent);
        [DllImport("user32")]
        public static extern Int32 GetWindowLong(int hWnd, Int32 nIndex);
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        [DllImport("user32.dll")]
        //public static extern int PostMessage(int hwnd, int wMsg, int wParam, uint lParam);
        public static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, Int32 wParam, Int32 lParam);
        [DllImport("user32.dll")]
        public static extern int SendMessage(int hwnd, int wMsg, int wParam, uint lParam);

        Form2 form2 = new Form2();
        Thread t1;

        public Form1()
        {
            InitializeComponent();
            textBox2.Enabled = false;
            trackBar1.Maximum = 17;
            //button2.Visible = false;
            textBox1.Text = "https://www.youtube.com/watch?v=wrpRv1pyV6I&list=RDwrpRv1pyV6I&t";
            //textBox2.Visible = false;

            
        }

        private string UrlSplit(string url)
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
            else if (url.Contains("https://") && checkBox3.Checked == false)
                url = "1";
            else if (checkBox3.Checked == false)
                url = "2";
            return url;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = textBox1.Text;

            url = UrlSplit(url);
            
            if (url == "1")
            {
                MessageBox.Show("주소인것 같지만, 유투브 주소를 찾지 못하였습니다.");
                return;
            }
            else if (url == "2")
            {
                MessageBox.Show("주소 형식을 벗어났습니다. https가 포함되는 주소를 입력해주세요.");
                return;
            }

            textBox2.Text = "https://www.youtube.com/embed/" + url;

            if (checkBox3.Checked == false)
                url = "https://www.youtube.com/embed/" + url;
            else
                url = textBox1.Text;

            form2.InitChrome(url);


            try
            {
                form2.Show();
            }
            catch
            {
                var forms = new Form2();
                form2 = forms;
                form2.InitChrome(textBox2.Text);
                form2.Show();
                //form2.Refresh();
                //form2run();
                //Thread t1 = new Thread(new ThreadStart(form2run));
                //t1.Start();
            }
        }

        void form2run()
        {
            var forms = new Form2();
            form2 = forms;
            form2.ShowDialog();
            //Application.Run(forms);
        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                form2.Show();
            }
            catch
            {
                
                //t1 = new Thread(new ThreadStart(form2run));
                //t1.Start();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
                form2.FormBorderStyle = FormBorderStyle.None;
            else
                form2.FormBorderStyle = FormBorderStyle.Sizable;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                if (trackBar1.Value != 17)
                    form2.TopMost = true;
                else
                {
                    form2.TopMost = false;
                    checkBox2.Checked = false;
                    TopMost = true;
                    MessageBox.Show("전체화면이므로 항상위 옵션이 해제됩니다.");
                    TopMost = false;
                }
            }
            else
                form2.TopMost = false;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (checkBox4.Checked == false)
            {
                double a1 = 16;
                double a2 = 9;
                double a3 = ((trackBar1.Value + 3) * 100);
                int a4;

                if (checkBox1.Checked == false)
                    a4 = ((int)((a2 * a3) / a1)) + 30;
                else
                    a4 = ((int)((a2 * a3) / a1));

                if (trackBar1.Value == 0)
                    WinMove("Player", 10, 10);
                else if (trackBar1.Value == 16)
                {

                    WinMove("Player", 10, 10);
                    TopMost = true;
                    form2.Size = new Size((int)a3, a4);
                }
                else if (trackBar1.Value == 17)
                {
                    //form2.TopMost = false;
                    checkBox2.Checked = false;
                    WinMove("Player", -8, -31, 1940, 1080);
                    TopMost = true;
                }
                else
                {
                    form2.Size = new Size((int)a3, a4);
                }
                TopMost = false;
            }
            else
            {
                if (trackBar1.Value == 0)
                {
                    form2.webBrowser1.Dock = DockStyle.None;
                    form2.webBrowser1.Location = new Point(-9, -60);     //테두리 제거시 -60 아닐시 -59
                    form2.webBrowser1.Size = new Size(443, 320);
                    //panel1.Dock = DockStyle.Fill 이면 AutoSizeMode가 고정이라면 panel1.Size를 변경해줄 필요가 없다.
                    form2.Size = new Size(442, 279);

                    
                }
                else if (trackBar1.Value == 1)
                {
                    form2.webBrowser1.Location = new Point(-9, -60);
                    //form2.webBrowser1.Size = new Size(443, 320);
                    form2.webBrowser1.Dock = DockStyle.Fill;
                    form2.Size = new Size(442, 279);

                }
                else if (trackBar1.Value == 2)
                {

                }
                else if (trackBar1.Value == 3)
                {

                }
            }
        }

        void WinMove(string name, int x1, int y1, int x2, int y2)
        {
            SetWindowPos(FindWindow(null, name), 0, x1, y1, x2, y2, 0);
        }
        void WinMove(string name, int x1, int y1)
        {
            const short SWP_NOSIZE = 1;
            const short SWP_NOZORDER = 0X4;
            const int SWP_SHOWWINDOW = 0x0040;
            SetWindowPos(FindWindow(null, name), 0, x1, y1, 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            form2.killapp();
        }

        private void textBox1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(textBox1, "YouTube 주소를 입력해주세요");
        }


        private uint MakeLparam(int x, int y)
        {
            uint val = Convert.ToUInt32((y * 0x10000) | (x & 0xFFFF));
            return val;
        }
        private void button3_Click(object sender, EventArgs e)
        {

            MessageBox.Show(form2.webBrowser1.Handle.ToString());

            //int x = 100;
            //int y = 100;
            //
            //PostMessage(form2.Handle, 0x201, 1, (int)MakeLparam(x, y));
            //System.Threading.Thread.Sleep(200);
            //PostMessage(form2.Handle, 0x202, 0, (int)MakeLparam(x, y));


            webBrowser1.Navigate("https://youtu.be/-tKVN2mAKRI");
            //int x = 410;
            //int y = 257;
            //int param = x | y << 16;
            //PostMessage(0x201, 1, param, FindWindow(null, "Palyer"));
            //PostMessage(0x202, 0, param, FindWindow(null, "Palyer"));
            //MessageBox.Show(param.ToString());
        }

        private void checkBox4_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(checkBox4, "재생이 안될 때 해당 기능을 사용하면 동영상 사이즈 조절크기가 제한되지만\n다시 동영상을 재생할 수 있습니다.");
        }

        bool waschecked = false;
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                trackBar1_Scroll(sender, e);
                form2.panel1.Visible = true;
                //form2.chrome.Visible = false;
                form2.InitChrome("about:blank");    //form2.chrome.Load("about:blank");
                trackBar1.Maximum = 4;
                string url = textBox1.Text;
                url = UrlSplit(url);
                if (url == "1")
                {
                    MessageBox.Show("주소인것 같지만, 유투브 주소를 찾지 못하였습니다.");
                    waschecked = false;
                    checkBox4.Checked = false;
                    return;
                }
                else if (url == "2")
                {
                    MessageBox.Show("주소 형식을 벗어났습니다. https가 포함되는 주소를 입력해주세요.");
                    waschecked = false;
                    checkBox4.Checked = false;
                    return;
                }

                form2.webBrowser1.Navigate("https://www.youtube.com/watch?v=" + url);

                waschecked = true;
            }
            else
            {
                if (waschecked == true)
                {
                    form2.panel1.Visible = false;
                    form2.chrome.Visible = true;
                    button1_Click(sender, e);   //textBox1로 다시로드
                    webBrowser1.Navigate("about:blank");    //webBrowser1 사이트제거
                    trackBar1.Maximum = 17;
                }
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
