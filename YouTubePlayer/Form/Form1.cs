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
        public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        //부모컨트롤 변경
        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        //커서위치 Set
        [DllImport("user32")]
        public static extern int SetCursorPos(int x, int y);
        //커서위치 Get
        [DllImport("user32")]
        public static extern Int32 GetCursorPos(out Point pt);
        //hWnd를 이동
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        //hWnd의 크기 (0, 0, right, bottom)
        [DllImport("user32")]
        static extern int GetClientRect(IntPtr hWnd, out RECT lpRect);
        //윈도우기준 hWnd의 위치 (x1, y1, x2, y2)
        [DllImport("user32")]
        public static extern int GetWindowRect(IntPtr hwnd, out RECT lpRect);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, IntPtr dwExtraInfo);
        private const int MOUSEEVENTF_MOVE = 0x0001;
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const int MOUSEEVENTF_RIGHTUP = 0x0010;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        private int timerLevel = 0;

        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        Form2 form2 = new Form2();
        Form3 form3 = new Form3();
        public Form1()
        {
            InitializeComponent();
            KeyPreview = true;
            form2.form1 = this;
            form3.form1 = this;
            //form2.Visible = false;
            //form3.Visible = false;
            button4.Visible = false;
            button3.Enabled = false;
            checkBox1.Enabled = false;
            checkBox2.Enabled = false;
            checkBox3.Enabled = false;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            trackBar1.Maximum = 17;
            //checkBox3.Enabled = false;
            trackBar1.Enabled = false;
            WinMove(Handle, 150, 90);
            checkBox4_CheckedChanged(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.Contains("https"))
            {
                MessageBox.Show("정확한 주소를 입력해주세요.");
                return;
            }
            else if (checkBox4.Checked && textBox1.Text.Contains("embed"))
            {
                MessageBox.Show("죄송합니다. embed는 지원하지 않습니다.");
                return;
            }
            else if (textBox1.Text.Contains("/v/"))
            {
                MessageBox.Show("죄송합니다. /v/사이트는 지원하지 않습니다.");
                return;
            }
            else if (textBox1.Text.Contains("/tv#/"))
            {
                MessageBox.Show("죄송합니다. /tv#/사이트는 지원하지 않습니다.");
                return;
            }
            else if (textBox1.Text.Contains("/music"))
            {
                MessageBox.Show("죄송합니다. 홈페이지는 재생할 동영상이 없는것 같습니다.");
                return;
            }
            else if (textBox1.Text.Contains("/channel/"))
            {
                MessageBox.Show("죄송합니다. 홈페이지는 재생할 동영상이 없는것 같습니다.");
                return;
            }
            else if (textBox1.Text.Contains(".com/"))
            {
                if (textBox1.Text.Split(new string[] { ".com/" }, StringSplitOptions.None)[1] == "")
                {
                    MessageBox.Show("죄송합니다. 홈페이지는 재생할 동영상이 없는것 같습니다.");
                    return;
                }
            }
            RECT size;
            GetWindowRect(Handle, out size);

            if (checkBox4.Checked == false)
            {
                //Chromeium을 사용합니다.
                ChromeYouTube();
            }
            else
            {
                try
                {
                    form2.form1 = this;
                    form2.Show();
                    form2.Visible = true;
                }
                catch
                {
                    form2 = new Form2();
                    form2.form1 = this;
                    form2.Show();
                }
                WinMove(form2.Handle, size.left, size.bottom + 10);
                try
                {
                    string test = form2.chrome.Address;
                    //chrome은 사용되었습니다.
                    form2.chrome.Load("about:blank");
                }
                catch
                {
                    //개체 정보가 인스턴트로 설정되지않았습니다.
                    //즉 chrome은 한번도 사용되지 않았습니다.
                }
                //webBrowser를 사용합니다.
                WebYouTube();
            }
            trackBar1.Enabled = true;
        }

        private void ChromeYouTube()
        {
            //webBrowser을 사용하다가 Chromeium을 사용할 때 webBrowser초기화


            string url = textBox1.Text;

            url = UrlSplit(url);

            if (url == "1")
            {
                MessageBox.Show("주소인것 같지만, 유투브 주소를 찾지 못하였습니다.", "Split Error");
                return;
            }
            else if (url == "2")
            {
                MessageBox.Show("주소 형식을 벗어났습니다. https가 포함되는 주소를 입력해주세요.", "Split Error");
                return;
            }

            url = "https://www.youtube.com/embed/" + url + "?autoplay=1";

            form2.InitChrome(url);

            try
            {
                form2.form1 = this;
                form2.Show();
                form2.Size = new Size(442, 279);
            }
            catch   //개체가 삭제되어 오류가 나는경우
            {
                Form2 forms = new Form2();
                form2 = forms;
                form2.form1 = this;
                form2.InitChrome(url);      //삭제된 상태라면 새로 생성하고 form2.InitChrome 구문이 한번 더 돌아야하기때문에
                form2.Show();
                
            }
            form2.Text = "Player - [ " + url + " ]";
            RECT size;
            GetWindowRect(Handle, out size);
            WinMove(form2.Handle, size.left, size.bottom + 10);
            //form2.Size = new Size(442, 279);  //인위적 조종 대신 트랙바를 이용
            trackBar1.Value = 2;
            object sender = new object(); EventArgs e = new EventArgs(); trackBar1_Scroll(sender, e);
            form2.webBrowser1.Navigate("about:blank");
            form2.webBrowser1.Visible = false;
        }

        private void WebYouTube()
        {
            form2.TopMost = true;
            form2.webBrowser1.Dock = DockStyle.None;
            form2.webBrowser1.ScrollBarsEnabled = false;
            form2.webBrowser1.Location = new Point(0, -60);
            form2.webBrowser1.Size = new Size(426, 300);
            form2.Size = new Size(442, 279);  //마우스 클릭때문에 인위적으로 조종해야함
            
            form2.webBrowser1.Navigate(textBox1.Text);
            //로딩될 때 까지 기다린다
            while (form2.webBrowser1.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
            //form2.webBrowser1.Document.GetElementById("movie_player").InvokeMember("Click");
            //form2.TopMost = false;
            //MouseMove();
            textBox1.Focus();
            //form2.FullScreen();

            timerLevel = 1;
            timer1.Enabled = true;
            form2.Text = "Player - [ " + textBox1.Text + " ]";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (timerLevel == 1)
            {
                form2.webBrowser1.Dock = DockStyle.Fill;
                trackBar1.Value = 2; trackBar1_Scroll(sender, e);
            }
            else if (timerLevel == 2)
            {
                return;
            }
        }

        

        public string UrlSplit(string url)
        {
            if (url.Contains("continue="))
                url = url.Split(new string[] { "&v=" }, StringSplitOptions.None)[1];    //이어재생 유튜브 본영상에서//https://www.youtube.com/watch?time_continue=110&v=fOzBVy-sDbM
            else if (url.Contains("&feature="))
                url = url.Split(new string[] { "v=", "&feature=" }, StringSplitOptions.None)[1];
            else if (url.Contains("watch?v=") && url.Contains("list=") && !url.Contains("&index="))
                url = url.Split(new string[] { "watch?v=", "&list=" }, StringSplitOptions.None)[1]; //리스트에 포함되어있는 주소("&index="가 없는경우)    //https://www.youtube.com/watch?v=iwsHL5fm9_A&list=PLaMkM8SLIoU8TAlUGaIqxT_QKtjUuPl6w
            ///else if (url.Contains("&list=RD"))
            ///    url = url.Split(new string[] { "list=RD" }, StringSplitOptions.None)[1];
            else if (url.Contains("watch?v=") && url.Contains("&t="))
                url = url.Split(new string[] { "watch?v=", "&t=" }, StringSplitOptions.None)[1];    //시작시간과 같이 있는 주소
            else if (url.Contains("watch?v=") && url.Contains("&index=") && !url.Contains("&list="))
                url = url.Split(new string[] { "watch?v=", "&index=" }, StringSplitOptions.None)[1];    //유튜브 재생목록시
            else if (url.Contains("watch?v=") && url.Contains("&list=") && url.Contains("&index="))
                url = url.Split(new string[] { "watch?v=", "&list=" }, StringSplitOptions.None)[1];    //리스트 + 인덱스      //오류때문에 밑에서 한번 더 잘라줘야함 https://www.youtube.com/watch?v=ucXk4i2vfpg&index=17&list=RDS__QPp8ORhc
            else if (url.Contains("watch?v="))
                url = url.Split(new string[] { "watch?v=" }, StringSplitOptions.None)[1];   //통상 유튜브
            else if (url.Contains("youtu.be/"))
                url = url.Split(new string[] { "youtu.be/" }, StringSplitOptions.None)[1];  //유튜브 단축영상
            else if (url.Contains("embed/"))
                url = url.Split(new string[] { "embed/" }, StringSplitOptions.None)[1];
            else if (url.Contains(".com/"))
                url = url.Split(new string[] { ".com/" }, StringSplitOptions.None)[1];
            else if (url.Contains("https://"))
                url = "1";
            else
                url = "2";

            //if (url.Contains("&list"))  //잘랐는데도 순서때문에 먼저 잘라진게 남아있는경우
            //    url = url.Split(new string[] { "&list" }, StringSplitOptions.None)[0];
            //else if (url.Contains("&index="))
            //    url = url.Split(new string[] { "&index=" }, StringSplitOptions.None)[0];
            if (url.Contains("&"))
                url = url.Split(new string[] { "&" }, StringSplitOptions.None)[0];

            return url;
        }

        

        private void Parentset()
        {
            SetParent(button1.Handle, form2.Handle);
        }

        private new void MouseMove()
        {
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, (IntPtr)0);    //마우스 왼쪽 버튼이 눌려져있을경우 뗌
            Point pt;
            RECT rect;
            GetWindowRect(form2.webBrowser1.Handle, out rect);
            GetCursorPos(out pt);
            SetCursorPos(rect.left + 396, rect.top + 281);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, (IntPtr)0);
            SetCursorPos(pt.X, pt.Y);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            RECT pos;
            GetWindowRect(form2.Handle, out pos);
            if (checkBox1.Checked == true)
            {
                if (trackBar1.Value < 17)
                    WinMove(form2.Handle, pos.left + 8, pos.top + 31);
                form2.FormBorderStyle = FormBorderStyle.None;
                form2.move = true;
            }
            else
            {
                if (trackBar1.Value < 17)
                    WinMove(form2.Handle, pos.left - 8, pos.top - 31);
                form2.FormBorderStyle = FormBorderStyle.Sizable;
                form2.move = false;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
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
            {
                WinMove(form2.Handle, 10, 10);
                form2.Size = new Size((int)a3, a4);
            }

            else if (trackBar1.Value == 16)
            {

                WinMove(form2.Handle, 10, 10);
                TopMost = true;
                form2.Size = new Size((int)a3, a4);
            }
            else if (trackBar1.Value == 17 && checkBox1.Checked == true)
            {
                //form2.TopMost = false;
                checkBox2.Checked = false;
                form2.TopMost = true;
                form2.TopMost = false;
                WinMove(form2.Handle, 0, 0, 1920, 1080);
                TopMost = true;
                //checkBox3.Enabled = true;
            }
            else if (trackBar1.Value == 17)
            {
                //form2.TopMost = false;
                checkBox2.Checked = false;
                WinMove(form2.Handle, -8, -31, 1938, 1120);
                TopMost = true;
                //checkBox3.Enabled = true;
            }
            else
            {
                form2.Size = new Size((int)a3, a4);
            }
            TopMost = false;

            if (trackBar1.Value != 17 && checkBox3.Enabled == true)
            {
                form2.TopMost = false;
                TopMost = false;
                checkBox3.Checked = false;
                //checkBox3.Enabled = false;
                if (checkBox1.Checked == false)
                    form2.FormBorderStyle = FormBorderStyle.Sizable;
            }
        }

        void WinMove(IntPtr hWnd, int x1, int y1, int x2, int y2)
        {
            SetWindowPos(hWnd, 0, x1, y1, x2, y2, 0);
        }
        void WinMove(IntPtr hWnd, int x1, int y1)
        {
            const short SWP_NOSIZE = 1;
            const short SWP_NOZORDER = 0X4;
            const int SWP_SHOWWINDOW = 0x0040;
            SetWindowPos(hWnd, 0, x1, y1, 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                if (trackBar1.Value != 17)
                {
                    form2.TopMost = true;

                }
                else
                {
                    form2.TopMost = false;
                    checkBox2.Checked = false;
                    TopMost = true;
                    MessageBox.Show("전체화면이므로 항상위 옵션이 해제됩니다.");
                    // TopMost = false;
                }
            }
            else
            {
                form2.TopMost = false;
                TopMost = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true && checkBox1.Checked == false)
            {
                trackBar1.Value = 17;
                //form2.FormBorderStyle = FormBorderStyle.None;
                //form2.TopMost = false;
                //WinMove(form2.Handle, 0, 0, 1920, 1080);
                WinMove(form2.Handle, -8, -31, 1938, 1120);
                TopMost = true;
                MessageBox.Show("전체화면을 해제하시려면 Alt + Tab 을 눌러\n유투브 플레이를 활성화 시켜주세요.");
            }
            else if (checkBox3.Checked == true && checkBox1.Checked == true)
            {
                trackBar1.Value = 17;
                //form2.TopMost = false;
                WinMove(form2.Handle, 0, 0, 1920, 1080);
                TopMost = true;
                MessageBox.Show("전체화면을 해제하시려면 Alt + Tab 을 눌러\n유투브 플레이를 활성화 시켜주세요.");
            }
            else
            {
                //if (checkBox1.Checked == false)
                    trackBar1.Value = 16;trackBar1_Scroll(sender, e);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                form3.chrome.Load("https://www.youtube.com/music");
                form3.Show();
                form3.Visible = true;
            }
            catch
            {
                form3 = new Form3();
                form3.Show();
                form3.form1 = this;
                form3.Visible = true;
            }
            RECT size;
            GetWindowRect(Handle, out size);

            MoveWindow(form3.Handle, size.right + 7, size.top, 946, 704, true);
            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //form3.chrome.Load("about:blank");
            //form3.Visible = false;
            form3.Close();
            button3.Enabled = false;
            textBox1.Text = form3.chrome.Address;
        }

        private void checkBox4_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(checkBox4, "해당주소 동영상이 재생이 안될때만 체크해주세요.\n\n체크를 해제하시면 동영상 화질이 좋아집니다.\n하지만, 실시간 스트리밍은 불가능합니다");
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {

        }

        private void button2_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(button2, "편의성을 위해 유투브에서 바로 검색하고 URL을 가져올 수 있습니다.");
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                checkBox4.Text = "실시간스트리밍 [ ON ]";
                checkBox4.ForeColor = Color.Red;
                form2.webBrowser1.Visible = true;
            }
            else
            {
                checkBox4.Text = "동영상 재생이 안되시나요?";
                checkBox4.ForeColor = Color.Gray;
                form2.webBrowser1.Visible = false;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)   //디버그입니다.
            {
                //MessageBox.Show(Auto.WinGetHandle("제목 없음 - 메모장", null));
                //form2.FullScreen();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //디버그입니다.

            MessageBox.Show(form2.chrome.Address);
        }


    }
}
