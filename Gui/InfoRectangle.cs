using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Database.Domain;


namespace Gui
{
    public partial class InfoRectangle : Form
    {

        private int rectangleSizeX = 20;
        private int rectangleSizeY = 20;

        private static int x = 40;
        private static int y = 40;
        Rectangle location;
        private int number;
        private string name;

        public InfoRectangle(Rectangle location)
        {
           InitializeComponent();

           name = "";
           number = 0;
           this.BackColor = Color.LimeGreen;
           this.TransparencyKey = Color.LimeGreen;
           this.Cursor = Cursors.Hand;
           //this.Refresh();
           FormBorderStyle = FormBorderStyle.None;
            //Fenster in Taskbar verstecken 
            this.ShowInTaskbar = false;
            this.location = location;
            setLocation( (location.X -2), (location.Y - location.Height/2) );


            // Keep the Windows Form on Top
            this.TopMost = true;
            // Create a Point object that will be used as the location of the form.
            Point tempPoint = new Point(x, y);
            // Set the location of the form using the Point object.
            this.DesktopLocation = tempPoint;


            // Create the ToolTip and associate with the Form container.
           
            
            this.toolTip1.ShowAlways = true;
            this.toolTip1.Active = true;
            this.toolTip1.SetToolTip(this, "blablalbala");
            this.Update();
            
        }

        public void setName(string Name)
        {
            this.name = Name;
        }
        public string getName(string Name)
        {
            return this.name;
        }

        public void setNumber(int number)
        {
            this.number = number;
        }

        public int getNumber()
        {
            return this.number;
        }
        /*
        public void setToolTip(String description)
        {
            this.toolTip1.ShowAlways = true;
            this.toolTip1.SetToolTip(this, description);
            this.Update();
        }
       */
            public void DrawString(String stepNumber)
        {
            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            string drawString = stepNumber;
            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 8, FontStyle.Bold);
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            float x = 2.2f;
            float y = 1.0f;
            formGraphics.DrawString(drawString, drawFont, drawBrush, x, y);
            drawFont.Dispose();
            drawBrush.Dispose();
            formGraphics.Dispose();
        }

        //Set new Location for the Rectangle
        //Aufruf: 
        //InfoRectangle pridate = new InfoRectangle();
        //pridate.setLocation(dd.Location.X, dd.Location.Y);
        //new InfoRectangle().Visible = true;
        public void setLocation(int xLocation, int yLocation)
        {
                x = xLocation;
                y = yLocation;
        }

    }
}
