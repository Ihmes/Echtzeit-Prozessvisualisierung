using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gui
{
    public enum Btn_State { Orange_default, Orange_hover, Orange_down, Grey_disabled }

    [DefaultEvent("Click")]
    public partial class simpleButton : UserControl
    {

        private Image defaultImage;
        private Image overImage;
        private Image downImage;
        private Image disabledImage;

        private bool mouseDown;
        private bool mouseOver;

        private bool isDisabled;

        private string title;

        public simpleButton()
        {
            this.defaultImage = Res.btn_2_orange;
            this.overImage = Res.btn_2_orange_over;
            this.downImage = Res.btn_2_orange_down;
            isDisabled = false;

            this.InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();
        }

        public void setState(Btn_State color)
        {
            switch (color)
            {
                case Btn_State.Orange_default: this.defaultImage = Res.btn_2_orange; isDisabled = false; break;
                case Btn_State.Orange_hover: this.overImage = Res.btn_2_orange_over; isDisabled = false; break;
                case Btn_State.Orange_down: this.downImage = Res.btn_2_orange_down; isDisabled = false; break;
                case Btn_State.Grey_disabled: this.disabledImage = Res.btn_2_grey; isDisabled = true; break;


            }
        }


        public string Title
        {
            get { return this.title; }
            set { this.title = value; this.Invalidate(); }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // btn
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "btn";
            this.Size = new System.Drawing.Size(114, 27);
            this.Load += new System.EventHandler(this.btn_Load);
            this.ResumeLayout(false);

        }

        private void btn_Load(object sender, EventArgs e)
        {

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (isDisabled == true)
            {
                e.Graphics.DrawImage(this.disabledImage, this.ClientRectangle);

            }
            else
            {
                if (this.mouseDown)
                {
                    e.Graphics.DrawImage(this.downImage, this.ClientRectangle);
                }
                else if (this.mouseOver)
                {
                    e.Graphics.DrawImage(this.overImage, this.ClientRectangle);
                }
                else
                {
                    e.Graphics.DrawImage(this.defaultImage, this.ClientRectangle);
                }
            }
            e.Graphics.DrawString(this.title, this.Font, new SolidBrush(this.ForeColor), this.ClientRectangle, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.mouseOver = true;
            this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.mouseOver = false;
            this.Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.mouseDown = true;
            this.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.mouseDown = false;
            this.Invalidate();
        }
    }
}
