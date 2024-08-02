using ExtensionMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static NativeMethods;

namespace MusicBeePlugin
{
    public static class ControlsTools
    {
        public static T FindControlChild<T>(Control container) where T : Control
        {
            foreach (Control child in container.Controls)
            {
                if (child is T TChild)
                    return TChild;
            }

            //Not found.
            return null;
        }

        public static List<T> FindControlAllChildren<T>(Control container) where T : Control
        {
            var children = new List<T>();

            foreach (Control child in container.Controls)
            {
                if (child is T TChild)
                    children.Add(TChild);
            }

            return children;
        }

        internal static int GetCustomScrollBarInitialWidth(float dpiScaling, int sbBorderWidth)
        {
            return (int)Math.Round(dpiScaling * Plugin.ScrollBarWidth) + 2 * (int)Math.Round(dpiScaling * sbBorderWidth);
        }

        internal static TextBox CreateMusicBeeTextBox()
        {
            var textBox = (TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);

            if (!Plugin.SavedSettings.dontUseSkinColors)
                textBox.Resize += TextBox_Resize;

            return textBox;
        }

        internal static void TextBox_Resize(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                var textBoxRectangle = new Rectangle(1, 1, textBox.Width - 2, textBox.Height - 2);
                var textBoxRegion = new Region(textBoxRectangle);
                textBox.Region = textBoxRegion;

                textBox.Refresh();
            }
        }

