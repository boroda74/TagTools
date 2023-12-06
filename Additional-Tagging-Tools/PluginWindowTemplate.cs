using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;



namespace MusicBeePlugin
{
    public partial class PluginWindowTemplate : Form
    {
        private bool modal;
        private bool fixedSize = false;
        private FormWindowState windowState;
        private int left;
        private int top;
        private int height;
        private int width;

        protected bool initialized = false;

        //ALL controls of the form
        protected List<Control> allControls = new List<Control> ();
        protected List<Button> nonDefaultableButtons = new List<Button> ();

        protected bool ignoreSizePositionChanges = true;

        protected static bool UseSkinColors = false;

        //Skin button labels for rendering disabled buttons
        protected Dictionary<Button, string> buttonLabels = new Dictionary<Button, string>();

        protected float dpiScaleFactor = 1;


        private delegate void StopButtonClicked(PluginWindowTemplate clickedForm, PrepareOperation prepareOperation);
        private delegate void TaskStarted();
        public delegate bool PrepareOperation();

        private StopButtonClicked stopButtonClicked;
        private TaskStarted taskStarted;

        protected static Plugin TagToolsPlugin;

        private Thread backgroundThread = null;
        protected ThreadStart job;

        public bool backgroundTaskIsScheduled = false;
        public bool backgroundTaskIsNativeMB = false;
        public bool backgroundTaskIsCanceled = false;
        public bool previewIsStopped = false;

        public bool previewIsGenerated = false;

        protected Button clickedButton;
        protected Button previewButton;
        protected Button closeButton;

        protected string okButtonText;
        protected string previewButtonText;
        protected string closeButtonText;

        protected bool ignoreLabelEnabledChanged = false;

        public bool backgroundTaskIsWorking()
        {
            lock (taskStarted)
            {
                if (backgroundTaskIsScheduled && !backgroundTaskIsCanceled)
                    return true;
            }

            return false;
        }

        public PluginWindowTemplate()
        {
        }

        public PluginWindowTemplate(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;
        }

        private void tabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabPage page = ((TabControl)sender).TabPages[e.Index];
            Rectangle paddedBounds = e.Bounds;
            int yOffset = (e.State == DrawItemState.Selected) ? -2 : 1;
            paddedBounds.Offset(1, yOffset);

            e.Graphics.FillRectangle(new SolidBrush(page.BackColor), e.Bounds);
            TextRenderer.DrawText(e.Graphics, page.Text, e.Font, paddedBounds, page.ForeColor);
        }

        public void button_EnabledChanged(object sender, System.EventArgs e)
        {
            Button button = (Button)sender;

            if (button.Enabled && button == this.AcceptButton)
                button_GotFocus(sender, e);
            else if (button.Focused)
                button_GotFocus(sender, e);
            else if (UseSkinColors)
                button_LostFocus(sender, e);
        }

        public void button_GotFocus(object sender, System.EventArgs e)
        {
            Button button = (Button)sender;

            Color borderColor = ButtonFocusedBorderColor;
            Color foreColor = ContrastColor;
            Color backColor = ControlHighlightColor;

            if (this.AcceptButton != button)
            {
                if (!nonDefaultableButtons.Contains(button))
                {
                    if (UseSkinColors)
                        ((Button)this.AcceptButton).FlatAppearance.BorderColor = ButtonBorderColor;

                    this.AcceptButton = button;//-----
                }
                else if (this.AcceptButton != null)
                {
                    ((Button)this.AcceptButton).Focus();

                    borderColor = ButtonBorderColor;
                    foreColor = ButtonForeColor;
                    backColor = ButtonBackColor;
                }
            }


            if (UseSkinColors)
            {
                if (button.Enabled)
                {
                    button.FlatAppearance.BorderColor = borderColor;
                    button.ForeColor = foreColor;
                    button.BackColor = backColor;
                }
                else
                {
                    button.FlatAppearance.BorderColor = ButtonBorderColor;
                    button.ForeColor = ButtonDisabledForeColor;
                    button.BackColor = ButtonDisabledBackColor;
                }
            }
        }

        public void button_LostFocus(object sender, System.EventArgs e)
        {
            Button button = (Button)sender;

            if (button.Enabled)
            {
                button.FlatAppearance.BorderColor = ButtonBorderColor;
                button.ForeColor = ButtonForeColor;
                button.BackColor = ButtonBackColor;
            }
            else
            {
                button.FlatAppearance.BorderColor = ButtonBorderColor;
                button.ForeColor = ButtonDisabledForeColor;
                button.BackColor = ButtonDisabledBackColor;
            }
        }

        public void button_Paint(object sender, PaintEventArgs e)
        {
            Button button = (Button)sender;

            string text = buttonLabels[button];

            // Set flags to center text on the button.
            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;   // center the text
            
            // Render the text onto the button.
            TextRenderer.DrawText(e.Graphics, text, Font, e.ClipRectangle, button.ForeColor, flags);
        }

        public void button_TextChanged(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string text = button.Text;

            if (!string.IsNullOrEmpty(text))
            {
                buttonLabels.AddReplace(button, text);

                if (UseSkinColors)
                    button.Text = string.Empty;

                button.Refresh();
            }
        }

        public static int GetPictureBoxY(Control control, Control control2)
        {
            float control2MiddleY = control2.Location.Y + control2.Height / 2f;
            return (int)(control2MiddleY - control.Height / 2f);
        }

        public int getGenericLeftRightAnchoredControlWidth(Control control)
        {
            return (int)(GetControlSize(control).Width * 1.0f / (4f * dpiScaleFactor - 3f)); //*****
        }

        public static void SetControlTagReferredNameMarksAndCustomRemark(Control control, string tagValue, bool scaledMovedX = false, bool scaledMovedY = false, 
            string control2Name = null, string remark = null)//---
        {
            (_, string oldControl2Name, bool scaledMovedOldX, bool scaledMovedOldY, string oldRemark) = GetControlTagReferredNameMarksAndCustomRemark(control);

            scaledMovedX |= scaledMovedOldX;
            scaledMovedY |= scaledMovedOldY;


            if (control2Name == null)
                control2Name = oldControl2Name;

            if (remark == null)
                remark = oldRemark;

            control.Tag = tagValue ?? string.Empty;

            if (!string.IsNullOrEmpty(control2Name))
                control.Tag += "#" + control2Name;

            if (scaledMovedX)
                control.Tag += "#scaled-moved-x";

            if (scaledMovedY)
                control.Tag += "#scaled-moved-y";

            if (!string.IsNullOrEmpty(remark))
                control.Tag += "@" + remark;
        }

        //Returns initial tag value, referred control name, moved-scaled marks and custom remark
        public static (string, string, bool, bool, string) GetControlTagReferredNameMarksAndCustomRemark(Control control)//---
        {
            if (control.Tag == null || !(control.Tag is string))
                return (string.Empty, string.Empty, false, false, string.Empty);

            string controlTag = control.Tag as string;
            bool scaledMovedX = controlTag.Contains("#scaled-moved-x");
            bool scaledMovedY = controlTag.Contains("#scaled-moved-y");
            controlTag = controlTag.Replace("#scaled-moved-x", string.Empty);
            controlTag = controlTag.Replace("#scaled-moved-y", string.Empty);

            string tagValue = controlTag;
            string remark = string.Empty;
            string control2Name = controlTag;

            //Optional tag value by itself
            if (controlTag.Contains("#"))
            {
                control2Name = Regex.Replace(controlTag, "^(.*?)#(.*)", "$2");
                tagValue = controlTag.Replace("#" + control2Name, string.Empty);
            }

            //Optional custom remark
            if (control2Name.Contains("@"))
            {
                remark = Regex.Replace(control2Name, "^(.*?)@(.*)", "$2");
                control2Name = control2Name.Replace("@" + remark, string.Empty);
            }

            if (tagValue.Contains("@"))
            {
                remark = Regex.Replace(tagValue, "^(.*?)@(.*)", "$2");
                tagValue = tagValue.Replace("@" + remark, string.Empty);
            }

            return (tagValue, control2Name, scaledMovedX, scaledMovedY, remark);
        }

