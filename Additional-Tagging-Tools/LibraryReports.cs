using MusicBeePlugin.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class LibraryReportsCommand : PluginWindowTemplate
    {
        public class ReportPreset
        {
            public bool autoApply = false;
            public string name = null;
            public bool userPreset = true;
            public Guid guid = Guid.NewGuid();
            public Guid permanentGuid = Guid.NewGuid();

            public bool hotkeyAssigned = false;
            public bool applyToSelectedTracks = true;
            public int hotkeySlot = -1; //0..Plugin.MaximumNumberOfLRHotkeys - 1

            public string[] groupingNames = new string[0];
            public string[] functionNames = new string[0];
            public FunctionType[] functionTypes = new FunctionType[0];
            public string[] parameterNames = new string[0];
            public string[] parameter2Names = new string[0];
            public bool totals;

            public string[] sourceTags = new string[0];
            public string[] destinationTags = new string[0];
            public string[] functionIds = new string[0];

            public int[] operations = new int[0];
            public string[] mulDivFactors = new string[0];
            public string[] precisionDigits = new string[0];
            public string[] appendTexts = new string[0];

            public bool conditionIsChecked = false;
            public string conditionField = null;
            public string conditionText = null;
            public string comparedField = null;

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
                    hotkeyAssigned = sourcePreset.hotkeyAssigned;
                    hotkeySlot = sourcePreset.hotkeySlot; //0..Plugin.MaximumNumberOfLRHotkeys - 1
                    functionIds = (string[])sourcePreset.functionIds.Clone();
                }
                else
                {
                    functionIds = new string[sourcePreset.functionIds.Length];
                }

                permanentGuid = sourcePreset.permanentGuid;

                applyToSelectedTracks = sourcePreset.applyToSelectedTracks;

                groupingNames = (string[])sourcePreset.groupingNames.Clone();
                functionNames = (string[])sourcePreset.functionNames.Clone();
                functionTypes = (FunctionType[])sourcePreset.functionTypes.Clone();
                parameterNames = (string[])sourcePreset.parameterNames.Clone();
                parameter2Names = (string[])sourcePreset.parameter2Names.Clone();
                totals = sourcePreset.totals;

                sourceTags = (string[])sourcePreset.sourceTags.Clone();
                destinationTags = (string[])sourcePreset.destinationTags.Clone();

                operations = (int[])sourcePreset.operations.Clone();
                mulDivFactors = (string[])sourcePreset.mulDivFactors.Clone();
                precisionDigits = (string[])sourcePreset.precisionDigits.Clone();
                appendTexts = (string[])sourcePreset.appendTexts.Clone();

                conditionIsChecked = sourcePreset.conditionIsChecked;
                conditionField = sourcePreset.conditionField;
                conditionText = sourcePreset.conditionText;
                comparedField = sourcePreset.comparedField;

                exportedTrackListName = sourcePreset.exportedTrackListName;
            }

            public string getHotkeyChar()
            {
                if (!hotkeyAssigned)
                    return "";
                else if (applyToSelectedTracks)
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
                return Plugin.ReportPresetHotkeyDescription + ": " + ToString();
            }

            public override string ToString()
            {
                return getName() + getHotkeyPostfix();
            }

            public string getName()
            {
                if (!string.IsNullOrWhiteSpace(name))
                    return name;

                string representation = "";

                if (groupingNames.Length > 0)
                    representation = groupingNames[0];

                for (int i = 1; i < groupingNames.Length; i++)
                {
                    representation += ", " + groupingNames[i];
                }


                if (representation == "" && functionNames.Length > 0)
                    representation = functionNames[0];
                else if (functionNames.Length > 0)
                    representation += ", " + functionNames[0];

                for (int i = 1; i < functionNames.Length; i++)
                {
                    representation += ", " + functionNames[i];
                }

                if (representation == "")
                    representation = Plugin.EmptyPresetName;


                return representation;
            }
        }

        public class AggregatedTags : SortedDictionary<string, Plugin.ConvertStringsResult[]>
        {
            public static string GetComposedGroupingValues(string[] groupingValues)
            {
                string composedGroupings;

                if (groupingValues.Length == 0)
                {
                    composedGroupings = "";
                }
                else
                {
                    composedGroupings = string.Join(Plugin.MultipleItemsSplitterId.ToString(), groupingValues);
                }

                return composedGroupings;
            }

            public void add(string url, string[] groupingValues, string[] functionValues, List<FunctionType> functionTypes, string[] parameter2Values, ref int[] resultTypes, bool totals)
            {
                string composedGroupings;
                Plugin.ConvertStringsResult[] aggregatedValues;


                if (groupingValues.Length == 0)
                {
                    composedGroupings = "";
                }
                else
                {
                    composedGroupings = GetComposedGroupingValues(groupingValues);

                    if (totals)
                        for (int i = groupingValues.Length - 1; i >= 0; i--)
                        {
                            groupingValues[i] = Plugin.TotalsString;
                            add(null, groupingValues, functionValues, functionTypes, parameter2Values, ref resultTypes, false);
                        }
                }


                if (functionValues == null)
                {
                    if (!TryGetValue(composedGroupings, out _))
                        Add(composedGroupings, null);
                }
                else
                {
                    if (!TryGetValue(composedGroupings, out aggregatedValues))
                    {
                        aggregatedValues = new Plugin.ConvertStringsResult[functionValues.Length + 1];

                        for (int i = 0; i < functionValues.Length; i++)
                        {
                            aggregatedValues[i] = new Plugin.ConvertStringsResult(1);

                            if (functionTypes[i] == FunctionType.Minimum)
                            {
                                aggregatedValues[i].result1f = double.MaxValue;
                                aggregatedValues[i].result1s = "口";
                            }
                            else if (functionTypes[i] == FunctionType.Maximum)
                            {
                                aggregatedValues[i].result1f = double.MinValue;
                                aggregatedValues[i].result1s = "";
                            }
                        }

                        aggregatedValues[functionValues.Length] = new Plugin.ConvertStringsResult(1);

                        Add(composedGroupings, aggregatedValues);
                    }

                    Plugin.ConvertStringsResult currentFunctionValue;

                    for (int i = 0; i < functionValues.Length + 1; i++)
                    {
                        if (i == functionValues.Length) //It are URLs
                        {
                            if (url != null)
                            {
                                if (!aggregatedValues[i].items.TryGetValue(url, out _))
                                    aggregatedValues[i].items.Add(url, false);

                                aggregatedValues[i].type = 1;
                            }
                        }
                        else if (functionTypes[i] == FunctionType.Count)
                        {
                            if (!aggregatedValues[i].items.TryGetValue(functionValues[i], out _))
                                aggregatedValues[i].items.Add(functionValues[i], false);

                            aggregatedValues[i].type = 1;
                        }
                        else if (functionTypes[i] == FunctionType.Sum)
                        {
                            currentFunctionValue = Plugin.ConvertStrings(functionValues[i]);

                            if (currentFunctionValue.type != 0)
                            {
                                aggregatedValues[i].result1f += currentFunctionValue.result1f;

                                if (aggregatedValues[i].result1fPrefix == null)
                                    aggregatedValues[i].result1fPrefix = currentFunctionValue.result1fPrefix;
                                else if (aggregatedValues[i].result1fPrefix != currentFunctionValue.result1fPrefix)
                                    aggregatedValues[i].result1fPrefix = "";

                                if (aggregatedValues[i].result1fSpace == null)
                                    aggregatedValues[i].result1fSpace = currentFunctionValue.result1fSpace;
                                else if (aggregatedValues[i].result1fSpace != currentFunctionValue.result1fSpace)
                                    aggregatedValues[i].result1fSpace = "";

                                if (aggregatedValues[i].result1fPostfix == null)
                                    aggregatedValues[i].result1fPostfix = currentFunctionValue.result1fPostfix;
                                else if (aggregatedValues[i].result1fPostfix != currentFunctionValue.result1fPostfix)
                                    aggregatedValues[i].result1fPostfix = "";

                                aggregatedValues[i].type = currentFunctionValue.type;
                            }
                        }
                        else if (functionTypes[i] == FunctionType.Minimum)
                        {
                            currentFunctionValue = Plugin.ConvertStrings(functionValues[i]);

                            if (currentFunctionValue.type == 0)
                            {
                                resultTypes[i] = 0;
                            }
                            else if (resultTypes[i] == 0)
                            {
                                currentFunctionValue.type = 0;
                            }
                            else if (currentFunctionValue.type > resultTypes[i])
                            {
                                resultTypes[i] = currentFunctionValue.type;
                            }
                            else
                            {
                                currentFunctionValue.type = resultTypes[i];
                            }

                            if (currentFunctionValue.type != 0)
                            {
                                if (aggregatedValues[i].result1f > currentFunctionValue.result1f)
                                    aggregatedValues[i].result1f = currentFunctionValue.result1f;

                                if (aggregatedValues[i].result1fPrefix == null)
                                    aggregatedValues[i].result1fPrefix = currentFunctionValue.result1fPrefix;
                                else if (aggregatedValues[i].result1fPrefix != currentFunctionValue.result1fPrefix)
                                    aggregatedValues[i].result1fPrefix = "";

                                if (aggregatedValues[i].result1fSpace == null)
                                    aggregatedValues[i].result1fSpace = currentFunctionValue.result1fSpace;
                                else if (aggregatedValues[i].result1fSpace != currentFunctionValue.result1fSpace)
                                    aggregatedValues[i].result1fSpace = "";

                                if (aggregatedValues[i].result1fPostfix == null)
                                    aggregatedValues[i].result1fPostfix = currentFunctionValue.result1fPostfix;
                                else if (aggregatedValues[i].result1fPostfix != currentFunctionValue.result1fPostfix)
                                    aggregatedValues[i].result1fPostfix = "";
                            }
                            else
                            {
                                if (Plugin.CompareStrings(aggregatedValues[i].result1s, currentFunctionValue.result1s) == 1)
                                    aggregatedValues[i].result1s = currentFunctionValue.result1s;
                            }

                            aggregatedValues[i].type = currentFunctionValue.type;
                        }
                        else if (functionTypes[i] == FunctionType.Maximum)
                        {
                            currentFunctionValue = Plugin.ConvertStrings(functionValues[i]);

                            if (currentFunctionValue.type == 0)
                            {
                                resultTypes[i] = 0;
                            }
                            else if (resultTypes[i] == 0)
                            {
                                currentFunctionValue.type = 0;
                            }
                            else if (currentFunctionValue.type < resultTypes[i])
                            {
                                resultTypes[i] = currentFunctionValue.type;
                            }
                            else
                            {
                                currentFunctionValue.type = resultTypes[i];
                            }

                            if (currentFunctionValue.type != 0)
                            {
                                if (aggregatedValues[i].result1f < currentFunctionValue.result1f)
                                    aggregatedValues[i].result1f = currentFunctionValue.result1f;

                                if (aggregatedValues[i].result1fPrefix == null)
                                    aggregatedValues[i].result1fPrefix = currentFunctionValue.result1fPrefix;
                                else if (aggregatedValues[i].result1fPrefix != currentFunctionValue.result1fPrefix)
                                    aggregatedValues[i].result1fPrefix = "";

                                if (aggregatedValues[i].result1fSpace == null)
                                    aggregatedValues[i].result1fSpace = currentFunctionValue.result1fSpace;
                                else if (aggregatedValues[i].result1fSpace != currentFunctionValue.result1fSpace)
                                    aggregatedValues[i].result1fSpace = "";

                                if (aggregatedValues[i].result1fPostfix == null)
                                    aggregatedValues[i].result1fPostfix = currentFunctionValue.result1fPostfix;
                                else if (aggregatedValues[i].result1fPostfix != currentFunctionValue.result1fPostfix)
                                    aggregatedValues[i].result1fPostfix = "";
                            }
                            else
                            {
                                if (Plugin.CompareStrings(aggregatedValues[i].result1s, currentFunctionValue.result1s) == -1)
                                    aggregatedValues[i].result1s = currentFunctionValue.result1s;
                            }

                            aggregatedValues[i].type = currentFunctionValue.type;
                        }
                        else if (functionTypes[i] == FunctionType.Average)
                        {
                            if (!aggregatedValues[i].items.TryGetValue(parameter2Values[i], out _))
                                aggregatedValues[i].items.Add(parameter2Values[i], false);

                            currentFunctionValue = Plugin.ConvertStrings(functionValues[i]);

                            if (currentFunctionValue.type != 0)
                            {
                                aggregatedValues[i].result1f += currentFunctionValue.result1f;

                                if (aggregatedValues[i].result1fPrefix == null)
                                    aggregatedValues[i].result1fPrefix = currentFunctionValue.result1fPrefix;
                                else if (aggregatedValues[i].result1fPrefix != currentFunctionValue.result1fPrefix)
                                    aggregatedValues[i].result1fPrefix = "";

                                if (aggregatedValues[i].result1fSpace == null)
                                    aggregatedValues[i].result1fSpace = currentFunctionValue.result1fSpace;
                                else if (aggregatedValues[i].result1fSpace != currentFunctionValue.result1fSpace)
                                    aggregatedValues[i].result1fSpace = "";

                                if (aggregatedValues[i].result1fPostfix == null)
                                    aggregatedValues[i].result1fPostfix = currentFunctionValue.result1fPostfix;
                                else if (aggregatedValues[i].result1fPostfix != currentFunctionValue.result1fPostfix)
                                    aggregatedValues[i].result1fPostfix = "";

                                aggregatedValues[i].type = currentFunctionValue.type;
                            }
                        }
                        else if (functionTypes[i] == FunctionType.AverageCount)
                        {
                            if (!aggregatedValues[i].items.TryGetValue(parameter2Values[i], out _))
                                aggregatedValues[i].items.Add(parameter2Values[i], false);

                            if (!aggregatedValues[i].items1.TryGetValue(functionValues[i], out _))
                                aggregatedValues[i].items1.Add(functionValues[i], false);
                        }
                    }
                }
            }

            public static string GetField(string composedGroupings, Plugin.ConvertStringsResult[] convertResults, int fieldNumber, List<string> groupingNames, int operation, string mulDivFactorRepr, string precisionDigitsRepr, string appendedText)
            {
                if (fieldNumber < groupingNames.Count)
                {
                    string field = composedGroupings.Split(Plugin.MultipleItemsSplitterId)[fieldNumber];

                    if (field == Plugin.TotalsString)
                        field = (Plugin.MsgAllTags + " \"" + groupingNames[fieldNumber] + "\"").ToUpper();

                    return field;
                }
                else
                {
                    return convertResults[fieldNumber - groupingNames.Count].getFormattedResult(operation, mulDivFactorRepr, precisionDigitsRepr, appendedText);
                }
            }

            public static string[] GetGroupings(KeyValuePair<string, Plugin.ConvertStringsResult[]> keyValue, List<string> groupingNames)
            {
                if (keyValue.Key == "")
                {
                    return new string[0];
                }
                else
                {
                    string[] fields = keyValue.Key.Split(Plugin.MultipleItemsSplitterId);

                    for (int i = 0; i < fields.Length; i++)
                        if (fields[i] == Plugin.TotalsString)
                            fields[i] = (Plugin.MsgAllTags + " '" + groupingNames[i] + "'").ToUpper();

                    return fields;
                }
            }
        }

        private delegate void AddRowToTable(List<string[]> rows);
        private delegate void UpdateTable();
        private AddRowToTable addRowToTable;
        private UpdateTable updateTable;

        private System.Threading.Timer periodicCacheClearingTimer;
        private void periodicCacheClearing(object state)
        {
            cachedAppliedPresetGuid = Guid.NewGuid();
            cachedQueriedActualGroupingValues = null;
            cachedQueriedFilesActualComposedGroupingValues.Clear();
            tags.Clear();

            foreach (var artPair in artworks)
            {
                artPair.Value.Dispose();
            }
            artworks.Clear();
        }

        private static List<string> ProccessedReportDeletions = new List<string>();

        //Cached UI and events workrounds
        public static Bitmap DefaultArtwork;
        public static string DefaultArtworkHash;
        public SortedDictionary<string, Bitmap> artworks = new SortedDictionary<string, Bitmap>();
        private Bitmap artwork; //For CD booklet export

        private string autoApplyText;
        private string nowTickedText;

        private bool ignoreCheckedPresetEvent = true;
        private int autoAppliedPresetCount;

        private string assignHotkeyCheckBoxText;
        private int reportPresetsWithHotkeysCount;
        private bool[] reportPresetHotkeyUsedSlots = new bool[Plugin.MaximumNumberOfLRHotkeys];

        private bool unsavedChanges = false;
        private string buttonCloseToolTip;
        private int presetsBoxLastSelectedIndex = -2;

        private bool ignoreSplitterMovedEvent = true;

        private bool presetIsLoaded = false;
        private bool ignorePresetChangedEvent = false;
        private bool completelyIgnoreItemCheckEvent = false;
        private bool completelyIgnoreFunctionChangedEvent = false;
        private bool sourceFieldComboBoxIndexChanging = false;
        private static bool BackgroundTaskIsInProgress = false;

        //Working locals
        public ReportPreset appliedPreset;
        private Guid cachedAppliedPresetGuid;


        private SortedDictionary<string, bool>[] cachedQueriedActualGroupingValues = null;
        private SortedDictionary<string, string> cachedQueriedFilesActualComposedGroupingValues = new SortedDictionary<string, string>(); //<url, composed groupings>

        private AggregatedTags tags = new AggregatedTags();

        private string conditionText;
        private string comparedFieldText;

        private int conditionField = -1;
        private int comparedField = -1;
        private int artworkField = -1;

        private readonly List<Plugin.MetaDataType> destinationTagIds = new List<Plugin.MetaDataType>();

        public static string PresetName = "";

        //Working locals & UI preset caching
        private readonly List<string> groupingNames = new List<string>();
        private readonly List<string> functionNames = new List<string>();
        private readonly List<FunctionType> functionTypes = new List<FunctionType>();
        private readonly List<string> parameterNames = new List<string>();
        private readonly List<string> parameter2Names = new List<string>();

        private readonly List<string> savedFunctionIds = new List<string>();

        private List<int> operations = new List<int>();
        private List<string> mulDivFactors = new List<string>();
        private List<string> precisionDigits = new List<string>();
        private List<string> appendTexts = new List<string>();

        //UI preset caching
        private readonly List<string> savedDestinationTagsNames = new List<string>();

        public new void Dispose()
        {
            periodicCacheClearingTimer?.Dispose();
            periodicCacheClearing(null);
        }

        public LibraryReportsCommand()
        {
            //It's not GUI control, not a Form (if constructed without parameters)
            //InitializeComponent();
        }

        public LibraryReportsCommand(bool setTimer)
        {
            //It's not GUI control, not a Form (if constructed without parameters)
            //InitializeComponent();

            periodicCacheClearingTimer = new System.Threading.Timer(periodicCacheClearing, null, 5 * 60 * 1000, 5 * 60 * 1000); //Every 5 mins.
        }

        public LibraryReportsCommand(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;

            initializeForm();
        }

        private void resetLocalsAndUiControls()
        {
            cachedAppliedPresetGuid = Guid.NewGuid();

            completelyIgnoreItemCheckEvent = true;

            while (previewTable.Columns.Count > 0)
                previewTable.Columns.RemoveAt(0);

            for (int i = 0; i < sourceTagList.Items.Count; i++)
            {
                sourceTagList.SetItemChecked(i, false);
            }

            conditionField = -1;
            comparedField = -1;
            artworkField = -1;

            sourceFieldComboBox.Items.Clear();
            conditionFieldList.Items.Clear();
            comparedFieldList.Items.Clear();

            groupingNames.Clear();
            functionNames.Clear();
            functionTypes.Clear();
            parameterNames.Clear();
            parameter2Names.Clear();

            operations.Clear();
            mulDivFactors.Clear();
            precisionDigits.Clear();
            appendTexts.Clear();

            completelyIgnoreItemCheckEvent = false;

            functionComboBox.SelectedIndex = 0;

            clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, buttonPreview, buttonExport, buttonClose);
        }

        private void prepareLocals()
        {
            if (cachedAppliedPresetGuid == appliedPreset.guid)
                return;

            groupingNames.Clear();
            groupingNames.AddRange(appliedPreset.groupingNames);
            functionNames.Clear();
            functionNames.AddRange(appliedPreset.functionNames);
            savedFunctionIds.Clear();
            savedFunctionIds.AddRange(appliedPreset.functionIds);
            functionTypes.Clear();
            functionTypes.AddRange(appliedPreset.functionTypes);
            parameterNames.Clear();
            parameterNames.AddRange(appliedPreset.parameterNames);
            parameter2Names.Clear();
            parameter2Names.AddRange(appliedPreset.parameter2Names);


            operations.Clear();
            operations.AddRange(appliedPreset.operations);
            mulDivFactors.Clear();
            mulDivFactors.AddRange(appliedPreset.mulDivFactors);
            precisionDigits.Clear();
            precisionDigits.AddRange(appliedPreset.precisionDigits);
            appendTexts.Clear();
            appendTexts.AddRange(appliedPreset.appendTexts);


            destinationTagIds.Clear();
            for (int i = 0; i < appliedPreset.destinationTags.Length; i++)
            {
                destinationTagIds.Add(Plugin.GetTagId(appliedPreset.destinationTags[i]));
            }

            conditionText = appliedPreset.conditionText;
            comparedFieldText = appliedPreset.comparedField;

            conditionField = -1;
            comparedField = -1;

            artworks.Clear();
            artworkField = -1;


            for (int i = 0; i < groupingNames.Count; i++)
            {
                if (groupingNames[i] == Plugin.ArtworkName)
                    artworkField = i;
            }


            if (appliedPreset.conditionIsChecked)
            {
                for (int i = 0; i < groupingNames.Count; i++)
                {
                    if (groupingNames[i] == appliedPreset.conditionField)
                        conditionField = i;

                    if (groupingNames[i] == comparedFieldText)
                        comparedField = i;
                }


                for (int i = 0; i < functionNames.Count; i++)
                {
                    if (functionNames[i] == appliedPreset.conditionField)
                        conditionField = groupingNames.Count + i;

                    if (functionNames[i] == comparedFieldText)
                        comparedField = groupingNames.Count + i;
                }
            }
        }

        private void setUnsavedChanges(bool flagUnsavedChanges)
        {
            if (flagUnsavedChanges && !unsavedChanges)
            {
                unsavedChanges = true;
                buttonClose.Image = Plugin.Warning;
                toolTip1.SetToolTip(buttonClose, buttonCloseToolTip);
            }
            else if (!flagUnsavedChanges && unsavedChanges)
            {
                unsavedChanges = false;
                buttonClose.Image = Resources.transparent_15;
                toolTip1.SetToolTip(buttonClose, "");
            }
        }

        private void setPresetChanged()
        {
            if (presetIsLoaded)
                return;

            if (presetsBox.SelectedIndex == -1)
                return;

            assignHotkeyCheckBox.Text = assignHotkeyCheckBoxText + (Plugin.MaximumNumberOfLRHotkeys - reportPresetsWithHotkeysCount) + "/" + Plugin.MaximumNumberOfLRHotkeys;

            UpdatePreset();
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

                    DefaultArtworkHash = GetResizedArtworkBase64Hash(pic);

                    try { hash = md5.ComputeHash((byte[])tc.ConvertTo(pic, typeof(byte[]))); }
                    catch { hash = md5.ComputeHash(new byte[] { 0x00 }); }

                    if (!artworks.TryGetValue(DefaultArtworkHash, out _))
                        artworks.Add(DefaultArtworkHash, pic);
                }
            }

            public static string GetResizedArtworkBase64Hash(Bitmap pic)
            {
                if (Plugin.SavedSettings.resizeArtwork)
                {
                    float xSF = (float)Plugin.SavedSettings.xArtworkSize / (float)pic.Width;
                    float ySF = (float)Plugin.SavedSettings.yArtworkSize / (float)pic.Height;
                    float SF;

                    if (xSF >= ySF)
                        SF = ySF;
                    else
                        SF = xSF;


                    try
                    {
                        Bitmap bm_dest = new Bitmap((int)(pic.Width * SF), (int)(pic.Height * SF), System.Drawing.Imaging.PixelFormat.Format32bppRgb);
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
                    if (artworkBase64 != "")
                        pic = (Bitmap)tc.ConvertFrom(Convert.FromBase64String(artworkBase64));
                    else
                        pic = new Bitmap(DefaultArtwork);
                }
                catch
                {
                    pic = new Bitmap(DefaultArtwork);
                }

                string Base64StringHash = GetResizedArtworkBase64Hash(pic);

                if (!artworks.TryGetValue(Base64StringHash, out _))
                    artworks.Add(Base64StringHash, pic);

                return Base64StringHash;
            }
        }

        public static string ReplaceLeadingZerosBySpaces(int i)
        {
            string oldSequenceNumber = i.ToString("D9");
            string sequenceNumber = "";

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

        private string executePreset(string[] queriedFiles, bool interactive, bool saveResultsToTags, string functionId)
        {
            if (functionId != null && string.IsNullOrWhiteSpace(functionId))
                return Plugin.SbIncorrectLrFunctionId;

            if (functionId != null && functionNames.Count == 0)
                return "???";

            string query;
            string tagValue;

            int sequenceNumberGrouping = -1;

            //Let's cache tag/prop ids
            Plugin.MetaDataType[] queriedGroupingsTagIds = new Plugin.MetaDataType[groupingNames.Count];
            for (int l = 0; l < queriedGroupingsTagIds.Length; l++)
                queriedGroupingsTagIds[l] = 0;

            Plugin.FilePropertyType[] queriedGroupingsPropIds = new Plugin.FilePropertyType[groupingNames.Count];
            for (int l = 0; l < queriedGroupingsTagIds.Length; l++)
                queriedGroupingsTagIds[l] = 0;


            Plugin.MetaDataType[] queriedActualGroupingsTagIds = new Plugin.MetaDataType[groupingNames.Count];
            for (int l = 0; l < queriedActualGroupingsTagIds.Length; l++)
                queriedActualGroupingsTagIds[l] = 0;

            Plugin.FilePropertyType[] queriedActualGroupingsPropIds = new Plugin.FilePropertyType[groupingNames.Count];
            for (int l = 0; l < queriedActualGroupingsTagIds.Length; l++)
                queriedActualGroupingsTagIds[l] = 0;

            
            int[] resultTypes = new int[functionNames.Count];

            for (int l = 0; l < functionNames.Count; l++)
            {
                resultTypes[l] = -1;
            }


            int lastSeqNumInOrder = 1;
            SortedDictionary<string, int> seqNumInOrder = null;


            for (int i = 0; i < groupingNames.Count; i++)
            {
                Plugin.MetaDataType tagId = Plugin.GetTagId(groupingNames[i]);
                string nativeTagName = Plugin.MbApiInterface.Setting_GetFieldName(tagId);

                if (groupingNames[i] == Plugin.SequenceNumberName)
                {
                    nativeTagName = null;
                    tagId = (Plugin.MetaDataType)(-99);
                    queriedActualGroupingsTagIds[i] = tagId;
                    sequenceNumberGrouping = i;

                    seqNumInOrder = new SortedDictionary<string, int>();
                }
                else
                {
                    if (tagId == 0)
                        queriedActualGroupingsPropIds[i] = Plugin.GetPropId(groupingNames[i]);
                    else
                        queriedActualGroupingsTagIds[i] = tagId;
                }


                //MusicBee doesn't support these tags for quering, so let's skip them in query
                if (tagId != Plugin.MetaDataType.AlbumArtistRaw && nativeTagName != null && nativeTagName != Plugin.ArtworkName)
                {
                    if (tagId == 0)
                        queriedGroupingsPropIds[i] = Plugin.GetPropId(groupingNames[i]);
                    else
                        queriedGroupingsTagIds[i] = tagId;
                }
            }


            bool cachedValuesAreRelevant = true;

            if (cachedAppliedPresetGuid != appliedPreset.guid)
                cachedValuesAreRelevant = false;


            SortedDictionary<string, bool>[] queriedGroupingValues = new SortedDictionary<string, bool>[groupingNames.Count];
            for (int l = 0; l < queriedGroupingValues.Length; l++)
                queriedGroupingValues[l] = new SortedDictionary<string, bool>();

            SortedDictionary<string, bool>[] queriedActualGroupingValues = new SortedDictionary<string, bool>[groupingNames.Count];
            for (int l = 0; l < queriedActualGroupingValues.Length; l++)
                queriedActualGroupingValues[l] = new SortedDictionary<string, bool>();


            //Let's add default artwork
            ResizedArtworkProvider.Init(artworkField, artworks);


            bool queryOnlyGroupings = false;
            if (functionNames.Count == 0)
                queryOnlyGroupings = true;

            if (queriedFiles == null && !queryOnlyGroupings)
            {
                query = "domain=Library";
            }
            else
            {
                if (queriedFiles == null && queryOnlyGroupings)
                {
                    if (!Plugin.MbApiInterface.Library_QueryFilesEx("domain=Library", out queriedFiles))
                        return "";

                    if (queriedFiles.Length == 0)
                        return "";
                }

                if (queryOnlyGroupings)
                    tags.Clear();

                for (int n = 0; n < queriedFiles.Length; n++)
                {
                    string[] currentFileGroupingValues = null;

                    if (queryOnlyGroupings)
                        currentFileGroupingValues = new string[groupingNames.Count];

                    for (int i = 0; i < groupingNames.Count; i++)
                    {
                        {
                            //Let's remember actual grouping values for future reuse
                            Plugin.MetaDataType tagId = queriedActualGroupingsTagIds[i];
                            Plugin.FilePropertyType propId = 0;
                            if (tagId == 0)
                                propId = queriedActualGroupingsPropIds[i];

                            if (tagId == (Plugin.MetaDataType)(-99))
                            {
                                tagValue = "xXxXxXxXx"; //Let's reserve space for future numbers
                            }
                            else if (tagId == 0 && propId == 0)
                            {
                                tagValue = ""; //Will ignore this grouping...
                            }
                            else if (tagId == Plugin.ArtistArtistsId || tagId == Plugin.ComposerComposersId) //Lets make smart conversion of list of artists/composers
                            {
                                tagValue = Plugin.GetFileTag(queriedFiles[n], tagId);
                                tagValue = Plugin.GetTagRepresentation(tagValue);
                            }
                            else if (i == artworkField) //It's artwork image. Lets fill cell with hash codes. 
                            {
                                tagValue = ResizedArtworkProvider.GetBase64ArtworkBase64Hash(Plugin.GetFileTag(queriedFiles[n], Plugin.MetaDataType.Artwork), artworks, out Bitmap pic);
                                artwork = pic; //For CD booklet export
                            }
                            else
                            {
                                if (tagId == 0)
                                {
                                    tagValue = Plugin.MbApiInterface.Library_GetFileProperty(queriedFiles[n], propId);
                                }
                                else
                                {
                                    tagValue = Plugin.GetFileTag(queriedFiles[n], tagId, true);
                                }
                            }

                            if (tagValue == "")
                                tagValue = " ";

                            if (!queryOnlyGroupings)
                            {
                                if (!queriedActualGroupingValues[i].TryGetValue(tagValue, out _))
                                    queriedActualGroupingValues[i].Add(tagValue, false);
                            }
                        }

                        if (queryOnlyGroupings)
                            currentFileGroupingValues[i] = tagValue;


                        if (!queryOnlyGroupings)
                        {
                            //Let's remember only grouping values, which can be used in query in Library_QueryFilesEx function
                            Plugin.MetaDataType tagId = queriedGroupingsTagIds[i];
                            Plugin.FilePropertyType propId = 0;
                            if (tagId == 0)
                                propId = queriedGroupingsPropIds[i];

                            if (tagId == 0 && propId == 0)
                            {
                                //MusicBee doesn't support for quering these tags, so let's skip them in query
                            }
                            else
                            {
                                if (tagId == 0)
                                {
                                    tagValue = Plugin.MbApiInterface.Library_GetFileProperty(queriedFiles[n], propId);
                                }
                                else
                                {
                                    tagValue = Plugin.GetFileTag(queriedFiles[n], tagId, true);
                                }

                                if (!queriedGroupingValues[i].TryGetValue(tagValue, out _))
                                    queriedGroupingValues[i].Add(tagValue, false);
                            }
                        }
                    }

                    if (queryOnlyGroupings)
                    {
                        if (interactive)
                            Plugin.SetStatusbarTextForFileOperations(Plugin.LibraryReportsCommandSbText, true, n, queriedFiles.Length, appliedPreset.getName());
                        else //if (functionId == null)
                            Plugin.SetStatusbarTextForFileOperations(Plugin.ApplyingLibraryReportSbText, true, n, queriedFiles.Length, appliedPreset.getName());


                        if (sequenceNumberGrouping != -1)
                        {
                            string composedGroupingValues = AggregatedTags.GetComposedGroupingValues(currentFileGroupingValues);
                            if (!seqNumInOrder.TryGetValue(composedGroupingValues, out _))
                                seqNumInOrder.Add(composedGroupingValues, lastSeqNumInOrder++);
                        }

                        tags.add(null, currentFileGroupingValues, null, null, null, ref resultTypes, appliedPreset.totals);
                    }
                }

                if (queryOnlyGroupings)
                {
                    if (sequenceNumberGrouping != -1)
                    {
                        AggregatedTags tags2 = new AggregatedTags();

                        foreach (KeyValuePair<string, Plugin.ConvertStringsResult[]> keyValue in tags)
                        {
                            int seqNum = seqNumInOrder[keyValue.Key];
                            string sequenceNumber = ReplaceLeadingZerosBySpaces(seqNum);
                            tags2.Add(keyValue.Key.Replace("xXxXxXxXx", sequenceNumber), keyValue.Value);
                        }

                        tags = tags2;
                    }


                    cachedAppliedPresetGuid = appliedPreset.guid;
                    applyOnlyGroupingsPresetResults();
                    return "...";
                }


                query = @"<SmartPlaylist><Source Type=""1""><Conditions CombineMethod=""All"">";

                for (int i = 0; i < groupingNames.Count; i++)
                {
                    Plugin.MetaDataType tagId = queriedGroupingsTagIds[i];
                    Plugin.FilePropertyType propId = queriedGroupingsPropIds[i];

                    if (tagId == 0 && propId == 0)
                    {
                        //Nothing to do...
                    }
                    else
                    {
                        string nativeTagName = Plugin.MbApiInterface.Setting_GetFieldName(tagId);

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
                    for (int i = 0; i < cachedQueriedActualGroupingValues.Length; i++)
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
                                        goto loopexit;
                                    }
                                }
                            }
                        }
                    }
                }
            }

        loopexit:
            if (cachedValuesAreRelevant)
            {
                cachedAppliedPresetGuid = appliedPreset.guid;
                cachedQueriedActualGroupingValues = queriedActualGroupingValues;


                if (queriedFiles == null)
                {
                    if (!Plugin.MbApiInterface.Library_QueryFilesEx(query, out queriedFiles))
                        return "";

                    if (queriedFiles.Length == 0)
                        return "";

                    return applyPresetResults(queriedFiles, interactive, saveResultsToTags, functionId, sequenceNumberGrouping);
                }
                else
                {
                    return applyPresetResults(queriedFiles, interactive, saveResultsToTags, functionId, sequenceNumberGrouping);
                }
            }


            if (!Plugin.MbApiInterface.Library_QueryFilesEx(query, out string[] files))
                return "";

            if (files.Length == 0)
                return "";


            cachedQueriedFilesActualComposedGroupingValues.Clear(); //<url, composed groupings>
            tags.Clear();

            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                bool skipFile = false; //If current file grouping tags are not contained in
                                       // queriedActualGroupingValues, i.e. queried file selection is excessive due to unsupported (and skipped in query) tags

                if (backgroundTaskIsCanceled)
                {
                    if (interactive)
                        Invoke(updateTable);

                    cachedAppliedPresetGuid = Guid.NewGuid();
                    return "";
                }

                string currentFile = files[fileCounter];
                string[] groupingValues;
                string[] functionValues;
                string[] parameter2Values;


                if (interactive)
                    Plugin.SetStatusbarTextForFileOperations(Plugin.LibraryReportsCommandSbText, true, fileCounter, files.Length, appliedPreset.getName());
                else if (functionId == null)
                    Plugin.SetStatusbarTextForFileOperations(Plugin.ApplyingLibraryReportSbText, true, fileCounter, files.Length, appliedPreset.getName());


                groupingValues = new string[groupingNames.Count];

                for (int i = 0; i < groupingNames.Count; i++)
                {
                    Plugin.MetaDataType tagId = queriedActualGroupingsTagIds[i];
                    Plugin.FilePropertyType propId = queriedActualGroupingsPropIds[i];

                    if (tagId == (Plugin.MetaDataType)(-99))
                    {
                        tagValue = "xXxXxXxXx"; //Let's reserve space for future numbers
                    }
                    else if (tagId == 0 && propId == 0)
                    {
                        tagValue = ""; //Will ignore this grouping...
                    }
                    else if (tagId == Plugin.ArtistArtistsId || tagId == Plugin.ComposerComposersId) //Let's make smart conversion of list of artists/composers
                    {
                        tagValue = Plugin.GetFileTag(currentFile, tagId);
                        tagValue = Plugin.GetTagRepresentation(tagValue);
                    }
                    else
                    {
                        if (tagId == 0)
                        {
                            tagValue = Plugin.MbApiInterface.Library_GetFileProperty(currentFile, propId);
                        }
                        else
                        {
                            tagValue = Plugin.GetFileTag(currentFile, tagId, true);
                        }
                    }


                    if (tagValue == "")
                        tagValue = " ";

                    if (queriedFiles != null && !queriedActualGroupingValues[i].TryGetValue(tagValue, out _))
                    {
                        skipFile = true;
                        break; //Break grouping loop
                    }

                    groupingValues[i] = tagValue;
                }

                if (skipFile)
                    continue; //Continue file loop

                string composedGroupingValues = AggregatedTags.GetComposedGroupingValues(groupingValues);

                if (sequenceNumberGrouping != -1)
                {
                    if (!seqNumInOrder.TryGetValue(composedGroupingValues, out _))
                        seqNumInOrder.Add(composedGroupingValues, lastSeqNumInOrder++);
                }

                if (!cachedQueriedFilesActualComposedGroupingValues.TryGetValue(currentFile, out _))
                    cachedQueriedFilesActualComposedGroupingValues.Add(currentFile, composedGroupingValues);

                functionValues = new string[functionNames.Count];
                parameter2Values = new string[functionNames.Count];


                for (int i = 0; i < functionNames.Count; i++)
                {
                    int parameterIndex = functionNames.IndexOf(parameterNames[i]);

                    Plugin.MetaDataType tagId = Plugin.GetTagId(parameterNames[i]);

                    if (parameterNames[i] == Plugin.SequenceNumberName)
                    {
                        tagValue = ReplaceLeadingZerosBySpaces(fileCounter);
                    }
                    else if (tagId == Plugin.ArtistArtistsId || tagId == Plugin.ComposerComposersId) //Lets make smart conversion of list of artists/composers
                    {

                        tagValue = Plugin.GetFileTag(currentFile, tagId);
                        tagValue = Plugin.GetTagRepresentation(tagValue);
                    }
                    else
                    {
                        if (tagId == 0)
                        {
                            Plugin.FilePropertyType propId = Plugin.GetPropId(parameterNames[i]);
                            tagValue = Plugin.MbApiInterface.Library_GetFileProperty(currentFile, propId);
                        }
                        else
                        {
                            tagValue = Plugin.GetFileTag(currentFile, tagId, true);
                        }
                    }

                    functionValues[i] = tagValue;

                    if (functionTypes[i] == FunctionType.Average || functionTypes[i] == FunctionType.AverageCount)
                    {
                        parameterIndex = functionNames.IndexOf(parameter2Names[i]);

                        tagId = Plugin.GetTagId(parameter2Names[i]);

                        if (tagId == Plugin.ArtistArtistsId || tagId == Plugin.ComposerComposersId) //Lets make smart conversion of list of artists/composers
                        {

                            tagValue = Plugin.GetFileTag(currentFile, tagId);
                            tagValue = Plugin.GetTagRepresentation(tagValue);
                        }
                        else
                        {
                            if (tagId == 0)
                            {
                                Plugin.FilePropertyType propId = Plugin.GetPropId(parameter2Names[i]);
                                tagValue = Plugin.MbApiInterface.Library_GetFileProperty(currentFile, propId);
                            }
                            else
                            {
                                tagValue = Plugin.GetFileTag(currentFile, tagId, true);
                            }
                        }

                        parameter2Values[i] = tagValue;
                    }
                    else
                    {
                        parameter2Values[i] = null;
                    }
                }

                if (queriedFiles == null || queriedFiles.Contains(currentFile))
                    tags.add(currentFile, groupingValues, functionValues, functionTypes, parameter2Values, ref resultTypes, appliedPreset.totals);
                else
                    tags.add(null, groupingValues, functionValues, functionTypes, parameter2Values, ref resultTypes, appliedPreset.totals);
            }


            if (sequenceNumberGrouping != -1)
            {
                AggregatedTags tags2 = new AggregatedTags();

                foreach (KeyValuePair<string, Plugin.ConvertStringsResult[]> keyValue in tags)
                {
                    int seqNum = seqNumInOrder[keyValue.Key];
                    string sequenceNumber = ReplaceLeadingZerosBySpaces(seqNum);
                    tags2.Add(keyValue.Key.Replace("xXxXxXxXx", sequenceNumber), keyValue.Value);
                }

                tags = tags2;
            }


            cachedAppliedPresetGuid = appliedPreset.guid;
            if (queriedFiles == null)
                return applyPresetResults(files, interactive, saveResultsToTags, functionId, sequenceNumberGrouping);
            else
                return applyPresetResults(queriedFiles, interactive, saveResultsToTags, functionId, sequenceNumberGrouping);
        }

        private string applyPresetResults(string[] queriedFiles, bool interactive, bool saveResultsToTags, string functionId, int sequenceNumberGrouping)
        {
            if (functionId != null)
                return getFunctionResult(queriedFiles[0], cachedQueriedFilesActualComposedGroupingValues, functionId);


            if (sequenceNumberGrouping != -1)
            {
                AggregatedTags tags2 = new AggregatedTags();

                int i = 1;
                foreach (KeyValuePair<string, Plugin.ConvertStringsResult[]> keyValue in tags)
                {
                    string sequenceNumber = ReplaceLeadingZerosBySpaces(i);
                    tags2.Add(keyValue.Key.Replace("xXxXxXxXx", sequenceNumber), keyValue.Value);
                    i++;
                }

                tags = tags2;
            }


            List<string[]> rows = new List<string[]>();

            if (interactive)
            {
                int filesCount = 0;
                int groupingsCount = 0;
                int totalGroupingsCount = tags.Count;
                foreach (KeyValuePair<string, Plugin.ConvertStringsResult[]> keyValue in tags)
                {
                    if (backgroundTaskIsCanceled)
                    {
                        if (interactive)
                            Invoke(updateTable);

                        cachedAppliedPresetGuid = Guid.NewGuid();
                        return "";
                    }

                    filesCount += keyValue.Value[keyValue.Value.Length - 1].items.Count;

                    string[] groupingsRow = AggregatedTags.GetGroupings(keyValue, groupingNames);
                    string[] row = new string[groupingsRow.Length + keyValue.Value.Length];

                    groupingsRow.CopyTo(row, 0);

                    for (int i = groupingsRow.Length; i < row.Length - 1; i++)
                    {
                        row[i] = AggregatedTags.GetField(keyValue.Key, keyValue.Value, i, groupingNames, 
                            operations[i - groupingsRow.Length], mulDivFactors[i - groupingsRow.Length], precisionDigits[i - groupingsRow.Length], appendTexts[i - groupingsRow.Length]);
                    }
                    row[row.Length - 1] = AggregatedTags.GetField(keyValue.Key, keyValue.Value, row.Length - 1, groupingNames, 
                        0, null, null, null);
                    rows.Add(row);

                    //Plugin.SetStatusbarTextForFileOperations(Plugin.LibraryReportsGneratingPreviewCommandSbText, true, groupingsCount, totalGroupingsCount, appliedPreset.getName());
                    if (groupingsCount % 100 == 0)
                    {
                        Plugin.SetStatusbarTextForFileOperations(Plugin.LibraryReportsGneratingPreviewCommandSbText, true, groupingsCount, totalGroupingsCount, appliedPreset.getName(), 0);
                        Invoke(addRowToTable, rows);
                        rows.Clear();
                    }

                    groupingsCount++;

                    previewIsGenerated = true;
                }

                Plugin.SetStatusbarTextForFileOperations(Plugin.LibraryReportsGneratingPreviewCommandSbText, true, --filesCount, filesCount, appliedPreset.getName());
            }


            if (saveResultsToTags)
            {
                Plugin.SetResultingSbText(appliedPreset.getName(), false, true);
                saveFields(interactive, queriedFiles, cachedQueriedFilesActualComposedGroupingValues);
            }
            else
            {
                Plugin.SetResultingSbText(appliedPreset.getName(), true, true);
            }

            if (interactive)
            {
                Invoke(addRowToTable, rows);
                Invoke(updateTable);
            }

            return "...";
        }

        private void applyOnlyGroupingsPresetResults()
        {
            List<string[]> rows = new List<string[]>();

            int groupingsCount = 0;
            int totalGroupingsCount = tags.Count;
            foreach (KeyValuePair<string, Plugin.ConvertStringsResult[]> keyValue in tags)
            {
                if (backgroundTaskIsCanceled)
                {
                    Invoke(updateTable);

                    cachedAppliedPresetGuid = Guid.NewGuid();
                    return;
                }

                string[] row = AggregatedTags.GetGroupings(keyValue, groupingNames);
                rows.Add(row);

                //Plugin.SetStatusbarTextForFileOperations(Plugin.LibraryReportsGneratingPreviewCommandSbText, true, groupingsCount, totalGroupingsCount, appliedPreset.getName());
                if (groupingsCount % 100 == 0)
                {
                    Plugin.SetStatusbarTextForFileOperations(Plugin.LibraryReportsGneratingPreviewCommandSbText, true, groupingsCount, totalGroupingsCount, appliedPreset.getName(), 0);
                    Invoke(addRowToTable, rows);
                    rows.Clear();
                }

                groupingsCount++;

                previewIsGenerated = true;
            }


            Plugin.SetStatusbarTextForFileOperations(Plugin.LibraryReportsGneratingPreviewCommandSbText, true, --groupingsCount, totalGroupingsCount, appliedPreset.getName(), 0);
            Plugin.SetResultingSbText(appliedPreset.getName(), true, true);

            Invoke(addRowToTable, rows);
            Invoke(updateTable);

            return;
        }

        private bool checkCondition(string composedGroupings, Plugin.ConvertStringsResult[] convertResults)
        {
            if (conditionField == -1)
                return true;

            string value = AggregatedTags.GetField(composedGroupings, convertResults, conditionField, groupingNames, 
                0, null, null, null);
            string comparedValue;


            if (comparedField == -1)
                comparedValue = comparedFieldText;
            else
                comparedValue = AggregatedTags.GetField(composedGroupings, convertResults, comparedField, groupingNames, 
                    0, null, null, null);


            if (conditionText == Plugin.ListItemConditionIs)
            {
                if (value == comparedValue)
                    return true;
                else
                    return false;
            }
            else if (conditionText == Plugin.ListItemConditionIsNot)
            {
                if (value != comparedValue)
                    return true;
                else
                    return false;
            }
            else if (conditionText == Plugin.ListItemConditionIsGreater)
            {
                if (Plugin.CompareStrings(value, comparedValue) == 1)
                    return true;
                else
                    return false;
            }
            else if (conditionText == Plugin.ListItemConditionIsLess)
            {
                if (Plugin.CompareStrings(value, comparedValue) == -1)
                    return true;
                else
                    return false;
            }

            return true;
        }

        private string getFunctionResult(string queriedFile, SortedDictionary<string, string> queriedFilesActualComposedGroupingValues, string functionId)
        {
            string composedFroupingValues = queriedFilesActualComposedGroupingValues[queriedFile];
            Plugin.ConvertStringsResult[] functionValues = tags[composedFroupingValues];

            for (int i = 0; i < functionNames.Count; i++)
            {
                if (savedFunctionIds[i] == functionId)
                {
                    return AggregatedTags.GetField(composedFroupingValues, functionValues, groupingNames.Count + i, groupingNames, 
                        operations[i], mulDivFactors[i], precisionDigits[i], appendTexts[i]);
                }
            }

            return "???";
        }

        private void saveFields(bool interactive, string[] queriedFiles, SortedDictionary<string, string> queriedFilesActualComposedGroupingValues)
        {
            for (int i = 0; i < queriedFiles.Length; i++)
            {
                if (backgroundTaskIsCanceled)
                {
                    cachedAppliedPresetGuid = Guid.NewGuid();
                    return;
                }

                if (interactive)
                    Plugin.SetStatusbarTextForFileOperations(Plugin.LibraryReportsCommandSbText, false, i, queriedFiles.Length, appliedPreset.getName());
                else
                    Plugin.SetStatusbarTextForFileOperations(Plugin.ApplyingLibraryReportSbText, false, i, queriedFiles.Length, appliedPreset.getName());


                string composedFroupingValues = queriedFilesActualComposedGroupingValues[queriedFiles[i]];
                Plugin.ConvertStringsResult[] functionValues = tags[composedFroupingValues];

                for (int j = 0; j < functionNames.Count; j++)
                {
                    if (destinationTagIds[j] > 0)
                    {
                        if (checkCondition(composedFroupingValues, functionValues))
                        {
                            string resultValue = AggregatedTags.GetField(composedFroupingValues, functionValues, groupingNames.Count + j, groupingNames, 
                                operations[j], mulDivFactors[j], precisionDigits[j], appendTexts[j]);
                            Plugin.SetFileTag(queriedFiles[i], destinationTagIds[j], resultValue);
                        }
                    }
                }

                Plugin.CommitTagsToFile(queriedFiles[i], false, false);
            }


            if (interactive)
                Invoke(updateTable);

            Plugin.SetResultingSbText(appliedPreset.getName(), true, true);
        }

        public void autoApplyReportPresetsOnStartup()
        {
            lock (Plugin.SavedSettings.reportsPresets)
            {
                try
                {
                    System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Lowest;

                    if (Plugin.SavedSettings.reportsPresets.Length == 0)
                    {
                        BackgroundTaskIsInProgress = false;
                        return;
                    }


                    if (!Plugin.MbApiInterface.Library_QueryFilesEx("domain=Library", out string[] files))
                        files = new string[0];


                    if (files.Length == 0)
                    {
                        BackgroundTaskIsInProgress = false;
                        return;
                    }

                    int appliedAsrPresetsCount = 0;
                    lock (Plugin.SavedSettings.reportsPresets)
                    {
                        foreach (var autoLibraryReportsPreset in Plugin.SavedSettings.reportsPresets)
                        {
                            if (!autoLibraryReportsPreset.autoApply)
                                continue;

                            appliedPreset = autoLibraryReportsPreset;
                            prepareLocals();
                            executePreset(null, false, true, null);

                            appliedAsrPresetsCount++;
                            if (appliedAsrPresetsCount >= Plugin.SavedSettings.autoAppliedReportPresetsCount)
                                break;
                        }
                    }
                }
                catch (System.Threading.ThreadAbortException)
                {
                    BackgroundTaskIsInProgress = false;
                }
            }

            Plugin.NumberOfTagChanges = 0;

            BackgroundTaskIsInProgress = false;
            Plugin.RefreshPanels(true);
        }

        public void applyReportPresetToLibrary()
        {
            lock (Plugin.SavedSettings.reportsPresets)
            {
                try
                {
                    System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Lowest;

                    prepareLocals();
                    executePreset(null, false, true, null);
                }
                catch (System.Threading.ThreadAbortException)
                {
                    BackgroundTaskIsInProgress = false;
                }
            }

            BackgroundTaskIsInProgress = false;
            Plugin.RefreshPanels(true);
        }

        public void applyReportPresetToSelectedTracks()
        {
            lock (Plugin.SavedSettings.reportsPresets)
            {
                try
                {
                    System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Lowest;

                    if (Plugin.MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out string[] selectedFiles))
                    {
                        if (selectedFiles.Length == 0)
                        {
                            Plugin.SetStatusbarText(Plugin.MsgNoFilesSelected, true);
                            System.Media.SystemSounds.Exclamation.Play();
                        }
                        else
                        {
                            prepareLocals();
                            executePreset(selectedFiles, false, true, null);
                        }
                    }
                }
                catch (System.Threading.ThreadAbortException)
                {
                    BackgroundTaskIsInProgress = false;
                }
            }

            BackgroundTaskIsInProgress = false;
            Plugin.RefreshPanels(true);
        }

        public static void AutoApplyReportPresets()
        {
            if (Plugin.SavedSettings.autoAppliedReportPresetsCount == 0)
                return;

            if (BackgroundTaskIsInProgress)
            {
                Plugin.SetStatusbarText(Plugin.AnotherLrPresetIsRunningSbText, false);
                System.Media.SystemSounds.Exclamation.Play();

                return;
            }
            else
            {
                BackgroundTaskIsInProgress = true;
            }

            Plugin.MbApiInterface.MB_CreateBackgroundTask(Plugin.LibraryReportsCommandForAutoApplying.autoApplyReportPresetsOnStartup, null);
        }

        public static void ApplyReportPreset(ReportPreset preset)
        {
            if (BackgroundTaskIsInProgress)
            {
                Plugin.SetStatusbarText(Plugin.AnotherLrPresetIsRunningSbText, false);
                System.Media.SystemSounds.Exclamation.Play();

                return;
            }
            else
            {
                BackgroundTaskIsInProgress = true;
            }

            Plugin.LibraryReportsCommandForHotkeys.appliedPreset = preset;
            if (preset.applyToSelectedTracks)
            {
                Plugin.MbApiInterface.MB_CreateBackgroundTask(Plugin.LibraryReportsCommandForHotkeys.applyReportPresetToSelectedTracks, null);
            }
            else
            {

                Plugin.MbApiInterface.MB_CreateBackgroundTask(Plugin.LibraryReportsCommandForHotkeys.applyReportPresetToLibrary, null);
            }
        }

        public static void ApplyReportPresetByHotkey(int presetIndex)
        {
            ReportPreset preset = Plugin.ReportPresetsWithHotkeys[presetIndex - 1];

            if (preset.hotkeySlot != presetIndex - 1)
            {
                MessageBox.Show(Plugin.MbForm, Plugin.SbLrHotkeysAreAssignedIncorrectly, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ApplyReportPreset(preset);
        }

        public static string AutoCalculateReportPresetFunction(string calculatedFile, string functionId)
        {
            if (!Plugin.ReportPresetIdsAreInitialized)
            {
                return Plugin.MsgRefershUi;
            }

            if (!Plugin.IdsReportPresets.TryGetValue(functionId, out ReportPreset preset))
                return Plugin.MsgIncorrectReportPresetId;

            Plugin.LibraryReportsCommandForFunctionIds.appliedPreset = preset;

            string[] selectedFiles = new string[] { calculatedFile };
            Plugin.LibraryReportsCommandForFunctionIds.prepareLocals();
            string functionValue = Plugin.LibraryReportsCommandForFunctionIds.executePreset(selectedFiles, false, false, functionId);

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
            for (int i = 0; i < Plugin.ReportPresetsWithHotkeys.Length; i++)
            {
                Plugin.ReportPresetsWithHotkeys[i] = null;
            }

            for (int i = 0; i < Plugin.SavedSettings.reportsPresets.Length; i++)
            {
                if (Plugin.SavedSettings.reportsPresets[i].hotkeyAssigned)
                    Plugin.ReportPresetsWithHotkeys[Plugin.SavedSettings.reportsPresets[i].hotkeySlot] = Plugin.SavedSettings.reportsPresets[i];
            }
        }

        private static void RegisterPresetHotkey(Plugin tagToolsPlugin, ReportPreset preset, int slot)
        {
            if (preset == null)
                return;


            switch (slot)
            {
                case 0:
                    Plugin.MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset1EventHandler);
                    break;
                case 1:
                    Plugin.MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset2EventHandler);
                    break;
                case 2:
                    Plugin.MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset3EventHandler);
                    break;
                case 3:
                    Plugin.MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset4EventHandler);
                    break;
                case 4:
                    Plugin.MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset5EventHandler);
                    break;
                case 5:
                    Plugin.MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset6EventHandler);
                    break;
                case 6:
                    Plugin.MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset7EventHandler);
                    break;
                case 7:
                    Plugin.MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset8EventHandler);
                    break;
                case 8:
                    Plugin.MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset9EventHandler);
                    break;
                case 9:
                    Plugin.MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset10EventHandler);
                    break;
                case 10:
                    Plugin.MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset11EventHandler);
                    break;
                case 11:
                    Plugin.MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset12EventHandler);
                    break;
                case 12:
                    Plugin.MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset13EventHandler);
                    break;
                case 13:
                    Plugin.MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset14EventHandler);
                    break;
                case 14:
                    Plugin.MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset15EventHandler);
                    break;
                case 15:
                    Plugin.MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset16EventHandler);
                    break;
                case 16:
                    Plugin.MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset17EventHandler);
                    break;
                case 17:
                    Plugin.MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset18EventHandler);
                    break;
                case 18:
                    Plugin.MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset19EventHandler);
                    break;
                case 19:
                    Plugin.MbApiInterface.MB_RegisterCommand(preset.getHotkeyDescription(), tagToolsPlugin.LRPreset20EventHandler);
                    break;
                default:
                    break;
            }
        }

        public static void RegisterLRPresetsHotkeys(Plugin tagToolsPlugin)
        {
            if (Plugin.SavedSettings.dontShowLibraryReports)
                return;

            FillReportPresetsWithHotkeysArray();

            for (int i = 0; i < Plugin.ReportPresetsWithHotkeys.Length; i += 2)
            {
                RegisterPresetHotkey(tagToolsPlugin, Plugin.ReportPresetsWithHotkeys[i], i);
            }
        }

        public static void InitReportPresetFunctionIds()
        {
            Plugin.ReportPresetIdsAreInitialized = false;

            Plugin.IdsReportPresets.Clear();

            for (int i = 0; i < Plugin.SavedSettings.reportsPresets.Length; i++)
            {
                ReportPreset autoLibraryReportsPreset = new ReportPreset(Plugin.SavedSettings.reportsPresets[i], true);

                for (int j = 0; j < autoLibraryReportsPreset.functionIds.Length; j++)
                {
                    if (!string.IsNullOrWhiteSpace(autoLibraryReportsPreset.functionIds[j]))
                    {
                        Plugin.IdsReportPresets.Add(autoLibraryReportsPreset.functionIds[j], autoLibraryReportsPreset);
                    }
                }
            }

            Plugin.ReportPresetIdsAreInitialized = true;
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            //Setting control not standard properties
            var heightField = typeof(CheckedListBox).GetField(
                "scaledListItemBordersHeight",
                BindingFlags.NonPublic | BindingFlags.Instance
            );

            var addedHeight = 4; // Some appropriate value, greater than the field's default of 2

            heightField.SetValue(presetsBox, addedHeight); // Where "presetsBox" is your CheckedListBox
            heightField.SetValue(sourceTagList, 5); // Where "sourceTagList" is your CheckedListBox


            //Setting themed images
            clearIdButton.Image = Plugin.ButtonRemoveImage;


            //Setting themed colors
            //Color sampleColor = SystemColors.Highlight;


            //Clear "unsaved changes" button image
            buttonClose.Image = Resources.transparent_15;
            buttonCloseToolTip = toolTip1.GetToolTip(buttonClose);
            toolTip1.SetToolTip(buttonClose, "");


            //Preparing "ticked presets" label text
            string entireText = autoApplyInfoLabel.Text;
            autoApplyText = Regex.Replace(entireText, @"^(.*?)\n(.*)", "$1").TrimEnd('\n').TrimEnd('\r');
            nowTickedText = Regex.Replace(entireText, @"^(.*?)\n(.*)", "\n$2").TrimEnd('\n').TrimEnd('\r');

            autoApplyInfoLabel.Text = autoApplyText;


            //Initialization
            functionComboBox.Items.Add(Plugin.GroupingName);
            functionComboBox.Items.Add(Plugin.CountName);
            functionComboBox.Items.Add(Plugin.SumName);
            functionComboBox.Items.Add(Plugin.MinimumName);
            functionComboBox.Items.Add(Plugin.MaximumName);
            functionComboBox.Items.Add(Plugin.AverageName);
            functionComboBox.Items.Add(Plugin.AverageCountName);

            Plugin.FillListByTagNames(sourceTagList.Items, true, true);
            Plugin.FillListByPropNames(sourceTagList.Items);
            sourceTagList.Items.Add(Plugin.SequenceNumberName);

            Plugin.FillListByTagNames(parameter2ComboBox.Items, true, false);
            Plugin.FillListByPropNames(parameter2ComboBox.Items);

            conditionList.Items.Add(Plugin.ListItemConditionIs);
            conditionList.Items.Add(Plugin.ListItemConditionIsNot);
            conditionList.Items.Add(Plugin.ListItemConditionIsGreater);
            conditionList.Items.Add(Plugin.ListItemConditionIsLess);


            CueProvider.SetTextBoxCue(presetNameTextBox, Plugin.CtlAutoLRPresetName);


            ignoreCheckedPresetEvent = false;
            autoAppliedPresetCount = 0;
            reportPresetsWithHotkeysCount = 0;
            for (int i = 0; i < Plugin.SavedSettings.reportsPresets.Length; i++)
            {
                ReportPreset autoLibraryReportsPreset = new ReportPreset(Plugin.SavedSettings.reportsPresets[i], true);

                presetsBox.Items.Add(autoLibraryReportsPreset);

                if (autoLibraryReportsPreset.autoApply)
                    presetsBox.SetItemChecked(i, true);

                if (autoLibraryReportsPreset.hotkeyAssigned)
                {
                    reportPresetsWithHotkeysCount++;
                    reportPresetHotkeyUsedSlots[autoLibraryReportsPreset.hotkeySlot] = true;
                }
            }
            ignoreCheckedPresetEvent = true;


            assignHotkeyCheckBoxText = assignHotkeyCheckBox.Text;
            assignHotkeyCheckBox.Text = assignHotkeyCheckBoxText + (Plugin.MaximumNumberOfLRHotkeys - reportPresetsWithHotkeysCount) + "/" + Plugin.MaximumNumberOfLRHotkeys;


            Plugin.FillListByTagNames(destinationTagList.Items);

            presetsBox.SelectedIndex = -1;
            buttonCopyPreset.Enabled = false;
            buttonDeletePreset.Enabled = false;

            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            presetsBox_SelectedIndexChanged(null, null);


            buttonExport.Enabled = false;
            labelFormat.Enabled = false;
            formatComboBox.Enabled = false;
            resizeArtworkCheckBox.Enabled = false;
            xArworkSizeUpDown.Enabled = false;
            yArworkSizeUpDown.Enabled = false;
            labelXxY.Enabled = false;
            labelPx.Enabled = false;
            openReportCheckBox.Enabled = false;

            buttonApply.Enabled = false;


            resizeArtworkCheckBox.Checked = Plugin.SavedSettings.resizeArtwork;
            xArworkSizeUpDown.Value = Plugin.SavedSettings.xArtworkSize == 0 ? 300 : Plugin.SavedSettings.xArtworkSize;
            yArworkSizeUpDown.Value = Plugin.SavedSettings.yArtworkSize == 0 ? 300 : Plugin.SavedSettings.yArtworkSize;

            openReportCheckBox.Checked = Plugin.SavedSettings.openReportAfterExporting;

            recalculateOnNumberOfTagsChangesCheckBox.Checked = Plugin.SavedSettings.recalculateOnNumberOfTagsChanges;
            numberOfTagsToRecalculateNumericUpDown.Value = Plugin.SavedSettings.numberOfTagsToRecalculate;

            string[] formats = Plugin.ExportedFormats.Split('|');
            for (int i = 0; i < formats.Length; i+=2)
            {
                formatComboBox.Items.Add(formats[i]);
            }
            formatComboBox.SelectedIndex = Plugin.SavedSettings.filterIndex - 1;

            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            addRowToTable = previewList_AddRowToTable;
            updateTable = previewList_updateTable;
        }

        public static string GetColumnName(string tagName, string tag2Name, FunctionType type)
        {
            if (type == FunctionType.Count)
            {
                return Plugin.CountName + "(" + tagName + ")";
            }
            else if (type == FunctionType.Sum)
            {
                return Plugin.SumName + "(" + tagName + ")";
            }
            else if (type == FunctionType.Minimum)
            {
                return Plugin.MinimumName + "(" + tagName + ")";
            }
            else if (type == FunctionType.Maximum)
            {
                return Plugin.MaximumName + "(" + tagName + ")";
            }
            else if (type == FunctionType.Average)
            {
                return Plugin.AverageName + "(" + tagName + "/" + tag2Name + ")";
            }
            else if (type == FunctionType.AverageCount)
            {
                return Plugin.AverageCountName + "(" + tagName + "/" + tag2Name + ")";
            }
            else //Its grouping
            {
                return tagName;
            }
        }

        private void previewList_AddRowToTable(List<string[]> rows)
        {
            foreach (var row in rows)
            {
                previewTable.Rows.Add(row);

                if (artworkField != -1)
                {
                    //Replace string hashes in the Artwork column with images.
                    string stringHash = (string)previewTable.Rows[previewTable.Rows.Count - 1].Cells[artworkField].Value;
                    Bitmap pic;

                    try
                    {
                        pic = artworks[stringHash];
                    }
                    catch
                    {
                        pic = artworks[DefaultArtworkHash];
                    }

                    previewTable.Rows[previewTable.Rows.Count - 1].Cells[artworkField].ValueType = typeof(Bitmap);
                    previewTable.Rows[previewTable.Rows.Count - 1].Cells[artworkField].Value = pic;
                }
            }
        }

        private void previewList_updateTable()
        {
            if (previewTable.Rows.Count > 0)
            {
                previewTable.CurrentCell = previewTable.Rows[0].Cells[0];
            }
        }

        private bool addColumn(string fieldName, string parameter2Name, FunctionType type)
        {
            DataGridViewColumn column;

            if (fieldName == Plugin.ArtworkName && type != FunctionType.Grouping)
            {
                MessageBox.Show(this, Plugin.MsgPleaseUseGroupingFunctionForArtworkTag, 
                    null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }


            DataGridViewTextBoxColumn textColumnTemplate = new DataGridViewTextBoxColumn();
            column = new DataGridViewColumn(textColumnTemplate.CellTemplate);
            column.SortMode = DataGridViewColumnSortMode.Programmatic;

            column.HeaderCell.Style.WrapMode = DataGridViewTriState.True;


            column.HeaderText = GetColumnName(fieldName, parameter2Name, type);

            conditionFieldList.Items.Add(column.HeaderText);
            if (conditionFieldList.SelectedIndex == -1)
                conditionFieldList.SelectedIndex = 0;

            comparedFieldList.Items.Add(column.HeaderText);

            conditionCheckBox.Enabled = true;
            conditionCheckBox_CheckedChanged(null, null);

            if (fieldName == Plugin.ArtworkName)
            {
                DataGridViewImageColumn imageColumnTemplate = new DataGridViewImageColumn();
                imageColumnTemplate.ImageLayout = DataGridViewImageCellLayout.Zoom;
                imageColumnTemplate.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
                column = new DataGridViewColumn(imageColumnTemplate.CellTemplate);
                column.HeaderText = fieldName;

                previewTable.Columns.Insert(groupingNames.Count, column);
                groupingNames.Add(fieldName);

                artworkField = groupingNames.Count - 1;
            }
            else if (type == FunctionType.Grouping)
            {
                previewTable.Columns.Insert(groupingNames.Count, column);
                groupingNames.Add(fieldName);
            }
            else
            {
                previewTable.Columns.Add(column);
                functionNames.Add(column.HeaderText);
                functionTypes.Add(type);
                parameterNames.Add(fieldName);

                if (type == FunctionType.Average || type == FunctionType.AverageCount)
                    parameter2Names.Add(parameter2Name);
                else
                    parameter2Names.Add(null);


                destinationTagList.SelectedItem = Plugin.NullTagName;
                savedFunctionIds.Add("");
                savedDestinationTagsNames.Add((string)destinationTagList.SelectedItem);

                operations.Add(0);
                mulDivFactors.Add((string)mulDivFactorComboBox.Items[0]);
                precisionDigits.Add((string)precisionDigitsComboBox.Items[0]);
                appendTexts.Add("");

                sourceFieldComboBox.Items.Add(column.HeaderText);
                if (sourceFieldComboBox.SelectedIndex == -1)
                    sourceFieldComboBox.SelectedIndex = 0;
            }


            if (previewTable.Rows.Count > 0)
                clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, buttonPreview, buttonExport, buttonClose);

            return true;
        }

        private void removeColumn(string fieldName, string parameter2Name, FunctionType type)
        {
            string columnHeader = GetColumnName(fieldName, parameter2Name, type);

            if (groupingNames.Contains(columnHeader))
            {
                groupingNames.Remove(columnHeader);
                if (fieldName == Plugin.ArtworkName)
                    artworkField = -1;
            }
            else
            {
                int index = functionNames.IndexOf(columnHeader);

                functionNames.RemoveAt(index);
                functionTypes.RemoveAt(index);
                parameterNames.RemoveAt(index);
                parameter2Names.RemoveAt(index);

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


            conditionFieldList.Items.Remove(columnHeader);
            if (conditionFieldList.SelectedIndex == -1 && conditionFieldList.Items.Count > 0)
                conditionFieldList.SelectedIndex = 0;


            if (comparedFieldList.Text == (string)comparedFieldList.SelectedValue)
                comparedFieldList.Text = "";

            comparedFieldList.Items.Remove(columnHeader);

            if (conditionFieldList.Items.Count == 0)
            {
                conditionCheckBox.Enabled = false;
                conditionCheckBox.Checked = false;
            }


            foreach (DataGridViewColumn column in previewTable.Columns)
            {
                if (column.HeaderText == columnHeader)
                {
                    previewTable.Columns.RemoveAt(column.Index);

                    if (previewTable.Columns.Count == 0)
                        clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, buttonPreview, buttonExport, buttonClose);

                    return;
                }
            }

            if (previewTable.Columns.Count == 0)
                clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, buttonPreview, buttonExport, buttonClose);
        }

        private void addAllTags()
        {
            for (int i = 0; i < sourceTagList.Items.Count; i++)
            {
                if ((string)sourceTagList.Items[i] != Plugin.ArtworkName)
                    sourceTagList.SetItemChecked(i, true);
            }
        }

        private bool prepareBackgroundPreview()
        {
            previewTable.Rows.Clear();
            previewIsGenerated = false;

            if (backgroundTaskIsWorking())
                return true;

            if (previewTable.Columns.Count == 0)
            {
                //MessageBox.Show(this, tagToolsPlugin.msgNoTagsSelected);
                return false;
            }

            appliedPreset = (ReportPreset)presetsBox.SelectedItem;
            prepareLocals();


            sourceTagList.Enabled = false;
            buttonCheckAll.Enabled = false;
            buttonUncheckAll.Enabled = false;

            return true;
        }

        private void previewTrackList()
        {
            if (!Plugin.MbApiInterface.Library_QueryFilesEx("domain=DisplayedFiles", out string[] files) || files.Length == 0)
            {
                Plugin.SetStatusbarText(Plugin.MsgNoFilesInCurrentView, false);
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }

            executePreset(files, true, false, null);
        }

        private void exportTrackList(bool openReport)
        {
            const int size = 550; //Height and width of CD artwork
            string fileDirectoryPath = null;
            string reportFullFileName = null;
            string reportOnlyFileName = null;

            ExportedDocument document = null;
            int seqNumField = -1;
            int urlField = -1;
            int albumArtistField = -1;
            int albumField = -1;
            int titleField = -1;
            int durationField = -1;
            Bitmap pic = null;

            backgroundTaskIsCanceled = false;

            if (!prepareBackgroundTask(false))
                return;

            if (openReport)
            {
                fileDirectoryPath = System.IO.Path.GetTempPath();
            }
            else
            {
                if (Plugin.SavedSettings.exportedLastFolder == null)
                    Plugin.SavedSettings.exportedLastFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";

                fileDirectoryPath = Plugin.SavedSettings.exportedLastFolder;
            }

            ReportPreset selectedPreset = (ReportPreset)presetsBox.SelectedItem;

            if (!openReport)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.InitialDirectory = fileDirectoryPath;
                dialog.FileName = selectedPreset.exportedTrackListName;
                dialog.Filter = Plugin.ExportedFormats;
                dialog.FilterIndex = Plugin.SavedSettings.filterIndex;

                if (dialog.ShowDialog(this) == DialogResult.Cancel) return;

                Plugin.SavedSettings.filterIndex = dialog.FilterIndex;
                formatComboBox.SelectedIndex = dialog.FilterIndex - 1;

                reportFullFileName = dialog.FileName;

                fileDirectoryPath = Regex.Replace(dialog.FileName, @"^(.*\\).*\..*", "$1"); //File directory path including ending \
                reportOnlyFileName = Regex.Replace(dialog.FileName, @"^.*\\(.*)\..*", "$1"); //Filename without path to file and extension

                Plugin.SavedSettings.filterIndex = dialog.FilterIndex;
                selectedPreset.exportedTrackListName = reportOnlyFileName;

                if (Plugin.SavedSettings.reportsPresets != null)
                {
                    foreach (ReportPreset preset in Plugin.SavedSettings.reportsPresets)
                    {
                        if (preset.permanentGuid == selectedPreset.permanentGuid)
                        {
                            preset.exportedTrackListName = reportOnlyFileName;
                            break;
                        }
                    }
                }
                
                Plugin.SavedSettings.exportedLastFolder = fileDirectoryPath;
            }
            else
            {
                reportOnlyFileName = selectedPreset.exportedTrackListName;
                reportFullFileName = fileDirectoryPath + reportOnlyFileName + Plugin.ExportedFormats
                    .Split('|')[(Plugin.SavedSettings.filterIndex - 1) * 2 + 1].Substring(1);
            }


            string imagesDirectoryName = reportOnlyFileName + ".files";


            for (int j = 0; j < groupingNames.Count; j++)
            {
                if (groupingNames[j] == Plugin.UrlTagName)
                    urlField = j;
                else if (groupingNames[j] == Plugin.DisplayedAlbumArtsistName)
                    albumArtistField = j;
                else if (groupingNames[j] == Plugin.MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Album))
                    albumField = j;
                else if (groupingNames[j] == Plugin.SequenceNumberName)
                    seqNumField = j;
                else if (groupingNames[j] == Plugin.MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.TrackTitle))
                    titleField = j;
                else if (groupingNames[j] == Plugin.MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Duration))
                    durationField = j;
            }


            if (Plugin.SavedSettings.filterIndex == 1 && (albumArtistField != 0 || albumField != 1 || artworkField != 2))
            {
                MessageBox.Show(this, Plugin.MsgFirstThreeGroupingFieldsInPreviewTableShouldBe, 
                    null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (Plugin.SavedSettings.filterIndex == 7 && (seqNumField != 0 || albumArtistField != 1 || albumField != 2 
            || artworkField != 3 || titleField != 4 || durationField != 5))
            {
                MessageBox.Show(this, Plugin.MsgFirstSixGroupingFieldsInPreviewTableShouldBe, 
                    null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (Plugin.SavedSettings.filterIndex == 5 && urlField == -1)
            {
                MessageBox.Show(this, Plugin.MsgForExportingPlaylistsURLfieldMustBeIncludedInTagList, 
                    null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            SortedDictionary<string, List<string>> albumArtistsAlbums = new SortedDictionary<string, List<string>>();
            string base64Artwork = null;

            List<int> albumTrackCounts = new List<int>();

            System.IO.FileStream stream = new System.IO.FileStream(reportFullFileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);

            try
            {
                if (Plugin.SavedSettings.filterIndex == 1 || Plugin.SavedSettings.filterIndex == 7)
                {
                    if (System.IO.Directory.Exists(fileDirectoryPath + imagesDirectoryName))
                        System.IO.Directory.Delete(fileDirectoryPath + imagesDirectoryName, true);

                    System.IO.Directory.CreateDirectory(fileDirectoryPath + imagesDirectoryName);
                }
                else if (Plugin.SavedSettings.filterIndex == 2 || Plugin.SavedSettings.filterIndex == 3)
                {
                    if (System.IO.Directory.Exists(fileDirectoryPath + imagesDirectoryName))
                        System.IO.Directory.Delete(fileDirectoryPath + imagesDirectoryName, true);

                    if (Plugin.SavedSettings.filterIndex == 2 || artworkField != -1)
                        System.IO.Directory.CreateDirectory(fileDirectoryPath + imagesDirectoryName);
                }


                switch (Plugin.SavedSettings.filterIndex)
                {
                    case 1:
                    case 7:
                        string[] groupingsValues1 = null;
                        string prevAlbum1 = null;
                        string prevAlbumArtist1 = null;

                        int trackCount = 0;
                        foreach (KeyValuePair<string, Plugin.ConvertStringsResult[]> keyValue in tags)
                        {
                            if (Plugin.SavedSettings.filterIndex == 7)
                            {
                                string[] groupingsValues = AggregatedTags.GetGroupings(keyValue, groupingNames);

                                if (base64Artwork == null)
                                    base64Artwork = groupingsValues[artworkField];
                                else if (base64Artwork == "")
                                    ;
                                else if (base64Artwork != groupingsValues[artworkField])
                                    base64Artwork = "";
                            }

                            groupingsValues1 = AggregatedTags.GetGroupings(keyValue, groupingNames);

                            if (prevAlbumArtist1 != groupingsValues1[albumArtistField] || prevAlbum1 != groupingsValues1[albumField])
                            {
                                if (prevAlbumArtist1 != groupingsValues1[albumArtistField])
                                {
                                    prevAlbumArtist1 = groupingsValues1[albumArtistField];

                                    albumArtistsAlbums.Add(prevAlbumArtist1, new List<string>());
                                }
                                prevAlbum1 = groupingsValues1[albumField];

                                albumArtistsAlbums[prevAlbumArtist1].Add(prevAlbum1);
                                albumTrackCounts.Add(trackCount);
                                trackCount = 0;
                            }

                            trackCount++;
                        }

                        albumTrackCounts.Add(trackCount);


                        if (Plugin.SavedSettings.filterIndex == 1)
                            document = new HtmlDocumentByAlbum(stream, TagToolsPlugin, fileDirectoryPath, imagesDirectoryName);
                        else if (Plugin.SavedSettings.filterIndex == 7)
                            document = new HtmlDocumentCDBooklet(stream, TagToolsPlugin, fileDirectoryPath, imagesDirectoryName);
                        break;
                    case 2:
                        document = new HtmlDocument(stream, TagToolsPlugin, fileDirectoryPath, imagesDirectoryName);
                        break;
                    case 3:
                        document = new HtmlTable(stream, TagToolsPlugin, fileDirectoryPath, imagesDirectoryName);
                        break;
                    case 4:
                        document = new TextDocument(stream, TagToolsPlugin);
                        break;
                    case 5:
                        document = new M3UDocument(stream, TagToolsPlugin);
                        break;
                    case 6:
                        document = new CsvDocument(stream, TagToolsPlugin);
                        break;
                    default:
                        return;
                }
            }
            catch (Exception ex)
            {
                stream.Close();
                MessageBox.Show(this, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (Plugin.SavedSettings.filterIndex != 7)
            {
                document.writeHeader();
            }
            else //It's CD booklet
            {
                Bitmap scaledPic;

                if (base64Artwork == "") //There are various artworks
                {
                    pic = artworks[DefaultArtworkHash];
                }
                else //All tracks have the same artwork
                {
                    pic = artwork;
                }

                float xSF = (float)size / (float)pic.Width;
                float ySF = (float)size / (float)pic.Height;
                float SF;

                if (xSF >= ySF)
                    SF = ySF;
                else
                    SF = xSF;


                scaledPic = new Bitmap((int)(pic.Width * SF), (int)(pic.Height * SF), System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                Graphics gr_dest = Graphics.FromImage(scaledPic);
                gr_dest.DrawImage(pic, 0, 0, scaledPic.Width, scaledPic.Height);
                gr_dest.Dispose();

                ((HtmlDocumentCDBooklet)document).writeHeader(size, Plugin.GetBitmapAverageColor(scaledPic), scaledPic, 
                    albumArtistsAlbums, tags.Count);

                scaledPic.Dispose();
            }

            if (Plugin.SavedSettings.filterIndex != 5 && Plugin.SavedSettings.filterIndex != 7) //Lets write table headers
            {
                for (int j = 0; j < groupingNames.Count; j++)
                {
                    document.addCellToRow(groupingNames[j], groupingNames[j], j == albumArtistField, j == albumField);
                }

                for (int j = 0; j < functionNames.Count; j++)
                {
                    document.addCellToRow(functionNames[j], functionNames[j], false, false);
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

            foreach (KeyValuePair<string, Plugin.ConvertStringsResult[]> keyValue in tags)
            {
                if (backgroundTaskIsCanceled)
                    return;

                if (checkCondition(keyValue.Key, keyValue.Value))
                {
                    string[] groupingsValues = AggregatedTags.GetGroupings(keyValue, groupingNames);

                    if (Plugin.SavedSettings.filterIndex == 1)
                    {
                        if (prevAlbumArtist != groupingsValues[albumArtistField])
                        {
                            i++;
                            prevAlbumArtist = groupingsValues[albumArtistField];
                            prevAlbum = groupingsValues[albumField];
                            document.beginAlbumArtist(groupingsValues[albumArtistField], groupingsValues.Length - 2 + functionNames.Count);
                            document.beginAlbum(groupingsValues[albumField], artworks[groupingsValues[artworkField]], groupingsValues[artworkField], albumTrackCounts[i]);
                        }
                        else if (prevAlbum != groupingsValues[albumField])
                        {
                            i++;
                            prevAlbum = groupingsValues[albumField];
                            document.beginAlbum(groupingsValues[albumField], artworks[groupingsValues[artworkField]], groupingsValues[artworkField], albumTrackCounts[i]);
                        }
                    }


                    if (Plugin.SavedSettings.filterIndex != 7) //It's not a CD booklet
                    {
                        for (int j = 0; j < groupingNames.Count; j++)
                        {
                            if (j == artworkField && (Plugin.SavedSettings.filterIndex == 1 || Plugin.SavedSettings.filterIndex == 2 || Plugin.SavedSettings.filterIndex == 3)) //Export images
                            {
                                pic = artworks[groupingsValues[artworkField]];

                                height = pic.Height;

                                document.addCellToRow(pic, groupingNames[j], groupingsValues[j], pic.Width, pic.Height);
                            }
                            else if (j == artworkField && Plugin.SavedSettings.filterIndex != 1 && Plugin.SavedSettings.filterIndex != 2 && Plugin.SavedSettings.filterIndex != 3) //Export image hashes
                                document.addCellToRow(groupingsValues[artworkField], groupingNames[j], j == albumArtistField, j == albumField);
                            else //Its not the artwork column
                                document.addCellToRow(groupingsValues[j], groupingNames[j], j == albumArtistField, j == albumField);
                        }

                        for (int j = 0; j < functionNames.Count; j++)
                        {
                            document.addCellToRow(AggregatedTags.GetField(keyValue.Key, keyValue.Value, groupingNames.Count + j, groupingNames, 
                                operations[j], mulDivFactors[j], precisionDigits[j], appendTexts[j]), 
                                functionNames[j], false, false);
                        }
                    }
                    else //It's a CD booklet
                    {
                        if (!multipleAlbumArtists) //1 album artist
                        {
                            if (!multipleAlbums) //1 album
                            {
                                ((HtmlDocumentCDBooklet)document).addTrack(int.Parse(groupingsValues[seqNumField]),
                                    null, null, groupingsValues[titleField], groupingsValues[durationField]);
                            }
                            else //Several albums
                            {
                                ((HtmlDocumentCDBooklet)document).addTrack(int.Parse(groupingsValues[seqNumField]),
                                    null, groupingsValues[albumField], groupingsValues[titleField], groupingsValues[durationField]);
                            }
                        }
                        else //Several album artists
                        {
                            ((HtmlDocumentCDBooklet)document).addTrack(int.Parse(groupingsValues[seqNumField]),
                                groupingsValues[albumArtistField], groupingsValues[albumField], groupingsValues[titleField],
                                groupingsValues[durationField]);
                        }
                    }


                    document.writeRow(height);
                    height = 0;
                }
            }

            if (Plugin.SavedSettings.filterIndex != 7) //It's not a CD booklet
                document.writeFooter();
            else
                ((HtmlDocumentCDBooklet)document).writeFooter(albumArtistsAlbums);

            document.close();

            if (openReportCheckBox.Checked)
            {
                var waiter = new System.Threading.Thread(OpenReport);
                waiter.Start(reportFullFileName);
            }
        }

        public static void OpenReport(object document)
        {
            string documentPathFileName = (string)document;

            System.Diagnostics.Process.Start(documentPathFileName);

            return; //Code below may not work reliable

            if (ProccessedReportDeletions.Contains(documentPathFileName))
                return;

            ProccessedReportDeletions.Add(documentPathFileName);

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
                ProccessedReportDeletions.Remove(documentPathFileName);
                return;
            }

            System.Threading.Thread.Sleep(10000);

            ProccessedReportDeletions.Remove(documentPathFileName);
            string imagesDirectoryPath = Regex.Replace(documentPathFileName, @"^(.*)\..*", "$1") + ".files";
            if (System.IO.Directory.Exists(imagesDirectoryPath))
                System.IO.Directory.Delete(imagesDirectoryPath, true);
        }

        private bool prepareBackgroundTask(bool checkForFunctions)
        {
            if (backgroundTaskIsWorking())
                return true;

            if (previewTable.Rows.Count == 0)
            {
                MessageBox.Show(this, Plugin.MsgPreviewIsNotGeneratedNothingToSave, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (checkForFunctions && sourceFieldComboBox.SelectedIndex == -1)
            {
                MessageBox.Show(this, Plugin.MsgNoAggregateFunctionNothingToSave, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            appliedPreset = (ReportPreset)presetsBox.SelectedItem;
            prepareLocals();

            if (checkForFunctions)
            {
                sourceTagList.Enabled = false;
                buttonCheckAll.Enabled = false;
                buttonUncheckAll.Enabled = false;
            }

            return true;
        }

        private void saveSettings()
        {
            Plugin.SavedSettings.recalculateOnNumberOfTagsChanges = recalculateOnNumberOfTagsChangesCheckBox.Checked;
            Plugin.SavedSettings.numberOfTagsToRecalculate = numberOfTagsToRecalculateNumericUpDown.Value;
            Plugin.SavedSettings.autoAppliedReportPresetsCount = autoAppliedPresetCount;

            Plugin.SavedSettings.reportsPresets = new ReportPreset[presetsBox.Items.Count];
            presetsBox.Items.CopyTo(Plugin.SavedSettings.reportsPresets, 0);

            InitReportPresetFunctionIds();
            RegisterLRPresetsHotkeys(TagToolsPlugin);
            setUnsavedChanges(false);

            Plugin.SavedSettings.filterIndex = formatComboBox.SelectedIndex + 1;
            Plugin.SavedSettings.openReportAfterExporting = openReportCheckBox.Checked;

            TagToolsPlugin.SaveSettings();
        }

        public override void enableDisablePreviewOptionControls(bool enable)
        {
            if (enable && previewIsGenerated)
                return;

            sourceTagList.Enabled = enable;
            buttonCheckAll.Enabled = enable;
            buttonUncheckAll.Enabled = enable;
            totalsCheckBox.Enabled = enable;

            functionComboBox.Enabled = enable;
            resizeArtworkCheckBox.Enabled = enable && previewIsGenerated;
            xArworkSizeUpDown.Enabled = enable && resizeArtworkCheckBox.Checked && previewIsGenerated;
            yArworkSizeUpDown.Enabled = enable && resizeArtworkCheckBox.Checked && previewIsGenerated;
            labelXxY.Enabled = enable && resizeArtworkCheckBox.Checked && previewIsGenerated;
            labelPx.Enabled = enable && resizeArtworkCheckBox.Checked && previewIsGenerated;

            buttonExport.Enabled = enable && previewIsGenerated;
            labelFormat.Enabled = enable && previewIsGenerated;
            formatComboBox.Enabled = enable && previewIsGenerated;
            openReportCheckBox.Enabled = enable && previewIsGenerated;

            //buttonApply.Enabled = enable;

            buttonSave.Enabled = enable;

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
            buttonExport.Enabled = previewIsGenerated;
            labelFormat.Enabled = previewIsGenerated;
            formatComboBox.Enabled = previewIsGenerated;
            resizeArtworkCheckBox.Enabled = previewIsGenerated;
            xArworkSizeUpDown.Enabled = previewIsGenerated;
            yArworkSizeUpDown.Enabled = previewIsGenerated;
            labelXxY.Enabled = previewIsGenerated;
            labelPx.Enabled = previewIsGenerated;
            openReportCheckBox.Enabled = previewIsGenerated;

            buttonPreview.Enabled = true;
            //buttonApply.Enabled = previewIsGenerated;

            buttonSave.Enabled = true;
        }

        public override void disableQueryingOrUpdatingButtons()
        {
            buttonExport.Enabled = false;
            labelFormat.Enabled = false;
            formatComboBox.Enabled = false;
            resizeArtworkCheckBox.Enabled = false;
            xArworkSizeUpDown.Enabled = false;
            yArworkSizeUpDown.Enabled = false;
            labelXxY.Enabled = false;
            labelPx.Enabled = false;
            openReportCheckBox.Enabled = false;

            buttonPreview.Enabled = false;
            //buttonApply.Enabled = false;

            buttonSave.Enabled = false;
        }

        private void recalculateOnNumberOfTagsChangesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            numberOfTagsToRecalculateNumericUpDown.Enabled = recalculateOnNumberOfTagsChangesCheckBox.Checked;
        }

        private void buttonAddPreset_Click(object sender, EventArgs e)
        {
            presetsBox.Items.Insert(0, new ReportPreset(Plugin.ExportedTrackList));
            presetsBox.SelectedIndex = 0;
            
            if (presetsBoxLastSelectedIndex == 0)
            {
                presetsBoxLastSelectedIndex = -1;
                presetsBox_SelectedIndexChanged(null, null);
            }
            
            setUnsavedChanges(true);
        }

        private void buttonCopyPreset_Click(object sender, EventArgs e)
        {
            int firstPredefinedPresetIndex;
            for (firstPredefinedPresetIndex = 0; firstPredefinedPresetIndex < presetsBox.Items.Count; firstPredefinedPresetIndex++)
            {
                if (!((ReportPreset)presetsBox.Items[firstPredefinedPresetIndex]).userPreset)
                    break;
            }

            int currentIndex = presetsBox.SelectedIndex;
            ReportPreset reportsPresetCopy = new ReportPreset((ReportPreset)presetsBox.SelectedItem, false);
            if (currentIndex < firstPredefinedPresetIndex)
                presetsBox.Items.Insert(currentIndex + 1, reportsPresetCopy);
            else
                presetsBox.Items.Insert(0, reportsPresetCopy);

            presetsBox.SelectedItem = reportsPresetCopy;

            if (presetsBoxLastSelectedIndex == presetsBox.SelectedIndex)
            {
                presetsBoxLastSelectedIndex = -1;
                presetsBox_SelectedIndexChanged(null, null);
            }

            setUnsavedChanges(true);
        }

        private void UpdatePreset()
        {
            if (presetsBox.SelectedIndex >= 0)
            {
                var preset = (ReportPreset)presetsBox.SelectedItem;

                preset.guid = Guid.NewGuid();

                if (string.IsNullOrWhiteSpace(presetNameTextBox.Text))
                    preset.name = null;
                else
                    preset.name = presetNameTextBox.Text;


                if (assignHotkeyCheckBox.Checked && !preset.hotkeyAssigned)
                {
                    preset.hotkeySlot = FindFirstSlot(reportPresetHotkeyUsedSlots, false);
                    reportPresetHotkeyUsedSlots[preset.hotkeySlot] = true;
                }
                else if (!assignHotkeyCheckBox.Checked && preset.hotkeyAssigned)
                {
                    reportPresetHotkeyUsedSlots[preset.hotkeySlot] = false;
                    preset.hotkeySlot = -1;
                }

                preset.hotkeyAssigned = assignHotkeyCheckBox.Checked;
                preset.applyToSelectedTracks = useHotkeyForSelectedTracksCheckBox.Checked;


                preset.groupingNames = new string[groupingNames.Count];
                preset.functionNames = new string[functionNames.Count];
                preset.functionIds = new string[savedFunctionIds.Count];
                preset.functionTypes = new FunctionType[functionTypes.Count];
                preset.parameterNames = new string[parameterNames.Count];
                preset.parameter2Names = new string[parameter2Names.Count];

                groupingNames.CopyTo(preset.groupingNames, 0);
                functionNames.CopyTo(preset.functionNames, 0);
                savedFunctionIds.CopyTo(preset.functionIds, 0);
                functionTypes.CopyTo(preset.functionTypes, 0);
                parameterNames.CopyTo(preset.parameterNames, 0);
                parameter2Names.CopyTo(preset.parameter2Names, 0);

                preset.comparedField = comparedFieldList.Text;
                preset.conditionText = conditionList.Text;
                preset.conditionField = conditionFieldList.Text;
                preset.conditionIsChecked = conditionCheckBox.Checked;
                preset.totals = totalsCheckBox.Checked;

                preset.autoApply = presetsBox.GetItemChecked(presetsBox.SelectedIndex);

                preset.destinationTags = new string[savedDestinationTagsNames.Count];

                savedDestinationTagsNames.CopyTo(preset.destinationTags, 0);


                preset.operations = new int[sourceFieldComboBox.Items.Count];
                preset.mulDivFactors = new string[sourceFieldComboBox.Items.Count];
                preset.precisionDigits = new string[sourceFieldComboBox.Items.Count];
                preset.appendTexts = new string[sourceFieldComboBox.Items.Count];

                operations.CopyTo(preset.operations, 0);
                mulDivFactors.CopyTo(preset.mulDivFactors, 0);
                precisionDigits.CopyTo(preset.precisionDigits, 0);
                appendTexts.CopyTo(preset.appendTexts, 0);


                presetsBox.Refresh();
            }
        }

        private void buttonDeletePreset_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this, Plugin.MsgDeletePresetConfirmation, "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No)
                return;

            presetsBoxSelectedIndexChanged(presetsBox.SelectedIndex);

            if (presetsBox.SelectedIndex >= 0)
            {
                ignoreCheckedPresetEvent = false;

                if (presetsBox.GetItemChecked(presetsBox.SelectedIndex))
                    presetsBox.SetItemChecked(presetsBox.SelectedIndex, false);

                ignoreCheckedPresetEvent = true;

                if (((ReportPreset)presetsBox.SelectedItem).hotkeyAssigned)
                {
                    reportPresetHotkeyUsedSlots[((ReportPreset)presetsBox.SelectedItem).hotkeySlot] = false;
                    reportPresetsWithHotkeysCount--;
                }

                int presetsBoxSelectedIndex = presetsBox.SelectedIndex;
                presetsBox.Items.RemoveAt(presetsBox.SelectedIndex);

                if (presetsBox.Items.Count - 1 >= presetsBoxSelectedIndex)
                    presetsBox.SelectedIndex = presetsBoxSelectedIndex;
                else if (presetsBox.Items.Count > 0)
                    presetsBox.SelectedIndex = presetsBox.Items.Count - 1;
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
            if (presetsBox.SelectedIndex == -1)
                return;

            if (sourceFieldComboBox.SelectedIndex == -1)
            {
                if (idTextBox.Text != "")
                {
                    MessageBox.Show(this, Plugin.MsgFirstSelectWhichFieldYouWantToAssignFunctionIdTo, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    idTextBox.Text = "";
                }

                return;
            }

            if (idTextBox.Text == "")
            {
                savedFunctionIds[sourceFieldComboBox.SelectedIndex] = "";

                setPresetChanged();

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
                savedFunctionIds[sourceFieldComboBox.SelectedIndex] = "";
                idTextBox.Text = "";
            }
            else
            {
                foreach (ReportPreset preset in presetsBox.Items) //Lets iterate through other (saved) presets
                {
                    foreach (var id in preset.functionIds)
                    {
                        if (idTextBox.Text == id && preset != (ReportPreset)presetsBox.SelectedItem)
                        {
                            MessageBox.Show(this, Plugin.MsgPresetExists.Replace("%%ID%%", idTextBox.Text), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            savedFunctionIds[sourceFieldComboBox.SelectedIndex] = "";
                            idTextBox.Text = "";
                            return;
                        }
                    }
                }

                for (int i = 0; i < sourceFieldComboBox.Items.Count; i++) //Lets iterate through current preset
                {
                    string id = savedFunctionIds[i];

                    if (idTextBox.Text == id && i != sourceFieldComboBox.SelectedIndex)
                    {
                        MessageBox.Show(this, Plugin.MsgPresetExists.Replace("%%ID%%", idTextBox.Text), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        savedFunctionIds[sourceFieldComboBox.SelectedIndex] = "";
                        idTextBox.Text = "";
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
            idTextBox.Text = "";
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
            Close();
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, buttonPreview, buttonExport, buttonClose);
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            ApplyReportPreset((ReportPreset)presetsBox.SelectedItem);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LibraryReportsCommand_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (unsavedChanges)
            {
                MessageBoxDefaultButton lastAnswer = Plugin.SavedSettings.lrUnsavedChangesLastAnswer;
                DialogResult result = MessageBox.Show(this, Plugin.MsgLrDoYouWantToSaveChangesBeforeClosingTheWindow,
                    "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, lastAnswer);

                if (result == DialogResult.Yes)
                {
                    Plugin.SavedSettings.lrUnsavedChangesLastAnswer = MessageBoxDefaultButton.Button1;
                    saveSettings();
                }
                else if (result == DialogResult.No)
                {
                    Plugin.SavedSettings.lrUnsavedChangesLastAnswer = MessageBoxDefaultButton.Button2;
                }
                else if (result == DialogResult.Cancel)
                {
                    Plugin.SavedSettings.lrUnsavedChangesLastAnswer = MessageBoxDefaultButton.Button3;
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void LibraryReportsCommand_Load(object sender, EventArgs e)
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

        private void sourceTagList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (completelyIgnoreItemCheckEvent)
                return;


            ignorePresetChangedEvent = true;

            if (e.NewValue == CheckState.Checked)
            {
                if (!addColumn((string)sourceTagList.Items[e.Index], (string)parameter2ComboBox.SelectedItem, (FunctionType)functionComboBox.SelectedIndex))
                {
                    e.NewValue = CheckState.Unchecked;
                }
            }
            else
            {
                removeColumn((string)sourceTagList.Items[e.Index], (string)parameter2ComboBox.SelectedItem, (FunctionType)functionComboBox.SelectedIndex);
            }


            setPresetChanged();

            ignorePresetChangedEvent = false;
        }

        private void buttonUncheckAll_Click(object sender, EventArgs e)
        {
            resetLocalsAndUiControls();

            setPresetChanged();
        }

        private void buttonCheckAll_Click(object sender, EventArgs e)
        {
            addAllTags();

            setPresetChanged();
        }

        private void functionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (completelyIgnoreFunctionChangedEvent)
                return;

            completelyIgnoreItemCheckEvent = true;


            for (int i = 0; i < sourceTagList.Items.Count; i++)
            {
                sourceTagList.SetItemChecked(i, false);
            }


            if (functionComboBox.SelectedIndex == 0) //Groupings
            {
                for (int i = 0; i < groupingNames.Count; i++)
                {
                    for (int j = 0; j < sourceTagList.Items.Count; j++)
                    {
                        if (groupingNames[i] == (string)sourceTagList.Items[j])
                            sourceTagList.SetItemChecked(j, true);
                    }
                }
            }
            else //Functions
            {
                for (int j = 0; j < sourceTagList.Items.Count; j++)
                {
                    for (int i = 0; i < functionNames.Count; i++)
                    {
                        if (functionTypes[i] == FunctionType.Average || functionTypes[i] == FunctionType.AverageCount)
                        {
                            if (functionComboBox.SelectedIndex == (int)functionTypes[i] && parameterNames[i] == (string)sourceTagList.Items[j] && parameter2Names[i] == (string)parameter2ComboBox.SelectedItem)
                                sourceTagList.SetItemChecked(j, true);
                        }
                        else
                        {
                            if (functionComboBox.SelectedIndex == (int)functionTypes[i] && parameterNames[i] == (string)sourceTagList.Items[j])
                                sourceTagList.SetItemChecked(j, true);
                        }
                    }
                }
            }

            if (functionComboBox.SelectedIndex >= functionComboBox.Items.Count - 2)
            {
                label6.Visible = true;
                parameter2ComboBox.Visible = true;
            }
            else
            {
                label6.Visible = false;
                parameter2ComboBox.Visible = false;
            }

            completelyIgnoreItemCheckEvent = false;
        }

        private void presetsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (presetsBoxLastSelectedIndex == presetsBox.SelectedIndex)
                return;

            if (previewIsGenerated)
                clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, buttonPreview, buttonExport, buttonClose);

            presetIsLoaded = true;
            presetsBoxSelectedIndexChanged(presetsBox.SelectedIndex);
            presetsBoxLastSelectedIndex = presetsBox.SelectedIndex;
            presetIsLoaded = false;

        }

        private void presetsBoxSelectedIndexChanged(int index)
        {
            if (index == -1)
            {
                buttonApply.Enabled = false;


                conditionCheckBox.Enabled = false;
                conditionFieldList.Enabled = false;
                conditionList.Enabled = false;
                comparedFieldList.Enabled = false;
                buttonPreview.Enabled = false;
                buttonCopyPreset.Enabled = false;
                buttonDeletePreset.Enabled = false;
                sourceFieldComboBox.Enabled = false;
                destinationTagList.Enabled = false;
                buttonCheckAll.Enabled = false;
                buttonUncheckAll.Enabled = false;
                functionComboBox.Enabled = false;
                parameter2ComboBox.Enabled = false;
                sourceTagList.Enabled = false;
                previewTable.Enabled = false;

                conditionCheckBox.Checked = false;
                conditionFieldList.Text = "";
                conditionList.Text = "";
                comparedFieldList.Text = "";
                sourceFieldComboBox.Text = "";
                destinationTagList.Text = "";
                functionComboBox.Text = "";
                parameter2ComboBox.Text = "";
                sourceTagList.Text = "";

                idTextBox.Text = "";
                idTextBox.Enabled = false;

                presetNameTextBox.Enabled = false;

                labelFunction.Enabled = false;

                assignHotkeyCheckBox.Enabled = false;
                useHotkeyForSelectedTracksCheckBox.Enabled = false;
                totalsCheckBox.Enabled = false;

                presetNameTextBox.Text = "";
                assignHotkeyCheckBox.Checked = false;
                useHotkeyForSelectedTracksCheckBox.CheckState = CheckState.Unchecked;

                sourceFieldComboBox_SelectedIndexChanged(null, null);

                return;
            }


            resetLocalsAndUiControls();

            ReportPreset preset = (ReportPreset)presetsBox.SelectedItem;

            buttonApply.Enabled = true;

            conditionCheckBox.Enabled = true;
            buttonPreview.Enabled = true;
            buttonCopyPreset.Enabled = true;
            buttonDeletePreset.Enabled = preset.userPreset;
            sourceFieldComboBox.Enabled = true;
            destinationTagList.Enabled = true;
            buttonCheckAll.Enabled = preset.userPreset;
            buttonUncheckAll.Enabled = preset.userPreset;
            functionComboBox.Enabled = preset.userPreset;
            parameter2ComboBox.Enabled = true;
            sourceTagList.Enabled = preset.userPreset;
            previewTable.Enabled = true;

            presetNameTextBox.Enabled = true;
            presetNameTextBox.ReadOnly = !preset.userPreset;

            labelFunction.Enabled = preset.userPreset;

            assignHotkeyCheckBox.Checked = preset.hotkeyAssigned;
            useHotkeyForSelectedTracksCheckBox.Checked = preset.applyToSelectedTracks;
            useHotkeyForSelectedTracksCheckBox.Enabled = assignHotkeyCheckBox.Checked;
            totalsCheckBox.Enabled = true;
            totalsCheckBox.Checked = preset.totals;

            if (reportPresetsWithHotkeysCount < Plugin.MaximumNumberOfLRHotkeys)
                assignHotkeyCheckBox.Enabled = true;
            else if (reportPresetsWithHotkeysCount == Plugin.MaximumNumberOfLRHotkeys && preset.hotkeyAssigned)
                assignHotkeyCheckBox.Enabled = true;
            else
                assignHotkeyCheckBox.Enabled = false;


            if (ignorePresetChangedEvent)
                return;


            presetNameTextBox.Text = preset.name ?? "";

            completelyIgnoreFunctionChangedEvent = true;

            Plugin.FillListByTagNames(destinationTagList.Items);

            //Groupings
            functionComboBox.SelectedIndex = 0;
            for (int i = 0; i < preset.groupingNames.Length; i++)
            {
                for (int j = 0; j < sourceTagList.Items.Count; j++)
                {
                    if (preset.groupingNames[i] == (string)sourceTagList.Items[j])
                    {
                        sourceTagList.SetItemChecked(j, true);
                        break;
                    }
                }
            }

            //Functions
            for (int i = 0; i < preset.functionNames.Length; i++)
            {
                completelyIgnoreItemCheckEvent = true; //Lets clear items list which were set in previous iteration
                for (int j = 0; j < sourceTagList.Items.Count; j++)
                {
                    sourceTagList.SetItemChecked(j, false);
                }
                completelyIgnoreItemCheckEvent = false;


                functionComboBox.SelectedIndex = (int)preset.functionTypes[i];
                parameter2ComboBox.SelectedItem = preset.parameter2Names[i];


                for (int j = 0; j < sourceTagList.Items.Count; j++)
                {
                    if (preset.parameterNames[i] == (string)sourceTagList.Items[j])
                    {
                        sourceTagList.SetItemChecked(j, true);
                        break;
                    }
                }
            }


            savedFunctionIds.Clear();
            savedFunctionIds.AddRange(preset.functionIds);

            savedDestinationTagsNames.Clear();
            savedDestinationTagsNames.AddRange(preset.destinationTags);


            if (parameter2ComboBox.SelectedIndex == -1)
                parameter2ComboBox.SelectedItem = Plugin.MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Url);



            operations.Clear();
            operations.AddRange(preset.operations);
            mulDivFactors.Clear();
            mulDivFactors.AddRange(preset.mulDivFactors);
            precisionDigits.Clear();
            precisionDigits.AddRange(preset.precisionDigits);
            appendTexts.Clear();
            appendTexts.AddRange(preset.appendTexts);


            totalsCheckBox.Checked = preset.totals;
            conditionCheckBox.Checked = preset.conditionIsChecked;
            conditionFieldList.Text = preset.conditionField;

            conditionList.Text = preset.conditionText;
            if (conditionList.SelectedIndex == -1)
                conditionList.SelectedIndex = 0;

            comparedFieldList.Text = preset.comparedField;

            completelyIgnoreFunctionChangedEvent = false;

            functionComboBox.SelectedIndex = 0;

            if (sourceFieldComboBox.Items.Count > 0)
            {
                sourceFieldComboBox.SelectedIndex = 0;
                destinationTagList.SelectedValue = preset.destinationTags[0];
                idTextBox.Text = savedFunctionIds[0];
            }
            else
            {
                idTextBox.Enabled = false;
            }

            sourceFieldComboBox_SelectedIndexChanged(null, null);
        }

        private void conditionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            conditionFieldList.Enabled = conditionCheckBox.Checked;
            conditionList.Enabled = conditionCheckBox.Checked;
            comparedFieldList.Enabled = conditionCheckBox.Checked;

            setPresetChanged();
        }

        private void previewList_DataError(object sender, DataGridViewDataErrorEventArgs e)
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

        private void presetsBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (ignoreCheckedPresetEvent)
            {
                if (e.NewValue == CheckState.Checked)
                    e.NewValue = CheckState.Unchecked;
                else if (e.NewValue == CheckState.Unchecked)
                    e.NewValue = CheckState.Checked;

                return;
            }

            if (presetsBox.SelectedIndex != -1)
                setUnsavedChanges(true);

            if (e.NewValue == CheckState.Checked)
            {
                ((ReportPreset)presetsBox.Items[e.Index]).autoApply = true;
                autoAppliedPresetCount++;

                if (!Plugin.SavedSettings.dontPlayTickedAutoApplyingAsrLrPresetSound && presetsBox.SelectedIndex != -1)
                    System.Media.SystemSounds.Exclamation.Play();
            }
            else
            {
                ((ReportPreset)presetsBox.Items[e.Index]).autoApply = false;
                autoAppliedPresetCount--;
            }



            if (autoAppliedPresetCount == 0)
            {
                autoApplyInfoLabel.Text = autoApplyText;
                autoApplyInfoLabel.ForeColor = Plugin.UntickedColor;
            }
            else
            {
                autoApplyInfoLabel.Text = autoApplyText 
                    + nowTickedText.ToUpper().Replace("%%TICKEDPRESETS%%", autoAppliedPresetCount.ToString());
                autoApplyInfoLabel.ForeColor = Plugin.TickedColor;
            }
        }

        private void presetsBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (presetsBox.SelectedIndex == -1)
                return;

            if (e.X <= 16)
            {
                ignoreCheckedPresetEvent = false;
                presetsBox.SetItemChecked(presetsBox.SelectedIndex, !presetsBox.GetItemChecked(presetsBox.SelectedIndex));
                ignoreCheckedPresetEvent = true;
            }
        }

        private void presetNameTextBox_Leave(object sender, EventArgs e)
        {
            if (presetsBox.SelectedIndex == -1)
                return;

            setPresetChanged();
        }

        private void assignHotkeyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (presetsBox.SelectedIndex == -1)
                return;

            useHotkeyForSelectedTracksCheckBox.Enabled = assignHotkeyCheckBox.Checked;

            if (presetIsLoaded)
                return;

            if (assignHotkeyCheckBox.Checked)
                reportPresetsWithHotkeysCount++;
            else //if (!assignHotkeyCheckBox.Checked)
                reportPresetsWithHotkeysCount--;

            setPresetChanged();
        }

        private void useHotkeyForSelectedTracksCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (presetsBox.SelectedIndex == -1)
                return;

            setPresetChanged();
        }

        private void previewTable_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.ColumnIndex == artworkField)
                    return;

                Plugin.DataGridViewCellComparer comparator = new Plugin.DataGridViewCellComparer();

                comparator.comparedColumnIndex = e.ColumnIndex;

                for (int i = 0; i < previewTable.Columns.Count; i++)
                    previewTable.Columns[i].HeaderCell.SortGlyphDirection = SortOrder.None;

                previewTable.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Ascending;

                Plugin.SetStatusbarText(Plugin.LibraryReportsCommandSbText + " (" + Plugin.SbSorting + ")", false);
                previewTable.Sort(comparator);
                Plugin.SetStatusbarText("", false);
            }
        }

        private void resizeArtworkCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Plugin.SavedSettings.resizeArtwork = resizeArtworkCheckBox.Checked;

            xArworkSizeUpDown.Enabled = resizeArtworkCheckBox.Checked;
            yArworkSizeUpDown.Enabled = resizeArtworkCheckBox.Checked;
        }

        private void xArworkSizeUpDown_ValueChanged(object sender, EventArgs e)
        {
            Plugin.SavedSettings.xArtworkSize = (ushort)xArworkSizeUpDown.Value;
        }

        private void yArworkSizeUpDown_ValueChanged(object sender, EventArgs e)
        {
            Plugin.SavedSettings.yArtworkSize = (ushort)yArworkSizeUpDown.Value;
        }

        private void sourceFieldComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sourceFieldComboBox.SelectedIndex == -1)
            {
                sourceFieldComboBox.Enabled = false;
                destinationTagList.Enabled = false;
                idTextBox.Enabled = false;
                clearIdButton.Enabled = false;

                destinationTagList.SelectedIndex = -1;
                idTextBox.Text = "";


                operationComboBox.SelectedIndex = 0;
                mulDivFactorComboBox.SelectedIndex = 0;
                precisionDigitsComboBox.SelectedIndex = 0;
                appendTextBox.Text = "";

                operationComboBox.Enabled = false;
                mulDivFactorComboBox.Enabled = false;
                precisionDigitsComboBox.Enabled = false;
                appendTextBox.Enabled = false;
                roundToLabel.Enabled = false;
                digitsLabel.Enabled = false;
                appendLabel.Enabled = false;


                return;
            }


            sourceFieldComboBox.Enabled = true;
            destinationTagList.Enabled = true;
            idTextBox.Enabled = true;
            clearIdButton.Enabled = true;

            operationComboBox.Enabled = true;
            mulDivFactorComboBox.Enabled = true;
            precisionDigitsComboBox.Enabled = true;
            appendTextBox.Enabled = true;
            roundToLabel.Enabled = true;
            digitsLabel.Enabled = true;
            appendLabel.Enabled = true;

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
                appendTextBox.Text = "";
            }
            else
            {
                appendTexts[sourceFieldComboBox.SelectedIndex] = appendTextBox.Text;
            }

            if (!sourceFieldComboBoxIndexChanging)
                setPresetChanged();
        }
    }

    public abstract class ExportedDocument
    {
        protected Plugin tagToolsPlugin;
        protected System.IO.FileStream stream;
        protected Encoding unicode = Encoding.UTF8;
        protected string text = "";
        protected byte[] buffer;

        protected virtual void getHeader()
        {
            text = "";
        }

        protected virtual void getFooter()
        {
            text = "";
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
            text = "";
            stream.Write(buffer, 0, buffer.Length);
        }

        public void writeFooter()
        {
            getFooter();
            buffer = unicode.GetBytes(text);
            text = "";
            stream.Write(buffer, 0, buffer.Length);
        }

        public abstract void addCellToRow(string cell, string cellName, bool dontOutput1, bool dontOutput2);
        public abstract void addCellToRow(Bitmap cell, string cellName, string imageHash, int width, int height);

        protected abstract void getRow(int height);

        public virtual void writeRow(int height)
        {
            getRow(height);
            buffer = unicode.GetBytes(text);
            text = "";
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

        public override void addCellToRow(string cell, string cellName, bool dontOutput1, bool dontOutput2)
        {
            if (text != "")
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
            if (text != "")
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

        public override void addCellToRow(string cell, string cellName, bool dontOutput1, bool dontOutput2)
        {
            if (text != "")
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
            if (text != "")
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

        public override void addCellToRow(string cell, string cellName, bool dontOutput1, bool dontOutput2)
        {
            if (cellName == Plugin.UrlTagName)
            {
                base.addCellToRow(cell, cellName, dontOutput1, dontOutput2);
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

        public override void addCellToRow(string cell, string cellName, bool dontOutput1, bool dontOutput2)
        {
            text += "\t<td>" + cell + "</td>\r\n";
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

        public override void addCellToRow(string cell, string cellName, bool dontOutput1, bool dontOutput2)
        {
            string rowClass;

            if (alternateRow == null)
                rowClass = "xl0";
            else if ((bool)alternateRow)
                rowClass = "xl2";
            else
                rowClass = "xl3";


            text += "\t<td class=" + rowClass + ">" + cell + "</td>\r\n";
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

        public override void addCellToRow(string cell, string cellName, bool dontOutput1, bool dontOutput2)
        {
            if (dontOutput1 || dontOutput2)
                return;

            base.addCellToRow(cell, cellName, dontOutput1, dontOutput2);
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
                + "\r\n< table>\r\n";
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
            text = "";
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
