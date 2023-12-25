using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static MusicBeePlugin.AdvancedSearchAndReplaceCommand;
using static MusicBeePlugin.Plugin;


namespace MusicBeePlugin
{
    public partial class MultipleSearchAndReplaceCommand : PluginWindowTemplate
    {
        private delegate void AddRowToTable(string[] row);
        private delegate void ProcessRowOfTable(int row);
        private AddRowToTable addRowToTable;
        private ProcessRowOfTable processRowOfTable;

        private MetaDataType sourceTagId;
        private FilePropertyType sourcePropId;
        private MetaDataType destinationTagId;

        private MetaDataType[] sourceTagIds;
        private FilePropertyType[] sourcePropIds;
        private string[] sourceTagNames;

        private string[] files = new string[0];
        private List<string[]> tags = new List<string[]>();
        private List<string[]> templates = new List<string[]>();

        private bool ignoreSplitterMovedEvent = true;
        //private bool ignoreTemplateNameTextBoxTextChanged = false;

        private Preset customMSR;

        private bool searchOnly = false;

        public MultipleSearchAndReplaceCommand(Plugin tagToolsPluginParam) : base(tagToolsPluginParam)
        {
            InitializeComponent();
        }

        protected override void initializeForm()
        {
            base.initializeForm();


            //Setting themed images
            buttonDeleteSaved.Image = ThemedBitmapAddRef(this, ButtonRemoveImage);
            autoApplyPictureBox.Image = ThemedBitmapAddRef(this, AutoAppliedPresetsAccent);
            buttonSettings.Image = ThemedBitmapAddRef(this, Gear);


            FillListByTagNames(destinationTagList.Items, false, false, false);
            destinationTagList.Text = SavedSettings.copyDestinationTagName;

            FillListByTagNames(sourceTagList.Items, true, false, false);
            sourceTagList.Text = SavedSettings.copySourceTagName;

            FillListByTagNames((templateTable.Columns[0] as DataGridViewComboBoxColumn).Items, true, false, false);


            templateTable.EnableHeadersVisualStyles = !UseMusicBeeFontSkinColors;

            templateTable.BackgroundColor = UnchangedCellStyle.BackColor;
            templateTable.DefaultCellStyle = UnchangedCellStyle;

            templateTable.Columns[1].HeaderCell.Style = HeaderCellStyle;
            templateTable.Columns[2].HeaderCell.Style = HeaderCellStyle;
            templateTable.Columns[3].HeaderCell.Style = HeaderCellStyle;
            templateTable.Columns[4].HeaderCell.Style = HeaderCellStyle;

            templateTable.Columns[1].Width = (int)Math.Round(templateTable.Columns[1].Width * hDpiFontScaling);
            templateTable.Columns[2].Width = (int)Math.Round(templateTable.Columns[1].Width * hDpiFontScaling);

            previewTable.EnableHeadersVisualStyles = !UseMusicBeeFontSkinColors;

            previewTable.BackgroundColor = UnchangedCellStyle.BackColor;
            previewTable.DefaultCellStyle = UnchangedCellStyle;

            DatagridViewCheckBoxHeaderCell cbHeader = new DatagridViewCheckBoxHeaderCell();
            cbHeader.Style = HeaderCellStyle;
            cbHeader.setState(true);
            cbHeader.OnCheckBoxClicked += new CheckBoxClickedHandler(cbHeader_OnCheckBoxClicked);

            DataGridViewCheckBoxColumn colCB = new DataGridViewCheckBoxColumn
            {
                HeaderCell = cbHeader,
                ThreeState = true,
                FalseValue = "F",
                TrueValue = "T",
                IndeterminateValue = string.Empty,
                Width = 25,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };

            previewTable.Columns.Insert(0, colCB);

            previewTable.Columns[1].HeaderCell.Style = HeaderCellStyle;
            previewTable.Columns[2].HeaderCell.Style = HeaderCellStyle;
            previewTable.Columns[3].HeaderCell.Style = HeaderCellStyle;

            addRowToTable = previewTable_AddRowToTable;
            processRowOfTable = previewTable_ProcessRowOfTable;


            customMSR = null;

            loadComboBox.Sorted = false;
            loadComboBox.Items.Add(CtlNewAsrPreset);
            foreach (var preset in Presets)
            {
                if (preset.Value.isMSRPreset)
                {
                    loadComboBox.Items.Add(preset.Value);
                }
            }
            loadComboBox.SelectedIndex = 0;

            buttonAdd_Click(null, null);

            templateNameTextBox.Text = CtlMSR;
            templateNameTextBox.SelectionStart = CtlMSR.Length;


            enableDisablePreviewOptionControls(true, true);
            enableQueryingOrUpdatingButtons();

            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        public override void preMoveScaleControl(Control control)
        {
            if (control.Name == "loadComboBox")
                loadComboBox.Width = loadComboBox.Width - (int)Math.Round(7 * loadComboBox.Margin.Left - 7 * loadComboBox.Margin.Left / hDpiFontScaling);

            base.preMoveScaleControl(control);
        }

        private void previewTable_AddRowToTable(string[] row)
        {
            if (backgroundTaskIsWorking())
                previewTable.Rows.Add(row);

            previewTableFormatRow(previewTable.RowCount - 1);
        }

        private void previewTable_ProcessRowOfTable(int rowIndex)
        {
            previewTable.Rows[rowIndex].Cells[0].Value = null;
            previewTableFormatRow(rowIndex);
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
                sourceTagIds = new MetaDataType[templateTable.Rows.Count];
                sourcePropIds = new FilePropertyType[templateTable.Rows.Count];
                sourceTagNames = new string[templateTable.Rows.Count];

                for (int i = 0; i < templateTable.Rows.Count; i++)
                {
                    sourceTagIds[i] = GetTagId(string.Empty + templateTable.Rows[i].Cells[0].Value);
                    sourcePropIds[i] = GetPropId(string.Empty + templateTable.Rows[i].Cells[0].Value);
                    sourceTagNames[i] = string.Empty + templateTable.Rows[i].Cells[0].Value;
                }
            }
            else
            {
                sourceTagId = GetTagId(sourceTagList.Text);
                sourcePropId = GetPropId(sourceTagList.Text);
                destinationTagId = GetTagId(destinationTagList.Text);
            }


            files = null;
            if (searchOnly)
            {
                if (!MbApiInterface.Library_QueryFilesEx("domain=DisplayedFiles", out files))
                    files = new string[0];
            }
            else
            {
                {
                    if (!MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files))
                        files = new string[0];
                }
            }


