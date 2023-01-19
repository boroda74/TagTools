using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class TagHistoryCommand : PluginWindowTemplate
    {
        private string[] trackUrls;
        private int[] trackIds;

        private List<string> tagNames;
        private List<Plugin.MetaDataType> tagIds;
        private int[] displayedTags;
        private List<int> reallyDisplayedTags;

        private Bitmap emptyArtwork = new Bitmap(1, 1);
        private int artworkRow;
        private string base64Artwork;
        private TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(Bitmap));

        private DataGridViewColumn columnTemplate;
        private DataGridViewCell artworkCellTemplate;

        private Color noBackupDataCellForeColor;

        private int rowHeadersWidth;
        private int defaultColumnWidth;

        private string lastSelectedFolder;

        private string[] libraryTags;

        private List<BackupType> originalCachedBackups;
        private List<string> originalCachedBackupFilenames;
        private List<BackupType> cachedBackups;
        private List<string> cachedBackupFilenames;
        private BackupType baseline;

        private string libraryName = Plugin.GetLibraryName();

        private bool ignoreAutoSelectTagsCheckBoxCheckedEvent = false;

        public TagHistoryCommand()
        {
            InitializeComponent();
        }

        public TagHistoryCommand(Plugin tagToolsPluginParam, string[] trackUrlsParam, int[] trackIdsParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;

            trackUrls = trackUrlsParam;
            trackIds = trackIdsParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            tagNames = new List<string>();
            Plugin.FillList(tagNames, false, true, false);

            tagIds = new List<Plugin.MetaDataType>();
            for (int i = 0; i < tagNames.Count; i++)
                tagIds.Add(Plugin.GetTagId(tagNames[i]));


            columnTemplate = (DataGridViewColumn)previewTable.Columns[1].Clone();
            artworkCellTemplate = previewTable.Columns[2].CellTemplate;

            noBackupDataCellForeColor = SystemColors.HotTrack;


            if (Plugin.SavedSettings.lastSelectedFolders == null)
            {
                Plugin.SavedSettings.lastSelectedFolders = new string[10];
                Plugin.SavedSettings.lastSelectedFolders[0] = Plugin.GetAutobackupDirectory(Plugin.SavedSettings.autobackupDirectory);
                Plugin.SavedSettings.lastSelectedFolders[1] = "";
                Plugin.SavedSettings.lastSelectedFolders[2] = "";
                Plugin.SavedSettings.lastSelectedFolders[3] = "";
                Plugin.SavedSettings.lastSelectedFolders[4] = "";
                Plugin.SavedSettings.lastSelectedFolders[5] = "";
                Plugin.SavedSettings.lastSelectedFolders[6] = "";
                Plugin.SavedSettings.lastSelectedFolders[7] = "";
                Plugin.SavedSettings.lastSelectedFolders[8] = "";
                Plugin.SavedSettings.lastSelectedFolders[9] = "";
            }

            searchFolderTextBox.Items.AddRange(Plugin.SavedSettings.lastSelectedFolders);
            searchFolderTextBox.Text = Plugin.SavedSettings.lastSelectedFolders[0];

            lastSelectedFolder = Plugin.SavedSettings.lastSelectedFolders[0];


            numberOfBackupsNumericUpDown.Value = Plugin.SavedSettings.defaultTagHistoryNumberOfBackups;


            for (int i = 0; i < trackUrls.Length; i++)
                trackListComboBox.Items.Add(Plugin.GetTrackRepresentation(trackUrls[i]));

            trackListComboBox.SelectedIndex = 0;


            if (!Plugin.SavedSettings.DontAutoSelectDisplayedTags || Plugin.SavedSettings.displayedTags == null)
            {
                int offset = 0;
                displayedTags = new int[tagIds.Count - 1];
                for (int i = 0; i < tagIds.Count; i++)
                    if (tagIds[i] == Plugin.MetaDataType.Artwork)
                        offset = -1;
                    else
                        displayedTags[i + offset] = (int)tagIds[i];

                Plugin.SavedSettings.displayedTags = displayedTags;
            }
            else
            {
                displayedTags = Plugin.SavedSettings.displayedTags;
            }

            ignoreAutoSelectTagsCheckBoxCheckedEvent = true;
            AutoSelectTagsCheckBox.Checked = !Plugin.SavedSettings.DontAutoSelectDisplayedTags;
            ignoreAutoSelectTagsCheckBoxCheckedEvent = false;


            rowHeadersWidth = Plugin.SavedSettings.rowHeadersWidth;
            defaultColumnWidth = Plugin.SavedSettings.defaultColumnWidth;

            if (rowHeadersWidth < 5)
                rowHeadersWidth = 150;

            if (defaultColumnWidth < 5)
                defaultColumnWidth = 200;


            previewTable.RowHeadersWidth = rowHeadersWidth;
            previewTable.Columns[0].Width = defaultColumnWidth;
        }

        private void fillTagNamesInTable(int trackIndex)
        {
            libraryTags = new string[displayedTags.Length];

            previewTable.RowCount = 1;
            if (reallyDisplayedTags.Count > 1)
                previewTable.RowCount = reallyDisplayedTags.Count;

            artworkRow = -1;
            base64Artwork = null;

            for (int j = 0; j < reallyDisplayedTags.Count; j++)
            {
                previewTable.Rows[j].HeaderCell.Value = Plugin.GetTagName((Plugin.MetaDataType)reallyDisplayedTags[j]);

                string tagValue = Plugin.GetFileTag(trackUrls[trackIndex], (Plugin.MetaDataType)reallyDisplayedTags[j]);

                libraryTags[j] = tagValue;


                if ((Plugin.MetaDataType)reallyDisplayedTags[j] == Plugin.MetaDataType.Artwork)
                {
                    artworkRow = j;


                    Bitmap artwork;

                    base64Artwork = tagValue;

                    if (tagValue == "")
                        artwork = emptyArtwork;
                    else
                        artwork = (Bitmap)typeConverter.ConvertFrom(Convert.FromBase64String(tagValue));


                    previewTable.Rows[j].Cells[0] = (DataGridViewCell)artworkCellTemplate.Clone();
                    previewTable.Rows[j].Cells[0].Value = artwork;
                    previewTable.Rows[j].Cells[0].ReadOnly = true;
                }
                else
                {
                    previewTable.Rows[j].Cells[0].Value = tagValue;
                }
            }
        }

        private void fillTable(string folder, bool includeSubfolders, int maxBackupCount, int trackIndex, bool reuseCache)
        {
            if (reuseCache)
            {
                fillTagNamesInTable(trackIndex);

                previewTable.ColumnCount = 1;

                if (reallyDisplayedTags.Count == 0)
                {
                    previewTable.RowCount = 1;

                    previewTable.Rows[0].HeaderCell.Value = Plugin.CtlNoDifferences;

                    if (artworkRow != 0)
                    {
                        previewTable.Rows[0].Cells[0].Value = "";
                    }
                    else
                    {
                        previewTable.Rows[0].Cells[0].Value = new Bitmap(1, 1);
                    }
                }
                else
                {
                    for (int backupCount = 0; backupCount < cachedBackups.Count; backupCount++)
                    {
                        BackupType backup = cachedBackups[backupCount];

                        DataGridViewColumn newColumn = (DataGridViewColumn)columnTemplate.Clone();
                        newColumn.HeaderText = Plugin.GetBackupDateTime(backup);
                        newColumn.ToolTipText = cachedBackupFilenames[backupCount];
                        newColumn.Width = defaultColumnWidth;

                        previewTable.Columns.Add(newColumn);


                        for (int j = 0; j < reallyDisplayedTags.Count; j++)
                        {
                            string tagValue = backup.getIncValue(trackIds[trackIndex], reallyDisplayedTags[j], baseline);

                            if (j != artworkRow)
                            {
                                if (tagValue != null)
                                    previewTable.Rows[j].Cells[backupCount + 1].Value = tagValue;
                                else
                                    previewTable.Rows[j].Cells[backupCount + 1].Value = Plugin.CtlNoBackupData;

                                if (tagValue == null)
                                    previewTable.Rows[j].Cells[backupCount + 1].Style.ForeColor = noBackupDataCellForeColor;
                                else if (tagValue != (string)previewTable.Rows[j].Cells[0].Value)
                                    previewTable.Rows[j].Cells[backupCount + 1].Style.ForeColor = noBackupDataCellForeColor;
                            }
                            else
                            {
                                Bitmap artwork;

                                if (tagValue != null)
                                {
                                    if (tagValue == "")
                                        artwork = emptyArtwork;
                                    else
                                        artwork = (Bitmap)typeConverter.ConvertFrom(Convert.FromBase64String(tagValue));
                                }
                                else
                                {
                                    artwork = Plugin.MissingArtwork;
                                }


                                previewTable.Rows[j].Cells[backupCount + 1] = (DataGridViewCell)artworkCellTemplate.Clone();
                                previewTable.Rows[j].Cells[backupCount + 1].Value = artwork;
                                previewTable.Rows[j].Cells[backupCount + 1].ReadOnly = true;

                                //if (tagValue == TagToolsPlugin.ctlNoBackupData)
                                //    previewTable.Rows[j].Cells[i + 2].Style.BackColor = noBackupDataCellForeColor;
                            }
                        }
                    }
                }
            }
            else
            {
                if (!System.IO.Directory.Exists(folder))
                {
                    MessageBox.Show(this, Plugin.MsgFolderDoesntExists, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }


                SortedDictionary<string, object> backupGuids = new SortedDictionary<string, object>();

                foreach (var tempTrackId in trackIds)
                {
                    SortedDictionary<string, bool> tempBackupGuids = Plugin.BackupIndex.getBackupGuidsForTrack(libraryName, tempTrackId);

                    foreach (var tempBackupGuid in tempBackupGuids)
                    {
                        if (!backupGuids.TryGetValue(tempBackupGuid.Key, out _))
                            backupGuids.Add(tempBackupGuid.Key, null);
                    }
                }

                SortedDictionary<int, string> backupsWithNegativeDates = new SortedDictionary<int, string>();


                string[] backupCacheFiles = System.IO.Directory.GetFiles(folder, "*.mbc", includeSubfolders ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly);

                BackupCacheType backupCache = new BackupCacheType();

                foreach (string backupCacheFile in backupCacheFiles)
                {
                    backupCache = BackupCacheType.Load(Plugin.GetBackupFilenameWithoutExtension(backupCacheFile));

                    if (backupGuids.TryGetValue(backupCache.guid, out _))
                    {
                        int negativeDate = -((backupCache.creationDate.Year * 10000 + backupCache.creationDate.Month * 100 + backupCache.creationDate.Day) * 1000000 +
                            backupCache.creationDate.Hour * 10000 + backupCache.creationDate.Minute * 100 + backupCache.creationDate.Second);

                        backupsWithNegativeDates.Add(negativeDate, Plugin.GetBackupFilenameWithoutExtension(backupCacheFile));
                    }
                }


                baseline = BackupType.Load(Plugin.GetBackupBaselineFilename(), ".bbl");

                BackupType backup;

                int totalBackupCount = maxBackupCount < backupsWithNegativeDates.Count ? maxBackupCount : backupsWithNegativeDates.Count;

                cachedBackups = new List<BackupType>();
                cachedBackupFilenames = new List<string>();

                int backupCount = 0;
                foreach (var backupWithNegativeDate in backupsWithNegativeDates)
                {
                    string backupFile = backupWithNegativeDate.Value;
                    backup = BackupType.LoadIncrementalBackupOnly(backupFile);
                    cachedBackups.Add(backup);

                    cachedBackupFilenames.Add(backupFile);

                    backupCount++;

                    if (backupCount >= maxBackupCount)
                        break;
                }


                originalCachedBackups = new List<BackupType>();
                foreach (var cache in cachedBackups)
                    originalCachedBackups.Add(cache);

                originalCachedBackupFilenames = new List<string>();
                foreach (var cache in cachedBackupFilenames)
                    originalCachedBackupFilenames.Add(cache);


                getReallyDisplayedTags(trackIndex);


                fillTable(folder, includeSubfolders, maxBackupCount, trackIndex, true);

                if (!Plugin.SavedSettings.dontPlayCompletedSound)
                    System.Media.SystemSounds.Asterisk.Play();
            }
        }

        private void getReallyDisplayedTags(int trackIndex)
        {
            reallyDisplayedTags = new List<int>();

            cachedBackups = new List<BackupType>();
            foreach (var cache in originalCachedBackups)
                cachedBackups.Add(cache);

            cachedBackupFilenames = new List<string>();
            foreach (var cache in originalCachedBackupFilenames)
                cachedBackupFilenames.Add(cache);


            if (!Plugin.SavedSettings.DontAutoSelectDisplayedTags)
            {
                List<int> reallyDisplayedTags2 = new List<int>();


                for (int j = displayedTags.Length - 1; j >= 0; j--)
                {
                    for (int i = cachedBackups.Count - 1; i >= 0; i--)
                    {
                        if (Plugin.GetFileTag(trackUrls[trackIndex], (Plugin.MetaDataType)displayedTags[j]) != cachedBackups[i].getIncValue(trackIds[trackIndex], displayedTags[j], baseline))
                        {
                            if (!reallyDisplayedTags2.Contains(displayedTags[j]))
                                reallyDisplayedTags2.Add(displayedTags[j]);
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


            if (!Plugin.SavedSettings.DontAutoSelectDisplayedTags)
            {
                for (int i = cachedBackups.Count - 1; i >= 0; i--)
                {
                    bool backupHaveDifferences = false;
                    for (int j = displayedTags.Length - 1; j >= 0; j--)
                    {
                        if (Plugin.GetFileTag(trackUrls[trackIndex], (Plugin.MetaDataType)displayedTags[j]) != cachedBackups[i].getIncValue(trackIds[trackIndex], displayedTags[j], baseline))
                        {
                            backupHaveDifferences = true;
                            break;
                        }
                    }

                    if (!backupHaveDifferences)
                    {
                        cachedBackups.RemoveAt(i);
                    }
                }
            }
        }

        private void saveSettings()
        {
            Plugin.SavedSettings.defaultTagHistoryNumberOfBackups = numberOfBackupsNumericUpDown.Value;
            Plugin.SavedSettings.displayedTags = displayedTags;
            Plugin.SavedSettings.rowHeadersWidth = rowHeadersWidth;
            Plugin.SavedSettings.defaultColumnWidth = defaultColumnWidth;
            //Plugin.SavedSettings.DontAutoSelectDisplayedTags = !AutoSelectTagsCheckBox.Checked;

            searchFolderTextBox.Items.CopyTo(Plugin.SavedSettings.lastSelectedFolders, 0);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < reallyDisplayedTags.Count; j++)
            {
                if (j == artworkRow)
                    Plugin.SetFileTag(trackUrls[trackListComboBox.SelectedIndex], (Plugin.MetaDataType)reallyDisplayedTags[j], base64Artwork);
                else
                    Plugin.SetFileTag(trackUrls[trackListComboBox.SelectedIndex], (Plugin.MetaDataType)reallyDisplayedTags[j], (string)previewTable.Rows[j].Cells[0].Value);
            }

            Plugin.CommitTagsToFile(trackUrls[trackListComboBox.SelectedIndex]);
            Plugin.MbApiInterface.MB_RefreshPanels();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            saveSettings();
            Close();
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = searchFolderTextBox.Text;

            if (dialog.ShowDialog(this) == DialogResult.Cancel) return;

            lastSelectedFolder = dialog.SelectedPath;

            Plugin.ComboBoxLeave(searchFolderTextBox, lastSelectedFolder);

            dialog.Dispose();
        }

        private void rereadButton_Click(object sender, EventArgs e)
        {
            fillTable(searchFolderTextBox.Text, false, (int)numberOfBackupsNumericUpDown.Value, trackListComboBox.SelectedIndex, false);
        }

        private void restoreSelectedButton_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < previewTable.RowCount; j++)
            {
                for (int i = 1; i < previewTable.ColumnCount; i++)
                {
                    if (j == artworkRow)
                    {
                        if (previewTable.Rows[j].Cells[i].Selected && cachedBackups[i - 1].getIncValue(trackIds[trackListComboBox.SelectedIndex], reallyDisplayedTags[j], baseline) != null)
                        {
                            previewTable.Rows[j].Cells[0].Value = previewTable.Rows[j].Cells[i].Value;
                            base64Artwork = cachedBackups[i - 1].getIncValue(trackIds[trackListComboBox.SelectedIndex], reallyDisplayedTags[j], baseline);
                            break;
                        }
                    }
                    else
                    {
                        if (previewTable.Rows[j].Cells[i].Selected && (string)previewTable.Rows[j].Cells[i].Value != Plugin.CtlNoBackupData)
                        {
                            previewTable.Rows[j].Cells[0].Value = previewTable.Rows[j].Cells[i].Value;
                            break;
                        }
                    }
                }
            }
        }

        private void selectTagsButton_Click(object sender, EventArgs e)
        {
            SelectTagsPlugin selectTagsForm = new SelectTagsPlugin(TagToolsPlugin, displayedTags);
            Display(selectTagsForm, true);
            displayedTags = selectTagsForm.displayedTags;

            fillTable(searchFolderTextBox.Text, false, (int)numberOfBackupsNumericUpDown.Value, trackListComboBox.SelectedIndex, false);
        }

        private void previewTable_RowHeadersWidthChanged(object sender, EventArgs e)
        {
            if (rememberColumnasDefaulltWidthCheckBox.Checked)
            {
                //rememberColumnasDefaulltWidthCheckBox.Checked = false;

                rowHeadersWidth = previewTable.RowHeadersWidth;
            }
        }

        private void previewTable_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (rememberColumnasDefaulltWidthCheckBox.Checked)
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
            for (int j = 0; j < previewTable.RowCount; j++)
                previewTable.Rows[j].Cells[0].Selected = false;


            if (previewTable.SelectedCells.Count > 1)
            {
                for (int j = 0; j < previewTable.RowCount; j++)
                {
                    int selectedRawCellsCount = 0;

                    for (int i = 1; i < previewTable.ColumnCount; i++)
                    {
                        if (previewTable.Rows[j].Cells[i].Selected)
                            selectedRawCellsCount++;
                    }

                    if (selectedRawCellsCount > 1)
                    {
                        for (int i = 1; i < previewTable.ColumnCount; i++)
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
            if (e.ColumnIndex > 0)
            {
                for (int j = 0; j < previewTable.RowCount; j++)
                    for (int i = 1; i < previewTable.ColumnCount; i++)
                        previewTable.Rows[j].Cells[i].Selected = false;
            }


            for (int j = 0; j < previewTable.RowCount; j++)
                previewTable.Rows[j].Cells[e.ColumnIndex].Selected = true;
        }

        private void trackListComboBox_DropDownClosed(object sender, EventArgs e)
        {
            getReallyDisplayedTags(trackListComboBox.SelectedIndex);
            fillTable(searchFolderTextBox.Text, false, (int)numberOfBackupsNumericUpDown.Value, trackListComboBox.SelectedIndex, true);
        }

        private void previewTable_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void undoButton_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < reallyDisplayedTags.Count; j++)
            {
                int libIndex = 0;
                foreach (var tagId in displayedTags)
                {
                    if (tagId == reallyDisplayedTags[j])
                        libIndex = j;
                }

                previewTable.Rows[j].Cells[0].Value = libraryTags[j];


                if ((Plugin.MetaDataType)displayedTags[j] == Plugin.MetaDataType.Artwork)
                {
                    Bitmap artwork;

                    if (libraryTags[j] == "")
                        artwork = emptyArtwork;
                    else
                        artwork = (Bitmap)typeConverter.ConvertFrom(Convert.FromBase64String(libraryTags[j]));


                    base64Artwork = libraryTags[j];

                    previewTable.Rows[j].Cells[0].Value = artwork;
                }
                else
                {
                    previewTable.Rows[j].Cells[0].Value = libraryTags[j];
                }

            }
        }

        private void searchFolderTextBox_Leave(object sender, EventArgs e)
        {
            lastSelectedFolder = searchFolderTextBox.Text;
            Plugin.ComboBoxLeave(searchFolderTextBox);
        }

        private void TagHistoryPlugin_Shown(object sender, EventArgs e)
        {
            try
            {
                previewTable.ColumnCount = 1;
                Refresh();
                fillTable(searchFolderTextBox.Text, false, (int)numberOfBackupsNumericUpDown.Value, 0, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void AutoSelectTagsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Plugin.SavedSettings.DontAutoSelectDisplayedTags = !AutoSelectTagsCheckBox.Checked;

            if (AutoSelectTagsCheckBox.Checked)
            {
                selectTagsButton.Enabled = false;

                int offset = 0;
                displayedTags = new int[tagIds.Count - 1];
                for (int i = 0; i < tagIds.Count; i++)
                    if (tagIds[i] == Plugin.MetaDataType.Artwork)
                        offset = -1;
                    else
                        displayedTags[i + offset] = (int)tagIds[i];
            }
            else
            {
                selectTagsButton.Enabled = true;

                displayedTags = Plugin.SavedSettings.displayedTags;
            }


            if (ignoreAutoSelectTagsCheckBoxCheckedEvent)
                return;


            fillTable(searchFolderTextBox.Text, false, (int)numberOfBackupsNumericUpDown.Value, trackListComboBox.SelectedIndex, false);
        }
    }
}
