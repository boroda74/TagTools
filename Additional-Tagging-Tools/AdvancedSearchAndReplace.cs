﻿using MusicBeePlugin.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;
using ExtensionMethods;//****


namespace MusicBeePlugin
{
    public partial class AdvancedSearchAndReplaceCommand : PluginWindowTemplate
    {
        private static string CustomText1;


        private const int tableColumnCount = 20; // IT'S COLUMN COUNT OF "previewTable" !!!

        private static bool IgnoreFilterComboBoxSelectedIndexChanged = false;

        private bool ignoreSplitterMovedEvent = true;

        private bool processPresetChanges = true;
        private bool ignoreCheckedPresetEvent = true;
        private int autoAppliedPresetCount;

        private delegate void AddRowToTable(object row, object changeTypes);
        private delegate void ProcessRowOfTable(int row);
        private AddRowToTable addRowToTable;
        private ProcessRowOfTable processRowOfTable;

        private static readonly Encoding Unicode = Encoding.UTF8;

        private string[] files = new string[0];
        private readonly List<string[]> tags = new List<string[]>();
        private Preset preset = null;
        private Preset backupedPreset = null;
        private bool processTagsMode = true;

        private SortedDictionary<string, Preset> presetProcessingCopies; // <Guid as string, Preset>: To deal with <All Tags> pseudo tag some presets will be duplicated
        private int allTagsReplaceIdsCount = 0;
        private bool[] allTagsPresetParameterTagMask;

        private Dictionary<TagIdCombination, bool> tagIdCombinations;

        private string[] fileTags;
        private string clipboardText;

        private static Dictionary<int, string> SetTags = new Dictionary<int, string>();
        private static string LastSetTagValue;

        public static string PresetsPath;
        public static SortedDictionary<Guid, Preset> Presets;
        private SortedDictionary<Guid, Preset> presetsWorkingCopy = new SortedDictionary<Guid, Preset>();

        private SortedDictionary<Guid, bool> autoAppliedAsrPresetGuids;
        private int asrPresetsWithHotkeysCount = 0;
        private Guid[] asrPresetsWithHotkeysGuids;
        private SortedDictionary<string, Guid> asrIdsPresetGuids;

        private string assignHotkeyCheckBoxText;


        private static string TagNameText;
        private static string OrigValueText;
        private static string NewValueText;

        private bool editButtonEnabled;

        private static string EditApplyText;
        private static string AutoApplyText;
        private static string ClickHereText;
        private static string NowTickedText;

        private static string AllTagsPresetsCantBeAutoappliedText;
        private static string AllTagsTagIsSelectedToolTip;

        private static string CellTooTip;

        private System.Threading.Timer allTagsWarningTimer = null;
        private int AllTagsPresetsCantBeAutoappliedСountdown = 0;
        private bool selectedPresetUsesAllTags = false;

        private bool showTickedOnlyChecked;
        private bool showPredefinedChecked;
        private bool showCustomizedChecked;
        private bool showUserChecked;
        private bool showPlaylistLimitedChecked;
        private bool showFunctionIdAssignedChecked;
        private bool showHotkeyAssignedChecked;

        private bool untickAllChecked;

        private float tableLayoutPanel3ColWidth0;
        private float tableLayoutPanel3ColWidth1;
        private float tableLayoutPanel3ColWidth2;

        private bool presetsChanged = false;
        private string buttonCloseToolTip;

        public AdvancedSearchAndReplaceCommand(Plugin tagToolsPluginParam) : base(tagToolsPluginParam)
        {
            InitializeComponent();
            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();


            //Setting control not standard properties
            var heightField = typeof(CheckedListBox).GetField(
                "scaledListItemBordersHeight",
                BindingFlags.NonPublic | BindingFlags.Instance
            );

            tableLayoutPanel3ColWidth0 = tableLayoutPanel3.ColumnStyles[0].Width;
            tableLayoutPanel3ColWidth1 = tableLayoutPanel3.ColumnStyles[1].Width;
            tableLayoutPanel3ColWidth2 = tableLayoutPanel3.ColumnStyles[2].Width;


            var addedHeight = 4; // Some appropriate value, greater than the field's default of 2

            heightField.SetValue(presetList, addedHeight); // Where "presetList" is your CheckedListBox


            //Setting themed images
            clearIdButton.Image = ButtonRemoveImage;
            clearSearchButton.Image = ButtonRemoveImage;

            tickedOnlyPictureBox.Image = AutoAppliedPresetsDimmed;
            predefinedPictureBox.Image = PredefinedPresetsDimmed;
            customizedPictureBox.Image = CustomizedPresetsDimmed;
            userPictureBox.Image = UserPresetsDimmed;
            playlistPictureBox.Image = PlaylistPresetsDimmed;
            functionIdPictureBox.Image = FunctionIdPresetsDimmed;
            hotkeyPictureBox.Image = HotkeyPresetsDimmed;
            uncheckAllFiltersPictureBox.Image = UncheckAllFiltersDimmed;

            buttonSettings.Image = Gear;

            //Setting themed colors
            descriptionBox.ForeColor = DimmedHighlight;
            descriptionBox.BackColor = BackColor;


            //Initialization
            presetsWorkingCopy = new SortedDictionary<Guid, Preset>();
            foreach (var tempPreset in Presets.Values)
            {
                Preset presetCopy = new Preset(tempPreset);
                presetsWorkingCopy.Add(presetCopy.guid, presetCopy);
            }

            autoAppliedAsrPresetGuids = new SortedDictionary<Guid, bool>();
            foreach (Guid guid in SavedSettings.autoAppliedAsrPresetGuids)
                autoAppliedAsrPresetGuids.Add(guid, false);

            asrPresetsWithHotkeysGuids = new Guid[MaximumNumberOfASRHotkeys];
            for (int j = 0; j < MaximumNumberOfASRHotkeys; j++)
            {
                if (SavedSettings.asrPresetsWithHotkeysGuids[j] != Guid.Empty)
                {
                    asrPresetsWithHotkeysGuids[j] = SavedSettings.asrPresetsWithHotkeysGuids[j];
                    presetsWorkingCopy[asrPresetsWithHotkeysGuids[j]].hotkeyAssigned = true;
                }
                else
                {
                    asrPresetsWithHotkeysGuids[j] = Guid.Empty;
                }
            }

            asrIdsPresetGuids = new SortedDictionary<string, Guid>();
            foreach (var pair in IdsAsrPresets)
            {
                asrIdsPresetGuids.Add(pair.Key, pair.Value.guid);
            }


            string entireText = autoApplyPresetslabel.Text;
            EditApplyText = Regex.Replace(entireText, @"^(.*?\.\s).*\n.*", "$1").TrimEnd('\n');
            AutoApplyText = Regex.Replace(entireText, @"^.*?\.\s(.*?\.\s).*\n.*", "$1").TrimEnd('\n');
            ClickHereText = Regex.Replace(entireText, @"^.*?\.\s.*?\.\s(.*?\.\s).*\n.*", "$1").TrimEnd('\n');
            NowTickedText = Regex.Replace(entireText, @"^.*?\.\s.*?\.\s.*?\.\s(.*)\n.*", "$1").TrimEnd('\n');
            AllTagsPresetsCantBeAutoappliedText = Regex.Replace(entireText, @"^.*?\.\s.*?\.\s.*?\.\s.*\n(.*)", "$1").TrimEnd('\n');

            processPresetChanges = false;

            assignHotkeyCheckBox.Enable(false);
            assignHotkeyCheckBox.Checked = false;


            CueProvider.SetCue(filterComboBox, CtlMixedFilters);

            presetList.Items.Clear();
            presetList.Sorted = false;
            ignoreCheckedPresetEvent = false;
            autoAppliedPresetCount = 0;
            foreach (Preset tempPreset in presetsWorkingCopy.Values)
                presetList.Items.Add(tempPreset, autoAppliedAsrPresetGuids.TryGetValue(tempPreset.guid, out _));

            showAutoapplyingWarningIfRequired();
            ignoreCheckedPresetEvent = true;
            presetList.Sorted = true;

            asrPresetsWithHotkeysCount = ASRPresetsWithHotkeysCount;
            assignHotkeyCheckBoxText = assignHotkeyCheckBoxLabel.Text;
            assignHotkeyCheckBoxLabel.Text = assignHotkeyCheckBoxText + (MaximumNumberOfASRHotkeys - asrPresetsWithHotkeysCount) + "/" + MaximumNumberOfASRHotkeys;

            previewTable.EnableHeadersVisualStyles = false;

            DatagridViewCheckBoxHeaderCell cbHeader = new DatagridViewCheckBoxHeaderCell();
            cbHeader.Style = HeaderCellStyle;//*******
            cbHeader.setState(true);
            cbHeader.OnCheckBoxClicked += new CheckBoxClickedHandler(cbHeader_OnCheckBoxClicked);

            DataGridViewCheckBoxColumn colCB = new DataGridViewCheckBoxColumn
            {
                HeaderCell = cbHeader,
                ThreeState = true,
                FalseValue = "F",
                TrueValue = "T",
                IndeterminateValue = "",
                Width = 25,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            };

            previewTable.Columns.Insert(0, colCB);
            previewTable.Columns[1].HeaderCell.Style = HeaderCellStyle;
            previewTable.Columns[2].HeaderCell.Style = HeaderCellStyle;
            previewTable.Columns[3].HeaderCell.Style = HeaderCellStyle;
            previewTable.Columns[4].HeaderCell.Style = HeaderCellStyle;
            previewTable.Columns[5].HeaderCell.Style = HeaderCellStyle;
            previewTable.Columns[6].HeaderCell.Style = HeaderCellStyle;
            previewTable.Columns[7].HeaderCell.Style = HeaderCellStyle;
            previewTable.Columns[8].HeaderCell.Style = HeaderCellStyle;
            previewTable.Columns[9].HeaderCell.Style = HeaderCellStyle;
            previewTable.Columns[10].HeaderCell.Style = HeaderCellStyle;
            previewTable.Columns[11].HeaderCell.Style = HeaderCellStyle;
            previewTable.Columns[12].HeaderCell.Style = HeaderCellStyle;
            previewTable.Columns[13].HeaderCell.Style = HeaderCellStyle;
            previewTable.Columns[14].HeaderCell.Style = HeaderCellStyle;
            previewTable.Columns[15].HeaderCell.Style = HeaderCellStyle;
            previewTable.Columns[16].HeaderCell.Style = HeaderCellStyle;
            previewTable.Columns[17].HeaderCell.Style = HeaderCellStyle;
            previewTable.Columns[18].HeaderCell.Style = HeaderCellStyle;
            previewTable.Columns[19].HeaderCell.Style = HeaderCellStyle;


            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;


            float tagNameFontSize = previewTable.DefaultCellStyle.Font.Size * 0.8f; //Maybe it's worth to adjust fine size !!!
            Font tagNameFont = new Font(previewTable.DefaultCellStyle.Font.FontFamily, tagNameFontSize, FontStyle.Bold);

            previewTable.Columns[4].HeaderCell.Style.Font = tagNameFont;
            previewTable.Columns[7].HeaderCell.Style.Font = tagNameFont;
            previewTable.Columns[10].HeaderCell.Style.Font = tagNameFont;
            previewTable.Columns[13].HeaderCell.Style.Font = tagNameFont;
            previewTable.Columns[16].HeaderCell.Style.Font = tagNameFont;

            previewTable.Columns[4].DefaultCellStyle.Font = tagNameFont;
            previewTable.Columns[7].DefaultCellStyle.Font = tagNameFont;
            previewTable.Columns[10].DefaultCellStyle.Font = tagNameFont;
            previewTable.Columns[13].DefaultCellStyle.Font = tagNameFont;
            previewTable.Columns[16].DefaultCellStyle.Font = tagNameFont;



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
            previewTable.Columns[13].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[14].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[15].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[16].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[17].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[18].HeaderCell.Style.WrapMode = DataGridViewTriState.True;

            //Last column is same/different filename alternating indicator
            previewTable.Columns[19].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            previewTable.Columns[19].HeaderCell.Style.WrapMode = DataGridViewTriState.False;
            previewTable.Columns[19].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            previewTable.Columns[19].Width = 25;
            previewTable.Columns[19].MinimumWidth = 25;

            TagNameText = previewTable.Columns[4].HeaderText;
            OrigValueText = previewTable.Columns[5].HeaderText;
            NewValueText = previewTable.Columns[6].HeaderText;

            MbApiInterface.Playlist_QueryPlaylists();
            string playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();

            while (playlist != null)
            {
                //if (mbApiInterface.Playlist_GetType(playlist) == Plugin.PlaylistFormat.Auto)
                {
                    Playlist newPlaylist = new Playlist(playlist);
                    playlistComboBox.Items.Add(newPlaylist);
                }

                playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();
            }

            if (playlistComboBox.Items.Count > 0)
                playlistComboBox.SelectedIndex = 0;


            processPresetChanges = true;

            presetList_SelectedIndexChanged(null, null);

            buttonClose.Image = Resources.transparent_15;
            buttonCloseToolTip = toolTip1.GetToolTip(buttonClose);
            toolTip1.SetToolTip(buttonClose, "");

            AllTagsTagIsSelectedToolTip = toolTip1.GetToolTip(labelTag);
            toolTip1.SetToolTip(labelTag, "");
            labelTag.Image = Resources.transparent_15;
            labelTag2.Image = Resources.transparent_15;
            labelTag3.Image = Resources.transparent_15;
            labelTag4.Image = Resources.transparent_15;
            labelTag5.Image = Resources.transparent_15;
            labelTag6.Image = Resources.transparent_15;

            addRowToTable = previewTable_AddRowToTable;
            processRowOfTable = previewTable_ProcessRowOfTable;


            IgnoreFilterComboBoxSelectedIndexChanged = true;
            filterComboBox.SelectedIndex = 0;
            IgnoreFilterComboBoxSelectedIndexChanged = false;
        }

        public enum TagType
        {
            Undefined = -1,
            NotUsed = 0,
            Readonly = 1,
            Writable = 2,
            WritableAllowAllTags = 3
        }


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

        public class Preset
        {
            public DateTime modifiedUtc;

            public bool favorite;

            public bool userPreset;
            public bool customizedByUser;
            public bool removePreset;
            public Guid guid;
            public string id;
            public bool hotkeyAssigned;
            public bool applyToPlayingTrack;

            public SerializableDictionary<string, string> names;
            public SerializableDictionary<string, string> descriptions;

            public bool ignoreCase;
            public bool condition;
            public string playlist;

            public List<float> columnWeights;

            public bool isMSRPreset;

            public int parameterTagType; //0 - Not used, 1 - Writable, 2 - Readonly
            public int parameterTag2Type;
            public int parameterTag3Type;
            public int parameterTag4Type;
            public int parameterTag5Type;
            public int parameterTag6Type;

            public TagType parameterTagTypeNew = TagType.Undefined;
            public TagType parameterTag2TypeNew = TagType.Undefined;
            public TagType parameterTag3TypeNew = TagType.Undefined;
            public TagType parameterTag4TypeNew = TagType.Undefined;
            public TagType parameterTag5TypeNew = TagType.Undefined;
            public TagType parameterTag6TypeNew = TagType.Undefined;

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

            public int allTagsReplaceIdsCount;
            public bool processTagsMode; //true - use processTags attribute, false - use preserveTags attribute
            public string processTags;
            public string preserveTags;

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

            public bool? limitation1;
            public bool? limitation2;
            public bool? limitation3;
            public bool? limitation4;
            public bool? limitation5;

            public Preset()
            {
                modifiedUtc = DateTime.UtcNow;
                favorite = false;
                userPreset = false;
                customizedByUser = false;
                removePreset = false;
                guid = Guid.NewGuid();
                id = "";
                preserveValues = null;

                allTagsReplaceIdsCount = 0;
                processTagsMode = true;
                processTags = null;
                preserveTags = null;

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
                    favorite = originalPreset.favorite;
                    userPreset = originalPreset.userPreset;
                    customizedByUser = originalPreset.customizedByUser;
                    if (copyGuid)
                        guid = originalPreset.guid;
                    id = originalPreset.id;
                    hotkeyAssigned = originalPreset.hotkeyAssigned;
                    preserveValues = originalPreset.preserveValues;
                }
                else
                {
                    userPreset = true;
                }

                allTagsReplaceIdsCount = originalPreset.allTagsReplaceIdsCount;
                processTagsMode = originalPreset.processTagsMode;
                processTags = originalPreset.processTags;
                preserveTags = originalPreset.preserveTags;

                if (userPreset)
                    customizedByUser = false;

                removePreset = originalPreset.removePreset;

                foreach (var langName in originalPreset.names)
                {
                    string lang = langName.Key;
                    if (lang == null)
                        continue;

                    string name = langName.Value ?? "";

                    if (!string.IsNullOrEmpty(presetNamePrefix) && !name.StartsWith(presetNamePrefix))
                        name = presetNamePrefix + " " + name;
                    else if (!string.IsNullOrEmpty(presetNamePrefix))
                        name = presetNamePrefix + name;

                    if (!string.IsNullOrEmpty(presetNameSuffix) && !name.EndsWith(presetNameSuffix))
                        name += " " + presetNameSuffix;
                    else if (!string.IsNullOrEmpty(presetNameSuffix))
                        name += presetNameSuffix;

                    names.Add(lang, name);
                }

                foreach (var langDesc in originalPreset.descriptions)
                {
                    if (langDesc.Key == null)
                        continue;

                    descriptions.Add(langDesc.Key, langDesc.Value);
                }

                foreach (float width in originalPreset.columnWeights)
                {
                    columnWeights.Add(width);
                }

                condition = originalPreset.condition;
                playlist = originalPreset.playlist;

                isMSRPreset = originalPreset.isMSRPreset;

                ignoreCase = originalPreset.ignoreCase;

                parameterTagTypeNew = originalPreset.parameterTagTypeNew;
                parameterTag2TypeNew = originalPreset.parameterTag2TypeNew;
                parameterTag3TypeNew = originalPreset.parameterTag3TypeNew;
                parameterTag4TypeNew = originalPreset.parameterTag4TypeNew;
                parameterTag5TypeNew = originalPreset.parameterTag5TypeNew;
                parameterTag6TypeNew = originalPreset.parameterTag6TypeNew;

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

                limitation1 = originalPreset.limitation1;
                limitation2 = originalPreset.limitation2;
                limitation3 = originalPreset.limitation3;
                limitation4 = originalPreset.limitation4;
                limitation5 = originalPreset.limitation5;
            }

            public string getHotkeyChar()
            {
                if (!hotkeyAssigned)
                    return "";
                else if (!applyToPlayingTrack)
                    return "";
                else
                    return "";
            }

            public string getHotkeyPostfix()
            {
                string hotkeyChar = getHotkeyChar();

                if (hotkeyChar == "")
                    return hotkeyChar;
                else
                    return " " + hotkeyChar;
            }

            public string getHotkeyDescription()
            {
                return AsrHotkeyDescription + "⌕: " + getName() + getHotkeyPostfix();
            }

            public override string ToString()
            {
                return (favorite ? "♥ " : "") + GetDictValue(names, Language) + (getCustomizationsFlag() ? " " : "") + (userPreset ? " " : "")
                    + (condition ? " " : "") + (id != "" ? " " : "") + getHotkeyPostfix();
            }

            public string getName(bool getEnglishName = false)
            {
                if (getEnglishName)
                    return GetDictValue(names, "en");
                else
                    return GetDictValue(names, Language);
            }

            public string getSafeFileName()
            {
                string presetSafeFileName = getName().Replace('\\', '-').Replace('/', '-').Replace('<', '[').Replace('>', ']')
                    .Replace(" : ", " - ").Replace(": ", " - ").Replace(":", "-")
                    .Replace("\"", "\'\'")
                    .Replace('*', '#').Replace('?', '#').Replace('|', '#');

                if (presetSafeFileName.Length > 251 - ASRPresetExtension.Length)
                    presetSafeFileName = presetSafeFileName.Substring(0, 250 - ASRPresetExtension.Length) + "…";

                return presetSafeFileName;
            }

            public static Preset Load(string filename, System.Xml.Serialization.XmlSerializer presetSerializer)
            {
                FileStream stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None);
                StreamReader file = new StreamReader(stream, Unicode);

                Preset savedPreset = (Preset)presetSerializer.Deserialize(file);
                savedPreset.hotkeyAssigned = false;

                file.Close();

