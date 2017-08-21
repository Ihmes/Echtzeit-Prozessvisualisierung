using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using ManagedWinapi.Accessibility;
using System.Runtime.InteropServices;
using ManagedWinapi.Windows;
using Database.Repository;
using Database.Domain;
namespace Gui
{

    class Visualizer
    {
        /*
        public static string STRG = "strg";
        
        SystemAccessibleObject parent;
        
        List<string> blacklist = new List<string>();
        
        
        

        
        SystemWindow win;
        
      
       
        
        InfoRectangle neuesRect;
        public static string objectName;
        
        public static Database.Domain.Process selectedProcess;
        
        //public static List<InfoRectangle> rectanglePool;
        
        
        

        
         * 
        */
        InfoRectangle neuesRect;
        public static string objectName;
        public static string descriptionTitle = "0";
        public static string descriptionProcessTitle = "0";
        public static Database.Repository.ProcessRepository repo;

        public static IList<Process> processList;

        bool dropdown;
        bool bool_newWindowOpen = false;
        SystemAccessibleObject[] allChilds;
        public static bool isDrawn = false;
        List<string> processStepNames;
        public static InfoRectangle rectangle;
        bool bool_found;
        bool bool_firstTime = false;
        bool bool_drawnAllObjects = false;

        static bool bool_neuerName;
        public static int int_processCounter = 0;
        public static int int_newPointer = 0;
        public static int int_drawCounter = 0;
        public static SystemAccessibleObject saoRef;

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref System.Drawing.Point lpPoint);
        System.Drawing.Point MousePoint = new System.Drawing.Point();
        private string string_selectedName = "";

        public string string_processName;
        public string actualProgram = "";

        public static InfoRectangle[] rectanglePool;

        ProcessstepView obj_anzeige;
        ProjectRepository obj_projectRepo;
        IList<Project> iList_allProjectsFromDB;

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetActiveWindow();
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        private static extern Int32 getWindowProcessID(IntPtr hwnd);
        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        public static List<Database.Domain.Process> list_allProcesses;
        public static IList<Database.Domain.ProcessStep> list_selectedProcessSteps;

        public static List<SystemAccessibleObject> list_firstLevelObjects;
        public static List<ProcessStep> list_firstLevelSteps;

        public static List<InfoRectangle> list_drawableRectangles;
        public static List<InfoRectangle> list_drawableRectanglesInNewWindow;
        public static List<InfoRectangle> list_drawableRectanglesFirstLevel;

        public static int int_amountFirstLevelObjects = 0;

        public static bool bool_notSelected = true;
        public static bool bool_newProgramIsOpen = false;
        public static bool bool_allowedToDraw = true;
        public static bool bool_allowedToDrawnewWindow = true;
        public static Process TempProz;
        public static string string_window;


        public bool bool_newProgram = false;
        private static int int_helpProcessCounter = 0;
        private delegate void SenderHandler(object sender, EventArgs e);
        private event SenderHandler _event;
        private event SenderHandler _Seledevent;

        public List<Database.Domain.ProcessStep> list_allProcessStepsfromDB;

        Dictionary<string, string> map_ProcessNameProcessStep;
        //SystemAccessibleObject saoObj;
        System.Diagnostics.Stopwatch stopWatch;
        System.Diagnostics.Stopwatch stopWatchNewProgram;
        System.Windows.Point point;
        Technologies.Technology tech;
        List<SystemAccessibleObject> list_windowObjects;
        public static List<SystemAccessibleObject> list_AllObjects;
        SystemAccessibleObject saoObj;

        SystemAccessibleObject defaultSaoObj;


        private System.Diagnostics.Process[] processes = null;

        public static void julia(Database.Domain.Process process)
        {
            /*
            int c = 0;
            IList<ProcessStep> selectedProcessSteps = process.ProcessSteps;
            foreach (ProcessStep step in selectedProcessSteps)
            {
                Console.WriteLine("Process : " + process.Name + "  with Step number : " + c + " and stepname = " + step.Name);
                c++;
            }
             */
        }
        public Visualizer(ProcessstepView anzeige)
        {
            //init before while-true starts!!
            //
            defaultSaoObj = null;
            string_window = "";
            list_drawableRectangles = new List<InfoRectangle>();
            list_drawableRectanglesInNewWindow = new List<InfoRectangle>();
            list_drawableRectanglesFirstLevel = new List<InfoRectangle>();
            list_firstLevelObjects = new List<SystemAccessibleObject>();
            list_firstLevelSteps = new List<ProcessStep>();
            list_allProcesses = new List<Process>();
            list_allProcessStepsfromDB = new List<ProcessStep>();
            list_selectedProcessSteps = new List<ProcessStep>();
            repo = new ProcessRepository();
            processList = repo.GetAllProcesses();
            map_ProcessNameProcessStep = new Dictionary<string, string>();

            this.obj_anzeige = anzeige;
            _event += new SenderHandler(obj_anzeige.Actions);
            _Seledevent += new SenderHandler(obj_anzeige.highlight_current_step);
            //Create new ProjectRepository object to get db access.
            obj_projectRepo = new ProjectRepository();
            //Get all Projects form the DB
            iList_allProjectsFromDB = obj_projectRepo.GetAllProjects();

            list_AllObjects = new List<SystemAccessibleObject>();
            getAllProcessSteps();
            tech = new Technologies.Technology();
            point = new System.Windows.Point();
            stopWatch = new System.Diagnostics.Stopwatch();
            stopWatchNewProgram = new System.Diagnostics.Stopwatch();
            list_windowObjects = new List<SystemAccessibleObject>();


            saoObj = null;
            bool_found = false;
            dropdown = false;

            objectName = "";
            /*
            allProcessSteps = new List<ProcessStep>();
            mapProcessNameProcessStep = new Dictionary<string, string>();
            System.Diagnostics.Process nPad = new System.Diagnostics.Process();
            Technologies.Technology tech = new Technologies.Technology();
            processName = "";
            
            nPad.StartInfo.FileName = "notepad++.exe";
            nPad.StartInfo.Arguments = "text.txt";
            nPad.Start();
            
            win = null;
           
            neuerName = true;
            allProcesses = new List<Database.Domain.Process>();
            
           
            /*
            recognize();
            */

            
        }



        public void recognize()
        {
            //startet vergleich von DB und Mousepointer



            while (true)
            {
                //SystemAccessibleObject sao = SystemAccessibleObject.FromPoint((int)point.X, (int)point.Y);


                /*
                win = new SystemWindow(GetActiveWindow());
                Console.WriteLine("Active : -!------------------" + win.ToString() +" ------------" + GetActiveWindow());
               // Console.WriteLine("-!------------------" + win.ClassName);
                win = new SystemWindow(new Control());
                Console.WriteLine("Leer : -!------------------" + win.ToString() + " ------------" + GetActiveWindow());


                Int32 pid = getWindowProcessID(GetActiveWindow());
                win = new SystemWindow(GetForegroundWindow());
                 * 
                Console.WriteLine("Foreground : -!------------------" + win.ToString() + " ------------" + GetActiveWindow());
                */

                // string_processName = Technologies.Technology.getApplicationNameByProcessName(tech.GetProcessNameByPosition(point));

                //Console.WriteLine("Counter : " + int_processCounter);

                string_processName = tech.GetProcessNameByPosition(point);
                //Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!");
                //Console.WriteLine("trolololol " + string_processName);
                Start();
                updateAllWindows();
            }

        }

        public void updateAllWindows()
        {
            if (rectanglePool != null)
            {
                int number = 1 + int_drawCounter;
                if (_Seledevent != null && int_helpProcessCounter != int_processCounter)
                {
                    _Seledevent(int_processCounter, new EventArgs());
                    int_helpProcessCounter = int_processCounter;
                }
                foreach (InfoRectangle rec in rectanglePool)
                {
                    if (rec != null && !rec.IsDisposed)
                    {



                        string zahl = "" + number;
                        rec.DrawString(zahl);
                        try
                        {
                            rec.Update();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Fehler in: public void updateAllWindows()" + e.StackTrace);
                            recognize();
                        }
                    }
                    number++;
                }

            }
        }