        public static (Control, int) GetReferredControl(Control control) //int - parent level if control2 is ancestor, otherwise 0
        {
            (_, string control2Name, _, _, _) = GetControlTagReferredNameMarksAndCustomRemark(control);
            
            if (string.IsNullOrEmpty(control2Name))
                return (null, 0);


            int level = 0;
            Control control2 = control.Parent.Controls[control2Name];

            Control parent = control.Parent;
            while (control2 == null && parent != null)
            {
                level++;

                if (parent.Name == control2Name)
                    control2 = parent;
                else
                    parent = parent.Parent;
            }

            if (control2 == null)
                level = 0;


            return (control2, level);
        }

        public static Control GetReferringControl(Control control)
        {
            Control referringControl = null;
            foreach (Control refControl in control.Parent.Controls)
            {
                (Control control2, _) = GetReferredControl(refControl);

                if (control2 == control && referringControl == null)
                    referringControl = refControl;
                else if (control2 == control)
                    throw new Exception("More than one referring control for control: " + control.Name + "!");

            }

            return referringControl;
        }

        //int - parent level if control2 is ancestor, otherwise 0 //---
        public static (Control, int, bool, bool) GetReferredControlAndControlLevelAndMarks(Control control)
        {
            (_, _, bool scaledMovedX, bool scaledMovedY, _) = GetControlTagReferredNameMarksAndCustomRemark(control);
            (Control control2, int level) = GetReferredControl(control);

            return (control2, level, scaledMovedX, scaledMovedY);
        }

        public Size GetControlSize(Control control)
        {
            if (control.GetType().IsSubclassOf(typeof(Form)) || control.GetType() == typeof(Form))
                return ((Form)control).ClientSize;
            else //if (control.GetType().IsSubclassOf(typeof(Button)) || control.GetType() == typeof(Button))//******
                return control.Size;
            //else
            //    return new Size(control.Size.Width - (int)((control.Margin.Left + control.Margin.Right) * dpiScaleFactor), control.Size.Height);
        }

        public int getRightAnchoredControlX(Control control, int controlNewWidth, Control control2, int level, float offset)
        {
            if (level == 0)
                return (int)(control2.Location.X - controlNewWidth - offset * dpiScaleFactor);
            else
                return (int)(GetControlSize(control2).Width - controlNewWidth - offset * dpiScaleFactor);
        }

        public int getRightAnchoredControlWidth(Control control, Control control2, int level, float offset)
        {
            if (level == 0)
                return (int)(control2.Location.X - control.Location.X - offset * dpiScaleFactor);
            else
                return (int)(GetControlSize(control2).Width - control.Location.X - offset * dpiScaleFactor);
        }

        public void preMoveScaleControl(Control control)
        {
            if (control.GetType().IsSubclassOf(typeof(Form)) || control.GetType() == typeof(Form))
                return;

            (_, string control2Name, _, _, string remark) = GetControlTagReferredNameMarksAndCustomRemark(control);
            if (remark == "non-defaultable")
                nonDefaultableButtons.Add((Button)control);


            if (
                //control.GetType().IsSubclassOf(typeof(GroupBox)) || control.GetType() == typeof(GroupBox) ||
                //control.GetType().IsSubclassOf(typeof(Label)) || control.GetType() == typeof(Label) ||
                control.GetType().IsSubclassOf(typeof(Button)) || control.GetType() == typeof(Button) ||
                control.GetType().IsSubclassOf(typeof(PictureBox)) || control.GetType() == typeof(PictureBox)
            )
            {
                Control referringControl = GetReferringControl(control);

                bool setMarks = false;
                if (control2Name == string.Empty && referringControl == null)
                    setMarks = true;


                //Control:  NOT Button, or NOT top, left & right anchored Button/no referred control. It's NOT a button in table layout panel.
                if (!(control.GetType().IsSubclassOf(typeof(Button)) || control.GetType() == typeof(Button)) || 
                    ((control.Anchor & AnchorStyles.Top) == 0 || (control.Anchor & AnchorStyles.Left) == 0 || (control.Anchor & AnchorStyles.Right) == 0))
                {
                    int controlNewWidth = (int)(control.Width * dpiScaleFactor);
                    Control parentControl = control.Parent;

                    //SQUARE (NOT BUTTON)
                    if (control.Width == control.Height)
                    {
                        control.Width = controlNewWidth;
                        control.Height = controlNewWidth;
                    }
                    else if (parentControl.GetType().IsSubclassOf(typeof(GroupBox)) || parentControl.GetType() == typeof(GroupBox) 
                            && (control.GetType().IsSubclassOf(typeof(Button)) || control.GetType() == typeof(Button) ||
                                control.GetType().IsSubclassOf(typeof(Label)) || control.GetType() == typeof(Label))
                    )
                    {
                        //Setting X position. Width is set below.
                        if ((control.Anchor & AnchorStyles.Left) != 0 && (control.Anchor & AnchorStyles.Right) == 0) //Left anchor
                        {
                            control.Location = new Point((int)(control.Location.X * dpiScaleFactor), control.Location.Y);
                        }
                        else if ((control.Anchor & AnchorStyles.Left) == 0 && (control.Anchor & AnchorStyles.Right) != 0) //Right anchor
                        {
                            float rightNewOffset = (parentControl.Width - control.Location.X - control.Width) * dpiScaleFactor;
                            float controlNewX = parentControl.Width - controlNewWidth - rightNewOffset;

                            control.Location = new Point((int)controlNewX, control.Location.Y);
                        }
                        else //Let's ignore left & right anchored controls for now
                        {
                            //...
                        }

                        control.Width = controlNewWidth;

                        if (setMarks)
                            SetControlTagReferredNameMarksAndCustomRemark(control, null, true, false);
                    }
                }


                //Lets' set Button's vertical size/position for generic button
                if (control.GetType().IsSubclassOf(typeof(Button)) || control.GetType() == typeof(Button))
                {
                    //Control (Button) NOT top, left & right anchor/no referred control. It's NOT a button in table layout panel.
                    //if ((control.Anchor & AnchorStyles.Top) == 0 || (control.Anchor & AnchorStyles.Left) == 0 || (control.Anchor & AnchorStyles.Right) == 0)
                    {
                        //NOT SQUARE (square button is a special case)
                        if (control.Width != control.Height)
                        {
                            int controlNewHeight = (int)(control.Height * dpiScaleFactor);

                            //Top anchor
                            if ((control.Anchor & AnchorStyles.Top) != 0 && (control.Anchor & AnchorStyles.Bottom) == 0)
                            {
                                control.Height = controlNewHeight;
                            }
                            //Bottom anchor
                            else if ((control.Anchor & AnchorStyles.Top) == 0 && (control.Anchor & AnchorStyles.Bottom) != 0)
                            {
                                int controlNewY = control.Location.Y + control.Height - controlNewHeight;

                                control.Location = new Point(control.Location.X, controlNewY);
                                control.Height = controlNewHeight;
                            }
                            //Top & bottom anchor/no vertical anchor
                            else
                            {
                                float controlMiddleY = (control.Location.Y + control.Height / 2f);
                                int controlNewY = (int)(controlMiddleY - controlNewHeight / 2f);

                                control.Location = new Point(control.Location.X, controlNewY);
                                control.Height = controlNewHeight;
                            }


                            if (setMarks)
                                SetControlTagReferredNameMarksAndCustomRemark(control, null, false, true);
                        }
                    }
                }
            }
        }

