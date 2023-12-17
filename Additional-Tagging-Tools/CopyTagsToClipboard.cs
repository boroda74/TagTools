using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class CopyTagsToClipboardCommand : PluginWindowTemplate
    {
        private bool ignoreCheckUncheckAllCheckBoxChecked = false;
        private bool ignoreTagSetComboBoxTextChanged = false;

        private bool returnSelectedTags = false;
        private string[] selectedTags = null;

        public CopyTagsToClipboardCommand(Plugin tagToolsPluginParam) : base(tagToolsPluginParam)
        {
            InitializeComponent();
        }

        public CopyTagsToClipboardCommand(Plugin tagToolsPluginParam, string formTitle, string copyButtonName) : base(tagToolsPluginParam)
        {
            InitializeComponent();

            tagSetComboBox.Visible = false;
            Text = formTitle;
            buttonOK.Text = copyButtonName;
            returnSelectedTags = true;

            MbForm = MbForm.IsDisposed ? (Form)FromHandle(MbApiInterface.MB_GetWindowHandle()) : MbForm;
            MbForm.AddOwnedForm(this);
        }


        protected override void initializeForm()
        {
            base.initializeForm();

            MbForm = MbForm.IsDisposed ? (Form)FromHandle(MbApiInterface.MB_GetWindowHandle()) : MbForm;
            MbForm.AddOwnedForm(this);

            tagSetComboBox.Items.Add(SavedSettings.copyTagsTagSets[0].setName);
            tagSetComboBox.Items.Add(SavedSettings.copyTagsTagSets[1].setName);
            tagSetComboBox.Items.Add(SavedSettings.copyTagsTagSets[2].setName);
            tagSetComboBox.Items.Add(SavedSettings.copyTagsTagSets[3].setName);
            tagSetComboBox.Items.Add(SavedSettings.copyTagsTagSets[4].setName);
            tagSetComboBox.Items.Add(SavedSettings.copyTagsTagSets[5].setName);
            tagSetComboBox.Items.Add(SavedSettings.copyTagsTagSets[6].setName);
            tagSetComboBox.Items.Add(SavedSettings.copyTagsTagSets[7].setName);
            tagSetComboBox.Items.Add(SavedSettings.copyTagsTagSets[8].setName);
            tagSetComboBox.Items.Add(SavedSettings.copyTagsTagSets[9].setName);

            tagSetComboBox_DropDownClosed(null, null);


            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        public static string[] SelectTags(Plugin tagToolsPluginParam, string formTitle, string copyButtonName, string[] preselectedTags, bool addArtworkAlso)
        {
            CopyTagsToClipboardCommand form = new CopyTagsToClipboardCommand(tagToolsPluginParam, formTitle, copyButtonName);

            form.selectedTags = (string[])preselectedTags.Clone();
            form.fillTags(false, addArtworkAlso);
            Display(form, true);

            return form.selectedTags;
        }

        public static int[] SelectTags(Plugin tagToolsPluginParam, string formTitle, string copyButtonName, int[] preselectedTags, bool addArtworkAlso)
        {
            CopyTagsToClipboardCommand form = new CopyTagsToClipboardCommand(tagToolsPluginParam, formTitle, copyButtonName);

            form.selectedTags = new string[preselectedTags.Length];
            for (int i = 0; i < preselectedTags.Length; i++)
                form.selectedTags[i] = GetTagName((MetaDataType)preselectedTags[i]);

            form.fillTags(false, addArtworkAlso);
            Display(form, true);

            int[] selectedTagIds = new int[form.selectedTags.Length];
            for (int i = 0; i < form.selectedTags.Length; i++)
                selectedTagIds[i] = (int)GetTagId(form.selectedTags[i]);

            return selectedTagIds;
        }

        private void processCheckedTags()
        {
            selectedTags = new string[checkedSourceTagList.Items.Count];

            for (int i = 0; i < checkedSourceTagList.Items.Count; i++)
                selectedTags[i] = checkedSourceTagList.Items[i].ToString().Substring(2);
        }

        private void fillTags(bool addReadonlyTagsAlso, bool addArtworkAlso)
        {
            checkedSourceTagList.Items.Clear();
            sourceTagList.Items.Clear();

            FillListByTagNames(sourceTagList.Items, addReadonlyTagsAlso, addArtworkAlso, false, true);

            if (addReadonlyTagsAlso)
                FillListByPropNames(sourceTagList.Items, true);


            for (int i = 0; i < selectedTags.Length; i++)
            {
                for (int j = 0; j < sourceTagList.Items.Count; j++)
                {
                    if (((string)sourceTagList.Items[j]).Substring(2) == selectedTags[i])
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

        }

        private void fillTagsFromTagSet(int tagSetId)
        {
            selectedTags = new string[SavedSettings.copyTagsTagSets[tagSetId].tagIds.Length];

            for (int i = 0; i < SavedSettings.copyTagsTagSets[SavedSettings.lastInteractiveTagSet].tagIds.Length; i++)
            {
                string tagName = GetTagName((MetaDataType)SavedSettings.copyTagsTagSets[SavedSettings.lastInteractiveTagSet].tagIds[i]);
                selectedTags[i] = tagName;
            }

            fillTags(true, true);
        }

        public static bool CopyTagsToClipboard(int tagSet)
        {
            string[] files = null;

            if (!MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files))
                files = new string[0];


            if (files.Length == 0)
            {
                MessageBox.Show(MbForm, MsgNoTracksSelected, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            SavedSettings.lastTagSet = tagSet;

            string clipboardText = string.Empty;
            foreach (string file in files)
            {
                string tags = string.Empty;
                for (int i = 0; i < SavedSettings.copyTagsTagSets[tagSet].tagIds.Length; i++)
                {
                    string tag = GetFileTag(file, (MetaDataType)SavedSettings.copyTagsTagSets[tagSet].tagIds[i]);
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

            //if (clipboardText == string.Empty)
            //    clipboardText = "\u0000";


            Clipboard.Clear();
            Clipboard.SetText(clipboardText);

            return true;
        }

        private void saveCurrentIds()
        {
            int displayedArtistOffset = 0;
            int displayedComposerOffset = 0;
            List<int> checkedIds = new List<int>();

            for (int i = 0; i < checkedSourceTagList.Items.Count; i++)
            {
                int id = (int)GetTagId((string)checkedSourceTagList.Items[i]);

                checkedIds.Add(id);

                if (((string)checkedSourceTagList.Items[i]).Substring(2) == DisplayedArtistName) //Displayed artist should be copied to clipboard the first or second
                {
                    if (displayedArtistOffset == 0)
                        displayedComposerOffset = 1;

                    checkedIds[checkedIds.Count - 1] = checkedIds[displayedArtistOffset];
                    checkedIds[displayedArtistOffset] = id;
                }
                else if (((string)checkedSourceTagList.Items[i]).Substring(2) == DisplayedComposerName) //Displayed composer should be copied to clipboard the first or second
                {
                    if (displayedComposerOffset == 0)
                        displayedArtistOffset = 1;

                    checkedIds[checkedIds.Count - 1] = checkedIds[displayedComposerOffset];
                    checkedIds[displayedComposerOffset] = id;
                }
            }

            SavedSettings.copyTagsTagSets[SavedSettings.lastInteractiveTagSet].tagIds = new int[checkedIds.Count];
            checkedIds.CopyTo(SavedSettings.copyTagsTagSets[SavedSettings.lastInteractiveTagSet].tagIds);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (returnSelectedTags)
                processCheckedTags();
            else
                CopyTagsToClipboard(SavedSettings.lastInteractiveTagSet);

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
                buttonOK.Enabled = returnSelectedTags;
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
                buttonOK.Enabled = returnSelectedTags;
            else
                buttonOK.Enabled = true;
        }

        private void tagSetComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (tagSetComboBox.SelectedIndex == SavedSettings.lastInteractiveTagSet)
                return;


            if (tagSetComboBox.SelectedIndex == -1)
            {
                tagSetComboBox.SelectedIndex = SavedSettings.lastInteractiveTagSet;
            }
            else if (tagSetComboBox.SelectedIndex == SavedSettings.lastInteractiveTagSet)
            {
                return;
            }
            else
            {
                SavedSettings.lastInteractiveTagSet = tagSetComboBox.SelectedIndex;
            }


            //checkedSourceTagList.SuspendPainting();
            //sourceTagList.SuspendPainting();


            fillTagsFromTagSet(SavedSettings.lastInteractiveTagSet);


            //checkedSourceTagList.Repaint();
            //sourceTagList.Repaint();
        }

        private void tagSetComboBox_TextChanged(object sender, EventArgs e)
        {
            if (ignoreTagSetComboBoxTextChanged)
                return;


            ignoreTagSetComboBoxTextChanged = true;

            string prefix = SavedSettings.copyTagsTagSets[SavedSettings.lastInteractiveTagSet].getPrefix();

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

            tagSetComboBox.Items[SavedSettings.lastInteractiveTagSet] = tagSetComboBox.Text;
            SavedSettings.copyTagsTagSets[SavedSettings.lastInteractiveTagSet].setName = tagSetComboBox.Text;

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

        private void checkUncheckAllCheckBoxLabel_Click(object sender, EventArgs e)
        {
            checkUncheckAllCheckBox.Checked = !checkUncheckAllCheckBox.Checked;
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
