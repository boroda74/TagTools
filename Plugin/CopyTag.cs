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


        private DataGridViewCellStyle unchangedCellStyle = new DataGridViewCellStyle(UnchangedCellStyle);
        private DataGridViewCellStyle changedCellStyle = new DataGridViewCellStyle(ChangedCellStyle);
        private DataGridViewCellStyle dimmedCellStyle = new DataGridViewCellStyle(DimmedCellStyle);

        private delegate void AddRowToTable(string[] row);
        private delegate void ProcessRowOfTable(int row);
        private AddRowToTable addRowToTable;
        private ProcessRowOfTable processRowOfTable;

        private delegate void UpdateCustomScrollBarsDelegate(DataGridView dataGridView);
        private UpdateCustomScrollBarsDelegate updateCustomScrollBars;

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

        internal CopyTag(Plugin tagToolsPluginParam) : base(tagToolsPluginParam)
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

            fileNameTextBoxCustom.Text = SavedSettings.customText[0];
            fileNameTextBoxCustom.AddRange(SavedSettings.customText);
            appendedTextBoxCustom.Text = SavedSettings.appendedText[0];
            appendedTextBoxCustom.AddRange(SavedSettings.appendedText);
            addedTextBoxCustom.Text = SavedSettings.addedText[0];
            addedTextBoxCustom.AddRange(SavedSettings.addedText);

            appendedTextBoxCustom.Enable(appendCheckBox.Checked);


            var headerCellStyle = new DataGridViewCellStyle(HeaderCellStyle);

            DataGridViewCheckBoxHeaderCell cbHeader = new DataGridViewCheckBoxHeaderCell();
            cbHeader.Style = headerCellStyle;
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

            previewTable.Columns[2].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[6].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[8].HeaderCell.Style = headerCellStyle;

            previewTable.Columns[2].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[6].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[8].HeaderCell.Style.WrapMode = DataGridViewTriState.True;

            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            addRowToTable = previewTable_AddRowToTable;
            processRowOfTable = previewTable_ProcessRowOfTable;

            updateCustomScrollBars = UpdateCustomScrollBars;


            enableDisablePreviewOptionControls(true, true);
            enableQueryingOrUpdatingButtons();


            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        private void previewTable_AddRowToTable(string[] row)
        {
            if (backgroundTaskIsWorking())
                previewTable.Rows.Add(row);

            previewTableFormatRow(previewTable.RowCount - 1);

            if (previewTable.RowCount % 16 == 0)
                UpdateCustomScrollBars(previewTable);
        }

        private void previewTable_ProcessRowOfTable(int rowIndex)
        {
            previewTable.Rows[rowIndex].Cells[0].Value = null;
            previewTableFormatRow(rowIndex);
        }

        internal bool prepareBackgroundPreview()
        {
            tags.Clear();
            previewTable.RowCount = 0;
            (previewTable.Columns[0].HeaderCell as DataGridViewCheckBoxHeaderCell).setState(true);

            UpdateCustomScrollBars(previewTable);

            if (previewIsGenerated)
            {
                previewIsGenerated = false;
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
            customText = fileNameTextBoxCustom.Text;
            appendedText = appendedTextBoxCustom.Text;
            addedText = addedTextBoxCustom.Text;


            files = null;
            if (!MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files))
                files = new string[0];


            if (sourceTagIsTextFile)
            {
                System.IO.Stream stream;
                stream = new System.IO.FileStream(fileNameTextBoxCustom.Text, System.IO.FileMode.Open);

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

                SetStatusbarTextForFileOperations(CopyTagSbText, true, fileCounter, files.Length, currentFile);

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
                else //if (isChecked == "F")
                {
                    row[7] = row[5];
                    row[8] = row[6];
                }

                Invoke(addRowToTable, new object[] { row });

                tags.Add(tag);

                previewIsGenerated = true;
            }

            Invoke(updateCustomScrollBars, previewTable);
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

                    SetStatusbarTextForFileOperations(CopyTagSbText, false, i, tags.Count, currentFile);

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
                switchOperation(applyChanges, (Button)sender, buttonOK, buttonPreview, buttonCancel, true, null);
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            if (!previewIsGenerated && sourceTagListCustom.Text == destinationTagListCustom.Text)
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
            appendedTextBoxCustom.Enable(appendCheckBox.Checked);

            if (appendCheckBox.Checked)
                addCheckBox.Checked = false;
        }

        private void appendCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!appendCheckBox.IsEnabled())
                return;

            appendCheckBox.Checked = !appendCheckBox.Checked;
            appendCheckBox_CheckedChanged(null, null);
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
            addCheckBox_CheckedChanged(null, null);
        }

        private void sourceTagList_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool test = (sourceTagListCustom.Text == TextFileTagName);
            fileNameLabel.Enable(test);
            test = (sourceTagListCustom.Text == TextFileTagName);
            fileNameTextBoxCustom.Enable(test);
            test = (sourceTagListCustom.Text == TextFileTagName);
            browseButton.Enable(test);
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
                int selectedIndex = sourceTagListCustom.SelectedIndex;

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

        internal override void enableDisablePreviewOptionControls(bool enable, bool dontChangeDisabled = false)
        {
            if (enable && previewIsGenerated)
                return;

            sourceTagListCustom.Enable(enable);
            destinationTagListCustom.Enable(enable);
            fileNameLabel.Enable(enable && sourceTagListCustom.Text == TextFileTagName);
            fileNameTextBoxCustom.Enable(enable && sourceTagListCustom.Text == TextFileTagName);
            browseButton.Enable(enable && sourceTagListCustom.Text == TextFileTagName);
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
            buttonOK.Enable(previewIsGenerated || SavedSettings.allowCommandExecutionWithoutPreview);
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

        private void previewTableFormatRow(int rowIndex)
        {
            if (SavedSettings.dontHighlightChangedTags)
                return;

            if ((string)previewTable.Rows[rowIndex].Cells[0].Value != "T")
            {
                for (int columnIndex = 1; columnIndex < previewTable.ColumnCount; columnIndex++)
                {
                    if (previewTable.Columns[columnIndex].Visible)
                        previewTable.Rows[rowIndex].Cells[columnIndex].Style = dimmedCellStyle;
                }

                return;
            }


            for (int columnIndex = 1; columnIndex < previewTable.ColumnCount; columnIndex++)
            {
                if (columnIndex == 8)
                {
                    if ((string)previewTable.Rows[rowIndex].Cells[6].Value == (string)previewTable.Rows[rowIndex].Cells[8].Value)
                        previewTable.Rows[rowIndex].Cells[8].Style = unchangedCellStyle;
                    else
                        previewTable.Rows[rowIndex].Cells[8].Style = changedCellStyle;
                }
                else
                {
                    previewTable.Rows[rowIndex].Cells[columnIndex].Style = unchangedCellStyle;
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

            fileNameTextBoxCustom.Text = dialog.FileName;
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
            if (!onlyIfDestinationEmptyCheckBox.IsEnabled())
                return;

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
            if (!onlyIfSourceNotEmptyCheckBox.IsEnabled())
                return;

            onlyIfSourceNotEmptyCheckBox.Checked = !onlyIfSourceNotEmptyCheckBox.Checked;
        }

        private void smartOperationCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!smartOperationCheckBox.IsEnabled())
                return;

            smartOperationCheckBox.Checked = !smartOperationCheckBox.Checked;
            onlyIfSourceNotEmptyCheckBox_CheckedChanged(null, null);
        }

        private void CopyTag_Load(object sender, EventArgs e)
        {
            (int, int, int, int, int, int, int) value = loadWindowLayout();

            if (value.Item1 != 0)
            {
                previewTable.Columns[2].Width = (int)Math.Round(value.Item1 * hDpiFontScaling);
                previewTable.Columns[6].Width = (int)Math.Round(value.Item2 * hDpiFontScaling);
                previewTable.Columns[8].Width = (int)Math.Round(value.Item3 * hDpiFontScaling);
            }
            else
            {
                previewTable.Columns[2].Width = (int)Math.Round(previewTable.Columns[2].Width * hDpiFontScaling);
                previewTable.Columns[6].Width = (int)Math.Round(previewTable.Columns[6].Width * hDpiFontScaling);
                previewTable.Columns[8].Width = (int)Math.Round(previewTable.Columns[8].Width * hDpiFontScaling);
            }
        }

        private void CopyTag_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveWindowLayout(previewTable.Columns[2].Width, previewTable.Columns[6].Width, previewTable.Columns[8].Width);
        }
    }
}
