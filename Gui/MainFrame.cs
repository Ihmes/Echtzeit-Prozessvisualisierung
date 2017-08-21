using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gui
{
    public partial class MainFrame : Form
    {
        public MainFrame()
        {
            InitializeComponent();
        }

        private void btn_pd_Click(object sender, EventArgs e)
        {
            //---- wait cursor
            this.Cursor = Cursors.WaitCursor;
            //----

            new DesignerMainFrame().Visible = true;
            this.Hide();
        }

        private void btn_pv_Click_1(object sender, EventArgs e)
        {

            //---- wait cursor
            this.Cursor = Cursors.WaitCursor;
            //----

            
            ProcessstepView anz = new ProcessstepView();
            anz.Visible = true;
            anz.TopMost = true;
            this.Hide();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                this.Dispose();
                if (CloseReason.UserClosing == e.CloseReason) Application.Exit();
            }
            catch (Exception ex)
            {
                this.Dispose();
                Application.Exit();
            }
        }

        private void MainFrame_Load(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.sennty.de/Prozessvisualisierung/Hilfe.html");
        }

       
    }
}
