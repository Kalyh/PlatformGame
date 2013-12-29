using Gloopy.Model;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Gloopy
{
    /// <summary>
    /// Simple Gloopy application using SharpDX.Toolkit.
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
            using (SplashScreen ss = new SplashScreen())
            {
                ss.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                ss.Show();
                Thread.Sleep(2000);
                ss.Close();
            }

            using (Menu m = new Menu())
            {
                m.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                m.ShowDialog();
            }
        }
    }
}