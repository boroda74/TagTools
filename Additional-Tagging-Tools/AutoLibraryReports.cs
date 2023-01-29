using MusicBeePlugin.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class AutoLibraryReportCommand : PluginWindowTemplate
    {
        public class AutoLibraryReportsPreset
        {
            public bool presetIsChecked = false;
            public string name = null;

            public string[] groupingNames = new string[0];
            public string[] functionNames = new string[0];
            public FunctionType[] functionTypes = new FunctionType[0];
            public string[] parameterNames = new string[0];
            public string[] parameter2Names = new string[0];

            public string[] sourceFeilds = new string[0];
            public string[] destinationTags = new string[0];
            public string[] ids = new string[0];

            public bool conditionIsChecked = false;
            public string[] conditionFields = new string[0];
            public string conditionField = null;
            public string condition = null;
            public string[] comparedFields = new string[0];
            public string comparedField = null;

            public override string ToString()
            {
                if (name != null)
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

        private class AggregatedTags : SortedDictionary<string, Plugin.ConvertStringsResults[]>
        {
            public void add(string url, string[] groupingValues, string[] functionValues, List<FunctionType> functionTypes, string[] parameter2Values)
            {
                string composedGroupings;
                if (groupingValues.Length == 0)
                {
                    composedGroupings = "";
                }
                else
                {
                    composedGroupings = String.Join("" + Plugin.MultipleItemsSplitterId, groupingValues);
                }


                if (!TryGetValue(composedGroupings, out Plugin.ConvertStringsResults[] aggregatedValues))
                {
                    aggregatedValues = new Plugin.ConvertStringsResults[functionValues.Length + 1];

                    for (int i = 0; i < functionValues.Length; i++)
                    {
                        aggregatedValues[i] = new Plugin.ConvertStringsResults(1);

                        if (functionTypes[i] == FunctionType.Minimum)
                            aggregatedValues[i].result1f = double.MaxValue;
                        else if (functionTypes[i] == FunctionType.Maximum)
                            aggregatedValues[i].result1f = double.MinValue;
                    }

                    aggregatedValues[functionValues.Length] = new Plugin.ConvertStringsResults(1);

                    Add(composedGroupings, aggregatedValues);
                }

                Plugin.ConvertStringsResults currentFunctionValue;

                for (int i = 0; i < functionValues.Length + 1; i++)
                {
                    if (i == functionValues.Length) //It is URLs
                    {
                        if (url != null)
                        {
                            if (!aggregatedValues[i].items.TryGetValue(url, out _))
                                aggregatedValues[i].items.Add(url, null);

                            aggregatedValues[i].type = 1;
                        }
                    }
                    else if (functionTypes[i] == FunctionType.Count)
                    {
                        if (!aggregatedValues[i].items.TryGetValue(functionValues[i], out _))
                            aggregatedValues[i].items.Add(functionValues[i], null);

                        aggregatedValues[i].type = 1;
                    }
                    else if (functionTypes[i] == FunctionType.Sum)
                    {
                        currentFunctionValue = Plugin.ConvertStrings(functionValues[i]);

                        if (currentFunctionValue.type != 0)
                        {
                            aggregatedValues[i].result1f += currentFunctionValue.result1f;

                            aggregatedValues[i].type = currentFunctionValue.type;
                        }
                    }
                    else if (functionTypes[i] == FunctionType.Minimum)
                    {
                        currentFunctionValue = Plugin.ConvertStrings(functionValues[i]);

                        if (currentFunctionValue.type != 0)
                        {
                            if (aggregatedValues[i].result1f > currentFunctionValue.result1f)
                                aggregatedValues[i].result1f = currentFunctionValue.result1f;

                            aggregatedValues[i].type = currentFunctionValue.type;
                        }
                    }
                    else if (functionTypes[i] == FunctionType.Maximum)
                    {
                        currentFunctionValue = Plugin.ConvertStrings(functionValues[i]);

                        if (currentFunctionValue.type != 0)
                        {
                            if (aggregatedValues[i].result1f < currentFunctionValue.result1f)
                                aggregatedValues[i].result1f = currentFunctionValue.result1f;

                            aggregatedValues[i].type = currentFunctionValue.type;
                        }
                    }
                    else if (functionTypes[i] == FunctionType.Average)
                    {
                        if (!aggregatedValues[i].items.TryGetValue(parameter2Values[i], out _))
                            aggregatedValues[i].items.Add(parameter2Values[i], null);

                        currentFunctionValue = Plugin.ConvertStrings(functionValues[i]);

                        if (currentFunctionValue.type != 0)
                        {
                            aggregatedValues[i].result1f += currentFunctionValue.result1f;

                            aggregatedValues[i].type = currentFunctionValue.type;
                        }
                    }
                    else if (functionTypes[i] == FunctionType.AverageCount)
                    {
                        if (!aggregatedValues[i].items.TryGetValue(parameter2Values[i], out _))
                            aggregatedValues[i].items.Add(parameter2Values[i], null);

                        if (!aggregatedValues[i].items1.TryGetValue(functionValues[i], out _))
                            aggregatedValues[i].items1.Add(functionValues[i], null);
                    }
                }
            }

            public static string GetField(KeyValuePair<string, Plugin.ConvertStringsResults[]> keyValue, int fieldNumber, List<string> groupingNames)
            {
                if (fieldNumber < groupingNames.Count)
                {
                    string field = keyValue.Key.Split(Plugin.MultipleItemsSplitterId)[fieldNumber];

                    if (field == Plugin.TotalsString)
                        field = (Plugin.MsgAllTags + " '" + groupingNames[fieldNumber] + "'").ToUpper();

                    return field;
                }
                else
                {
                    return keyValue.Value[fieldNumber - groupingNames.Count].getResult();
                }
            }

            public static string[] GetGroupings(KeyValuePair<string, Plugin.ConvertStringsResults[]> keyValue, List<string> groupingNames)
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

        private delegate void AddRowToTable(string[] row);
        private delegate void UpdateTable();
        private AddRowToTable addRowToTable;
        private UpdateTable updateTable;

        private string[] files = new string[0];
        private readonly AggregatedTags tags = new AggregatedTags();

        private readonly List<string> groupingNames = new List<string>();
        private readonly List<string> functionNames = new List<string>();
        private readonly List<FunctionType> functionTypes = new List<FunctionType>();
        private readonly List<string> parameterNames = new List<string>();
        private readonly List<string> parameter2Names = new List<string>();

        private int conditionField = -1;
        private int comparedField = -1;
        private readonly List<int> savedTagFields = new List<int>();
        private readonly List<Plugin.MetaDataType> destinationTagIds = new List<Plugin.MetaDataType>();

        private string conditionFieldText;
        private string conditionListText;
        private string comparedFieldText;

        private readonly List<string> savedDestinationTagList = new List<string>();
        private readonly List<string> savedIds = new List<string>();

        private bool ignorePresetChangedEvent = false;
        private bool completelyIgnoreItemCheckEvent = false;
        private bool completelyIgnoreFunctionChangedEvent = false;

        private static bool BackgroundTaskIsInProgress = false;

        private delegate object NewAutoLibraryReportsPluginDelegate();
        private static NewAutoLibraryReportsPluginDelegate NewAutoLibraryReportsPlugin;

        private static AutoLibraryReportCommand NewAutoLibraryReportsPluginFunction()
        {
            return new AutoLibraryReportCommand();
        }


        public AutoLibraryReportCommand()
        {
            InitializeComponent();
        }

        public AutoLibraryReportCommand(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;

            initializeForm();
        }

        public static void AutoCalculate(Plugin tagToolsPluginParam)
        {
            TagToolsPlugin = tagToolsPluginParam;

            if (BackgroundTaskIsInProgress)
                return;
            else
                BackgroundTaskIsInProgress = true;

            NewAutoLibraryReportsPlugin = NewAutoLibraryReportsPluginFunction;
            AutoLibraryReportCommand autoLibraryReportsCommand = (AutoLibraryReportCommand)Plugin.MbForm.Invoke(NewAutoLibraryReportsPlugin);
            Plugin.MbApiInterface.MB_CreateBackgroundTask(autoLibraryReportsCommand.autoCalculate, null);
        }

        public static string AutoCalculatePresetFunction(Plugin tagToolsPluginParam, string url, string functionId)
        {
            TagToolsPlugin = tagToolsPluginParam;

            if (Plugin.SavedSettings.autoLibraryReportsPresets.Length == 0)
                return "";

            lock (Plugin.SavedSettings.autoLibraryReportsPresets)
            {
                string value = "";

                foreach (var autoLibraryReportsPreset in Plugin.SavedSettings.autoLibraryReportsPresets)
                {
                    for (int m = 0; m < autoLibraryReportsPreset.ids.Length; m++)
                    {
                        if (autoLibraryReportsPreset.ids[m] != functionId)
                            continue;

                        List<string> groupingNames = new List<string>();
                        groupingNames.AddRange(autoLibraryReportsPreset.groupingNames);
                        string functionName = autoLibraryReportsPreset.functionNames[m];
                        FunctionType functionType = autoLibraryReportsPreset.functionTypes[m];
                        string parameterName = autoLibraryReportsPreset.parameterNames[m];
                        string parameter2Name = autoLibraryReportsPreset.parameter2Names[m];

                        string currentFile;
                        string tagValue;
                        string[] groupingValues;
                        string functionValue;
                        string parameter2Value;
                        Plugin.MetaDataType tagId;
                        Plugin.FilePropertyType propId;

                        Plugin.ConvertStringsResults aggregatedValues = new Plugin.ConvertStringsResults(1);

                        if (functionType == FunctionType.Minimum)
                            aggregatedValues.result1f = double.MaxValue;
                        else if (functionType == FunctionType.Maximum)
                            aggregatedValues.result1f = double.MinValue;


                        string query = @"<SmartPlaylist><Source Type=""1""><Conditions CombineMethod=""All"">";

                        for (int i = 0; i < groupingNames.Count; i++)
                        {
                            tagId = Plugin.GetTagId(groupingNames[i]);

                            if (groupingNames[i] == Plugin.SequenceNumberName)
                            {
                                //Nothing to do...
                            }
                            else
                            {
                                if (tagId == 0)
                                {
                                    propId = Plugin.GetPropId(groupingNames[i]);
                                    tagValue = Plugin.MbApiInterface.Library_GetFileProperty(url, propId);
                                }
                                else
                                {
                                    tagValue = Plugin.GetFileTag(url, tagId, true);
                                }

                                query += @"<Condition Field=""" + groupingNames[i] + @""" Comparison=""Is"" Value=""" + tagValue.Replace("\"", "&quot;") + @""" />";
                            }
                        }

                        query += "</Conditions></Source></SmartPlaylist>";


                        if (!Plugin.MbApiInterface.Library_QueryFilesEx(query, out string[] files))
                            return "";


                        if (files.Length == 0)
                            return "";


                        for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
                        {
                            currentFile = files[fileCounter];
                            groupingValues = new string[groupingNames.Count];

                            for (int i = 0; i < groupingNames.Count; i++)
                            {
                                tagId = Plugin.GetTagId(groupingNames[i]);

                                if (groupingNames[i] == Plugin.SequenceNumberName)
                                {
                                    tagValue = fileCounter.ToString("D6");
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
                                        propId = Plugin.GetPropId(groupingNames[i]);
                                        tagValue = Plugin.MbApiInterface.Library_GetFileProperty(currentFile, propId);
                                    }
                                    else
                                    {
                                        tagValue = Plugin.GetFileTag(currentFile, tagId, true);
                                    }
                                }


                                if (tagValue == "")
                                    tagValue = " ";

                                groupingValues[i] = tagValue;
                            }


                            tagId = Plugin.GetTagId(parameterName);

                            if (parameterName == Plugin.SequenceNumberName)
                            {
                                tagValue = fileCounter.ToString();
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
                                    propId = Plugin.GetPropId(parameterName);
                                    tagValue = Plugin.MbApiInterface.Library_GetFileProperty(currentFile, propId);
                                }
                                else
                                {
                                    tagValue = Plugin.GetFileTag(currentFile, tagId, true);
                                }
                            }

                            functionValue = tagValue;

                            if (functionType == FunctionType.Average || functionType == FunctionType.AverageCount)
                            {
                                tagId = Plugin.GetTagId(parameter2Name);

                                if (tagId == Plugin.ArtistArtistsId || tagId == Plugin.ComposerComposersId) //Lets make smart conversion of list of artists/composers
                                {

                                    tagValue = Plugin.GetFileTag(currentFile, tagId);
                                    tagValue = Plugin.GetTagRepresentation(tagValue);
                                }
                                else
                                {
                                    if (tagId == 0)
                                    {
                                        propId = Plugin.GetPropId(parameter2Name);
                                        tagValue = Plugin.MbApiInterface.Library_GetFileProperty(currentFile, propId);
                                    }
                                    else
                                    {
                                        tagValue = Plugin.GetFileTag(currentFile, tagId, true);
                                    }
                                }

                                parameter2Value = tagValue;
                            }
                            else
                            {
                                parameter2Value = null;
                            }


                            Plugin.ConvertStringsResults currentFunctionValue;


                            if (functionType == FunctionType.Count)
                            {
                                if (!aggregatedValues.items.TryGetValue(functionValue, out _))
                                    aggregatedValues.items.Add(functionValue, null);

                                aggregatedValues.type = 1;
                            }
                            else if (functionType == FunctionType.Sum)
                            {
                                currentFunctionValue = Plugin.ConvertStrings(functionValue);

                                aggregatedValues.result1f += currentFunctionValue.result1f;
                                aggregatedValues.type = currentFunctionValue.type;
                            }
                            else if (functionType == FunctionType.Minimum)
                            {
                                currentFunctionValue = Plugin.ConvertStrings(functionValue);

                                if (aggregatedValues.result1f > currentFunctionValue.result1f)
                                    aggregatedValues.result1f = currentFunctionValue.result1f;

                                aggregatedValues.type = currentFunctionValue.type;
                            }
                            else if (functionType == FunctionType.Maximum)
                            {
                                currentFunctionValue = Plugin.ConvertStrings(functionValue);

                                if (aggregatedValues.result1f < currentFunctionValue.result1f)
                                    aggregatedValues.result1f = currentFunctionValue.result1f;

                                aggregatedValues.type = currentFunctionValue.type;
                            }
                            else if (functionType == FunctionType.Average)
                            {
                                if (!aggregatedValues.items.TryGetValue(parameter2Value, out _))
                                    aggregatedValues.items.Add(parameter2Value, null);

                                currentFunctionValue = Plugin.ConvertStrings(functionValue);

                                aggregatedValues.result1f += currentFunctionValue.result1f;
                                aggregatedValues.type = currentFunctionValue.type;
                            }
                            else if (functionType == FunctionType.AverageCount)
                            {
                                if (!aggregatedValues.items.TryGetValue(parameter2Value, out _))
                                    aggregatedValues.items.Add(parameter2Value, null);

                                if (!aggregatedValues.items1.TryGetValue(functionValue, out _))
                                    aggregatedValues.items1.Add(functionValue, null);
                            }
                        }

                        value = aggregatedValues.getResult();
                        goto exit;
                    }
                }

            exit: return value;
            }
        }

        public void autoCalculate()
        {
            try
            {
                System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Lowest;

                if (Plugin.SavedSettings.autoLibraryReportsPresets.Length == 0)
                {
                    BackgroundTaskIsInProgress = false;
                    return;
                }


                bool delayExecution = true;

                while (delayExecution)
                {
                    bool thisCommandWindowIsOpened = false;

                    lock (Plugin.OpenedForms)
                    {
                        foreach (PluginWindowTemplate form in Plugin.OpenedForms)
                        {
                            if (form.GetType() == GetType())
                            {
                                thisCommandWindowIsOpened = true;
                                break;
                            }
                        }
                    }

                    if (thisCommandWindowIsOpened)
                        System.Threading.Thread.Sleep(10000);
                    else
                        delayExecution = false;
                }


                files = null;
                if (!Plugin.MbApiInterface.Library_QueryFilesEx("domain=Library", out files))
                    files = new string[0];


                if (files.Length == 0)
                {
                    BackgroundTaskIsInProgress = false;
                    return;
                }


                lock (Plugin.SavedSettings.autoLibraryReportsPresets)
                {
                    foreach (var autoLibraryReportsPreset in Plugin.SavedSettings.autoLibraryReportsPresets)
                    {
                        if (!autoLibraryReportsPreset.presetIsChecked)
                            continue;

                        groupingNames.Clear();
                        groupingNames.AddRange(autoLibraryReportsPreset.groupingNames);
                        functionNames.Clear();
                        functionNames.AddRange(autoLibraryReportsPreset.functionNames);
                        functionTypes.Clear();
                        functionTypes.AddRange(autoLibraryReportsPreset.functionTypes);
                        parameterNames.Clear();
                        parameterNames.AddRange(autoLibraryReportsPreset.parameterNames);
                        parameter2Names.Clear();
                        parameter2Names.AddRange(autoLibraryReportsPreset.parameter2Names);

                        previewTrackList(false);


                        conditionFieldText = autoLibraryReportsPreset.conditionField;
                        comparedFieldText = autoLibraryReportsPreset.comparedField;
                        conditionListText = autoLibraryReportsPreset.condition;

                        destinationTagIds.Clear();
                        for (int i = 0; i < autoLibraryReportsPreset.destinationTags.Length; i++)
                            destinationTagIds.Add(Plugin.GetTagId(autoLibraryReportsPreset.destinationTags[i]));

                        conditionField = -1;
                        comparedField = -1;


                        string columnHeader = "";

                        if (autoLibraryReportsPreset.conditionIsChecked)
                        {
                            for (int i = 0; i < groupingNames.Count; i++)
                            {
                                if (groupingNames[i] == conditionFieldText)
                                    conditionField = i;

                                if (groupingNames[i] == comparedFieldText)
                                    comparedField = i;
                            }


                            for (int i = 0; i < functionNames.Count; i++)
                            {
                                columnHeader = functionNames[i];

                                if (columnHeader == conditionFieldText)
                                    conditionField = groupingNames.Count + i;

                                if (columnHeader == comparedFieldText)
                                    comparedField = groupingNames.Count + i;
                            }
                        }


                        savedTagFields.Clear();
                        for (int i = 0; i < functionNames.Count; i++)
                        {
                            columnHeader = functionNames[i];

                            for (int j = 0; j < autoLibraryReportsPreset.sourceFeilds.Length; j++)
                            {
                                if (columnHeader == autoLibraryReportsPreset.sourceFeilds[j])
                                    savedTagFields.Add(groupingNames.Count + i);
                            }
                        }

                        saveFields(false);
                    }
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
                BackgroundTaskIsInProgress = false;
            }

            Plugin.RefreshPanels(true);

            Plugin.NumberOfTagChanges = 0;
            BackgroundTaskIsInProgress = false;
        }

        //public static void Display(bool modalForm = false)
        //{
        //    if (BackgroundTaskIsInProgress)
        //    {
        //        MessageBox.Show(Plugin.MbForm, TagToolsPlugin.msgBackgroundAutoLibraryReportIsExecuted);
        //        return;
        //    }

        //    PluginWindowTemplate.Display(modalForm);
        //}

        protected new void initializeForm()
        {
            base.initializeForm();

            float avgForeBrightness = (ForeColor.R + ForeColor.G + ForeColor.B) / 3.0f;
            if (avgForeBrightness > 0.5f)
            {
                clearIdButton.Image = Resources.clear_button_light;
            }


            float highlightWeight = 0.8f;//***
            Color highlightColor = SystemColors.Highlight;

            labelNotSaved.ForeColor = Plugin.GetHighlightColor(highlightColor, BackColor, highlightWeight);


            functionComboBox.Items.Add(Plugin.GroupingName);
            functionComboBox.Items.Add(Plugin.CountName);
            functionComboBox.Items.Add(Plugin.SumName);
            functionComboBox.Items.Add(Plugin.MinimumName);
            functionComboBox.Items.Add(Plugin.MaximumName);
            functionComboBox.Items.Add(Plugin.AverageName);
            functionComboBox.Items.Add(Plugin.AverageCountName);

            Plugin.FillList(sourceTagList.Items, true);
            Plugin.FillListWithProps(sourceTagList.Items);
            sourceTagList.Items.Add(Plugin.SequenceNumberName);

            Plugin.FillList(parameter2ComboBox.Items, true);
            Plugin.FillListWithProps(parameter2ComboBox.Items);

            conditionList.Items.Add(Plugin.ListItemConditionIs);
            conditionList.Items.Add(Plugin.ListItemConditionIsNot);
            conditionList.Items.Add(Plugin.ListItemConditionIsGreater);
            conditionList.Items.Add(Plugin.ListItemConditionIsLess);

            presetsBox.Items.AddRange(Plugin.SavedSettings.autoLibraryReportsPresets);
            for (int i = 0; i < Plugin.SavedSettings.autoLibraryReportsPresets.Length; i++)
            {
                if (Plugin.SavedSettings.autoLibraryReportsPresets[i].presetIsChecked)
                    presetsBox.SetItemChecked(i, true);

            }

            presetsBox.SelectedIndex = -1;

            Plugin.FillList(destinationTagList.Items);

            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            presetsBox_SelectedIndexChanged(null, null);

            recalculateOnNumberOfTagsChangesCheckBox.Checked = Plugin.SavedSettings.recalculateOnNumberOfTagsChanges;
            numberOfTagsToRecalculateNumericUpDown.Value = Plugin.SavedSettings.numberOfTagsToRecalculate;

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

        private void previewList_AddRowToTable(string[] row)
        {
            previewTable.Rows.Add(row);
        }

        private void previewList_updateTable()
        {
            sourceTagList.Enabled = true;
            buttonCheckAll.Enabled = true;
            buttonUncheckAll.Enabled = true;

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
                MessageBox.Show(this, Plugin.MsgPleaseUseGroupingFunctionForArtworkTag, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }


            DataGridViewTextBoxColumn textColumnTemplate = new DataGridViewTextBoxColumn();
            column = new DataGridViewColumn(textColumnTemplate.CellTemplate)
            {
                SortMode = DataGridViewColumnSortMode.Programmatic
            };

            column.HeaderCell.Style.WrapMode = DataGridViewTriState.True;


            column.HeaderText = GetColumnName(fieldName, parameter2Name, type);


            conditionFieldList.Items.Add(column.HeaderText);
            if (conditionFieldList.SelectedIndex == -1)
                conditionFieldList.SelectedIndex = 0;

            comparedFieldList.Items.Add(column.HeaderText);


            if (type == FunctionType.Grouping)
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


                sourceFieldComboBox.Items.Add(column.HeaderText);
                savedDestinationTagList.Add((string)destinationTagList.Items[0]);
                savedIds.Add("");
                idTextBox.Text = "";

                if (sourceFieldComboBox.SelectedIndex == -1)
                    sourceFieldComboBox.SelectedIndex = 0;
            }


            if (previewTable.Rows.Count > 0)
                clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, buttonPreview, Plugin.EmptyButton, buttonCancel);

            return true;
        }

        private void removeColumn(string fieldName, string parameter2Name, FunctionType type)
        {
            string columnHeader = GetColumnName(fieldName, parameter2Name, type);

            if (groupingNames.Contains(columnHeader))
            {
                groupingNames.Remove(columnHeader);
            }
            else
            {
                int index = functionNames.IndexOf(columnHeader);

                functionNames.RemoveAt(index);
                functionTypes.RemoveAt(index);
                parameterNames.RemoveAt(index);
                parameter2Names.RemoveAt(index);

                savedDestinationTagList.RemoveAt(sourceFieldComboBox.SelectedIndex);
                savedIds.RemoveAt(sourceFieldComboBox.SelectedIndex);
                idTextBox.Text = "";

                sourceFieldComboBox.Items.Remove(columnHeader);
                if (sourceFieldComboBox.SelectedIndex == -1 && sourceFieldComboBox.Items.Count > 0)
                    sourceFieldComboBox.SelectedIndex = 0;
            }


            conditionFieldList.Items.Remove(columnHeader);
            if (conditionFieldList.SelectedIndex == -1 && conditionFieldList.Items.Count > 0)
                conditionFieldList.SelectedIndex = 0;


            if (comparedFieldList.Text == (string)comparedFieldList.SelectedValue)
                comparedFieldList.Text = "";

            comparedFieldList.Items.Remove(columnHeader);


            foreach (DataGridViewColumn column in previewTable.Columns)
            {
                if (column.HeaderText == columnHeader)
                {
                    previewTable.Columns.RemoveAt(column.Index);

                    if (previewTable.Columns.Count == 0)
                        clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, buttonPreview, Plugin.EmptyButton, buttonCancel);

                    return;
                }
            }

            if (previewTable.Columns.Count == 0)
                clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, buttonPreview, Plugin.EmptyButton, buttonCancel);
        }

        private void clearList()
        {
            completelyIgnoreItemCheckEvent = true;

            for (int i = 0; i < sourceTagList.Items.Count; i++)
            {
                sourceTagList.SetItemChecked(i, false);
            }

            previewTable.Columns.Clear();

            conditionField = -1;
            comparedField = -1;
            savedTagFields.Clear();

            sourceFieldComboBox.Items.Clear();
            conditionFieldList.Items.Clear();
            comparedFieldList.Items.Clear();

            groupingNames.Clear();
            functionNames.Clear();
            functionTypes.Clear();
            parameterNames.Clear();
            parameter2Names.Clear();

            completelyIgnoreItemCheckEvent = false;

            functionComboBox.SelectedIndex = 0;

            clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, Plugin.EmptyButton, buttonPreview, buttonCancel);
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

            files = null;
            if (!Plugin.MbApiInterface.Library_QueryFilesEx("domain=Library", out files))
                files = new string[0];

            if (files.Length == 0)
            {
                MessageBox.Show(this, Plugin.MsgNoFilesInCurrentView, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }


            sourceTagList.Enabled = false;
            buttonCheckAll.Enabled = false;
            buttonUncheckAll.Enabled = false;

            return true;
        }

        private void previewTrackList()
        {
            previewTrackList(true);
        }

        private void previewTrackList(bool interactive)
        {
            string currentFile;
            string tagValue;
            string[] groupingValues;
            string[] functionValues;
            string[] parameter2Values;
            Plugin.MetaDataType tagId;
            Plugin.FilePropertyType propId;


            tags.Clear();

            if (functionNames.Count == 0)
                return;

            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (backgroundTaskIsCanceled)
                {
                    if (interactive)
                        Invoke(updateTable);

                    return;
                }

                currentFile = files[fileCounter];

                Plugin.SetStatusbarTextForFileOperations(Plugin.LibraryReportsCommandSbText, true, fileCounter, files.Length, currentFile);


                groupingValues = new string[groupingNames.Count];

                for (int i = 0; i < groupingNames.Count; i++)
                {
                    tagId = Plugin.GetTagId(groupingNames[i]);

                    if (groupingNames[i] == Plugin.SequenceNumberName)
                    {
                        tagValue = fileCounter.ToString("D6");
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
                            propId = Plugin.GetPropId(groupingNames[i]);
                            tagValue = Plugin.MbApiInterface.Library_GetFileProperty(currentFile, propId);
                        }
                        else
                        {
                            tagValue = Plugin.GetFileTag(currentFile, tagId, true);
                        }
                    }


                    if (tagValue == "")
                        tagValue = " ";

                    groupingValues[i] = tagValue;
                }


                functionValues = new string[functionNames.Count];
                parameter2Values = new string[functionNames.Count];


                for (int i = 0; i < functionNames.Count; i++)
                {
                    int parameterIndex = functionNames.IndexOf(parameterNames[i]);

                    tagId = Plugin.GetTagId(parameterNames[i]);

                    if (parameterNames[i] == Plugin.SequenceNumberName)
                    {
                        tagValue = fileCounter.ToString();
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
                            propId = Plugin.GetPropId(parameterNames[i]);
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
                                propId = Plugin.GetPropId(parameter2Names[i]);
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


                tags.add(currentFile, groupingValues, functionValues, functionTypes, parameter2Values);
            }


            int k = 0;
            foreach (KeyValuePair<string, Plugin.ConvertStringsResults[]> keyValue in tags)
            {
                if (backgroundTaskIsCanceled)
                {
                    if (interactive)
                        Invoke(updateTable);

                    return;
                }

                k++;
                Plugin.SetStatusbarTextForFileOperations(Plugin.LibraryReportsGneratingPreviewCommandSbText, true, k, tags.Count);

                string[] groupingsRow = AggregatedTags.GetGroupings(keyValue, groupingNames);
                string[] row = new string[groupingsRow.Length + keyValue.Value.Length];

                groupingsRow.CopyTo(row, 0);

                for (int i = groupingsRow.Length; i < row.Length; i++)
                {
                    row[i] = AggregatedTags.GetField(keyValue, i, groupingNames);
                }

                if (interactive)
                    Invoke(addRowToTable, new Object[] { row });

                previewIsGenerated = true;
            }

            Plugin.SetStatusbarTextForFileOperations(Plugin.LibraryReportsGneratingPreviewCommandSbText, true, tags.Count - 1, tags.Count, null, true);

            if (interactive)
                Invoke(updateTable);
        }

        private bool checkCondition(KeyValuePair<string, Plugin.ConvertStringsResults[]> keyValue)
        {
            if (conditionField == -1)
                return true;

            string value = AggregatedTags.GetField(keyValue, conditionField, groupingNames);
            string comparedValue;


            if (comparedField == -1)
                comparedValue = comparedFieldText;
            else
                comparedValue = AggregatedTags.GetField(keyValue, comparedField, groupingNames);


            if (conditionListText == Plugin.ListItemConditionIs)
            {
                if (value == comparedValue)
                    return true;
                else
                    return false;
            }
            else if (conditionListText == Plugin.ListItemConditionIsNot)
            {
                if (value != comparedValue)
                    return true;
                else
                    return false;
            }
            else if (conditionListText == Plugin.ListItemConditionIsGreater)
            {
                if (Plugin.CompareStrings(value, comparedValue) == 1)
                    return true;
                else
                    return false;
            }
            else if (conditionListText == Plugin.ListItemConditionIsLess)
            {
                if (Plugin.CompareStrings(value, comparedValue) == -1)
                    return true;
                else
                    return false;
            }

            return true;
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


            conditionFieldText = conditionList.Text;
            comparedFieldText = comparedFieldList.Text;

            destinationTagIds.Clear();
            for (int i = 0; i < destinationTagList.Items.Count; i++)
            {
                destinationTagIds.Add(Plugin.GetTagId((string)destinationTagList.Items[i]));
            }

            conditionField = -1;
            comparedField = -1;


            string columnHeader;

            if (conditionCheckBox.Checked)
            {
                for (int i = 0; i < groupingNames.Count; i++)
                {
                    if (groupingNames[i] == conditionFieldList.Text)
                        conditionField = i;

                    if (groupingNames[i] == comparedFieldList.Text)
                        comparedField = i;
                }


                for (int i = 0; i < functionNames.Count; i++)
                {
                    columnHeader = functionNames[i];

                    if (columnHeader == conditionFieldList.Text)
                        conditionField = groupingNames.Count + i;

                    if (columnHeader == comparedFieldList.Text)
                        comparedField = groupingNames.Count + i;
                }
            }


            for (int i = 0; i < functionNames.Count; i++)
            {
                columnHeader = functionNames[i];

                for (int j = 0; j < sourceFieldComboBox.Items.Count; j++)
                {
                    if (columnHeader == (string)sourceFieldComboBox.Items[j])
                        savedTagFields.Add(groupingNames.Count + i);
                }
            }


            if (checkForFunctions)
            {
                sourceTagList.Enabled = false;
                buttonCheckAll.Enabled = false;
                buttonUncheckAll.Enabled = false;
            }

            return true;
        }

        private void saveFields(bool interactive = true)
        {
            SortedDictionary<string, object> urls;
            int i = 0;

            foreach (KeyValuePair<string, Plugin.ConvertStringsResults[]> keyValue in tags)
            {
                i++;

                if (backgroundTaskIsCanceled)
                    return;

                if (checkCondition(keyValue))
                {
                    urls = keyValue.Value[keyValue.Value.Length - 1].items;

                    foreach (KeyValuePair<string, object> keyValueUrls in urls)
                    {
                        Plugin.SetStatusbarTextForFileOperations(Plugin.LibraryReportsCommandSbText, false, i, tags.Count, keyValueUrls.Key);

                        for (int j = 0; j < savedTagFields.Count; j++)
                        {
                            Plugin.SetFileTag(keyValueUrls.Key, destinationTagIds[j], AggregatedTags.GetField(keyValue, savedTagFields[j], groupingNames));
                        }

                        Plugin.CommitTagsToFile(keyValueUrls.Key, false, false);
                    }
                }
            }

            if (interactive)
                Invoke(updateTable);

            //TagToolsPlugin.refreshPanels(true);

            Plugin.SetStatusbarTextForFileOperations(Plugin.LibraryReportsCommandSbText, false, tags.Count - 1, tags.Count, null, true);
        }

        private void saveSettings()
        {
            Plugin.SavedSettings.recalculateOnNumberOfTagsChanges = recalculateOnNumberOfTagsChangesCheckBox.Checked;
            Plugin.SavedSettings.numberOfTagsToRecalculate = numberOfTagsToRecalculateNumericUpDown.Value;

            for (int i = 0; i < presetsBox.Items.Count; i++)
            {
                ((AutoLibraryReportsPreset)presetsBox.Items[i]).presetIsChecked = presetsBox.GetItemChecked(i);
            }

            Plugin.SavedSettings.autoLibraryReportsPresets = new AutoLibraryReportsPreset[presetsBox.Items.Count];
            presetsBox.Items.CopyTo(Plugin.SavedSettings.autoLibraryReportsPresets, 0);
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            if (functionNames.Count == 0)
                return;

            clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, (Button)sender, Plugin.EmptyButton, buttonCancel);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            saveSettings();
            Close();
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
            buttonPreview.Enabled = true;
        }

        public override void disableQueryingOrUpdatingButtons()
        {
            buttonPreview.Enabled = false;
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



            ignorePresetChangedEvent = false;

            labelNotSaved.Visible = true;
        }

        private void buttonUncheckAll_Click(object sender, EventArgs e)
        {
            clearList();
            buttonUpdatePreset_Click(null, null);
        }

        private void buttonCheckAll_Click(object sender, EventArgs e)
        {
            addAllTags();

            labelNotSaved.Visible = true;
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

        private void checkBoxCondition_CheckedChanged(object sender, EventArgs e)
        {
            conditionFieldList.Enabled = conditionCheckBox.Checked;
            conditionList.Enabled = conditionCheckBox.Checked;
            comparedFieldList.Enabled = conditionCheckBox.Checked;

            labelNotSaved.Visible = true;
        }

        private void previewList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //e.ThrowException = false;
        }

        private void previewTable_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Plugin.DataGridViewCellComparer comparator = new Plugin.DataGridViewCellComparer
                {
                    comparedColumnIndex = e.ColumnIndex
                };

                for (int i = 0; i < previewTable.Columns.Count; i++)
                    previewTable.Columns[i].HeaderCell.SortGlyphDirection = SortOrder.None;

                previewTable.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Ascending;

                Plugin.SetStatusbarText(Plugin.LibraryReportsCommandSbText + " (" + Plugin.SbSorting + ")", true);
                previewTable.Sort(comparator);
            }
        }

        private void presetsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (presetsBox.SelectedIndex == -1)
            {
                conditionCheckBox.Enabled = false;
                conditionFieldList.Enabled = false;
                conditionList.Enabled = false;
                comparedFieldList.Enabled = false;
                buttonPreview.Enabled = false;
                sourceFieldComboBox.Enabled = false;
                destinationTagList.Enabled = false;
                buttonCheckAll.Enabled = false;
                buttonUncheckAll.Enabled = false;
                functionComboBox.Enabled = false;
                parameter2ComboBox.Enabled = false;
                sourceTagList.Enabled = false;
                previewTable.Enabled = false;

                buttonUpdatePreset.Enabled = false;
                buttonDeletePreset.Enabled = false;

                idTextBox.Enabled = false;

                fieldComboBox_SelectedIndexChanged(null, null);

                return;
            }
            else
            {
                conditionCheckBox.Enabled = true;
                buttonPreview.Enabled = true;
                sourceFieldComboBox.Enabled = true;
                destinationTagList.Enabled = true;
                buttonCheckAll.Enabled = true;
                buttonUncheckAll.Enabled = true;
                functionComboBox.Enabled = true;
                parameter2ComboBox.Enabled = true;
                sourceTagList.Enabled = true;
                previewTable.Enabled = true;

                buttonUpdatePreset.Enabled = true;
                buttonDeletePreset.Enabled = true;
            }

            if (ignorePresetChangedEvent)
                return;


            AutoLibraryReportsPreset autoLibraryReportsPreset = (AutoLibraryReportsPreset)presetsBox.SelectedItem;

            clearList();

            completelyIgnoreFunctionChangedEvent = true;

            //Groupings
            functionComboBox.SelectedIndex = 0;
            for (int i = 0; i < autoLibraryReportsPreset.groupingNames.Length; i++)
            {
                for (int j = 0; j < sourceTagList.Items.Count; j++)
                {
                    if (autoLibraryReportsPreset.groupingNames[i] == (string)sourceTagList.Items[j])
                    {
                        sourceTagList.SetItemChecked(j, true);
                        break;
                    }
                }
            }

            //Functions
            for (int i = 0; i < autoLibraryReportsPreset.functionNames.Length; i++)
            {
                functionComboBox.SelectedIndex = (int)autoLibraryReportsPreset.functionTypes[i];

                parameter2ComboBox.SelectedItem = autoLibraryReportsPreset.parameter2Names[i];

                completelyIgnoreItemCheckEvent = true; //Lets clear items list which were set in previous iteration
                for (int j = 0; j < sourceTagList.Items.Count; j++)
                {
                    sourceTagList.SetItemChecked(j, false);
                }
                completelyIgnoreItemCheckEvent = false;


                for (int j = 0; j < sourceTagList.Items.Count; j++)
                {
                    if (autoLibraryReportsPreset.parameterNames[i] == (string)sourceTagList.Items[j])
                    {
                        sourceTagList.SetItemChecked(j, true);
                        break;
                    }
                }
            }

            if (parameter2ComboBox.SelectedIndex == -1)
                parameter2ComboBox.SelectedItem = Plugin.MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Url);

            destinationTagList.Items.Clear();
            Plugin.FillList(destinationTagList.Items);



            sourceFieldComboBox.Items.Clear();
            savedDestinationTagList.Clear();
            savedIds.Clear();
            for (int i = 0; i < autoLibraryReportsPreset.sourceFeilds.Length; i++)
            {
                sourceFieldComboBox.Items.Add(autoLibraryReportsPreset.sourceFeilds[i]);
                savedDestinationTagList.Add(autoLibraryReportsPreset.destinationTags[i]);
                savedIds.Add(autoLibraryReportsPreset.ids[i]);
            }


            conditionCheckBox.Checked = autoLibraryReportsPreset.conditionIsChecked;

            conditionFieldList.Text = autoLibraryReportsPreset.conditionField;

            conditionList.Text = autoLibraryReportsPreset.condition;
            if (conditionList.SelectedIndex == -1)
                conditionList.SelectedIndex = 0;

            comparedFieldList.Text = autoLibraryReportsPreset.comparedField;

            completelyIgnoreFunctionChangedEvent = false;

            functionComboBox.SelectedIndex = 0;

            if (sourceFieldComboBox.Items.Count > 0)
            {
                sourceFieldComboBox.SelectedIndex = 0;
                destinationTagList.SelectedValue = savedDestinationTagList[0];
                idTextBox.Text = savedIds[0];
            }
            else
            {
                idTextBox.Enabled = false;
            }

            fieldComboBox_SelectedIndexChanged(null, null);

            labelNotSaved.Visible = false;
        }

        private void fieldComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sourceFieldComboBox.SelectedIndex == -1)
            {
                destinationTagList.Enabled = false;
                idTextBox.Enabled = false;
                clearIdButton.Enabled = false;
                return;
            }
            else
            {
                destinationTagList.Enabled = true;
                idTextBox.Enabled = true;
                clearIdButton.Enabled = true;
            }

            destinationTagList.Text = savedDestinationTagList[sourceFieldComboBox.SelectedIndex];
            idTextBox.Text = savedIds[sourceFieldComboBox.SelectedIndex];

            labelNotSaved.Visible = true;
        }

        private void destinationTagList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sourceFieldComboBox.Items.Count != 0)
            {
                savedDestinationTagList[sourceFieldComboBox.SelectedIndex] = destinationTagList.Text;

                labelNotSaved.Visible = true;
            }
        }

        private void buttonAddPreset_Click(object sender, EventArgs e)
        {
            presetsBox.Items.Add(new AutoLibraryReportsPreset());
        }

        private void buttonDeletePreset_Click(object sender, EventArgs e)
        {
            if (presetsBox.SelectedIndex >= 0)
            {
                int presetsBoxSelectedIndex = presetsBox.SelectedIndex;
                presetsBox.Items.RemoveAt(presetsBox.SelectedIndex);

                if (presetsBox.Items.Count - 1 >= presetsBoxSelectedIndex)
                    presetsBox.SelectedIndex = presetsBoxSelectedIndex;
                else if (presetsBox.Items.Count > 0)
                    presetsBox.SelectedIndex = presetsBox.Items.Count - 1;
                else
                    clearList();
            }
        }

        private void buttonUpdatePreset_Click(object sender, EventArgs e)
        {
            if (presetsBox.SelectedIndex >= 0)
            {
                var autoLibraryReportsPreset = (AutoLibraryReportsPreset)presetsBox.SelectedItem;

                autoLibraryReportsPreset.groupingNames = new string[groupingNames.Count];
                autoLibraryReportsPreset.functionNames = new string[functionNames.Count];
                autoLibraryReportsPreset.functionTypes = new FunctionType[functionTypes.Count];
                autoLibraryReportsPreset.parameterNames = new string[parameterNames.Count];
                autoLibraryReportsPreset.parameter2Names = new string[parameter2Names.Count];

                groupingNames.CopyTo(autoLibraryReportsPreset.groupingNames, 0);
                functionNames.CopyTo(autoLibraryReportsPreset.functionNames, 0);
                functionTypes.CopyTo(autoLibraryReportsPreset.functionTypes, 0);
                parameterNames.CopyTo(autoLibraryReportsPreset.parameterNames, 0);
                parameter2Names.CopyTo(autoLibraryReportsPreset.parameter2Names, 0);

                autoLibraryReportsPreset.comparedField = comparedFieldList.Text;
                autoLibraryReportsPreset.condition = conditionList.Text;
                autoLibraryReportsPreset.conditionField = conditionFieldList.Text;
                autoLibraryReportsPreset.conditionIsChecked = conditionCheckBox.Checked;

                autoLibraryReportsPreset.comparedFields = new string[comparedFieldList.Items.Count];
                autoLibraryReportsPreset.conditionFields = new string[conditionFieldList.Items.Count];

                comparedFieldList.Items.CopyTo(autoLibraryReportsPreset.comparedFields, 0);
                conditionFieldList.Items.CopyTo(autoLibraryReportsPreset.conditionFields, 0);

                autoLibraryReportsPreset.presetIsChecked = presetsBox.GetItemChecked(presetsBox.SelectedIndex);

                autoLibraryReportsPreset.sourceFeilds = new string[sourceFieldComboBox.Items.Count];
                autoLibraryReportsPreset.destinationTags = new string[sourceFieldComboBox.Items.Count];
                autoLibraryReportsPreset.ids = new string[sourceFieldComboBox.Items.Count];
                for (int i = 0; i < sourceFieldComboBox.Items.Count; i++)
                {
                    autoLibraryReportsPreset.sourceFeilds[i] = (string)sourceFieldComboBox.Items[i];
                    autoLibraryReportsPreset.destinationTags[i] = savedDestinationTagList[i];
                    autoLibraryReportsPreset.ids[i] = savedIds[i];
                }

                presetsBox.Refresh();

                labelNotSaved.Visible = false;
            }
        }

        private void recalculateOnNumberOfTagsChangesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            numberOfTagsToRecalculateNumericUpDown.Enabled = recalculateOnNumberOfTagsChangesCheckBox.Checked;
        }

        private void conditionFieldList_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelNotSaved.Visible = true;
        }

        private void conditionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelNotSaved.Visible = true;
        }

        private void comparedFieldList_TextUpdate(object sender, EventArgs e)
        {
            labelNotSaved.Visible = true;
        }

        private void idTextBox_Leave(object sender, EventArgs e)//*****
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
                savedIds[sourceFieldComboBox.SelectedIndex] = "";
                labelNotSaved.Visible = true;
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
                savedIds[sourceFieldComboBox.SelectedIndex] = "";
                idTextBox.Text = "";
            }
            else
            {
                foreach (AutoLibraryReportsPreset tempPreset in presetsBox.Items) //Lets iterate through other (saved) presets
                {
                    foreach (var id in tempPreset.ids)
                    {
                        if (idTextBox.Text == id && tempPreset != (AutoLibraryReportsPreset)presetsBox.SelectedItem)
                        {
                            MessageBox.Show(this, Plugin.MsgPresetExists.Replace("%ID%", idTextBox.Text), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            savedIds[sourceFieldComboBox.SelectedIndex] = "";
                            idTextBox.Text = "";
                            return;
                        }
                    }
                }

                for (int i = 0; i < sourceFieldComboBox.Items.Count; i++) //Lets iterate through current preset
                {
                    string id = savedIds[i];

                    if (idTextBox.Text == id && i != sourceFieldComboBox.SelectedIndex)
                    {
                        MessageBox.Show(this, Plugin.MsgPresetExists.Replace("%ID%", idTextBox.Text), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        savedIds[sourceFieldComboBox.SelectedIndex] = "";
                        idTextBox.Text = "";
                        return;
                    }
                }

                savedIds[sourceFieldComboBox.SelectedIndex] = idTextBox.Text;
                labelNotSaved.Visible = true;
            }
        }

        private void clearIdButton_Click(object sender, EventArgs e)
        {
            idTextBox.Text = "";
            idTextBox_Leave(null, null);
        }
    }
}
