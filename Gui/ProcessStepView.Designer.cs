namespace Gui
{
    partial class ProcessstepView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessstepView));
            this.listbox_choose = new System.Windows.Forms.ListBox();
            this.listbox_processteps = new System.Windows.Forms.ListBox();
            this.visualizer_select = new Gui.simpleButton();
            this.visualizer_stop = new Gui.simpleButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listbox_choose
            // 
            this.listbox_choose.FormattingEnabled = true;
            this.listbox_choose.Location = new System.Drawing.Point(12, 27);
            this.listbox_choose.Name = "listbox_choose";
            this.listbox_choose.Size = new System.Drawing.Size(144, 108);
            this.listbox_choose.TabIndex = 4;
            this.listbox_choose.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // listbox_processteps
            // 
            this.listbox_processteps.FormattingEnabled = true;
            this.listbox_processteps.Location = new System.Drawing.Point(169, 27);
            this.listbox_processteps.Name = "listbox_processteps";
            this.listbox_processteps.Size = new System.Drawing.Size(247, 108);
            this.listbox_processteps.TabIndex = 6;
            // 
            // visualizer_select
            // 
            this.visualizer_select.Location = new System.Drawing.Point(81, 141);
            this.visualizer_select.Name = "visualizer_select";
            this.visualizer_select.Size = new System.Drawing.Size(75, 27);
            this.visualizer_select.TabIndex = 7;
            this.visualizer_select.Title = "Select";
            this.visualizer_select.Click += new System.EventHandler(this.visualizer_select_Click);
            // 
            // visualizer_stop
            // 
            this.visualizer_stop.Location = new System.Drawing.Point(341, 141);
            this.visualizer_stop.Name = "visualizer_stop";
            this.visualizer_stop.Size = new System.Drawing.Size(75, 27);
            this.visualizer_stop.TabIndex = 8;
            this.visualizer_stop.Title = "Stop";
            this.visualizer_stop.Click += new System.EventHandler(this.visualizer_stop_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(428, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem2.Text = "?";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // ProcessstepView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 174);
            this.Controls.Add(this.visualizer_stop);
            this.Controls.Add(this.visualizer_select);
            this.Controls.Add(this.listbox_processteps);
            this.Controls.Add(this.listbox_choose);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ProcessstepView";
            this.Text = "ProcessstepView";
            this.Load += new System.EventHandler(this.Anzeige_Load);
            this.Resize += new System.EventHandler(this.Anzeige_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listbox_choose;
        private System.Windows.Forms.ListBox listbox_processteps;
        private simpleButton visualizer_select;
        private simpleButton visualizer_stop;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;

    }
}