using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using ExtensionMethods;

using static MusicBeePlugin.Plugin;
using static MusicBeePlugin.AdvancedSearchAndReplace;

namespace MusicBeePlugin
{
    public partial class ChangeCase : PluginWindowTemplate
    {
        public enum ChangeCaseOptions
        {
            SentenceCase = -1,
            LowerCase = 0,
            UpperCase = 1,
            TitleCase = 2,
            ToggleCase = 3,
        }

        public class Row
        {
            public string Checked { get; set; }
            public string File { get; set; }
            public string Track { get; set; }
            public string OriginalTagValue { get; set; }
            public string OriginalTagValueNormalized { get; set; }
            public string NewTagValue { get; set; }
            public string NewTagValueNormalized { get; set; }
        }

        public class ChangeCaseStep
        {
            public int rule;

            public bool? exceptionWordsState;
            public int exceptionWordsIndex;
            public int exceptionCharsIndex;
            public int exceptionCharPair1Index;
            public int exceptionCharPair2Index;
            public int sentenceSeparatorsIndex;
            public bool alwaysCapitalize1stWord;
            public bool alwaysCapitalizeLastWord;
            public bool ignoreSingleLetterExceptedWords;
        }

        public class ChangeCasePreset : IComparable
        {
            public bool predefined;
            public SerializableDictionary<string, string> names;
            public SerializableDictionary<string, string> descriptions;

            public List<ChangeCaseStep> steps;

            public ChangeCasePreset()
            {
                names = new SerializableDictionary<string, string>();
                descriptions = new SerializableDictionary<string, string>();
                steps = new List<ChangeCaseStep>();
            }

            public int CompareTo(object referencePreset)
            {
                if (referencePreset == null)
                    referencePreset = string.Empty;

                return (this.predefined.ToString() + this.getName()).CompareTo((referencePreset as ChangeCasePreset).predefined.ToString() 
                    + referencePreset.ToString());
            }

            public override string ToString()
            {
                return getName() + (!predefined ? " " : string.Empty);
            }

            public string getName()
            {
                return GetDictValue(names, Language);
            }

            public string getDescription()
            {
                return GetDictValue(descriptions, Language);
            }
        }

        private bool recordMode;
        private ChangeCasePreset recordedPreset;
        private ChangeCaseStep currentStep;

        private CustomComboBox presetBoxCustom;
        private CustomComboBox sourceTagListCustom;
        private CustomComboBox exceptionWordsBoxCustom;
        private CustomComboBox exceptionCharsBoxCustom;
        private CustomComboBox openingExceptionCharsBoxCustom;
        private CustomComboBox closingExceptionCharsBoxCustom;
        private CustomComboBox sentenceSeparatorsBoxCustom;

        private CustomListBox exceptionWordsEnumList;
        private ComboBox exceptionWordsEnumComboBox;

        private CustomListBox exceptionCharsEnumList;
        private ComboBox exceptionCharsEnumComboBox;

        private CustomListBox openingExceptionCharsEnumList;
        private ComboBox openingExceptionCharsEnumComboBox;

        private CustomListBox closingExceptionCharsEnumList;
        private ComboBox closingExceptionCharsEnumComboBox;

        private CustomListBox sentenceSeparatorsEnumList;
        private ComboBox sentenceSeparatorsEnumComboBox;

        private bool ignoreComboBoxSelectedIndexChanged = false;

        private bool closingExceptionCharsBoxLeaving;

        private readonly DataGridViewCellStyle unchangedCellStyle = new DataGridViewCellStyle(UnchangedCellStyle);
        private readonly DataGridViewCellStyle changedCellStyle = new DataGridViewCellStyle(ChangedCellStyle);
        private readonly DataGridViewCellStyle dimmedCellStyle = new DataGridViewCellStyle(DimmedCellStyle);


        private List<Row> rows = new List<Row>();
        private BindingSource source = new BindingSource();

        private string[] files = Array.Empty<string>();
        private readonly List<string[]> tags = new List<string[]>();
        private MetaDataType sourceTagId;
        private List<bool> processedRowList = new List<bool>(); //Indices of processed tracks

        private int changeCaseFlag;
        private bool useWhiteList;
        private string[] exceptedWords;
        private string[] exceptionChars;
        private string[] openingExceptionChars;
        private string[] closingExceptionChars;
        private string[] sentenceSeparators;
        private bool? alwaysCapitalize1stWord;
        private bool? alwaysCapitalizeLastWord;
        private bool ignoreSingleLetterExceptedWords;

        private string sentenceCaseText;//****************
        private string lowerCaseText;
        private string upperCaseText;
        private string titleCaseText;
        private string toggleCaseText;


        private string changeOnlyWordsText;
        private string exceptForWordsText;

        private string exceptionCharsText;
        private string exceptionCharPairsText;
        private string sentenceSeparatorsText;

        private string alwaysCapitalize1stWordText;
        private string alwaysCapitalizeLastWordText;
        private string ignoreSingleLetterExceptedWordsText;

        private string helpMessage;

        public ChangeCase(Plugin plugin) : base(plugin)
        {
            InitializeComponent();
        }

