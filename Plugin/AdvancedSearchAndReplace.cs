﻿using ExtensionMethods;
using MusicBeePlugin.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using static MusicBeePlugin.Plugin;


namespace MusicBeePlugin
{
    public partial class AdvancedSearchAndReplace : PluginWindowTemplate
    {
        protected bool forceCloseForms = true;

        private static string CustomText1;


        private CustomComboBox parameterTag6ListCustom;
        private CustomComboBox parameterTag5ListCustom;
        private CustomComboBox parameterTag4ListCustom;
        private CustomComboBox parameterTag3ListCustom;
        private CustomComboBox parameterTag2ListCustom;
        private CustomComboBox parameterTagListCustom;

        private CustomComboBox playlistComboBoxCustom;
        private CustomComboBox filterComboBoxCustom;


        private const int tableColumnCount = 20; //IT'S COLUMN COUNT OF "previewTable" !!!
        const int ColumnsTrackWidth = 400;


        private static bool IgnoreFilterComboBoxSelectedIndexChanged;

        private bool ignoreSplitterMovedEvent = true;

        private bool previewTableLeftMouseButtonPressed = false;

        private bool processPresetChanges = true;
        private bool ignoreCheckedPresetEvent = true;
        private int autoAppliedPresetCount;

        private Font tagNameFont;

        private Bitmap warning;
        private Bitmap warningWide;
        private Bitmap checkedState;
        private Bitmap uncheckedState;

        private readonly DataGridViewCellStyle unchangedCellStyle = new DataGridViewCellStyle(UnchangedCellStyle);
        private readonly DataGridViewCellStyle changedCellStyle = new DataGridViewCellStyle(ChangedCellStyle);
        private readonly DataGridViewCellStyle dimmedCellStyle = new DataGridViewCellStyle(DimmedCellStyle);
        private readonly DataGridViewCellStyle preservedTagCellStyle = new DataGridViewCellStyle(PreservedTagCellStyle);
        private readonly DataGridViewCellStyle preservedTagValueCellStyle = new DataGridViewCellStyle(PreservedTagValueCellStyle);

        private delegate void AddRowToTable(string[] row, object changeTypes);
        private delegate void ProcessRowOfTable(int row);
        private delegate void UpdateTable();
        private AddRowToTable addRowToTable;
        private ProcessRowOfTable processRowOfTable;
        private UpdateTable updateTable;


        private static readonly Encoding Unicode = Encoding.UTF8;

        private string[] files = Array.Empty<string>();
        private readonly List<string[]> tags = new List<string[]>();
        private Preset selectedPreset;
        private Preset backedUpPreset;
        private bool processTagsMode = true;

        private SortedDictionary<string, Preset> presetProcessingCopies; //<Guid as string, Preset>: To deal with <All Tags> pseudo tag some presets will be duplicated
        private int allTagsReplaceIdsCount;
        private bool[] allTagsPresetParameterTagMask;

        private Dictionary<TagIdCombination, bool> tagIdCombinations;

        private string[] fileTags;
        private string clipboardText;

        private static Dictionary<int, string> SetTags = new Dictionary<int, string>();
        private static string LastSetTagValue;

        internal static string PresetsPath;
        internal static SortedDictionary<Guid, Preset> Presets;
        private SortedDictionary<Guid, Preset> presetsWorkingCopy = new SortedDictionary<Guid, Preset>();

        private SortedDictionary<Guid, bool> autoAppliedAsrPresetGuids;
        private int asrPresetsWithHotkeysCount;
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

        private static string AllTagsPresetsCantBeAutoAppliedText;
        private static string AllTagsTagIsSelectedToolTip;

        private static string AutoApplyDisabledText;

        private static string CellTooTip;

        private System.Threading.Timer allTagsWarningTimer;
        private int AllTagsPresetsCantBeAutoAppliedCountdown;
        private bool selectedPresetUsesAllTags;

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

        private bool unsavedChanges;
        private string buttonCloseToolTip;
        private int presetListLastSelectedIndex = -2;

        internal AdvancedSearchAndReplace(Plugin plugin) : base(plugin)
        {
            InitializeComponent();

            new ControlBorder(this.searchTextBox);
            new ControlBorder(this.customTextBox);
            new ControlBorder(this.customText2Box);
            new ControlBorder(this.customText3Box);
            new ControlBorder(this.customText4Box);
            new ControlBorder(this.idTextBox);
            new ControlBorder(this.preserveTagValuesTextBox);
            new ControlBorder(this.processPreserveTagsTextBox);
            new ControlBorder(this.descriptionBox);
        }

        protected override void initializeForm()
        {
            base.initializeForm();
            Enable(false, autoApplyPresetsLabel);


            parameterTag6ListCustom = namesComboBoxes["parameterTag6List"];
            parameterTag5ListCustom = namesComboBoxes["parameterTag5List"];
            parameterTag4ListCustom = namesComboBoxes["parameterTag4List"];
            parameterTag3ListCustom = namesComboBoxes["parameterTag3List"];
            parameterTag2ListCustom = namesComboBoxes["parameterTag2List"];
            parameterTagListCustom = namesComboBoxes["parameterTagList"];

            playlistComboBoxCustom = namesComboBoxes["playlistComboBox"];
            filterComboBoxCustom = namesComboBoxes["filterComboBox"];


            warning = ReplaceBitmap(null, Warning);
            warningWide = ReplaceBitmap(null, WarningWide);
            checkedState = ReplaceBitmap(null, CheckedState);
            uncheckedState = ReplaceBitmap(null, UncheckedState);

            //Setting control not standard properties
            //var heightField = presetList.GetType().GetField(
            //   "scaledListItemBordersHeight",
            //   BindingFlags.NonPublic | BindingFlags.Instance
            //);

            //var addedHeight = (int)Math.Round(4d * dpiScaleFactor); //Some appropriate value, greater than the field's default of 2
            //heightField.SetValue(presetList, addedHeight); //Where "presetList" is your CheckedListBox

            tableLayoutPanel3ColWidth0 = tableLayoutPanel3.ColumnStyles[0].Width * hDpiFontScaling;
            tableLayoutPanel3ColWidth1 = tableLayoutPanel3.ColumnStyles[1].Width;
            tableLayoutPanel3ColWidth2 = tableLayoutPanel3.ColumnStyles[2].Width * hDpiFontScaling;


            //Setting themed images
            searchPictureBox.Image = ReplaceBitmap(searchPictureBox.Image, Search);

            tickedOnlyPictureBox.Image = ReplaceBitmap(tickedOnlyPictureBox.Image, AutoAppliedPresetsDimmed);
            predefinedPictureBox.Image = ReplaceBitmap(predefinedPictureBox.Image, PredefinedPresetsDimmed);
            customizedPictureBox.Image = ReplaceBitmap(customizedPictureBox.Image, CustomizedPresetsDimmed);
            userPictureBox.Image = ReplaceBitmap(userPictureBox.Image, UserPresetsDimmed);
            playlistPictureBox.Image = ReplaceBitmap(playlistPictureBox.Image, PlaylistPresetsDimmed);
            functionIdPictureBox.Image = ReplaceBitmap(functionIdPictureBox.Image, FunctionIdPresetsDimmed);
            hotkeyPictureBox.Image = ReplaceBitmap(hotkeyPictureBox.Image, HotkeyPresetsDimmed);
            uncheckAllFiltersPictureBox.Image = ReplaceBitmap(uncheckAllFiltersPictureBox.Image, UncheckAllFiltersDimmed);

            buttonLabels[clearSearchButton] = string.Empty;
            clearSearchButton.Text = string.Empty;
            clearSearchButton.Image = ReplaceBitmap(clearSearchButton.Image, ClearField);

            buttonLabels[clearIdButton] = string.Empty;
            clearIdButton.Text = string.Empty;
            clearIdButton.Image = ReplaceBitmap(clearIdButton.Image, ClearField);

            buttonSettings.Image = ReplaceBitmap(buttonSettings.Image, Gear);

            //Setting themed colors
            descriptionBox.ForeColor = DimmedHighlight;
            descriptionBox.BackColor = BackColor;


            //Initialization
            presetsWorkingCopy = new SortedDictionary<Guid, Preset>();
            foreach (var preset in Presets.Values)
            {
                var presetCopy = new Preset(preset);
                presetsWorkingCopy.Add(presetCopy.guid, presetCopy);
            }

            autoAppliedAsrPresetGuids = new SortedDictionary<Guid, bool>();
            foreach (var guid in SavedSettings.autoAppliedAsrPresetGuids)
                autoAppliedAsrPresetGuids.Add(guid, false);

            asrPresetsWithHotkeysGuids = new Guid[MaximumNumberOfAsrHotkeys];
            for (var j = 0; j < MaximumNumberOfAsrHotkeys; j++)
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


            var entireText = autoApplyPresetsLabel.Text;
            EditApplyText = Regex.Replace(entireText, @"^(.*?)~.*", "$1");
            AutoApplyText = Regex.Replace(entireText, @"^.*?~(.*?)~.*", "$1");
            ClickHereText = Regex.Replace(entireText, @"^.*?~.*?~(.*?)~.*", "$1");
            NowTickedText = Regex.Replace(entireText, @"^.*?~.*?~.*?~(.*?)~.*", "$1");
            AllTagsPresetsCantBeAutoAppliedText = Regex.Replace(entireText, @"^.*?~.*?~.*?~.*?~(.*?)~.*", "$1");
            AutoApplyDisabledText = Regex.Replace(entireText, @"^.*?~.*?~.*?~.*?~.*?~(.*)", "$1");


            asrPresetsWithHotkeysCount = AsrPresetsWithHotkeysCount;
            assignHotkeyCheckBoxText = Regex.Replace(assignHotkeyCheckBoxLabel.Text, "^(.*:\\s).*", "$1");
            assignHotkeyCheckBoxLabel.Text = assignHotkeyCheckBoxText + (MaximumNumberOfAsrHotkeys - asrPresetsWithHotkeysCount) + "/" + MaximumNumberOfAsrHotkeys;


            var headerCellStyle = new DataGridViewCellStyle(HeaderCellStyle);

            var cbHeader = new DataGridViewCheckBoxHeaderCell();
            cbHeader.Style = headerCellStyle;
            cbHeader.setState(true);
            cbHeader.OnCheckBoxClicked += cbHeader_OnCheckBoxClicked;

            var colCB = new DataGridViewCheckBoxColumn
            {
                HeaderCell = cbHeader,
                ThreeState = true,
                FalseValue = "F",
                TrueValue = "T",
                IndeterminateValue = string.Empty,
                Width = 25,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            };

            previewTable.Columns.Insert(0, colCB);
            previewTable.Columns[1].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[2].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[3].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[4].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[5].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[6].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[7].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[8].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[9].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[10].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[11].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[12].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[13].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[14].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[15].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[16].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[17].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[18].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[19].HeaderCell.Style = headerCellStyle;

            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;


            var tagNameFontSize = previewTable.DefaultCellStyle.Font.Size * 0.8f; //Maybe it's worth to adjust fine size !!!
            tagNameFont = new Font(Font.FontFamily, tagNameFontSize, FontStyle.Bold);//***
            //tagNameFont = Font.Clone() as Font;

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
            previewTable.Columns[19].Width = (int)Math.Round(25 * hDpiFontScaling);
            previewTable.Columns[19].MinimumWidth = (int)Math.Round(25 * hDpiFontScaling);

            TagNameText = previewTable.Columns[4].HeaderText;
            OrigValueText = previewTable.Columns[5].HeaderText;
            NewValueText = previewTable.Columns[6].HeaderText;


            buttonClose.Image = Resources.transparent_15;
            buttonCloseToolTip = toolTip1.GetToolTip(buttonClose);
            toolTip1.SetToolTip(buttonClose, string.Empty);

            AllTagsTagIsSelectedToolTip = toolTip1.GetToolTip(labelTag);
            toolTip1.SetToolTip(labelTag, string.Empty);
            pictureTag.Image = Resources.transparent_15;
            pictureTag2.Image = Resources.transparent_15;
            pictureTag3.Image = Resources.transparent_15;
            pictureTag4.Image = Resources.transparent_15;
            pictureTag5.Image = Resources.transparent_15;
            pictureTag6.Image = Resources.transparent_15;

            addRowToTable = previewTable_AddRowToTable;
            processRowOfTable = previewTable_ProcessRowOfTable;
            updateTable = previewTable_UpdateTable;


            IgnoreFilterComboBoxSelectedIndexChanged = true;
            filterComboBoxCustom.SelectedIndex = 0;
            IgnoreFilterComboBoxSelectedIndexChanged = false;


            processPresetChanges = false;

            presetList.Sorted = false;
            ignoreCheckedPresetEvent = false;

            autoAppliedPresetCount = 0;
            foreach (var preset in presetsWorkingCopy.Values)
                presetList.Items.Add(preset, autoAppliedAsrPresetGuids.ContainsKey(preset.guid));

            presetList.Sorted = true;


            assignHotkeyCheckBox.Checked = false;


            MbApiInterface.Playlist_QueryPlaylists();
            var playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();

            while (playlist != null)
            {
                //if (mbApiInterface.Playlist_GetType(playlist) == MusicBeePlugin.PlaylistFormat.Auto)
                {
                    var newPlaylist = new Playlist(playlist);
                    playlistComboBoxCustom.Items.Add(newPlaylist);
                }

                playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();
            }

            if (playlistComboBoxCustom.Items.Count > 0)
                playlistComboBoxCustom.SelectedIndex = 0;


            SetComboBoxCue(filterComboBoxCustom, CtlMixedFilters);

            presetList.SelectedIndex = -1;
            presetList_SelectedIndexChanged(null, null);


            showAutoApplyingWarningIfRequired();


            Enable(true, autoApplyPresetsLabel);


            ignoreCheckedPresetEvent = true;
            processPresetChanges = true;

            updateCustomScrollBars(presetList);

            descriptionBox.Refresh();

            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        public enum TagType
        {
            Undefined = -1, //****** remove  carefully!!!!
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
            public bool changed; //"changed" is set to "false" initially and on saving preset, and is set to "true" on any preset modification
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

            public TagType parameterTagTypeNew = TagType.NotUsed; //****** rename to parameterTagType later !!!!
            public TagType parameterTag2TypeNew = TagType.NotUsed;
            public TagType parameterTag3TypeNew = TagType.NotUsed;
            public TagType parameterTag4TypeNew = TagType.NotUsed;
            public TagType parameterTag5TypeNew = TagType.NotUsed;
            public TagType parameterTag6TypeNew = TagType.NotUsed;

            public TagType parameterTagType = TagType.NotUsed; //****** make primary and the only later !!!!
            public TagType parameterTag2Type= TagType.NotUsed;
            public TagType parameterTag3Type = TagType.NotUsed;
            public TagType parameterTag4Type = TagType.NotUsed;
            public TagType parameterTag5Type = TagType.NotUsed;
            public TagType parameterTag6Type = TagType.NotUsed;

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
                id = string.Empty;
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
                    var lang = langName.Key;
                    if (lang == null)
                        continue;

                    var name = langName.Value ?? string.Empty;

                    if (!string.IsNullOrEmpty(presetNamePrefix) && !name.StartsWith(presetNamePrefix))
                        name = presetNamePrefix  + " " + name;
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

                foreach (var width in originalPreset.columnWeights)
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

                limitation1 = originalPreset.limitation1;
                limitation2 = originalPreset.limitation2;
                limitation3 = originalPreset.limitation3;
                limitation4 = originalPreset.limitation4;
                limitation5 = originalPreset.limitation5;
            }

            private string getHotkeyChar()
            {
                if (!hotkeyAssigned)
                    return string.Empty;
                else if (!applyToPlayingTrack)
                    return "";
                else
                    return "";
            }

            private string getHotkeyPostfix()
            {
                var hotkeyChar = getHotkeyChar();

                if (hotkeyChar == string.Empty)
                    return hotkeyChar;
                else
                    return " " + hotkeyChar;
            }

            public string getHotkeyDescription()
            {
                return AsrPresetHotkeyDescription + "⌕: " + getName() + getHotkeyPostfix();
            }

            public override string ToString()
            {
                var changedPostfix = changed ? " ⚠" : string.Empty;

                return (favorite ? "♥ " : string.Empty) + GetDictValue(names, Language) + (getCustomizationsFlag() ? " " : string.Empty) + (userPreset ? " " : string.Empty)
                    + (condition ? " " : string.Empty) + (id != string.Empty ? " " : string.Empty) + getHotkeyPostfix() + changedPostfix;
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
                var presetSafeFileName = getName().Replace('\\', '-').Replace('/', '-').Replace('<', '[').Replace('>', ']')
                    .Replace(" : ", " - ").Replace(": ", " - ").Replace(":", "-")
                    .Replace("\"", "\'\'")
                    .Replace('*', '#').Replace('?', '#').Replace('|', '#');

                if (presetSafeFileName.Length > 251 - AsrPresetExtension.Length)
                    presetSafeFileName = presetSafeFileName.Substring(0, 250 - AsrPresetExtension.Length) + "…";

                return presetSafeFileName;
            }

            public static Preset Load(string filename, XmlSerializer presetSerializer)
            {
                var stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None);
                var file = new StreamReader(stream, Unicode);

                var savedPreset = (Preset)presetSerializer.Deserialize(file);
                savedPreset.hotkeyAssigned = false;

                file.Close();


                if (savedPreset.parameterTagTypeNew == TagType.Undefined) //***** remove later!!!
                    savedPreset.parameterTagTypeNew = TagType.NotUsed;

                if (savedPreset.parameterTag2TypeNew == TagType.Undefined)
                    savedPreset.parameterTag2TypeNew = TagType.NotUsed;

                if (savedPreset.parameterTag3TypeNew == TagType.Undefined)
                    savedPreset.parameterTag3TypeNew = TagType.NotUsed;

                if (savedPreset.parameterTag4TypeNew == TagType.Undefined)
                    savedPreset.parameterTag4TypeNew = TagType.NotUsed;

                if (savedPreset.parameterTag5TypeNew == TagType.Undefined)
                    savedPreset.parameterTag5TypeNew = TagType.NotUsed;

                if (savedPreset.parameterTag6TypeNew == TagType.Undefined)
                    savedPreset.parameterTag6TypeNew = TagType.NotUsed;


                savedPreset.parameterTagType = savedPreset.parameterTagTypeNew;
                savedPreset.parameterTag2Type= savedPreset.parameterTag2TypeNew;
                savedPreset.parameterTag3Type = savedPreset.parameterTag3TypeNew;
                savedPreset.parameterTag4Type = savedPreset.parameterTag4TypeNew;
                savedPreset.parameterTag5Type = savedPreset.parameterTag5TypeNew;
                savedPreset.parameterTag6Type = savedPreset.parameterTag6TypeNew;


                savedPreset.savePreset(Path.Combine(PresetsPath, savedPreset.getSafeFileName() + AsrPresetExtension)); //******* remove later !!!!

                return savedPreset;
            }

