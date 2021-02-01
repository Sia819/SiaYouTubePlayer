using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using CefSharp;
using CefSharp.WinForms;



using System.Diagnostics;
using System.Runtime.InteropServices;

using System.Collections;

using System.Management;


namespace Selennium
{


    public partial class Form1 : Form
    {
        Form2 form2 = new Form2();
        Form3 form3 = new Form3();

        public Form1()
        {
            InitializeComponent();



        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            form3.Show();
            form3.webBrowser1.Navigate("https://www.youtube.com/watch?v=LOajYHKEHG8");
            while (form3.webBrowser1.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
            */
            webBrowser1.Navigate("https://www.youtube.com/watch?v=q_sQUK418mM");
        }

        // ChromeDriver chrome;

        //IWebDriver driver;



        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32")]
        public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);
        [DllImport("user32.dll")]
        //public static extern int PostMessage(int hwnd, int wMsg, int wParam, uint lParam);
        public static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, Int32 wParam, Int32 lParam);
        [DllImport("user32.dll")]
        public static extern int SendMessage(int hwnd, int wMsg, int wParam, uint lParam);
        [DllImport("user32")]
        public static extern Int32 GetCursorPos(out Point pt);
        [DllImport("user32")]
        public static extern int SetCursorPos(int x, int y);
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



        public void button1_Click(object sender, EventArgs e)
        {

            ChromeRemote();

            //dockIt();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //SetParent(hWnd, panel1.Handle);
            //SetParent(hWnd, panel1.Handle);
            //var element = form3.webBrowser1.Document.GetElementById("movie_player").InvokeMember("Click");

            var a = form3.webBrowser1.Document.GetElementById("ytp-fullscreen-button");


            int num = 123;

            //a.InvokeMember
            //HtmlElement aa = form3.webBrowser1.Document.GetElementFromPoint(Point point);


            //webBrowser1.Document.GetElementsByTagName("ytp-fullscreen-button").Cl

        }

        //static IEnumerable<HtmlElement> ElementsByClass(HtmlDocument doc, string className)
        static IEnumerable<HtmlElement> ElementsByClass(HtmlDocument doc, string className)
        {
            foreach (HtmlElement e in doc.All)
                if (e.GetAttribute("className") == className)
                    yield return e;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //SetParent(hWnd, form2.panel1.Handle);
            //Point po = new Point(int.Parse(textBox1.Text), int.Parse(textBox2.Text));
            //HtmlElement aa = form3.webBrowser1.Document.GetElementFromPoint(po);
            //MessageBox.Show(aa.ToString());


            player = form3.webBrowser1.Document.GetElementById("movie_player");       //유투브 플레이어

            //player.InvokeMember("MouseEnter");
            //player.InvokeMember("MouseUp");
            //player.InvokeMember("MouseUp");
            
            //player.InvokeMember("DoubleClick");


            //webBrowser1.Document.InvokeScript();      //자바스크립트
            /*
            HtmlElement aa = ElementsByClass(form3.webBrowser1.Document, "ytp-next-button ytp-button").First(); 
            //HtmlElement aa = ElementsByClass(form3.webBrowser1.Document, "ytp-fullscreen-button ytp-button").First();
            
            //aa.Id
            //InvokeMember("Click");.

            string st = aa.Id;
            MessageBox.Show(st);
            aa.Focus();
            aa.InvokeMember("Click");
            
            //aa.InvokeMember("Click");
            */
            Thread t1 = new Thread(new ThreadStart(click));
            t1.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //393, 284
            //int x = 100;
            //int y = 100;
            //IntPtr nav = FindWindow("Internet Explorer_Server1", null);
            //PostMessage(webBrowser1.Handle, 0x201, 1, (int)MakeLparam(x, y));
            //PostMessage(webBrowser1.Handle, 0x202, 0, (int)MakeLparam(x, y));

            //mouse_event(MOUSEEVENTF_MOVE, 100, 100, 0, webBrowser1.Handle);
            //SetCursorPos(100, 100);
            Point pos = new Point();
            GetCursorPos(out pos);
            MessageBox.Show(pos.ToString());
            
        }

        private uint MakeLparam(int x, int y)
        {
            uint val = Convert.ToUInt32((y * 0x10000) | (x & 0xFFFF));
            return val;
        }

