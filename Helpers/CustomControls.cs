using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using ExtensionMethods;

using static NativeMethods;

namespace MusicBeePlugin
{
    partial class Plugin
    {
        internal const int ThumbMoveRepeatInterval = 200; //Milliseconds //---
        internal const int DontCreateVScrollBar = -2;
    }

    internal static class ControlsTools
    {
        internal static T FindControlChild<T>(Control container) where T : Control
        {
            foreach (Control child in container.Controls)
            {
                if (child is T TChild)
                    return TChild;
            }

            //Not found.
            return null;
        }

        internal static List<T> FindControlAllChildren<T>(Control container) where T : Control
        {
            var children = new List<T>();

            foreach (Control child in container.Controls)
            {
                if (child is T TChild)
                    children.Add(TChild);
            }

            return children;
        }

        internal static void DrawCheckBox(Graphics graphics, float dpiScaling, Rectangle checkBoxBounds, int borderWidth, Color backColor, Color borderColor, Color checkMarkColor, bool state)
        {
            Point pt = new Point(checkBoxBounds.Left + (int)Math.Round(2 * dpiScaling), checkBoxBounds.Top + (int)Math.Round(4 * dpiScaling));

            int inflateOffset = (int)Math.Round(4 * dpiScaling);

            checkBoxBounds.Inflate(-inflateOffset, -inflateOffset);
            checkBoxBounds.Offset(0, -(int)Math.Round(dpiScaling));

            Rectangle checkMarkBounds = new Rectangle(checkBoxBounds.Left + (int)Math.Round(dpiScaling), checkBoxBounds.Top - (int)Math.Round(2 * dpiScaling),
                checkBoxBounds.Width + (int)Math.Round(4 * dpiScaling), checkBoxBounds.Height + (int)Math.Round(4 * dpiScaling));
            checkMarkBounds.Inflate(-borderWidth - (int)Math.Round(dpiScaling), -borderWidth - (int)Math.Round(dpiScaling));
            checkMarkBounds.Offset((int)Math.Round(-3 * dpiScaling), (int)Math.Round(dpiScaling));

            if (backColor != Plugin.NoColor)
                graphics.FillRectangle(new SolidBrush(backColor), checkBoxBounds);

            if (state)
            {
                string marks = "";
                string mark = marks[4].ToString();

                using (Font wing = new Font("Segoe Fluent Icons", (int)Math.Round(checkBoxBounds.Height * 0.9f), FontStyle.Regular, GraphicsUnit.Pixel))
                    graphics.DrawString(mark, wing, new SolidBrush(checkMarkColor), checkMarkBounds);
            }

            graphics.DrawRectangle(new Pen(borderColor, borderWidth), checkBoxBounds);
        }

        internal static void DrawCheckBox(Graphics graphics, float dpiScaling, Rectangle checkBoxBounds, bool state)
        {
            var p = new Point();
            var s = CheckBoxRenderer.GetGlyphSize(graphics,
            System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);

            p.X = checkBoxBounds.Location.X + (checkBoxBounds.Width / 2) - (s.Width / 2) - 1;
            p.Y = checkBoxBounds.Location.Y + (checkBoxBounds.Height / 2) - (s.Height / 2) - 1;

            Point checkBoxLocation = p;
            System.Windows.Forms.VisualStyles.CheckBoxState cbState = System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;

            if (state)
                cbState = System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal;

            CheckBoxRenderer.DrawCheckBox(graphics, checkBoxLocation, cbState);
        }

        internal static int GetCustomScrollBarInitialWidth(float dpiScaling, int sbBorderWidth)
        {
            return (int)Math.Round(dpiScaling * Plugin.ScrollBarWidth) + 2 * (int)Math.Round(dpiScaling * sbBorderWidth);
        }

        internal static TextBox CreateMusicBeeTextBox()
        {
            return (TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
        }

        internal static void BorderedControl_Resize(object sender, EventArgs e)
        {
            if (sender is Control control)
            {
                if (control.Parent != null) //Not disposed
                {
                    control.Parent.Size = control.Size;

                    var controlRectangle = new Rectangle(1, 1, control.Width - 2, control.Height - 2);
                    var controlRegion = new Region(controlRectangle);
                    control.Region = controlRegion;

                    control.Refresh();
                }
            }
        }

        internal static void BorderedControl_LocationChanged(object sender, EventArgs e)
        {
            var control = sender as Control;
            var controlBorder = control.Parent as ControlBorder;

            if (!controlBorder.getIgnoreControl_LocationChanged())
                controlBorder.changeControlBorderLocation();
        }

        internal static void DrawBorder(Control control, Color color, Color focusedColor, Color disabledColor)
        {
            using (var graphics = control.CreateGraphics())
            {
                if (!control.IsEnabled())
                    ControlPaint.DrawBorder(graphics, new Rectangle(0, 0, control.Width, control.Height), disabledColor,
                        ButtonBorderStyle.Solid);
                else if (control.ContainsFocus)
                    ControlPaint.DrawBorder(graphics, new Rectangle(0, 0, control.Width, control.Height), focusedColor,
                        ButtonBorderStyle.Solid);
                else
                    ControlPaint.DrawBorder(graphics, new Rectangle(0, 0, control.Width, control.Height), color,
                        ButtonBorderStyle.Solid);
            }
        }

        internal static void DrawBorder(Control control)
        {
            DrawBorder(control, Plugin.ButtonBorderColor, Plugin.ButtonBorderColor, Plugin.ButtonBorderColor);
        }
    }

    internal sealed class ControlBorder : UserControl
    {
        private readonly Color borderColorDisabled = Plugin.ScrollBarBorderColor;
        private readonly Color borderColorActive = Plugin.ScrollBarFocusedBorderColor;
        private readonly Color borderColor = Plugin.ScrollBarBorderColor;

        internal readonly Control control;
        private bool ignoreControl_LocationChanged = false;

        internal bool ignoreControl_VisibleChanged = false;

        internal ControlBorder(Control control, Color borderColor, Color borderColorDisabled, Color borderColorActive) :
            this(control)
        {
            this.borderColorDisabled = borderColorDisabled;
            this.borderColorActive = borderColorActive;
            this.borderColor = borderColor;
        }

        internal ControlBorder(Control control, PluginWindowTemplate ownerForm = null)
        {
            if (Plugin.SavedSettings.dontUseSkinColors)
                return;


            var form = ownerForm;

            if (form == null)
                form = control.FindForm() as PluginWindowTemplate;

            form.controlBorders.AddUnique(this); //-V3149

            this.control = control;

            Name = control.Name;
            Tag = control.Tag;
            control.Tag = null;

            Location = control.Location;
            Size = control.Size;
            Anchor = control.Anchor;
            Dock = control.Dock;
            Margin = control.Margin;
            Padding = Padding.Empty;

            TabIndex = control.TabIndex;
            TabStop = control.TabStop;

            var parent = control.Parent;
            var index = 0;
            TableLayoutPanelCellPosition cellPosition = default;

            if (parent != null)
            {
                index = parent.Controls.IndexOf(control);

                if (parent is TableLayoutPanel panel)
                    cellPosition = panel.GetCellPosition(control);

                parent.Controls.RemoveAt(index);
            }

            Controls.Add(control);


            control.Margin = Padding.Empty;
            control.Left = 0;
            control.Top = 0;
            control.Dock = DockStyle.None;
            control.TabStop = false;
            control.LocationChanged += ControlsTools.BorderedControl_LocationChanged;
            control.SizeChanged += ControlsTools.BorderedControl_Resize;

            if (parent != null)
            {
                parent.Controls.Add(this);
                parent.Controls.SetChildIndex(this, index);
                if (parent is TableLayoutPanel layoutPanel)
                    layoutPanel.SetCellPosition(this, cellPosition);
            }


            if (control is TextBox textBox && textBox.Multiline || control.Parent is CustomComboBox)
                control.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            else
                control.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;


            Paint += controlBorder_Paint;
            GotFocus += (sender, args) => { control.Focus(); };

            ResizeRedraw = true;
        }

        internal bool getIgnoreControl_LocationChanged()
        {
            return ignoreControl_LocationChanged;
        }

        internal void changeControlBorderLocation()
        {
            if (control.Left != 0)
                Left = control.Left;

            if (control.Top != 0)
                Top = control.Top;

            ignoreControl_LocationChanged = true;
            control.Left = 0;
            control.Top = 0;
            ignoreControl_LocationChanged = false;
        }

        private void controlBorder_Paint(object sender, PaintEventArgs e)
        {
            ControlsTools.DrawBorder(sender as Control, borderColor, borderColorActive, borderColorDisabled);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                control?.Dispose();
            }
        }

        ~ControlBorder()
        {
            Dispose(false);
        }
    }

    internal class CustomComboBox : UserControl
    {
        private readonly Color borderColorDisabled = Plugin.ScrollBarBorderColor;
        private readonly Color borderColorActive = Plugin.ScrollBarFocusedBorderColor;
        private readonly Color borderColor = Plugin.ScrollBarBorderColor;

        private Bitmap downArrowComboBoxImage;
        private Bitmap disabledDownArrowComboBoxImage;

        private int scaledPx = 1;
        private string cue = string.Empty;

        internal readonly PluginWindowTemplate ownerForm;

        internal readonly ComboBox comboBox;

        private TextBox textBox;
        private Button button;
        private CustomListBox listBox;

        private bool itemsChangedProcessed = false;
        private bool setItemsChangedProcessed = false;

        public bool recordMode;

        private CustomListBox specialStateListBox = null;
        private string defaultSpecialState = null;
        private int specialStateCharCount;

        private int lastSelectedIndex = -1;

        private bool textBoxReadOnly;

        private bool ignoreEnabledChanged = false;
        private bool enabled = true;
        private bool droppedDown = false;

        private TableLayoutPanel dropDownPanel;
        private ToolStripDropDown dropDown;
        private bool enableFiltering;

        private int initialDropDownWidth = 250;
        private int initialDropDownHeight = 100;


        private static readonly object EVENT_SELECTEDINDEXCHANGED = new object();

        internal event EventHandler SelectedIndexChanged
        {
            add
            {
                Events.AddHandler(EVENT_SELECTEDINDEXCHANGED, value);
            }
            remove
            {
                Events.RemoveHandler(EVENT_SELECTEDINDEXCHANGED, value);
            }
        }


        private static readonly object EVENT_SELECTEDITEMCHANGED = new object();

        internal event EventHandler SelectedItemChanged
        {
            add
            {
                Events.AddHandler(EVENT_SELECTEDINDEXCHANGED, value);
            }
            remove
            {
                Events.RemoveHandler(EVENT_SELECTEDINDEXCHANGED, value);
            }
        }


        private static readonly object EVENT_DROPDOWNCLOSED = new object();

        internal event EventHandler DropDownClosed
        {
            add
            {
                Events.AddHandler(EVENT_DROPDOWNCLOSED, value);
            }
            remove
            {
                Events.RemoveHandler(EVENT_DROPDOWNCLOSED, value);
            }
        }


        private TableLayoutPanel getDropDown(int customScrollBarInitialWidth)
        {
            var dropDownPanel = new TableLayoutPanel();

            dropDownPanel.Font = Font;
            dropDownPanel.Margin = Padding.Empty;
            dropDownPanel.Padding = Padding.Empty;
            dropDownPanel.AutoSize = true;
            dropDownPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            dropDownPanel.TabStop = false;

            var columnStyle0 = new ColumnStyle(SizeType.Absolute, 0);
            var columnStyle1 = new ColumnStyle(SizeType.Absolute, initialDropDownWidth - customScrollBarInitialWidth);
            var columnStyle2 = new ColumnStyle(SizeType.Absolute, customScrollBarInitialWidth);

            dropDownPanel.ColumnStyles.Add(columnStyle0);
            dropDownPanel.ColumnStyles.Add(columnStyle1);
            dropDownPanel.ColumnStyles.Add(columnStyle2);

            var rowStyle0 = new RowStyle(SizeType.AutoSize, 100f);
            var rowStyle1 = new RowStyle(SizeType.Absolute, 0f);

            dropDownPanel.RowStyles.Add(rowStyle0);
            dropDownPanel.RowStyles.Add(rowStyle1);


            listBox = new CustomListBox(customScrollBarInitialWidth, scaledPx, false);

            listBox.Font = Font;
            listBox.MultiColumn = false;
            listBox.IntegralHeight = false;
            listBox.Margin = Padding.Empty;
            listBox.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            listBox.TabStop = false;
            listBox.Tag = this;

            listBox.SelectedIndex = -1;
            lastSelectedIndex = -1;

            dropDownPanel.Controls.Add(listBox, 1, 0);
            ownerForm.skinControl(listBox, Plugin.SbBorderWidth);

            listBox.SetBorderColors(listBox.BackColor, listBox.BackColor, listBox.BackColor);

            return dropDownPanel;
        }

        internal string[] GetSpecialStateItems()
        {
            SortedDictionary<int, string> items = new SortedDictionary<int, string>();

            if (specialStateListBox == null)
            {
                for (int i = 0; i < comboBox.Items.Count; i++)
                {
                    int specialStateIndex = GetItemSpecialStateIndex(i);
                    if (specialStateIndex != -1)
                        items.Add(specialStateIndex, GetSpecialStateItem(i));
                }
            }
            else
            {
                for (int i = 0; i < specialStateListBox.Items.Count; i++)
                {
                    int specialStateIndex = GetItemSpecialStateIndex(i);
                    if (specialStateIndex != -1)
                        items.Add(specialStateIndex, GetSpecialStateItem(i));
                }
            }

            string[] specialStateItems = new string[items.Count];
            items.Values.CopyTo(specialStateItems, 0);

            return specialStateItems;
        }

        internal bool IsDefaultSpecialStateItem(int index)
        {
            if (specialStateListBox == null && comboBox != null)
            {
                if ((comboBox.Items[index] as string).Substring(0, GetSpecialStateCharCount(true)) == GetDefaultSpecialState())
                    return true;
                else
                    return false;
            }
            else if (specialStateListBox != null)
            {
                if (specialStateListBox.Items[index] as string == GetDefaultSpecialState())
                    return true;
                else
                    return false;
            }
            else
            {
                return true;
            }
        }

        internal string GetSpecialStateItem(int index)
        {
            if (specialStateListBox == null)
                return comboBox.Items[index].ToString().Substring(GetSpecialStateCharCount(true));
            else
                return listBox.Items[index].ToString();
        }

        internal int GetItemSpecialStateIndex(int index)
        {
            string specialStateValue = null;

            if (specialStateListBox == null)
                specialStateValue = comboBox.Items[index].ToString().Substring(0, GetSpecialStateCharCount(true));
            else
                specialStateValue = specialStateListBox.Items[index].ToString();

            if (int.TryParse(specialStateValue.Trim(' '), out int specialStateIndex))
                return specialStateIndex;

            return -1;
        }

        internal bool SelectItemBySpecialStateIndex(int index)
        {
            if (GetDefaultSpecialState() == null)
            {
                System.Media.SystemSounds.Beep.Play();
                return false;
            }

            SelectedIndex = -1;
            for (int i = 0; i < Items.Count; i++)
            {
                if (GetItemSpecialStateIndex(i) == index)
                {
                    SelectedIndex = i;
                    break;
                }
            }

            if (SelectedIndex == -1)
            {
                System.Media.SystemSounds.Beep.Play();
                return false;
            }
            else
            {
                return true;
            }
        }

        internal void SetItemSpecialState(int index, int state)
        {
            string specialState;

            if (state == -1)
                specialState = GetDefaultSpecialState();
            else if (specialStateListBox == null)
                specialState = state.ToString("D" + specialStateCharCount) + ' ';
            else
                specialState = state.ToString("D" + specialStateCharCount);


            if (specialStateListBox != null)
                specialStateListBox.Items[index] = specialState;
            else
                comboBox.Items[index] = specialState + comboBox.Items[index].ToString().Substring(GetSpecialStateCharCount(true));
        }

        internal string GetItemSpecialState(int index)
        {
            if (specialStateListBox == null)
                return comboBox.Items[index].ToString().Substring(0, GetSpecialStateCharCount(false));
            else
                return specialStateListBox.Items[index].ToString();
        }

        internal string GetItemNextSpecialState(int index)
        {
            if (SpecialState >= MaxSpecialState)
            {
                return GetItemSpecialState(index);
            }
            else
            {
                SpecialState++;
                return GetCurrentSpecialState();
            }
        }

        internal string GetCurrentSpecialState()
        {
            if (specialStateListBox == null)
                return SpecialState.ToString("D" + specialStateCharCount) + ' ';
            else
                return SpecialState.ToString("D" + specialStateCharCount);
        }

        internal int GetSpecialStateCharCount(bool includeServiceSpace)
        {
            if (specialStateListBox == null || includeServiceSpace)
                return specialStateCharCount + 1;
            else
                return specialStateCharCount;
        }

        internal string GetDefaultSpecialState()
        {
            return defaultSpecialState;
        }

