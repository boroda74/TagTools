using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;
using ExtensionMethods;

namespace MusicBeePlugin
{
    public partial class TagHistoryCommand : PluginWindowTemplate
    {
        private string[] trackUrls;
        private int[] trackIds;

        private List<string> tagNames;
        private List<MetaDataType> tagIds;
        private int[] displayedTags;
        private List<int> reallyDisplayedTags;

        private Bitmap emptyArtwork = new Bitmap(1, 1);
        private int artworkRow;
        private TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(Bitmap));

        private DataGridViewColumn columnTemplate;
        private DataGridViewCell artworkCellTemplate;

        private Color noBackupDataCellForeColor;

        private int rowHeadersWidth;
        private int defaultColumnWidth;

        private string lastSelectedFolder;

        private string[,] libraryTags;
        private string[,] currentTags;
        private List<string[,]> backupTags;

        private List<BackupType> originalCachedBackups;
        private List<string> originalCachedBackupFilenames;
        private List<BackupType> cachedBackups;
        private List<string> cachedBackupFilenames;
        private BackupType baseline;

        private string libraryName = GetLibraryName();

        private bool ignoreAutoSelectTagsCheckBoxCheckedEvent = false;

        public TagHistoryCommand(Plugin tagToolsPluginParam, string[] trackUrlsParam, int[] trackIdsParam) : base(tagToolsPluginParam)
        {
            InitializeComponent();

            trackUrls = trackUrlsParam;
            trackIds = trackIdsParam;
        }

        protected override void initializeForm()
        {
            base.initializeForm();

            tagNames = new List<string>();
            FillListByTagNames(tagNames, false, true, false);

            tagIds = new List<MetaDataType>();
            for (int i = 0; i < tagNames.Count; i++)
                tagIds.Add(GetTagId(tagNames[i]));


            previewTable.EnableHeadersVisualStyles = !UseMusicBeeFontSkinColors;

            previewTable.BackgroundColor = UnchangedCellStyle.BackColor;
            previewTable.DefaultCellStyle = UnchangedCellStyle;

            previewTable.Columns[0].HeaderCell.Style = HeaderCellStyle;
            previewTable.Columns[1].HeaderCell.Style = HeaderCellStyle;
            previewTable.Columns[2].HeaderCell.Style = HeaderCellStyle;

            columnTemplate = (DataGridViewColumn)previewTable.Columns[1].Clone();
            artworkCellTemplate = previewTable.Columns[2].CellTemplate;


            Color sampleColor = SystemColors.HotTrack;

            //noBackupDataCellForeColor = sampleColor;
            noBackupDataCellForeColor = GetWeightedColor(AccentColor, sampleColor);//***



            if (SavedSettings.lastSelectedFolders == null)
            {
                SavedSettings.lastSelectedFolders = new string[10];
                SavedSettings.lastSelectedFolders[0] = GetAutobackupDirectory(SavedSettings.autobackupDirectory);
                SavedSettings.lastSelectedFolders[1] = string.Empty;
                SavedSettings.lastSelectedFolders[2] = string.Empty;
                SavedSettings.lastSelectedFolders[3] = string.Empty;
                SavedSettings.lastSelectedFolders[4] = string.Empty;
                SavedSettings.lastSelectedFolders[5] = string.Empty;
                SavedSettings.lastSelectedFolders[6] = string.Empty;
                SavedSettings.lastSelectedFolders[7] = string.Empty;
                SavedSettings.lastSelectedFolders[8] = string.Empty;
                SavedSettings.lastSelectedFolders[9] = string.Empty;
            }

            searchFolderTextBox.Items.AddRange(SavedSettings.lastSelectedFolders);
            searchFolderTextBox.Text = SavedSettings.lastSelectedFolders[0];

            lastSelectedFolder = SavedSettings.lastSelectedFolders[0];


            numberOfBackupsNumericUpDown.Value = SavedSettings.defaultTagHistoryNumberOfBackups;


            for (int i = 0; i < trackUrls.Length; i++)
                trackListComboBox.Items.Add(GetTrackRepresentation(trackUrls[i]));

            trackListComboBox.SelectedIndex = 0;


            if (!SavedSettings.dontAutoSelectDisplayedTags || SavedSettings.displayedTags == null)
            {
                int offset = 0;
                displayedTags = new int[tagIds.Count - 1];
                for (int i = 0; i < tagIds.Count; i++)
                    if (tagIds[i] == MetaDataType.Artwork)
                        offset = -1;
                    else
                        displayedTags[i + offset] = (int)tagIds[i];

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

        private void fillTagNamesInTable(int trackIndex)
        {
            libraryTags = new string[trackListComboBox.Items.Count, reallyDisplayedTags.Count];
            currentTags = new string[trackListComboBox.Items.Count, reallyDisplayedTags.Count];
            backupTags = new List<string[,]>();

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

                    previewTable.Rows[j].Cells[0] = (DataGridViewCell)artworkCellTemplate.Clone();
                    previewTable.Rows[j].Cells[0].ReadOnly = true;
                }
            }

            if (trackIndex > 0)
            {
                fillTagNamesInTableInternal(trackIndex);
            }
            else
            {
                for (int i = 1; i < trackListComboBox.Items.Count; i++)
                    fillTagNamesInTableInternal(i);
            }
        }