                if (savedPreset.parameterTagTypeNew == TagType.Undefined)
                    savedPreset.parameterTagTypeNew = GetTagTypeByPresetParameterTagType(savedPreset.parameterTagType);
                if (savedPreset.parameterTag2TypeNew == TagType.Undefined)
                    savedPreset.parameterTag2TypeNew = GetTagTypeByPresetParameterTagType(savedPreset.parameterTag2Type);
                if (savedPreset.parameterTag3TypeNew == TagType.Undefined)
                    savedPreset.parameterTag3TypeNew = GetTagTypeByPresetParameterTagType(savedPreset.parameterTag3Type);
                if (savedPreset.parameterTag4TypeNew == TagType.Undefined)
                    savedPreset.parameterTag4TypeNew = GetTagTypeByPresetParameterTagType(savedPreset.parameterTag4Type);
                if (savedPreset.parameterTag5TypeNew == TagType.Undefined)
                    savedPreset.parameterTag5TypeNew = GetTagTypeByPresetParameterTagType(savedPreset.parameterTag5Type);
                if (savedPreset.parameterTag6TypeNew == TagType.Undefined)
                    savedPreset.parameterTag6TypeNew = GetTagTypeByPresetParameterTagType(savedPreset.parameterTag6Type);

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
                else if (favorite != referencePreset.favorite)
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
                else if (allTagsReplaceIdsCount != referencePreset.allTagsReplaceIdsCount)
                    customized = true;
                else if (processTagsMode != referencePreset.processTagsMode)
                    customized = true;
                else if (processTags != referencePreset.processTags)
                    customized = true;
                else if (preserveTags != referencePreset.preserveTags)
                    customized = true;

                if (!userPreset)
                    customizedByUser |= customized;

                if (form != null)
                {
                    if (form.presetsChanged || customized || areFineCustomizationsMade(referencePreset))
                    {
                        form.setCheckedState(form.customizedPresetPictureBox, customized);
                        form.setPresetsChanged();
                    }
                }
            }

            public bool getCustomizationsFlag()
            {
                return customizedByUser || (!userPreset && !string.IsNullOrWhiteSpace(preserveValues));
            }

            public void copyBasicCustomizationsFrom(Preset referencePreset)
            {
                //userPreset = referencePreset.userPreset;
                favorite = referencePreset.favorite;
                hotkeyAssigned = referencePreset.hotkeyAssigned;
                id = referencePreset.id;
                processTagsMode = referencePreset.processTagsMode;
                processTags = referencePreset.processTags;
                preserveTags = referencePreset.preserveTags;
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
                    pathName = Path.Combine(@"\\?\" + PresetsPath, guid.ToString() + ASRPresetExtension);

                System.Xml.Serialization.XmlSerializer presetSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Preset));

                FileStream stream = System.IO.File.Open(pathName, FileMode.Create, FileAccess.Write, FileShare.None);
                StreamWriter file = new StreamWriter(stream, Unicode);

                presetSerializer.Serialize(file, this);

                file.Close();

