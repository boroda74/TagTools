using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;
using ExtensionMethods;

namespace MusicBeePlugin
{
    public partial class CopyTagCommand : PluginWindowTemplate
    {
        private delegate void AddRowToTable(string[] row);
        private delegate void ProcessRowOfTable(int row);
        private AddRowToTable addRowToTable;
        private ProcessRowOfTable processRowOfTable;

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
        private string customText;
        private string appendedText;
        private string addedText;

        private string[] files = new string[0];
        private List<string[]> tags = new List<string[]>();
        private string[] fileTags;

        public CopyTagCommand(Plugin tagToolsPluginParam) : base(tagToolsPluginParam)
        {
            InitializeComponent();
        }

        protected override void initializeForm()
        {
            base.initializeForm();

            label3.Enable(false);

            buttonSettings.Image = ThemedBitmapAddRef(this, Gear);

            FillListByTagNames(destinationTagList.Items);
            destinationTagList.Text = SavedSettings.copyDestinationTagName;

            sourceTagList.Text = SavedSettings.copySourceTagName;

            onlyIfDestinationEmptyCheckBox.Checked = SavedSettings.onlyIfDestinationIsEmpty;
            onlyIfSourceNotEmptyCheckBox.Checked = SavedSettings.onlyIfSourceNotEmpty;
            smartOperationCheckBox.Checked = SavedSettings.smartOperation;
            appendCheckBox.Checked = SavedSettings.appendSource;
            addCheckBox.Checked = SavedSettings.addSource;

            fileNameTextBox.Text = SavedSettings.customText[0];
            fileNameTextBox.Items.AddRange(SavedSettings.customText);
            appendedTextBox.Text = SavedSettings.appendedText[0];
            appendedTextBox.Items.AddRange(SavedSettings.appendedText);
            addedTextBox.Text = SavedSettings.addedText[0];
            addedTextBox.Items.AddRange(SavedSettings.addedText);

            appendedTextBox.Enable(appendCheckBox.Checked);

            DatagridViewCheckBoxHeaderCell cbHeader = new DatagridViewCheckBoxHeaderCell();
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

            previewTable.Columns[2].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[6].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[8].HeaderCell.Style.WrapMode = DataGridViewTriState.True;

            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            addRowToTable = previewTable_AddRowToTable;
            processRowOfTable = previewTable_ProcessRowOfTable;


            enableDisablePreviewOptionControls(true, true);
            enableQueryingOrUpdatingButtons();

            button_GotFocus(this.AcceptButton, null); //Let's mark active button
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

        public bool prepareBackgroundPreview()
        {
            tags.Clear();
            previewTable.Rows.Clear();
            previewIsGenerated = false;
            ((DatagridViewCheckBoxHeaderCell)previewTable.Columns[0].HeaderCell).setState(true);

            if (backgroundTaskIsWorking())
                return true;

            if (fileNameTextBox.Enabled && !System.IO.File.Exists(fileNameTextBox.Text))
            {
                MessageBox.Show(this, MsgFileNotFound, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            sourceTagId = GetTagId(sourceTagList.Text);
            sourcePropId = GetPropId(sourceTagList.Text);
            destinationTagId = GetTagId(destinationTagList.Text);
            sourceTagIsSN = (sourceTagList.Text == SequenceNumberName);
            sourceTagIsClipboard = (sourceTagList.Text == ClipboardTagName);
            sourceTagIsTextFile = fileNameTextBox.Enabled;
            smartOperation = smartOperationCheckBox.Checked;
            onlyIfDestinationEmpty = onlyIfDestinationEmptyCheckBox.Checked;
            onlyIfSourceNotEmpty = onlyIfSourceNotEmptyCheckBox.Checked;
            appendSource = appendCheckBox.Checked;
            addSource = addCheckBox.Checked;
            customText = fileNameTextBox.Text;
            appendedText = appendedTextBox.Text;
            addedText = addedTextBox.Text;


            files = null;
            if (!MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files))
                files = new string[0];


            if (sourceTagIsTextFile)
            {
                System.IO.Stream stream;
                stream = new System.IO.FileStream(fileNameTextBox.Text, System.IO.FileMode.Open);

                byte[] buffer = new byte[stream.Length];

                Encoding encoding = Encoding.Default; //Current ANSI encoding

                Encoding utf8 = Encoding.UTF8;
                Encoding unicode = Encoding.Unicode;
                Encoding bigEndianUnicode = Encoding.BigEndianUnicode;

                stream.Read(buffer, 0, (int)stream.Length);
                stream.Close();

                if (buffer[0] == unicode.GetPreamble()[0] && buffer[1] == unicode.GetPreamble()[1])
                {
                    encoding = unicode;
                }
                else if (buffer[0] == bigEndianUnicode.GetPreamble()[0] && buffer[1] == bigEndianUnicode.GetPreamble()[1])
                {
                    encoding = bigEndianUnicode;
                }
                else if (buffer[0] == utf8.GetPreamble()[0] && buffer[1] == utf8.GetPreamble()[1])
                {
                    encoding = utf8;
                }

                fileTags = encoding.GetString(buffer).Split(new string[] { "\r\n" }, StringSplitOptions.None);

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

                fileTags = Clipboard.GetText().Split(new string[] { "\r\n" }, StringSplitOptions.None);

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


            if (files.Length == 0)
            {
                MessageBox.Show(this, MsgNoTracksSelected, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else
            {
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
                    tag = new string[3];

                    tag[0] = (string)previewTable.Rows[fileCounter].Cells[0].Value;
                    tag[1] = (string)previewTable.Rows[fileCounter].Cells[1].Value;
                    tag[2] = (string)previewTable.Rows[fileCounter].Cells[7].Value;

                    tags.Add(tag);
                }

                return true;
            }
        }

        private void previewChanges()
        {
            string currentFile;
            SwappedTags swappedTags;

            string sourceTagValue;
            string destinationTagValue;

            string isChecked;

            bool stripNotChangedLines = SavedSettings.dontIncludeInPreviewLinesWithoutChangedTags;

            string track;
            string[] row = { "Checked", "File", "Track", "SupposedDestinationTag", "SupposedDestinationTagT", "OriginalDestinationTag", "OriginalDestinationTagT", "NewTag", "NewTagT" };
            string[] tag = { "Checked", "file", "newTag" };

            int count = 0;
            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                count++;

                currentFile = files[fileCounter];

                SetStatusbarTextForFileOperations(CopyTagCommandSbText, true, fileCounter, files.Length, currentFile);

                if (sourceTagIsTextFile || sourceTagIsClipboard)
                {
                    sourceTagValue = fileTags[fileCounter];
                }
                else if (sourceTagIsSN)
                {
                    sourceTagValue = count.ToString();
                }
                else if (sourcePropId == 0)
                {
                    sourceTagValue = GetFileTag(currentFile, sourceTagId);
                }
                else
                {
                    sourceTagValue = MbApiInterface.Library_GetFileProperty(currentFile, sourcePropId);
                }

                destinationTagValue = GetFileTag(currentFile, destinationTagId);

                track = GetTrackRepresentation(currentFile);

                swappedTags = SwapTags(sourceTagValue, destinationTagValue, sourceTagId, destinationTagId,
                    smartOperation, appendSource, appendedText, addSource, addedText);

                if (sourceTagValue == destinationTagValue && stripNotChangedLines)
                    continue;
                else if (onlyIfDestinationEmpty && destinationTagValue != string.Empty && stripNotChangedLines)
                    continue;
                else if (onlyIfSourceNotEmpty && sourceTagValue == string.Empty && stripNotChangedLines)
                    continue;
                else if (sourceTagValue == destinationTagValue)
                    isChecked = "F";
                else if (onlyIfDestinationEmpty && destinationTagValue != string.Empty)
                    isChecked = "F";
                else if (onlyIfSourceNotEmpty && sourceTagValue == string.Empty)
                    isChecked = "F";
                else
                    isChecked = "T";

                tag = new string[3];

                tag[0] = isChecked;
                tag[1] = currentFile;
                tag[2] = swappedTags.newDestinationTagValue;


                row = new string[9];

                row[0] = isChecked;
                row[1] = currentFile;
                row[2] = track;
                row[3] = swappedTags.newDestinationTagValue;
                row[4] = swappedTags.newDestinationTagTValue;
                row[5] = destinationTagValue;
                row[6] = swappedTags.destinationTagTValue;


                if (isChecked == "T")
                {
                    row[7] = row[3];
                    row[8] = row[4];
                }
                else if (isChecked == "F")
                {
                    row[7] = row[5];
                    row[8] = row[6];
                }

                Invoke(addRowToTable, new object[] { row });

                tags.Add(tag);

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
                if (backgroundTaskIsCanceled)
                    return;

                isChecked = tags[i][0];

                if (isChecked == "T")
                {
                    currentFile = tags[i][1];
                    newTag = tags[i][2];


                    tags[i][0] = string.Empty;

                    Invoke(processRowOfTable, new object[] { i });

                    SetStatusbarTextForFileOperations(CopyTagCommandSbText, false, i, tags.Count, currentFile);

                    SetFileTag(currentFile, destinationTagId, newTag);
                    CommitTagsToFile(currentFile);
                }
            }

            RefreshPanels(true);

            SetResultingSbText();
        }

        private void saveSettings()
        {
            saveWindowLayout(previewTable.Columns[1].Width, previewTable.Columns[2].Width, previewTable.Columns[3].Width);

            SavedSettings.copySourceTagName = sourceTagList.Text;
            SavedSettings.copyDestinationTagName = destinationTagList.Text;
            SavedSettings.onlyIfDestinationIsEmpty = onlyIfDestinationEmptyCheckBox.Checked;
            SavedSettings.onlyIfSourceNotEmpty = onlyIfSourceNotEmptyCheckBox.Checked;
            SavedSettings.smartOperation = smartOperationCheckBox.Checked;
            SavedSettings.appendSource = appendCheckBox.Checked;
            SavedSettings.addSource = addCheckBox.Checked;
            fileNameTextBox.Items.CopyTo(SavedSettings.customText, 0);
            appendedTextBox.Items.CopyTo(SavedSettings.appendedText, 0);
            addedTextBox.Items.CopyTo(SavedSettings.addedText, 0);

            TagToolsPlugin.SaveSettings();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (sourceTagList.Text == destinationTagList.Text)
            {
                MessageBox.Show(this, MsgSourceAndDestinationTagsAreTheSame, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            saveSettings();
            if (prepareBackgroundTask())
                switchOperation(applyChanges, (Button)sender, buttonOK, buttonPreview, buttonCancel, true, null);
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            if (sourceTagList.Text == destinationTagList.Text)
            {
                MessageBox.Show(this, MsgSourceAndDestinationTagsAreTheSame, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewChanges, (Button)sender, buttonOK, buttonCancel);
            enableQueryingOrUpdatingButtons();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void appendCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            appendedTextBox.Enable(appendCheckBox.Checked);

            if (appendCheckBox.Checked)
                addCheckBox.Checked = false;
        }

        private void appendCheckBoxLabel_Click(object sender, EventArgs e)
        {
            appendCheckBox.Checked = !appendCheckBox.Checked;
            appendCheckBox_CheckedChanged(null, null);
        }

        private void addCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            addedTextBox.Enable(addCheckBox.Checked);

            if (addCheckBox.Checked)
                appendCheckBox.Checked = false;
        }

        private void addCheckBoxLabel_Click(object sender, EventArgs e)
        {
            addCheckBox.Checked = !addCheckBox.Checked;
            addCheckBox_CheckedChanged(null, null);
        }

        private void sourceTagList_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool test = (sourceTagList.Text == TextFileTagName);
            label3.Enable(test);
            test = (sourceTagList.Text == TextFileTagName);
            fileNameTextBox.Enable(test);
            test = (sourceTagList.Text == TextFileTagName);
            browseButton.Enable(test);
        }

        private void destinationTagList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (destinationTagList.Text == MbApiInterface.Setting_GetFieldName(MetaDataType.HasLyrics))
            {
                sourceTagList.Items.Clear();
                sourceTagList.Items.Add("<Null>");
                sourceTagList.Items.Add("<+>");

                sourceTagList.SelectedIndex = 0;
            }
            else
            {
                int selectedIndex = sourceTagList.SelectedIndex;

                sourceTagList.Items.Clear();

                FillListByTagNames(sourceTagList.Items, true);
                FillListByPropNames(sourceTagList.Items);
                //sourceTagList.Items.Add(Plugin.emptyValueTagName);
                sourceTagList.Items.Add(ClipboardTagName);
                sourceTagList.Items.Add(TextFileTagName);
                sourceTagList.Items.Add(SequenceNumberName);

                try
                {
                    sourceTagList.SelectedIndex = selectedIndex;
                }
                catch
                {
                    sourceTagList.SelectedIndex = 0;
                }
            }
        }

        private void customTextBox_EnabledChanged(object sender, EventArgs e)
        {
            label3.Enable(fileNameTextBox.Enabled);
        }

        private void cbHeader_OnCheckBoxClicked(bool state)
        {
            foreach (DataGridViewRow row in previewTable.Rows)
            {
                if ((string)row.Cells[0].Value == null)
                    continue;

                if (state)
                    row.Cells[0].Value = "F";
                else
                    row.Cells[0].Value = "T";

                DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(0, row.Index);
                previewTable_CellContentClick(null, e);
            }
        }

        private void previewTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string newTagValue;
                string newTagTValue;

                string isChecked = (string)previewTable.Rows[e.RowIndex].Cells[0].Value;

                if (isChecked == "T")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "F";

                    newTagValue = (string)previewTable.Rows[e.RowIndex].Cells[5].Value;
                    newTagTValue = (string)previewTable.Rows[e.RowIndex].Cells[6].Value;

                    previewTable.Rows[e.RowIndex].Cells[7].Value = newTagValue;
                    previewTable.Rows[e.RowIndex].Cells[8].Value = newTagTValue;
                }
                else if (isChecked == "F")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "T";

                    newTagValue = (string)previewTable.Rows[e.RowIndex].Cells[3].Value;
                    newTagTValue = (string)previewTable.Rows[e.RowIndex].Cells[4].Value;

                    previewTable.Rows[e.RowIndex].Cells[7].Value = newTagValue;
                    previewTable.Rows[e.RowIndex].Cells[8].Value = newTagTValue;
                }

                previewTableFormatRow(e.RowIndex);
            }
        }