        internal protected override void initializeForm()
        {
            base.initializeForm();

            presetBoxCustom = namesComboBoxes["presetBox"];

            sourceTagListCustom = namesComboBoxes["sourceTagList"];

            exceptionWordsBoxCustom = namesComboBoxes["exceptionWordsBox"];
            exceptionWordsBoxCustom.Leave += exceptionWordsBox_Leave;

            exceptionCharsBoxCustom = namesComboBoxes["exceptionCharsBox"];
            exceptionCharsBoxCustom.Leave += exceptionCharsBox_Leave;

            openingExceptionCharsBoxCustom = namesComboBoxes["openingExceptionCharsBox"];
            openingExceptionCharsBoxCustom.Leave += openingExceptionCharsBox_Leave;

            closingExceptionCharsBoxCustom = namesComboBoxes["closingExceptionCharsBox"];
            closingExceptionCharsBoxCustom.Leave += closingExceptionCharsBox_Leave;

            sentenceSeparatorsBoxCustom = namesComboBoxes["sentenceSeparatorsBox"];
            sentenceSeparatorsBoxCustom.Leave += sentenceSeparatorsBox_Leave;


            buttonLabels[buttonDeletePreset] = string.Empty;
            buttonDeletePreset.Text = string.Empty;
            ReplaceButtonBitmap(buttonDeletePreset, ClearField);

            buttonLabels[buttonPlayPreset] = string.Empty;
            buttonPlayPreset.Text = string.Empty;
            ReplaceButtonBitmap(buttonPlayPreset, Play);

            buttonLabels[buttonRecordPreset] = string.Empty;
            buttonRecordPreset.Text = string.Empty;
            ReplaceButtonBitmap(buttonRecordPreset, Record);

            buttonLabels[buttonStopRecordingPreset] = string.Empty;
            buttonStopRecordingPreset.Text = string.Empty;
            ReplaceButtonBitmap(buttonStopRecordingPreset, Stop);

            buttonLabels[removeExceptionButton] = string.Empty;
            removeExceptionButton.Text = string.Empty;
            ReplaceButtonBitmap(removeExceptionButton, ClearField);

            ReplaceButtonBitmap(buttonSettings, Gear);


            sentenceCaseText = sentenceCaseRadioButtonLabel.Text;
            lowerCaseText = lowerCaseRadioButtonLabel.Text;
            upperCaseText = upperCaseRadioButtonLabel.Text;
            titleCaseText = titleCaseRadioButtonLabel.Text;
            toggleCaseText = toggleCaseRadioButtonLabel.Text;


            changeOnlyWordsText = toolTip1.GetToolTip(exceptionWordsCheckBoxLabel);
            exceptForWordsText = exceptionWordsCheckBoxLabel.Text;
            toolTip1.SetToolTip(exceptionWordsCheckBoxLabel, toolTip1.GetToolTip(exceptionWordsCheckBox));

            exceptionCharsText = exceptionCharsCheckBoxLabel.Text;
            exceptionCharPairsText = exceptionCharPairsCheckBoxLabel.Text;
            sentenceSeparatorsText = sentenceSeparatorsCheckBoxLabel.Text;

            alwaysCapitalize1stWordText = alwaysCapitalize1stWordCheckBoxLabel.Text;
            alwaysCapitalizeLastWordText = alwaysCapitalizeLastWordCheckBoxLabel.Text;
            ignoreSingleLetterExceptedWordsText = ignoreSingleLetterExceptedWordsCheckBoxLabel.Text;


            helpMessage = toolTip1.GetToolTip(buttonHelp);
            toolTip1.SetToolTip(buttonHelp, string.Empty);


            FillListByTagNames(sourceTagListCustom.Items);
            sourceTagListCustom.Text = SavedSettings.changeCaseSourceTagName;

            setChangeCaseOptionsRadioButtons(SavedSettings.changeCaseFlag);


            string toolTip = exceptionWordsBoxCustom.GetToolTip(toolTip1);
            if (useSkinColors)
                toolTip = Regex.Replace(toolTip, @"^(.*\r\n)(.*\r\n)(.*)\r\n(.*)", "$1$2$4");
            else
                toolTip = Regex.Replace(toolTip, @"^(.*\r\n)(.*\r\n)(.*)\r\n(.*)", "$1$2$3");

            exceptionWordsBoxCustom.SetToolTip(toolTip1, toolTip);


            toolTip = exceptionCharsBoxCustom.GetToolTip(toolTip1);
            if (useSkinColors)
                toolTip = Regex.Replace(toolTip, @"^(.*\r\n)(.*)\r\n(.*)", "$1$3");
            else
                toolTip = Regex.Replace(toolTip, @"^(.*\r\n)(.*)\r\n(.*)", "$1$2");

            exceptionCharsBoxCustom.SetToolTip(toolTip1, toolTip);


            toolTip = openingExceptionCharsBoxCustom.GetToolTip(toolTip1);
            if (useSkinColors)
                toolTip = Regex.Replace(toolTip, @"^(.*\r\n)(.*\r\n)(.*\r\n)(.*\r\n)(.*)\r\n(.*)", "$1$2$3$4$6");
            else
                toolTip = Regex.Replace(toolTip, @"^(.*\r\n)(.*\r\n)(.*\r\n)(.*\r\n)(.*)\r\n(.*)", "$1$2$3$4$5");

            openingExceptionCharsBoxCustom.SetToolTip(toolTip1, toolTip);


            toolTip = closingExceptionCharsBoxCustom.GetToolTip(toolTip1);
            if (useSkinColors)
                toolTip = Regex.Replace(toolTip, @"^(.*\r\n)(.*\r\n)(.*\r\n)(.*\r\n)(.*)\r\n(.*)", "$1$2$3$4$6");
            else
                toolTip = Regex.Replace(toolTip, @"^(.*\r\n)(.*\r\n)(.*\r\n)(.*\r\n)(.*)\r\n(.*)", "$1$2$3$4$5");

            closingExceptionCharsBoxCustom.SetToolTip(toolTip1, toolTip);


            toolTip = sentenceSeparatorsBoxCustom.GetToolTip(toolTip1);
            if (useSkinColors)
                toolTip = Regex.Replace(toolTip, @"^(.*\r\n)(.*)\r\n(.*)", "$1$3");
            else
                toolTip = Regex.Replace(toolTip, @"^(.*\r\n)(.*)\r\n(.*)", "$1$2");

            sentenceSeparatorsBoxCustom.SetToolTip(toolTip1, toolTip);



            var list1 = exceptionWordsBoxCustom.CreateSpecialStateColumn("#", 1);
            exceptionWordsBoxCustom.MaxSpecialState = 5; //-----

            exceptionWordsCheckBox.CheckState = GetCheckState(SavedSettings.useExceptionWords);
            exceptionWordsBoxCustom.AddRange(SavedSettings.exceptedWords);

            if (list1 is CustomListBox)
            {
                exceptionWordsEnumList = list1 as CustomListBox;
                exceptionWordsEnumList.Click += enumList_Click;
            }
            else if (list1 is ComboBox)
            {
                exceptionWordsEnumComboBox = list1 as ComboBox;
                exceptionWordsEnumComboBox.DropDownClosed += enumComboBox_DropDownClosed;
            }

            string text = SavedSettings.exceptedWords[0] as string;
            if (text.Length >= exceptionWordsBoxCustom.GetSpecialStateCharCount(true) && text[exceptionWordsBoxCustom.GetSpecialStateCharCount(true) - 1] == ' ')
                text = addSubstituteComboBoxSpecialState(text, string.Empty, exceptionWordsBoxCustom.GetSpecialStateCharCount(true));

            exceptionWordsBoxCustom.Text = text;



            var list2 = exceptionCharsBoxCustom.CreateSpecialStateColumn("#", 1);
            exceptionCharsBoxCustom.MaxSpecialState = 5; //-----

            exceptionCharsCheckBox.Checked = SavedSettings.useExceptionChars;
            exceptionCharsBoxCustom.AddRange(SavedSettings.exceptionChars);

            if (list2 is CustomListBox)
            {
                exceptionCharsEnumList = list2 as CustomListBox;
                exceptionCharsEnumList.Click += enumList_Click;
            }
            else if (list2 is ComboBox)
            {
                exceptionCharsEnumComboBox = list2 as ComboBox;
                exceptionCharsEnumComboBox.DropDownClosed += enumComboBox_DropDownClosed;
            }

            text = (SavedSettings.exceptionChars[0] as string) + " ";
            if (text.Length >= exceptionCharsBoxCustom.GetSpecialStateCharCount(true) && text[exceptionCharsBoxCustom.GetSpecialStateCharCount(true) - 1] == ' ')
                text = addSubstituteComboBoxSpecialState(text, string.Empty, exceptionCharsBoxCustom.GetSpecialStateCharCount(true));

            exceptionCharsBoxCustom.Text = text;



            var list3 = openingExceptionCharsBoxCustom.CreateSpecialStateColumn("#", 1);
            openingExceptionCharsBoxCustom.MaxSpecialState = 5; //-----

            exceptionCharPairsCheckBox.Checked = SavedSettings.useExceptionCharPairs;
            openingExceptionCharsBoxCustom.AddRange(SavedSettings.openingExceptionChars);

            if (list3 is CustomListBox)
            {
                openingExceptionCharsEnumList = list3 as CustomListBox;
                openingExceptionCharsEnumList.Click += enumList_Click;
            }
            else if (list3 is ComboBox)
            {
                openingExceptionCharsEnumComboBox = list3 as ComboBox;
                openingExceptionCharsEnumComboBox.DropDownClosed += enumComboBox_DropDownClosed;
            }

            text = (SavedSettings.openingExceptionChars[0] as string) + " ";
            if (text.Length >= openingExceptionCharsBoxCustom.GetSpecialStateCharCount(true) && text[openingExceptionCharsBoxCustom.GetSpecialStateCharCount(true) - 1] == ' ')
                text = addSubstituteComboBoxSpecialState(text, string.Empty, openingExceptionCharsBoxCustom.GetSpecialStateCharCount(true));

            openingExceptionCharsBoxCustom.Text = text;



            var list4 = closingExceptionCharsBoxCustom.CreateSpecialStateColumn("#", 1);
            closingExceptionCharsBoxCustom.MaxSpecialState = 5; //-----

            closingExceptionCharsBoxCustom.AddRange(SavedSettings.closingExceptionChars);

            if (list4 is CustomListBox)
            {
                closingExceptionCharsEnumList = list4 as CustomListBox;
                closingExceptionCharsEnumList.Click += enumList_Click;
            }
            else if (list4 is ComboBox)
            {
                closingExceptionCharsEnumComboBox = list4 as ComboBox;
                closingExceptionCharsEnumComboBox.DropDownClosed += enumComboBox_DropDownClosed;
            }

            text = (SavedSettings.closingExceptionChars[0] as string) + " ";
            if (text.Length >= closingExceptionCharsBoxCustom.GetSpecialStateCharCount(true) && text[closingExceptionCharsBoxCustom.GetSpecialStateCharCount(true) - 1] == ' ')
                text = addSubstituteComboBoxSpecialState(text, string.Empty, closingExceptionCharsBoxCustom.GetSpecialStateCharCount(true));

            closingExceptionCharsBoxCustom.Text = text;



            var list5 = sentenceSeparatorsBoxCustom.CreateSpecialStateColumn("#", 1);
            sentenceSeparatorsBoxCustom.MaxSpecialState = 5; //-----

            sentenceSeparatorsCheckBox.Checked = SavedSettings.useSentenceSeparators;
            sentenceSeparatorsBoxCustom.AddRange(SavedSettings.sentenceSeparators);

            if (list5 is CustomListBox)
            {
                sentenceSeparatorsEnumList = list5 as CustomListBox;
                sentenceSeparatorsEnumList.Click += enumList_Click;
            }
            else if (list5 is ComboBox)
            {
                sentenceSeparatorsEnumComboBox = list5 as ComboBox;
                sentenceSeparatorsEnumComboBox.DropDownClosed += enumComboBox_DropDownClosed;
            }

            text = (SavedSettings.sentenceSeparators[0] as string) + " ";
            if (text.Length >= sentenceSeparatorsBoxCustom.GetSpecialStateCharCount(true) && text[sentenceSeparatorsBoxCustom.GetSpecialStateCharCount(true) - 1] == ' ')
                text = addSubstituteComboBoxSpecialState(text, string.Empty, sentenceSeparatorsBoxCustom.GetSpecialStateCharCount(true));

            sentenceSeparatorsBoxCustom.Text = text;



            alwaysCapitalize1stWordCheckBox.CheckState = GetCheckState(SavedSettings.alwaysCapitalize1stWord);
            alwaysCapitalizeLastWordCheckBox.CheckState = GetCheckState(SavedSettings.alwaysCapitalizeLastWord);

            ignoreSingleLetterExceptedWordsCheckBox.Checked = SavedSettings.ignoreSingleLetterExceptedWords;

            exceptWordsCheckBox_CheckStateChanged(null, null);
            exceptCharsCheckBox_CheckedChanged(null, null);
            exceptionCharPairsCheckBox_CheckedChanged(null, null);
            sentenceSeparatorsCheckBox_CheckedChanged(null, null);
            casingRuleRadioButton_CheckedChanged(null, null);


            var headerCellStyle = new DataGridViewCellStyle(HeaderCellStyle);

            var cbHeader = new DataGridViewCheckBoxHeaderCell();
            cbHeader.Style = headerCellStyle;
            cbHeader.setState(true);

            var colCB = new DataGridViewCheckBoxColumn
            {
                HeaderCell = cbHeader,
                ThreeState = true,
                FalseValue = ColumnUncheckedState,
                TrueValue = ColumnCheckedState,
                IndeterminateValue = string.Empty,
                Width = 25,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Resizable = DataGridViewTriState.False,
                DataPropertyName = "Checked",
            };

            previewTable.Columns.Insert(0, colCB);
            previewTable.Columns[2].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[4].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[6].HeaderCell.Style = headerCellStyle;

            previewTable.Columns[2].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[4].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[6].HeaderCell.Style.WrapMode = DataGridViewTriState.True;

            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            source.DataSource = rows;
            previewTable.DataSource = source;

            (previewTable.Columns[0].HeaderCell as DataGridViewCheckBoxHeaderCell).OnCheckBoxClicked += cbHeader_OnCheckBoxClicked;


            enableDisablePreviewOptionControls(true, true);
            enableQueryingOrUpdatingButtons();

            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        private string addSubstituteListBoxSpecialState(string itemText, string state, int specialStateCharCount)
        {
            return itemText;
        }

        private void enumList_Click(object sender, EventArgs e)
        {
            CustomListBox enumList = sender as CustomListBox;
            CustomComboBox customComboBox = (sender as ListControl).Tag as CustomComboBox;

            if (recordMode)
                return;


            if (ModifierKeys == Keys.Shift)
            {
                customComboBox.SpecialState = -1;

                for (int i = 0; i < enumList.Items.Count; i++)
                    enumList.Items[i] = customComboBox.GetDefaultSpecialState();

                return;
            }
            else if (customComboBox.SpecialState != -1 && !customComboBox.IsDefaultSpecialStateItem(enumList.SelectedIndex))
            {
                return;
            }


            if (customComboBox.SpecialState == -1)
            {
                for (int i = 0; i < enumList.Items.Count; i++)
                    enumList.Items[i] = customComboBox.GetDefaultSpecialState();

                customComboBox.SpecialState = 1;
                enumList.Items[enumList.SelectedIndex] = customComboBox.GetCurrentSpecialState();
            }
            else if (customComboBox.IsDefaultSpecialStateItem(enumList.SelectedIndex))
            {
                customComboBox.SpecialState++;
                enumList.Items[enumList.SelectedIndex] = customComboBox.GetCurrentSpecialState();
            }
        }

        private string addSubstituteComboBoxSpecialState(string itemText, string state, int specialStateCharCount)
        {
            itemText = itemText ?? string.Empty;

            if (string.IsNullOrWhiteSpace(itemText))
                itemText = state;
            else
                itemText = state + itemText.Substring(specialStateCharCount);

            return itemText;
        }

        private void enumComboBox_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox enumComboBox = sender as ComboBox;
            CustomComboBox customComboBox = (sender as ListControl).Tag as CustomComboBox;

            if (recordMode && customComboBox.GetSpecialStateItem(customComboBox.SelectedIndex) == customComboBox.GetDefaultSpecialState())
            {
                ignoreComboBoxSelectedIndexChanged = true;
                enumComboBox.DroppedDown = true;
                System.Media.SystemSounds.Beep.Play();
                return;
            }
            else if (recordMode)
            {
                ignoreComboBoxSelectedIndexChanged = false;
            }


            if (ignoreComboBoxSelectedIndexChanged || enumComboBox.SelectedIndex == -1)
            {
                ignoreComboBoxSelectedIndexChanged = false;
                return;
            }

            string currentText = customComboBox.Text;
            if (currentText.Length >= customComboBox.GetSpecialStateCharCount(true)
                && currentText[customComboBox.GetSpecialStateCharCount(true) - 1] == ' ')

                currentText = addSubstituteComboBoxSpecialState(customComboBox.Text, string.Empty, customComboBox.GetSpecialStateCharCount(true));


            string itemText = customComboBox.GetSpecialStateItem(enumComboBox.SelectedIndex);
            itemText = Regex.Replace(itemText.Trim(' '), @"\s{2,}", " ");

            if (ModifierKeys != Keys.Control) //Let's process only ctrl-clicks and shift-clicks here for native combo box
            {
                customComboBox.SpecialState = -1;

                ignoreComboBoxSelectedIndexChanged = true;

                if (ModifierKeys == Keys.Shift)
                {
                    for (int i = 0; i < enumComboBox.Items.Count; i++)
                        enumComboBox.Items[i] = addSubstituteComboBoxSpecialState(enumComboBox.Items[i] as string,
                            customComboBox.GetDefaultSpecialState(), customComboBox.GetSpecialStateCharCount(true));

                    customComboBox.SetText(currentText, true);

                    ignoreComboBoxSelectedIndexChanged = false;

                    return;
                }

                CustomComboBoxLeave(customComboBox, itemText, addSubstituteComboBoxSpecialState, customComboBox.GetDefaultSpecialState());

                customComboBox.SetText(itemText, true);

                ignoreComboBoxSelectedIndexChanged = false;

                return;
            }

            ignoreComboBoxSelectedIndexChanged = true;

            int enumComboBoxSelectedIndex = enumComboBox.SelectedIndex;

            enumComboBox.DroppedDown = true;

            if (customComboBox.SpecialState == -1)
            {
                customComboBox.SpecialState = 1;

                for (int i = 0; i < enumComboBox.Items.Count; i++)
                {
                    if (i == enumComboBoxSelectedIndex)
                        enumComboBox.Items[i] = customComboBox.GetCurrentSpecialState() + itemText;
                    else
                        enumComboBox.Items[i] = addSubstituteComboBoxSpecialState(enumComboBox.Items[i] as string, 
                            customComboBox.GetDefaultSpecialState(), customComboBox.GetSpecialStateCharCount(true));
                }
            }
            else
            {
                if (customComboBox.IsDefaultSpecialStateItem(enumComboBoxSelectedIndex))
                    enumComboBox.Items[enumComboBoxSelectedIndex] = customComboBox.GetItemNextSpecialState(enumComboBoxSelectedIndex) + itemText;
            }

            customComboBox.SetText(currentText, true);

            ignoreComboBoxSelectedIndexChanged = false;
        }

