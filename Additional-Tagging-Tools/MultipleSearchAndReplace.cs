using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static MusicBeePlugin.AdvancedSearchAndReplaceCommand;


namespace MusicBeePlugin
{
    public partial class MultipleSearchAndReplaceCommand : PluginWindowTemplate
    {
        private delegate void AddRowToTable(string[] row);
        private delegate void ProcessRowOfTable(int row);
        private AddRowToTable addRowToTable;
        private ProcessRowOfTable processRowOfTable;

        private Plugin.MetaDataType sourceTagId;
        private Plugin.FilePropertyType sourcePropId;
        private Plugin.MetaDataType destinationTagId;

        private Plugin.MetaDataType[] sourceTagIds;
        private Plugin.FilePropertyType[] sourcePropIds;
        private string[] sourceTagNames;

        private string[] files = new string[0];
        private List<string[]> tags = new List<string[]>();
        private List<string[]> templates = new List<string[]>();

        //private bool ignoreTemplateNameTextBoxTextChanged = false;

        private Preset customMSR;

        private bool searchOnly = false;

        public MultipleSearchAndReplaceCommand()
        {
            InitializeComponent();
        }

        public MultipleSearchAndReplaceCommand(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();


            //Setting themed images
            buttonDeleteSaved.Image = Plugin.ButtonRemoveImage;
            autoApplyPictureBox.Image = Plugin.AutoAppliedPresetsAccent;


            Plugin.FillListByTagNames(destinationTagList.Items, false, false, false);
            destinationTagList.Text = Plugin.SavedSettings.copyDestinationTagName;

            Plugin.FillListByTagNames(sourceTagList.Items, true, false, false);
            sourceTagList.Text = Plugin.SavedSettings.copySourceTagName;

            Plugin.FillListByTagNames(((DataGridViewComboBoxColumn)templateTable.Columns[0]).Items, true, false, false);


            DatagridViewCheckBoxHeaderCell cbHeader = new DatagridViewCheckBoxHeaderCell();
            cbHeader.setState(true);
            cbHeader.OnCheckBoxClicked += new CheckBoxClickedHandler(cbHeader_OnCheckBoxClicked);

            DataGridViewCheckBoxColumn colCB = new DataGridViewCheckBoxColumn();
            colCB.HeaderCell = cbHeader;
            colCB.ThreeState = true;
            colCB.FalseValue = "F";
            colCB.TrueValue = "T";
            colCB.IndeterminateValue = "";
            colCB.Width = 25;
            colCB.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            previewTable.Columns.Insert(0, colCB);

            addRowToTable = previewList_AddRowToTable;
            processRowOfTable = previewList_ProcessRowOfTable;

            (int, int, int, int, int, int, int) value = loadWindowLayout();

            if (value.Item1 > 0)
            {
                previewTable.Columns[1].Width = value.Item1;
                previewTable.Columns[2].Width = value.Item2;
                previewTable.Columns[3].Width = value.Item3;

                templateTable.Columns[0].Width = value.Item5;
                templateTable.Columns[3].Width = value.Item6;
                templateTable.Columns[4].Width = value.Item7;
            }

            if (value.Item4 > 0)
            {
                splitContainer1.SplitterDistance = value.Item4;
            }

            customMSR = null;

            loadComboBox.Sorted = false;
            loadComboBox.Items.Add(Plugin.CtlNewASRPreset);
            foreach (var preset in Presets)
            {
                if (preset.Value.isMSRPreset)
                {
                    loadComboBox.Items.Add(preset.Value);
                }
            }
            loadComboBox.SelectedIndex = 0;

            buttonAdd_Click(null, null);

            templateNameTextBox.Text = Plugin.CtlMSR;
            templateNameTextBox.SelectionStart = Plugin.CtlMSR.Length;
        }

        private void previewList_AddRowToTable(string[] rows)
        {
            if (backgroundTaskIsWorking())
                previewTable.Rows.Add(rows);
        }

        private void previewList_ProcessRowOfTable(int row)
        {
            previewTable.Rows[row].Cells[0].Value = "";
        }