        internal ListControl CreateSpecialStateColumn(string defaultValue, int columnCharCount)
        {
            specialStateCharCount = defaultValue.Length;
            defaultSpecialState = string.Empty;
            for (int i = defaultValue.Length; i < columnCharCount; i++)
            {
                specialStateCharCount++;
                defaultSpecialState = ' ' + defaultSpecialState;
            }

            defaultSpecialState += defaultValue;

            if (listBox == null)
            {
                defaultSpecialState += ' ';

                for (int i = 0; i < comboBox.Items.Count; i++)
                {
                    string text = comboBox.Items[i] as string;

                    if (text.Length < GetSpecialStateCharCount(true) || text[GetSpecialStateCharCount(true) - 1] != ' ')
                        comboBox.Items[i] = defaultSpecialState + text;
                }

                comboBox.Tag = this;

                return comboBox; //For the caller to be able to set event handlers
            }
            else
            {
                var additionalColumnWidth = (TextRenderer.MeasureText(ownerForm.CreateGraphics(), " " + defaultSpecialState, this.Font)).Width;

                dropDownPanel.ColumnStyles[0].Width = additionalColumnWidth;

                specialStateListBox = new CustomListBox(0, scaledPx, false);

                specialStateListBox.Font = Font;
                specialStateListBox.MultiColumn = false;
                specialStateListBox.IntegralHeight = false;
                specialStateListBox.Margin = Padding.Empty;
                specialStateListBox.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                specialStateListBox.TabStop = false;
                specialStateListBox.Tag = this;


                specialStateListBox.MinimumSize = new Size(additionalColumnWidth, 2);
                specialStateListBox.MaximumSize = new Size(additionalColumnWidth, listBox.MaximumSize.Height);


                listBox_ItemsChanged(null, null);

                for (int i = 0; i < listBox.Items.Count; i++)
                {
                    string text = listBox.Items[i] as string;

                    if (text.Length >= GetSpecialStateCharCount(true) && text[GetSpecialStateCharCount(true) - 1] == ' ')
                    {
                        listBox.Items[i] = text.Substring(GetSpecialStateCharCount(true));
                        specialStateListBox.Items[i] = text.Substring(0, GetSpecialStateCharCount(true));
                    }
                    else
                    {
                        specialStateListBox.Items[i] = GetDefaultSpecialState();
                    }
                }


                specialStateListBox.AdjustAdditionalListHeight(initialDropDownHeight);

                specialStateListBox.SelectedIndex = listBox.SelectedIndex;

                specialStateListBox.GotFocus += changeTextBoxBackground;
                specialStateListBox.LostFocus += changeTextBoxBackground;
                specialStateListBox.MouseMove += listBox_MouseMove;
                specialStateListBox.SelectedIndexChanged += additionalListBox_SelectedIndexChanged;


                dropDownPanel.Controls.Add(specialStateListBox, 0, 0);
                ownerForm.skinControl(specialStateListBox, Plugin.DontCreateVScrollBar);

                specialStateListBox.SetBorderColors(specialStateListBox.BackColor, specialStateListBox.BackColor, specialStateListBox.BackColor);

                specialStateListBox.Tag = this;

                return specialStateListBox; //For the caller to be able to set event handlers
            }
        }

        internal int GetSpecialStateColumnWidth()
        {
            if (specialStateListBox == null)
                return 0;
            else
                return specialStateListBox.Width;
        }

        internal CustomComboBox(PluginWindowTemplate ownerForm, ComboBox comboBox, bool skinned,
            Color? borderColor = null, Color? borderColorDisabled = null, Color? borderColorActive = null)
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            //SetStyle(ControlStyles.DoubleBuffer, true);

            this.ownerForm = ownerForm;

            ToolTip toolTip = ownerForm.getFormToolTip("toolTip1");

            if (skinned)
            {
                if (borderColorDisabled != null)
                    this.borderColorDisabled = (Color)borderColorDisabled;

                if (borderColorActive != null)
                    this.borderColorActive = (Color)borderColorActive;

                if (borderColor != null)
                    this.borderColor = (Color)borderColor;

                this.comboBox = null;

                if (comboBox.AccessibleName == "filtered")
                    initSkinned(comboBox, toolTip, true);
                else
                    initSkinned(comboBox, toolTip, false);
            }
            else
            {
                this.comboBox = (ComboBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.ComboBox);
                init(comboBox, toolTip);
            }

            EnabledChanged += OnEnabledChanged;
            VisibleChanged += OnVisibleChanged;

            this.SpecialState = -1;
            this.Visible = comboBox.Visible;
            this.Enabled = comboBox.Enabled;
            this.Sorted = comboBox.Sorted;
        }

        private void init(ComboBox comboBox, ToolTip toolTip)
        {
            this.scaledPx = ownerForm.scaledPx;
            this.enableFiltering = false;

            this.textBox = null;
            this.button = null;
            this.listBox = null;
            this.dropDownPanel = null;
            this.dropDown = null;
            this.initialDropDownWidth = comboBox.DropDownWidth;

            var parent = comboBox.Parent;
            var index = parent.Controls.IndexOf(comboBox);
            TableLayoutPanelCellPosition cellPosition = default;
            var location = comboBox.Location;

            if (parent is TableLayoutPanel panel)
                cellPosition = panel.GetCellPosition(comboBox);

            this.Font = comboBox.Font;
            this.Margin = comboBox.Margin;
            this.Padding = Padding.Empty;

            this.Anchor = comboBox.Anchor;
            this.TabIndex = comboBox.TabIndex;
            this.TabStop = comboBox.TabStop;
            this.Tag = comboBox.Tag;
            this.Name = comboBox.Name;


            if (toolTip != null)
                toolTip.SetToolTip(this.comboBox, toolTip.GetToolTip(comboBox));

            this.comboBox.DropDownStyle = comboBox.DropDownStyle;

            foreach (var item in comboBox.Items)
                this.comboBox.Items.Add(item);

            this.comboBox.Font = this.Font;

            this.comboBox.Location = new Point(0, 0);
            this.comboBox.Margin = Padding.Empty;
            this.comboBox.Size = new Size(comboBox.Width, comboBox.Height);
            this.comboBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            this.Size = new Size(this.comboBox.Width, this.comboBox.Height + 1);

            this.comboBox.DropDownClosed += OnDropDownClosed;
            this.comboBox.Sorted = comboBox.Sorted;

            this.Controls.Add(this.comboBox);


            CopyComboBoxEventHandlers(comboBox, this.comboBox);


            this.SizeChanged += customComboBox_SizeChanged;
            customComboBox_SizeChanged(this, null);

            this.DropDownStyle = comboBox.DropDownStyle;

            var allControlsIndex = ownerForm.allControls.IndexOf(comboBox);
            ownerForm.allControls.RemoveAt(allControlsIndex);
            ownerForm.allControls.Insert(allControlsIndex, this);

            parent.Controls.RemoveAt(index);
            comboBox.Tag = null;
            comboBox.Dispose();

            parent.Controls.Add(this);
            parent.Controls.SetChildIndex(this, index);
            if (parent is TableLayoutPanel layoutPanel)
                layoutPanel.SetCellPosition(this, cellPosition);

            this.Location = location;

            ownerForm.namesComboBoxes.AddReplace(Name, this);
        }

        private void initSkinned(ComboBox comboBox, ToolTip toolTip, bool enableFiltering)
        {
            this.scaledPx = ownerForm.scaledPx;
            this.enableFiltering = enableFiltering;

            downArrowComboBoxImage = Plugin.CopyBitmap(Plugin.DownArrowComboBoxImage);
            disabledDownArrowComboBoxImage = Plugin.CopyBitmap(Plugin.DisabledDownArrowComboBoxImage);

            this.initialDropDownWidth = comboBox.DropDownWidth;
            //if (comboBox.DropDownWidth < comboBox.Width)
            //    initialDropDownWidth = comboBox.Width;

            var dropDownWidth = comboBox.DropDownWidth;
            if (comboBox.DropDownWidth < comboBox.Width)
                dropDownWidth = comboBox.Width;

            this.initialDropDownHeight = comboBox.DropDownHeight + 3 * scaledPx;
            if (comboBox.DropDownHeight == 106)
                this.initialDropDownHeight = comboBox.ItemHeight * 22 + 3 * scaledPx;

            var parent = comboBox.Parent;
            var index = parent.Controls.IndexOf(comboBox);
            TableLayoutPanelCellPosition cellPosition = default;
            var location = comboBox.Location;

            if (parent is TableLayoutPanel panel)
                cellPosition = panel.GetCellPosition(comboBox);

            this.Font = comboBox.Font;
            this.ForeColor = Plugin.InputControlForeColor;
            this.BackColor = Plugin.InputControlBackColor;
            this.Margin = comboBox.Margin;
            this.Padding = Padding.Empty;
            this.Anchor = comboBox.Anchor;
            this.TabIndex = comboBox.TabIndex;
            this.TabStop = comboBox.TabStop;
            this.Tag = comboBox.Tag;
            this.Name = comboBox.Name;


            var customScrollBarInitialWidth = ControlsTools.GetCustomScrollBarInitialWidth(ownerForm.dpiScaling, Plugin.SbBorderWidth);

            this.textBox = (TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);

            if (toolTip != null)
                toolTip.SetToolTip(this.textBox, toolTip.GetToolTip(comboBox));

            this.textBox.Font = this.Font;
            this.textBox.Location = new Point(0, 0);
            this.textBox.Margin = Padding.Empty;
            this.textBox.Size = new Size(comboBox.Width - customScrollBarInitialWidth + scaledPx, this.textBox.Height);
            this.textBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            this.textBox.TabStop = false;

            if (comboBox.DropDownStyle == ComboBoxStyle.DropDownList)
                this.textBox.Click += button_Click;

            this.textBox.GotFocus += changeTextBoxBackground;
            this.textBox.LostFocus += changeTextBoxBackground;
            this.textBox.KeyPress += ownerForm.CustomComboBox_KeyPress;
            this.textBox.KeyDown += ownerForm.CustomComboBox_KeyDown;

            this.Size = new Size(comboBox.Width, this.textBox.Height);


            this.button = new Button();
            this.button.Font = this.Font;
            this.button.Margin = Padding.Empty;
            this.button.Location = new Point(comboBox.Width - customScrollBarInitialWidth, 0);
            this.button.Size = new Size(customScrollBarInitialWidth, this.textBox.Height);
            this.button.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.button.Text = string.Empty;
            this.button.ImageAlign = ContentAlignment.MiddleCenter;
            this.button.TextImageRelation = TextImageRelation.Overlay;
            this.button.Image = downArrowComboBoxImage;
            this.button.TabStop = false;

            this.button.Click += button_Click;


            this.Controls.Add(textBox);
            this.Controls.Add(button);

            ownerForm.skinControl(this.textBox);
            ownerForm.skinControl(this.button);

            ownerForm.nonDefaultingButtons.Add(this.button);


            var textBoxRectangle = new Rectangle(1, 1, this.textBox.Width - 2, this.textBox.Height - 2);
            var textBoxRegion = new Region(textBoxRectangle);
            this.textBox.Region = textBoxRegion;


            this.dropDownPanel = getDropDown(customScrollBarInitialWidth);
            var border = new ControlBorder(this.dropDownPanel, ownerForm);

            if (enableFiltering)
            {
                this.dropDown = null;

                this.dropDownPanel.Parent.Visible = false;

                ownerForm.Controls.Add(dropDownPanel.Parent);
                ownerForm.Controls.SetChildIndex(dropDownPanel.Parent, 0);
            }
            else
            {
                var controlHost = new ToolStripControlHost(dropDownPanel);
                controlHost.Font = Font;
                controlHost.AutoSize = true;
                controlHost.Padding = Padding.Empty;
                controlHost.Margin = Padding.Empty;


                this.dropDown = new ToolStripDropDown();
                this.dropDown.Font = this.Font;
                this.dropDown.BackColor = this.textBox.BackColor;
                this.dropDown.ForeColor = this.textBox.ForeColor;
                this.dropDown.Padding = Padding.Empty;
                this.dropDown.Margin = Padding.Empty;
                this.dropDown.AutoSize = true;
                this.dropDown.DropShadowEnabled = true;
                this.dropDown.Items.Add(controlHost);


                this.dropDown.Tag = this;
            }


            foreach (var item in comboBox.Items)
                this.listBox.Items.Add(item);

            this.listBox.GotFocus += changeTextBoxBackground;
            this.listBox.LostFocus += changeTextBoxBackground;
            this.listBox.MouseMove += listBox_MouseMove;
            this.listBox.Click += listBox_ItemChosen;
            this.listBox.ItemsChanged += listBox_ItemsChanged;
            this.listBox.SelectedIndexChanged += listBox_SelectedIndexChanged;

            this.listBox.AdjustHeight(dropDownWidth, this.initialDropDownHeight, 0);


            CopyComboBoxEventHandlers(comboBox, this);


            this.Paint += controlBorder_Paint;
            this.GotFocus += customComboBox_GotFocus;
            this.SizeChanged += customComboBox_SizeChanged;
            customComboBox_SizeChanged(this, null);

            this.DropDownStyle = comboBox.DropDownStyle;
            this.listBox.Sorted = comboBox.Sorted;

            var allControlsIndex = ownerForm.allControls.IndexOf(comboBox);
            ownerForm.allControls.RemoveAt(allControlsIndex);
            ownerForm.allControls.Insert(allControlsIndex, this);


            parent.Controls.RemoveAt(index);
            comboBox.Dispose();


            parent.Controls.Add(this);
            parent.Controls.SetChildIndex(this, index);
            if (parent is TableLayoutPanel layoutPanel)
                layoutPanel.SetCellPosition(this, cellPosition);

            this.Location = location;

            ownerForm.namesComboBoxes.AddReplace(Name, this);
            ownerForm.SizeChanged += ownerForm_SizeChanged;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                textBox?.Dispose();
                button?.Dispose();
                listBox?.Dispose();
                specialStateListBox?.Dispose();
                dropDownPanel?.Dispose();
                dropDown?.Dispose();

                comboBox?.Dispose();
            }