        private void setChangeCaseOptionsRadioButtons(int pos)
        {
            switch (pos)
            {
                case 1:
                    sentenceCaseRadioButton.Checked = true;
                    break;
                case 2:
                    lowerCaseRadioButton.Checked = true;
                    break;
                case 3:
                    upperCaseRadioButton.Checked = true;
                    break;
                case 4:
                    titleCaseRadioButton.Checked = true;
                    break;
                default:
                    toggleCaseRadioButton.Checked = true;
                    break;
            }
        }

        private int getChangeCaseOptionsRadioButtons()
        {
            if (sentenceCaseRadioButton.Checked) return 1;
            else if (lowerCaseRadioButton.Checked) return 2;
            else if (upperCaseRadioButton.Checked) return 3;
            else if (titleCaseRadioButton.Checked) return 4;
            else return 5;
        }

        private void previewTable_ProcessRowsOfTable(List<bool> processedRowList)
        {
            for (int i = 0; i < processedRowList.Count; i++)
            {
                previewTable.CurrentCell = previewTable.Rows[i].Cells[0];

                if (processedRowList[i])
                    previewTable.Rows[i].Cells[0].Value = null;

                previewTableFormatRow(previewTable, i);
            }
        }

        private void resetPreviewData()
        {
            previewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            previewTable.AllowUserToResizeColumns = true;
            previewTable.AllowUserToResizeRows = true;
            foreach (DataGridViewColumn column in previewTable.Columns)
                column.SortMode = DataGridViewColumnSortMode.Automatic;


            previewIsGenerated = false;
            backgroundTaskIsStopping = false;
            backgroundTaskIsStoppedOrCancelled = false;

            tags.Clear();
            rows.Clear();
            source.ResetBindings(false);

            (previewTable.Columns[0].HeaderCell as DataGridViewCheckBoxHeaderCell).setState(true);

            updateCustomScrollBars(previewTable);
            SetStatusBarText(string.Empty, false);

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
            previewTable.AllowUserToResizeColumns = true;
            previewTable.AllowUserToResizeRows = true;
            foreach (DataGridViewColumn column in previewTable.Columns)
                column.SortMode = DataGridViewColumnSortMode.Automatic;//----- Programmatic!!!

            previewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            autoSizeTableRows(previewTable, 2);


            backgroundTaskIsScheduled = false;
            backgroundTaskIsStopping = false;
            backgroundTaskIsStoppedOrCancelled = false;

            enableDisablePreviewOptionControls(true);
            enableQueryingOrUpdatingButtons();

            updateCustomScrollBars(previewTable);
            SetResultingSbText();

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

            previewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            previewTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;


            backgroundTaskIsScheduled = false;
            backgroundTaskIsStopping = false;
            backgroundTaskIsStoppedOrCancelled = false;
            closeFormOnStopping = false;
            ignoreClosingForm = false;

            previewTable_ProcessRowsOfTable(processedRowList);

            SetResultingSbText();

            return true;
        }

        internal static string ChangeSubstringCase(string substring, ChangeCaseOptions changeCaseOption, bool isTheFirstWord)
        {
            var newSubstring = string.Empty;

            switch (changeCaseOption)
            {
                case ChangeCaseOptions.SentenceCase:
                    if (isTheFirstWord)
                        newSubstring = ChangeSubstringCase(substring, ChangeCaseOptions.TitleCase, true);
                    else
                        newSubstring = ChangeSubstringCase(substring, ChangeCaseOptions.LowerCase, false);

                    break;
                case ChangeCaseOptions.TitleCase:
                    var isTheFirstChar = true;

                    foreach (var currentChar in substring)
                    {
                        if (isTheFirstChar)
                        {
                            newSubstring += (string.Empty + currentChar).ToUpper();
                            isTheFirstChar = false;
                        }
                        else
                            newSubstring += (string.Empty + currentChar).ToLower();
                    }
                    break;
                case ChangeCaseOptions.LowerCase:
                    newSubstring = substring.ToLower();
                    break;
                case ChangeCaseOptions.UpperCase:
                    newSubstring = substring.ToUpper();
                    break;
                case ChangeCaseOptions.ToggleCase:
                    foreach (var currentChar in substring)
                    {
                        if ((string.Empty + currentChar).ToUpper() == (string.Empty + currentChar)) //Char is uppercased
                            newSubstring += (string.Empty + currentChar).ToLower();
                        else //Char is lowercased
                            newSubstring += (string.Empty + currentChar).ToUpper();
                    }
                    break;
            }

            return newSubstring;
        }

        internal static bool IsCharASymbol(char character)
        {
            if (char.IsLetter(character)) //Letters
            {
                return false;
            }
            else if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(character) == System.Globalization.UnicodeCategory.NonSpacingMark) //Diacritics
            {
                return false;
            }
            else
            {
                switch (character)
                {
                    case '\'':
                        return false;
                    case '’':
                        return false;
                    case '0':
                        return false;
                    case '1':
                        return false;
                    case '2':
                        return false;
                    case '3':
                        return false;
                    case '4':
                        return false;
                    case '5':
                        return false;
                    case '6':
                        return false;
                    case '7':
                        return false;
                    case '8':
                        return false;
                    case '9':
                        return false;
                    default:
                        return true;
                }
            }
        }

        internal static bool IsItemContainedInArray(string item, string[] array)
        {
            item = string.Empty + item; //It converts null to string

            foreach (var currentItem in array)
            {
                if (currentItem.ToLower() == "*rn") //Roman numerals
                {
                    item = Regex.Replace(item, @"([IiVvXxLl]{2,})", string.Empty); //Latin letters replacements
                    item = Regex.Replace(item, @"([ⅢⅣⅤⅥⅦⅧⅨⅩⅪⅫⅬⅭⅮⅯⅰⅱⅲⅳⅴⅵⅶⅷⅸⅹⅺⅻⅼⅽⅾⅿↀↁↂↃↄↅↆↇↈ]+)", string.Empty); //Roman numerals replacements

                    if (string.IsNullOrEmpty(item))
                        return true;
                    else
                        return false;
                }
                else if (item.ToLower() == currentItem.ToLower())
                {
                    return true;
                }
            }

            return false;
        }

        internal static bool IsItemContainedInArray(char item, string[] array)
        {
            return IsItemContainedInArray(item.ToString(), array);
        }

        internal static int ItemIndexInArray(string item, string[] array)
        {
            item = string.Empty + item; //It converts null to string

            for (var i = 0; i < array.Length; i++)
                if (item.ToLower() == array[i].ToLower()) return i;

            return -1;
        }

        internal static int ItemIndexInArray(char item, string[] array)
        {
            return ItemIndexInArray(item.ToString(), array);
        }

        internal static string ChangeWordsCase(string source, ChangeCaseOptions changeCaseOption)
        {
            List<char> encounteredLeftExceptionChars = null;
            bool wasCharException = false;
            return ChangeWordsCase(source, changeCaseOption, ref encounteredLeftExceptionChars, ref wasCharException);
        }

