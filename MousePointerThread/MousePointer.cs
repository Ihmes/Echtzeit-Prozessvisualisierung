using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using Technologies;
using ManagedWinapi;
using ManagedWinapi.Accessibility;
using System.Linq;
using System.Windows.Forms;
namespace MousePointerThread
{
    public class MousePointer
    {
        /*/// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }*/

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref System.Drawing.Point lpPoint);
        /*
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(HookType code, HookProc func, IntPtr hInstance, int threadID);

        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        private delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);
        private HookProc myCallbackDelegate = null;

        public enum HookType : int
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }
        */
        System.Drawing.Point MousePoint = new System.Drawing.Point();

        private static int _xKoordinate = 0;
        private static int _yKoordinate = 0;

        public MousePointer()
        {
            
        }

        public void StartMousePointer()
        {
            Technologies.Technology tech = new Technologies.Technology();

            System.Windows.Point point = new System.Windows.Point();
            while (true)
            {
                Thread.Sleep(30);
                //Statt Mausklick + Strg nur Strg ist besser
                //Wenn Mausklick + Strg wird das jeweilige Dropdown geöffnet
                //Maus muss über einem "Prozessschritt" sein, dann Strg drücken -> Ausgabe Konsole
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                {
                    String s = Start(point, tech);
                    Console.WriteLine(s);
                }
            }
        }
  

        private String Start(System.Windows.Point point, Technologies.Technology tech)
        {            
            //Technologies.Jab jab = new Technologies.Jab();
           // Console.Write(tech.GetNameByPosition(point));
            
            GetCursorPos(ref MousePoint);

            XKoordinate = MousePoint.X;
            YKoordinate = MousePoint.Y;
            //Text = "X = " + XKoordinate + " " + "Y = " + YKoordinate;

            point.X = XKoordinate;
            point.Y = YKoordinate;
            
            SystemAccessibleObject accessibleObject = null;
            accessibleObject = SystemAccessibleObject.FromPoint((int)point.X,(int)point.Y);

            var bounds = accessibleObject.Location;
            

            String name = "Name: " + tech.GetNameByPosition(point);

            if (name.Equals("Name: no name"))
            {
                point = new System.Windows.Point(accessibleObject.Location.X, accessibleObject.Location.Y - 0.5);
                name = "Name: " + tech.GetNameByPosition(point);
            }

            //String id = "ID: " + tech.GetIdByPosition(point);
            String type = "Typ: " + tech.GetTypeByPosition(point);
            String process = "Process: " + tech.GetProcessNameByPosition(point);
            String windowName = "Window: " + tech.GetWindowNameByPosition(point);

            String ausgabe = name+" "+type+" "+process+" "+windowName;

            return ausgabe;
        }

        //Get-Method for the x-coodinate
        public static int XKoordinate
        {
            set;
            get;
        }

        //Get-Method for the y-coodinate
        public static int YKoordinate
        {
            set { _yKoordinate = value; }
            get { return _yKoordinate; }
        }
    }
}
