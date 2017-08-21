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
    public partial class NewProcessFrame : Form
    {
        private delegate void SenderHandler(object sender, EventArgs e);
        private event SenderHandler _event;

        public NewProcessFrame(DesignerMainFrame mainFrame)
        {
            InitializeComponent();
            _event += new SenderHandler(mainFrame.ReadNewProcessFormularData);
        }

        private void NewProcessFrame_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            Process newProject = new Process(this.textBoxName.Text, this.textBoxDescription.Text, SelectedObjects.Project);
            if (_event != null)
            {
                _event(newProject, new EventArgs());
            }
            this.Dispose();
        }

        private void btn_cancel_Click_1(object sender, EventArgs e)
        {
            this.Dispose();
        }

       

    }
}