        public int getAdjustedControlHeight(int controlHeight)
        {
            return (int)Math.Round(controlHeight * ((dpiScaleFactor - 1) * 0.34f + 1)); //*****
        }

        public void postMoveScaleControl(Control control, bool forceMoveScale = false)
        {
            if (control.GetType().IsSubclassOf(typeof(Form)) || control.GetType() == typeof(Form))
                return;

            int controlNewWidth = (int)(control.Width * dpiScaleFactor);
            int controlNewHeight = getAdjustedControlHeight(control.Height);


            (_, string control2Name, bool scaledMovedX, bool scaledMovedY, _) = GetControlTagReferredNameMarksAndCustomRemark(control);

            if (!forceMoveScale && (scaledMovedX || scaledMovedY))
                return;
            else if (string.IsNullOrEmpty(control2Name))
                return;

            Control parentControl = control.Parent;

            if (!(
                control.GetType().IsSubclassOf(typeof(ComboBox)) || control.GetType() == typeof(ComboBox) ||
                control.GetType().IsSubclassOf(typeof(TextBox)) || control.GetType() == typeof(TextBox) ||
                control.GetType().IsSubclassOf(typeof(Button)) || control.GetType() == typeof(Button)
            ))
            {
                return;
            }

            if (control.GetType().IsSubclassOf(typeof(TextBox)) || control.GetType() == typeof(TextBox))
            {
                if (((TextBox)control).Multiline)
                    return;
            }

            if (!control.GetType().IsSubclassOf(typeof(Button)) && control.GetType() != typeof(Button))
            {
                if ((control.Anchor & AnchorStyles.Left) != 0 && (control.Anchor & AnchorStyles.Right) != 0) //Not a Button, control left & right anchor/no referred control
                {
                    controlNewWidth = getGenericLeftRightAnchoredControlWidth(control);
                    controlNewHeight = control.Height;
                }
            }
            //Control (Button) NOT top, left & right anchor/no referred control. It's NOT a button in table layout panel. Let's reset Button's width & height.
            else if ((control.Anchor & AnchorStyles.Top) == 0 || (control.Anchor & AnchorStyles.Left) == 0 || (control.Anchor & AnchorStyles.Right) == 0)
            {
                //SQUARE
                if (control.Width == control.Height)
                    controlNewWidth = control.Width;

                controlNewHeight = control.Height;
            }


            //Setting X positions. Width is set below.
            if ((control.Anchor & AnchorStyles.Left) != 0 && (control.Anchor & AnchorStyles.Right) == 0 && controlNewWidth != control.Width) //Left anchor, width changed
            {
                control.Location = new Point((int)(control.Location.X * dpiScaleFactor), control.Location.Y);
            }
            else if ((control.Anchor & AnchorStyles.Left) == 0 && (control.Anchor & AnchorStyles.Right) != 0 && controlNewWidth != control.Width) //Right anchor, width changed
            {
                int rightNewOffset = (int)((GetControlSize(parentControl).Width - (control.Location.X + control.Width)) * dpiScaleFactor);
                int controlNewX = GetControlSize(parentControl).Width - controlNewWidth - rightNewOffset;

                control.Location = new Point(controlNewX, control.Location.Y);
            }


            //Setting Y positions. Height is set below.
            //Nothing at the moment...

            //Setting width & height.
            control.Width = controlNewWidth;
            control.Height = controlNewHeight;
        }

