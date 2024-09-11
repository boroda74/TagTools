using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    /// <summary>
    /// Summary description for FlatTabControl.
    /// </summary>
    [ToolboxBitmap(typeof(TabControl))] //---, Designer(typeof(Designers.FlatTabControlDesigner))]

    public class FlatTabControl : TabControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Container components = null;
        private SubClass scUpDown = null;
        private bool bUpDown; // true when the button UpDown is required
        private ImageList leftRightImages = null;
        private const int nMargin = 5;
        private Color mBackColor = SystemColors.Control;
        private Color mBorderColor = SystemColors.ControlDark;
        private StringAlignment tabTextAlignment = StringAlignment.Near;

        public FlatTabControl()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // double buffering
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            bUpDown = false;

            this.ControlAdded += new ControlEventHandler(FlatTabControl_ControlAdded);
            this.ControlRemoved += new ControlEventHandler(FlatTabControl_ControlRemoved);
            this.SelectedIndexChanged += new EventHandler(FlatTabControl_SelectedIndexChanged);

            leftRightImages = new ImageList();
            //leftRightImages.ImageSize = new Size(16, 16); // default

            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FlatTabControl));
            Bitmap updownImage = ((System.Drawing.Bitmap)(resources.GetObject("TabIcons.bmp")));

            if (updownImage != null)
            {
                updownImage.MakeTransparent(Color.White);
                leftRightImages.Images.AddStrip(updownImage);
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                leftRightImages.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawControl(e.Graphics);
        }

        internal void DrawControl(Graphics g)
        {
            if (!Visible)
                return;

            Rectangle TabControlArea = this.ClientRectangle;
            Rectangle TabArea = this.DisplayRectangle;

            // fill client area
            Brush br = new SolidBrush(mBackColor);
            g.FillRectangle(br, TabControlArea);
            br.Dispose();

            // draw border
            int nDelta = SystemInformation.Border3DSize.Width;

            Pen border = new Pen(mBorderColor);
            TabArea.Inflate(nDelta, nDelta);
            g.DrawRectangle(border, TabArea);
            border.Dispose();


            // clip region for drawing tabs
            Region rsaved = g.Clip;
            Rectangle rreg;

            int nWidth = TabArea.Width + nMargin;
            if (bUpDown)
            {
                // exclude updown control for painting
                if (NativeMethods.IsWindowVisible(scUpDown.Handle))
                {
                    Rectangle rupdown = new Rectangle();
                    NativeMethods.GetWindowRect(scUpDown.Handle, ref rupdown);
                    Rectangle rupdown2 = this.RectangleToClient(rupdown);

                    nWidth = rupdown2.X;
                }
            }

            rreg = new Rectangle(TabArea.Left, TabControlArea.Top, nWidth - nMargin, TabControlArea.Height);

            g.SetClip(rreg);

            // draw tabs
            for (int i = 0; i < this.TabCount; i++)
                DrawTab(g, this.TabPages[i], i);

            g.Clip = rsaved;


            // draw background to cover flat border areas
            if (this.SelectedTab != null)
            {
                TabPage tabPage = this.SelectedTab;
                Color color = tabPage.BackColor;
                border = new Pen(color);

                TabArea.Offset(1, 1);
                TabArea.Width -= 2;
                TabArea.Height -= 2;

                g.DrawRectangle(border, TabArea);
                TabArea.Width -= 1;
                TabArea.Height -= 1;
                g.DrawRectangle(border, TabArea);

                border.Dispose();
            }
        }

        internal void DrawTab(Graphics g, TabPage tabPage, int nIndex)
        {
            Rectangle recBounds = this.GetTabRect(nIndex);
            RectangleF tabTextArea = (RectangleF)this.GetTabRect(nIndex);

            bool bSelected = (this.SelectedIndex == nIndex);

            Point[] pt = new Point[7];
            if (this.Alignment == TabAlignment.Top)
            {
                pt[0] = new Point(recBounds.Left, recBounds.Bottom);
                pt[1] = new Point(recBounds.Left, recBounds.Top + 3);
                pt[2] = new Point(recBounds.Left + 3, recBounds.Top);
                pt[3] = new Point(recBounds.Right - 3, recBounds.Top);
                pt[4] = new Point(recBounds.Right, recBounds.Top + 3);
                pt[5] = new Point(recBounds.Right, recBounds.Bottom);
                pt[6] = new Point(recBounds.Left, recBounds.Bottom);
            }
            else
            {
                pt[0] = new Point(recBounds.Left, recBounds.Top);
                pt[1] = new Point(recBounds.Right, recBounds.Top);
                pt[2] = new Point(recBounds.Right, recBounds.Bottom - 3);
                pt[3] = new Point(recBounds.Right - 3, recBounds.Bottom);
                pt[4] = new Point(recBounds.Left + 3, recBounds.Bottom);
                pt[5] = new Point(recBounds.Left, recBounds.Bottom - 3);
                pt[6] = new Point(recBounds.Left, recBounds.Top);
            }

            // fill this tab with background color
            Brush br = new SolidBrush(tabPage.BackColor);
            g.FillPolygon(br, pt);
            br.Dispose();

            // draw border
            //g.DrawRectangle(SystemPens.ControlDark, recBounds);
            Pen border = new Pen(mBorderColor);
            g.DrawPolygon(border, pt);

            if (bSelected)
            {
                // clear bottom lines
                Pen pen = new Pen(tabPage.BackColor);

                switch (this.Alignment)
                {
                    case TabAlignment.Top:
                        g.DrawLine(pen, recBounds.Left + 1, recBounds.Bottom, recBounds.Right - 1, recBounds.Bottom);
                        g.DrawLine(pen, recBounds.Left + 1, recBounds.Bottom + 1, recBounds.Right - 1, recBounds.Bottom + 1);
                        break;

                    case TabAlignment.Bottom:
                        g.DrawLine(pen, recBounds.Left + 1, recBounds.Top, recBounds.Right - 1, recBounds.Top);
                        g.DrawLine(pen, recBounds.Left + 1, recBounds.Top - 1, recBounds.Right - 1, recBounds.Top - 1);
                        g.DrawLine(pen, recBounds.Left + 1, recBounds.Top - 2, recBounds.Right - 1, recBounds.Top - 2);
                        break;
                }

                pen.Dispose();
            }

            // draw tab's icon
            if ((tabPage.ImageIndex >= 0) && (ImageList != null) && (ImageList.Images[tabPage.ImageIndex] != null))
            {
                int nLeftMargin = 8;
                int nRightMargin = 2;

                Image img = ImageList.Images[tabPage.ImageIndex];

                Rectangle rimage = new Rectangle(recBounds.X + nLeftMargin, recBounds.Y + 1, img.Width, img.Height);

                // adjust rectangles
                float nAdj = (float)(nLeftMargin + img.Width + nRightMargin);

                rimage.Y += (recBounds.Height - img.Height) / 2;
                tabTextArea.X += nAdj;
                tabTextArea.Width -= nAdj;

                // draw icon
                g.DrawImage(img, rimage);
            }

            // draw string
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = tabTextAlignment;
            stringFormat.LineAlignment = StringAlignment.Center;

            br = new SolidBrush(tabPage.ForeColor);

            if (tabTextAlignment == StringAlignment.Near)
                g.DrawString("  " + tabPage.Text, Font, br, tabTextArea, stringFormat);
            else if (tabTextAlignment == StringAlignment.Far)
                g.DrawString(tabPage.Text + "  ", Font, br, tabTextArea, stringFormat);
            else
                g.DrawString(tabPage.Text, Font, br, tabTextArea, stringFormat);
        }

        internal void DrawIcons(Graphics g)
        {
            if ((leftRightImages == null) || (leftRightImages.Images.Count != 4))
                return;

            // calc positions
            Rectangle TabControlArea = this.ClientRectangle;

            Rectangle r0 = new Rectangle();
            NativeMethods.GetClientRect(scUpDown.Handle, ref r0);

            Brush br = new SolidBrush(mBackColor);
            g.FillRectangle(br, r0);
            br.Dispose();

            Pen border = new Pen(mBorderColor);
            Rectangle rborder = r0;
            rborder.Inflate(-1, -1);
            g.DrawRectangle(border, rborder);
            border.Dispose();

            int nMiddle = (r0.Width / 2);
            int nTop = (r0.Height - 16) / 2;
            int nLeft = (nMiddle - 16) / 2;

            Rectangle r1 = new Rectangle(nLeft, nTop, 16, 16);
            Rectangle r2 = new Rectangle(nMiddle + nLeft, nTop, 16, 16);

            // draw buttons
            Image img = leftRightImages.Images[1];
            if (img != null)
            {
                if (this.TabCount > 0)
                {
                    Rectangle r3 = this.GetTabRect(0);
                    if (r3.Left < TabControlArea.Left)
                        g.DrawImage(img, r1);
                    else
                    {
                        img = leftRightImages.Images[3];
                        if (img != null)
                            g.DrawImage(img, r1);
                    }
                }
            }

            img = leftRightImages.Images[0];
            if (img != null)
            {
                if (this.TabCount > 0)
                {
                    Rectangle r3 = this.GetTabRect(this.TabCount - 1);
                    if (r3.Right > (TabControlArea.Width - r0.Width))
                        g.DrawImage(img, r2);
                    else
                    {
                        img = leftRightImages.Images[2];
                        if (img != null)
                            g.DrawImage(img, r2);
                    }
                }
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            FindUpDown();
        }

        private void FlatTabControl_ControlAdded(object sender, ControlEventArgs e) //-V3013
        {
            FindUpDown();
            UpdateUpDown();
        }

        private void FlatTabControl_ControlRemoved(object sender, ControlEventArgs e)
        {
            FindUpDown();
            UpdateUpDown();
        }

        private void FlatTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUpDown();
            Invalidate();   // we need to update border and background colors
        }

        private void FindUpDown()
        {
            bool bFound = false;

            // find the UpDown control
            IntPtr pWnd = NativeMethods.GetWindow(this.Handle, NativeMethods.GW_CHILD);

            while (pWnd != IntPtr.Zero)
            {
                // Get the window class name
                char[] className = new char[33];

                int length = NativeMethods.GetClassName(pWnd, className, 32);

                string s = new string(className, 0, length);

                if (s == "msctls_updown32")
                {
                    bFound = true;

                    if (!bUpDown)
                    {
                        // Subclass it
                        this.scUpDown = new SubClass(pWnd, true);
                        this.scUpDown.SubClassedWndProc += new SubClass.SubClassWndProcEventHandler(scUpDown_SubClassedWndProc);

                        bUpDown = true;
                    }
                    break;
                }

                pWnd = NativeMethods.GetWindow(pWnd, NativeMethods.GW_HWNDNEXT);
            }

            if ((!bFound) && (bUpDown))
                bUpDown = false;
        }

        private void UpdateUpDown()
        {
            if (bUpDown)
            {
                if (NativeMethods.IsWindowVisible(scUpDown.Handle))
                {
                    Rectangle rect = new Rectangle();

                    NativeMethods.GetClientRect(scUpDown.Handle, ref rect);
                    NativeMethods.InvalidateRect(scUpDown.Handle, ref rect, true);
                }
            }
        }

        #region scUpDown_SubClassedWndProc Event Handler
        private int scUpDown_SubClassedWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case NativeMethods.WM_PAINT:
                    {
                        // redraw
                        IntPtr hDC = NativeMethods.GetWindowDC(scUpDown.Handle);
                        Graphics g = Graphics.FromHdc(hDC);

                        DrawIcons(g);

                        g.Dispose();
                        NativeMethods.ReleaseDC(scUpDown.Handle, hDC);

                        // return 0 (processed)
                        m.Result = IntPtr.Zero;

                        // validate current rect
                        Rectangle rect = new Rectangle();

                        NativeMethods.GetClientRect(scUpDown.Handle, ref rect);
                        NativeMethods.ValidateRect(scUpDown.Handle, ref rect);
                    }

                    return 1;
            }

            return 0;
        }
        #endregion

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new Container();
        }
        #endregion

        #region Properties
        [Editor(typeof(TabPageExCollectionEditor), typeof(UITypeEditor))]
        public new TabPageCollection TabPages
        {
            get
            {
                return base.TabPages;
            }
        }

        public new TabAlignment Alignment
        {
            get { return base.Alignment; }
            set
            {
                TabAlignment ta = value;
                if ((ta != TabAlignment.Top) && (ta != TabAlignment.Bottom))
                    ta = TabAlignment.Top;

                base.Alignment = ta;
            }
        }

        [Browsable(false)]
        public new bool Multiline
        {
            get { return base.Multiline; }
            set { base.Multiline = false; } //-V3077
        }

        [Browsable(true)]
        public override Color BackColor
        {
            get { return mBackColor; }
            set { mBackColor = value; this.Invalidate(); }
        }

        [Browsable(true)]
        public Color BorderColor
        {
            get { return mBorderColor; }
            set { mBorderColor = value; this.Invalidate(); }
        }

        [Browsable(true)]
        public StringAlignment TabTextAlignment
        {
            get { return tabTextAlignment; }
            set { tabTextAlignment = value; this.Invalidate(); }
        }
        #endregion

        #region TabPageExCollectionEditor
        public class TabPageExCollectionEditor : CollectionEditor
        {
            public TabPageExCollectionEditor(System.Type type) : base(type)
            {
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(TabPage);
            }
        }
        #endregion
    }
}
