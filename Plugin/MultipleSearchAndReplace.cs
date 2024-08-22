using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static MusicBeePlugin.AdvancedSearchAndReplace;
using static MusicBeePlugin.Plugin;


namespace MusicBeePlugin
{
    public partial class MultipleSearchAndReplace : PluginWindowTemplate
    {
        private CustomComboBox sourceTagListCustom;
        private CustomComboBox destinationTagListCustom;
        private CustomComboBox loadComboBoxCustom;


        private readonly DataGridViewCellStyle unchangedCellStyle = new DataGridViewCellStyle(UnchangedCellStyle);
        private readonly DataGridViewCellStyle changedCellStyle = new DataGridViewCellStyle(ChangedCellStyle);
        private readonly DataGridViewCellStyle dimmedCellStyle = new DataGridViewCellStyle(DimmedCellStyle);

        private MetaDataType sourceTagId;
        private FilePropertyType sourcePropId;
        private MetaDataType destinationTagId;

        private MetaDataType[] sourceTagIds;
        private FilePropertyType[] sourcePropIds;
        private string[] sourceTagNames;

        private bool smartOperation;

        private string[] files = Array.Empty<string>();
        private readonly List<string[]> tags = new List<string[]>();
        private readonly List<string[]> templates = new List<string[]>();
        private List<bool> processedRowList = new List<bool>(); //Indices of processed tracks

        private bool ignoreSplitterMovedEvent = true;
        //private bool ignoreTemplateNameTextBoxTextChanged = false;

        private Preset customMSR;

        internal MultipleSearchAndReplace(Plugin plugin) : base(plugin)
        {
            InitializeComponent();

            new ControlBorder(this.templateNameTextBox);
        }

