using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class CopyTagsToClipboardCommand : PluginWindowTemplate
    {
        private bool ignoreCheckUncheckAllCheckBoxChecked = false;
        private bool ignoreTagSetComboBoxTextChanged = false;

        public CopyTagsToClipboardCommand()
        {
            InitializeComponent();
        }

        public CopyTagsToClipboardCommand(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            //base.initializeForm();

            Plugin.MbForm = Plugin.MbForm.IsDisposed ? (Form)FromHandle(Plugin.MbApiInterface.MB_GetWindowHandle()) : Plugin.MbForm;
            Plugin.MbForm.AddOwnedForm(this);

            tagSetComboBox.Items.Add(Plugin.SavedSettings.copyTagsTagSets[0].setName);
            tagSetComboBox.Items.Add(Plugin.SavedSettings.copyTagsTagSets[1].setName);
            tagSetComboBox.Items.Add(Plugin.SavedSettings.copyTagsTagSets[2].setName);
            tagSetComboBox.Items.Add(Plugin.SavedSettings.copyTagsTagSets[3].setName);
            tagSetComboBox.Items.Add(Plugin.SavedSettings.copyTagsTagSets[4].setName);
            tagSetComboBox.Items.Add(Plugin.SavedSettings.copyTagsTagSets[5].setName);
            tagSetComboBox.Items.Add(Plugin.SavedSettings.copyTagsTagSets[6].setName);
            tagSetComboBox.Items.Add(Plugin.SavedSettings.copyTagsTagSets[7].setName);
            tagSetComboBox.Items.Add(Plugin.SavedSettings.copyTagsTagSets[8].setName);
            tagSetComboBox.Items.Add(Plugin.SavedSettings.copyTagsTagSets[9].setName);

            tagSetComboBox_DropDownClosed(null, null);
        }

        public static bool CopyTagsToClipboard(int tagSet)
        {
            string[] files = null;

            if (!Plugin.MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files))
                files = new string[0];


            if (files.Length == 0)
            {
                MessageBox.Show(Plugin.MbForm, Plugin.MsgNoFilesSelected, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            Plugin.SavedSettings.lastTagSet = tagSet;

            string clipboardText = "";
            foreach (string file in files)
            {
                string tags = "";
                for (int i = 0; i < Plugin.SavedSettings.copyTagsTagSets[tagSet].tagIds.Length; i++)
                {
                    string tag = Plugin.GetFileTag(file, (Plugin.MetaDataType)Plugin.SavedSettings.copyTagsTagSets[tagSet].tagIds[i]);
                    tag = tag.Replace("\u0000", "\u0006").Replace("\u000D", "\u0007").Replace("\u000A", "\u0008");
                    tag += "\t";

                    if (tag.Length > 4000000)
                        tag = "\t";

                    tags += tag;
                }

                tags = tags.Remove(tags.Length - 1);

                clipboardText += tags + "\n";
            }

            clipboardText = clipboardText.Remove(clipboardText.Length - 1);

            //if (clipboardText == "")
            //    clipboardText = "\u0000";


            System.Windows.Clipboard.SetText(clipboardText);
            //NativeWindowsClipboard.SetText(clipboardText);//***

            return true;
        }

        private void saveCurrentIds()
        {
            int displayedArtistOffset = 0;
            int displayedComposerOffset = 0;
            List<int> checkedIds = new List<int>();

            for (int i = 0; i < checkedSourceTagList.Items.Count; i++)
            {
                int id = (int)Plugin.GetTagId((string)checkedSourceTagList.Items[i]);

                checkedIds.Add(id);

                if (((string)checkedSourceTagList.Items[i]).Substring(2) == Plugin.DisplayedArtistName) //Displayed artist should be copied to clipboard the first or second
                {
                    if (displayedArtistOffset == 0)
                        displayedComposerOffset = 1;

                    checkedIds[checkedIds.Count - 1] = checkedIds[displayedArtistOffset];
                    checkedIds[displayedArtistOffset] = id;
                }
                else if (((string)checkedSourceTagList.Items[i]).Substring(2) == Plugin.DisplayedComposerName) //Displayed composer should be copied to clipboard the first or second
                {
                    if (displayedComposerOffset == 0)
                        displayedArtistOffset = 1;

                    checkedIds[checkedIds.Count - 1] = checkedIds[displayedComposerOffset];
                    checkedIds[displayedComposerOffset] = id;
                }
            }

            Plugin.SavedSettings.copyTagsTagSets[Plugin.SavedSettings.lastInteractiveTagSet].tagIds = new int[checkedIds.Count];
            checkedIds.CopyTo(Plugin.SavedSettings.copyTagsTagSets[Plugin.SavedSettings.lastInteractiveTagSet].tagIds);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            CopyTagsToClipboard(Plugin.SavedSettings.lastInteractiveTagSet);
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkedSourceTagList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedSourceTagList.SelectedIndex != -1)
            {
                sourceTagList.Items.Add(checkedSourceTagList.Items[checkedSourceTagList.SelectedIndex]);
                checkedSourceTagList.Items.RemoveAt(checkedSourceTagList.SelectedIndex);
            }


            saveCurrentIds();


            ignoreCheckUncheckAllCheckBoxChecked = true;

            if (sourceTagList.Items.Count == 0)
                checkUncheckAllCheckBox.Checked = true;
            else
                checkUncheckAllCheckBox.Checked = false;

            ignoreCheckUncheckAllCheckBoxChecked = false;


            if (checkedSourceTagList.Items.Count == 0)
                buttonOK.Enabled = false;
            else
                buttonOK.Enabled = true;
        }

        private void sourceTagList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sourceTagList.SelectedIndex != -1)
            {
                checkedSourceTagList.Items.Add(sourceTagList.Items[sourceTagList.SelectedIndex]);

                for (int i = 0; i < checkedSourceTagList.Items.Count; i++)
                    checkedSourceTagList.SetItemChecked(i, true);

                sourceTagList.Items.RemoveAt(sourceTagList.SelectedIndex);
            }


            saveCurrentIds();


            if (sourceTagList.Items.Count == 0)
                checkUncheckAllCheckBox.Checked = true;
            else
                checkUncheckAllCheckBox.Checked = false;


            if (checkedSourceTagList.Items.Count == 0)
                buttonOK.Enabled = false;
            else
                buttonOK.Enabled = true;
        }

        private void tagSetComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (tagSetComboBox.SelectedIndex == Plugin.SavedSettings.lastInteractiveTagSet)
                return;


            if (tagSetComboBox.SelectedIndex == -1)
            {
                tagSetComboBox.SelectedIndex = Plugin.SavedSettings.lastInteractiveTagSet;
            }
            else if (tagSetComboBox.SelectedIndex == Plugin.SavedSettings.lastInteractiveTagSet)
            {
                return;
            }
            else
            {
                Plugin.SavedSettings.lastInteractiveTagSet = tagSetComboBox.SelectedIndex;
            }


            //checkedSourceTagList.SuspendPainting();
            //sourceTagList.SuspendPainting();


            checkedSourceTagList.Items.Clear();
            sourceTagList.Items.Clear();

            Plugin.FillList(sourceTagList.Items, true, true, false, true);
            Plugin.FillListWithProps(sourceTagList.Items, true);


            for (int i = 0; i < Plugin.SavedSettings.copyTagsTagSets[Plugin.SavedSettings.lastInteractiveTagSet].tagIds.Length; i++)
            {
                for (int j = 0; j < sourceTagList.Items.Count; j++)
                {
                    string tagName = Plugin.GetTagName((Plugin.MetaDataType)Plugin.SavedSettings.copyTagsTagSets[Plugin.SavedSettings.lastInteractiveTagSet].tagIds[i]);

                    if (((string)sourceTagList.Items[j]).Substring(2) == tagName)
                    {
                        sourceTagList.SetItemChecked(j, true);
                        break;
                    }
                }
            }

            for (int i = 0; i < sourceTagList.Items.Count;)
                if (sourceTagList.GetItemChecked(i))
                    sourceTagList.SelectedIndex = i;
                else
                    i++;


            //checkedSourceTagList.Repaint();
            //sourceTagList.Repaint();
        }

        private void tagSetComboBox_TextChanged(object sender, EventArgs e)
        {
            if (ignoreTagSetComboBoxTextChanged)
                return;


            ignoreTagSetComboBoxTextChanged = true;

            string prefix = Plugin.SavedSettings.copyTagsTagSets[Plugin.SavedSettings.lastInteractiveTagSet].getPrefix();

            if (!tagSetComboBox.Text.StartsWith(prefix))
            {
                int matchingLength = 0;
                for (int i = 1; i <= prefix.Length && i <= tagSetComboBox.Text.Length; i++)
                {
                    if (prefix.Substring(0,i) == tagSetComboBox.Text.Substring(0, i))
                        matchingLength = i;
                    else
                        break;
                }

                int selectionStart = tagSetComboBox.SelectionStart;
                if (matchingLength > 0)
                {
                    tagSetComboBox.Text = prefix + tagSetComboBox.Text.Substring(matchingLength);
                    tagSetComboBox.SelectionStart = prefix.Length - matchingLength + selectionStart;
                }
                else
                {
                    tagSetComboBox.Text = prefix + tagSetComboBox.Text;
                    tagSetComboBox.SelectionStart = prefix.Length + selectionStart + 1;
                }
            }

            tagSetComboBox.Items[Plugin.SavedSettings.lastInteractiveTagSet] = tagSetComboBox.Text;
            Plugin.SavedSettings.copyTagsTagSets[Plugin.SavedSettings.lastInteractiveTagSet].setName = tagSetComboBox.Text;

            ignoreTagSetComboBoxTextChanged = false;
        }

        private void checkUncheckAllCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ignoreCheckUncheckAllCheckBoxChecked)
                return;


            //checkedSourceTagList.SuspendPainting();
            //sourceTagList.SuspendPainting();


            if (checkUncheckAllCheckBox.Checked)
                while (sourceTagList.Items.Count > 0)
                    sourceTagList.SelectedIndex = 0;
            else
                while (checkedSourceTagList.Items.Count > 0)
                    checkedSourceTagList.SelectedIndex = 0;


            //checkedSourceTagList.Repaint();
            //sourceTagList.Repaint();
        }

        private void CopyTagsToClipboardCommand_FormClosed(object sender, FormClosedEventArgs e)
        {
            TagToolsPlugin.addCopyTagsToClipboardUsingMenuItems();
            TagToolsPlugin.addCopyTagsToClipboardUsingContextMenuItems();
        }
    }

    public class CustomListBox : CheckedListBox
    {
        private bool ProcessPaintMessages = true;

        protected override void OnPaint(PaintEventArgs e)
        {
            if (ProcessPaintMessages)
                base.OnPaint(e);
        }

        public void SuspendPainting()
        {
            ProcessPaintMessages = false;
        }

        public void Repaint()
        {
            ProcessPaintMessages = true;

            Invalidate();
        }
    }

    unsafe public static class NativeWindowsClipboard//***
    {
        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        private static extern int OpenClipboard(
            IntPtr hWndNewOwner
        );

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        private static extern int CloseClipboard();

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        private static extern int EmptyClipboard();

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr GetClipboardData(
            uint uFormat
        );

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SetClipboardData(
            uint uFormat,
            IntPtr hMem
        );

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr GlobalLock(
            IntPtr hMem
        );

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern int GlobalUnlock(
            IntPtr hMem
        );

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr GlobalAlloc(
            uint uFlags,
            ulong dwBytes
        );

        [DllImport("msvcrt", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern int wcscpy(char* str1, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder str2);

        private const uint CF_UNICODETEXT = 13;
        private const uint GMEM_DDESHARE = 8192;

        public static void SetText(string strText)
        {
            if (OpenClipboard(IntPtr.Zero) != 0)
            {
                IntPtr hgBuffer;
                char* chBuffer;
                EmptyClipboard();
                hgBuffer = GlobalAlloc(GMEM_DDESHARE, 2 * Convert.ToUInt32(strText.Length + 1)); // 2 = sizeof(wchar_t) -- C++; sizeof(char) -- C#, UInt32 - for 32bit apps
                chBuffer = (char*)GlobalLock(hgBuffer);
                wcscpy(chBuffer, new StringBuilder(strText));
                GlobalUnlock(hgBuffer);
                SetClipboardData(CF_UNICODETEXT, hgBuffer);
                CloseClipboard();
            }
        }

        public static string GetText()
        {
            if (OpenClipboard(IntPtr.Zero) != 0)
            {
                IntPtr hData = GetClipboardData(CF_UNICODETEXT);
                char* chBuffer = (char*)GlobalLock(hData);
                string strResult = new string(chBuffer);
                GlobalUnlock(hData);
                CloseClipboard();
                return strResult;
            }
            return "";
        }
    }

    public partial class Plugin
    {
        public void copyTagsUsingTagSet1EventHandler(object sender, EventArgs e)
        {
            CopyTagsToClipboardCommand.CopyTagsToClipboard(0);
        }

        public void copyTagsUsingTagSet2EventHandler(object sender, EventArgs e)
        {
            CopyTagsToClipboardCommand.CopyTagsToClipboard(1);
        }

        public void copyTagsUsingTagSet3EventHandler(object sender, EventArgs e)
        {
            CopyTagsToClipboardCommand.CopyTagsToClipboard(2);
        }

        public void copyTagsUsingTagSet4EventHandler(object sender, EventArgs e)
        {
            CopyTagsToClipboardCommand.CopyTagsToClipboard(3);
        }

        public void copyTagsUsingTagSet5EventHandler(object sender, EventArgs e)
        {
            CopyTagsToClipboardCommand.CopyTagsToClipboard(4);
        }

        public void copyTagsUsingTagSet6EventHandler(object sender, EventArgs e)
        {
            CopyTagsToClipboardCommand.CopyTagsToClipboard(5);
        }

        public void copyTagsUsingTagSet7EventHandler(object sender, EventArgs e)
        {
            CopyTagsToClipboardCommand.CopyTagsToClipboard(6);
        }

        public void copyTagsUsingTagSet8EventHandler(object sender, EventArgs e)
        {
            CopyTagsToClipboardCommand.CopyTagsToClipboard(7);
        }

        public void copyTagsUsingTagSet9EventHandler(object sender, EventArgs e)
        {
            CopyTagsToClipboardCommand.CopyTagsToClipboard(8);
        }

        public void copyTagsUsingTagSet10EventHandler(object sender, EventArgs e)
        {
            CopyTagsToClipboardCommand.CopyTagsToClipboard(9);
        }
    }
}