            public bool areFineCustomizationsMade(Preset referencePreset)
            {
                var areFineCustomizationsMade = false;

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

            public void setCustomizationsFlag(AdvancedSearchAndReplace form, Preset referencePreset)
            {
                if (referencePreset == null)
                    return;

                var customized = false;

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
                    if (form.unsavedChanges || customized || areFineCustomizationsMade(referencePreset))
                    {
                        changed = true;
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
                    pathName = Path.Combine(@"\\?\" + PresetsPath, guid.ToString() + AsrPresetExtension);

                var presetSerializer = new XmlSerializer(typeof(Preset));

                var stream = System.IO.File.Open(pathName, FileMode.Create, FileAccess.Write, FileShare.None);
                var file = new StreamWriter(stream, Unicode);

                presetSerializer.Serialize(file, this);

                file.Close();

                return pathName;
            }

            public string replaceVariable(string pattern, bool isSearchPattern) //isSearchPattern: true - search (i.e. must be escaped), false - replace
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

            public (int, bool[]) getAllTagsReplaceTagIds() //Returns (<All Tags> replace parameter Id count, <All Tags> parameter Id mask)
            {
                var allTagsParameterTagMask = new bool[5];
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


                return (countAllTagsReplaceTagIds(), allTagsParameterTagMask);
            }

            public int countAllTagsReplaceTagIds() //Counts <All Tags> replace parameter Ids
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
                switch ((ServiceMetaData)tagId) //-V3002
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

        internal struct SearchedAndReplacedTagsStruct
        {
            internal string searchedTagValue;
            internal string searchedTag2Value;
            internal string searchedTag3Value;
            internal string searchedTag4Value;
            internal string searchedTag5Value;

            internal string originalReplacedTagValue;
            internal string originalReplacedTag2Value;
            internal string originalReplacedTag3Value;
            internal string originalReplacedTag4Value;
            internal string originalReplacedTag5Value;

            internal string replacedTagValue;
            internal string replacedTag2Value;
            internal string replacedTag3Value;
            internal string replacedTag4Value;
            internal string replacedTag5Value;

            internal bool replacedTagValuePreserved;
            internal bool replacedTag2ValuePreserved;
            internal bool replacedTag3ValuePreserved;
            internal bool replacedTag4ValuePreserved;
            internal bool replacedTag5ValuePreserved;

            internal bool replacedTagPreserved;
            internal bool replacedTag2Preserved;
            internal bool replacedTag3Preserved;
            internal bool replacedTag4Preserved;
            internal bool replacedTag5Preserved;
        }

        public class Playlist
        {
            public string playlist;

            public Playlist(string playlist)
            {
                this.playlist = playlist;
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

            internal virtual void multiEvaluate1(string currentFile, ref string tagValue)
            {
                var args = Regex.Matches(tagValue, @"\\@" + functionName + @"\[\[(.*?)\]\]");

                for (var i = 0; i < args.Count; i++)
                {
                    var arg = Regex.Replace(args[i].Value, @"\\@" + functionName + @"\[\[(.*?)\]\]", "$1");
                    var result = calculate(currentFile, arg);
                    tagValue = Regex.Replace(tagValue, @"\\@" + functionName + @"\[\[" + Regex.Escape(arg) + @"\]\]", result, RegexOptions.None);
                }
            }

            internal virtual void multiEvaluate2(string currentFile, ref string tagValue)
            {
                var args1 = Regex.Matches(tagValue, @"\\@" + functionName + @"\[\[(.*)\;\;.*?\]\]");
                //var args2 = Regex.Matches(tagValue, @"\\@" + functionName + @"\[\[.*\;\;(.*?)\]\]");

                for (var i = 0; i < args1.Count; i++)
                {
                    var arg1 = Regex.Replace(args1[i].Value, @"\\@" + functionName + @"\[\[(.*)\;\;.*?\]\]", "$1");
                    var arg2 = Regex.Replace(args1[i].Value, @"\\@" + functionName + @"\[\[.*\;\;(.*?)\]\]", "$1");
                    var result = calculate(currentFile, arg1, arg2);
                    tagValue = Regex.Replace(tagValue, @"\\@" + functionName + @"\[\[" + Regex.Escape(arg1) + @"\;\;" + Regex.Escape(arg2) + @"\]\]", result, RegexOptions.None);
                }
            }

            internal string evaluate(string currentFile, string tagValue)
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
            internal Rg2sc()
            {
                functionName = "rg2sc";
            }

            protected override string calculate(string currentFile, string parameter0, string parameter1 = null) //-V3065
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


                var soundCheck1000d = 1000 * Math.Pow(10.0, (-0.1 * replayGain));
                //if (soundCheck1000d > 65534)
                //   soundCheck1000d = 65534;
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
                //   soundCheck2500d = 65534;
                //uint soundCheck2500;

                //try
                //{
                //   soundCheck2500 = Convert.ToUInt32(soundCheck2500d);
                //}
                //catch
                //{
                //   soundCheck2500 = 0;
                //}


                var ITUNNORM = (" " + soundCheck1000.ToString("X8"));
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
            internal Rg2sc4mp3()
            {
                functionName = "rg2sc4mp3";
            }

            protected override string calculate(string currentFile, string parameter0, string parameter1 = null)
            {
                return base.calculate(currentFile, parameter0, parameter1) + "\u0000";
            }
        }

        private class Tc : Function
        {
            internal Tc()
            {
                functionName = "tc";
            }

            protected override string calculate(string currentFile, string parameter0, string parameter1 = null)
            {
                if (parameter1 == null)
                    parameter1 = SavedSettings.exceptionWordsAsr;

                var exceptedWords = parameter1.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                parameter0 = ChangeCase.ChangeWordsCase(parameter0, ChangeCase.ChangeCaseOptions.LowerCase);

                var result = ChangeCase.ChangeSentenceCase(parameter0, ChangeCase.ChangeCaseOptions.TitleCase, exceptedWords, false,
                    SavedSettings.exceptionCharsAsr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries),
                    SavedSettings.openingExceptionCharsAsr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries),
                    SavedSettings.closingExceptionCharsAsr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries),
                    SavedSettings.sentenceSeparatorsAsr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), true, true);

                return result;
            }
        }

        private class Lc : Function
        {
            internal Lc()
            {
                functionName = "lc";
            }

