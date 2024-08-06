using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class CompareTracks : PluginWindowTemplate
    {
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

        private DataGridViewColumn columnTemplate;
        private DataGridViewCell artworkCellTemplate;

        private int rowHeadersWidth;
        private int defaultColumnWidth;

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


            var headerCellStyle = new DataGridViewCellStyle(HeaderCellStyle);

            previewTable.TopLeftHeaderCell.Style = headerCellStyle;
            previewTable.TopLeftHeaderCell.Value = CtlTags;

            previewTable.RowHeadersDefaultCellStyle = headerCellStyle;

            previewTable.Columns[0].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[1].HeaderCell.Style = headerCellStyle;

            columnTemplate = previewTable.Columns[0].Clone() as DataGridViewColumn;
            artworkCellTemplate = previewTable.Columns[1].CellTemplate;


            if (SavedSettings.displayedTags == null)
            {
                displayedTags = new int[tagIds.Count];
                for (var i = 0; i < tagIds.Count; i++)
                    displayedTags[i] = (int)tagIds[i];

                SavedSettings.displayedTags = displayedTags;
            }
            else
            {
                displayedTags = SavedSettings.displayedTags;
            }

            ignoreAutoSelectTagsCheckBoxCheckedEvent = true;
            autoSelectTagsCheckBox.Checked = !SavedSettings.dontAutoSelectDisplayedTags;
            ignoreAutoSelectTagsCheckBoxCheckedEvent = false;


            rowHeadersWidth = SavedSettings.rowHeadersWidth;
            defaultColumnWidth = SavedSettings.defaultColumnWidth;

            if (rowHeadersWidth < 5)
                rowHeadersWidth = 150;

            if (defaultColumnWidth < 5)
                defaultColumnWidth = 200;


            previewTable.RowHeadersWidth = rowHeadersWidth;
            previewTable.Columns[0].Width = defaultColumnWidth;


            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        private void fillTagNamesInTable()
        {
            previewTable.RowCount = 1;
            if (reallyDisplayedTags.Count > 1)
                previewTable.RowCount = reallyDisplayedTags.Count;

            artworkRow = -1;

            for (var j = 0; j < reallyDisplayedTags.Count; j++)
            {
                previewTable.Rows[j].HeaderCell.Value = GetTagName((MetaDataType)reallyDisplayedTags[j]);

                if ((MetaDataType)reallyDisplayedTags[j] == MetaDataType.Artwork)
                    artworkRow = j;
            }

            updateCustomScrollBars(previewTable);
        }

        private bool fillTable(bool reuseCache)
        {
            if (reuseCache)
            {
                fillTagNamesInTable();

                enableQueryingOrUpdatingButtons();

                this.Enable(true, null);


                if (reallyDisplayedTags.Count == 0)
                {
                    resetPreviewTable(true, true);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                this.Enable(false, null);

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

        private void resetPreviewTable(bool resetRows, bool noReallyDisplayedTagsOtherwiseNoPreview)
        {
            previewIsGenerated = false;
            cachedTracks = null;
            buffer.Clear();


            buttonOK.Enable(false);

            var newColumn = columnTemplate.Clone() as DataGridViewColumn;
            newColumn.Width = defaultColumnWidth;

            if (resetRows)
            {
                previewTable.RowCount = 1;

                if (noReallyDisplayedTagsOtherwiseNoPreview)
                    previewTable.Rows[0].HeaderCell.Value = CtlNoDifferences;
                else
                    previewTable.Rows[0].HeaderCell.Value = CtlNoPreview;
            }

            previewTable.Columns.Insert(0, newColumn);
            previewTable.ColumnCount = 1;

            updateCustomScrollBars(previewTable);
            SetStatusBarText(string.Empty, false);

            enableQueryingOrUpdatingButtons();
            enableDisablePreviewOptionControls(true);
        }

        private void addTrackColumn(int trackNo)
        {
            if (!backgroundTaskIsWorking() || previewIsStopped)
                return;

            try
            {
                var newColumn = columnTemplate.Clone() as DataGridViewColumn;
                newColumn.HeaderText = (trackNo + 1).ToString();
                newColumn.ToolTipText = trackUrls[trackNo];
                newColumn.Width = defaultColumnWidth;

                if (trackNo == 0)
                {
                    previewTable.Columns.Insert(0, newColumn);
                    previewTable.ColumnCount = 1;
                }
                else
                {
                    previewTable.Columns.Add(newColumn);
                }
            }
            catch
            {
                //Generating preview is stopped. There is some .Net bug. Let's ignore.
            }


            if ((previewTable.ColumnCount & 0x1f) == 0)
                updateCustomScrollBars(previewTable);

            SetStatusBarTextForFileOperations(CompareTracksSbText, true, trackNo, cachedTracks.Count);
        }

        private void setTrackTags(int trackNo, string[] tagValues)
        {
            if (!backgroundTaskIsWorking() || previewIsStopped)
                return;

            for (var j = 0; j < reallyDisplayedTags.Count; j++)
            {
                if (j != artworkRow)
                {
                    previewTable.Rows[j].Cells[trackNo].Value = tagValues[j];
                }
                else
                {
                    Bitmap artwork;

                    if (tagValues[j] == string.Empty)
                        artwork = emptyArtwork;
                    else
                        artwork = typeConverter.ConvertFrom(Convert.FromBase64String(tagValues[j])) as Bitmap;


                    previewTable.Rows[j].Cells[trackNo] = artworkCellTemplate.Clone() as DataGridViewCell;
                    previewTable.Rows[j].Cells[trackNo].Value = artwork;
                    previewTable.Rows[j].Cells[trackNo].Tag = tagValues[j];
                    previewTable.Rows[j].Cells[trackNo].ReadOnly = true;
                }
            }
        }

        private void resetFormToGeneratedPreview()
        {
            enableDisablePreviewOptionControls(true);

            updateCustomScrollBars(previewTable);
            SetResultingSbText();
        }

        internal bool prepareBackgroundPreview()
        {
            enableDisablePreviewOptionControls(false);

            if (previewIsGenerated)
            {
                resetPreviewTable(true, false);
                enableDisablePreviewOptionControls(true);
                return true;
            }

            resetPreviewTable(false, false);

            return fillTable(false);
        }

        private void previewChanges()
        {
            string[] tagValues = new string[reallyDisplayedTags.Count];

            previewIsGenerated = true;

            for (var trackNo = 0; trackNo < cachedTracks.Count; trackNo++)
            {
                if (backgroundTaskIsCanceled || previewIsStopped)
                {
                    Invoke(new Action(() => { resetPreviewTable(false, false); }));
                    return;
                }


                Invoke(new Action(() => { addTrackColumn(trackNo); }));

                for (var j = 0; j < reallyDisplayedTags.Count; j++)
                {
                    if (backgroundTaskIsCanceled || previewIsStopped)
                    {
                        Invoke(new Action(() => { resetPreviewTable(false, false); }));
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

                    tagValues[j] = cachedTracks[trackNo][tagPosition];
                }

                Invoke(new Action(() => { setTrackTags(trackNo, tagValues); }));
            }

            Invoke(new Action(() => { resetFormToGeneratedPreview(); }));
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
            SavedSettings.displayedTags = displayedTags;
            SavedSettings.rowHeadersWidth = rowHeadersWidth;
            SavedSettings.defaultColumnWidth = defaultColumnWidth;
            SavedSettings.dontAutoSelectDisplayedTags = !autoSelectTagsCheckBox.Checked;

            TagToolsPlugin.SaveSettings();
        }

        private void applyChanges()
        {
            if (cachedTracks == null || cachedTracks.Count == 0)
                throw new Exception("No cached tracks!");

            for (var i = 0; i < trackUrls.Length; i++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                string currentFile = trackUrls[i];

                SetStatusBarTextForFileOperations(CompareTracksSbText, false, i, trackUrls.Length, currentFile);

                for (var j = 0; j < reallyDisplayedTags.Count; j++)
                {
                    if (j == artworkRow)
                        SetFileTag(currentFile, (MetaDataType)reallyDisplayedTags[j], (string)previewTable.Rows[j].Cells[i].Tag);
                    else
                        SetFileTag(currentFile, (MetaDataType)reallyDisplayedTags[j], (string)previewTable.Rows[j].Cells[i].Value);
                }

                CommitTagsToFile(currentFile);
            }

            RefreshPanels(true);
            SetResultingSbText();
        }

        internal override void enableDisablePreviewOptionControls(bool enable, bool dontChangeDisabled = false)
        {
            autoSelectTagsCheckBox.Enable(enable);
            buttonSelectTags.Enable(enable && !autoSelectTagsCheckBox.Checked);
            rememberColumnAsDefaultWidthCheckBox.Enable(enable);
            buttonCopy.Enable(enable);
            buttonPaste.Enable(enable);
            buttonClear.Enable(enable);
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

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewChanges, buttonPreview, buttonOK, buttonClose, 0, 1);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            switchOperation(applyChanges, buttonOK, buttonOK, buttonPreview, buttonClose, true, null, true);
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

        private void buttonSelectTags_Click(object sender, EventArgs e)
        {
            var newDisplayedTags = CopyTagsToClipboard.SelectTags(TagToolsPlugin, SelectDisplayedTagsWindowTitle, SelectButtonName, displayedTags, true, true);
            if (areOldAndNewDisplayedTagsDifferent(displayedTags, newDisplayedTags))
            {
                displayedTags = newDisplayedTags;
                resetPreviewTable(true, false);
                clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewChanges, buttonPreview, buttonOK, buttonClose, 0, 1);
            }
        }

        private void autoSelectTagsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (autoSelectTagsCheckBox.Checked)
            {
                buttonSelectTags.Enable(false);

                displayedTagsBackup = displayedTags;
                displayedTags = new int[tagIds.Count];

                for (var i = 0; i < tagIds.Count; i++)
                    displayedTags[i] = (int)tagIds[i];
            }
            else
            {
                buttonSelectTags.Enable(true);

                displayedTags = displayedTagsBackup;
            }


            if (ignoreAutoSelectTagsCheckBoxCheckedEvent)
                return;


            resetPreviewTable(true, false);
            clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewChanges, buttonPreview, buttonOK, buttonClose, 0, 1);
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

        private void CompareTracks_Shown(object sender, EventArgs e)
        {
            previewTable.ColumnCount = 1;
            Refresh();
            clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewChanges, buttonPreview, buttonOK, buttonClose, 0, 1);
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
    }
}
