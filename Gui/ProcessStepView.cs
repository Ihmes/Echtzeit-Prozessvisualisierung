using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using ManagedWinapi.Accessibility;
using System.Runtime.InteropServices;
using ManagedWinapi.Windows;
using Database.Repository;
using Database.Domain;
using System.IO;

namespace Gui
{
     
       
    public partial class ProcessstepView : Form
    { 
        private Thread myThread;
        private delegate void SenderHandler(object sender, EventArgs e);
        string name = "";
        int index = 1;
        //===============================================System Tray start=======================================================
        //WICHTIG Das Icon muss muss unter LiveContext.Utility/Resources/tray.ico liegen!!
        ContextMenu cm;
        MenuItem miCurr;
        int temp;
        int iIndex = 0;
        private static NotifyIcon notico;
        //================================================System Tray ende======================================================

        public ProcessstepView()
        {
            InitializeComponent();
            temp = 1; 
            ThreadStart myThreadDelegate = new ThreadStart(new Visualizer(this).recognize);
            myThread = new Thread(myThreadDelegate);
            myThread.Start();
            //visualizer = new Visualizer();
           //_event += new SenderHandler(mainFrame.ReadNewProjectFormularData);
            //====================================NotifyIcon and Kontextmenü Systemtray start=======================================================
            // Kontextmenü erzeugen
            cm = new ContextMenu();


            //Kontextmenüeinträge erzeugen
            miCurr = new MenuItem();
            miCurr.Index = iIndex++;
            miCurr.Text = "&Öffnen";
            miCurr.Click += new System.EventHandler(notico_Click_1);
            cm.MenuItems.Add(miCurr);


            // Kontextmenüeinträge erzeugen
            miCurr = new MenuItem();
            miCurr.Index = iIndex++;
            miCurr.Text = "&Hilfe";
            miCurr.Click += new System.EventHandler(Action1Click);
            cm.MenuItems.Add(miCurr);


            // Kontextmenüeinträge erzeugen
            miCurr = new MenuItem();
            miCurr.Index = iIndex++;
            miCurr.Text = "&Beenden";
            miCurr.Click += new System.EventHandler(ExitClick);
            cm.MenuItems.Add(miCurr);

            // NotifyIcon erzeugen
            notico = new NotifyIcon();
            notico.Icon = new Icon(Path.Combine(Application.StartupPath, @"../../../LiveContext.Utility/Resources/tray.ico")); // Eigenes Icon einsetzen
            notico.Text = "Prozessvisualisierung";
            notico.DoubleClick += new EventHandler(notico_Click_1);

            //====================================NotifyIcon and Kontextmenü Systemtray ende=======================================================
            notico.Visible = true;
        }

        //===============================Menu Aktionen von Notify  Start======================================================================
        private static void Action1Click(Object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.sennty.de/Prozessvisualisierung/Hilfe.html");
        }

        private static void ExitClick(Object sender, EventArgs e)
        {
            notico.Dispose();
            Application.Exit();
        }

        private void notico_Click_1(object sender, System.EventArgs e)
        {
            this.ShowInTaskbar = true;
            notico.Visible = false;
            this.WindowState = FormWindowState.Normal;
        }

        private void Anzeige_Resize(object sender, System.EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                notico.Visible = true;
                notico.ContextMenu = cm;
                this.ShowInTaskbar = false;
                this.WindowState = FormWindowState.Minimized;
            }
        }
        //===============================Menu Aktionen von Notify  ende======================================================================

        public void showBalloonTip(String Title, String Text)
        {
            notico.Dispose();
            notico = new NotifyIcon();
            notico.BalloonTipText = Text;
            notico.BalloonTipTitle = Title;
            notico.BalloonTipIcon = ToolTipIcon.Info;
        
            try
            {
                notico.ShowBalloonTip(3);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("ArgumentException durch ShowBalloonTip");
            }
        }