            protected override string calculate(string currentFile, string parameter0, string parameter1 = null)
            {
                if (parameter1 == null)
                    parameter1 = SavedSettings.exceptionWordsAsr;

                var exceptedWords = parameter1.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var result = ChangeCase.ChangeSentenceCase(parameter0, ChangeCase.ChangeCaseOptions.LowerCase, exceptedWords, false,
                    SavedSettings.exceptionCharsAsr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries),
                    SavedSettings.openingExceptionCharsAsr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries),
                    SavedSettings.closingExceptionCharsAsr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries),
                    SavedSettings.sentenceSeparatorsAsr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), false, false);

                return result;
            }
        }

        private class Uc : Function
        {
            internal Uc()
            {
                functionName = "uc";
            }

            protected override string calculate(string currentFile, string parameter0, string parameter1 = null)
            {
                if (parameter1 == null)
                    parameter1 = SavedSettings.exceptionWordsAsr;

                var exceptedWords = parameter1.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var result = ChangeCase.ChangeSentenceCase(parameter0, ChangeCase.ChangeCaseOptions.UpperCase, exceptedWords, false,
                    SavedSettings.exceptionCharsAsr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries),
                    SavedSettings.openingExceptionCharsAsr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries),
                    SavedSettings.closingExceptionCharsAsr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries),
                    SavedSettings.sentenceSeparatorsAsr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), false, false);

                return result;
            }
        }

        private class Sc : Function
        {
            internal Sc()
            {
                functionName = "sc";
            }

            protected override string calculate(string currentFile, string parameter0, string parameter1 = null)
            {
                if (parameter1 == null)
                    parameter1 = SavedSettings.exceptionWordsAsr;

                var exceptedWords = parameter1.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                parameter0 = ChangeCase.ChangeWordsCase(parameter0, ChangeCase.ChangeCaseOptions.LowerCase);

                var result = ChangeCase.ChangeSentenceCase(parameter0, ChangeCase.ChangeCaseOptions.SentenceCase, exceptedWords, false, 
                    SavedSettings.exceptionCharsAsr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries),
                    SavedSettings.openingExceptionCharsAsr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries),
                    SavedSettings.closingExceptionCharsAsr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries),
                    SavedSettings.sentenceSeparatorsAsr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), true, false);

                return result;
            }
        }

        private class Eval : Function
        {
            internal Eval()
            {
                functionName = "eval";
            }

            protected override string calculate(string currentFile, string parameter0, string parameter1 = null) //-V3065
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
            internal Char1()
            {
                functionName = "char";
            }

            protected override string calculate(string currentFile, string parameter0, string parameter1 = null)
            {
                var charcode = ushort.Parse(parameter0, System.Globalization.NumberStyles.HexNumber);
                var character = ((char)charcode).ToString();

                if (parameter1 == null)
                {
                    return character;
                }
                else
                {
                    var sequence = string.Empty;

                    var times = int.Parse(parameter1);
                    for (var i = 0; i < times; i++)
                        sequence += character;

                    return sequence;
                }
            }
        }

        private class Repunct : Function
        {
            internal Repunct()
            {
                functionName = "repunct";
            }

            protected override string calculate(string currentFile, string parameter0, string parameter1 = null) //-V3065
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
            internal SortPerformers()
            {
                functionName = "sortperformers";
            }

            internal override void multiEvaluate1(string currentFile, ref string tagValue)
            {
                var args = Regex.Matches(tagValue, @"\\@" + functionName + @"\[\[(.*)\]\]");

                for (var i = 0; i < args.Count; i++)
                {
                    var rawArg = Regex.Replace(args[i].Value, @"\\@" + functionName + @"\[\[(.*)\]\]", "$1");
                    var arg = rawArg.Replace("\u000E", "[").Replace("\u000F", "]").Replace("\u0010", ",").Replace("\u0011", @"\");
                    var result = calculate(currentFile, arg);
                    tagValue = Regex.Replace(tagValue, @"\\@" + functionName + @"\[\[" + Regex.Escape(rawArg) + @"\]\]", result, RegexOptions.IgnoreCase);
                }
            }

            protected override string calculate(string currentFile, string parameter0, string parameter1 = null) //-V3065
            {
                CustomText1 = Regex.Replace(CustomText1, "; ", ";");
                var artists = parameter0.Replace("\u000E", "[").Replace("\u000F", "]").Split(new[] { "\u0000" }, StringSplitOptions.RemoveEmptyEntries);
                var roles = CustomText1.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                var performersList = new List<string>();

                for (var i = 0; i < artists.Length; i++)
                {
                    if (artists[i][0] == '\u0002')
                    {
                        performersList.Add(artists[i]);
                        artists[i] = null;
                    }
                }

                if (performersList.Count == 0)
                    return parameter0;


                for (var i = 0; i < performersList.Count; i++)
                {
                    for (var j = 0; j < roles.Length; j++)
                    {
                        if (Regex.IsMatch(performersList[i], @"^.*\(" + roles[j] + @"\).*"))
                        {
                            performersList[i] = j.ToString("D3") + performersList[i];
                            break;
                        }
                    }
                }

                for (var i = 0; i < performersList.Count; i++)
                {
                    if (performersList[i][0] != '0')
                        performersList[i] = "999" + performersList[i];
                }

                performersList.Sort();

                var newArtists = string.Empty;

                var performerNo = 0;
                for (var i = 0; i < artists.Length; i++)
                {
                    if (artists[i] == null)
                    {
                        newArtists += performersList[performerNo++].Remove(0, 3) + "\u0000";
                    }
                    else
                    {
                        newArtists += artists[i] + "\u0000";
                    }
                }

                newArtists = newArtists.Remove(newArtists.Length - 1, 1);

                return newArtists;
            }
        }

        private class SortInvolvedPeopleByRoles : Function
        {
            internal SortInvolvedPeopleByRoles()
            {
                functionName = "sortinvolvedpeoplebyrole";
            }

            internal override void multiEvaluate1(string currentFile, ref string tagValue)
            {
                var args = Regex.Matches(tagValue, @"\\@" + functionName + @"\[\[(.*)\]\]");

                for (var i = 0; i < args.Count; i++)
                {
                    var rawArg = Regex.Replace(args[i].Value, @"\\@" + functionName + @"\[\[(.*)\]\]", "$1");
                    var arg = rawArg.Replace("\u000E", "[").Replace("\u000F", "]").Replace("\u0010", ",").Replace("\u0011", @"\");
                    var result = calculate(currentFile, arg);
                    tagValue = Regex.Replace(tagValue, @"\\@" + functionName + @"\[\[" + Regex.Escape(rawArg) + @"\]\]", result, RegexOptions.IgnoreCase);
                }
            }

            protected override string calculate(string currentFile, string parameter0, string parameter1 = null) //-V3065
            {
                CustomText1 = Regex.Replace(CustomText1, "; ", ";");
                var people = parameter0.Replace("\u000E", "[").Replace("\u000F", "]").Replace("; ", ";").Replace(": ", ":").Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                var roles = CustomText1.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                var involvedPeople = new List<string>();

                for (var i = 0; i < people.Length; i++)
                {
                    involvedPeople.Add(people[i]);
                }

                people = null;

                if (involvedPeople.Count == 0)
                    return parameter0;


                for (var i = 0; i < involvedPeople.Count; i++)
                {
                    for (var j = 0; j < roles.Length; j++)
                    {
                        if (Regex.IsMatch(involvedPeople[i], "^" + roles[j] + ":.*"))
                        {
                            involvedPeople[i] = j.ToString("D3") + involvedPeople[i];
                            break;
                        }
                    }
                }

                for (var i = 0; i < involvedPeople.Count; i++)
                {
                    if (involvedPeople[i][0] != '0')
                        involvedPeople[i] = "999" + involvedPeople[i];
                }

                involvedPeople.Sort();

                var newPeople = string.Empty;

                var personNo = 0;
                for (var i = 0; i < involvedPeople.Count; i++)
                {
                    newPeople += involvedPeople[personNo++].Remove(0, 3) + ";";
                }

                newPeople = newPeople.Remove(newPeople.Length - 1, 1);

                return newPeople;
            }
        }

        private class SearchReplace : Function
        {
            internal SearchReplace()
            {
                functionName = "replace";
            }

            protected override string calculate(string currentFile, string parameter0, string parameter1 = null)
            {
                if (parameter1 == null)
                    return CtlAsrSyntaxError;

                var secondParameter = parameter1.Split('|');

                for (var i = 0; i < secondParameter.Length; i++)
                {
                    var pair = secondParameter[i].Split('/');

                    if (pair.Length != 2)
                    {
                        return CtlAsrSyntaxError;
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

        internal static string GetDictValue(SortedDictionary<string, string> dict, string language)
        {
            if (language == null)
                return string.Empty;


            dict.TryGetValue(language, out var value);

            if (value == null)
                dict.TryGetValue("en", out value);

            return value;
        }

        internal static void SetDictValue(SortedDictionary<string, string> dict, string language, string newValue)
        {
            if (language == null)
                return;


            dict.TryGetValue(language, out var value);

            if (value != null)
                dict.Remove(language);

            dict.Add(language, newValue);


            dict.TryGetValue("en", out value);

            if (value == null)
                dict.Add("en", newValue);
        }

        internal static void InitAsr()
        {
            if (SavedSettings.dontShowAsr)
            {
                lock (AsrAutoAppliedPresets)
                {
                    AsrAutoAppliedPresets.Clear();
                    IdsAsrPresets.Clear();
                    AsrPresetsWithHotkeysCount = 0;
                    for (var i = 0; i < AsrPresetsWithHotkeys.Length; i++)
                        AsrPresetsWithHotkeys[i] = null;
                }

                return;
            }

            lock (AsrAutoAppliedPresets)
            {
                SetTags = new Dictionary<int, string>();

                PresetsPath = Path.Combine(@"\\?\" + MbApiInterface.Setting_GetPersistentStoragePath(), AsrPresetsDirectory);
                Presets = new SortedDictionary<Guid, Preset>();

                AsrAutoAppliedPresets.Clear();
                AsrPresetsWithHotkeysCount = 0;

                if (!Directory.Exists(PresetsPath))
                    Directory.CreateDirectory(PresetsPath);

                var presetNames = Directory.GetFiles(PresetsPath, "*" + AsrPresetExtension);
                var presetSerializer = new XmlSerializer(typeof(Preset));

                for (var i = presetNames.Length - 1; i >= 0; i--)
                {
                    var presetName = presetNames[i];

                    try
                    {
                        var preset = Preset.Load(presetName, presetSerializer);

                        if (preset.guid.ToString() != "ff8d53d9-526b-4b40-bbf0-848b6b892f70")
                        {
                            if (Presets.TryGetValue(preset.guid, out var existingPreset))
                            {
                                if (preset.modifiedUtc >= existingPreset.modifiedUtc)
                                {
                                    Presets.Remove(preset.guid);
                                    AsrAutoAppliedPresets.RemoveExisting(existingPreset);
                                    IdsAsrPresets.RemoveExisting(preset.id);
                                }
                            }

                            Presets.Add(preset.guid, preset);

                            if (SavedSettings.autoAppliedAsrPresetGuids.Contains(preset.guid))
                                AsrAutoAppliedPresets.Add(preset);

                            if (SavedSettings.asrPresetsWithHotkeysGuids.Contains(preset.guid))
                            {
                                int index;
                                AsrPresetsWithHotkeysCount++;
                                for (index = 0; index < SavedSettings.asrPresetsWithHotkeysGuids.Length; index++)
                                {
                                    if (SavedSettings.asrPresetsWithHotkeysGuids[index] == preset.guid)
                                        break;
                                }
                                AsrPresetsWithHotkeys[index] = preset;
                                preset.hotkeyAssigned = true;
                            }
                            else
                            {
                                preset.hotkeyAssigned = false;
                            }

                            if (!string.IsNullOrEmpty(preset.id))
                            {
                                IdsAsrPresets.Add(preset.id, preset);
                            }
                        }
                        else
                        {
                            MSR = preset;
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

        internal static void RegisterAsrPresetsHotkeysAndMenuItems(Plugin plugin)
        {
            AsrPresetsMenuItem?.DropDown.Items.Clear();

            AsrPresetsContextMenuItem?.DropDown.Items.Clear();

            if (SavedSettings.dontShowAsr)
                return;

            for (var i = 0; i < AsrPresetsWithHotkeys.Length; i++)
            {
                var preset = AsrPresetsWithHotkeys[i];

                if (preset != null)
                {
                    switch (i)
                    {
                        case 0:
                            MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.AsrPreset1EventHandler);
                            AsrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset1EventHandler);
                            AsrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset1EventHandler);
                            break;
                        case 1:
                            MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.AsrPreset2EventHandler);
                            AsrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset2EventHandler);
                            AsrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset2EventHandler);
                            break;
                        case 2:
                            MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.AsrPreset3EventHandler);
                            AsrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset3EventHandler);
                            AsrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset3EventHandler);
                            break;
                        case 3:
                            MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.AsrPreset4EventHandler);
                            AsrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset4EventHandler);
                            AsrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset4EventHandler);
                            break;
                        case 4:
                            MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.AsrPreset5EventHandler);
                            AsrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset5EventHandler);
                            AsrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset5EventHandler);
                            break;
                        case 5:
                            MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.AsrPreset6EventHandler);
                            AsrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset6EventHandler);
                            AsrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset6EventHandler);
                            break;
                        case 6:
                            MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.AsrPreset7EventHandler);
                            AsrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset7EventHandler);
                            AsrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset7EventHandler);
                            break;
                        case 7:
                            MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.AsrPreset8EventHandler);
                            AsrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset8EventHandler);
                            AsrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset8EventHandler);
                            break;
                        case 8:
                            MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.AsrPreset9EventHandler);
                            AsrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset9EventHandler);
                            AsrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset9EventHandler);
                            break;
                        case 9:
                            MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.AsrPreset10EventHandler);
                            AsrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset10EventHandler);
                            AsrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset10EventHandler);
                            break;
                        case 10:
                            MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.AsrPreset11EventHandler);
                            AsrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset11EventHandler);
                            AsrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset11EventHandler);
                            break;
                        case 11:
                            MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.AsrPreset12EventHandler);
                            AsrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset12EventHandler);
                            AsrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset12EventHandler);
                            break;
                        case 12:
                            MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.AsrPreset13EventHandler);
                            AsrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset13EventHandler);
                            AsrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset13EventHandler);
                            break;
                        case 13:
                            MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.AsrPreset14EventHandler);
                            AsrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset14EventHandler);
                            AsrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset14EventHandler);
                            break;
                        case 14:
                            MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.AsrPreset15EventHandler);
                            AsrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset15EventHandler);
                            AsrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset15EventHandler);
                            break;
                        case 15:
                            MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.AsrPreset16EventHandler);
                            AsrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset16EventHandler);
                            AsrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset16EventHandler);
                            break;
                        case 16:
                            MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.AsrPreset17EventHandler);
                            AsrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset17EventHandler);
                            AsrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset17EventHandler);
                            break;
                        case 17:
                            MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.AsrPreset18EventHandler);
                            AsrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset18EventHandler);
                            AsrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset18EventHandler);
                            break;
                        case 18:
                            MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.AsrPreset19EventHandler);
                            AsrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset19EventHandler);
                            AsrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset19EventHandler);
                            break;
                        case 19:
                            MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.AsrPreset20EventHandler);
                            AsrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset20EventHandler);
                            AsrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.AsrPreset20EventHandler);
                            break;
                        default:
                            throw new Exception("Incorrect ASR hotkey slot: " + i + "!");
                    }
                }
            }
        }

        private static int FindFirstSlot(Guid[] presetGuids, Guid searchedGuid)
        {
            for (var i = 0; i < presetGuids.Length; i++)
            {
                if (presetGuids[i] == searchedGuid)
                    return i;
            }

            return -1;
        }

        protected static bool SetFileTag(string sourceFileUrl, MetaDataType tagId, string value, bool updateOnlyChangedTags, AdvancedSearchAndReplace plugin)
        {
            if (tagId == ClipboardTagId)
            {
                if (plugin == null)
                    return false;

                if (plugin.clipboardText == string.Empty)
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

            var sourceTagValue = GetTag(sourceFileUrl, plugin, (int)tagId);
            SetTag((int)tagId, value);

            if (updateOnlyChangedTags && sourceTagValue == value)
                return true;
            else
                return Plugin.SetFileTag(sourceFileUrl, tagId, value);
        }

        private void previewTable_AddRowToTable(object[] row, object changeTypes)
        {
            bool failed = true;
            while (failed && backgroundTaskIsWorking() && !previewIsStopped)
            {
                try
                {
                    previewTable.Rows.Add(row);
                    failed = false;
                }
                catch
                {
                    //Let's retry...
                    Thread.Sleep(ActionRetryDelay);
                }
            }

            if (backgroundTaskIsWorking() && !previewIsStopped)
            {
                for (var i = 0; i < 5; i++)
                    previewTable.Rows[previewTable.RowCount - 1].Cells[6 + i * 3].Tag = ((ChangesDetectionResult[])changeTypes)[i];

                previewTableFormatRow(previewTable.RowCount - 1);
            }

            if (previewTable.RowCount > 0)
                previewTable.FirstDisplayedScrollingRowIndex = previewTable.RowCount - 1;

            if ((previewTable.RowCount & 0x1f) == 0)
                updateCustomScrollBars(previewTable);
        }

        private void previewTable_ProcessRowOfTable(int rowIndex)
        {
            previewTable.Rows[rowIndex].Cells[0].Value = null;
            previewTableFormatRow(rowIndex);
        }

        private void previewTable_UpdateTable()
        {
            updateCustomScrollBars(previewTable);
            previewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (previewTable.RowCount > 0)
            {
                previewTable.FirstDisplayedScrollingRowIndex = previewTable.RowCount - 1;
                previewTable.CurrentCell = previewTable.Rows[0].Cells[0];
            }
        }

        internal static string Replace(string currentFile, string value, string searchedPattern, string replacedPattern, bool ignoreCase, out bool match)
        {
            match = false;
            if (searchedPattern == string.Empty)
                return value;


            try
            {
                match = Regex.IsMatch(value, searchedPattern, RegexOptions.IgnoreCase);

                if (!match)
                    return value;


                replacedPattern = replacedPattern.Replace(";", "\u0011").Replace("[[", "\u0017").Replace("]]", "\u0018");

                if (ignoreCase)
                    value = Regex.Replace(value, searchedPattern, replacedPattern, RegexOptions.IgnoreCase);
                else
                    value = Regex.Replace(value, searchedPattern, replacedPattern, RegexOptions.None);

                value = value.Replace("\u0017", "[[").Replace("\u0018", "]]").Replace("\u0011", ";");


                //Let's evaluate all supported functions
                var nullFunction = new Function();
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

                Function sortinvolvedpeoplebyrole = new SortInvolvedPeopleByRoles();
                value = sortinvolvedpeoplebyrole.evaluate(currentFile, value);

                Function searchreplace = new SearchReplace();
                value = searchreplace.evaluate(currentFile, value);
            }
            catch (Exception)
            {
                //MessageBox.Show(MusicBeePlugin.MbForm, ex.Message);

                return CtlAsrSyntaxError;
            }

            return value;
        }

        internal static string GetTag(string currentFile, AdvancedSearchAndReplace asrCommand, int tagId)
        {
            if (tagId == (int)ClipboardTagId)
            {
                if (asrCommand == null)
                    return string.Empty;

                if (asrCommand.fileTags == null)
                    return string.Empty;

                var position = -1;
                for (var i = 0; i < asrCommand.files.Length; i++)
                {
                    if (asrCommand.files[i] == currentFile)
                    {
                        position = i;
                        break;
                    }
                }

                if (position == -1)
                    return string.Empty;

                return asrCommand.fileTags[position];
            }


            SetTags.TryGetValue(tagId, out var cachedTagValue);

            if (cachedTagValue == null)
            {
                if (tagId < PropMetaDataThreshold)
                    return GetFileTag(currentFile, (MetaDataType)tagId);
                else
                    return MbApiInterface.Library_GetFileProperty(currentFile, (FilePropertyType)(tagId - PropMetaDataThreshold));
            }
            else
            {
                return cachedTagValue;
            }
        }

        internal static void SetTag(int tagId, string tagValue)
        {
            SetTags.AddReplace(tagId, tagValue);

            LastSetTagValue = tagValue;
        }

        internal static void SetReplacedTag(string currentFile, AdvancedSearchAndReplace asrCommand,
            int searchedTagId, int replacedTagId, string searchedPattern, string replacedPattern,
            string preserveValues, string preserveTags, string processTags, bool ignoreCase, bool append, bool? limitation,
            out string searchedTagValue, out string replacedTagValue, out string originalReplacedTagValue,
            out bool replacedTagValuePreserved, out bool replacedTagPreserved)
        {
            if (searchedPattern == string.Empty)
            {
                searchedTagValue = string.Empty;
                replacedTagValue = string.Empty;
                originalReplacedTagValue = string.Empty;
                replacedTagPreserved = false;
                replacedTagValuePreserved = false;
                return;
            }


            searchedTagValue = GetTag(currentFile, asrCommand, searchedTagId);
            originalReplacedTagValue = GetTag(currentFile, asrCommand, replacedTagId);


            if (searchedTagValue == string.Empty && limitation == false) //Don't replace with empty values
            {
                replacedTagValue = originalReplacedTagValue;
                replacedTagPreserved = false;
                replacedTagValuePreserved = false;
                return;
            }


            if (originalReplacedTagValue != string.Empty && limitation == true) //Don't replace not empty values
            {
                replacedTagValue = originalReplacedTagValue;
                replacedTagPreserved = false;
                replacedTagValuePreserved = false;
                return;
            }


            var replacedTagName = GetTagName((MetaDataType)replacedTagId);

            if (!string.IsNullOrWhiteSpace(processTags) && !(";" + processTags + ";").Contains(";" + replacedTagName + ";"))
            {
                replacedTagPreserved = true;
                replacedTagValuePreserved = false;
                replacedTagValue = originalReplacedTagValue;
                return;
            }

            if (!string.IsNullOrWhiteSpace(preserveTags) && (";" + preserveTags + ";").Contains(";" + replacedTagName + ";"))
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

        internal static SearchedAndReplacedTagsStruct GetReplacedTags(string currentFile, ref Preset preset, AdvancedSearchAndReplace asrCommand = null, bool copyPreset = true)
        {
            if (copyPreset)
                preset = new Preset(preset);

            var searchedAndReplacedTags = new SearchedAndReplacedTagsStruct();
            SetTags.Clear();

            var (count, _) = preset.getAllTagsReplaceTagIds();
            string processTags = null;
            string preserveTags = null;
            if (count > 0)
            {
                if (preset.processTagsMode)
                    processTags = preset.processTags;
                else
                    preserveTags = preset.preserveTags;
            }

            preset.searchedTagId = preset.substituteTagId(preset.searchedTagId);
            preset.replacedTagId = preset.substituteTagId(preset.replacedTagId);

            SetReplacedTag(currentFile, asrCommand, preset.searchedTagId, preset.replacedTagId,
                preset.replaceVariable(preset.searchedPattern, true), preset.replaceVariable(preset.replacedPattern, false),
                preset.preserveValues, preserveTags, processTags,
                preset.ignoreCase, preset.append, preset.limitation1,
                out searchedAndReplacedTags.searchedTagValue, out searchedAndReplacedTags.replacedTagValue, out searchedAndReplacedTags.originalReplacedTagValue,
                out searchedAndReplacedTags.replacedTagValuePreserved, out searchedAndReplacedTags.replacedTagPreserved);


            preset.searchedTag2Id = preset.substituteTagId(preset.searchedTag2Id);
            preset.replacedTag2Id = preset.substituteTagId(preset.replacedTag2Id);

            SetReplacedTag(currentFile, asrCommand, preset.searchedTag2Id, preset.replacedTag2Id,
                preset.replaceVariable(preset.searchedPattern2, true), preset.replaceVariable(preset.replacedPattern2, false),
                preset.preserveValues, preserveTags, processTags,
                preset.ignoreCase, preset.append2, preset.limitation2,
                out searchedAndReplacedTags.searchedTag2Value, out searchedAndReplacedTags.replacedTag2Value, out searchedAndReplacedTags.originalReplacedTag2Value,
                out searchedAndReplacedTags.replacedTag2ValuePreserved, out searchedAndReplacedTags.replacedTag2Preserved);


            preset.searchedTag3Id = preset.substituteTagId(preset.searchedTag3Id);
            preset.replacedTag3Id = preset.substituteTagId(preset.replacedTag3Id);

            SetReplacedTag(currentFile, asrCommand, preset.searchedTag3Id, preset.replacedTag3Id,
                preset.replaceVariable(preset.searchedPattern3, true), preset.replaceVariable(preset.replacedPattern3, false),
                preset.preserveValues, preserveTags, processTags,
                preset.ignoreCase, preset.append3, preset.limitation3,
                out searchedAndReplacedTags.searchedTag3Value, out searchedAndReplacedTags.replacedTag3Value, out searchedAndReplacedTags.originalReplacedTag3Value,
                out searchedAndReplacedTags.replacedTag3ValuePreserved, out searchedAndReplacedTags.replacedTag3Preserved);


            preset.searchedTag4Id = preset.substituteTagId(preset.searchedTag4Id);
            preset.replacedTag4Id = preset.substituteTagId(preset.replacedTag4Id);

            SetReplacedTag(currentFile, asrCommand, preset.searchedTag4Id, preset.replacedTag4Id,
                preset.replaceVariable(preset.searchedPattern4, true), preset.replaceVariable(preset.replacedPattern4, false),
                preset.preserveValues, preserveTags, processTags,
                preset.ignoreCase, preset.append4, preset.limitation4,
                out searchedAndReplacedTags.searchedTag4Value, out searchedAndReplacedTags.replacedTag4Value, out searchedAndReplacedTags.originalReplacedTag4Value,
                out searchedAndReplacedTags.replacedTag4ValuePreserved, out searchedAndReplacedTags.replacedTag4Preserved);


            preset.searchedTag5Id = preset.substituteTagId(preset.searchedTag5Id);
            preset.replacedTag5Id = preset.substituteTagId(preset.replacedTag5Id);

            SetReplacedTag(currentFile, asrCommand, preset.searchedTag5Id, preset.replacedTag5Id,
                preset.replaceVariable(preset.searchedPattern5, true), preset.replaceVariable(preset.replacedPattern5, false),
                preset.preserveValues, preserveTags, processTags,
                preset.ignoreCase, preset.append5, preset.limitation5,
                out searchedAndReplacedTags.searchedTag5Value, out searchedAndReplacedTags.replacedTag5Value, out searchedAndReplacedTags.originalReplacedTag5Value,
                out searchedAndReplacedTags.replacedTag5ValuePreserved, out searchedAndReplacedTags.replacedTag5Preserved);


            return searchedAndReplacedTags;
        }

        internal static void SaveReplacedTags(string currentFile, Preset preset, SearchedAndReplacedTagsStruct searchedAndReplacedTags, AdvancedSearchAndReplace asrCommand = null)
        {
            if (searchedAndReplacedTags.replacedTagValue == CtlAsrSyntaxError || searchedAndReplacedTags.replacedTag2Value == CtlAsrSyntaxError ||
                searchedAndReplacedTags.replacedTag3Value == CtlAsrSyntaxError || searchedAndReplacedTags.replacedTag4Value == CtlAsrSyntaxError ||
                searchedAndReplacedTags.replacedTag5Value == CtlAsrSyntaxError)
                return;

            SetTags.Clear();

            if (preset.searchedPattern != string.Empty)
            {
                SetFileTag(currentFile, (MetaDataType)preset.substituteTagId(preset.replacedTagId),
                    searchedAndReplacedTags.replacedTagValue, true, asrCommand);

                if (preset.searchedPattern2 != string.Empty)
                {
                    SetFileTag(currentFile, (MetaDataType)preset.substituteTagId(preset.replacedTag2Id),
                        searchedAndReplacedTags.replacedTag2Value, true, asrCommand);

                    if (preset.searchedPattern3 != string.Empty)
                    {
                        SetFileTag(currentFile, (MetaDataType)preset.substituteTagId(preset.replacedTag3Id),
                            searchedAndReplacedTags.replacedTag3Value, true, asrCommand);

                        if (preset.searchedPattern4 != string.Empty)
                        {
                            SetFileTag(currentFile, (MetaDataType)preset.substituteTagId(preset.replacedTag4Id),
                                searchedAndReplacedTags.replacedTag4Value, true, asrCommand);

                            if (preset.searchedPattern5 != string.Empty)
                            {
                                SetFileTag(currentFile, (MetaDataType)preset.substituteTagId(preset.replacedTag5Id),
                                    searchedAndReplacedTags.replacedTag5Value, true, asrCommand);
                            }
                        }
                    }
                }
            }


            CommitTagsToFile(currentFile, true, true);
        }

        internal static void ReplaceTags(string currentFile, Preset preset)
        {
            lock (Presets)
            {
                var searchedAndReplacedTags = GetReplacedTags(currentFile, ref preset);
                SaveReplacedTags(currentFile, preset, searchedAndReplacedTags);
            }
        }

        internal static void AsrAutoApplyPresets(string currentFile)
        {
            if (!SavedSettings.allowAsrLrPresetAutoExecution)
                return;
            else if (AsrAutoAppliedPresets.Count == 0)
                return;


            var appliedPresets = new SortedDictionary<Guid, bool>();

            lock (AsrAutoAppliedPresets)
            {
                foreach (var preset in AsrAutoAppliedPresets)
                {

                    if (preset.allTagsReplaceIdsCount > 0)
                        SetStatusBarText(MsgAsrPresetsUsingAllTagsPseudoTagNameCannotBeAutoApplied
                            .Replace("%%PRESET-NAME%%!", preset.getName()).Replace("%%AllTagsPseudoTagName%%", AllTagsPseudoTagName), true);

                    var conditionSatisfied = true;

                    if (preset.condition)
                    {
                        conditionSatisfied = false;

                        MbApiInterface.Playlist_QueryPlaylists();
                        var playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();

                        while (playlist != null)
                        {
                            if (playlist == preset.playlist)
                            {
                                conditionSatisfied = MbApiInterface.Playlist_IsInList(playlist, currentFile);
                                break;
                            }

                            playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();
                        }
                    }

                    if (conditionSatisfied)
                    {
                        ReplaceTags(currentFile, preset); //-V3156
                        appliedPresets.AddSkip(preset.guid);
                    }
                }

                if (appliedPresets.Count > 1)
                    SetStatusBarText(SbAsrPresetsAreApplied.Replace("%%PRESET-COUNT%%", appliedPresets.Count.ToString()), true);
                else if (appliedPresets.Count == 1)
                    SetStatusBarText(SbAsrPresetIsApplied.Replace("%%PRESET-NAME%%", Presets[appliedPresets.ElementAt(0).Key].getName()), true);

                RefreshPanels(true);
            }
        }

        internal static string GetLastReplacedTag(string currentFile, Preset preset)
        {
            var conditionSatisfied = true;

            if (preset.condition)
            {
                conditionSatisfied = false;

                MbApiInterface.Playlist_QueryPlaylists();
                var playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();

                while (playlist != null)
                {
                    if (playlist == preset.playlist)
                    {
                        conditionSatisfied = MbApiInterface.Playlist_IsInList(playlist, currentFile);
                        break;
                    }

                    playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();
                }
            }

            if (conditionSatisfied)
            {
                GetReplacedTags(currentFile, ref preset);
                return LastSetTagValue;
            }

            return string.Empty;
        }

        internal static void ApplyPreset(int presetIndex)
        {
            var preset = AsrPresetsWithHotkeys[presetIndex - 1];

            if (preset == null)
                return;

            if (preset.allTagsReplaceIdsCount > 0)
                SetStatusBarText(MsgAsrPresetsUsingAllTagsPseudoTagNameCannotBeAutoApplied
                    .Replace("%%PRESET-NAME%%!", preset.getName()).Replace("%%AllTagsPseudoTagName%%", AllTagsPseudoTagName), true);


            var conditionSatisfied = true;

            if (!preset.applyToPlayingTrack)
            {
                if (MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out var files))
                {
                    if (files.Length == 0)
                    {
                        MessageBox.Show(MbForm, MsgSelectTracks, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    for (var i = 0; i < files.Length; i++)
                    {
                        var currentFile = files[i];

                        if (preset.condition)
                        {
                            conditionSatisfied = false;

                            MbApiInterface.Playlist_QueryPlaylists();
                            var playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();

                            while (playlist != null)
                            {
                                if (playlist == preset.playlist)
                                {
                                    conditionSatisfied = MbApiInterface.Playlist_IsInList(playlist, currentFile);
                                    break;
                                }

                                playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();
                            }
                        }

                        if (conditionSatisfied)
                        {
                            ReplaceTags(currentFile, preset);
                            SetStatusBarText(SbAsrPresetIsApplied.Replace("%%PRESET-NAME%%", preset.getName()), true);
                        }
                    }
                }
            }
            else
            {
                var currentFile = MbApiInterface.NowPlaying_GetFileUrl();

                if (!string.IsNullOrEmpty(currentFile))
                {
                    if (preset.condition)
                    {
                        conditionSatisfied = false;

                        MbApiInterface.Playlist_QueryPlaylists();
                        var playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();

                        while (playlist != null)
                        {
                            if (playlist == preset.playlist)
                            {
                                conditionSatisfied = MbApiInterface.Playlist_IsInList(playlist, currentFile);
                                break;
                            }

                            playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();
                        }
                    }

                    if (conditionSatisfied)
                    {
                        ReplaceTags(currentFile, preset);
                        SetStatusBarText(SbAsrPresetIsApplied.Replace("%%PRESET-NAME%%", preset.getName()), true);
                    }
                }
            }

            RefreshPanels(true);
        }

        private bool prepareBackgroundPreview()
        {
            tags.Clear();

            if (previewTable.RowCount > 0)
            {
                bool failed = true;
                while (failed)
                {
                    try
                    {
                        previewTable.RowCount = 0;
                        failed = false;
                    }
                    catch
                    {
                        //Let's retry...
                        Thread.Sleep(ActionRetryDelay);
                    }
                }

                updateCustomScrollBars(previewTable);
            }

            (previewTable.Columns[0].HeaderCell as DataGridViewCheckBoxHeaderCell).setState(true);

            if (previewIsGenerated)
            {
                previewIsGenerated = false;
                return true;
            }

            previewIsGenerated = false;

            if (backgroundTaskIsWorking())
                return true;

            var allWritableTagNames = new List<string>();
            FillListByTagNames(allWritableTagNames, false, false, false);

            var allWritableTagIds = new List<int>();
            foreach (var tagName in allWritableTagNames)
                allWritableTagIds.Add((int)GetTagId(tagName));

            tagIdCombinations = generateParameterTagIdCombinationsForAllTagsPseudoTags(allWritableTagIds, selectedPreset);

            clipboardText = string.Empty;
            fileTags = null;



            files = null;
            if (!MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files))
                files = Array.Empty<string>();



            if (selectedPreset.searchedTagId == (int)ClipboardTagId || selectedPreset.searchedTag2Id == (int)ClipboardTagId || selectedPreset.searchedTag3Id == (int)ClipboardTagId
                || selectedPreset.searchedTag4Id == (int)ClipboardTagId || selectedPreset.searchedTag5Id == (int)ClipboardTagId)
            {
                if (!Clipboard.ContainsText())
                {
                    MessageBox.Show(this, MsgClipboardDoesntContainText, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                fileTags = Clipboard.GetText().Replace("\r\n", "\r").Replace("\n", "\r").Split(new[] { "\r" }, StringSplitOptions.None);

                if (fileTags.Length != files.Length)
                {
                    MessageBox.Show(this, MsgNumberOfTagsInClipboardDoesntCorrespondToNumberOfSelectedTracks
                        .Replace("%%FILE-TAGS-LENGTH%%", fileTags.Length.ToString())
                        .Replace("%%SELECTED-FILES-COUNT%%", files.Length.ToString()),
                        string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }


            if (files.Length == 0)
            {
                MessageBox.Show(this, MsgNoTracksSelected, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else
            {
                previewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
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
                for (var fileCounter = 0; fileCounter < previewTable.RowCount; fileCounter++)
                    tags[fileCounter][0] = (string)previewTable.Rows[fileCounter].Cells[0].Value;

                return true;
            }
        }

        private struct TagIdCombination
        {
            internal int parameterTagId;
            internal int parameterTag2Id;
            internal int parameterTag3Id;
            internal int parameterTag4Id;
            internal int parameterTag5Id;
            internal int parameterTag6Id;
        }

        private Dictionary<TagIdCombination, bool> generateParameterTagIdCombinationsForAllTagsPseudoTags(List<int> allWritableTagIds, Preset preset)
        {
            var tagIdCombinationsForAllTagsPseudoTag = new Dictionary<TagIdCombination, bool>();

            if (preset.parameterTagId == (int)AllTagsPseudoTagId)
            {
                foreach (var tagId1 in allWritableTagIds)
                {
                    if (preset.parameterTag2Id == (int)AllTagsPseudoTagId)
                    {
                        foreach (var tagId2 in allWritableTagIds)
                        {
                            if (preset.parameterTag3Id == (int)AllTagsPseudoTagId)
                            {
                                foreach (var tagId3 in allWritableTagIds)
                                {
                                    if (preset.parameterTag4Id == (int)AllTagsPseudoTagId)
                                    {
                                        foreach (var tagId4 in allWritableTagIds)
                                        {
                                            if (preset.parameterTag5Id == (int)AllTagsPseudoTagId)
                                            {
                                                foreach (var tagId5 in allWritableTagIds)
                                                {
                                                    if (preset.parameterTag6Id == (int)AllTagsPseudoTagId)
                                                    {
                                                        foreach (var tagId6 in allWritableTagIds)
                                                        {
                                                            TagIdCombination tagCombination;
                                                            tagCombination.parameterTagId = tagId1;
                                                            tagCombination.parameterTag2Id = tagId2;
                                                            tagCombination.parameterTag3Id = tagId3;
                                                            tagCombination.parameterTag4Id = tagId4;
                                                            tagCombination.parameterTag5Id = tagId5;
                                                            tagCombination.parameterTag6Id = tagId6;

                                                            tagIdCombinationsForAllTagsPseudoTag.Add(tagCombination, false);
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
                                                        tagCombination.parameterTag6Id = preset.parameterTag6Id;

                                                        tagIdCombinationsForAllTagsPseudoTag.Add(tagCombination, false);
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
                                                tagCombination.parameterTag5Id = preset.parameterTag5Id;
                                                tagCombination.parameterTag6Id = preset.parameterTag6Id;

                                                tagIdCombinationsForAllTagsPseudoTag.Add(tagCombination, false);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        TagIdCombination tagCombination;
                                        tagCombination.parameterTagId = tagId1;
                                        tagCombination.parameterTag2Id = tagId2;
                                        tagCombination.parameterTag3Id = tagId3;
                                        tagCombination.parameterTag4Id = preset.parameterTag4Id;
                                        tagCombination.parameterTag5Id = preset.parameterTag5Id;
                                        tagCombination.parameterTag6Id = preset.parameterTag6Id;

                                        tagIdCombinationsForAllTagsPseudoTag.Add(tagCombination, false);
                                    }
                                }
                            }
                            else
                            {
                                TagIdCombination tagCombination;
                                tagCombination.parameterTagId = tagId1;
                                tagCombination.parameterTag2Id = tagId2;
                                tagCombination.parameterTag3Id = preset.parameterTag3Id;
                                tagCombination.parameterTag4Id = preset.parameterTag4Id;
                                tagCombination.parameterTag5Id = preset.parameterTag5Id;
                                tagCombination.parameterTag6Id = preset.parameterTag6Id;

                                tagIdCombinationsForAllTagsPseudoTag.Add(tagCombination, false);
                            }
                        }
                    }
                    else
                    {
                        TagIdCombination tagCombination;
                        tagCombination.parameterTagId = tagId1;
                        tagCombination.parameterTag2Id = preset.parameterTag2Id;
                        tagCombination.parameterTag3Id = preset.parameterTag3Id;
                        tagCombination.parameterTag4Id = preset.parameterTag4Id;
                        tagCombination.parameterTag5Id = preset.parameterTag5Id;
                        tagCombination.parameterTag6Id = preset.parameterTag6Id;

                        tagIdCombinationsForAllTagsPseudoTag.Add(tagCombination, false);
                    }
                }
            }
            else
            {
                TagIdCombination tagCombination;
                tagCombination.parameterTagId = preset.parameterTagId;
                tagCombination.parameterTag2Id = preset.parameterTag2Id;
                tagCombination.parameterTag3Id = preset.parameterTag3Id;
                tagCombination.parameterTag4Id = preset.parameterTag4Id;
                tagCombination.parameterTag5Id = preset.parameterTag5Id;
                tagCombination.parameterTag6Id = preset.parameterTag6Id;

                tagIdCombinationsForAllTagsPseudoTag.Add(tagCombination, false);
            }

            return tagIdCombinationsForAllTagsPseudoTag;
        }

        private SortedDictionary<string, Preset> generatePresetsForAllTagsPseudoTags(Preset preset)
        {
            var allTagsPseudoTagPresets = new SortedDictionary<string, Preset>();


            foreach (var tagsCombination in tagIdCombinations.Keys)
            {
                var combinationPreset = new Preset(preset, true, false)
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

        private enum ChangesDetectionResult
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
            var leaveNotChanged = !SavedSettings.dontIncludeInPreviewLinesWithoutChangedTags;
            var leavePreservedTagsResults = !SavedSettings.dontIncludeInPreviewLinesWithPreservedTagsAsr;
            var leavePreservedValuesResults = !SavedSettings.dontIncludeInPreviewLinesWithPreservedTagValuesAsr;

            var result = ChangesDetectionResult.Skip;

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
            var res = 100;
            var res1 = (int)result1;
            var res2 = (int)result2;
            var res3 = (int)result3;
            var res4 = (int)result4;
            var res5 = (int)result5;

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
        //1st result: false - skip, true - proceed
        {
            var result1 = ChangesDetectionResult.NoExclusionsDetected;
            var result2 = ChangesDetectionResult.NoExclusionsDetected;
            var result3 = ChangesDetectionResult.NoExclusionsDetected;
            var result4 = ChangesDetectionResult.NoExclusionsDetected;
            var result5 = ChangesDetectionResult.NoExclusionsDetected;

            if (processingPreset.searchedPattern == string.Empty || DetectTempTag(processingPreset.replacedTagId))
            {
                result1 = ChangesDetectionResult.Ignore;
            }
            else if (searchedAndReplacedTags.originalReplacedTagValue == searchedAndReplacedTags.replacedTagValue)
            {
                result1 = DetectPresetStepChanges(searchedAndReplacedTags.replacedTagPreserved, searchedAndReplacedTags.replacedTagValuePreserved);
            }

            if (processingPreset.searchedPattern2 == string.Empty || DetectTempTag(processingPreset.replacedTag2Id))
            {
                result2 = ChangesDetectionResult.Ignore;
            }
            else if (searchedAndReplacedTags.originalReplacedTag2Value == searchedAndReplacedTags.replacedTag2Value)
            {
                result2 = DetectPresetStepChanges(searchedAndReplacedTags.replacedTag2Preserved, searchedAndReplacedTags.replacedTag2ValuePreserved);
            }

            if (processingPreset.searchedPattern3 == string.Empty || DetectTempTag(processingPreset.replacedTag3Id))
            {
                result3 = ChangesDetectionResult.Ignore;
            }
            else if (searchedAndReplacedTags.originalReplacedTag3Value == searchedAndReplacedTags.replacedTag3Value)
            {
                result3 = DetectPresetStepChanges(searchedAndReplacedTags.replacedTag3Preserved, searchedAndReplacedTags.replacedTag3ValuePreserved);
            }

            if (processingPreset.searchedPattern4 == string.Empty || DetectTempTag(processingPreset.replacedTag4Id))
            {
                result4 = ChangesDetectionResult.Ignore;
            }
            else if (searchedAndReplacedTags.originalReplacedTag4Value == searchedAndReplacedTags.replacedTag4Value)
            {
                result4 = DetectPresetStepChanges(searchedAndReplacedTags.replacedTag4Preserved, searchedAndReplacedTags.replacedTag4ValuePreserved);
            }

            if (processingPreset.searchedPattern5 == string.Empty || DetectTempTag(processingPreset.replacedTag5Id))
            {
                result5 = ChangesDetectionResult.Ignore;
            }
            else if (searchedAndReplacedTags.originalReplacedTag5Value == searchedAndReplacedTags.replacedTag5Value)
            {
                result5 = DetectPresetStepChanges(searchedAndReplacedTags.replacedTag5Preserved, searchedAndReplacedTags.replacedTag5ValuePreserved);
            }

            var minResult = Min(result1, result2, result3, result4, result5);
            if (minResult == ChangesDetectionResult.Skip || minResult == ChangesDetectionResult.Ignore)
                return (false, minResult, result1, result2, result3, result4, result5);
            else
                return (true, minResult, result1, result2, result3, result4, result5);
        }

        private void previewChanges()
        {
            // ReSharper disable once RedundantAssignment
            string[] tag = { "Checked", "Preset GUID", "URL", "newTag1", "newTag2", "newTag3", "newTag4", "newTag5" };

            // ReSharper disable once RedundantAssignment
            string[] row = { "Checked", "Preset GUID", "URL", "Track",
                "TagName1", "OriginalTag1", "NewTag1", "TagName2", "OriginalTag2", "NewTag2",
                "TagName3", "OriginalTag3", "NewTag3", "TagName4", "OriginalTag4", "NewTag4",
                "TagName5", "OriginalTag5", "NewTag5", "even/odd indicator" };


            presetProcessingCopies = generatePresetsForAllTagsPseudoTags(selectedPreset);

            string lastFile = null;
            var even = true;

            lock (Presets)
            {
                foreach (var processingCopyGuidPreset in presetProcessingCopies)
                {
                    var processingPresetCopy = processingCopyGuidPreset.Value;

                    for (var fileCounter = 0; fileCounter < files.Length; fileCounter++)
                    {
                        if (backgroundTaskIsCanceled)
                            return;

                        var currentFile = files[fileCounter];
                        if (lastFile != currentFile)
                        {
                            lastFile = currentFile;
                            even = !even;
                        }

                        SetStatusBarTextForFileOperations(AsrSbText, true, fileCounter, files.Length, currentFile);

                        var track = GetTrackRepresentation(currentFile);

                        var searchedAndReplacedTags = GetReplacedTags(currentFile, ref processingPresetCopy, this, false);

                        var (proceed, minResult, changeType1, changeType2, changeType3, changeType4, changeType5)

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


                            var changeTypes = new ChangesDetectionResult[5];

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


            Invoke(updateTable);
            SetResultingSbText();
        }

        private void applyToSelected()
        {
            if (tags.Count == 0)
                previewChanges();

            for (var i = 0; i < tags.Count; i++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                var isChecked = tags[i][0];
                var combinationPreset = presetProcessingCopies[tags[i][1]];
                var currentFile = tags[i][2];

                if (isChecked == "T")
                {
                    lock (Presets)
                    {
                        var searchedAndReplacedTags = new SearchedAndReplacedTagsStruct();
                        tags[i][0] = string.Empty;

                        searchedAndReplacedTags.replacedTagValue = tags[i][3];
                        searchedAndReplacedTags.replacedTag2Value = tags[i][4];
                        searchedAndReplacedTags.replacedTag3Value = tags[i][5];
                        searchedAndReplacedTags.replacedTag4Value = tags[i][6];
                        searchedAndReplacedTags.replacedTag5Value = tags[i][7];

                        SaveReplacedTags(currentFile, combinationPreset, searchedAndReplacedTags, this);
                    }

                    Invoke(processRowOfTable, new object[] { i });
                }

                SetStatusBarTextForFileOperations(AsrSbText, false, i, tags.Count, currentFile);
            }


            if (selectedPreset.replacedTagId == (int)ClipboardTagId || selectedPreset.replacedTag2Id == (int)ClipboardTagId || selectedPreset.replacedTag3Id == (int)ClipboardTagId
                || selectedPreset.replacedTag4Id == (int)ClipboardTagId || selectedPreset.replacedTag5Id == (int)ClipboardTagId)
            {
                var thread = new Thread(() => Clipboard.SetText(clipboardText));
                thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                thread.Start();
                thread.Join();
            }


            RefreshPanels(true);
            SetResultingSbText();
        }

        internal static string AsrGetTagName(int tagId)
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

        internal static int AsrGetTagId(string tagName)
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

        private void editPreset(Preset preset, bool itsNewPreset, bool readOnly)
        {
            bool presetChanged;
            using (var tagToolsForm = new AsrPresetEditor(TagToolsPlugin))
                presetChanged = tagToolsForm.editPreset(preset, readOnly);

            if (presetChanged)
            {
                if (!readOnly)
                {
                    preset.modifiedUtc = DateTime.UtcNow;
                    unsavedChanges = true;
                    preset.changed = true;

                    buttonClose.Image = warningWide;
                    toolTip1.SetToolTip(buttonClose, buttonCloseToolTip);
                }
                else
                {
                    preset.setCustomizationsFlag(this, backedUpPreset);
                }

                if (itsNewPreset)
                {
                    presetsWorkingCopy.Add(preset.guid, preset);

                    presetList.Items.Add(preset);

                    presetListLastSelectedIndex = -1;
                    refreshPresetList(preset.guid);
                }
                else// if (preset.getName() != backedUpPresetName || descriptionBox.Text != GetDictValue(preset.descriptions, Language)
                    //|| preset.userPreset != backedUpPreset.userPreset || preset.customizedByUser != backedUpPreset.customizedByUser)
                {
                    refreshPresetList(preset.guid);
                }
            }
        }

        private string getCountedPresetFilename(SortedDictionary<string, int> countedPresetFilenames, string presetFilename)
        {
            if (countedPresetFilenames.TryGetValue(presetFilename, out var count))
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
            var savedPresetPaths = new SortedDictionary<string, bool>();
            var countedPresetFilenames = new SortedDictionary<string, int>();
            foreach (var preset in presetsWorkingCopy.Values)
            {
                var presetFilename = getCountedPresetFilename(countedPresetFilenames, preset.getSafeFileName());
                savedPresetPaths.Add(preset.savePreset(Path.Combine(PresetsPath, presetFilename + AsrPresetExtension)), false);
            }

            if (MSR != null)
            {
                var presetFilename = getCountedPresetFilename(countedPresetFilenames, MSR.getSafeFileName());
                savedPresetPaths.Add(MSR.savePreset(Path.Combine(PresetsPath, presetFilename + AsrPresetExtension)), false);
            }


            var presetNames = Directory.GetFiles(PresetsPath, "*");
            foreach (var presetName in presetNames)
            {
                if (!savedPresetPaths.ContainsKey(presetName))
                    System.IO.File.Delete(presetName);
            }


            Presets = new SortedDictionary<Guid, Preset>();
            foreach (var preset in presetsWorkingCopy.Values)
            {
                preset.changed = false;
                var presetCopy = new Preset(preset);
                Presets.Add(presetCopy.guid, presetCopy);
            }

            AsrPresetsWithHotkeysCount = asrPresetsWithHotkeysCount;

            SavedSettings.asrPresetsWithHotkeysGuids = new Guid[MaximumNumberOfAsrHotkeys];
            AsrPresetsWithHotkeys = new Preset[MaximumNumberOfAsrHotkeys];
            for (var j = 0; j < MaximumNumberOfAsrHotkeys; j++)
            {
                SavedSettings.asrPresetsWithHotkeysGuids[j] = asrPresetsWithHotkeysGuids[j];

                if (asrPresetsWithHotkeysGuids[j] != Guid.Empty)
                    AsrPresetsWithHotkeys[j] = Presets[asrPresetsWithHotkeysGuids[j]];
                else
                    AsrPresetsWithHotkeys[j] = null;
            }

            SavedSettings.autoAppliedAsrPresetGuids = new List<Guid>();
            AsrAutoAppliedPresets = new List<Preset>();
            foreach (var guid in autoAppliedAsrPresetGuids.Keys)
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

            InitAsr();

            TagToolsPlugin.addPluginContextMenuItems();
            TagToolsPlugin.addPluginMenuItems();


            unsavedChanges = false;
            buttonClose.Image = Resources.transparent_15;
            toolTip1.SetToolTip(buttonClose, string.Empty);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (prepareBackgroundTask())
                switchOperation(applyToSelected, sender as Button, buttonOK, buttonPreview, buttonClose, true, null);
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewChanges, sender as Button, buttonOK, buttonClose);

            if (previewTable.RowCount > 0)
                previewTable.FirstDisplayedScrollingRowIndex = previewTable.RowCount - 1;
        }

        private void buttonSaveClose_Click(object sender, EventArgs e)
        {
            if (!backgroundTaskIsScheduled)
                saveSettings();

            if (ModifierKeys != Keys.Control)
                Close();
            else
                presetList.Refresh();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            forceCloseForms = false;
            Close();
        }

        private void deletePreset(Preset presetToRemove)
        {
            if (autoAppliedAsrPresetGuids.RemoveExisting(presetToRemove.guid))
                autoAppliedPresetCount--;



            idTextBox.Text = string.Empty;
            asrIdsPresetGuids.RemoveExisting(presetToRemove.id);


            if (presetList.SelectedItem == presetToRemove)
            {
                assignHotkeyCheckBox.Checked = false;
            }
            else
            {
                for (var j = 0; j < asrPresetsWithHotkeysGuids.Length; j++)
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

            unsavedChanges = true;
            buttonClose.Image = warningWide;
            toolTip1.SetToolTip(buttonClose, buttonCloseToolTip);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(this, MsgDeletePresetConfirmation, string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No)
                return;

            deletePreset(presetList.SelectedItem as Preset);
            showAutoApplyingWarningIfRequired();
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            editPreset(new Preset(selectedPreset, false, false, null, "*"), true, false);
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            editPreset(selectedPreset, false, false);
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            var newPreset = new Preset
            {
                names = new SerializableDictionary<string, string>(),
                descriptions = new SerializableDictionary<string, string>(),
                userPreset = true
            };

            newPreset.names.Add(Language, DefaultAsrPresetName + (presetList.Items.Count + 1));
            newPreset.ignoreCase = true;

            editPreset(newPreset, true, false);
        }

        internal static TagType GetTagTypeByPresetParameterTagType(int presetParameterTagTypeOld)
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

        internal static void FillParameterTagList(TagType parameterTagType, string selectedTagName, CustomComboBox parameterTagList, Label parameterTagLabel)
        {
            parameterTagList.ItemsClear();

            if (parameterTagType == TagType.NotUsed) //Not used
            {
                selectedTagName = string.Empty;
                parameterTagList.Enable(false);
                parameterTagLabel.Enable(false);
            }
            else if (parameterTagType == TagType.Readonly) //Read only
            {
                FillListByTagNames(parameterTagList.Items, true);
                FillListByPropNames(parameterTagList.Items);
                parameterTagList.Enable(true);
                parameterTagLabel.Enable(true);
            }
            else if (parameterTagType == TagType.Writable) //Writable
            {
                FillListByTagNames(parameterTagList.Items);
                parameterTagList.Enable(true);
                parameterTagLabel.Enable(true);
            }
            else //Writable (incl. <All Tags>)
            {
                FillListByTagNames(parameterTagList.Items, false, false, true, false, true);
                parameterTagList.Enable(true);
                parameterTagLabel.Enable(true);
            }


            if (parameterTagList.Items.Contains(selectedTagName))
                parameterTagList.SelectedItem = selectedTagName;
            else if (parameterTagList.Items.Count > 0)
                parameterTagList.SelectedIndex = 0;
            else
                parameterTagList.SelectedIndex = -1;
        }

        private static bool DetectTempTag(int tagId)
        {
            if (tagId <= (int)ServiceMetaData.TempTag1 || tagId == (int)NullTagId)
                return true;
            else
                return false;
        }

        //Returns if column weights have been changed from saved settings
        internal void nameColumns()
        {
            if (selectedPreset == null)
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


            previewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            if (selectedPreset.columnWeights == null)
                selectedPreset.columnWeights = new List<float>();

            if (selectedPreset.columnWeights.Count < 4 || !(selectedPreset.columnWeights[1] > 0))
            {
                selectedPreset.columnWeights.Clear();
            }
            else if ((int)selectedPreset.columnWeights[3] == selectedPreset.columnWeights[3])
            {
                previewTable.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                previewTable.Columns[3].Width = (int)selectedPreset.columnWeights[3];
            }

            for (int i = 0; i < selectedPreset.columnWeights.Count - 1; i++)
                if (!(selectedPreset.columnWeights[i] > 0))
                    selectedPreset.columnWeights[i] = 20;


            if (allTagsReplaceIdsCount == 0)
                previewTable.Columns[19].Visible = false;
            else
                previewTable.Columns[19].Visible = true;

            if (selectedPreset.searchedPattern != string.Empty)
            {
                previewTable.Columns[4].Visible = allTagsPresetParameterTagMask[0];
                previewTable.Columns[4].HeaderText = TagNameText;
                previewTable.Columns[5].HeaderText = GetTagName((MetaDataType)selectedPreset.substituteTagId(selectedPreset.replacedTagId), OrigValueText);
                previewTable.Columns[6].HeaderText = NewValueText;

                if (DetectTempTag(selectedPreset.replacedTagId))
                {
                    previewTable.Columns[5].Visible = false;
                    previewTable.Columns[6].Visible = false;
                }
                else
                {
                    previewTable.Columns[5].Visible = true;
                    previewTable.Columns[6].Visible = true;
                }

                if (selectedPreset.columnWeights.Count > 3)
                {
                    //previewTable.Columns[3].FillWeight = selectedPreset.columnWeights[0];
                    previewTable.Columns[4].FillWeight = selectedPreset.columnWeights[1];
                    previewTable.Columns[5].FillWeight = selectedPreset.columnWeights[2];
                    //previewTable.Columns[6].FillWeight = selectedPreset.columnWeights[3]; //****
                }
                else
                {
                    previewTable.Columns[3].Width = ColumnsTrackWidth;
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


                if (selectedPreset.searchedPattern2 != string.Empty)
                {
                    previewTable.Columns[7].Visible = allTagsPresetParameterTagMask[1];
                    previewTable.Columns[7].HeaderText = TagNameText;
                    previewTable.Columns[8].HeaderText = GetTagName((MetaDataType)selectedPreset.substituteTagId(selectedPreset.replacedTag2Id), OrigValueText);
                    previewTable.Columns[9].HeaderText = NewValueText;

                    if (DetectTempTag(selectedPreset.replacedTag2Id))
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

                    if (selectedPreset.columnWeights.Count > 6)
                    {
                        previewTable.Columns[7].FillWeight = selectedPreset.columnWeights[4];
                        previewTable.Columns[8].FillWeight = selectedPreset.columnWeights[5];
                        previewTable.Columns[9].FillWeight = selectedPreset.columnWeights[6];
                    }


                    if (selectedPreset.searchedPattern3 != string.Empty)
                    {
                        previewTable.Columns[10].Visible = allTagsPresetParameterTagMask[2];
                        previewTable.Columns[10].HeaderText = TagNameText;
                        previewTable.Columns[11].HeaderText = GetTagName((MetaDataType)selectedPreset.substituteTagId(selectedPreset.replacedTag3Id), OrigValueText);
                        previewTable.Columns[12].HeaderText = NewValueText;

                        if (DetectTempTag(selectedPreset.replacedTag3Id))
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

                        if (selectedPreset.columnWeights.Count > 9)
                        {
                            previewTable.Columns[10].FillWeight = selectedPreset.columnWeights[7];
                            previewTable.Columns[11].FillWeight = selectedPreset.columnWeights[8];
                            previewTable.Columns[12].FillWeight = selectedPreset.columnWeights[9];
                        }


                        if (selectedPreset.searchedPattern4 != string.Empty)
                        {
                            previewTable.Columns[13].Visible = allTagsPresetParameterTagMask[3];
                            previewTable.Columns[13].HeaderText = TagNameText;
                            previewTable.Columns[14].HeaderText = GetTagName((MetaDataType)selectedPreset.substituteTagId(selectedPreset.replacedTag4Id), OrigValueText);
                            previewTable.Columns[15].HeaderText = NewValueText;

                            if (DetectTempTag(selectedPreset.replacedTag4Id))
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

                            if (selectedPreset.columnWeights.Count > 12)
                            {
                                previewTable.Columns[13].FillWeight = selectedPreset.columnWeights[10];
                                previewTable.Columns[14].FillWeight = selectedPreset.columnWeights[11];
                                previewTable.Columns[15].FillWeight = selectedPreset.columnWeights[12];
                            }


                            if (selectedPreset.searchedPattern5 != string.Empty)
                            {
                                previewTable.Columns[16].Visible = allTagsPresetParameterTagMask[4];
                                previewTable.Columns[16].HeaderText = TagNameText;
                                previewTable.Columns[17].HeaderText = GetTagName((MetaDataType)selectedPreset.substituteTagId(selectedPreset.replacedTag5Id), OrigValueText);
                                previewTable.Columns[18].HeaderText = NewValueText;

                                if (DetectTempTag(selectedPreset.replacedTag5Id))
                                {
                                    previewTable.Columns[17].Visible = false;
                                    previewTable.Columns[18].Visible = false;
                                }
                                else
                                {
                                    previewTable.Columns[17].Visible = (previewTable.Columns[14].HeaderText != string.Empty);
                                    previewTable.Columns[18].Visible = (previewTable.Columns[14].HeaderText != string.Empty);
                                }

                                if (previewTable.Columns[17].HeaderText == previewTable.Columns[14].HeaderText)
                                {
                                    previewTable.Columns[15].Visible = false;
                                    previewTable.Columns[16].Visible = false;
                                    previewTable.Columns[17].Visible = false;
                                }

                                if (selectedPreset.columnWeights.Count > 15)
                                {
                                    previewTable.Columns[16].FillWeight = selectedPreset.columnWeights[13];
                                    previewTable.Columns[17].FillWeight = selectedPreset.columnWeights[14];
                                    previewTable.Columns[18].FillWeight = selectedPreset.columnWeights[15];
                                }
                            }
                            else
                            {
                                previewTable.Columns[16].HeaderText = string.Empty;
                                previewTable.Columns[17].HeaderText = string.Empty;
                                previewTable.Columns[18].HeaderText = string.Empty;

                                previewTable.Columns[16].Visible = false;
                                previewTable.Columns[17].Visible = false;
                                previewTable.Columns[18].Visible = false;
                            }
                        }
                        else
                        {
                            previewTable.Columns[13].HeaderText = string.Empty;
                            previewTable.Columns[14].HeaderText = string.Empty;
                            previewTable.Columns[15].HeaderText = string.Empty;
                            previewTable.Columns[16].HeaderText = string.Empty;
                            previewTable.Columns[17].HeaderText = string.Empty;
                            previewTable.Columns[18].HeaderText = string.Empty;

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
                        previewTable.Columns[10].HeaderText = string.Empty;
                        previewTable.Columns[11].HeaderText = string.Empty;
                        previewTable.Columns[12].HeaderText = string.Empty;
                        previewTable.Columns[13].HeaderText = string.Empty;
                        previewTable.Columns[14].HeaderText = string.Empty;
                        previewTable.Columns[15].HeaderText = string.Empty;
                        previewTable.Columns[16].HeaderText = string.Empty;
                        previewTable.Columns[17].HeaderText = string.Empty;
                        previewTable.Columns[18].HeaderText = string.Empty;

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
                    previewTable.Columns[7].HeaderText = string.Empty;
                    previewTable.Columns[8].HeaderText = string.Empty;
                    previewTable.Columns[9].HeaderText = string.Empty;
                    previewTable.Columns[10].HeaderText = string.Empty;
                    previewTable.Columns[11].HeaderText = string.Empty;
                    previewTable.Columns[12].HeaderText = string.Empty;
                    previewTable.Columns[13].HeaderText = string.Empty;
                    previewTable.Columns[14].HeaderText = string.Empty;
                    previewTable.Columns[15].HeaderText = string.Empty;
                    previewTable.Columns[16].HeaderText = string.Empty;
                    previewTable.Columns[17].HeaderText = string.Empty;
                    previewTable.Columns[18].HeaderText = string.Empty;

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
                previewTable.Columns[4].HeaderText = string.Empty;
                previewTable.Columns[5].HeaderText = string.Empty;
                previewTable.Columns[6].HeaderText = string.Empty;
                previewTable.Columns[7].HeaderText = string.Empty;
                previewTable.Columns[8].HeaderText = string.Empty;
                previewTable.Columns[9].HeaderText = string.Empty;
                previewTable.Columns[10].HeaderText = string.Empty;
                previewTable.Columns[11].HeaderText = string.Empty;
                previewTable.Columns[12].HeaderText = string.Empty;
                previewTable.Columns[13].HeaderText = string.Empty;
                previewTable.Columns[14].HeaderText = string.Empty;
                previewTable.Columns[15].HeaderText = string.Empty;
                previewTable.Columns[16].HeaderText = string.Empty;
                previewTable.Columns[17].HeaderText = string.Empty;
                previewTable.Columns[18].HeaderText = string.Empty;

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
                label.Image = checkedState;
            else
                label.Image = uncheckedState;

            presetList.Refresh();
        }

        private void presetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (presetListLastSelectedIndex == presetList.SelectedIndex)
                return;

            if (previewIsGenerated)
                clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewChanges, buttonPreview, buttonOK, buttonClose);

            processPresetChanges = false;
            presetListSelectedIndexChanged(presetList.SelectedIndex);
            presetListLastSelectedIndex = presetList.SelectedIndex;
            processPresetChanges = true;
        }

        private void presetListSelectedIndexChanged(int index)
        {
            if (presetListLastSelectedIndex != -2) //It's during form init
                Enable(false, autoApplyPresetsLabel);

            if (index < 0)
            {
                selectedPreset = null;
                backedUpPreset = null;

                descriptionBox.Text = string.Empty;

                setCheckedState(userPresetPictureBox, false);
                setCheckedState(customizedPresetPictureBox, false);

                preserveTagValuesTextBox.Text = string.Empty;

                allTagsReplaceIdsCount = 0;
                allTagsPresetParameterTagMask = new[] { false, false, false, false, false };

                processTagsMode = true;
                buttonProcessPreserveTags.Text = AsrProcessTagsButtonName;
                processPreserveTagsTextBox.Text = string.Empty;

                parameterTagListCustom.ItemsClear();
                parameterTag2ListCustom.ItemsClear();
                parameterTag3ListCustom.ItemsClear();
                parameterTag4ListCustom.ItemsClear();
                parameterTag5ListCustom.ItemsClear();
                parameterTag6ListCustom.ItemsClear();


                conditionCheckBox.Checked = false;
                assignHotkeyCheckBox.Checked = false;
                applyToPlayingTrackCheckBox.Checked = false;
                favoriteCheckBox.Checked = false;

                enableDisablePreviewOptionControls(true, true);
                disableQueryingOrUpdatingButtons();

                if (presetListLastSelectedIndex != -2) //It's during form init
                    Enable(true, autoApplyPresetsLabel);
            }
            else
            {
                presetsWorkingCopy.TryGetValue((presetList.Items[index] as Preset).guid, out selectedPreset);
                backedUpPreset = new Preset(selectedPreset);

                if (!selectedPreset.userPreset && !DeveloperMode)
                    editButtonEnabled = false;
                else
                    editButtonEnabled = true;

                descriptionBox.Text = GetDictValue(selectedPreset.descriptions, Language);

                setCheckedState(userPresetPictureBox, selectedPreset.userPreset);
                setCheckedState(customizedPresetPictureBox, selectedPreset.getCustomizationsFlag());


                preserveTagValuesTextBox.Text = selectedPreset.preserveValues;

                (allTagsReplaceIdsCount, allTagsPresetParameterTagMask) = selectedPreset.getAllTagsReplaceTagIds();

                processTagsMode = selectedPreset.processTagsMode;
                if (processTagsMode)
                {
                    buttonProcessPreserveTags.Text = AsrProcessTagsButtonName;
                    processPreserveTagsTextBox.Text = selectedPreset.processTags;
                }
                else
                {
                    buttonProcessPreserveTags.Text = AsrPreserveTagsButtonName;
                    processPreserveTagsTextBox.Text = selectedPreset.preserveTags;
                }


                FillParameterTagList(selectedPreset.parameterTagTypeNew, AsrGetTagName(selectedPreset.parameterTagId), parameterTagListCustom, labelTag);
                FillParameterTagList(selectedPreset.parameterTag2TypeNew, AsrGetTagName(selectedPreset.parameterTag2Id), parameterTag2ListCustom, labelTag2);
                FillParameterTagList(selectedPreset.parameterTag3TypeNew, AsrGetTagName(selectedPreset.parameterTag3Id), parameterTag3ListCustom, labelTag3);
                FillParameterTagList(selectedPreset.parameterTag4TypeNew, AsrGetTagName(selectedPreset.parameterTag4Id), parameterTag4ListCustom, labelTag4);
                FillParameterTagList(selectedPreset.parameterTag5TypeNew, AsrGetTagName(selectedPreset.parameterTag5Id), parameterTag5ListCustom, labelTag5);
                FillParameterTagList(selectedPreset.parameterTag6TypeNew, AsrGetTagName(selectedPreset.parameterTag6Id), parameterTag6ListCustom, labelTag6);

                customTextBox.Text = selectedPreset.customTextChecked ? selectedPreset.customText : string.Empty;
                customText2Box.Text = selectedPreset.customText2Checked ? selectedPreset.customText2 : string.Empty;
                customText3Box.Text = selectedPreset.customText3Checked ? selectedPreset.customText3 : string.Empty;
                customText4Box.Text = selectedPreset.customText4Checked ? selectedPreset.customText4 : string.Empty;

                var hotkeyAssigned = selectedPreset.hotkeyAssigned;
                if (asrPresetsWithHotkeysCount >= MaximumNumberOfAsrHotkeys && !hotkeyAssigned)
                {
                    assignHotkeyCheckBox.Enable(false);
                    assignHotkeyCheckBox.Checked = false;
                }
                else
                {
                    assignHotkeyCheckBox.Enable(true);
                    assignHotkeyCheckBox.Checked = hotkeyAssigned;
                }

                applyToPlayingTrackCheckBox.Checked = selectedPreset.applyToPlayingTrack;
                favoriteCheckBox.Checked = selectedPreset.favorite;

                idTextBox.Text = selectedPreset.id;


                if (playlistComboBoxCustom.Items.Count == 0)
                {
                    conditionCheckBox.Enable(false);
                    conditionCheckBox.Checked = false;
                }
                else
                {
                    conditionCheckBox.Enable(true);

                    if (selectedPreset.condition)
                    {
                        var playlistFound = false;
                        foreach (Playlist playlist in playlistComboBoxCustom.Items)
                        {
                            if (playlist.playlist == selectedPreset.playlist)
                            {
                                playlistComboBoxCustom.SelectedItem = playlist;
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


                enableDisablePreviewOptionControls(true, true);
                enableQueryingOrUpdatingButtons();

                if (presetListLastSelectedIndex != -2) //It's during form init
                    Enable(true, autoApplyPresetsLabel);
            }

            //Preset referring <All Tags> must not be checked for auto-execution. This function call shows icons on tag labels
            showAllTagsWarningIfRequired(selectedPreset, false);

            //Let's deal with preview table
            nameColumns();


            updateCustomScrollBars(previewTable);
            updateCustomScrollBars(descriptionBox);
            descriptionBox.ScrollToTop();
        }

        private void exportPreset(Preset preset, string presetPathName)
        {
            var savedPreset = new Preset(preset)
            {
                applyToPlayingTrack = false,
                condition = false,
                playlist = null,
                preserveValues = string.Empty,
                id = string.Empty
            };

            savedPreset.savePreset(presetPathName);
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            var presetSafeFileName = selectedPreset.getSafeFileName();

            var dialog = new SaveFileDialog
            {
                //Title = "Save ASR preset",
                Filter = AsrPresetNaming + "|*" + AsrPresetExtension,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                FileName = presetSafeFileName
            };

            if (dialog.ShowDialog(this) == DialogResult.Cancel) return;

            exportPreset(selectedPreset, dialog.FileName);
            dialog.Dispose();
        }

        private void buttonExportUser_Click(object sender, EventArgs e)
        {
            var developerExport = DeveloperMode && ModifierKeys == Keys.Control;

            var dialog = new FolderBrowserDialog
            {
                //Description = "Save user ASR presets",
                SelectedPath = SavedSettings.defaultAsrPresetsExportFolder,
            };

            if (dialog.ShowDialog(this) == DialogResult.Cancel) return;
            SavedSettings.defaultAsrPresetsExportFolder = dialog.SelectedPath;

            var countedPresetFileNames = new SortedDictionary<string, int>();
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

                    var presetFilePath = @"\\?\" + Path.Combine(dialog.SelectedPath, presetFilename + AsrPresetExtension);
                    exportPreset(currentPresetKVPair.Value, presetFilePath);
                }
            }

            if (developerExport && MSR != null)
            {
                var presetFilePath = @"\\?\" + Path.Combine(dialog.SelectedPath, "!" + MSR.guid.ToString("B") + AsrPresetExtension);
                exportPreset(MSR, presetFilePath);
            }

            dialog.Dispose();
        }

        internal void import()
        {
            var dialog = new OpenFileDialog
            {
                //Title = "Load ASR preset",
                Filter = AsrPresetNaming + "|*" + AsrPresetExtension,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Multiselect = true
            };

            if (dialog.ShowDialog(this) == DialogResult.Cancel) return;
            dialog.Dispose();


            var selectedPresetGuid = Guid.Empty;
            if (presetList.SelectedItem != null)
                selectedPresetGuid = (presetList.SelectedItem as Preset).guid;


            var askToImportExistingAsCopy = true;
            var importExistingAsCopy = false;

            var isMsrImported = false;
            var numberOfImportedPresets = 0;
            var numberOfImportedAsCopyPresets = 0;
            var numberOfErrors = 0;

            var presetSerializer = new XmlSerializer(typeof(Preset));

            foreach (var filename in dialog.FileNames)
            {
                try
                {
                    var newPreset = Preset.Load(filename, presetSerializer);

                    presetsWorkingCopy.TryGetValue(newPreset.guid, out var existingPreset);

                    if (newPreset.guid.ToString() != "ff8d53d9-526b-4b40-bbf0-848b6b892f70")
                    {
                        if (existingPreset != null)
                        {
                            if (askToImportExistingAsCopy)
                            {
                                var result = MessageBox.Show(this, MsgDoYouWantToImportExistingPresetsAsCopies,
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
                                var newPresetCopy = new Preset(newPreset, true, false, null, "*");

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
                    else
                    {
                        MSR = newPreset;
                        isMsrImported = true;
                    }
                }
                catch
                {
                    numberOfErrors++;
                }
            }

            refreshPresetList(selectedPresetGuid);

            var message = string.Empty;
            if (isMsrImported)
                message += AddLeadingSpaces(1, 4) + MsgMsrPresetWasImported;
            //if (numberOfImportedPresets > 0)
            message += AddLeadingSpaces(numberOfImportedPresets, 4) + GetPluralForm(MsgPresetsWereImported, numberOfImportedPresets);
            if (numberOfImportedAsCopyPresets > 0)
                message += AddLeadingSpaces(numberOfImportedAsCopyPresets, 4) + GetPluralForm(MsgPresetsWereImportedAsCopies, numberOfImportedAsCopyPresets);
            if (numberOfErrors > 0)
                message += AddLeadingSpaces(numberOfErrors, 4) + GetPluralForm(MsgPresetsFailedToImport, numberOfErrors);

            if (numberOfImportedPresets + numberOfImportedAsCopyPresets > 0)
            {
                unsavedChanges = true;
                buttonClose.Image = warningWide;
                toolTip1.SetToolTip(buttonClose, buttonCloseToolTip);
            }

            MessageBox.Show(this, message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        internal void setPresetsChanged()
        {
            if (!processPresetChanges)
                return;

            unsavedChanges = true;
            buttonClose.Image = warningWide;
            toolTip1.SetToolTip(buttonClose, buttonCloseToolTip);
        }

        internal void refreshPresetList(Guid selectedPresetGuid)
        {
            searchTextBox.Text = string.Empty;
            showPlaylistLimitedChecked = false;
            showFunctionIdAssignedChecked = false;
            showHotkeyAssignedChecked = false;
            showTickedOnlyChecked = false;

            presetList.Items.Clear();
            presetList.Sorted = false;
            ignoreCheckedPresetEvent = false;
            autoAppliedPresetCount = 0;
            foreach (var preset in presetsWorkingCopy.Values)
                presetList.Items.Add(preset, autoAppliedAsrPresetGuids.ContainsKey(preset.guid));

            presetList.SelectedIndex = -1;
            presetList_SelectedIndexChanged(null, null);
            ignoreCheckedPresetEvent = true;
            presetList.Sorted = true;


            if (!presetsWorkingCopy.TryGetValue(selectedPresetGuid, out var value))
                presetList.SelectedIndex = -1;
            else if ((presetList.SelectedItem as Preset)?.guid != selectedPresetGuid)
                presetList.SelectedItem = value;


            updateCustomScrollBars(presetList);
        }

        internal void install(bool installAll)
        {
            var isMsrInstalled = false;
            var numberOfInstalledPresets = 0;
            var numberOfReinstalledPresets = 0;
            var numberOfUpdatedPresets = 0;
            var numberOfUpdatedCustomizedPresets = 0;
            var numberOfReinstalledCustomizedPresets = 0;
            var numberOfNotChangedSkippedPresets = 0;
            var numberOfRemovedPresets = 0;
            var numberOfErrors = 0;


            var selectedPresetGuid = Guid.Empty;
            if (presetList.SelectedItem != null)
                selectedPresetGuid = (presetList.SelectedItem as Preset).guid;


            if (MessageBox.Show(this, MsgInstallingConfirmation, string.Empty,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                == DialogResult.No)
                return;


            if (!Directory.Exists(PresetsPath))
                Directory.CreateDirectory(PresetsPath);

            if (Directory.Exists(PluginsPath))
            {
                var newPresetNames = Directory.GetFiles(Path.Combine(PluginsPath, AsrPresetsDirectory), "*" + AsrPresetExtension);
                var presetSerializer = new XmlSerializer(typeof(Preset));

                var askToResetCustomizedByUser = true;
                var resetCustomizedByUser = true;
                var askToRemovePresets = true;
                var removePresets = true;
                for (var i = newPresetNames.Length - 1; i >= 0; i--)
                {
                    var newPresetName = newPresetNames[i];

                    try
                    {
                        var newPreset = Preset.Load(newPresetName, presetSerializer);

                        if (newPreset.guid.ToString() != "ff8d53d9-526b-4b40-bbf0-848b6b892f70")
                        {
                            presetsWorkingCopy.TryGetValue(newPreset.guid, out var existingPreset);

                            if (existingPreset != null)
                            {
                                var anyCustomization = existingPreset.getCustomizationsFlag() || (existingPreset.condition || existingPreset.applyToPlayingTrack);

                                if (installAll)
                                {
                                    if (askToResetCustomizedByUser && anyCustomization)
                                    {
                                        if (MessageBox.Show(this, MsgDoYouWantToResetYourCustomizedPredefinedPresets, string.Empty, MessageBoxButtons.YesNo,
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
                                        if (MessageBox.Show(this, MsgDoYouWantToRemovePredefinedPresets, string.Empty, MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                                        {
                                            askToRemovePresets = false;
                                            removePresets = false;
                                        }
                                        else
                                        {
                                            askToRemovePresets = false;
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
                                    if (askToRemovePresets)
                                    {
                                        if (MessageBox.Show(this, MsgDoYouWantToResetYourCustomizedPredefinedPresets, string.Empty, MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                                        {
                                            askToRemovePresets = false;
                                            removePresets = false;
                                        }
                                        else
                                        {
                                            askToRemovePresets = false;
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
                                else //if (!importAll && (newPreset.modifiedUtc <= existingPreset.modifiedUtc || newPreset.modifiedUtc <= MusicBeePlugin.SavedSettings.lastAsrImportDateUtc))
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
                            isMsrInstalled = true;
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


            var message = string.Empty;
            if (isMsrInstalled)
                message += AddLeadingSpaces(1, 4) + MsgMsrPresetWasInstalled;
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

            if (message == string.Empty)
            {
                message = MsgPresetsNotFound;
            }
            else
            {
                unsavedChanges = true;
                buttonClose.Image = warningWide;
                toolTip1.SetToolTip(buttonClose, buttonCloseToolTip);
            }


            MessageBox.Show(this, message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        internal void deleteAll()
        {
            var numberOfDeletedPresets = 0;

            var result = MessageBox.Show(this, MsgDeletingConfirmation, string.Empty, MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No)
                return;

            var selectedPresetGuid = Guid.Empty;
            if (selectedPreset != null)
                selectedPresetGuid = selectedPreset.guid;

            var predefinedPresets = new List<Preset>();
            foreach (var presetKeyValuePair in presetsWorkingCopy)
            {
                if (!presetKeyValuePair.Value.userPreset)
                {
                    predefinedPresets.Add(presetKeyValuePair.Value);
                }
            }

            foreach (var preset in predefinedPresets)
            {
                deletePreset(preset);
                numberOfDeletedPresets++;
            }

            searchTextBox.Text = string.Empty;
            showPlaylistLimitedChecked = false;
            showFunctionIdAssignedChecked = false;
            showHotkeyAssignedChecked = false;
            showTickedOnlyChecked = false;

            presetList.Items.Clear();
            presetList.Sorted = false;
            ignoreCheckedPresetEvent = false;
            autoAppliedPresetCount = 0;
            foreach (var preset in presetsWorkingCopy.Values)
                presetList.Items.Add(preset, autoAppliedAsrPresetGuids.ContainsKey(preset.guid));

            refreshPresetList(selectedPresetGuid);

            showAutoApplyingWarningIfRequired();

            ignoreCheckedPresetEvent = true;
            presetList.Sorted = true;


            var message = string.Empty;

            if (numberOfDeletedPresets == 0)
                message = MsgNoPresetsDeleted;
            else
                message += numberOfDeletedPresets + GetPluralForm(MsgPresetsWereDeleted, numberOfDeletedPresets);

            MessageBox.Show(this, message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (AllTagsPresetsCantBeAutoAppliedCountdown == 0)
            {
                allTagsWarningTimer.Change(-1, -1);
                Invoke(new Action(showAutoApplyingWarningIfRequired));
                return;
            }
            else if (AllTagsPresetsCantBeAutoAppliedCountdown == 1)
            {
                AllTagsPresetsCantBeAutoAppliedCountdown--;
                allTagsWarningTimer.Change(10000, -1);
                Invoke(new Action(() => { autoApplyPresetsLabel.ForeColor = TickedColor; }));
                return;
            }


            if ((AllTagsPresetsCantBeAutoAppliedCountdown & 0x1) == 0)
                Invoke(new Action(() => { autoApplyPresetsLabel.ForeColor = TickedColor; }));
            else
                Invoke(new Action(() => { autoApplyPresetsLabel.ForeColor = UntickedColor; }));

            AllTagsPresetsCantBeAutoAppliedCountdown--;
        }

        private void showAllTagsWarning(bool showGeneralWarning, List<PictureBox> allTagsImages = null, List<Label> allTagsLabels = null, List<PictureBox> genericImages = null, List<Label> genericLabels = null)
        {
            if (allTagsReplaceIdsCount > 0 && !buttonProcessPreserveTags.Visible)
            {
                tableLayoutPanel3.ColumnStyles[0].Width = tableLayoutPanel3ColWidth0;
                tableLayoutPanel3.ColumnStyles[1].SizeType = SizeType.Percent;
                tableLayoutPanel3.ColumnStyles[1].Width = tableLayoutPanel3ColWidth1;
                tableLayoutPanel3.ColumnStyles[2].Width = tableLayoutPanel3ColWidth2;

                buttonProcessPreserveTags.Visible = true;
                processPreserveTagsTextBox.Visible = true;
                buttonSelectPreservedTags.Visible = true;
            }
            else if (allTagsReplaceIdsCount == 0 && buttonProcessPreserveTags.Visible)
            {
                buttonProcessPreserveTags.Visible = false;
                processPreserveTagsTextBox.Visible = false;
                buttonSelectPreservedTags.Visible = false;

                tableLayoutPanel3.ColumnStyles[0].Width = 0;
                tableLayoutPanel3.ColumnStyles[1].SizeType = SizeType.Absolute;
                tableLayoutPanel3.ColumnStyles[1].Width = 0;
                tableLayoutPanel3.ColumnStyles[2].Width = 0;
            }

            if (allTagsImages != null)
            {
                for (var i = 0; i < allTagsImages.Count; i++)
                {
                    allTagsImages[i].Image = warning;
                    toolTip1.SetToolTip(allTagsImages[i], AllTagsTagIsSelectedToolTip);
                    toolTip1.SetToolTip(allTagsLabels[i], AllTagsTagIsSelectedToolTip);
                }
            }

            if (genericImages != null)
            {
                for (var i = 0; i < genericImages.Count; i++)
                {
                    genericImages[i].Image = Resources.transparent_15;
                    toolTip1.SetToolTip(genericImages[i], string.Empty);
                    toolTip1.SetToolTip(genericLabels[i], string.Empty);
                }
            }


            if (showGeneralWarning)
            {
                if (processPresetChanges)
                {
                    System.Media.SystemSounds.Exclamation.Play();

                    AllTagsPresetsCantBeAutoAppliedCountdown = 20;
                    autoApplyPresetsLabel.Text = AllTagsPresetsCantBeAutoAppliedText.ToUpper();
                    toolTip1.SetToolTip(autoApplyPresetsLabel, AllTagsPresetsCantBeAutoAppliedText.ToUpper());

                    if (allTagsWarningTimer == null)
                        allTagsWarningTimer = new System.Threading.Timer(flashAllTagsWarning, null, 0, 250);
                    else
                        allTagsWarningTimer.Change(0, 250);
                }
            }
            else
            {
                if (allTagsWarningTimer != null && AllTagsPresetsCantBeAutoAppliedCountdown == 0)
                {
                    allTagsWarningTimer.Change(-1, -1);
                    showAutoApplyingWarningIfRequired();
                }
            }
        }

        private bool showAllTagsWarningIfRequired(Preset paramPreset, bool showGeneralWarning)
        {
            if (paramPreset == null)
            {
                selectedPresetUsesAllTags = false;
                showAllTagsWarning(false);

                disableQueryingOrUpdatingButtons();

                return false;
            }


            var allTagsImages = new List<PictureBox>();
            var allTagsLabels = new List<Label>();

            var genericImages = new List<PictureBox>();
            var genericLabels = new List<Label>();

            if (paramPreset.detectAllTagsTagId(paramPreset.parameterTagId))
            {
                allTagsImages.Add(pictureTag);
                allTagsLabels.Add(labelTag);
            }
            else
            {
                genericImages.Add(pictureTag);
                genericLabels.Add(labelTag);
            }

            if (paramPreset.detectAllTagsTagId(paramPreset.parameterTag2Id))
            {
                allTagsImages.Add(pictureTag2);
                allTagsLabels.Add(labelTag2);
            }
            else
            {
                genericImages.Add(pictureTag2);
                genericLabels.Add(labelTag2);
            }

            if (paramPreset.detectAllTagsTagId(paramPreset.parameterTag3Id))
            {
                allTagsImages.Add(pictureTag3);
                allTagsLabels.Add(labelTag3);
            }
            else
            {
                genericImages.Add(pictureTag3);
                genericLabels.Add(labelTag3);
            }

            if (paramPreset.detectAllTagsTagId(paramPreset.parameterTag4Id))
            {
                allTagsImages.Add(pictureTag4);
                allTagsLabels.Add(labelTag4);
            }
            else
            {
                genericImages.Add(pictureTag4);
                genericLabels.Add(labelTag4);
            }

            if (paramPreset.detectAllTagsTagId(paramPreset.parameterTag5Id))
            {
                allTagsImages.Add(pictureTag5);
                allTagsLabels.Add(labelTag5);
            }
            else
            {
                genericImages.Add(pictureTag5);
                genericLabels.Add(labelTag5);
            }

            if (paramPreset.detectAllTagsTagId(paramPreset.parameterTag6Id))
            {
                allTagsImages.Add(pictureTag6);
                allTagsLabels.Add(labelTag6);
            }
            else
            {
                genericImages.Add(pictureTag6);
                genericLabels.Add(labelTag6);
            }

            if (allTagsLabels.Count > 0)
            {
                selectedPresetUsesAllTags = true;
                assignHotkeyCheckBox.Checked = false;
                assignHotkeyCheckBox.Enable(false);

                if (autoAppliedAsrPresetGuids.ContainsKey(paramPreset.guid))
                {
                    var presetIndex = presetList.Items.IndexOf(paramPreset);
                    ignoreCheckedPresetEvent = false;
                    presetList.SetItemChecked(presetIndex, false);
                    ignoreCheckedPresetEvent = true;
                }

                showAllTagsWarning(showGeneralWarning, allTagsImages, allTagsLabels, genericImages, genericLabels);

                CellTooTip = CtlAsrAllTagsCellTooTip;

                enableQueryingOrUpdatingButtons();

                return true;
            }
            else
            {
                selectedPresetUsesAllTags = false;
                assignHotkeyCheckBox.Enable(true);

                showAllTagsWarning(false, null, null, genericImages, genericLabels);

                CellTooTip = CtlAsrCellTooTip;

                enableQueryingOrUpdatingButtons();

                return false;
            }
        }

        private void parameterTag_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            selectedPreset.parameterTagId = AsrGetTagId(parameterTagListCustom.Text);
            allTagsReplaceIdsCount = selectedPreset.countAllTagsReplaceTagIds();
            showAllTagsWarningIfRequired(selectedPreset, autoAppliedAsrPresetGuids.ContainsKey(selectedPreset.guid));
            nameColumns();
            selectedPreset.setCustomizationsFlag(this, backedUpPreset);
        }

        private void parameterTag2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            selectedPreset.parameterTag2Id = AsrGetTagId(parameterTag2ListCustom.Text);
            allTagsReplaceIdsCount = selectedPreset.countAllTagsReplaceTagIds();
            showAllTagsWarningIfRequired(selectedPreset, autoAppliedAsrPresetGuids.ContainsKey(selectedPreset.guid));
            nameColumns();
            selectedPreset.setCustomizationsFlag(this, backedUpPreset);
        }

        private void parameterTag3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            selectedPreset.parameterTag3Id = AsrGetTagId(parameterTag3ListCustom.Text);
            allTagsReplaceIdsCount = selectedPreset.countAllTagsReplaceTagIds();
            showAllTagsWarningIfRequired(selectedPreset, autoAppliedAsrPresetGuids.ContainsKey(selectedPreset.guid));
            nameColumns();
            selectedPreset.setCustomizationsFlag(this, backedUpPreset);
        }

        private void parameterTag4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            selectedPreset.parameterTag4Id = AsrGetTagId(parameterTag4ListCustom.Text);
            allTagsReplaceIdsCount = selectedPreset.countAllTagsReplaceTagIds();
            showAllTagsWarningIfRequired(selectedPreset, autoAppliedAsrPresetGuids.ContainsKey(selectedPreset.guid));
            nameColumns();
            selectedPreset.setCustomizationsFlag(this, backedUpPreset);
        }

        private void parameterTag5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            selectedPreset.parameterTag5Id = AsrGetTagId(parameterTag5ListCustom.Text);
            allTagsReplaceIdsCount = selectedPreset.countAllTagsReplaceTagIds();
            showAllTagsWarningIfRequired(selectedPreset, autoAppliedAsrPresetGuids.ContainsKey(selectedPreset.guid));
            nameColumns();
            selectedPreset.setCustomizationsFlag(this, backedUpPreset);
        }

        private void parameterTag6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            selectedPreset.parameterTag6Id = AsrGetTagId(parameterTag6ListCustom.Text);
            allTagsReplaceIdsCount = selectedPreset.countAllTagsReplaceTagIds();
            showAllTagsWarningIfRequired(selectedPreset, autoAppliedAsrPresetGuids.ContainsKey(selectedPreset.guid));
            nameColumns();
            selectedPreset.setCustomizationsFlag(this, backedUpPreset);
        }

        private void customTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            selectedPreset.customText = customTextBox.Text;
            selectedPreset.setCustomizationsFlag(this, backedUpPreset);
        }

        private void customText2Box_TextChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            selectedPreset.customText2 = customText2Box.Text;
            selectedPreset.setCustomizationsFlag(this, backedUpPreset);
        }

        private void customText3Box_TextChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            selectedPreset.customText3 = customText3Box.Text;
            selectedPreset.setCustomizationsFlag(this, backedUpPreset);
        }

        private void customText4Box_TextChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            selectedPreset.customText4 = customText4Box.Text;
            selectedPreset.setCustomizationsFlag(this, backedUpPreset);
        }

        private void preserveTagValuesTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            selectedPreset.preserveValues = preserveTagValuesTextBox.Text;
            selectedPreset.setCustomizationsFlag(this, backedUpPreset);
        }

        private void processPreserveTagsTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            if (processTagsMode)
                selectedPreset.processTags = processPreserveTagsTextBox.Text;
            else
                selectedPreset.preserveTags = processPreserveTagsTextBox.Text;

            selectedPreset.setCustomizationsFlag(this, backedUpPreset);
        }

        private void buttonProcessPreserveTags_Click(object sender, EventArgs e)
        {
            processTagsMode = !processTagsMode;

            if (processTagsMode)
            {
                buttonProcessPreserveTags.Text = AsrProcessTagsButtonName;
                processPreserveTagsTextBox.Text = selectedPreset.processTags;
            }
            else
            {
                buttonProcessPreserveTags.Text = AsrPreserveTagsButtonName;
                processPreserveTagsTextBox.Text = selectedPreset.preserveTags;
            }

            selectedPreset.processTagsMode = processTagsMode;
            selectedPreset.setCustomizationsFlag(this, backedUpPreset);
        }

        private void buttonSelectPreservedTags_Click(object sender, EventArgs e)
        {
            var selectedTags = processPreserveTagsTextBox.Text.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            selectedTags = CopyTagsToClipboard.SelectTags(TagToolsPlugin, SelectTagsWindowTitle, SelectButtonName, selectedTags, false, false);

            var preserveTags = string.Empty;
            if (selectedTags.Length > 0)
            {
                foreach (var tagName in selectedTags)
                {
                    preserveTags += tagName + ";";
                }
                preserveTags = preserveTags.Trim(';');
            }

            processPreserveTagsTextBox.Text = preserveTags;
        }

        private void conditionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;


            playlistComboBoxCustom.Enable(conditionCheckBox.Checked);

            if (conditionCheckBox.Checked && playlistComboBoxCustom.SelectedIndex == -1 && playlistComboBoxCustom.Items.Count > 0)
            {
                playlistComboBoxCustom.SelectedIndex = 0;
            }
            else if (conditionCheckBox.Checked && playlistComboBoxCustom.Items.Count == 0)
            {
                conditionCheckBox.Checked = false;
            }

            selectedPreset.condition = playlistComboBoxCustom.IsEnabled();
            selectedPreset.setCustomizationsFlag(this, backedUpPreset);
        }

        private void conditionCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!conditionCheckBox.IsEnabled())
                return;

            conditionCheckBox.Checked = !conditionCheckBox.Checked;
        }

        private void playlistComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            if (playlistComboBoxCustom.SelectedIndex >= 0)
                selectedPreset.playlist = (playlistComboBoxCustom.SelectedItem as Playlist).playlist;
            else
                selectedPreset.playlist = string.Empty;

            selectedPreset.setCustomizationsFlag(this, backedUpPreset);
        }

        private void cbHeader_OnCheckBoxClicked(bool state)
        {
            foreach (DataGridViewRow row in previewTable.Rows)
            {
                if (row.Cells[0].Value == null)
                    continue;

                if (state)
                    row.Cells[0].Value = "F";
                else
                    row.Cells[0].Value = "T";

                var e = new DataGridViewCellEventArgs(0, row.Index);
                previewTable_CellContentClick(null, e);
            }
        }

        private void toggleRow(bool check, int rowIndex)
        {
            if (!check)
            {
                previewTable.Rows[rowIndex].Cells[0].Value = "F";

                var sourceTag1Value = (string)previewTable.Rows[rowIndex].Cells[5].Value;
                var sourceTag2Value = (string)previewTable.Rows[rowIndex].Cells[8].Value;
                var sourceTag3Value = (string)previewTable.Rows[rowIndex].Cells[11].Value;
                var sourceTag4Value = (string)previewTable.Rows[rowIndex].Cells[14].Value;
                var sourceTag5Value = (string)previewTable.Rows[rowIndex].Cells[17].Value;

                previewTable.Rows[rowIndex].Cells[6].Value = sourceTag1Value;
                previewTable.Rows[rowIndex].Cells[9].Value = sourceTag2Value;
                previewTable.Rows[rowIndex].Cells[12].Value = sourceTag3Value;
                previewTable.Rows[rowIndex].Cells[15].Value = sourceTag4Value;
                previewTable.Rows[rowIndex].Cells[18].Value = sourceTag5Value;
            }
            else //if (check)
            {
                previewTable.Rows[rowIndex].Cells[0].Value = "T";

                string newTag1Value;
                string newTag2Value;
                string newTag3Value;
                string newTag4Value;
                string newTag5Value;

                lock (Presets)
                {
                    var combinationPreset = presetProcessingCopies[(string)previewTable.Rows[rowIndex].Cells[1].Value];
                    var curentFile = (string)previewTable.Rows[rowIndex].Cells[2].Value;
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
                var sameTags = false;
                var addRemoveTagsToProcessedPreserved = false;

                if (selectedPresetUsesAllTags && ModifierKeys == Keys.Shift)
                {
                    sameTags = true;
                }
                if (selectedPresetUsesAllTags && ModifierKeys == Keys.Control)
                {
                    sameTags = true;
                    addRemoveTagsToProcessedPreserved = true;
                }


                var isChecked = (string)previewTable.Rows[e.RowIndex].Cells[0].Value;

                var check = true;
                var indeterminate = false;

                if (isChecked == "T")
                    check = false;
                else if (isChecked == "F")
                    ;
                else
                    indeterminate = true;

                if (indeterminate)
                    return;


                if (!sameTags) //Toggle checked state of single row
                {
                    toggleRow(check, e.RowIndex);

                    previewTableFormatRow(e.RowIndex);
                }
                else //Toggle checked state of all rows for the same tag(s) - 1 row can refer to up to 5 tags
                {
                    var invokedTags = new string[5];

                    for (var i = 0; i < 5; i++)
                    {
                        invokedTags[i] = (string)previewTable.Rows[e.RowIndex].Cells[4 + i * 3].Value;
                        if (invokedTags[i] == null)
                            continue;

                        if (addRemoveTagsToProcessedPreserved && processTagsMode)
                        {
                            if (string.IsNullOrEmpty(selectedPreset.processTags))
                                selectedPreset.processTags = string.Empty;

                            if (check && !(";" + selectedPreset.processTags + ";").Contains(";" + invokedTags[i] + ";"))
                                selectedPreset.processTags += ";" + invokedTags[i];
                            else if (!check && (";" + selectedPreset.processTags + ";").Contains(";" + invokedTags[i] + ";"))
                                selectedPreset.processTags = selectedPreset.processTags.Replace(invokedTags[i], string.Empty).Replace(";;", ";");
                        }
                        else if (addRemoveTagsToProcessedPreserved && !processTagsMode)
                        {
                            if (string.IsNullOrEmpty(selectedPreset.preserveTags))
                                selectedPreset.preserveTags = string.Empty;

                            if (!check && !(";" + selectedPreset.preserveTags + ";").Contains(";" + invokedTags[i] + ";"))
                                selectedPreset.preserveTags += ";" + invokedTags[i];
                            else if (check && (";" + selectedPreset.preserveTags + ";").Contains(";" + invokedTags[i] + ";"))
                                selectedPreset.preserveTags = selectedPreset.preserveTags.Replace(invokedTags[i], string.Empty).Replace(";;", ";");
                        }
                    }

                    if (addRemoveTagsToProcessedPreserved && processTagsMode)
                    {
                        selectedPreset.processTags = selectedPreset.processTags.Replace(";;", ";");
                        selectedPreset.processTags = selectedPreset.processTags.Trim(';');
                        processPreserveTagsTextBox.Text = selectedPreset.processTags;
                    }
                    else if (addRemoveTagsToProcessedPreserved && !processTagsMode)
                    {
                        selectedPreset.preserveTags = selectedPreset.preserveTags.Replace(";;", ";");
                        selectedPreset.preserveTags = selectedPreset.preserveTags.Trim(';');
                        processPreserveTagsTextBox.Text = selectedPreset.preserveTags;
                    }


                    for (var j = 0; j < previewTable.RowCount; j++)
                    {
                        var processRow = true;

                        for (var i = 0; i < 5; i++)
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

        private void PreviewTable_MouseDown(object sender, MouseEventArgs e)
        {
            previewTableLeftMouseButtonPressed = true;
        }

        private void PreviewTable_MouseUp(object sender, MouseEventArgs e)
        {
            previewTableLeftMouseButtonPressed = false;
        }

        private void PreviewTable_MouseLeave(object sender, EventArgs e)
        {
            previewTableLeftMouseButtonPressed = false;
        }

        private void previewTable_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            saveColumnWidths(selectedPreset);
        }

        private void previewTableFormatRow(int rowIndex)
        {
            previewTable.Rows[rowIndex].Cells[0].ToolTipText = CellTooTip;

            if (SavedSettings.dontHighlightChangedTags)
                return;

            for (var columnIndex = 1; columnIndex < previewTable.ColumnCount; columnIndex++)
            {
                if (previewTable.Rows[rowIndex].Cells[columnIndex].Visible)
                {
                    if ((string)previewTable.Rows[rowIndex].Cells[0].Value == "T")
                        previewTable.Rows[rowIndex].Cells[columnIndex].Style = unchangedCellStyle;
                    else
                        previewTable.Rows[rowIndex].Cells[columnIndex].Style = dimmedCellStyle;
                }
            }


            for (var columnIndex = 1; columnIndex < previewTable.ColumnCount; columnIndex++)
            {
                if (columnIndex == 6 && previewTable.Columns[columnIndex].Visible)
                {
                    var cell = previewTable.Rows[rowIndex].Cells[6];
                    var changeType = (ChangesDetectionResult)cell.Tag;

                    if (changeType == ChangesDetectionResult.PreservedTagsDetected)
                        cell.Style = preservedTagCellStyle;
                    else if (changeType == ChangesDetectionResult.PreservedTagValuesDetected)
                        cell.Style = preservedTagValueCellStyle;
                    else if (changeType == ChangesDetectionResult.NoChangesDetected)
                        cell.Style = unchangedCellStyle;
                    else if (changeType == ChangesDetectionResult.NoExclusionsDetected)
                        cell.Style = changedCellStyle;
                }
                else if (columnIndex == 9 && previewTable.Columns[columnIndex].Visible)
                {
                    var cell = previewTable.Rows[rowIndex].Cells[9];

                    if ((ChangesDetectionResult)cell.Tag == ChangesDetectionResult.PreservedTagsDetected)
                    {
                        cell.Style = preservedTagCellStyle;
                        return;
                    }
                    else if ((ChangesDetectionResult)cell.Tag == ChangesDetectionResult.PreservedTagValuesDetected)
                    {
                        cell.Style = preservedTagValueCellStyle;
                        return;
                    }


                    if (previewTable.Columns[8].Visible)
                    {
                        if ((string)cell.Value == (string)previewTable.Rows[rowIndex].Cells[8].Value)
                            cell.Style = unchangedCellStyle;
                        else
                            cell.Style = changedCellStyle;

                        return;
                    }

                    if (previewTable.Columns[5].Visible)
                        break;

                    if ((ChangesDetectionResult)previewTable.Rows[rowIndex].Cells[3].Tag == ChangesDetectionResult.PreservedTagsDetected)
                        cell.Style = preservedTagCellStyle;
                    else if ((ChangesDetectionResult)previewTable.Rows[rowIndex].Cells[3].Tag == ChangesDetectionResult.PreservedTagValuesDetected)
                        cell.Style = preservedTagValueCellStyle;
                    else if ((string)cell.Value == (string)previewTable.Rows[rowIndex].Cells[2].Value)
                        cell.Style = unchangedCellStyle;
                    else
                        cell.Style = changedCellStyle;
                }
                else if (columnIndex == 12 && previewTable.Columns[columnIndex].Visible)
                {
                    var cell = previewTable.Rows[rowIndex].Cells[12];

                    if ((ChangesDetectionResult)cell.Tag == ChangesDetectionResult.PreservedTagsDetected)
                    {
                        cell.Style = preservedTagCellStyle;
                        return;
                    }
                    else if ((ChangesDetectionResult)cell.Tag == ChangesDetectionResult.PreservedTagValuesDetected)
                    {
                        cell.Style = preservedTagValueCellStyle;
                        return;
                    }

                    if (previewTable.Columns[11].Visible)
                    {
                        if ((string)cell.Value == (string)previewTable.Rows[rowIndex].Cells[11].Value)
                            cell.Style = unchangedCellStyle;
                        else
                            cell.Style = changedCellStyle;

                        return;
                    }

                    var i = 8;
                    do
                    {
                        if (previewTable.Columns[i].Visible)
                            break;

                        i -= 3;
                    }
                    while (i >= 5);

                    if ((ChangesDetectionResult)previewTable.Rows[rowIndex].Cells[i + 1].Tag == ChangesDetectionResult.PreservedTagsDetected)
                        cell.Style = preservedTagCellStyle;
                    else if ((ChangesDetectionResult)previewTable.Rows[rowIndex].Cells[i + 1].Tag == ChangesDetectionResult.PreservedTagValuesDetected)
                        cell.Style = preservedTagValueCellStyle;
                    else if ((string)cell.Value == (string)previewTable.Rows[rowIndex].Cells[i].Value)
                        cell.Style = unchangedCellStyle;
                    else
                        cell.Style = changedCellStyle;
                }
                else if (columnIndex == 15 && previewTable.Columns[columnIndex].Visible)
                {
                    var cell = previewTable.Rows[rowIndex].Cells[15];

                    if ((ChangesDetectionResult)cell.Tag == ChangesDetectionResult.PreservedTagsDetected)
                    {
                        cell.Style = preservedTagCellStyle;
                        return;
                    }
                    else if ((ChangesDetectionResult)cell.Tag == ChangesDetectionResult.PreservedTagValuesDetected)
                    {
                        cell.Style = preservedTagValueCellStyle;
                        return;
                    }

                    if (previewTable.Columns[11].Visible)
                    {
                        if ((string)cell.Value == (string)previewTable.Rows[rowIndex].Cells[14].Value)
                            cell.Style = unchangedCellStyle;
                        else
                            cell.Style = changedCellStyle;

                        return;
                    }

                    var i = 11;
                    do
                    {
                        if (previewTable.Columns[i].Visible)
                            break;

                        i -= 3;
                    }
                    while (i >= 5);

                    if ((ChangesDetectionResult)previewTable.Rows[rowIndex].Cells[i + 1].Tag == ChangesDetectionResult.PreservedTagsDetected)
                        cell.Style = preservedTagCellStyle;
                    else if ((ChangesDetectionResult)previewTable.Rows[rowIndex].Cells[i + 1].Tag == ChangesDetectionResult.PreservedTagValuesDetected)
                        cell.Style = preservedTagValueCellStyle;
                    else if ((string)cell.Value == (string)previewTable.Rows[rowIndex].Cells[i].Value)
                        cell.Style = unchangedCellStyle;
                    else
                        cell.Style = changedCellStyle;
                }
                else if (columnIndex == 18 && previewTable.Columns[columnIndex].Visible)
                {
                    var cell = previewTable.Rows[rowIndex].Cells[18];

                    if ((ChangesDetectionResult)cell.Tag == ChangesDetectionResult.PreservedTagsDetected)
                    {
                        cell.Style = preservedTagCellStyle;
                        return;
                    }
                    else if ((ChangesDetectionResult)cell.Tag == ChangesDetectionResult.PreservedTagValuesDetected)
                    {
                        cell.Style = preservedTagValueCellStyle;
                        return;
                    }

                    if (previewTable.Columns[11].Visible)
                    {
                        if ((string)cell.Value == (string)previewTable.Rows[rowIndex].Cells[17].Value)
                            cell.Style = unchangedCellStyle;
                        else
                            cell.Style = changedCellStyle;

                        return;
                    }

                    var i = 14;
                    do
                    {
                        if (previewTable.Columns[i].Visible)
                            break;

                        i -= 3;
                    }
                    while (i >= 5);

                    if ((ChangesDetectionResult)previewTable.Rows[rowIndex].Cells[i + 1].Tag == ChangesDetectionResult.PreservedTagsDetected)
                        cell.Style = preservedTagCellStyle;
                    else if ((ChangesDetectionResult)previewTable.Rows[rowIndex].Cells[i + 1].Tag == ChangesDetectionResult.PreservedTagValuesDetected)
                        cell.Style = preservedTagValueCellStyle;
                    else if ((string)cell.Value == (string)previewTable.Rows[rowIndex].Cells[i].Value)
                        cell.Style = unchangedCellStyle;
                    else
                        cell.Style = changedCellStyle;
                }
            }
        }

        internal override void enableDisablePreviewOptionControls(bool enable, bool dontChangeDisabled = false)
        {
            if (enable && previewIsGenerated)
                return;

            buttonCreate.Enable(enable);
            buttonImportNew.Enable(enable);
            buttonImportAll.Enable(enable);
            buttonImport.Enable(enable);
            buttonExportCustom.Enable(enable);
            buttonDeleteAll.Enable(enable);


            if (selectedPreset == null)
                enable = false;

            assignHotkeyCheckBox.Enable(enable && (assignHotkeyCheckBox.IsEnabled() || !dontChangeDisabled));
            applyToPlayingTrackCheckBox.Enable(enable && assignHotkeyCheckBox.Checked);
            conditionCheckBox.Enable(enable && (conditionCheckBox.IsEnabled() || !dontChangeDisabled));
            playlistComboBoxCustom.Enable(conditionCheckBox.Checked);
            favoriteCheckBox.Enable(enable);
            idTextBox.Enable(enable);
            clearIdButton.Enable(enable);

            labelTag.Enable(enable && (labelTag.IsEnabled() || !dontChangeDisabled));
            parameterTagListCustom.Enable(enable && (parameterTagListCustom.IsEnabled() || !dontChangeDisabled));
            labelTag2.Enable(enable && (labelTag2.IsEnabled() || !dontChangeDisabled));
            parameterTag2ListCustom.Enable(enable && (parameterTag2ListCustom.IsEnabled() || !dontChangeDisabled));
            labelTag3.Enable(enable && (labelTag3.IsEnabled() || !dontChangeDisabled));
            parameterTag3ListCustom.Enable(enable && (parameterTag3ListCustom.IsEnabled() || !dontChangeDisabled));
            labelTag4.Enable(enable && (labelTag4.IsEnabled() || !dontChangeDisabled));
            parameterTag4ListCustom.Enable(enable && (parameterTag4ListCustom.IsEnabled() || !dontChangeDisabled));
            labelTag5.Enable(enable && (labelTag5.IsEnabled() || !dontChangeDisabled));
            parameterTag5ListCustom.Enable(enable && (parameterTag5ListCustom.IsEnabled() || !dontChangeDisabled));
            labelTag6.Enable(enable && (labelTag6.IsEnabled() || !dontChangeDisabled));
            parameterTag6ListCustom.Enable(enable && (parameterTag6ListCustom.IsEnabled() || !dontChangeDisabled));

            customTextLabel.Enable(enable && selectedPreset.customTextChecked); //-V3125
            customTextBox.Enable(enable && selectedPreset.customTextChecked);
            customText2Label.Enable(enable && selectedPreset.customText2Checked);
            customText2Box.Enable(enable && selectedPreset.customText2Checked);
            customText3Label.Enable(enable && selectedPreset.customText3Checked);
            customText3Box.Enable(enable && selectedPreset.customText3Checked);
            customText4Label.Enable(enable && selectedPreset.customText4Checked);
            customText4Box.Enable(enable && selectedPreset.customText4Checked);

            labelPreserveTagValues.Enable(enable);
            processPreserveTagsTextBox.Enable(enable);
            processPreserveTagsTextBox.ReadOnly = !processPreserveTagsTextBox.IsEnabled();
            preserveTagValuesTextBox.Enable(enable);

            buttonProcessPreserveTags.Enable(false);

            buttonExport.Enable(enable && (editButtonEnabled || selectedPreset.getCustomizationsFlag()));

            buttonCopy.Enable(enable);
            buttonEdit.Enable(enable && editButtonEnabled);
            buttonDelete.Enable(enable);
        }

        internal override void enableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonPreview, " ");
            dirtyErrorProvider.SetError(buttonPreview, string.Empty);
        }

        internal override void disableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonPreview, getBackgroundTasksWarning());
        }

        internal override void enableQueryingOrUpdatingButtons()
        {
            presetList.Enable(!backgroundTaskIsScheduled);

            buttonOK.Enable(((previewIsGenerated && !previewIsStopped) || (SavedSettings.allowCommandExecutionWithoutPreview) && !selectedPresetUsesAllTags));
            buttonPreview.Enable(true);
        }

        internal override void disableQueryingOrUpdatingButtons()
        {
            presetList.Enable(!backgroundTaskIsScheduled);

            buttonOK.Enable(false);
            buttonPreview.Enable(false);
        }

        private void filterPresetList()
        {
            var searchStrings = searchTextBox.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            presetList.Items.Clear();
            presetList.Sorted = false;
            ignoreCheckedPresetEvent = false;

            setCheckedPicturesStates();

            foreach (var preset in presetsWorkingCopy.Values)
            {
                var filteringCriteriaAreMeet = true;


                if (showHotkeyAssignedChecked && !preset.hotkeyAssigned)
                {
                    filteringCriteriaAreMeet = false;
                }
                else if (showFunctionIdAssignedChecked && string.IsNullOrEmpty(preset.id))
                {
                    filteringCriteriaAreMeet = false;
                }
                else if (showPlaylistLimitedChecked && !preset.condition)
                {
                    filteringCriteriaAreMeet = false;
                }
                else if (showUserChecked && !preset.userPreset)
                {
                    filteringCriteriaAreMeet = false;
                }
                else if (showCustomizedChecked && !preset.getCustomizationsFlag())
                {
                    filteringCriteriaAreMeet = false;
                }
                else if (showPredefinedChecked && preset.userPreset)
                {
                    filteringCriteriaAreMeet = false;
                }


                if (filteringCriteriaAreMeet)
                {
                    var presetName = preset.ToString();

                    foreach (var searchString in searchStrings)
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
                    var autoApply = autoAppliedAsrPresetGuids.ContainsKey(preset.guid);
                    if (!showTickedOnlyChecked || autoApply)
                    {
                        presetList.Items.Add(preset, autoApply);
                        if (autoApply)
                            autoAppliedPresetCount--;
                    }
                }
            }

            showAutoApplyingWarningIfRequired();
            ignoreCheckedPresetEvent = true;
            presetList.Sorted = true;
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            filterPresetList();
        }

        private void clearSearchButton_Click(object sender, EventArgs e)
        {
            searchTextBox.Text = string.Empty;
        }

        private void assignHotkeyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            applyToPlayingTrackCheckBox.Enable(assignHotkeyCheckBox.Checked);
            applyToPlayingTrackCheckBox.Checked = applyToPlayingTrackCheckBox.Checked && !assignHotkeyCheckBox.Checked;

            if (assignHotkeyCheckBox.Checked)
            {
                var slot = FindFirstSlot(asrPresetsWithHotkeysGuids, Guid.Empty);
                asrPresetsWithHotkeysGuids[slot] = selectedPreset.guid; //-V3106
                selectedPreset.hotkeyAssigned = true;

                asrPresetsWithHotkeysCount++;
            }
            else
            {
                var slot = FindFirstSlot(asrPresetsWithHotkeysGuids, selectedPreset.guid);
                asrPresetsWithHotkeysGuids[slot] = Guid.Empty; //-V3106
                selectedPreset.hotkeyAssigned = false;

                asrPresetsWithHotkeysCount--;
            }

            selectedPreset.setCustomizationsFlag(this, backedUpPreset);

            assignHotkeyCheckBoxLabel.Text = assignHotkeyCheckBoxText + (MaximumNumberOfAsrHotkeys - asrPresetsWithHotkeysCount) + "/" + MaximumNumberOfAsrHotkeys;
        }

        private void assignHotkeyCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!assignHotkeyCheckBox.IsEnabled())
                return;

            assignHotkeyCheckBox.Checked = !assignHotkeyCheckBox.Checked;
        }

        private void showAutoApplyingWarningIfRequired()
        {
            if (autoAppliedPresetCount == 0)
            {
                if (SavedSettings.allowAsrLrPresetAutoExecution)
                {
                    autoApplyPresetsLabel.Text = EditApplyText + AutoApplyText;

                    toolTip1.SetToolTip(autoApplyPresetsLabel, EditApplyText + "\n" + AutoApplyText);

                    autoApplyPresetsLabel.ForeColor = UntickedColor;
                }
                else
                {
                    autoApplyPresetsLabel.Text = EditApplyText + AutoApplyDisabledText.ToUpper();

                    toolTip1.SetToolTip(autoApplyPresetsLabel, EditApplyText + "\n" + AutoApplyDisabledText.ToUpper());

                    autoApplyPresetsLabel.ForeColor = TickedColor;
                }
                //tickedOnlyChecked = false;
                //tickedOnlyCheckBox.Enable(false);
            }
            else
            {
                if (SavedSettings.allowAsrLrPresetAutoExecution)
                {
                    autoApplyPresetsLabel.Text = EditApplyText + AutoApplyText + "\n"
                    + NowTickedText.ToUpper().Replace("%%TICKED-PRESETS%%", autoAppliedPresetCount.ToString());

                    toolTip1.SetToolTip(autoApplyPresetsLabel, EditApplyText + "\n" + AutoApplyText + "\n\n"
                        + ClickHereText.ToUpper() + "\n" + NowTickedText.ToUpper().Replace("%%TICKED-PRESETS%%", autoAppliedPresetCount.ToString()));
                }
                else
                {
                    autoApplyPresetsLabel.Text = EditApplyText + AutoApplyDisabledText.ToUpper() + "\n"
                    + NowTickedText.ToUpper().Replace("%%TICKED-PRESETS%%", autoAppliedPresetCount.ToString());

                    toolTip1.SetToolTip(autoApplyPresetsLabel, EditApplyText + "\n" + AutoApplyDisabledText.ToUpper() + "\n\n"
                        + ClickHereText.ToUpper() + "\n" + NowTickedText.ToUpper().Replace("%%TICKED-PRESETS%%", autoAppliedPresetCount.ToString()));
                }

                autoApplyPresetsLabel.ForeColor = TickedColor;
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


            var checkedChangedPreset = (Preset)presetList.Items[e.Index];

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
                }
            }


            if (processPresetChanges)
            {
                showAutoApplyingWarningIfRequired();
                setPresetsChanged();
            }
        }

        private void idTextBox_Leave(object sender, EventArgs e)
        {
            if (idTextBox.Text == selectedPreset.id)
            {
                return;
            }
            else if (idTextBox.Text == string.Empty)
            {
                asrIdsPresetGuids.RemoveExisting(selectedPreset.id);

                selectedPreset.id = string.Empty;
                selectedPreset.setCustomizationsFlag(this, backedUpPreset);

                return;
            }

            var allowedRemoved = Regex.Replace(idTextBox.Text, @"[0-9]", string.Empty);
            allowedRemoved = Regex.Replace(allowedRemoved, @"[a-z]", string.Empty);
            allowedRemoved = Regex.Replace(allowedRemoved, @"[A-Z]", string.Empty);
            allowedRemoved = Regex.Replace(allowedRemoved, @"\-", string.Empty);
            allowedRemoved = Regex.Replace(allowedRemoved, @"_", string.Empty);
            allowedRemoved = Regex.Replace(allowedRemoved, @"\:", string.Empty);

            if (allowedRemoved != string.Empty)
            {
                MessageBox.Show(this, MsgNotAllowedSymbols, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                if (presetList.SelectedItem == selectedPreset)
                    idTextBox.Text = selectedPreset.id;

                idTextBox.Focus();
            }
            else
            {
                foreach (var idGuid in asrIdsPresetGuids)
                {
                    if (idTextBox.Text == idGuid.Key && selectedPreset.guid != idGuid.Value)
                    {
                        MessageBox.Show(this, MsgPresetExists.Replace("%%ID%%", idTextBox.Text), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        if (presetList.SelectedItem == selectedPreset)
                            idTextBox.Text = selectedPreset.id;

                        idTextBox.Focus();

                        return;
                    }
                }


                asrIdsPresetGuids.RemoveExisting(selectedPreset.id);

                selectedPreset.id = idTextBox.Text;
                asrIdsPresetGuids.Add(selectedPreset.id, selectedPreset.guid);

                selectedPreset.setCustomizationsFlag(this, backedUpPreset);
            }
        }

        private void clearIdButton_Click(object sender, EventArgs e)
        {
            idTextBox.Text = string.Empty;
            idTextBox_Leave(null, null);
        }

        private void applyToPlayingTrackCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            selectedPreset.applyToPlayingTrack = applyToPlayingTrackCheckBox.Checked;
            selectedPreset.setCustomizationsFlag(this, backedUpPreset);
        }

        private void applyToPlayingTrackCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!applyToPlayingTrackCheckBox.IsEnabled())
                return;

            applyToPlayingTrackCheckBox.Checked = !applyToPlayingTrackCheckBox.Checked;
        }

        private void favoriteCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!processPresetChanges)
                return;

            selectedPreset.favorite = favoriteCheckBox.Checked;
            selectedPreset.setCustomizationsFlag(this, backedUpPreset);
            refreshPresetList(selectedPreset.guid);
        }

        private void favoriteCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!favoriteCheckBox.IsEnabled())
                return;

            favoriteCheckBox.Checked = !favoriteCheckBox.Checked;
        }

        private void saveColumnWidths(Preset savedPreset)
        {
            if (savedPreset == null || !processPresetChanges || !previewTableLeftMouseButtonPressed || backgroundTaskIsWorking())
                return;


            if (savedPreset.columnWeights == null)
                savedPreset.columnWeights = new List<float>();

            if (savedPreset.columnWeights.Count != previewTable.ColumnCount)
            {
                savedPreset.columnWeights.Clear();
                for (var i = 0; i < previewTable.ColumnCount; i++)
                    savedPreset.columnWeights.Add(previewTable.Columns[i].FillWeight);
            }
            else
            {
                for (var i = 0; i < previewTable.ColumnCount; i++)
                    savedPreset.columnWeights[i] = previewTable.Columns[i].FillWeight;
            }


            bool failed = true;
            while (failed)
            {
                try
                {
                    previewTable.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    savedPreset.columnWeights[3] = previewTable.Columns[3].Width;
                    failed = false;
                }
                catch
                {
                    //Let's retry...
                    Thread.Sleep(ActionRetryDelay);
                }
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

            if (selectedPreset.userPreset)
            {
                editPreset(selectedPreset, false, false);
            }
            else
            {
                if (!SavedSettings.dontShowPredefinedPresetsCantBeChangedMessage &&
                    MessageBox.Show(this, MsgAsrPredefinedPresetsCantBeChanged,
                    string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    SavedSettings.dontShowPredefinedPresetsCantBeChangedMessage = true;
                }

                editPreset(selectedPreset, false, true);
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
            var settings = new QuickSettings(TagToolsPlugin);
            Display(settings, true);
            showAutoApplyingWarningIfRequired();
        }

        private void setCheckedPicturesStates()
        {
            var checkedCount = 0;
            var checkedFilter = 0;

            if (showHotkeyAssignedChecked)
            {
                hotkeyPictureBox.Image = ReplaceBitmap(hotkeyPictureBox.Image, HotkeyPresetsAccent);
                checkedCount++;
                checkedFilter = 7;
            }
            else
            {
                hotkeyPictureBox.Image = ReplaceBitmap(hotkeyPictureBox.Image, HotkeyPresetsDimmed);
            }

            if (showFunctionIdAssignedChecked)
            {
                functionIdPictureBox.Image = ReplaceBitmap(functionIdPictureBox.Image, FunctionIdPresetsAccent);
                checkedCount++;
                checkedFilter = 6;
            }
            else
            {
                functionIdPictureBox.Image = ReplaceBitmap(functionIdPictureBox.Image, FunctionIdPresetsDimmed);
            }

            if (showPlaylistLimitedChecked)
            {
                playlistPictureBox.Image = ReplaceBitmap(playlistPictureBox.Image, PlaylistPresetsAccent);
                checkedCount++;
                checkedFilter = 5;
            }
            else
            {
                playlistPictureBox.Image = ReplaceBitmap(playlistPictureBox.Image, PlaylistPresetsDimmed);
            }

            if (showUserChecked)
            {
                userPictureBox.Image = ReplaceBitmap(userPictureBox.Image, UserPresetsAccent);
                checkedCount++;
                checkedFilter = 4;
            }
            else
            {
                userPictureBox.Image = ReplaceBitmap(userPictureBox.Image, UserPresetsDimmed);
            }

            if (showCustomizedChecked)
            {
                customizedPictureBox.Image = ReplaceBitmap(customizedPictureBox.Image, CustomizedPresetsAccent);
                checkedCount++;
                checkedFilter = 3;
            }
            else
            {
                customizedPictureBox.Image = ReplaceBitmap(customizedPictureBox.Image, CustomizedPresetsDimmed);
            }

            if (showPredefinedChecked)
            {
                predefinedPictureBox.Image = ReplaceBitmap(predefinedPictureBox.Image, PredefinedPresetsAccent);
                checkedCount++;
                checkedFilter = 2;
            }
            else
            {
                predefinedPictureBox.Image = ReplaceBitmap(predefinedPictureBox.Image, PredefinedPresetsDimmed);
            }

            if (showTickedOnlyChecked)
            {
                tickedOnlyPictureBox.Image = ReplaceBitmap(tickedOnlyPictureBox.Image, AutoAppliedPresetsAccent);
                checkedCount++;
                checkedFilter = 1;
            }
            else
            {
                tickedOnlyPictureBox.Image = ReplaceBitmap(tickedOnlyPictureBox.Image, AutoAppliedPresetsDimmed);
            }


            IgnoreFilterComboBoxSelectedIndexChanged = true;

            if (checkedCount == 0)
            {
                untickAllChecked = false;
                filterComboBoxCustom.SelectedIndex = 0;
            }
            else if (checkedCount == 1)
            {
                untickAllChecked = true;
                filterComboBoxCustom.SelectedIndex = checkedFilter;
            }
            else //if (checkedCount > 1)
            {
                untickAllChecked = true;
                filterComboBoxCustom.SelectedIndex = -1;
            }

            IgnoreFilterComboBoxSelectedIndexChanged = false;


            if (untickAllChecked)
                uncheckAllFiltersPictureBox.Image = ReplaceBitmap(uncheckAllFiltersPictureBox.Image, UncheckAllFiltersAccent);
            else
                uncheckAllFiltersPictureBox.Image = ReplaceBitmap(uncheckAllFiltersPictureBox.Image, UncheckAllFiltersDimmed);

        }

        private void AdvancedSearchAndReplace_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!processPresetChanges)
            {
                e.Cancel = true;
            }
            else if (!PluginClosing && unsavedChanges)
            {
                var lastAnswer = SavedSettings.unsavedChangesConfirmationLastAnswer;
                var confirmationButtons = MessageBoxButtons.YesNo;

                if (!forceCloseForms)
                    confirmationButtons = MessageBoxButtons.YesNoCancel;

                forceCloseForms = true;

                var result = MessageBox.Show(this, MsgAsrDoYouWantToSaveChangesBeforeClosingTheWindow,
                    string.Empty, confirmationButtons, MessageBoxIcon.Warning, lastAnswer);


                if (result == DialogResult.Yes)
                {
                    SavedSettings.unsavedChangesConfirmationLastAnswer = MessageBoxDefaultButton.Button1;
                    saveSettings();
                }
                else if (result == DialogResult.No)
                {
                    SavedSettings.unsavedChangesConfirmationLastAnswer = MessageBoxDefaultButton.Button2;
                }
                else //if (result == DialogResult.Cancel)
                {
                    //SavedSettings.asrUnsavedChangesLastAnswer = MessageBoxDefaultButton.Button3;
                    e.Cancel = true;
                }
            }
        }

        private void AdvancedSearchAndReplace_Load(object sender, EventArgs e)
        {
            var value = loadWindowLayout();

            ignoreSplitterMovedEvent = true;

            //Let's scale split containers manually (auto-scaling is improper)
            foreach (var scsa in splitContainersScalingAttributes)
            {
                var sc = scsa.splitContainer;
                sc.Panel1MinSize = (int)Math.Round(scsa.panel1MinSize * vDpiFontScaling);
                sc.Panel2MinSize = (int)Math.Round(scsa.panel2MinSize * vDpiFontScaling);

                if (value.Item4 != 0)
                {
                    sc.SplitterDistance = (int)Math.Round(value.Item4 * vDpiFontScaling);
                }
                else
                {
                    sc.SplitterDistance = (int)Math.Round(scsa.splitterDistance * vDpiFontScaling);
                }
            }

            ignoreSplitterMovedEvent = false; //-V3008
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


            if (ModifierKeys == Keys.Control && filterComboBoxCustom.SelectedIndex > 0)
            {
                switch (filterComboBoxCustom.SelectedIndex)
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
            else if (filterComboBoxCustom.SelectedIndex >= 0)
            {
                showTickedOnlyChecked = false;
                showPredefinedChecked = false;
                showCustomizedChecked = false;
                showUserChecked = false;
                showPlaylistLimitedChecked = false;
                showFunctionIdAssignedChecked = false;
                showHotkeyAssignedChecked = false;

                switch (filterComboBoxCustom.SelectedIndex)
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

    partial class Plugin
    {
        internal void AsrPreset1EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplace.ApplyPreset(1);
        }

        internal void AsrPreset2EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplace.ApplyPreset(2);
        }

        internal void AsrPreset3EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplace.ApplyPreset(3);
        }

        internal void AsrPreset4EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplace.ApplyPreset(4);
        }

        internal void AsrPreset5EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplace.ApplyPreset(5);
        }

        internal void AsrPreset6EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplace.ApplyPreset(6);
        }

        internal void AsrPreset7EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplace.ApplyPreset(7);
        }

        internal void AsrPreset8EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplace.ApplyPreset(8);
        }

        internal void AsrPreset9EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplace.ApplyPreset(9);
        }

        internal void AsrPreset10EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplace.ApplyPreset(10);
        }

        internal void AsrPreset11EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplace.ApplyPreset(11);
        }

        internal void AsrPreset12EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplace.ApplyPreset(12);
        }

        internal void AsrPreset13EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplace.ApplyPreset(13);
        }

        internal void AsrPreset14EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplace.ApplyPreset(14);
        }

        internal void AsrPreset15EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplace.ApplyPreset(15);
        }

        internal void AsrPreset16EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplace.ApplyPreset(16);
        }

        internal void AsrPreset17EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplace.ApplyPreset(17);
        }

        internal void AsrPreset18EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplace.ApplyPreset(18);
        }

        internal void AsrPreset19EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplace.ApplyPreset(19);
        }

        internal void AsrPreset20EventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplace.ApplyPreset(20);
        }
    }
}
