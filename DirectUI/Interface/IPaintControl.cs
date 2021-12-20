using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectUI.Interface
{
    public interface IPaintControl
    {
        void Paint(PaintEventArgs e);

        void MouseMove(MouseEventArgs e);
        void MouseDown(MouseEventArgs e);
        void MouseUp(MouseEventArgs e);
        void MouseWheel(MouseEventArgs e);
        void MouseEnter();
        void MouseLeave();
    }
}
