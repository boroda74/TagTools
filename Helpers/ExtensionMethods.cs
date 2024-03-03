using MusicBeePlugin;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static NativeMethods;


namespace ExtensionMethods
{
    public static class DictionariesExtensions
    {
        public static bool AddReplace<T1, T2>(this Dictionary<T1, T2> dictionary, T1 key, T2 value)
        {
            if (dictionary.TryGetValue(key, out _))
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

        public static bool RemoveExisting<T1, T2>(this Dictionary<T1, T2> dictionary, T1 key)
        {
            if (dictionary.TryGetValue(key, out _))
            {
                dictionary.Remove(key);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Contains<T1, T2>(this Dictionary<T1, T2> dictionary, T1 key)
        {
            return dictionary.TryGetValue(key, out _);
        }

        public static bool AddReplace<T1, T2>(this SortedDictionary<T1, T2> dictionary, T1 key, T2 value)
        {
            if (dictionary.TryGetValue(key, out _))
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

        public static bool RemoveExisting<T1, T2>(this SortedDictionary<T1, T2> dictionary, T1 key)
        {
            if (dictionary.TryGetValue(key, out _))
            {
                dictionary.Remove(key);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Contains<T1, T2>(this SortedDictionary<T1, T2> dictionary, T1 key)
        {
            return dictionary.TryGetValue(key, out _);
        }
    }

    public static class DataGridViewExtensions
    {
        public static void DisableColumnsAutoSize(this DataGridView dataGridView, bool? keepColumnWidths)
        {
            if (keepColumnWidths == null)
                dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            for (int i = 0; i < dataGridView.ColumnCount; i++)
            {
                int columnWidth = dataGridView.Columns[i].Width;
                dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                if (keepColumnWidths != false)
                    dataGridView.Columns[i].Width = columnWidth;
            }
        }

        public static void SetColumnsAutoSizeFill(this DataGridView dataGridView)
        {
            for (int i = 0; i < dataGridView.ColumnCount; i++)
                dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        public static void SetColumnsAutoSizeAllCells(this DataGridView dataGridView)
        {
            for (int i = 0; i < dataGridView.ColumnCount; i++)
                dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        public static void SetColumnsAutoSizeAllCellsLastFill(this DataGridView dataGridView)
        {
            for (int i = 0; i < dataGridView.ColumnCount - 1; i++)
                dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridView.Columns[dataGridView.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
    }

    public static class TextBoxExtensions
    {
        public static int GetCaretPosition(this TextBox textBox)
        {
            Point caret;
            GetCaretPos(out caret);
            return textBox.GetCharIndexFromPosition(caret);
        }

        public static int GetScrollPosition(this TextBox textbox)
        {
            Point lowPoint = new Point(5, 5);
            int charOnFirstVisibleLine = textbox.GetCharIndexFromPosition(lowPoint);
            int firstVisibleLine = textbox.GetLineFromCharIndex(charOnFirstVisibleLine);
            return firstVisibleLine;
        }

        public static void Scroll(this TextBox textbox, int delta)
        {
            SendMessage(textbox.Handle, EM_LINESCROLL, 0, delta);
        }

        public static void ScrollToTop(this TextBox textbox)
        {
            var customVScrollBar = ControlsTools.FindControlChild<CustomVScrollBar>(textbox.Parent);

            SendMessage(textbox.Handle, EM_LINESCROLL, 0, -100000);
            customVScrollBar?.SetValue(0);
            customVScrollBar?.Invalidate();
            textbox.Invalidate();
        }
    }

    public static class ListBoxExtensions
    {
        public static void Scroll(this ListBox listBox, int delta)
        {
            SendMessage(listBox.Handle, EM_LINESCROLL, 0, delta);
        }

        public static void ScrollToTop(this ListBox listBox)
        {
            var customVScrollBar = ControlsTools.FindControlChild<CustomVScrollBar>(listBox.Parent);

            SendMessage(listBox.Handle, EM_LINESCROLL, 0, -100000);
            customVScrollBar?.SetValue(0);
            customVScrollBar?.Invalidate();
            listBox.Invalidate();
        }
    }

    public static class ReadonlyControls
    {
        public static void Enable(this Control control, bool state)
        {
            Color enabledColor;
            Color disabledColor;

            if (Plugin.SavedSettings.dontUseSkinColors)
            {
                enabledColor = SystemColors.ControlText;
                disabledColor = SystemColors.GrayText;
            }
            else
            {
                enabledColor = Plugin.AccentColor;
                disabledColor = Plugin.DimmedAccentColor;
            }


            if (control is Label)
            {
                if (state)
                    control.ForeColor = enabledColor;
                else
                    control.ForeColor = disabledColor;

                return;
            }


            control.Enabled = state;

            if (control is CheckBox || control is RadioButton)
            {
                (control.FindForm() as PluginWindowTemplate).controlsReferencesX.TryGetValue(control, out var control2);
                if (control2 != null && control2 is Label)
                    control2.Enable(state);
            }
        }

        public static bool IsEnabled(this Control control)
        {
            Color disabledColor;

            if (Plugin.SavedSettings.dontUseSkinColors)
                disabledColor = SystemColors.GrayText;
            else
                disabledColor = Plugin.DimmedAccentColor;


            if (control is Label)
                return control.ForeColor != disabledColor;


            return control.Enabled;
        }
    }
}
