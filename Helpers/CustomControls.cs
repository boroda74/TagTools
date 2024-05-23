using ExtensionMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static ExtensionMethods.ReadonlyControls;
using static NativeMethods;

namespace MusicBeePlugin
{
    public static class ControlsTools
    {
        public static T FindControlChild<T>(Control container) where T : Control
        {
            foreach (Control child in container.Controls)
            {
                if (child is T Tchild)
                    return Tchild;
            }

            //Not found.
            return null;
        }

        public static List<T> FindControlAllChildren<T>(Control container) where T : Control
        {
            var children = new List<T>();

            foreach (Control child in container.Controls)
            {
                if (child is T Tchild)
                    children.Add(Tchild);
            }

            return children;
        }

        static public int GetCustomScrollBarInitialWidth(float dpiScaling, int sbBorderWidth)
        {
            return (int)Math.Round(dpiScaling * (Plugin.ScrollBarWidth + 2 * sbBorderWidth));
        }
    }

    public class CustomComboBox : UserControl
    {
        public int lastSelectedIndex = -1;

        private int scaledPx = 1;
        private string cue = string.Empty;

        public ComboBox comboBox = null;

        private TextBox textBox = null;
        private Button button = null;
        private CustomListBox listBox = null;

        private bool textBoxReadOnlyCache = false;

        private ToolStripDropDown dropDown = null;
        private static readonly TimeSpan DropDownAutoCloseThreshold = TimeSpan.FromMilliseconds(250);
        private DateTime dropDownClosedTime = DateTime.UtcNow - DropDownAutoCloseThreshold;
        private int initialDropDownWidth = 250;


        private static readonly object EVENT_SELECTEDINDEXCHANGED = new object();

        public event EventHandler SelectedIndexChanged
        {
            add
            {
                base.Events.AddHandler(EVENT_SELECTEDINDEXCHANGED, value);
            }
            remove
            {
                base.Events.RemoveHandler(EVENT_SELECTEDINDEXCHANGED, value);
            }
        }


        private static readonly object EVENT_SELECTEDITEMCHANGED = new object();

        public event EventHandler SelectedItemChanged
        {
            add
            {
                base.Events.AddHandler(EVENT_SELECTEDINDEXCHANGED, value);
            }
            remove
            {
                base.Events.RemoveHandler(EVENT_SELECTEDINDEXCHANGED, value);
            }
        }


        private static readonly object EVENT_DROPDOWNCLOSED = new object();

        public event EventHandler DropDownClosed
        {
            add
            {
                base.Events.AddHandler(EVENT_DROPDOWNCLOSED, value);
            }
            remove
            {
                base.Events.RemoveHandler(EVENT_DROPDOWNCLOSED, value);
            }
        }


        private TableLayoutPanel getDropDown(PluginWindowTemplate ownerForm, int customScrollBarInitialWidth)
        {
            var tableLayoutPanel = new TableLayoutPanel();

            tableLayoutPanel.Font = this.Font;
            tableLayoutPanel.Margin = new Padding(0, 0, 0, 0);
            tableLayoutPanel.Padding = new Padding(0, 0, 0, 0);
            tableLayoutPanel.AutoSize = true;
            tableLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            var columnStyle0 = new ColumnStyle(SizeType.AutoSize, 100f);
            var columnStyle1 = new ColumnStyle(SizeType.Absolute, 0f);

            tableLayoutPanel.ColumnStyles.Add(columnStyle0);
            tableLayoutPanel.ColumnStyles.Add(columnStyle1);

            var rowStyle0 = new RowStyle(SizeType.AutoSize, 100f);
            var rowStyle1 = new RowStyle(SizeType.Absolute, 0f);

            tableLayoutPanel.RowStyles.Add(rowStyle0);
            tableLayoutPanel.RowStyles.Add(rowStyle1);


            listBox = new CustomListBox(customScrollBarInitialWidth, scaledPx);
            listBox.Font = this.Font;
            listBox.MultiColumn = false;
            listBox.IntegralHeight = false;
            listBox.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            listBox.Margin = new Padding(0, 0, 0, 0);

            listBox.SelectedIndex = -1;

            tableLayoutPanel.Controls.Add(listBox, 0, 0);

            ownerForm.skinControl(listBox, 0);

            return tableLayoutPanel;
        }

        public CustomComboBox(PluginWindowTemplate ownerForm, ComboBox comboBoxRef, bool skinned)
        {
            //SetStyle(ControlStyles.ResizeRedraw, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //SetStyle(ControlStyles.DoubleBuffer, true);

            if (skinned)
                initSkinned(ownerForm, comboBoxRef);
            else
                init(ownerForm, comboBoxRef);
        }

        private void init(PluginWindowTemplate ownerForm, ComboBox comboBox)
        {
            scaledPx = ownerForm.scaledPx;
            textBox = null;
            button = null;
            listBox = null;
            dropDown = null;
            initialDropDownWidth = comboBox.DropDownWidth;

            Control parent = comboBox.Parent;
            int index = parent.Controls.IndexOf(comboBox);
            TableLayoutPanelCellPosition cellPosition = default;
            Point location = comboBox.Location;

            if (parent is TableLayoutPanel)
                cellPosition = (parent as TableLayoutPanel).GetCellPosition(comboBox);

            this.Font = comboBox.Font;
            this.Margin = comboBox.Margin;
            this.Padding = new Padding(0, 0, 0, 0);

            this.Anchor = comboBox.Anchor;
            this.TabIndex = comboBox.TabIndex;
            this.TabStop = comboBox.TabStop;
            this.Tag = comboBox.Tag;
            this.Name = comboBox.Name;


            this.comboBox = (ComboBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.ComboBox);
            this.comboBox.DropDownStyle = comboBox.DropDownStyle;

            foreach (var item in comboBox.Items)
                this.comboBox.Items.Add(item);

            this.comboBox.Font = this.Font;

            this.comboBox.Location = new Point(0, 0);
            this.comboBox.Margin = new Padding(0, 0, 0, 0);
            this.comboBox.Size = new Size(comboBox.Width, comboBox.Height);
            this.comboBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            this.Size = new Size(this.comboBox.Width, this.comboBox.Height + 1);

            this.Controls.Add(this.comboBox);

            this.DropDownStyle = comboBox.DropDownStyle;


            CopyComboBoxEventHandlers(comboBox, this.comboBox);


            this.SizeChanged += customComboBox_SizeChanged;
            customComboBox_SizeChanged(this, null);

            int allControlsIndex = ownerForm.allControls.IndexOf(comboBox);
            ownerForm.allControls.RemoveAt(allControlsIndex);
            ownerForm.allControls.Insert(allControlsIndex, this);

            parent.Controls.RemoveAt(index);
            comboBox.Dispose();

            parent.Controls.Add(this);
            parent.Controls.SetChildIndex(this, index);
            if (parent is TableLayoutPanel)
                (parent as TableLayoutPanel).SetCellPosition(this, cellPosition);

            this.Location = location;

            ownerForm.namesComboBoxes.Add(this.Name, this);
        }

