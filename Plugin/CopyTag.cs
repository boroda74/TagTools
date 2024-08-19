using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class CopyTag : PluginWindowTemplate
    {
        private CustomComboBox sourceTagListCustom;
        private CustomComboBox destinationTagListCustom;
        private CustomComboBox fileNameTextBoxCustom;
        private CustomComboBox appendedTextBoxCustom;
        private CustomComboBox addedTextBoxCustom;


        private readonly DataGridViewCellStyle unchangedCellStyle = new DataGridViewCellStyle(UnchangedCellStyle);
        private readonly DataGridViewCellStyle changedCellStyle = new DataGridViewCellStyle(ChangedCellStyle);
        private readonly DataGridViewCellStyle dimmedCellStyle = new DataGridViewCellStyle(DimmedCellStyle);

        private MetaDataType sourceTagId;
        private FilePropertyType sourcePropId;
        private MetaDataType destinationTagId;
        private bool sourceTagIsSN;
        private bool sourceTagIsClipboard;
        private bool sourceTagIsTextFile;
        private bool onlyIfDestinationEmpty;
        private bool onlyIfSourceNotEmpty;
        private bool smartOperation;
        private bool appendSource;
        private bool addSource;
        private string appendedText;
        private string addedText;

        private string[] files = Array.Empty<string>();
        private readonly List<string[]> tags = new List<string[]>();
        private string[] fileTags;
        private List<bool> processedRowList = new List<bool>(); //Indices of processed tracks//--------------------

        internal CopyTag(Plugin plugin) : base(plugin)
        {
            InitializeComponent();
        }

        protected override void initializeForm()
        {
            base.initializeForm();

            sourceTagListCustom = namesComboBoxes["sourceTagList"];
            destinationTagListCustom = namesComboBoxes["destinationTagList"];
            fileNameTextBoxCustom = namesComboBoxes["fileNameTextBox"];
            appendedTextBoxCustom = namesComboBoxes["appendedTextBox"];
            addedTextBoxCustom = namesComboBoxes["addedTextBox"];


            fileNameLabel.Enable(false);

            buttonSettings.Image = ReplaceBitmap(buttonSettings.Image, Gear);

            FillListByTagNames(destinationTagListCustom.Items);
            destinationTagListCustom.Text = SavedSettings.copyDestinationTagName;

            sourceTagListCustom.Text = SavedSettings.copySourceTagName;

            onlyIfDestinationEmptyCheckBox.Checked = SavedSettings.onlyIfDestinationIsEmpty;
            onlyIfSourceNotEmptyCheckBox.Checked = SavedSettings.onlyIfSourceNotEmpty;
            smartOperationCheckBox.Checked = SavedSettings.smartOperation;
            appendCheckBox.Checked = SavedSettings.appendSource;
            addCheckBox.Checked = SavedSettings.addSource;

            fileNameTextBoxCustom.Text = SavedSettings.customText[0] as string;
            fileNameTextBoxCustom.AddRange(SavedSettings.customText);
            appendedTextBoxCustom.Text = SavedSettings.appendedText[0] as string;
            appendedTextBoxCustom.AddRange(SavedSettings.appendedText);
            addedTextBoxCustom.Text = SavedSettings.addedText[0] as string;
            addedTextBoxCustom.AddRange(SavedSettings.addedText);

            appendedTextBoxCustom.Enable(appendCheckBox.Checked);


            var headerCellStyle = new DataGridViewCellStyle(HeaderCellStyle);

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
            previewTable.Columns[2].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[6].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[8].HeaderCell.Style = headerCellStyle;

            previewTable.Columns[2].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[6].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[8].HeaderCell.Style.WrapMode = DataGridViewTriState.True;

            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            enableDisablePreviewOptionControls(true, true);
            enableQueryingOrUpdatingButtons();


            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        private void previewTable_ProcessRowsOfTable(List<bool> processedRowList)//-----------------
        {
            for (int i = 0; i < processedRowList.Count; i++)
            {
                previewTable.CurrentCell = previewTable.Rows[i].Cells[0];

                if (processedRowList[i])
                    previewTable.Rows[i].Cells[0].Value = null;

                previewTableFormatRow(previewTable, i);
            }
        }

        private void resetPreviewData()//***********
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


            backgroundTaskIsStopping = false;//---------
            backgroundTaskIsStoppedOrCancelled = false;

            tags.Clear();
            previewTable.RowCount = 0;
            (previewTable.Columns[0].HeaderCell as DataGridViewCheckBoxHeaderCell).setState(true);

            updateCustomScrollBars(previewTable);
            SetStatusBarText(string.Empty, false);

            enableQueryingOrUpdatingButtons();
            enableDisablePreviewOptionControls(true);

            if (closeFormOnStopping && ignoreClosingForm && backgroundTaskIsScheduled)//**************
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

            previewTable.AllowUserToResizeColumns = true;//-----------------
            previewTable.AllowUserToResizeRows = true;
            foreach (DataGridViewColumn column in previewTable.Columns)
                column.SortMode = DataGridViewColumnSortMode.Automatic;//----------------- Not for all commands!!!!


            backgroundTaskIsScheduled = false;//-----------------
            backgroundTaskIsStopping = false;
            backgroundTaskIsStoppedOrCancelled = false;

            enableDisablePreviewOptionControls(true);
            enableQueryingOrUpdatingButtons();

            updateCustomScrollBars(previewTable);
            SetResultingSbText();

            if (closeFormOnStopping)//**********
            {
                ignoreClosingForm = false;
                Close();
            }

            ignoreClosingForm = false;
        }

        private bool applyingChangesStopped()//---------------
        {
            previewTable.AllowUserToResizeColumns = true;//-----------------
            previewTable.AllowUserToResizeRows = true;
            foreach (DataGridViewColumn column in previewTable.Columns)
                column.SortMode = DataGridViewColumnSortMode.Automatic;//----------------- Not for all commands!!!!


            backgroundTaskIsScheduled = false;
            backgroundTaskIsStopping = false;
            backgroundTaskIsStoppedOrCancelled = false;
            closeFormOnStopping = false;
            //ignoreClosingForm = false;//------------------

            previewTable_ProcessRowsOfTable(processedRowList);

            SetResultingSbText();

            if (closeFormOnStopping)//-----------------
            {
                ignoreClosingForm = false;
                Close();
            }

            ignoreClosingForm = false;

            return true;
        }

        internal bool prepareBackgroundPreview() //--------------- disable column/row resizing/sorting!!!
        {
            resetPreviewData();//******

            if (backgroundTaskIsStopping)//*******
                backgroundTaskIsStoppedOrCancelled = true;

            if (previewIsGenerated)
            {
                previewIsGenerated = false;
                enableDisablePreviewOptionControls(true);//********
                return true;
            }

            previewIsGenerated = false;

            if (backgroundTaskIsWorking())
                return true;

            if (fileNameTextBoxCustom.IsEnabled() && !System.IO.File.Exists(fileNameTextBoxCustom.Text))
            {
                MessageBox.Show(this, MsgFileNotFound, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }


            previewTable.AllowUserToResizeColumns = false;//--------------------
            previewTable.AllowUserToResizeRows = false;
            foreach (DataGridViewColumn column in previewTable.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;


            sourceTagId = GetTagId(sourceTagListCustom.Text);
            sourcePropId = GetPropId(sourceTagListCustom.Text);
            destinationTagId = GetTagId(destinationTagListCustom.Text);
            sourceTagIsSN = (sourceTagListCustom.Text == SequenceNumberName);
            sourceTagIsClipboard = (sourceTagListCustom.Text == ClipboardTagName);
            sourceTagIsTextFile = fileNameTextBoxCustom.IsEnabled();
            smartOperation = smartOperationCheckBox.Checked;
            onlyIfDestinationEmpty = onlyIfDestinationEmptyCheckBox.Checked;
            onlyIfSourceNotEmpty = onlyIfSourceNotEmptyCheckBox.Checked;
            appendSource = appendCheckBox.Checked;
            addSource = addCheckBox.Checked;
            appendedText = appendedTextBoxCustom.Text;
            addedText = addedTextBoxCustom.Text;


            MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files);
            if (files == null || files.Length == 0)
            {
                MessageBox.Show(this, MsgNoTracksSelected, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }


            if (sourceTagIsTextFile)
            {
                System.IO.Stream stream = new System.IO.FileStream(fileNameTextBoxCustom.Text, System.IO.FileMode.Open);

                var buffer = new byte[stream.Length];

                var encoding = Encoding.Default; //Current ANSI encoding

                var utf8 = Encoding.UTF8;
                var unicode = Encoding.Unicode;
                var bigEndianUnicode = Encoding.BigEndianUnicode;

                var count = stream.Read(buffer, 0, (int)stream.Length);
                stream.Close();

                if (count > 1 && buffer[0] == unicode.GetPreamble()[0] && buffer[1] == unicode.GetPreamble()[1])
                    encoding = unicode;
                else if (count > 1 && buffer[0] == bigEndianUnicode.GetPreamble()[0] && buffer[1] == bigEndianUnicode.GetPreamble()[1])
                    encoding = bigEndianUnicode;
                else if (count > 1 && buffer[0] == utf8.GetPreamble()[0] && buffer[1] == utf8.GetPreamble()[1])
                    encoding = utf8;

                fileTags = encoding.GetString(buffer).Split(new[] { "\r\n" }, StringSplitOptions.None);

                if (fileTags.Length != files.Length)
                {
                    MessageBox.Show(this, MsgNumberOfTagsInTextFileDoesntCorrespondToNumberOfSelectedTracks
                        .Replace("%%TEXT-TAG-FILES-COUNT%%", fileTags.Length.ToString())
                        .Replace("%%SELECTED-FILES-COUNT%%", files.Length.ToString()),
                        string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            else if (sourceTagIsClipboard)
            {
                if (!Clipboard.ContainsText())
                {
                    MessageBox.Show(this, MsgClipboardDoesntContainText,
                        string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                fileTags = Clipboard.GetText().Split(new[] { "\r\n" }, StringSplitOptions.None);

                if (fileTags.Length != files.Length)
                {
                    MessageBox.Show(this, MsgNumberOfTagsInClipboardDoesntCorrespondToNumberOfSelectedTracks
                        .Replace("%%FILE-TAGS-LENGTH%%", fileTags.Length.ToString())
                        .Replace("%%SELECTED-FILES-COUNT%%", files.Length.ToString()),
                        string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            else
            {
                fileTags = null;
            }

            previewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            return true;
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
                previewTable.AllowUserToResizeColumns = false;//--------------------
                previewTable.AllowUserToResizeRows = false;
                foreach (DataGridViewColumn column in previewTable.Columns)
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;


                tags.Clear();

                for (var fileCounter = 0; fileCounter < previewTable.Rows.Count; fileCounter++)
                {
                    string[] tag = { "Checked", "File", "NewTag" }; //************ Move something similar here for all commands!!!
                    tag[0] = previewTable.Rows[fileCounter].Cells[0].Value as string;//***********
                    tag[1] = previewTable.Rows[fileCounter].Cells[1].Value as string;
                    tag[2] = previewTable.Rows[fileCounter].Cells[7].Value as string;

                    tags.Add(tag);
                }

                ignoreClosingForm = true;//------------

                return true;
            }
        }

        private void previewChanges()
        {
            previewIsGenerated = true;//******* at the beginning!!!

            List<string[]> rows = new List<string[]>();//********
            var stripNotChangedLines = SavedSettings.dontIncludeInPreviewLinesWithoutChangedTags;

            for (var fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (checkStoppingStatus())//*********
                {
                    Invoke(new Action(() => { stopButtonClickedMethod(prepareBackgroundPreview); }));
                    return;
                }


                string[] row = { "Checked", "File", "Track", "SupposedDestinationTag", "SupposedDestinationTagT", "OriginalDestinationTag", "OriginalDestinationTagT", "NewTag", "NewTagT" }; //************ Move something similar here for all commands!!!
                string[] tag = { "Checked", "File", "NewTag" };

                var currentFile = files[fileCounter];

                SetStatusBarTextForFileOperations(CopyTagSbText, true, fileCounter, files.Length, currentFile);

                string sourceTagValue;
                if (sourceTagIsTextFile || sourceTagIsClipboard)
                    sourceTagValue = fileTags[fileCounter];
                else if (sourceTagIsSN)
                    sourceTagValue = fileCounter.ToString();
                else if (sourcePropId == 0)
                    sourceTagValue = GetFileTag(currentFile, sourceTagId);
                else
                    sourceTagValue = MbApiInterface.Library_GetFileProperty(currentFile, sourcePropId);

                var destinationTagValue = GetFileTag(currentFile, destinationTagId);

                var track = GetTrackRepresentation(currentFile);

                var swappedTags = SwapTags(sourceTagValue, destinationTagValue, sourceTagId, destinationTagId,
                    smartOperation, appendSource, appendedText, addSource, addedText);

                string isChecked;
                if (swappedTags.destinationNormalizedTagValue == swappedTags.newDestinationNormalizedTagValue && stripNotChangedLines)//******** check all command that use SwapTags()!!!!
                    continue;
                else if (onlyIfDestinationEmpty && swappedTags.destinationNormalizedTagValue != string.Empty && stripNotChangedLines)
                    continue;
                else if (onlyIfSourceNotEmpty && swappedTags.sourceNormalizedTagValue == string.Empty && stripNotChangedLines)
                    continue;
                else if (swappedTags.destinationNormalizedTagValue == swappedTags.newDestinationNormalizedTagValue)
                    isChecked = "F";
                else if (onlyIfDestinationEmpty && swappedTags.destinationNormalizedTagValue != string.Empty)
                    isChecked = "F";
                else if (onlyIfSourceNotEmpty && swappedTags.sourceNormalizedTagValue == string.Empty)
                    isChecked = "F";
                else
                    isChecked = "T";


                tag[0] = isChecked;
                tag[1] = currentFile;
                tag[2] = swappedTags.newDestinationTagValue;

                tags.Add(tag);


                row[0] = isChecked;
                row[1] = currentFile;
                row[2] = track;
                row[3] = swappedTags.newDestinationTagValue;
                row[4] = swappedTags.newDestinationNormalizedTagValue;
                row[5] = destinationTagValue;
                row[6] = swappedTags.destinationNormalizedTagValue;

                if (isChecked == "T")
                {
                    row[7] = row[3];
                    row[8] = row[4];
                }
                else //if (isChecked == "F")
                {
                    row[7] = row[5];
                    row[8] = row[6];
                }

                rows.Add(row);


                int rowCountToFormat1 = 0;
                Invoke(new Action(() => { rowCountToFormat1 = AddRowsToTable(this, previewTable, rows, false, true); }));//********
                Invoke(new Action(() => { FormatChangedTags(this, previewTable, rowCountToFormat1, previewTableFormatRow); }));//---------------- => FormatChangedTags()
            }

            int rowCountToFormat2 = 0;
            Invoke(new Action(() => { rowCountToFormat2 = AddRowsToTable(this, previewTable, rows, true, true); }));//------------------- check if it's lst row == true!!!
            Invoke(new Action(() => { FormatChangedTags(this, previewTable, rowCountToFormat2, previewTableFormatRow); checkStoppedStatus(); resetFormToGeneratedPreview(); }));
        }

        private void applyChanges()
        {
            if (tags.Count == 0)
                throw new Exception("Something went wrong! Empty 'tags' local variable (must be filled on generating preview.)");//***********

            processedRowList.Clear(); //Indices of processed tracks//--------------------
            Invoke(new Action(() => { previewTable.CurrentCell = previewTable.Rows[0].Cells[0]; }));//*************
            Invoke(new Action(() => { previewTable.FirstDisplayedCell = previewTable.CurrentCell; }));//************

            for (var i = 0; i < tags.Count; i++)
            {
                if (checkStoppingStatus())//---------------
                {
                    Invoke(new Action(() => { stopButtonClickedMethod(applyingChangesStopped); }));
                    return;
                }


                var isChecked = tags[i][0];

                if (isChecked == "T")
                {
                    var currentFile = tags[i][1];
                    var newTag = tags[i][2];


                    tags[i][0] = string.Empty;

                    processedRowList.Add(true);//-------------------
                    SetStatusBarTextForFileOperations(CopyTagSbText, false, i, tags.Count, currentFile);//-------------------- IF "T"

                    SetFileTag(currentFile, destinationTagId, newTag);
                    CommitTagsToFile(currentFile);
                }
                else//--------------
                {
                    processedRowList.Add(false);
                }

                //Invoke(new Action(() => { previewTable.CurrentCell = previewTable.Rows[i].Cells[0]; }));//-------------
                //Invoke(new Action(() => { previewTable.FirstDisplayedCell = previewTable.CurrentCell; }));//-----------
            }

            //Invoke(new Action(() => { previewTable.CurrentCell = previewTable.Rows[tags.Count - 1].Cells[0]; }));//--------------
            //Invoke(new Action(() => { previewTable.FirstDisplayedCell = previewTable.CurrentCell; }));//------------------
            Invoke(new Action(() => { applyingChangesStopped(); }));//-----------------

            RefreshPanels(true);
            SetResultingSbText();
        }

        private void saveSettings()
        {
            saveWindowLayout(previewTable.Columns[1].Width, previewTable.Columns[2].Width, previewTable.Columns[3].Width);

            SavedSettings.copySourceTagName = sourceTagListCustom.Text;
            SavedSettings.copyDestinationTagName = destinationTagListCustom.Text;
            SavedSettings.onlyIfDestinationIsEmpty = onlyIfDestinationEmptyCheckBox.Checked;
            SavedSettings.onlyIfSourceNotEmpty = onlyIfSourceNotEmptyCheckBox.Checked;
            SavedSettings.smartOperation = smartOperationCheckBox.Checked;
            SavedSettings.appendSource = appendCheckBox.Checked;
            SavedSettings.addSource = addCheckBox.Checked;
            fileNameTextBoxCustom.Items.CopyTo(SavedSettings.customText, 0);
            appendedTextBoxCustom.Items.CopyTo(SavedSettings.appendedText, 0);
            addedTextBoxCustom.Items.CopyTo(SavedSettings.addedText, 0);

            TagToolsPlugin.SaveSettings();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (sourceTagListCustom.Text == destinationTagListCustom.Text)
            {
                MessageBox.Show(this, MsgSourceAndDestinationTagsAreTheSame, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            saveSettings();
            if (prepareBackgroundTask())
                switchOperation(applyChanges, sender as Button, buttonOK, buttonPreview, buttonClose, true, null);
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            if (!previewIsGenerated && sourceTagListCustom.Text == destinationTagListCustom.Text)
            {
                MessageBox.Show(this, MsgSourceAndDestinationTagsAreTheSame, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            ignoreClosingForm = clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewChanges, buttonPreview, buttonOK, buttonClose); //**********
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void appendCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            appendedTextBoxCustom.Enable(appendCheckBox.Checked);

            if (appendCheckBox.Checked)
                addCheckBox.Checked = false;
        }

        private void appendCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!appendCheckBox.IsEnabled())
                return;

            appendCheckBox.Checked = !appendCheckBox.Checked;
        }

        private void addCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            addedTextBoxCustom.Enable(addCheckBox.Checked);

            if (addCheckBox.Checked)
                appendCheckBox.Checked = false;
        }

        private void addCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!addCheckBox.IsEnabled())
                return;

            addCheckBox.Checked = !addCheckBox.Checked;
        }

        private void sourceTagList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var test = (sourceTagListCustom.Text == TextFileTagName);
            fileNameLabel.Enable(test);
            test = (sourceTagListCustom.Text == TextFileTagName);
            fileNameTextBoxCustom.Enable(test);
            test = (sourceTagListCustom.Text == TextFileTagName);
            buttonBrowse.Enable(test);
        }

        private void destinationTagList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (destinationTagListCustom.Text == MbApiInterface.Setting_GetFieldName(MetaDataType.HasLyrics))
            {
                sourceTagListCustom.ItemsClear();
                sourceTagListCustom.Items.Add("<Null>");
                sourceTagListCustom.Items.Add("<+>");

                sourceTagListCustom.SelectedIndex = 0;
            }
            else
            {
                var selectedIndex = sourceTagListCustom.SelectedIndex;

                sourceTagListCustom.ItemsClear();

                FillListByTagNames(sourceTagListCustom.Items, true);
                FillListByPropNames(sourceTagListCustom.Items);
                //sourceTagListCustom.Items.Add(MusicBeePlugin.emptyValueTagName);
                sourceTagListCustom.Items.Add(ClipboardTagName);
                sourceTagListCustom.Items.Add(TextFileTagName);
                sourceTagListCustom.Items.Add(SequenceNumberName);

                try
                {
                    sourceTagListCustom.SelectedIndex = selectedIndex;
                }
                catch
                {
                    sourceTagListCustom.SelectedIndex = 0;
                }
            }
        }

        private void customTextBox_EnabledChanged(object sender, EventArgs e)
        {
            fileNameLabel.Enable(fileNameTextBoxCustom.IsEnabled());
        }

        private void cbHeader_OnCheckBoxClicked(bool state)
        {
            foreach (DataGridViewRow row in previewTable.Rows)
            {
                if (row.Cells[0].Value == null)
                    continue;

                if (state)
                    row.Cells[0].Value = "F";
                else
                    row.Cells[0].Value = "T";

                var e = new DataGridViewCellEventArgs(0, row.Index);
                previewTable_CellContentClick(null, e);
            }
        }

        private void previewTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string newTagValue;
                string newTagTValue;

                var isChecked = previewTable.Rows[e.RowIndex].Cells[0].Value as string;

                if (isChecked == "T")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "F";

                    newTagValue = previewTable.Rows[e.RowIndex].Cells[5].Value as string;
                    newTagTValue = previewTable.Rows[e.RowIndex].Cells[6].Value as string;

                    previewTable.Rows[e.RowIndex].Cells[7].Value = newTagValue;
                    previewTable.Rows[e.RowIndex].Cells[8].Value = newTagTValue;
                }
                else if (isChecked == "F")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "T";

                    newTagValue = previewTable.Rows[e.RowIndex].Cells[3].Value as string;
                    newTagTValue = previewTable.Rows[e.RowIndex].Cells[4].Value as string;

                    previewTable.Rows[e.RowIndex].Cells[7].Value = newTagValue;
                    previewTable.Rows[e.RowIndex].Cells[8].Value = newTagTValue;
                }

                previewTableFormatRow(sender as DataGridView, e.RowIndex);
            }
        }

        internal override void enableDisablePreviewOptionControls(bool enable, bool dontChangeDisabled = false)
        {
            if (enable && previewIsGenerated)
                return;

            sourceTagListCustom.Enable(enable);
            destinationTagListCustom.Enable(enable);

            fileNameLabel.Enable(enable && sourceTagListCustom.Text == TextFileTagName);
            fileNameTextBoxCustom.Enable(enable && sourceTagListCustom.Text == TextFileTagName); //******** CUSTOM!!!!
            buttonBrowse.Enable(enable && sourceTagListCustom.Text == TextFileTagName);
            smartOperationCheckBox.Enable(enable);

            onlyIfDestinationEmptyCheckBox.Enable(enable);
            onlyIfSourceNotEmptyCheckBox.Enable(enable);

            addCheckBox.Enable(enable);
            addedTextBoxCustom.Enable(enable && addCheckBox.Checked);
            appendCheckBox.Enable(enable);
            appendedTextBoxCustom.Enable(enable && appendCheckBox.Checked);
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
            buttonOK.Enable((previewIsGenerated || SavedSettings.allowCommandExecutionWithoutPreview) && !backgroundTaskIsStopping && (!backgroundTaskIsWorking() || backgroundTaskIsUpdatingTags));//********
            buttonPreview.Enable(true);
        }

        internal override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enable(false);
            buttonPreview.Enable(false);
        }

        private void filenameTextBox_Leave(object sender, EventArgs e)
        {
            CustomComboBoxLeave(fileNameTextBoxCustom);
        }

        private void appendedTextBox_Leave(object sender, EventArgs e)
        {
            CustomComboBoxLeave(appendedTextBoxCustom);
        }

        private void addedTextBox_Leave(object sender, EventArgs e)
        {
            CustomComboBoxLeave(addedTextBoxCustom);
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
                if (columnIndex == 8)
                {
                    if ((string)dataGridView.Rows[rowIndex].Cells[6].Value == (string)dataGridView.Rows[rowIndex].Cells[8].Value)
                        dataGridView.Rows[rowIndex].Cells[8].Style = unchangedCellStyle;
                    else
                        dataGridView.Rows[rowIndex].Cells[8].Style = changedCellStyle;
                }
                else
                {
                    dataGridView.Rows[rowIndex].Cells[columnIndex].Style = unchangedCellStyle;
                }
            }
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = CtlCopyTagFilter,
                FilterIndex = 0
            };
            if (dialog.ShowDialog(this) == DialogResult.Cancel) return;
            dialog.Dispose();

            fileNameTextBoxCustom.Text = dialog.FileName;
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            var settings = new QuickSettings(TagToolsPlugin);
            Display(settings, true);
        }

        private void onlyIfDestinationEmptyCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!onlyIfDestinationEmptyCheckBox.IsEnabled())
                return;

            onlyIfDestinationEmptyCheckBox.Checked = !onlyIfDestinationEmptyCheckBox.Checked;
        }


        private void onlyIfSourceNotEmptyCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!onlyIfSourceNotEmptyCheckBox.IsEnabled())
                return;

            onlyIfSourceNotEmptyCheckBox.Checked = !onlyIfSourceNotEmptyCheckBox.Checked;
        }

        private void smartOperationCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!smartOperationCheckBox.IsEnabled())
                return;

            smartOperationCheckBox.Checked = !smartOperationCheckBox.Checked;
        }

        private void CopyTag_Load(object sender, EventArgs e)
        {
            var value = loadWindowLayout();

            if (value.Item1 != 0)
            {
                previewTable.Columns[2].FillWeight = value.Item1;//***********
                previewTable.Columns[6].FillWeight = value.Item2;
                previewTable.Columns[8].FillWeight = value.Item3;
            }
            //else //********** Column auto-size is Fill!!!!
            //{
            //    previewTable.Columns[2].Width = (int)Math.Round(previewTable.Columns[2].Width * hDpiFontScaling);
            //    previewTable.Columns[6].Width = (int)Math.Round(previewTable.Columns[6].Width * hDpiFontScaling);
            //    previewTable.Columns[8].Width = (int)Math.Round(previewTable.Columns[8].Width * hDpiFontScaling);
            //}
        }

        private void CopyTag_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ignoreClosingForm)//***********
            {
                if (!backgroundTaskIsUpdatingTags)//-------------------
                {
                    closeFormOnStopping = true;
                    buttonClose.Enable(false);
                }

                backgroundTaskIsStopping = true;
                SetStatusBarText(CopyTagSbText + SbTextStoppingCurrentOperation, false); //********* CopyTagSbText -> new command text

                e.Cancel = true;
            }
            else
            {
                saveWindowLayout(previewTable.Columns[2].Width, previewTable.Columns[6].Width, previewTable.Columns[8].Width);
            }
        }
    }
}