        private void fillTagNamesInTableInternal(int trackIndex)
        {
            for (int j = 0; j < reallyDisplayedTags.Count; j++)
            {
                string tagValue = GetFileTag(trackUrls[trackIndex - 1], (MetaDataType)reallyDisplayedTags[j]);

                if (libraryTags[0, j] == null)
                    libraryTags[0, j] = tagValue;
                else if (libraryTags[0, j] != tagValue)
                    libraryTags[0, j] = "(Mixed values)";//******

                libraryTags[trackIndex, j] = tagValue;

                currentTags[0, j] = libraryTags[0, j];
                currentTags[trackIndex, j] = tagValue;

                if (artworkRow == j)
                {
                    Bitmap artwork;

                    if (libraryTags[0, j] == string.Empty)
                        artwork = emptyArtwork;
                    if (libraryTags[0, j] == "(Mixed values)")//******
                        artwork = MissingArtwork; //*****
                    else
                        artwork = (Bitmap)typeConverter.ConvertFrom(Convert.FromBase64String(tagValue));


                    previewTable.Rows[j].Cells[0] = (DataGridViewCell)artworkCellTemplate.Clone();
                    previewTable.Rows[j].Cells[0].Value = artwork;
                    previewTable.Rows[j].Cells[0].ReadOnly = true;
                }
                else
                {
                    previewTable.Rows[j].Cells[0].Value = libraryTags[0, j];
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

                    previewTable.Rows[0].HeaderCell.Value = CtlNoDifferences;

                    if (artworkRow != 0)
                    {
                        previewTable.Rows[0].Cells[0].Value = null;
                    }
                    else
                    {
                        previewTable.Rows[0].Cells[0].Value = new Bitmap(1, 1);
                    }
                }
                else
                {
                    for (int backupIndex = 0; backupIndex < cachedBackups.Count; backupIndex++)
                    {
                        if (trackIndex > 0)
                        {
                            fillTableInternalReuseCache(backupIndex, trackIndex);
                            fillTableInternalReuseCacheFinal(backupIndex);
                        }
                        else
                        {
                            for (int i = 1; i < trackListComboBox.Items.Count; i++)
                                fillTableInternalReuseCache(backupIndex, i);

                            fillTableInternalReuseCacheFinal(backupIndex);
                        }
                    }
                }
            }
            else
            {
                fillTableFillCache(folder, includeSubfolders, maxBackupCount, trackIndex);
            }
        }

