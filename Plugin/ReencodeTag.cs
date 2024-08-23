using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using ExtensionMethods;

using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class ReEncodeTag : PluginWindowTemplate
    {
        private CustomComboBox sourceTagListCustom;
        private CustomComboBox initialEncodingListCustom;
        private CustomComboBox usedEncodingListCustom;


        private readonly DataGridViewCellStyle unchangedCellStyle = new DataGridViewCellStyle(UnchangedCellStyle);
        private readonly DataGridViewCellStyle changedCellStyle = new DataGridViewCellStyle(ChangedCellStyle);
        private readonly DataGridViewCellStyle dimmedCellStyle = new DataGridViewCellStyle(DimmedCellStyle);

        private string[] files = Array.Empty<string>();
        private readonly List<string[]> tags = new List<string[]>();
        private MetaDataType sourceTagId;
        private List<bool> processedRowList = new List<bool>(); //Indices of processed tracks

        private Encoding defaultEncoding;
        private Encoding originalEncoding;
        private Encoding usedEncoding;

        internal ReEncodeTag(Plugin plugin) : base(plugin)
        {
            InitializeComponent();
        }

        protected override void initializeForm()
        {
            base.initializeForm();

            sourceTagListCustom = namesComboBoxes["sourceTagList"];
            initialEncodingListCustom = namesComboBoxes["initialEncodingList"];
            usedEncodingListCustom = namesComboBoxes["usedEncodingList"];


            buttonSettings.Image = ReplaceBitmap(buttonSettings.Image, Gear);

            FillListByTagNames(sourceTagListCustom.Items, false, false, false);
            sourceTagListCustom.Text = SavedSettings.reencodeTagSourceTagName;
            if (sourceTagListCustom.SelectedIndex == -1)
                sourceTagListCustom.SelectedIndex = 0;

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
            previewTable.Columns[3].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[4].HeaderCell.Style = headerCellStyle;

            previewTable.Columns[2].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[3].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[4].HeaderCell.Style.WrapMode = DataGridViewTriState.True;

            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            enableDisablePreviewOptionControls(true, true);
            enableQueryingOrUpdatingButtons();


            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        private string reencode(string source)
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

                previewTableFormatRow(previewTable, i);
            }
        }

        private void resetPreviewData()
        {
            if (previewIsGenerated)
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

            if (previewIsGenerated)
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


            sourceTagId = GetTagId(sourceTagListCustom.Text);
            originalEncoding = Encoding.GetEncoding(initialEncodingListCustom.Text);
            usedEncoding = Encoding.GetEncoding(usedEncodingListCustom.Text);


            MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files);
            if (files == null || files.Length == 0)
            {
                MessageBox.Show(this, MsgNoTracksSelected, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else
            {
                previewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
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
                    var tag = new string[3];

                    tag[0] = previewTable.Rows[fileCounter].Cells[0].Value as string;
                    tag[1] = previewTable.Rows[fileCounter].Cells[1].Value as string;
                    tag[2] = previewTable.Rows[fileCounter].Cells[4].Value as string;

                    tags.Add(tag);
                }

                ignoreClosingForm = true;

                return true;
            }
        }

        private void previewChanges()
        {
            previewIsGenerated = true;

            List<string[]> rows = new List<string[]>();
            var stripNotChangedLines = SavedSettings.dontIncludeInPreviewLinesWithoutChangedTags;

            for (var fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (checkStoppingStatus())
                {
                    Invoke(new Action(() => { stopButtonClickedMethod(prepareBackgroundPreview); }));
                    return;
                }


                // ReSharper disable once RedundantAssignment
                string[] row = { "Checked", "File", "Track", "OriginalTag", "NewTag" };
                // ReSharper disable once RedundantAssignment
                string[] tag = { "Checked", "file", "newTag" };

                var currentFile = files[fileCounter];

                SetStatusBarTextForFileOperations(ReEncodeTagSbText, true, fileCounter, files.Length, currentFile);

                var sourceTagValue = GetFileTag(currentFile, sourceTagId);
                var newTagValue = reencode(sourceTagValue);

                var track = GetTrackRepresentation(currentFile);


                string isChecked;
                if (sourceTagValue == newTagValue && stripNotChangedLines)
                    continue;
                else if (sourceTagValue == newTagValue)
                    isChecked = "F";
                else
                    isChecked = "T";

                tag[0] = isChecked;
                tag[1] = currentFile;
                tag[2] = newTagValue;

                tags.Add(tag);


                row[0] = isChecked;
                row[1] = currentFile;
                row[2] = track;
                row[3] = sourceTagValue;
                row[4] = newTagValue;

                rows.Add(row);


                int rowCountToFormat1 = 0;
                Invoke(new Action(() => { rowCountToFormat1 = AddRowsToTable(this, previewTable, rows, false, true); }));
                Invoke(new Action(() => { FormatChangedTags(this, previewTable, rowCountToFormat1, previewTableFormatRow); }));
            }

            int rowCountToFormat2 = 0;
            Invoke(new Action(() => { rowCountToFormat2 = AddRowsToTable(this, previewTable, rows, true, true); }));
            Invoke(new Action(() => { FormatChangedTags(this, previewTable, rowCountToFormat2, previewTableFormatRow); checkStoppedStatus(); resetFormToGeneratedPreview(); }));
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
                    var currentFile = tags[i][1];
                    var newTag = tags[i][2];

                    tags[i][0] = string.Empty;

                    processedRowList.Add(true);
                    SetStatusBarTextForFileOperations(ReEncodeTagSbText, false, i, tags.Count, currentFile);

                    SetFileTag(currentFile, sourceTagId, newTag);
                    CommitTagsToFile(currentFile);
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

            SavedSettings.reencodeTagSourceTagName = sourceTagListCustom.Text;
            SavedSettings.initialEncodingName = initialEncodingListCustom.Text;
            //SavedSettings.usedEncodingName = usedEncodingListCustom.Text;
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

                var isChecked = previewTable.Rows[e.RowIndex].Cells[0].Value as string;

                if (isChecked == "T")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "F";

                    sourceTagValue = previewTable.Rows[e.RowIndex].Cells[3].Value as string;

                    previewTable.Rows[e.RowIndex].Cells[4].Value = sourceTagValue;
                }
                else if (isChecked == "F")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "T";

                    sourceTagValue = previewTable.Rows[e.RowIndex].Cells[3].Value as string;

                    var newTagValue = reencode(sourceTagValue);

                    previewTable.Rows[e.RowIndex].Cells[4].Value = newTagValue;
                }

                previewTableFormatRow(previewTable, e.RowIndex);
            }
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
                if (columnIndex == 4)
                {
                    if ((string)dataGridView.Rows[rowIndex].Cells[3].Value == (string)dataGridView.Rows[rowIndex].Cells[4].Value)
                        dataGridView.Rows[rowIndex].Cells[4].Style = unchangedCellStyle;
                    else
                        dataGridView.Rows[rowIndex].Cells[4].Style = changedCellStyle;
                }
                else
                {
                    dataGridView.Rows[rowIndex].Cells[columnIndex].Style = unchangedCellStyle;
                }
            }
        }

        internal override void enableDisablePreviewOptionControls(bool enable, bool dontChangeDisabled = false)
        {
            if (enable && previewIsGenerated)
                return;

            sourceTagListCustom.Enable(enable);
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

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            var settings = new QuickSettings(TagToolsPlugin);
            Display(settings, true);
        }

        private void ReEncodeTagPlugin_Load(object sender, EventArgs e)
        {
            var value = loadWindowLayout();

            if (value.Item1 != 0)
            {
                previewTable.Columns[2].FillWeight = value.Item1;
                previewTable.Columns[3].FillWeight = value.Item2;
                previewTable.Columns[4].FillWeight = value.Item3;
            }
        }

        private void ReEncodeTagPlugin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ignoreClosingForm)
            {
                if (!backgroundTaskIsUpdatingTags)
                {
                    closeFormOnStopping = true;
                    buttonClose.Enable(false);
                }

                backgroundTaskIsStopping = true;
                SetStatusBarText(ReEncodeTagSbText + SbTextStoppingCurrentOperation, false);

                e.Cancel = true;
            }
            else
            {
                saveWindowLayout(previewTable.Columns[2].Width, previewTable.Columns[3].Width, previewTable.Columns[4].Width);
            }
        }
    }
}
