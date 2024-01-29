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
        protected bool modal;
        protected bool fixedSize = false;
        protected FormWindowState windowState;
        protected int left;
        protected int top;
        protected int height;
        protected int width;

        public bool dontShowForm = false;

        //SKIN COLORING/CUSTOM FORM'S FONT/DPI SCALING
        public float dpiScaling = 1;

        public float hDpiFormScaling = 1;
        public float vDpiFormScaling = 1;

        public float hDpiFontScaling = 1;
        public float vDpiFontScaling = 1;


        //ALL controls of the form and their references (if any)
        protected List<Control> allControls = new List<Control>();

        internal Dictionary<Control, Control> controlsReferencesX = new Dictionary<Control, Control>();
        protected Dictionary<Control, Control> controlsReferencedX = new Dictionary<Control, Control>();

        internal Dictionary<Control, Control> controlsReferencesY = new Dictionary<Control, Control>();
        protected Dictionary<Control, Control> controlsReferencedY = new Dictionary<Control, Control>();

        protected List<Button> nonDefaultableButtons = new List<Button>();
        protected bool artificiallyFocusedAcceptButton = false;

        protected List<Control> pinnedToParentControlsX = new List<Control>();
        protected List<Control> leftRightAnchoredControls = new List<Control>();

        protected List<Control> pinnedToParentControlsY = new List<Control>();
        protected List<Control> topBottomAnchoredControls = new List<Control>();

        protected struct SplitContainerScalingAttributes
        {
            public SplitContainer splitContainer;
            public int panel1MinSize;
            public int panel2MinSize;
            public int splitterDistance;
        }

        protected List<SplitContainerScalingAttributes> splitContainersScalingAttributes = new List<SplitContainerScalingAttributes>();

        protected bool ignoreSizePositionChanges = true;

        protected static bool UseSkinColors = false;
        protected static bool UseMusicBeeFont = false;
        protected static bool UseCustomFont = false;

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
                {
                    comboBox.DropDownStyle = ComboBoxStyle.DropDown;

                    if (comboBox.Text == string.Empty && controlCueBanners.Contains(comboBox))
                        comboBox.Text = controlCueBanners[comboBox];
                }
                else
                {
                    comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                }
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
                foreColor = SystemColors.HighlightText;//*****
                backColor = SystemColors.Highlight;
            }
            //Disabled
            else if (e.State == (DrawItemState.Disabled | DrawItemState.NoAccelerator
                | DrawItemState.NoFocusRect | DrawItemState.ComboBoxEdit))
            {
                foreColor = DimmedAccentColor;
                backColor = comboBox.BackColor;
            }
            else //if (comboBox.Enabled)
            {
                foreColor = comboBox.ForeColor;
                backColor = comboBox.BackColor;
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
            TabPage page = (sender as TabControl).TabPages[e.Index];
            Rectangle paddedBounds = e.Bounds;
            int yOffset = (e.State == DrawItemState.Selected) ? -2 : 1;
            paddedBounds.Offset(1, yOffset);

            e.Graphics.FillRectangle(new SolidBrush(page.BackColor), e.Bounds);
            TextRenderer.DrawText(e.Graphics, page.Text, e.Font, paddedBounds, page.ForeColor);
        }

        public void button_EnabledChanged(object sender, System.EventArgs e)
        {
            Button button = sender as Button;

            if (button.Enabled && button == AcceptButton)
                button_GotFocus(sender, e);
            else if (button.Focused)
                button_GotFocus(sender, e);
            else if (UseSkinColors)
                button_LostFocus(sender, e);
        }

        protected void setButtonColors(Button button)
        {
            if (UseSkinColors)
            {
                Button acceptButton = AcceptButton as Button;

                if (button.Enabled && button == acceptButton)
                {
                    button.FlatAppearance.BorderColor = ButtonFocusedBorderColor;
                    button.ForeColor = ControlHighlightForeColor;
                    button.BackColor = ControlHighlightBackColor;
                }
                //else if (button.Enabled && button.Focused) //*****
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
            Button acceptButton = AcceptButton as Button;

            if (button == lastSelectedControl)
            {
                //Nothing...
            }
            else if (artificiallyFocusedAcceptButton && acceptButton == button)
            {
                artificiallyFocusedAcceptButton = false;
                button_LostFocus(button, null);
                lastSelectedControl?.Select();
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
            Button button = sender as Button;

            string text = buttonLabels[button];

            // Set flags to center text on the button.
            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;   // center the text

            // Render the text onto the button.
            TextRenderer.DrawText(e.Graphics, text, button.Font, e.ClipRectangle, button.ForeColor, flags);
        }

        public void button_TextChanged(object sender, EventArgs e)
        {
            Button button = sender as Button;
            string text = button.Text;

            if (!string.IsNullOrEmpty(text))
            {
                buttonLabels.AddReplace(button, text);

                if (UseSkinColors)
                {
                    button.Text = string.Empty;
                    button.Refresh();
                }
            }
        }

        public void control_Enter(object sender, System.EventArgs e)
        {
            var control = sender as Control;
            if (!(control is Button) && !control.GetType().IsSubclassOf(typeof(Button)) && control.Controls.Count == 0)
                lastSelectedControl = control;
        }

        //Returns X referred controls array, int - array of parent levelX if control2X is ancestor, otherwise 0,
        //then the same for Y, then scaled/moved marks and remarks
        public static (Control[], int[], Control[], int[], bool, bool, bool, bool, string[]) GetReferredControlControlLevelMarksRemarks(Control control)
        {
            (_, _, _, bool scaledMovedL, bool scaledMovedR, bool scaledMovedT, bool scaledMovedB, string[] remarks) = GetControlTagReferredNamesMarksRemarks(control);
            (Control[] controls2X, int[] levelsX, Control[] controls2Y, int[] levelsY) = GetReferredControls(control);

            return (controls2X, levelsX, controls2Y, levelsY, scaledMovedL, scaledMovedR, scaledMovedT, scaledMovedB, remarks);
        }


        public static void SetControlTagReferredNameMarksRemarks(Control control, string tagValue,
            bool scaledMovedL = false, bool scaledMovedR = false, bool scaledMovedT = false, bool scaledMovedB = false,
            string[] control2NamesX = null, string[] control2NamesY = null, params string[] remarks)
        {
            (_, string[] oldControl2NamesXArray, string[] oldControl2NamesYArray, bool scaledMovedOldL, bool scaledMovedOldR, bool scaledMovedOldT, bool scaledMovedOldB, string[] oldRemarks) = 
                GetControlTagReferredNamesMarksRemarks(control);

            List<string> oldControl2NamesX = oldControl2NamesXArray.ToList();
            List<string> oldControl2NamesY = oldControl2NamesYArray.ToList();


            if (control2NamesX != null)
            {
                for (int i = 0; i < control2NamesX.Length; i++)
                {
                    if (!oldControl2NamesX.Contains(control2NamesX[i]))
                        oldControl2NamesX.Add(control2NamesX[i]);
                }
            }

            string control2NameX = string.Empty;
            foreach (var controlName in oldControl2NamesX)
                control2NameX += "|" + controlName;

            control2NameX.TrimEnd('|');


            if (control2NamesY != null)
            {
                for (int i = 0; i < control2NamesY.Length; i++)
                {
                    if (!oldControl2NamesY.Contains(control2NamesY[i]))
                        oldControl2NamesY.Add(control2NamesY[i]);
                }
            }

            string control2NameY = string.Empty;
            foreach (var controlName in oldControl2NamesY)
                control2NameY += "|" + controlName;

            control2NameY.TrimEnd('|');


            scaledMovedL |= scaledMovedOldL;
            scaledMovedR |= scaledMovedOldR;
            scaledMovedT |= scaledMovedOldT;
            scaledMovedB |= scaledMovedOldB;


            if (remarks.Length == 0)
                remarks = oldRemarks;

            control.Tag = tagValue ?? string.Empty;

            if (!string.IsNullOrEmpty(control2NameX))
                control.Tag += "#" + control2NameX;

            if (!string.IsNullOrEmpty(control2NameY))
                control.Tag += "&" + control2NameY;

            if (scaledMovedL)
                control.Tag += "#scaled-moved-left";

            if (scaledMovedR)
                control.Tag += "#scaled-moved-right";

            if (scaledMovedT)
                control.Tag += "#scaled-moved-top";

            if (scaledMovedB)
                control.Tag += "#scaled-moved-bottom";

            if (remarks.Length != 0)
                control.Tag += remarks.Aggregate(string.Empty, (arg1, arg2) => arg1 + "@" + arg2);
        }

        //Returns initial tag value, referred X control names array, referred Y control names array, moved-scaled marks and array of custom remarks
        public static (string, string[], string[], bool, bool, bool, bool, string[]) GetControlTagReferredNamesMarksRemarks(Control control)
        {
            if (control.Tag == null || !(control.Tag is string))
                return (string.Empty, new string[0], new string[0], false, false, false, false, new string[0]);

            string controlTag = control.Tag as string;
            bool scaledMovedL = controlTag.Contains("#scaled-moved-left");
            bool scaledMovedR = controlTag.Contains("#scaled-moved-right");
            bool scaledMovedT = controlTag.Contains("#scaled-moved-top");
            bool scaledMovedB = controlTag.Contains("#scaled-moved-bottom");
            controlTag = controlTag.Replace("#scaled-moved-left", string.Empty);
            controlTag = controlTag.Replace("#scaled-moved-right", string.Empty);
            controlTag = controlTag.Replace("#scaled-moved-top", string.Empty);
            controlTag = controlTag.Replace("#scaled-moved-bottom", string.Empty);

            string tagValue = controlTag;
            string remarks = string.Empty;
            string control2NameX = controlTag;
            string control2NameY = string.Empty;

            //Optional tag value by itself
            if (controlTag.Contains("#"))
            {
                control2NameX = Regex.Replace(controlTag, "^(.*?)#(.*)", "$2");
                tagValue = controlTag.Replace("#" + control2NameX, string.Empty);
            }

            //Optional custom remark
            if (control2NameX.Contains("@"))
            {
                remarks = Regex.Replace(control2NameX, "^(.*?)@(.*)", "$2");
                control2NameX = control2NameX.Replace("@" + remarks, string.Empty);
            }

            //control2NameY reference
            if (control2NameX.Contains("&"))
            {
                control2NameY = Regex.Replace(control2NameX, "^(.*?)&(.*)", "$2");
                control2NameX = control2NameX.Replace("&" + control2NameY, string.Empty);
            }

            if (tagValue.Contains("@"))
            {
                remarks = Regex.Replace(tagValue, "^(.*?)@(.*)", "$2");
                tagValue = tagValue.Replace("@" + remarks, string.Empty);
            }

            var control2NamesX = control2NameX.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            var control2NamesY = control2NameY.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            if (remarks == string.Empty)
                return (tagValue, control2NamesX, control2NamesY, scaledMovedL, scaledMovedR, scaledMovedT, scaledMovedB, new string[0]);
            else
                return (tagValue, control2NamesX, control2NamesY, scaledMovedL, scaledMovedR, scaledMovedT, scaledMovedB, remarks.Split('@'));
        }

        public static (Control, int) GetReferredControlByName(Control control, string control2Name) //int - parent levelX if control2X is ancestor, otherwise 0
        {
            if (string.IsNullOrEmpty(control2Name))
                return (null, 0);

            int levelX = 0;
            Control control2X = control.Parent.Controls[control2Name];

            Control parent = control.Parent;
            while (control2X == null && parent != null)
            {
                levelX++;

                if (parent.Name == control2Name)
                    control2X = parent;
                else
                    parent = parent.Parent;
            }

            if (control2X == null)
                levelX = 0;


            return (control2X, levelX);
        }

        //Returns X referred controls array, int - array of parent levelX if control2X is ancestor, otherwise 0, then the same for Y
        public static (Control[], int[], Control[], int[]) GetReferredControls(Control control)
        {
            (_, string[] control2NamesX, string[] control2NamesY, _, _, _, _, _) = 
                GetControlTagReferredNamesMarksRemarks(control);

            Control[] controls2X = new Control[control2NamesX.Length];
            int[] levelsX = new int[control2NamesX.Length];
            for (int i = 0; i < control2NamesX.Length; i++)
            {
                (Control control2X, int levelX) = GetReferredControlByName(control, control2NamesX[i]);
                controls2X[i] = control2X;
                levelsX[i] = levelX;
            }

            Control[] controls2Y = new Control[control2NamesY.Length];
            int[] levelsY = new int[control2NamesY.Length];
            for (int i = 0; i < control2NamesY.Length; i++)
            {
                (Control control2Y, int levelY) = GetReferredControlByName(control, control2NamesY[i]);
                controls2Y[i] = control2Y;
                levelsY[i] = levelY;
            }

            return (controls2X, levelsX, controls2Y, levelsY);
        }

        public static Size GetControlSize(Control control)
        {
            if (control.GetType().IsSubclassOf(typeof(Form)) || control is Form)
                return (control as Form).ClientSize;
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

        public static (int, int, int, int) GetControl2LeftRightMarginsOrParent0WidthPaddings(Control control, Control control2X, bool pinnedToParentX = false) //Returns: Left, Right or 0 & parent Width, margins or parent padding
        {
            if (control2X == null && pinnedToParentX)
                return (0, GetControlSize(control.Parent).Width, control.Parent.Padding.Left, control.Parent.Padding.Right);


            int levelX = GetControlLevel(control, control2X);

            if (levelX > 0)
                return (GetControlSize(control2X).Width, GetControlSize(control2X).Width, control2X.Padding.Right, control2X.Padding.Left);
            else //if (levelX == 0)
                return (control2X.Left, control2X.Left + control2X.Width, control2X.Margin.Left, control2X.Margin.Right);
        }

        public static (int, int, int, int) GetControl2TopBottomMarginsOrParent0HeightPaddings(Control control, Control control2Y, bool pinnedToParentY = false) //Returns: Top, Bottom or 0 & parent Height, margins or parent padding
        {
            if (control2Y == null && pinnedToParentY)
                return (0, GetControlSize(control.Parent).Height, control.Parent.Padding.Top, control.Parent.Padding.Bottom);


            int levelY = GetControlLevel(control, control2Y);

            if (levelY > 0)
                return (GetControlSize(control2Y).Height, GetControlSize(control2Y).Height, control2Y.Padding.Bottom, control2Y.Padding.Top);
            else //if (levelY == 0)
                return (control2Y.Top, control2Y.Top + control2Y.Height, control2Y.Margin.Top, control2Y.Margin.Bottom);
        }

        public int getRightAnchoredControlX(Control control, int controlNewWidth, Control control2X, bool pinnedToParentX, int levelX, float offset = 0)
        {
            if (pinnedToParentX)
            {
                return GetControlSize(control.Parent).Width - controlNewWidth - (control.Margin.Right + control.Parent.Padding.Right);
            }
            else if (levelX == 0)
            {
                if (offset == 0)
                    return control2X.Left - controlNewWidth - (control.Margin.Right + control2X.Margin.Left);
                else
                    return (int)Math.Round(control2X.Left - controlNewWidth - offset * hDpiFontScaling);
            }
            else
            {
                return GetControlSize(control2X).Width - controlNewWidth - (control.Margin.Right + control2X.Padding.Right);
            }
        }

        public int getBottomAnchoredControlY(Control control, int controlNewHeight, Control control2Y, bool pinnedToParentY, int levelY, float offset = 0)
        {
            if (pinnedToParentY)
            {
                return GetControlSize(control.Parent).Height - controlNewHeight - (control.Margin.Bottom + control.Parent.Padding.Bottom);
            }
            else if (levelY == 0)
            {
                if (offset == 0)
                    return control2Y.Top - controlNewHeight - (control.Margin.Bottom + control2Y.Margin.Top);
                else
                    return (int)Math.Round(control2Y.Top - controlNewHeight - offset * vDpiFontScaling);
            }
            else
            {
                return GetControlSize(control2Y).Height - controlNewHeight - (control.Margin.Bottom + control2Y.Padding.Bottom);
            }
        }

        public static float GetYCenteredToY2(float controlHeight, float refControlY, Control control2X)
        {
            float control2MiddleY = refControlY + control2X.Height / 2f;
            return control2MiddleY - controlHeight / 2f;
        }

        public int getButtonY(Control control, AnchorStyles style)
        {
            float controlNewY;

            if ((control.Anchor & AnchorStyles.Top) == 0 && (control.Anchor & AnchorStyles.Bottom) == 0) //Not vertically anchored
                controlNewY = control.Top;
            else
                controlNewY = control.Top - (control.Height - control.Height / vDpiFontScaling) / 4f;//***

            return (int)Math.Round(controlNewY);
        }

        public int getLabelY(Control control)
        {
            float controlNewY = control.Top - (control.Height - control.Height / vDpiFontScaling) / 2;
            return (int)Math.Round(controlNewY);
        }

        public int getPictureBoxY(Control control, float scale)
        {
            float controlNewY = control.Top - (control.Height - control.Height / scale) / 2;
            return (int)Math.Round(controlNewY);
        }

        public int getPictureBoxY(float controlHeight, Control control2X)
        {
            float heightDifference = controlHeight - control2X.Height;
            float controlNewY = GetYCenteredToY2(controlHeight, control2X.Top, control2X);

            controlNewY -= heightDifference / 6 / vDpiFontScaling; //*****
            controlNewY += controlHeight * 0.07f;
            return (int)Math.Round(controlNewY);
        }

        public int getCheckBoxesRadioButtonsY(Control control, Control control2X)
        {
            if (control.Height == control2X.Height)
            {
                return control.Top;
            }
            else
            {
                float heightDifference = control.Height - control2X.Height;
                float controlNewY = GetYCenteredToY2(control.Height, control2X.Top, control2X);

                controlNewY -= heightDifference / 6 / vDpiFontScaling; //*****
                controlNewY += control.Height * 0.07f;
                return (int)Math.Round(controlNewY);
            }
        }

        //Let's correct flaws of AUTO-scaling
        public virtual void preMoveScaleControl(Control control)
        {
            if (control.GetType().IsSubclassOf(typeof(Form)) || control is Form)
                return;

            string stringTag = control.Tag as string;

            if (control.GetType().IsSubclassOf(typeof(Button)) || control is Button)
            {
                control.Top = getButtonY(control, control.Anchor);
            }
            else if (control.GetType().IsSubclassOf(typeof(PictureBox)) || control is PictureBox)
            {
                control.Width = (int)Math.Round(Math.Round(control.Width / hDpiFontScaling) * dpiScaling);
                control.Height = (int)Math.Round(Math.Round(control.Height / vDpiFontScaling) * dpiScaling);

                if (stringTag?.Contains('&') != true)
                    control.Top = getPictureBoxY(control, dpiScaling);
            }
            else if (control.GetType().IsSubclassOf(typeof(Label)) || control is Label)
            {
                control.Top = getLabelY(control);
            }


            //Control that was initially square (before AUTO-scaling) 
            if (stringTag?.Contains("@square-control") == true)
                control.Width = control.Height;
        }

        public void moveScaleControlDependentReferringControlsX(Control control, int control2XIndex = -1)
        {
            if (control.GetType().IsSubclassOf(typeof(Form)) || control is Form)
                return;


            const float smallControlOffsetX = 0.1f;
            const float checkBoxRadioButtonOffsetCompensationX = -12f;
            const float checkBoxRadioButtonAndPictureBoxOffsetCompensationX = -7f;//*****


            controlsReferencedX.TryGetValue(control, out Control referringControlX);

            (Control[] controls2X, int[] levelsX, _, _, bool scaledMovedL, bool scaledMovedR, bool scaledMovedT, bool scaledMovedB, string[] remarks) = GetReferredControlControlLevelMarksRemarks(control);
            bool pinnedToParentX = remarks.Contains("pinned-to-parent-x");


            //control is left & right anchored. Let's move/scale it at the end (in another function).
            if ((control.Anchor & AnchorStyles.Left) != 0 && (control.Anchor & AnchorStyles.Right) != 0)
            {
                if (!leftRightAnchoredControls.Contains(control))
                    leftRightAnchoredControls.Add(control);

                return;
            }


            //control is left anchored
            if (referringControlX != null && (control.Anchor & AnchorStyles.Left) != 0 && (control.Anchor & AnchorStyles.Right) == 0)
                moveScaleControlDependentReferringControlsX(referringControlX);
            //referringControl is left anchored, control must be left & right anchored (left anchored only is considered above) 
            else if (referringControlX != null && (referringControlX.Anchor & AnchorStyles.Left) != 0 && (referringControlX.Anchor & AnchorStyles.Right) != 0)
                moveScaleControlDependentReferringControlsX(referringControlX);


            //Special case. Will move/scale it to parent. Let's proceed further...
            if (pinnedToParentX && control2XIndex != -1)
                ;
            //No controls2X and control is NOT left & right anchored (see above)
            else if (controls2X == null)
                return;
            //No controls2X and control is NOT left & right anchored (see above)
            else if (controls2X.Length == 0)
                return;
            else if (control2XIndex == -1)
            {
                for (int i = 0; i < controls2X.Length; i++)
                    moveScaleControlDependentReferringControlsX(control, i);

                return;
            }

            //controls2X[control2XIndex] is right anchored and control is NOT left & right anchored (see earlier)
            if ((controls2X[control2XIndex].Anchor & AnchorStyles.Left) == 0 && (controls2X[control2XIndex].Anchor & AnchorStyles.Right) != 0)
                moveScaleControlDependentReferringControlsX(controls2X[control2XIndex]);



            if (scaledMovedL && scaledMovedR && scaledMovedT)
                return;


            bool scaledMovedL2 = true;
            bool scaledMovedR2 = true;

            if (controls2X[control2XIndex] != null)
                (_, _, _, scaledMovedL2, scaledMovedR2, _, _, _) = GetControlTagReferredNamesMarksRemarks(controls2X[control2XIndex]);


            //VERTICAL MOVEMENT OF HORIZONTALLY LINKED CONTROLS
            if (control.GetType().IsSubclassOf(typeof(CheckBox)) || control is CheckBox ||
                        control.GetType().IsSubclassOf(typeof(RadioButton)) || control is RadioButton
            )
            {
                if (!scaledMovedT)
                {
                    if (controls2X[control2XIndex].GetType().IsSubclassOf(typeof(Label)) || controls2X[control2XIndex] is Label)
                        control.Top = getCheckBoxesRadioButtonsY(control, controls2X[control2XIndex]);
                    else
                        control.Top = (int)Math.Round(GetYCenteredToY2(control.Height, controls2X[control2XIndex].Top, controls2X[control2XIndex]));

                    scaledMovedT = true;
                }
            }
            else if (control.GetType().IsSubclassOf(typeof(PictureBox)) || control is PictureBox)
            {
                if (controls2X[control2XIndex].GetType().IsSubclassOf(typeof(Label)) || controls2X[control2XIndex] is Label)
                {
                    if (!scaledMovedT)
                    {
                        control.Top = getPictureBoxY(control.Height, controls2X[control2XIndex]);
                        scaledMovedT = true;
                    }
                }
            }


            //control left anchored
            if ((control.Anchor & AnchorStyles.Left) != 0 && (control.Anchor & AnchorStyles.Right) == 0)
            {
                if (pinnedToParentX || levelsX[control2XIndex] > 0)
                {
                    if (!scaledMovedL)
                    {
                        var parent = controls2X[control2XIndex];
                        if (parent == null || pinnedToParentX)
                            parent = control.Parent;

                        control.Left = parent.Padding.Left + control.Margin.Left;
                        scaledMovedL = true;
                    }
                }

                if (controls2X[control2XIndex] != null)
                {
                    if (control.GetType().IsSubclassOf(typeof(CheckBox)) || control is CheckBox ||
                                control.GetType().IsSubclassOf(typeof(RadioButton)) || control is RadioButton
                    )
                    {
                        if (!scaledMovedL2)
                        {
                            controls2X[control2XIndex].Left = (int)Math.Round(control.Left + control.Width + checkBoxRadioButtonOffsetCompensationX * hDpiFontScaling);
                            scaledMovedL2 = true;
                        }
                    }
                    else if (control.GetType().IsSubclassOf(typeof(PictureBox)) || control is PictureBox)
                    {
                        if (!scaledMovedL2)
                        {
                            if (controls2X[control2XIndex].GetType().IsSubclassOf(typeof(Label)) || controls2X[control2XIndex] is Label)
                                controls2X[control2XIndex].Left = (int)Math.Round(control.Left + control.Width + smallControlOffsetX * hDpiFontScaling);
                            else
                                controls2X[control2XIndex].Left = control.Left + control.Width + control.Margin.Right + controls2X[control2XIndex].Margin.Left;

                            scaledMovedL2 = true;
                        }
                    }
                    //controls2X[control2XIndex] left anchored
                    else if ((controls2X[control2XIndex].Anchor & AnchorStyles.Left) != 0 && (controls2X[control2XIndex].Anchor & AnchorStyles.Right) == 0)
                    {
                        if (!scaledMovedL2)
                        {
                            controls2X[control2XIndex].Left = control.Left + control.Width + controls2X[control2XIndex].Margin.Left + control.Margin.Right;
                            scaledMovedL2 = true;
                        }
                    }
                    //controls2X[control2XIndex] must NOT be right anchored. See below.
                    else if ((controls2X[control2XIndex].Anchor & AnchorStyles.Left) == 0 && (controls2X[control2XIndex].Anchor & AnchorStyles.Right) != 0)
                    {
                        throw new Exception("Invalid controls2X[control2XIndex] anchoring for control: " + control.Name + "! Expected anchor is not right.");
                    }
                    //controls2X[control2XIndex] must NOT be not anchored
                    else if ((controls2X[control2XIndex].Anchor & AnchorStyles.Left) == 0 && (controls2X[control2XIndex].Anchor & AnchorStyles.Right) == 0)
                    {
                        throw new Exception("Invalid controls2X[control2XIndex] anchoring for control: " + control.Name + "! Expected anchor is not none.");
                    }
                }
            }
            //control right anchored
            else if ((control.Anchor & AnchorStyles.Left) == 0 && (control.Anchor & AnchorStyles.Right) != 0)
            {
                if (pinnedToParentX || levelsX[control2XIndex] > 0)
                {
                    if (!scaledMovedL)
                    {
                        var parent = controls2X[control2XIndex];
                        if (parent == null || pinnedToParentX)
                            parent = control.Parent;

                        control.Left = GetControlSize(parent).Width - control.Width - (parent.Padding.Right + control.Margin.Right);
                        scaledMovedL = true;
                    }
                }
                else if (control.GetType().IsSubclassOf(typeof(CheckBox)) || control is CheckBox ||
                    control.GetType().IsSubclassOf(typeof(RadioButton)) || control is RadioButton
                )
                {
                    if (!scaledMovedL)
                    {
                        if (controls2X[control2XIndex].GetType().IsSubclassOf(typeof(Label)) || controls2X[control2XIndex] is Label)
                            control.Left = getRightAnchoredControlX(control, control.Width, controls2X[control2XIndex], pinnedToParentX, levelsX[control2XIndex], checkBoxRadioButtonOffsetCompensationX * hDpiFontScaling);
                        else
                            control.Left = getRightAnchoredControlX(control, control.Width, controls2X[control2XIndex], pinnedToParentX, levelsX[control2XIndex]);

                        scaledMovedL = true;
                    }
                    else if (!scaledMovedL2)
                    {
                        int controlXPlaceholder;

                        if (controls2X[control2XIndex].GetType().IsSubclassOf(typeof(Label)) || controls2X[control2XIndex] is Label)
                            controlXPlaceholder = getRightAnchoredControlX(control, control.Width, controls2X[control2XIndex], pinnedToParentX, levelsX[control2XIndex], checkBoxRadioButtonOffsetCompensationX * hDpiFontScaling);
                        else if (controls2X[control2XIndex].GetType().IsSubclassOf(typeof(PictureBox)) || controls2X[control2XIndex] is PictureBox)
                            controlXPlaceholder = getRightAnchoredControlX(control, control.Width, controls2X[control2XIndex], pinnedToParentX, levelsX[control2XIndex], checkBoxRadioButtonAndPictureBoxOffsetCompensationX * hDpiFontScaling);
                        else
                            controlXPlaceholder = getRightAnchoredControlX(control, control.Width, controls2X[control2XIndex], pinnedToParentX, levelsX[control2XIndex]);

                        int distance = controlXPlaceholder - controls2X[control2XIndex].Left;
                        controls2X[control2XIndex].Left = control.Left - distance;

                        scaledMovedL2 = true;
                    }
                }
                else if (control.GetType().IsSubclassOf(typeof(PictureBox)) || control is PictureBox)
                {
                    if (!scaledMovedL)
                    {
                        if (controls2X[control2XIndex].GetType().IsSubclassOf(typeof(Label)) || controls2X[control2XIndex] is Label)
                            control.Left = getRightAnchoredControlX(control, control.Width, controls2X[control2XIndex], pinnedToParentX, levelsX[control2XIndex], smallControlOffsetX);
                        else
                            control.Left = getRightAnchoredControlX(control, control.Width, controls2X[control2XIndex], pinnedToParentX, levelsX[control2XIndex], control.Margin.Right + controls2X[control2XIndex].Margin.Left);

                        scaledMovedL = true;
                    }
                }
                //controls2X[control2XIndex] right anchored
                else if ((controls2X[control2XIndex].Anchor & AnchorStyles.Left) == 0 && (controls2X[control2XIndex].Anchor & AnchorStyles.Right) != 0)
                {
                    if (!scaledMovedL)
                    {
                        control.Left = getRightAnchoredControlX(control, control.Width, controls2X[control2XIndex], pinnedToParentX, levelsX[control2XIndex]);
                        scaledMovedL = true;
                    }
                }
                //controls2X[control2XIndex] must NOT be left anchored. See below.
                else if ((controls2X[control2XIndex].Anchor & AnchorStyles.Left) != 0 && (controls2X[control2XIndex].Anchor & AnchorStyles.Right) == 0)
                {
                    throw new Exception("Invalid controls2X[control2XIndex] anchoring for control: " + control.Name + "! Expected anchor is not left.");
                }
                //controls2X[control2XIndex] must NOT be not anchored
                else if ((controls2X[control2XIndex].Anchor & AnchorStyles.Left) == 0 && (controls2X[control2XIndex].Anchor & AnchorStyles.Right) == 0)
                {
                    throw new Exception("Invalid controls2X[control2XIndex] anchoring for control: " + control.Name + "! Expected anchor is not none.");
                }
            }


            SetControlTagReferredNameMarksRemarks(control, null, scaledMovedL, scaledMovedR, scaledMovedT);

            if (controls2X[control2XIndex] != null)
                SetControlTagReferredNameMarksRemarks(controls2X[control2XIndex], null, scaledMovedL2, scaledMovedR2);
        }

        public void moveScaleControlDependentReferringControlsY(Control control, int control2YIndex = -1)
        {
            if (control.GetType().IsSubclassOf(typeof(Form)) || control is Form)
                return;


            controlsReferencedY.TryGetValue(control, out Control referringControlY);

            (_, _, Control[] controls2Y, int[] levelsY, _, _, bool scaledMovedT, bool scaledMovedB, string[] remarks) = GetReferredControlControlLevelMarksRemarks(control);
            bool pinnedToParentY = remarks.Contains("pinned-to-parent-y");

            //control is top & bottom anchored. Let's move/scale it at the end (in another function).
            if ((control.Anchor & AnchorStyles.Top) != 0 && (control.Anchor & AnchorStyles.Bottom) != 0)
            {
                if (!topBottomAnchoredControls.Contains(control))
                    topBottomAnchoredControls.Add(control);

                return;
            }


            //control is top anchored
            if (referringControlY != null && (control.Anchor & AnchorStyles.Top) != 0 && (control.Anchor & AnchorStyles.Bottom) == 0)
                moveScaleControlDependentReferringControlsY(referringControlY);
            //referringControl is top anchored, control must be top & bottom anchored (top anchored only is considered above) 
            else if (referringControlY != null && (referringControlY.Anchor & AnchorStyles.Top) != 0 && (referringControlY.Anchor & AnchorStyles.Bottom) != 0)
                moveScaleControlDependentReferringControlsY(referringControlY);


            if (pinnedToParentY && control2YIndex != -1)
                ;
            //No controls2Y and control is NOT top & bottom anchored (see above)
            else if (controls2Y == null)
                return;
            //No controls2Y and control is NOT top & bottom anchored (see above)
            else if (controls2Y.Length == 0)
                return;
            else if (control2YIndex == -1)
            {
                for (int i = 0; i < controls2Y.Length; i++)
                    moveScaleControlDependentReferringControlsY(control, i);

                return;
            }

            //controls2Y[control2YIndex] is bottom anchored and control is NOT top & bottom anchored (see earlier)
            if ((controls2Y[control2YIndex].Anchor & AnchorStyles.Top) == 0 && (controls2Y[control2YIndex].Anchor & AnchorStyles.Bottom) != 0)
                moveScaleControlDependentReferringControlsX(controls2Y[control2YIndex]);



            if (scaledMovedT && scaledMovedB)
                return;


            bool scaledMovedT2 = true;
            bool scaledMovedB2 = true;

            if (controls2Y[control2YIndex] != null)
                (_, _, _, _, _, scaledMovedT2, scaledMovedB2, _) = GetControlTagReferredNamesMarksRemarks(controls2Y[control2YIndex]);


            //control top anchored
            if ((control.Anchor & AnchorStyles.Top) != 0 && (control.Anchor & AnchorStyles.Bottom) == 0)
            {
                if (pinnedToParentY || levelsY[control2YIndex] > 0)
                {
                    if (!scaledMovedT)
                    {
                        var parent = controls2Y[control2YIndex];
                        if (parent == null)
                            parent = control.Parent;

                        control.Top = parent.Padding.Top + control.Margin.Top;
                        scaledMovedT = true;
                    }
                }

                if (controls2Y[control2YIndex] != null)
                {
                    //controls2Y[control2YIndex] top anchored
                    if ((controls2Y[control2YIndex].Anchor & AnchorStyles.Top) != 0 && (controls2Y[control2YIndex].Anchor & AnchorStyles.Bottom) == 0)
                    {
                        if (!scaledMovedT2)
                        {
                            controls2Y[control2YIndex].Top = control.Top + control.Height + controls2Y[control2YIndex].Margin.Top + control.Margin.Bottom;
                            scaledMovedT2 = true;
                        }
                    }
                    //controls2Y[control2YIndex] must NOT be bottom anchored. See below.
                    else if ((controls2Y[control2YIndex].Anchor & AnchorStyles.Top) == 0 && (controls2Y[control2YIndex].Anchor & AnchorStyles.Bottom) != 0)
                    {
                        throw new Exception("Invalid controls2Y[control2YIndex] anchoring for control: " + control.Name + "! Expected anchor is not bottom.");
                    }
                    //controls2Y[control2YIndex] is NOT anchored
                    else if ((controls2Y[control2YIndex].Anchor & AnchorStyles.Top) == 0 && (controls2Y[control2YIndex].Anchor & AnchorStyles.Bottom) == 0)
                    {
                        if (!scaledMovedT2)
                        {
                            float middleY = control.Top + control.Height / 2f;
                            float newControlT2 = middleY - controls2Y[control2YIndex].Height / 2f + vDpiFontScaling * 2f; //*****

                            controls2Y[control2YIndex].Top = (int)Math.Round(newControlT2);
                            scaledMovedT2 = true;
                        }
                    }
                }
            }
            //control bottom anchored
            else if ((control.Anchor & AnchorStyles.Top) == 0 && (control.Anchor & AnchorStyles.Bottom) != 0)
            {
                if (pinnedToParentY || levelsY[control2YIndex] > 0)
                {
                    if (!scaledMovedT)
                    {
                        var parent = controls2Y[control2YIndex];
                        if (parent == null)
                            parent = control.Parent;

                        control.Top = GetControlSize(parent).Height - control.Height - (parent.Padding.Bottom + control.Margin.Bottom);
                        scaledMovedT = true;
                    }
                }
                //controls2Y[control2YIndex] bottom anchored
                else if ((controls2Y[control2YIndex].Anchor & AnchorStyles.Top) == 0 && (controls2Y[control2YIndex].Anchor & AnchorStyles.Bottom) != 0)
                {
                    if (!scaledMovedT)
                    {
                        control.Top = getBottomAnchoredControlY(control, control.Height, controls2Y[control2YIndex], pinnedToParentY, levelsY[control2YIndex]);
                        scaledMovedT = true;
                    }
                }
                //controls2Y[control2YIndex] must NOT be top anchored. See below.
                else if ((controls2Y[control2YIndex].Anchor & AnchorStyles.Top) != 0 && (controls2Y[control2YIndex].Anchor & AnchorStyles.Bottom) == 0)
                {
                    throw new Exception("Invalid controls2Y[control2YIndex] anchoring for control: " + control.Name + "! Expected anchor is not top.");
                }
                //controls2Y[control2YIndex] must NOT be not anchored
                else if ((controls2Y[control2YIndex].Anchor & AnchorStyles.Top) == 0 && (controls2Y[control2YIndex].Anchor & AnchorStyles.Bottom) == 0)
                {
                    throw new Exception("Invalid controls2Y[control2YIndex] anchoring for control: " + control.Name + "! Expected anchor is not none.");
                }
            }


            SetControlTagReferredNameMarksRemarks(control, null, false, false, scaledMovedT, scaledMovedB);

            if (controls2Y[control2YIndex] != null)
                SetControlTagReferredNameMarksRemarks(controls2Y[control2YIndex], null, false, false, scaledMovedT2, scaledMovedB2);
        }

        public void scaleMoveLeftRightAnchoredControls()
        {
            foreach (var control in leftRightAnchoredControls)
            {
                controlsReferencedX.TryGetValue(control, out var controlReferencing);
                controlsReferencesX.TryGetValue(control, out var controlReference);

                (_, _, _, _, _, _, _, string[] remarks) = GetControlTagReferredNamesMarksRemarks(control);
                bool pinnedToParentX = remarks.Contains("pinned-to-parent-x");
                int controlReferencingLevel = GetControlLevel(controlReferencing, control);

                int controlRNew;

                if (controlReferencing != null && controlReferencingLevel == 0 && controlReference != null)
                {
                    (int controlReferencingLeft, int controlReferencingRight, int controlReferencingMarginLeft, int controlReferencingMarginRight) = GetControl2LeftRightMarginsOrParent0WidthPaddings(control, controlReferencing, pinnedToParentX);
                    (int controlReferenceLeft, int controlReferenceRight, int controlReferenceMarginLeft, int controlReferenceMarginRight) = GetControl2LeftRightMarginsOrParent0WidthPaddings(control, controlReference, pinnedToParentX);

                    controlRNew = controlReferenceLeft - (controlReferenceMarginLeft + control.Margin.Right);
                    control.Left = controlReferencingRight + (controlReferencingMarginRight + control.Margin.Left);
                    control.Width = controlRNew - control.Left;

                }
                else if (controlReferencing != null && controlReferencingLevel == 0 && controlReference == null)
                {
                    (int controlReferencingLeft, int controlReferencingRight, int controlReferencingMarginLeft, int controlReferencingMarginRight) = GetControl2LeftRightMarginsOrParent0WidthPaddings(control, controlReferencing, pinnedToParentX);

                    controlRNew = control.Left + control.Width;
                    control.Left = controlReferencingLeft + (controlReferencingMarginRight + control.Margin.Left);
                    control.Width = controlRNew - control.Left;
                }
                else if ((controlReferencing == null || controlReferencingLevel > 0) && controlReference != null)
                {
                    (int controlReferenceLeft, int controlReferenceRight, int controlReferenceMarginLeft, int controlReferenceMarginRight) = GetControl2LeftRightMarginsOrParent0WidthPaddings(control, controlReference, pinnedToParentX);

                    if (pinnedToParentX && GetControlLevel(control, controlReference) > 0) //Pinned to controlReference (i.e. controlReference is a parent)
                    {
                        //(int controlReferencingLeft, int controlReferencingRight, int controlReferencingMarginLeft, int controlReferencingMarginRight) = GetControl2LeftRightMarginsOrParent0WidthPaddings(control, control.Parent, true);

                        control.Left = controlReferenceMarginRight + control.Margin.Left;
                        controlRNew = controlReferenceRight - (controlReferenceMarginRight + control.Margin.Right);
                    }
                    else
                    {
                        controlRNew = controlReferenceLeft - (controlReferenceMarginLeft + control.Margin.Right);
                    }

                    control.Width = controlRNew - control.Left;
                }
                else //if (controlReferencing == null && controlReference == null)
                {
                    //Nothing to do. Lets keep auto-scaled scaling/position... 
                }
            }
        }

        public void scaleMoveTopBottomAnchoredControls()
        {
            foreach (var control in topBottomAnchoredControls)
            {
                controlsReferencedY.TryGetValue(control, out var controlReferencing);
                controlsReferencesY.TryGetValue(control, out var controlReference);

                (_, _, _, _, _, _, _, string[] remarks) = GetControlTagReferredNamesMarksRemarks(control);
                bool pinnedToParentY = remarks.Contains("pinned-to-parent-y");
                int controlReferencingLevel = GetControlLevel(controlReferencing, control);

                int controlRNew;

                if (controlReferencing != null && controlReferencingLevel == 0 && controlReference != null)
                {
                    (int controlReferencingTop, int controlReferencingBottom, int controlReferencingMarginTop, int controlReferencingMarginBottom) = GetControl2TopBottomMarginsOrParent0HeightPaddings(control, controlReferencing, pinnedToParentY);
                    (int controlReferenceTop, int controlReferenceBottom, int controlReferenceMarginTop, int controlReferenceMarginBottom) = GetControl2TopBottomMarginsOrParent0HeightPaddings(control, controlReference, pinnedToParentY);

                    controlRNew = controlReferenceTop - (controlReferenceMarginTop + control.Margin.Bottom);
                    control.Top = controlReferencingBottom + (controlReferencingMarginBottom + control.Margin.Top);
                    control.Height = controlRNew - control.Top;

                }
                else if (controlReferencing != null && controlReferencingLevel == 0 && controlReference == null)
                {
                    (int controlReferencingTop, int controlReferencingBottom, int controlReferencingMarginTop, int controlReferencingMarginBottom) = GetControl2TopBottomMarginsOrParent0HeightPaddings(control, controlReferencing, pinnedToParentY);

                    controlRNew = control.Top + control.Height;
                    control.Top = controlReferencingTop + (controlReferencingMarginBottom + control.Margin.Top);
                    control.Height = controlRNew - control.Top;
                }
                else if ((controlReferencing == null || controlReferencingLevel > 0) && controlReference != null)
                {
                    (int controlReferenceTop, int controlReferenceBottom, int controlReferenceMarginTop, int controlReferenceMarginBottom) = GetControl2TopBottomMarginsOrParent0HeightPaddings(control, controlReference, pinnedToParentY);

                    if (pinnedToParentY && GetControlLevel(control, controlReference) > 0) //Pinned to controlReference (i.e. controlReference is a parent)
                    {
                        //(int controlReferencingTop, int controlReferencingBottom, int controlReferencingMarginTop, int controlReferencingMarginBottom) = GetControl2TopBottomMarginsOrParent0HeightPaddings(control, control.Parent, true);

                        control.Top = controlReferenceMarginBottom + control.Margin.Top;
                        controlRNew = controlReferenceBottom - (controlReferenceMarginBottom + control.Margin.Bottom);
                    }
                    else
                    {
                        controlRNew = controlReferenceTop - (controlReferenceMarginTop + control.Margin.Bottom);
                    }

                    control.Height = controlRNew - control.Top;
                }
                else //if (controlReferencing == null && controlReference == null)
                {
                    //Nothing to do. Lets keep auto-scaled scaling/position... 
                }
            }
        }

        public void skinControl(Control control)
        {
            //if (control.GetType().IsSubclassOf(typeof(TabControl)) || control is TabControl)
            //{
            //    TabControl tabControl = control as TabControl;
            //    tabControl.DrawItem += form.TabControl_DrawItem;
            //
            //    return;
            //}

            if (control.GetType().IsSubclassOf(typeof(SplitContainer)) || control is SplitContainer)
            {
                SplitContainer splitContainer = (SplitContainer)control;

                splitContainer.Panel1.BackColor = FormBackColor;
                splitContainer.Panel1.ForeColor = AccentColor;

                splitContainer.Panel2.BackColor = FormBackColor;
                splitContainer.Panel2.ForeColor = AccentColor;

                splitContainer.BackColor = SplitterColor;

                return;
            }

            if (control.GetType().IsSubclassOf(typeof(Button)) || control is Button)
            {
                Button button = control as Button;

                buttonLabels.AddReplace(button, button.Text);


                button.TextChanged += button_TextChanged;
                button.GotFocus += button_GotFocus;
                button.LostFocus += button_LostFocus;


                if (!UseSkinColors)
                {
                    button.FlatStyle = FlatStyle.Standard;
                    buttonLabels.AddReplace(button, button.Text);
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
            if (!UseSkinColors)
                return;


            if (control.GetType().IsSubclassOf(typeof(TextBox)) || control is TextBox)
            {
                control.BackColor = InputControlBackColor;
                control.ForeColor = InputControlForeColor;

                (control as TextBox).BorderStyle = BorderStyle.FixedSingle;
            }
            else if (control.GetType().IsSubclassOf(typeof(ComboBox)) || control is ComboBox)
            {
                ComboBox comboBox = control as ComboBox;

                control.BackColor = InputControlBackColor;
                control.ForeColor = InputControlForeColor;

                comboBox.FlatStyle = FlatStyle.Flat;

                //For rendering high contrast cue banners. Works with DropDownLists only.
                comboBox.DrawItem += comboBox_DrawItem;
                comboBox.DrawMode = DrawMode.OwnerDrawFixed;

                if (comboBox.DropDownStyle == ComboBoxStyle.DropDown) //DropDown looks ugly if disabled. Let's handle 
                {
                    dropDownStyleComboBox.Add(comboBox);

                    comboBox.EnabledChanged += comboBox_EnabledChanged;
                    comboBox_EnabledChanged(comboBox, null);
                }
            }
            else if (control.GetType().IsSubclassOf(typeof(ListBox)) || control is ListBox)
            {
                control.BackColor = InputControlBackColor;
                control.ForeColor = InputControlForeColor;

                (control as ListBox).BorderStyle = BorderStyle.FixedSingle;
            }
            else if (control.GetType().IsSubclassOf(typeof(Label)) || control is Label)
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
            else if (control.GetType().IsSubclassOf(typeof(GroupBox)) || control is GroupBox)
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

                (control as GroupBox).FlatStyle = FlatStyle.Standard;
            }
            else if (control.GetType().IsSubclassOf(typeof(CheckBox)) || control is CheckBox)
            {
                control.BackColor = FormBackColor;
                control.ForeColor = FormForeColor;

                (control as CheckBox).FlatStyle = FlatStyle.System;
            }
            else if (control.GetType().IsSubclassOf(typeof(RadioButton)) || control is RadioButton)
            {
                control.BackColor = FormBackColor;
                control.ForeColor = FormForeColor;

                (control as RadioButton).FlatStyle = FlatStyle.System;
            }
            else if (control is DataGridView)
            {
                control.BackColor = HeaderCellStyle.BackColor;
                control.ForeColor = HeaderCellStyle.ForeColor;

                (control as DataGridView).EnableHeadersVisualStyles = false;
                (control as DataGridView).BorderStyle = BorderStyle.FixedSingle;
                (control as DataGridView).ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                (control as DataGridView).BackgroundColor = UnchangedCellStyle.BackColor;
                (control as DataGridView).DefaultCellStyle = UnchangedCellStyle;
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

                (Control[] controls2X, int[] levelsX, Control[] controls2Y, int[] levelsY, _, _, _, _, string[] remarks) = GetReferredControlControlLevelMarksRemarks(control);

                if (controls2X != null)
                {
                    for (int j = 0; j < controls2X.Length; j++)
                    {
                        controlsReferencesX.AddReplace(control, controls2X[j]);
                        controlsReferencedX.AddReplace(controls2X[j], control);
                    }
                }

                if (remarks.Contains("pinned-to-parent-x") && !pinnedToParentControlsX.Contains(control))
                    pinnedToParentControlsX.Add(control);


                if (controls2Y != null)
                {
                    for (int j = 0; j < controls2Y.Length; j++)
                    {
                        controlsReferencesY.AddReplace(control, controls2Y[j]);
                        controlsReferencedY.AddReplace(controls2Y[j], control);
                    }
                }

                if (remarks.Contains("pinned-to-parent-y") && !pinnedToParentControlsY.Contains(control))
                    pinnedToParentControlsY.Add(control);


                if (remarks.Contains("non-defaultable") && !nonDefaultableButtons.Contains(control))
                    nonDefaultableButtons.Add(control as Button);
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
            if (UseSkinColors)
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
                moveScaleControlDependentReferringControlsX(allControls[i]);

            for (int i = allControls.Count - 1; i >= 0; i--)
                moveScaleControlDependentReferringControlsY(allControls[i]);

            scaleMoveLeftRightAnchoredControls();
            scaleMoveTopBottomAnchoredControls();
        }

        protected void setInitialFormMaximumMinimumSize(Size initialMinimumSize, Size initialMaximumSize, bool sameMinMaxWidth, bool sameMinMaxHeight)
        {
            if (sameMinMaxWidth && sameMinMaxHeight)
                fixedSize = true;


            hDpiFormScaling = (float)MinimumSize.Width / initialMinimumSize.Width;
            vDpiFormScaling = (float)MinimumSize.Height / initialMinimumSize.Height;

            if (!fixedSize)
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


            if (modal)
                MinimizeBox = false;

            if (fixedSize)
            {
                MaximizeBox = false;
                FormBorderStyle = FormBorderStyle.FixedDialog;
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
            if (font1 == null || font2 == null)
                return FontEquality.DifferentStylesSizes | FontEquality.DifferentNames;


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

            for (int i = 0; i < allControls.Count; i++) //Required for correct DPI scaling
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
                    scsa.panel1MinSize = sc.Panel1MinSize;
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
            Font initialFormFont = resource == null ? Font : resource as Font;


            MaximumSize = new Size(0, 0); //Let's temporary remove max. size restrictions


            FontEquality mbThisFormFontInitialEquality = CompareFonts(Font, initialFormFont);

            if (UseMusicBeeFont || UseCustomFont || mbThisFormFontInitialEquality != FontEquality.Equal)
            {
                Font workingFont;

                if (UseMusicBeeFont)
                {
                    workingFont = MbApiInterface.Setting_GetDefaultFont();
                }
                else if (UseCustomFont)
                {
                    workingFont = new System.Drawing.Font(
                        SavedSettings.pluginFontFamilyName,
                        SavedSettings.pluginFontSize,
                        SavedSettings.pluginFontStyle
                        );
                }
                else
                {
                    workingFont = Font;
                }

                FontEquality workingFontThisFormFontEquality = CompareFonts(workingFont, initialFormFont);


                for (int i = allControls.Count - 1; i >= 0; i--) //Required for correct DPI scaling
                    allControls[i].SuspendLayout();

                SuspendLayout();


                if (workingFontThisFormFontEquality == FontEquality.DifferentFontUnits)
                {
                    MessageBox.Show(MbForm, "Unsupported MusicBee font type!\n" +
                        "Either choose different font in MusicBee preferences or disable using skin colors in plugin settings.",
                        string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //Won't change form's font, but let's rescale in case of DPI change...
                }
                else if (workingFontThisFormFontEquality == FontEquality.Equal)
                {
                    //Won't change form's font, but let's rescale in case of DPI change...
                }
                else
                {
                    bool formFontEqualNamesStyles;

                    if ((workingFontThisFormFontEquality & FontEquality.EqualNames & FontEquality.EqualStyles) != 0) //Let's change font sizes only, they can't be the same (see above)
                    {
                        formFontEqualNamesStyles = true;
                        Font = new Font(Font.Name, workingFont.Size, Font.Style);
                    }
                    else
                    {
                        formFontEqualNamesStyles = false;
                        Font = new Font(workingFont.Name, workingFont.Size, workingFont.Style | Font.Style);
                    }


                    foreach (var control in allControls)
                    {
                        if (ownFontControls.Contains(control))
                        {
                            var controlFormFontEquality = CompareFonts(control.Font, initialFormFont);

                            if (formFontEqualNamesStyles)
                                control.Font = new Font(control.Font.Name, workingFont.Size * control.Font.Size / initialFormFont.Size, control.Font.Style);
                            else if ((controlFormFontEquality & FontEquality.EqualNames) != 0 || (controlFormFontEquality & FontEquality.PartiallyEqualNames) != 0)
                                control.Font = new Font(workingFont.Name, workingFont.Size * control.Font.Size / initialFormFont.Size, workingFont.Style | control.Font.Style);
                            else if ((controlFormFontEquality & FontEquality.SymbolStyles) == 0)
                                control.Font = new Font(control.Font.Name, workingFont.Size * control.Font.Size / initialFormFont.Size, workingFont.Style | control.Font.Style);
                            else
                                control.Font = new Font(control.Font.Name, workingFont.Size * control.Font.Size / initialFormFont.Size, control.Font.Style);
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


                initialFormFont.Dispose();
                if (!UseMusicBeeFont)
                    workingFont.Dispose();
            }

            MinimumSize = Size;

            hDpiFontScaling = (float)ClientSize.Width / initialClientSize.Width;
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

                MaximizedBounds = new Rectangle(Screen.FromControl(this).WorkingArea.Left, 0,
                    Screen.FromControl(this).WorkingArea.Width, maximizedHeight);
            }
        }

        protected void initAndShow()
        {
            if (!dontShowForm) //If dontShowForm, then form is created to get DPI/font scaling only, won't show it, form will be disposed soon.
            {
                //Common initialization
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
            if (DeviceDpi != 96)
                dpiScaling = DeviceDpi / 96f;

            UseSkinColors = !SavedSettings.dontUseSkinColors;
            UseMusicBeeFont = SavedSettings.useMusicBeeFont;
            UseCustomFont = SavedSettings.useCustomFont;

            addAllChildrenControls(this);


            ignoreSizePositionChanges = true;

            scaleForm(); //DPI/font scaling

            if (dontShowForm) //Form is created to get DPI/font scaling only, won't show it, form will be disposed soon.
                return;


            loadWindowSizesPositions();

            if (width != 0 && height != 0 && !fixedSize) //Saved state, not fixed size
            {
                width = (int)Math.Round(width * hDpiFormScaling);
                height = (int)Math.Round(height * vDpiFormScaling);


                if (MinimumSize.Width != 0 && width < MinimumSize.Width)
                    width = MinimumSize.Width;

                if (MaximumSize.Width != 0 && width > MaximumSize.Width)
                    width = MaximumSize.Width;


                if (MinimumSize.Height != 0 && height < MinimumSize.Height)
                    height = MinimumSize.Height;

                if (MaximumSize.Height != 0 && height > MaximumSize.Height)
                    height = MaximumSize.Height;


                Width = width;
                Height = height;
                WindowState = windowState;
            }
            else if (fixedSize) //Fixed size
            {
                Width = (int)Math.Round(Width * hDpiFormScaling);
                WindowState = FormWindowState.Normal;

                width = Width;
                height = Height;
            }
            else //No saved state, not fixed size
            {
                WindowState = FormWindowState.Normal;

                width = Width;
                height = Height;

                left = Left;
                top = Top;
            }

            if (left != 0 || top != 0) //Saved position
            {
                left = (int)Math.Round(left * hDpiFormScaling);
                top = (int)Math.Round(top * vDpiFormScaling);
            }
            else
            {
                int screenWidth = Screen.FromControl(this).WorkingArea.Width;
                int screenHeight = Screen.FromControl(this).WorkingArea.Height;

                left = (screenWidth - width) / 2;
                top = (screenHeight - height) / 2;
            }


            setFormMaximizedBounds();
            skinMoveScaleAllControls();


            SetBounds(-20 - Width, -20 - Height, Width, Height);
            showFormInternal();
        }

        protected void showFormInternal()
        {
            if (MbForm.IsDisposed)
                MbForm = (Form)FromHandle(MbApiInterface.MB_GetWindowHandle());

            if (modal)
                base.ShowDialog(MbForm);
            else
                base.Show(MbForm);
        }

        public new void Show()
        {
            modal = false;
            initAndShow();
        }

        public new void ShowDialog()
        {
            modal = true;
            initAndShow();
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

                        if (form.Visible && form.WindowState != FormWindowState.Minimized) //Restored or maximized window
                        {
                            form.Activate();
                        }
                        else //Hidden or minimized window
                        {
                            form.Left = form.left;
                            form.Top = form.top;
                            form.Width = form.width;
                            form.Height = form.height;
                            form.WindowState = form.windowState;
                            form.Visible = true;
                            form.Activate();
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

        private void PluginWindowTemplate_Load(object sender, EventArgs e)
        {
            initializeForm();
        }

        private void PluginWindowTemplate_Shown(object sender, EventArgs e)
        {
            for (int i = allControls.Count - 1; i >= 0; i--)
            {
                if (allControls[i].Focused && allControls[i].Controls.Count == 0)
                {
                    lastSelectedControl = allControls[i];
                    break;
                }
            }

            Application.DoEvents();

            Activate();
            SetBounds(left, top, width, height);
            ignoreSizePositionChanges = false;
        }

        private void PluginWindowTemplate_Move(object sender, EventArgs e)
        {
            if (ignoreSizePositionChanges)
                return;

            if (Left + Width >= 0 && Top + Height >= 0)
            {
                left = Left;
                top = Top;
            }

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
                else if (Left + Width >= 0 && Top + Height >= 0)
                {
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

            if (windowSettings != null 
                && windowSettings.x + windowSettings.w >= 0 
                && windowSettings.y + windowSettings.h >= 0
            )
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
            if (Left + Width >= 0 && Top + Height >= 0)
            {
                var currentWindowSettings = findCreateSavedWindowSettings(true);


                if (WindowState == FormWindowState.Normal)
                {
                    width = Width;
                    height = Height;
                    left = Left;
                    top = Top;
                    windowState = FormWindowState.Normal;
                }


                int width2 = (int)Math.Round(width / hDpiFormScaling);
                int height2 = (int)Math.Round(height / vDpiFormScaling);

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


                int left2 = (int)Math.Round(left / hDpiFormScaling);
                if (Math.Abs(left2 - currentWindowSettings.x) <= 1) // 1px
                {
                    left2 = currentWindowSettings.x;
                }

                int top2 = (int)Math.Round(top / vDpiFormScaling);
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
        }

        protected void saveWindowLayout(int column1Width = 0, int column2Width = 0, int column3Width = 0, int splitterDistance = 0, int table2column1Width = 0, int table2column2Width = 0, int table2column3Width = 0)
        {
            var currentWindowSettings = findCreateSavedWindowSettings(true);

            if (column1Width != 0)
            {
                int column1Width2 = (int)Math.Round(column1Width / hDpiFontScaling);
                if (100f * Math.Abs(column1Width2 - currentWindowSettings.column1Width) / column1Width2 < 0.5f)
                    column1Width2 = currentWindowSettings.column1Width;

                currentWindowSettings.column1Width = column1Width2;


                int column2Width2 = (int)Math.Round(column2Width / hDpiFontScaling);
                if (100f * Math.Abs(column2Width2 - currentWindowSettings.column1Width) / column2Width2 < 0.5f)
                    column2Width2 = currentWindowSettings.column2Width;

                currentWindowSettings.column2Width = column2Width2;


                int column3Width2 = (int)Math.Round(column3Width / hDpiFontScaling);
                if (100f * Math.Abs(column3Width2 - currentWindowSettings.column3Width) / column3Width2 < 0.5f)
                    column3Width2 = currentWindowSettings.column3Width;

                currentWindowSettings.column3Width = column3Width2;
            }

            if (table2column1Width != 0)
            {
                int table2column1Width2 = (int)Math.Round(table2column1Width / hDpiFontScaling);
                if (100f * Math.Abs(table2column1Width2 - currentWindowSettings.table2column1Width) / table2column1Width2 < 0.5f)
                    table2column1Width2 = currentWindowSettings.table2column1Width;

                currentWindowSettings.table2column1Width = table2column1Width2;


                int table2column2Width2 = (int)Math.Round(table2column2Width / hDpiFontScaling);
                if (100f * Math.Abs(table2column2Width2 - currentWindowSettings.table2column1Width) / table2column2Width2 < 0.5f)
                    table2column2Width2 = currentWindowSettings.table2column2Width;

                currentWindowSettings.table2column2Width = table2column2Width2;


                int table2column3Width2 = (int)Math.Round(table2column3Width / hDpiFontScaling);
                if (100f * Math.Abs(table2column3Width2 - currentWindowSettings.table2column3Width) / table2column3Width2 < 0.5f)
                    table2column3Width2 = currentWindowSettings.table2column3Width;

                currentWindowSettings.table2column3Width = table2column3Width2;
            }

            if (splitterDistance != 0)
            {
                int splitterDistance2 = (int)Math.Round(splitterDistance / vDpiFontScaling);
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

