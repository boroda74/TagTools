using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
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

        public bool dontShowForm = false;
        protected bool hidden = false;//******
        protected bool initialized = false;

        //SKIN COLORING/CUSTOM FORM'S FONT/DPI SCALING
        public float dpiScaling = 1;

        public float hDpiFormScaling = 1;
        public float vDpiFormScaling = 1;

        public float hDpiFontScaling = 1;
        public float vDpiFontScaling = 1;


        //ALL controls of the form and their references (if any)
        protected List<Control> allControls = new List<Control> ();
        internal Dictionary<Control, Control> controlsReferences = new Dictionary<Control, Control>();
        protected Dictionary<Control, Control> controlsReferenced = new Dictionary<Control, Control>();

        protected struct SplitContainerScalingAttributes
        {
            public SplitContainer splitContainer;
            public int panel1MinSize;
            public int panel2MinSize;
            public int splitterDistance;
        }

        protected List<SplitContainerScalingAttributes> splitContainersScalingAttributes = new List<SplitContainerScalingAttributes>();

        protected List<Button> nonDefaultableButtons = new List<Button>();
        protected bool artificiallyFocusedAcceptButton = false;

        protected List<Control> pinnedToParentControls = new List<Control>();
        protected List<Control> leftRightAnchoredControls = new List<Control>();

        protected bool ignoreSizePositionChanges = true;

        protected static bool UseMusicBeeFontSkinColors = false;

        //Skin button labels for rendering disabled buttons & combo boxes
        protected Dictionary<Button, string> buttonLabels = new Dictionary<Button, string>();
        protected Dictionary<Control, string> controlCueBanners = new Dictionary<Control, string>();
        protected List<ComboBox> dropDownStyleComboBox = new List<ComboBox>();

        protected Control lastSelectedControl;


        //BACKGROUND TASKS PROCESSING
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

        public PluginWindowTemplate()
        {
            //Some operations won't create visual forms of commands. Only they use this constructor. Let's skip component initialization in this case.
        }

        public PluginWindowTemplate(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;
        }

        public bool backgroundTaskIsWorking()
        {
            lock (taskStarted)
            {
                if (backgroundTaskIsScheduled && !backgroundTaskIsCanceled)
                    return true;
            }

            return false;
        }

        public void SetComboBoxCue(ComboBox comboBox, string cue)
        {
            controlCueBanners.AddReplace(comboBox, cue);
            comboBox.SetCue(cue);
        }

        public void ClearComboBoxCue(ComboBox comboBox)
        {
            controlCueBanners.RemoveExisting(comboBox);
            comboBox.ClearCue();
        }

        public void comboBox_EnabledChanged(object sender, System.EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            if (dropDownStyleComboBox.Contains(comboBox))
            {
                if (comboBox.Enabled)
                    comboBox.DropDownStyle = ComboBoxStyle.DropDown;
                else
                    comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            }
        }

        private void comboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            int index = e.Index;

            Color foreColor;
            Color backColor;

            if (comboBox.Enabled && comboBox.Focused && (index == -1 || (e.State & DrawItemState.Selected) != 0))
            {
                foreColor = SystemColors.HighlightText;
                backColor = SystemColors.Highlight;
            }
            //Disabled
            else if (e.State == (DrawItemState.Disabled | DrawItemState.NoAccelerator
                | DrawItemState.NoFocusRect | DrawItemState.ComboBoxEdit))
            {
                foreColor = DimmedAccentColor;
                backColor = FormBackColor;
            }
            else //if (comboBox.Enabled)
            {
                foreColor = ForeColor;
                backColor = BackColor;
            }

            string text = string.Empty;
            if (index == -1)
            {
                if (!controlCueBanners.TryGetValue(comboBox, out text))
                    text = comboBox.Text;
            }
            else
            {
                text = comboBox.Items[index].ToString();
            }

            //Draw the background of the item.
            e.DrawBackground();

            e.Graphics.FillRectangle(new SolidBrush(backColor), e.Bounds);

            TextRenderer.DrawText(
                dc: e.Graphics,
                text: text,
                font: comboBox.Font,
                bounds: e.Bounds,
                foreColor: foreColor,
                backColor: backColor,
                flags: TextFormatFlags.Top | TextFormatFlags.Left);


            //Draw the focus rectangle if the mouse hovers over an item.
            e.DrawFocusRectangle();
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

            if (button.Enabled && button == AcceptButton)
                button_GotFocus(sender, e);
            else if (button.Focused)
                button_GotFocus(sender, e);
            else if (UseMusicBeeFontSkinColors)
                button_LostFocus(sender, e);
        }

        protected void setButtonColors(Button button)
        {
            if (UseMusicBeeFontSkinColors)
            {
                Button acceptButton = (Button)AcceptButton;

                if (button.Enabled && button == acceptButton)
                {
                    button.FlatAppearance.BorderColor = ButtonFocusedBorderColor;
                    button.ForeColor = ControlHighlightForeColor;
                    button.BackColor = ControlHighlightBackColor;
                }
                //else if (button.Enabled && button.Focused) //***
                //{
                //    button.FlatAppearance.BorderColor = ButtonFocusedBorderColor;
                //    button.ForeColor = ButtonForeColor;
                //    button.BackColor = ButtonBackColor;
                //}
                else if (button.Enabled)
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
        }

        public void button_GotFocus(object sender, System.EventArgs e)
        {
            Button button = sender as Button;
            Button acceptButton = (Button)AcceptButton;

            if (button == lastSelectedControl)
            {
                //Nothing...
            }
            else if (artificiallyFocusedAcceptButton && acceptButton == button)
            {
                artificiallyFocusedAcceptButton = false;

                //EITHER:
                //lastSelectedControl = button;

                //OR:
                button_LostFocus(button, null);
                lastSelectedControl.Select();
            }
            else if (acceptButton != button) //Not accept button clicked
            {
                if (!nonDefaultableButtons.Contains(button)) //It's defaultable button
                {
                    lastSelectedControl = button;
                    AcceptButton = button;
                    button_LostFocus(acceptButton, null);
                }
                else if (acceptButton != null && acceptButton.Enabled && !artificiallyFocusedAcceptButton) //It's non-defaultable button & accept button enabled
                {
                    artificiallyFocusedAcceptButton = true;
                    acceptButton.Focus();
                }
                else //It's non-defaultable button & accept button disabled, let's activate last active control
                {
                    lastSelectedControl?.Select();
                }
            }
            else //Accept button clicked
            {
                lastSelectedControl = button;
            }


            setButtonColors(button);
        }

        public void button_LostFocus(object sender, System.EventArgs e)
        {
            setButtonColors(sender as Button);
        }

        public void button_Paint(object sender, PaintEventArgs e)
        {
            Button button = (Button)sender;

            string text = buttonLabels[button];

            // Set flags to center text on the button.
            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;   // center the text
            
            // Render the text onto the button.
            TextRenderer.DrawText(e.Graphics, text, button.Font, e.ClipRectangle, button.ForeColor, flags);
        }

        public void button_TextChanged(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string text = button.Text;

            if (!string.IsNullOrEmpty(text))
            {
                buttonLabels.AddReplace(button, text);

                if (UseMusicBeeFontSkinColors)
                    button.Text = string.Empty;

                button.Refresh();
            }
        }

        public void control_Enter(object sender, System.EventArgs e)
        {
            var control = sender as Control;
            if (!(control is Button) && !control.GetType().IsSubclassOf(typeof(Button)) && control.Controls.Count == 0)
                lastSelectedControl = control;
        }

        //int - parent level if control2 is ancestor, otherwise 0
        public static (Control, int, bool, bool, bool, string[]) GetReferredControlControlLevelMarksRemarks(Control control)
        {
            (_, _, bool scaledMovedL, bool scaledMovedR, bool scaledMovedY, string[] remarks) = GetControlTagReferredNameMarksRemarks(control);
            (Control control2, int level) = GetReferredControl(control);

            return (control2, level, scaledMovedL, scaledMovedR, scaledMovedY, remarks);
        }


        public static void SetControlTagReferredNameMarksRemarks(Control control, string tagValue,
            bool scaledMovedL = false, bool scaledMovedR = false, bool scaledMovedY = false,
            string control2Name = null, params string[] remarks)
        {
            (_, string oldControl2Name, bool scaledMovedOldL, bool scaledMovedOldR, bool scaledMovedOldY, string[] oldRemarks) = GetControlTagReferredNameMarksRemarks(control);

            scaledMovedL |= scaledMovedOldL;
            scaledMovedR |= scaledMovedOldR;
            scaledMovedY |= scaledMovedOldY;

            
            if (control2Name == null)
                control2Name = oldControl2Name;

            if (remarks.Length == 0)
                remarks = oldRemarks;

            control.Tag = tagValue ?? string.Empty;

            if (!string.IsNullOrEmpty(control2Name))
                control.Tag += "#" + control2Name;

            if (scaledMovedL)
                control.Tag += "#scaled-moved-left";

            if (scaledMovedR)
                control.Tag += "#scaled-moved-right";

            if (scaledMovedY)
                control.Tag += "#scaled-moved-y";

            if (remarks.Length != 0)
                control.Tag += remarks.Aggregate(string.Empty, (arg1, arg2) => arg1 + "@" + arg2);
        }

        //Returns initial tag value, referred control name, moved-scaled marks and array of custom remarks
        public static (string, string, bool, bool, bool, string[]) GetControlTagReferredNameMarksRemarks(Control control)
        {
            if (control.Tag == null || !(control.Tag is string))
                return (string.Empty, string.Empty, false, false, false, new string[0]);

            string controlTag = control.Tag as string;
            bool scaledMovedL = controlTag.Contains("#scaled-moved-left");
            bool scaledMovedR = controlTag.Contains("#scaled-moved-right");
            bool scaledMovedY = controlTag.Contains("#scaled-moved-y");
            controlTag = controlTag.Replace("#scaled-moved-left", string.Empty);
            controlTag = controlTag.Replace("#scaled-moved-right", string.Empty);
            controlTag = controlTag.Replace("#scaled-moved-y", string.Empty);

            string tagValue = controlTag;
            string remarks = string.Empty;
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
                remarks = Regex.Replace(control2Name, "^(.*?)@(.*)", "$2");
                control2Name = control2Name.Replace("@" + remarks, string.Empty);
            }

            if (tagValue.Contains("@"))
            {
                remarks = Regex.Replace(tagValue, "^(.*?)@(.*)", "$2");
                tagValue = tagValue.Replace("@" + remarks, string.Empty);
            }

            if (remarks == string.Empty)
                return (tagValue, control2Name, scaledMovedL, scaledMovedR, scaledMovedY, new string[0]);
            else
                return (tagValue, control2Name, scaledMovedL, scaledMovedR, scaledMovedY, remarks.Split('@'));
        }

        public static (Control, int) GetReferredControl(Control control) //int - parent level if control2 is ancestor, otherwise 0
        {
            (_, string control2Name, _, _, _, _) = GetControlTagReferredNameMarksRemarks(control);

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

        public static Size GetControlSize(Control control)
        {
            if (control.GetType().IsSubclassOf(typeof(Form)) || control.GetType() == typeof(Form))
                return ((Form)control).ClientSize;
            else
                return control.Size;
        }

        public static int GetControlLevel(Control control, Control control2)
        {
            if (control == null)
                return 0;


            string control2Name = control2.Name;

            int level = 0;

            Control foundParent = null;
            Control parent = control.Parent;
            while (foundParent == null && parent != null)
            {
                level++;

                if (parent.Name == control2Name)
                    foundParent = parent;
                else
                    parent = parent.Parent;
            }

            if (foundParent == null)
                level = 0;

            return level;
        }

        public static (int, int, int, int) GetControl2LeftRightMarginsOrParent0WidthPaddings(Control control, Control control2, bool pinnedToParent = false) //Returns: Left, Right or 0 & parent Width, margins or parent padding
        {
            if (control2 == null && pinnedToParent)
                return (0, GetControlSize(control.Parent).Width, control.Parent.Padding.Left, control.Parent.Padding.Right);
            

            int level = GetControlLevel(control, control2);

            if (level > 0)
                return (GetControlSize(control2).Width, GetControlSize(control2).Width, control2.Padding.Right, control2.Padding.Left);
            else //if (level == 0)
                return (control2.Location.X, control2.Location.X + control2.Width, control2.Margin.Left, control2.Margin.Right);
        }

        public int getRightAnchoredControlX(Control control, int controlNewWidth, Control control2, bool pinnedToParent, int level, float offset = 0)
        {
            if (pinnedToParent)
            {
                return GetControlSize(control.Parent).Width - controlNewWidth - (control.Margin.Right + control.Parent.Padding.Right);
            }
            else if (level == 0)
            {
                if (offset == 0)
                    return control2.Location.X - controlNewWidth - (control.Margin.Right + control2.Margin.Left);
                else
                    return (int)Math.Round(control2.Location.X - controlNewWidth - offset * hDpiFontScaling);
            }
            else
            {
                return GetControlSize(control2).Width - controlNewWidth - (control.Margin.Right + control2.Padding.Right);
            }
        }

        public static float GetYCenteredToY2(float controlHeight, float refControlY, Control control2)
        {
            float control2MiddleY = refControlY + control2.Height / 2f;
            return control2MiddleY - controlHeight / 2f;
        }

        public int getButtonY(Control control, AnchorStyles style)
        {
            float controlNewY = control.Location.Y - (control.Height - control.Height / vDpiFontScaling) / 4;//***
            return (int)Math.Round(controlNewY);
        }

        public int getLabelY(Control control)
        {
            float controlNewY = control.Location.Y - (control.Height - control.Height / vDpiFontScaling) / 2;//***
            return (int)Math.Round(controlNewY);
        }

        public int getPictureBoxY(Control control, float scale)
        {
            float controlNewY = control.Location.Y - (control.Height - control.Height / scale) / 2;
            return (int)Math.Round(controlNewY);
        }

        public int getPictureBoxY(float controlHeight, Control control2)
        {
            float heightDifference = controlHeight - control2.Height;
            float controlNewY = GetYCenteredToY2(controlHeight, control2.Location.Y, control2);

            controlNewY -= heightDifference / 4 * vDpiFontScaling; //***
            controlNewY += controlHeight * 0.12f;
            return (int)Math.Round(controlNewY);
        }

        public int getCheckBoxesRadioButtonsY(Control control, Control control2)
        {
            if (control.Height == control2.Height)
            {
                return control.Location.Y;
            }
            else
            {
                float heightDifference = control.Height - control2.Height;
                float controlNewY = GetYCenteredToY2(control.Height, control2.Location.Y, control2);

                controlNewY -= heightDifference / 4 * vDpiFontScaling; //***
                controlNewY += control.Height * 0.12f;
                return (int)Math.Round(controlNewY);
            }
        }

        //Let's correct flaws of AUTO-scaling
        public virtual void preMoveScaleControl(Control control)
        {
            //if (control.GetType().IsSubclassOf(typeof(Form)) || control.GetType() == typeof(Form))
                return;

            string stringTag = control.Tag as string;

            if (control.GetType().IsSubclassOf(typeof(Button)) || control.GetType() == typeof(Button))
            {
                control.Location = new Point(control.Location.X, getButtonY(control, control.Anchor));
            }
            else if (control.GetType().IsSubclassOf(typeof(PictureBox)) || control.GetType() == typeof(PictureBox))
            {
                if (stringTag?.Contains("@small-picture") == true)
                {
                    control.Width = (int)Math.Round(control.Width * hDpiFontScaling);
                    control.Height = (int)Math.Round(control.Height * hDpiFontScaling);

                    control.Location = new Point(control.Location.X, getPictureBoxY(control, hDpiFontScaling));
                }
                else
                {
                    control.Width = (int)Math.Round(control.Width * vDpiFontScaling);
                    control.Height = (int)Math.Round(control.Height * vDpiFontScaling);

                    control.Location = new Point(control.Location.X, getPictureBoxY(control, vDpiFontScaling));
                }
            }
            else if (control.GetType().IsSubclassOf(typeof(Label)) || control.GetType() == typeof(Label))
            {
                control.Location = new Point(control.Location.X, getLabelY(control));
            }


            //Control that was initially square (before AUTO-scaling) 
            if (stringTag?.Contains("@square-control") == true)
                control.Width = control.Height;
        }

        public void moveScaleControlDependentReferringControls(Control control)
        {
            if (control.GetType().IsSubclassOf(typeof(Form)) || control.GetType() == typeof(Form))
                return;


            const float smallControlOffsetX = 0.1f;
            const float checkBoxRadioButtonOffsetCompensationX = 12f;


            controlsReferenced.TryGetValue(control, out Control referringControl);

            (Control control2, int level, bool scaledMovedL, bool scaledMovedR, bool scaledMovedY, string[] remarks) = GetReferredControlControlLevelMarksRemarks(control);
            bool pinnedToParent = remarks.Contains("pinned-to-parent");

            //control is left & right anchored. Let's move/scale it at the end (in another function).
            if ((control.Anchor & AnchorStyles.Left) != 0 && (control.Anchor & AnchorStyles.Right) != 0)
            {
                if (!leftRightAnchoredControls.Contains(control))
                    leftRightAnchoredControls.Add(control);

                return;
            }


            //control is left anchored
            if (referringControl != null && (control.Anchor & AnchorStyles.Left) != 0 && (control.Anchor & AnchorStyles.Right) == 0)
                moveScaleControlDependentReferringControls(referringControl);
            //referringControl is left anchored, control must be left & right anchored (left anchored only is considered above) 
            else if (referringControl != null && (referringControl.Anchor & AnchorStyles.Left) != 0 && (referringControl.Anchor & AnchorStyles.Right) != 0)
                moveScaleControlDependentReferringControls(referringControl);


            //Special case. Will move/scale it to parent. Let's proceed further...
            if (pinnedToParent)
                ;
            //No control2 and control is NOT left & right anchored (see above)
            else if (control2 == null)
                return;
            //control2 is right anchored and control is NOT left & right anchored (see earlier)
            else if ((control2.Anchor & AnchorStyles.Left) == 0 && (control2.Anchor & AnchorStyles.Right) != 0)
                moveScaleControlDependentReferringControls(control2);



            if (scaledMovedL && scaledMovedR && scaledMovedY)
                return;


            bool scaledMovedL2 = true;
            bool scaledMovedR2 = true;
            bool scaledMovedY2 = true;

            if (control2 != null)
                (_, _, scaledMovedL2, scaledMovedR2, scaledMovedY2, _) = GetControlTagReferredNameMarksRemarks(control2);


            //control left anchored
            if ((control.Anchor & AnchorStyles.Left) != 0 && (control.Anchor & AnchorStyles.Right) == 0)
            {
                if (pinnedToParent || level > 0)
                {
                    if (!scaledMovedL)
                    {
                        var parent = control2;
                        if (parent == null)
                            parent = control.Parent;

                        control.Location = new Point(parent.Padding.Left + control.Margin.Left, control.Location.Y);
                        scaledMovedL = true;
                    }
                }
                else if (control.GetType().IsSubclassOf(typeof(CheckBox)) || control.GetType() == typeof(CheckBox) ||
                            control.GetType().IsSubclassOf(typeof(RadioButton)) || control.GetType() == typeof(RadioButton)
                )
                {
                    if (!scaledMovedY)
                    {
                        if (control2.GetType().IsSubclassOf(typeof(Label)) || control2.GetType() == typeof(Label))
                        {
                            control.Location = new Point(control.Location.X, getCheckBoxesRadioButtonsY(control, control2));
                        }
                        else
                        {
                            control.Location = new Point(control.Location.X, (int)Math.Round(GetYCenteredToY2(control.Height, control2.Location.Y, control2)));
                        }

                        scaledMovedY = true;
                    }

                    if (!scaledMovedL2)
                    {
                        control2.Location = new Point((int)Math.Round(control.Location.X + control.Width - checkBoxRadioButtonOffsetCompensationX * hDpiFontScaling), control2.Location.Y);
                        scaledMovedL2 = true;
                    }
                }
                else if (control.GetType().IsSubclassOf(typeof(PictureBox)) || control.GetType() == typeof(PictureBox))
                {
                    if (!scaledMovedY2)
                    {
                        if (control2.GetType().IsSubclassOf(typeof(Label)) || control2.GetType() == typeof(Label))
                        {
                            control2.Location = new Point((int)Math.Round(control.Location.X + control.Width + smallControlOffsetX * hDpiFontScaling), control2.Location.Y);
                        }
                        else
                        {
                            control2.Location = new Point(control.Location.X + control.Width + control.Margin.Right + control2.Margin.Left, control2.Location.Y);
                        }

                        scaledMovedL2 = true;
                    }

                    if (!scaledMovedY)
                    {
                        control.Location = new Point(control.Location.X, getPictureBoxY(control.Height, control2));
                        scaledMovedY = true;
                    }
                }
                //control2 left anchored
                else if ((control2.Anchor & AnchorStyles.Left) != 0 && (control2.Anchor & AnchorStyles.Right) == 0)
                {
                    if (!scaledMovedL2)
                    {
                        control2.Location = new Point(control.Location.X + control.Width + control2.Margin.Left + control.Margin.Right, control2.Location.Y);
                        scaledMovedL2 = true;
                    }
                }
                //control2 must NOT be right anchored. See below.
                else if ((control2.Anchor & AnchorStyles.Left) == 0 && (control2.Anchor & AnchorStyles.Right) != 0)
                {
                    throw new Exception("Invalid control2 anchoring for control: " + control.Name + "! Expected anchor is not right.");
                }
                //control2 must NOT be not anchored
                else if ((control2.Anchor & AnchorStyles.Left) == 0 && (control2.Anchor & AnchorStyles.Right) == 0)
                {
                    throw new Exception("Invalid control2 anchoring for control: " + control.Name + "! Expected anchor is not none.");
                }
            }
            //control right anchored
            else if ((control.Anchor & AnchorStyles.Left) == 0 && (control.Anchor & AnchorStyles.Right) != 0)
            {
                if (pinnedToParent || level > 0)
                {
                    if (!scaledMovedL)
                    {
                        var parent = control2;
                        if (parent == null)
                            parent = control.Parent;

                        int controlNewX = GetControlSize(parent).Width - control.Width - (parent.Padding.Right + control.Margin.Right);

                        control.Location = new Point(controlNewX, control.Location.Y);
                        scaledMovedL = true;
                    }
                }
                else if (control.GetType().IsSubclassOf(typeof(CheckBox)) || control.GetType() == typeof(CheckBox) ||
                    control.GetType().IsSubclassOf(typeof(RadioButton)) || control.GetType() == typeof(RadioButton)
                )
                {
                    if (!scaledMovedL && !scaledMovedY)
                    {
                        if (!scaledMovedY && (control2.GetType().IsSubclassOf(typeof(Label)) || control2.GetType() == typeof(Label)))
                        {
                            control.Location = new Point(getRightAnchoredControlX(control, control.Width, control2, pinnedToParent, level, -checkBoxRadioButtonOffsetCompensationX * hDpiFontScaling), getCheckBoxesRadioButtonsY(control, control2));
                        }
                        else
                        {
                            control.Location = new Point(getRightAnchoredControlX(control, control.Width, control2, pinnedToParent, level), (int)Math.Round(GetYCenteredToY2(control.Height, control2.Location.Y, control2)));
                        }

                        scaledMovedL = true;
                        scaledMovedY = true;
                    }
                }
                else if (control.GetType().IsSubclassOf(typeof(PictureBox)) || control.GetType() == typeof(PictureBox))
                {
                    if (!scaledMovedL)
                    {
                        if (control2.GetType().IsSubclassOf(typeof(Label)) || control2.GetType() == typeof(Label))
                        {
                            control.Location = new Point(getRightAnchoredControlX(control, control.Width, control2, pinnedToParent, level, smallControlOffsetX), control.Location.Y);
                        }
                        else
                        {
                            control.Location = new Point(getRightAnchoredControlX(control, control.Width, control2, pinnedToParent, level, control.Margin.Right + control2.Margin.Left), control.Location.Y);
                        }

                        scaledMovedL = true;
                    }

                    if (!scaledMovedY)
                    {
                        control.Location = new Point(control.Location.X, getPictureBoxY(control.Height, control2));
                        scaledMovedY = true;
                    }
                }
                //control2 right anchored
                else if (((control2.Anchor & AnchorStyles.Left) == 0 && (control2.Anchor & AnchorStyles.Right) != 0) || level > 0)
                {
                    if (!scaledMovedL)
                    {
                        control.Location = new Point(getRightAnchoredControlX(control, control.Width, control2, pinnedToParent, level), control.Location.Y);
                        scaledMovedL = true;
                    }
                }
                //control2 must NOT be left anchored. See below.
                else if ((control2.Anchor & AnchorStyles.Left) != 0 && (control2.Anchor & AnchorStyles.Right) == 0)
                {
                    throw new Exception("Invalid control2 anchoring for control: " + control.Name + "! Expected anchor is not left.");
                }
                //control2 must NOT be not anchored
                else if ((control2.Anchor & AnchorStyles.Left) == 0 && (control2.Anchor & AnchorStyles.Right) == 0)
                {
                    throw new Exception("Invalid control2 anchoring for control: " + control.Name + "! Expected anchor is not none.");
                }
            }


            SetControlTagReferredNameMarksRemarks(control, null, scaledMovedL, scaledMovedR, scaledMovedY);

            if (control2 != null)
                SetControlTagReferredNameMarksRemarks(control2, null, scaledMovedL2, scaledMovedR2, scaledMovedY2);
        }

        public void scaleMoveLeftRightAnchoredControls()
        {
            foreach (var control in leftRightAnchoredControls)
            {
                controlsReferenced.TryGetValue(control, out var controlReferencing);
                controlsReferences.TryGetValue(control, out var controlReference);

                (_, _, _, _, _, string[] remarks) = GetControlTagReferredNameMarksRemarks(control);
                bool pinnedToParent = remarks.Contains("pinned-to-parent");
                int controlReferencingLevel = GetControlLevel(controlReferencing, control); 

                int controlRNew;

                if (controlReferencing != null && controlReferencingLevel == 0 && controlReference != null)
                {
                    (int controlReferencingLeft, int controlReferencingRight, int controlReferencingMarginLeft, int controlReferencingMarginRight) = GetControl2LeftRightMarginsOrParent0WidthPaddings(control, controlReferencing, pinnedToParent);
                    (int controlReferenceLeft, int controlReferenceRight, int controlReferenceMarginLeft, int controlReferenceMarginRight) = GetControl2LeftRightMarginsOrParent0WidthPaddings(control, controlReference, pinnedToParent);

                    controlRNew = controlReferenceLeft - (controlReferenceMarginLeft + control.Margin.Right);
                    control.Location = new Point(controlReferencingRight + (controlReferencingMarginRight + control.Margin.Left), control.Location.Y);
                    control.Width = controlRNew - control.Location.X;
                
                }
                else if (controlReferencing != null && controlReferencingLevel == 0 && controlReference == null)
                {
                    (int controlReferencingLeft, int controlReferencingRight, int controlReferencingMarginLeft, int controlReferencingMarginRight) = GetControl2LeftRightMarginsOrParent0WidthPaddings(control, controlReferencing, pinnedToParent);

                    controlRNew = control.Location.X + control.Width;
                    control.Location = new Point(controlReferencingLeft + (controlReferencingMarginRight + control.Margin.Left), control.Location.Y);
                    control.Width = controlRNew - control.Location.X;
                }
                else if ((controlReferencing == null || controlReferencingLevel > 0) && controlReference != null)
                {
                    (int controlReferenceLeft, int controlReferenceRight, int controlReferenceMarginLeft, int controlReferenceMarginRight) = GetControl2LeftRightMarginsOrParent0WidthPaddings(control, controlReference, pinnedToParent);

                    if (pinnedToParent && GetControlLevel(control, controlReference) > 0) //Pinned to controlReference (i.e. controlReference is a parent)
                    {
                        (int controlReferencingLeft, int controlReferencingRight, int controlReferencingMarginLeft, int controlReferencingMarginRight) = GetControl2LeftRightMarginsOrParent0WidthPaddings(control, control.Parent, true);

                        control.Location = new Point(controlReferencingMarginRight + control.Margin.Left, control.Location.Y);
                        controlRNew = controlReferenceRight - (controlReferenceMarginRight + control.Margin.Right);
                    }
                    else
                    {
                        controlRNew = controlReferenceLeft - (controlReferenceMarginLeft + control.Margin.Right);
                    }

                    control.Width = controlRNew - control.Location.X;
                }
                else //if (controlReferencing == null && controlReference == null)
                {
                    //Nothing to do. Lets keep auto-scaled scaling/position... 
                }
            }
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
                button.LostFocus += button_LostFocus;


                if (!UseMusicBeeFontSkinColors)
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

                    button.Text = string.Empty;
                }

                return;
            }
            else
            {
                control.Enter += control_Enter;
            }


            //SplitContainer and (disabled) Button above must be skinned in any case (even if using system colors)
            if (!UseMusicBeeFontSkinColors)
                return;


            if (control.GetType().IsSubclassOf(typeof(TextBox)) || control.GetType() == typeof(TextBox))
            {
                control.BackColor = InputControlBackColor;
                control.ForeColor = InputControlForeColor;

                ((TextBox)control).BorderStyle = BorderStyle.FixedSingle;
            }
            else if (control.GetType().IsSubclassOf(typeof(ComboBox)) || control.GetType() == typeof(ComboBox))
            {
                ComboBox comboBox = control as ComboBox;

                control.BackColor = InputControlBackColor;
                control.ForeColor = InputControlForeColor;

                comboBox.FlatStyle = FlatStyle.Flat;

                if (comboBox.DropDownStyle == ComboBoxStyle.DropDownList) //For rendering high contrast cue banners.
                {
                    comboBox.DrawItem += comboBox_DrawItem;
                    comboBox.DrawMode = DrawMode.OwnerDrawFixed;
                }
                else //DropDown look ugly if disabled. Let's handle 
                {
                    dropDownStyleComboBox.Add(comboBox);

                    comboBox.EnabledChanged += comboBox_EnabledChanged;
                    comboBox_EnabledChanged(comboBox, null);
                }
            }
            else if (control.GetType().IsSubclassOf(typeof(ListBox)) || control.GetType() == typeof(ListBox))
            {
                control.BackColor = InputControlBackColor;
                control.ForeColor = InputControlForeColor;
                
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
                control.ForeColor = FormForeColor;

                ((CheckBox)control).FlatStyle = FlatStyle.System;
            }
            else if (control.GetType().IsSubclassOf(typeof(RadioButton)) || control.GetType() == typeof(RadioButton))
            {
                control.BackColor = FormBackColor;
                control.ForeColor = FormForeColor;

                ((RadioButton)control).FlatStyle = FlatStyle.System;
            }
            else if (control.GetType() == typeof(DataGridView))
            {
                control.BackColor = HeaderCellStyle.BackColor;
                control.ForeColor = HeaderCellStyle.ForeColor;

                ((DataGridView)control).BorderStyle = BorderStyle.FixedSingle;
                ((DataGridView)control).ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                ((DataGridView)control).BackgroundColor = UnchangedCellStyle.BackColor;
                ((DataGridView)control).DefaultCellStyle = UnchangedCellStyle;
            }
            else
            {
                control.BackColor = FormBackColor;
                control.ForeColor = AccentColor;
            }

            return;
        }

        public void fillControlsReferencesRemarks()
        {
            for (int i = allControls.Count - 1; i >= 0; i--)
            {
                var control = allControls[i];
                
                (Control control2, int level, _, _, _, string[] remarks) = GetReferredControlControlLevelMarksRemarks(control);

                if (control2 != null)
                {
                    controlsReferences.AddReplace(control, control2);
                    controlsReferenced.AddReplace(control2, control);
                }

                if (remarks.Contains("pinned-to-parent") && !pinnedToParentControls.Contains(control))
                    pinnedToParentControls.Add(control);

                if (remarks.Contains("non-defaultable") && !nonDefaultableButtons.Contains(control))
                    nonDefaultableButtons.Add((Button)control);
            }
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
            if (UseMusicBeeFontSkinColors)
            {
                BackColor = FormBackColor;
                ForeColor = FormForeColor;
            }

            fillControlsReferencesRemarks();


            for (int i = allControls.Count - 1; i >= 0; i--)
                skinControl(allControls[i]);


            for (int i = allControls.Count - 1; i >= 0; i--)
                preMoveScaleControl(allControls[i]);

            for (int i = allControls.Count - 1; i >= 0; i--)
                moveScaleControlDependentReferringControls(allControls[i]);

            scaleMoveLeftRightAnchoredControls();
        }

        protected void setInitialFormMaximumMinimumSize(Size initialMinimumSize, Size initialMaximumSize, bool sameMinMaxWidth, bool sameMinMaxHeight)
        {
            if (modal)
                MinimizeBox = false;


            hDpiFormScaling = (float)MinimumSize.Width / initialMinimumSize.Width;
            vDpiFormScaling = (float)MinimumSize.Height / initialMinimumSize.Height;

            if (fixedSize)
            {
                Width = (int)Math.Round(Width * hDpiFormScaling);
            }
            else
            {
                int maxWidth = 0;
                if (sameMinMaxWidth)
                    maxWidth = MinimumSize.Width;
                else if (initialMaximumSize.Width != 0)
                    maxWidth = (int)Math.Round(initialMaximumSize.Width * hDpiFormScaling);

                int maxHeight = 0;
                if (sameMinMaxHeight)
                    maxHeight = MinimumSize.Height;
                else if (initialMaximumSize.Height != 0)
                    maxHeight = (int)Math.Round(initialMaximumSize.Height * vDpiFormScaling);

                MaximumSize = new Size(maxWidth, maxHeight);
            }
        }

        public enum FontEquality
        {
            DifferentFontUnits = -1,

            DifferentStylesSizes = 0,

            EqualSizes = 1,

            SymbolStyles = 2,
            EqualStyles = 4,

            EqualSizesStyles = EqualSizes | EqualStyles,

            DifferentNames = 8,
            PartiallyEqualNames = 16,
            EqualNames = 32,

            Equal = EqualSizesStyles | EqualNames,
        }

        public static FontEquality CompareFonts(Font font1, Font font2)
        {
            FontEquality fontNameEquality;

            string lcFontName = font1.Name.ToLower();
            bool isSymbolFont = lcFontName.Contains("icons") || lcFontName.Contains("mdl2") || lcFontName.Contains("symbol");

            lcFontName = font2.Name.ToLower();
            isSymbolFont |= lcFontName.Contains("icons") || lcFontName.Contains("mdl2") || lcFontName.Contains("symbol");

            if (font1.Name == font2.Name)
                fontNameEquality = FontEquality.EqualNames;
            else if ((font1.Name.StartsWith(font2.Name) || font2.Name.StartsWith(font1.Name)) && !isSymbolFont)
                fontNameEquality = FontEquality.PartiallyEqualNames;
            else
                fontNameEquality = FontEquality.DifferentNames;


            if (font1.Unit != font2.Unit)
            {
                return FontEquality.DifferentFontUnits;
            }
            else if (isSymbolFont && font1.Size == font2.Size)
            {
                return FontEquality.EqualSizes | FontEquality.SymbolStyles | fontNameEquality;
            }
            else if (isSymbolFont)
            {
                return FontEquality.SymbolStyles | fontNameEquality;
            }
            else if (font1.Style == font2.Style && font1.Size == font2.Size && font1.GdiVerticalFont == font2.GdiVerticalFont)
            {
                return FontEquality.EqualSizesStyles | fontNameEquality;
            }
            else if (font1.Size == font2.Size && font1.GdiVerticalFont == font2.GdiVerticalFont)
            {
                return FontEquality.EqualSizes | fontNameEquality;
            }
            else
            {
                return FontEquality.DifferentStylesSizes | fontNameEquality;
            }
        }

        protected void scaleForm()
        {
            List<Control> ownFontControls = new List<Control>();

            for (int i = 0; i <  allControls.Count; i++) //Required for correct DPI scaling
            {
                var control = allControls[i];

                var fontEquality = CompareFonts(control.Font, Font);
                bool sameFonts = (fontEquality == FontEquality.Equal);

                if (!sameFonts)
                    ownFontControls.Add(control);

                if (control.GetType().IsSubclassOf(typeof(ContainerControl)))
                    (control as ContainerControl).AutoScaleMode = AutoScaleMode.Inherit;

                if (control is SplitContainer) //Let's remember initial properties to scale them manually later
                {
                    SplitContainer sc = control as SplitContainer;
                    SplitContainerScalingAttributes scsa = default;
                    scsa.splitContainer = sc;
                    scsa.panel2MinSize = sc.Panel1MinSize;
                    scsa.panel2MinSize = sc.Panel2MinSize;
                    scsa.splitterDistance = sc.SplitterDistance;

                    splitContainersScalingAttributes.Add(scsa);
                }
            }


            bool sameMinMaxWidth = false;
            bool sameMinMaxHeight = false;

            if ((Tag as string)?.Contains("@min-max-width-same") == true)
                sameMinMaxWidth = true;

            if ((Tag as string)?.Contains("@min-max-height-same") == true)
                sameMinMaxHeight = true;

            var resources = new ResourceManager(GetType());

            object resource = resources.GetObject("$this.MaximumSize");
            Size initialMaximumSize = resource == null ? new Size(0, 0) : (Size)resource;
            resource = resources.GetObject("$this.MinimumSize");
            Size initialMinimumSize = resource == null ? new Size(0, 0) : (Size)resource;
            resource = resources.GetObject("$this.ClientSize");
            Size initialClientSize = resource == null ? new Size(0, 0) : (Size)resource;
            resource = resources.GetObject("$this.Font");
            Font initialFormFont = resource == null ? Font : (Font)resource;


            MaximumSize = new Size(0, 0); //Let's temporary remove max. size restrictions


            Font mbFont;
            if (UseMusicBeeFontSkinColors)
                mbFont = MbApiInterface.Setting_GetDefaultFont();
            else
                mbFont = Font;

            FontEquality mbThisFormFontEquality = CompareFonts(mbFont, initialFormFont);


            for (int i = allControls.Count - 1; i >= 0; i--) //Required for correct DPI scaling
                allControls[i].SuspendLayout();

            SuspendLayout();


            if (mbThisFormFontEquality == FontEquality.DifferentFontUnits)
            {
                MessageBox.Show(MbForm, "Unsupported MusicBee font type!\n" + //*******
                    "Either choose different font in MusicBee preferences or disable using skin colors in plugin settings.",
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Won't change form's font, but let's rescale in case of DPI change...
            }
            else if (mbThisFormFontEquality == FontEquality.Equal)
            {
                //Won't change form's font, but let's rescale in case of DPI change...
            }
            else
            {
                bool formFontEqualNamesStyles;

                if ((mbThisFormFontEquality & FontEquality.EqualNames & FontEquality.EqualStyles) != 0) //Let's change font sizes only, they can't be the same (see above)
                {
                    formFontEqualNamesStyles = true;
                    Font = new Font(Font.Name, mbFont.Size, Font.Style);
                }
                else
                {
                    formFontEqualNamesStyles = false;
                    Font = new Font(mbFont.Name, mbFont.Size, mbFont.Style | Font.Style);
                }


                foreach (var control in allControls)
                {
                    if (ownFontControls.Contains(control))
                    {
                        var controlFormFontEquality = CompareFonts(control.Font, initialFormFont);

                        if (formFontEqualNamesStyles)
                            control.Font = new Font(control.Font.Name, mbFont.Size * control.Font.Size / initialFormFont.Size, control.Font.Style);
                        else if ((controlFormFontEquality & FontEquality.EqualNames) != 0 || (controlFormFontEquality & FontEquality.PartiallyEqualNames) != 0)
                            control.Font = new Font(mbFont.Name, mbFont.Size * control.Font.Size / initialFormFont.Size, mbFont.Style | control.Font.Style);
                        else if ((controlFormFontEquality & FontEquality.SymbolStyles) == 0)
                            control.Font = new Font(control.Font.Name, mbFont.Size * control.Font.Size / initialFormFont.Size, mbFont.Style | control.Font.Style);
                        else
                            control.Font = new Font(control.Font.Name, mbFont.Size * control.Font.Size / initialFormFont.Size, control.Font.Style);
                    }
                    else if (control.Text == " ") //It's zero width space
                    {
                        control.Font = new Font(initialFormFont.Name, control.Font.Size, FontStyle.Regular);
                    }
                }
            }


            for (int i = 0; i < allControls.Count; i++) //Required for correct DPI scaling
            {
                allControls[i].ResumeLayout(false);
                allControls[i].PerformLayout();
            }


            ResumeLayout(false);
            PerformLayout();


            MinimumSize = Size;

            hDpiFontScaling = (float)ClientSize.Width / initialClientSize.Width;//***** hDpiFontScaling VS vDpiFontScaling !!!
            vDpiFontScaling = (float)ClientSize.Height / initialClientSize.Height;


            //Split containers must be scaled manually in the child form's "OnLoad" handler using "splitContainersScalingAttributes" (auto-scaling is improper)

            setInitialFormMaximumMinimumSize(initialMinimumSize, initialMaximumSize, sameMinMaxWidth, sameMinMaxHeight);
        }

        protected void setFormMaximizedBounds()
        {
            if (modal && MaximumSize.Height != 0 && !fixedSize)
            {
                int maximizedHeight = MaximumSize.Height;

                if (maximizedHeight > Screen.FromControl(this).WorkingArea.Height)
                    maximizedHeight = Screen.FromControl(this).WorkingArea.Height;

                MaximizedBounds = new Rectangle(Screen.FromControl(this).WorkingArea.Left, 0, //****** scaling needed?
                    Screen.FromControl(this).WorkingArea.Width, maximizedHeight);
            }
        }

        protected void primaryInitialization()
        {
            if (!dontShowForm) //If forceHidden, then form is created to get DPI/font scaling only, won't show it, form will be disposed soon.
            {
                if (initialized)
                {
                    tryShowForm();
                    return;
                }


                //Common initialization
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
            }

            //DPI/font scaling & loading plugin windows sizes/positions
            fixedSize = (FormBorderStyle == FormBorderStyle.FixedDialog) || (FormBorderStyle == FormBorderStyle.FixedSingle) ? true : false;

            if (DeviceDpi != 96)
                dpiScaling = DeviceDpi / 96f;

            UseMusicBeeFontSkinColors = SavedSettings.useMusicBeeFontSkinColors;
            addAllChildrenControls(this);


            ignoreSizePositionChanges = true;

            scaleForm(); //DPI/font scaling

            if (dontShowForm) //Form is created to get DPI/font scaling only, won't show it, form will be disposed soon.
                return;


            loadWindowSizesPositions();

            if (width != 0 && height != 0)
            {
                width = (int)(width * hDpiFormScaling);

                if (!fixedSize)
                    height = (int)(height * vDpiFormScaling);

                Width = width;
                Height = height;
                WindowState = windowState;
            }
            else if (!modal && !fixedSize)
            {
                width = (int)(Width * hDpiFormScaling);
                height = (int)(Height * vDpiFormScaling);

                Width = width;
                Height = height;
                WindowState = FormWindowState.Normal;
            }
            else if (!fixedSize) //Modal
            {
                Width = MinimumSize.Width;
                Height = MinimumSize.Height;
                WindowState = FormWindowState.Normal;

                width = Width;
                height = Height;

            }
            else //Fixed size
            {
                Width = (int)(Width * hDpiFormScaling);
                Height = Height;
                WindowState = FormWindowState.Normal;

                width = Width;
                height = Height;
            }

            Top = (int)(top * hDpiFormScaling);
            Left = (int)(left * hDpiFormScaling);

            setFormMaximizedBounds();

            skinMoveScaleAllControls();//***


            ignoreSizePositionChanges = false;
            initialized = true;


            tryShowForm();
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


                if (!DontShowShowHiddenWindows && OpenedFormsSubmenu.DropDownItems.Count == 0)
                {
                    AddMenuItem(OpenedFormsSubmenu, ShowHiddenCommandName, null, TagToolsPlugin.showHiddenEventHandler);
                    AddMenuItem(OpenedFormsSubmenu, "-", null, null);
                }


                AddMenuItem(OpenedFormsSubmenu, newForm.Text, null, TagToolsPlugin.openWindowActivationEventHandler, true, newForm);

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
            for (int i = allControls.Count - 1; i >= 0; i--)
                if (allControls[i].Focused)
                    lastSelectedControl = allControls[i];
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
                foreach (var item in OpenedFormsSubmenu.DropDownItems)
                {
                    var menuItem = item as ToolStripMenuItem;
                    if (menuItem != null)
                    {
                        var form = menuItem.Tag as PluginWindowTemplate;
                        if (form == this)
                        {
                            OpenedFormsSubmenu.DropDownItems.Remove(menuItem);
                            break;
                        }
                    }
                }

                if (!DontShowShowHiddenWindows && OpenedFormsSubmenu.DropDownItems.Count == 2)
                {
                    OpenedFormsSubmenu.DropDownItems.RemoveAt(1);
                    OpenedFormsSubmenu.DropDownItems.RemoveAt(0);
                }


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


            int width2 = (int)(width / hDpiFormScaling);
            int height2;

            if (fixedSize)
                currentWindowSettings.h = height2 = 0;
            else if (!modal)
                height2 = (int)(height / vDpiFormScaling);
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


            int left2 = (int)(left / hDpiFormScaling);
            if (Math.Abs(left2 - currentWindowSettings.x) <= 1) // 1px
            {
                left2 = currentWindowSettings.x;
            }

            int top2 = (int)(top / vDpiFormScaling);
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
                int column1Width2 = (int)(column1Width / hDpiFontScaling);
                if (100f * Math.Abs(column1Width2 - currentWindowSettings.column1Width) / column1Width2 < 0.5f)
                    column1Width2 = currentWindowSettings.column1Width;

                currentWindowSettings.column1Width = column1Width2;


                int column2Width2 = (int)(column2Width / hDpiFontScaling);
                if (100f * Math.Abs(column2Width2 - currentWindowSettings.column1Width) / column2Width2 < 0.5f)
                    column2Width2 = currentWindowSettings.column2Width;

                currentWindowSettings.column2Width = column2Width2;


                int column3Width2 = (int)(column3Width / hDpiFontScaling);
                if (100f * Math.Abs(column3Width2 - currentWindowSettings.column3Width) / column3Width2 < 0.5f)
                    column3Width2 = currentWindowSettings.column3Width;

                currentWindowSettings.column3Width = column3Width2;
            }

            if (table2column1Width != 0)
            {
                int table2column1Width2 = (int)(table2column1Width / hDpiFontScaling);
                if (100f * Math.Abs(table2column1Width2 - currentWindowSettings.table2column1Width) / table2column1Width2 < 0.5f)
                    table2column1Width2 = currentWindowSettings.table2column1Width;

                currentWindowSettings.table2column1Width = table2column1Width2;


                int table2column2Width2 = (int)(table2column2Width / hDpiFontScaling);
                if (100f * Math.Abs(table2column2Width2 - currentWindowSettings.table2column1Width) / table2column2Width2 < 0.5f)
                    table2column2Width2 = currentWindowSettings.table2column2Width;

                currentWindowSettings.table2column2Width = table2column2Width2;


                int table2column3Width2 = (int)(table2column3Width / hDpiFontScaling);
                if (100f * Math.Abs(table2column3Width2 - currentWindowSettings.table2column3Width) / table2column3Width2 < 0.5f)
                    table2column3Width2 = currentWindowSettings.table2column3Width;

                currentWindowSettings.table2column3Width = table2column3Width2;
            }

            if (splitterDistance != 0)
            {
                int splitterDistance2 = (int)(splitterDistance / vDpiFontScaling);
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

                buttonLabels.TryGetValue(previewButtonParam, out previewButtonText);
                buttonLabels.TryGetValue(okButtonParam, out okButtonText);
                buttonLabels.TryGetValue(closeButtonParam, out closeButtonText);

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