        private bool prepareBackgroundPreview()
        {
            tags.Clear();
            templates.Clear();
            previewTable.Rows.Clear();

            if (backgroundTaskIsWorking())
                return true;

            searchOnly = searchOnlyCheckBox.Checked;

            sourceTagId = 0;
            sourcePropId = 0;
            destinationTagId = 0;

            sourceTagIds = null;
            sourcePropIds = null;
            sourceTagNames = null;
            if (searchOnly)
            {
                sourceTagIds = new Plugin.MetaDataType[templateTable.Rows.Count];
                sourcePropIds = new Plugin.FilePropertyType[templateTable.Rows.Count];
                sourceTagNames = new string[templateTable.Rows.Count];

                for (int i = 0; i < templateTable.Rows.Count; i++)
                {
                    sourceTagIds[i] = Plugin.GetTagId("" + templateTable.Rows[i].Cells[0].Value);
                    sourcePropIds[i] = Plugin.GetPropId("" + templateTable.Rows[i].Cells[0].Value);
                    sourceTagNames[i] = "" + templateTable.Rows[i].Cells[0].Value;
                }
            }
            else
            {
                sourceTagId = Plugin.GetTagId(sourceTagList.Text);
                sourcePropId = Plugin.GetPropId(sourceTagList.Text);
                destinationTagId = Plugin.GetTagId(destinationTagList.Text);
            }


            files = null;
            if (searchOnly)
            {
                if (!Plugin.MbApiInterface.Library_QueryFilesEx("domain=DisplayedFiles", out files))
                    files = new string[0];
            }
            else
            {
                {
                    if (!Plugin.MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files))
                        files = new string[0];
                }
            }


