using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using ExtensionMethods;

using static MusicBeePlugin.AdvancedSearchAndReplace;
using static MusicBeePlugin.Plugin;


namespace MusicBeePlugin
{
    public partial class MultipleSearchAndReplace : PluginWindowTemplate
    {
        public class Row
        {
            public string Checked { get; set; }
            public string File { get; set; }
            public string Track { get; set; }
            public string OriginalTagValue { get; set; }
            public string NewTagValue { get; set; }
        }

        private CustomComboBox sourceTagListCustom;
        private CustomComboBox destinationTagListCustom;
        private CustomComboBox loadComboBoxCustom;


        private readonly DataGridViewCellStyle unchangedCellStyle = new DataGridViewCellStyle(UnchangedCellStyle);
        private readonly DataGridViewCellStyle changedCellStyle = new DataGridViewCellStyle(ChangedCellStyle);
        private readonly DataGridViewCellStyle dimmedCellStyle = new DataGridViewCellStyle(DimmedCellStyle);


        private List<Row> rows = new List<Row>();
        private BindingSource source = new BindingSource();

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

        public MultipleSearchAndReplace(Plugin plugin) : base(plugin)
        {
            InitializeComponent();

            WindowIcon = MsrIcon;
            TitleBarText = this.Text;

            new ControlBorder(this.templateNameTextBox);
        }

        internal protected override void initializeForm()
        {
            base.initializeForm();

            sourceTagListCustom = namesComboBoxes["sourceTagList"];
            destinationTagListCustom = namesComboBoxes["destinationTagList"];
            loadComboBoxCustom = namesComboBoxes["loadComboBox"];


            //Setting themed images
            autoApplyPictureBox.Image = ReplaceBitmap(autoApplyPictureBox.Image, AutoAppliedPresetsAccent);

            buttonLabels[buttonDeleteSaved] = string.Empty;
            buttonDeleteSaved.Text = string.Empty;
            ReplaceButtonBitmap(buttonDeleteSaved, ClearField);

            ReplaceButtonBitmap(buttonSettings, Gear);


            FillListByTagNames(destinationTagListCustom.Items, false, false, false);
            destinationTagListCustom.Text = SavedSettings.copyDestinationTagName;

            FillListByTagNames(sourceTagListCustom.Items, true, false, false);
            sourceTagListCustom.Text = SavedSettings.copySourceTagName;

            smartOperationCheckBox.Checked = SavedSettings.smartOperation;


            var headerCheckBoxCellStyle = new DataGridViewCellStyle(HeaderCellStyle);
            headerCheckBoxCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            templateTable.Columns[0].HeaderCell.Style = headerCheckBoxCellStyle;
            templateTable.Columns[1].HeaderCell.Style = headerCheckBoxCellStyle;


            var headerCellStyle = new DataGridViewCellStyle(HeaderCellStyle);

            templateTable.Columns[2].HeaderCell.Style = headerCellStyle;
            templateTable.Columns[3].HeaderCell.Style = headerCellStyle;

            var cbHeader = new DataGridViewCheckBoxHeaderCell();
            cbHeader.Style = headerCellStyle;
            cbHeader.setState(true);

            var colCB = new DataGridViewCheckBoxColumn
            {
                HeaderCell = cbHeader,
                ThreeState = true,
                FalseValue = ColumnUncheckedState,
                TrueValue = ColumnCheckedState,
                IndeterminateValue = string.Empty,
                Width = 25,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Resizable = DataGridViewTriState.False,
                DataPropertyName = "Checked",
            };

            previewTable.Columns.Insert(0, colCB);
            previewTable.Columns[1].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[2].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[3].HeaderCell.Style = headerCellStyle;


            customMSR = null;

            loadComboBoxCustom.Sorted = false;
            loadComboBoxCustom.Add(CtlNewAsrPreset);
            foreach (var preset in Presets)
            {
                if (preset.Value.isMSRPreset)
                    loadComboBoxCustom.Add(preset.Value);
            }
            loadComboBoxCustom.SelectedIndex = 0;

            buttonAdd_Click(null, null);

            templateNameTextBox.Text = CtlMSR;
            templateNameTextBox.SelectionStart = CtlMSR.Length;

            source.DataSource = rows;
            previewTable.DataSource = source;

            (previewTable.Columns[0].HeaderCell as DataGridViewCheckBoxHeaderCell).OnCheckBoxClicked += cbHeader_OnCheckBoxClicked;


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
            buttonSettings.Left = autoApplyCheckBox.Left - (int)Math.Round(hDpiFontScaling);
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
            previewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            previewTable.AllowUserToResizeColumns = true;
            previewTable.AllowUserToResizeRows = true;
            foreach (DataGridViewColumn column in previewTable.Columns)
                column.SortMode = DataGridViewColumnSortMode.Automatic;


            previewIsGenerated = false;
            backgroundTaskIsStopping = false;
            backgroundTaskIsStoppedOrCancelled = false;

            tags.Clear();
            templates.Clear();
            rows.Clear();
            source.ResetBindings(false);

            (previewTable.Columns[0].HeaderCell as DataGridViewCheckBoxHeaderCell).setState(true);

            updateCustomScrollBars(previewTable);
            SetStatusBarText(this, string.Empty, false);

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
            previewTable.AllowUserToResizeColumns = true;
            previewTable.AllowUserToResizeRows = true;
            foreach (DataGridViewColumn column in previewTable.Columns)
                column.SortMode = DataGridViewColumnSortMode.Automatic;

            previewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            autoSizeTableRows(previewTable, 2);


            backgroundTaskIsScheduled = false;
            backgroundTaskIsStopping = false;
            backgroundTaskIsStoppedOrCancelled = false;

            enableDisablePreviewOptionControls(true);
            enableQueryingOrUpdatingButtons();

            updateCustomScrollBars(previewTable);
            SetResultingSbText(this);

            if (closeFormOnStopping)
            {
                ignoreClosingForm = false;
                Close();
                return;
            }

            ignoreClosingForm = false;

            previewTable.Focus();
        }

