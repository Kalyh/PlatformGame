using MyGame1.Model;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MyGame1
{
    /// <summary>
    /// Simple MyGame1 application using SharpDX.Toolkit.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
#if NETFX_CORE
        [MTAThread]
#else
        [STAThread]
#endif
        static void Main()
        {
            SplashScreen ss = new SplashScreen();
            ss.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ss.Show();
            Thread.Sleep(2000);
            ss.Close();
            using (var program = new MyGame())
                program.Run();

        }
    }
}