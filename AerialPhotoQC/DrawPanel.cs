using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace AerialPhotoQC
{
    public class DrawPanel: Panel
    {
        public DrawPanel()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }
    }
}
