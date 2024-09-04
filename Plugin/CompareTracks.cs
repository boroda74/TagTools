using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using ExtensionMethods;

using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class CompareTracks : PluginWindowTemplate
    {
        private DataGridViewColumn columnTemplate;
        private DataGridViewCellStyle headerCellStyle;
        private DataGridViewCell artworkCellTemplate;

        private int rowHeadersWidth;
        private int defaultColumnWidth;


        private DataGridViewBoundColumnList<DataGridViewBoundColumns> rows;
        BindingSource source = new BindingSource();

        private readonly string[] trackUrls;

        private List<string> tagNames;
        private List<MetaDataType> tagIds;
        private int[] displayedTags;
        private int[] displayedTagsBackup;
        private List<int> reallyDisplayedTags;

        private readonly Bitmap emptyArtwork = new Bitmap(1, 1);
        private int artworkRow = -1;
        private readonly TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(Bitmap));

        private List<List<string>> cachedTracks; //List of tracks in the order of trackUrls of list of displayedTags

        private bool ignoreAutoSelectTagsCheckBoxCheckedEvent;

        private readonly Dictionary<int, object> buffer = new Dictionary<int, object>();
        private Bitmap bufferArtwork;

        internal CompareTracks(Plugin plugin, string[] files) : base(plugin)
        {
            InitializeComponent();
            trackUrls = files;
        }

        protected override void initializeForm()
        {
            base.initializeForm();

            tagNames = new List<string>();
            FillListByTagNames(tagNames, false, true, false);

            tagIds = new List<MetaDataType>();
            for (var i = 0; i < tagNames.Count; i++)
                tagIds.Add(GetTagId(tagNames[i]));


            headerCellStyle = new DataGridViewCellStyle(HeaderCellStyle);

            previewTable.ColumnHeadersDefaultCellStyle = headerCellStyle;
            previewTable.RowHeadersDefaultCellStyle = headerCellStyle;

            previewTable.TopLeftHeaderCell.Style = headerCellStyle;
            previewTable.TopLeftHeaderCell.Value = CtlTags;

            previewTable.RowHeadersDefaultCellStyle = headerCellStyle;

            previewTable.Columns[0].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[1].HeaderCell.Style = headerCellStyle;

            columnTemplate = previewTable.Columns[0].Clone() as DataGridViewColumn;
            artworkCellTemplate = previewTable.Columns[1].CellTemplate;

            previewTable.DefaultCellStyle = new DataGridViewCellStyle(UnchangedCellStyle);

            if (SavedSettings.ctDisplayedTags == null)
            {
                displayedTags = new int[tagIds.Count];
                for (var i = 0; i < tagIds.Count; i++)
                    displayedTags[i] = (int)tagIds[i];

                SavedSettings.ctDisplayedTags = displayedTags;
            }
            else
            {
                displayedTags = SavedSettings.ctDisplayedTags;
            }

            ignoreAutoSelectTagsCheckBoxCheckedEvent = true;
            autoSelectTagsCheckBox.Checked = !SavedSettings.ctDontAutoSelectDisplayedTags;
            ignoreAutoSelectTagsCheckBoxCheckedEvent = false;


            rowHeadersWidth = SavedSettings.ctRowHeadersWidth;
            defaultColumnWidth = SavedSettings.ctDefaultColumnWidth;

            if (rowHeadersWidth < 5)
                rowHeadersWidth = 150;

            if (defaultColumnWidth < 5)
                defaultColumnWidth = 200;


            previewTable.RowHeadersWidth = rowHeadersWidth;
            previewTable.Columns[0].Width = defaultColumnWidth;


            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        private void detectArtworkRow()
        {
            artworkRow = -1;

            for (var j = 0; j < reallyDisplayedTags.Count; j++)
            {
                if ((MetaDataType)reallyDisplayedTags[j] == MetaDataType.Artwork)
                {
                    artworkRow = j;
                    break;
                }
            }
        }

        private void fillTagNamesInTable()
        {
            for (var j = 0; j < reallyDisplayedTags.Count; j++)
                previewTable.Rows[j].HeaderCell.Value = GetTagName((MetaDataType)reallyDisplayedTags[j]);
        }

        private bool fillTable(bool reuseCache)
        {
            if (reuseCache)
            {
                detectArtworkRow();
                enableQueryingOrUpdatingButtons();

                this.Enable(true, null, null);


                if (reallyDisplayedTags.Count == 0)
                {
                    resetPreviewData(true);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                this.Enable(false, null, null);

                enableDisablePreviewOptionControls(false);
                disableQueryingOrUpdatingButtons();

                cachedTracks = new List<List<string>>();

                if (displayedTags.Length > 0)
                {
                    for (var i = 0; i < trackUrls.Length; i++)
                    {
                        var cachedTrack = new List<string>();
                        cachedTracks.Add(cachedTrack);

                        for (var j = 0; j < displayedTags.Length; j++)
                            cachedTrack.Add(GetFileTag(trackUrls[i], (MetaDataType)displayedTags[j]));
                    }
                }


                getReallyDisplayedTags();

                return fillTable(true);
            }
        }

        private void resetPreviewData(bool noReallyDisplayedTagsOtherwiseNoPreview)
        {
            previewTable.AllowUserToResizeColumns = false;
            previewTable.AllowUserToResizeRows = false;


            previewIsGenerated = false;
            backgroundTaskIsStopping = false;
            backgroundTaskIsStoppedOrCancelled = false;


            previewTable.DataSource = null;//--------

            cachedTracks = null;
            buffer.Clear();
            artworkRow = -1;

            this.Enable(true, null, null);


            previewTable.DataSource = null;
            rows?.Clear();

            previewTable.RowCount = 1;

            if (noReallyDisplayedTagsOtherwiseNoPreview)
                previewTable.Rows[0].HeaderCell.Value = CtlNoDifferences;
            else
                previewTable.Rows[0].HeaderCell.Value = CtlNoPreview;


            var newColumn = columnTemplate.Clone() as DataGridViewColumn;
            newColumn.Width = defaultColumnWidth;

            previewTable.ColumnCount = 1;
            previewTable.Columns.Insert(0, newColumn);
            previewTable.ColumnCount = 1;

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
            previewTable.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);

            previewTable.AllowUserToResizeColumns = true;
            previewTable.AllowUserToResizeRows = true;


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

        internal bool prepareBackgroundPreview()
        {
            resetPreviewData(false);

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


            enableDisablePreviewOptionControls(false);


            previewTable.AllowUserToResizeColumns = false;
            previewTable.AllowUserToResizeRows = false;


            return fillTable(false);
        }

        private bool applyingChangesStopped()
        {
            previewTable.AllowUserToResizeColumns = true;
            previewTable.AllowUserToResizeRows = true;


            backgroundTaskIsScheduled = false;
            backgroundTaskIsStopping = false;
            backgroundTaskIsStoppedOrCancelled = false;
            closeFormOnStopping = false;
            ignoreClosingForm = false;

            SetResultingSbText();

            return true;
        }

        private void formatTrackColumns()
        {
            if (checkStoppingStatus())
                return;


            //try
            {
                for (int i = 0; i < previewTable.ColumnCount; i++)
                {
                    previewTable.Columns[i].HeaderCell.ToolTipText = trackUrls[i];
                    previewTable.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    previewTable.Columns[i].Width = defaultColumnWidth;
                }
            }
            //catch //-V3163 //-V5606
            //{
            //    //Generating preview is stopped. There is some .Net bug. Let's ignore. //--------
            //}
        }

        private void formatArtworkRow(string[] artworkTagValues)
        {

            if (artworkTagValues == null)
                return;

            if (checkStoppingStatus())
                return;

            for (int i = 0; i < cachedTracks.Count; i++)
            {
                //try
                {
                    previewTable.Rows[artworkRow].Cells[i] = artworkCellTemplate.Clone() as DataGridViewCell;
                    previewTable.Rows[artworkRow].Cells[i].Tag = artworkTagValues[i];
                    previewTable.Rows[artworkRow].Cells[i].ReadOnly = true;
                }
                //catch //-V3163 //-V5606
                //{
                //    //Generating preview is stopped. There is some .Net bug. Let's ignore. //--------
                //}
            }
        }

        private void addTagRow(int tagRowIndex, string[] tagValues)
        {
            List<object> row = new List<object>();

            for (var j = 0; j < cachedTracks.Count; j++)
            {
                if (checkStoppingStatus())
                    return;


                if (tagRowIndex != artworkRow)
                {
                    row.Add(tagValues[j]);
                }
                else
                {
                    Bitmap artwork;

                    if (string.IsNullOrEmpty(tagValues[j]))
                        artwork = emptyArtwork;
                    else
                        artwork = typeConverter.ConvertFrom(Convert.FromBase64String(tagValues[j])) as Bitmap;


                    row.Add(artwork);
                }

                SetStatusBarTextForFileOperations(CompareTracksSbText + CompareTracksSbTextTagNo + tagRowIndex + "/" + reallyDisplayedTags.Count, true, j, cachedTracks.Count);
            }

            rows.AddRow(row);
        }

        private void previewChanges()
        {
            previewIsGenerated = true;

            List<string> columnNames = new List<string>();
            for (var trackNo = 0; trackNo < cachedTracks.Count; trackNo++)
                columnNames.Add((trackNo + 1).ToString());

            rows = new DataGridViewBoundColumnList<DataGridViewBoundColumns>(columnNames);
            source.DataSource = rows;

            Invoke(new Action(() => { formatTrackColumns(); }));

            string[] artworkTagValues = null;

            for (var j = 0; j < reallyDisplayedTags.Count; j++)
            {
                if (checkStoppingStatus())
                {
                    Invoke(new Action(() => { stopButtonClickedMethod(prepareBackgroundPreview); }));
                    return;
                }


                var tagPosition = 0;
                for (var i = 0; i < displayedTags.Length; i++)
                {
                    if (displayedTags[i] == reallyDisplayedTags[j])
                    {
                        tagPosition = i;
                        break;
                    }
                }

                string[] tagValues = new string[cachedTracks.Count];

                for (var trackNo = 0; trackNo < cachedTracks.Count; trackNo++)
                {
                    if (checkStoppingStatus())
                    {
                        Invoke(new Action(() => { stopButtonClickedMethod(prepareBackgroundPreview); }));
                        return;
                    }


                    tagValues[trackNo] = cachedTracks[trackNo][tagPosition];
                }

                if (j == artworkRow)
                    artworkTagValues = tagValues;

                addTagRow(j, tagValues);
            }

            Invoke(new Action(() => {
                if (previewTable.ColumnCount > 0)
                    previewTable.Columns.RemoveAt(0);

                source.ResetBindings(true); 
                previewTable.DataSource = source; 
                }));

            Invoke(new Action(() => { formatArtworkRow(artworkTagValues); fillTagNamesInTable(); checkStoppedStatus(); resetFormToGeneratedPreview(); }));
        }

        private void getReallyDisplayedTags()
        {
            reallyDisplayedTags = new List<int>();

            if (autoSelectTagsCheckBox.Checked)
            {
                var reallyDisplayedTags2 = new List<int>();


                for (var j = displayedTags.Length - 1; j >= 0; j--)
                {
                    for (var i = cachedTracks.Count - 1; i >= 0; i--)
                    {
                        for (var k = cachedTracks.Count - 1; k >= 0; k--)
                        {
                            if (cachedTracks[i][j] != cachedTracks[k][j])
                            {
                                if (reallyDisplayedTags2.AddUnique(displayedTags[j]))
                                    break;
                            }
                        }
                    }
                }


                for (var j = reallyDisplayedTags2.Count - 1; j >= 0; j--)
                    reallyDisplayedTags.Add(reallyDisplayedTags2[j]);
            }
            else
            {
                for (var j = 0; j < displayedTags.Length; j++)
                    reallyDisplayedTags.Add(displayedTags[j]);
            }


            if (autoSelectTagsCheckBox.Checked)
            {
                for (var i = cachedTracks.Count - 1; i >= 0; i--)
                {
                    var trackHaveDifferences = false;

                    for (var j = displayedTags.Length - 1; j >= 0; j--)
                    {
                        for (var k = cachedTracks.Count - 1; k >= 0; k--)
                        {
                            if (cachedTracks[i][j] != cachedTracks[k][j])
                            {
                                trackHaveDifferences = true;
                                goto breakpoint;
                            }
                        }
                    }

                breakpoint:
                    if (!trackHaveDifferences)
                    {
                        cachedTracks.RemoveAt(i);
                    }
                }
            }
        }

        private void saveSettings()
        {
            SavedSettings.ctDisplayedTags = displayedTags;
            SavedSettings.ctRowHeadersWidth = rowHeadersWidth;
            SavedSettings.ctDefaultColumnWidth = defaultColumnWidth;
            SavedSettings.ctDontAutoSelectDisplayedTags = !autoSelectTagsCheckBox.Checked;

            TagToolsPlugin.SaveSettings();
        }

        private void applyChanges()
        {
            if (cachedTracks == null || cachedTracks.Count == 0)
                throw new Exception("No cached tracks!");

            for (var i = 0; i < trackUrls.Length; i++)
            {
                if (checkStoppingStatus())
                {
                    Invoke(new Action(() => { stopButtonClickedMethod(applyingChangesStopped); }));
                    return;
                }


                string currentFile = trackUrls[i];

                SetStatusBarTextForFileOperations(CompareTracksSbText, false, i, trackUrls.Length, currentFile);

                for (var j = 0; j < reallyDisplayedTags.Count; j++)
                {
                    if (j == artworkRow)
                        SetFileTag(currentFile, (MetaDataType)reallyDisplayedTags[j], previewTable.Rows[j].Cells[i].Tag as string);
                    else
                        SetFileTag(currentFile, (MetaDataType)reallyDisplayedTags[j], previewTable.Rows[j].Cells[i].Value as string);
                }

                CommitTagsToFile(currentFile);
            }

            Invoke(new Action(() => { applyingChangesStopped(); }));

            RefreshPanels(true);
            SetResultingSbText();
        }

        internal override void enableDisablePreviewOptionControls(bool enable, bool dontChangeDisabled = false)
        {
            enable &= !backgroundTaskIsWorking();

            autoSelectTagsCheckBox.Enable(enable);
            buttonSelectTags.Enable(enable && !autoSelectTagsCheckBox.Checked);
            rememberColumnAsDefaultWidthCheckBox.Enable(enable);

            buttonCopy.Enable(enable && previewIsGenerated);
            buttonPaste.Enable(enable && previewIsGenerated);
            buttonClear.Enable(enable && previewIsGenerated);
        }

        internal override void enableQueryingOrUpdatingButtons()
        {
            buttonOK.Enable(previewIsGenerated && !backgroundTaskIsStopping && (!backgroundTaskIsWorking() || backgroundTaskIsUpdatingTags));
            buttonPreview.Enable(true);
        }

        internal override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enable(false);
            buttonPreview.Enable(false);
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            ignoreClosingForm = clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewChanges, buttonPreview, buttonOK, buttonClose, false);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            ignoreClosingForm = true;
            switchOperation(applyChanges, buttonOK, buttonOK, buttonPreview, buttonClose, true, null);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            saveSettings();
            Close();
        }

        private bool areOldAndNewDisplayedTagsDifferent(int[] oldTags, int[] newTags)
        {
            if (newTags.Length != oldTags.Length)
            {
                return true;
            }
            else
            {
                bool newTagsAreDifferent = false;
                for (int i = 0; i < newTags.Length; i++)
                {
                    if (newTags[i] != oldTags[i])
                    {
                        newTagsAreDifferent = true;
                        break;
                    }
                }

                if (newTagsAreDifferent)
                    return true;
                else
                    return false;
            }
        }

        private void fillAllDisplayedTagNames(int[] newDisplayedTags)
        {
            displayedTags = newDisplayedTags;
            resetPreviewData(false);


            this.Enable(false, null, null);
            enableDisablePreviewOptionControls(false);
            disableQueryingOrUpdatingButtons();


            cachedTracks = new List<List<string>>();
            reallyDisplayedTags = new List<int>();
            foreach (int tagId in displayedTags)
                reallyDisplayedTags.Add(tagId);

            buttonPreview.Text = buttonPreviewName;
            fillTable(true);


            enableQueryingOrUpdatingButtons();
            enableDisablePreviewOptionControls(true);
            this.Enable(true, null, null);
        }

        private void buttonSelectTags_Click(object sender, EventArgs e)
        {
            bool previewIsGeneratedBackup = previewIsGenerated;

            var newDisplayedTags = CopyTagsToClipboard.SelectTags(TagToolsPlugin, SelectDisplayedTagsWindowTitle, SelectButtonName, displayedTags, true, true);
            if (areOldAndNewDisplayedTagsDifferent(displayedTags, newDisplayedTags))
            {
                fillAllDisplayedTagNames(newDisplayedTags);
                resetPreviewData(false);

                if (previewIsGeneratedBackup)
                    buttonPreview_Click(null, null);
            }
        }

        private void autoSelectTagsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool previewIsGeneratedBackup = previewIsGenerated;

            if (autoSelectTagsCheckBox.Checked)
            {
                enableDisablePreviewOptionControls(false);
                disableQueryingOrUpdatingButtons();

                displayedTagsBackup = displayedTags;
                displayedTags = new int[tagIds.Count];

                for (var i = 0; i < tagIds.Count; i++)
                    displayedTags[i] = (int)tagIds[i];


                if (ignoreAutoSelectTagsCheckBoxCheckedEvent)
                {
                    enableQueryingOrUpdatingButtons();
                    enableDisablePreviewOptionControls(true);

                    return;
                }


                resetPreviewData(false);
            }
            else
            {
                fillAllDisplayedTagNames(displayedTagsBackup);
                resetPreviewData(false);
            }

            if (previewIsGeneratedBackup)
                buttonPreview_Click(null, null);
        }

        private void previewTable_RowHeadersWidthChanged(object sender, EventArgs e)
        {
            if (rememberColumnAsDefaultWidthCheckBox.Checked)
            {
                //rememberColumnAsDefaultWidthCheckBox.Checked = false;

                rowHeadersWidth = previewTable.RowHeadersWidth;
            }
        }

        private void previewTable_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (rememberColumnAsDefaultWidthCheckBox.Checked)
            {
                //rememberColumnAsDefaultWidthCheckBox.Checked = false;

                for (var i = 0; i < previewTable.ColumnCount; i++)
                {
                    previewTable.Columns[i].Width = e.Column.Width;

                }

                defaultColumnWidth = e.Column.Width;
            }
        }

        private void previewTable_SelectionChanged(object sender, EventArgs e)
        {
            if (previewTable.SelectedCells.Count > 1)
            {
                for (var j = 0; j < previewTable.RowCount; j++)
                {
                    var selectedRawCellsCount = 0;

                    for (var i = 0; i < previewTable.ColumnCount; i++)
                    {
                        if (previewTable.Rows[j].Cells[i].Selected)
                            selectedRawCellsCount++;
                    }

                    if (selectedRawCellsCount > 1)
                    {
                        for (var i = 0; i < previewTable.ColumnCount; i++)
                        {
                            if (previewTable.Rows[j].Cells[i].Selected)
                            {
                                previewTable.Rows[j].Cells[i].Selected = false;
                                selectedRawCellsCount--;
                            }

                            if (selectedRawCellsCount <= 1)
                                break;
                        }
                    }
                }
            }
        }

        private void previewTable_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex > -1)
            {
                for (var j = 0; j < previewTable.RowCount; j++)
                    for (var i = 1; i < previewTable.ColumnCount; i++)
                        previewTable.Rows[j].Cells[i].Selected = false;
            }


            for (var j = 0; j < previewTable.RowCount; j++)
                previewTable.Rows[j].Cells[e.ColumnIndex].Selected = true;
        }

        private void previewTable_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            buffer.Clear();

            for (var j = 0; j < previewTable.RowCount; j++)
            {
                for (var i = 0; i < previewTable.ColumnCount; i++)
                {
                    if (j == artworkRow)
                    {
                        if (previewTable.Rows[j].Cells[i].Selected)
                        {
                            bufferArtwork = (Bitmap)previewTable.Rows[j].Cells[i].Value;
                            buffer.Add(j, previewTable.Rows[j].Cells[i].Tag);
                            break;
                        }
                    }
                    else
                    {
                        if (previewTable.Rows[j].Cells[i].Selected)
                        {
                            buffer.Add(j, previewTable.Rows[j].Cells[i].Value);
                            break;
                        }
                    }
                }
            }
        }

        private void buttonPaste_Click(object sender, EventArgs e)
        {
            if (previewTable.CurrentCell != null && buffer.Count > 0)
            {
                foreach (var rowIndexTagValue in buffer)
                {
                    if (rowIndexTagValue.Key == artworkRow)
                    {
                        previewTable.Rows[rowIndexTagValue.Key].Cells[previewTable.CurrentCell.ColumnIndex].Value = bufferArtwork;
                        previewTable.Rows[rowIndexTagValue.Key].Cells[previewTable.CurrentCell.ColumnIndex].Tag = rowIndexTagValue.Value;
                    }
                    else
                    {
                        previewTable.Rows[rowIndexTagValue.Key].Cells[previewTable.CurrentCell.ColumnIndex].Value = rowIndexTagValue.Value;
                    }
                }
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            for (var j = 0; j < previewTable.RowCount; j++)
            {
                for (var i = 0; i < previewTable.ColumnCount; i++)
                {
                    if (previewTable.Rows[j].Cells[i].Selected)
                    {
                        if (i == artworkRow)
                        {
                            previewTable.Rows[j].Cells[i].Value = emptyArtwork;
                            previewTable.Rows[j].Cells[i].Tag = string.Empty;
                            break;
                        }
                        else
                        {
                            previewTable.Rows[j].Cells[i].Value = null;
                        }
                    }
                }
            }
        }

        private void rememberColumnAsDefaultWidthCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!rememberColumnAsDefaultWidthCheckBox.IsEnabled())
                return;

            rememberColumnAsDefaultWidthCheckBox.Checked = !rememberColumnAsDefaultWidthCheckBox.Checked;
        }

        private void CompareTracks_Load(object sender, EventArgs e)
        {
            placeholderLabel.Visible = false;
        }

        private void CompareTracks_Shown(object sender, EventArgs e)
        {
            resetPreviewData(false);
        }

        private void CompareTracks_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ignoreClosingForm)
            {
                if (!backgroundTaskIsUpdatingTags)
                {
                    closeFormOnStopping = true;
                    buttonClose.Enable(false);

                    backgroundTaskIsStopping = true;
                    SetStatusBarText(CompareTracksSbText + SbTextStoppingCurrentOperation, false);

                    e.Cancel = true;
                }
            }
        }
    }
}
