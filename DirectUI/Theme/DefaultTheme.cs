using DirectUI.Interface;
using DirectUI.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectUI.Theme
{
    public class DefaultTheme : IPaintForm
    {
        RectangleF _captionRect;
        RectangleF _closeRect;
        bool _closeHover;

        DirectForm _owner;

        public DefaultTheme(DirectForm owner)
        {
            _owner = owner;
        }

        public void MouseDown(MouseEventArgs e)
        {
        }

        public void MouseEnter()
        {
        }

        public void MouseLeave()
        {
            if (_closeHover)
            {
                _closeHover = false;
                _owner.Invalidate();
            }
        }

        public void MouseMove(MouseEventArgs e)
        {
            if (_closeRect.Contains(e.Location))
            {
                if (!_closeHover)
                {
                    _closeHover = true;
                    _owner.Invalidate();
                }
            }
            else if (_closeHover)
            {
                _closeHover = false;
                _owner.Invalidate();
            }
        }

        public void MouseUp(MouseEventArgs e)
        {
            if (_closeRect.Contains(e.Location))
            {
                _owner.Close();
            }
        }

        public void MouseWheel(MouseEventArgs e)
        {
        }

        public void Paint(PaintEventArgs e)
        {
        }

        public void PaintCaption(PaintEventArgs e)
        {
            _captionRect = new RectangleF(
                _owner.Padding.Left,
                _owner.Padding.Top,
                e.ClipRectangle.Width - _owner.CaptionHeight - (_owner.Padding.Left + _owner.Padding.Right),
                _owner.CaptionHeight);

            RectangleF _innerCaptionRect = new RectangleF(
                _captionRect.X,
                _captionRect.Y,
                e.ClipRectangle.Width - (_owner.Padding.Left + _owner.Padding.Right),
                _owner.CaptionHeight);

            using (SolidBrush brush = new SolidBrush(_owner.CaptionColor))
            {
                e.Graphics.FillRectangle(brush, _innerCaptionRect);
            }

            using (SolidBrush brush = new SolidBrush(Color.White))
            {
                using (var _centerFormat = StringFormat.GenericTypographic)
                {
                    _centerFormat.Alignment = StringAlignment.Near;
                    _centerFormat.LineAlignment = StringAlignment.Center;

                    e.Graphics.DrawString(_owner.Text, _owner.Font, brush, new RectangleF(5 + _captionRect.X, _captionRect.Y, _captionRect.Width, _captionRect.Height), _centerFormat);
                }
            }
        }

        private float _iconSizeProportion = 1.8f;

        public void PaintClose(PaintEventArgs e)
        {
            _closeRect = new RectangleF(
                e.ClipRectangle.Width - _owner.CaptionHeight - _owner.Padding.Right,
                _owner.Padding.Top,
                _owner.CaptionHeight,
                _owner.CaptionHeight);

            float wh = _closeRect.Width / _iconSizeProportion;

            RectangleF _innerIconRect = new RectangleF(_closeRect.X + ((_closeRect.Width - wh) / 2), _closeRect.Y + ((_closeRect.Height - wh) / 2), wh, wh);

            if (_closeHover)
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(32, 255, 255, 255)))
                {
                    e.Graphics.FillRectangle(brush, _closeRect);
                }
            }

            using (HightQualityGraphics g = new HightQualityGraphics(e.Graphics))
            {
                e.Graphics.DrawImage(Resources.close, _innerIconRect);
            }
        }

        public RectangleF GetCaptionBounds()
        {
            return _captionRect;
        }
    }
}