using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using ExtensionMethods;

using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class ReEncodeTags : PluginWindowTemplate
    {
        public class Row
        {
            public string Checked { get; set; }
            public string File { get; set; }
            public string Track { get; set; }
            public string NewTrack { get; set; }
        }

        private CustomComboBox initialEncodingListCustom;
        private CustomComboBox usedEncodingListCustom;


        private List<Row> rows = new List<Row>();
        BindingSource source = new BindingSource();

        private string[] files = Array.Empty<string>();
        private readonly List<string[]> currentTracks = new List<string[]>();
        private readonly List<string[]> newTracks = new List<string[]>();
        private readonly List<string> cuesheetTracks = new List<string>();
        private List<bool> processedRowList = new List<bool>(); //Indices of processed tracks

        private Encoding defaultEncoding;
        private Encoding originalEncoding;
        private Encoding usedEncoding;

        private bool previewSortTags;

        public ReEncodeTags(Plugin plugin) : base(plugin)
        {
            InitializeComponent();
        }

        internal protected override void initializeForm()
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

            if (string.IsNullOrEmpty(initialEncodingListCustom.Text))
                initialEncodingListCustom.Text = defaultEncoding.WebName;
            if (string.IsNullOrEmpty(usedEncodingListCustom.Text))
                usedEncodingListCustom.Text = defaultEncoding.WebName;


            var headerCellStyle = new DataGridViewCellStyle(HeaderCellStyle);

            var cbHeader = new DataGridViewCheckBoxHeaderCell();
            cbHeader.Style = headerCellStyle;
            cbHeader.setState(true);

            var colCB = new DataGridViewCheckBoxColumn
            {
                HeaderCell = cbHeader,
                ThreeState = true,
                FalseValue = Plugin.ColumnUncheckedState,
                TrueValue = Plugin.ColumnCheckedState,
                IndeterminateValue = string.Empty,
                Width = 25,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Resizable = DataGridViewTriState.False,
                DataPropertyName = "Checked",
            };

            previewTable.Columns.Insert(0, colCB);

            previewTable.Columns[2].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[3].HeaderCell.Style = headerCellStyle;

            previewTable.Columns[2].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[3].HeaderCell.Style.WrapMode = DataGridViewTriState.True;

            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            source.DataSource = rows;
            previewTable.DataSource = source;

            (previewTable.Columns[0].HeaderCell as DataGridViewCheckBoxHeaderCell).OnCheckBoxClicked += cbHeader_OnCheckBoxClicked;


            enableDisablePreviewOptionControls(true, true);
            enableQueryingOrUpdatingButtons();

            button_GotFocus(AcceptButton, null); //Let's mark active button
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

        private void previewTable_ProcessRowsOfTable(List<bool> processedRowList)
        {
            for (int i = 0; i < processedRowList.Count; i++)
            {
                previewTable.CurrentCell = previewTable.Rows[i].Cells[0];

                if (processedRowList[i])
                    previewTable.Rows[i].Cells[0].Value = null;
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

            currentTracks.Clear();
            newTracks.Clear();
            rows.Clear();
            source.ResetBindings(false);

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
            previewTable.AllowUserToResizeColumns = true;
            previewTable.AllowUserToResizeRows = true;
            foreach (DataGridViewColumn column in previewTable.Columns)
                column.SortMode = DataGridViewColumnSortMode.Automatic;

            previewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            previewTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;


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


            originalEncoding = Encoding.GetEncoding(initialEncodingListCustom.Text);
            usedEncoding = Encoding.GetEncoding(usedEncodingListCustom.Text);

            previewSortTags = previewSortTagsСheckBox.Checked;

            MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files);
            if (files == null || files.Length == 0)
            {
                MessageBox.Show(this, MsgNoTracksSelected, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else
            {
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


            for (var fileCounter = 0; fileCounter < rows.Count; fileCounter++)
            {
                if (rows[fileCounter].Checked == Plugin.ColumnCheckedState)
                    currentTracks[fileCounter][0] = Plugin.ColumnCheckedState;
                else
                    currentTracks[fileCounter][0] = Plugin.ColumnUncheckedState;
            }

            ignoreClosingForm = true;

            return true;
        }

        private void previewChanges()
        {
            previewIsGenerated = true;

            var numberOfWritableTags = TagIdsNames.Count - ReadonlyTagsNames.Length - 1;

            var wasCuesheet = false;
            for (var fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (checkStoppingStatus())
                {
                    Invoke(new Action(() => { stopButtonClickedMethod(prepareBackgroundPreview); }));
                    return;
                }


                var currentFile = files[fileCounter];

                SetStatusBarTextForFileOperations(ReEncodeTagSbText, true, fileCounter, files.Length, currentFile);

                var currentTags = new string[numberOfWritableTags + 2];
                currentTags[0] = Plugin.ColumnCheckedState;
                currentTags[1] = currentFile;

                var newTags = new string[numberOfWritableTags + 2];
                newTags[0] = Plugin.ColumnCheckedState;
                newTags[1] = currentFile;

                if (!string.IsNullOrEmpty(GetFileTag(currentFile, MetaDataType.Cuesheet)))
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


                Row row = new Row
                {
                    Checked = Plugin.ColumnCheckedState,
                    File = currentFile,
                    Track = GetTrackRepresentation(currentTags, newTags, tagNames, previewSortTags),
                    NewTrack = GetTrackRepresentation(newTags, currentTags, tagNames, previewSortTags),
                };

                rows.Add(row);


                int rowCountToFormat1 = 0;
                Invoke(new Action(() => { rowCountToFormat1 = AddRowsToTable(this, previewTable, source, rows.Count, false, true); }));
                Invoke(new Action(() => { FormatChangedTags(this, previewTable, rowCountToFormat1, false); }));

                currentTracks.Add(currentTags);
                newTracks.Add(newTags);
            }

            int rowCountToFormat2 = 0;
            Invoke(new Action(() => { rowCountToFormat2 = AddRowsToTable(this, previewTable, source, rows.Count, true, true); }));
            Invoke(new Action(() => { FormatChangedTags(this, previewTable, rowCountToFormat2, true); checkStoppedStatus(); resetFormToGeneratedPreview(); }));

            if (wasCuesheet)
                LastCommandSbText = "<CUESHEET>";
            else
                SetResultingSbText();
        }

        private void applyChanges()
        {
            if (newTracks.Count == 0)
                throw new Exception("Something went wrong! Empty 'newTracks' local variable (must be filled on generating preview.)");

            processedRowList.Clear(); //Indices of processed tracks
            Invoke(new Action(() => { previewTable.CurrentCell = previewTable.Rows[0].Cells[0]; }));
            Invoke(new Action(() => { previewTable.FirstDisplayedCell = previewTable.CurrentCell; }));

            cuesheetTracks.Clear();

            for (var i = 0; i < currentTracks.Count; i++)
            {
                if (checkStoppingStatus())
                {
                    Invoke(new Action(() => { stopButtonClickedMethod(applyingChangesStopped); }));
                    return;
                }


                var isChecked = newTracks[i][0];

                if (isChecked == Plugin.ColumnCheckedState)
                {
                    var currentFile = newTracks[i][1];

                    currentTracks[i][0] = string.Empty;

                    processedRowList.Add(true);
                    SetStatusBarTextForFileOperations(ReEncodeTagSbText, false, i, newTracks.Count, currentFile);

                    var cuesheet = GetFileTag(currentFile, MetaDataType.Cuesheet);
                    if (string.IsNullOrEmpty(cuesheet))
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
            saveWindowLayout(previewTable.Columns[1].Width, previewTable.Columns[2].Width, previewTable.Columns[3].Width);

            SavedSettings.initialEncodingName = initialEncodingListCustom.Text;
            //SavedSettings.usedEncodingName = usedEncodingListCustom.Text;

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
                    rows[0].Checked = Plugin.ColumnUncheckedState;
                else
                    rows[0].Checked = Plugin.ColumnCheckedState;
            }

            int firstRow = previewTable.FirstDisplayedCell.RowIndex;

            source.ResetBindings(false);

            var firstCell = previewTable.Rows[firstRow].Cells[0];
            previewTable.FirstDisplayedCell = firstCell;
        }

        private void previewTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string sourceTagValue;

                var isChecked = previewTable.Rows[e.RowIndex].Cells[0].Value as string;

                if (isChecked == Plugin.ColumnCheckedState)
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = Plugin.ColumnUncheckedState;

                    sourceTagValue = previewTable.Rows[e.RowIndex].Cells[2].Value as string;

                    previewTable.Rows[e.RowIndex].Cells[3].Value = sourceTagValue;
                }
                else if (isChecked == Plugin.ColumnUncheckedState)
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = Plugin.ColumnCheckedState;

                    sourceTagValue = previewTable.Rows[e.RowIndex].Cells[2].Value as string;

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
            buttonOK.Enable((previewIsGenerated || SavedSettings.allowCommandExecutionWithoutPreview) && !backgroundTaskIsStopping && (!backgroundTaskIsWorking() || backgroundTaskIsUpdatingTags));
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
                    MessageBox.Show(this, previewTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as string);
                }
                catch //-V3163 //-V5606
                {
                    // ignored
                }
            }
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            var settings = new QuickSettings(TagToolsPlugin);
            Display(settings, true);
        }

        private void ReEncodeTags_Load(object sender, EventArgs e)
        {
            var value = loadWindowLayout();

            if (value.Item1 != 0)
            {
                previewTable.Columns[1].FillWeight = value.Item1;
                previewTable.Columns[2].FillWeight = value.Item2;
            }
        }

        private void ReEncodeTags_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ignoreClosingForm)
            {
                if (!backgroundTaskIsUpdatingTags)
                {
                    closeFormOnStopping = true;
                    buttonClose.Enable(false);

                    backgroundTaskIsStopping = true;
                    SetStatusBarText(ReEncodeTagSbText + SbTextStoppingCurrentOperation, false);

                    e.Cancel = true;
                }
            }
            else
            {
                saveWindowLayout(previewTable.Columns[1].Width, previewTable.Columns[2].Width);
            }
        }
    }
}