        public override void enableDisablePreviewOptionControls(bool enable, bool dontChangeDisabled = false)
        {
            if (enable && previewIsGenerated)
                return;

            sourceTagList.Enable(enable);
            destinationTagList.Enable(enable);
            label3.Enable(enable && sourceTagList.Text == TextFileTagName);
            fileNameTextBox.Enable(enable && sourceTagList.Text == TextFileTagName);
            browseButton.Enable(enable && sourceTagList.Text == TextFileTagName);
            smartOperationCheckBox.Enable(enable);
            onlyIfDestinationEmptyCheckBox.Enable(enable);
            onlyIfSourceNotEmptyCheckBox.Enable(enable);
            addCheckBox.Enable(enable);
            addedTextBox.Enable(enable && addCheckBox.Checked);
            appendCheckBox.Enable(enable);
            appendedTextBox.Enable(enable && appendCheckBox.Checked);
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

        private void filenameTextBox_Leave(object sender, EventArgs e)
        {
            ComboBoxLeave(fileNameTextBox);
        }

        private void appendedTextBox_Leave(object sender, EventArgs e)
        {
            ComboBoxLeave(appendedTextBox);
        }

        private void addedTextBox_Leave(object sender, EventArgs e)
        {
            ComboBoxLeave(addedTextBox);
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
                if (columnIndex == 8)
                {
                    if ((string)previewTable.Rows[rowIndex].Cells[6].Value == (string)previewTable.Rows[rowIndex].Cells[8].Value)
                        previewTable.Rows[rowIndex].Cells[8].Style = UnchangedCellStyle;
                    else
                        previewTable.Rows[rowIndex].Cells[8].Style = ChangedCellStyle;
                }
                else
                {
                    previewTable.Rows[rowIndex].Cells[columnIndex].Style = UnchangedCellStyle;
                }
            }
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Text files|*.txt",
                FilterIndex = 0
            };
            if (dialog.ShowDialog(this) == DialogResult.Cancel) return;

