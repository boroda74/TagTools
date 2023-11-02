﻿using System;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public class ReadOnlyCheckBox : CheckBox//*****
    {
        [Category("Appearance")]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        private bool readOnly = false;

        public bool ReadOnly
        {
            get { return readOnly; }

            set 
            { 
                readOnly = value;
                
                if (readOnly)
                {

                }
            }
        }

        protected override void OnClick(EventArgs e)
        {
            if (!ReadOnly) base.OnClick(e);
        }
    }

    public static class CueProvider
    {
        private const int EM_SETCUEBANNER = 0x1501;
        private const int CB_SETCUEBANNER = 0x1703;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        public static void SetCue(TextBox textBox, string cue)
        {
            SendMessage(textBox.Handle, EM_SETCUEBANNER, 0, cue);
        }

        public static void ClearCue(TextBox textBox)
        {
            SendMessage(textBox.Handle, EM_SETCUEBANNER, 0, string.Empty);
        }

        public static void SetCue(ComboBox comboBox, string cue)
        {
            SendMessage(comboBox.Handle, CB_SETCUEBANNER, 0, cue);
        }

        public static void ClearCue(ComboBox comboBox)
        {
            SendMessage(comboBox.Handle, CB_SETCUEBANNER, 0, string.Empty);
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
