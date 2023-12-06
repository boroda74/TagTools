using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;
using ExtensionMethods;

namespace MusicBeePlugin
{
    public partial class CompareTracksCommand : PluginWindowTemplate
    {
        private string[] trackUrls;

        private List<string> tagNames;
        private List<MetaDataType> tagIds;
        private int[] displayedTags;
        private List<int> reallyDisplayedTags;

        private Bitmap emptyArtwork = new Bitmap(1, 1);
        private int artworkRow;
        private TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(Bitmap));

        private List<List<string>> cachedTracks;

        private DataGridViewColumn columnTemplate;
        private DataGridViewCell artworkCellTemplate;

        private Color noBackupDataCellForeColor;

        private int rowHeadersWidth;
        private int defaultColumnWidth;

        private bool ignoreAutoSelectTagsCheckBoxCheckedEvent = false;

        private Dictionary<int, object> buffer = new Dictionary<int, object>();
        private Bitmap Artwork;

        public CompareTracksCommand(Plugin tagToolsPluginParam, string[] files) : base(tagToolsPluginParam)
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
            for (int i = 0; i < tagNames.Count; i++)
                tagIds.Add(GetTagId(tagNames[i]));


            columnTemplate = (DataGridViewColumn)previewTable.Columns[0].Clone();
            artworkCellTemplate = previewTable.Columns[1].CellTemplate;

            noBackupDataCellForeColor = SystemColors.HotTrack;


            if (!SavedSettings.dontAutoSelectDisplayedTags || SavedSettings.displayedTags == null)
            {
                displayedTags = new int[tagIds.Count];
                for (int i = 0; i < tagIds.Count; i++)
                    displayedTags[i] = (int)tagIds[i];

                SavedSettings.displayedTags = displayedTags;
            }
            else
            {
                displayedTags = SavedSettings.displayedTags;
            }

            ignoreAutoSelectTagsCheckBoxCheckedEvent = true;
            AutoSelectTagsCheckBox.Checked = !SavedSettings.dontAutoSelectDisplayedTags;
            ignoreAutoSelectTagsCheckBoxCheckedEvent = false;


            rowHeadersWidth = SavedSettings.rowHeadersWidth;
            defaultColumnWidth = SavedSettings.defaultColumnWidth;

            if (rowHeadersWidth < 5)
                rowHeadersWidth = 150;

            if (defaultColumnWidth < 5)
                defaultColumnWidth = 200;


            previewTable.RowHeadersWidth = rowHeadersWidth;
            previewTable.Columns[0].Width = defaultColumnWidth;