            if (files.Length == 0 && searchOnly)
            {
                MessageBox.Show(this, Plugin.MsgNoFilesDisplayed, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else if (files.Length == 0 && !searchOnly)
            {
                MessageBox.Show(this, Plugin.MsgNoFilesSelected, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else
            {
                for (int i = 0; i < templateTable.Rows.Count; i++)
                {
                    string[] template = new string[5];

                    template[0] = "" + templateTable.Rows[i].Cells[0].Value;
                    template[1] = "" + templateTable.Rows[i].Cells[1].Value;
                    template[2] = "" + templateTable.Rows[i].Cells[2].Value;
                    template[3] = "" + templateTable.Rows[i].Cells[3].Value;
                    template[4] = "" + templateTable.Rows[i].Cells[4].Value;

                    templates.Add(template);
                }

                return true;
            }
        }

        private bool prepareBackgroundTask()
        {
            if (previewTable.Rows.Count == 0)
            {
                return prepareBackgroundPreview();
            }
            else
            {
                string[] tag;

                tags.Clear();

                for (int fileCounter = 0; fileCounter < previewTable.Rows.Count; fileCounter++)
                {
                    tag = new string[5];

                    tag[0] = (string)previewTable.Rows[fileCounter].Cells[0].Value;
                    tag[3] = (string)previewTable.Rows[fileCounter].Cells[3].Value;
                    tag[4] = (string)previewTable.Rows[fileCounter].Cells[4].Value;

                    tags.Add(tag);
                }

                return true;
            }
        }

        private void previewChanges()
        {
            string currentFile;

            string sourceTagValue = null;
            string track;
            string[] row = { "Checked", "Track", "oldTag", "newTag", "File" };

            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                currentFile = files[fileCounter];

                Plugin.SetStatusbarTextForFileOperations(Plugin.MsrCommandSbText, true, fileCounter, files.Length, currentFile);

                track = Plugin.GetTrackRepresentation(currentFile);
                row = new string[5];

                if (searchOnly)
                {
                    List<string> tagValues = new List<string>();
                    List<string> tagNames = new List<string>();
                    string sourceTagName;

                    bool areChanges = true;
                    int i = 0;
                    while (i < sourceTagIds.Length && areChanges)
                    {
                        if (sourcePropIds[i] == 0)
                        {
                            sourceTagValue = Plugin.GetFileTag(currentFile, sourceTagIds[i]);
                        }
                        else
                        {
                            sourceTagValue = Plugin.MbApiInterface.Library_GetFileProperty(currentFile, sourcePropIds[i]);
                        }
                        sourceTagName = sourceTagNames[i];

                        string newSourceTagValue;
                        bool match;

                        try
                        {
                            string template0 = templates[i][0];
                            string template1 = templates[i][1];
                            string template2 = templates[i][2];
                            string template3 = templates[i][3];

                            if (template1 == "T" && template2 == "T") //Regex/case sensitive
                            {
                                newSourceTagValue = Replace(currentFile, sourceTagValue, template3, "", "", false, out match);
                            }
                            else if (template1 == "T" && template2 == "F") //Regex/case insensitive
                            {
                                newSourceTagValue = Replace(currentFile, sourceTagValue, template3, "", "", true, out match);
                            }
                            else if (template1 == "F" && template2 == "T") //Case sensitive
                            {
                                template3 = Regex.Escape(template3);
                                newSourceTagValue = Replace(currentFile, sourceTagValue, template3, "", "", false, out match);
                            }
                            else //Case insensitive
                            {
                                template3 = Regex.Escape(template3);
                                newSourceTagValue = Replace(currentFile, sourceTagValue, template3, "", "", true, out match);
                            }

                            if (sourceTagValue == newSourceTagValue)
                            {
                                areChanges = false;
                            }
                            else
                            {
                                tagValues.Add(sourceTagValue);
                                tagNames.Add(sourceTagName);
                            }
                        }
                        catch
                        {
                            sourceTagValue = "SYNTAX ERROR!";
                            tagValues.Add(sourceTagValue);
                            tagNames.Add(sourceTagName);
                        }

                        i++;
                    }

                    if (areChanges)
                    {
                        if (tagValues.Count == 1)
                        {
                            row[0] = "T";
                            row[1] = track;
                            row[2] = sourceTagValue;
                            row[3] = "";
                            row[4] = currentFile;

                            Invoke(addRowToTable, new object[] { row });
                            tags.Add(row);
                        }
                        else
                        {
                            row[0] = "T";
                            row[1] = track;
                            row[2] = "";
                            row[3] = "";
                            row[4] = currentFile;

                            Invoke(addRowToTable, new object[] { row });
                            tags.Add(row);

                            for (int j = 0; j < tagValues.Count; j++)
                            {
                                {
                                    row[0] = null;
                                    row[1] = "            " + tagNames[j];
                                    row[2] = tagValues[j];
                                    row[3] = "";
                                    row[4] = "";

                                    Invoke(addRowToTable, new object[] { row });
                                    tags.Add(row);
                                }
                            }
                        }
                    }
                }
                else
                {
                    string destinationTagValue;
                    string newDestinationTagValue1;
                    string newDestinationTagValue2;

                    if (sourcePropId == 0)
                    {
                        sourceTagValue = Plugin.GetFileTag(currentFile, sourceTagId);
                    }
                    else
                    {
                        sourceTagValue = Plugin.MbApiInterface.Library_GetFileProperty(currentFile, sourcePropId);
                    }

                    destinationTagValue = Plugin.GetFileTag(currentFile, destinationTagId);

                    bool match;
                    newDestinationTagValue1 = null;
                    newDestinationTagValue2 = sourceTagValue;
                    foreach (string[] template in templates)
                    {
                        try
                        {
                            string template0 = template[0];
                            string template1 = template[1];
                            string template2 = template[2];
                            string template3 = template[3];
                            string template4 = template[4];

                            if (template1 == "T" && template2 == "T") //Regex/case sensitive
                            {
                                newDestinationTagValue1 = Replace(currentFile, newDestinationTagValue2, template3, template4, "", false, out match);
                            }
                            else if (template1 == "T" && template2 == "F") //Regex/case insensitive
                            {
                                newDestinationTagValue1 = Replace(currentFile, newDestinationTagValue2, template3, template4, "", true, out match);
                            }
                            else if (template1 == "F" && template2 == "T") //Case sensitive
                            {
                                template3 = Regex.Escape(template[3]);
                                newDestinationTagValue1 = Replace(currentFile, newDestinationTagValue2, template3, template4, "", false, out match);
                            }
                            else //Case insensitive
                            {
                                template3 = Regex.Escape(template[3]);
                                newDestinationTagValue1 = Replace(currentFile, newDestinationTagValue2, template3, template4, "", true, out match);
                            }

                            newDestinationTagValue2 = newDestinationTagValue1;
                        }
                        catch
                        {
                            newDestinationTagValue1 = "SYNTAX ERROR!";
                            break;
                        }
                    }

                    if (destinationTagValue == newDestinationTagValue1)
                        continue;


                    row[0] = "T";
                    row[1] = track;
                    row[2] = destinationTagValue;
                    row[3] = newDestinationTagValue1;
                    row[4] = currentFile;

                    Invoke(addRowToTable, new object[] { row });
                    tags.Add(row);
                }

                previewIsGenerated = true;
            }

            Plugin.SetResultingSbText();
        }

        private void applyChanges()
        {
            string currentFile;
            string newTag;
            string isChecked;

            if (tags.Count == 0)
                previewChanges();

            for (int i = 0; i < tags.Count; i++)
            {
                if (backgroundTaskIsCanceled || !backgroundTaskIsScheduled)
                    return;

                isChecked = tags[i][0];

                if (isChecked == "T")
                {
                    currentFile = tags[i][4];
                    newTag = tags[i][3];

                    if (newTag != "SYNTAX ERROR!")
                    {
                        tags[i][0] = "";

                        Invoke(processRowOfTable, new object[] { i });

                        Plugin.SetStatusbarTextForFileOperations(Plugin.MsrCommandSbText, false, i, tags.Count, currentFile);

                        Plugin.SetFileTag(currentFile, destinationTagId, newTag);
                        Plugin.CommitTagsToFile(currentFile);
                    }
                }
            }

            Plugin.RefreshPanels(true);

            Plugin.SetResultingSbText();
        }

        private void saveSettings()
        {
            Plugin.SavedSettings.copySourceTagName = sourceTagList.Text;
            Plugin.SavedSettings.copyDestinationTagName = destinationTagList.Text;

            TagToolsPlugin.SaveSettings();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (searchOnlyCheckBox.Checked)
            {
                //***
            }
            else
            {
                saveSettings();
                if (prepareBackgroundTask())
                    switchOperation(applyChanges, (Button)sender, buttonOK, buttonPreview, buttonCancel, true, null);
            }
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            if (searchOnlyCheckBox.Checked)
            {
                clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewChanges, (Button)sender, buttonOK, buttonCancel, 2);
            }
            else
            {
                clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewChanges, (Button)sender, buttonOK, buttonCancel, 0);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbHeader_OnCheckBoxClicked(bool state)
        {
            foreach (DataGridViewRow row in previewTable.Rows)
            {
                if (state && row.Cells[0].Value != null)
                    row.Cells[0].Value = "T";
                else if (row.Cells[0].Value != null)
                    row.Cells[0].Value = "F";
            }
        }

        private void previewTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string isChecked = (string)previewTable.Rows[e.RowIndex].Cells[0].Value;

                if (isChecked == "T")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "F";
                }
                else if (isChecked == "F")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "T";
                }
            }
        }

        public override void enableDisablePreviewOptionControls(bool enable)
        {
            bool intialEnabled = enable;
            enable = (enable && previewTable.Rows.Count == 0) || searchOnlyCheckBox.Checked;

            searchOnlyCheckBox.Enabled = intialEnabled && !backgroundTaskIsWorking();

            templateNameTextBox.Enabled = enable && !searchOnlyCheckBox.Checked;
            //ignoreTemplateNameTextBoxTextChanged = true;
            templateNameTextBox_TextChanged(null, null);
            //ignoreTemplateNameTextBoxTextChanged = false;

            sourceTagList.Enabled = enable && !searchOnlyCheckBox.Checked;
            destinationTagList.Enabled = enable && !autoDestinationTagCheckBox.Checked && !searchOnlyCheckBox.Checked;
            label1.Enabled = enable && !searchOnlyCheckBox.Checked;
            label2.Enabled = enable && !searchOnlyCheckBox.Checked;
            label3.Enabled = enable && !searchOnlyCheckBox.Checked;
            buttonSave.Enabled = enable && !searchOnlyCheckBox.Checked;
            loadComboBox.Enabled = enable && !searchOnlyCheckBox.Checked;
            buttonAdd.Enabled = enable;
            templateTable.Enabled = enable;
            autoDestinationTagCheckBox.Enabled = enable && !searchOnlyCheckBox.Checked;

            dataGridViewTextBoxColumn16.Visible = !searchOnlyCheckBox.Checked;
            NewTag.Visible = !searchOnlyCheckBox.Checked;
            if (searchOnlyCheckBox.Checked)
            {
                OriginalTag.HeaderText = Plugin.OriginalTagHeaderTextTagValue;
                SearchTag.Visible = true;
            }
            else
            {
                OriginalTag.HeaderText = Plugin.OriginalTagHeaderText;
                SearchTag.Visible = false;
            }

            if (templateTable.Rows.Count == 1)
            {
                buttonUp.Enabled = false;
                buttonDown.Enabled = false;
                buttonDelete.Enabled = false;
            }
            else
            {
                buttonUp.Enabled = enable && !searchOnlyCheckBox.Checked;
                buttonDown.Enabled = enable && !searchOnlyCheckBox.Checked;
                buttonDelete.Enabled = enable;
            }

            if (searchOnlyCheckBox.Checked)
            {
                buttonOK.Enabled = intialEnabled && (previewTable.Rows.Count > 0);
                buttonOK.Text = Plugin.SelectFoundButtonName;
                if (!backgroundTaskIsWorking())
                {
                    buttonPreview.Text = Plugin.FindButtonName;
                }
            }
            else if (intialEnabled)
            {
                buttonOK.Text = Plugin.OKButtonName;
                if (previewTable.Rows.Count == 0)
                {
                    buttonPreview.Text = Plugin.PreviewButtonName;
                }
                else
                {
                    buttonPreview.Text = Plugin.ClearButtonName;
                }
            }
        }

        public override void enableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonPreview, " ");
            dirtyErrorProvider.SetError(buttonPreview, string.Empty);
        }

        public override void disableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonPreview, getBackgroundTasksWarning());
        }

        public override void enableQueryingOrUpdatingButtons()
        {
            buttonOK.Enabled = true;
            buttonPreview.Enabled = true;
        }

        public override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enabled = false;
            buttonPreview.Enabled = false;
        }

        //private void previewTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{
        //    if ((string)previewTable.Rows[e.RowIndex].Cells[1].Value == (string)previewTable.Rows[e.RowIndex].Cells[2].Value)
        //        previewTable.Rows[e.RowIndex].Cells[2].Style = Plugin.UnchangedStyle;
        //    else
        //        previewTable.Rows[e.RowIndex].Cells[2].Style = Plugin.ChangedStyle;
        //}

        public static string EncodeSearchReplaceTemplate(string searchTemplate, string replaceTemplate, bool useRegexes, bool caseSensetive)
        {
            searchTemplate = searchTemplate.Replace(@"\", @"\\").Replace(@"#", @"\#").Replace("/", @"\L").Replace("|", @"\V").Replace(@"*", @"\X");
            replaceTemplate = replaceTemplate.Replace(@"\", @"\\").Replace(@"#", @"\#").Replace("/", @"\L").Replace("|", @"\V").Replace(@"*", @"\X").Replace(@"$", @"\T");

            string query;

            if (caseSensetive && useRegexes)
            {
                query = "#*" + searchTemplate + "/" + replaceTemplate;
            }
            else if (!caseSensetive && useRegexes)
            {
                query = "*" + searchTemplate + "/" + replaceTemplate;
            }
            else if (caseSensetive && !useRegexes)
            {
                query = "#" + searchTemplate + "/" + replaceTemplate;
            }
            else //if (!caseSensetive && !useRegexes)
            {
                query = searchTemplate + "/" + replaceTemplate;
            }

            return query;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (Plugin.MSR == null)
            {
                MessageBox.Show(this, Plugin.MsgYouMustImportStandardASRPresetsFirst, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (templateNameTextBox.Text == Plugin.CtlMSR)
            {
                MessageBox.Show(this, Plugin.MsgGiveNameToASRpreset, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                templateNameTextBox.Focus();
                templateNameTextBox.SelectionStart = Plugin.CtlMSR.Length;
                return;
            }


            bool presetExists = true;

            if (loadComboBox.SelectedIndex == 0)
            {
                presetExists = false;

                foreach (var preset in Presets)
                {
                    if (preset.Value.getName() == templateNameTextBox.Text)
                    {
                        presetExists = true;
                        customMSR = preset.Value;
                        break;
                    }
                }

                if (!presetExists)
                {
                    customMSR = new Preset(Plugin.MSR, false, false, null, null);
                }
            }

            if (!presetExists)
            {
                if (MessageBox.Show(this, Plugin.MsgAreYouSureYouWantToSaveASRpreset
                    .Replace("%%PRESETNAME%%", templateNameTextBox.Text), 
                        Plugin.MsgSavePreset, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }
            else if (templateNameTextBox.Text == customMSR.getName())
            {
                if (MessageBox.Show(this, Plugin.MsgAreYouSureYouWantToOverwriteASRpreset
                    .Replace("%%PRESETNAME%%", templateNameTextBox.Text),
                        Plugin.MsgSavePreset, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }
            else
            {
                if (MessageBox.Show(this, Plugin.MsgAreYouSureYouWantToOverwriteRenameASRpreset
                    .Replace("%%PRESETNAME%%", customMSR.getName())
                    .Replace("%%NEWPRESETNAME%%", templateNameTextBox.Text),
                        Plugin.MsgSavePreset, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }

            string query = "";
            for (int i = 0; i < templateTable.Rows.Count; i++)
            {
                query += EncodeSearchReplaceTemplate("" + templateTable.Rows[i].Cells[3].Value, "" + templateTable.Rows[i].Cells[4].Value, (string)templateTable.Rows[i].Cells[1].Value == "T" ? true : false, (string)templateTable.Rows[i].Cells[2].Value == "T" ? true : false) + "|";
            }

            query = query.Remove(query.Length - 1);


            string oldSafeFileName = customMSR.getSafeFileName();


            List<string> names = new List<string>();
            foreach (var name in customMSR.names)
            {
                names.Add(name.Key);
            }

            customMSR.names.Clear();

            foreach (var name in names)
            {
                customMSR.names.Add(name, templateNameTextBox.Text);
            }

            int sourceTagIdInt;
            int sourcePropIdInt;

            sourceTagIdInt = (int)Plugin.GetTagId(sourceTagList.Text);
            sourcePropIdInt = (int)Plugin.GetPropId(sourceTagList.Text);
            destinationTagId = Plugin.GetTagId(destinationTagList.Text);
            if (sourcePropIdInt != 0)
                sourceTagIdInt = sourcePropIdInt;

            customMSR.customText = query;
            customMSR.parameterTagId = sourceTagIdInt;
            customMSR.parameterTag2Id = (int)destinationTagId;
            customMSR.isMSRPreset = true;
            customMSR.userPreset = true;

            if (File.Exists(Path.Combine(PresetsPath, oldSafeFileName + Plugin.ASRPresetExtension)))
                File.Delete(Path.Combine(PresetsPath, oldSafeFileName + Plugin.ASRPresetExtension));

            customMSR.savePreset(Path.Combine(PresetsPath, customMSR.getSafeFileName() + Plugin.ASRPresetExtension));

            if (Presets.TryGetValue(customMSR.guid, out _))
                Presets.Remove(customMSR.guid);

            Presets.Add(customMSR.guid, customMSR);

            if (autoApplyCheckBox.Checked && !Plugin.SavedSettings.autoAppliedAsrPresetGuids.Contains(customMSR.guid))
            {
                Plugin.AsrAutoAppliedPresets.Add(customMSR);
                Plugin.SavedSettings.autoAppliedAsrPresetGuids.Add(customMSR.guid);
            }
            else if (!autoApplyCheckBox.Checked && Plugin.SavedSettings.autoAppliedAsrPresetGuids.Contains(customMSR.guid))
            {
                Plugin.AsrAutoAppliedPresets.Remove(customMSR);
                Plugin.SavedSettings.autoAppliedAsrPresetGuids.Remove(customMSR.guid);
            }

            if (loadComboBox.SelectedIndex > 0)
            {
                int index = loadComboBox.SelectedIndex;
                loadComboBox.Items.RemoveAt(index);
                loadComboBox.Items.Insert(index, customMSR);
            }
            else
            {
                loadComboBox.Items.Add(customMSR);
            }
            loadComboBox.SelectedItem = customMSR;
            loadComboBox.Enabled = sourceTagList.Enabled;
            buttonDeleteSaved.Enabled = true;

            TagToolsPlugin.SaveSettings();
        }

        private void templateNameTextBox_TextChanged(object sender, EventArgs e)
        {
            //if (!ignoreTemplateNameTextBoxTextChanged)
            //    loadComboBox.SelectedIndex = 0;

            if (templateNameTextBox.Text == "")
                buttonSave.Enabled = false;
            else
                buttonSave.Enabled = true;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            templateTable.Rows.Add();
            if (templateTable.Rows.Count == 1)
            {
                templateTable.Rows[templateTable.Rows.Count - 1].Cells[0].Value = Plugin.SavedSettings.copySourceTagName;
            }
            else
            {
                templateTable.Rows[templateTable.Rows.Count - 1].Cells[0].Value = templateTable.Rows[templateTable.Rows.Count - 2].Cells[0].Value;
            }
            templateTable.Rows[templateTable.Rows.Count - 1].Cells[5].Value = (templateTable.Rows.Count).ToString("D8");
            templateTable.Rows[templateTable.Rows.Count - 1].Cells[1].Value = "F";
            templateTable.Rows[templateTable.Rows.Count - 1].Cells[2].Value = "F";
            templateTable.Rows[templateTable.Rows.Count - 1].Selected = true;


            if (templateTable.Rows.Count > 1)
            {
                buttonDelete.Enabled = true;
                buttonUp.Enabled = !searchOnlyCheckBox.Checked;
                buttonDown.Enabled = !searchOnlyCheckBox.Checked;
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int index = templateTable.CurrentRow.Index;
            if (index >= 0)
            {
                if (index == templateTable.Rows.Count - 1)
                    templateTable.Rows[index].Selected = false;

                templateTable.Rows.RemoveAt(index);

                for (int i = 0; i < templateTable.Rows.Count; i++)
                {
                    templateTable.Rows[i].Cells[5].Value = i.ToString("D8");
                }

                if (templateTable.Rows.Count < 2)
                {
                    buttonUp.Enabled = false;
                    buttonDown.Enabled = false;
                    buttonDelete.Enabled = false;
                }
            }
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            int index = templateTable.CurrentRow.Index;

            if (index > 0)
            {
                templateTable.Rows[index].Cells[5].Value = (index - 1).ToString("D8");
                templateTable.Rows[index - 1].Cells[5].Value = index.ToString("D8");
                templateTable.Sort(templateTable.Columns[5], System.ComponentModel.ListSortDirection.Ascending);
            }
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            int index = templateTable.CurrentRow.Index;

            if (index < templateTable.Rows.Count - 1)
            {
                templateTable.Rows[index].Cells[5].Value = (index + 1).ToString("D8");
                templateTable.Rows[index + 1].Cells[5].Value = index.ToString("D8");
                templateTable.Sort(templateTable.Columns[5], System.ComponentModel.ListSortDirection.Ascending);
            }
        }

        private void loadComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loadComboBox.SelectedIndex == 0)
            {
                customMSR = null;
                templateNameTextBox.Text = Plugin.CtlMSR;
                templateNameTextBox.SelectionStart = Plugin.CtlMSR.Length;
                buttonDeleteSaved.Enabled = false;
                autoApplyCheckBox.Checked = false;
                return;
            }
            else
            {
                customMSR = (Preset)loadComboBox.SelectedItem;
            }

            buttonDeleteSaved.Enabled = true;

            if (Plugin.SavedSettings.autoAppliedAsrPresetGuids.Contains(customMSR.guid))
                autoApplyCheckBox.Checked = true;
            else
                autoApplyCheckBox.Checked = false;

            //ignoreTemplateNameTextBoxTextChanged = true;
            templateNameTextBox.Text = customMSR.getName();
            //ignoreTemplateNameTextBoxTextChanged = false;

            templateTable.Rows.Clear();

            sourceTagList.SelectedItem = Plugin.GetTagName((Plugin.MetaDataType)customMSR.parameterTagId);
            destinationTagList.SelectedItem = Plugin.GetTagName((Plugin.MetaDataType)customMSR.parameterTag2Id);

            string[] queries = customMSR.customText.Split('|');
            string[] pair;

            foreach (string query in queries)
            {
                buttonAdd_Click(null, null);

                if (query.Length > 1 && query[0] == '#' && query[1] == '*') //Case sensitive/regexes
                {
                    pair = query.Substring(2).Split('/');

                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[1].Value = "T";
                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[2].Value = "T";
                }
                else if (query.Length > 0 && query[0] == '*') //Case insensitive/regexes
                {
                    pair = query.Substring(1).Split('/');

                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[1].Value = "T";
                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[2].Value = "F";
                }
                else if (query.Length > 0 && query[0] == '#') //Case sensitive
                {
                    pair = query.Substring(1).Split('/');

                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[1].Value = "F";
                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[2].Value = "T";
                }
                else //Case insensitive
                {
                    pair = query.Split('/');

                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[1].Value = "F";
                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[2].Value = "F";
                }

                pair[0] = pair[0].Replace(@"\#", "#").Replace(@"\L", "/").Replace(@"\V", "|").Replace(@"\X", "*").Replace(@"\\", @"\");
                pair[1] = pair[1].Replace(@"\#", "#").Replace(@"\L", "/").Replace(@"\V", "|").Replace(@"\X", "*").Replace(@"\\", @"\").Replace(@"\T", @"$");

                templateTable.Rows[templateTable.Rows.Count - 1].Cells[3].Value = pair[0];
                templateTable.Rows[templateTable.Rows.Count - 1].Cells[4].Value = pair[1];
            }
        }

        private void autoDestinationTagCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (autoDestinationTagCheckBox.Checked)
            {
                destinationTagList.Enabled = false;
                int destinationTagListSelectedIndex = destinationTagList.SelectedIndex;
                destinationTagList.SelectedItem = sourceTagList.SelectedItem;

                if (destinationTagList.SelectedIndex == -1)
                {
                    destinationTagList.SelectedIndex = destinationTagListSelectedIndex;
                }
            }
            else
            {
                destinationTagList.Enabled = !searchOnlyCheckBox.Checked;
            }
        }

        private void sourceTagList_SelectedIndexChanged(object sender, EventArgs e)
        {
                autoDestinationTagCheckBox_CheckedChanged(null, null);
        }

        private void SearchOnlyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            enableDisablePreviewOptionControls(true);
            clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewChanges, buttonPreview, buttonOK, buttonCancel, 1);
        }

        private void buttonDeleteSaved_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, Plugin.MsgAreYouSureYouWantToDeleteASRpreset.Replace("%%PRESETNAME%%", templateNameTextBox.Text), 
                    Plugin.MsgDeletePreset, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;


            if (Plugin.SavedSettings.autoAppliedAsrPresetGuids.Contains(customMSR.guid))
            {
                Plugin.SavedSettings.autoAppliedAsrPresetGuids.Remove(customMSR.guid);
            }

            for (int j = 0; j < Plugin.AsrPresetsWithHotkeys.Length; j++)
            {
                if (Plugin.AsrPresetsWithHotkeys[j] == customMSR)
                {
                    Plugin.AsrPresetsWithHotkeys[j] = null;
                    Plugin.SavedSettings.asrPresetsWithHotkeysGuids[j] = Guid.Empty;
                    Plugin.ASRPresetsWithHotkeysCount--;
                }
            }

            foreach (var pair in Plugin.IdsAsrPresets)
            {
                if (pair.Value == customMSR)
                    Plugin.IdsAsrPresets.Remove(pair.Key);
            }

            File.Delete(Path.Combine(PresetsPath, customMSR.getSafeFileName() + Plugin.ASRPresetExtension));

            Presets.Remove(customMSR.guid);
            loadComboBox.Items.Remove(customMSR);

            loadComboBox.SelectedIndex = 0;
            buttonDeleteSaved.Enabled = false;


            TagToolsPlugin.SaveSettings();
        }

        private void MultipleSearchAndReplaceCommand_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveWindowLayout(previewTable.Columns[1].Width, previewTable.Columns[2].Width, previewTable.Columns[3].Width,
                splitContainer1.SplitterDistance, 
                templateTable.Columns[0].Width, templateTable.Columns[3].Width, templateTable.Columns[4].Width);
        }
    }
}