            if (files.Length == 0 && searchOnly)
            {
                MessageBox.Show(this, MsgNoTracksDisplayed, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else if (files.Length == 0 && !searchOnly)
            {
                MessageBox.Show(this, MsgNoTracksSelected, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else
            {
                for (int i = 0; i < templateTable.Rows.Count; i++)
                {
                    string[] template = new string[5];

                    template[0] = string.Empty + templateTable.Rows[i].Cells[0].Value;
                    template[1] = string.Empty + templateTable.Rows[i].Cells[1].Value;
                    template[2] = string.Empty + templateTable.Rows[i].Cells[2].Value;
                    template[3] = string.Empty + templateTable.Rows[i].Cells[3].Value;
                    template[4] = string.Empty + templateTable.Rows[i].Cells[4].Value;

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

            bool stripNotChangedLines = SavedSettings.dontIncludeInPreviewLinesWithoutChangedTags;

            string sourceTagValue = null;
            string track;
            string[] row = { "Checked", "Track", "oldTag", "newTag", "File" };

            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                currentFile = files[fileCounter];

                SetStatusbarTextForFileOperations(MsrCommandSbText, true, fileCounter, files.Length, currentFile);

                track = GetTrackRepresentation(currentFile);
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
                            sourceTagValue = GetFileTag(currentFile, sourceTagIds[i]);
                        }
                        else
                        {
                            sourceTagValue = MbApiInterface.Library_GetFileProperty(currentFile, sourcePropIds[i]);
                        }
                        sourceTagName = sourceTagNames[i];

                        string newSourceTagValue;

                        try
                        {
                            string template0 = templates[i][0];
                            string template1 = templates[i][1];
                            string template2 = templates[i][2];
                            string template3 = templates[i][3];

                            if (template1 == "T" && template2 == "T") //Regex/case sensitive
                            {
                                newSourceTagValue = Replace(currentFile, sourceTagValue, template3, string.Empty, false, out _);
                            }
                            else if (template1 == "T" && template2 == "F") //Regex/case insensitive
                            {
                                newSourceTagValue = Replace(currentFile, sourceTagValue, template3, string.Empty, true, out _);
                            }
                            else if (template1 == "F" && template2 == "T") //Case sensitive
                            {
                                template3 = Regex.Escape(template3);
                                newSourceTagValue = Replace(currentFile, sourceTagValue, template3, string.Empty, false, out _);
                            }
                            else //Case insensitive
                            {
                                template3 = Regex.Escape(template3);
                                newSourceTagValue = Replace(currentFile, sourceTagValue, template3, string.Empty, true, out _);
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
                            row[3] = string.Empty;
                            row[4] = currentFile;

                            Invoke(addRowToTable, new object[] { row });
                            tags.Add(row);
                        }
                        else
                        {
                            row[0] = "T";
                            row[1] = track;
                            row[2] = string.Empty;
                            row[3] = string.Empty;
                            row[4] = currentFile;

                            Invoke(addRowToTable, new object[] { row });
                            tags.Add(row);

                            for (int j = 0; j < tagValues.Count; j++)
                            {
                                {
                                    row[0] = null;
                                    row[1] = "            " + tagNames[j];
                                    row[2] = tagValues[j];
                                    row[3] = string.Empty;
                                    row[4] = string.Empty;

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
                        sourceTagValue = GetFileTag(currentFile, sourceTagId);
                    }
                    else
                    {
                        sourceTagValue = MbApiInterface.Library_GetFileProperty(currentFile, sourcePropId);
                    }

                    destinationTagValue = GetFileTag(currentFile, destinationTagId);

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
                                newDestinationTagValue1 = Replace(currentFile, newDestinationTagValue2, template3, template4, false, out match);
                            }
                            else if (template1 == "T" && template2 == "F") //Regex/case insensitive
                            {
                                newDestinationTagValue1 = Replace(currentFile, newDestinationTagValue2, template3, template4, true, out match);
                            }
                            else if (template1 == "F" && template2 == "T") //Case sensitive
                            {
                                template3 = Regex.Escape(template[3]);
                                newDestinationTagValue1 = Replace(currentFile, newDestinationTagValue2, template3, template4, false, out match);
                            }
                            else //Case insensitive
                            {
                                template3 = Regex.Escape(template[3]);
                                newDestinationTagValue1 = Replace(currentFile, newDestinationTagValue2, template3, template4, true, out match);
                            }

                            newDestinationTagValue2 = newDestinationTagValue1;
                        }
                        catch
                        {
                            newDestinationTagValue1 = "SYNTAX ERROR!";
                            break;
                        }
                    }

                    if (destinationTagValue == newDestinationTagValue1 && stripNotChangedLines)
                        continue;


                    row[0] = (destinationTagValue == newDestinationTagValue1 ? "F" : "T");
                    row[1] = track;
                    row[2] = destinationTagValue;
                    row[3] = newDestinationTagValue1;
                    row[4] = currentFile;

                    Invoke(addRowToTable, new object[] { row });
                    tags.Add(row);
                }

                previewIsGenerated = true;
            }

            SetResultingSbText();
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
                        tags[i][0] = string.Empty;

                        Invoke(processRowOfTable, new object[] { i });

                        SetStatusbarTextForFileOperations(MsrCommandSbText, false, i, tags.Count, currentFile);

                        SetFileTag(currentFile, destinationTagId, newTag);
                        CommitTagsToFile(currentFile);
                    }
                }
            }

