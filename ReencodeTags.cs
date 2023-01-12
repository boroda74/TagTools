using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class ReencodeTagsCommand : PluginWindowTemplate
    {
        private delegate void AddRowToTable(string[] row);
        private delegate void ProcessRowOfTable(int row);
        private AddRowToTable addRowToTable;
        private ProcessRowOfTable processRowOfTable;

        private string[] files = new string[0];
        private List<string[]> currentTracks = new List<string[]>();
        private List<string[]> newTracks = new List<string[]>();
        private List<string> cuesheetTracks = new List<string>();

        private Encoding defaultEncoding;
        private Encoding originalEncoding;
        private Encoding usedEncoding;

        private bool previewSortTags;

        public ReencodeTagsCommand()
        {
            InitializeComponent();
        }

        public ReencodeTagsCommand(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            defaultEncoding = Encoding.Default;
            EncodingInfo[] encodings = Encoding.GetEncodings();

            for (int i = 1; i < encodings.Length; i++)
            {
                initialEncodingsList.Items.Add(encodings[i].Name);
                usedEncodingsList.Items.Add(encodings[i].Name);
            }

            initialEncodingsList.Text = Plugin.SavedSettings.initialEncodingName;
            //usedEncodingsList.Text = Plugin.savedSettings.usedEncodingName;

            if (initialEncodingsList.Text == "")
                initialEncodingsList.Text = defaultEncoding.WebName;
            if (usedEncodingsList.Text == "")
                usedEncodingsList.Text = defaultEncoding.WebName;

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
            previewTable.Columns[3].HeaderCell.Style.WrapMode = DataGridViewTriState.True;

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
            previewTable.Rows.Add(row);
        }

        private void previewList_ProcessRowOfTable(int row)
        {
            previewTable.Rows[row].Cells[0].Value = "";
        }

        private string reencodeTag(string source)
        {
            try
            {
                byte[] usedBytes = usedEncoding.GetBytes(source);
                return originalEncoding.GetString(usedBytes);
            }
            catch
            {
                return Plugin.TableCellError;
            }
        }

        private bool prepareBackgroundPreview()
        {
            currentTracks.Clear();
            newTracks.Clear();
            previewTable.Rows.Clear();
            previewIsGenerated = false;
            ((DatagridViewCheckBoxHeaderCell)previewTable.Columns[0].HeaderCell).setState(true);

            if (backgroundTaskIsWorking())
                return true;

            originalEncoding = Encoding.GetEncoding(initialEncodingsList.Text);
            usedEncoding = Encoding.GetEncoding(usedEncodingsList.Text);

            previewSortTags = previewSortTagsСheckBox.Checked;

            files = null;
            if (!Plugin.MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files))
                files = new string[0];

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
                for (int fileCounter = 0; fileCounter < previewTable.Rows.Count; fileCounter++)
                {
                    if ((string)previewTable.Rows[fileCounter].Cells[0].Value == "True")
                    {
                        currentTracks[fileCounter][0] = "True";
                    }
                    else
                    {
                        currentTracks[fileCounter][0] = "False";
                    }
                }

                return true;
            }
        }

        private void previewChanges()
        {
            string currentFile;

            string[] row = { "Checked", "File", "OriginalTrack", "NewTrack" };

            int numberOfWritableTags = Plugin.TagIdsNames.Count - Plugin.ReadonlyTagsNames.Length - 1;

            bool wasCuesheet = false;
            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                currentFile = files[fileCounter];

                Plugin.SetStatusbarTextForFileOperations(Plugin.ReencodeTagCommandSbText, true, fileCounter, files.Length, currentFile);

                string[] currentTags = new string[numberOfWritableTags + 2];
                currentTags[0] = "True";
                currentTags[1] = currentFile;

                string[] newTags = new string[numberOfWritableTags + 2];
                newTags[0] = "True";
                newTags[1] = currentFile;

                if (Plugin.GetFileTag(currentFile, Plugin.MetaDataType.Cuesheet) != "")
                    wasCuesheet = true;

                string[] tagNames = new string[numberOfWritableTags + 2];
                int j = 0;
                foreach (var tagIdName in Plugin.TagIdsNames)
                {
                    for (int i = 0; i < Plugin.ReadonlyTagsNames.Length; i++)
                        if (Plugin.ReadonlyTagsNames[i] == tagIdName.Value || Plugin.MetaDataType.Artwork == tagIdName.Key)
                            goto loopEnd;

                    currentTags[j + 2] = Plugin.GetFileTag(currentFile, tagIdName.Key);
                    newTags[j + 2] = reencodeTag(currentTags[j + 2]);

                    tagNames[j + 2] = tagIdName.Value;

                    j++;

                loopEnd:;
                }


                row = new string[4];

                row[0] = "True";
                row[1] = currentFile;
                row[2] = Plugin.GetTrackRepresentation(currentTags, newTags, tagNames, previewSortTags);
                row[3] = Plugin.GetTrackRepresentation(newTags, currentTags, tagNames, previewSortTags);

                Invoke(addRowToTable, new Object[] { row });

                currentTracks.Add(currentTags);
                newTracks.Add(newTags);

                previewIsGenerated = true;
            }

            if (wasCuesheet)
            {
                Plugin.LastCommandSbText = "<CUESHEET>";
            }
            else
            {
                Plugin.SetStatusbarTextForFileOperations(Plugin.ReencodeTagCommandSbText, true, files.Length - 1, files.Length, null, true);
            }
        }

        private void applyChanges()
        {
            string currentFile;
            string isChecked;

            if (newTracks.Count == 0)
                previewChanges();

            cuesheetTracks.Clear();

            for (int i = 0; i < currentTracks.Count; i++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                isChecked = newTracks[i][0];

                if (isChecked == "True")
                {
                    currentFile = newTracks[i][1];

                    currentTracks[i][0] = "";

                    Invoke(processRowOfTable, new Object[] { i });

                    Plugin.SetStatusbarTextForFileOperations(Plugin.ReencodeTagCommandSbText, false, i, newTracks.Count, currentFile);

                    string cuesheet = Plugin.GetFileTag(currentFile, Plugin.MetaDataType.Cuesheet);
                    if (cuesheet == "")
                    {
                        int j = 0;
                        foreach (var tagIdName in Plugin.TagIdsNames)
                        {
                            for (int k = 0; k < Plugin.ReadonlyTagsNames.Length; k++)
                                if (Plugin.ReadonlyTagsNames[k] == tagIdName.Value)
                                    goto loopEnd;

                            if (tagIdName.Key == Plugin.MetaDataType.Artwork)
                                goto loopEnd;

                            Plugin.SetFileTag(currentFile, tagIdName.Key, newTracks[i][j + 2]);

                            j++;

                        loopEnd:;
                        }

                        Plugin.CommitTagsToFile(currentFile);
                    }
                    else if (!cuesheetTracks.Contains(currentFile))
                    {
                        Plugin.SetFileTag(currentFile, Plugin.MetaDataType.Cuesheet, reencodeTag(cuesheet));

                        Plugin.CommitTagsToFile(currentFile);

                        cuesheetTracks.Add(currentFile);
                    }
                }
            }

            Plugin.RefreshPanels(true);

            Plugin.SetStatusbarTextForFileOperations(Plugin.ReencodeTagCommandSbText, false, newTracks.Count - 1, newTracks.Count, null, true);
        }

        private void saveSettings()
        {
            saveWindowSizesPositions(previewTable.Columns[1].Width, previewTable.Columns[2].Width, previewTable.Columns[3].Width);

            Plugin.SavedSettings.initialEncodingName = initialEncodingsList.Text;
            //Plugin.savedSettings.usedEncodingName = usedEncodingsList.Text;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            saveSettings();
            if (prepareBackgroundTask())
                switchOperation(applyChanges, (Button)sender, buttonOK, buttonPreview, buttonCancel, true, null);
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewChanges, (Button)sender, buttonOK, buttonCancel);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
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
                string sourceTagValue;
                string newTagValue;

                string isChecked = (string)previewTable.Rows[e.RowIndex].Cells[0].Value;

                if (isChecked == "True")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "False";

                    sourceTagValue = (string)previewTable.Rows[e.RowIndex].Cells[2].Value;

                    previewTable.Rows[e.RowIndex].Cells[3].Value = sourceTagValue;
                }
                else if (isChecked == "False")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "True";

                    sourceTagValue = (string)previewTable.Rows[e.RowIndex].Cells[2].Value;

                    newTagValue = reencodeTag(sourceTagValue);

                    previewTable.Rows[e.RowIndex].Cells[3].Value = newTagValue;
                }
            }
        }

        public override void enableDisablePreviewOptionControls(bool enable)
        {
            if (enable && previewIsGenerated)
                return;

            initialEncodingsList.Enabled = enable;
            usedEncodingsList.Enabled = enable;
        }

        public override void enableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonPreview, String.Empty);
        }

        public override void disableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonPreview, " ");
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

        private void previewTable_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
            {
                try
                {
                    MessageBox.Show(this, (string)previewTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                }
                catch { };
            }
        }
    }
}
