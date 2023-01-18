using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class LibraryReportsCommand : PluginWindowTemplate
    {
        public class LibraryReportsPreset
        {
            public string name = null;
            public string[] groupingNames;
            public string[] functionNames;
            public FunctionType[] functionTypes;
            public string[] parameterNames;
            public string[] parameter2Names;
            public bool totals;

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


                return representation;
            }
        }

        private class AggregatedTags : SortedDictionary<string, Plugin.ConvertStringsResults[]>
        {
            public void add(string url, string[] groupingValues, string[] functionValues, List<FunctionType> functionTypes, string[] parameter2Values, bool totals)
            {
                string composedGroupings;
                Plugin.ConvertStringsResults[] aggregatedValues;


                if (groupingValues.Length == 0)
                {
                    composedGroupings = "";
                }
                else
                {
                    composedGroupings = String.Join("" + Plugin.MultipleItemsSplitterId, groupingValues);

                    if (totals)
                        for (int i = groupingValues.Length - 1; i >= 0; i--)
                        {
                            groupingValues[i] = Plugin.TotalsString;
                            add(null, groupingValues, functionValues, functionTypes, parameter2Values, false);
                        }
                }


                if (!TryGetValue(composedGroupings, out aggregatedValues))
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
                    if (i == functionValues.Length) //It are URLs
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
        private AggregatedTags tags = new AggregatedTags();

        public static string PresetName = "";
        private List<string> groupingNames = new List<string>();
        private List<string> functionNames = new List<string>();
        private List<FunctionType> functionTypes = new List<FunctionType>();
        private List<string> parameterNames = new List<string>();
        private List<string> parameter2Names = new List<string>();

        private int conditionField = -1;
        private int comparedField = -1;
        private int savedTagField = -1;
        private int artworkField = -1;

        private string conditionListText;
        private string conditionValueListText;

        private bool totals;

        public static Bitmap DefaultArtwork;
        public static string DefaultArtworkHash;
        public SortedDictionary<string, Bitmap> Artworks = new SortedDictionary<string, Bitmap>();
        private Bitmap Artwork; //For CD booklet export
        private Plugin.MetaDataType tagId = 0;

        private bool ignorePresetChangedEvent = false;
        private bool ignoreItemCheckEvent = false;
        private bool completelyIgnoreItemCheckEvent = false;
        private bool completelyIgnoreFunctionChangedEvent = false;

        private static List<string> ProccessedReportDeletions = new List<string>();

        public LibraryReportsCommand()
        {
            InitializeComponent();
        }

        public LibraryReportsCommand(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            buttonOK.Enabled = false;
            buttonSave.Enabled = false;

            functionComboBox.Items.Add(Plugin.GroupingName);
            functionComboBox.Items.Add(Plugin.CountName);
            functionComboBox.Items.Add(Plugin.SumName);
            functionComboBox.Items.Add(Plugin.MinimumName);
            functionComboBox.Items.Add(Plugin.MaximumName);
            functionComboBox.Items.Add(Plugin.AverageName);
            functionComboBox.Items.Add(Plugin.AverageCountName);

            Plugin.FillList(sourceTagList.Items, true, true);
            Plugin.FillListWithProps(sourceTagList.Items);
            sourceTagList.Items.Add(Plugin.SequenceNumberName);

            Plugin.FillList(parameter2ComboBox.Items, true, false);
            Plugin.FillListWithProps(parameter2ComboBox.Items);

            presetsBox.Items.AddRange(Plugin.SavedSettings.libraryReportsPresets);

            presetsBox.SelectedIndex = Plugin.SavedSettings.libraryReportsPresetNumber;

            fieldComboBox.Text = Plugin.SavedSettings.savedFieldName;

            Plugin.FillList(destinationTagList.Items);
            destinationTagList.Text = Plugin.SavedSettings.destinationTagOfSavedFieldName;

            conditionCheckBox.Checked = Plugin.SavedSettings.conditionIsChecked;

            conditionFieldList.Text = Plugin.SavedSettings.conditionTagName;

            conditionList.Items.Add(Plugin.ListItemConditionIs);
            conditionList.Items.Add(Plugin.ListItemConditionIsNot);
            conditionList.Items.Add(Plugin.ListItemConditionIsGreater);
            conditionList.Items.Add(Plugin.ListItemConditionIsLess);
            conditionList.Text = Plugin.SavedSettings.condition;

            comparedFieldList.Text = Plugin.SavedSettings.comparedField;

            resizeArtworkCheckBox.Checked = Plugin.SavedSettings.resizeArtwork;
            xArworkSizeUpDown.Value = Plugin.SavedSettings.xArtworkSize == 0 ? 300 : Plugin.SavedSettings.xArtworkSize;
            yArworkSizeUpDown.Value = Plugin.SavedSettings.yArtworkSize == 0 ? 300 : Plugin.SavedSettings.yArtworkSize;

            openReportCheckBox.Checked = Plugin.SavedSettings.openReportAfterExporting;

            string[] formats = Plugin.ExportedFormats.Split('|');
            for (int i = 0; i < formats.Length; i+=2)
            {
                formatComboBox.Items.Add(formats[i]);
            }
            formatComboBox.SelectedIndex = Plugin.SavedSettings.filterIndex - 1;

            DefaultArtwork = Plugin.MissingArtwork;

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

        private void previewList_AddRowToTable(string[] row)
        {
            previewTable.Rows.Add(row);

            if (artworkField != -1)
            {
                //Replace string hashes in the Artwork column with images.
                string stringHash = (string)previewTable.Rows[previewTable.Rows.Count - 1].Cells[artworkField].Value;
                Bitmap pic;

                try
                {
                    pic = Artworks[stringHash];
                }
                catch
                {
                    pic = Artworks[DefaultArtworkHash];
                }

                previewTable.Rows[previewTable.Rows.Count - 1].Cells[artworkField].ValueType = typeof(Bitmap);
                previewTable.Rows[previewTable.Rows.Count - 1].Cells[artworkField].Value = pic;
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


                fieldComboBox.Items.Add(column.HeaderText);
                if (fieldComboBox.SelectedIndex == -1)
                    fieldComboBox.SelectedIndex = 0;
            }


            if (previewTable.Rows.Count > 0)
                clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, buttonPreview, buttonOK, buttonCancel);

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

                fieldComboBox.Items.Remove(columnHeader);
                if (fieldComboBox.SelectedIndex == -1 && fieldComboBox.Items.Count > 0)
                    fieldComboBox.SelectedIndex = 0;
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
                        clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, buttonPreview, buttonOK, buttonCancel);

                    return;
                }
            }

            if (previewTable.Columns.Count == 0)
                clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, buttonPreview, buttonOK, buttonCancel);
        }

        private void clearList()
        {
            completelyIgnoreItemCheckEvent = true;

            while (previewTable.Columns.Count > 0)
                previewTable.Columns.RemoveAt(0);
                
            for (int i = 0; i < sourceTagList.Items.Count; i++)
            {
                sourceTagList.SetItemChecked(i, false);
            }

            conditionField = -1;
            comparedField = -1;
            savedTagField = -1;
            artworkField = -1;

            fieldComboBox.Items.Clear();
            conditionFieldList.Items.Clear();
            comparedFieldList.Items.Clear();

            groupingNames.Clear();
            functionNames.Clear();
            functionTypes.Clear();
            parameterNames.Clear();
            parameter2Names.Clear();

            completelyIgnoreItemCheckEvent = false;

            functionComboBox.SelectedIndex = 0;

            clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, buttonPreview, buttonOK, buttonCancel);
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

            totals = totalsCheckBox.Checked;

            files = null;
            if (!Plugin.MbApiInterface.Library_QueryFilesEx("domain=DisplayedFiles", out files))
                files = new string[0];

            if (files.Length == 0)
            {
                MessageBox.Show(this, Plugin.MsgNoFilesInCurrentView, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }


            return true;
        }

        private void previewTrackList()
        {
            string currentFile;
            string tagValue;
            string[] groupingValues;
            string[] functionValues;
            string[] parameter2Values;
            Plugin.FilePropertyType propId;


            tags.Clear();
            Artworks.Clear();

            MD5Cng md5 = new MD5Cng();

            //Let's add default artwork
            Bitmap pic = new Bitmap(DefaultArtwork);
            TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
            byte[] hash;

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

            DefaultArtworkHash = Convert.ToBase64String(hash);

            Artworks.Add(DefaultArtworkHash, pic);


            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (backgroundTaskIsCanceled)
                {
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
                    else if (i == artworkField) //Its artwork image. Lets fill cell with hash codes. 
                    {
                        string artworkBase64 = Plugin.GetFileTag(currentFile, Plugin.MetaDataType.Artwork);

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

                        if (Plugin.SavedSettings.resizeArtwork)
                        {
                            float xSF = (float)Plugin.SavedSettings.xArtworkSize / (float)pic.Width;
                            float ySF = (float)Plugin.SavedSettings.yArtworkSize / (float)pic.Height;
                            float SF;

                            if (xSF >= ySF)
                                SF = ySF;
                            else
                                SF = xSF;


                            Bitmap bm_dest = new Bitmap((int)(pic.Width * SF), (int)(pic.Height * SF), System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                            Graphics gr_dest = Graphics.FromImage(bm_dest);
                            gr_dest.DrawImage(pic, 0, 0, bm_dest.Width, bm_dest.Height);
                            pic.Dispose();
                            gr_dest.Dispose();

                            pic = bm_dest;
                        }

                        try { hash = md5.ComputeHash((byte[])tc.ConvertTo(pic, typeof(byte[]))); }
                        catch { hash = md5.ComputeHash(new byte[] { 0x00 }); }

                        tagValue = Convert.ToBase64String(hash);

                        try
                        {
                            Artworks.Add(tagValue, pic);
                            Artwork = pic; //For CD booklet export
                        }
                        catch { }
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


                tags.add(currentFile, groupingValues, functionValues, functionTypes, parameter2Values, totals);
            }


            int k = 0;
            foreach (KeyValuePair<string, Plugin.ConvertStringsResults[]> keyValue in tags)
            {
                if (backgroundTaskIsCanceled)
                {
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

                Invoke(addRowToTable, new Object[] { row });

                previewIsGenerated = true;
            }

            Plugin.SetStatusbarTextForFileOperations(Plugin.LibraryReportsGneratingPreviewCommandSbText, true, tags.Count - 1, tags.Count, null, true);

            Invoke(updateTable);
        }

        private bool checkCondition(KeyValuePair<string, Plugin.ConvertStringsResults[]> keyValue)
        {
            if (conditionField == -1)
                return true;

            string value = AggregatedTags.GetField(keyValue, conditionField, groupingNames);
            string comparedValue;


            if (comparedField == -1)
                comparedValue = conditionValueListText;
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

        private Color GetBitmapAverageColor(Bitmap img)
        {
            Int32 avgR = 0, avgG = 0, avgB = 0;
            Int32 blurPixelCount = 0;

            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    Color pixel = img.GetPixel(x, y);
                    avgR += pixel.R;
                    avgG += pixel.G;
                    avgB += pixel.B;

                    blurPixelCount++;
                }
            }

            avgR = avgR * 3 / 2 / blurPixelCount;
            avgR = avgR > 255 ? 255 : avgR;

            avgG = avgG * 3 / 2 / blurPixelCount;
            avgG = avgG > 255 ? 255 : avgG;

            avgB = avgB * 3 / 2 / blurPixelCount;
            avgB = avgB > 255 ? 255 : avgB;


            return Color.FromArgb(avgR, avgG, avgB);
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
                fileDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";
            }

            if (!openReport)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.InitialDirectory = fileDirectoryPath;
                dialog.FileName = Plugin.SavedSettings.exportedTrackListName;
                dialog.Filter = Plugin.ExportedFormats;
                dialog.FilterIndex = Plugin.SavedSettings.filterIndex;

                if (dialog.ShowDialog() == DialogResult.Cancel) return;

                Plugin.SavedSettings.filterIndex = dialog.FilterIndex;
                formatComboBox.SelectedIndex = dialog.FilterIndex - 1;

                reportFullFileName = dialog.FileName;

                fileDirectoryPath = Regex.Replace(dialog.FileName, @"^(.*\\).*\..*", "$1"); //File directory path including ending \
                reportOnlyFileName = Regex.Replace(dialog.FileName, @"^.*\\(.*)\..*", "$1"); //Filename without path to file and extension

                Plugin.SavedSettings.filterIndex = dialog.FilterIndex;
                Plugin.SavedSettings.exportedTrackListName = reportOnlyFileName;
            }
            else
            {
                reportOnlyFileName = Plugin.SavedSettings.exportedTrackListName;
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
                        foreach (KeyValuePair<string, Plugin.ConvertStringsResults[]> keyValue in tags)
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
                    pic = Artworks[DefaultArtworkHash];
                }
                else //All tracks have the same artwork
                {
                    pic = Artwork;
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

                ((HtmlDocumentCDBooklet)document).writeHeader(size, GetBitmapAverageColor(scaledPic), scaledPic, 
                    albumArtistsAlbums, tags.Count);
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

            foreach (KeyValuePair<string, Plugin.ConvertStringsResults[]> keyValue in tags)
            {
                if (backgroundTaskIsCanceled)
                    return;

                if (checkCondition(keyValue))
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
                            document.beginAlbum(groupingsValues[albumField], Artworks[groupingsValues[artworkField]], groupingsValues[artworkField], albumTrackCounts[i]);
                        }
                        else if (prevAlbum != groupingsValues[albumField])
                        {
                            i++;
                            prevAlbum = groupingsValues[albumField];
                            document.beginAlbum(groupingsValues[albumField], Artworks[groupingsValues[artworkField]], groupingsValues[artworkField], albumTrackCounts[i]);
                        }
                    }


                    if (Plugin.SavedSettings.filterIndex != 7) //It's not a CD booklet
                    {
                        for (int j = 0; j < groupingNames.Count; j++)
                        {
                            if (j == artworkField && (Plugin.SavedSettings.filterIndex == 1 || Plugin.SavedSettings.filterIndex == 2 || Plugin.SavedSettings.filterIndex == 3)) //Export images
                            {
                                pic = Artworks[groupingsValues[artworkField]];

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
                            document.addCellToRow(AggregatedTags.GetField(keyValue, groupingNames.Count + j, groupingNames), functionNames[j], false, false);
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

            //return; //Code below may not work reliable

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

            if (checkForFunctions && fieldComboBox.SelectedIndex == -1)
            {
                MessageBox.Show(this, Plugin.MsgNoAggregateFunctionNothingToSave, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }


            conditionListText = conditionList.Text;
            conditionValueListText = comparedFieldList.Text;

            tagId = Plugin.GetTagId(destinationTagList.Text);

            conditionField = -1;
            comparedField = -1;


            string columnHeader = "";

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

                if (columnHeader == fieldComboBox.Text)
                    savedTagField = groupingNames.Count + i;
            }


            if (checkForFunctions)
            {
                sourceTagList.Enabled = false;
                buttonCheckAll.Enabled = false;
                buttonUncheckAll.Enabled = false;
            }

            return true;
        }

        private void saveField()
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

                        Plugin.SetFileTag(keyValueUrls.Key, tagId, AggregatedTags.GetField(keyValue, savedTagField, groupingNames));
                        Plugin.CommitTagsToFile(keyValueUrls.Key);
                    }
                }
            }

            Plugin.RefreshPanels(true);

            Plugin.SetStatusbarTextForFileOperations(Plugin.LibraryReportsCommandSbText, false, tags.Count - 1, tags.Count, null, true);

            Invoke(updateTable);
        }

        private void saveSettings()
        {
            if (presetsBox.SelectedIndex == 0 || presetsBox.Text == "")
            {
                LibraryReportsPreset libraryReportsPreset = new LibraryReportsPreset();

                libraryReportsPreset.groupingNames = new string[groupingNames.Count];
                libraryReportsPreset.functionNames = new string[functionNames.Count];
                libraryReportsPreset.functionTypes = new FunctionType[functionTypes.Count];
                libraryReportsPreset.parameterNames = new string[parameterNames.Count];
                libraryReportsPreset.parameter2Names = new string[parameter2Names.Count];
                libraryReportsPreset.totals = totalsCheckBox.Checked;

                groupingNames.CopyTo(libraryReportsPreset.groupingNames, 0);
                functionNames.CopyTo(libraryReportsPreset.functionNames, 0);
                functionTypes.CopyTo(libraryReportsPreset.functionTypes, 0);
                parameterNames.CopyTo(libraryReportsPreset.parameterNames, 0);
                parameter2Names.CopyTo(libraryReportsPreset.parameter2Names, 0);

                ignorePresetChangedEvent = true;
                Plugin.SetItemInComboBox(presetsBox, libraryReportsPreset);
                ignorePresetChangedEvent = false;

                presetsBox.Items.CopyTo(Plugin.SavedSettings.libraryReportsPresets, 0);

                PresetName = Plugin.LibraryReport;
            }
            else if (presetsBox.SelectedIndex < Plugin.NumberOfPredefinedPresets)
            {
                PresetName = presetsBox.Text;
            }
            else
            {
                PresetName = Plugin.LibraryReport;
            }

            Plugin.SavedSettings.savedFieldName = fieldComboBox.Text;
            Plugin.SavedSettings.destinationTagOfSavedFieldName = destinationTagList.Text;
            Plugin.SavedSettings.conditionIsChecked = conditionCheckBox.Checked;
            Plugin.SavedSettings.conditionTagName = conditionFieldList.Text;
            Plugin.SavedSettings.condition = conditionList.Text;
            Plugin.SavedSettings.comparedField = comparedFieldList.Text;
            Plugin.SavedSettings.filterIndex = formatComboBox.SelectedIndex + 1;
            Plugin.SavedSettings.openReportAfterExporting = openReportCheckBox.Checked;

            Plugin.SavedSettings.libraryReportsPresetNumber = presetsBox.SelectedIndex;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            saveSettings();
            exportTrackList(openReportCheckBox.Checked);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            saveSettings();
            if (prepareBackgroundTask(true))
                switchOperation(saveField, (Button)sender, buttonOK, buttonPreview, buttonCancel, true, null);
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewTrackList, (Button)sender, buttonOK, buttonCancel);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        public override void enableDisablePreviewOptionControls(bool enable)
        {
            if (enable && previewIsGenerated)
                return;

            presetsBox.Enabled = enable;
            sourceTagList.Enabled = enable;
            buttonCheckAll.Enabled = enable;
            buttonUncheckAll.Enabled = enable;
            totalsCheckBox.Enabled = enable;
            functionComboBox.Enabled = enable;
            resizeArtworkCheckBox.Enabled = enable;
            xArworkSizeUpDown.Enabled = enable && resizeArtworkCheckBox.Checked;
            yArworkSizeUpDown.Enabled = enable && resizeArtworkCheckBox.Checked;

            buttonOK.Enabled = !enable;
            buttonSave.Enabled = !enable;
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
            buttonSave.Enabled = true;
        }

        public override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enabled = false;
            buttonPreview.Enabled = false;
            buttonSave.Enabled = false;

        }

        private void sourceTagList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (completelyIgnoreItemCheckEvent)
                return;


            ignorePresetChangedEvent = true;

            if (!ignoreItemCheckEvent)
                presetsBox.SelectedItem = Plugin.LibraryReportsPreset;

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
        }

        private void buttonUncheckAll_Click(object sender, EventArgs e)
        {
            clearList();
            presetsBox.SelectedIndex = 0;
        }

        private void buttonCheckAll_Click(object sender, EventArgs e)
        {
            addAllTags();
        }

        private void presetsBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (ignorePresetChangedEvent)
                return;

            ignoreItemCheckEvent = true;

            LibraryReportsPreset libraryReportsPreset = (LibraryReportsPreset)presetsBox.SelectedItem;

            clearList();

            completelyIgnoreFunctionChangedEvent = true;

            //Groupings
            functionComboBox.SelectedIndex = 0;
            for (int i = 0; i < libraryReportsPreset.groupingNames.Length; i++)
            {
                for (int j = 0; j < sourceTagList.Items.Count; j++)
                {
                    if (libraryReportsPreset.groupingNames[i] == (string)sourceTagList.Items[j])
                        sourceTagList.SetItemChecked(j, true);
                }
            }

            //Functions
            for (int i = 0; i < libraryReportsPreset.functionNames.Length; i++)
            {
                functionComboBox.SelectedIndex = (int)libraryReportsPreset.functionTypes[i];

                parameter2ComboBox.SelectedItem = libraryReportsPreset.parameter2Names[i];

                completelyIgnoreItemCheckEvent = true; //Lets clear items list which was set in previous iteration
                for (int j = 0; j < sourceTagList.Items.Count; j++)
                {
                    sourceTagList.SetItemChecked(j, false);
                }
                completelyIgnoreItemCheckEvent = false;


                for (int j = 0; j < sourceTagList.Items.Count; j++)
                {
                    if (libraryReportsPreset.parameterNames[i] == (string)sourceTagList.Items[j])
                    {
                        sourceTagList.SetItemChecked(j, true);
                    }
                }
            }

            if (parameter2ComboBox.SelectedIndex == -1)
                parameter2ComboBox.SelectedItem = Plugin.MbApiInterface.Setting_GetFieldName((Plugin.MetaDataType)Plugin.FilePropertyType.Url);

            totalsCheckBox.Checked = libraryReportsPreset.totals;

            completelyIgnoreFunctionChangedEvent = false;
            ignoreItemCheckEvent = false;

            functionComboBox.SelectedIndex = 0;
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
        }

        private void previewList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
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

                Plugin.SetStatusbarText(Plugin.LibraryReportsCommandSbText + " (" + Plugin.SbSorting + ")", true);
                previewTable.Sort(comparator);
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
}
