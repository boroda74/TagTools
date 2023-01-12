using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;


namespace MusicBeePlugin
{
    public partial class CopyTagCommand : PluginWindowTemplate
    {
        private delegate void AddRowToTable(string[] row);
        private delegate void ProcessRowOfTable(int row);
        private AddRowToTable addRowToTable;
        private ProcessRowOfTable processRowOfTable;

        private Plugin.MetaDataType sourceTagId;
        private Plugin.FilePropertyType sourcePropId;
        private Plugin.MetaDataType destinationTagId;
        private bool sourceTagIsSN;
        private bool sourceTagIsClipboard;
        private bool sourceTagIsTextFile;
        private bool onlyIfDestinationEmpty;
        private bool smartOperation;
        private bool appendSource;
        private bool addSource;
        private string customText;
        private string appendedText;
        private string addedText;

        private string[] files = new string[0];
        private List<string[]> tags = new List<string[]>();
        private string[] fileTags;

        public CopyTagCommand()
        {
            InitializeComponent();
        }

        public CopyTagCommand(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            Plugin.FillList(destinationTagList.Items);
            destinationTagList.Text = Plugin.SavedSettings.copyDestinationTagName;

            sourceTagList.Text = Plugin.SavedSettings.copySourceTagName;

            onlyIfDestinationEmptyCheckBox.Checked = Plugin.SavedSettings.onlyIfDestinationIsEmpty;
            smartOperationCheckBox.Checked = Plugin.SavedSettings.smartOperation;
            appendCheckBox.Checked = Plugin.SavedSettings.appendSource;
            addCheckBox.Checked = Plugin.SavedSettings.addSource;

            fileNameTextBox.Text = Plugin.SavedSettings.customText[0];
            fileNameTextBox.Items.AddRange(Plugin.SavedSettings.customText);
            appendedTextBox.Text = Plugin.SavedSettings.appendedText[0];
            appendedTextBox.Items.AddRange(Plugin.SavedSettings.appendedText);
            addedTextBox.Text = Plugin.SavedSettings.addedText[0];
            addedTextBox.Items.AddRange(Plugin.SavedSettings.addedText);

            appendedTextBox.Enabled = appendCheckBox.Checked;

            DatagridViewCheckBoxHeaderCell cbHeader = new DatagridViewCheckBoxHeaderCell();
            cbHeader.setState(true);
            cbHeader.OnCheckBoxClicked += new CheckBoxClickedHandler(cbHeader_OnCheckBoxClicked);

            DataGridViewCheckBoxColumn colCB = new DataGridViewCheckBoxColumn();
            colCB.HeaderCell = cbHeader;
            colCB.ThreeState = true;
            colCB.FalseValue = "False";
            colCB.TrueValue = "True";
            colCB.IndeterminateValue = "";
            colCB.Width = 25;
            colCB.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            previewTable.Columns.Insert(0, colCB);

            previewTable.Columns[2].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[6].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[8].HeaderCell.Style.WrapMode = DataGridViewTriState.True;

            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            addRowToTable = previewList_AddRowToTable;
            processRowOfTable = previewList_ProcessRowOfTable;

            int column1Width;
            int column2Width;
            int column3Width;

            int table2column1Width;
            int table2column2Width;
            int table2column3Width;

            loadWindowSizesPositions(true, out column1Width, out column2Width, out column3Width, out table2column1Width, out table2column2Width, out table2column3Width);

            if (column1Width > 0)
            {
                previewTable.Columns[1].Width = column1Width;
                previewTable.Columns[2].Width = column2Width;
                previewTable.Columns[3].Width = column3Width;
            }
        }

        private void previewList_AddRowToTable(string[] row)
        {
            if (backgroundTaskIsWorking())
                previewTable.Rows.Add(row);
        }

