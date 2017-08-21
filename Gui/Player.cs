using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using ManagedWinapi.Accessibility;
using System.Runtime.InteropServices;
using Technologies;
using ManagedWinapi.Windows;
using System.Diagnostics;
using System.Windows.Automation;
using ManagedWinapi.Windows.Contents;

namespace Gui
{
    class Player 
    {

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref System.Drawing.Point lpPoint);
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        [DllImport("user32.dll")]
        static extern IntPtr GetMenu(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern int GetMenuItemCount(IntPtr hMenu);
        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();

       // [DllImport("user32.Dll")] //Alle offenen Fenster abrufen
        //public static extern int EnumWindows(ProcessListDemo.Windows.WinCallBack x, int y);

        [DllImport("user32.dll")] //PrÃŒfen, ob ein Fenster Sichtbar ist
        public static extern bool IsWindowVisible(IntPtr hWnd);


        private delegate void SenderHandler(object sender, EventArgs e);
        private event SenderHandler _event;
        private Technology technology;
        private string processName = "";
        private Process[] processes = null;

        List<SystemAccessibleObject> allSaoRefs;
        List<string> namen;
        

        public Player(string processName)
        {
            allSaoRefs = new List<SystemAccessibleObject>();
            namen = new List<string>();
           // _event += new SenderHandler(playerFrame.Observe);
            //stepDictionary.Add(playerFrame.
            technology = new Technology();
            this.processName = processName;
            ObserveAndPlay();

        }



        public void allreallyAllChilds(SystemAccessibleObject sao)
        {

            SystemAccessibleObject[] childs = sao.Children;

            foreach (SystemAccessibleObject child in childs)
            {
               // SystemAccessibleObject[] saoref = child.Children;
                allreallyAllChilds(child);

                if (!(namen.Contains(child.Name)))
                {
                    namen.Add(child.Name);
                }
            }
        }


        public void ObserveAndPlay()
        {
            processes = Process.GetProcessesByName(processName);
            Process current = Process.GetCurrentProcess();
            Process proc = null;

            // if process already started, get the window
            if (processes.Length > 0)
            {
                proc = processes[0];

                    // check if active window is the needed process' window, switch to if not
                    if(current.Id != proc.Id)
                     
                    {
                        SwitchToThisWindow(proc.MainWindowHandle, true);
                    }
            }
            // else start needed program
            else
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = processName;
                proc = Process.Start(startInfo);
                Console.WriteLine(processName + " wurde gestartet!");
                Thread.Sleep(1000);
                

            }
            
           // SystemWindow window = new SystemWindow(GetForegroundWindow());
            SystemWindow window2 = new SystemWindow(proc.MainWindowHandle);

            SystemWindow win = new SystemWindow(GetActiveWindow());

           // WindowContent content =  window.Content;
           // Console.WriteLine("short: " + content.ShortDescription + "| long " + content.LongDescription);
            Console.WriteLine("==========================" 
              //+ "\r" + window.Title 
                + "\r" + window2.Title);
            SystemWindow[] progWindows = window2.AllChildWindows;
            SystemWindow[] progWindows1 = window2.AllDescendantWindows;
            SystemAccessibleObject sao = null;
            /*
            foreach (SystemWindow wdw in progWindows1)
            {
                

                Console.WriteLine("--------------" + sao.Name);
                Console.WriteLine(
                    "title:" + wdw.Title + "\t" +
                    "class:" + wdw.ClassName + "\t" +
                    "type:" + wdw.GetType() + "\t"
                    + "l:" + wdw.Rectangle.Left + " t:" + wdw.Rectangle.Top + " breite:" + wdw.Rectangle.Width + " hoehe:" + wdw.Rectangle.Height);
            }
            */

            Control cntr = new Control();

            SystemWindow www = new SystemWindow(cntr);

            sao = SystemAccessibleObject.FromWindow(window2, AccessibleObjectID.OBJID_WINDOW);

            

            if (sao != null)
            {

                allreallyAllChilds(sao);
              
            }
            /*
            foreach (SystemAccessibleObject child in allSaoRefs)
            {
                Console.WriteLine("Object Name : " + child.Name + "\tlocation : " + child.Location);
            }
            */
            foreach (string child in namen)
            {
                Console.WriteLine("Object Name : " + child);
            }
            /*
            sao = SystemAccessibleObject.FromWindow(window2, AccessibleObjectID.OBJID_TITLEBAR);

            if (sao != null)
            {
                SystemAccessibleObject[] bÃ€mChilds = sao.Children;
                foreach (SystemAccessibleObject child in bÃ€mChilds)
                {

                    Console.WriteLine("Titelbar" + child.Name);
                }
            }

            sao = SystemAccessibleObject.FromWindow(window2, AccessibleObjectID.OBJID_SYSMENU);

            if (sao != null)
            {
                SystemAccessibleObject[] bÃ€mChilds = sao.Children;
                foreach (SystemAccessibleObject child in bÃ€mChilds)
                {

                    Console.WriteLine("System Menu" + child.Name);
                }
            }
            /*
            foreach (SystemWindow wdw in progWindows)
            {
                Console.WriteLine(
                    "title:" + wdw.Title + "\t" +
                    "class:" + wdw.ClassName + "\t" +
                    "type:" + wdw.GetType() + "\t" 
                    + "l:" + wdw.Rectangle.Left + " t:" + wdw.Rectangle.Top + " breite:" + wdw.Rectangle.Width + " hoehe:" + wdw.Rectangle.Height);
            }
             * */
            IntPtr menu = GetMenu(proc.MainWindowHandle);
            
            int menuCount = GetMenuItemCount(menu);
            Console.WriteLine("menÃŒ " + menu + " hat menuCount: " + menuCount);
            
                
            Play();

        }