        protected override void initializeForm()
        {
            base.initializeForm();

            sourceTagListCustom = namesComboBoxes["sourceTagList"];
            destinationTagListCustom = namesComboBoxes["destinationTagList"];
            loadComboBoxCustom = namesComboBoxes["loadComboBox"];


            //Setting themed images
            autoApplyPictureBox.Image = ReplaceBitmap(autoApplyPictureBox.Image, AutoAppliedPresetsAccent);

            buttonLabels[buttonDeleteSaved] = string.Empty;
            buttonDeleteSaved.Text = string.Empty;
            buttonDeleteSaved.Image = ReplaceBitmap(buttonDeleteSaved.Image, ClearField);

            buttonSettings.Image = ReplaceBitmap(buttonSettings.Image, Gear);


            FillListByTagNames(destinationTagListCustom.Items, false, false, false);
            destinationTagListCustom.Text = SavedSettings.copyDestinationTagName;

            FillListByTagNames(sourceTagListCustom.Items, true, false, false);
            sourceTagListCustom.Text = SavedSettings.copySourceTagName;

            smartOperationCheckBox.Checked = SavedSettings.smartOperation;


            var headerCellStyle = new DataGridViewCellStyle(HeaderCellStyle);

            templateTable.Columns[0].HeaderCell.Style = headerCellStyle;
            templateTable.Columns[1].HeaderCell.Style = headerCellStyle;
            templateTable.Columns[2].HeaderCell.Style = headerCellStyle;
            templateTable.Columns[3].HeaderCell.Style = headerCellStyle;

            var cbHeader = new DataGridViewCheckBoxHeaderCell();
            cbHeader.Style = headerCellStyle;
            cbHeader.setState(true);
            cbHeader.OnCheckBoxClicked += cbHeader_OnCheckBoxClicked;

            var colCB = new DataGridViewCheckBoxColumn
            {
                HeaderCell = cbHeader,
                ThreeState = true,
                FalseValue = "F",
                TrueValue = "T",
                IndeterminateValue = string.Empty,
                Width = 25,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };

            previewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            previewTable.Columns.Insert(0, colCB);
            previewTable.Columns[1].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[2].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[3].HeaderCell.Style = headerCellStyle;


            customMSR = null;

            loadComboBoxCustom.Sorted = false;
            loadComboBoxCustom.Items.Add(CtlNewAsrPreset);
            foreach (var preset in Presets)
            {
                if (preset.Value.isMSRPreset)
                {
                    loadComboBoxCustom.Items.Add(preset.Value);
                }
            }
            loadComboBoxCustom.SelectedIndex = 0;

            buttonAdd_Click(null, null);

            templateNameTextBox.Text = CtlMSR;
            templateNameTextBox.SelectionStart = CtlMSR.Length;


            enableDisablePreviewOptionControls(true, true);
            enableQueryingOrUpdatingButtons();

            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        private void moveScaleButtonAdd()
        {
            buttonAdd.Left = sourceTagList.Left;
        }

        private void moveScaleButtonDelete()
        {
            buttonDelete.Left = sourceTagList.Left + sourceTagList.Width - buttonDelete.Width;
        }

        private void moveScaleAutoApplyCheckBox()
        {
            autoApplyCheckBox.Left = buttonSave.Left - autoApplyCheckBox.Width - autoApplyCheckBox.Margin.Right - buttonSave.Margin.Left;
        }

        private void moveScaleTemplateNameTextBox()
        {
            templateNameTextBox.Left = destinationTagList.Left;
            templateNameTextBox.Width = autoApplyCheckBox.Left - destinationTagList.Left - templateNameTextBox.Margin.Right - autoApplyCheckBox.Margin.Left;
        }

        private void moveScalePresetLabel()
        {
            presetLabel.Left = templateNameTextBox.Left - presetLabel.Width - presetLabel.Margin.Right - templateNameTextBox.Margin.Left;
        }

        private void moveScaleAutoApplyPictureBox()
        {
            autoApplyPictureBox.Left = buttonSave.Left - autoApplyPictureBox.Width - autoApplyPictureBox.Margin.Right - buttonSave.Margin.Left;
            autoApplyPictureBox.Top = buttonSave.Top + (int)Math.Round((buttonSave.Height - autoApplyPictureBox.Height) / 1.5f);
        }

        private void moveScaleButtonSave()
        {
            buttonSave.Left = buttonPreview.Left;
        }

        private void moveScaleLoadComboBoxCustom()
        {
            loadComboBoxCustom.Left = buttonOK.Left;
            loadComboBoxCustom.Width = buttonDeleteSaved.Left - loadComboBoxCustom.Left - loadComboBoxCustom.Margin.Right - buttonDeleteSaved.Margin.Left;
        }

        private void previewTable_ProcessRowsOfTable(List<bool> processedRowList)
        {
            for (int i = 0; i < processedRowList.Count; i++)
            {
                previewTable.CurrentCell = previewTable.Rows[i].Cells[0];

                if (processedRowList[i])
                    previewTable.Rows[i].Cells[0].Value = null;

                previewTableFormatRow(previewTable, i);
            }
        }

        private void resetPreviewData()
        {
            if (previewIsGenerated)//------------- check!!!
            {
                previewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                previewTable.AllowUserToResizeColumns = true;
                previewTable.AllowUserToResizeRows = true;
                foreach (DataGridViewColumn column in previewTable.Columns)
                    column.SortMode = DataGridViewColumnSortMode.Automatic;
            }
            else
            {
                previewTable.AllowUserToResizeColumns = false;
                previewTable.AllowUserToResizeRows = false;
                foreach (DataGridViewColumn column in previewTable.Columns)
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }


            backgroundTaskIsStopping = false;
            backgroundTaskIsStoppedOrCancelled = false;

            tags.Clear();
            templates.Clear();
            previewTable.RowCount = 0;
            (previewTable.Columns[0].HeaderCell as DataGridViewCheckBoxHeaderCell).setState(true);

            updateCustomScrollBars(previewTable);
            SetStatusBarText(string.Empty, false);

            enableQueryingOrUpdatingButtons();
            enableDisablePreviewOptionControls(true);

            if (closeFormOnStopping && ignoreClosingForm && backgroundTaskIsScheduled)
            {
                ignoreClosingForm = false;
                Close();
            }

            if (backgroundTaskIsScheduled)
                ignoreClosingForm = false;
        }

        private void resetFormToGeneratedPreview()
        {
            previewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            previewTable.AllowUserToResizeColumns = true;
            previewTable.AllowUserToResizeRows = true;
            foreach (DataGridViewColumn column in previewTable.Columns)
                column.SortMode = DataGridViewColumnSortMode.Automatic;



            backgroundTaskIsScheduled = false;
            backgroundTaskIsStopping = false;
            backgroundTaskIsStoppedOrCancelled = false;

            enableDisablePreviewOptionControls(true);
            enableQueryingOrUpdatingButtons();

            updateCustomScrollBars(previewTable);
            SetResultingSbText();

            if (closeFormOnStopping)
            {
                ignoreClosingForm = false;
                Close();
            }

            ignoreClosingForm = false;
        }

        private bool applyingChangesStopped()
        {
            previewTable.AllowUserToResizeColumns = true;
            previewTable.AllowUserToResizeRows = true;
            foreach (DataGridViewColumn column in previewTable.Columns)
                column.SortMode = DataGridViewColumnSortMode.Automatic;



            backgroundTaskIsScheduled = false;
            backgroundTaskIsStopping = false;
            backgroundTaskIsStoppedOrCancelled = false;
            closeFormOnStopping = false;

            previewTable_ProcessRowsOfTable(processedRowList);

            SetResultingSbText();

            if (closeFormOnStopping)
            {
                ignoreClosingForm = false;
                Close();
            }

            ignoreClosingForm = false;

            return true;
        }

        private bool prepareBackgroundPreview()
        {
            resetPreviewData();

            if (backgroundTaskIsStopping)
                backgroundTaskIsStoppedOrCancelled = true;

            if (previewIsGenerated && !backgroundTaskIsWorking())
            {
                previewIsGenerated = false;
                enableDisablePreviewOptionControls(true);
                return true;
            }

            previewIsGenerated = false;

            if (backgroundTaskIsWorking())
                return true;


            previewTable.AllowUserToResizeColumns = false;
            previewTable.AllowUserToResizeRows = false;
            foreach (DataGridViewColumn column in previewTable.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;


            sourceTagId = 0;
            sourcePropId = 0;
            destinationTagId = 0;

            sourceTagIds = null;
            sourcePropIds = null;
            sourceTagNames = null;

            sourceTagId = GetTagId(sourceTagListCustom.Text);
            sourcePropId = GetPropId(sourceTagListCustom.Text);
            destinationTagId = GetTagId(destinationTagListCustom.Text);

            smartOperation = smartOperationCheckBox.Checked;


            MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files);


            if (files == null || files.Length == 0)
            {
                MessageBox.Show(this, MsgNoTracksSelected, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else
            {
                for (var i = 0; i < templateTable.Rows.Count; i++)
                {
                    var template = new string[4];

                    template[0] = string.Empty + templateTable.Rows[i].Cells[0].Value;
                    template[1] = string.Empty + templateTable.Rows[i].Cells[1].Value;
                    template[2] = string.Empty + templateTable.Rows[i].Cells[2].Value;
                    template[3] = string.Empty + templateTable.Rows[i].Cells[3].Value;

                    templates.Add(template);
                }

                previewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                return true;
            }
        }

        private bool prepareBackgroundTask()
        {
            if (backgroundTaskIsWorking())
                return false;

            if (previewTable.Rows.Count == 0)
            {
                return prepareBackgroundPreview();
            }
            else
            {
                previewTable.AllowUserToResizeColumns = false;
                previewTable.AllowUserToResizeRows = false;
                foreach (DataGridViewColumn column in previewTable.Columns)
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;


                tags.Clear();

                for (var fileCounter = 0; fileCounter < previewTable.Rows.Count; fileCounter++)
                {
                    var tag = new string[5];

                    tag[0] = previewTable.Rows[fileCounter].Cells[0].Value as string;
                    tag[3] = previewTable.Rows[fileCounter].Cells[3].Value as string;
                    tag[4] = previewTable.Rows[fileCounter].Cells[4].Value as string;

                    tags.Add(tag);
                }

                ignoreClosingForm = true;

                return true;
            }
        }

        private void previewChanges()
        {
            previewIsGenerated = true;

            string sourceTagValue = null;
            var stripNotChangedLines = SavedSettings.dontIncludeInPreviewLinesWithoutChangedTags;

            for (var fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (checkStoppingStatus())
                {
                    Invoke(new Action(() => { stopButtonClickedMethod(prepareBackgroundPreview); }));
                    return;
                }


                string[] row = { "Checked", "Track", "oldTag", "newTag", "File" };
                var currentFile = files[fileCounter];

                SetStatusBarTextForFileOperations(MsrSbText, true, fileCounter, files.Length, currentFile);

                var track = GetTrackRepresentation(currentFile);
                row = new string[5];

                if (sourcePropId == 0)
                    sourceTagValue = GetFileTag(currentFile, sourceTagId);
                else
                    sourceTagValue = MbApiInterface.Library_GetFileProperty(currentFile, sourcePropId);

                var destinationTagValue = GetFileTag(currentFile, destinationTagId);
                var swappedTags = SwapTags(sourceTagValue, destinationTagValue, sourceTagId, destinationTagId, smartOperation);


                string newDestinationTagValue1 = null;
                string newDestinationTagValue2 = swappedTags.newSourceNormalizedTagValue;

                foreach (var template in templates)
                {
                    try
                    {
                        var template1 = template[0];
                        var template2 = template[1];
                        var template3 = template[2];
                        var template4 = template[3];

                        if (template1 == "T" && template2 == "T") //Regex/case-sensitive
                        {
                            newDestinationTagValue1 = Replace(currentFile, newDestinationTagValue2, template3, template4, false, out _);
                        }
                        else if (template1 == "T" && template2 == "F") //Regex/case-insensitive
                        {
                            newDestinationTagValue1 = Replace(currentFile, newDestinationTagValue2, template3, template4, true, out _);
                        }
                        else if (template1 == "F" && template2 == "T") //Case-sensitive
                        {
                            template3 = Regex.Escape(template3);
                            newDestinationTagValue1 = Replace(currentFile, newDestinationTagValue2, template3, template4, false, out _);
                        }
                        else //Case-insensitive
                        {
                            template3 = Regex.Escape(template3);
                            newDestinationTagValue1 = Replace(currentFile, newDestinationTagValue2, template3, template4, true, out _);
                        }

                        newDestinationTagValue2 = newDestinationTagValue1;
                    }
                    catch
                    {
                        newDestinationTagValue1 = CtlAsrSyntaxError;
                        break;
                    }
                }

                string isChecked;
                if (swappedTags.destinationNormalizedTagValue == newDestinationTagValue1 && stripNotChangedLines)
                    continue;
                else if (swappedTags.destinationNormalizedTagValue == newDestinationTagValue1)
                    isChecked = "F";
                else
                    isChecked = "T";


                row[0] = isChecked;
                row[1] = track;
                row[2] = swappedTags.destinationNormalizedTagValue;
                row[3] = newDestinationTagValue1;
                row[4] = currentFile;

                tags.Add(row);

                int rowCountToFormat1 = 0;
                Invoke(new Action(() => { rowCountToFormat1 = AddRowsToTable(this, previewTable, tags, false, true); }));
                Invoke(new Action(() => { FormatChangedTags(this, previewTable, rowCountToFormat1, previewTableFormatRow); }));
            }

            int rowCountToFormat3 = 0;
            Invoke(new Action(() => { rowCountToFormat3 = AddRowsToTable(this, previewTable, tags, true, true); }));
            Invoke(new Action(() => { FormatChangedTags(this, previewTable, rowCountToFormat3, previewTableFormatRow); checkStoppedStatus(); resetFormToGeneratedPreview(); }));
        }

        private void applyChanges()
        {
            if (tags.Count == 0)
                throw new Exception("Something went wrong! Empty 'tags' local variable (must be filled on generating preview.)");

            processedRowList.Clear(); //Indices of processed tracks
            Invoke(new Action(() => { previewTable.CurrentCell = previewTable.Rows[0].Cells[0]; }));
            Invoke(new Action(() => { previewTable.FirstDisplayedCell = previewTable.CurrentCell; }));

            for (var i = 0; i < tags.Count; i++)
            {
                if (checkStoppingStatus())
                {
                    Invoke(new Action(() => { stopButtonClickedMethod(applyingChangesStopped); }));
                    return;
                }


                var isChecked = tags[i][0];

                if (isChecked == "T")
                {
                    var currentFile = tags[i][4];
                    var newTag = tags[i][3];

                    if (newTag != CtlAsrSyntaxError)
                    {
                        tags[i][0] = string.Empty;

                        processedRowList.Add(true);
                        SetStatusBarTextForFileOperations(MsrSbText, false, i, tags.Count, currentFile);

                        SetFileTag(currentFile, destinationTagId, newTag);
                        CommitTagsToFile(currentFile);
                    }
                }
                else
                {
                    processedRowList.Add(false);
                }
            }

            Invoke(new Action(() => { applyingChangesStopped(); }));

            RefreshPanels(true);
            SetResultingSbText();
        }

        private void saveSettings()
        {
            SavedSettings.copySourceTagName = sourceTagListCustom.Text;
            SavedSettings.copyDestinationTagName = destinationTagListCustom.Text;
            SavedSettings.smartOperation = smartOperationCheckBox.Checked;

            TagToolsPlugin.SaveSettings();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            saveSettings();
            if (prepareBackgroundTask())
                switchOperation(applyChanges, sender as Button, buttonOK, buttonPreview, buttonClose, true, null);
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            ignoreClosingForm = clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewChanges, buttonPreview, buttonOK, buttonClose);
        }

        private void buttonClose_Click(object sender, EventArgs e)
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
                var isChecked = previewTable.Rows[e.RowIndex].Cells[0].Value as string;

                if (isChecked == "T")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "F";
                }
                else if (isChecked == "F")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "T";
                }

