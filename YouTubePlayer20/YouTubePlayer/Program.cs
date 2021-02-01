using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Windows.Forms;

namespace YouTubePlayer
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // System.Windows.Forms.Application.EnableVisualStyles();
            // System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            // System.Windows.Forms.Application.Run(new Form1());

            System.Windows.Application app = new System.Windows.Application();
            //app.Run(new ChromeWebPannel());     // 컨트롤이 있는 브라우저
            app.Run(new SimpleMainWindow());        // 심플 브라우저
        }
    }
}
