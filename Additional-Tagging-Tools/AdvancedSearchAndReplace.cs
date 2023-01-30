using MusicBeePlugin.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class AdvancedSearchAndReplaceCommand : PluginWindowTemplate
    {
        private static string CustomText1;


        private static Color UntickedColor;
        private static Color TickedColor;

        private static Bitmap AutoAppliedPresetsAccent;
        private static Bitmap AutoAppliedPresetsDimmed;

        private static Bitmap PredefinedPresetsAccent;
        private static Bitmap PredefinedPresetsDimmed;

        private static Bitmap CustomizedPresetsAccent;
        private static Bitmap CustomizedPresetsDimmed;

        private static Bitmap UserPresetsAccent;
        private static Bitmap UserPresetsDimmed;

        private static Bitmap PlaylistPresetsAccent;
        private static Bitmap PlaylistPresetsDimmed;

        private static Bitmap FunctionIdPresetsAccent;
        private static Bitmap FunctionIdPresetsDimmed;

        private static Bitmap HotkeyPresetsAccent;
        private static Bitmap HotkeyPresetsDimmed;

        private static Bitmap UncheckAllFiltersAccent;
        private static Bitmap UncheckAllFiltersDimmed;


        private static bool ignorefFlterComboBoxSelectedIndexChanged = false;

        private bool ignoreCheckedPresetEvent = true;
        private int autoAppliedPresetCount;

        private delegate void AddRowToTable(string[] row);
        private delegate void ProcessRowOfTable(int row);
        private AddRowToTable addRowToTable;
        private ProcessRowOfTable processRowOfTable;

        private Preset currentPreset;

        private static readonly Encoding Unicode = Encoding.UTF8;

        private string[] files = new string[0];
        private readonly List<string[]> tags = new List<string[]>();
        private Preset preset = null;
        private Preset backupedPreset = null;

        private string[] fileTags;
        private string clipboardText;

        private static SearchedAndReplacedTagsStruct SearchedAndReplacedTags;
        private static Dictionary<int, string> SetTags = new Dictionary<int, string>();
        private static string LastSetTagValue;

        public static string PresetsPath;
        public static SortedDictionary<Guid, Preset> Presets;
        private SortedDictionary<Guid, Preset> presetsWorkingCopy;

        private List<Guid> autoAppliedAsrPresetGuids;
        private int asrPresetsWithHotkeysCount = 0;
        private Guid[] asrPresetsWithHotkeysGuids;
        private SortedDictionary<string, Guid> asrIdsPresetGuids;

        private string assignHotkeyCheckBoxText;
        private bool processPresetCheckBoxCheckedEvent = true;

        public Image checkedState;
        public Image uncheckedState;

        private string newValueText;

        private bool editButtonEnabled;

        private string editApplyText;
        private string autoApplyText;
        private string clickHereText;
        private string nowTickedText;

        private bool tickedOnlyChecked;
        private bool predefinedChecked;
        private bool customizedChecked;
        private bool userChecked;
        private bool playlistChecked;
        private bool functionIdChecked;
        private bool hotkeyChecked;

        private bool untickAllChecked;

        private bool presetsChanged = false;
        private string buttonCloseToolTip;

        private const int PropMetaDataThreshold = 1000;
        private enum ServiceMetaData
        {
            ParameterTagId1 = -101,
            ParameterTagId2 = -102,
            ParameterTagId3 = -103,
            ParameterTagId4 = -104,
            ParameterTagId5 = -105,
            ParameterTagId6 = -106,

            TempTag1 = -201,
            TempTag2 = -202,
            TempTag3 = -203,
            TempTag4 = -204
        }

        public struct SavedPreset
        {
            public DateTime modified;
            public bool modifiedByUser;
            public Guid guid;
            public string id;

            public string[] names;
            public string[] descriptions;

            public bool ignoreCase;
            public bool autoApply;
            public bool assignHotkey;
            public bool applyToPlayingTrack;
            public int hotkeySlot;
            public bool condition;
            public string playlist;

            public List<float> columnWeights;

            public bool isMSRPreset;

            public int parameterTagType;
            public int parameterTag2Type;
            public int parameterTag3Type;
            public int parameterTag4Type;
            public int parameterTag5Type;
            public int parameterTag6Type;

            public int parameterTagId;
            public int parameterTag2Id;
            public int parameterTag3Id;
            public int parameterTag4Id;
            public int parameterTag5Id;
            public int parameterTag6Id;

            public bool customTextChecked;
            public string customText;
            public bool customText2Checked;
            public string customText2;
            public bool customText3Checked;
            public string customText3;
            public bool customText4Checked;
            public string customText4;

            public string preserveValues;

            public int searchedTagId;
            public string searchedPattern;
            public int searchedTag2Id;
            public string searchedPattern2;
            public int searchedTag3Id;
            public string searchedPattern3;
            public int searchedTag4Id;
            public string searchedPattern4;
            public int searchedTag5Id;
            public string searchedPattern5;

            public int replacedTagId;
            public string replacedPattern;
            public int replacedTag2Id;
            public string replacedPattern2;
            public int replacedTag3Id;
            public string replacedPattern3;
            public int replacedTag4Id;
            public string replacedPattern4;
            public int replacedTag5Id;
            public string replacedPattern5;

            public bool append;
            public bool append2;
            public bool append3;
            public bool append4;
            public bool append5;
        }

        public class Preset
        {
            public Preset(SavedPreset savedPreset)//***
                : this()
            {
                userPreset = savedPreset.modifiedByUser;

                if ("" + savedPreset.modified == "01.01.0001 0:00:00")
                    modifiedUtc = DateTime.UtcNow;
                else
                    modifiedUtc = savedPreset.modified;


                if ("" + savedPreset.guid != "00000000-0000-0000-0000-000000000000")
                    guid = savedPreset.guid;

                id = savedPreset.id;

                for (int j = 0; j < savedPreset.names.Length; j += 2)
                {
                    names.Add(savedPreset.names[j], savedPreset.names[j + 1]);
                }

                for (int j = 0; j < savedPreset.descriptions.Length; j += 2)
                {
                    descriptions.Add(savedPreset.descriptions[j], savedPreset.descriptions[j + 1]);
                }

                applyToPlayingTrack = savedPreset.applyToPlayingTrack;
                condition = savedPreset.condition;
                playlist = savedPreset.playlist;
                preserveValues = Regex.Replace(savedPreset.preserveValues ?? "", "\uFFFD", " ");

                columnWeights = new List<float>();
                foreach (float width in savedPreset.columnWeights)
                {
                    columnWeights.Add(width);
                }

                isMSRPreset = savedPreset.isMSRPreset;

                ignoreCase = savedPreset.ignoreCase;

                parameterTagType = savedPreset.parameterTagType;
                parameterTag2Type = savedPreset.parameterTag2Type;
                parameterTag3Type = savedPreset.parameterTag3Type;
                parameterTag4Type = savedPreset.parameterTag4Type;
                parameterTag5Type = savedPreset.parameterTag5Type;
                parameterTag6Type = savedPreset.parameterTag6Type;

                parameterTagId = savedPreset.parameterTagId;
                parameterTag2Id = savedPreset.parameterTag2Id;
                parameterTag3Id = savedPreset.parameterTag3Id;
                parameterTag4Id = savedPreset.parameterTag4Id;
                parameterTag5Id = savedPreset.parameterTag5Id;
                parameterTag6Id = savedPreset.parameterTag6Id;

                customTextChecked = savedPreset.customTextChecked;
                customText = Regex.Replace(savedPreset.customText ?? "", "\uFFFD", " ");
                customText2Checked = savedPreset.customText2Checked;
                customText2 = Regex.Replace(savedPreset.customText2 ?? "", "\uFFFD", " ");
                customText3Checked = savedPreset.customText3Checked;
                customText3 = Regex.Replace(savedPreset.customText3 ?? "", "\uFFFD", " ");
                customText4Checked = savedPreset.customText4Checked;
                customText4 = Regex.Replace(savedPreset.customText4 ?? "", "\uFFFD", " ");

                searchedTagId = savedPreset.searchedTagId;
                searchedPattern = Regex.Replace(savedPreset.searchedPattern ?? "", "\uFFFD", " ");
                searchedTag2Id = savedPreset.searchedTag2Id;
                searchedPattern2 = Regex.Replace(savedPreset.searchedPattern2 ?? "", "\uFFFD", " ");
                searchedTag3Id = savedPreset.searchedTag3Id;
                searchedPattern3 = Regex.Replace(savedPreset.searchedPattern3 ?? "", "\uFFFD", " ");
                searchedTag4Id = savedPreset.searchedTag4Id;
                searchedPattern4 = Regex.Replace(savedPreset.searchedPattern4 ?? "", "\uFFFD", " ");
                searchedTag5Id = savedPreset.searchedTag5Id;
                searchedPattern5 = Regex.Replace(savedPreset.searchedPattern5 ?? "", "\uFFFD", " ");

                replacedTagId = savedPreset.replacedTagId;
                replacedPattern = Regex.Replace(savedPreset.replacedPattern ?? "", "\uFFFD", " ");
                replacedTag2Id = savedPreset.replacedTag2Id;
                replacedPattern2 = Regex.Replace(savedPreset.replacedPattern2 ?? "", "\uFFFD", " ");
                replacedTag3Id = savedPreset.replacedTag3Id;
                replacedPattern3 = Regex.Replace(savedPreset.replacedPattern3 ?? "", "\uFFFD", " ");
                replacedTag4Id = savedPreset.replacedTag4Id;
                replacedPattern4 = Regex.Replace(savedPreset.replacedPattern4 ?? "", "\uFFFD", " ");
                replacedTag5Id = savedPreset.replacedTag5Id;
                replacedPattern5 = Regex.Replace(savedPreset.replacedPattern5 ?? "", "\uFFFD", " ");

                append = savedPreset.append;
                append2 = savedPreset.append2;
                append3 = savedPreset.append3;
                append4 = savedPreset.append4;
                append5 = savedPreset.append5;
            }

            public Preset()
            {
                modifiedUtc = DateTime.UtcNow;
                userPreset = false;
                customizedByUser = false;
                removePreset = false;
                guid = Guid.NewGuid();
                id = "";
                preserveValues = "";

                names = new SerializableDictionary<string, string>();
                descriptions = new SerializableDictionary<string, string>();

                columnWeights = new List<float>();
            }

            public Preset(Preset originalPreset, bool fullCopy = true, bool copyGuid = true, string presetNamePrefix = null, string presetNameSuffix = null)
                : this()
            {
                if (fullCopy)
                {
                    modifiedUtc = originalPreset.modifiedUtc;
                    userPreset = originalPreset.userPreset;
                    customizedByUser = originalPreset.customizedByUser;
                    if (copyGuid)
                        guid = originalPreset.guid;
                    id = originalPreset.id;
                    hotkeyAssigned = originalPreset.hotkeyAssigned;
                }
                else
                {
                    userPreset = true;
                }

                if (userPreset)
                    customizedByUser = false;

                removePreset = originalPreset.removePreset;

                foreach (string key in originalPreset.names.Keys)
                {
                    originalPreset.names.TryGetValue(key, out string value);

                    string name = value;

                    if (!string.IsNullOrEmpty(presetNamePrefix) && !name.StartsWith(presetNamePrefix))
                        name = presetNamePrefix + " " + name;
                    else if (!string.IsNullOrEmpty(presetNamePrefix))
                        name = presetNamePrefix + name;

                    if (!string.IsNullOrEmpty(presetNameSuffix) && !name.EndsWith(presetNameSuffix))
                        name = name + " " + presetNameSuffix;
                    else if (!string.IsNullOrEmpty(presetNameSuffix))
                        name = name + presetNameSuffix;

                    names.Add(key, name);
                }

                foreach (string key in originalPreset.descriptions.Keys)
                {

                    originalPreset.descriptions.TryGetValue(key, out string value);
                    descriptions.Add(key, value);
                }

                columnWeights = new List<float>();
                foreach (float width in originalPreset.columnWeights)
                {
                    columnWeights.Add(width);
                }

                condition = originalPreset.condition;
                playlist = originalPreset.playlist;
                preserveValues = "";

                isMSRPreset = originalPreset.isMSRPreset;

                ignoreCase = originalPreset.ignoreCase;

                parameterTagType = originalPreset.parameterTagType;
                parameterTag2Type = originalPreset.parameterTag2Type;
                parameterTag3Type = originalPreset.parameterTag3Type;
                parameterTag4Type = originalPreset.parameterTag4Type;
                parameterTag5Type = originalPreset.parameterTag5Type;
                parameterTag6Type = originalPreset.parameterTag6Type;

                parameterTagId = originalPreset.parameterTagId;
                parameterTag2Id = originalPreset.parameterTag2Id;
                parameterTag3Id = originalPreset.parameterTag3Id;
                parameterTag4Id = originalPreset.parameterTag4Id;
                parameterTag5Id = originalPreset.parameterTag5Id;
                parameterTag6Id = originalPreset.parameterTag6Id;

                customTextChecked = originalPreset.customTextChecked;
                customText = originalPreset.customText;
                customText2Checked = originalPreset.customText2Checked;
                customText2 = originalPreset.customText2;
                customText3Checked = originalPreset.customText3Checked;
                customText3 = originalPreset.customText3;
                customText4Checked = originalPreset.customText4Checked;
                customText4 = originalPreset.customText4;

                searchedTagId = originalPreset.searchedTagId;
                searchedPattern = originalPreset.searchedPattern;
                searchedTag2Id = originalPreset.searchedTag2Id;
                searchedPattern2 = originalPreset.searchedPattern2;
                searchedTag3Id = originalPreset.searchedTag3Id;
                searchedPattern3 = originalPreset.searchedPattern3;
                searchedTag4Id = originalPreset.searchedTag4Id;
                searchedPattern4 = originalPreset.searchedPattern4;
                searchedTag5Id = originalPreset.searchedTag5Id;
                searchedPattern5 = originalPreset.searchedPattern5;

                replacedTagId = originalPreset.replacedTagId;
                replacedPattern = originalPreset.replacedPattern;
                replacedTag2Id = originalPreset.replacedTag2Id;
                replacedPattern2 = originalPreset.replacedPattern2;
                replacedTag3Id = originalPreset.replacedTag3Id;
                replacedPattern3 = originalPreset.replacedPattern3;
                replacedTag4Id = originalPreset.replacedTag4Id;
                replacedPattern4 = originalPreset.replacedPattern4;
                replacedTag5Id = originalPreset.replacedTag5Id;
                replacedPattern5 = originalPreset.replacedPattern5;

                append = originalPreset.append;
                append2 = originalPreset.append2;
                append3 = originalPreset.append3;
                append4 = originalPreset.append4;
                append5 = originalPreset.append5;
            }

            public override string ToString()
            {
                return GetDictValue(names, Plugin.Language) + (customizedByUser ? " " : "") + (userPreset ? " " : "") 
                    + (condition ? " " : "") + (id != "" ? " " : "") + (hotkeyAssigned ? " ★" : "");
            }

            public string getName(bool getEnglishName = false)
            {
                if (getEnglishName)
                    return GetDictValue(names, "en");
                else
                    return GetDictValue(names, Plugin.Language);
            }

            public string getSafeFileName()
            {
                string presetSafeFileName = getName().Replace('\\', '-').Replace('/', '-').Replace('<', '[').Replace('>', ']')
                    .Replace(" : ", " - ").Replace(": ", " - ").Replace(":", "-")
                    .Replace("\"", "\'\'")
                    .Replace('*', '#').Replace('?', '#').Replace('|', '#');

                if (presetSafeFileName.Length > 251 - Plugin.ASRPresetExtension.Length)
                    presetSafeFileName = presetSafeFileName.Substring(0, 250 - Plugin.ASRPresetExtension.Length) + "…";

                return presetSafeFileName;
            }

            public static Preset LoadOld(string filename, System.Xml.Serialization.XmlSerializer presetSerializer)//***
            {
                FileStream stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None);
                StreamReader file = new StreamReader(stream, Unicode);

                SavedPreset oldSavedPreset = (SavedPreset)presetSerializer.Deserialize(file);
                file.Close();

                Preset savedPreset = new Preset(oldSavedPreset);
                savedPreset.hotkeyAssigned = false;

                return savedPreset;
            }

            public static Preset Load(string filename, System.Xml.Serialization.XmlSerializer presetSerializer, System.Xml.Serialization.XmlSerializer oldPresetSerializer)
            {
                if (filename.EndsWith(Plugin.OldASRPresetExtension))//***
                    return LoadOld(filename, oldPresetSerializer);

                FileStream stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None);
                StreamReader file = new StreamReader(stream, Unicode);

                Preset savedPreset = (Preset)presetSerializer.Deserialize(file);
                savedPreset.hotkeyAssigned = false;

                file.Close();

                return savedPreset;
            }

            public bool areFineCustomizationsMade(Preset referencePreset)
            {
                bool areFineCustomizationsMade = false;

                if (hotkeyAssigned != referencePreset.hotkeyAssigned)
                    areFineCustomizationsMade = true;
                else if (id != referencePreset.id)
                    areFineCustomizationsMade = true;
                else if (applyToPlayingTrack != referencePreset.applyToPlayingTrack)
                    areFineCustomizationsMade = true;
                else if (condition != referencePreset.condition)
                    areFineCustomizationsMade = true;
                else if (playlist != referencePreset.playlist)
                    areFineCustomizationsMade = true;
                else if (preserveValues != referencePreset.preserveValues)
                    areFineCustomizationsMade = true;

                return areFineCustomizationsMade;
            }

            public void setCustomizationsFlag(AdvancedSearchAndReplaceCommand form, Preset referencePreset)
            {
                if (referencePreset == null)
                    return;

                bool customized = false;

                if (customText != referencePreset.customText)
                    customized = true;
                else if (customText2 != referencePreset.customText2)
                    customized = true;
                else if (customText3 != referencePreset.customText3)
                    customized = true;
                else if (customText4 != referencePreset.customText4)
                    customized = true;
                else if (parameterTagId != referencePreset.parameterTagId)
                    customized = true;
                else if (parameterTag2Id != referencePreset.parameterTag2Id)
                    customized = true;
                else if (parameterTag3Id != referencePreset.parameterTag3Id)
                    customized = true;
                else if (parameterTag4Id != referencePreset.parameterTag4Id)
                    customized = true;
                else if (parameterTag5Id != referencePreset.parameterTag5Id)
                    customized = true;
                else if (parameterTag6Id != referencePreset.parameterTag6Id)
                    customized = true;

                if (!userPreset)
                    customizedByUser |= customized;

                if (form != null)
                {
                    if (form.presetsChanged || customized || areFineCustomizationsMade(referencePreset))
                    {
                        form.presetsChanged = true;
                        form.setCheckedState(form.customizedPresetPictureBox, customized);
                        form.buttonClose.Image = Resources.UnsavedChanges_14;
                        form.toolTip1.SetToolTip(form.buttonClose, form.buttonCloseToolTip);
                    }
                }
            }

            public void copyBasicCustomizationsFrom(Preset referencePreset)
            {
                //userPreset = referencePreset.userPreset;
                hotkeyAssigned = referencePreset.hotkeyAssigned;
                id = referencePreset.id;
            }

            public void copyExtendedCustomizationsFrom(Preset referencePreset)
            {
                copyBasicCustomizationsFrom(referencePreset);

                customizedByUser = referencePreset.customizedByUser;

                applyToPlayingTrack = referencePreset.applyToPlayingTrack; //***
                condition = referencePreset.condition;
                playlist = referencePreset.playlist;
                preserveValues = referencePreset.preserveValues;
            }

            public void copyAdvancedCustomizationsFrom(Preset referencePreset)
            {
                copyExtendedCustomizationsFrom(referencePreset);

                customText = referencePreset.customText;
                customText2 = referencePreset.customText2;
                customText3 = referencePreset.customText3;
                customText4 = referencePreset.customText4;
                parameterTagId = referencePreset.parameterTagId;
                parameterTag2Id = referencePreset.parameterTag2Id;
                parameterTag3Id = referencePreset.parameterTag3Id;
                parameterTag4Id = referencePreset.parameterTag4Id;
                parameterTag5Id = referencePreset.parameterTag5Id;
                parameterTag6Id = referencePreset.parameterTag6Id;
            }

            public string savePreset(string pathName = null)
            {
                if (pathName == null)
                    pathName = Path.Combine(@"\\?\" + PresetsPath, guid.ToString() + Plugin.ASRPresetExtension);//***

                System.Xml.Serialization.XmlSerializer presetSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Preset));

                FileStream stream = System.IO.File.Open(pathName, FileMode.Create, FileAccess.Write, FileShare.None);
                StreamWriter file = new StreamWriter(stream, Unicode);

                presetSerializer.Serialize(file, this);

                file.Close();

                return pathName;
            }

            public DateTime modifiedUtc;
            public bool userPreset;
            public bool customizedByUser;
            public bool removePreset;
            public Guid guid;
            public string id;
            public bool hotkeyAssigned;

            public SerializableDictionary<string, string> names;
            public SerializableDictionary<string, string> descriptions;

            public bool ignoreCase;
            public bool applyToPlayingTrack;
            public bool condition;
            public string playlist;

            public List<float> columnWeights;

            public bool isMSRPreset;

            public int parameterTagType;
            public int parameterTag2Type;
            public int parameterTag3Type;
            public int parameterTag4Type;
            public int parameterTag5Type;
            public int parameterTag6Type;

            public int parameterTagId;
            public int parameterTag2Id;
            public int parameterTag3Id;
            public int parameterTag4Id;
            public int parameterTag5Id;
            public int parameterTag6Id;

            public bool customTextChecked;
            public string customText;
            public bool customText2Checked;
            public string customText2;
            public bool customText3Checked;
            public string customText3;
            public bool customText4Checked;
            public string customText4;

            public string preserveValues;

            public int searchedTagId;
            public string searchedPattern;
            public int searchedTag2Id;
            public string searchedPattern2;
            public int searchedTag3Id;
            public string searchedPattern3;
            public int searchedTag4Id;
            public string searchedPattern4;
            public int searchedTag5Id;
            public string searchedPattern5;

            public int replacedTagId;
            public string replacedPattern;
            public int replacedTag2Id;
            public string replacedPattern2;
            public int replacedTag3Id;
            public string replacedPattern3;
            public int replacedTag4Id;
            public string replacedPattern4;
            public int replacedTag5Id;
            public string replacedPattern5;

            public bool append;
            public bool append2;
            public bool append3;
            public bool append4;
            public bool append5;
        }

        public struct SearchedAndReplacedTagsStruct
        {
            public string searchedTagValue;
            public string searchedTag2Value;
            public string searchedTag3Value;
            public string searchedTag4Value;
            public string searchedTag5Value;

            public string originalReplacedTagValue;
            public string originalReplacedTag2Value;
            public string originalReplacedTag3Value;
            public string originalReplacedTag4Value;
            public string originalReplacedTag5Value;

            public string replacedTagValue;
            public string replacedTag2Value;
            public string replacedTag3Value;
            public string replacedTag4Value;
            public string replacedTag5Value;
        }

        public class Playlist
        {
            public string playlist;

            public Playlist(string playlistParam)
            {
                playlist = playlistParam;
            }

            public override string ToString()
            {
                return Regex.Replace(playlist, ".*\\\\(.*)\\.[^.]*$", "$1");
            }
        }

        private class Function
        {
            protected string functionName = "null";

            protected virtual string calculate(string currentFile, string parameter0, string parameter1 = null)
            {
                return "\u0000";
            }

            public virtual void multiEvaluate1(string currentFile, ref string tagValue)
            {
                var args = Regex.Matches(tagValue, @"\\@" + functionName + @"\[\[(.*?)\]\]");

                for (int i = 0; i < args.Count; i++)
                {
                    string arg = Regex.Replace(args[i].Value, @"\\@" + functionName + @"\[\[(.*?)\]\]", "$1");
                    string result = calculate(currentFile, arg);
                    tagValue = Regex.Replace(tagValue, @"\\@" + functionName + @"\[\[" + Regex.Escape(arg) + @"\]\]", result, RegexOptions.None);
                }
            }

            public virtual void multiEvaluate2(string currentFile, ref string tagValue)
            {
                var args1 = Regex.Matches(tagValue, @"\\@" + functionName + @"\[\[(.*)\;\;.*?\]\]");
                //var args2 = Regex.Matches(tagValue, @"\\@" + functionName + @"\[\[.*\;\;(.*?)\]\]");

                for (int i = 0; i < args1.Count; i++)
                {
                    string arg1 = Regex.Replace(args1[i].Value, @"\\@" + functionName + @"\[\[(.*)\;\;.*?\]\]", "$1");
                    string arg2 = Regex.Replace(args1[i].Value, @"\\@" + functionName + @"\[\[.*\;\;(.*?)\]\]", "$1");
                    string result = calculate(currentFile, arg1, arg2);
                    tagValue = Regex.Replace(tagValue, @"\\@" + functionName + @"\[\[" + Regex.Escape(arg1) + @"\;\;" + Regex.Escape(arg2) + @"\]\]", result, RegexOptions.None);
                }
            }

            public string evaluate(string currentFile, string tagValue)
            {
                if (Regex.IsMatch(tagValue, @"\\@" + functionName + @"\[\[(.*)\;\;(.*?)\]\]", RegexOptions.None))
                    multiEvaluate2(currentFile, ref tagValue);


                if (Regex.IsMatch(tagValue, @"\\@" + functionName + @"\[\[(.*?)\]\]", RegexOptions.None))
                    multiEvaluate1(currentFile, ref tagValue);


                return tagValue;
            }
        }

        private class Rg2sc : Function
        {
            public Rg2sc()
            {
                functionName = "rg2sc";
            }

            protected override string calculate(string currentFile, string parameter0, string parameter1 = null)
            {
                parameter0 = Regex.Replace(parameter0, @"(.*)\s.*", "$1");


                double replayGain;

                try
                {
                    replayGain = Convert.ToDouble(parameter0);
                }
                catch
                {
                    replayGain = 0;
                }


                double soundCheck1000d = 1000 * Math.Pow(10.0, (-0.1 * replayGain));
                //if (soundCheck1000d > 65534)
                //    soundCheck1000d = 65534;
                uint soundCheck1000;

                try
                {
                    soundCheck1000 = Convert.ToUInt32(soundCheck1000d);
                }
                catch
                {
                    soundCheck1000 = 0;
                }


                //double soundCheck2500d = 2500 * Math.Pow(10.0, (-0.1 * replayGain));
                //if (soundCheck2500d > 65534)
                //    soundCheck2500d = 65534;
                //uint soundCheck2500;

                //try
                //{
                //    soundCheck2500 = Convert.ToUInt32(soundCheck2500d);
                //}
                //catch
                //{
                //    soundCheck2500 = 0;
                //}


                string ITUNNORM = (" " + soundCheck1000.ToString("X8"));
                ITUNNORM += (" " + soundCheck1000.ToString("X8"));

                ITUNNORM += (" " + soundCheck1000.ToString("X8"));
                ITUNNORM += (" " + soundCheck1000.ToString("X8"));

                ITUNNORM += (" " + soundCheck1000.ToString("X8"));
                ITUNNORM += (" " + soundCheck1000.ToString("X8"));

                ITUNNORM += (" " + soundCheck1000.ToString("X8"));
                ITUNNORM += (" " + soundCheck1000.ToString("X8"));

                ITUNNORM += (" " + soundCheck1000.ToString("X8"));
                ITUNNORM += (" " + soundCheck1000.ToString("X8"));

                return ITUNNORM;
            }
        }

        private class Rg2sc4mp3 : Rg2sc
        {
            public Rg2sc4mp3()
            {
                functionName = "rg2sc4mp3";
            }

            protected override string calculate(string currentFile, string parameter0, string parameter1 = null)
            {
                return base.calculate(currentFile, parameter0) + "\u0000";
            }
        }

        private class Tc : Function
        {
            public Tc()
            {
                functionName = "tc";
            }

            protected override string calculate(string currentFile, string parameter0, string parameter1 = null)
            {
                string[] exceptionWords;

                if (parameter1 == null)
                    parameter1 = Plugin.SavedSettings.exceptionWordsASR;

                exceptionWords = parameter1.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                parameter0 = ChangeCaseCommand.ChangeWordsCase(parameter0, ChangeCaseCommand.ChangeCaseOptions.lowerCase, null, false,
                    null, Plugin.SavedSettings.wordSplittersASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                string result = ChangeCaseCommand.ChangeWordsCase(parameter0, ChangeCaseCommand.ChangeCaseOptions.titleCase, exceptionWords, false,
                    Plugin.SavedSettings.exceptionCharsASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), Plugin.SavedSettings.wordSplittersASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), true, true);

                return result;
            }
        }

        private class Lc : Function
        {
            public Lc()
            {
                functionName = "lc";
            }

            protected override string calculate(string currentFile, string parameter0, string parameter1 = null)
            {
                string[] exceptionWords;

                if (parameter1 == null)
                    parameter1 = Plugin.SavedSettings.exceptionWordsASR;

                exceptionWords = parameter1.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                string result = ChangeCaseCommand.ChangeWordsCase(parameter0, ChangeCaseCommand.ChangeCaseOptions.lowerCase, exceptionWords, false,
                    Plugin.SavedSettings.exceptionCharsASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), Plugin.SavedSettings.wordSplittersASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

                return result;
            }
        }

        private class Uc : Function
        {
            public Uc()
            {
                functionName = "uc";
            }

            protected override string calculate(string currentFile, string parameter0, string parameter1 = null)
            {
                string[] exceptionWords;

                if (parameter1 == null)
                    parameter1 = Plugin.SavedSettings.exceptionWordsASR;

                exceptionWords = parameter1.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                string result = ChangeCaseCommand.ChangeWordsCase(parameter0, ChangeCaseCommand.ChangeCaseOptions.upperCase, exceptionWords, false,
                    Plugin.SavedSettings.exceptionCharsASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), Plugin.SavedSettings.wordSplittersASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

                return result;
            }
        }

        private class Sc : Function
        {
            public Sc()
            {
                functionName = "sc";
            }

            protected override string calculate(string currentFile, string parameter0, string parameter1 = null)
            {
                string[] exceptionWords;

                if (parameter1 == null)
                    parameter1 = Plugin.SavedSettings.exceptionWordsASR;

                exceptionWords = parameter1.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                parameter0 = ChangeCaseCommand.ChangeWordsCase(parameter0, ChangeCaseCommand.ChangeCaseOptions.lowerCase, null, false,
                    null, Plugin.SavedSettings.wordSplittersASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                string result = ChangeCaseCommand.ChangeWordsCase(parameter0, ChangeCaseCommand.ChangeCaseOptions.sentenceCase, exceptionWords, false,
                    Plugin.SavedSettings.exceptionCharsASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), Plugin.SavedSettings.wordSplittersASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), true, false);

                return result;
            }
        }

        private class Eval : Function
        {
            public Eval()
            {
                functionName = "eval";
            }

            protected override string calculate(string currentFile, string parameter0, string parameter1 = null)
            {
                return Plugin.MbApiInterface.MB_Evaluate(parameter0, currentFile);
            }
        }

        private class Char1 : Function
        {
            public Char1()
            {
                functionName = "char";
            }

            protected override string calculate(string currentFile, string parameter0, string parameter1 = null)
            {
                ushort charcode = ushort.Parse(parameter0, System.Globalization.NumberStyles.HexNumber);
                return ((char)charcode).ToString();
            }
        }

        private class Repunct : Function
        {
            public Repunct()
            {
                functionName = "repunct";
            }

            protected override string calculate(string currentFile, string parameter0, string parameter1 = null)
            {
                parameter0 = Regex.Replace(parameter0, "\u013F", "L");
                parameter0 = Regex.Replace(parameter0, "\u0140", "l");
                parameter0 = Regex.Replace(parameter0, "\u2018", "'");
                parameter0 = Regex.Replace(parameter0, "\u2019", "'");
                parameter0 = Regex.Replace(parameter0, "\u201A", "'");
                parameter0 = Regex.Replace(parameter0, "\u201B", "'");
                parameter0 = Regex.Replace(parameter0, "\u201C", "\"");
                parameter0 = Regex.Replace(parameter0, "\u201D", "\"");
                parameter0 = Regex.Replace(parameter0, "\u201E", "\"");
                parameter0 = Regex.Replace(parameter0, "\u201F", "\"");
                parameter0 = Regex.Replace(parameter0, "\u2032", "'");
                parameter0 = Regex.Replace(parameter0, "\u2033", "\"");
                parameter0 = Regex.Replace(parameter0, "\u301D", "\"");
                parameter0 = Regex.Replace(parameter0, "\u301E", "\"");
                parameter0 = Regex.Replace(parameter0, "\u00AB", "<<");
                parameter0 = Regex.Replace(parameter0, "\u00BB", ">>");
                parameter0 = Regex.Replace(parameter0, "\u2039", "<");
                parameter0 = Regex.Replace(parameter0, "\u203A", ">");
                parameter0 = Regex.Replace(parameter0, "\u00AD", "");
                parameter0 = Regex.Replace(parameter0, "\u2010", "-");
                parameter0 = Regex.Replace(parameter0, "\u2011", "-");
                parameter0 = Regex.Replace(parameter0, "\u2012", "-");
                parameter0 = Regex.Replace(parameter0, "\u2013", "-");
                parameter0 = Regex.Replace(parameter0, "\u2014", "-");
                parameter0 = Regex.Replace(parameter0, "\u2015", "-");
                parameter0 = Regex.Replace(parameter0, "\uFE31", "|");
                parameter0 = Regex.Replace(parameter0, "\uFE32", "|");
                parameter0 = Regex.Replace(parameter0, "\uFE58", "-");
                parameter0 = Regex.Replace(parameter0, "\u2016", "||");
                parameter0 = Regex.Replace(parameter0, "\u2044", "/");
                parameter0 = Regex.Replace(parameter0, "\u2045", "[");
                parameter0 = Regex.Replace(parameter0, "\u2046", "]");
                parameter0 = Regex.Replace(parameter0, "\u204E", "*");
                parameter0 = Regex.Replace(parameter0, "\u3008", "<");
                parameter0 = Regex.Replace(parameter0, "\u3009", ">");
                parameter0 = Regex.Replace(parameter0, "\u300A", "<<");
                parameter0 = Regex.Replace(parameter0, "\u300B", ">>");
                parameter0 = Regex.Replace(parameter0, "\u3014", "[");
                parameter0 = Regex.Replace(parameter0, "\u3015", "]");
                parameter0 = Regex.Replace(parameter0, "\u3018", "[");
                parameter0 = Regex.Replace(parameter0, "\u3019", "]");
                parameter0 = Regex.Replace(parameter0, "\u301A", "[");
                parameter0 = Regex.Replace(parameter0, "\u301B", "]");
                parameter0 = Regex.Replace(parameter0, "\uFE11", ",");
                parameter0 = Regex.Replace(parameter0, "\uFE12", ".");
                parameter0 = Regex.Replace(parameter0, "\uFE39", "[");
                parameter0 = Regex.Replace(parameter0, "\uFE3A", "]");
                parameter0 = Regex.Replace(parameter0, "\uFE3D", "<<");
                parameter0 = Regex.Replace(parameter0, "\uFE3E", ">>");
                parameter0 = Regex.Replace(parameter0, "\uFE3F", "<");
                parameter0 = Regex.Replace(parameter0, "\uFE3F", "");
                parameter0 = Regex.Replace(parameter0, "\uFE40", ">");
                parameter0 = Regex.Replace(parameter0, "\uFE51", ",");
                parameter0 = Regex.Replace(parameter0, "\uFE5D", "[");
                parameter0 = Regex.Replace(parameter0, "\uFE5E", "]");
                parameter0 = Regex.Replace(parameter0, "\uFF5F", "((");
                parameter0 = Regex.Replace(parameter0, "\uFF60", "))");
                parameter0 = Regex.Replace(parameter0, "\uFF61", ".");
                parameter0 = Regex.Replace(parameter0, "\uFF64", ",");
                parameter0 = Regex.Replace(parameter0, "\u2212", "-");
                parameter0 = Regex.Replace(parameter0, "\u2215", "/");
                parameter0 = Regex.Replace(parameter0, "\u2216", "\\");
                parameter0 = Regex.Replace(parameter0, "\u2223", "|");
                parameter0 = Regex.Replace(parameter0, "\u2225", "||");
                parameter0 = Regex.Replace(parameter0, "\u226A", "<<");
                parameter0 = Regex.Replace(parameter0, "\u226B", ">>");
                parameter0 = Regex.Replace(parameter0, "\u2985", "((");
                parameter0 = Regex.Replace(parameter0, "\u2986", "))");
                parameter0 = Regex.Replace(parameter0, "\u200B", "");

                return parameter0;
            }
        }

        private class SortPerformers : Function
        {
            public SortPerformers()
            {
                functionName = "sortperformers";
            }

            public override void multiEvaluate1(string currentFile, ref string tagValue)
            {
                var args = Regex.Matches(tagValue, @"\\@" + functionName + @"\[\[(.*)\]\]");

                for (int i = 0; i < args.Count; i++)
                {
                    string rawArg = Regex.Replace(args[i].Value, @"\\@" + functionName + @"\[\[(.*)\]\]", "$1");
                    string arg = rawArg.Replace("\u000E", "[").Replace("\u000F", "]").Replace("\u0010", ",").Replace("\u0011", @"\");
                    string result = calculate(currentFile, arg);
                    tagValue = Regex.Replace(tagValue, @"\\@" + functionName + @"\[\[" + Regex.Escape(rawArg) + @"\]\]", result, RegexOptions.IgnoreCase);
                }
            }

            protected override string calculate(string currentFile, string parameter0, string parameter1 = null)
            {
                CustomText1 = Regex.Replace(CustomText1, "; ", ";");
                string[] artists = parameter0.Replace("\u000E", "[").Replace("\u000F", "]").Split(new string[] { "\u0000" }, StringSplitOptions.RemoveEmptyEntries);
                string[] roles = CustomText1.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                List<string> performersList = new List<string>();

                for (int i = 0; i < artists.Length; i++)
                {
                    if (artists[i][0] == '\x02')
                    {
                        performersList.Add(artists[i]);
                        artists[i] = null;
                    }
                }

                if (performersList.Count == 0)
                    return parameter0;


                for (int i = 0; i < performersList.Count; i++)
                {
                    for (int j = 0; j < roles.Length; j++)
                    {
                        if (Regex.IsMatch(performersList[i], @".*\(" + roles[j] + @"\).*"))
                        {
                            performersList[i] = j.ToString("D3") + performersList[i];
                            break;
                        }
                    }
                }

                for (int i = 0; i < performersList.Count; i++)
                {
                    if (performersList[i][0] != '0')
                        performersList[i] = "999" + performersList[i];
                }

                performersList.Sort();

                string newArtists = "";

                int performerNo = 0;
                for (int i = 0; i < artists.Length; i++)
                {
                    if (artists[i] == null)
                    {
                        newArtists += performersList[performerNo++].Remove(0, 3) + "\x00";
                    }
                    else
                    {
                        newArtists += artists[i] + "\x00";
                    }
                }

                newArtists = newArtists.Remove(newArtists.Length - 1, 1);

                return newArtists;
            }
        }

        private class SearchReplace : Function
        {
            public SearchReplace()
            {
                functionName = "replace";
            }

            protected override string calculate(string currentFile, string parameter0, string parameter1 = null)
            {
                string[] secondParameter = parameter1.Split('|');

                for (int i = 0; i < secondParameter.Length; i++)
                {
                    string[] pair = secondParameter[i].Split('/');

                    if (pair.Length != 2)
                    {
                        return "SYNTAX ERROR!";
                    }

                    if (pair[0].Length > 0 && pair[0][0] == '#')
                    {
                        if (pair[0].Length > 1 && pair[0][1] == '*')
                        {
                            pair[0] = pair[0].Substring(2);
                            pair[0] = pair[0].Replace(@"\\", @"\").Replace(@"\L", @"/").Replace(@"\V", @"|").Replace(@"\#", @"#").Replace(@"\X", @"*");
                            pair[1] = pair[1].Replace(@"\\", @"\").Replace(@"\L", @"/").Replace(@"\V", @"|").Replace(@"\#", @"#").Replace(@"\X", @"*").Replace(@"\T", @"$");
                            parameter0 = Replace(currentFile, parameter0, pair[0], pair[1], "", false, out _);
                        }
                        else
                        {
                            pair[0] = pair[0].Substring(1);
                            pair[0] = pair[0].Replace(@"\\", @"\").Replace(@"\L", @"/").Replace(@"\V", @"|").Replace(@"\#", @"#").Replace(@"\X", @"*");
                            pair[1] = pair[1].Replace(@"\\", @"\").Replace(@"\L", @"/").Replace(@"\V", @"|").Replace(@"\#", @"#").Replace(@"\X", @"*").Replace(@"\T", @"$");
                            pair[0] = Regex.Escape(pair[0]);
                            parameter0 = Replace(currentFile, parameter0, pair[0], pair[1], "", false, out _);
                        }
                    }
                    else
                    {
                        if (pair[0].Length > 0 && pair[0][0] == '*')
                        {
                            pair[0] = pair[0].Substring(1);
                            pair[0] = pair[0].Replace(@"\\", @"\").Replace(@"\L", @"/").Replace(@"\V", @"|").Replace(@"\#", @"#").Replace(@"\X", @"*");
                            pair[1] = pair[1].Replace(@"\\", @"\").Replace(@"\L", @"/").Replace(@"\V", @"|").Replace(@"\#", @"#").Replace(@"\X", @"*").Replace(@"\T", @"$");
                            parameter0 = Replace(currentFile, parameter0, pair[0], pair[1], "", true, out _);
                        }
                        else
                        {
                            pair[0] = pair[0].Replace(@"\\", @"\").Replace(@"\L", @"/").Replace(@"\V", @"|").Replace(@"\#", @"#").Replace(@"\X", @"*");
                            pair[1] = pair[1].Replace(@"\\", @"\").Replace(@"\L", @"/").Replace(@"\V", @"|").Replace(@"\#", @"#").Replace(@"\X", @"*").Replace(@"\T", @"$");
                            pair[0] = Regex.Escape(pair[0]);
                            parameter0 = Replace(currentFile, parameter0, pair[0], pair[1], "", true, out _);
                        }
                    }
                }

                return parameter0;
            }
        }

        public static string GetDictValue(SortedDictionary<string, string> dict, string language)
        {
            dict.TryGetValue(language, out string value);

            if (value == null)
                dict.TryGetValue("en", out value);

            return value;
        }

        public static void SetDictValue(SortedDictionary<string, string> dict, string language, string newValue)
        {
            dict.TryGetValue(language, out string value);

            if (value != null)
                dict.Remove(language);

            dict.Add(language, newValue);


            dict.TryGetValue("en", out value);

            if (value == null)
                dict.Add("en", newValue);
        }

        public static void Init()
        {
            if (Plugin.SavedSettings.dontShowASR)
                return;

            lock (Plugin.AutoAppliedPresets)
            {
                Encoding Unicode = Encoding.UTF8;

                SetTags = new Dictionary<int, string>();

                PresetsPath = Path.Combine(@"\\?\" + Plugin.MbApiInterface.Setting_GetPersistentStoragePath(), Plugin.AsrPresetsDirectory);
                Presets = new SortedDictionary<Guid, Preset>();
                string[] presetNames;

                Plugin.AutoAppliedPresets.Clear();
                Plugin.ASRPresetsWithHotkeysCount = 0;

                if (!Directory.Exists(PresetsPath))
                    Directory.CreateDirectory(PresetsPath);

                presetNames = Directory.GetFiles(PresetsPath, "*");
                System.Xml.Serialization.XmlSerializer presetSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Preset));
                System.Xml.Serialization.XmlSerializer presetSerializerOld = new System.Xml.Serialization.XmlSerializer(typeof(SavedPreset));//***

                foreach (string presetName in presetNames)
                {
                    try
                    {
                        Preset tempPreset = Preset.Load(presetName, presetSerializer, presetSerializerOld);

                        if (tempPreset.guid.ToString() != "ff8d53d9-526b-4b40-bbf0-848b6b892f70")
                        {
                            if (Presets.TryGetValue(tempPreset.guid, out Preset existingPreset))
                            {
                                Presets.Remove(tempPreset.guid);
                                if (Plugin.SavedSettings.autoAppliedAsrPresetGuids.Contains(tempPreset.guid))
                                    Plugin.AutoAppliedPresets.Remove(existingPreset);
                                if (Plugin.AsrIdsPresets.TryGetValue(tempPreset.id, out _))
                                    Plugin.AsrIdsPresets.Remove(tempPreset.id);
                            }

                            Presets.Add(tempPreset.guid, tempPreset);

                            if (Plugin.SavedSettings.autoAppliedAsrPresetGuids.Contains(tempPreset.guid))
                                Plugin.AutoAppliedPresets.Add(tempPreset);

                            if (Plugin.SavedSettings.asrPresetsWithHotkeysGuids.Contains(tempPreset.guid))
                            {
                                int index;
                                Plugin.ASRPresetsWithHotkeysCount++;
                                for (index = 0; index < Plugin.SavedSettings.asrPresetsWithHotkeysGuids.Length; index++)
                                {
                                    if (Plugin.SavedSettings.asrPresetsWithHotkeysGuids[index] == tempPreset.guid)
                                        break;
                                }
                                Plugin.AsrPresetsWithHotkeys[index] = tempPreset;
                                tempPreset.hotkeyAssigned = true;
                            }
                            else
                            {
                                tempPreset.hotkeyAssigned = false;
                            }

                            if (!string.IsNullOrEmpty(tempPreset.id))
                            {
                                Plugin.AsrIdsPresets.Add(tempPreset.id, tempPreset);
                            }
                        }
                        else
                        {
                            Plugin.MSR = tempPreset;
                        }
                    }
                    catch { };
                }
            }
        }

        public static void RegisterASRPresetsHotkeysAndMenuItems(Plugin tagToolsPlugin)
        {
            Plugin.ASRPresetsMenuItem?.DropDown.Items.Clear();

            Plugin.ASRPresetsContextMenuItem?.DropDown.Items.Clear();

            if (Plugin.SavedSettings.dontShowASR)
                return;

            for (int i = 0; i < Plugin.AsrPresetsWithHotkeys.Length; i++)
            {
                Preset tempPreset = Plugin.AsrPresetsWithHotkeys[i];

                if (tempPreset != null)
                {
                    switch (i)
                    { 
                        case 0:
                            Plugin.MbApiInterface.MB_RegisterCommand(Plugin.AsrHotkeyDescription + tempPreset.getName(), tagToolsPlugin.ASRPreset1EventHandler);
                            Plugin.ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset1EventHandler);
                            Plugin.ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset1EventHandler);
                            break;
                        case 1:
                            Plugin.MbApiInterface.MB_RegisterCommand(Plugin.AsrHotkeyDescription + tempPreset.getName(), tagToolsPlugin.ASRPreset2EventHandler);
                            Plugin.ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset2EventHandler);
                            Plugin.ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset2EventHandler);
                            break;
                        case 2:
                            Plugin.MbApiInterface.MB_RegisterCommand(Plugin.AsrHotkeyDescription + tempPreset.getName(), tagToolsPlugin.ASRPreset3EventHandler);
                            Plugin.ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset3EventHandler);
                            Plugin.ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset3EventHandler);
                            break;
                        case 3:
                            Plugin.MbApiInterface.MB_RegisterCommand(Plugin.AsrHotkeyDescription + tempPreset.getName(), tagToolsPlugin.ASRPreset4EventHandler);
                            Plugin.ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset4EventHandler);
                            Plugin.ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset4EventHandler);
                            break;
                        case 4:
                            Plugin.MbApiInterface.MB_RegisterCommand(Plugin.AsrHotkeyDescription + tempPreset.getName(), tagToolsPlugin.ASRPreset5EventHandler);
                            Plugin.ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset5EventHandler);
                            Plugin.ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset5EventHandler);
                            break;
                        case 5:
                            Plugin.MbApiInterface.MB_RegisterCommand(Plugin.AsrHotkeyDescription + tempPreset.getName(), tagToolsPlugin.ASRPreset6EventHandler);
                            Plugin.ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset6EventHandler);
                            Plugin.ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset6EventHandler);
                            break;
                        case 6:
                            Plugin.MbApiInterface.MB_RegisterCommand(Plugin.AsrHotkeyDescription + tempPreset.getName(), tagToolsPlugin.ASRPreset7EventHandler);
                            Plugin.ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset7EventHandler);
                            Plugin.ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset7EventHandler);
                            break;
                        case 7:
                            Plugin.MbApiInterface.MB_RegisterCommand(Plugin.AsrHotkeyDescription + tempPreset.getName(), tagToolsPlugin.ASRPreset8EventHandler);
                            Plugin.ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset8EventHandler);
                            Plugin.ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset8EventHandler);
                            break;
                        case 8:
                            Plugin.MbApiInterface.MB_RegisterCommand(Plugin.AsrHotkeyDescription + tempPreset.getName(), tagToolsPlugin.ASRPreset9EventHandler);
                            Plugin.ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset9EventHandler);
                            Plugin.ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset9EventHandler);
                            break;
                        case 9:
                            Plugin.MbApiInterface.MB_RegisterCommand(Plugin.AsrHotkeyDescription + tempPreset.getName(), tagToolsPlugin.ASRPreset10EventHandler);
                            Plugin.ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset10EventHandler);
                            Plugin.ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset10EventHandler);
                            break;
                        case 10:
                            Plugin.MbApiInterface.MB_RegisterCommand(Plugin.AsrHotkeyDescription + tempPreset.getName(), tagToolsPlugin.ASRPreset11EventHandler);
                            Plugin.ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset11EventHandler);
                            Plugin.ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset11EventHandler);
                            break;
                        case 11:
                            Plugin.MbApiInterface.MB_RegisterCommand(Plugin.AsrHotkeyDescription + tempPreset.getName(), tagToolsPlugin.ASRPreset12EventHandler);
                            Plugin.ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset12EventHandler);
                            Plugin.ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset12EventHandler);
                            break;
                        case 12:
                            Plugin.MbApiInterface.MB_RegisterCommand(Plugin.AsrHotkeyDescription + tempPreset.getName(), tagToolsPlugin.ASRPreset13EventHandler);
                            Plugin.ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset13EventHandler);
                            Plugin.ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset13EventHandler);
                            break;
                        case 13:
                            Plugin.MbApiInterface.MB_RegisterCommand(Plugin.AsrHotkeyDescription + tempPreset.getName(), tagToolsPlugin.ASRPreset14EventHandler);
                            Plugin.ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset14EventHandler);
                            Plugin.ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset14EventHandler);
                            break;
                        case 14:
                            Plugin.MbApiInterface.MB_RegisterCommand(Plugin.AsrHotkeyDescription + tempPreset.getName(), tagToolsPlugin.ASRPreset15EventHandler);
                            Plugin.ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset15EventHandler);
                            Plugin.ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset15EventHandler);
                            break;
                        case 15:
                            Plugin.MbApiInterface.MB_RegisterCommand(Plugin.AsrHotkeyDescription + tempPreset.getName(), tagToolsPlugin.ASRPreset16EventHandler);
                            Plugin.ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset16EventHandler);
                            Plugin.ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset16EventHandler);
                            break;
                        case 16:
                            Plugin.MbApiInterface.MB_RegisterCommand(Plugin.AsrHotkeyDescription + tempPreset.getName(), tagToolsPlugin.ASRPreset17EventHandler);
                            Plugin.ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset17EventHandler);
                            Plugin.ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset17EventHandler);
                            break;
                        case 17:
                            Plugin.MbApiInterface.MB_RegisterCommand(Plugin.AsrHotkeyDescription + tempPreset.getName(), tagToolsPlugin.ASRPreset18EventHandler);
                            Plugin.ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset18EventHandler);
                            break;
                        case 18:
                            Plugin.MbApiInterface.MB_RegisterCommand(Plugin.AsrHotkeyDescription + tempPreset.getName(), tagToolsPlugin.ASRPreset19EventHandler);
                            Plugin.ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset19EventHandler);
                            Plugin.ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset19EventHandler);
                            break;
                        case 19:
                            Plugin.MbApiInterface.MB_RegisterCommand(Plugin.AsrHotkeyDescription + tempPreset.getName(), tagToolsPlugin.ASRPreset20EventHandler);
                            Plugin.ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset20EventHandler);
                            Plugin.ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset20EventHandler);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public AdvancedSearchAndReplaceCommand()
        {
            InitializeComponent();
        }

        public AdvancedSearchAndReplaceCommand(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();


            var heightField = typeof(CheckedListBox).GetField(
                "scaledListItemBordersHeight",
                BindingFlags.NonPublic | BindingFlags.Instance
            );

            var addedHeight = 3; // Some appropriate value, greater than the field's default of 2

            heightField.SetValue(presetList, addedHeight); // Where "presetList" is your CheckedListBox


            uncheckedState = Plugin.GetSolidImageByBitmapMask(ForeColor, BackColor, Resources.uncheck_mark, 0.5f);
            checkedState = Plugin.GetSolidImageByBitmapMask(ForeColor, BackColor, Resources.check_mark, 1.0f);


            clearIdButton.Image = Plugin.GetSolidImageByBitmapMask(clearIdButton.ForeColor, clearIdButton.BackColor, Resources.clear_button, 1.0f);
            clearSearchButton.Image = Plugin.GetSolidImageByBitmapMask(clearSearchButton.ForeColor, clearSearchButton.BackColor, Resources.clear_button, 1.0f);

            //Color accentColor = SystemColors.Highlight;
            Color accentColor = ForeColor;
            float accentWeight = 1.0f;

            Color dimmedColor = ForeColor;
            float dimmedWeight = 0.2f;


            AutoAppliedPresetsAccent = Plugin.GetSolidImageByBitmapMask(accentColor, BackColor, Resources.auto_applied_presets, accentWeight);
            AutoAppliedPresetsDimmed = Plugin.GetSolidImageByBitmapMask(dimmedColor, BackColor, Resources.auto_applied_presets, dimmedWeight);

            PredefinedPresetsAccent = Plugin.GetSolidImageByBitmapMask(accentColor, BackColor, Resources.predefined_presets, accentWeight);
            PredefinedPresetsDimmed = Plugin.GetSolidImageByBitmapMask(dimmedColor, BackColor, Resources.predefined_presets, dimmedWeight);

            CustomizedPresetsAccent = Plugin.GetSolidImageByBitmapMask(accentColor, BackColor, Resources.customized_presets, accentWeight);
            CustomizedPresetsDimmed = Plugin.GetSolidImageByBitmapMask(dimmedColor, BackColor, Resources.customized_presets, dimmedWeight);

            UserPresetsAccent = Plugin.GetSolidImageByBitmapMask(accentColor, BackColor, Resources.user_presets, accentWeight);
            UserPresetsDimmed = Plugin.GetSolidImageByBitmapMask(dimmedColor, BackColor, Resources.user_presets, dimmedWeight);

            PlaylistPresetsAccent = Plugin.GetSolidImageByBitmapMask(accentColor, BackColor, Resources.playlist_presets, accentWeight);
            PlaylistPresetsDimmed = Plugin.GetSolidImageByBitmapMask(dimmedColor, BackColor, Resources.playlist_presets, dimmedWeight);

            FunctionIdPresetsAccent = Plugin.GetSolidImageByBitmapMask(accentColor, BackColor, Resources.function_id_presets, accentWeight);
            FunctionIdPresetsDimmed = Plugin.GetSolidImageByBitmapMask(dimmedColor, BackColor, Resources.function_id_presets, dimmedWeight);

            HotkeyPresetsAccent = Plugin.GetSolidImageByBitmapMask(accentColor, BackColor, Resources.hotkey_presets, accentWeight);
            HotkeyPresetsDimmed = Plugin.GetSolidImageByBitmapMask(dimmedColor, BackColor, Resources.hotkey_presets, dimmedWeight);

            UncheckAllFiltersAccent = Plugin.GetSolidImageByBitmapMask(accentColor, BackColor, Resources.uncheck_all_preset_filters, accentWeight);
            UncheckAllFiltersDimmed = Plugin.GetSolidImageByBitmapMask(dimmedColor, BackColor, Resources.uncheck_all_preset_filters, dimmedWeight);

            tickedOnlyPictureBox.Image = AutoAppliedPresetsDimmed;
            predefinedPictureBox.Image = PredefinedPresetsDimmed;
            customizedPictureBox.Image = CustomizedPresetsDimmed;
            userPictureBox.Image = UserPresetsDimmed;
            playlistPictureBox.Image = PlaylistPresetsDimmed;
            functionIdPictureBox.Image = FunctionIdPresetsDimmed;
            hotkeyPictureBox.Image = HotkeyPresetsDimmed;


            ignorefFlterComboBoxSelectedIndexChanged = true;
            filterComboBox.SelectedIndex = 0;
            ignorefFlterComboBoxSelectedIndexChanged = false;


            Color highlightColor = SystemColors.Highlight;

            float highlightWeight = 0.65f;//***
            descriptionBox.ForeColor = Plugin.GetHighlightColor(highlightColor, BackColor, highlightWeight);
            descriptionBox.BackColor = BackColor;


            float hotTrackWeight = 0.8f;

            //Color hotTrack = SystemColors.HotTrack;
            Color hotTrack = SystemColors.Highlight;

            UntickedColor = ForeColor;
            TickedColor = Plugin.GetHighlightColor(hotTrack, BackColor, hotTrackWeight);


            presetsWorkingCopy = new SortedDictionary<Guid, Preset>();
            foreach (var tempPreset in Presets.Values)
            {
                Preset presetCopy = new Preset(tempPreset);
                presetsWorkingCopy.Add(presetCopy.guid, presetCopy);
            }

            autoAppliedAsrPresetGuids = new List<Guid>();
            foreach (Guid guid in Plugin.SavedSettings.autoAppliedAsrPresetGuids)
                autoAppliedAsrPresetGuids.Add(guid);

            asrPresetsWithHotkeysGuids = new Guid[Plugin.MaximumNumberOfASRHotkeys];
            for (int j = 0; j < Plugin.MaximumNumberOfASRHotkeys; j++)
            {
                if (Plugin.SavedSettings.asrPresetsWithHotkeysGuids[j] != Guid.Empty)
                {
                    asrPresetsWithHotkeysGuids[j] = Plugin.SavedSettings.asrPresetsWithHotkeysGuids[j];
                    presetsWorkingCopy[asrPresetsWithHotkeysGuids[j]].hotkeyAssigned = true;
                }
                else
                {
                    asrPresetsWithHotkeysGuids[j] = Guid.Empty;
                }
            }

            asrIdsPresetGuids = new SortedDictionary<string, Guid>();
            foreach (var pair in Plugin.AsrIdsPresets)
            {
                asrIdsPresetGuids.Add(pair.Key, pair.Value.guid);
            }


            string entireText = label5.Text;
            editApplyText = Regex.Replace(entireText, @"(.*?\.\s).*", "$1").TrimEnd('\n');
            autoApplyText = Regex.Replace(entireText, @".*?\.\s(.*?\.\s).*", "$1").TrimEnd('\n');
            clickHereText = Regex.Replace(entireText, @".*?\.\s.*?\.\s(.*?\.\s).*", "$1").TrimEnd('\n');
            nowTickedText = Regex.Replace(entireText, @".*?\.\s.*?\.\s.*?\.\s(.*)", "$1").TrimEnd('\n');

            processPresetCheckBoxCheckedEvent = false;

            assignHotkeyCheckBox.Enabled = false;
            assignHotkeyCheckBox.Checked = false;

            processPresetCheckBoxCheckedEvent = true;

            presetList.Items.Clear();
            presetList.Sorted = false;
            ignoreCheckedPresetEvent = false;
            autoAppliedPresetCount = 0;
            int i = 0;
            foreach (Preset tempPreset in presetsWorkingCopy.Values)
            {
                preset = tempPreset;
                presetList.Items.Add(tempPreset);
                presetList.SetItemChecked(i, autoAppliedAsrPresetGuids.Contains(tempPreset.guid));
                i++;
            }
            presetListItemCheckChanged();
            ignoreCheckedPresetEvent = true;
            presetList.Sorted = true;

            asrPresetsWithHotkeysCount = Plugin.ASRPresetsWithHotkeysCount;
            assignHotkeyCheckBoxText = assignHotkeyCheckBox.Text;
            assignHotkeyCheckBox.Text = assignHotkeyCheckBoxText + (Plugin.MaximumNumberOfASRHotkeys - asrPresetsWithHotkeysCount) + "/" + Plugin.MaximumNumberOfASRHotkeys;

            DatagridViewCheckBoxHeaderCell cbHeader = new DatagridViewCheckBoxHeaderCell();
            cbHeader.setState(true);
            cbHeader.OnCheckBoxClicked += new CheckBoxClickedHandler(cbHeader_OnCheckBoxClicked);

            DataGridViewCheckBoxColumn colCB = new DataGridViewCheckBoxColumn
            {
                HeaderCell = cbHeader,
                ThreeState = true,
                FalseValue = "False",
                TrueValue = "True",
                IndeterminateValue = "",
                Width = 25,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };

            previewTable.Columns.Insert(0, colCB);

            previewTable.Columns[2].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[3].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[4].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[5].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[6].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[7].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[8].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[9].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[10].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[11].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[12].HeaderCell.Style.WrapMode = DataGridViewTriState.True;

            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            newValueText = previewTable.Columns[4].HeaderText;

            Plugin.MbApiInterface.Playlist_QueryPlaylists();
            string playlist = Plugin.MbApiInterface.Playlist_QueryGetNextPlaylist();

            while (playlist != null)
            {
                //if (mbApiInterface.Playlist_GetType(playlist) == Plugin.PlaylistFormat.Auto)
                {
                    Playlist newPlaylist = new Playlist(playlist);
                    playlistComboBox.Items.Add(newPlaylist);
                }

                playlist = Plugin.MbApiInterface.Playlist_QueryGetNextPlaylist();
            }

            presetList_SelectedIndexChanged(null, null);

            buttonClose.Image = Resources.Empty_14;
            buttonCloseToolTip = toolTip1.GetToolTip(buttonClose);
            toolTip1.SetToolTip(buttonClose, "");

            addRowToTable = previewList_AddRowToTable;
            processRowOfTable = previewList_ProcessRowOfTable;
        }

        private static int FindFirstSlot(Guid[] presetGuids, Guid searchedGuid)
        {
            for (int i = 0; i < presetGuids.Length; i++)
            {
                if (presetGuids[i] == searchedGuid)
                    return i;
            }

            return -1;
        }

        protected static bool SetFileTag(string sourceFileUrl, Plugin.MetaDataType tagId, string value, bool updateOnlyChangedTags, AdvancedSearchAndReplaceCommand plugin)
        {
            if (tagId == Plugin.ClipboardTagId)
            {
                if (plugin == null)
                    return false;

                if (plugin.clipboardText == "")
                {
                    plugin.clipboardText += value;
                    return true;
                }
                else
                {
                    plugin.clipboardText += "\r\n" + value;
                    return true;
                }
            }

            string sourceTagValue = GetTag(sourceFileUrl, plugin, (int)tagId);
            SetTag((int)tagId, value);

            if (!updateOnlyChangedTags || sourceTagValue != value)
                return Plugin.SetFileTag(sourceFileUrl, tagId, value, false);
            else
                return true;
        }

        private void previewList_AddRowToTable(string[] row)
        {
            previewTable.Rows.Add(row);
        }

        private void previewList_ProcessRowOfTable(int row)
        {
            previewTable.Rows[row].Cells[0].Value = "";
        }

        public static string Replace(string currentFile, string value, string searchedPattern, string replacedPattern, string preserveValues, bool ignoreCase, out bool match)
        {
            match = false;
            if (searchedPattern == "")
                return value;


            try
            {
                if (!string.IsNullOrEmpty(preserveValues) && (preserveValues + ";;").Contains(value + ";;"))
                    return value;


                match = Regex.IsMatch(value, searchedPattern, RegexOptions.IgnoreCase);

                if (!match)
                    return value;


                replacedPattern = replacedPattern.Replace(";;", "\u0011").Replace("[[", "\u0017").Replace("]]", "\u0018");

                if (ignoreCase)
                    value = Regex.Replace(value, searchedPattern, replacedPattern, RegexOptions.IgnoreCase);
                else
                    value = Regex.Replace(value, searchedPattern, replacedPattern, RegexOptions.None);

                value = value.Replace("\u0017", "[[").Replace("\u0018", "]]").Replace("\u0011", ";;");


                //Lets evaluate all supported functions
                Function nullFunction = new Function();
                value = nullFunction.evaluate(currentFile, value);

                Function rg2sc = new Rg2sc();
                value = rg2sc.evaluate(currentFile, value);

                Function rg2sc4mp3 = new Rg2sc4mp3();
                value = rg2sc4mp3.evaluate(currentFile, value);

                Function tc = new Tc();
                value = tc.evaluate(currentFile, value);

                Function lc = new Lc();
                value = lc.evaluate(currentFile, value);

                Function uc = new Uc();
                value = uc.evaluate(currentFile, value);

                Function sc = new Sc();
                value = sc.evaluate(currentFile, value);

                Function eval = new Eval();
                value = eval.evaluate(currentFile, value);

                Function char1 = new Char1();
                value = char1.evaluate(currentFile, value);

                Function repunct = new Repunct();
                value = repunct.evaluate(currentFile, value);

                Function sortperformers = new SortPerformers();
                value = sortperformers.evaluate(currentFile, value);

                Function searchreplace = new SearchReplace();
                value = searchreplace.evaluate(currentFile, value);
            }
            catch (Exception)
            {
                //MessageBox.Show(Plugin.MbForm, ex.Message);

                return "SYNTAX ERROR!";
            }

            return value;
        }

        public static string GetTag(string currentFile, AdvancedSearchAndReplaceCommand plugin, int tagId)
        {
            if (tagId == (int)Plugin.ClipboardTagId)
            {
                if (plugin == null)
                    return "";

                if (plugin.fileTags == null)
                    return "";

                int position = -1;
                for (int i = 0; i < plugin.files.Length; i++)
                {
                    if (plugin.files[i] == currentFile)
                    {
                        position = i;
                        break;
                    }
                }

                return plugin.fileTags[position];
            }


            SetTags.TryGetValue(tagId, out string tempTag);

            if (tempTag == null)
            {
                if ((int)tagId < PropMetaDataThreshold)
                    return Plugin.GetFileTag(currentFile, (Plugin.MetaDataType)tagId);
                else
                    return Plugin.MbApiInterface.Library_GetFileProperty(currentFile, (Plugin.FilePropertyType)(tagId - PropMetaDataThreshold));
            }
            else
            {
                return tempTag;
            }
        }

        public static void SetTag(int tagId, string tagValue)
        {
            if (SetTags.TryGetValue(tagId, out _))
                SetTags.Remove(tagId);

            SetTags.Add(tagId, tagValue);

            LastSetTagValue = tagValue;
        }

        public static void SetReplacedTag(string currentFile, AdvancedSearchAndReplaceCommand plugin, int searchedTagId, int replacedTagId, string searchedPattern, string replacedPattern,
            string preserveValues, bool ignoreCase, bool append, out string searchedTagValue, out string replacedTagValue, out string originalReplacedTagValue)
        {
            string sourceTagValue;
            string newTagValue;
            string originalNewTagValue;


            if (searchedPattern == "")
            {
                searchedTagValue = "";
                replacedTagValue = "";
                originalReplacedTagValue = "";
                return;
            }

            sourceTagValue = GetTag(currentFile, plugin, searchedTagId);
            originalNewTagValue = GetTag(currentFile, plugin, replacedTagId);

            newTagValue = Replace(currentFile, sourceTagValue, searchedPattern, replacedPattern, preserveValues, ignoreCase, out bool match);


            if (append)
                newTagValue = originalNewTagValue + newTagValue;

            SetTag(replacedTagId, newTagValue);

            if (match)
                replacedTagValue = newTagValue;
            else
                replacedTagValue = originalNewTagValue;

            searchedTagValue = sourceTagValue;
            originalReplacedTagValue = originalNewTagValue;
        }

        public static string ReplaceVariable(Preset presetParam, string pattern, bool searchedPattern)
        {
            CustomText1 = presetParam.customText;


            if (presetParam.customTextChecked && searchedPattern)
                pattern = Regex.Replace(pattern, @"\\@1", Regex.Escape(presetParam.customText));
            else if (presetParam.customTextChecked) //Replaced pattern
                pattern = Regex.Replace(pattern, @"\\@1", presetParam.customText);

            if (presetParam.customText2Checked && searchedPattern)
                pattern = Regex.Replace(pattern, @"\\@2", Regex.Escape(presetParam.customText2));
            else if (presetParam.customText2Checked) //Replaced pattern
                pattern = Regex.Replace(pattern, @"\\@2", presetParam.customText2);

            if (presetParam.customText3Checked && searchedPattern)
                pattern = Regex.Replace(pattern, @"\\@3", Regex.Escape(presetParam.customText3));
            else if (presetParam.customText3Checked) //Replaced pattern
                pattern = Regex.Replace(pattern, @"\\@3", presetParam.customText3);

            if (presetParam.customText4Checked && searchedPattern)
                pattern = Regex.Replace(pattern, @"\\@4", Regex.Escape(presetParam.customText4));
            else if (presetParam.customText4Checked) //Replaced pattern
                pattern = Regex.Replace(pattern, @"\\@4", presetParam.customText4);

            return pattern;
        }

        public static int SubstituteTagId(Preset presetParam, int tagId)
        {
            switch ((ServiceMetaData)tagId)
            {
                case ServiceMetaData.ParameterTagId1:
                    tagId = presetParam.parameterTagId;
                    break;
                case ServiceMetaData.ParameterTagId2:
                    tagId = presetParam.parameterTag2Id;
                    break;
                case ServiceMetaData.ParameterTagId3:
                    tagId = presetParam.parameterTag3Id;
                    break;
                case ServiceMetaData.ParameterTagId4:
                    tagId = presetParam.parameterTag4Id;
                    break;
                case ServiceMetaData.ParameterTagId5:
                    tagId = presetParam.parameterTag5Id;
                    break;
                case ServiceMetaData.ParameterTagId6:
                    tagId = presetParam.parameterTag6Id;
                    break;
            }

            return tagId;
        }

        public static void GetReplacedTags(string currentFile, Preset presetParam, AdvancedSearchAndReplaceCommand plugin)
        {
            SearchedAndReplacedTags = new SearchedAndReplacedTagsStruct();
            SetTags.Clear();

            SetReplacedTag(currentFile, plugin, SubstituteTagId(presetParam, presetParam.searchedTagId), SubstituteTagId(presetParam, presetParam.replacedTagId),
                ReplaceVariable(presetParam, presetParam.searchedPattern, true), ReplaceVariable(presetParam, presetParam.replacedPattern, false), presetParam.preserveValues, presetParam.ignoreCase, presetParam.append,
                out SearchedAndReplacedTags.searchedTagValue, out SearchedAndReplacedTags.replacedTagValue, out SearchedAndReplacedTags.originalReplacedTagValue);
            SetReplacedTag(currentFile, plugin, SubstituteTagId(presetParam, presetParam.searchedTag2Id), SubstituteTagId(presetParam, presetParam.replacedTag2Id),
                ReplaceVariable(presetParam, presetParam.searchedPattern2, true), ReplaceVariable(presetParam, presetParam.replacedPattern2, false), presetParam.preserveValues, presetParam.ignoreCase, presetParam.append2,
                out SearchedAndReplacedTags.searchedTag2Value, out SearchedAndReplacedTags.replacedTag2Value, out SearchedAndReplacedTags.originalReplacedTag2Value);
            SetReplacedTag(currentFile, plugin, SubstituteTagId(presetParam, presetParam.searchedTag3Id), SubstituteTagId(presetParam, presetParam.replacedTag3Id),
                ReplaceVariable(presetParam, presetParam.searchedPattern3, true), ReplaceVariable(presetParam, presetParam.replacedPattern3, false), presetParam.preserveValues, presetParam.ignoreCase, presetParam.append3,
                out SearchedAndReplacedTags.searchedTag3Value, out SearchedAndReplacedTags.replacedTag3Value, out SearchedAndReplacedTags.originalReplacedTag3Value);
            SetReplacedTag(currentFile, plugin, SubstituteTagId(presetParam, presetParam.searchedTag4Id), SubstituteTagId(presetParam, presetParam.replacedTag4Id),
                ReplaceVariable(presetParam, presetParam.searchedPattern4, true), ReplaceVariable(presetParam, presetParam.replacedPattern4, false), presetParam.preserveValues, presetParam.ignoreCase, presetParam.append4,
                out SearchedAndReplacedTags.searchedTag4Value, out SearchedAndReplacedTags.replacedTag4Value, out SearchedAndReplacedTags.originalReplacedTag4Value);
            SetReplacedTag(currentFile, plugin, SubstituteTagId(presetParam, presetParam.searchedTag5Id), SubstituteTagId(presetParam, presetParam.replacedTag5Id),
                ReplaceVariable(presetParam, presetParam.searchedPattern5, true), ReplaceVariable(presetParam, presetParam.replacedPattern5, false), presetParam.preserveValues, presetParam.ignoreCase, presetParam.append5,
                out SearchedAndReplacedTags.searchedTag5Value, out SearchedAndReplacedTags.replacedTag5Value, out SearchedAndReplacedTags.originalReplacedTag5Value);
        }

        public static void SaveReplacedTags(string currentFile, Preset presetParam, AdvancedSearchAndReplaceCommand plugin)
        {
            if (SearchedAndReplacedTags.replacedTagValue == "SYNTAX ERROR!" || SearchedAndReplacedTags.replacedTag2Value == "SYNTAX ERROR!" ||
                SearchedAndReplacedTags.replacedTag3Value == "SYNTAX ERROR!" || SearchedAndReplacedTags.replacedTag4Value == "SYNTAX ERROR!" ||
                SearchedAndReplacedTags.replacedTag5Value == "SYNTAX ERROR!")
                return;

            SetTags.Clear();

            if (presetParam.searchedPattern != "")
            {
                SetFileTag(currentFile, (Plugin.MetaDataType)SubstituteTagId(presetParam, presetParam.replacedTagId), SearchedAndReplacedTags.replacedTagValue, true, plugin);

                if (presetParam.searchedPattern2 != "")
                {
                    SetFileTag(currentFile, (Plugin.MetaDataType)SubstituteTagId(presetParam, presetParam.replacedTag2Id), SearchedAndReplacedTags.replacedTag2Value, true, plugin);

                    if (presetParam.searchedPattern3 != "")
                    {
                        SetFileTag(currentFile, (Plugin.MetaDataType)SubstituteTagId(presetParam, presetParam.replacedTag3Id), SearchedAndReplacedTags.replacedTag3Value, true, plugin);

                        if (presetParam.searchedPattern4 != "")
                        {
                            SetFileTag(currentFile, (Plugin.MetaDataType)SubstituteTagId(presetParam, presetParam.replacedTag4Id), SearchedAndReplacedTags.replacedTag4Value, true, plugin);

                            if (presetParam.searchedPattern5 != "")
                            {
                                SetFileTag(currentFile, (Plugin.MetaDataType)SubstituteTagId(presetParam, presetParam.replacedTag5Id), SearchedAndReplacedTags.replacedTag5Value, true, plugin);
                            }
                        }
                    }
                }
            }


            Plugin.CommitTagsToFile(currentFile, true, true);
        }

        public static bool DetectTagChanges(Preset presetParam)
        {
            bool wereChanges = false;

            if (presetParam.searchedPattern != "")
            {
                if (SearchedAndReplacedTags.originalReplacedTagValue != SearchedAndReplacedTags.replacedTagValue)
                {
                    wereChanges = true;
                    goto exit;
                }

                if (presetParam.searchedPattern2 != "")
                {
                    if (SearchedAndReplacedTags.originalReplacedTag2Value != SearchedAndReplacedTags.replacedTag2Value)
                    {
                        wereChanges = true;
                        goto exit;
                    }

                    if (presetParam.searchedPattern3 != "")
                    {
                        if (SearchedAndReplacedTags.originalReplacedTag3Value != SearchedAndReplacedTags.replacedTag3Value)
                        {
                            wereChanges = true;
                            goto exit;
                        }

                        if (presetParam.searchedPattern4 != "")
                        {
                            if (SearchedAndReplacedTags.originalReplacedTag4Value != SearchedAndReplacedTags.replacedTag4Value)
                            {
                                wereChanges = true;
                                goto exit;
                            }

                            if (presetParam.searchedPattern5 != "")
                            {
                                if (SearchedAndReplacedTags.originalReplacedTag5Value != SearchedAndReplacedTags.replacedTag5Value)
                                {
                                    wereChanges = true;
                                    goto exit;
                                }
                            }
                        }
                    }
                }
            }


        exit: return wereChanges;
        }

        public static void ReplaceTags(string currentFile, Preset presetParam)
        {
            lock (Presets)
            {
                GetReplacedTags(currentFile, presetParam, null);
                SaveReplacedTags(currentFile, presetParam, null);
            }
        }

        public static void AutoApply(object currentFileObj, object tagToolsPluginObj)
        {
            string currentFile = (string)currentFileObj;
            Plugin tagToolsPluginParam = (Plugin)tagToolsPluginObj;

            SortedDictionary<Guid, bool> appliedPresets = new SortedDictionary<Guid, bool>();

            lock (Plugin.AutoAppliedPresets)
            {
                if (Plugin.AutoAppliedPresets.Count > 0)
                {
                    foreach (Preset tempPreset in Plugin.AutoAppliedPresets)
                    {
                        bool conditionSatisfied = true;

                        if (tempPreset.condition)
                        {
                            conditionSatisfied = false;

                            Plugin.MbApiInterface.Playlist_QueryPlaylists();
                            string playlist = Plugin.MbApiInterface.Playlist_QueryGetNextPlaylist();

                            while (playlist != null)
                            {
                                if (playlist == tempPreset.playlist)
                                {
                                    conditionSatisfied = Plugin.MbApiInterface.Playlist_IsInList(playlist, currentFile);
                                    break;
                                }

                                playlist = Plugin.MbApiInterface.Playlist_QueryGetNextPlaylist();
                            }
                        }

                        if (conditionSatisfied)
                        {
                            ReplaceTags(currentFile, tempPreset);

                            if (!appliedPresets.TryGetValue(tempPreset.guid, out _))
                                appliedPresets.Add(tempPreset.guid, false);
                        }
                    }

                    if (appliedPresets.Count > 1)
                        Plugin.SetStatusbarText(Plugin.SbAsrPresetsAreApplied.Replace("%PRESETCOUNT%", appliedPresets.Count.ToString()), true);
                    else if (appliedPresets.Count == 1)
                        Plugin.SetStatusbarText(Plugin.SbAsrPresetIsApplied.Replace("%PRESETNAME%", Presets[appliedPresets.ElementAt(0).Key].getName()), true);

                    Plugin.RefreshPanels(true);
                }
            }
        }

        public static string GetLastReplacedTag(string currentFile, Preset preset)
        {
            bool conditionSatisfied = true;

            if (preset.condition)
            {
                conditionSatisfied = false;

                Plugin.MbApiInterface.Playlist_QueryPlaylists();
                string playlist = Plugin.MbApiInterface.Playlist_QueryGetNextPlaylist();

                while (playlist != null)
                {
                    if (playlist == preset.playlist)
                    {
                        conditionSatisfied = Plugin.MbApiInterface.Playlist_IsInList(playlist, currentFile);
                        break;
                    }

                    playlist = Plugin.MbApiInterface.Playlist_QueryGetNextPlaylist();
                }
            }

            if (conditionSatisfied)
            {
                GetReplacedTags(currentFile, preset, null);
                return LastSetTagValue;
            }

            return "";
        }

        public static void Apply(int hotkeySlot)
        {
            Preset tempPreset = Plugin.AsrPresetsWithHotkeys[hotkeySlot - 1];

            if (tempPreset == null)
                return;

            bool conditionSatisfied = true;

            if (!tempPreset.applyToPlayingTrack)
            {
                if (Plugin.MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out string[] files))
                {
                    if (files.Length == 0)
                    {
                        MessageBox.Show(Plugin.MbForm, Plugin.MsgSelectTrack, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    for (int i = 0; i < files.Length; i++)
                    {
                        string currentFile = files[i];

                        if (tempPreset.condition)
                        {
                            conditionSatisfied = false;

                            Plugin.MbApiInterface.Playlist_QueryPlaylists();
                            string playlist = Plugin.MbApiInterface.Playlist_QueryGetNextPlaylist();

                            while (playlist != null)
                            {
                                if (playlist == tempPreset.playlist)
                                {
                                    conditionSatisfied = Plugin.MbApiInterface.Playlist_IsInList(playlist, currentFile);
                                    break;
                                }

                                playlist = Plugin.MbApiInterface.Playlist_QueryGetNextPlaylist();
                            }
                        }

                        if (conditionSatisfied)
                        {
                            ReplaceTags(currentFile, tempPreset);
                            Plugin.SetStatusbarText(Plugin.SbAsrPresetIsApplied.Replace("%PRESETNAME%", tempPreset.getName()), true);
                        }
                    }
                }
            }
            else
            {
                string currentFile = Plugin.MbApiInterface.NowPlaying_GetFileUrl();

                if (!string.IsNullOrEmpty(currentFile))
                {
                    if (tempPreset.condition)
                    {
                        conditionSatisfied = false;

                        Plugin.MbApiInterface.Playlist_QueryPlaylists();
                        string playlist = Plugin.MbApiInterface.Playlist_QueryGetNextPlaylist();

                        while (playlist != null)
                        {
                            if (playlist == tempPreset.playlist)
                            {
                                conditionSatisfied = Plugin.MbApiInterface.Playlist_IsInList(playlist, currentFile);
                                break;
                            }

                            playlist = Plugin.MbApiInterface.Playlist_QueryGetNextPlaylist();
                        }
                    }

                    if (conditionSatisfied)
                    {
                        ReplaceTags(currentFile, tempPreset);
                        Plugin.SetStatusbarText(Plugin.SbAsrPresetIsApplied.Replace("%PRESETNAME%", tempPreset.getName()), true);
                    }
                }
            }

            Plugin.RefreshPanels(true);
        }

        private bool prepareBackgroundPreview()
        {
            tags.Clear();
            previewTable.Rows.Clear();
            ((DatagridViewCheckBoxHeaderCell)previewTable.Columns[0].HeaderCell).setState(true);

            if (backgroundTaskIsWorking())
                return true;

            clipboardText = "";
            fileTags = null;



            files = null;
            if (!Plugin.MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files))
                files = new string[0];



            Preset preset = (Preset)presetList.SelectedItem;
            if (preset.searchedTagId == (int)Plugin.ClipboardTagId || preset.searchedTag2Id == (int)Plugin.ClipboardTagId || preset.searchedTag3Id == (int)Plugin.ClipboardTagId
                || preset.searchedTag4Id == (int)Plugin.ClipboardTagId || preset.searchedTag5Id == (int)Plugin.ClipboardTagId)
            {
                if (!Clipboard.ContainsText())
                {
                    MessageBox.Show(this, Plugin.MsgClipboardDesntContainText, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                fileTags = Clipboard.GetText().Split(new string[] { "\r\n" }, StringSplitOptions.None);

                if (fileTags.Length != files.Length)
                {
                    MessageBox.Show(this, Plugin.MsgNumberOfTagsInClipboard + fileTags.Length 
                        + Plugin.MsgDoesntCorrespondToNumberOfSelectedTracksC + files.Length + Plugin.MsgMessageEndC, 
                        null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }


            if (files.Length == 0)
            {
                MessageBox.Show(this, Plugin.MsgNoFilesSelected, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool prepareBackgroundTask()
        {
            if (backgroundTaskIsWorking())
                return true;

            currentPreset = (Preset)presetList.SelectedItem;

            if (previewTable.Rows.Count == 0)
            {
                return prepareBackgroundPreview();
            }
            else
            {
                string[] tag;

                tags.Clear();

                for (int fileCounter = 0; fileCounter < previewTable.Rows.Count; fileCounter++)
                {
                    tag = new string[7];

                    tag[0] = (string)previewTable.Rows[fileCounter].Cells[0].Value;
                    tag[1] = (string)previewTable.Rows[fileCounter].Cells[1].Value;
                    tag[2] = (string)previewTable.Rows[fileCounter].Cells[4].Value;
                    tag[3] = (string)previewTable.Rows[fileCounter].Cells[6].Value;
                    tag[4] = (string)previewTable.Rows[fileCounter].Cells[8].Value;
                    tag[5] = (string)previewTable.Rows[fileCounter].Cells[10].Value;
                    tag[6] = (string)previewTable.Rows[fileCounter].Cells[12].Value;

                    tags.Add(tag);
                }

                return true;
            }
        }

        private void previewChanges()
        {
            string currentFile;

            string track;
            string[] row = { "Checked", "File", "Track", "OriginalTag1", "NewTag1", "OriginalTag2", "NewTag2", "OriginalTag3", "NewTag3", "OriginalTag4", "NewTag4", "OriginalTag5", "NewTag5" };
            string[] tag = { "Checked", "file", "newTag1", "newTag2", "newTag3", "newTag4", "newTag5" };

            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                currentFile = files[fileCounter];

                Plugin.SetStatusbarTextForFileOperations(Plugin.AsrCommandSbText, true, fileCounter, files.Length, currentFile);

                track = Plugin.GetTrackRepresentation(currentFile);

                lock (Presets)
                {
                    GetReplacedTags(currentFile, preset, this);

                    if (SearchedAndReplacedTags.originalReplacedTagValue != SearchedAndReplacedTags.replacedTagValue ||
                        SearchedAndReplacedTags.originalReplacedTag2Value != SearchedAndReplacedTags.replacedTag2Value ||
                        SearchedAndReplacedTags.originalReplacedTag3Value != SearchedAndReplacedTags.replacedTag3Value ||
                        SearchedAndReplacedTags.originalReplacedTag4Value != SearchedAndReplacedTags.replacedTag4Value ||
                        SearchedAndReplacedTags.originalReplacedTag5Value != SearchedAndReplacedTags.replacedTag5Value)
                    {
                        tag = new string[7];

                        tag[0] = "True";
                        tag[1] = currentFile;
                        tag[2] = SearchedAndReplacedTags.replacedTagValue;
                        tag[3] = SearchedAndReplacedTags.replacedTag2Value;
                        tag[4] = SearchedAndReplacedTags.replacedTag3Value;
                        tag[5] = SearchedAndReplacedTags.replacedTag4Value;
                        tag[6] = SearchedAndReplacedTags.replacedTag5Value;


                        row = new string[13];

                        row[0] = "True";
                        row[1] = currentFile;
                        row[2] = track;
                        row[3] = SearchedAndReplacedTags.originalReplacedTagValue;
                        row[4] = SearchedAndReplacedTags.replacedTagValue;
                        row[5] = SearchedAndReplacedTags.originalReplacedTag2Value;
                        row[6] = SearchedAndReplacedTags.replacedTag2Value;
                        row[7] = SearchedAndReplacedTags.originalReplacedTag3Value;
                        row[8] = SearchedAndReplacedTags.replacedTag3Value;
                        row[9] = SearchedAndReplacedTags.originalReplacedTag4Value;
                        row[10] = SearchedAndReplacedTags.replacedTag4Value;
                        row[11] = SearchedAndReplacedTags.originalReplacedTag5Value;
                        row[12] = SearchedAndReplacedTags.replacedTag5Value;

                        Invoke(addRowToTable, new Object[] { row });

                        tags.Add(tag);

                        previewIsGenerated = true;
                    }
                }
            }

            Plugin.SetStatusbarTextForFileOperations(Plugin.AsrCommandSbText, true, files.Length - 1, files.Length, null, true);
        }

        private void applyToSelected()
        {
            string currentFile;
            string isChecked;

            if (tags.Count == 0)
                previewChanges();

            for (int i = 0; i < tags.Count; i++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                isChecked = tags[i][0];
                currentFile = tags[i][1];

                if (isChecked == "True")
                {
                    lock (Presets)
                    {
                        tags[i][0] = "";

                        SearchedAndReplacedTags.replacedTagValue = tags[i][2];
                        SearchedAndReplacedTags.replacedTag2Value = tags[i][3];
                        SearchedAndReplacedTags.replacedTag3Value = tags[i][4];
                        SearchedAndReplacedTags.replacedTag4Value = tags[i][5];
                        SearchedAndReplacedTags.replacedTag5Value = tags[i][6];

                        SaveReplacedTags(currentFile, preset, this);
                    }

                    Invoke(processRowOfTable, new Object[] { i });
                }

                Plugin.SetStatusbarTextForFileOperations(Plugin.AsrCommandSbText, false, i, tags.Count, currentFile);
            }


            if (currentPreset.replacedTagId == (int)Plugin.ClipboardTagId || currentPreset.replacedTag2Id == (int)Plugin.ClipboardTagId || currentPreset.replacedTag3Id == (int)Plugin.ClipboardTagId
                || currentPreset.replacedTag4Id == (int)Plugin.ClipboardTagId || currentPreset.replacedTag5Id == (int)Plugin.ClipboardTagId)
            {
                System.Threading.Thread thread = new System.Threading.Thread(() => Clipboard.SetText(clipboardText));
                thread.SetApartmentState(System.Threading.ApartmentState.STA); //Set the thread to STA
                thread.Start();
                thread.Join();
            }


            Plugin.RefreshPanels(true);

            Plugin.SetStatusbarTextForFileOperations(Plugin.AsrCommandSbText, false, tags.Count - 1, tags.Count, null, true);
        }

        public string getTagName(int tagId)
        {
            string tagName;

            if (tagId == (int)ServiceMetaData.ParameterTagId1)
                tagName = "<" + Plugin.ParameterTagName + " 1>";
            else if (tagId == (int)ServiceMetaData.ParameterTagId2)
                tagName = "<" + Plugin.ParameterTagName + " 2>";
            else if (tagId == (int)ServiceMetaData.ParameterTagId3)
                tagName = "<" + Plugin.ParameterTagName + " 3>";
            else if (tagId == (int)ServiceMetaData.ParameterTagId4)
                tagName = "<" + Plugin.ParameterTagName + " 4>";
            else if (tagId == (int)ServiceMetaData.ParameterTagId5)
                tagName = "<" + Plugin.ParameterTagName + " 5>";
            else if (tagId == (int)ServiceMetaData.ParameterTagId6)
                tagName = "<" + Plugin.ParameterTagName + " 6>";
            else if (tagId == (int)ServiceMetaData.TempTag1)
                tagName = "<" + Plugin.TempTagName + " 1>";
            else if (tagId == (int)ServiceMetaData.TempTag2)
                tagName = "<" + Plugin.TempTagName + " 2>";
            else if (tagId == (int)ServiceMetaData.TempTag3)
                tagName = "<" + Plugin.TempTagName + " 3>";
            else if (tagId == (int)ServiceMetaData.TempTag4)
                tagName = "<" + Plugin.TempTagName + " 4>";
            else if (tagId == (int)Plugin.ClipboardTagId)
                tagName = Plugin.ClipboardTagName;
            else if (tagId < PropMetaDataThreshold)
                tagName = Plugin.GetTagName((Plugin.MetaDataType)tagId);
            else
                tagName = Plugin.GetPropName((Plugin.FilePropertyType)(tagId - PropMetaDataThreshold));

            return tagName;
        }

        public int getTagId(string tagName)
        {
            int tagId;

            if (tagName == "<" + Plugin.ParameterTagName + " 1>")
                tagId = (int)ServiceMetaData.ParameterTagId1;
            else if (tagName == "<" + Plugin.ParameterTagName + " 2>")
                tagId = (int)ServiceMetaData.ParameterTagId2;
            else if (tagName == "<" + Plugin.ParameterTagName + " 3>")
                tagId = (int)ServiceMetaData.ParameterTagId3;
            else if (tagName == "<" + Plugin.ParameterTagName + " 4>")
                tagId = (int)ServiceMetaData.ParameterTagId4;
            else if (tagName == "<" + Plugin.ParameterTagName + " 5>")
                tagId = (int)ServiceMetaData.ParameterTagId5;
            else if (tagName == "<" + Plugin.ParameterTagName + " 6>")
                tagId = (int)ServiceMetaData.ParameterTagId6;
            else if (tagName == "<" + Plugin.TempTagName + " 1>")
                tagId = (int)ServiceMetaData.TempTag1;
            else if (tagName == "<" + Plugin.TempTagName + " 2>")
                tagId = (int)ServiceMetaData.TempTag2;
            else if (tagName == "<" + Plugin.TempTagName + " 3>")
                tagId = (int)ServiceMetaData.TempTag3;
            else if (tagName == "<" + Plugin.TempTagName + " 4>")
                tagId = (int)ServiceMetaData.TempTag4;
            else if (tagName == Plugin.ClipboardTagName)
                tagId = (int)Plugin.ClipboardTagId;
            else
                tagId = (int)Plugin.GetTagId(tagName);


            if (tagId == 0)
            {
                tagId = (int)Plugin.GetPropId(tagName);

                if (tagId != 0)
                    tagId += PropMetaDataThreshold;
            }

            return tagId;
        }

        private void editPreset(Preset tempPreset, bool itsNewPreset, bool readOnly)
        {
            bool presetChanged;
            using (ASRPresetEditor tagToolsForm = new ASRPresetEditor(TagToolsPlugin, this))
            {
                presetChanged = tagToolsForm.editPreset(ref tempPreset, readOnly);
            }

            if (presetChanged)
            {
                if (!readOnly)
                {
                    tempPreset.modifiedUtc = DateTime.UtcNow;
                    presetsChanged = true;

                    buttonClose.Image = Resources.UnsavedChanges_14;
                    toolTip1.SetToolTip(buttonClose, buttonCloseToolTip);
                }
                else
                {
                    tempPreset.setCustomizationsFlag(this, backupedPreset);
                }

                if (itsNewPreset)
                {
                    presetsWorkingCopy.Add(tempPreset.guid, tempPreset);

                    presetList.Items.Add(tempPreset);
                    presetList.SelectedItem = tempPreset;
                }
                else
                {
                    presetList_SelectedIndexChanged(null, null);
                }
            }
        }

        private string getCountedPresetFilename(SortedDictionary<string, int> countedPresetFilenames, string presetFilename)
        {
            if (countedPresetFilenames.TryGetValue(presetFilename, out int count))
            {
                countedPresetFilenames.Remove(presetFilename);
                count++;
                countedPresetFilenames.Add(presetFilename, count);

                return presetFilename + " #" + count.ToString("D2");
            }
            else
            {
                countedPresetFilenames.Add(presetFilename, 1);

                return presetFilename;
            }
        }

        private void saveSettings()
        {
            SortedDictionary<string, bool> savedPresetPaths = new SortedDictionary<string, bool>();
            SortedDictionary<string, int> countedPresetFilenames = new SortedDictionary<string, int>();
            foreach (Preset tempPreset in presetsWorkingCopy.Values)
            {
                string presetFilename = getCountedPresetFilename(countedPresetFilenames, tempPreset.getSafeFileName());
                savedPresetPaths.Add(tempPreset.savePreset(Path.Combine(PresetsPath, presetFilename + Plugin.ASRPresetExtension)), false);
            }

            if (Plugin.MSR != null)
            {
                string presetFilename = getCountedPresetFilename(countedPresetFilenames, Plugin.MSR.getSafeFileName());
                savedPresetPaths.Add(Plugin.MSR.savePreset(Path.Combine(PresetsPath, presetFilename + Plugin.ASRPresetExtension)), false);
            }


            string[] presetNames = Directory.GetFiles(PresetsPath, "*");
            foreach (string presetName in presetNames)
            {
                if (!savedPresetPaths.TryGetValue(presetName, out _))
                    System.IO.File.Delete(presetName);
            }


            Presets = new SortedDictionary<Guid, Preset>();
            foreach (var tempPreset in presetsWorkingCopy.Values)
            {
                Preset presetCopy = new Preset(tempPreset);
                Presets.Add(presetCopy.guid, presetCopy);
            }

            Plugin.ASRPresetsWithHotkeysCount = asrPresetsWithHotkeysCount;

            Plugin.SavedSettings.asrPresetsWithHotkeysGuids = new Guid[Plugin.MaximumNumberOfASRHotkeys];
            Plugin.AsrPresetsWithHotkeys = new Preset[Plugin.MaximumNumberOfASRHotkeys];
            for (int j = 0; j < Plugin.MaximumNumberOfASRHotkeys; j++)
            {
                Plugin.SavedSettings.asrPresetsWithHotkeysGuids[j] = asrPresetsWithHotkeysGuids[j];

                if (asrPresetsWithHotkeysGuids[j] != Guid.Empty)
                    Plugin.AsrPresetsWithHotkeys[j] = Presets[asrPresetsWithHotkeysGuids[j]];
                else
                    Plugin.AsrPresetsWithHotkeys[j] = null;
            }

            Plugin.SavedSettings.autoAppliedAsrPresetGuids = new List<Guid>();
            Plugin.AutoAppliedPresets = new List<Preset>();
            foreach (Guid guid in autoAppliedAsrPresetGuids)
            {
                Plugin.SavedSettings.autoAppliedAsrPresetGuids.Add(guid);
                Plugin.AutoAppliedPresets.Add(Presets[guid]);
            }

            Plugin.AsrIdsPresets = new SortedDictionary<string, Preset>();
            foreach (var pair in asrIdsPresetGuids)
            {
                Plugin.AsrIdsPresets.Add(pair.Key, Presets[pair.Value]);
            }


            TagToolsPlugin.SaveSettings();

            presetsChanged = false;
            buttonClose.Image = Resources.Empty_14;
            toolTip1.SetToolTip(buttonClose, "");
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (prepareBackgroundTask())
                switchOperation(applyToSelected, (Button)sender, buttonOK, buttonPreview, buttonSaveClose, true, null);
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewChanges, (Button)sender, buttonOK, buttonSaveClose);
        }

        private void buttonSaveClose_Click(object sender, EventArgs e)
        {
            saveSettings();
            RegisterASRPresetsHotkeysAndMenuItems(TagToolsPlugin);
            Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            saveSettings();
            RegisterASRPresetsHotkeysAndMenuItems(TagToolsPlugin);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void deletePreset(Preset presetToRemove)
        {
            if (autoAppliedAsrPresetGuids.Contains(presetToRemove.guid))
            {
                autoAppliedPresetCount--;
                autoAppliedAsrPresetGuids.Remove(presetToRemove.guid);
            }



            idTextBox.Text = "";
            if (asrIdsPresetGuids.TryGetValue(presetToRemove.id, out _))
            {
                asrIdsPresetGuids.Remove(presetToRemove.id);
            }


            if (presetList.SelectedItem == presetToRemove)
            {
                assignHotkeyCheckBox.Checked = false;
            }
            else
            {
                for (int j = 0; j < asrPresetsWithHotkeysGuids.Length; j++)
                {
                    if (asrPresetsWithHotkeysGuids[j] == presetToRemove.guid)
                    {
                        asrPresetsWithHotkeysGuids[j] = Guid.Empty;
                        asrPresetsWithHotkeysCount--;
                        break;
                    }
                }
            }


            presetsWorkingCopy.Remove(presetToRemove.guid);
            if (presetList.Items.Contains(presetToRemove))
                presetList.Items.Remove(presetToRemove);

            presetsChanged = true;
            buttonClose.Image = Resources.UnsavedChanges_14;
            toolTip1.SetToolTip(buttonClose, buttonCloseToolTip);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this, Plugin.MsgDeletePresetConfirmation, "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No)
                return;

            deletePreset((Preset)presetList.SelectedItem);
            presetListItemCheckChanged();
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            editPreset(new Preset(preset, false, false, null, "*"), true, false);
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            editPreset(preset, false, false);
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            Preset newPreset = new Preset
            {
                names = new SerializableDictionary<string, string>(),
                descriptions = new SerializableDictionary<string, string>(),
                userPreset = true
            };

            newPreset.names.Add(Plugin.Language, Plugin.DefaultASRPresetName + (presetList.Items.Count + 1));
            newPreset.ignoreCase = true;

            editPreset(newPreset, true, false);
        }


        public void fillParameterTagList(int parameterTagType, ComboBox parameterTagListParam, Label parameterTagLabelParam)
        {
            parameterTagListParam.Items.Clear();

            if (parameterTagType == 0) //Not used
            {
                parameterTagListParam.Text = "";
                parameterTagListParam.Enabled = false;
                parameterTagLabelParam.Enabled = false;
            }
            else if (parameterTagType == 1) //Writable
            {
                Plugin.FillList(parameterTagListParam.Items);
                parameterTagListParam.Enabled = true;
                parameterTagLabelParam.Enabled = true;
            }
            else if (parameterTagType == 2) //Read only
            {
                Plugin.FillList(parameterTagListParam.Items, true);
                Plugin.FillListWithProps(parameterTagListParam.Items);
                parameterTagListParam.Enabled = true;
                parameterTagLabelParam.Enabled = true;
            }
        }

        public void nameColumns()
        {
            if (preset.searchedPattern != "")
            {
                previewTable.Columns[3].HeaderText = Plugin.GetTagName((Plugin.MetaDataType)SubstituteTagId(preset, preset.replacedTagId));
                previewTable.Columns[4].HeaderText = newValueText;

                if (preset.replacedTagId < -200 || preset.replacedTagId == (int)Plugin.NullTagId)
                {
                    previewTable.Columns[3].Visible = false;
                    previewTable.Columns[4].Visible = false;
                }
                else
                {
                    previewTable.Columns[3].Visible = true;
                    previewTable.Columns[4].Visible = true;
                }

                if (preset.columnWeights.Count > 2)
                {
                    previewTable.Columns[2].FillWeight = preset.columnWeights[0];
                    previewTable.Columns[3].FillWeight = preset.columnWeights[1];
                    previewTable.Columns[4].FillWeight = preset.columnWeights[2];
                }
                else
                {
                    previewTable.Columns[2].Width = 737;
                    previewTable.Columns[3].Width = 100;
                    previewTable.Columns[4].Width = 100;
                    previewTable.Columns[5].Width = 100;
                    previewTable.Columns[6].Width = 100;
                    previewTable.Columns[7].Width = 100;
                    previewTable.Columns[8].Width = 100;
                    previewTable.Columns[9].Width = 100;
                    previewTable.Columns[10].Width = 100;
                    previewTable.Columns[11].Width = 100;
                    previewTable.Columns[12].Width = 100;
                }


                if (preset.searchedPattern2 != "")
                {
                    previewTable.Columns[5].HeaderText = Plugin.GetTagName((Plugin.MetaDataType)SubstituteTagId(preset, preset.replacedTag2Id));
                    previewTable.Columns[6].HeaderText = newValueText;

                    if (preset.replacedTag2Id < -200 || preset.replacedTag2Id == (int)Plugin.NullTagId)
                    {
                        previewTable.Columns[5].Visible = false;
                        previewTable.Columns[6].Visible = false;
                    }
                    else
                    {
                        previewTable.Columns[5].Visible = true;
                        previewTable.Columns[6].Visible = true;
                    }

                    if (previewTable.Columns[5].HeaderText == previewTable.Columns[3].HeaderText)
                    {
                        previewTable.Columns[4].Visible = false;
                        previewTable.Columns[5].Visible = false;
                    }

                    if (preset.columnWeights.Count > 4)
                    {
                        previewTable.Columns[5].FillWeight = preset.columnWeights[3];
                        previewTable.Columns[6].FillWeight = preset.columnWeights[4];
                    }


                    if (preset.searchedPattern3 != "")
                    {
                        previewTable.Columns[7].HeaderText = Plugin.GetTagName((Plugin.MetaDataType)SubstituteTagId(preset, preset.replacedTag3Id));
                        previewTable.Columns[8].HeaderText = newValueText;

                        if (preset.replacedTag3Id < -200 || preset.replacedTag3Id == (int)Plugin.NullTagId)
                        {
                            previewTable.Columns[7].Visible = false;
                            previewTable.Columns[8].Visible = false;
                        }
                        else
                        {
                            previewTable.Columns[7].Visible = true;
                            previewTable.Columns[8].Visible = true;
                        }

                        if (previewTable.Columns[7].HeaderText == previewTable.Columns[5].HeaderText)
                        {
                            previewTable.Columns[6].Visible = false;
                            previewTable.Columns[7].Visible = false;
                        }

                        if (preset.columnWeights.Count > 6)
                        {
                            previewTable.Columns[7].FillWeight = preset.columnWeights[5];
                            previewTable.Columns[8].FillWeight = preset.columnWeights[6];
                        }


                        if (preset.searchedPattern4 != "")
                        {
                            previewTable.Columns[9].HeaderText = Plugin.GetTagName((Plugin.MetaDataType)SubstituteTagId(preset, preset.replacedTag4Id));
                            previewTable.Columns[10].HeaderText = newValueText;

                            if (preset.replacedTag4Id < -200 || preset.replacedTag4Id == (int)Plugin.NullTagId)
                            {
                                previewTable.Columns[9].Visible = false;
                                previewTable.Columns[10].Visible = false;
                            }
                            else
                            {
                                previewTable.Columns[9].Visible = true;
                                previewTable.Columns[10].Visible = true;
                            }

                            if (previewTable.Columns[9].HeaderText == previewTable.Columns[7].HeaderText)
                            {
                                previewTable.Columns[8].Visible = false;
                                previewTable.Columns[9].Visible = false;
                            }

                            if (preset.columnWeights.Count > 8)
                            {
                                previewTable.Columns[9].FillWeight = preset.columnWeights[7];
                                previewTable.Columns[10].FillWeight = preset.columnWeights[8];
                            }


                            if (preset.searchedPattern5 != "")
                            {
                                previewTable.Columns[11].HeaderText = Plugin.GetTagName((Plugin.MetaDataType)SubstituteTagId(preset, preset.replacedTag5Id));
                                previewTable.Columns[12].HeaderText = newValueText;

                                if (preset.replacedTag5Id < -200 || preset.replacedTag5Id == (int)Plugin.NullTagId)
                                {
                                    previewTable.Columns[11].Visible = false;
                                    previewTable.Columns[12].Visible = false;
                                }
                                else
                                {
                                    previewTable.Columns[11].Visible = (previewTable.Columns[11].HeaderText != "");
                                    previewTable.Columns[12].Visible = (previewTable.Columns[11].HeaderText != "");
                                }

                                if (previewTable.Columns[11].HeaderText == previewTable.Columns[9].HeaderText)
                                {
                                    previewTable.Columns[10].Visible = false;
                                    previewTable.Columns[11].Visible = false;
                                }

                                if (preset.columnWeights.Count > 10)
                                {
                                    previewTable.Columns[11].FillWeight = preset.columnWeights[9];
                                    previewTable.Columns[12].FillWeight = preset.columnWeights[10];
                                }
                            }
                            else
                            {
                                previewTable.Columns[11].HeaderText = "";
                                previewTable.Columns[12].HeaderText = "";

                                previewTable.Columns[11].Visible = false;
                                previewTable.Columns[12].Visible = false;
                            }
                        }
                        else
                        {
                            previewTable.Columns[9].HeaderText = "";
                            previewTable.Columns[10].HeaderText = "";
                            previewTable.Columns[11].HeaderText = "";
                            previewTable.Columns[12].HeaderText = "";

                            previewTable.Columns[9].Visible = false;
                            previewTable.Columns[10].Visible = false;
                            previewTable.Columns[11].Visible = false;
                            previewTable.Columns[12].Visible = false;
                        }
                    }
                    else
                    {
                        previewTable.Columns[7].HeaderText = "";
                        previewTable.Columns[8].HeaderText = "";
                        previewTable.Columns[9].HeaderText = "";
                        previewTable.Columns[10].HeaderText = "";
                        previewTable.Columns[11].HeaderText = "";
                        previewTable.Columns[12].HeaderText = "";

                        previewTable.Columns[7].Visible = false;
                        previewTable.Columns[8].Visible = false;
                        previewTable.Columns[9].Visible = false;
                        previewTable.Columns[10].Visible = false;
                        previewTable.Columns[11].Visible = false;
                        previewTable.Columns[12].Visible = false;
                    }
                }
                else
                {
                    previewTable.Columns[5].HeaderText = "";
                    previewTable.Columns[6].HeaderText = "";
                    previewTable.Columns[7].HeaderText = "";
                    previewTable.Columns[8].HeaderText = "";
                    previewTable.Columns[9].HeaderText = "";
                    previewTable.Columns[10].HeaderText = "";
                    previewTable.Columns[11].HeaderText = "";
                    previewTable.Columns[12].HeaderText = "";

                    previewTable.Columns[5].Visible = false;
                    previewTable.Columns[6].Visible = false;
                    previewTable.Columns[7].Visible = false;
                    previewTable.Columns[8].Visible = false;
                    previewTable.Columns[9].Visible = false;
                    previewTable.Columns[10].Visible = false;
                    previewTable.Columns[11].Visible = false;
                    previewTable.Columns[12].Visible = false;
                }
            }
            else
            {
                previewTable.Columns[3].HeaderText = "";
                previewTable.Columns[4].HeaderText = "";
                previewTable.Columns[5].HeaderText = "";
                previewTable.Columns[6].HeaderText = "";
                previewTable.Columns[7].HeaderText = "";
                previewTable.Columns[8].HeaderText = "";
                previewTable.Columns[9].HeaderText = "";
                previewTable.Columns[10].HeaderText = "";
                previewTable.Columns[11].HeaderText = "";
                previewTable.Columns[12].HeaderText = "";

                previewTable.Columns[3].Visible = false;
                previewTable.Columns[4].Visible = false;
                previewTable.Columns[5].Visible = false;
                previewTable.Columns[6].Visible = false;
                previewTable.Columns[7].Visible = false;
                previewTable.Columns[8].Visible = false;
                previewTable.Columns[9].Visible = false;
                previewTable.Columns[10].Visible = false;
                previewTable.Columns[11].Visible = false;
                previewTable.Columns[12].Visible = false;
            }
        }

        private void setCheckedState(PictureBox label, bool flag)
        {
            if (flag)
            {
                label.Image = checkedState;
            }
            else
            {
                label.Image = uncheckedState;
            }

            presetList.Refresh();
        }

        private void presetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            processPresetCheckBoxCheckedEvent = false;

            if (presetList.SelectedIndex == -1)
            {
                backupedPreset = null;

                descriptionBox.Text = "";

                userPresetPictureBox.Image = uncheckedState; ;
                customizedPresetPictureBox.Image = uncheckedState;

                preserveValuesTextBox.Enabled = false;
                label2.Enabled = false;

                parameterTagList.Enabled = false;
                labelTag.Enabled = false;

                parameterTag2List.Enabled = false;
                labelTag2.Enabled = false;

                parameterTag3List.Enabled = false;
                labelTag3.Enabled = false;

                parameterTag4List.Enabled = false;
                labelTag4.Enabled = false;

                parameterTag5List.Enabled = false;
                labelTag5.Enabled = false;

                parameterTag6List.Enabled = false;
                labelTag6.Enabled = false;

                customTextBox.Enabled = false;
                customTextLabel.Enabled = false;

                customText2Box.Enabled = false;
                customText2Label.Enabled = false;

                customText3Box.Enabled = false;
                customText3Label.Enabled = false;

                customText4Box.Enabled = false;
                customText4Label.Enabled = false;

                conditionCheckBox.Checked = false;
                conditionCheckBox.Enabled = false;

                assignHotkeyCheckBox.Checked = false;
                assignHotkeyCheckBox.Enabled = false;

                applyToPlayingTrackCheckBox.Checked = false;
                applyToPlayingTrackCheckBox.Enabled = false;

                idTextBox.Enabled = false;
                clearIdButton.Enabled = false;

                buttonExport.Enabled = false;
                buttonCopy.Enabled = false;
                buttonEdit.Enabled = false;
                buttonDelete.Enabled = false;
                buttonPreview.Enabled = false;
                buttonOK.Enabled = false;
            }
            else
            {
                presetsWorkingCopy.TryGetValue(((Preset)presetList.SelectedItem).guid, out preset);
                backupedPreset = new Preset(preset);

                if (!preset.userPreset && !Plugin.DeveloperMode)
                    editButtonEnabled = false;
                else
                    editButtonEnabled = true;


                descriptionBox.Text = GetDictValue(preset.descriptions, Plugin.Language);

                setCheckedState(userPresetPictureBox, preset.userPreset);
                setCheckedState(customizedPresetPictureBox, preset.customizedByUser);

                preserveValuesTextBox.Enabled = true;
                preserveValuesTextBox.Text = preset.preserveValues;
                label2.Enabled = true;

                fillParameterTagList(preset.parameterTagType, parameterTagList, labelTag);
                parameterTagList.Text = getTagName(preset.parameterTagId);
                fillParameterTagList(preset.parameterTag2Type, parameterTag2List, labelTag2);
                parameterTag2List.Text = getTagName(preset.parameterTag2Id);
                fillParameterTagList(preset.parameterTag3Type, parameterTag3List, labelTag3);
                parameterTag3List.Text = getTagName(preset.parameterTag3Id);
                fillParameterTagList(preset.parameterTag4Type, parameterTag4List, labelTag4);
                parameterTag4List.Text = getTagName(preset.parameterTag4Id);
                fillParameterTagList(preset.parameterTag5Type, parameterTag5List, labelTag5);
                parameterTag5List.Text = getTagName(preset.parameterTag5Id);
                fillParameterTagList(preset.parameterTag6Type, parameterTag6List, labelTag6);
                parameterTag6List.Text = getTagName(preset.parameterTag6Id);

                customTextBox.Text = preset.customTextChecked ? preset.customText : "";
                customTextBox.Enabled = preset.customTextChecked;
                customTextLabel.Enabled = preset.customTextChecked;
                customText2Box.Text = preset.customText2Checked ? preset.customText2 : "";
                customText2Box.Enabled = preset.customText2Checked;
                customText2Label.Enabled = preset.customText2Checked;
                customText3Box.Text = preset.customText3Checked ? preset.customText3 : "";
                customText3Box.Enabled = preset.customText3Checked;
                customText3Label.Enabled = preset.customText3Checked;
                customText4Box.Text = preset.customText4Checked ? preset.customText4 : "";
                customText4Box.Enabled = preset.customText4Checked;
                customText4Label.Enabled = preset.customText4Checked;

                bool hotkeyAssigned = preset.hotkeyAssigned;
                if (asrPresetsWithHotkeysCount >= Plugin.MaximumNumberOfASRHotkeys && !hotkeyAssigned)
                {
                    assignHotkeyCheckBox.Enabled = false;
                    assignHotkeyCheckBox.Checked = false;
                }
                else
                {
                    assignHotkeyCheckBox.Enabled = true;
                    assignHotkeyCheckBox.Checked = hotkeyAssigned;
                }

                applyToPlayingTrackCheckBox.Enabled = assignHotkeyCheckBox.Checked;
                applyToPlayingTrackCheckBox.Checked = preset.applyToPlayingTrack;


                idTextBox.Enabled = true;
                clearIdButton.Enabled = true;

                buttonExport.Enabled = editButtonEnabled || preset.customizedByUser;
                buttonCopy.Enabled = true;
                buttonEdit.Enabled = editButtonEnabled;
                buttonDelete.Enabled = true;
                buttonPreview.Enabled = true;
                buttonOK.Enabled = true;

                idTextBox.Text = preset.id;


                if (playlistComboBox.Items.Count == 0)
                {
                    conditionCheckBox.Enabled = false;
                    conditionCheckBox.Checked = false;
                }
                else
                {
                    conditionCheckBox.Enabled = true;

                    if (preset.condition)
                    {
                        bool playlistFound = false;
                        foreach (Playlist tempPreset in playlistComboBox.Items)
                        {
                            if (tempPreset.playlist == preset.playlist)
                            {
                                playlistComboBox.SelectedItem = tempPreset;
                                conditionCheckBox.Checked = true;
                                playlistFound = true;
                                break;
                            }
                        }

                        if (!playlistFound)
                        {
                            conditionCheckBox.Checked = false;
                            playlistComboBox.SelectedIndex = -1;
                        }
                    }
                    else
                    {
                        conditionCheckBox.Checked = false;
                    }
                }


                //Lets deal with preview table
                nameColumns();
            }

            processPresetCheckBoxCheckedEvent = true;
        }

        private void exportPreset(Preset preset, string presetPathName)
        {
            Preset savedPreset = new Preset(preset);
            savedPreset.applyToPlayingTrack = false;
            savedPreset.condition = false;
            savedPreset.playlist = null;
            savedPreset.preserveValues = "";
            savedPreset.id = "";

            savedPreset.savePreset(presetPathName);
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            string presetSafeFileName = preset.getSafeFileName();

            SaveFileDialog dialog = new SaveFileDialog
            {
                //Title = "Save ASR preset",
                Filter = Plugin.ASRPresetNaming + "|*" + Plugin.ASRPresetExtension,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                FileName = presetSafeFileName
            };

            if (dialog.ShowDialog(this) == DialogResult.Cancel) return;

            exportPreset(preset, dialog.FileName);
        }

        private void buttonExportCustom_Click(object sender, EventArgs e)
        {
            bool developerExport = false;
            if (Plugin.DeveloperMode && ModifierKeys == Keys.Control)
            {
                developerExport = true;
            }

            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                //Description = "Save user ASR presets",
                SelectedPath = Plugin.SavedSettings.defaultAsrPresetsExportFolder,
            };

            if (dialog.ShowDialog(this) == DialogResult.Cancel) return;
            Plugin.SavedSettings.defaultAsrPresetsExportFolder = dialog.SelectedPath;

            SortedDictionary<string, int> countedPresetFileNames = new SortedDictionary<string, int>();
            foreach (var currentPresetKVPair in presetsWorkingCopy)
            {
                if (currentPresetKVPair.Value.userPreset ^ developerExport)
                {
                    string presetFilename;

                    if (developerExport)
                    {
                        presetFilename = "口" + currentPresetKVPair.Value.guid.ToString();
                    }
                    else
                    {
                        presetFilename = getCountedPresetFilename(countedPresetFileNames, currentPresetKVPair.Value.getSafeFileName());
                    }

                    string presetFilePath = @"\\?\" + Path.Combine(dialog.SelectedPath, presetFilename + Plugin.ASRPresetExtension);
                    exportPreset(currentPresetKVPair.Value, presetFilePath);
                }
            }

            if (developerExport && Plugin.MSR != null)
            {
                string presetFilePath = @"\\?\" + Path.Combine(dialog.SelectedPath, "口" + Plugin.MSR.guid.ToString() + Plugin.ASRPresetExtension);
                exportPreset(Plugin.MSR, presetFilePath);
            }
        }

        public void import()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                //Title = "Load ASR preset",
                Filter = Plugin.ASRPresetNaming + "|*" + Plugin.ASRPresetExtension,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Multiselect = true
            };

            if (dialog.ShowDialog(this) == DialogResult.Cancel) return;


            Guid selectedPresetGuid = Guid.Empty;
            if (presetList.SelectedItem != null)
                selectedPresetGuid = ((Preset)presetList.SelectedItem).guid;


            bool askToImportExistingAsCopy = true;
            bool importExistingAsCopy = false;

            int numberOfImportedPresets = 0;
            int numberOfImportedAsCopyPresets = 0;
            int numberOfErrors = 0;

            System.Xml.Serialization.XmlSerializer presetSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Preset));
            System.Xml.Serialization.XmlSerializer presetSerializerOld = new System.Xml.Serialization.XmlSerializer(typeof(SavedPreset));//***

            foreach (string filename in dialog.FileNames)
            {
                try
                {
                    Preset newPreset = Preset.Load(filename, presetSerializer, presetSerializerOld);

                    if (!presetsWorkingCopy.TryGetValue(newPreset.guid, out Preset currentPreset))
                        currentPreset = null;

                    if (currentPreset != null)
                    {
                        if (askToImportExistingAsCopy)
                        {
                            DialogResult result = MessageBox.Show(this, Plugin.MsgDoYouWantToImportExistingPresetsAsCopies, 
                                null, MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                            askToImportExistingAsCopy = false;
                            if (result == DialogResult.Yes)
                            {
                                importExistingAsCopy = true;
                            }
                            else //if (result == DialogResult.No)
                            {
                                importExistingAsCopy = false;
                            }
                        }


                        if (importExistingAsCopy)
                        {
                            Preset newPresetCopy = new Preset(newPreset, true, false,  null, "*");

                            presetsWorkingCopy.Add(newPresetCopy.guid, newPresetCopy);
                            numberOfImportedAsCopyPresets++;
                        }
                        else
                        {
                            newPreset.copyExtendedCustomizationsFrom(currentPreset);

                            presetsWorkingCopy.Remove(newPreset.guid);
                            presetsWorkingCopy.Add(newPreset.guid, newPreset);
                            numberOfImportedPresets++;
                        }
                    }
                    else //if (currentPreset == null)
                    {
                        presetsWorkingCopy.Add(newPreset.guid, newPreset);
                        numberOfImportedPresets++;
                    }
                }
                catch
                {
                    numberOfErrors++;
                }
            }

            refreshPresetList(selectedPresetGuid);

            string message = "";
            //if (numberOfImportedPresets > 0)
                message += AddLeadingSpaces(numberOfImportedPresets, 4) + GetPluralForm(Plugin.MsgPresetsWereImported, numberOfImportedPresets);
            if (numberOfImportedAsCopyPresets > 0)
                message += AddLeadingSpaces(numberOfImportedAsCopyPresets, 4) + GetPluralForm(Plugin.MsgPresetsWereImportedAsCopies, numberOfImportedAsCopyPresets);
            if (numberOfErrors > 0)
                message += AddLeadingSpaces(numberOfErrors, 4) + GetPluralForm(Plugin.MsgPresetsFailedToImport, numberOfErrors);

            if (numberOfImportedPresets + numberOfImportedAsCopyPresets > 0)
            {
                presetsChanged = true;
                buttonClose.Image = Resources.UnsavedChanges_14;
                toolTip1.SetToolTip(buttonClose, buttonCloseToolTip);
            }

            MessageBox.Show(this, message, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static string GetPluralForm(string sentence, int number)
        {
            int form;
            int remainder = number % 10;

            if (number == 0) //Here may be special form like "No files" instead of "0 files"
                form = 5;
            else if (number >= 5 && number <= 20)
                form = 5;
            else if (remainder == 0)
                form = 5;
            else if (remainder == 1)
                form = 1;
            else if (remainder >= 2 && remainder <= 4)
                form = 2;
            else
                form = 5;

            switch (form)
            {
                case 1:
                    sentence = Regex.Replace(sentence, @"\{(.*?);(.*?);(.*?)\}", "$1");
                    break;
                case 2:
                    sentence = Regex.Replace(sentence, @"\{(.*?);(.*?);(.*?)\}", "$2");
                    break;
                case 5:
                    sentence = Regex.Replace(sentence, @"\{(.*?);(.*?);(.*?)\}", "$3");
                    break;
            }

            return sentence;
        }

        public static string AddLeadingSpaces(int number, int spacesCount, int zerosCount = 1)
        {
            string leadingZerosNumber = number.ToString("D" + spacesCount);
            string leadingSpaces = "";
            int maxZerosIndex;
            for (maxZerosIndex = 0; maxZerosIndex < spacesCount - zerosCount; maxZerosIndex++)
            {
                if (leadingZerosNumber[maxZerosIndex] == '0')
                    leadingSpaces += '\u2007';
                else
                    break;
            }

            return leadingSpaces + leadingZerosNumber.Substring(maxZerosIndex);
        }

        public void refreshPresetList(Guid selectedPresetGuid)
        {
            searchTextBox.Text = "";
            playlistChecked = false;
            functionIdChecked = false;
            hotkeyChecked = false;
            tickedOnlyChecked = false;

            presetList.Items.Clear();
            presetList.Sorted = false;
            ignoreCheckedPresetEvent = false;
            autoAppliedPresetCount = 0;
            int i = 0;
            foreach (Preset tempPreset in presetsWorkingCopy.Values)
            {
                presetList.Items.Add(tempPreset);
                presetList.SetItemChecked(i, autoAppliedAsrPresetGuids.Contains(tempPreset.guid));
                i++;
            }
            presetList.SelectedIndex = -1;
            presetList_SelectedIndexChanged(null, null);
            presetListItemCheckChanged();
            ignoreCheckedPresetEvent = true;
            presetList.Sorted = true;


            if (!presetsWorkingCopy.TryGetValue(selectedPresetGuid, out _))
                presetList.SelectedIndex = -1;
            else
                presetList.SelectedItem = presetsWorkingCopy[selectedPresetGuid];

        }

        public void install(bool installAll)
        {
            string[] newPresetNames;
            int numberOfInstalledPresets = 0;
            int numberOfReinstalledPresets = 0;
            int numberOfUpdatedPresets = 0;
            int numberOfUpdatedCustomizedPresets = 0;
            int numberOfReinstalledCustomizedPresets = 0;
            int numberOfNotChangedSkippedPresets = 0;
            int numberOfRemovedPresets = 0;
            int numberOfErrors = 0;


            Guid selectedPresetGuid = Guid.Empty;
            if (presetList.SelectedItem != null)
                selectedPresetGuid = ((Preset)presetList.SelectedItem).guid;


            if (MessageBox.Show(this, Plugin.MsgInstallingConfirmation, "", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) 
                == DialogResult.No)
                return;


            if (!Directory.Exists(PresetsPath))
                Directory.CreateDirectory(PresetsPath);

            if (Directory.Exists(Plugin.PluginsPath))
            {
                newPresetNames = Directory.GetFiles(Path.Combine(Plugin.PluginsPath, Plugin.AsrPresetsDirectory), "*");
                System.Xml.Serialization.XmlSerializer presetSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Preset));
                System.Xml.Serialization.XmlSerializer presetSerializerOld = new System.Xml.Serialization.XmlSerializer(typeof(SavedPreset));//***

                bool askToResetCustomizedByUser = true;
                bool resetCustomizedByUser = true;
                bool askToRemovePresets = true;
                bool removePresets = true;
                foreach (string newPresetName in newPresetNames)
                {
                    try
                    {
                        Preset newPreset = Preset.Load(newPresetName, presetSerializer, presetSerializerOld);

                        if (newPreset.guid.ToString() != "ff8d53d9-526b-4b40-bbf0-848b6b892f70")
                        {
                            if (!presetsWorkingCopy.TryGetValue(newPreset.guid, out Preset currentPreset))
                                currentPreset = null;

                            if (currentPreset != null)
                            {
                                bool anyCustomization = currentPreset.customizedByUser;
                                if (currentPreset.condition || currentPreset.applyToPlayingTrack)
                                    anyCustomization = true;

                                if (installAll)
                                {
                                    if (askToResetCustomizedByUser && anyCustomization)
                                    {
                                        if (MessageBox.Show(this, Plugin.MsgDoYouWantToResetYourCustomizedPredefinedPresets, "", MessageBoxButtons.YesNo, 
                                            MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                                        {
                                            askToResetCustomizedByUser = false;
                                            resetCustomizedByUser = false;
                                        }
                                        else
                                        {
                                            askToResetCustomizedByUser = false;
                                        }
                                    }

                                    if (askToRemovePresets && newPreset.removePreset)
                                    {
                                        if (MessageBox.Show(this, Plugin.MsgDoYouWantToRemovePredefinedPresets, "", MessageBoxButtons.YesNo, 
                                            MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                                        {
                                            askToResetCustomizedByUser = false;
                                            removePresets = false;
                                        }
                                        else
                                        {
                                            askToResetCustomizedByUser = false;
                                        }
                                    }


                                    if (newPreset.removePreset && removePresets)
                                    {
                                        deletePreset(currentPreset);
                                        numberOfRemovedPresets++;
                                    }
                                    else if (!anyCustomization || resetCustomizedByUser)
                                    {
                                        newPreset.copyBasicCustomizationsFrom(currentPreset);

                                        presetsWorkingCopy.Remove(newPreset.guid);
                                        presetsWorkingCopy.Add(newPreset.guid, newPreset);
                                        if (newPreset.modifiedUtc > currentPreset.modifiedUtc) //Updating
                                            numberOfUpdatedPresets++;
                                        else
                                            numberOfReinstalledPresets++;
                                    }
                                    else //Update preset to latest version, and copy *all* customizations
                                    {
                                        newPreset.copyAdvancedCustomizationsFrom(currentPreset);

                                        presetsWorkingCopy.Remove(newPreset.guid);
                                        presetsWorkingCopy.Add(newPreset.guid, newPreset);
                                        if (newPreset.modifiedUtc > currentPreset.modifiedUtc) //Updating
                                            numberOfUpdatedCustomizedPresets++;
                                        else
                                            numberOfReinstalledCustomizedPresets++;
                                    }
                                }
                                else if (newPreset.removePreset && newPreset.modifiedUtc > Plugin.SavedSettings.lastAsrImportDateUtc)
                                {
                                    if (askToRemovePresets && newPreset.removePreset)
                                    {
                                        if (MessageBox.Show(this, Plugin.MsgDoYouWantToResetYourCustomizedPredefinedPresets, "", MessageBoxButtons.YesNo, 
                                            MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                                        {
                                            askToResetCustomizedByUser = false;
                                            removePresets = false;
                                        }
                                        else
                                        {
                                            askToResetCustomizedByUser = false;
                                        }
                                    }

                                    if (removePresets)
                                    {
                                        deletePreset(currentPreset);
                                        numberOfRemovedPresets++;
                                    }
                                }
                                else if (newPreset.modifiedUtc > currentPreset.modifiedUtc) //Install only new presets
                                {
                                    if (!anyCustomization)
                                    {
                                        newPreset.copyExtendedCustomizationsFrom(currentPreset);

                                        presetsWorkingCopy.Remove(newPreset.guid);
                                        presetsWorkingCopy.Add(newPreset.guid, newPreset);
                                        numberOfUpdatedPresets++;
                                    }
                                    else //Update preset to latest version, and copy *all* customizations
                                    {
                                        newPreset.copyAdvancedCustomizationsFrom(currentPreset);

                                        presetsWorkingCopy.Remove(newPreset.guid);
                                        presetsWorkingCopy.Add(newPreset.guid, newPreset);
                                        numberOfUpdatedCustomizedPresets++;
                                    }
                                }
                                else //if (!importAll && (newPreset.modifiedUtc <= currentPreset.modifiedUtc || newPreset.modifiedUtc <= Plugin.SavedSettings.lastAsrImportDateUtc))
                                {
                                    numberOfNotChangedSkippedPresets++;
                                }
                            }
                            else if (!newPreset.removePreset)
                            {
                                if (installAll || newPreset.modifiedUtc > Plugin.SavedSettings.lastAsrImportDateUtc)
                                {
                                    presetsWorkingCopy.Add(newPreset.guid, newPreset);
                                    numberOfInstalledPresets++;
                                }
                                else
                                {
                                    numberOfNotChangedSkippedPresets++;
                                }
                            }
                        }
                        else
                        {
                            Plugin.MSR = newPreset;
                        }
                    }
                    catch
                    {
                        numberOfErrors++;
                    }
                }
            }

            if (numberOfInstalledPresets > 0)
                Plugin.SavedSettings.lastAsrImportDateUtc = DateTime.UtcNow;


            refreshPresetList(selectedPresetGuid);


            string message = "";
            if (numberOfInstalledPresets > 0)
                message += AddLeadingSpaces(numberOfInstalledPresets, 4) + GetPluralForm(Plugin.MsgPresetsWereInstalled, numberOfInstalledPresets);
            if (numberOfReinstalledCustomizedPresets > 0)
                message += AddLeadingSpaces(numberOfReinstalledCustomizedPresets, 4) + GetPluralForm(Plugin.MsgPresetsCustomizedWereReinstalled, numberOfReinstalledCustomizedPresets);
            if (numberOfReinstalledPresets > 0)
                message += AddLeadingSpaces(numberOfReinstalledPresets, 4) + GetPluralForm(Plugin.MsgPresetsWereReinstalled, numberOfReinstalledPresets);
            if (numberOfUpdatedPresets > 0)
                message += AddLeadingSpaces(numberOfUpdatedPresets, 4) + GetPluralForm(Plugin.MsgPresetsWereUpdated, numberOfUpdatedPresets);
            if (numberOfUpdatedCustomizedPresets > 0)
                message += AddLeadingSpaces(numberOfUpdatedCustomizedPresets, 4) + GetPluralForm(Plugin.MsgPresetsCustomizedWereUpdated, numberOfUpdatedCustomizedPresets);
            if (numberOfNotChangedSkippedPresets > 0)
                message += AddLeadingSpaces(numberOfNotChangedSkippedPresets, 4) + GetPluralForm(Plugin.MsgPresetsWereNotChanged, numberOfNotChangedSkippedPresets);
            if (numberOfRemovedPresets > 0)
                message += AddLeadingSpaces(numberOfRemovedPresets, 4) + GetPluralForm(Plugin.MsgPresetsRemoved, numberOfRemovedPresets);
            if (numberOfErrors > 0)
                message += AddLeadingSpaces(numberOfErrors, 4) + GetPluralForm(Plugin.MsgPresetsFailedToUpdate, numberOfErrors);

            if (message == "")
            {
                message = Plugin.MsgPresetsNotFound;
            }
            else
            {
                presetsChanged = true;
                buttonClose.Image = Resources.UnsavedChanges_14;
                toolTip1.SetToolTip(buttonClose, buttonCloseToolTip);
            }


            MessageBox.Show(this, message, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void deleteAll()
        {
            int numberOfDeletedPresets = 0;
            int numberOfErrors = 0;

            DialogResult result = MessageBox.Show(this, Plugin.MsgDeletingConfirmation, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No)
                return;


            List<Preset> predefinedPresets = new List<Preset>();
            foreach (var presetKeyValuePair in presetsWorkingCopy)
            {
                if (!presetKeyValuePair.Value.userPreset)
                {
                    predefinedPresets.Add(presetKeyValuePair.Value);
                }
            }

            foreach (var tempPreset in predefinedPresets)
            {
                deletePreset(tempPreset);
                numberOfDeletedPresets++;
            }

            searchTextBox.Text = "";
            playlistChecked = false;
            functionIdChecked = false;
            hotkeyChecked = false;
            tickedOnlyChecked = false;

            presetList.Items.Clear();
            presetList.Sorted = false;
            ignoreCheckedPresetEvent = false;
            autoAppliedPresetCount = 0;
            int i = 0;
            foreach (Preset tempPreset in presetsWorkingCopy.Values)
            {
                presetList.Items.Add(tempPreset);
                presetList.SetItemChecked(i, autoAppliedAsrPresetGuids.Contains(tempPreset.guid));
                i++;
            }
            presetList.SelectedIndex = -1;
            presetList_SelectedIndexChanged(null, null);
            presetListItemCheckChanged();
            ignoreCheckedPresetEvent = true;
            presetList.Sorted = true;


            string message = "";

            if (numberOfDeletedPresets == 0 && numberOfErrors == 0)
                message = Plugin.MsgNoPresetsDeleted;
            else
                message += numberOfDeletedPresets + GetPluralForm(Plugin.MsgPresetsWereDeleted, numberOfDeletedPresets);

            if (numberOfErrors > 0)
                message += "\n" + numberOfErrors + GetPluralForm(Plugin.MsgFailedToDelete, numberOfErrors);


            MessageBox.Show(this, message, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonImportNew_Click(object sender, EventArgs e)
        {
            install(false);
        }

        private void buttonImportAll_Click(object sender, EventArgs e)
        {
            install(true);
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            import();
        }

        private void buttonDeleteAll_Click(object sender, EventArgs e)
        {
            deleteAll();
        }

        private void parameterTag_SelectedIndexChanged(object sender, EventArgs e)
        {
            preset.parameterTagId = getTagId(parameterTagList.Text);
            nameColumns();
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void parameterTag2_SelectedIndexChanged(object sender, EventArgs e)
        {
            preset.parameterTag2Id = getTagId(parameterTag2List.Text);
            nameColumns();
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void parameterTag3_SelectedIndexChanged(object sender, EventArgs e)
        {
            preset.parameterTag3Id = getTagId(parameterTag3List.Text);
            nameColumns();
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void parameterTag4List_SelectedIndexChanged(object sender, EventArgs e)
        {
            preset.parameterTag4Id = getTagId(parameterTag4List.Text);
            nameColumns();
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void parameterTag5List_SelectedIndexChanged(object sender, EventArgs e)
        {
            preset.parameterTag5Id = getTagId(parameterTag5List.Text);
            nameColumns();
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void parameterTag6List_SelectedIndexChanged(object sender, EventArgs e)
        {
            preset.parameterTag6Id = getTagId(parameterTag6List.Text);
            nameColumns();
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void customText_TextChanged(object sender, EventArgs e)
        {
            preset.customText = customTextBox.Text;
            nameColumns();
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void customText2Box_TextChanged(object sender, EventArgs e)
        {
            preset.customText2 = customText2Box.Text;
            nameColumns();
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void customText3Box_TextChanged(object sender, EventArgs e)
        {
            preset.customText3 = customText3Box.Text;
            nameColumns();
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void customText4Box_TextChanged(object sender, EventArgs e)
        {
            preset.customText4 = customText4Box.Text;
            nameColumns();
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void preserveValuesTextBox_TextChanged(object sender, EventArgs e)
        {
            preset.preserveValues = preserveValuesTextBox.Text;
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void conditionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (conditionCheckBox.Checked && playlistComboBox.SelectedIndex == -1 && playlistComboBox.Items.Count > 0)
            {
                playlistComboBox.SelectedIndex = 0;
            }
            else if (conditionCheckBox.Checked && playlistComboBox.Items.Count == 0)
            {
                conditionCheckBox.Checked = false;
            }

            playlistComboBox.Enabled = conditionCheckBox.Checked;
            preset.condition = playlistComboBox.Enabled;
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void playlistComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (playlistComboBox.SelectedIndex >= 0)
                preset.playlist = ((Playlist)(playlistComboBox.SelectedItem)).playlist;
            else
                preset.playlist = "";

            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void cbHeader_OnCheckBoxClicked(bool state)
        {
            foreach (DataGridViewRow row in previewTable.Rows)
            {
                if ((string)row.Cells[0].Value == "")
                    continue;

                if (state)
                    row.Cells[0].Value = "False";
                else
                    row.Cells[0].Value = "True";

                DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(0, row.Index);
                previewTable_CellContentClick(null, e);
            }
        }

        private void previewTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string sourceTag1Value;
                string sourceTag2Value;
                string sourceTag3Value;
                string sourceTag4Value;
                string sourceTag5Value;
                string newTag1Value;
                string newTag2Value;
                string newTag3Value;
                string newTag4Value;
                string newTag5Value;

                string isChecked = (string)previewTable.Rows[e.RowIndex].Cells[0].Value;

                if (isChecked == "True")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "False";

                    sourceTag1Value = (string)previewTable.Rows[e.RowIndex].Cells[3].Value;
                    sourceTag2Value = (string)previewTable.Rows[e.RowIndex].Cells[5].Value;
                    sourceTag3Value = (string)previewTable.Rows[e.RowIndex].Cells[7].Value;
                    sourceTag4Value = (string)previewTable.Rows[e.RowIndex].Cells[9].Value;
                    sourceTag5Value = (string)previewTable.Rows[e.RowIndex].Cells[11].Value;

                    previewTable.Rows[e.RowIndex].Cells[4].Value = sourceTag1Value;
                    previewTable.Rows[e.RowIndex].Cells[6].Value = sourceTag2Value;
                    previewTable.Rows[e.RowIndex].Cells[8].Value = sourceTag3Value;
                    previewTable.Rows[e.RowIndex].Cells[10].Value = sourceTag4Value;
                    previewTable.Rows[e.RowIndex].Cells[12].Value = sourceTag5Value;
                }
                else if (isChecked == "False")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "True";

                    lock (Presets)
                    {
                        GetReplacedTags((string)previewTable.Rows[e.RowIndex].Cells[1].Value, preset, this);

                        newTag1Value = SearchedAndReplacedTags.replacedTagValue;
                        newTag2Value = SearchedAndReplacedTags.replacedTag2Value;
                        newTag3Value = SearchedAndReplacedTags.replacedTag3Value;
                        newTag4Value = SearchedAndReplacedTags.replacedTag4Value;
                        newTag5Value = SearchedAndReplacedTags.replacedTag5Value;
                    }

                    previewTable.Rows[e.RowIndex].Cells[4].Value = newTag1Value;
                    previewTable.Rows[e.RowIndex].Cells[6].Value = newTag2Value;
                    previewTable.Rows[e.RowIndex].Cells[8].Value = newTag3Value;
                    previewTable.Rows[e.RowIndex].Cells[10].Value = newTag4Value;
                    previewTable.Rows[e.RowIndex].Cells[12].Value = newTag5Value;
                }
            }
        }

        private void previewTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                if ((string)previewTable.Rows[e.RowIndex].Cells[3].Value == (string)previewTable.Rows[e.RowIndex].Cells[4].Value)
                    previewTable.Rows[e.RowIndex].Cells[4].Style = Plugin.UnchangedStyle;
                else
                    previewTable.Rows[e.RowIndex].Cells[4].Style = Plugin.ChangedStyle;
            }
            else if (e.ColumnIndex == 6)
            {
                int i;
                for (i = 5; i >=3; i -= 2)
                {
                    if (previewTable.Columns[i].Visible)
                        break;
                }
                if ((string)previewTable.Rows[e.RowIndex].Cells[i].Value == (string)previewTable.Rows[e.RowIndex].Cells[6].Value)
                    previewTable.Rows[e.RowIndex].Cells[6].Style = Plugin.UnchangedStyle;
                else
                    previewTable.Rows[e.RowIndex].Cells[6].Style = Plugin.ChangedStyle;
            }
            else if (e.ColumnIndex == 8)
            {
                int i;
                for (i = 7; i >= 3; i -= 2)
                {
                    if (previewTable.Columns[i].Visible)
                        break;
                }
                if ((string)previewTable.Rows[e.RowIndex].Cells[i].Value == (string)previewTable.Rows[e.RowIndex].Cells[8].Value)
                    previewTable.Rows[e.RowIndex].Cells[8].Style = Plugin.UnchangedStyle;
                else
                    previewTable.Rows[e.RowIndex].Cells[8].Style = Plugin.ChangedStyle;
            }
            else if (e.ColumnIndex == 10)
            {
                int i;
                for (i = 9; i >= 3; i -= 2)
                {
                    if (previewTable.Columns[i].Visible)
                        break;
                }
                if ((string)previewTable.Rows[e.RowIndex].Cells[i].Value == (string)previewTable.Rows[e.RowIndex].Cells[10].Value)
                    previewTable.Rows[e.RowIndex].Cells[10].Style = Plugin.UnchangedStyle;
                else
                    previewTable.Rows[e.RowIndex].Cells[10].Style = Plugin.ChangedStyle;
            }
            else if (e.ColumnIndex == 12)
            {
                int i;
                for (i = 11; i >= 3; i -= 2)
                {
                    if (previewTable.Columns[i].Visible)
                        break;
                }
                if ((string)previewTable.Rows[e.RowIndex].Cells[i].Value == (string)previewTable.Rows[e.RowIndex].Cells[12].Value)
                    previewTable.Rows[e.RowIndex].Cells[12].Style = Plugin.UnchangedStyle;
                else
                    previewTable.Rows[e.RowIndex].Cells[12].Style = Plugin.ChangedStyle;
            }
        }

        public override void enableDisablePreviewOptionControls(bool enable)
        {
            if (enable && previewIsGenerated)
                return;

            presetList.Enabled = enable;

            if (enable)
            {
                presetList_SelectedIndexChanged(null, null);
            }
            else
            {
                parameterTagList.Enabled = enable;
                parameterTag2List.Enabled = enable;
                parameterTag3List.Enabled = enable;
                parameterTag4List.Enabled = enable;
                parameterTag5List.Enabled = enable;
                parameterTag6List.Enabled = enable;

                customTextBox.Enabled = enable;
                customText2Box.Enabled = enable;
                customText3Box.Enabled = enable;
                customText4Box.Enabled = enable;

                preserveValuesTextBox.Enabled = enable;
            }

            buttonExport.Enabled = enable && editButtonEnabled;
            buttonImportNew.Enabled = enable;
            buttonImportAll.Enabled = enable;
            buttonImport.Enabled = enable;
            buttonExportCustom.Enabled = enable;

            buttonCreate.Enabled = enable;
            buttonCopy.Enabled = enable;
            buttonEdit.Enabled = enable && editButtonEnabled;
            buttonDelete.Enabled = enable;
            buttonDeleteAll.Enabled = enable;
        }

        public override void enableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonPreview, " ");
            dirtyErrorProvider.SetError(buttonPreview, String.Empty);
        }

        public override void disableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonPreview, getBackgroundTasksWarning());
        }

        public override void enableQueryingOrUpdatingButtons()
        {
            buttonOK.Enabled = true;
            buttonPreview.Enabled = true;
        }

        public override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enabled = false;
            buttonPreview.Enabled = false;
        }

        private void filterPresetList()
        {
            string[] searchStrings = searchTextBox.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            presetList.Items.Clear();
            presetList.Sorted = false;
            ignoreCheckedPresetEvent = false;

            setCheckedPicturesStates();

            int i = 0;
            foreach (Preset tempPreset in presetsWorkingCopy.Values)
            {
                bool filteringCriteriaAreMeet = true;


                if (hotkeyChecked && !tempPreset.hotkeyAssigned)
                {
                    filteringCriteriaAreMeet = false;
                }
                else if (functionIdChecked && string.IsNullOrEmpty(tempPreset.id))
                {
                    filteringCriteriaAreMeet = false;
                }
                else if (playlistChecked && !tempPreset.condition)
                {
                    filteringCriteriaAreMeet = false;
                }
                else if (userChecked && !tempPreset.userPreset)
                {
                    filteringCriteriaAreMeet = false;
                }
                else if (customizedChecked && !tempPreset.customizedByUser)
                {
                    filteringCriteriaAreMeet = false;
                }
                else if (predefinedChecked && tempPreset.userPreset)
                {
                    filteringCriteriaAreMeet = false;
                }


                if (filteringCriteriaAreMeet)
                {
                    string presetName = tempPreset.ToString();

                    foreach (string searchString in searchStrings)
                    {
                        if (!Regex.IsMatch(presetName, Regex.Escape(searchString), RegexOptions.IgnoreCase))
                        {
                            filteringCriteriaAreMeet = false;
                            break;
                        }
                    }
                }


                if (filteringCriteriaAreMeet)
                {
                    bool autoApply = autoAppliedAsrPresetGuids.Contains(tempPreset.guid);
                    if (!tickedOnlyChecked || autoApply)
                    {
                        presetList.Items.Add(tempPreset);
                        presetList.SetItemChecked(i, autoApply);
                        if (autoApply)
                            autoAppliedPresetCount--;

                        i++;
                    }
                }
            }
            presetListItemCheckChanged();
            ignoreCheckedPresetEvent = true;
            presetList.Sorted = true;
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            filterPresetList();
        }

        private void clearSearchButton_Click(object sender, EventArgs e)
        {
            searchTextBox.Text = "";
        }

        private void assignHotkeyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!processPresetCheckBoxCheckedEvent)
                return;


            if (assignHotkeyCheckBox.Checked)
            {
                int slot = FindFirstSlot(asrPresetsWithHotkeysGuids, Guid.Empty);
                asrPresetsWithHotkeysGuids[slot] = ((Preset)presetList.SelectedItem).guid;
                ((Preset)presetList.SelectedItem).hotkeyAssigned = true;

                asrPresetsWithHotkeysCount++;

                applyToPlayingTrackCheckBox.Enabled = true;
            }
            else
            {
                int slot = FindFirstSlot(asrPresetsWithHotkeysGuids, ((Preset)presetList.SelectedItem).guid);
                asrPresetsWithHotkeysGuids[slot] = Guid.Empty;
                ((Preset)presetList.SelectedItem).hotkeyAssigned = false;

                asrPresetsWithHotkeysCount--;

                applyToPlayingTrackCheckBox.Enabled = false;
                applyToPlayingTrackCheckBox.Checked = false;
            }

            preset.setCustomizationsFlag(this, backupedPreset);

            assignHotkeyCheckBox.Text = assignHotkeyCheckBoxText + (Plugin.MaximumNumberOfASRHotkeys - asrPresetsWithHotkeysCount) + "/" + Plugin.MaximumNumberOfASRHotkeys;
        }

        private void presetListItemCheckChanged()
        {
            if (autoAppliedPresetCount == 0)
            {
                label5.Text = editApplyText + autoApplyText;

                toolTip1.SetToolTip(label5, editApplyText + "\n" + autoApplyText + "\n\n"
                    + nowTickedText.ToUpper().Replace("%TICKED-PRESETS%", autoAppliedPresetCount.ToString()));

                label5.ForeColor = UntickedColor;
                //tickedOnlyChecked = false;
                //tickedOnlyCheckBox.Enabled = false;
            }
            else
            {
                label5.Text = editApplyText + autoApplyText + "\n"
                    + nowTickedText.ToUpper().Replace("%TICKED-PRESETS%", autoAppliedPresetCount.ToString());

                toolTip1.SetToolTip(label5, editApplyText + "\n" + autoApplyText + "\n\n"
                    + clickHereText.ToUpper() + "\n" + nowTickedText.ToUpper().Replace("%TICKED-PRESETS%", autoAppliedPresetCount.ToString()));

                label5.ForeColor = TickedColor;
                //tickedOnlyCheckBox.Enabled = true;
            }
        }

        private void presetList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (ignoreCheckedPresetEvent)
            {
                if (e.NewValue == CheckState.Checked)
                    e.NewValue = CheckState.Unchecked;
                else if (e.NewValue == CheckState.Unchecked)
                    e.NewValue = CheckState.Checked;

                return;
            }


            if (e.NewValue == CheckState.Checked)
            {
                autoAppliedPresetCount++;
                if (presetList.SelectedIndex != -1)
                {
                    autoAppliedAsrPresetGuids.Add(((Preset)presetList.SelectedItem).guid);
                    presetsChanged = true;
                    if (!Plugin.SavedSettings.dontPlayTickedAsrPresetSound)
                        System.Media.SystemSounds.Exclamation.Play();
                }
            }
            else
            {
                autoAppliedPresetCount--;
                if (presetList.SelectedIndex != -1)
                {
                    autoAppliedAsrPresetGuids.Remove(((Preset)presetList.SelectedItem).guid);
                    presetsChanged = true;
                }
            }

            presetListItemCheckChanged();
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void idTextBox_Leave(object sender, EventArgs e)
        {
            if (idTextBox.Text == preset.id)
            {
                return;
            }
            else if (idTextBox.Text == "")
            {
                if (asrIdsPresetGuids.TryGetValue(preset.id, out _))
                    asrIdsPresetGuids.Remove(preset.id);

                preset.id = "";
                preset.setCustomizationsFlag(this, backupedPreset);

                return;
            }

            string allowedRemoved = Regex.Replace(idTextBox.Text, @"[0-9]", "");
            allowedRemoved = Regex.Replace(allowedRemoved, @"[a-z]", "");
            allowedRemoved = Regex.Replace(allowedRemoved, @"[A-Z]", "");
            allowedRemoved = Regex.Replace(allowedRemoved, @"\-", "");
            allowedRemoved = Regex.Replace(allowedRemoved, @"_", "");
            allowedRemoved = Regex.Replace(allowedRemoved, @"\:", "");

            if (allowedRemoved != "")
            {
                MessageBox.Show(this, Plugin.MsgNotAllowedSymbols, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                if (presetList.SelectedItem == preset)
                    idTextBox.Text = preset.id;

                idTextBox.Focus();

                return;
            }
            else
            {
                foreach (var idGuid in asrIdsPresetGuids)
                {
                    if (idTextBox.Text == idGuid.Key && preset.guid != idGuid.Value)
                    {
                        MessageBox.Show(this, Plugin.MsgPresetExists.Replace("%ID%", idTextBox.Text), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        if (presetList.SelectedItem == preset)
                            idTextBox.Text = preset.id;

                        idTextBox.Focus();

                        return;
                    }
                }


                if (asrIdsPresetGuids.TryGetValue(preset.id, out _))
                    asrIdsPresetGuids.Remove(preset.id);

                preset.id = idTextBox.Text;
                asrIdsPresetGuids.Add(preset.id, preset.guid);

                preset.setCustomizationsFlag(this, backupedPreset);

                return;
            }
        }

        private void clearIdButton_Click(object sender, EventArgs e)
        {
            idTextBox.Text = "";
            idTextBox_Leave(null, null);
        }

        private void applyToPlayingTrackCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ((Preset)presetList.SelectedItem).applyToPlayingTrack = applyToPlayingTrackCheckBox.Checked;
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void previewTable_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (preset == null)
                return;

            preset.columnWeights.Clear();
            for (int i = 2; i < previewTable.ColumnCount; i++)
            {
                preset.columnWeights.Add(previewTable.Columns[i].FillWeight);
            }

            presetsWorkingCopy.Remove(preset.guid);
            presetsWorkingCopy.Add(preset.guid, preset);
        }

        private void presetList_MouseClick(object sender, MouseEventArgs e)
        {
            if (presetList.SelectedIndex == -1)
                return;

            if (e.X <= 16)
            {
                ignoreCheckedPresetEvent = false;
                presetList.SetItemChecked(presetList.SelectedIndex, !presetList.GetItemChecked(presetList.SelectedIndex));
                ignoreCheckedPresetEvent = true;
            }
        }

        private void presetList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (presetList.SelectedIndex == -1)
                return;

            if (preset.userPreset)
            {
                editPreset(preset, false, false);
            }
            else
            {
                if (!Plugin.SavedSettings.dontShowPredefinedPresetsCantBeChangedMessage && MessageBox.Show(this, Plugin.MsgPredefinedPresetsCantBeChanged,
                    null, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Plugin.SavedSettings.dontShowPredefinedPresetsCantBeChangedMessage = true;
                };

                editPreset(preset, false, true);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            if (autoAppliedPresetCount > 0)
            {
                tickedOnlyChecked = true;

                filterPresetList();
            }
        }

        private void setCheckedPicturesStates()
        {
            int checkedCount = 0;
            int checkedFilter = 0;

            if (hotkeyChecked)
            {
                hotkeyPictureBox.Image = HotkeyPresetsAccent;
                checkedCount++;
                checkedFilter = 7;
            }
            else
            {
                hotkeyPictureBox.Image = HotkeyPresetsDimmed;
            }

            if (functionIdChecked)
            {
                functionIdPictureBox.Image = FunctionIdPresetsAccent;
                checkedCount++;
                checkedFilter = 6;
            }
            else
            {
                functionIdPictureBox.Image = FunctionIdPresetsDimmed;
            }

            if (playlistChecked)
            {
                playlistPictureBox.Image = PlaylistPresetsAccent;
                checkedCount++;
                checkedFilter = 5;
            }
            else
            {
                playlistPictureBox.Image = PlaylistPresetsDimmed;
            }

            if (userChecked)
            {
                userPictureBox.Image = UserPresetsAccent;
                checkedCount++;
                checkedFilter = 4;
            }
            else
            {
                userPictureBox.Image = UserPresetsDimmed;
            }

            if (customizedChecked)
            {
                customizedPictureBox.Image = CustomizedPresetsAccent;
                checkedCount++;
                checkedFilter = 3;
            }
            else
            {
                customizedPictureBox.Image = CustomizedPresetsDimmed;
            }

            if (predefinedChecked)
            {
                predefinedPictureBox.Image = PredefinedPresetsAccent;
                checkedCount++;
                checkedFilter = 2;
            }
            else
            {
                predefinedPictureBox.Image = PredefinedPresetsDimmed;
            }

            if (tickedOnlyChecked)
            {
                tickedOnlyPictureBox.Image = AutoAppliedPresetsAccent;
                checkedCount++;
                checkedFilter = 1;
            }
            else
            {
                tickedOnlyPictureBox.Image = AutoAppliedPresetsDimmed;
            }


            ignorefFlterComboBoxSelectedIndexChanged = true;

            if (checkedCount == 0)
            {
                untickAllChecked = false;
                filterComboBox.SelectedIndex = 0;
            }
            else
            {
                untickAllChecked = true;

                if (checkedCount > 1)
                    filterComboBox.SelectedIndex = -1;
                else
                    filterComboBox.SelectedIndex = checkedFilter;
            }

            ignorefFlterComboBoxSelectedIndexChanged = false;



            if (untickAllChecked)
                uncheckAllFiltersPictureBox.Image = UncheckAllFiltersAccent;
            else
                uncheckAllFiltersPictureBox.Image = UncheckAllFiltersDimmed;

        }

        private void AdvancedSearchAndReplaceCommand_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (presetsChanged)
            {
                if (MessageBox.Show(this, Plugin.MsgDoYouWantToCloseWindowAndLoseAllChanges, 
                    "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) 
                    == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void filterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ignorefFlterComboBoxSelectedIndexChanged)
                return;


            if (ModifierKeys == Keys.Control && filterComboBox.SelectedIndex > 0)
            {
                switch (filterComboBox.SelectedIndex)
                {
                    case 1:
                        tickedOnlyChecked = !tickedOnlyChecked;

                        break;
                    case 2:
                        predefinedChecked = !predefinedChecked;

                        break;
                    case 3:
                        customizedChecked = !customizedChecked;

                        break;
                    case 4:
                        userChecked = !userChecked;

                        break;
                    case 5:
                        playlistChecked = !playlistChecked;

                        break;
                    case 6:
                        functionIdChecked = !functionIdChecked;

                        break;
                    case 7:
                        hotkeyChecked = !hotkeyChecked;

                        break;
                }
            }
            else if (filterComboBox.SelectedIndex >= 0)
            {
                tickedOnlyChecked = false;
                predefinedChecked = false;
                customizedChecked = false;
                userChecked = false;
                playlistChecked = false;
                functionIdChecked = false;
                hotkeyChecked = false;

                switch (filterComboBox.SelectedIndex)
                {
                    case 1:
                        tickedOnlyChecked = true;

                        break;
                    case 2:
                        predefinedChecked = true;

                        break;
                    case 3:
                        customizedChecked = true;

                        break;
                    case 4:
                        userChecked = true;

                        break;
                    case 5:
                        playlistChecked = true;

                        break;
                    case 6:
                        functionIdChecked = true;

                        break;
                    case 7:
                        hotkeyChecked = true;

                        break;
                }
            }


            filterPresetList();
        }

        private void tickedOnlyPictureBox_Click(object sender, EventArgs e)
        {
            tickedOnlyChecked = !tickedOnlyChecked;

            if (tickedOnlyChecked && ModifierKeys != Keys.Control)
            {
                predefinedChecked = false;
                customizedChecked = false;
                userChecked = false;
                playlistChecked = false;
                functionIdChecked = false;
                hotkeyChecked = false;
            }

            filterPresetList();
        }

        private void predefinedPictureBox_Click(object sender, EventArgs e)
        {
            predefinedChecked = !predefinedChecked;

            if (predefinedChecked && ModifierKeys != Keys.Control)
            {
                tickedOnlyChecked = false;
                customizedChecked = false;
                userChecked = false;
                playlistChecked = false;
                functionIdChecked = false;
                hotkeyChecked = false;
            }
            else if (predefinedChecked && userChecked)
            {
                userChecked = false;
            }

            filterPresetList();
        }

        private void customizedPictureBox_Click(object sender, EventArgs e)
        {
            customizedChecked = !customizedChecked;

            if (customizedChecked && ModifierKeys != Keys.Control)
            {
                tickedOnlyChecked = false;
                predefinedChecked = false;
                userChecked = false;
                playlistChecked = false;
                functionIdChecked = false;
                hotkeyChecked = false;
            }
            else if (customizedChecked && userChecked)
            {
                userChecked = false;
            }

            filterPresetList();
        }

        private void userPictureBox_Click(object sender, EventArgs e)
        {
            userChecked = !userChecked;

            if (userChecked && ModifierKeys != Keys.Control)
            {
                tickedOnlyChecked = false;
                predefinedChecked = false;
                customizedChecked = false;
                playlistChecked = false;
                functionIdChecked = false;
                hotkeyChecked = false;
            }
            else if (customizedChecked && userChecked)
            {
                customizedChecked = false;
                predefinedChecked = false;
            }
            else if (predefinedChecked && userChecked)
            {
                predefinedChecked = false;
            }

            filterPresetList();
        }

        private void playlistPictureBox_Click(object sender, EventArgs e)
        {
            playlistChecked = !playlistChecked;

            if (playlistChecked && ModifierKeys != Keys.Control)
            {
                tickedOnlyChecked = false;
                predefinedChecked = false;
                customizedChecked = false;
                userChecked = false;
                functionIdChecked = false;
                hotkeyChecked = false;
            }

            filterPresetList();
        }

        private void functionIdPictureBox_Click(object sender, EventArgs e)
        {
            functionIdChecked = !functionIdChecked;

            if (functionIdChecked && ModifierKeys != Keys.Control)
            {
                tickedOnlyChecked = false;
                predefinedChecked = false;
                customizedChecked = false;
                userChecked = false;
                playlistChecked = false;
                hotkeyChecked = false;
            }

            filterPresetList();
        }

        private void hotkeyPictureBox_Click(object sender, EventArgs e)
        {
            hotkeyChecked = !hotkeyChecked;

            if (hotkeyChecked && ModifierKeys != Keys.Control)
            {
                tickedOnlyChecked = false;
                predefinedChecked = false;
                customizedChecked = false;
                userChecked = false;
                playlistChecked = false;
                functionIdChecked = false;
            }

            filterPresetList();
        }

        private void uncheckAllFiltersPictureBox_Click(object sender, EventArgs e)
        {
            untickAllChecked = false;

            hotkeyChecked = false;
            functionIdChecked = false;
            playlistChecked = false;
            userChecked = false;
            customizedChecked = false;
            predefinedChecked = false;
            tickedOnlyChecked = false;

            filterPresetList();
        }
    }

    public partial class Plugin
    {
        public void ASRPreset1EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.Apply(1);
        }

        public void ASRPreset2EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.Apply(2);
        }

        public void ASRPreset3EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.Apply(3);
        }

        public void ASRPreset4EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.Apply(4);
        }

        public void ASRPreset5EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.Apply(5);
        }

        public void ASRPreset6EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.Apply(6);
        }

        public void ASRPreset7EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.Apply(7);
        }

        public void ASRPreset8EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.Apply(8);
        }

        public void ASRPreset9EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.Apply(9);
        }

        public void ASRPreset10EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.Apply(10);
        }

        public void ASRPreset11EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.Apply(11);
        }

        public void ASRPreset12EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.Apply(12);
        }

        public void ASRPreset13EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.Apply(13);
        }

        public void ASRPreset14EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.Apply(14);
        }

        public void ASRPreset15EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.Apply(15);
        }

        public void ASRPreset16EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.Apply(16);
        }

        public void ASRPreset17EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.Apply(17);
        }

        public void ASRPreset18EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.Apply(18);
        }

        public void ASRPreset19EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.Apply(19);
        }

        public void ASRPreset20EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.Apply(20);
        }
    }
}