            base.Dispose(disposing);
        }

        ~CustomComboBox()
        {
            Dispose(false);
        }

        public override void Refresh()
        {
            if (textBox != null && (textBox.Text != cue || string.IsNullOrEmpty(cue)))
            {
                if (listBox.SelectedIndex == -1 || listBox.Items[listBox.SelectedIndex] == null)
                    Text = string.Empty;
                else
                    Text = listBox.Items[listBox.SelectedIndex].ToString();
            }

            base.Refresh();
        }

        internal string GetToolTip(ToolTip toolTip)
        {
            if (textBox != null)
                return toolTip.GetToolTip(textBox);
            else
                return toolTip.GetToolTip(comboBox);
        }

        internal void SetToolTip(ToolTip toolTip, string text)
        {
            if (textBox != null)
                toolTip.SetToolTip(textBox, text);
            else
                toolTip.SetToolTip(comboBox, text);
        }

        internal bool IsFiltered()
        {
            return enableFiltering;
        }

        internal void ShowDropDownContent()
        {
            if (listBox != null)
                listBox.Visible = true;
            else
                ; //comboBox.DroppedDown = true; //-----
        }

        internal void HideDropDownContent()
        {
            if (listBox != null)
                listBox.Visible = false;
            else
                ; // comboBox.DroppedDown = false; //-----
        }

        internal object DataSource
        {
            get
            {
                if (listBox == null)
                    return comboBox.DataSource;
                else
                    return listBox.DataSource;
            }

            set
            {
                if (listBox == null)
                    comboBox.DataSource = value;
                else
                    listBox.DataSource = value;
            }
        }

        internal int IndexOfText(string text)
        {
            var index = SelectedIndex;

            if (index >= 0 && Items[index].ToString() == text)
                return index;

            for (var i = 0; i < Items.Count; i++)
                if (Items[i].ToString() == text)
                    return i;

            return -1;
        }

        internal void CopyTo(object[] array, int index)
        {
            if (listBox != null)
            {
                if (specialStateListBox == null)
                {
                    listBox.Items.CopyTo(array, index);
                }
                else
                {
                    for (int i = index; i < listBox.Items.Count; i++)
                    {
                        string itemText = listBox.Items[i] as string;
                        string itemState = specialStateListBox.Items[i] as string;
                        array[i] = itemState + " " + itemText;
                    }
                }
            }
            else
            {
                comboBox.Items.CopyTo(array, index);
            }
        }

        internal void Add(object item)
        {
            setItemsChangedProcessed = true;

            if (listBox != null)
            {
                if (specialStateListBox == null)
                {
                    listBox.Items.Add(item);
                }
                else
                {
                    string state = GetDefaultSpecialState();
                    string text = item as string;
                    if (text.Length >= GetSpecialStateCharCount(true) && text[GetSpecialStateCharCount(true) - 1] == ' ')
                    {
                        state = text.Substring(0, GetSpecialStateCharCount(true) - 1);
                        text = text.Substring(GetSpecialStateCharCount(true));
                    }

                    listBox.Items.Add(text);
                    specialStateListBox.Items.Add(state);
                }

                listBox_ItemsChanged(null, null);
            }
            else
            {
                comboBox.Items.Add(item);
            }

            ItemsChangedSpecialMode();
            lastSelectedIndex = SelectedIndex;

            setItemsChangedProcessed = false;
        }

        internal void AddRange(object[] items)
        {
            setItemsChangedProcessed = true;

            if (listBox != null)
            {
                if (specialStateListBox == null)
                {
                    listBox.Items.AddRange(items);
                }
                else
                {
                    for (int i = 0; i < items.Length; i++)
                    {
                        string state = GetDefaultSpecialState();
                        string text = items[i] as string;
                        if (text.Length >= GetSpecialStateCharCount(true) && text[GetSpecialStateCharCount(true) - 1] == ' ')
                        {
                            state = text.Substring(0, GetSpecialStateCharCount(true) - 1);
                            text = text.Substring(GetSpecialStateCharCount(true));
                        }

                        listBox.Items.Add(text);
                        specialStateListBox.Items.Add(state);
                    }
                }

                listBox_ItemsChanged(null, null);
            }
            else
            {
                comboBox.Items.AddRange(items);
            }

            ItemsChangedSpecialMode();
            lastSelectedIndex = SelectedIndex;

            setItemsChangedProcessed = false;
        }

        internal void AddRange(IList<object> items)
        {
            setItemsChangedProcessed = true;

            if (listBox != null)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    string state = GetDefaultSpecialState();
                    string text = items[i] as string;
                    if (text.Length >= GetSpecialStateCharCount(true) && text[GetSpecialStateCharCount(true) - 1] == ' ')
                    {
                        state = text.Substring(0, GetSpecialStateCharCount(true) - 1);
                        text = text.Substring(GetSpecialStateCharCount(true));
                    }

                    listBox.Items.Add(text);
                    specialStateListBox.Items.Add(state);
                }

                listBox_ItemsChanged(null, null);
            }
            else
            {
                foreach (object item in items)
                    comboBox.Items.Add(item);
            }

            ItemsChangedSpecialMode();
            lastSelectedIndex = SelectedIndex;

            setItemsChangedProcessed = false;
        }

        internal void RemoveAt(int index)
        {
            setItemsChangedProcessed = true;

            Items.RemoveAt(index);

            if (listBox != null)
            {
                listBox_ItemsChanged(null, null);

                if (specialStateListBox != null)
                    specialStateListBox.Items.RemoveAt(index);
            }

            ItemsChangedSpecialMode();
            lastSelectedIndex = SelectedIndex;

            setItemsChangedProcessed = false;
        }

        internal void Remove(object item)
        {
            setItemsChangedProcessed = true;

            int index = Items.IndexOf(item);
            Items.RemoveAt(index);

            if (listBox != null)
            {
                listBox_ItemsChanged(null, null);

                if (specialStateListBox != null)
                    specialStateListBox.Items.RemoveAt(index);
            }

            ItemsChangedSpecialMode();
            lastSelectedIndex = SelectedIndex;

            setItemsChangedProcessed = false;
        }

        internal void Insert(int index, object item, string specialState = null)
        {
            setItemsChangedProcessed = true;

            if (defaultSpecialState == null)
            {
                Items.Insert(index, item);
            }
            else if (specialStateListBox == null)
            {
                Items.Insert(index, specialState + item);
            }
            else
            {
                Items.Insert(index, item);
                specialStateListBox.Items.Insert(index, specialState);
            }

            if (listBox != null)
                listBox_ItemsChanged(null, null);

            ItemsChangedSpecialMode();
            lastSelectedIndex = SelectedIndex;

            setItemsChangedProcessed = false;
        }

        internal void ItemsClear()
        {
            setItemsChangedProcessed = true;

            if (listBox != null)
            {
                listBox.Items.Clear();
                listBox_ItemsChanged(null, null);

                if (specialStateListBox != null)
                    specialStateListBox.Items.Clear();
            }
            else
            {
                comboBox.Items.Clear();
            }

            SelectedIndex = -1;

            ItemsChangedSpecialMode();
            lastSelectedIndex = SelectedIndex;

            setItemsChangedProcessed = false;
        }

        internal IList Items
        {
            get
            {
                if (listBox != null)
                    return listBox.Items;
                else
                    return comboBox.Items;
            }
        }

        internal bool IsFocused()
        {
            if (listBox == null)
                return comboBox.Focused || comboBox.DroppedDown;
            else
                return listBox.Focused || button.Focused || specialStateListBox?.Focused == true || droppedDown;
        }

        internal string GetItemText(object item)
        {
            if (listBox != null)
                return listBox.GetItemText(item);
            else
                return comboBox.GetItemText(item);
        }

        internal bool StandardKeyPressHandling
        {
            get
            {
                if (enabled && DropDownStyle == ComboBoxStyle.DropDownList)
                    return false;
                else
                    return true;
            }
        }

        //Returns: false - process space & key right; null - process space
        internal bool? StandardKeyDownHandling
        {
            get
            {
                if (enabled && DropDownStyle == ComboBoxStyle.DropDownList && !DroppedDown)
                    return false;
                else if (enabled && DropDownStyle == ComboBoxStyle.DropDown && DroppedDown
                    && ((SelectionStart == Text.Length && SelectionLength == 0) || (SelectionStart == 0 && SelectionLength == Text.Length))
                )
                    return false;
                else if (enabled && DropDownStyle == ComboBoxStyle.DropDown && DroppedDown
                    && (SelectionStart == 0 && SelectionLength == 0)
                )
                    return null;
                else
                    return true;
            }
        }

        internal int SpecialState //Currently only for enumerating in additional column
        {
            get; set;
        }

        internal int MaxSpecialState //Currently only for enumerating in additional column
        {
            get; set;
        } = 5;

        internal bool Sorted
        {
            get
            {
                if (listBox != null)
                    return listBox.Sorted;
                else
                    return comboBox.Sorted;
            }

            set
            {
                if (listBox != null)
                    listBox.Sorted = value;
                else
                    comboBox.Sorted = value;
            }
        }

        internal int SelectionStart
        {
            get
            {
                if (textBox != null)
                    return textBox.SelectionStart;
                else
                    return comboBox.SelectionStart;
            }

            set
            {
                if (textBox != null)
                    textBox.SelectionStart = value;
                else
                    comboBox.SelectionStart = value;
            }
        }

        internal int SelectionLength
        {
            get
            {
                if (textBox != null)
                    return textBox.SelectionLength;
                else
                    return comboBox.SelectionLength;
            }

            set
            {
                if (textBox != null)
                    textBox.SelectionLength = value;
                else
                    comboBox.SelectionLength = value;
            }
        }

        internal void SetText(string value, bool forceDropDownListText)
        {
            var index = IndexOfText(value);

            if (textBox != null)
            {
                var oldText = textBox.Text;
                var oldSelectedIndex = listBox.SelectedIndex;
                object oldSelectedItem = null;
                if (listBox.SelectedIndex >= 0)
                    oldSelectedItem = listBox.Items[listBox.SelectedIndex];

                listBox.SelectedIndex = index;
                lastSelectedIndex = index;

                if (forceDropDownListText || index >= 0 || DropDownStyle == ComboBoxStyle.DropDown)
                    textBox.Text = value;
                else //if (!forceDropDownListText && index == -1 && DropDownStyle == ComboBoxStyle.DropDownList)
                    textBox.Text = cue;

                SetTextBoxColors(null);

                if (oldSelectedIndex != listBox.SelectedIndex)
                    Events[EVENT_SELECTEDINDEXCHANGED]?.DynamicInvoke(this, null);

                if (listBox.SelectedIndex != -1 && oldSelectedItem != listBox.Items[listBox.SelectedIndex] || oldSelectedItem != null)
                    Events[EVENT_SELECTEDITEMCHANGED]?.DynamicInvoke(this, null);

                if (oldText != value)
                    this.FireEvent("TextChanged", null);
            }
            else//if (comboBox != null)
            {
                var oldText = comboBox.Text;
                var oldSelectedIndex = comboBox.SelectedIndex;
                object oldSelectedItem = null;
                if (comboBox.SelectedIndex >= 0)
                    oldSelectedItem = comboBox.Items[comboBox.SelectedIndex];

                if (!forceDropDownListText)
                {
                    comboBox.SetCue(cue);
                    comboBox.Text = value;
                }
                else if (DropDownStyle == ComboBoxStyle.DropDownList) //&& forceDropDownListText && index == -1
                {
                    comboBox.SetCue(value);
                    comboBox.Text = string.Empty; //Let's show cue
                }
                else //if (DropDownStyle == ComboBoxStyle.DropDown && forceDropDownListText && index == -1)
                {
                    comboBox.SetCue(cue);

                    // handing off the reset of the ComboBox selected value to a delegate method - using MethodInvoker on the forms main thread is an efficient to do this
                    // see https://msdn.microsoft.com/en-us/library/system.windows.forms.methodinvoker(v=vs.110).aspx
                    BeginInvoke(new Action(() => { comboBox.Text = value; }));
                }

                if (oldSelectedIndex != comboBox.SelectedIndex)
                    Events[EVENT_SELECTEDINDEXCHANGED]?.DynamicInvoke(this, null);

                if (comboBox.SelectedIndex != -1 && oldSelectedItem != comboBox.Items[comboBox.SelectedIndex] || oldSelectedItem != null)
                    Events[EVENT_SELECTEDITEMCHANGED]?.DynamicInvoke(this, null);

                if (oldText != value)
                    this.FireEvent("TextChanged", null);
            }
        }

        public override string Text
        {
            get
            {
                if (textBox != null)
                    return textBox.Text;
                else
                    return comboBox.Text;
            }

            set
            {
                if (textBox != null && textBox.Text != value)
                    SetText(value, false);
                else if (textBox == null && comboBox.Text != value)
                    SetText(value, false);
            }
        }

        internal int SelectedIndex
        {
            get
            {
                if (listBox != null)
                    return listBox.SelectedIndex;
                else
                    return comboBox.SelectedIndex;
            }

            set
            {
                if (listBox != null && lastSelectedIndex != value)
                {
                    object oldSelectedItem = null;
                    if (listBox.SelectedIndex >= 0)
                        oldSelectedItem = listBox.Items[listBox.SelectedIndex];

                    listBox.SelectedIndex = value;
                    lastSelectedIndex = value;


                    if (value != -1)
                        textBox.Text = listBox.Items[value].ToString();
                    else if (DropDownStyle == ComboBoxStyle.DropDownList)
                        textBox.Text = string.Empty;


                    Events[EVENT_SELECTEDINDEXCHANGED]?.DynamicInvoke(this, null);

                    if (listBox.SelectedIndex != -1 && oldSelectedItem != listBox.Items[listBox.SelectedIndex] || oldSelectedItem != null)
                        Events[EVENT_SELECTEDITEMCHANGED]?.DynamicInvoke(this, null);


                    if (value != -1)
                        Text = listBox.Items[value].ToString();
                    else if (DropDownStyle == ComboBoxStyle.DropDownList)
                        Text = string.Empty;
                }
                else if (listBox == null && comboBox.SelectedIndex != value)
                {
                    object oldSelectedItem = null;
                    if (comboBox.SelectedIndex != -1)
                        oldSelectedItem = comboBox.Items[comboBox.SelectedIndex];

                    comboBox.SelectedIndex = value;


                    Events[EVENT_SELECTEDINDEXCHANGED]?.DynamicInvoke(this, null);

                    if (comboBox.SelectedIndex != -1 && oldSelectedItem != comboBox.Items[comboBox.SelectedIndex] || oldSelectedItem != null)
                        Events[EVENT_SELECTEDITEMCHANGED]?.DynamicInvoke(this, null);

                    if (value != -1)
                    {
                        string text = comboBox.Items[value].ToString();
                        if (defaultSpecialState != null && text.Length > GetSpecialStateCharCount(true))
                            text = text.Substring(GetSpecialStateCharCount(true));
                        else if (defaultSpecialState != null)
                            text = string.Empty;

                        Text = text;
                    }
                    else if (DropDownStyle == ComboBoxStyle.DropDownList)
                    {
                        Text = string.Empty;
                    }
                }
            }
        }

        internal object SelectedItem
        {
            get
            {
                if (listBox != null)
                {
                    if (listBox.SelectedIndex != -1)
                        return listBox.Items[listBox.SelectedIndex];
                    else
                        return null;
                }
                else
                {
                    if (comboBox.SelectedIndex != -1)
                    {
                        if (defaultSpecialState == null)
                        {
                            return comboBox.Items[comboBox.SelectedIndex];
                        }
                        else
                        {
                            string text = comboBox.Items[comboBox.SelectedIndex].ToString();
                            if (defaultSpecialState != null && text.Length > GetSpecialStateCharCount(true))
                                text = text.Substring(GetSpecialStateCharCount(true));
                            else if (defaultSpecialState != null)
                                text = string.Empty;

                            return text;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            set
            {
                if (listBox != null && value == null)
                {
                    var oldSelectedIndex = listBox.SelectedIndex;
                    listBox.SelectedIndex = -1;

                    string text = textBox.Text;

                    textBox.Text = string.Empty;

                    if (oldSelectedIndex != listBox.SelectedIndex)
                        Events[EVENT_SELECTEDINDEXCHANGED]?.DynamicInvoke(this, null);

                    if (oldSelectedIndex != listBox.SelectedIndex)
                        Events[EVENT_SELECTEDITEMCHANGED]?.DynamicInvoke(this, null);

                    textBox.Text = text;

                    Text = string.Empty;
                }
                else if (listBox == null && value == null)
                {
                    var oldSelectedIndex = comboBox.SelectedIndex;
                    comboBox.SelectedIndex = -1;

                    if (oldSelectedIndex != comboBox.SelectedIndex)
                        Events[EVENT_SELECTEDINDEXCHANGED]?.DynamicInvoke(this, null);

                    if (oldSelectedIndex != comboBox.SelectedIndex)
                        Events[EVENT_SELECTEDITEMCHANGED]?.DynamicInvoke(this, null);

                    Text = string.Empty;
                }
                else if (listBox != null && ((listBox.SelectedIndex != -1 && listBox.Items[listBox.SelectedIndex] != value) || value != null))
                {
                    var oldSelectedIndex = listBox.SelectedIndex;
                    object oldSelectedItem = null;
                    if (listBox.SelectedIndex != -1)
                        oldSelectedItem = listBox.Items[listBox.SelectedIndex];

                    int index = listBox.Items.IndexOf(value);
                    listBox.SelectedIndex = index;

                    lastSelectedIndex = listBox.SelectedIndex;


                    string text = textBox.Text;

                    if (listBox.SelectedIndex != -1 && listBox.Items[listBox.SelectedIndex] != null)
                        textBox.Text = listBox.Items[listBox.SelectedIndex].ToString();
                    else if (DropDownStyle == ComboBoxStyle.DropDownList)
                        textBox.Text = string.Empty;


                    if (oldSelectedIndex != listBox.SelectedIndex)
                        Events[EVENT_SELECTEDINDEXCHANGED]?.DynamicInvoke(this, null);

                    if (listBox.SelectedIndex != -1 && oldSelectedItem != listBox.Items[listBox.SelectedIndex] || oldSelectedItem != null)
                        Events[EVENT_SELECTEDITEMCHANGED]?.DynamicInvoke(this, null);


                    textBox.Text = text;

                    if (listBox.SelectedIndex != -1 && listBox.Items[listBox.SelectedIndex] != null)
                        Text = listBox.Items[listBox.SelectedIndex].ToString();
                    else if (DropDownStyle == ComboBoxStyle.DropDownList)
                        Text = string.Empty;
                }
                else if (listBox == null && ((comboBox.SelectedIndex != -1 && comboBox.Items[comboBox.SelectedIndex] != value) || value != null))
                {
                    var oldSelectedIndex = comboBox.SelectedIndex;
                    object oldSelectedItem = null;
                    if (comboBox.SelectedIndex != -1)
                        oldSelectedItem = comboBox.Items[comboBox.SelectedIndex];

                    int index = comboBox.Items.IndexOf(value);
                    comboBox.SelectedIndex = index;


                    if (oldSelectedIndex != comboBox.SelectedIndex)
                        Events[EVENT_SELECTEDINDEXCHANGED]?.DynamicInvoke(this, null);

                    if (comboBox.SelectedIndex != -1 && oldSelectedItem != comboBox.Items[comboBox.SelectedIndex] || oldSelectedItem != null)
                        Events[EVENT_SELECTEDITEMCHANGED]?.DynamicInvoke(this, null);


                    string state = string.Empty;

                    if (defaultSpecialState != null)
                    {
                        state = comboBox.Items[comboBox.SelectedIndex].ToString(); //-V3095
                        state = state.Substring(GetSpecialStateCharCount(true));
                    }

                    if (comboBox.SelectedIndex != -1 && comboBox.Items[comboBox.SelectedIndex] != null)
                        Text = state + value;
                    else if (DropDownStyle == ComboBoxStyle.DropDownList)
                        Text = string.Empty;
                }
            }
        }

        internal bool ReadOnly
        {
            get
            {
                if (textBox != null)
                    return textBoxReadOnly;
                else
                    return comboBox.DropDownStyle == ComboBoxStyle.DropDownList;
            }

            set
            {
                textBoxReadOnly = value;

                if (textBox != null)
                    textBox.ReadOnly = value || !enabled;
                else if (value)
                    comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                else
                    comboBox.DropDownStyle = ComboBoxStyle.DropDown;
            }
        }

        internal void ForceReadonly(bool state)
        {
            if (state)
            {
                if (textBox != null)
                {
                    textBoxReadOnly = textBox.ReadOnly;
                    textBox.ReadOnly = true;
                    button.Enable(false);
                }
                else
                {
                    comboBox.Enable(false);
                }
            }
            else
            {
                if (textBox != null)
                {
                    textBox.ReadOnly = textBoxReadOnly;
                    button.Enable(true);
                }
                else
                {
                    comboBox.Enable(true);
                }
            }
        }

        internal bool IsReallyEnabled()
        {
            return enabled;
        }

        private void OnVisibleChanged(object sender, EventArgs e)
        {
            if (textBox != null)
            {
                textBox.Visible = Visible;
                button.Visible = Visible;
            }
            else
            {
                comboBox.Visible = Visible;
            }
        }

        internal bool GetEnabled()
        {
            return enabled;
        }

        internal void SetTextBoxColors(bool? enable)
        {
            if (textBox != null)
            {
                enable = (this.IsEnabled() && enable == null) || enable == true;

                if (enable == true)
                {
                    var inputControlForeColor = Plugin.InputControlForeColor;

                    if (textBox.Focused && textBox.ReadOnly)
                        inputControlForeColor = Plugin.ButtonMouseOverForeColor;

                    //Let's show cue for readonly mode if cue is defined
                    if (listBox.SelectedIndex == -1 && (string.IsNullOrEmpty(textBox.Text) || textBox.Text == cue) && textBox.ReadOnly)
                        textBox.ForeColor = inputControlForeColor;
                    //Let's show cue for editable mode if cue is defined
                    else if (listBox.SelectedIndex == -1 && (string.IsNullOrEmpty(textBox.Text) || textBox.Text == cue) && !textBox.ReadOnly)
                        textBox.ForeColor = Plugin.InputControlDimmedForeColor;
                    //No cue, just "Text"
                    else
                        textBox.ForeColor = inputControlForeColor;

                    if (textBox.Focused && textBox.ReadOnly)
                        textBox.BackColor = Plugin.ButtonMouseOverBackColor;
                    else
                        textBox.BackColor = Plugin.InputControlBackColor;
                }
                else //Disabled
                {
                    textBox.ForeColor = Plugin.InputControlDeepDimmedForeColor;//---
                    textBox.BackColor = Plugin.InputControlDeepDimmedBackColor; //---
                }
            }
        }

        internal void SetCue(string cue)
        {
            this.cue = cue;

            if (textBox != null)
                textBox.SetCue(cue);
            else
                comboBox.SetCue(cue);
        }

        private void OnEnabledChanged(object sender, EventArgs e)
        {
            if (ignoreEnabledChanged)
            {
                ignoreEnabledChanged = false;
                return;
            }

            enabled = Enabled;

            if (!Enabled)
            {
                ignoreEnabledChanged = true;
                Enabled = true;
            }
            ;

            SetEnabled(enabled);
        }

        internal void SetEnabled(bool enable)
        {
            enabled = enable;

            if (textBox != null)
            {
                textBox.ReadOnly = ReadOnly || !enabled;
                button.Enable(enabled);

                setButtonColors();
                SetTextBoxColors(enabled);
            }
            else
            {
                comboBox.Enable(enabled);
            }
        }

        internal bool DroppedDown
        {
            get
            {
                if (textBox == null)
                    droppedDown = comboBox.DroppedDown;

                return droppedDown;
            }

            set
            {
                if (droppedDown != value)
                {
                    if (textBox != null)
                    {
                        button_Click(null, null);
                    }
                    else //if (textBox == null)
                    {
                        string text = comboBox.Text;
                        droppedDown = value;
                        comboBox.DroppedDown = value;

                        if (comboBox.DropDownStyle == ComboBoxStyle.DropDown && comboBox.Text != text)
                            comboBox.Text = text;
                    }
                }
            }
        }

        private void OnDropDownClosed(object sender, EventArgs e)
        {
            if (DropDownStyle == ComboBoxStyle.DropDown && SelectionLength == 0)
            {
                SelectionStart = 0;
                SelectionLength = Text.Length;
            }

            Events[EVENT_DROPDOWNCLOSED]?.DynamicInvoke(this, null);
        }

        internal ComboBoxStyle DropDownStyle
        {
            get
            {
                if (textBox != null)
                    return textBoxReadOnly ? ComboBoxStyle.DropDownList : ComboBoxStyle.DropDown;
                else
                    return comboBox.DropDownStyle;
            }

            set
            {
                if (textBox == null && comboBox.DropDownStyle == value)
                    ;
                else if (textBox == null && comboBox.DropDownStyle != value)
                    comboBox.DropDownStyle = value;
                else if (value == ComboBoxStyle.DropDown)
                    ReadOnly = false;
                else if (value == ComboBoxStyle.DropDownList)
                    ReadOnly = true;
                else
                    throw new Exception(Plugin.ExInvalidDropdownStyle);
            }
        }

        private static void CopyComboBoxEventHandlers(ComboBox refComboBox, Control destControl)
        {
            if (destControl is CustomComboBox customComboBox)
            {
                refComboBox.CopyEventHandlersTo(customComboBox.textBox, "Leave", false);
                refComboBox.CopyEventHandlersTo(customComboBox.listBox, "Leave", false);
                refComboBox.CopyEventHandlersTo(customComboBox.textBox, "TextChanged", false);
                refComboBox.CopyEventHandlersTo(destControl, "SelectedIndexChanged", false);
                refComboBox.CopyEventHandlersTo(destControl, "SelectedItemChanged", false);
                refComboBox.CopyEventHandlersTo(destControl, "VisibleChanged", false);
                refComboBox.CopyEventHandlersTo(destControl, "EnabledChanged", false);
                refComboBox.CopyEventHandlersTo(destControl, "DropDownClosed", false);
            }
            else
            {
                refComboBox.CopyEventHandlersTo(destControl, "Leave", false);
                refComboBox.CopyEventHandlersTo(destControl, "TextChanged", false);
                refComboBox.CopyEventHandlersTo(destControl, "SelectedIndexChanged", false);
                refComboBox.CopyEventHandlersTo(destControl, "SelectedItemChanged", false);
                refComboBox.CopyEventHandlersTo(destControl, "VisibleChanged", false);
                refComboBox.CopyEventHandlersTo(destControl, "EnabledChanged", false);
                refComboBox.CopyEventHandlersTo(destControl, "DropDownClosed", false);
            }
        }

        private void setButtonColors()
        {
            ownerForm.setSkinnedControlColors(button, enabled);

            if (this.IsEnabled())
                button.Image = downArrowComboBoxImage;
            else
                button.Image = disabledDownArrowComboBoxImage;
        }

        private void controlBorder_Paint(object sender, PaintEventArgs e)
        {
            ControlsTools.DrawBorder(this, borderColor, borderColorActive, borderColorDisabled);
        }

        private void changeTextBoxBackground(object sender, EventArgs e)
        {
            SetTextBoxColors(null);
        }

        internal bool OpenCustomDropDown()
        {
            if (textBox != null)
            {
                DroppedDown = true;
                return true;
            }

            return false;
        }

        internal void SelectCurrentDropDownItem()
        {
            if (textBox != null)
            {
                listBox_ItemChosen(null, null);
            }
            else
            {
                comboBox.SelectedItem = comboBox.Items[comboBox.SelectedIndex];
                DroppedDown = false;
                Events[EVENT_SELECTEDITEMCHANGED]?.DynamicInvoke(this, null);
            }
        }

        internal void ItemsChangedSpecialMode() //If items are changed using enum mode. Must be called manually
        {
            if (specialStateListBox != null && specialStateListBox.Items.Count < listBox.Items.Count)
                for (int i = specialStateListBox.Items.Count; i < listBox.Items.Count; i++)
                    specialStateListBox.Items.Add(defaultSpecialState);
            else if (specialStateListBox != null && specialStateListBox.Items.Count > listBox.Items.Count)
                for (int i = listBox.Items.Count; i < specialStateListBox.Items.Count; i++)
                    specialStateListBox.Items.Remove(listBox.Items.Count);


            if (specialStateListBox != null)
            {
                for (int i = 0; i < specialStateListBox.Items.Count; i++)
                {
                    if (specialStateListBox.Items[i] == null || string.IsNullOrWhiteSpace(specialStateListBox.Items[i].ToString()))
                        specialStateListBox.Items[i] = defaultSpecialState;
                }
            }
            else if (defaultSpecialState != null)
            {
                for (int i = 0; i < comboBox.Items.Count; i++)
                {
                    if (comboBox.Items[i] == null || comboBox.Items[i].ToString().Length < defaultSpecialState.Length)
                        comboBox.Items[i] = defaultSpecialState + comboBox.Items[i];
                    else if (comboBox.Items[i].ToString()[defaultSpecialState.Length - 1] != ' ')
                        comboBox.Items[i] = defaultSpecialState + comboBox.Items[i];
                }
            }
        }

        private void drawCustomDropDown()
        {
            var dropDownWidth = Width;
            if (dropDownWidth < initialDropDownWidth)
                dropDownWidth = initialDropDownWidth;

            if (listBox.GetItemsHeight() > initialDropDownHeight)
                listBox.AdjustWidth(dropDownWidth, true, GetSpecialStateColumnWidth());
            else
                listBox.AdjustWidth(dropDownWidth, false, GetSpecialStateColumnWidth());

            listBox.AdjustHeight(dropDownWidth, initialDropDownHeight, GetSpecialStateColumnWidth());
            specialStateListBox?.AdjustAdditionalListHeight(initialDropDownHeight);


            dropDownPanel.ColumnStyles[1].Width = listBox.Width;

            string text = textBox.Text;

            int index = listBox.SelectedIndex;
            if (listBox.SelectedIndex == -1)
            {
                index = IndexOfText(textBox.Text);
                if (index >= 0)
                    listBox.SelectedIndex = index;
                else if (listBox.Items.Count > 0)
                    listBox.SelectedIndex = 0;
            }

            var textBoxScreenFormLocation = textBox.PointToScreen(textBox.Location);
            if (enableFiltering) //Filtered list
                textBoxScreenFormLocation = ownerForm.PointToClient(textBoxScreenFormLocation);

            //try to position drop down below textbox
            var ptBottomScreen = new Point(textBoxScreenFormLocation.X, textBoxScreenFormLocation.Y + textBox.Height);

            //determine if it will fit on the screen below the textbox
            var dropdownBoundsDown = new Rectangle(ptBottomScreen, dropDownPanel.Size);

            bool openDropDownUp = false;
            bool openDropDownLeft = false;

            if (enableFiltering) //Filtered list
            {
                if (dropdownBoundsDown.Bottom > ownerForm.Height)
                    //show below
                    openDropDownUp = true;

                if (dropdownBoundsDown.Right > ownerForm.Width)
                    //show to the left
                    openDropDownLeft = true;


                if (!openDropDownUp && textBoxScreenFormLocation.Y + listBox.Height > ownerForm.Height)
                {
                    listBox.Height = textBoxScreenFormLocation.Y - ownerForm.Height - 10; //-10: small vertical adjustment
                    dropDownPanel.RowStyles[0].Height = listBox.Height;
                }
                else if (openDropDownUp && textBoxScreenFormLocation.Y - listBox.Height < 0)
                {
                    listBox.Height = textBoxScreenFormLocation.Y - 10; //-10: small vertical adjustment
                    dropDownPanel.RowStyles[0].Height = listBox.Height;
                }

                ControlsTools.BorderedControl_Resize(dropDownPanel, null);

                if (!openDropDownUp && !openDropDownLeft)
                    dropDownPanel.Location = new Point(textBoxScreenFormLocation.X, textBoxScreenFormLocation.Y + textBox.Height);
                else if (openDropDownUp && !openDropDownLeft)
                    dropDownPanel.Location = new Point(textBoxScreenFormLocation.X, textBoxScreenFormLocation.Y - dropDownPanel.Height);
                else if (!openDropDownUp && openDropDownLeft)
                    dropDownPanel.Location = new Point(textBoxScreenFormLocation.X - dropDownPanel.Width + textBox.Width, textBoxScreenFormLocation.Y + textBox.Height);
                else //if (openDropDownUp && openDropDownLeft)
                    dropDownPanel.Location = new Point(textBoxScreenFormLocation.X - dropDownPanel.Width + textBox.Width, textBoxScreenFormLocation.Y - dropDownPanel.Height);

                dropDownPanel.Parent.Show();
            }
            else
            {
                if (dropdownBoundsDown.Bottom > Screen.GetWorkingArea(dropdownBoundsDown).Bottom)
                    //show below
                    openDropDownUp = true;

                if (dropdownBoundsDown.Right > Screen.GetWorkingArea(dropdownBoundsDown).Right)
                    //show to the left
                    openDropDownLeft = true;


                if (!openDropDownUp && !openDropDownLeft)
                    dropDown.Show(ptBottomScreen, ToolStripDropDownDirection.BelowRight);
                else if (openDropDownUp && !openDropDownLeft)
                    dropDown.Show(textBoxScreenFormLocation, ToolStripDropDownDirection.AboveRight);
                else if (!openDropDownUp && openDropDownLeft)
                    dropDown.Show(ptBottomScreen, ToolStripDropDownDirection.BelowLeft);
                else //if (openDropDownUp && openDropDownLeft)
                    dropDown.Show(textBoxScreenFormLocation, ToolStripDropDownDirection.AboveLeft);
            }

            textBox.Text = text;
            listBox.SelectedIndex = index;
        }

        private void button_Click(object sender, EventArgs e)
        {
            if (!droppedDown)
            {
                if (!enabled)
                    return;


                textBox.Focus();
                drawCustomDropDown();

                if (sender == null) //"DroppedDown" property changed
                    textBox.Focus();
                else
                    listBox.Focus();

                droppedDown = true;
            }
            else
            {
                SpecialState = -1;

                if (enableFiltering)
                    //dropDownPanel.Hide();
                    dropDownPanel.Parent.Hide();
                else
                    dropDown.Close();

                if (sender != null)
                    textBox.Focus();

                droppedDown = false;

                Events[EVENT_DROPDOWNCLOSED]?.DynamicInvoke(this, null);
            }
        }

        private void customComboBox_SizeChanged(object sender, EventArgs e)
        {
            if (textBox == null)
            {
                if (Width < initialDropDownWidth)
                    comboBox.DropDownWidth = initialDropDownWidth;
                else
                    comboBox.DropDownWidth = Width;
            }
            else
            {
                var textBoxRectangle = new Rectangle(1, 1, textBox.Width - 2, textBox.Height - 2);
                var textBoxRegion = new Region(textBoxRectangle);
                textBox.Region = textBoxRegion;
            }
        }

        private void listBox_ItemsChanged(object sender, EventArgs e)
        {
            if (itemsChangedProcessed)
            {
                itemsChangedProcessed = false;
                return;
            }

            itemsChangedProcessed = setItemsChangedProcessed;

            var dropDownWidth = Width;
            if (dropDownWidth < initialDropDownWidth)
                dropDownWidth = initialDropDownWidth;

            listBox.AdjustHeight(dropDownWidth, initialDropDownHeight, GetSpecialStateColumnWidth());
            specialStateListBox?.AdjustAdditionalListHeight(initialDropDownHeight);

            if (DropDownStyle == ComboBoxStyle.DropDownList)
            {
                if (listBox.SelectedIndex == -1)
                    Text = string.Empty;
                else if (DropDownStyle == ComboBoxStyle.DropDownList && listBox.Items[listBox.SelectedIndex] == null)
                    Text = string.Empty;
                else
                    Text = listBox.Items[listBox.SelectedIndex].ToString(); //-V3125
            }
            else if (listBox.SelectedIndex >= 0)
            {
                Text = listBox.Items[listBox.SelectedIndex].ToString();
            }

            PluginWindowTemplate.UpdateCustomScrollBars(listBox);
        }

        private void customComboBox_GotFocus(object sender, EventArgs e)
        {
            textBox.Focus();
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (specialStateListBox != null)
                specialStateListBox.SelectedIndex = listBox.SelectedIndex;
        }

        private void additionalListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox.SelectedIndex = specialStateListBox.SelectedIndex;
        }

        internal void listBox_ItemChosen(object sender, EventArgs e)
        {
            if (recordMode && GetSpecialStateItem(listBox.SelectedIndex) == GetDefaultSpecialState())
            {
                System.Media.SystemSounds.Beep.Play();
                return;
            }

            if (lastSelectedIndex == listBox.SelectedIndex)
                lastSelectedIndex = -2;

            SelectedIndex = listBox.SelectedIndex;
            DroppedDown = false;
            textBox.Focus();
        }

        private void listBox_MouseMove(object sender, MouseEventArgs e)
        {
            var currentListBox = sender as CustomListBox;
            var index = currentListBox.IndexFromPoint(e.X, e.Y);

            if (index >= 0 && index < listBox.Items.Count)
            {
                listBox.SelectedIndex = index;

                if (specialStateListBox != null)
                    specialStateListBox.SelectedIndex = index;
            }
        }

        private void ownerForm_SizeChanged(object sender, EventArgs e)
        {
            if (enableFiltering && droppedDown)
                drawCustomDropDown();
        }
    }

    internal class CustomTextBox : TextBox
    {
        private readonly Color borderColorDisabled = Plugin.ScrollBarBorderColor;
        private readonly Color borderColorActive = Plugin.ScrollBarFocusedBorderColor;
        private readonly Color borderColor = Plugin.ScrollBarBorderColor;

        internal CustomVScrollBar vScrollBar;

        internal CustomTextBox(Color borderColor, Color borderColorDisabled, Color borderColorActive)
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            //SetStyle(ControlStyles.DoubleBuffer, true);

            this.borderColorDisabled = borderColorDisabled;
            this.borderColorActive = borderColorActive;
            this.borderColor = borderColor;

            var thisRectangle = new Rectangle(1, 1, Width - 2, Height - 2);
            var thisRegion = new Region(thisRectangle);
            Region = thisRegion;

            Paint += (sender, args) => { ControlsTools.DrawBorder(this, this.borderColor, this.borderColorActive, this.borderColorDisabled); };
        }

        internal int ScrollPosition { get; set; }

        internal void UpdateCaretPositionForScrolling(int position)
        {
            ScrollPosition = GetLineFromCharIndex(position);
        }

        internal void Scroll(int delta)
        {
            ScrollPosition += delta;
            SendMessage(Handle, EM_LINESCROLL, Zero, (IntPtr)delta);
        }

        internal void ScrollToTop()
        {
            SendMessage(Handle, EM_LINESCROLL, Zero, (IntPtr)(-100000));
            vScrollBar.SetValue(0);
            vScrollBar.Invalidate();
            Invalidate();
        }

        //internal bool ShowScrollbars //Doesn't work
        //{
        //    get { return dontUseSkinColors; }

        //    set
        //    {
        //        if (dontUseSkinColors != value)
        //        {
        //            dontUseSkinColors = value;
        //            if (IsHandleCreated)
        //                RecreateHandle();
        //        }
        //    }
        //}

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (vScrollBar?.Visible == true)
                vScrollBar.Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                vScrollBar?.Dispose();
            }
        }

        ~CustomTextBox()
        {
            Dispose(false);
        }
    }

    internal class CustomListBox : ListBox
    {
        private Color borderColorDisabled = Plugin.ScrollBarBorderColor;
        private Color borderColorActive = Plugin.ScrollBarFocusedBorderColor;
        private Color borderColor = Plugin.ScrollBarBorderColor;

        private int scaledPx = -10000;
        private int customScrollBarInitialWidth;

        private bool dontUseSkinColors;

        internal CustomHScrollBar hScrollBar;
        internal CustomVScrollBar vScrollBar;

        internal event EventHandler ItemsChanged;

        internal CustomListBox(bool dontUseSkinColors)
        {
            this.dontUseSkinColors = dontUseSkinColors;

            InitializeComponent();

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            //TODO: Add any initialization after the InitForm call
        }

        internal CustomListBox(int customScrollBarInitialWidth, int scaledPx, bool dontUseSkinColors) : this(false)
        {
            this.dontUseSkinColors = dontUseSkinColors;
            this.customScrollBarInitialWidth = customScrollBarInitialWidth;
            this.scaledPx = scaledPx;
        }

        internal CustomListBox(int customScrollBarInitialWidth, int scaledPx, bool dontUseSkinColors, Color borderColor, Color borderColorDisabled, Color borderColorActive)
            : this(customScrollBarInitialWidth, scaledPx, dontUseSkinColors)
        {
            this.borderColorDisabled = borderColorDisabled;
            this.borderColorActive = borderColorActive;
            this.borderColor = borderColor;
        }

        internal void SetBorderColors(Color borderColor, Color borderColorDisabled, Color borderColorActive)
        {
            this.borderColorDisabled = borderColorDisabled;
            this.borderColorActive = borderColorActive;
            this.borderColor = borderColor;
        }

        public new int GetItemHeight(int index)
        {
            return ItemHeight;
        }

        public override int ItemHeight
        {
            get { return this.FontHeight; }

            set { this.FontHeight = value; }
        }

        internal int GetItemsHeight()
        {
            return ItemHeight * Items.Count + 3 * scaledPx;
        }

        internal void AdjustHeight(int dropDownWidth, int initialDropDownHeight, int additionalColumnWidth)
        {
            if (scaledPx > 0)
            {
                bool scrollBarVisible;
                var itemsHeight = GetItemsHeight();

                if (itemsHeight > initialDropDownHeight)
                {
                    if ((Parent as TableLayoutPanel).ColumnStyles[2].Width != vScrollBar.Width) //-V3024
                        (Parent as TableLayoutPanel).ColumnStyles[2].Width = vScrollBar.Width;

                    if (MaximumSize.Height != initialDropDownHeight)
                        MaximumSize = new Size(MaximumSize.Width, initialDropDownHeight);

                    if (Height != initialDropDownHeight)
                    {
                        Height = initialDropDownHeight;
                        vScrollBar.ResetMetricsSize(initialDropDownHeight);
                    }

                    scrollBarVisible = true;
                }
                else
                {
                    if ((Parent as TableLayoutPanel).ColumnStyles[2].Width != 0) //-V3024
                        (Parent as TableLayoutPanel).ColumnStyles[2].Width = 0;

                    if (MaximumSize.Height != itemsHeight)
                        MaximumSize = new Size(MaximumSize.Width, itemsHeight);

                    if (Height != itemsHeight)
                        Height = itemsHeight;

                    scrollBarVisible = false;
                }

                vScrollBar.Visible = scrollBarVisible;
                AdjustWidth(dropDownWidth, scrollBarVisible, additionalColumnWidth);
            }
        }

        internal void AdjustAdditionalListHeight(int initialDropDownHeight)
        {
            if (scaledPx > 0)
            {
                var itemsHeight = GetItemsHeight();

                if (itemsHeight > initialDropDownHeight)
                {
                    if (MaximumSize.Height != initialDropDownHeight)
                        MaximumSize = new Size(MaximumSize.Width, initialDropDownHeight);

                    if (Height != initialDropDownHeight)
                        Height = initialDropDownHeight;
                }
                else
                {
                    if (MaximumSize.Height != itemsHeight)
                        MaximumSize = new Size(MaximumSize.Width, itemsHeight);

                    if (Height != itemsHeight)
                        Height = itemsHeight;
                }
            }
        }

        internal void AdjustWidth(int dropDownWidth, bool scrollBarVisible, int additionalColumnWidth)
        {
            if (scrollBarVisible)
            {
                MinimumSize = new Size(dropDownWidth - customScrollBarInitialWidth - additionalColumnWidth, 2);
                MaximumSize = new Size(dropDownWidth - customScrollBarInitialWidth - additionalColumnWidth, MaximumSize.Height);
            }
            else
            {
                MinimumSize = new Size(dropDownWidth - additionalColumnWidth, 2);
                MaximumSize = new Size(dropDownWidth - additionalColumnWidth, MaximumSize.Height);
            }


            if (Width != MinimumSize.Width)
                Width = MinimumSize.Width;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;

                if (!dontUseSkinColors)
                {
                    cp.ExStyle &= ~WS_EX_CLIENTEDGE;
                    cp.Style |= WS_BORDER;

                    cp.Style = cp.Style & ~WS_HSCROLL & ~WS_VSCROLL;
                }

                return cp;
            }
        }

        //internal bool ShowScrollbars //Doesn't work
        //{
        //    get { return dontUseSkinColors; }

        //    set
        //    {
        //        if (dontUseSkinColors != value)
        //        {
        //            dontUseSkinColors = value;
        //            if (IsHandleCreated)
        //                RecreateHandle();
        //        }
        //    }
        //}

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == LB_ADDSTRING ||
                m.Msg == LB_INSERTSTRING ||
                m.Msg == LB_DELETESTRING ||
                m.Msg == LB_RESETCONTENT)

                if (ItemsChanged != null)
                    ItemsChanged(this, EventArgs.Empty); //-V3083 //-V5605

            if (m.Msg == WM_NCPAINT && !dontUseSkinColors)
            {
                WmNcPaint(ref m);
                return;
            }

            base.WndProc(ref m);
        }

        private void DrawBorder(Graphics g)
        {
            if (!this.IsEnabled())
                ControlPaint.DrawBorder(g, new Rectangle(0, 0, Width, Height), borderColorDisabled, ButtonBorderStyle.Solid);
            else if (ContainsFocus)
                ControlPaint.DrawBorder(g, new Rectangle(0, 0, Width, Height), borderColorActive, ButtonBorderStyle.Solid);
            else
                ControlPaint.DrawBorder(g, new Rectangle(0, 0, Width, Height), borderColor, ButtonBorderStyle.Solid);
        }

        private void WmNcPaint(ref Message m)
        {
            IntPtr hDC = GetWindowDC(m.HWnd);
            if (hDC != IntPtr.Zero)
            {
                using (Graphics g = Graphics.FromHdc(hDC))
                    DrawBorder(g);

                m.Result = (IntPtr)1;

                ReleaseDC(m.HWnd, hDC);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawBorder(e.Graphics);

            if (hScrollBar?.Visible == true)
                hScrollBar.Invalidate();

            if (vScrollBar?.Visible == true)
                vScrollBar.Invalidate();
        }

        private void InitializeComponent()
        {
            //Nothing for now...
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                hScrollBar?.Dispose();
                vScrollBar?.Dispose();
            }
        }

        ~CustomListBox()
        {
            Dispose(false);
        }
    }

    internal class CustomCheckedListBox : CheckedListBox
    {
        private const float ItemScale = 1.08f;

        private readonly Color borderColorDisabled = Plugin.ScrollBarBorderColor;
        private readonly Color borderColorActive = Plugin.ScrollBarFocusedBorderColor;
        private readonly Color borderColor = Plugin.ScrollBarBorderColor;

        private readonly bool dontUseSkinColors;

        internal CustomHScrollBar hScrollBar;
        internal CustomVScrollBar vScrollBar;

        internal event EventHandler ItemsChanged;

        internal CustomCheckedListBox(bool dontUseSkinColors)
        {
            this.dontUseSkinColors = dontUseSkinColors;

            InitializeComponent();

            SetStyle(ControlStyles.ResizeRedraw, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //SetStyle(ControlStyles.DoubleBuffer, true);

            //TODO: Add any initialization after the InitForm call
        }

        internal CustomCheckedListBox(bool dontUseSkinColors, Color borderColor, Color borderColorDisabled, Color borderColorActive)
            : this(dontUseSkinColors)
        {
            this.borderColorDisabled = borderColorDisabled;
            this.borderColorActive = borderColorActive;
            this.borderColor = borderColor;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;

                if (!dontUseSkinColors)
                {
                    cp.ExStyle &= ~WS_EX_CLIENTEDGE;
                    cp.Style |= WS_BORDER;

                    cp.Style = cp.Style & ~WS_HSCROLL & ~WS_VSCROLL;
                }

                return cp;
            }
        }

        //internal bool ShowScrollbars //Doesn't work
        //{
        //    get { return dontUseSkinColors; }

        //    set
        //    {
        //        if (dontUseSkinColors != value)
        //        {
        //            dontUseSkinColors = value;
        //            if (IsHandleCreated)
        //                RecreateHandle();
        //        }
        //    }
        //}

        internal new int GetItemHeight(int index)
        {
            return ItemHeight;
        }

        public override int ItemHeight
        {
            get { return (int)Math.Round(this.FontHeight * ItemScale); }

            set { this.FontHeight = (int)Math.Round(value / ItemScale); }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (!dontUseSkinColors) //Skinned list box
                (this.FindForm() as PluginWindowTemplate).listBox_DrawItem(this, e);
            else //Not skinned list box. Call the original OnDrawItem, using original colors
                base.OnDrawItem(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == LB_ADDSTRING ||
                m.Msg == LB_INSERTSTRING ||
                m.Msg == LB_DELETESTRING ||
                m.Msg == LB_RESETCONTENT)

                if (ItemsChanged != null)
                    ItemsChanged(this, EventArgs.Empty); //-V3083 //-V5605

            if (m.Msg == WM_NCPAINT && !dontUseSkinColors)
            {
                WmNcPaint(ref m);
                return;
            }

            base.WndProc(ref m);
        }

        private void DrawBorder(Graphics g)
        {
            if (!this.IsEnabled())
                ControlPaint.DrawBorder(g, new Rectangle(0, 0, Width, Height), borderColorDisabled, ButtonBorderStyle.Solid);
            else if (ContainsFocus)
                ControlPaint.DrawBorder(g, new Rectangle(0, 0, Width, Height), borderColorActive, ButtonBorderStyle.Solid);
            else
                ControlPaint.DrawBorder(g, new Rectangle(0, 0, Width, Height), borderColor, ButtonBorderStyle.Solid);
        }

        private void WmNcPaint(ref Message m)
        {
            IntPtr hDC = GetWindowDC(m.HWnd);
            if (hDC != IntPtr.Zero)
            {
                using (Graphics g = Graphics.FromHdc(hDC))
                    DrawBorder(g);

                m.Result = (IntPtr)1;

                ReleaseDC(m.HWnd, hDC);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawBorder(e.Graphics);

            if (hScrollBar?.Visible == true)
                hScrollBar.Invalidate();

            if (vScrollBar?.Visible == true)
                vScrollBar.Invalidate();
        }

        private void InitializeComponent()
        {
            //Nothing for now...
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                hScrollBar?.Dispose();
                vScrollBar?.Dispose();
            }
        }

        ~CustomCheckedListBox()
        {
            Dispose(false);
        }
    }


    //Returns Minimum, Maximum, SmallChange, LargeChange
    internal delegate (int, int, int, int) GetScrollBarMetricsDelegate(Control scrolledControl, Control refScrollBar);

    //Custom scroll bars
    internal sealed class CustomVScrollBar : UserControl
    {
        private readonly int initialWidth;
        private int initialHeight;
        private readonly int sbBorderWidth;
        private readonly int reservedBordersSpace;
        private int reservedSpace;
        private readonly int minimumThumbHeight = 5; //5px, will be scaled according to DPI in constructor
        private readonly int scaledPx;
        private readonly int upImageAdditionalTopHeight = 3; //3px, will be scaled according to DPI in constructor
        private readonly int upImageAdditionalBottomHeight = 4; //4px, will be scaled according to DPI in constructor
        private int offsetX;

        private System.Threading.Timer thumbUpDownRepeater;

        internal bool SettingParentScroll;

        internal Control ScrolledControl;

        private Control refScrollBar;
        private readonly GetScrollBarMetricsDelegate GetExternalMetrics;

        //private Image moUpArrowImage_Over;
        //private Image moUpArrowImage_Down;
        //private Image moDownArrowImage_Over;
        //private Image moDownArrowImage_Down;
        //private Image thumbArrowImage;

        private readonly Color scrollBarBackColor;
        private readonly Color scrollBarBorderColor;
        private readonly Color narrowScrollBarBackColor;

        private readonly Color scrollBarThumbAndSpansForeColor;
        private readonly Color scrollBarThumbAndSpansBackColor;
        private readonly Color scrollBarThumbAndSpansBorderColor;

        private Brush scrollBarBackBrush;
        private Brush scrollBarBorderBrush;
        private Brush narrowScrollBarBackBrush;
        private Brush scrollBarThumbAndSpansBackBrush;
        private Brush scrollBarThumbAndSpansBorderBrush;

        private int upArrowImageHeight;
        private int upArrowImageWidth;

        private int largeChange = 50;
        private int smallChange = 5;
        private int minimum;
        private int maximum = 100;
        private int value;
        private int clickPoint;

        private int thumbTop;

        private bool thumbDown;
        private bool thumbDragging;

        private bool upArrowDown;
        private bool downArrowDown;

        internal event EventHandler ValueChanged;

        internal void CreateBrushes()
        {
            if (scrollBarBackColor != Plugin.NoColor)
                scrollBarBackBrush = new SolidBrush(scrollBarBackColor);

            if (scrollBarBorderColor != Plugin.NoColor)
                scrollBarBorderBrush = new SolidBrush(scrollBarBorderColor);

            if (narrowScrollBarBackColor != Plugin.NoColor)
                narrowScrollBarBackBrush = new SolidBrush(narrowScrollBarBackColor);

            if (scrollBarThumbAndSpansBackColor != Plugin.NoColor)
                scrollBarThumbAndSpansBackBrush = new SolidBrush(scrollBarThumbAndSpansBackColor);

            if (scrollBarThumbAndSpansBorderColor != Plugin.NoColor)
                scrollBarThumbAndSpansBorderBrush = new SolidBrush(scrollBarThumbAndSpansBorderColor);
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                scrollBarBackBrush?.Dispose();
                scrollBarBorderBrush?.Dispose();
                narrowScrollBarBackBrush?.Dispose();
                scrollBarThumbAndSpansBackBrush?.Dispose();
                scrollBarThumbAndSpansBorderBrush?.Dispose();

                UpArrowImage?.Dispose();
                DownArrowImage?.Dispose();

                ThumbTopImage?.Dispose();
                ThumbMiddleImage?.Dispose();
                ThumbBottomImage?.Dispose();

            }
        }

        ~CustomVScrollBar()
        {
            Dispose(false);
        }


        //refScrollBar must be null if scrolledControl is not a parent of custom scroll bar & scroll bar must be placed on the right to scrolledControl
        internal CustomVScrollBar(Form ownerForm, Control scrolledControl, int smallChange,
            GetScrollBarMetricsDelegate getScrollBarMetrics, int borderWidth, Control refScrollBar = null, bool useSelfAsRefScrollBar = false)
        {
            InitializeComponent();

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //SetStyle(ControlStyles.DoubleBuffer, true);


            scrollBarBackColor = Plugin.ScrollBarBackColor;
            scrollBarBorderColor = Plugin.ScrollBarBorderColor;
            narrowScrollBarBackColor = Plugin.NarrowScrollBarBackColor;

            scrollBarThumbAndSpansForeColor = Plugin.ScrollBarThumbAndSpansForeColor;
            scrollBarThumbAndSpansBackColor = Plugin.ScrollBarThumbAndSpansBackColor;
            scrollBarThumbAndSpansBorderColor = Plugin.ScrollBarThumbAndSpansBorderColor;


            UpArrowImage = Plugin.CopyBitmap(Plugin.UpArrowImage);
            DownArrowImage = Plugin.CopyBitmap(Plugin.DownArrowImage);

            ThumbTopImage = Plugin.CopyBitmap(Plugin.ThumbTopImage);
            ThumbMiddleImage = Plugin.CopyBitmap(Plugin.ThumbMiddleVerticalImage);
            ThumbBottomImage = Plugin.CopyBitmap(Plugin.ThumbBottomImage);

            upArrowImageHeight = UpArrowImage.Height; //-V3080
            upArrowImageWidth = UpArrowImage.Width;

            this.smallChange = smallChange;

            GetExternalMetrics = getScrollBarMetrics;
            ScrolledControl = scrolledControl;

            if (borderWidth == -1)
            {
                sbBorderWidth = Plugin.SbBorderWidth;
            }
            else
            {
                sbBorderWidth = borderWidth;
                scrollBarBorderColor = Plugin.ScrollBarBackColor;
            }

            var dpiScaling = (ownerForm as PluginWindowTemplate).dpiScaling;
            scaledPx = (ownerForm as PluginWindowTemplate).scaledPx;

            if (ThumbTopImage == null)
                minimumThumbHeight = (int)Math.Round(minimumThumbHeight * dpiScaling);
            else
                minimumThumbHeight = ThumbTopImage.Height * 2;

            upImageAdditionalTopHeight = (int)Math.Round(dpiScaling * upImageAdditionalTopHeight);
            upImageAdditionalBottomHeight = (int)Math.Round(dpiScaling * upImageAdditionalBottomHeight);

            if (refScrollBar != null || useSelfAsRefScrollBar) //scrolledControlIsParent has own scroll bar CONTROLS, which must be replaced
            {
                if (useSelfAsRefScrollBar)
                    this.refScrollBar = this;
                else
                    this.refScrollBar = refScrollBar;

                Visible = false;

                reservedBordersSpace = 2 * scaledPx;

                initialWidth = ControlsTools.GetCustomScrollBarInitialWidth(dpiScaling, sbBorderWidth);

                if (refScrollBar != null)
                    Width = refScrollBar.Width;
                else
                    Width = initialWidth;

                ResetMetricsSize(ScrolledControl.Height);
                AdjustReservedSpace(0);

                Margin = Padding.Empty;

                ScrolledControl.Controls.Add(this);

                offsetX = Width - initialWidth;

                if (refScrollBar != null && !useSelfAsRefScrollBar)
                    refScrollBar.Visible = false;

                Top = scaledPx;
                Left = ScrolledControl.Width - Width - scaledPx;
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            }
            else //custom scroll bars will be placed on the right to/below scrolledControlIsParent in the tableLayoutPanel
            {
                this.refScrollBar = this;
                Visible = true;

                reservedBordersSpace = 0;

                initialWidth = ControlsTools.GetCustomScrollBarInitialWidth(dpiScaling, sbBorderWidth);
                Width = initialWidth;

                ResetMetricsSize(ScrolledControl.Height);
                AdjustReservedSpace(0);


                TableLayoutPanel tableLayoutPanel;

                if (scrolledControl.Parent is TableLayoutPanel tableLayoutPanel1)
                    tableLayoutPanel = tableLayoutPanel1;
                else
                    tableLayoutPanel = ScrolledControl.Parent.Parent as TableLayoutPanel;


                if (ScrolledControl is CustomListBox box)
                    box.vScrollBar = this;
                if (ScrolledControl is CustomCheckedListBox listBox)
                    listBox.vScrollBar = this;
                else if (ScrolledControl is CustomTextBox textBox)
                    textBox.vScrollBar = this;

                var position = tableLayoutPanel.GetCellPosition(ScrolledControl); //-V3149
                tableLayoutPanel.ColumnStyles[position.Column + 1].Width = Width;
                tableLayoutPanel.RowStyles[position.Row + 1].Height = 0;

                Margin = Padding.Empty;
                Anchor = AnchorStyles.Top | AnchorStyles.Left;

                tableLayoutPanel.Controls.Add(this, position.Column + 1, position.Row); //At 2nd column, 1st row

                Dock = DockStyle.Fill;
            }

            Value = 0;

            SizeChanged += customScrollBar_SizeChanged;
        }

        internal void ResetMetricsSize(int newParentHeight)
        {
            initialHeight = newParentHeight - reservedBordersSpace;
            Height = initialHeight - reservedSpace;


            var (nRealRange, nPixelRange, nTrackHeight, nThumbHeight, fThumbHeight) = GetMetrics();


            MinimumSize = new Size(upArrowImageWidth + 2 * sbBorderWidth,
                2 * (upArrowImageHeight + upImageAdditionalTopHeight
                + upImageAdditionalBottomHeight + sbBorderWidth) + 4 * scaledPx);


            var fValuePerc = ((float)value - minimum) / (maximum - minimum);
            (minimum, maximum, smallChange, largeChange) = GetExternalMetrics(ScrolledControl, refScrollBar);
            value = (int)Math.Round(fValuePerc * (maximum - minimum)) + minimum;
            if (value < minimum)
                value = minimum;
            else if (value > maximum)
                value = maximum;

            //figure out thumb top
            SetThumbTopFromValueAndLargeChange();

            if (ValueChanged != null)
                ValueChanged(this, null); //-V3083 //-V5605

            Invalidate();
        }

        internal void AdjustReservedSpace(int space)
        {
            //reservedSpace = space; //---
            //Height = initialHeight - space;

            //Invalidate();
        }

        private void customScrollBar_SizeChanged(object sender, EventArgs e)
        {
            offsetX = Width - initialWidth;
            Invalidate();
        }

        internal int LargeChange
        {
            get
            {
                return largeChange;
            }

            set
            {
                largeChange = value;

                //figure out thumb top
                SetThumbTopFromValueAndLargeChange();

                if (ValueChanged != null)
                    ValueChanged(this, null);

                Invalidate();
            }
        }

        internal int SmallChange
        {
            get
            {
                return smallChange;
            }

            set
            {
                smallChange = value;
                Invalidate();
            }
        }

        internal int Minimum
        {
            get
            {
                return minimum;
            }

            set
            {
                minimum = value;

                //figure out thumb top
                SetThumbTopFromValueAndLargeChange();

                if (ValueChanged != null)
                    ValueChanged(this, null);

                Invalidate();
            }
        }

        internal int Maximum
        {
            get
            {
                return maximum;
            }

            set
            {
                maximum = value;

                //figure out thumb top
                SetThumbTopFromValueAndLargeChange();

                if (ValueChanged != null)
                    ValueChanged(this, null);

                Invalidate();
            }
        }

        internal (int, int, int, int, float) GetMetrics()
        {
            var nTrackHeight = Height - 2 * (upArrowImageHeight
                                             + upImageAdditionalTopHeight + upImageAdditionalBottomHeight)
                                      - 2 * sbBorderWidth; //-1 scaled px of top scroll bar border & +1 of bottom scroll bar border
            var fThumbHeight = (float)largeChange / (maximum - minimum) * nTrackHeight;
            var nThumbHeight = (int)Math.Round(fThumbHeight);

            if (nThumbHeight > nTrackHeight)
            {
                nThumbHeight = nTrackHeight;
                fThumbHeight = nTrackHeight;
            }

            if (nThumbHeight < minimumThumbHeight)
            {
                nThumbHeight = minimumThumbHeight;
                fThumbHeight = minimumThumbHeight;
            }

            var nRealRange = maximum - minimum - largeChange;
            var nPixelRange = nTrackHeight - nThumbHeight;

            return (nRealRange, nPixelRange, nTrackHeight, nThumbHeight, fThumbHeight);
        }

        internal int GetThumbTop()
        {
            return thumbTop;
        }

        internal void SetThumbTop(int thumbTop)
        {
            if (thumbTop == this.thumbTop)
                return;

            var (_, nPixelRange, _, _, _) = GetMetrics();

            if (thumbTop > nPixelRange)
                this.thumbTop = nPixelRange;
            else if (thumbTop < 0)
                this.thumbTop = 0;
            else
                this.thumbTop = thumbTop;


            //figure out value
            var fPerc = (float)this.thumbTop / nPixelRange;
            var fValue = fPerc * (maximum - minimum);

            value = (int)Math.Round(fValue);

            Invalidate();
        }

        internal void SetThumbTopFromValueAndLargeChange()
        {
            var (_, nPixelRange, _, _, _) = GetMetrics();

            var fPerc = 0.0f;
            if (maximum - minimum != 0)
                fPerc = (float)value / (maximum - minimum);

            var fTop = fPerc * nPixelRange;
            thumbTop = (int)Math.Round(fTop);

            Invalidate();
        }

        internal void SetValue(int value)
        {
            if (SettingParentScroll)
            {
                SettingParentScroll = false;
                return;
            }

            if (value == this.value)
                return;
            else if (value < minimum)
                this.value = minimum;
            else if (value > maximum)
                this.value = maximum;
            else
                this.value = value;

            //figure out thumb top
            SetThumbTopFromValueAndLargeChange();
        }

        internal int Value
        {
            get
            {
                return value;
            }

            set
            {
                SetValue(value);

                if (ValueChanged != null)
                    ValueChanged(this, null);
            }
        }

        internal Image UpArrowImage { get; set; }

        internal Image DownArrowImage { get; set; }

        internal Image ThumbTopImage { get; set; }

        internal Image ThumbMiddleImage { get; set; }

        internal Image ThumbBottomImage { get; set; }

        private void OnPaint1(PaintEventArgs e, Image upArrowImageArg, Image downArrowImageArg,
            Image thumbTopImageArg, Image thumbMiddleImageArg, Image thumbBottomImageArg, bool stretchThumbImage)
        {
            if (stretchThumbImage && thumbMiddleImageArg == null)
                throw new Exception(Plugin.ExImpossibleToStretchThumb);


            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //let's calculate thumb metrics
            var (_, _, _, nThumbHeight, fThumbHeight) = GetMetrics();

            //fSpanHeight: +1 scaled px TOP & BOTTOM scroll bar border: total 2 scaled px per BOTH spans
            float fSpanHeight;
            var thumbTopBottomImageHeight = 0;
            if (thumbMiddleImageArg == null)
            {
                fSpanHeight = fThumbHeight / 2.0f;
            }
            else if (stretchThumbImage && thumbTopImageArg == null)
            {
                fSpanHeight = fThumbHeight / 2.0f;
            }
            else if (stretchThumbImage)
            {
                thumbTopBottomImageHeight = thumbTopImageArg.Height; //-V3125 //-V3095
                fSpanHeight = fThumbHeight / 2.0f;
            }
            else //let's draw thumb middle at the vertical center of scroll bar (it's supposed that solid color spans are drawn in this case)
            {
                fSpanHeight = fThumbHeight - thumbMiddleImageArg.Height / 2.0f;
            }

            var nSpanHeight = (int)Math.Round(fSpanHeight);


            //draw native scroll bar left/right & top/bottom borders, scroll bar background will be drawn over it later
            if (sbBorderWidth > 0 && scrollBarBorderBrush != null)
                e.Graphics.FillRectangle(scrollBarBorderBrush, new Rectangle(0, 0, Width, Height)); //+1 scaled px shift to height

            //draw native scroll bar background
            //+1 scaled px to top shift below due to scroll bar top border,
            //-2 scaled px of size for scroll bar top/bottom & left/right borders
            e.Graphics.FillRectangle(scrollBarBackBrush, new Rectangle(sbBorderWidth, sbBorderWidth,
                Width - 2 * sbBorderWidth, Height - 2 * sbBorderWidth));

            //draw narrow scroll bar background
            //+1 scaled px to top shift below due to scroll bar top border,
            //-2 scaled px of size for scroll bar top/bottom & left/right borders
            if (narrowScrollBarBackBrush != null && Width != initialWidth)
                e.Graphics.FillRectangle(narrowScrollBarBackBrush, new Rectangle(sbBorderWidth + offsetX, sbBorderWidth,
                    initialWidth - 2 * sbBorderWidth, Height - 2 * sbBorderWidth));


            //draw top image background
            if (scrollBarThumbAndSpansBackBrush != null)
                e.Graphics.FillRectangle(scrollBarThumbAndSpansBackBrush, new Rectangle(sbBorderWidth + offsetX,
                    sbBorderWidth,
                    initialWidth - 2 * sbBorderWidth, upArrowImageArg.Height
                    + upImageAdditionalTopHeight + upImageAdditionalBottomHeight));

            //draw top image
            //+1 scaled px nTop offset due to scroll bar top border
            //-2 scaled px of width for scroll bar left/right borders
            e.Graphics.DrawImage(upArrowImageArg, new Rectangle(new Point(offsetX + (initialWidth - upArrowImageArg.Width) / 2,
                sbBorderWidth + upImageAdditionalTopHeight),
                new Size(upArrowImageArg.Width, upArrowImageArg.Height)));

            var nTop = thumbTop;
            nTop += upArrowImageArg.Height + sbBorderWidth //+1 scaled px to top shift below due to scroll bar top border
                + upImageAdditionalTopHeight + upImageAdditionalBottomHeight;


            //draw spans & thumb left/right & top/bottom borders, spans & thumb background will be drawn over it later
            if (scrollBarThumbAndSpansBorderBrush != null)
                e.Graphics.FillRectangle(scrollBarThumbAndSpansBorderBrush, new Rectangle(sbBorderWidth + offsetX, nTop,
                initialWidth - 2 * sbBorderWidth, nThumbHeight));

            //draw spans & thumb background, thumb image(s) will be drawn over it later
            if (scrollBarThumbAndSpansBackBrush != null)
                e.Graphics.FillRectangle(scrollBarThumbAndSpansBackBrush, new Rectangle(scaledPx + sbBorderWidth + offsetX,
                    nTop + sbBorderWidth,
                    initialWidth - 2 * sbBorderWidth - scaledPx, nThumbHeight - sbBorderWidth));

            //draw thumb
            if (thumbMiddleImageArg != null)
            {
                if (stretchThumbImage)
                {
                    if (thumbTopImageArg != null)
                    {
                        e.Graphics.DrawImage(thumbTopImageArg, new Rectangle(offsetX + (initialWidth - thumbTopImageArg.Width) / 2, nTop,
                            thumbTopImageArg.Width, thumbTopImageArg.Height));
                    }

                    nTop += thumbTopBottomImageHeight;

                    Plugin.DrawRepeatedImage(e.Graphics, thumbMiddleImageArg, offsetX
                        + (int)Math.Round((initialWidth - 1.375d * thumbMiddleImageArg.Width) / 2), nTop,
                        thumbMiddleImageArg.Width, nThumbHeight - 2 * thumbTopBottomImageHeight, true, false);

                    nTop += nThumbHeight - 2 * thumbTopBottomImageHeight;

                    if (thumbTopImageArg != null) //Both thumbTopImageArg & thumbBottomImageArg must be null if thumbTopImageArg is null
                    {
                        e.Graphics.DrawImage(thumbBottomImageArg, new Rectangle(offsetX + (initialWidth - thumbBottomImageArg.Width) / 2, nTop,
                        thumbBottomImageArg.Width, thumbBottomImageArg.Height));
                    }
                }
                else
                {
                    nTop += nSpanHeight;

                    e.Graphics.DrawImage(thumbMiddleImageArg, new Rectangle(offsetX
                        + (int)Math.Round((initialWidth - 1.375d * thumbMiddleImageArg.Width) / 2), nTop,
                        thumbMiddleImageArg.Width, thumbMiddleImageArg.Height));
                }
            }


            //draw bottom image background
            if (scrollBarThumbAndSpansBackBrush != null)
                e.Graphics.FillRectangle(scrollBarThumbAndSpansBackBrush, new Rectangle(sbBorderWidth + offsetX,
                    Height - downArrowImageArg.Height
                    - upImageAdditionalTopHeight - upImageAdditionalBottomHeight - sbBorderWidth,
                    initialWidth - 2 * sbBorderWidth, downArrowImageArg.Height
                    + upImageAdditionalTopHeight + upImageAdditionalBottomHeight));

            //draw bottom image
            //-1 scaled px vertical offset due to scroll bar bottom border
            //-2 scaled px of width for scroll bar left/right borders
            e.Graphics.DrawImage(downArrowImageArg, new Rectangle(new Point(offsetX + (initialWidth - downArrowImageArg.Width) / 2,
                Height - downArrowImageArg.Height
                - upImageAdditionalTopHeight - sbBorderWidth),
                new Size(downArrowImageArg.Width, downArrowImageArg.Height)));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Below are some skinning versions

            //Version 1:
            //OnPaint1(e, _upArrowImage, _downArrowImage, null, _thumbMiddleImage, null, false);
            //Version 2:
            //OnPaint1(e, _upArrowImage, _downArrowImage, null, null, null, false);
            //Version 3:
            //OnPaint1(e, _upArrowImage, _downArrowImage, _thumbTopImage, _thumbMiddleImage, _thumbBottomImage, true);
            //Version 4:
            OnPaint1(e, UpArrowImage, DownArrowImage, null, ThumbMiddleImage, null, true);
        }

        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }

            set
            {
                base.AutoSize = value;
                if (base.AutoSize)
                    Width = initialWidth;
            }
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            //
            //CustomScrollBar
            //
            MouseDown += CustomScrollBar_MouseDown;
            MouseMove += CustomScrollBar_MouseMove;
            MouseUp += CustomScrollBar_MouseUp;
            ResumeLayout(false);
        }

        private void moveThumbUpDown(object state)
        {
            if (!downArrowDown && !upArrowDown)
                return;


            var (nRealRange, nPixelRange, nTrackHeight, nThumbHeight, fThumbHeight) = GetMetrics();

            if (upArrowDown)
            {
                if (nRealRange > 0)
                {
                    if (nPixelRange > 0)
                    {
                        if (thumbTop - smallChange < 0)
                            thumbTop = 0;
                        else
                            thumbTop -= smallChange;

                        //figure out value
                        var fPerc = (float)thumbTop / nPixelRange;
                        var fValue = fPerc * (maximum - minimum);

                        value = (int)Math.Round(fValue);
                    }
                }
            }
            else if (downArrowDown)
            {
                if (nRealRange > 0)
                {
                    if (nPixelRange > 0)
                    {
                        if (thumbTop + smallChange > nPixelRange)
                            thumbTop = nPixelRange;
                        else
                            thumbTop += smallChange;

                        //figure out value
                        var fPerc = (float)thumbTop / nPixelRange;
                        var fValue = fPerc * (maximum - minimum);

                        value = (int)Math.Round(fValue);
                    }
                }
            }

            Plugin.MbForm.Invoke(new Action(() =>
            {
                if (ValueChanged != null)
                    ValueChanged(this, null);
            }));
        }

        private void CustomScrollBar_MouseDown(object sender, MouseEventArgs e)
        {
            var ptPoint = PointToClient(Cursor.Position);

            var (nRealRange, nPixelRange, nTrackHeight, nThumbHeight, fThumbHeight) = GetMetrics();

            var nTop = thumbTop;
            nTop += upArrowImageHeight + upImageAdditionalTopHeight + upImageAdditionalBottomHeight
                + sbBorderWidth; //+1 scaled px to top shift below due to top border


            var thumbRect = new Rectangle(new Point(sbBorderWidth + offsetX, nTop),
                new Size(initialWidth - 2 * sbBorderWidth, nThumbHeight));

            var uparrowRect = new Rectangle(new Point(sbBorderWidth + offsetX, sbBorderWidth),
                new Size(initialWidth - 2 * sbBorderWidth, (upArrowImageHeight
                + upImageAdditionalTopHeight + upImageAdditionalBottomHeight)));

            var downarrowRect = new Rectangle(new Point(sbBorderWidth + offsetX, upArrowImageHeight
                + upImageAdditionalTopHeight + upImageAdditionalBottomHeight + sbBorderWidth + nTrackHeight),
                new Size(initialWidth - 2 * sbBorderWidth, upArrowImageHeight
                + upImageAdditionalTopHeight + upImageAdditionalBottomHeight));

            upArrowDown = false;
            downArrowDown = false;

            if (thumbRect.Contains(ptPoint))
            {
                //hit the thumb
                clickPoint = (ptPoint.Y - nTop);
                thumbDown = true;
            }
            else if (uparrowRect.Contains(ptPoint))
            {
                upArrowDown = true;
                thumbUpDownRepeater = new System.Threading.Timer(moveThumbUpDown, null, 0, Plugin.ThumbMoveRepeatInterval);
            }
            else if (downarrowRect.Contains(ptPoint))
            {
                downArrowDown = true;
                thumbUpDownRepeater = new System.Threading.Timer(moveThumbUpDown, null, 0, Plugin.ThumbMoveRepeatInterval);
            }
            else if (thumbRect.Top > ptPoint.Y && thumbRect.Left < ptPoint.X && thumbRect.Left + thumbRect.Width > ptPoint.X)
            {
                float fScrollDistance = Height - 2 * (upArrowImageHeight
                    + upImageAdditionalTopHeight + upImageAdditionalBottomHeight + sbBorderWidth);

                var fScrollPerc = 0.0f;
                if (fScrollDistance != 0) //-V3024
                    fScrollPerc = ((float)ptPoint.Y - (upArrowImageHeight
                        + upImageAdditionalTopHeight + upImageAdditionalBottomHeight + sbBorderWidth)) / fScrollDistance;

                if (fScrollPerc < 0)
                    fScrollPerc = 0;

                //figure out value
                var fValue = minimum + fScrollPerc * (maximum - minimum);
                value = (int)Math.Round(fValue);

                //figure out thumb top
                SetThumbTopFromValueAndLargeChange();

                if (ValueChanged != null)
                    ValueChanged(this, null);
            }
            else if (thumbRect.Top + thumbRect.Height < ptPoint.Y && thumbRect.Left < ptPoint.X && thumbRect.Left + thumbRect.Width > ptPoint.X)
            {
                float fScrollDistance = Height - 2 * (upArrowImageHeight
                    + upImageAdditionalTopHeight + upImageAdditionalBottomHeight + sbBorderWidth);

                var fScrollPerc = 0.0f;
                if (fScrollDistance != 0) //-V3024
                    fScrollPerc = 1f - ((float)Height - 2 * (upArrowImageHeight
                        + upImageAdditionalTopHeight + upImageAdditionalBottomHeight + sbBorderWidth) - ptPoint.Y) / fScrollDistance;

                if (fScrollPerc > 1)
                    fScrollPerc = 1;

                //figure out value
                var fValue = minimum + fScrollPerc * (maximum - minimum);
                value = (int)Math.Round(fValue);

                //figure out thumb top
                SetThumbTopFromValueAndLargeChange();

                if (ValueChanged != null)
                    ValueChanged(this, null);
            }
        }

        private void CustomScrollBar_MouseUp(object sender, MouseEventArgs e)
        {
            thumbDown = false;
            thumbDragging = false;
            upArrowDown = false;
            downArrowDown = false;

            thumbUpDownRepeater?.Dispose();
            thumbUpDownRepeater = null;
        }

        private void MoveThumb(int y)
        {
            var (nRealRange, nPixelRange, _, _, _) = GetMetrics();

            var nSpot = clickPoint;

            if (thumbDown && nRealRange > 0)
            {
                if (nPixelRange > 0)
                {
                    var nNewThumbTop = y - (upArrowImageHeight
                                            + upImageAdditionalTopHeight + upImageAdditionalBottomHeight + sbBorderWidth + nSpot);

                    if (nNewThumbTop < 0)
                        thumbTop = 0;
                    else if (nNewThumbTop > nPixelRange)
                        thumbTop = nPixelRange;
                    else
                        thumbTop = y - (upArrowImageHeight
                            + upImageAdditionalTopHeight + upImageAdditionalBottomHeight + sbBorderWidth + nSpot);

                    //figure out value
                    var fPerc = (float)thumbTop / nPixelRange;
                    var fValue = fPerc * (maximum - minimum);
                    value = (int)Math.Round(fValue);
                }
            }
        }

        private void CustomScrollBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (thumbDown)
                thumbDragging = true;

            if (thumbDragging)
            {
                MoveThumb(e.Y);

                if (ValueChanged != null)
                    ValueChanged(this, null);

                Invalidate();
            }
        }
    }

    internal sealed class CustomHScrollBar : UserControl
    {
        private int initialWidth;
        private readonly int initialHeight;
        private readonly int sbBorderWidth;
        private readonly int reservedBordersSpace;
        private int reservedSpace;
        private readonly int minimumThumbWidth = 5; //5px, will be scaled according to DPI in constructor
        private readonly int scaledPx;
        private readonly int leftImageAdditionalLeftWidth = 3; //3px, will be scaled according to DPI in constructor
        private readonly int leftImageAdditionalRightWidth = 4; //4px, will be scaled according to DPI in constructor
        private int offsetY;

        private System.Threading.Timer thumbLeftRightRepeater;

        internal bool SettingParentScroll;

        internal Control ScrolledControl;

        private Control refScrollBar;
        private readonly GetScrollBarMetricsDelegate GetExternalMetrics;

        //private Image moLeftArrowImage_Over;
        //private Image moLeftArrowImage_Right;
        //private Image moRightArrowImage_Over;
        //private Image moRightArrowImage_Right;
        //private Image thumbArrowImage;

        private readonly Color scrollBarBackColor;
        private readonly Color scrollBarBorderColor;
        private readonly Color narrowScrollBarBackColor;

        private readonly Color scrollBarThumbAndSpansForeColor;
        private readonly Color scrollBarThumbAndSpansBackColor;
        private readonly Color scrollBarThumbAndSpansBorderColor;

        private Brush scrollBarBackBrush;
        private Brush scrollBarBorderBrush;
        private Brush narrowScrollBarBackBrush;
        private Brush scrollBarThumbAndSpansBackBrush;
        private Brush scrollBarThumbAndSpansBorderBrush;

        private int leftArrowImageHeight;
        private int leftArrowImageWidth;

        private int largeChange = 50;
        private int smallChange = 20;
        private int minimum;
        private int maximum = 100;
        private int value;
        private int clickPoint;

        private int thumbLeft;

        private bool thumbDown;
        private bool thumbDragging;

        private bool leftArrowDown;
        private bool rightArrowDown;

        internal event EventHandler ValueChanged;

        internal void CreateBrushes()
        {
            if (scrollBarBackColor != Plugin.NoColor)
                scrollBarBackBrush = new SolidBrush(scrollBarBackColor);

            if (scrollBarBorderColor != Plugin.NoColor)
                scrollBarBorderBrush = new SolidBrush(scrollBarBorderColor);

            if (narrowScrollBarBackColor != Plugin.NoColor)
                narrowScrollBarBackBrush = new SolidBrush(narrowScrollBarBackColor);

            if (scrollBarThumbAndSpansBackColor != Plugin.NoColor)
                scrollBarThumbAndSpansBackBrush = new SolidBrush(scrollBarThumbAndSpansBackColor);

            if (scrollBarThumbAndSpansBorderColor != Plugin.NoColor)
                scrollBarThumbAndSpansBorderBrush = new SolidBrush(scrollBarThumbAndSpansBorderColor);
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                scrollBarBackBrush?.Dispose();
                scrollBarBorderBrush?.Dispose();
                narrowScrollBarBackBrush?.Dispose();
                scrollBarThumbAndSpansBackBrush?.Dispose();
                scrollBarThumbAndSpansBorderBrush?.Dispose();

                LeftArrowImage?.Dispose();
                RightArrowImage?.Dispose();

                ThumbLeftImage?.Dispose();
                ThumbMiddleImage?.Dispose();
                ThumbRightImage?.Dispose();
            }
        }

        ~CustomHScrollBar()
        {
            Dispose(false);
        }

        //refScrollBar must be null if scrolledControl is not a parent of custom scroll bar & scroll bar must be placed below scrolledControl
        internal CustomHScrollBar(Form ownerForm, Control scrolledControl,
            GetScrollBarMetricsDelegate getScrollBarMetrics, Control refScrollBar = null, bool useSelfAsRefScrollBar = false)
        {
            InitializeComponent();

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //SetStyle(ControlStyles.DoubleBuffer, true);


            scrollBarBackColor = Plugin.ScrollBarBackColor;
            scrollBarBorderColor = Plugin.ScrollBarBorderColor;
            narrowScrollBarBackColor = Plugin.NarrowScrollBarBackColor;

            scrollBarThumbAndSpansForeColor = Plugin.ScrollBarThumbAndSpansForeColor;
            scrollBarThumbAndSpansBackColor = Plugin.ScrollBarThumbAndSpansBackColor;
            scrollBarThumbAndSpansBorderColor = Plugin.ScrollBarThumbAndSpansBorderColor;


            LeftArrowImage = Plugin.CopyBitmap(Plugin.LeftArrowImage);
            RightArrowImage = Plugin.CopyBitmap(Plugin.RightArrowImage);

            ThumbLeftImage = Plugin.CopyBitmap(Plugin.ThumbLeftImage);
            ThumbMiddleImage = Plugin.CopyBitmap(Plugin.ThumbMiddleHorizontalImage);
            ThumbRightImage = Plugin.CopyBitmap(Plugin.ThumbRightImage);

            leftArrowImageHeight = LeftArrowImage.Height; //-V3080
            leftArrowImageWidth = LeftArrowImage.Width;

            GetExternalMetrics = getScrollBarMetrics;
            ScrolledControl = scrolledControl;

            var dpiScaling = (ownerForm as PluginWindowTemplate).dpiScaling;
            scaledPx = (ownerForm as PluginWindowTemplate).scaledPx;

            if (ThumbLeftImage == null)
                minimumThumbWidth = (int)Math.Round(minimumThumbWidth * dpiScaling);
            else
                minimumThumbWidth = ThumbLeftImage.Width * 2;

            leftImageAdditionalLeftWidth = (int)Math.Round(dpiScaling * leftImageAdditionalLeftWidth);
            leftImageAdditionalRightWidth = (int)Math.Round(dpiScaling * leftImageAdditionalRightWidth);

            if (refScrollBar != null || useSelfAsRefScrollBar) //scrolledControlIsParent has own scroll bar CONTROLS, which must be replaced
            {
                if (useSelfAsRefScrollBar)
                    this.refScrollBar = this;
                else
                    this.refScrollBar = refScrollBar;

                Visible = false;

                sbBorderWidth = 0;
                reservedBordersSpace = 2 * scaledPx;

                initialHeight = ControlsTools.GetCustomScrollBarInitialWidth(dpiScaling, sbBorderWidth);

                if (refScrollBar != null)
                    Height = refScrollBar.Height;
                else
                    Width = initialHeight;

                ResetMetricsSize(ScrolledControl.Width);
                AdjustReservedSpace(0);

                Margin = Padding.Empty;

                ScrolledControl.Controls.Add(this);

                offsetY = Height - initialHeight;

                if (refScrollBar != null && !useSelfAsRefScrollBar)
                    refScrollBar.Visible = false;

                Left = scaledPx;
                Top = ScrolledControl.Height - Height - scaledPx;
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            }
            else //custom scroll bars will be placed on the right to/below scrolledControlIsParent
            {
                this.refScrollBar = this;
                Visible = true;

                sbBorderWidth = Plugin.SbBorderWidth;
                reservedBordersSpace = 0;

                initialHeight = ControlsTools.GetCustomScrollBarInitialWidth(dpiScaling, sbBorderWidth);
                Height = initialHeight;

                ResetMetricsSize(ScrolledControl.Width);
                AdjustReservedSpace(0);

                var tableLayoutPanel = ScrolledControl.Parent as TableLayoutPanel;

                if (ScrolledControl is CustomListBox box)
                    box.hScrollBar = this;
                if (ScrolledControl is CustomCheckedListBox listBox)
                    listBox.hScrollBar = this;

                var position = tableLayoutPanel.GetCellPosition(ScrolledControl); //-V3149
                tableLayoutPanel.ColumnStyles[position.Column + 1].Width = 0;
                tableLayoutPanel.RowStyles[position.Row + 1].Height = Height;

                Margin = Padding.Empty;
                Anchor = AnchorStyles.Top | AnchorStyles.Left;

                tableLayoutPanel.Controls.Add(this, position.Column, position.Row + 1); //At 1st column, 2nd row

                Dock = DockStyle.Fill;
            }

            Value = 0;

            SizeChanged += customScrollBar_SizeChanged;
        }

        internal void ResetMetricsSize(int newParentWidth)
        {
            initialWidth = newParentWidth - reservedBordersSpace;
            Width = initialWidth - reservedSpace;


            var (nRealRange, nPixelRange, nTrackWidth, nThumbWidth, fThumbWidth) = GetMetrics();


            MinimumSize = new Size(2 * (leftArrowImageHeight + leftImageAdditionalLeftWidth
                + leftImageAdditionalRightWidth + sbBorderWidth) + 4 * scaledPx, leftArrowImageHeight + 2 * sbBorderWidth);


            var fValuePerc = ((float)value - minimum) / (maximum - minimum);
            (minimum, maximum, smallChange, largeChange) = GetExternalMetrics(ScrolledControl, refScrollBar);
            value = (int)Math.Round(fValuePerc * (maximum - minimum)) + minimum;
            if (value < minimum)
                value = minimum;
            else if (value > maximum)
                value = maximum;

            //figure out thumb left
            SetThumbLeftFromValueAndLargeChange();

            if (ValueChanged != null)
                ValueChanged(this, null); //-V3083 //-V5605

            Invalidate();
        }

        internal void AdjustReservedSpace(int space)
        {
            reservedSpace = space;
            Width = initialWidth - space;

            Invalidate();
        }


        private void customScrollBar_SizeChanged(object sender, EventArgs e)
        {
            offsetY = Height - initialHeight;
            Invalidate();
        }

        internal int LargeChange
        {
            get
            {
                return largeChange;
            }

            set
            {
                largeChange = value;

                //figure out thumb left
                SetThumbLeftFromValueAndLargeChange();

                if (ValueChanged != null)
                    ValueChanged(this, null);

                Invalidate();
            }
        }

        internal int SmallChange
        {
            get
            {
                return smallChange;
            }

            set
            {
                smallChange = value;
                Invalidate();
            }
        }

        internal int Minimum
        {
            get
            {
                return minimum;
            }

            set
            {
                minimum = value;

                //figure out thumb left
                SetThumbLeftFromValueAndLargeChange();

                if (ValueChanged != null)
                    ValueChanged(this, null);

                Invalidate();
            }
        }

        internal int Maximum
        {
            get
            {
                return maximum;
            }

            set
            {
                maximum = value;

                //figure out thumb left
                SetThumbLeftFromValueAndLargeChange();

                if (ValueChanged != null)
                    ValueChanged(this, null);

                Invalidate();
            }
        }

        internal (int, int, int, int, float) GetMetrics()
        {
            var nTrackWidth = Width - 2 * (leftArrowImageWidth
                                           + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth)
                                    - 2 * sbBorderWidth; //-1 scaled px of left scroll bar border & +1 of right scroll bar border
            var fThumbWidth = (float)largeChange / (maximum - minimum) * nTrackWidth;
            var nThumbWidth = (int)Math.Round(fThumbWidth);

            if (nThumbWidth > nTrackWidth)
            {
                nThumbWidth = nTrackWidth;
                fThumbWidth = nTrackWidth;
            }

            if (nThumbWidth < minimumThumbWidth)
            {
                nThumbWidth = minimumThumbWidth;
                fThumbWidth = minimumThumbWidth;
            }

            var nRealRange = maximum - minimum - largeChange;
            var nPixelRange = nTrackWidth - nThumbWidth;

            return (nRealRange, nPixelRange, nTrackWidth, nThumbWidth, fThumbWidth);
        }

        internal int GetThumbLeft()
        {
            return thumbLeft;
        }

        internal void SetThumbLeft(int thumbLeft)
        {
            if (thumbLeft == this.thumbLeft)
                return;

            var (_, nPixelRange, _, _, _) = GetMetrics();

            if (thumbLeft > nPixelRange)
                this.thumbLeft = nPixelRange;
            else if (thumbLeft < 0)
                this.thumbLeft = 0;
            else
                this.thumbLeft = thumbLeft;


            //figure out value
            var fPerc = (float)this.thumbLeft / nPixelRange;
            var fValue = fPerc * (maximum - minimum);

            value = (int)Math.Round(fValue);

            Invalidate();
        }

        internal void SetThumbLeftFromValueAndLargeChange()
        {
            var (_, nPixelRange, _, _, _) = GetMetrics();

            var fPerc = 0.0f;
            if (maximum - minimum != 0)
                fPerc = (float)value / (maximum - minimum);

            var fLeft = fPerc * nPixelRange;
            thumbLeft = (int)Math.Round(fLeft);

            Invalidate();
        }

        internal void SetValue(int value)
        {
            if (SettingParentScroll)
            {
                SettingParentScroll = false;
                return;
            }

            if (value == this.value)
                return;
            else if (value < minimum)
                this.value = minimum;
            else if (value > maximum)
                this.value = maximum;
            else
                this.value = value;

            //figure out thumb left
            SetThumbLeftFromValueAndLargeChange();

            Invalidate();
        }

        internal int Value
        {
            get
            {
                return value;
            }

            set
            {
                SetValue(value);

                if (ValueChanged != null)
                    ValueChanged(this, null);
            }
        }

        internal Image LeftArrowImage { get; set; }

        internal Image RightArrowImage { get; set; }

        internal Image ThumbLeftImage { get; set; }

        internal Image ThumbMiddleImage { get; set; }

        internal Image ThumbRightImage { get; set; }

        private void OnPaint1(PaintEventArgs e, Image leftArrowImageArg, Image rightArrowImageArg,
            Image thumbLeftImageArg, Image thumbMiddleImageArg, Image thumbRightImageArg, bool stretchThumbImage)
        {
            if (stretchThumbImage && thumbMiddleImageArg == null)
                throw new Exception(Plugin.ExImpossibleToStretchThumb);


            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //let's calculate thumb metrics
            var (_, _, _, nThumbWidth, fThumbWidth) = GetMetrics();

            //fSpanWidth: +1 scaled px Left & Right scroll bar border: total 2 scaled px per BOTH spans
            float fSpanWidth;
            var thumbLeftRightImageWidth = 0;
            if (thumbMiddleImageArg == null)
            {
                fSpanWidth = fThumbWidth / 2.0f;
            }
            else if (stretchThumbImage && thumbLeftImageArg == null)
            {
                fSpanWidth = fThumbWidth / 2.0f;
            }
            else if (stretchThumbImage)
            {
                thumbLeftRightImageWidth = thumbLeftImageArg.Width; //-V3125 //-V3095
                fSpanWidth = fThumbWidth / 2.0f;
            }
            else //let's draw thumb middle at the Horizontal center of scroll bar (it's supposed that solid color spans are drawn in this case)
            {
                fSpanWidth = fThumbWidth - thumbMiddleImageArg.Width / 2.0f;
            }

            var nSpanWidth = (int)Math.Round(fSpanWidth);


            //draw native scroll bar top/bottom & left/right borders, scroll bar background will be drawn over it later
            if (sbBorderWidth > 0 && scrollBarBorderBrush != null)
                e.Graphics.FillRectangle(scrollBarBorderBrush, new Rectangle(0, 0, Width, Height)); //+1 scaled px shift to width

            //draw native scroll bar background
            //+1 scaled px to left shift below due to scroll bar left border,
            //-2 scaled px of size for scroll bar left/right & top/bottom borders
            e.Graphics.FillRectangle(scrollBarBackBrush, new Rectangle(sbBorderWidth, sbBorderWidth,
               Width - 2 * sbBorderWidth, Height - 2 * sbBorderWidth));

            //draw narrow scroll bar background
            //+1 scaled px to left shift below due to scroll bar left border,
            //-2 scaled px of size for scroll bar left/right & top/bottom borders
            if (narrowScrollBarBackBrush != null && Height != initialHeight)
                e.Graphics.FillRectangle(narrowScrollBarBackBrush, new Rectangle(sbBorderWidth, sbBorderWidth + offsetY,
                    Width - 2 * sbBorderWidth, initialHeight - 2 * sbBorderWidth));


            //draw left image background
            if (scrollBarThumbAndSpansBackBrush != null)
                e.Graphics.FillRectangle(scrollBarThumbAndSpansBackBrush, new Rectangle(sbBorderWidth,
                    sbBorderWidth + offsetY,
                    leftArrowImageArg.Width + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth,
                    initialHeight - 2 * sbBorderWidth));

            //draw left image
            //+1 scaled px nLeft offset due to scroll bar left border
            //-2 scaled px of height for scroll bar top/bottom borders
            e.Graphics.DrawImage(leftArrowImageArg, new Rectangle(new Point(sbBorderWidth + leftImageAdditionalLeftWidth,
                offsetY + (initialHeight - leftArrowImageArg.Height) / 2),
                new Size(leftArrowImageArg.Width, leftArrowImageArg.Height)));

            var nLeft = thumbLeft;
            nLeft += leftArrowImageArg.Width + sbBorderWidth //+1 scaled px to left shift below due to scroll bar left border
                + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth;


            //draw spans & thumb top/bottom & left/right borders, spans & thumb background will be drawn over it later
            if (scrollBarThumbAndSpansBorderBrush != null)
                e.Graphics.FillRectangle(scrollBarThumbAndSpansBorderBrush, new Rectangle(nLeft, sbBorderWidth + offsetY,
                nThumbWidth, initialHeight - 2 * sbBorderWidth));

            //draw spans & thumb background, thumb image(s) will be drawn over it later
            if (scrollBarThumbAndSpansBackBrush != null)
                e.Graphics.FillRectangle(scrollBarThumbAndSpansBackBrush, new Rectangle(nLeft + sbBorderWidth,
                    scaledPx + sbBorderWidth + offsetY,
                    nThumbWidth - sbBorderWidth, initialHeight - 2 * sbBorderWidth - scaledPx));

            //draw thumb
            if (thumbMiddleImageArg != null)
            {
                if (stretchThumbImage)
                {
                    if (thumbLeftImageArg != null)
                    {
                        e.Graphics.DrawImage(thumbLeftImageArg, new Rectangle(nLeft, offsetY + (initialHeight - leftArrowImageArg.Height) / 2,
                            thumbLeftImageArg.Width, thumbLeftImageArg.Height));
                    }

                    nLeft += thumbLeftRightImageWidth;

                    Plugin.DrawRepeatedImage(e.Graphics, thumbMiddleImageArg, nLeft,
                        offsetY + (int)Math.Round((initialHeight - 1.375d * thumbMiddleImageArg.Height) / 2),
                        nThumbWidth - 2 * thumbLeftRightImageWidth, thumbMiddleImageArg.Height, false, true);

                    nLeft += nThumbWidth - 2 * thumbLeftRightImageWidth;

                    if (thumbLeftImageArg != null) //Both thumbLeftImageArg & thumbRightImageArg must be null if thumbLeftImageArg is null
                    {
                        e.Graphics.DrawImage(thumbRightImageArg, new Rectangle(nLeft, offsetY + (initialHeight - leftArrowImageArg.Height) / 2,
                        thumbRightImageArg.Width, thumbRightImageArg.Height));
                    }
                }
                else
                {
                    nLeft += nSpanWidth;

                    e.Graphics.DrawImage(thumbMiddleImageArg, new Rectangle(nLeft,
                        offsetY + (int)Math.Round((initialHeight - 1.375d * thumbMiddleImageArg.Height) / 2),
                        thumbMiddleImageArg.Width, thumbMiddleImageArg.Height));
                }
            }


            //draw right image background
            if (scrollBarThumbAndSpansBackBrush != null)
                e.Graphics.FillRectangle(scrollBarThumbAndSpansBackBrush, new Rectangle(Width - rightArrowImageArg.Width
                    - leftImageAdditionalLeftWidth - leftImageAdditionalRightWidth - sbBorderWidth,
                    sbBorderWidth + offsetY,
                    rightArrowImageArg.Width + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth,
                    initialHeight - 2 * sbBorderWidth));

            //draw right image
            //-1 scaled px Horizontal offset due to scroll bar right border
            //-2 scaled px of height for scroll bar top/bottom borders
            e.Graphics.DrawImage(rightArrowImageArg, new Rectangle(new Point(Width - rightArrowImageArg.Width
                - leftImageAdditionalLeftWidth - sbBorderWidth, offsetY + (initialHeight - leftArrowImageArg.Height) / 2),
                new Size(rightArrowImageArg.Width, rightArrowImageArg.Height)));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Below are some skinning versions

            //Version 1:
            //OnPaint1(e, leftArrowImage, _rightArrowImage, null, _thumbMiddleImage, null, false);
            //Version 2:
            //OnPaint1(e, leftArrowImage, _rightArrowImage, null, null, null, false);
            //Version 3:
            //OnPaint1(e, leftArrowImage, _rightArrowImage, _thumbLeftImage, _thumbMiddleImage, _thumbRightImage, true);
            //Version 4:
            OnPaint1(e, LeftArrowImage, RightArrowImage, null, ThumbMiddleImage, null, true);
        }

        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }

            set
            {
                base.AutoSize = value;
                if (base.AutoSize)
                    Height = initialHeight;
            }
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            //
            //CustomScrollBar
            //
            MouseDown += CustomScrollBar_MouseDown;
            MouseMove += CustomScrollBar_MouseMove;
            MouseUp += CustomScrollBar_MouseUp;
            ResumeLayout(false);
        }

        private void moveThumbLeftRight(object state)
        {
            var (nRealRange, nPixelRange, nTrackWidth, nThumbWidth, fThumbWidth) = GetMetrics();

            if (leftArrowDown)
            {
                if (nRealRange > 0)
                {
                    if (nPixelRange > 0)
                    {
                        if (thumbLeft - smallChange < 0)
                            thumbLeft = 0;
                        else
                            thumbLeft -= smallChange / 4;

                        //figure out value
                        var fPerc = (float)thumbLeft / nPixelRange;
                        var fValue = fPerc * (maximum - minimum);

                        value = (int)Math.Round(fValue);
                    }
                }
            }
            else if (rightArrowDown)
            {
                if (nRealRange > 0)
                {
                    if (nPixelRange > 0)
                    {
                        if (thumbLeft + smallChange > nPixelRange)
                            thumbLeft = nPixelRange;
                        else
                            thumbLeft += smallChange / 4;

                        //figure out value
                        var fPerc = (float)thumbLeft / nPixelRange;
                        var fValue = fPerc * (maximum - minimum);

                        value = (int)Math.Round(fValue);
                    }
                }
            }

            Plugin.MbForm.Invoke(new Action(() =>
            {
                if (ValueChanged != null)
                    ValueChanged(this, null);
            }));
        }

        private void CustomScrollBar_MouseDown(object sender, MouseEventArgs e)
        {
            var ptPoint = PointToClient(Cursor.Position);

            var (nRealRange, nPixelRange, nTrackWidth, nThumbWidth, fThumbWidth) = GetMetrics();

            var nLeft = thumbLeft;
            nLeft += leftArrowImageWidth + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth
                + sbBorderWidth; //+1 scaled px to left shift below due to left border


            var thumbRect = new Rectangle(new Point(nLeft, sbBorderWidth + offsetY),
                new Size(nThumbWidth, initialHeight - 2 * sbBorderWidth));
            var leftarrowRect = new Rectangle(new Point(sbBorderWidth, sbBorderWidth + offsetY),
                new Size(leftArrowImageWidth + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth,
                initialHeight - 2 * sbBorderWidth));
            var rightarrowRect = new Rectangle(new Point(leftArrowImageWidth
                                                         + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth + sbBorderWidth + nTrackWidth,
                sbBorderWidth + offsetY),
                new Size(leftArrowImageWidth + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth,
                initialHeight - 2 * sbBorderWidth));

            if (thumbRect.Contains(ptPoint))
            {
                //hit the thumb
                clickPoint = (ptPoint.X - nLeft);
                thumbDown = true;
            }
            else if (leftarrowRect.Contains(ptPoint))
            {
                leftArrowDown = true;
                thumbLeftRightRepeater = new System.Threading.Timer(moveThumbLeftRight, null, 0, Plugin.ThumbMoveRepeatInterval);
            }
            else if (rightarrowRect.Contains(ptPoint))
            {
                rightArrowDown = true;
                thumbLeftRightRepeater = new System.Threading.Timer(moveThumbLeftRight, null, 0, Plugin.ThumbMoveRepeatInterval);
            }
            else if (thumbRect.Left > ptPoint.X && thumbRect.Top < ptPoint.Y && thumbRect.Top + thumbRect.Height > ptPoint.Y)
            {
                float fScrollDistance = Width - 2 * (leftArrowImageWidth
                    + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth + sbBorderWidth);

                var fScrollPerc = 0.0f;
                if (fScrollDistance != 0) //-V3024
                    fScrollPerc = ((float)ptPoint.X - (leftArrowImageWidth
                        + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth + sbBorderWidth)) / fScrollDistance;

                if (fScrollPerc < 0)
                    fScrollPerc = 0;

                //figure out value
                var fValue = minimum + fScrollPerc * (maximum - minimum);
                value = (int)Math.Round(fValue);

                //figure out thumb left
                SetThumbLeftFromValueAndLargeChange();

                if (ValueChanged != null)
                    ValueChanged(this, null);
            }
            else if (thumbRect.Left + thumbRect.Width < ptPoint.X && thumbRect.Top < ptPoint.Y && thumbRect.Top + thumbRect.Height > ptPoint.Y)
            {
                float fScrollDistance = Width - 2 * (leftArrowImageWidth
                    + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth + sbBorderWidth);

                var fScrollPerc = 0.0f;
                if (fScrollDistance != 0) //-V3024
                    fScrollPerc = 1f - ((float)Width - 2 * (leftArrowImageWidth
                        + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth + sbBorderWidth) - ptPoint.X) / fScrollDistance;

                if (fScrollPerc > 1)
                    fScrollPerc = 1;

                //figure out value
                var fValue = minimum + fScrollPerc * (maximum - minimum);
                value = (int)Math.Round(fValue);

                //figure out thumb left
                SetThumbLeftFromValueAndLargeChange();

                if (ValueChanged != null)
                    ValueChanged(this, null);
            }
        }

        private void CustomScrollBar_MouseUp(object sender, MouseEventArgs e)
        {
            thumbDown = false;
            thumbDragging = false;
            leftArrowDown = false;
            rightArrowDown = false;

            thumbLeftRightRepeater?.Dispose();
            thumbLeftRightRepeater = null;
        }

        private void MoveThumb(int x)
        {
            var (nRealRange, nPixelRange, _, _, _) = GetMetrics();

            var nSpot = clickPoint;

            if (thumbDown && nRealRange > 0)
            {
                if (nPixelRange > 0)
                {
                    var nNewThumbLeft = x - (leftArrowImageWidth
                                             + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth + sbBorderWidth + nSpot);

                    if (nNewThumbLeft < 0)
                        thumbLeft = 0;
                    else if (nNewThumbLeft > nPixelRange)
                        thumbLeft = nPixelRange;
                    else
                        thumbLeft = x - (leftArrowImageWidth
                            + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth + sbBorderWidth + nSpot);

                    //figure out value
                    var fPerc = (float)thumbLeft / nPixelRange;
                    var fValue = fPerc * (maximum - minimum);
                    value = (int)Math.Round(fValue);
                }
            }
        }

        private void CustomScrollBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (thumbDown)
                thumbDragging = true;

            if (thumbDragging)
            {
                MoveThumb(e.X);

                if (ValueChanged != null)
                    ValueChanged(this, null);

                Invalidate();
            }
        }
    }
}

internal class InterpolatedBox : PictureBox
{
    #region Interpolation Property
    private InterpolationMode interpolation = InterpolationMode.Default;

    internal InterpolationMode Interpolation
    {
        get { return interpolation; }
        set
        {
            if (value == InterpolationMode.Invalid)
                throw new ArgumentException("\"Invalid\" is not a valid value."); //(Duh!)

            interpolation = value;
            Invalidate(); //Image should be redrawn when a different interpolation is selected
        }
    }
    #endregion

    protected override void OnPaint(PaintEventArgs pe)
    {
        //Before the PictureBox renders the image, we modify the
        //graphics object to change the interpolation.

        //Set the selected interpolation.
        pe.Graphics.InterpolationMode = interpolation;
        //Certain interpolation modes (such as nearest neighbor) need
        //to be offset by half a pixel to render correctly.
        pe.Graphics.PixelOffsetMode = PixelOffsetMode.Half;

        //Allow the PictureBox to draw.
        base.OnPaint(pe);
    }
}
