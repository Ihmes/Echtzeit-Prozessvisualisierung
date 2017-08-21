namespace Gui
{
    partial class DesignerMainFrame
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DesignerMainFrame));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.neuesProjektToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.beendenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hilfeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.listBoxProject = new System.Windows.Forms.ListBox();
            this.listBoxProcess = new System.Windows.Forms.ListBox();
            this.listBoxProcessStep = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_back = new Gui.simpleButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_add_process = new Gui.simpleButton();
            this.btn_ps_add_edit = new Gui.simpleButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.delete_project = new Gui.simpleBtn_delete();
            this.delete_process = new Gui.simpleBtn_delete();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiToolStripMenuItem,
            this.hilfeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(434, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dateiToolStripMenuItem
            // 
            this.dateiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.neuesProjektToolStripMenuItem,
            this.beendenToolStripMenuItem});
            this.dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
            this.dateiToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.dateiToolStripMenuItem.Text = "File";
            // 
            // neuesProjektToolStripMenuItem
            // 
            this.neuesProjektToolStripMenuItem.Name = "neuesProjektToolStripMenuItem";
            this.neuesProjektToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.neuesProjektToolStripMenuItem.Text = "New Project";
            this.neuesProjektToolStripMenuItem.Click += new System.EventHandler(this.neuesProjektToolStripMenuItem_Click);
            // 
            // beendenToolStripMenuItem
            // 
            this.beendenToolStripMenuItem.Name = "beendenToolStripMenuItem";
            this.beendenToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.beendenToolStripMenuItem.Text = "Close";
            this.beendenToolStripMenuItem.Click += new System.EventHandler(this.beendenToolStripMenuItem_Click);
            // 
            // hilfeToolStripMenuItem
            // 
            this.hilfeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.hilfeToolStripMenuItem.Name = "hilfeToolStripMenuItem";
            this.hilfeToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.hilfeToolStripMenuItem.Text = "Help";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(79, 22);
            this.toolStripMenuItem2.Text = "?";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Location = new System.Drawing.Point(0, 479);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(434, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // listBoxProject
            // 
            this.listBoxProject.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxProject.FormattingEnabled = true;
            this.listBoxProject.ItemHeight = 16;
            this.listBoxProject.Location = new System.Drawing.Point(12, 63);
            this.listBoxProject.Name = "listBoxProject";
            this.listBoxProject.Size = new System.Drawing.Size(191, 148);
            this.listBoxProject.TabIndex = 3;
            this.listBoxProject.SelectedIndexChanged += new System.EventHandler(this.listBoxProject_SelectedIndexChanged);
            // 
            // listBoxProcess
            // 
            this.listBoxProcess.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.listBoxProcess.FormattingEnabled = true;
            this.listBoxProcess.ItemHeight = 16;
            this.listBoxProcess.Location = new System.Drawing.Point(229, 64);
            this.listBoxProcess.Name = "listBoxProcess";
            this.listBoxProcess.Size = new System.Drawing.Size(193, 148);
            this.listBoxProcess.TabIndex = 4;
            this.listBoxProcess.SelectedIndexChanged += new System.EventHandler(this.listBoxProcess_SelectedIndexChanged);
            // 
            // listBoxProcessStep
            // 
            this.listBoxProcessStep.Enabled = false;
            this.listBoxProcessStep.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.listBoxProcessStep.FormattingEnabled = true;
            this.listBoxProcessStep.ItemHeight = 16;
            this.listBoxProcessStep.Location = new System.Drawing.Point(12, 287);
            this.listBoxProcessStep.Name = "listBoxProcessStep";
            this.listBoxProcessStep.Size = new System.Drawing.Size(410, 148);
            this.listBoxProcessStep.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Gold;
            this.label2.Font = new System.Drawing.Font("Verdana", 12F);
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
            this.label2.Location = new System.Drawing.Point(226, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 18);
            this.label2.TabIndex = 6;
            this.label2.Text = "Process";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Violet;
            this.label3.Font = new System.Drawing.Font("Verdana", 12F);
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.Location = new System.Drawing.Point(9, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 18);
            this.label3.TabIndex = 7;
            this.label3.Text = "Process Steps";
            // 
            // btn_back
            // 
            this.btn_back.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_back.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_back.Location = new System.Drawing.Point(293, 479);
            this.btn_back.Name = "btn_back";
            this.btn_back.Size = new System.Drawing.Size(129, 25);
            this.btn_back.TabIndex = 8;
            this.btn_back.Title = "Go back";
            this.btn_back.Click += new System.EventHandler(this.btn_back_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.Location = new System.Drawing.Point(9, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Project";
            // 
            // btn_add_process
            // 
            this.btn_add_process.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_add_process.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_add_process.Location = new System.Drawing.Point(293, 218);
            this.btn_add_process.Name = "btn_add_process";
            this.btn_add_process.Size = new System.Drawing.Size(129, 27);
            this.btn_add_process.TabIndex = 9;
            this.btn_add_process.Title = "Add Process";
            this.btn_add_process.Click += new System.EventHandler(this.btn_add_process_Click);
            // 
            // btn_ps_add_edit
            // 
            this.btn_ps_add_edit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ps_add_edit.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_ps_add_edit.Location = new System.Drawing.Point(293, 438);
            this.btn_ps_add_edit.Name = "btn_ps_add_edit";
            this.btn_ps_add_edit.Size = new System.Drawing.Size(129, 27);
            this.btn_ps_add_edit.TabIndex = 10;
            this.btn_ps_add_edit.Title = "Add / Edit Step";
            this.btn_ps_add_edit.Click += new System.EventHandler(this.btn_ps_add_edit_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Orange;
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(0, 255);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(435, 25);
            this.panel1.TabIndex = 11;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Orange;
            this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(0, 33);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(435, 25);
            this.panel2.TabIndex = 12;
            // 
            // delete_project
            // 
            this.delete_project.Location = new System.Drawing.Point(12, 218);
            this.delete_project.Name = "delete_project";
            this.delete_project.Size = new System.Drawing.Size(27, 27);
            this.delete_project.TabIndex = 13;
            this.delete_project.Title = null;
            this.delete_project.Click += new System.EventHandler(this.delete_project_Click);
            // 
            // delete_process
            // 
            this.delete_process.Location = new System.Drawing.Point(229, 218);
            this.delete_process.Name = "delete_process";
            this.delete_process.Size = new System.Drawing.Size(27, 27);
            this.delete_process.TabIndex = 14;
            this.delete_process.Title = null;
            this.delete_process.Click += new System.EventHandler(this.delete_process_Click);
            // 
            // DesignerMainFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(434, 504);
            this.Controls.Add(this.delete_process);
            this.Controls.Add(this.delete_project);
            this.Controls.Add(this.btn_ps_add_edit);
            this.Controls.Add(this.btn_add_process);
            this.Controls.Add(this.btn_back);
            this.Controls.Add(this.listBoxProcessStep);
            this.Controls.Add(this.listBoxProcess);
            this.Controls.Add(this.listBoxProject);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "DesignerMainFrame";
            this.Text = "Process Designer";
            this.Load += new System.EventHandler(this.MainFrame_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem hilfeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ListBox listBoxProject;
        private System.Windows.Forms.ListBox listBoxProcess;
        private System.Windows.Forms.ListBox listBoxProcessStep;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem neuesProjektToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem beendenToolStripMenuItem;
        private simpleButton btn_back;
        private System.Windows.Forms.Label label1;
        private simpleButton btn_add_process;
        private simpleButton btn_ps_add_edit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private simpleBtn_delete delete_project;
        private simpleBtn_delete delete_process;
    }
}

