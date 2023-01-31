using System;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public class ComboBoxPlus : ComboBox
    {
        private const int CB_SETCUEBANNER = 0x1703;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        private string placeholder = string.Empty;
        public string Placeholder
        {
            get { return placeholder; }
            set
            {
                placeholder = value;
                SendMessage(Handle, CB_SETCUEBANNER, 0, Placeholder);
            }
        }
    }

    public class InterpolatedBox : PictureBox
    {
        #region Interpolation Property
        private InterpolationMode interpolation = InterpolationMode.Default;

        [DefaultValue(typeof(InterpolationMode), "Default"),
        Description("The interpolation used to render the image.")]
        public InterpolationMode Interpolation
        {
            get { return interpolation; }
            set
            {
                if (value == InterpolationMode.Invalid)
                    throw new ArgumentException("\"Invalid\" is not a valid value."); // (Duh!)

                interpolation = value;
                Invalidate(); // Image should be redrawn when a different interpolation is selected
            }
        }
        #endregion

        protected override void OnPaint(PaintEventArgs pe)
        {
            // Before the PictureBox renders the image, we modify the
            // graphics object to change the interpolation.

            // Set the selected interpolation.
            pe.Graphics.InterpolationMode = interpolation;
            // Certain interpolation modes (such as nearest neighbor) need
            // to be offset by half a pixel to render correctly.
            pe.Graphics.PixelOffsetMode = PixelOffsetMode.Half;

            // Allow the PictureBox to draw.
            base.OnPaint(pe);
        }
    }
}
