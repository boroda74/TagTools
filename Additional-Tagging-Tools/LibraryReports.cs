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
using System.Windows.Forms;
using static ExtensionMethods.NativeMethods;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class LibraryReportsCommand : PluginWindowTemplate
    {
        protected bool forceCloseForms = true;

        private static string DontUseSplitter;

        private delegate void AddRowsToTable(List<string[]> rows);
        private delegate void UpdateTable();
        private AddRowsToTable addRowsToTable;
        private UpdateTable updateTable;

        private System.Threading.Timer periodicCacheClearingTimer;

        private void periodicCacheClearing(object state)
        {
            cachedAppliedPresetGuid = Guid.NewGuid();
            cachedQueriedActualGroupingValues = null;
            queriedFilesActualComposedGroupingValues.Clear();
            tags.Clear();
            fileTags.Clear();
            clearArtworks();
        }

        private static List<string> ProcessedReportDeletions = new List<string>();

        //Cached UI and events workarounds
        public static Bitmap DefaultArtwork;
        public static string DefaultArtworkHash;
        public SortedDictionary<string, Bitmap> artworks = new SortedDictionary<string, Bitmap>();
        private Bitmap artwork; //For CD booklet export

        private static Font TotalsFont; // Preview table font for "Totals" cells
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

        private static string HelpMsg;

        private static string TagsDataGridViewCheckBoxToolTip;
        private static string ExpressionsDataGridViewCheckBoxToolTip;

        private static string DoYouWantToDeleteTheFieldMsg;
        private static string DoYouWantToReplaceTheFieldMsg;

        private static string NoExpressionText;

        private bool ignoreCheckedPresetEvent = true;
        private bool ignorePresetChangedEvent = false;
        private int autoAppliedPresetCount;

        private string assignHotkeyCheckBoxText;
        private int reportPresetsWithHotkeysCount;
        private bool[] reportPresetHotkeyUsedSlots = new bool[MaximumNumberOfLRHotkeys];

        private bool unsavedChanges = false;
        private string buttonCloseToolTip;
        private int presetListLastSelectedIndex = -2;

        private bool ignoreSplitterMovedEvent = true;

        private ReportPreset selectedPreset = null;

        private bool presetIsLoading = false;
        private bool sourceFieldComboBoxIndexChanging = false;

        private static bool BackgroundTaskIsInProgress = false;

        private bool resultsAreFiltered = false;
        private bool? lastSelectedRefCheckStatus;

        //Working locals
        public ReportPreset appliedPreset;
        private Guid cachedAppliedPresetGuid;
        private ReportPreset[] reportPresets = null;


        private SortedDictionary<string, bool>[] cachedQueriedActualGroupingValues = null;
        private SortedDictionary<string, List<string>> queriedFilesActualComposedGroupingValues = new SortedDictionary<string, List<string>>(); //<url, list of composed groupings>

        private bool[] columnsRightAlignment; // Cache for preview table/exported report 
        private ResultType[] columnTypes; // Cache for preview table/exported report 

        private AggregatedTags tags = new AggregatedTags();
        private AggregatedTags fileTags = new AggregatedTags();

        private string[] lastFiles = null;

        private Comparison comparison;
        private string comparedFieldText;

        private int conditionField = -1;
        private int comparedField = -1;

        private int artworkField = -1;
        private int sequenceNumberField = -1;

        private readonly List<MetaDataType> destinationTagIds = new List<MetaDataType>();

        public static string PresetName = string.Empty;

        private string expressionBackup = string.Empty;
        private string splitterBackup = string.Empty;
        private bool trimValuesBackup = false;

        private bool? newColumn = false;
        private int tagsDataGridViewSelectedRow = -1;

        private SortedDictionary<string, PresetColumnAttributes> groupings = new SortedDictionary<string, PresetColumnAttributes>(); // Short ids, attributes (various expressions)


        //Working locals & UI preset caching
        private List<string> sortedShortIds = new List<string>(); // Short Ids
        private Dictionary<string, List<string>> shortIdsExprs = new Dictionary<string, List<string>>(); // Short ids, expressions

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


        public LibraryReportsCommand(bool setTimer)
        {
            //It's not GUI control, not a Form in this case
            //InitializeComponent();

            periodicCacheClearingTimer = new System.Threading.Timer(periodicCacheClearing, null, 5 * 60 * 1000, 5 * 60 * 1000); //Every 5 mins.
        }

        public LibraryReportsCommand(Plugin tagToolsPluginParam) : base(tagToolsPluginParam)
        {
            InitializeComponent();
        }

        protected override void initializeForm()
        {
            base.initializeForm();

            //Setting control not standard properties
            //var heightField = typeof(CheckedListBox).GetField(
            //    "scaledListItemBordersHeight",
            //    BindingFlags.NonPublic | BindingFlags.Instance
            //);

            //var addedHeight = 4; // Some appropriate value, greater than the field's default of 2
            //heightField.SetValue(presetList, addedHeight); // Where "presetList" is your CheckedListBox

            previewTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;


            if (DontUseSplitter == null)
                DontUseSplitter = (multipleItemsSplitterComboBox.Items[0] as string).TrimStart(' ');

            //Setting themed images
            openReportCheckBoxPicture.Image = ThemedBitmapAddRef(this, Window);

            clearIdButton.Image = ThemedBitmapAddRef(this, ButtonRemoveImage);
            buttonSettings.Image = ThemedBitmapAddRef(this, Gear);

            buttonClearExpression.Image = ThemedBitmapAddRef(this, ButtonRemoveImage);


            //Clearing "unsaved changes" button image
            buttonClose.Image = Resources.transparent_15;
            buttonCloseToolTip = toolTip1.GetToolTip(buttonClose);
            toolTip1.SetToolTip(buttonClose, string.Empty);


            //Preparing "ticked presets" label text
            string entireText = autoApplyPresetsLabel.Text;
            AutoApplyText = Regex.Replace(entireText, @"^(.*?)~.*?~.*", "$1");
            NowTickedText = Regex.Replace(entireText, @"^.*?~(.*?)~.*", "$1");
            AutoApplyDisabledText = Regex.Replace(entireText, @"^.*?~.*?~(.*)", "$1");//---

            if (SavedSettings.allowAsrLrPresetAutoexecution)
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
            ButtonFilterResultsText = buttonFilterResults.Text;
            ButtonFilterResultsToolTip = toolTip1.GetToolTip(buttonFilterResults);


            //Saving useAnotherPresetAsSourceCheckBox tool tip
            string useAnotherPresetAsSourceFullToolTip = toolTip1.GetToolTip(useAnotherPresetAsSourceCheckBox);
            UseAnotherPresetAsSourceToolTip = Regex.Replace(useAnotherPresetAsSourceFullToolTip, @"^((?:[^:]|\r|\n)*)\:\r\n((?:[^:]|\r|\n)*)\:\r\n((?:[^:]|\r|\n)*)", "$1");
            UseAnotherPresetAsSourceIsSenselessToolTip = Regex.Replace(useAnotherPresetAsSourceFullToolTip, @"^((?:[^:]|\r|\n)*)\:\r\n((?:[^:]|\r|\n)*)\:\r\n((?:[^:]|\r|\n)*)", "$2");
            UseAnotherPresetAsSourceIsInBrokenChainToolTip = Regex.Replace(useAnotherPresetAsSourceFullToolTip, @"^((?:[^:]|\r|\n)*)\:\r\n((?:[^:]|\r|\n)*)\:\r\n((?:[^:]|\r|\n)*)", "$3");


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
            HelpMsg = Regex.Replace(buttonHelpInitialToolTip, @"^(.*?)\r\n\r\n((.|\r|\n)*)", "$2");
            toolTip1.SetToolTip(buttonHelp, buttonHelpToolTip);


            // Saving field & expression tables tooltips for check boxes
            TagsDataGridViewCheckBoxToolTip = tagsDataGridView.Columns[0].HeaderCell.ToolTipText;
            tagsDataGridView.Columns[0].HeaderCell.ToolTipText = null;

            ExpressionsDataGridViewCheckBoxToolTip = expressionsDataGridView.Columns[0].HeaderCell.ToolTipText;
            expressionsDataGridView.Columns[0].HeaderCell.ToolTipText = null;


            // Saving text for message box shown on last expression removing
            DoYouWantToDeleteTheFieldMsg = expressionsDataGridView.Columns[1].HeaderCell.ToolTipText;
            expressionsDataGridView.Columns[1].HeaderCell.ToolTipText = null;


            // Saving text for message box shown on field replacing
            DoYouWantToReplaceTheFieldMsg = tagsDataGridView.Columns[1].HeaderCell.ToolTipText;
            tagsDataGridView.Columns[1].HeaderCell.ToolTipText = null;


            //Initialization
            string[] formats = ExportedFormats.Split('|');
            for (int i = 0; i < formats.Length; i += 2)
                formatComboBox.Items.Add(formats[i]);

            smartOperationCheckBox.Checked = SavedSettings.smartOperation;

            foreach (var fname in LrFunctionNames)
                functionComboBox.Items.Add(fname);

            FillListByTagNames(sourceTagList.Items, true, true, false);
            FillListByPropNames(sourceTagList.Items);
            sourceTagList.Items.Add(SequenceNumberName);

            FillListByTagNames(parameter2ComboBox.Items, true, false, false);
            FillListByPropNames(parameter2ComboBox.Items);
            parameter2ComboBox.SelectedIndex = 0;

            conditionList.Items.Add(ListItemConditionIs);
            conditionList.Items.Add(ListItemConditionIsNot);
            conditionList.Items.Add(ListItemConditionIsGreater);
            conditionList.Items.Add(ListItemConditionIsGreaterOrEqual);
            conditionList.Items.Add(ListItemConditionIsLess);
            conditionList.Items.Add(ListItemConditionIsLessOrEqual);


            presetNameTextBox.SetCue(CtlAutoLrPresetName);

            NoExpressionText = expressionTextBox.Text;
            expressionTextBox.SetCue(NoExpressionText);


            ignoreCheckedPresetEvent = false;
            autoAppliedPresetCount = 0;
            reportPresetsWithHotkeysCount = 0;
            for (int i = 0; i < SavedSettings.reportsPresets.Length; i++)
            {
                ReportPreset autoLibraryReportsPreset = new ReportPreset(SavedSettings.reportsPresets[i], true);

                presetList.Items.Add(autoLibraryReportsPreset, autoLibraryReportsPreset.autoApply);

                if (autoLibraryReportsPreset.hotkeyAssigned)
                {
                    reportPresetsWithHotkeysCount++;
                    reportPresetHotkeyUsedSlots[autoLibraryReportsPreset.hotkeySlot] = true;
                }
            }
            ignoreCheckedPresetEvent = true;


            assignHotkeyCheckBoxText = Regex.Replace(assignHotkeyCheckBoxLabel.Text, "^(.*:\\s).*", "$1");
            assignHotkeyCheckBoxLabel.Text = assignHotkeyCheckBoxText + (MaximumNumberOfLRHotkeys - reportPresetsWithHotkeysCount) + "/" + MaximumNumberOfLRHotkeys;


            FillListByTagNames(destinationTagList.Items);


            //"Totals" font
            float tagNameFontSize = previewTable.DefaultCellStyle.Font.Size * 0.7f; //Maybe it's worth to adjust fine size !!!
            TotalsFont = new Font(Font.FontFamily, tagNameFontSize, FontStyle.Bold);


            previewTable.EnableHeadersVisualStyles = !UseMusicBeeFontSkinColors;
            tagsDataGridView.EnableHeadersVisualStyles = !UseMusicBeeFontSkinColors;
            expressionsDataGridView.EnableHeadersVisualStyles = !UseMusicBeeFontSkinColors;

            previewTable.BackgroundColor = UnchangedCellStyle.BackColor;
            previewTable.DefaultCellStyle = UnchangedCellStyle;

            tagsDataGridView.BackgroundColor = UnchangedCellStyle.BackColor;
            tagsDataGridView.DefaultCellStyle = UnchangedCellStyle;
            tagsDataGridView.Columns[0].HeaderCell.Style = HeaderCellStyle;
            tagsDataGridView.Columns[1].HeaderCell.Style = HeaderCellStyle;
            tagsDataGridView.Columns[2].HeaderCell.Style = HeaderCellStyle;
            tagsDataGridView.Columns[3].HeaderCell.Style = HeaderCellStyle;

            expressionsDataGridView.BackgroundColor = UnchangedCellStyle.BackColor;
            expressionsDataGridView.DefaultCellStyle = UnchangedCellStyle;
            expressionsDataGridView.Columns[0].HeaderCell.Style = HeaderCellStyle;
            expressionsDataGridView.Columns[1].HeaderCell.Style = HeaderCellStyle;

            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            if (Language == "ru")
                tagsDataGridView.Columns[1].Width = 130;

            tagsDataGridView.Columns[1].Width = (int)Math.Round(tagsDataGridView.Columns[1].Width * hDpiFontScaling);

            presetList.SelectedIndex = -1;
            presetList_SelectedIndexChanged(null, null);


            resizeArtworkCheckBox.Checked = SavedSettings.resizeArtwork;
            xArtworkSizeUpDown.Value = SavedSettings.xArtworkSize == 0 ? 300 : SavedSettings.xArtworkSize;
            yArtworkSizeUpDown.Value = SavedSettings.yArtworkSize == 0 ? 300 : SavedSettings.yArtworkSize;

            openReportCheckBox.Checked = SavedSettings.openReportAfterExporting;

            recalculateOnNumberOfTagsChangesCheckBox.Checked = SavedSettings.recalculateOnNumberOfTagsChanges;
            numberOfTagsToRecalculateNumericUpDown.Value = SavedSettings.numberOfTagsToRecalculate;


            addRowsToTable = previewTable_AddRowsToTable;
            updateTable = previewTable_UpdateTable;


            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        public class ColumnAttributesBase
        {
            public LrFunctionType functionType;
            public string parameterName;
            public DataType dataType = DataType.String;
            public ResultType resultType = ResultType.UseOtherResults;
            public string parameter2Name = null;
            public string splitter = null;
            public bool trimValues = false;

            public ColumnAttributesBase()
            {
                //Nothing to do...
            }

            public ColumnAttributesBase(LrFunctionType typeParam, string parameterNameParam, string parameter2NameParam,
                string splitterParam, bool trimValuesParam)
            {
                functionType = typeParam;
                parameterName = parameterNameParam;
                parameter2Name = parameter2NameParam;
                splitter = splitterParam;
                trimValues = trimValuesParam;
            }

            public ColumnAttributesBase(ColumnAttributesBase sourceAttribs)
            {
                functionType = sourceAttribs.functionType;
                parameterName = sourceAttribs.parameterName;
                parameter2Name = sourceAttribs.parameter2Name;
                splitter = sourceAttribs.splitter;
                trimValues = sourceAttribs.trimValues;
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
                ColumnAttributesBase colAttrs = obj as ColumnAttributesBase;

                if (colAttrs == null)
                    return false;

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
        }

        public class ColumnAttributes : ColumnAttributesBase
        {
            public string expression = string.Empty;

            public ColumnAttributes() : base()
            {
                //Nothing to do...
            }

            public ColumnAttributes(LrFunctionType typeParam, string expressionParam, string parameterNameParam, string parameter2NameParam,
                string splitterParam, bool trimValuesParam)
                : base(typeParam, parameterNameParam, parameter2NameParam,
                    splitterParam, trimValuesParam)
            {
                expression = expressionParam;
            }

            public ColumnAttributes(ColumnAttributes sourceAttribs) : base(sourceAttribs)
            {
                expression = sourceAttribs.expression;
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


                ColumnAttributes colAttrs = obj as ColumnAttributes;

                if (colAttrs == null)
                    return false;
                else if (expression != colAttrs.expression)
                    return false;

                return true;
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
            public int[] columnIndices = new int[0]; // per expression for current grouping/function

            public PresetColumnAttributes() : base()
            {
                //Nothing to do...
            }

            public PresetColumnAttributes(LrFunctionType typeParam, string[] expressionsParam, string parameterNameParam, string parameter2NameParam,
                string splitterParam, bool trimValuesParam)
                : base(typeParam, parameterNameParam, parameter2NameParam,
                    splitterParam, trimValuesParam)
            {
                expressions = expressionsParam;
            }

            public PresetColumnAttributes(ColumnAttributesBase sourceAttribs) : base(sourceAttribs)
            {
                // Nothing to do...
            }

            public PresetColumnAttributes(PresetColumnAttributes sourceAttribs) : base(sourceAttribs)
            {
                expressions = (string[])sourceAttribs.expressions.Clone();
                columnIndices = (int[])sourceAttribs.columnIndices.Clone();
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

                return ((int)functionType).ToString("D2") + "\u0007" + ((int)GetTagId(parameterName)).ToString("D4")
                    + "\u0007" + ((int)GetTagId(parameter2Name)).ToString("D4") + "\u0007" + expression;
            }

            public override bool Equals(object obj)
            {
                if (!base.Equals(obj))
                    return false;


                ColumnAttributes colAttrs = obj as ColumnAttributes;

                if (colAttrs == null)
                {
                    return false;
                }
                else
                {
                    foreach (var expression in expressions)
                        if (expression != colAttrs.expression)
                            return false;
                }

                return true;
            }

            // returns {columnIndex, resulting tag value}, accepts int[] columnIndices: per expression for current grouping/function
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

            // <columnIndex, resulting tag value>, int[] columnIndices: per expression for current grouping/function
            public List<ColumnIndexTagValue> getSplitValues(string url, string tagValue)
            {
                var results = new List<ColumnIndexTagValue>();

                if (string.IsNullOrWhiteSpace(splitter))
                {
                    results.AddRange(evaluateExpressions(url, tagValue));
                }
                else
                {
                    string workingSplitter = splitter.Replace("\\0", "\0");
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

        public class ColumnAttributesDict : Dictionary<string, ColumnAttributes> // Column unique ID (string), Column settings
        {
            public ColumnAttributesDict() : base()
            {
                //Nothing to do...
            }

            public ColumnAttributesDict(ColumnAttributes[] attribCollection) : base()
            {
                foreach (var attribs in attribCollection)
                    Add(attribs.getUniqueId(), attribs);
            }

            // Returns PresetColumnAttributes[], column count
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
                }

                return -1;
            }

            public List<string> idsToSortedList(bool getUniqueIds, List<string> list = null)
            {
                if (list == null)
                    list = new List<string>();

                if (getUniqueIds)
                {
                    foreach (var key in Keys)
                        list.Add(key);
                }
                else
                {
                    foreach (var attribs in Values)
                    {
                        string shortId = attribs.getShortId();
                        if (!list.Contains(shortId))
                            list.Add(shortId);
                    }
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
                        longIdsList.Add(pair.Key);
                }

                return longIdsList;
            }

            public void remove(List<string> longIdsList)
            {
                foreach (var longId in longIdsList)
                    Remove(longId);
            }
        }

        public struct ReportPresetReference
        {
            public string name;
            public Guid permanentGuid;

            //public ReportPresetReference()
            //{
            //    name = null;
            //    permanentGuid = Guid.NewGuid();
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
                    {
                        if (preset.conditionIsChecked)
                            return preset;
                        else
                            return null;
                    }
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
            public bool userPreset = true;
            public Guid guid = Guid.NewGuid(); // guid is reset on every preset update in UI
            public Guid permanentGuid = Guid.NewGuid(); // permanentGuid is reset on preset copying in UI

            public bool hotkeyAssigned = false;
            public bool applyToSelectedTracks = true;
            public int hotkeySlot = -1; //0..Plugin.MaximumNumberOfLRHotkeys - 1

            public PresetColumnAttributes[] groupings = new PresetColumnAttributes[0];

            public string[] groupingNames = new string[0];//**** remove later !!!!

            public PresetColumnAttributes[] functions = new PresetColumnAttributes[0];

            public LrFunctionType[] functionTypes = new LrFunctionType[0];//**** remove later !!!!
            public string[] parameterNames = new string[0];//**** remove later !!!!
            public string[] parameter2Names = new string[0];//**** remove later !!!!

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

            public LrReportFormat fileFormatIndex = LrReportFormat.HtmlDocument;
            public string exportedTrackListName = null;

            public ReportPreset()
            {
                //Nothing to do...
            }

            public ReportPreset(string exportedTrackList)
            {
                exportedTrackListName = exportedTrackList;
            }

            public ReportPreset(ReportPreset sourcePreset, bool fullCopy)
            {
                if (fullCopy)
                {
                    name = sourcePreset.name;
                    autoApply = sourcePreset.autoApply;
                    userPreset = sourcePreset.userPreset;
                    guid = sourcePreset.guid;
                    permanentGuid = sourcePreset.permanentGuid;
                    hotkeyAssigned = sourcePreset.hotkeyAssigned;
                    hotkeySlot = sourcePreset.hotkeySlot; //0..Plugin.MaximumNumberOfLRHotkeys - 1
                    functionIds = (string[])sourcePreset.functionIds.Clone();
                }
                else
                {
                    functionIds = new string[sourcePreset.functionIds.Length];
                }

                autoName = sourcePreset.autoName;

                applyToSelectedTracks = sourcePreset.applyToSelectedTracks;

                groupings = (PresetColumnAttributes[])sourcePreset.groupings.Clone();

                groupingNames = (string[])sourcePreset.groupingNames.Clone();//**** remove later !!!!

                functions = (PresetColumnAttributes[])sourcePreset.functions.Clone();

                functionTypes = (LrFunctionType[])sourcePreset.functionTypes.Clone();//**** remove later !!!!
                parameterNames = (string[])sourcePreset.parameterNames.Clone();//**** remove later !!!!
                parameter2Names = (string[])sourcePreset.parameter2Names.Clone();//**** remove later !!!!

                totals = sourcePreset.totals;

                sourceTags = (string[])sourcePreset.sourceTags.Clone();
                destinationTags = (string[])sourcePreset.destinationTags.Clone();

                operations = (int[])sourcePreset.operations.Clone();
                mulDivFactors = (string[])sourcePreset.mulDivFactors.Clone();
                precisionDigits = (string[])sourcePreset.precisionDigits.Clone();
                appendTexts = (string[])sourcePreset.appendTexts.Clone();

                useAnotherPresetAsSource = sourcePreset.useAnotherPresetAsSource;
                anotherPresetAsSource = sourcePreset.anotherPresetAsSource;

                conditionIsChecked = sourcePreset.conditionIsChecked;
                conditionField = sourcePreset.conditionField;
                comparison = sourcePreset.comparison;
                comparedField = sourcePreset.comparedField;

                fileFormatIndex = sourcePreset.fileFormatIndex;
                exportedTrackListName = sourcePreset.exportedTrackListName;
            }

            public string getHotkeyChar()
            {
                if (!hotkeyAssigned)
                    return string.Empty;
                else if (applyToSelectedTracks)
                    return "";
                else
                    return "";
            }

            public string getHotkeyPostfix()
            {
                string hotkeyChar = getHotkeyChar();

                if (hotkeyChar == string.Empty)
                    return hotkeyChar;
                else
                    return " " + hotkeyChar;
            }

            public string getHotkeyDescription()
            {
                return ReportPresetHotkeyDescription + ": " + ToString();
            }

            public override string ToString()
            {
                return getName() + getHotkeyPostfix();
            }

            public string getName()
            {
                if (!string.IsNullOrWhiteSpace(name))
                    return name;
                else if (autoName == null)
                    generateAutoName(null, null);

                return autoName;
            }

            public void generateAutoName(ColumnAttributesDict grDictRef, ColumnAttributesDict fnDictRef)
            {
                autoName = string.Empty;

                if (grDictRef == null && fnDictRef == null)
                {
                    grDictRef = new ColumnAttributesDict();
                    fnDictRef = new ColumnAttributesDict();
                    int ColumnCount = prepareDict(grDictRef, null, groupings, 0);
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

        public static void LoadPresetColumnAttributes(List<PresetColumnAttributes> presetAttribsListRef, List<string> shortIdsRef, List<List<int>> columnIndicesRef,
            List<List<string>> expressionsRef, ColumnAttributes attribs, int columnIndex)
        {
            PresetColumnAttributes presetAttribs;

            string shortId = attribs.getShortId();
            int index = shortIdsRef.IndexOf(shortId);
            if (index == -1)
            {
                shortIdsRef.Add(shortId);

                index = shortIdsRef.Count - 1;

                presetAttribs = new PresetColumnAttributes(attribs);
                presetAttribsListRef.Add(presetAttribs);
                expressionsRef.Add(new List<string>());
                columnIndicesRef.Add(new List<int>());
            }

            expressionsRef[index].Add(attribs.expression);
            columnIndicesRef[index].Add(columnIndex);
        }

        public void SetMultipleItemsSplitterComboBoxCue(ComboBox comboBox, string cue)
        {
            if (string.IsNullOrEmpty(cue))
                SetComboBoxCue(multipleItemsSplitterComboBox, DontUseSplitter);
            else
                SetComboBoxCue(multipleItemsSplitterComboBox, cue);
        }

        // Helper for Iterate()
        private static void MoveDependent(IEnumerator<string>[] enumerators, int[] dependentGroupingColumns, string[] lastValues, int currentLoopIndex, bool reset)
        {
            for (int j = dependentGroupingColumns.Length - 1; j >= 0; j--)
            {
                if (currentLoopIndex != j && dependentGroupingColumns[currentLoopIndex] == dependentGroupingColumns[j]) // Let's move all dependent groupings
                {
                    if (reset)
                        enumerators[j].Reset();

                    enumerators[j].MoveNext();
                    lastValues[j] = enumerators[j].Current;
                }
            }
        }

        // Let's iterate through all possible split grouping values (but not repeating the same groupings for different expressions)
        //
        // Accepts IEnumerator<split grouping values>[grouping column count],
        //
        // int[grouping column count] dependentGroupingColumns: indices of groupings
        //     in global dictionary "groupings" (which doesn't include expressions)
        //
        // Returns false when iteration completed, and string[grouping column count] lastValues on every iteration
        private static bool Iterate(IEnumerator<string>[] enumerators, int[] dependentGroupingColumns, string[] lastValues, ref int minimalLoopIndex, ref int currentLoopIndex)
        {
            bool movedThis = enumerators[currentLoopIndex].MoveNext();

            int currentGroupingIndex = dependentGroupingColumns[currentLoopIndex];

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

        // SortedDictionary<composed groupings OR OTHER UNIQUE ID, function results>
        public class AggregatedTags : SortedDictionary<string, ConvertStringsResult[]>
        {
            // Accepts List<split grouping values>[grouping column count],
            //     returns List<string -> composed groupings> for every split grouping
            // int[grouping column count] dependentGroupingColumns: indices of groupings
            //     in global dictionary "groupings" (which doesn't include expressions)
            public static List<string> GetComposedGroupingValues(List<string>[] groupingValuesLists, int[] dependentGroupingColumns, bool totals)
            {
                var composedGroupings = new List<string>();

                if (groupingValuesLists.Length == 0)
                {
                    composedGroupings.Add(string.Empty);
                    return composedGroupings;
                }

                var groupingValues = new string[groupingValuesLists.Length];

                string[] lastValues = new string[groupingValuesLists.Length]; // This array will store all grouping values of last iteration
                int minimalLoopIndex = groupingValuesLists.Length - 2;
                int currentLoopIndex = groupingValuesLists.Length - 1;

                // Neither of the lists in groupingValuesLists array can be empty, so let's get 1st value of every list
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

                    composedGroupings.Add(string.Join(MultipleItemsSplitterId.ToString(), groupingValues));


                    if (totals)
                    {
                        for (int j = groupingValues.Length - 1; j >= 0; j--)
                        {
                            groupingValues[j] = TotalsString;
                            composedGroupings.Add(string.Join(MultipleItemsSplitterId.ToString(), groupingValues));
                        }
                    }
                }
                while (Iterate(enumerators, dependentGroupingColumns, lastValues, ref minimalLoopIndex, ref currentLoopIndex));

                return composedGroupings;
            }

            public void add(string url, List<string> composedGroupingsList, ColumnAttributesDict functions,
                string[] functionValues, string[] parameter2Values)
            {
                if (functionValues == null || functionValues.Length == 0)
                {
                    foreach (var composedGroupings in composedGroupingsList)
                    {
                        ConvertStringsResult[] aggregatedValues;

                        if (!TryGetValue(composedGroupings, out aggregatedValues))
                        {
                            aggregatedValues = new ConvertStringsResult[1];
                            aggregatedValues[0] = new ConvertStringsResult(ResultType.UseOtherResults, DataType.String); //This are URLs (SortedDictionary<url, false>)

                            Add(composedGroupings, aggregatedValues);
                        }

                        if (url != null) // Let's deal with URLs (SortedDictionary<url, false>)
                            aggregatedValues[0].urls.AddReplace(url, false);
                    }
                }
                else
                {
                    foreach (var composedGroupings in composedGroupingsList)
                    {
                        ConvertStringsResult[] aggregatedValues;

                        if (!TryGetValue(composedGroupings, out aggregatedValues))
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
                                aggregatedValues[i].items.AddReplace(currentFunctionValue, false);
                                aggregatedValues[i].resultType = ResultType.ItemCount;

                                function.resultType = ResultType.ItemCount;
                                function.dataType = DataType.String;
                            }
                            else if (function.functionType == LrFunctionType.AverageCount)
                            {
                                aggregatedValues[i].items1.AddReplace(currentFunctionValue, false);
                                aggregatedValues[i].items.AddReplace(parameter2Values[i], false);

                                function.resultType = ResultType.UnknownOrString;
                                function.dataType = DataType.String;
                            }
                            else if (function.functionType == LrFunctionType.Average)
                            {
                                aggregatedValues[i].items.AddReplace(parameter2Values[i], false);
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


                            if (url != null) // Let's deal with URLs (SortedDictionary<url, false>)
                                aggregatedValues[i].urls.AddReplace(url, false);
                        }
                    }
                }
            }

            public static int CompareField(string composedGroupings, ConvertStringsResult[] convertResults, int fieldNumber, ColumnAttributesDict groupings, string comparedValue)
            {
                if (fieldNumber < groupings.Count)
                {
                    string field = composedGroupings.Split(MultipleItemsSplitterId)[fieldNumber];

                    if (field == TotalsString)
                        field = (MsgAllTags + " \"" + groupings.ElementAt(fieldNumber).Value.getColumnName(true, true, true) + "\"").ToUpper();

                    return field.CompareTo(comparedValue);
                }
                else
                {
                    ConvertStringsResult currentFunctionResult = convertResults[fieldNumber - groupings.Count];
                    ConvertStringsResult comparedFunctionValue = ConvertStrings(comparedValue, ResultType.UseOtherResults, DataType.String);

                    return currentFunctionResult.compare(comparedFunctionValue);
                }
            }

            public static string GetField(string composedGroupings, ConvertStringsResult[] convertResults, int fieldNumber, ColumnAttributesDict groupings, int operation, string mulDivFactorRepr, string precisionDigitsRepr, string appendedText)
            {
                if (fieldNumber < groupings.Count)
                {
                    string field = composedGroupings.Split(MultipleItemsSplitterId)[fieldNumber];

                    if (field == TotalsString)
                        field = (MsgAllTags + " \"" + groupings.ElementAt(fieldNumber).Value.getColumnName(true, true, true) + "\"").ToUpper();

                    return field;
                }
                else
                {
                    return convertResults[fieldNumber - groupings.Count].getFormattedResult(operation, mulDivFactorRepr, precisionDigitsRepr, appendedText);
                }
            }

            public static string[] GetGroupings(KeyValuePair<string, ConvertStringsResult[]> keyValue, ColumnAttributesDict groupings)
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
                            fields[i] = (MsgAllTags + " '" + groupings.ElementAt(i).Value.getColumnName(true, true, true) + "'").ToUpper();

                    return fields;
                }
            }
        }

        public static string GetSplitterRepresentation(string splitter, bool trimValues, bool addSpacePrefix)
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

        public static string GetColumnName(string tagName, string tag2Name, LrFunctionType type, string splitter, bool trimValues,
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
                if (attribs.parameterName == SequenceNumberName) // It's a number. Let's right-align the column.
                {
                    columnRightAlignments[j] = true;
                    resultTypes[j] = ResultType.Double;
                }
                else if (attribs.parameterName == DateCreatedTagName) // It's a number. Let's right-align the column.
                {
                    columnRightAlignments[j] = true;
                    resultTypes[j] = ResultType.DateTime;
                }
                else
                {
                    DataType columnType = MbApiInterface.Setting_GetDataType(GetTagId(attribs.parameterName));
                    if (columnType == DataType.Number || columnType == DataType.Rating) // Let's right-align the column.
                    {
                        columnRightAlignments[j] = true;
                        resultTypes[j] = ResultType.Double;
                    }
                    else if (columnType == DataType.DateTime) // Let's right-align the column.
                    {
                        columnRightAlignments[j] = true;
                        resultTypes[j] = ResultType.DateTime;
                    }
                    else // It's either string, or internal plugin pseudo-tag like "Filename". Let's left-align the column.
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
                if (function.resultType >= ResultType.AutoDouble && function.resultType <= ResultType.ItemCount) // It's either number or date/time/duration. Let's right-align the column.
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
            if (previewTable.RowCount == 0) // There are no rows in preview table yet, so let's adjust column text alignment according to data type
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
                    if (i != artworkField)
                    {
                        if (row[i].StartsWith(MsgAllTags))
                            previewTable.Rows[previewTable.RowCount - 1].Cells[i].Style.Font = TotalsFont;

                        previewTable.Rows[previewTable.RowCount - 1].Cells[i].ToolTipText = row[i] + "\n\n" + LrCellToolTip;

                        if (maxWidths[i] < row[i].Length)
                            maxWidths[i] = row[i].Length;
                    }
                    else
                    {
                        //Lets replace string hashes in the Artwork column with images.
                        string stringHash = row[artworkField];
                        Bitmap pic;

                        if (!artworks.TryGetValue(stringHash, out pic))
                            pic = artworks[DefaultArtworkHash];

                        previewTable.Rows[previewTable.RowCount - 1].Cells[artworkField].ValueType = typeof(Bitmap);
                        previewTable.Rows[previewTable.RowCount - 1].Cells[artworkField].Value = pic;
                        previewTable.Rows[previewTable.RowCount - 1].MinimumHeight = PreviewTableDefaultArtworkSize;
                    }
                }
            }
        }

        private void previewTable_UpdateTable()
        {
            for (int i = 0; i < previewTable.ColumnCount; i++)
            {
                if (i != artworkField && maxWidths[i] > 0)
                    previewTable.Columns[i].FillWeight = maxWidths[i];
            }

            previewTable.AutoResizeColumns();
            previewTable.AutoResizeRows();

            if (previewTable.RowCount > 0)
                previewTable.CurrentCell = previewTable.Rows[0].Cells[0];
        }

        private void adjustPresetAsSourceUI(ComboBox foundPresetRefs, ReportPresetReference selectedPresetRef, bool? selectedRefCheckStatus)
        {
            if (selectedRefCheckStatus == true)
            {
                toolTip1.SetToolTip(useAnotherPresetAsSourceCheckBox, UseAnotherPresetAsSourceToolTip);
                useAnotherPresetAsSourceCheckBox.Image = Resources.transparent_15;
                SetComboBoxCue(foundPresetRefs, string.Empty);

                foundPresetRefs.SelectedItem = selectedPresetRef;
            }
            else if (selectedRefCheckStatus == null)
            {
                toolTip1.SetToolTip(useAnotherPresetAsSourceCheckBox, UseAnotherPresetAsSourceToolTip + "\n\n" + UseAnotherPresetAsSourceIsSenselessToolTip.ToUpper());
                useAnotherPresetAsSourceCheckBox.Image = ThemedBitmapAddRef(this, WarningWide);
                SetComboBoxCue(foundPresetRefs, selectedPresetRef.name);

                foundPresetRefs.SelectedIndex = -1;
            }
            else //if(selectedRefCheckStatus == false)
            {
                toolTip1.SetToolTip(useAnotherPresetAsSourceCheckBox, UseAnotherPresetAsSourceToolTip + "\n\n" + UseAnotherPresetAsSourceIsInBrokenChainToolTip.ToUpper());
                useAnotherPresetAsSourceCheckBox.Image = ThemedBitmapAddRef(this, WarningWide);
                SetComboBoxCue(foundPresetRefs, selectedPresetRef.name);

                foundPresetRefs.SelectedIndex = -1;
            }
        }

        private bool? checkPresetReference(ReportPreset[] presetList, SortedDictionary<Guid, bool> referredPresetGuids,
            bool useAnotherPresetAsSource, ReportPresetReference presetReference)
        // Returns: true - reference chain is good, null - reference chain is senseless (there is !conditionIsChecked), false - reference chain is bad (looped references)
        {
            if (!useAnotherPresetAsSource)
                return true;
            else if (presetReference.permanentGuid == Guid.Empty)
                return null;


            foreach (var preset in presetList)
            {
                if (preset.permanentGuid == presetReference.permanentGuid)
                {
                    if (!referredPresetGuids.Contains(preset.permanentGuid))
                    {
                        referredPresetGuids.Add(preset.permanentGuid, false);
                        bool? checkStatus = checkPresetReference(presetList, referredPresetGuids, preset.useAnotherPresetAsSource, preset.anotherPresetAsSource);

                        if (checkStatus == false)
                            return false;
                        else if (checkStatus == null)
                            return null;
                        else if (!preset.conditionIsChecked)
                            return null;
                        else //if (preset.conditionIsChecked)
                            return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        private void findFilteringPresetsUI(ComboBox foundPresetRefs, CheckedListBox presetList, ReportPreset currentPreset, ReportPresetReference selectedPresetRef)
        {
            bool? selectedRefCheckStatus = (selectedPresetRef.permanentGuid == Guid.Empty ? true : false);
            foundPresetRefs.Items.Clear();

            var referredPresetGuids = new SortedDictionary<Guid, bool>();
            var currentPresets = getReportPresetsArrayUI();

            foreach (var item in presetList.Items)
            {
                ReportPreset preset = item as ReportPreset;

                if (preset != currentPreset)
                {
                    referredPresetGuids.Clear();
                    referredPresetGuids.Add(currentPreset.permanentGuid, false);

                    var presetAsReference = new ReportPresetReference(preset);
                    bool? checkStatus = checkPresetReference(currentPresets, referredPresetGuids, true, presetAsReference);

                    if (preset.permanentGuid == selectedPresetRef.permanentGuid)
                    {
                        selectedRefCheckStatus = checkStatus;
                        selectedPresetRef.name = preset.getName();

                        if (checkStatus == true)
                            foundPresetRefs.Items.Add(selectedPresetRef);
                    }
                    else if (checkStatus == true)
                    {
                        foundPresetRefs.Items.Add(presetAsReference);
                    }
                }
            }

            adjustPresetAsSourceUI(foundPresetRefs, selectedPresetRef, selectedRefCheckStatus);
            lastSelectedRefCheckStatus = selectedRefCheckStatus;
        }

        private void clearArtworks()
        {
            artwork?.Dispose();
            foreach (var pair in artworks)
                pair.Value.Dispose();

            artworks.Clear();
        }

        private void resetLocalsAndUiControls()
        {
            cachedAppliedPresetGuid = Guid.NewGuid();

            while (previewTable.ColumnCount > 0)
                previewTable.Columns.RemoveAt(0);

            tagsDataGridView.Rows.Clear();
            tagsDataGridViewSelectedRow = -1;
            expressionsDataGridView.Rows.Clear();

            expressionBackup = string.Empty;
            splitterBackup = string.Empty;
            trimValuesBackup = false;

            conditionField = -1;
            comparedField = -1;

            artworkField = -1;
            clearArtworks();

            sequenceNumberField = -1;

            sourceFieldComboBox.Items.Clear();
            conditionFieldList.Items.Clear();
            comparedFieldList.Items.Clear();

            groupings.Clear();
            groupingsDict.Clear();
            functionsDict.Clear();

            operations.Clear();
            mulDivFactors.Clear();
            precisionDigits.Clear();
            appendTexts.Clear();

            functionComboBox.SelectedIndex = 0;

            clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, buttonPreview, buttonOK, buttonClose);
        }

        // Returns column count
        private static int prepareDict(ColumnAttributesDict dictRef, SortedDictionary<string, PresetColumnAttributes> presetDictRef,
            PresetColumnAttributes[] attribsSet, int startingColumnIndex)
        {
            int columnIndex = startingColumnIndex;
            int maxColumnIndex = startingColumnIndex - 1;
            for (int i = 0; i < attribsSet.Length; i++)
                maxColumnIndex += attribsSet[i].expressions.Length;

            if (presetDictRef != null)
                for (int i = 0; i < attribsSet.Length; i++)
                    presetDictRef.Add(attribsSet[i].getShortId(), attribsSet[i]);


                repeat_again:
            if (columnIndex <= maxColumnIndex)
            {
                for (int i = 0; i < attribsSet.Length; i++)
                {
                    for (int j = 0; j < attribsSet[i].expressions.Length; j++)
                    {
                        if (columnIndex == attribsSet[i].columnIndices[j])
                        {
                            dictRef.Add(attribsSet[i].getUniqueId(j),
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
            if (cachedAppliedPresetGuid == appliedPreset.guid)
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
            if (flagUnsavedChanges && !unsavedChanges)
            {
                unsavedChanges = true;
                buttonClose.Image = ThemedBitmapAddRef(this, WarningWide);
                toolTip1.SetToolTip(buttonClose, buttonCloseToolTip);
            }
            else if (!flagUnsavedChanges && unsavedChanges)
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

            assignHotkeyCheckBoxLabel.Text = assignHotkeyCheckBoxText + (MaximumNumberOfLRHotkeys - reportPresetsWithHotkeysCount) + "/" + MaximumNumberOfLRHotkeys;

            updatePreset();
            setUnsavedChanges(true);
        }

        private static class ResizedArtworkProvider
        {
            static TypeConverter tc = null;
            static byte[] hash;
            static MD5Cng md5 = null;

            public static void Init(int artworkField, SortedDictionary<string, Bitmap> artworks)
            {
                if (artworkField != -1)
                {
                    var pic = new Bitmap(DefaultArtwork);

                    tc = TypeDescriptor.GetConverter(typeof(Bitmap));
                    md5 = new MD5Cng();

                    DefaultArtworkHash = GetResizedArtworkBase64Hash(ref pic);

                    try { hash = md5.ComputeHash((byte[])tc.ConvertTo(pic, typeof(byte[]))); }
                    catch { hash = md5.ComputeHash(new byte[] { 0x00 }); }

                    artworks.AddReplace(DefaultArtworkHash, pic);
                }
            }

            public static string GetResizedArtworkBase64Hash(ref Bitmap pic)
            {
                if (SavedSettings.resizeArtwork)
                {
                    float xSF = SavedSettings.xArtworkSize / (float)pic.Width;
                    float ySF = SavedSettings.yArtworkSize / (float)pic.Height;
                    float SF;

                    if (xSF >= ySF)
                        SF = ySF;
                    else
                        SF = xSF;


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

            public static string GetBase64ArtworkBase64Hash(string artworkBase64, SortedDictionary<string, Bitmap> artworks, out Bitmap pic)
            {
                try
                {
                    if (artworkBase64 != string.Empty)
                        pic = (Bitmap)tc.ConvertFrom(Convert.FromBase64String(artworkBase64));
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

        public static string ConvertSequenceNumberToString(int i)
        {
            return i.ToString();


            string oldSequenceNumber = i.ToString("D9");
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

        private string executePreset(string[] queriedFiles, bool interactive, bool saveResultsToTags, string functionId,
            bool? filterResults) // filterResults: null - proceed as usual (filter results on tag saving only, any "interactive" is allowed),
                                 // true & "!interactive" - update lastFiles by filtered file list
                                 // ALL OTHER COMBINATIONS OF filterResults AND interactive ARE PROHIBITED!
                                 // NOT NULL filterResults MUST BE USED ONLY FOR FILTERING PRESET CHAIN INVOKED INSIDE THIS FUNCTION

        {
            string parameterCombinationErrorMessage = "Prohibited parameter combination: interactive = " + interactive + ", filterResults = " + filterResults;
            if (!interactive)
            {
                if (filterResults == false)
                    throw new Exception(parameterCombinationErrorMessage);
            }
            else
            {
                if (filterResults != null)
                    throw new Exception(parameterCombinationErrorMessage);
            }


            if (functionId != null && string.IsNullOrWhiteSpace(functionId))
            {
                cachedAppliedPresetGuid = Guid.NewGuid();
                return SbIncorrectLrFunctionId;
            }

            if (functionId != null && functionsDict.Count == 0)
            {
                cachedAppliedPresetGuid = Guid.NewGuid();
                return "???";
            }


            //LET'S DEAL WITH ANOTHER PRESET AS A SOURCE
            if (appliedPreset.useAnotherPresetAsSource)
            {
                var checkStatus = checkPresetReference(reportPresets, new SortedDictionary<Guid, bool>(), true, appliedPreset.anotherPresetAsSource);
                if (checkStatus != true)
                {
                    if (functionId == null)
                    {
                        SetStatusbarText(SbBrokenPresetRetrievalChain.Replace("%%PRESETNAME%%", appliedPreset.getName()), true);
                        System.Media.SystemSounds.Question.Play();
                        DisablePlaySoundOnce = true;
                    }

                    cachedAppliedPresetGuid = Guid.NewGuid();
                    return "!!!";
                }
                ReportPreset anotherPresetAsSource = appliedPreset.anotherPresetAsSource.findPreset(reportPresets);

                var appliedPresetInUse = appliedPreset;
                appliedPreset = anotherPresetAsSource;
                executePreset(queriedFiles, false, false, null, true);
                queriedFiles = lastFiles;
                appliedPreset = appliedPresetInUse;
            }


            //MAIN PROCESSING
            prepareLocals();

            string query;
            string tagValue;


            // int[] dependentGroupingColumns will be used several time below, let's cache it
            // 
            // int[grouping column count] dependentGroupingColumns: indices of groupings
            //     in global dictionary "groupings" (which doesn't include expressions),
            //     but -1 for groupings having only one column 
            int[] dependentGroupingColumns = new int[groupingsDict.Count];
            int p = -1;
            foreach (var grouping in groupings.Values)
            {
                p++;

                for (int l = 0; l < grouping.columnIndices.Length; l++)
                    dependentGroupingColumns[grouping.columnIndices[l]] = p;
            }


            //Let's cache tag/prop ids
            MetaDataType[] queriedGroupingsTagIds = new MetaDataType[groupings.Count];
            for (int l = 0; l < queriedGroupingsTagIds.Length; l++)
                queriedGroupingsTagIds[l] = 0;

            FilePropertyType[] queriedGroupingsPropIds = new FilePropertyType[groupings.Count];
            for (int l = 0; l < queriedGroupingsPropIds.Length; l++)
                queriedGroupingsPropIds[l] = 0;


            MetaDataType[] queriedActualGroupingsTagIds = new MetaDataType[groupings.Count];
            for (int l = 0; l < queriedActualGroupingsTagIds.Length; l++)
                queriedActualGroupingsTagIds[l] = 0;

            FilePropertyType[] queriedActualGroupingsPropIds = new FilePropertyType[groupings.Count];
            for (int l = 0; l < queriedActualGroupingsTagIds.Length; l++)
                queriedActualGroupingsTagIds[l] = 0;


            foreach (var function in functionsDict.Values)
            {
                DataType dataType = MbApiInterface.Setting_GetDataType(GetTagId(function.parameterName));

                if (dataType == DataType.Number)
                {
                    function.resultType = ResultType.Double;
                    function.dataType = dataType;
                }
                else if (dataType == DataType.Rating)
                {
                    function.resultType = ResultType.Double;
                    function.dataType = dataType;
                }
                else if (dataType == DataType.DateTime)
                {
                    function.resultType = ResultType.Year;
                    function.dataType = dataType;
                }
                else
                {
                    function.resultType = ResultType.UseOtherResults;
                    function.dataType = DataType.String;
                }
            }


            int lastSeqNumInOrder = 1;
            SortedDictionary<string, int> seqNumInOrder = null;


            int i = -1;
            foreach (var attribs in groupings.Values)
            {
                i++;

                MetaDataType tagId = GetTagId(attribs.parameterName);
                string nativeTagName = MbApiInterface.Setting_GetFieldName(tagId);

                if (attribs.parameterName == SequenceNumberName)
                {
                    nativeTagName = null;
                    tagId = (MetaDataType)(-99);
                    queriedActualGroupingsTagIds[i] = tagId;

                    seqNumInOrder = new SortedDictionary<string, int>();
                }
                else
                {
                    if (tagId == 0)
                        queriedActualGroupingsPropIds[i] = GetPropId(attribs.parameterName);
                    else
                        queriedActualGroupingsTagIds[i] = tagId;
                }


                //MusicBee doesn't support these tags for querying, so let's skip them in query
                if (tagId != MetaDataType.AlbumArtistRaw && nativeTagName != null && nativeTagName != ArtworkName)
                {
                    if (tagId == 0)
                        queriedGroupingsPropIds[i] = GetPropId(attribs.parameterName);
                    else
                        queriedGroupingsTagIds[i] = tagId;
                }
            }


            bool cachedValuesAreRelevant = true;

            if (cachedAppliedPresetGuid != appliedPreset.guid)
                cachedValuesAreRelevant = false;


            SortedDictionary<string, bool>[] queriedGroupingValues = new SortedDictionary<string, bool>[groupings.Count];
            for (int l = 0; l < queriedGroupingValues.Length; l++)
                queriedGroupingValues[l] = new SortedDictionary<string, bool>();

            SortedDictionary<string, bool>[] queriedActualGroupingValues = new SortedDictionary<string, bool>[groupings.Count];
            for (int l = 0; l < queriedActualGroupingValues.Length; l++)
                queriedActualGroupingValues[l] = new SortedDictionary<string, bool>();


            //Let's add default artwork
            clearArtworks();
            ResizedArtworkProvider.Init(artworkField, artworks);


            queriedFilesActualComposedGroupingValues.Clear(); //<url, composed groupings>

            bool queryOnlyGroupings = false;
            if (functionsDict.Count == 0)
                queryOnlyGroupings = true;

            if (queriedFiles == null && !queryOnlyGroupings)
            {
                query = "domain=Library";
            }
            else
            {
                if (queriedFiles == null && queryOnlyGroupings)
                {
                    if (!MbApiInterface.Library_QueryFilesEx("domain=Library", out queriedFiles))
                    {
                        cachedAppliedPresetGuid = appliedPreset.guid;
                        return string.Empty;
                    }

                    if (queriedFiles.Length == 0)
                    {
                        cachedAppliedPresetGuid = appliedPreset.guid;
                        return string.Empty;
                    }
                }

                if (queryOnlyGroupings)
                    tags.Clear();

                for (int n = 0; n < queriedFiles.Length; n++)
                {
                    List<string>[] currentFileGroupingValues = null; // array (size of grouping count) of list of split values

                    if (queryOnlyGroupings)
                    {
                        currentFileGroupingValues = new List<string>[groupingsDict.Count];
                        for (int f = 0; f < currentFileGroupingValues.Length; f++)
                            currentFileGroupingValues[f] = new List<string>();
                    }

                    i = -1;
                    foreach (var attribs in groupings.Values)
                    {
                        i++;

                        {
                            //Let's remember actual grouping values for future reuse
                            MetaDataType tagId = queriedActualGroupingsTagIds[i];
                            FilePropertyType propId = 0;
                            if (tagId == 0)
                                propId = queriedActualGroupingsPropIds[i];

                            if (tagId == (MetaDataType)(-99))
                            {
                                tagValue = "xXxXxXxXx"; //Let's reserve space for future numbers
                            }
                            else if (tagId == 0 && propId == 0)
                            {
                                tagValue = string.Empty; //Will ignore this grouping...
                            }
                            else if (tagId == ArtistArtistsId || tagId == ComposerComposersId) //Lets make smart conversion of list of artists/composers
                            {
                                tagValue = GetFileTag(queriedFiles[n], tagId);
                                if (smartOperation)
                                    tagValue = GetTagRepresentation(tagValue);
                                else
                                    tagValue = RemoveRoleIds(tagValue);
                            }
                            else if (attribs.parameterName == ArtworkName) //It's artwork image. Lets fill cell with hash codes. 
                            {
                                tagValue = ResizedArtworkProvider.GetBase64ArtworkBase64Hash(
                                    GetFileTag(queriedFiles[n], MetaDataType.Artwork), artworks, out Bitmap pic);
                                artwork = pic; //For CD booklet export
                            }
                            else
                            {
                                if (tagId == 0)
                                {
                                    tagValue = MbApiInterface.Library_GetFileProperty(queriedFiles[n], propId);
                                }
                                else
                                {
                                    tagValue = GetFileTag(queriedFiles[n], tagId, true);
                                }
                            }

                            if (!queryOnlyGroupings)
                                queriedActualGroupingValues[i].AddReplace(tagValue, false);
                        }

                        if (queryOnlyGroupings)
                        {
                            var columnIndicesTagValues = attribs.getSplitValues(queriedFiles[n], tagValue);
                            foreach (var columnIndexTag in columnIndicesTagValues)
                                currentFileGroupingValues[columnIndexTag.index].Add(columnIndexTag.value);
                        }
                        else
                        {
                            //Let's remember only grouping values, which can be used in query in Library_QueryFilesEx function
                            MetaDataType tagId = queriedGroupingsTagIds[i];
                            FilePropertyType propId = 0;
                            if (tagId == 0)
                                propId = queriedGroupingsPropIds[i];

                            if (tagId == 0 && propId == 0)
                            {
                                //MusicBee doesn't support for querying these tags, so let's skip them in query
                            }
                            else
                            {
                                if (tagId == 0)
                                {
                                    tagValue = MbApiInterface.Library_GetFileProperty(queriedFiles[n], propId);
                                }
                                else
                                {
                                    tagValue = GetFileTag(queriedFiles[n], tagId, true);
                                }

                                queriedGroupingValues[i].AddReplace(tagValue, false);
                            }
                        }
                    }

                    if (queryOnlyGroupings)
                    {
                        if (interactive)
                            SetStatusbarTextForFileOperations(LibraryReportsCommandSbText, true, n, queriedFiles.Length, appliedPreset.getName());
                        else //if (functionId == null)
                            SetStatusbarTextForFileOperations(ApplyingLibraryReportSbText, true, n, queriedFiles.Length, appliedPreset.getName());

                        List<string> composedGroupingValuesList = AggregatedTags.GetComposedGroupingValues(currentFileGroupingValues, dependentGroupingColumns, appliedPreset.totals);

                        if (!queriedFilesActualComposedGroupingValues.TryGetValue(queriedFiles[n], out List<string> existingComposedGroupingValuesList))
                        {
                            existingComposedGroupingValuesList = new List<string>();
                            queriedFilesActualComposedGroupingValues.Add(queriedFiles[n], existingComposedGroupingValuesList);
                        }

                        foreach (var composedGroupingValues in composedGroupingValuesList)
                        {
                            if (!existingComposedGroupingValuesList.Contains(composedGroupingValues))
                                existingComposedGroupingValuesList.Add(composedGroupingValues);

                            if (sequenceNumberField != -1)
                            {
                                if (!seqNumInOrder.Contains(composedGroupingValues))
                                    seqNumInOrder.Add(composedGroupingValues, lastSeqNumInOrder++);
                            }
                        }

                        tags.add(queriedFiles[n], composedGroupingValuesList, null, null, null);
                    }
                }

                if (queryOnlyGroupings)
                {
                    if (sequenceNumberField != -1)
                    {
                        AggregatedTags tags2 = new AggregatedTags();

                        foreach (KeyValuePair<string, ConvertStringsResult[]> keyValue in tags)
                        {
                            int seqNum = seqNumInOrder[keyValue.Key];
                            string sequenceNumber = ConvertSequenceNumberToString(seqNum);
                            tags2.Add(keyValue.Key.Replace("xXxXxXxXx", sequenceNumber), keyValue.Value);
                        }

                        tags = tags2;
                    }


                    cachedAppliedPresetGuid = appliedPreset.guid;
                    applyOnlyGroupingsPresetResults(queriedFiles, interactive, sequenceNumberField, filterResults);
                    return "...";
                }


                query = @"<SmartPlaylist><Source Type=""1""><Conditions CombineMethod=""All"">";

                i = -1;
                foreach (var attribs in groupings.Values)
                {
                    i++;

                    MetaDataType tagId = queriedGroupingsTagIds[i];
                    FilePropertyType propId = queriedGroupingsPropIds[i];

                    if (tagId == 0 && propId == 0)
                    {
                        //Nothing to do...
                    }
                    else
                    {
                        string nativeTagName = MbApiInterface.Setting_GetFieldName(tagId);

                        query += @"<Condition Field=""" + nativeTagName + @""" Comparison=""IsIn""";

                        int n = 1;
                        foreach (var queriedValues in queriedGroupingValues[i])
                        {
                            query += @" Value" + (n++) + @"=""" + queriedValues.Key.Replace("\"", "&quot;") + @"""";
                        }

                        query += @" />";
                    }
                }

                query += "</Conditions></Source></SmartPlaylist>";
            }


            if (cachedValuesAreRelevant && queriedFiles != null)
            {
                cachedValuesAreRelevant = false;

                if (cachedQueriedActualGroupingValues != null && cachedQueriedActualGroupingValues.Length == queriedActualGroupingValues.Length)
                {
                    for (i = 0; i < cachedQueriedActualGroupingValues.Length; i++)
                    {
                        if (cachedQueriedActualGroupingValues[i].Count == queriedActualGroupingValues[i].Count)
                        {
                            cachedValuesAreRelevant = true;

                            foreach (var cachedPair in cachedQueriedActualGroupingValues[i])
                            {
                                foreach (var pair in queriedActualGroupingValues[i])
                                {
                                    if (cachedPair.Key != pair.Key)
                                    {
                                        cachedValuesAreRelevant = false;
                                        goto loop_exit;
                                    }
                                }
                            }
                        }
                    }
                }
            }

        loop_exit:
            if (cachedValuesAreRelevant)
            {
                cachedAppliedPresetGuid = appliedPreset.guid;
                cachedQueriedActualGroupingValues = queriedActualGroupingValues;


                if (queriedFiles == null)
                {
                    if (!MbApiInterface.Library_QueryFilesEx(query, out queriedFiles))
                        return string.Empty;

                    if (queriedFiles.Length == 0)
                        return string.Empty;

                    return applyPresetResults(queriedFiles, interactive, saveResultsToTags, functionId, sequenceNumberField, filterResults);
                }
                else
                {
                    return applyPresetResults(queriedFiles, interactive, saveResultsToTags, functionId, sequenceNumberField, filterResults);
                }
            }


            if (!MbApiInterface.Library_QueryFilesEx(query, out string[] files))
            {
                cachedAppliedPresetGuid = appliedPreset.guid;
                return string.Empty;
            }

            if (files.Length == 0)
            {
                cachedAppliedPresetGuid = appliedPreset.guid;
                return string.Empty;
            }


            tags.Clear();

            List<string>[] groupingValuesList = new List<string>[groupingsDict.Count];
            for (int f = 0; f < groupingValuesList.Length; f++)
                groupingValuesList[f] = new List<string>();

            List<LrFunctionType> functionTypes = new List<LrFunctionType>();
            string[] functionValues = new string[functionsDict.Count];
            string[] parameter2Values = new string[functionsDict.Count];


            if (!interactive && filterResults == true) // Only for filtering by another preset
                ; // Nothing...
            else // Let's later cache results for function ids
                fileTags.Clear();

            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                bool skipFile = false; //If current file grouping tags are not contained in
                                       // queriedActualGroupingValues, i.e. queried file selection is excessive due to unsupported (and skipped in query) tags

                if (backgroundTaskIsCanceled)
                {
                    if (interactive)
                        Invoke(updateTable);

                    cachedAppliedPresetGuid = Guid.NewGuid();
                    return string.Empty;
                }

                string currentFile = files[fileCounter];


                if (interactive)
                    SetStatusbarTextForFileOperations(LibraryReportsCommandSbText, true, fileCounter, files.Length, appliedPreset.getName());
                else if (functionId == null)
                    SetStatusbarTextForFileOperations(ApplyingLibraryReportSbText, true, fileCounter, files.Length, appliedPreset.getName());


                for (int f = 0; f < groupingValuesList.Length; f++)
                    groupingValuesList[f].Clear();


                i = -1;
                foreach (var attribs in groupings.Values)
                {
                    i++;

                    MetaDataType tagId = queriedActualGroupingsTagIds[i];
                    FilePropertyType propId = queriedActualGroupingsPropIds[i];

                    if (tagId == (MetaDataType)(-99))
                    {
                        tagValue = "xXxXxXxXx"; //Let's reserve space for future numbers
                    }
                    else if (tagId == 0 && propId == 0)
                    {
                        tagValue = string.Empty; //Will ignore this grouping...
                    }
                    else if (tagId == ArtistArtistsId || tagId == ComposerComposersId) //Let's make smart conversion of list of artists/composers
                    {
                        tagValue = GetFileTag(currentFile, tagId);
                        if (smartOperation)
                            tagValue = GetTagRepresentation(tagValue);
                        else
                            tagValue = RemoveRoleIds(tagValue);
                    }
                    else
                    {
                        if (tagId == 0)
                        {
                            tagValue = MbApiInterface.Library_GetFileProperty(currentFile, propId);
                        }
                        else
                        {
                            tagValue = GetFileTag(currentFile, tagId, true);
                        }
                    }


                    if (queriedFiles != null && !queriedActualGroupingValues[i].Contains(tagValue))
                    {
                        skipFile = true;
                        break; //Break grouping loop
                    }

                    var columnIndicesTags = attribs.getSplitValues(currentFile, tagValue);
                    foreach (var columnIndexTag in columnIndicesTags)
                        groupingValuesList[columnIndexTag.index].Add(columnIndexTag.value);
                }

                if (skipFile)
                    continue; //Continue file loop

                List<string> composedGroupingValuesList = AggregatedTags.GetComposedGroupingValues(groupingValuesList, dependentGroupingColumns, appliedPreset.totals);

                if (!queriedFilesActualComposedGroupingValues.TryGetValue(currentFile, out List<string> existingComposedGroupingValuesList))
                {
                    existingComposedGroupingValuesList = new List<string>();
                    queriedFilesActualComposedGroupingValues.Add(currentFile, existingComposedGroupingValuesList);
                }

                foreach (var composedGroupingValues in composedGroupingValuesList)
                {
                    if (!existingComposedGroupingValuesList.Contains(composedGroupingValues))
                        existingComposedGroupingValuesList.Add(composedGroupingValues);

                    if (sequenceNumberField != -1)
                    {
                        if (!seqNumInOrder.Contains(composedGroupingValues))
                            seqNumInOrder.Add(composedGroupingValues, lastSeqNumInOrder++);
                    }
                }

                functionTypes.Clear();

                int j = -1;
                foreach (var attribs in functionsDict.Values)
                {
                    j++;

                    MetaDataType tagId = GetTagId(attribs.parameterName);

                    if (attribs.parameterName == SequenceNumberName)
                    {
                        tagValue = ConvertSequenceNumberToString(fileCounter);
                    }
                    else if (tagId == ArtistArtistsId || tagId == ComposerComposersId) //Lets make smart conversion of list of artists/composers
                    {

                        tagValue = GetFileTag(currentFile, tagId);
                        if (smartOperation)
                            tagValue = GetTagRepresentation(tagValue);
                        else
                            tagValue = RemoveRoleIds(tagValue);
                    }
                    else
                    {
                        if (tagId == 0)
                        {
                            FilePropertyType propId = GetPropId(attribs.parameterName);
                            tagValue = MbApiInterface.Library_GetFileProperty(currentFile, propId);
                        }
                        else
                        {
                            tagValue = GetFileTag(currentFile, tagId, true);
                        }
                    }

                    functionTypes.Add(attribs.functionType);
                    functionValues[j] = tagValue;

                    if (attribs.functionType == LrFunctionType.Average || attribs.functionType == LrFunctionType.AverageCount)
                    {
                        tagId = GetTagId(attribs.parameter2Name);

                        if (tagId == ArtistArtistsId || tagId == ComposerComposersId) //Lets make smart conversion of list of artists/composers
                        {

                            tagValue = GetFileTag(currentFile, tagId);
                            if (smartOperation)
                                tagValue = GetTagRepresentation(tagValue);
                            else
                                tagValue = RemoveRoleIds(tagValue);
                        }
                        else
                        {
                            if (tagId == 0)
                            {
                                FilePropertyType propId = GetPropId(attribs.parameter2Name);
                                tagValue = MbApiInterface.Library_GetFileProperty(currentFile, propId);
                            }
                            else
                            {
                                tagValue = GetFileTag(currentFile, tagId, true);
                            }
                        }

                        parameter2Values[j] = tagValue;
                    }
                    else
                    {
                        parameter2Values[j] = null;
                    }
                }

                if (queriedFiles == null || queriedFiles.Contains(currentFile))
                {
                    tags.add(currentFile, composedGroupingValuesList, functionsDict, functionValues, parameter2Values);

                    if (!interactive && filterResults == true) // Only for filtering by another preset
                        ; // Nothing...
                    else // Let's cache results for function ids
                        fileTags.add(currentFile, new List<string> { currentFile }, functionsDict, functionValues, parameter2Values);
                }
                else
                {
                    tags.add(null, composedGroupingValuesList, functionsDict, functionValues, parameter2Values);
                }
            }


            if (sequenceNumberField != -1)
            {
                AggregatedTags tags2 = new AggregatedTags();

                foreach (KeyValuePair<string, ConvertStringsResult[]> keyValue in tags)
                {
                    int seqNum = seqNumInOrder[keyValue.Key];
                    string sequenceNumber = ConvertSequenceNumberToString(seqNum);
                    tags2.Add(keyValue.Key.Replace("xXxXxXxXx", sequenceNumber), keyValue.Value);
                }

                tags = tags2;
            }


            cachedAppliedPresetGuid = appliedPreset.guid;
            if (queriedFiles == null)
                return applyPresetResults(files, interactive, saveResultsToTags, functionId, sequenceNumberField, filterResults);
            else
                return applyPresetResults(queriedFiles, interactive, saveResultsToTags, functionId, sequenceNumberField, filterResults);
        }

        private string applyPresetResults(string[] queriedFiles, bool interactive, bool saveResultsToTags, string functionId, int sequenceNumberGrouping,
            bool? filterResults) // filterResults: true - filter queriedFiles list by condition, false - undo filtering of queriedFiles,
                                 // null - skip filtering, proceed as usual, true & !interactive - update lastFiles by filtered file list
        {
            if (queriedFiles == null)
                queriedFiles = lastFiles;
            else
                lastFiles = (string[])queriedFiles.Clone();

            if (functionId != null)
                return getFunctionResult(queriedFiles[0], queriedFilesActualComposedGroupingValues, functionId);


            if (sequenceNumberGrouping != -1)
            {
                AggregatedTags tags2 = new AggregatedTags();

                int i = 1;
                foreach (KeyValuePair<string, ConvertStringsResult[]> keyValue in tags)
                {
                    string sequenceNumber = ConvertSequenceNumberToString(i);
                    tags2.Add(keyValue.Key.Replace("xXxXxXxXx", sequenceNumber), keyValue.Value);
                    i++;
                }

                tags = tags2;
            }


            List<string[]> rows = new List<string[]>();

            if (!interactive && filterResults == true) // Only for filtering by another preset
            {
                var filteredFiles = new List<string>();

                for (int i = 0; i < queriedFiles.Length; i++)
                {
                    if (backgroundTaskIsCanceled)
                    {
                        cachedAppliedPresetGuid = Guid.NewGuid();
                        return string.Empty;
                    }

                    List<string> composedGroupingValuesList = queriedFilesActualComposedGroupingValues[queriedFiles[i]];
                    foreach (string composedGroupingValues in composedGroupingValuesList)
                    {
                        ConvertStringsResult[] functionValues = tags[composedGroupingValues];

                        if (checkCondition(composedGroupingValues, functionValues))
                        {
                            filteredFiles.Add(queriedFiles[i]);
                            break; // Let's continue with the next file
                        }
                    }
                }

                lastFiles = new string[filteredFiles.Count];
                filteredFiles.CopyTo(lastFiles);
            }
            else if (interactive)
            {
                bool showRow = true;

                int filesCount = 0;
                int groupingsCount = 0;
                int totalGroupingsCount = tags.Count;
                foreach (KeyValuePair<string, ConvertStringsResult[]> keyValue in tags)
                {
                    if (backgroundTaskIsCanceled && filterResults == null)
                    {
                        Invoke(updateTable);

                        cachedAppliedPresetGuid = Guid.NewGuid();
                        return string.Empty;
                    }

                    filesCount += keyValue.Value[0].urls.Count;

                    string composedGroupingValues = keyValue.Key;
                    string[] groupingsRow = AggregatedTags.GetGroupings(keyValue, groupingsDict);
                    string[] row = new string[groupingsRow.Length + keyValue.Value.Length];

                    ConvertStringsResult[] functionValues = tags[composedGroupingValues];

                    if (filterResults == true)
                    {
                        string comparedValue;

                        if (comparedField == -1)
                            comparedValue = comparedFieldText;
                        else
                            comparedValue = AggregatedTags.GetField(composedGroupingValues, functionValues, comparedField, groupingsDict,
                                0, null, null, null);

                        showRow = checkCondition(composedGroupingValues, functionValues);
                    }

                    groupingsRow.CopyTo(row, 0);
                    for (int i = groupingsRow.Length; i < row.Length; i++)
                    {
                        string functionValue = AggregatedTags.GetField(composedGroupingValues, keyValue.Value, i, groupingsDict,
                                operations[i - groupingsRow.Length], mulDivFactors[i - groupingsRow.Length], precisionDigits[i - groupingsRow.Length], appendTexts[i - groupingsRow.Length]);

                        row[i] = functionValue;
                    }


                    if (showRow)
                    {
                        rows.Add(row);

                        //SetStatusbarTextForFileOperations(LibraryReportsGeneratingPreviewCommandSbText, true, groupingsCount, totalGroupingsCount, appliedPreset.getName());
                        if (groupingsCount % 100 == 0)
                        {
                            SetStatusbarTextForFileOperations(LibraryReportsGeneratingPreviewCommandSbText, true, groupingsCount, totalGroupingsCount, appliedPreset.getName(), 0);
                            Invoke(addRowsToTable, rows);
                            rows.Clear();
                        }

                        groupingsCount++;
                    }

                    previewIsGenerated = true;
                }

                SetStatusbarTextForFileOperations(LibraryReportsGeneratingPreviewCommandSbText, true, --filesCount, filesCount, appliedPreset.getName());
            }


            if (saveResultsToTags)
            {
                SetResultingSbText(appliedPreset.getName(), false, true);
                saveFields(interactive, queriedFiles, queriedFilesActualComposedGroupingValues);
            }
            else
            {
                SetResultingSbText(appliedPreset.getName(), true, true);
            }

            if (interactive)
            {
                Invoke(addRowsToTable, rows);
                Invoke(updateTable);
            }

            return "...";
        }

        private void applyOnlyGroupingsPresetResults(string[] queriedFiles, bool interactive, int sequenceNumberGrouping, bool? filterResults)
        // filterResults: true - filter queriedFiles list by condition, false - undo filtering of queriedFiles,
        // null - skip filtering, proceed as usual, true & !interactive - update lastFiles by filtered file list
        {
            if (queriedFiles == null)
                queriedFiles = lastFiles;
            else
                lastFiles = (string[])queriedFiles.Clone();


            if (sequenceNumberGrouping != -1)
            {
                AggregatedTags tags2 = new AggregatedTags();

                int i = 1;
                foreach (KeyValuePair<string, ConvertStringsResult[]> keyValue in tags)
                {
                    string sequenceNumber = ConvertSequenceNumberToString(i);
                    tags2.Add(keyValue.Key.Replace("xXxXxXxXx", sequenceNumber), keyValue.Value);
                    i++;
                }

                tags = tags2;
            }


            if (!interactive && filterResults == true) // Only for filtering by another preset
            {
                var filteredFiles = new List<string>();

                for (int i = 0; i < queriedFiles.Length; i++)
                {
                    if (backgroundTaskIsCanceled)
                    {
                        cachedAppliedPresetGuid = Guid.NewGuid();
                        return;
                    }

                    List<string> composedGroupingValuesList = queriedFilesActualComposedGroupingValues[queriedFiles[i]];
                    foreach (string composedGroupingValues in composedGroupingValuesList)
                    {
                        if (checkCondition(composedGroupingValues, null))
                        {
                            filteredFiles.Add(queriedFiles[i]);
                            break; // Let's continue with the next file
                        }
                    }
                }

                lastFiles = new string[filteredFiles.Count];
                filteredFiles.CopyTo(lastFiles);
            }
            else if (interactive)
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

                        cachedAppliedPresetGuid = Guid.NewGuid();
                        return;
                    }

                    string[] row = AggregatedTags.GetGroupings(keyValue, groupingsDict);


                    if (filterResults == true)
                    {
                        string comparedValue;

                        if (comparedField == -1)
                            comparedValue = comparedFieldText;
                        else
                            comparedValue = row[comparedField];

                        showRow = checkCondition(keyValue.Key, null);
                    }

                    if (showRow)
                    {
                        rows.Add(row);

                        //Plugin.SetStatusbarTextForFileOperations(LibraryReportsGeneratingPreviewCommandSbText, true, groupingsCount, totalGroupingsCount, appliedPreset.getName());
                        if (groupingsCount % 100 == 0)
                        {
                            SetStatusbarTextForFileOperations(LibraryReportsGeneratingPreviewCommandSbText, true, groupingsCount, totalGroupingsCount, appliedPreset.getName(), 0);
                            Invoke(addRowsToTable, rows);
                            rows.Clear();
                        }
                    }

                    groupingsCount++;

                    previewIsGenerated = true;
                }


                SetStatusbarTextForFileOperations(LibraryReportsGeneratingPreviewCommandSbText, true, --groupingsCount, totalGroupingsCount, appliedPreset.getName(), 0);
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

            string value = AggregatedTags.GetField(composedGroupings, convertResults, conditionField, groupingsDict,
                0, null, null, null);
            string comparedValue;


            if (comparedField == -1)
                comparedValue = comparedFieldText;
            else
                comparedValue = AggregatedTags.GetField(composedGroupings, convertResults, comparedField, groupingsDict,
                    0, null, null, null);

            return checkCondition(AggregatedTags.CompareField(composedGroupings, convertResults, conditionField, groupingsDict, comparedValue));
        }

        private string getFunctionResult(string queriedFile, SortedDictionary<string, List<string>> queriedFilesActualComposedGroupingValues, string functionId)
        {
            ConvertStringsResult[] functionValues = fileTags[queriedFile];

            int i = 0;
            foreach (var attribs in functionsDict.Values)
            {
                if (savedFunctionIds[i] == functionId)
                {
                    return AggregatedTags.GetField(null, functionValues, groupingsDict.Count + i, groupingsDict,
                        operations[i], mulDivFactors[i], precisionDigits[i], appendTexts[i]);
                }

                i++;
            }

            return "???";
        }

        private void saveFields(bool interactive, string[] queriedFiles, SortedDictionary<string, List<string>> queriedFilesActualComposedGroupingValues)
        {
            for (int i = 0; i < queriedFiles.Length; i++)
            {
                if (backgroundTaskIsCanceled)
                {
                    cachedAppliedPresetGuid = Guid.NewGuid();
                    return;
                }

                if (interactive)
                    SetStatusbarTextForFileOperations(LibraryReportsCommandSbText, false, i, queriedFiles.Length, appliedPreset.getName());
                else
                    SetStatusbarTextForFileOperations(ApplyingLibraryReportSbText, false, i, queriedFiles.Length, appliedPreset.getName());


                List<string> composedGroupingValuesList = queriedFilesActualComposedGroupingValues[queriedFiles[i]];
                ConvertStringsResult[] fileFunctionValues = fileTags[queriedFiles[i]]; // Let's write aggregated values for entire file

                foreach (string composedGroupingValues in composedGroupingValuesList)
                {
                    if (checkCondition(composedGroupingValues, fileFunctionValues))
                    {
                        int j = 0;
                        foreach (var attribs in functionsDict.Values)
                        {
                            if (destinationTagIds[j] > 0)
                            {
                                string resultValue = AggregatedTags.GetField(composedGroupingValues, fileFunctionValues, groupingsDict.Count + j, groupingsDict,
                                    operations[j], mulDivFactors[j], precisionDigits[j], appendTexts[j]);
                                SetFileTag(queriedFiles[i], destinationTagIds[j], resultValue);
                            }

                            j++;
                        }

                        CommitTagsToFile(queriedFiles[i], false, false);
                        break; // Continue with the next file
                    }
                }
            }


            SetResultingSbText(appliedPreset.getName(), true, true);
        }

        public void autoApplyReportPresetsOnStartup()
        {
            lock (SavedSettings.reportsPresets)
            {
                try
                {
                    System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Lowest;

                    if (SavedSettings.reportsPresets.Length == 0)
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

                    int appliedAsrPresetsCount = 0;
                    lock (SavedSettings.reportsPresets)
                    {
                        foreach (var autoLibraryReportsPreset in SavedSettings.reportsPresets)
                        {
                            if (!autoLibraryReportsPreset.autoApply)
                                continue;

                            reportPresets = SavedSettings.reportsPresets;
                            appliedPreset = autoLibraryReportsPreset;
                            executePreset(null, false, true, null, null);

                            appliedAsrPresetsCount++;
                            if (appliedAsrPresetsCount >= SavedSettings.autoAppliedReportPresetsCount)
                                break;
                        }
                    }
                }
                catch (System.Threading.ThreadAbortException)
                {
                    // Let's just stop the thread...
                }
            }

            NumberOfTagChanges = 0;

            BackgroundTaskIsInProgress = false;
            RefreshPanels(true);
        }

        public void applyReportPresetToLibrary()
        {
            lock (SavedSettings.reportsPresets)
            {
                try
                {
                    System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Lowest;

                    executePreset(null, false, true, null, null);
                }
                catch (System.Threading.ThreadAbortException)
                {
                    // Let's just stop the thread...
                }
            }

            BackgroundTaskIsInProgress = false;

            RefreshPanels(true);
        }

        public void applyReportPresetToSelectedTracks()
        {
            lock (SavedSettings.reportsPresets)
            {
                try
                {
                    System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Lowest;

                    if (MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out string[] selectedFiles))
                    {
                        if (selectedFiles.Length == 0)
                        {
                            SetStatusbarText(MsgNoTracksSelected, true);
                            System.Media.SystemSounds.Exclamation.Play();
                        }
                        else
                        {
                            executePreset(selectedFiles, false, true, null, null);
                        }
                    }
                }
                catch (System.Threading.ThreadAbortException)
                {
                    // Let's just stop the thread...
                }
            }

            BackgroundTaskIsInProgress = false;
            RefreshPanels(true);
        }

        public static void AutoApplyReportPresets()
        {
            if (!SavedSettings.allowAsrLrPresetAutoexecution)
                return;
            else if (SavedSettings.autoAppliedReportPresetsCount == 0)
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

        public static void ApplyReportPreset(ReportPreset preset, ReportPreset[] reportPresets)
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
            {
                MbApiInterface.MB_CreateBackgroundTask(LibraryReportsCommandForHotkeys.applyReportPresetToSelectedTracks, null);
            }
            else
            {

                MbApiInterface.MB_CreateBackgroundTask(LibraryReportsCommandForHotkeys.applyReportPresetToLibrary, null);
            }
        }

        public static void ApplyReportPresetByHotkey(int presetIndex)
        {
            ReportPreset preset = ReportPresetsWithHotkeys[presetIndex - 1];

            if (preset.hotkeySlot != presetIndex - 1)
            {
                MessageBox.Show(MbForm, SbLrHotkeysAreAssignedIncorrectly, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ApplyReportPreset(preset, SavedSettings.reportsPresets);
        }

        public static string AutoCalculateReportPresetFunction(string calculatedFile, string functionId)
        {
            if (!ReportPresetIdsAreInitialized)
            {
                return MsgRefreshUi;
            }

            if (!IdsReportPresets.TryGetValue(functionId, out ReportPreset preset))
                return MsgIncorrectReportPresetId;

            LibraryReportsCommandForFunctionIds.reportPresets = SavedSettings.reportsPresets;
            LibraryReportsCommandForFunctionIds.appliedPreset = preset;

            string[] selectedFiles = new string[] { calculatedFile };
            string functionValue = LibraryReportsCommandForFunctionIds.executePreset(selectedFiles, false, false, functionId, null);

            return functionValue;
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

        public static void FillReportPresetsWithHotkeysArray()
        {
            for (int i = 0; i < ReportPresetsWithHotkeys.Length; i++)
            {
                ReportPresetsWithHotkeys[i] = null;
            }

            for (int i = 0; i < SavedSettings.reportsPresets.Length; i++)
            {
                if (SavedSettings.reportsPresets[i].hotkeyAssigned)
                    ReportPresetsWithHotkeys[SavedSettings.reportsPresets[i].hotkeySlot] = SavedSettings.reportsPresets[i];
            }
        }

        private static void RegisterPresetHotkey(Plugin tagToolsPlugin, ReportPreset preset, int slot)
        {
            if (preset == null)
                return;


            switch (slot)
            {
                case 0:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset1EventHandler);
                    break;
                case 1:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset2EventHandler);
                    break;
                case 2:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset3EventHandler);
                    break;
                case 3:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset4EventHandler);
                    break;
                case 4:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset5EventHandler);
                    break;
                case 5:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset6EventHandler);
                    break;
                case 6:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset7EventHandler);
                    break;
                case 7:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset8EventHandler);
                    break;
                case 8:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset9EventHandler);
                    break;
                case 9:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset10EventHandler);
                    break;
                case 10:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset11EventHandler);
                    break;
                case 11:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset12EventHandler);
                    break;
                case 12:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset13EventHandler);
                    break;
                case 13:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset14EventHandler);
                    break;
                case 14:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset15EventHandler);
                    break;
                case 15:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset16EventHandler);
                    break;
                case 16:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset17EventHandler);
                    break;
                case 17:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset18EventHandler);
                    break;
                case 18:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset19EventHandler);
                    break;
                case 19:
                    MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset20EventHandler);
                    break;
                default:
                    break;
            }
        }

        public static void RegisterLRPresetsHotkeys(Plugin tagToolsPlugin)
        {
            if (SavedSettings.dontShowLibraryReports)
                return;

            FillReportPresetsWithHotkeysArray();

            for (int i = 0; i < ReportPresetsWithHotkeys.Length; i += 2)
            {
                RegisterPresetHotkey(tagToolsPlugin, ReportPresetsWithHotkeys[i], i);
            }
        }

        public static void InitReportPresetFunctionIds()
        {
            ReportPresetIdsAreInitialized = false;

            IdsReportPresets.Clear();

            for (int i = 0; i < SavedSettings.reportsPresets.Length; i++)
            {
                ReportPreset autoLibraryReportsPreset = new ReportPreset(SavedSettings.reportsPresets[i], true);

                for (int j = 0; j < autoLibraryReportsPreset.functionIds.Length; j++)
                {
                    if (!string.IsNullOrWhiteSpace(autoLibraryReportsPreset.functionIds[j]))
                    {
                        IdsReportPresets.Add(autoLibraryReportsPreset.functionIds[j], autoLibraryReportsPreset);
                    }
                }
            }

            ReportPresetIdsAreInitialized = true;
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

            // Some column attributes are not unique, they can be replaced, and must not be included in column name
            string simpleColumnName = GetColumnName(fieldName, parameter2Name, type, null, false, expression, true, false, true);

            int oldSortedShortIdsIndex = sortedShortIds.IndexOf(shortId);
            int oldExpressionIndex = -1;
            if (shortIdsExprs.TryGetValue(shortId, out var exprs1))
                oldExpressionIndex = exprs1.IndexOf(expressionBackup);

            // Let's check if the column exists already
            int replacedColumnIndex = -1;

            if (newColumn == null) // Updating splitter, trim or expression. Let's silently replace old column 
            {
                updateColumns(shortId, fieldName, parameter2Name, type, splitter, trimValues, expression);
                expressionBackup = expression;
                splitterBackup = splitter;
                trimValuesBackup = trimValues;

                return true;
            }
            else if (groupingsDict.Contains(uniqueId) || functionsDict.Contains(uniqueId))
            {
                if (presetIsLoading)
                    throw new Exception("This field already exists in preset!");
                else
                {
                    if (MessageBox.Show(this, DoYouWantToReplaceTheFieldMsg.Replace(@"\\", "\n\n").Replace("%%FIELDNAME%%", simpleColumnName),
                    string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return false;
                    else // Confirmed replacement
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
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            conditionFieldList.Items.Add(fullFieldName);
            if (conditionFieldList.SelectedIndex == -1)
                conditionFieldList.SelectedIndex = 0;

            comparedFieldList.Items.Add(fullFieldName);

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
                previewTable.Columns[replacedColumnIndex].HeaderCell.Style = HeaderCellStyle;
                groupingsDict.Add(uniqueId, columnAttributes);
            }
            else if (type == LrFunctionType.Grouping)
            {
                if (replacedColumnIndex == -1)
                    replacedColumnIndex = groupingsDict.Count;

                previewTable.Columns.Insert(replacedColumnIndex, column);
                previewTable.Columns[replacedColumnIndex].HeaderCell.Style = HeaderCellStyle;
                groupingsDict.Add(uniqueId, columnAttributes);
            }
            else
            {
                if (replacedColumnIndex == -1)
                {
                    previewTable.Columns.Add(column);
                    previewTable.Columns[previewTable.ColumnCount - 1].HeaderCell.Style = HeaderCellStyle;
                }
                else
                {
                    previewTable.Columns.Insert(replacedColumnIndex, column);
                    previewTable.Columns[replacedColumnIndex].HeaderCell.Style = HeaderCellStyle;
                }

                functionsDict.Add(uniqueId, columnAttributes);


                destinationTagList.SelectedItem = NullTagName;
                savedFunctionIds.Add(string.Empty);
                savedDestinationTagsNames.Add(destinationTagList.SelectedItem as string);

                operations.Add(0);
                mulDivFactors.Add(mulDivFactorComboBox.Items[0] as string);
                precisionDigits.Add(precisionDigitsComboBox.Items[0] as string);
                appendTexts.Add(string.Empty);

                sourceFieldComboBox.Items.Add(fullFieldName);
                if (sourceFieldComboBox.SelectedIndex == -1)
                    sourceFieldComboBox.SelectedIndex = 0;
            }


            sortedShortIds = groupingsDict.idsToSortedList(false);
            sortedShortIds = functionsDict.idsToSortedList(false, sortedShortIds);
            int newSortedShortIdsIndex = sortedShortIds.IndexOf(shortId);

            expressionBackup = expression;
            splitterBackup = splitter;
            trimValuesBackup = trimValues;

            if (oldSortedShortIdsIndex == -1 || !shortIdsExprs.Contains(shortId)) // New column, even not considering expressions, OR replaced (and removed above) LAST expression
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

                if (!presetIsLoading) // Interactive
                {
                    selectRow(tagsDataGridView, newSortedShortIdsIndex);
                    fillExpressionsDataGridView(shortId, true); // New expression will be added from shortIdsExprs
                }
                else
                {
                    deselectAllRows(tagsDataGridView);
                    if (expressionsDataGridView.RowCount > 0)
                        expressionsDataGridView.Rows.Clear();
                }
            }
            else // Updating existing column (which may not be selected/current)
            {
                tagsDataGridView.Rows[oldSortedShortIdsIndex].Cells[1].Tag = columnAttributes;
                tagsDataGridView.Rows[oldSortedShortIdsIndex].Cells[2].Value = columnAttributes.parameterName;
                if (columnAttributes.functionType == LrFunctionType.Grouping)
                    tagsDataGridView.Rows[newSortedShortIdsIndex].Cells[3].Value = GetSplitterRepresentation(columnAttributes.splitter, columnAttributes.trimValues, false);

                if (oldSortedShortIdsIndex != tagsDataGridViewSelectedRow) // Updating tag column which is not current
                    selectRow(tagsDataGridView, oldSortedShortIdsIndex);

                var exprs = shortIdsExprs[shortId];

                if (oldExpressionIndex == -1) // It's new expression, let's add it to the end of list
                    exprs.Add(expression);
                else // It's replaced expression (already removed above, but it wasn't last one) 
                    exprs.Insert(oldExpressionIndex, expression);

                if (!presetIsLoading) // Interactive
                    fillExpressionsDataGridView(shortId, null); // New expression will be added from shortIdsExprs
            }



            if (previewTable.RowCount > 0)
                clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, buttonPreview, buttonExport, buttonClose);

            return true;
        }

        private void updateColumns(string shortId, string fieldName, string parameter2Name, LrFunctionType type, string splitter, bool trimValues, string expression)
        {
            string fullFieldName = GetColumnName(fieldName, parameter2Name, type, splitter, trimValues, expression, false, true, true);

            var columnAttributes = new ColumnAttributes(type, expression, fieldName, parameter2Name, splitter, trimValues);

            for (int i = 0; i < previewTable.ColumnCount; i++)
            {
                var colUniqueId = (string)previewTable.Columns[i].HeaderCell.Tag;

                ColumnAttributesDict currentDict;
                if (type == LrFunctionType.Grouping)
                    currentDict = groupingsDict;
                else
                    currentDict = functionsDict;

                // If the current column doesn't correspond to the type of currentDict, let's skip it
                if (!currentDict.TryGetValue(colUniqueId, out var currentAttributes))
                    continue;


                if (shortId == currentAttributes.getShortId())
                {
                    string newBackup;
                    if (currentAttributes.expression == expressionBackup) // Let's replace it
                    {
                        newBackup = expression;

                        int exprIndex = shortIdsExprs[shortId].IndexOf(expressionBackup);
                        if (exprIndex != -1)
                            shortIdsExprs[shortId][exprIndex] = expression;
                    }
                    else
                    {
                        newBackup = currentAttributes.expression; // Let's keep it as is
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

                functionsDict.Remove(uniqueId);

                int currentSourceFieldComboBoxSelectedIndex = sourceFieldComboBox.SelectedIndex;

                savedDestinationTagsNames.RemoveAt(index);
                savedFunctionIds.RemoveAt(index);

                operations.RemoveAt(index);
                mulDivFactors.RemoveAt(index);
                precisionDigits.RemoveAt(index);
                appendTexts.RemoveAt(index);

                sourceFieldComboBox.Items.RemoveAt(index);
                if (sourceFieldComboBox.SelectedIndex == -1 && sourceFieldComboBox.Items.Count > 0)
                    sourceFieldComboBox.SelectedIndex = 0;
                else if (sourceFieldComboBox.SelectedIndex == -1)
                    sourceFieldComboBox_SelectedIndexChanged(null, null);
            }


            conditionFieldList.Items.Remove(fullFieldName);
            if (conditionFieldList.SelectedIndex == -1 && conditionFieldList.Items.Count > 0)
                conditionFieldList.SelectedIndex = 0;


            if (comparedFieldList.Text == (string)comparedFieldList.SelectedValue)
                comparedFieldList.Text = string.Empty;

            comparedFieldList.Items.Remove(fullFieldName);

            if (conditionFieldList.Items.Count == 0)
            {
                conditionCheckBox.Enable(false);
                conditionCheckBox.Checked = false;
            }


            previewTable.Columns.RemoveAt(columnIndex);


            // Now let's deal with field & expression tables
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
                reportPresetsLocal[i] = (ReportPreset)presetList.Items[i];

            return reportPresetsLocal;
        }

        private bool prepareBackgroundPreview()
        {
            smartOperation = smartOperationCheckBox.Checked;

            resultsAreFiltered = false;
            buttonFilterResultsChangeLabel();

            if (!presetIsLoading && previewTable.RowCount == 0)
                presetTabControl.SelectedTab = tabPage1;

            previewTable.Rows.Clear();
            previewIsGenerated = false;

            if (backgroundTaskIsWorking())
                return true;

            if (previewTable.ColumnCount == 0)
            {
                //MessageBox.Show(this, tagToolsPlugin.msgNoTagsSelected);
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

            executePreset(files, true, false, null, null);
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

                selectedPreset.exportedTrackListName = reportOnlyFileName;

                if (SavedSettings.reportsPresets != null)
                {
                    foreach (ReportPreset preset in SavedSettings.reportsPresets)
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


            //Album artists/0 [Albums] for HTML grouped by albums, Album artists/sequence number of album artist for CD Booklets [Albums]
            SortedDictionary<string, List<string>> albumArtistsAlbums = new SortedDictionary<string, List<string>>();
            string base64Artwork = null;

            List<int> albumTrackCounts = new List<int>();

            System.IO.FileStream stream = new System.IO.FileStream(reportFullFileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);

            try
            {
                if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocument || selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentCdBooklet)
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
                            if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentCdBooklet)
                            {
                                string[] groupingsValues = AggregatedTags.GetGroupings(keyValue, groupingsDict);

                                if (base64Artwork == null)
                                    base64Artwork = groupingsValues[artworkField];
                                else if (base64Artwork == string.Empty)
                                    ;
                                else if (base64Artwork != groupingsValues[artworkField])
                                    base64Artwork = string.Empty;
                            }

                            groupingsValues1 = AggregatedTags.GetGroupings(keyValue, groupingsDict);

                            if (prevAlbumArtist1 != groupingsValues1[albumArtistField] || prevAlbum1 != groupingsValues1[albumField])
                            {
                                if (prevAlbumArtist1 != groupingsValues1[albumArtistField])
                                {
                                    if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentCdBooklet)
                                        albumArtistsAlbumsSequenceNumber++;

                                    prevAlbumArtist1 = groupingsValues1[albumArtistField];
                                    currentAlbumArtistAlbums = new List<string>();

                                    albumArtistsAlbums.Add(prevAlbumArtist1 + albumArtistsAlbumsSequenceNumber.ToString("D4"), currentAlbumArtistAlbums);
                                }
                                else
                                {
                                    currentAlbumArtistAlbums = albumArtistsAlbums[prevAlbumArtist1 + albumArtistsAlbumsSequenceNumber.ToString("D4")];
                                }

                                prevAlbum1 = groupingsValues1[albumField];

                                currentAlbumArtistAlbums.Add(prevAlbum1);
                                albumTrackCounts.Add(trackCount);
                                trackCount = 0;
                            }

                            trackCount++;
                        }

                        albumTrackCounts.Add(trackCount);


                        if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentByAlbums)
                            document = new HtmlDocumentByAlbum(stream, TagToolsPlugin, fileDirectoryPath, imagesDirectoryName);
                        else //if (selectedPreset.fileFormatIndex == 7)
                            document = new HtmlDocumentCDBooklet(stream, TagToolsPlugin, fileDirectoryPath, imagesDirectoryName);
                        break;
                    case LrReportFormat.HtmlDocument:
                        document = new HtmlDocument(stream, TagToolsPlugin, fileDirectoryPath, imagesDirectoryName);
                        break;
                    case LrReportFormat.HtmlTable:
                        document = new HtmlTable(stream, TagToolsPlugin, fileDirectoryPath, imagesDirectoryName);
                        break;
                    case LrReportFormat.TabSeparatedText:
                        document = new TextDocument(stream, TagToolsPlugin);
                        break;
                    case LrReportFormat.M3u:
                        document = new M3UDocument(stream, TagToolsPlugin);
                        break;
                    case LrReportFormat.Csv:
                        document = new CsvDocument(stream, TagToolsPlugin);
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


            if (selectedPreset.fileFormatIndex != LrReportFormat.HtmlDocumentCdBooklet)
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


            if (selectedPreset.fileFormatIndex != LrReportFormat.M3u && selectedPreset.fileFormatIndex != LrReportFormat.HtmlDocumentCdBooklet) //Lets write table headers
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


            foreach (KeyValuePair<string, ConvertStringsResult[]> keyValue in tags)
            {
                if (backgroundTaskIsCanceled)
                    return;

                if (checkCondition(keyValue.Key, keyValue.Value))
                {
                    string[] groupingsValues = AggregatedTags.GetGroupings(keyValue, groupingsDict);

                    if (selectedPreset.fileFormatIndex == LrReportFormat.HtmlDocumentByAlbums)
                    {
                        if (prevAlbumArtist != groupingsValues[albumArtistField])
                        {
                            i++;
                            prevAlbumArtist = groupingsValues[albumArtistField];
                            prevAlbum = groupingsValues[albumField];
                            document.beginAlbumArtist(groupingsValues[albumArtistField], groupingsValues.Length - 2 + functionsDict.Count);
                            document.beginAlbum(groupingsValues[albumField], artworks[groupingsValues[artworkField]], groupingsValues[artworkField], albumTrackCounts[i]);
                        }
                        else if (prevAlbum != groupingsValues[albumField])
                        {
                            i++;
                            prevAlbum = groupingsValues[albumField];
                            document.beginAlbum(groupingsValues[albumField], artworks[groupingsValues[artworkField]], groupingsValues[artworkField], albumTrackCounts[i]);
                        }
                    }


                    if (selectedPreset.fileFormatIndex == LrReportFormat.M3u) //It's M3U playlist
                    {
                        SortedDictionary<string, bool> urls = keyValue.Value[0].urls;

                        foreach (var urlPair in urls)
                            allUrls.AddReplace(urlPair.Key, false);
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
                                (document as HtmlDocumentCDBooklet).addTrack(int.Parse(groupingsValues[seqNumField]),
                                    null, null, groupingsValues[titleField], groupingsValues[durationField]);
                            }
                            else //Several albums
                            {
                                (document as HtmlDocumentCDBooklet).addTrack(int.Parse(groupingsValues[seqNumField]),
                                    null, groupingsValues[albumField], groupingsValues[titleField], groupingsValues[durationField]);
                            }
                        }
                        else //Several album artists
                        {
                            (document as HtmlDocumentCDBooklet).addTrack(int.Parse(groupingsValues[seqNumField]),
                                groupingsValues[albumArtistField], groupingsValues[albumField], groupingsValues[titleField],
                                groupingsValues[durationField]);
                        }
                    }


                    if (selectedPreset.fileFormatIndex != LrReportFormat.M3u) //It's NOT M3U playlist
                        document.writeRow(height);

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
                var waiter = new System.Threading.Thread(OpenReport);
                waiter.Start(reportFullFileName);
            }
        }

        public static void OpenReport(object document)
        {
            string documentPathFileName = document as string;

            System.Diagnostics.Process.Start(documentPathFileName);

            return; //Code below does not work reliably

            if (ProcessedReportDeletions.Contains(documentPathFileName))
                return;

            ProcessedReportDeletions.Add(documentPathFileName);

            const int waitingTimeout = 5; //In milliseconds

            int retryCount = 0;
            bool reportHasBeenLocked = false;
            while (!reportHasBeenLocked && retryCount < 30000 / waitingTimeout)
            {
                try
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(documentPathFileName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None))
                    {
                        //Nothing to do, just checking if file is read by external application. It is not. Let's retry.
                    }

                    retryCount++;
                    System.Threading.Thread.Sleep(waitingTimeout);
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
                    using (System.IO.FileStream stream = new System.IO.FileStream(documentPathFileName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None))
                    {
                        reportHasBeenLocked = false; //File is unlocked by external application. Let's try to delete it now.
                    }
                }
                catch (System.IO.IOException)
                {
                    //Nothing to do, just checking if file is read by external application. It is. Let's retry.
                    retryCount++;
                    System.Threading.Thread.Sleep(2000);
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
                    System.Threading.Thread.Sleep(2000);
                }
            }

            string documentExtension = Regex.Replace(documentPathFileName, @"^.*(\..*)", "$1");
            if (retry || documentExtension != ".htm")
            {
                ProcessedReportDeletions.Remove(documentPathFileName);
                return;
            }

            System.Threading.Thread.Sleep(10000);

            ProcessedReportDeletions.Remove(documentPathFileName);
            string imagesDirectoryPath = Regex.Replace(documentPathFileName, @"^(.*)\..*", "$1") + ".files";
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

        private void saveSettings()
        {
            SavedSettings.recalculateOnNumberOfTagsChanges = recalculateOnNumberOfTagsChangesCheckBox.Checked;
            SavedSettings.numberOfTagsToRecalculate = numberOfTagsToRecalculateNumericUpDown.Value;
            SavedSettings.autoAppliedReportPresetsCount = autoAppliedPresetCount;

            SavedSettings.smartOperation = smartOperationCheckBox.Checked;

            SavedSettings.reportsPresets = new ReportPreset[presetList.Items.Count];
            presetList.Items.CopyTo(SavedSettings.reportsPresets, 0);

            InitReportPresetFunctionIds();
            RegisterLRPresetsHotkeys(TagToolsPlugin);
            setUnsavedChanges(false);

            SavedSettings.openReportAfterExporting = openReportCheckBox.Checked;

            TagToolsPlugin.SaveSettings();
        }

        public override void enableDisablePreviewOptionControls(bool enable, bool dontChangeDisabled = false)
        {
            buttonAddPreset.Enable(enable);

            if (selectedPreset == null)
                enable = false;


            buttonCopyPreset.Enable(enable);
            buttonDeletePreset.Enable(enable && selectedPreset.userPreset);

            smartOperationCheckBox.Enable(enable && !previewIsGenerated);

            buttonFilterResults.Enable(enable && conditionCheckBox.Enabled && conditionCheckBox.Checked && previewIsGenerated);

            useHotkeyForSelectedTracksCheckBox.Enable(enable && assignHotkeyCheckBox.Checked && assignHotkeyCheckBox.Enabled);


            tagsDataGridView.Enable(enable && !previewIsGenerated && newColumn == false);
            expressionsDataGridView.Enable(enable && !previewIsGenerated && newColumn == false);

            sourceTagList.Enable(enable && !previewIsGenerated && newColumn == true);
            buttonAddFunction.Enable(enable && !previewIsGenerated && selectedPreset.userPreset);
            buttonUpdateFunction.Enable(enable && !previewIsGenerated && newColumn != false);

            parameter2ComboBox.Enable(enable && !previewIsGenerated && newColumn == true);
            parameter2Label.Enable(enable && !previewIsGenerated && newColumn == true);
            forTagLabel.Enable(enable && !previewIsGenerated && newColumn == true);
            functionComboBox.Enable(enable && !previewIsGenerated && newColumn == true);
            labelFunction.Enable(enable && !previewIsGenerated && newColumn == true);

            multipleItemsSplitterTrimCheckBox.Enable(enable && !previewIsGenerated && expressionBackup != null && selectedPreset.userPreset);
            multipleItemsSplitterComboBox.Enable(enable && !previewIsGenerated && expressionBackup != null && selectedPreset.userPreset);
            multipleItemsSplitterLabel.Enable(enable && !previewIsGenerated && expressionBackup != null && selectedPreset.userPreset);

            expressionLabel.Enable(enable && !previewIsGenerated && expressionBackup != null && selectedPreset.userPreset);
            expressionTextBox.Enable(enable && !previewIsGenerated && expressionBackup != null && selectedPreset.userPreset);
            buttonClearExpression.Enable(enable && !previewIsGenerated && expressionBackup != null && selectedPreset.userPreset);

            totalsCheckBox.Enable(enable && !previewIsGenerated);

            useAnotherPresetAsSourceCheckBox.Enable(enable && !previewIsGenerated);
            useAnotherPresetAsSourceComboBox.Enable(enable && !previewIsGenerated);

            conditionCheckBox.Enable(enable && !previewIsGenerated);

            resizeArtworkCheckBox.Enable(enable && !previewIsGenerated);
            xArtworkSizeUpDown.Enable(enable && resizeArtworkCheckBox.Checked && !previewIsGenerated);
            yArtworkSizeUpDown.Enable(enable && resizeArtworkCheckBox.Checked && !previewIsGenerated);
            labelXxY.Enable(enable && resizeArtworkCheckBox.Checked && !previewIsGenerated);
            labelPx.Enable(enable && resizeArtworkCheckBox.Checked && !previewIsGenerated);

            buttonExport.Enable(enable && previewIsGenerated);
            labelFormat.Enable(enable && previewIsGenerated);
            formatComboBox.Enable(enable && previewIsGenerated);
            openReportCheckBox.Enable(enable && previewIsGenerated);

            if (sourceFieldComboBox.SelectedIndex != -1)
            {
                operationComboBox.Enable(enable && !previewIsGenerated);
                mulDivFactorComboBox.Enable(enable && !previewIsGenerated);
                roundToLabel.Enable(enable && !previewIsGenerated);
                precisionDigitsComboBox.Enable(enable && !previewIsGenerated);
                digitsLabel.Enable(enable && !previewIsGenerated);
                appendLabel.Enable(enable && !previewIsGenerated);
                appendTextBox.Enable(enable && !previewIsGenerated);
            }
            else
            {
                operationComboBox.Enable(false);
                mulDivFactorComboBox.Enable(false);
                roundToLabel.Enable(false);
                precisionDigitsComboBox.Enable(false);
                digitsLabel.Enable(false);
                appendLabel.Enable(false);
                appendTextBox.Enable(false);
            }
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
            presetList.Enable(!backgroundTaskIsScheduled && (newColumn == false));

            buttonOK.Enable(previewIsGenerated || SavedSettings.allowCommandExecutionWithoutPreview);
            buttonPreview.Enable(true);
        }

        public override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enable(false);
            buttonPreview.Enable(false);
        }

        private void recalculateOnNumberOfTagsChangesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            numberOfTagsToRecalculateNumericUpDown.Enable(recalculateOnNumberOfTagsChangesCheckBox.Checked);
        }

        private void recalculateOnNumberOfTagsChangesCheckBoxLabel_Click(object sender, EventArgs e)
        {
            recalculateOnNumberOfTagsChangesCheckBox.Checked = !recalculateOnNumberOfTagsChangesCheckBox.Checked;
            recalculateOnNumberOfTagsChangesCheckBox_CheckedChanged(null, null);
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
                    reportPresetHotkeyUsedSlots[selectedPreset.hotkeySlot] = true;
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


                selectedPreset.comparedField = comparedFieldList.Text;
                selectedPreset.comparison = (Comparison)conditionList.SelectedIndex;
                selectedPreset.conditionField = conditionFieldList.Text;
                selectedPreset.conditionIsChecked = conditionCheckBox.Checked;
                selectedPreset.totals = totalsCheckBox.Checked;

                selectedPreset.useAnotherPresetAsSource = useAnotherPresetAsSourceCheckBox.Checked;
                if (!selectedPreset.useAnotherPresetAsSource)
                {
                    lastSelectedRefCheckStatus = true;
                    selectedPreset.anotherPresetAsSource = new ReportPresetReference();
                }
                else if (useAnotherPresetAsSourceComboBox.SelectedIndex != -1)
                {
                    lastSelectedRefCheckStatus = true;
                    selectedPreset.anotherPresetAsSource = (ReportPresetReference)useAnotherPresetAsSourceComboBox.SelectedItem;
                }

                selectedPreset.autoApply = presetList.GetItemChecked(presetList.SelectedIndex);

                selectedPreset.destinationTags = new string[savedDestinationTagsNames.Count];

                savedDestinationTagsNames.CopyTo(selectedPreset.destinationTags, 0);


                selectedPreset.operations = new int[sourceFieldComboBox.Items.Count];
                selectedPreset.mulDivFactors = new string[sourceFieldComboBox.Items.Count];
                selectedPreset.precisionDigits = new string[sourceFieldComboBox.Items.Count];
                selectedPreset.appendTexts = new string[sourceFieldComboBox.Items.Count];

                selectedPreset.operations = new int[operations.Count];
                operations.CopyTo(selectedPreset.operations);
                selectedPreset.mulDivFactors = new string[mulDivFactors.Count];
                mulDivFactors.CopyTo(selectedPreset.mulDivFactors);
                selectedPreset.precisionDigits = new string[precisionDigits.Count];
                precisionDigits.CopyTo(selectedPreset.precisionDigits);
                selectedPreset.appendTexts = new string[appendTexts.Count];
                appendTexts.CopyTo(selectedPreset.appendTexts);

                selectedPreset.generateAutoName(groupingsDict, functionsDict);

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

                //setUnsavedChanges(true);
            }
        }

        private void conditionFieldList_SelectedIndexChanged(object sender, EventArgs e)
        {
            setPresetChanged();
        }

        private void conditionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            setPresetChanged();
        }

        private void comparedFieldList_TextUpdate(object sender, EventArgs e)
        {
            setPresetChanged();
        }

        private void idTextBox_Leave(object sender, EventArgs e)
        {
            if (presetList.SelectedIndex == -1)
                return;

            if (sourceFieldComboBox.SelectedIndex == -1)
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
                savedFunctionIds[sourceFieldComboBox.SelectedIndex] = string.Empty;

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
                savedFunctionIds[sourceFieldComboBox.SelectedIndex] = string.Empty;
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
                            savedFunctionIds[sourceFieldComboBox.SelectedIndex] = string.Empty;
                            idTextBox.Text = string.Empty;
                            return;
                        }
                    }
                }

                for (int i = 0; i < sourceFieldComboBox.Items.Count; i++) //Lets iterate through current preset
                {
                    string id = savedFunctionIds[i];

                    if (idTextBox.Text == id && i != sourceFieldComboBox.SelectedIndex)
                    {
                        MessageBox.Show(this, MsgPresetExists.Replace("%%ID%%", idTextBox.Text), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        savedFunctionIds[sourceFieldComboBox.SelectedIndex] = string.Empty;
                        idTextBox.Text = string.Empty;
                        return;
                    }
                }

                savedFunctionIds[sourceFieldComboBox.SelectedIndex] = idTextBox.Text;

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
            saveSettings();

            if (ModifierKeys != Keys.Control)
                Close();
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
            previewTable.Rows.Clear();

            if (backgroundTaskIsWorking())
                return;

            if (previewTable.ColumnCount == 0)
            {
                //MessageBox.Show(this, tagToolsPlugin.msgNoTagsSelected);
                return;
            }

            appliedPreset = selectedPreset;
            prepareConditionRelatedLocals();

            if (resultsAreFiltered)
                applyPresetResults(null, true, false, null, -1, false);
            else
                applyPresetResults(null, true, false, null, -1, true);

            resultsAreFiltered = !resultsAreFiltered;

            buttonFilterResultsChangeLabel();
        }

        private void LibraryReportsCommand_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (PluginClosing)
            {
                return;
            }
            else if (unsavedChanges)
            {
                MessageBoxDefaultButton lastAnswer = SavedSettings.unsavedChangesConfirmationLastAnswer;
                MessageBoxButtons confirmationButtons = MessageBoxButtons.YesNo;

                if (!forceCloseForms)
                    confirmationButtons = MessageBoxButtons.YesNoCancel;

                forceCloseForms = true;

                DialogResult result = MessageBox.Show(this, MsgAsrDoYouWantToSaveChangesBeforeClosingTheWindow,
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

        private void LibraryReportsCommand_Load(object sender, EventArgs e)
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

            ignoreSplitterMovedEvent = false;
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (!ignoreSplitterMovedEvent)
                saveWindowLayout(0, 0, 0, splitContainer1.SplitterDistance);
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, HelpMsg, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void setColumnChanged(bool? newColumnParam)
        {
            if (presetIsLoading)
            {
                return;
            }
            else if (newColumn != false && newColumnParam == null)
            {
                return;
            }
            else if (newColumn != false) // Discarding changes
            {
                expressionTextBox.Text = expressionBackup;
                SetMultipleItemsSplitterComboBoxCue(multipleItemsSplitterComboBox, splitterBackup);
                multipleItemsSplitterTrimCheckBox.Checked = trimValuesBackup;

                setColumnSaved();

                return;
            }
            else if (newColumnParam == null) // Updating expression
            {
                //buttonUpdateFunction.Text = ButtonUpdateFunctionUpdateText;
                buttonUpdateFunction.Text = ButtonUpdateFunctionUpdateText;
                toolTip1.SetToolTip(buttonUpdateFunction, ButtonUpdateFunctionUpdateToolTip);
            }

            //buttonAddFunction.Text = ButtonAddFunctionDiscardText;
            buttonAddFunction.Text = ButtonAddFunctionDiscardText;
            toolTip1.SetToolTip(buttonAddFunction, ButtonAddFunctionDiscardToolTip);

            newColumn = newColumnParam;

            buttonUpdateFunction.Image = ThemedBitmapAddRef(this, Warning);

            enableDisablePreviewOptionControls(true);
        }

        private void setColumnSaved()
        {
            buttonAddFunction.Text = ButtonAddFunctionCreateText;
            toolTip1.SetToolTip(buttonAddFunction, ButtonAddFunctionCreateToolTip);

            newColumn = false;

            tagsDataGridView.Enable(true);
            expressionsDataGridView.Enable(true);

            //buttonUpdateFunction.Text = ButtonUpdateFunctionSaveText;
            buttonUpdateFunction.Text = ButtonUpdateFunctionSaveText;

            toolTip1.SetToolTip(buttonUpdateFunction, ButtonUpdateFunctionSaveToolTip);
            buttonUpdateFunction.Image = Resources.transparent_15;

            expressionTextBox.Text = expressionBackup;
            SetMultipleItemsSplitterComboBoxCue(multipleItemsSplitterComboBox, splitterBackup);
            multipleItemsSplitterTrimCheckBox.Checked = trimValuesBackup;

            enableDisablePreviewOptionControls(true);
        }

        private void buttonAddFunction_Click(object sender, EventArgs e)
        {
            setColumnChanged(true);
        }

        private void buttonUpdateFunction_Click(object sender, EventArgs e)
        {
            if (addColumn(sourceTagList.Text, parameter2ComboBox.Text, (LrFunctionType)functionComboBox.SelectedIndex,
                multipleItemsSplitterComboBox.Text, multipleItemsSplitterTrimCheckBox.Checked, expressionTextBox.Text))
            {
                setColumnSaved();
                setPresetChanged();
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
                    }
                }
                else
                {
                    ids = functionsDict.getAllIdsByShortId(shortId);
                    foreach (var id in ids)
                    {
                        var attribs = functionsDict[id];
                        removeColumn(attribs.parameterName, attribs.parameter2Name, attribs.functionType, null, false, attribs.expression);
                    }
                }
            }
        }
        private void tagsDataGridViewRowSelected(int rowIndex)
        {
            if (rowIndex >= 0) // Maybe new row is selected
            {
                tagsDataGridViewSelectedRow = rowIndex;

                var shortId = (string)tagsDataGridView.Rows[rowIndex].Cells[0].Tag;

                var commonAttr = (ColumnAttributes)tagsDataGridView.Rows[rowIndex].Cells[1].Tag;

                splitterBackup = commonAttr.splitter;
                trimValuesBackup = commonAttr.trimValues;

                functionComboBox.SelectedIndex = (int)commonAttr.functionType;
                sourceTagList.SelectedItem = commonAttr.parameterName;
                parameter2ComboBox.SelectedItem = commonAttr.parameter2Name;
                SetMultipleItemsSplitterComboBoxCue(multipleItemsSplitterComboBox, commonAttr.splitter);
                multipleItemsSplitterTrimCheckBox.Checked = commonAttr.trimValues;


                fillExpressionsDataGridView(shortId, false);
            }
            else
            {
                expressionsDataGridView.Rows.Clear();
            }

            enableDisablePreviewOptionControls(true);
        }

        private void tagsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 0 && e.RowIndex >= 0) // Maybe new row is selected
                if (tagsDataGridViewSelectedRow != e.RowIndex)
                    tagsDataGridViewRowSelected(e.RowIndex);
        }

        // If newExpression == true, then new row is added to expressionsDataGridView, select it, last row
        // If newExpression == false, then new tag column is selected, select 1st row here
        // If newExpression == null, then it's just existing expression replaced, reselect it
        private void fillExpressionsDataGridView(string shortId, bool? newExpression)
        {
            int index = getSelectedRow(expressionsDataGridView);

            if (expressionsDataGridView.RowCount > 0)
                expressionsDataGridView.Rows.Clear();

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
                var shortId = (string)tagsDataGridView.Rows[tagsDataGridViewSelectedRow].Cells[0].Tag;
                var commonAttr = (ColumnAttributes)tagsDataGridView.Rows[tagsDataGridViewSelectedRow].Cells[1].Tag;

                if (expressionsDataGridView.RowCount == 1)
                {
                    if (MessageBox.Show(this, DoYouWantToDeleteTheFieldMsg.Replace(@"\\", "\n\n")
                        .Replace("%%FIELDNAME%%", commonAttr.getColumnName(false, false, false)),
                        string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;
                }

                string expression = (string)expressionsDataGridView.Rows[e.RowIndex].Cells[1].Tag;
                removeColumn(commonAttr.parameterName, commonAttr.parameter2Name, commonAttr.functionType, commonAttr.splitter, commonAttr.trimValues, expression);
            }
        }

        private void expressionsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 0 && e.RowIndex >= 0) // Maybe new row is selected
            {
                expressionBackup = (string)expressionsDataGridView.Rows[e.RowIndex].Cells[1].Tag;
                expressionTextBox.Text = expressionBackup;

                enableDisablePreviewOptionControls(true);
            }
        }

        private void sourceTagList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sourceTagList.Text == ArtworkName || sourceTagList.Text == SequenceNumberName)
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
            // Nothing is required for now
        }

        private void multipleItemsSplitterComboBox_TextUpdate(object sender, EventArgs e)
        {
            if (multipleItemsSplitterComboBox.Text == splitterBackup || newColumn == true)
                return;

            setColumnChanged(null);
        }

        private string multipleItemsSplitterComboBoxValueChanged()
        {
            if (multipleItemsSplitterComboBox.SelectedItem == null)
                return null;


            string text = multipleItemsSplitterComboBox.GetItemText(multipleItemsSplitterComboBox.SelectedItem);

            if (multipleItemsSplitterComboBox.SelectedIndex == 1)
            {
                text = "\\0";
                multipleItemsSplitterTrimCheckBox.Checked = false;
            }
            else
            {
                text = text.TrimStart(' ');

                if (multipleItemsSplitterComboBox.SelectedIndex == 0)
                    multipleItemsSplitterTrimCheckBox.Checked = false;
                else
                    multipleItemsSplitterTrimCheckBox.Checked = true;
            }


            return text;
        }

        private void multipleItemsSplitterComboBox_DropDownClosed(object sender, EventArgs e)
        {
            string text = multipleItemsSplitterComboBoxValueChanged();

            if (text == null)
                return;


            SetMultipleItemsSplitterComboBoxCue(multipleItemsSplitterComboBox, text);

            if (text == splitterBackup || newColumn == true)
                return;

            setColumnChanged(null);
        }

        private void multipleItemsSplitterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Nothing is required for now
        }

        private void multipleItemsSplitterTrimCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (multipleItemsSplitterTrimCheckBox.Checked == trimValuesBackup || newColumn == true)
                return;

            setColumnChanged(null);
        }

        private void multipleItemsSplitterTrimCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!multipleItemsSplitterTrimCheckBox.Enabled)
                return;

            multipleItemsSplitterTrimCheckBox.Checked = !multipleItemsSplitterTrimCheckBox.Checked;
            multipleItemsSplitterTrimCheckBox_CheckedChanged(null, null);
        }

        private void functionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (presetIsLoading)
                return;


            if (functionComboBox.SelectedIndex >= functionComboBox.Items.Count - 2) // Average or Average Count
            {
                multipleItemsSplitterLabel.Visible = false;
                multipleItemsSplitterComboBox.Visible = false;
                multipleItemsSplitterTrimCheckBox.Visible = false;
                multipleItemsSplitterTrimCheckBoxLabel.Visible = false;

                parameter2Label.Visible = true;
                parameter2ComboBox.Visible = true;
            }
            else if (functionComboBox.SelectedIndex == 0) // Grouping
            {
                multipleItemsSplitterLabel.Visible = true;
                multipleItemsSplitterComboBox.Visible = true;
                multipleItemsSplitterTrimCheckBox.Visible = true;
                multipleItemsSplitterTrimCheckBoxLabel.Visible = true;

                parameter2Label.Visible = false;
                parameter2ComboBox.Visible = false;
            }
            else // Other functions
            {
                multipleItemsSplitterLabel.Visible = false;
                multipleItemsSplitterComboBox.Visible = false;
                multipleItemsSplitterTrimCheckBox.Visible = false;
                multipleItemsSplitterTrimCheckBoxLabel.Visible = true;

                parameter2Label.Visible = false;
                parameter2ComboBox.Visible = false;
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
            sortedShortIds.Clear();
            shortIdsExprs.Clear();

            groupings.Clear();
            groupingsDict.Clear();
            functionsDict.Clear();

            setColumnSaved();

            functionComboBox.SelectedIndex = 0;
            sourceTagList.SelectedIndex = 0;
            expressionTextBox.Text = string.Empty;

            if (index == -1)
            {
                SetComboBoxCue(multipleItemsSplitterComboBox, DontUseSplitter);

                selectedPreset = null;

                formatComboBox.SelectedIndex = 1;

                presetNameTextBox.Enable(false);

                lastSelectedRefCheckStatus = true;
                useAnotherPresetAsSourceCheckBox.Checked = false;
                useAnotherPresetAsSourceComboBox.Items.Clear();
                useAnotherPresetAsSourceCheckBox_CheckedChanged(null, null);

                multipleItemsSplitterTrimCheckBox.Checked = false;

                conditionCheckBox.Checked = false;
                conditionCheckBox_CheckedChanged(null, null);

                conditionFieldList.Text = string.Empty;
                conditionList.Text = string.Empty;
                comparedFieldList.Text = string.Empty;
                sourceFieldComboBox.Text = string.Empty;
                destinationTagList.Text = string.Empty;
                functionComboBox.Text = string.Empty;
                parameter2ComboBox.Text = string.Empty;
                sourceTagList.Text = string.Empty;

                idTextBox.Text = string.Empty;

                totalsCheckBox.Checked = false;

                assignHotkeyCheckBox.Enable(false);

                presetNameTextBox.Text = string.Empty;
                assignHotkeyCheckBox.Checked = false;
                useHotkeyForSelectedTracksCheckBox.CheckState = CheckState.Unchecked;

                sourceFieldComboBox_SelectedIndexChanged(null, null);

                tagsDataGridView.Rows.Clear();
                tagsDataGridViewSelectedRow = -1;
                expressionsDataGridView.Rows.Clear();

                expressionBackup = null;
                splitterBackup = null;
                trimValuesBackup = false;

                enableDisablePreviewOptionControls(true);
                disableQueryingOrUpdatingButtons();

                return;
            }


            resetLocalsAndUiControls();

            selectedPreset = (ReportPreset)presetList.SelectedItem;

            if (selectedPreset.fileFormatIndex == 0)
                formatComboBox.SelectedIndex = 1;
            else
                formatComboBox.SelectedIndex = (int)selectedPreset.fileFormatIndex - 1;


            presetNameTextBox.Enable(true);
            presetNameTextBox.ReadOnly = !selectedPreset.userPreset;


            assignHotkeyCheckBox.Checked = selectedPreset.hotkeyAssigned;
            useHotkeyForSelectedTracksCheckBox.Checked = selectedPreset.applyToSelectedTracks;
            totalsCheckBox.Checked = selectedPreset.totals;

            if (reportPresetsWithHotkeysCount < MaximumNumberOfLRHotkeys)
                assignHotkeyCheckBox.Enable(true);
            else if (reportPresetsWithHotkeysCount == MaximumNumberOfLRHotkeys && selectedPreset.hotkeyAssigned)
                assignHotkeyCheckBox.Enable(true);
            else
                assignHotkeyCheckBox.Enable(false);


            if (ignorePresetChangedEvent)
                return;


            presetNameTextBox.Text = selectedPreset.name ?? string.Empty;

            FillListByTagNames(destinationTagList.Items);


            //Groupings
            var tempDict = new ColumnAttributesDict();
            int columnCount = prepareDict(tempDict, null, selectedPreset.groupings, 0);
            foreach (var attribs in tempDict.Values)
            {
                addColumn(attribs.parameterName, null, LrFunctionType.Grouping,
                    attribs.splitter, attribs.trimValues, attribs.expression);
            }


            //Functions
            tempDict.Clear();
            prepareDict(tempDict, null, selectedPreset.functions, columnCount);
            foreach (var attribs in tempDict.Values)
            {
                addColumn(attribs.parameterName, attribs.parameter2Name, attribs.functionType,
                    null, false, attribs.expression);
            }


            functionComboBox.SelectedIndex = 0;
            functionComboBox_SelectedIndexChanged(null, null);
            sourceTagList.SelectedIndex = 0;
            sourceTagList_SelectedIndexChanged(null, null);
            parameter2ComboBox.SelectedIndex = 0;
            parameter2ComboBox_SelectedIndexChanged(null, null);
            multipleItemsSplitterTrimCheckBox.Checked = false;
            multipleItemsSplitterTrimCheckBox_CheckedChanged(null, null);

            savedFunctionIds.Clear();
            savedFunctionIds.AddRange(selectedPreset.functionIds);

            savedDestinationTagsNames.Clear();
            savedDestinationTagsNames.AddRange(selectedPreset.destinationTags);


            if (parameter2ComboBox.SelectedIndex == -1)
                parameter2ComboBox.SelectedItem = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);



            operations.Clear();
            operations.AddRange(selectedPreset.operations);
            mulDivFactors.Clear();
            mulDivFactors.AddRange(selectedPreset.mulDivFactors);
            precisionDigits.Clear();
            precisionDigits.AddRange(selectedPreset.precisionDigits);
            appendTexts.Clear();
            appendTexts.AddRange(selectedPreset.appendTexts);


            useAnotherPresetAsSourceCheckBox.Checked = selectedPreset.useAnotherPresetAsSource;
            findFilteringPresetsUI(useAnotherPresetAsSourceComboBox, presetList, selectedPreset, selectedPreset.anotherPresetAsSource);
            useAnotherPresetAsSourceCheckBox_CheckedChanged(null, null);


            totalsCheckBox.Checked = selectedPreset.totals;
            conditionCheckBox.Checked = selectedPreset.conditionIsChecked;
            conditionFieldList.Text = selectedPreset.conditionField;
            conditionList.SelectedIndex = (int)selectedPreset.comparison;
            comparedFieldList.Text = selectedPreset.comparedField;

            if (sourceFieldComboBox.Items.Count > 0)
            {
                sourceFieldComboBox.SelectedIndex = 0;
                destinationTagList.SelectedValue = selectedPreset.destinationTags[0];
                idTextBox.Text = savedFunctionIds[0];
            }
            else
            {
                sourceFieldComboBox_SelectedIndexChanged(null, null);
            }


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


            enableDisablePreviewOptionControls(true);
            enableQueryingOrUpdatingButtons();
        }

        private void conditionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            conditionFieldList.Enable(conditionCheckBox.Enabled && conditionCheckBox.Checked);
            conditionList.Enable(conditionCheckBox.Enabled && conditionCheckBox.Checked && conditionFieldList.SelectedIndex != -1);
            comparedFieldList.Enable(conditionCheckBox.Enabled && conditionCheckBox.Checked && conditionFieldList.SelectedIndex != -1);
            buttonFilterResults.Enable(conditionCheckBox.Enabled && conditionCheckBox.Checked && previewIsGenerated && conditionFieldList.SelectedIndex != -1);

            setPresetChanged();
        }

        private void conditionCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!conditionCheckBox.Enabled)
                return;

            conditionCheckBox.Checked = !conditionCheckBox.Checked;
            conditionCheckBox_CheckedChanged(null, null);
        }

        private void previewTable_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //e.ThrowException = false;
        }

        private void destinationTagList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sourceFieldComboBox.Items.Count != 0)
            {
                savedDestinationTagsNames[sourceFieldComboBox.SelectedIndex] = destinationTagList.Text;

                if (!sourceFieldComboBoxIndexChanging)
                    setPresetChanged();
            }
        }

        private void presetList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e != null)
            {
                if (ignoreCheckedPresetEvent)
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
                if (SavedSettings.allowAsrLrPresetAutoexecution)
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
                if (SavedSettings.allowAsrLrPresetAutoexecution)
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
            if (!assignHotkeyCheckBox.Enabled)
                return;

            assignHotkeyCheckBox.Checked = !assignHotkeyCheckBox.Checked;
            assignHotkeyCheckBox_CheckedChanged(null, null);
        }

        private void useHotkeyForSelectedTracksCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (presetList.SelectedIndex == -1)
                return;

            setPresetChanged();
        }

        private void useHotkeyForSelectedTracksCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!useHotkeyForSelectedTracksCheckBox.Enabled)
                return;

            useHotkeyForSelectedTracksCheckBox.Checked = !useHotkeyForSelectedTracksCheckBox.Checked;
            useHotkeyForSelectedTracksCheckBox_CheckedChanged(null, null);
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

                SetStatusbarText(LibraryReportsCommandSbText + " (" + SbSorting + ")", false);
                previewTable.Sort(comparator);
                SetStatusbarText(string.Empty, false);
            }
        }

        private void resizeArtworkCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SavedSettings.resizeArtwork = resizeArtworkCheckBox.Checked;

            xArtworkSizeUpDown.Enable(resizeArtworkCheckBox.Checked);
            yArtworkSizeUpDown.Enable(resizeArtworkCheckBox.Checked);
        }

        private void resizeArtworkCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!resizeArtworkCheckBox.Enabled)
                return;

            resizeArtworkCheckBox.Checked = !resizeArtworkCheckBox.Checked;
            resizeArtworkCheckBox_CheckedChanged(null, null);
        }

        private void xArtworkSizeUpDown_ValueChanged(object sender, EventArgs e)
        {
            SavedSettings.xArtworkSize = (ushort)xArtworkSizeUpDown.Value;
        }

        private void yArtworkSizeUpDown_ValueChanged(object sender, EventArgs e)
        {
            SavedSettings.yArtworkSize = (ushort)yArtworkSizeUpDown.Value;
        }

        private void sourceFieldComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sourceFieldComboBox.SelectedIndex == -1)
            {
                sourceFieldComboBox.Enable(false);
                labelSaveField.Enable(false);
                labelSaveToTag.Enable(false);
                labelFunctionId.Enable(false);

                destinationTagList.Enable(false);
                idTextBox.Enable(false);
                clearIdButton.Enable(false);

                destinationTagList.SelectedIndex = -1;
                idTextBox.Text = string.Empty;


                operationComboBox.SelectedIndex = 0;
                mulDivFactorComboBox.SelectedIndex = 0;
                precisionDigitsComboBox.SelectedIndex = 0;
                appendTextBox.Text = string.Empty;

                operationComboBox.Enable(false);
                mulDivFactorComboBox.Enable(false);
                precisionDigitsComboBox.Enable(false);
                appendTextBox.Enable(false);
                roundToLabel.Enable(false);
                digitsLabel.Enable(false);
                appendLabel.Enable(false);

                return;
            }


            sourceFieldComboBox.Enable(true);
            labelSaveField.Enable(true);
            labelSaveToTag.Enable(true);
            labelFunctionId.Enable(true);

            destinationTagList.Enable(true);
            idTextBox.Enable(true);
            clearIdButton.Enable(true);

            operationComboBox.Enable(true);
            mulDivFactorComboBox.Enable(true);
            precisionDigitsComboBox.Enable(true);
            appendTextBox.Enable(true);
            roundToLabel.Enable(true);
            digitsLabel.Enable(true);
            appendLabel.Enable(true);

            sourceFieldComboBoxIndexChanging = true;
            destinationTagList.Text = savedDestinationTagsNames[sourceFieldComboBox.SelectedIndex];
            idTextBox.Text = savedFunctionIds[sourceFieldComboBox.SelectedIndex];
            sourceFieldComboBoxIndexChanging = false;


            operationComboBox.SelectedIndex = operations[sourceFieldComboBox.SelectedIndex];
            mulDivFactorComboBox.Text = mulDivFactors[sourceFieldComboBox.SelectedIndex];
            precisionDigitsComboBox.Text = precisionDigits[sourceFieldComboBox.SelectedIndex];
            appendTextBox.Text = appendTexts[sourceFieldComboBox.SelectedIndex];
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
            if (!totalsCheckBox.Enabled)
                return;

            totalsCheckBox.Checked = !totalsCheckBox.Checked;
            totalsCheckBox_CheckedChanged(null, null);
        }

        private void operationComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sourceFieldComboBox.SelectedIndex == -1)
            {
                operationComboBox.SelectedIndex = 0;
            }
            else
            {
                operations[sourceFieldComboBox.SelectedIndex] = operationComboBox.SelectedIndex;

                if (!sourceFieldComboBoxIndexChanging)
                    setPresetChanged();
            }
        }

        public static string TryParseUint(string textValue, uint defaultValue)
        {
            if (textValue.Length == 0)
                return defaultValue.ToString("F0");

            uint value = defaultValue;
            if (!uint.TryParse(textValue, out value))
                return TryParseUint(textValue.Substring(0, textValue.Length - 1), defaultValue);
            else
                return value.ToString("F0");
        }

        private void mulDivFactorComboBox_TextChanged(object sender, EventArgs e)
        {
            if (sourceFieldComboBox.SelectedIndex == -1)
            {
                mulDivFactorComboBox.SelectedIndex = 0;
            }
            else switch (mulDivFactorComboBox.Text)
                {
                    case "":
                        mulDivFactorComboBox.SelectedIndex = 0;
                        mulDivFactors[sourceFieldComboBox.SelectedIndex] = (string)mulDivFactorComboBox.Items[0];

                        break;
                    case "1 (ignore)":
                    case "1 (игнорировать)":
                    case "1000 (K)":
                    case "1000000 (M)":
                    case "1024 (K)":
                    case "1048576 (M)":
                        mulDivFactors[sourceFieldComboBox.SelectedIndex] = mulDivFactorComboBox.Text;

                        break;
                    default:
                        string mulDivFactorText = TryParseUint(mulDivFactorComboBox.Text, 1);
                        if (mulDivFactorComboBox.Text != mulDivFactorText)
                        {
                            mulDivFactorComboBox.Text = mulDivFactorText;
                            mulDivFactorComboBox.SelectionStart = mulDivFactorText.Length;
                            mulDivFactorComboBox.SelectionLength = 0;
                        }

                        mulDivFactors[sourceFieldComboBox.SelectedIndex] = mulDivFactorComboBox.Text;

                        break;
                }

            if (!sourceFieldComboBoxIndexChanging)
                setPresetChanged();
        }

        private void precisionDigitsComboBox_TextChanged(object sender, EventArgs e)
        {
            if (sourceFieldComboBox.SelectedIndex == -1)
            {
                precisionDigitsComboBox.SelectedIndex = 0;
            }
            else switch (precisionDigitsComboBox.Text)
                {
                    case "":
                        precisionDigitsComboBox.SelectedIndex = 0;
                        precisionDigits[sourceFieldComboBox.SelectedIndex] = (string)precisionDigitsComboBox.Items[0];

                        break;
                    case "(don't round)":
                    case "(не округлять)":
                        precisionDigits[sourceFieldComboBox.SelectedIndex] = (string)precisionDigitsComboBox.Items[0];

                        break;
                    default:
                        string precisionDigitsText = TryParseUint(precisionDigitsComboBox.Text, 0);
                        if (precisionDigitsComboBox.Text != precisionDigitsText)
                        {
                            precisionDigitsComboBox.Text = precisionDigitsText;
                            precisionDigitsComboBox.SelectionStart = precisionDigitsText.Length;
                            precisionDigitsComboBox.SelectionLength = 0;
                        }

                        precisionDigits[sourceFieldComboBox.SelectedIndex] = precisionDigitsComboBox.Text;

                        break;
                }

            if (!sourceFieldComboBoxIndexChanging)
                setPresetChanged();
        }

        private void appendTextBox_TextChanged(object sender, EventArgs e)
        {
            if (sourceFieldComboBox.SelectedIndex == -1)
            {
                appendTextBox.Text = string.Empty;
            }
            else
            {
                appendTexts[sourceFieldComboBox.SelectedIndex] = appendTextBox.Text;
            }

            if (!sourceFieldComboBoxIndexChanging)
                setPresetChanged();
        }

        private void useAnotherPresetAsSourceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setPresetChanged();

            if (presetIsLoading)
                return;

            lastSelectedRefCheckStatus = true;
            adjustPresetAsSourceUI(useAnotherPresetAsSourceComboBox, (ReportPresetReference)useAnotherPresetAsSourceComboBox.SelectedItem, lastSelectedRefCheckStatus);
        }

        private void useAnotherPresetAsSourceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            setPresetChanged();
            useAnotherPresetAsSourceComboBox.Enable(useAnotherPresetAsSourceCheckBox.Checked);

            if (presetIsLoading)
                return;

            if (selectedPreset != null)
            {
                bool? selectedRefCheckStatus = lastSelectedRefCheckStatus;
                if (selectedPreset.useAnotherPresetAsSource && selectedPreset.anotherPresetAsSource.permanentGuid == Guid.Empty)
                    selectedRefCheckStatus = null;

                adjustPresetAsSourceUI(useAnotherPresetAsSourceComboBox, selectedPreset.anotherPresetAsSource, selectedRefCheckStatus);
            }
        }

        private void useAnotherPresetAsSourceCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!useAnotherPresetAsSourceCheckBox.Enabled)
                return;

            useAnotherPresetAsSourceCheckBox.Checked = !useAnotherPresetAsSourceCheckBox.Checked;
            useAnotherPresetAsSourceCheckBox_CheckedChanged(null, null);
        }

        private void previewTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && ModifierKeys == Keys.Control)
            {
                comparedFieldList.Text = previewTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                comparedFieldList_TextUpdate(null, null);
            }
        }

        private void previewTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex != artworkField && ModifierKeys == Keys.Control)
            {
                conditionFieldList.SelectedIndex = e.ColumnIndex;
                comparedFieldList.Text = previewTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                comparedFieldList_TextUpdate(null, null);
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
            if (!smartOperationCheckBox.Enabled)
                return;

            smartOperationCheckBox.Checked = !smartOperationCheckBox.Checked;
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            PluginQuickSettings settings = new PluginQuickSettings(TagToolsPlugin);
            Display(settings, true);
            presetList_ItemCheck(null, null); //Let's refresh auto-apply warning
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (!openReportCheckBox.Enabled)
                return;

            openReportCheckBox.Checked = !openReportCheckBox.Checked;
        }

        private void openReportCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!openReportCheckBox.Enabled)
                return;

            openReportCheckBox.Checked = !openReportCheckBox.Checked;
        }

        private void formatComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedPreset != null && !presetIsLoading)
            {
                selectedPreset.fileFormatIndex = (LrReportFormat)formatComboBox.SelectedIndex + 1;
                setPresetChanged();
            }
        }
    }

    public abstract class ExportedDocument
    {
        protected Plugin tagToolsPlugin;
        protected System.IO.FileStream stream;
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

        public ExportedDocument(System.IO.FileStream newStream, Plugin tagToolsPluginParam)
        {
            tagToolsPlugin = tagToolsPluginParam;
            stream = newStream;

            //Write UTF-8 encoding mark
            buffer = unicode.GetPreamble();
            stream.Write(buffer, 0, buffer.Length);
        }

        public string getImageName(string hash)
        {
            hash = Regex.Replace(hash, @"\\", "_");
            hash = Regex.Replace(hash, @"/", "_");
            hash = Regex.Replace(hash, @"\:", "_");

            return hash;
        }

        public virtual void writeHeader()
        {
            getHeader();
            buffer = unicode.GetBytes(text);
            text = string.Empty;
            stream.Write(buffer, 0, buffer.Length);
        }

        public void writeFooter()
        {
            getFooter();
            buffer = unicode.GetBytes(text);
            text = string.Empty;
            stream.Write(buffer, 0, buffer.Length);
        }

        public abstract void addCellToRow(string cell, string cellName, bool rightAlign, bool dontOutput1, bool dontOutput2);
        public abstract void addCellToRow(Bitmap cell, string cellName, string imageHash, int width, int height);

        protected abstract void getRow(int height);

        public virtual void writeRow(int height)
        {
            getRow(height);
            buffer = unicode.GetBytes(text);
            text = string.Empty;
            stream.Write(buffer, 0, buffer.Length);
        }

        public void close()
        {
            stream.Close();
        }

        public virtual void beginAlbumArtist(string albumArtist, int columnsCount)
        {
            return;
        }

        public virtual void beginAlbum(string album, Bitmap artwork, string imageHash, int albumTrackCount)
        {
            return;
        }
    }

    public class TextDocument : ExportedDocument
    {
        public TextDocument(System.IO.FileStream newStream, Plugin tagToolsPluginParam)
            : base(newStream, tagToolsPluginParam)
        {
        }

        public override void addCellToRow(string cell, string cellName, bool rightAlign, bool dontOutput1, bool dontOutput2)
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

        public override void addCellToRow(Bitmap cell, string cellName, string imageHash, int width, int height)
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

    public class CsvDocument : TextDocument
    {
        public CsvDocument(System.IO.FileStream newStream, Plugin tagToolsPluginParam)
            : base(newStream, tagToolsPluginParam)
        {
        }

        private string delimiter = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;

        public override void addCellToRow(string cell, string cellName, bool rightAlign, bool dontOutput1, bool dontOutput2)
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

        public override void addCellToRow(Bitmap cell, string cellName, string imageHash, int width, int height)
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

    public class M3UDocument : TextDocument
    {
        public M3UDocument(System.IO.FileStream newStream, Plugin tagToolsPluginParam)
            : base(newStream, tagToolsPluginParam)
        {
        }

        public override void addCellToRow(string cell, string cellName, bool rightAlign, bool dontOutput1, bool dontOutput2)
        {
            if (cellName == UrlTagName)
            {
                base.addCellToRow(cell, cellName, rightAlign, dontOutput1, dontOutput2);
            }
        }
    }

    public class HtmlTable : ExportedDocument
    {
        protected string fileDirectoryPath;
        protected string imagesDirectoryName;
        protected const string defaultImageName = "Missing Artwork.png";
        protected bool defaultImageWasExported = false;

        public HtmlTable(System.IO.FileStream newStream, Plugin tagToolsPluginParam, string fileDirectoryPathParam, string imagesDirectoryNameParam)
            : base(newStream, tagToolsPluginParam)
        {
            fileDirectoryPath = fileDirectoryPathParam;
            imagesDirectoryName = imagesDirectoryNameParam;
        }

        protected override void getHeader()
        {
            text = "<html>\r\n<head>\r\n</head>\r\n<body>\r\n<table border=1>\r\n";
        }

        protected override void getFooter()
        {
            text = "</table>\r\n</body>\r\n</html>\r\n";
        }

        public override void addCellToRow(string cell, string cellName, bool rightAlign, bool dontOutput1, bool dontOutput2)
        {
            text += "\t<td" + (rightAlign ? " align='right'" : string.Empty) + ">" + cell + "</td>\r\n";
        }

        public override void addCellToRow(Bitmap cell, string cellName, string imageHash, int width, int height)
        {
            string imageName = getImageName(imageHash) + ".jpg";
            cell.Save(fileDirectoryPath + imagesDirectoryName + @"\" + imageName, System.Drawing.Imaging.ImageFormat.Jpeg);

            text += "\t<td height=" + height + " width=" + width + "> <img src=\"" + imagesDirectoryName + @"\" + imageName + "\" > </td>\r\n";
        }

        protected override void getRow(int height)
        {
            text = "<tr>\r\n" + text + " </tr>\r\n";
        }
    }

    public class HtmlDocument : HtmlTable
    {
        protected bool? alternateRow = null;

        public HtmlDocument(System.IO.FileStream newStream, Plugin tagToolsPluginParam, string fileDirectoryPathParam, string imagesDirectoryNameParam)
            : base(newStream, tagToolsPluginParam, fileDirectoryPathParam, imagesDirectoryNameParam)
        {
        }

        public override void writeHeader()
        {
            System.IO.FileStream stylesheet = new System.IO.FileStream(fileDirectoryPath + imagesDirectoryName + @"\stylesheet.css", System.IO.FileMode.Create);

            //Write UTF-8 encoding mark
            buffer = unicode.GetPreamble();
            stylesheet.Write(buffer, 0, buffer.Length);

            buffer = unicode.GetBytes("td {color:#050505;font-size:11.0pt;font-weight:400;font-style:normal;font-family:Arial, sans-serif;white-space:nowrap;}");
            stylesheet.Write(buffer, 0, buffer.Length);
            buffer = unicode.GetBytes(".xl1 {color:#0070C0;font-size:16.0pt;font-weight:700;font-family:Arial, sans-serif;	}");
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
            text = "<html>\r\n<head>\r\n<link rel=Stylesheet href=\"" + imagesDirectoryName + "\\stylesheet.css\">"
                + "\r\n </head>\r\n<body>\r\n" +
                "<table> <tr> <td class=xl1>" + LibraryReportsCommand.PresetName + "</td> </tr> </table> <table style='border-collapse:collapse'>"
                + "\r\n";
        }

        public override void addCellToRow(string cell, string cellName, bool rightAlign, bool dontOutput1, bool dontOutput2)
        {
            string rowClass;

            if (alternateRow == null)
                rowClass = "xl0";
            else if ((bool)alternateRow)
                rowClass = "xl2";
            else
                rowClass = "xl3";


            text += "\t<td class=" + rowClass + (rightAlign ? " align='right'" : string.Empty) + ">" + cell + "</td>\r\n";
        }

        public override void addCellToRow(Bitmap cell, string cellName, string imageHash, int width, int height)
        {
            string rowClass;

            if (alternateRow == null)
                rowClass = "xl0";
            else if ((bool)alternateRow)
                rowClass = "xl2";
            else
                rowClass = "xl3";

            string imageName = getImageName(imageHash) + ".jpg";
            cell.Save(fileDirectoryPath + imagesDirectoryName + @"\" + imageName, System.Drawing.Imaging.ImageFormat.Jpeg);

            text += "\t<td class=" + rowClass + " height=" + height + " width=" + width + "> <img src=\"" + imagesDirectoryName + @"\" + imageName + "\" > </td>\r\n";
        }

        public override void writeRow(int height)
        {
            if (alternateRow == null)
                alternateRow = false;
            else
                alternateRow = !alternateRow;

            base.writeRow(height);
        }
    }

    public class HtmlDocumentByAlbum : HtmlDocument
    {
        public HtmlDocumentByAlbum(System.IO.FileStream newStream, Plugin tagToolsPluginParam, string fileDirectoryPathParam, string imagesDirectoryNameParam)
            : base(newStream, tagToolsPluginParam, fileDirectoryPathParam, imagesDirectoryNameParam)
        {
        }

        public override void beginAlbumArtist(string albumArtist, int columnsCount)
        {
            text = "\t<tr> <td colspan=" + columnsCount + " class=xl5>" + albumArtist + "</td> </tr>\r\n";
        }

        public override void beginAlbum(string album, Bitmap artwork, string imageHash, int albumTrackCount)
        {
            string imageName = getImageName(imageHash) + ".jpg";
            artwork.Save(fileDirectoryPath + imagesDirectoryName + @"\" + imageName, System.Drawing.Imaging.ImageFormat.Jpeg);

            text += "\t<td rowspan=" + albumTrackCount + " class=xl6> <img src=\"" + imagesDirectoryName + @"\" + imageName + "\" > <br>" + album + "</td>\r\n";
            //text += "\t<td class=xl6> <img src=\"" + imagesDirectoryName + @"\" + imageName + "\" > <br>" + album + "</td>\r\n";
        }

        public override void addCellToRow(string cell, string cellName, bool rightAlign, bool dontOutput1, bool dontOutput2)
        {
            if (dontOutput1 || dontOutput2)
                return;

            base.addCellToRow(cell, cellName, rightAlign, dontOutput1, dontOutput2);
        }

        public override void addCellToRow(Bitmap cell, string cellName, string imageHash, int width, int height)
        {
        }
    }

    public class HtmlDocumentCDBooklet : HtmlTable
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

        public HtmlDocumentCDBooklet(System.IO.FileStream newStream, Plugin tagToolsPluginParam, string fileDirectoryPathParam, string imagesDirectoryNameParam)
            : base(newStream, tagToolsPluginParam, fileDirectoryPathParam, imagesDirectoryNameParam)
        {
        }

        public void writeHeader(int size, Color bitmapAverageColor, Bitmap scaledPic, SortedDictionary<string, List<string>> albumArtistsAlbums, int trackCount)
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
                albumArtistsFontSize = albumArtistsFontSize / (albumArtistsAlbums.Count - 4);

            string albumArtistsFontSizeString = albumArtistsFontSize.ToString().Replace(',', '.');

            if (albumCount > 5)
                albumFontSize = albumFontSize / (albumCount - 4);

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
            text = "<html>\r\n<head>\r\n"
                + "\t<link rel=Stylesheet href='" + imagesDirectoryName + "/stylesheet.css'>"
                + "\r\n</head>\r\n"
                + "<body>\r\n<table><tr>\r\n"
                + "<td class=xl0 width=" + backgroundSize + " height=" + backgroundSize + ">"
                + "\r\n<table>\r\n";
        }

        protected void getFooter(SortedDictionary<string, List<string>> albumArtistsAlbums)
        {
            text = "</table>\r\n</td>\r\n<td width=" + backgroundSize + " height=" + backgroundSize
                + " background='" + imagesDirectoryName + "/bg1.jpg' style='background-repeat:no-repeat;background-position:center center;'><table>\r\n";

            foreach (KeyValuePair<string, List<string>> albumArtist in albumArtistsAlbums)
            {
                text += "\t<tr><td class=xl3 width=" + backgroundSize + ">" + albumArtist.Key + "</td></tr>\r\n";
                foreach (string album in albumArtist.Value)
                {
                    text += "\t\t<tr><td class=xl4 width=" + backgroundSize + ">" + album + "</td></tr>\r\n";
                }
            }

            text += "</table></td>\r\n</tr></table>\r\n</body>"
                + "\r\n</html>\r\n";
        }

        public void writeFooter(SortedDictionary<string, List<string>> albumArtistsAlbums)
        {
            getFooter(albumArtistsAlbums);
            buffer = unicode.GetBytes(text);
            text = string.Empty;
            stream.Write(buffer, 0, buffer.Length);
        }

        public void addTrack(int seqNum, string albumArtist, string album, string title, string duration)
        {
            text = "\t<td class=xl1>" + seqNum.ToString("D2") + ".";

            if (albumArtist != null)
                text += " " + albumArtist + " &#x25C6;";

            if (album != null)
                text += " " + album + " &#x25C6;";

            text += " " + title + "</td>";

            text += "<td class=xl2>" + duration + "</td>\r\n";
        }
    }

    public partial class Plugin
    {
        public void LRPreset1EventHandler(object sender, EventArgs e)
        {
            LibraryReportsCommand.ApplyReportPresetByHotkey(1);
        }

        public void LRPreset2EventHandler(object sender, EventArgs e)
        {
            LibraryReportsCommand.ApplyReportPresetByHotkey(2);
        }

        public void LRPreset3EventHandler(object sender, EventArgs e)
        {
            LibraryReportsCommand.ApplyReportPresetByHotkey(3);
        }

        public void LRPreset4EventHandler(object sender, EventArgs e)
        {
            LibraryReportsCommand.ApplyReportPresetByHotkey(4);
        }

        public void LRPreset5EventHandler(object sender, EventArgs e)
        {
            LibraryReportsCommand.ApplyReportPresetByHotkey(5);
        }

        public void LRPreset6EventHandler(object sender, EventArgs e)
        {
            LibraryReportsCommand.ApplyReportPresetByHotkey(6);
        }

        public void LRPreset7EventHandler(object sender, EventArgs e)
        {
            LibraryReportsCommand.ApplyReportPresetByHotkey(7);
        }

        public void LRPreset8EventHandler(object sender, EventArgs e)
        {
            LibraryReportsCommand.ApplyReportPresetByHotkey(8);
        }

        public void LRPreset9EventHandler(object sender, EventArgs e)
        {
            LibraryReportsCommand.ApplyReportPresetByHotkey(9);
        }

        public void LRPreset10EventHandler(object sender, EventArgs e)
        {
            LibraryReportsCommand.ApplyReportPresetByHotkey(10);
        }

        public void LRPreset11EventHandler(object sender, EventArgs e)
        {
            LibraryReportsCommand.ApplyReportPresetByHotkey(11);
        }

        public void LRPreset12EventHandler(object sender, EventArgs e)
        {
            LibraryReportsCommand.ApplyReportPresetByHotkey(12);
        }

        public void LRPreset13EventHandler(object sender, EventArgs e)
        {
            LibraryReportsCommand.ApplyReportPresetByHotkey(13);
        }

        public void LRPreset14EventHandler(object sender, EventArgs e)
        {
            LibraryReportsCommand.ApplyReportPresetByHotkey(14);
        }

        public void LRPreset15EventHandler(object sender, EventArgs e)
        {
            LibraryReportsCommand.ApplyReportPresetByHotkey(15);
        }

        public void LRPreset16EventHandler(object sender, EventArgs e)
        {
            LibraryReportsCommand.ApplyReportPresetByHotkey(16);
        }

        public void LRPreset17EventHandler(object sender, EventArgs e)
        {
            LibraryReportsCommand.ApplyReportPresetByHotkey(17);
        }

        public void LRPreset18EventHandler(object sender, EventArgs e)
        {
            LibraryReportsCommand.ApplyReportPresetByHotkey(18);
        }

        public void LRPreset19EventHandler(object sender, EventArgs e)
        {
            LibraryReportsCommand.ApplyReportPresetByHotkey(19);
        }

        public void LRPreset20EventHandler(object sender, EventArgs e)
        {
            LibraryReportsCommand.ApplyReportPresetByHotkey(20);
        }
    }
}
