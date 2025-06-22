using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using ExtensionMethods;

using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class TagHistory : PluginWindowTemplate
    {
        //Columns: backup #
        internal class Row : DataGridViewBoundColumns
        {
            public object LibraryTag;

            [DisplayName("Library")]
            public object CurrentTag { get; set; }
        }

        private CustomComboBox searchFolderComboBoxCustom;
        private CustomComboBox trackListComboBoxCustom;


        private readonly DataGridViewCellStyle headerCellStyle = new DataGridViewCellStyle(HeaderCellStyle);
        private readonly DataGridViewCellStyle unchangedCellStyle = new DataGridViewCellStyle(UnchangedCellStyle);
        private readonly DataGridViewCellStyle changedCellStyle = new DataGridViewCellStyle(ChangedCellStyle);


        //Array: track #, 0 - all tracks; List: reallyDisplayedTags.Count
        private DataGridViewBoundColumnList<Row>[] rows;
        BindingSource source = new BindingSource();

        private readonly string[] trackUrls;
        private readonly int[] trackIds;

        private List<string> tagNames;
        private List<MetaDataType> tagIds;
        private int[] displayedTags;
        private int[] displayedTagsBackup;
        private List<int> reallyDisplayedTags = new List<int>();
        private string[,] artworkTagValues; //Array: [trackUrl; library artwork, current artwork, backup artworks]

        private readonly Bitmap MixedArtworks = Properties.Resources.multiple_artworks;
        private readonly Bitmap MixedArtworksAccent = Properties.Resources.multiple_artworks_accent;

        private readonly Bitmap emptyArtwork = new Bitmap(1, 1);
        private int artworkRow;
        private readonly TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(Bitmap));

        private DataGridViewColumn libraryColumnTemplate;
        private DataGridViewColumn columnTemplate;
        private DataGridViewCell artworkCellTemplate;

        private Color noBackupDataCellForeColor;

        private int rowHeadersWidth;
        private int defaultColumnWidth;


        //Arrays of tag values (dictionaries): backup # - 1 (library tag values) + 1 (current tag values) + backupTags.Count; reallyDisplayedTags.Count
        private SortedDictionary<string, bool>[,] tagValues;
        private int[,] tagHashes;

        private string lastSelectedFolder;
        private int numberOfBackups;
        private int trackIndex = -1; //= trackListComboBoxCustom.SelectedIndex
        private bool ignoreComboBoxSelectedIndexChanged = false;

        private List<Backup> originalCachedBackups = new List<Backup>();
        private List<string> originalCachedBackupFilenames = new List<string>();
        private List<Backup> cachedBackups = new List<Backup>();
        private List<string> cachedBackupFilenames = new List<string>();
        private Backup baseline;

        private readonly string libraryName = BrGetCurrentLibraryName();

        private bool ignoreAutoSelectTagsCheckBoxCheckedEvent;

        public TagHistory(Plugin plugin, string[] trackUrls, int[] trackIds) : base(plugin)
        {
            InitializeComponent();

            WindowIcon = TagHistoryIcon;

            this.trackUrls = trackUrls;
            this.trackIds = trackIds;
        }

        internal protected override void initializeForm()
        {
            base.initializeForm();

            searchFolderComboBoxCustom = namesComboBoxes["searchFolderComboBox"];
            trackListComboBoxCustom = namesComboBoxes["trackListComboBox"];


            rows = new DataGridViewBoundColumnList<Row>[trackUrls.Length + 1];

            tagNames = new List<string>();
            FillListByTagNames(tagNames, false, true, false, false, false, false);

            tagIds = new List<MetaDataType>();
            for (var i = 0; i < tagNames.Count; i++)
                tagIds.Add(GetTagId(tagNames[i]));

            previewTable.ColumnHeadersDefaultCellStyle = headerCellStyle;
            previewTable.RowHeadersDefaultCellStyle = headerCellStyle;

            previewTable.TopLeftHeaderCell.Style = headerCellStyle;
            previewTable.TopLeftHeaderCell.Value = CtlTags;

            previewTable.RowHeadersDefaultCellStyle = headerCellStyle;

            previewTable.Columns[0].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[1].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[2].HeaderCell.Style = headerCellStyle;

            libraryColumnTemplate = previewTable.Columns[0].Clone() as DataGridViewColumn;
            columnTemplate = previewTable.Columns[1].Clone() as DataGridViewColumn;
            artworkCellTemplate = previewTable.Columns[2].CellTemplate;

            previewTable.DefaultCellStyle = unchangedCellStyle;


            var sampleColor = SystemColors.HotTrack;

            //noBackupDataCellForeColor = sampleColor;
            noBackupDataCellForeColor = GetWeightedColor(AccentColor, sampleColor);



            if (SavedSettings.thLastSelectedFolders == null)
            {
                SavedSettings.thLastSelectedFolders = new object[10];

                SavedSettings.thLastSelectedFolders[0] = BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory);
                SavedSettings.thLastSelectedFolders[1] = string.Empty;
                SavedSettings.thLastSelectedFolders[2] = string.Empty;
                SavedSettings.thLastSelectedFolders[3] = string.Empty;
                SavedSettings.thLastSelectedFolders[4] = string.Empty;
                SavedSettings.thLastSelectedFolders[5] = string.Empty;
                SavedSettings.thLastSelectedFolders[6] = string.Empty;
                SavedSettings.thLastSelectedFolders[7] = string.Empty;
                SavedSettings.thLastSelectedFolders[8] = string.Empty;
                SavedSettings.thLastSelectedFolders[9] = string.Empty;
            }

            lastSelectedFolder = SavedSettings.thLastSelectedFolders[0] as string;
            searchFolderComboBoxCustom.AddRange(SavedSettings.thLastSelectedFolders);
            searchFolderComboBoxCustom.Text = SavedSettings.thLastSelectedFolders[0] as string;


            numberOfBackupsNumericUpDown.Value = SavedSettings.thDefaultTagHistoryNumberOfBackups;


            if (SavedSettings.thDisplayedTags == null)
            {
                var offset = 0;
                displayedTags = new int[tagIds.Count - 1]; //-V3171
                for (var i = 0; i < tagIds.Count; i++)
                    if (tagIds[i] == MetaDataType.Artwork)
                        offset = -1;
                    else
                        displayedTags[i + offset] = (int)tagIds[i]; //-V3106

                SavedSettings.thDisplayedTags = displayedTags;
            }
            else
            {
                displayedTags = SavedSettings.thDisplayedTags;
            }

            for (var i = 0; i < trackUrls.Length; i++)
                trackListComboBoxCustom.Add(GetTrackRepresentation(trackUrls[i]));

            trackIndex = 0;
            trackListComboBoxCustom.SelectedIndex = 0;


            ignoreAutoSelectTagsCheckBoxCheckedEvent = true;
            autoSelectTagsCheckBox.Checked = !SavedSettings.thDontAutoSelectDisplayedTags;
            ignoreAutoSelectTagsCheckBoxCheckedEvent = false;


            rowHeadersWidth = SavedSettings.thRowHeadersWidth;
            defaultColumnWidth = SavedSettings.thDefaultColumnWidth;

            if (rowHeadersWidth < 5)
                rowHeadersWidth = 150;

            if (defaultColumnWidth < 5)
                defaultColumnWidth = 200;


            previewTable.RowHeadersWidth = rowHeadersWidth;
            previewTable.Columns[0].Width = defaultColumnWidth;


            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        //reuseTagValuesColumn == false: reread library tags of tracks; reuseTagValuesColumn == true: reuse library tags of tracks; reuseTagValuesColumn == null: reuse current tags of tracks
        private void fillAllTracksCurrentLibraryTagValues(bool fillLibraryTags, bool? reuseTagValuesColumn)
        {
            if (reallyDisplayedTags.Count == 0)
            {
                backgroundTaskIsStopping = true;
                return;
            }


            for (var j = 0; j < reallyDisplayedTags.Count; j++)
                rows[0][j].CurrentTag = null;

            for (var i = 1; i < trackUrls.Length + 1; i++)
            {
                SetStatusBarText(TagHistorySbText + TagHistorySbTextFillingLibraryTagValues + i + "/" + trackUrls.Length, true);

                for (var j = 0; j < reallyDisplayedTags.Count; j++)
                {
                    if (checkStoppingStatus())
                        return;


                    string tagValue;
                    string commonTagValue;

                    if (artworkRow != j)
                    {
                        if (reuseTagValuesColumn == null)
                            tagValue = rows[i][j].CurrentTag as string; //Reuse current tag of track
                        else if (reuseTagValuesColumn == true)
                            tagValue = rows[i][j].LibraryTag as string; //Reuse library tag of track
                        else //if (reuseTagValuesColumn == false)
                            tagValue = GetFileTag(trackUrls[i - 1], (MetaDataType)reallyDisplayedTags[j]); //Reread library tag of track

                        if (rows[0][j].CurrentTag == null || rows[0][j].CurrentTag as string == tagValue)
                            commonTagValue = tagValue;
                        else
                            commonTagValue = CtlMixedValues;


                        if (fillLibraryTags)
                        {
                            rows[0][j].LibraryTag = commonTagValue;
                            rows[i][j].LibraryTag = tagValue;
                        }

                        rows[0][j].CurrentTag = commonTagValue;
                        rows[i][j].CurrentTag = tagValue;
                    }
                    else
                    {
                        if (reuseTagValuesColumn == null) //Reuse current artwork of track
                            tagValue = artworkTagValues[i, 1];
                        else if (reuseTagValuesColumn == true)
                            tagValue = artworkTagValues[i, 0]; //Reuse library artwork of track
                        else //if (reuseTagValuesColumn == false)
                            tagValue = GetFileTag(trackUrls[i - 1], (MetaDataType)reallyDisplayedTags[j]); //Reread library artwork of track

                        if (artworkTagValues[0, 1] == null || artworkTagValues[0, 1] == tagValue) //All tracks current artwork
                            commonTagValue = tagValue;
                        else
                            commonTagValue = CtlMixedValues;

                        if (fillLibraryTags)
                        {
                            artworkTagValues[0, 0] = commonTagValue; //All tracks library artwork
                            artworkTagValues[i, 0] = commonTagValue; //Library artwork
                        }

                        artworkTagValues[0, 1] = commonTagValue; //All tracks current artwork
                        artworkTagValues[i, 1] = commonTagValue; //Current artwork


                        Bitmap artwork;
                        if (reuseTagValuesColumn == null) //Reuse current artwork of track
                            artwork = rows[i][j].CurrentTag as Bitmap;
                        else if (reuseTagValuesColumn == true) //Reuse library artwork of track
                            artwork = rows[i][j].LibraryTag as Bitmap;
                        else if (string.IsNullOrEmpty(tagValue)) //if (reuseTagValuesColumn == false) //Reread library value
                            artwork = emptyArtwork;
                        else //if (reuseTagValuesColumn == false) //Reread library value
                            artwork = typeConverter.ConvertFrom(Convert.FromBase64String(tagValue)) as Bitmap;


                        Bitmap commonArtwork;
                        if (artworkTagValues[0, 1] == null || artworkTagValues[0, 1] == tagValue) //All tracks current artwork
                            commonArtwork = artwork;
                        else
                            commonArtwork = MixedArtworks;


                        if (fillLibraryTags)
                        {
                            rows[0][j].LibraryTag = commonArtwork;
                            rows[i][j].LibraryTag = artwork;
                        }

                        rows[0][j].CurrentTag = commonArtwork;
                        rows[i][j].CurrentTag = artwork;
                    }


                    if (tagValue != null)
                    {
                        if (fillLibraryTags)
                            tagValues[0, j].AddSkip(tagValue); //Library tag values

                        tagValues[1, j].AddSkip(tagValue); //Current tag values
                    }
                }
            }
        }

        private void prepareFillTable(int trackIndex, bool reuseTagValues)
        {
            if (checkStoppingStatus())
                return;


            SetStatusBarText(TagHistorySbText + SbTextPreparingPreviewTable, false);

            if (!reuseTagValues)
            {
                tagValues = new SortedDictionary<string, bool>[cachedBackups.Count + 2, reallyDisplayedTags.Count];
                for (var i = 0; i < tagValues.GetLength(0); i++)
                    for (var j = 0; j < tagValues.GetLength(1); j++)
                        tagValues[i, j] = new SortedDictionary<string, bool>();

                tagHashes = new int[cachedBackups.Count + 2, reallyDisplayedTags.Count];


                artworkRow = -1;
                for (int j = 0; j < reallyDisplayedTags.Count; j++)
                {
                    if (checkStoppingStatus())
                        return;


                    if (reallyDisplayedTags.Count == 0)
                    {
                        SetStatusBarText(string.Empty, false);
                        backgroundTaskIsStopping = true;
                        return;
                    }

                    if ((MetaDataType)reallyDisplayedTags[j] == MetaDataType.Artwork)
                        artworkRow = j;
                }


                List<string> columnNames = new List<string>();
                for (int backupIndex = 0; backupIndex < cachedBackups.Count; backupIndex++)
                    columnNames.Add(BrGetBackupDateTime(cachedBackups[backupIndex]));

                if (trackIndex == 0)
                {
                    for (int i = 0; i < trackUrls.Length + 1; i++)
                        rows[i] = new DataGridViewBoundColumnList<Row>(columnNames).CreateRows(cachedBackups.Count, reallyDisplayedTags.Count);
                }
                else
                {
                    rows[trackIndex] = new DataGridViewBoundColumnList<Row>(columnNames).CreateRows(cachedBackups.Count, reallyDisplayedTags.Count);
                }
            }
            else
            {
                for (int j = 0; j < reallyDisplayedTags.Count; j++)
                {
                    rows[0][j].LibraryTag = null;
                    rows[0][j].CurrentTag = null;
                }
            }


            prepareFillTableInternal(reuseTagValues);

            Invoke(new Action(() =>
            {
                source.DataSource = rows[trackIndex];
                previewTable.DataSource = source;
                source.ResetBindings(true);
            }));
        }

        private void prepareFillTableInternal(bool reuseTagValues)
        {
            if (!reuseTagValues)
            {
                tagValues = new SortedDictionary<string, bool>[cachedBackups.Count + 2, reallyDisplayedTags.Count];
                for (var i = 0; i < tagValues.GetLength(0); i++)
                    for (var j = 0; j < tagValues.GetLength(1); j++)
                        tagValues[i, j] = new SortedDictionary<string, bool>();

                tagHashes = new int[cachedBackups.Count + 2, reallyDisplayedTags.Count];
            }

            fillAllTracksCurrentLibraryTagValues(true, reuseTagValues);
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

        private void fillTable(string folder, bool includeSubfolders, int maxBackupCount, bool reuseCache, bool reuseTagValues)
        {
            if (reuseCache)
            {
                Invoke(new Action(() =>
                {
                    enableDisablePreviewOptionControls(true);
                    enableQueryingOrUpdatingButtons();
                    this.Enable(true, null, null);
                }));

                prepareFillTable(trackIndex, reuseTagValues);

                if (reallyDisplayedTags.Count == 0)
                    backgroundTaskIsStopping = true;


                if (reallyDisplayedTags.Count == 0)
                {
                    Invoke(new Action(() =>
                    {
                        resetPreviewData(true);

                        if (artworkRow != 0)
                            previewTable.Rows[0].Cells[0].Value = null;
                        else
                            previewTable.Rows[0].Cells[0].Value = new Bitmap(1, 1);

                        previewTableFormat();
                    }));
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

                    Invoke(new Action(() =>
                    {
                        previewTableFormat();
                    }));
                }
            }
            else
            {
                Invoke(new Action(() =>
                {
                    enableDisablePreviewOptionControls(false);
                    disableQueryingOrUpdatingButtons();
                    this.Enable(false, null, null);
                }));

                fillTableFillCache(folder, includeSubfolders, maxBackupCount);

                if (checkStoppingStatus())
                {
                    Invoke(new Action(() =>
                    {
                        enableDisablePreviewOptionControls(false);
                        disableQueryingOrUpdatingButtons();
                        this.Enable(true, null, null);
                    }));
                }
            }
        }

        private void fillTableFillCache(string folder, bool includeSubfolders, int maxBackupCount)
        {
            if (!System.IO.Directory.Exists(folder))
            {
                Invoke(new Action(() =>
                {
                    MessageBox.Show(this, MsgBrFolderDoesntExists, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    backgroundTaskIsStopping = true;
                }));

                return;
            }


            var backupGuids = new SortedDictionary<Guid, bool>();

            for (int j = 0; j < trackIds.Length; j++)
            {
                int tempTrackId = trackIds[j];

                if (checkStoppingStatus())
                    return;


                SetStatusBarTextForFileOperations(TagHistorySbText + TagHistorySbTextEnumeratingBackups, true, j, trackIds.Length);

                SortedDictionary<Guid, bool> tempBackupGuids = Plugin.BackupIndex.getBackupGuidsForTrack(libraryName, tempTrackId);

                foreach (var tempBackupGuid in tempBackupGuids)
                    backupGuids.AddSkip(tempBackupGuid.Key);
            }

            var backupsWithNegativeDates = new SortedDictionary<int, string>();


            var backupCacheFiles = System.IO.Directory.GetFiles(folder, "*.mbc", includeSubfolders ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly);

            for (int j = 0; j < backupCacheFiles.Length; j++)
            {
                if (checkStoppingStatus())
                    return;


                string backupCacheFile = backupCacheFiles[j];

                SetStatusBarTextForFileOperations(TagHistorySbText + TagHistorySbTextLoadingBackupIndexCache, true, j, backupCacheFiles.Length);

                var backupCache = BackupCache.Load(BrGetBackupFilenameWithoutExtension(backupCacheFile));
                if (backupCache == null)
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show(this, "Can't load backup cache file \"" + backupCacheFile, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        backgroundTaskIsStopping = true;
                    }));

                    return;
                }

                if (backupGuids.ContainsKey(backupCache.guid))
                {
                    var negativeDate = -((backupCache.creationDate.Year * 10000 + backupCache.creationDate.Month * 100 + backupCache.creationDate.Day) * 1000000 +
                                         backupCache.creationDate.Hour * 10000 + backupCache.creationDate.Minute * 100 + backupCache.creationDate.Second);

                    backupsWithNegativeDates.Add(negativeDate, BrGetBackupFilenameWithoutExtension(backupCacheFile));
                }
            }


            SetStatusBarText(TagHistorySbText + TagHistorySbTextLoadingBaselineBackup, true);

            baseline = Backup.Load(BrGetBackupBaselineFilename(), ".bbl");
            if (baseline == null)
            {
                Invoke(new Action(() =>
                {
                    MessageBox.Show(this, "Can't load backup baseline file \"" + BrGetBackupBaselineFilename(), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    backgroundTaskIsStopping = true;
                }));

                return;
            }

            cachedBackups.Clear();
            cachedBackupFilenames.Clear();

            var backupIndex = 0;
            foreach (var backupWithNegativeDate in backupsWithNegativeDates)
            {
                if (checkStoppingStatus())
                    return;


                SetStatusBarTextForFileOperations(TagHistorySbText + TagHistorySbTextLoadingIncrementalBackups, true, backupIndex, backupsWithNegativeDates.Count);

                var backupFile = backupWithNegativeDate.Value;
                var backup = Backup.LoadIncrementalBackupOnly(backupFile);
                cachedBackups.Add(backup);

                cachedBackupFilenames.Add(backupFile);

                backupIndex++;

                if (backupIndex >= maxBackupCount)
                    break;
            }


            SetStatusBarText(TagHistorySbText + SbTextPerformingServiceOperations, true);

            originalCachedBackups.Clear();
            foreach (var cache in cachedBackups)
                originalCachedBackups.Add(cache);

            originalCachedBackupFilenames.Clear();
            foreach (var cache in cachedBackupFilenames)
                originalCachedBackupFilenames.Add(cache);


            getReallyDisplayedTags(trackIndex, false);

            if (reallyDisplayedTags.Count == 0)
                backgroundTaskIsStopping = true;
            else
                fillTable(folder, includeSubfolders, maxBackupCount, true, false);
        }

        private void fillTagNamesInTable()
        {
            for (var j = 0; j < reallyDisplayedTags.Count; j++)
            {
                if (checkStoppingStatus())
                    return;


                previewTable.Rows[j].HeaderCell.Style = headerCellStyle;
                previewTable.Rows[j].HeaderCell.Value = GetTagName((MetaDataType)reallyDisplayedTags[j]);
            }
        }

        private void fillTableInternalReuseCache(int backupIndex, int trackIndex)
        {
            var backup = cachedBackups[backupIndex];

            for (var j = 0; j < reallyDisplayedTags.Count; j++)
            {
                if (checkStoppingStatus())
                    return;


                string tagValue = backup.getIncValue(trackIds[trackIndex - 1], reallyDisplayedTags[j], baseline);

                Bitmap artwork = MissingArtwork;

                if (tagValue != null)
                {
                    try
                    {
                        artwork = typeConverter.ConvertFrom(Convert.FromBase64String(tagValue)) as Bitmap;
                    }
                    catch //-V3163 //-V5606
                    {
                        ; //Let's keep MissingArtwork
                    }
                }


                if (j == artworkRow && artworkTagValues[0, backupIndex + 2] == null)
                    rows[0][j].Columns[backupIndex] = artwork;
                else if (j == artworkRow && artworkTagValues[0, backupIndex + 2] != tagValue)
                    rows[0][j].Columns[backupIndex] = MixedArtworks;
                else if (rows[0][j].Columns[backupIndex] == null)
                    rows[0][j].Columns[backupIndex] = tagValue;
                else if (rows[0][j].Columns[backupIndex] as string != tagValue)
                    rows[0][j].Columns[backupIndex] = CtlMixedValues;


                if (j == artworkRow)
                    rows[trackIndex][j].Columns[backupIndex] = artwork;
                else
                    rows[trackIndex][j].Columns[backupIndex] = tagValue;

                if (tagValue != null)
                    tagValues[backupIndex + 2, j].AddSkip(tagValue);
            }
        }

        private void fillTableInternalReuseCacheFinal(int backupIndex)
        {
            if (checkStoppingStatus())
                return;


            //Let's calculate tag hashes
            for (var i = 0; i < tagValues.GetLength(0); i++)
            {
                for (var j = 0; j < tagValues.GetLength(1); j++)
                    calculateHashes(i, j);
            }


            fillTagNamesInTable();

            for (var j = 0; j < reallyDisplayedTags.Count; j++)
            {
                if (checkStoppingStatus())
                    return;


                if (j != artworkRow)
                {
                    if (rows[0][j].Columns[backupIndex] == null)
                        previewTable.Rows[j].Cells[backupIndex + 1].ToolTipText = CtlNoBackupData.Trim('(', ')');
                    else if (rows[0][j].Columns[backupIndex] as string == CtlMixedValues)
                        previewTable.Rows[j].Cells[backupIndex + 1].ToolTipText = CtlMixedValues.Trim('(', ')');
                    else
                        previewTable.Rows[j].Cells[backupIndex + 1].ToolTipText = rows[0][j].Columns[backupIndex] as string;
                }
                else
                {
                    if (artworkTagValues[0, backupIndex + 2] == null)
                        previewTable.Rows[j].Cells[backupIndex + 1].ToolTipText = CtlNoBackupData.Trim('(', ')');
                    else if (artworkTagValues[0, backupIndex + 2] == CtlMixedValues)
                        previewTable.Rows[j].Cells[backupIndex + 1].ToolTipText = CtlMixedValues.Trim('(', ')');
                    else
                        previewTable.Rows[j].Cells[backupIndex + 1].ToolTipText = null;
                }
            }
        }

        private void formatTrackColumns()
        {
            if (checkStoppingStatus())
                return;


            for (int i = 2; i < previewTable.ColumnCount; i++) //Starting with i = 2 because there is additional temporary excessive empty "Library" column #0
            {
                previewTable.Columns[i].HeaderCell.ToolTipText = cachedBackupFilenames[i - 2];
                previewTable.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                previewTable.Columns[i].Width = defaultColumnWidth;
            }
        }

        private void formatArtworkRow(int trackIndex)
        {
            if (artworkRow == -1)
                return;

            if (checkStoppingStatus())
                return;

            for (int i = 0; i < previewTable.ColumnCount; i++)
            {
                previewTable.Rows[artworkRow].Cells[i] = artworkCellTemplate.Clone() as DataGridViewCell;
                previewTable.Rows[artworkRow].Cells[i].Tag = artworkTagValues[trackIndex, i + 1];
                previewTable.Rows[artworkRow].Cells[i].ReadOnly = true;
            }
        }

        private void resetPreviewData(bool noReallyDisplayedTagsOtherwiseNoPreview)
        {
            previewTable.AllowUserToResizeColumns = false;
            previewTable.AllowUserToResizeRows = false;
            foreach (DataGridViewColumn column in previewTable.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;


            previewIsGenerated = false;
            backgroundTaskIsStopping = false;
            backgroundTaskIsStoppedOrCancelled = false;

            previewTable.DataSource = null;

            cachedBackups.Clear();
            tagValues = null;
            tagHashes = null;
            artworkRow = -1;


            buttonPreview.Text = ButtonPreviewName;

            this.Enable(true, null, null);


            previewTable.DataSource = null;
            for (int i = 0; i < rows.Length; i++)
                rows[i]?.Clear();

            previewTable.RowCount = 1;

            previewTable.Rows[0].HeaderCell.Style = headerCellStyle;

            if (noReallyDisplayedTagsOtherwiseNoPreview)
                previewTable.Rows[0].HeaderCell.Value = CtlNoDifferences;
            else
                previewTable.Rows[0].HeaderCell.Value = CtlNoPreview;

            previewTable.Rows[0].Cells[0].Value = null;


            var newColumn = libraryColumnTemplate.Clone() as DataGridViewColumn;
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


            if (previewTable.RowCount == 1 && previewTable.ColumnCount == 1 && string.IsNullOrEmpty(previewTable.Rows[0].Cells[0].Value as string))
                previewTable.Rows[0].HeaderCell.Value = CtlNoDifferences;


            backgroundTaskIsScheduled = false;
            backgroundTaskIsStopping = false;
            backgroundTaskIsStoppedOrCancelled = false;

            enableDisablePreviewOptionControls(true);

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


            backgroundTaskIsScheduled = false;
            backgroundTaskIsStopping = false;
            backgroundTaskIsStoppedOrCancelled = false;
            closeFormOnStopping = false;
            ignoreClosingForm = false;


            RefreshPanels(true);
            SetResultingSbText();

            enableDisablePreviewOptionControls(true);
            enableQueryingButtons();

            updateCustomScrollBars(previewTable);

            return true;
        }

        internal bool prepareBackgroundPreview() //-V3009
        {
            if (backgroundTaskIsStopping)
                backgroundTaskIsStoppedOrCancelled = true;

            if (previewIsGenerated)
            {
                resetPreviewData(false);
                return true;
            }

            previewIsGenerated = false;

            if (backgroundTaskIsWorking())
                return true;


            enableDisablePreviewOptionControls(false);


            previewTable.AllowUserToResizeColumns = false;
            previewTable.AllowUserToResizeRows = false;


            return true;
        }

        private void previewChanges()
        {
            previewIsGenerated = true;

            if (artworkRow != -1)
                artworkTagValues = new string[trackUrls.Length, numberOfBackups + 2]; // +library artwork, +current artwork 

            fillTable(lastSelectedFolder, false, numberOfBackups, false, false);

            Invoke(new Action(() => { formatTrackColumns(); }));

            for (var backupIndex = 0; backupIndex < cachedBackups.Count; backupIndex++)
            {
                if (checkStoppingStatus())
                {
                    Invoke(new Action(() => { stopButtonClickedMethod(prepareBackgroundPreview); }));
                    return;
                }


                if (trackIndex > 0)
                {
                    SetStatusBarTextForFileOperations(TagHistorySbText, true, 0, 1, TagToolsPlugin.CustomFunc_Name(trackUrls[trackIndex - 1]));

                    fillTableInternalReuseCache(backupIndex, trackIndex);

                    updateCustomScrollBars(previewTable, 0, -1);
                    SetResultingSbText();
                }
                else
                {
                    for (var i = 1; i < trackUrls.Length + 1; i++)
                    {
                        if (checkStoppingStatus())
                        {
                            Invoke(new Action(() => { stopButtonClickedMethod(prepareBackgroundPreview); }));
                            return;
                        }


                        fillTableInternalReuseCache(backupIndex, i);

                        Invoke(new Action(() => { updateCustomScrollBars(previewTable, 0, -1); }));
                        SetStatusBarTextForFileOperations(TagHistorySbText, true, i - 1, trackUrls.Length, TagToolsPlugin.CustomFunc_Name(trackUrls[i - 1]));
                    }
                }
            }

            Invoke(new Action(() =>
            {
                if (previewTable.ColumnCount == 1)
                    previewIsGenerated = false;

                if (previewTable.ColumnCount > 0)
                    previewTable.Columns.RemoveAt(0);

                source.ResetBindings(true);

                for (var backupIndex = 0; backupIndex < cachedBackups.Count; backupIndex++)
                {
                    if (checkStoppingStatus())
                    {
                        stopButtonClickedMethod(prepareBackgroundPreview);
                        return;
                    }

                    fillTableInternalReuseCacheFinal(backupIndex);
                }

                formatArtworkRow(trackIndex);
                previewTableFormat();
                checkStoppedStatus();
                resetFormToGeneratedPreview();
            }));
        }

        //Returns true if reallyDisplayedTags are NOT changed
        private bool getReallyDisplayedTags(int trackIndex, bool compareCurrent)
        {
            int[] currentlyDisplayedTagIds = null;
            if (compareCurrent)
            {
                currentlyDisplayedTagIds = new int[reallyDisplayedTags.Count];
                reallyDisplayedTags.CopyTo(currentlyDisplayedTagIds);
            }

            reallyDisplayedTags.Clear();

            cachedBackups = new List<Backup>();
            foreach (var cache in originalCachedBackups)
                cachedBackups.Add(cache);

            cachedBackupFilenames = new List<string>();
            foreach (var cache in originalCachedBackupFilenames)
                cachedBackupFilenames.Add(cache);


            bool[] backupHaveDifferences = new bool[cachedBackups.Count];
            for (var k = 0; k < cachedBackups.Count; k++)
                backupHaveDifferences[k] = false;

            if (trackIndex > 0)
            {
                getReallyDisplayedTagsInternal(trackIndex, ref backupHaveDifferences, currentlyDisplayedTagIds);
                prepareCleanupCachedBackupsInternal(trackIndex, backupHaveDifferences);
                cleanupCachedBackupsInternal(backupHaveDifferences);
            }
            else
            {
                for (var i = 1; i < trackUrls.Length + 1; i++)
                {
                    getReallyDisplayedTagsInternal(i, ref backupHaveDifferences, currentlyDisplayedTagIds);
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

        private void getReallyDisplayedTagsInternal(int trackIndex, ref bool[] backupHaveDifferences, int[] currentlyDisplayedTagIds)
        {
            if (autoSelectTagsCheckBox.Checked)
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
                        int currentlyDisplayedTagIndex = Array.IndexOf(currentlyDisplayedTagIds, displayedTags[j]);

                        if (currentlyDisplayedTagIndex >= 0
                            && rows[trackIndex][currentlyDisplayedTagIndex].CurrentTag != rows[trackIndex][currentlyDisplayedTagIndex].LibraryTag)

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
        }

        private void prepareCleanupCachedBackupsInternal(int trackIndex, bool[] backupHaveDifferences)
        {
            if (autoSelectTagsCheckBox.Checked)
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
            if (autoSelectTagsCheckBox.Checked)
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
            SavedSettings.thDefaultTagHistoryNumberOfBackups = numberOfBackupsNumericUpDown.Value;
            SavedSettings.thDisplayedTags = displayedTags;
            SavedSettings.thRowHeadersWidth = rowHeadersWidth;
            SavedSettings.thDefaultColumnWidth = defaultColumnWidth;
            SavedSettings.thDontAutoSelectDisplayedTags = !autoSelectTagsCheckBox.Checked;

            searchFolderComboBoxCustom.Items.CopyTo(SavedSettings.thLastSelectedFolders, 0);

            TagToolsPlugin.SaveSettings();
        }

        private void saveTags(int trackIndex)
        {
            for (var j = 0; j < reallyDisplayedTags.Count; j++)
            {
                if (j != artworkRow)
                    SetFileTag(trackUrls[trackIndex - 1], (MetaDataType)reallyDisplayedTags[j], rows[trackIndex][j].CurrentTag as string);
                else
                    SetFileTag(trackUrls[trackIndex - 1], (MetaDataType)reallyDisplayedTags[j], artworkTagValues[trackIndex, 1]);
            }

            CommitTagsToFile(trackUrls[trackIndex - 1]);
        }

        private void applyChanges()
        {
            if (trackIndex > 0)
            {
                SetStatusBarTextForFileOperations(TagHistorySbText, false, 0, 1, TagToolsPlugin.CustomFunc_Name(trackUrls[trackIndex - 1]));
                saveTags(trackIndex);
            }
            else
            {
                for (var i = 0; i < trackUrls.Length; i++)
                {
                    if (checkStoppingStatus())
                    {
                        Invoke(new Action(() => { stopButtonClickedMethod(applyingChangesStopped); }));
                        return;
                    }


                    SetStatusBarTextForFileOperations(TagHistorySbText, false, i, trackUrls.Length, TagToolsPlugin.CustomFunc_Name(trackUrls[i]));
                    saveTags(i + 1);
                }
            }

            Invoke(new Action(() => { applyingChangesStopped(); }));
        }

        internal override void enableDisablePreviewOptionControls(bool enable, bool dontChangeDisabled = false)
        {
            enable &= !backgroundTaskIsWorking();

            trackListComboBoxCustom.Enable(enable);
            searchFolderComboBoxCustom.Enable(enable);
            buttonBrowse.Enable(enable);
            numberOfBackupsNumericUpDown.Enable(enable);
            autoSelectTagsCheckBox.Enable(enable);
            buttonSelectTags.Enable(enable && !autoSelectTagsCheckBox.Checked);
            rememberColumnAsDefaultWidthCheckBox.Enable(enable);

            buttonRestoreSelected.Enable(enable && previewIsGenerated && reallyDisplayedTags.Count > 0);
            buttonUndo.Enable(enable && previewIsGenerated && reallyDisplayedTags.Count > 0);
        }

        internal override void enableQueryingOrUpdatingButtons()
        {
            buttonOK.Enable(previewIsGenerated && !backgroundTaskIsStopping && (!backgroundTaskIsWorking() || backgroundTaskIsUpdatingTags));
            buttonPreview.Enable(true);
            rereadButton.Enable(previewIsGenerated && !backgroundTaskIsStopping && !backgroundTaskIsWorking());
        }

        internal override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enable(false);
            buttonPreview.Enable(false);
            rereadButton.Enable(false);
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            ignoreClosingForm = clickOnPreviewButton(prepareBackgroundPreview, previewChanges, buttonPreview, buttonOK, buttonClose, false);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            ignoreClosingForm = true;
            previewTable.AllowUserToResizeColumns = false;
            previewTable.AllowUserToResizeRows = false;
            switchOperation(applyChanges, buttonOK, buttonOK, buttonPreview, buttonClose, true, null);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            saveSettings();
            Close();
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog
            {
                SelectedPath = searchFolderComboBoxCustom.Text
            };

            if (dialog.ShowDialog(this) == DialogResult.Cancel) return;

            lastSelectedFolder = dialog.SelectedPath;

            CustomComboBoxLeave(searchFolderComboBoxCustom, lastSelectedFolder);

            dialog.Dispose();

            resetPreviewData(false);
        }

        private void rereadButton_Click(object sender, EventArgs e)
        {
            resetPreviewData(false);
            buttonPreview_Click(null, null);
        }

        private void restoreSelected(int trackIndex)
        {
            for (var j = 0; j < previewTable.RowCount; j++)
            {
                for (var i = 1; i < previewTable.ColumnCount; i++)
                {
                    if (previewTable.Rows[j].Cells[i].Selected && cachedBackups[i - 1].getIncValue(trackIds[trackIndex - 1], reallyDisplayedTags[j], baseline) != null)
                    {
                        rows[trackIndex][j].CurrentTag = rows[trackIndex][j].Columns[i - 1];

                        if (j == artworkRow)
                            artworkTagValues[trackIndex, 1] = artworkTagValues[trackIndex, i + 1];

                        tagValues[1, j].Clear();
                        foreach (var key in tagValues[i + 1, j].Keys)
                            tagValues[1, j].AddSkip(key);

                        tagHashes[1, j] = tagHashes[i + 1, j];

                        break;
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


            fillAllTracksCurrentLibraryTagValues(false, null);
            source.ResetBindings(false);
            fillTagNamesInTable();
            previewTableFormat();

            ClickPlayer.Play();
        }

        private void undoButton_Click(object sender, EventArgs e)
        {
            for (var j = 0; j < reallyDisplayedTags.Count; j++)
            {
                for (var i = 0; i < trackUrls.Length + 1; i++)
                    rows[i][j].CurrentTag = rows[trackIndex][j].LibraryTag;

                tagValues[1, j].Clear();
                foreach (var key in tagValues[0, j].Keys)
                    tagValues[1, j].AddSkip(key);

                tagHashes[1, j] = tagHashes[0, j];


                if (j == artworkRow)
                {
                    for (var i = 0; i < trackUrls.Length + 1; i++)
                        artworkTagValues[i, 1] = artworkTagValues[i, 0];
                }
            }

            fillAllTracksCurrentLibraryTagValues(false, null);
            source.ResetBindings(false);
            fillTagNamesInTable();
            previewTableFormat();

            ClickPlayer.Play();
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


            reallyDisplayedTags.Clear();
            foreach (int tagId in displayedTags)
                reallyDisplayedTags.Add(tagId);


            enableQueryingOrUpdatingButtons();
            enableDisablePreviewOptionControls(true);
            this.Enable(true, null, null);
        }

        private void buttonSelectTags_Click(object sender, EventArgs e)
        {
            bool previewIsGeneratedBackup = previewIsGenerated;

            var newDisplayedTags = CopyTagsToClipboard.SelectTags(TagToolsPlugin, SelectDisplayedTagsWindowTitle, SelectButtonName, displayedTags, SavedSettings.backupArtworks, false);
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
                displayedTags = new int[tagIds.Count - 1]; //-V3171
                var offset = 0;
                for (var i = 0; i < tagIds.Count; i++)
                    if (tagIds[i] == MetaDataType.Artwork)
                        offset = -1;
                    else
                        displayedTags[i + offset] = (int)tagIds[i]; //-V3106


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

        private void previewTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var newValue = previewTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as string;
            if (newValue == CtlMixedValues || e.ColumnIndex != 0)
                return;

            if (tagValues.GetLength(1) > 0)
            {
                if (newValue == null)
                    newValue = string.Empty;

                tagValues[1, e.RowIndex].Clear();
                tagValues[1, e.RowIndex].AddSkip(newValue);

                if (trackListComboBoxCustom.SelectedIndex > 0)
                {
                    rows[trackListComboBoxCustom.SelectedIndex][e.RowIndex].CurrentTag = newValue;
                }
                else
                {
                    for (var i = 0; i < trackUrls.Length + 1; i++)
                        rows[i][e.RowIndex].CurrentTag = newValue;
                }

                calculateHashes(1, e.RowIndex);
                source.ResetBindings(false);

                previewTableCellFormatting(trackListComboBoxCustom.SelectedIndex, 0, e.RowIndex);
                fillTagNamesInTable();
            }
        }

        private void previewTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //Nothing at the moment...
        }

        private void previewTableFormat()
        {
            if (previewTable.ColumnCount > tagHashes.GetLength(0) - 1)
                for (var i = 1; i < previewTable.ColumnCount; i++) //Starting with i = 1 because there is additional temporary excessive empty "Library" column #0
                    for (var j = 0; j < previewTable.RowCount; j++)
                        previewTableCellFormatting(trackListComboBoxCustom.SelectedIndex, i - 1, j);
            else
                for (var i = 0; i < previewTable.ColumnCount; i++)
                    for (var j = 0; j < previewTable.RowCount; j++)
                        previewTableCellFormatting(trackListComboBoxCustom.SelectedIndex, i, j);

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


            if (rowIndex != artworkRow)
            {
                string tagValue;

                if (columnIndex == 0) //Current tags
                    tagValue = rows[trackIndex][rowIndex].CurrentTag as string;
                else //Backup tags
                    tagValue = rows[trackIndex][rowIndex].Columns[columnIndex - 1] as string;


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
            else
            {
                string tagValue = artworkTagValues[trackIndex, columnIndex + 1];

                if (tagValue == CtlMixedValues)
                {
                    Bitmap artwork;

                    if (allTagsAreEqual == true)
                        artwork = MixedArtworksAccent;
                    //else if (allTagsAreEqual == null) //This never must happen
                    //   artwork = emptyArtwork;
                    else //if (allTagsAreEqual == false)
                        artwork = MixedArtworks;

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


        private void trackListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (trackIndex != trackListComboBoxCustom.SelectedIndex)
            {
                trackIndex = trackListComboBoxCustom.SelectedIndex;
                fillTable(searchFolderComboBoxCustom.Text, false, (int)numberOfBackupsNumericUpDown.Value,
                    true, getReallyDisplayedTags(trackListComboBoxCustom.SelectedIndex, true));
            }
        }

        private void previewTable_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void searchFolderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ignoreComboBoxSelectedIndexChanged)
                return;

            ignoreComboBoxSelectedIndexChanged = true;

            if (searchFolderComboBoxCustom.Text != lastSelectedFolder)
            {
                if (string.IsNullOrEmpty(searchFolderComboBoxCustom.Text))
                {
                    searchFolderComboBoxCustom.SelectedItem = lastSelectedFolder;
                }
                else
                {
                    CustomComboBoxLeave(searchFolderComboBoxCustom);
                    lastSelectedFolder = searchFolderComboBoxCustom.Text;

                    if (previewIsGenerated)
                        resetPreviewData(false);
                }
            }

            ignoreComboBoxSelectedIndexChanged = false;
        }

        private void numberOfBackupsNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            numberOfBackups = (int)numberOfBackupsNumericUpDown.Value;
        }

        private void rememberColumnAsDefaultWidthCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!rememberColumnAsDefaultWidthCheckBox.IsEnabled())
                return;

            rememberColumnAsDefaultWidthCheckBox.Checked = !rememberColumnAsDefaultWidthCheckBox.Checked;
        }

        private void TagHistory_Load(object sender, EventArgs e)
        {
            placeholderLabel3.Visible = false;
        }

        private void TagHistory_Shown(object sender, EventArgs e)
        {
            resetPreviewData(false);
        }

        private void TagHistory_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ignoreClosingForm)
            {
                if (!backgroundTaskIsUpdatingTags)
                {
                    closeFormOnStopping = true;
                    buttonClose.Enable(false);

                    backgroundTaskIsStopping = true;
                    SetStatusBarText(TagHistorySbText + SbTextStoppingCurrentOperation, false);

                    e.Cancel = true;
                }
            }
        }
    }
}