        private void previewList_ProcessRowOfTable(int row)
        {
            previewTable.Rows[row].Cells[0].Value = "";
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
                MessageBox.Show(this, Plugin.MsgFileNotFound, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            sourceTagId = Plugin.GetTagId(sourceTagList.Text);
            sourcePropId = Plugin.GetPropId(sourceTagList.Text);
            destinationTagId = Plugin.GetTagId(destinationTagList.Text);
            sourceTagIsSN = (sourceTagList.Text == Plugin.SequenceNumberName);
            sourceTagIsClipboard = (sourceTagList.Text == Plugin.ClipboardTagName);
            sourceTagIsTextFile = fileNameTextBox.Enabled;
            smartOperation = smartOperationCheckBox.Checked;
            onlyIfDestinationEmpty = onlyIfDestinationEmptyCheckBox.Checked;
            appendSource = appendCheckBox.Checked;
            addSource = addCheckBox.Checked;
            customText = fileNameTextBox.Text;
            appendedText = appendedTextBox.Text;
            addedText = addedTextBox.Text;


            files = null;
            if (!Plugin.MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files))
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
                    MessageBox.Show(this, Plugin.MsgNumberOfTagsInTextFile + fileTags.Length + Plugin.MsgDoesntCorrespondToNumberOfSelectedTracks + files.Length + Plugin.MsgMessageEnd, 
                        null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            else if (sourceTagIsClipboard)
            {
                if (!Clipboard.ContainsText())
                {
                    MessageBox.Show(this, Plugin.MsgClipboardDesntContainText, 
                        null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                fileTags = Clipboard.GetText().Split(new string[] { "\r\n" }, StringSplitOptions.None);

                if (fileTags.Length != files.Length)
                {
                    MessageBox.Show(this, Plugin.MsgNumberOfTagsInClipboard + fileTags.Length + Plugin.MsgDoesntCorrespondToNumberOfSelectedTracksC + files.Length + Plugin.MsgMessageEndC, 
                        null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            else
            {
                fileTags = null;
            }


            if (files.Length == 0)
            {
                MessageBox.Show(this, Plugin.MsgNoFilesSelected, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            Plugin.SwappedTags swappedTags;

            string sourceTagValue;
            string destinationTagValue;

            string isChecked;

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

                Plugin.SetStatusbarTextForFileOperations(Plugin.CopyTagCommandSbText, true, fileCounter, files.Length, currentFile);

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
                    sourceTagValue = Plugin.GetFileTag(currentFile, sourceTagId);
                }
                else
                {
                    sourceTagValue = Plugin.MbApiInterface.Library_GetFileProperty(currentFile, sourcePropId);
                }

                destinationTagValue = Plugin.GetFileTag(currentFile, destinationTagId);

                track = Plugin.GetTrackRepresentation(currentFile);

                swappedTags = Plugin.SwapTags(sourceTagValue, destinationTagValue, sourceTagId, destinationTagId,
                    smartOperation, appendSource, appendedText, addSource, addedText);

                if (sourceTagValue == destinationTagValue)
                    isChecked = "False";
                else if (onlyIfDestinationEmpty && destinationTagValue != "")
                    isChecked = "False";
                else
                    isChecked = "True";

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


                if (isChecked == "True")
                {
                    row[7] = row[3];
                    row[8] = row[4];
                }
                else if (isChecked == "False")
                {
                    row[7] = row[5];
                    row[8] = row[6];
                }

                Invoke(addRowToTable, new Object[] { row });

                tags.Add(tag);

                previewIsGenerated = true;
            }

            Plugin.SetStatusbarTextForFileOperations(Plugin.CopyTagCommandSbText, true, files.Length - 1, files.Length, null, true);
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

                if (isChecked == "True")
                {
                    currentFile = tags[i][1];
                    newTag = tags[i][2];


                    tags[i][0] = "";

                    Invoke(processRowOfTable, new Object[] { i });

                    Plugin.SetStatusbarTextForFileOperations(Plugin.CopyTagCommandSbText, false, i, tags.Count, currentFile);

                    Plugin.SetFileTag(currentFile, destinationTagId, newTag);
                    Plugin.CommitTagsToFile(currentFile);
                }
            }

            Plugin.RefreshPanels(true);

            Plugin.SetStatusbarTextForFileOperations(Plugin.CopyTagCommandSbText, false, tags.Count - 1, tags.Count, null, true);
        }

        private void saveSettings()
        {
            saveWindowSizesPositions(previewTable.Columns[1].Width, previewTable.Columns[2].Width, previewTable.Columns[3].Width);

            Plugin.SavedSettings.copySourceTagName = sourceTagList.Text;
            Plugin.SavedSettings.copyDestinationTagName = destinationTagList.Text;
            Plugin.SavedSettings.onlyIfDestinationIsEmpty = onlyIfDestinationEmptyCheckBox.Checked;
            Plugin.SavedSettings.smartOperation = smartOperationCheckBox.Checked;
            Plugin.SavedSettings.appendSource = appendCheckBox.Checked;
            Plugin.SavedSettings.addSource = addCheckBox.Checked;
            fileNameTextBox.Items.CopyTo(Plugin.SavedSettings.customText, 0);
            appendedTextBox.Items.CopyTo(Plugin.SavedSettings.appendedText, 0);
            addedTextBox.Items.CopyTo(Plugin.SavedSettings.addedText, 0);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (sourceTagList.Text == destinationTagList.Text)
            {
                MessageBox.Show(this, Plugin.MsgSourceAndDestinationTagsAreTheSame, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                MessageBox.Show(this, Plugin.MsgSourceAndDestinationTagsAreTheSame, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewChanges, (Button)sender, buttonOK, buttonCancel);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void appendCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            appendedTextBox.Enabled = appendCheckBox.Checked;

            if (appendCheckBox.Checked)
                addCheckBox.Checked = false;
        }

        private void addCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            addedTextBox.Enabled = addCheckBox.Checked;

            if (addCheckBox.Checked)
                appendCheckBox.Checked = false;
        }

        private void sourceTagList_SelectedIndexChanged(object sender, EventArgs e)
        {
            label3.Enabled = (sourceTagList.Text == Plugin.TextFileTagName);
            fileNameTextBox.Enabled = (sourceTagList.Text == Plugin.TextFileTagName);
            browseButton.Enabled = (sourceTagList.Text == Plugin.TextFileTagName);
        }

        private void destinationTagList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (destinationTagList.Text == Plugin.MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.HasLyrics))
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

                Plugin.FillList(sourceTagList.Items, true);
                Plugin.FillListWithProps(sourceTagList.Items);
                //sourceTagList.Items.Add(Plugin.emptyValueTagName);
                sourceTagList.Items.Add(Plugin.ClipboardTagName);
                sourceTagList.Items.Add(Plugin.TextFileTagName);
                sourceTagList.Items.Add(Plugin.SequenceNumberName);

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
            label3.Enabled = fileNameTextBox.Enabled;
        }

        private void cbHeader_OnCheckBoxClicked(bool state)
        {
            foreach (DataGridViewRow row in previewTable.Rows)
            {
                if ((string)row.Cells[0].Value == "")
                    continue;

                if (state)
                    row.Cells[0].Value = "False";
                else
                    row.Cells[0].Value = "True";

                DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(0, row.Index);
                previewList_CellContentClick(null, e);
            }
        }

        private void previewList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string newTagValue;
                string newTagTValue;

                string isChecked = (string)previewTable.Rows[e.RowIndex].Cells[0].Value;

                if (isChecked == "True")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "False";

                    newTagValue = (string)previewTable.Rows[e.RowIndex].Cells[5].Value;
                    newTagTValue = (string)previewTable.Rows[e.RowIndex].Cells[6].Value;

                    previewTable.Rows[e.RowIndex].Cells[7].Value = newTagValue;
                    previewTable.Rows[e.RowIndex].Cells[8].Value = newTagTValue;
                }
                else if (isChecked == "False")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "True";

                    newTagValue = (string)previewTable.Rows[e.RowIndex].Cells[3].Value;
                    newTagTValue = (string)previewTable.Rows[e.RowIndex].Cells[4].Value;

                    previewTable.Rows[e.RowIndex].Cells[7].Value = newTagValue;
                    previewTable.Rows[e.RowIndex].Cells[8].Value = newTagTValue;
                }
            }
        }

