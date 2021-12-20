using DirectUI.Interface;
using DirectUI.Theme;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectUI
{
    public class DirectForm : Form
    {
        private Bitmap _canvas;
        private Graphics _graphics;

        private int _captionHeight = 36;
        public int CaptionHeight { get => _captionHeight; set => _captionHeight = value; }

        // netease default theme color
        private Color _captionColor = Color.FromArgb(198, 47, 47);
        public Color CaptionColor { get => _captionColor; set => _captionColor = value; }

        private Color _backColor = Color.FromArgb(230, 56, 56, 56);
        public override Color BackColor { get => _backColor; set => _backColor = value; }

        private IPaintForm _theme = null;
        public IPaintForm Theme
        {
            get
            {
                return _theme;
            }
            set
            {
                _theme = value ?? throw new ArgumentNullException("Theme");
                Invalidate();
            }
        }

        public new FormBorderStyle FormBorderStyle
        {
            get
            {
                return base.FormBorderStyle;
            }
        }

        public RectangleF ClientArea
        {
            get
            {
                return new RectangleF(
                    Padding.Left,
                    Padding.Top + CaptionHeight,
                    _canvas.Width - (Padding.Left + Padding.Right),
                    _canvas.Height - CaptionHeight - (Padding.Top + Padding.Bottom));
            }
        }

        private void InitCanvas()
        {
            if (_canvas == null || _canvas.Width != Width || _canvas.Height != Height)
            {
                if (_graphics != null)
                    _graphics.Dispose();
                if (_canvas != null)
                    _canvas.Dispose();

                _canvas = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                _graphics = Graphics.FromImage(_canvas);
            }
        }

        public DirectForm()
        {
            base.FormBorderStyle = FormBorderStyle.None;
            _theme = new DefaultTheme(this);
            MinimizeBox = false;
            MaximizeBox = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitCanvas();
            Invalidate();
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            InitCanvas();
            Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            // paint caption
            Theme.PaintCaption(e);
            Theme.PaintClose(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Theme.Paint(e);
        }

        public new void Invalidate()
        {
            PaintEventArgs args = new PaintEventArgs(_graphics, new Rectangle(Point.Empty, _canvas.Size));
            OnPaintBackground(args);
            OnPaint(args);

            if (_canvas != null)
            {
                IntPtr screenDc = Win32.GetDC(IntPtr.Zero);
                IntPtr memDc = Win32.CreateCompatibleDC(screenDc);
                IntPtr hBitmap = IntPtr.Zero;
                IntPtr oldBitmap = IntPtr.Zero;

                try
                {
                    hBitmap = _canvas.GetHbitmap(Color.FromArgb(0));
                    oldBitmap = Win32.SelectObject(memDc, hBitmap);

                    Point topPos = Location;
                    Size size = _canvas.Size;
                    Point pointSource = Point.Empty;

                    Win32.BLENDFUNCTION blend = new Win32.BLENDFUNCTION();
                    blend.BlendOp = Win32.AC_SRC_OVER;
                    blend.BlendFlags = 0;
                    blend.SourceConstantAlpha = 255;
                    blend.AlphaFormat = Win32.AC_SRC_ALPHA;

                    Win32.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, Win32.ULW_ALPHA);
                }
                finally
                {
                    Win32.ReleaseDC(IntPtr.Zero, screenDc);
                    if (hBitmap != IntPtr.Zero)
                    {
                        Win32.SelectObject(memDc, oldBitmap);
                        Win32.DeleteObject(hBitmap);
                    }
                    Win32.DeleteDC(memDc);
                }
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (!DesignMode)
                {
                    cp.ExStyle |= 0x00080000;
                    const int WS_MINIMIZEBOX = 0x00020000;
                    cp.Style = cp.Style | WS_MINIMIZEBOX;
                }
                return cp;
            }
        }

        protected override void WndProc(ref Message m)
        {
            // for move form
            if (m.Msg != 71)
            {
                switch (m.Msg)
                {
                    case 132:
                        int value = m.LParam.ToInt32();
                        Point p = new Point(Win32.LOWORD(value), Win32.HIWORD(value));
                        p = new Point(p.X - Left, p.Y - Top);
                        if (Theme.GetCaptionBounds().Contains(p))
                        {
                            m.Result = new IntPtr(2);
                            return;
                        }
                        break;
                }
            }

            base.WndProc(ref m);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            Theme.MouseEnter();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            Theme.MouseLeave();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Theme.MouseMove(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Theme.MouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Theme.MouseUp(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            Theme.MouseWheel(e);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DirectForm));
            this.SuspendLayout();
            // 
            // DirectForm
            // 
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.ForeColor = System.Drawing.Color.Coral;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DirectForm";
            this.Load += new System.EventHandler(this.DirectForm_Load);
            this.ResumeLayout(false);

        }

        private void DirectForm_Load(object sender, EventArgs e)
        {

        }
    }
}