        private void initSkinned(PluginWindowTemplate ownerForm, ComboBox comboBox)
        {
            scaledPx = ownerForm.scaledPx;
            this.comboBox = null;
            initialDropDownWidth = comboBox.DropDownWidth;

            int dropDownWidth = comboBox.DropDownWidth;
            if (comboBox.DropDownWidth < comboBox.Width)
                dropDownWidth = comboBox.Width;

            int dropDownHeight = comboBox.DropDownHeight + 3 * scaledPx;
            if (comboBox.DropDownHeight == 106)
                dropDownHeight = comboBox.ItemHeight * 20 + 3 * scaledPx;

            Control parent = comboBox.Parent;
            int index = parent.Controls.IndexOf(comboBox);
            TableLayoutPanelCellPosition cellPosition = default;
            Point location = comboBox.Location;

            if (parent is TableLayoutPanel)
                cellPosition = (parent as TableLayoutPanel).GetCellPosition(comboBox);

            this.Font = comboBox.Font;
            this.ForeColor = Plugin.InputControlForeColor;
            this.BackColor = Plugin.InputControlBackColor;
            this.Margin = comboBox.Margin;
            this.Padding = new Padding(0, 0, 0, 0);
            this.Anchor = comboBox.Anchor;
            this.TabIndex = comboBox.TabIndex;
            this.TabStop = comboBox.TabStop;
            this.Tag = comboBox.Tag;
            this.Name = comboBox.Name;


            int customScrollBarInitialWidth = ControlsTools.GetCustomScrollBarInitialWidth(ownerForm.dpiScaling, 0);

            textBox = (TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);

            textBox.Font = this.Font;
            textBox.Location = new Point(0, 0);
            textBox.Margin = new Padding(0, 0, 0, 0);
            textBox.Size = new Size(comboBox.Width - customScrollBarInitialWidth, textBox.Height);
            textBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            if (comboBox.DropDownStyle == ComboBoxStyle.DropDownList)
                textBox.Click += button_Click;

            textBox.KeyPress += ownerForm.CustomComboBox_KeyPress;
            textBox.KeyDown += ownerForm.CustomComboBox_KeyDown;

            ownerForm.skinControl(textBox);

            this.Size = new Size(comboBox.Width, textBox.Height);

            this.Controls.Add(textBox);


            button = new Button();
            button.Font = this.Font;

            button.Location = new Point(comboBox.Width - customScrollBarInitialWidth, -1);
            button.Margin = new Padding(0, 0, 0, 0);
            button.Size = new Size(customScrollBarInitialWidth, textBox.Height + 2);
            button.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            button.Text = string.Empty;
            button.ImageAlign = ContentAlignment.MiddleCenter;
            button.TextImageRelation = TextImageRelation.Overlay;
            button.Image = Plugin.DownArrowComboBoxImage;

            button.Click += button_Click;

            ownerForm.skinControl(button);
            button.FlatAppearance.BorderColor = ownerForm.narrowScrollBarBackColor;

            this.Controls.Add(button);

            ownerForm.nonDefaultableButtons.Add(button);


            TableLayoutPanel dropDownControl = getDropDown(ownerForm, customScrollBarInitialWidth);
            var controlHost = new ToolStripControlHost(dropDownControl);
            controlHost.Font = this.Font;
            controlHost.AutoSize = true;
            controlHost.Padding = new Padding(0, 0, 0, 0);
            controlHost.Margin = new Padding(0, 0, 0, 0);


            dropDown = new ToolStripDropDown();
            dropDown.Font = this.Font;
            dropDown.BackColor = textBox.BackColor;
            dropDown.ForeColor = textBox.ForeColor;
            dropDown.Padding = new Padding(0, 0, 0, 0);
            dropDown.Margin = new Padding(0, 0, 0, 0);
            dropDown.AutoSize = true;
            dropDown.DropShadowEnabled = true;
            dropDown.Items.Add(controlHost);

            dropDown.Tag = this;

            dropDown.Closed += dropDown_Closed;
            dropDown.KeyPress += ownerForm.CustomComboBox_KeyPress;
            dropDown.KeyDown += ownerForm.CustomComboBox_KeyDown;


            foreach (var item in comboBox.Items)
                listBox.Items.Add(item);

            listBox.MouseMove += listBox_MouseMove;
            listBox.Click += listBox_ItemChosen;
            listBox.ItemsChanged += listBox_ItemsChanged;

            listBox.MinimumSize = new Size(dropDownWidth, 2);
            listBox.MaximumSize = new Size(dropDownWidth, dropDownHeight);


            CopyComboBoxEventHandlers(comboBox, this);


            this.SizeChanged += customComboBox_SizeChanged;
            customComboBox_SizeChanged(this, null);

            this.DropDownStyle = comboBox.DropDownStyle;

            int allControlsIndex = ownerForm.allControls.IndexOf(comboBox);
            ownerForm.allControls.RemoveAt(allControlsIndex);
            ownerForm.allControls.Insert(allControlsIndex, this);

            parent.Controls.RemoveAt(index);
            comboBox.Dispose();


            parent.Controls.Add(this);
            parent.Controls.SetChildIndex(this, index);
            if (parent is TableLayoutPanel)
                (parent as TableLayoutPanel).SetCellPosition(this, cellPosition);

            this.Location = location;

            ownerForm.namesComboBoxes.Add(this.Name, this);
        }

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (textBox != null)
                    textBox.Dispose();

                if (button != null)
                    button.Dispose();

                if (listBox != null)
                    listBox.Dispose();

                if (dropDown != null)
                    dropDown.Dispose();

                if (comboBox != null)
                    comboBox.Dispose();
            }

