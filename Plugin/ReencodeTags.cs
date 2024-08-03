using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class ReEncodeTags : PluginWindowTemplate
    {
        private CustomComboBox initialEncodingListCustom;
        private CustomComboBox usedEncodingListCustom;


        private delegate void AddRowToTable(string[] row);
        private delegate void ProcessRowOfTable(int row);
        private AddRowToTable addRowToTable;
        private ProcessRowOfTable processRowOfTable;

        private string[] files = Array.Empty<string>();
        private readonly List<string[]> currentTracks = new List<string[]>();
        private readonly List<string[]> newTracks = new List<string[]>();
        private readonly List<string> cuesheetTracks = new List<string>();

        private Encoding defaultEncoding;
        private Encoding originalEncoding;
        private Encoding usedEncoding;

        private bool previewSortTags;

        internal ReEncodeTags(Plugin plugin) : base(plugin)
        {
            InitializeComponent();
        }

        protected override void initializeForm()
        {
            base.initializeForm();

            initialEncodingListCustom = namesComboBoxes["initialEncodingList"];
            usedEncodingListCustom = namesComboBoxes["usedEncodingList"];


            buttonSettings.Image = ReplaceBitmap(buttonSettings.Image, Gear);

            defaultEncoding = Encoding.Default;
            var encodings = Encoding.GetEncodings();

            for (var i = 1; i < encodings.Length; i++)
            {
                initialEncodingListCustom.Items.Add(encodings[i].Name);
                usedEncodingListCustom.Items.Add(encodings[i].Name);
            }

            initialEncodingListCustom.Text = SavedSettings.initialEncodingName;
            //usedEncodingListCustom.Text = SavedSettings.usedEncodingName;

            if (initialEncodingListCustom.Text == string.Empty)
                initialEncodingListCustom.Text = defaultEncoding.WebName;
            if (usedEncodingListCustom.Text == string.Empty)
                usedEncodingListCustom.Text = defaultEncoding.WebName;


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

            previewTable.Columns.Insert(0, colCB);

            previewTable.Columns[2].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[3].HeaderCell.Style = headerCellStyle;

            previewTable.Columns[2].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[3].HeaderCell.Style.WrapMode = DataGridViewTriState.True;

            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            addRowToTable = previewTable_AddRowToTable;
            processRowOfTable = previewTable_ProcessRowOfTable;


            enableDisablePreviewOptionControls(true, true);
            enableQueryingOrUpdatingButtons();

            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        private void previewTable_AddRowToTable(object[] row)
        {
            previewTable.Rows.Add(row);

            previewTable.FirstDisplayedScrollingRowIndex = previewTable.RowCount - 1;
            if ((previewTable.RowCount & 0x1f) == 0)
                updateCustomScrollBars(previewTable);
        }

        private void previewTable_ProcessRowOfTable(int row)
        {
            previewTable.Rows[row].Cells[0].Value = null;
        }

        private string reencodeTag(string source)
        {
            try
            {
                var usedBytes = usedEncoding.GetBytes(source);
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
            previewTable.RowCount = 0;
            (previewTable.Columns[0].HeaderCell as DataGridViewCheckBoxHeaderCell).setState(true);

            updateCustomScrollBars(previewTable);

            if (previewIsGenerated)
            {
                previewIsGenerated = false;
                return true;
            }

            previewIsGenerated = false;

            if (backgroundTaskIsWorking())
                return true;

            originalEncoding = Encoding.GetEncoding(initialEncodingListCustom.Text);
            usedEncoding = Encoding.GetEncoding(usedEncodingListCustom.Text);

            previewSortTags = previewSortTagsСheckBox.Checked;

            files = null;
            if (!MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files))
                files = Array.Empty<string>();

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
                for (var fileCounter = 0; fileCounter < previewTable.Rows.Count; fileCounter++)
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
            // ReSharper disable once RedundantAssignment
            string[] row = { "Checked", "File", "OriginalTrack", "NewTrack" };

            var numberOfWritableTags = TagIdsNames.Count - ReadonlyTagsNames.Length - 1;

            var wasCuesheet = false;
            for (var fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                var currentFile = files[fileCounter];

                SetStatusBarTextForFileOperations(ReEncodeTagSbText, true, fileCounter, files.Length, currentFile);

                var currentTags = new string[numberOfWritableTags + 2];
                currentTags[0] = "T";
                currentTags[1] = currentFile;

                var newTags = new string[numberOfWritableTags + 2];
                newTags[0] = "T";
                newTags[1] = currentFile;

                if (GetFileTag(currentFile, MetaDataType.Cuesheet) != string.Empty)
                    wasCuesheet = true;

                var tagNames = new string[numberOfWritableTags + 2];
                var j = 0;
                foreach (var tagIdName in TagIdsNames)
                {
                    for (var i = 0; i < ReadonlyTagsNames.Length; i++)
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

            Invoke(new Action(() => { updateCustomScrollBars(previewTable); }));

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
            if (newTracks.Count == 0)
                previewChanges();

            cuesheetTracks.Clear();

            for (var i = 0; i < currentTracks.Count; i++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                var isChecked = newTracks[i][0];

                if (isChecked == "T")
                {
                    var currentFile = newTracks[i][1];

                    currentTracks[i][0] = string.Empty;

                    // ReSharper disable once RedundantExplicitParamsArrayCreation
                    Invoke(processRowOfTable, new object[] { i });

                    SetStatusBarTextForFileOperations(ReEncodeTagSbText, false, i, newTracks.Count, currentFile);

                    var cuesheet = GetFileTag(currentFile, MetaDataType.Cuesheet);
                    if (cuesheet == string.Empty)
                    {
                        var j = 0;
                        foreach (var tagIdName in TagIdsNames)
                        {
                            for (var k = 0; k < ReadonlyTagsNames.Length; k++)
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

            SavedSettings.initialEncodingName = initialEncodingListCustom.Text;
            //SavedSettings.usedEncodingName = usedEncodingListCustom.Text;

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

            if (previewTable.RowCount > 0)
            {
                var vScrollBar = ControlsTools.FindControlChild<VScrollBar>(previewTable);
                vScrollBar.Value = vScrollBar.Maximum;
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
                string sourceTagValue;

                var isChecked = (string)previewTable.Rows[e.RowIndex].Cells[0].Value;

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

                    var newTagValue = reencodeTag(sourceTagValue);

                    previewTable.Rows[e.RowIndex].Cells[3].Value = newTagValue;
                }
            }
        }

        internal override void enableDisablePreviewOptionControls(bool enable, bool dontChangeDisabled = false)
        {
            if (enable && previewIsGenerated)
                return;

            initialEncodingListCustom.Enable(enable);
            usedEncodingListCustom.Enable(enable);
        }

        internal override void enableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonPreview, string.Empty);
        }

        internal override void disableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonPreview, " ");
            dirtyErrorProvider.SetError(buttonPreview, getBackgroundTasksWarning());
        }

        internal override void enableQueryingOrUpdatingButtons()
        {
            buttonOK.Enable((previewIsGenerated && !previewIsStopped) || SavedSettings.allowCommandExecutionWithoutPreview);
            buttonPreview.Enable(true);
        }

        internal override void disableQueryingOrUpdatingButtons()
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
                catch
                {
                    // ignored
                }
            }
        }

        private void ReEncodeTags_Load(object sender, EventArgs e)
        {
            var value = loadWindowLayout();

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

        private void ReEncodeTags_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveWindowLayout(previewTable.Columns[1].Width, previewTable.Columns[2].Width);
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            var settings = new QuickSettings(TagToolsPlugin);
            Display(settings, true);
        }
    }
}