            RefreshPanels(true);

            SetResultingSbText();
        }

        private void saveSettings()
        {
            SavedSettings.copySourceTagName = sourceTagList.Text;
            SavedSettings.copyDestinationTagName = destinationTagList.Text;

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
                enableQueryingOrUpdatingButtons();
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

                previewTableFormatRow(e.RowIndex);
            }
        }

        public override void enableDisablePreviewOptionControls(bool enable, bool dontChangeDisabled = false)
        {
            bool initiallyEnabled = enable;
            enable = (enable && previewTable.Rows.Count == 0) || searchOnlyCheckBox.Checked;

            searchOnlyCheckBox.Enable(initiallyEnabled && !backgroundTaskIsWorking());

            templateNameTextBox.Enable(enable && !searchOnlyCheckBox.Checked);
            //ignoreTemplateNameTextBoxTextChanged = true;
            templateNameTextBox_TextChanged(null, null);
            //ignoreTemplateNameTextBoxTextChanged = false;

            sourceTagList.Enable(enable && !searchOnlyCheckBox.Checked);
            destinationTagList.Enable(enable && !autoDestinationTagCheckBox.Checked && !searchOnlyCheckBox.Checked);
            fromTagLabel.Enable(enable && !searchOnlyCheckBox.Checked);
            toTagLabel.Enable(enable && !searchOnlyCheckBox.Checked);
            presetLabel.Enable(enable && !searchOnlyCheckBox.Checked);
            buttonSave.Enable(enable && !searchOnlyCheckBox.Checked);
            loadComboBox.Enable(enable && !searchOnlyCheckBox.Checked);
            buttonAdd.Enable(enable);
            templateTable.Enable(enable);
            autoDestinationTagCheckBox.Enable(enable && !searchOnlyCheckBox.Checked);

            ReplaceWith.Visible = !searchOnlyCheckBox.Checked;
            NewTag.Visible = !searchOnlyCheckBox.Checked;
            if (searchOnlyCheckBox.Checked)
            {
                OriginalTag.HeaderText = OriginalTagHeaderTextTagValue;
                SearchTag.Visible = true;
            }
            else
            {
                OriginalTag.HeaderText = OriginalTagHeaderText;
                SearchTag.Visible = false;
            }

            if (templateTable.Rows.Count == 1)
            {
                buttonUp.Enable(false);
                buttonDown.Enable(false);
                buttonDelete.Enable(false);
            }
            else
            {
                buttonUp.Enable(enable && !searchOnlyCheckBox.Checked);
                buttonDown.Enable(enable && !searchOnlyCheckBox.Checked);
                buttonDelete.Enable(enable);
            }

            if (searchOnlyCheckBox.Checked)
            {
                buttonOK.Enable(initiallyEnabled && (previewTable.Rows.Count > 0));
                buttonOK.Text = SelectFoundButtonName;
                if (!backgroundTaskIsWorking())
                {
                    buttonPreview.Text = FindButtonName;
                }
            }
            else if (initiallyEnabled)
            {
                buttonOK.Text = OKButtonName;
                if (previewTable.Rows.Count == 0)
                {
                    buttonPreview.Text = PreviewButtonName;
                }
                else
                {
                    buttonPreview.Text = ClearButtonName;
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
            buttonOK.Enable(previewIsGenerated || SavedSettings.allowCommandExecutionWithoutPreview);
            buttonPreview.Enable(true);
        }

        public override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enable(false);
            buttonPreview.Enable(false);
        }

        private void previewTableFormatRow(int rowIndex)
        {
            if (SavedSettings.dontHighlightChangedTags)
                return;

            if ((string)previewTable.Rows[rowIndex].Cells[0].Value != "T")
            {
                for (int columnIndex = 1; columnIndex < previewTable.ColumnCount; columnIndex++)
                {
                    if (previewTable.Columns[columnIndex].Visible)
                        previewTable.Rows[rowIndex].Cells[columnIndex].Style = DimmedCellStyle;
                }

                return;
            }


            for (int columnIndex = 1; columnIndex < previewTable.ColumnCount; columnIndex++)
            {
                if (columnIndex == 3)
                {
                    if ((string)previewTable.Rows[rowIndex].Cells[3].Value == (string)previewTable.Rows[rowIndex].Cells[2].Value)
                        previewTable.Rows[rowIndex].Cells[3].Style = UnchangedCellStyle;
                    else
                        previewTable.Rows[rowIndex].Cells[3].Style = ChangedCellStyle;
                }
                else
                {
                    previewTable.Rows[rowIndex].Cells[columnIndex].Style = UnchangedCellStyle;
                }
            }
        }

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
            if (MSR == null)
            {
                MessageBox.Show(this, MsgYouMustImportStandardAsrPresetsFirst, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (templateNameTextBox.Text == CtlMSR)
            {
                MessageBox.Show(this, MsgGiveNameToAsrPreset, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                templateNameTextBox.Focus();
                templateNameTextBox.SelectionStart = CtlMSR.Length;
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
                    customMSR = new Preset(MSR, false, false, null, null);
                }
            }

            if (!presetExists)
            {
                if (MessageBox.Show(this, MsgAreYouSureYouWantToSaveAsrPreset
                    .Replace("%%PRESETNAME%%", templateNameTextBox.Text),
                        MsgSavePreset, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }
            else if (templateNameTextBox.Text == customMSR.getName())
            {
                if (MessageBox.Show(this, MsgAreYouSureYouWantToOverwriteAsrPreset
                    .Replace("%%PRESETNAME%%", templateNameTextBox.Text),
                        MsgSavePreset, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }
            else
            {
                if (MessageBox.Show(this, MsgAreYouSureYouWantToOverwriteRenameAsrPreset
                    .Replace("%%PRESETNAME%%", customMSR.getName())
                    .Replace("%%NEWPRESETNAME%%", templateNameTextBox.Text),
                        MsgSavePreset, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }

            string query = string.Empty;
            for (int i = 0; i < templateTable.Rows.Count; i++)
            {
                query += EncodeSearchReplaceTemplate(string.Empty + templateTable.Rows[i].Cells[3].Value, string.Empty + templateTable.Rows[i].Cells[4].Value, (string)templateTable.Rows[i].Cells[1].Value == "T" ? true : false, (string)templateTable.Rows[i].Cells[2].Value == "T" ? true : false) + "|";
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

            sourceTagIdInt = (int)GetTagId(sourceTagList.Text);
            sourcePropIdInt = (int)GetPropId(sourceTagList.Text);
            destinationTagId = GetTagId(destinationTagList.Text);
            if (sourcePropIdInt != 0)
                sourceTagIdInt = sourcePropIdInt;

            customMSR.customText = query;
            customMSR.parameterTagId = sourceTagIdInt;
            customMSR.parameterTag2Id = (int)destinationTagId;
            customMSR.isMSRPreset = true;
            customMSR.userPreset = true;

            if (File.Exists(Path.Combine(PresetsPath, oldSafeFileName + ASRPresetExtension)))
                File.Delete(Path.Combine(PresetsPath, oldSafeFileName + ASRPresetExtension));

            customMSR.savePreset(Path.Combine(PresetsPath, customMSR.getSafeFileName() + ASRPresetExtension));

            Presets.AddReplace(customMSR.guid, customMSR);

            if (autoApplyCheckBox.Checked && !SavedSettings.autoAppliedAsrPresetGuids.Contains(customMSR.guid))
            {
                AsrAutoAppliedPresets.Add(customMSR);
                SavedSettings.autoAppliedAsrPresetGuids.Add(customMSR.guid);
            }
            else if (!autoApplyCheckBox.Checked && SavedSettings.autoAppliedAsrPresetGuids.Contains(customMSR.guid))
            {
                AsrAutoAppliedPresets.Remove(customMSR);
                SavedSettings.autoAppliedAsrPresetGuids.Remove(customMSR.guid);
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
            loadComboBox.Enable(sourceTagList.Enabled);
            buttonDeleteSaved.Enable(true);

            TagToolsPlugin.SaveSettings();
        }

        private void templateNameTextBox_TextChanged(object sender, EventArgs e)
        {
            //if (!ignoreTemplateNameTextBoxTextChanged)
            //    loadComboBox.SelectedIndex = 0;

            if (templateNameTextBox.Text == string.Empty)
                buttonSave.Enable(false);
            else
                buttonSave.Enable(true);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            templateTable.Rows.Add();
            if (templateTable.Rows.Count == 1)
            {
                templateTable.Rows[templateTable.Rows.Count - 1].Cells[0].Value = SavedSettings.copySourceTagName;
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
                buttonDelete.Enable(true);
                buttonUp.Enable(!searchOnlyCheckBox.Checked);
                buttonDown.Enable(!searchOnlyCheckBox.Checked);
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
                    buttonUp.Enable(false);
                    buttonDown.Enable(false);
                    buttonDelete.Enable(false);
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
                templateNameTextBox.Text = CtlMSR;
                templateNameTextBox.SelectionStart = CtlMSR.Length;
                buttonDeleteSaved.Enable(false);
                autoApplyCheckBox.Checked = false;
                return;
            }
            else
            {
                customMSR = (Preset)loadComboBox.SelectedItem;
            }

            buttonDeleteSaved.Enable(true);

            if (SavedSettings.autoAppliedAsrPresetGuids.Contains(customMSR.guid))
                autoApplyCheckBox.Checked = true;
            else
                autoApplyCheckBox.Checked = false;

            //ignoreTemplateNameTextBoxTextChanged = true;
            templateNameTextBox.Text = customMSR.getName();
            //ignoreTemplateNameTextBoxTextChanged = false;

            templateTable.Rows.Clear();

            sourceTagList.SelectedItem = GetTagName((MetaDataType)customMSR.parameterTagId);
            destinationTagList.SelectedItem = GetTagName((MetaDataType)customMSR.parameterTag2Id);

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
                destinationTagList.Enable(false);
                int destinationTagListSelectedIndex = destinationTagList.SelectedIndex;
                destinationTagList.SelectedItem = sourceTagList.SelectedItem;

                if (destinationTagList.SelectedIndex == -1)
                {
                    destinationTagList.SelectedIndex = destinationTagListSelectedIndex;
                }
            }
            else
            {
                destinationTagList.Enable(!searchOnlyCheckBox.Checked);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            autoDestinationTagCheckBox.Checked = !autoDestinationTagCheckBox.Checked;
            autoDestinationTagCheckBox_CheckedChanged(null, null);
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
            if (MessageBox.Show(this, MsgAreYouSureYouWantToDeleteAsrPreset.Replace("%%PRESETNAME%%", templateNameTextBox.Text),
                    MsgDeletePreset, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;


            if (SavedSettings.autoAppliedAsrPresetGuids.Contains(customMSR.guid))
            {
                SavedSettings.autoAppliedAsrPresetGuids.Remove(customMSR.guid);
            }

            for (int j = 0; j < AsrPresetsWithHotkeys.Length; j++)
            {
                if (AsrPresetsWithHotkeys[j] == customMSR)
                {
                    AsrPresetsWithHotkeys[j] = null;
                    SavedSettings.asrPresetsWithHotkeysGuids[j] = Guid.Empty;
                    ASRPresetsWithHotkeysCount--;
                }
            }

            foreach (var pair in IdsAsrPresets)
            {
                if (pair.Value == customMSR)
                    IdsAsrPresets.Remove(pair.Key);
            }

            File.Delete(Path.Combine(PresetsPath, customMSR.getSafeFileName() + ASRPresetExtension));

            Presets.Remove(customMSR.guid);
            loadComboBox.Items.Remove(customMSR);

            loadComboBox.SelectedIndex = 0;
            buttonDeleteSaved.Enable(false);


            TagToolsPlugin.SaveSettings();
        }

        private void MultipleSearchAndReplaceCommand_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveWindowLayout(templateTable.Columns[0].Width, templateTable.Columns[3].Width, templateTable.Columns[4].Width,
                0,
                previewTable.Columns[0].Width, previewTable.Columns[1].Width, previewTable.Columns[2].Width);
        }

        private void MultipleSearchAndReplaceCommand_Load(object sender, EventArgs e)
        {
            (int, int, int, int, int, int, int) value = loadWindowLayout();

            if (value.Item1 != 0)
            {
                templateTable.Columns[0].Width = (int)Math.Round(value.Item1 * hDpiFontScaling);
                templateTable.Columns[3].Width = (int)Math.Round(value.Item2 * hDpiFontScaling);
                templateTable.Columns[4].Width = (int)Math.Round(value.Item3 * hDpiFontScaling);

                previewTable.Columns[0].Width = (int)Math.Round(value.Item5 * hDpiFontScaling);
                previewTable.Columns[1].Width = (int)Math.Round(value.Item6 * hDpiFontScaling);
                previewTable.Columns[2].Width = (int)Math.Round(value.Item7 * hDpiFontScaling);
            }
            else
            {
                templateTable.Columns[0].Width = (int)Math.Round(templateTable.Columns[0].Width * hDpiFontScaling);
                templateTable.Columns[3].Width = (int)Math.Round(templateTable.Columns[3].Width * hDpiFontScaling);
                templateTable.Columns[4].Width = (int)Math.Round(templateTable.Columns[4].Width * hDpiFontScaling);

                previewTable.Columns[0].Width = (int)Math.Round(previewTable.Columns[0].Width * hDpiFontScaling);
                previewTable.Columns[1].Width = (int)Math.Round(previewTable.Columns[1].Width * hDpiFontScaling);
                previewTable.Columns[2].Width = (int)Math.Round(previewTable.Columns[2].Width * hDpiFontScaling);
            }


            ignoreSplitterMovedEvent = true;

            //Let's scale split containers manually (auto-scaling is improper)
            foreach (var scsa in splitContainersScalingAttributes)
            {
                var sc = scsa.splitContainer;
                sc.Panel1MinSize = (int)Math.Round(scsa.panel1MinSize * vDpiFontScaling);
                sc.Panel2MinSize = (int)Math.Round(scsa.panel2MinSize * vDpiFontScaling);

                if (value.Item4 != 0)
                {
                    sc.SplitterDistance = (int)Math.Round(value.Item4 * vDpiFontScaling);
                }
                else
                {
                    sc.SplitterDistance = (int)Math.Round(scsa.splitterDistance * vDpiFontScaling);
                }
            }

            ignoreSplitterMovedEvent = false;


            placeholderLabel1.Visible = false;
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (!ignoreSplitterMovedEvent)
                saveWindowLayout(0, 0, 0, splitContainer1.SplitterDistance);
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            PluginQuickSettings settings = new PluginQuickSettings(TagToolsPlugin);
            Display(settings, true);
        }

        private void autoApplyPictureBox_Click(object sender, EventArgs e)
        {
            autoApplyCheckBox.Checked = !autoApplyCheckBox.Checked;
        }
    }
}
