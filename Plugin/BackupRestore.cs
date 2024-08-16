using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    partial class Plugin
    {
        internal void InitBackupRestore()
        {
            PeriodicAutoBackupTimer?.Dispose();
            PeriodicAutoBackupTimer = null;

            //(Auto)backup init
            if (!SavedSettings.dontShowBackupRestore)
            {
                string autoBackupDirectoryFullPath = BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory);

                if (File.Exists(autoBackupDirectoryFullPath + @"\" + BackupIndexFileName))
                {
                    var stream = File.Open(autoBackupDirectoryFullPath + @"\" + BackupIndexFileName, FileMode.Open, FileAccess.Read, FileShare.None);
                    var file = new StreamReader(stream, Encoding.UTF8);

                    var backupSerializer = new XmlSerializer(typeof(BackupIndex));
                    DialogResult deleteAllBackups = DialogResult.No;

                    try
                    {
                        BackupIndex = (BackupIndex)backupSerializer.Deserialize(file);
                    }
                    catch
                    {
                        BackupIndex = new BackupIndex();

                        var result = (DialogResult)MbForm.Invoke(new Func<DialogResult>(() => MessageBox.Show(MbForm, MsgBrMasterBackupIndexIsCorrupted.Replace("%%BACKUP-FOLDER%%", autoBackupDirectoryFullPath), 
                            string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)));
                    }
                    finally
                    {
                        file.Close();

                        if (deleteAllBackups == DialogResult.Yes)
                        {
                            var fsEntries = Directory.EnumerateFiles(BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory));
                            foreach (string fsEntry in fsEntries)
                                File.Delete(fsEntry);

                            fsEntries = Directory.EnumerateDirectories(BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory));
                            foreach (string fsEntry in fsEntries)
                                Directory.Delete(fsEntry, true);
                        }
                    }
                }
                else
                {
                    BackupIndex = new BackupIndex();
                }
            }

            if (!SavedSettings.dontShowBackupRestore && SavedSettings.autoBackupInterval != 0)
                PeriodicAutoBackupTimer = new System.Threading.Timer(RegularAutoBackup, null, new TimeSpan(0, 0, (int)SavedSettings.autoBackupInterval * 60), new TimeSpan(0, 0, (int)SavedSettings.autoBackupInterval * 60));
        }

        internal static string BrGetCurrentLibraryName()
        {
            string libraryName = null;

            if (!SavedSettings.dontTryToGuessLibraryName)
            {
                if (MbApiInterface.Playlist_QueryPlaylists())
                {
                    var playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();

                    if (!string.IsNullOrEmpty(playlist))
                        libraryName = Regex.Replace(playlist, @"^.*\\([^\\]*)\\Playlists\\.*", "$1");
                }
            }

            if (string.IsNullOrWhiteSpace(libraryName))
                libraryName = "";
            else
                libraryName += " - ";

            return libraryName;
        }

        internal static string BrGetAutoBackupDirectory(string autoBackupDirectory)
        {
            if (autoBackupDirectory.Length > 0 && autoBackupDirectory[0] == '\\')
                autoBackupDirectory = MbApiInterface.Setting_GetPersistentStoragePath().Substring(0, 2) + autoBackupDirectory;
            else if (autoBackupDirectory.Length > 1 && autoBackupDirectory[1] != ':')
                autoBackupDirectory = MbApiInterface.Setting_GetPersistentStoragePath() + autoBackupDirectory;

            return autoBackupDirectory;
        }

        internal static string BrGetDefaultBackupFilename(string prefix)
        {
            var now = DateTime.Now;

            return BrGetCurrentLibraryName() + prefix + now.Year.ToString("D4") + "-" + now.Month.ToString("D2") + "-" + now.Day.ToString("D2")  + " "
                + now.Hour.ToString("D2") + "." + now.Minute.ToString("D2") + "." + now.Second.ToString("D2");
        }

        internal static string BrGetBackupBaselineFilename(string libraryName = null)
        {
            if (libraryName == null)
                return BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory) + @"\" + BrGetCurrentLibraryName() + "Baseline";
            else
                return BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory) + @"\" + libraryName + "Baseline";
        }

        internal static string BrGetBackupDateTime(Backup backup)
        {
            var backupDate = backup.creationDate.ToLocalTime();
            return backupDate.Year.ToString("D4") + "-" + backupDate.Month.ToString("D2") + "-" + backupDate.Day.ToString("D2")  + " "
                + backupDate.Hour.ToString("D2") + ":" + backupDate.Minute.ToString("D2"); // + "." + backupDate.Second.ToString("D2");
        }

        internal static string BrGetBackupFilenameWithoutExtension(string fullFilename)
        {
            return Regex.Replace(fullFilename, @"(.*)\..*", "$1");
        }

        internal static string BrGetBackupSafeFilenameWithoutExtension(string fullFilename)
        {
            return Regex.Replace(fullFilename, @".*\\(.*)\..*", "$1");
        }

        internal static string BrGetBackupSafeFilename(string fullFilename)
        {
            return Regex.Replace(fullFilename, @".*\\(.*)", "$1");
        }
    }

    [XmlRoot("dictionary")]
    public class SerializableDictionary<TKey, TValue> : SortedDictionary<TKey, TValue>, IXmlSerializable
    {
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            var keySerializer = new XmlSerializer(typeof(TKey));
            var valueSerializer = new XmlSerializer(typeof(TValue));

            var wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                var key = (TKey)keySerializer.Deserialize(reader);
                var value = (TValue)valueSerializer.Deserialize(reader);

                Add(key, value);
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            var keySerializer = new XmlSerializer(typeof(TKey));
            var valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (var pair in this)
            {
                keySerializer.Serialize(writer, pair.Key);
                valueSerializer.Serialize(writer, pair.Value);
            }
        }
    }

    public class BackupCache
    {
        public bool isAutoCreated;
        public DateTime creationDate;
        public Guid guid;
        public string libraryName;

        public BackupCache()
        {
            creationDate = DateTime.UtcNow;
            guid = Guid.NewGuid();
            libraryName = BrGetCurrentLibraryName();
        }

        public BackupCache(BackupCache source, bool isAutoCreated)
        {
            this.isAutoCreated = isAutoCreated;
            creationDate = source.creationDate;
            guid = source.guid;
            libraryName = source.libraryName;
        }

        internal virtual void save(string fileName)
        {
            Stream stream = File.Open(fileName + ".mbc", FileMode.Create, FileAccess.Write, FileShare.None);
            var file = new StreamWriter(stream, Encoding.UTF8);
            var serializer = new XmlSerializer(typeof(BackupCache));

            serializer.Serialize(file, this);

            file.Close();
        }

        internal static BackupCache Load(string fileName)
        {
            var stream = File.Open(fileName + ".mbc", FileMode.Open, FileAccess.Read, FileShare.None);
            var file = new StreamReader(stream, Encoding.UTF8);
            var serializer = new XmlSerializer(typeof(BackupCache));
            BackupCache backupCache;

            try
            {
                backupCache = (BackupCache)serializer.Deserialize(file);
            }
            catch
            {
                file.Close();

                MbForm.Invoke(new Action(() => {
                    MessageBox.Show(MbForm, MsgBrBackupIsCorrupted.Replace("%%FILENAME%%", fileName));
                }));

                return null;
            }

            file.Close();

            return backupCache;
        }
    }

    public class Backup : BackupCache
    {
        public List<Guid> incrementalBackups = new List<Guid>(); //This field in used by baseline files only
        public SerializableDictionary<int, SerializableDictionary<int, string>> tracks = new SerializableDictionary<int, SerializableDictionary<int, string>>();

        public Backup()
        {
            //Nothing...
        }

        public Backup(Backup backup, bool isAutoCreated) : base(backup, isAutoCreated)
        {
            //Nothing...
        }

        public Backup(bool isAutoCreated)
        {
            this.isAutoCreated = isAutoCreated;
            guid = Guid.NewGuid();
            libraryName = BrGetCurrentLibraryName();
        }

        internal override void save(string fileName)
        {
            Backup baseline;
            var incrementalBackup = new Backup(this, isAutoCreated);
            var baselineFilename = BrGetBackupBaselineFilename();

            if (File.Exists(baselineFilename + ".bbl")) //Backup baseline file exists
            {
                baseline = Load(baselineFilename, ".bbl");
                if (baseline == null)
                    return;


                var tagNames = new List<string>();
                FillListByTagNames(tagNames, false, true, false);

                var tagIds = new List<MetaDataType>();
                for (var i = 0; i < tagNames.Count; i++)
                    tagIds.Add(GetTagId(tagNames[i]));


                const int percentageStep = 5;
                var lastShownPercentage = 0;

                var fileCounter = 0;
                var totalFiles = tracks.Count;
                foreach (var track in tracks)
                {
                    var trackId = track.Key;
                    var trackNeedsToBeBackedUp = true;

                    var percentage = 100 * fileCounter / totalFiles;
                    if (lastShownPercentage + percentageStep < percentage)
                    {
                        lastShownPercentage = percentage;
                        SetStatusBarText(SbComparingTags + percentage + "%", false);
                    }

                    SerializableDictionary<int, string> tagsForIncrementalBackup;
                    if (baseline.tracks.TryGetValue(trackId, out var baselineTags)) //Found current track in baseline
                    {
                        trackNeedsToBeBackedUp = false;
                        tagsForIncrementalBackup = new SerializableDictionary<int, string>();

                        for (var m = 0; m < tagIds.Count; m++)
                        {
                            baselineTags.TryGetValue((int)tagIds[m], out var baseValue);
                            track.Value.TryGetValue((int)tagIds[m], out var backupValue);

                            if (backupValue != baseValue)
                            {
                                trackNeedsToBeBackedUp = true;
                                tagsForIncrementalBackup.Add((int)tagIds[m], backupValue);
                            }
                        }
                    }
                    else //New track
                    {
                        tagsForIncrementalBackup = new SerializableDictionary<int, string>();

                        for (var m = 0; m < tagIds.Count; m++)
                        {
                            track.Value.TryGetValue((int)tagIds[m], out var backupValue);

                            if (backupValue != null)
                                tagsForIncrementalBackup.Add((int)tagIds[m], backupValue);
                            else
                                trackNeedsToBeBackedUp = false;
                        }
                    }


                    if (trackNeedsToBeBackedUp)
                    {
                        incrementalBackup.tracks.Add(trackId, tagsForIncrementalBackup);
                        TempTracksNeededToBeBackedUp.Add(trackId, false);
                    }
                }


                if (isAutoCreated && incrementalBackup.tracks.Count == 0)
                {
                    SetStatusBarText(SbTagAutoBackupSkipped, false);
                    System.Threading.Thread.Sleep(2000);
                    SetStatusBarText(string.Empty, false);
                    return;
                }
            }
            else //Backup baseline file doesn't exist
            {
                baseline = this;

                TempTracksNeededToBeBackedUp.Clear();
                UpdatedTracksForBackupCount = 0;
                BackupIsAlwaysNeeded = true;
            }


            baseline.incrementalBackups.Add(incrementalBackup.guid);


            //Let's save baseline first
            Stream streamBbl = File.Open(baselineFilename + ".bbl", FileMode.Create, FileAccess.Write, FileShare.None);
            var fileBbl = new StreamWriter(streamBbl, Encoding.UTF8);
            var serializerBbl = new XmlSerializer(typeof(Backup));

            serializerBbl.Serialize(fileBbl, baseline);

            fileBbl.Close();



            //Let's save incremental backup
            Stream stream = File.Open(fileName + ".xml", FileMode.Create, FileAccess.Write, FileShare.None);
            var file = new StreamWriter(stream, Encoding.UTF8);
            var serializer = new XmlSerializer(typeof(Backup));

            serializer.Serialize(file, incrementalBackup);

            file.Close();


            var backupCache = new BackupCache(incrementalBackup, isAutoCreated);
            backupCache.save(fileName);

            System.Threading.Thread.Sleep(2000);

            SetStatusBarText(string.Empty, false);
        }

        internal static Backup Load(string fileName, string backupFileExtension = ".xml")
        {
            var baselineFilename = BrGetBackupBaselineFilename();

            if (!File.Exists(baselineFilename + ".bbl")) //Backup baseline file doesn't exist
            {
                MbForm.Invoke(new Action(() => {
                    MessageBox.Show(MbForm, MsgBrBackupBaselineFileDoesntExist.Replace("%%FILENAME%%", baselineFilename + ".bbl"));
                }));

                return null;
            }


            var stream = File.Open(fileName + backupFileExtension, FileMode.Open, FileAccess.Read, FileShare.None);
            var file = new StreamReader(stream, Encoding.UTF8);
            var backupSerializer = new XmlSerializer(typeof(Backup));
            Backup incrementalBackup;

            try
            {
                incrementalBackup = (Backup)backupSerializer.Deserialize(file);
            }
            catch (Exception ex)
            {
                file.Close();

                MbForm.Invoke(new Action(() => {
                    MessageBox.Show(MbForm, MsgBrBackupIsCorrupted.Replace("%%FILENAME%%", fileName) + "\n\n" + ex.Message);
                }));

                return null;
            }

            file.Close();


            if (backupFileExtension != ".bbl") //We are loading incremental backup
            {
                var baseline = Load(baselineFilename, ".bbl");
                if (baseline == null)
                    return null;

                foreach (var incTrack in incrementalBackup.tracks)
                {
                    if (baseline.tracks.TryGetValue(incTrack.Key, out var tags)) //Track exists in baseline
                    {
                        var incTags = incTrack.Value;

                        foreach (var incTagId in incTags.Keys)
                        {
                            var incValue = incTags[incTagId];

                            if (incValue != null && incValue != "_x0000_")
                                tags.AddReplace(incTagId, incValue);
                        }
                    }
                    else //Track doesn't exist in baseline
                    {
                        baseline.tracks.Add(incTrack.Key, incTrack.Value);
                    }
                }

                return baseline; //Modified baseline
            }
            else
            {
                return incrementalBackup; //Loaded backup (baseline)
            }
        }

        internal static Backup LoadIncrementalBackupOnly(string fileName)
        {
            var stream = File.Open(fileName + ".xml", FileMode.Open, FileAccess.Read, FileShare.None);
            var file = new StreamReader(stream, Encoding.UTF8);
            var backupSerializer = new XmlSerializer(typeof(Backup));
            Backup incrementalBackup;

            try
            {
                incrementalBackup = (Backup)backupSerializer.Deserialize(file);
            }
            catch (Exception ex)
            {
                file.Close();

                MbForm.Invoke(new Action(() => { MessageBox.Show(MbForm, MsgBrBackupIsCorrupted.Replace("%%FILENAME%%", fileName) + "\n\n" + ex.Message); }));

                return null;
            }

            file.Close();


            return incrementalBackup;
        }

        internal static string DecodeValue(string value)
        {
            if (value == "_x0000_")
                value = null;
            else
                value = System.Xml.XmlConvert.DecodeName(value);

            return value;
        }

        internal string getValue(int trackId, int tagId)
        {
            if (!tracks.TryGetValue(trackId, out var tags))
                return null;

            tags.TryGetValue(tagId, out var value);

            if (value != null)
                return DecodeValue(value);
            else
                return null;
        }

        internal string getIncValue(int trackId, int tagId, Backup baseline)
        {
            if (!tracks.TryGetValue(trackId, out var tags))
                return baseline.getValue(trackId, tagId);

            tags.TryGetValue(tagId, out var value);

            if (value != null)
                return DecodeValue(value);
            else
                return baseline.getValue(trackId, tagId);
        }

        internal static string EncodeValue(string value)
        {
            if (value == null)
            {
                value = "_x0000_";
            }
            else
            {
                value = System.Xml.XmlConvert.EncodeName(value);

                value = value.Replace("_x0020_", " ");
                value = value.Replace("_x0021_", "!");
                value = value.Replace("_x0022_", "\"");
                value = value.Replace("_x0023_", "#");
                value = value.Replace("_x0024_", "$");
                value = value.Replace("_x0025_", "%");
                value = value.Replace("_x0027_", "'");
                value = value.Replace("_x0028_", "(");
                value = value.Replace("_x0029_", ")");
                value = value.Replace("_x002A_", "*");
                value = value.Replace("_x002B_", "+");
                value = value.Replace("_x002C_", ",");
                value = value.Replace("_x002F_", "/");
                value = value.Replace("_x003B_", ";");
                value = value.Replace("_x003D_", "=");
                value = value.Replace("_x003F_", "?");
                value = value.Replace("_x0040_", "@");
                value = value.Replace("_x005B_", "[");
                value = value.Replace("_x005C_", "\\");
                value = value.Replace("_x005D_", "]");
                value = value.Replace("_x005E_", "^");
                value = value.Replace("_x005F_", "_");
                value = value.Replace("_x0060_", "`");
                value = value.Replace("_x007B_", "{");
                value = value.Replace("_x007C_", "|");
                value = value.Replace("_x007D_", "}");
                value = value.Replace("_x007E_", "~");
            }

            return value;
        }

        internal void setValue(string value, int trackId, int tagId)
        {
            value = EncodeValue(value);

            if (!tracks.TryGetValue(trackId, out var tags))
            {
                tags = new SerializableDictionary<int, string>();
                tracks.Add(trackId, tags);
            }

            tags.AddReplace(tagId, value);
        }
    }

    //<library name + trackId, backup Guid>
    public class BackupIndex : SerializableDictionary<string, SerializableDictionary<Guid, bool>>
    {
        public BackupIndex()
        {
            //Nothing...
        }

        internal void saveBackupAsync(object parameters)
        {
            // ReSharper disable once PossibleNullReferenceException
            var backupName = (parameters as object[])[0] as string;
            // ReSharper disable once PossibleNullReferenceException
            var statusBarText = (parameters as object[])[1] as string;
            // ReSharper disable once PossibleNullReferenceException
            var isAutoCreated = (bool)(parameters as object[])[2];
            // ReSharper disable once PossibleNullReferenceException
            var createEmptyBackup = (bool)(parameters as object[])[3];

            try
            {
                saveBackup(backupName, statusBarText, isAutoCreated, createEmptyBackup);
            }
            catch (System.Threading.ThreadAbortException)
            {
                //Nothing to do, just let's cancel the task.
            }
            catch //Maybe *baseline backup* failure. Let's restore cached values.
            {
                if (CustomTrackIdTag > 0)
                {
                    SavedSettings.useCustomTrackIdTag = UseCustomTrackIdTag;
                    SavedSettings.customTrackIdTag = CustomTrackIdTag;
                }

                throw;
            }
        }

        internal void saveBackup(string backupName, string statusBarText, bool isAutoCreated, bool createEmptyBackup)
        {
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Lowest;

            TempTracksNeededToBeBackedUp.Clear();

            string[] files;

            if (createEmptyBackup)
                files = Array.Empty<string>();
            else if (!MbApiInterface.Library_QueryFilesEx("domain=Library", out files))
                return;

            var libraryName = BrGetCurrentLibraryName();

            lock (TracksNeededToBeBackedUp)
            {
                var tagNames = new List<string>();
                FillListByTagNames(tagNames, false, true, false);

                var tagIds = new List<MetaDataType>();
                for (var i = 0; i < tagNames.Count; i++)
                    tagIds.Add(GetTagId(tagNames[i]));

                var backup = new Backup(isAutoCreated);

                var lastShownCount = 0;
                for (var fileCounter = 0; fileCounter < files.Length; fileCounter++)
                {
                    var percentage = 100 * fileCounter / files.Length;
                    if (lastShownCount < percentage)
                    {
                        lastShownCount = percentage;
                        SetStatusBarText(statusBarText  + " " + percentage + "% (" + backupName + ")", false);
                    }

                    var currentFile = files[fileCounter];
                    var trackIdString = GetPersistentTrackId(currentFile);
                    var trackId = int.Parse(trackIdString);

                    if (SavedSettings.useCustomTrackIdTag)
                    {
                        SetFileTag(currentFile, (MetaDataType)SavedSettings.customTrackIdTag, trackIdString, true);
                        CommitTagsToFile(currentFile, true, true);
                    }


                    if (!BackupIsAlwaysNeeded && !TracksNeededToBeBackedUp.ContainsKey(trackId))
                        continue;


                    var libraryTrackId = AddLibraryNameToTrackId(libraryName, trackIdString);
                    var libraryTags = GetFileTags(currentFile, tagIds);

                    for (var i = 0; i < tagIds.Count; i++)
                    {
                        if (SavedSettings.backupArtworks || tagIds[i] != MetaDataType.Artwork)
                            backup.setValue(libraryTags[i], trackId, (int)tagIds[i]);
                    }

                    if (!TryGetValue(libraryTrackId, out var trackBackups))
                    {
                        trackBackups = new SerializableDictionary<Guid, bool>();
                        Add(libraryTrackId, trackBackups);
                    }

                    trackBackups.AddSkip(backup.guid);
                }


                backup.save(backupName);


                var stream = File.Open(BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory) + @"\" + BackupIndexFileName, FileMode.Create, FileAccess.Write, FileShare.None);
                var file = new StreamWriter(stream, Encoding.UTF8);
                var backupIndexSerializer = new XmlSerializer(typeof(BackupIndex));

                backupIndexSerializer.Serialize(file, this);

                file.Close();
            }


            TracksNeededToBeBackedUp = TempTracksNeededToBeBackedUp;
            TempTracksNeededToBeBackedUp = new SortedDictionary<int, bool>();
            UpdatedTracksForBackupCount = 0;
            BackupIsAlwaysNeeded = false;

            if (!SavedSettings.dontPlayCompletedSound && !isAutoCreated)
                System.Media.SystemSounds.Asterisk.Play();

            SetStatusBarText(string.Empty, false);
        }

        internal static void LoadBackupAsync(object parameters)
        {
            var backupName = (parameters as object[])[0] as string;
            var statusBarText = (parameters as object[])[1] as string;
            var restoreForEntireLibrary = (bool)(parameters as object[])[2];
            var restoreFromAnotherLibrary = (bool)(parameters as object[])[3];

            lock (OpenedForms)
                NumberOfNativeMbBackgroundTasks++;

            try
            {
                LoadBackup(backupName, statusBarText, restoreForEntireLibrary, restoreFromAnotherLibrary);
            }
            catch (System.Threading.ThreadAbortException) 
            {
                //Nothing to do, just let's cancel the task.
            }
            catch //Maybe *baseline backup* failure. Let's restore cached values.
            {
                if (CustomTrackIdTag > 0)
                {
                    SavedSettings.useCustomTrackIdTag = UseCustomTrackIdTag;
                    SavedSettings.customTrackIdTag = CustomTrackIdTag;
                }

                throw;
            }

            lock (OpenedForms)
                NumberOfNativeMbBackgroundTasks--;
        }

        internal static void LoadBackup(string backupName, string statusBarText, bool restoreForEntireLibrary, bool restoreFromAnotherLibrary)
        {
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Lowest;

            string query;

            if (restoreForEntireLibrary)
                query = "domain=Library";
            else
                query = "domain=SelectedFiles";


            var backup = Backup.Load(backupName);
            if (backup == null)
                return;

            if (!restoreFromAnotherLibrary && backup.libraryName != BrGetCurrentLibraryName())
            {
                System.Media.SystemSounds.Hand.Play();
                SetStatusBarText(string.Empty, false);

                var result = (DialogResult)MbForm.Invoke(new Func<DialogResult>(() => MessageBox.Show(MbForm, MsgBrThisIsTheBackupOfDifferentLibrary, string.Empty,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2)));

                if (result == DialogResult.No) return;
            }


            MbApiInterface.Library_QueryFilesEx(query, out var files);
            if (files == null || files.Length == 0)
            {
                MbForm.Invoke(new Action(() =>
                {
                    MessageBox.Show(MbForm, MsgNoTracksSelected);
                }));

                return;
            }


            var tagNames = new List<string>();
            FillListByTagNames(tagNames, false, true, false);

            var tagIds = new List<MetaDataType>();
            for (var i = 0; i < tagNames.Count; i++)
                tagIds.Add(GetTagId(tagNames[i]));

            var lastShownCount = 0;
            for (var fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                var percentage = 100 * fileCounter / files.Length;
                if (lastShownCount < percentage)
                {
                    lastShownCount = percentage;
                    SetStatusBarText(statusBarText  + " " + percentage + "% (" + backupName + ")", false);
                }

                var currentFile = files[fileCounter];
                var trackId = GetPersistentTrackIdInt(currentFile, UseCustomTrackIdTag);

                var tagsWereSet = false;
                for (var i = 0; i < tagIds.Count; i++)
                {
                    var tagValue = backup.getValue(trackId, (int)tagIds[i]);

                    if (tagValue != null)
                        tagsWereSet |= SetFileTag(currentFile, tagIds[i], tagValue, true);
                }

                if (tagsWereSet)
                {
                    UpdateTrackForBackup(currentFile);
                    CommitTagsToFile(currentFile);
                }
            }


            if (!SavedSettings.dontPlayCompletedSound)
                System.Media.SystemSounds.Asterisk.Play();

            SetStatusBarText(string.Empty, false);
            RefreshPanels(true);
        }

        internal SerializableDictionary<Guid, bool> getBackupGuidsForTrack(string libraryName, int trackId)
        {
            if (!TryGetValue(AddLibraryNameToTrackId(libraryName, trackId.ToString()), out var trackBackups))
                trackBackups = new SerializableDictionary<Guid, bool>();

            return trackBackups;
        }

        internal void deleteBackup(BackupCache backupCache)
        {
            foreach (var trackBackups in Values)
                trackBackups.RemoveExisting(backupCache.guid);

            bool wereSomeChanges;

            do
            {
                wereSomeChanges = false;

                foreach (var trackBackups in this)
                {
                    if (trackBackups.Value.Count == 0)
                    {
                        Remove(trackBackups.Key);
                        wereSomeChanges = true;
                        break;
                    }
                }
            }
            while (wereSomeChanges);



            var baselineFilename = BrGetBackupBaselineFilename(backupCache.libraryName);

            if (File.Exists(baselineFilename + ".bbl")) //Backup baseline file exists
            {
                var baseline = Backup.Load(baselineFilename, ".bbl");
                if (baseline == null)
                    return;

                baseline.incrementalBackups.Remove(backupCache.guid);

                if (baseline.incrementalBackups.Count == 0) //Let's delete baseline file
                {
                    File.Delete(baselineFilename + ".bbl");
                }
                else //Let's save baseline first
                {
                    Stream streamBbl = File.Open(baselineFilename + ".bbl", FileMode.Create, FileAccess.Write, FileShare.None);
                    var fileBbl = new StreamWriter(streamBbl, Encoding.UTF8);

                    var serializerBbl = new XmlSerializer(typeof(Backup));

                    serializerBbl.Serialize(fileBbl, baseline);

                    fileBbl.Close();
                }
            }



            var stream = File.Open(BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory) + @"\" + BackupIndexFileName, FileMode.Create, FileAccess.Write, FileShare.None);
            var file = new StreamWriter(stream, Encoding.UTF8);

            var backupIndexSerializer = new XmlSerializer(typeof(BackupIndex));

            backupIndexSerializer.Serialize(file, this);

            file.Close();
        }
    }
}
