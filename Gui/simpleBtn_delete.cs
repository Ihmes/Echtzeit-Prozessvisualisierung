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
    public enum Btn_State_delete { Delete_default, Delete_hover, Delete_down, Delete_disabled }

    [DefaultEvent("Click")]
    public partial class simpleBtn_delete : UserControl
    {

        private Image defaultDelete;
        private Image overDelete;
        private Image downDelete;
        private Image disabledDelete;

        private bool mouseDown;
        private bool mouseOver;

        private bool isDisabled;

        private string title;

        public simpleBtn_delete()
        {
            this.defaultDelete = Res2.delete_orange;
            this.overDelete = Res2.delete_orange_hover;
            this.downDelete = Res2.delete_orange_down;

            isDisabled = false;

            this.InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();
        }

        private void InitializeComponent()
        {
            
        }

        public void setState(Btn_State_delete color)
        {
            switch (color)
            {
                case Btn_State_delete.Delete_default: this.defaultDelete = Res2.delete_orange; isDisabled = false; break;
                case Btn_State_delete.Delete_hover: this.overDelete = Res2.delete_orange_hover; isDisabled = false; break;
                case Btn_State_delete.Delete_down: this.downDelete = Res2.delete_orange_down; isDisabled = false; break;
                case Btn_State_delete.Delete_disabled: this.disabledDelete = Res2.delete_grey; isDisabled = true; break;
            }
        }


        public string Title
        {
            get { return this.title; }
            set { this.title = value; this.Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (isDisabled == true)
            {
                e.Graphics.DrawImage(this.disabledDelete, this.ClientRectangle);
            }
            else
            {
                if (this.mouseDown)
                {
                    e.Graphics.DrawImage(this.downDelete, this.ClientRectangle);
                }
                else if (this.mouseOver)
                {
                    e.Graphics.DrawImage(this.overDelete, this.ClientRectangle);
                }
                else
                {
                    e.Graphics.DrawImage(this.defaultDelete, this.ClientRectangle);
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