        internal static string ChangeWordsCase(string source, ChangeCaseOptions changeCaseOption, ref List<char> encounteredLeftExceptionChars,
            ref bool wasCharException, string[] exceptedWords = null, bool useWhiteList = false,
            string[] exceptionChars = null, string[] openingExceptionChars = null, string[] closingExceptionChars = null,
            bool? alwaysCapitalize1stWord = false, bool? alwaysCapitalizeLastWord = false, bool ignoreSingleLetterExceptedWords = false)
        {
            var newString = string.Empty;
            var currentWord = string.Empty;
            var prePrevChar = '\u0000';
            var prevChar = '\u0000';
            var resetCharException = false;
            var wasSingleLetterWordCharException = false;
            var isTheFirstWord = true;

            if (encounteredLeftExceptionChars == null)
                encounteredLeftExceptionChars = new List<char>();

            if (exceptedWords == null)
                exceptedWords = Array.Empty<string>();

            if (exceptionChars == null)
                exceptionChars = Array.Empty<string>();

            if (openingExceptionChars == null)
                openingExceptionChars = Array.Empty<string>();

            if (closingExceptionChars == null)
                closingExceptionChars = Array.Empty<string>();


            foreach (var currentChar in source)
            {
                //Delayed reset of char exception (to skip spaces after exception chars if any)
                if (wasCharException && !IsCharASymbol(currentChar) && (prevChar == ' ' || prevChar == MultipleItemsSplitterId
                    || ItemIndexInArray(prevChar, closingExceptionChars) >= 0))
                    resetCharException = true;

                //Let's exclude abbreviations even if they are contained in exceptedWords list (e.g. "A." if exceptedWords contains the article "a")
                if (ignoreSingleLetterExceptedWords && currentChar == '.' && currentWord.Length == 1) //-V3095
                {
                    wasSingleLetterWordCharException = true;
                }
                else
                {
                    if (prePrevChar != '.')
                        wasSingleLetterWordCharException = false;

                    //Char exception
                    if (IsCharASymbol(currentChar))
                    {
                        if (IsItemContainedInArray(currentChar, exceptionChars)) //Ignore changing case later
                            wasCharException = true;

                        if (IsItemContainedInArray(currentChar, openingExceptionChars)) //Ignore changing case later
                            encounteredLeftExceptionChars.Add(currentChar);
                    }
                }

                //Let's try to remove "between chars" changing case exception from encounteredLeftExceptionChars and mark wasCharException
                var closingIndex = ItemIndexInArray(currentChar, closingExceptionChars);
                if (closingIndex >= 0 && ItemIndexInArray(encounteredLeftExceptionChars[encounteredLeftExceptionChars.Count - 1], openingExceptionChars) == closingIndex)
                {
                    wasCharException = true;
                    encounteredLeftExceptionChars.RemoveAt(encounteredLeftExceptionChars.Count - 1);
                }


                if (IsCharASymbol(currentChar) || ItemIndexInArray(currentChar, closingExceptionChars) >= 0) //Possible end of word
                {
                    if (!IsCharASymbol(prevChar)) //End of word
                    {
                        //Always Capitalize 1st word in tag if this option is checked
                        if (alwaysCapitalize1stWord == true && isTheFirstWord)
                            newString = newString + ChangeSubstringCase(currentWord, ChangeCaseOptions.TitleCase, true) + currentChar;
                        //Ignore changing case (for UPPERCASE excepted words)
                        else if (alwaysCapitalize1stWord == null && isTheFirstWord && IsItemContainedInArray(currentWord, exceptedWords) && !useWhiteList)
                            newString = newString + currentWord + currentChar;
                        //Ignore changing case for abbreviations even if they are contained in exceptedWords list (e.g. "A." if exceptedWords contains the article "a")
                        else if (wasSingleLetterWordCharException)
                            newString = newString + currentWord + currentChar;
                        //Ignore changing case
                        else if ((wasCharException || IsItemContainedInArray(currentWord, exceptedWords)) && !useWhiteList)
                            newString = newString + currentWord + currentChar;
                        //Change case if alwaysCapitalize1stWord == null and isTheFirstWord == true 
                        else if (alwaysCapitalize1stWord == null && isTheFirstWord)
                            newString = newString + ChangeSubstringCase(currentWord, ChangeCaseOptions.TitleCase, true) + currentChar;
                        //Ignore changing case if the word is not contained in whitelist, otherwise proceed as usual
                        else if (!IsItemContainedInArray(currentWord, exceptedWords) && useWhiteList)
                            newString = newString + currentWord + currentChar;
                        //Change case
                        else
                            newString = newString + ChangeSubstringCase(currentWord, changeCaseOption, isTheFirstWord) + currentChar;


                        if (resetCharException)
                        {
                            resetCharException = false;
                            wasCharException = false;
                        }

                        currentWord = string.Empty; //Beginning of new word
                        isTheFirstWord = false;
                    }
                    else //Not the end of word, several repeating word splitters
                    {
                        newString += currentChar;
                    }
                }
                else //Not the end of word
                {
                    if (string.IsNullOrEmpty(currentWord) && !IsCharASymbol(currentChar)) //Beginning of new word
                        currentWord += currentChar;
                    else if (string.IsNullOrEmpty(currentWord)) //Several repeating symbols between words
                        newString += currentChar;
                    else //Letter or symbol in the middle of the word
                        currentWord += currentChar;
                }


                prePrevChar = prevChar;
                prevChar = currentChar;
            }


            //String is ended, so last currentWord IS a word
            if (resetCharException)
            {
                //resetCharException = false;
                wasCharException = false;
            }

            //Ignore changing case if the word is not contained in whitelist, otherwise proceed as usual
            if (!IsItemContainedInArray(currentWord, exceptedWords) && useWhiteList)
            {
                newString += currentWord;
            }
            else
            {
                //Always Capitalize last word if this option is checked, but there was no character exception
                if (alwaysCapitalizeLastWord == true && !wasCharException && encounteredLeftExceptionChars.Count == 0)
                    newString += ChangeSubstringCase(currentWord, ChangeCaseOptions.TitleCase, true);
                //Ignore changing case (for UPPERCASE excepted words)
                else if (alwaysCapitalizeLastWord == null && IsItemContainedInArray(currentWord, exceptedWords) && !useWhiteList)
                    newString += ChangeSubstringCase(currentWord, ChangeCaseOptions.TitleCase, true);
                //Ignore changing case for abbreviations even if they are contained in exceptedWords list (e.g. "A." if exceptedWords contains the article "a")
                else if (wasSingleLetterWordCharException)
                    newString += currentWord;
                //Ignore changing case
                else if (wasCharException || (IsItemContainedInArray(currentWord, exceptedWords) && !useWhiteList) || encounteredLeftExceptionChars.Count > 0)
                    newString += currentWord;
                //Change case if alwaysCapitalize1stWord == null and isTheFirstWord == true 
                else if (alwaysCapitalize1stWord == null && isTheFirstWord)
                    newString = newString + ChangeSubstringCase(currentWord, ChangeCaseOptions.TitleCase, true);
                //Ignore changing case if the word is not contained in whitelist, otherwise proceed as usual
                else if (!IsItemContainedInArray(currentWord, exceptedWords) && useWhiteList)
                    newString = newString + currentWord;
                //Change case
                else
                    newString += ChangeSubstringCase(currentWord, changeCaseOption, isTheFirstWord);
            }

            return newString;
        }

        internal static string ChangeSentenceCase(string source, ChangeCaseOptions changeCaseOption, string[] exceptedWords = null, bool useWhiteList = false,
            string[] exceptionChars = null, string[] openingExceptionChars = null, string[] closingExceptionChars = null, string[] sentenceSeparators = null,
            bool? alwaysCapitalize1stWord = true, bool? alwaysCapitalizeLastWord = true, bool ignoreSingleLetterExceptedWords = false)
        {
            var newString = string.Empty;
            var currentSentence = string.Empty;
            var prePrevChar = '\u0000';
            var prevChar = '\u0000';
            var wasCharException = false;
            var wasSingleLetterWordCharException = false;

            if (exceptedWords == null)
                exceptedWords = Array.Empty<string>();

            if (exceptionChars == null)
                exceptionChars = Array.Empty<string>();

            if (openingExceptionChars == null)
                openingExceptionChars = Array.Empty<string>();

            if (closingExceptionChars == null)
                closingExceptionChars = Array.Empty<string>();

            if (sentenceSeparators == null)
                sentenceSeparators = Array.Empty<string>();

            List<char> encounteredLeftExceptionChars = new List<char>();


            bool wasEndOfSentence = false;
            foreach (var currentChar in source)
            {
                //Let's exclude abbreviations even if they are contained in exceptedWords list (e.g. "A." if exceptedWords contains the article "a")
                if (ignoreSingleLetterExceptedWords && currentChar == '.' && !IsCharASymbol(prevChar))
                {
                    wasSingleLetterWordCharException = true;
                }
                else
                {
                    if (prePrevChar != '.')
                        wasSingleLetterWordCharException = false;

                    //Char exception
                    if (IsCharASymbol(currentChar))
                    {
                        if (IsItemContainedInArray(currentChar, openingExceptionChars)) //Ignore changing case later
                            encounteredLeftExceptionChars.Add(currentChar);
                    }

                    //Let's ignore sentence separators between opening & closing exception chars
                    if (encounteredLeftExceptionChars.Count == 0)
                    {
                        //Beginning of new sentence
                        if (currentChar == MultipleItemsSplitterId || IsItemContainedInArray(currentChar, sentenceSeparators))
                            wasEndOfSentence = true;
                        //Beginning of new sentence
                        else if (prevChar == '.' && currentChar == ' ' && !wasSingleLetterWordCharException)
                            wasEndOfSentence = true;
                    }
                }


                //Let's try to remove "between chars" changing case exception from encounteredLeftExceptionChars and mark wasCharException
                var closingIndex = ItemIndexInArray(currentChar, closingExceptionChars);
                if (closingIndex >= 0 && ItemIndexInArray(encounteredLeftExceptionChars[encounteredLeftExceptionChars.Count - 1], openingExceptionChars) == closingIndex)
                {
                    wasCharException = true;
                    encounteredLeftExceptionChars.RemoveAt(encounteredLeftExceptionChars.Count - 1);
                }

                if (wasEndOfSentence && currentChar == ' ') //Not the beginning of new sentence
                {
                    currentSentence += currentChar;
                }
                else if (wasEndOfSentence && !IsCharASymbol(currentChar)) //Beginning of new sentence
                {
                    wasEndOfSentence = false;

                    newString += ChangeWordsCase(currentSentence, changeCaseOption, ref encounteredLeftExceptionChars, ref wasCharException, exceptedWords, useWhiteList, exceptionChars,
                        openingExceptionChars, closingExceptionChars, alwaysCapitalize1stWord, alwaysCapitalizeLastWord, ignoreSingleLetterExceptedWords);

                    currentSentence = currentChar.ToString();
                }
                else //Not the beginning of new sentence
                {
                    currentSentence += currentChar;
                }


                prePrevChar = prevChar;
                prevChar = currentChar;
            }

            //String is ended, so last currentSentence IS a sentence
            newString += ChangeWordsCase(currentSentence, changeCaseOption, ref encounteredLeftExceptionChars, ref wasCharException, exceptedWords, useWhiteList, exceptionChars, openingExceptionChars,
                closingExceptionChars, alwaysCapitalize1stWord, alwaysCapitalizeLastWord, ignoreSingleLetterExceptedWords);

            return newString;
        }

