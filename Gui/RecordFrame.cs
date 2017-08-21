using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Database.Domain;
using System.Collections;
using Database.Repository;

namespace Gui
{
    public partial class RecordFrame : Form
    {
        private delegate void SenderHandler(object sender, EventArgs e);
        private event SenderHandler _event;
        private Thread myThread;
        private ProcessRepository ProcessRepo;
        private IList<ProcessStep> ProcessStepList;
        private string ElementName;
        private string Type;
        private string Program;
        private string Window;
        private string Parent;


        public RecordFrame(DesignerMainFrame mainFrame)
        {
            InitializeComponent();
            ProcessRepo = new ProcessRepository();
            FillListBoxProcessStep();
            _event += new SenderHandler(mainFrame.RecordMainFrameEvent);
            ThreadStart myThreadDelegate = new ThreadStart(new Recorder(this).StartMousePointer);
            myThread = new Thread(myThreadDelegate);
            myThread.Start();
        }

        private void FillListBoxProcessStep()
        {
            ProcessStepList = SelectedObjects.Process.ProcessSteps;
            this.listBoxProcessSteps.Items.Clear();
            foreach (ProcessStep p in ProcessStepList)
            {
                this.listBoxProcessSteps.Items.Add(p);
            }
        }

        private void buttonEndRecord_Click_1(object sender, EventArgs e)
        {
            myThread.Abort();
            SelectedObjects.Process.ProcessSteps = ProcessStepList;
            ProcessRepo.Update(SelectedObjects.Process);
            this.Dispose();
            if (_event != null)
            {
                _event(this, new EventArgs());
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (CloseReason.UserClosing == e.CloseReason)
            {
                myThread.Abort();
                SelectedObjects.Process.ProcessSteps = ProcessStepList;
                ProcessRepo.Update(SelectedObjects.Process);
                this.Dispose();
                if (_event != null)
                {
                    _event(this, new EventArgs());
                }
            }
        }

        public void MakeRecorderActions(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new SenderHandler(MakeRecorderActions), new object[] { sender, e });
                return;
            }
            IDictionary Dictionary = (IDictionary)sender;
            this.labelInformation.Text = (string)Dictionary[Recorder.INFO] + "\n" + (string)Dictionary["0"] + "\n" + (string)Dictionary["1"] + "\n" + (string)Dictionary["2"];
            if ("true".Equals(Dictionary[Recorder.STRG]))
            {
                ElementName = (string)Dictionary[Recorder.NAME];
                Type = (string)Dictionary[Recorder.TYPE];
                Program = (string)Dictionary[Recorder.PROGRAMM];
                Window = (string)Dictionary[Recorder.WINDOW];
                Parent = (string)Dictionary[Recorder.PARENT];
                this.textBoxName.Text = ElementName;

                Dictionary[Recorder.STRG] = "false";
            }
        }

        private void RecordFrame_Load(object sender, EventArgs e)
        {

        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (!("".Equals(this.textBoxName.Text)))
            {
                this.add_step.Enabled = true;
            }
            else
            {
                this.add_step.Enabled = false;
            }
        }

        private void add_step_Click(object sender, EventArgs e)
        {
            if ((ElementName != null && Type != null && Program != null && Window != null))
            {
                string Name = this.textBoxName.Text;
                string Description = this.textBoxDescription.Text;
                int Step = ProcessStepList.Count + 1;
                ProcessStep ProcessStep = new ProcessStep(Name, Description, Step, ElementName, Type, Program, Window, Parent);
                ProcessStepList.Add(ProcessStep);
                FillListBoxProcessStep();
                this.textBoxName.Text = "";
                this.textBoxDescription.Text = "";
                ElementName = null;
                Type = null;
                Program = null;
                Window = null;
            }
            else
            {
                MessageBox.Show("Bitte wählen Sie das GUI-Element mit \"Mouse-Hover + STRG\" aus. Sie können dann den Namen ändern!!!");
                this.textBoxName.Text = "";
            }

        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            ProcessStep ProcessStepFromList = ProcessStepList.ElementAt(((ProcessStep)this.listBoxProcessSteps.SelectedItem).Step - 1);
            if (ProcessStepFromList.Step == 1)
            {
                MessageBox.Show(ProcessStepFromList.Name + " kann nicht nach oben verschoben werden");
            }
            else
            {
                ProcessStep PreviousProcessStepFromList = ProcessStepList.ElementAt(ProcessStepFromList.Step - 2);
                int temp = ProcessStepFromList.Step;
                ProcessStepFromList.Step = PreviousProcessStepFromList.Step;
                PreviousProcessStepFromList.Step = temp;
                SelectedObjects.Process.ProcessSteps = ProcessStepList;
                FillListBoxProcessStep();
                this.listBoxProcessSteps.SelectedIndex = ProcessStepFromList.Step - 1;
            }
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            ProcessStep ProcessStepFromList = (ProcessStep)this.listBoxProcessSteps.SelectedItem;
            if (ProcessStepFromList.Step == ProcessStepList.Count)
            {
                MessageBox.Show(ProcessStepFromList.Name + " kann nicht nach unten verschoben werden");
            }
            else
            {
                ProcessStep NextProcessStepFromList = ProcessStepList.ElementAt(ProcessStepFromList.Step);
                int temp = ProcessStepFromList.Step;
                ProcessStepFromList.Step = NextProcessStepFromList.Step;
                NextProcessStepFromList.Step = temp;
                SelectedObjects.Process.ProcessSteps = ProcessStepList;
                FillListBoxProcessStep();
                this.listBoxProcessSteps.SelectedIndex = ProcessStepFromList.Step - 1;
            }

        }

