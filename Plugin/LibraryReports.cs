﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

using ExtensionMethods;

using MusicBeePlugin.Properties;

using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class LibraryReports : PluginWindowTemplate
    {
        internal protected bool forceCloseForms = true;

        internal class Row : DataGridViewBoundColumns
        {
            internal string artworkBase64Hash;

            public Row() : base()
            {
                //Nothing...
            }
        }

        private CustomComboBox precisionDigitsComboBoxCustom;
        private CustomComboBox mulDivFactorComboBoxCustom;
        private CustomComboBox operationComboBoxCustom;
        private CustomComboBox destinationTagListCustom;
        private CustomComboBox sourceFieldComboBoxCustom;
        private CustomComboBox comparedFieldListCustom;
        private CustomComboBox conditionListCustom;
        private CustomComboBox conditionFieldListCustom;
        private CustomComboBox formatComboBoxCustom;
        private CustomComboBox useAnotherPresetAsSourceComboBoxCustom;
        private CustomComboBox multipleItemsSplitterComboBoxCustom;
        private CustomComboBox parameter2ComboBoxCustom;
        private CustomComboBox sourceTagListCustom;
        private CustomComboBox functionComboBoxCustom;


        private static string DontUseSplitter;

        private Bitmap warningWide;
        private Bitmap warning;

        private DataGridViewCellStyle headerCellStyle;

        private DataGridViewTextBoxColumn columnTemplate;
        private DataGridViewImageColumn artworkColumnTemplate;

        private System.Threading.Timer periodicCacheClearingTimer = null;
        private static bool DelayedFunctionCacheUpdatePrioritySet = false;


        private DataGridViewBoundColumnList<Row> rows;
        private BindingSource source = new BindingSource();

        private void periodicCacheClearing(object state)
        {
            if ((PluginClosing || SavedSettings.dontShowLibraryReports || CachedPresetsTags.Count > PresetCacheCountThreshold)
                && !BackgroundTaskIsInProgress)
            {
                lock (CachedPresetsTags)
                {
                    if (CachedPresetsTags.Count > PresetCacheCountCriticalThreshold)
                    {
                        lastCachedAppliedPresetGuid = Guid.NewGuid();
                        cachedAppliedPresetGuids.Clear();

                        CachedPresetsTags.Clear();

                        CachedPresetsGroupingsTagIds.Clear();
                        CachedPresetsGroupingsPropIds.Clear();
                        CachedPresetsActualGroupingsTagIds.Clear();
                        CachedPresetsActualGroupingsPropIds.Clear();
                        CachedPresetsNativeTagNames.Clear();

                        CachedPresetsFilesActualComposedSplitGroupingTagsList.Clear();
                        CachedPresetsFilesActualGroupingTags.Clear();
                        CachedPresetsFilesActualGroupingTagsRaw.Clear();

                        clearArtworks();
                    }
                    else
                    {
                        var clearedSome = false;

                        foreach (var guid in cachedAppliedPresetGuids.Keys)
                        {
                            if (CachedPresetsTags.Count > TagsCacheClearingGroupingsCountThreshold || !AllLrPresetGuidsInUse.ContainsKey(guid))
                            {
                                clearedSome = true;

                                if (guid == lastCachedAppliedPresetGuid)
                                    lastCachedAppliedPresetGuid = Guid.NewGuid();

                                cachedAppliedPresetGuids.Remove(guid);

                                CachedPresetsTags.Remove(guid);

                                CachedPresetsGroupingsTagIds.Remove(guid);
                                CachedPresetsGroupingsPropIds.Remove(guid);
                                CachedPresetsActualGroupingsTagIds.Remove(guid);
                                CachedPresetsActualGroupingsPropIds.Remove(guid);
                                CachedPresetsNativeTagNames.Remove(guid);
                                CachedPresetsFilesActualGroupingTags.Remove(guid);
                                CachedPresetsFilesActualGroupingTagsRaw.Remove(guid);
                                CachedPresetsFilesActualComposedSplitGroupingTagsList.Remove(guid);
                            }
                        }

                        if (clearedSome)
                            clearArtworks();
                    }
                }
            }

            if (PluginClosing || SavedSettings.dontShowLibraryReports
                || (!ReportPresetIdsAreInitialized && LibraryReportsCommandForHotkeys == null && LibraryReportsCommandForAutoApplying == null))
            {
                periodicCacheClearingTimer?.Dispose();
                periodicCacheClearingTimer = null;
            }
        }


        private static readonly List<string> ProcessedReportDeletions = new List<string>();

        //Cached UI and events workarounds
        internal static Bitmap DefaultArtwork; //Not resized. Only a sample.
        internal static string ResizedDefaultArtworkBase64; //Resized artwork base64 string. Artwork is stored in "artworkCache"
        internal static string ResizedDefaultArtworkBase64Hash; //Resized artwork base64 hash string. Artwork is stored in "artworkCache"

        internal static Bitmap ArtworkTotals; //Not resized. Only a sample.
        internal static string ResizedArtworkTotalsId = "%%ALL-ARTWORKS%%"; //Resized artwork base64 string. Artwork is stored in "artworkCache"

        internal static SortedDictionary<string, Bitmap> ResizedArtworkCache = new SortedDictionary<string, Bitmap>(); //<resized base64, resized artwork>
        private static Bitmap CdBookletArtwork; //Not resized common artwork ror CD booklet export
        private static string CdBookletArtworkBase64 = null; //Base64 string of not resized common artwork ror CD booklet export

        private Font totalsFont; //Preview table font for "Totals" cells

        private const int MaxColumnRepresentationLength = 80; //---
        private const int MaxExprRepresentationLength = 40;

        private const int PreviewTableColumnMinimumWidth = 75;
        private const int PreviewTableColumnMaximumWidth = 200; //px. //---

        private const int PreviewTableDefaultArtworkSize = 200;
        private int previewTableArtworkSize = PreviewTableDefaultArtworkSize;

        private static string AutoApplyText;
        private static string NowTickedText;

        private static string AutoApplyDisabledText;

        private static string ButtonFilterResultsText;
        private static string ButtonFilterResultsToolTip;

        private static string ButtonAddFunctionCreateText;
        private static string ButtonAddFunctionCreateToolTip;
        private static string ButtonAddFunctionDiscardText;
        private static string ButtonAddFunctionDiscardToolTip;

        private static string ButtonUpdateFunctionSaveText;
        private static string ButtonUpdateFunctionUpdateText;

        private static string ButtonUpdateFunctionSaveToolTip;
        private static string ButtonUpdateFunctionUpdateToolTip;

        private static string MsgHelp;

        private static string TagsDataGridViewCheckBoxToolTip;
        private static string ExpressionsDataGridViewCheckBoxToolTip;

        private static string MsgDoYouWantToDeleteTheField;
        private static string MsgDoYouWantToReplaceTheField;

        private static string NoExpressionText;//===
        private static string AutoColumnNameText;

        private bool ignoreCheckedPresetEvent = true;
        private int autoAppliedPresetCount;

        private string assignHotkeyCheckBoxText;
        private int reportPresetsWithHotkeysCount;
        private readonly bool[] reportPresetHotkeyUsedSlots = new bool[MaximumNumberOfLrHotkeys];

        private bool unsavedChanges;
        private string buttonCloseToolTip;
        private int presetListLastSelectedIndex = -2;

        private bool ignoreSplitterMovedEvent = true;

        private ReportPreset selectedPreset;

        private bool presetIsLoading;
        private bool sourceFieldComboBoxIndexChanging;

        internal static volatile bool BackgroundTaskIsInProgress;

        //Not reliable. Make sure to call checkAllPresetChains() before use!
        private bool resultsAreFiltered;
        private AggregatedTags previewedTags;
        private SortedDictionary<int, List<string>> previewedFilesActualComposedSplitGroupingTagsList;

        //Working locals
        internal ReportPreset appliedPreset;
        internal ReportPreset[] reportPresets;
        private static SortedDictionary<Guid, ReportPreset> PresetsInteractiveWorkingCopy = new SortedDictionary<Guid, ReportPreset>(); //<Permanent guid, preset>

        private readonly SortedDictionary<Guid, bool> cachedAppliedPresetGuids = new SortedDictionary<Guid, bool>();
        private Guid lastCachedAppliedPresetGuid;
        private static readonly List<Guid> PresetsProcessedByFunctionCacheUpdate = new List<Guid>(); //<ReportPreset.guid>

        private static bool[] ColumnsRightAlignment; //Cache for preview table/exported report 
        private ResultType[] columnTypes; //Cache for preview table/exported report 


        private static readonly SortedDictionary<Guid, AggregatedTags> CachedPresetsTags = new SortedDictionary<Guid, AggregatedTags>();

        private static readonly SortedDictionary<Guid, MetaDataType[]> CachedPresetsGroupingsTagIds = new SortedDictionary<Guid, MetaDataType[]>();
        private static readonly SortedDictionary<Guid, FilePropertyType[]> CachedPresetsGroupingsPropIds = new SortedDictionary<Guid, FilePropertyType[]>();
        private static readonly SortedDictionary<Guid, MetaDataType[]> CachedPresetsActualGroupingsTagIds = new SortedDictionary<Guid, MetaDataType[]>();
        private static readonly SortedDictionary<Guid, FilePropertyType[]> CachedPresetsActualGroupingsPropIds = new SortedDictionary<Guid, FilePropertyType[]>();
        private static readonly SortedDictionary<Guid, string[]> CachedPresetsNativeTagNames = new SortedDictionary<Guid, string[]>();

        //<Preset GUID, <TrackId, Grouping tags[]>>
        private static readonly SortedDictionary<Guid, SortedDictionary<int, string[]>> CachedPresetsFilesActualGroupingTags
            = new SortedDictionary<Guid, SortedDictionary<int, string[]>>();
        //<Preset GUID, <TrackId, Grouping tags[]>>
        private static readonly SortedDictionary<Guid, SortedDictionary<int, string[]>> CachedPresetsFilesActualGroupingTagsRaw
            = new SortedDictionary<Guid, SortedDictionary<int, string[]>>();
        //<Preset GUID, <TrackId, List of <composed groupings>>>
        private static readonly SortedDictionary<Guid, SortedDictionary<int, List<string>>> CachedPresetsFilesActualComposedSplitGroupingTagsList
            = new SortedDictionary<Guid, SortedDictionary<int, List<string>>>();


        private string[] lastFiles;


        private bool hidePreview;

        private Comparison comparison;
        private string comparedFieldText;

        private int conditionField = -1;
        private int comparedField = -1;

        private int artworkField = -1;
        private int sequenceNumberField = -1;

        private readonly List<MetaDataType> destinationTagIds = new List<MetaDataType>();

        private string expressionBackup = string.Empty;//===
        private string splitterBackup = string.Empty;
        private bool trimValuesBackup;

        private string columnNameBackup = string.Empty;

        private bool? newColumn = false;
        private int tagsDataGridViewSelectedRow = -1;

        private readonly PresetColumnAttributesDict groupings = new PresetColumnAttributesDict(); //Short ids, attributes (various expressions)


        //Working locals & UI preset caching
        private List<string> sortedShortIds = new List<string>(); //Short ids
        private readonly Dictionary<string, List<string>> shortIdsExprs = new Dictionary<string, List<string>>(); //Short ids, expressions
        private readonly Dictionary<string, List<string>> shortIdsColNames = new Dictionary<string, List<string>>(); //Short ids, column names //===

        private readonly ColumnAttributesDict groupingsDict = new ColumnAttributesDict();
        private readonly ColumnAttributesDict functionsDict = new ColumnAttributesDict();

        private readonly List<string> savedFunctionIds = new List<string>();

        private readonly List<int> operations = new List<int>();
        private readonly List<string> mulDivFactors = new List<string>();
        private readonly List<string> precisionDigits = new List<string>();
        private readonly List<string> appendTexts = new List<string>();

        //UI preset caching
        private readonly List<string> savedDestinationTagsNames = new List<string>();
        private bool smartOperation;


        //Set timer for cache periodic cleanup in constructor
        public LibraryReports()
        {
            //It's not GUI control, not a Form in this case
            //InitializeComponent();

            periodicCacheClearingTimer = new System.Threading.Timer(periodicCacheClearing, null, FunctionCacheClearingDelay, FunctionCacheClearingDelay);
        }

        public LibraryReports(Plugin plugin) : base(plugin)
        {
            InitializeComponent();

            WindowIcon = LrIcon;
            TitleBarText = this.Text;

            new ControlBorder(this.presetNameTextBox);
            new ControlBorder(this.appendTextBox);
            new ControlBorder(this.idTextBox);
            new ControlBorder(this.expressionTextBox);
            new ControlBorder(this.columnNameTextBox);
        }

        internal protected override void initializeForm()
        {
            base.initializeForm();


            Enable(false, autoApplyPresetsLabel, presetList);


            previewTable.ColumnWidthChanged += previewTable_ColumnWidthChanged;


            precisionDigitsComboBoxCustom = namesComboBoxes["precisionDigitsComboBox"];
            mulDivFactorComboBoxCustom = namesComboBoxes["mulDivFactorComboBox"];
            operationComboBoxCustom = namesComboBoxes["operationComboBox"];
            destinationTagListCustom = namesComboBoxes["destinationTagList"];
            sourceFieldComboBoxCustom = namesComboBoxes["sourceFieldComboBox"];
            comparedFieldListCustom = namesComboBoxes["comparedFieldList"];
            conditionListCustom = namesComboBoxes["conditionList"];
            conditionFieldListCustom = namesComboBoxes["conditionFieldList"];
            formatComboBoxCustom = namesComboBoxes["formatComboBox"];
            useAnotherPresetAsSourceComboBoxCustom = namesComboBoxes["useAnotherPresetAsSourceComboBox"];
            multipleItemsSplitterComboBoxCustom = namesComboBoxes["multipleItemsSplitterComboBox"];
            parameter2ComboBoxCustom = namesComboBoxes["parameter2ComboBox"];
            sourceTagListCustom = namesComboBoxes["sourceTagList"];
            functionComboBoxCustom = namesComboBoxes["functionComboBox"];

            if (useSkinColors)
                multipleItemsSplitterComboBox.SelectedIndexChanged += multipleItemsSplitterComboBox_DropDownClosed;

            warningWide = ReplaceBitmap(null, WarningWide);
            warning = ReplaceBitmap(null, Warning);

            useAnotherPresetAsSourceLabel.Left = totalsCheckBoxLabel.Left;

            //Setting control not standard properties
            //var heightField = presetList.GetType().GetField(
            //   "scaledListItemBordersHeight",
            //   BindingFlags.NonPublic | BindingFlags.Instance
            //);

            //var addedHeight = 4; //Some appropriate value, greater than the field's default of 2
            //heightField.SetValue(presetList, addedHeight); //Where "presetList" is your CheckedListBox


            if (DontUseSplitter == null)
                DontUseSplitter = (multipleItemsSplitterComboBoxCustom.Items[0] as string).TrimStart(' ');

            //Setting themed images
            buttonLabels[buttonClearExpression] = string.Empty;
            buttonClearExpression.Text = string.Empty;
            ReplaceButtonBitmap(buttonClearExpression, ClearField);

            buttonLabels[clearIdButton] = string.Empty;
            clearIdButton.Text = string.Empty;
            ReplaceButtonBitmap(clearIdButton, ClearField);

            openReportCheckBoxPicture.Image = ReplaceBitmap(openReportCheckBoxPicture.Image, Window);
            ReplaceButtonBitmap(buttonSettings, Gear);


            //Clearing "unsaved changes" button image
            buttonClose.Image = Resources.transparent_15;
            buttonCloseToolTip = toolTip1.GetToolTip(buttonClose);
            toolTip1.SetToolTip(buttonClose, string.Empty);


            //Preparing "ticked presets" label text
            var entireText = autoApplyPresetsLabel.Text;
            AutoApplyText = Regex.Replace(entireText, @"^(.*?)~.*?~.*", "$1");
            NowTickedText = Regex.Replace(entireText, @"^.*?~(.*?)~.*", "$1");
            AutoApplyDisabledText = Regex.Replace(entireText, @"^.*?~.*?~(.*)", "$1");

            if (SavedSettings.allowAsrLrPresetAutoExecution)
            {
                autoApplyPresetsLabel.Text = AutoApplyText;
                autoApplyPresetsLabel.ForeColor = UntickedColor;
            }
            else
            {
                autoApplyPresetsLabel.Text = AutoApplyText + "\n" + AutoApplyDisabledText.ToUpper();
                autoApplyPresetsLabel.ForeColor = TickedColor;
            }


            hidePreviewCheckBox.Checked = SavedSettings.hideLrPreview;

            //Clearing "source preset is missing" check box image
            useAnotherPresetAsSourceLabel.Image = Resources.transparent_15;


            //Saving buttonFilterResults tool tip & button label
            ButtonFilterResultsText = buttonLabels[buttonFilterResults];
            ButtonFilterResultsToolTip = toolTip1.GetToolTip(buttonFilterResults);


            //Saving Create/Discard new field button labels & tool tip
            //string buttonAddFunctionText = buttonAddFunction.Text;
            var buttonAddFunctionText = buttonLabels[buttonAddFunction];
            ButtonAddFunctionCreateText = Regex.Replace(buttonAddFunctionText, @"^(.*?)\:(.*)", "$1");
            ButtonAddFunctionDiscardText = Regex.Replace(buttonAddFunctionText, @"^(.*?)\:(.*)", "$2");
            //buttonAddFunction.Text = ButtonAddFunctionCreateText;
            buttonAddFunction.Text = ButtonAddFunctionCreateText;

            var buttonAddFunctionToolTip = toolTip1.GetToolTip(buttonAddFunction);
            ButtonAddFunctionCreateToolTip = Regex.Replace(buttonAddFunctionToolTip, @"^(.*?)\:(.*)", "$1");
            ButtonAddFunctionDiscardToolTip = Regex.Replace(buttonAddFunctionToolTip, @"^(.*?)\:(.*)", "$2");
            toolTip1.SetToolTip(buttonAddFunction, ButtonAddFunctionCreateToolTip);


            //Saving Save/Update field button labels & tool tip
            //string buttonUpdateFunctionText = buttonUpdateFunction.Text;
            var buttonUpdateFunctionText = buttonLabels[buttonUpdateFunction];
            ButtonUpdateFunctionSaveText = Regex.Replace(buttonUpdateFunctionText, @"^(.*?)\:(.*)", "$1");
            ButtonUpdateFunctionUpdateText = Regex.Replace(buttonUpdateFunctionText, @"^(.*?)\:(.*)", "$2");
            //buttonUpdateFunction.Text = ButtonUpdateFunctionSaveText;
            buttonUpdateFunction.Text = ButtonUpdateFunctionSaveText;

            var buttonUpdateFunctionToolTip = toolTip1.GetToolTip(buttonUpdateFunction);
            ButtonUpdateFunctionSaveToolTip = Regex.Replace(buttonUpdateFunctionToolTip, @"^(.*?)\:(.*)", "$1");
            ButtonUpdateFunctionUpdateToolTip = Regex.Replace(buttonUpdateFunctionToolTip, @"^(.*?)\:(.*)", "$2");
            toolTip1.SetToolTip(buttonUpdateFunction, ButtonUpdateFunctionSaveToolTip);


            //Saving Help button tool tip
            var buttonHelpInitialToolTip = toolTip1.GetToolTip(buttonHelp);
            var buttonHelpToolTip = Regex.Replace(buttonHelpInitialToolTip, @"^(.*?)\r\n\r\n((.|\r|\n)*)", "$1");
            MsgHelp = Regex.Replace(buttonHelpInitialToolTip, @"^(.*?)\r\n\r\n((.|\r|\n)*)", "$2");
            toolTip1.SetToolTip(buttonHelp, buttonHelpToolTip);


            //Saving field & expression tables tooltips for check boxes
            TagsDataGridViewCheckBoxToolTip = tagsDataGridView.Columns[0].HeaderCell.ToolTipText;
            tagsDataGridView.Columns[0].HeaderCell.ToolTipText = null;

            ExpressionsDataGridViewCheckBoxToolTip = expressionsDataGridView.Columns[0].HeaderCell.ToolTipText;
            expressionsDataGridView.Columns[0].HeaderCell.ToolTipText = null;


            //Saving text for message box shown on last expression removing
            MsgDoYouWantToDeleteTheField = expressionsDataGridView.Columns[1].HeaderCell.ToolTipText;
            expressionsDataGridView.Columns[1].HeaderCell.ToolTipText = null;


            //Saving text for message box shown on field replacing
            MsgDoYouWantToReplaceTheField = tagsDataGridView.Columns[1].HeaderCell.ToolTipText;
            tagsDataGridView.Columns[1].HeaderCell.ToolTipText = null;


            //Initialization
            var formats = ExportedFormats.Split('|');
            for (var i = 0; i < formats.Length; i += 2)
                formatComboBoxCustom.Add(formats[i]);

            smartOperationCheckBox.Checked = SavedSettings.smartOperation;

            foreach (var fname in LrFunctionNames)
                functionComboBoxCustom.Add(fname);

            FillListByTagNames(sourceTagListCustom.Items, true, true, false);
            FillListByPropNames(sourceTagListCustom.Items);
            sourceTagListCustom.Add(SequenceNumberName);

            FillListByTagNames(parameter2ComboBoxCustom.Items, true, false, false);
            FillListByPropNames(parameter2ComboBoxCustom.Items);
            parameter2ComboBoxCustom.SelectedIndex = 0;

            conditionListCustom.Add(ListItemConditionIs);
            conditionListCustom.Add(ListItemConditionIsNot);
            conditionListCustom.Add(ListItemConditionIsGreater);
            conditionListCustom.Add(ListItemConditionIsGreaterOrEqual);
            conditionListCustom.Add(ListItemConditionIsLess);
            conditionListCustom.Add(ListItemConditionIsLessOrEqual);


            presetNameTextBox.SetCue(CtlLrPresetAutoName);

            NoExpressionText = expressionTextBox.Text;
            expressionTextBox.SetCue(NoExpressionText);

            AutoColumnNameText = columnNameTextBox.Text;
            columnNameTextBox.SetCue(AutoColumnNameText);

            PresetsInteractiveWorkingCopy.Clear();

            lock (SavedSettings.reportPresets)
            {
                ignoreCheckedPresetEvent = false;
                autoAppliedPresetCount = 0;
                reportPresetsWithHotkeysCount = 0;

                for (var i = 0; i < SavedSettings.reportPresets.Length; i++)
                {
                    var preset = new ReportPreset(SavedSettings.reportPresets[i], true);

                    presetList.Items.Add(preset, preset.autoApply);
                    PresetsInteractiveWorkingCopy.Add(preset.permanentGuid, preset);

                    if (preset.hotkeyAssigned)
                    {
                        reportPresetsWithHotkeysCount++;
                        reportPresetHotkeyUsedSlots[preset.hotkeySlot] = true;
                    }
                }

                ignoreCheckedPresetEvent = true;
            }


            assignHotkeyCheckBoxText = Regex.Replace(assignHotkeyCheckBoxLabel.Text, "^(.*:\\s).*", "$1");
            assignHotkeyCheckBoxLabel.Text = assignHotkeyCheckBoxText + (MaximumNumberOfLrHotkeys - reportPresetsWithHotkeysCount) + "/" + MaximumNumberOfLrHotkeys;


            FillListByTagNames(destinationTagListCustom.Items);


            //"Totals" font
            var tagNameFontSize = previewTable.DefaultCellStyle.Font.Size * 0.7f; //Maybe it's worth to adjust fine size !!!
            totalsFont = new Font(Font.FontFamily, tagNameFontSize, FontStyle.Bold);


            headerCellStyle = new DataGridViewCellStyle(HeaderCellStyle);

            previewTable.ColumnHeadersDefaultCellStyle = headerCellStyle;
            previewTable.RowHeadersDefaultCellStyle = headerCellStyle;

            //previewTable.TopLeftHeaderCell.Style = headerCellStyle;
            //previewTable.TopLeftHeaderCell.Value = CtlTags;

            previewTable.Columns[0].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[1].HeaderCell.Style = headerCellStyle;

            columnTemplate = previewTable.Columns[0].Clone() as DataGridViewTextBoxColumn;
            columnTemplate.Resizable = DataGridViewTriState.True;
            columnTemplate.MinimumWidth = PreviewTableColumnMinimumWidth;
            columnTemplate.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            columnTemplate.SortMode = DataGridViewColumnSortMode.Programmatic;


            artworkColumnTemplate = previewTable.Columns[1].Clone() as DataGridViewImageColumn;
            artworkColumnTemplate.ImageLayout = DataGridViewImageCellLayout.Zoom;
            artworkColumnTemplate.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            artworkColumnTemplate.Resizable = DataGridViewTriState.True;
            artworkColumnTemplate.SortMode = DataGridViewColumnSortMode.NotSortable;

            previewTable.DefaultCellStyle = new DataGridViewCellStyle(UnchangedCellStyle);
            previewTable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.NotSet;

            previewTable.ColumnCount = 0;


            resizeArtworkCheckBox.Checked = false;
            newArtworkSizeUpDown.Value = 300;

            openReportCheckBox.Checked = SavedSettings.openReportAfterExporting;


            tagsDataGridView.Columns[0].HeaderCell.Style = headerCellStyle;
            tagsDataGridView.Columns[1].HeaderCell.Style = headerCellStyle;
            tagsDataGridView.Columns[2].HeaderCell.Style = headerCellStyle;
            tagsDataGridView.Columns[3].HeaderCell.Style = headerCellStyle;

            expressionsDataGridView.Columns[0].HeaderCell.Style = headerCellStyle;
            expressionsDataGridView.Columns[1].HeaderCell.Style = headerCellStyle;

            if (Language == "ru")
                tagsDataGridView.Columns[1].Width = 130;

            tagsDataGridView.Columns[1].Width = (int)Math.Round(tagsDataGridView.Columns[1].Width * hDpiFontScaling);


            presetListLastSelectedIndex = -2;
            presetList_SelectedIndexChanged(null, null);

            updateCustomScrollBars(presetList);


            Enable(true, autoApplyPresetsLabel, presetList);


            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        public class ColumnAttributesBase
        {
            private LrFunctionType _functionType;
            private string _parameterName;

            public LrFunctionType functionType
            {
                get { return _functionType; }

                set
                {
                    _functionType = value;

                    if (_functionType == LrFunctionType.Count)
                    {
                        resultType = ResultType.ItemCount;
                        dataType = DataType.Number;
                    }
                    else if (_functionType == LrFunctionType.AverageCount)
                    {
                        resultType = ResultType.AverageCount;
                        dataType = DataType.Number;
                    }

                }
            }

            public string parameterName
            {
                get { return _parameterName; }

                set
                {
                    _parameterName = value;

                    var tagId = GetTagId(_parameterName);
                    var propId = GetPropId(_parameterName);
                    dataType = MbApiInterface.Setting_GetDataType(tagId);

                    if (dataType == DataType.Number)
                    {
                        resultType = ResultType.Double;
                    }
                    else if (dataType == DataType.Rating)
                    {
                        resultType = ResultType.Double;
                    }
                    else if (propId == FilePropertyType.Duration)
                    {
                        resultType = ResultType.TimeSpan;
                    }
                    else if (dataType == DataType.DateTime)
                    {
                        resultType = ResultType.Year;
                    }
                    else if (_functionType == LrFunctionType.Count)
                    {
                        resultType = ResultType.ItemCount;
                        dataType = DataType.Number;
                    }
                    else if (_functionType == LrFunctionType.AverageCount)
                    {
                        resultType = ResultType.AverageCount;
                        dataType = DataType.Number;
                    }
                    else
                    {
                        resultType = ResultType.UseOtherResults;
                        dataType = DataType.String;
                    }
                }
            }

            public DataType dataType = DataType.String;
            public ResultType resultType = ResultType.UseOtherResults;
            public string parameter2Name;
            public string splitter;
            public bool trimValues;

            public ColumnAttributesBase()
            {
                //Nothing to do...
            }

            public ColumnAttributesBase(ColumnAttributesBase colAttr, bool fullCopy = false)
            {
                functionType = colAttr.functionType;
                parameterName = colAttr.parameterName;
                parameter2Name = colAttr.parameter2Name;
                splitter = colAttr.splitter;
                trimValues = colAttr.trimValues;

                if (fullCopy)
                {
                    dataType = colAttr.dataType;
                    resultType = colAttr.resultType;
                }
            }

            public ColumnAttributesBase(LrFunctionType functionType, string parameterName, string parameter2Name, string splitter, bool trimValues)
            {
                this.functionType = functionType;
                this.parameterName = parameterName;
                this.parameter2Name = parameter2Name;
                this.splitter = splitter;
                this.trimValues = trimValues;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            internal virtual string getShortId()
            {
                return ((int)functionType).ToString("D2") + "\u0007" + ((int)GetTagId(parameterName)).ToString("D4")
                    + "\u0007" + ((int)GetTagId(parameter2Name)).ToString("D4");
            }

            public override bool Equals(object obj)
            {
                if (obj is ColumnAttributesBase colAttrs)
                {
                    if (functionType != colAttrs.functionType)
                        return false;
                    else if (parameterName != colAttrs.parameterName)
                        return false;
                    else if (parameter2Name != colAttrs.parameter2Name)
                        return false;
                    else if (splitter != colAttrs.splitter)
                        return false;
                    else if (trimValues != colAttrs.trimValues)
                        return false;

                    return true;
                }

                return false;
            }
        }

        public class ColumnAttributes : ColumnAttributesBase
        {
            public string expression = string.Empty;
            public string columnName = string.Empty;

            public ColumnAttributes()
            {
                //Nothing to do...
            }

            public ColumnAttributes(ColumnAttributes sourceAttribs, bool fullCopy = false) : base(sourceAttribs, fullCopy)
            {
                expression = sourceAttribs.expression;

                if (fullCopy)
                    columnName = sourceAttribs.columnName;
            }

            public ColumnAttributes(LrFunctionType functionType, string expression, string columnName, string parameterName, string parameter2Name,
                string splitter, bool trimValues) : base(functionType, parameterName, parameter2Name, splitter, trimValues)
            {
                this.expression = expression;
                this.columnName = columnName;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            internal string getUniqueId()
            {
                return ((int)functionType).ToString("D2") + "\u0007" + ((int)GetTagId(parameterName)).ToString("D4")
                    + "\u0007" + ((int)GetTagId(parameter2Name)).ToString("D4") + "\u0007" + (expression ?? string.Empty);
            }

            public override bool Equals(object obj)
            {
                if (!base.Equals(obj))
                    return false;


                if (obj is ColumnAttributes colAttrs)
                {
                    if (expression == colAttrs.expression && columnName == colAttrs.columnName)
                        return true;
                }

                return false;
            }

            internal string getColumnName(bool trimName, bool includeSplitterTrimInfo, bool includeExpression)
            {
                return GetColumnName(parameterName, parameter2Name, functionType, splitter, trimValues, expression, columnName, trimName,
                    includeSplitterTrimInfo, includeExpression);
            }

            internal string evaluateExpression(string url, string tagValue)
            {
                if (string.IsNullOrWhiteSpace(expression))
                {
                    return tagValue;
                }
                else
                {
                    tagValue = tagValue.Replace('\"', '\u0007');
                    var workingExpression = expression.Replace("\\@", "\"" + tagValue + "\"");
                    tagValue = MbApiInterface.MB_Evaluate(workingExpression, url);
                    tagValue = tagValue.Replace('\u0007', '\"');

                    return tagValue;
                }
            }
        }

        public struct ColumnIndexTagValue
        {
            public int index;
            public string value;
        }

        public class PresetColumnAttributes : ColumnAttributesBase
        {
            public string[] expressions = new[] { string.Empty };
            public int[] columnIndices = Array.Empty<int>(); //per expression for current grouping/function
            public string[] columnNames = new[] { string.Empty };

            public PresetColumnAttributes()
            {
                //Nothing to do...
            }

            public PresetColumnAttributes(ColumnAttributesBase sourceAttribs, bool fullCopy = false) : base(sourceAttribs, fullCopy)
            {
                //Nothing to do...
            }

            public PresetColumnAttributes(PresetColumnAttributes sourceAttribs, bool fullCopy) : base(sourceAttribs, fullCopy)
            {
                expressions = sourceAttribs.expressions.Clone() as string[];
                columnIndices = sourceAttribs.columnIndices.Clone() as int[];
                columnNames = sourceAttribs.columnNames.Clone() as string[];
            }

            public PresetColumnAttributes(LrFunctionType functionType, string[] expressions, string[] columnNames, string parameterName, string parameter2Name,
                string splitter, bool trimValues)
                : base(functionType, parameterName, parameter2Name,
                    splitter, trimValues)
            {
                this.expressions = expressions;
                this.columnNames = columnNames;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            internal string getUniqueId(int exprIndex)
            {
                var expression = string.Empty;
                if (exprIndex >= 0)
                    expression = expressions[exprIndex];

                var parameterId = (int)GetTagId(parameterName);
                var parameter2Id = (int)GetTagId(parameter2Name);

                if (parameterId == 0)
                    return null;

                return ((int)functionType).ToString("D2") + "\u0007" + parameterId.ToString("D4")
                    + "\u0007" + parameter2Id.ToString("D4") + "\u0007" + expression;
            }

            public override bool Equals(object obj)
            {
                if (!base.Equals(obj))
                    return false;


                if (obj is ColumnAttributes colAttrs) //-V3197
                {
                    foreach (var expression in expressions)
                        if (expression != colAttrs.expression)
                            return false;

                    return true;
                }

                return false;
            }

            //returns {columnIndex, resulting tag value}, accepts int[] columnIndices: per expression for current grouping/function
            internal List<ColumnIndexTagValue> evaluateExpressions(string url, string tagValue)
            {
                tagValue = tagValue.Replace('\"', '\u0007');

                var tagExprValues = new List<ColumnIndexTagValue>();
                for (var i = 0; i < expressions.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(expressions[i]))
                    {
                        tagExprValues.Add(new ColumnIndexTagValue { index = columnIndices[i], value = tagValue.Replace('\u0007', '\"') });
                    }
                    else
                    {
                        var workingExpression = expressions[i].Replace("\\@", "\"" + tagValue + "\"");
                        var result = MbApiInterface.MB_Evaluate(workingExpression, url);
                        result = result.Replace('\u0007', '\"');

                        tagExprValues.Add(new ColumnIndexTagValue { index = columnIndices[i], value = result });
                    }
                }

                return tagExprValues;
            }

            //<columnIndex, resulting tag value>, int[] columnIndices: per expression for current grouping/function
            internal List<ColumnIndexTagValue> getSplitValues(string url, string tagValue)
            {
                var results = new List<ColumnIndexTagValue>();

                if (string.IsNullOrWhiteSpace(splitter))
                {
                    results.AddRange(evaluateExpressions(url, tagValue));
                }
                else
                {
                    var workingSplitter = splitter.Replace("\\0", "\u0000");
                    var splitValues = tagValue.Split(new[] { workingSplitter }, StringSplitOptions.None);
                    for (var i = 0; i < splitValues.Length; i++)
                    {
                        if (trimValues)
                            splitValues[i] = splitValues[i].Trim();

                        results.AddRange(evaluateExpressions(url, splitValues[i]));
                    }
                }

                return results;
            }
        }

        public class ColumnAttributesDict : Dictionary<string, ColumnAttributes> //Column unique ID (string), Column settings
        {
            public ColumnAttributesDict()
            {
                //Nothing to do...
            }

            public ColumnAttributesDict(ColumnAttributesDict dict, bool fullCopy = false)
            {
                foreach (var keyValue in dict)
                    Add(keyValue.Key, new ColumnAttributes(keyValue.Value, fullCopy));
            }

            public ColumnAttributesDict(ColumnAttributes[] attribCollection)
            {
                foreach (var attribs in attribCollection)
                    Add(attribs.getUniqueId(), attribs);
            }

            //Returns PresetColumnAttributes[], column count
            internal (PresetColumnAttributes[], int) valuesToPresetArray(int startingColumnIndex)
            {
                var presetAttribListRef = new List<PresetColumnAttributes>();
                var columnCount = 0;

                var shortIdsRef = new List<string>();
                var columnIndicesRef = new List<List<int>>();
                var expressionsRef = new List<List<string>>();
                var colNamesRef = new List<List<string>>();

                var i = startingColumnIndex - 1;
                foreach (var attribs in Values)
                {
                    i++;

                    LoadPresetColumnAttributes(presetAttribListRef, shortIdsRef, columnIndicesRef, expressionsRef, colNamesRef, attribs, i);
                }

                for (var j = 0; j < presetAttribListRef.Count; j++)
                {
                    presetAttribListRef[j].expressions = expressionsRef[j].ToArray();
                    presetAttribListRef[j].columnNames = colNamesRef[j].ToArray();
                    presetAttribListRef[j].columnIndices = new int[columnIndicesRef[j].Count];
                    columnIndicesRef[j].CopyTo(presetAttribListRef[j].columnIndices);
                    columnCount += columnIndicesRef[j].Count;
                }

                return (presetAttribListRef.ToArray(), columnCount);
            }

            internal int indexOf(string searchedKey)
            {
                var i = 0;
                foreach (var key in Keys)
                {
                    if (key == searchedKey)
                        return i;

                    i++;
                }

                return -1;
            }

            internal List<string> idsToList(bool getUniqueIds, List<string> list = null)
            {
                list = list ?? new List<string>();

                if (getUniqueIds)
                {
                    foreach (var key in Keys)
                        list.Add(key);
                }
                else
                {
                    foreach (var attribs in Values)
                        list.AddUnique(attribs.getShortId());
                }

                return list;
            }

            internal List<string> idsToSortedList(bool getUniqueIds, List<string> list = null)
            {
                list = idsToList(getUniqueIds, list);
                list.Sort();

                return list;
            }

            internal List<string> getAllIdsByShortId(string shortId)
            {
                var longIdsList = new List<string>();

                foreach (var pair in this)
                {
                    if (pair.Value.getShortId() == shortId)
                        longIdsList.AddUnique(pair.Key);
                }

                return longIdsList;
            }

            internal void remove(List<string> longIdsList)
            {
                foreach (var longId in longIdsList)
                    Remove(longId);
            }
        }

        public class PresetColumnAttributesDict : SortedDictionary<string, PresetColumnAttributes> //Column unique ID (string), Preset column settings
        {
            public PresetColumnAttributesDict()
            {
                //Nothing to do...
            }

            public PresetColumnAttributesDict(PresetColumnAttributesDict dict, out bool splitGroupingsExist, bool fullCopy = false)
            {
                splitGroupingsExist = false;

                foreach (var keyValue in dict)
                {
                    Add(keyValue.Key, new PresetColumnAttributes(keyValue.Value, fullCopy));
                    splitGroupingsExist |= !string.IsNullOrWhiteSpace(keyValue.Value.splitter);
                }
            }
        }

        internal static ReportPreset GetCreatePredefinedPreset(Guid presetPermanentGuid, string presetName,
            SortedDictionary<Guid, ReportPreset> existingPredefinedPresets,
            PresetColumnAttributes[] groupings, PresetColumnAttributes[] functions,
            string[] destinationTags, string[] functionIds, bool totals,
            LrReportFormat fileFormatIndex
            )
        {
            if (existingPredefinedPresets.TryGetValue(presetPermanentGuid, out var libraryReportsPreset) && libraryReportsPreset != null)
            {
                libraryReportsPreset.groupings = groupings;
                libraryReportsPreset.functions = functions;
                libraryReportsPreset.totals = totals;
                libraryReportsPreset.name = presetName.ToUpper();
                libraryReportsPreset.fileFormatIndex = fileFormatIndex;

                if (libraryReportsPreset.destinationTags.Length != destinationTags.Length)
                {
                    libraryReportsPreset.destinationTags = destinationTags;
                    libraryReportsPreset.functionIds = functionIds;
                    libraryReportsPreset.conditionField = groupings[0].parameterName;
                    libraryReportsPreset.comparison = Comparison.IsGreaterOrEqual;
                }
            }
            else
            {
                libraryReportsPreset = new ReportPreset()
                {

                    permanentGuid = presetPermanentGuid,
                    groupings = groupings,
                    functions = functions,
                    totals = totals,
                    name = presetName.ToUpper(),
                    userPreset = false,

                    destinationTags = destinationTags,
                    functionIds = functionIds,
                    conditionField = groupings[0].parameterName,
                    comparison = Comparison.IsGreaterOrEqual,

                    exportedTrackListName = ExportedTrackList,
                    fileFormatIndex = fileFormatIndex,
                };
            }

            libraryReportsPreset.sourceTags = new string[functions.Length];
            for (var i = 0; i < functions.Length; i++)
                libraryReportsPreset.sourceTags[i] = GetColumnName(functions[i].parameterName, functions[i].parameter2Name,
                    functions[i].functionType, null, false, null, string.Empty, false, false, true);

            return libraryReportsPreset;
        }

        public struct ReportPresetReference
        {
            public Guid permanentGuid;

            public ReportPresetReference(bool placeholder) //-V3117
            {
                permanentGuid = Guid.Empty;
            }

            public ReportPresetReference(ReportPreset referencePreset)
            {
                permanentGuid = referencePreset.permanentGuid;
            }

            internal ReportPreset findPreset()
            {
                foreach (var preset in PresetsInteractiveWorkingCopy.Values)
                {
                    if (preset.permanentGuid == permanentGuid)
                        return preset;
                }

                return null;
            }

            public override string ToString()
            {
                if (PresetsInteractiveWorkingCopy.TryGetValue(permanentGuid, out var preset))
                    return preset.ToString();
                else
                    return MsgLrReferredPresetNotFound;
            }
        }

        public enum Comparison
        {
            Is = 0,
            IsNot = 1,
            IsGreater = 2,
            IsGreaterOrEqual = 3,
            IsLess = 4,
            IsLessOrEqual = 5
        }

        public class ReportPreset
        {
            public bool autoApply = false;

            public string name;
            public string autoName;
            private bool columnsChecked;

            public bool userPreset = true;
            public Guid guid = Guid.NewGuid(); //guid is reset on every preset update in UI
            public Guid initialGuid; //initial guid is copied from guid on any preset creation, and is reset on every preset update in UI
            public Guid permanentGuid = Guid.NewGuid(); //permanentGuid is reset on preset copying in UI


            public bool hotkeyAssigned = false;
            public bool applyToSelectedTracks = true;
            public int hotkeySlot = -1; //0..MusicBeePlugin.MaximumNumberOfLrHotkeys - 1

            public PresetColumnAttributes[] groupings = Array.Empty<PresetColumnAttributes>();
            public PresetColumnAttributes[] functions = Array.Empty<PresetColumnAttributes>();

            public bool totals;

            public string[] sourceTags = Array.Empty<string>();
            public string[] destinationTags = Array.Empty<string>();
            public string[] functionIds = Array.Empty<string>();

            public int[] operations = Array.Empty<int>();
            public string[] mulDivFactors = Array.Empty<string>();
            public string[] precisionDigits = Array.Empty<string>();
            public string[] appendTexts = Array.Empty<string>();

            public bool useAnotherPresetAsSource = false;
            public ReportPresetReference anotherPresetAsSource = new ReportPresetReference();

            public bool conditionIsChecked;
            public string conditionField;
            public Comparison comparison = Comparison.Is;
            public string comparedField;

            public bool resizeArtwork;
            public int newArtworkSize = 300;

            public LrReportFormat fileFormatIndex = LrReportFormat.HtmlDocument;
            public string exportedTrackListName;

            public ReportPreset()
            {
                initialGuid = guid;

                if (!useAnotherPresetAsSource)
                    anotherPresetAsSource = default;
            }

            public ReportPreset(string exportedTrackList) : this()
            {
                exportedTrackListName = exportedTrackList;
            }

            public ReportPreset(ReportPreset sourcePreset, bool fullCopy) : this()
            {
                if (fullCopy)
                {
                    name = sourcePreset.name;
                    autoApply = sourcePreset.autoApply;
                    userPreset = sourcePreset.userPreset;
                    guid = sourcePreset.guid;
                    initialGuid = guid;
                    permanentGuid = sourcePreset.permanentGuid;
                    hotkeyAssigned = sourcePreset.hotkeyAssigned;
                    hotkeySlot = sourcePreset.hotkeySlot; //0..MusicBeePlugin.MaximumNumberOfLrHotkeys - 1
                    functionIds = sourcePreset.functionIds.Clone() as string[];
                }
                else
                {
                    functionIds = new string[sourcePreset.functionIds.Length];
                }

                autoName = sourcePreset.autoName;

                applyToSelectedTracks = sourcePreset.applyToSelectedTracks;

                groupings = sourcePreset.groupings.Clone() as PresetColumnAttributes[];
                functions = sourcePreset.functions.Clone() as PresetColumnAttributes[];

                totals = sourcePreset.totals;

                sourceTags = sourcePreset.sourceTags.Clone() as string[];
                destinationTags = sourcePreset.destinationTags.Clone() as string[];

                operations = sourcePreset.operations.Clone() as int[];
                mulDivFactors = sourcePreset.mulDivFactors.Clone() as string[];
                precisionDigits = sourcePreset.precisionDigits.Clone() as string[];
                appendTexts = sourcePreset.appendTexts.Clone() as string[];

                useAnotherPresetAsSource = sourcePreset.useAnotherPresetAsSource;
                anotherPresetAsSource = sourcePreset.anotherPresetAsSource;

                conditionIsChecked = sourcePreset.conditionIsChecked;
                conditionField = sourcePreset.conditionField;
                comparison = sourcePreset.comparison;
                comparedField = sourcePreset.comparedField;

                resizeArtwork = sourcePreset.resizeArtwork;
                newArtworkSize = sourcePreset.newArtworkSize;

                fileFormatIndex = sourcePreset.fileFormatIndex;
                exportedTrackListName = sourcePreset.exportedTrackListName;
            }

            private string getHotkeyChar()
            {
                if (!hotkeyAssigned)
                    return string.Empty;
                else if (applyToSelectedTracks)
                    return "";
                else
                    return "";
            }

            private string getHotkeyPostfix()
            {
                var hotkeyChar = getHotkeyChar();

                if (string.IsNullOrEmpty(hotkeyChar))
                    return hotkeyChar;
                else
                    return " " + hotkeyChar;
            }

            internal string getHotkeyDescription()
            {
                return LrPresetHotkeyDescription + ": " + ToString();
            }

            private int getReferredCount()
            {
                int refCount = 0;

                foreach (var preset in PresetsInteractiveWorkingCopy.Values)
                    if (PresetsInteractiveWorkingCopy.TryGetValue(preset.anotherPresetAsSource.permanentGuid, out var next) && next != null
                        && next.guid == guid)

                        refCount++;

                return refCount;
            }

            private string getNextPresetChars(ReportPreset referencePreset, string currentRemark = "")
            {
                int referredCount = getReferredCount();

                if (string.IsNullOrEmpty(currentRemark)) //It's the 1st iteration
                    if (referredCount > 0)
                        for (int i = 0; i < referredCount; i++)
                            currentRemark += "";


                if (referencePreset.useAnotherPresetAsSource && PresetsInteractiveWorkingCopy.TryGetValue(referencePreset.anotherPresetAsSource.permanentGuid, out var nextPreset)
                    && nextPreset != null)

                    return getNextPresetChars(nextPreset, currentRemark + "  ");
                else
                    return currentRemark;
            }

            public override string ToString()
            {
                if (initialGuid == guid)
                    return getName() + getNextPresetChars(this) + getHotkeyPostfix();
                else
                    return getName() + getNextPresetChars(this) + getHotkeyPostfix() + " ⚠";
            }

            internal string getName()
            {
                var grDictRef = new ColumnAttributesDict();

                if (!columnsChecked && prepareDict(grDictRef, null, groupings, 0) == -1)
                {
                    columnsChecked = true;
                    generateAutoName(null, null);
                    return autoName;
                }
                else if (!string.IsNullOrWhiteSpace(name))
                {
                    columnsChecked = true;
                    return name;
                }
                else if (autoName == null)
                {
                    columnsChecked = true;
                    generateAutoName(null, null);
                }

                columnsChecked = true;
                return autoName;
            }

            internal void generateAutoName(ColumnAttributesDict grDictRef, ColumnAttributesDict fnDictRef)
            {
                autoName = string.Empty;

                if (grDictRef == null && fnDictRef == null)
                {
                    grDictRef = new ColumnAttributesDict();
                    fnDictRef = new ColumnAttributesDict();
                    var ColumnCount = prepareDict(grDictRef, null, groupings, 0);
                    if (ColumnCount == -1)
                    {
                        autoName = CtlLrInvalidPresetFormatAutoName;
                        return;
                    }

                    prepareDict(fnDictRef, null, groupings, ColumnCount);
                }

                foreach (var attribs in grDictRef.Values)
                    autoName += (string.IsNullOrEmpty(autoName) ? string.Empty : ", ") + attribs.getColumnName(true, true, true);

                foreach (var attribs in fnDictRef.Values)
                    autoName += (string.IsNullOrEmpty(autoName) ? string.Empty : ", ") + attribs.getColumnName(true, true, true);

                if (string.IsNullOrEmpty(autoName))
                    autoName = EmptyPresetName;
            }
        }

        internal static void LoadPresetColumnAttributes(List<PresetColumnAttributes> presetAttribsListRef, List<string> shortIdsRef, List<List<int>> columnIndicesRef,
            List<List<string>> expressionsRef, List<List<string>> colNamesRef, ColumnAttributes attribs, int columnIndex)
        {
            var shortId = attribs.getShortId();
            var index = shortIdsRef.IndexOf(shortId);
            if (index == -1)
            {
                shortIdsRef.AddUnique(shortId);

                index = shortIdsRef.Count - 1;

                var presetAttribs = new PresetColumnAttributes(attribs);
                presetAttribsListRef.Add(presetAttribs);
                expressionsRef.Add(new List<string>());
                columnIndicesRef.Add(new List<int>());
                colNamesRef.Add(new List<string>());
            }

            expressionsRef[index].Add(attribs.expression);
            columnIndicesRef[index].Add(columnIndex);
            colNamesRef[index].Add(attribs.columnName);
        }

        //Helper for Iterate()
        private static void MoveDependent(IEnumerator<string>[] enumerators, int[] dependentGroupingColumns, string[] lastValues, int currentLoopIndex, bool reset)
        {
            for (var j = dependentGroupingColumns.Length - 1; j >= 0; j--)
            {
                if (currentLoopIndex != j && dependentGroupingColumns[currentLoopIndex] == dependentGroupingColumns[j]) //Let's move all dependent groupings
                {
                    if (reset)
                        enumerators[j].Reset();

                    enumerators[j].MoveNext();
                    lastValues[j] = enumerators[j].Current;
                }
            }
        }

        //Let's iterate through all possible split grouping values (but not repeating the same groupings for different expressions)
        //
        //Accepts IEnumerator<split grouping values>[grouping column count],
        //
        //int[grouping column count] dependentGroupingColumns: indices of groupings
        //    in global dictionary "groupings" (which doesn't include expressions)
        //
        //Returns false when iteration completed, and string[grouping column count] lastValues on every iteration
        private static bool Iterate(IEnumerator<string>[] enumerators, int[] dependentGroupingColumns, string[] lastValues, ref int minimalLoopIndex, ref int currentLoopIndex)
        {
            var movedThis = enumerators[currentLoopIndex].MoveNext();

            if (movedThis)
            {
                lastValues[currentLoopIndex] = enumerators[currentLoopIndex].Current;

                MoveDependent(enumerators, dependentGroupingColumns, lastValues, currentLoopIndex, false);

                return true;
            }
            else if (currentLoopIndex == 0)
            {
                return false;
            }
            else
            {
                var oldCurrentLoopIndex = currentLoopIndex;

                if (currentLoopIndex == minimalLoopIndex + 1)
                {
                    if (enumerators[minimalLoopIndex].MoveNext())
                    {
                        lastValues[minimalLoopIndex] = enumerators[minimalLoopIndex].Current;

                        MoveDependent(enumerators, dependentGroupingColumns, lastValues, minimalLoopIndex, false);
                    }
                    else
                    {
                        enumerators[minimalLoopIndex].Reset();
                        enumerators[minimalLoopIndex].MoveNext();
                        lastValues[minimalLoopIndex] = enumerators[minimalLoopIndex].Current;

                        MoveDependent(enumerators, dependentGroupingColumns, lastValues, minimalLoopIndex, true);

                        minimalLoopIndex--;
                        if (minimalLoopIndex < 0)
                            return false;
                    }

                    currentLoopIndex = enumerators.Length - 1;
                }
                else
                {
                    currentLoopIndex--;
                }

                for (var i = oldCurrentLoopIndex; i < enumerators.Length; i++)
                {
                    enumerators[i].Reset();
                    if (i != currentLoopIndex)
                    {
                        enumerators[i].MoveNext();
                        lastValues[i] = enumerators[i].Current;
                    }

                    MoveDependent(enumerators, dependentGroupingColumns, lastValues, i, true);
                }

                return Iterate(enumerators, dependentGroupingColumns, lastValues, ref minimalLoopIndex, ref currentLoopIndex);
            }
        }

        //SortedDictionary<composed groupings OR OTHER UNIQUE ID, function results>
        private class AggregatedTags : SortedDictionary<string, ConvertStringsResult[]>
        {
            //Accepts List<split grouping values>[grouping column count],
            //    returns List<string -> composed groupings> for every split grouping
            //int[grouping column count] dependentGroupingColumns: indices of groupings
            //    in global dictionary "groupings" (which doesn't include expressions)
            internal static List<string> GetComposedGroupingTags(List<string>[] groupingValuesLists, int[] dependentGroupingColumns, bool totals)
            {
                var composedGroupings = new List<string>();

                if (groupingValuesLists.Length == 0)
                {
                    composedGroupings.Add(string.Empty);
                    return composedGroupings;
                }

                var groupingValues = new string[groupingValuesLists.Length];

                var lastValues = new string[groupingValuesLists.Length]; //This array will store all grouping values of last iteration
                var minimalLoopIndex = groupingValuesLists.Length - 2;
                var currentLoopIndex = groupingValuesLists.Length - 1;

                //Neither of the lists in groupingValuesLists array can be empty, so let's get 1st value of every list
                var enumerators = new IEnumerator<string>[groupingValuesLists.Length];
                for (var i = 0; i < groupingValuesLists.Length; i++)
                {
                    (enumerators[i] = groupingValuesLists[i].GetEnumerator()).MoveNext();
                    lastValues[i] = enumerators[i].Current;
                }

                do
                {
                    for (var i = 0; i < enumerators.Length; i++)
                        groupingValues[i] = lastValues[i];

                    composedGroupings.AddUnique(string.Join(LrGroupingsSplitterId.ToString(), groupingValues));


                    if (totals)
                    {
                        for (var j = groupingValues.Length - 1; j >= 0; j--)
                        {
                            groupingValues[j] = TotalsString;
                            composedGroupings.AddUnique(string.Join(LrGroupingsSplitterId.ToString(), groupingValues));
                        }
                    }
                }
                while (Iterate(enumerators, dependentGroupingColumns, lastValues, ref minimalLoopIndex, ref currentLoopIndex));

                return composedGroupings;
            }

            internal void add(string url, List<string> composedGroupingsList, ConvertStringsResult[] functionResults)
            {
                foreach (var composedGroupings in composedGroupingsList)
                {
                    if (url != null) //Let's deal with URLs (SortedDictionary<URL, false>)
                        foreach (var functionResult in functionResults)
                            functionResult.urls.AddSkip(url);

                    if (ContainsKey(composedGroupings))
                        Remove(composedGroupings);

                    Add(composedGroupings, functionResults);
                }
            }

            internal void add(string url, List<string> composedGroupingsList, ColumnAttributesDict functions,
                string[] functionValues, string[] parameter2Values)
            {
                if (functionValues == null || functionValues.Length == 0)
                {
                    foreach (var composedGroupings in composedGroupingsList)
                    {
                        if (!TryGetValue(composedGroupings, out var aggregatedValues))
                        {
                            aggregatedValues = new ConvertStringsResult[1];
                            aggregatedValues[0] = new ConvertStringsResult(ResultType.UseOtherResults, DataType.String); //This is URLs (SortedDictionary<url, false>)

                            Add(composedGroupings, aggregatedValues);
                        }

                        if (url != null) //Let's deal with URLs (SortedDictionary<url, false>)
                            aggregatedValues[0].urls.AddSkip(url);
                    }
                }
                else
                {
                    foreach (var composedGroupings in composedGroupingsList)
                    {
                        if (!TryGetValue(composedGroupings, out var aggregatedValues))
                        {
                            aggregatedValues = new ConvertStringsResult[functionValues.Length];

                            var j = -1;
                            foreach (var function in functions.Values)
                            {
                                j++;

                                aggregatedValues[j] = new ConvertStringsResult(function.resultType, function.dataType);

                                if (function.functionType == LrFunctionType.Count)
                                {
                                    function.resultType = ResultType.ItemCount;
                                    function.dataType = DataType.Number;
                                }
                                else if (function.functionType == LrFunctionType.AverageCount)
                                {
                                    function.resultType = ResultType.AverageCount;
                                    function.dataType = DataType.Number;
                                }
                                else if (function.functionType == LrFunctionType.Minimum)
                                {
                                    aggregatedValues[j].resultD = double.MaxValue;
                                    aggregatedValues[j].resultS = "口";
                                }
                                else if (function.functionType == LrFunctionType.Maximum)
                                {
                                    aggregatedValues[j].resultD = double.MinValue;
                                    aggregatedValues[j].resultS = string.Empty;
                                }
                            }

                            Add(composedGroupings, aggregatedValues);
                        }

                        var i = -1;
                        foreach (var function in functions.Values)
                        {
                            i++;

                            ConvertStringsResult currentFunctionResult;

                            var currentFunctionValue = function.evaluateExpression(url, functionValues[i]);

                            if (function.functionType == LrFunctionType.Count)
                            {
                                aggregatedValues[i].items.AddSkip(currentFunctionValue);
                                aggregatedValues[i].resultType = ResultType.ItemCount;

                                function.resultType = ResultType.ItemCount;
                                function.dataType = DataType.Number;
                            }
                            else if (function.functionType == LrFunctionType.AverageCount)
                            {
                                aggregatedValues[i].items1.AddSkip(currentFunctionValue);
                                aggregatedValues[i].items.AddSkip(parameter2Values[i]);

                                function.resultType = ResultType.AverageCount;
                                function.dataType = DataType.Number;
                            }
                            else if (function.functionType == LrFunctionType.Average)
                            {
                                aggregatedValues[i].items.AddSkip(parameter2Values[i]);
                                currentFunctionResult = ConvertStrings(currentFunctionValue, function.resultType, function.dataType);

                                if (currentFunctionResult.resultType < function.resultType)
                                    currentFunctionResult.resultType = function.resultType;

                                if (currentFunctionResult.resultType < ResultType.UnknownOrString)
                                {
                                    aggregatedValues[i].resultD += currentFunctionResult.resultD;

                                    if (aggregatedValues[i].resultDPrefix == null)
                                        aggregatedValues[i].resultDPrefix = currentFunctionResult.resultDPrefix;
                                    else if (aggregatedValues[i].resultDPrefix != currentFunctionResult.resultDPrefix)
                                        aggregatedValues[i].resultDPrefix = string.Empty;

                                    if (aggregatedValues[i].resultDSpace == null)
                                        aggregatedValues[i].resultDSpace = currentFunctionResult.resultDSpace;
                                    else if (aggregatedValues[i].resultDSpace != currentFunctionResult.resultDSpace)
                                        aggregatedValues[i].resultDSpace = string.Empty;

                                    if (aggregatedValues[i].resultDPostfix == null)
                                        aggregatedValues[i].resultDPostfix = currentFunctionResult.resultDPostfix;
                                    else if (aggregatedValues[i].resultDPostfix != currentFunctionResult.resultDPostfix)
                                        aggregatedValues[i].resultDPostfix = string.Empty;

                                    aggregatedValues[i].resultType = currentFunctionResult.resultType;
                                }
                                else
                                {
                                    aggregatedValues[i].resultD = currentFunctionResult.resultD;
                                    aggregatedValues[i].resultS = currentFunctionResult.resultS;
                                }


                                if (currentFunctionResult.resultType > function.resultType)
                                {
                                    function.resultType = currentFunctionResult.resultType;
                                    aggregatedValues[i].resultType = currentFunctionResult.resultType;
                                }
                            }
                            else if (function.functionType == LrFunctionType.Sum)
                            {
                                currentFunctionResult = ConvertStrings(currentFunctionValue, function.resultType, function.dataType);

                                if (currentFunctionResult.resultType < function.resultType)
                                    currentFunctionResult.resultType = function.resultType;

                                if (currentFunctionResult.resultType < ResultType.UnknownOrString)
                                {
                                    aggregatedValues[i].resultD += currentFunctionResult.resultD;

                                    if (aggregatedValues[i].resultDPrefix == null)
                                        aggregatedValues[i].resultDPrefix = currentFunctionResult.resultDPrefix;
                                    else if (aggregatedValues[i].resultDPrefix != currentFunctionResult.resultDPrefix)
                                        aggregatedValues[i].resultDPrefix = string.Empty;

                                    if (aggregatedValues[i].resultDSpace == null)
                                        aggregatedValues[i].resultDSpace = currentFunctionResult.resultDSpace;
                                    else if (aggregatedValues[i].resultDSpace != currentFunctionResult.resultDSpace)
                                        aggregatedValues[i].resultDSpace = string.Empty;

                                    if (aggregatedValues[i].resultDPostfix == null)
                                        aggregatedValues[i].resultDPostfix = currentFunctionResult.resultDPostfix;
                                    else if (aggregatedValues[i].resultDPostfix != currentFunctionResult.resultDPostfix)
                                        aggregatedValues[i].resultDPostfix = string.Empty;
                                }
                                else
                                {
                                    aggregatedValues[i].resultD = currentFunctionResult.resultD;
                                    aggregatedValues[i].resultS = currentFunctionResult.resultS;
                                }


                                if (currentFunctionResult.resultType > function.resultType)
                                {
                                    function.resultType = currentFunctionResult.resultType;
                                    aggregatedValues[i].resultType = currentFunctionResult.resultType;
                                }
                            }
                            else if (function.functionType == LrFunctionType.Minimum)
                            {
                                currentFunctionResult = ConvertStrings(currentFunctionValue, function.resultType, function.dataType);

                                if (currentFunctionResult.resultType < function.resultType)
                                    currentFunctionResult.resultType = function.resultType;


                                if (aggregatedValues[i].resultD > currentFunctionResult.resultD)
                                    aggregatedValues[i].resultD = currentFunctionResult.resultD;

                                if (CompareStrings(aggregatedValues[i].resultS, currentFunctionResult.resultS, ResultType.UnknownOrString) == 1)
                                    aggregatedValues[i].resultS = currentFunctionResult.resultS;


                                if (currentFunctionResult.resultType == ResultType.Double || currentFunctionResult.resultType == ResultType.AutoDouble)
                                {
                                    if (aggregatedValues[i].resultDPrefix == null)
                                        aggregatedValues[i].resultDPrefix = currentFunctionResult.resultDPrefix;
                                    else if (aggregatedValues[i].resultDPrefix != currentFunctionResult.resultDPrefix)
                                        aggregatedValues[i].resultDPrefix = string.Empty;

                                    if (aggregatedValues[i].resultDSpace == null)
                                        aggregatedValues[i].resultDSpace = currentFunctionResult.resultDSpace;
                                    else if (aggregatedValues[i].resultDSpace != currentFunctionResult.resultDSpace)
                                        aggregatedValues[i].resultDSpace = string.Empty;

                                    if (aggregatedValues[i].resultDPostfix == null)
                                        aggregatedValues[i].resultDPostfix = currentFunctionResult.resultDPostfix;
                                    else if (aggregatedValues[i].resultDPostfix != currentFunctionResult.resultDPostfix)
                                        aggregatedValues[i].resultDPostfix = string.Empty;
                                }


                                if (currentFunctionResult.resultType > function.resultType)
                                {
                                    function.resultType = currentFunctionResult.resultType;
                                    aggregatedValues[i].resultType = currentFunctionResult.resultType;
                                }
                            }
                            else if (function.functionType == LrFunctionType.Maximum)
                            {
                                currentFunctionResult = ConvertStrings(currentFunctionValue, function.resultType, function.dataType);

                                if (currentFunctionResult.resultType < function.resultType)
                                    currentFunctionResult.resultType = function.resultType;


                                if (aggregatedValues[i].resultD < currentFunctionResult.resultD)
                                    aggregatedValues[i].resultD = currentFunctionResult.resultD;

                                if (CompareStrings(aggregatedValues[i].resultS, currentFunctionResult.resultS, ResultType.UnknownOrString) == -1)
                                    aggregatedValues[i].resultS = currentFunctionResult.resultS;


                                if (currentFunctionResult.resultType == ResultType.Double || currentFunctionResult.resultType == ResultType.AutoDouble)
                                {
                                    if (aggregatedValues[i].resultDPrefix == null)
                                        aggregatedValues[i].resultDPrefix = currentFunctionResult.resultDPrefix;
                                    else if (aggregatedValues[i].resultDPrefix != currentFunctionResult.resultDPrefix)
                                        aggregatedValues[i].resultDPrefix = string.Empty;

                                    if (aggregatedValues[i].resultDSpace == null)
                                        aggregatedValues[i].resultDSpace = currentFunctionResult.resultDSpace;
                                    else if (aggregatedValues[i].resultDSpace != currentFunctionResult.resultDSpace)
                                        aggregatedValues[i].resultDSpace = string.Empty;

                                    if (aggregatedValues[i].resultDPostfix == null)
                                        aggregatedValues[i].resultDPostfix = currentFunctionResult.resultDPostfix;
                                    else if (aggregatedValues[i].resultDPostfix != currentFunctionResult.resultDPostfix)
                                        aggregatedValues[i].resultDPostfix = string.Empty;
                                }


                                if (currentFunctionResult.resultType > function.resultType)
                                {
                                    function.resultType = currentFunctionResult.resultType;
                                    aggregatedValues[i].resultType = currentFunctionResult.resultType;
                                }
                            }


                            if (url != null) //Let's deal with URLs (SortedDictionary<url, false>)
                                aggregatedValues[i].urls.AddSkip(url);
                        }
                    }
                }
            }

            internal static int CompareField(string composedGroupings, ConvertStringsResult[] convertResults, int fieldNumber, ColumnAttributesDict groupings, string comparedValue)
            {
                if (fieldNumber < groupings.Count)
                {
                    var field = composedGroupings.Split(LrGroupingsSplitterId)[fieldNumber];

                    if (field == TotalsString)
                        field = (CtlAllTags + " '" + groupings.ElementAt(fieldNumber).Value.getColumnName(true, true, true) + "'").ToUpper();

                    return field.CompareTo(comparedValue);
                }
                else
                {
                    var currentFunctionResult = convertResults[fieldNumber - groupings.Count];
                    var comparedFunctionValue = ConvertStrings(comparedValue, ResultType.UseOtherResults, DataType.String);

                    return currentFunctionResult.compare(comparedFunctionValue);
                }
            }

            internal static string GetField(string composedGroupings, ConvertStringsResult[] convertResults, int fieldNumber, ColumnAttributesDict groupings,
                int operation, string mulDivFactorRepr, string precisionDigitsRepr, string appendedText, int artworkField = -1)
            {
                if (fieldNumber < groupings.Count)
                {
                    var field = composedGroupings.Split(LrGroupingsSplitterId)[fieldNumber];

                    if (field == TotalsString && fieldNumber != artworkField)
                        field = (CtlAllTags + " '" + groupings.ElementAt(fieldNumber).Value.getColumnName(true, true, true) + "'").ToUpper();
                    else if (field == TotalsString && fieldNumber == artworkField)
                        field = "%%ALL-ARTWORKS%%";

                    return field;
                }
                else
                {
                    return convertResults[fieldNumber - groupings.Count].getFormattedResult(operation, mulDivFactorRepr, precisionDigitsRepr, appendedText);
                }
            }

            internal static object[] GetGroupings(KeyValuePair<string, ConvertStringsResult[]> keyValue, ColumnAttributesDict groupings, int artworkField = -1)
            {
                if (string.IsNullOrEmpty(keyValue.Key))
                {
                    return Array.Empty<object>();
                }
                else
                {
                    var fields = keyValue.Key.Split(LrGroupingsSplitterId);

                    for (var i = 0; i < fields.Length; i++)
                    {
                        if (fields[i] == TotalsString && i != artworkField)
                            fields[i] = (CtlAllTags + " '" + groupings.ElementAt(i).Value.getColumnName(true, true, true) + "'").ToUpper();
                        else if (fields[i] == TotalsString && i == artworkField)
                            fields[i] = "%%ALL-ARTWORKS%%";
                    }

                    object[] results = new object[fields.Length];
                    for (int i = 0; i < fields.Length; i++)
                        results[i] = fields[i];

                    return results;
                }
            }

            internal static string[] GetGroupings(string composedGroupings, ColumnAttributesDict groupings, int artworkField)
            {
                if (string.IsNullOrEmpty(composedGroupings))
                {
                    return Array.Empty<string>();
                }
                else
                {
                    var fields = composedGroupings.Split(LrGroupingsSplitterId);

                    for (var i = 0; i < fields.Length; i++)
                    {
                        if (fields[i] == TotalsString && i != artworkField)
                            fields[i] = (CtlAllTags + " '" + groupings.ElementAt(i).Value.getColumnName(true, true, true) + "'").ToUpper();
                        else if (fields[i] == TotalsString && i == artworkField)
                            fields[i] = "%%ALL-ARTWORKS%%";
                    }

                    return fields;
                }
            }
        }

        internal static string GetSplitterRepresentation(string splitter, bool trimValues, bool addSpacePrefix)
        {
            var representation = string.Empty;

            if (!string.IsNullOrEmpty(splitter) && addSpacePrefix)
                representation += " [" + splitter + "]";
            else if (!string.IsNullOrEmpty(splitter))
                representation += "[" + splitter + "]";

            if (!string.IsNullOrEmpty(splitter) && trimValues)
                representation += " *";

            return representation;
        }

        internal static string GetColumnName(string tagName, string tag2Name, LrFunctionType type, string splitter, bool trimValues,
            string expression, string columnName, bool trimName, bool includeSplitterTrimInfo, bool includeExpression)
        {
            if (!string.IsNullOrEmpty(columnName))
                return columnName;


            columnName = tagName;

            if (includeSplitterTrimInfo)
                columnName += GetSplitterRepresentation(splitter, trimValues, true);

            if (type == LrFunctionType.Average || type == LrFunctionType.AverageCount)
                columnName = LrFunctionNames[(int)type] + "(" + columnName + "/" + tag2Name + ")";
            else if (type != LrFunctionType.Grouping)
                columnName = LrFunctionNames[(int)type] + "(" + columnName + ")";

            if (includeExpression && !string.IsNullOrWhiteSpace(expression))
            {
                if (trimName && expression.Length > MaxExprRepresentationLength)
                {
                    var exprRepresentationMiddle = MaxExprRepresentationLength / 2;
                    var exprRepresentationTailStart = expression.Length - exprRepresentationMiddle;

                    columnName += " : " + expression.Substring(0, exprRepresentationMiddle) + "…"
                        + expression.Substring(exprRepresentationTailStart);
                }
                else
                {
                    columnName += " : " + expression;
                }
            }

            if (trimName && columnName.Length > MaxColumnRepresentationLength)
            {
                columnName = columnName.Substring(0, MaxColumnRepresentationLength);
                if (columnName[columnName.Length - 1] != '…')
                    columnName += "…";
            }

            return columnName;
        }


        private (ResultType[], bool[]) getColumnTypesRightAlignment()
        {
            var columnRightAlignments = new bool[groupingsDict.Count + functionsDict.Count];
            var resultTypes = new ResultType[groupingsDict.Count + functionsDict.Count];

            var j = 0;
            foreach (var grouping in groupingsDict.Values)
            {
                if (grouping.parameterName == SequenceNumberName) //It's a number. Let's right-align the column.
                {
                    columnRightAlignments[j] = true;
                    resultTypes[j] = ResultType.Double;
                }
                else if (grouping.parameterName == DateCreatedTagName) //It's a number. Let's right-align the column.
                {
                    columnRightAlignments[j] = true;
                    resultTypes[j] = ResultType.DateTime;
                }
                else
                {
                    resultTypes[j] = grouping.resultType;

                    if (grouping.resultType >= ResultType.AutoDouble && grouping.resultType <= ResultType.ItemCount) //It's either number or date/time/duration. Let's right-align the column.
                        columnRightAlignments[j] = true;
                    else
                        columnRightAlignments[j] = false;
                }

                j++;
            }

            var i = 0;
            foreach (var function in functionsDict.Values)
            {
                resultTypes[j + i] = function.resultType;

                if (function.resultType >= ResultType.AutoDouble && function.resultType <= ResultType.ItemCount) //It's either number or date/time/duration. Let's right-align the column.
                    columnRightAlignments[j + i] = true;
                else
                    columnRightAlignments[j + i] = false;

                i++;
            }

            return (resultTypes, columnRightAlignments);
        }

        //Returns the number of rows, which require formatting changed tags (i.e. number of rows actually added to table), 0 if no formatting is required
        private int previewTable_AddRowsToTable(BindingSource source, int rowsCount, bool itsFirstRowRange, bool itsLastRowRange)
        {
            if (hidePreview)
                return 0;


            if (itsFirstRowRange) //There are no rows in preview table yet, so let's adjust column text alignment according to data type
            {
                if (artworkField == -1)
                    previewTable.RowTemplate.Resizable = DataGridViewTriState.True;
                else
                    previewTable.Columns[artworkField].Resizable = DataGridViewTriState.False;
            }


            return AddRowsToTable(this, previewTable, source, rows.Count, itsLastRowRange, false, true);
        }

        private void previewTableFormatRow(DataGridView dataGridView, int rowIndex)
        {
            for (var j = 0; j < previewTable.ColumnCount; j++)
            {
                string cellValue = rows[rowIndex].Columns[j] as string;

                if (j != artworkField)
                {
                    if (cellValue != null && cellValue.StartsWith(CtlAllTags))
                        previewTable.Rows[rowIndex].Cells[j].Style.Font = totalsFont;

                    previewTable.Rows[rowIndex].Cells[j].ToolTipText = cellValue + "\n\n" + LrCellToolTip;
                }
                else //Artwork column
                {
                    previewTable.Columns[j].MinimumWidth = previewTableArtworkSize;
                    previewTable.Rows[rowIndex].MinimumHeight = previewTableArtworkSize;
                    previewTable.Columns[j].Width = previewTableArtworkSize;
                    previewTable.Rows[rowIndex].Height = previewTableArtworkSize;
                }
            }
        }

        internal void updateLrCacheForRenamedTrack(string oldFilenameNewFilename)
        {
            string[] filenames = oldFilenameNewFilename.Split('\0');
            int newTrackId = GetPersistentTrackIdInt(filenames[1]);

            foreach (var guidTags in CachedPresetsTags)
            {
                if (guidTags.Value.TryGetValue(filenames[0], out var tags))
                {
                    guidTags.Value.Remove(filenames[0]);
                    guidTags.Value.Add(filenames[1], tags);
                }
            }


            lock (FilesUpdatedByPlugin)
            {
                if (FilesUpdatedByPlugin.Contains(filenames[0]))
                {
                    FilesUpdatedByPlugin.Remove(filenames[0]);
                    FilesUpdatedByPlugin.Add(filenames[1]);
                }
            }


            lock (ChangedFiles)
            {
                if (ChangedFiles.Contains(filenames[0]))
                {
                    ChangedFiles.Remove(filenames[0]);
                    ChangedFiles.Add(filenames[1]);
                }
            }


            lock (LrTrackCacheNeededToBeUpdated)
                LrTrackCacheNeededToBeUpdated.ReplaceExisting(newTrackId, filenames[1]);

            lock (LrTrackCacheNeededToBeUpdatedWorkingCopy)
                for (int i = 0; i < LrTrackCacheNeededToBeUpdatedWorkingCopy.Length; i++)
                    if (LrTrackCacheNeededToBeUpdatedWorkingCopy[i] == filenames[0])
                        LrTrackCacheNeededToBeUpdatedWorkingCopy[i] = filenames[1];
        }

        private void clearArtworks()
        {
            lock (ResizedArtworkCache)
            {
                CdBookletArtwork?.Dispose();
                CdBookletArtwork = null;

                CdBookletArtworkBase64 = null;

                foreach (var pair in ResizedArtworkCache)
                    pair.Value?.Dispose();

                ResizedArtworkCache.Clear();
            }
        }

        private void resetLocalsAndUiControls()
        {
            lastCachedAppliedPresetGuid = Guid.NewGuid();


            previewTable.DataSource = null;
            source.DataSource = null;
            source.ResetBindings(true);
            rows = null;

            while (previewTable.ColumnCount > 0)
                previewTable.Columns.RemoveAt(0);

            tagsDataGridView.RowCount = 0;
            tagsDataGridViewSelectedRow = -1;
            expressionsDataGridView.RowCount = 0;


            updateCustomScrollBars(presetList);
            updateCustomScrollBars(previewTable);
            updateCustomScrollBars(tagsDataGridView);
            updateCustomScrollBars(expressionsDataGridView);


            expressionBackup = string.Empty;
            splitterBackup = string.Empty;
            trimValuesBackup = false;
            columnNameBackup = string.Empty;

            conditionField = -1;
            comparedField = -1;

            artworkField = -1;
            clearArtworks();

            sequenceNumberField = -1;

            sourceFieldComboBoxCustom.ItemsClear();
            conditionFieldListCustom.ItemsClear();
            comparedFieldListCustom.ItemsClear();


            sortedShortIds.Clear();
            shortIdsExprs.Clear();
            shortIdsColNames.Clear();

            functionComboBoxCustom.SelectedIndex = -1;
            sourceTagListCustom.SelectedIndex = -1;
            expressionTextBox.Text = string.Empty;
            columnNameTextBox.Text = string.Empty;

            groupings.Clear();
            groupingsDict.Clear();
            functionsDict.Clear();

            operations.Clear();
            mulDivFactors.Clear();
            precisionDigits.Clear();
            appendTexts.Clear();

            functionComboBoxCustom.SelectedIndex = 0;

            if (previewIsGenerated)
                clickOnPreviewButton(prepareBackgroundPreview, previewReport, buttonPreview, buttonOK, buttonClose);
        }

        //Returns column count
        private static int prepareDict(ColumnAttributesDict dictRef, PresetColumnAttributesDict presetDictRef,
            PresetColumnAttributes[] attribsSet, int startingColumnIndex)
        {
            var columnIndex = startingColumnIndex;
            var maxColumnIndex = startingColumnIndex - 1;
            for (var i = 0; i < attribsSet.Length; i++)
                maxColumnIndex += attribsSet[i].expressions.Length;

            if (presetDictRef != null)
                for (var i = 0; i < attribsSet.Length; i++)
                    presetDictRef.AddSkip(attribsSet[i].getShortId(), attribsSet[i]);


                repeat_again:
            if (columnIndex <= maxColumnIndex)
            {
                for (var i = 0; i < attribsSet.Length; i++)
                {
                    for (var j = 0; j < attribsSet[i].expressions.Length; j++)
                    {
                        if (columnIndex == attribsSet[i].columnIndices[j])
                        {
                            var uniqueId = attribsSet[i].getUniqueId(j);
                            if (uniqueId == null)
                                return -1;

                            dictRef.AddSkip(uniqueId,
                                new ColumnAttributes(attribsSet[i].functionType, attribsSet[i].expressions[j], attribsSet[i].columnNames[j], attribsSet[i].parameterName,
                                attribsSet[i].parameter2Name, attribsSet[i].splitter, attribsSet[i].trimValues));

                            columnIndex++;
                            goto repeat_again;
                        }
                    }
                }
            }

            return maxColumnIndex + 1 - startingColumnIndex;
        }

        private void prepareConditionRelatedLocals()
        {
            comparison = appliedPreset.comparison;
            comparedFieldText = appliedPreset.comparedField;

            conditionField = -1;
            comparedField = -1;

            if (appliedPreset.conditionIsChecked)
            {
                var i = -1;
                foreach (var attribs in groupingsDict.Values)
                {
                    i++;

                    if (attribs.getColumnName(false, false, true) == appliedPreset.conditionField)
                        conditionField = i;

                    if (attribs.getColumnName(false, false, true) == comparedFieldText)
                        comparedField = i;
                }


                i = -1;
                foreach (var attribs in functionsDict.Values)
                {
                    i++;

                    if (attribs.getColumnName(false, false, true) == appliedPreset.conditionField)
                        conditionField = groupingsDict.Count + i;

                    if (attribs.getColumnName(false, false, true) == comparedFieldText)
                        comparedField = groupingsDict.Count + i;
                }
            }
        }

        private void setArtworkSequenceNumberFields()
        {
            artworkField = -1;
            sequenceNumberField = -1;

            var i = 0;
            foreach (var attribs in groupingsDict.Values)
            {
                if (attribs.parameterName == ArtworkName)
                    artworkField = i;
                else if (attribs.parameterName == SequenceNumberName)
                    sequenceNumberField = i;

                i++;
            }
        }

        private List<string> prepareRows()
        {
            List<string> columnNames = new List<string>();

            foreach (var attribs in groupingsDict.Values)
                columnNames.Add(GetColumnName(attribs.parameterName, null, LrFunctionType.Grouping, attribs.splitter, attribs.trimValues, attribs.expression, attribs.columnName, true, true, true));

            foreach (var attribs in functionsDict.Values)
                columnNames.Add(GetColumnName(attribs.parameterName, attribs.parameter2Name, attribs.functionType, null, false, attribs.expression, attribs.columnName, true, true, true));

            rows = new DataGridViewBoundColumnList<Row>(columnNames);

            Invoke(new Action(() => { source.DataSource = rows; source.ResetBindings(false); }));

            return columnNames;
        }

        private void prepareLocals()
        {
            if (lastCachedAppliedPresetGuid == appliedPreset.guid)
                return;


            var columnNames = prepareRows();


            groupings.Clear();
            groupingsDict.Clear();
            var columnCount = prepareDict(groupingsDict, groupings, appliedPreset.groupings, 0);

            functionsDict.Clear();
            prepareDict(functionsDict, null, appliedPreset.functions, columnCount);

            savedFunctionIds.Clear();
            savedFunctionIds.AddRange(appliedPreset.functionIds);


            operations.Clear();
            operations.AddRange(appliedPreset.operations);
            mulDivFactors.Clear();
            mulDivFactors.AddRange(appliedPreset.mulDivFactors);
            precisionDigits.Clear();
            precisionDigits.AddRange(appliedPreset.precisionDigits);
            appendTexts.Clear();
            appendTexts.AddRange(appliedPreset.appendTexts);


            destinationTagIds.Clear();
            for (var j = 0; j < appliedPreset.destinationTags.Length; j++)
                destinationTagIds.Add(GetTagId(appliedPreset.destinationTags[j]));

            clearArtworks();
            setArtworkSequenceNumberFields();

            prepareConditionRelatedLocals();
        }

        private void setUnsavedChanges(bool flagUnsavedChanges)
        {
            if (flagUnsavedChanges)
            {
                unsavedChanges = true;
                buttonClose.Image = warningWide;
                toolTip1.SetToolTip(buttonClose, buttonCloseToolTip);
            }
            else //if (!flagUnsavedChanges)
            {
                unsavedChanges = false;
                buttonClose.Image = Resources.transparent_15;
                toolTip1.SetToolTip(buttonClose, string.Empty);
            }

            buttonClose.Refresh();
        }

        private void setPresetChanged()
        {
            if (presetIsLoading)
                return;

            if (selectedPreset == null)
                return;

            assignHotkeyCheckBoxLabel.Text = assignHotkeyCheckBoxText + (MaximumNumberOfLrHotkeys - reportPresetsWithHotkeysCount) + "/" + MaximumNumberOfLrHotkeys;

            updatePreset();
            setUnsavedChanges(true);
        }

        private static class ResizedArtworkProvider
        {
            private static TypeConverter tc;
            private static int newArtworkSize = -1;

            internal static void Init(int artworkField, SortedDictionary<string, Bitmap> artworkCache, int presetNewArtworkSize)
            {
                if (artworkField != -1)
                {
                    newArtworkSize = presetNewArtworkSize;
                    tc = TypeDescriptor.GetConverter(typeof(Bitmap));


                    Bitmap pic1 = SafeCopyBitmap(DefaultArtwork);
                    Bitmap pic2 = pic1;

                    if (newArtworkSize > 0)
                    {
                        float SF;

                        if (pic1.Width >= pic1.Height) //-V3080
                            SF = newArtworkSize / (float)pic1.Width;
                        else
                            SF = newArtworkSize / (float)pic1.Height;

                        pic2 = ScaleBitmap(pic1, (int)Math.Round(pic1.Width * SF), (int)Math.Round(pic1.Height * SF));
                        pic1.Dispose();
                    }

                    ResizedDefaultArtworkBase64 = Convert.ToBase64String((byte[])tc.ConvertTo(pic2, typeof(byte[])));
                    ResizedDefaultArtworkBase64Hash = ResizedDefaultArtworkBase64.GetHashCode().ToString("X8");

                    lock (artworkCache)
                        artworkCache.AddReplace(ResizedDefaultArtworkBase64Hash, pic2);


                    pic1 = SafeCopyBitmap(ArtworkTotals);
                    pic2 = pic1;

                    if (newArtworkSize > 0)
                    {
                        float SF;

                        if (pic1.Width >= pic1.Height)
                            SF = newArtworkSize / (float)pic1.Width;
                        else
                            SF = newArtworkSize / (float)pic1.Height;

                        pic2 = ScaleBitmap(pic1, (int)Math.Round(pic1.Width * SF), (int)Math.Round(pic1.Height * SF));
                        pic1.Dispose();
                    }

                    lock (artworkCache)
                        artworkCache.AddReplace(ResizedArtworkTotalsId, pic2);
                }
            }

            internal static Bitmap DecodeResizeArtwork(string artworkBase64, SortedDictionary<string, Bitmap> artworkCache)
            {
                if (artworkCache.TryGetValue(artworkBase64.GetHashCode().ToString("X8"), out Bitmap pic))
                    return pic;


                pic = DecodeArtwork(artworkBase64);

                if (newArtworkSize > 0)
                {
                    float SF;

                    if (pic.Width >= pic.Height) //-V3080
                        SF = newArtworkSize / (float)pic.Width;
                    else
                        SF = newArtworkSize / (float)pic.Height;


                    try
                    {
                        var bm_dest = new Bitmap((int)Math.Round(pic.Width * SF), (int)Math.Round(pic.Height * SF), System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                        var gr_dest = Graphics.FromImage(bm_dest);
                        gr_dest.DrawImage(pic, 0, 0, bm_dest.Width, bm_dest.Height);
                        pic.Dispose();
                        gr_dest.Dispose();

                        pic = bm_dest;
                    }
                    catch
                    {
                        pic = new Bitmap(1, 1);
                    }
                }


                artworkCache.Add(artworkBase64.GetHashCode().ToString("X8"), pic);

                return pic;
            }

            internal static Bitmap DecodeArtwork(string artworkBase64)
            {
                Bitmap pic;

                try
                {
                    if (!string.IsNullOrEmpty(artworkBase64))
                        pic = tc.ConvertFrom(Convert.FromBase64String(artworkBase64)) as Bitmap;
                    else
                        pic = ResizedArtworkCache[ResizedDefaultArtworkBase64Hash];
                }
                catch
                {
                    pic = ResizedArtworkCache[ResizedDefaultArtworkBase64Hash];
                }


                return pic;
            }
        }

        internal static string ConvertSequenceNumberToString(int i)
        {
            var oldSequenceNumber = i.ToString("D9");
            var sequenceNumber = string.Empty;

            var j = 0;
            for (j = 0; j < 8; j++)
            {
                if (oldSequenceNumber[j] == '0')
                    sequenceNumber += '\u2007';
                else
                    break;
            }

            while (j < 9)
            {
                sequenceNumber += oldSequenceNumber[j];
                j++;
            }

            return sequenceNumber;
        }

        private void updateAppliedPresetCache(AggregatedTags tags, SortedDictionary<int, string[]> filesGroupingTags, SortedDictionary<int, string[]> filesGroupingTagsRaw,
            SortedDictionary<int, List<string>> filesActualComposedGroupingTags)
        {
            lock (CachedPresetsTags)
            {
                lastCachedAppliedPresetGuid = appliedPreset.guid;
                cachedAppliedPresetGuids.AddSkip(appliedPreset.guid);
                CachedPresetsTags.AddReplace(appliedPreset.guid, tags);
                CachedPresetsFilesActualGroupingTags.AddReplace(appliedPreset.guid, filesGroupingTags);
                CachedPresetsFilesActualGroupingTagsRaw.AddReplace(appliedPreset.guid, filesGroupingTagsRaw);
                CachedPresetsFilesActualComposedSplitGroupingTagsList.AddReplace(appliedPreset.guid, filesActualComposedGroupingTags);
            }
        }

        private void clearAppliedPresetCache()
        {
            lock (CachedPresetsTags)
            {
                lastCachedAppliedPresetGuid = Guid.NewGuid();
                cachedAppliedPresetGuids.RemoveExisting(appliedPreset.guid);
                CachedPresetsTags.RemoveExisting(appliedPreset.guid);
                CachedPresetsFilesActualGroupingTags.RemoveExisting(appliedPreset.guid);
                CachedPresetsFilesActualGroupingTagsRaw.RemoveExisting(appliedPreset.guid);
                CachedPresetsFilesActualComposedSplitGroupingTagsList.RemoveExisting(appliedPreset.guid);
            }
        }

        private string getTagValue(string file, MetaDataType tagId, FilePropertyType propId, bool getRawValues)
        {
            string tagValue;

            if (tagId == (MetaDataType)(-99)) //Sequence number
            {
                tagValue = "xXxXxXxXx"; //Let's reserve space for future numbers
            }
            else if (tagId == 0 && propId == 0)
            {
                tagValue = string.Empty; //Will ignore this grouping...
            }
            else if (tagId == ArtistArtistsId || tagId == ComposerComposersId) //Let's make smart conversion of list of artists/composers
            {
                tagValue = GetFileTag(file, tagId);
                if (smartOperation && !getRawValues)
                    tagValue = GetTagRepresentation(tagValue);
                else if (!getRawValues)
                    tagValue = RemoveRoleIds(tagValue);
            }
            else if (tagId == MetaDataType.Artwork) //It's artwork image. Let's fill cell with base64 codes. 
            {
                tagValue = GetFileTag(file, MetaDataType.Artwork);

                if (CdBookletArtworkBase64 == null && string.IsNullOrEmpty(tagValue))
                {
                    CdBookletArtworkBase64 = ResizedDefaultArtworkBase64;
                    CdBookletArtwork = ResizedArtworkCache[ResizedDefaultArtworkBase64Hash];
                }
                else if (CdBookletArtworkBase64 == null)
                {
                    CdBookletArtworkBase64 = tagValue;
                    CdBookletArtwork = ResizedArtworkProvider.DecodeArtwork(tagValue); //NOT resized common artwork for CD booklet export
                }
                else if (CdBookletArtworkBase64 != tagValue)
                {
                    CdBookletArtworkBase64 = ResizedDefaultArtworkBase64;
                    CdBookletArtwork = ResizedArtworkCache[ResizedDefaultArtworkBase64Hash];
                }
            }
            else if (propId != 0)
            {
                tagValue = MbApiInterface.Library_GetFileProperty(file, propId);
            }
            else
            {
                tagValue = GetFileTag(file, tagId, true);
            }

            return tagValue;
        }

        internal int getCachedFunctionResultIndex(string functionId)
        {
            var idIndex = appliedPreset.functionIds.IndexOfFirst(functionId);

            if (idIndex >= 0 && !IsTagEmpty(appliedPreset.destinationTags[idIndex]))
                return idIndex;
            else
                return -1;
        }

        internal string executePreset(PluginWindowTemplate form, string[] queriedFiles, bool interactive, bool saveResultsToTags, string functionId,
            bool filterResults, bool forceCacheUpdate, //filterResults:
                                                       //  false - proceed as usual (filter results by this preset condition only (if defined) on tag saving only for
                                                       //  any "interactive")
                                                       //  true & !interactive - update lastFiles by filtered file list AND by this preset condition (if defined)
                                                       //  true & interactive - filter queriedFiles list by this preset condition only (if defined)
                                                       //
                                                       //   "true & !interactive" filterResults MUST BE USED ONLY FOR FILTERING PRESET CHAIN INVOKED INSIDE THIS FUNCTION
                                                       //
                                                       //
                                                       //forceCacheUpdate: false - use cache if available, true - force cache update

            SortedDictionary<string, bool>[] queriedGroupingTagsRaw = null,             //If readOtherwiseProcessExcludedGroupingTags == null then just use dictionaries
            SortedDictionary<string, bool>[] queriedActualGroupingTags = null,          //as local variables. If readOtherwiseProcessExcludedGroupingTags == true,
            SortedDictionary<string, bool>[] queriedActualGroupingTagsRaw = null,       //then return tags, which will be changed soon 
            bool? readOtherwiseProcessExcludedGroupingTags = null)                      //If dictionaries are not nulls and it's cache update operation, then process
                                                                                        //all tracks with the same grouping tags as in dictionaries (because track
                                                                                        //set with these tags has been changed) 
        {
            if (queriedFiles != null && queriedFiles.Length == 0)
            {
                clearAppliedPresetCache();

                if (interactive)
                    Invoke(new Action(() => { MessageBox.Show(MbForm, SbLrEmptyTrackListToBeApplied, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error); }));

                return SbLrEmptyTrackListToBeApplied;
            }

            if (functionId != null && string.IsNullOrWhiteSpace(functionId))
            {
                clearAppliedPresetCache();
                return SbIncorrectLrFunctionId + "\"" + functionId + "\"";
            }

            var queryEntireLibrary = false;



            string returnValue = null;

            lock (SavedSettings.reportPresets)
            {
                if (functionId != null && queriedFiles == null)
                {
                    clearAppliedPresetCache();
                    returnValue = SbLrNot1TrackPassedToLrFunctionId;
                }
                else if (functionId != null && queriedFiles.Length != 1) //queriedFiles != null if functionId != null. Checked above. //-V3125 //-V3095
                {
                    clearAppliedPresetCache();
                    returnValue = SbLrNot1TrackPassedToLrFunctionId;
                }

                if (returnValue == null)
                {
                    if (queriedFiles == null && readOtherwiseProcessExcludedGroupingTags != false) //Not recalculating preset based on cached tags
                    {
                        queryEntireLibrary = true;

                        if (!MbApiInterface.Library_QueryFilesEx("domain=Library", out queriedFiles))
                            return string.Empty;

                        if (queriedFiles.Length == 0)
                            return string.Empty;
                    }

                    appliedPreset = new ReportPreset(appliedPreset, true);
                }
            }

            if (returnValue != null)
                return returnValue;


            //LET'S DEAL WITH ANOTHER PRESET AS A SOURCE
            if (appliedPreset.useAnotherPresetAsSource)
            {
                var anotherPresetAsSource = appliedPreset.anotherPresetAsSource.findPreset();
                if (anotherPresetAsSource?.conditionIsChecked != true)
                    return "!!!";


                var appliedPresetInUse = appliedPreset;
                appliedPreset = anotherPresetAsSource;
                executePreset(form, queriedFiles, false, false, functionId, true, forceCacheUpdate);
                queriedFiles = lastFiles;
                appliedPreset = appliedPresetInUse;
            }


            //MAIN PROCESSING
            PresetColumnAttributesDict groupings;
            ColumnAttributesDict functionsDict;
            bool splitGroupingsExist;

            lock (SavedSettings.reportPresets)
            {
                prepareLocals();

                groupings = new PresetColumnAttributesDict(this.groupings, out splitGroupingsExist, true);
                functionsDict = new ColumnAttributesDict(this.functionsDict, true);
            }


            if (functionId != null && functionsDict.Count == 0)
            {
                clearAppliedPresetCache();
                return SbIncorrectLrFunctionId + functionId + "!";
            }

            var queryOnlyGroupings = functionsDict.Count == 0;


            if (splitGroupingsExist && !queryOnlyGroupings && saveResultsToTags) //It's senseless to save spit groupings of one file to this file
            {
                clearAppliedPresetCache();

                if (interactive)
                    MessageBox.Show(MbForm, SbLrSenselessToSaveSpitGroupingsTo1File, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return SbLrSenselessToSaveSpitGroupingsTo1File;
            }


            //int[] dependentGroupingColumns will be used several time below, let's cache it
            //
            //int[grouping column count] dependentGroupingColumns: indices of groupings
            //    in global dictionary "groupings" (which doesn't include expressions),
            //    but -1 for groupings having only one column 
            var dependentGroupingColumns = new int[groupings.Count];
            var p = -1;
            foreach (var grouping in groupings.Values)
            {
                p++;

                for (var l = 0; l < grouping.columnIndices.Length; l++)
                    dependentGroupingColumns[grouping.columnIndices[l]] = p;
            }


            var presetGroupingTagsAreCached = true;


            //Let's cache tag/prop ids
            MetaDataType[] queriedActualGroupingsTagIds;
            FilePropertyType[] queriedActualGroupingsPropIds;
            string[] queriedNativeTagNames;

            if (cachedAppliedPresetGuids.ContainsKey(appliedPreset.guid))
            {
                queriedActualGroupingsTagIds = CachedPresetsActualGroupingsTagIds[appliedPreset.guid];
                queriedActualGroupingsPropIds = CachedPresetsActualGroupingsPropIds[appliedPreset.guid];
                queriedNativeTagNames = CachedPresetsNativeTagNames[appliedPreset.guid];
            }
            else
            {
                presetGroupingTagsAreCached = false;

                var queriedGroupingsTagIds = new MetaDataType[groupings.Count];
                for (var l = 0; l < queriedGroupingsTagIds.Length; l++)
                    queriedGroupingsTagIds[l] = 0;

                var queriedGroupingsPropIds = new FilePropertyType[groupings.Count];
                for (var l = 0; l < queriedGroupingsPropIds.Length; l++)
                    queriedGroupingsPropIds[l] = 0;


                queriedActualGroupingsTagIds = new MetaDataType[groupings.Count];
                for (var l = 0; l < queriedActualGroupingsTagIds.Length; l++)
                    queriedActualGroupingsTagIds[l] = 0;

                queriedActualGroupingsPropIds = new FilePropertyType[groupings.Count];
                for (var l = 0; l < queriedActualGroupingsTagIds.Length; l++)
                    queriedActualGroupingsTagIds[l] = 0;

                queriedNativeTagNames = new string[groupings.Count];


                var i = -1;
                foreach (var attribs in groupings.Values)
                {
                    i++;

                    var tagId = GetTagId(attribs.parameterName);
                    var propId = GetPropId(attribs.parameterName);
                    string nativeTagName;

                    if (attribs.parameterName == SequenceNumberName)
                    {
                        nativeTagName = null;
                        tagId = (MetaDataType)(-99);
                        propId = 0;
                        queriedActualGroupingsTagIds[i] = tagId;
                    }
                    else
                    {
                        if (tagId == 0)
                        {
                            queriedActualGroupingsPropIds[i] = propId;
                            nativeTagName = MbApiInterface.Setting_GetFieldName((MetaDataType)propId);
                        }
                        else
                        {
                            queriedActualGroupingsTagIds[i] = tagId;
                            nativeTagName = MbApiInterface.Setting_GetFieldName(tagId);
                        }
                    }

                    if (string.IsNullOrWhiteSpace(nativeTagName))
                        nativeTagName = null;
                    else if (tagId == MetaDataType.Artwork)
                        nativeTagName = null;


                    queriedNativeTagNames[i] = nativeTagName;


                    //MusicBee doesn't support these tags for querying, so let's skip them in query
                    if (tagId != MetaDataType.AlbumArtistRaw && nativeTagName != null && nativeTagName != ArtworkName)
                    {
                        if (tagId == 0)
                            queriedGroupingsPropIds[i] = propId;
                        else
                            queriedGroupingsTagIds[i] = tagId;
                    }
                }

                if (cachedAppliedPresetGuids.Count > PresetCacheCountCriticalThreshold)
                    periodicCacheClearing(null);

                CachedPresetsGroupingsTagIds.AddReplace(appliedPreset.guid, queriedGroupingsTagIds);
                CachedPresetsGroupingsPropIds.AddReplace(appliedPreset.guid, queriedGroupingsPropIds);
                CachedPresetsActualGroupingsTagIds.AddReplace(appliedPreset.guid, queriedActualGroupingsTagIds);
                CachedPresetsActualGroupingsPropIds.AddReplace(appliedPreset.guid, queriedActualGroupingsPropIds);
                CachedPresetsNativeTagNames.AddReplace(appliedPreset.guid, queriedNativeTagNames);
            }


            AggregatedTags tags;
            SortedDictionary<int, string[]> cachedFilesActualGroupingTags;
            SortedDictionary<int, string[]> cachedFilesActualGroupingTagsRaw;
            SortedDictionary<int, List<string>> cachedFilesActualComposedSplitGroupingTagsList;

            lock (CachedPresetsTags)
            {
                if (!CachedPresetsTags.TryGetValue(appliedPreset.guid, out tags))
                {
                    tags = new AggregatedTags();
                    CachedPresetsTags.Add(appliedPreset.guid, tags);
                }


                if (!CachedPresetsFilesActualGroupingTags.TryGetValue(appliedPreset.guid, out cachedFilesActualGroupingTags))
                {
                    cachedFilesActualGroupingTags = new SortedDictionary<int, string[]>(); //<URLs, Grouping tag[]s>
                    CachedPresetsFilesActualGroupingTags.Add(appliedPreset.guid, cachedFilesActualGroupingTags);
                }

                if (!CachedPresetsFilesActualGroupingTagsRaw.TryGetValue(appliedPreset.guid, out cachedFilesActualGroupingTagsRaw))
                {
                    cachedFilesActualGroupingTagsRaw = new SortedDictionary<int, string[]>(); //<URLs, Grouping tag[]s>
                    CachedPresetsFilesActualGroupingTagsRaw.Add(appliedPreset.guid, cachedFilesActualGroupingTagsRaw);
                }

                //cachedFilesActualComposedSplitGroupingTagsList: <TrackId, List of <composed groupings>>
                if (!CachedPresetsFilesActualComposedSplitGroupingTagsList.TryGetValue(appliedPreset.guid, out cachedFilesActualComposedSplitGroupingTagsList))
                {
                    cachedFilesActualComposedSplitGroupingTagsList = new SortedDictionary<int, List<string>>(); //<URL, List of <composed groupings>>
                    CachedPresetsFilesActualComposedSplitGroupingTagsList.Add(appliedPreset.guid, cachedFilesActualComposedSplitGroupingTagsList);
                }


                if (forceCacheUpdate && readOtherwiseProcessExcludedGroupingTags != false)
                {
                    for (int k = 0; k < queriedFiles.Length; k++)
                    {
                        int trackId = GetPersistentTrackIdInt(queriedFiles[k]);

                        cachedFilesActualGroupingTags.RemoveExisting(trackId);
                        cachedFilesActualGroupingTagsRaw.RemoveExisting(trackId);
                        cachedFilesActualComposedSplitGroupingTagsList.RemoveExisting(trackId);
                    }
                }
            }


            if (!queryOnlyGroupings && queriedGroupingTagsRaw == null) //Use dictionaries as local variables (not reading tags, and not recalculating changed tags)
            {
                PrepareGroupingTagDictionaries(
                            appliedPreset,
                            ref queriedGroupingTagsRaw,
                            ref queriedActualGroupingTags,
                            ref queriedActualGroupingTagsRaw
                            );
            }


            //Let's add default artwork
            if (functionId == null)
            {
                clearArtworks();
                ResizedArtworkProvider.Init(artworkField, ResizedArtworkCache, appliedPreset.resizeArtwork ? appliedPreset.newArtworkSize : -1);
            }


            var actualSplitGroupingTagsList = new List<string>[groupings.Count]; //array (size of grouping count) of list of split grouping tags
            for (var f = 0; f < actualSplitGroupingTagsList.Length; f++)
                actualSplitGroupingTagsList[f] = new List<string>();


            var queriedFilesDict = new SortedDictionary<string, bool>();

            if (readOtherwiseProcessExcludedGroupingTags != false) //queriedFiles == null if readOtherwiseProcessExcludedGroupingTags == false
            {
                foreach (var file in queriedFiles)
                    queriedFilesDict.AddSkip(file);


                //Not recalculating preset based on cached grouping tags
                if (!processFileGroupings(form, queriedFilesDict, interactive, queryOnlyGroupings, groupings, appliedPreset.totals,
                        queriedActualGroupingsTagIds, queriedActualGroupingsPropIds,
                        actualSplitGroupingTagsList, cachedFilesActualComposedSplitGroupingTagsList,
                        cachedFilesActualGroupingTags, cachedFilesActualGroupingTagsRaw,
                        dependentGroupingColumns, queriedGroupingTagsRaw,
                        queriedActualGroupingTags, queriedActualGroupingTagsRaw,
                        queriedNativeTagNames))
                {

                    queriedGroupingTagsRaw = null; //In case if it's reading changing tags operation and these dictionaries must be returned
                    queriedActualGroupingTags = null;
                    queriedActualGroupingTagsRaw = null;

                    return string.Empty;
                }
                else if (readOtherwiseProcessExcludedGroupingTags == true) //Tags changing. Let's clear cache of aggregated tags to be changed soon
                {
                    for (int k = 0; k < queriedFiles.Length; k++)
                        tags.RemoveExisting(queriedFiles[k]);
                }
            }


            if (queryOnlyGroupings)
            {
                updateAppliedPresetCache(tags, cachedFilesActualGroupingTags, cachedFilesActualGroupingTagsRaw, cachedFilesActualComposedSplitGroupingTagsList);
                applyOnlyGroupingsPresetResults(form, queriedFiles, cachedFilesActualComposedSplitGroupingTagsList, interactive, filterResults);

                if (readOtherwiseProcessExcludedGroupingTags == true) //Nothing to recalculate after tag changes
                {
                    queriedGroupingTagsRaw = null; //In case if it's reading changing tags operation and these dictionaries must be returned
                    queriedActualGroupingTags = null;
                    queriedActualGroupingTagsRaw = null;
                }

                return "...";
            }
            //Let's return tags, which may not be relevant soon (either will be changed or track will be removed)
            //Grouping tags are returned through queriedGroupingTagsRaw, queriedActualGroupingTags and
            //queriedActualGroupingTagsRaw
            else if (readOtherwiseProcessExcludedGroupingTags == true)
            {
                return null;
            }


            string[] potentiallyAffectedFiles;

            if (queryEntireLibrary && readOtherwiseProcessExcludedGroupingTags != false) //Not recalculating preset based on cached grouping tags
            {
                potentiallyAffectedFiles = queriedFiles;
            }
            //If recalculating preset based on cached grouping tags, then queriedGroupingTagsRaw, queriedActualGroupingTags & queriedActualGroupingTagsRaw
            //are passed as parameters
            else
            {
                var query = @"<SmartPlaylist><Source Type=""1""><Conditions CombineMethod=""All"">";

                var i = -1;
                foreach (var attribs in groupings.Values)
                {
                    i++;

                    var nativeTagName = queriedNativeTagNames[i];

                    if (nativeTagName != null)
                    {
                        query += @"<Condition Field=""" + nativeTagName + @""" Comparison=""IsIn""";

                        var n = 1;
                        foreach (var queriedTagRaw in queriedGroupingTagsRaw[i])
                            query += @" Value" + (n++) + @"=""" + queriedTagRaw.Key.Replace("\"", "&quot;") + @"""";

                        query += @" />";
                    }
                }

                query += "</Conditions></Source></SmartPlaylist>";


                if (!MbApiInterface.Library_QueryFilesEx(query, out potentiallyAffectedFiles))
                {
                    updateAppliedPresetCache(tags, cachedFilesActualGroupingTags, cachedFilesActualGroupingTagsRaw, cachedFilesActualComposedSplitGroupingTagsList);
                    return string.Empty;
                }

                if (potentiallyAffectedFiles.Length == 0)
                {
                    updateAppliedPresetCache(tags, cachedFilesActualGroupingTags, cachedFilesActualGroupingTagsRaw, cachedFilesActualComposedSplitGroupingTagsList);
                    return string.Empty;
                }

                //Recalculating preset based on cached grouping tags passed as parameters
                //via queriedGroupingTagsRaw, queriedActualGroupingTags & queriedActualGroupingTagsRaw
                if (readOtherwiseProcessExcludedGroupingTags == false)
                    queriedFiles = potentiallyAffectedFiles;
            }


            var cachedTagsAreRelevant = false;

            if (presetGroupingTagsAreCached && !forceCacheUpdate)
            {
                cachedTagsAreRelevant = true;

                for (var i = 0; i < potentiallyAffectedFiles.Length; i++)
                {
                    var trackId = GetPersistentTrackIdInt(potentiallyAffectedFiles[i]);

                    if (cachedFilesActualComposedSplitGroupingTagsList.TryGetValue(trackId, out var fileActualComposedGroupingTagsList))
                    {
                        foreach (var fileActualComposedGroupingTags in fileActualComposedGroupingTagsList)
                        {
                            if (!tags.ContainsKey(fileActualComposedGroupingTags))
                            {
                                cachedTagsAreRelevant = false;
                                goto loop_exit;
                            }
                        }
                    }
                    else
                    {
                        cachedTagsAreRelevant = false;
                        goto loop_exit;
                    }
                }
            }

        loop_exit:
            if (cachedTagsAreRelevant)
            {
                updateAppliedPresetCache(tags, cachedFilesActualGroupingTags, cachedFilesActualGroupingTagsRaw, cachedFilesActualComposedSplitGroupingTagsList);
                return applyPresetResults(form, queriedFiles, tags, cachedFilesActualComposedSplitGroupingTagsList, interactive, saveResultsToTags,
                    functionId, filterResults);
            }


            var newFilesDict = new SortedDictionary<string, bool>();
            foreach (var file in potentiallyAffectedFiles)
                if (!queriedFilesDict.ContainsKey(file))
                    newFilesDict.AddSkip(file);


            if (!processFileGroupings(form, newFilesDict, interactive, false, groupings, appliedPreset.totals,
                    queriedActualGroupingsTagIds, queriedActualGroupingsPropIds,
                    actualSplitGroupingTagsList, cachedFilesActualComposedSplitGroupingTagsList,
                    cachedFilesActualGroupingTags, cachedFilesActualGroupingTagsRaw,
                    dependentGroupingColumns, queriedGroupingTagsRaw,
                    queriedActualGroupingTags, queriedActualGroupingTagsRaw,
                    queriedNativeTagNames))

                return string.Empty;


            tags.Clear(); //Let's reset aggregated function results


            var functionTags = new string[functionsDict.Count];
            var parameter2Tags = new string[functionsDict.Count];

            var affectedFilesDict = new SortedDictionary<string, bool>();

            string[] affectedFiles = null;
            if (queryEntireLibrary)
                affectedFiles = potentiallyAffectedFiles;


            for (var n = 0; n < potentiallyAffectedFiles.Length; n++)
            {
                //Skip if current file grouping tags are not contained in actualGroupingTagsRaw, i.e. queried file selection is
                //excessive due to unsupported (and skipped in query) tags
                var skipFile = false;

                if (checkStoppingStatus())
                {
                    clearAppliedPresetCache();

                    if (interactive)
                    {
                        Invoke(new Action(() =>
                        {
                            if (backgroundTaskIsUpdatingTags)
                                stopButtonClickedMethod(applyingChangesStopped);
                            else
                                stopButtonClickedMethod(prepareBackgroundPreview);
                        }));
                    }
                    else
                    {
                        SetStatusBarText(form, null, true);
                        backgroundTaskIsStoppedOrCancelled = true;
                    }

                    return string.Empty;
                }


                if (interactive)
                    SetStatusBarTextForFileOperations(form, LibraryReportsSbText, true, n, potentiallyAffectedFiles.Length, appliedPreset.getName());
                else if (functionId == null && readOtherwiseProcessExcludedGroupingTags == null)
                    SetStatusBarTextForFileOperations(form, ApplyingLrPresetSbText, true, n, potentiallyAffectedFiles.Length, appliedPreset.getName());


                var currentFile = potentiallyAffectedFiles[n];
                var trackId = GetPersistentTrackIdInt(currentFile);

                if (!queryEntireLibrary)
                {
                    if (cachedFilesActualGroupingTagsRaw.TryGetValue(trackId, out var actualGroupingTagsRaw))
                    {
                        for (var i = 0; i < groupings.Values.Count; i++)
                        {
                            if (!queriedActualGroupingTagsRaw[i].ContainsKey(actualGroupingTagsRaw[i]))
                            {
                                skipFile = true; //Current file hasn't been actually queried
                                break; //Break grouping loop
                            }
                        }
                    }
                    else
                    {
                        for (var i = 0; i < groupings.Values.Count; i++)
                        {
                            var tagId = queriedActualGroupingsTagIds[i];
                            var propId = queriedActualGroupingsPropIds[i];

                            if (!queriedActualGroupingTagsRaw[i].ContainsKey(getTagValue(currentFile, tagId, propId, true)))
                            {
                                skipFile = true; //Current file hasn't been actually queried
                                break; //Break grouping loop
                            }
                        }
                    }
                }

                if (skipFile)
                    continue; //Continue file loop because current file hasn't been actually queried


                if (!queryEntireLibrary) //Not entire library queried, and file is not skipped
                    affectedFilesDict.AddSkip(currentFile);


                var j = -1;
                foreach (var attribs in functionsDict.Values)
                {
                    j++;

                    var tagId = GetTagId(attribs.parameterName);
                    var propId = GetPropId(attribs.parameterName);

                    functionTags[j] = getTagValue(currentFile, tagId, propId, false);

                    if (attribs.functionType == LrFunctionType.Average || attribs.functionType == LrFunctionType.AverageCount)
                    {
                        tagId = GetTagId(attribs.parameter2Name);
                        propId = GetPropId(attribs.parameter2Name);

                        parameter2Tags[j] = getTagValue(currentFile, tagId, propId, false);
                    }
                    else
                    {
                        parameter2Tags[j] = null;
                    }
                }


                var composedActualSplitGroupingTagsList = cachedFilesActualComposedSplitGroupingTagsList[trackId];
                tags.add(currentFile, composedActualSplitGroupingTagsList, functionsDict, functionTags, parameter2Tags);
            }


            if (!queryEntireLibrary)
            {
                affectedFiles = new string[affectedFilesDict.Count];
                var l = 0;
                foreach (var file in affectedFilesDict.Keys)
                    affectedFiles[l++] = file;
            }

            updateAppliedPresetCache(tags, cachedFilesActualGroupingTags, cachedFilesActualGroupingTagsRaw, cachedFilesActualComposedSplitGroupingTagsList);

            if (forceCacheUpdate) //cacheUsage: true - force cache update, false - use cache if available
                return applyPresetResults(form, affectedFiles, tags, cachedFilesActualComposedSplitGroupingTagsList, interactive, true, string.Empty, filterResults);
            else
                return applyPresetResults(form, affectedFiles, tags, cachedFilesActualComposedSplitGroupingTagsList, interactive, saveResultsToTags, functionId, filterResults);
        }

        private bool processFileGroupings(PluginWindowTemplate form, SortedDictionary<string, bool> queriedFilesDict, bool interactive, bool queryOnlyGroupings, PresetColumnAttributesDict groupings,
            bool totals, MetaDataType[] queriedActualGroupingsTagIds, FilePropertyType[] queriedActualGroupingsPropIds,
            List<string>[] actualSplitGroupingTagsList, SortedDictionary<int, List<string>> cachedFilesActualComposedSplitGroupingTagsList,
            SortedDictionary<int, string[]> cachedFilesActualGroupingTags, SortedDictionary<int, string[]> cachedFilesActualGroupingTagsRaw,
            int[] dependentGroupingColumns, SortedDictionary<string, bool>[] queriedGroupingTagsRaw,
            SortedDictionary<string, bool>[] queriedActualGroupingTags, SortedDictionary<string, bool>[] queriedActualGroupingTagsRaw,
            string[] queriedNativeTagNames)
        {
            List<string> composedActualSplitGroupingTagsList = null;
            int lastSeqNumInOrder = 1;

            var n = -1;
            foreach (var currentFile in queriedFilesDict.Keys)
            {
                n++;

                if (checkStoppingStatus())
                {
                    clearAppliedPresetCache();

                    if (interactive)
                    {
                        Invoke(new Action(() =>
                        {
                            if (backgroundTaskIsUpdatingTags)
                                stopButtonClickedMethod(applyingChangesStopped);
                            else
                                stopButtonClickedMethod(prepareBackgroundPreview);
                        }));
                    }
                    else
                    {
                        SetStatusBarText(form, null, true);
                        backgroundTaskIsStoppedOrCancelled = true;
                    }

                    return false;
                }


                var trackId = GetPersistentTrackIdInt(currentFile);

                //sequenceNumberField == -1 is because of sequence numbers can be different every time
                if (sequenceNumberField == -1 && cachedFilesActualGroupingTags.TryGetValue(trackId, out var actualGroupingTags)
                    && cachedFilesActualGroupingTagsRaw.TryGetValue(trackId, out var actualGroupingTagsRaw)
                    && cachedFilesActualComposedSplitGroupingTagsList.TryGetValue(trackId, out composedActualSplitGroupingTagsList))
                {
                    var h = -1;
                    foreach (var attribs in groupings.Values)
                    {
                        h++;

                        var tagValue = actualGroupingTags[h];
                        var tagValueRaw = actualGroupingTagsRaw[h];


                        //Let's remember actual (not raw) grouping tags for future reuse
                        queriedActualGroupingTags[h].AddSkip(tagValue);
                        queriedActualGroupingTagsRaw[h].AddSkip(tagValueRaw);

                        //Let's remember only grouping (raw) tags, which can be used in query in Library_QueryFilesEx function
                        //MusicBee doesn't support for querying some tags, so let's skip them in query
                        if (queriedNativeTagNames[h] != null)
                            queriedGroupingTagsRaw[h].AddSkip(tagValueRaw);


                        if (attribs.parameterName == ArtworkName)
                            ResizedArtworkProvider.DecodeResizeArtwork(tagValue, ResizedArtworkCache);
                    }
                }
                else
                {
                    actualGroupingTags = null;
                    actualGroupingTagsRaw = null;

                    for (var f = 0; f < actualSplitGroupingTagsList.Length; f++)
                        actualSplitGroupingTagsList[f].Clear();

                    if (!queryOnlyGroupings)
                    {
                        actualGroupingTags = new string[groupings.Count];
                        actualGroupingTagsRaw = new string[groupings.Count];
                    }


                    var h = -1;
                    foreach (var attribs in groupings.Values)
                    {
                        h++;

                        var tagId = queriedActualGroupingsTagIds[h];
                        var propId = queriedActualGroupingsPropIds[h];

                        var tagValue = getTagValue(currentFile, tagId, propId, false); // getTagValue() returns "xXxXxXxXx" for sequenceNumber grouping. See code below.

                        if (!queryOnlyGroupings)
                        {
                            var tagValueRaw = getTagValue(currentFile, tagId, propId, true);
                            actualGroupingTags[h] = tagValue;
                            actualGroupingTagsRaw[h] = tagValueRaw;


                            //Let's remember actual grouping tags for future reuse
                            queriedActualGroupingTags[h].AddSkip(tagValue);
                            queriedActualGroupingTagsRaw[h].AddSkip(tagValueRaw);

                            //Let's remember only grouping taga, which can be used in query in Library_QueryFilesEx function
                            //MusicBee doesn't support for querying some tags, so let's skip them in query
                            if (queriedNativeTagNames[h] != null)
                                queriedGroupingTagsRaw[h].AddSkip(tagValueRaw);
                        }


                        if (attribs.parameterName == ArtworkName)
                        {
                            ResizedArtworkProvider.DecodeResizeArtwork(tagValue, ResizedArtworkCache);
                            actualSplitGroupingTagsList[h].Add(tagValue.GetHashCode().ToString("X8"));
                        }
                        else
                        {
                            var columnIndicesTagValues = attribs.getSplitValues(currentFile, tagValue);
                            foreach (var columnIndexTag in columnIndicesTagValues)
                                actualSplitGroupingTagsList[columnIndexTag.index].Add(columnIndexTag.value);
                        }
                    }


                    if (sequenceNumberField != -1)
                    {
                        for (int p = 0; p < actualSplitGroupingTagsList[sequenceNumberField].Count; p++)
                            actualSplitGroupingTagsList[sequenceNumberField][p] = ConvertSequenceNumberToString(lastSeqNumInOrder++);
                    }

                    composedActualSplitGroupingTagsList = AggregatedTags.GetComposedGroupingTags(actualSplitGroupingTagsList, dependentGroupingColumns, totals);

                    //cachedFilesActualComposedSplitGroupingTagsList: <TrackId, List of <composed groupings>>
                    cachedFilesActualComposedSplitGroupingTagsList.AddReplace(trackId, composedActualSplitGroupingTagsList);


                    if (!queryOnlyGroupings)
                    {
                        cachedFilesActualGroupingTags.AddReplace(trackId, actualGroupingTags);
                        cachedFilesActualGroupingTagsRaw.AddReplace(trackId, actualGroupingTagsRaw);
                    }
                }
            }

            return true;
        }


        private string applyPresetResults(PluginWindowTemplate form, string[] queriedFiles, AggregatedTags tags, SortedDictionary<int, List<string>> filesActualComposedSplitGroupingTagsLists,
            bool interactive, bool saveResultsToTags, string functionId, bool filterResults)
        //filterResults:
        //  false & interactive & queriedFiles == null - clear filtering results by this preset condition
        //  false - proceed as usual (filter results by this preset condition only (if defined) on tag saving only for any "interactive")
        //  true & !interactive - update lastFiles by filtered file list AND by this preset condition (if defined)
        //  true & interactive - filter queriedFiles list by this preset condition only (if defined)
        {
            if (!interactive && filterResults) //Filter queriedFiles by another preset (lastFiles) AND by this preset condition
                queriedFiles = lastFiles;
            else if (queriedFiles == null) //It can be only interactive filtering by condition/undoing interactive filtering by condition
                queriedFiles = lastFiles;
            else //Skip or undo filtering by another preset, AND MAYBE filter queriedFiles list by CONDITION only
                lastFiles = queriedFiles.Clone() as string[];


            if (!string.IsNullOrEmpty(functionId)) //Get function id result; cache update is not forced
                return getFunctionResult(queriedFiles[0], functionId, tags, filesActualComposedSplitGroupingTagsLists);


            var showRow = true;

            SortedDictionary<string, bool> filteredFiles = null;
            SortedDictionary<string, object[]> composedSplitGroupingTagsReportRows = null;

            if (!interactive && filterResults) //Only for filtering by another preset (lastFiles which are now queriedFiles) AND by this preset condition
            {
                filteredFiles = new SortedDictionary<string, bool>();
            }
            else if (interactive)
            {
                Invoke(new Action(() =>
                {
                    previewTable.ColumnCount = 0;
                    previewTable.DataSource = source;
                    formatTrackColumns();
                }));

                composedSplitGroupingTagsReportRows = new SortedDictionary<string, object[]>();
            }

            if (interactive || filterResults)
            {
                for (var i = 0; i < queriedFiles.Length; i++)
                {
                    if (checkStoppingStatus())
                    {
                        clearAppliedPresetCache();

                        if (interactive)
                        {
                            Invoke(new Action(() =>
                            {
                                if (backgroundTaskIsUpdatingTags)
                                    stopButtonClickedMethod(applyingChangesStopped);
                                else
                                    stopButtonClickedMethod(prepareBackgroundPreview);
                            }));
                        }
                        else
                        {
                            SetStatusBarText(form, null, true);
                            backgroundTaskIsStoppedOrCancelled = true;
                        }

                        return string.Empty;
                    }


                    var trackId = GetPersistentTrackIdInt(queriedFiles[i]);

                    var composedSplitGroupingTagsList = filesActualComposedSplitGroupingTagsLists[trackId];
                    foreach (var composedSplitGroupingTags in composedSplitGroupingTagsList)
                    {
                        var functionResults = tags[composedSplitGroupingTags];

                        if (filterResults)
                            showRow = checkCondition(composedSplitGroupingTags, functionResults);

                        if (showRow && !interactive && filterResults)
                        {
                            filteredFiles.AddSkip(queriedFiles[i]);
                            break; //Let's continue with the next file
                        }
                        else if (showRow && interactive && !composedSplitGroupingTagsReportRows.ContainsKey(composedSplitGroupingTags))
                        {
                            var groupingsRow = AggregatedTags.GetGroupings(composedSplitGroupingTags, groupingsDict, artworkField);
                            var row = new Row();

                            row.Columns.AddRange(groupingsRow);

                            for (var j = 0; j < functionResults.Length; j++)
                            {
                                var functionResult = AggregatedTags.GetField(null, functionResults, groupingsDict.Count + j, groupingsDict, //-V3080
                                        operations[j], mulDivFactors[j], precisionDigits[j], appendTexts[j], artworkField);

                                row.Columns.Add(functionResult);
                            }


                            object[] rowArray = null;

                            if (artworkField != -1)
                            {
                                Bitmap pic;

                                lock (ResizedArtworkCache)
                                {
                                    if (groupingsRow[artworkField] == TotalsString) //-V3054
                                        pic = ResizedArtworkCache[ResizedArtworkTotalsId];
                                    else //if (!artworks.TryGetValue(artworkBase64, out pic))
                                        pic = ResizedArtworkCache[groupingsRow[artworkField]];
                                }

                                row.Columns[artworkField] = pic;
                                row.artworkBase64Hash = groupingsRow[artworkField];

                                rowArray = new object[row.Columns.Count + 1];
                                row.Columns.CopyTo(rowArray);
                                rowArray[row.Columns.Count] = row.artworkBase64Hash;
                            }
                            else
                            {
                                rowArray = new object[row.Columns.Count];
                                row.Columns.CopyTo(rowArray);
                            }


                            composedSplitGroupingTagsReportRows.Add(composedSplitGroupingTags, rowArray);
                        }
                    }
                }


                if (!interactive && filterResults) //Only for filtering by another preset (lastFiles which are now queriedFiles) AND by this preset condition
                {
                    lastFiles = new string[filteredFiles.Count];
                    var l = 0;
                    foreach (var file in filteredFiles.Keys)
                        lastFiles[l++] = file;
                }
                else if (interactive)
                {
                    int groupingIndex = 0;
                    int groupingsCount = composedSplitGroupingTagsReportRows.Count;

                    bool itsFirstRowRange = true;
                    foreach (var splitGroupingsRows in composedSplitGroupingTagsReportRows)
                    {
                        if (checkStoppingStatus())
                        {
                            clearAppliedPresetCache();

                            Invoke(new Action(() =>
                            {
                                if (backgroundTaskIsUpdatingTags)
                                    stopButtonClickedMethod(applyingChangesStopped);
                                else
                                    stopButtonClickedMethod(prepareBackgroundPreview);
                            }));

                            return string.Empty;
                        }


                        if (artworkField != -1)
                        {
                            object[] newRow = new object[splitGroupingsRows.Value.Length - 1];
                            for (int l = 0; l < splitGroupingsRows.Value.Length - 2; l++)
                                newRow[l] = splitGroupingsRows.Value[l];

                            rows.AddRow(newRow);
                            rows[rows.Count - 1].artworkBase64Hash = splitGroupingsRows.Value[splitGroupingsRows.Value.Length - 1].ToString();
                        }
                        else
                        {
                            rows.AddRow(splitGroupingsRows.Value);
                        }


                        //Saving preset results, see check for functionId above
                        SetStatusBarTextForFileOperations(form, LibraryReportsGeneratingPreviewSbText, true, groupingIndex++, groupingsCount,
                            appliedPreset.getName(), 0);

                        int rowCountToFormat1 = 0;
                        Invoke(new Action(() => { rowCountToFormat1 = previewTable_AddRowsToTable(source, rows.Count, itsFirstRowRange, false); itsFirstRowRange = false; }));
                        Invoke(new Action(() => { FormatChangedTags(this, previewTable, rowCountToFormat1, false, previewTableFormatRow); }));
                    }

                    int rowCountToFormat2 = 0;
                    Invoke(new Action(() => { rowCountToFormat2 = previewTable_AddRowsToTable(source, rows.Count, itsFirstRowRange, true); }));
                    Invoke(new Action(() => { FormatChangedTags(this, previewTable, rowCountToFormat2, true, previewTableFormatRow); checkStoppedStatus(); resetFormToGeneratedPreview(); }));

                    //Saving preset results, see check for functionId above
                    SetResultingSbText(form, appliedPreset.getName(), true, true);
                }
            }


            if (saveResultsToTags && functionId == null) //Saving preset results
            {
                SetResultingSbText(form, appliedPreset.getName(), true, true);
                saveFields(form, queriedFiles, interactive, false, tags);
            }
            else if (saveResultsToTags) //functionId == "", forcing LR functions cache update, see check for functionId above
            {
                saveFields(form, queriedFiles, interactive, true, tags);
            }
            else //Previewing report results
            {
                SetResultingSbText(form, appliedPreset.getName(), true, true);
            }


            return "...";
        }

        private void applyOnlyGroupingsPresetResults(PluginWindowTemplate form, string[] queriedFiles,
            SortedDictionary<int, List<string>> filesActualComposedSplitGroupingTagsLists,
            bool interactive, bool filterResults)
        //filterResults:
        //  false & interactive - Skip or undo filtering by this preset CONDITION only (if defined)
        //  true & !interactive - filter queriedFiles by another preset (lastFiles) AND by this preset condition (if defined)
        //  true & interactive - filter queriedFiles list by CONDITION only (if defined)
        //
        //  FALSE & !INTERACTIVE OF filterResults IS PROHIBITED (because only groupings never saved to tags, so it's senseless)!
        {
            if (!interactive && filterResults) //Filter queriedFiles by another preset (lastFiles) AND by this preset condition (if defined)
                queriedFiles = lastFiles;
            else //if (interactive) //Skip or undo filtering by another preset, AND filter by this preset condition (if defined)
                lastFiles = queriedFiles.Clone() as string[]; //queriedFiles can't be null for applyOnlyGroupingsPresetResults()


            if (!interactive && filterResults) //Only for filtering by another preset (lastFiles which are now queriedFiles) AND by this preset condition
            {
                var filteredFiles = new List<string>();

                for (var i = 0; i < queriedFiles.Length; i++)
                {
                    if (checkStoppingStatus())
                    {
                        clearAppliedPresetCache();
                        SetStatusBarText(form, null, true);
                        backgroundTaskIsStoppedOrCancelled = true;
                    }


                    var trackId = GetPersistentTrackIdInt(queriedFiles[i]);

                    var composedActualGroupingTagsList = filesActualComposedSplitGroupingTagsLists[trackId];
                    foreach (var composedGroupingTags in composedActualGroupingTagsList)
                    {
                        if (checkCondition(composedGroupingTags, null)) //-V3080
                        {
                            filteredFiles.AddUnique(queriedFiles[i]);
                            break; //Let's continue with the next file
                        }
                    }
                }

                lastFiles = new string[filteredFiles.Count];
                filteredFiles.CopyTo(lastFiles);
            }
            else //if (interactive) //!interactive & !filterResults is prohibited (because only groupings never saved to tags, so it's senseless)
            {
                Invoke(new Action(() =>
                {
                    previewTable.DataSource = source;
                    formatTrackColumns();
                }));

                SortedDictionary<string, object[]> composedSplitGroupingTagsReportRows = new SortedDictionary<string, object[]>();


                for (var i = 0; i < queriedFiles.Length; i++)
                {
                    if (checkStoppingStatus())
                    {
                        clearAppliedPresetCache();

                        Invoke(new Action(() =>
                        {
                            if (backgroundTaskIsUpdatingTags)
                                stopButtonClickedMethod(applyingChangesStopped);
                            else
                                stopButtonClickedMethod(prepareBackgroundPreview);
                        }));

                        return;
                    }


                    var trackId = GetPersistentTrackIdInt(queriedFiles[i]);

                    var composedSplitGroupingTagsList = filesActualComposedSplitGroupingTagsLists[trackId];
                    foreach (var composedSplitGroupingTags in composedSplitGroupingTagsList)
                    {
                        var showRow = true;


                        if (filterResults)
                            showRow = checkCondition(composedSplitGroupingTags, null); //-V3080

                        if (showRow && !composedSplitGroupingTagsReportRows.ContainsKey(composedSplitGroupingTags))
                        {
                            string[] groupingsRow = AggregatedTags.GetGroupings(composedSplitGroupingTags, groupingsDict, artworkField);
                            Row row = new Row();

                            row.Columns.AddRange(groupingsRow);


                            object[] rowArray = null;

                            if (artworkField != -1)
                            {
                                Bitmap pic;

                                lock (ResizedArtworkCache)
                                {
                                    if (string.IsNullOrEmpty(groupingsRow[artworkField]))
                                        groupingsRow[artworkField] = ResizedDefaultArtworkBase64;

                                    if (groupingsRow[artworkField] == TotalsString) //-V3054
                                        pic = ResizedArtworkCache[ResizedArtworkTotalsId];
                                    else //if (!artworks.TryGetValue(artworkBase64, out pic))
                                        pic = ResizedArtworkCache[groupingsRow[artworkField]];
                                }

                                row.Columns[artworkField] = pic;
                                row.artworkBase64Hash = groupingsRow[artworkField];

                                rowArray = new object[row.Columns.Count + 1];
                                row.Columns.CopyTo(rowArray);
                                rowArray[row.Columns.Count] = row.artworkBase64Hash;
                            }
                            else
                            {
                                rowArray = new object[row.Columns.Count];
                                row.Columns.CopyTo(rowArray);
                            }


                            composedSplitGroupingTagsReportRows.Add(composedSplitGroupingTags, rowArray);
                        }
                    }
                }


                int groupingIndex = 0;
                int groupingsCount = composedSplitGroupingTagsReportRows.Count;

                bool itsFirstRowRange = true;
                foreach (var splitGroupingsRows in composedSplitGroupingTagsReportRows)
                {
                    if (checkStoppingStatus())
                    {
                        clearAppliedPresetCache();

                        Invoke(new Action(() =>
                        {
                            if (backgroundTaskIsUpdatingTags)
                                stopButtonClickedMethod(applyingChangesStopped);
                            else
                                stopButtonClickedMethod(prepareBackgroundPreview);
                        }));

                        return;
                    }


                    if (artworkField != -1)
                    {
                        object[] newRow = new object[splitGroupingsRows.Value.Length - 1];
                        for (int l = 0; l < splitGroupingsRows.Value.Length - 1; l++)
                            newRow[l] = splitGroupingsRows.Value[l];

                        rows.AddRow(newRow);
                        rows[rows.Count - 1].artworkBase64Hash = splitGroupingsRows.Value[splitGroupingsRows.Value.Length - 1].ToString();
                    }
                    else
                    {
                        rows.AddRow(splitGroupingsRows.Value);
                    }


                    SetStatusBarTextForFileOperations(form, LibraryReportsGeneratingPreviewSbText, true, groupingIndex++, groupingsCount,
                        appliedPreset.getName(), 0);

                    int rowCountToFormat1 = 0;
                    Invoke(new Action(() => { rowCountToFormat1 = previewTable_AddRowsToTable(source, rows.Count, itsFirstRowRange, false); itsFirstRowRange = false; }));
                    Invoke(new Action(() => { FormatChangedTags(this, previewTable, rowCountToFormat1, false, previewTableFormatRow); }));
                }

                int rowCountToFormat2 = 0;
                Invoke(new Action(() => { rowCountToFormat2 = previewTable_AddRowsToTable(source, rows.Count, itsFirstRowRange, true); }));
                Invoke(new Action(() =>
                {
                    FormatChangedTags(this, previewTable, rowCountToFormat2, true, previewTableFormatRow);
                    checkStoppedStatus(); resetFormToGeneratedPreview();
                }));

                SetResultingSbText(form, appliedPreset.getName(), true, true);
            }
        }

        private bool checkCondition(int comparisonResult)
        {
            if (conditionField == -1)
                return true;


            if (comparison == Comparison.Is)
            {
                if (comparisonResult == 0)
                    return true;
                else
                    return false;
            }
            else if (comparison == Comparison.IsNot)
            {
                if (comparisonResult != 0)
                    return true;
                else
                    return false;
            }
            else if (comparison == Comparison.IsGreater)
            {
                if (comparisonResult == 1)
                    return true;
                else
                    return false;
            }
            else if (comparison == Comparison.IsGreaterOrEqual)
            {
                if (comparisonResult >= 0)
                    return true;
                else
                    return false;
            }
            else if (comparison == Comparison.IsLess)
            {
                if (comparisonResult == -1)
                    return true;
                else
                    return false;
            }
            else if (comparison == Comparison.IsLessOrEqual)
            {
                if (comparisonResult <= 0)
                    return true;
                else
                    return false;
            }

            return true;
        }

        private bool checkCondition(string composedGroupings, ConvertStringsResult[] convertResults)
        {
            if (conditionField == -1)
                return true;

            string comparedValue;


            if (comparedField == -1)
                comparedValue = comparedFieldText;
            else
                comparedValue = AggregatedTags.GetField(composedGroupings, convertResults, comparedField, groupingsDict,
                    0, null, null, null);

            return checkCondition(AggregatedTags.CompareField(composedGroupings, convertResults, conditionField, groupingsDict, comparedValue));
        }

        private bool checkCondition(object[] row)
        {
            if (conditionField == -1)
                return true;

            string comparedValue;


            if (comparedField == -1)
                comparedValue = comparedFieldText;
            else
                comparedValue = row[comparedField] as string;

            return checkCondition((row[conditionField] as string).CompareTo(comparedValue));
        }

        private string getAllLibrariesFunctionResultsInternal(string cachedResult, string newResult)
        {
            if (LrCurrentLibraryPathHash == null) //Path base64 is not yet generated since MB startup
                LrCurrentLibraryPathHash = GetStringHash(GetCurrentLibraryPath());

            var appliedPresetGuid = appliedPreset.guid.ToString();
            var presetResultPostfix = LrCachedFunctionResultPresetSeparator + appliedPresetGuid + LrCachedFunctionResultLibraryPathHashSeparator;
            var resultPostfix = LrCachedFunctionResultPresetSeparator + LrCurrentLibraryPathHash + presetResultPostfix;

            if (!cachedResult.Contains(LrCachedFunctionResultLibraryPathHashSeparator)
                || !cachedResult.Contains(LrCachedFunctionResultPresetSeparator)) //Function result hasn't been cached

                return newResult + resultPostfix;


            var librariesResults = cachedResult.Split(new[] { LrCachedFunctionResultLibraryPathHashSeparator }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < librariesResults.Length; i++)
            {
                var libraryResultParts = librariesResults[i].Split(new[] { LrCachedFunctionResultPresetSeparator }, StringSplitOptions.RemoveEmptyEntries);

                if (!AllLrPresetGuidsInUse.ContainsKey(Guid.Parse(libraryResultParts[2])))
                    librariesResults[i] = null;
                else if (libraryResultParts[1] == LrCurrentLibraryPathHash && libraryResultParts[2] == appliedPresetGuid)
                    librariesResults[i] = newResult + resultPostfix;
            }

            var newFullResult = string.Empty;
            for (var i = 0; i < librariesResults.Length; i++)
            {
                if (librariesResults[i] != null)
                    newFullResult += librariesResults[i];
            }

            return newFullResult;
        }

        private string getCachedFunctionResultInternal(string cachedResult)
        {
            var appliedPresetGuid = appliedPreset.guid.ToString();
            var presetResultPostfix = LrCachedFunctionResultPresetSeparator + appliedPresetGuid + LrCachedFunctionResultLibraryPathHashSeparator;
            var resultPostfix = LrCachedFunctionResultPresetSeparator + LrCurrentLibraryPathHash + presetResultPostfix;

            if (!cachedResult.Contains(resultPostfix)) //appliedPreset hasn't been cached
                return null;


            var librariesResults = cachedResult.Split(new[] { LrCachedFunctionResultLibraryPathHashSeparator }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < librariesResults.Length; i++)
            {
                var libraryResultParts = librariesResults[i].Split(new[] { LrCachedFunctionResultPresetSeparator }, StringSplitOptions.RemoveEmptyEntries);

                if (libraryResultParts[1] == LrCurrentLibraryPathHash && libraryResultParts[2] == appliedPresetGuid)
                    return libraryResultParts[0];
            }


            return null; //This must never happen
        }

        private string getCachedFunctionResult(string queriedFile, string functionId)
        {
            if (functionId == null)
                return null;

            MetaDataType destinationTagId = 0;
            var idIndex = getCachedFunctionResultIndex(functionId);

            if (idIndex >= 0)
                destinationTagId = GetTagId(appliedPreset.destinationTags[idIndex]);

            if (destinationTagId == 0)
                return null;


            //if (queriedFile.EndsWith(".asx")) //---
            //    return null;

            var cachedResult = GetFileTag(queriedFile, destinationTagId);

            if (LrCurrentLibraryPathHash == null) //Path base64 is not yet generated since MB startup
                LrCurrentLibraryPathHash = GetStringHash(GetCurrentLibraryPath());
            else if (!cachedResult.Contains(LrCachedFunctionResultPresetSeparator + LrCurrentLibraryPathHash
                + LrCachedFunctionResultPresetSeparator)) //Maybe library has been switched since MB startup

                LrCurrentLibraryPathHash = GetStringHash(GetCurrentLibraryPath());


            var trackId = GetPersistentTrackIdInt(queriedFile);

            if (!queriedFile.EndsWith(".asx") && !cachedResult.Contains(LrCachedFunctionResultPresetSeparator + LrCurrentLibraryPathHash
                + LrCachedFunctionResultPresetSeparator)) //Function results cache hasn't been filled for the current library 
            {
                bool notYetCachedTrack;
                lock (LrTrackCacheNeededToBeUpdated)
                    notYetCachedTrack = LrTrackCacheNeededToBeUpdated.ContainsKey(trackId);

                //New tracks have been added to the library, and they are not yet pre-cached
                if (notYetCachedTrack)
                    return null;

                //Function results cache is not initially filled for all tracks of the library.
                //Maybe new tracks have been added to library, or function results cache is filled for the other libraries only.
                //Needed to apply all affected presets.
                var libraryPathHash = GetCurrentLibraryPath().GetHashCode();

                if (LrLastAskedCacheFillLibraryPathHash != libraryPathHash) //Let's check if the question below hs been asked already
                {
                    LrLastAskedCacheFillLibraryPathHash = libraryPathHash;

                    var result = MessageBox.Show(this, MsgLrCachedPresetsNotApplied,
                        string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                    if (result == DialogResult.No)
                        return null;


                    var notCachedPresets = new List<ReportPreset>();

                    foreach (var preset in SavedSettings.reportPresets)
                    {
                        for (var i = 0; i < preset.functionIds.Length; i++)
                        {
                            destinationTagId = 0;
                            idIndex = getCachedFunctionResultIndex(preset.functionIds[i]);

                            if (idIndex >= 0)
                                destinationTagId = GetTagId(preset.destinationTags[idIndex]);

                            if (destinationTagId != 0)
                            {
                                var checkedCachedResult = GetFileTag(queriedFile, destinationTagId);

                                //functionIds[i] results cache hasn't been filled for the current library
                                if (!checkedCachedResult.Contains(LrCachedFunctionResultPresetSeparator + LrCurrentLibraryPathHash
                                    + LrCachedFunctionResultPresetSeparator))
                                {
                                    notCachedPresets.Add(preset);
                                    break; //Let's break functionIds loop
                                }
                            }
                        }
                    }

                    using (var tagToolsForm = new LrFunctionPrecaching(TagToolsPlugin))
                        tagToolsForm.precachePresets(SavedSettings.reportPresets, notCachedPresets);

                    //return CtlLrRefreshUi; //Maybe getCachedFunctionResult(queriedFile, functionId); ? //---
                    return getCachedFunctionResult(queriedFile, functionId);
                }
                else
                {
                    return null;
                }
            }


            //Here all is fine with library path base64. Let's check preset cache
            return getCachedFunctionResultInternal(cachedResult);
        }

        private string getFunctionResult(string queriedFile, string functionId, AggregatedTags tags,
            SortedDictionary<int, List<string>> filesActualComposedSplitGroupingTagsLists)
        {
            ConvertStringsResult[] functionResults = null;
            var trackId = GetPersistentTrackIdInt(queriedFile);

            if (filesActualComposedSplitGroupingTagsLists.TryGetValue(trackId, out var fileActualComposedGroupingTagsList))
            {
                foreach (var actualComposedGroupingTags in fileActualComposedGroupingTagsList)
                {
                    if (!tags.TryGetValue(actualComposedGroupingTags, out functionResults))
                    {
                        clearAppliedPresetCache();
                        return CtlLrError;
                    }

                    if (functionResults.Length == 0)
                    {
                        clearAppliedPresetCache();
                        return CtlLrError;
                    }
                }
            }
            else
            {
                clearAppliedPresetCache();
                return CtlLrError;
            }


            var savedFunctionIndex = savedFunctionIds.IndexOf(functionId);
            if (savedFunctionIndex >= 0)
            {
                var functionResult = AggregatedTags.GetField(null, functionResults, groupingsDict.Count + savedFunctionIndex, groupingsDict, //-V3080
                    operations[savedFunctionIndex], mulDivFactors[savedFunctionIndex], precisionDigits[savedFunctionIndex], appendTexts[savedFunctionIndex]);

                if (destinationTagIds[savedFunctionIndex] > 0) //destinationTagIds[savedFunctionIndex] is used as cache for function id
                {
                    var cachedResult = GetFileTag(queriedFile, destinationTagIds[savedFunctionIndex]);
                    var fullNewResult = getAllLibrariesFunctionResultsInternal(cachedResult, functionResult);

                    SetFileTag(queriedFile, destinationTagIds[savedFunctionIndex], fullNewResult, true);
                    CommitTagsToFile(queriedFile, true, true);
                }

                return functionResult;
            }

            return "???";
        }

        private void saveFields(PluginWindowTemplate form, string[] queriedFiles, bool interactive, bool forceCacheUpdate, AggregatedTags tags)
        {
            var filesActualComposedGroupingTags = CachedPresetsFilesActualComposedSplitGroupingTagsList[appliedPreset.guid];

            for (var i = 0; i < queriedFiles.Length; i++)
            {

                if (checkStoppingStatus())
                {
                    if (interactive)
                    {
                        Invoke(new Action(() =>
                        {
                            if (backgroundTaskIsUpdatingTags)
                                stopButtonClickedMethod(applyingChangesStopped);
                            else
                                stopButtonClickedMethod(prepareBackgroundPreview);
                        }));
                    }
                    else
                    {
                        nonInteractiveUpdatingTaskStopped();
                    }

                    return;
                }


                var trackId = GetPersistentTrackIdInt(queriedFiles[i]);
                var tagsSet = false;

                if (interactive)
                    SetStatusBarTextForFileOperations(form, LibraryReportsSbText, false, i, queriedFiles.Length, appliedPreset.getName());
                else if (!forceCacheUpdate)
                    SetStatusBarTextForFileOperations(form, ApplyingLrPresetSbText, false, i, queriedFiles.Length, appliedPreset.getName());

                var composedActualGroupingTagsList = filesActualComposedGroupingTags[trackId];
                if (composedActualGroupingTagsList.Count != 1)
                    return;

                foreach (var composedActualGroupingTags in composedActualGroupingTagsList)
                {
                    var fileFunctionResults = tags[composedActualGroupingTags]; //Let's write aggregated results for entire file

                    if (checkCondition(composedActualGroupingTags, fileFunctionResults))
                    {
                        var j = -1;
                        foreach (var attribs in functionsDict.Values)
                        {
                            j++;

                            if (destinationTagIds[j] > 0 && !string.IsNullOrWhiteSpace(savedFunctionIds[j])) //destinationTagIds[j] is used as cache for function id
                            {
                                var result = AggregatedTags.GetField(composedActualGroupingTags, fileFunctionResults, groupingsDict.Count + j, groupingsDict,
                                    operations[j], mulDivFactors[j], precisionDigits[j], appendTexts[j]);

                                var cachedResult = GetFileTag(queriedFiles[i], destinationTagIds[j]);
                                var fullNewResult = getAllLibrariesFunctionResultsInternal(cachedResult, result);

                                tagsSet = SetFileTag(queriedFiles[i], destinationTagIds[j], fullNewResult, true);
                            }
                            else if (destinationTagIds[j] > 0)
                            {
                                var result = AggregatedTags.GetField(composedActualGroupingTags, fileFunctionResults, groupingsDict.Count + j, groupingsDict,
                                    operations[j], mulDivFactors[j], precisionDigits[j], appendTexts[j]);

                                tagsSet = SetFileTag(queriedFiles[i], destinationTagIds[j], result, true);
                            }
                        }

                        break; //Continue with the next file, because only one of composed grouping tags can be saved to the same file anyway
                    }
                    else //Let's write empty string if condition hasn't passed and custom tags are used as a cache
                    {
                        var j = -1;
                        foreach (var attribs in functionsDict.Values)
                        {
                            j++;

                            if (destinationTagIds[j] > 0 && !string.IsNullOrWhiteSpace(savedFunctionIds[j])) //destinationTagIds[j] is used as cache for function id
                            {
                                var cachedResult = GetFileTag(queriedFiles[i], destinationTagIds[j]);
                                var fullNewResult = getAllLibrariesFunctionResultsInternal(cachedResult, string.Empty);

                                tagsSet = SetFileTag(queriedFiles[i], destinationTagIds[j], fullNewResult, true);
                            }
                        }

                        break; //Continue with the next file, because only one of composed grouping tags can be saved to the same file anyway
                    }
                }


                if (tagsSet)
                    CommitTagsToFile(queriedFiles[i], true, true);
            }

            if (interactive)
            {
                Invoke(new Action(() =>
                {
                    if (backgroundTaskIsUpdatingTags)
                        stopButtonClickedMethod(applyingChangesStopped);
                    else
                        stopButtonClickedMethod(prepareBackgroundPreview);
                }));
            }
            else
            {
                nonInteractiveUpdatingTaskStopped();
            }


            if (interactive || !forceCacheUpdate)
                SetResultingSbText(form, appliedPreset.getName(), true, true);
        }

        //Dictionaries of tags values, arrays of groupings
        internal protected static void PrepareGroupingTagDictionaries(ReportPreset preset,
            ref SortedDictionary<string, bool>[] queriedGroupingTagsRaw,
            ref SortedDictionary<string, bool>[] queriedActualGroupingTags,
            ref SortedDictionary<string, bool>[] queriedActualGroupingTagsRaw
            )
        {
            //Only grouping tags, which can be used in MB query (not required if only groupings are queried)
            if (queriedGroupingTagsRaw == null)
            {
                queriedGroupingTagsRaw = new SortedDictionary<string, bool>[preset.groupings.Length];
                for (var l = 0; l < queriedGroupingTagsRaw.Length; l++)
                    queriedGroupingTagsRaw[l] = new SortedDictionary<string, bool>();
            }

            //All displayed (formatted) grouping tags as is (not required if only groupings are queried)
            if (queriedActualGroupingTags == null)
            {
                queriedActualGroupingTags = new SortedDictionary<string, bool>[preset.groupings.Length];

                for (var l = 0; l < queriedActualGroupingTags.Length; l++)
                    queriedActualGroupingTags[l] = new SortedDictionary<string, bool>();
            }

            //All raw grouping tags as is (not required if only groupings are queried)
            if (queriedActualGroupingTagsRaw == null)
            {
                queriedActualGroupingTagsRaw = new SortedDictionary<string, bool>[preset.groupings.Length];

                for (var l = 0; l < queriedActualGroupingTagsRaw.Length; l++)
                    queriedActualGroupingTagsRaw[l] = new SortedDictionary<string, bool>();
            }
        }

        internal void autoApplyReportPresetsOnStartup()
        {
            try
            {
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;

                if (ReportPresetsForAutoApplying.Count == 0)
                {
                    BackgroundTaskIsInProgress = false;
                    return;
                }


                MbApiInterface.Library_QueryFilesEx("domain=Library", out var files);
                if (files == null || files.Length == 0)
                {
                    BackgroundTaskIsInProgress = false;
                    return;
                }

                lock (ReportPresetsForAutoApplying)
                {
                    foreach (var autoLibraryReportsPreset in ReportPresetsForAutoApplying)
                    {
                        lock (LrPresetExecutionLocker)
                        {
                            appliedPreset = autoLibraryReportsPreset;
                            backgroundTaskIsUpdatingTags = true;
                            executePreset(null, null, false, true, null, false, true);
                        }
                    }
                }
            }
            catch (ThreadAbortException) //-V3163 //-V5606
            {
                //Let's just stop the thread...
            }

            BackgroundTaskIsInProgress = false;
            RefreshPanels(true);
        }

        internal void applyReportPresetToLibrary()
        {
            lock (LrPresetExecutionLocker)
            {
                try
                {
                    Thread.CurrentThread.Priority = ThreadPriority.Lowest;

                    backgroundTaskIsUpdatingTags = true;
                    executePreset(null, null, false, true, null, false, true);
                }
                catch (ThreadAbortException) //-V3163 //-V5606
                {
                    //Let's just stop the thread...
                }
            }

            BackgroundTaskIsInProgress = false;

            RefreshPanels(true);
        }

        internal void applyReportPresetToSelectedTracks()
        {
            lock (LrPresetExecutionLocker)
            {
                try
                {
                    Thread.CurrentThread.Priority = ThreadPriority.Lowest;

                    MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out var selectedFiles);
                    if (selectedFiles == null || selectedFiles.Length == 0)
                    {
                        SetStatusBarText(this, MsgNoTracksSelected, true);
                        System.Media.SystemSounds.Exclamation.Play();
                    }
                    else
                    {
                        backgroundTaskIsUpdatingTags = true;
                        executePreset(this, selectedFiles, false, true, null, false, true);
                    }
                }
                catch (ThreadAbortException) //-V3163 //-V5606
                {
                    //Let's just stop the thread...
                }
            }

            BackgroundTaskIsInProgress = false;
            RefreshPanels(true);
        }

        internal static void LrNotifyFileTagsChanged(string newChangedFile, string changingFile)
        {
            if (!ReportPresetIdsAreInitialized)
                return;
            else if (IdsReportPresets.Count == 0)
                return;


            if (newChangedFile != null)
            {
                var trackId = GetPersistentTrackIdInt(newChangedFile);

                lock (LrTrackCacheNeededToBeUpdated)
                    if (!LrTrackCacheNeededToBeUpdated.AddSkip(trackId, newChangedFile))
                        LrTrackCacheNeededToBeUpdatedLastAddedTime = DateTime.UtcNow;
            }

            if (changingFile != null)
            {
                while (BackgroundTaskIsInProgress)
                    Thread.Sleep(ActionRetryDelay);

                BackgroundTaskIsInProgress = true;

                lock (IdsReportPresets)
                {
                    var trackId = GetPersistentTrackIdInt(changingFile);

                    foreach (var idPreset in IdsReportPresets)
                    {
                        lock (LrPresetExecutionLocker)
                        {
                            lock (PresetChangingGroupingTagsRaw)
                            {
                                if (!PresetChangingGroupingTagsRaw.TryGetValue(idPreset.Value.guid, out var filesQueriedGroupingTagsRaw))
                                {
                                    filesQueriedGroupingTagsRaw = new SortedDictionary<int, SortedDictionary<string, bool>[]>();
                                    PresetChangingGroupingTagsRaw.Add(idPreset.Value.guid, filesQueriedGroupingTagsRaw);
                                }

                                if (!PresetChangingActualGroupingTags.TryGetValue(idPreset.Value.guid, out var filesQueriedActualGroupingTags))
                                {
                                    filesQueriedActualGroupingTags = new SortedDictionary<int, SortedDictionary<string, bool>[]>();
                                    PresetChangingActualGroupingTags.Add(idPreset.Value.guid, filesQueriedActualGroupingTags);
                                }

                                if (!PresetChangingActualGroupingTagsRaw.TryGetValue(idPreset.Value.guid, out var filesQueriedActualGroupingTagsRaw))
                                {
                                    filesQueriedActualGroupingTagsRaw = new SortedDictionary<int, SortedDictionary<string, bool>[]>();
                                    PresetChangingActualGroupingTagsRaw.Add(idPreset.Value.guid, filesQueriedActualGroupingTagsRaw);
                                }


                                bool addNewDictionaries = !filesQueriedGroupingTagsRaw.TryGetValue(trackId, out var queriedGroupingTagsRaw);
                                filesQueriedActualGroupingTags.TryGetValue(trackId, out var queriedActualGroupingTags);
                                filesQueriedActualGroupingTagsRaw.TryGetValue(trackId, out var queriedActualGroupingTagsRaw);


                                //Dictionaries of tags values, arrays of groupings. Dictionaries won't be recreated if they aren't nulls
                                PrepareGroupingTagDictionaries(
                                            idPreset.Value,
                                            ref queriedGroupingTagsRaw,
                                            ref queriedActualGroupingTags,
                                            ref queriedActualGroupingTagsRaw
                                            );

                                if (addNewDictionaries)
                                {
                                    filesQueriedGroupingTagsRaw.Add(trackId, queriedGroupingTagsRaw);
                                    filesQueriedActualGroupingTags.Add(trackId, queriedActualGroupingTags);
                                    filesQueriedActualGroupingTagsRaw.Add(trackId, queriedActualGroupingTagsRaw);
                                }

                                LibraryReportsCommandForFunctionIds.appliedPreset = idPreset.Value;
                                LibraryReportsCommandForFunctionIds.backgroundTaskIsUpdatingTags = false;

                                //queriedGroupingTagsRaw, queriedActualGroupingTags, queriedActualGroupingTagsRaw may be nulled. Check this! 
                                //Let's remember grouping tag value, which must be changed soon (it's now TagsChanging notification, let's process
                                //these tags on TagsChanged notification)
                                LibraryReportsCommandForFunctionIds.executePreset(null,
                                    new string[] { changingFile },
                                    false, true, null, false, false,
                                    queriedGroupingTagsRaw, queriedActualGroupingTags, queriedActualGroupingTagsRaw, true);
                            }
                        }
                    }
                }

                BackgroundTaskIsInProgress = false;
            }
        }

        internal static void DelayedFunctionCacheUpdate(object state)
        {
            if (!ReportPresetIdsAreInitialized)
                return;
            else if (IdsReportPresets.Count == 0)
                return;

            if (!DelayedFunctionCacheUpdatePrioritySet)
            {
                DelayedFunctionCacheUpdatePrioritySet = true;
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;
            }



            var skipFunctionsUpdate = true;

            lock (LrTrackCacheNeededToBeUpdated)
            {
                if (LrTrackCacheNeededToBeUpdatedWorkingCopy.Length != 0)
                {
                    skipFunctionsUpdate = false;
                }
                else if (LrTrackCacheNeededToBeUpdated.Count > 0 && DateTime.UtcNow - LrTrackCacheNeededToBeUpdatedLastAddedTime > LrTrackCacheNeededToBeUpdatedAccumulationPeriod)
                {
                    skipFunctionsUpdate = false;

                    if (PresetChangingGroupingTagsRaw.Count > 0)
                    {
                        foreach (var idPreset in IdsReportPresets)
                        {
                            lock (PresetChangingGroupingTagsRaw)
                            {
                                PresetChangingGroupingTagsRaw.TryGetValue(idPreset.Value.guid, out var filesQueriedGroupingTagsRaw);
                                PresetChangingActualGroupingTags.TryGetValue(idPreset.Value.guid, out var filesQueriedActualGroupingTags);
                                PresetChangingActualGroupingTagsRaw.TryGetValue(idPreset.Value.guid, out var filesQueriedActualGroupingTagsRaw);

                                if (filesQueriedGroupingTagsRaw != null)
                                {
                                    foreach (int trackId in filesQueriedGroupingTagsRaw.Keys)
                                    {
                                        if (LrTrackCacheNeededToBeUpdated.TryGetValue(trackId, out var trackUrl))
                                        {
                                            var queriedGroupingTagsRaw = filesQueriedGroupingTagsRaw[trackId];
                                            var queriedActualGroupingTags = filesQueriedActualGroupingTags[trackId];
                                            var queriedActualGroupingTagsRaw = filesQueriedActualGroupingTagsRaw[trackId];


                                            filesQueriedActualGroupingTagsRaw.Remove(trackId);
                                            filesQueriedActualGroupingTags.Remove(trackId);
                                            filesQueriedActualGroupingTagsRaw.Remove(trackId);

                                            if (filesQueriedActualGroupingTagsRaw.Count == 0)
                                            {
                                                PresetChangingGroupingTagsRaw.Remove(idPreset.Value.guid);
                                                PresetChangingActualGroupingTags.Remove(idPreset.Value.guid);
                                                PresetChangingActualGroupingTagsRaw.Remove(idPreset.Value.guid);
                                            }


                                            if (!ChangingGroupingTagsRawWorkingCopy.TryGetValue(idPreset.Value.guid,
                                                out var existingQueriedGroupingTagsRaw))
                                            {
                                                existingQueriedGroupingTagsRaw = new SortedDictionary<string, bool>[queriedGroupingTagsRaw.Length];
                                                for (int j = 0; j < existingQueriedGroupingTagsRaw.Length; j++)
                                                    existingQueriedGroupingTagsRaw[j] = new SortedDictionary<string, bool>();

                                                ChangingGroupingTagsRawWorkingCopy.Add(idPreset.Value.guid, existingQueriedGroupingTagsRaw);
                                            }

                                            if (!ChangingActualGroupingTagsWorkingCopy.TryGetValue(idPreset.Value.guid,
                                                out var existingQueriedActualGroupingTags))
                                            {
                                                existingQueriedActualGroupingTags = new SortedDictionary<string, bool>[queriedActualGroupingTags.Length];
                                                for (int j = 0; j < existingQueriedActualGroupingTags.Length; j++)
                                                    existingQueriedActualGroupingTags[j] = new SortedDictionary<string, bool>();

                                                ChangingActualGroupingTagsWorkingCopy.Add(idPreset.Value.guid, existingQueriedActualGroupingTags);
                                            }

                                            if (!ChangingActualGroupingTagsRawWorkingCopy.TryGetValue(idPreset.Value.guid,
                                                out var existingQueriedActualGroupingTagsRaw))
                                            {
                                                existingQueriedActualGroupingTagsRaw = new SortedDictionary<string, bool>[queriedActualGroupingTagsRaw.Length];
                                                for (int j = 0; j < existingQueriedActualGroupingTagsRaw.Length; j++)
                                                    existingQueriedActualGroupingTagsRaw[j] = new SortedDictionary<string, bool>();

                                                ChangingActualGroupingTagsRawWorkingCopy.Add(idPreset.Value.guid, existingQueriedActualGroupingTagsRaw);
                                            }


                                            for (int j = 0; j < queriedGroupingTagsRaw.Length; j++)
                                            {
                                                foreach (var groupingValue in queriedGroupingTagsRaw[j].Keys)
                                                    existingQueriedGroupingTagsRaw[j].AddSkip(groupingValue);
                                            }

                                            for (int j = 0; j < queriedActualGroupingTags.Length; j++)
                                            {
                                                foreach (var groupingValue in queriedActualGroupingTags[j].Keys)
                                                    existingQueriedActualGroupingTags[j].AddSkip(groupingValue);
                                            }

                                            for (int j = 0; j < queriedActualGroupingTagsRaw.Length; j++)
                                            {
                                                foreach (var groupingValue in queriedActualGroupingTagsRaw[j].Keys)
                                                    existingQueriedActualGroupingTagsRaw[j].AddSkip(groupingValue);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }


                    LrTrackCacheNeededToBeUpdatedWorkingCopy = new string[LrTrackCacheNeededToBeUpdated.Count];

                    lock (LrTrackCacheNeededToBeUpdatedWorkingCopy)
                    {
                        int i = 0;
                        foreach (var changedFile in LrTrackCacheNeededToBeUpdated.Values)
                            LrTrackCacheNeededToBeUpdatedWorkingCopy[i++] = changedFile;

                        LrTrackCacheNeededToBeUpdated.Clear();
                    }
                }


                if (skipFunctionsUpdate)
                    goto loop_end;


                if (BackgroundTaskIsInProgress)
                    Thread.Sleep(ActionRetryDelay);

                if (BackgroundTaskIsInProgress)
                    goto loop_end;


                BackgroundTaskIsInProgress = true;

                lock (IdsReportPresets)
                {
                    foreach (var idPreset in IdsReportPresets)
                    {
                        lock (LrPresetExecutionLocker)
                        {
                            if (PresetsProcessedByFunctionCacheUpdate.AddUnique(idPreset.Value.guid))
                            {
                                var removeGroupings = ChangingGroupingTagsRawWorkingCopy.TryGetValue(idPreset.Value.guid,
                                    out var queriedChangingGroupingTagsRawWorkingCopy);
                                ChangingActualGroupingTagsWorkingCopy.TryGetValue(idPreset.Value.guid,
                                    out var queriedChangingActualGroupingTagsWorkingCopy);
                                ChangingActualGroupingTagsRawWorkingCopy.TryGetValue(idPreset.Value.guid,
                                    out var queriedChangingActualGroupingTagsRawWorkingCopy);


                                LibraryReportsCommandForFunctionIds.appliedPreset = idPreset.Value;
                                LibraryReportsCommandForFunctionIds.backgroundTaskIsUpdatingTags = true;

                                LibraryReportsCommandForFunctionIds.executePreset(null, null, false, true, null, false, true,
                                    queriedChangingGroupingTagsRawWorkingCopy,
                                    queriedChangingActualGroupingTagsWorkingCopy,
                                    queriedChangingActualGroupingTagsRawWorkingCopy,
                                    false);

                                //LibraryReportsCommandForFunctionIds.backgroundTaskIsUpdatingTags = true;

                                LibraryReportsCommandForFunctionIds.executePreset(null,
                                    LrTrackCacheNeededToBeUpdatedWorkingCopy,
                                    false, true, idPreset.Key, false, true);



                                if (removeGroupings)
                                {
                                    ChangingGroupingTagsRawWorkingCopy.Remove(idPreset.Value.guid);
                                    ChangingActualGroupingTagsWorkingCopy.Remove(idPreset.Value.guid);
                                    ChangingActualGroupingTagsRawWorkingCopy.Remove(idPreset.Value.guid);
                                }
                            }
                        }
                    }
                }

                BackgroundTaskIsInProgress = false;

                LrTrackCacheNeededToBeUpdatedWorkingCopy = new string[0];
                PresetsProcessedByFunctionCacheUpdate.Clear();

                RefreshPanels();

            loop_end:
                ; //Let's wait for the next timer event
            }
        }

        internal static void AutoApplyReportPresets()
        {
            if (SavedSettings.dontShowLibraryReports)
                return;
            else if (!SavedSettings.allowAsrLrPresetAutoExecution)
                return;
            else if (LibraryReportsCommandForAutoApplying == null)
                return;
            else if (ReportPresetsForAutoApplying.Count == 0)
                return;

            if (BackgroundTaskIsInProgress)
            {
                SetStatusBarText(null, AnotherLrPresetIsRunningSbText, false);
                System.Media.SystemSounds.Exclamation.Play();

                return;
            }
            else
            {
                BackgroundTaskIsInProgress = true;
            }

            MbApiInterface.MB_CreateBackgroundTask(LibraryReportsCommandForAutoApplying.autoApplyReportPresetsOnStartup, null);
        }

        internal static void ApplyReportPreset(ReportPreset preset, ReportPreset[] reportPresets, LibraryReports form = null)
        {
            if (BackgroundTaskIsInProgress && (form == null || !form.backgroundTaskIsScheduled))
            {
                SetStatusBarText(form, AnotherLrPresetIsRunningSbText, false);
                System.Media.SystemSounds.Exclamation.Play();

                return;
            }
            else
            {
                BackgroundTaskIsInProgress = true;
            }

            if (form != null)
            {
                form.reportPresets = reportPresets;
                form.appliedPreset = preset;
                form.backgroundTaskIsUpdatingTags = true;

                form.switchOperation(form.applyReportPresetToSelectedTracks, form.buttonOK, form.buttonOK, form.buttonPreview, form.buttonClose, true, null);
            }
            else
            {
                LibraryReportsCommandForHotkeys.reportPresets = reportPresets;
                LibraryReportsCommandForHotkeys.appliedPreset = preset;
                LibraryReportsCommandForHotkeys.backgroundTaskIsUpdatingTags = true;

                if (preset.applyToSelectedTracks)
                    MbApiInterface.MB_CreateBackgroundTask(LibraryReportsCommandForHotkeys.applyReportPresetToSelectedTracks, null);
                else
                    MbApiInterface.MB_CreateBackgroundTask(LibraryReportsCommandForHotkeys.applyReportPresetToLibrary, null);
            }
        }

        internal static void ApplyReportPresetByHotkey(int presetIndex)
        {
            var preset = ReportPresetsWithHotkeys[presetIndex - 1];

            if (preset.hotkeySlot != presetIndex - 1)
            {
                MessageBox.Show(MbForm, SbLrHotkeysAreAssignedIncorrectly, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lock (ReportPresetsWithHotkeys)
                ApplyReportPreset(preset, LibraryReportsCommandForHotkeys.reportPresets);
        }

        internal static string AutoCalculateReportPresetFunction(string calculatedFile, string functionId)
        {
            if (!ReportPresetIdsAreInitialized)
                return CtlLrRefreshUi;

            if (!IdsReportPresets.TryGetValue(functionId, out var preset))
                return CtlLrIncorrectReportPresetId;


            LibraryReportsCommandForFunctionIds.appliedPreset = preset;
            LibraryReportsCommandForFunctionIds.backgroundTaskIsUpdatingTags = true;

            var returnValue = LibraryReportsCommandForFunctionIds.getCachedFunctionResult(calculatedFile, functionId);

            if (returnValue != null)
                return returnValue;

            var selectedFiles = new[] { calculatedFile };
            return LibraryReportsCommandForFunctionIds.executePreset(null, selectedFiles, false, true, functionId, false, false);
        }

        private static int FindFirstSlot(bool[] slots, bool searchedValue)
        {
            for (var i = 0; i < slots.Length; i++)
            {
                if (slots[i] == searchedValue)
                    return i;
            }

            return -1;
        }

        private static void RegisterPresetHotkey(Plugin plugin, ReportPreset preset, int slot)
        {
            if (preset == null)
                return;


            switch (slot)
            {
                case 0:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset1EventHandler);
                    break;
                case 1:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset2EventHandler);
                    break;
                case 2:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset3EventHandler);
                    break;
                case 3:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset4EventHandler);
                    break;
                case 4:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset5EventHandler);
                    break;
                case 5:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset6EventHandler);
                    break;
                case 6:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset7EventHandler);
                    break;
                case 7:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset8EventHandler);
                    break;
                case 8:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset9EventHandler);
                    break;
                case 9:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset10EventHandler);
                    break;
                case 10:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset11EventHandler);
                    break;
                case 11:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset12EventHandler);
                    break;
                case 12:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset13EventHandler);
                    break;
                case 13:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset14EventHandler);
                    break;
                case 14:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset15EventHandler);
                    break;
                case 15:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset16EventHandler);
                    break;
                case 16:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset17EventHandler);
                    break;
                case 17:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset18EventHandler);
                    break;
                case 18:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset19EventHandler);
                    break;
                case 19:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset20EventHandler);
                    break;
                default:
                    throw new Exception(ExIncorrectLrHotkeySlot.Replace("%%SLOT%%", slot.ToString()));
            }

            if (LrPresetsMenuItem != null)
            {
                switch (slot)
                {
                    case 0:
                        LrPresetsMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset1EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 1:
                        LrPresetsMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset2EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 2:
                        LrPresetsMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset3EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 3:
                        LrPresetsMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset4EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 4:
                        LrPresetsMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset5EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 5:
                        LrPresetsMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset6EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 6:
                        LrPresetsMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset7EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 7:
                        LrPresetsMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset8EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 8:
                        LrPresetsMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset9EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 9:
                        LrPresetsMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset10EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 10:
                        LrPresetsMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset11EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 11:
                        LrPresetsMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset12EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 12:
                        LrPresetsMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset13EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 13:
                        LrPresetsMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset14EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 14:
                        LrPresetsMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset15EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 15:
                        LrPresetsMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset16EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 16:
                        LrPresetsMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset17EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 17:
                        LrPresetsMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset18EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 18:
                        LrPresetsMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset19EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 19:
                        LrPresetsMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset20EventHandler);//===.Image = LrPresetIcon;
                        break;
                    default:
                        throw new Exception(ExIncorrectLrHotkeySlot.Replace("%%SLOT%%", slot.ToString()));
                }
            }

            if (LrPresetsContextMenuItem != null)
            {
                switch (slot)
                {
                    case 0:
                        LrPresetsContextMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset1EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 1:
                        LrPresetsContextMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset2EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 2:
                        LrPresetsContextMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset3EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 3:
                        LrPresetsContextMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset4EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 4:
                        LrPresetsContextMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset5EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 5:
                        LrPresetsContextMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset6EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 6:
                        LrPresetsContextMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset7EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 7:
                        LrPresetsContextMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset8EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 8:
                        LrPresetsContextMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset9EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 9:
                        LrPresetsContextMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset10EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 10:
                        LrPresetsContextMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset11EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 11:
                        LrPresetsContextMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset12EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 12:
                        LrPresetsContextMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset13EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 13:
                        LrPresetsContextMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset14EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 14:
                        LrPresetsContextMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset15EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 15:
                        LrPresetsContextMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset16EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 16:
                        LrPresetsContextMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset17EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 17:
                        LrPresetsContextMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset18EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 18:
                        LrPresetsContextMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset19EventHandler);//===.Image = LrPresetIcon;
                        break;
                    case 19:
                        LrPresetsContextMenuItem.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset20EventHandler);//===.Image = LrPresetIcon;
                        break;
                    default:
                        throw new Exception(ExIncorrectLrHotkeySlot.Replace("%%SLOT%%", slot.ToString()));
                }
            }
        }

        private static void InitLrPresetsHotkeys()
        {
            lock (ReportPresetsWithHotkeys)
            {
                LrPresetsWithHotkeysCount = 0;
                ReportPresetsWithHotkeys = new ReportPreset[MaximumNumberOfLrHotkeys];
                LibraryReportsCommandForHotkeys?.Dispose();
                LibraryReportsCommandForHotkeys = null;

                if (SavedSettings.dontShowLibraryReports)
                    return;

                LibraryReportsCommandForHotkeys = new LibraryReports();

                for (var i = 0; i < SavedSettings.reportPresets.Length; i++)
                {
                    if (SavedSettings.reportPresets[i].hotkeyAssigned)
                    {
                        var libraryReportsPreset = new ReportPreset(SavedSettings.reportPresets[i], true);
                        ReportPresetsWithHotkeys[libraryReportsPreset.hotkeySlot] = libraryReportsPreset;
                        LrPresetsWithHotkeysCount++;

                        FillPresetFilteringChain(SavedSettings.reportPresets, AllLrPresetGuidsInUse, libraryReportsPreset);
                    }
                }

                LibraryReportsCommandForHotkeys.reportPresets = new ReportPreset[SavedSettings.reportPresets.Length];
                SavedSettings.reportPresets.CopyTo(LibraryReportsCommandForHotkeys.reportPresets, 0);
            }
        }

        private static void InitLrPresetsFunctionIds()
        {
            lock (IdsReportPresets)
            {
                UpdateFunctionCacheTimer?.Dispose();

                ReportPresetIdsAreInitialized = false;

                lock (IdsReportPresets)
                    IdsReportPresets.Clear();

                LibraryReportsCommandForFunctionIds?.Dispose();
                LibraryReportsCommandForFunctionIds = null;

                if (SavedSettings.dontShowLibraryReports)
                    return;

                LibraryReportsCommandForFunctionIds = new LibraryReports();

                for (var i = 0; i < SavedSettings.reportPresets.Length; i++)
                {
                    for (var j = 0; j < SavedSettings.reportPresets[i].functionIds.Length; j++)
                    {
                        if (!string.IsNullOrWhiteSpace(SavedSettings.reportPresets[i].functionIds[j]))
                        {
                            var libraryReportsPreset = new ReportPreset(SavedSettings.reportPresets[i], true);
                            IdsReportPresets.Add(libraryReportsPreset.functionIds[j], libraryReportsPreset);

                            FillPresetFilteringChain(SavedSettings.reportPresets, AllLrPresetGuidsInUse, libraryReportsPreset);
                        }
                    }
                }

                LibraryReportsCommandForFunctionIds.reportPresets = new ReportPreset[SavedSettings.reportPresets.Length];
                SavedSettings.reportPresets.CopyTo(LibraryReportsCommandForFunctionIds.reportPresets, 0);

                ReportPresetIdsAreInitialized = true;
            }


            if (IdsReportPresets.Count > 0)
                UpdateFunctionCacheTimer = new System.Threading.Timer(DelayedFunctionCacheUpdate, null, FunctionCacheUpdateDelay, FunctionCacheUpdateDelay);
        }

        private static void InitLrPresetsAutoApplying()
        {
            lock (ReportPresetsForAutoApplying)
            {
                LibraryReportsCommandForAutoApplying?.Dispose();
                LibraryReportsCommandForAutoApplying = null;

                if (SavedSettings.dontShowLibraryReports)
                    return;

                LibraryReportsCommandForAutoApplying = new LibraryReports();

                foreach (var preset in SavedSettings.reportPresets)
                {
                    if (preset.autoApply)
                    {
                        ReportPresetsForAutoApplying.Add(preset);

                        FillPresetFilteringChain(SavedSettings.reportPresets, AllLrPresetGuidsInUse, preset);
                    }
                }

                LibraryReportsCommandForAutoApplying.reportPresets = new ReportPreset[SavedSettings.reportPresets.Length];
                SavedSettings.reportPresets.CopyTo(LibraryReportsCommandForAutoApplying.reportPresets, 0);
            }
        }

        internal static void InitLr()
        {
            lock (SavedSettings.reportPresets)
            {
                AllLrPresetGuidsInUse.Clear();

                InitLrPresetsFunctionIds();
                InitLrPresetsHotkeys();
                InitLrPresetsAutoApplying();
            }
        }

        internal static void RegisterLrPresetsHotkeysAndMenuItems(Plugin plugin)
        {
            for (var i = 0; i < ReportPresetsWithHotkeys.Length; i++)
                RegisterPresetHotkey(plugin, ReportPresetsWithHotkeys[i], i);
        }

        private int getSelectedRow(DataGridView table)
        {
            if (table.SelectedRows.Count > 0)
                return table.SelectedRows[0].Index;
            else
                return -1;
        }

        private void selectRow(DataGridView table, int index)
        {
            deselectAllRows(table);
            table.Rows[index].Selected = true;

            if (table == tagsDataGridView)
            {
                tagsDataGridViewSelectedRow = index;
                tagsDataGridViewRowSelected(index);
            }
        }

        private void deselectAllRows(DataGridView table)
        {
            foreach (DataGridViewRow row in table.SelectedRows)
                row.Selected = false;

            if (table == tagsDataGridView)
                tagsDataGridViewSelectedRow = -1;
        }

        private bool addColumn(string fieldName, string parameter2Name, LrFunctionType type, string splitter, bool trimValues, string expression, string colName)
        {
            if (splitter == DontUseSplitter)
                splitter = string.Empty;

            if (fieldName == ArtworkName || fieldName == SequenceNumberName)
            {
                if (type != LrFunctionType.Grouping)
                {
                    if (presetIsLoading)
                        throw new Exception(MsgPleaseUseGroupingFunction);
                    else
                        MessageBox.Show(this, MsgPleaseUseGroupingFunction,
                                string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    return false;
                }

                expression = null;
                colName = string.Empty;
            }

            var columnAttributes = new ColumnAttributes(type, expression, colName, fieldName, parameter2Name, splitter, trimValues);//===


            var uniqueId = columnAttributes.getUniqueId();
            var shortId = columnAttributes.getShortId();

            var columnName = GetColumnName(fieldName, parameter2Name, type, splitter, trimValues, expression, colName, true, true, true);
            var fullFieldName = GetColumnName(fieldName, parameter2Name, type, splitter, trimValues, expression, colName, false, false, true);

            //Some column attributes are not unique, they can be replaced, and must not be included in column name
            var simpleColumnName = GetColumnName(fieldName, parameter2Name, type, null, false, expression, colName, true, false, true);

            var oldSortedShortIdsIndex = sortedShortIds.IndexOf(shortId);
            var oldExpressionIndex = -1;
            if (shortIdsExprs.TryGetValue(shortId, out var exprs1))
                oldExpressionIndex = exprs1.IndexOf(expression);//===

            //Let's check if the column exists already
            var replacedColumnIndex = -1;

            if (newColumn == null) //Updating splitter, trim or expression. Let's silently replace old column 
            {
                updateColumns(shortId, fieldName, parameter2Name, type, splitter, trimValues, expression, colName);
                expressionBackup = expression;
                splitterBackup = splitter;
                trimValuesBackup = trimValues;
                columnNameBackup = colName;

                return true;
            }
            else if (groupingsDict.ContainsKey(uniqueId) || functionsDict.ContainsKey(uniqueId))
            {
                if (presetIsLoading)
                    throw new Exception(ExThisFieldAlreadyDefinedInPreset);
                else
                {
                    if (MessageBox.Show(this, MsgDoYouWantToReplaceTheField.Replace(@"\\", "\n\n").Replace("%%FIELD-NAME%%", simpleColumnName),
                    string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return false;
                    else //Confirmed replacement
                        replacedColumnIndex = removeColumn(fieldName, parameter2Name, type, splitter, trimValues, expression, colName);
                }
            }


            var column = columnTemplate.Clone() as DataGridViewColumn;
            column.HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            column.HeaderText = columnName;
            column.DataPropertyName = columnName;
            column.HeaderCell.Tag = uniqueId;


            conditionFieldListCustom.Add(fullFieldName);
            if (conditionFieldListCustom.SelectedIndex == -1)
                conditionFieldListCustom.SelectedIndex = 0;

            comparedFieldListCustom.Add(fullFieldName);

            conditionCheckBox.Enable(true);
            conditionCheckBox_CheckedChanged(null, null);

            if (fieldName == ArtworkName)
            {
                var imageColumn = artworkColumnTemplate.Clone() as DataGridViewImageColumn;
                imageColumn.HeaderText = fieldName;
                imageColumn.DataPropertyName = fieldName;
                imageColumn.Width = previewTableArtworkSize;
                imageColumn.HeaderCell.Tag = uniqueId;


                if (replacedColumnIndex == -1)
                    replacedColumnIndex = groupingsDict.Count;

                previewTable.Columns.Insert(replacedColumnIndex, imageColumn);
                previewTable.Columns[replacedColumnIndex].HeaderCell.Style = headerCellStyle;
                groupingsDict.Add(uniqueId, columnAttributes);
            }
            else if (type == LrFunctionType.Grouping)
            {
                if (replacedColumnIndex == -1)
                    replacedColumnIndex = groupingsDict.Count;

                previewTable.Columns.Insert(replacedColumnIndex, column);
                previewTable.Columns[replacedColumnIndex].HeaderCell.Style = headerCellStyle;
                groupingsDict.Add(uniqueId, columnAttributes);
            }
            else
            {
                if (replacedColumnIndex == -1)
                {
                    previewTable.Columns.Add(column);
                    previewTable.Columns[previewTable.ColumnCount - 1].HeaderCell.Style = headerCellStyle;
                }
                else
                {
                    previewTable.Columns.Insert(replacedColumnIndex, column);
                    previewTable.Columns[replacedColumnIndex].HeaderCell.Style = headerCellStyle;
                }

                functionsDict.AddSkipUi(uniqueId, columnAttributes, buttonOK, assignHotkeyCheckBox);


                destinationTagListCustom.SelectedItem = NullTagName;
                savedFunctionIds.Add(string.Empty);
                savedDestinationTagsNames.Add(destinationTagListCustom.SelectedItem as string);

                operations.Add(0);
                mulDivFactors.Add(mulDivFactorComboBoxCustom.Items[0] as string);
                precisionDigits.Add(precisionDigitsComboBoxCustom.Items[0] as string);
                appendTexts.Add(string.Empty);

                sourceFieldComboBoxCustom.Add(fullFieldName);
                if (sourceFieldComboBoxCustom.SelectedIndex == -1)
                    sourceFieldComboBoxCustom.SelectedIndex = 0;
            }


            sortedShortIds = groupingsDict.idsToSortedList(false);
            sortedShortIds = functionsDict.idsToSortedList(false, sortedShortIds);
            var newSortedShortIdsIndex = sortedShortIds.IndexOf(shortId);

            expressionBackup = expression;
            splitterBackup = splitter;
            trimValuesBackup = trimValues;
            columnNameBackup = colName;

            if (oldSortedShortIdsIndex == -1 || !shortIdsExprs.TryGetValue(shortId, out var exprs)) //New column, even not considering expressions, OR replaced (and removed above) LAST expression
            {
                shortIdsExprs.Add(shortId, new List<string> { expression });
                shortIdsColNames.Add(shortId, new List<string> { colName });

                tagsDataGridView.Rows.Insert(newSortedShortIdsIndex, 1);
                tagsDataGridView.Rows[newSortedShortIdsIndex].Cells[0].Value = ColumnCheckedState;
                tagsDataGridView.Rows[newSortedShortIdsIndex].Cells[0].Tag = shortId;

                if (selectedPreset?.userPreset == true)
                    tagsDataGridView.Rows[newSortedShortIdsIndex].Cells[0].ToolTipText = TagsDataGridViewCheckBoxToolTip;

                tagsDataGridView.Rows[newSortedShortIdsIndex].Cells[1].Value = LrFunctionNames[(int)columnAttributes.functionType];
                tagsDataGridView.Rows[newSortedShortIdsIndex].Cells[1].Tag = columnAttributes;
                tagsDataGridView.Rows[newSortedShortIdsIndex].Cells[2].Value = columnAttributes.parameterName;

                if (columnAttributes.functionType == LrFunctionType.Grouping)
                    tagsDataGridView.Rows[newSortedShortIdsIndex].Cells[3].Value = GetSplitterRepresentation(columnAttributes.splitter, columnAttributes.trimValues, false);
                else if (columnAttributes.parameter2Name != null)
                    tagsDataGridView.Rows[newSortedShortIdsIndex].Cells[3].Value = columnAttributes.parameter2Name;

                if (!presetIsLoading) //Interactive
                {
                    selectRow(tagsDataGridView, newSortedShortIdsIndex); //-V3106
                    fillExpressionsDataGridView(shortId, true); //New expression will be added from shortIdsExprs
                }
                else
                {
                    deselectAllRows(tagsDataGridView);
                    if (expressionsDataGridView.RowCount > 0)
                        expressionsDataGridView.RowCount = 0;
                }
            }
            else //Updating existing column (which may not be selected/current)
            {
                tagsDataGridView.Rows[oldSortedShortIdsIndex].Cells[1].Tag = columnAttributes;
                tagsDataGridView.Rows[oldSortedShortIdsIndex].Cells[2].Value = columnAttributes.parameterName;
                if (columnAttributes.functionType == LrFunctionType.Grouping)
                    tagsDataGridView.Rows[newSortedShortIdsIndex].Cells[3].Value = GetSplitterRepresentation(columnAttributes.splitter, columnAttributes.trimValues, false);

                if (oldSortedShortIdsIndex != tagsDataGridViewSelectedRow) //Updating tag column which is not current
                    selectRow(tagsDataGridView, oldSortedShortIdsIndex);

                var names = shortIdsColNames[shortId];

                if (oldExpressionIndex == -1) //It's new expression, let's add it to the end of list
                {
                    exprs.Add(expression);
                    names.Add(colName);
                }
                else //It's replaced expression (already removed above, but it wasn't last one) 
                {
                    exprs.Insert(oldExpressionIndex, expression);
                    names.Insert(oldExpressionIndex, colName);
                }

                if (!presetIsLoading) //Interactive
                    fillExpressionsDataGridView(shortId, null); //New expression will be added from shortIdsExprs
            }

            return true;
        }

        private void updateColumns(string shortId, string fieldName, string parameter2Name, LrFunctionType type, string splitter, bool trimValues, string expression, string columnName)
        {
            var columnAttributes = new ColumnAttributes(type, expression, columnName, fieldName, parameter2Name, splitter, trimValues);

            for (var i = 0; i < previewTable.ColumnCount; i++)
            {
                var colUniqueId = previewTable.Columns[i].HeaderCell.Tag as string;

                ColumnAttributesDict currentDict;
                if (type == LrFunctionType.Grouping)
                    currentDict = groupingsDict;
                else
                    currentDict = functionsDict;

                //If the current column doesn't correspond to the type of currentDict, let's skip it
                if (!currentDict.TryGetValue(colUniqueId, out var currentAttributes))
                    continue;


                if (shortId == currentAttributes.getShortId())
                {
                    if (currentAttributes.expression == expressionBackup) //Let's replace it
                    {
                        var exprIndex = shortIdsExprs[shortId].IndexOf(expressionBackup);
                        if (exprIndex != -1)
                        {
                            shortIdsExprs[shortId][exprIndex] = expression;
                            shortIdsColNames[shortId][exprIndex] = columnName;
                        }
                    }
                    else //Let's keep it as is
                    {
                        continue;
                    }

                    var newAttributes = new ColumnAttributes(type, expression, columnName, fieldName, parameter2Name, splitter, trimValues);

                    currentDict.Remove(colUniqueId);
                    currentDict.Add(newAttributes.getUniqueId(), newAttributes);

                    var newColumnName = GetColumnName(fieldName, parameter2Name, type, splitter, trimValues, expression, columnName, true, true, true);

                    previewTable.Columns[i].HeaderText = newColumnName;
                    previewTable.Columns[i].HeaderCell.Tag = newAttributes.getUniqueId();

                    var tagIndex = getSelectedRow(tagsDataGridView);
                    tagsDataGridView.Rows[tagIndex].Cells[1].Tag = columnAttributes;
                    if (type == LrFunctionType.Grouping)
                        tagsDataGridView.Rows[tagIndex].Cells[3].Value = GetSplitterRepresentation(splitter, trimValues, false);

                    for (var j = 0; j < expressionsDataGridView.RowCount; j++)
                    {
                        if (expressionsDataGridView.Rows[j].Cells[1].Tag as string == expressionBackup)
                        {
                            expressionsDataGridView.Rows[j].Cells[1].Value = (string.IsNullOrEmpty(expression) ? NoExpressionText : expression);
                            expressionsDataGridView.Rows[j].Cells[1].Tag = expression;

                            expressionsDataGridView.Rows[j].Cells[2].Value = (string.IsNullOrEmpty(columnName) ? AutoColumnNameText : columnName);
                            expressionsDataGridView.Rows[j].Cells[2].Tag = columnName;

                            break;
                        }
                    }
                }
            }
        }

        private int removeColumn(string fieldName, string parameter2Name, LrFunctionType type, string splitter, bool trimValues, string expression, string columnName)
        {
            var fullFieldName = GetColumnName(fieldName, parameter2Name, type, splitter, trimValues, expression, columnName, false, false, true);

            var columnAttributes = new ColumnAttributes(type, expression, columnName, fieldName, parameter2Name, splitter, trimValues);

            var uniqueId = columnAttributes.getUniqueId();
            var shortId = columnAttributes.getShortId();

            var columnIndex = -1;
            for (var i = 0; i < previewTable.ColumnCount; i++)
            {
                var colUniqueId = previewTable.Columns[i].HeaderCell.Tag as string;

                if (colUniqueId == uniqueId)
                {
                    columnIndex = i;
                    break;
                }
            }

            if (type == LrFunctionType.Grouping)
            {
                groupingsDict.Remove(uniqueId);
            }
            else
            {
                var index = functionsDict.indexOf(uniqueId);

                functionsDict.RemoveUi(uniqueId, buttonOK, assignHotkeyCheckBox);

                var currentSourceFieldComboBoxSelectedIndex = sourceFieldComboBoxCustom.SelectedIndex;

                savedDestinationTagsNames.RemoveAt(index); //-V3057
                savedFunctionIds.RemoveAt(index); //-V3057

                operations.RemoveAt(index); //-V3057
                mulDivFactors.RemoveAt(index); //-V3057
                precisionDigits.RemoveAt(index); //-V3057
                appendTexts.RemoveAt(index); //-V3057

                sourceFieldComboBoxCustom.RemoveAt(index);
                if (sourceFieldComboBoxCustom.SelectedIndex == -1 && sourceFieldComboBoxCustom.Items.Count > 0)
                    sourceFieldComboBoxCustom.SelectedIndex = 0;
                else if (sourceFieldComboBoxCustom.SelectedIndex == -1)
                    sourceFieldComboBox_SelectedIndexChanged(null, null);
            }


            conditionFieldListCustom.Remove(fullFieldName);
            if (conditionFieldListCustom.SelectedIndex == -1 && conditionFieldListCustom.Items.Count > 0)
                conditionFieldListCustom.SelectedIndex = 0;


            if (comparedFieldListCustom.Text == comparedFieldListCustom.SelectedItem as string)
                comparedFieldListCustom.Text = string.Empty;

            comparedFieldListCustom.Remove(fullFieldName);

            if (conditionFieldListCustom.Items.Count == 0)
            {
                conditionCheckBox.Enable(false);
                conditionCheckBox.Checked = false;
            }


            previewTable.Columns.RemoveAt(columnIndex);


            //Now let's deal with field & expression tables
            var sortedShortIdsIndex = sortedShortIds.IndexOf(shortId);

            var exprs = shortIdsExprs[shortId];
            exprs.Remove(expression);
            var names = shortIdsColNames[shortId];
            names.Remove(columnName);

            if (exprs.Count == 0)
            {
                shortIdsExprs.Remove(shortId);
                sortedShortIds.Remove(shortId);

                shortIdsColNames.Remove(shortId);//===

                tagsDataGridView.Rows.RemoveAt(sortedShortIdsIndex);

                string newShortId = null;
                tagsDataGridViewSelectedRow = getSelectedRow(tagsDataGridView);
                if (tagsDataGridViewSelectedRow != -1)
                    newShortId = tagsDataGridView.Rows[tagsDataGridViewSelectedRow].Cells[0].Tag as string;

                fillExpressionsDataGridView(newShortId, false);
            }
            else
            {
                fillExpressionsDataGridView(shortId, false);
            }


            //if (previewTable.ColumnCount == 0) //--- Actually not needed because changing preset definition is disabled, when preview is generated
            //    clickOnPreviewButton(prepareBackgroundPreview, previewReport, buttonPreview, buttonExport, buttonClose);

            if (!presetIsLoading)
                setPresetChanged();

            return columnIndex;
        }

        private ReportPreset[] getReportPresetsArrayUI()
        {
            var reportPresetsLocal = new ReportPreset[presetList.Items.Count];

            if (reportPresetsLocal.Length > 0)
                presetList.Items.CopyTo(reportPresetsLocal, 0);

            return reportPresetsLocal;
        }

        private void formatTrackColumns()
        {
            if (checkStoppingStatus())
                return;


            (columnTypes, ColumnsRightAlignment) = getColumnTypesRightAlignment();

            for (int i = 0; i < previewTable.ColumnCount; i++)
            {
                previewTable.Columns[i].MinimumWidth = PreviewTableColumnMinimumWidth;
                previewTable.Columns[i].Resizable = DataGridViewTriState.True;
                previewTable.Columns[i].ReadOnly = true;
                previewTable.Columns[i].HeaderCell.Style.WrapMode = DataGridViewTriState.True;

                if (i == artworkField)
                {
                    previewTable.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    previewTable.Columns[i].MinimumWidth = previewTableArtworkSize;
                    previewTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    previewTable.Columns[i].DefaultCellStyle.WrapMode = DataGridViewTriState.False;

                    if (resizeArtworkCheckBox.Checked)
                    {
                        previewTable.Columns[artworkField].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        previewTable.Columns[i].Width = previewTableArtworkSize;
                    }
                    else
                    {
                        previewTable.Columns[artworkField].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
                    }
                }
                else if (ColumnsRightAlignment[i])
                {
                    previewTable.Columns[i].SortMode = DataGridViewColumnSortMode.Programmatic;
                    previewTable.Columns[i].MinimumWidth = PreviewTableColumnMinimumWidth;
                    previewTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                    previewTable.Columns[i].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                }
                else
                {
                    previewTable.Columns[i].SortMode = DataGridViewColumnSortMode.Programmatic;
                    previewTable.Columns[i].MinimumWidth = PreviewTableColumnMinimumWidth;
                    previewTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                    previewTable.Columns[i].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                }
            }

            previewTable.AutoResizeColumnHeadersHeight();
        }

        private void resetPreviewData()
        {
            previewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            previewTable.AllowUserToResizeColumns = false;
            previewTable.AllowUserToResizeRows = false;
            foreach (DataGridViewColumn column in previewTable.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;


            previewIsGenerated = false;
            backgroundTaskIsStopping = false;
            backgroundTaskIsStoppedOrCancelled = false;

            updateCustomScrollBars(previewTable);
            SetStatusBarText(this, string.Empty, false);

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
            SetStatusBarText(this, SbLrResizingPreviewTable, false);
            previewTable.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);

            previewTable.AllowUserToResizeColumns = true;
            previewTable.AllowUserToResizeRows = true;
            for (int i = 0; i < previewTable.ColumnCount; i++)
            {
                previewTable.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;

                if (previewTable.Columns[i].Width > PreviewTableColumnMaximumWidth)
                    previewTable.Columns[i].Width = PreviewTableColumnMaximumWidth;
            }

            for (int i = 0; i < previewTable.RowCount; i++)
                previewTable.Rows[i].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            previewTable.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);


            backgroundTaskIsScheduled = false;
            backgroundTaskIsStopping = false;
            backgroundTaskIsStoppedOrCancelled = false;

            enableDisablePreviewOptionControls(true);
            enableQueryingOrUpdatingButtons();

            if (previewTable.RowCount > 0 && previewTable.Columns.GetColumnCount(DataGridViewElementStates.Visible) > 0)
            {
                previewTable.CurrentCell = previewTable.Rows[0].Cells[0];
                previewTable.FirstDisplayedCell = previewTable.CurrentCell;
            }

            updateCustomScrollBars(previewTable);

            SetStatusBarText(this, string.Empty, false);
            //SetResultingSbText(this);

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
            foreach (DataGridViewColumn column in previewTable.Columns)
                column.SortMode = DataGridViewColumnSortMode.Automatic;


            backgroundTaskIsScheduled = false;
            backgroundTaskIsStopping = false;
            backgroundTaskIsStoppedOrCancelled = false;
            closeFormOnStopping = false;

            SetResultingSbText(this);

            if (closeFormOnStopping)
            {
                ignoreClosingForm = false;
                Close();
            }

            ignoreClosingForm = false;

            return true;
        }

        private bool prepareBackgroundPreview()
        {
            if (backgroundTaskIsStopping)
                backgroundTaskIsStoppedOrCancelled = true;

            if (previewIsGenerated)
            {
                rows?.Clear();
                source.ResetBindings(false);

                previewIsGenerated = false;
                enableDisablePreviewOptionControls(true);
                return true;
            }

            previewIsGenerated = false;

            if (backgroundTaskIsWorking())
                return true;

            resetPreviewData();


            if (!presetIsLoading && previewTable.RowCount == 0)
                presetTabControl.SelectedTab = tabPage1;


            smartOperation = smartOperationCheckBox.Checked;

            resultsAreFiltered = false;
            buttonFilterResultsChangeLabel();

            if (previewTable.Columns.GetColumnCount(DataGridViewElementStates.Visible) == 0)
            {
                //MessageBox.Show(this, plugin.msgNoTagsSelected);
                return false;
            }


            previewTable.AllowUserToResizeColumns = false;
            previewTable.AllowUserToResizeRows = false;
            foreach (DataGridViewColumn column in previewTable.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            previewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;


            reportPresets = getReportPresetsArrayUI();
            appliedPreset = selectedPreset;
            backgroundTaskIsUpdatingTags = false;

            return true;
        }

        private void previewReport()
        {
            previewIsGenerated = true;

            MbApiInterface.Library_QueryFilesEx("domain=DisplayedFiles", out var files);
            if (files == null || files.Length == 0)
            {
                SetStatusBarText(this, MsgNoTracksInCurrentView, false);
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }

            executePreset(this, files, true, false, null, false, false);

            CachedPresetsTags.TryGetValue(appliedPreset.guid, out previewedTags);
            CachedPresetsFilesActualComposedSplitGroupingTagsList.TryGetValue(appliedPreset.guid, out previewedFilesActualComposedSplitGroupingTagsList);
        }

        private void exportTrackList(bool openReport)
        {
            const int CdBookletArtworkSize = 550; //Height and width of CD booklet artwork
            string fileDirectoryPath;
            string reportFullFileName;
            string reportOnlyFileName;

            ExportedDocument document;
            int seqNumField = -1;
            int albumArtistField = -1;
            int albumField = -1;
            int titleField = -1;
            int durationField = -1;
            int urlField = -1;

            bool htmlExport = false;

            backgroundTaskIsStopping = false;

            var j = -1;
            foreach (var attribs in groupingsDict.Values)
            {
                j++;

                if (attribs.getColumnName(false, false, false) == DisplayedAlbumArtistName)
                    albumArtistField = j;
                else if (attribs.getColumnName(false, false, false) == MbApiInterface.Setting_GetFieldName(MetaDataType.Album))
                    albumField = j;
                else if (attribs.getColumnName(false, false, false) == SequenceNumberName)
                    seqNumField = j;
                else if (attribs.getColumnName(false, false, false) == MbApiInterface.Setting_GetFieldName(MetaDataType.TrackTitle))
                    titleField = j;
                else if (attribs.getColumnName(false, false, false) == MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Duration))
                    durationField = j;
                else if (attribs.getColumnName(false, false, false) == MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url))
                    urlField = j;
            }


            if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentByAlbums && (albumArtistField != 0 || albumField != 1 || artworkField != 2))
            {
                MessageBox.Show(this, MsgFirstThreeGroupingFieldsInPreviewTableShouldBe,
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (selectedPreset.fileFormatIndex == LrReportFormat.M3u && urlField == -1)
            {
                MessageBox.Show(this, MsgUrlFieldRequired,
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentCdBooklet && (seqNumField != 0 || albumArtistField != 1 || albumField != 2
            || artworkField != 3 || titleField != 4 || durationField != 5))
            {
                MessageBox.Show(this, MsgFirstSixGroupingFieldsInPreviewTableShouldBe,
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentAlbumGrid && (albumField != 0
            || albumArtistField != 1 || artworkField != 2))
            {
                MessageBox.Show(this, MsgFirstThreeGroupingFieldsInPreviewTableShouldBe2,
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentAlbumGrid && (!selectedPreset.resizeArtwork || selectedPreset.newArtworkSize < 60))
            {
                MessageBox.Show(this, MsgResizingArtworksRequired,
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            if (openReport)
            {
                fileDirectoryPath = System.IO.Path.GetTempPath();
            }
            else
            {
                if (SavedSettings.exportedLastFolder == null)
                    SavedSettings.exportedLastFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";

                fileDirectoryPath = SavedSettings.exportedLastFolder;
            }

            if (!openReport)
            {
                var dialog = new SaveFileDialog
                {
                    InitialDirectory = fileDirectoryPath,
                    FileName = selectedPreset.exportedTrackListName,
                    Filter = ExportedFormats.Split('|')[((int)selectedPreset.fileFormatIndex - 1) * 2]
                        + "|" + ExportedFormats.Split('|')[((int)selectedPreset.fileFormatIndex - 1) * 2 + 1],
                    FilterIndex = 1
                };

                if (dialog.ShowDialog(this) == DialogResult.Cancel) return;

                reportFullFileName = dialog.FileName;

                fileDirectoryPath = Regex.Replace(dialog.FileName, @"^(.*\\).*\..*", "$1"); //File directory path including ending \
                reportOnlyFileName = Regex.Replace(dialog.FileName, @"^.*\\(.*)\..*", "$1"); //Filename without path to file and extension

                dialog.Dispose();

                selectedPreset.exportedTrackListName = reportOnlyFileName;

                if (SavedSettings.reportPresets != null)
                {
                    foreach (var preset in SavedSettings.reportPresets)
                    {
                        if (preset.permanentGuid == selectedPreset.permanentGuid)
                        {
                            preset.exportedTrackListName = reportOnlyFileName;
                            break;
                        }
                    }
                }

                SavedSettings.exportedLastFolder = fileDirectoryPath;
            }
            else
            {
                reportOnlyFileName = selectedPreset.exportedTrackListName;
                reportFullFileName = fileDirectoryPath + reportOnlyFileName + ExportedFormats
                    .Split('|')[((int)selectedPreset.fileFormatIndex - 1) * 2 + 1].Substring(1);
            }

            var imagesDirectoryName = reportOnlyFileName + ".files";


            if (!checkPreview(rows))
                return;


            //Album artists/Albums for HTML grouped by albums, Album artists/sequence number of album artist for CD Booklets [Albums]
            var albumArtistsAlbums = new SortedDictionary<string, List<string>>();
            string artworkBase64 = null;
            var albumTrackCounts = new List<int>();
            Bitmap resizedCdBookletArtwork = null; //For CD booklet export


            var stream = new System.IO.FileStream(reportFullFileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);

            try
            {
                if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocument || selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentCdBooklet
                    || selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentAlbumGrid)
                {
                    if (System.IO.Directory.Exists(fileDirectoryPath + imagesDirectoryName))
                        System.IO.Directory.Delete(fileDirectoryPath + imagesDirectoryName, true);

                    System.IO.Directory.CreateDirectory(fileDirectoryPath + imagesDirectoryName);
                }
                else if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlTable)
                {
                    if (System.IO.Directory.Exists(fileDirectoryPath + imagesDirectoryName))
                        System.IO.Directory.Delete(fileDirectoryPath + imagesDirectoryName, true);

                    if (artworkField != -1)
                        System.IO.Directory.CreateDirectory(fileDirectoryPath + imagesDirectoryName);
                }


                switch (selectedPreset.fileFormatIndex)
                {
                    case LrReportFormat.HtmlDocumentByAlbums:
                    case LrReportFormat.HtmlDocumentCdBooklet:
                        htmlExport = true;
                        List<string> currentAlbumArtistAlbums;
                        string prevAlbum1 = null;
                        string prevAlbumArtist1 = null;
                        var albumArtistsAlbumsSequenceNumber = 0;

                        var trackCount = 0;
                        for (int k = 0; k < rows.Count; k++)
                        {
                            List<object> groupingsValues = rows[k].Columns;


                            if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentCdBooklet)
                            {
                                Bitmap artwork = groupingsValues[artworkField] as Bitmap;
                                string currentArtworkBase64 = rows[k].artworkBase64Hash;

                                if (artworkBase64 == null)
                                    artworkBase64 = currentArtworkBase64;
                                else if (artworkBase64 == string.Empty)
                                    ; //Nothing...
                                else if (artworkBase64 != currentArtworkBase64)
                                    artworkBase64 = string.Empty;
                            }


                            if (prevAlbumArtist1 != groupingsValues[albumArtistField] as string || prevAlbum1 != groupingsValues[albumField] as string) //-V3106
                            {
                                if (prevAlbumArtist1 != groupingsValues[albumArtistField] as string) //-V3106
                                {
                                    if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentCdBooklet)
                                        albumArtistsAlbumsSequenceNumber++;

                                    prevAlbumArtist1 = groupingsValues[albumArtistField] as string; //-V3106
                                    currentAlbumArtistAlbums = new List<string>();

                                    albumArtistsAlbums.Add(prevAlbumArtist1 + albumArtistsAlbumsSequenceNumber.ToString("D4"), currentAlbumArtistAlbums);
                                }
                                else
                                {
                                    currentAlbumArtistAlbums = albumArtistsAlbums[prevAlbumArtist1 + albumArtistsAlbumsSequenceNumber.ToString("D4")];
                                }

                                prevAlbum1 = groupingsValues[albumField] as string; //-V3106

                                currentAlbumArtistAlbums.Add(prevAlbum1);
                                albumTrackCounts.Add(trackCount);
                                trackCount = 0;
                            }

                            trackCount++;
                        }

                        albumTrackCounts.Add(trackCount);


                        if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentByAlbums)
                            document = new HtmlDocumentByAlbum(stream, TagToolsPlugin, selectedPreset.getName(), fileDirectoryPath, imagesDirectoryName);
                        else //if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentCdBooklet)
                            document = new HtmlDocumentCDBooklet(stream, TagToolsPlugin, selectedPreset.getName(), fileDirectoryPath, imagesDirectoryName);
                        break;
                    case LrReportFormat.HtmlDocument:
                        htmlExport = true;
                        document = new HtmlDocument(stream, TagToolsPlugin, selectedPreset.getName(), fileDirectoryPath, imagesDirectoryName);
                        break;
                    case LrReportFormat.HtmlTable:
                        htmlExport = true;
                        document = new HtmlTable(stream, TagToolsPlugin, selectedPreset.getName(), fileDirectoryPath, imagesDirectoryName);
                        break;
                    case LrReportFormat.TabSeparatedText:
                        document = new TextDocument(stream, TagToolsPlugin, selectedPreset.getName());
                        break;
                    case LrReportFormat.M3u:
                        document = new M3UDocument(stream, TagToolsPlugin, selectedPreset.getName());
                        break;
                    case LrReportFormat.Csv:
                        document = new CsvDocument(stream, TagToolsPlugin, selectedPreset.getName());
                        break;
                    case LrReportFormat.HtmlDocumentAlbumGrid:
                        htmlExport = true;
                        document = new HtmlDocumentAlbumGrid(stream, TagToolsPlugin, selectedPreset.getName(), fileDirectoryPath, imagesDirectoryName);
                        break;
                    default:
                        return;
                }
            }
            catch (Exception ex)
            {
                stream.Close();
                MessageBox.Show(this, ex.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            const int artworkGridHtmlWidth = 740;
            const int artworkGridBordersWidth = 30;
            int artworkGridMaxImageSize = (int)newArtworkSizeUpDown.Value;
            int artworkGridRowImageCount = (int)((float)artworkGridHtmlWidth / (artworkGridMaxImageSize + artworkGridBordersWidth));

            if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentAlbumGrid) //It's special case because every entry in tags dictionary is a separate album
            {
                var fontSize = 12; //Font size units are pixels
                var labelHeight = (int)(fontSize * 2.6f * 125 / artworkGridMaxImageSize);

                if (labelHeight < (int)(fontSize * 1.3f))
                    labelHeight = (int)(fontSize * 1.3f);

                const string color = "#000000"; //---

                (document as HtmlDocumentAlbumGrid).writeHeader(color, artworkGridRowImageCount, artworkGridMaxImageSize, artworkGridMaxImageSize, labelHeight, fontSize);
            }
            else if (selectedPreset.fileFormatIndex != LrReportFormat.HtmlDocumentCdBooklet)
            {
                document.writeHeader();
            }
            else //It's CD booklet
            {
                Bitmap pic;

                if (string.IsNullOrEmpty(CdBookletArtworkBase64)) //There are various artworks or no artwork
                    pic = DefaultArtwork;
                else //All tracks have the same artwork
                    pic = CdBookletArtwork;

                var xSF = CdBookletArtworkSize / (float)pic.Width;
                var ySF = CdBookletArtworkSize / (float)pic.Height;
                float SF;

                if (xSF >= ySF)
                    SF = ySF;
                else
                    SF = xSF;


                resizedCdBookletArtwork = new Bitmap((int)Math.Round(pic.Width * SF), (int)Math.Round(pic.Height * SF), System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                var gr_dest = Graphics.FromImage(resizedCdBookletArtwork);
                gr_dest.DrawImage(pic, 0, 0, resizedCdBookletArtwork.Width, resizedCdBookletArtwork.Height);
                gr_dest.Dispose();

                (document as HtmlDocumentCDBooklet).writeHeader(CdBookletArtworkSize, GetBitmapAverageColor(resizedCdBookletArtwork), resizedCdBookletArtwork,
                    albumArtistsAlbums, rows.Count);

                resizedCdBookletArtwork.Dispose();
            }


            if (selectedPreset.fileFormatIndex != LrReportFormat.M3u && selectedPreset.fileFormatIndex != LrReportFormat.HtmlDocumentCdBooklet
                && selectedPreset.fileFormatIndex != LrReportFormat.HtmlDocumentAlbumGrid) //Let's write table headers
            {
                var k = -1;
                foreach (var attribs in groupingsDict.Values)
                {
                    k++;
                    document.addCellToRow(attribs.getColumnName(true, true, true), attribs.getColumnName(true, true, true), false,
                        k == albumArtistField, k == albumField);
                }

                foreach (var attribs in functionsDict.Values)
                {
                    document.addCellToRow(attribs.getColumnName(true, true, true), attribs.getColumnName(true, true, true), false,
                        false, false);
                }

                document.writeRow(0);
            }


            var multipleAlbumArtists = false;
            var multipleAlbums = false;
            if (albumArtistsAlbums.Count == 1) //1 album artist
            {
                foreach (var albumArtistAlbums in albumArtistsAlbums)
                {
                    if (albumArtistAlbums.Value.Count > 1) //Several albums
                    {
                        multipleAlbums = true;
                        break;
                    }
                }
            }
            else //Several album artists
            {
                multipleAlbumArtists = true;
            }


            int rowHeight = 0;
            int i = 0;

            string prevAlbum = null;
            string prevAlbumArtist = null;


            int groupingCount = 0;
            for (int k = 0; k < rows.Count; k++)
            {
                object[] row = new object[rows[k].Columns.Count];
                rows[k].Columns.CopyTo(row);

                if (htmlExport)
                    for (int q = 0; q < row.Length; q++)
                        if (q != artworkField)
                            row[q] = System.Net.WebUtility.HtmlEncode(row[q] as string);


                if (checkCondition(row))
                {
                    groupingCount++;

                    if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentByAlbums)
                    {
                        if (prevAlbumArtist != row[albumArtistField] as string) //-V3106
                        {
                            i++;
                            prevAlbumArtist = row[albumArtistField] as string; //-V3106
                            prevAlbum = row[albumField] as string; //-V3106

                            Bitmap currentArtwork = row[artworkField] as Bitmap;
                            string currentArtworkBase64Hash = rows[k].artworkBase64Hash;

                            document.beginAlbumArtist(row[albumArtistField] as string, row.Length); //-V3106
                            document.beginAlbum(row[albumField] as string, currentArtwork, currentArtworkBase64Hash, albumTrackCounts[i]); //-V3106
                        }
                        else if (prevAlbum != row[albumField] as string) //-V3106
                        {
                            i++;
                            prevAlbum = row[albumField] as string; //-V3106

                            Bitmap currentArtwork = row[artworkField] as Bitmap;
                            string currentArtworkBase64Hash = rows[k].artworkBase64Hash;

                            document.beginAlbum(row[albumField] as string, currentArtwork, currentArtworkBase64Hash, albumTrackCounts[i]); //-V3106
                        }
                    }


                    if (selectedPreset.fileFormatIndex == LrReportFormat.M3u) //It's M3U playlist
                    {
                        document.addCellToRow(row[urlField] as string, UrlTagName, false, false, false); //-V3106
                        document.writeRow(0);
                    }
                    else if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentAlbumGrid) //Album grid
                    {
                        string albumLabel = row[0] as string;

                        Bitmap currentArtwork = row[artworkField] as Bitmap;
                        string currentArtworkBase64Hash = rows[k].artworkBase64Hash;


                        for (var l = 3; l < row.Length; l++) //groupingsValues: 1st value MUST BE album name, 2nd value MUST BE artwork
                            albumLabel += "/" + row[l];

                        (document as HtmlDocumentAlbumGrid).addCellToRow(currentArtwork, albumLabel, currentArtworkBase64Hash);
                    }
                    else if (selectedPreset.fileFormatIndex != LrReportFormat.HtmlDocumentCdBooklet) //It's NOT a CD booklet
                    {
                        var l = -1;
                        foreach (var attribs in groupingsDict.Values)
                        {
                            l++;

                            if (l == artworkField && (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentByAlbums
                                || selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocument
                                || selectedPreset.fileFormatIndex == LrReportFormat.HtmlTable)) //Export images
                            {
                                Bitmap currentArtwork = row[artworkField] as Bitmap;
                                string currentArtworkBase64Hash = rows[k].artworkBase64Hash;

                                rowHeight = CdBookletArtwork.Height;

                                document.addCellToRow(currentArtwork, attribs.getColumnName(true, true, true), currentArtworkBase64Hash, CdBookletArtwork.Width, CdBookletArtwork.Height);
                            }
                            else if (l == artworkField && selectedPreset.fileFormatIndex != LrReportFormat.HtmlDocumentByAlbums
                                && selectedPreset.fileFormatIndex != LrReportFormat.HtmlDocument
                                && selectedPreset.fileFormatIndex != LrReportFormat.HtmlTable) //Export image base64es
                            {
                                Bitmap currentArtwork = row[artworkField] as Bitmap;
                                string currentArtworkBase64Hash = rows[k].artworkBase64Hash;

                                document.addCellToRow(currentArtworkBase64Hash, attribs.getColumnName(true, true, true), ColumnsRightAlignment[l],
                                    l == albumArtistField, l == albumField);
                            }
                            else //It's not the artwork column
                            {
                                document.addCellToRow(row[l] as string, attribs.getColumnName(true, true, true), ColumnsRightAlignment[l],
                                    l == albumArtistField, l == albumField);
                            }
                        }

                        l = -1;
                        foreach (var attribs in functionsDict.Values)
                        {
                            l++;

                            document.addCellToRow(row[groupingsDict.Count + l] as string, attribs.getColumnName(true, true, true),
                                ColumnsRightAlignment[groupingsDict.Count + l], false, false);
                        }
                    }
                    else //It's a CD booklet
                    {
                        if (!multipleAlbumArtists) //1 album artist
                        {
                            if (!multipleAlbums) //1 album
                            {
                                (document as HtmlDocumentCDBooklet).addTrack(row[seqNumField] as string, //-V3106
                                    null, null, row[titleField] as string, row[durationField] as string); //-V3106
                            }
                            else //Several albums
                            {
                                (document as HtmlDocumentCDBooklet).addTrack(row[seqNumField] as string, //-V3106
                                    null, row[albumField] as string, row[titleField] as string, row[durationField] as string); //-V3080 //-V3106
                            }
                        }
                        else //Several album artists
                        {
                            (document as HtmlDocumentCDBooklet).addTrack(row[seqNumField] as string, //-V3106
                                row[albumArtistField] as string, row[albumField] as string, row[titleField] as string, //-V3106
                                row[durationField] as string); //-V3106
                        }
                    }


                    if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentAlbumGrid)
                    {
                        if (groupingCount + 1 > artworkGridRowImageCount)
                        {
                            document.writeRow(0);
                            groupingCount = 0;
                        }
                    }
                    else if (selectedPreset.fileFormatIndex != LrReportFormat.M3u) //It's NOT M3U playlist
                    {
                        document.writeRow(rowHeight);
                    }

                    rowHeight = 0;
                }
            }

            if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentCdBooklet) //It's a CD booklet
                (document as HtmlDocumentCDBooklet).writeFooter(albumArtistsAlbums);
            else if (selectedPreset.fileFormatIndex != LrReportFormat.M3u) //It's NOT a M3U playlist
                document.writeFooter();

            document.close();

            if (openReportCheckBox.Checked)
            {
                var waiter = new Thread(OpenReport);
                waiter.Start(reportFullFileName);
            }
        }

        internal static void OpenReport(object document)
        {
            var documentPathFileName = document as string;

            System.Diagnostics.Process.Start(documentPathFileName);

            return; //Code below does not work reliably

            if (ProcessedReportDeletions.Contains(documentPathFileName)) //-V3142
                return;

            ProcessedReportDeletions.Add(documentPathFileName);

            const int waitingTimeout = 5; //In milliseconds

            var retryCount = 0;
            var reportHasBeenLocked = false;
            while (!reportHasBeenLocked && retryCount < 30000 / waitingTimeout)
            {
                try
                {
                    using (var stream = new System.IO.FileStream(documentPathFileName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None)) //-V3156
                        ; //Nothing to do, just checking if file is read by external application. It is not. Let's retry.

                    retryCount++;
                    Thread.Sleep(waitingTimeout);
                }
                catch (System.IO.IOException)
                {
                    reportHasBeenLocked = true; //File is read by external application. Now let's wait, when it is unlocked.
                }
            }


            retryCount = 0;
            while (reportHasBeenLocked && retryCount < 30000 / waitingTimeout)
            {
                try
                {
                    using (var stream = new System.IO.FileStream(documentPathFileName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None)) //-V3156
                        reportHasBeenLocked = false; //File is unlocked by external application. Let's try to delete it now.
                }
                catch (System.IO.IOException)
                {
                    //Nothing to do, just checking if file is read by external application. It is. Let's retry.
                    retryCount++;
                    Thread.Sleep(2000);
                }
            }


            retryCount = 0;
            var retry = true;
            while (retry && retryCount < 30000 / waitingTimeout)
            {
                try
                {
                    System.IO.File.Delete(documentPathFileName);
                    retry = false;
                }
                catch (System.IO.IOException)
                {
                    retryCount++;
                    Thread.Sleep(2000);
                }
            }

            var documentExtension = Regex.Replace(documentPathFileName, @"^.*(\..*)", "$1"); //-V3156
            if (retry || documentExtension != ".htm")
            {
                ProcessedReportDeletions.Remove(documentPathFileName);
                return;
            }

            Thread.Sleep(10000);

            ProcessedReportDeletions.Remove(documentPathFileName);
            var imagesDirectoryPath = Regex.Replace(documentPathFileName, @"^(.*)\..*", "$1") + ".files"; //-V3156
            if (System.IO.Directory.Exists(imagesDirectoryPath))
                System.IO.Directory.Delete(imagesDirectoryPath, true);
        }

        private bool checkPreview(DataGridViewBoundColumnList<Row> rows)
        {
            if (backgroundTaskIsWorking())
                return false;

            if (rows.Count == 0)
            {
                MessageBox.Show(this, MsgPreviewIsNotGeneratedNothingToSave, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        private static bool IsTagEmpty(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
                return true;
            else if (tagName == NullTagName)
                return true;
            else
                return false;
        }

        private bool saveSettings()
        {
            if (selectedPreset != null && selectedPreset.useAnotherPresetAsSource && selectedPreset.anotherPresetAsSource.permanentGuid == Guid.Empty)
            {
                selectedPreset.useAnotherPresetAsSource = false;
                setPresetChanged();
            }


            var newOrChangedCachedPresets = new List<ReportPreset>();
            var allCurrentPresets = new ReportPreset[PresetsInteractiveWorkingCopy.Count];

            if (allCurrentPresets.Length > 0)
                PresetsInteractiveWorkingCopy.Values.CopyTo(allCurrentPresets, 0);

            foreach (ReportPreset changedPreset in PresetsInteractiveWorkingCopy.Values)
            {
                var isItNewPreset = true;

                lock (SavedSettings.reportPresets)
                {
                    foreach (var savedPreset in SavedSettings.reportPresets)
                    {
                        if (changedPreset.permanentGuid == savedPreset.permanentGuid)
                        {
                            isItNewPreset = false;

                            if (changedPreset.guid != savedPreset.guid)
                            {
                                for (var i = 0; i < changedPreset.functions.Length; i++)
                                {
                                    if (i < savedPreset.functionIds.Length)
                                    {
                                        if (string.IsNullOrWhiteSpace(changedPreset.functionIds[i]))
                                            ; //Caching not affected...
                                        else if (IsTagEmpty(changedPreset.destinationTags[i]))
                                            ; //Caching not affected...
                                        else if (savedPreset.functionIds[i] == changedPreset.functionIds[i] && savedPreset.destinationTags[i] == changedPreset.destinationTags[i])
                                            ; //Caching not affected...
                                        else
                                            newOrChangedCachedPresets.Add(changedPreset);
                                    }
                                    //Else new functions have been added. Let's check if precaching is required.
                                    else if (!string.IsNullOrWhiteSpace(changedPreset.functionIds[i]) && !IsTagEmpty(changedPreset.destinationTags[i]))
                                    {
                                        newOrChangedCachedPresets.Add(changedPreset);
                                    }
                                }
                            }

                            break;
                        }
                    }
                }

                if (isItNewPreset)
                {
                    for (var i = 0; i < changedPreset.functions.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(changedPreset.functionIds[i]) && !IsTagEmpty(changedPreset.destinationTags[i]))
                            newOrChangedCachedPresets.Add(changedPreset);
                    }
                }
            }


            var notEmptyPresetCount = 0;
            foreach (var preset in PresetsInteractiveWorkingCopy.Values)
            {
                if (preset.groupings.Length > 0 || preset.functions.Length > 0)
                {
                    preset.initialGuid = preset.guid;
                    notEmptyPresetCount++;
                }
            }

            lock (SavedSettings.reportPresets)
            {
                var presetCache = SavedSettings.reportPresets;

                var skippedPresetCount = 0;
                SavedSettings.reportPresets = new ReportPreset[notEmptyPresetCount];

                int i = -1;
                foreach (var preset in PresetsInteractiveWorkingCopy.Values)
                {
                    i++;

                    if (preset.groupings.Length > 0 || preset.functions.Length > 0)
                        SavedSettings.reportPresets[i - skippedPresetCount] = new ReportPreset(preset, true);
                    else
                        skippedPresetCount++;
                }
            }


            InitLr();


            SavedSettings.smartOperation = smartOperationCheckBox.Checked;
            SavedSettings.openReportAfterExporting = openReportCheckBox.Checked;

            TagToolsPlugin.addPluginContextMenuItems();
            TagToolsPlugin.addPluginMenuItems();

            setUnsavedChanges(false);
            TagToolsPlugin.SaveSettings();


            if (newOrChangedCachedPresets.Count > 0)
            {
                var result = MessageBox.Show(this, GetPluralForm(MsgLrCachedPresetsChanged, newOrChangedCachedPresets.Count)
                    .Replace("%%CHANGED-PRESET-COUNT%%", newOrChangedCachedPresets.Count.ToString()),
                    string.Empty, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                if (result == DialogResult.Cancel)
                    return false;
                else if (result == DialogResult.Yes)
                    using (var tagToolsForm = new LrFunctionPrecaching(TagToolsPlugin))
                        _ = tagToolsForm.precachePresets(allCurrentPresets, newOrChangedCachedPresets);
            }


            return true;
        }

        internal override void enableDisablePreviewOptionControls(bool enable, bool dontChangeDisabled = false)
        {
            buttonAddPreset.Enable(enable);
            //buttonSaveClose.Enable(enable && (!backgroundTaskIsUpdatingTags || !backgroundTaskIsWorking()));
            buttonSaveClose.Enable(enable);

            if (selectedPreset == null)
                enable = false;

            presetNameTextBox.Enable(enable);

            hidePreviewCheckBox.Enable(enable && !previewIsGenerated);

            buttonCopyPreset.Enable(enable);
            buttonDeletePreset.Enable(enable && selectedPreset.userPreset); //-V3125

            smartOperationCheckBox.Enable(enable && !previewIsGenerated);

            assignHotkeyCheckBox.Enable(enable);
            useHotkeyForSelectedTracksCheckBox.Enable(enable && assignHotkeyCheckBox.Checked && assignHotkeyCheckBox.IsEnabled());


            tagsDataGridView.Enable(enable && !previewIsGenerated && newColumn == false);
            expressionsDataGridView.Enable(enable && !previewIsGenerated && newColumn == false);

            sourceTagListCustom.Enable(enable && !previewIsGenerated && newColumn == true);
            buttonAddFunction.Enable(enable && !previewIsGenerated && selectedPreset.userPreset);
            buttonUpdateFunction.Enable(enable && !previewIsGenerated && newColumn != false);

            parameter2ComboBoxCustom.Enable(enable && !previewIsGenerated && newColumn == true);
            parameter2Label.Enable(enable && !previewIsGenerated && newColumn == true);
            forTagLabel.Enable(enable && !previewIsGenerated && newColumn == true);
            functionComboBoxCustom.Enable(enable && !previewIsGenerated && newColumn == true);
            labelFunction.Enable(enable && !previewIsGenerated && newColumn == true);

            multipleItemsSplitterTrimCheckBox.Enable(enable && !previewIsGenerated && expressionBackup != null && selectedPreset.userPreset);
            multipleItemsSplitterComboBoxCustom.Enable(enable && !previewIsGenerated && expressionBackup != null && selectedPreset.userPreset);
            multipleItemsSplitterLabel.Enable(enable && !previewIsGenerated && expressionBackup != null && selectedPreset.userPreset);

            expressionLabel.Enable(enable && !previewIsGenerated && expressionBackup != null && selectedPreset.userPreset);
            expressionTextBox.Enable(enable && !previewIsGenerated && expressionBackup != null && selectedPreset.userPreset);
            buttonClearExpression.Enable(enable && !previewIsGenerated && expressionBackup != null && selectedPreset.userPreset);

            columnNameTextBox.Enable(enable && !previewIsGenerated && expressionBackup != null && selectedPreset.userPreset);

            totalsCheckBox.Enable(enable && !previewIsGenerated);

            useAnotherPresetAsSourceLabel.Enable(enable && !previewIsGenerated);
            useAnotherPresetAsSourceComboBoxCustom.Enable(enable && !previewIsGenerated);
            clearUseAnotherPresetButton.Enable(enable && !previewIsGenerated && useAnotherPresetAsSourceComboBoxCustom.SelectedIndex >= 0);

            conditionCheckBox.Enable(enable);
            conditionFieldListCustom.Enable(enable && conditionCheckBox.Checked);
            conditionListCustom.Enable(enable && conditionCheckBox.Checked);
            comparedFieldListCustom.Enable(enable && conditionCheckBox.Checked);

            resizeArtworkCheckBox.Enable(enable && !previewIsGenerated);
            newArtworkSizeUpDown.Enable(enable && resizeArtworkCheckBox.Checked && !previewIsGenerated);
            labelRemark.Enable(enable && resizeArtworkCheckBox.Checked && !previewIsGenerated);

            buttonFilterResults.Enable(enable && conditionCheckBox.Checked && previewIsGenerated);
            buttonExport.Enable(enable && previewIsGenerated);
            labelFormat.Enable(enable && previewIsGenerated);
            formatComboBoxCustom.Enable(enable && previewIsGenerated);
            openReportCheckBox.Enable(enable && previewIsGenerated);

            if (sourceFieldComboBoxCustom.SelectedIndex != -1)
            {
                operationComboBoxCustom.Enable(enable && !previewIsGenerated);
                mulDivFactorComboBoxCustom.Enable(enable && !previewIsGenerated);
                roundToLabel.Enable(enable && !previewIsGenerated);
                precisionDigitsComboBoxCustom.Enable(enable && !previewIsGenerated);
                digitsLabel.Enable(enable && !previewIsGenerated);
                appendLabel.Enable(enable && !previewIsGenerated);
                appendTextBox.Enable(enable && !previewIsGenerated);
            }
            else
            {
                operationComboBoxCustom.Enable(false);
                mulDivFactorComboBoxCustom.Enable(false);
                roundToLabel.Enable(false);
                precisionDigitsComboBoxCustom.Enable(false);
                digitsLabel.Enable(false);
                appendLabel.Enable(false);
                appendTextBox.Enable(false);
            }
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
            presetList.Enable(!backgroundTaskIsWorking() && (newColumn == false));

            buttonOK.Enable((previewIsGenerated || SavedSettings.allowCommandExecutionWithoutPreview) && !backgroundTaskIsStopping && (!backgroundTaskIsWorking() || backgroundTaskIsUpdatingTags) && functionsDict.Count > 0);
            buttonPreview.Enable(true);
        }

        internal override void disableQueryingOrUpdatingButtons()
        {
            presetList.Enable(!backgroundTaskIsWorking());

            buttonOK.Enable(false);
            buttonPreview.Enable(false);
        }

        private void buttonAddPreset_Click(object sender, EventArgs e)
        {
            var newPreset = new ReportPreset(ExportedTrackList);
            presetList.Items.Insert(0, newPreset);
            presetList.SelectedIndex = 0;

            PresetsInteractiveWorkingCopy.Add(newPreset.permanentGuid, newPreset);

            if (presetListLastSelectedIndex == 0)
            {
                presetListLastSelectedIndex = -1;
                presetList_SelectedIndexChanged(null, null);
            }

            presetTabControl.SelectedIndex = 1;

            setUnsavedChanges(true);
        }

        private void buttonCopyPreset_Click(object sender, EventArgs e)
        {
            int firstPredefinedPresetIndex;
            for (firstPredefinedPresetIndex = 0; firstPredefinedPresetIndex < presetList.Items.Count; firstPredefinedPresetIndex++)
            {
                if (!(presetList.Items[firstPredefinedPresetIndex] as ReportPreset).userPreset)
                    break;
            }

            var currentIndex = presetList.SelectedIndex;
            var reportsPresetCopy = new ReportPreset(selectedPreset, false);
            if (currentIndex < firstPredefinedPresetIndex)
                presetList.Items.Insert(currentIndex + 1, reportsPresetCopy);
            else
                presetList.Items.Insert(0, reportsPresetCopy);

            presetList.SelectedItem = reportsPresetCopy;

            PresetsInteractiveWorkingCopy.Add(reportsPresetCopy.permanentGuid, reportsPresetCopy);

            if (presetListLastSelectedIndex == presetList.SelectedIndex)
            {
                presetListLastSelectedIndex = -1;
                presetList_SelectedIndexChanged(null, null);
            }

            presetTabControl.SelectedIndex = 1;

            setUnsavedChanges(true);
        }

        private void updatePreset()
        {
            if (selectedPreset != null)
            {
                selectedPreset.guid = Guid.NewGuid();

                if (string.IsNullOrWhiteSpace(presetNameTextBox.Text))
                    selectedPreset.name = null;
                else
                    selectedPreset.name = presetNameTextBox.Text;

                if (assignHotkeyCheckBox.Checked && !selectedPreset.hotkeyAssigned)
                {
                    selectedPreset.hotkeySlot = FindFirstSlot(reportPresetHotkeyUsedSlots, false);
                    reportPresetHotkeyUsedSlots[selectedPreset.hotkeySlot] = true; //-V3106
                }
                else if (!assignHotkeyCheckBox.Checked && selectedPreset.hotkeyAssigned)
                {
                    reportPresetHotkeyUsedSlots[selectedPreset.hotkeySlot] = false;
                    selectedPreset.hotkeySlot = -1;
                }

                selectedPreset.hotkeyAssigned = assignHotkeyCheckBox.Checked;
                selectedPreset.applyToSelectedTracks = useHotkeyForSelectedTracksCheckBox.Checked;


                selectedPreset.functionIds = new string[savedFunctionIds.Count];
                savedFunctionIds.CopyTo(selectedPreset.functionIds);

                int columnCount;
                (selectedPreset.groupings, columnCount) = groupingsDict.valuesToPresetArray(0);
                (selectedPreset.functions, _) = functionsDict.valuesToPresetArray(columnCount);


                selectedPreset.comparedField = comparedFieldListCustom.Text;
                selectedPreset.comparison = (Comparison)conditionListCustom.SelectedIndex;
                selectedPreset.conditionField = conditionFieldListCustom.Text;
                selectedPreset.conditionIsChecked = conditionCheckBox.Checked;
                selectedPreset.totals = totalsCheckBox.Checked;

                if (useAnotherPresetAsSourceComboBox.SelectedIndex == -1)
                {
                    selectedPreset.useAnotherPresetAsSource = false;
                    selectedPreset.anotherPresetAsSource = default;
                }
                else
                {
                    selectedPreset.useAnotherPresetAsSource = true;
                    selectedPreset.anotherPresetAsSource = (ReportPresetReference)useAnotherPresetAsSourceComboBoxCustom.SelectedItem; //-V3148
                }

                selectedPreset.autoApply = presetList.GetItemChecked(presetList.SelectedIndex);

                selectedPreset.destinationTags = new string[savedDestinationTagsNames.Count];

                savedDestinationTagsNames.CopyTo(selectedPreset.destinationTags, 0);


                selectedPreset.operations = new int[operations.Count];
                operations.CopyTo(selectedPreset.operations);
                selectedPreset.mulDivFactors = new string[mulDivFactors.Count];
                mulDivFactors.CopyTo(selectedPreset.mulDivFactors);
                selectedPreset.precisionDigits = new string[precisionDigits.Count];
                precisionDigits.CopyTo(selectedPreset.precisionDigits);
                selectedPreset.appendTexts = new string[appendTexts.Count];
                appendTexts.CopyTo(selectedPreset.appendTexts);

                selectedPreset.resizeArtwork = resizeArtworkCheckBox.Checked;
                selectedPreset.newArtworkSize = (int)newArtworkSizeUpDown.Value;

                selectedPreset.generateAutoName(groupingsDict, functionsDict);

                AllLrPresetGuidsInUse.AddSkip(selectedPreset.guid);

                presetList.Refresh();
            }
        }

        private void buttonDeletePreset_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(this, MsgDeletePresetConfirmation, string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No)
                return;

            presetListSelectedIndexChanged(presetList.SelectedIndex);

            if (presetList.SelectedIndex >= 0)
            {
                ignoreCheckedPresetEvent = false;

                if (presetList.GetItemChecked(presetList.SelectedIndex))
                    presetList.SetItemChecked(presetList.SelectedIndex, false);

                ignoreCheckedPresetEvent = true;

                if (selectedPreset.hotkeyAssigned)
                {
                    reportPresetHotkeyUsedSlots[selectedPreset.hotkeySlot] = false;
                    reportPresetsWithHotkeysCount--;
                }

                PresetsInteractiveWorkingCopy.Remove((presetList.SelectedItem as ReportPreset).permanentGuid);

                var presetListSelectedIndex = presetList.SelectedIndex;
                presetList.Items.RemoveAt(presetList.SelectedIndex);

                presetListLastSelectedIndex = -1;

                if (presetList.Items.Count - 1 >= presetListSelectedIndex)
                    presetList.SelectedIndex = presetListSelectedIndex;
                else if (presetList.Items.Count > 0)
                    presetList.SelectedIndex = presetList.Items.Count - 1;
                else
                    resetLocalsAndUiControls();

                setUnsavedChanges(true);
            }
        }

        private void conditionFieldList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (presetIsLoading)
                return;

            setPresetChanged();
        }

        private void conditionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (presetIsLoading)
                return;

            setPresetChanged();
        }

        private void comparedFieldList_TextChanged(object sender, EventArgs e)
        {
            if (presetIsLoading)
                return;

            setPresetChanged();
        }

        private void idTextBox_Leave(object sender, EventArgs e)
        {
            if (presetList.SelectedIndex == -1)
                return;

            SetMultipleItemsSplitterComboBoxText(splitterBackup);

            if (sourceFieldComboBoxCustom.SelectedIndex == -1)
            {
                if (!string.IsNullOrEmpty(idTextBox.Text))
                {
                    MessageBox.Show(this, MsgFirstSelectWhichFieldYouWantToAssignFunctionIdTo, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    idTextBox.Text = string.Empty;
                }

                return;
            }

            if (string.IsNullOrEmpty(idTextBox.Text))
            {
                savedFunctionIds[sourceFieldComboBoxCustom.SelectedIndex] = string.Empty;

                setPresetChanged();

                return;
            }

            var allowedRemoved = Regex.Replace(idTextBox.Text, @"[0-9]", string.Empty);
            allowedRemoved = Regex.Replace(allowedRemoved, @"[a-z]", string.Empty);
            allowedRemoved = Regex.Replace(allowedRemoved, @"[A-Z]", string.Empty);
            allowedRemoved = Regex.Replace(allowedRemoved, @"\-", string.Empty);
            allowedRemoved = Regex.Replace(allowedRemoved, @"_", string.Empty);
            allowedRemoved = Regex.Replace(allowedRemoved, @"\:", string.Empty);

            if (!string.IsNullOrEmpty(allowedRemoved))
            {
                MessageBox.Show(this, MsgNotAllowedSymbols, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                savedFunctionIds[sourceFieldComboBoxCustom.SelectedIndex] = string.Empty;
                idTextBox.Text = string.Empty;
            }
            else
            {
                foreach (ReportPreset preset in presetList.Items) //Let's iterate through other (saved) presets
                {
                    foreach (var id in preset.functionIds)
                    {
                        if (idTextBox.Text == id && preset != selectedPreset)
                        {
                            MessageBox.Show(this, MsgPresetExists.Replace("%%ID%%", idTextBox.Text), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            savedFunctionIds[sourceFieldComboBoxCustom.SelectedIndex] = string.Empty;
                            idTextBox.Text = string.Empty;
                            return;
                        }
                    }
                }

                for (var i = 0; i < sourceFieldComboBoxCustom.Items.Count; i++) //Let's iterate through current preset
                {
                    var id = savedFunctionIds[i];

                    if (idTextBox.Text == id && i != sourceFieldComboBoxCustom.SelectedIndex)
                    {
                        MessageBox.Show(this, MsgPresetExists.Replace("%%ID%%", idTextBox.Text), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        savedFunctionIds[sourceFieldComboBoxCustom.SelectedIndex] = string.Empty;
                        idTextBox.Text = string.Empty;
                        return;
                    }
                }

                savedFunctionIds[sourceFieldComboBoxCustom.SelectedIndex] = idTextBox.Text;

                if (!sourceFieldComboBoxIndexChanging)
                    setPresetChanged();
            }
        }

        private void clearIdButton_Click(object sender, EventArgs e)
        {
            idTextBox.Text = string.Empty;
            idTextBox_Leave(null, null);
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            //saveSettings();
            exportTrackList(openReportCheckBox.Checked);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (!saveSettings())
                return;

            if (ModifierKeys != Keys.Control)
                Close();
            else
                presetList.Refresh();
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {

            if (resizeArtworkCheckBox.Checked)
                previewTableArtworkSize = (int)newArtworkSizeUpDown.Value;
            else
                previewTableArtworkSize = PreviewTableDefaultArtworkSize;

            hidePreview = hidePreviewCheckBox.Checked;
            ignoreClosingForm = clickOnPreviewButton(prepareBackgroundPreview, previewReport, buttonPreview, buttonExport, buttonClose);

            if (!ignoreClosingForm)
            {
                presetListLastSelectedIndex = -1;
                presetList_SelectedIndexChanged(null, null);
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out var files);
            if (files == null || files.Length == 0)
            {
                MessageBox.Show(MbForm, MsgNoTracksSelected, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            previewTable.AllowUserToResizeColumns = false;
            previewTable.AllowUserToResizeRows = false;
            foreach (DataGridViewColumn column in previewTable.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            ApplyReportPreset(selectedPreset, getReportPresetsArrayUI(), this);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            forceCloseForms = false;
            Close();
        }

        private void buttonFilterResultsChangeLabel()
        {
            if (resultsAreFiltered)
            {
                buttonFilterResults.Text = LrButtonFilterResultsShowAllText;
                toolTip1.SetToolTip(buttonFilterResults, LrButtonFilterResultsShowAllToolTip);
            }
            else
            {
                buttonFilterResults.Text = ButtonFilterResultsText;
                toolTip1.SetToolTip(buttonFilterResults, ButtonFilterResultsToolTip);
            }
        }

        private void buttonFilterResults_Click(object sender, EventArgs e)
        {
            rows.Clear();
            source.ResetBindings(false);

            if (backgroundTaskIsWorking())
            {
                updateCustomScrollBars(previewTable);

                return;
            }

            if (previewTable.Columns.GetColumnCount(DataGridViewElementStates.Visible) == 0)
            {
                updateCustomScrollBars(previewTable);

                //MessageBox.Show(this, plugin.msgNoTagsSelected);
                return;
            }

            appliedPreset = selectedPreset;
            reportPresets = getReportPresetsArrayUI();
            backgroundTaskIsUpdatingTags = false;

            prepareConditionRelatedLocals();

            backgroundTaskIsStopping = false;
            backgroundTaskIsScheduled = false;

            if (resultsAreFiltered)
                applyPresetResults(this, null, previewedTags, previewedFilesActualComposedSplitGroupingTagsList, true, false, null, false);
            else
                applyPresetResults(this, null, previewedTags, previewedFilesActualComposedSplitGroupingTagsList, true, false, null, true);

            resultsAreFiltered = !resultsAreFiltered;

            updateCustomScrollBars(previewTable);

            buttonFilterResultsChangeLabel();
        }

        private void LibraryReports_Load(object sender, EventArgs e)
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

        private void LibraryReports_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ignoreClosingForm)
            {
                if (!backgroundTaskIsUpdatingTags)
                {
                    closeFormOnStopping = true;
                    buttonClose.Enable(false);
                    buttonSaveClose.Enable(false);

                    backgroundTaskIsStopping = true;
                    SetStatusBarText(this, LibraryReportsSbText + SbTextStoppingCurrentOperation, false);

                    e.Cancel = true;
                }
            }
            else if (!PluginClosing && unsavedChanges)
            {
                var lastAnswer = SavedSettings.unsavedChangesConfirmationLastAnswer;
                var confirmationButtons = MessageBoxButtons.YesNo;
                DialogResult result;
                if (!forceCloseForms)
                    confirmationButtons = MessageBoxButtons.YesNoCancel;

                result = MessageBox.Show(this, MsgLrDoYouWantToSaveChangesBeforeClosingTheWindow,
                    string.Empty, confirmationButtons, MessageBoxIcon.Warning, lastAnswer);


                forceCloseForms = true;

                if (result == DialogResult.Yes)
                {
                    SavedSettings.unsavedChangesConfirmationLastAnswer = MessageBoxDefaultButton.Button1;
                    if (!saveSettings())
                        e.Cancel = true;
                }
                else if (result == DialogResult.No)
                {
                    SavedSettings.unsavedChangesConfirmationLastAnswer = MessageBoxDefaultButton.Button2;
                }
                else if (result == DialogResult.OK)
                {
                    //Nothing to do, let's ignore all preset changes and just close the form...
                }
                else //if (result == DialogResult.Cancel)
                {
                    //SavedSettings.unsavedChangesConfirmationLastAnswer = MessageBoxDefaultButton.Button3;
                    e.Cancel = true;
                }
            }
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (!ignoreSplitterMovedEvent)
                saveWindowLayout(0, 0, 0, splitContainer1.SplitterDistance);
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, MsgHelp, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void setColumnChanged(bool? newColumn)
        {
            if (presetIsLoading)
            {
                return;
            }
            else if (this.newColumn != false && newColumn == null)
            {
                enableDisablePreviewOptionControls(true);

                buttonAddPreset.Enable(false);
                buttonCopyPreset.Enable(false);
                buttonDeletePreset.Enable(false);

                buttonSaveClose.Enable(false);
                buttonClose.Enable(false);

                assignHotkeyCheckBox.Checked = false;
                assignHotkeyCheckBox.Enable(false);


                disableQueryingOrUpdatingButtons();

                return;
            }
            else if (this.newColumn != false) //Discarding changes
            {
                expressionTextBox.Text = expressionBackup;
                SetMultipleItemsSplitterComboBoxText(splitterBackup);
                multipleItemsSplitterTrimCheckBox.Checked = trimValuesBackup;

                columnNameTextBox.Text = columnNameBackup;

                enableDisablePreviewOptionControls(true);
                enableQueryingOrUpdatingButtons();

                setColumnSaved();

                return;
            }
            else if (newColumn == null) //Updating expression
            {
                buttonUpdateFunction.Text = ButtonUpdateFunctionUpdateText;
                toolTip1.SetToolTip(buttonUpdateFunction, ButtonUpdateFunctionUpdateToolTip);
            }

            //Adding new grouping/function or updating expression
            buttonAddFunction.Text = ButtonAddFunctionDiscardText;
            toolTip1.SetToolTip(buttonAddFunction, ButtonAddFunctionDiscardToolTip);

            this.newColumn = newColumn;

            buttonUpdateFunction.Image = warning;

            enableDisablePreviewOptionControls(true);

            buttonAddPreset.Enable(false);
            buttonCopyPreset.Enable(false);
            buttonDeletePreset.Enable(false);

            buttonSaveClose.Enable(false);
            buttonClose.Enable(false);

            assignHotkeyCheckBox.Checked = false;
            assignHotkeyCheckBox.Enable(false);


            disableQueryingOrUpdatingButtons();
        }

        private void setColumnSaved()
        {
            buttonAddFunction.Text = ButtonAddFunctionCreateText;
            toolTip1.SetToolTip(buttonAddFunction, ButtonAddFunctionCreateToolTip);

            newColumn = false;

            enableDisablePreviewOptionControls(true);

            buttonSaveClose.Enable(true);
            buttonClose.Enable(true);

            enableQueryingOrUpdatingButtons();

            tagsDataGridView.Enable(true);
            expressionsDataGridView.Enable(true);

            buttonUpdateFunction.Text = ButtonUpdateFunctionSaveText;

            toolTip1.SetToolTip(buttonUpdateFunction, ButtonUpdateFunctionSaveToolTip);
            buttonUpdateFunction.Image = Resources.transparent_15;

            expressionTextBox.Text = expressionBackup;
            SetMultipleItemsSplitterComboBoxText(splitterBackup);
            multipleItemsSplitterTrimCheckBox.Checked = trimValuesBackup;

            columnNameTextBox.Text = columnNameBackup;

            enableDisablePreviewOptionControls(true);
        }

        private void buttonAddFunction_Click(object sender, EventArgs e)
        {
            setColumnChanged(true);
        }

        private void buttonUpdateFunction_Click(object sender, EventArgs e)
        {
            if (addColumn(sourceTagListCustom.Text, parameter2ComboBoxCustom.Text, (LrFunctionType)functionComboBoxCustom.SelectedIndex,
                multipleItemsSplitterComboBoxCustom.Text, multipleItemsSplitterTrimCheckBox.Checked, expressionTextBox.Text, columnNameTextBox.Text))
            {
                setColumnSaved();
                setPresetChanged();

                updateCustomScrollBars(previewTable);
                updateCustomScrollBars(tagsDataGridView);
                updateCustomScrollBars(expressionsDataGridView);
            }
        }

        private void tagsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0 && newColumn == false && selectedPreset?.userPreset == true)
            {
                var shortId = tagsDataGridView.Rows[e.RowIndex].Cells[0].Tag as string;
                var commonAttr = (ColumnAttributes)tagsDataGridView.Rows[e.RowIndex].Cells[1].Tag;

                List<string> ids;
                if (commonAttr.functionType == LrFunctionType.Grouping)
                {
                    ids = groupingsDict.getAllIdsByShortId(shortId);
                    foreach (var id in ids)
                    {
                        var attribs = groupingsDict[id];
                        removeColumn(attribs.parameterName, null, LrFunctionType.Grouping, attribs.splitter, attribs.trimValues, attribs.expression, attribs.columnName);

                        updateCustomScrollBars(previewTable);
                        updateCustomScrollBars(tagsDataGridView);
                        updateCustomScrollBars(expressionsDataGridView);
                    }
                }
                else
                {
                    ids = functionsDict.getAllIdsByShortId(shortId);
                    foreach (var id in ids)
                    {
                        var attribs = functionsDict[id];
                        removeColumn(attribs.parameterName, attribs.parameter2Name, attribs.functionType, null, false, attribs.expression, attribs.columnName);

                        updateCustomScrollBars(previewTable);
                        updateCustomScrollBars(tagsDataGridView);
                        updateCustomScrollBars(expressionsDataGridView);
                    }
                }
            }
        }
        private void tagsDataGridViewRowSelected(int rowIndex)
        {
            if (rowIndex >= 0) //Maybe new row is selected
            {
                tagsDataGridViewSelectedRow = rowIndex;

                var shortId = tagsDataGridView.Rows[rowIndex].Cells[0].Tag as string;

                var commonAttr = (ColumnAttributes)tagsDataGridView.Rows[rowIndex].Cells[1].Tag;

                splitterBackup = commonAttr.splitter;
                trimValuesBackup = commonAttr.trimValues;

                functionComboBoxCustom.SelectedIndex = (int)commonAttr.functionType;
                sourceTagListCustom.SelectedItem = commonAttr.parameterName;
                parameter2ComboBoxCustom.SelectedItem = commonAttr.parameter2Name;
                SetMultipleItemsSplitterComboBoxText(commonAttr.splitter);
                multipleItemsSplitterTrimCheckBox.Checked = commonAttr.trimValues;


                fillExpressionsDataGridView(shortId, false);
            }
            else
            {
                expressionsDataGridView.RowCount = 0;
            }

            enableDisablePreviewOptionControls(true);

            updateCustomScrollBars(expressionsDataGridView);
        }

        private void tagsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 0 && e.RowIndex >= 0) //Maybe new row is selected
                if (tagsDataGridViewSelectedRow != e.RowIndex)
                    tagsDataGridViewRowSelected(e.RowIndex);
        }

        //If newExpression == true, then new row is added to expressionsDataGridView, select it, last row
        //If newExpression == false, then new tag column is selected, select 1st row here
        //If newExpression == null, then it's just existing expression replaced, reselect it
        private void fillExpressionsDataGridView(string shortId, bool? newExpression)
        {
            var index = getSelectedRow(expressionsDataGridView);

            if (expressionsDataGridView.RowCount > 0)
                expressionsDataGridView.RowCount = 0;

            if (tagsDataGridViewSelectedRow == -1)
                return;

            var exprs = shortIdsExprs[shortId];
            var names = shortIdsColNames[shortId];

            for (int i = 0; i < exprs.Count; i++)
            {
                var expr = exprs[i];
                var name = names[i];

                expressionsDataGridView.Rows.Add();
                expressionsDataGridView.Rows[expressionsDataGridView.RowCount - 1].Cells[0].Value = ColumnCheckedState;
                expressionsDataGridView.Rows[expressionsDataGridView.RowCount - 1].Cells[1].Value = string.IsNullOrEmpty(expr) ? NoExpressionText : expr;
                expressionsDataGridView.Rows[expressionsDataGridView.RowCount - 1].Cells[1].Tag = expr;
                expressionsDataGridView.Rows[expressionsDataGridView.RowCount - 1].Cells[2].Value = string.IsNullOrEmpty(name) ? AutoColumnNameText : name;
                expressionsDataGridView.Rows[expressionsDataGridView.RowCount - 1].Cells[2].Tag = name;

                if (selectedPreset?.userPreset == true)
                    expressionsDataGridView.Rows[expressionsDataGridView.RowCount - 1].Cells[0].ToolTipText = ExpressionsDataGridViewCheckBoxToolTip;
            }

            deselectAllRows(expressionsDataGridView);

            if (newExpression == false)
            {
                selectRow(expressionsDataGridView, 0);

                expressionBackup = expressionsDataGridView.Rows[0].Cells[1].Tag as string;
                expressionTextBox.Text = expressionBackup;

                columnNameBackup = expressionsDataGridView.Rows[0].Cells[2].Tag as string;
                columnNameTextBox.Text = columnNameBackup;
            }
            else if (newExpression == null)
            {
                selectRow(expressionsDataGridView, index);

                expressionBackup = expressionsDataGridView.Rows[index].Cells[1].Tag as string;
                expressionTextBox.Text = expressionBackup;

                columnNameBackup = expressionsDataGridView.Rows[0].Cells[2].Tag as string;
                columnNameTextBox.Text = columnNameBackup;
            }
            else
            {
                var lastIndex = expressionsDataGridView.RowCount - 1;
                selectRow(expressionsDataGridView, lastIndex);

                expressionBackup = expressionsDataGridView.Rows[lastIndex].Cells[1].Tag as string;
                expressionTextBox.Text = expressionBackup;

                columnNameBackup = expressionsDataGridView.Rows[lastIndex].Cells[2].Tag as string;
                columnNameTextBox.Text = columnNameBackup;
            }
        }

        private void expressionsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0 && newColumn == false && selectedPreset?.userPreset == true)
            {
                var commonAttr = (ColumnAttributes)tagsDataGridView.Rows[tagsDataGridViewSelectedRow].Cells[1].Tag;

                if (expressionsDataGridView.RowCount == 1)
                {
                    if (MessageBox.Show(this, MsgDoYouWantToDeleteTheField.Replace(@"\\", "\n\n")
                        .Replace("%%FIELD-NAME%%", commonAttr.getColumnName(false, false, false)),
                        string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;
                }

                var expression = expressionsDataGridView.Rows[e.RowIndex].Cells[1].Tag as string;
                var columnName = expressionsDataGridView.Rows[e.RowIndex].Cells[2].Tag as string;

                removeColumn(commonAttr.parameterName, commonAttr.parameter2Name, commonAttr.functionType, commonAttr.splitter, commonAttr.trimValues, expression, columnName);

                updateCustomScrollBars(previewTable);
                updateCustomScrollBars(tagsDataGridView);
                updateCustomScrollBars(expressionsDataGridView);
            }
        }

        private void expressionsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 0 && e.RowIndex >= 0) //Maybe new row is selected
            {
                expressionBackup = expressionsDataGridView.Rows[e.RowIndex].Cells[1].Tag as string;
                expressionTextBox.Text = expressionBackup;

                columnNameBackup = expressionsDataGridView.Rows[e.RowIndex].Cells[2].Tag as string;
                columnNameTextBox.Text = columnNameBackup;

                enableDisablePreviewOptionControls(true);
            }
        }

        private void sourceTagList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sourceTagListCustom.Text == ArtworkName || sourceTagListCustom.Text == SequenceNumberName)
            {
                expressionBackup = null;
                columnNameBackup = null;
            }
            else if (expressionBackup == null)
            {
                if (expressionsDataGridView.SelectedRows.Count > 0)
                {
                    var index = expressionsDataGridView.SelectedRows[0].Index;
                    expressionBackup = expressionsDataGridView.Rows[index].Cells[1].Tag as string;
                    columnNameBackup = expressionsDataGridView.Rows[index].Cells[2].Tag as string;
                }
            }

            enableDisablePreviewOptionControls(true);
        }

        private void parameter2ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Nothing is required for now
        }

        private void multipleItemsSplitterComboBox_TextUpdate(object sender, EventArgs e)
        {
            if (multipleItemsSplitterComboBoxCustom.Text == splitterBackup || newColumn == true)
                return;

            setColumnChanged(null);
        }

        internal void SetMultipleItemsSplitterComboBoxText(string text)
        {
            if (destinationTagListCustom.SelectedItem as string == NullTagName && string.IsNullOrEmpty(idTextBox.Text))//====
            {
                enableDisableItemSplitter(true);
                if (string.IsNullOrEmpty(text))
                    text = DontUseSplitter;
            }
            else
            {
                enableDisableItemSplitter(false);
                text = DontUseSplitter;
            }


            multipleItemsSplitterComboBoxCustom.SetText(text, true);
        }

        private string getMultipleItemsSplitterComboBoxText()
        {
            if (multipleItemsSplitterComboBoxCustom.SelectedItem == null)
                return null;


            var text = multipleItemsSplitterComboBoxCustom.GetItemText(multipleItemsSplitterComboBoxCustom.SelectedItem);

            if (multipleItemsSplitterComboBoxCustom.SelectedIndex == 0)
            {
                text = text.TrimStart(' ');
                multipleItemsSplitterTrimCheckBox.Checked = false;
            }
            else if (multipleItemsSplitterComboBoxCustom.SelectedIndex == 1)
            {
                text = "\\0";
                multipleItemsSplitterTrimCheckBox.Checked = false;
            }
            else
            {
                text = text.TrimStart(' ');
                text = text.Substring(0, text.IndexOf(' '));
                multipleItemsSplitterTrimCheckBox.Checked = true;
            }


            return text;
        }

        private void multipleItemsSplitterComboBox_DropDownClosed(object sender, EventArgs e)
        {
            var text = getMultipleItemsSplitterComboBoxText();

            if (text == null)
                return;


            SetMultipleItemsSplitterComboBoxText(text);


            if (text == splitterBackup || newColumn == true)
                return;

            setColumnChanged(null);
        }

        private void multipleItemsSplitterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Nothing is required for now
        }

        private void multipleItemsSplitterTrimCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (multipleItemsSplitterTrimCheckBox.Checked == trimValuesBackup || newColumn == true)
                return;

            setColumnChanged(null);
        }

        private void multipleItemsSplitterTrimCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!multipleItemsSplitterTrimCheckBox.IsEnabled())
                return;

            multipleItemsSplitterTrimCheckBox.Checked = !multipleItemsSplitterTrimCheckBox.Checked;
        }

        private void functionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (functionComboBoxCustom.SelectedIndex >= functionComboBoxCustom.Items.Count - 2) //Average or Average Count
            {
                multipleItemsSplitterLabel.Visible = false;
                multipleItemsSplitterComboBoxCustom.Visible = false;

                multipleItemsSplitterTrimCheckBox.Visible = false;
                multipleItemsSplitterTrimCheckBoxLabel.Visible = false;

                parameter2Label.Visible = true;
                parameter2ComboBoxCustom.Visible = true;
            }
            else if (functionComboBoxCustom.SelectedIndex == 0) //Grouping
            {
                multipleItemsSplitterLabel.Visible = true;
                multipleItemsSplitterComboBoxCustom.Visible = true;

                multipleItemsSplitterTrimCheckBoxLabel.Visible = true;
                multipleItemsSplitterTrimCheckBox.Visible = true;

                parameter2Label.Visible = false;
                parameter2ComboBoxCustom.Visible = false;
            }
            else //Other functions
            {
                multipleItemsSplitterLabel.Visible = false;
                multipleItemsSplitterComboBoxCustom.Visible = false;

                multipleItemsSplitterTrimCheckBox.Visible = false;
                multipleItemsSplitterTrimCheckBoxLabel.Visible = false;

                parameter2Label.Visible = false;
                parameter2ComboBoxCustom.Visible = false;
            }
        }

        private void expressionTextBox_TextChanged(object sender, EventArgs e)
        {

            if (expressionTextBox.Text == expressionBackup || newColumn == true)
                return;

            setColumnChanged(null);
        }

        private void buttonClearExpression_Click(object sender, EventArgs e)
        {
            expressionTextBox.Text = string.Empty;
        }

        private void columnNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (columnNameTextBox.Text == columnNameBackup || newColumn == true)
                return;

            setColumnChanged(null);
        }

        private void presetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (presetListLastSelectedIndex == presetList.SelectedIndex)
                return;

            if (previewIsGenerated)
                clickOnPreviewButton(prepareBackgroundPreview, previewReport, buttonPreview, buttonOK, buttonClose);

            selectedPreset = presetList.SelectedItem as ReportPreset;

            presetIsLoading = true;
            presetListSelectedIndexChanged(presetList.SelectedIndex);
            presetListLastSelectedIndex = presetList.SelectedIndex;
            presetIsLoading = false;
        }

        private void presetListSelectedIndexChanged(int index)
        {
            if (presetListLastSelectedIndex != -2) //It's not form init
                Enable(false, autoApplyPresetsLabel, null);


            resetLocalsAndUiControls();

            setColumnSaved();

            multipleItemsSplitterComboBoxCustom.SetText(string.Empty, false);

            selectedPreset = null;

            formatComboBoxCustom.SelectedIndex = 1;

            presetNameTextBox.Enable(false);

            useAnotherPresetAsSourceComboBoxCustom.ItemsClear();

            multipleItemsSplitterTrimCheckBox.Checked = false;

            conditionCheckBox.Checked = false;
            conditionCheckBox_CheckedChanged(null, null);

            conditionFieldListCustom.Text = string.Empty;
            conditionListCustom.Text = string.Empty;
            comparedFieldListCustom.Text = string.Empty;
            sourceFieldComboBoxCustom.Text = string.Empty;
            destinationTagListCustom.Text = string.Empty;
            functionComboBoxCustom.SelectedIndex = -1;
            parameter2ComboBoxCustom.Text = string.Empty;
            sourceTagListCustom.Text = string.Empty;

            idTextBox.Text = string.Empty;

            totalsCheckBox.Checked = false;

            assignHotkeyCheckBox.Enable(false);

            presetNameTextBox.Text = string.Empty;
            assignHotkeyCheckBox.Checked = false;
            useHotkeyForSelectedTracksCheckBox.CheckState = CheckState.Unchecked;

            resizeArtworkCheckBox.Checked = false;
            resizeArtworkCheckBox.Enable(false);
            newArtworkSizeUpDown.Enable(false);

            sourceFieldComboBox_SelectedIndexChanged(null, null);
            functionComboBox_SelectedIndexChanged(null, null);

            tagsDataGridView.RowCount = 0;
            tagsDataGridViewSelectedRow = -1;
            expressionsDataGridView.RowCount = 0;

            expressionBackup = null;
            splitterBackup = null;
            trimValuesBackup = false;
            columnNameBackup = null;


            if (index < 0)
            {
                enableDisablePreviewOptionControls(true);
                disableQueryingOrUpdatingButtons();

                if (presetListLastSelectedIndex != -2) //It's not form init
                    Enable(true, autoApplyPresetsLabel, null);

                updateCustomScrollBars(presetList);
                updateCustomScrollBars(previewTable);
                updateCustomScrollBars(tagsDataGridView);
                updateCustomScrollBars(expressionsDataGridView);
            }
            else
            {
                initPreset();
            }
        }

        private void initPreset()
        {
            selectedPreset = presetList.SelectedItem as ReportPreset;

            if (selectedPreset != null && selectedPreset.useAnotherPresetAsSource && selectedPreset.anotherPresetAsSource.permanentGuid == Guid.Empty)
            {
                selectedPreset.useAnotherPresetAsSource = false;
                setPresetChanged();
            }


            enableDisablePreviewOptionControls(false);

            if (selectedPreset.fileFormatIndex == 0)
                formatComboBoxCustom.SelectedIndex = 1;
            else
                formatComboBoxCustom.SelectedIndex = (int)selectedPreset.fileFormatIndex - 1;


            //Groupings
            var dict = new ColumnAttributesDict();
            var columnCount = prepareDict(dict, null, selectedPreset.groupings, 0);
            if (columnCount == -1)
            {
                multipleItemsSplitterComboBoxCustom.SetText(string.Empty, false);

                enableDisablePreviewOptionControls(true);
                enableQueryingOrUpdatingButtons();

                Enable(true, autoApplyPresetsLabel, null);

                return;
            }


            previewTable.SetColumnsAutoSizeHeaders();

            foreach (var attribs in dict.Values)
            {
                addColumn(attribs.parameterName, null, LrFunctionType.Grouping,
                    attribs.splitter, attribs.trimValues, attribs.expression, attribs.columnName);
            }


            //Functions
            dict.Clear();
            prepareDict(dict, null, selectedPreset.functions, columnCount);
            foreach (var attribs in dict.Values)
            {
                addColumn(attribs.parameterName, attribs.parameter2Name, attribs.functionType,
                    null, false, attribs.expression, attribs.columnName);
            }

            if (selectedPreset.operations.Length == selectedPreset.functions.Length
                && selectedPreset.mulDivFactors.Length == selectedPreset.functions.Length
                && selectedPreset.precisionDigits.Length == selectedPreset.functions.Length
                && selectedPreset.appendTexts.Length == selectedPreset.functions.Length
                )
            {
                operations.Clear();
                operations.AddRange(selectedPreset.operations);
                mulDivFactors.Clear();
                mulDivFactors.AddRange(selectedPreset.mulDivFactors);
                precisionDigits.Clear();
                precisionDigits.AddRange(selectedPreset.precisionDigits);
                appendTexts.Clear();
                appendTexts.AddRange(selectedPreset.appendTexts);
            }
            else
            {
                selectedPreset.operations = new int[operations.Count];
                operations.CopyTo(selectedPreset.operations);
                selectedPreset.mulDivFactors = new string[mulDivFactors.Count];
                mulDivFactors.CopyTo(selectedPreset.mulDivFactors);
                selectedPreset.precisionDigits = new string[precisionDigits.Count];
                precisionDigits.CopyTo(selectedPreset.precisionDigits);
                selectedPreset.appendTexts = new string[appendTexts.Count];
                appendTexts.CopyTo(selectedPreset.appendTexts);
            }


            previewTable.DisableColumnsAutoSize(true);


            savedFunctionIds.Clear();
            savedFunctionIds.AddRange(selectedPreset.functionIds);

            savedDestinationTagsNames.Clear();
            savedDestinationTagsNames.AddRange(selectedPreset.destinationTags);


            if (parameter2ComboBoxCustom.SelectedIndex == -1)
                parameter2ComboBoxCustom.SelectedItem = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);


            assignHotkeyCheckBox.Checked = selectedPreset.hotkeyAssigned;
            useHotkeyForSelectedTracksCheckBox.Checked = selectedPreset.applyToSelectedTracks;
            totalsCheckBox.Checked = selectedPreset.totals;

            if (reportPresetsWithHotkeysCount < MaximumNumberOfLrHotkeys)
                ; //Won't change state
            else if (reportPresetsWithHotkeysCount == MaximumNumberOfLrHotkeys && selectedPreset.hotkeyAssigned)
                ; //Won't change state
            else
                assignHotkeyCheckBox.Enable(false);


            findFilteringPresetsUI(useAnotherPresetAsSourceComboBoxCustom, selectedPreset, selectedPreset.anotherPresetAsSource);


            totalsCheckBox.Checked = selectedPreset.totals;
            conditionCheckBox.Checked = selectedPreset.conditionIsChecked;
            conditionFieldListCustom.Text = selectedPreset.conditionField;
            conditionListCustom.SelectedIndex = (int)selectedPreset.comparison;
            comparedFieldListCustom.Text = selectedPreset.comparedField;

            if (sourceFieldComboBoxCustom.Items.Count > 0)
            {
                sourceFieldComboBoxCustom.SelectedIndex = 0;
                destinationTagListCustom.SelectedItem = selectedPreset.destinationTags[0];
                idTextBox.Text = savedFunctionIds[0];
            }
            else
            {
                sourceFieldComboBox_SelectedIndexChanged(null, null);
            }


            resizeArtworkCheckBox.Checked = selectedPreset.resizeArtwork;
            newArtworkSizeUpDown.Value = selectedPreset.newArtworkSize;

            if (parameter2ComboBoxCustom.SelectedIndex == -1)
                parameter2ComboBoxCustom.SelectedItem = GetPropName(FilePropertyType.Url);

            parameter2ComboBox_SelectedIndexChanged(null, null);


            if (selectedPreset.groupings.Length > 0)
                functionComboBoxCustom.SelectedIndex = 0;
            else if (selectedPreset.functions.Length > 0)
                functionComboBoxCustom.SelectedIndex = (int)selectedPreset.functions[0].functionType;
            else
                functionComboBoxCustom.SelectedIndex = 0;

            if (sourceTagListCustom.Items.Count > 0)
                sourceTagListCustom.SelectedIndex = 0;


            setArtworkSequenceNumberFields();
            formatTrackColumns();

            if (tagsDataGridView.RowCount > 0)
            {
                selectRow(tagsDataGridView, 0);
            }
            else
            {
                expressionBackup = string.Empty;
                splitterBackup = string.Empty;
                trimValuesBackup = false;
                columnNameBackup = string.Empty;
            }


            if (expressionsDataGridView.RowCount > 0)//===
                selectRow(expressionsDataGridView, 0);

            setColumnSaved();


            enableDisablePreviewOptionControls(true);
            enableQueryingOrUpdatingButtons();


            presetNameTextBox.Enable(true);
            presetNameTextBox.ReadOnly(!selectedPreset.userPreset);
            presetNameTextBox.Text = selectedPreset.name ?? string.Empty;


            if (presetListLastSelectedIndex != -2) //It's during form init
            {
                Enable(true, autoApplyPresetsLabel, null);
                presetList.Focus();
            }


            updateCustomScrollBars(presetList);
            updateCustomScrollBars(previewTable);
            updateCustomScrollBars(tagsDataGridView);
            updateCustomScrollBars(expressionsDataGridView);
        }

        private void conditionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            conditionFieldListCustom.Enable(conditionCheckBox.Checked);
            conditionListCustom.Enable(conditionCheckBox.Checked && conditionFieldListCustom.SelectedIndex != -1);
            comparedFieldListCustom.Enable(conditionCheckBox.Checked && conditionFieldListCustom.SelectedIndex != -1);
            buttonFilterResults.Enable(conditionCheckBox.Checked && previewIsGenerated);

            if (presetIsLoading)
                return;

            updatePreset();
            setUnsavedChanges(true);
            setPresetChanged();
        }

        private void conditionCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!conditionCheckBox.IsEnabled())
                return;

            conditionCheckBox.Checked = !conditionCheckBox.Checked;
        }

        private void previewTable_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //e.ThrowException = false;
        }

        private void enableDisableItemSplitter(bool status)
        {
            if (status)
            {
                multipleItemsSplitterLabel.Enable(true);
                multipleItemsSplitterComboBoxCustom.Enable(true);
                multipleItemsSplitterTrimCheckBox.Enable(true);
                multipleItemsSplitterTrimCheckBoxLabel.Enable(true);
            }
            else
            {
                multipleItemsSplitterLabel.Enable(false);
                multipleItemsSplitterComboBoxCustom.Enable(false);
                multipleItemsSplitterTrimCheckBox.Enable(false);
                multipleItemsSplitterTrimCheckBoxLabel.Enable(false);
            }
        }

        private void destinationTagList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetMultipleItemsSplitterComboBoxText(splitterBackup);

            if (sourceFieldComboBoxCustom.SelectedIndex != -1 && destinationTagListCustom.SelectedItem as string == NullTagName)
            {
                savedDestinationTagsNames[sourceFieldComboBoxCustom.SelectedIndex] = destinationTagListCustom.Text;

                if (!sourceFieldComboBoxIndexChanging)
                    setPresetChanged();
            }
        }

        private void presetList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e != null)
            {
                if (ignoreCheckedPresetEvent || functionsDict.Count == 0)
                {
                    if (e.NewValue == CheckState.Checked)
                        e.NewValue = CheckState.Unchecked;
                    else if (e.NewValue == CheckState.Unchecked)
                        e.NewValue = CheckState.Checked;

                    return;
                }

                if (presetList.SelectedIndex != -1)
                    setUnsavedChanges(true);

                if (e.NewValue == CheckState.Checked)
                {
                    (presetList.Items[e.Index] as ReportPreset).autoApply = true;
                    autoAppliedPresetCount++;

                    if (!SavedSettings.dontPlayTickedAutoApplyingAsrLrPresetSound && presetList.SelectedIndex != -1)
                        System.Media.SystemSounds.Exclamation.Play();
                }
                else
                {
                    (presetList.Items[e.Index] as ReportPreset).autoApply = false;
                    autoAppliedPresetCount--;
                }
            }


            if (autoAppliedPresetCount == 0)
            {
                if (SavedSettings.allowAsrLrPresetAutoExecution)
                {
                    autoApplyPresetsLabel.Text = AutoApplyText;

                    autoApplyPresetsLabel.ForeColor = UntickedColor;
                }
                else
                {
                    autoApplyPresetsLabel.Text = AutoApplyText + "\n"
                        + AutoApplyDisabledText.ToUpper();

                    autoApplyPresetsLabel.ForeColor = TickedColor;
                }
            }
            else
            {
                if (SavedSettings.allowAsrLrPresetAutoExecution)
                {
                    autoApplyPresetsLabel.Text = AutoApplyText + "\n"
                    + NowTickedText.ToUpper().Replace("%%TICKED-PRESETS%%", autoAppliedPresetCount.ToString());
                }
                else
                {
                    autoApplyPresetsLabel.Text = AutoApplyText + "\n"
                    + AutoApplyDisabledText.ToUpper() + "\n"
                    + NowTickedText.ToUpper().Replace("%%TICKED-PRESETS%%", autoAppliedPresetCount.ToString());
                }

                autoApplyPresetsLabel.ForeColor = TickedColor;
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

        private void presetNameTextBox_Leave(object sender, EventArgs e)
        {
            if (presetList.SelectedIndex == -1)
                return;

            if (presetIsLoading)
                return;

            if (presetNameTextBox.GetType() != typeof(TextBox) && presetNameTextBox.GetType() != typeof(CustomTextBox))
                return;

            setPresetChanged();
        }

        private void assignHotkeyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (presetList.SelectedIndex == -1)
                return;

            useHotkeyForSelectedTracksCheckBox.Enable(assignHotkeyCheckBox.Checked);

            if (presetIsLoading)
                return;

            if (assignHotkeyCheckBox.Checked)
                reportPresetsWithHotkeysCount++;
            else //if (!assignHotkeyCheckBox.Checked)
                reportPresetsWithHotkeysCount--;

            setPresetChanged();
        }

        private void assignHotkeyCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!assignHotkeyCheckBox.IsEnabled())
                return;

            assignHotkeyCheckBox.Checked = !assignHotkeyCheckBox.Checked;
        }

        private void useHotkeyForSelectedTracksCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (presetList.SelectedIndex == -1)
                return;

            setPresetChanged();
        }

        private void useHotkeyForSelectedTracksCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!useHotkeyForSelectedTracksCheckBox.IsEnabled())
                return;

            useHotkeyForSelectedTracksCheckBox.Checked = !useHotkeyForSelectedTracksCheckBox.Checked;
        }

        private void previewTable_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && rows != null)
            {
                if (e.ColumnIndex == artworkField)
                    return;

                bool nosorting = true;
                for (var i = 0; i < previewTable.ColumnCount; i++)
                    if (previewTable.Columns[i].HeaderCell.SortGlyphDirection != SortOrder.None)
                        nosorting = false;

                bool ascending = true;
                if (nosorting && e.ColumnIndex == 0)
                    ascending = false;
                else if (previewTable.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                    ascending = false;

                for (var i = 0; i < previewTable.ColumnCount; i++)
                    previewTable.Columns[i].HeaderCell.SortGlyphDirection = SortOrder.None;

                var comparer = new DataGridViewBoundColumnsComparer
                {
                    ResultTypes = columnTypes,
                    ComparedColumnIndex = e.ColumnIndex,
                    Ascending = ascending,
                };


                SetStatusBarText(this, LibraryReportsSbText + " (" + SbSorting + ")", false);
                rows.Sort(comparer);
                SetStatusBarText(this, string.Empty, false);


                int firstDisplayedRow = previewTable.FirstDisplayedScrollingRowIndex;

                source.ResetBindings(false);

                for (int i = 0; i < previewTable.RowCount; i++)
                    previewTable.Rows[i].DefaultCellStyle.WrapMode = DataGridViewTriState.False;

                previewTable.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);

                for (int i = 0; i < previewTable.ColumnCount; i++)
                {
                    if (previewTable.Columns[i].Width > PreviewTableColumnMaximumWidth)
                        previewTable.Columns[i].Width = PreviewTableColumnMaximumWidth;
                }

                for (int i = 0; i < previewTable.RowCount; i++)
                    previewTable.Rows[i].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                previewTable.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);

                for (int j = 0; j < previewTable.RowCount; j++)
                    previewTableFormatRow(previewTable, j);

                updateCustomScrollBars(previewTable);

                previewTable.FirstDisplayedScrollingRowIndex = firstDisplayedRow;

                if (ascending)
                    previewTable.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                else
                    previewTable.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Descending;
            }
        }

        private void resizeArtworkCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            newArtworkSizeUpDown.Enable(resizeArtworkCheckBox.Checked);
            setPresetChanged();
        }

        private void resizeArtworkCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!resizeArtworkCheckBox.IsEnabled())
                return;

            resizeArtworkCheckBox.Checked = !resizeArtworkCheckBox.Checked;
        }

        private void newArtworkSizeUpDown_ValueChanged(object sender, EventArgs e)
        {
            setPresetChanged();
        }

        private void sourceFieldComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sourceFieldComboBoxCustom.SelectedIndex == -1)
            {
                sourceFieldComboBoxCustom.Enable(false);
                labelSaveField.Enable(false);
                labelSaveToTag.Enable(false);
                labelFunctionId.Enable(false);

                destinationTagListCustom.Enable(false);
                idTextBox.Enable(false);
                clearIdButton.Enable(false);

                destinationTagListCustom.SelectedIndex = -1;
                idTextBox.Text = string.Empty;


                operationComboBoxCustom.SelectedIndex = 0;
                mulDivFactorComboBoxCustom.SelectedIndex = 0;
                precisionDigitsComboBoxCustom.SelectedIndex = 0;
                appendTextBox.Text = string.Empty;

                operationComboBoxCustom.Enable(false);
                mulDivFactorComboBoxCustom.Enable(false);
                precisionDigitsComboBoxCustom.Enable(false);
                appendTextBox.Enable(false);
                roundToLabel.Enable(false);
                digitsLabel.Enable(false);
                appendLabel.Enable(false);

                return;
            }


            sourceFieldComboBoxCustom.Enable(true);
            labelSaveField.Enable(true);
            labelSaveToTag.Enable(true);
            labelFunctionId.Enable(true);

            destinationTagListCustom.Enable(true);
            idTextBox.Enable(true);
            clearIdButton.Enable(true);

            operationComboBoxCustom.Enable(true);
            mulDivFactorComboBoxCustom.Enable(true);
            precisionDigitsComboBoxCustom.Enable(true);
            appendTextBox.Enable(true);
            roundToLabel.Enable(true);
            digitsLabel.Enable(true);
            appendLabel.Enable(true);

            sourceFieldComboBoxIndexChanging = true;
            destinationTagListCustom.Text = savedDestinationTagsNames[sourceFieldComboBoxCustom.SelectedIndex];
            idTextBox.Text = savedFunctionIds[sourceFieldComboBoxCustom.SelectedIndex];
            sourceFieldComboBoxIndexChanging = false;


            operationComboBoxCustom.SelectedIndex = operations[sourceFieldComboBoxCustom.SelectedIndex];
            mulDivFactorComboBoxCustom.Text = mulDivFactors[sourceFieldComboBoxCustom.SelectedIndex];
            precisionDigitsComboBoxCustom.Text = precisionDigits[sourceFieldComboBoxCustom.SelectedIndex];
            appendTextBox.Text = appendTexts[sourceFieldComboBoxCustom.SelectedIndex];
        }

        private void totalsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            setPresetChanged();
        }

        private void totalsCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!totalsCheckBox.IsEnabled())
                return;

            totalsCheckBox.Checked = !totalsCheckBox.Checked;
        }

        private void operationComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sourceFieldComboBoxCustom.SelectedIndex == -1)
            {
                operationComboBoxCustom.SelectedIndex = 0;
            }
            else
            {
                operations[sourceFieldComboBoxCustom.SelectedIndex] = operationComboBoxCustom.SelectedIndex;

                if (!sourceFieldComboBoxIndexChanging)
                    setPresetChanged();
            }
        }

        internal static string TryParseUint(string textValue, uint defaultValue)
        {
            if (textValue.Length == 0)
                return defaultValue.ToString("F0");

            if (!uint.TryParse(textValue, out var value))
                return TryParseUint(textValue.Substring(0, textValue.Length - 1), defaultValue);
            else
                return value.ToString("F0");
        }

        private void mulDivFactorComboBox_TextChanged(object sender, EventArgs e)
        {
            if (sourceFieldComboBoxCustom.SelectedIndex == -1)
            {
                mulDivFactorComboBoxCustom.SelectedIndex = 0;
            }
            else
            {
                switch (mulDivFactorComboBoxCustom.Text)
                {
                    case "":
                        mulDivFactorComboBoxCustom.SelectedIndex = 0;
                        mulDivFactors[sourceFieldComboBoxCustom.SelectedIndex] = mulDivFactorComboBoxCustom.Items[0] as string;

                        break;
                    case "1 (ignore)":
                    case "1 (игнорировать)":
                    case "1000 (K)":
                    case "1000000 (M)":
                    case "1024 (K)":
                    case "1048576 (M)":
                        mulDivFactors[sourceFieldComboBoxCustom.SelectedIndex] = mulDivFactorComboBoxCustom.Text;

                        break;
                    default:
                        var mulDivFactorText = TryParseUint(mulDivFactorComboBoxCustom.Text, 1);
                        if (mulDivFactorComboBoxCustom.Text != mulDivFactorText)
                        {
                            mulDivFactorComboBoxCustom.Text = mulDivFactorText;
                            mulDivFactorComboBoxCustom.SelectionStart = mulDivFactorText.Length;
                            mulDivFactorComboBoxCustom.SelectionLength = 0;
                        }

                        mulDivFactors[sourceFieldComboBoxCustom.SelectedIndex] = mulDivFactorComboBoxCustom.Text;

                        break;
                }
            }

            if (!sourceFieldComboBoxIndexChanging)
                setPresetChanged();
        }

        private void precisionDigitsComboBox_TextChanged(object sender, EventArgs e)
        {
            if (sourceFieldComboBoxCustom.SelectedIndex == -1)
            {
                precisionDigitsComboBoxCustom.SelectedIndex = 0;
            }
            else
            {
                switch (precisionDigitsComboBoxCustom.Text)
                {
                    case "":
                        precisionDigitsComboBoxCustom.SelectedIndex = 0;
                        precisionDigits[sourceFieldComboBoxCustom.SelectedIndex] = precisionDigitsComboBoxCustom.Items[0] as string;

                        break;
                    case "(don't round)":
                    case "(не округлять)":
                        precisionDigits[sourceFieldComboBoxCustom.SelectedIndex] = precisionDigitsComboBoxCustom.Items[0] as string;

                        break;
                    default:
                        var precisionDigitsText = TryParseUint(precisionDigitsComboBoxCustom.Text, 0);
                        if (precisionDigitsComboBoxCustom.Text != precisionDigitsText)
                        {
                            precisionDigitsComboBoxCustom.Text = precisionDigitsText;
                            precisionDigitsComboBoxCustom.SelectionStart = precisionDigitsText.Length;
                            precisionDigitsComboBoxCustom.SelectionLength = 0;
                        }

                        precisionDigits[sourceFieldComboBoxCustom.SelectedIndex] = precisionDigitsComboBoxCustom.Text;

                        break;
                }
            }

            if (!sourceFieldComboBoxIndexChanging)
                setPresetChanged();
        }

        private void appendTextBox_TextChanged(object sender, EventArgs e)
        {
            if (sourceFieldComboBoxCustom.SelectedIndex == -1)
            {
                appendTextBox.Text = string.Empty;
            }
            else
            {
                appendTexts[sourceFieldComboBoxCustom.SelectedIndex] = appendTextBox.Text;
            }

            if (!sourceFieldComboBoxIndexChanging)
                setPresetChanged();
        }

        private void useAnotherPresetAsSourceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (useAnotherPresetAsSourceComboBox.SelectedIndex == -1)
                clearUseAnotherPresetButton.Enable(false);
            else
                clearUseAnotherPresetButton.Enable(true);

            if (presetIsLoading)
                return;

            updatePreset();
            setPresetChanged();

            useAnotherPresetAsSourceComboBoxCustom.Refresh();
            presetList.Refresh();
        }

        private void clearUseAnotherPresetButton_Click(object sender, EventArgs e)
        {
            useAnotherPresetAsSourceComboBox.SelectedIndex = -1;
        }

        private void previewTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex != artworkField && ModifierKeys == Keys.Control)
            {
                conditionFieldListCustom.SelectedIndex = e.ColumnIndex;
                comparedFieldListCustom.Text = previewTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                comparedFieldList_TextChanged(null, null);
            }
        }

        private void previewTable_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (e.Column.Index == artworkField)
            {
                for (var i = 0; i < previewTable.RowCount; i++)
                    previewTable.Rows[i].MinimumHeight = e.Column.Width;

                previewTable.AutoResizeRows();
            }

            updateCustomScrollBars(previewTable);
        }

        private void previewTable_RowHeightChanged(object sender, DataGridViewRowEventArgs e)
        {
            if (artworkField == -1 && ModifierKeys == Keys.Shift)
            {
                previewTable.AutoResizeRows();
            }
            else if (artworkField == -1 && ModifierKeys == Keys.Alt)
            {
                for (var i = 0; i < previewTable.RowCount; i++)
                    if (i != e.Row.Index && previewTable.Rows[i].Height < e.Row.Height)
                        previewTable.Rows[i].Height = e.Row.Height;
            }

            updateCustomScrollBars(previewTable);
        }

        private void smartOperationCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!smartOperationCheckBox.IsEnabled())
                return;

            smartOperationCheckBox.Checked = !smartOperationCheckBox.Checked;
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            var settings = new QuickSettings(TagToolsPlugin);
            Display(settings, this, true);
            presetList_ItemCheck(null, null); //Let's refresh auto-apply warning
        }

        private void openReportCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!openReportCheckBox.IsEnabled())
                return;

            openReportCheckBox.Checked = !openReportCheckBox.Checked;
        }

        private void formatComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedPreset != null && !presetIsLoading)
            {
                selectedPreset.fileFormatIndex = (LrReportFormat)formatComboBoxCustom.SelectedIndex + 1;
                setPresetChanged();
            }
        }

        private void hidePreviewCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (hidePreviewCheckBox.IsEnabled())
                hidePreviewCheckBox.Checked = !hidePreviewCheckBox.Checked;
        }

        private void hidePreviewCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            previewTable.Visible = !hidePreviewCheckBox.Checked;
            SavedSettings.hideLrPreview = hidePreviewCheckBox.Checked;
        }
    }

    internal abstract class ExportedDocument
    {
        internal protected System.IO.FileStream stream;
        internal protected Plugin plugin;
        internal protected string header;
        internal protected Encoding unicode = Encoding.UTF8;
        internal protected string text = string.Empty;
        internal protected byte[] buffer;

        internal protected virtual void getHeader()
        {
            text = string.Empty;
        }

        internal protected virtual void getFooter()
        {
            text = string.Empty;
        }

        internal ExportedDocument(System.IO.FileStream newStream, Plugin plugin, string presetName)
        {
            stream = newStream;
            this.plugin = plugin;
            header = presetName;

            //Write UTF-8 encoding mark
            buffer = unicode.GetPreamble();
            stream.Write(buffer, 0, buffer.Length);
        }

        internal string getImageName(string base64Hash)
        {
            //base64 = Regex.Replace(base64, @"\\", "_");
            //base64 = Regex.Replace(base64, @"/", "_");
            //base64 = Regex.Replace(base64, @"\:", "_");

            return base64Hash;
        }

        internal virtual void writeHeader()
        {
            getHeader();
            buffer = unicode.GetBytes(text);
            text = string.Empty;
            stream.Write(buffer, 0, buffer.Length);
        }

        internal void writeFooter()
        {
            getFooter();
            buffer = unicode.GetBytes(text);
            text = string.Empty;
            stream.Write(buffer, 0, buffer.Length);
        }

        internal abstract void addCellToRow(string cell, string cellName, bool rightAlign, bool dontOutput1, bool dontOutput2);
        internal abstract void addCellToRow(Bitmap cell, string cellName, string imageBase64, int width, int height);

        internal protected abstract void getRow(int height);

        internal virtual void writeRow(int height)
        {
            getRow(height);
            buffer = unicode.GetBytes(text);
            text = string.Empty;
            stream.Write(buffer, 0, buffer.Length);
        }

        internal void close()
        {
            stream.Close();
        }

        internal virtual void beginAlbumArtist(string albumArtist, int columnsCount)
        {
            //Nothing...
        }

        internal virtual void beginAlbum(string album, Bitmap artwork, string artworkBase64, int albumTrackCount)
        {
            //Nothing...
        }
    }

    internal class TextDocument : ExportedDocument
    {
        internal TextDocument(System.IO.FileStream newStream, Plugin plugin, string presetName)
            : base(newStream, plugin, presetName)
        {
        }

        internal override void addCellToRow(string cell, string cellName, bool rightAlign, bool dontOutput1, bool dontOutput2)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text += "\t" + cell;
            }
            else
            {
                text += cell;
            }
        }

        internal override void addCellToRow(Bitmap cell, string cellName, string imageBase64, int width, int height)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text += "\tARTWORK";
            }
            else
            {
                text += "ARTWORK";
            }
        }

        internal protected override void getRow(int height)
        {
            text += "\r\n";
        }
    }

    internal class CsvDocument : TextDocument
    {
        internal CsvDocument(System.IO.FileStream newStream, Plugin plugin, string presetName)
            : base(newStream, plugin, presetName)
        {
        }

        private readonly string delimiter = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;

        internal override void addCellToRow(string cell, string cellName, bool rightAlign, bool dontOutput1, bool dontOutput2)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text += (delimiter + "\"" + cell.Replace("\"", "\"\"") + "\"");
            }
            else
            {
                text += "\"" + cell.Replace("\"", "\"\"") + "\"";
            }
        }

        internal override void addCellToRow(Bitmap cell, string cellName, string imageBase64, int width, int height)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text += (delimiter + "\"ARTWORK\"");
            }
            else
            {
                text += "\"ARTWORK\"";
            }
        }
    }

    internal class M3UDocument : TextDocument
    {
        internal M3UDocument(System.IO.FileStream newStream, Plugin plugin, string presetName)
            : base(newStream, plugin, presetName)
        {
        }

        internal override void addCellToRow(string cell, string cellName, bool rightAlign, bool dontOutput1, bool dontOutput2)
        {
            if (cellName == UrlTagName)
            {
                base.addCellToRow(cell, cellName, rightAlign, dontOutput1, dontOutput2);
            }
        }
    }

    internal class HtmlTable : ExportedDocument
    {
        internal protected string fileDirectoryPath;
        internal protected string imagesDirectoryName;
        internal protected const string defaultImageName = "Missing Artwork.png";
        internal protected bool defaultImageWasExported = false;

        internal HtmlTable(System.IO.FileStream newStream, Plugin plugin, string presetName, string fileDirectoryPath, string imagesDirectoryName)
            : base(newStream, plugin, presetName)
        {
            this.fileDirectoryPath = fileDirectoryPath;
            this.imagesDirectoryName = imagesDirectoryName;
        }

        internal protected override void getHeader()
        {
            text = "<html><head></head><header>" + header + "</header><body><table border=1>";
        }

        internal protected override void getFooter()
        {
            text = "</table></body></html>";
        }

        internal override void addCellToRow(string cell, string cellName, bool rightAlign, bool dontOutput1, bool dontOutput2)
        {
            text += "<td" + (rightAlign ? " align='right'" : string.Empty) + ">" + cell + "</td>";
        }

        internal override void addCellToRow(Bitmap cell, string cellName, string imageBase64Hash, int width, int height)
        {
            var imageName = getImageName(imageBase64Hash) + ".jpg";
            cell.Save(fileDirectoryPath + imagesDirectoryName + @"\" + imageName, System.Drawing.Imaging.ImageFormat.Jpeg);

            text += "<td height=" + height + " width=" + width + "> <img src=\"" + imagesDirectoryName + @"\" + imageName + "\" > </td>";
        }

        internal protected override void getRow(int height)
        {
            text = "<tr>" + text + " </tr>";
        }
    }

    internal class HtmlDocumentAlbumGrid : HtmlTable
    {
        internal protected bool? alternateRow;

        internal HtmlDocumentAlbumGrid(System.IO.FileStream newStream, Plugin plugin, string presetName, string fileDirectoryPath, string imagesDirectoryName)
            : base(newStream, plugin, presetName, fileDirectoryPath, imagesDirectoryName)
        {
        }

        internal void writeHeader(string color, int rowImageCount, int imageWidth, int imageHeight, int labelHeight, int fontSize)
        {
            var stylesheet = new System.IO.FileStream(fileDirectoryPath + imagesDirectoryName + @"\stylesheet.css", System.IO.FileMode.Create);

            //Write UTF-8 encoding mark
            buffer = unicode.GetPreamble();
            stylesheet.Write(buffer, 0, buffer.Length);

            buffer = unicode.GetBytes(".header {color:" + color + ";width:auto;height:auto;font-size:" + (fontSize * 3) + "px;font-weight:400;font-style:normal;font-family:Arial, sans-serif;white-space:nowrap;text-align:center;}");
            stylesheet.Write(buffer, 0, buffer.Length);
            buffer = unicode.GetBytes("table {border:solid white;} td {border:solid white;}");
            stylesheet.Write(buffer, 0, buffer.Length);
            buffer = unicode.GetBytes(".image {width:" + imageWidth + "px;height:" + imageHeight + "px;}");
            stylesheet.Write(buffer, 0, buffer.Length);
            buffer = unicode.GetBytes(".text {width:" + imageWidth + "px;height:" + labelHeight + "px;color:" + color + ";font-size:" + fontSize + "px;font-weight:400;font-style:normal;font-family:Arial, sans-serif;white-space:wrap;text-align:center;");
            stylesheet.Write(buffer, 0, buffer.Length);

            stylesheet.Close();

            base.writeHeader();
        }

        internal protected override void getHeader()
        {
            text = "<html><head><link rel=Stylesheet href=\"" + imagesDirectoryName + "\\stylesheet.css\">"
            + " </head><body><table class='header'><tr><td>" + header + "</td></tr><tr><td><table>";
        }

        internal protected override void getFooter()
        {
            text = "</table></td></tr></table></body></html>";
        }

        internal void addCellToRow(Bitmap cell, string cellName, string imageBase64Hash)
        {
            var imageName = getImageName(imageBase64Hash) + ".jpg";
            cell.Save(fileDirectoryPath + imagesDirectoryName + @"\" + imageName, System.Drawing.Imaging.ImageFormat.Jpeg);

            text += "<td><table><tr><td class=\"image\" width=" + cell.Width + " height=" + cell.Height + "><img src=\"" + imagesDirectoryName + @"\" + imageName + "\" ></td>" +
                "<tr><td class=\"text\">" + cellName + "</td></tr></table>";
        }

        internal override void writeRow(int height)
        {
            if (alternateRow == null)
                alternateRow = false;
            else
                alternateRow = !alternateRow;

            base.writeRow(height);
        }
    }

    internal class HtmlDocument : HtmlTable
    {
        internal protected bool? alternateRow;

        internal HtmlDocument(System.IO.FileStream newStream, Plugin plugin, string presetName, string fileDirectoryPath, string imagesDirectoryName)
            : base(newStream, plugin, presetName, fileDirectoryPath, imagesDirectoryName)
        {
        }

        internal override void writeHeader()
        {
            var stylesheet = new System.IO.FileStream(fileDirectoryPath + imagesDirectoryName + @"\stylesheet.css", System.IO.FileMode.Create);

            //Write UTF-8 encoding mark
            buffer = unicode.GetPreamble();
            stylesheet.Write(buffer, 0, buffer.Length);

            buffer = unicode.GetBytes("td {color:#050505;font-size:11.0pt;font-weight:400;font-style:normal;font-family:Arial, sans-serif;white-space:nowrap;}");
            stylesheet.Write(buffer, 0, buffer.Length);
            buffer = unicode.GetBytes(".xl1 {color:#0070C0;font-size:16.0pt;font-weight:700;font-family:Arial, sans-serif;}");
            stylesheet.Write(buffer, 0, buffer.Length);
            buffer = unicode.GetBytes(".xl2 {color:white;font-size:12.0pt;border-top:1.0pt solid windowtext;border-right:1pt solid windowtext;border-bottom:1pt solid windowtext;border-left:1pt solid windowtext;background:#95B3D7;}");
            stylesheet.Write(buffer, 0, buffer.Length);
            buffer = unicode.GetBytes(".xl3 {font-size:12.0pt;border-top:1pt solid windowtext;border-right:1pt solid windowtext;border-bottom:1.0pt solid windowtext;border-left:1.0pt solid windowtext;background:#DCE6F1;}");
            stylesheet.Write(buffer, 0, buffer.Length);
            buffer = unicode.GetBytes(".xl0 {color:white;font-size:12.0pt;font-weight:700;border-top:1.0pt solid windowtext;border-right:1pt solid windowtext;border-bottom:1pt solid windowtext;border-left:1.0pt solid windowtext;background:#4F81BD;}");
            stylesheet.Write(buffer, 0, buffer.Length);
            buffer = unicode.GetBytes(".xl5 {color:#0070A0;font-size:16.0pt;font-weight:700;border-top:0pt solid windowtext;border-right:0pt solid windowtext;border-bottom:2pt solid windowtext;border-left:0pt solid windowtext}");
            stylesheet.Write(buffer, 0, buffer.Length);
            buffer = unicode.GetBytes(".xl6 {color:black;font-size:12.0pt;font-weight:400;padding-top:1px;vertical-align:top;border-top:1.0pt solid windowtext;border-right:0pt solid windowtext;border-bottom:0pt solid windowtext;border-left:0pt solid windowtext}");
            stylesheet.Write(buffer, 0, buffer.Length);

            stylesheet.Close();

            base.writeHeader();

        }

        internal protected override void getHeader()
        {
            text = "<html><head><link rel=Stylesheet href=\"" + imagesDirectoryName + "\\stylesheet.css\">"
                + " </head><header class=xl1>" + header + "</header><body>" +
                "<table style='border-collapse:collapse'>"
                + "";
        }

        internal override void addCellToRow(string cell, string cellName, bool rightAlign, bool dontOutput1, bool dontOutput2)
        {
            string rowClass;

            if (alternateRow == null)
                rowClass = "xl0";
            else if (alternateRow == true)
                rowClass = "xl2";
            else
                rowClass = "xl3";


            text += "<td class=" + rowClass + (rightAlign ? " align='right'" : string.Empty) + ">" + cell + "</td>";
        }

        internal override void addCellToRow(Bitmap cell, string cellName, string imageBase64Hash, int width, int height)
        {
            string rowClass;

            if (alternateRow == null)
                rowClass = "xl0";
            else if (alternateRow == true)
                rowClass = "xl2";
            else
                rowClass = "xl3";

            var imageName = getImageName(imageBase64Hash) + ".jpg";
            cell.Save(fileDirectoryPath + imagesDirectoryName + @"\" + imageName, System.Drawing.Imaging.ImageFormat.Jpeg);

            text += "<td class=" + rowClass + " height=" + height + " width=" + width + "> <img src=\"" + imagesDirectoryName + @"\" + imageName + "\" > </td>";
        }

        internal override void writeRow(int height)
        {
            if (alternateRow == null)
                alternateRow = false;
            else
                alternateRow = !alternateRow;

            base.writeRow(height);
        }
    }

    internal class HtmlDocumentByAlbum : HtmlDocument
    {
        internal HtmlDocumentByAlbum(System.IO.FileStream newStream, Plugin plugin, string presetName, string fileDirectoryPath, string imagesDirectoryName)
            : base(newStream, plugin, presetName, fileDirectoryPath, imagesDirectoryName)
        {
        }

        internal override void beginAlbumArtist(string albumArtist, int columnsCount)
        {
            text = "<tr> <td colspan=" + columnsCount + " class=xl5>" + albumArtist + "</td> </tr>";
        }

        internal override void beginAlbum(string album, Bitmap artwork, string imageBase64Hash, int albumTrackCount)
        {
            var imageName = getImageName(imageBase64Hash) + ".jpg";
            artwork.Save(fileDirectoryPath + imagesDirectoryName + @"\" + imageName, System.Drawing.Imaging.ImageFormat.Jpeg);

            text += "<td rowspan=" + albumTrackCount + " class=xl6> <img src=\"" + imagesDirectoryName + @"\" + imageName + "\" > <br>" + album + "</td>";
            //text += "<td class=xl6> <img src=\"" + imagesDirectoryName + @"\" + imageName + "\" > <br>" + album + "</td>";
        }

        internal override void addCellToRow(string cell, string cellName, bool rightAlign, bool dontOutput1, bool dontOutput2)
        {
            if (dontOutput1 || dontOutput2)
                return;

            base.addCellToRow(cell, cellName, rightAlign, dontOutput1, dontOutput2);
        }

        internal override void addCellToRow(Bitmap cell, string cellName, string imageBase64, int width, int height)
        {
        }
    }

    internal class HtmlDocumentCDBooklet : HtmlTable
    {
        internal protected int backgroundSize;

        internal protected float trackFontSize;
        internal protected float albumArtistsFontSize;
        internal protected float albumFontSize;
        internal protected float fontScaling;

        internal protected string trackTextColor;
        internal protected string albumArtistTextColor;
        internal protected string albumArtistTextOutlineColor;
        internal protected string albumTextColor;
        internal protected string albumTextOutlineColor;
        internal protected string backgroundColor;

        internal HtmlDocumentCDBooklet(System.IO.FileStream newStream, Plugin plugin, string presetName, string fileDirectoryPath, string imagesDirectoryName)
            : base(newStream, plugin, presetName, fileDirectoryPath, imagesDirectoryName)
        {
        }

        internal void writeHeader(int size, Color bitmapAverageColor, Bitmap scaledPic, SortedDictionary<string, List<string>> albumArtistsAlbums, int trackCount)
        {
            backgroundSize = size;

            trackFontSize = 8.0f;
            albumArtistsFontSize = 15.0f;
            albumFontSize = 12.0f;

            fontScaling = 1;
            if (trackCount > 25) //---
                fontScaling = (1f - (float)(trackCount - 25) / trackCount);

            trackFontSize *= fontScaling;

            var trackFontSizeString = trackFontSize.ToString().Replace(',', '.');


            int albumArtistsRowCount = 0;
            int albumsRowCount = 0;
            foreach (var albumArtistAlbums in albumArtistsAlbums)
            {
                albumArtistsRowCount += (albumArtistAlbums.Key.Length / 55) + (albumArtistAlbums.Key.Length % 55 > 0 ? 1 : 0);

                foreach (string album in albumArtistAlbums.Value)
                    albumsRowCount += (album.Length / 68) + (album.Length % 68 > 0 ? 1 : 0);
            }

            if (albumArtistsRowCount > 10) //---
                albumArtistsFontSize *= 1f - (float)(albumArtistsRowCount - 10) / albumArtistsRowCount;

            var albumArtistsFontSizeString = albumArtistsFontSize.ToString().Replace(',', '.');

            if (albumsRowCount > 10) //---
                albumFontSize *= 1f - (float)(albumsRowCount - 10) / albumsRowCount;

            var albumFontSizeString = albumFontSize.ToString().Replace(',', '.');

            if (bitmapAverageColor.R + bitmapAverageColor.G + bitmapAverageColor.B < 127 * 3)
            {
                trackTextColor = "#ffffff";
                albumArtistTextColor = "#e5d179";
                albumArtistTextOutlineColor = "#917b1c";
                albumTextColor = "#8dc6e9";
                albumTextOutlineColor = "#1c6491";
            }
            else
            {
                trackTextColor = "#000000";
                albumArtistTextColor = "#917b1c";
                albumArtistTextOutlineColor = "#e5d179";
                albumTextColor = "#1c6491";
                albumTextOutlineColor = "#8dc6e9";
            }

            backgroundColor = "#";
            backgroundColor += bitmapAverageColor.R.ToString("X2");
            backgroundColor += bitmapAverageColor.G.ToString("X2");
            backgroundColor += bitmapAverageColor.B.ToString("X2");

            var stylesheet = new System.IO.FileStream(fileDirectoryPath + imagesDirectoryName + @"\stylesheet.css", System.IO.FileMode.Create);

            //Write UTF-8 encoding mark
            buffer = unicode.GetPreamble();
            stylesheet.Write(buffer, 0, buffer.Length);

            buffer = unicode.GetBytes(".xl0 {padding-top:25px;vertical-align:top;background-color:" + backgroundColor + ";}");
            stylesheet.Write(buffer, 0, buffer.Length);
            buffer = unicode.GetBytes(".xl1 {color:" + trackTextColor + ";font-size:" + trackFontSizeString + "pt;font-weight:200;font-family:Arial, sans-serif;text-align:left;padding-left:30px;padding-right:20px;width:430px;}");
            stylesheet.Write(buffer, 0, buffer.Length);
            buffer = unicode.GetBytes(".xl2 {color:" + trackTextColor + ";font-size:" + trackFontSizeString + "pt;font-weight:200;font-family:Arial, sans-serif;text-align:left;}");
            stylesheet.Write(buffer, 0, buffer.Length);
            buffer = unicode.GetBytes(".xl3 {color:" + albumArtistTextColor + ";font-size:" + albumArtistsFontSizeString + "pt;font-weight:700;font-family:Arial, sans-serif;text-align:center;text-shadow:0px 0px 3px " + albumArtistTextOutlineColor + ";}");
            stylesheet.Write(buffer, 0, buffer.Length);
            buffer = unicode.GetBytes(".xl4 {color:" + albumTextColor + ";font-size:" + albumFontSizeString + "pt;font-weight:700;font-family:Arial, sans-serif;text-align:center;text-shadow:0px 0px 3px " + albumTextOutlineColor + ";}");
            stylesheet.Write(buffer, 0, buffer.Length);

            stylesheet.Close();

            scaledPic.Save(fileDirectoryPath + imagesDirectoryName + @"\bg1.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

            base.writeHeader();
        }

        internal protected override void getHeader()
        {
            text = "<html><head>"
                + "<link rel=Stylesheet href='" + imagesDirectoryName + "/stylesheet.css'>"
                + "</head>"
                + "<body><table><tr>"
                + "<td class=xl0 width=" + backgroundSize + " height=" + backgroundSize + ">"
                + "<table>";
        }

        internal protected void getFooter(SortedDictionary<string, List<string>> albumArtistsAlbums)
        {
            text = "</table></td><td width=" + backgroundSize + " height=" + backgroundSize
                + " background='" + imagesDirectoryName + "/bg1.jpg' style='background-repeat:no-repeat;background-position:center center;'><table>";

            foreach (var albumArtist in albumArtistsAlbums)
            {
                text += "<tr><td class=xl3 width=" + backgroundSize + ">" + albumArtist.Key + "</td></tr>";
                foreach (var album in albumArtist.Value)
                {
                    text += "<tr><td class=xl4 width=" + backgroundSize + ">" + album + "</td></tr>";
                }
            }

            text += "</table></td></tr></table></body>"
                + "</html>";
        }

        internal void writeFooter(SortedDictionary<string, List<string>> albumArtistsAlbums)
        {
            getFooter(albumArtistsAlbums);
            buffer = unicode.GetBytes(text);
            text = string.Empty;
            stream.Write(buffer, 0, buffer.Length);
        }

        internal void addTrack(string seqNum, string albumArtist, string album, string title, string duration)
        {
            string trimmedSeqNum = seqNum.Substring(seqNum.Length - 2);
            string unformattedText = trimmedSeqNum + "." + albumArtist + " ◆ " + album + " ◆ " + title;

            int criticalLength = (int)Math.Round(85 / fontScaling);
            int maxLength = (int)Math.Round(82 / fontScaling);
            int maxAlbumArtistLength = (int)Math.Round(20 / fontScaling);
            int maxAlbumLength = (int)Math.Round(25 / fontScaling);

            int albumArtistLength = 0;
            if (unformattedText.Length > criticalLength)
            {
                if (albumArtist != null)
                {
                    albumArtistLength = albumArtist.Length - 3 >= maxAlbumArtistLength ? maxAlbumArtistLength : albumArtist.Length; //-3 is text " ◆ "

                    if (albumArtist.Length > albumArtistLength)
                        albumArtist = albumArtist.Substring(0, albumArtistLength) + "...";
                    else
                        albumArtistLength = albumArtist.Length - 3; //For "...". See below.

                    albumArtistLength += 6; //+3 for "..."; +3 for " ◆ "
                }


                int albumLength = 0;
                if (album != null)
                {
                    albumLength = maxAlbumLength + maxAlbumArtistLength - albumArtist.Length - 3 >= album.Length ? //-3 is text " ◆ "
                        album.Length : maxAlbumLength + maxAlbumArtistLength - albumArtist.Length;

                    if (album.Length > albumLength)
                        album = album.Substring(0, albumLength) + "...";
                    else
                        albumLength = album.Length - 3; //For "...". See below.

                    albumLength += 6; //+3 for "..."; +3 for " ◆ "
                }


                int titleLength = maxLength - albumLength - albumArtistLength - 4 >= title.Length ? ///-4 is track#
                    title.Length : maxLength - albumLength - albumArtistLength - 4;

                if (title.Length > titleLength)
                    title = title.Substring(0, titleLength) + "...";
            }

            text = "<td class=xl1>" + trimmedSeqNum + ".";

            if (albumArtist != null)
                text += " " + albumArtist + " &#x25C6;";

            if (album != null)
                text += " " + album + " &#x25C6;";

            text += " " + title + "</td>";

            text += "<td class=xl2>" + duration + "</td>";
        }
    }

    partial class Plugin
    {
        internal void LrPreset1EventHandler(object sender, EventArgs e)
        {
            LibraryReports.ApplyReportPresetByHotkey(1);
        }

        internal void LrPreset2EventHandler(object sender, EventArgs e)
        {
            LibraryReports.ApplyReportPresetByHotkey(2);
        }

        internal void LrPreset3EventHandler(object sender, EventArgs e)
        {
            LibraryReports.ApplyReportPresetByHotkey(3);
        }

        internal void LrPreset4EventHandler(object sender, EventArgs e)
        {
            LibraryReports.ApplyReportPresetByHotkey(4);
        }

        internal void LrPreset5EventHandler(object sender, EventArgs e)
        {
            LibraryReports.ApplyReportPresetByHotkey(5);
        }

        internal void LrPreset6EventHandler(object sender, EventArgs e)
        {
            LibraryReports.ApplyReportPresetByHotkey(6);
        }

        internal void LrPreset7EventHandler(object sender, EventArgs e)
        {
            LibraryReports.ApplyReportPresetByHotkey(7);
        }

        internal void LrPreset8EventHandler(object sender, EventArgs e)
        {
            LibraryReports.ApplyReportPresetByHotkey(8);
        }

        internal void LrPreset9EventHandler(object sender, EventArgs e)
        {
            LibraryReports.ApplyReportPresetByHotkey(9);
        }

        internal void LrPreset10EventHandler(object sender, EventArgs e)
        {
            LibraryReports.ApplyReportPresetByHotkey(10);
        }

        internal void LrPreset11EventHandler(object sender, EventArgs e)
        {
            LibraryReports.ApplyReportPresetByHotkey(11);
        }

        internal void LrPreset12EventHandler(object sender, EventArgs e)
        {
            LibraryReports.ApplyReportPresetByHotkey(12);
        }

        internal void LrPreset13EventHandler(object sender, EventArgs e)
        {
            LibraryReports.ApplyReportPresetByHotkey(13);
        }

        internal void LrPreset14EventHandler(object sender, EventArgs e)
        {
            LibraryReports.ApplyReportPresetByHotkey(14);
        }

        internal void LrPreset15EventHandler(object sender, EventArgs e)
        {
            LibraryReports.ApplyReportPresetByHotkey(15);
        }

        internal void LrPreset16EventHandler(object sender, EventArgs e)
        {
            LibraryReports.ApplyReportPresetByHotkey(16);
        }

        internal void LrPreset17EventHandler(object sender, EventArgs e)
        {
            LibraryReports.ApplyReportPresetByHotkey(17);
        }

        internal void LrPreset18EventHandler(object sender, EventArgs e)
        {
            LibraryReports.ApplyReportPresetByHotkey(18);
        }

        internal void LrPreset19EventHandler(object sender, EventArgs e)
        {
            LibraryReports.ApplyReportPresetByHotkey(19);
        }

        internal void LrPreset20EventHandler(object sender, EventArgs e)
        {
            LibraryReports.ApplyReportPresetByHotkey(20);
        }
    }
}