        private string changeCase(string source, int changeCaseOptions, string[] exceptionWordsArg, bool useWhiteListArg, string[] exceptionCharsArg,
            string[] openingExceptionCharsArg, string[] closingExceptionCharsArg, string[] sentenceSeparatorsArg,
            bool? alwaysCapitalize1stWordArg, bool? alwaysCapitalizeLastWordArg, bool ignoreSingleLetterExceptedWordsArg)
        {
            if (changeCaseOptions == 1)
                return ChangeSentenceCase(source, ChangeCaseOptions.SentenceCase, exceptionWordsArg, useWhiteListArg, exceptionCharsArg, openingExceptionCharsArg,
                    closingExceptionCharsArg, sentenceSeparatorsArg, alwaysCapitalize1stWordArg, alwaysCapitalizeLastWordArg, ignoreSingleLetterExceptedWordsArg);
            else if (changeCaseOptions == 2)
                return ChangeSentenceCase(source, ChangeCaseOptions.LowerCase, exceptionWordsArg, useWhiteListArg, exceptionCharsArg,
                    openingExceptionCharsArg, closingExceptionCharsArg, sentenceSeparatorsArg, alwaysCapitalize1stWordArg, alwaysCapitalizeLastWordArg, ignoreSingleLetterExceptedWordsArg);
            else if (changeCaseOptions == 3)
                return ChangeSentenceCase(source, ChangeCaseOptions.UpperCase, exceptionWordsArg, useWhiteListArg, exceptionCharsArg,
                    openingExceptionCharsArg, closingExceptionCharsArg, sentenceSeparatorsArg, alwaysCapitalize1stWordArg, alwaysCapitalizeLastWordArg, ignoreSingleLetterExceptedWordsArg);
            else if (changeCaseOptions == 4)
                return ChangeSentenceCase(source, ChangeCaseOptions.TitleCase, exceptionWordsArg, useWhiteListArg, exceptionCharsArg,
                    openingExceptionCharsArg, closingExceptionCharsArg, sentenceSeparatorsArg, alwaysCapitalize1stWordArg, alwaysCapitalizeLastWordArg, ignoreSingleLetterExceptedWordsArg);
            else //if (changeCaseOptions == 5)
                return ChangeSentenceCase(source, ChangeCaseOptions.ToggleCase, exceptionWordsArg, useWhiteListArg, exceptionCharsArg,
                    openingExceptionCharsArg, closingExceptionCharsArg, sentenceSeparatorsArg, alwaysCapitalize1stWordArg, alwaysCapitalizeLastWordArg, ignoreSingleLetterExceptedWordsArg);
        }

        private bool prepareBackgroundPreview()
        {
            if (backgroundTaskIsStopping)
                backgroundTaskIsStoppedOrCancelled = true;

            if (previewIsGenerated)
            {
                resetPreviewData();
                return true;
            }

            previewIsGenerated = false;

            if (backgroundTaskIsWorking())
                return true;

            resetPreviewData();


            changeCaseFlag = getChangeCaseOptionsRadioButtons();

            useWhiteList = exceptionWordsCheckBox.CheckState == CheckState.Indeterminate;
            exceptedWords = null;
            exceptionChars = null;
            openingExceptionChars = null;
            closingExceptionChars = null;
            sentenceSeparators = null;
            alwaysCapitalize1stWord = GetNullableBoolFromCheckState(alwaysCapitalize1stWordCheckBox.CheckState);
            alwaysCapitalizeLastWord = GetNullableBoolFromCheckState(alwaysCapitalizeLastWordCheckBox.CheckState);
            ignoreSingleLetterExceptedWords = ignoreSingleLetterExceptedWordsCheckBox.Checked;

            if (exceptionWordsCheckBox.IsEnabled() && exceptionWordsCheckBox.CheckState != CheckState.Unchecked)
                exceptedWords = exceptionWordsBoxCustom.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (exceptionCharsCheckBox.IsEnabled() && exceptionCharsCheckBox.Checked)
                exceptionChars = exceptionCharsBoxCustom.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (exceptionCharPairsCheckBox.IsEnabled() && exceptionCharPairsCheckBox.Checked)
            {
                openingExceptionChars = openingExceptionCharsBoxCustom.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                closingExceptionChars = closingExceptionCharsBoxCustom.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }

            if (sentenceSeparatorsCheckBox.IsEnabled() && sentenceSeparatorsCheckBox.Checked)
                sentenceSeparators = sentenceSeparatorsBoxCustom.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            sourceTagId = GetTagId(sourceTagListCustom.Text);

            MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files);
            if (files == null || files.Length == 0)
            {
                MessageBox.Show(this, MsgNoTracksSelected, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else
            {
                previewTable.AllowUserToResizeColumns = false;
                previewTable.AllowUserToResizeRows = false;
                foreach (DataGridViewColumn column in previewTable.Columns)
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;

                previewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                previewTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

                return true;
            }
        }

        private bool prepareBackgroundTask()
        {
            if (backgroundTaskIsWorking())
                return false;

            if (rows.Count == 0 && !prepareBackgroundPreview())
                return false;

            previewTable.AllowUserToResizeColumns = false;
            previewTable.AllowUserToResizeRows = false;
            foreach (DataGridViewColumn column in previewTable.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;


            tags.Clear();

            for (var fileCounter = 0; fileCounter < rows.Count; fileCounter++)
            {
                string[] tag = { "Checked", "File", "NewTag" };

                tag[0] = rows[fileCounter].Checked;
                tag[1] = rows[fileCounter].File;
                tag[2] = rows[fileCounter].NewTagValueNormalized;

                tags.Add(tag);
            }

            ignoreClosingForm = true;

            return true;
        }

        private void previewChanges()
        {
            previewIsGenerated = true;

            var stripNotChangedLines = SavedSettings.dontIncludeInPreviewLinesWithoutChangedTags;

            for (var fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (checkStoppingStatus())
                {
                    Invoke(new Action(() => { stopButtonClickedMethod(prepareBackgroundPreview); }));
                    return;
                }

                var currentFile = files[fileCounter];

                SetStatusBarTextForFileOperations(ChangeCaseSbText, true, fileCounter, files.Length, currentFile);

                var sourceTagValue = GetFileTag(currentFile, sourceTagId);
                var sourceTagValueRepresentation = GetTagRepresentation(sourceTagValue);
                var newTagValue = changeCase(sourceTagValue, changeCaseFlag, exceptedWords, useWhiteList, exceptionChars,
                    openingExceptionChars, closingExceptionChars, sentenceSeparators, alwaysCapitalize1stWord, alwaysCapitalizeLastWord, ignoreSingleLetterExceptedWords);
                var newTagValueRepresentation = GetTagRepresentation(newTagValue);

                var track = GetTrackRepresentation(currentFile);


                string isChecked;
                if (sourceTagValue == newTagValue && stripNotChangedLines)
                    continue;
                else if (sourceTagValue == newTagValue)
                    isChecked = ColumnUncheckedState;
                else
                    isChecked = ColumnCheckedState;


                Row row = new Row
                {
                    Checked = isChecked,
                    File = currentFile,
                    Track = track,
                    OriginalTagValue = sourceTagValue,
                    OriginalTagValueNormalized = sourceTagValueRepresentation,
                    NewTagValue = newTagValue,
                    NewTagValueNormalized = newTagValueRepresentation,
                };

                rows.Add(row);


                int rowCountToFormat1 = 0;
                Invoke(new Action(() => { rowCountToFormat1 = AddRowsToTable(this, previewTable, source, rows.Count, false, true); }));
                Invoke(new Action(() => { FormatChangedTags(this, previewTable, rowCountToFormat1, false, previewTableFormatRow); }));
            }

            int rowCountToFormat2 = 0;
            Invoke(new Action(() => { rowCountToFormat2 = AddRowsToTable(this, previewTable, source, rows.Count, true, true); }));
            Invoke(new Action(() => { FormatChangedTags(this, previewTable, rowCountToFormat2, true, previewTableFormatRow); checkStoppedStatus(); resetFormToGeneratedPreview(); }));
        }

        private void reapplyRules()
        {
            if (rows.Count == 0)
            {
                MessageBox.Show(this, MsgPreviewIsNotGeneratedNothingToChange, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            changeCaseFlag = getChangeCaseOptionsRadioButtons();

            useWhiteList = exceptionWordsCheckBox.CheckState == CheckState.Indeterminate;
            exceptedWords = null;
            exceptionChars = null;
            openingExceptionChars = null;
            closingExceptionChars = null;
            sentenceSeparators = null;

            if (exceptionWordsCheckBox.IsEnabled() && exceptionWordsCheckBox.CheckState != CheckState.Unchecked)
                exceptedWords = exceptionWordsBoxCustom.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (exceptionCharsCheckBox.IsEnabled() && exceptionCharsCheckBox.Checked)
                exceptionChars = exceptionCharsBoxCustom.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (exceptionCharPairsCheckBox.IsEnabled() && exceptionCharPairsCheckBox.Checked)
            {
                openingExceptionChars = openingExceptionCharsBoxCustom.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                closingExceptionChars = closingExceptionCharsBoxCustom.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }

            if (sentenceSeparatorsCheckBox.IsEnabled() && sentenceSeparatorsCheckBox.Checked)
                sentenceSeparators = sentenceSeparatorsBoxCustom.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < rows.Count; i++)
            {
                if (rows[i].Checked == ColumnCheckedState)
                {
                    var newTagValue = changeCase(rows[i].NewTagValue, changeCaseFlag, exceptedWords, useWhiteList, exceptionChars,
                        openingExceptionChars, closingExceptionChars, sentenceSeparators, alwaysCapitalize1stWord, alwaysCapitalizeLastWord, ignoreSingleLetterExceptedWords);
                    var newTagValueRepresentation = GetTagRepresentation(newTagValue);

                    rows[i].NewTagValue = newTagValue;
                    rows[i].NewTagValueNormalized = newTagValueRepresentation;
                }
            }

            source.ResetBindings(false);
            for (var i = 0; i < rows.Count; i++)
                previewTableFormatRow(previewTable, i);
        }

        private void applyChanges()
        {
            if (tags.Count == 0)
                throw new Exception("Something went wrong! Empty 'tags' local variable (must be filled on generating preview.)");

            processedRowList.Clear(); //Indices of processed tracks
            Invoke(new Action(() => { previewTable.CurrentCell = previewTable.Rows[0].Cells[0]; }));
            Invoke(new Action(() => { previewTable.FirstDisplayedCell = previewTable.CurrentCell; }));

            for (var i = 0; i < tags.Count; i++)
            {
                if (checkStoppingStatus())
                {
                    Invoke(new Action(() => { stopButtonClickedMethod(applyingChangesStopped); }));
                    return;
                }


                var isChecked = tags[i][0];

                if (isChecked == ColumnCheckedState)
                {
                    var currentFile = tags[i][1];
                    var newTagValue = tags[i][2];

                    tags[i][0] = string.Empty;

                    processedRowList.Add(true);
                    SetStatusBarTextForFileOperations(ChangeCaseSbText, false, i, tags.Count, currentFile);

                    SetFileTag(currentFile, sourceTagId, newTagValue);
                    CommitTagsToFile(currentFile);
                }
                else
                {
                    processedRowList.Add(false);
                }
            }

            Invoke(new Action(() => { applyingChangesStopped(); }));

            RefreshPanels(true);
            SetResultingSbText();
        }

        private void saveSettings()
        {
            saveWindowLayout(previewTable.Columns[1].Width, previewTable.Columns[2].Width, previewTable.Columns[3].Width);

            SavedSettings.changeCaseSourceTagName = sourceTagListCustom.Text;
            SavedSettings.changeCaseFlag = getChangeCaseOptionsRadioButtons();

            SavedSettings.useExceptionWords = GetNullableBoolFromCheckState(exceptionWordsCheckBox.CheckState);
            exceptionWordsBoxCustom.CopyTo(SavedSettings.exceptedWords, 0);

            SavedSettings.useExceptionChars = exceptionCharsCheckBox.Checked;
            exceptionCharsBoxCustom.Items.CopyTo(SavedSettings.exceptionChars, 0);

            SavedSettings.useExceptionCharPairs = exceptionCharPairsCheckBox.Checked;
            openingExceptionCharsBoxCustom.Items.CopyTo(SavedSettings.openingExceptionChars, 0);
            closingExceptionCharsBoxCustom.Items.CopyTo(SavedSettings.closingExceptionChars, 0);

            SavedSettings.useSentenceSeparators = sentenceSeparatorsCheckBox.Checked;
            sentenceSeparatorsBoxCustom.Items.CopyTo(SavedSettings.sentenceSeparators, 0);

            SavedSettings.alwaysCapitalize1stWord = GetNullableBoolFromCheckState(alwaysCapitalize1stWordCheckBox.CheckState);
            SavedSettings.alwaysCapitalizeLastWord = GetNullableBoolFromCheckState(alwaysCapitalizeLastWordCheckBox.CheckState);

            SavedSettings.ignoreSingleLetterExceptedWords = ignoreSingleLetterExceptedWordsCheckBox.Checked;

            TagToolsPlugin.SaveSettings();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            saveSettings();
            if (prepareBackgroundTask())
                switchOperation(applyChanges, sender as Button, buttonOK, buttonPreview, buttonClose, true, null);
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            ignoreClosingForm = clickOnPreviewButton(prepareBackgroundPreview, previewChanges, buttonPreview, buttonOK, buttonClose);

            if (recordMode)
                buttonPreview.Enable(false);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonReapply_Click(object sender, EventArgs e)
        {
            if (recordMode)
                saveStep();

            reapplyRules();
        }

        private void exceptWordsCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            exceptionWordsBoxCustom.Enable(exceptionWordsCheckBox.IsEnabled() && exceptionWordsCheckBox.CheckState != CheckState.Unchecked);
            removeExceptionButton.Enable(exceptionWordsCheckBox.IsEnabled() && exceptionWordsCheckBox.CheckState != CheckState.Unchecked);

            if (exceptionWordsCheckBox.CheckState == CheckState.Indeterminate)
                exceptionWordsCheckBoxLabel.Text = changeOnlyWordsText;
            else
                exceptionWordsCheckBoxLabel.Text = exceptForWordsText;
        }

        private void exceptionWordsCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!exceptionWordsCheckBox.IsEnabled())
                return;

            exceptionWordsCheckBox.CheckState = GetNextCheckState(exceptionWordsCheckBox.CheckState);
        }

        private void exceptCharsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            exceptionCharsBoxCustom.Enable(exceptionCharsCheckBox.Checked);
            buttonAsrExceptWordsAfterSymbols.Enable(exceptionCharsCheckBox.Checked);
        }

