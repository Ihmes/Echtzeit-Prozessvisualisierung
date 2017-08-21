using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Database.Domain;
using Database.Repository;
using System.Threading;

namespace Gui
{
    public partial class DesignerMainFrame : Form
    {

        private IList<Project> ProjectList;
        private ProjectRepository ProjectRepo;

        public DesignerMainFrame()
        {
            InitializeComponent();
            ProjectRepo = new ProjectRepository();
            ProjectList = ProjectRepo.GetAllProjects();
        }

        private void MainFrame_Load(object sender, EventArgs e)
        {
            //Update MainFrame
            FillListBoxProject();
        }

        private void UpdateMainFrame()
        {
            if (listBoxProject.SelectedItem == null)
            {
                btn_add_process.setState(Gui.Btn_State.Grey_disabled);
                btn_add_process.Enabled = false;

                delete_process.setState(Gui.Btn_State_delete.Delete_disabled);
                delete_process.Enabled = false;
            }
            if (listBoxProcess.SelectedItem == null)
            {
                btn_ps_add_edit.setState(Gui.Btn_State.Grey_disabled);
                btn_ps_add_edit.Enabled = false;
            }

        }

        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Dispose();
                Application.Exit();
            }
            catch (Exception ee)
            {
                Application.Exit();
                Console.WriteLine(ee.StackTrace);
            }
        }

        private void listBoxProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedObjects.Project = (Project)listBoxProject.SelectedItem;
            SelectedObjects.Process = null;
            SelectedObjects.ProcessStep = null;
            listBoxProcess.Items.Clear();
            listBoxProcessStep.Items.Clear();
            FillListBoxProcess();

            this.btn_add_process.setState(Gui.Btn_State.Orange_default);
            this.btn_add_process.Enabled = true;

            this.delete_process.setState(Gui.Btn_State_delete.Delete_default);
            this.delete_process.Enabled = true;
        }

        private void listBoxProcess_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedObjects.Process = (Process)listBoxProcess.SelectedItem;
            SelectedObjects.ProcessStep = null;
            listBoxProcessStep.Items.Clear();
            FillListBoxProcessStep();

            this.btn_ps_add_edit.setState(Gui.Btn_State.Orange_default);
            this.btn_ps_add_edit.Enabled = true;
        }

        private void FillListBoxProject()
        {
            listBoxProject.Items.Clear();
            listBoxProcess.Items.Clear();
            listBoxProcessStep.Items.Clear();
            SelectedObjects.Project = null;
            SelectedObjects.Process = null;
            SelectedObjects.ProcessStep = null;
            foreach (Project project in ProjectList)
            {
                this.listBoxProject.Items.Add(project);
            }
            UpdateMainFrame();
        }

        private void FillListBoxProcess()
        {
            listBoxProcess.Items.Clear();
            listBoxProcessStep.Items.Clear();
            foreach (Process p in SelectedObjects.Project.Processes)
            {
                this.listBoxProcess.Items.Add(p);
            }
            UpdateMainFrame();
        }

        private void FillListBoxProcessStep()
        {
            listBoxProcessStep.Items.Clear();
            //TODO
            //Fehler abfangen
            foreach (ProcessStep p in SelectedObjects.Process.ProcessSteps)
            {
                this.listBoxProcessStep.Items.Add(p);
                Console.WriteLine(p.Step);
            }
            UpdateMainFrame();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            //shows mainframe again
            new MainFrame().Visible = true;
            this.Hide();
        }

        public void ReadNewProjectFormularData(object sender, EventArgs e)
        {
            Project newProject = (Project)sender;
            if (ProjectRepo != null)
            {
                ProjectRepo.Add(newProject);
                ProjectList.Add(newProject);
                FillListBoxProject();
            }
        }

        public void ReadNewProcessFormularData(object sender, EventArgs e)
        {
            Process newProcess = (Process)sender;
            SelectedObjects.Project.Processes.Add(newProcess);
            if (ProjectRepo != null)
            {
                ProjectRepo.Update(SelectedObjects.Project);
                FillListBoxProcess();
            }
        }

        public void RecordMainFrameEvent(object sender, EventArgs e)
        {
            this.Visible = true;
            FillListBoxProcessStep();
        }

        private void neuesProjektToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new NewProjectFrame(this).Visible = true;
        }

        private void btn_back_Click_1(object sender, EventArgs e)
        {
            new MainFrame().Visible = true;
            this.Hide();
        }

        private void btn_add_process_Click(object sender, EventArgs e)
        {
            new NewProcessFrame(this).Visible = true;
        }

        private void btn_ps_add_edit_Click(object sender, EventArgs e)
        {
            this.Hide();
            new RecordFrame(this).Visible = true;
        }

        private void listBoxProcessStep_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void delete_process_Click(object sender, EventArgs e)
        {
            //---- wait cursor
            this.Cursor = Cursors.WaitCursor;
            //----

            Process ProcessFromList = (Process)this.listBoxProcess.SelectedItem;
            Project ProjectFromList = (Project)this.listBoxProject.SelectedItem;

            Database.Repository.ProcessRepository db_process = new Database.Repository.ProcessRepository();

            db_process.Delete(ProcessFromList);


            ProjectList.Clear();
            ProjectList = ProjectRepo.GetAllProjects();

            MainFrame_Load(sender, e);

            //---- default cursor
            this.Cursor = Cursors.Default;
            //----
            UpdateMainFrame();

        }

        private void delete_project_Click(object sender, EventArgs e)
        {
            //---- wait cursor
            this.Cursor = Cursors.WaitCursor;
            //----

            Project ProjectFromList = (Project)this.listBoxProject.SelectedItem;

            Database.Repository.ProjectRepository db_project = new Database.Repository.ProjectRepository();
            db_project.Delete(ProjectFromList);

            ProjectList.Clear();
            ProjectList = ProjectRepo.GetAllProjects();

            MainFrame_Load(sender, e);
            this.Cursor = Cursors.Default;
            UpdateMainFrame();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.sennty.de/Prozessvisualisierung/Hilfe.html");
        }

    }
}