            base.Dispose(disposing);
        }

        ~CustomComboBox()
        {
            Dispose(false);
        }


        public int IndexOfText(string text)
        {
            int index = SelectedIndex;

            if (index >= 0 && Items[index].ToString() == text)
                return index;

            for (int i = 0; i < Items.Count; i++)
                if (Items[i].ToString() == text)
                    return i;

            return -1;
        }

        public void AddRange(object[] items)
        {
            if (comboBox == null)
                listBox.Items.AddRange(items);
            else
                comboBox.Items.AddRange(items);
        }

        public void ItemsClear()
        {
            if (comboBox == null)
                listBox.Items.Clear();
            else
                comboBox.Items.Clear();

            SelectedIndex = -1;
        }

        public IList Items
        {
            get
            {
                if (comboBox == null)
                    return (IList)listBox.Items;
                else
                    return (IList)comboBox.Items;
            }
        }

        public string GetItemText(object item)
        {
            if (comboBox == null)
                return listBox.GetItemText(item);
            else
                return comboBox.GetItemText(item);
        }

        public bool MustKeyEventsBeHandled
        {
            get
            {
                if (comboBox == null && button.IsEnabled())
                    return true;
                else
                    return false;
            }
        }

        public bool Sorted
        {
            get
            {
                if (comboBox == null)
                    return listBox.Sorted;
                else
                    return comboBox.Sorted;
            }

            set
            {
                if (comboBox == null)
                    listBox.Sorted = value;
                else
                    comboBox.Sorted = value;
            }
        }

        public int SelectionStart
        {
            get
            {
                if (comboBox == null)
                    return textBox.SelectionStart;
                else
                    return comboBox.SelectionStart;
            }

            set
            {
                if (comboBox == null)
                    textBox.SelectionStart = value;
                else
                    comboBox.SelectionStart = value;
            }
        }

        public int SelectionLength
        {
            get
            {
                if (comboBox == null)
                    return textBox.SelectionLength;
                else
                    return comboBox.SelectionLength;
            }

            set
            {
                if (comboBox == null)
                    textBox.SelectionLength = value;
                else
                    comboBox.SelectionLength = value;
            }
        }

        public void SetText(string value)
        {
            int index = IndexOfText(value);

            if (comboBox == null)
            {
                if (index == -1 && string.IsNullOrEmpty(value) && textBox.ReadOnly)
                {
                    textBox.ForeColor = Plugin.InputControlForeColor;
                    textBox.Text = cue;
                }
                else if (index == -1 && string.IsNullOrEmpty(value) && !textBox.ReadOnly)
                {
                    textBox.ForeColor = Plugin.InputControlDimmedForeColor;
                    textBox.Text = cue;
                }
                else
                {
                    textBox.ForeColor = Plugin.InputControlForeColor;
                    textBox.Text = value;

                    SelectedIndex = index;
                }

                this.FireEvent("TextChanged", null);
            }
            else//if (comboBox != null)
            {
                if (index == -1 && DropDownStyle == ComboBoxStyle.DropDown)
                    comboBox.Text = value;
                else
                    SelectedIndex = index;

                this.FireEvent("TextChanged", null);
            }
        }

        public override string Text
        {
            get
            {
                if (comboBox == null)
                    return textBox.Text;
                else
                    return comboBox.Text;
            }

            set
            {
                if (comboBox == null && textBox.Text != value)
                    SetText(value);
                else if (comboBox != null && comboBox.Text != value)
                    SetText(value);
            }
        }

        public int SelectedIndex
        {
            get
            {
                if (comboBox == null)
                    return listBox.SelectedIndex;
                else
                    return comboBox.SelectedIndex;
            }

            set
            {
                if (comboBox == null && lastSelectedIndex != value)
                {
                    listBox.SelectedIndex = value;
                    lastSelectedIndex = value;

                    if (value != -1)
                        Text = listBox.Items[value].ToString();
                    else if (DropDownStyle == ComboBoxStyle.DropDownList)
                        Text = string.Empty;

                    base.Events[EVENT_SELECTEDINDEXCHANGED]?.DynamicInvoke(this, null);
                }
                else if (comboBox != null && comboBox.SelectedIndex != value)
                {
                    comboBox.SelectedIndex = value;

                    if (value != -1)
                        Text = comboBox.Items[value].ToString();
                    else if (DropDownStyle == ComboBoxStyle.DropDownList)
                        Text = string.Empty;

                    base.Events[EVENT_SELECTEDINDEXCHANGED]?.DynamicInvoke(this, null);
                }
            }
        }

        public object SelectedItem
        {
            get
            {
                if (comboBox == null)
                    return listBox.SelectedItem;
                else
                    return comboBox.SelectedItem;
            }

            set
            {
                if (comboBox == null && listBox.SelectedItem != value)
                {
                    listBox.SelectedItem = value;
                    lastSelectedIndex = listBox.SelectedIndex;

                    if (value != null)
                        Text = value.ToString();
                    else if (DropDownStyle == ComboBoxStyle.DropDownList)
                        Text = string.Empty;

                    base.Events[EVENT_SELECTEDITEMCHANGED]?.DynamicInvoke(this, null);
                }
                else if (comboBox != null && comboBox.SelectedItem != value)
                {
                    comboBox.SelectedItem = value;

                    if (value != null)
                        Text = value.ToString();

                    base.Events[EVENT_SELECTEDITEMCHANGED]?.DynamicInvoke(this, null);
                }
            }
        }

        public bool ReadOnly
        {
            get
            {
                if (comboBox == null)
                    return textBoxReadOnlyCache;
                else
                    return comboBox.DropDownStyle == ComboBoxStyle.DropDownList;
            }

            set
            {
                textBoxReadOnlyCache = value;

                if (comboBox == null)
                    textBox.ReadOnly = value;
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
                if (comboBox == null)
                {
                    textBoxReadOnlyCache = textBox.ReadOnly;
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
                if (comboBox == null)
                {
                    textBox.ReadOnly = textBoxReadOnlyCache;
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
            if (comboBox == null)
                return button.IsEnabled();
            else
                return comboBox.IsEnabled();
        }

        public new bool Visible
        {
            get { return base.Visible; }

            set
            {
                if (comboBox == null && base.Visible != value)
                {
                    textBox.Visible = value;
                    button.Visible = value;

                    this.FireEvent("VisibleChanged", null);
                }
                else if (comboBox != null && base.Visible != value)
                {
                    comboBox.Visible = value;

                    this.FireEvent("VisibleChanged", null);
                }

                base.Visible = value;
            }
        }

        public new bool Enabled
        {
            get { return base.Enabled; }

            set
            {
                if (base.Enabled != value)
                {
                    comboBox?.Enable(value);
                    textBox?.Enable(value);
                    button?.Enable(value);

                    base.Enabled = value;

                    this.FireEvent("EnabledChanged", null);
                }
            }
        }

        public ComboBoxStyle DropDownStyle
        {
            get
            {
                if (comboBox == null)
                    return textBoxReadOnlyCache ? ComboBoxStyle.DropDownList : ComboBoxStyle.DropDown;
                else
                    return comboBox.DropDownStyle;
            }

            set
            {
                if (comboBox != null && comboBox.DropDownStyle == value)
                    return;
                else if (comboBox != null && comboBox.DropDownStyle != value)
                    comboBox.DropDownStyle = value;
                else if (value == ComboBoxStyle.DropDown)
                    ReadOnly = false;
                else if (value == ComboBoxStyle.DropDownList)
                    ReadOnly = true;
                else
                    throw new Exception(Plugin.ExInvalidDropdownStyle);
            }
        }

        private void CopyComboBoxEventHandlers(ComboBox comboBox, Control destControl)
        {
            if (destControl is CustomComboBox customComboBox)
            {
                comboBox.CopyEventHandlersTo(customComboBox.textBox, "TextChanged", false);
                comboBox.CopyEventHandlersTo(customComboBox.listBox, "SelectedIndexChanged", false);
                comboBox.CopyEventHandlersTo(customComboBox.listBox, "SelectedItemChanged", false);
                comboBox.CopyEventHandlersTo(destControl, "VisibleChanged", false);
                comboBox.CopyEventHandlersTo(destControl, "EnabledChanged", false);
                comboBox.CopyEventHandlersTo(destControl, "DropDownClosed", false);
            }
            else
            {
                comboBox.CopyEventHandlersTo(destControl, "TextChanged", false);
                comboBox.CopyEventHandlersTo(destControl, "SelectedIndexChanged", false);
                comboBox.CopyEventHandlersTo(destControl, "SelectedItemChanged", false);
                comboBox.CopyEventHandlersTo(destControl, "VisibleChanged", false);
                comboBox.CopyEventHandlersTo(destControl, "EnabledChanged", false);
                comboBox.CopyEventHandlersTo(destControl, "DropDownClosed", false);
            }
        }

        private void customComboBox_SizeChanged(object sender, EventArgs e)
        {
            if (comboBox == null)
            {
                int dropDownWidth = this.Width;
                if (dropDownWidth < initialDropDownWidth)
                    dropDownWidth = initialDropDownWidth;

                listBox.AdjustHeight(dropDownWidth);
            }
            else
            {
                if (this.Width < initialDropDownWidth)
                    comboBox.DropDownWidth = initialDropDownWidth;
                else
                    comboBox.DropDownWidth = this.Width;
            }
        }

        private void listBox_ItemsChanged(object sender, EventArgs e)
        {
            int dropDownWidth = this.Width;
            if (dropDownWidth < initialDropDownWidth)
                dropDownWidth = initialDropDownWidth;

            listBox.AdjustHeight(dropDownWidth);

            PluginWindowTemplate.UpdateCustomScrollBars(listBox);
        }

        public void listBox_ItemChosen(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex >= 0)
            {
                int index = listBox.SelectedIndex;
                listBox.SelectedIndex = -1;
                SelectedIndex = index;
                textBox.Focus();
            }

            dropDown.Close();
        }

        private void listBox_MouseMove(object sender, MouseEventArgs e)
        {
            var listBox = sender as CustomListBox;
            int index = listBox.IndexFromPoint(e.X, e.Y);

            if (index >= 0)
                listBox.SelectedIndex = index;
        }

        private void TextBox_GotFocus(object sender, EventArgs e)
        {
            button_Click(null, null);
        }

        private void dropDown_Closed(object sender, EventArgs e)
        {
            dropDownClosedTime = DateTime.UtcNow;

            base.Events[EVENT_DROPDOWNCLOSED]?.DynamicInvoke(this, null);
        }

        private void button_Click(object sender, EventArgs e)
        {
            if (!button.IsEnabled())
                return;


            if (DateTime.UtcNow - dropDownClosedTime > DropDownAutoCloseThreshold)
            {
                listBox.SelectedIndex = lastSelectedIndex;

                Point textBoxScreenLocation = textBox.PointToScreen(textBox.Location);

                //try to position _dropDown below textbox
                Point pt = textBoxScreenLocation;
                pt.Offset(0, textBox.Height);

                //determine if it will fit on the screen below the textbox
                Size dropdownSize = dropDown.GetPreferredSize(Size.Empty);
                Rectangle dropdownBounds = new Rectangle(pt, dropdownSize);

                if (dropdownBounds.Bottom <= Screen.GetWorkingArea(dropdownBounds).Bottom)
                {   //show below
                    dropDown.Show(pt, ToolStripDropDownDirection.BelowRight);
                }
                else
                {   //show above
                    dropDown.Show(textBoxScreenLocation, ToolStripDropDownDirection.AboveRight);
                }
            }
        }

        public void SetCue(string cue, bool forceCue)
        {
            if (forceCue)
                SelectedIndex = -1;

            if (comboBox == null)
            {
                this.cue = cue;
                if (listBox.SelectedIndex == -1)
                    this.SetText(string.Empty);
            }
            else
            {
                comboBox.SetCue(cue);
            }
        }
    }

    public class CustomTextBox : TextBox
    {
        private int scrollPosition = 0;

        public CustomVScrollBar vScrollBar = null;

        public int ScrollPosition
        {
            get { return scrollPosition; }
            set
            {
                if (scrollPosition != value)
                    scrollPosition = value;
            }
        }

        public void UpdateCaretPositionForScrolling(int position)
        {
            scrollPosition = GetLineFromCharIndex(position);
        }

        public void Scroll(int delta)
        {
            scrollPosition += delta;
            NativeMethods.SendMessage(this.Handle, NativeMethods.EM_LINESCROLL, Zero, (IntPtr)delta);
        }
        public void ScrollToTop()
        {
            NativeMethods.SendMessage(this.Handle, NativeMethods.EM_LINESCROLL, Zero, (IntPtr)(-100000));
            vScrollBar.SetValue(0);
            vScrollBar.Invalidate();
            Invalidate();
        }
    }

    public class CustomListBox : ListBox
    {
        private readonly int scaledPx = -10000;
        private readonly int customScrollBarInitialWidth = 0;

        private readonly bool showScroll = true;
        private bool processPaintMessages = true;

        public CustomHScrollBar hScrollBar = null;
        public CustomVScrollBar vScrollBar = null;

        public event EventHandler ItemsChanged;

        public CustomListBox(bool showScroll)
        {
            this.showScroll = showScroll;

            InitializeComponent();

            //SetStyle(ControlStyles.ResizeRedraw, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //SetStyle(ControlStyles.DoubleBuffer, true);

            //TODO: Add any initialization after the InitForm call
        }

        public CustomListBox(int customScrollBarInitialWidth, int scaledPx) : this(false)
        {
            this.customScrollBarInitialWidth = customScrollBarInitialWidth;
            this.scaledPx = scaledPx;
        }

        public void AdjustHeight(int dropDownWidth)
        {
            if (scaledPx > 0)
            {
                int itemsHeight = ItemHeight * Items.Count + 3 * scaledPx;

                if (itemsHeight > MaximumSize.Height)
                {
                    if ((Parent as TableLayoutPanel).ColumnStyles[1].Width != vScrollBar.Width)
                    {
                        (Parent as TableLayoutPanel).ColumnStyles[1].Width = vScrollBar.Width;
                        if (Height != MaximumSize.Height)
                        {
                            Height = MaximumSize.Height;
                            vScrollBar.ResetMetricsSize(Height);
                        }
                    }

                    vScrollBar.Visible = true;
                }
                else
                {
                    if ((Parent as TableLayoutPanel).ColumnStyles[1].Width != 0)
                        (Parent as TableLayoutPanel).ColumnStyles[1].Width = 0;

                    if (Height != itemsHeight)
                        Height = itemsHeight;

                    vScrollBar.Visible = false;
                }


                if (vScrollBar.Visible)
                {
                    MinimumSize = new Size(dropDownWidth - customScrollBarInitialWidth, 2);
                    MaximumSize = new Size(dropDownWidth - customScrollBarInitialWidth, MaximumSize.Height);
                }
                else
                {
                    MinimumSize = new Size(dropDownWidth, 2);
                    MaximumSize = new Size(dropDownWidth, MaximumSize.Height);
                }


                if (Width != MinimumSize.Width)
                    Width = MinimumSize.Width;
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                if (!showScroll)
                    cp.Style = cp.Style & ~NativeMethods.WS_HSCROLL & ~NativeMethods.WS_VSCROLL;

                return cp;
            }
        }

        //public bool ShowScrollbars //Doesn't work
        //{
        //    get { return showScroll; }

        //    set
        //    {
        //        if (showScroll != value)
        //        {
        //            showScroll = value;
        //            if (IsHandleCreated)
        //                RecreateHandle();
        //        }
        //    }
        //}

        public void SuspendPainting()
        {
            processPaintMessages = false;
        }

        public void Repaint()
        {
            processPaintMessages = true;

            Refresh();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == LB_ADDSTRING ||
                m.Msg == LB_INSERTSTRING ||
                m.Msg == LB_DELETESTRING ||
                m.Msg == LB_RESETCONTENT)

                if (ItemsChanged != null)
                    ItemsChanged(this, EventArgs.Empty); //-V3083

            base.WndProc(ref m);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (processPaintMessages)
            {
                base.OnPaint(e);

                if (hScrollBar?.Visible == true)
                    hScrollBar.Invalidate();

                if (vScrollBar?.Visible == true)
                    vScrollBar.Invalidate();
            }
        }

        private void InitializeComponent()
        {
            //Nothing for now...
        }
    }

    public class CustomCheckedListBox : CheckedListBox
    {
        private readonly bool showScroll = true;

        private bool processPaintMessages = true;

        public CustomHScrollBar hScrollBar = null;
        public CustomVScrollBar vScrollBar = null;

        public event EventHandler ItemsChanged;

        public CustomCheckedListBox(bool showScroll)
        {
            this.showScroll = showScroll;

            InitializeComponent();

            //SetStyle(ControlStyles.ResizeRedraw, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //SetStyle(ControlStyles.DoubleBuffer, true);

            //TODO: Add any initialization after the InitForm call
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                if (!showScroll)
                    cp.Style = cp.Style & ~NativeMethods.WS_HSCROLL & ~NativeMethods.WS_VSCROLL;

                return cp;
            }
        }

        //public bool ShowScrollbars //Doesn't work
        //{
        //    get { return showScroll; }

        //    set
        //    {
        //        if (showScroll != value)
        //        {
        //            showScroll = value;
        //            if (IsHandleCreated)
        //                RecreateHandle();
        //        }
        //    }
        //}

        public void SuspendPainting()
        {
            processPaintMessages = false;
        }

        public void Repaint()
        {
            processPaintMessages = true;

            Refresh();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == LB_ADDSTRING ||
                m.Msg == LB_INSERTSTRING ||
                m.Msg == LB_DELETESTRING ||
                m.Msg == LB_RESETCONTENT)

                if (ItemsChanged != null)
                    ItemsChanged(this, EventArgs.Empty); //-V3083

            base.WndProc(ref m);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (processPaintMessages)
            {
                base.OnPaint(e);

                if (hScrollBar?.Visible == true)
                    hScrollBar.Invalidate();

                if (vScrollBar?.Visible == true)
                    vScrollBar.Invalidate();
            }
        }

        private void InitializeComponent()
        {
            //Nothing for now...
        }
    }


    //Returns Minimum, Maximum, LargeChange
    public delegate (int, int, int) GetScrollBarMetricsDelegate(Control scrolledControl, Control refScrollBar);

    //Custom scroll bars
    public class CustomVScrollBar : UserControl
    {
        private readonly float dpiScaling = 1;
        private readonly int initialWidth;
        private int initialHeight;
        private readonly int sbBorderWidth = 0;
        private readonly int reservedBordersSpace = 0;
        private int reservedSpace = 0;
        private readonly int minimumThumbHeight = 5; //5px, will be scaled according to DPI in constructor
        private readonly int scaledPx = 1;
        private readonly int upImageAdditionalTopHeight = 3; //3px, will be scaled according to DPI in constructor
        private readonly int upImageAdditionalBottomHeight = 4; //4px, will be scaled according to DPI in constructor
        private int offsetX = 0;


        public bool SettingParentScroll = false;

        public Control ScrolledControl;

        private readonly GetScrollBarMetricsDelegate GetExternalMetrics;
        private readonly Control refScrollBar;

        protected Image upArrowImage = null;
        //protected Image moUpArrowImage_Over = null;
        //protected Image moUpArrowImage_Down = null;
        protected Image downArrowImage = null;
        //protected Image moDownArrowImage_Over = null;
        //protected Image moDownArrowImage_Down = null;
        protected Image thumbArrowImage = null;

        protected Image thumbTopImage = null;
        protected Image thumbMiddleImage = null;
        protected Image thumbBottomImage = null;

        protected Color scrollBarBackColor;
        protected Color scrollBarBorderColor;
        protected Color narrowScrollBarBackColor;

        protected Color scrollBarThumbAndSpansForeColor;
        protected Color scrollBarThumbAndSpansBackColor;
        protected Color scrollBarThumbAndSpansBorderColor;

        protected Brush scrollBarBackBrush = null;
        protected Brush scrollBarBorderBrush = null;
        protected Brush narrowScrollBarBackBrush = null;
        protected Brush scrollBarThumbAndSpansBackBrush = null;
        protected Brush scrollBarThumbAndSpansBorderBrush = null;

        protected int largeChange = 50;
        protected int smallChange = 10;
        protected int minimum = 0;
        protected int maximum = 100;
        protected int value = 0;
        private int clickPoint;

        protected int thumbTop = 0;

        protected bool autoSize = false;

        private bool thumbDown = false;
        private bool thumbDragging = false;

        public new event EventHandler Scroll;
        public event EventHandler ValueChanged;

        public void CreateBrushes()
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


        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (scrollBarBackBrush != null)
                    scrollBarBackBrush.Dispose();

                if (scrollBarBorderBrush != null)
                    scrollBarBorderBrush.Dispose();

                if (narrowScrollBarBackBrush != null)
                    narrowScrollBarBackBrush.Dispose();

                if (scrollBarThumbAndSpansBackBrush != null)
                    scrollBarThumbAndSpansBackBrush.Dispose();

                if (scrollBarThumbAndSpansBorderBrush != null)
                    scrollBarThumbAndSpansBorderBrush.Dispose();
            }

            base.Dispose(disposing);
        }

        ~CustomVScrollBar()
        {
            Dispose(false);
        }


        //refScrollBar must be null if scrolledControl is not a parent of custom scroll bar & scroll bar must be placed on the right to scrolledControl
        public CustomVScrollBar(Form ownerForm, Control scrolledControl,
            GetScrollBarMetricsDelegate getScrollBarMetrics, int borderWidth, Control refScrollBar = null)
        {
            InitializeComponent();

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);


            scrollBarBackColor = Plugin.ScrollBarBackColor;
            scrollBarBorderColor = Plugin.ScrollBarBorderColor;
            narrowScrollBarBackColor = Plugin.NarrowScrollBarBackColor;

            scrollBarThumbAndSpansForeColor = Plugin.ScrollBarThumbAndSpansForeColor;
            scrollBarThumbAndSpansBackColor = Plugin.ScrollBarThumbAndSpansBackColor;
            scrollBarThumbAndSpansBorderColor = Plugin.ScrollBarThumbAndSpansBorderColor;


            upArrowImage = Plugin.ReplaceBitmap(upArrowImage, Plugin.UpArrowImage);
            downArrowImage = Plugin.ReplaceBitmap(downArrowImage, Plugin.DownArrowImage);

            thumbTopImage = Plugin.ReplaceBitmap(thumbTopImage, Plugin.ThumbTopImage);
            thumbMiddleImage = Plugin.ReplaceBitmap(thumbMiddleImage, Plugin.ThumbMiddleVerticalImage);
            thumbBottomImage = Plugin.ReplaceBitmap(thumbBottomImage, Plugin.ThumbBottomImage);

            GetExternalMetrics = getScrollBarMetrics;
            ScrolledControl = scrolledControl;

            if (refScrollBar == null)
                this.refScrollBar = this;
            else
                this.refScrollBar = refScrollBar;

            if (borderWidth == -1)
                sbBorderWidth = Plugin.SbBorderWidth;
            else
                sbBorderWidth = borderWidth;

            dpiScaling = (ownerForm as PluginWindowTemplate).dpiScaling;
            scaledPx = (ownerForm as PluginWindowTemplate).scaledPx;

            if (ThumbTopImage == null)
                minimumThumbHeight = (int)Math.Round(minimumThumbHeight * dpiScaling);
            else
                minimumThumbHeight = ThumbTopImage.Height * 2;

            upImageAdditionalTopHeight = (int)Math.Round(dpiScaling * upImageAdditionalTopHeight);
            upImageAdditionalBottomHeight = (int)Math.Round(dpiScaling * upImageAdditionalBottomHeight);

            if (refScrollBar != null) //scrolledControlIsParent has own scroll bar CONTROLS, which must be replaced
            {
                Visible = false;

                reservedBordersSpace = 2 * scaledPx;

                initialWidth = ControlsTools.GetCustomScrollBarInitialWidth(dpiScaling, sbBorderWidth);
                Width = refScrollBar.Width;

                ResetMetricsSize(ScrolledControl.Height);
                AdjustReservedSpace(0);

                Margin = new Padding(0, 0, 0, 0);

                ScrolledControl.Controls.Add(this);

                offsetX = Width - initialWidth;

                refScrollBar.Visible = false;

                Top = scaledPx;
                Left = ScrolledControl.Width - Width - scaledPx;
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            }
            else //custom scroll bars will be placed on the right to/below scrolledControlIsParent in the tableLayoutPanel
            {
                Visible = true;

                reservedBordersSpace = 0;

                initialWidth = ControlsTools.GetCustomScrollBarInitialWidth(dpiScaling, sbBorderWidth);
                Width = initialWidth;

                ResetMetricsSize(ScrolledControl.Height);
                AdjustReservedSpace(0);

                var tableLayoutPanel = ScrolledControl.Parent as TableLayoutPanel;

                if (ScrolledControl is CustomListBox)
                    (ScrolledControl as CustomListBox).vScrollBar = this;
                if (ScrolledControl is CustomCheckedListBox)
                    (ScrolledControl as CustomCheckedListBox).vScrollBar = this;
                else if (ScrolledControl is CustomTextBox)
                    (ScrolledControl as CustomTextBox).vScrollBar = this;

                var position = tableLayoutPanel.GetCellPosition(ScrolledControl); //-V3149
                tableLayoutPanel.ColumnStyles[position.Column + 1].Width = Width;
                tableLayoutPanel.RowStyles[position.Row + 1].Height = 0;

                Margin = new Padding(0, 0, 0, 0);
                Anchor = AnchorStyles.Top | AnchorStyles.Left;

                tableLayoutPanel.Controls.Add(this, position.Column + 1, position.Row); //At 2nd column, 1st row

                Dock = DockStyle.Fill;
            }

            smallChange = 25;
            Value = 0;

            SizeChanged += customScrollBar_SizeChanged;
        }

        public void ResetMetricsSize(int newParentHeight)
        {
            initialHeight = newParentHeight - reservedBordersSpace;
            Height = initialHeight - reservedSpace;


            (int nRealRange, int nPixelRange, int nTrackHeight, int nThumbHeight, float fThumbHeight) = GetMetrics();


            MinimumSize = new Size(upArrowImage.Width + 2 * sbBorderWidth,
                2 * (upArrowImage.Height + upImageAdditionalTopHeight
                + upImageAdditionalBottomHeight + sbBorderWidth) + 4 * scaledPx);


            float fValuePerc = ((float)value - minimum) / (maximum - minimum);
            (minimum, maximum, largeChange) = GetExternalMetrics(ScrolledControl, refScrollBar);
            value = (int)Math.Round(fValuePerc * (maximum - minimum)) + minimum;
            if (value < minimum)
                value = minimum;
            else if (value > maximum)
                value = maximum;

            //figure out thumb top
            SetThumbTopFromValueAndLargeChange();

            if (ValueChanged != null)
                ValueChanged(this, null); //-V3083

            Invalidate();
        }

        public void AdjustReservedSpace(int space)
        {
            reservedSpace = space;
            Height = initialHeight - space;

            Invalidate();
        }

        protected void customScrollBar_SizeChanged(object sender, EventArgs e)
        {
            offsetX = Width - initialWidth;
            Invalidate();
        }

        public int LargeChange
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

        public int SmallChange
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

        public int Minimum
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

        public int Maximum
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

        public (int, int, int, int, float) GetMetrics()
        {
            int nTrackHeight = Height - 2 * (upArrowImage.Height
                + upImageAdditionalTopHeight + upImageAdditionalBottomHeight)
                - 2 * sbBorderWidth; //-1 scaled px of top scroll bar border & +1 of bottom scroll bar border
            float fThumbHeight = (float)largeChange / (maximum - minimum) * nTrackHeight;
            int nThumbHeight = (int)Math.Round(fThumbHeight);

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

            int nRealRange = maximum - minimum - largeChange;
            int nPixelRange = nTrackHeight - nThumbHeight;

            return (nRealRange, nPixelRange, nTrackHeight, nThumbHeight, fThumbHeight);
        }

        public int GetThumbTop()
        {
            return thumbTop;
        }

        public void SetThumbTop(int thumbTop)
        {
            if (thumbTop == this.thumbTop)
                return;

            (_, int nPixelRange, _, _, _) = GetMetrics();

            if (thumbTop > nPixelRange)
                this.thumbTop = nPixelRange;
            else if (thumbTop < 0)
                this.thumbTop = 0;
            else
                this.thumbTop = thumbTop;


            //figure out value
            float fPerc = (float)this.thumbTop / nPixelRange;
            float fValue = fPerc * (maximum - minimum);

            value = (int)Math.Round(fValue);

            Invalidate();
        }

        public void SetThumbTopFromValueAndLargeChange()
        {
            (_, int nPixelRange, _, _, _) = GetMetrics();

            float fPerc = 0.0f;
            if (maximum - minimum != 0)
                fPerc = (float)value / (maximum - minimum);

            float fTop = fPerc * nPixelRange;
            thumbTop = (int)Math.Round(fTop);

            Invalidate();
        }

        public void SetValue(int value)
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

        public int Value
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

        public Image UpArrowImage
        {
            get
            {
                return upArrowImage;
            }

            set
            {
                upArrowImage = value;
            }
        }

        public Image DownArrowImage
        {
            get
            {
                return downArrowImage;
            }

            set
            {
                downArrowImage = value;
            }
        }

        public Image ThumbTopImage
        {
            get
            {
                return thumbTopImage;
            }

            set
            {
                thumbTopImage = value;
            }
        }

        public Image ThumbMiddleImage
        {
            get
            {
                return thumbMiddleImage;
            }

            set
            {
                thumbMiddleImage = value;
            }
        }

        public Image ThumbBottomImage
        {
            get
            {
                return thumbBottomImage;
            }

            set
            {
                thumbBottomImage = value;
            }
        }

        private void OnPaint1(PaintEventArgs e, Image upArrowImage, Image downArrowImage,
            Image thumbTopImage, Image thumbMiddleImage, Image thumbBottomImage, bool stretchThumbImage)
        {
            if (stretchThumbImage && thumbMiddleImage == null)
                throw new Exception(Plugin.ExImpossibleToStretchThumb);


            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //let's calculate thumb metrics
            (_, _, _, int nThumbHeight, float fThumbHeight) = GetMetrics();

            //fSpanHeight: +1 scaled px TOP & BOTTOM scroll bar border: total 2 scaled px per BOTH spans
            float fSpanHeight;
            int thumbTopBottomImageHeight = 0;
            if (thumbMiddleImage == null)
            {
                fSpanHeight = fThumbHeight / 2.0f;
            }
            else if (stretchThumbImage && thumbTopImage == null)
            {
                fSpanHeight = fThumbHeight / 2.0f;
            }
            else if (stretchThumbImage)
            {
                thumbTopBottomImageHeight = thumbTopImage.Height; //-V3125 //-V3095
                fSpanHeight = fThumbHeight / 2.0f;
            }
            else //let's draw thumb middle at the vertical center of scroll bar (it's supposed that solid color spans are drawn in this case)
            {
                fSpanHeight = fThumbHeight - thumbMiddleImage.Height / 2.0f;
            }

            int nSpanHeight = (int)Math.Round(fSpanHeight);


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
                    initialWidth - 2 * sbBorderWidth, upArrowImage.Height
                    + upImageAdditionalTopHeight + upImageAdditionalBottomHeight));

            //draw top image
            //+1 scaled px nTop offset due to scroll bar top border
            //-2 scaled px of width for scroll bar left/right borders
            e.Graphics.DrawImage(upArrowImage, new Rectangle(new Point(sbBorderWidth + offsetX,
                sbBorderWidth + upImageAdditionalTopHeight),
                new Size(upArrowImage.Width, upArrowImage.Height)));

            int nTop = thumbTop;
            nTop += upArrowImage.Height + sbBorderWidth //+1 scaled px to top shift below due to scroll bar top border
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
            if (thumbMiddleImage != null)
            {
                if (stretchThumbImage)
                {
                    if (thumbTopImage != null)
                    {
                        e.Graphics.DrawImage(thumbTopImage, new Rectangle(sbBorderWidth + offsetX, nTop,
                            thumbTopImage.Width, thumbTopImage.Height));
                    }

                    nTop += thumbTopBottomImageHeight;

                    Plugin.DrawRepeatedImage(e.Graphics, thumbMiddleImage, sbBorderWidth + offsetX, nTop,
                        thumbMiddleImage.Width, nThumbHeight - 2 * thumbTopBottomImageHeight, true, false);

                    nTop += nThumbHeight - 2 * thumbTopBottomImageHeight;

                    if (thumbTopImage != null) //Both thumbTopImage & thumbBottomImage must be null if thumbTopImage is null
                    {
                        e.Graphics.DrawImage(thumbBottomImage, new Rectangle(sbBorderWidth + offsetX, nTop,
                        thumbBottomImage.Width, thumbBottomImage.Height));
                    }
                }
                else
                {
                    nTop += nSpanHeight;

                    e.Graphics.DrawImage(thumbMiddleImage, new Rectangle(sbBorderWidth + offsetX, nTop,
                        thumbMiddleImage.Width, thumbMiddleImage.Height));
                }
            }


            //draw bottom image background
            if (scrollBarThumbAndSpansBackBrush != null)
                e.Graphics.FillRectangle(scrollBarThumbAndSpansBackBrush, new Rectangle(sbBorderWidth + offsetX,
                    Height - downArrowImage.Height
                    - upImageAdditionalTopHeight - upImageAdditionalBottomHeight - sbBorderWidth,
                    initialWidth - 2 * sbBorderWidth, downArrowImage.Height
                    + upImageAdditionalTopHeight + upImageAdditionalBottomHeight));

            //draw bottom image
            //-1 scaled px vertical offset due to scroll bar bottom border
            //-2 scaled px of width for scroll bar left/right borders
            e.Graphics.DrawImage(downArrowImage, new Rectangle(new Point(sbBorderWidth + offsetX,
                Height - downArrowImage.Height
                - upImageAdditionalTopHeight - sbBorderWidth),
                new Size(downArrowImage.Width, downArrowImage.Height)));
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
            OnPaint1(e, upArrowImage, downArrowImage, null, thumbMiddleImage, null, true);
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
            Name = "CustomScrollBar";
            MouseDown += new MouseEventHandler(CustomScrollBar_MouseDown);
            MouseMove += new MouseEventHandler(CustomScrollBar_MouseMove);
            MouseUp += new MouseEventHandler(CustomScrollBar_MouseUp);
            ResumeLayout(false);
        }

        private void CustomScrollBar_MouseDown(object sender, MouseEventArgs e)
        {
            Point ptPoint = PointToClient(Cursor.Position);

            (int nRealRange, int nPixelRange, int nTrackHeight, int nThumbHeight, float fThumbHeight) = GetMetrics();

            int nTop = thumbTop;
            nTop += upArrowImage.Height + upImageAdditionalTopHeight + upImageAdditionalBottomHeight
                + sbBorderWidth; //+1 scaled px to top shift below due to top border


            Rectangle thumbRect = new Rectangle(new Point(sbBorderWidth + offsetX, nTop),
                new Size(initialWidth - 2 * sbBorderWidth, nThumbHeight));
            Rectangle uparrowRect = new Rectangle(new Point(sbBorderWidth + offsetX, sbBorderWidth),
                new Size(initialWidth - 2 * sbBorderWidth, (upArrowImage.Height
                + upImageAdditionalTopHeight + upImageAdditionalBottomHeight)));
            Rectangle downarrowRect = new Rectangle(new Point(sbBorderWidth + offsetX, upArrowImage.Height
                + upImageAdditionalTopHeight + upImageAdditionalBottomHeight + sbBorderWidth + nTrackHeight),
                new Size(initialWidth - 2 * sbBorderWidth, upArrowImage.Height
                + upImageAdditionalTopHeight + upImageAdditionalBottomHeight));

            if (thumbRect.Contains(ptPoint))
            {
                //hit the thumb
                clickPoint = (ptPoint.Y - nTop);
                thumbDown = true;
            }
            else if (uparrowRect.Contains(ptPoint))
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
                        float fPerc = (float)thumbTop / nPixelRange;
                        float fValue = fPerc * (maximum - minimum);

                        value = (int)Math.Round(fValue);

                        if (ValueChanged != null)
                            ValueChanged(this, null);
                    }
                }
            }
            else if (downarrowRect.Contains(ptPoint))
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
                        float fPerc = (float)thumbTop / nPixelRange;
                        float fValue = fPerc * (maximum - minimum);

                        value = (int)Math.Round(fValue);

                        if (ValueChanged != null)
                            ValueChanged(this, null);
                    }
                }
            }
            else if (thumbRect.Top > ptPoint.Y && thumbRect.Left < ptPoint.X && thumbRect.Left + thumbRect.Width > ptPoint.X)
            {
                float fScrollDistance = Height - 2 * (upArrowImage.Height
                    + upImageAdditionalTopHeight + upImageAdditionalBottomHeight + sbBorderWidth);

                float fScrollPerc = 0.0f;
                if (fScrollDistance != 0)
                    fScrollPerc = ((float)ptPoint.Y - (upArrowImage.Height
                        + upImageAdditionalTopHeight + upImageAdditionalBottomHeight + sbBorderWidth)) / fScrollDistance;

                if (fScrollPerc < 0)
                    fScrollPerc = 0;

                //figure out value
                float fValue = minimum + fScrollPerc * (maximum - minimum);
                value = (int)Math.Round(fValue);

                //figure out thumb top
                SetThumbTopFromValueAndLargeChange();

                if (ValueChanged != null)
                    ValueChanged(this, null);
            }
            else if (thumbRect.Top + thumbRect.Height < ptPoint.Y && thumbRect.Left < ptPoint.X && thumbRect.Left + thumbRect.Width > ptPoint.X)
            {
                float fScrollDistance = Height - 2 * (upArrowImage.Height
                    + upImageAdditionalTopHeight + upImageAdditionalBottomHeight + sbBorderWidth);

                float fScrollPerc = 0.0f;
                if (fScrollDistance != 0)
                    fScrollPerc = 1f - ((float)Height - 2 * (upArrowImage.Height
                        + upImageAdditionalTopHeight + upImageAdditionalBottomHeight + sbBorderWidth) - ptPoint.Y) / fScrollDistance;

                if (fScrollPerc > 1)
                    fScrollPerc = 1;

                //figure out value
                float fValue = minimum + fScrollPerc * (maximum - minimum);
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
        }

        private void MoveThumb(int y)
        {
            (int nRealRange, int nPixelRange, _, _, _) = GetMetrics();

            int nSpot = clickPoint;

            if (thumbDown && nRealRange > 0)
            {
                if (nPixelRange > 0)
                {
                    int nNewThumbTop = y - (upArrowImage.Height
                        + upImageAdditionalTopHeight + upImageAdditionalBottomHeight + sbBorderWidth + nSpot);

                    if (nNewThumbTop < 0)
                        thumbTop = 0;
                    else if (nNewThumbTop > nPixelRange)
                        thumbTop = nPixelRange;
                    else
                        thumbTop = y - (upArrowImage.Height
                            + upImageAdditionalTopHeight + upImageAdditionalBottomHeight + sbBorderWidth + nSpot);

                    //figure out value
                    float fPerc = (float)thumbTop / nPixelRange;
                    float fValue = fPerc * (maximum - minimum);
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

    public class CustomHScrollBar : UserControl
    {
        private readonly float dpiScaling = 1;
        private int initialWidth;
        private readonly int initialHeight;
        private readonly int sbBorderWidth = 0;
        private readonly int reservedBordersSpace = 0;
        private int reservedSpace = 0;
        private readonly int minimumThumbWidth = 5; //5px, will be scaled according to DPI in constructor
        private readonly int scaledPx = 1;
        private readonly int leftImageAdditionalLeftWidth = 3; //3px, will be scaled according to DPI in constructor
        private readonly int leftImageAdditionalRightWidth = 4; //4px, will be scaled according to DPI in constructor
        private int offsetY = 0;


        public bool SettingParentScroll = false;

        public Control ScrolledControl;

        private readonly GetScrollBarMetricsDelegate GetExternalMetrics;
        private readonly Control refScrollBar;

        protected Image leftArrowImage = null;
        //protected Image moLeftArrowImage_Over = null;
        //protected Image moLeftArrowImage_Right = null;
        protected Image rightArrowImage = null;
        //protected Image moRightArrowImage_Over = null;
        //protected Image moRightArrowImage_Right = null;
        protected Image thumbArrowImage = null;

        protected Image thumbLeftImage = null;
        protected Image thumbMiddleImage = null;
        protected Image thumbRightImage = null;

        protected Color scrollBarBackColor;
        protected Color scrollBarBorderColor;
        protected Color narrowScrollBarBackColor;

        protected Color scrollBarThumbAndSpansForeColor;
        protected Color scrollBarThumbAndSpansBackColor;
        protected Color scrollBarThumbAndSpansBorderColor;

        protected Brush scrollBarBackBrush = null;
        protected Brush scrollBarBorderBrush = null;
        protected Brush narrowScrollBarBackBrush = null;
        protected Brush scrollBarThumbAndSpansBackBrush = null;
        protected Brush scrollBarThumbAndSpansBorderBrush = null;

        protected int largeChange = 50;
        protected int smallChange = 10;
        protected int minimum = 0;
        protected int maximum = 100;
        protected int value = 0;
        private int clickPoint;

        protected int thumbLeft = 0;

        protected bool autoSize = false;

        private bool thumbDown = false;
        private bool thumbDragging = false;

        public new event EventHandler Scroll;
        public event EventHandler ValueChanged;

        public void CreateBrushes()
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

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (scrollBarBackBrush != null)
                    scrollBarBackBrush.Dispose();

                if (scrollBarBorderBrush != null)
                    scrollBarBorderBrush.Dispose();

                if (narrowScrollBarBackBrush != null)
                    narrowScrollBarBackBrush.Dispose();

                if (scrollBarThumbAndSpansBackBrush != null)
                    scrollBarThumbAndSpansBackBrush.Dispose();

                if (scrollBarThumbAndSpansBorderBrush != null)
                    scrollBarThumbAndSpansBorderBrush.Dispose();
            }

            base.Dispose(disposing);
        }

        ~CustomHScrollBar()
        {
            Dispose(false);
        }

        //refScrollBar must be null if scrolledControl is not a parent of custom scroll bar & scroll bar must be placed below scrolledControl
        public CustomHScrollBar(Form ownerForm, Control scrolledControl,
            GetScrollBarMetricsDelegate getScrollBarMetrics, Control refScrollBar = null)
        {
            InitializeComponent();

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);


            scrollBarBackColor = Plugin.ScrollBarBackColor;
            scrollBarBorderColor = Plugin.ScrollBarBorderColor;
            narrowScrollBarBackColor = Plugin.NarrowScrollBarBackColor;

            scrollBarThumbAndSpansForeColor = Plugin.ScrollBarThumbAndSpansForeColor;
            scrollBarThumbAndSpansBackColor = Plugin.ScrollBarThumbAndSpansBackColor;
            scrollBarThumbAndSpansBorderColor = Plugin.ScrollBarThumbAndSpansBorderColor;


            leftArrowImage = Plugin.ReplaceBitmap(leftArrowImage, Plugin.LeftArrowImage);
            rightArrowImage = Plugin.ReplaceBitmap(rightArrowImage, Plugin.RightArrowImage);

            thumbLeftImage = Plugin.ReplaceBitmap(thumbLeftImage, Plugin.ThumbLeftImage);
            thumbMiddleImage = Plugin.ReplaceBitmap(thumbMiddleImage, Plugin.ThumbMiddleHorizontalImage);
            thumbRightImage = Plugin.ReplaceBitmap(thumbRightImage, Plugin.ThumbRightImage);

            GetExternalMetrics = getScrollBarMetrics;
            ScrolledControl = scrolledControl;

            if (refScrollBar == null)
                this.refScrollBar = this;
            else
                this.refScrollBar = refScrollBar;

            dpiScaling = (ownerForm as PluginWindowTemplate).dpiScaling;
            scaledPx = (ownerForm as PluginWindowTemplate).scaledPx;

            if (thumbLeftImage == null)
                minimumThumbWidth = (int)Math.Round(minimumThumbWidth * dpiScaling);
            else
                minimumThumbWidth = thumbLeftImage.Width * 2;

            leftImageAdditionalLeftWidth = (int)Math.Round(dpiScaling * leftImageAdditionalLeftWidth);
            leftImageAdditionalRightWidth = (int)Math.Round(dpiScaling * leftImageAdditionalRightWidth);

            if (refScrollBar != null) //scrolledControlIsParent has own scroll bar CONTROLS, which must be replaced
            {
                Visible = false;

                sbBorderWidth = 0;
                reservedBordersSpace = 2 * scaledPx;

                initialHeight = ControlsTools.GetCustomScrollBarInitialWidth(dpiScaling, sbBorderWidth);
                Height = refScrollBar.Height;

                ResetMetricsSize(ScrolledControl.Width);
                AdjustReservedSpace(0);

                Margin = new Padding(0, 0, 0, 0);

                ScrolledControl.Controls.Add(this);

                offsetY = Height - initialHeight;

                refScrollBar.Visible = false;

                Left = scaledPx;
                Top = ScrolledControl.Height - Height - scaledPx;
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            }
            else //custom scroll bars will be placed on the right to/below scrolledControlIsParent
            {
                Visible = true;

                sbBorderWidth = Plugin.SbBorderWidth;
                reservedBordersSpace = 0;

                initialHeight = ControlsTools.GetCustomScrollBarInitialWidth(dpiScaling, sbBorderWidth);
                Height = initialHeight;

                ResetMetricsSize(ScrolledControl.Width);
                AdjustReservedSpace(0);

                var tableLayoutPanel = ScrolledControl.Parent as TableLayoutPanel;

                if (ScrolledControl is CustomListBox)
                    (ScrolledControl as CustomListBox).hScrollBar = this;
                if (ScrolledControl is CustomCheckedListBox)
                    (ScrolledControl as CustomCheckedListBox).hScrollBar = this;

                var position = tableLayoutPanel.GetCellPosition(ScrolledControl); //-V3149
                tableLayoutPanel.ColumnStyles[position.Column + 1].Width = 0;
                tableLayoutPanel.RowStyles[position.Row + 1].Height = Height;

                Margin = new Padding(0, 0, 0, 0);
                Anchor = AnchorStyles.Top | AnchorStyles.Left;

                tableLayoutPanel.Controls.Add(this, position.Column, position.Row + 1); //At 1st column, 2nd row

                Dock = DockStyle.Fill;
            }

            smallChange = 25;
            Value = 0;

            SizeChanged += customScrollBar_SizeChanged;
        }

        public void ResetMetricsSize(int newParentWidth)
        {
            initialWidth = newParentWidth - reservedBordersSpace;
            Width = initialWidth - reservedSpace;


            (int nRealRange, int nPixelRange, int nTrackWidth, int nThumbWidth, float fThumbWidth) = GetMetrics();


            MinimumSize = new Size(2 * (leftArrowImage.Height + leftImageAdditionalLeftWidth
                + leftImageAdditionalRightWidth + sbBorderWidth) + 4 * scaledPx, leftArrowImage.Height + 2 * sbBorderWidth);


            float fValuePerc = ((float)value - minimum) / (maximum - minimum);
            (minimum, maximum, largeChange) = GetExternalMetrics(ScrolledControl, refScrollBar);
            value = (int)Math.Round(fValuePerc * (maximum - minimum)) + minimum;
            if (value < minimum)
                value = minimum;
            else if (value > maximum)
                value = maximum;

            //figure out thumb left
            SetThumbLeftFromValueAndLargeChange();

            if (ValueChanged != null)
                ValueChanged(this, null); //-V3083

            Invalidate();
        }

        public void AdjustReservedSpace(int space)
        {
            reservedSpace = space;
            Width = initialWidth - space;

            Invalidate();
        }


        protected void customScrollBar_SizeChanged(object sender, EventArgs e)
        {
            offsetY = Height - initialHeight;
            Invalidate();
        }

        public int LargeChange
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

        public int SmallChange
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

        public int Minimum
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

        public int Maximum
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

        public (int, int, int, int, float) GetMetrics()
        {
            int nTrackWidth = Width - 2 * (leftArrowImage.Width
                + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth)
                - 2 * sbBorderWidth; //-1 scaled px of left scroll bar border & +1 of right scroll bar border
            float fThumbWidth = (float)largeChange / (maximum - minimum) * nTrackWidth;
            int nThumbWidth = (int)Math.Round(fThumbWidth);

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

            int nRealRange = maximum - minimum - largeChange;
            int nPixelRange = nTrackWidth - nThumbWidth;

            return (nRealRange, nPixelRange, nTrackWidth, nThumbWidth, fThumbWidth);
        }

        public int GetThumbLeft()
        {
            return thumbLeft;
        }

        public void SetThumbLeft(int thumbLeft)
        {
            if (thumbLeft == this.thumbLeft)
                return;

            (_, int nPixelRange, _, _, _) = GetMetrics();

            if (thumbLeft > nPixelRange)
                this.thumbLeft = nPixelRange;
            else if (thumbLeft < 0)
                this.thumbLeft = 0;
            else
                this.thumbLeft = thumbLeft;


            //figure out value
            float fPerc = (float)this.thumbLeft / nPixelRange;
            float fValue = fPerc * (maximum - minimum);

            value = (int)Math.Round(fValue);

            Invalidate();
        }

        public void SetThumbLeftFromValueAndLargeChange()
        {
            (_, int nPixelRange, _, _, _) = GetMetrics();

            float fPerc = 0.0f;
            if (maximum - minimum != 0)
                fPerc = (float)value / (maximum - minimum);

            float fLeft = fPerc * nPixelRange;
            thumbLeft = (int)Math.Round(fLeft);

            Invalidate();
        }

        public void SetValue(int value)
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

        public int Value
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

        public Image LeftArrowImage
        {
            get
            {
                return leftArrowImage;
            }

            set
            {
                leftArrowImage = value;
            }
        }

        public Image RightArrowImage
        {
            get
            {
                return rightArrowImage;
            }

            set
            {
                rightArrowImage = value;
            }
        }

        public Image ThumbLeftImage
        {
            get
            {
                return thumbLeftImage;
            }

            set
            {
                thumbLeftImage = value;
            }
        }

        public Image ThumbMiddleImage
        {
            get
            {
                return thumbMiddleImage;
            }

            set
            {
                thumbMiddleImage = value;
            }
        }

        public Image ThumbRightImage
        {
            get
            {
                return thumbRightImage;
            }

            set
            {
                thumbRightImage = value;
            }
        }

        private void OnPaint1(PaintEventArgs e, Image leftArrowImage, Image rightArrowImage,
            Image thumbLeftImage, Image thumbMiddleImage, Image thumbRightImage, bool stretchThumbImage)
        {
            if (stretchThumbImage && thumbMiddleImage == null)
                throw new Exception(Plugin.ExImpossibleToStretchThumb);


            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //let's calculate thumb metrics
            (_, _, _, int nThumbWidth, float fThumbWidth) = GetMetrics();

            //fSpanWidth: +1 scaled px Left & Right scroll bar border: total 2 scaled px per BOTH spans
            float fSpanWidth;
            int thumbLeftRightImageWidth = 0;
            if (thumbMiddleImage == null)
            {
                fSpanWidth = fThumbWidth / 2.0f;
            }
            else if (stretchThumbImage && thumbLeftImage == null)
            {
                fSpanWidth = fThumbWidth / 2.0f;
            }
            else if (stretchThumbImage)
            {
                thumbLeftRightImageWidth = thumbLeftImage.Width; //-V3125 //-V3095
                fSpanWidth = fThumbWidth / 2.0f;
            }
            else //let's draw thumb middle at the Horizontal center of scroll bar (it's supposed that solid color spans are drawn in this case)
            {
                fSpanWidth = fThumbWidth - thumbMiddleImage.Width / 2.0f;
            }

            int nSpanWidth = (int)Math.Round(fSpanWidth);


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
                    leftArrowImage.Width + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth,
                    initialHeight - 2 * sbBorderWidth));

            //draw left image
            //+1 scaled px nLeft offset due to scroll bar left border
            //-2 scaled px of height for scroll bar top/bottom borders
            e.Graphics.DrawImage(leftArrowImage, new Rectangle(new Point(sbBorderWidth + leftImageAdditionalLeftWidth,
                sbBorderWidth + offsetY),
                new Size(leftArrowImage.Width, leftArrowImage.Height)));

            int nLeft = thumbLeft;
            nLeft += leftArrowImage.Width + sbBorderWidth //+1 scaled px to left shift below due to scroll bar left border
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
            if (thumbMiddleImage != null)
            {
                if (stretchThumbImage)
                {
                    if (thumbLeftImage != null)
                    {
                        e.Graphics.DrawImage(thumbLeftImage, new Rectangle(nLeft, sbBorderWidth + offsetY,
                            thumbLeftImage.Width, thumbLeftImage.Height));
                    }

                    nLeft += thumbLeftRightImageWidth;

                    Plugin.DrawRepeatedImage(e.Graphics, thumbMiddleImage, nLeft, sbBorderWidth + offsetY,
                        nThumbWidth - 2 * thumbLeftRightImageWidth, thumbMiddleImage.Height, false, true);

                    nLeft += nThumbWidth - 2 * thumbLeftRightImageWidth;

                    if (thumbLeftImage != null) //Both thumbLeftImage & thumbRightImage must be null if thumbLeftImage is null
                    {
                        e.Graphics.DrawImage(thumbRightImage, new Rectangle(nLeft, sbBorderWidth + offsetY,
                        thumbRightImage.Width, thumbRightImage.Height));
                    }
                }
                else
                {
                    nLeft += nSpanWidth;

                    e.Graphics.DrawImage(thumbMiddleImage, new Rectangle(nLeft, sbBorderWidth + offsetY,
                        thumbMiddleImage.Width, thumbMiddleImage.Height));
                }
            }


            //draw right image background
            if (scrollBarThumbAndSpansBackBrush != null)
                e.Graphics.FillRectangle(scrollBarThumbAndSpansBackBrush, new Rectangle(Width - rightArrowImage.Width
                    - leftImageAdditionalLeftWidth - leftImageAdditionalRightWidth - sbBorderWidth,
                    sbBorderWidth + offsetY,
                    rightArrowImage.Width + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth,
                    initialHeight - 2 * sbBorderWidth));

            //draw right image
            //-1 scaled px Horizontal offset due to scroll bar right border
            //-2 scaled px of height for scroll bar top/bottom borders
            e.Graphics.DrawImage(rightArrowImage, new Rectangle(new Point(Width - rightArrowImage.Width
                - leftImageAdditionalLeftWidth - sbBorderWidth, sbBorderWidth + offsetY),
                new Size(rightArrowImage.Width, rightArrowImage.Height)));
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
            OnPaint1(e, leftArrowImage, rightArrowImage, null, thumbMiddleImage, null, true);
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
            Name = "CustomScrollBar";
            MouseDown += new MouseEventHandler(CustomScrollBar_MouseDown);
            MouseMove += new MouseEventHandler(CustomScrollBar_MouseMove);
            MouseUp += new MouseEventHandler(CustomScrollBar_MouseUp);
            ResumeLayout(false);
        }

        private void CustomScrollBar_MouseDown(object sender, MouseEventArgs e)
        {
            Point ptPoint = PointToClient(Cursor.Position);

            (int nRealRange, int nPixelRange, int nTrackWidth, int nThumbWidth, float fThumbWidth) = GetMetrics();

            int nLeft = thumbLeft;
            nLeft += leftArrowImage.Width + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth
                + sbBorderWidth; //+1 scaled px to left shift below due to left border


            Rectangle thumbRect = new Rectangle(new Point(nLeft, sbBorderWidth + offsetY),
                new Size(nThumbWidth, initialHeight - 2 * sbBorderWidth));
            Rectangle leftarrowRect = new Rectangle(new Point(sbBorderWidth, sbBorderWidth + offsetY),
                new Size(leftArrowImage.Width + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth,
                initialHeight - 2 * sbBorderWidth));
            Rectangle rightarrowRect = new Rectangle(new Point(leftArrowImage.Width
                + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth + sbBorderWidth + nTrackWidth,
                sbBorderWidth + offsetY),
                new Size(leftArrowImage.Width + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth,
                initialHeight - 2 * sbBorderWidth));

            if (thumbRect.Contains(ptPoint))
            {
                //hit the thumb
                clickPoint = (ptPoint.X - nLeft);
                thumbDown = true;
            }
            else if (leftarrowRect.Contains(ptPoint))
            {
                if (nRealRange > 0)
                {
                    if (nPixelRange > 0)
                    {
                        if (thumbLeft - smallChange < 0)
                            thumbLeft = 0;
                        else
                            thumbLeft -= smallChange;

                        //figure out value
                        float fPerc = (float)thumbLeft / nPixelRange;
                        float fValue = fPerc * (maximum - minimum);

                        value = (int)Math.Round(fValue);

                        if (ValueChanged != null)
                            ValueChanged(this, null);
                    }
                }
            }
            else if (rightarrowRect.Contains(ptPoint))
            {
                if (nRealRange > 0)
                {
                    if (nPixelRange > 0)
                    {
                        if (thumbLeft + smallChange > nPixelRange)
                            thumbLeft = nPixelRange;
                        else
                            thumbLeft += smallChange;

                        //figure out value
                        float fPerc = (float)thumbLeft / nPixelRange;
                        float fValue = fPerc * (maximum - minimum);

                        value = (int)Math.Round(fValue);

                        if (ValueChanged != null)
                            ValueChanged(this, null);
                    }
                }
            }
            else if (thumbRect.Left > ptPoint.X && thumbRect.Top < ptPoint.Y && thumbRect.Top + thumbRect.Height > ptPoint.Y)
            {
                float fScrollDistance = Width - 2 * (leftArrowImage.Width
                    + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth + sbBorderWidth);

                float fScrollPerc = 0.0f;
                if (fScrollDistance != 0)
                    fScrollPerc = ((float)ptPoint.X - (leftArrowImage.Width
                        + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth + sbBorderWidth)) / fScrollDistance;

                if (fScrollPerc < 0)
                    fScrollPerc = 0;

                //figure out value
                float fValue = minimum + fScrollPerc * (maximum - minimum);
                value = (int)Math.Round(fValue);

                //figure out thumb left
                SetThumbLeftFromValueAndLargeChange();

                if (ValueChanged != null)
                    ValueChanged(this, null);
            }
            else if (thumbRect.Left + thumbRect.Width < ptPoint.X && thumbRect.Top < ptPoint.Y && thumbRect.Top + thumbRect.Height > ptPoint.Y)
            {
                float fScrollDistance = Width - 2 * (leftArrowImage.Width
                    + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth + sbBorderWidth);

                float fScrollPerc = 0.0f;
                if (fScrollDistance != 0)
                    fScrollPerc = 1f - ((float)Width - 2 * (leftArrowImage.Width
                        + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth + sbBorderWidth) - ptPoint.X) / fScrollDistance;

                if (fScrollPerc > 1)
                    fScrollPerc = 1;

                //figure out value
                float fValue = minimum + fScrollPerc * (maximum - minimum);
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
        }

        private void MoveThumb(int x)
        {
            (int nRealRange, int nPixelRange, _, _, _) = GetMetrics();

            int nSpot = clickPoint;

            if (thumbDown && nRealRange > 0)
            {
                if (nPixelRange > 0)
                {
                    int nNewThumbLeft = x - (leftArrowImage.Width
                        + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth + sbBorderWidth + nSpot);

                    if (nNewThumbLeft < 0)
                        thumbLeft = 0;
                    else if (nNewThumbLeft > nPixelRange)
                        thumbLeft = nPixelRange;
                    else
                        thumbLeft = x - (leftArrowImage.Width
                            + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth + sbBorderWidth + nSpot);

                    //figure out value
                    float fPerc = (float)thumbLeft / nPixelRange;
                    float fValue = fPerc * (maximum - minimum);
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

public class InterpolatedBox : PictureBox
{
    #region Interpolation Property
    private InterpolationMode interpolation = InterpolationMode.Default;

    public InterpolationMode Interpolation
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