        public void Actions(object sender, EventArgs e)
        {
            int counter = 0;
            
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new SenderHandler(Actions), new object[] { sender, e });
                return;
            }
            List<string> allProcesses = (List<string>)sender;

            /*
            foreach (string pr in allProcesses)
            {
                Console.WriteLine("-----------------!!!!!!!!-----------" + pr);
            }
            */
            if (allProcesses != null && allProcesses.Count > 0)
            { 
                if (name.Equals(allProcesses.ElementAt(0)))
                {
                    if (!(listbox_choose.Items.Count > 0))
                    {
                        foreach (string process in allProcesses)
                        {
                            if (counter > 0)
                            {
                                listbox_choose.Items.Add(process);
                            }
                            counter++;
                        }
                    }
                }
                else
                {
                    name = allProcesses.ElementAt(0);
                    listbox_choose.Items.Clear();
                }
            }
            
        }



        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            //shows mainframe again
            new MainFrame().Visible = true;
            this.Hide();
        }

        private void visualizer_stop_Click(object sender, EventArgs e)
        {
            Visualizer.bool_notSelected = true;
           // label2.Text = "";
            listbox_processteps.Items.Clear();
            
            
            listbox_choose.Items.Clear();
            //Visualizer.rectanglePool.Clear();
            
            Visualizer.int_processCounter = 1;
            Visualizer.int_newPointer = 1;
        }

       

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //listBox1.SelectedIndex = listBox1.FindString("Two");
        }

        private void visualizer_select_Click(object sender, EventArgs e)
        {
            listbox_processteps.Items.Clear();
            
            if (listbox_choose.SelectedItem != null)
            {
                string name = listbox_choose.SelectedItem.ToString();
                Console.WriteLine("hallo fuerMarkus   : " + name);
                Process pro = null;
                foreach (Database.Domain.Process process in Visualizer.list_allProcesses)
                {
                    if (process.Name.Equals(name))
                    {
                        Visualizer.julia(process);
                        pro = process;
                        break;
                    }
                }

                Visualizer.rectanglePool = new InfoRectangle[pro.ProcessSteps.Count()];
                Visualizer.rectanglePool[0] = Visualizer.rectangle;

                if (pro != null)
                {
                    
                    //listbox_processteps.Items.Add(pro.Name);
                    int c = 1;
                    foreach (ProcessStep step in pro.ProcessSteps)
                    {
                        
                        listbox_processteps.Items.Add( "Step: " + c + ". " + step.Name + "\n"); 
                        Console.WriteLine("---------------------Program   : "+step.Program);
                        c++;
                    }
                    listbox_processteps.SetSelected((temp - 1), true);
                    
                }

                Visualizer.bool_notSelected = false;
                //Visualizer.selectedProcess = pro;
               
                Visualizer.selectedProcess(pro);
                //Console.WriteLine("wiesoooo");
                //Visualizer.getAllFirstLevelObjects(Visualizer.saoRef);
            }
            else
            {
                //label2.Text = "Bitte ein Prozessschritt auswählen";
                listbox_processteps.Items.Clear();
                listbox_processteps.Items.Add("Bitte ein Prozessschritt auswählen");
            }
        }


        public void highlight_current_step(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new SenderHandler(highlight_current_step), new object[] { sender, e });
                return;
            }
                   //listbox_processteps.BeginUpdate();
            int index = (int)sender;
                   if (index <= 0)
                       Console.WriteLine("Item is not available INDEX:" + index);
                   else
                   {
                       try
                       {
                           //this.listbox_processteps.ClearSelected();
                           Console.WriteLine("------------------------------- Selected auswahl------------------------------" + index);
                           listbox_processteps.SetSelected((index - 1), true);
                           this.Update();
                       }
                       catch(ArgumentOutOfRangeException ea) {
                           Console.WriteLine("ArgumentOutOfRangeException (bei listbox SetSelcted) Index: " + index);
                       }
                        
                   }
                   //this.Refresh();
    

                    //listbox_processteps.EndUpdate();
                    //Anzeige.ActiveForm.ShowDialog();
                    //Application.DoEvents();
                

        }

        private void Anzeige_Load(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.sennty.de/Prozessvisualisierung/Hilfe.html");
        }

     
      


    }
}