        internal static void DrawTextBoxBorder(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Parent is TextBoxBorder textBoxBorder)
                {
                    using (var graphics = textBoxBorder.CreateGraphics())
                    {
                        if (textBox.Multiline)
                        {
                            if (!textBoxBorder.textBox.Enabled)
                                ControlPaint.DrawBorder(graphics, new Rectangle(0, 0, textBoxBorder.Width, textBoxBorder.Height),
                                    Plugin.ScrollBarBorderColor, ButtonBorderStyle.Solid);
                            else if (textBoxBorder.ContainsFocus)
                                ControlPaint.DrawBorder(graphics, new Rectangle(0, 0, textBoxBorder.Width, textBoxBorder.Height),
                                    Plugin.ScrollBarFocusedBorderColor, ButtonBorderStyle.Solid);
                            else
                                ControlPaint.DrawBorder(graphics, new Rectangle(0, 0, textBoxBorder.Width, textBoxBorder.Height),
                                    Plugin.ScrollBarBorderColor, ButtonBorderStyle.Solid);
                        }
                        else
                        {
                            if (!textBoxBorder.textBox.Enabled)
                                ControlPaint.DrawBorder(graphics, new Rectangle(0, 0, textBoxBorder.Width, textBoxBorder.Height),
                                    Plugin.InputControlBorderColor, ButtonBorderStyle.Solid);
                            else if (textBoxBorder.ContainsFocus)
                                ControlPaint.DrawBorder(graphics, new Rectangle(0, 0, textBoxBorder.Width, textBoxBorder.Height),
                                    Plugin.InputPanelBackColor, ButtonBorderStyle.Solid);
                            else
                                ControlPaint.DrawBorder(graphics, new Rectangle(0, 0, textBoxBorder.Width, textBoxBorder.Height),
                                    Plugin.InputPanelBackColor, ButtonBorderStyle.Solid);
                        }
                    }
                }
            }
        }

        internal static void DrawBorder(Control control)
        {
            if (control.Parent is TextBoxBorder textBoxBorder)
            {
                using (var graphics = control.CreateGraphics())
                {
                    if (!textBoxBorder.textBox.Enabled)
                        ControlPaint.DrawBorder(graphics, new Rectangle(0, 0, textBoxBorder.Width, textBoxBorder.Height),
                            Plugin.InputPanelBorderColor, ButtonBorderStyle.Solid);
                    else if (textBoxBorder.ContainsFocus)
                        ControlPaint.DrawBorder(graphics, new Rectangle(0, 0, textBoxBorder.Width, textBoxBorder.Height),
                            Plugin.ScrollBarFocusedBorderColor, ButtonBorderStyle.Solid);
                    else
                        ControlPaint.DrawBorder(graphics, new Rectangle(0, 0, textBoxBorder.Width, textBoxBorder.Height),
                            Plugin.ScrollBarBorderColor, ButtonBorderStyle.Solid);
                }
            }
        }

        internal static void DrawBorder(Control control, Color color, Color focusedColor, Color disabledColor)
        {
            using (var graphics = control.CreateGraphics())
            {
                if (!control.Enabled)
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
    }

    public sealed class TextBoxBorder : UserControl
    {
        internal readonly TextBox textBox;

        public TextBoxBorder(TextBox textBox)
        {
            if (Plugin.SavedSettings.dontUseSkinColors)
                return;


            this.textBox = textBox;

            ControlsTools.TextBox_Resize(textBox, null);

            Name = textBox.Name;
            Tag = textBox.Tag;
            textBox.Tag = null;

            Location = textBox.Location;
            Size = textBox.Size;
            Anchor = textBox.Anchor;
            Dock = textBox.Dock;
            Margin = textBox.Margin;
            Padding = Padding.Empty;

            TabIndex = textBox.TabIndex;
            TabStop = textBox.TabStop;

            var parent = textBox.Parent;
            var index = parent.Controls.IndexOf(textBox);

            TableLayoutPanelCellPosition cellPosition = default;
            if (parent is TableLayoutPanel panel)
                cellPosition = panel.GetCellPosition(textBox);

            parent.Controls.RemoveAt(index);
            Controls.Add(textBox);

            textBox.Margin = Padding.Empty;
            textBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            textBox.Left = 0;
            textBox.Top = 0;
            textBox.Dock = DockStyle.None;
            textBox.TabStop = false;

            parent.Controls.Add(this);
            parent.Controls.SetChildIndex(this, index);
            if (parent is TableLayoutPanel layoutPanel)
                layoutPanel.SetCellPosition(this, cellPosition);

            Resize += TextBoxBorder_Resize;
            Paint += TextBoxBorder_Paint;
            GotFocus += (sender, args) => { textBox.Focus(); };

            textBox.Paint += ControlsTools.DrawTextBoxBorder;

            ResizeRedraw = true;
        }

        private void TextBoxBorder_Paint(object sender, PaintEventArgs e)
        {
            ControlsTools.DrawTextBoxBorder((sender as TextBoxBorder).textBox, e);
        }

        private void TextBoxBorder_Resize(object sender, EventArgs e)
        {
            textBox.Size = Size;
        }

        private void TextBox_Resize(object sender, EventArgs e)
        {
            Size = textBox.Size;
        }

        protected override void Dispose(bool disposing)
        {
           base.Dispose(disposing);

           if (disposing)
            {
                textBox?.Dispose();
            }
        }

        ~TextBoxBorder()
        {
            Dispose(false);
        }
    }

    public class CustomComboBox : UserControl
    {
        private readonly Color borderColorDisabled = Plugin.ScrollBarBackColor;
        private readonly Color borderColorActive = Plugin.ScrollBarFocusedBorderColor;
        private readonly Color borderColor = Plugin.ScrollBarBackColor;


        public int lastSelectedIndex = -1;

        private int scaledPx = 1;
        private string cue = string.Empty;

        public ComboBox comboBox;

        private TextBox textBox;
        private Button button;
        private CustomListBox listBox;

        private bool textBoxReadOnlyCache;

        private ToolStripDropDown dropDown;
        private static readonly TimeSpan DropDownAutoCloseThreshold = TimeSpan.FromMilliseconds(250);
        private DateTime dropDownClosedTime = DateTime.UtcNow - DropDownAutoCloseThreshold;
        private int initialDropDownWidth = 250;
        private int initialDropDownHeight = 100;


        private static readonly object EVENT_SELECTEDINDEXCHANGED = new object();

        public event EventHandler SelectedIndexChanged
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

        public event EventHandler SelectedItemChanged
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

        public event EventHandler DropDownClosed
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


        private TableLayoutPanel getDropDown(PluginWindowTemplate ownerForm, int customScrollBarInitialWidth)
        {
            var tableLayoutPanel = new TableLayoutPanel();

            tableLayoutPanel.Font = Font;
            tableLayoutPanel.Margin = new Padding(0, 0, 0, 0);
            tableLayoutPanel.Padding = new Padding(0, 0, 0, 0);
            tableLayoutPanel.AutoSize = true;
            tableLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            var columnStyle0 = new ColumnStyle(SizeType.AutoSize, 100f);
            var columnStyle1 = new ColumnStyle(SizeType.Absolute, customScrollBarInitialWidth);

            tableLayoutPanel.ColumnStyles.Add(columnStyle0);
            tableLayoutPanel.ColumnStyles.Add(columnStyle1);

            var rowStyle0 = new RowStyle(SizeType.AutoSize, 100f);
            var rowStyle1 = new RowStyle(SizeType.Absolute, 0f);

            tableLayoutPanel.RowStyles.Add(rowStyle0);
            tableLayoutPanel.RowStyles.Add(rowStyle1);

            
            listBox = new CustomListBox(customScrollBarInitialWidth - (int)Math.Round((ownerForm.dpiScaling - 1) * 8), scaledPx, false, 
                Plugin.ScrollBarBackColor, Plugin.ScrollBarBackColor, Plugin.ScrollBarBackColor);

            listBox.Font = Font;
            listBox.MultiColumn = false;
            listBox.IntegralHeight = false;
            listBox.Margin = new Padding(0, 0, 0, 0);
            listBox.Dock = DockStyle.Fill;
            listBox.TabStop = false;

            listBox.SelectedIndex = -1;

            tableLayoutPanel.Controls.Add(listBox, 0, 0);

            ownerForm.skinControl(listBox, Plugin.SbBorderWidth);

            return tableLayoutPanel;
        }

        public CustomComboBox(PluginWindowTemplate ownerForm, ComboBox comboBoxRef, bool skinned)
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //SetStyle(ControlStyles.DoubleBuffer, true);

            if (skinned)
                initSkinned(ownerForm, comboBoxRef);
            else
                init(ownerForm, comboBoxRef);

            EnabledChanged += OnEnabledChanged;
            VisibleChanged += OnVisibleChanged;
        }

        public CustomComboBox(PluginWindowTemplate ownerForm, ComboBox comboBoxRef, bool skinned, Color borderColor, Color borderColorDisabled, Color borderColorActive)
            : this(ownerForm, comboBoxRef, skinned)
        {
            this.borderColorDisabled = borderColorDisabled;
            this.borderColorActive = borderColorActive;
            this.borderColor = borderColor;
        }


        private void init(PluginWindowTemplate ownerForm, ComboBox comboBox)
        {
            this.Paint += (sender, args) => { ControlsTools.DrawBorder(this, borderColor, borderColorActive, borderColorDisabled); };
            
            scaledPx = ownerForm.scaledPx;
            textBox = null;
            button = null;
            listBox = null;
            dropDown = null;
            initialDropDownWidth = comboBox.DropDownWidth;

            var parent = comboBox.Parent;
            var index = parent.Controls.IndexOf(comboBox);
            TableLayoutPanelCellPosition cellPosition = default;
            var location = comboBox.Location;

            if (parent is TableLayoutPanel panel)
                cellPosition = panel.GetCellPosition(comboBox);

            Font = comboBox.Font;
            Margin = comboBox.Margin;
            Padding = new Padding(0, 0, 0, 0);

            Anchor = comboBox.Anchor;
            TabIndex = comboBox.TabIndex;
            TabStop = comboBox.TabStop;
            Tag = comboBox.Tag;
            Name = comboBox.Name;


            this.comboBox = (ComboBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.ComboBox);
            this.comboBox.DropDownStyle = comboBox.DropDownStyle;

            foreach (var item in comboBox.Items)
                this.comboBox.Items.Add(item);

            this.comboBox.Font = Font;

            this.comboBox.Location = new Point(0, 0);
            this.comboBox.Margin = new Padding(0, 0, 0, 0);
            this.comboBox.Size = new Size(comboBox.Width, comboBox.Height);
            this.comboBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            Size = new Size(this.comboBox.Width, this.comboBox.Height + 1);

            Controls.Add(this.comboBox);

            DropDownStyle = comboBox.DropDownStyle;


            CopyComboBoxEventHandlers(comboBox, this.comboBox);


            SizeChanged += customComboBox_SizeChanged;
            customComboBox_SizeChanged(this, null);

            var allControlsIndex = ownerForm.allControls.IndexOf(comboBox);
            ownerForm.allControls.RemoveAt(allControlsIndex);
            ownerForm.allControls.Insert(allControlsIndex, this);

            parent.Controls.RemoveAt(index);
            comboBox.Dispose();

            parent.Controls.Add(this);
            parent.Controls.SetChildIndex(this, index);
            if (parent is TableLayoutPanel layoutPanel)
                layoutPanel.SetCellPosition(this, cellPosition);

            Location = location;

            ownerForm.namesComboBoxes.Add(Name, this);
        }

        private void initSkinned(PluginWindowTemplate ownerForm, ComboBox comboBox)
        {
            scaledPx = ownerForm.scaledPx;
            this.comboBox = null;

            initialDropDownWidth = comboBox.DropDownWidth;
            //if (comboBox.DropDownWidth < comboBox.Width)
            //    initialDropDownWidth = comboBox.Width;

            var dropDownWidth = comboBox.DropDownWidth;
            if (comboBox.DropDownWidth < comboBox.Width)
                dropDownWidth = comboBox.Width;

            initialDropDownHeight = comboBox.DropDownHeight + 3 * scaledPx;
            if (comboBox.DropDownHeight == 106)
                initialDropDownHeight = comboBox.ItemHeight * 20 + 3 * scaledPx;

            var parent = comboBox.Parent;
            var index = parent.Controls.IndexOf(comboBox);
            TableLayoutPanelCellPosition cellPosition = default;
            var location = comboBox.Location;

            if (parent is TableLayoutPanel panel)
                cellPosition = panel.GetCellPosition(comboBox);

            Font = comboBox.Font;
            ForeColor = Plugin.InputControlForeColor;
            BackColor = Plugin.InputControlBackColor;
            Margin = comboBox.Margin;
            Padding = new Padding(0, 0, 0, 0);
            Anchor = comboBox.Anchor;
            TabIndex = comboBox.TabIndex;
            TabStop = comboBox.TabStop;
            Tag = comboBox.Tag;
            Name = comboBox.Name;


            var customScrollBarInitialWidth = ControlsTools.GetCustomScrollBarInitialWidth(ownerForm.dpiScaling, Plugin.SbBorderWidth);

            textBox = (TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);

            textBox.Font = Font;
            textBox.Location = new Point(0, 0);
            textBox.Margin = new Padding(0, 0, 0, 0);
            textBox.Size = new Size(comboBox.Width - customScrollBarInitialWidth - 1, textBox.Height);
            textBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            textBox.TabStop = false;

            if (comboBox.DropDownStyle == ComboBoxStyle.DropDownList)
                textBox.Click += button_Click;

            textBox.KeyPress += ownerForm.CustomComboBox_KeyPress;
            textBox.KeyDown += ownerForm.CustomComboBox_KeyDown;

            Size = new Size(comboBox.Width, textBox.Height);


            button = new Button();
            button.Font = Font;

            button.Margin = new Padding(0, 0, 0, 0);

            button.Location = new Point(comboBox.Width - customScrollBarInitialWidth - 1, 1);
            button.Size = new Size(customScrollBarInitialWidth, textBox.Height - 2);


            button.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            button.Text = string.Empty;
            button.ImageAlign = ContentAlignment.MiddleCenter;
            button.TextImageRelation = TextImageRelation.Overlay;
            button.Image = Plugin.CopyBitmap(Plugin.DownArrowComboBoxImage);
            button.TabStop = false;

            //button.Click += button_Click;//****
            button.GotFocus += button_Click;


            Controls.Add(button);
            Controls.Add(textBox);

            ownerForm.nonDefaultingButtons.Add(button);
            

            ownerForm.skinControl(button);
            button.FlatAppearance.BorderColor = Plugin.ScrollBarBackColor;

            ownerForm.skinControl(textBox);


            var textBoxRectangle = new Rectangle(1, 1, textBox.Width - 2, textBox.Height - 2);
            var textBoxRegion = new Region(textBoxRectangle);
            textBox.Region = textBoxRegion;


            var dropDownControl = getDropDown(ownerForm, customScrollBarInitialWidth);
            var controlHost = new ToolStripControlHost(dropDownControl);
            controlHost.Font = Font;
            controlHost.AutoSize = true;
            controlHost.Padding = new Padding(0, 0, 0, 0);
            controlHost.Margin = new Padding(0, 0, 0, 0);


            dropDown = new ToolStripDropDown();
            dropDown.Font = Font;
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

            listBox.AdjustHeight(dropDownWidth, initialDropDownHeight);


            CopyComboBoxEventHandlers(comboBox, this);


            GotFocus += CustomComboBox_GotFocus;
            SizeChanged += customComboBox_SizeChanged;
            customComboBox_SizeChanged(this, null);

            DropDownStyle = comboBox.DropDownStyle;

            var allControlsIndex = ownerForm.allControls.IndexOf(comboBox);
            ownerForm.allControls.RemoveAt(allControlsIndex);
            ownerForm.allControls.Insert(allControlsIndex, this);

            parent.Controls.RemoveAt(index);
            comboBox.Dispose();


            parent.Controls.Add(this);
            parent.Controls.SetChildIndex(this, index);
            if (parent is TableLayoutPanel layoutPanel)
                layoutPanel.SetCellPosition(this, cellPosition);

            Location = location;

            ownerForm.namesComboBoxes.Add(Name, this);
        }

        private void CustomComboBox_GotFocus(object sender, EventArgs e)
        {
            textBox.Focus();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                textBox?.Dispose();
                button?.Dispose();
                listBox?.Dispose();
                dropDown?.Dispose();
                comboBox?.Dispose();
            }
        }

        ~CustomComboBox()
        {
            Dispose(false);
        }


        public int IndexOfText(string text)
        {
            var index = SelectedIndex;

            if (index >= 0 && Items[index].ToString() == text)
                return index;

            for (var i = 0; i < Items.Count; i++)
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
                    return listBox.Items;
                else
                    return comboBox.Items;
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
                if (comboBox == null && ReadOnly && button.IsEnabled())
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
            var index = IndexOfText(value);

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

                    Events[EVENT_SELECTEDINDEXCHANGED]?.DynamicInvoke(this, null);
                }
                else if (comboBox != null && comboBox.SelectedIndex != value)
                {
                    comboBox.SelectedIndex = value;

                    if (value != -1)
                        Text = comboBox.Items[value].ToString();
                    else if (DropDownStyle == ComboBoxStyle.DropDownList)
                        Text = string.Empty;

                    Events[EVENT_SELECTEDINDEXCHANGED]?.DynamicInvoke(this, null);
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

                    Events[EVENT_SELECTEDITEMCHANGED]?.DynamicInvoke(this, null);
                }
                else if (comboBox != null && comboBox.SelectedItem != value)
                {
                    comboBox.SelectedItem = value;

                    if (value != null)
                        Text = value.ToString();

                    Events[EVENT_SELECTEDITEMCHANGED]?.DynamicInvoke(this, null);
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

        private void OnVisibleChanged(object sender, EventArgs e)
        {
            if (comboBox == null)
            {
                textBox.Visible = Visible;
                button.Visible = Visible;
            }
            else
            {
                comboBox.Visible = Visible;
            }
        }

        internal void OnEnabledChanged(object sender, EventArgs e)
        {
            comboBox?.Enable(Enabled);
            textBox?.Enable(Enabled);
            button?.Enable(Enabled);
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
                    ;
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

        private void CopyComboBoxEventHandlers(ComboBox refComboBox, Control destControl)
        {
            if (destControl is CustomComboBox customComboBox)
            {
                refComboBox.CopyEventHandlersTo(customComboBox.textBox, "TextChanged", false);
                refComboBox.CopyEventHandlersTo(destControl, "SelectedIndexChanged", false);
                refComboBox.CopyEventHandlersTo(destControl, "SelectedItemChanged", false);
                refComboBox.CopyEventHandlersTo(destControl, "VisibleChanged", false);
                refComboBox.CopyEventHandlersTo(destControl, "EnabledChanged", false);
                refComboBox.CopyEventHandlersTo(destControl, "DropDownClosed", false);
            }
            else
            {
                refComboBox.CopyEventHandlersTo(destControl, "TextChanged", false);
                refComboBox.CopyEventHandlersTo(destControl, "SelectedIndexChanged", false);
                refComboBox.CopyEventHandlersTo(destControl, "SelectedItemChanged", false);
                refComboBox.CopyEventHandlersTo(destControl, "VisibleChanged", false);
                refComboBox.CopyEventHandlersTo(destControl, "EnabledChanged", false);
                refComboBox.CopyEventHandlersTo(destControl, "DropDownClosed", false);
            }
        }

        private void customComboBox_SizeChanged(object sender, EventArgs e)
        {
            if (comboBox != null)
            {
                if (Width < initialDropDownWidth)
                    comboBox.DropDownWidth = initialDropDownWidth;
                else
                    comboBox.DropDownWidth = Width;
            }
        }

        private void listBox_ItemsChanged(object sender, EventArgs e)
        {
            var dropDownWidth = Width;
            if (dropDownWidth < initialDropDownWidth)
                dropDownWidth = initialDropDownWidth;

            listBox.AdjustHeight(dropDownWidth, initialDropDownHeight);

            PluginWindowTemplate.UpdateCustomScrollBars(listBox);
        }

        public void listBox_ItemChosen(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex >= 0)
            {
                var index = listBox.SelectedIndex;
                listBox.SelectedIndex = -1;
                textBox.Focus();
                dropDown.Close();

                SelectedIndex = index;
            }
            else
            {
                textBox.Focus();
                dropDown.Close();
            }
        }

        private void listBox_MouseMove(object sender, MouseEventArgs e)
        {
            var index = listBox.IndexFromPoint(e.X, e.Y);

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

            Events[EVENT_DROPDOWNCLOSED]?.DynamicInvoke(this, null);
        }

        private void button_Click(object sender, EventArgs e)
        {
            if (!button.IsEnabled())
                return;


            if (DateTime.UtcNow - dropDownClosedTime > DropDownAutoCloseThreshold)
            {
                var dropDownWidth = Width;
                if (dropDownWidth < initialDropDownWidth)
                    dropDownWidth = initialDropDownWidth;

                if (listBox.GetItemsHeight() > initialDropDownHeight)
                    listBox.AdjustWidth(dropDownWidth, true);
                else
                    listBox.AdjustWidth(dropDownWidth, false);


                listBox.SelectedIndex = lastSelectedIndex;

                var textBoxScreenLocation = textBox.PointToScreen(textBox.Location);

                //try to position _dropDown below textbox
                var pt = textBoxScreenLocation;
                pt.Offset(0, textBox.Height);

                //determine if it will fit on the screen below the textbox
                var dropdownSize = dropDown.GetPreferredSize(Size.Empty);
                var dropdownBounds = new Rectangle(pt, dropdownSize);

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
                    SetText(string.Empty);
            }
            else
            {
                comboBox.SetCue(cue);
            }
        }
    }

    public class CustomTextBox : TextBox
    {
        public CustomVScrollBar vScrollBar;

        public CustomTextBox(Color borderColor, Color borderColorDisabled, Color borderColorActive)
        {
            var thisRectangle = new Rectangle(1, 1, Width - 2, Height - 2);
            var thisRegion = new Region(thisRectangle);
            Region = thisRegion;

            Paint += (sender, args) => { ControlsTools.DrawBorder(this, borderColor, borderColorActive, borderColorDisabled); };
        }

        public int ScrollPosition { get; set; }

        public void UpdateCaretPositionForScrolling(int position)
        {
            ScrollPosition = GetLineFromCharIndex(position);
        }

        public void Scroll(int delta)
        {
            ScrollPosition += delta;
            SendMessage(Handle, EM_LINESCROLL, Zero, (IntPtr)delta);
        }

        public void ScrollToTop()
        {
            SendMessage(Handle, EM_LINESCROLL, Zero, (IntPtr)(-100000));
            vScrollBar.SetValue(0);
            vScrollBar.Invalidate();
            Invalidate();
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

    public class CustomListBox : ListBox
    {
        private readonly Color borderColorDisabled = Plugin.ScrollBarBorderColor;
        private readonly Color borderColorActive = Plugin.ScrollBarFocusedBorderColor;
        private readonly Color borderColor = Plugin.ScrollBarBorderColor;

        private readonly int scaledPx = -10000;
        private readonly int customScrollBarInitialWidth;

        private readonly bool showScroll;
        private bool processPaintMessages = true;

        public CustomHScrollBar hScrollBar;
        public CustomVScrollBar vScrollBar;

        public event EventHandler ItemsChanged;

        public CustomListBox(bool showScroll)
        {
            this.showScroll = showScroll;

            InitializeComponent();

            SetStyle(ControlStyles.ResizeRedraw, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //SetStyle(ControlStyles.DoubleBuffer, true);

            //TODO: Add any initialization after the InitForm call
        }

        public CustomListBox(int customScrollBarInitialWidth, int scaledPx, bool showScroll) : this(false)
        {
            this.showScroll = showScroll;
            this.customScrollBarInitialWidth = customScrollBarInitialWidth;
            this.scaledPx = scaledPx;
        }

        public CustomListBox(int customScrollBarInitialWidth, int scaledPx, bool showScroll, Color borderColor, Color borderColorDisabled, Color borderColorActive) 
            : this(customScrollBarInitialWidth, scaledPx, showScroll)
        {
            this.borderColorDisabled = borderColorDisabled;
            this.borderColorActive = borderColorActive;
            this.borderColor = borderColor;
        }

        public int GetItemsHeight()
        {
            return ItemHeight * Items.Count + 3 * scaledPx;
        }

        public void AdjustHeight(int dropDownWidth, int initialDropDownHeight)
        {
            if (scaledPx > 0)
            {
                bool scrollBarVisible;
                var itemsHeight = GetItemsHeight();

                if (itemsHeight > initialDropDownHeight)
                {
                    // ReSharper disable once PossibleNullReferenceException
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if ((Parent as TableLayoutPanel).ColumnStyles[1].Width != vScrollBar.Width)
                        // ReSharper disable once PossibleNullReferenceException
                        (Parent as TableLayoutPanel).ColumnStyles[1].Width = vScrollBar.Width;

                    if (MaximumSize.Height != initialDropDownHeight)
                    {
                        MaximumSize = new Size(MaximumSize.Width, initialDropDownHeight);
                        Height = initialDropDownHeight;
                        vScrollBar.ResetMetricsSize(initialDropDownHeight);
                    }

                    scrollBarVisible = true;
                }
                else
                {
                    // ReSharper disable once PossibleNullReferenceException
                    if ((Parent as TableLayoutPanel).ColumnStyles[1].Width != 0)
                        // ReSharper disable once PossibleNullReferenceException
                        (Parent as TableLayoutPanel).ColumnStyles[1].Width = 0;

                    if (MaximumSize.Height != itemsHeight)
                    {
                        MaximumSize = new Size(MaximumSize.Width, itemsHeight);
                        Height = itemsHeight;
                    }

                    scrollBarVisible = false;
                }

                vScrollBar.Visible = scrollBarVisible;
                AdjustWidth(dropDownWidth, scrollBarVisible);
            }
        }

        public void AdjustWidth(int dropDownWidth, bool scrollBarVisible)
        {
            if (scrollBarVisible)
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

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;

                if (!showScroll)
                {
                    cp.ExStyle &= ~WS_EX_CLIENTEDGE;
                    cp.Style |= WS_BORDER;

                    cp.Style = cp.Style & ~WS_HSCROLL & ~WS_VSCROLL;
                }

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

            if (m.Msg == WM_NCPAINT && !showScroll) 
            {
                WmNcPaint(ref m);
                return;
            }            
            
            base.WndProc(ref m);
        }

        private void DrawBorder(Graphics g)
        {
            if (!Enabled)
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
            if (processPaintMessages)
            {
                base.OnPaint(e);
                DrawBorder(e.Graphics);

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

    public class CustomCheckedListBox : CheckedListBox
    {
        private readonly Color borderColorDisabled = Plugin.ScrollBarBorderColor;
        private readonly Color borderColorActive = Plugin.ScrollBarFocusedBorderColor;
        private readonly Color borderColor = Plugin.ScrollBarBorderColor;

        private readonly bool showScroll;

        private bool processPaintMessages = true;

        public CustomHScrollBar hScrollBar;
        public CustomVScrollBar vScrollBar;

        public event EventHandler ItemsChanged;

        public CustomCheckedListBox(bool showScroll)
        {
            this.showScroll = showScroll;

            InitializeComponent();

            SetStyle(ControlStyles.ResizeRedraw, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //SetStyle(ControlStyles.DoubleBuffer, true);

            //TODO: Add any initialization after the InitForm call
        }

        public CustomCheckedListBox(bool showScroll, Color borderColor, Color borderColorDisabled, Color borderColorActive) 
            : this(showScroll)
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

                if (!showScroll)
                {
                    cp.ExStyle &= ~WS_EX_CLIENTEDGE;
                    cp.Style |= WS_BORDER;

                    cp.Style = cp.Style & ~WS_HSCROLL & ~WS_VSCROLL;
                }

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

            if (m.Msg == WM_NCPAINT && !showScroll) 
            {
                WmNcPaint(ref m);
                return;
            }            
            
            base.WndProc(ref m);
        }

        private void DrawBorder(Graphics g)
        {
            if (!Enabled)
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
            if (processPaintMessages)
            {
                base.OnPaint(e);
                //DrawBorder(e.Graphics);

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


    //Returns Minimum, Maximum, LargeChange
    public delegate (int, int, int) GetScrollBarMetricsDelegate(Control scrolledControl, Control refScrollBar);

    //Custom scroll bars
    public sealed class CustomVScrollBar : UserControl
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

        public bool SettingParentScroll;

        public Control ScrolledControl;

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

        private int largeChange = 50;
        private int smallChange;
        private int minimum;
        private int maximum = 100;
        private int value;
        private int clickPoint;

        private int thumbTop;

        private bool thumbDown;
        private bool thumbDragging;

        private bool upArrowDown;
        private bool downArrowDown;

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


            UpArrowImage = Plugin.CopyBitmap(Plugin.UpArrowImage);
            DownArrowImage = Plugin.CopyBitmap(Plugin.DownArrowImage);

            ThumbTopImage = Plugin.CopyBitmap(Plugin.ThumbTopImage);
            ThumbMiddleImage = Plugin.CopyBitmap(Plugin.ThumbMiddleVerticalImage);
            ThumbBottomImage = Plugin.CopyBitmap(Plugin.ThumbBottomImage);

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

            if (refScrollBar != null) //scrolledControlIsParent has own scroll bar CONTROLS, which must be replaced
            {
                this.refScrollBar = refScrollBar;
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

                Margin = new Padding(0, 0, 0, 0);
                Anchor = AnchorStyles.Top | AnchorStyles.Left;

                tableLayoutPanel.Controls.Add(this, position.Column + 1, position.Row); //At 2nd column, 1st row

                Dock = DockStyle.Fill;
            }

            smallChange = 5; //****
            Value = 0;

            SizeChanged += customScrollBar_SizeChanged;
        }

        public void ResetMetricsSize(int newParentHeight)
        {
            initialHeight = newParentHeight - reservedBordersSpace;
            Height = initialHeight - reservedSpace;


            var (nRealRange, nPixelRange, nTrackHeight, nThumbHeight, fThumbHeight) = GetMetrics();


            MinimumSize = new Size(UpArrowImage.Width + 2 * sbBorderWidth,
                2 * (UpArrowImage.Height + upImageAdditionalTopHeight
                + upImageAdditionalBottomHeight + sbBorderWidth) + 4 * scaledPx);


            var fValuePerc = ((float)value - minimum) / (maximum - minimum);
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

        private void customScrollBar_SizeChanged(object sender, EventArgs e)
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
            var nTrackHeight = Height - 2 * (UpArrowImage.Height
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

        public int GetThumbTop()
        {
            return thumbTop;
        }

        public void SetThumbTop(int thumbTop)
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

        public void SetThumbTopFromValueAndLargeChange()
        {
            var (_, nPixelRange, _, _, _) = GetMetrics();

            var fPerc = 0.0f;
            if (maximum - minimum != 0)
                fPerc = (float)value / (maximum - minimum);

            var fTop = fPerc * nPixelRange;
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

        public Image UpArrowImage { get; set; }

        public Image DownArrowImage { get; set; }

        public Image ThumbTopImage { get; set; }

        public Image ThumbMiddleImage { get; set; }

        public Image ThumbBottomImage { get; set; }

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

            Plugin.MbForm.Invoke(new Action(() => {
                if (ValueChanged != null)
                    ValueChanged(this, null);
            }));
        }

        private void CustomScrollBar_MouseDown(object sender, MouseEventArgs e)
        {
            var ptPoint = PointToClient(Cursor.Position);

            var (nRealRange, nPixelRange, nTrackHeight, nThumbHeight, fThumbHeight) = GetMetrics();

            var nTop = thumbTop;
            nTop += UpArrowImage.Height + upImageAdditionalTopHeight + upImageAdditionalBottomHeight
                + sbBorderWidth; //+1 scaled px to top shift below due to top border


            var thumbRect = new Rectangle(new Point(sbBorderWidth + offsetX, nTop),
                new Size(initialWidth - 2 * sbBorderWidth, nThumbHeight));

            var uparrowRect = new Rectangle(new Point(sbBorderWidth + offsetX, sbBorderWidth),
                new Size(initialWidth - 2 * sbBorderWidth, (UpArrowImage.Height
                + upImageAdditionalTopHeight + upImageAdditionalBottomHeight)));

            var downarrowRect = new Rectangle(new Point(sbBorderWidth + offsetX, UpArrowImage.Height
                + upImageAdditionalTopHeight + upImageAdditionalBottomHeight + sbBorderWidth + nTrackHeight),
                new Size(initialWidth - 2 * sbBorderWidth, UpArrowImage.Height
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
                thumbUpDownRepeater = new System.Threading.Timer(moveThumbUpDown, null, 0, 50);
            }
            else if (downarrowRect.Contains(ptPoint))
            {
                downArrowDown = true;
                thumbUpDownRepeater = new System.Threading.Timer(moveThumbUpDown, null, 0, 50);
            }
            else if (thumbRect.Top > ptPoint.Y && thumbRect.Left < ptPoint.X && thumbRect.Left + thumbRect.Width > ptPoint.X)
            {
                float fScrollDistance = Height - 2 * (UpArrowImage.Height
                    + upImageAdditionalTopHeight + upImageAdditionalBottomHeight + sbBorderWidth);

                var fScrollPerc = 0.0f;
                if (fScrollDistance != 0)
                    fScrollPerc = ((float)ptPoint.Y - (UpArrowImage.Height
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
                float fScrollDistance = Height - 2 * (UpArrowImage.Height
                    + upImageAdditionalTopHeight + upImageAdditionalBottomHeight + sbBorderWidth);

                var fScrollPerc = 0.0f;
                if (fScrollDistance != 0)
                    fScrollPerc = 1f - ((float)Height - 2 * (UpArrowImage.Height
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
                    var nNewThumbTop = y - (UpArrowImage.Height
                                            + upImageAdditionalTopHeight + upImageAdditionalBottomHeight + sbBorderWidth + nSpot);

                    if (nNewThumbTop < 0)
                        thumbTop = 0;
                    else if (nNewThumbTop > nPixelRange)
                        thumbTop = nPixelRange;
                    else
                        thumbTop = y - (UpArrowImage.Height
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

    public sealed class CustomHScrollBar : UserControl
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

        public bool SettingParentScroll;

        public Control ScrolledControl;

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

        private int largeChange = 50;
        private int smallChange;
        private int minimum;
        private int maximum = 100;
        private int value;
        private int clickPoint;

        private int thumbLeft;

        private bool thumbDown;
        private bool thumbDragging;

        private bool leftArrowDown;
        private bool rightArrowDown;

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


            LeftArrowImage = Plugin.CopyBitmap(Plugin.LeftArrowImage);
            RightArrowImage = Plugin.CopyBitmap(Plugin.RightArrowImage);

            ThumbLeftImage = Plugin.CopyBitmap(Plugin.ThumbLeftImage);
            ThumbMiddleImage = Plugin.CopyBitmap(Plugin.ThumbMiddleHorizontalImage);
            ThumbRightImage = Plugin.CopyBitmap(Plugin.ThumbRightImage);

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

            if (refScrollBar != null) //scrolledControlIsParent has own scroll bar CONTROLS, which must be replaced
            {
                this.refScrollBar = refScrollBar;
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

                Margin = new Padding(0, 0, 0, 0);
                Anchor = AnchorStyles.Top | AnchorStyles.Left;

                tableLayoutPanel.Controls.Add(this, position.Column, position.Row + 1); //At 1st column, 2nd row

                Dock = DockStyle.Fill;
            }

            smallChange = 5; //****
            Value = 0;

            SizeChanged += customScrollBar_SizeChanged;
        }

        public void ResetMetricsSize(int newParentWidth)
        {
            initialWidth = newParentWidth - reservedBordersSpace;
            Width = initialWidth - reservedSpace;


            var (nRealRange, nPixelRange, nTrackWidth, nThumbWidth, fThumbWidth) = GetMetrics();


            MinimumSize = new Size(2 * (LeftArrowImage.Height + leftImageAdditionalLeftWidth
                + leftImageAdditionalRightWidth + sbBorderWidth) + 4 * scaledPx, LeftArrowImage.Height + 2 * sbBorderWidth);


            var fValuePerc = ((float)value - minimum) / (maximum - minimum);
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


        private void customScrollBar_SizeChanged(object sender, EventArgs e)
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
            var nTrackWidth = Width - 2 * (LeftArrowImage.Width
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

        public int GetThumbLeft()
        {
            return thumbLeft;
        }

        public void SetThumbLeft(int thumbLeft)
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

        public void SetThumbLeftFromValueAndLargeChange()
        {
            var (_, nPixelRange, _, _, _) = GetMetrics();

            var fPerc = 0.0f;
            if (maximum - minimum != 0)
                fPerc = (float)value / (maximum - minimum);

            var fLeft = fPerc * nPixelRange;
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

        public Image LeftArrowImage { get; set; }

        public Image RightArrowImage { get; set; }

        public Image ThumbLeftImage { get; set; }

        public Image ThumbMiddleImage { get; set; }

        public Image ThumbRightImage { get; set; }

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
                            thumbLeft -= smallChange;

                        //figure out value
                        var fPerc = (float)thumbLeft / nPixelRange;
                        var fValue = fPerc * (maximum - minimum);

                        value = (int)Math.Round(fValue);

                        if (ValueChanged != null)
                            ValueChanged(this, null);
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
                            thumbLeft += smallChange;

                        //figure out value
                        var fPerc = (float)thumbLeft / nPixelRange;
                        var fValue = fPerc * (maximum - minimum);

                        value = (int)Math.Round(fValue);

                        if (ValueChanged != null)
                            ValueChanged(this, null);
                    }
                }
            }

            Plugin.MbForm.Invoke(new Action(() => {
                if (ValueChanged != null)
                    ValueChanged(this, null);
            }));
        }

        private void CustomScrollBar_MouseDown(object sender, MouseEventArgs e)
        {
            var ptPoint = PointToClient(Cursor.Position);

            var (nRealRange, nPixelRange, nTrackWidth, nThumbWidth, fThumbWidth) = GetMetrics();

            var nLeft = thumbLeft;
            nLeft += LeftArrowImage.Width + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth
                + sbBorderWidth; //+1 scaled px to left shift below due to left border


            var thumbRect = new Rectangle(new Point(nLeft, sbBorderWidth + offsetY),
                new Size(nThumbWidth, initialHeight - 2 * sbBorderWidth));
            var leftarrowRect = new Rectangle(new Point(sbBorderWidth, sbBorderWidth + offsetY),
                new Size(LeftArrowImage.Width + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth,
                initialHeight - 2 * sbBorderWidth));
            var rightarrowRect = new Rectangle(new Point(LeftArrowImage.Width
                                                         + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth + sbBorderWidth + nTrackWidth,
                sbBorderWidth + offsetY),
                new Size(LeftArrowImage.Width + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth,
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
                thumbLeftRightRepeater = new System.Threading.Timer(moveThumbLeftRight, null, 0, 50);
            }
            else if (rightarrowRect.Contains(ptPoint))
            {
                rightArrowDown = true;
                thumbLeftRightRepeater = new System.Threading.Timer(moveThumbLeftRight, null, 0, 50);
            }
            else if (thumbRect.Left > ptPoint.X && thumbRect.Top < ptPoint.Y && thumbRect.Top + thumbRect.Height > ptPoint.Y)
            {
                float fScrollDistance = Width - 2 * (LeftArrowImage.Width
                    + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth + sbBorderWidth);

                var fScrollPerc = 0.0f;
                if (fScrollDistance != 0)
                    fScrollPerc = ((float)ptPoint.X - (LeftArrowImage.Width
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
                float fScrollDistance = Width - 2 * (LeftArrowImage.Width
                    + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth + sbBorderWidth);

                var fScrollPerc = 0.0f;
                if (fScrollDistance != 0)
                    fScrollPerc = 1f - ((float)Width - 2 * (LeftArrowImage.Width
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
                    var nNewThumbLeft = x - (LeftArrowImage.Width
                                             + leftImageAdditionalLeftWidth + leftImageAdditionalRightWidth + sbBorderWidth + nSpot);

                    if (nNewThumbLeft < 0)
                        thumbLeft = 0;
                    else if (nNewThumbLeft > nPixelRange)
                        thumbLeft = nPixelRange;
                    else
                        thumbLeft = x - (LeftArrowImage.Width
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