        private void delete_step_Click(object sender, EventArgs e)
        {


            myThread.Abort();
            ProcessStep ProcessStepFromList = (ProcessStep)this.listBoxProcessSteps.SelectedItem;
            int deletedStep = (Math.Abs(ProcessStepFromList.Step));
            //Überprüfe ob es der letzte oder einzige Step ist

            Console.WriteLine("ProcessStepListCount " + ProcessStepList.Count());
            Console.WriteLine("DELTED STEP " + deletedStep);
            if (ProcessStepList.Count().Equals((deletedStep)))
            {

                Console.WriteLine("++++++++++++++++++++++++++ LETZER SCHRITT");
                //---- wait cursor
                this.Cursor = Cursors.WaitCursor;
                //----

                this.delete_step.Enabled = false;
                //ProcessStep ProcessStepFromList = (ProcessStep)this.listBoxProcessSteps.SelectedItem;

                Database.Repository.ProcessStepRepository processStepRepo = new Database.Repository.ProcessStepRepository();

                try
                {
                    //deletedStep = (Math.Abs(ProcessStepFromList.Step));

                    processStepRepo.Delete(ProcessStepList.ElementAt(deletedStep - 1));
                    ProcessStepList.RemoveAt(deletedStep - 1);


                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine("Keine Element wurde ausgewählt");
                }

                FillListBoxProcessStep();
                ProcessStepList.ToList().ForEach(p =>Console.WriteLine(p.Step));

                //---- default cursor
                this.Cursor = Cursors.Default;
                //----
                ThreadStart myThreadDelegate = new ThreadStart(new Recorder(this).StartMousePointer);
                myThread = new Thread(myThreadDelegate);
                myThread.Start();
            }


            else
            {
                Console.WriteLine("+++++++++++++++++++++ Schritte noch vorhanden");
                DialogResult result = MessageBox.Show("Do you want to delete all following process steps?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


                if (result == DialogResult.Yes)
                {
                    Console.WriteLine("********************** YES");
                    //---- wait cursor
                    this.Cursor = Cursors.WaitCursor;
                    //----

                    this.delete_step.Enabled = false;

                    // ProcessStep ProcessStepFromList = (ProcessStep)this.listBoxProcessSteps.SelectedItem;

                    Database.Repository.ProcessStepRepository processStepRepo = new Database.Repository.ProcessStepRepository();

                    try
                    {
                        deletedStep = (Math.Abs(ProcessStepFromList.Step));
                        while (ProcessStepList.Count >= deletedStep)
                        {
                            processStepRepo.Delete(ProcessStepList.ElementAt(deletedStep - 1));
                            ProcessStepList.RemoveAt(deletedStep - 1);
                        }

                    }
                    catch (NullReferenceException ex)
                    {
                        Console.WriteLine("Keine Element wurde ausgewählt");
                    }

                    FillListBoxProcessStep();
                    ProcessStepList.ToList().ForEach(p =>Console.WriteLine(p.Step));

                    //---- default cursor
                    this.Cursor = Cursors.Default;

                    //----

                    ThreadStart myThreadDelegate = new ThreadStart(new Recorder(this).StartMousePointer);
                    myThread = new Thread(myThreadDelegate);
                    myThread.Start();

                }
                else
                {
                    Console.WriteLine("********************** NO");

                    //---- wait cursor
                    this.Cursor = Cursors.WaitCursor;
                    //----

                    this.delete_step.Enabled = false;
                    //ProcessStep ProcessStepFromList = (ProcessStep)this.listBoxProcessSteps.SelectedItem;

                    Database.Repository.ProcessStepRepository processStepRepo = new Database.Repository.ProcessStepRepository();

                    try
                    {
                        deletedStep = (Math.Abs(ProcessStepFromList.Step));

                        processStepRepo.Delete(ProcessStepList.ElementAt(deletedStep - 1));
                        ProcessStepList.RemoveAt(deletedStep - 1);


                    }
                    catch (NullReferenceException RefExn)
                    {
                        Console.WriteLine("Keine Element wurde ausgewählt");
                    }

                    FillListBoxProcessStep();
                    ProcessStepList.ToList().ForEach(p =>Console.WriteLine(p.Step));

                    //---- default cursor
                    this.Cursor = Cursors.Default;
                    //----
                    ThreadStart myThreadDelegate = new ThreadStart(new Recorder(this).StartMousePointer);
                    myThread = new Thread(myThreadDelegate);
                    myThread.Start();


                }

            }
        }

        private void listBoxProcessSteps_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxProcessSteps.SelectedItem != null)
            {
                this.delete_step.Enabled = true;
                // beschreibung soll bei klick auf objekt in textBoxDescription erscheinen
                ProcessStep ProcessStepFromList = (ProcessStep)this.listBoxProcessSteps.SelectedItem;
                this.textBoxDescription.Text = ProcessStepFromList.Description.ToString();
            }
        }



        private void labelInformation_Click(object sender, EventArgs e)
        {

        }


    }
}