        public void moveScaleReferringDependentControls(Control control)
        {
            if (control.GetType().IsSubclassOf(typeof(Form)) || control.GetType() == typeof(Form))
                return;


            bool scaledMovedXRef = false;
            bool scaledMovedYRef = false;
            Control referringControl = GetReferringControl(control);
            if (referringControl != null)
                (_, _, scaledMovedXRef, scaledMovedYRef, _) = GetControlTagReferredNameMarksAndCustomRemark(referringControl);

            (Control control2, int level, bool scaledMovedX, bool scaledMovedY) = GetReferredControlAndControlLevelAndMarks(control);

            if (scaledMovedX && scaledMovedY)
                return;



            //control left anchor
            if (referringControl != null && (control.Anchor & AnchorStyles.Left) != 0 && (control.Anchor & AnchorStyles.Right) == 0)
                moveScaleReferringDependentControls(referringControl);
            //Manually horizontally aligned CheckBox
            else if (referringControl != null && scaledMovedXRef && !scaledMovedYRef && (referringControl.GetType().IsSubclassOf(typeof(CheckBox)) || referringControl.GetType() == typeof(CheckBox)))
                moveScaleReferringDependentControls(referringControl);
            //Manually horizontally aligned RadioButton
            else if (referringControl != null && scaledMovedXRef && !scaledMovedYRef && (referringControl.GetType().IsSubclassOf(typeof(RadioButton)) || referringControl.GetType() == typeof(RadioButton)))
                moveScaleReferringDependentControls(referringControl);


            if (control2 == null)
                return;


            (_, _, bool scaledMovedX2, bool scaledMovedY2, _) = GetControlTagReferredNameMarksAndCustomRemark(control2);
            (Control control3, int level2) = GetReferredControl(control2);


            //Manually horizontally aligned CheckBox. Let's proceed further...
            if (scaledMovedX && !scaledMovedY && (control.GetType().IsSubclassOf(typeof(CheckBox)) || control.GetType() == typeof(CheckBox)))
                ;
            //Manually horizontally aligned RadioButton. Let's proceed further...
            else if (scaledMovedX && !scaledMovedY && (control.GetType().IsSubclassOf(typeof(RadioButton)) || control.GetType() == typeof(RadioButton)))
                ;
            //control2 right anchor and control is NOT manually horizontally aligned CheckBox or RadioButton
            else if ((control2.Anchor & AnchorStyles.Left) == 0 && (control2.Anchor & AnchorStyles.Right) != 0)
                moveScaleReferringDependentControls(control2);


            const float genericOffsetX = 10;//******
            const float edgeOffsetX = 6f;
            const float buttonOffsetX = 8f;
            const float smallControlOffsetX = 8f;
            const float smallControlOffsetY = 0.8f;//*****


            if ((control.Anchor & AnchorStyles.Left) != 0 && (control.Anchor & AnchorStyles.Right) == 0) //control left anchor
            {
                if (control.GetType().IsSubclassOf(typeof(GroupBox)) || control.GetType() == typeof(GroupBox))
                {
                    if (!scaledMovedX)
                    {
                        float controlNewWidth = control.Width * dpiScaleFactor;

                        control.Width = (int)controlNewWidth;
                        scaledMovedX = true;
                    }
                }
                else if (level > 0)
                {
                    if (!scaledMovedX)
                    {
                        control.Location = new Point((int)(edgeOffsetX * dpiScaleFactor), control.Location.Y);
                        scaledMovedX = true;
                    }
                }
                else if (control.GetType().IsSubclassOf(typeof(CheckBox)) || control.GetType() == typeof(CheckBox) || 
                    control.GetType().IsSubclassOf(typeof(RadioButton)) || control.GetType() == typeof(RadioButton)
                )
                {
                    if (!scaledMovedY && (control2.GetType().IsSubclassOf(typeof(Label)) || control2.GetType() == typeof(Label)))
                    {
                        control.Location = new Point(control.Location.X, (int)(control2.Location.Y - smallControlOffsetY * dpiScaleFactor));
                        scaledMovedY = true;
                    }

                    if (!scaledMovedX2)
                    {
                        control2.Location = new Point((int)(control.Location.X + control.Width - smallControlOffsetX * dpiScaleFactor), control2.Location.Y);
                        scaledMovedX2 = true;
                    }
                }
                else if (control.GetType().IsSubclassOf(typeof(PictureBox)) || control.GetType() == typeof(PictureBox))
                {
                    int controlNewY = GetPictureBoxY(control, control2);

                    if (!scaledMovedY)
                    {
                        control.Location = new Point(control.Location.X, controlNewY);
                        scaledMovedY = true;
                    }

                    if (!scaledMovedX2)
                    {
                        control2.Location = new Point((int)(control.Location.X + control.Width + smallControlOffsetX * dpiScaleFactor), control2.Location.Y);//*****
                        scaledMovedX2 = true;
                    }
                }
                else if ((control2.Anchor & AnchorStyles.Left) != 0 && (control2.Anchor & AnchorStyles.Right) == 0) //control2 left anchor
                {
                    float offset = genericOffsetX;
                    if (control2.GetType().IsSubclassOf(typeof(Button)) || control2.GetType() == typeof(Button))
                    {
                        if (control2.Width == control2.Height)
                            offset = buttonOffsetX;
                    }


                    if (!scaledMovedX2)
                    {
                        control2.Location = new Point((int)(control.Location.X + control.Width + offset * dpiScaleFactor), control2.Location.Y);
                        scaledMovedX2 = true;
                    }
                }
                //control2 must NOT have right anchor. See below.
                else if ((control2.Anchor & AnchorStyles.Left) == 0 && (control2.Anchor & AnchorStyles.Right) != 0)
                {
                    throw new Exception("Invalid control2 anchoring! Expected anchor is not right.");
                }
                else //control2 left & right or no anchors
                {
                    if (!scaledMovedX2)
                    {
                        if (control3 == null) //Let's center relative parent
                        {
                            control2.Location = new Point((int)(control.Location.X + control.Width + genericOffsetX * dpiScaleFactor), control2.Location.Y);
                            control2.Width = getGenericLeftRightAnchoredControlWidth(control2);
                        }
                        else
                        {
                            control2.Location = new Point((int)(control.Location.X + control.Width + genericOffsetX * dpiScaleFactor), control2.Location.Y);
                            control2.Width = getRightAnchoredControlWidth(control2, control3, level2, genericOffsetX);
                        }

                        scaledMovedX2 = true;
                    }
                }
            }
            else if ((control.Anchor & AnchorStyles.Left) == 0 && (control.Anchor & AnchorStyles.Right) != 0) //control right anchor
            {
                if (control.GetType().IsSubclassOf(typeof(GroupBox)) || control.GetType() == typeof(GroupBox))
                {
                    if (!scaledMovedX)
                    {
                        float controlNewWidth = control.Width * dpiScaleFactor;
                        int rightOffset = GetControlSize(control2).Width - (control.Location.X + control.Width);
                        float controlNewX = GetControlSize(control2).Width - rightOffset - controlNewWidth;

                        control.Location = new Point((int)controlNewX, control.Location.Y);
                        control.Width = (int)controlNewWidth;
                        scaledMovedX = true;
                    }
                }
                else if (level > 0)
                {
                    if (!scaledMovedX)
                    {
                        control.Location = new Point(getRightAnchoredControlX(control, control.Width, control2, level, edgeOffsetX), control.Location.Y);
                        scaledMovedX = true;
                    }
                }
                else if (control.GetType().IsSubclassOf(typeof(CheckBox)) || control.GetType() == typeof(CheckBox) || 
                    control.GetType().IsSubclassOf(typeof(RadioButton)) || control.GetType() == typeof(RadioButton)
                )
                {
                    if (scaledMovedX && !scaledMovedY)
                    {
                        if (control2.GetType().IsSubclassOf(typeof(Label)) || control2.GetType() == typeof(Label))
                        {
                            control.Location = new Point(control.Location.X, (int)(control2.Location.Y - smallControlOffsetY * dpiScaleFactor));
                            scaledMovedY = true;
                        }

                        if (!scaledMovedX2)
                        {
                            control2.Location = new Point((int)(control.Location.X + control.Width - smallControlOffsetX * dpiScaleFactor), control2.Location.Y);
                            scaledMovedX2 = true;
                        }
                    }
                    else
                    {
                        if (!scaledMovedX && !scaledMovedY)
                        {
                            if (!scaledMovedY && (control2.GetType().IsSubclassOf(typeof(Label)) || control2.GetType() == typeof(Label)))
                            {
                                control.Location = new Point(getRightAnchoredControlX(control, control.Width, control2, level, -smallControlOffsetX), (int)(control2.Location.Y - smallControlOffsetY * dpiScaleFactor));
                                scaledMovedY = true;
                            }
                            else
                            {
                                control.Location = new Point(getRightAnchoredControlX(control, control.Width, control2, level, -smallControlOffsetX), control.Location.Y);
                            }

                            scaledMovedX = true;
                        }
                    }
                }
                else if (control.GetType().IsSubclassOf(typeof(PictureBox)) || control.GetType() == typeof(PictureBox)) //control (PictureBox) must have right anchor
                {
                    int controlNewY = GetPictureBoxY(control, control2);

                    if (!scaledMovedX && !scaledMovedY)
                    {
                        control.Location = new Point(getRightAnchoredControlX(control, control.Width, control2, level, smallControlOffsetX), controlNewY);
                        scaledMovedX = true;
                        scaledMovedY = true;
                    }
                }
                else if ((control2.Anchor & AnchorStyles.Left) == 0 && (control2.Anchor & AnchorStyles.Right) != 0) //control2 right anchor
                {
                    float offset = genericOffsetX;
                    if (!control.GetType().IsSubclassOf(typeof(Button)) && control.GetType() != typeof(Button))
                    {
                        if (control2.GetType().IsSubclassOf(typeof(Button)) || control2.GetType() == typeof(Button))
                            offset = buttonOffsetX;
                    }


                    if (control3 != null && level2 > 0)
                    {
                        if (!scaledMovedX2)
                        {
                            if (level2 == 0)
                                control2.Location = new Point(getRightAnchoredControlX(control2, control2.Width, control3, level2, genericOffsetX), control2.Location.Y);
                            else
                                control2.Location = new Point(getRightAnchoredControlX(control2, control2.Width, control3, level2, edgeOffsetX), control2.Location.Y);

                            scaledMovedX2 = true;
                        }
                    }

                    if (!scaledMovedX)
                    {
                        control.Location = new Point(getRightAnchoredControlX(control, control.Width, control2, level, offset), control.Location.Y);
                        scaledMovedX = true;
                    }
                }
                //control2 must NOT have left anchor. See below.
                else if ((control2.Anchor & AnchorStyles.Left) != 0 && (control2.Anchor & AnchorStyles.Right) == 0)
                {
                    throw new Exception("Invalid control2 anchoring! Expected anchor is not left.");
                }
                else //control2 left & right or no anchors
                {
                    if (!scaledMovedX2)
                    {
                        if (control3 == null) //Let's center relative parent
                        {
                            int control2NewWidth = getGenericLeftRightAnchoredControlWidth(control2);

                            control2.Location = new Point((int)(control.Location.X + control.Width + genericOffsetX * dpiScaleFactor), control2.Location.Y);
                            control2.Width = control2NewWidth;
                        }
                        else
                        {
                            int control2NewWidth = getRightAnchoredControlX(control2, control2.Width, control3, level2, edgeOffsetX) - control2.Location.X;

                            control2.Location = new Point((int)(control.Location.X + control.Width + genericOffsetX * dpiScaleFactor), control2.Location.Y);
                            control2.Width = control2NewWidth;//*****
                        }

                        scaledMovedX2 = true;
                    }
                }
            }
            else if ((control.Anchor & AnchorStyles.Left) != 0 && (control.Anchor & AnchorStyles.Right) != 0)  //control left & right anchor
            {
                if (control.GetType().IsSubclassOf(typeof(Panel)) || control.GetType() == typeof(Panel))
                {
                    if (!scaledMovedX)
                    {
                        float controlNewX = control.Location.X * dpiScaleFactor;
                        float controlNewWidth = GetControlSize(control2).Width - controlNewX;

                        control.Location = new Point((int)controlNewX, control.Location.Y);
                        control.Width = (int)controlNewWidth;
                        scaledMovedX = true;
                    }
                }
                else if (level > 0)
                {
                    if (!scaledMovedX)
                    {
                        float controlMiddleX = control.Location.X + control.Width / 2f;
                        float controlNewWidth = control.Width * dpiScaleFactor;
                        float controlNewX = controlMiddleX - controlNewWidth / 2f;

                        control.Location = new Point((int)controlNewX, control.Location.Y);
                        control.Width = (int)controlNewWidth;
                        scaledMovedX = true;
                    }
                }
                //control2 must have right anchor. See below.
                else if ((control2.Anchor & AnchorStyles.Left) != 0 || (control2.Anchor & AnchorStyles.Right) == 0)
                {
                    throw new Exception("Invalid control2 anchoring! Expected anchor is right.");
                }
                else //control2 right anchor
                {
                    float offset = genericOffsetX;
                    if (control2.GetType().IsSubclassOf(typeof(Button)) || control2.GetType() == typeof(Button))
                    {
                        if (control2.Width == control2.Height)
                            offset = buttonOffsetX;
                    }


                    if (!scaledMovedX2)
                    {
                        moveScaleReferringDependentControls(control2);
                    }

                    control.Width = (int)(control2.Location.X - control.Location.X - offset * dpiScaleFactor);
                    scaledMovedX = true; 
                }
            }


            SetControlTagReferredNameMarksAndCustomRemark(control, null, true, true);
            SetControlTagReferredNameMarksAndCustomRemark(control2, null, scaledMovedX2, scaledMovedY2);//******

            if (scaledMovedX2 || scaledMovedY2)//***** see above
                moveScaleReferringDependentControls(control2);
        }

