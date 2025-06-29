﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using ExtensionMethods;

using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class CopyTagsToClipboard : PluginWindowTemplate
    {
        private CustomComboBox tagSetComboBoxCustom;

        private bool ignoreTagSetComboBoxTextChanged;
        private string currentPrefix;

        private string[] availableTags;
        private string[] prefixedTags;
        private object[] prefixedUnselectedTags;
        private object[] prefixedSelectedTags;
        private string[] selectedTags;
        private readonly bool returnSelectedTags;

        public CopyTagsToClipboard(Plugin plugin) : base(plugin)
        {
            InitializeComponent();

            WindowIcon = CopyTagsIcon;
            TitleBarText = this.Text;
        }

        public CopyTagsToClipboard(Plugin plugin, string formTitle, string copyButtonName) : this(plugin)
        {
            WindowIcon = CopyTagsIcon;
            tagSetComboBoxCustom.Visible = false;
            Text = formTitle;
            TitleBarText = formTitle;
            buttonOK.Text = copyButtonName;
            returnSelectedTags = true;
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }


        internal protected override void initializeForm()
        {
            base.initializeForm();

            currentPrefix = SavedSettings.copyTagsTagSets[SavedSettings.lastInteractiveTagSet].getPrefix();

            tagSetComboBoxCustom = namesComboBoxes["tagSetComboBox"];
            tagSetComboBoxCustom.Visible = tagSetComboBox.Visible;

            if (!returnSelectedTags)
            {
                tagSetComboBoxCustom.Add(SavedSettings.copyTagsTagSets[0].setName);
                tagSetComboBoxCustom.Add(SavedSettings.copyTagsTagSets[1].setName);
                tagSetComboBoxCustom.Add(SavedSettings.copyTagsTagSets[2].setName);
                tagSetComboBoxCustom.Add(SavedSettings.copyTagsTagSets[3].setName);
                tagSetComboBoxCustom.Add(SavedSettings.copyTagsTagSets[4].setName);
                tagSetComboBoxCustom.Add(SavedSettings.copyTagsTagSets[5].setName);
                tagSetComboBoxCustom.Add(SavedSettings.copyTagsTagSets[6].setName);
                tagSetComboBoxCustom.Add(SavedSettings.copyTagsTagSets[7].setName);
                tagSetComboBoxCustom.Add(SavedSettings.copyTagsTagSets[8].setName);
                tagSetComboBoxCustom.Add(SavedSettings.copyTagsTagSets[9].setName);

                prepareAvailableTags(true, true, true);
                tagSetComboBox_SelectedIndexChanged(null, null);

                buttonOK.Enable(checkedSourceTagList.Items.Count > 0);
            }


            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        internal static string[] SelectTags(Plugin plugin, Form ownerForm, string formTitle, string copyButtonName, string[] preselectedTags, bool addArtworkAlso, bool addDateCreatedAlso)
        {
            var form = new CopyTagsToClipboard(plugin, formTitle, copyButtonName);

            form.selectedTags = preselectedTags.Clone() as string[];
            form.fillTags(false, addArtworkAlso, addDateCreatedAlso);
            Display(form, ownerForm, true);

            return form.selectedTags;
        }

        internal static int[] SelectTags(Plugin plugin, Form ownerForm, string formTitle, string copyButtonName, int[] preselectedTags, bool addArtworkAlso, bool addDateCreatedAlso)
        {
            var form = new CopyTagsToClipboard(plugin, formTitle, copyButtonName);

            form.selectedTags = new string[preselectedTags.Length];
            for (var i = 0; i < preselectedTags.Length; i++)
                form.selectedTags[i] = GetTagName((MetaDataType)preselectedTags[i]);

            form.fillTags(false, addArtworkAlso, addDateCreatedAlso);
            Display(form, ownerForm, true);

            var selectedTagIds = new int[form.selectedTags.Length];
            for (var i = 0; i < form.selectedTags.Length; i++)
                selectedTagIds[i] = (int)GetTagId(form.selectedTags[i]);

            return selectedTagIds;
        }

        private void processCheckedTags()
        {
            selectedTags = new string[checkedSourceTagList.Items.Count];

            for (var i = 0; i < checkedSourceTagList.Items.Count; i++)
                selectedTags[i] = checkedSourceTagList.Items[i].ToString().Substring(2);
        }

        private void prepareAvailableTags(bool addReadonlyTagsAlso, bool addArtworkAlso, bool addDateCreatedAlso)
        {
            var availableTagList = new List<string>();
            FillListByTagNames(availableTagList, addReadonlyTagsAlso, addArtworkAlso, false, false, false, addDateCreatedAlso);

            if (addReadonlyTagsAlso)
                FillListByPropNames(availableTagList);

            availableTags = availableTagList.ToArray();

            prefixedTags = new string[availableTags.Length];
            for (var i = 0; i < availableTags.Length; i++)
                prefixedTags[i] = GetTagPrefix(availableTags[i]) + availableTags[i];
        }

        private void fillPreparedTags()
        {
            prefixedSelectedTags = new object[selectedTags.Length];
            for (var i = 0; i < selectedTags.Length; i++)
                prefixedSelectedTags[i] = GetTagPrefix(selectedTags[i]) + selectedTags[i];


            prefixedUnselectedTags = prefixedTags.Except(prefixedSelectedTags).ToArray();

            checkedSourceTagList.Items.Clear();
            sourceTagList.Items.Clear();

            checkedSourceTagList.Items.AddRange(prefixedSelectedTags);
            sourceTagList.Items.AddRange(prefixedUnselectedTags);

            updateCustomScrollBars(sourceTagList);
            updateCustomScrollBars(checkedSourceTagList);
        }

        private void fillTags(bool addReadonlyTagsAlso, bool addArtworkAlso, bool addDateCreatedAlso)
        {
            prepareAvailableTags(addReadonlyTagsAlso, addArtworkAlso, addDateCreatedAlso);
            fillPreparedTags();
        }

        private void fillTagsFromTagSet(int tagSetId)
        {
            selectedTags = new string[SavedSettings.copyTagsTagSets[tagSetId].tagIds.Length];

            for (var i = 0; i < SavedSettings.copyTagsTagSets[SavedSettings.lastInteractiveTagSet].tagIds.Length; i++)
            {
                var tagId = SavedSettings.copyTagsTagSets[SavedSettings.lastInteractiveTagSet].tagIds[i];
                if (tagId == 0)
                {
                    MessageBox.Show(MbForm, "Tag set \"" + tagSetComboBoxCustom.Text + "\" is corrupted!");
                    SavedSettings.copyTagsTagSets[SavedSettings.lastInteractiveTagSet].tagIds = Array.Empty<int>();
                    return;
                }

                var tagName = GetTagName((MetaDataType)tagId);
                selectedTags[i] = tagName;
            }

            fillPreparedTags();
        }

        internal static bool CopyTagsToClipboardUsingTagSet(int tagSet)
        {
            MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out var files);
            if (files == null || files.Length == 0)
            {
                MessageBox.Show(MbForm, MsgNoTracksSelected, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            SavedSettings.lastTagSet = tagSet;

            var clipboardText = string.Empty;


            //Let's copy tag names as the 1st row
            var tagNames = string.Empty;
            for (var i = 0; i < SavedSettings.copyTagsTagSets[tagSet].tagIds.Length; i++)
            {
                var tag = GetTagName((MetaDataType)SavedSettings.copyTagsTagSets[tagSet].tagIds[i]);
                tag += '\t';

                tagNames += tag;
            }

            tagNames = tagNames.Remove(tagNames.Length - 1);

            clipboardText += tagNames + '\n';


            foreach (var file in files)
            {
                var tags = string.Empty;
                for (var i = 0; i < SavedSettings.copyTagsTagSets[tagSet].tagIds.Length; i++)
                {
                    var tag = GetFileTag(file, (MetaDataType)SavedSettings.copyTagsTagSets[tagSet].tagIds[i]);
                    tag = tag.Replace('\u0000', '\u0006').Replace('\u000D', '\u0007').Replace('\u000A', '\u0008');
                    tag += '\t';

                    if (tag.Length > 4000000)
                        tag = "\t";

                    tags += tag;
                }

                tags = tags.Remove(tags.Length - 1);

                clipboardText += tags + '\n';
            }

            clipboardText = clipboardText.Remove(clipboardText.Length - 1);

            //if (string.IsNullOrEmpty(clipboardText))
            //   clipboardText = "\u0000";


            NativeMethods.CloseClipboard();
            Clipboard.Clear();
            Clipboard.SetText(clipboardText);

            return true;
        }

        private void saveCurrentIds()
        {
            var displayedArtistOffset = 0;
            var displayedComposerOffset = 0;
            var checkedIds = new List<int>();

            for (var i = 0; i < checkedSourceTagList.Items.Count; i++)
            {
                var id = (int)GetTagId(checkedSourceTagList.Items[i] as string);

                checkedIds.Add(id);

                if ((checkedSourceTagList.Items[i] as string).Substring(2) == DisplayedArtistName) //Displayed artist should be copied to clipboard the first or second
                {
                    if (displayedArtistOffset == 0)
                        displayedComposerOffset = 1;

                    checkedIds[checkedIds.Count - 1] = checkedIds[displayedArtistOffset];
                    checkedIds[displayedArtistOffset] = id;
                }
                else if ((checkedSourceTagList.Items[i] as string).Substring(2) == DisplayedComposerName) //Displayed composer should be copied to clipboard the first or second
                {
                    if (displayedComposerOffset == 0)
                        displayedArtistOffset = 1;

                    checkedIds[checkedIds.Count - 1] = checkedIds[displayedComposerOffset];
                    checkedIds[displayedComposerOffset] = id;
                }
            }

            SavedSettings.copyTagsTagSets[SavedSettings.lastInteractiveTagSet].tagIds = new int[checkedIds.Count];

            TagToolsPlugin.addPluginContextMenuItems();
            TagToolsPlugin.addPluginMenuItems();


            checkedIds.CopyTo(SavedSettings.copyTagsTagSets[SavedSettings.lastInteractiveTagSet].tagIds);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (returnSelectedTags)
                processCheckedTags();
            else
                CopyTagsToClipboardUsingTagSet(SavedSettings.lastInteractiveTagSet);

            Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
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
            buttonOK.Enable(returnSelectedTags || checkedSourceTagList.Items.Count > 0);

            updateCustomScrollBars(sourceTagList);
            updateCustomScrollBars(checkedSourceTagList);
        }

        private void sourceTagList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sourceTagList.SelectedIndex != -1)
            {
                checkedSourceTagList.Items.Add(sourceTagList.Items[sourceTagList.SelectedIndex]);
                sourceTagList.Items.RemoveAt(sourceTagList.SelectedIndex);
            }

            saveCurrentIds();
            buttonOK.Enable(true);

            updateCustomScrollBars(sourceTagList);
            updateCustomScrollBars(checkedSourceTagList);
        }

        private void tagSetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tagSetComboBoxCustom.SelectedIndex == -1)
                tagSetComboBoxCustom.SelectedIndex = SavedSettings.lastInteractiveTagSet;
            else if (tagSetComboBoxCustom.SelectedIndex == SavedSettings.lastInteractiveTagSet)
                return;
            else
                SavedSettings.lastInteractiveTagSet = tagSetComboBoxCustom.SelectedIndex;

            currentPrefix = SavedSettings.copyTagsTagSets[SavedSettings.lastInteractiveTagSet].getPrefix();

            tagSetComboBoxCustom.SelectionStart = currentPrefix.Length;
            tagSetComboBoxCustom.SelectionLength = 0;

            fillTagsFromTagSet(SavedSettings.lastInteractiveTagSet);
        }

        private void tagSetComboBox_TextChanged(object sender, EventArgs e)
        {
            if (ignoreTagSetComboBoxTextChanged || tagSetComboBoxCustom.SelectedIndex != SavedSettings.lastInteractiveTagSet)
                return;


            ignoreTagSetComboBoxTextChanged = true;

            if (tagSetComboBoxCustom.Text.Substring(0, currentPrefix.Length) != currentPrefix)
                tagSetComboBoxCustom.Text = currentPrefix + tagSetComboBoxCustom.Text.Substring(currentPrefix.Length);


            tagSetComboBoxCustom.Items[SavedSettings.lastInteractiveTagSet] = tagSetComboBoxCustom.Text;
            SavedSettings.copyTagsTagSets[SavedSettings.lastInteractiveTagSet].setName = tagSetComboBoxCustom.Text;

            ignoreTagSetComboBoxTextChanged = false;
        }

        private void checkUncheckAllCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //checkedSourceTagList.SuspendPainting();
            //sourceTagList.SuspendPainting();


            if (checkUncheckAllCheckBox.Checked)
                selectedTags = availableTags.Clone() as string[];
            else
                selectedTags = Array.Empty<string>();

            fillPreparedTags();


            //checkedSourceTagList.Repaint();
            //sourceTagList.Repaint();
        }

        private void checkUncheckAllCheckBoxLabel_Click(object sender, EventArgs e)
        {
            checkUncheckAllCheckBox.Checked = !checkUncheckAllCheckBox.Checked;
        }

        private void CopyTagsToClipboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            TagToolsPlugin.addCopyTagsToClipboardUsingMenuItems();
            TagToolsPlugin.addCopyTagsToClipboardUsingContextMenuItems();
        }
    }


    partial class Plugin
    {
        internal void copyTagsUsingTagSet1EventHandler(object sender, EventArgs e)
        {
            CopyTagsToClipboard.CopyTagsToClipboardUsingTagSet(0);
        }

        internal void copyTagsUsingTagSet2EventHandler(object sender, EventArgs e)
        {
            CopyTagsToClipboard.CopyTagsToClipboardUsingTagSet(1);
        }

        internal void copyTagsUsingTagSet3EventHandler(object sender, EventArgs e)
        {
            CopyTagsToClipboard.CopyTagsToClipboardUsingTagSet(2);
        }

        internal void copyTagsUsingTagSet4EventHandler(object sender, EventArgs e)
        {
            CopyTagsToClipboard.CopyTagsToClipboardUsingTagSet(3);
        }

        internal void copyTagsUsingTagSet5EventHandler(object sender, EventArgs e)
        {
            CopyTagsToClipboard.CopyTagsToClipboardUsingTagSet(4);
        }

        internal void copyTagsUsingTagSet6EventHandler(object sender, EventArgs e)
        {
            CopyTagsToClipboard.CopyTagsToClipboardUsingTagSet(5);
        }

        internal void copyTagsUsingTagSet7EventHandler(object sender, EventArgs e)
        {
            CopyTagsToClipboard.CopyTagsToClipboardUsingTagSet(6);
        }

        internal void copyTagsUsingTagSet8EventHandler(object sender, EventArgs e)
        {
            CopyTagsToClipboard.CopyTagsToClipboardUsingTagSet(7);
        }

        internal void copyTagsUsingTagSet9EventHandler(object sender, EventArgs e)
        {
            CopyTagsToClipboard.CopyTagsToClipboardUsingTagSet(8);
        }

        internal void copyTagsUsingTagSet10EventHandler(object sender, EventArgs e)
        {
            CopyTagsToClipboard.CopyTagsToClipboardUsingTagSet(9);
        }
    }
}
