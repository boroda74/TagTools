using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MusicBeePlugin
{
    public partial class Plugin
    {
        public static string GetLibraryName()
        {
            string libraryName = null;

            if (!SavedSettings.dontTryToGuessLibraryName)
            {
                if (MbApiInterface.Playlist_QueryPlaylists())
                {
                    string playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();

                    if (!string.IsNullOrEmpty(playlist))
                    {
                        libraryName = Regex.Replace(playlist, @".*\\([^\\]*)\\Playlists\\.*", "$1");
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(libraryName))
                libraryName = string.Empty;
            else
                libraryName += " - ";

            return libraryName;
        }

        public static string GetAutobackupDirectory(string autobackupDirectory)
        {
            if (autobackupDirectory.Length > 0 && autobackupDirectory[0] == '\\')
            {
                autobackupDirectory = MbApiInterface.Setting_GetPersistentStoragePath().Substring(0, 2) + autobackupDirectory;
            }
            else if (autobackupDirectory.Length > 1 && autobackupDirectory[1] != ':')
            {
                autobackupDirectory = MbApiInterface.Setting_GetPersistentStoragePath() + autobackupDirectory;
            }

            return autobackupDirectory;
        }

        public static string GetPersistentTrackId(string currentFile)
        {
            string id = MbApiInterface.Library_GetDevicePersistentId(currentFile, DeviceIdType.MusicBeeNativeId);

            if (id == null)
                id = "-1";

            return id;
        }

        public static string AddLibraryNameToTrackId(string libraryName, string trackId)
        {
            return libraryName + "%%%%" + trackId;
        }

        public static string GetDefaultBackupFilename(string prefix)
        {
            DateTime now = DateTime.Now;

            return GetLibraryName() + prefix + now.Year.ToString("D4") + "-" + now.Month.ToString("D2") + "-" + now.Day.ToString("D2") + " " + now.Hour.ToString("D2") + "." + now.Minute.ToString("D2") + "." + now.Second.ToString("D2");
        }

        public static string GetBackupBaselineFilename(string libraryName = null)
        {
            if (libraryName == null)
                return GetAutobackupDirectory(SavedSettings.autobackupDirectory) + @"\" + GetLibraryName() + "Baseline";
            else
                return GetAutobackupDirectory(SavedSettings.autobackupDirectory) + @"\" + libraryName + "Baseline";
        }

        public static string GetBackupDateTime(BackupType backup)
        {
            DateTime backupDate = backup.creationDate.ToLocalTime();
            return backupDate.Year.ToString("D4") + "-" + backupDate.Month.ToString("D2") + "-" + backupDate.Day.ToString("D2") + " " + backupDate.Hour.ToString("D2") + ":" + backupDate.Minute.ToString("D2"); // + "." + backupDate.Second.ToString("D2");
        }

        public static string GetBackupFilenameWithoutExtension(string fullFilename)
        {
            return Regex.Replace(fullFilename, @"(.*)\..*", "$1");
        }

        public static string GetBackupSafeFilenameWithoutExtension(string fullFilename)
        {
            return Regex.Replace(fullFilename, @".*\\(.*)\..*", "$1");
        }

        public static string GetBackupSafeFilename(string fullFilename)
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
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                TKey key = (TKey)keySerializer.Deserialize(reader);
                TValue value = (TValue)valueSerializer.Deserialize(reader);

                Add(key, value);
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                keySerializer.Serialize(writer, pair.Key);
                valueSerializer.Serialize(writer, pair.Value);
            }
        }
    }

    public class BackupCacheType
    {
        public bool isAutocreated;
        public DateTime creationDate;
        public string guid;
        public string libraryName;

        public BackupCacheType() : base()
        {
            creationDate = DateTime.UtcNow;
            guid = Guid.NewGuid().ToString();
            libraryName = Plugin.GetLibraryName();
        }

        public BackupCacheType(BackupCacheType source, bool isAutocreatedParam) : base()
        {
            isAutocreated = isAutocreatedParam;
            creationDate = source.creationDate;
            guid = source.guid;
            libraryName = source.libraryName;
        }

        public virtual void save(string fileName)
        {
            System.IO.Stream stream = System.IO.File.Open(fileName + ".mbc", System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
            System.IO.StreamWriter file = new System.IO.StreamWriter(stream, Encoding.UTF8);

            XmlSerializer serializer = new XmlSerializer(typeof(BackupCacheType));

            serializer.Serialize(file, this);

            file.Close();
        }

        public static BackupCacheType Load(string fileName, string backupFileExtension = ".xml")
        {
            System.IO.FileStream stream = System.IO.File.Open(fileName + ".mbc", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);
            System.IO.StreamReader file = new System.IO.StreamReader(stream, Encoding.UTF8);

            XmlSerializer serializer = new XmlSerializer(typeof(BackupCacheType));

            BackupCacheType backupCache;
            try
            {
                backupCache = (BackupCacheType)serializer.Deserialize(file);
            }
            catch
            {
                file.Close();

                MessageBox.Show(Plugin.MbForm, Plugin.MsgBackupIsCorrupted.Replace("%%FILENAME%%", fileName), string.Empty, 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return null;
            }

            file.Close();

            return backupCache;
        }
    }

    public class BackupType : BackupCacheType
    {
        public List<string> incrementalBackups = new List<string>(); //This field in used by baseline files only
        public SerializableDictionary<int, SerializableDictionary<int, string>> tracks = new SerializableDictionary<int, SerializableDictionary<int, string>>();

        public BackupType() : base()
        {
            return;
        }

        public BackupType(BackupType backup, bool isAutocreatedParam) : base(backup, isAutocreatedParam)
        {
            return;
        }

        public BackupType(bool isAutocreatedParam) : base()
        {
            isAutocreated = isAutocreatedParam;
            guid = Guid.NewGuid().ToString();
            libraryName = Plugin.GetLibraryName();
        }

        public override void save(string fileName)
        {
            BackupType baseline;
            BackupType incrementalBackup = new BackupType(this, isAutocreated);
            string baselineFilename = Plugin.GetBackupBaselineFilename();

            if (System.IO.File.Exists(baselineFilename + ".bbl")) //Backup baseline file exists
            {
                baseline = Load(baselineFilename, ".bbl");
                if (baseline == null)
                    return;


                List<string> tagNames = new List<string>();
                Plugin.FillListByTagNames(tagNames, false, true, false);

                List<Plugin.MetaDataType> tagIds = new List<Plugin.MetaDataType>();
                for (int i = 0; i < tagNames.Count; i++)
                    tagIds.Add(Plugin.GetTagId(tagNames[i]));


                SerializableDictionary<int, string> tagsForIncrementalBackup;


                int fileCounter = 0;
                int totalFiles = tracks.Count;
                foreach (var track in tracks)
                {
                    int trackId = track.Key;

                    bool trackNeedsToBeBackuped = true;
                    SerializableDictionary<int, string> baselineTags;

                    int lastShownCount = 0;
                    int percentage = 100 * fileCounter / totalFiles;
                    if (lastShownCount < percentage)
                    {
                        lastShownCount = percentage;
                        Plugin.MbApiInterface.MB_SetBackgroundTaskMessage(Plugin.SbCompairingTags + percentage + "%");
                    }

                    if (baseline.tracks.TryGetValue(trackId, out baselineTags)) //Found current track in baseline
                    {
                        trackNeedsToBeBackuped = false;
                        tagsForIncrementalBackup = new SerializableDictionary<int, string>();

                        for (int m = 0; m < tagIds.Count; m++)
                        {
                            baselineTags.TryGetValue((int)tagIds[m], out string baseValue);
                            track.Value.TryGetValue((int)tagIds[m], out string backupValue);

                            if (backupValue != baseValue)
                            {
                                trackNeedsToBeBackuped = true;
                                tagsForIncrementalBackup.Add((int)tagIds[m], backupValue);
                            }
                        }
                    }
                    else //New track
                    {
                        tagsForIncrementalBackup = new SerializableDictionary<int, string>();

                        for (int m = 0; m < tagIds.Count; m++)
                        {
                            string backupValue;
                            track.Value.TryGetValue((int)tagIds[m], out backupValue);

                            if (backupValue != null)
                            {
                                trackNeedsToBeBackuped = true;
                                tagsForIncrementalBackup.Add((int)tagIds[m], backupValue);
                            }
                        }
                    }


                    if (trackNeedsToBeBackuped)
                    {
                        incrementalBackup.tracks.Add(trackId, tagsForIncrementalBackup);
                        Plugin.TempTracksNeedsToBeBackedUp.Add(trackId, true);
                    }
                }


                if (isAutocreated && incrementalBackup.tracks.Count == 0)
                {
                    Plugin.MbApiInterface.MB_SetBackgroundTaskMessage(Plugin.SbTagAutobackupSkipped);

                    if (Plugin.SavedSettings.playCanceledSound)
                        System.Media.SystemSounds.Hand.Play();

                    System.Threading.Thread.Sleep(2000);

                    Plugin.MbApiInterface.MB_SetBackgroundTaskMessage(string.Empty);

                    return;
                }
            }
            else //Backup baseline file doesn't exist
            {
                baseline = this;

                Plugin.TempTracksNeedsToBeBackedUp.Clear();
                Plugin.UpdatedTracksForBackupCount = 0;
                Plugin.BackupIsAlwaysNeeded = true;
            }


            baseline.incrementalBackups.Add(incrementalBackup.guid);


            //Lets save baseline first
            System.IO.Stream streamBbl = System.IO.File.Open(baselineFilename + ".bbl", System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
            System.IO.StreamWriter fileBbl = new System.IO.StreamWriter(streamBbl, Encoding.UTF8);

            XmlSerializer serializerBbl = new XmlSerializer(typeof(BackupType));

            serializerBbl.Serialize(fileBbl, baseline);

            fileBbl.Close();



            //Lets save incremental backup
            System.IO.Stream stream = System.IO.File.Open(fileName + ".xml", System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
            System.IO.StreamWriter file = new System.IO.StreamWriter(stream, Encoding.UTF8);

            XmlSerializer serializer = new XmlSerializer(typeof(BackupType));

            serializer.Serialize(file, incrementalBackup);

            file.Close();


            BackupCacheType backupCache = new BackupCacheType(incrementalBackup, isAutocreated);
            backupCache.save(fileName);

            System.Threading.Thread.Sleep(2000);

            Plugin.MbApiInterface.MB_SetBackgroundTaskMessage(string.Empty);
        }

        public new static BackupType Load(string fileName, string backupFileExtension = ".xml")
        {
            BackupType baseline;
            string baselineFilename = Plugin.GetBackupBaselineFilename();

            if (!System.IO.File.Exists(baselineFilename + ".bbl")) //Backup baseline file doesn't exist
            {
                MessageBox.Show(Plugin.MbForm, Plugin.MsgBackupBaselineFileDoesntExist.Replace("%%FILENAME%%", baselineFilename + ".bbl"), 
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }


            //if (!System.IO.File.Exists(fileName + backupFileExtension)) //***
            //{
            //    MessageBox.Show(Plugin.MbForm, Plugin.MsgBackupFileDoesntExist.Replace("%%FILENAME%%", fileName),
            //        string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            //    return null;
            //}

            System.IO.FileStream stream = System.IO.File.Open(fileName + backupFileExtension, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);
            System.IO.StreamReader file = new System.IO.StreamReader(stream, Encoding.UTF8);

            XmlSerializer backupSerializer = new XmlSerializer(typeof(BackupType));

            BackupType incrementalBackup;
            try
            {
                incrementalBackup = (BackupType)backupSerializer.Deserialize(file);
            }
            catch (Exception ex)
            {
                file.Close();

                MessageBox.Show(Plugin.MbForm, Plugin.MsgBackupIsCorrupted.Replace("%%FILENAME%%", fileName) + "\n\n" + ex.Message, 
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return null;
            }

            file.Close();


            if (backupFileExtension != ".bbl") //We are loading incremental backup
            {
                baseline = Load(baselineFilename, ".bbl");
                if (baseline == null)
                    return null;


                foreach (var incTrack in incrementalBackup.tracks)
                {
                    SerializableDictionary<int, string> tags;
                    if (baseline.tracks.TryGetValue(incTrack.Key, out tags)) //Track exists in baseline
                    {
                        var incTags = incTrack.Value;

                        foreach (var incTagId in incTags.Keys)
                        {
                            string incValue = incTags[incTagId];

                            if (incValue != null && incValue != "_x0000_")
                                tags.AddReplace(incTagId, incValue); //Track tag exists in baseline
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

        public static BackupType LoadIncrementalBackupOnly(string fileName)
        {
            System.IO.FileStream stream = System.IO.File.Open(fileName + ".xml", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);
            System.IO.StreamReader file = new System.IO.StreamReader(stream, Encoding.UTF8);

            XmlSerializer backupSerializer = new XmlSerializer(typeof(BackupType));

            BackupType incrementalBackup;

            try
            {
                incrementalBackup = (BackupType)backupSerializer.Deserialize(file);
            }
            catch (Exception ex)
            {
                file.Close();

                MessageBox.Show(Plugin.MbForm, Plugin.MsgBackupIsCorrupted.Replace("%%FILENAME%%", fileName) + "\n\n" + ex.Message, 
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return null;
            }

            file.Close();


            return incrementalBackup;
        }

        public static string DecodeValue(string value)
        {
            if (value == "_x0000_")
                value = null;
            else
                value = System.Xml.XmlConvert.DecodeName(value);

            return value;
        }

        public string getValue(int trackId, int tagId)
        {
            if (!tracks.TryGetValue(trackId, out SerializableDictionary<int, string> tags))
                return null;

            tags.TryGetValue(tagId, out string value);

            if (value != null)
                return DecodeValue(value);
            else
                return null;
        }

        public string getIncValue(int trackId, int tagId, BackupType baseline)
        {
            if (!tracks.TryGetValue(trackId, out SerializableDictionary<int, string> tags))
                return baseline.getValue(trackId, tagId);

            tags.TryGetValue(tagId, out string value);

            if (value != null)
                return DecodeValue(value);
            else
                return baseline.getValue(trackId, tagId);
        }

        public static string EncodeValue(string value)
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

        public void setValue(string valueParam, int trackId, int tagId)
        {
            valueParam = EncodeValue(valueParam);

            if (!tracks.TryGetValue(trackId, out SerializableDictionary<int, string> tags))
            {
                tags = new SerializableDictionary<int, string>();
                tracks.Add(trackId, tags);
            }

            tags.AddReplace(tagId, valueParam);
        }
    }

    public class BackupIndex : SerializableDictionary<string, SerializableDictionary<string, bool>>
    {
        public void saveBackupAsync(object parameters)
        {
            string backupName = (string)(((object[])parameters)[0]);
            string statusbarText = (string)(((object[])parameters)[1]);
            bool isAutocreatedParam = (bool)(((object[])parameters)[2]);

            saveBackup(backupName, statusbarText, isAutocreatedParam);
        }

        public void saveBackup(string backupName, string statusbarText, bool isAutocreatedParam)
        {
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Lowest;

            Plugin.TempTracksNeedsToBeBackedUp.Clear();

            string[] files = null;
            if (Plugin.MbApiInterface.Library_QueryFilesEx("domain=Library", out files))
            {
                string currentFile;
                int trackId;
                string libraryName = Plugin.GetLibraryName();
                string libraryTrackId;
                string[] libraryTags;

                lock (Plugin.AutobackupLocker)
                {
                    List<string> tagNames = new List<string>();
                    Plugin.FillListByTagNames(tagNames, false, true, false);

                    List<Plugin.MetaDataType> tagIds = new List<Plugin.MetaDataType>();
                    for (int i = 0; i < tagNames.Count; i++)
                        tagIds.Add(Plugin.GetTagId(tagNames[i]));

                    BackupType backup = new BackupType(isAutocreatedParam);

                    int lastShownCount = 0;
                    for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
                    {
                        int percentage = 100 * fileCounter / files.Length;
                        if (lastShownCount < percentage)
                        {
                            lastShownCount = percentage;
                            Plugin.MbApiInterface.MB_SetBackgroundTaskMessage(statusbarText + " " + percentage + "% (" + backupName + ")");
                        }

                        currentFile = files[fileCounter];
                        trackId = int.Parse(Plugin.GetPersistentTrackId(currentFile));


                        if (!Plugin.BackupIsAlwaysNeeded && !Plugin.TracksNeedsToBeBackedUp.Contains(trackId))
                            continue;


                        libraryTrackId = Plugin.AddLibraryNameToTrackId(libraryName, trackId.ToString());
                        libraryTags = Plugin.GetFileTags(currentFile, tagIds);

                        for (int i = 0; i < tagIds.Count; i++)
                        {
                            if (Plugin.SavedSettings.backupArtworks || tagIds[i] != Plugin.MetaDataType.Artwork)
                                backup.setValue(libraryTags[i], trackId, (int)tagIds[i]);
                        }

                        if (!TryGetValue(libraryTrackId, out SerializableDictionary<string, bool> trackBackups))
                        {
                            trackBackups = new SerializableDictionary<string, bool>();
                            Add(libraryTrackId, trackBackups);
                        }

                        trackBackups.AddReplace(backup.guid, true);
                    }


                    backup.save(backupName);


                    System.IO.FileStream stream = System.IO.File.Open(Plugin.GetAutobackupDirectory(Plugin.SavedSettings.autobackupDirectory) + @"\" + Plugin.BackupIndexFileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
                    System.IO.StreamWriter file = new System.IO.StreamWriter(stream, Encoding.UTF8);

                    XmlSerializer backupIndexSerializer = new XmlSerializer(typeof(BackupIndex));

                    backupIndexSerializer.Serialize(file, this);

                    file.Close();
                }
            }


            Plugin.TracksNeedsToBeBackedUp = Plugin.TempTracksNeedsToBeBackedUp;
            Plugin.TempTracksNeedsToBeBackedUp = new SortedDictionary<int, bool>();
            Plugin.UpdatedTracksForBackupCount = 0;
            Plugin.BackupIsAlwaysNeeded = false;

            if (!Plugin.SavedSettings.dontPlayCompletedSound)
                System.Media.SystemSounds.Asterisk.Play();

            Plugin.MbApiInterface.MB_SetBackgroundTaskMessage(string.Empty);
        }

        public static void LoadBackupAsync(object parameters)
        {
            string backupName = (string)(((object[])parameters)[0]);
            string statusbarText = (string)(((object[])parameters)[1]);
            bool restoreForEntireLibrary = (bool)(((object[])parameters)[2]);
            Plugin tagToolsPlugin = (Plugin)(((object[])parameters)[3]);

            lock (Plugin.OpenedForms)
            {
                Plugin.NumberOfNativeMbBackgroundTasks++;
            }

            try
            {
                LoadBackup(backupName, statusbarText, restoreForEntireLibrary);
            }
            catch (System.Threading.ThreadAbortException) { }

            lock (Plugin.OpenedForms)
            {
                Plugin.NumberOfNativeMbBackgroundTasks--;
            }
        }

        public static void LoadBackup(string backupName, string statusbarText, bool restoreForEntireLibrary)
        {
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Lowest;

            string query;

            if (restoreForEntireLibrary)
                query = "domain=Library";
            else
                query = "domain=SelectedFiles";


            BackupType backup = BackupType.Load(backupName);
            if (backup == null)
                return;

            if (backup.libraryName != Plugin.GetLibraryName())
            {
                System.Media.SystemSounds.Hand.Play();
                Plugin.MbApiInterface.MB_SetBackgroundTaskMessage(string.Empty);

                MessageBox.Show(Plugin.MbForm, Plugin.MsgThisIsTheBackupOfDifferentLibrary, 
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            string[] files = null;
            if (Plugin.MbApiInterface.Library_QueryFilesEx(query, out files))
            {
                string currentFile;
                int trackId;

                if (files.Length == 0)
                {
                    MessageBox.Show(Plugin.MbForm, Plugin.MsgNoTracksSelected, string.Empty, 
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }


                List<string> tagNames = new List<string>();
                Plugin.FillListByTagNames(tagNames, false, true, false);

                List<Plugin.MetaDataType> tagIds = new List<Plugin.MetaDataType>();
                for (int i = 0; i < tagNames.Count; i++)
                    tagIds.Add(Plugin.GetTagId(tagNames[i]));

                int lastShownCount = 0;
                for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
                {
                    int percentage = 100 * fileCounter / files.Length;
                    if (lastShownCount < percentage)
                    {
                        lastShownCount = percentage;
                        Plugin.MbApiInterface.MB_SetBackgroundTaskMessage(statusbarText + " " + percentage + "% (" + backupName + ")");
                    }

                    currentFile = files[fileCounter];
                    trackId = int.Parse(Plugin.GetPersistentTrackId(currentFile));

                    bool tagsWereSet = false;
                    for (int i = 0; i < tagIds.Count; i++)
                    {
                        string tagValue = backup.getValue(trackId, (int)tagIds[i]);

                        if (tagValue != null)
                            tagsWereSet |= Plugin.SetFileTag(currentFile, tagIds[i], tagValue, true);
                    }

                    if (tagsWereSet)
                    {
                        Plugin.UpdateTrackForBackup(currentFile);
                        Plugin.CommitTagsToFile(currentFile);
                    }
                }
            }


            if (!Plugin.SavedSettings.dontPlayCompletedSound)
                System.Media.SystemSounds.Asterisk.Play();

            Plugin.MbApiInterface.MB_SetBackgroundTaskMessage(string.Empty);
            Plugin.MbApiInterface.MB_RefreshPanels();
        }

        public SerializableDictionary<string, bool> getBackupGuidsForTrack(string libraryName, int trackId)
        {
            if (!TryGetValue(Plugin.AddLibraryNameToTrackId(libraryName, trackId.ToString()), out SerializableDictionary<string, bool> trackBackups))
                trackBackups = new SerializableDictionary<string, bool>();

            return trackBackups;
        }

        public void deleteBackup(BackupCacheType backupCache)
        {
            foreach (var trackBackups in Values)
                trackBackups.RemoveExisting(backupCache.guid);


            bool wereSomeChanges = false;

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



            string baselineFilename = Plugin.GetBackupBaselineFilename(backupCache.libraryName);

            if (System.IO.File.Exists(baselineFilename + ".bbl")) //Backup baseline file exists
            {
                BackupType baseline = BackupType.Load(baselineFilename, ".bbl");
                baseline.incrementalBackups.Remove(backupCache.guid);

                if (baseline.incrementalBackups.Count == 0) //Lets delete baseline file
                {
                    System.IO.File.Delete(baselineFilename + ".bbl");
                }
                else //Lets save baseline first
                {
                    System.IO.Stream streamBbl = System.IO.File.Open(baselineFilename + ".bbl", System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
                    System.IO.StreamWriter fileBbl = new System.IO.StreamWriter(streamBbl, Encoding.UTF8);

                    XmlSerializer serializerBbl = new XmlSerializer(typeof(BackupType));

                    serializerBbl.Serialize(fileBbl, baseline);

                    fileBbl.Close();
                }
            }



            System.IO.FileStream stream = System.IO.File.Open(Plugin.GetAutobackupDirectory(Plugin.SavedSettings.autobackupDirectory) + @"\" + Plugin.BackupIndexFileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
            System.IO.StreamWriter file = new System.IO.StreamWriter(stream, Encoding.UTF8);

            XmlSerializer backupIndexSerializer = new XmlSerializer(typeof(BackupIndex));

            backupIndexSerializer.Serialize(file, this);

            file.Close();
        }
    }
}