        private void Play()
        {

            Console.WriteLine(" play method");
        }

            /*
            System.Windows.Point point = new System.Windows.Point();
            while (true)
            {
                //Thread.Sleep(100);
                //Statt Mausklick + Strg nur Strg ist besser
                //Wenn Mausklick + Strg wird das jeweilige Dropdown geÃ¶ffnet
                //Maus muss ÃŒber einem "Prozessschritt" sein, dann Strg drÃŒcken -> Ausgabe Konsole
                //Start(point, technology);
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                {
                    //Dictionary[STRG] = "true";
                }
                   
                if (_event != null)
                {
                    //_event(Dictionary, new EventArgs());
                }
                
            }
        }
  

        private void Start(System.Windows.Point point, Technologies.Technology tech)
        {            
            GetCursorPos(ref MousePoint);

            XKoordinate = MousePoint.X;
            YKoordinate = MousePoint.Y;

            point.X = XKoordinate;
            point.Y = YKoordinate;
            
            SystemAccessibleObject accessibleObject = null;
            try
            {
                accessibleObject = SystemAccessibleObject.FromPoint((int)point.X, (int)point.Y);
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler: " + e.Message);
            }

            try
            {
                //Console.WriteLine("##########" + accessibleObject.Window.Process);
                //Console.WriteLine("##########" + accessibleObject.Parent.Name);
                SystemWindow window = new SystemWindow(accessibleObject.Window.HWnd);
                if (window != null)
                {
                    Console.WriteLine(window.WindowState);
                }
            }
            catch (Exception e)
            {

                Console.WriteLine("Fehler: " + e.Message);
            }

            if (accessibleObject != null)
            {
                SystemAccessibleObject saoRef = accessibleObject;

                //Process[] processes = Process.GetProcessesByName("devenv");
                Process[] processes = Process.GetProcesses();
                if (processes.Length > 0)
                {
                    foreach (Process p in processes)
                    {
                        if (p.ProcessName.Equals("notepad++"))
                        {
                            SetForegroundWindow(p.MainWindowHandle);
                        }
                        else
                        {
                            
                            ProcessStartInfo startInfo = new ProcessStartInfo();
                            startInfo.FileName = "notepad++";
                            Process.Start(startInfo);
                        }
                        // if (processes[i].ProcessName.Equals("devenv"))
                        if ((int)p.MainWindowHandle != 0)
                        {


                            // SystemWindow window = processes[i].BasePriority;

                            //Hauptfenster ist vorhanden, der Prozess ist also eine Anwendung:
                            //Prozessname und Titel des Hauptfensters auslesen
                            Console.WriteLine("windowhandle: " + p.MainWindowHandle);
                            Console.WriteLine("type: " + p.MainWindowHandle.GetType());
                            Console.WriteLine("Prozessname: {0}\r\nFenstertitel:{1}\r\n",
                                p.ProcessName, p.MainWindowTitle);
                        }
                    }
                }
                
                Console.WriteLine(SystemWindow.ForegroundWindow.CheckState);
                Console.WriteLine("(" + saoRef.Location.X + " | " + saoRef.Location.Y + ") ID: " + saoRef);
            }

            string Name = tech.GetNameByPosition(point);
            if (Name.Equals("no name"))
            {
                point = new System.Windows.Point(accessibleObject.Location.X, accessibleObject.Location.Y - 0.5);
                Name = tech.GetNameByPosition(point);
            }
            string Type = tech.GetTypeByPosition(point).ToString();
            string Program = tech.GetProcessNameByPosition(point);
            string Window = tech.GetWindowNameByPosition(point);
            string Info = String.Format("Info: Name = {0} Program = {1}", Name, Program);

            //Dictionary[INFO] = Info;
            //Dictionary[NAME] = Name;
            //Dictionary[PROGRAMM] = Program;
            //Dictionary[TYPE] = Type;
            //Dictionary[WINDOW] = Window;
        }

        //Get-Method for the x-coodinate
        public static int XKoordinate
        {
            set { _xKoordinate = value; }
            get { return _xKoordinate; }
        }

        //Get-Method for the y-coodinate
        public static int YKoordinate
        {
            set { _yKoordinate = value; }
            get { return _yKoordinate; }
        }
             */
        
    }
}

