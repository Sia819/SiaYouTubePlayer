using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Selennium
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_MouseMove(object sender, MouseEventArgs e)
        {
            Text = e.X + ", " + e.Y;
        }

        private void Form3_MouseClick(object sender, MouseEventArgs e)
        {
            
        }
    }
}
