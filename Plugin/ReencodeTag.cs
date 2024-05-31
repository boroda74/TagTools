using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
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

        private delegate void AddRowToTable(string[] row);
        private delegate void ProcessRowOfTable(int row);
        private AddRowToTable addRowToTable;
        private ProcessRowOfTable processRowOfTable;

        private delegate void UpdateCustomScrollBarsDelegate(DataGridView dataGridView);
        private UpdateCustomScrollBarsDelegate updateCustomScrollBars;

        private string[] files = Array.Empty<string>();
        private readonly List<string[]> tags = new List<string[]>();
        private MetaDataType sourceTagId;

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
            previewTable.Columns[4].HeaderCell.Style = headerCellStyle;

            previewTable.Columns[2].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[3].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[4].HeaderCell.Style.WrapMode = DataGridViewTriState.True;

            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            addRowToTable = previewTable_AddRowToTable;
            processRowOfTable = previewTable_ProcessRowOfTable;

            updateCustomScrollBars = UpdateCustomScrollBars;


            enableDisablePreviewOptionControls(true, true);
            enableQueryingOrUpdatingButtons();

            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        private void previewTable_AddRowToTable(object[] row)
        {
            previewTable.Rows.Add(row);
            previewTableFormatRow(previewTable.RowCount - 1);

            if ((previewTable.RowCount & 0x1f) == 0)
                UpdateCustomScrollBars(previewTable);
        }

        private void previewTable_ProcessRowOfTable(int rowIndex)
        {
            previewTable.Rows[rowIndex].Cells[0].Value = null;
            previewTableFormatRow(rowIndex);
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

        private bool prepareBackgroundPreview()
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

            sourceTagId = GetTagId(sourceTagListCustom.Text);
            originalEncoding = Encoding.GetEncoding(initialEncodingListCustom.Text);
            usedEncoding = Encoding.GetEncoding(usedEncodingListCustom.Text);


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
                tags.Clear();

                for (var fileCounter = 0; fileCounter < previewTable.Rows.Count; fileCounter++)
                {
                    var tag = new string[3];

                    tag[0] = (string)previewTable.Rows[fileCounter].Cells[0].Value;
                    tag[1] = (string)previewTable.Rows[fileCounter].Cells[1].Value;
                    tag[2] = (string)previewTable.Rows[fileCounter].Cells[4].Value;

                    tags.Add(tag);
                }

                return true;
            }
        }

        private void previewChanges()
        {
            var stripNotChangedLines = SavedSettings.dontIncludeInPreviewLinesWithoutChangedTags;

            // ReSharper disable once RedundantAssignment
            string[] row = { "Checked", "File", "Track", "OriginalTag", "NewTag" };
            // ReSharper disable once RedundantAssignment
            string[] tag = { "Checked", "file", "newTag" };

            for (var fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (backgroundTaskIsCanceled)
                    return;

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

                tag = new string[3];

                tag[0] = isChecked;
                tag[1] = currentFile;
                tag[2] = newTagValue;


                row = new string[5];

                row[0] = isChecked;
                row[1] = currentFile;
                row[2] = track;
                row[3] = sourceTagValue;
                row[4] = newTagValue;

                Invoke(addRowToTable, new object[] { row });

                tags.Add(tag);

                previewIsGenerated = true;
            }

            Invoke(updateCustomScrollBars, previewTable);
            SetResultingSbText();
        }

        private void applyChanges()
        {
            if (tags.Count == 0)
                previewChanges();

            for (var i = 0; i < tags.Count; i++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                var isChecked = tags[i][0];

                if (isChecked == "T")
                {
                    var currentFile = tags[i][1];
                    var newTag = tags[i][2];

                    tags[i][0] = string.Empty;

                    Invoke(processRowOfTable, new object[] { i });

                    SetStatusBarTextForFileOperations(ReEncodeTagSbText, false, i, tags.Count, currentFile);

                    SetFileTag(currentFile, sourceTagId, newTag);
                    CommitTagsToFile(currentFile);
                }
            }

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

                    sourceTagValue = (string)previewTable.Rows[e.RowIndex].Cells[3].Value;

                    previewTable.Rows[e.RowIndex].Cells[4].Value = sourceTagValue;
                }
                else if (isChecked == "F")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "T";

                    sourceTagValue = (string)previewTable.Rows[e.RowIndex].Cells[3].Value;

                    var newTagValue = reencode(sourceTagValue);

                    previewTable.Rows[e.RowIndex].Cells[4].Value = newTagValue;
                }

                previewTableFormatRow(e.RowIndex);
            }
        }

        private void previewTableFormatRow(int rowIndex)
        {
            if (SavedSettings.dontHighlightChangedTags)
                return;

            if ((string)previewTable.Rows[rowIndex].Cells[0].Value != "T")
            {
                for (var columnIndex = 1; columnIndex < previewTable.ColumnCount; columnIndex++)
                {
                    if (previewTable.Columns[columnIndex].Visible)
                        previewTable.Rows[rowIndex].Cells[columnIndex].Style = dimmedCellStyle;
                }

                return;
            }


            for (var columnIndex = 1; columnIndex < previewTable.ColumnCount; columnIndex++)
            {
                if (columnIndex == 4)
                {
                    if ((string)previewTable.Rows[rowIndex].Cells[3].Value == (string)previewTable.Rows[rowIndex].Cells[4].Value)
                        previewTable.Rows[rowIndex].Cells[4].Style = unchangedCellStyle;
                    else
                        previewTable.Rows[rowIndex].Cells[4].Style = changedCellStyle;
                }
                else
                {
                    previewTable.Rows[rowIndex].Cells[columnIndex].Style = unchangedCellStyle;
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
            buttonOK.Enable((previewIsGenerated && !previewIsStopped) || SavedSettings.allowCommandExecutionWithoutPreview);
            buttonPreview.Enable(true);
        }

        internal override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enable(false);
            buttonPreview.Enable(false);
        }

        private void ReEncodeTagPlugin_Load(object sender, EventArgs e)
        {
            var value = loadWindowLayout();

            if (value.Item1 != 0)
            {
                previewTable.Columns[2].Width = (int)Math.Round(value.Item1 * hDpiFontScaling);
                previewTable.Columns[3].Width = (int)Math.Round(value.Item2 * hDpiFontScaling);
                previewTable.Columns[4].Width = (int)Math.Round(value.Item3 * hDpiFontScaling);
            }
            else
            {
                previewTable.Columns[2].Width = (int)Math.Round(previewTable.Columns[2].Width * hDpiFontScaling);
                previewTable.Columns[3].Width = (int)Math.Round(previewTable.Columns[3].Width * hDpiFontScaling);
                previewTable.Columns[4].Width = (int)Math.Round(previewTable.Columns[4].Width * hDpiFontScaling);
            }
        }

        private void ReEncodeTagPlugin_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveWindowLayout(previewTable.Columns[2].Width, previewTable.Columns[3].Width, previewTable.Columns[4].Width);
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            var settings = new QuickSettings(TagToolsPlugin);
            Display(settings, true);
        }
    }
}
