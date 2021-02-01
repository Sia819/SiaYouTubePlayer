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
using System.Diagnostics;

namespace Selennium
{
    public partial class Form2 : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        static extern int GetClientRect(IntPtr hWnd, out RECT lpRect);

        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        public Form2()
        {
            InitializeComponent();
        }

        IntPtr Pid;

        public void Form2_SizeChanged(object sender, EventArgs e)
        {
            // GetClientRect(Pid,}
            int a = 21;
            string str = $"asdf {a}asdfa";

            RECT size;

            int i = GetClientRect(Pid, out size); //??


            this.Text = $"width: {size.right}, height: {size.bottom} ";

            //Text = panel1.Size.ToString() + " Pid : " + Pid;
            MoveWindow(Pid, 10, 10, panel1.Width, panel1.Height, false);

            panel1.MaximumSize = new Size(Width, Height);
            //panel1.Size = new Size(panel1.Width, panel1.Height);
        }

        public void SetPid(IntPtr num)
        {
            Pid = num;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            /*
            RECT size;

            int i = GetClientRect(Pid, out size); //??
            Debug.WriteLine(size.right);

            if (size.right >= Screen.PrimaryScreen.Bounds.Width)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
                this.WindowState = FormWindowState.Normal;
                */
        }
    }
}
