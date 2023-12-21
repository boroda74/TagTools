using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;
using ExtensionMethods;

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

        public ReencodeTagsCommand(Plugin tagToolsPluginParam) : base(tagToolsPluginParam)
        {
            InitializeComponent();
        }

        protected override void initializeForm()
        {
            base.initializeForm();

            buttonSettings.Image = ThemedBitmapAddRef(this, Gear);

            defaultEncoding = Encoding.Default;
            EncodingInfo[] encodings = Encoding.GetEncodings();

            for (int i = 1; i < encodings.Length; i++)
            {
                initialEncodingList.Items.Add(encodings[i].Name);
                usedEncodingList.Items.Add(encodings[i].Name);
            }

            initialEncodingList.Text = SavedSettings.initialEncodingName;
            //usedEncodingsList.Text = SavedSettings.usedEncodingName;

            if (initialEncodingList.Text == string.Empty)
                initialEncodingList.Text = defaultEncoding.WebName;
            if (usedEncodingList.Text == string.Empty)
                usedEncodingList.Text = defaultEncoding.WebName;


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

            previewTable.Columns[2].HeaderCell.Style = HeaderCellStyle;
            previewTable.Columns[3].HeaderCell.Style = HeaderCellStyle;

            previewTable.Columns[2].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[3].HeaderCell.Style.WrapMode = DataGridViewTriState.True;

            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            addRowToTable = previewTable_AddRowToTable;
            processRowOfTable = previewTable_ProcessRowOfTable;


            enableDisablePreviewOptionControls(true, true);
            enableQueryingOrUpdatingButtons();

            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        private void previewTable_AddRowToTable(string[] row)
        {
            previewTable.Rows.Add(row);
        }

        private void previewTable_ProcessRowOfTable(int row)
        {
            previewTable.Rows[row].Cells[0].Value = null;
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
                return TableCellError;
            }
        }

        private bool prepareBackgroundPreview()
        {
            currentTracks.Clear();
            newTracks.Clear();
            previewTable.Rows.Clear();
            previewIsGenerated = false;
            (previewTable.Columns[0].HeaderCell as DatagridViewCheckBoxHeaderCell).setState(true);

            if (backgroundTaskIsWorking())
                return true;

            originalEncoding = Encoding.GetEncoding(initialEncodingList.Text);
            usedEncoding = Encoding.GetEncoding(usedEncodingList.Text);

            previewSortTags = previewSortTagsСheckBox.Checked;

            files = null;
            if (!MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files))
                files = new string[0];

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
                for (int fileCounter = 0; fileCounter < previewTable.Rows.Count; fileCounter++)
                {
                    if ((string)previewTable.Rows[fileCounter].Cells[0].Value == "T")
                    {
                        currentTracks[fileCounter][0] = "T";
                    }
                    else
                    {
                        currentTracks[fileCounter][0] = "F";
                    }
                }

                return true;
            }
        }

        private void previewChanges()
        {
            string currentFile;

            string[] row = { "Checked", "File", "OriginalTrack", "NewTrack" };

            int numberOfWritableTags = TagIdsNames.Count - ReadonlyTagsNames.Length - 1;

            bool wasCuesheet = false;
            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                currentFile = files[fileCounter];

                SetStatusbarTextForFileOperations(ReencodeTagCommandSbText, true, fileCounter, files.Length, currentFile);

                string[] currentTags = new string[numberOfWritableTags + 2];
                currentTags[0] = "T";
                currentTags[1] = currentFile;

                string[] newTags = new string[numberOfWritableTags + 2];
                newTags[0] = "T";
                newTags[1] = currentFile;

                if (GetFileTag(currentFile, MetaDataType.Cuesheet) != string.Empty)
                    wasCuesheet = true;

                string[] tagNames = new string[numberOfWritableTags + 2];
                int j = 0;
                foreach (var tagIdName in TagIdsNames)
                {
                    for (int i = 0; i < ReadonlyTagsNames.Length; i++)
                        if (ReadonlyTagsNames[i] == tagIdName.Value || MetaDataType.Artwork == tagIdName.Key)
                            goto loopEnd;

                    currentTags[j + 2] = GetFileTag(currentFile, tagIdName.Key);
                    newTags[j + 2] = reencodeTag(currentTags[j + 2]);

                    tagNames[j + 2] = tagIdName.Value;

                    j++;

                loopEnd:;
                }


                row = new string[4];

                row[0] = "T";
                row[1] = currentFile;
                row[2] = GetTrackRepresentation(currentTags, newTags, tagNames, previewSortTags);
                row[3] = GetTrackRepresentation(newTags, currentTags, tagNames, previewSortTags);

                Invoke(addRowToTable, new object[] { row });

                currentTracks.Add(currentTags);
                newTracks.Add(newTags);

                previewIsGenerated = true;
            }

            if (wasCuesheet)
            {
                LastCommandSbText = "<CUESHEET>";
            }
            else
            {
                SetResultingSbText();
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

                if (isChecked == "T")
                {
                    currentFile = newTracks[i][1];

                    currentTracks[i][0] = string.Empty;

                    Invoke(processRowOfTable, new object[] { i });

                    SetStatusbarTextForFileOperations(ReencodeTagCommandSbText, false, i, newTracks.Count, currentFile);

                    string cuesheet = GetFileTag(currentFile, MetaDataType.Cuesheet);
                    if (cuesheet == string.Empty)
                    {
                        int j = 0;
                        foreach (var tagIdName in TagIdsNames)
                        {
                            for (int k = 0; k < ReadonlyTagsNames.Length; k++)
                                if (ReadonlyTagsNames[k] == tagIdName.Value)
                                    goto loopEnd;

                            if (tagIdName.Key == MetaDataType.Artwork)
                                goto loopEnd;

                            SetFileTag(currentFile, tagIdName.Key, newTracks[i][j + 2]);

                            j++;

                        loopEnd:;
                        }

                        CommitTagsToFile(currentFile);
                    }
                    else if (!cuesheetTracks.Contains(currentFile))
                    {
                        SetFileTag(currentFile, MetaDataType.Cuesheet, reencodeTag(cuesheet));

                        CommitTagsToFile(currentFile);

                        cuesheetTracks.Add(currentFile);
                    }
                }
            }

            RefreshPanels(true);

            SetResultingSbText();
        }

        private void saveSettings()
        {
            saveWindowLayout(previewTable.Columns[1].Width, previewTable.Columns[2].Width, previewTable.Columns[3].Width);

            SavedSettings.initialEncodingName = initialEncodingList.Text;
            //SavedSettings.usedEncodingName = usedEncodingsList.Text;

            TagToolsPlugin.SaveSettings();
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
            enableQueryingOrUpdatingButtons();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
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
                string sourceTagValue;
                string newTagValue;

                string isChecked = (string)previewTable.Rows[e.RowIndex].Cells[0].Value;

                if (isChecked == "T")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "F";

                    sourceTagValue = (string)previewTable.Rows[e.RowIndex].Cells[2].Value;

                    previewTable.Rows[e.RowIndex].Cells[3].Value = sourceTagValue;
                }
                else if (isChecked == "F")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "T";

                    sourceTagValue = (string)previewTable.Rows[e.RowIndex].Cells[2].Value;

                    newTagValue = reencodeTag(sourceTagValue);

                    previewTable.Rows[e.RowIndex].Cells[3].Value = newTagValue;
                }
            }
        }

        public override void enableDisablePreviewOptionControls(bool enable, bool dontChangeDisabled = false)
        {
            if (enable && previewIsGenerated)
                return;

            initialEncodingList.Enable(enable);
            usedEncodingList.Enable(enable);
        }

        public override void enableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonPreview, string.Empty);
        }

        public override void disableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonPreview, " ");
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

        private void ReencodeTagsCommand_Load(object sender, EventArgs e)
        {
            (int, int, int, int, int, int, int) value = loadWindowLayout();

            if (value.Item1 != 0)
            {
                previewTable.Columns[1].Width = (int)Math.Round(value.Item1 * hDpiFontScaling);
                previewTable.Columns[2].Width = (int)Math.Round(value.Item2 * hDpiFontScaling);
            }
            else
            {
                previewTable.Columns[1].Width = (int)Math.Round(previewTable.Columns[1].Width * hDpiFontScaling);
                previewTable.Columns[2].Width = (int)Math.Round(previewTable.Columns[2].Width * hDpiFontScaling);
            }
        }

        private void ReencodeTagsCommand_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveWindowLayout(previewTable.Columns[1].Width, previewTable.Columns[2].Width);
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            PluginQuickSettings settings = new PluginQuickSettings(TagToolsPlugin);
            Display(settings, true);
        }
    }
}