        private void fillTableInternalReuseCacheFinal(int backupIndex)
        {
            var currentBackupTags = backupTags[backupIndex];

            for (int j = 0; j < reallyDisplayedTags.Count; j++)
            {
                if (j != artworkRow)
                {
                    if (currentBackupTags[0, j] != null)
                        previewTable.Rows[j].Cells[backupIndex + 1].Value = currentBackupTags[0, j];
                    else
                        previewTable.Rows[j].Cells[backupIndex + 1].Value = CtlNoBackupData;

                    if (currentBackupTags[0, j] == null)
                        previewTable.Rows[j].Cells[backupIndex + 1].Style.ForeColor = noBackupDataCellForeColor;
                    else if (currentBackupTags[0, j] != (string)previewTable.Rows[j].Cells[0].Value)
                        previewTable.Rows[j].Cells[backupIndex + 1].Style.ForeColor = noBackupDataCellForeColor;
                }
                else
                {
                    Bitmap artwork;

                    if (currentBackupTags[0, j] != null)
                    {
                        if (currentBackupTags[0, j] == string.Empty)
                            artwork = emptyArtwork;
                        else if (currentBackupTags[0, j] == "(Mixed values)") //******
                            artwork = MissingArtwork; //******
                        else
                            artwork = (Bitmap)typeConverter.ConvertFrom(Convert.FromBase64String(currentBackupTags[0, j]));
                    }
                    else
                    {
                        artwork = MissingArtwork;
                    }

                    previewTable.Rows[j].Cells[backupIndex + 1] = (DataGridViewCell)artworkCellTemplate.Clone();
                    previewTable.Rows[j].Cells[backupIndex + 1].Value = artwork;
                    previewTable.Rows[j].Cells[backupIndex + 1].ReadOnly = true;

                    //if (tagValue == TagToolsPlugin.ctlNoBackupData)
                    //    previewTable.Rows[j].Cells[i + 2].Style.BackColor = noBackupDataCellForeColor;
                }
            }
        }

        private void fillTableInternalReuseCache(int backupIndex, int trackIndex)
        {
            BackupType backup = cachedBackups[backupIndex];

            DataGridViewColumn newColumn = (DataGridViewColumn)columnTemplate.Clone();
            newColumn.HeaderText = GetBackupDateTime(backup);
            newColumn.ToolTipText = cachedBackupFilenames[backupIndex];
            newColumn.Width = defaultColumnWidth;

            previewTable.Columns.Add(newColumn);

            string[,] currentBackupTags;
            if (backupIndex < backupTags.Count)
                currentBackupTags = backupTags[backupIndex];
            else
                currentBackupTags = new string[trackListComboBox.Items.Count, reallyDisplayedTags.Count];

            for (int j = 0; j < reallyDisplayedTags.Count; j++)
            {
                string tagValue = backup.getIncValue(trackIds[trackIndex - 1], reallyDisplayedTags[j], baseline);

                if (currentBackupTags[0, j] == null)
                    currentBackupTags[0, j] = tagValue;
                else if (currentBackupTags[0, j] != tagValue)
                    currentBackupTags[0, j] = "(Mixed values)";//******

                currentBackupTags[trackIndex, j] = tagValue;
            }

            if (backupIndex >= backupTags.Count)
                backupTags.Add(currentBackupTags);
        }

