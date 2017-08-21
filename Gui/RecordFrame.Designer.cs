namespace Gui
{
    partial class RecordFrame
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecordFrame));
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.labelInformation = new System.Windows.Forms.Label();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonEndRecord = new Gui.simpleButton();
            this.add_step = new Gui.simpleButton();
            this.listBoxProcessSteps = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.delete_step = new Gui.simpleBtn_delete();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
            this.label2.Location = new System.Drawing.Point(251, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Name";
            // 
            // textBoxName
            // 
            this.textBoxName.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBoxName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxName.Location = new System.Drawing.Point(254, 31);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.ReadOnly = true;
            this.textBoxName.Size = new System.Drawing.Size(295, 22);
            this.textBoxName.TabIndex = 2;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.Location = new System.Drawing.Point(251, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "Description";
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDescription.Location = new System.Drawing.Point(254, 130);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(298, 87);
            this.textBoxDescription.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.Location = new System.Drawing.Point(12, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 18);
            this.label3.TabIndex = 5;
            this.label3.Text = "Process-Steps";
            // 
            // labelInformation
            // 
            this.labelInformation.AutoSize = true;
            this.labelInformation.Location = new System.Drawing.Point(12, 275);
            this.labelInformation.Name = "labelInformation";
            this.labelInformation.Size = new System.Drawing.Size(31, 13);
            this.labelInformation.TabIndex = 9;
            this.labelInformation.Text = "Info: ";
            // 
            // buttonUp
            // 
            this.buttonUp.Image = ((System.Drawing.Image)(resources.GetObject("buttonUp.Image")));
            this.buttonUp.Location = new System.Drawing.Point(144, 222);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(25, 39);
            this.buttonUp.TabIndex = 10;
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonDown
            // 
            this.buttonDown.Image = ((System.Drawing.Image)(resources.GetObject("buttonDown.Image")));
            this.buttonDown.Location = new System.Drawing.Point(175, 222);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(25, 39);
            this.buttonDown.TabIndex = 11;
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // buttonEndRecord
            // 
            this.buttonEndRecord.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonEndRecord.Location = new System.Drawing.Point(466, 231);
            this.buttonEndRecord.Name = "buttonEndRecord";
            this.buttonEndRecord.Size = new System.Drawing.Size(83, 31);
            this.buttonEndRecord.TabIndex = 12;
            this.buttonEndRecord.Title = "Save";
            this.buttonEndRecord.Click += new System.EventHandler(this.buttonEndRecord_Click_1);
            // 
            // add_step
            // 
            this.add_step.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.add_step.Location = new System.Drawing.Point(466, 56);
            this.add_step.Name = "add_step";
            this.add_step.Size = new System.Drawing.Size(83, 31);
            this.add_step.TabIndex = 13;
            this.add_step.Title = "Add";
            this.add_step.Click += new System.EventHandler(this.add_step_Click);
            // 
            // listBoxProcessSteps
            // 
            this.listBoxProcessSteps.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxProcessSteps.FormattingEnabled = true;
            this.listBoxProcessSteps.ItemHeight = 16;
            this.listBoxProcessSteps.Location = new System.Drawing.Point(15, 31);
            this.listBoxProcessSteps.Name = "listBoxProcessSteps";
            this.listBoxProcessSteps.Size = new System.Drawing.Size(185, 180);
            this.listBoxProcessSteps.TabIndex = 6;
            this.listBoxProcessSteps.SelectedIndexChanged += new System.EventHandler(this.listBoxProcessSteps_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(564, 25);
            this.panel1.TabIndex = 14;
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
            this.panel2.Location = new System.Drawing.Point(200, 99);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(364, 25);
            this.panel2.TabIndex = 15;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 269);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.Size = new System.Drawing.Size(564, 22);
            this.statusStrip1.TabIndex = 16;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // delete_step
            // 
            this.delete_step.Location = new System.Drawing.Point(206, 32);
            this.delete_step.Name = "delete_step";
            this.delete_step.Size = new System.Drawing.Size(22, 22);
            this.delete_step.TabIndex = 17;
            this.delete_step.Tag = "";
            this.delete_step.Title = "";
            this.delete_step.Click += new System.EventHandler(this.delete_step_Click);
            // 
            // RecordFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 291);
            this.Controls.Add(this.delete_step);
            this.Controls.Add(this.listBoxProcessSteps);
            this.Controls.Add(this.add_step);
            this.Controls.Add(this.buttonEndRecord);
            this.Controls.Add(this.buttonDown);
            this.Controls.Add(this.buttonUp);
            this.Controls.Add(this.labelInformation);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxDescription);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RecordFrame";
            this.Text = "Prozessschritte aufnehmen";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.RecordFrame_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelInformation;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonDown;
        private simpleButton buttonEndRecord;
        private simpleButton add_step;
        private System.Windows.Forms.ListBox listBoxProcessSteps;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private simpleBtn_delete delete_step;

    }
}