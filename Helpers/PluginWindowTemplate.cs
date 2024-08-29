using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

using ExtensionMethods;

using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class PluginWindowTemplate : Form
    {
        protected bool modal;
        protected bool fixedSize;
        protected FormWindowState windowState;
        protected int left;
        protected int top;
        protected int height;
        protected int width;

        internal bool dontShowForm = false;

        //SKIN COLORING/CUSTOM FORM'S FONT/DPI SCALING
        internal float dpiScaling = 1;
        internal int scaledPx = 1;

        internal float hDpiFormScaling = 1;
        internal float vDpiFormScaling = 1;

        internal float hDpiFontScaling = 1;
        internal float vDpiFontScaling = 1;


        //ALL controls of the form and their references (if any)
        internal List<Control> allControls = new List<Control>();
        internal SortedDictionary<string, CustomComboBox> namesComboBoxes = new SortedDictionary<string, CustomComboBox>();

        internal Dictionary<Control, Control> controlsReferencesX = new Dictionary<Control, Control>();
        internal Dictionary<Control, Control> controlsReferencedX = new Dictionary<Control, Control>();

        internal Dictionary<Control, Control> controlsReferencesY = new Dictionary<Control, Control>();
        internal Dictionary<Control, Control> controlsReferencedY = new Dictionary<Control, Control>();

        internal List<Button> nonDefaultingButtons = new List<Button>();
        protected bool artificiallyFocusedAcceptButton;

        protected List<Control> pinnedToParentControlsX = new List<Control>();
        protected List<Control> leftRightAnchoredControls = new List<Control>();

        protected List<Control> pinnedToParentControlsY = new List<Control>();
        protected List<Control> topBottomAnchoredControls = new List<Control>();


        protected struct SplitContainerScalingAttributes
        {
            internal SplitContainer splitContainer;
            internal int panel1MinSize;
            internal int panel2MinSize;
            internal int splitterDistance;
        }

        protected List<SplitContainerScalingAttributes> splitContainersScalingAttributes = new List<SplitContainerScalingAttributes>();


        //Font & colors
        internal readonly bool useSkinColors;
        protected bool useMusicBeeFont;
        protected bool useCustomFont;

        private Font workingFont;


        //Opened form cached colors (for button repainting)
        protected Color controlHighlightForeColor;
        protected Color controlHighlightBackColor;
        protected Color buttonForeColor;
        protected Color buttonBackColor;
        protected Color buttonBorderColor;
        internal Color narrowScrollBarBackColor; //It's for combo box buttons
        protected Color buttonDisabledForeColor;
        protected Color buttonDisabledBackColor;


        //Skin button labels for rendering disabled buttons, combo boxes & contol borders
        protected Dictionary<Button, string> buttonLabels = new Dictionary<Button, string>();
        protected Dictionary<Control, string> controlCueBanners = new Dictionary<Control, string>();
        internal List<ControlBorder> controlBorders = new List<ControlBorder>();

        protected Control lastSelectedControl;


        //Let's ignore size/position change events during form DPI/font scaling
        protected bool ignoreSizePositionChanges = true;


        //BACKGROUND TASKS PROCESSING
        internal delegate void StopButtonClicked(PrepareOperation prepareOperation);
        internal delegate void TaskStarted();
        public delegate bool PrepareOperation();

        internal delegate void DataGridViewFormatChangedTags(DataGridView dataGridView, int rowIndex);


        private volatile bool isStopButtonAlreadyClicked = false;
        private TaskStarted taskStarted;

        protected static Plugin TagToolsPlugin;

        private Thread backgroundThread;
        protected ThreadStart job;

        internal volatile bool backgroundTaskIsScheduled;
        internal volatile bool backgroundTaskIsUpdatingTags;
        internal volatile bool backgroundTaskIsStopping;

        internal volatile bool backgroundTaskIsStoppedOrCancelled;
        internal volatile bool previewIsGenerated;

        protected volatile bool closeFormOnStopping = false;
        protected volatile bool ignoreClosingForm = false;

        protected Button clickedButton;
        protected Button previewButton;
        protected Button closeButton;

        protected string buttonOKName;
        protected string buttonPreviewName;
        protected string buttonCloseName;

        internal PluginWindowTemplate()
        {
            //Some operations won't create visual forms of commands. Only they use this constructor. Let's skip component initialization in this case.
        }

        internal PluginWindowTemplate(Plugin plugin)
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            TagToolsPlugin = plugin;
            useSkinColors = !SavedSettings.dontUseSkinColors;
        }

        internal bool backgroundTaskIsWorking()
        {
            if (backgroundTaskIsScheduled && !backgroundTaskIsStoppedOrCancelled)
                return true;

            return false;
        }

        internal void SetComboBoxCue(CustomComboBox comboBox, string cue)
        {
            if (comboBox.comboBox != null)
                controlCueBanners.AddReplace(comboBox.comboBox, cue);

            comboBox.SetCue(cue);
        }

        internal void SetComboBoxCue(ComboBox comboBox, string cue)
        {
            controlCueBanners.AddReplace(comboBox, cue);
            comboBox.SetCue(cue);
        }

        internal void ClearComboBoxCue(ComboBox comboBox)
        {
            controlCueBanners.RemoveExisting(comboBox);
            comboBox.ClearCue();
        }

        private void comboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            var comboBox = sender as ComboBox;

            var index = e.Index;

            Color foreColor;
            Color backColor;

            if (comboBox.IsEnabled() && comboBox.Focused && (index == -1 || (e.State & DrawItemState.Selected) != 0))
            {
                foreColor = SystemColors.HighlightText; //---
                backColor = SystemColors.Highlight;
            }
            //Disabled
            else if (e.State == (DrawItemState.Disabled | DrawItemState.NoAccelerator
                | DrawItemState.NoFocusRect | DrawItemState.ComboBoxEdit))
            {
                foreColor = DimmedAccentColor;
                backColor = comboBox.BackColor;
            }
            else //if (control.IsEnabled())
            {
                foreColor = comboBox.ForeColor;
                backColor = comboBox.BackColor;
            }

            string text;
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

        internal void button_EnabledChanged(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button.IsEnabled() && button == AcceptButton)
                button_GotFocus(sender, e);
            else if (button.Focused)
                button_GotFocus(sender, e);
            else if (useSkinColors)
                button_LostFocus(sender, e);
        }

        internal void button_GotFocus(object sender, EventArgs e)
        {
            var button = sender as Button;
            var acceptButton = AcceptButton as Button;

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
                if (!nonDefaultingButtons.Contains(button)) //It's defaultable button
                {
                    lastSelectedControl = button;
                    AcceptButton = button;
                    button_LostFocus(acceptButton, null);
                }
                else if (acceptButton != null && acceptButton.IsEnabled() && !artificiallyFocusedAcceptButton) //It's non-defaultable button & accept button enabled
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


            setSkinnedControlColors(button, null);
        }

        internal void button_LostFocus(object sender, EventArgs e)
        {
            if (sender != null)
                setSkinnedControlColors(sender as Button, null);
        }

        internal void button_Paint(object sender, PaintEventArgs e)
        {
            var button = sender as Button;

            var text = buttonLabels[button];

            //Set flags to center text on the button.
            var flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;   //center the text

            //Render the text onto the button.
            TextRenderer.DrawText(e.Graphics, text, button.Font, e.ClipRectangle, button.ForeColor, flags);
        }

        internal void button_TextChanged(object sender, EventArgs e)
        {
            var button = sender as Button;
            var text = button.Text;

            if (!string.IsNullOrEmpty(text))
            {
                buttonLabels.AddReplace(button, text);

                if (useSkinColors)
                {
                    button.Text = string.Empty;
                    button.Invalidate();
                }
            }
        }

        internal void control_Enter(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (!(control is Button) && !control.GetType().IsSubclassOf(typeof(Button)) && control.Controls.Count == 0)
                lastSelectedControl = control;
        }

        //Returns X referred controls array, int - array of scrolledControl levelX if control2X is ancestor, otherwise 0,
        //then the same for Y, then scaled/moved marks and remarks
        internal static (Control[], int[], Control[], int[], bool, bool, bool, bool, string[]) GetReferredControlsControlLevelMarksRemarks(Control control)
        {
            var (_, _, _, scaledMovedL, scaledMovedR, scaledMovedT, scaledMovedB, remarks) = GetControlTagReferredNamesMarksRemarks(control);
            var (controls2X, levelsX, controls2Y, levelsY) = GetReferredControls(control);

            return (controls2X, levelsX, controls2Y, levelsY, scaledMovedL, scaledMovedR, scaledMovedT, scaledMovedB, remarks);
        }


        protected void Enable(bool state, Label accentedLabel)
        {
            foreach (var control in allControls)
            {
                if (control == accentedLabel)
                    continue;


                if (control is ListBox)
                {
                    var customHScrollBar = ControlsTools.FindControlChild<CustomHScrollBar>(control.Parent);
                    EnableControl(customHScrollBar, state);

                    var customVScrollBar = ControlsTools.FindControlChild<CustomVScrollBar>(control.Parent);
                    EnableControl(customVScrollBar, state);

                    EnableControl(control, state);
                }
                else if (!control.HasChildren || control is CustomComboBox)
                {
                    EnableControl(control, state);
                }
            }
        }

        private static void EnableControl(Control control, bool state)
        {
            if (control == null)
                return;


            if (state)
            {
                if (control.AccessibleDescription != DisabledState)
                    control.EnableInternal(true);

                control.AccessibleDescription = string.Empty;
            }
            else
            {
                if (control.IsEnabled())
                    control.AccessibleDescription = EnabledState;
                else
                    control.AccessibleDescription = DisabledState;

                control.EnableInternal(false);
            }
        }

        internal static void SetControlTagReferredNameMarksRemarks(Control control, string tagValue,
            bool scaledMovedL = false, bool scaledMovedR = false, bool scaledMovedT = false, bool scaledMovedB = false,
            string[] control2NamesX = null, string[] control2NamesY = null, params string[] remarks)
        {
            var (_, oldControl2NamesXArray, oldControl2NamesYArray, scaledMovedOldL, scaledMovedOldR, scaledMovedOldT, scaledMovedOldB, oldRemarks) =
                GetControlTagReferredNamesMarksRemarks(control);

            var oldControl2NamesX = oldControl2NamesXArray.ToList();
            var oldControl2NamesY = oldControl2NamesYArray.ToList();


            if (control2NamesX != null)
            {
                for (var i = 0; i < control2NamesX.Length; i++)
                    oldControl2NamesX.AddUnique(control2NamesX[i]);
            }

            var control2NameX = string.Empty;
            foreach (var controlName in oldControl2NamesX)
                control2NameX += "|" + controlName;

            control2NameX = control2NameX.TrimEnd('|');


            if (control2NamesY != null)
            {
                for (var i = 0; i < control2NamesY.Length; i++)
                    oldControl2NamesY.AddUnique(control2NamesY[i]);
            }

            var control2NameY = string.Empty;
            foreach (var controlName in oldControl2NamesY)
                control2NameY += "|" + controlName;

            control2NameY = control2NameY.TrimEnd('|');


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
        internal static (string, string[], string[], bool, bool, bool, bool, string[]) GetControlTagReferredNamesMarksRemarks(Control control)
        {
            if (control.Tag == null || !(control.Tag is string controlTag))
                return (string.Empty, Array.Empty<string>(), Array.Empty<string>(), false, false, false, false, Array.Empty<string>());

            var scaledMovedL = controlTag.Contains("#scaled-moved-left");
            var scaledMovedR = controlTag.Contains("#scaled-moved-right");
            var scaledMovedT = controlTag.Contains("#scaled-moved-top");
            var scaledMovedB = controlTag.Contains("#scaled-moved-bottom");
            controlTag = controlTag.Replace("#scaled-moved-left", string.Empty);
            controlTag = controlTag.Replace("#scaled-moved-right", string.Empty);
            controlTag = controlTag.Replace("#scaled-moved-top", string.Empty);
            controlTag = controlTag.Replace("#scaled-moved-bottom", string.Empty);

            var tagValue = controlTag;
            var remarks = string.Empty;
            var control2NameX = controlTag;
            var control2NameY = string.Empty;

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

            var control2NamesX = control2NameX.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            var control2NamesY = control2NameY.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            if (string.IsNullOrEmpty(remarks))
                return (tagValue, control2NamesX, control2NamesY, scaledMovedL, scaledMovedR, scaledMovedT, scaledMovedB, Array.Empty<string>());
            else
                return (tagValue, control2NamesX, control2NamesY, scaledMovedL, scaledMovedR, scaledMovedT, scaledMovedB, remarks.Split('@'));
        }

        internal static (Control, int) GetReferredControlByName(Control control, string control2Name) //int - scrolledControl levelX if control2X is ancestor, otherwise 0
        {
            if (string.IsNullOrEmpty(control2Name))
                return (null, 0);

            var levelX = 0;
            var control2X = control.Parent.Controls[control2Name];

            var parent = control.Parent;
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

        //Returns X referred controls array, int - array of scrolledControl levelX if control2X is ancestor, otherwise 0, then the same for Y
        internal static (Control[], int[], Control[], int[]) GetReferredControls(Control control)
        {
            var (_, control2NamesX, control2NamesY, _, _, _, _, _) =
                GetControlTagReferredNamesMarksRemarks(control);

            var controls2X = new Control[control2NamesX.Length];
            var levelsX = new int[control2NamesX.Length];
            for (var i = 0; i < control2NamesX.Length; i++)
            {
                var (control2X, levelX) = GetReferredControlByName(control, control2NamesX[i]);
                controls2X[i] = control2X;
                levelsX[i] = levelX;
            }

            var controls2Y = new Control[control2NamesY.Length];
            var levelsY = new int[control2NamesY.Length];
            for (var i = 0; i < control2NamesY.Length; i++)
            {
                var (control2Y, levelY) = GetReferredControlByName(control, control2NamesY[i]);
                controls2Y[i] = control2Y;
                levelsY[i] = levelY;
            }

            return (controls2X, levelsX, controls2Y, levelsY);
        }

        internal static Size GetControlSize(Control control)
        {
            if (control.GetType().IsSubclassOf(typeof(Form)) || control is Form)
                // ReSharper disable once PossibleNullReferenceException
                return (control as Form).ClientSize;
            else
                return control.Size;
        }

        internal static int GetControlLevel(Control control, Control control2)
        {
            if (control == null)
                return 0;


            var control2Name = control2.Name;

            var level = 0;

            Control foundParent = null;
            var parent = control.Parent;
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

        internal static (int, int, int, int) GetControl2LeftRightMarginsOrParent0WidthPaddings(Control control, Control control2X, bool pinnedToParentX = false) //Returns: Left, Right or 0 & scrolledControl Width, margins or scrolledControl padding
        {
            if (control2X == null && pinnedToParentX)
                return (0, GetControlSize(control.Parent).Width, control.Parent.Padding.Left, control.Parent.Padding.Right);


            var levelX = GetControlLevel(control, control2X);

            if (levelX > 0)
                return (GetControlSize(control2X).Width, GetControlSize(control2X).Width, control2X.Padding.Right, control2X.Padding.Left); //-V3125
            else //if (levelX == 0)
                return (control2X.Left, control2X.Left + control2X.Width, control2X.Margin.Left, control2X.Margin.Right);
        }

        internal static (int, int, int, int) GetControl2TopBottomMarginsOrParent0HeightPaddings(Control control, Control control2Y, bool pinnedToParentY = false) //Returns: Top, Bottom or 0 & scrolledControl Height, margins or scrolledControl padding
        {
            if (control2Y == null && pinnedToParentY)
                return (0, GetControlSize(control.Parent).Height, control.Parent.Padding.Top, control.Parent.Padding.Bottom);


            var levelY = GetControlLevel(control, control2Y);

            if (levelY > 0)
                return (GetControlSize(control2Y).Height, GetControlSize(control2Y).Height, control2Y.Padding.Bottom, control2Y.Padding.Top); //-V3125
            else //if (levelY == 0)
                return (control2Y.Top, control2Y.Top + control2Y.Height, control2Y.Margin.Top, control2Y.Margin.Bottom);
        }

        internal int getRightAnchoredControlX(Control control, int controlNewWidth, Control control2X, bool pinnedToParentX, int levelX, float offset = 0)
        {
            if (pinnedToParentX)
            {
                return GetControlSize(control.Parent).Width - controlNewWidth - (control.Margin.Right + control.Parent.Padding.Right);
            }
            else if (levelX == 0)
            {
                if (offset == 0) //-V3024
                    return control2X.Left - controlNewWidth - (control.Margin.Right + control2X.Margin.Left);
                else
                    return (int)Math.Round(control2X.Left - controlNewWidth - offset * hDpiFontScaling);
            }
            else
            {
                return GetControlSize(control2X).Width - controlNewWidth - (control.Margin.Right + control2X.Padding.Right);
            }
        }

        internal int getBottomAnchoredControlY(Control control, int controlNewHeight, Control control2Y, bool pinnedToParentY, int levelY, float offset = 0)
        {
            if (pinnedToParentY)
            {
                return GetControlSize(control.Parent).Height - controlNewHeight - (control.Margin.Bottom + control.Parent.Padding.Bottom);
            }
            else if (levelY == 0)
            {
                if (offset == 0) //-V3024
                    return control2Y.Top - controlNewHeight - (control.Margin.Bottom + control2Y.Margin.Top);
                else
                    return (int)Math.Round(control2Y.Top - controlNewHeight - offset * vDpiFontScaling);
            }
            else
            {
                return GetControlSize(control2Y).Height - controlNewHeight - (control.Margin.Bottom + control2Y.Padding.Bottom);
            }
        }

        internal static float GetYCenteredToY2(float controlHeight, float refControlY, Control control2X)
        {
            var control2MiddleY = refControlY + control2X.Height / 2f;
            return control2MiddleY - controlHeight / 2f;
        }

        internal int getButtonY(Control control)
        {
            float controlNewY;

            if ((control.Anchor & AnchorStyles.Top) == 0 && (control.Anchor & AnchorStyles.Bottom) == 0) //Not vertically anchored
                controlNewY = control.Top;
            else
                controlNewY = control.Top - (control.Height - control.Height / vDpiFontScaling) / 4f; //---

            return (int)Math.Round(controlNewY);
        }

        internal int getLabelY(Control control)
        {
            var controlNewY = control.Top - (control.Height - control.Height / vDpiFontScaling) / 2;
            return (int)Math.Round(controlNewY);
        }

        internal int getPictureBoxY(Control control, float scale)
        {
            var controlNewY = control.Top - (control.Height - control.Height / scale) / 2;
            return (int)Math.Round(controlNewY);
        }

        internal int getPictureBoxY(float controlHeight, Control control2X)
        {
            var heightDifference = controlHeight - control2X.Height;
            var controlNewY = GetYCenteredToY2(controlHeight, control2X.Top, control2X);

            controlNewY -= heightDifference / 6 / vDpiFontScaling; //---
            controlNewY += controlHeight * 0.07f;
            return (int)Math.Round(controlNewY);
        }

        internal int getCheckBoxesRadioButtonsY(Control control, Control control2X)
        {
            if (control.Height == control2X.Height)
            {
                return control.Top;
            }
            else
            {
                float heightDifference = control.Height - control2X.Height;
                var controlNewY = GetYCenteredToY2(control.Height, control2X.Top, control2X);

                controlNewY -= heightDifference / 6 / vDpiFontScaling; //---
                controlNewY += control.Height * 0.07f;
                return (int)Math.Round(controlNewY);
            }
        }

        //Let's correct flaws of AUTO-scaling
        internal virtual void preMoveScaleControl(Control control)
        {
            if (control.GetType().IsSubclassOf(typeof(Form)) || control is Form)
                return;

            var stringTag = control.Tag as string;

            if (control.GetType().IsSubclassOf(typeof(Button)) || control is Button)
            {
                control.Top = getButtonY(control);
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
            if (stringTag?.Contains("@square-control") == true || stringTag?.Contains("@square-button") == true)
            {
                //Let's change auto-scaled button height to auto-scaled height of generic controls, not buttons
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (control is Button && !stringTag.Contains("@square-button") && (ButtonHeightDpiFontScaling != 1 || TextBoxHeightDpiFontScaling != 1)) //-V3024
                {
                    int initialHeight = control.Height;
                    control.Height = (int)Math.Round(control.Height * TextBoxHeightDpiFontScaling / ButtonHeightDpiFontScaling);
                    control.Top -= (int)Math.Round((control.Height - initialHeight + 1f) / 2f, MidpointRounding.ToEven);
                }

                if ((control.Anchor & AnchorStyles.Right) != 0)
                {
                    if (control.Width > control.Height)
                        control.Left -= control.Width - control.Height;
                    else
                        control.Left += control.Width - control.Height;
                }


                control.Width = control.Height;
            }
        }

        internal void moveScaleControlDependentReferringControlsX(Control control, int control2XIndex = -1)
        {
            if (control.GetType().IsSubclassOf(typeof(Form)) || control is Form)
                return;


            const float smallControlOffsetX = 0.1f;
            const float checkBoxRadioButtonOffsetCompensationX = -12f;
            const float checkBoxRadioButtonAndPictureBoxOffsetCompensationX = -7f;//---


            controlsReferencedX.TryGetValue(control, out var referringControlX);

            var (controls2X, levelsX, _, _, scaledMovedL, scaledMovedR, scaledMovedT, _, remarks) = GetReferredControlsControlLevelMarksRemarks(control);
            var pinnedToParentX = remarks.Contains("pinned-to-parent-x");


            //control is left & right anchored. Let's move/scale it at the end (in another function).
            if ((control.Anchor & AnchorStyles.Left) != 0 && (control.Anchor & AnchorStyles.Right) != 0)
            {
                leftRightAnchoredControls.AddUnique(control);
                return;
            }


            //control is left anchored
            if (referringControlX != null && (control.Anchor & AnchorStyles.Left) != 0 && (control.Anchor & AnchorStyles.Right) == 0)
                moveScaleControlDependentReferringControlsX(referringControlX);
            //referringControl is left anchored, control must be left & right anchored (left anchored only is considered above) 
            else if (referringControlX != null && (referringControlX.Anchor & AnchorStyles.Left) != 0 && (referringControlX.Anchor & AnchorStyles.Right) != 0)
                moveScaleControlDependentReferringControlsX(referringControlX);


            //Special case. Will move/scale it to scrolledControl. Let's proceed further...
            if (pinnedToParentX && control2XIndex != -1)
                ; //Nothing...
            //No controls2X and control is Not left & right anchored (see above)
            else if (controls2X == null)
                return;
            //No controls2X and control is Not left & right anchored (see above)
            else if (controls2X.Length == 0)
                return;
            else if (control2XIndex == -1)
            {
                for (var i = 0; i < controls2X.Length; i++)
                    moveScaleControlDependentReferringControlsX(control, i);

                return;
            }

            //controls2X[control2XIndex] is right anchored and control is Not left & right anchored (see earlier)
            if ((controls2X[control2XIndex].Anchor & AnchorStyles.Left) == 0 && (controls2X[control2XIndex].Anchor & AnchorStyles.Right) != 0) //-V3125
                moveScaleControlDependentReferringControlsX(controls2X[control2XIndex]);



            if (scaledMovedL && scaledMovedR && scaledMovedT)
                return;


            var scaledMovedL2 = true;
            var scaledMovedR2 = true;

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

                        var distance = controlXPlaceholder - controls2X[control2XIndex].Left;
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

        internal void moveScaleControlDependentReferringControlsY(Control control, int control2YIndex = -1)
        {
            if (control.GetType().IsSubclassOf(typeof(Form)) || control is Form)
                return;


            controlsReferencedY.TryGetValue(control, out var referringControlY);

            var (_, _, controls2Y, levelsY, _, _, scaledMovedT, scaledMovedB, remarks) = GetReferredControlsControlLevelMarksRemarks(control);
            var pinnedToParentY = remarks.Contains("pinned-to-parent-y");

            //control is top & bottom anchored. Let's move/scale it at the end (in another function).
            if ((control.Anchor & AnchorStyles.Top) != 0 && (control.Anchor & AnchorStyles.Bottom) != 0)
            {
                topBottomAnchoredControls.AddUnique(control);
                return;
            }


            //control is top anchored
            if (referringControlY != null && (control.Anchor & AnchorStyles.Top) != 0 && (control.Anchor & AnchorStyles.Bottom) == 0)
                moveScaleControlDependentReferringControlsY(referringControlY);
            //referringControl is top anchored, control must be top & bottom anchored (top anchored only is considered above) 
            else if (referringControlY != null && (referringControlY.Anchor & AnchorStyles.Top) != 0 && (referringControlY.Anchor & AnchorStyles.Bottom) != 0)
                moveScaleControlDependentReferringControlsY(referringControlY);


            if (pinnedToParentY && control2YIndex != -1)
                ; //Nothing...
            //No controls2Y and control is Not top & bottom anchored (see above)
            else if (controls2Y == null)
                return;
            //No controls2Y and control is Not top & bottom anchored (see above)
            else if (controls2Y.Length == 0)
                return;
            else if (control2YIndex == -1)
            {
                for (var i = 0; i < controls2Y.Length; i++)
                    moveScaleControlDependentReferringControlsY(control, i);

                return;
            }

            //controls2Y[control2YIndex] is bottom anchored and control is Not top & bottom anchored (see earlier)
            if ((controls2Y[control2YIndex].Anchor & AnchorStyles.Top) == 0 && (controls2Y[control2YIndex].Anchor & AnchorStyles.Bottom) != 0) //-V3125
                moveScaleControlDependentReferringControlsX(controls2Y[control2YIndex]);



            if (scaledMovedT && scaledMovedB)
                return;


            var scaledMovedT2 = true;
            var scaledMovedB2 = true;

            if (controls2Y[control2YIndex] != null)
                (_, _, _, _, _, scaledMovedT2, scaledMovedB2, _) = GetControlTagReferredNamesMarksRemarks(controls2Y[control2YIndex]);


            //control top anchored
            if ((control.Anchor & AnchorStyles.Top) != 0 && (control.Anchor & AnchorStyles.Bottom) == 0)
            {
                if (pinnedToParentY || levelsY[control2YIndex] > 0)
                {
                    if (!scaledMovedT)
                    {
                        var parent = controls2Y[control2YIndex] ?? control.Parent;

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
                            var middleY = control.Top + control.Height / 2f;
                            var newControlT2 = middleY - controls2Y[control2YIndex].Height / 2f + vDpiFontScaling * 2f;

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
                        var parent = controls2Y[control2YIndex] ?? control.Parent;

                        control.Top = GetControlSize(parent).Height - control.Height - (parent.Padding.Bottom + control.Margin.Bottom);
                        scaledMovedT = true;
                    }
                }
                //controls2Y[control2YIndex] bottom anchored
                else if ((controls2Y[control2YIndex].Anchor & AnchorStyles.Top) == 0 && (controls2Y[control2YIndex].Anchor & AnchorStyles.Bottom) != 0) //-V3125
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

        internal void scaleMoveLeftRightAnchoredControls()
        {
            foreach (var control in leftRightAnchoredControls)
            {
                controlsReferencedX.TryGetValue(control, out var controlReferencing);
                controlsReferencesX.TryGetValue(control, out var controlReference);

                var (_, _, _, _, _, _, _, remarks) = GetControlTagReferredNamesMarksRemarks(control);
                var pinnedToParentX = remarks.Contains("pinned-to-parent-x");
                var controlReferencingLevel = GetControlLevel(controlReferencing, control);

                int controlRNew;

                if (controlReferencing != null && controlReferencingLevel == 0 && controlReference != null)
                {
                    var (_, controlReferencingRight, _, controlReferencingMarginRight) = GetControl2LeftRightMarginsOrParent0WidthPaddings(control, controlReferencing, pinnedToParentX);
                    var (controlReferenceLeft, _, controlReferenceMarginLeft, _) = GetControl2LeftRightMarginsOrParent0WidthPaddings(control, controlReference, pinnedToParentX);

                    controlRNew = controlReferenceLeft - (controlReferenceMarginLeft + control.Margin.Right);
                    control.Left = controlReferencingRight + (controlReferencingMarginRight + control.Margin.Left);
                    control.Width = controlRNew - control.Left;

                }
                else if (controlReferencing != null && controlReferencingLevel == 0 && controlReference == null)
                {
                    var (controlReferencingLeft, _, _, controlReferencingMarginRight) = GetControl2LeftRightMarginsOrParent0WidthPaddings(control, controlReferencing, pinnedToParentX);

                    controlRNew = control.Left + control.Width;
                    control.Left = controlReferencingLeft + (controlReferencingMarginRight + control.Margin.Left);
                    control.Width = controlRNew - control.Left;
                }
                else if ((controlReferencing == null || controlReferencingLevel > 0) && controlReference != null)
                {
                    var (controlReferenceLeft, controlReferenceRight, controlReferenceMarginLeft, controlReferenceMarginRight) = GetControl2LeftRightMarginsOrParent0WidthPaddings(control, controlReference, pinnedToParentX);

                    if (pinnedToParentX && GetControlLevel(control, controlReference) > 0) //Pinned to controlReference (i.e. controlReference is a scrolledControl)
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
                    //Nothing to do. Let's keep auto-scaled scaling/position... 
                }
            }
        }

        internal void scaleMoveTopBottomAnchoredControls()
        {
            foreach (var control in topBottomAnchoredControls)
            {
                controlsReferencedY.TryGetValue(control, out var controlReferencing);
                controlsReferencesY.TryGetValue(control, out var controlReference);

                var (_, _, _, _, _, _, _, remarks) = GetControlTagReferredNamesMarksRemarks(control);
                var pinnedToParentY = remarks.Contains("pinned-to-parent-y");
                var controlReferencingLevel = GetControlLevel(controlReferencing, control);

                int controlRNew;

                if (controlReferencing != null && controlReferencingLevel == 0 && controlReference != null)
                {
                    var (_, controlReferencingBottom, _, controlReferencingMarginBottom) = GetControl2TopBottomMarginsOrParent0HeightPaddings(control, controlReferencing, pinnedToParentY);
                    var (controlReferenceTop, _, controlReferenceMarginTop, _) = GetControl2TopBottomMarginsOrParent0HeightPaddings(control, controlReference, pinnedToParentY);

                    controlRNew = controlReferenceTop - (controlReferenceMarginTop + control.Margin.Bottom);
                    control.Top = controlReferencingBottom + (controlReferencingMarginBottom + control.Margin.Top);
                    control.Height = controlRNew - control.Top;

                }
                else if (controlReferencing != null && controlReferencingLevel == 0 && controlReference == null)
                {
                    var (controlReferencingTop, _, _, controlReferencingMarginBottom) = GetControl2TopBottomMarginsOrParent0HeightPaddings(control, controlReferencing, pinnedToParentY);

                    controlRNew = control.Top + control.Height;
                    control.Top = controlReferencingTop + (controlReferencingMarginBottom + control.Margin.Top);
                    control.Height = controlRNew - control.Top;
                }
                else if ((controlReferencing == null || controlReferencingLevel > 0) && controlReference != null)
                {
                    var (controlReferenceTop, controlReferenceBottom, controlReferenceMarginTop, controlReferenceMarginBottom) = GetControl2TopBottomMarginsOrParent0HeightPaddings(control, controlReference, pinnedToParentY);

                    if (pinnedToParentY && GetControlLevel(control, controlReference) > 0) //Pinned to controlReference (i.e. controlReference is a scrolledControl)
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
                    //Nothing to do. Let's keep auto-scaled scaling/position... 
                }
            }
        }

        protected void dgvCustomHScrollBar_Scroll(object sender, EventArgs e)
        {
            dgvCustomHScrollBar_ValueChanged(sender, e);
        }

        protected void dgvCustomHScrollBar_ValueChanged(object sender, EventArgs e)
        {
            var customHScrollBar = sender as CustomHScrollBar;
            var dataGridView = customHScrollBar.Parent as DataGridView;
            var value = customHScrollBar.Value;
            var largeChange = customHScrollBar.LargeChange;
            var maximum = customHScrollBar.Maximum;
            var form = customHScrollBar.FindForm();

            if (dataGridView.ColumnCount > 0 && form != null && form.WindowState != FormWindowState.Minimized)
            {
                customHScrollBar.SettingParentScroll = true;

                if (maximum == 0)
                    dataGridView.HorizontalScrollingOffset = 0;
                else
                    dataGridView.HorizontalScrollingOffset = (int)(((float)maximum - largeChange) * value / maximum);

                customHScrollBar.SettingParentScroll = false; //-V3008

                customHScrollBar.Invalidate();
                dataGridView.Invalidate();
            }
        }

        protected void dgvCustomVScrollBar_Scroll(object sender, EventArgs e)
        {
            dgvCustomVScrollBar_ValueChanged(sender, e);
        }

        protected void setDgvVerticalOffset(DataGridView dataGridView, int offset)
        {
            try
            {
                PropertyInfo verticalOffset = dataGridView.GetType().GetProperty("VerticalOffset", BindingFlags.NonPublic | BindingFlags.Instance);
                verticalOffset.SetValue(dataGridView, offset, null);
            }
            catch //-V3163 //-V5606
            {
                //Let's ignore...
            }
        }

        protected void dgvCustomVScrollBar_ValueChanged(object sender, EventArgs e)
        {
            var customVScrollBar = sender as CustomVScrollBar;
            var dataGridView = customVScrollBar.Parent as DataGridView;
            var value = customVScrollBar.Value;
            var largeChange = customVScrollBar.LargeChange;
            var maximum = customVScrollBar.Maximum;
            var form = customVScrollBar.FindForm();
            var vScrollBar = ControlsTools.FindControlChild<VScrollBar>(dataGridView); //-V3149

            if (dataGridView.RowCount > 0 && form != null && form.WindowState != FormWindowState.Minimized)
            {
                customVScrollBar.SettingParentScroll = true;

                if (maximum > 0)
                    setDgvVerticalOffset(dataGridView, (int)(((float)maximum - largeChange) * value / maximum) + 1);

                customVScrollBar.SettingParentScroll = false; //-V3008

                vScrollBar.Visible = false; //-V3080
                customVScrollBar.Invalidate();
                dataGridView.Invalidate();
            }
        }

        private void dataGridView_MouseWheel(object sender, MouseEventArgs e)
        {
            var dataGridView = sender as DataGridView;
            var customVScrollBar = ControlsTools.FindControlChild<CustomVScrollBar>(dataGridView);

            int delta;
            if ((ModifierKeys & Keys.Shift) == 0 && customVScrollBar.Visible) //-V3080
            {
                if ((ModifierKeys & Keys.Alt) == 0)
                    delta = -e.Delta / 4; //---
                else
                    delta = -e.Delta * 4;

                customVScrollBar.Value += delta;
            }
            else if ((ModifierKeys & Keys.Shift) != 0 || !customVScrollBar.Visible)
            {
                var customHScrollBar = ControlsTools.FindControlChild<CustomHScrollBar>(dataGridView);
                if (customHScrollBar.Visible) //-V3080
                {
                    if ((ModifierKeys & Keys.Alt) == 0)
                        delta = -e.Delta * 2; //---
                    else
                        delta = -e.Delta * 32;

                    customHScrollBar.Value += delta;
                }
            }

            dataGridView.Invalidate();
        }

        private void dataGridView_Scroll(object sender, ScrollEventArgs e)
        {
            var dataGridView = sender as DataGridView;

            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                var customHScrollBar = ControlsTools.FindControlChild<CustomHScrollBar>(dataGridView);

                if (customHScrollBar.SettingParentScroll) //-V3080
                    return;

                customHScrollBar.SetThumbLeft(dataGridView.HorizontalScrollingOffset);

            }
            else //if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                var customVScrollBar = ControlsTools.FindControlChild<CustomVScrollBar>(dataGridView);

                if (customVScrollBar.SettingParentScroll) //-V3080
                    return;

                customVScrollBar.SetThumbTop(dataGridView.VerticalScrollingOffset);
            }
        }

        protected void listBoxCustomHScrollBar_Scroll(object sender, EventArgs e)
        {
            listBoxCustomHScrollBar_ValueChanged(sender, e);
        }

        protected void listBoxCustomHScrollBar_ValueChanged(object sender, EventArgs e)
        {
            var customHScrollBar = sender as CustomHScrollBar;
            var listBox = customHScrollBar.ScrolledControl as ListBox;

            if (listBox.Items.Count > 0)
            {
                var (totalColumns, visibleColumns, visible1stColumnItems) = GetListBoxMetrics(listBox);

                var nColumnRange = totalColumns - visibleColumns;
                var (_, nPixelRange, _, _, _) = customHScrollBar.GetMetrics();

                var firstVisibleColumnIndex = listBox.TopIndex / visible1stColumnItems;
                var newFirstVisibleColumnIndex = firstVisibleColumnIndex;
                if (nPixelRange > 0)
                    newFirstVisibleColumnIndex = (int)Math.Round((float)nColumnRange * customHScrollBar.GetThumbLeft()
                        / nPixelRange);

                customHScrollBar.SettingParentScroll = true;
                listBox.TopIndex = newFirstVisibleColumnIndex * visible1stColumnItems;
                customHScrollBar.SettingParentScroll = false; //-V3008

                customHScrollBar.Invalidate();
                listBox.Invalidate();
            }
        }

        protected void listBoxCustomVScrollBar_Scroll(object sender, EventArgs e)
        {
            listBoxCustomVScrollBar_ValueChanged(sender, e);
        }

        protected void listBoxCustomVScrollBar_ValueChanged(object sender, EventArgs e)
        {
            var customVScrollBar = sender as CustomVScrollBar;
            var listBox = customVScrollBar.ScrolledControl as ListBox;

            if (listBox.Items.Count > 0)
            {
                var (_, _, visible1stColumnItems) = GetListBoxMetrics(listBox);

                var nRowRange = listBox.Items.Count - visible1stColumnItems;
                var (_, nPixelRange, _, _, _) = customVScrollBar.GetMetrics();

                var newFirstDisplayedScrollingItemIndex = listBox.TopIndex;
                if (nPixelRange > 0)
                    newFirstDisplayedScrollingItemIndex = (int)Math.Round((float)nRowRange * customVScrollBar.GetThumbTop() / nPixelRange);

                customVScrollBar.SettingParentScroll = true;
                listBox.TopIndex = newFirstDisplayedScrollingItemIndex;
                customVScrollBar.SettingParentScroll = false; //-V3008

                customVScrollBar.Invalidate();
                listBox.Invalidate();
            }
        }

        private void listBox_MouseWheel(object sender, MouseEventArgs e)
        {
            var listBox = sender as ListBox;

            var (_, _, visible1stColumnItems) = GetListBoxMetrics(listBox);

            int delta;
            if (listBox.MultiColumn)
            {
                if ((ModifierKeys & Keys.Alt) == 0)
                    delta = -e.Delta / 64; //---
                else
                    delta = -e.Delta / 16;

                delta *= visible1stColumnItems;

                if (listBox.TopIndex + delta < 0)
                    listBox.TopIndex = 0;
                else if (listBox.TopIndex + delta > listBox.Items.Count - visible1stColumnItems)
                    listBox.TopIndex = listBox.Items.Count - visible1stColumnItems;
                else
                    listBox.TopIndex += delta;
            }
            else
            {
                if ((ModifierKeys & Keys.Alt) == 0)
                    delta = -e.Delta / 16; //---
                else
                    delta = -e.Delta / 4;

                if (listBox.TopIndex + delta < 0)
                    listBox.TopIndex = 0;
                else if (listBox.TopIndex + delta > listBox.Items.Count - visible1stColumnItems)
                    listBox.TopIndex = listBox.Items.Count - visible1stColumnItems;
                else
                    listBox.TopIndex += delta;
            }

            listBox_SelectedIndexChanged(listBox, null);
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var listBox = sender as ListBox;

            if (listBox.MultiColumn)
            {
                CustomHScrollBar customHScrollBar = null;
                if (listBox is CustomListBox box)
                    customHScrollBar = box.hScrollBar;
                else if (listBox is CustomCheckedListBox checkedListBox)
                    customHScrollBar = checkedListBox.hScrollBar;

                if (customHScrollBar == null)
                    return;

                var (totalColumns, visibleColumns, visible1stColumnItems) = GetListBoxMetrics(listBox);

                var nRowRange = totalColumns - visibleColumns;


                var (_, nPixelRange, _, _, _) = customHScrollBar.GetMetrics();

                var nThumbLeft = 0;
                if (nRowRange > 0)
                    nThumbLeft = (int)Math.Round(((float)listBox.TopIndex / visible1stColumnItems) * nPixelRange / nRowRange);

                customHScrollBar.SetThumbLeft(nThumbLeft);
            }
            else
            {
                CustomVScrollBar customVScrollBar = null;
                if (listBox is CustomListBox box)
                    customVScrollBar = box.vScrollBar;
                else if (listBox is CustomCheckedListBox checkedListBox)
                    customVScrollBar = checkedListBox.vScrollBar;

                if (customVScrollBar == null)
                    return;

                var (_, _, visible1stColumnItems) = GetListBoxMetrics(listBox);

                var nRowRange = listBox.Items.Count - visible1stColumnItems;


                var (_, nPixelRange, _, _, _) = customVScrollBar.GetMetrics();

                var nThumbTop = 0;
                if (nRowRange > 0)
                    nThumbTop = (int)Math.Round((float)listBox.TopIndex * nPixelRange / nRowRange);

                customVScrollBar.SetThumbTop(nThumbTop);
            }
        }

        //Returns: totalLines, visibleLines, visible1stColumnItems
        internal static (int, int, int) GetListBoxMetrics(ListBox listBox)
        {
            var totalColumns = 1;
            var visibleColumns = 1;

            var maxVisibleColumnItems = listBox.ClientSize.Height / listBox.ItemHeight;
            int visible1stColumnItems;

            if (listBox.MultiColumn)
            {
                var columnWidth = listBox.ColumnWidth;

                if (columnWidth > 0)
                    visibleColumns = listBox.ClientSize.Width / columnWidth;

                if (listBox.Items.Count > 0)
                {
                    totalColumns = listBox.Items.Count / maxVisibleColumnItems;
                    if (listBox.Items.Count % maxVisibleColumnItems > 0)
                        totalColumns++;
                }
            }


            if (visibleColumns > totalColumns)
                visibleColumns = totalColumns;


            if (listBox.Items.Count > maxVisibleColumnItems)
                visible1stColumnItems = maxVisibleColumnItems;
            else
                visible1stColumnItems = listBox.Items.Count;

            return (totalColumns, visibleColumns, visible1stColumnItems);
        }

        //Returns Minimum, Maximum, LargeChange
        internal static (int, int, int) GetListBoxScrollBarMetrics(Control scrolledControl, Control refScrollBar)
        {
            var listBox = scrolledControl as ListBox;

            var (totalColumns, visibleColumns, visible1stColumnItems) = GetListBoxMetrics(listBox); //-V3149

            if (refScrollBar is CustomHScrollBar && totalColumns > 0)
                return (0, totalColumns, visibleColumns);
            else if (refScrollBar is CustomHScrollBar)
                return (0, 1, 1);
            else if (refScrollBar is CustomVScrollBar && listBox.Items.Count > 0)
                return (0, listBox.Items.Count, visible1stColumnItems);
            else if (refScrollBar is CustomVScrollBar)
                return (0, 1, 1);

            throw new Exception("Invalid scroll bar class: " + refScrollBar.GetType().FullName);
        }

        protected void tbCustomVScrollBar_Scroll(object sender, EventArgs e)
        {
            tbCustomVScrollBar_ValueChanged(sender, e);
        }

        protected void tbCustomVScrollBar_ValueChanged(object sender, EventArgs e)
        {
            var customVScrollBar = sender as CustomVScrollBar;
            var textBox = customVScrollBar.ScrolledControl as TextBox;

            if (textBox.Text.Length > 0)
            {
                var (totalLines, visibleLines) = GetTextBoxMetrics(textBox);

                var nRowRange = totalLines - visibleLines;
                var (_, nPixelRange, _, _, _) = customVScrollBar.GetMetrics();

                var scrollPosition = textBox.GetScrollPosition();

                var newFirstDisplayedLine = scrollPosition;
                if (nPixelRange > 0)
                    newFirstDisplayedLine = (int)Math.Round((float)nRowRange * customVScrollBar.GetThumbTop()
                        / nPixelRange);

                textBox.Scroll(newFirstDisplayedLine - scrollPosition);

                customVScrollBar.Invalidate();
                textBox.Invalidate();
            }
        }

        protected void ctbCustomVScrollBar_Scroll(object sender, EventArgs e)
        {
            ctbCustomVScrollBar_ValueChanged(sender, e);
        }

        protected void ctbCustomVScrollBar_ValueChanged(object sender, EventArgs e)
        {
            var customVScrollBar = sender as CustomVScrollBar;
            var textBox = customVScrollBar.ScrolledControl as CustomTextBox;

            if (textBox.Text.Length > 0)
            {
                var (totalLines, visibleLines) = GetTextBoxMetrics(textBox);

                var nRowRange = totalLines - visibleLines;
                var (_, nPixelRange, _, _, _) = customVScrollBar.GetMetrics();

                var scrollPosition = textBox.ScrollPosition;

                var newFirstDisplayedLine = scrollPosition;
                if (nPixelRange > 0)
                    newFirstDisplayedLine = (int)Math.Round((float)nRowRange * customVScrollBar.GetThumbTop()
                        / nPixelRange);

                textBox.Scroll(newFirstDisplayedLine - scrollPosition);

                customVScrollBar.Invalidate();
                textBox.Invalidate();
            }
        }

        private void customOrStandardTextBox_MouseWheel(object sender, MouseEventArgs e)
        {
            int delta;

            if ((ModifierKeys & Keys.Alt) == 0)
                delta = -e.Delta / 32; //---
            else
                delta = -e.Delta / 8;

            if (sender is CustomTextBox box)
            {
                box.Scroll(delta);
                customTextBoxScrolled(box, null);
            }
            else
            {
                (sender as TextBox).Scroll(delta);
                textBoxScrolled(sender as TextBox, null);
            }
        }

        private void customOrStandardTextBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            if (sender is CustomTextBox customTextBox)
            {
                customTextBox.UpdateCaretPositionForScrolling(customTextBox.GetCaretPosition());

                customTextBoxScrolled(customTextBox, null);
            }
            else
            {
                textBoxScrolled(sender as TextBox, null);
            }
        }

        private void customOrStandardTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            if ((MouseButtons & MouseButtons.Left) == 0)
                return;

            if (sender is CustomTextBox customTextBox)
            {
                customTextBox.UpdateCaretPositionForScrolling(customTextBox.GetCaretPosition());

                customTextBoxScrolled(customTextBox, null);
            }
            else
            {
                textBoxScrolled(sender as TextBox, null);
            }
        }

        private void customOrStandardTextBox_KeyUpDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;

            if (!textBox.ReadOnly
                || e.KeyData == Keys.PageUp || e.KeyData == Keys.PageDown || e.KeyData == Keys.Home || e.KeyData == Keys.End
                || e.KeyData == Keys.Up || e.KeyData == Keys.Down || e.KeyData == Keys.Left || e.KeyData == Keys.Right)
            {
                if (sender is CustomTextBox customTextBox)
                {
                    customTextBox.UpdateCaretPositionForScrolling(textBox.GetCaretPosition());
                    customTextBoxScrolled(customTextBox, null);
                }
                else
                {
                    textBoxScrolled(textBox, null);
                }
            }
        }

        private void textBoxScrolled(TextBox textBox, EventArgs e)
        {
            var customVScrollBar = ControlsTools.FindControlChild<CustomVScrollBar>(textBox.Parent)
                                   ?? ControlsTools.FindControlChild<CustomVScrollBar>(textBox.Parent.Parent);

            var (totalLines, visibleLines) = GetTextBoxMetrics(textBox);
            var nRowRange = totalLines - visibleLines;

            var (_, nPixelRange, _, _, _) = customVScrollBar.GetMetrics();

            var nThumbTop = 0;
            if (nRowRange > 0)
                nThumbTop = (int)Math.Round((float)textBox.GetScrollPosition() * nPixelRange / nRowRange);

            customVScrollBar.SetThumbTop(nThumbTop);
        }

        private void customTextBoxScrolled(CustomTextBox textBox, EventArgs e)
        {
            var customVScrollBar = textBox.vScrollBar;

            var (totalLines, visibleLines) = GetTextBoxMetrics(textBox);
            var nRowRange = totalLines - visibleLines;

            var (_, nPixelRange, _, _, _) = customVScrollBar.GetMetrics();

            var nThumbTop = 0;
            if (nRowRange > 0)
                nThumbTop = (int)Math.Round((float)textBox.ScrollPosition * nPixelRange / nRowRange);

            customVScrollBar.SetThumbTop(nThumbTop);
        }

        //Returns: totalLines, visibleLines
        internal static (int, int) GetTextBoxMetrics(TextBox textBox)
        {
            var totalLines = textBox.GetLineFromCharIndex(textBox.Text.Length - 1) + 1;
            int visibleLines;

            var lineHeight = TextRenderer.MeasureText("X", textBox.Font).Height;
            var maxVisibleLines = textBox.ClientSize.Height / lineHeight;


            if (totalLines > maxVisibleLines)
                visibleLines = maxVisibleLines;
            else
                visibleLines = totalLines;

            return (totalLines, visibleLines);
        }

        //Returns Minimum, Maximum, LargeChange
        internal static (int, int, int) GetTextBoxScrollBarMetrics(Control scrolledControl, Control refScrollBar)
        {
            var textBox = scrolledControl as TextBox;

            var (totalLines, visibleLines) = GetTextBoxMetrics(textBox); //-V3149

            if (refScrollBar is CustomVScrollBar && totalLines > 0)
                return (0, totalLines, visibleLines);
            else if (refScrollBar is CustomVScrollBar)
                return (0, 1, 1);

            throw new Exception("Invalid scroll bar class: " + refScrollBar.GetType().FullName);
        }

        private static (CustomHScrollBar, int, CustomVScrollBar, int, bool) UpdateDgvCustomScrollBarsInternal(DataGridView dataGridView)
        {
            var hScrollBar = ControlsTools.FindControlChild<HScrollBar>(dataGridView);
            var customHScrollBar = ControlsTools.FindControlChild<CustomHScrollBar>(dataGridView);
            var customHScrollBarVisibleHeight = 0;


            bool customScrollBarsMetricsChanged = false;

            if (customHScrollBar != null)
            {
                (_, int maximum, int largeChange) = GetDataGridViewScrollBarMetrics(dataGridView, customHScrollBar);

                if (customHScrollBar.Maximum != maximum || customHScrollBar.LargeChange != largeChange)
                {
                    customScrollBarsMetricsChanged = true;

                    customHScrollBar.Minimum = 0;
                    customHScrollBar.Maximum = maximum;
                    customHScrollBar.LargeChange = largeChange;
                }

                if (customHScrollBar.Maximum > customHScrollBar.LargeChange)
                    customHScrollBarVisibleHeight = customHScrollBar.Height;
            }


            var vScrollBar = ControlsTools.FindControlChild<VScrollBar>(dataGridView);
            var customVScrollBar = ControlsTools.FindControlChild<CustomVScrollBar>(dataGridView);
            var customVScrollBarVisibleWidth = 0;


            if (customVScrollBar != null)
            {
                (_, int maximum, int largeChange) = GetDataGridViewScrollBarMetrics(dataGridView, customVScrollBar);

                if (customVScrollBar.Maximum != maximum || customVScrollBar.LargeChange != largeChange)
                {
                    customScrollBarsMetricsChanged = true;

                    customVScrollBar.Minimum = 0;
                    customVScrollBar.Maximum = maximum;
                    customVScrollBar.LargeChange = largeChange;
                }

                if (customVScrollBar.Maximum > customVScrollBar.LargeChange)
                    customVScrollBarVisibleWidth = customVScrollBar.Width;
            }


            return (customHScrollBar, customHScrollBarVisibleHeight, customVScrollBar, customVScrollBarVisibleWidth, customScrollBarsMetricsChanged);
        }

        private static (CustomHScrollBar, int, CustomVScrollBar, int, bool) UpdateClbCustomScrollBarsInternal(ListBox listBox)
        {
            var customHScrollBarVisibleHeight = 0;
            var customVScrollBarVisibleWidth = 0;


            var (totalColumns, visibleColumns, visible1stColumnItems) = GetListBoxMetrics(listBox);

            CustomHScrollBar hScrollBar = null;
            CustomVScrollBar vScrollBar = null;

            bool customScrollBarsMetricsChanged = false;

            if (listBox is CustomListBox customListBox)
            {
                hScrollBar = customListBox.hScrollBar;
                vScrollBar = customListBox.vScrollBar;
            }
            else if (listBox is CustomCheckedListBox customCheckedListBox)
            {
                hScrollBar = customCheckedListBox.hScrollBar;
                vScrollBar = customCheckedListBox.vScrollBar;
            }

            if (listBox.MultiColumn && hScrollBar != null)
            {
                if (hScrollBar.Maximum != totalColumns || hScrollBar.LargeChange != visibleColumns)
                {
                    customScrollBarsMetricsChanged = true;

                    hScrollBar.Minimum = 0;
                    hScrollBar.Maximum = totalColumns;
                    hScrollBar.LargeChange = visibleColumns;
                }

                customHScrollBarVisibleHeight = hScrollBar.Height;
            }
            else if (vScrollBar != null)
            {
                if (vScrollBar.Maximum != listBox.Items.Count || vScrollBar.LargeChange != visible1stColumnItems)
                {
                    customScrollBarsMetricsChanged = true;

                    vScrollBar.Minimum = 0;
                    vScrollBar.Maximum = listBox.Items.Count;
                    vScrollBar.LargeChange = visible1stColumnItems;
                }

                customVScrollBarVisibleWidth = vScrollBar.Width;
            }

            return (hScrollBar, customHScrollBarVisibleHeight, vScrollBar, customVScrollBarVisibleWidth, customScrollBarsMetricsChanged);
        }

        private static (CustomVScrollBar, bool) UpdateTbCustomScrollBarsInternal(TextBox textBox)
        {
            var (totalLines, visibleLines) = GetTextBoxMetrics(textBox);

            var vScrollBar = ControlsTools.FindControlChild<CustomVScrollBar>(textBox.Parent);
            bool textBoxScrollBarsMetricsChanged = false;


            if (vScrollBar != null)
            {
                if (vScrollBar.Maximum != totalLines || vScrollBar.LargeChange != visibleLines)
                {
                    textBoxScrollBarsMetricsChanged = true;

                    vScrollBar.Minimum = 0;
                    vScrollBar.Maximum = totalLines;
                    vScrollBar.LargeChange = visibleLines;
                }
            }

            return (vScrollBar, textBoxScrollBarsMetricsChanged);
        }

        private static (CustomVScrollBar, bool) UpdateCtbCustomScrollBarsInternal(CustomTextBox textBox)
        {
            var (totalLines, visibleLines) = GetTextBoxMetrics(textBox);

            var vScrollBar = textBox.vScrollBar;
            bool customScrollBarsMetricsChanged = false;

            if (vScrollBar != null)
            {
                if (vScrollBar.Maximum != totalLines || vScrollBar.LargeChange != visibleLines)
                {
                    customScrollBarsMetricsChanged = true;

                    vScrollBar.Minimum = 0;
                    vScrollBar.Maximum = totalLines;
                    vScrollBar.LargeChange = visibleLines;
                }
            }

            return (vScrollBar, customScrollBarsMetricsChanged);
        }

        //Use -1 for columnOrItemCount/rowCount to always redraw horizontal/vertical scroll bars; 0 to never redraw them
        internal void updateCustomScrollBars(Control scrolledControl, int columnCount = -1, int rowCount = -1)
        {
            if (useSkinColors)
                UpdateCustomScrollBars(scrolledControl, columnCount, rowCount);
        }

        //Use -1 for columnOrItemCount/rowCount to always redraw horizontal/vertical scroll bars; 0 to never redraw them
        internal static void UpdateCustomScrollBars(Control scrolledControl, int columnOrItemCount = -1, int rowCount = -1)
        {
            CustomHScrollBar customHScrollBar = null;
            int customHScrollBarVisibleHeight = 0;

            CustomVScrollBar customVScrollBar = null;
            int customVScrollBarVisibleWidth = 0;

            bool customScrollBarsMetricsChanged = false;


            if (columnOrItemCount == 0)
                rowCount = -1;
            else if (rowCount == 0)
                columnOrItemCount = -1;


            if (scrolledControl is DataGridView dataGridView)
            {
                bool dataGridViewScrollBarsMetricsChanged;

                (customHScrollBar, customHScrollBarVisibleHeight, customVScrollBar, customVScrollBarVisibleWidth, dataGridViewScrollBarsMetricsChanged)
                    = UpdateDgvCustomScrollBarsInternal(dataGridView);

                customScrollBarsMetricsChanged |= dataGridViewScrollBarsMetricsChanged;
            }
            else if (scrolledControl is CustomListBox || scrolledControl is CustomCheckedListBox)
            {
                bool listBoxScrollBarsMetricsChanged;

                (customHScrollBar, customHScrollBarVisibleHeight, customVScrollBar, customVScrollBarVisibleWidth, listBoxScrollBarsMetricsChanged)
                    = UpdateClbCustomScrollBarsInternal(scrolledControl as ListBox);

                customScrollBarsMetricsChanged |= listBoxScrollBarsMetricsChanged;
            }
            else if (scrolledControl is CustomTextBox control)
            {
                bool customTextBoxScrollBarsMetricsChanged;

                customVScrollBarVisibleWidth = 1; //Must be > 0 to make vert. scroll bar visible 
                (customVScrollBar, customTextBoxScrollBarsMetricsChanged) = UpdateCtbCustomScrollBarsInternal(control);

                customScrollBarsMetricsChanged |= customTextBoxScrollBarsMetricsChanged;
            }
            else if (scrolledControl is TextBox textBox)
            {
                bool textBoxScrollBarsMetricsChanged;

                customVScrollBarVisibleWidth = 1; //Must be > 0 to make vert. scroll bar visible 
                (customVScrollBar, textBoxScrollBarsMetricsChanged) = UpdateTbCustomScrollBarsInternal(textBox);

                customScrollBarsMetricsChanged |= textBoxScrollBarsMetricsChanged;
            }


            if (columnOrItemCount != -1 && ((columnOrItemCount - 1) & 0x1f) != 0) //Let's skip scroll bar redraw
            {
                customScrollBarsMetricsChanged = false;
            }
            else if (customHScrollBar != null && customScrollBarsMetricsChanged)
            {
                customHScrollBar.AdjustReservedSpace(customVScrollBarVisibleWidth);
                customHScrollBar.ResetMetricsSize(scrolledControl.Width);
                customHScrollBar.Visible = (customHScrollBarVisibleHeight > 0);
                customHScrollBar.Invalidate();
            }

            if (rowCount != -1 && ((rowCount - 1) & 0x1f) != 0) //Let's skip scroll bar redraw
            {
                customScrollBarsMetricsChanged = false;
            }
            else if (customHScrollBar != null && customScrollBarsMetricsChanged)
            {
                if (customHScrollBar?.Visible == true)
                    customVScrollBar.AdjustReservedSpace(0);
                else
                    customVScrollBar.AdjustReservedSpace(customHScrollBarVisibleHeight);

                customVScrollBar.ResetMetricsSize(scrolledControl.Height);
                customVScrollBar.Visible = (customVScrollBarVisibleWidth > 0);
                customVScrollBar.Invalidate();
            }


            if (customScrollBarsMetricsChanged)
            {
                scrolledControl.Invalidate();
                Application.DoEvents();
            }
        }

        //Returns Minimum, Maximum, LargeChange
        internal static (int, int, int) GetDataGridViewScrollBarMetrics(Control scrolledControl, Control customScrollBar)
        {
            var dataGridView = scrolledControl as DataGridView;

            if (customScrollBar is CustomHScrollBar hScrollBar)
            {
                int maximum = dataGridView.Columns.GetColumnsWidth(DataGridViewElementStates.Visible);

                int largeChange = maximum;
                if (dataGridView.ColumnCount > 0 && dataGridView.DisplayedColumnCount(false) < dataGridView.Columns.GetColumnCount(DataGridViewElementStates.Visible))
                    largeChange = dataGridView.Columns.GetColumnsWidth(DataGridViewElementStates.Displayed) - (int)(dataGridView.Columns[dataGridView.ColumnCount - 1].Width / 1f);
                else if (dataGridView.ColumnCount > 0)
                    largeChange = dataGridView.Columns.GetColumnsWidth(DataGridViewElementStates.Displayed);

                return (0, maximum, largeChange);
            }
            else if (customScrollBar is CustomVScrollBar vScrollBar)
            {
                int maximum = dataGridView.Rows.GetRowsHeight(DataGridViewElementStates.Visible);

                int largeChange = maximum;
                if (dataGridView.RowCount > 0 && dataGridView.DisplayedRowCount(false) < dataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible))
                    largeChange = dataGridView.Rows.GetRowsHeight(DataGridViewElementStates.Displayed) - (int)(dataGridView.Rows[dataGridView.RowCount - 1].Height / 1f);
                else if (dataGridView.RowCount > 0)
                    largeChange = dataGridView.Rows.GetRowsHeight(DataGridViewElementStates.Displayed);

                return (0, maximum, largeChange);
            }

            throw new Exception("Invalid reference scroll bar class: " + customScrollBar.GetType().FullName);
        }


        //Returns Minimum, Maximum, LargeChange
        //internal static (int, int, int) GetDataGridViewVScrollBarMetrics(Control scrolledControl, Control refScrollBar) //-----
        //{
        //    if (refScrollBar is VScrollBar scrollBar)
        //        return (scrollBar.Minimum, scrollBar.Maximum, scrollBar.LargeChange);

        //    throw new Exception("Invalid reference scroll bar class: " + refScrollBar.GetType().FullName);
        //}

        private void customScrollBarParent_SizeChanged(object sender, EventArgs e)
        {
            var control = sender as Control;

            var customHScrollBar = ControlsTools.FindControlChild<CustomHScrollBar>(control);
            customHScrollBar.ResetMetricsSize(control.Width); //-V3080

            var customVScrollBar = ControlsTools.FindControlChild<CustomVScrollBar>(control);
            customVScrollBar.ResetMetricsSize(control.Height); //-V3080

            control.Invalidate();
        }


        private void customScrollBarListBox_SizeChanged(object sender, EventArgs e)
        {
            var listBox = sender as ListBox;

            var customHScrollBar = ControlsTools.FindControlChild<CustomHScrollBar>(listBox.Parent);
            customHScrollBar?.ResetMetricsSize(listBox.Width);

            var customVScrollBar = ControlsTools.FindControlChild<CustomVScrollBar>(listBox.Parent);
            customVScrollBar?.ResetMetricsSize(listBox.Height);

            listBox.Invalidate();
        }

        private void customScrollBarCustomListBox_SizeChanged(object sender, EventArgs e)
        {
            if (sender is CustomListBox box)
            {
                var customHScrollBar = box.hScrollBar;
                customHScrollBar?.ResetMetricsSize(box.Width);

                var customVScrollBar = box.vScrollBar;
                customVScrollBar?.ResetMetricsSize(box.Height);

                box.Invalidate();
            }
            else if (sender is CustomCheckedListBox listBox)
            {
                var customHScrollBar = listBox.hScrollBar;
                customHScrollBar?.ResetMetricsSize(listBox.Width);

                var customVScrollBar = listBox.vScrollBar;
                customVScrollBar?.ResetMetricsSize(listBox.Height);

                listBox.Invalidate();
            }
        }

        private void customScrollBarTextBox_SizeChanged(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;

            var customVScrollBar = ControlsTools.FindControlChild<CustomVScrollBar>(textBox.Parent);
            customVScrollBar.ResetMetricsSize(textBox.Height); //-V3080

            textBox.Invalidate();
        }

        private void customScrollBarTextBoxBorder_SizeChanged(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;

            var customVScrollBar = ControlsTools.FindControlChild<CustomVScrollBar>(textBox.Parent.Parent);
            customVScrollBar.ResetMetricsSize(textBox.Height); //-V3080

            textBox.Invalidate();
        }

        private void customScrollBarCustomTextBox_SizeChanged(object sender, EventArgs e)
        {
            var textBox = sender as CustomTextBox;

            var customVScrollBar = textBox.vScrollBar;
            customVScrollBar.ResetMetricsSize(textBox.Height);

            textBox.Invalidate();
        }

        protected void scrollBar_VisibleChanged(object sender, EventArgs e)
        {
            var control = sender as Control;

            if (control.Visible)
            {
                if (sender is HScrollBar hScrollBar)
                {
                    var customHScrollBar = ControlsTools.FindControlChild<CustomHScrollBar>(hScrollBar.Parent);

                    hScrollBar.Visible = false;
                    hScrollBar.Invalidate();

                    customHScrollBar.Visible = true; //-V3080
                    customHScrollBar.Invalidate();

                    Application.DoEvents();
                }
                else if (sender is VScrollBar vScrollBar)
                {
                    var customVScrollBar = ControlsTools.FindControlChild<CustomVScrollBar>(vScrollBar.Parent);

                    vScrollBar.Visible = false;
                    vScrollBar.Invalidate();

                    customVScrollBar.Visible = true; //-V3080
                    customVScrollBar.Invalidate();

                    Application.DoEvents();
                }
            }
        }

        internal static Control FindFocusedControl(Control control)
        {
            var container = control as ContainerControl;
            while (container != null)
            {
                control = container.ActiveControl;
                container = control as ContainerControl;
            }

            return control;
        }

        //Returns the number of rows, which require formatting changed tags, 0 if no formatting is required
        internal static int AddRowsToTable(PluginWindowTemplate form, DataGridView dataGridView, List<string[]> rowList, bool itsLastRowRange, bool selectLastRow)
        {
            int rowListCount = rowList.Count;

            if (rowListCount > 0)
            {
                if (itsLastRowRange || ((rowListCount & 0x1f) == 0))
                {
                    int startRowIndex = dataGridView.RowCount;

                    var rows = new DataGridViewRow[rowListCount];

                    for (int i = 0; i < rowListCount; i++)
                    {
                        DataGridViewRow row = new DataGridViewRow();
                        row.CreateCells(dataGridView);

                        for (int j = 0; j < rowList[i].Length; j++)
                            row.Cells[j].Value = rowList[i][j];

                        rows[i] = row;
                    }

                    rowList.Clear();
                    dataGridView.Rows.AddRange(rows);

                    if (selectLastRow)
                        dataGridView.CurrentCell = dataGridView[0, dataGridView.RowCount - 1];

                    if (form.useSkinColors)
                        UpdateCustomScrollBars(dataGridView, 1, dataGridView.RowCount);

                    return rowListCount;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (form.useSkinColors)
                    UpdateCustomScrollBars(dataGridView);

                return 0;
            }
        }

        internal static void FormatChangedTags(PluginWindowTemplate form, DataGridView dataGridView, int rowCountToFormat, DataGridViewFormatChangedTags dataGridViewFormatChangedTags = null)
        {
            if (dataGridViewFormatChangedTags != null)
                for (int j = dataGridView.RowCount - rowCountToFormat; j < dataGridView.RowCount; j++)
                    dataGridViewFormatChangedTags(dataGridView, j);


            if (dataGridView.RowCount > 0)
                dataGridView.FirstDisplayedCell = dataGridView.CurrentCell;

            if (form.useSkinColors)
                UpdateCustomScrollBars(dataGridView, 1, dataGridView.RowCount);
        }

        protected CustomComboBox getKeyEventCustomComboBox(object sender)
        {
            CustomComboBox customComboBox;

            if (sender is ToolStripDropDown toolStripDropDown) //It's opened custom combo box list
            {
                customComboBox = toolStripDropDown.Tag as CustomComboBox;
            }
            else if (sender is Control control)
            {
                var focusedControl = FindFocusedControl(control);

                if (focusedControl == null || !(focusedControl.Parent is CustomComboBox box))
                    return null;

                customComboBox = box;
            }
            else
            {
                return null;
            }

            if (!customComboBox.MustKeyEventsBeHandled)
                return null;

            return customComboBox;
        }

        internal void CustomComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            var customComboBox = getKeyEventCustomComboBox(sender);

            if (customComboBox == null)
                return;


            //First char may be some symbol like "<", let's search for the second char in this case
            var searchedLetter = e.KeyChar.ToString().ToLower();


            for (var searchedIndex = 0; searchedIndex < 2; searchedIndex++)
            {
                foreach (string tagName in customComboBox.Items)
                {
                    if (tagName.Length > searchedIndex && tagName[searchedIndex].ToString().ToLower() == searchedLetter)
                    {
                        customComboBox.SelectedItem = tagName;
                        return;
                    }
                }
            }
        }

        //It's for handling key up/key down events. See above the handling of generic letters.
        internal void CustomComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            var form = sender as Form;
            var customComboBox = getKeyEventCustomComboBox(sender);

            if (customComboBox == null)
                return;


            if (e.KeyCode == Keys.Space)
                customComboBox.listBox_ItemChosen(null, null);
            if (e.KeyCode == Keys.Enter)
                form.AcceptButton.PerformClick();
            else if (e.KeyCode == Keys.Up && customComboBox.SelectedIndex > 0)
                customComboBox.SelectedIndex--;
            else if (e.KeyCode == Keys.PageUp && customComboBox.SelectedIndex > 19)
                customComboBox.SelectedIndex -= 20;
            else if (e.KeyCode == Keys.PageUp)
                customComboBox.SelectedIndex = 0;
            else if (e.KeyCode == Keys.Down && customComboBox.SelectedIndex < customComboBox.Items.Count - 1)
                customComboBox.SelectedIndex++;
            else if (e.KeyCode == Keys.PageDown && customComboBox.SelectedIndex < customComboBox.Items.Count - 21)
                customComboBox.SelectedIndex += 20;
            else if (e.KeyCode == Keys.PageDown)
                customComboBox.SelectedIndex = customComboBox.Items.Count - 1;

        }

        //Returns: enabled fore & back colors, disabled fore & back colors
        internal void setSkinnedControlColors(Control control, bool? enable) //-----
        {
            if (useSkinColors)
            {
                if (control is Button button) //button != null always if it is Button
                {
                    var acceptButton = AcceptButton as Button;

                    //Default button
                    if ((control.IsEnabled() && enable == null) || enable == true && button == acceptButton)
                    {
                        button.ForeColor = controlHighlightForeColor;
                        button.BackColor = controlHighlightBackColor;
                        button.FlatAppearance.BorderColor = ButtonFocusedBorderColor;
                    }
                    //else if ((control.IsEnabled() && state == null) || state == true && button.Focused) //-----
                    //{
                    //   button.ForeColor = _buttonForeColor;
                    //   button.BackColor = _buttonBackColor;
                    //   button.FlatAppearance.BorderColor = ButtonFocusedBorderColor;
                    //}
                    //Combo box button
                    else if ((control.IsEnabled() && enable == null) || enable == true && button.Parent is CustomComboBox)
                    {
                        button.ForeColor = buttonForeColor;
                        button.BackColor = narrowScrollBarBackColor;
                        button.FlatAppearance.BorderColor = ScrollBarBackColor;
                    }
                    //Generic button
                    else if ((control.IsEnabled() && enable == null) || enable == true)
                    {
                        button.ForeColor = buttonForeColor;
                        button.BackColor = buttonBackColor;
                        button.FlatAppearance.BorderColor = ButtonBorderColor;
                    }


                    //Combo box disabled button
                    else if (button.Parent is CustomComboBox)
                    {
                        button.ForeColor = buttonForeColor;
                        button.BackColor = InputPanelBackColor; //---
                        //button.BackColor = narrowScrollBarBackColor; //---
                        button.FlatAppearance.BorderColor = ButtonBorderColor;
                    }
                    //Generic disabled button
                    else
                    {

                        button.ForeColor = buttonDisabledForeColor;
                        button.BackColor = buttonDisabledBackColor;
                        button.FlatAppearance.BorderColor = ButtonBorderColor;
                    }
                }
                else if (control is CheckBox)
                {
                    if ((control.IsEnabled() && enable == null) || enable == true)
                        control.ForeColor = FormForeColor;
                    else
                        control.ForeColor = DimmedAccentColor;

                    control.BackColor = FormBackColor;
                }
                else if (control is RadioButton)
                {
                    if ((control.IsEnabled() && enable == null) || enable == true)
                        control.ForeColor = FormForeColor;
                    else
                        control.ForeColor = DimmedAccentColor;

                    control.BackColor = FormBackColor;
                }
                else if (control is Label)
                {
                    if ((control.IsEnabled() && enable == null) || enable == true)
                    {
                        if (control.ForeColor == FormForeColor || control.ForeColor == AccentColor || control.ForeColor == DimmedAccentColor)
                            control.ForeColor = AccentColor;
                        else
                            control.ForeColor = GetHighlightColor(control.ForeColor, AccentColor, FormBackColor);
                    }
                    else
                    {
                        if (control.ForeColor == FormForeColor || control.ForeColor == AccentColor || control.ForeColor == DimmedAccentColor)
                            control.ForeColor = DimmedAccentColor;
                        else
                            control.ForeColor = GetHighlightColor(control.ForeColor, DimmedAccentColor, FormBackColor);
                    }

                    control.BackColor = FormBackColor;
                }
                else if (control is CustomComboBox customComboBox)
                {
                    customComboBox.SetColors(enable);
                }
                else if (control is ComboBox comboBox && comboBox.DropDownStyle == ComboBoxStyle.Simple)
                {
                    if ((control.IsEnabled() && enable == null) || enable == true)
                    {
                        comboBox.ForeColor = InputControlForeColor;
                        comboBox.BackColor = InputControlBackColor;
                    }
                    else
                    {
                        comboBox.ForeColor = DimmedAccentColor;
                        comboBox.BackColor = InputPanelBackColor; //---
                        //comboBox.BackColor = InputControlDimmedBackColor; //---
                    }
                }
                else if (control is NumericUpDown numericUpDown)
                {
                    if (((control.IsEnabled() && enable == null) || enable == true) && !numericUpDown.ReadOnly)
                    {
                        control.ForeColor = InputControlForeColor;
                        control.BackColor = InputControlBackColor;
                    }
                    else if ((control.IsEnabled() && enable == null) || enable == true) //Enabled, but readonly
                    {
                        control.ForeColor = AccentColor;
                        control.BackColor = InputControlDimmedBackColor;
                    }
                    else
                    {
                        control.ForeColor = DimmedAccentColor;
                        control.BackColor = InputPanelBackColor; //---
                        //control.BackColor = InputControlDimmedBackColor; //---
                    }
                }
                else if (control is TextBox textBox)
                {
                    if (((control.IsEnabled() && enable == null) || enable == true) && !textBox.ReadOnly)
                    {
                        control.ForeColor = InputControlForeColor;
                        control.BackColor = InputControlBackColor;
                    }
                    else if ((control.IsEnabled() && enable == null) || enable == true) //Enabled, but readonly
                    {
                        control.ForeColor = AccentColor;
                        control.BackColor = InputControlDimmedBackColor;
                    }
                    else
                    {
                        control.ForeColor = DimmedAccentColor;
                        control.BackColor = InputPanelBackColor; //---
                        //control.BackColor = InputControlDimmedBackColor; //---
                    }
                }
                else if (control is ListBox)
                {
                    if ((control.IsEnabled() && enable == null) || enable == true)
                    {
                        control.ForeColor = InputControlForeColor;
                        control.BackColor = InputControlBackColor;
                    }
                    else
                    {
                        control.ForeColor = DimmedAccentColor;
                        control.BackColor = InputPanelBackColor; //---
                        //control.BackColor = InputControlDimmedBackColor; //---
                    }
                }
                else if (control is DataGridView)
                {
                    control.BackColor = HeaderCellStyle.BackColor;
                    control.ForeColor = HeaderCellStyle.ForeColor;
                }
                else if (control is FlatTabControl tabControl)
                {
                    if ((control.IsEnabled() && enable == null) || enable == true)
                    {
                        tabControl.ForeColor = AccentColor;
                        tabControl.BackColor = InputPanelBackColor;
                        tabControl.BorderColor = InputPanelBorderColor;

                        foreach (TabPage child in tabControl.TabPages)
                        {
                            child.ForeColor = AccentColor;
                            child.BackColor = InputPanelBackColor;
                            //child.myBorderColor = InputPanelBorderColor;
                        }
                    }
                    else
                    {
                        tabControl.ForeColor = DimmedAccentColor;
                        tabControl.BackColor = InputPanelBackColor;
                        tabControl.BorderColor = InputPanelBorderColor;

                        foreach (TabPage child in tabControl.TabPages)
                        {
                            child.ForeColor = DimmedAccentColor;
                            child.BackColor = InputPanelBackColor;
                            //child.myBorderColor = InputPanelBorderColor;
                        }
                    }
                }
                else if (control is GroupBox || control is SplitContainer || control is Panel)
                {
                    //foreach (Control child in control.Controls)
                    //    setSkinnedControlColors(child, enable);
                }
                else
                {
                    if ((control.IsEnabled() && enable == null) || enable == true)
                        control.ForeColor = AccentColor;
                    else
                        control.ForeColor = DimmedAccentColor;

                    control.BackColor = FormBackColor;
                }
            }
        }

        internal void skinControl(Control control, int borderWidth = -1)
        {
            WaitPreparedThemedBitmapsAndColors();

            if (control is SplitContainer splitContainer)
            {
                splitContainer.Panel1.BackColor = FormBackColor;
                splitContainer.Panel1.ForeColor = AccentColor;

                splitContainer.Panel2.BackColor = FormBackColor;
                splitContainer.Panel2.ForeColor = AccentColor;

                splitContainer.BackColor = SplitterColor;

                return;
            }

            if (control is Button button)
            {
                buttonLabels.AddReplace(button, button.Text);

                button.TextChanged += button_TextChanged;
                button.GotFocus += button_GotFocus;
                button.LostFocus += button_LostFocus;


                if (!useSkinColors)
                {
                    button.FlatStyle = FlatStyle.Standard;
                    buttonLabels.AddReplace(button, button.Text);
                }
                else if (UseNativeButtonPaint && !(button.Parent is CustomComboBox))
                {
                    setSkinnedControlColors(button, null);

                    button.FlatStyle = FlatStyle.Standard;
                    buttonLabels.AddReplace(button, button.Text);
                }
                else
                {
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 1;
                    button.FlatAppearance.MouseOverBackColor = ButtonMouseOverBackColor;
                    button.FlatAppearance.MouseDownBackColor = ButtonMouseOverBackColor;

                    button.EnabledChanged += button_EnabledChanged;
                    button.Paint += button_Paint;

                    button.Text = string.Empty;

                    setSkinnedControlColors(button, null);
                }

                return;
            }

            control.Enter += control_Enter;


            //SplitContainer and (disabled) Button above must be skinned in any case (even if using system colors)
            if (!useSkinColors)
            {
                if (control is CustomListBox || control is CustomCheckedListBox || control is CustomTextBox)
                {
                    var parent = control.Parent as TableLayoutPanel;

                    parent.ColumnStyles[1].Width = 0; //-V3149
                    var cellPosition = parent.GetCellPosition(control);
                    parent.RowStyles[cellPosition.Row + 1].Height = 0;
                }
                else if (control is TextBox textBox && control.Parent is TableLayoutPanel textBoxParent)
                {
                    if (textBox.Multiline)
                    {
                        textBoxParent.ColumnStyles[1].Width = 0;
                        var cellPosition = textBoxParent.GetCellPosition(control);
                        textBoxParent.RowStyles[cellPosition.Row + 1].Height = 0;
                    }
                }
                else if (control is ControlBorder controlBorder && controlBorder.control is TextBox textBox1 && control.Parent is TableLayoutPanel textBoxBorderParent)
                {
                    if (textBox1.Multiline)
                    {
                        textBoxBorderParent.ColumnStyles[1].Width = 0;
                        var cellPosition = textBoxBorderParent.GetCellPosition(control);
                        textBoxBorderParent.RowStyles[cellPosition.Row + 1].Height = 0;
                    }
                }
                else if (control is ComboBox comboBox)
                {
                    if (comboBox.DropDownStyle == ComboBoxStyle.DropDown || comboBox.DropDownStyle == ComboBoxStyle.DropDownList)
                    {
                        var customComboBox = new CustomComboBox(this, comboBox, false);

                        customComboBox.comboBox.FlatStyle = FlatStyle.System;

                        //For rendering high contrast cue banners. Works with DropDownLists only.
                        customComboBox.comboBox.DrawItem += comboBox_DrawItem;
                        customComboBox.comboBox.DrawMode = DrawMode.OwnerDrawFixed;
                    }
                    else
                    {
                        comboBox.FlatStyle = FlatStyle.System;
                    }
                }

                return;
            }


            //useSkinColors == true here
            if (control is ControlBorder textBoxBorder)
            {
                skinControl(textBoxBorder.control);
            }
            else if (control is FlatTabControl flatTabControl)
            {
                flatTabControl.Left -= 2;
                flatTabControl.Width += 4;

                flatTabControl.Top -= 2;
                flatTabControl.Height += 4;

                for (int i = 0; i < flatTabControl.TabCount; i++)
                {
                    int top = flatTabControl.TabPages[i].Padding.Top;
                    int bottom = flatTabControl.TabPages[i].Padding.Bottom;
                    int left = flatTabControl.TabPages[i].Padding.Left;
                    int right = flatTabControl.TabPages[i].Padding.Right;

                    flatTabControl.TabPages[i].Padding = new Padding(left + 1, top - 1, right - 2, bottom - 1);
                }
            }
            else if (control is CheckBox checkBox)
            {
                checkBox.FlatStyle = FlatStyle.System;
            }
            else if (control is RadioButton radioButton)
            {
                radioButton.FlatStyle = FlatStyle.System;
            }
            else if (control is GroupBox groupBox)
            {
                groupBox.FlatStyle = FlatStyle.Standard;
            }
            else if (control is NumericUpDown numericUpDown)
            {
                numericUpDown.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (control is ComboBox comboBox)
            {
                if (comboBox.DropDownStyle == ComboBoxStyle.DropDown || comboBox.DropDownStyle == ComboBoxStyle.DropDownList)
                    new CustomComboBox(this, comboBox, true);
                else
                    comboBox.FlatStyle = FlatStyle.Flat;
            }
            else if (control is TextBox textBox)
            {
                textBox.BorderStyle = BorderStyle.FixedSingle;

                if (textBox.Multiline)
                {
                    if (textBox.Parent is TableLayoutPanel)
                    {
                        textBox.ScrollBars = ScrollBars.None;
                        textBox.WordWrap = true;

                        var customVScrollBar = new CustomVScrollBar(this, textBox, GetTextBoxScrollBarMetrics, borderWidth);
                        customVScrollBar.CreateBrushes();

                        customVScrollBar.SmallChange = 3;
                        customVScrollBar.Scroll += tbCustomVScrollBar_Scroll;
                        customVScrollBar.ValueChanged += tbCustomVScrollBar_ValueChanged;

                        textBox.MouseUp += customOrStandardTextBox_MouseUp;
                        textBox.MouseMove += customOrStandardTextBox_MouseMove;
                        textBox.KeyDown += customOrStandardTextBox_KeyUpDown;
                        textBox.KeyUp += customOrStandardTextBox_KeyUpDown;
                        textBox.MouseWheel += customOrStandardTextBox_MouseWheel;
                        textBox.SizeChanged += customScrollBarTextBox_SizeChanged;
                    }
                    else if (textBox.Parent is ControlBorder)
                    {
                        if (textBox.Parent?.Parent is TableLayoutPanel)
                        {
                            textBox.ScrollBars = ScrollBars.None;
                            textBox.WordWrap = true;

                            var customVScrollBar = new CustomVScrollBar(this, textBox, GetTextBoxScrollBarMetrics, borderWidth);
                            customVScrollBar.CreateBrushes();

                            customVScrollBar.SmallChange = 3;
                            customVScrollBar.Scroll += tbCustomVScrollBar_Scroll;
                            customVScrollBar.ValueChanged += tbCustomVScrollBar_ValueChanged;

                            textBox.MouseUp += customOrStandardTextBox_MouseUp;
                            textBox.MouseMove += customOrStandardTextBox_MouseMove;
                            textBox.KeyDown += customOrStandardTextBox_KeyUpDown;
                            textBox.KeyUp += customOrStandardTextBox_KeyUpDown;
                            textBox.MouseWheel += customOrStandardTextBox_MouseWheel;
                            textBox.SizeChanged += customScrollBarTextBoxBorder_SizeChanged;
                        }
                    }
                    else if (control is CustomTextBox)
                    {
                        textBox.ScrollBars = ScrollBars.None;
                        textBox.WordWrap = true;

                        var customVScrollBar = new CustomVScrollBar(this, textBox, GetTextBoxScrollBarMetrics, borderWidth);
                        customVScrollBar.CreateBrushes();

                        customVScrollBar.SmallChange = 3;
                        customVScrollBar.Scroll += ctbCustomVScrollBar_Scroll;
                        customVScrollBar.ValueChanged += ctbCustomVScrollBar_ValueChanged;

                        textBox.MouseUp += customOrStandardTextBox_MouseUp;
                        textBox.MouseMove += customOrStandardTextBox_MouseMove;
                        textBox.KeyDown += customOrStandardTextBox_KeyUpDown;
                        textBox.KeyUp += customOrStandardTextBox_KeyUpDown;
                        textBox.MouseWheel += customOrStandardTextBox_MouseWheel;
                        textBox.SizeChanged += customScrollBarCustomTextBox_SizeChanged;
                    }
                }
            }
            else if (control is ListBox)
            {
                var listBox = control as ListBox;

                listBox.HorizontalScrollbar = false;
                listBox.BorderStyle = BorderStyle.FixedSingle;

                if (listBox is ListBox || listBox is CheckedListBox)
                    global::NativeMethods.HideScrollBar(listBox.Handle, global::NativeMethods.SB_BOTH);


                if (listBox.MultiColumn)
                {
                    var customHScrollBar = new CustomHScrollBar(this, listBox, GetListBoxScrollBarMetrics);
                    customHScrollBar.CreateBrushes();

                    customHScrollBar.Scroll += listBoxCustomHScrollBar_Scroll;
                    customHScrollBar.ValueChanged += listBoxCustomHScrollBar_ValueChanged;

                    listBox.SelectedIndexChanged += listBox_SelectedIndexChanged;
                    listBox.MouseWheel += listBox_MouseWheel;

                    if (listBox is CustomListBox || listBox is CustomCheckedListBox)
                        listBox.SizeChanged += customScrollBarCustomListBox_SizeChanged;
                    else
                        listBox.SizeChanged += customScrollBarListBox_SizeChanged;
                }
                else
                {
                    var customVScrollBar = new CustomVScrollBar(this, listBox, GetListBoxScrollBarMetrics, borderWidth);
                    customVScrollBar.CreateBrushes();

                    customVScrollBar.Scroll += listBoxCustomVScrollBar_Scroll;
                    customVScrollBar.ValueChanged += listBoxCustomVScrollBar_ValueChanged;

                    listBox.SelectedIndexChanged += listBox_SelectedIndexChanged;
                    listBox.MouseWheel += listBox_MouseWheel;

                    if (listBox is CustomListBox || listBox is CustomCheckedListBox)
                        listBox.SizeChanged += customScrollBarCustomListBox_SizeChanged;
                    else
                        listBox.SizeChanged += customScrollBarListBox_SizeChanged;
                }
            }
            else if (control is DataGridView dataGridView)
            {
                //Let's turn on data grid view double buffering
                Type dgvType = dataGridView.GetType();//---
                PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(dataGridView, true, null);


                dataGridView.ScrollBars = ScrollBars.Both;

                dataGridView.EnableHeadersVisualStyles = false;
                dataGridView.BorderStyle = BorderStyle.FixedSingle;
                dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dataGridView.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                dataGridView.BackgroundColor = UnchangedCellStyle.BackColor;
                dataGridView.DefaultCellStyle = new DataGridViewCellStyle(UnchangedCellStyle);


                var hScrollBar = ControlsTools.FindControlChild<HScrollBar>(dataGridView);
                var vScrollBar = ControlsTools.FindControlChild<VScrollBar>(dataGridView);

                var customHScrollBar = new CustomHScrollBar(this, dataGridView, GetDataGridViewScrollBarMetrics, hScrollBar, true);
                customHScrollBar.CreateBrushes();
                hScrollBar.Visible = false; //-V3080

                customHScrollBar.Scroll += dgvCustomHScrollBar_Scroll;
                customHScrollBar.ValueChanged += dgvCustomHScrollBar_ValueChanged;

                hScrollBar.VisibleChanged += scrollBar_VisibleChanged;


                var customVScrollBar = new CustomVScrollBar(this, dataGridView, GetDataGridViewScrollBarMetrics, 0, vScrollBar, true);
                customVScrollBar.CreateBrushes();
                vScrollBar.Visible = false; //-V3080

                customVScrollBar.Scroll += dgvCustomVScrollBar_Scroll;
                customVScrollBar.ValueChanged += dgvCustomVScrollBar_ValueChanged;

                vScrollBar.VisibleChanged += scrollBar_VisibleChanged;


                dataGridView.Scroll += dataGridView_Scroll;
                dataGridView.MouseWheel += dataGridView_MouseWheel;
                dataGridView.SizeChanged += customScrollBarParent_SizeChanged;

                if (dataGridView.Parent is FlatTabControl)
                {

                }
                else if (dataGridView.Parent is FlatTabControl || dataGridView.Parent?.Parent is FlatTabControl)
                {

                }
            }

            setSkinnedControlColors(control, null);
        }

        internal void fillControlsReferencesRemarks()
        {
            for (var i = allControls.Count - 1; i >= 0; i--)
            {
                var control = allControls[i];

                var (controls2X, _, controls2Y, _, _, _, _, _, remarks) = GetReferredControlsControlLevelMarksRemarks(control);

                if (controls2X != null)
                {
                    for (var j = 0; j < controls2X.Length; j++)
                    {
                        controlsReferencesX.AddReplace(control, controls2X[j]);
                        controlsReferencedX.AddReplace(controls2X[j], control);
                    }
                }

                if (remarks.Contains("pinned-to-parent-x"))
                    pinnedToParentControlsX.AddUnique(control);


                if (controls2Y != null)
                {
                    for (var j = 0; j < controls2Y.Length; j++)
                    {
                        controlsReferencesY.AddReplace(control, controls2Y[j]);
                        controlsReferencedY.AddReplace(controls2Y[j], control);
                    }
                }

                if (remarks.Contains("pinned-to-parent-y"))
                    pinnedToParentControlsY.AddUnique(control);


                if (remarks.Contains("non-defaultable"))
                    nonDefaultingButtons.AddUnique(control as Button);
            }
        }

        internal void addAllChildrenControls(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                allControls.Add(control);
                addAllChildrenControls(control);
            }
        }

        internal void skinMoveScaleAllControls()
        {
            //Opened form cached colors (for button repainting)
            controlHighlightForeColor = ControlHighlightForeColor;
            controlHighlightBackColor = ControlHighlightBackColor;
            buttonForeColor = ButtonForeColor;
            buttonBackColor = ButtonBackColor;
            buttonBorderColor = ButtonBorderColor;
            narrowScrollBarBackColor = NarrowScrollBarBackColor;
            buttonDisabledForeColor = ButtonDisabledForeColor;
            buttonDisabledBackColor = ButtonDisabledBackColor;


            if (useSkinColors)
            {
                BackColor = FormBackColor;
                ForeColor = FormForeColor;
            }


            fillControlsReferencesRemarks();


            for (var i = allControls.Count - 1; i >= 0; i--)
                preMoveScaleControl(allControls[i]);

            for (var i = allControls.Count - 1; i >= 0; i--)
                moveScaleControlDependentReferringControlsX(allControls[i]);

            for (var i = allControls.Count - 1; i >= 0; i--)
                moveScaleControlDependentReferringControlsY(allControls[i]);


            scaleMoveLeftRightAnchoredControls();
            scaleMoveTopBottomAnchoredControls();


            for (var i = allControls.Count - 1; i >= 0; i--)
                skinControl(allControls[i]);

            if (useSkinColors)
            {
                KeyPress += CustomComboBox_KeyPress;
                KeyDown += CustomComboBox_KeyDown;
            }
        }

        protected void setInitialFormMaximumMinimumSize(Size initialMinimumSize, Size initialMaximumSize, bool sameMinMaxWidth, bool sameMinMaxHeight)
        {
            if (sameMinMaxWidth && sameMinMaxHeight)
                fixedSize = true;


            hDpiFormScaling = (float)MinimumSize.Width / initialMinimumSize.Width;
            vDpiFormScaling = (float)MinimumSize.Height / initialMinimumSize.Height;

            if (!fixedSize)
            {
                var maxWidth = 0;
                if (sameMinMaxWidth)
                    maxWidth = MinimumSize.Width;
                else if (initialMaximumSize.Width != 0)
                    maxWidth = (int)Math.Round(initialMaximumSize.Width * hDpiFormScaling);

                var maxHeight = 0;
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

        [Flags]
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

        internal static FontEquality CompareFonts(Font font1, Font font2)
        {
            if (font1 == null || font2 == null)
                return FontEquality.DifferentStylesSizes | FontEquality.DifferentNames;


            FontEquality fontNameEquality;

            var lcFontName = font1.Name.ToLower();
            var isSymbolFont = lcFontName.Contains("icons") || lcFontName.Contains("mdl2") || lcFontName.Contains("symbol");

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
            else if (isSymbolFont && Math.Abs(font1.Size - font2.Size) < 0.5)
            {
                return FontEquality.EqualSizes | FontEquality.SymbolStyles | fontNameEquality;
            }
            else if (isSymbolFont)
            {
                return FontEquality.SymbolStyles | fontNameEquality;
            }
            else if (font1.Style == font2.Style && Math.Abs(font1.Size - font2.Size) < 0.5 && font1.GdiVerticalFont == font2.GdiVerticalFont)
            {
                return FontEquality.EqualSizesStyles | fontNameEquality;
            }
            else if (Math.Abs(font1.Size - font2.Size) < 0.5 && font1.GdiVerticalFont == font2.GdiVerticalFont)
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
            var ownFontControls = new List<Control>();

            for (var i = 0; i < allControls.Count; i++) //Required for correct DPI scaling
            {
                var control = allControls[i];

                var fontEquality = CompareFonts(control.Font, Font);
                var sameFonts = (fontEquality == FontEquality.Equal);

                if (!sameFonts || control is TextBox || control is CustomTextBox)
                    ownFontControls.Add(control);

                if (control.GetType().IsSubclassOf(typeof(ContainerControl)))
                    (control as ContainerControl).AutoScaleMode = AutoScaleMode.Inherit;

                if (control is SplitContainer sc) //Let's remember initial properties to scale them manually later
                {
                    SplitContainerScalingAttributes scsa = default;
                    scsa.splitContainer = sc;
                    scsa.panel1MinSize = sc.Panel1MinSize;
                    scsa.panel2MinSize = sc.Panel2MinSize;
                    scsa.splitterDistance = sc.SplitterDistance;

                    splitContainersScalingAttributes.Add(scsa);
                }
            }


            var sameMinMaxWidth = false;
            var sameMinMaxHeight = false;

            if ((Tag as string)?.Contains("@min-max-width-same") == true)
                sameMinMaxWidth = true;

            if ((Tag as string)?.Contains("@min-max-height-same") == true)
                sameMinMaxHeight = true;

            var resources = new ResourceManager(GetType());

            var resource = resources.GetObject("$this.MaximumSize");
            var initialMaximumSize = resource == null ? new Size(0, 0) : (Size)resource;
            resource = resources.GetObject("$this.MinimumSize");
            var initialMinimumSize = resource == null ? new Size(0, 0) : (Size)resource;
            resource = resources.GetObject("$this.ClientSize");
            var initialClientSize = resource == null ? new Size(0, 0) : (Size)resource;
            resource = resources.GetObject("$this.Font");
            var initialFormFont = resource == null ? Font : resource as Font;


            MaximumSize = new Size(0, 0); //Let's temporary remove max. size restrictions


            var mbThisFormFontInitialEquality = CompareFonts(Font, initialFormFont);

            if (useMusicBeeFont || useCustomFont || mbThisFormFontInitialEquality != FontEquality.Equal)
            {
                if (useMusicBeeFont)
                {
                    workingFont = MbApiInterface.Setting_GetDefaultFont();
                }
                else if (useCustomFont)
                {
                    workingFont = new Font(
                        SavedSettings.pluginFontFamilyName,
                        SavedSettings.pluginFontSize,
                        SavedSettings.pluginFontStyle
                        );
                }
                else
                {
                    workingFont = Font;
                }

                var workingFontThisFormFontEquality = CompareFonts(workingFont, initialFormFont);


                for (var i = allControls.Count - 1; i >= 0; i--) //Required for correct DPI scaling
                    allControls[i].SuspendLayout();

                SuspendLayout();


                if (workingFontThisFormFontEquality == FontEquality.DifferentFontUnits)
                {
                    MessageBox.Show(MbForm, MsgUnsupportedMusicBeeFontType, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);

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


                for (var i = 0; i < allControls.Count; i++) //Required for correct DPI scaling
                {
                    allControls[i].ResumeLayout(false);
                    allControls[i].PerformLayout();
                }


                ResumeLayout(false);
                PerformLayout();


                initialFormFont.Dispose();
                if (!useMusicBeeFont)
                    workingFont.Dispose();

                workingFont = null;
            }

            MinimumSize = Size;

            hDpiFontScaling = (float)ClientSize.Width / initialClientSize.Width;
            vDpiFontScaling = (float)ClientSize.Height / initialClientSize.Height;


            //Split containers must be scaled manually in the child form's "OnLoad" handler using "_splitContainersScalingAttributes" (auto-scaling is improper)

            setInitialFormMaximumMinimumSize(initialMinimumSize, initialMaximumSize, sameMinMaxWidth, sameMinMaxHeight);
        }

        protected void setFormMaximizedBounds()
        {
            if (modal && MaximumSize.Height != 0 && !fixedSize)
            {
                var maximizedHeight = MaximumSize.Height;

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

                taskStarted = taskStartedMethod;

                TagToolsPlugin.fillTagNames();
            }

            //DPI/font scaling & loading plugin windows sizes/positions
            if (DeviceDpi != 96)
                dpiScaling = DeviceDpi / 96f;

            scaledPx = (int)Math.Round(dpiScaling);

            useMusicBeeFont = SavedSettings.useMusicBeeFont;
            useCustomFont = SavedSettings.useCustomFont;

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
                var screenWidth = Screen.FromControl(this).WorkingArea.Width;
                var screenHeight = Screen.FromControl(this).WorkingArea.Height;

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
            if (PluginClosing)
                return;

            while (MbForm.Disposing || !MbForm.IsHandleCreated)
                Thread.Sleep(ActionRetryDelay);

            if (MbForm.IsDisposed)
                MbForm = Control.FromHandle(MbApiInterface.MB_GetWindowHandle()) as Form;

            if (modal)
                base.ShowDialog(MbForm);
            else
                base.Show(MbForm);
        }

        internal new void Show()
        {
            modal = false;
            initAndShow();
        }

        internal new void ShowDialog()
        {
            modal = true;
            initAndShow();
        }

        internal static void Display(PluginWindowTemplate newForm, bool modalForm = false)
        {
            lock (OpenedForms)
            {
                foreach (var form in OpenedForms)
                {
                    if (form.GetType() == newForm.GetType())
                    {
                        if (form != newForm)
                            newForm.Close();

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
                    AddMenuItem(OpenedFormsSubmenu, ShowHiddenWindowsName, null, TagToolsPlugin.showHiddenEventHandler);
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

        protected virtual void childClassFormShown()
        {
            //Must be implemented in child classes. It is called before main processing of PluginWindowTemplate_Shown().
        }

        protected virtual void OnMaximizing()
        {
            //...
        }

        protected virtual void OnMaximized()
        {
            //...
        }

        protected virtual void OnRestoring()
        {
            //...
        }

        protected virtual void OnRestored()
        {
            //...
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MAXIMIZE = 0xF030;
            const int SC_MINIMIZE = 0xF020;
            const int SC_RESTORE = 0xF120;

            // Call before - don't use when "call after" is used
            // depending on the needs may be called before, after or even never (see below)
            // base.WndProc(ref m);

            if (m.Msg == WM_SYSCOMMAND)
            {
                /// <see cref="https://learn.microsoft.com/en-us/windows/win32/menurc/wm-syscommand"/>
                /// Quote:
                /// In WM_SYSCOMMAND messages, the four low - order bits of the wParam parameter 
                /// are used internally by the system.To obtain the correct result when testing 
                /// the value of wParam, an application must combine the value 0xFFF0 with the 
                /// wParam value by using the bitwise AND operator.
                int wParam = (m.WParam.ToInt32() & 0xFFF0);

                if (wParam == SC_MAXIMIZE)
                    OnMaximizing();
                else if (wParam == SC_MINIMIZE)
                    ; //Nothing for now...
                else if (wParam == SC_RESTORE)
                    OnRestoring();
            }

            // Call after - don't use when "call before" is used
            base.WndProc(ref m);
        }
        private void PluginWindowTemplate_Shown(object sender, EventArgs e)
        {
            for (var i = allControls.Count - 1; i >= 0; i--)
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

            childClassFormShown();

            if (SavedSettings.not1stTimeUsage)
                return;

            if (GetType() == typeof(Settings))
                return;

            if (GetType() == typeof(QuickSettings))
                return;

            SavedSettings.not1stTimeUsage = true;
            var result = MessageBox.Show(MbForm, Msg1stTimeUsage, string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                var tagToolsForm = new Settings(TagToolsPlugin);
                Display(tagToolsForm, true);
            }
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

                    OnMaximized();
                }
                else if (Left + Width >= 0 && Top + Height >= 0)
                {
                    left = Left;
                    top = Top;
                    width = Width;
                    height = Height;

                    OnRestored();
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
            if (backgroundTaskIsScheduled && backgroundTaskIsUpdatingTags && closeButton.IsEnabled())
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
                    if (item is ToolStripMenuItem menuItem)
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
            }
        }

        protected WindowSettingsType findCreateSavedWindowSettings(bool createIfAbsent)
        {
            var fullName = GetType().FullName;
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
            var column1Width = 0;
            var column2Width = 0;
            var column3Width = 0;
            var splitterDistance = 0;
            var table2column1Width = 0;
            var table2column2Width = 0;
            var table2column3Width = 0;

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


                var width2 = (int)Math.Round(width / hDpiFormScaling);
                var height2 = (int)Math.Round(height / vDpiFormScaling);

                if (width2 == 0)
                    width2 = currentWindowSettings.w; //-V3080
                else if (100d * Math.Abs(width2 - currentWindowSettings.w) / width2 < 0.5d) //0.5%
                    width2 = currentWindowSettings.w; //-V3080

                if (height2 == 0)
                    height2 = currentWindowSettings.h;
                else if (100d * Math.Abs(height2 - currentWindowSettings.h) / height2 < 0.5d) //0.5%
                    height2 = currentWindowSettings.h;


                var left2 = (int)Math.Round(left / hDpiFormScaling);
                if (Math.Abs(left2 - currentWindowSettings.x) <= 1) //1px
                    left2 = currentWindowSettings.x;

                var top2 = (int)Math.Round(top / vDpiFormScaling);
                if (Math.Abs(top2 - currentWindowSettings.y) <= 1) //1px
                    top2 = currentWindowSettings.y;



                currentWindowSettings.x = left2;
                currentWindowSettings.y = top2;
                currentWindowSettings.w = width2;
                currentWindowSettings.h = height2;

                if (!Visible || WindowState == FormWindowState.Minimized)
                    currentWindowSettings.max = (windowState == FormWindowState.Maximized);
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
                var column1Width2 = (int)Math.Round(column1Width / hDpiFontScaling);
                if (100f * Math.Abs(column1Width2 - currentWindowSettings.column1Width) / column1Width2 < 0.5f) //-V3080
                    column1Width2 = currentWindowSettings.column1Width;

                currentWindowSettings.column1Width = column1Width2;


                var column2Width2 = (int)Math.Round(column2Width / hDpiFontScaling);
                if (100f * Math.Abs(column2Width2 - currentWindowSettings.column2Width) / column2Width2 < 0.5f)
                    column2Width2 = currentWindowSettings.column2Width;

                currentWindowSettings.column2Width = column2Width2;


                var column3Width2 = (int)Math.Round(column3Width / hDpiFontScaling);
                if (100f * Math.Abs(column3Width2 - currentWindowSettings.column3Width) / column3Width2 < 0.5f)
                    column3Width2 = currentWindowSettings.column3Width;

                currentWindowSettings.column3Width = column3Width2;
            }

            if (table2column1Width != 0)
            {
                var table2column1Width2 = (int)Math.Round(table2column1Width / hDpiFontScaling);
                if (100f * Math.Abs(table2column1Width2 - currentWindowSettings.table2column1Width) / table2column1Width2 < 0.5f)
                    table2column1Width2 = currentWindowSettings.table2column1Width;

                currentWindowSettings.table2column1Width = table2column1Width2;


                var table2column2Width2 = (int)Math.Round(table2column2Width / hDpiFontScaling);
                if (100f * Math.Abs(table2column2Width2 - currentWindowSettings.table2column2Width) / table2column2Width2 < 0.5f)
                    table2column2Width2 = currentWindowSettings.table2column2Width;

                currentWindowSettings.table2column2Width = table2column2Width2;


                var table2column3Width2 = (int)Math.Round(table2column3Width / hDpiFontScaling);
                if (100f * Math.Abs(table2column3Width2 - currentWindowSettings.table2column3Width) / table2column3Width2 < 0.5f)
                    table2column3Width2 = currentWindowSettings.table2column3Width;

                currentWindowSettings.table2column3Width = table2column3Width2;
            }

            if (splitterDistance != 0)
            {
                var splitterDistance2 = (int)Math.Round(splitterDistance / vDpiFontScaling);
                if (100f * Math.Abs(splitterDistance2 - currentWindowSettings.splitterDistance) / splitterDistance2 < 0.5f)
                    splitterDistance2 = currentWindowSettings.splitterDistance;

                currentWindowSettings.splitterDistance = splitterDistance2;
            }
        }

        private void serializedOperation()
        {
            var taskWasStarted = false;

            InitializeSbText();

            try
            {
                if (backgroundTaskIsStopping || backgroundTaskIsStoppedOrCancelled || !backgroundTaskIsScheduled) //Let's skip this queued operation
                {
                    if (DisablePlaySoundOnce)
                        DisablePlaySoundOnce = false;
                    else if (SavedSettings.playCanceledSound)
                        System.Media.SystemSounds.Hand.Play();
                }
                else //Let's start operation
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
                backgroundTaskIsStopping = true;
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
                    backgroundTaskIsStopping = false;

                    if (backgroundTaskIsUpdatingTags && taskWasStarted)
                        NumberOfNativeMbBackgroundTasks--;
                }

                if (!closeFormOnStopping && !Disposing && !IsDisposed && IsHandleCreated)
                {
                    RefreshPanels(true);
                    SetResultingSbText();

                    try
                    {
                        if (clickedButton != EmptyButton && !isStopButtonAlreadyClicked)
                            Invoke(new Action(() => { stopButtonClickedMethod(null); }));

                        if (!Visible && !isStopButtonAlreadyClicked)
                        {
                            if (SavedSettings.closeShowHiddenWindows == 1)
                                Invoke(new Action(() => { Close(); }));
                            else
                                Invoke(new Action(() => { Visible = true; }));
                        }
                    }
                    catch (InvalidOperationException) //-V3163 //-V5606
                    {
                        ; //Still disposing. Let's ignore...
                    }
                }
            }
        }

        internal void switchOperation(ThreadStart operation, Button clickedButton, Button okButton, Button previewButton, Button closeButton,
            bool backgroundTaskIsUpdatingTags, PrepareOperation prepareOperation)
        {
            buttonLabels.TryGetValue(previewButton, out buttonPreviewName);
            buttonLabels.TryGetValue(okButton, out buttonOKName);
            buttonLabels.TryGetValue(closeButton, out buttonCloseName);

            this.clickedButton = clickedButton;
            this.previewButton = previewButton;
            this.closeButton = closeButton;


            if (backgroundTaskIsScheduled && backgroundTaskIsStopping) //Let's restart operation
            {
                isStopButtonAlreadyClicked = false;
                backgroundTaskIsStopping = false;
                backgroundTaskIsStoppedOrCancelled = false;

                lock (OpenedForms)
                {
                    if (this.backgroundTaskIsUpdatingTags)
                        NumberOfNativeMbBackgroundTasks++;
                }

                queryingOrUpdatingButtonClick();
            }
            else if (backgroundTaskIsScheduled && !backgroundTaskIsStopping) //Let's stop operation
            {
                backgroundTaskIsStopping = true;

                lock (OpenedForms)
                {
                    if (this.backgroundTaskIsUpdatingTags)
                        NumberOfNativeMbBackgroundTasks--;
                }

                stopButtonClickedMethod(prepareOperation);
            }
            else //if (!backgroundTaskIsScheduled) //Let's start operation
            {
                this.backgroundTaskIsUpdatingTags = backgroundTaskIsUpdatingTags;

                isStopButtonAlreadyClicked = false;
                backgroundTaskIsStopping = false;
                backgroundTaskIsStoppedOrCancelled = false;
                backgroundTaskIsScheduled = true;
                backgroundThread = null;

                job = operation;

                if (this.backgroundTaskIsUpdatingTags)
                {
                    queryingOrUpdatingButtonClick();

                    lock (OpenedForms)
                        NumberOfNativeMbBackgroundTasks++;

                    MbApiInterface.MB_CreateBackgroundTask(serializedOperation, this);
                }
                else
                {
                    queryingOrUpdatingButtonClick();

                    var workingThread = new Thread(serializedOperation);
                    workingThread.Start();
                }

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

        internal virtual void enableDisablePreviewOptionControls(bool enable, bool dontChangeDisabled = false)
        {
            //Implemented in derived classes... 
        }

        internal virtual void enableQueryingButtons()
        {
            //Implemented in derived classes... 
        }

        internal virtual void disableQueryingButtons()
        {
            //Implemented in derived classes... 
        }

        internal virtual void enableQueryingOrUpdatingButtons()
        {
            //Implemented in derived classes... 
        }

        internal virtual void disableQueryingOrUpdatingButtons()
        {
            //Implemented in derived classes... 
        }

        internal void queryingOrUpdatingButtonClick()
        {
            lock (OpenedForms)
            {
                foreach (var form in OpenedForms)
                {
                    if (backgroundTaskIsUpdatingTags && form != this && !(form.backgroundTaskIsUpdatingTags && form.backgroundTaskIsScheduled && !form.backgroundTaskIsStoppedOrCancelled))
                        form.disableQueryingButtons();
                }

                if (backgroundTaskIsUpdatingTags) //Updating operation
                {
                    enableQueryingButtons();

                    if (clickedButton != EmptyButton)
                    {
                        buttonOKName = buttonLabels[clickedButton];
                        clickedButton.Text = ButtonCancelName;
                        clickedButton.Enable(true);

                        if (buttonLabels.TryGetValue(closeButton, out buttonCloseName))
                        {
                            closeButton.Text = ButtonHideName;
                            closeButton.Enable(true);
                        }
                    }
                }
                else //Querying operation
                {
                    clickedButton.Text = ButtonStopName;
                    clickedButton.Enable(true);
                }

                enableDisablePreviewOptionControls(false);
                enableQueryingOrUpdatingButtons();

                if (backgroundTaskIsUpdatingTags)
                    previewButton.Enable(false);
            }
        }

        internal void nonInteractiveUpdatingTaskStopped()
        {

            lock (OpenedForms)
            {
                foreach (var form in OpenedForms)
                {
                    if (backgroundTaskIsUpdatingTags && form != this) //Updating operation
                    {
                        if (NumberOfNativeMbBackgroundTasks > 0 && !(form.backgroundTaskIsUpdatingTags && form.backgroundTaskIsScheduled && !form.backgroundTaskIsStoppedOrCancelled))
                            form.disableQueryingButtons();
                        else
                            form.enableQueryingButtons();
                    }
                }

                if (backgroundTaskIsUpdatingTags) //Updating operation
                {
                    backgroundTaskIsStopping = true;
                    backgroundTaskIsStoppedOrCancelled = false;
                }
                else
                {
                    throw new Exception("Something went wrong! Non-interactive preview operation!");
                }
            }
        }

        internal void stopButtonClickedMethod(PrepareOperation prepareOperation)
        {
            if (isStopButtonAlreadyClicked)
                return;

            lock (OpenedForms)
            {
                foreach (var form in OpenedForms)
                {
                    if (backgroundTaskIsUpdatingTags && form != this) //Updating operation
                    {
                        if (NumberOfNativeMbBackgroundTasks > 0 && !(form.backgroundTaskIsUpdatingTags && form.backgroundTaskIsScheduled && !form.backgroundTaskIsStoppedOrCancelled))
                            form.disableQueryingButtons();
                        else
                            form.enableQueryingButtons();
                    }
                }

                disableQueryingButtons();

                if (backgroundTaskIsUpdatingTags) //Updating operation
                {
                    clickedButton.Text = buttonOKName;

                    if (previewIsGenerated)
                        previewButton.Text = ButtonClearName;
                    else
                        previewButton.Text = buttonPreviewName;

                    if (buttonLabels.ContainsKey(closeButton))
                        closeButton.Text = buttonCloseName;

                    enableQueryingOrUpdatingButtons();

                    backgroundTaskIsStopping = true;
                    backgroundTaskIsStoppedOrCancelled = false;
                }
                else //Querying operation
                {
                    if (backgroundTaskIsScheduled)
                        disableQueryingOrUpdatingButtons();
                    else
                        enableQueryingOrUpdatingButtons();

                    if (prepareOperation != null && backgroundTaskIsStopping)
                    {
                        prepareOperation();
                        previewIsGenerated = false;
                        backgroundTaskIsStopping = false;
                        backgroundTaskIsStoppedOrCancelled = false;
                        clickedButton.Text = buttonPreviewName;
                    }
                    else
                    {
                        if (previewIsGenerated)
                            clickedButton.Text = ButtonClearName;
                        else
                            clickedButton.Text = buttonPreviewName;

                        backgroundTaskIsStopping = false;
                        backgroundTaskIsStoppedOrCancelled = false;

                        closeButton.Enable(true);
                    }
                }
            }
        }

        private void taskStartedMethod()
        {
            lock (OpenedForms)
            {
                if (backgroundTaskIsUpdatingTags) //Updating operation
                {
                    enableQueryingButtons();
                    clickedButton.Text = ButtonStopName;
                    previewButton.Enable(false);
                }
            }
        }

        protected bool checkStoppingStatus()
        {
            if (PluginClosing)
                return true;
            else if (backgroundTaskIsScheduled && (backgroundTaskIsStopping || backgroundTaskIsStoppedOrCancelled))
                return true;
            else
                return false;
        }

        protected bool checkStoppedStatus()
        {
            if (backgroundTaskIsScheduled && backgroundTaskIsStoppedOrCancelled)
            {
                enableDisablePreviewOptionControls(true);
                enableQueryingOrUpdatingButtons();

                if (backgroundTaskIsUpdatingTags && backgroundTaskIsScheduled)
                    previewButton.Enable(false);

                return true;
            }

            if (!backgroundTaskIsScheduled)
            {
                enableDisablePreviewOptionControls(true);
                enableQueryingOrUpdatingButtons();

                return true;
            }

            return false;
        }

        //Returns: true if preview is started (closing current form must be temporary disabled), otherwise returns false
        protected bool clickOnPreviewButton(DataGridView previewTable, PrepareOperation prepareOperation, ThreadStart operation,
            Button previewButton, Button okButton, Button closeButton, bool clearPreview = false)
        {
            if (!previewIsGenerated && !backgroundTaskIsWorking() && !clearPreview)
            {
                previewIsGenerated = false;

                if (prepareOperation != null)
                {
                    if (prepareOperation())
                    {
                        switchOperation(operation, previewButton, okButton, previewButton, closeButton, false, prepareOperation);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    switchOperation(operation, previewButton, okButton, previewButton, closeButton, false, null);
                    return true;
                }
            }
            else if (backgroundTaskIsWorking())
            {
                backgroundTaskIsStopping = true;
                return true;
            }
            else
            {
                if (prepareOperation != null)
                    prepareOperation();

                previewIsGenerated = false;
                backgroundTaskIsStopping = false;
                backgroundTaskIsStoppedOrCancelled = false;
                previewButton.Text = ButtonPreviewName;
                enableDisablePreviewOptionControls(true);
                enableQueryingOrUpdatingButtons();

                return false;
            }
        }
    }
}