        private bool applyingChangesStopped()
        {
            previewTable.AllowUserToResizeColumns = true;
            previewTable.AllowUserToResizeRows = true;
            foreach (DataGridViewColumn column in previewTable.Columns)
                column.SortMode = DataGridViewColumnSortMode.Automatic;

            previewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            previewTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;


            backgroundTaskIsScheduled = false;
            backgroundTaskIsStopping = false;
            backgroundTaskIsStoppedOrCancelled = false;
            closeFormOnStopping = false;

            previewTable_ProcessRowsOfTable(processedRowList);

            SetResultingSbText(this);

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
            if (backgroundTaskIsStopping)
                backgroundTaskIsStoppedOrCancelled = true;

            if (previewIsGenerated)
            {
                resetPreviewData();
                return true;
            }

            previewIsGenerated = false;

            if (backgroundTaskIsWorking())
                return true;

            resetPreviewData();


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

                previewTable.AllowUserToResizeColumns = false;
                previewTable.AllowUserToResizeRows = false;
                foreach (DataGridViewColumn column in previewTable.Columns)
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;

                previewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                previewTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

                return true;
            }
        }

        private bool prepareBackgroundTask()
        {
            if (backgroundTaskIsWorking())
                return false;

            if (rows.Count == 0 && !prepareBackgroundPreview())
                return false;

            previewTable.AllowUserToResizeColumns = false;
            previewTable.AllowUserToResizeRows = false;
            foreach (DataGridViewColumn column in previewTable.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;


            tags.Clear();

            for (var fileCounter = 0; fileCounter < previewTable.Rows.Count; fileCounter++)
            {
                var tag = new string[5];

                tag[0] = rows[fileCounter].Checked;
                tag[3] = rows[fileCounter].NewTagValue;
                tag[4] = rows[fileCounter].File;

                tags.Add(tag);
            }

            ignoreClosingForm = true;

            return true;
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


                var currentFile = files[fileCounter];

                SetStatusBarTextForFileOperations(this, MsrSbText, true, fileCounter, files.Length, currentFile);

                var track = GetTrackRepresentation(currentFile);

                if (sourcePropId == 0)
                    sourceTagValue = GetFileTag(currentFile, sourceTagId);
                else
                    sourceTagValue = MbApiInterface.Library_GetFileProperty(currentFile, sourcePropId);

                var destinationTagValue = GetFileTag(currentFile, destinationTagId);
                var swappedTags = SwapTags(sourceTagValue, destinationTagValue, sourceTagId, destinationTagId, smartOperation);


                string newDestinationTagValue1 = null;
                string newDestinationTagValue2 = swappedTags.newDestinationNormalizedTagValue;

                foreach (var template in templates)
                {
                    try
                    {
                        var template1 = template[0];
                        var template2 = template[1];
                        var template3 = template[2];
                        var template4 = template[3];

                        if (template1 == ColumnCheckedState && template2 == ColumnCheckedState) //Regex/case-sensitive
                        {
                            newDestinationTagValue1 = Replace(currentFile, newDestinationTagValue2, template3, template4, false, out _);
                        }
                        else if (template1 == ColumnCheckedState && template2 == ColumnUncheckedState) //Regex/case-insensitive
                        {
                            newDestinationTagValue1 = Replace(currentFile, newDestinationTagValue2, template3, template4, true, out _);
                        }
                        else if (template1 == ColumnCheckedState && template2 == string.Empty) //Regex/case-preserving
                        {
                            throw new Exception(@"Unsupported parameter combination: ""Use regexes"" and ""Preserve case""!");
                        }
                        else if (template1 == ColumnUncheckedState && template2 == ColumnCheckedState) //Case-sensitive
                        {
                            template3 = Regex.Escape(template3);
                            newDestinationTagValue1 = Replace(currentFile, newDestinationTagValue2, template3, template4, false, out _);
                        }
                        else if (template1 == ColumnUncheckedState && template2 == ColumnUncheckedState) //Case-insensitive
                        {
                            template3 = Regex.Escape(template3);
                            newDestinationTagValue1 = Replace(currentFile, newDestinationTagValue2, template3, template4, true, out _);
                        }
                        else //if (template1 == ColumnUncheckedState && template2 == string.Empty) //Case-preserving
                        {
                            template3 = Regex.Escape(template3);
                            newDestinationTagValue1 = Replace(currentFile, newDestinationTagValue2, template3, template4, null, out _);
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
                    isChecked = ColumnUncheckedState;
                else
                    isChecked = ColumnCheckedState;


                Row row = new Row
                {
                    Checked = isChecked,
                    File = currentFile,
                    Track = track,
                    OriginalTagValue = swappedTags.destinationNormalizedTagValue,
                    NewTagValue = newDestinationTagValue1,
                };

                rows.Add(row);


                int rowCountToFormat1 = 0;
                Invoke(new Action(() => { rowCountToFormat1 = AddRowsToTable(this, previewTable, source, rows.Count, false, true); }));
                Invoke(new Action(() => { FormatChangedTags(this, previewTable, rowCountToFormat1, false, previewTableFormatRow); }));
            }

            int rowCountToFormat3 = 0;
            Invoke(new Action(() => { rowCountToFormat3 = AddRowsToTable(this, previewTable, source, rows.Count, true, true); }));
            Invoke(new Action(() => { FormatChangedTags(this, previewTable, rowCountToFormat3, true, previewTableFormatRow); checkStoppedStatus(); resetFormToGeneratedPreview(); }));
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

                if (isChecked == ColumnCheckedState)
                {
                    var currentFile = tags[i][4];
                    var newTag = tags[i][3];

                    if (newTag != CtlAsrSyntaxError)
                    {
                        tags[i][0] = string.Empty;

                        processedRowList.Add(true);
                        SetStatusBarTextForFileOperations(this, MsrSbText, false, i, tags.Count, currentFile);

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
            SetResultingSbText(this);
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
            ignoreClosingForm = clickOnPreviewButton(prepareBackgroundPreview, previewChanges, buttonPreview, buttonOK, buttonClose);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbHeader_OnCheckBoxClicked(bool state)
        {
            if (rows.Count == 0)
                return;


            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[0].Checked == null)
                    continue;

                if (state)
                    rows[0].Checked = ColumnUncheckedState;
                else
                    rows[0].Checked = ColumnCheckedState;
            }

            int firstRow = previewTable.FirstDisplayedCell.RowIndex;

            source.ResetBindings(false);
            for (int i = 0; i < rows.Count; i++)
                previewTableFormatRow(previewTable, i);

            var firstCell = previewTable.Rows[firstRow].Cells[0];
            previewTable.FirstDisplayedCell = firstCell;
        }

        private void previewTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                var isChecked = previewTable.Rows[e.RowIndex].Cells[0].Value as string;

                if (isChecked == ColumnCheckedState)
                    previewTable.Rows[e.RowIndex].Cells[0].Value = ColumnUncheckedState;
                else if (isChecked == ColumnUncheckedState)
                    previewTable.Rows[e.RowIndex].Cells[0].Value = ColumnCheckedState;

                previewTableFormatRow(previewTable, e.RowIndex);
            }
        }

        private void templateTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                var isCheckedRegex = templateTable.Rows[e.RowIndex].Cells[0].Value as string;

                if (isCheckedRegex == ColumnCheckedState)
                {
                    templateTable.Rows[e.RowIndex].Cells[0].Value = ColumnUncheckedState;
                }
                else //if (isCheckedRegex == ColumnUncheckedState)
                {
                    templateTable.Rows[e.RowIndex].Cells[0].Value = ColumnCheckedState;

                    var isCheckedStateCasing = templateTable.Rows[e.RowIndex].Cells[1].Value as string;
                    if (isCheckedStateCasing == string.Empty)
                        templateTable.Rows[e.RowIndex].Cells[1].Value = ColumnUncheckedState;
                }
            }
            else if (e.ColumnIndex == 1)
            {
                var isCheckedStateCasing = templateTable.Rows[e.RowIndex].Cells[1].Value as string;

                if (isCheckedStateCasing == ColumnCheckedState)
                {
                    templateTable.Rows[e.RowIndex].Cells[1].Value = ColumnIndeterminateState;

                    var isCheckedRegex = templateTable.Rows[e.RowIndex].Cells[0].Value as string;
                    if (isCheckedRegex == ColumnCheckedState)
                        templateTable.Rows[e.RowIndex].Cells[0].Value = ColumnUncheckedState;
                }
                else if (isCheckedStateCasing == ColumnIndeterminateState)
                {
                    templateTable.Rows[e.RowIndex].Cells[1].Value = ColumnUncheckedState;
                }
                else //if (isCheckedStateCasing == ColumnUncheckedState)
                {
                    templateTable.Rows[e.RowIndex].Cells[1].Value = ColumnCheckedState;
                }
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

            if (templateTable.RowCount == 1)
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

            if (dataGridView.Rows[rowIndex].Cells[0].Value as string != ColumnCheckedState)
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
                    if (dataGridView.Rows[rowIndex].Cells[3].Value as string == dataGridView.Rows[rowIndex].Cells[2].Value as string)
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

        internal static string EncodeSearchReplaceTemplate(string searchTemplate, string replaceTemplate, bool useRegexes, bool? caseSensitive)
        {
            searchTemplate = searchTemplate.Replace(@"\", @"\\").Replace(@"#", @"\#").Replace("?", @"\?").Replace("/", @"\L").Replace("|", @"\V").Replace(@"*", @"\X");
            replaceTemplate = replaceTemplate.Replace(@"\", @"\\").Replace(@"#", @"\#").Replace("?", @"\?").Replace("/", @"\L").Replace("|", @"\V").Replace(@"*", @"\X").Replace(@"$", @"\T");

            string query;

            if (caseSensitive == true && useRegexes)
            {
                query = "#*" + searchTemplate + "/" + replaceTemplate;
            }
            else if (caseSensitive == null && useRegexes)
            {
                query = "?*" + searchTemplate + "/" + replaceTemplate;
            }
            else if (caseSensitive == false && useRegexes)
            {
                query = "*" + searchTemplate + "/" + replaceTemplate;
            }
            else if (caseSensitive == true && !useRegexes)
            {
                query = "#" + searchTemplate + "/" + replaceTemplate;
            }
            else if (caseSensitive == null && !useRegexes)
            {
                query = "?" + searchTemplate + "/" + replaceTemplate;
            }
            else //if (caseSensitive == false && !useRegexes)
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
                    customMSR = new Preset(MSR, false, false);
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
            for (var i = 0; i < templateTable.RowCount; i++)
            {
                if (templateTable.Rows[i].Cells[1].Value as string == ColumnCheckedState)
                {
                    query += EncodeSearchReplaceTemplate(string.Empty + templateTable.Rows[i].Cells[2].Value, string.Empty + templateTable.Rows[i].Cells[3].Value,
                        templateTable.Rows[i].Cells[0].Value as string == ColumnCheckedState, true) + "|";
                }
                else if (templateTable.Rows[i].Cells[1].Value as string == ColumnUncheckedState)
                {
                    query += EncodeSearchReplaceTemplate(string.Empty + templateTable.Rows[i].Cells[2].Value, string.Empty + templateTable.Rows[i].Cells[3].Value,
                        templateTable.Rows[i].Cells[0].Value as string == ColumnCheckedState, false) + "|";
                }
                else if (templateTable.Rows[i].Cells[1].Value as string == ColumnIndeterminateState)
                {
                    query += EncodeSearchReplaceTemplate(string.Empty + templateTable.Rows[i].Cells[2].Value, string.Empty + templateTable.Rows[i].Cells[3].Value,
                        templateTable.Rows[i].Cells[0].Value as string == ColumnCheckedState, null) + "|";
                }
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
                customMSR.names.Add(name, templateNameTextBox.Text);

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

            if (System.IO.File.Exists(Path.Combine(PresetsPath, oldSafeFileName + AsrPresetExtension)))
                System.IO.File.Delete(Path.Combine(PresetsPath, oldSafeFileName + AsrPresetExtension));

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
                loadComboBoxCustom.RemoveAt(index);
                loadComboBoxCustom.Insert(index, customMSR);
            }
            else
            {
                loadComboBoxCustom.Add(customMSR);
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
            templateTable.Rows[templateTable.Rows.Count - 1].Cells[0].Value = ColumnUncheckedState;
            templateTable.Rows[templateTable.Rows.Count - 1].Cells[1].Value = ColumnUncheckedState;
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

            customMSR = (Preset)loadComboBoxCustom.SelectedItem;
            buttonDeleteSaved.Enable(true);
            autoApplyCheckBox.Checked = SavedSettings.autoAppliedAsrPresetGuids.Contains(customMSR.guid); //-V3080

            //ignoreTemplateNameTextBoxTextChanged = true;
            templateNameTextBox.Text = customMSR.getName();
            //ignoreTemplateNameTextBoxTextChanged = false;

            templateTable.RowCount = 0;

            sourceTagListCustom.SelectedItem = GetTagName((MetaDataType)customMSR.parameterTagId);
            destinationTagListCustom.SelectedItem = GetTagName((MetaDataType)customMSR.parameterTag2Id);


            templateTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            var queries = customMSR.customText.Split('|');

            foreach (var query in queries)
            {
                buttonAdd_Click(null, null);

                string[] pair;
                if (query.Length > 1 && query[0] == '#' && query[1] == '*') //Case-sensitive/regexes
                {
                    pair = query.Substring(2).Split('/');

                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[0].Value = ColumnCheckedState;
                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[1].Value = ColumnCheckedState;
                }
                else if (query.Length > 1 && query[0] == '?' && query[1] == '*') //Case-preserving/regexes
                {
                    pair = query.Substring(2).Split('/');

                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[0].Value = ColumnCheckedState;
                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[1].Value = ColumnIndeterminateState;
                }
                else if (query.Length > 0 && query[0] == '*') //Case-insensitive/regexes
                {
                    pair = query.Substring(1).Split('/');

                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[0].Value = ColumnCheckedState;
                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[1].Value = ColumnUncheckedState;
                }
                else if (query.Length > 0 && query[0] == '#') //Case-sensitive
                {
                    pair = query.Substring(1).Split('/');

                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[0].Value = ColumnUncheckedState;
                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[1].Value = ColumnCheckedState;
                }
                else if (query.Length > 0 && query[0] == '?') //Case-preserving
                {
                    pair = query.Substring(1).Split('/');

                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[0].Value = ColumnUncheckedState;
                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[1].Value = ColumnIndeterminateState;
                }
                else //Case-insensitive
                {
                    pair = query.Split('/');

                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[0].Value = ColumnUncheckedState;
                    templateTable.Rows[templateTable.Rows.Count - 1].Cells[1].Value = ColumnUncheckedState;
                }

                pair[0] = pair[0].Replace(@"\#", "#").Replace(@"\?", "?").Replace(@"\L", "/").Replace(@"\V", "|").Replace(@"\X", "*").Replace(@"\\", @"\");
                pair[1] = pair[1].Replace(@"\#", "#").Replace(@"\?", "?").Replace(@"\L", "/").Replace(@"\V", "|").Replace(@"\X", "*").Replace(@"\\", @"\").Replace(@"\T", @"$");

                templateTable.Rows[templateTable.Rows.Count - 1].Cells[2].Value = pair[0];
                templateTable.Rows[templateTable.Rows.Count - 1].Cells[3].Value = pair[1];
            }

            templateTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            templateTable.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
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

            string customMSRKey = null;
            foreach (var pair in IdsAsrPresets)
            {
                if (pair.Value == customMSR)
                {
                    customMSRKey = pair.Key;
                    break;
                }
            }

            if (customMSRKey != null)
                IdsAsrPresets.Remove(customMSRKey);


            System.IO.File.Delete(Path.Combine(PresetsPath, customMSR.getSafeFileName() + AsrPresetExtension));

            Presets.Remove(customMSR.guid);
            loadComboBoxCustom.Remove(customMSR);

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
            Display(settings, this, true);
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
                templateTable.Columns[2].FillWeight = value.Item1;
                templateTable.Columns[3].FillWeight = value.Item2;
                templateTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                previewTable.Columns[2].FillWeight = value.Item5;
                previewTable.Columns[3].FillWeight = value.Item6;
                previewTable.Columns[4].FillWeight = value.Item7;
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

                    backgroundTaskIsStopping = true;
                    SetStatusBarText(this, MsrSbText + SbTextStoppingCurrentOperation, false);

                    e.Cancel = true;
                }
            }
            else
            {
                saveWindowLayout(templateTable.Columns[2].Width, templateTable.Columns[3].Width, 0,
                    0,
                    previewTable.Columns[2].Width, previewTable.Columns[3].Width, previewTable.Columns[4].Width);
            }
        }

        private void templateTable_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void previewTable_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