        public bool sameProgram(IList<ProcessStep> processSteps)
        {
            if (processSteps.Count() < int_processCounter + 1)
            {
                if (!(processSteps.ElementAt(int_processCounter).Program.Equals(processSteps.ElementAt(int_processCounter + 1).Program)))
                {
                    return false;
                }
            }
            return true;
            //selectedProcess
        }
        public bool sameWindow(IList<ProcessStep> processSteps)
        {
            if (processSteps.Count() < int_processCounter + 1)
            {
                if (!(processSteps.ElementAt(int_processCounter).Window.Equals(processSteps.ElementAt(int_processCounter + 1).Window)))
                {
                    return false;
                }
            }
            return true;
        }

        private void Start()
        {
            GetCursorPos(ref MousePoint);

            XKoordinate = MousePoint.X;
            YKoordinate = MousePoint.Y;

            point.X = XKoordinate;
            point.Y = YKoordinate;

            getPointFromMouseposition();

            if (bool_notSelected == true)
            {
                dbCompareWithSelectedObj(saoRef);
            }
            else
            {
                StartNewProgram();
                //childsOfSelectedObject(saoRef);
            }

        }

        private void StartNewProgram()
        {
            showAll();
            FindChilds();
            nextStepInNewWindoworProgram();
            isNewWindow();
            highlighten();
            
            
            // Gibt die Beschreibung der Prozessschritte aus
            //BalloonTip();

        }

        private void BalloonTipProject(String key)
        {

            if (descriptionProcessTitle.Equals("0"))
            {
                descriptionProcessTitle = key;
            }

            foreach (Process prozess in processList)
            {
                if (prozess.Name.Equals(key))
                { 
                    TempProz = prozess;
                   
                    if (!TempProz.Equals(null))
                    {
                        if (!descriptionProcessTitle.Equals(TempProz.Name))
                        {
                            obj_anzeige.showBalloonTip("Project Description:", TempProz.Description);
                            descriptionProcessTitle = TempProz.Name;
                            break;
                        }
                    }
                }
            }
         }
        
      

        private void BalloonTip() {

            if (descriptionTitle.Equals("0"))
            {
                descriptionTitle = saoRef.Name;
            }
            
            foreach (ProcessStep step in list_selectedProcessSteps)
            {   
                if (saoRef.Name.Equals(step.Name) && saoRef.Parent.Name.Equals(step.Parent))
                {
                    if(!descriptionTitle.Equals(saoRef.Name))
                    {
                        Console.WriteLine("name sao  " + saoRef.Name + "   DesTitle   " + descriptionTitle);
                        obj_anzeige.showBalloonTip("Description:", step.Description);
                        descriptionTitle = step.Name;
                        break;
                    }
                    else {
                        Console.WriteLine("name sao  " + saoRef.Name + "   DesTitle   " + descriptionTitle);
                    }
                }
            }
        }

