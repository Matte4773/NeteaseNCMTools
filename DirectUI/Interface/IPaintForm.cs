using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectUI.Interface
{
    public interface IPaintForm : IPaintControl
    {
        void PaintCaption(PaintEventArgs g);
        void PaintClose(PaintEventArgs e);

        RectangleF GetCaptionBounds();
    }
}