            fileNameTextBox.Text = dialog.FileName;
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            PluginQuickSettings settings = new PluginQuickSettings(TagToolsPlugin);
            Display(settings, true);
        }

        private void onlyIfDestinationEmptyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (onlyIfDestinationEmptyCheckBox.Checked)
                onlyIfSourceNotEmptyCheckBox.Checked = false;
        }
        private void onlyIfDestinationEmptyCheckBoxLabel_Click(object sender, EventArgs e)
        {
            onlyIfDestinationEmptyCheckBox.Checked = !onlyIfDestinationEmptyCheckBox.Checked;
            onlyIfDestinationEmptyCheckBox_CheckedChanged(null, null);
        }


        private void onlyIfSourceNotEmptyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (onlyIfSourceNotEmptyCheckBox.Checked)
                onlyIfDestinationEmptyCheckBox.Checked = false;
        }

        private void onlyIfSourceNotEmptyCheckBoxLabel_Click(object sender, EventArgs e)
        {
            onlyIfSourceNotEmptyCheckBox.Checked = !onlyIfSourceNotEmptyCheckBox.Checked;
        }

        private void smartOperationCheckBoxLabel_Click(object sender, EventArgs e)
        {
            smartOperationCheckBox.Checked = !smartOperationCheckBox.Checked;
            onlyIfSourceNotEmptyCheckBox_CheckedChanged(null, null);
        }

        private void CopyTagCommand_Load(object sender, EventArgs e)
        {
            (int, int, int, int, int, int, int) value = loadWindowLayout();

            if (value.Item1 != 0)
            {
                previewTable.Columns[2].Width = (int)(value.Item1 * dpiScaleFactor);
                previewTable.Columns[6].Width = (int)(value.Item2 * dpiScaleFactor);
                previewTable.Columns[8].Width = (int)(value.Item3 * dpiScaleFactor);
            }
            else
            {
                previewTable.Columns[2].Width = (int)(previewTable.Columns[2].Width * dpiScaleFactor);
                previewTable.Columns[6].Width = (int)(previewTable.Columns[6].Width * dpiScaleFactor);
                previewTable.Columns[8].Width = (int)(previewTable.Columns[8].Width * dpiScaleFactor);
            }
        }

        private void CopyTagCommand_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveWindowLayout(previewTable.Columns[2].Width, previewTable.Columns[6].Width, previewTable.Columns[8].Width);
        }
    }
}