            button_GotFocus(this.AcceptButton, null); //Let's mark active button
        }

        private void fillTagNamesInTable()
        {
            previewTable.RowCount = 1;
            if (reallyDisplayedTags.Count > 1)
                previewTable.RowCount = reallyDisplayedTags.Count;

            artworkRow = -1;

            for (int j = 0; j < reallyDisplayedTags.Count; j++)
            {
                previewTable.Rows[j].HeaderCell.Value = GetTagName((MetaDataType)reallyDisplayedTags[j]);

                if ((MetaDataType)reallyDisplayedTags[j] == MetaDataType.Artwork)
                {
                    artworkRow = j;
                }
            }
        }

        private void fillTable(bool reuseCache)
        {
            if (reuseCache)
            {
                fillTagNamesInTable();

                if (reallyDisplayedTags.Count == 0)
                {
                    DataGridViewColumn newColumn = (DataGridViewColumn)columnTemplate.Clone();
                    newColumn.HeaderText = string.Empty;
                    newColumn.ToolTipText = string.Empty;
                    newColumn.Width = defaultColumnWidth;

                    previewTable.Columns.Insert(0, newColumn);
                    previewTable.ColumnCount = 1;


                    previewTable.RowCount = 1;

                    previewTable.Rows[0].HeaderCell.Value = CtlNoDifferences;

                    if (artworkRow != 0)
                    {
                        previewTable.Rows[0].Cells[0].Value = null;
                    }
                    else
                    {
                        previewTable.Rows[0].Cells[0].Value = new Bitmap(1, 1);
                        previewTable.Rows[0].Cells[0].Tag = string.Empty;
                    }
                }
                else
                {
                    for (int trackNo = 0; trackNo < cachedTracks.Count; trackNo++)
                    {
                        DataGridViewColumn newColumn = (DataGridViewColumn)columnTemplate.Clone();
                        newColumn.HeaderText = string.Empty + (trackNo + 1);
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


                        for (int j = 0; j < reallyDisplayedTags.Count; j++)
                        {
                            int tagPosition = 0;
                            for (int i = 0; i < displayedTags.Length; i++)
                            {
                                if (displayedTags[i] == reallyDisplayedTags[j])
                                {
                                    tagPosition = i;
                                    break;
                                }
                            }

                            string tagValue = cachedTracks[trackNo][tagPosition];

                            if (j != artworkRow)
                            {
                                previewTable.Rows[j].Cells[trackNo].Value = tagValue;
                            }
                            else
                            {
                                Bitmap artwork;

                                if (tagValue == string.Empty)
                                    artwork = emptyArtwork;
                                else
                                    artwork = (Bitmap)typeConverter.ConvertFrom(Convert.FromBase64String(tagValue));


                                previewTable.Rows[j].Cells[trackNo] = (DataGridViewCell)artworkCellTemplate.Clone();
                                previewTable.Rows[j].Cells[trackNo].Value = artwork;
                                previewTable.Rows[j].Cells[trackNo].Tag = tagValue;
                                previewTable.Rows[j].Cells[trackNo].ReadOnly = true;
                            }
                        }
                    }
                }
            }
            else
            {
                cachedTracks = new List<List<string>>();

                for (int i = 0; i < trackUrls.Length; i++)
                {
                    var cachedTrack = new List<string>();
                    cachedTracks.Add(cachedTrack);

                    for (int j = 0; j < displayedTags.Length; j++)
                    {
                        cachedTrack.Add(GetFileTag(trackUrls[i], (MetaDataType)displayedTags[j]));
                    }
                }


                getReallyDisplayedTags();


                fillTable(true);

                if (!SavedSettings.dontPlayCompletedSound)
                    System.Media.SystemSounds.Asterisk.Play();
            }
        }

        private void getReallyDisplayedTags()
        {
            reallyDisplayedTags = new List<int>();

            if (!SavedSettings.dontAutoSelectDisplayedTags)
            {
                List<int> reallyDisplayedTags2 = new List<int>();


                for (int j = displayedTags.Length - 1; j >= 0; j--)
                {
                    for (int i = cachedTracks.Count - 1; i >= 0; i--)
                    {
                        for (int k = cachedTracks.Count - 1; k >= 0; k--)
                        {
                            if (cachedTracks[i][j] != cachedTracks[k][j])
                            {
                                if (!reallyDisplayedTags2.Contains(displayedTags[j]))
                                {
                                    reallyDisplayedTags2.Add(displayedTags[j]);
                                    break;
                                }
                            }
                        }
                    }
                }


                for (int j = reallyDisplayedTags2.Count - 1; j >= 0; j--)
                    reallyDisplayedTags.Add(reallyDisplayedTags2[j]);
            }
            else
            {
                for (int j = 0; j < displayedTags.Length; j++)
                    reallyDisplayedTags.Add(displayedTags[j]);
            }


            if (!SavedSettings.dontAutoSelectDisplayedTags)
            {
                for (int i = cachedTracks.Count - 1; i >= 0; i--)
                {
                    bool trackHaveDifferences = false;

                    for (int j = displayedTags.Length - 1; j >= 0; j--)
                    {
                        for (int k = cachedTracks.Count - 1; k >= 0; k--)
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
            SavedSettings.dontAutoSelectDisplayedTags = !AutoSelectTagsCheckBox.Checked;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < trackUrls.Length; i++)
            {
                for (int j = 0; j < reallyDisplayedTags.Count; j++)
                {
                    if (j == artworkRow)
                        SetFileTag(trackUrls[i], (MetaDataType)reallyDisplayedTags[j], (string)previewTable.Rows[j].Cells[i].Tag);
                    else
                        SetFileTag(trackUrls[i], (MetaDataType)reallyDisplayedTags[j], (string)previewTable.Rows[j].Cells[i].Value);
                }

                CommitTagsToFile(trackUrls[i]);
            }

            MbApiInterface.MB_RefreshPanels();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            saveSettings();
            Close();
        }

        private void selectTagsButton_Click(object sender, EventArgs e)
        {
            buffer.Clear();
            displayedTags = CopyTagsToClipboardCommand.SelectTags(TagToolsPlugin, SelectDisplayedTagsWindowTitle, SelectButtonName, displayedTags, true);
            fillTable(false);
        }

        private void previewTable_RowHeadersWidthChanged(object sender, EventArgs e)
        {
            if (rememberColumnAsDefaultWidthCheckBox.Checked)
            {
                //rememberColumnasDefaulltWidthCheckBox.Checked = false;

                rowHeadersWidth = previewTable.RowHeadersWidth;
            }
        }

        private void previewTable_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (rememberColumnAsDefaultWidthCheckBox.Checked)
            {
                //rememberColumnasDefaulltWidthCheckBox.Checked = false;

                for (int i = 0; i < previewTable.ColumnCount; i++)
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
                for (int j = 0; j < previewTable.RowCount; j++)
                {
                    int selectedRawCellsCount = 0;

                    for (int i = 0; i < previewTable.ColumnCount; i++)
                    {
                        if (previewTable.Rows[j].Cells[i].Selected)
                            selectedRawCellsCount++;
                    }

                    if (selectedRawCellsCount > 1)
                    {
                        for (int i = 0; i < previewTable.ColumnCount; i++)
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
                for (int j = 0; j < previewTable.RowCount; j++)
                    for (int i = 1; i < previewTable.ColumnCount; i++)
                        previewTable.Rows[j].Cells[i].Selected = false;
            }


            for (int j = 0; j < previewTable.RowCount; j++)
                previewTable.Rows[j].Cells[e.ColumnIndex].Selected = true;
        }

        private void previewTable_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void TagHistoryPlugin_Shown(object sender, EventArgs e)
        {
            previewTable.ColumnCount = 1;
            Refresh();
            fillTable(false);
        }

        private void AutoSelectTagsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            buffer.Clear();

            SavedSettings.dontAutoSelectDisplayedTags = !AutoSelectTagsCheckBox.Checked;

            if (AutoSelectTagsCheckBox.Checked)
            {
                selectTagsButton.Enable(false);

                displayedTags = new int[tagIds.Count];
                for (int i = 0; i < tagIds.Count; i++)
                    displayedTags[i] = (int)tagIds[i];
            }
            else
            {
                selectTagsButton.Enable(true);

                displayedTags = SavedSettings.displayedTags;
            }


            if (ignoreAutoSelectTagsCheckBoxCheckedEvent)
                return;


            fillTable(false);
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            buffer.Clear();

            for (int j = 0; j < previewTable.RowCount; j++)
            {
                for (int i = 0; i < previewTable.ColumnCount; i++)
                {
                    if (j == artworkRow)
                    {
                        if (previewTable.Rows[j].Cells[i].Selected)
                        {
                            Artwork = (Bitmap)previewTable.Rows[j].Cells[i].Value;
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
                        previewTable.Rows[rowIndexTagValue.Key].Cells[previewTable.CurrentCell.ColumnIndex].Value = Artwork;
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
            for (int j = 0; j < previewTable.RowCount; j++)
            {
                for (int i = 0; i < previewTable.ColumnCount; i++)
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

        private void rememberColumnasDefaulltWidthCheckBoxLabel_Click(object sender, EventArgs e)
        {
            rememberColumnAsDefaultWidthCheckBox.Checked = !rememberColumnAsDefaultWidthCheckBox.Checked;
        }
    }
}
