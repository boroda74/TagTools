using ExtensionMethods;
using MusicBeePlugin.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class LibraryReports : PluginWindowTemplate
    {
        protected bool forceCloseForms = true;

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

        private Bitmap warningWide = null;
        private Bitmap errorWide = null;
        private Bitmap fatalErrorWide = null;
        private Bitmap errorFatalErrorWide = null;

        private Bitmap warning = null;
        private Bitmap error = null;
        private Bitmap fatalError = null;
        private Bitmap errorFatalError = null;

        private DataGridViewCellStyle headerCellStyle;

        private delegate void AddRowsToTable(List<string[]> rows);
        private delegate void UpdateTable();
        private AddRowsToTable addRowsToTable;
        private UpdateTable updateTable;

        private System.Threading.Timer periodicCacheClearingTimer;

        private void periodicCacheClearing(object state)//****
        {
            if ((PluginClosing || SavedSettings.dontShowLibraryReports || cachedPresetsTags.Count > PresetCacheCountThreshold)
                && !BackgroundTaskIsInProgress)
            {
                if (cachedPresetsTags.Count > PresetCacheCountCriticalThreshold)
                {
                    lastCachedAppliedPresetGuid = Guid.NewGuid();
                    cachedAppliedPresetGuids.Clear();

                    cachedPresetsTags.Clear();

                    cachedPresetsGroupingsTagIds.Clear();
                    cachedPresetsGroupingsPropIds.Clear();
                    cachedPresetsActualGroupingsTagIds.Clear();
                    cachedPresetsActualGroupingsPropIds.Clear();
                    cachedPresetsNativeTagNames.Clear();

                    cachedPresetsFilesActualComposedSplitGroupingTagsList.Clear();
                    cachedPresetsFilesActualGroupingTags.Clear();
                    cachedPresetsFilesActualGroupingTagsRaw.Clear();

                    clearArtworks();
                }
                else
                {
                    bool clearedSome = false;

                    foreach (var guid in cachedAppliedPresetGuids.Keys)
                    {
                        if (cachedPresetsTags.Count > TagsCacheClearingGroupingsCountThreshold || !AllLrPresetGuidsInUse.ContainsKey(guid))
                        {
                            clearedSome = true;

                            if (guid == lastCachedAppliedPresetGuid)
                                lastCachedAppliedPresetGuid = Guid.NewGuid();

                            cachedAppliedPresetGuids.Remove(guid);

                            cachedPresetsTags.Remove(guid);

                            cachedPresetsGroupingsTagIds.Remove(guid);
                            cachedPresetsGroupingsPropIds.Remove(guid);
                            cachedPresetsActualGroupingsTagIds.Remove(guid);
                            cachedPresetsActualGroupingsPropIds.Remove(guid);
                            cachedPresetsNativeTagNames.Remove(guid);
                            cachedPresetsFilesActualGroupingTags.Remove(guid);
                            cachedPresetsFilesActualGroupingTagsRaw.Remove(guid);
                            cachedPresetsFilesActualComposedSplitGroupingTagsList.Remove(guid);
                        }
                    }

                    if (clearedSome)
                        clearArtworks();
                }
            }

            if (PluginClosing || SavedSettings.dontShowLibraryReports
                || (!ReportPresetIdsAreInitialized && LibraryReportsCommandForHotkeys == null && LibraryReportsCommandForAutoApplying == null))
            {
                periodicCacheClearingTimer?.Dispose();
                periodicCacheClearingTimer = null;
            }
        }


        private static List<string> ProcessedReportDeletions = new List<string>();

        //Cached UI and events workarounds
        internal static Bitmap DefaultArtwork;
        internal static string DefaultArtworkHash;
        internal SortedDictionary<string, Bitmap> artworks = new SortedDictionary<string, Bitmap>();
        private Bitmap artwork; //For CD booklet export

        private Font totalsFont; //Preview table font for "Totals" cells
        private int[] maxWidths = null;

        private const int MaxColumnRepresentationLength = 80; //***
        private const int MaxExprRepresentationLength = 40;

        private const int PreviewTableColumnMinimumWidth = 75;
        private const int PreviewTableDefaultArtworkSize = 200;

        private static string AutoApplyText;
        private static string NowTickedText;

        private static string AutoApplyDisabledText;

        private static string UseAnotherPresetAsSourceToolTip;
        private static string UseAnotherPresetAsSourceIsSenselessToolTip;
        private static string UseAnotherPresetAsSourceIsInBrokenChainToolTip;
        private static string UseAnotherPresetAsSourceCheckPresetChainToolTip;

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

        private static string NoExpressionText;

        private bool ignoreCheckedPresetEvent = true;
        private int autoAppliedPresetCount;

        private string assignHotkeyCheckBoxText;
        private int reportPresetsWithHotkeysCount;
        private bool[] reportPresetHotkeyUsedSlots = new bool[MaximumNumberOfLrHotkeys];

        private bool unsavedChanges = false;
        private string buttonCloseToolTip;
        private int presetListLastSelectedIndex = -2;

        private bool ignoreSplitterMovedEvent = true;

        private ReportPreset selectedPreset = null;

        private bool presetIsLoading = false;
        private bool sourceFieldComboBoxIndexChanging = false;

        internal static volatile bool BackgroundTaskIsInProgress = false;

        //Not reliable. Make sure to call checkAllPresetChains() before use!
        private bool allPresetsCheckStatusIsSenseless;
        private bool allPresetsCheckStatusIsBroken;

        private bool resultsAreFiltered = false;
        private AggregatedTags previewedTags = null;
        private SortedDictionary<int, List<string>> previewedFilesActualComposedSplitGroupingTagsList = null;

        //Working locals
        internal ReportPreset appliedPreset;
        private SortedDictionary<Guid, bool> cachedAppliedPresetGuids = new SortedDictionary<Guid, bool>();
        private Guid lastCachedAppliedPresetGuid;
        internal ReportPreset[] reportPresets = null;
        private static List<Guid> PresetsProcessedByFunctionCacheUpdate = new List<Guid>(); //<ReportPreset.guid>


        private bool[] columnsRightAlignment; //Cache for preview table/exported report 
        private ResultType[] columnTypes; //Cache for preview table/exported report 


        private SortedDictionary<Guid, AggregatedTags> cachedPresetsTags = new SortedDictionary<Guid, AggregatedTags>();

        private SortedDictionary<Guid, MetaDataType[]> cachedPresetsGroupingsTagIds = new SortedDictionary<Guid, MetaDataType[]>();
        private SortedDictionary<Guid, FilePropertyType[]> cachedPresetsGroupingsPropIds = new SortedDictionary<Guid, FilePropertyType[]>();
        private SortedDictionary<Guid, MetaDataType[]> cachedPresetsActualGroupingsTagIds = new SortedDictionary<Guid, MetaDataType[]>();
        private SortedDictionary<Guid, FilePropertyType[]> cachedPresetsActualGroupingsPropIds = new SortedDictionary<Guid, FilePropertyType[]>();
        private SortedDictionary<Guid, string[]> cachedPresetsNativeTagNames = new SortedDictionary<Guid, string[]>();

        //<Preset GUID, <TrackId, Grouping tags[]>>
        private SortedDictionary<Guid, SortedDictionary<int, string[]>> cachedPresetsFilesActualGroupingTags = new SortedDictionary<Guid, SortedDictionary<int, string[]>>();
        //<Preset GUID, <TrackId, Grouping tags[]>>
        private SortedDictionary<Guid, SortedDictionary<int, string[]>> cachedPresetsFilesActualGroupingTagsRaw = new SortedDictionary<Guid, SortedDictionary<int, string[]>>();
        //<Preset GUID, <TrackId, List of <composed groupings>>>
        private SortedDictionary<Guid, SortedDictionary<int, List<string>>> cachedPresetsFilesActualComposedSplitGroupingTagsList = new SortedDictionary<Guid, SortedDictionary<int, List<string>>>();


        private string[] lastFiles = null;


        private Comparison comparison;
        private string comparedFieldText;

        private int conditionField = -1;
        private int comparedField = -1;

        private int artworkField = -1;
        private int sequenceNumberField = -1;

        private readonly List<MetaDataType> destinationTagIds = new List<MetaDataType>();

        internal static string PresetName = string.Empty;

        private string expressionBackup = string.Empty;
        private string splitterBackup = string.Empty;
        private bool trimValuesBackup = false;

        private bool? newColumn = false;
        private int tagsDataGridViewSelectedRow = -1;

        private PresetColumnAttributesDict groupings = new PresetColumnAttributesDict(); //Short ids, attributes (various expressions)


        //Working locals & UI preset caching
        private List<string> sortedShortIds = new List<string>(); //Short ids
        private Dictionary<string, List<string>> shortIdsExprs = new Dictionary<string, List<string>>(); //Short ids, expressions

        private ColumnAttributesDict groupingsDict = new ColumnAttributesDict();
        private ColumnAttributesDict functionsDict = new ColumnAttributesDict();

        private readonly List<string> savedFunctionIds = new List<string>();

        private List<int> operations = new List<int>();
        private List<string> mulDivFactors = new List<string>();
        private List<string> precisionDigits = new List<string>();
        private List<string> appendTexts = new List<string>();

        //UI preset caching
        private readonly List<string> savedDestinationTagsNames = new List<string>();
        private bool smartOperation;


        //Set timer for cache periodic cleanup in constructor
        internal LibraryReports()
        {
            //It's not GUI control, not a Form in this case
            //InitializeComponent();

            periodicCacheClearingTimer = new System.Threading.Timer(periodicCacheClearing, null, FunctionCacheClearingDelay, FunctionCacheClearingDelay);
        }

        internal LibraryReports(Plugin plugin) : base(plugin)
        {
            InitializeComponent();

        }

        protected override void initializeForm()
        {
            base.initializeForm();
            this.Enable(false, autoApplyPresetsLabel);


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
            errorWide = ReplaceBitmap(null, ErrorWide);
            fatalErrorWide = ReplaceBitmap(null, FatalErrorWide);
            errorFatalErrorWide = ReplaceBitmap(null, ErrorFatalErrorWide);

            warning = ReplaceBitmap(null, Warning);
            error = ReplaceBitmap(null, Error);
            fatalError = ReplaceBitmap(null, FatalError);
            errorFatalError = ReplaceBitmap(null, ErrorFatalError);

            //Setting control not standard properties
            //var heightField = presetList.GetType().GetField(
            //   "scaledListItemBordersHeight",
            //   BindingFlags.NonPublic | BindingFlags.Instance
            //);

            //var addedHeight = 4; //Some appropriate value, greater than the field's default of 2
            //heightField.SetValue(presetList, addedHeight); //Where "presetList" is your CheckedListBox

            previewTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;


            if (DontUseSplitter == null)
                DontUseSplitter = (multipleItemsSplitterComboBoxCustom.Items[0] as string).TrimStart(' ');

            //Setting themed images
            openReportCheckBoxPicture.Image = ReplaceBitmap(openReportCheckBoxPicture.Image, Window);

            clearIdButton.Image = ReplaceBitmap(clearIdButton.Image, ButtonRemoveImage);
            buttonSettings.Image = ReplaceBitmap(buttonSettings.Image, Gear);

            buttonClearExpression.Image = ReplaceBitmap(buttonClearExpression.Image, ButtonRemoveImage);


            //Clearing "unsaved changes" button image
            buttonClose.Image = Resources.transparent_15;
            buttonCloseToolTip = toolTip1.GetToolTip(buttonClose);
            toolTip1.SetToolTip(buttonClose, string.Empty);


            //Preparing "ticked presets" label text
            string entireText = autoApplyPresetsLabel.Text;
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


            //Clearing "source preset is missing" check box image
            useAnotherPresetAsSourceCheckBox.Image = Resources.transparent_15;


            //Saving buttonFilterResults tool tip & button label
            ButtonFilterResultsText = buttonLabels[buttonFilterResults];
            ButtonFilterResultsToolTip = toolTip1.GetToolTip(buttonFilterResults);


            //Saving useAnotherPresetAsSourceCheckBox tool tip
            string useAnotherPresetAsSourceFullToolTip = toolTip1.GetToolTip(useAnotherPresetAsSourceCheckBox).Replace("\n", string.Empty);
            UseAnotherPresetAsSourceToolTip = Regex.Replace(useAnotherPresetAsSourceFullToolTip, @"^((?:[^:])*?):\r((?:[^:])*?):\r((?:[^:])*?):\r(.*)", "$1");
            UseAnotherPresetAsSourceIsSenselessToolTip = Regex.Replace(useAnotherPresetAsSourceFullToolTip, @"^((?:[^:])*?):\r((?:[^:])*?):\r((?:[^:])*?):\r(.*)", "$2");
            UseAnotherPresetAsSourceIsInBrokenChainToolTip = Regex.Replace(useAnotherPresetAsSourceFullToolTip, @"^((?:[^:])*?):\r((?:[^:])*?):\r((?:[^:])*?):\r(.*)", "$3");
            UseAnotherPresetAsSourceCheckPresetChainToolTip = Regex.Replace(useAnotherPresetAsSourceFullToolTip, @"^((?:[^:])*?):\r((?:[^:])*?):\r((?:[^:])*?):\r(.*)", "$4");


            //Saving Create/Discard new field button labels & tool tip
            //string buttonAddFunctionText = buttonAddFunction.Text;
            string buttonAddFunctionText = buttonLabels[buttonAddFunction];
            ButtonAddFunctionCreateText = Regex.Replace(buttonAddFunctionText, @"^(.*?)\:(.*)", "$1");
            ButtonAddFunctionDiscardText = Regex.Replace(buttonAddFunctionText, @"^(.*?)\:(.*)", "$2");
            //buttonAddFunction.Text = ButtonAddFunctionCreateText;
            buttonAddFunction.Text = ButtonAddFunctionCreateText;

            string buttonAddFunctionToolTip = toolTip1.GetToolTip(buttonAddFunction);
            ButtonAddFunctionCreateToolTip = Regex.Replace(buttonAddFunctionToolTip, @"^(.*?)\:(.*)", "$1");
            ButtonAddFunctionDiscardToolTip = Regex.Replace(buttonAddFunctionToolTip, @"^(.*?)\:(.*)", "$2");
            toolTip1.SetToolTip(buttonAddFunction, ButtonAddFunctionCreateToolTip);


            //Saving Save/Update field button labels & tool tip
            //string buttonUpdateFunctionText = buttonUpdateFunction.Text;
            string buttonUpdateFunctionText = buttonLabels[buttonUpdateFunction];
            ButtonUpdateFunctionSaveText = Regex.Replace(buttonUpdateFunctionText, @"^(.*?)\:(.*)", "$1");
            ButtonUpdateFunctionUpdateText = Regex.Replace(buttonUpdateFunctionText, @"^(.*?)\:(.*)", "$2");
            //buttonUpdateFunction.Text = ButtonUpdateFunctionSaveText;
            buttonUpdateFunction.Text = ButtonUpdateFunctionSaveText;

            string buttonUpdateFunctionToolTip = toolTip1.GetToolTip(buttonUpdateFunction);
            ButtonUpdateFunctionSaveToolTip = Regex.Replace(buttonUpdateFunctionToolTip, @"^(.*?)\:(.*)", "$1");
            ButtonUpdateFunctionUpdateToolTip = Regex.Replace(buttonUpdateFunctionToolTip, @"^(.*?)\:(.*)", "$2");
            toolTip1.SetToolTip(buttonUpdateFunction, ButtonUpdateFunctionSaveToolTip);


            //Saving Help button tool tip
            string buttonHelpInitialToolTip = toolTip1.GetToolTip(buttonHelp);
            string buttonHelpToolTip = Regex.Replace(buttonHelpInitialToolTip, @"^(.*?)\r\n\r\n((.|\r|\n)*)", "$1");
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
            string[] formats = ExportedFormats.Split('|');
            for (int i = 0; i < formats.Length; i += 2)
                formatComboBoxCustom.Items.Add(formats[i]);

            smartOperationCheckBox.Checked = SavedSettings.smartOperation;

            foreach (var fname in LrFunctionNames)
                functionComboBoxCustom.Items.Add(fname);

            FillListByTagNames(sourceTagListCustom.Items, true, true, false);
            FillListByPropNames(sourceTagListCustom.Items);
            sourceTagListCustom.Items.Add(SequenceNumberName);

            FillListByTagNames(parameter2ComboBoxCustom.Items, true, false, false);
            FillListByPropNames(parameter2ComboBoxCustom.Items);
            parameter2ComboBoxCustom.SelectedIndex = 0;

            conditionListCustom.Items.Add(ListItemConditionIs);
            conditionListCustom.Items.Add(ListItemConditionIsNot);
            conditionListCustom.Items.Add(ListItemConditionIsGreater);
            conditionListCustom.Items.Add(ListItemConditionIsGreaterOrEqual);
            conditionListCustom.Items.Add(ListItemConditionIsLess);
            conditionListCustom.Items.Add(ListItemConditionIsLessOrEqual);


            presetNameTextBox.SetCue(CtlLrPresetAutoName);

            NoExpressionText = expressionTextBox.Text;
            expressionTextBox.SetCue(NoExpressionText);


            lock (SavedSettings.reportPresets) //***
            {
                ignoreCheckedPresetEvent = false;
                autoAppliedPresetCount = 0;
                reportPresetsWithHotkeysCount = 0;
                for (int i = 0; i < SavedSettings.reportPresets.Length; i++)
                {
                    ReportPreset preset = new ReportPreset(SavedSettings.reportPresets[i], true);

                    presetList.Items.Add(preset, preset.autoApply);

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
            float tagNameFontSize = previewTable.DefaultCellStyle.Font.Size * 0.7f; //Maybe it's worth to adjust fine size !!!
            totalsFont = new Font(Font.FontFamily, tagNameFontSize, FontStyle.Bold);

            headerCellStyle = new DataGridViewCellStyle(HeaderCellStyle);


            resizeArtworkCheckBox.Checked = false;
            newArtworkSizeUpDown.Value = 300;


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


            openReportCheckBox.Checked = SavedSettings.openReportAfterExporting;


            addRowsToTable = previewTable_AddRowsToTable;
            updateTable = previewTable_UpdateTable;


            this.Enable(true, autoApplyPresetsLabel);


            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        public class ColumnAttributesBase
        {
            public LrFunctionType functionType;
            private string _parameterName;

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
                    else
                    {
                        resultType = ResultType.UseOtherResults;
                        dataType = DataType.String;
                    }
                }
            }

            public DataType dataType = DataType.String;
            public ResultType resultType = ResultType.UseOtherResults;
            public string parameter2Name = null;
            public string splitter = null;
            public bool trimValues = false;

            public ColumnAttributesBase()
            {
                //Nothing to do...
            }

            public ColumnAttributesBase(ColumnAttributesBase colAttr, bool fullCopy = false)
            {
                functionType = colAttr.functionType;
                parameterName = colAttr.parameterName;
                parameter2Name = colAttr.parameterName;
                splitter = colAttr.splitter;
                trimValues = colAttr.trimValues;

                if (fullCopy)
                {
                    dataType = colAttr.dataType;
                    resultType = colAttr.resultType;
                }
            }

            public ColumnAttributesBase(LrFunctionType functionType, string parameterName, string parameter2Name,
                string splitter, bool trimValues)
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

            public string getShortId()
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

            public ColumnAttributes() : base()
            {
                //Nothing to do...
            }

            public ColumnAttributes(ColumnAttributes sourceAttribs, bool fullCopy = false) : base(sourceAttribs, fullCopy)
            {
                expression = sourceAttribs.expression;
            }

            public ColumnAttributes(LrFunctionType functionType, string expression, string parameterName, string parameter2Name,
                string splitter, bool trimValues) : base(functionType, parameterName, parameter2Name, splitter, trimValues)
            {
                this.expression = expression;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public string getUniqueId()
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
                    if (expression == colAttrs.expression)
                        return true;
                }

                return false;
            }

            public string getColumnName(bool trimName, bool includeSplitterTrimInfo, bool includeExpression)
            {
                return GetColumnName(parameterName, parameter2Name, functionType, splitter, trimValues, expression, trimName,
                    includeSplitterTrimInfo, includeExpression);
            }

            public string evaluateExpression(string url, string tagValue)
            {
                if (string.IsNullOrWhiteSpace(expression))
                {
                    return tagValue;
                }
                else
                {
                    tagValue = tagValue.Replace('\"', '\u0007');
                    string workingExpression = expression.Replace("\\@", "\"" + tagValue + "\"");
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
            public string[] expressions = new string[] { string.Empty };
            public int[] columnIndices = new int[0]; //per expression for current grouping/function

            public PresetColumnAttributes() : base()
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
            }

            public PresetColumnAttributes(LrFunctionType functionType, string[] expressions, string parameterName, string parameter2Name,
                string splitter, bool trimValues)
                : base(functionType, parameterName, parameter2Name,
                    splitter, trimValues)
            {
                this.expressions = expressions;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public string getUniqueId(int exprIndex)
            {
                string expression = string.Empty;
                if (exprIndex >= 0)
                    expression = expressions[exprIndex];

                int parameterId = (int)GetTagId(parameterName);
                int parameter2Id = (int)GetTagId(parameter2Name);

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
            public List<ColumnIndexTagValue> evaluateExpressions(string url, string tagValue)
            {
                tagValue = tagValue.Replace('\"', '\u0007');

                var tagExprValues = new List<ColumnIndexTagValue>();
                for (int i = 0; i < expressions.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(expressions[i]))
                    {
                        tagExprValues.Add(new ColumnIndexTagValue { index = columnIndices[i], value = tagValue });
                    }
                    else
                    {
                        string workingExpression = expressions[i].Replace("\\@", "\"" + tagValue + "\"");
                        string result = MbApiInterface.MB_Evaluate(workingExpression, url);
                        result = result.Replace('\u0007', '\"');

                        tagExprValues.Add(new ColumnIndexTagValue { index = columnIndices[i], value = result });
                    }
                }

                return tagExprValues;
            }

            //<columnIndex, resulting tag value>, int[] columnIndices: per expression for current grouping/function
            public List<ColumnIndexTagValue> getSplitValues(string url, string tagValue)
            {
                var results = new List<ColumnIndexTagValue>();

                if (string.IsNullOrWhiteSpace(splitter))
                {
                    results.AddRange(evaluateExpressions(url, tagValue));
                }
                else
                {
                    string workingSplitter = splitter.Replace("\\0", "\u0000");
                    string[] splitValues = tagValue.Split(new string[] { workingSplitter }, StringSplitOptions.None);
                    for (int i = 0; i < splitValues.Length; i++)
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
            public ColumnAttributesDict() : base()
            {
                //Nothing to do...
            }

            public ColumnAttributesDict(ColumnAttributesDict dict, bool fullCopy = false) : base()
            {
                foreach (var keyValue in dict)
                    Add(keyValue.Key, new ColumnAttributes(keyValue.Value, fullCopy));
            }

            public ColumnAttributesDict(ColumnAttributes[] attribCollection) : base()
            {
                foreach (var attribs in attribCollection)
                    Add(attribs.getUniqueId(), attribs);
            }

            //Returns PresetColumnAttributes[], column count
            public (PresetColumnAttributes[], int) valuesToPresetArray(int startingColumnIndex)
            {
                var presetAttribListRef = new List<PresetColumnAttributes>();
                int columnCount = 0;

                var shortIdsRef = new List<string>();
                var columnIndicesRef = new List<List<int>>();
                var expressionsRef = new List<List<string>>();

                int i = startingColumnIndex - 1;
                foreach (var attribs in Values)
                {
                    i++;

                    LoadPresetColumnAttributes(presetAttribListRef, shortIdsRef, columnIndicesRef, expressionsRef, attribs, i);
                }

                for (int j = 0; j < presetAttribListRef.Count; j++)
                {
                    presetAttribListRef[j].expressions = expressionsRef[j].ToArray();
                    presetAttribListRef[j].columnIndices = new int[columnIndicesRef[j].Count];
                    columnIndicesRef[j].CopyTo(presetAttribListRef[j].columnIndices);
                    columnCount += columnIndicesRef[j].Count;
                }

                return (presetAttribListRef.ToArray(), columnCount);
            }

            public int indexOf(string searchedKey)
            {
                int i = 0;
                foreach (var key in Keys)
                {
                    if (key == searchedKey)
                        return i;

                    i++;
                }

                return -1;
            }

            public List<string> idsToSortedList(bool getUniqueIds, List<string> list = null)
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

                list.Sort();

                return list;
            }

            public List<string> getAllIdsByShortId(string shortId)
            {
                var longIdsList = new List<string>();

                foreach (var pair in this)
                {
                    if (pair.Value.getShortId() == shortId)
                        longIdsList.AddUnique(pair.Key);
                }

                return longIdsList;
            }

            public void remove(List<string> longIdsList)
            {
                foreach (var longId in longIdsList)
                    Remove(longId);
            }
        }

        public class PresetColumnAttributesDict : SortedDictionary<string, PresetColumnAttributes> //Column unique ID (string), Preset column settings
        {
            public PresetColumnAttributesDict() : base()
            {
                //Nothing to do...
            }

            public PresetColumnAttributesDict(PresetColumnAttributesDict dict, out bool splitGroupingsExist, bool fullCopy = false) : base()
            {
                splitGroupingsExist = false;

                foreach (var keyValue in dict)
                {
                    Add(keyValue.Key, new PresetColumnAttributes(keyValue.Value, fullCopy));
                    splitGroupingsExist |= !string.IsNullOrWhiteSpace(keyValue.Value.splitter);
                }
            }
        }

        public static ReportPreset GetCreatePredefinedPreset(Guid presetPermanentGuid, string presetName, 
            SortedDictionary<Guid, ReportPreset> existingPredefinedPresets,
            PresetColumnAttributes[] groupings, PresetColumnAttributes[] functions,
            string[] destinationTags, string[] functionIds
            )
        {
            if (existingPredefinedPresets.TryGetValue(presetPermanentGuid, out var libraryReportsPreset))
            {
                libraryReportsPreset.groupings = groupings;
                libraryReportsPreset.functions = functions;

                libraryReportsPreset.name = presetName.ToUpper();

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
                    totals = true,
                    name = presetName.ToUpper(),
                    userPreset = false,

                    destinationTags = destinationTags,
                    functionIds = functionIds,
                    conditionField = groupings[0].parameterName,
                    comparison = Comparison.IsGreaterOrEqual,

                    exportedTrackListName = ExportedTrackList,
                    fileFormatIndex = LrReportFormat.HtmlDocument,
                };
            }

            libraryReportsPreset.sourceTags = new string[functions.Length];
            for (int i = 0; i < functions.Length; i++)
                libraryReportsPreset.sourceTags[i] = GetColumnName(functions[i].parameterName, functions[i].parameter2Name,
                    functions[i].functionType, null, false, null, false, false, true);

            return libraryReportsPreset;
        }

        public struct ReportPresetReference
        {
            public string name;
            public Guid permanentGuid;

            //public ReportPresetReference()
            //{
            //   name = null;
            //   permanentGuid = Guid.NewGuid();
            //}

            public ReportPresetReference(ReportPreset referencePreset)
            {
                name = referencePreset.getName();
                permanentGuid = referencePreset.permanentGuid;
            }

            public ReportPreset findPreset(ReportPreset[] reportsPresets)
            {
                foreach (var preset in reportsPresets)
                {
                    if (preset.permanentGuid == permanentGuid)
                        return preset;
                }

                return null;
            }

            public override string ToString()
            {
                return name;
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

            public string name = null;
            public string autoName = null;
            private bool columnsChecked = false;

            public bool userPreset = true;
            public Guid guid = Guid.NewGuid(); //guid is reset on every preset update in UI
            public Guid initialGuid = Guid.Empty; //initial guid is copied from guid on any preset creation, and is reset on every preset update in UI
            public Guid permanentGuid = Guid.NewGuid(); //permanentGuid is reset on preset copying in UI


            private bool senselessReferringPresetsInChain = false;
            private bool brokenReferringPresetsInChain = false;

            internal void setSenselessBrokenReferringPresetsInChain(bool senselessReferringPresetsInChain, bool brokenReferringPresetsInChain)
            {
                if (this.senselessReferringPresetsInChain != senselessReferringPresetsInChain || this.brokenReferringPresetsInChain != brokenReferringPresetsInChain)
                    this.guid = Guid.NewGuid();

                this.senselessReferringPresetsInChain = senselessReferringPresetsInChain;
                this.brokenReferringPresetsInChain = brokenReferringPresetsInChain;
            }

            internal (bool, bool) getSenselessBrokenReferringPresetsInChain()
            {
                return (senselessReferringPresetsInChain, brokenReferringPresetsInChain);
            }


            public bool hotkeyAssigned = false;
            public bool applyToSelectedTracks = true;
            public int hotkeySlot = -1; //0..MusicBeePlugin.MaximumNumberOfLrHotkeys - 1

            public PresetColumnAttributes[] groupings = new PresetColumnAttributes[0];
            public PresetColumnAttributes[] functions = new PresetColumnAttributes[0];

            public bool totals;

            public string[] sourceTags = new string[0];
            public string[] destinationTags = new string[0];
            public string[] functionIds = new string[0];

            public int[] operations = new int[0];
            public string[] mulDivFactors = new string[0];
            public string[] precisionDigits = new string[0];
            public string[] appendTexts = new string[0];

            public bool useAnotherPresetAsSource = false;
            public ReportPresetReference anotherPresetAsSource;

            public bool conditionIsChecked = false;
            public string conditionField = null;
            public Comparison comparison = Comparison.Is;
            public string comparedField = null;

            public bool resizeArtwork = false;
            public int newArtworkSize = 300;

            public LrReportFormat fileFormatIndex = LrReportFormat.HtmlDocument;
            public string exportedTrackListName = null;

            public ReportPreset()
            {
                initialGuid = guid;
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
                string hotkeyChar = getHotkeyChar();

                if (hotkeyChar == string.Empty)
                    return hotkeyChar;
                else
                    return " " + hotkeyChar;
            }

            internal string getHotkeyDescription()
            {
                return LrPresetHotkeyDescription + ": " + ToString();
            }

            private string getSenselessBrokenReferringPresetsInChainPostfix()
            {
                string postfix = string.Empty;

                if (brokenReferringPresetsInChain)
                    postfix += '❌'; //Char of Segue UI Symbol font, which will be used as fallback for any font

                if (senselessReferringPresetsInChain)
                    postfix += ''; //It's bold question mark for Segue UI Symbol font, which will be used as fallback for any font

                return postfix;
            }

            public override string ToString()
            {
                if (initialGuid == guid)
                    return getName() + getHotkeyPostfix() + getSenselessBrokenReferringPresetsInChainPostfix();
                else
                    return getName() + getHotkeyPostfix() + getSenselessBrokenReferringPresetsInChainPostfix() + " ⚠";
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
                    int ColumnCount = prepareDict(grDictRef, null, groupings, 0);
                    if (ColumnCount == -1)
                    {
                        autoName = CtlLrInvalidPresetFormatAutoName;
                        return;
                    }

                    prepareDict(fnDictRef, null, groupings, ColumnCount);
                }

                foreach (var attribs in grDictRef.Values)
                    autoName += (autoName == string.Empty ? string.Empty : ", ") + attribs.getColumnName(true, true, true);

                foreach (var attribs in fnDictRef.Values)
                    autoName += (autoName == string.Empty ? string.Empty : ", ") + attribs.getColumnName(true, true, true);

                if (autoName == string.Empty)
                    autoName = EmptyPresetName;
            }
        }

        internal static void LoadPresetColumnAttributes(List<PresetColumnAttributes> presetAttribsListRef, List<string> shortIdsRef, List<List<int>> columnIndicesRef,
            List<List<string>> expressionsRef, ColumnAttributes attribs, int columnIndex)
        {
            PresetColumnAttributes presetAttribs;

            string shortId = attribs.getShortId();
            int index = shortIdsRef.IndexOf(shortId);
            if (index == -1)
            {
                shortIdsRef.AddUnique(shortId);

                index = shortIdsRef.Count - 1;

                presetAttribs = new PresetColumnAttributes(attribs);
                presetAttribsListRef.Add(presetAttribs);
                expressionsRef.Add(new List<string>());
                columnIndicesRef.Add(new List<int>());
            }

            expressionsRef[index].Add(attribs.expression);
            columnIndicesRef[index].Add(columnIndex);
        }

        //Helper for Iterate()
        private static void MoveDependent(IEnumerator<string>[] enumerators, int[] dependentGroupingColumns, string[] lastValues, int currentLoopIndex, bool reset)
        {
            for (int j = dependentGroupingColumns.Length - 1; j >= 0; j--)
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
            bool movedThis = enumerators[currentLoopIndex].MoveNext();

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
                int oldCurrentLoopIndex = currentLoopIndex;

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

                for (int i = oldCurrentLoopIndex; i < enumerators.Length; i++)
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

                string[] lastValues = new string[groupingValuesLists.Length]; //This array will store all grouping values of last iteration
                int minimalLoopIndex = groupingValuesLists.Length - 2;
                int currentLoopIndex = groupingValuesLists.Length - 1;

                //Neither of the lists in groupingValuesLists array can be empty, so let's get 1st value of every list
                var enumerators = new IEnumerator<string>[groupingValuesLists.Length];
                for (int i = 0; i < groupingValuesLists.Length; i++)
                {
                    (enumerators[i] = groupingValuesLists[i].GetEnumerator()).MoveNext();
                    lastValues[i] = enumerators[i].Current;
                }

                do
                {
                    for (int i = 0; i < enumerators.Length; i++)
                        groupingValues[i] = lastValues[i];

                    composedGroupings.AddUnique(string.Join(MultipleItemsSplitterId.ToString(), groupingValues));


                    if (totals)
                    {
                        for (int j = groupingValues.Length - 1; j >= 0; j--)
                        {
                            groupingValues[j] = TotalsString;
                            composedGroupings.AddUnique(string.Join(MultipleItemsSplitterId.ToString(), groupingValues));
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
                        if (!TryGetValue(composedGroupings, out ConvertStringsResult[] aggregatedValues))
                        {
                            aggregatedValues = new ConvertStringsResult[1];
                            aggregatedValues[0] = new ConvertStringsResult(ResultType.UseOtherResults, DataType.String); //This are URLs (SortedDictionary<url, false>)

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
                        if (!TryGetValue(composedGroupings, out ConvertStringsResult[] aggregatedValues))
                        {
                            aggregatedValues = new ConvertStringsResult[functionValues.Length];

                            int j = -1;
                            foreach (var function in functions.Values)
                            {
                                j++;

                                aggregatedValues[j] = new ConvertStringsResult(function.resultType, function.dataType);

                                if (function.functionType == LrFunctionType.Count)
                                {
                                    function.resultType = ResultType.ItemCount;
                                    function.dataType = DataType.String;
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

                        ConvertStringsResult currentFunctionResult;

                        int i = -1;
                        foreach (var function in functions.Values)
                        {
                            i++;

                            string currentFunctionValue = function.evaluateExpression(url, functionValues[i]);

                            if (function.functionType == LrFunctionType.Count)
                            {
                                aggregatedValues[i].items.AddSkip(currentFunctionValue);
                                aggregatedValues[i].resultType = ResultType.ItemCount;

                                function.resultType = ResultType.ItemCount;
                                function.dataType = DataType.String;
                            }
                            else if (function.functionType == LrFunctionType.AverageCount)
                            {
                                aggregatedValues[i].items1.AddSkip(currentFunctionValue);
                                aggregatedValues[i].items.AddSkip(parameter2Values[i]);

                                function.resultType = ResultType.UnknownOrString;
                                function.dataType = DataType.String;
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

                                if (CompareStrings(aggregatedValues[i].resultS, currentFunctionResult.resultS, ResultType.UnknownOrString, DataType.String) == 1)
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

                                if (CompareStrings(aggregatedValues[i].resultS, currentFunctionResult.resultS, ResultType.UnknownOrString, DataType.String) == -1)
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
                    string field = composedGroupings.Split(MultipleItemsSplitterId)[fieldNumber];

                    if (field == TotalsString)
                        field = (CtlAllTags + " \"" + groupings.ElementAt(fieldNumber).Value.getColumnName(true, true, true) + "\"").ToUpper();

                    return field.CompareTo(comparedValue);
                }
                else
                {
                    ConvertStringsResult currentFunctionResult = convertResults[fieldNumber - groupings.Count];
                    ConvertStringsResult comparedFunctionValue = ConvertStrings(comparedValue, ResultType.UseOtherResults, DataType.String);

                    return currentFunctionResult.compare(comparedFunctionValue);
                }
            }

            internal static string GetField(string composedGroupings, ConvertStringsResult[] convertResults, int fieldNumber, ColumnAttributesDict groupings, int operation, string mulDivFactorRepr, string precisionDigitsRepr, string appendedText)
            {
                if (fieldNumber < groupings.Count)
                {
                    string field = composedGroupings.Split(MultipleItemsSplitterId)[fieldNumber];

                    if (field == TotalsString)
                        field = (CtlAllTags + " \"" + groupings.ElementAt(fieldNumber).Value.getColumnName(true, true, true) + "\"").ToUpper();

                    return field;
                }
                else
                {
                    return convertResults[fieldNumber - groupings.Count].getFormattedResult(operation, mulDivFactorRepr, precisionDigitsRepr, appendedText);
                }
            }

            internal static string[] GetGroupings(KeyValuePair<string, ConvertStringsResult[]> keyValue, ColumnAttributesDict groupings)
            {
                if (keyValue.Key == string.Empty)
                {
                    return new string[0];
                }
                else
                {
                    string[] fields = keyValue.Key.Split(MultipleItemsSplitterId);

                    for (int i = 0; i < fields.Length; i++)
                        if (fields[i] == TotalsString)
                            fields[i] = (CtlAllTags + " '" + groupings.ElementAt(i).Value.getColumnName(true, true, true) + "'").ToUpper();

                    return fields;
                }
            }

            internal static string[] GetGroupings(string composedGroupings, ColumnAttributesDict groupings)
            {
                if (composedGroupings == string.Empty)
                {
                    return new string[0];
                }
                else
                {
                    string[] fields = composedGroupings.Split(MultipleItemsSplitterId);

                    for (int i = 0; i < fields.Length; i++)
                        if (fields[i] == TotalsString)
                            fields[i] = (CtlAllTags + " '" + groupings.ElementAt(i).Value.getColumnName(true, true, true) + "'").ToUpper();

                    return fields;
                }
            }
        }

        internal static string GetSplitterRepresentation(string splitter, bool trimValues, bool addSpacePrefix)
        {
            string representation = string.Empty;

            if (!string.IsNullOrEmpty(splitter) && addSpacePrefix)
                representation += " [" + splitter + "]";
            else if (!string.IsNullOrEmpty(splitter))
                representation += "[" + splitter + "]";

            if (!string.IsNullOrEmpty(splitter) && trimValues)
                representation += " *";

            return representation;
        }

        internal static string GetColumnName(string tagName, string tag2Name, LrFunctionType type, string splitter, bool trimValues,
            string expression, bool trimName, bool includeSplitterTrimInfo, bool includeExpression)
        {
            string columnName = tagName;

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
                    int exprRepresentationMiddle = MaxExprRepresentationLength / 2;
                    int exprRepresentationTailStart = expression.Length - exprRepresentationMiddle;

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
            bool[] columnRightAlignments = new bool[groupingsDict.Count + functionsDict.Count];
            ResultType[] resultTypes = new ResultType[groupingsDict.Count + functionsDict.Count];

            int j = 0;
            foreach (var attribs in groupingsDict.Values)
            {
                if (attribs.parameterName == SequenceNumberName) //It's a number. Let's right-align the column.
                {
                    columnRightAlignments[j] = true;
                    resultTypes[j] = ResultType.Double;
                }
                else if (attribs.parameterName == DateCreatedTagName) //It's a number. Let's right-align the column.
                {
                    columnRightAlignments[j] = true;
                    resultTypes[j] = ResultType.DateTime;
                }
                else
                {
                    DataType columnType = MbApiInterface.Setting_GetDataType(GetTagId(attribs.parameterName));
                    if (columnType == DataType.Number || columnType == DataType.Rating) //Let's right-align the column.
                    {
                        columnRightAlignments[j] = true;
                        resultTypes[j] = ResultType.Double;
                    }
                    else if (columnType == DataType.DateTime) //Let's right-align the column.
                    {
                        columnRightAlignments[j] = true;
                        resultTypes[j] = ResultType.DateTime;
                    }
                    else //It's either string, or internal plugin pseudo-tag like "Filename". Let's left-align the column.
                    {
                        columnRightAlignments[j] = false;
                        resultTypes[j] = ResultType.UnknownOrString;
                    }
                }

                j++;
            }

            int i = 0;
            foreach (var function in functionsDict.Values)
            {
                if (function.resultType >= ResultType.AutoDouble && function.resultType <= ResultType.ItemCount) //It's either number or date/time/duration. Let's right-align the column.
                    columnRightAlignments[j + i] = true;
                else
                    columnRightAlignments[j + i] = false;

                resultTypes[j] = function.resultType;

                i++;
            }

            return (resultTypes, columnRightAlignments);
        }

        private void previewTable_AddRowsToTable(List<string[]> rows)
        {
            if (previewTable.RowCount == 0) //There are no rows in preview table yet, so let's adjust column text alignment according to data type
            {
                (columnTypes, columnsRightAlignment) = getColumnTypesRightAlignment();

                for (int i = 0; i < columnsRightAlignment.Length; i++)
                {
                    if (columnsRightAlignment[i])
                        previewTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                    else
                        previewTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                }
            }


            foreach (var row in rows)
            {
                previewTable.Rows.Add(row);

                if (artworkField == -1)
                    previewTable.Rows[previewTable.RowCount - 1].Resizable = DataGridViewTriState.True;
                else
                    previewTable.Rows[previewTable.RowCount - 1].Resizable = DataGridViewTriState.False;


                for (int i = 0; i < previewTable.ColumnCount; i++)
                {
                    if (backgroundTaskIsCanceled)
                        break;


                    if (i != artworkField)
                    {
                        if (row[i].StartsWith(CtlAllTags) && previewTable.RowCount > 0)
                            previewTable.Rows[previewTable.RowCount - 1].Cells[i].Style.Font = totalsFont;

                        if (previewTable.RowCount > 0)
                            previewTable.Rows[previewTable.RowCount - 1].Cells[i].ToolTipText = row[i] + "\n\n" + LrCellToolTip;

                        if (maxWidths[i] < row[i].Length)
                            maxWidths[i] = row[i].Length;
                    }
                    else
                    {
                        //Lets replace string hashes in the Artwork column with images.
                        string stringHash = row[artworkField];

                        if (!artworks.TryGetValue(stringHash, out Bitmap pic))
                            pic = artworks[DefaultArtworkHash];

                        if (previewTable.RowCount > 0)
                        {
                            previewTable.Rows[previewTable.RowCount - 1].Cells[artworkField].ValueType = typeof(Bitmap);
                            previewTable.Rows[previewTable.RowCount - 1].Cells[artworkField].Value = pic;
                            previewTable.Rows[previewTable.RowCount - 1].MinimumHeight = PreviewTableDefaultArtworkSize;
                        }
                    }
                }
            }

            UpdateCustomScrollBars(previewTable);
        }

        private void previewTable_UpdateTable()
        {
            for (int i = 0; i < previewTable.ColumnCount; i++)
            {
                if (i != artworkField && maxWidths[i] > 0)
                    previewTable.Columns[i].FillWeight = maxWidths[i];
            }

            previewTable.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            previewTable.DisableColumnsAutoSize(true);

            previewTable.AutoResizeRows();

            UpdateCustomScrollBars(previewTable);

            if (previewTable.RowCount > 0)
                previewTable.CurrentCell = previewTable.Rows[0].Cells[0];
        }

        private void clearArtworks()
        {
            artwork?.Dispose();
            artwork = null;

            foreach (var pair in artworks)
                pair.Value.Dispose();

            artworks.Clear();
        }

        private void resetLocalsAndUiControls()
        {
            lastCachedAppliedPresetGuid = Guid.NewGuid();

            while (previewTable.ColumnCount > 0)
                previewTable.Columns.RemoveAt(0);

            tagsDataGridView.RowCount = 0;
            tagsDataGridViewSelectedRow = -1;
            expressionsDataGridView.RowCount = 0;


            UpdateCustomScrollBars(presetList);
            UpdateCustomScrollBars(previewTable);
            UpdateCustomScrollBars(tagsDataGridView);
            UpdateCustomScrollBars(expressionsDataGridView);


            expressionBackup = string.Empty;
            splitterBackup = string.Empty;
            trimValuesBackup = false;

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

            functionComboBoxCustom.SelectedIndex = -1;
            sourceTagListCustom.SelectedIndex = -1;
            expressionTextBox.Text = string.Empty;


            groupings.Clear();
            groupingsDict.Clear();
            functionsDict.Clear();

            operations.Clear();
            mulDivFactors.Clear();
            precisionDigits.Clear();
            appendTexts.Clear();

            functionComboBoxCustom.SelectedIndex = 0;

            clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, buttonPreview, buttonOK, buttonClose);
        }

        //Returns column count
        private static int prepareDict(ColumnAttributesDict dictRef, PresetColumnAttributesDict presetDictRef,
            PresetColumnAttributes[] attribsSet, int startingColumnIndex)
        {
            int columnIndex = startingColumnIndex;
            int maxColumnIndex = startingColumnIndex - 1;
            for (int i = 0; i < attribsSet.Length; i++)
                maxColumnIndex += attribsSet[i].expressions.Length;

            if (presetDictRef != null)
                for (int i = 0; i < attribsSet.Length; i++)
                    presetDictRef.AddSkip(attribsSet[i].getShortId(), attribsSet[i]);


                repeat_again:
            if (columnIndex <= maxColumnIndex)
            {
                for (int i = 0; i < attribsSet.Length; i++)
                {
                    for (int j = 0; j < attribsSet[i].expressions.Length; j++)
                    {
                        if (columnIndex == attribsSet[i].columnIndices[j])
                        {
                            string uniqueId = attribsSet[i].getUniqueId(j);
                            if (uniqueId == null)
                                return -1;

                            dictRef.AddSkip(uniqueId,
                                new ColumnAttributes(attribsSet[i].functionType, attribsSet[i].expressions[j], attribsSet[i].parameterName,
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
                int i = -1;
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

        private void prepareLocals()
        {
            if (lastCachedAppliedPresetGuid == appliedPreset.guid)
                return;

            groupings.Clear();
            groupingsDict.Clear();
            int columnCount = prepareDict(groupingsDict, groupings, appliedPreset.groupings, 0);

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
            for (int j = 0; j < appliedPreset.destinationTags.Length; j++)
                destinationTagIds.Add(GetTagId(appliedPreset.destinationTags[j]));

            artworkField = -1;
            clearArtworks();

            sequenceNumberField = -1;

            int i = 0;
            foreach (var attribs in groupingsDict.Values)
            {
                if (attribs.parameterName == ArtworkName)
                    artworkField = i;
                else if (attribs.parameterName == SequenceNumberName)
                    sequenceNumberField = i;

                i++;
            }

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
            static TypeConverter tc = null;
            static byte[] hash;
            static MD5Cng md5 = null;

            static int newArtworkSize = -1;

            internal static void Init(int artworkField, SortedDictionary<string, Bitmap> artworks, int presetNewArtworkSize)
            {
                if (artworkField != -1)
                {
                    newArtworkSize = presetNewArtworkSize;

                    var pic = new Bitmap(DefaultArtwork);

                    tc = TypeDescriptor.GetConverter(typeof(Bitmap));
                    md5 = new MD5Cng();

                    DefaultArtworkHash = GetResizedArtworkBase64Hash(ref pic);

                    try { hash = md5.ComputeHash(tc.ConvertTo(pic, typeof(byte[])) as byte[]); }
                    catch { hash = md5.ComputeHash(new byte[] { 0x00 }); }

                    artworks.AddReplace(DefaultArtworkHash, pic);
                }
            }

            internal static string GetResizedArtworkBase64Hash(ref Bitmap pic)
            {
                if (newArtworkSize > 0)
                {
                    float SF;

                    if (pic.Width >= pic.Height)
                        SF = newArtworkSize / (float)pic.Width;
                    else
                        SF = newArtworkSize / (float)pic.Height;


                    try
                    {
                        Bitmap bm_dest = new Bitmap((int)Math.Round(pic.Width * SF), (int)Math.Round(pic.Height * SF), System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                        Graphics gr_dest = Graphics.FromImage(bm_dest);
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

                try { hash = md5.ComputeHash((byte[])tc.ConvertTo(pic, typeof(byte[]))); }
                catch { hash = md5.ComputeHash(new byte[] { 0x00 }); }

                return Convert.ToBase64String(hash);
            }

            internal static string GetBase64ArtworkBase64Hash(string artworkBase64, SortedDictionary<string, Bitmap> artworks, out Bitmap pic)
            {
                try
                {
                    if (artworkBase64 != string.Empty)
                        pic = tc.ConvertFrom(Convert.FromBase64String(artworkBase64)) as Bitmap;
                    else
                        pic = new Bitmap(DefaultArtwork);
                }
                catch
                {
                    pic = new Bitmap(DefaultArtwork);
                }

                string Base64StringHash = GetResizedArtworkBase64Hash(ref pic);
                artworks.AddReplace(Base64StringHash, pic);

                return Base64StringHash;
            }
        }

        internal static string ConvertSequenceNumberToString(int i)
        {
            return i.ToString();


            string oldSequenceNumber = i.ToString("D9"); //-V3142
            string sequenceNumber = string.Empty;

            int j = 0;
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
            this.lastCachedAppliedPresetGuid = appliedPreset.guid;
            this.cachedAppliedPresetGuids.AddSkip(appliedPreset.guid);
            this.cachedPresetsTags.AddReplace(appliedPreset.guid, tags);
            this.cachedPresetsFilesActualGroupingTags.AddReplace(appliedPreset.guid, filesGroupingTags);
            this.cachedPresetsFilesActualGroupingTagsRaw.AddReplace(appliedPreset.guid, filesGroupingTagsRaw);
            this.cachedPresetsFilesActualComposedSplitGroupingTagsList.AddReplace(appliedPreset.guid, filesActualComposedGroupingTags);
        }

        private void clearAppliedPresetCache()
        {
            this.lastCachedAppliedPresetGuid = Guid.NewGuid();
            this.cachedAppliedPresetGuids.RemoveExisting(appliedPreset.guid);
            this.cachedPresetsTags.RemoveExisting(appliedPreset.guid);
            this.cachedPresetsFilesActualGroupingTags.RemoveExisting(appliedPreset.guid);
            this.cachedPresetsFilesActualGroupingTagsRaw.RemoveExisting(appliedPreset.guid);
            cachedPresetsFilesActualComposedSplitGroupingTagsList.RemoveExisting(appliedPreset.guid);
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
            else if (tagId == ArtistArtistsId || tagId == ComposerComposersId) //Lets make smart conversion of list of artists/composers
            {
                tagValue = GetFileTag(file, tagId);
                if (smartOperation && !getRawValues)
                    tagValue = GetTagRepresentation(tagValue);
                else if (!getRawValues)
                    tagValue = RemoveRoleIds(tagValue);
            }
            else if (tagId == MetaDataType.Artwork) //It's artwork image. Lets fill cell with hash codes. 
            {
                tagValue = ResizedArtworkProvider.GetBase64ArtworkBase64Hash(
                    GetFileTag(file, MetaDataType.Artwork), artworks, out Bitmap pic);

                artwork = pic; //For CD booklet export
            }
            else if (tagId == 0)
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
            int idIndex = appliedPreset.functionIds.IndexOfFirst(functionId);

            if (idIndex >= 0 && !IsTagEmpty(appliedPreset.destinationTags[idIndex]))
                return idIndex;
            else
                return -1;
        }

        internal string executePreset(string[] queriedFiles, bool interactive, bool saveResultsToTags, string functionId,
            bool filterResults, bool forceCacheUpdate) //filterResults:
                                                       //  false - proceed as usual (filter results by this preset condition only (if defined) on tag saving only for any "interactive")
                                                       //  true & !interactive - update lastFiles by filtered file list AND by this preset condition (if defined)
                                                       //  true & interactive - filter queriedFiles list by this preset condition only (if defined)
                                                       //
                                                       //   "true & !interactive" filterResults MUST BE USED ONLY FOR FILTERING PRESET CHAIN INVOKED INSIDE THIS FUNCTION
                                                       //
                                                       //
                                                       //forceCacheUpdate: false - use cache if available, true - force cache update
        {
            if (queriedFiles != null && queriedFiles.Length == 0)
            {
                clearAppliedPresetCache();

                if (interactive)
                    MessageBox.Show(MbForm, SbLrEmptyTrackListToBeApplied, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return SbLrEmptyTrackListToBeApplied;
            }

            if (functionId != null && string.IsNullOrWhiteSpace(functionId))
            {
                clearAppliedPresetCache();
                return SbIncorrectLrFunctionId + "\"" + functionId + "\"";
            }

            bool queryEntireLibrary = false;



            string returnValue = null;

            lock (SavedSettings.reportPresets) //***
            {
                if (functionId != null && queriedFiles == null)
                {
                    clearAppliedPresetCache();
                    returnValue = SbLrNot1TrackPassedToLrFunctionId;
                }
                else if (functionId != null && queriedFiles.Length != 1) //queriedFiles != null if functionId != null. Checked above.
                {
                    clearAppliedPresetCache();
                    returnValue = SbLrNot1TrackPassedToLrFunctionId;
                }

                if (returnValue == null)
                {
                    if (queriedFiles == null)
                    {
                        queryEntireLibrary = true;

                        if (!MbApiInterface.Library_QueryFilesEx("domain=Library", out queriedFiles))
                            return string.Empty;

                        if (queriedFiles.Length == 0)
                            return string.Empty;
                    }

                    appliedPreset = new ReportPreset(appliedPreset, true);

                    var reportPresets = new ReportPreset[this.reportPresets.Length];
                    this.reportPresets.CopyTo(reportPresets, 0);
                    SavedSettings.reportPresets.CopyTo(reportPresets, 0);
                }
            }

            if (returnValue != null)
                return returnValue;


            //LET'S DEAL WITH ANOTHER PRESET AS A SOURCE
            if (appliedPreset.useAnotherPresetAsSource)
            {
                (_, bool relatedPresetsCheckStatusIsSenseless, bool relatedPresetsCheckStatusIsBroken) = checkPresetChainDeeper(reportPresets, appliedPreset, appliedPreset, false);
                if (relatedPresetsCheckStatusIsSenseless || relatedPresetsCheckStatusIsBroken)
                {
                    if (functionId == null)
                    {
                        SetStatusbarText(SbBrokenPresetRetrievalChain.Replace("%%PRESETNAME%%", appliedPreset.getName()), true);
                        System.Media.SystemSounds.Question.Play();
                        DisablePlaySoundOnce = true;
                    }

                    clearAppliedPresetCache();
                    return "!!!";
                }

                ReportPreset anotherPresetAsSource = appliedPreset.anotherPresetAsSource.findPreset(reportPresets);
                if (anotherPresetAsSource?.conditionIsChecked != true)
                    return "!!!";


                var appliedPresetInUse = appliedPreset;
                appliedPreset = anotherPresetAsSource;
                executePreset(queriedFiles, false, false, functionId, true, forceCacheUpdate);
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

            bool queryOnlyGroupings = false;
            if (functionsDict.Count == 0)
                queryOnlyGroupings = true;


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
            int[] dependentGroupingColumns = new int[groupings.Count];
            int p = -1;
            foreach (var grouping in groupings.Values)
            {
                p++;

                for (int l = 0; l < grouping.columnIndices.Length; l++)
                    dependentGroupingColumns[grouping.columnIndices[l]] = p;
            }


            int lastSeqNumInOrder = 1;
            bool presetGroupingTagsAreCached = true;


            //Let's cache tag/prop ids
            MetaDataType[] queriedGroupingsTagIds;
            FilePropertyType[] queriedGroupingsPropIds;
            MetaDataType[] queriedActualGroupingsTagIds;
            FilePropertyType[] queriedActualGroupingsPropIds;
            string[] queriedNativeTagNames;

            if (cachedAppliedPresetGuids.ContainsKey(appliedPreset.guid))
            {
                queriedGroupingsTagIds = this.cachedPresetsGroupingsTagIds[appliedPreset.guid];
                queriedGroupingsPropIds = this.cachedPresetsGroupingsPropIds[appliedPreset.guid];
                queriedActualGroupingsTagIds = this.cachedPresetsActualGroupingsTagIds[appliedPreset.guid];
                queriedActualGroupingsPropIds = this.cachedPresetsActualGroupingsPropIds[appliedPreset.guid];
                queriedNativeTagNames = this.cachedPresetsNativeTagNames[appliedPreset.guid];
            }
            else
            {
                presetGroupingTagsAreCached = false;

                queriedGroupingsTagIds = new MetaDataType[groupings.Count];
                for (int l = 0; l < queriedGroupingsTagIds.Length; l++)
                    queriedGroupingsTagIds[l] = 0;

                queriedGroupingsPropIds = new FilePropertyType[groupings.Count];
                for (int l = 0; l < queriedGroupingsPropIds.Length; l++)
                    queriedGroupingsPropIds[l] = 0;


                queriedActualGroupingsTagIds = new MetaDataType[groupings.Count];
                for (int l = 0; l < queriedActualGroupingsTagIds.Length; l++)
                    queriedActualGroupingsTagIds[l] = 0;

                queriedActualGroupingsPropIds = new FilePropertyType[groupings.Count];
                for (int l = 0; l < queriedActualGroupingsTagIds.Length; l++)
                    queriedActualGroupingsTagIds[l] = 0;

                queriedNativeTagNames = new string[groupings.Count];


                int i = -1;
                foreach (var attribs in groupings.Values)
                {
                    i++;

                    MetaDataType tagId = GetTagId(attribs.parameterName);
                    FilePropertyType propId = GetPropId(attribs.parameterName);
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

                this.cachedPresetsGroupingsTagIds.AddReplace(appliedPreset.guid, queriedGroupingsTagIds);
                this.cachedPresetsGroupingsPropIds.AddReplace(appliedPreset.guid, queriedGroupingsPropIds);
                this.cachedPresetsActualGroupingsTagIds.AddReplace(appliedPreset.guid, queriedActualGroupingsTagIds);
                this.cachedPresetsActualGroupingsPropIds.AddReplace(appliedPreset.guid, queriedActualGroupingsPropIds);
                this.cachedPresetsNativeTagNames.AddReplace(appliedPreset.guid, queriedNativeTagNames);
            }


            if (!this.cachedPresetsTags.TryGetValue(appliedPreset.guid, out var tags))
            {
                tags = new AggregatedTags();
                this.cachedPresetsTags.Add(appliedPreset.guid, tags);
            }

            if (!this.cachedPresetsFilesActualGroupingTags.TryGetValue(appliedPreset.guid, out var cachedFilesActualGroupingTags))
            {
                cachedFilesActualGroupingTags = new SortedDictionary<int, string[]>(); //<URLs, Grouping tag[]s>
                this.cachedPresetsFilesActualGroupingTags.Add(appliedPreset.guid, cachedFilesActualGroupingTags);
            }

            if (!this.cachedPresetsFilesActualGroupingTagsRaw.TryGetValue(appliedPreset.guid, out var cachedFilesActualGroupingTagsRaw))
            {
                cachedFilesActualGroupingTagsRaw = new SortedDictionary<int, string[]>(); //<URLs, Grouping tag[]s>
                this.cachedPresetsFilesActualGroupingTagsRaw.Add(appliedPreset.guid, cachedFilesActualGroupingTagsRaw);
            }

            if (!this.cachedPresetsFilesActualComposedSplitGroupingTagsList.TryGetValue(appliedPreset.guid, out var cachedFilesActualComposedSplitGroupingTagsList))
            {
                cachedFilesActualComposedSplitGroupingTagsList = new SortedDictionary<int, List<string>>(); //<URL, List of <composed groupings>>
                this.cachedPresetsFilesActualComposedSplitGroupingTagsList.Add(appliedPreset.guid, cachedFilesActualComposedSplitGroupingTagsList);
            }


            SortedDictionary<string, bool>[] queriedGroupingTagsRaw = null;
            SortedDictionary<string, bool>[] queriedActualGroupingTags = null;
            SortedDictionary<string, bool>[] queriedActualGroupingTagsRaw = null;

            if (!queryOnlyGroupings)
            {
                //Only grouping tags, which can be used in MB query (not required if only groupings are queried)
                queriedGroupingTagsRaw = new SortedDictionary<string, bool>[groupings.Count];
                for (int l = 0; l < queriedGroupingTagsRaw.Length; l++)
                    queriedGroupingTagsRaw[l] = new SortedDictionary<string, bool>();

                //All displayed (formatted) grouping tags as is (not required if only groupings are queried)
                queriedActualGroupingTags = new SortedDictionary<string, bool>[groupings.Count];

                for (int l = 0; l < queriedActualGroupingTags.Length; l++)
                    queriedActualGroupingTags[l] = new SortedDictionary<string, bool>();

                //All raw grouping tags as is (not required if only groupings are queried)
                queriedActualGroupingTagsRaw = new SortedDictionary<string, bool>[groupings.Count];

                for (int l = 0; l < queriedActualGroupingTagsRaw.Length; l++)
                    queriedActualGroupingTagsRaw[l] = new SortedDictionary<string, bool>();
            }


            //Let's add default artwork
            if (functionId == null)
            {
                clearArtworks();
                ResizedArtworkProvider.Init(artworkField, artworks, appliedPreset.resizeArtwork ? appliedPreset.newArtworkSize : -1);
            }


            List<string>[] actualSplitGroupingTagsList = new List<string>[groupings.Count]; //array (size of grouping count) of list of split grouping tags
            for (int f = 0; f < actualSplitGroupingTagsList.Length; f++)
                actualSplitGroupingTagsList[f] = new List<string>();


            SortedDictionary<string, bool> queriedFilesDict = new SortedDictionary<string, bool>();
            foreach (var file in queriedFiles)
                queriedFilesDict.AddSkip(file);


            processFileGroupings(queriedFilesDict, interactive, functionId, queryOnlyGroupings, lastSeqNumInOrder, 
                        tags, queriedActualGroupingsTagIds, queriedActualGroupingsPropIds,
                        actualSplitGroupingTagsList, cachedFilesActualComposedSplitGroupingTagsList,
                        cachedFilesActualGroupingTags, cachedFilesActualGroupingTagsRaw,
                        dependentGroupingColumns, queriedGroupingTagsRaw, //-V3080
                        queriedActualGroupingTags, queriedActualGroupingTagsRaw, //-V3080
                        queriedNativeTagNames);


            if (queryOnlyGroupings)
            {
                updateAppliedPresetCache(tags, cachedFilesActualGroupingTags, cachedFilesActualGroupingTagsRaw, cachedFilesActualComposedSplitGroupingTagsList);
                applyOnlyGroupingsPresetResults(queriedFiles, interactive, filterResults);
                return "...";
            }


            string[] potentiallyAffectedFiles;

            if (queryEntireLibrary)
            {
                potentiallyAffectedFiles = queriedFiles;
            }
            else
            {
                string query = @"<SmartPlaylist><Source Type=""1""><Conditions CombineMethod=""All"">";

                int i = -1;
                foreach (var attribs in groupings.Values)
                {
                    i++;

                    string nativeTagName = queriedNativeTagNames[i];

                    if (nativeTagName != null)
                    {
                        query += @"<Condition Field=""" + nativeTagName + @""" Comparison=""IsIn""";

                        int n = 1;
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
            }


            bool cachedTagsAreRelevant = false;

            if (presetGroupingTagsAreCached && !forceCacheUpdate)
            {
                cachedTagsAreRelevant = true;

                for (int i = 0; i < potentiallyAffectedFiles.Length; i++)
                {
                    int trackId = GetPersistentTrackIdInt(potentiallyAffectedFiles[i]);

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
                return applyPresetResults(queriedFiles, tags, cachedFilesActualComposedSplitGroupingTagsList, interactive, saveResultsToTags, functionId, filterResults);
            }


            SortedDictionary<string, bool> newFilesDict = new SortedDictionary<string, bool>();
            foreach (var file in potentiallyAffectedFiles)
                if (!queriedFilesDict.ContainsKey(file))
                    newFilesDict.AddSkip(file);


            processFileGroupings(newFilesDict, interactive, functionId, false, lastSeqNumInOrder, 
                        null, queriedActualGroupingsTagIds, queriedActualGroupingsPropIds,
                        actualSplitGroupingTagsList, cachedFilesActualComposedSplitGroupingTagsList,
                        cachedFilesActualGroupingTags, cachedFilesActualGroupingTagsRaw,
                        dependentGroupingColumns, queriedGroupingTagsRaw,
                        queriedActualGroupingTags, queriedActualGroupingTagsRaw,
                        queriedNativeTagNames);


            tags.Clear(); //Let's resets aggregated functions


            string[] functionTags = new string[functionsDict.Count];
            string[] parameter2Tags = new string[functionsDict.Count];

            SortedDictionary<string, bool> affectedFilesDict = new SortedDictionary<string, bool>();

            string[] affectedFiles = null;
            if (queryEntireLibrary)
                affectedFiles = potentiallyAffectedFiles;


            for (int n = 0; n < potentiallyAffectedFiles.Length; n++)
            {
                bool skipFile = false; //Skip if current file grouping tags are not contained in
                                       //actualGroupingTagsRaw, i.e. queried file selection is excessive due to unsupported (and skipped in query) tags

                if (backgroundTaskIsCanceled)
                {
                    if (interactive)
                        Invoke(updateTable);

                    clearAppliedPresetCache();
                    return string.Empty;
                }


                if (interactive)
                    SetStatusbarTextForFileOperations(LibraryReportsSbText, true, n, potentiallyAffectedFiles.Length, appliedPreset.getName());
                else if (functionId == null)
                    SetStatusbarTextForFileOperations(ApplyingLrPresetSbText, true, n, potentiallyAffectedFiles.Length, appliedPreset.getName());


                string currentFile = potentiallyAffectedFiles[n];
                int trackId = GetPersistentTrackIdInt(currentFile);

                if (!queryEntireLibrary)
                {
                    if (cachedFilesActualGroupingTagsRaw.TryGetValue(trackId, out var actualGroupingTagsRaw))
                    {
                        for (int i = 0; i < groupings.Values.Count; i++)
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
                        for (int i = 0; i < groupings.Values.Count; i++)
                        {
                            MetaDataType tagId = queriedActualGroupingsTagIds[i];
                            FilePropertyType propId = queriedActualGroupingsPropIds[i];

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


                int j = -1;
                foreach (var attribs in functionsDict.Values)
                {
                    j++;

                    MetaDataType tagId = GetTagId(attribs.parameterName);
                    FilePropertyType propId = GetPropId(attribs.parameterName);

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
                int l = 0;
                foreach (var file in affectedFilesDict.Keys)
                    affectedFiles[l++] = file;
            }

            updateAppliedPresetCache(tags, cachedFilesActualGroupingTags, cachedFilesActualGroupingTagsRaw, cachedFilesActualComposedSplitGroupingTagsList);

            if (forceCacheUpdate) //cacheUsage: true - force cache update, false - use cache if available
                return applyPresetResults(affectedFiles, tags, cachedFilesActualComposedSplitGroupingTagsList, interactive, true, string.Empty, filterResults);
            else
                return applyPresetResults(affectedFiles, tags, cachedFilesActualComposedSplitGroupingTagsList, interactive, saveResultsToTags, functionId, filterResults);
        }

        private void processFileGroupings(SortedDictionary<string, bool> queriedFilesDict, bool interactive, string functionId, bool queryOnlyGroupings, int lastSeqNumInOrder,
            AggregatedTags tags, MetaDataType[] queriedActualGroupingsTagIds, FilePropertyType[] queriedActualGroupingsPropIds,
            List<string>[] actualSplitGroupingTagsList, SortedDictionary<int, List<string>> cachedFilesActualComposedSplitGroupingTagsList,
            SortedDictionary<int, string[]> cachedFilesActualGroupingTags, SortedDictionary<int, string[]> cachedFilesActualGroupingTagsRaw,
            int[] dependentGroupingColumns, SortedDictionary<string, bool>[] queriedGroupingTagsRaw,
            SortedDictionary<string, bool>[] queriedActualGroupingTags, SortedDictionary<string, bool>[] queriedActualGroupingTagsRaw,
            string[] queriedNativeTagNames)
        {
            List<string> composedActualSplitGroupingTagsList = null;


            int n = -1;
            foreach (string currentFile in queriedFilesDict.Keys)
            {
                n++;

                int trackId = GetPersistentTrackIdInt(currentFile);

                //sequenceNumberField == -1 is because of sequence numbers can be different every time
                if (sequenceNumberField == -1 && cachedFilesActualGroupingTags.TryGetValue(trackId, out var actualGroupingTags)
                    && cachedFilesActualGroupingTagsRaw.TryGetValue(trackId, out var actualGroupingTagsRaw)
                    && cachedFilesActualComposedSplitGroupingTagsList.TryGetValue(trackId, out composedActualSplitGroupingTagsList))
                {
                    int h = -1;
                    foreach (var attribs in groupings.Values)
                    {
                        h++;

                        string tagValue = actualGroupingTags[h];
                        string tagValueRaw = actualGroupingTagsRaw[h];


                        //Let's remember actual (not raw) grouping tags for future reuse
                        queriedActualGroupingTags[h].AddSkip(tagValue);
                        queriedActualGroupingTagsRaw[h].AddSkip(tagValueRaw);

                        //Let's remember only grouping (raw) tags, which can be used in query in Library_QueryFilesEx function
                        //MusicBee doesn't support for querying some tags, so let's skip them in query
                        if (queriedNativeTagNames[h] != null)
                            queriedGroupingTagsRaw[h].AddSkip(tagValueRaw);
                    }
                }
                else
                {
                    actualGroupingTags = null;
                    actualGroupingTagsRaw = null;

                    for (int f = 0; f < actualSplitGroupingTagsList.Length; f++)
                        actualSplitGroupingTagsList[f].Clear();

                    if (!queryOnlyGroupings)
                    {
                        actualGroupingTags = new string[groupings.Count];
                        actualGroupingTagsRaw = new string[groupings.Count];
                    }


                    int h = -1;
                    foreach (var attribs in groupings.Values)
                    {
                        h++;

                        MetaDataType tagId = queriedActualGroupingsTagIds[h];
                        FilePropertyType propId = queriedActualGroupingsPropIds[h];

                        string tagValue = getTagValue(currentFile, tagId, propId, false); // getTagValue() returns "xXxXxXxXx" for sequenceNumber grouping. See code below.

                        if (!queryOnlyGroupings)
                        {
                            string tagValueRaw = getTagValue(currentFile, tagId, propId, true);
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


                        var columnIndicesTagValues = attribs.getSplitValues(currentFile, tagValue);
                        foreach (var columnIndexTag in columnIndicesTagValues)
                            actualSplitGroupingTagsList[columnIndexTag.index].Add(columnIndexTag.value);


                        composedActualSplitGroupingTagsList = AggregatedTags.GetComposedGroupingTags(actualSplitGroupingTagsList, dependentGroupingColumns, appliedPreset.totals);

                        if (sequenceNumberField != -1)
                        {
                            List<string> composedActualSplitGroupingTagsList2 = new List<string>();

                            foreach (var composedGroupingTags1 in composedActualSplitGroupingTagsList)
                            {
                                string sequenceNumber = ConvertSequenceNumberToString(lastSeqNumInOrder++);
                                string composedGroupingTags2 = composedGroupingTags1.Replace("xXxXxXxXx", sequenceNumber);
                                composedActualSplitGroupingTagsList2.Add(composedGroupingTags2);
                            }

                            composedActualSplitGroupingTagsList = composedActualSplitGroupingTagsList2;
                        }

                        cachedFilesActualComposedSplitGroupingTagsList.AddReplace(trackId, composedActualSplitGroupingTagsList);
                    }


                    if (!queryOnlyGroupings)
                    {
                        cachedFilesActualGroupingTags.AddReplace(trackId, actualGroupingTags);
                        cachedFilesActualGroupingTagsRaw.AddReplace(trackId, actualGroupingTagsRaw);
                    }
                }


                if (interactive)
                    SetStatusbarTextForFileOperations(LibraryReportsSbText, true, n, queriedFilesDict.Count, appliedPreset.getName());
                else if (functionId == null)
                    SetStatusbarTextForFileOperations(ApplyingLrPresetSbText, true, n, queriedFilesDict.Count, appliedPreset.getName());

                if (queryOnlyGroupings)
                    tags.add(currentFile, composedActualSplitGroupingTagsList, null, null, null);
            }


            return;
        }


        private string applyPresetResults(string[] queriedFiles, AggregatedTags tags, SortedDictionary<int, List<string>> filesActualComposedSplitGroupingTagsLists,
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


            bool showRow = true;

            SortedDictionary<string, bool> filteredFiles = null;
            List<string[]> rows = null;
            SortedDictionary<string, string[]> composedSplitGroupingTagsReportRows = null;

            if (!interactive && filterResults) //Only for filtering by another preset (lastFiles which are now queriedFiles) AND by this preset condition
            {
                filteredFiles = new SortedDictionary<string, bool>();
            }
            else if (interactive)
            {
                rows = new List<string[]>();
                composedSplitGroupingTagsReportRows = new SortedDictionary<string, string[]>();
            }

            if (interactive || filterResults)
            {
                for (int i = 0; i < queriedFiles.Length; i++)
                {
                    if (backgroundTaskIsCanceled && interactive)
                    {
                        Invoke(updateTable);
                    }

                    if (backgroundTaskIsCanceled)
                    {
                        clearAppliedPresetCache();
                        return string.Empty;
                    }

                    int trackId = GetPersistentTrackIdInt(queriedFiles[i]);

                    List<string> composedSplitGroupingTagsList = filesActualComposedSplitGroupingTagsLists[trackId];
                    foreach (string composedSplitGroupingTags in composedSplitGroupingTagsList)
                    {
                        ConvertStringsResult[] functionResults = tags[composedSplitGroupingTags];

                        if (filterResults)
                            showRow = checkCondition(composedSplitGroupingTags, functionResults);

                        if (showRow && !interactive && filterResults)
                        {
                            filteredFiles.AddSkip(queriedFiles[i]);
                            break; //Let's continue with the next file
                        }
                        else if (showRow && interactive && !composedSplitGroupingTagsReportRows.ContainsKey(composedSplitGroupingTags))
                        {
                            string[] groupingsRow = AggregatedTags.GetGroupings(composedSplitGroupingTags, groupingsDict);
                            string[] row = new string[groupingsRow.Length + functionResults.Length];

                            groupingsRow.CopyTo(row, 0);
                            for (int j = 0; j < functionResults.Length; j++)
                            {
                                string functionResult = AggregatedTags.GetField(null, functionResults, groupingsDict.Count + j, groupingsDict, //-V3080
                                        operations[j], mulDivFactors[j], precisionDigits[j], appendTexts[j]);

                                row[groupingsDict.Count + j] = functionResult;
                            }

                            composedSplitGroupingTagsReportRows.Add(composedSplitGroupingTags, row);
                        }
                    }
                }


                if (!interactive && filterResults) //Only for filtering by another preset (lastFiles which are now queriedFiles) AND by this preset condition
                {
                    lastFiles = new string[filteredFiles.Count];
                    int l = 0;
                    foreach (var file in filteredFiles.Keys)
                        lastFiles[l++] = file;
                }
                else if (interactive)
                {
                    int groupingIndex = 0;
                    int groupingsCount = composedSplitGroupingTagsReportRows.Count;

                    foreach (var row in composedSplitGroupingTagsReportRows.Values)
                    {
                        rows.Add(row);

                        if (++groupingIndex % 100 == 0)
                        {
                            SetStatusbarTextForFileOperations(LibraryReportsGeneratingPreviewSbText, true, groupingIndex, groupingsCount, appliedPreset.getName(), 0);
                            Invoke(addRowsToTable, rows);
                            rows.Clear();
                        }
                    }

                    Invoke(addRowsToTable, rows);
                    SetStatusbarTextForFileOperations(LibraryReportsGeneratingPreviewSbText, true, --groupingIndex, groupingsCount, appliedPreset.getName());
                }
            }


            if (saveResultsToTags && functionId == null) //Saving preset results
            {
                SetResultingSbText(appliedPreset.getName(), false, true);
                saveFields(queriedFiles, interactive, tags);
            }
            else if (saveResultsToTags) //Forcing LR functions cache update
            {
                saveFields(queriedFiles, interactive, tags);
            }
            else
            {
                SetResultingSbText(appliedPreset.getName(), true, true);
            }

            if (interactive)
            {
                Invoke(updateTable);
                previewIsGenerated = true;
            }

            return "...";
        }

        private void applyOnlyGroupingsPresetResults(string[] queriedFiles, bool interactive, bool filterResults)
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


            var tags = cachedPresetsTags[appliedPreset.guid];
            var filesActualComposedGroupingTags = cachedPresetsFilesActualComposedSplitGroupingTagsList[appliedPreset.guid];

            if (!interactive && filterResults) //Only for filtering by another preset (lastFiles which are now queriedFiles) AND by this preset condition
            {
                var filteredFiles = new List<string>();

                for (int i = 0; i < queriedFiles.Length; i++)
                {
                    if (backgroundTaskIsCanceled)
                    {
                        clearAppliedPresetCache();
                        return;
                    }

                    int trackId = GetPersistentTrackIdInt(queriedFiles[i]);

                    List<string> composedActualGroupingTagsList = filesActualComposedGroupingTags[trackId];
                    foreach (string composedGroupingTags in composedActualGroupingTagsList)
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
                bool showRow = true;

                List<string[]> rows = new List<string[]>();

                int groupingsCount = 0;
                int totalGroupingsCount = tags.Count;
                foreach (KeyValuePair<string, ConvertStringsResult[]> keyValue in tags)
                {
                    if (backgroundTaskIsCanceled)
                    {
                        Invoke(updateTable);

                        clearAppliedPresetCache();
                        return;
                    }

                    string[] row = AggregatedTags.GetGroupings(keyValue, groupingsDict);


                    if (filterResults)
                    {
                        string comparedValue;

                        if (comparedField == -1)
                            comparedValue = comparedFieldText;
                        else
                            comparedValue = row[comparedField];

                        showRow = checkCondition(keyValue.Key, null); //-V3080
                    }

                    if (showRow)
                    {
                        rows.Add(row);

                        //MusicBeePlugin.SetStatusbarTextForFileOperations(LibraryReportsGeneratingPreviewCommandSbText, true, groupingsCount, totalGroupingsCount, appliedPreset.getName());
                        if (groupingsCount % 100 == 0)
                        {
                            SetStatusbarTextForFileOperations(LibraryReportsGeneratingPreviewSbText, true, groupingsCount, totalGroupingsCount, appliedPreset.getName(), 0);
                            Invoke(addRowsToTable, rows);
                            rows.Clear();
                        }
                    }

                    groupingsCount++;

                    previewIsGenerated = true;
                }


                SetStatusbarTextForFileOperations(LibraryReportsGeneratingPreviewSbText, true, --groupingsCount, totalGroupingsCount, appliedPreset.getName(), 0);
                SetResultingSbText(appliedPreset.getName(), true, true);

                Invoke(addRowsToTable, rows);
                Invoke(updateTable);
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

        private string getAllLibrariesFunctionResultsInternal(string cachedResult, string newResult)
        {
            if (LrCurrentLibraryPathHash == null) //Path hash is not yet generated since MB startup
                LrCurrentLibraryPathHash = GetStringHash(GetCurrentLibraryPath());

            string appliedPresetGuid = appliedPreset.guid.ToString();
            string presetResultPostfix = LrCachedFunctionResultPresetSeparator + appliedPresetGuid + LrCachedFunctionResultLibraryPathHashSeparator;
            string resultPostfix = LrCachedFunctionResultPresetSeparator + LrCurrentLibraryPathHash + presetResultPostfix;

            if (!cachedResult.Contains(LrCachedFunctionResultLibraryPathHashSeparator)
                || !cachedResult.Contains(LrCachedFunctionResultPresetSeparator)) //Function result hasn't been cached

                return newResult + resultPostfix;


            var librariesResults = cachedResult.Split(new string[] { LrCachedFunctionResultLibraryPathHashSeparator }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < librariesResults.Length; i++)
            {
                string[] libraryResultParts = librariesResults[i].Split(new string[] { LrCachedFunctionResultPresetSeparator }, StringSplitOptions.RemoveEmptyEntries);

                if (!AllLrPresetGuidsInUse.ContainsKey(Guid.Parse(libraryResultParts[2])))
                    librariesResults[i] = null;
                else if (libraryResultParts[1] == LrCurrentLibraryPathHash && libraryResultParts[2] == appliedPresetGuid)
                    librariesResults[i] = newResult + resultPostfix;
            }

            string newFullResult = string.Empty;
            for (int i = 0; i < librariesResults.Length; i++)
            {
                if (librariesResults[i] != null)
                    newFullResult += librariesResults[i];
            }

            return newFullResult;
        }

        private string getCachedFunctionResultInternal(string cachedResult)
        {
            string appliedPresetGuid = appliedPreset.guid.ToString();
            string presetResultPostfix = LrCachedFunctionResultPresetSeparator + appliedPresetGuid + LrCachedFunctionResultLibraryPathHashSeparator;
            string resultPostfix = LrCachedFunctionResultPresetSeparator + LrCurrentLibraryPathHash + presetResultPostfix;

            if (!cachedResult.Contains(resultPostfix)) //appliedPreset hasn't been cached
                return null;


            var librariesResults = cachedResult.Split(new string[] { LrCachedFunctionResultLibraryPathHashSeparator }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < librariesResults.Length; i++)
            {
                string[] libraryResultParts = librariesResults[i].Split(new string[] { LrCachedFunctionResultPresetSeparator }, StringSplitOptions.RemoveEmptyEntries);

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
            int idIndex = getCachedFunctionResultIndex(functionId);

            if (idIndex >= 0)
                destinationTagId = GetTagId(appliedPreset.destinationTags[idIndex]);

            if (destinationTagId == 0)
                return null;


            //if (queriedFile.EndsWith(".asx")) //---
            //    return null;

            string cachedResult = GetFileTag(queriedFile, destinationTagId);

            if (LrCurrentLibraryPathHash == null) //Path hash is not yet generated since MB startup
                LrCurrentLibraryPathHash = GetStringHash(GetCurrentLibraryPath());
            else if (!cachedResult.Contains(LrCachedFunctionResultPresetSeparator + LrCurrentLibraryPathHash
                + LrCachedFunctionResultPresetSeparator)) //Maybe library has been switched since MB startup

                LrCurrentLibraryPathHash = GetStringHash(GetCurrentLibraryPath());


            int trackId = GetPersistentTrackIdInt(queriedFile);

            if (!queriedFile.EndsWith(".asx") && !cachedResult.Contains(LrCachedFunctionResultPresetSeparator + LrCurrentLibraryPathHash
                + LrCachedFunctionResultPresetSeparator)) //Function results cache hasn't been filled for the current library 
            {
                bool notYetCachedTrack = false;
                lock (LrTrackCacheNeededToBeUpdated)
                    notYetCachedTrack = LrTrackCacheNeededToBeUpdated.ContainsKey(trackId);

                //New tracks have been added to the library, and they are not yet pre-cached
                if (notYetCachedTrack)
                    return null;

                //Function results cache is not initially filled for all tracks of the library.
                //Maybe new tracks have been added to library, or function results cache is filled for the other libraries only.
                //Needed to apply all affected presets.
                int libraryPathGetHashCode = GetCurrentLibraryPath().GetHashCode();

                if (LrLastAskedCacheFillLibraryPathHash != libraryPathGetHashCode) //Let's check if the question below hs been asked already
                {
                    LrLastAskedCacheFillLibraryPathHash = libraryPathGetHashCode;

                    DialogResult result = MessageBox.Show(this, MsgLrCachedPresetsNotApplied,
                        string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                    if (result == DialogResult.No)
                        return null;


                    List<ReportPreset> notCachedPresets = new List<ReportPreset>();

                    foreach (var preset in SavedSettings.reportPresets)
                    {
                        for (int i = 0; i < preset.functionIds.Length; i++)
                        {
                            destinationTagId = 0;
                            idIndex = getCachedFunctionResultIndex(preset.functionIds[i]);

                            if (idIndex >= 0)
                                destinationTagId = GetTagId(preset.destinationTags[idIndex]);

                            if (destinationTagId != 0)
                            {
                                string checkedCachedResult = GetFileTag(queriedFile, destinationTagId);

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

                    using (LrFunctionPrecaching tagToolsForm = new LrFunctionPrecaching(TagToolsPlugin))
                        tagToolsForm.precachePresets(SavedSettings.reportPresets, notCachedPresets);

                    return CtlLrRefreshUi; //Maybe getCachedFunctionResult(queriedFile, functionId); ? //---
                }
                else
                {
                    return null;
                }
            }


            //Here all is fine with library path hash. Let's check preset cache
            return getCachedFunctionResultInternal(cachedResult);
        }

        private string getFunctionResult(string queriedFile, string functionId, AggregatedTags tags,
            SortedDictionary<int, List<string>> filesActualComposedSplitGroupingTagsLists)
        {
            ConvertStringsResult[] functionResults = null;
            int trackId = GetPersistentTrackIdInt(queriedFile);

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


            int savedFunctionIndex = savedFunctionIds.IndexOf(functionId);
            if (savedFunctionIndex >= 0)
            {
                string functionResult = AggregatedTags.GetField(null, functionResults, groupingsDict.Count + savedFunctionIndex, groupingsDict,  //-V3080
                    operations[savedFunctionIndex], mulDivFactors[savedFunctionIndex], precisionDigits[savedFunctionIndex], appendTexts[savedFunctionIndex]);

                if (destinationTagIds[savedFunctionIndex] > 0) //destinationTagIds[savedFunctionIndex] is used as cache for function id
                {
                    string cachedResult = GetFileTag(queriedFile, destinationTagIds[savedFunctionIndex]);
                    string fullNewResult = getAllLibrariesFunctionResultsInternal(cachedResult, functionResult);

                    SetFileTag(queriedFile, destinationTagIds[savedFunctionIndex], fullNewResult, true);
                    CommitTagsToFile(queriedFile, true, true);
                }

                return functionResult;
            }

            return "???";
        }

        private void saveFields(string[] queriedFiles, bool interactive, AggregatedTags tags)
        {
            var filesActualComposedGroupingTags = this.cachedPresetsFilesActualComposedSplitGroupingTagsList[appliedPreset.guid];

            for (int i = 0; i < queriedFiles.Length; i++)
            {

                int trackId = GetPersistentTrackIdInt(queriedFiles[i]);
                bool tagsSet = false;

                if (backgroundTaskIsCanceled)
                {
                    clearAppliedPresetCache();
                    return;
                }

                if (interactive)
                    SetStatusbarTextForFileOperations(LibraryReportsSbText, false, i, queriedFiles.Length, appliedPreset.getName());
                else
                    SetStatusbarTextForFileOperations(ApplyingLrPresetSbText, false, i, queriedFiles.Length, appliedPreset.getName());

                var composedActualGroupingTagsList = filesActualComposedGroupingTags[trackId];
                if (composedActualGroupingTagsList.Count != 1)
                    return;

                foreach (string composedActualGroupingTags in composedActualGroupingTagsList)
                {
                    ConvertStringsResult[] fileFunctionResults = tags[composedActualGroupingTags]; //Let's write aggregated results for entire file

                    if (checkCondition(composedActualGroupingTags, fileFunctionResults))
                    {
                        int j = -1;
                        foreach (var attribs in functionsDict.Values)
                        {
                            j++;

                            if (destinationTagIds[j] > 0 && !string.IsNullOrWhiteSpace(savedFunctionIds[j])) //destinationTagIds[j] is used as cache for function id
                            {
                                string result = AggregatedTags.GetField(composedActualGroupingTags, fileFunctionResults, groupingsDict.Count + j, groupingsDict,
                                    operations[j], mulDivFactors[j], precisionDigits[j], appendTexts[j]);

                                string cachedResult = GetFileTag(queriedFiles[i], destinationTagIds[j]);
                                string fullNewResult = getAllLibrariesFunctionResultsInternal(cachedResult, result);

                                SetFileTag(queriedFiles[i], destinationTagIds[j], fullNewResult, true);
                                tagsSet = true;
                            }
                            else if (destinationTagIds[j] > 0)
                            {
                                string result = AggregatedTags.GetField(composedActualGroupingTags, fileFunctionResults, groupingsDict.Count + j, groupingsDict,
                                    operations[j], mulDivFactors[j], precisionDigits[j], appendTexts[j]);

                                SetFileTag(queriedFiles[i], destinationTagIds[j], result, true);
                                tagsSet = true;
                            }
                        }

                        break; //Continue with the next file, because only one of composed grouping tags can be saved to the same file anyway
                    }
                    else //Let's write empty string if condition hasn't passed and custom tags are used as a cache
                    {
                        int j = -1;
                        foreach (var attribs in functionsDict.Values)
                        {
                            j++;

                            if (destinationTagIds[j] > 0 && !string.IsNullOrWhiteSpace(savedFunctionIds[j])) //destinationTagIds[j] is used as cache for function id
                            {
                                string cachedResult = GetFileTag(queriedFiles[i], destinationTagIds[j]);
                                string fullNewResult = getAllLibrariesFunctionResultsInternal(cachedResult, string.Empty);

                                SetFileTag(queriedFiles[i], destinationTagIds[j], fullNewResult, true);
                                tagsSet = true;
                            }
                        }

                        break; //Continue with the next file, because only one of composed grouping tags can be saved to the same file anyway
                    }
                }


                if (tagsSet)
                    CommitTagsToFile(queriedFiles[i], true, true);
            }


            SetResultingSbText(appliedPreset.getName(), true, true);
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


                if (!MbApiInterface.Library_QueryFilesEx("domain=Library", out string[] files))
                    files = new string[0];


                if (files.Length == 0)
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
                            executePreset(null, false, true, null, false, true);
                        }
                    }
                }
            }
            catch (ThreadAbortException)
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

                    executePreset(null, false, true, null, false, true);
                }
                catch (ThreadAbortException)
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

                    if (MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out string[] selectedFiles))
                    {
                        if (selectedFiles.Length == 0)
                        {
                            SetStatusbarText(MsgNoTracksSelected, true);
                            System.Media.SystemSounds.Exclamation.Play();
                        }
                        else
                        {
                            executePreset(selectedFiles, false, true, null, false, true);
                        }
                    }
                }
                catch (ThreadAbortException)
                {
                    //Let's just stop the thread...
                }
            }

            BackgroundTaskIsInProgress = false;
            RefreshPanels(true);
        }

        internal static void LrNotifyFileTagsChanged(string changedFile)
        {
            if (!ReportPresetIdsAreInitialized)
                return;
            else if (IdsReportPresets.Count == 0)
                return;


            //string nodeName = MbApiInterface.Library_GetFileTag(changedFile, (MetaDataType)42);
            //
            //if (nodeName != MusicName)
            //    return;

            int trackId = GetPersistentTrackIdInt(changedFile);

            lock (LrTrackCacheNeededToBeUpdated)
                LrTrackCacheNeededToBeUpdated.AddSkip(trackId, changedFile);
        }

        internal static void UpdateFunctionCacheLoop()
        {
            if (!ReportPresetIdsAreInitialized)
                return;
            else if (IdsReportPresets.Count == 0)
                return;

            Thread.CurrentThread.Priority = ThreadPriority.Lowest;


            while (!PluginClosing && ReportPresetIdsAreInitialized && IdsReportPresets.Count > 0)
            {
                bool skipFunctionsUpdate = false;

                lock (LrTrackCacheNeededToBeUpdated)
                {
                    if (LrTrackCacheNeededToBeUpdated.Count == 0)
                    {
                        skipFunctionsUpdate = true;
                    }
                    else
                    {
                        LrTrackCacheNeededToBeUpdatedWorkingCopy = new string[LrTrackCacheNeededToBeUpdated.Count];

                        int i = 0;
                        foreach (var changedFile in LrTrackCacheNeededToBeUpdated.Values)
                            LrTrackCacheNeededToBeUpdatedWorkingCopy[i++] = changedFile;

                        LrTrackCacheNeededToBeUpdated.Clear();
                    }
                }

                if (skipFunctionsUpdate)
                    goto loop_end;


                if (BackgroundTaskIsInProgress) //-V3029
                    Thread.Sleep(500);//****

                if (BackgroundTaskIsInProgress)
                    goto loop_end;


                BackgroundTaskIsInProgress = true;

                foreach (var idPreset in IdsReportPresets)
                {
                    lock (LrPresetExecutionLocker)
                    {
                        if (PresetsProcessedByFunctionCacheUpdate.AddUnique(idPreset.Value.guid))
                        {
                            LibraryReportsCommandForFunctionIds.appliedPreset = idPreset.Value;
                            LibraryReportsCommandForFunctionIds.executePreset(LrTrackCacheNeededToBeUpdatedWorkingCopy, false, true, null, false, true);
                        }
                    }
                }

                BackgroundTaskIsInProgress = false;

                LrTrackCacheNeededToBeUpdatedWorkingCopy = null;
                PresetsProcessedByFunctionCacheUpdate.Clear();

                RefreshPanels();

            loop_end:
                Thread.Sleep(FunctionCacheUpdateDelay);
            }
        }

        internal static void AutoApplyReportPresets()
        {
            if (!SavedSettings.allowAsrLrPresetAutoExecution)
                return;
            else if (LibraryReportsCommandForAutoApplying == null)
                return;
            else if (ReportPresetsForAutoApplying.Count == 0)
                return;

            if (BackgroundTaskIsInProgress)
            {
                SetStatusbarText(AnotherLrPresetIsRunningSbText, false);
                System.Media.SystemSounds.Exclamation.Play();

                return;
            }
            else
            {
                BackgroundTaskIsInProgress = true;
            }

            MbApiInterface.MB_CreateBackgroundTask(LibraryReportsCommandForAutoApplying.autoApplyReportPresetsOnStartup, null);
        }

        internal static void ApplyReportPreset(ReportPreset preset, ReportPreset[] reportPresets)
        {
            if (BackgroundTaskIsInProgress)
            {
                SetStatusbarText(AnotherLrPresetIsRunningSbText, false);
                System.Media.SystemSounds.Exclamation.Play();

                return;
            }
            else
            {
                BackgroundTaskIsInProgress = true;
            }

            LibraryReportsCommandForHotkeys.reportPresets = reportPresets;
            LibraryReportsCommandForHotkeys.appliedPreset = preset;

            if (preset.applyToSelectedTracks)
                MbApiInterface.MB_CreateBackgroundTask(LibraryReportsCommandForHotkeys.applyReportPresetToSelectedTracks, null);
            else
                MbApiInterface.MB_CreateBackgroundTask(LibraryReportsCommandForHotkeys.applyReportPresetToLibrary, null);
        }

        internal static void ApplyReportPresetByHotkey(int presetIndex)
        {
            ReportPreset preset = ReportPresetsWithHotkeys[presetIndex - 1];

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

            if (!IdsReportPresets.TryGetValue(functionId, out ReportPreset preset))
                return CtlLrIncorrectReportPresetId;


            LibraryReportsCommandForFunctionIds.appliedPreset = preset;
            string returnValue = LibraryReportsCommandForFunctionIds.getCachedFunctionResult(calculatedFile, functionId);

            if (returnValue != null)
                return returnValue;

            string[] selectedFiles = new string[] { calculatedFile };
            return LibraryReportsCommandForFunctionIds.executePreset(selectedFiles, false, true, functionId, false, false);
        }

        private static int FindFirstSlot(bool[] slots, bool searchedValue)
        {
            for (int i = 0; i < slots.Length; i++)
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
                    LrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset1EventHandler);
                    LrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset1EventHandler);
                    break;
                case 1:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset2EventHandler);
                    LrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset2EventHandler);
                    LrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset2EventHandler);
                    break;
                case 2:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset3EventHandler);
                    LrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset3EventHandler);
                    LrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset3EventHandler);
                    break;
                case 3:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset4EventHandler);
                    LrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset4EventHandler);
                    LrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset4EventHandler);
                    break;
                case 4:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset5EventHandler);
                    LrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset5EventHandler);
                    LrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset5EventHandler);
                    break;
                case 5:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset6EventHandler);
                    LrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset6EventHandler);
                    LrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset6EventHandler);
                    break;
                case 6:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset7EventHandler);
                    LrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset7EventHandler);
                    LrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset7EventHandler);
                    break;
                case 7:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset8EventHandler);
                    LrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset8EventHandler);
                    LrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset8EventHandler);
                    break;
                case 8:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset9EventHandler);
                    LrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset9EventHandler);
                    LrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset9EventHandler);
                    break;
                case 9:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset10EventHandler);
                    LrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset10EventHandler);
                    LrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset10EventHandler);
                    break;
                case 10:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset11EventHandler);
                    LrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset11EventHandler);
                    LrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset11EventHandler);
                    break;
                case 11:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset12EventHandler);
                    LrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset12EventHandler);
                    LrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset12EventHandler);
                    break;
                case 12:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset13EventHandler);
                    LrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset13EventHandler);
                    LrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset13EventHandler);
                    break;
                case 13:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset14EventHandler);
                    LrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset14EventHandler);
                    LrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset14EventHandler);
                    break;
                case 14:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset15EventHandler);
                    LrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset15EventHandler);
                    LrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset15EventHandler);
                    break;
                case 15:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset16EventHandler);
                    LrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset16EventHandler);
                    LrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset16EventHandler);
                    break;
                case 16:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset17EventHandler);
                    LrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset17EventHandler);
                    LrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset17EventHandler);
                    break;
                case 17:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset18EventHandler);
                    LrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset18EventHandler);
                    LrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset18EventHandler);
                    break;
                case 18:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset19EventHandler);
                    LrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset19EventHandler);
                    LrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset19EventHandler);
                    break;
                case 19:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), plugin.LrPreset20EventHandler);
                    LrPresetsMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset20EventHandler);
                    LrPresetsContextMenuItem?.DropDown.Items.Add(preset.getName(), null, plugin.LrPreset20EventHandler);
                    break;
                default:
                    throw new Exception(ExIncorrectLrHotkeySlot.Replace("%%SLOT%%", slot.ToString()));
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

                for (int i = 0; i < SavedSettings.reportPresets.Length; i++)
                {
                    if (SavedSettings.reportPresets[i].hotkeyAssigned)
                    {
                        ReportPreset libraryReportsPreset = new ReportPreset(SavedSettings.reportPresets[i], true);
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
                ReportPresetIdsAreInitialized = false;

                lock (IdsReportPresets)
                    IdsReportPresets.Clear();

                LibraryReportsCommandForFunctionIds?.Dispose();
                LibraryReportsCommandForFunctionIds = null;

                if (SavedSettings.dontShowLibraryReports)
                    return;

                LibraryReportsCommandForFunctionIds = new LibraryReports();

                for (int i = 0; i < SavedSettings.reportPresets.Length; i++)
                {
                    for (int j = 0; j < SavedSettings.reportPresets[i].functionIds.Length; j++)
                    {
                        if (!string.IsNullOrWhiteSpace(SavedSettings.reportPresets[i].functionIds[j]))
                        {
                            ReportPreset libraryReportsPreset = new ReportPreset(SavedSettings.reportPresets[i], true);
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
            {
                UpdateFunctionCacheThread = new Thread(UpdateFunctionCacheLoop);
                UpdateFunctionCacheThread.Start();
            }
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
            for (int i = 0; i < ReportPresetsWithHotkeys.Length; i++)
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

        private bool addColumn(string fieldName, string parameter2Name, LrFunctionType type, string splitter, bool trimValues, string expression)
        {
            DataGridViewColumn column;

            if (splitter == DontUseSplitter)
                splitter = "";

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
            }

            ColumnAttributes columnAttributes = new ColumnAttributes(type, expression, fieldName, parameter2Name, splitter, trimValues);


            string uniqueId = columnAttributes.getUniqueId();
            string shortId = columnAttributes.getShortId();

            string columnName = GetColumnName(fieldName, parameter2Name, type, splitter, trimValues, expression, true, true, true);
            string fullFieldName = GetColumnName(fieldName, parameter2Name, type, splitter, trimValues, expression, false, false, true);

            //Some column attributes are not unique, they can be replaced, and must not be included in column name
            string simpleColumnName = GetColumnName(fieldName, parameter2Name, type, null, false, expression, true, false, true);

            int oldSortedShortIdsIndex = sortedShortIds.IndexOf(shortId);
            int oldExpressionIndex = -1;
            if (shortIdsExprs.TryGetValue(shortId, out var exprs1))
                oldExpressionIndex = exprs1.IndexOf(expressionBackup);

            //Let's check if the column exists already
            int replacedColumnIndex = -1;

            if (newColumn == null) //Updating splitter, trim or expression. Let's silently replace old column 
            {
                updateColumns(shortId, fieldName, parameter2Name, type, splitter, trimValues, expression);
                expressionBackup = expression;
                splitterBackup = splitter;
                trimValuesBackup = trimValues;

                return true;
            }
            else if (groupingsDict.ContainsKey(uniqueId) || functionsDict.ContainsKey(uniqueId))
            {
                if (presetIsLoading)
                    throw new Exception(ExThisFieldAlreadyDefinedInPreset);
                else
                {
                    if (MessageBox.Show(this, MsgDoYouWantToReplaceTheField.Replace(@"\\", "\n\n").Replace("%%FIELDNAME%%", simpleColumnName),
                    string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return false;
                    else //Confirmed replacement
                        replacedColumnIndex = removeColumn(fieldName, parameter2Name, type, splitter, trimValues, expression);
                }
            }


            DataGridViewTextBoxColumn textColumnTemplate = new DataGridViewTextBoxColumn();
            column = new DataGridViewColumn(textColumnTemplate.CellTemplate)
            {
                SortMode = DataGridViewColumnSortMode.Programmatic
            };

            column.HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            column.HeaderText = columnName;
            column.HeaderCell.Tag = uniqueId;
            column.Resizable = DataGridViewTriState.True;
            column.MinimumWidth = PreviewTableColumnMinimumWidth;
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

            conditionFieldListCustom.Items.Add(fullFieldName);
            if (conditionFieldListCustom.SelectedIndex == -1)
                conditionFieldListCustom.SelectedIndex = 0;

            comparedFieldListCustom.Items.Add(fullFieldName);

            conditionCheckBox.Enable(true);
            conditionCheckBox_CheckedChanged(null, null);

            if (fieldName == ArtworkName)
            {
                DataGridViewImageColumn imageColumn = new DataGridViewImageColumn
                {
                    ImageLayout = DataGridViewImageCellLayout.Zoom,
                    HeaderText = fieldName,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                    Resizable = DataGridViewTriState.True,
                    Width = PreviewTableDefaultArtworkSize,
                    SortMode = DataGridViewColumnSortMode.Programmatic,
                };
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

                sourceFieldComboBoxCustom.Items.Add(fullFieldName);
                if (sourceFieldComboBoxCustom.SelectedIndex == -1)
                    sourceFieldComboBoxCustom.SelectedIndex = 0;
            }


            sortedShortIds = groupingsDict.idsToSortedList(false);
            sortedShortIds = functionsDict.idsToSortedList(false, sortedShortIds);
            int newSortedShortIdsIndex = sortedShortIds.IndexOf(shortId);

            expressionBackup = expression;
            splitterBackup = splitter;
            trimValuesBackup = trimValues;

            if (oldSortedShortIdsIndex == -1 || !shortIdsExprs.ContainsKey(shortId)) //New column, even not considering expressions, OR replaced (and removed above) LAST expression
            {
                shortIdsExprs.Add(shortId, new List<string> { expression });

                tagsDataGridView.Rows.Insert(newSortedShortIdsIndex, 1);
                tagsDataGridView.Rows[newSortedShortIdsIndex].Cells[0].Value = "T";
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

                var exprs = shortIdsExprs[shortId];

                if (oldExpressionIndex == -1) //It's new expression, let's add it to the end of list
                    exprs.Add(expression);
                else //It's replaced expression (already removed above, but it wasn't last one) 
                    exprs.Insert(oldExpressionIndex, expression);

                if (!presetIsLoading) //Interactive
                    fillExpressionsDataGridView(shortId, null); //New expression will be added from shortIdsExprs
            }


            if (previewTable.RowCount > 0)
                clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, buttonPreview, buttonExport, buttonClose);

            return true;
        }

        private void updateColumns(string shortId, string fieldName, string parameter2Name, LrFunctionType type, string splitter, bool trimValues, string expression)
        {
            var columnAttributes = new ColumnAttributes(type, expression, fieldName, parameter2Name, splitter, trimValues);

            for (int i = 0; i < previewTable.ColumnCount; i++)
            {
                var colUniqueId = (string)previewTable.Columns[i].HeaderCell.Tag;

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
                    string newBackup;
                    if (currentAttributes.expression == expressionBackup) //Let's replace it
                    {
                        newBackup = expression;

                        int exprIndex = shortIdsExprs[shortId].IndexOf(expressionBackup);
                        if (exprIndex != -1)
                            shortIdsExprs[shortId][exprIndex] = expression;
                    }
                    else
                    {
                        newBackup = currentAttributes.expression; //Let's keep it as is
                    }

                    var newAttributes = new ColumnAttributes(type, newBackup, fieldName, parameter2Name, splitter, trimValues);

                    currentDict.Remove(colUniqueId);
                    currentDict.Add(newAttributes.getUniqueId(), newAttributes);

                    string newColumnName = GetColumnName(fieldName, parameter2Name, type, splitter, trimValues, expression, true, true, true);

                    previewTable.Columns[i].HeaderText = newColumnName;
                    previewTable.Columns[i].HeaderCell.Tag = newAttributes.getUniqueId();

                    int tagIndex = getSelectedRow(tagsDataGridView);
                    tagsDataGridView.Rows[tagIndex].Cells[1].Tag = columnAttributes;
                    if (type == LrFunctionType.Grouping)
                        tagsDataGridView.Rows[tagIndex].Cells[3].Value = GetSplitterRepresentation(splitter, trimValues, false);

                    for (int j = 0; j < expressionsDataGridView.ColumnCount; j++)
                    {
                        if ((string)expressionsDataGridView.Rows[j].Cells[1].Tag == expressionBackup)
                        {
                            expressionsDataGridView.Rows[j].Cells[1].Value = (expression == string.Empty ? NoExpressionText : expression);
                            expressionsDataGridView.Rows[j].Cells[1].Tag = expression;
                            break;
                        }
                    }
                }
            }
        }

        private int removeColumn(string fieldName, string parameter2Name, LrFunctionType type, string splitter, bool trimValues, string expression)
        {
            string fullFieldName = GetColumnName(fieldName, parameter2Name, type, splitter, trimValues, expression, false, false, true);

            var columnAttributes = new ColumnAttributes(type, expression, fieldName, parameter2Name, splitter, trimValues);

            var uniqueId = columnAttributes.getUniqueId();
            var shortId = columnAttributes.getShortId();

            int columnIndex = -1;
            for (int i = 0; i < previewTable.ColumnCount; i++)
            {
                var colUniqueId = (string)previewTable.Columns[i].HeaderCell.Tag;

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
                int index = functionsDict.indexOf(uniqueId);

                functionsDict.RemoveUi(uniqueId, buttonOK, assignHotkeyCheckBox);

                int currentSourceFieldComboBoxSelectedIndex = sourceFieldComboBoxCustom.SelectedIndex;

                savedDestinationTagsNames.RemoveAt(index); //-V3057
                savedFunctionIds.RemoveAt(index); //-V3057

                operations.RemoveAt(index); //-V3057
                mulDivFactors.RemoveAt(index); //-V3057
                precisionDigits.RemoveAt(index); //-V3057
                appendTexts.RemoveAt(index); //-V3057

                sourceFieldComboBoxCustom.Items.RemoveAt(index);
                if (sourceFieldComboBoxCustom.SelectedIndex == -1 && sourceFieldComboBoxCustom.Items.Count > 0)
                    sourceFieldComboBoxCustom.SelectedIndex = 0;
                else if (sourceFieldComboBoxCustom.SelectedIndex == -1)
                    sourceFieldComboBox_SelectedIndexChanged(null, null);
            }


            conditionFieldListCustom.Items.Remove(fullFieldName);
            if (conditionFieldListCustom.SelectedIndex == -1 && conditionFieldListCustom.Items.Count > 0)
                conditionFieldListCustom.SelectedIndex = 0;


            if (comparedFieldListCustom.Text == (string)comparedFieldListCustom.SelectedItem)
                comparedFieldListCustom.Text = string.Empty;

            comparedFieldListCustom.Items.Remove(fullFieldName);

            if (conditionFieldListCustom.Items.Count == 0)
            {
                conditionCheckBox.Enable(false);
                conditionCheckBox.Checked = false;
            }


            previewTable.Columns.RemoveAt(columnIndex);


            //Now let's deal with field & expression tables
            int sortedShortIdsIndex = sortedShortIds.IndexOf(shortId);

            var exprs = shortIdsExprs[shortId];
            exprs.Remove(expression);
            if (exprs.Count == 0)
            {
                shortIdsExprs.Remove(shortId);
                sortedShortIds.Remove(shortId);

                tagsDataGridView.Rows.RemoveAt(sortedShortIdsIndex);

                string newShortId = null;
                tagsDataGridViewSelectedRow = getSelectedRow(tagsDataGridView);
                if (tagsDataGridViewSelectedRow != -1)
                    newShortId = (string)tagsDataGridView.Rows[tagsDataGridViewSelectedRow].Cells[0].Tag;

                fillExpressionsDataGridView(newShortId, false);
            }
            else
            {
                fillExpressionsDataGridView(shortId, false);
            }


            if (previewTable.ColumnCount == 0)
                clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, buttonPreview, buttonExport, buttonClose);

            if (!presetIsLoading)
                setPresetChanged();

            return columnIndex;
        }

        private ReportPreset[] getReportPresetsArrayUI()
        {
            var reportPresetsLocal = new ReportPreset[presetList.Items.Count];
            for (int i = 0; i < presetList.Items.Count; i++)
                reportPresetsLocal[i] = presetList.Items[i] as ReportPreset;

            return reportPresetsLocal;
        }

        private bool prepareBackgroundPreview()
        {
            smartOperation = smartOperationCheckBox.Checked;

            resultsAreFiltered = false;
            buttonFilterResultsChangeLabel();

            if (!presetIsLoading && previewTable.RowCount == 0)
                presetTabControl.SelectedTab = tabPage1;

            if (previewTable.RowCount > 0)
            {
                previewTable.RowCount = 0;

                UpdateCustomScrollBars(previewTable);
            }

            if (previewIsGenerated)
            {
                previewIsGenerated = false;
                return true;
            }

            previewIsGenerated = false;

            if (backgroundTaskIsWorking())
                return true;


            if (previewTable.ColumnCount == 0)
            {
                //MessageBox.Show(this, plugin.msgNoTagsSelected);
                return false;
            }

            maxWidths = new int[previewTable.ColumnCount];

            reportPresets = getReportPresetsArrayUI();
            appliedPreset = selectedPreset;

            return true;
        }

        private void previewTrackList()
        {
            if (!MbApiInterface.Library_QueryFilesEx("domain=DisplayedFiles", out string[] files) || files.Length == 0)
            {
                SetStatusbarText(MsgNoTracksInCurrentView, false);
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }

            reportPresets = getReportPresetsArrayUI();
            appliedPreset = selectedPreset;
            executePreset(files, true, false, null, false, false);

            this.cachedPresetsTags.TryGetValue(appliedPreset.guid, out previewedTags);
            this.cachedPresetsFilesActualComposedSplitGroupingTagsList.TryGetValue(appliedPreset.guid, out previewedFilesActualComposedSplitGroupingTagsList);
        }

        private void exportTrackList(bool openReport)
        {
            const int CdBookletArtworkSize = 550; //Height and width of CD booklet artwork
            string fileDirectoryPath = null;
            string reportFullFileName = null;
            string reportOnlyFileName = null;

            ExportedDocument document = null;
            int seqNumField = -1;
            int albumArtistField = -1;
            int albumField = -1;
            int titleField = -1;
            int durationField = -1;
            Bitmap pic = null;

            backgroundTaskIsCanceled = false;

            if (!checkPreview())
                return;

            int j = -1;
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
            }


            if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentByAlbums && (albumArtistField != 0 || albumField != 1 || artworkField != 2))
            {
                MessageBox.Show(this, MsgFirstThreeGroupingFieldsInPreviewTableShouldBe,
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
                SaveFileDialog dialog = new SaveFileDialog
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
                    foreach (ReportPreset preset in SavedSettings.reportPresets)
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

            string imagesDirectoryName = reportOnlyFileName + ".files";


            var tags = cachedPresetsTags[selectedPreset.guid];

            //Album artists/Albums for HTML grouped by albums, Album artists/sequence number of album artist for CD Booklets [Albums]
            SortedDictionary<string, List<string>> albumArtistsAlbums = new SortedDictionary<string, List<string>>();
            string base64Artwork = null;

            List<int> albumTrackCounts = new List<int>();

            System.IO.FileStream stream = new System.IO.FileStream(reportFullFileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);

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

                        List<string> currentAlbumArtistAlbums = null;
                        string[] groupingsValues1 = null;
                        string prevAlbum1 = null;
                        string prevAlbumArtist1 = null;
                        int albumArtistsAlbumsSequenceNumber = 0;

                        int trackCount = 0;
                        foreach (KeyValuePair<string, ConvertStringsResult[]> keyValue in tags)
                        {
                            groupingsValues1 = AggregatedTags.GetGroupings(keyValue, groupingsDict);


                            if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentCdBooklet)
                            {
                                if (base64Artwork == null)
                                    base64Artwork = groupingsValues1[artworkField];
                                else if (base64Artwork == string.Empty)
                                    ; //Nothing...
                                else if (base64Artwork != groupingsValues1[artworkField])
                                    base64Artwork = string.Empty;
                            }


                            if (prevAlbumArtist1 != groupingsValues1[albumArtistField] || prevAlbum1 != groupingsValues1[albumField]) //-V3106
                            {
                                if (prevAlbumArtist1 != groupingsValues1[albumArtistField]) //-V3106
                                {
                                    if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentCdBooklet)
                                        albumArtistsAlbumsSequenceNumber++;

                                    prevAlbumArtist1 = groupingsValues1[albumArtistField]; //-V3106
                                    currentAlbumArtistAlbums = new List<string>();

                                    albumArtistsAlbums.Add(prevAlbumArtist1 + albumArtistsAlbumsSequenceNumber.ToString("D4"), currentAlbumArtistAlbums);
                                }
                                else
                                {
                                    currentAlbumArtistAlbums = albumArtistsAlbums[prevAlbumArtist1 + albumArtistsAlbumsSequenceNumber.ToString("D4")];
                                }

                                prevAlbum1 = groupingsValues1[albumField]; //-V3106

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
                        document = new HtmlDocument(stream, TagToolsPlugin, selectedPreset.getName(), fileDirectoryPath, imagesDirectoryName);
                        break;
                    case LrReportFormat.HtmlTable:
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
                int fontSize = 12; //Font size units are pixels
                int labelHeight = (int)(fontSize * 2.6f * 125 / artworkGridMaxImageSize);

                if (labelHeight < (int)(fontSize * 1.3f))
                    labelHeight = (int)(fontSize * 1.3f);

                const string color = "#000000";//***

                (document as HtmlDocumentAlbumGrid).writeHeader(color, artworkGridRowImageCount, artworkGridMaxImageSize, artworkGridMaxImageSize, labelHeight, fontSize);
            }
            else if (selectedPreset.fileFormatIndex != LrReportFormat.HtmlDocumentCdBooklet)
            {
                document.writeHeader();
            }
            else //It's CD booklet
            {
                Bitmap scaledPic;

                if (base64Artwork == string.Empty) //There are various artworks
                {
                    pic = artworks[DefaultArtworkHash];
                }
                else //All tracks have the same artwork
                {
                    pic = artwork;
                }

                float xSF = CdBookletArtworkSize / (float)pic.Width;
                float ySF = CdBookletArtworkSize / (float)pic.Height;
                float SF;

                if (xSF >= ySF)
                    SF = ySF;
                else
                    SF = xSF;


                scaledPic = new Bitmap((int)Math.Round(pic.Width * SF), (int)Math.Round(pic.Height * SF), System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                Graphics gr_dest = Graphics.FromImage(scaledPic);
                gr_dest.DrawImage(pic, 0, 0, scaledPic.Width, scaledPic.Height);
                gr_dest.Dispose();

                (document as HtmlDocumentCDBooklet).writeHeader(CdBookletArtworkSize, GetBitmapAverageColor(scaledPic), scaledPic,
                    albumArtistsAlbums, tags.Count);

                scaledPic.Dispose();
            }


            if (selectedPreset.fileFormatIndex != LrReportFormat.M3u && selectedPreset.fileFormatIndex != LrReportFormat.HtmlDocumentCdBooklet
                && selectedPreset.fileFormatIndex != LrReportFormat.HtmlDocumentAlbumGrid) //Lets write table headers
            {
                int k = -1;
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


            bool multipleAlbumArtists = false;
            bool multipleAlbums = false;
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


            int height = 0;
            int i = 0;

            string prevAlbum = null;
            string prevAlbumArtist = null;

            SortedDictionary<string, bool> allUrls = null;
            if (selectedPreset.fileFormatIndex == LrReportFormat.M3u) //It's M3U playlist
                allUrls = new SortedDictionary<string, bool>();


            int groupingCount = 0;
            foreach (KeyValuePair<string, ConvertStringsResult[]> keyValue in tags)
            {
                if (backgroundTaskIsCanceled)
                    return;

                if (checkCondition(keyValue.Key, keyValue.Value))
                {
                    groupingCount++;

                    string[] groupingsValues = AggregatedTags.GetGroupings(keyValue, groupingsDict); //It's the values (in correct order) of all groupings for current grouping set

                    if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentByAlbums)
                    {
                        if (prevAlbumArtist != groupingsValues[albumArtistField]) //-V3106
                        {
                            i++;
                            prevAlbumArtist = groupingsValues[albumArtistField]; //-V3106
                            prevAlbum = groupingsValues[albumField]; //-V3106
                            document.beginAlbumArtist(groupingsValues[albumArtistField], groupingsValues.Length - 2 + functionsDict.Count); //-V3106
                            document.beginAlbum(groupingsValues[albumField], artworks[groupingsValues[artworkField]], groupingsValues[artworkField], albumTrackCounts[i]); //-V3106
                        }
                        else if (prevAlbum != groupingsValues[albumField]) //-V3106
                        {
                            i++;
                            prevAlbum = groupingsValues[albumField]; //-V3106
                            document.beginAlbum(groupingsValues[albumField], artworks[groupingsValues[artworkField]], groupingsValues[artworkField], albumTrackCounts[i]); //-V3106
                        }
                    }


                    if (selectedPreset.fileFormatIndex == LrReportFormat.M3u) //It's M3U playlist
                    {
                        SortedDictionary<string, bool> urls = keyValue.Value[0].urls;

                        foreach (var urlPair in urls)
                            allUrls.AddSkip(urlPair.Key);
                    }
                    else if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentAlbumGrid) //Album grid
                    {
                        pic = artworks[groupingsValues[artworkField]];
                        string albumLabel = groupingsValues[0];

                        for (int l = 3; l < groupingsValues.Length; l++) //groupingsValues: 1st value MUST BE album name, 2nd value MUST BE artwork
                            albumLabel += "/" + groupingsValues[l];

                        (document as HtmlDocumentAlbumGrid).addCellToRow(pic, albumLabel, groupingsValues[artworkField]);
                    }
                    else if (selectedPreset.fileFormatIndex != LrReportFormat.HtmlDocumentCdBooklet) //It's NOT a CD booklet
                    {
                        int l = -1;
                        foreach (var attribs in groupingsDict.Values)
                        {
                            l++;

                            if (l == artworkField && (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentByAlbums
                                || selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocument
                                || selectedPreset.fileFormatIndex == LrReportFormat.HtmlTable)) //Export images
                            {
                                pic = artworks[groupingsValues[artworkField]];

                                height = pic.Height;

                                document.addCellToRow(pic, attribs.getColumnName(true, true, true), groupingsValues[l], pic.Width, pic.Height);
                            }
                            else if (l == artworkField && selectedPreset.fileFormatIndex != LrReportFormat.HtmlDocumentByAlbums
                                && selectedPreset.fileFormatIndex != LrReportFormat.HtmlDocument
                                && selectedPreset.fileFormatIndex != LrReportFormat.HtmlTable) //Export image hashes
                                document.addCellToRow(groupingsValues[artworkField], attribs.getColumnName(true, true, true), columnsRightAlignment[l],
                                    l == albumArtistField, l == albumField);
                            else //Its not the artwork column
                                document.addCellToRow(groupingsValues[l], attribs.getColumnName(true, true, true), columnsRightAlignment[l],
                                    l == albumArtistField, l == albumField);
                        }

                        l = -1;
                        foreach (var attribs in functionsDict.Values)
                        {
                            l++;

                            document.addCellToRow(AggregatedTags.GetField(keyValue.Key, keyValue.Value, groupingsDict.Count + l, groupingsDict,
                                operations[l], mulDivFactors[l], precisionDigits[l], appendTexts[l]),
                                attribs.getColumnName(true, true, true), columnsRightAlignment[groupingsDict.Count + l], false, false);
                        }
                    }
                    else //It's a CD booklet
                    {
                        if (!multipleAlbumArtists) //1 album artist
                        {
                            if (!multipleAlbums) //1 album
                            {
                                (document as HtmlDocumentCDBooklet).addTrack(int.Parse(groupingsValues[seqNumField]), //-V3106
                                    null, null, groupingsValues[titleField], groupingsValues[durationField]); //-V3106
                            }
                            else //Several albums
                            {
                                (document as HtmlDocumentCDBooklet).addTrack(int.Parse(groupingsValues[seqNumField]), //-V3106
                                    null, groupingsValues[albumField], groupingsValues[titleField], groupingsValues[durationField]); //-V3106
                            }
                        }
                        else //Several album artists
                        {
                            (document as HtmlDocumentCDBooklet).addTrack(int.Parse(groupingsValues[seqNumField]), //-V3106
                                groupingsValues[albumArtistField], groupingsValues[albumField], groupingsValues[titleField], //-V3106
                                groupingsValues[durationField]); //-V3106
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
                        document.writeRow(height);
                    }

                    height = 0;
                }
            }

            if (selectedPreset.fileFormatIndex == LrReportFormat.M3u) //It's M3U playlist
            {
                foreach (var url in allUrls.Keys)
                {
                    document.addCellToRow(url, UrlTagName, false, false, false);
                    document.writeRow(0);
                }
            }
            else if (selectedPreset.fileFormatIndex != LrReportFormat.HtmlDocumentCdBooklet) //It's NOT a CD booklet
                document.writeFooter();
            else
                (document as HtmlDocumentCDBooklet).writeFooter(albumArtistsAlbums);

            document.close();

            if (openReportCheckBox.Checked)
            {
                var waiter = new Thread(OpenReport);
                waiter.Start(reportFullFileName);
            }
        }

        internal static void OpenReport(object document)
        {
            string documentPathFileName = document as string;

            System.Diagnostics.Process.Start(documentPathFileName);

            return; //Code below does not work reliably

            if (ProcessedReportDeletions.Contains(documentPathFileName)) //-V3142
                return;

            ProcessedReportDeletions.Add(documentPathFileName);

            const int waitingTimeout = 5; //In milliseconds

            int retryCount = 0;
            bool reportHasBeenLocked = false;
            while (!reportHasBeenLocked && retryCount < 30000 / waitingTimeout)
            {
                try
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(documentPathFileName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None)) //-V3156
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
                    using (System.IO.FileStream stream = new System.IO.FileStream(documentPathFileName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None)) //-V3156
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
            bool retry = true;
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

            string documentExtension = Regex.Replace(documentPathFileName, @"^.*(\..*)", "$1"); //-V3156
            if (retry || documentExtension != ".htm")
            {
                ProcessedReportDeletions.Remove(documentPathFileName);
                return;
            }

            Thread.Sleep(10000);

            ProcessedReportDeletions.Remove(documentPathFileName);
            string imagesDirectoryPath = Regex.Replace(documentPathFileName, @"^(.*)\..*", "$1") + ".files"; //-V3156
            if (System.IO.Directory.Exists(imagesDirectoryPath))
                System.IO.Directory.Delete(imagesDirectoryPath, true);
        }

        private bool checkPreview()
        {
            if (backgroundTaskIsWorking())
                return false;

            if (previewTable.RowCount == 0)
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
                checkAdjustFilteringPreset();
            }


            List<ReportPreset> newOrChangedCachedPresets = new List<ReportPreset>();
            ReportPreset[] allCurrentPresets = new ReportPreset[presetList.Items.Count];
            presetList.Items.CopyTo(allCurrentPresets, 0);

            foreach (ReportPreset changedPreset in presetList.Items)
            {
                bool isItNewPreset = true;

                lock (SavedSettings.reportPresets)
                {
                    foreach (ReportPreset savedPreset in SavedSettings.reportPresets)
                    {
                        if (changedPreset.permanentGuid == savedPreset.permanentGuid)
                        {
                            isItNewPreset = false;

                            if (changedPreset.guid != savedPreset.guid)
                            {
                                for (int i = 0; i < changedPreset.functions.Length; i++)
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
                    for (int i = 0; i < changedPreset.functions.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(changedPreset.functionIds[i]) && !IsTagEmpty(changedPreset.destinationTags[i]))
                            newOrChangedCachedPresets.Add(changedPreset);
                    }
                }
            }


            int notEmptyPresetCount = 0;
            for (int i = 0; i < presetList.Items.Count; i++)
            {
                var preset = presetList.Items[i] as ReportPreset;

                if (preset.groupings.Length > 0 || preset.functions.Length > 0)
                {
                    preset.initialGuid = preset.guid;
                    notEmptyPresetCount++;
                }
            }

            lock (SavedSettings.reportPresets)
            {
                var presetCache = SavedSettings.reportPresets;

                int skippedPresetCount = 0;
                SavedSettings.reportPresets = new ReportPreset[notEmptyPresetCount];
                for (int i = 0; i < presetList.Items.Count; i++)
                {
                    var preset = presetList.Items[i] as ReportPreset;

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
                DialogResult result = MessageBox.Show(this, GetPluralForm(MsgLrCachedPresetsChanged, newOrChangedCachedPresets.Count)
                    .Replace("%%CHANGED-PRESET-COUNT%%", newOrChangedCachedPresets.Count.ToString()),
                    string.Empty, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                if (result == DialogResult.Cancel)
                    return false;
                else if (result == DialogResult.Yes)
                    using (LrFunctionPrecaching tagToolsForm = new LrFunctionPrecaching(TagToolsPlugin))
                        _ = tagToolsForm.precachePresets(allCurrentPresets, newOrChangedCachedPresets);
            }


            return true;
        }

        internal override void enableDisablePreviewOptionControls(bool enable, bool dontChangeDisabled = false)
        {
            (allPresetsCheckStatusIsSenseless, allPresetsCheckStatusIsBroken) = checkAllPresetChains(false);

            buttonSaveClose.Enable(!allPresetsCheckStatusIsSenseless && !allPresetsCheckStatusIsBroken);

            buttonAddPreset.Enable(enable);

            if (selectedPreset == null)
                enable = false;


            buttonCopyPreset.Enable(enable);
            buttonDeletePreset.Enable(enable && selectedPreset.userPreset); //-V3125

            smartOperationCheckBox.Enable(enable && !previewIsGenerated);

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

            totalsCheckBox.Enable(enable && !previewIsGenerated);

            useAnotherPresetAsSourceCheckBox.Enable(enable && !previewIsGenerated);
            useAnotherPresetAsSourceComboBoxCustom.Enable(enable && useAnotherPresetAsSourceCheckBox.Checked && !previewIsGenerated);

            conditionCheckBox.Enable(enable && !previewIsGenerated);
            conditionFieldListCustom.Enable(enable && !previewIsGenerated && conditionCheckBox.Checked);
            conditionListCustom.Enable(enable && !previewIsGenerated && conditionCheckBox.Checked);
            comparedFieldListCustom.Enable(enable && !previewIsGenerated && conditionCheckBox.Checked);

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
            presetList.Enable(!backgroundTaskIsScheduled && (newColumn == false));

            buttonOK.Enable((previewIsGenerated || SavedSettings.allowCommandExecutionWithoutPreview) && functionsDict.Count > 0);
            buttonPreview.Enable(true);
        }

        internal override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enable(false);
            buttonPreview.Enable(false);
        }

        private void buttonAddPreset_Click(object sender, EventArgs e)
        {
            presetList.Items.Insert(0, new ReportPreset(ExportedTrackList));
            presetList.SelectedIndex = 0;

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

            int currentIndex = presetList.SelectedIndex;
            ReportPreset reportsPresetCopy = new ReportPreset(selectedPreset, false);
            if (currentIndex < firstPredefinedPresetIndex)
                presetList.Items.Insert(currentIndex + 1, reportsPresetCopy);
            else
                presetList.Items.Insert(0, reportsPresetCopy);

            presetList.SelectedItem = reportsPresetCopy;

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

                selectedPreset.useAnotherPresetAsSource = useAnotherPresetAsSourceCheckBox.Checked;
                if (!selectedPreset.useAnotherPresetAsSource)
                    selectedPreset.anotherPresetAsSource = default;
                else if (useAnotherPresetAsSourceComboBoxCustom.SelectedIndex != -1)
                    selectedPreset.anotherPresetAsSource = (ReportPresetReference)useAnotherPresetAsSourceComboBoxCustom.SelectedItem;

                selectedPreset.autoApply = presetList.GetItemChecked(presetList.SelectedIndex);

                selectedPreset.destinationTags = new string[savedDestinationTagsNames.Count];

                savedDestinationTagsNames.CopyTo(selectedPreset.destinationTags, 0);


                selectedPreset.operations = new int[sourceFieldComboBoxCustom.Items.Count];
                selectedPreset.mulDivFactors = new string[sourceFieldComboBoxCustom.Items.Count];
                selectedPreset.precisionDigits = new string[sourceFieldComboBoxCustom.Items.Count];
                selectedPreset.appendTexts = new string[sourceFieldComboBoxCustom.Items.Count];

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

                (_, bool relatedPresetsCheckStatusIsSenseless, bool relatedPresetsCheckStatusIsBroken) = checkPresetChainDeeper(getReportPresetsArrayUI(), selectedPreset, selectedPreset, false);
                if (!relatedPresetsCheckStatusIsSenseless && !relatedPresetsCheckStatusIsBroken)
                    AllLrPresetGuidsInUse.AddSkip(selectedPreset.guid);

                presetList.Refresh();
            }
        }

        private void buttonDeletePreset_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this, MsgDeletePresetConfirmation, string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

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

                int presetListSelectedIndex = presetList.SelectedIndex;
                presetList.Items.RemoveAt(presetList.SelectedIndex);

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

            SetMultipleItemsSplitterComboBoxCue(splitterBackup);

            if (sourceFieldComboBoxCustom.SelectedIndex == -1)
            {
                if (idTextBox.Text != string.Empty)
                {
                    MessageBox.Show(this, MsgFirstSelectWhichFieldYouWantToAssignFunctionIdTo, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    idTextBox.Text = string.Empty;
                }

                return;
            }

            if (idTextBox.Text == string.Empty)
            {
                savedFunctionIds[sourceFieldComboBoxCustom.SelectedIndex] = string.Empty;

                setPresetChanged();

                return;
            }

            string allowedRemoved = Regex.Replace(idTextBox.Text, @"[0-9]", string.Empty);
            allowedRemoved = Regex.Replace(allowedRemoved, @"[a-z]", string.Empty);
            allowedRemoved = Regex.Replace(allowedRemoved, @"[A-Z]", string.Empty);
            allowedRemoved = Regex.Replace(allowedRemoved, @"\-", string.Empty);
            allowedRemoved = Regex.Replace(allowedRemoved, @"_", string.Empty);
            allowedRemoved = Regex.Replace(allowedRemoved, @"\:", string.Empty);

            if (allowedRemoved != string.Empty)
            {
                MessageBox.Show(this, MsgNotAllowedSymbols, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                savedFunctionIds[sourceFieldComboBoxCustom.SelectedIndex] = string.Empty;
                idTextBox.Text = string.Empty;
            }
            else
            {
                foreach (ReportPreset preset in presetList.Items) //Lets iterate through other (saved) presets
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

                for (int i = 0; i < sourceFieldComboBoxCustom.Items.Count; i++) //Lets iterate through current preset
                {
                    string id = savedFunctionIds[i];

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
            clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, buttonPreview, buttonExport, buttonClose);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            ApplyReportPreset(selectedPreset, getReportPresetsArrayUI());
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
            previewTable.RowCount = 0;

            if (backgroundTaskIsWorking())
            {
                UpdateCustomScrollBars(previewTable);

                return;
            }

            if (previewTable.ColumnCount == 0)
            {
                UpdateCustomScrollBars(previewTable);

                //MessageBox.Show(this, plugin.msgNoTagsSelected);
                return;
            }

            appliedPreset = selectedPreset;
            reportPresets = getReportPresetsArrayUI();

            prepareConditionRelatedLocals();

            backgroundTaskIsCanceled = false;
            backgroundTaskIsScheduled = false;

            if (resultsAreFiltered)
                applyPresetResults(null, previewedTags, previewedFilesActualComposedSplitGroupingTagsList, true, false, null, false);
            else
                applyPresetResults(null, previewedTags, previewedFilesActualComposedSplitGroupingTagsList, true, false, null, true);

            resultsAreFiltered = !resultsAreFiltered;

            UpdateCustomScrollBars(previewTable);

            buttonFilterResultsChangeLabel();
        }

        private void LibraryReports_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (PluginClosing)
            {
                return;
            }
            else if (unsavedChanges)
            {
                MessageBoxDefaultButton lastAnswer = SavedSettings.unsavedChangesConfirmationLastAnswer;
                MessageBoxButtons confirmationButtons = MessageBoxButtons.YesNo;
                DialogResult result;
                (bool allPresetsCheckStatusIsSenseless, bool allPresetsCheckStatusIsBroken) = checkAllPresetChains(false);

                if (allPresetsCheckStatusIsSenseless || allPresetsCheckStatusIsBroken)
                {
                    confirmationButtons = MessageBoxButtons.OKCancel;

                    string message;
                    if (allPresetsCheckStatusIsSenseless && !allPresetsCheckStatusIsBroken)
                        message = UseAnotherPresetAsSourceIsSenselessToolTip.Replace("\r", string.Empty) + "\n\n";
                    else if (!allPresetsCheckStatusIsSenseless && allPresetsCheckStatusIsBroken)
                        message = UseAnotherPresetAsSourceIsInBrokenChainToolTip.Replace("\r", string.Empty) + "\n\n";
                    else //if (relatedPresetsCheckStatusIsSenseless && relatedPresetsCheckStatusIsBroken)
                        message = UseAnotherPresetAsSourceIsSenselessToolTip.Replace("\r", string.Empty)
                            + "\n\n" + UseAnotherPresetAsSourceIsInBrokenChainToolTip.Replace("\r", string.Empty) + "\n\n";

                    result = MessageBox.Show(this, message + MsgLrDoYouWantToCloseTheWindowWithoutSavingChanges.ToUpper(),
                        string.Empty, confirmationButtons, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                }
                else
                {
                    if (!forceCloseForms)
                        confirmationButtons = MessageBoxButtons.YesNoCancel;

                    result = MessageBox.Show(this, MsgLrDoYouWantToSaveChangesBeforeClosingTheWindow,
                        string.Empty, confirmationButtons, MessageBoxIcon.Warning, lastAnswer);
                }


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

        private void LibraryReports_Load(object sender, EventArgs e)
        {
            (int, int, int, int, int, int, int) value = loadWindowLayout();

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
                SetMultipleItemsSplitterComboBoxCue(splitterBackup);
                multipleItemsSplitterTrimCheckBox.Checked = trimValuesBackup;


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

            //allPresetsCheckStatusIsSenseless and allPresetsCheckStatusIsBroken are right now updated in enableDisablePreviewOptionControls()
            buttonSaveClose.Enable(!allPresetsCheckStatusIsSenseless && !allPresetsCheckStatusIsBroken);
            buttonClose.Enable(true);

            enableQueryingOrUpdatingButtons();

            tagsDataGridView.Enable(true);
            expressionsDataGridView.Enable(true);

            buttonUpdateFunction.Text = ButtonUpdateFunctionSaveText;

            toolTip1.SetToolTip(buttonUpdateFunction, ButtonUpdateFunctionSaveToolTip);
            buttonUpdateFunction.Image = Resources.transparent_15;

            expressionTextBox.Text = expressionBackup;
            SetMultipleItemsSplitterComboBoxCue(splitterBackup);
            multipleItemsSplitterTrimCheckBox.Checked = trimValuesBackup;

            enableDisablePreviewOptionControls(true);
        }

        private void buttonAddFunction_Click(object sender, EventArgs e)
        {
            setColumnChanged(true);
        }

        private void buttonUpdateFunction_Click(object sender, EventArgs e)
        {
            if (addColumn(sourceTagListCustom.Text, parameter2ComboBoxCustom.Text, (LrFunctionType)functionComboBoxCustom.SelectedIndex,
                multipleItemsSplitterComboBoxCustom.Text, multipleItemsSplitterTrimCheckBox.Checked, expressionTextBox.Text))
            {
                setColumnSaved();
                setPresetChanged();

                UpdateCustomScrollBars(previewTable);
                UpdateCustomScrollBars(tagsDataGridView);
                UpdateCustomScrollBars(expressionsDataGridView);
            }
        }

        private void tagsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0 && newColumn == false && selectedPreset?.userPreset == true)
            {
                var shortId = (string)tagsDataGridView.Rows[e.RowIndex].Cells[0].Tag;
                var commonAttr = (ColumnAttributes)tagsDataGridView.Rows[e.RowIndex].Cells[1].Tag;

                List<string> ids;
                if (commonAttr.functionType == LrFunctionType.Grouping)
                {
                    ids = groupingsDict.getAllIdsByShortId(shortId);
                    foreach (var id in ids)
                    {
                        var attribs = groupingsDict[id];
                        removeColumn(attribs.parameterName, null, LrFunctionType.Grouping, attribs.splitter, attribs.trimValues, attribs.expression);

                        UpdateCustomScrollBars(previewTable);
                        UpdateCustomScrollBars(tagsDataGridView);
                        UpdateCustomScrollBars(expressionsDataGridView);
                    }
                }
                else
                {
                    ids = functionsDict.getAllIdsByShortId(shortId);
                    foreach (var id in ids)
                    {
                        var attribs = functionsDict[id];
                        removeColumn(attribs.parameterName, attribs.parameter2Name, attribs.functionType, null, false, attribs.expression);

                        UpdateCustomScrollBars(previewTable);
                        UpdateCustomScrollBars(tagsDataGridView);
                        UpdateCustomScrollBars(expressionsDataGridView);
                    }
                }
            }
        }
        private void tagsDataGridViewRowSelected(int rowIndex)
        {
            if (rowIndex >= 0) //Maybe new row is selected
            {
                tagsDataGridViewSelectedRow = rowIndex;

                var shortId = (string)tagsDataGridView.Rows[rowIndex].Cells[0].Tag;

                var commonAttr = (ColumnAttributes)tagsDataGridView.Rows[rowIndex].Cells[1].Tag;

                splitterBackup = commonAttr.splitter;
                trimValuesBackup = commonAttr.trimValues;

                functionComboBoxCustom.SelectedIndex = (int)commonAttr.functionType;
                sourceTagListCustom.SelectedItem = commonAttr.parameterName;
                parameter2ComboBoxCustom.SelectedItem = commonAttr.parameter2Name;
                SetMultipleItemsSplitterComboBoxCue(commonAttr.splitter);
                multipleItemsSplitterTrimCheckBox.Checked = commonAttr.trimValues;


                fillExpressionsDataGridView(shortId, false);
            }
            else
            {
                expressionsDataGridView.RowCount = 0;
            }

            enableDisablePreviewOptionControls(true);

            UpdateCustomScrollBars(expressionsDataGridView);
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
            int index = getSelectedRow(expressionsDataGridView);

            if (expressionsDataGridView.RowCount > 0)
                expressionsDataGridView.RowCount = 0;

            if (tagsDataGridViewSelectedRow == -1)
                return;

            var exprs = shortIdsExprs[shortId];
            foreach (var expr in exprs)
            {
                expressionsDataGridView.Rows.Add();
                expressionsDataGridView.Rows[expressionsDataGridView.RowCount - 1].Cells[0].Value = "T";
                expressionsDataGridView.Rows[expressionsDataGridView.RowCount - 1].Cells[1].Value = (expr == string.Empty ? NoExpressionText : expr);
                expressionsDataGridView.Rows[expressionsDataGridView.RowCount - 1].Cells[1].Tag = expr;

                if (selectedPreset?.userPreset == true)
                    expressionsDataGridView.Rows[expressionsDataGridView.RowCount - 1].Cells[0].ToolTipText = ExpressionsDataGridViewCheckBoxToolTip;
            }

            deselectAllRows(expressionsDataGridView);

            if (newExpression == false)
            {
                selectRow(expressionsDataGridView, 0);
                expressionBackup = (string)expressionsDataGridView.Rows[0].Cells[1].Tag;
                expressionTextBox.Text = expressionBackup;
            }
            else if (newExpression == null)
            {
                selectRow(expressionsDataGridView, index);
                expressionBackup = (string)expressionsDataGridView.Rows[index].Cells[1].Tag;
                expressionTextBox.Text = expressionBackup;
            }
            else
            {
                int lastIndex = expressionsDataGridView.RowCount - 1;
                selectRow(expressionsDataGridView, lastIndex);
                expressionBackup = (string)expressionsDataGridView.Rows[lastIndex].Cells[1].Tag;
                expressionTextBox.Text = expressionBackup;
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
                        .Replace("%%FIELDNAME%%", commonAttr.getColumnName(false, false, false)),
                        string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;
                }

                string expression = (string)expressionsDataGridView.Rows[e.RowIndex].Cells[1].Tag;
                removeColumn(commonAttr.parameterName, commonAttr.parameter2Name, commonAttr.functionType, commonAttr.splitter, commonAttr.trimValues, expression);

                UpdateCustomScrollBars(previewTable);
                UpdateCustomScrollBars(tagsDataGridView);
                UpdateCustomScrollBars(expressionsDataGridView);
            }
        }

        private void expressionsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 0 && e.RowIndex >= 0) //Maybe new row is selected
            {
                expressionBackup = (string)expressionsDataGridView.Rows[e.RowIndex].Cells[1].Tag;
                expressionTextBox.Text = expressionBackup;

                enableDisablePreviewOptionControls(true);
            }
        }

        private void sourceTagList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sourceTagListCustom.Text == ArtworkName || sourceTagListCustom.Text == SequenceNumberName)
            {
                expressionBackup = null;
            }
            else if (expressionBackup == null)
            {
                if (expressionsDataGridView.SelectedRows.Count > 0)
                {
                    int index = expressionsDataGridView.SelectedRows[0].Index;
                    expressionBackup = (string)expressionsDataGridView.Rows[index].Cells[1].Tag;
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

        internal void SetMultipleItemsSplitterComboBoxCue(string cue)
        {
            if (destinationTagListCustom.SelectedIndex <= 0 && idTextBox.Text == string.Empty)
            {
                enableDisableItemSplitter(true);
            }
            else
            {
                enableDisableItemSplitter(false);
                cue = null;
            }


            if (string.IsNullOrEmpty(cue))
                SetComboBoxCue(multipleItemsSplitterComboBoxCustom, DontUseSplitter, true);
            else
                SetComboBoxCue(multipleItemsSplitterComboBoxCustom, cue, true);
        }

        private string multipleItemsSplitterComboBoxValueChanged()
        {
            if (multipleItemsSplitterComboBoxCustom.SelectedItem == null)
                return null;


            string text = multipleItemsSplitterComboBoxCustom.GetItemText(multipleItemsSplitterComboBoxCustom.SelectedItem);

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
            string text = multipleItemsSplitterComboBoxValueChanged();

            if (text == null)
                return;


            SetMultipleItemsSplitterComboBoxCue(text);

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

        private void presetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (presetListLastSelectedIndex == presetList.SelectedIndex)
                return;

            if (previewIsGenerated)
                clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, buttonPreview, buttonOK, buttonClose);

            presetIsLoading = true;
            presetListSelectedIndexChanged(presetList.SelectedIndex);
            presetListLastSelectedIndex = presetList.SelectedIndex;
            presetIsLoading = false;
        }

        private void presetListSelectedIndexChanged(int index)
        {
            functionsDict.Clear(); //As soon as possible because availability of some UI controls depends on it

            if (selectedPreset != null && selectedPreset.useAnotherPresetAsSource && selectedPreset.anotherPresetAsSource.permanentGuid == Guid.Empty)
            {
                selectedPreset.useAnotherPresetAsSource = false;
                checkAdjustFilteringPreset();
            }


            resetLocalsAndUiControls();

            if (index == -1)
            {
                setColumnSaved();

                SetComboBoxCue(multipleItemsSplitterComboBoxCustom, DontUseSplitter);

                selectedPreset = null;

                formatComboBoxCustom.SelectedIndex = 1;

                presetNameTextBox.Enable(false);

                useAnotherPresetAsSourceCheckBox.Checked = false;
                useAnotherPresetAsSourceComboBoxCustom.ItemsClear();
                useAnotherPresetAsSourceCheckBox_CheckedChanged(null, null);

                multipleItemsSplitterTrimCheckBox.Checked = false;

                conditionCheckBox.Checked = false;
                conditionCheckBox_CheckedChanged(null, null);

                conditionFieldListCustom.Text = string.Empty;
                conditionListCustom.Text = string.Empty;
                comparedFieldListCustom.Text = string.Empty;
                sourceFieldComboBoxCustom.Text = string.Empty;
                destinationTagListCustom.Text = string.Empty;
                functionComboBoxCustom.Text = string.Empty;
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

                previewTable.ColumnCount = 0;
                tagsDataGridView.RowCount = 0;
                tagsDataGridViewSelectedRow = -1;
                expressionsDataGridView.RowCount = 0;

                expressionBackup = null;
                splitterBackup = null;
                trimValuesBackup = false;

                enableDisablePreviewOptionControls(true);
                disableQueryingOrUpdatingButtons();

                UpdateCustomScrollBars(presetList);
                UpdateCustomScrollBars(previewTable);
                UpdateCustomScrollBars(tagsDataGridView);
                UpdateCustomScrollBars(expressionsDataGridView);

                return;
            }


            this.Enable(false, autoApplyPresetsLabel);


            selectedPreset = presetList.SelectedItem as ReportPreset;

            enableDisablePreviewOptionControls(false);

            if (selectedPreset.fileFormatIndex == 0)
                formatComboBoxCustom.SelectedIndex = 1;
            else
                formatComboBoxCustom.SelectedIndex = (int)selectedPreset.fileFormatIndex - 1;


            presetNameTextBox.Enable(true);
            presetNameTextBox.ReadOnly = !selectedPreset.userPreset;


            previewTable.DisableColumnsAutoSize(null);

            presetNameTextBox.Text = selectedPreset.name ?? string.Empty;

            FillListByTagNames(destinationTagListCustom.Items);


            //Groupings
            var dict = new ColumnAttributesDict();
            int columnCount = prepareDict(dict, null, selectedPreset.groupings, 0);
            if (columnCount == -1)
            {
                enableDisablePreviewOptionControls(true);
                enableQueryingOrUpdatingButtons();

                this.Enable(true, autoApplyPresetsLabel);

                return;
            }

            foreach (var attribs in dict.Values)
            {
                addColumn(attribs.parameterName, null, LrFunctionType.Grouping,
                    attribs.splitter, attribs.trimValues, attribs.expression);
            }


            //Functions
            dict.Clear();
            prepareDict(dict, null, selectedPreset.functions, columnCount);
            foreach (var attribs in dict.Values)
            {
                addColumn(attribs.parameterName, attribs.parameter2Name, attribs.functionType,
                    null, false, attribs.expression);
            }


            functionComboBoxCustom.SelectedIndex = 0;
            functionComboBox_SelectedIndexChanged(null, null);
            sourceTagListCustom.SelectedIndex = 0;
            sourceTagList_SelectedIndexChanged(null, null);
            parameter2ComboBoxCustom.SelectedIndex = 0;
            parameter2ComboBox_SelectedIndexChanged(null, null);
            multipleItemsSplitterTrimCheckBox.Checked = false;
            multipleItemsSplitterTrimCheckBox_CheckedChanged(null, null);

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


            operations.Clear();
            operations.AddRange(selectedPreset.operations);
            mulDivFactors.Clear();
            mulDivFactors.AddRange(selectedPreset.mulDivFactors);
            precisionDigits.Clear();
            precisionDigits.AddRange(selectedPreset.precisionDigits);
            appendTexts.Clear();
            appendTexts.AddRange(selectedPreset.appendTexts);


            useAnotherPresetAsSourceCheckBox.Checked = selectedPreset.useAnotherPresetAsSource;
            findFilteringPresetsUI(useAnotherPresetAsSourceComboBoxCustom, selectedPreset);
            useAnotherPresetAsSourceCheckBox_CheckedChanged(null, null);


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

            if (tagsDataGridView.RowCount > 0)
            {
                selectRow(tagsDataGridView, 0);
            }
            else
            {
                expressionBackup = string.Empty;
                splitterBackup = string.Empty;
                trimValuesBackup = false;
            }

            setColumnSaved();

            enableDisablePreviewOptionControls(true);
            enableQueryingOrUpdatingButtons();


            this.Enable(true, autoApplyPresetsLabel);


            UpdateCustomScrollBars(presetList);
            UpdateCustomScrollBars(previewTable);
            UpdateCustomScrollBars(tagsDataGridView);
            UpdateCustomScrollBars(expressionsDataGridView);
        }

        private void conditionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            conditionFieldListCustom.Enable(conditionCheckBox.IsEnabled() && conditionCheckBox.Checked);
            conditionListCustom.Enable(conditionCheckBox.IsEnabled() && conditionCheckBox.Checked && conditionFieldListCustom.SelectedIndex != -1);
            comparedFieldListCustom.Enable(conditionCheckBox.IsEnabled() && conditionCheckBox.Checked && conditionFieldListCustom.SelectedIndex != -1);


            if (presetIsLoading)
                return;

            updatePreset();

            //(selectedPreset, null,  FALSE, FALSE): See checkAdjustAllPresetChainsUI() below, which will update status
            //Only to get presetCheckStatusIsSenseless and presetCheckStatusIsBroken used below
            (bool presetCheckStatusIsSenseless, bool presetCheckStatusIsBroken) = checkRelatedPresetsChain(selectedPreset, selectedPreset, false, false);


            if (useAnotherPresetAsSourceComboBoxCustom.SelectedItem == null)
                adjustFilteringPresetUI(useAnotherPresetAsSourceComboBoxCustom, default, presetCheckStatusIsSenseless, presetCheckStatusIsBroken);
            else
                adjustFilteringPresetUI(useAnotherPresetAsSourceComboBoxCustom, (ReportPresetReference)useAnotherPresetAsSourceComboBoxCustom.SelectedItem, presetCheckStatusIsSenseless, presetCheckStatusIsBroken);


            checkAdjustAllPresetChainsUI();
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

        void enableDisableItemSplitter(bool status)
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
            SetMultipleItemsSplitterComboBoxCue(splitterBackup);

            if (sourceFieldComboBoxCustom.SelectedIndex >= 0)
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
                    + NowTickedText.ToUpper().Replace("%%TICKEDPRESETS%%", autoAppliedPresetCount.ToString());
                }
                else
                {
                    autoApplyPresetsLabel.Text = AutoApplyText + "\n"
                    + AutoApplyDisabledText.ToUpper() + "\n"
                    + NowTickedText.ToUpper().Replace("%%TICKEDPRESETS%%", autoAppliedPresetCount.ToString());
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
            if (e.Button == MouseButtons.Left)
            {
                if (e.ColumnIndex == artworkField)
                    return;

                DataGridViewCellComparer comparator = new DataGridViewCellComparer
                {
                    resultTypes = columnTypes,
                    comparedColumnIndex = e.ColumnIndex
                };

                for (int i = 0; i < previewTable.ColumnCount; i++)
                    previewTable.Columns[i].HeaderCell.SortGlyphDirection = SortOrder.None;

                previewTable.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Ascending;

                SetStatusbarText(LibraryReportsSbText + " (" + SbSorting + ")", false);
                previewTable.Sort(comparator);
                SetStatusbarText(string.Empty, false);
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

        private void numberOfTagsToRecalculateNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            setUnsavedChanges(true);
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

            if (!uint.TryParse(textValue, out uint value))
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
            else switch (mulDivFactorComboBoxCustom.Text)
                {
                    case "":
                        mulDivFactorComboBoxCustom.SelectedIndex = 0;
                        mulDivFactors[sourceFieldComboBoxCustom.SelectedIndex] = (string)mulDivFactorComboBoxCustom.Items[0];

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
                        string mulDivFactorText = TryParseUint(mulDivFactorComboBoxCustom.Text, 1);
                        if (mulDivFactorComboBoxCustom.Text != mulDivFactorText)
                        {
                            mulDivFactorComboBoxCustom.Text = mulDivFactorText;
                            mulDivFactorComboBoxCustom.SelectionStart = mulDivFactorText.Length;
                            mulDivFactorComboBoxCustom.SelectionLength = 0;
                        }

                        mulDivFactors[sourceFieldComboBoxCustom.SelectedIndex] = mulDivFactorComboBoxCustom.Text;

                        break;
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
            else switch (precisionDigitsComboBoxCustom.Text)
                {
                    case "":
                        precisionDigitsComboBoxCustom.SelectedIndex = 0;
                        precisionDigits[sourceFieldComboBoxCustom.SelectedIndex] = (string)precisionDigitsComboBoxCustom.Items[0];

                        break;
                    case "(don't round)":
                    case "(не округлять)":
                        precisionDigits[sourceFieldComboBoxCustom.SelectedIndex] = (string)precisionDigitsComboBoxCustom.Items[0];

                        break;
                    default:
                        string precisionDigitsText = TryParseUint(precisionDigitsComboBoxCustom.Text, 0);
                        if (precisionDigitsComboBoxCustom.Text != precisionDigitsText)
                        {
                            precisionDigitsComboBoxCustom.Text = precisionDigitsText;
                            precisionDigitsComboBoxCustom.SelectionStart = precisionDigitsText.Length;
                            precisionDigitsComboBoxCustom.SelectionLength = 0;
                        }

                        precisionDigits[sourceFieldComboBoxCustom.SelectedIndex] = precisionDigitsComboBoxCustom.Text;

                        break;
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
            if (presetIsLoading)
                return;

            updatePreset();

            checkAdjustFilteringPreset();
            checkAdjustAllPresetChainsUI();
        }

        private void useAnotherPresetAsSourceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            useAnotherPresetAsSourceComboBoxCustom.Enable(useAnotherPresetAsSourceCheckBox.Checked);

            if (useAnotherPresetAsSourceCheckBox.Checked && useAnotherPresetAsSourceComboBoxCustom.SelectedIndex == -1)
            {
                if (useAnotherPresetAsSourceComboBoxCustom.Items.Count > 0)
                    useAnotherPresetAsSourceComboBoxCustom.SelectedIndex = 0;
            }
            else if (!useAnotherPresetAsSourceCheckBox.Checked)
            {
                useAnotherPresetAsSourceComboBoxCustom.SelectedIndex = -1;
            }

            if (presetIsLoading)
                return;

            updatePreset();

            checkAdjustFilteringPreset();
            checkAdjustAllPresetChainsUI();
        }

        private void useAnotherPresetAsSourceCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!useAnotherPresetAsSourceCheckBox.IsEnabled())
                return;

            useAnotherPresetAsSourceCheckBox.Checked = !useAnotherPresetAsSourceCheckBox.Checked;
        }

        private void previewTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && ModifierKeys == Keys.Control)
            {
                comparedFieldListCustom.Text = previewTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                comparedFieldList_TextChanged(null, null);
            }
        }

        private void previewTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex != artworkField && ModifierKeys == Keys.Control)
            {
                conditionFieldListCustom.SelectedIndex = e.ColumnIndex;
                comparedFieldListCustom.Text = previewTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
        }

        private void previewTable_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (e.Column.Index == artworkField)
            {
                for (int i = 0; i < previewTable.RowCount; i++)
                    previewTable.Rows[i].MinimumHeight = e.Column.Width;

                previewTable.AutoResizeRows();
            }
        }

        private void previewTable_RowHeightChanged(object sender, DataGridViewRowEventArgs e)
        {
            if (artworkField == -1 && ModifierKeys == Keys.Shift)
            {
                previewTable.AutoResizeRows();
            }
            else if (artworkField == -1 && ModifierKeys == Keys.Alt)
            {
                for (int i = 0; i < previewTable.RowCount; i++)
                    if (i != e.Row.Index && previewTable.Rows[i].Height < e.Row.Height)
                        previewTable.Rows[i].Height = e.Row.Height;
            }
        }

        private void smartOperationCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!smartOperationCheckBox.IsEnabled())
                return;

            smartOperationCheckBox.Checked = !smartOperationCheckBox.Checked;
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            QuickSettings settings = new QuickSettings(TagToolsPlugin);
            Display(settings, true);
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
    }

    internal abstract class ExportedDocument
    {
        protected System.IO.FileStream stream;
        protected Plugin plugin;
        protected string header;
        protected Encoding unicode = Encoding.UTF8;
        protected string text = string.Empty;
        protected byte[] buffer;

        protected virtual void getHeader()
        {
            text = string.Empty;
        }

        protected virtual void getFooter()
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

        internal string getImageName(string hash)
        {
            hash = Regex.Replace(hash, @"\\", "_");
            hash = Regex.Replace(hash, @"/", "_");
            hash = Regex.Replace(hash, @"\:", "_");

            return hash;
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
        internal abstract void addCellToRow(Bitmap cell, string cellName, string imageHash, int width, int height);

        protected abstract void getRow(int height);

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
            return;
        }

        internal virtual void beginAlbum(string album, Bitmap artwork, string imageHash, int albumTrackCount)
        {
            return;
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
            if (text != string.Empty)
            {
                text += "\t" + cell;
            }
            else
            {
                text += cell;
            }
        }

        internal override void addCellToRow(Bitmap cell, string cellName, string imageHash, int width, int height)
        {
            if (text != string.Empty)
            {
                text += "\tARTWORK";
            }
            else
            {
                text += "ARTWORK";
            }
        }

        protected override void getRow(int height)
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

        private string delimiter = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;

        internal override void addCellToRow(string cell, string cellName, bool rightAlign, bool dontOutput1, bool dontOutput2)
        {
            if (text != string.Empty)
            {
                text += (delimiter + "\"" + cell.Replace("\"", "\"\"") + "\"");
            }
            else
            {
                text += "\"" + cell.Replace("\"", "\"\"") + "\"";
            }
        }

        internal override void addCellToRow(Bitmap cell, string cellName, string imageHash, int width, int height)
        {
            if (text != string.Empty)
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
        protected string fileDirectoryPath;
        protected string imagesDirectoryName;
        protected const string defaultImageName = "Missing Artwork.png";
        protected bool defaultImageWasExported = false;

        internal HtmlTable(System.IO.FileStream newStream, Plugin plugin, string presetName, string fileDirectoryPath, string imagesDirectoryName)
            : base(newStream, plugin, presetName)
        {
            this.fileDirectoryPath = fileDirectoryPath;
            this.imagesDirectoryName = imagesDirectoryName;
        }

        protected override void getHeader()
        {
            text = "<html><head></head><header>" + header + "</header><body><table border=1>";
        }

        protected override void getFooter()
        {
            text = "</table></body></html>";
        }

        internal override void addCellToRow(string cell, string cellName, bool rightAlign, bool dontOutput1, bool dontOutput2)
        {
            text += "<td" + (rightAlign ? " align='right'" : string.Empty) + ">" + cell + "</td>";
        }

        internal override void addCellToRow(Bitmap cell, string cellName, string imageHash, int width, int height)
        {
            string imageName = getImageName(imageHash) + ".jpg";
            cell.Save(fileDirectoryPath + imagesDirectoryName + @"\" + imageName, System.Drawing.Imaging.ImageFormat.Jpeg);

            text += "<td height=" + height + " width=" + width + "> <img src=\"" + imagesDirectoryName + @"\" + imageName + "\" > </td>";
        }

        protected override void getRow(int height)
        {
            text = "<tr>" + text + " </tr>";
        }
    }

    internal class HtmlDocumentAlbumGrid : HtmlTable
    {
        protected bool? alternateRow = null;

        internal HtmlDocumentAlbumGrid(System.IO.FileStream newStream, Plugin plugin, string presetName, string fileDirectoryPath, string imagesDirectoryName)
            : base(newStream, plugin, presetName, fileDirectoryPath, imagesDirectoryName)
        {
        }

        internal void writeHeader(string color, int rowImageCount, int imageWidth, int imageHeight, int labelHeight, int fontSize)
        {
            System.IO.FileStream stylesheet = new System.IO.FileStream(fileDirectoryPath + imagesDirectoryName + @"\stylesheet.css", System.IO.FileMode.Create);

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

        protected override void getHeader()
        {
            text = "<html><head><link rel=Stylesheet href=\"" + imagesDirectoryName + "\\stylesheet.css\">"
            + " </head><body><table class='header'><tr><td>" + header + "</td></tr><tr><td><table>";
        }

        protected override void getFooter()
        {
            text = "</table></td></tr></table></body></html>";
        }

        internal void addCellToRow(Bitmap cell, string cellName, string imageHash)
        {
            string imageName = getImageName(imageHash) + ".jpg";
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
        protected bool? alternateRow = null;

        internal HtmlDocument(System.IO.FileStream newStream, Plugin plugin, string presetName, string fileDirectoryPath, string imagesDirectoryName)
            : base(newStream, plugin, presetName, fileDirectoryPath, imagesDirectoryName)
        {
        }

        internal override void writeHeader()
        {
            System.IO.FileStream stylesheet = new System.IO.FileStream(fileDirectoryPath + imagesDirectoryName + @"\stylesheet.css", System.IO.FileMode.Create);

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

        protected override void getHeader()
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

        internal override void addCellToRow(Bitmap cell, string cellName, string imageHash, int width, int height)
        {
            string rowClass;

            if (alternateRow == null)
                rowClass = "xl0";
            else if (alternateRow == true)
                rowClass = "xl2";
            else
                rowClass = "xl3";

            string imageName = getImageName(imageHash) + ".jpg";
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

        internal override void beginAlbum(string album, Bitmap artwork, string imageHash, int albumTrackCount)
        {
            string imageName = getImageName(imageHash) + ".jpg";
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

        internal override void addCellToRow(Bitmap cell, string cellName, string imageHash, int width, int height)
        {
        }
    }

    internal class HtmlDocumentCDBooklet : HtmlTable
    {
        protected int backgroundSize = 0;

        protected float trackFontSize;
        protected float albumArtistsFontSize;
        protected float albumFontSize;

        protected string trackTextColor;
        protected string albumArtistTextColor;
        protected string albumArtistTextOutlineColor;
        protected string albumTextColor;
        protected string albumTextOutlineColor;
        protected string backgroundColor;

        internal HtmlDocumentCDBooklet(System.IO.FileStream newStream, Plugin plugin, string presetName, string fileDirectoryPath, string imagesDirectoryName)
            : base(newStream, plugin, presetName, fileDirectoryPath, imagesDirectoryName)
        {
        }

        internal void writeHeader(int size, Color bitmapAverageColor, Bitmap scaledPic, SortedDictionary<string, List<string>> albumArtistsAlbums, int trackCount)
        {
            backgroundSize = size;

            trackFontSize = 12.0f;
            albumArtistsFontSize = 50.0f;
            albumFontSize = 30.0f;

            int albumCount = 0;
            foreach (KeyValuePair<string, List<string>> albumArtist in albumArtistsAlbums)
            {
                albumCount += albumArtist.Value.Count;
            }

            if (trackCount > 20)
                trackFontSize = 9.0f;
            else if (trackCount > 15)
                trackFontSize = 10.0f;
            else if (trackCount > 10)
                trackFontSize = 11.0f;

            string trackFontSizeString = trackFontSize.ToString().Replace(',', '.');

            if (albumArtistsAlbums.Count > 5)
                albumArtistsFontSize /= albumArtistsAlbums.Count - 4;

            string albumArtistsFontSizeString = albumArtistsFontSize.ToString().Replace(',', '.');

            if (albumCount > 5)
                albumFontSize /= albumCount - 4;

            string albumFontSizeString = albumFontSize.ToString().Replace(',', '.');

            if (bitmapAverageColor.A + bitmapAverageColor.R + bitmapAverageColor.G < 127 * 3)
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

            System.IO.FileStream stylesheet = new System.IO.FileStream(fileDirectoryPath + imagesDirectoryName + @"\stylesheet.css", System.IO.FileMode.Create);

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

        protected override void getHeader()
        {
            text = "<html><head>"
                + "<link rel=Stylesheet href='" + imagesDirectoryName + "/stylesheet.css'>"
                + "</head>"
                + "<body><table><tr>"
                + "<td class=xl0 width=" + backgroundSize + " height=" + backgroundSize + ">"
                + "<table>";
        }

        protected void getFooter(SortedDictionary<string, List<string>> albumArtistsAlbums)
        {
            text = "</table></td><td width=" + backgroundSize + " height=" + backgroundSize
                + " background='" + imagesDirectoryName + "/bg1.jpg' style='background-repeat:no-repeat;background-position:center center;'><table>";

            foreach (KeyValuePair<string, List<string>> albumArtist in albumArtistsAlbums)
            {
                text += "<tr><td class=xl3 width=" + backgroundSize + ">" + albumArtist.Key + "</td></tr>";
                foreach (string album in albumArtist.Value)
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

        internal void addTrack(int seqNum, string albumArtist, string album, string title, string duration)
        {
            text = "<td class=xl1>" + seqNum.ToString("D2") + ".";

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