        private void fillTableFillCache(string folder, bool includeSubfolders, int maxBackupCount, int trackIndex)
        {
            if (!System.IO.Directory.Exists(folder))
            {
                MessageBox.Show(this, MsgFolderDoesntExists, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            SortedDictionary<string, bool> backupGuids = new SortedDictionary<string, bool>();

            foreach (var tempTrackId in trackIds)
            {
                SortedDictionary<string, bool> tempBackupGuids = Plugin.BackupIndex.getBackupGuidsForTrack(libraryName, tempTrackId);

                foreach (var tempBackupGuid in tempBackupGuids)
                    backupGuids.AddReplace(tempBackupGuid.Key, false);
            }

            SortedDictionary<int, string> backupsWithNegativeDates = new SortedDictionary<int, string>();


            string[] backupCacheFiles = System.IO.Directory.GetFiles(folder, "*.mbc", includeSubfolders ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly);

            BackupCacheType backupCache = new BackupCacheType();

            foreach (string backupCacheFile in backupCacheFiles)
            {
                backupCache = BackupCacheType.Load(GetBackupFilenameWithoutExtension(backupCacheFile));

                if (backupGuids.Contains(backupCache.guid))
                {
                    int negativeDate = -((backupCache.creationDate.Year * 10000 + backupCache.creationDate.Month * 100 + backupCache.creationDate.Day) * 1000000 +
                        backupCache.creationDate.Hour * 10000 + backupCache.creationDate.Minute * 100 + backupCache.creationDate.Second);

                    backupsWithNegativeDates.Add(negativeDate, GetBackupFilenameWithoutExtension(backupCacheFile));
                }
            }


            baseline = BackupType.Load(GetBackupBaselineFilename(), ".bbl");

            BackupType backup;

            int totalBackupCount = maxBackupCount < backupsWithNegativeDates.Count ? maxBackupCount : backupsWithNegativeDates.Count;

            cachedBackups = new List<BackupType>();
            cachedBackupFilenames = new List<string>();

            int backupIndex = 0;
            foreach (var backupWithNegativeDate in backupsWithNegativeDates)
            {
                string backupFile = backupWithNegativeDate.Value;
                backup = BackupType.LoadIncrementalBackupOnly(backupFile);
                cachedBackups.Add(backup);

                cachedBackupFilenames.Add(backupFile);

                backupIndex++;

                if (backupIndex >= maxBackupCount)
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

            if (!SavedSettings.dontPlayCompletedSound)
                System.Media.SystemSounds.Asterisk.Play();
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

            bool[] backupHaveDifferences = null;

            if (trackIndex > 0)
            {
                getReallyDisplayedTagsInternal(trackIndex, ref backupHaveDifferences);
                prepareCleanupCachedBackupsInternal(trackIndex, backupHaveDifferences);
                cleanupCachedBackupsInternal(backupHaveDifferences);
            }
            else
            {
                for (int i = 1; i < trackListComboBox.Items.Count; i++)
                {
                    getReallyDisplayedTagsInternal(i, ref backupHaveDifferences);
                    prepareCleanupCachedBackupsInternal(i, backupHaveDifferences);
                }

                cleanupCachedBackupsInternal(backupHaveDifferences);
            }
        }

        private void getReallyDisplayedTagsInternal(int trackIndex, ref bool[] backupHaveDifferences)
        {
            if (!SavedSettings.dontAutoSelectDisplayedTags)
            {
                List<int> reallyDisplayedTags2 = new List<int>();


                for (int j = displayedTags.Length - 1; j >= 0; j--)
                {
                    for (int i = cachedBackups.Count - 1; i >= 0; i--)
                    {
                        if (GetFileTag(trackUrls[trackIndex - 1], (MetaDataType)displayedTags[j]) != cachedBackups[i].getIncValue(trackIds[trackIndex - 1], displayedTags[j], baseline))
                        {
                            if (!reallyDisplayedTags2.Contains(displayedTags[j]))
                                reallyDisplayedTags2.Add(displayedTags[j]);
                        }
                    }
                }


                for (int j = reallyDisplayedTags2.Count - 1; j >= 0; j--)
                {
                    if (!reallyDisplayedTags.Contains(reallyDisplayedTags2[j]))
                        reallyDisplayedTags.Add(reallyDisplayedTags2[j]);
                }
            }
            else
            {
                for (int j = 0; j < displayedTags.Length; j++)
                {
                    if (!reallyDisplayedTags.Contains(displayedTags[j]))
                        reallyDisplayedTags.Add(displayedTags[j]);
                }
            }

            backupHaveDifferences = new bool[cachedBackups.Count];
            for (int k = 0; k < cachedBackups.Count; k++)
                backupHaveDifferences[k] = false;
        }

        private void prepareCleanupCachedBackupsInternal(int trackIndex, bool[] backupHaveDifferences)
        {
            if (!SavedSettings.dontAutoSelectDisplayedTags)
            {
                for (int i = cachedBackups.Count - 1; i >= 0; i--)
                {
                    for (int j = displayedTags.Length - 1; j >= 0; j--)
                    {
                        if (GetFileTag(trackUrls[trackIndex - 1], (MetaDataType)displayedTags[j]) != cachedBackups[i].getIncValue(trackIds[trackIndex - 1], displayedTags[j], baseline))
                        {
                            backupHaveDifferences[i] = true;
                            break;
                        }
                    }
                }
            }
        }

        private void cleanupCachedBackupsInternal(bool[] backupHaveDifferences)
        {
            if (!SavedSettings.dontAutoSelectDisplayedTags)
            {
                for (int i = cachedBackups.Count - 1; i >= 0; i--)
                {
                    if (!backupHaveDifferences[i])
                        cachedBackups.RemoveAt(i);
                }
            }
        }

        private void saveSettings()
        {
            SavedSettings.defaultTagHistoryNumberOfBackups = numberOfBackupsNumericUpDown.Value;
            SavedSettings.displayedTags = displayedTags;
            SavedSettings.rowHeadersWidth = rowHeadersWidth;
            SavedSettings.defaultColumnWidth = defaultColumnWidth;
            //SavedSettings.DontAutoSelectDisplayedTags = !autoSelectTagsCheckBox.Checked;

            searchFolderTextBox.Items.CopyTo(SavedSettings.lastSelectedFolders, 0);

            TagToolsPlugin.SaveSettings();
        }

        private void saveTags(int trackIndex)
        {
            for (int j = 0; j < reallyDisplayedTags.Count; j++)
                SetFileTag(trackUrls[trackIndex - 1], (MetaDataType)reallyDisplayedTags[j], currentTags[trackIndex, j]);

            CommitTagsToFile(trackUrls[trackIndex - 1]);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (trackListComboBox.SelectedIndex > 0)
            {
                saveTags(trackListComboBox.SelectedIndex);
            }
            else
            {
                for (int i = 1; i < trackListComboBox.Items.Count; i++)
                    saveTags(i);
            }

            MbApiInterface.MB_RefreshPanels();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            saveSettings();
            Close();
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                SelectedPath = searchFolderTextBox.Text
            };

            if (dialog.ShowDialog(this) == DialogResult.Cancel) return;

            lastSelectedFolder = dialog.SelectedPath;

            ComboBoxLeave(searchFolderTextBox, lastSelectedFolder);

            dialog.Dispose();
        }

        private void rereadButton_Click(object sender, EventArgs e)
        {
            fillTable(searchFolderTextBox.Text, false, (int)numberOfBackupsNumericUpDown.Value, trackListComboBox.SelectedIndex, false);
        }

        private void restoreSelected(int trackIndex)
        {
            for (int j = 0; j < previewTable.RowCount; j++)
            {
                for (int i = 1; i < previewTable.ColumnCount; i++)
                {
                    if (j == artworkRow)
                    {
                        if (previewTable.Rows[j].Cells[i].Selected && cachedBackups[i - 1].getIncValue(trackIds[trackIndex - 1], reallyDisplayedTags[j], baseline) != null)
                        {
                            currentTags[trackIndex, j] = backupTags[i - 1][trackIndex, j];
                            break;
                        }
                    }
                    else
                    {
                        if (previewTable.Rows[j].Cells[i].Selected && (string)previewTable.Rows[j].Cells[i].Value != CtlNoBackupData)
                        {
                            currentTags[trackIndex, j] = backupTags[i - 1][trackIndex, j];
                            break;
                        }
                    }
                }
            }
        }

        private void restoreSelectedButton_Click(object sender, EventArgs e)
        {
            if (trackListComboBox.SelectedIndex > 0)
            {
                restoreSelected(trackListComboBox.SelectedIndex);
            }
            else
            {
                for (int i = 1; i < trackListComboBox.Items.Count; i++)
                    restoreSelected(i);
            }


            for (int j = 0; j < previewTable.RowCount; j++)
            {
                for (int i = 1; i < previewTable.ColumnCount; i++)
                {
                    if (j == artworkRow)
                    {
                        if (previewTable.Rows[j].Cells[i].Selected && previewTable.Rows[j].Cells[i].Value != emptyArtwork)
                        {
                            previewTable.Rows[j].Cells[0].Value = previewTable.Rows[j].Cells[i].Value;
                            break;
                        }
                    }
                    else
                    {
                        if (previewTable.Rows[j].Cells[i].Selected && (string)previewTable.Rows[j].Cells[i].Value != CtlNoBackupData)
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
            displayedTags = CopyTagsToClipboardCommand.SelectTags(TagToolsPlugin, SelectDisplayedTagsWindowTitle, SelectButtonName, displayedTags, true);
            fillTable(searchFolderTextBox.Text, false, (int)numberOfBackupsNumericUpDown.Value, trackListComboBox.SelectedIndex, false);
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
                for (int i = 0; i < currentTags.GetLength(0); i++)
                    currentTags[i, j] = libraryTags[i, j];

                previewTable.Rows[j].Cells[0].Value = libraryTags[0, j];


                if ((MetaDataType)reallyDisplayedTags[j] == MetaDataType.Artwork)
                {
                    Bitmap artwork;

                    if (libraryTags[0, j] == string.Empty)
                        artwork = emptyArtwork;
                    else if (libraryTags[0, j] == "(Mixed values)") //******
                        artwork = MissingArtwork; //******
                    else
                        artwork = (Bitmap)typeConverter.ConvertFrom(Convert.FromBase64String(libraryTags[0, j]));


                    previewTable.Rows[j].Cells[0].Value = artwork;
                }
                else
                {
                    previewTable.Rows[j].Cells[0].Value = libraryTags[0, j];
                }

            }
        }

        private void searchFolderTextBox_Leave(object sender, EventArgs e)
        {
            lastSelectedFolder = searchFolderTextBox.Text;
            ComboBoxLeave(searchFolderTextBox);
        }

        private void TagHistoryPlugin_Shown(object sender, EventArgs e)
        {
            previewTable.ColumnCount = 1;
            Refresh();
            fillTable(searchFolderTextBox.Text, false, (int)numberOfBackupsNumericUpDown.Value, 0, false);
        }

        private void TagHistoryCommand_Load(object sender, EventArgs e)
        {
            placeholderLabel3.Visible = false;
        }

        private void autoSelectTagsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SavedSettings.dontAutoSelectDisplayedTags = !autoSelectTagsCheckBox.Checked;

            if (autoSelectTagsCheckBox.Checked)
            {
                selectTagsButton.Enable(false);

                int offset = 0;
                displayedTags = new int[tagIds.Count - 1];
                for (int i = 0; i < tagIds.Count; i++)
                    if (tagIds[i] == MetaDataType.Artwork)
                        offset = -1;
                    else
                        displayedTags[i + offset] = (int)tagIds[i];
            }
            else
            {
                selectTagsButton.Enable(true);

                displayedTags = SavedSettings.displayedTags;
            }


            if (ignoreAutoSelectTagsCheckBoxCheckedEvent)
                return;


            fillTable(searchFolderTextBox.Text, false, (int)numberOfBackupsNumericUpDown.Value, trackListComboBox.SelectedIndex, false);
        }

        private void rememberColumnAsDefaultWidthCheckBoxLabel_Click(object sender, EventArgs e)
        {
            rememberColumnAsDefaultWidthCheckBox.Checked = !rememberColumnAsDefaultWidthCheckBox.Checked;
        }
    }
}