        public override void enableDisablePreviewOptionControls(bool enable)
        {
            if (enable && previewIsGenerated)
                return;

            sourceTagList.Enabled = enable;
            destinationTagList.Enabled = enable;
            label3.Enabled = enable && sourceTagList.Text == Plugin.TextFileTagName;
            fileNameTextBox.Enabled = enable && sourceTagList.Text == Plugin.TextFileTagName;
            browseButton.Enabled = enable && sourceTagList.Text == Plugin.TextFileTagName;
            smartOperationCheckBox.Enabled = enable;
            onlyIfDestinationEmptyCheckBox.Enabled = enable;
            addCheckBox.Enabled = enable;
            addedTextBox.Enabled = enable && addCheckBox.Checked;
            appendCheckBox.Enabled = enable;
            appendedTextBox.Enabled = enable && appendCheckBox.Checked;
        }

        public override void enableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonPreview, " ");
            dirtyErrorProvider.SetError(buttonPreview, String.Empty);
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

        private void filenameTextBox_Leave(object sender, EventArgs e)
        {
            Plugin.ComboBoxLeave(fileNameTextBox);
        }

        private void appendedTextBox_Leave(object sender, EventArgs e)
        {
            Plugin.ComboBoxLeave(appendedTextBox);
        }

        private void addedTextBox_Leave(object sender, EventArgs e)
        {
            Plugin.ComboBoxLeave(addedTextBox);
        }

        private void previewTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((string)previewTable.Rows[e.RowIndex].Cells[6].Value == (string)previewTable.Rows[e.RowIndex].Cells[8].Value)
                previewTable.Rows[e.RowIndex].Cells[8].Style = Plugin.UnchangedStyle;
            else
                previewTable.Rows[e.RowIndex].Cells[8].Style = Plugin.ChangedStyle;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text files|*.txt";
            dialog.FilterIndex = 0;
            if (dialog.ShowDialog() == DialogResult.Cancel) return;

            fileNameTextBox.Text = dialog.FileName;
        }
    }
}
