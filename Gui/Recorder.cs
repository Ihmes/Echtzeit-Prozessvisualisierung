using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ManagedWinapi.Accessibility;
using System.Runtime.InteropServices;
using Technologies;
using ManagedWinapi.Windows;
using System.Windows;
using Database;

namespace Gui
{
    public class Recorder
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
        private delegate void SenderHandler(object sender, EventArgs e);
        private event SenderHandler _event;
        private IDictionary<string, string> Dictionary;
        public static string INFO = "info";
        public static string NAME = "name";
        public static string TYPE = "type";
        public static string PROGRAMM = "program";
        public static string WINDOW = "window";
        public static string PARENT = "parent";
        public static string STRG = "strg";
        public static int counter = 0;
        private static int processCounter = 0;
        private string selectedName = "";
        List<string> processName;
        bool firstTime = false;
        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();

        private static List<String> allChilds;
        
        List<string> blacklist = new List<string>();
        SystemAccessibleObject saoRef;
        SystemAccessibleObject parent;

       
        public Recorder(RecordFrame recordFrame)
        {
            _event += new SenderHandler(recordFrame.MakeRecorderActions);
            Dictionary = new SortedDictionary<string, string>();

            allChilds = new List<string>();
        }

        public void StartMousePointer()
        {
            Technologies.Technology tech = new Technologies.Technology();

            Dictionary<string,string> allProccessSteps = getAllProcessSteps();
 
            System.Windows.Point point = new System.Windows.Point();
            while (true)
            {
                //Thread.Sleep(100);
                //Statt Mausklick + Strg nur Strg ist besser
                //Wenn Mausklick + Strg wird das jeweilige Dropdown geöffnet
                //Maus muss über einem "Prozessschritt" sein, dann Strg drücken -> Ausgabe Konsole
                Start(point, tech, allProccessSteps);
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                {
                    
                    Dictionary[STRG] = "true";
                    //children(saoRef,parent);
                }

                if (_event != null)
                {
                    _event(Dictionary, new EventArgs());
                }

            }
        }

        private static Dictionary<string, string> getAllProcessSteps()
        {
            //Create new ProjectRepository object to get db access.
            Database.Repository.ProjectRepository projectRepo = new Database.Repository.ProjectRepository();
            //Get all Projects form the DB
            IList<Database.Domain.Project> allProjects = projectRepo.GetAllProjects();

            //List for all ProcessStep names
            Dictionary<string,string> allSteps = new Dictionary<string,string>();

            foreach (Database.Domain.Project allP in allProjects)
            {
                foreach (Database.Domain.Process pp in allP.Processes)
                {
                    IList<Database.Domain.ProcessStep> allPS = pp.ProcessSteps;
                    if (allPS.Count > 0 && pp != null)
                    {
                        allSteps.Add(pp.Name,allPS.ElementAt(0).Name);
                        Console.WriteLine("Prozess aus " + allP.Name + "   Name : " + allPS.ElementAt(0).Name);
                    }
                }
            }
            return allSteps;
        }


        private void Start(System.Windows.Point point, Technologies.Technology tech, Dictionary<string,string> allProcessSteps)
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
            catch (Exception)
            {
                Console.WriteLine("Fehler");
            }

            try
            {
                // Console.WriteLine("##########" + accessibleObject.Window.Process);
                //Console.WriteLine("##########" + accessibleObject.Parent.Name);
                
                SystemWindow window = new SystemWindow(accessibleObject.Window.HWnd);
                IntPtr pointer = GetActiveWindow();
                SystemWindow window2 = new SystemWindow(pointer);
                //IntPtr comboboxex = FindWindowEx(HWnD, 0, "comboboxex32", null);
                //IntPtr combo = FindWindowEx(comboboxex, 0, "ComboBox", null);
                //IntPtr edit = FindWindowEx(combo, 0, "Edit", null);

                //// note: 3rd child of "Edit" may work, too
                //IntPtr intptrEdit = FindWindowEx(intptrAddressBand, IntPtr.Zero, "Edit", null);
                //var sao = SystemAccessibleObject.FromWindow(new ManagedWinapi.Windows.SystemWindow(intptrEdit), ManagedWinapi.Accessibility.AccessibleObjectID.OBJID_WINDOW);
                //var url = sao.Children[3].Value;

                if (window != null)
                {
                    Console.WriteLine(window +"   "+ window.ClassName +"        "+ window.Title);
                }
            }
            catch (Exception)
            {

                Console.WriteLine("Fehler");
            }

