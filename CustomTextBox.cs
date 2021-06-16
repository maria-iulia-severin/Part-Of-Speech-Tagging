using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_Tagging
{
    public partial class CustomTextBox : UserControl
    {
        //Fields
        private Color borderColor = Color.FromArgb(18, 118, 121);
        private int borderSize = 2;
        private bool underlinedStyle = false;

        //Constructor
        public CustomTextBox()
        {
            InitializeComponent();
        }

        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                this.Invalidate();
            }
        }
        public int BorderSize
        {
            get { return borderSize; }
            set { borderSize = value; 
                this.Invalidate(); }
        }
        public bool UnderlinedStyle
        {
            get { return underlinedStyle; }
            set { underlinedStyle = value; 
                this.Invalidate(); }
        }

        //Overridden methods
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graph = e.Graphics;

            //Draw border
            using (Pen penBoder  = new Pen(borderColor,borderSize))
            {
                penBoder.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                if(underlinedStyle)
                {
                    graph.DrawLine(penBoder, 0, this.Height - 1, this.Width, this.Height - 1);
                }
                else
                {
                    graph.DrawLine(penBoder, 0, this.Height - 1, this.Width, this.Height - 1);
                    //graph.DrawRectangle(penBoder, 0, 0, this.Width - 0.5F, this.Height - 0, 5F);
                }
            }         
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if(this.DesignMode)
                UpdateControlHeight();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateControlHeight();
        }
        protected void UpdateControlHeight()
        {
            if(textBoxPropozitie.Multiline==false)
            {
                int txtHeight = TextRenderer.MeasureText("Text", this.Font).Height + 1;
                textBoxPropozitie.Multiline = true;
                textBoxPropozitie.MinimumSize = new Size(0, txtHeight);
                textBoxPropozitie.Multiline = false;

                this.Height = textBoxPropozitie.Height + this.Padding.Top + this.Padding.Bottom;
            }
        }
    }
}
