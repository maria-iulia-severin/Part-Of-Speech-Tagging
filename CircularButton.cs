﻿using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace POS_Tagging
{
    class CircularButton : Button
    {
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            GraphicsPath grPath = new GraphicsPath();

            grPath.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
            this.Region = new System.Drawing.Region(grPath);
            base.OnPaint(pevent);
        }
    }
}
