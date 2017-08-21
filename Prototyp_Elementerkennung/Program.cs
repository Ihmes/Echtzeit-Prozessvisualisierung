using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ManagedWinapi;
using ManagedWinapi.Accessibility;
using System.Threading;
using System.Runtime.InteropServices;

namespace Prototyp_Elementerkennung
{
    static class Program
    {
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref System.Drawing.Point lpPoint);

        //System.Drawing.Point MousePoint = new System.Drawing.Point();
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SystemAccessibleObject cww = SystemAccessibleObject.FromWindow(new ManagedWinapi.Windows.SystemWindow(new Control("")), AccessibleObjectID.OBJID_WINDOW);
            if (cww != null)
            {
                Console.WriteLine("####################");
                Console.WriteLine(cww.Name);

                while (true)
                {
                    Thread.Sleep(1000);
                    cww = SystemAccessibleObject.FromWindow(new ManagedWinapi.Windows.SystemWindow(new Control("")), AccessibleObjectID.OBJID_WINDOW);
                    Console.WriteLine(cww.Window.Process);
                }
            }
        }
    }
}
