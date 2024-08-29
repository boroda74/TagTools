using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using MusicBeePlugin;

using static MusicBeePlugin.LibraryReports;
using static NativeMethods;


namespace ExtensionMethods
{
    internal static class DictionariesExtensions
    {
        internal static bool AddReplace<T1, T2>(this Dictionary<T1, T2> dictionary, T1 key, T2 value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
                dictionary.Add(key, value);
                return true;
            }
            else
            {
                dictionary.Add(key, value);
                return false;
            }
        }

        internal static bool AddSkip<T1, T2>(this Dictionary<T1, T2> dictionary, T1 key, T2 value)
        {
            if (dictionary.ContainsKey(key))
            {
                return true;
            }
            else
            {
                dictionary.Add(key, value);
                return false;
            }
        }

        internal static bool AddSkip<T>(this Dictionary<T, bool> dictionary, T key)
        {
            if (dictionary.ContainsKey(key))
            {
                return true;
            }
            else
            {
                dictionary.Add(key, false);
                return false;
            }
        }

        internal static void Add<T>(this Dictionary<T, bool> dictionary, T key)
        {
            dictionary.Add(key, false);
        }

        internal static bool RemoveExisting<T1, T2>(this Dictionary<T1, T2> dictionary, T1 key)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
                return true;
            }
            else
            {
                return false;
            }
        }


        internal static bool AddReplace<T1, T2>(this SortedDictionary<T1, T2> dictionary, T1 key, T2 value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
                dictionary.Add(key, value);
                return true;
            }
            else
            {
                dictionary.Add(key, value);
                return false;
            }
        }

        internal static bool AddSkip<T1, T2>(this SortedDictionary<T1, T2> dictionary, T1 key, T2 value)
        {
            if (dictionary.ContainsKey(key))
            {
                return true;
            }
            else
            {
                dictionary.Add(key, value);
                return false;
            }
        }

        internal static bool AddSkip<T>(this SortedDictionary<T, bool> dictionary, T key)
        {
            if (dictionary.ContainsKey(key))
            {
                return true;
            }
            else
            {
                dictionary.Add(key, false);
                return false;
            }
        }

        internal static void Add<T>(this SortedDictionary<T, bool> dictionary, T key)
        {
            dictionary.Add(key, false);
        }

        internal static bool RemoveExisting<T1, T2>(this SortedDictionary<T1, T2> dictionary, T1 key)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static bool ReplaceExisting<T1, T2>(this SortedDictionary<T1, T2> dictionary, T1 key, T2 newValue)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
                dictionary.Add(key, newValue);
                return true;
            }
            else
            {
                return false;
            }
        }


        internal static bool AddReplaceUi(this ColumnAttributesDict dictionary, string key, ColumnAttributes value, Button button, CheckBox checkBox)
        {
            button.Enable(true);
            checkBox.Enable(true);

            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
                dictionary.Add(key, value);
                return true;
            }
            else
            {
                dictionary.Add(key, value);
                return false;
            }
        }

        internal static bool AddSkipUi(this ColumnAttributesDict dictionary, string key, ColumnAttributes value, Button button, CheckBox checkBox)
        {
            button.Enable(true);
            checkBox.Enable(true);

            if (dictionary.ContainsKey(key))
            {
                return true;
            }
            else
            {
                dictionary.Add(key, value);
                return false;
            }
        }

        internal static bool RemoveExistingUi(this ColumnAttributesDict dictionary, string key, Button button, CheckBox checkBox)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);

                if (dictionary.Count == 0)
                {
                    button.Enable(false);
                    checkBox.Enable(false);
                    checkBox.Checked = false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        internal static void RemoveUi(this ColumnAttributesDict dictionary, string key, Button button, CheckBox checkBox)
        {
            dictionary.Remove(key);

            if (dictionary.Count == 0)
            {
                button.Enable(false);
                checkBox.Enable(false);
                checkBox.Checked = false;
            }
        }

        internal static void ClearUi(this ColumnAttributesDict dictionary, Button button)
        {
            dictionary.Clear();
            button.Enable(false);
        }


        internal static bool AddUnique<T>(this List<T> list, T value)
        {
            if (!list.Contains(value))
            {
                list.Add(value);
                return true;
            }

            return false;
        }

        internal static bool RemoveExisting<T>(this List<T> list, T value)
        {
            if (list.Contains(value))
            {
                list.Remove(value);
                return true;
            }

            return false;
        }

        internal static int IndexOfFirst<T>(this T[] array, T value)
        {
            if (value == null) //-V3111
            {
                for (var i = 0; i < array.Length; i++)
                {
                    if (array[i] == null) //-V3111
                        return i;
                }

                return -1;
            }

            var @default = EqualityComparer<T>.Default;
            for (var i = 0; i < array.Length; i++)
            {
                if (@default.Equals(array[i], value))
                    return i;
            }

            return -1;
        }

        internal static int IndexOfLast<T>(this T[] array, T value)
        {
            if (value == null) //-V3111
            {
                for (var i = array.Length - 1; i >= 0; i--)
                {
                    if (array[i] == null) //-V3111
                        return i;
                }

                return -1;
            }

            var @default = EqualityComparer<T>.Default;
            for (var i = array.Length - 1; i >= 0; i--)
            {
                if (@default.Equals(array[i], value))
                    return i;
            }

            return -1;
        }
    }

    internal static class DataGridViewExtensions
    {
        internal static void DisableColumnsAutoSize(this DataGridView dataGridView, bool keepColumnWidths)
        {
            for (var i = 0; i < dataGridView.ColumnCount; i++)
            {
                var columnWidth = dataGridView.Columns[i].Width;
                dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                if (keepColumnWidths)
                    dataGridView.Columns[i].Width = columnWidth;
            }

            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        }

        internal static void SetColumnsAutoSizeFill(this DataGridView dataGridView)
        {
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            for (var i = 0; i < dataGridView.ColumnCount; i++)
                dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        internal static void SetColumnsAutoSizeAllCells(this DataGridView dataGridView)
        {
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            for (var i = 0; i < dataGridView.ColumnCount; i++)
                dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        internal static void SetColumnsAutoSizeHeaders(this DataGridView dataGridView)
        {
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;

            for (var i = 0; i < dataGridView.ColumnCount; i++)
                dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
        }

        internal static void SetColumnsAutoSizeAllCellsLastFill(this DataGridView dataGridView)
        {
            for (var i = 0; i < dataGridView.ColumnCount - 1; i++)
                dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridView.Columns[dataGridView.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
    }

    internal static class TextBoxExtensions
    {
        internal static int GetCaretPosition(this TextBox textBox)
        {
            GetCaretPos(out var caret);
            return textBox.GetCharIndexFromPosition(caret);
        }

        internal static int GetScrollPosition(this TextBox textBox)
        {
            var lowPoint = new Point(5, 5);
            var charOnFirstVisibleLine = textBox.GetCharIndexFromPosition(lowPoint);
            var firstVisibleLine = textBox.GetLineFromCharIndex(charOnFirstVisibleLine);
            return firstVisibleLine;
        }

        internal static void Scroll(this TextBox textBox, int delta)
        {
            SendMessage(textBox.Handle, EM_LINESCROLL, Zero, (IntPtr)delta);
        }

        internal static void ScrollToTop(this TextBox textBox)
        {
            var customVScrollBar = ControlsTools.FindControlChild<CustomVScrollBar>(textBox.Parent);

            SendMessage(textBox.Handle, EM_LINESCROLL, Zero, (IntPtr)(-100000));
            customVScrollBar?.SetValue(0);
            customVScrollBar?.Invalidate();
            textBox.Invalidate();
        }
    }

    internal static class ListBoxExtensions
    {
        internal static void Scroll(this ListBox listBox, int delta)
        {
            SendMessage(listBox.Handle, EM_LINESCROLL, Zero, (IntPtr)delta);
        }

        internal static void ScrollToTop(this ListBox listBox)
        {
            var customVScrollBar = ControlsTools.FindControlChild<CustomVScrollBar>(listBox.Parent);

            SendMessage(listBox.Handle, EM_LINESCROLL, Zero, (IntPtr)(-100000));
            customVScrollBar?.SetValue(0);
            customVScrollBar?.Invalidate();
            listBox.Invalidate();
        }
    }

    internal static class ReadonlyControls
    {
        internal static void MakeReadonly(this Control parent, bool enable)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is Button button)
                    button.Enable(!enable);
                else if (control.GetType().IsSubclassOf(typeof(TextBox)) || control is TextBox)
                    (control as TextBox).ReadOnly = enable;
                else if (control.GetType().IsSubclassOf(typeof(CustomComboBox)) || control is CustomComboBox)
                    (control as CustomComboBox).ForceReadonly(enable);
                else if (control.GetType().IsSubclassOf(typeof(ListBox)) || control is ListBox)
                    (control as ListBox).Enable(!enable);
                else if (control is GroupBox)
                    ; //Nothing...
                else if (control is TableLayoutPanel)
                    ; //Nothing...
                else if (control is Panel)
                    ; //Nothing...
                else
                    control.Enable(false);

                control.MakeReadonly(enable);
            }
        }

        internal static void Enable(this Control control, bool enable)
        {
            if (string.IsNullOrEmpty(control.AccessibleDescription))
                control.EnableInternal(enable);
            else if (enable && control.AccessibleDescription == Plugin.DisabledState)
                control.AccessibleDescription = Plugin.EnabledState;
            else if (!enable && control.AccessibleDescription == Plugin.EnabledState)
                control.AccessibleDescription = Plugin.DisabledState;
        }

        internal static void EnableInternal(this Control control, bool enable)
        {
            var form = control.FindForm() as PluginWindowTemplate;

            if (control is GroupBox || control is SplitContainer || control is Panel)
            {
                foreach (Control child in control.Controls)
                    child.Enable(enable);

                return;
            }

            if (control.Parent is CustomComboBox)
                return;


            if (control is CustomComboBox customComboBox)
            {
                form = customComboBox.ownerForm;

                if (form == null || form.Disposing || form.IsDisposed || !form.IsHandleCreated)
                    return;

                form.setSkinnedControlColors(control, enable);
                control.Enabled = enable;
                return;
            }


            if (form == null || form.Disposing || form.IsDisposed || !form.IsHandleCreated)
                return;

            if (control is TextBox || control is ListBox)
            {
                form.setSkinnedControlColors(control, enable);
                control.Enabled = enable;
                return;
            }



            if (control is Label)
            {
                form.controlsReferencedX.TryGetValue(control, out var control2);
                if (!(control2 is CheckBox || control2 is RadioButton))
                    form.setSkinnedControlColors(control, enable);

                return;
            }


            form.setSkinnedControlColors(control, enable);

            control.Enabled = enable;

            if (control is CheckBox || control is RadioButton)
            {
                form.controlsReferencesX.TryGetValue(control, out var control2);
                if (control2 is Label label)
                    label.Enable(enable);
            }
        }

        internal static bool IsEnabled(this Control control)
        {
            Color disabledColor;

            if (Plugin.SavedSettings.dontUseSkinColors)
                disabledColor = SystemColors.GrayText;
            else
                disabledColor = Plugin.DimmedAccentColor;


            if (control.AccessibleDescription == Plugin.DisabledState)
                return false;
            else if (control.AccessibleDescription == Plugin.EnabledState)
                return true;
            else if (control is Label)
                return control.ForeColor != disabledColor;
            else
                return control.Enabled;
        }
    }
}
