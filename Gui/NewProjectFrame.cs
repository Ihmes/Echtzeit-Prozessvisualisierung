using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Database.Domain;

namespace Gui
{
    public partial class NewProjectFrame : Form
    {
        private delegate void SenderHandler(object sender, EventArgs e);
        private event SenderHandler _event;

        public NewProjectFrame(DesignerMainFrame mainFrame)
        {
            InitializeComponent();
            _event += new SenderHandler(mainFrame.ReadNewProjectFormularData);
        }

        private void NewProjectFrame_Load(object sender, EventArgs e)
        {

        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            Project newProject = new Project(this.textBoxName.Text, this.textBoxDescription.Text);
            if (_event != null)
            {
                _event(newProject, new EventArgs());
            }
            this.Dispose();
        }


    }
}