                previewTableFormatRow(previewTable, e.RowIndex);
            }
        }

        internal override void enableDisablePreviewOptionControls(bool enable, bool dontChangeDisabled = false)
        {
            var initiallyEnabled = enable;
            enable = (enable && previewTable.Rows.Count == 0);

            templateNameTextBox.Enable(enable);
            //ignoreTemplateNameTextBoxTextChanged = true;
            templateNameTextBox_TextChanged(null, null);
            //ignoreTemplateNameTextBoxTextChanged = false;

            sourceTagListCustom.Enable(enable);
            destinationTagListCustom.Enable(enable && !autoDestinationTagCheckBox.Checked);
            fromTagLabel.Enable(enable);
            toTagLabel.Enable(enable);
            presetLabel.Enable(enable);
            buttonSave.Enable(enable);
            loadComboBoxCustom.Enable(enable);
            buttonAdd.Enable(enable);
            templateTable.Enable(enable);
            autoDestinationTagCheckBox.Enable(enable);
            smartOperationCheckBox.Enable(enable);

            if (templateTable.Rows.Count == 1)
            {
                buttonUp.Enable(false);
                buttonDown.Enable(false);
                buttonDelete.Enable(false);
            }
            else
            {
                buttonUp.Enable(enable);
                buttonDown.Enable(enable);
                buttonDelete.Enable(enable);
            }

            if (initiallyEnabled)
            {
                buttonOK.Text = ButtonOKName;

                if (previewTable.Rows.Count == 0)
                    buttonPreview.Text = ButtonPreviewName;
                else
                    buttonPreview.Text = ButtonClearName;
            }
        }

        internal override void enableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonPreview, " ");
            dirtyErrorProvider.SetError(buttonPreview, string.Empty);
        }

        internal override void disableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonPreview, getBackgroundTasksWarning());
        }

        internal override void enableQueryingOrUpdatingButtons()
        {
            buttonOK.Enable((previewIsGenerated || SavedSettings.allowCommandExecutionWithoutPreview) && !backgroundTaskIsStopping && (!backgroundTaskIsWorking() || backgroundTaskIsUpdatingTags));
            buttonPreview.Enable(true);
        }

        internal override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enable(false);
            buttonPreview.Enable(false);
        }

        private void previewTableFormatRow(DataGridView dataGridView, int rowIndex)
        {
            if (SavedSettings.dontHighlightChangedTags)
                return;

            if ((string)dataGridView.Rows[rowIndex].Cells[0].Value != "T")
            {
                for (var columnIndex = 1; columnIndex < dataGridView.ColumnCount; columnIndex++)
                {
                    if (dataGridView.Columns[columnIndex].Visible)
                        dataGridView.Rows[rowIndex].Cells[columnIndex].Style = dimmedCellStyle;
                }

                return;
            }


            for (var columnIndex = 1; columnIndex < dataGridView.ColumnCount; columnIndex++)
            {
                if (columnIndex == 3)
                {
                    if ((string)dataGridView.Rows[rowIndex].Cells[3].Value == (string)dataGridView.Rows[rowIndex].Cells[2].Value)
                        dataGridView.Rows[rowIndex].Cells[3].Style = unchangedCellStyle;
                    else
                        dataGridView.Rows[rowIndex].Cells[3].Style = changedCellStyle;
                }
                else
                {
                    dataGridView.Rows[rowIndex].Cells[columnIndex].Style = unchangedCellStyle;
                }
            }
        }

        internal static string EncodeSearchReplaceTemplate(string searchTemplate, string replaceTemplate, bool useRegexes, bool caseSensitive)
        {
            searchTemplate = searchTemplate.Replace(@"\", @"\\").Replace(@"#", @"\#").Replace("/", @"\L").Replace("|", @"\V").Replace(@"*", @"\X");
            replaceTemplate = replaceTemplate.Replace(@"\", @"\\").Replace(@"#", @"\#").Replace("/", @"\L").Replace("|", @"\V").Replace(@"*", @"\X").Replace(@"$", @"\T");

            string query;

            if (caseSensitive && useRegexes)
            {
                query = "#*" + searchTemplate + "/" + replaceTemplate;
            }
            else if (!caseSensitive && useRegexes)
            {
                query = "*" + searchTemplate + "/" + replaceTemplate;
            }
            else if (caseSensitive && !useRegexes)
            {
                query = "#" + searchTemplate + "/" + replaceTemplate;
            }
            else //if (!caseSensitive && !useRegexes)
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

            if (string.IsNullOrEmpty(templateNameTextBox.Text) || templateNameTextBox.Text == CtlMSR)
            {
                MessageBox.Show(this, MsgMsrGiveNameToAsrPreset, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                templateNameTextBox.Focus();
                templateNameTextBox.SelectionStart = CtlMSR.Length;
                return;
            }


            var presetExists = true;

            if (loadComboBoxCustom.SelectedIndex == 0)
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
                    customMSR = new Preset(MSR, false, false);
                }
            }

            if (!presetExists)
            {
                if (MessageBox.Show(this, MsgMsrAreYouSureYouWantToSaveAsrPreset
                    .Replace("%%PRESET-NAME%%", templateNameTextBox.Text),
                        MsgMsrSavePreset, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }
            else if (templateNameTextBox.Text == customMSR.getName())
            {
                if (MessageBox.Show(this, MsgMsrAreYouSureYouWantToOverwriteAsrPreset
                    .Replace("%%PRESET-NAME%%", templateNameTextBox.Text),
                        MsgMsrSavePreset, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }
            else
            {
                if (MessageBox.Show(this, MsgMsrAreYouSureYouWantToOverwriteRenameAsrPreset
                    .Replace("%%PRESET-NAME%%", customMSR.getName())
                    .Replace("%%NEWPRESETNAME%%", templateNameTextBox.Text),
                        MsgMsrSavePreset, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }

            var query = string.Empty;
            for (var i = 0; i < templateTable.Rows.Count; i++)
            {
                query += EncodeSearchReplaceTemplate(string.Empty + templateTable.Rows[i].Cells[2].Value, string.Empty + templateTable.Rows[i].Cells[3].Value, 
                    (string)templateTable.Rows[i].Cells[0].Value == "T", (string)templateTable.Rows[i].Cells[1].Value == "T") + "|";
            }

            query = query.Remove(query.Length - 1);


            var oldSafeFileName = customMSR.getSafeFileName();


            var names = new List<string>();
            foreach (var name in customMSR.names)
            {
                names.Add(name.Key);
            }

            customMSR.names.Clear();

            foreach (var name in names)
            {
                customMSR.names.Add(name, templateNameTextBox.Text);
            }

            var sourceTagIdInt = (int)GetTagId(sourceTagListCustom.Text);
            var sourcePropIdInt = (int)GetPropId(sourceTagListCustom.Text);
            destinationTagId = GetTagId(destinationTagListCustom.Text);
            if (sourcePropIdInt != 0)
                sourceTagIdInt = sourcePropIdInt;

            customMSR.customText = query;
            customMSR.parameterTagId = sourceTagIdInt;
            customMSR.parameterTag2Id = (int)destinationTagId;
            customMSR.isMSRPreset = true;
            customMSR.userPreset = true;

            if (File.Exists(Path.Combine(PresetsPath, oldSafeFileName + AsrPresetExtension)))
                File.Delete(Path.Combine(PresetsPath, oldSafeFileName + AsrPresetExtension));

            customMSR.savePreset(Path.Combine(PresetsPath, customMSR.getSafeFileName() + AsrPresetExtension));

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

            if (loadComboBoxCustom.SelectedIndex > 0)
            {
                var index = loadComboBoxCustom.SelectedIndex;
                loadComboBoxCustom.Items.RemoveAt(index);
                loadComboBoxCustom.Items.Insert(index, customMSR);
            }
            else
            {
                loadComboBoxCustom.Items.Add(customMSR);
            }

            loadComboBoxCustom.SelectedItem = customMSR;
            loadComboBoxCustom.Enable(sourceTagListCustom.IsEnabled());
            buttonDeleteSaved.Enable(true);

            TagToolsPlugin.SaveSettings();
        }

        private void templateNameTextBox_TextChanged(object sender, EventArgs e)
        {
            //if (!ignoreTemplateNameTextBoxTextChanged)
            //   loadComboBoxCustom.SelectedIndex = 0;

            if (string.IsNullOrEmpty(templateNameTextBox.Text))
                buttonSave.Enable(false);
            else
                buttonSave.Enable(true);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            templateTable.Rows.Add();
            templateTable.Rows[templateTable.Rows.Count - 1].Cells[4].Value = templateTable.Rows.Count.ToString("D8");
            templateTable.Rows[templateTable.Rows.Count - 1].Cells[0].Value = "F";
            templateTable.Rows[templateTable.Rows.Count - 1].Cells[1].Value = "F";
            templateTable.Rows[templateTable.Rows.Count - 1].Selected = true;


            if (templateTable.Rows.Count > 1)
            {
                buttonDelete.Enable(true);
                buttonUp.Enable(true);
                buttonDown.Enable(true);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            var index = templateTable.CurrentRow?.Index ?? -1;
            if (index >= 0)
            {
                if (index == templateTable.Rows.Count - 1)
                    templateTable.Rows[index].Selected = false;

                templateTable.Rows.RemoveAt(index);

                for (var i = 0; i < templateTable.Rows.Count; i++)
                {
                    templateTable.Rows[i].Cells[4].Value = i.ToString("D8");
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
            var index = templateTable.CurrentRow?.Index ?? -1;

            if (index > 0)
            {
                templateTable.Rows[index].Cells[4].Value = (index - 1).ToString("D8");
                templateTable.Rows[index - 1].Cells[4].Value = index.ToString("D8");
                templateTable.Sort(templateTable.Columns[4], System.ComponentModel.ListSortDirection.Ascending);
            }
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            var index = templateTable.CurrentRow?.Index ?? -1;

            if (index < templateTable.Rows.Count - 1)
            {
                templateTable.Rows[index].Cells[4].Value = (index + 1).ToString("D8");
                templateTable.Rows[index + 1].Cells[4].Value = index.ToString("D8");
                templateTable.Sort(templateTable.Columns[4], System.ComponentModel.ListSortDirection.Ascending);
            }
        }

        private void loadComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loadComboBoxCustom.SelectedIndex == 0)
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
                customMSR = (Preset)loadComboBoxCustom.SelectedItem;
            }

            buttonDeleteSaved.Enable(true);
            autoApplyCheckBox.Checked = SavedSettings.autoAppliedAsrPresetGuids.Contains(customMSR.guid);

            //ignoreTemplateNameTextBoxTextChanged = true;
            templateNameTextBox.Text = customMSR.getName();
            //ignoreTemplateNameTextBoxTextChanged = false;

            templateTable.RowCount = 0;

            sourceTagListCustom.SelectedItem = GetTagName((MetaDataType)customMSR.parameterTagId);
            destinationTagListCustom.SelectedItem = GetTagName((MetaDataType)customMSR.parameterTag2Id);

            var queries = customMSR.customText.Split('|');

            foreach (var query in queries)
            {
                buttonAdd_Click(null, null);

                string[] pair;
                if (query.Length > 1 && query[0] == '#' && query[1] == '*') //Case-sensitive/regexes
                {
                    pair = query.Substring(2).Split('/');

                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[0].Value = "T";
                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[1].Value = "T";
                }
                else if (query.Length > 0 && query[0] == '*') //Case-insensitive/regexes
                {
                    pair = query.Substring(1).Split('/');

                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[0].Value = "T";
                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[1].Value = "F";
                }
                else if (query.Length > 0 && query[0] == '#') //Case-sensitive
                {
                    pair = query.Substring(1).Split('/');

                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[0].Value = "F";
                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[1].Value = "T";
                }
                else //Case-insensitive
                {
                    pair = query.Split('/');

                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[0].Value = "F";
                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[1].Value = "F";
                }

                pair[0] = pair[0].Replace(@"\#", "#").Replace(@"\L", "/").Replace(@"\V", "|").Replace(@"\X", "*").Replace(@"\\", @"\");
                pair[1] = pair[1].Replace(@"\#", "#").Replace(@"\L", "/").Replace(@"\V", "|").Replace(@"\X", "*").Replace(@"\\", @"\").Replace(@"\T", @"$");

                templateTable.Rows[templateTable.Rows.Count - 1].Cells[2].Value = pair[0];
                templateTable.Rows[templateTable.Rows.Count - 1].Cells[3].Value = pair[1];
            }
        }

        private void autoDestinationTagCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (autoDestinationTagCheckBox.Checked)
            {
                destinationTagListCustom.Enable(false);
                var destinationTagListSelectedIndex = destinationTagListCustom.SelectedIndex;
                destinationTagListCustom.SelectedItem = sourceTagListCustom.SelectedItem;

                if (destinationTagListCustom.SelectedIndex == -1)
                {
                    destinationTagListCustom.SelectedIndex = destinationTagListSelectedIndex;
                }
            }
            else
            {
                destinationTagListCustom.Enable(true);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            autoDestinationTagCheckBox.Checked = !autoDestinationTagCheckBox.Checked;
        }

        private void sourceTagList_SelectedIndexChanged(object sender, EventArgs e)
        {
            autoDestinationTagCheckBox_CheckedChanged(null, null);
        }

        private void SearchOnlyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            enableDisablePreviewOptionControls(true);
            clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewChanges, buttonPreview, buttonOK, buttonClose, true);
        }

        private void buttonDeleteSaved_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, MsgMsrAreYouSureYouWantToDeleteAsrPreset.Replace("%%PRESET-NAME%%", templateNameTextBox.Text),
                    MsgMsrDeletePreset, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;


            SavedSettings.autoAppliedAsrPresetGuids.RemoveExisting(customMSR.guid);

            for (var j = 0; j < AsrPresetsWithHotkeys.Length; j++)
            {
                if (AsrPresetsWithHotkeys[j] == customMSR)
                {
                    AsrPresetsWithHotkeys[j] = null;
                    SavedSettings.asrPresetsWithHotkeysGuids[j] = Guid.Empty;
                    AsrPresetsWithHotkeysCount--;
                }
            }

            foreach (var pair in IdsAsrPresets)
            {
                if (pair.Value == customMSR)
                    IdsAsrPresets.Remove(pair.Key);
            }

            File.Delete(Path.Combine(PresetsPath, customMSR.getSafeFileName() + AsrPresetExtension));

            Presets.Remove(customMSR.guid);
            loadComboBoxCustom.Items.Remove(customMSR);

            loadComboBoxCustom.SelectedIndex = 0;
            buttonDeleteSaved.Enable(false);

            TagToolsPlugin.SaveSettings();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (!ignoreSplitterMovedEvent)
                saveWindowLayout(0, 0, 0, splitContainer1.SplitterDistance);
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            var settings = new QuickSettings(TagToolsPlugin);
            Display(settings, true);
        }

        private void autoApplyPictureBox_Click(object sender, EventArgs e)
        {
            autoApplyCheckBox.Checked = !autoApplyCheckBox.Checked;
        }

        private void smartOperationCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!smartOperationCheckBox.IsEnabled())
                return;

            smartOperationCheckBox.Checked = !smartOperationCheckBox.Checked;
        }

        private void MultipleSearchAndReplace_Load(object sender, EventArgs e)
        {
            var value = loadWindowLayout();

            if (value.Item1 != 0)
            {
                templateTable.Columns[3].FillWeight = value.Item1;
                templateTable.Columns[4].FillWeight = value.Item2;

                previewTable.Columns[1].FillWeight = value.Item5;
                previewTable.Columns[2].FillWeight = value.Item6;
                previewTable.Columns[3].FillWeight = value.Item7;
            }


            ignoreSplitterMovedEvent = true;

            //Let's scale split containers manually (auto-scaling is improper)
            foreach (var scsa in splitContainersScalingAttributes)
            {
                var sc = scsa.splitContainer;
                sc.Panel1MinSize = (int)Math.Round(scsa.panel1MinSize * vDpiFontScaling);
                sc.Panel2MinSize = (int)Math.Round(scsa.panel2MinSize * vDpiFontScaling);

                if (value.Item4 != 0)
                    sc.SplitterDistance = (int)Math.Round(value.Item4 * vDpiFontScaling);
                else
                    sc.SplitterDistance = (int)Math.Round(scsa.splitterDistance * vDpiFontScaling);
            }

            ignoreSplitterMovedEvent = false; //-V3008


            moveScaleButtonAdd();
            moveScaleButtonDelete();


            moveScaleButtonSave();
            moveScaleAutoApplyCheckBox();
            moveScaleTemplateNameTextBox();
            moveScalePresetLabel();
            moveScaleAutoApplyPictureBox();

            moveScaleLoadComboBoxCustom();
        }

        private void MultipleSearchAndReplace_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ignoreClosingForm)
            {
                if (!backgroundTaskIsUpdatingTags)
                {
                    closeFormOnStopping = true;
                    buttonClose.Enable(false);
                }

                backgroundTaskIsStopping = true;
                SetStatusBarText(MsrSbText + SbTextStoppingCurrentOperation, false);

                e.Cancel = true;
            }
            else
            {
                saveWindowLayout(templateTable.Columns[3].Width, templateTable.Columns[4].Width, 0, 
                    0,
                    previewTable.Columns[1].Width, previewTable.Columns[2].Width, previewTable.Columns[3].Width);
            }
        }
    }
}