            saoRef = accessibleObject;

            //Console.WriteLine(accessibleObject.Location.X + " - " + accessibleObject.Location.Y + " ID: " + accessibleObject);

            //TODO
            //Try catch um if
            string Name = tech.GetNameByPosition(point);
            if (Name.Equals("no name"))
            {
                point = new System.Windows.Point(accessibleObject.Location.X, accessibleObject.Location.Y - 0.5);
                Name = tech.GetNameByPosition(point);
            }
            string parentName = "";
            try
            {
                parentName = saoRef.Parent.Name;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            string Type = tech.GetTypeByPosition(point).ToString();
            //string processName = tech.GetProcessNameByPosition(point);
            //string Program = Technologies.Technology.getApplicationNameByProcessName(tech.GetProcessNameByPosition(point));
            string Program = tech.GetProcessNameByPosition(point);
            string Window = tech.GetWindowNameByPosition(point);
            if (Window != null &&  Window.Equals(""))
            {
                SystemWindow window = saoRef.Window;

                Console.WriteLine("window name: " + window.Title);
                //höchstes parentwindow holen
                while (window.Title.Equals(""))
                {
                    window = window.Parent;
                    Console.WriteLine("Fenstername : " + window.Title);
                }

                Window = window.Title;
            }
            string Parent = "";
            int c = 0;
            try
            {
                if (saoRef.Parent.Name != null)
                {
                    while (Parent != null && Parent.Equals("") && c < 10)
                    {
                        try
                        {
                            Parent = saoRef.Parent.Name;
                        }
                        catch (COMException e)
                        {

                        }
                        c++;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            string Info = String.Format("Info: Name = {0} Program = {1}", Name+"  parent: "+parentName, Program);

            

          //  var tecIdsAcc = tech.GetIdsByPosition(point, null);
            //SystemAccessibleObject[] allChildren = saoRef.Children;


            
            //Rekursive Methode findet Objekte oft doppelt, wie Date zum Beispiel. Einmal beim "normalen" 
            //durchlaufen und ein
            //zweites mal, wenn es diesen Reiter öffnent



            //dbCompareWithSelectedObj(saoRef, allProcessSteps);


            /*foreach (SystemAccessibleObject childs in allChildren)
            {
                try
                {
                    Console.WriteLine("-----------------------ChildName : " + childs.Name + "    Parent: " + childs.Parent);

                }
                catch (COMException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            String wtitle = saoRef.Window.Title;

            SystemWindow ww = new SystemWindow(new Control(wtitle));
            SystemAccessibleObject aa = SystemAccessibleObject.FromWindow(ww, AccessibleObjectID.OBJID_WINDOW);

*/
            //Console.WriteLine("!!!!!!!!!!!!!!!!!!!!  " + ww.Title + "     " + aa.Name + "   loc" + aa.Location);
            counter = 0;
            
            /*
            int count = 0;
            foreach (var id in tecIdsAcc)
            {
                Dictionary[count + ""] = id;
                count++;
                Console.WriteLine("++++++++++++++" + id);
                //   SystemAccessibleObject test = new SystemAccessibleObject(id, 0);
            }
            // content.Add(CreateTextElement("ACC [" + (count++) + "]: " + id, GetColor(id)));
            */
            Dictionary[INFO] = Info;
            Dictionary[NAME] = Name;
            Dictionary[PROGRAMM] = Program;
            Dictionary[TYPE] = Type;
            Dictionary[WINDOW] = Window;
            Dictionary[PARENT] = Parent;
        }

        private void dbCompareWithSelectedObj(SystemAccessibleObject sao, Dictionary<string,string> allProcessSteps)
        {
            //Create new ProjectRepository object to get db access.
            Database.Repository.ProcessRepository processRepo = new Database.Repository.ProcessRepository();
            //falls späterer prozessschritt auf 1. ebene eventuell mit parent+childs beim ersten schritt die objekte holen
           
            foreach (string dbnames in allProcessSteps.Values)
            {
                processName = new List<string>();
                if (!selectedName.Equals(sao.Name))
                {
                    
                    blacklist.Clear();
                    processCounter = 0;
                }
                if(sao.Name!=null && sao.Name.Equals(dbnames))
                {
                    if (firstTime == false)
                    {
                        //children füllt die zwei statischen Listen childs und chilsOfParents mit
                        //Objekten vom Typ SystemAccessibleObject. 
                        children(sao, sao.Parent);
                        firstTime = true;
                    }
                    //alle eltern und kindern in eine ObjectListe speichern
                    

                    //Object visuell markieren!
                    
                    // TODO

                    foreach (string key in allProcessSteps.Keys)
                    {
                        string value = "";
                        bool test = allProcessSteps.TryGetValue(key, out value);
                        //Console.WriteLine("DB Name = " + dbnames + "  aus Map = " + value);
                        if (dbnames.Equals(value))
                        {
                            Console.WriteLine("Process zu " + dbnames + " ist " + key);
                            if (!key.Equals("") && key != null)
                            {
                                processName.Add(key);
                                
                            }
                        }
                    }
                    if (processName.Count() > 1)
                    {
                        //neues Fentser erstellen
                    }
                    else
                    {
                        Database.Domain.Process processObject = new Database.Domain.Process();
                        //Kindelemente markieren
                        IList<Database.Domain.Process> allProcessesFromDB = processRepo.GetAllProcesses();
                        foreach (Database.Domain.Process process in allProcessesFromDB)
                        {
                            if(process.Name.Equals(processName.ElementAt(0)))
                            {
                                processObject = process;
                            }
                        }
                        //Liste mit allen Prozessschritten des ausgewählten Prozesses!
                        IList<Database.Domain.ProcessStep> processList = processObject.ProcessSteps;
                        //TODO
                        //Markieren aller sichtbaren Elemente!
                        //Alle Childs (ständig neu berechnen) mit processList abgleichen
                        //

                    }

                    selectedName = sao.Name;
                    Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!   Name : " + sao.Name);
                }
            }

            SystemAccessibleObject[] allChildren = sao.Children;

            foreach (SystemAccessibleObject allObjects in allChildren)
            {
                foreach (string dbnames in allProcessSteps.Values)
                {
                    //Console.WriteLine("+++++++++++++++++     Name DB: " + dbnames + "    Name object: " + allObjects.Name);
                    Boolean weiter = true;
                    if (allObjects.Name != null && allObjects.Name.Equals(dbnames))
                    {
                        foreach (string blacklistItem in blacklist)
                        {
                            if (allObjects.Name.Equals(blacklistItem))
                            {
                                weiter = false;
                            }
                        }
                        if (weiter == true)
                        {
                            Console.WriteLine("#############        Name: " + allObjects.Name + "   Location: " + allObjects.Location);
                        }
                        weiter = true;
                        blacklist.Add(allObjects.Name);
                    }
                }
            }
        }





        private static void children(SystemAccessibleObject saoRef, SystemAccessibleObject parentObject)
        {
            SystemAccessibleObject[] childs = null;
            SystemAccessibleObject[] childsOfParent = null;
            try
            {
                childs = saoRef.Children;
                childsOfParent = parentObject.Children;
            }
            catch (COMException e)
            {
            }

            if (childs != null)
            {
                foreach (SystemAccessibleObject child in childs)
                {
                    try
                    {
                        Console.WriteLine("Name : " + child.Name + "    Location: " + child.Location);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }
            }
            Console.WriteLine("-----------------------------------");

            if (childsOfParent != null)
            {
                foreach (SystemAccessibleObject childOfP in childsOfParent)
                {
                    try
                    {
                        Console.WriteLine("Name : " + childOfP.Name + "    Location: " + childOfP.Location);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }
            }

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

        private void getAllChildren(SystemAccessibleObject[] childd)
        {
            counter++;
            Console.WriteLine(counter);
            foreach (SystemAccessibleObject dd in childd)
            {

               
                Accessibility.IAccessible test = dd.IAccessible;


                if (dd != null)
                {
                    SystemAccessibleObject childOMine = new SystemAccessibleObject(test, 0);
                    if (childOMine != null)
                    {
                        Console.WriteLine("ChildOMine name: " + childOMine.Name);
                    }
                    else
                    {
                        Console.WriteLine("childOOOO ist null");
                    }
                    
                    try
                    {
                       

                        Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaaa233423fhasdlfalsdfjaldsfjalsdfjaldsfjalsfjalfjdlsjfa");
                        Console.WriteLine("name : " + dd.Name + "    loco:" + dd.Location + " mousepoint : X "+XKoordinate+" : Y "+YKoordinate);

                        allChilds.Add(dd.Name + "LOCATION:" + dd.Location);

                        SystemAccessibleObject[] kinder = dd.Children;

                        if (kinder.Length!=0)
                        {

                            Console.WriteLine("-----------------------------------------------------------------");

                            getAllChildren(childOMine.Children);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                else
                {
                    
                    break;
                }
            }
        }
    }
}