        private void highlighten()
        {
            try
            {
                int c = 1;
                
                foreach(ProcessStep step in list_selectedProcessSteps){

                    if (saoRef.Name.Equals(step.Name) && saoRef.Parent.Name.Equals(step.Parent))
                    {

                /**    if(descriptionTitle.Equals("0")){
                            descriptionTitle=step.Name;
                            Console.WriteLine("AAA!AAAAAAAAAAAAAANNNFAAAAAAAAAAANG");
                    }
                    if(!descriptionTitle.Equals(step.Name)){
                        Console.WriteLine("Desc:"  + descriptionTitle);
                        Console.WriteLine("Name:" + step.Name);
                        obj_anzeige.showBalloonTip("Description:", step.Description);
                        Console.WriteLine("FOOOOOOOOORTSETZUNG-----------------fffffffffffffffffffffffffffffffff");
                    }
                 **/    

                        

                        if (_Seledevent != null)
                        {
                            _Seledevent(c, new EventArgs());
                          
                            
                        


                   //int_helpProcessCounter = int_processCounter;
                        }
                    }
                    c++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler in highlighten " + e.StackTrace);
                recognize();
            }
        }

        private void isNewWindow()
        {
            string Window = "";
            try
            {
                Window = saoRef.Window.Title;

                if (Window != null && Window.Equals(""))
                {
                    SystemWindow window = saoRef.Window;

                    //Console.WriteLine("window name: " + window.Title);
                    //höchstes parentwindow holen
                    while (window.Title.Equals(""))
                    {
                        window = window.Parent;
                        //Console.WriteLine("Fenstername : " + window.Title);
                    }

                    Window = window.Title;
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("Fehler in isNewWindow " + e.StackTrace);
                Start();
            }
            if (!string_window.Equals(Window))
            {

                Console.WriteLine("hier hier neues Windwooooooooooooooooooooooooooooow");
                Console.WriteLine("string_window : " + string_window + "\t saoref window : " + Window);
                searchNewWindow(Window);

                if (bool_allowedToDrawnewWindow && list_firstLevelObjects.Count > 0)
                {
                    Console.WriteLine("---------------------------------------");
                    //bool_allowedToDraw = false;
                    drawChildsInNewWindow();
                }

            }
            else
            {
                hideChildsfromNewWindow();
            }


            string a = string_processName;
        }

        private void searchNewWindow(string windowTitle)
        {
            getNextStepOnNewProgram();
        }

        private void getNextStepOnNewProgram()
        {
            // SystemWindow window = sao.Window;
            //Console.WriteLine("windooooow : " + window.Title);
            //SystemAccessibleObject sao_obj = getSaoFromHighestWindow(window);

            //Console.WriteLine("Window :" + window + "\t saoobj :" + saoObj + "\t stepname :" + processStepName);

            if (saoRef != null)
            {
                oneChild(saoRef);
            }
        }

        private void oneChild(SystemAccessibleObject saoRef)
        {
            SystemAccessibleObject[] childs = saoRef.Children;
            if (childs != null)
            {
                foreach (SystemAccessibleObject child in childs)
                {
                    // SystemAccessibleObject[] saoref = child.Children;
                    int c = 1;
                    foreach (ProcessStep step in list_selectedProcessSteps)
                    {
                        if (child != null && child.Name != null && child.Name.Equals(step.Name) && child.Parent.Name.Equals(step.Parent))
                        {

                            Console.WriteLine("oneChild(SystemAccessibleObject saoRef)-----------gefunden: " + child.Name + "   " + child.Location);
                            /*
                            saoRef = child;
                            saoObj = saoRef;
                            break;
                            */
                            neuesRect = new InfoRectangle(child.Location);
                            neuesRect.setNumber(c);
                            neuesRect.Visible = true;
                            neuesRect.TopMost = true;
                            neuesRect.DrawString("" + c);
                            neuesRect.Update();

                            if (!list_drawableRectanglesInNewWindow.Contains(neuesRect))
                            {
                                list_drawableRectanglesInNewWindow.Add(neuesRect);
                            }
                        }
                        c++;

                    }
                    oneChild(child);
                }
            }
        }

        private void nextStepInNewWindoworProgram()
        {
            for (int i = 0; i < list_selectedProcessSteps.Count; i++)
            {
                ProcessStep step = list_selectedProcessSteps.ElementAt(i);
                try
                {
                    if (step.Name.Equals(saoRef.Name) && step.Parent.Equals(saoRef.Parent.Name))
                    {
                        Console.WriteLine("hier hier -------------------- hier hier ");
                        //wenn im neuen programm dann programm öffnen
                        if ((i + 1) < list_selectedProcessSteps.Count && !step.Program.Equals(list_selectedProcessSteps.ElementAt(i + 1).Program))
                        {
                            Console.WriteLine("nächster schritt in neuem programm");
                        }


                        //wenn in neuem fenster dann buble andere farbe
                        else if ((i + 1) < list_selectedProcessSteps.Count && !step.Window.Equals(list_selectedProcessSteps.ElementAt(i + 1).Window))
                        {
                            Console.WriteLine("nächster schritt in neuem Fenster");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Fehler in nextStepInNewWindoworProgram " + e.StackTrace);
                    Start();
                }
            }
        }

        private void FindChilds()
        {
            try
            {
                SystemAccessibleObject foundSao = null;
                //Ist die Maus auf einem Objekt der 1. Ebene?
                //Wenn setze foundSao auf dieses Objekt
                foreach (SystemAccessibleObject sao in list_firstLevelObjects)
                {

                    try
                    {
                        //Console.WriteLine("geht !! SaoRef : " + saoRef.Name + "\t sao in list_firstLevelObjects : " + sao.Name);
                        if (saoRef.Name.Equals(sao.Name) && sao.Parent.Name.Equals(saoRef.Parent.Name))
                        {
                            foundSao = sao;

                            if (defaultSaoObj != null && !defaultSaoObj.Name.Equals(foundSao.Name))
                            {
                                if (stopWatchNewProgram.ElapsedMilliseconds > 300)
                                {
                                    hideChilds();
                                }
                            }
                            else
                            {
                                if (!stopWatchNewProgram.IsRunning)
                                {
                                    stopWatchNewProgram.Start();
                                }
                            }
                            defaultSaoObj = sao;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Fehler in FindChilds : " + e.StackTrace);
                        Start();

                    }

                }

                if (foundSao != null)
                {

                    findAndDrawChilds(saoRef);
                }
                else
                {
                    if (!stopWatchNewProgram.IsRunning)
                    {
                        stopWatchNewProgram.Start();
                    }
                    //Nur disposen wenn nicht im dropdown -> saoRef ist kein child von defaultSao
                    if (!isInDropDown())
                    {
                        if (stopWatchNewProgram.ElapsedMilliseconds > 300)
                        {
                            hideChilds();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                recognize();
                Console.WriteLine("Fehler in FindChilds : " + e.StackTrace);
            }
        }

        private void findAndDrawChilds(SystemAccessibleObject saoref)
        {
            try
            {
                Console.WriteLine("kann zeichnen wenn true : " + bool_allowedToDraw);
                getChildsofSaoRef(saoref);
                //draw/find all childs
                findAndAddChilds(allChilds);
                //drawChilds();

                if (bool_allowedToDraw && list_firstLevelObjects.Count > 0)
                {
                    //bool_allowedToDraw = false;
                    drawChilds();
                    hideChildsfromNewWindow();
                    stopWatchNewProgram.Reset();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler in findAndDrawChilds : " + e.StackTrace);
                recognize();
            }
        }

        private bool isInDropDown()
        {
            try
            {
                //Console.WriteLine("default : " + defaultSaoObj.Name + " saoref.parent.name : " + saoRef.Parent.Name);
                if (saoRef.Parent.Name.Equals(defaultSaoObj.Name))
                {
                    findAndDrawChilds(defaultSaoObj);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler in isInDropDown" + e.StackTrace);
                Start();
                //return false;
            }
            return false;
        }

        private void hideChilds()
        {
            foreach (InfoRectangle rect in list_drawableRectangles)
            {
                rect.Dispose();
                rect.Update();
            }
            list_drawableRectangles.Clear();
            bool_allowedToDraw = true;
        }
        private void hideChildsfromNewWindow()
        {
            //Console.WriteLine("what ??????");
            foreach (InfoRectangle rect in list_drawableRectanglesInNewWindow)
            {
                rect.Dispose();
                rect.Update();
            }
            list_drawableRectanglesInNewWindow.Clear();
            bool_allowedToDrawnewWindow = true;
        }

        private void drawChilds()
        {
            Console.WriteLine("sollte zeichnen");
            foreach (InfoRectangle rects in list_drawableRectangles)
            {
                Console.WriteLine("rects : " + rects.getNumber());
                rects.Visible = true;
                rects.TopMost = true;
                rects.DrawString("" + rects.getNumber());
                rects.Update();
            }
            bool_allowedToDraw = false;
            //bool_allowedToDrawnewWindow = false;
        }
        private void drawChildsInNewWindow()
        {
            //Console.WriteLine("sollte zeichnen im  neuem Fenster!!!");
            foreach (InfoRectangle rects in list_drawableRectanglesInNewWindow)
            {
                //Console.WriteLine("rects : " + rects.getNumber());
                rects.Visible = true;
                rects.TopMost = true;
                rects.DrawString("" + rects.getNumber());
                rects.Update();
            }
            //bool_allowedToDraw = false;
            bool_allowedToDrawnewWindow = false;
        }

        private void showAll()
        {
            System.Diagnostics.Stopwatch sp = new System.Diagnostics.Stopwatch();
            sp.Start();
            //|| sp.ElapsedMilliseconds > 7000
            while (!findProgram() )
            {
                recognize();
            }
            sp.Stop();
            sp.Reset();

            if (!bool_drawnAllObjects)
            {
                bool_drawnAllObjects = true;
                Console.WriteLine("nur einmal " + bool_drawnAllObjects);
                getAllFirstLevelObjects(saoRef, actualProgram);
            }
        }

        private bool findProgram()
        {
            foreach (ProcessStep step in list_selectedProcessSteps)
            {
                try
                {
                    string_processName = tech.GetProcessNameByPosition(point);
                    //Console.WriteLine("step.name : "+step.Name+"\t step.program : " + step.Program + "\t string_processName :" + string_processName);
                    //Console.WriteLine("-------------------------------------");
                    //Console.WriteLine("actualprogram : " + actualProgram + "\t string_processName : " + string_processName + "\t step.program : " + step.Program + "\tstepname : " + step.Name);
                    //Console.WriteLine("-------------------------------------");
                    if (string_processName.Equals(step.Program))
                    {

                        if (!actualProgram.Equals(string_processName))
                        {
                            bool_drawnAllObjects = false;
                            
                            hideAllFirstLevel();
                            
                        }
                        actualProgram = step.Program;
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Fehler in findProgram " + e.StackTrace);
                    recognize();
                }
            }
            return false;
        }

        private void childsOfSelectedObject(SystemAccessibleObject saoRef)
        {
            if (!bool_drawnAllObjects)
            {
                Console.WriteLine("nur einmal " + bool_drawnAllObjects);
                getAllFirstLevelObjects(saoRef);
                bool_drawnAllObjects = true;
            }

            try
            {
                //Console.WriteLine("wiiiiiiiiiiiiiichtig : " + int_processCounter);
                //Console.WriteLine("Counter : " + int_processCounter + "\tProcessStep : " + list_selectedProcessSteps.ElementAt(int_processCounter).Name + "\t nexte Process Step : " + list_selectedProcessSteps.ElementAt(int_processCounter + 1).Name);
                //nächster Schritt im selben Programm, wenn ja in einem neuen Fenster?
                //oder nächster Schritt in einem anderen Programm?
                if (list_selectedProcessSteps.Count() >= int_processCounter)
                {
                    //Console.WriteLine("erste if");
                    //Programm vom aktuellen Schritt gleich wie der darauf folgende Schritt?
                    if (int_processCounter > 0 && !list_selectedProcessSteps.ElementAt(int_processCounter - 1).Program.Equals(list_selectedProcessSteps.ElementAt(int_processCounter).Program))
                    {
                        //Neues Programms

                        hideAll();
                        updateAllWindows();
                        int_newPointer = int_processCounter;
                        //bool_notSelected = true;
                        objectInNewProgram(list_selectedProcessSteps.ElementAt(int_processCounter));
                        int_drawCounter = int_processCounter;
                        drawOneObject();
                        setNewList();

                        //bool_notSelected = true;


                        isObjectSelected(saoRef);


                    }
                    else
                    {
                        bool_newProgramIsOpen = false;
                        //Programm ist gleich
                        //ist das fenster vom aktuellen Schritt gleich wie das darauf folgende?
                        if (int_processCounter > 0 && !list_selectedProcessSteps.ElementAt(int_processCounter - 1).Window.Equals(list_selectedProcessSteps.ElementAt(int_processCounter).Window))
                        {
                            //Neues Fenster
                            //alle Objekte holen
                            //hideAll();
                            if (bool_newWindowOpen)
                            {
                                if (saoRef.Name.Equals(list_selectedProcessSteps.ElementAt(int_processCounter).Name))
                                {
                                    int_processCounter++;
                                }
                                else
                                {
                                    objectInSameProgram(list_selectedProcessSteps.ElementAt(int_processCounter));
                                }
                            }
                            else
                            {
                                objectInNewWindow();
                            }

                            try
                            {
                                Console.WriteLine("int_processCounter " + int_processCounter + "\tint_newPointer " + int_newPointer + "\t SaoRef : " + saoRef.Name);
                            }
                            catch (COMException e)
                            {
                                Console.WriteLine(e.StackTrace);
                            }

                            //DrawTwoObjects();

                        }
                        else
                        {
                            //normal weiter
                            Console.WriteLine("---------------hier hier hier-------------");
                            Console.WriteLine("int_processCounter " + int_processCounter + "\tint_newPointer " + int_newPointer);

                            objectInSameProgram();
                            //DrawTwoObjects();
                        }
                    }
                }
                else
                {
                    getChildsOfFirstItem();
                    hide_repaint();
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("Nullpointer in childsOfSelectedObject : " + e.StackTrace);
            }
        }

        private void objectInSameProgram(ProcessStep processStep)
        {
            //list_selectedProcessSteps.ElementAt(int_processCounter)
            //string processName = list_selectedProcessSteps.ElementAt(int_processCounter).Program;
            string processName = processStep.Program;


            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            System.Diagnostics.Process[] procs = System.Diagnostics.Process.GetProcessesByName(processName);
            //processes = System.Diagnostics.Process.GetProcessesByName(processName);
            //System.Diagnostics.Process current = System.Diagnostics.Process.GetCurrentProcess();
            System.Diagnostics.Process proc = null;


            // if process already started, get the window
            if (procs.Length > 0)
            {
                proc = procs[0];
            }
            //Console.WriteLine("current pro :" + current.ProcessName);
            //Console.WriteLine("erstellter pro :" + proc.ProcessName);
            sw.Stop();
            //Console.WriteLine("Prozess erstellen : " + sw.ElapsedMilliseconds + "  " + sw.Elapsed);
            rectanglePool[0].Dispose();

            SystemWindow window2 = new SystemWindow(proc.MainWindowHandle);

            System.Diagnostics.Stopwatch sww = new System.Diagnostics.Stopwatch();
            sww.Start();
            getNextStepOnNewProgramWithProcess(window2, list_selectedProcessSteps.ElementAt(int_processCounter).Name);
            sww.Stop();
            //Console.WriteLine("Rekursion erstellen : " + sww.ElapsedMilliseconds + "  " + sww.Elapsed);
            drawOneObjectInNewWindow();
            //setNewList();
        }

        private void setNewList()
        {
            //TODO
            //list_selectedProcessSteps muss gekürzt/geändert werden sodass das Element vom neuen programm an Stelle 0 ist!
            IList<ProcessStep> argList = list_selectedProcessSteps;
            int max = list_selectedProcessSteps.Count();
            list_selectedProcessSteps = new List<ProcessStep>();
            for (int i = int_newPointer; i < max; i++)
            {
                list_selectedProcessSteps.Add(argList.ElementAt(i));
            }
            int_newPointer = int_processCounter;
        }



        private void isObjectSelected(SystemAccessibleObject sao)
        {
            /*
            foreach (ProcessStep step in list_selectedProcessSteps)
            {
                if (step.Name.Equals(sao.Name))
                {
                    bool_newProgram = true;
                    //int_processCounter++;
                }
            }   
             * */
        }

        private void objectInNewProgram(ProcessStep processStep)
        {
            openNewProgram(processStep);
        }


        private void objectInSameProgram()
        {
            //Fills the field allChilds

            if (allChilds != null)
            {
                IList<ProcessStep> selectedProcessSteps = list_selectedProcessSteps;

                //Console.WriteLine("int_processCounter : " + int_processCounter + "Element : " + selectedProcessSteps.ElementAt(int_processCounter) + " SaoRef : " + saoRef);
                ProcessStep step = selectedProcessSteps.ElementAt(int_processCounter);
                try
                {
                    if (saoRef.Parent.Name != null && saoRef.Parent.Name.Equals(step.Parent) && (step).Name.Equals(saoRef.Name))
                    {
                        //Console.WriteLine("counter sollte ++ ");
                        if (rectanglePool.Length > int_processCounter)
                        {
                            int_processCounter++;
                        }
                        ProcessStep actuallStep = selectedProcessSteps.ElementAt(int_processCounter);
                        ProcessStep previousStep = selectedProcessSteps.ElementAt(int_processCounter - 1);


                        if (!(actuallStep.Parent.Equals(previousStep.Parent) || actuallStep.Parent.Equals(previousStep.Name)) && (int_processCounter > 0 && previousStep.Program.Equals(actuallStep.Program)))
                        {
                            Console.WriteLine("neue ebene");
                            hideAll();
                            updateAllWindows();
                            int_drawCounter = int_processCounter;
                            int_newPointer = int_processCounter;
                            //bool_notSelected = true;

                            string processName = actuallStep.Program;


                            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                            sw.Start();
                            System.Diagnostics.Process[] procs = System.Diagnostics.Process.GetProcessesByName(processName);
                            //processes = System.Diagnostics.Process.GetProcessesByName(processName);
                            //System.Diagnostics.Process current = System.Diagnostics.Process.GetCurrentProcess();
                            System.Diagnostics.Process proc = null;


                            // if process already started, get the window
                            if (procs.Length > 0)
                            {
                                proc = procs[0];
                            }
                            //Console.WriteLine("current pro :" + current.ProcessName);
                            //Console.WriteLine("erstellter pro :" + proc.ProcessName);
                            sw.Stop();
                            Console.WriteLine("Prozess erstellen : " + sw.ElapsedMilliseconds + "  " + sw.Elapsed);
                            rectanglePool[0].Dispose();

                            SystemWindow window2 = new SystemWindow(proc.MainWindowHandle);

                            System.Diagnostics.Stopwatch sww = new System.Diagnostics.Stopwatch();
                            sww.Start();
                            //getNextStepOnNewProgram(window2, actuallStep.Name);
                            getNextStepOnNewProgramWithProcess(window2, actuallStep.Name);
                            sww.Stop();
                            Console.WriteLine("Rekursion erstellen : " + sww.ElapsedMilliseconds + "  " + sww.Elapsed);
                            drawOneObject();
                            setNewList();

                        }
                    }

                }
                catch (COMException e)
                {
                    Console.WriteLine(e.StackTrace);
                }

                findAndAddChilds(allChilds);

                foreach (SystemAccessibleObject child in allChilds)
                {

                    step = selectedProcessSteps.ElementAt(int_processCounter);
                    if (step.Name.Equals(child.Name))
                    {

                        //Console.WriteLine("location von " + child.Name + " ist : " + child.Location);
                        //zeichnen
                        // x und y != 0 falls die Objekte nicht sichtbar sind (nicht sichtbar = location = 0)
                        try
                        {

                            if (child.Location != null && child.Location.X != 0 && child.Location.Y != 0)
                            {

                                //if (neuesRect == null)
                                if (rectanglePool[int_processCounter] == null)
                                {

                                    neuesRect = new InfoRectangle(child.Location);
                                    //neuesRect.Visible = true;
                                    //neuesRect.TopMost = true;
                                    rectanglePool[int_processCounter] = neuesRect;
                                    Console.WriteLine(" rect nummer : " + int_processCounter + " existiert");
                                    if (int_processCounter > 1)
                                    {
                                        //drawAllRectangles();
                                        DrawTwoObjects();
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }
            }
            getChildsOfFirstItem();
            hide_repaint();
        }

        private void findAndAddChilds(SystemAccessibleObject[] allChilds)
        {
            try
            { 
                int c = 1;
                //list_drawableRectangles.Clear();
                foreach (ProcessStep step in list_selectedProcessSteps)
                {

                    //Console.WriteLine("stepname : " + step.Name + "\t counter : " + c);



                    foreach (SystemAccessibleObject child in allChilds)
                    {
                        if (step.Name.Equals(child.Name))
                        {
                            try
                            {

                                if (child.Location != null && child.Location.X != 0 && child.Location.Y != 0)
                                {

                                    neuesRect = new InfoRectangle(child.Location);
                                    neuesRect.setNumber(c);

                                    if (!list_drawableRectangles.Contains(neuesRect))
                                    {
                                        list_drawableRectangles.Add(neuesRect);
                                        neuesRect.Visible = true;
                                        neuesRect.TopMost = true;
                                        neuesRect.DrawString(c + "");
                                        neuesRect.Update();
                                    }

                                    //rectanglePool[int_processCounter] = neuesRect;
                                    //Console.WriteLine(" rect nummer : " + int_processCounter + " existiert");
                                }
                            }
                            catch (Exception e)
                            {
                                recognize();
                            }
                        }

                    }
                    c++;
                }
            }
            catch (Exception e)
            {
                recognize();
            }
        }

        private void DrawFirstTwoObjects()
        {
            InfoRectangle rectangle = rectanglePool[int_processCounter - 1];
            rectangle.Visible = true;
            rectangle.TopMost = true;
            rectangle.Update();
            rectangle = rectanglePool[int_processCounter];
            rectangle.Visible = true;
            rectangle.TopMost = true;
            rectangle.Update();

            if (int_processCounter > 1)
            {
                rectangle = rectanglePool[int_processCounter - 2];
                rectangle.Dispose();
            }
        }

        private void hide_repaint()
        {
            try
            {
                if (dropdown == true && (saoObj != null && saoObj.Name != null && saoRef != null && saoRef.Parent != null && saoRef.Parent.Name != null) && (saoRef.Name.Equals(saoObj.Name) || saoRef.Parent.Name.Equals(saoObj.Name)))
                {

                    //Console.WriteLine("---------!!!!!!!!!!!!!!!!!!  saoRef Name : " + saoRef.Name + "\tsaoObj Name : " + saoObj.Name);
                    //Console.WriteLine("Parent Parent---------!!!!!!!!!!!!!!!!!!  saoRef Name : " + saoRef.Parent.Name + "\tsaoObj Name : " + saoObj.Parent.Name);
                    stopWatch.Reset();
                    //Console.WriteLine("----------------------------------------------zähle counter hoch  " + int_processCounter);
                    //!list_selectedProcessSteps.ElementAt(int_processCounter - 1).Program.Equals(list_selectedProcessSteps.ElementAt(int_processCounter).Program) && 
                    if (int_processCounter >= 1 && rectanglePool[1] != null)
                    {
                        if (int_processCounter > 0 && !(list_selectedProcessSteps.ElementAt(int_processCounter - 1).Program.Equals(list_selectedProcessSteps.ElementAt(int_processCounter).Program)))
                        {

                        }
                        else
                        {
                            drawAllRectangles();
                        }
                    }

                }
                else
                {
                    if (!stopWatch.IsRunning)
                    {
                        stopWatch.Start();
                    }

                    //Console.WriteLine("1111111111111111111111111111111---------!!!!!!!!!!!!!!!!!! dropdown ist : " + dropdown);
                    if (stopWatch.ElapsedMilliseconds > 300)
                    {
                        dropdown = false;

                        hideAll();
                        updateAllWindows();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private static void hideAll()
        {
            if (rectanglePool != null && rectanglePool.Length > 0)
            {
                for (int i = 0; i < rectanglePool.Length; i++)
                {
                    if (rectanglePool[i] != null && i != int_newPointer)
                    {
                        rectanglePool[i].Visible = false;
                    }
                }
            }
        }


        private void getChildsofSaoRef(SystemAccessibleObject saoref)
        {
            try
            {
                //Console.WriteLine("ttt222");
                //Console.WriteLine("sao name : " + saoRef.Name);

                allChilds = saoref.Children;
                if (allChilds.Length > 0 && allChilds[0] != null)
                {
                    //Console.WriteLine("sao child name : " + allChilds[0].Name);

                    if (saoref.Name.Equals(allChilds[0].Name))
                    {
                        allChilds = allChilds[0].Children;
                        allChilds = allChilds[0].Children;
                    }
                }
                /*
                if (allChilds != null)
                {
                    Console.WriteLine("ttt333");
                    if (dropdown == false)
                    {
                        //Console.WriteLine("---------!!!!!!!!!!!!!!!!!!");
                        dropdown = true;
                        saoObj = saoRef;
                    }
                }
                 * */
            }
            catch (COMException e)
            {
                Console.WriteLine("Fehler in  getChildsofSaoRef" + e.StackTrace);
                Start();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("Fehler in  getChildsofSaoRef" + e.StackTrace);
                Start();
            }


        }

        private void getChildsOfFirstItem()
        {
            //Console.WriteLine("tt111   " + int_newPointer);
            if (list_selectedProcessSteps.Count() > 0)
            {
                try
                {
                    Console.WriteLine("counter hier : " + int_newPointer);
                    for (int i = 0; i < list_selectedProcessSteps.Count(); i++)
                    {
                        Console.WriteLine("step number : " + i + "   name : " + list_selectedProcessSteps.ElementAt(i));
                    }
                    if (list_selectedProcessSteps.ElementAt(int_newPointer).Name.Equals(saoRef.Name))
                    {
                        //Console.WriteLine("ttt222");
                        //Console.WriteLine("sao name : " + saoRef.Name);

                        allChilds = saoRef.Children;
                        if (allChilds.Length > 0 && allChilds[0] != null)
                        {
                            //Console.WriteLine("sao child name : " + allChilds[0].Name);

                            if (saoRef.Name.Equals(allChilds[0].Name))
                            {
                                allChilds = allChilds[0].Children;
                                allChilds = allChilds[0].Children;
                            }
                        }
                        if (allChilds != null)
                        {
                            Console.WriteLine("ttt333");
                            if (dropdown == false)
                            {
                                //Console.WriteLine("---------!!!!!!!!!!!!!!!!!!");
                                dropdown = true;
                                saoObj = saoRef;
                            }
                        }
                    }
                }
                catch (COMException e)
                {
                    Console.WriteLine(e.StackTrace);
                }

            }
        }

        private void objectInNewWindow()
        {
            getNextStepOnNewProgram(saoRef.Window, list_selectedProcessSteps.ElementAt(int_processCounter).Name);
            //list_windowObjects = getAllFirstLevelObjects(saoRef);
            if (list_selectedProcessSteps.ElementAt(int_processCounter).Name.Equals(saoRef.Name))
            {
                if (rectanglePool[int_processCounter] == null)
                {
                    hideAll();
                    drawOneObjectInNewWindow();
                    int_processCounter++;
                    int_newPointer = int_processCounter;
                    bool_newWindowOpen = true;

                }

            }

        }

        private void drawOneObjectInNewWindow()
        {
            //rectanglePool = new InfoRectangle[list_selectedProcessSteps.Count()];
            //int_processCounter = 0;


            if (rectanglePool[int_processCounter] == null)
            {
                if (saoRef != null)
                {


                    neuesRect = new InfoRectangle(saoRef.Location);
                    neuesRect.Visible = true;
                    neuesRect.TopMost = true;
                    rectanglePool[int_processCounter] = neuesRect;
                    Console.WriteLine(" rect nummer : " + int_processCounter + " existiert und hat den namen _:" + saoRef.Name + "\t mit der location :" + saoRef.Location);
                    if (int_processCounter > 0)
                    {
                        InfoRectangle rectangle = rectanglePool[int_processCounter];
                        rectangle.Visible = true;
                        rectangle.TopMost = true;
                        rectangle.Update();
                        //drawAllRectangles();
                        //DrawTwoObjects();
                    }
                }
                else
                {
                    Console.WriteLine("konnte nicht zeichnen saoref null, counter : " + int_processCounter);
                }
            }
            else
            {
                Console.WriteLine("pool an stelle " + int_processCounter + " besetzt");
            }
        }

        private void DrawTwoObjects()
        {
            InfoRectangle rectangle = rectanglePool[int_processCounter - 1];
            rectangle.Visible = true;
            rectangle.TopMost = true;
            rectangle.Update();
            rectangle = rectanglePool[int_processCounter];
            rectangle.Visible = true;
            rectangle.TopMost = true;
            rectangle.Update();
            if (int_processCounter > 1)
            {
                rectangle = rectanglePool[int_processCounter - 2];
                rectangle.Dispose();
            }
        }

        private void drawOneObject()
        {
            rectanglePool = new InfoRectangle[list_selectedProcessSteps.Count()];
            int_processCounter = 0;


            if (rectanglePool[int_processCounter] == null)
            {
                if (saoRef != null)
                {

                    try
                    {
                        neuesRect = new InfoRectangle(saoRef.Location);
                        neuesRect.Visible = true;
                        neuesRect.TopMost = true;
                        rectanglePool[int_processCounter] = neuesRect;
                        Console.WriteLine(" rect nummer : " + int_processCounter + " existiert");
                        if (int_processCounter > 0)
                        {
                            InfoRectangle rectangle = rectanglePool[int_processCounter];
                            rectangle.Visible = true;
                            rectangle.TopMost = true;
                            rectangle.Update();
                            //drawAllRectangles();
                            //DrawTwoObjects();
                        }
                    }
                    catch (COMException e)
                    {
                        Console.WriteLine(e.StackTrace);
                    }
                }
                else
                {
                    Console.WriteLine("konnte nicht zeichnen saoref null, counter : " + int_processCounter);
                }
            }
            else
            {
                Console.WriteLine("pool an stelle " + int_processCounter + " besetzt");
            }
        }

        private void objectInNewProgram()
        {
            openNewProgram(null);
        }

        private void getChilds()
        {
            throw new NotImplementedException();
        }

        private void getPointFromMouseposition()
        {

            SystemAccessibleObject accessibleObject = null;
            try
            {
                accessibleObject = SystemAccessibleObject.FromPoint((int)point.X, (int)point.Y);
            }
            catch (Exception)
            {
                Console.WriteLine("Fehler");
            }

            saoRef = accessibleObject;

            string Name = tech.GetNameByPosition(point);

            //TODO
            //try catch bei location -0.5 abfangen

            if (Name.Equals("no name"))
            {
                try
                {
                    point = new System.Windows.Point(accessibleObject.Location.X, accessibleObject.Location.Y - 0.5);
                    Name = tech.GetNameByPosition(point);
                }
                catch (Exception e)
                {

                }
            }


            //var tecIdsAcc = tech.GetIdsByPosition(point, null);
        }



        private void openNewProgram(ProcessStep processStep)
        {
            if (bool_newProgramIsOpen == false)
            {
                string processName = processStep.Program;


                processes = System.Diagnostics.Process.GetProcessesByName(processName);
                System.Diagnostics.Process current = System.Diagnostics.Process.GetCurrentProcess();
                System.Diagnostics.Process proc = null;

                // if process already started, get the window
                if (processes.Length > 0)
                {
                    proc = processes[0];

                    // check if active window is the needed process' window, switch to if not
                    if (current.Id != proc.Id)
                    {
                        SwitchToThisWindow(proc.MainWindowHandle, true);
                        bool_newProgramIsOpen = true;
                    }
                }
                // else start needed program
                else
                {
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.FileName = processName;
                    try
                    {
                        proc = System.Diagnostics.Process.Start(startInfo);
                    }
                    catch (Exception e)
                    {
                    }
                    Console.WriteLine(processName + " wurde gestartet!");
                    Thread.Sleep(1000);
                    bool_newProgramIsOpen = true;

                }

                // SystemWindow window = new SystemWindow(GetForegroundWindow());
                if (proc != null)
                {
                    SystemWindow window2 = new SystemWindow(proc.MainWindowHandle);
                    //hier rekursiv alle Objekte holen
                    try
                    {
                        SystemAccessibleObject sao = SystemAccessibleObject.FromWindow(window2, AccessibleObjectID.OBJID_WINDOW);
                    }
                    catch (COMException e)
                    {
                        Console.WriteLine("Fehler in openNewProgram" + e.StackTrace);
                    }
                    //List<SystemAccessibleObject> allobj = getAllFirstLevelObjects(sao);

                    Console.WriteLine("window ist " + window2.Title + "\tProcessStep ist : " + processStep.Name);

                    //sets saoRef on new object
                    getNextStepOnNewProgramWithProcess(window2, processStep.Name);
                    if (saoRef != null)
                    {
                        //Console.WriteLine("habe neues programm geöffnet und das object ist: Name: " + saoRef.Name + " -- location: " + saoRef.Location);
                    }
                }
            }
        }

        private void getNextStepOnNewProgram(SystemWindow window, string processStepName)
        {
            // SystemWindow window = sao.Window;
            //Console.WriteLine("windooooow : " + window.Title);
            //SystemAccessibleObject sao_obj = getSaoFromHighestWindow(window);

            //Console.WriteLine("Window :" + window + "\t saoobj :" + saoObj + "\t stepname :" + processStepName);

            if (saoRef != null)
            {
                oneChild(saoRef, processStepName);
            }
        }

        private void getNextStepOnNewProgramWithProcess(SystemWindow window, string processStepName)
        {
            // SystemWindow window = sao.Window;
            //Console.WriteLine("windooooow : " + window.Title);
            SystemAccessibleObject sao_obj = getSaoFromHighestWindow(window);

            //Console.WriteLine("Window :" + window + "\t saoobj :" + sao_obj + "\t stepname :" + processStepName);

            if (sao_obj != null)
            {
                oneChild(sao_obj, processStepName);
            }
        }

        private static void getNextStepOnNewProgramWithProcess2(SystemWindow window)
        {
            // SystemWindow window = sao.Window;
            Console.WriteLine("windooooow : " + window.Title);
            SystemAccessibleObject sao_obj = getSaoFromHighestWindow(window);

            //Console.WriteLine("Window :" + window + "\t saoobj :" + sao_obj + "\t stepname :" + processStepName);

            if (sao_obj != null)
            {
                Console.WriteLine("obj für oneChild2 : " + sao_obj);
                oneChild2(sao_obj);
            }
        }

        private static void oneChild2(SystemAccessibleObject sao_obj)
        {
            //Console.WriteLine("bin in oneChild2");
            SystemAccessibleObject[] childs = sao_obj.Children;
            if (childs != null)
            {
                foreach (SystemAccessibleObject child in childs)
                {
                    // SystemAccessibleObject[] saoref = child.Children;

                    foreach (ProcessStep step in list_firstLevelSteps)
                    {
                        // Console.WriteLine("schleife : " + step.Name);
                        // Console.WriteLine("child : " + child.Name + "\t stepname : " + step.Name);
                        if (child != null && child.Name != null && child.Name.Equals(step.Name) && step.Parent.Equals(child.Parent.Name))
                        {
                            Console.WriteLine("oneChild2-----------gefunden: " + child.Name + "   " + child.Location);
                            /*
                            InfoRectangle rectangle = new InfoRectangle(child.Location);
                            rectangle.Visible = true;
                            rectangle.TopMost = true;
                            rectangle.Update();
                            rectangle.setName(child.Name);
                            if (!list_drawableRectanglesFirstLevel.Contains(rectangle))
                            {
                                list_drawableRectanglesFirstLevel.Add(rectangle);
                            }
                             * */
                            list_firstLevelObjects.Add(child);
                            //list_firstLevelSteps.Remove(step);
                            int_amountFirstLevelObjects++;
                        }
                    }
                    if (list_firstLevelSteps.Count == int_amountFirstLevelObjects)
                    {
                        break;
                    }
                    oneChild2(child);
                }
            }

        }

        private static void hideAllFirstLevel()
        {   
            foreach (InfoRectangle rec in list_drawableRectanglesFirstLevel)
            {
                Console.WriteLine("first level löschen!!!! nummer : " + rec.getNumber());
                rec.Dispose();
                rec.Update();
            }
            list_drawableRectanglesFirstLevel.Clear();
            list_firstLevelObjects.Clear();
        }
        private static void drawAllFirstLevel()
        {
            Console.WriteLine("drawAllFirstLevel");

            InfoRectangle rectangle = null;
            int c = 1;
            foreach (ProcessStep step in list_selectedProcessSteps)
            { 
                /*
                foreach (InfoRectangle rec in list_drawableRectanglesFirstLevel)
                {
                    if (step.Name.Equals(rec.Name))
                    {
                        string zahl = c + "";
                        rec.Visible = true;
                        rec.TopMost = true;
                        rec.DrawString(zahl);
                        rec.Update();
                    }
                }
                c++;
                 * */
                foreach (SystemAccessibleObject sao in list_firstLevelObjects)
                {
                    if (step.Name.Equals(sao.Name) && sao.Parent.Name.Equals(step.Parent))
                    {
                        string zahl = c + "";
                        rectangle = new InfoRectangle(sao.Location);

                        rectangle.Visible = true;
                        rectangle.TopMost = true;
                        rectangle.DrawString(zahl);
                        rectangle.setNumber(c);
                        rectangle.Update();
                        if (!list_drawableRectanglesFirstLevel.Contains(rectangle))
                        {
                            list_drawableRectanglesFirstLevel.Add(rectangle);
                        }
                        Console.WriteLine("omg");

                    }
                }
                c++;
               
            }

            foreach (SystemAccessibleObject sao in list_firstLevelObjects)
            {
                Console.WriteLine("first level objects : " + sao.Name);
            }
        }

        private void drawAllRectangles()
        {
            InfoRectangle rectangle = rectanglePool[int_processCounter - 1];
            rectangle.Visible = true;
            rectangle.TopMost = true;
            rectangle.Update();
            rectangle = rectanglePool[int_processCounter];
            rectangle.Visible = true;
            rectangle.TopMost = true;
            rectangle.Update();
            if (int_processCounter > 1)
            {
                rectangle = rectanglePool[int_processCounter - 2];
                rectangle.Dispose();
            }
        }

      
        private void getAllProcessSteps()
        {


            //List for all ProcessStep names
            //Dictionary<string, string> allSteps = new Dictionary<string, string>();

            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++");

            if (iList_allProjectsFromDB != null)
            {
                foreach (Database.Domain.Project allProject in iList_allProjectsFromDB)
                {
                    foreach (Database.Domain.Process process in allProject.Processes)
                    {
                        list_allProcesses.Add(process);
                        IList<ProcessStep> allProcessSteps = process.ProcessSteps;

                        if (allProcessSteps.Count > 0 && process != null)
                        {
                            map_ProcessNameProcessStep.Add(process.Name, allProcessSteps.ElementAt(0).Name);
                            if (allProcessSteps.ElementAt(0) != null)
                            {
                                list_allProcessStepsfromDB.Add(allProcessSteps.ElementAt(0));
                                //allProcessSteps.Add(allProcessSteps.ElementAt(0));
                            }
                        }
                    }
                }
            }
        }

        private static void children(SystemAccessibleObject saoRef, SystemAccessibleObject parentObject)
        {
            SystemAccessibleObject[] childs = saoRef.Children;
            SystemAccessibleObject[] childsOfParent = parentObject.Children;

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
            Console.WriteLine("-----------------------------------");

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

        public static int XKoordinate
        {
            set;
            get;
        }

        //Get-Method for the y-coodinate
        public static int YKoordinate
        {
            set;
            get;
        }

        private void dbCompareWithSelectedObj(SystemAccessibleObject sao)
        {
            //Console.WriteLine("dbCompareWithSelectedObj start sao : " + sao.Name);
            //Console.WriteLine("dbCompareWithSelectedObj start Program Name : " + string_processName);
            //alle Kinder ausgeben
            SystemAccessibleObject p = null;
            SystemAccessibleObject[] kinder = null;

            try
            {
                p = sao.Parent;
                kinder = sao.Children;
            }
            catch (Exception e)
            {

            }

            if (bool_newProgramIsOpen)
            {
                foreach (ProcessStep step in list_selectedProcessSteps)
                {
                    if (sao.Name != null && sao.Name.Equals(step.Name) && step.Program.Equals(string_processName))
                    {
                        saoRef = sao;
                        bool_notSelected = false;

                    }
                }
            }

            bool_found = false;
            bool_neuerName = false;

            if (!bool_newProgramIsOpen && rectanglePool != null)
            {
                Console.WriteLine("dispose all recs");
                foreach (InfoRectangle rec in rectanglePool)
                {
                    if (rec != null)
                    {
                        rec.Dispose();
                        rec.Update();
                    }
                }
                rectanglePool = null;
            }

            try
            {
                if (sao.Name != null && !string_selectedName.Equals(sao.Name))
                {
                    bool_neuerName = true;
                }
            }
            catch (ArgumentException)
            {
                // Show the user that 7 cannot be divided by 2.
                Console.WriteLine("Wer liegt ausserhalb des Breiches");
            }

            catch (NullReferenceException e)
            {
                Console.WriteLine(e.StackTrace);
            }

            recognizeFirstProcess(sao);


            if (bool_found)
            {
                if (rectangle == null)
                {

                    rectangle = new InfoRectangle(sao.Location);
                    rectangle.TopMost = true;
                    rectangle.Visible = true;

                }
                else
                {
                    if (bool_neuerName)
                    {

                        rectangle.Dispose();
                        rectangle = new InfoRectangle(sao.Location);
                        rectangle.Visible = true;
                        rectangle.TopMost = true;
                        //blacklist.Clear();
                        int_processCounter = int_newPointer;
                    }
                }
            }
        }

        public static void getAllFirstLevelObjects(SystemAccessibleObject sao_obj)
        {
            //Alle Steps durchlaufen
            ProcessStep default_Step = list_selectedProcessSteps.ElementAt(0);
            Console.WriteLine("deafultStep : " + default_Step.Name);
            string window = default_Step.Window;
            string program = default_Step.Program;

            list_firstLevelSteps.Add(default_Step);
            for (int i = 1; i < list_selectedProcessSteps.Count(); i++)
            {
                //
                ProcessStep step = list_selectedProcessSteps.ElementAt(i);
                if (!(default_Step.Name.Equals(step.Parent)) && (step.Program.Equals(program) && step.Window.Equals(window)))
                {
                    default_Step = list_selectedProcessSteps.ElementAt(i);
                    list_firstLevelSteps.Add(default_Step);
                    //Console.WriteLine("neuer Step : " + default_Step.Name);
                    //statische Liste füllen
                }
                //zwei statische Listen -> rekursive Suche -> wenn ein element gefunden 
                //dann in die eine liste schreiben und aus der anderen löschen

            }


            System.Diagnostics.Process[] procs = System.Diagnostics.Process.GetProcessesByName(default_Step.Program);
            //processes = System.Diagnostics.Process.GetProcessesByName(processName);
            //System.Diagnostics.Process current = System.Diagnostics.Process.GetCurrentProcess();
            System.Diagnostics.Process proc = null;


            // if process already started, get the window
            if (procs.Length > 0)
            {
                proc = procs[0];
            }
            //Console.WriteLine("current pro :" + current.ProcessName);
            //Console.WriteLine("erstellter pro :" + proc.ProcessName);

            //rectanglePool[0].Dispose();

            SystemWindow window2 = new SystemWindow(proc.MainWindowHandle);



            //getNextStepOnNewProgram(window2, actuallStep.Name);
            getNextStepOnNewProgramWithProcess2(window2);

            Console.WriteLine("zeichnen");
            drawAllFirstLevel();
        }

        public static void getAllFirstLevelObjects(SystemAccessibleObject sao_obj, string actualProgram)
        {
            Console.WriteLine("aktuelles programm : " + actualProgram);
            ProcessStep default_Step = null;
            //Console.WriteLine("deafultStep : " + default_Step.Name);
            string_window = "";
            string program = "";
            //finde erstes Element mit dem selben programm wie actualProgram
            int c = 0;
            foreach (ProcessStep step in list_selectedProcessSteps)
            {
                Console.WriteLine("step programm : " + step.Program);
                if (step.Program.Equals(actualProgram))
                {
                    default_Step = step;
                    string_window = default_Step.Window;
                    program = default_Step.Program;
                    break;
                }
                c++;

            }

            try
            {
                list_firstLevelSteps.Add(default_Step);
                for (int i = c; i < list_selectedProcessSteps.Count(); i++)
                {
                    //
                    ProcessStep step = list_selectedProcessSteps.ElementAt(i);
                    if (!(default_Step.Name.Equals(step.Parent)) && (step.Program.Equals(program) && step.Window.Equals(string_window)))
                    {
                        default_Step = list_selectedProcessSteps.ElementAt(i);
                        list_firstLevelSteps.Add(default_Step);
                        //Console.WriteLine("neuer Step : " + default_Step.Name);
                        //statische Liste füllen
                    }
                    //zwei statische Listen -> rekursive Suche -> wenn ein element gefunden 
                    //dann in die eine liste schreiben und aus der anderen löschen

                }
            }
            catch (Exception e)
            { 
                Console.WriteLine("Fehler in getAllFirstLevelObjects " + e.StackTrace);
            }

            System.Diagnostics.Process[] procs = System.Diagnostics.Process.GetProcessesByName(default_Step.Program);
            //processes = System.Diagnostics.Process.GetProcessesByName(processName);
            //System.Diagnostics.Process current = System.Diagnostics.Process.GetCurrentProcess();
            System.Diagnostics.Process proc = null;


            // if process already started, get the window
            if (procs.Length > 0)
            {
                proc = procs[0];
            }
            //Console.WriteLine("current pro :" + current.ProcessName);
            //Console.WriteLine("erstellter pro :" + proc.ProcessName);

            //rectanglePool[0].Dispose();

            SystemWindow window2 = new SystemWindow(proc.MainWindowHandle);



            //getNextStepOnNewProgram(window2, actuallStep.Name);
            getNextStepOnNewProgramWithProcess2(window2);

            Console.WriteLine("zeichnen hoha");
            drawAllFirstLevel();
        }

        private static SystemAccessibleObject getSaoFromHighestWindow(SystemWindow window)
        {
            //höchstes parentwindow holen
            /*
            SystemWindow win = window;
            while (!win.Title.Equals(win.Parent.Title))
            {
                win = win.Parent;
            }

            //rekursiv von diesem Fenster alle "Firstlevel" Objekte holen
            //bei Programmen wie NP++ kann es auch sein das es alle Objekte holt ( aufwendig)
            */
            SystemAccessibleObject sao = null;
            //Console.WriteLine("window : " + win.Title);
            try
            {
                sao = SystemAccessibleObject.FromWindow(window, AccessibleObjectID.OBJID_WINDOW);
                Console.WriteLine("getSaoFromHighestWindow sao : " + sao.Name);
            }
            catch (COMException e)
            {

            }


            return sao;
        }

        public static List<SystemAccessibleObject> allreallyAllChilds(SystemAccessibleObject sao)
        {

            SystemAccessibleObject[] childs = sao.Children;
            List<SystemAccessibleObject> allObject = new List<SystemAccessibleObject>();

            foreach (SystemAccessibleObject child in childs)
            {
                // SystemAccessibleObject[] saoref = child.Children;
                allreallyAllChilds(child);

                if (!(list_AllObjects.Contains(child)))
                {
                    allObject.Add(child);
                    //list_AllObjects.Add(child);
                    //namen.Add(child.Name);
                }
            }
            return allObject;
        }

        public void oneChild(SystemAccessibleObject firstLevelSao, string processStepName)
        {

            SystemAccessibleObject[] childs = firstLevelSao.Children;
            if (childs != null)
            {
                foreach (SystemAccessibleObject child in childs)
                {
                    // SystemAccessibleObject[] saoref = child.Children;


                    if (child != null && child.Name != null && child.Name.Equals(processStepName))
                    {
                        Console.WriteLine(" oneChild(SystemAccessibleObject firstLevelSao, string processStepName)-----------gefunden: " + child.Name + "   " + child.Location);
                        saoRef = child;
                        saoObj = saoRef;
                        break;

                        //list_AllObjects.Add(child);
                        //namen.Add(child.Name);
                    }

                    oneChild(child, processStepName);
                }
            }
        }

        private void recognizeFirstProcess(SystemAccessibleObject sao)
        {
            foreach (ProcessStep step in list_allProcessStepsfromDB)
            {

                processStepNames = new List<string>();
                //mit try abfangen
                //TODO!!!!!!!!!!!!!

                //Console.WriteLine("Process Name : " + step.Name + "    ---ProcessStep Programm Name : " + step.Program + "----   Aktuelles Fentser : " + string_processName);


                try
                {
                    if (sao.Parent.Name != null && sao.Parent.Name.Equals(step.Parent) && sao.Name != null && sao.Name.Equals(step.Name) && step.Program.Equals(string_processName))
                    {
                        saoRef = sao;
                        bool_found = true;
                        //TODO
                        //Timer einbauen 1 sec warten erst dann Erkennung starten
                        //Timer if -> wenn sich name ändert dann timer auf 0 setzen und pridate auf null setzen

                        processStepNames.Add(sao.Name);

                        if (bool_firstTime == false)
                        {
                            bool_firstTime = true;

                        }

                        foreach (string key in map_ProcessNameProcessStep.Keys)
                        {
                            string value = "";
                            bool test = map_ProcessNameProcessStep.TryGetValue(key, out value);
                            //Console.WriteLine("DB Name = " + dbnames + "  aus Map = " + value);
                            if (step.Name.Equals(value))
                            {
                                Console.WriteLine("Process zu " + step.Name + " ist " + key);
                                if (!key.Equals("") && key != null)
                                {
                                    //BLBLBLBLBBLLB
                                    BalloonTipProject(key);
                                    processStepNames.Add(key);
                                }
                            }
                        }
                        /*
                       if (processStepNames.Count() > 1)
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
                               if (process.Name.Equals(processStepNames.ElementAt(0)))
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
                       */
                        string_selectedName = sao.Name;

                        if (_event != null)
                        {
                            _event(processStepNames, new EventArgs());
                        }
                    }
                }
                catch (COMException ce)
                {
                    Console.WriteLine("catch COMException ");
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
        }


        internal static void selectedProcess(Process pro)
        {
            list_selectedProcessSteps = pro.ProcessSteps;
            foreach (ProcessStep step in list_selectedProcessSteps)
            {
                Console.WriteLine("step : " + step.Name);
            }
        }
    }
}
