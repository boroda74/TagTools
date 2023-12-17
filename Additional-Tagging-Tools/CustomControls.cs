using System;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ExtensionMethods
{
    internal static class NativeMethods
    {
        private const int EM_SETCUEBANNER = 0x1501;
        private const int CB_SETCUEBANNER = 0x1703;
        private static IntPtr Zero = IntPtr.Zero;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        public static void SetCue(this TextBox textBox, string cue)
        {
            SendMessage(textBox.Handle, EM_SETCUEBANNER, Zero, cue);
        }

        public static void ClearCue(this TextBox textBox)
        {
            SendMessage(textBox.Handle, EM_SETCUEBANNER, Zero, string.Empty);
        }

        public static void SetCue(this ComboBox comboBox, string cue)
        {
            SendMessage(comboBox.Handle, CB_SETCUEBANNER, Zero, cue);
        }

        public static void ClearCue(this ComboBox comboBox)
        {
            SendMessage(comboBox.Handle, CB_SETCUEBANNER, Zero, string.Empty);
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