                return pathName;
            }

            public string replaceVariable(string pattern, bool isSearchPattern) // isSearchPattern: true - search (i.e. must be escaped), false - replace
            {
                CustomText1 = customText;


                if (customTextChecked && isSearchPattern)
                    pattern = Regex.Replace(pattern, @"\\@1", Regex.Escape(customText));
                else if (customTextChecked) //Replaced pattern
                    pattern = Regex.Replace(pattern, @"\\@1", customText);

                if (customText2Checked && isSearchPattern)
                    pattern = Regex.Replace(pattern, @"\\@2", Regex.Escape(customText2));
                else if (customText2Checked) //Replaced pattern
                    pattern = Regex.Replace(pattern, @"\\@2", customText2);

                if (customText3Checked && isSearchPattern)
                    pattern = Regex.Replace(pattern, @"\\@3", Regex.Escape(customText3));
                else if (customText3Checked) //Replaced pattern
                    pattern = Regex.Replace(pattern, @"\\@3", customText3);

                if (customText4Checked && isSearchPattern)
                    pattern = Regex.Replace(pattern, @"\\@4", Regex.Escape(customText4));
                else if (customText4Checked) //Replaced pattern
                    pattern = Regex.Replace(pattern, @"\\@4", customText4);

                return pattern;
            }

            public bool detectAllTagsTagId(int tagId)
            {
                if (substituteTagId(tagId) == (int)AllTagsPseudoTagId)
                    return true;
                else
                    return false;
            }

            public (int, bool[]) getAllTagsReplaceTagIds() // Returns (<All Tags> replace parameter Id count, <All Tags> parameter Id mask)
            {
                bool[] allTagsParameterTagMask = new bool[5];
                allTagsParameterTagMask[0] = false;
                allTagsParameterTagMask[1] = false;
                allTagsParameterTagMask[2] = false;
                allTagsParameterTagMask[3] = false;
                allTagsParameterTagMask[4] = false;

                if (detectAllTagsTagId(replacedTagId))
                    allTagsParameterTagMask[0] = true;

                if (detectAllTagsTagId(replacedTag2Id))
                    allTagsParameterTagMask[1] = true;

                if (detectAllTagsTagId(replacedTag3Id))
                    allTagsParameterTagMask[2] = true;

                if (detectAllTagsTagId(replacedTag4Id))
                    allTagsParameterTagMask[3] = true;

                if (detectAllTagsTagId(replacedTag5Id))
                    allTagsParameterTagMask[4] = true;

                return (allTagsReplaceIdsCount, allTagsParameterTagMask);
            }

            public int countAllTagsReplaceTagIds() // Counts <All Tags> replace parameter Ids
            {
                allTagsReplaceIdsCount = 0;

                if (detectAllTagsTagId(replacedTagId))
                    allTagsReplaceIdsCount++;

                if (detectAllTagsTagId(replacedTag2Id))
                    allTagsReplaceIdsCount++;

                if (detectAllTagsTagId(replacedTag3Id))
                    allTagsReplaceIdsCount++;

                if (detectAllTagsTagId(replacedTag4Id))
                    allTagsReplaceIdsCount++;

                if (detectAllTagsTagId(replacedTag5Id))
                    allTagsReplaceIdsCount++;

                return allTagsReplaceIdsCount;
            }

            public int substituteTagId(int tagId)
            {
                switch ((ServiceMetaData)tagId)
                {
                    case ServiceMetaData.ParameterTagId1:
                        tagId = parameterTagId;
                        break;
                    case ServiceMetaData.ParameterTagId2:
                        tagId = parameterTag2Id;
                        break;
                    case ServiceMetaData.ParameterTagId3:
                        tagId = parameterTag3Id;
                        break;
                    case ServiceMetaData.ParameterTagId4:
                        tagId = parameterTag4Id;
                        break;
                    case ServiceMetaData.ParameterTagId5:
                        tagId = parameterTag5Id;
                        break;
                    case ServiceMetaData.ParameterTagId6:
                        tagId = parameterTag6Id;
                        break;
                }

                return tagId;
            }
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

            public bool replacedTagValuePreserved;
            public bool replacedTag2ValuePreserved;
            public bool replacedTag3ValuePreserved;
            public bool replacedTag4ValuePreserved;
            public bool replacedTag5ValuePreserved;

            public bool replacedTagPreserved;
            public bool replacedTag2Preserved;
            public bool replacedTag3Preserved;
            public bool replacedTag4Preserved;
            public bool replacedTag5Preserved;
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
                    parameter1 = SavedSettings.exceptionWordsASR;

                exceptionWords = parameter1.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                parameter0 = ChangeCaseCommand.ChangeWordsCase(parameter0, ChangeCaseCommand.ChangeCaseOptions.lowerCase, null, false,
                    null, SavedSettings.wordSplittersASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                string result = ChangeCaseCommand.ChangeWordsCase(parameter0, ChangeCaseCommand.ChangeCaseOptions.titleCase, exceptionWords, false,
                    SavedSettings.exceptionCharsASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), SavedSettings.wordSplittersASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), true, true);

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
                    parameter1 = SavedSettings.exceptionWordsASR;

                exceptionWords = parameter1.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                string result = ChangeCaseCommand.ChangeWordsCase(parameter0, ChangeCaseCommand.ChangeCaseOptions.lowerCase, exceptionWords, false,
                    SavedSettings.exceptionCharsASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), SavedSettings.wordSplittersASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

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
                    parameter1 = SavedSettings.exceptionWordsASR;

                exceptionWords = parameter1.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                string result = ChangeCaseCommand.ChangeWordsCase(parameter0, ChangeCaseCommand.ChangeCaseOptions.upperCase, exceptionWords, false,
                    SavedSettings.exceptionCharsASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), SavedSettings.wordSplittersASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

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
                    parameter1 = SavedSettings.exceptionWordsASR;

                exceptionWords = parameter1.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                parameter0 = ChangeCaseCommand.ChangeWordsCase(parameter0, ChangeCaseCommand.ChangeCaseOptions.lowerCase, null, false,
                    null, SavedSettings.wordSplittersASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                string result = ChangeCaseCommand.ChangeWordsCase(parameter0, ChangeCaseCommand.ChangeCaseOptions.sentenceCase, exceptionWords, false,
                    SavedSettings.exceptionCharsASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), SavedSettings.wordSplittersASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), true, false);

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
                parameter0 = parameter0.Replace("\"\"", "^^");
                parameter0 = parameter0.Replace("\"", "&&&");
                parameter0 = parameter0.Replace("^^", "\"");

                parameter0 = parameter0.Replace(",,", "~~");
                parameter0 = parameter0.Replace(",", "``");
                parameter0 = parameter0.Replace("~~", ",");


                parameter0 = MbApiInterface.MB_Evaluate(parameter0, currentFile);


                parameter0 = parameter0.Replace("``", ",");

                parameter0 = parameter0.Replace("&&&", "\"");

                return parameter0;
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
                string character = ((char)charcode).ToString();

                if (parameter1 == null)
                {
                    return character;
                }
                else
                {
                    string sequence = "";

                    int times = int.Parse(parameter1);
                    for (int i = 0; i < times; i++)
                        sequence += character;
                    
                    return sequence;
                }
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
                            parameter0 = Replace(currentFile, parameter0, pair[0], pair[1], false, out _);
                        }
                        else
                        {
                            pair[0] = pair[0].Substring(1);
                            pair[0] = pair[0].Replace(@"\\", @"\").Replace(@"\L", @"/").Replace(@"\V", @"|").Replace(@"\#", @"#").Replace(@"\X", @"*");
                            pair[1] = pair[1].Replace(@"\\", @"\").Replace(@"\L", @"/").Replace(@"\V", @"|").Replace(@"\#", @"#").Replace(@"\X", @"*").Replace(@"\T", @"$");
                            pair[0] = Regex.Escape(pair[0]);
                            parameter0 = Replace(currentFile, parameter0, pair[0], pair[1], false, out _);
                        }
                    }
                    else
                    {
                        if (pair[0].Length > 0 && pair[0][0] == '*')
                        {
                            pair[0] = pair[0].Substring(1);
                            pair[0] = pair[0].Replace(@"\\", @"\").Replace(@"\L", @"/").Replace(@"\V", @"|").Replace(@"\#", @"#").Replace(@"\X", @"*");
                            pair[1] = pair[1].Replace(@"\\", @"\").Replace(@"\L", @"/").Replace(@"\V", @"|").Replace(@"\#", @"#").Replace(@"\X", @"*").Replace(@"\T", @"$");
                            parameter0 = Replace(currentFile, parameter0, pair[0], pair[1], true, out _);
                        }
                        else
                        {
                            pair[0] = pair[0].Replace(@"\\", @"\").Replace(@"\L", @"/").Replace(@"\V", @"|").Replace(@"\#", @"#").Replace(@"\X", @"*");
                            pair[1] = pair[1].Replace(@"\\", @"\").Replace(@"\L", @"/").Replace(@"\V", @"|").Replace(@"\#", @"#").Replace(@"\X", @"*").Replace(@"\T", @"$");
                            pair[0] = Regex.Escape(pair[0]);
                            parameter0 = Replace(currentFile, parameter0, pair[0], pair[1], true, out _);
                        }
                    }
                }

                return parameter0;
            }
        }

        public static string GetDictValue(SortedDictionary<string, string> dict, string language)
        {
            if (language == null)
                return "";


            dict.TryGetValue(language, out string value);

            if (value == null)
                dict.TryGetValue("en", out value);

            return value;
        }

        public static void SetDictValue(SortedDictionary<string, string> dict, string language, string newValue)
        {
            if (language == null)
                return;


            dict.TryGetValue(language, out string value);

            if (value != null)
                dict.Remove(language);

            dict.Add(language, newValue);


            dict.TryGetValue("en", out value);

            if (value == null)
                dict.Add("en", newValue);
        }

        public static void InitASR()
        {
            if (SavedSettings.dontShowASR)
                return;

            lock (AsrAutoAppliedPresets)
            {
                Encoding Unicode = Encoding.UTF8;

                SetTags = new Dictionary<int, string>();

                PresetsPath = Path.Combine(@"\\?\" + MbApiInterface.Setting_GetPersistentStoragePath(), AsrPresetsDirectory);
                Presets = new SortedDictionary<Guid, Preset>();
                string[] presetNames;

                AsrAutoAppliedPresets.Clear();
                ASRPresetsWithHotkeysCount = 0;

                if (!Directory.Exists(PresetsPath))
                    Directory.CreateDirectory(PresetsPath);

                presetNames = Directory.GetFiles(PresetsPath, "*" + ASRPresetExtension);
                System.Xml.Serialization.XmlSerializer presetSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Preset));

                for (int i = presetNames.Length - 1; i >= 0; i--)
                {
                    string presetName = presetNames[i];
                        
                    try
                    {
                        Preset tempPreset = Preset.Load(presetName, presetSerializer);

                        if (tempPreset.guid.ToString() != "ff8d53d9-526b-4b40-bbf0-848b6b892f70")
                        {
                            if (Presets.TryGetValue(tempPreset.guid, out Preset existingPreset))
                            {
                                if (tempPreset.modifiedUtc >= existingPreset.modifiedUtc)
                                {
                                    Presets.Remove(tempPreset.guid);
                                    if (SavedSettings.autoAppliedAsrPresetGuids.Contains(tempPreset.guid))
                                        AsrAutoAppliedPresets.Remove(existingPreset);
                                    if (IdsAsrPresets.TryGetValue(tempPreset.id, out _))
                                        IdsAsrPresets.Remove(tempPreset.id);
                                }
                            }

                            Presets.Add(tempPreset.guid, tempPreset);

                            if (SavedSettings.autoAppliedAsrPresetGuids.Contains(tempPreset.guid))
                                AsrAutoAppliedPresets.Add(tempPreset);

                            if (SavedSettings.asrPresetsWithHotkeysGuids.Contains(tempPreset.guid))
                            {
                                int index;
                                ASRPresetsWithHotkeysCount++;
                                for (index = 0; index < SavedSettings.asrPresetsWithHotkeysGuids.Length; index++)
                                {
                                    if (SavedSettings.asrPresetsWithHotkeysGuids[index] == tempPreset.guid)
                                        break;
                                }
                                AsrPresetsWithHotkeys[index] = tempPreset;
                                tempPreset.hotkeyAssigned = true;
                            }
                            else
                            {
                                tempPreset.hotkeyAssigned = false;
                            }

                            if (!string.IsNullOrEmpty(tempPreset.id))
                            {
                                IdsAsrPresets.Add(tempPreset.id, tempPreset);
                            }
                        }
                        else
                        {
                            MSR = tempPreset;
                        }
                    }
                    catch { };
                }
            }
        }

        public static void RegisterASRPresetsHotkeysAndMenuItems(Plugin tagToolsPlugin)
        {
            ASRPresetsMenuItem?.DropDown.Items.Clear();

            ASRPresetsContextMenuItem?.DropDown.Items.Clear();

            if (SavedSettings.dontShowASR)
                return;

            for (int i = 0; i < AsrPresetsWithHotkeys.Length; i++)
            {
                Preset tempPreset = AsrPresetsWithHotkeys[i];

                if (tempPreset != null)
                {
                    switch (i)
                    {
                        case 0:
                            MbApiInterface.MB_RegisterCommand(tempPreset.getHotkeyDescription(), tagToolsPlugin.ASRPreset1EventHandler);
                            ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset1EventHandler);
                            ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset1EventHandler);
                            break;
                        case 1:
                            MbApiInterface.MB_RegisterCommand(tempPreset.getHotkeyDescription(), tagToolsPlugin.ASRPreset2EventHandler);
                            ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset2EventHandler);
                            ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset2EventHandler);
                            break;
                        case 2:
                            MbApiInterface.MB_RegisterCommand(tempPreset.getHotkeyDescription(), tagToolsPlugin.ASRPreset3EventHandler);
                            ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset3EventHandler);
                            ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset3EventHandler);
                            break;
                        case 3:
                            MbApiInterface.MB_RegisterCommand(tempPreset.getHotkeyDescription(), tagToolsPlugin.ASRPreset4EventHandler);
                            ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset4EventHandler);
                            ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset4EventHandler);
                            break;
                        case 4:
                            MbApiInterface.MB_RegisterCommand(tempPreset.getHotkeyDescription(), tagToolsPlugin.ASRPreset5EventHandler);
                            ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset5EventHandler);
                            ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset5EventHandler);
                            break;
                        case 5:
                            MbApiInterface.MB_RegisterCommand(tempPreset.getHotkeyDescription(), tagToolsPlugin.ASRPreset6EventHandler);
                            ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset6EventHandler);
                            ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset6EventHandler);
                            break;
                        case 6:
                            MbApiInterface.MB_RegisterCommand(tempPreset.getHotkeyDescription(), tagToolsPlugin.ASRPreset7EventHandler);
                            ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset7EventHandler);
                            ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset7EventHandler);
                            break;
                        case 7:
                            MbApiInterface.MB_RegisterCommand(tempPreset.getHotkeyDescription(), tagToolsPlugin.ASRPreset8EventHandler);
                            ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset8EventHandler);
                            ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset8EventHandler);
                            break;
                        case 8:
                            MbApiInterface.MB_RegisterCommand(tempPreset.getHotkeyDescription(), tagToolsPlugin.ASRPreset9EventHandler);
                            ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset9EventHandler);
                            ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset9EventHandler);
                            break;
                        case 9:
                            MbApiInterface.MB_RegisterCommand(tempPreset.getHotkeyDescription(), tagToolsPlugin.ASRPreset10EventHandler);
                            ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset10EventHandler);
                            ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset10EventHandler);
                            break;
                        case 10:
                            MbApiInterface.MB_RegisterCommand(tempPreset.getHotkeyDescription(), tagToolsPlugin.ASRPreset11EventHandler);
                            ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset11EventHandler);
                            ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset11EventHandler);
                            break;
                        case 11:
                            MbApiInterface.MB_RegisterCommand(tempPreset.getHotkeyDescription(), tagToolsPlugin.ASRPreset12EventHandler);
                            ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset12EventHandler);
                            ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset12EventHandler);
                            break;
                        case 12:
                            MbApiInterface.MB_RegisterCommand(tempPreset.getHotkeyDescription(), tagToolsPlugin.ASRPreset13EventHandler);
                            ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset13EventHandler);
                            ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset13EventHandler);
                            break;
                        case 13:
                            MbApiInterface.MB_RegisterCommand(tempPreset.getHotkeyDescription(), tagToolsPlugin.ASRPreset14EventHandler);
                            ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset14EventHandler);
                            ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset14EventHandler);
                            break;
                        case 14:
                            MbApiInterface.MB_RegisterCommand(tempPreset.getHotkeyDescription(), tagToolsPlugin.ASRPreset15EventHandler);
                            ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset15EventHandler);
                            ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset15EventHandler);
                            break;
                        case 15:
                            MbApiInterface.MB_RegisterCommand(tempPreset.getHotkeyDescription(), tagToolsPlugin.ASRPreset16EventHandler);
                            ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset16EventHandler);
                            ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset16EventHandler);
                            break;
                        case 16:
                            MbApiInterface.MB_RegisterCommand(tempPreset.getHotkeyDescription(), tagToolsPlugin.ASRPreset17EventHandler);
                            ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset17EventHandler);
                            ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset17EventHandler);
                            break;
                        case 17:
                            MbApiInterface.MB_RegisterCommand(tempPreset.getHotkeyDescription(), tagToolsPlugin.ASRPreset18EventHandler);
                            ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset18EventHandler);
                            ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset18EventHandler);
                            break;
                        case 18:
                            MbApiInterface.MB_RegisterCommand(tempPreset.getHotkeyDescription(), tagToolsPlugin.ASRPreset19EventHandler);
                            ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset19EventHandler);
                            ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset19EventHandler);
                            break;
                        case 19:
                            MbApiInterface.MB_RegisterCommand(tempPreset.getHotkeyDescription(), tagToolsPlugin.ASRPreset20EventHandler);
                            ASRPresetsMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset20EventHandler);
                            ASRPresetsContextMenuItem?.DropDown.Items.Add(tempPreset.getName(), null, tagToolsPlugin.ASRPreset20EventHandler);
                            break;
                        default:
                            break;
                    }
                }
            }
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

        protected static bool SetFileTag(string sourceFileUrl, MetaDataType tagId, string value, bool updateOnlyChangedTags, AdvancedSearchAndReplaceCommand plugin)
        {
            if (tagId == ClipboardTagId)
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

        private void previewTable_AddRowToTable(object row, object changeTypes)
        {
            previewTable.Rows.Add((string[])row);

            for (int i = 0; i < 5; i++)
                previewTable.Rows[previewTable.RowCount - 1].Cells[6 + i * 3].Tag = ((ChangesDetectionResult[])changeTypes)[i];

            previewTableFormatRow(previewTable.RowCount - 1);
        }

        private void previewTable_ProcessRowOfTable(int rowIndex)
        {
            previewTable.Rows[rowIndex].Cells[0].Value = null;
            previewTableFormatRow(rowIndex);
        }

        public static string Replace(string currentFile, string value, string searchedPattern, string replacedPattern, bool ignoreCase, out bool match)
        {
            match = false;
            if (searchedPattern == "")
                return value;


            try
            {
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

        public static string GetTag(string currentFile, AdvancedSearchAndReplaceCommand asrCommand, int tagId)
        {
            if (tagId == (int)ClipboardTagId)
            {
                if (asrCommand == null)
                    return "";

                if (asrCommand.fileTags == null)
                    return "";

                int position = -1;
                for (int i = 0; i < asrCommand.files.Length; i++)
                {
                    if (asrCommand.files[i] == currentFile)
                    {
                        position = i;
                        break;
                    }
                }

                return asrCommand.fileTags[position];
            }


            SetTags.TryGetValue(tagId, out string tempTag);

            if (tempTag == null)
            {
                if (tagId < PropMetaDataThreshold)
                    return GetFileTag(currentFile, (MetaDataType)tagId);
                else
                    return MbApiInterface.Library_GetFileProperty(currentFile, (FilePropertyType)(tagId - PropMetaDataThreshold));
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

        public static void SetReplacedTag(string currentFile, AdvancedSearchAndReplaceCommand asrCommand, 
            int searchedTagId, int replacedTagId, string searchedPattern, string replacedPattern, 
            string preserveValues, string preserveTags, string processTags, bool ignoreCase, bool append, bool? limitation, 
            out string searchedTagValue, out string replacedTagValue, out string originalReplacedTagValue, 
            out bool replacedTagValuePreserved, out bool replacedTagPreserved)
        {
            if (searchedPattern == "")
            {
                searchedTagValue = "";
                replacedTagValue = "";
                originalReplacedTagValue = "";
                replacedTagPreserved = false;
                replacedTagValuePreserved = false;
                return;
            }


            searchedTagValue = GetTag(currentFile, asrCommand, searchedTagId);
            originalReplacedTagValue = GetTag(currentFile, asrCommand, replacedTagId);


            if (searchedTagValue == "" && limitation == false) //Don't replace with empty values
            {
                replacedTagValue = originalReplacedTagValue;
                replacedTagPreserved = false;
                replacedTagValuePreserved = false;
                return;
            }


            if (originalReplacedTagValue != "" && limitation == true) //Don't replace not empty values
            {
                replacedTagValue = originalReplacedTagValue;
                replacedTagPreserved = false;
                replacedTagValuePreserved = false;
                return;
            }


            string replacedTagName = GetTagName((MetaDataType)replacedTagId);

            if (!string.IsNullOrWhiteSpace(processTags) && !(";;" + processTags + ";;").Contains(";;" + replacedTagName + ";;"))
            {
                replacedTagPreserved = true;
                replacedTagValuePreserved = false;
                replacedTagValue = originalReplacedTagValue;
                return;
            }

            if (!string.IsNullOrWhiteSpace(preserveTags) && (";;" + preserveTags + ";;").Contains(";;" + replacedTagName + ";;"))
            {
                replacedTagPreserved = true;
                replacedTagValuePreserved = false;
                replacedTagValue = originalReplacedTagValue;
                return;
            }
            replacedTagPreserved = false;

            if (!string.IsNullOrWhiteSpace(preserveValues) && (";;" + preserveValues + ";;").Contains(";;" + originalReplacedTagValue + ";;"))
            {
                replacedTagValuePreserved = true;
                replacedTagValue = originalReplacedTagValue;
                return;
            }
            replacedTagValuePreserved = false;


            replacedTagValue = Replace(currentFile, searchedTagValue, searchedPattern, replacedPattern, ignoreCase, out _);


            if (append)
                replacedTagValue = originalReplacedTagValue + replacedTagValue;

            SetTag(replacedTagId, replacedTagValue);
        }

        public static SearchedAndReplacedTagsStruct GetReplacedTags(string currentFile, ref Preset presetParam, AdvancedSearchAndReplaceCommand asrCommand = null, bool copyPresetParam = true)
        {
            if (copyPresetParam)
                presetParam = new Preset(presetParam);

            var searchedAndReplacedTags = new SearchedAndReplacedTagsStruct();
            SetTags.Clear();

            (int count, bool[] mask) = presetParam.getAllTagsReplaceTagIds();
            string processTags = null;
            string preserveTags = null;
            if (count > 0)
            {
                if (presetParam.processTagsMode)
                    processTags = presetParam.processTags;
                else
                    preserveTags = presetParam.preserveTags;
            }

            presetParam.searchedTagId = presetParam.substituteTagId(presetParam.searchedTagId);
            presetParam.replacedTagId = presetParam.substituteTagId(presetParam.replacedTagId);

            SetReplacedTag(currentFile, asrCommand, presetParam.searchedTagId, presetParam.replacedTagId,
                presetParam.replaceVariable(presetParam.searchedPattern, true), presetParam.replaceVariable(presetParam.replacedPattern, false), 
                presetParam.preserveValues, preserveTags, processTags, 
                presetParam.ignoreCase, presetParam.append, presetParam.limitation1, 
                out searchedAndReplacedTags.searchedTagValue, out searchedAndReplacedTags.replacedTagValue, out searchedAndReplacedTags.originalReplacedTagValue, 
                out searchedAndReplacedTags.replacedTagValuePreserved, out searchedAndReplacedTags.replacedTagPreserved);


            presetParam.searchedTag2Id = presetParam.substituteTagId(presetParam.searchedTag2Id);
            presetParam.replacedTag2Id = presetParam.substituteTagId(presetParam.replacedTag2Id);

            SetReplacedTag(currentFile, asrCommand, presetParam.searchedTag2Id, presetParam.replacedTag2Id,
                presetParam.replaceVariable(presetParam.searchedPattern2, true), presetParam.replaceVariable(presetParam.replacedPattern2, false),
                presetParam.preserveValues, preserveTags, processTags,
                presetParam.ignoreCase, presetParam.append2, presetParam.limitation2,
                out searchedAndReplacedTags.searchedTag2Value, out searchedAndReplacedTags.replacedTag2Value, out searchedAndReplacedTags.originalReplacedTag2Value,
                out searchedAndReplacedTags.replacedTag2ValuePreserved, out searchedAndReplacedTags.replacedTag2Preserved);


            presetParam.searchedTag3Id = presetParam.substituteTagId(presetParam.searchedTag3Id);
            presetParam.replacedTag3Id = presetParam.substituteTagId(presetParam.replacedTag3Id);

            SetReplacedTag(currentFile, asrCommand, presetParam.searchedTag3Id, presetParam.replacedTag3Id,
                presetParam.replaceVariable(presetParam.searchedPattern3, true), presetParam.replaceVariable(presetParam.replacedPattern3, false),
                presetParam.preserveValues, preserveTags, processTags,
                presetParam.ignoreCase, presetParam.append3, presetParam.limitation3, 
                out searchedAndReplacedTags.searchedTag3Value, out searchedAndReplacedTags.replacedTag3Value, out searchedAndReplacedTags.originalReplacedTag3Value,
                out searchedAndReplacedTags.replacedTag3ValuePreserved, out searchedAndReplacedTags.replacedTag3Preserved);


            presetParam.searchedTag4Id = presetParam.substituteTagId(presetParam.searchedTag4Id);
            presetParam.replacedTag4Id = presetParam.substituteTagId(presetParam.replacedTag4Id);

            SetReplacedTag(currentFile, asrCommand, presetParam.searchedTag4Id, presetParam.replacedTag4Id,
                presetParam.replaceVariable(presetParam.searchedPattern4, true), presetParam.replaceVariable(presetParam.replacedPattern4, false),
                presetParam.preserveValues, preserveTags, processTags,
                presetParam.ignoreCase, presetParam.append4, presetParam.limitation4, 
                out searchedAndReplacedTags.searchedTag4Value, out searchedAndReplacedTags.replacedTag4Value, out searchedAndReplacedTags.originalReplacedTag4Value,
                out searchedAndReplacedTags.replacedTag4ValuePreserved, out searchedAndReplacedTags.replacedTag4Preserved);


            presetParam.searchedTag5Id = presetParam.substituteTagId(presetParam.searchedTag5Id);
            presetParam.replacedTag5Id = presetParam.substituteTagId(presetParam.replacedTag5Id);

            SetReplacedTag(currentFile, asrCommand, presetParam.searchedTag5Id, presetParam.replacedTag5Id,
                presetParam.replaceVariable(presetParam.searchedPattern5, true), presetParam.replaceVariable(presetParam.replacedPattern5, false),
                presetParam.preserveValues, preserveTags, processTags,
                presetParam.ignoreCase, presetParam.append5, presetParam.limitation5, 
                out searchedAndReplacedTags.searchedTag5Value, out searchedAndReplacedTags.replacedTag5Value, out searchedAndReplacedTags.originalReplacedTag5Value, 
                out searchedAndReplacedTags.replacedTag5ValuePreserved, out searchedAndReplacedTags.replacedTag5Preserved);


            return searchedAndReplacedTags;
        }

        public static void SaveReplacedTags(string currentFile, Preset presetParam, SearchedAndReplacedTagsStruct searchedAndReplacedTags, AdvancedSearchAndReplaceCommand asrCommand = null)
        {
            if (searchedAndReplacedTags.replacedTagValue == "SYNTAX ERROR!" || searchedAndReplacedTags.replacedTag2Value == "SYNTAX ERROR!" ||
                searchedAndReplacedTags.replacedTag3Value == "SYNTAX ERROR!" || searchedAndReplacedTags.replacedTag4Value == "SYNTAX ERROR!" ||
                searchedAndReplacedTags.replacedTag5Value == "SYNTAX ERROR!")
                return;

            SetTags.Clear();

            if (presetParam.searchedPattern != "")
            {
                SetFileTag(currentFile, (MetaDataType)presetParam.substituteTagId(presetParam.replacedTagId), 
                    searchedAndReplacedTags.replacedTagValue, true, asrCommand);

                if (presetParam.searchedPattern2 != "")
                {
                    SetFileTag(currentFile, (MetaDataType)presetParam.substituteTagId(presetParam.replacedTag2Id), 
                        searchedAndReplacedTags.replacedTag2Value, true, asrCommand);

                    if (presetParam.searchedPattern3 != "")
                    {
                        SetFileTag(currentFile, (MetaDataType)presetParam.substituteTagId(presetParam.replacedTag3Id), 
                            searchedAndReplacedTags.replacedTag3Value, true, asrCommand);

                        if (presetParam.searchedPattern4 != "")
                        {
                            SetFileTag(currentFile, (MetaDataType)presetParam.substituteTagId(presetParam.replacedTag4Id), 
                                searchedAndReplacedTags.replacedTag4Value, true, asrCommand);

                            if (presetParam.searchedPattern5 != "")
                            {
                                SetFileTag(currentFile, (MetaDataType)presetParam.substituteTagId(presetParam.replacedTag5Id), 
                                    searchedAndReplacedTags.replacedTag5Value, true, asrCommand);
                            }
                        }
                    }
                }
            }


            CommitTagsToFile(currentFile, true, true);
        }

        public static void ReplaceTags(string currentFile, Preset presetParam)
        {
            lock (Presets)
            {
                var searchedAndReplacedTags = GetReplacedTags(currentFile, ref presetParam);
                SaveReplacedTags(currentFile, presetParam, searchedAndReplacedTags);
            }
        }

        public static void AsrAutoApplyPresets(object currentFileObj, object tagToolsPluginObj)
        {
            string currentFile = (string)currentFileObj;
            Plugin tagToolsPluginParam = (Plugin)tagToolsPluginObj;

            SortedDictionary<Guid, bool> appliedPresets = new SortedDictionary<Guid, bool>();

            lock (AsrAutoAppliedPresets)
            {
                if (AsrAutoAppliedPresets.Count > 0)
                {
                    foreach (Preset tempPreset in AsrAutoAppliedPresets)
                    {

                        if (tempPreset.allTagsReplaceIdsCount > 0)
                            SetStatusbarText(MsgAsrPresetsUsingAllTagsPseudoTagNameCannotBeAutoApplied
                                .Replace("%%PRESETNAME%%!", tempPreset.getName()).Replace("%%AllTagsPseudoTagName%%", AllTagsPseudoTagName), true);

                        bool conditionSatisfied = true;

                        if (tempPreset.condition)
                        {
                            conditionSatisfied = false;

                            MbApiInterface.Playlist_QueryPlaylists();
                            string playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();

                            while (playlist != null)
                            {
                                if (playlist == tempPreset.playlist)
                                {
                                    conditionSatisfied = MbApiInterface.Playlist_IsInList(playlist, currentFile);
                                    break;
                                }

                                playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();
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
                        SetStatusbarText(SbAsrPresetsAreApplied.Replace("%%PRESETCOUNT%%", appliedPresets.Count.ToString()), true);
                    else if (appliedPresets.Count == 1)
                        SetStatusbarText(SbAsrPresetIsApplied.Replace("%%PRESETNAME%%", Presets[appliedPresets.ElementAt(0).Key].getName()), true);

                    RefreshPanels(true);
                }
            }
        }

        public static string GetLastReplacedTag(string currentFile, Preset presetParam)
        {
            bool conditionSatisfied = true;

            if (presetParam.condition)
            {
                conditionSatisfied = false;

                MbApiInterface.Playlist_QueryPlaylists();
                string playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();

                while (playlist != null)
                {
                    if (playlist == presetParam.playlist)
                    {
                        conditionSatisfied = MbApiInterface.Playlist_IsInList(playlist, currentFile);
                        break;
                    }

                    playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();
                }
            }

            if (conditionSatisfied)
            {
                GetReplacedTags(currentFile, ref presetParam);
                return LastSetTagValue;
            }

            return "";
        }

        public static void ApplyPreset(int presetIndex)
        {
            Preset tempPreset = AsrPresetsWithHotkeys[presetIndex - 1];

            if (tempPreset == null)
                return;

            if (tempPreset.allTagsReplaceIdsCount > 0)
                SetStatusbarText(MsgAsrPresetsUsingAllTagsPseudoTagNameCannotBeAutoApplied
                    .Replace("%%PRESETNAME%%!", tempPreset.getName()).Replace("%%AllTagsPseudoTagName%%", AllTagsPseudoTagName), true);


            bool conditionSatisfied = true;

            if (!tempPreset.applyToPlayingTrack)
            {
                if (MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out string[] files))
                {
                    if (files.Length == 0)
                    {
                        MessageBox.Show(MbForm, MsgSelectTrack, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    for (int i = 0; i < files.Length; i++)
                    {
                        string currentFile = files[i];

                        if (tempPreset.condition)
                        {
                            conditionSatisfied = false;

                            MbApiInterface.Playlist_QueryPlaylists();
                            string playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();

                            while (playlist != null)
                            {
                                if (playlist == tempPreset.playlist)
                                {
                                    conditionSatisfied = MbApiInterface.Playlist_IsInList(playlist, currentFile);
                                    break;
                                }

                                playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();
                            }
                        }

                        if (conditionSatisfied)
                        {
                            ReplaceTags(currentFile, tempPreset);
                            SetStatusbarText(SbAsrPresetIsApplied.Replace("%%PRESETNAME%%", tempPreset.getName()), true);
                        }
                    }
                }
            }
            else
            {
                string currentFile = MbApiInterface.NowPlaying_GetFileUrl();

                if (!string.IsNullOrEmpty(currentFile))
                {
                    if (tempPreset.condition)
                    {
                        conditionSatisfied = false;

                        MbApiInterface.Playlist_QueryPlaylists();
                        string playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();

                        while (playlist != null)
                        {
                            if (playlist == tempPreset.playlist)
                            {
                                conditionSatisfied = MbApiInterface.Playlist_IsInList(playlist, currentFile);
                                break;
                            }

                            playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();
                        }
                    }

                    if (conditionSatisfied)
                    {
                        ReplaceTags(currentFile, tempPreset);
                        SetStatusbarText(SbAsrPresetIsApplied.Replace("%%PRESETNAME%%", tempPreset.getName()), true);
                    }
                }
            }

            RefreshPanels(true);
        }

        private bool prepareBackgroundPreview()
        {
            tags.Clear();
            previewTable.Rows.Clear();
            ((DatagridViewCheckBoxHeaderCell)previewTable.Columns[0].HeaderCell).setState(true);

            if (backgroundTaskIsWorking())
                return true;

            var allWritableTagNames = new List<string>();
            FillListByTagNames(allWritableTagNames, false, false, false);

            var allWritableTagIds = new List<int>();
            foreach (string tagName in allWritableTagNames)
                allWritableTagIds.Add((int)GetTagId(tagName));

            tagIdCombinations = generateParameterTagIdCombinationsForAllTagsPseudoTags(allWritableTagIds, preset);

            clipboardText = "";
            fileTags = null;



            files = null;
            if (!MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files))
                files = new string[0];



            if (preset.searchedTagId == (int)ClipboardTagId || preset.searchedTag2Id == (int)ClipboardTagId || preset.searchedTag3Id == (int)ClipboardTagId
                || preset.searchedTag4Id == (int)ClipboardTagId || preset.searchedTag5Id == (int)ClipboardTagId)
            {
                if (!Clipboard.ContainsText())
                {
                    MessageBox.Show(this, MsgClipboardDoesntContainText, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                fileTags = Clipboard.GetText().Replace("\r\n", "\r").Replace("\n", "\r").Split(new string[] { "\r" }, StringSplitOptions.None);

                if (fileTags.Length != files.Length)
                {
                    MessageBox.Show(this, MsgNumberOfTagsInClipboard + fileTags.Length 
                        + MsgDoesntCorrespondToNumberOfSelectedTracksC + files.Length + MsgMessageEndC, 
                        "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }


            if (files.Length == 0)
            {
                MessageBox.Show(this, MsgNoFilesSelected, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

            if (previewTable.RowCount == 0)
            {
                return prepareBackgroundPreview();
            }
            else
            {
                for (int fileCounter = 0; fileCounter < previewTable.RowCount; fileCounter++)
                    tags[fileCounter][0] = (string)previewTable.Rows[fileCounter].Cells[0].Value;

                return true;
            }
        }

        struct TagIdCombination
        {
            public int parameterTagId;
            public int parameterTag2Id;
            public int parameterTag3Id;
            public int parameterTag4Id;
            public int parameterTag5Id;
            public int parameterTag6Id;
        }

        private Dictionary<TagIdCombination, bool> generateParameterTagIdCombinationsForAllTagsPseudoTags(List<int> allWritableTagIds, Preset presetParam)
        {
            var tagIdCombinations = new Dictionary<TagIdCombination, bool>();

            if (presetParam.parameterTagId == (int)AllTagsPseudoTagId)
            {
                foreach (int tagId1 in allWritableTagIds)
                {
                    if (presetParam.parameterTag2Id == (int)AllTagsPseudoTagId)
                    {
                        foreach (int tagId2 in allWritableTagIds)
                        {
                            if (presetParam.parameterTag3Id == (int)AllTagsPseudoTagId)
                            {
                                foreach (int tagId3 in allWritableTagIds)
                                {
                                    if (presetParam.parameterTag4Id == (int)AllTagsPseudoTagId)
                                    {
                                        foreach (int tagId4 in allWritableTagIds)
                                        {
                                            if (presetParam.parameterTag5Id == (int)AllTagsPseudoTagId)
                                            {
                                                foreach (int tagId5 in allWritableTagIds)
                                                {
                                                    if (presetParam.parameterTag6Id == (int)AllTagsPseudoTagId)
                                                    {
                                                        foreach (int tagId6 in allWritableTagIds)
                                                        {
                                                            TagIdCombination tagCombination;
                                                            tagCombination.parameterTagId = tagId1;
                                                            tagCombination.parameterTag2Id = tagId2;
                                                            tagCombination.parameterTag3Id = tagId3;
                                                            tagCombination.parameterTag4Id = tagId4;
                                                            tagCombination.parameterTag5Id = tagId5;
                                                            tagCombination.parameterTag6Id = tagId6;

                                                            tagIdCombinations.Add(tagCombination, false);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        TagIdCombination tagCombination;
                                                        tagCombination.parameterTagId = tagId1;
                                                        tagCombination.parameterTag2Id = tagId2;
                                                        tagCombination.parameterTag3Id = tagId3;
                                                        tagCombination.parameterTag4Id = tagId4;
                                                        tagCombination.parameterTag5Id = tagId5;
                                                        tagCombination.parameterTag6Id = presetParam.parameterTag6Id;

                                                        tagIdCombinations.Add(tagCombination, false);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                TagIdCombination tagCombination;
                                                tagCombination.parameterTagId = tagId1;
                                                tagCombination.parameterTag2Id = tagId2;
                                                tagCombination.parameterTag3Id = tagId3;
                                                tagCombination.parameterTag4Id = tagId4;
                                                tagCombination.parameterTag5Id = presetParam.parameterTag5Id;
                                                tagCombination.parameterTag6Id = presetParam.parameterTag6Id;

                                                tagIdCombinations.Add(tagCombination, false);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        TagIdCombination tagCombination;
                                        tagCombination.parameterTagId = tagId1;
                                        tagCombination.parameterTag2Id = tagId2;
                                        tagCombination.parameterTag3Id = tagId3;
                                        tagCombination.parameterTag4Id = presetParam.parameterTag4Id;
                                        tagCombination.parameterTag5Id = presetParam.parameterTag5Id;
                                        tagCombination.parameterTag6Id = presetParam.parameterTag6Id;

                                        tagIdCombinations.Add(tagCombination, false);
                                    }
                                }
                            }
                            else
                            {
                                TagIdCombination tagCombination;
                                tagCombination.parameterTagId = tagId1;
                                tagCombination.parameterTag2Id = tagId2;
                                tagCombination.parameterTag3Id = presetParam.parameterTag3Id;
                                tagCombination.parameterTag4Id = presetParam.parameterTag4Id;
                                tagCombination.parameterTag5Id = presetParam.parameterTag5Id;
                                tagCombination.parameterTag6Id = presetParam.parameterTag6Id;

                                tagIdCombinations.Add(tagCombination, false);
                            }
                        }
                    }
                    else
                    {
                        TagIdCombination tagCombination;
                        tagCombination.parameterTagId = tagId1;
                        tagCombination.parameterTag2Id = presetParam.parameterTag2Id;
                        tagCombination.parameterTag3Id = presetParam.parameterTag3Id;
                        tagCombination.parameterTag4Id = presetParam.parameterTag4Id;
                        tagCombination.parameterTag5Id = presetParam.parameterTag5Id;
                        tagCombination.parameterTag6Id = presetParam.parameterTag6Id;

                        tagIdCombinations.Add(tagCombination, false);
                    }
                }
            }
            else
            {
                TagIdCombination tagCombination;
                tagCombination.parameterTagId = presetParam.parameterTagId;
                tagCombination.parameterTag2Id = presetParam.parameterTag2Id;
                tagCombination.parameterTag3Id = presetParam.parameterTag3Id;
                tagCombination.parameterTag4Id = presetParam.parameterTag4Id;
                tagCombination.parameterTag5Id = presetParam.parameterTag5Id;
                tagCombination.parameterTag6Id = presetParam.parameterTag6Id;

                tagIdCombinations.Add(tagCombination, false);
            }

            return tagIdCombinations;
        }

        private SortedDictionary<string, Preset> generatePresetsForAllTagsPseudoTags(Preset presetParam)
        {
            var allTagsPseudoTagPresets = new SortedDictionary<string, Preset>();


            foreach (var tagsCombination in tagIdCombinations.Keys)
            {
                var combinationPreset = new Preset(presetParam, true, false)
                {
                    parameterTagId = tagsCombination.parameterTagId,
                    parameterTag2Id = tagsCombination.parameterTag2Id,
                    parameterTag3Id = tagsCombination.parameterTag3Id,
                    parameterTag4Id = tagsCombination.parameterTag4Id,
                    parameterTag5Id = tagsCombination.parameterTag5Id,
                    parameterTag6Id = tagsCombination.parameterTag6Id
                };

                allTagsPseudoTagPresets.Add(combinationPreset.guid.ToString(), combinationPreset);
            }

            return allTagsPseudoTagPresets;
        }

        enum ChangesDetectionResult
        {
            NoExclusionsDetected = 0,
            Skip = 1,
            PreservedTagsDetected = 2,
            PreservedTagValuesDetected = 3,
            NoChangesDetected = 4,
            Ignore = 5
        }

        private static ChangesDetectionResult DetectPresetStepChanges(bool replacedTagPreserved, bool replacedTagValuePreserved)
        {
            bool leaveNotChanged = !SavedSettings.dontIncludeInPreviewLinesWithoutChangedTags;
            bool leavePreservedTagsResults = !SavedSettings.dontIncludeInPreviewLinesWithPreservedTagsAsr;
            bool leavePreservedValuesResults = !SavedSettings.dontIncludeInPreviewLinesWithPreservedTagValuesAsr;

            ChangesDetectionResult result = ChangesDetectionResult.Skip;

            if (replacedTagPreserved && leavePreservedTagsResults)
            {
                result = ChangesDetectionResult.PreservedTagsDetected;
            }
            else if (replacedTagValuePreserved && leavePreservedValuesResults)
            {
                result = ChangesDetectionResult.PreservedTagValuesDetected;
            }
            else if (leaveNotChanged)
            {
                result = ChangesDetectionResult.NoChangesDetected;
            }

            return result;
        }

        private static ChangesDetectionResult Min(ChangesDetectionResult result1, ChangesDetectionResult result2, 
            ChangesDetectionResult result3, ChangesDetectionResult result4, ChangesDetectionResult result5)
        {
            int res = 100;
            int res1 = (int)result1;
            int res2 = (int)result2;
            int res3 = (int)result3;
            int res4 = (int)result4;
            int res5 = (int)result5;

            if (res > res1)
                res = res1;

            if (res > res2)
                res = res2;

            if (res > res3)
                res = res3;

            if (res > res4)
                res = res4;

            if (res > res5)
                res = res5;

            return (ChangesDetectionResult)res;
        }

        private static (bool, ChangesDetectionResult, ChangesDetectionResult, ChangesDetectionResult, ChangesDetectionResult, ChangesDetectionResult, ChangesDetectionResult) 
            DetectTagsChanges(Preset processingPreset, SearchedAndReplacedTagsStruct searchedAndReplacedTags)
            // 1st result: false - skip, true - proceed
        {
            ChangesDetectionResult result1 = ChangesDetectionResult.NoExclusionsDetected;
            ChangesDetectionResult result2 = ChangesDetectionResult.NoExclusionsDetected;
            ChangesDetectionResult result3 = ChangesDetectionResult.NoExclusionsDetected;
            ChangesDetectionResult result4 = ChangesDetectionResult.NoExclusionsDetected;
            ChangesDetectionResult result5 = ChangesDetectionResult.NoExclusionsDetected;

            if (processingPreset.searchedPattern == "" || DetectTempTag(processingPreset.replacedTagId))
            {
                result1 = ChangesDetectionResult.Ignore;
            }
            else if (searchedAndReplacedTags.originalReplacedTagValue == searchedAndReplacedTags.replacedTagValue)
            {
                result1 = DetectPresetStepChanges(searchedAndReplacedTags.replacedTagPreserved, searchedAndReplacedTags.replacedTagValuePreserved);
            }

            if (processingPreset.searchedPattern2 == "" || DetectTempTag(processingPreset.replacedTag2Id))
            {
                result2 = ChangesDetectionResult.Ignore;
            }
            else if (searchedAndReplacedTags.originalReplacedTag2Value == searchedAndReplacedTags.replacedTag2Value)
            {
                result2 = DetectPresetStepChanges(searchedAndReplacedTags.replacedTag2Preserved, searchedAndReplacedTags.replacedTag2ValuePreserved);
            }

            if (processingPreset.searchedPattern3 == "" || DetectTempTag(processingPreset.replacedTag3Id))
            {
                result3 = ChangesDetectionResult.Ignore;
            }
            else if (searchedAndReplacedTags.originalReplacedTag3Value == searchedAndReplacedTags.replacedTag3Value)
            {
                result3 = DetectPresetStepChanges(searchedAndReplacedTags.replacedTag3Preserved, searchedAndReplacedTags.replacedTag3ValuePreserved);
            }

            if (processingPreset.searchedPattern4 == "" || DetectTempTag(processingPreset.replacedTag4Id))
            {
                result4 = ChangesDetectionResult.Ignore;
            }
            else if (searchedAndReplacedTags.originalReplacedTag4Value == searchedAndReplacedTags.replacedTag4Value)
            {
                result4 = DetectPresetStepChanges(searchedAndReplacedTags.replacedTag4Preserved, searchedAndReplacedTags.replacedTag4ValuePreserved);
            }

            if (processingPreset.searchedPattern5 == "" || DetectTempTag(processingPreset.replacedTag5Id))
            {
                result5 = ChangesDetectionResult.Ignore;
            }
            else if (searchedAndReplacedTags.originalReplacedTag5Value == searchedAndReplacedTags.replacedTag5Value)
            {
                result5 = DetectPresetStepChanges(searchedAndReplacedTags.replacedTag5Preserved, searchedAndReplacedTags.replacedTag5ValuePreserved);
            }

            ChangesDetectionResult minResult = Min(result1, result2, result3, result4, result5);
            if (minResult == ChangesDetectionResult.Skip || minResult == ChangesDetectionResult.Ignore)
                return (false, minResult, result1, result2, result3, result4, result5);
            else
                return (true, minResult, result1, result2, result3, result4, result5);
        }

        private void previewChanges()
        {
            string[] tag = { "Checked", "Preset GUID", "URL", "newTag1", "newTag2", "newTag3", "newTag4", "newTag5" };

            string[] row = { "Checked", "Preset GUID", "URL", "Track",
                "TagName1", "OriginalTag1", "NewTag1", "TagName2", "OriginalTag2", "NewTag2",
                "TagName3", "OriginalTag3", "NewTag3", "TagName4", "OriginalTag4", "NewTag4",
                "TagName5", "OriginalTag5", "NewTag5", "even/odd indicator" };


            presetProcessingCopies = generatePresetsForAllTagsPseudoTags(preset);

            string lastFile = null;
            bool even = true;

            lock (Presets)
            {
                foreach (var processingCopyGuidPreset in presetProcessingCopies)
                {
                    Preset processingPresetCopy = processingCopyGuidPreset.Value;

                    for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
                    {
                        if (backgroundTaskIsCanceled)
                            return;

                        string currentFile = files[fileCounter];
                        if (lastFile != currentFile)
                        {
                            lastFile = currentFile;
                            even = !even;
                        }

                        SetStatusbarTextForFileOperations(AsrCommandSbText, true, fileCounter, files.Length, currentFile);

                        string track = GetTrackRepresentation(currentFile);

                        var searchedAndReplacedTags = GetReplacedTags(currentFile, ref processingPresetCopy, this, false);

                        (bool proceed, ChangesDetectionResult minResult, ChangesDetectionResult changeType1, ChangesDetectionResult changeType2, 
                            ChangesDetectionResult changeType3, ChangesDetectionResult changeType4, ChangesDetectionResult changeType5) 
                            
                            = DetectTagsChanges(processingPresetCopy, searchedAndReplacedTags);

                        if (proceed)
                        {
                            tag = new string[8];

                            tag[0] = (minResult == ChangesDetectionResult.NoExclusionsDetected ? "T" : "F");
                            tag[1] = processingCopyGuidPreset.Key;
                            tag[2] = currentFile;
                            tag[3] = searchedAndReplacedTags.replacedTagValue;
                            tag[4] = searchedAndReplacedTags.replacedTag2Value;
                            tag[5] = searchedAndReplacedTags.replacedTag3Value;
                            tag[6] = searchedAndReplacedTags.replacedTag4Value;
                            tag[7] = searchedAndReplacedTags.replacedTag5Value;

                            tags.Add(tag);


                            string processedOriginalReplacedTagName = null;
                            string processedOriginalReplacedTag2Name = null;
                            string processedOriginalReplacedTag3Name = null;
                            string processedOriginalReplacedTag4Name = null;
                            string processedOriginalReplacedTag5Name = null;

                            if (allTagsReplaceIdsCount > 0 && allTagsPresetParameterTagMask[0])
                                processedOriginalReplacedTagName = GetTagName((MetaDataType)processingPresetCopy.replacedTagId);

                            if (allTagsReplaceIdsCount > 0 && allTagsPresetParameterTagMask[1])
                                processedOriginalReplacedTag2Name = GetTagName((MetaDataType)processingPresetCopy.replacedTag2Id);

                            if (allTagsReplaceIdsCount > 0 && allTagsPresetParameterTagMask[2])
                                processedOriginalReplacedTag3Name = GetTagName((MetaDataType)processingPresetCopy.replacedTag3Id);

                            if (allTagsReplaceIdsCount > 0 && allTagsPresetParameterTagMask[3])
                                processedOriginalReplacedTag4Name = GetTagName((MetaDataType)processingPresetCopy.replacedTag4Id);

                            if (allTagsReplaceIdsCount > 0 && allTagsPresetParameterTagMask[4])
                                processedOriginalReplacedTag5Name = GetTagName((MetaDataType)processingPresetCopy.replacedTag5Id);

                            row = new string[tableColumnCount];

                            row[0] = (minResult == ChangesDetectionResult.NoExclusionsDetected ? "T" : "F");
                            row[1] = processingCopyGuidPreset.Key;
                            row[2] = currentFile;
                            row[3] = track;
                            row[4] = processedOriginalReplacedTagName;
                            row[5] = searchedAndReplacedTags.originalReplacedTagValue;
                            row[6] = searchedAndReplacedTags.replacedTagValue;
                            row[7] = processedOriginalReplacedTag2Name;
                            row[8] = searchedAndReplacedTags.originalReplacedTag2Value;
                            row[9] = searchedAndReplacedTags.replacedTag2Value;
                            row[10] = processedOriginalReplacedTag3Name;
                            row[11] = searchedAndReplacedTags.originalReplacedTag3Value;
                            row[12] = searchedAndReplacedTags.replacedTag3Value;
                            row[13] = processedOriginalReplacedTag4Name;
                            row[14] = searchedAndReplacedTags.originalReplacedTag4Value;
                            row[15] = searchedAndReplacedTags.replacedTag4Value;
                            row[16] = processedOriginalReplacedTag5Name;
                            row[17] = searchedAndReplacedTags.originalReplacedTag5Value;
                            row[18] = searchedAndReplacedTags.replacedTag5Value;


                            if (even && allTagsReplaceIdsCount > 0)
                                row[19] = "⬛";
                            else if (allTagsReplaceIdsCount > 0)
                                row[19] = "⭕";
                            else
                                row[19] = null;


                            ChangesDetectionResult[] changeTypes = new ChangesDetectionResult[5];

                            changeTypes[0] = changeType1;
                            changeTypes[1] = changeType2;
                            changeTypes[2] = changeType3;
                            changeTypes[3] = changeType4;
                            changeTypes[4] = changeType5;


                            Invoke(addRowToTable, row, changeTypes);

                            previewIsGenerated = true;
                        }
                    }
                }
            }

            SetResultingSbText();
        }

        private void applyToSelected()
        {
            if (tags.Count == 0)
                previewChanges();

            for (int i = 0; i < tags.Count; i++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                string isChecked = tags[i][0];
                Preset combinationPreset = presetProcessingCopies[tags[i][1]];
                string currentFile = tags[i][2];

                if (isChecked == "T")
                {
                    lock (Presets)
                    {
                        SearchedAndReplacedTagsStruct searchedAndReplacedTags = new SearchedAndReplacedTagsStruct();
                        tags[i][0] = "";

                        searchedAndReplacedTags.replacedTagValue = tags[i][3];
                        searchedAndReplacedTags.replacedTag2Value = tags[i][4];
                        searchedAndReplacedTags.replacedTag3Value = tags[i][5];
                        searchedAndReplacedTags.replacedTag4Value = tags[i][6];
                        searchedAndReplacedTags.replacedTag5Value = tags[i][7];

                        SaveReplacedTags(currentFile, combinationPreset, searchedAndReplacedTags, this);
                    }

                    Invoke(processRowOfTable, new object[] { i });
                }

                SetStatusbarTextForFileOperations(AsrCommandSbText, false, i, tags.Count, currentFile);
            }


            if (preset.replacedTagId == (int)ClipboardTagId || preset.replacedTag2Id == (int)ClipboardTagId || preset.replacedTag3Id == (int)ClipboardTagId
                || preset.replacedTag4Id == (int)ClipboardTagId || preset.replacedTag5Id == (int)ClipboardTagId)
            {
                System.Threading.Thread thread = new System.Threading.Thread(() => Clipboard.SetText(clipboardText));
                thread.SetApartmentState(System.Threading.ApartmentState.STA); //Set the thread to STA
                thread.Start();
                thread.Join();
            }


            RefreshPanels(true);

            SetResultingSbText();
        }

        public static string AsrGetTagName(int tagId)
        {
            string tagName;

            if (tagId == (int)ServiceMetaData.ParameterTagId1)
                tagName = "<" + ParameterTagName + " 1>";
            else if (tagId == (int)ServiceMetaData.ParameterTagId2)
                tagName = "<" + ParameterTagName + " 2>";
            else if (tagId == (int)ServiceMetaData.ParameterTagId3)
                tagName = "<" + ParameterTagName + " 3>";
            else if (tagId == (int)ServiceMetaData.ParameterTagId4)
                tagName = "<" + ParameterTagName + " 4>";
            else if (tagId == (int)ServiceMetaData.ParameterTagId5)
                tagName = "<" + ParameterTagName + " 5>";
            else if (tagId == (int)ServiceMetaData.ParameterTagId6)
                tagName = "<" + ParameterTagName + " 6>";
            else if (tagId == (int)ServiceMetaData.TempTag1)
                tagName = "<" + TempTagName + " 1>";
            else if (tagId == (int)ServiceMetaData.TempTag2)
                tagName = "<" + TempTagName + " 2>";
            else if (tagId == (int)ServiceMetaData.TempTag3)
                tagName = "<" + TempTagName + " 3>";
            else if (tagId == (int)ServiceMetaData.TempTag4)
                tagName = "<" + TempTagName + " 4>";
            else if (tagId == (int)ClipboardTagId)
                tagName = ClipboardTagName;
            else if (tagId < PropMetaDataThreshold)
                tagName = GetTagName((MetaDataType)tagId);
            else
                tagName = GetPropName((FilePropertyType)(tagId - PropMetaDataThreshold));

            return tagName;
        }

        public static int AsrGetTagId(string tagName)
        {
            int tagId;

            if (tagName == "<" + ParameterTagName + " 1>")
                tagId = (int)ServiceMetaData.ParameterTagId1;
            else if (tagName == "<" + ParameterTagName + " 2>")
                tagId = (int)ServiceMetaData.ParameterTagId2;
            else if (tagName == "<" + ParameterTagName + " 3>")
                tagId = (int)ServiceMetaData.ParameterTagId3;
            else if (tagName == "<" + ParameterTagName + " 4>")
                tagId = (int)ServiceMetaData.ParameterTagId4;
            else if (tagName == "<" + ParameterTagName + " 5>")
                tagId = (int)ServiceMetaData.ParameterTagId5;
            else if (tagName == "<" + ParameterTagName + " 6>")
                tagId = (int)ServiceMetaData.ParameterTagId6;
            else if (tagName == "<" + TempTagName + " 1>")
                tagId = (int)ServiceMetaData.TempTag1;
            else if (tagName == "<" + TempTagName + " 2>")
                tagId = (int)ServiceMetaData.TempTag2;
            else if (tagName == "<" + TempTagName + " 3>")
                tagId = (int)ServiceMetaData.TempTag3;
            else if (tagName == "<" + TempTagName + " 4>")
                tagId = (int)ServiceMetaData.TempTag4;
            else if (tagName == ClipboardTagName)
                tagId = (int)ClipboardTagId;
            else
                tagId = (int)GetTagId(tagName);


            if (tagId == 0)
            {
                tagId = (int)GetPropId(tagName);

                if (tagId != 0)
                    tagId += PropMetaDataThreshold;
            }

            return tagId;
        }

        private void editPreset(Preset tempPreset, bool itsNewPreset, bool readOnly)
        {
            bool presetChanged;
            using (ASRPresetEditor tagToolsForm = new ASRPresetEditor(TagToolsPlugin))
            {
                presetChanged = tagToolsForm.editPreset(tempPreset, readOnly);
            }

            if (presetChanged)
            {
                if (!readOnly)
                {
                    tempPreset.modifiedUtc = DateTime.UtcNow;
                    presetsChanged = true;

                    buttonClose.Image = Warning;
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

                    refreshPresetList(tempPreset.guid);
                }
                else
                {
                    refreshPresetList(tempPreset.guid);
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
            saveColumnWidths(preset);

            SortedDictionary<string, bool> savedPresetPaths = new SortedDictionary<string, bool>();
            SortedDictionary<string, int> countedPresetFilenames = new SortedDictionary<string, int>();
            foreach (Preset tempPreset in presetsWorkingCopy.Values)
            {
                string presetFilename = getCountedPresetFilename(countedPresetFilenames, tempPreset.getSafeFileName());
                savedPresetPaths.Add(tempPreset.savePreset(Path.Combine(PresetsPath, presetFilename + ASRPresetExtension)), false);
            }

            if (MSR != null)
            {
                string presetFilename = getCountedPresetFilename(countedPresetFilenames, MSR.getSafeFileName());
                savedPresetPaths.Add(MSR.savePreset(Path.Combine(PresetsPath, presetFilename + ASRPresetExtension)), false);
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

            ASRPresetsWithHotkeysCount = asrPresetsWithHotkeysCount;

            SavedSettings.asrPresetsWithHotkeysGuids = new Guid[MaximumNumberOfASRHotkeys];
            AsrPresetsWithHotkeys = new Preset[MaximumNumberOfASRHotkeys];
            for (int j = 0; j < MaximumNumberOfASRHotkeys; j++)
            {
                SavedSettings.asrPresetsWithHotkeysGuids[j] = asrPresetsWithHotkeysGuids[j];

                if (asrPresetsWithHotkeysGuids[j] != Guid.Empty)
                    AsrPresetsWithHotkeys[j] = Presets[asrPresetsWithHotkeysGuids[j]];
                else
                    AsrPresetsWithHotkeys[j] = null;
            }

            SavedSettings.autoAppliedAsrPresetGuids = new List<Guid>();
            AsrAutoAppliedPresets = new List<Preset>();
            foreach (Guid guid in autoAppliedAsrPresetGuids.Keys)
            {
                SavedSettings.autoAppliedAsrPresetGuids.Add(guid);
                AsrAutoAppliedPresets.Add(Presets[guid]);
            }

            IdsAsrPresets = new SortedDictionary<string, Preset>();
            foreach (var pair in asrIdsPresetGuids)
            {
                IdsAsrPresets.Add(pair.Key, Presets[pair.Value]);
            }


            TagToolsPlugin.SaveSettings();

            RegisterASRPresetsHotkeysAndMenuItems(TagToolsPlugin);

            presetsChanged = false;
            buttonClose.Image = Resources.transparent_15;
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

            if (Form.ModifierKeys != Keys.Control)
                Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            saveSettings();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void deletePreset(Preset presetToRemove)
        {
            if (autoAppliedAsrPresetGuids.TryGetValue(presetToRemove.guid, out _))
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
            buttonClose.Image = Warning;
            toolTip1.SetToolTip(buttonClose, buttonCloseToolTip);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this, MsgDeletePresetConfirmation, "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No)
                return;

            deletePreset((Preset)presetList.SelectedItem);
            showAutoapplyingWarningIfRequired();
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

            newPreset.names.Add(Language, DefaultASRPresetName + (presetList.Items.Count + 1));
            newPreset.ignoreCase = true;

            editPreset(newPreset, true, false);
        }

        public static TagType GetTagTypeByPresetParameterTagType(int presetParameterTagTypeOld)
        {
            TagType tagType;

            switch (presetParameterTagTypeOld)
            {
                case 0:
                    tagType = TagType.NotUsed;
                    break;
                case 1:
                    tagType = TagType.Writable;
                    break;
                case 2:
                    tagType = TagType.Readonly;
                    break;
                case 3:
                    tagType = TagType.WritableAllowAllTags;
                    break;
                default:
                    tagType = TagType.NotUsed;
                    break;
            }

            return tagType;
        }

        public static void FillParameterTagList(TagType parameterTagType, string selectedTagName, ComboBox parameterTagListParam, Label parameterTagLabelParam)
        {
            parameterTagListParam.Items.Clear();

            if (parameterTagType == TagType.NotUsed) //Not used
            {
                selectedTagName = "";
                parameterTagListParam.Enable(false);
                parameterTagLabelParam.Enable(false);
            }
            else if (parameterTagType == TagType.Readonly) //Read only
            {
                FillListByTagNames(parameterTagListParam.Items, true);
                FillListByPropNames(parameterTagListParam.Items);
                parameterTagListParam.Enable(true);
                parameterTagLabelParam.Enable(true);
            }
            else if (parameterTagType == TagType.Writable) //Writable
            {
                FillListByTagNames(parameterTagListParam.Items);
                parameterTagListParam.Enable(true);
                parameterTagLabelParam.Enable(true);
            }
            else //Writable (incl. <All Tags>)
            {
                FillListByTagNames(parameterTagListParam.Items, false, false, true, false, true);
                parameterTagListParam.Enable(true);
                parameterTagLabelParam.Enable(true);
            }


            if (parameterTagListParam.Items.Contains(selectedTagName))
                parameterTagListParam.SelectedItem = selectedTagName;
            else if (parameterTagListParam.Items.Count > 0)
                parameterTagListParam.SelectedIndex = 0;
            else
                parameterTagListParam.SelectedIndex = -1;
        }

        private static bool DetectTempTag(int tagId)
        {
            if (tagId <= (int)ServiceMetaData.TempTag1 || tagId == (int)NullTagId)
                return true;
            else
                return false;
        }

        public void nameColumns()
        {
            if (preset == null)
            {
                previewTable.Columns[4].Visible = false;
                previewTable.Columns[5].Visible = false;
                previewTable.Columns[6].Visible = false;
                previewTable.Columns[7].Visible = false;
                previewTable.Columns[8].Visible = false;
                previewTable.Columns[9].Visible = false;
                previewTable.Columns[10].Visible = false;
                previewTable.Columns[11].Visible = false;
                previewTable.Columns[12].Visible = false;
                previewTable.Columns[13].Visible = false;
                previewTable.Columns[14].Visible = false;
                previewTable.Columns[15].Visible = false;
                previewTable.Columns[16].Visible = false;
                previewTable.Columns[17].Visible = false;
                previewTable.Columns[18].Visible = false;
                previewTable.Columns[19].Visible = false;

                return;
            }


            if (allTagsReplaceIdsCount == 0)
                previewTable.Columns[19].Visible = false;
            else
                previewTable.Columns[19].Visible = true;

            if (preset.searchedPattern != "")
            {
                previewTable.Columns[4].Visible = allTagsPresetParameterTagMask[0];
                previewTable.Columns[4].HeaderText = TagNameText;
                previewTable.Columns[5].HeaderText = GetTagName((MetaDataType)preset.substituteTagId(preset.replacedTagId), OrigValueText);
                previewTable.Columns[6].HeaderText = NewValueText;

                if (DetectTempTag(preset.replacedTagId))
                {
                    previewTable.Columns[5].Visible = false;
                    previewTable.Columns[6].Visible = false;
                }
                else
                {
                    previewTable.Columns[5].Visible = true;
                    previewTable.Columns[6].Visible = true;
                }

                if (preset.columnWeights.Count > 3)
                {
                    previewTable.Columns[3].FillWeight = preset.columnWeights[0];
                    previewTable.Columns[4].FillWeight = preset.columnWeights[1];
                    previewTable.Columns[5].FillWeight = preset.columnWeights[2];
                    previewTable.Columns[6].FillWeight = preset.columnWeights[3];
                }
                else
                {
                    previewTable.Columns[3].Width = 737;
                    previewTable.Columns[4].Width = 100;
                    previewTable.Columns[5].Width = 100;
                    previewTable.Columns[6].Width = 100;
                    previewTable.Columns[7].Width = 100;
                    previewTable.Columns[8].Width = 100;
                    previewTable.Columns[9].Width = 100;
                    previewTable.Columns[10].Width = 100;
                    previewTable.Columns[11].Width = 100;
                    previewTable.Columns[12].Width = 100;
                    previewTable.Columns[13].Width = 100;
                    previewTable.Columns[14].Width = 100;
                    previewTable.Columns[15].Width = 100;
                    previewTable.Columns[16].Width = 100;
                    previewTable.Columns[17].Width = 100;
                    previewTable.Columns[18].Width = 100;
                }


                if (preset.searchedPattern2 != "")
                {
                    previewTable.Columns[7].Visible = allTagsPresetParameterTagMask[1];
                    previewTable.Columns[7].HeaderText = TagNameText;
                    previewTable.Columns[8].HeaderText = GetTagName((MetaDataType)preset.substituteTagId(preset.replacedTag2Id), OrigValueText);
                    previewTable.Columns[9].HeaderText = NewValueText;

                    if (DetectTempTag(preset.replacedTag2Id))
                    {
                        previewTable.Columns[8].Visible = false;
                        previewTable.Columns[9].Visible = false;
                    }
                    else
                    {
                        previewTable.Columns[8].Visible = true;
                        previewTable.Columns[9].Visible = true;
                    }

                    if (previewTable.Columns[8].HeaderText == previewTable.Columns[5].HeaderText)
                    {
                        previewTable.Columns[6].Visible = false;
                        previewTable.Columns[7].Visible = false;
                        previewTable.Columns[8].Visible = false;
                    }

                    if (preset.columnWeights.Count > 6)
                    {
                        previewTable.Columns[7].FillWeight = preset.columnWeights[4];
                        previewTable.Columns[8].FillWeight = preset.columnWeights[5];
                        previewTable.Columns[9].FillWeight = preset.columnWeights[6];
                    }


                    if (preset.searchedPattern3 != "")
                    {
                        previewTable.Columns[10].Visible = allTagsPresetParameterTagMask[2];
                        previewTable.Columns[10].HeaderText = TagNameText;
                        previewTable.Columns[11].HeaderText = GetTagName((MetaDataType)preset.substituteTagId(preset.replacedTag3Id), OrigValueText);
                        previewTable.Columns[12].HeaderText = NewValueText;

                        if (DetectTempTag(preset.replacedTag3Id))
                        {
                            previewTable.Columns[11].Visible = false;
                            previewTable.Columns[12].Visible = false;
                        }
                        else
                        {
                            previewTable.Columns[11].Visible = true;
                            previewTable.Columns[12].Visible = true;
                        }

                        if (previewTable.Columns[11].HeaderText == previewTable.Columns[8].HeaderText)
                        {
                            previewTable.Columns[9].Visible = false;
                            previewTable.Columns[10].Visible = false;
                            previewTable.Columns[11].Visible = false;
                        }

                        if (preset.columnWeights.Count > 9)
                        {
                            previewTable.Columns[10].FillWeight = preset.columnWeights[7];
                            previewTable.Columns[11].FillWeight = preset.columnWeights[8];
                            previewTable.Columns[12].FillWeight = preset.columnWeights[9];
                        }


                        if (preset.searchedPattern4 != "")
                        {
                            previewTable.Columns[13].Visible = allTagsPresetParameterTagMask[3];
                            previewTable.Columns[13].HeaderText = TagNameText;
                            previewTable.Columns[14].HeaderText = GetTagName((MetaDataType)preset.substituteTagId(preset.replacedTag4Id), OrigValueText);
                            previewTable.Columns[15].HeaderText = NewValueText;

                            if (DetectTempTag(preset.replacedTag4Id))
                            {
                                previewTable.Columns[14].Visible = false;
                                previewTable.Columns[15].Visible = false;
                            }
                            else
                            {
                                previewTable.Columns[14].Visible = true;
                                previewTable.Columns[15].Visible = true;
                            }

                            if (previewTable.Columns[14].HeaderText == previewTable.Columns[11].HeaderText)
                            {
                                previewTable.Columns[12].Visible = false;
                                previewTable.Columns[13].Visible = false;
                                previewTable.Columns[14].Visible = false;
                            }

                            if (preset.columnWeights.Count > 12)
                            {
                                previewTable.Columns[13].FillWeight = preset.columnWeights[10];
                                previewTable.Columns[14].FillWeight = preset.columnWeights[11];
                                previewTable.Columns[15].FillWeight = preset.columnWeights[12];
                            }


                            if (preset.searchedPattern5 != "")
                            {
                                previewTable.Columns[16].Visible = allTagsPresetParameterTagMask[4];
                                previewTable.Columns[16].HeaderText = TagNameText;
                                previewTable.Columns[17].HeaderText = GetTagName((MetaDataType)preset.substituteTagId(preset.replacedTag5Id), OrigValueText);
                                previewTable.Columns[18].HeaderText = NewValueText;

                                if (DetectTempTag(preset.replacedTag5Id))
                                {
                                    previewTable.Columns[17].Visible = false;
                                    previewTable.Columns[18].Visible = false;
                                }
                                else
                                {
                                    previewTable.Columns[17].Visible = (previewTable.Columns[14].HeaderText != "");
                                    previewTable.Columns[18].Visible = (previewTable.Columns[14].HeaderText != "");
                                }

                                if (previewTable.Columns[17].HeaderText == previewTable.Columns[14].HeaderText)
                                {
                                    previewTable.Columns[15].Visible = false;
                                    previewTable.Columns[16].Visible = false;
                                    previewTable.Columns[17].Visible = false;
                                }

                                if (preset.columnWeights.Count > 15)
                                {
                                    previewTable.Columns[16].FillWeight = preset.columnWeights[13];
                                    previewTable.Columns[17].FillWeight = preset.columnWeights[14];
                                    previewTable.Columns[18].FillWeight = preset.columnWeights[15];
                                }
                            }
                            else
                            {
                                previewTable.Columns[16].HeaderText = "";
                                previewTable.Columns[17].HeaderText = "";
                                previewTable.Columns[18].HeaderText = "";

                                previewTable.Columns[16].Visible = false;
                                previewTable.Columns[17].Visible = false;
                                previewTable.Columns[18].Visible = false;
                            }
                        }
                        else
                        {
                            previewTable.Columns[13].HeaderText = "";
                            previewTable.Columns[14].HeaderText = "";
                            previewTable.Columns[15].HeaderText = "";
                            previewTable.Columns[16].HeaderText = "";
                            previewTable.Columns[17].HeaderText = "";
                            previewTable.Columns[18].HeaderText = "";

                            previewTable.Columns[13].Visible = false;
                            previewTable.Columns[14].Visible = false;
                            previewTable.Columns[15].Visible = false;
                            previewTable.Columns[16].Visible = false;
                            previewTable.Columns[17].Visible = false;
                            previewTable.Columns[18].Visible = false;
                        }
                    }
                    else
                    {
                        previewTable.Columns[10].HeaderText = "";
                        previewTable.Columns[11].HeaderText = "";
                        previewTable.Columns[12].HeaderText = "";
                        previewTable.Columns[13].HeaderText = "";
                        previewTable.Columns[14].HeaderText = "";
                        previewTable.Columns[15].HeaderText = "";
                        previewTable.Columns[16].HeaderText = "";
                        previewTable.Columns[17].HeaderText = "";
                        previewTable.Columns[18].HeaderText = "";

                        previewTable.Columns[10].Visible = false;
                        previewTable.Columns[11].Visible = false;
                        previewTable.Columns[12].Visible = false;
                        previewTable.Columns[13].Visible = false;
                        previewTable.Columns[14].Visible = false;
                        previewTable.Columns[15].Visible = false;
                        previewTable.Columns[16].Visible = false;
                        previewTable.Columns[17].Visible = false;
                        previewTable.Columns[18].Visible = false;
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
                    previewTable.Columns[13].HeaderText = "";
                    previewTable.Columns[14].HeaderText = "";
                    previewTable.Columns[15].HeaderText = "";
                    previewTable.Columns[16].HeaderText = "";
                    previewTable.Columns[17].HeaderText = "";
                    previewTable.Columns[18].HeaderText = "";

                    previewTable.Columns[7].Visible = false;
                    previewTable.Columns[8].Visible = false;
                    previewTable.Columns[9].Visible = false;
                    previewTable.Columns[10].Visible = false;
                    previewTable.Columns[11].Visible = false;
                    previewTable.Columns[12].Visible = false;
                    previewTable.Columns[13].Visible = false;
                    previewTable.Columns[14].Visible = false;
                    previewTable.Columns[15].Visible = false;
                    previewTable.Columns[16].Visible = false;
                    previewTable.Columns[17].Visible = false;
                    previewTable.Columns[18].Visible = false;
                }
            }
            else
            {
                previewTable.Columns[4].HeaderText = "";
                previewTable.Columns[5].HeaderText = "";
                previewTable.Columns[6].HeaderText = "";
                previewTable.Columns[7].HeaderText = "";
                previewTable.Columns[8].HeaderText = "";
                previewTable.Columns[9].HeaderText = "";
                previewTable.Columns[10].HeaderText = "";
                previewTable.Columns[11].HeaderText = "";
                previewTable.Columns[12].HeaderText = "";
                previewTable.Columns[13].HeaderText = "";
                previewTable.Columns[14].HeaderText = "";
                previewTable.Columns[15].HeaderText = "";
                previewTable.Columns[16].HeaderText = "";
                previewTable.Columns[17].HeaderText = "";
                previewTable.Columns[18].HeaderText = "";

                previewTable.Columns[4].Visible = false;
                previewTable.Columns[5].Visible = false;
                previewTable.Columns[6].Visible = false;
                previewTable.Columns[7].Visible = false;
                previewTable.Columns[8].Visible = false;
                previewTable.Columns[9].Visible = false;
                previewTable.Columns[10].Visible = false;
                previewTable.Columns[11].Visible = false;
                previewTable.Columns[12].Visible = false;
                previewTable.Columns[13].Visible = false;
                previewTable.Columns[14].Visible = false;
                previewTable.Columns[15].Visible = false;
                previewTable.Columns[16].Visible = false;
                previewTable.Columns[17].Visible = false;
                previewTable.Columns[18].Visible = false;
            }
        }

        private void setCheckedState(PictureBox label, bool flag)
        {
            if (flag)
            {
                label.Image = CheckedState;
            }
            else
            {
                label.Image = UncheckedState;
            }
            presetList.Refresh();
        }

        private void presetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            processPresetChanges = false;

            saveColumnWidths(preset);


            if (presetList.SelectedIndex == -1)
            {
                preset = null;
                backupedPreset = null;

                descriptionBox.Text = "";

                userPresetPictureBox.Image = UncheckedState;
                customizedPresetPictureBox.Image = UncheckedState;

                labelPreserveTagValues.Enable(false);
                preserveTagValuesTextBox.Text = "";
                preserveTagValuesTextBox.Enable(false);

                allTagsReplaceIdsCount = 0;
                allTagsPresetParameterTagMask = new bool[] { false, false, false, false, false };

                processTagsMode = true;
                buttonProcessPreserveTags.Text = AsrProcessTagsButtonName;
                buttonProcessPreserveTags.Enable(false);
                processPreserveTagsTextBox.Text = "";
                processPreserveTagsTextBox.Enable(false);

                parameterTagList.Items.Clear();
                parameterTagList.Enable(false);
                labelTag.Enable(false);

                parameterTag2List.Items.Clear();
                parameterTag2List.Enable(false);
                labelTag2.Enable(false);

                parameterTag3List.Items.Clear();
                parameterTag3List.Enable(false);
                labelTag3.Enable(false);

                parameterTag4List.Items.Clear();
                parameterTag4List.Enable(false);
                labelTag4.Enable(false);

                parameterTag5List.Items.Clear();
                parameterTag5List.Enable(false);
                labelTag5.Enable(false);

                parameterTag6List.Items.Clear();
                parameterTag6List.Enable(false);
                labelTag6.Enable(false);

                customTextBox.Enable(false);
                customTextLabel.Enable(false);

                customText2Box.Enable(false);
                customText2Label.Enable(false);

                customText3Box.Enable(false);
                customText3Label.Enable(false);

                customText4Box.Enable(false);
                customText4Label.Enable(false);

                conditionCheckBox.Checked = false;
                conditionCheckBox.Enable(false);

                assignHotkeyCheckBox.Checked = false;
                assignHotkeyCheckBox.Enable(false);

                applyToPlayingTrackCheckBox.Checked = false;
                applyToPlayingTrackCheckBox.Enable(false);

                idTextBox.Enable(false);
                clearIdButton.Enable(false);

                favoriteCheckBox.Enable(false);
                favoriteCheckBox.Checked = false;

                buttonExport.Enable(false);
                buttonCopy.Enable(false);
                buttonEdit.Enable(false);
                buttonDelete.Enable(false);
                buttonPreview.Enable(false);
                buttonOK.Enable(false);
            }
            else
            {
                presetsWorkingCopy.TryGetValue(((Preset)presetList.SelectedItem).guid, out preset);
                backupedPreset = new Preset(preset);

                if (!preset.userPreset && !DeveloperMode)
                    editButtonEnabled = false;
                else
                    editButtonEnabled = true;


                descriptionBox.Text = GetDictValue(preset.descriptions, Language);

                setCheckedState(userPresetPictureBox, preset.userPreset);
                setCheckedState(customizedPresetPictureBox, preset.getCustomizationsFlag());


                labelPreserveTagValues.Enable(true);
                preserveTagValuesTextBox.Enable(true);
                preserveTagValuesTextBox.Text = preset.preserveValues;

                buttonProcessPreserveTags.Enable(true);
                processPreserveTagsTextBox.Enable(true);
                processPreserveTagsTextBox.ReadOnly = false;

                (allTagsReplaceIdsCount, allTagsPresetParameterTagMask) = preset.getAllTagsReplaceTagIds();

                processTagsMode = preset.processTagsMode;
                if (processTagsMode)
                {
                    buttonProcessPreserveTags.Text = AsrProcessTagsButtonName;
                    processPreserveTagsTextBox.Text = preset.processTags;
                }
                else
                {
                    buttonProcessPreserveTags.Text = AsrPreserveTagsButtonName;
                    processPreserveTagsTextBox.Text = preset.preserveTags;
                }



                FillParameterTagList(preset.parameterTagTypeNew, AsrGetTagName(preset.parameterTagId), parameterTagList, labelTag);
                FillParameterTagList(preset.parameterTag2TypeNew, AsrGetTagName(preset.parameterTag2Id), parameterTag2List, labelTag2);
                FillParameterTagList(preset.parameterTag3TypeNew, AsrGetTagName(preset.parameterTag3Id), parameterTag3List, labelTag3);
                FillParameterTagList(preset.parameterTag4TypeNew, AsrGetTagName(preset.parameterTag4Id), parameterTag4List, labelTag4);
                FillParameterTagList(preset.parameterTag5TypeNew, AsrGetTagName(preset.parameterTag5Id), parameterTag5List, labelTag5);
                FillParameterTagList(preset.parameterTag6TypeNew, AsrGetTagName(preset.parameterTag6Id), parameterTag6List, labelTag6);

                customTextBox.Text = preset.customTextChecked ? preset.customText : "";
                customTextBox.Enable(preset.customTextChecked);
                customTextLabel.Enable(preset.customTextChecked);
                customText2Box.Text = preset.customText2Checked ? preset.customText2 : "";
                customText2Box.Enable(preset.customText2Checked);
                customText2Label.Enable(preset.customText2Checked);
                customText3Box.Text = preset.customText3Checked ? preset.customText3 : "";
                customText3Box.Enable(preset.customText3Checked);
                customText3Label.Enable(preset.customText3Checked);
                customText4Box.Text = preset.customText4Checked ? preset.customText4 : "";
                customText4Box.Enable(preset.customText4Checked);
                customText4Label.Enable(preset.customText4Checked);

                bool hotkeyAssigned = preset.hotkeyAssigned;
                if (asrPresetsWithHotkeysCount >= MaximumNumberOfASRHotkeys && !hotkeyAssigned)
                {
                    assignHotkeyCheckBox.Enable(false);
                    assignHotkeyCheckBox.Checked = false;
                }
                else
                {
                    assignHotkeyCheckBox.Enable(true);
                    assignHotkeyCheckBox.Checked = hotkeyAssigned;
                }

                applyToPlayingTrackCheckBox.Enable(assignHotkeyCheckBox.Checked);
                applyToPlayingTrackCheckBox.Checked = preset.applyToPlayingTrack;


                idTextBox.Enable(true);
                clearIdButton.Enable(true);

                favoriteCheckBox.Enable(true);
                favoriteCheckBox.Checked = preset.favorite;

                buttonExport.Enable(editButtonEnabled || preset.customizedByUser);
                buttonCopy.Enable(true);
                buttonEdit.Enable(editButtonEnabled);
                buttonDelete.Enable(true);
                buttonPreview.Enable(true);
                buttonOK.Enable(!selectedPresetUsesAllTags || previewIsGenerated);

                idTextBox.Text = preset.id;


                if (playlistComboBox.Items.Count == 0)
                {
                    conditionCheckBox.Enable(false);
                    conditionCheckBox.Checked = false;
                }
                else
                {
                    conditionCheckBox.Enable(true);

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
                            conditionCheckBox.Checked = false;
                    }
                    else
                    {
                        conditionCheckBox.Checked = false;
                    }
                }
            }

            //Normally preset referring <All Tags> must not be checked for auto-execution, but this function call shows icons on tag labels
            showAllTagsWarningIfRequired(preset, true);

            //Lets deal with preview table
            nameColumns();

            processPresetChanges = true;
        }

        private void exportPreset(Preset preset, string presetPathName)
        {
            Preset savedPreset = new Preset(preset)
            {
                applyToPlayingTrack = false,
                condition = false,
                playlist = null,
                preserveValues = "",
                id = ""
            };

            savedPreset.savePreset(presetPathName);
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            string presetSafeFileName = preset.getSafeFileName();

            SaveFileDialog dialog = new SaveFileDialog
            {
                //Title = "Save ASR preset",
                Filter = ASRPresetNaming + "|*" + ASRPresetExtension,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                FileName = presetSafeFileName
            };

            if (dialog.ShowDialog(this) == DialogResult.Cancel) return;

            exportPreset(preset, dialog.FileName);
        }

        private void buttonExportUser_Click(object sender, EventArgs e)
        {
            bool developerExport = false;
            if (DeveloperMode && ModifierKeys == Keys.Control)
            {
                developerExport = true;
            }

            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                //Description = "Save user ASR presets",
                SelectedPath = SavedSettings.defaultAsrPresetsExportFolder,
            };

            if (dialog.ShowDialog(this) == DialogResult.Cancel) return;
            SavedSettings.defaultAsrPresetsExportFolder = dialog.SelectedPath;

            SortedDictionary<string, int> countedPresetFileNames = new SortedDictionary<string, int>();
            foreach (var currentPresetKVPair in presetsWorkingCopy)
            {
                if (currentPresetKVPair.Value.userPreset ^ developerExport)
                {
                    string presetFilename;

                    if (developerExport)
                    {
                        presetFilename = "!" + currentPresetKVPair.Value.guid.ToString("B");
                    }
                    else
                    {
                        presetFilename = getCountedPresetFilename(countedPresetFileNames, currentPresetKVPair.Value.getSafeFileName());
                    }

                    string presetFilePath = @"\\?\" + Path.Combine(dialog.SelectedPath, presetFilename + ASRPresetExtension);
                    exportPreset(currentPresetKVPair.Value, presetFilePath);
                }
            }

            if (developerExport && MSR != null)
            {
                string presetFilePath = @"\\?\" + Path.Combine(dialog.SelectedPath, "!" + MSR.guid.ToString("B") + ASRPresetExtension);
                exportPreset(MSR, presetFilePath);
            }
        }

        public void import()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                //Title = "Load ASR preset",
                Filter = ASRPresetNaming + "|*" + ASRPresetExtension,
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

            foreach (string filename in dialog.FileNames)
            {
                try
                {
                    Preset newPreset = Preset.Load(filename, presetSerializer);

                    if (!presetsWorkingCopy.TryGetValue(newPreset.guid, out Preset existingPreset))
                        existingPreset = null;

                    if (existingPreset != null)
                    {
                        if (askToImportExistingAsCopy)
                        {
                            DialogResult result = MessageBox.Show(this, MsgDoYouWantToImportExistingPresetsAsCopies, 
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
                            newPreset.copyExtendedCustomizationsFrom(existingPreset);

                            presetsWorkingCopy.Remove(newPreset.guid);
                            presetsWorkingCopy.Add(newPreset.guid, newPreset);
                            numberOfImportedPresets++;
                        }
                    }
                    else //if (existingPreset == null)
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
                message += AddLeadingSpaces(numberOfImportedPresets, 4) + GetPluralForm(MsgPresetsWereImported, numberOfImportedPresets);
            if (numberOfImportedAsCopyPresets > 0)
                message += AddLeadingSpaces(numberOfImportedAsCopyPresets, 4) + GetPluralForm(MsgPresetsWereImportedAsCopies, numberOfImportedAsCopyPresets);
            if (numberOfErrors > 0)
                message += AddLeadingSpaces(numberOfErrors, 4) + GetPluralForm(MsgPresetsFailedToImport, numberOfErrors);

            if (numberOfImportedPresets + numberOfImportedAsCopyPresets > 0)
            {
                presetsChanged = true;
                buttonClose.Image = Warning;
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

        public void setPresetsChanged()
        {
            presetsChanged = true;
            buttonClose.Image = Warning;
            toolTip1.SetToolTip(buttonClose, buttonCloseToolTip);
        }

        public void refreshPresetList(Guid selectedPresetGuid)
        {
            searchTextBox.Text = "";
            showPlaylistLimitedChecked = false;
            showFunctionIdAssignedChecked = false;
            showHotkeyAssignedChecked = false;
            showTickedOnlyChecked = false;

            presetList.Items.Clear();
            presetList.Sorted = false;
            ignoreCheckedPresetEvent = false;
            autoAppliedPresetCount = 0;
            foreach (Preset tempPreset in presetsWorkingCopy.Values)
                presetList.Items.Add(tempPreset, autoAppliedAsrPresetGuids.TryGetValue(tempPreset.guid, out _));

            presetList.SelectedIndex = -1;
            presetList_SelectedIndexChanged(null, null);
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


            if (MessageBox.Show(this, MsgInstallingConfirmation, "", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) 
                == DialogResult.No)
                return;


            if (!Directory.Exists(PresetsPath))
                Directory.CreateDirectory(PresetsPath);

            if (Directory.Exists(PluginsPath))
            {
                newPresetNames = Directory.GetFiles(Path.Combine(PluginsPath, AsrPresetsDirectory), "*" + ASRPresetExtension);
                System.Xml.Serialization.XmlSerializer presetSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Preset));

                bool askToResetCustomizedByUser = true;
                bool resetCustomizedByUser = true;
                bool askToRemovePresets = true;
                bool removePresets = true;
                for (int i = newPresetNames.Length - 1; i >= 0; i--)
                {
                    string newPresetName = newPresetNames[i];
                    
                    try
                    {
                        Preset newPreset = Preset.Load(newPresetName, presetSerializer);

                        if (newPreset.guid.ToString() != "ff8d53d9-526b-4b40-bbf0-848b6b892f70")
                        {
                            if (!presetsWorkingCopy.TryGetValue(newPreset.guid, out Preset existingPreset))
                                existingPreset = null;

                            if (existingPreset != null)
                            {
                                bool anyCustomization = existingPreset.getCustomizationsFlag();
                                if (existingPreset.condition || existingPreset.applyToPlayingTrack)
                                    anyCustomization = true;

                                if (installAll)
                                {
                                    if (askToResetCustomizedByUser && anyCustomization)
                                    {
                                        if (MessageBox.Show(this, MsgDoYouWantToResetYourCustomizedPredefinedPresets, "", MessageBoxButtons.YesNo, 
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
                                        if (MessageBox.Show(this, MsgDoYouWantToRemovePredefinedPresets, "", MessageBoxButtons.YesNo, 
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
                                        deletePreset(existingPreset);
                                        numberOfRemovedPresets++;
                                    }
                                    else if (!anyCustomization || resetCustomizedByUser)
                                    {
                                        newPreset.copyBasicCustomizationsFrom(existingPreset);

                                        presetsWorkingCopy.Remove(newPreset.guid);
                                        presetsWorkingCopy.Add(newPreset.guid, newPreset);
                                        if (newPreset.modifiedUtc > existingPreset.modifiedUtc) //Updating
                                            numberOfUpdatedPresets++;
                                        else
                                            numberOfReinstalledPresets++;
                                    }
                                    else //Update preset to latest version, and copy *all* customizations
                                    {
                                        newPreset.copyAdvancedCustomizationsFrom(existingPreset);
                                        newPreset.preserveValues = existingPreset.preserveValues;

                                        presetsWorkingCopy.Remove(newPreset.guid);
                                        presetsWorkingCopy.Add(newPreset.guid, newPreset);
                                        if (newPreset.modifiedUtc > existingPreset.modifiedUtc) //Updating
                                            numberOfUpdatedCustomizedPresets++;
                                        else
                                            numberOfReinstalledCustomizedPresets++;
                                    }
                                }
                                else if (newPreset.removePreset && newPreset.modifiedUtc > SavedSettings.lastAsrImportDateUtc)
                                {
                                    if (askToRemovePresets && newPreset.removePreset)
                                    {
                                        if (MessageBox.Show(this, MsgDoYouWantToResetYourCustomizedPredefinedPresets, "", MessageBoxButtons.YesNo, 
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
                                        deletePreset(existingPreset);
                                        numberOfRemovedPresets++;
                                    }
                                }
                                else if (newPreset.modifiedUtc > existingPreset.modifiedUtc) //Install only new presets
                                {
                                    if (!anyCustomization)
                                    {
                                        newPreset.copyExtendedCustomizationsFrom(existingPreset);

                                        presetsWorkingCopy.Remove(newPreset.guid);
                                        presetsWorkingCopy.Add(newPreset.guid, newPreset);
                                        numberOfUpdatedPresets++;
                                    }
                                    else //Update preset to latest version, and copy *all* customizations
                                    {
                                        newPreset.copyAdvancedCustomizationsFrom(existingPreset);

                                        presetsWorkingCopy.Remove(newPreset.guid);
                                        presetsWorkingCopy.Add(newPreset.guid, newPreset);
                                        numberOfUpdatedCustomizedPresets++;
                                    }
                                }
                                else //if (!importAll && (newPreset.modifiedUtc <= existingPreset.modifiedUtc || newPreset.modifiedUtc <= Plugin.SavedSettings.lastAsrImportDateUtc))
                                {
                                    numberOfNotChangedSkippedPresets++;
                                }
                            }
                            else if (!newPreset.removePreset)
                            {
                                if (installAll || newPreset.modifiedUtc > SavedSettings.lastAsrImportDateUtc)
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
                            MSR = newPreset;
                        }
                    }
                    catch
                    {
                        numberOfErrors++;
                    }
                }
            }

            if (numberOfInstalledPresets > 0)
                SavedSettings.lastAsrImportDateUtc = DateTime.UtcNow;


            refreshPresetList(selectedPresetGuid);


            string message = "";
            if (numberOfInstalledPresets > 0)
                message += AddLeadingSpaces(numberOfInstalledPresets, 4) + GetPluralForm(MsgPresetsWereInstalled, numberOfInstalledPresets);
            if (numberOfReinstalledCustomizedPresets > 0)
                message += AddLeadingSpaces(numberOfReinstalledCustomizedPresets, 4) + GetPluralForm(MsgPresetsCustomizedWereReinstalled, numberOfReinstalledCustomizedPresets);
            if (numberOfReinstalledPresets > 0)
                message += AddLeadingSpaces(numberOfReinstalledPresets, 4) + GetPluralForm(MsgPresetsWereReinstalled, numberOfReinstalledPresets);
            if (numberOfUpdatedPresets > 0)
                message += AddLeadingSpaces(numberOfUpdatedPresets, 4) + GetPluralForm(MsgPresetsWereUpdated, numberOfUpdatedPresets);
            if (numberOfUpdatedCustomizedPresets > 0)
                message += AddLeadingSpaces(numberOfUpdatedCustomizedPresets, 4) + GetPluralForm(MsgPresetsCustomizedWereUpdated, numberOfUpdatedCustomizedPresets);
            if (numberOfNotChangedSkippedPresets > 0)
                message += AddLeadingSpaces(numberOfNotChangedSkippedPresets, 4) + GetPluralForm(MsgPresetsWereNotChanged, numberOfNotChangedSkippedPresets);
            if (numberOfRemovedPresets > 0)
                message += AddLeadingSpaces(numberOfRemovedPresets, 4) + GetPluralForm(MsgPresetsRemoved, numberOfRemovedPresets);
            if (numberOfErrors > 0)
                message += AddLeadingSpaces(numberOfErrors, 4) + GetPluralForm(MsgPresetsFailedToUpdate, numberOfErrors);

            if (message == "")
            {
                message = MsgPresetsNotFound;
            }
            else
            {
                presetsChanged = true;
                buttonClose.Image = Warning;
                toolTip1.SetToolTip(buttonClose, buttonCloseToolTip);
            }


            MessageBox.Show(this, message, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void deleteAll()
        {
            int numberOfDeletedPresets = 0;
            int numberOfErrors = 0;

            DialogResult result = MessageBox.Show(this, MsgDeletingConfirmation, "", MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No)
                return;

            Guid selectedPresetGuid = Guid.Empty;
            if (preset != null)
                selectedPresetGuid = preset.guid;

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
            showPlaylistLimitedChecked = false;
            showFunctionIdAssignedChecked = false;
            showHotkeyAssignedChecked = false;
            showTickedOnlyChecked = false;

            presetList.Items.Clear();
            presetList.Sorted = false;
            ignoreCheckedPresetEvent = false;
            autoAppliedPresetCount = 0;
            foreach (Preset tempPreset in presetsWorkingCopy.Values)
                presetList.Items.Add(tempPreset, autoAppliedAsrPresetGuids.TryGetValue(tempPreset.guid, out _));

            refreshPresetList(selectedPresetGuid);

            showAutoapplyingWarningIfRequired();

            ignoreCheckedPresetEvent = true;
            presetList.Sorted = true;


            string message = "";

            if (numberOfDeletedPresets == 0 && numberOfErrors == 0)
                message = MsgNoPresetsDeleted;
            else
                message += numberOfDeletedPresets + GetPluralForm(MsgPresetsWereDeleted, numberOfDeletedPresets);

            if (numberOfErrors > 0)
                message += "\n" + numberOfErrors + GetPluralForm(MsgFailedToDelete, numberOfErrors);


            MessageBox.Show(this, message, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonInstallNew_Click(object sender, EventArgs e)
        {
            install(false);
        }

        private void buttonInstallAll_Click(object sender, EventArgs e)
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

        private void flashAllTagsWarning(object state)
        {
            if (AllTagsPresetsCantBeAutoappliedСountdown == 0)
            {
                allTagsWarningTimer.Change(-1, -1);
                MbForm.Invoke(new Action(() => { showAutoapplyingWarningIfRequired(); }));
                return;
            }
            else if (AllTagsPresetsCantBeAutoappliedСountdown == 1)
            {
                AllTagsPresetsCantBeAutoappliedСountdown--;
                allTagsWarningTimer.Change(10000, -1);
                MbForm.Invoke(new Action(() => { autoApplyPresetslabel.ForeColor = TickedColor; }));
                return;
            }


            if (AllTagsPresetsCantBeAutoappliedСountdown % 2 == 0)
            {
                MbForm.Invoke(new Action(() => { autoApplyPresetslabel.ForeColor = TickedColor; }));
            }
            else
            {
                MbForm.Invoke(new Action(() => { autoApplyPresetslabel.ForeColor = UntickedColor; }));
            }

            AllTagsPresetsCantBeAutoappliedСountdown--;
        }

        private void showAllTagsWarning(bool showGeneralWarning, List<Label> allTagslabels = null, List<Label> genericLabels = null)
        {
            if (allTagsReplaceIdsCount > 0 && tableLayoutPanel3.ColumnStyles[0].SizeType == SizeType.Absolute)
            {
                tableLayoutPanel3.ColumnStyles[0].SizeType = SizeType.AutoSize;
                tableLayoutPanel3.ColumnStyles[0].Width = tableLayoutPanel3ColWidth0;
                tableLayoutPanel3.ColumnStyles[1].SizeType = SizeType.Percent;
                tableLayoutPanel3.ColumnStyles[1].Width = tableLayoutPanel3ColWidth1;
                tableLayoutPanel3.ColumnStyles[2].SizeType = SizeType.Absolute;
                tableLayoutPanel3.ColumnStyles[2].Width = tableLayoutPanel3ColWidth2;

                buttonProcessPreserveTags.Visible = true;
                processPreserveTagsTextBox.Visible = true;
                buttonSelectPreservedTags.Visible = true;
            }
            else if (allTagsReplaceIdsCount == 0 && tableLayoutPanel3.ColumnStyles[0].SizeType != SizeType.Absolute)
            {
                buttonProcessPreserveTags.Visible = false;
                processPreserveTagsTextBox.Visible = false;
                buttonSelectPreservedTags.Visible = false;

                tableLayoutPanel3.ColumnStyles[0].SizeType = SizeType.Absolute;
                tableLayoutPanel3.ColumnStyles[0].Width = 0;
                tableLayoutPanel3.ColumnStyles[1].SizeType = SizeType.Absolute;
                tableLayoutPanel3.ColumnStyles[1].Width = 0;
                tableLayoutPanel3.ColumnStyles[2].SizeType = SizeType.Absolute;
                tableLayoutPanel3.ColumnStyles[2].Width = 0;
            }

            if (allTagslabels != null)
            {
                foreach (var label in allTagslabels)
                {
                    label.Image = Resources.warning_12b;
                    toolTip1.SetToolTip(label, AllTagsTagIsSelectedToolTip);
                }
            }

            if (genericLabels != null)
            {
                foreach (var label in genericLabels)
                {
                    label.Image = Resources.transparent_15;
                    toolTip1.SetToolTip(label, "");
                }
            }


            if (showGeneralWarning)
            {
                if (processPresetChanges)
                {
                    System.Media.SystemSounds.Exclamation.Play();

                    AllTagsPresetsCantBeAutoappliedСountdown = 20;
                    autoApplyPresetslabel.Text = AllTagsPresetsCantBeAutoappliedText.ToUpper();
                    toolTip1.SetToolTip(autoApplyPresetslabel, AllTagsPresetsCantBeAutoappliedText.ToUpper());

                    if (allTagsWarningTimer == null)
                        allTagsWarningTimer = new System.Threading.Timer(flashAllTagsWarning, null, 0, 250);
                    else
                        allTagsWarningTimer.Change(0, 250);
                }
            }
            else
            {
                if (allTagsWarningTimer != null)
                    allTagsWarningTimer.Change(-1, -1);

                showAutoapplyingWarningIfRequired();
            }
        }

        private bool showAllTagsWarningIfRequired(Preset paramPreset, bool showGeneralWarning)
        {
            if (paramPreset == null)
            {
                selectedPresetUsesAllTags = false;
                showAllTagsWarning(false, null, null);
                
                return false;
            }


            List<Label> allTagsLabels = new List<Label>();
            List<Label> genericLabels = new List<Label>();

            if (paramPreset.detectAllTagsTagId(paramPreset.parameterTagId))
                allTagsLabels.Add(labelTag);
            else
                genericLabels.Add(labelTag);

            if (paramPreset.detectAllTagsTagId(paramPreset.parameterTag2Id))
                allTagsLabels.Add(labelTag2);
            else
                genericLabels.Add(labelTag2);

            if (paramPreset.detectAllTagsTagId(paramPreset.parameterTag3Id))
                allTagsLabels.Add(labelTag3);
            else
                genericLabels.Add(labelTag3);

            if (paramPreset.detectAllTagsTagId(paramPreset.parameterTag4Id))
                allTagsLabels.Add(labelTag4);
            else
                genericLabels.Add(labelTag4);

            if (paramPreset.detectAllTagsTagId(paramPreset.parameterTag5Id))
                allTagsLabels.Add(labelTag5);
            else
                genericLabels.Add(labelTag5);

            if (paramPreset.detectAllTagsTagId(paramPreset.parameterTag6Id))
                allTagsLabels.Add(labelTag6);
            else
                genericLabels.Add(labelTag6);

            if (allTagsLabels.Count > 0)
            {
                selectedPresetUsesAllTags = true;
                assignHotkeyCheckBox.Checked = false;
                assignHotkeyCheckBox.Enable(false);
                buttonOK.Enable(previewIsGenerated);

                if (autoAppliedAsrPresetGuids.TryGetValue(paramPreset.guid, out _))
                {
                    int presetIndex = presetList.Items.IndexOf(paramPreset);
                    ignoreCheckedPresetEvent = false;
                    presetList.SetItemChecked(presetIndex, false);
                    ignoreCheckedPresetEvent = true;
                }

                showAllTagsWarning(showGeneralWarning, allTagsLabels, genericLabels);

                CellTooTip = CtlAsrAllTagsCellTooTip;
                return true;
            }
            else
            {
                selectedPresetUsesAllTags = false;
                assignHotkeyCheckBox.Enable(true);
                buttonOK.Enable(true);

                showAllTagsWarning(false, null, genericLabels);

                CellTooTip = CtlAsrCellTooTip;

                return false;
            }
        }

        private void parameterTag_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            preset.parameterTagId = AsrGetTagId(parameterTagList.Text);
            allTagsReplaceIdsCount = preset.countAllTagsReplaceTagIds();
            showAllTagsWarningIfRequired(preset, autoAppliedAsrPresetGuids.TryGetValue(preset.guid, out _));
            nameColumns();
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void parameterTag2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            preset.parameterTag2Id = AsrGetTagId(parameterTag2List.Text);
            allTagsReplaceIdsCount = preset.countAllTagsReplaceTagIds();
            showAllTagsWarningIfRequired(preset, autoAppliedAsrPresetGuids.TryGetValue(preset.guid, out _));
            nameColumns();
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void parameterTag3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            preset.parameterTag3Id = AsrGetTagId(parameterTag3List.Text);
            allTagsReplaceIdsCount = preset.countAllTagsReplaceTagIds();
            showAllTagsWarningIfRequired(preset, autoAppliedAsrPresetGuids.TryGetValue(preset.guid, out _));
            nameColumns();
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void parameterTag4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            preset.parameterTag4Id = AsrGetTagId(parameterTag4List.Text);
            allTagsReplaceIdsCount = preset.countAllTagsReplaceTagIds();
            showAllTagsWarningIfRequired(preset, autoAppliedAsrPresetGuids.TryGetValue(preset.guid, out _));
            nameColumns();
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void parameterTag5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            preset.parameterTag5Id = AsrGetTagId(parameterTag5List.Text);
            allTagsReplaceIdsCount = preset.countAllTagsReplaceTagIds();
            showAllTagsWarningIfRequired(preset, autoAppliedAsrPresetGuids.TryGetValue(preset.guid, out _));
            nameColumns();
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void parameterTag6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            preset.parameterTag6Id = AsrGetTagId(parameterTag6List.Text);
            allTagsReplaceIdsCount = preset.countAllTagsReplaceTagIds();
            showAllTagsWarningIfRequired(preset, autoAppliedAsrPresetGuids.TryGetValue(preset.guid, out _));
            nameColumns();
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void customTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            preset.customText = customTextBox.Text;
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void customText2Box_TextChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            preset.customText2 = customText2Box.Text;
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void customText3Box_TextChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            preset.customText3 = customText3Box.Text;
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void customText4Box_TextChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            preset.customText4 = customText4Box.Text;
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void preserveTagValuesTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            preset.preserveValues = preserveTagValuesTextBox.Text;
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void processPreserveTagsTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            if (processTagsMode)
                preset.processTags = processPreserveTagsTextBox.Text;
            else
                preset.preserveTags = processPreserveTagsTextBox.Text;

            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void buttonProcessPreserveTags_Click(object sender, EventArgs e)
        {
            processTagsMode = !processTagsMode;

            if (processTagsMode)
            {
                buttonProcessPreserveTags.Text = AsrProcessTagsButtonName;
                processPreserveTagsTextBox.Text = preset.processTags;
            }
            else
            {
                buttonProcessPreserveTags.Text = AsrPreserveTagsButtonName;
                processPreserveTagsTextBox.Text = preset.preserveTags;
            }

            preset.processTagsMode = processTagsMode;
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void buttonSelectPreservedTags_Click(object sender, EventArgs e)
        {
            string[] selectedTags = processPreserveTagsTextBox.Text.Split(new string[] { ";;" }, StringSplitOptions.RemoveEmptyEntries);
            selectedTags = CopyTagsToClipboardCommand.SelectTags(TagToolsPlugin, SelectTagsWindowTitle, SelectButtonName, selectedTags, false);

            string preserveTags = "";
            if (selectedTags.Length > 0)
            {
                foreach (var tagName in selectedTags)
                {
                    preserveTags += tagName + ";;";
                }
                preserveTags = preserveTags.Trim(';');
            }

            processPreserveTagsTextBox.Text = preserveTags;
        }

        private void conditionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;


            if (conditionCheckBox.Checked && playlistComboBox.SelectedIndex == -1 && playlistComboBox.Items.Count > 0)
            {
                playlistComboBox.SelectedIndex = 0;
            }
            else if (conditionCheckBox.Checked && playlistComboBox.Items.Count == 0)
            {
                conditionCheckBox.Checked = false;
            }

            playlistComboBox.Enable(conditionCheckBox.Checked);

            if (!processPresetChanges)
                return;

            preset.condition = playlistComboBox.Enabled;
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void conditionCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!conditionCheckBox.Enabled)
                return;

            conditionCheckBox.Checked = !conditionCheckBox.Checked;
            conditionCheckBox_CheckedChanged(null, null);
        }

        private void playlistComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

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
                if ((string)row.Cells[0].Value == null)
                    continue;

                if (state)
                    row.Cells[0].Value = "F";
                else
                    row.Cells[0].Value = "T";

                DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(0, row.Index);
                previewTable_CellContentClick(null, e);
            }
        }

        private void toggleRow(bool check, int rowIndex)
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

            if (!check)
            {
                previewTable.Rows[rowIndex].Cells[0].Value = "F";

                sourceTag1Value = (string)previewTable.Rows[rowIndex].Cells[5].Value;
                sourceTag2Value = (string)previewTable.Rows[rowIndex].Cells[8].Value;
                sourceTag3Value = (string)previewTable.Rows[rowIndex].Cells[11].Value;
                sourceTag4Value = (string)previewTable.Rows[rowIndex].Cells[14].Value;
                sourceTag5Value = (string)previewTable.Rows[rowIndex].Cells[17].Value;

                previewTable.Rows[rowIndex].Cells[6].Value = sourceTag1Value;
                previewTable.Rows[rowIndex].Cells[9].Value = sourceTag2Value;
                previewTable.Rows[rowIndex].Cells[12].Value = sourceTag3Value;
                previewTable.Rows[rowIndex].Cells[15].Value = sourceTag4Value;
                previewTable.Rows[rowIndex].Cells[18].Value = sourceTag5Value;
            }
            else //if (check)
            {
                previewTable.Rows[rowIndex].Cells[0].Value = "T";

                lock (Presets)
                {
                    Preset combinationPreset = presetProcessingCopies[(string)previewTable.Rows[rowIndex].Cells[1].Value];
                    string curentFile = (string)previewTable.Rows[rowIndex].Cells[2].Value;
                    var searchedAndReplacedTags = GetReplacedTags(curentFile, ref combinationPreset, this, false);

                    newTag1Value = searchedAndReplacedTags.replacedTagValue;
                    newTag2Value = searchedAndReplacedTags.replacedTag2Value;
                    newTag3Value = searchedAndReplacedTags.replacedTag3Value;
                    newTag4Value = searchedAndReplacedTags.replacedTag4Value;
                    newTag5Value = searchedAndReplacedTags.replacedTag5Value;
                }

                previewTable.Rows[rowIndex].Cells[6].Value = newTag1Value;
                previewTable.Rows[rowIndex].Cells[9].Value = newTag2Value;
                previewTable.Rows[rowIndex].Cells[12].Value = newTag3Value;
                previewTable.Rows[rowIndex].Cells[15].Value = newTag4Value;
                previewTable.Rows[rowIndex].Cells[18].Value = newTag5Value;
            }
        }

        private void previewTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                bool sameTags = false;
                bool addRemoveTagsToProcessedPreserved = false;

                if (selectedPresetUsesAllTags && Form.ModifierKeys == Keys.Shift)
                {
                    sameTags = true;
                }
                if (selectedPresetUsesAllTags && Form.ModifierKeys == Keys.Control)
                {
                    sameTags = true;
                    addRemoveTagsToProcessedPreserved = true;
                }


                string isChecked = (string)previewTable.Rows[e.RowIndex].Cells[0].Value;

                bool check = true;
                bool indeterminate = false;

                if (isChecked == "T")
                    check = false;
                else if (isChecked == "F")
                    check = true;
                else
                    indeterminate = true;

                if (indeterminate)
                    return;


                if (!sameTags) // Toggle checked state of single row
                {
                    toggleRow(check, e.RowIndex);

                    previewTableFormatRow(e.RowIndex);
                }
                else // Toggle checked state of all rows for the same tag(s) - 1 row can refer to up to 5 tags
                {
                    string[] invokedTags = new string[5];

                    for (int i = 0; i < 5; i++)
                    {
                        invokedTags[i] = (string)previewTable.Rows[e.RowIndex].Cells[4 + i * 3].Value;
                        if (invokedTags[i] == null)
                            continue;

                        if (addRemoveTagsToProcessedPreserved && processTagsMode)
                        {
                            if (string.IsNullOrEmpty(preset.processTags))
                                preset.processTags = "";

                            if (check && !(";;" + preset.processTags + ";;").Contains(";;" + invokedTags[i] + ";;"))
                                preset.processTags += ";;" + invokedTags[i];
                            else if (!check && (";;" + preset.processTags + ";;").Contains(";;" + invokedTags[i] + ";;"))
                                preset.processTags = preset.processTags.Replace(invokedTags[i], "").Replace(";;;;", ";;");
                        }
                        else if (addRemoveTagsToProcessedPreserved && !processTagsMode)
                        {
                            if (string.IsNullOrEmpty(preset.preserveTags))
                                preset.preserveTags = "";

                            if (!check && !(";;" + preset.preserveTags + ";;").Contains(";;" + invokedTags[i] + ";;"))
                                preset.preserveTags += ";;" + invokedTags[i];
                            else if (check && (";;" + preset.preserveTags + ";;").Contains(";;" + invokedTags[i] + ";;"))
                                preset.preserveTags = preset.preserveTags.Replace(invokedTags[i], "").Replace(";;;;", ";;");
                        }
                    }

                    if (addRemoveTagsToProcessedPreserved && processTagsMode)
                    {
                        preset.processTags = preset.processTags.Replace(";;;;", ";;");
                        preset.processTags = preset.processTags.Trim(';');
                        processPreserveTagsTextBox.Text = preset.processTags;
                    }
                    else if (addRemoveTagsToProcessedPreserved && !processTagsMode)
                    {
                        preset.preserveTags = preset.preserveTags.Replace(";;;;", ";;");
                        preset.preserveTags = preset.preserveTags.Trim(';');
                        processPreserveTagsTextBox.Text = preset.preserveTags;
                    }


                    for (int j = 0; j < previewTable.RowCount; j++)
                    {
                        bool processRow = true;

                        for (int i = 0; i < 5; i++)
                        {
                            if ((string)previewTable.Rows[j].Cells[4 + i * 3].Value != invokedTags[i])
                            {
                                processRow = false;
                                break;
                            }
                        }

                        if (processRow)
                        {
                            toggleRow(check, j);

                            previewTableFormatRow(j);
                        }
                    }
                }
            }
        }

        private void previewTableFormatRow(int rowIndex)
        {
            previewTable.Rows[rowIndex].Cells[0].ToolTipText = CellTooTip;

            if (SavedSettings.dontHighlightChangedTags)
                return;

            for (int columnIndex = 1; columnIndex < previewTable.ColumnCount; columnIndex++)
            {
                if (previewTable.Rows[rowIndex].Cells[columnIndex].Visible)
                {
                    if ((string)previewTable.Rows[rowIndex].Cells[0].Value == "T")
                        previewTable.Rows[rowIndex].Cells[columnIndex].Style = UnchangedCellStyle;
                    else
                        previewTable.Rows[rowIndex].Cells[columnIndex].Style = DimmedCellStyle;
                }
            }


            for (int columnIndex = 1; columnIndex < previewTable.ColumnCount; columnIndex++)
            {
                if (columnIndex == 6 && previewTable.Columns[columnIndex].Visible)
                { 
                    var cell = previewTable.Rows[rowIndex].Cells[6];
                    ChangesDetectionResult changeType = (ChangesDetectionResult)cell.Tag;

                    if (changeType == ChangesDetectionResult.PreservedTagsDetected)
                        cell.Style = PreservedTagCellStyle;
                    else if (changeType == ChangesDetectionResult.PreservedTagValuesDetected)
                        cell.Style = PreservedTagValueCellStyle;
                    else if (changeType == ChangesDetectionResult.NoChangesDetected)
                        cell.Style = UnchangedCellStyle;
                    else if (changeType == ChangesDetectionResult.NoExclusionsDetected)
                        cell.Style = ChangedCellStyle;
                } 
                else if (columnIndex == 9 && previewTable.Columns[columnIndex].Visible)
                {
                    var cell = previewTable.Rows[rowIndex].Cells[9];

                    if ((ChangesDetectionResult)cell.Tag == ChangesDetectionResult.PreservedTagsDetected)
                    {
                        cell.Style = PreservedTagCellStyle;
                        return;
                    }
                    else if ((ChangesDetectionResult)cell.Tag == ChangesDetectionResult.PreservedTagValuesDetected)
                    {
                        cell.Style = PreservedTagValueCellStyle;
                        return;
                    }


                    if (previewTable.Columns[8].Visible)
                    {
                        if ((string)cell.Value == (string)previewTable.Rows[rowIndex].Cells[8].Value)
                            cell.Style = UnchangedCellStyle;
                        else
                            cell.Style = ChangedCellStyle;

                        return;
                    }

                    int i = 5;
                    do
                    {
                        if (previewTable.Columns[i].Visible)
                            break;

                        i -= 3;
                    }
                    while (i >= 5);

                    if ((ChangesDetectionResult)previewTable.Rows[rowIndex].Cells[i + 1].Tag == ChangesDetectionResult.PreservedTagsDetected)
                        cell.Style = PreservedTagCellStyle;
                    else if ((ChangesDetectionResult)previewTable.Rows[rowIndex].Cells[i + 1].Tag == ChangesDetectionResult.PreservedTagValuesDetected)
                        cell.Style = PreservedTagValueCellStyle;
                    else if ((string)cell.Value == (string)previewTable.Rows[rowIndex].Cells[i].Value)
                        cell.Style = UnchangedCellStyle;
                    else
                        cell.Style = ChangedCellStyle;
                }
                else if (columnIndex == 12 && previewTable.Columns[columnIndex].Visible)
                {
                    var cell = previewTable.Rows[rowIndex].Cells[12];

                    if ((ChangesDetectionResult)cell.Tag == ChangesDetectionResult.PreservedTagsDetected)
                    {
                        cell.Style = PreservedTagCellStyle;
                        return;
                    }
                    else if ((ChangesDetectionResult)cell.Tag == ChangesDetectionResult.PreservedTagValuesDetected)
                    {
                        cell.Style = PreservedTagValueCellStyle;
                        return;
                    }

                    if (previewTable.Columns[11].Visible)
                    {
                        if ((string)cell.Value == (string)previewTable.Rows[rowIndex].Cells[11].Value)
                            cell.Style = UnchangedCellStyle;
                        else
                            cell.Style = ChangedCellStyle;

                        return;
                    }

                    int i = 8;
                    do
                    {
                        if (previewTable.Columns[i].Visible)
                            break;

                        i -= 3;
                    }
                    while (i >= 5);

                    if ((ChangesDetectionResult)previewTable.Rows[rowIndex].Cells[i + 1].Tag == ChangesDetectionResult.PreservedTagsDetected)
                        cell.Style = PreservedTagCellStyle;
                    else if ((ChangesDetectionResult)previewTable.Rows[rowIndex].Cells[i + 1].Tag == ChangesDetectionResult.PreservedTagValuesDetected)
                        cell.Style = PreservedTagValueCellStyle;
                    else if ((string)cell.Value == (string)previewTable.Rows[rowIndex].Cells[i].Value)
                        cell.Style = UnchangedCellStyle;
                    else
                        cell.Style = ChangedCellStyle;
                }
                else if (columnIndex == 15 && previewTable.Columns[columnIndex].Visible)
                {
                    var cell = previewTable.Rows[rowIndex].Cells[15];
                    ChangesDetectionResult changeType = (ChangesDetectionResult)cell.Tag;

                    if ((ChangesDetectionResult)cell.Tag == ChangesDetectionResult.PreservedTagsDetected)
                    {
                        cell.Style = PreservedTagCellStyle;
                        return;
                    }
                    else if ((ChangesDetectionResult)cell.Tag == ChangesDetectionResult.PreservedTagValuesDetected)
                    {
                        cell.Style = PreservedTagValueCellStyle;
                        return;
                    }

                    if (previewTable.Columns[11].Visible)
                    {
                        if ((string)cell.Value == (string)previewTable.Rows[rowIndex].Cells[14].Value)
                            cell.Style = UnchangedCellStyle;
                        else
                            cell.Style = ChangedCellStyle;

                        return;
                    }

                    int i = 11;
                    do
                    {
                        if (previewTable.Columns[i].Visible)
                            break;

                        i -= 3;
                    }
                    while (i >= 5);

                    if ((ChangesDetectionResult)previewTable.Rows[rowIndex].Cells[i + 1].Tag == ChangesDetectionResult.PreservedTagsDetected)
                        cell.Style = PreservedTagCellStyle;
                    else if ((ChangesDetectionResult)previewTable.Rows[rowIndex].Cells[i + 1].Tag == ChangesDetectionResult.PreservedTagValuesDetected)
                        cell.Style = PreservedTagValueCellStyle;
                    else if ((string)cell.Value == (string)previewTable.Rows[rowIndex].Cells[i].Value)
                        cell.Style = UnchangedCellStyle;
                    else
                        cell.Style = ChangedCellStyle;
                }
                else if (columnIndex == 18 && previewTable.Columns[columnIndex].Visible)
                {
                    var cell = previewTable.Rows[rowIndex].Cells[18];

                    if ((ChangesDetectionResult)cell.Tag == ChangesDetectionResult.PreservedTagsDetected)
                    {
                        cell.Style = PreservedTagCellStyle;
                        return;
                    }
                    else if ((ChangesDetectionResult)cell.Tag == ChangesDetectionResult.PreservedTagValuesDetected)
                    {
                        cell.Style = PreservedTagValueCellStyle;
                        return;
                    }

                    if (previewTable.Columns[11].Visible)
                    {
                        if ((string)cell.Value == (string)previewTable.Rows[rowIndex].Cells[17].Value)
                            cell.Style = UnchangedCellStyle;
                        else
                            cell.Style = ChangedCellStyle;

                        return;
                    }

                    int i = 14;
                    do
                    {
                        if (previewTable.Columns[i].Visible)
                            break;

                        i -= 3;
                    }
                    while (i >= 5);

                    if ((ChangesDetectionResult)previewTable.Rows[rowIndex].Cells[i + 1].Tag == ChangesDetectionResult.PreservedTagsDetected)
                        cell.Style = PreservedTagCellStyle;
                    else if ((ChangesDetectionResult)previewTable.Rows[rowIndex].Cells[i + 1].Tag == ChangesDetectionResult.PreservedTagValuesDetected)
                        cell.Style = PreservedTagValueCellStyle;
                    else if ((string)cell.Value == (string)previewTable.Rows[rowIndex].Cells[i].Value)
                        cell.Style = UnchangedCellStyle;
                    else
                        cell.Style = ChangedCellStyle;
                }
            }
        }

        public override void enableDisablePreviewOptionControls(bool enable)
        {
            if (enable && previewIsGenerated)
                return;

            presetList.Enable(enable);

            if (enable)
            {
                presetList_SelectedIndexChanged(null, null);
            }
            else
            {
                parameterTagList.Enable(enable);
                parameterTag2List.Enable(enable);
                parameterTag3List.Enable(enable);
                parameterTag4List.Enable(enable);
                parameterTag5List.Enable(enable);
                parameterTag6List.Enable(enable);

                customTextBox.Enable(enable);
                customText2Box.Enable(enable);
                customText3Box.Enable(enable);
                customText4Box.Enable(enable);

                processPreserveTagsTextBox.Enable(true);
                processPreserveTagsTextBox.ReadOnly = !enable;
                preserveTagValuesTextBox.Enable(true);
            }

            buttonExport.Enable(enable && editButtonEnabled);
            buttonImportNew.Enable(enable);
            buttonImportAll.Enable(enable);
            buttonImport.Enable(enable);
            buttonExportCustom.Enable(enable);

            buttonCreate.Enable(enable);
            buttonCopy.Enable(enable);
            buttonEdit.Enable(enable && editButtonEnabled);
            buttonDelete.Enable(enable);
            buttonDeleteAll.Enable(enable);
        }

        public override void enableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonPreview, " ");
            dirtyErrorProvider.SetError(buttonPreview, string.Empty);
        }

        public override void disableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonPreview, getBackgroundTasksWarning());
        }

        public override void enableQueryingOrUpdatingButtons()
        {
            buttonOK.Enable(true);
            buttonPreview.Enable(true);
        }

        public override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enable(false);
            buttonPreview.Enable(false);
        }

        private void filterPresetList()
        {
            string[] searchStrings = searchTextBox.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            presetList.Items.Clear();
            presetList.Sorted = false;
            ignoreCheckedPresetEvent = false;

            setCheckedPicturesStates();

            foreach (Preset tempPreset in presetsWorkingCopy.Values)
            {
                bool filteringCriteriaAreMeet = true;


                if (showHotkeyAssignedChecked && !tempPreset.hotkeyAssigned)
                {
                    filteringCriteriaAreMeet = false;
                }
                else if (showFunctionIdAssignedChecked && string.IsNullOrEmpty(tempPreset.id))
                {
                    filteringCriteriaAreMeet = false;
                }
                else if (showPlaylistLimitedChecked && !tempPreset.condition)
                {
                    filteringCriteriaAreMeet = false;
                }
                else if (showUserChecked && !tempPreset.userPreset)
                {
                    filteringCriteriaAreMeet = false;
                }
                else if (showCustomizedChecked && !tempPreset.getCustomizationsFlag())
                {
                    filteringCriteriaAreMeet = false;
                }
                else if (showPredefinedChecked && tempPreset.userPreset)
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
                    bool autoApply = autoAppliedAsrPresetGuids.TryGetValue(tempPreset.guid, out _);
                    if (!showTickedOnlyChecked || autoApply)
                    {
                        presetList.Items.Add(tempPreset, autoApply);
                        if (autoApply)
                            autoAppliedPresetCount--;
                    }
                }
            }
            //showAutoapplyingWarningIfRequired();//***
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
            if (!processPresetChanges)
                return;


            if (assignHotkeyCheckBox.Checked)
            {
                int slot = FindFirstSlot(asrPresetsWithHotkeysGuids, Guid.Empty);
                asrPresetsWithHotkeysGuids[slot] = preset.guid;
                preset.hotkeyAssigned = true;

                asrPresetsWithHotkeysCount++;

                applyToPlayingTrackCheckBox.Enable(true);
            }
            else
            {
                int slot = FindFirstSlot(asrPresetsWithHotkeysGuids, preset.guid);
                asrPresetsWithHotkeysGuids[slot] = Guid.Empty;
                preset.hotkeyAssigned = false;

                asrPresetsWithHotkeysCount--;

                applyToPlayingTrackCheckBox.Enable(false);
                applyToPlayingTrackCheckBox.Checked = false;
            }

            preset.setCustomizationsFlag(this, backupedPreset);

            assignHotkeyCheckBoxLabel.Text = assignHotkeyCheckBoxText + (MaximumNumberOfASRHotkeys - asrPresetsWithHotkeysCount) + "/" + MaximumNumberOfASRHotkeys;
        }

        private void assignHotkeyCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!assignHotkeyCheckBox.Enabled)
                return;

            assignHotkeyCheckBox.Checked = !assignHotkeyCheckBox.Checked;
            assignHotkeyCheckBox_CheckedChanged(null, null);
        }

        private void showAutoapplyingWarningIfRequired()
        {
            if (autoAppliedPresetCount == 0)
            {
                autoApplyPresetslabel.Text = EditApplyText + AutoApplyText;

                toolTip1.SetToolTip(autoApplyPresetslabel, EditApplyText + "\n" + AutoApplyText + "\n\n"
                    + NowTickedText.ToUpper().Replace("%%TICKEDPRESETS%%", autoAppliedPresetCount.ToString()));

                autoApplyPresetslabel.ForeColor = UntickedColor;
                //tickedOnlyChecked = false;
                //tickedOnlyCheckBox.Enable(false);
            }
            else
            {
                autoApplyPresetslabel.Text = EditApplyText + AutoApplyText + "\n"
                    + NowTickedText.ToUpper().Replace("%%TICKEDPRESETS%%", autoAppliedPresetCount.ToString());

                toolTip1.SetToolTip(autoApplyPresetslabel, EditApplyText + "\n" + AutoApplyText + "\n\n"
                    + ClickHereText.ToUpper() + "\n" + NowTickedText.ToUpper().Replace("%%TICKEDPRESETS%%", autoAppliedPresetCount.ToString()));

                autoApplyPresetslabel.ForeColor = TickedColor;
                //tickedOnlyCheckBox.Enable(true);
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


            Preset checkedChangedPreset = (Preset)presetList.Items[e.Index];

            if (e.NewValue == CheckState.Checked)
            {
                if (showAllTagsWarningIfRequired(checkedChangedPreset, true))
                {
                    e.NewValue = CheckState.Unchecked;
                    return;
                }

                autoAppliedPresetCount++;
                if (presetList.SelectedIndex != -1)
                {
                    autoAppliedAsrPresetGuids.Add(checkedChangedPreset.guid, false);
                    presetsChanged = true;
                    
                    if (!SavedSettings.dontPlayTickedAutoApplyingAsrLrPresetSound)
                        System.Media.SystemSounds.Exclamation.Play();
                }
            }
            else
            {
                autoAppliedPresetCount--;
                if (presetList.SelectedIndex != -1)
                {
                    autoAppliedAsrPresetGuids.Remove(checkedChangedPreset.guid);
                    presetsChanged = true;
                }
            }

            showAutoapplyingWarningIfRequired();
            setPresetsChanged();        
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
                MessageBox.Show(this, MsgNotAllowedSymbols, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

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
                        MessageBox.Show(this, MsgPresetExists.Replace("%%ID%%", idTextBox.Text), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

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
            if (!processPresetChanges)
                return;

            preset.applyToPlayingTrack = applyToPlayingTrackCheckBox.Checked;
            preset.setCustomizationsFlag(this, backupedPreset);
        }

        private void applyToPlayingTrackCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!applyToPlayingTrackCheckBox.Enabled)
                return;

            applyToPlayingTrackCheckBox.Checked = !applyToPlayingTrackCheckBox.Checked;
            applyToPlayingTrackCheckBox_CheckedChanged(null, null);
        }

        private void favoriteCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            preset.favorite = favoriteCheckBox.Checked;
            preset.setCustomizationsFlag(this, backupedPreset);
            refreshPresetList(preset.guid);
        }

        private void favoriteCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!favoriteCheckBox.Enabled)
                return;

            favoriteCheckBox.Checked = !favoriteCheckBox.Checked;
            favoriteCheckBox_CheckedChanged(null, null);
        }

        private void saveColumnWidths(Preset savedPreset)
        {
            if (savedPreset == null)
                return;

            savedPreset.columnWeights.Clear();
            for (int i = 3; i < previewTable.ColumnCount; i++)
            {
                savedPreset.columnWeights.Add(previewTable.Columns[i].FillWeight);
            }
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
                if (!SavedSettings.dontShowPredefinedPresetsCantBeChangedMessage && 
                    MessageBox.Show(this, MsgPredefinedPresetsCantBeChanged,
                    "", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    SavedSettings.dontShowPredefinedPresetsCantBeChangedMessage = true;
                };

                editPreset(preset, false, true);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            if (autoAppliedPresetCount > 0)
            {
                showTickedOnlyChecked = true;

                filterPresetList();
            }
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            PluginQuickSettings settings = new PluginQuickSettings(TagToolsPlugin);
            PluginWindowTemplate.Display(settings, true);
        }

        private void setCheckedPicturesStates()
        {
            int checkedCount = 0;
            int checkedFilter = 0;

            if (showHotkeyAssignedChecked)
            {
                hotkeyPictureBox.Image = HotkeyPresetsAccent;
                checkedCount++;
                checkedFilter = 7;
            }
            else
            {
                hotkeyPictureBox.Image = HotkeyPresetsDimmed;
            }

            if (showFunctionIdAssignedChecked)
            {
                functionIdPictureBox.Image = FunctionIdPresetsAccent;
                checkedCount++;
                checkedFilter = 6;
            }
            else
            {
                functionIdPictureBox.Image = FunctionIdPresetsDimmed;
            }

            if (showPlaylistLimitedChecked)
            {
                playlistPictureBox.Image = PlaylistPresetsAccent;
                checkedCount++;
                checkedFilter = 5;
            }
            else
            {
                playlistPictureBox.Image = PlaylistPresetsDimmed;
            }

            if (showUserChecked)
            {
                userPictureBox.Image = UserPresetsAccent;
                checkedCount++;
                checkedFilter = 4;
            }
            else
            {
                userPictureBox.Image = UserPresetsDimmed;
            }

            if (showCustomizedChecked)
            {
                customizedPictureBox.Image = CustomizedPresetsAccent;
                checkedCount++;
                checkedFilter = 3;
            }
            else
            {
                customizedPictureBox.Image = CustomizedPresetsDimmed;
            }

            if (showPredefinedChecked)
            {
                predefinedPictureBox.Image = PredefinedPresetsAccent;
                checkedCount++;
                checkedFilter = 2;
            }
            else
            {
                predefinedPictureBox.Image = PredefinedPresetsDimmed;
            }

            if (showTickedOnlyChecked)
            {
                tickedOnlyPictureBox.Image = AutoAppliedPresetsAccent;
                checkedCount++;
                checkedFilter = 1;
            }
            else
            {
                tickedOnlyPictureBox.Image = AutoAppliedPresetsDimmed;
            }


            IgnoreFilterComboBoxSelectedIndexChanged = true;

            if (checkedCount == 0)
            {
                untickAllChecked = false;
                filterComboBox.SelectedIndex = 0;
            }
            else if (checkedCount == 1)
            {
                untickAllChecked = true;
                filterComboBox.SelectedIndex = checkedFilter;
            }
            else //if (checkedCount > 1)
            {
                untickAllChecked = true;
                filterComboBox.SelectedIndex = -1;
            }

            IgnoreFilterComboBoxSelectedIndexChanged = false;


            if (untickAllChecked)
                uncheckAllFiltersPictureBox.Image = UncheckAllFiltersAccent;
            else
                uncheckAllFiltersPictureBox.Image = UncheckAllFiltersDimmed;

        }

        private void AdvancedSearchAndReplaceCommand_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (presetsChanged)
            {
                MessageBoxDefaultButton lastAnswer = SavedSettings.asrUnsavedChangesLastAnswer;
                DialogResult result = MessageBox.Show(this, MsgAsrDoYouWantToSaveChangesBeforeClosingTheWindow,
                    "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, lastAnswer);

                if (result == DialogResult.Yes)
                {
                    SavedSettings.asrUnsavedChangesLastAnswer = MessageBoxDefaultButton.Button1;
                    saveSettings();
                }
                else if (result == DialogResult.No)
                {
                    SavedSettings.asrUnsavedChangesLastAnswer = MessageBoxDefaultButton.Button2;
                }
                else if (result == DialogResult.Cancel)
                {
                    SavedSettings.asrUnsavedChangesLastAnswer = MessageBoxDefaultButton.Button3;
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void AdvancedSearchAndReplaceCommand_Load(object sender, EventArgs e)
        {
            (int, int, int, int, int, int, int) value = loadWindowLayout();

            if (value.Item4 != 0)
            {
                ignoreSplitterMovedEvent = true;
                splitContainer1.SplitterDistance = value.Item4;
            }

            ignoreSplitterMovedEvent = false;
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (!ignoreSplitterMovedEvent)
                saveWindowLayout(0, 0, 0, splitContainer1.SplitterDistance);
        }

        private void filterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IgnoreFilterComboBoxSelectedIndexChanged)
                return;


            if (ModifierKeys == Keys.Control && filterComboBox.SelectedIndex > 0)
            {
                switch (filterComboBox.SelectedIndex)
                {
                    case 1:
                        showTickedOnlyChecked = !showTickedOnlyChecked;

                        break;
                    case 2:
                        showPredefinedChecked = !showPredefinedChecked;

                        break;
                    case 3:
                        showCustomizedChecked = !showCustomizedChecked;

                        break;
                    case 4:
                        showUserChecked = !showUserChecked;

                        break;
                    case 5:
                        showPlaylistLimitedChecked = !showPlaylistLimitedChecked;

                        break;
                    case 6:
                        showFunctionIdAssignedChecked = !showFunctionIdAssignedChecked;

                        break;
                    case 7:
                        showHotkeyAssignedChecked = !showHotkeyAssignedChecked;

                        break;
                }
            }
            else if (filterComboBox.SelectedIndex >= 0)
            {
                showTickedOnlyChecked = false;
                showPredefinedChecked = false;
                showCustomizedChecked = false;
                showUserChecked = false;
                showPlaylistLimitedChecked = false;
                showFunctionIdAssignedChecked = false;
                showHotkeyAssignedChecked = false;

                switch (filterComboBox.SelectedIndex)
                {
                    case 1:
                        showTickedOnlyChecked = true;

                        break;
                    case 2:
                        showPredefinedChecked = true;

                        break;
                    case 3:
                        showCustomizedChecked = true;

                        break;
                    case 4:
                        showUserChecked = true;

                        break;
                    case 5:
                        showPlaylistLimitedChecked = true;

                        break;
                    case 6:
                        showFunctionIdAssignedChecked = true;

                        break;
                    case 7:
                        showHotkeyAssignedChecked = true;

                        break;
                }
            }


            filterPresetList();
        }

        private void tickedOnlyPictureBox_Click(object sender, EventArgs e)
        {
            showTickedOnlyChecked = !showTickedOnlyChecked;

            if (showTickedOnlyChecked && ModifierKeys != Keys.Control)
            {
                showPredefinedChecked = false;
                showCustomizedChecked = false;
                showUserChecked = false;
                showPlaylistLimitedChecked = false;
                showFunctionIdAssignedChecked = false;
                showHotkeyAssignedChecked = false;
            }

            filterPresetList();
        }

        private void predefinedPictureBox_Click(object sender, EventArgs e)
        {
            showPredefinedChecked = !showPredefinedChecked;

            if (showPredefinedChecked && ModifierKeys != Keys.Control)
            {
                showTickedOnlyChecked = false;
                showCustomizedChecked = false;
                showUserChecked = false;
                showPlaylistLimitedChecked = false;
                showFunctionIdAssignedChecked = false;
                showHotkeyAssignedChecked = false;
            }
            else if (showPredefinedChecked && showUserChecked)
            {
                showUserChecked = false;
            }

            filterPresetList();
        }

        private void customizedPictureBox_Click(object sender, EventArgs e)
        {
            showCustomizedChecked = !showCustomizedChecked;

            if (showCustomizedChecked && ModifierKeys != Keys.Control)
            {
                showTickedOnlyChecked = false;
                showPredefinedChecked = false;
                showUserChecked = false;
                showPlaylistLimitedChecked = false;
                showFunctionIdAssignedChecked = false;
                showHotkeyAssignedChecked = false;
            }
            else if (showCustomizedChecked && showUserChecked)
            {
                showUserChecked = false;
            }

            filterPresetList();
        }

        private void userPictureBox_Click(object sender, EventArgs e)
        {
            showUserChecked = !showUserChecked;

            if (showUserChecked && ModifierKeys != Keys.Control)
            {
                showTickedOnlyChecked = false;
                showPredefinedChecked = false;
                showCustomizedChecked = false;
                showPlaylistLimitedChecked = false;
                showFunctionIdAssignedChecked = false;
                showHotkeyAssignedChecked = false;
            }
            else if (showCustomizedChecked && showUserChecked)
            {
                showCustomizedChecked = false;
                showPredefinedChecked = false;
            }
            else if (showPredefinedChecked && showUserChecked)
            {
                showPredefinedChecked = false;
            }

            filterPresetList();
        }

        private void playlistPictureBox_Click(object sender, EventArgs e)
        {
            showPlaylistLimitedChecked = !showPlaylistLimitedChecked;

            if (showPlaylistLimitedChecked && ModifierKeys != Keys.Control)
            {
                showTickedOnlyChecked = false;
                showPredefinedChecked = false;
                showCustomizedChecked = false;
                showUserChecked = false;
                showFunctionIdAssignedChecked = false;
                showHotkeyAssignedChecked = false;
            }

            filterPresetList();
        }

        private void functionIdPictureBox_Click(object sender, EventArgs e)
        {
            showFunctionIdAssignedChecked = !showFunctionIdAssignedChecked;

            if (showFunctionIdAssignedChecked && ModifierKeys != Keys.Control)
            {
                showTickedOnlyChecked = false;
                showPredefinedChecked = false;
                showCustomizedChecked = false;
                showUserChecked = false;
                showPlaylistLimitedChecked = false;
                showHotkeyAssignedChecked = false;
            }

            filterPresetList();
        }

        private void hotkeyPictureBox_Click(object sender, EventArgs e)
        {
            showHotkeyAssignedChecked = !showHotkeyAssignedChecked;

            if (showHotkeyAssignedChecked && ModifierKeys != Keys.Control)
            {
                showTickedOnlyChecked = false;
                showPredefinedChecked = false;
                showCustomizedChecked = false;
                showUserChecked = false;
                showPlaylistLimitedChecked = false;
                showFunctionIdAssignedChecked = false;
            }

            filterPresetList();
        }

        private void uncheckAllFiltersPictureBox_Click(object sender, EventArgs e)
        {
            untickAllChecked = false;

            showHotkeyAssignedChecked = false;
            showFunctionIdAssignedChecked = false;
            showPlaylistLimitedChecked = false;
            showUserChecked = false;
            showCustomizedChecked = false;
            showPredefinedChecked = false;
            showTickedOnlyChecked = false;

            filterPresetList();
        }
    }

    public partial class Plugin
    {
        public void ASRPreset1EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.ApplyPreset(1);
        }

        public void ASRPreset2EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.ApplyPreset(2);
        }

        public void ASRPreset3EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.ApplyPreset(3);
        }

        public void ASRPreset4EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.ApplyPreset(4);
        }

        public void ASRPreset5EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.ApplyPreset(5);
        }

        public void ASRPreset6EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.ApplyPreset(6);
        }

        public void ASRPreset7EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.ApplyPreset(7);
        }

        public void ASRPreset8EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.ApplyPreset(8);
        }

        public void ASRPreset9EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.ApplyPreset(9);
        }

        public void ASRPreset10EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.ApplyPreset(10);
        }

        public void ASRPreset11EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.ApplyPreset(11);
        }

        public void ASRPreset12EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.ApplyPreset(12);
        }

        public void ASRPreset13EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.ApplyPreset(13);
        }

        public void ASRPreset14EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.ApplyPreset(14);
        }

        public void ASRPreset15EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.ApplyPreset(15);
        }

        public void ASRPreset16EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.ApplyPreset(16);
        }

        public void ASRPreset17EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.ApplyPreset(17);
        }

        public void ASRPreset18EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.ApplyPreset(18);
        }

        public void ASRPreset19EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.ApplyPreset(19);
        }

        public void ASRPreset20EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand.ApplyPreset(20);
        }
    }
}
