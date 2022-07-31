using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectUI
{
    public class HightQualityGraphics : IDisposable
    {
        private Graphics _g;

        public HightQualityGraphics(Graphics g)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            _g = g;
        }

        public void Dispose()
        {
            _g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            _g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            _g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
        }
    }
}