        HtmlElement player;
        void click()
        {
            //player = form3.webBrowser1.Document.GetElementById("movie_player");       //유투브 플레이어
            
            player.InvokeMember("MouseDown");
            Thread.Sleep(200);
            player.InvokeMember("MoudeUp");
            player.InvokeMember("Click");
            Thread.Sleep(200);
            player.InvokeMember("MouseDown");
            Thread.Sleep(200);
            player.InvokeMember("MoudeUp");
            player.InvokeMember("Click");
            player.InvokeMember("DoubleClick");
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Q)
            { 
                MessageBox.Show("");
                button3_Click(sender, e);
            }
        }



        //#########################################################################################
        //public static IEnumerable<Process> GetChildren(this Process parent)
        public IEnumerable<Process> GetChildren(Process parent)
        {
            var query = new ManagementObjectSearcher($@"
        SELECT *
        FROM Win32_Process
        WHERE ParentProcessId={parent.Id}");

            return from item in query.Get().OfType<ManagementBaseObject>()
                   let childProcessId = (int)(UInt32)item["ProcessId"]
                   select Process.GetProcessById(childProcessId);
        }
        /*
        public IntPtr AttachDriverService(DriverService service)
        {
            //get the process started by selenium
            var driverProcess = Process.GetProcessById(service.ProcessId);

            //find the first child-process (should be the browser)
            var browserProcess = driverProcess.GetChildren()
                                    .Where(p => p.ProcessName != "conhost")
                                    .First();

            return browserProcess.MainWindowHandle;
        }
        */
        public IntPtr AttachDriverService(DriverService service)
        {
            //get the process started by selenium
            var driverProcess = Process.GetProcessById(service.ProcessId);
            var browserProcess = GetChildren(driverProcess)
                                    .Where(p => p.ProcessName != "conhost")
                                    .First();

            return browserProcess.MainWindowHandle;
        }
        //#########################################################################################


        IntPtr hWnd;
        private void ChromeRemote()
        {
            //ChromeOptions options = new ChromeOptions();
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("disable-infobars");
            

            IWebDriver Driver = new ChromeDriver(service, options);

            
            
            

            hWnd = AttachDriverService(service);

            SetParent(hWnd, form2.panel1.Handle);


            MoveWindow(hWnd, 0, 0, 100, 100, true);

            form2.SetPid(hWnd);

            form2.Show();
            //var hWnd = FindWindow(null, Driver.Title);

            //string st = Driver.CurrentWindowHandle;
            //MessageBox.Show(st);
            Driver.Navigate().GoToUrl("https://www.youtube.com/watch?v=-tKVN2mAKRI&list=RD-tKVN2mAKRI");
            //var rank = Driver.FindElement(By.ClassName("list_rank"));
            object sender = new object();
            EventArgs e = new EventArgs();
            form2.Form2_SizeChanged(sender, e);
            Thread.Sleep(5000);
            Driver.FindElement(By.ClassName("ytp-fullscreen-button")).Click();
            //var a = Driver.FindElement(By.ClassName("ytp-right-controls"));
            //a.FindElement(By.ClassName(""));
            //rank.
        }

        private void web()
        {
            //IWebDriver 
        }

        private void Naver()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Url = "http://www.naver.com";
            driver.Manage().Window.Maximize();

            IWebElement q = driver.FindElement(By.Id("query"));
            q.SendKeys("최신영화순위");
            driver.FindElement(By.Id("search_btn")).Click();
        }

        private Process pDocked;
        private IntPtr hWndOriginalParent;
        private IntPtr hWndDocked;

        private void dockIt()
        {
            if (hWndDocked != IntPtr.Zero) //don't do anything if there's already a window docked.
                return;
            //hWndParent = IntPtr.Zero;

            pDocked = Process.Start(@"notepad");
            while (hWndDocked == IntPtr.Zero)
            {
                pDocked.WaitForInputIdle(1000); //wait for the window to be ready for input;
                pDocked.Refresh();              //update process info
                if (pDocked.HasExited)
                {
                    return; //abort if the process finished before we got a handle.
                }
                hWndDocked = pDocked.MainWindowHandle;  //cache the window handle
            }
            //Windows API call to change the parent of the target window.
            //It returns the hWnd of the window's parent prior to this call.
            hWndOriginalParent = SetParent(hWndDocked, panel1.Handle);

            //Wire up the event to keep the window sized to match the control
            panel1.SizeChanged += new EventHandler(Panel1_Resize);
            //Perform an initial call to set the size.
            Panel1_Resize(new Object(), new EventArgs());
        }

        private void undockIt()
        {
            //Restores the application to it's original parent.
            SetParent(hWndDocked, hWndOriginalParent);
        }

        private void Panel1_Resize(object sender, EventArgs e)
        {
            //Change the docked windows size to match its parent's size. 
            MoveWindow(hWndDocked, 0, 0, panel1.Width, panel1.Height, true);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

    }

    
}
