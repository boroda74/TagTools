﻿using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class TagHistory : PluginWindowTemplate
    {
        private CustomComboBox searchFolderTextBoxCustom;
        private CustomComboBox trackListComboBoxCustom;


        private readonly DataGridViewCellStyle headerCellStyle = new DataGridViewCellStyle(HeaderCellStyle);
        private readonly DataGridViewCellStyle unchangedCellStyle = new DataGridViewCellStyle(UnchangedCellStyle);
        private readonly DataGridViewCellStyle changedCellStyle = new DataGridViewCellStyle(ChangedCellStyle);

        private readonly string[] trackUrls;
        private readonly int[] trackIds;

        private List<string> tagNames;
        private List<MetaDataType> tagIds;
        private int[] displayedTags;
        private List<int> reallyDisplayedTags;

        private readonly Bitmap MultipleArtworks = Properties.Resources.multiple_artworks;
        private readonly Bitmap MultipleArtworksAccent = Properties.Resources.multiple_artworks_accent;

        private readonly Bitmap emptyArtwork = new Bitmap(1, 1);
        private int artworkRow;
        private readonly TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(Bitmap));

        private DataGridViewColumn columnTemplate;
        private DataGridViewCell artworkCellTemplate;

        private Color noBackupDataCellForeColor;

        private int rowHeadersWidth;
        private int defaultColumnWidth;

        private string lastSelectedFolder;

        //Arrays of tag values (dictionaries): backup # - 1 (library tag values) + 1 (current tag values) + backupTags.Count; reallyDisplayedTags.Count
        private SortedDictionary<string, bool>[,] tagValues;
        private int[,] tagHashes;

        //Array sizes: track #, 0 - all tracks; reallyDisplayedTags.Count
        private string[,] libraryTags;
        private string[,] currentTags;

        //List: backup #; Arrays: track #; reallyDisplayedTags.Count
        private List<string[,]> backupTags;

        private List<Backup> originalCachedBackups;
        private List<string> originalCachedBackupFilenames;
        private List<Backup> cachedBackups;
        private List<string> cachedBackupFilenames;
        private Backup baseline;

        private readonly string libraryName = BrGetCurrentLibraryName();

        private bool ignoreAutoSelectTagsCheckBoxCheckedEvent;

        internal TagHistory(Plugin plugin, string[] trackUrls, int[] trackIds) : base(plugin)
        {
            InitializeComponent();

            this.trackUrls = trackUrls;
            this.trackIds = trackIds;
        }

        protected override void initializeForm()
        {
            base.initializeForm();

            searchFolderTextBoxCustom = namesComboBoxes["searchFolderTextBox"];
            trackListComboBoxCustom = namesComboBoxes["trackListComboBox"];


            if (trackUrls.Length == 0)
            {
                MessageBox.Show(MbForm, MsgNoTracksSelected, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Close();
                return;
            }

            tagNames = new List<string>();
            FillListByTagNames(tagNames, false, true, false, false, false, false);

            tagIds = new List<MetaDataType>();
            for (var i = 0; i < tagNames.Count; i++)
                tagIds.Add(GetTagId(tagNames[i]));

            previewTable.TopLeftHeaderCell.Style = headerCellStyle;
            previewTable.TopLeftHeaderCell.Value = CtlTags;

            previewTable.RowHeadersDefaultCellStyle = headerCellStyle;

            previewTable.Columns[0].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[1].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[2].HeaderCell.Style = headerCellStyle;

            columnTemplate = previewTable.Columns[1].Clone() as DataGridViewColumn;
            artworkCellTemplate = previewTable.Columns[2].CellTemplate;


            var sampleColor = SystemColors.HotTrack;

            //noBackupDataCellForeColor = sampleColor;
            noBackupDataCellForeColor = GetWeightedColor(AccentColor, sampleColor);//***



            if (SavedSettings.lastSelectedFolders == null)
            {
                SavedSettings.lastSelectedFolders = new object[10];
                SavedSettings.lastSelectedFolders[0] = BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory);
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

            searchFolderTextBoxCustom.AddRange(SavedSettings.lastSelectedFolders);
            searchFolderTextBoxCustom.Text = SavedSettings.lastSelectedFolders[0] as string;

            lastSelectedFolder = SavedSettings.lastSelectedFolders[0] as string;


            numberOfBackupsNumericUpDown.Value = SavedSettings.defaultTagHistoryNumberOfBackups;


            for (var i = 0; i < trackUrls.Length; i++)
                trackListComboBoxCustom.Items.Add(GetTrackRepresentation(trackUrls[i]));

            trackListComboBoxCustom.SelectedIndex = 0;


            if (!SavedSettings.dontAutoSelectDisplayedTags || SavedSettings.displayedTags == null)
            {
                var offset = 0;
                displayedTags = new int[tagIds.Count - 1]; //-V3171
                for (var i = 0; i < tagIds.Count; i++)
                    if (tagIds[i] == MetaDataType.Artwork)
                        offset = -1;
                    else
                        displayedTags[i + offset] = (int)tagIds[i]; //-V3106

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

        private void fillTagNamesInTable(int trackIndex, bool reuseTagValues)
        {
            if (!reuseTagValues)
            {
                libraryTags = new string[trackUrls.Length + 1, reallyDisplayedTags.Count];
                currentTags = new string[trackUrls.Length + 1, reallyDisplayedTags.Count];
                backupTags = new List<string[,]>();

                tagValues = new SortedDictionary<string, bool>[cachedBackups.Count + 2, reallyDisplayedTags.Count];
                for (var i = 0; i < tagValues.GetLength(0); i++)
                    for (var j = 0; j < tagValues.GetLength(1); j++)
                        tagValues[i, j] = new SortedDictionary<string, bool>();

                tagHashes = new int[cachedBackups.Count + 2, reallyDisplayedTags.Count];


                previewTable.RowCount = 1;
                if (reallyDisplayedTags.Count > 1)
                    previewTable.RowCount = reallyDisplayedTags.Count;

                artworkRow = -1;

                for (var j = 0; j < reallyDisplayedTags.Count; j++)
                {
                    previewTable.Rows[j].HeaderCell.Style = headerCellStyle;
                    previewTable.Rows[j].HeaderCell.Value = GetTagName((MetaDataType)reallyDisplayedTags[j]);

                    if ((MetaDataType)reallyDisplayedTags[j] == MetaDataType.Artwork)
                        artworkRow = j;
                }
            }
            else
            {
                for (var j = 0; j < reallyDisplayedTags.Count; j++)
                {
                    libraryTags[0, j] = null;
                    currentTags[0, j] = null;
                }
            }

            if (trackIndex > 0)
            {
                fillTagNamesInTableInternal(trackIndex, reuseTagValues);
            }
            else
            {
                for (var i = 1; i < trackUrls.Length + 1; i++)
                    fillTagNamesInTableInternal(i, reuseTagValues);
            }

            updateCustomScrollBars(previewTable);
        }

        private void fillTagNamesInTableInternal(int trackIndex, bool reuseTagValues)
        {
            for (var j = 0; j < reallyDisplayedTags.Count; j++)
            {
                string tagValue;
                string commonTagValue;

                if (reuseTagValues)
                    tagValue = currentTags[trackIndex, j];
                else
                    tagValue = GetFileTag(trackUrls[trackIndex - 1], (MetaDataType)reallyDisplayedTags[j]);

                if (libraryTags[0, j] == null || libraryTags[0, j] == tagValue)
                    commonTagValue = tagValue;
                else
                    commonTagValue = CtlMixedValues;


                libraryTags[0, j] = commonTagValue;
                libraryTags[trackIndex, j] = tagValue;

                currentTags[0, j] = commonTagValue;
                currentTags[trackIndex, j] = tagValue;

                if (tagValue != null)
                {
                    tagValues[0, j].AddSkip(tagValue); //Library tag values
                    tagValues[1, j].AddSkip(tagValue); //Current tag values
                }


                if (artworkRow == j)
                {
                    Bitmap artwork;

                    if (commonTagValue == null)
                        artwork = emptyArtwork;
                    else if (commonTagValue == string.Empty)
                        artwork = MissingArtwork;
                    else if (commonTagValue == CtlMixedValues)
                        artwork = MultipleArtworks;
                    else
                        artwork = typeConverter.ConvertFrom(Convert.FromBase64String(tagValue)) as Bitmap;


                    previewTable.Rows[j].Cells[0] = artworkCellTemplate.Clone() as DataGridViewCell;
                    previewTable.Rows[j].Cells[0].Value = artwork;
                    previewTable.Rows[j].Cells[0].ReadOnly = true;
                }
                else
                {
                    previewTable.Rows[j].Cells[0].Value = commonTagValue;
                }
            }
        }

        private void calculateHashes(int backupIndex, int tagIndex)
        {
            //tagValues & tagHashes are arrays of tag values (dictionaries): backup #: 1 (library tag values) + 1 (current tag values) + backupTags.Count; reallyDisplayedTags.Count
            if (tagValues[backupIndex, tagIndex].Count == 0) //No backup data
            {
                tagHashes[backupIndex, tagIndex] = 0;
            }
            else
            {
                tagHashes[backupIndex, tagIndex] = 0;

                foreach (var tagValue in tagValues[backupIndex, tagIndex].Keys)
                    tagHashes[backupIndex, tagIndex] += tagValue.GetHashCode();

                tagHashes[backupIndex, tagIndex] = tagHashes[backupIndex, tagIndex].GetHashCode();

                if (tagHashes[backupIndex, tagIndex] == 0) //0 is "no backup data" only
                    tagHashes[backupIndex, tagIndex] = 1;
            }
        }

        private void fillTable(string folder, bool includeSubfolders, int maxBackupCount, int trackIndex, bool reuseCache, bool reuseTagValues)
        {
            if (reuseCache)
            {
                fillTagNamesInTable(trackIndex, reuseTagValues);

                if (!reuseTagValues)
                    previewTable.ColumnCount = 1;

                if (reallyDisplayedTags.Count == 0)
                {
                    previewTable.RowCount = 1;

                    previewTable.Rows[0].HeaderCell.Style = headerCellStyle;
                    previewTable.Rows[0].HeaderCell.Value = CtlNoDifferences;

                    if (artworkRow != 0)
                    {
                        previewTable.Rows[0].Cells[0].Value = null;
                    }
                    else
                    {
                        previewTable.Rows[0].Cells[0].Value = new Bitmap(1, 1);
                    }

                    restoreSelectedButton.Enable(false);
                    undoButton.Enable(false);
                }
                else
                {
                    for (var backupIndex = 0; backupIndex < cachedBackups.Count; backupIndex++)
                    {
                        if (trackIndex > 0)
                        {
                            fillTableInternalReuseCache(backupIndex, trackIndex);
                            fillTableInternalReuseCacheFinal(backupIndex);
                        }
                        else
                        {
                            for (var i = 1; i < trackUrls.Length + 1; i++)
                                fillTableInternalReuseCache(backupIndex, i);

                            fillTableInternalReuseCacheFinal(backupIndex);
                        }
                    }

                    previewTableFormat();

                    restoreSelectedButton.Enable(true);
                    undoButton.Enable(true);
                }
            }
            else
            {
                fillTableFillCache(folder, includeSubfolders, maxBackupCount, trackIndex);
                Enabled = true;
            }
        }

        private void fillTableInternalReuseCacheFinal(int backupIndex)
        {
            //Let's calculate tag hashes
            for (var i = 0; i < tagValues.GetLength(0); i++)
            {
                for (var j = 0; j < tagValues.GetLength(1); j++)
                    calculateHashes(i, j);
            }


            var currentBackupTags = backupTags[backupIndex];

            for (var j = 0; j < reallyDisplayedTags.Count; j++)
            {
                if (j != artworkRow)
                {
                    if (currentBackupTags[0, j] == null)
                    {
                        previewTable.Rows[j].Cells[backupIndex + 1].Value = CtlNoBackupData;
                        previewTable.Rows[j].Cells[backupIndex + 1].ToolTipText = CtlNoBackupData.Trim('(', ')');
                    }
                    else
                    {
                        previewTable.Rows[j].Cells[backupIndex + 1].Value = currentBackupTags[0, j];
                        if (currentBackupTags[0, j] == CtlMixedValues)
                            previewTable.Rows[j].Cells[backupIndex + 1].ToolTipText = CtlMixedValues.Trim('(', ')');
                        else
                            previewTable.Rows[j].Cells[backupIndex + 1].ToolTipText = currentBackupTags[0, j];
                    }
                }
                else
                {
                    Bitmap artwork;

                    if (currentBackupTags[0, j] == null)
                        artwork = emptyArtwork;
                    else if (currentBackupTags[0, j] == string.Empty)
                        artwork = MissingArtwork;
                    else if (currentBackupTags[0, j] == CtlMixedValues)
                        artwork = null; //Let's set this in CellFormat handler
                    else
                        artwork = typeConverter.ConvertFrom(Convert.FromBase64String(currentBackupTags[0, j])) as Bitmap;

                    previewTable.Rows[j].Cells[backupIndex + 1] = artworkCellTemplate.Clone() as DataGridViewCell;
                    previewTable.Rows[j].Cells[backupIndex + 1].Value = artwork;
                    previewTable.Rows[j].Cells[backupIndex + 1].ReadOnly = true;
                }
            }
        }

        private void fillTableInternalReuseCache(int backupIndex, int trackIndex)
        {
            var backup = cachedBackups[backupIndex];

            string[,] currentBackupTags;
            if (backupIndex < backupTags.Count)
                currentBackupTags = backupTags[backupIndex];
            else
                currentBackupTags = new string[trackUrls.Length + 1, reallyDisplayedTags.Count];

            for (var j = 0; j < reallyDisplayedTags.Count; j++)
            {
                var tagValue = backup.getIncValue(trackIds[trackIndex - 1], reallyDisplayedTags[j], baseline);

                if (currentBackupTags[0, j] == null)
                    currentBackupTags[0, j] = tagValue;
                else if (currentBackupTags[0, j] != tagValue)
                    currentBackupTags[0, j] = CtlMixedValues;

                currentBackupTags[trackIndex, j] = tagValue;

                if (tagValue != null)
                    tagValues[backupIndex + 2, j].AddSkip(tagValue);
            }

            if (backupIndex >= backupTags.Count)
            {
                backupTags.Add(currentBackupTags);

                var newColumn = columnTemplate.Clone() as DataGridViewColumn;
                newColumn.HeaderText = BrGetBackupDateTime(backup);
                newColumn.ToolTipText = cachedBackupFilenames[backupIndex];
                newColumn.Width = defaultColumnWidth;

                previewTable.Columns.Add(newColumn);
            }
        }

        private void fillTableFillCache(string folder, bool includeSubfolders, int maxBackupCount, int trackIndex)
        {
            if (!System.IO.Directory.Exists(folder))
            {
                MessageBox.Show(this, MsgBrFolderDoesntExists, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            var backupGuids = new SortedDictionary<string, bool>();

            foreach (var tempTrackId in trackIds)
            {
                SortedDictionary<string, bool> tempBackupGuids = Plugin.BackupIndex.getBackupGuidsForTrack(libraryName, tempTrackId);

                foreach (var tempBackupGuid in tempBackupGuids)
                    backupGuids.AddSkip(tempBackupGuid.Key);
            }

            var backupsWithNegativeDates = new SortedDictionary<int, string>();


            var backupCacheFiles = System.IO.Directory.GetFiles(folder, "*.mbc", includeSubfolders ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly);

            foreach (var backupCacheFile in backupCacheFiles)
            {
                var backupCache = BackupCache.Load(BrGetBackupFilenameWithoutExtension(backupCacheFile));
                if (backupCache == null)
                    return;

                if (backupGuids.ContainsKey(backupCache.guid))
                {
                    var negativeDate = -((backupCache.creationDate.Year * 10000 + backupCache.creationDate.Month * 100 + backupCache.creationDate.Day) * 1000000 +
                                         backupCache.creationDate.Hour * 10000 + backupCache.creationDate.Minute * 100 + backupCache.creationDate.Second);

                    backupsWithNegativeDates.Add(negativeDate, BrGetBackupFilenameWithoutExtension(backupCacheFile));
                }
            }


            baseline = Backup.Load(BrGetBackupBaselineFilename(), ".bbl");
            if (baseline == null)
            {
                Close();
                return;
            }

            cachedBackups = new List<Backup>();
            cachedBackupFilenames = new List<string>();

            var backupIndex = 0;
            foreach (var backupWithNegativeDate in backupsWithNegativeDates)
            {
                var backupFile = backupWithNegativeDate.Value;
                var backup = Backup.LoadIncrementalBackupOnly(backupFile);
                cachedBackups.Add(backup);

                cachedBackupFilenames.Add(backupFile);

                backupIndex++;

                if (backupIndex >= maxBackupCount)
                    break;
            }


            originalCachedBackups = new List<Backup>();
            foreach (var cache in cachedBackups)
                originalCachedBackups.Add(cache);

            originalCachedBackupFilenames = new List<string>();
            foreach (var cache in cachedBackupFilenames)
                originalCachedBackupFilenames.Add(cache);


            getReallyDisplayedTags(trackIndex, false);
            fillTable(folder, includeSubfolders, maxBackupCount, trackIndex, true, false);

            if (!SavedSettings.dontPlayCompletedSound)
                System.Media.SystemSounds.Asterisk.Play();
        }

        //Returns if reallyDisplayedTags is NOT changed
        private bool getReallyDisplayedTags(int trackIndex, bool compareCurrent)
        {
            int[] currentlyDisplayedTagIds = null;
            if (compareCurrent)
            {
                currentlyDisplayedTagIds = new int[reallyDisplayedTags.Count];
                reallyDisplayedTags.CopyTo(currentlyDisplayedTagIds);
            }

            reallyDisplayedTags = new List<int>();

            cachedBackups = new List<Backup>();
            foreach (var cache in originalCachedBackups)
                cachedBackups.Add(cache);

            cachedBackupFilenames = new List<string>();
            foreach (var cache in originalCachedBackupFilenames)
                cachedBackupFilenames.Add(cache);


            bool[] backupHaveDifferences = null;

            if (trackIndex > 0)
            {
                getReallyDisplayedTagsInternal(trackIndex, out backupHaveDifferences, currentlyDisplayedTagIds);
                prepareCleanupCachedBackupsInternal(trackIndex, backupHaveDifferences);
                cleanupCachedBackupsInternal(backupHaveDifferences);
            }
            else
            {
                for (var i = 1; i < trackUrls.Length + 1; i++)
                {
                    getReallyDisplayedTagsInternal(i, out backupHaveDifferences, currentlyDisplayedTagIds);
                    prepareCleanupCachedBackupsInternal(i, backupHaveDifferences);
                }

                cleanupCachedBackupsInternal(backupHaveDifferences);
            }


            if (currentlyDisplayedTagIds == null || currentlyDisplayedTagIds.Length != reallyDisplayedTags.Count)
            {
                return false;
            }
            else
            {
                for (var i = 0; i < reallyDisplayedTags.Count; i++)
                {
                    if (currentlyDisplayedTagIds[i] != reallyDisplayedTags[i])
                        return false;
                }

                return true;
            }
        }

        private void getReallyDisplayedTagsInternal(int trackIndex, out bool[] backupHaveDifferences, int[] currentlyDisplayedTagIds)
        {
            if (!SavedSettings.dontAutoSelectDisplayedTags)
            {
                var reallyDisplayedTags2 = new List<int>();


                for (var j = displayedTags.Length - 1; j >= 0; j--)
                {
                    if ((MetaDataType)displayedTags[j] == DateCreatedTagId)
                        continue;


                    for (var i = cachedBackups.Count - 1; i >= 0; i--)
                    {
                        if (GetFileTag(trackUrls[trackIndex - 1], (MetaDataType)displayedTags[j]) != cachedBackups[i].getIncValue(trackIds[trackIndex - 1], displayedTags[j], baseline))
                            reallyDisplayedTags2.AddUnique(displayedTags[j]);
                    }

                    //Maybe current tags are changed already
                    if (currentlyDisplayedTagIds != null)
                    {
                        var currentlyDisplayedTagId = Array.IndexOf(currentlyDisplayedTagIds, displayedTags[j]);

                        if (currentlyDisplayedTagId >= 0
                            && currentTags[trackIndex, currentlyDisplayedTagId] != libraryTags[trackIndex, currentlyDisplayedTagId])

                            reallyDisplayedTags2.AddUnique(displayedTags[j]);
                    }
                }


                for (var j = reallyDisplayedTags2.Count - 1; j >= 0; j--)
                    reallyDisplayedTags.AddUnique(reallyDisplayedTags2[j]);
            }
            else
            {
                for (var j = 0; j < displayedTags.Length; j++)
                    reallyDisplayedTags.AddUnique(displayedTags[j]);
            }

            backupHaveDifferences = new bool[cachedBackups.Count];
            for (var k = 0; k < cachedBackups.Count; k++)
                backupHaveDifferences[k] = false;
        }

        private void prepareCleanupCachedBackupsInternal(int trackIndex, bool[] backupHaveDifferences)
        {
            if (!SavedSettings.dontAutoSelectDisplayedTags)
            {
                for (var i = cachedBackups.Count - 1; i >= 0; i--)
                {
                    for (var j = displayedTags.Length - 1; j >= 0; j--)
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
                for (var i = cachedBackups.Count - 1; i >= 0; i--)
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

            searchFolderTextBoxCustom.Items.CopyTo(SavedSettings.lastSelectedFolders, 0);

            TagToolsPlugin.SaveSettings();
        }

        private void saveTags(int trackIndex)
        {
            for (var j = 0; j < reallyDisplayedTags.Count; j++)
                SetFileTag(trackUrls[trackIndex - 1], (MetaDataType)reallyDisplayedTags[j], currentTags[trackIndex, j]);

            CommitTagsToFile(trackUrls[trackIndex - 1]);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (trackListComboBoxCustom.SelectedIndex > 0)
            {
                saveTags(trackListComboBoxCustom.SelectedIndex);
            }
            else
            {
                for (var i = 1; i < trackUrls.Length + 1; i++)
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
            var dialog = new FolderBrowserDialog
            {
                SelectedPath = searchFolderTextBoxCustom.Text
            };

            if (dialog.ShowDialog(this) == DialogResult.Cancel) return;

            lastSelectedFolder = dialog.SelectedPath;

            CustomComboBoxLeave(searchFolderTextBoxCustom, lastSelectedFolder);

            dialog.Dispose();
        }

        private void rereadButton_Click(object sender, EventArgs e)
        {
            fillTable(searchFolderTextBoxCustom.Text, false, (int)numberOfBackupsNumericUpDown.Value, trackListComboBoxCustom.SelectedIndex, false, false);
        }

        private void restoreSelected(int trackIndex)
        {
            for (var j = 0; j < previewTable.RowCount; j++)
            {
                for (var i = 1; i < previewTable.ColumnCount; i++)
                {
                    if (j == artworkRow)
                    {
                        if (previewTable.Rows[j].Cells[i].Selected && cachedBackups[i - 1].getIncValue(trackIds[trackIndex - 1], reallyDisplayedTags[j], baseline) != null)
                        {
                            currentTags[trackIndex, j] = backupTags[i - 1][trackIndex, j];

                            tagValues[1, j].Clear();
                            foreach (var key in tagValues[i + 1, j].Keys)
                                tagValues[1, j].AddSkip(key);

                            tagHashes[1, j] = tagHashes[i + 1, j];

                            previewTableCellFormatting(trackIndex, 0, j);

                            break;
                        }
                    }
                    else
                    {
                        if (previewTable.Rows[j].Cells[i].Selected && (string)previewTable.Rows[j].Cells[i].Value != CtlNoBackupData)
                        {
                            currentTags[trackIndex, j] = backupTags[i - 1][trackIndex, j];

                            tagValues[1, j].Clear();
                            foreach (var key in tagValues[i + 1, j].Keys)
                                tagValues[1, j].AddSkip(key);

                            tagHashes[1, j] = tagHashes[i + 1, j];

                            previewTableCellFormatting(trackIndex, 0, j);

                            break;
                        }
                    }
                }
            }
        }

        private void restoreSelectedButton_Click(object sender, EventArgs e)
        {
            if (trackListComboBoxCustom.SelectedIndex > 0)
            {
                restoreSelected(trackListComboBoxCustom.SelectedIndex);
            }
            else
            {
                for (var i = 1; i < trackUrls.Length + 1; i++)
                    restoreSelected(i);
            }


            for (var j = 0; j < previewTable.RowCount; j++)
            {
                for (var i = 1; i < previewTable.ColumnCount; i++)
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
            displayedTags = CopyTagsToClipboard.SelectTags(TagToolsPlugin, SelectDisplayedTagsWindowTitle, SelectButtonName, displayedTags, SavedSettings.backupArtworks, false);
            fillTable(searchFolderTextBoxCustom.Text, false, (int)numberOfBackupsNumericUpDown.Value, trackListComboBoxCustom.SelectedIndex, false, false);
        }

        private void previewTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var newValue = previewTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as string;
            if (newValue == CtlMixedValues || e.ColumnIndex != 0)
                return;


            tagValues[1, e.RowIndex].Clear();
            tagValues[1, e.RowIndex].AddSkip(newValue);

            if (trackListComboBoxCustom.SelectedIndex > 0)
            {
                currentTags[trackListComboBoxCustom.SelectedIndex, e.RowIndex] = newValue;
            }
            else
            {
                for (var i = 0; i < currentTags.GetLength(0); i++)
                    currentTags[i, e.RowIndex] = newValue;
            }

            calculateHashes(1, e.RowIndex);
            previewTableCellFormatting(trackListComboBoxCustom.SelectedIndex, 0, e.RowIndex);
        }

        private void previewTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //Nothing at the moment
        }

        private void previewTableFormat()
        {
            for (var i = 0; i < previewTable.ColumnCount; i++)
            {
                for (var j = 0; j < previewTable.RowCount; j++)
                {
                    previewTableCellFormatting(trackListComboBoxCustom.SelectedIndex, i, j);
                }
            }

            updateCustomScrollBars(previewTable);
        }

        private void previewTableCellFormatting(int trackIndex, int columnIndex, int rowIndex)
        {
            if (tagHashes == null || columnIndex > tagHashes.GetLength(0))
                return;


            bool? allTagsAreEqual = true; //null: no backup data

            if (tagHashes[columnIndex + 1, rowIndex] == 0) //No backup data
                allTagsAreEqual = null;
            else if (tagHashes[columnIndex + 1, rowIndex] != tagHashes[0, rowIndex])
                allTagsAreEqual = false;


            string tagValue;

            if (columnIndex == 0) //Current tags
                tagValue = libraryTags[trackIndex, rowIndex];
            else //Backup tags
                tagValue = backupTags[columnIndex - 1][trackIndex, rowIndex];


            if (rowIndex != artworkRow)
            {
                if (allTagsAreEqual == null)
                {
                    previewTable.Rows[rowIndex].Cells[columnIndex].Style.ForeColor = noBackupDataCellForeColor;
                    previewTable.Rows[rowIndex].Cells[columnIndex].Style.BackColor = unchangedCellStyle.BackColor;
                }
                else if (allTagsAreEqual == true)
                {
                    previewTable.Rows[rowIndex].Cells[columnIndex].Style = unchangedCellStyle;
                }
                else
                {
                    previewTable.Rows[rowIndex].Cells[columnIndex].Style = changedCellStyle;
                }
            }
            else if (tagValue == CtlMixedValues)
            {
                Bitmap artwork;

                if (allTagsAreEqual == true)
                    artwork = MultipleArtworksAccent;
                //else if (allTagsAreEqual == null) //This never must happen
                //   artwork = emptyArtwork;
                else //if (allTagsAreEqual == false)
                    artwork = MultipleArtworks;

                if (previewTable.Rows[rowIndex].Cells[columnIndex].Value != artwork)
                {
                    previewTable.Rows[rowIndex].Cells[columnIndex].Value = artwork;

                    if (allTagsAreEqual == true)
                        previewTable.Rows[rowIndex].Cells[columnIndex].ToolTipText = CtlMixedValues.Trim('(', ')') + "\n" + CtlMixedValuesSameAsInLibrary;
                    else //if (allTagsAreEqual == false)
                        previewTable.Rows[rowIndex].Cells[columnIndex].ToolTipText = CtlMixedValues.Trim('(', ')') + "\n" + CtlMixedValuesDifferentFromLibrary;
                    //else //This never must happen
                    //   previewTable.Rows[rowIndex].Cells[columnIndex].ToolTipText = null;
                }
            }
            else if (previewTable.Rows[rowIndex].Cells[columnIndex].ToolTipText != null) //Not mixed artworks
            {
                previewTable.Rows[rowIndex].Cells[columnIndex].ToolTipText = null;
            }
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
            for (var j = 0; j < previewTable.RowCount; j++)
                previewTable.Rows[j].Cells[0].Selected = false;


            if (previewTable.SelectedCells.Count > 1)
            {
                for (var j = 0; j < previewTable.RowCount; j++)
                {
                    var selectedRawCellsCount = 0;

                    for (var i = 1; i < previewTable.ColumnCount; i++)
                    {
                        if (previewTable.Rows[j].Cells[i].Selected)
                            selectedRawCellsCount++;
                    }

                    if (selectedRawCellsCount > 1)
                    {
                        for (var i = 1; i < previewTable.ColumnCount; i++)
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
                for (var j = 0; j < previewTable.RowCount; j++)
                    for (var i = 1; i < previewTable.ColumnCount; i++)
                        previewTable.Rows[j].Cells[i].Selected = false;
            }


            for (var j = 0; j < previewTable.RowCount; j++)
                previewTable.Rows[j].Cells[e.ColumnIndex].Selected = true;
        }

        private void trackListComboBox_DropDownClosed(object sender, EventArgs e)
        {
            fillTable(searchFolderTextBoxCustom.Text, false, (int)numberOfBackupsNumericUpDown.Value, trackListComboBoxCustom.SelectedIndex,
                true, getReallyDisplayedTags(trackListComboBoxCustom.SelectedIndex, true));
        }

        private void previewTable_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void undoButton_Click(object sender, EventArgs e)
        {
            for (var j = 0; j < reallyDisplayedTags.Count; j++)
            {
                for (var i = 0; i < currentTags.GetLength(0); i++)
                    currentTags[i, j] = libraryTags[i, j];

                tagValues[1, j].Clear();
                foreach (var key in tagValues[0, j].Keys)
                    tagValues[1, j].AddSkip(key);

                tagHashes[1, j] = tagHashes[0, j];


                if (j == artworkRow)
                {
                    Bitmap artwork;

                    if (libraryTags[0, j] == string.Empty)
                        artwork = MissingArtwork;
                    else if (libraryTags[0, j] == CtlMixedValues)
                        artwork = MultipleArtworks;
                    else
                        artwork = typeConverter.ConvertFrom(Convert.FromBase64String(libraryTags[0, j])) as Bitmap;


                    previewTable.Rows[j].Cells[0].Value = artwork;
                }
                else
                {
                    previewTable.Rows[j].Cells[0].Value = libraryTags[0, j];
                }
            }

            previewTableFormat();
        }

        private void searchFolderTextBox_Leave(object sender, EventArgs e)
        {
            lastSelectedFolder = searchFolderTextBoxCustom.Text;
            CustomComboBoxLeave(searchFolderTextBoxCustom);
        }

        private void TagHistoryPlugin_Shown(object sender, EventArgs e)
        {
            Enabled = false;
            fillTable(searchFolderTextBoxCustom.Text, false, (int)numberOfBackupsNumericUpDown.Value, 0, false, false);
        }

        private void TagHistory_Load(object sender, EventArgs e)
        {
            placeholderLabel3.Visible = false;
        }

        private void autoSelectTagsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SavedSettings.dontAutoSelectDisplayedTags = !autoSelectTagsCheckBox.Checked;

            if (autoSelectTagsCheckBox.Checked)
            {
                selectTagsButton.Enable(false);

                var offset = 0;
                displayedTags = new int[tagIds.Count - 1]; //-V3171
                for (var i = 0; i < tagIds.Count; i++)
                    if (tagIds[i] == MetaDataType.Artwork)
                        offset = -1;
                    else
                        displayedTags[i + offset] = (int)tagIds[i]; //-V3106
            }
            else
            {
                selectTagsButton.Enable(true);

                displayedTags = SavedSettings.displayedTags;
            }


            if (ignoreAutoSelectTagsCheckBoxCheckedEvent)
                return;


            fillTable(searchFolderTextBoxCustom.Text, false, (int)numberOfBackupsNumericUpDown.Value, trackListComboBoxCustom.SelectedIndex, false, false);
        }

        private void rememberColumnAsDefaultWidthCheckBoxLabel_Click(object sender, EventArgs e)
        {
            rememberColumnAsDefaultWidthCheckBox.Checked = !rememberColumnAsDefaultWidthCheckBox.Checked;
        }
    }
}