        public void skinControl(Control control)
        {
            //if (control.GetType().IsSubclassOf(typeof(TabControl)) || control.GetType() == typeof(TabControl))
            //{
            //    TabControl tabControl = (TabControl)control;
            //    tabControl.DrawItem += form.TabControl_DrawItem;
            //
            //    return;
            //}

            if (control.GetType().IsSubclassOf(typeof(SplitContainer)) || control.GetType() == typeof(SplitContainer))
            {
                SplitContainer splitContainer = (SplitContainer) control;

                splitContainer.Panel1.BackColor = FormBackColor;
                splitContainer.Panel1.ForeColor = AccentColor;

                splitContainer.Panel2.BackColor = FormBackColor;
                splitContainer.Panel2.ForeColor = AccentColor;

                splitContainer.BackColor = SplitterColor;

                return;
            }

            if (control.GetType().IsSubclassOf(typeof(Button)) || control.GetType() == typeof(Button))
            {
                Button button = (Button)control;

                buttonLabels.AddReplace(button, button.Text);


                button.TextChanged += button_TextChanged;
                button.GotFocus += button_GotFocus;


                if (!UseSkinColors)
                {
                    button.FlatStyle = FlatStyle.Standard;
                }
                else
                {
                    button.ForeColor = ButtonForeColor;
                    button.BackColor = ButtonBackColor;

                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 1;
                    button.FlatAppearance.BorderColor = ButtonBorderColor;

                    button.EnabledChanged += button_EnabledChanged;
                    button.Paint += button_Paint;
                    button.LostFocus += button_LostFocus;

                    button.Text = string.Empty;
                }

                return;
            }


            //SplitContainer and (disabled) Button above must be skinned in any case (even if using system colors)
            if (!UseSkinColors)
                return;


            if (control.GetType().IsSubclassOf(typeof(TextBox)) || control.GetType() == typeof(TextBox))
            {
                control.BackColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentBackground));
                control.ForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));

                ((TextBox)control).BorderStyle = BorderStyle.FixedSingle;
            }
            else if (control.GetType().IsSubclassOf(typeof(ComboBox)) || control.GetType() == typeof(ComboBox))
            {
                control.BackColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentBackground));
                control.ForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));

                ((ComboBox)control).FlatStyle = FlatStyle.Flat;
            }
            else if (control.GetType().IsSubclassOf(typeof(ListBox)) || control.GetType() == typeof(ListBox))
            {
                control.BackColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentBackground));
                control.ForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));
                
                ((ListBox)control).BorderStyle = BorderStyle.Fixed3D;
            }
            else if (control.GetType().IsSubclassOf(typeof(Label)) || control.GetType() == typeof(Label))
            {
                control.BackColor = FormBackColor;

                if (control.ForeColor == SystemColors.ControlText)
                {
                    control.ForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanelLabel, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));
                }
                else
                {
                    Color stdColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanelLabel, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));
                    control.ForeColor = GetHighlightColor(control.ForeColor, stdColor, FormBackColor, 0.80f);
                }
            }
            else if (control.GetType().IsSubclassOf(typeof(GroupBox)) || control.GetType() == typeof(GroupBox))
            {
                control.BackColor = FormBackColor;

                if (control.ForeColor == SystemColors.ControlText)
                {
                    control.ForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanelLabel, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));
                }
                else
                {
                    Color stdColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanelLabel, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));
                    control.ForeColor = GetHighlightColor(control.ForeColor, stdColor, FormBackColor, 0.80f);
                }

                ((GroupBox)control).FlatStyle = FlatStyle.System;
            }
            else if (control.GetType().IsSubclassOf(typeof(CheckBox)) || control.GetType() == typeof(CheckBox))
            {
                control.BackColor = FormBackColor;
                control.ForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanel, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));

                ((CheckBox)control).FlatStyle = FlatStyle.System;
            }
            else if (control.GetType().IsSubclassOf(typeof(RadioButton)) || control.GetType() == typeof(RadioButton))
            {
                control.BackColor = FormBackColor;
                control.ForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanelLabel, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));

                ((RadioButton)control).FlatStyle = FlatStyle.System;
            }
            else if (control.GetType() == typeof(DataGridView))
            {
                control.BackColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentBackground));
                control.ForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));

                ((DataGridView)control).BackgroundColor = control.BackColor;
                ((DataGridView)control).DefaultCellStyle.BackColor = control.BackColor;
            }
            else
            {
                control.BackColor = FormBackColor;
                control.ForeColor = AccentColor;
            }

            return;
        }

        public void addAllChildrenControls(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                allControls.Add(control);
                addAllChildrenControls(control);
            }
        }

        public void skinMoveScaleAllControls()
        {
            if (UseSkinColors)
            {
                BackColor = FormBackColor;
                ForeColor = AccentColor;
            }

            addAllChildrenControls(this);

            for (int i = allControls.Count - 1; i >=0; i--)
                skinControl(allControls[i]);

            for (int i = allControls.Count - 1; i >= 0; i--)
                preMoveScaleControl(allControls[i]);

            for (int i = allControls.Count - 1; i >= 0; i--)
                moveScaleReferringDependentControls(allControls[i]);

            for (int i = allControls.Count - 1; i >= 0; i--)
                postMoveScaleControl(allControls[i]);
        }

        protected void setInitialFormMaximumMinimumSize()
        {
            int minimumWidth;

            if (Tag as string == "@fixed-min-width")
                minimumWidth = MinimumSize.Width;
            else if (Tag as string == "@small-min-width")
                minimumWidth = (int)(MinimumSize.Width * ((dpiScaleFactor - 1) * 0.5f + 1));
            else
                minimumWidth = (int)(MinimumSize.Width * dpiScaleFactor);


            if (!modal && !fixedSize)
            {
                MinimumSize = new Size(minimumWidth, (int)(MinimumSize.Height * dpiScaleFactor));
                MaximumSize = new Size((int)(MaximumSize.Width * dpiScaleFactor), (int)(MaximumSize.Height * dpiScaleFactor));
            }
            else if (!fixedSize)
            {
                int minimumHeight = (int)(MinimumSize.Height * ((dpiScaleFactor - 1) * 0.059f + 1));

                MinimumSize = new Size(minimumWidth, minimumHeight);
                MaximumSize = new Size((int)(MaximumSize.Width * dpiScaleFactor), (int)(MaximumSize.Height * dpiScaleFactor));
            }
        }

        protected void setFormMaximizedBounds()
        {
            if (modal && MaximumSize.Height != 0 && !fixedSize)
            {
                int maximizedHeight = Height;

                if (maximizedHeight > Screen.FromControl(this).WorkingArea.Height)
                    maximizedHeight = Screen.FromControl(this).WorkingArea.Height;

                MaximizedBounds = new Rectangle((int)(Screen.FromControl(this).WorkingArea.Left), 0, 
                    (int)(Screen.FromControl(this).WorkingArea.Width), maximizedHeight);
            }
        }

        public static void Display(PluginWindowTemplate newForm, bool modalForm = false)
        {
            lock (OpenedForms)
            {
                foreach (PluginWindowTemplate form in OpenedForms)
                {
                    if (form.GetType() == newForm.GetType())
                    {
                        if (form != newForm)
                            newForm.Dispose();

                        if (form.Visible && form.WindowState != FormWindowState.Minimized)
                        {
                            form.Activate();
                        }
                        else
                        {
                            form.Left = form.left;
                            form.Top = form.top;
                            form.Width = form.width;
                            form.Height = form.height;
                            form.WindowState = form.windowState;

                            if (form.modal)
                                form.ShowDialog();
                            else
                                form.Show();
                        }

                        return;
                    }
                }

                
                OpenedForms.Add(newForm);

                if (modalForm)
                    newForm.ShowDialog();
                else
                    newForm.Show();
            }
        }

        protected virtual void initializeForm()
        {
            //Implemented in derived classes... 
        }

        protected void primaryInitialization()
        {
            if (initialized)
            {
                tryShowForm();
                return;
            }


            fixedSize = (FormBorderStyle == FormBorderStyle.FixedDialog) || (FormBorderStyle == FormBorderStyle.FixedSingle) ? true : false;

            if (DeviceDpi != 96)
                dpiScaleFactor = DeviceDpi / 96f;

            UseSkinColors = SavedSettings.useSkinColors;

            ignoreSizePositionChanges = true;


            if (MbForm.IsDisposed)
                MbForm = (Form)FromHandle(MbApiInterface.MB_GetWindowHandle());

            MbForm.AddOwnedForm(this);

            clickedButton = EmptyButton;

            lock (OpenedForms)
            {
                if (NumberOfNativeMbBackgroundTasks > 0)
                    disableQueryingButtons();
                else
                    enableQueryingButtons();
            }

            stopButtonClicked = stopButtonClickedMethod;
            taskStarted = taskStartedMethod;

            TagToolsPlugin.fillTagNames();


            loadWindowSizesPositions();

            if (width != 0 && height != 0)
            {
                width = (int)(width * dpiScaleFactor);

                if (!modal && !fixedSize)
                    height = (int)(height * dpiScaleFactor);

                setInitialFormMaximumMinimumSize();

                Width = width;
                Height = height;
                WindowState = windowState;
            }
            else if (!modal && !fixedSize)
            {
                width = (int)(Width * dpiScaleFactor);
                height = (int)(Height * dpiScaleFactor);

                setInitialFormMaximumMinimumSize();

                if (Tag as string == "@fixed-min-width")
                    width = MinimumSize.Width;

                Width = width;
                Height = height;
                WindowState = FormWindowState.Normal;
            }
            else if (!fixedSize) //Modal
            {
                setInitialFormMaximumMinimumSize();

                if (Tag as string == "@fixed-min-width")
                    Width = MinimumSize.Width;

                Height = MinimumSize.Height;
                WindowState = FormWindowState.Normal;

                width = Width;
                height = Height;

            }
            else //Fixed size
            {
                if (Tag as string != "@fixed-min-width")
                    Width = (int)(Width * dpiScaleFactor);

                Height = Height;
                WindowState = FormWindowState.Normal;

                width = Width;
                height = Height;
            }

            setFormMaximizedBounds();


            left = (int)(left * dpiScaleFactor);
            top = (int)(top * dpiScaleFactor);


            Left = left;
            Top = top;

            skinMoveScaleAllControls();


            ignoreSizePositionChanges = false;
            initialized = true;


            tryShowForm();
        }

        protected void tryShowForm()
        {
            if (modal)
                base.ShowDialog();
            else
                base.Show();
        }

        public new void Show()
        {
            modal = false;
            primaryInitialization();
        }

        public new void ShowDialog()
        {
            modal = true;
            primaryInitialization();
        }

        private void PluginWindowTemplate_Move(object sender, EventArgs e)
        {
            if (ignoreSizePositionChanges)
                return;

            left = Left;
            top = Top;

            Refresh();
        }

        private void PluginWindowTemplate_Resize(object sender, EventArgs e)
        {
            if (ignoreSizePositionChanges)
                return;


            if (WindowState == FormWindowState.Minimized)
            {
                left = RestoreBounds.Left;
                top = RestoreBounds.Top;
                width = RestoreBounds.Width;
                height = RestoreBounds.Height;

                if (!SavedSettings.minimizePluginWindows)
                    Hide();
            }
            else
            {
                windowState = WindowState;

                if (WindowState == FormWindowState.Maximized)
                {
                    left = RestoreBounds.Left;
                    top = RestoreBounds.Top;
                    width = RestoreBounds.Width;
                    height = RestoreBounds.Height;
                }
                else
                {
                    setFormMaximizedBounds();

                    left = Left;
                    top = Top;
                    width = Width;
                    height = Height;
                }

                Refresh();
            }
        }

        private void PluginWindowTemplate_VisibleChanged(object sender, EventArgs e)
        {
            if (ignoreSizePositionChanges)
                return;

            if (Visible && width != 0)
            {
                WindowState = windowState;
                Left = left;
                Top = top;
                Width = width;
                Height = height;

                //windowWidth = 0;
            }
        }

        private void PluginWindowTemplate_Load(object sender, EventArgs e)
        {
            initializeForm();
        }

        private void PluginWindowTemplate_Shown(object sender, EventArgs e)
        {
            //Nothing at the moment...
        }

        private void PluginWindowTemplate_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundTaskIsScheduled)
            {
                e.Cancel = true;
                Hide();
            }
            else
            {
                saveWindowSizesPositions();
            }
        }

        private void PluginWindowTemplate_FormClosed(object sender, FormClosedEventArgs e)
        {
            lock (OpenedForms)
            {
                OpenedForms.Remove(this);
                FormsThemedBitmapsRelease(this);
            }
        }

        protected WindowSettingsType findCreateSavedWindowSettings(bool createIfAbsent)
        {
            string fullName = GetType().FullName;
            WindowSettingsType currentWindowSettings = null;

            foreach (var windowSettings in SavedSettings.windowsSettings)
            {
                if (windowSettings.className == fullName)
                {
                    currentWindowSettings = windowSettings;
                    break;
                }
            }

            if (currentWindowSettings == null && createIfAbsent)
            {
                currentWindowSettings = new WindowSettingsType
                {
                    className = fullName
                };

                SavedSettings.windowsSettings.Add(currentWindowSettings);
            }

            return currentWindowSettings;
        }

        protected void loadWindowSizesPositions()
        {
            var windowSettings = findCreateSavedWindowSettings(false);

            if (windowSettings != null)
            {
                left = windowSettings.x;
                top = windowSettings.y;

                width = windowSettings.w;
                height = windowSettings.h;

                if (windowSettings.max)
                    windowState = FormWindowState.Maximized;
                else
                    windowState = FormWindowState.Normal;
            }
        }

        protected (int, int, int, int, int, int, int) loadWindowLayout()
        {
            int column1Width = 0;
            int column2Width = 0;
            int column3Width = 0;
            int splitterDistance = 0;
            int table2column1Width = 0;
            int table2column2Width = 0;
            int table2column3Width = 0;

            var windowSettings = findCreateSavedWindowSettings(false);

            if (windowSettings != null)
            {
                column1Width = windowSettings.column1Width;
                column2Width = windowSettings.column2Width;
                column3Width = windowSettings.column3Width;

                splitterDistance = windowSettings.splitterDistance;

                table2column1Width = windowSettings.table2column1Width;
                table2column2Width = windowSettings.table2column2Width;
                table2column3Width = windowSettings.table2column3Width;
            }

            return (column1Width, column2Width, column3Width,
                splitterDistance,
                table2column1Width, table2column2Width, table2column3Width);
        }

        protected void saveWindowSizesPositions()
        {
            var currentWindowSettings = findCreateSavedWindowSettings(true);


            if(WindowState == FormWindowState.Normal)
            {
                width = Width;
                height = Height;
                left = Left;
                top = Top;
                windowState = FormWindowState.Normal;
            }


            int width2 = (int)(width / dpiScaleFactor);
            int height2;

            if (fixedSize)
                currentWindowSettings.h = height2 = 0;
            else if (!modal)
                height2 = (int)(height / dpiScaleFactor);
            else
                height2 = height;

            if (width2 == 0)
            {
                width2 = currentWindowSettings.w;
            }
            else if (100d * Math.Abs(width2 - currentWindowSettings.w) / width2 < 0.5d) // 0.5%
            {
                width2 = currentWindowSettings.w;
            }

            if (height2 == 0)
            {
                height2 = currentWindowSettings.h;
            }
            else if (100d * Math.Abs(height2 - currentWindowSettings.h) / height2 < 0.5d) // 0.5%
            {
                height2 = currentWindowSettings.h;
            }


            int left2 = (int)(left / dpiScaleFactor);
            if (Math.Abs(left2 - currentWindowSettings.x) <= 1) // 1px
            {
                left2 = currentWindowSettings.x;
            }

            int top2 = (int)(top / dpiScaleFactor);
            if (Math.Abs(top2 - currentWindowSettings.y) <= 1) // 1px
            {
                top2 = currentWindowSettings.y;
            }



            currentWindowSettings.x = left2;
            currentWindowSettings.y = top2;
            currentWindowSettings.w = width2;
            currentWindowSettings.h = height2;

            if (!Visible || WindowState == FormWindowState.Minimized)
                currentWindowSettings.max = (windowState == FormWindowState.Maximized ? true : false);
            else if (windowState == FormWindowState.Maximized)
                currentWindowSettings.max = true;
            else
                currentWindowSettings.max = false;
        }

        protected void saveWindowLayout(int column1Width = 0, int column2Width = 0, int column3Width = 0, int splitterDistance = 0, int table2column1Width = 0, int table2column2Width = 0, int table2column3Width = 0)
        {
            var currentWindowSettings = findCreateSavedWindowSettings(true);

            if (column1Width != 0)
            {
                int column1Width2 = (int)(column1Width / dpiScaleFactor);
                if (100f * Math.Abs(column1Width2 - currentWindowSettings.column1Width) / column1Width2 < 0.5f)
                    column1Width2 = currentWindowSettings.column1Width;

                currentWindowSettings.column1Width = column1Width2;


                int column2Width2 = (int)(column2Width / dpiScaleFactor);
                if (100f * Math.Abs(column2Width2 - currentWindowSettings.column1Width) / column2Width2 < 0.5f)
                    column2Width2 = currentWindowSettings.column2Width;

                currentWindowSettings.column2Width = column2Width2;


                int column3Width2 = (int)(column3Width / dpiScaleFactor);
                if (100f * Math.Abs(column3Width2 - currentWindowSettings.column3Width) / column3Width2 < 0.5f)
                    column3Width2 = currentWindowSettings.column3Width;

                currentWindowSettings.column3Width = column3Width2;
            }

            if (table2column1Width != 0)
            {
                int table2column1Width2 = (int)(table2column1Width / dpiScaleFactor);
                if (100f * Math.Abs(table2column1Width2 - currentWindowSettings.table2column1Width) / table2column1Width2 < 0.5f)
                    table2column1Width2 = currentWindowSettings.table2column1Width;

                currentWindowSettings.table2column1Width = table2column1Width2;


                int table2column2Width2 = (int)(table2column2Width / dpiScaleFactor);
                if (100f * Math.Abs(table2column2Width2 - currentWindowSettings.table2column1Width) / table2column2Width2 < 0.5f)
                    table2column2Width2 = currentWindowSettings.table2column2Width;

                currentWindowSettings.table2column2Width = table2column2Width2;


                int table2column3Width2 = (int)(table2column3Width / dpiScaleFactor);
                if (100f * Math.Abs(table2column3Width2 - currentWindowSettings.table2column3Width) / table2column3Width2 < 0.5f)
                    table2column3Width2 = currentWindowSettings.table2column3Width;

                currentWindowSettings.table2column3Width = table2column3Width2;
            }

            if (splitterDistance != 0)
            {
                int splitterDistance2 = (int)(splitterDistance / dpiScaleFactor);
                if (100f * Math.Abs(splitterDistance2 - currentWindowSettings.splitterDistance) / splitterDistance2 < 0.5f)
                    splitterDistance2 = currentWindowSettings.splitterDistance;

                currentWindowSettings.splitterDistance = splitterDistance2;
            }
        }

        private void serializedOperation()
        {
            bool taskWasStarted = false;

            InitializeSbText();

            try
            {
                if (backgroundTaskIsCanceled)
                {
                    if (DisablePlaySoundOnce)
                        DisablePlaySoundOnce = false;
                    else if (SavedSettings.playCanceledSound)
                        System.Media.SystemSounds.Hand.Play();
                }
                else
                {
                    if (DisablePlaySoundOnce)
                        DisablePlaySoundOnce = false;
                    else if (SavedSettings.playStartedSound)
                        System.Media.SystemSounds.Exclamation.Play();

                    backgroundThread = Thread.CurrentThread;
                    backgroundThread.Priority = ThreadPriority.BelowNormal;


                    if (clickedButton != EmptyButton)
                        Invoke(taskStarted);

                    taskWasStarted = true;
                    job();
                }
            }
            catch (ThreadAbortException)
            {
                backgroundTaskIsCanceled = true;
            }
            finally
            {
                lock (taskStarted)
                {
                    if (DisablePlaySoundOnce)
                        DisablePlaySoundOnce = false;
                    else if (!SavedSettings.dontPlayCompletedSound)
                        System.Media.SystemSounds.Asterisk.Play();

                    backgroundTaskIsScheduled = false;
                    backgroundTaskIsCanceled = false;

                    if (backgroundTaskIsNativeMB && taskWasStarted)
                    {
                        NumberOfNativeMbBackgroundTasks--;
                    }
                }

                if (clickedButton != EmptyButton)
                    Invoke(stopButtonClicked, new object[] { this, null });

                RefreshPanels(true);

                SetResultingSbText();

                if (!Visible)
                {
                    if (SavedSettings.closeShowHiddenWindows == 1)
                        Close();
                    else
                        Visible = true;
                }
            }
        }

        public void switchOperation(ThreadStart operation, Button clickedButtonParam, Button okButtonParam, Button previewButtonParam, Button closeButtonParam, bool backgroundTaskIsNativeMbParam, PrepareOperation prepareOperation)
        {
            if (backgroundTaskIsScheduled && !backgroundTaskIsWorking())
            {
                if (backgroundTaskIsCanceled)
                {
                    backgroundTaskIsCanceled = false;

                    lock (OpenedForms)
                    {
                        if (backgroundTaskIsNativeMB)
                        {
                            NumberOfNativeMbBackgroundTasks++;
                        }
                    }

                    queryingOrUpdatingButtonClick(this);
                }
                else
                {
                    backgroundTaskIsCanceled = true;

                    lock (OpenedForms)
                    {
                        if (backgroundTaskIsNativeMB)
                        {
                            NumberOfNativeMbBackgroundTasks--;
                        }
                    }

                    stopButtonClicked(this, prepareOperation);
                }
            }
            else if (backgroundTaskIsWorking())
            {
                stopButtonClicked(this, prepareOperation);
            }
            else
            {
                backgroundTaskIsNativeMB = backgroundTaskIsNativeMbParam;

                backgroundTaskIsCanceled = false;
                backgroundTaskIsScheduled = true;
                backgroundThread = null;

                clickedButton = clickedButtonParam;
                previewButton = previewButtonParam;
                closeButton = closeButtonParam;

                job = operation;

                if (backgroundTaskIsNativeMB)
                {
                    lock (OpenedForms)
                    {
                        NumberOfNativeMbBackgroundTasks++;
                    }

                    MbApiInterface.MB_CreateBackgroundTask(serializedOperation, this);
                }
                else
                {
                    Thread tempThread = new Thread(serializedOperation);
                    tempThread.Start();
                }

                previewButtonText = buttonLabels[previewButtonParam];
                okButtonText = buttonLabels[okButtonParam];
                closeButtonText = buttonLabels[closeButtonParam];

                queryingOrUpdatingButtonClick(this);
            }
        }

        protected string getBackgroundTasksWarning()
        {
            if (NumberOfNativeMbBackgroundTasks == 1)
                return CtlDirtyError1sf + NumberOfNativeMbBackgroundTasks + CtlDirtyError2sf;
            else if (NumberOfNativeMbBackgroundTasks > 1)
                return CtlDirtyError1mf + NumberOfNativeMbBackgroundTasks + CtlDirtyError2mf;
            else
                return string.Empty;
        }

        public virtual void enableDisablePreviewOptionControls(bool enable, bool dontChangeDisabled = false)
        {
            //Implemented in derived classes... 
        }

        public virtual void enableQueryingButtons()
        {
            //Implemented in derived classes... 
        }

        public virtual void disableQueryingButtons()
        {
            //Implemented in derived classes... 
        }

        public virtual void enableQueryingOrUpdatingButtons()
        {
            //Implemented in derived classes... 
        }

        public virtual void disableQueryingOrUpdatingButtons()
        {
            //Implemented in derived classes... 
        }

        public void queryingOrUpdatingButtonClick(PluginWindowTemplate clickedForm)
        {
            lock (OpenedForms)
            {
                foreach (PluginWindowTemplate form in OpenedForms)
                {
                    if (backgroundTaskIsNativeMB && form != clickedForm && !(form.backgroundTaskIsNativeMB && form.backgroundTaskIsScheduled && !form.backgroundTaskIsCanceled))
                    {
                        form.disableQueryingButtons();
                    }
                }

                if (backgroundTaskIsNativeMB) //Updating operation
                {
                    clickedForm.enableQueryingButtons();
                    clickedForm.disableQueryingOrUpdatingButtons();

                    okButtonText = buttonLabels[clickedButton];
                    clickedButton.Text = CancelButtonName;
                    clickedButton.Enable(true);

                    closeButtonText = buttonLabels[closeButton];
                    closeButton.Text = HideButtonName;
                    closeButton.Enable(true);
                }
                else //Querying operation
                {
                    clickedForm.disableQueryingOrUpdatingButtons();

                    clickedButton.Text = StopButtonName;
                    clickedButton.Enable(true);

                    closeButton.Enable(false);
                }

                enableDisablePreviewOptionControls(false);
            }
        }

        public void stopButtonClickedMethod(PluginWindowTemplate clickedForm, PrepareOperation prepareOperation)
        {
            lock (OpenedForms)
            {
                foreach (PluginWindowTemplate form in OpenedForms)
                {
                    if (backgroundTaskIsNativeMB && form != clickedForm) //Updating operation
                    {
                        if (NumberOfNativeMbBackgroundTasks > 0 && !(form.backgroundTaskIsNativeMB && form.backgroundTaskIsScheduled && !form.backgroundTaskIsCanceled))
                        {
                            form.disableQueryingButtons();
                        }
                        else
                        {
                            form.enableQueryingButtons();
                        }
                    }
                }

                clickedForm.disableQueryingButtons();
                clickedForm.enableQueryingOrUpdatingButtons();

                if (backgroundTaskIsNativeMB) //Updating operation
                {
                    clickedButton.Text = okButtonText;

                    if (previewIsGenerated)
                        previewButton.Text = ClearButtonName;
                    else
                        previewButton.Text = PreviewButtonName;

                    if (backgroundTaskIsScheduled)
                        previewButton.Enable(false);
                    else
                        closeButton.Text = closeButtonText;
                }
                else //Querying operation
                {
                    if (prepareOperation != null)
                    {
                        previewIsGenerated = false;
                        prepareOperation();
                        backgroundTaskIsCanceled = true;
                        clickedButton.Text = PreviewButtonName;
                        previewIsStopped = true;
                    }
                    else
                    {
                        backgroundTaskIsCanceled = true;

                        if (previewIsGenerated && !previewIsStopped)
                        {
                            clickedButton.Text = ClearButtonName;
                        }
                        else
                        {
                            previewIsStopped = false;
                            clickedButton.Text = previewButtonText;
                        }

                        closeButton.Enable(true);
                    }
                }

                enableDisablePreviewOptionControls(true);
            }
        }

        private void taskStartedMethod()
        {
            lock (OpenedForms)
            {
                if (backgroundTaskIsNativeMB) //Updating operation
                {
                    enableQueryingButtons();

                    clickedButton.Text = StopButtonName;
                }
            }
        }

        // clearPreview = 0: //--- ??? //****** What is this?
        protected void clickOnPreviewButton(DataGridView previewTable, PrepareOperation prepareOperation, ThreadStart operation, 
            Button clickedButtonParam, Button okButtonParam, Button closeButtonParam, int clearPreview = 0)
        {
            if ((previewTable.Rows.Count == 0 || backgroundTaskIsWorking() || clearPreview == 2) && clearPreview != 1)
            {
                if (prepareOperation())
                    switchOperation(operation, clickedButtonParam, okButtonParam, clickedButtonParam, closeButtonParam, false, prepareOperation);
            }
            else
            {
                previewIsGenerated = false;
                prepareOperation();
                clickedButtonParam.Text = PreviewButtonName;
                enableDisablePreviewOptionControls(true);
            }
        }
    }
}