        private void exceptionCharsCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!exceptionCharsCheckBox.IsEnabled())
                return;

            exceptionCharsCheckBox.Checked = !exceptionCharsCheckBox.Checked;
        }

        private void exceptionCharPairsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!exceptionCharPairsCheckBox.Checked && !CheckIfTheSameNumberOfCharsInStrings(openingExceptionCharsBoxCustom.Text, closingExceptionCharsBoxCustom.Text))
            {
                openingExceptionCharsBoxCustom.Enable(true);
                closingExceptionCharsBoxCustom.Enable(true);
                exceptionCharPairsCheckBox.Checked = true;

                ActiveControl = closingExceptionCharsBoxCustom;
                closingExceptionCharsBox_Leave(null, null);
            }
            else
            {
                openingExceptionCharsBoxCustom.Enable(exceptionCharPairsCheckBox.Checked);
                closingExceptionCharsBoxCustom.Enable(exceptionCharPairsCheckBox.Checked);
                buttonAsrExceptWordsBetweenSymbols.Enable(exceptionCharPairsCheckBox.Checked);
            }
        }

        private void exceptionCharPairsCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!exceptionCharPairsCheckBox.IsEnabled())
                return;

            exceptionCharPairsCheckBox.Checked = !exceptionCharPairsCheckBox.Checked;
        }

        private void sentenceSeparatorsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            sentenceSeparatorsBoxCustom.Enable(sentenceSeparatorsCheckBox.Checked);
            buttonAsrSentenceSeparators.Enable(sentenceSeparatorsCheckBox.Checked);
        }

        private void sentenceSeparatorsCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!sentenceSeparatorsCheckBox.IsEnabled())
                return;

            sentenceSeparatorsCheckBox.Checked = !sentenceSeparatorsCheckBox.Checked;
        }

        private void cbHeader_OnCheckBoxClicked(bool state)
        {
            if (rows.Count == 0)
                return;


            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[0].Checked == null)
                    continue;

                if (state)
                    rows[0].Checked = ColumnUncheckedState;
                else
                    rows[0].Checked = ColumnCheckedState;
            }

            int firstRow = previewTable.FirstDisplayedCell.RowIndex;

            source.ResetBindings(false);
            for (int i = 0; i < rows.Count; i++)
                previewTableFormatRow(previewTable, i);

            var firstCell = previewTable.Rows[firstRow].Cells[0];
            previewTable.FirstDisplayedCell = firstCell;
        }

        private void previewTableFormatRow(DataGridView dataGridView, int rowIndex)
        {
            if (SavedSettings.dontHighlightChangedTags)
                return;

            if (dataGridView.Rows[rowIndex].Cells[0].Value as string != ColumnCheckedState)
            {
                for (var columnIndex = 1; columnIndex < dataGridView.ColumnCount; columnIndex++)
                {
                    if (dataGridView.Columns[columnIndex].Visible)
                        dataGridView.Rows[rowIndex].Cells[columnIndex].Style = dimmedCellStyle;
                }

                return;
            }


            for (var columnIndex = 1; columnIndex < dataGridView.ColumnCount; columnIndex++)
            {
                if (columnIndex == 6)
                {
                    if (dataGridView.Rows[rowIndex].Cells[3].Value as string == dataGridView.Rows[rowIndex].Cells[5].Value as string)
                        dataGridView.Rows[rowIndex].Cells[6].Style = unchangedCellStyle;
                    else
                        dataGridView.Rows[rowIndex].Cells[6].Style = changedCellStyle;
                }
                else
                {
                    dataGridView.Rows[rowIndex].Cells[columnIndex].Style = unchangedCellStyle;
                }
            }
        }

        private void previewTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string sourceTagValue;

                var isChecked = previewTable.Rows[e.RowIndex].Cells[0].Value as string;

                if (isChecked == ColumnCheckedState)
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = ColumnUncheckedState;

                    sourceTagValue = previewTable.Rows[e.RowIndex].Cells[3].Value as string;
                    var sourceTagTValue = previewTable.Rows[e.RowIndex].Cells[4].Value as string;

                    previewTable.Rows[e.RowIndex].Cells[5].Value = sourceTagValue;
                    previewTable.Rows[e.RowIndex].Cells[6].Value = sourceTagTValue;
                }
                else if (isChecked == ColumnUncheckedState)
                {
                    changeCaseFlag = getChangeCaseOptionsRadioButtons();

                    useWhiteList = exceptionWordsCheckBox.CheckState == CheckState.Indeterminate;
                    exceptedWords = null;
                    exceptionChars = null;
                    openingExceptionChars = null;
                    closingExceptionChars = null;
                    sentenceSeparators = null;

                    if (exceptionWordsCheckBox.IsEnabled() && exceptionWordsCheckBox.CheckState != CheckState.Unchecked)
                        exceptedWords = exceptionWordsBoxCustom.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (exceptionCharsCheckBox.IsEnabled() && exceptionCharsCheckBox.Checked)
                        exceptionChars = exceptionCharsBoxCustom.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (exceptionCharPairsCheckBox.IsEnabled() && exceptionCharPairsCheckBox.Checked)
                    {
                        openingExceptionChars = openingExceptionCharsBoxCustom.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        closingExceptionChars = closingExceptionCharsBoxCustom.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    }

                    if (sentenceSeparatorsCheckBox.IsEnabled() && sentenceSeparatorsCheckBox.Checked)
                        sentenceSeparators = sentenceSeparatorsBoxCustom.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    previewTable.Rows[e.RowIndex].Cells[0].Value = ColumnCheckedState;

                    sourceTagValue = previewTable.Rows[e.RowIndex].Cells[3].Value as string;

                    var newTagValue = changeCase(sourceTagValue, changeCaseFlag, exceptedWords, useWhiteList, exceptionChars,
                        openingExceptionChars, closingExceptionChars, sentenceSeparators, alwaysCapitalize1stWord, alwaysCapitalizeLastWord, ignoreSingleLetterExceptedWords);
                    var newTagTValue = GetTagRepresentation(newTagValue);

                    previewTable.Rows[e.RowIndex].Cells[5].Value = newTagValue;
                    previewTable.Rows[e.RowIndex].Cells[6].Value = newTagTValue;
                }

                previewTableFormatRow(previewTable, e.RowIndex);
            }
        }

        internal override void enableDisablePreviewOptionControls(bool enable, bool dontChangeDisabled = false)
        {
            buttonPlayPreset.Enable(enable && !previewIsGenerated);
            buttonRecordPreset.Enable(enable && !previewIsGenerated);

            sourceTagListCustom.Enable(enable);


            enable = previewIsGenerated || !backgroundTaskIsScheduled || backgroundTaskIsStoppedOrCancelled;

            sentenceCaseRadioButton.Enable(enable);
            lowerCaseRadioButton.Enable(enable);
            upperCaseRadioButton.Enable(enable);
            titleCaseRadioButton.Enable(enable);
            toggleCaseRadioButton.Enable(enable);

            exceptionWordsCheckBox.Enable(enable);
            exceptionCharsCheckBox.Enable(enable);
            exceptionCharPairsCheckBox.Enable(enable);
            sentenceSeparatorsCheckBox.Enable(enable);

            alwaysCapitalize1stWordCheckBox.Enable(enable);
            alwaysCapitalizeLastWordCheckBox.Enable(enable);
            ignoreSingleLetterExceptedWordsCheckBox.Enable(enable);


            exceptionWordsBoxCustom.Enable(enable && exceptionWordsCheckBox.Checked);
            exceptionCharsBoxCustom.Enable(enable && exceptionCharsCheckBox.Checked);
            openingExceptionCharsBoxCustom.Enable(enable && exceptionCharPairsCheckBox.Checked);
            closingExceptionCharsBoxCustom.Enable(enable && exceptionCharPairsCheckBox.Checked);
            sentenceSeparatorsBoxCustom.Enable(enable && sentenceSeparatorsCheckBox.Checked);

            removeExceptionButton.Enable(enable && exceptionWordsCheckBox.Checked);
            buttonAsrExceptWordsAfterSymbols.Enable(enable && exceptionCharsCheckBox.Checked);
            buttonAsrExceptWordsBetweenSymbols.Enable(enable && exceptionCharPairsCheckBox.Checked);
            buttonAsrSentenceSeparators.Enable(enable && sentenceSeparatorsCheckBox.Checked);
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
            buttonOK.Enable((previewIsGenerated || SavedSettings.allowCommandExecutionWithoutPreview) && !recordMode && !backgroundTaskIsStopping 
                && (!backgroundTaskIsWorking() || backgroundTaskIsUpdatingTags));
            buttonPreview.Enable(true);
            buttonReapply.Enable(previewIsGenerated && !backgroundTaskIsWorking());
        }

        internal override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enable(false);
            buttonPreview.Enable(false);
            buttonReapply.Enable(false);
        }

        private void exceptionWordsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (recordMode)
            {
                if (ignoreComboBoxSelectedIndexChanged)
                    return;

                currentStep.exceptionWordsIndex = exceptionWordsBoxCustom.GetItemSpecialStateIndex(exceptionWordsBoxCustom.SelectedIndex);
            }

            if (exceptionWordsEnumComboBox == null)
                CustomComboBoxLeave(exceptionWordsBoxCustom, exceptionWordsBoxCustom.Text, addSubstituteListBoxSpecialState, exceptionWordsBoxCustom.GetDefaultSpecialState());
            else
                CustomComboBoxLeave(exceptionWordsBoxCustom, exceptionWordsBoxCustom.Text, addSubstituteComboBoxSpecialState, exceptionWordsBoxCustom.GetDefaultSpecialState());
        }

        private void exceptionWordsBox_Leave(object sender, EventArgs e)
        {
            exceptionWordsBoxCustom.Text = Regex.Replace(exceptionWordsBoxCustom.Text.Trim(' '), @"\s{2,}", " ");


            if (exceptionWordsEnumComboBox == null && !exceptionWordsBoxCustom.IsFocused())
                CustomComboBoxLeave(exceptionWordsBoxCustom, exceptionWordsBoxCustom.Text, addSubstituteListBoxSpecialState, exceptionWordsBoxCustom.GetDefaultSpecialState());
            else if (exceptionWordsEnumComboBox != null)
                CustomComboBoxLeave(exceptionWordsBoxCustom, exceptionWordsBoxCustom.Text, addSubstituteComboBoxSpecialState, exceptionWordsBoxCustom.GetDefaultSpecialState());
        }

        private void exceptionCharsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (recordMode)
            {
                if (ignoreComboBoxSelectedIndexChanged)
                    return;

                currentStep.exceptionCharsIndex = exceptionCharsBoxCustom.GetItemSpecialStateIndex(exceptionCharsBoxCustom.SelectedIndex);
            }

            if (exceptionCharsEnumComboBox == null)
                CustomComboBoxLeave(exceptionCharsBoxCustom, exceptionCharsBoxCustom.Text, addSubstituteListBoxSpecialState, exceptionCharsBoxCustom.GetDefaultSpecialState());
            else
                CustomComboBoxLeave(exceptionCharsBoxCustom, exceptionCharsBoxCustom.Text, addSubstituteComboBoxSpecialState, exceptionCharsBoxCustom.GetDefaultSpecialState());
        }

        private void exceptionCharsBox_Leave(object sender, EventArgs e)
        {
            exceptionCharsBoxCustom.Text = Regex.Replace(exceptionCharsBoxCustom.Text.Trim(' '), @"\s{2,}", " ");


            if (exceptionCharsEnumComboBox == null && !exceptionCharsBoxCustom.IsFocused())
                CustomComboBoxLeave(exceptionCharsBoxCustom, exceptionCharsBoxCustom.Text, addSubstituteListBoxSpecialState, exceptionCharsBoxCustom.GetDefaultSpecialState());
            else if (exceptionCharsEnumComboBox != null)
                CustomComboBoxLeave(exceptionCharsBoxCustom, exceptionCharsBoxCustom.Text, addSubstituteComboBoxSpecialState, exceptionCharsBoxCustom.GetDefaultSpecialState());
        }

        private void openingExceptionCharsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (recordMode)
            {
                if (ignoreComboBoxSelectedIndexChanged)
                    return;

                currentStep.exceptionCharPair1Index = openingExceptionCharsBoxCustom.GetItemSpecialStateIndex(openingExceptionCharsBoxCustom.SelectedIndex);
            }

            if (openingExceptionCharsEnumComboBox == null)
                CustomComboBoxLeave(openingExceptionCharsBoxCustom, openingExceptionCharsBoxCustom.Text, addSubstituteListBoxSpecialState, openingExceptionCharsBoxCustom.GetDefaultSpecialState());
            else
                CustomComboBoxLeave(openingExceptionCharsBoxCustom, openingExceptionCharsBoxCustom.Text, addSubstituteComboBoxSpecialState, openingExceptionCharsBoxCustom.GetDefaultSpecialState());
        }

        private void openingExceptionCharsBox_Leave(object sender, EventArgs e)
        {
            openingExceptionCharsBoxCustom.Text = Regex.Replace(openingExceptionCharsBoxCustom.Text.Trim(' '), @"\s{2,}", " ");

            if (ActiveControl != closingExceptionCharsBoxCustom && !CheckIfTheSameNumberOfCharsInStrings(openingExceptionCharsBoxCustom.Text, closingExceptionCharsBoxCustom.Text))
            {
                closingExceptionCharsBoxCustom.Focus();
                closingExceptionCharsBoxCustom.SelectionStart = closingExceptionCharsBoxCustom.Text.Length + 1;
                closingExceptionCharsBoxCustom.SelectionLength = 0;
            }

            if (openingExceptionCharsEnumComboBox == null && !openingExceptionCharsBoxCustom.IsFocused())
                CustomComboBoxLeave(openingExceptionCharsBoxCustom, openingExceptionCharsBoxCustom.Text, addSubstituteListBoxSpecialState, openingExceptionCharsBoxCustom.GetDefaultSpecialState());
            else if (openingExceptionCharsEnumComboBox != null)
                CustomComboBoxLeave(openingExceptionCharsBoxCustom, openingExceptionCharsBoxCustom.Text, addSubstituteComboBoxSpecialState, openingExceptionCharsBoxCustom.GetDefaultSpecialState());
        }

        private void closingExceptionCharsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (recordMode)
            {
                if (ignoreComboBoxSelectedIndexChanged)
                    return;

                currentStep.exceptionCharPair2Index = closingExceptionCharsBoxCustom.GetItemSpecialStateIndex(closingExceptionCharsBoxCustom.SelectedIndex);
            }

            if (closingExceptionCharsEnumComboBox == null)
                CustomComboBoxLeave(closingExceptionCharsBoxCustom, closingExceptionCharsBoxCustom.Text, addSubstituteListBoxSpecialState, closingExceptionCharsBoxCustom.GetDefaultSpecialState());
            else
                CustomComboBoxLeave(closingExceptionCharsBoxCustom, closingExceptionCharsBoxCustom.Text, addSubstituteComboBoxSpecialState, closingExceptionCharsBoxCustom.GetDefaultSpecialState());
        }

        private void closingExceptionCharsBox_Leave(object sender, EventArgs e)
        {
            if (closingExceptionCharsBoxLeaving)
                return;


            closingExceptionCharsBoxCustom.Text = Regex.Replace(closingExceptionCharsBoxCustom.Text.Trim(' '), @"\s{2,}", " ");

            if (ActiveControl != openingExceptionCharsBoxCustom && !CheckIfTheSameNumberOfCharsInStrings(openingExceptionCharsBoxCustom.Text, closingExceptionCharsBoxCustom.Text))
            {
                closingExceptionCharsBoxLeaving = true;

                MessageBox.Show(MbForm, MsgCsTheNumberOfOpeningExceptionCharactersMustBe,
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);

                closingExceptionCharsBoxLeaving = false;

                closingExceptionCharsBoxCustom.Focus();
                closingExceptionCharsBoxCustom.SelectionStart = closingExceptionCharsBoxCustom.Text.Length + 1;
                closingExceptionCharsBoxCustom.SelectionLength = 0;
            }

            if (closingExceptionCharsEnumComboBox == null && !closingExceptionCharsBoxCustom.IsFocused())
                CustomComboBoxLeave(closingExceptionCharsBoxCustom, closingExceptionCharsBoxCustom.Text, addSubstituteListBoxSpecialState, closingExceptionCharsBoxCustom.GetDefaultSpecialState());
            else if (closingExceptionCharsEnumComboBox != null)
                CustomComboBoxLeave(closingExceptionCharsBoxCustom, closingExceptionCharsBoxCustom.Text, addSubstituteComboBoxSpecialState, closingExceptionCharsBoxCustom.GetDefaultSpecialState());
        }

        private void sentenceSeparatorsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (recordMode)
            {
                if (ignoreComboBoxSelectedIndexChanged)
                    return;

                currentStep.sentenceSeparatorsIndex = sentenceSeparatorsBoxCustom.GetItemSpecialStateIndex(sentenceSeparatorsBoxCustom.SelectedIndex);
            }

            if (sentenceSeparatorsEnumComboBox == null)
                CustomComboBoxLeave(sentenceSeparatorsBoxCustom, sentenceSeparatorsBoxCustom.Text, addSubstituteListBoxSpecialState, sentenceSeparatorsBoxCustom.GetDefaultSpecialState());
            else
                CustomComboBoxLeave(sentenceSeparatorsBoxCustom, sentenceSeparatorsBoxCustom.Text, addSubstituteComboBoxSpecialState, sentenceSeparatorsBoxCustom.GetDefaultSpecialState());
        }

        private void sentenceSeparatorsBox_Leave(object sender, EventArgs e)
        {
            sentenceSeparatorsBoxCustom.Text = Regex.Replace(sentenceSeparatorsBoxCustom.Text.Trim(' '), @"\s{2,}", " ");


            if (sentenceSeparatorsEnumComboBox == null && !sentenceSeparatorsBoxCustom.IsFocused())
                CustomComboBoxLeave(sentenceSeparatorsBoxCustom, sentenceSeparatorsBoxCustom.Text, addSubstituteListBoxSpecialState, sentenceSeparatorsBoxCustom.GetDefaultSpecialState());
            else if (sentenceSeparatorsEnumComboBox != null)
                CustomComboBoxLeave(sentenceSeparatorsBoxCustom, sentenceSeparatorsBoxCustom.Text, addSubstituteComboBoxSpecialState, sentenceSeparatorsBoxCustom.GetDefaultSpecialState());
        }

        internal static string[] GetCharsInString(string text)
        {
            if (text == null)
                return null;

            //Let's count the number of chars except for spaces
            var count = 0;
            foreach (var ch in text)
                if (ch != ' ')
                    count++;

            var chars = new string[count];
            count = 0;
            foreach (var ch in text)
                if (ch != ' ')
                    chars[count++] = ch.ToString();

            return chars;
        }

        internal static bool CheckIfTheSameNumberOfCharsInStrings(string text1, string text2)
        {
            if (text1 == null && text2 != null)
                return false;
            else if (text1 != null && text2 == null)
                return false;
            else if (text1 == null && text2 == null)
                return true;

            var leftItems = GetCharsInString(text1);
            var rightItems = GetCharsInString(text2);

            if (leftItems.Length != rightItems.Length) //-V3080
                return false;

            return true;
        }

        private void casingRuleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            //Nothing for now...
        }

        private void sentenceCaseRadioButtonLabel_Click(object sender, EventArgs e)
        {
            if (!sentenceCaseRadioButton.IsEnabled())
                return;

            sentenceCaseRadioButton.Checked = true;
        }

        private void lowerCaseRadioButtonLabel_Click(object sender, EventArgs e)
        {
            if (!lowerCaseRadioButton.IsEnabled())
                return;

            lowerCaseRadioButton.Checked = true;
        }

        private void upperCaseRadioButtonLabel_Click(object sender, EventArgs e)
        {
            if (!upperCaseRadioButton.IsEnabled())
                return;

            upperCaseRadioButton.Checked = true;
        }

        private void titleCaseRadioButtonLabel_Click(object sender, EventArgs e)
        {
            if (!titleCaseRadioButton.IsEnabled())
                return;

            titleCaseRadioButton.Checked = true;
        }

        private void toggleCaseRadioButtonLabel_Click(object sender, EventArgs e)
        {
            if (!toggleCaseRadioButton.IsEnabled())
                return;

            toggleCaseRadioButton.Checked = true;
        }

        private void removeExceptionButton_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < exceptionWordsBoxCustom.Items.Count; i++)
            {
                if (exceptionWordsBoxCustom.Items[i].ToString() == exceptionWordsBoxCustom.Text)
                {
                    exceptionWordsBoxCustom.Items.Remove(exceptionWordsBoxCustom.Text);
                    exceptionWordsBoxCustom.Items.Add(string.Empty);

                    exceptionWordsBoxCustom.Text = string.Empty;

                    break;
                }
            }
        }

        private void buttonAsr_Click(object sender, EventArgs e)
        {
            SavedSettings.exceptionWordsAsr = exceptionWordsBoxCustom.Text;
        }

        private void buttonAsrExceptWordsAfterSymbols_Click(object sender, EventArgs e)
        {
            SavedSettings.exceptionCharsAsr = exceptionCharsBoxCustom.Text;
        }

        private void buttonAsrExceptWordsBetweenSymbols_Click(object sender, EventArgs e)
        {
            SavedSettings.openingExceptionCharsAsr = openingExceptionCharsBoxCustom.Text;
            SavedSettings.closingExceptionCharsAsr = closingExceptionCharsBoxCustom.Text;
        }

        private void buttonAsrSentenceSeparators_Click(object sender, EventArgs e)
        {
            SavedSettings.sentenceSeparatorsAsr = sentenceSeparatorsBoxCustom.Text;
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            var settings = new QuickSettings(TagToolsPlugin);
            Display(settings, true);
        }

        private void alwaysCapitalize1stWordCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!alwaysCapitalize1stWordCheckBox.IsEnabled())
                return;

            alwaysCapitalize1stWordCheckBox.CheckState = GetNextCheckState(alwaysCapitalize1stWordCheckBox.CheckState);
        }

        private void alwaysCapitalizeLastWordCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!alwaysCapitalizeLastWordCheckBox.IsEnabled())
                return;

            alwaysCapitalizeLastWordCheckBox.CheckState = GetNextCheckState(alwaysCapitalizeLastWordCheckBox.CheckState);
        }

        private void ignoreSingleLetterExceptedWordsCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!ignoreSingleLetterExceptedWordsCheckBox.IsEnabled())
                return;

            ignoreSingleLetterExceptedWordsCheckBox.Checked = !ignoreSingleLetterExceptedWordsCheckBox.Checked;
        }

        private void ChangeCase_Load(object sender, EventArgs e)
        {
            var value = loadWindowLayout();

            if (value.Item1 != 0)
            {
                previewTable.Columns[2].FillWeight = value.Item1;
                previewTable.Columns[4].FillWeight = value.Item2;
                previewTable.Columns[6].FillWeight = value.Item3;
            }

        }

        private void ChangeCase_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ignoreClosingForm)
            {
                if (!backgroundTaskIsUpdatingTags)
                {
                    closeFormOnStopping = true;
                    buttonClose.Enable(false);

                    backgroundTaskIsStopping = true;
                    SetStatusBarText(ChangeCaseSbText + SbTextStoppingCurrentOperation, false);

                    e.Cancel = true;
                }
            }
            else
            {
                saveWindowLayout(previewTable.Columns[2].Width, previewTable.Columns[4].Width, previewTable.Columns[6].Width);
            }
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, helpMessage);
        }

        private void buttonDescription_Click(object sender, EventArgs e)
        {
            if (presetBox.SelectedItem != null)
                MessageBox.Show(this, (presetBox.SelectedItem as ChangeCasePreset).getDescription());
        }

        private void buttonDeletePreset_Click(object sender, EventArgs e)
        {
            presetBox.Items.Remove(presetBox.SelectedItem);
        }

        private void buttonPlayPreset_Click(object sender, EventArgs e)//************
        {

        }

        private void saveStep()
        {
            currentStep.rule = getChangeCaseOptionsRadioButtons();
            currentStep.exceptionWordsState = GetNullableBoolFromCheckState(exceptionWordsCheckBox.CheckState);
            currentStep.alwaysCapitalize1stWord = alwaysCapitalize1stWordCheckBox.Checked;
            currentStep.alwaysCapitalizeLastWord = alwaysCapitalizeLastWordCheckBox.Checked;
            currentStep.ignoreSingleLetterExceptedWords = ignoreSingleLetterExceptedWordsCheckBox.Checked;

            recordedPreset.steps.Add(currentStep);
            currentStep = new ChangeCaseStep();
        }

        private void buttonRecordPreset_Click(object sender, EventArgs e)
        {
            buttonOK.Enable(false);

            presetBox.Enable(false);
            buttonDeletePreset.Enable(false);
            buttonPlayPreset.Enable(false);
            buttonRecordPreset.Enable(false);
            buttonStopRecordingPreset.Enable(true);

            recordMode = true;
            recordedPreset = new ChangeCasePreset();
            currentStep = new ChangeCaseStep();
        }

        private void buttonStopRecordingPreset_Click(object sender, EventArgs e)
        {
            currentStep = null;

            bool nameDefined;
            using (var tagToolsForm = new ChangeCasePresetNaming(TagToolsPlugin))
                nameDefined = tagToolsForm.editPreset(recordedPreset);

            if (nameDefined && !string.IsNullOrEmpty(GetDictValue(recordedPreset.names, Language)))//*************
            {
                presetBoxCustom.Add(recordedPreset);
                presetBoxCustom.SelectedItem = recordedPreset;
            }

            recordedPreset = null;
            recordMode = false;

            buttonOK.Enable(false);

            presetBox.Enable(true);
            buttonDeletePreset.Enable(true);
            buttonPlayPreset.Enable(true);
            buttonRecordPreset.Enable(true);
            buttonStopRecordingPreset.Enable(false);
        }

        private void presetBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((presetBox.SelectedItem as ChangeCasePreset)?.predefined == true)
                buttonDeletePreset.Enable(false);
            else
                buttonDeletePreset.Enable(true);
        }
    }
}
