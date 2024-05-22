using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

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


        private CustomComboBox sourceTagListCustom;
        private CustomComboBox exceptionWordsBoxCustom;
        private CustomComboBox exceptionCharsBoxCustom;
        private CustomComboBox leftExceptionCharsBoxCustom;
        private CustomComboBox rightExceptionCharsBoxCustom;
        private CustomComboBox wordSeparatorsBoxCustom;


        private readonly DataGridViewCellStyle unchangedCellStyle = new DataGridViewCellStyle(UnchangedCellStyle);
        private readonly DataGridViewCellStyle changedCellStyle = new DataGridViewCellStyle(ChangedCellStyle);
        private readonly DataGridViewCellStyle dimmedCellStyle = new DataGridViewCellStyle(DimmedCellStyle);

        private delegate void AddRowToTable(string[] row);
        private delegate void ProcessRowOfTable(int row);
        private AddRowToTable addRowToTable;
        private ProcessRowOfTable processRowOfTable;

        private delegate void UpdateCustomScrollBarsDelegate(DataGridView dataGridView);
        private UpdateCustomScrollBarsDelegate updateCustomScrollBars;

        private string[] files = new string[0];
        private readonly List<string[]> tags = new List<string[]>();
        private MetaDataType sourceTagId;

        private int changeCaseFlag;
        private bool useWhiteList;
        private string[] exceptionWords;
        private string[] exceptionChars;
        private string[] leftExceptionChars;
        private string[] rightExceptionChars;
        private string[] wordSeparators;
        private bool alwaysCapitalize1stWord;
        private bool alwaysCapitalizeLastWord;

        private string wordSeparatorsLabel;
        private string wordSeparatorsSentenceLabel;

        internal ChangeCase(Plugin plugin) : base(plugin)
        {
            InitializeComponent();
        }

        protected override void initializeForm()
        {
            base.initializeForm();

            wordSeparatorsSentenceLabel = wordSeparatorsCheckBoxLabel.Text;
            wordSeparatorsLabel = toolTip1.GetToolTip(wordSeparatorsCheckBoxLabel);
            toolTip1.SetToolTip(wordSeparatorsCheckBoxLabel, string.Empty);

            sourceTagListCustom = namesComboBoxes["sourceTagList"];

            exceptionWordsBoxCustom = namesComboBoxes["exceptionWordsBox"];
            exceptionWordsBoxCustom.Leave += new EventHandler(exceptionWordsBox_Leave);

            exceptionCharsBoxCustom = namesComboBoxes["exceptionCharsBox"];
            exceptionCharsBoxCustom.Leave += new EventHandler(exceptionCharsBox_Leave);

            leftExceptionCharsBoxCustom = namesComboBoxes["leftExceptionCharsBox"];
            leftExceptionCharsBoxCustom.Leave += new EventHandler(leftExceptionCharsBox_Leave);

            rightExceptionCharsBoxCustom = namesComboBoxes["rightExceptionCharsBox"];
            rightExceptionCharsBoxCustom.Leave += new EventHandler(rightExceptionCharsBox_Leave);

            wordSeparatorsBoxCustom = namesComboBoxes["wordSeparatorsBox"];
            wordSeparatorsBoxCustom.Leave += new EventHandler(wordSeparatorsBox_Leave);


            removeExceptionButton.Image = ReplaceBitmap(removeExceptionButton.Image, ButtonRemoveImage);
            buttonSettings.Image = ReplaceBitmap(buttonSettings.Image, Gear);

            FillListByTagNames(sourceTagListCustom.Items);
            sourceTagListCustom.Text = SavedSettings.changeCaseSourceTagName;

            setChangeCaseOptionsRadioButtons(SavedSettings.changeCaseFlag);

            exceptionWordsCheckBox.Checked = SavedSettings.useExceptionWords;
            onlyWordsCheckBox.Checked = SavedSettings.useOnlyWords;
            exceptionWordsBoxCustom.AddRange(SavedSettings.exceptionWords);
            exceptionWordsBoxCustom.Text = SavedSettings.exceptionWords[0];

            exceptionCharsCheckBox.Checked = SavedSettings.useExceptionChars;
            exceptionCharsBoxCustom.AddRange(SavedSettings.exceptionChars);
            exceptionCharsBoxCustom.Text = SavedSettings.exceptionChars[0];

            exceptionCharPairsCheckBox.Checked = SavedSettings.useExceptionCharPairs;
            leftExceptionCharsBoxCustom.AddRange(SavedSettings.leftExceptionChars);
            leftExceptionCharsBoxCustom.Text = SavedSettings.leftExceptionChars[0];
            rightExceptionCharsBoxCustom.AddRange(SavedSettings.rightExceptionChars);
            rightExceptionCharsBoxCustom.Text = SavedSettings.rightExceptionChars[0];

            wordSeparatorsCheckBox.Checked = SavedSettings.useWordSeparators;
            wordSeparatorsBoxCustom.AddRange(SavedSettings.wordSeparators);
            wordSeparatorsBoxCustom.Text = SavedSettings.wordSeparators[0];

            alwaysCapitalize1stWordCheckBox.Checked = SavedSettings.alwaysCapitalize1stWord;
            alwaysCapitalizeLastWordCheckBox.Checked = SavedSettings.alwaysCapitalizeLastWord;

            exceptWordsCheckBox_CheckedChanged(null, null);
            exceptCharsCheckBox_CheckedChanged(null, null);
            exceptionCharPairsCheckBox_CheckedChanged(null, null);
            wordSeparatorsCheckBox_CheckedChanged(null, null);
            casingRuleRadioButton_CheckedChanged(null, null);


            var headerCellStyle = new DataGridViewCellStyle(HeaderCellStyle);

            DataGridViewCheckBoxHeaderCell cbHeader = new DataGridViewCheckBoxHeaderCell();
            cbHeader.Style = headerCellStyle;
            cbHeader.setState(true);
            cbHeader.OnCheckBoxClicked += new CheckBoxClickedHandler(cbHeader_OnCheckBoxClicked);

            DataGridViewCheckBoxColumn colCB = new DataGridViewCheckBoxColumn
            {
                HeaderCell = cbHeader,
                ThreeState = true,
                FalseValue = "F",
                TrueValue = "T",
                IndeterminateValue = string.Empty,
                Width = 25,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };

            previewTable.Columns.Insert(0, colCB);

            previewTable.Columns[2].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[4].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[6].HeaderCell.Style = headerCellStyle;

            previewTable.Columns[2].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[4].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[6].HeaderCell.Style.WrapMode = DataGridViewTriState.True;

            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            addRowToTable = previewTable_AddRowToTable;
            processRowOfTable = previewTable_ProcessRowOfTable;

            updateCustomScrollBars = UpdateCustomScrollBars;


            enableDisablePreviewOptionControls(true, true);
            enableQueryingOrUpdatingButtons();

            button_GotFocus(AcceptButton, null); //Let's mark active button
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

        private void previewTable_AddRowToTable(string[] row)
        {
            previewTable.Rows.Add(row);
            previewTableFormatRow(previewTable.RowCount - 1);

            if (previewTable.RowCount % 16 == 0)
                UpdateCustomScrollBars(previewTable);
        }

        private void previewTable_ProcessRowOfTable(int rowIndex)
        {
            previewTable.Rows[rowIndex].Cells[0].Value = null;
            previewTableFormatRow(rowIndex);
        }

        internal static string ChangeSubstringCase(string substring, ChangeCaseOptions changeCaseOption, bool isTheFirstWord)
        {
            string newSubstring = string.Empty;

            switch (changeCaseOption)
            {
                case ChangeCaseOptions.SentenceCase:
                    if (isTheFirstWord)
                        newSubstring = ChangeSubstringCase(substring, ChangeCaseOptions.TitleCase, isTheFirstWord);
                    else
                        newSubstring = ChangeSubstringCase(substring, ChangeCaseOptions.LowerCase, isTheFirstWord);

                    break;
                case ChangeCaseOptions.TitleCase:
                    bool isTheFirstChar = true;

                    foreach (char currentChar in substring)
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
                    foreach (char currentChar in substring)
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

        internal static bool IsCharCaseSensitive(char item)
        {
            if ((string.Empty + item).ToLower() == (string.Empty + item).ToUpper())
                return false;
            else
                return true;
        }

        internal static bool IsItemContainedInArray(string item, string[] array)
        {
            item = string.Empty + item; //It converts null to string

            foreach (string currentItem in array)
                if (item.ToLower() == currentItem.ToLower()) return true;

            return false;
        }

        internal static bool IsItemContainedInArray(char item, string[] array)
        {
            return IsItemContainedInArray(item.ToString(), array);
        }

        internal static int ItemIndexInArray(string item, string[] array)
        {
            item = string.Empty + item; //It converts null to string

            for (int i = 0; i < array.Length; i++)
                if (item.ToLower() == array[i].ToLower()) return i;

            return -1;
        }

        internal static int ItemIndexInArray(char item, string[] array)
        {
            return ItemIndexInArray(item.ToString(), array);
        }

        internal static string ChangeWordsCase(string source, ChangeCaseOptions changeCaseOption, string[] exceptionWords = null, bool useWhiteList = false, string[] exceptionChars = null,
            string[] leftExceptionChars = null, string[] rightExceptionChars = null, string[] wordSeparators = null, bool alwaysCapitalize1stWord = false, bool alwaysCapitalizeLastWord = false)
        {
            string newString = string.Empty;
            string currentWord = string.Empty;
            char prevChar = '\u0000';
            bool wasCharException = false;
            bool isTheFirstWord = true;
            List<char> encounteredLeftExceptionChars = new List<char>();

            if (exceptionWords == null)
                exceptionWords = new string[0];

            if (exceptionChars == null)
                exceptionChars = new string[0];

            if (leftExceptionChars == null)
                leftExceptionChars = new string[0];

            if (rightExceptionChars == null)
                rightExceptionChars = new string[0];

            if (wordSeparators == null)
                wordSeparators = new string[0];


            foreach (char currentChar in source)
            {
                //Let's try to remove "between chars" changing case exception from encounteredLeftExceptionChars and mark wasCharException
                int rightIndex = ItemIndexInArray(currentChar, rightExceptionChars);
                if (rightIndex >= 0 && rightIndex < encounteredLeftExceptionChars.Count)
                {
                    if (encounteredLeftExceptionChars[rightIndex].ToString() == leftExceptionChars[rightIndex])
                    {
                        wasCharException = true;
                        encounteredLeftExceptionChars.RemoveAt(rightIndex);
                    }
                }

                if (IsItemContainedInArray(currentChar, wordSeparators) || currentChar == ' ' || currentChar == MultipleItemsSplitterId) //Possible end of word
                {
                    if (!IsItemContainedInArray(prevChar, wordSeparators) && prevChar != ' ' && prevChar != MultipleItemsSplitterId) //End of word
                    {
                        if (alwaysCapitalize1stWord && isTheFirstWord) //Always Capitalize 1st word in tag if this option is checked
                        {
                            newString = newString + ChangeSubstringCase(currentWord, ChangeCaseOptions.TitleCase, true) + currentChar;
                        }
                        else if (wasCharException || (IsItemContainedInArray(currentWord, exceptionWords) && !useWhiteList) || (!IsItemContainedInArray(currentWord, exceptionWords) && useWhiteList)
                            || encounteredLeftExceptionChars.Count > 0) //Ignore changing case
                        {
                            newString = newString + currentWord + currentChar;
                        }
                        else //Change case
                        {
                            newString = newString + ChangeSubstringCase(currentWord, changeCaseOption, isTheFirstWord) + currentChar;
                        }


                        currentWord = string.Empty; //Beginning of new word
                        wasCharException = false;
                        isTheFirstWord = false;
                    }
                    else //Not the end of word, several repeating word splitters
                    {
                        newString += currentChar;
                    }
                }
                else //Not the end of word
                {
                    if (currentWord == string.Empty && IsCharCaseSensitive(currentChar)) //Beginning of new word
                    {
                        if (IsItemContainedInArray(currentChar, exceptionChars)) //Ignore changing case later
                            wasCharException = true;

                        if (IsItemContainedInArray(currentChar, leftExceptionChars)) //Ignore changing case later
                            encounteredLeftExceptionChars.Add(currentChar);

                        currentWord += currentChar;
                    }
                    else if (currentWord == string.Empty) //Several repeating symbols between words
                    {
                        if (IsItemContainedInArray(currentChar, exceptionChars)) //Ignore changing case later
                            wasCharException = true;

                        if (IsItemContainedInArray(currentChar, leftExceptionChars)) //Ignore changing case later
                            encounteredLeftExceptionChars.Add(currentChar);

                        newString += currentChar;
                    }
                    else //Letter or symbol in the middle of the word
                    {
                        currentWord += currentChar;
                    }
                }

                prevChar = currentChar;
            }

            //String is ended, so last currentWord IS a word
            if (alwaysCapitalizeLastWord && !wasCharException && encounteredLeftExceptionChars.Count == 0) //Always Capitalize last word if this option is checked, but there was no character exception
                newString += ChangeSubstringCase(currentWord, ChangeCaseOptions.TitleCase, true);
            else if (wasCharException || (IsItemContainedInArray(currentWord, exceptionWords) && !useWhiteList)
                || (!IsItemContainedInArray(currentWord, exceptionWords) && useWhiteList) || encounteredLeftExceptionChars.Count > 0) //Ignore changing case
                newString += currentWord;
            else //Change case
                newString += ChangeSubstringCase(currentWord, changeCaseOption, isTheFirstWord);

            return newString;
        }

        internal static string ChangeSentenceCase(string source, string[] exceptionWords = null, bool useWhiteList = false, string[] exceptionChars = null,
            string[] leftExceptionChars = null, string[] rightExceptionChars = null, string[] sentenceSeparators = null, bool alwaysCapitalize1stWord = false, bool alwaysCapitalizeLastWord = false)
        {
            string newString = string.Empty;
            string currentSentence = string.Empty;
            char prevChar = '\u0000';

            if (exceptionWords == null)
                exceptionWords = new string[0];

            if (exceptionChars == null)
                exceptionChars = new string[0];

            if (leftExceptionChars == null)
                leftExceptionChars = new string[0];

            if (rightExceptionChars == null)
                rightExceptionChars = new string[0];

            if (sentenceSeparators == null)
                sentenceSeparators = new string[0];


            foreach (char currentChar in source)
            {
                if ((prevChar == '.' && currentChar == ' ') || currentChar == MultipleItemsSplitterId || (IsItemContainedInArray(prevChar, sentenceSeparators) && currentChar == ' ')) //Beginning of new sentence
                {
                    newString += ChangeWordsCase(currentSentence, ChangeCaseOptions.SentenceCase, exceptionWords, useWhiteList, exceptionChars,
                        leftExceptionChars, rightExceptionChars, null, alwaysCapitalize1stWord, alwaysCapitalizeLastWord);
                    currentSentence = string.Empty + currentChar;
                }
                else //Not the beginning of new sentence
                {
                    currentSentence += currentChar;
                }

                prevChar = currentChar;
            }

            //String is ended, so last currentSentence IS a sentence
            newString += ChangeWordsCase(currentSentence, ChangeCaseOptions.SentenceCase, exceptionWords, useWhiteList, exceptionChars, leftExceptionChars,
                rightExceptionChars, null, alwaysCapitalize1stWord, alwaysCapitalizeLastWord);

            return newString;
        }

        private string changeCase(string source, int changeCaseOptions, string[] exceptionWords, bool useWhiteList, string[] exceptionChars,
            string[] leftExceptionChars, string[] rightExceptionChars, string[] wordSeparators, bool alwaysCapitalize1stWord, bool alwaysCapitalizeLastWord)
        {
            if (changeCaseOptions == 1) //Splitting to sentences
            {
                return ChangeSentenceCase(source, exceptionWords, useWhiteList, exceptionChars, leftExceptionChars,
                    rightExceptionChars, wordSeparators, alwaysCapitalize1stWord, alwaysCapitalizeLastWord);
            }
            else //Splitting to words
            {
                if (changeCaseOptions == 2)
                    return ChangeWordsCase(source, ChangeCaseOptions.LowerCase, exceptionWords, useWhiteList, exceptionChars,
                        leftExceptionChars, rightExceptionChars, wordSeparators, alwaysCapitalize1stWord, alwaysCapitalizeLastWord);
                else if (changeCaseOptions == 3)
                    return ChangeWordsCase(source, ChangeCaseOptions.UpperCase, exceptionWords, useWhiteList, exceptionChars,
                        leftExceptionChars, rightExceptionChars, wordSeparators, alwaysCapitalize1stWord, alwaysCapitalizeLastWord);
                else if (changeCaseOptions == 4)
                    return ChangeWordsCase(source, ChangeCaseOptions.TitleCase, exceptionWords, useWhiteList, exceptionChars,
                        leftExceptionChars, rightExceptionChars, wordSeparators, alwaysCapitalize1stWord, alwaysCapitalizeLastWord);
                else //if (changeCaseOptions == 5)
                    return ChangeWordsCase(source, ChangeCaseOptions.ToggleCase, exceptionWords, useWhiteList, exceptionChars,
                        leftExceptionChars, rightExceptionChars, wordSeparators, alwaysCapitalize1stWord, alwaysCapitalizeLastWord);
            }
        }

        private bool prepareBackgroundPreview()
        {
            tags.Clear();
            previewTable.RowCount = 0;
            (previewTable.Columns[0].HeaderCell as DataGridViewCheckBoxHeaderCell).setState(true);

            UpdateCustomScrollBars(previewTable);

            if (previewIsGenerated)
            {
                previewIsGenerated = false;
                return true;
            }

            previewIsGenerated = false;

            if (backgroundTaskIsWorking())
                return true;

            changeCaseFlag = getChangeCaseOptionsRadioButtons();

            useWhiteList = onlyWordsCheckBox.Checked;
            exceptionWords = null;
            exceptionChars = null;
            leftExceptionChars = null;
            rightExceptionChars = null;
            wordSeparators = null;
            alwaysCapitalize1stWord = alwaysCapitalize1stWordCheckBox.Checked;
            alwaysCapitalizeLastWord = alwaysCapitalizeLastWordCheckBox.Checked;

            if (exceptionWordsCheckBox.IsEnabled() && exceptionWordsCheckBox.Checked)
                exceptionWords = exceptionWordsBoxCustom.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (onlyWordsCheckBox.IsEnabled() && onlyWordsCheckBox.Checked)
                exceptionWords = exceptionWordsBoxCustom.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (exceptionCharsCheckBox.IsEnabled() && exceptionCharsCheckBox.Checked)
                exceptionChars = exceptionCharsBoxCustom.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (exceptionCharPairsCheckBox.IsEnabled() && exceptionCharPairsCheckBox.Checked)
            {
                leftExceptionChars = leftExceptionCharsBoxCustom.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                rightExceptionChars = rightExceptionCharsBoxCustom.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }

            if (wordSeparatorsCheckBox.IsEnabled() && wordSeparatorsCheckBox.Checked)
                wordSeparators = wordSeparatorsBoxCustom.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            sourceTagId = GetTagId(sourceTagListCustom.Text);

            files = null;
            if (!MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files))
                files = new string[0];

            if (files.Length == 0)
            {
                MessageBox.Show(this, MsgNoTracksSelected, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool prepareBackgroundTask()
        {
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
                    tag = new string[3];

                    tag[0] = (string)previewTable.Rows[fileCounter].Cells[0].Value;
                    tag[1] = (string)previewTable.Rows[fileCounter].Cells[1].Value;
                    tag[2] = (string)previewTable.Rows[fileCounter].Cells[5].Value;

                    tags.Add(tag);
                }

                return true;
            }
        }

        private void previewChanges()
        {
            string currentFile;
            string sourceTagValue;
            string sourceTagTValue;
            string newTagValue;
            string newTagTValue;

            string isChecked;

            bool stripNotChangedLines = SavedSettings.dontIncludeInPreviewLinesWithoutChangedTags;

            string track;
            string[] row = { "Checked", "File", "Track", "OriginalTag", "OriginalTagT", "NewTag", "NewTagT" };
            string[] tag = { "Checked", "file", "newTag" };

            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                currentFile = files[fileCounter];

                SetStatusbarTextForFileOperations(ChangeCaseSbText, true, fileCounter, files.Length, currentFile);

                sourceTagValue = GetFileTag(currentFile, sourceTagId);
                sourceTagTValue = GetTagRepresentation(sourceTagValue);
                newTagValue = changeCase(sourceTagValue, changeCaseFlag, exceptionWords, useWhiteList, exceptionChars,
                    leftExceptionChars, rightExceptionChars, wordSeparators, alwaysCapitalize1stWord, alwaysCapitalizeLastWord);
                newTagTValue = GetTagRepresentation(newTagValue);

                track = GetTrackRepresentation(currentFile);


                if (sourceTagValue == newTagValue && stripNotChangedLines)
                    continue;
                else if (sourceTagValue == newTagValue)
                    isChecked = "F";
                else
                    isChecked = "T";

                tag = new string[3];

                tag[0] = isChecked;
                tag[1] = currentFile;
                tag[2] = newTagValue;


                row = new string[7];

                row[0] = isChecked;
                row[1] = currentFile;
                row[2] = track;
                row[3] = sourceTagValue;
                row[4] = sourceTagTValue;
                row[5] = newTagValue;
                row[6] = newTagTValue;

                Invoke(addRowToTable, new object[] { row });

                tags.Add(tag);

                previewIsGenerated = true;
            }

            Invoke(updateCustomScrollBars, previewTable);
            SetResultingSbText();
        }

        private void reapplyRules()
        {
            string newTagValue;
            string newTagTValue;

            if (previewTable.Rows.Count == 0)
            {
                MessageBox.Show(this, MsgPreviewIsNotGeneratedNothingToChange, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            changeCaseFlag = getChangeCaseOptionsRadioButtons();

            useWhiteList = onlyWordsCheckBox.Checked;
            exceptionWords = null;
            exceptionChars = null;
            leftExceptionChars = null;
            rightExceptionChars = null;
            wordSeparators = null;

            if (exceptionWordsCheckBox.IsEnabled() && exceptionWordsCheckBox.Checked)
                exceptionWords = exceptionWordsBoxCustom.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            else if (exceptionWordsCheckBox.IsEnabled() && onlyWordsCheckBox.Checked)
                exceptionWords = exceptionWordsBoxCustom.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (exceptionCharsCheckBox.IsEnabled() && exceptionCharsCheckBox.Checked)
                exceptionChars = exceptionCharsBoxCustom.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (exceptionCharPairsCheckBox.IsEnabled() && exceptionCharPairsCheckBox.Checked)
            {
                leftExceptionChars = leftExceptionCharsBoxCustom.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                rightExceptionChars = rightExceptionCharsBoxCustom.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }

            if (wordSeparatorsCheckBox.IsEnabled() && wordSeparatorsCheckBox.Checked)
                wordSeparators = wordSeparatorsBoxCustom.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < previewTable.Rows.Count; i++)
            {
                if ((string)previewTable.Rows[i].Cells[0].Value == "T")
                {
                    newTagValue = changeCase((string)previewTable.Rows[i].Cells[5].Value, changeCaseFlag, exceptionWords, useWhiteList, exceptionChars,
                        leftExceptionChars, rightExceptionChars, wordSeparators, alwaysCapitalize1stWord, alwaysCapitalizeLastWord);
                    newTagTValue = GetTagRepresentation(newTagValue);

                    previewTable.Rows[i].Cells[5].Value = newTagValue;
                    previewTable.Rows[i].Cells[6].Value = newTagTValue;
                }
            }
        }

        private void applyChanges()
        {
            string currentFile;
            string newTagValue;
            string isChecked;

            if (tags.Count == 0)
                previewChanges();

            for (int i = 0; i < tags.Count; i++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                isChecked = tags[i][0];

                if (isChecked == "T")
                {
                    currentFile = tags[i][1];
                    newTagValue = tags[i][2];

                    tags[i][0] = string.Empty;

                    Invoke(processRowOfTable, new object[] { i });

                    SetStatusbarTextForFileOperations(ChangeCaseSbText, false, i, tags.Count, currentFile);

                    SetFileTag(currentFile, sourceTagId, newTagValue);
                    CommitTagsToFile(currentFile);
                }
            }

            RefreshPanels(true);

            SetResultingSbText();
        }

        private void saveSettings()
        {
            saveWindowLayout(previewTable.Columns[1].Width, previewTable.Columns[2].Width, previewTable.Columns[3].Width);

            SavedSettings.changeCaseSourceTagName = sourceTagListCustom.Text;
            SavedSettings.changeCaseFlag = getChangeCaseOptionsRadioButtons();

            SavedSettings.useExceptionWords = exceptionWordsCheckBox.Checked;
            SavedSettings.useOnlyWords = onlyWordsCheckBox.Checked;
            exceptionWordsBoxCustom.Items.CopyTo(SavedSettings.exceptionWords, 0);

            SavedSettings.useExceptionChars = exceptionCharsCheckBox.Checked;
            exceptionCharsBoxCustom.Items.CopyTo(SavedSettings.exceptionChars, 0);

            SavedSettings.useExceptionCharPairs = exceptionCharPairsCheckBox.Checked;
            leftExceptionCharsBoxCustom.Items.CopyTo(SavedSettings.leftExceptionChars, 0);
            rightExceptionCharsBoxCustom.Items.CopyTo(SavedSettings.rightExceptionChars, 0);

            SavedSettings.useWordSeparators = wordSeparatorsCheckBox.Checked;
            wordSeparatorsBoxCustom.Items.CopyTo(SavedSettings.wordSeparators, 0);

            SavedSettings.alwaysCapitalize1stWord = alwaysCapitalize1stWordCheckBox.Checked;
            SavedSettings.alwaysCapitalizeLastWord = alwaysCapitalizeLastWordCheckBox.Checked;

            TagToolsPlugin.SaveSettings();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            saveSettings();
            if (prepareBackgroundTask())
                switchOperation(applyChanges, (Button)sender, buttonOK, buttonPreview, buttonCancel, true, null);
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewChanges, (Button)sender, buttonOK, buttonCancel);
            enableQueryingOrUpdatingButtons();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            exceptionWordsBoxCustom.Items.CopyTo(SavedSettings.exceptionWords, 0);
            Close();
        }

        private void buttonReapply_Click(object sender, EventArgs e)
        {
            reapplyRules();
        }

        private void exceptWordsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            exceptionWordsBoxCustom.Enable(exceptionWordsCheckBox.Checked || onlyWordsCheckBox.Checked);
            removeExceptionButton.Enable(exceptionWordsCheckBox.Checked || onlyWordsCheckBox.Checked);

            if (exceptionWordsCheckBox.Checked)
                onlyWordsCheckBox.Checked = false;
        }

        private void exceptionWordsCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!exceptionWordsCheckBox.IsEnabled())
                return;

            exceptionWordsCheckBox.Checked = !exceptionWordsCheckBox.Checked;
        }

        private void onlyWordsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            exceptionWordsBoxCustom.Enable(exceptionWordsCheckBox.Checked || onlyWordsCheckBox.Checked);
            removeExceptionButton.Enable(exceptionWordsCheckBox.Checked || onlyWordsCheckBox.Checked);

            if (onlyWordsCheckBox.Checked)
                exceptionWordsCheckBox.Checked = false;
        }

        private void onlyWordsCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!onlyWordsCheckBox.IsEnabled())
                return;

            onlyWordsCheckBox.Checked = !onlyWordsCheckBox.Checked;
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
            leftExceptionCharsBoxCustom.Enable(exceptionCharPairsCheckBox.Checked);
            rightExceptionCharsBoxCustom.Enable(exceptionCharPairsCheckBox.Checked);
            buttonAsrExceptWordsBetweenSymbols.Enable(exceptionCharPairsCheckBox.Checked);
        }

        private void exceptionCharPairsCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!exceptionCharPairsCheckBox.IsEnabled())
                return;

            exceptionCharPairsCheckBox.Checked = !exceptionCharPairsCheckBox.Checked;
        }

        private void wordSeparatorsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            wordSeparatorsBoxCustom.Enable(wordSeparatorsCheckBox.Checked);
            buttonAsrWordSeparators.Enable(wordSeparatorsCheckBox.Checked);
        }

        private void wordSeparatorsCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!wordSeparatorsCheckBox.IsEnabled())
                return;

            wordSeparatorsCheckBox.Checked = !wordSeparatorsCheckBox.Checked;
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

        private void previewTableFormatRow(int rowIndex)
        {
            if (SavedSettings.dontHighlightChangedTags)
                return;

            if ((string)previewTable.Rows[rowIndex].Cells[0].Value != "T")
            {
                for (int columnIndex = 1; columnIndex < previewTable.ColumnCount; columnIndex++)
                {
                    if (previewTable.Columns[columnIndex].Visible)
                        previewTable.Rows[rowIndex].Cells[columnIndex].Style = dimmedCellStyle;
                }

                return;
            }


            for (int columnIndex = 1; columnIndex < previewTable.ColumnCount; columnIndex++)
            {
                if (columnIndex == 6)
                {
                    if ((string)previewTable.Rows[rowIndex].Cells[3].Value == (string)previewTable.Rows[rowIndex].Cells[5].Value)
                        previewTable.Rows[rowIndex].Cells[6].Style = unchangedCellStyle;
                    else
                        previewTable.Rows[rowIndex].Cells[6].Style = changedCellStyle;
                }
                else
                {
                    previewTable.Rows[rowIndex].Cells[columnIndex].Style = unchangedCellStyle;
                }
            }

        }

        private void previewTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string sourceTagValue;
                string sourceTagTValue;
                string newTagValue;
                string newTagTValue;

                string isChecked = (string)previewTable.Rows[e.RowIndex].Cells[0].Value;

                if (isChecked == "T")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "F";

                    sourceTagValue = (string)previewTable.Rows[e.RowIndex].Cells[3].Value;
                    sourceTagTValue = (string)previewTable.Rows[e.RowIndex].Cells[4].Value;

                    previewTable.Rows[e.RowIndex].Cells[5].Value = sourceTagValue;
                    previewTable.Rows[e.RowIndex].Cells[6].Value = sourceTagTValue;
                }
                else if (isChecked == "F")
                {
                    changeCaseFlag = getChangeCaseOptionsRadioButtons();

                    useWhiteList = onlyWordsCheckBox.Checked;
                    exceptionWords = null;
                    exceptionChars = null;
                    leftExceptionChars = null;
                    rightExceptionChars = null;
                    wordSeparators = null;

                    if (exceptionWordsCheckBox.IsEnabled() && exceptionWordsCheckBox.Checked)
                        exceptionWords = exceptionWordsBoxCustom.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    else if (onlyWordsCheckBox.IsEnabled() && onlyWordsCheckBox.Checked)
                        exceptionWords = exceptionWordsBoxCustom.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (exceptionCharsCheckBox.IsEnabled() && exceptionCharsCheckBox.Checked)
                        exceptionChars = exceptionCharsBoxCustom.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (exceptionCharPairsCheckBox.IsEnabled() && exceptionCharPairsCheckBox.Checked)
                    {
                        leftExceptionChars = leftExceptionCharsBoxCustom.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        rightExceptionChars = rightExceptionCharsBoxCustom.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    }

                    if (wordSeparatorsCheckBox.IsEnabled() && wordSeparatorsCheckBox.Checked)
                        wordSeparators = wordSeparatorsBoxCustom.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    previewTable.Rows[e.RowIndex].Cells[0].Value = "T";

                    sourceTagValue = (string)previewTable.Rows[e.RowIndex].Cells[3].Value;

                    newTagValue = changeCase(sourceTagValue, changeCaseFlag, exceptionWords, useWhiteList, exceptionChars,
                        leftExceptionChars, rightExceptionChars, wordSeparators, alwaysCapitalize1stWord, alwaysCapitalizeLastWord);
                    newTagTValue = GetTagRepresentation(newTagValue);

                    previewTable.Rows[e.RowIndex].Cells[5].Value = newTagValue;
                    previewTable.Rows[e.RowIndex].Cells[6].Value = newTagTValue;
                }

                previewTableFormatRow(e.RowIndex);
            }
        }

        internal override void enableDisablePreviewOptionControls(bool enable, bool dontChangeDisabled = false)
        {
            if (enable && previewIsGenerated)
                return;

            sourceTagListCustom.Enable(enable);
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
            buttonOK.Enable(previewIsGenerated || SavedSettings.allowCommandExecutionWithoutPreview);
            buttonPreview.Enable(true);
            buttonReapply.Enable(true);
        }

        internal override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enable(false);
            buttonPreview.Enable(false);
            buttonReapply.Enable(false);
        }

        private void exceptionWordsBox_Leave(object sender, EventArgs e)
        {
            exceptionWordsBoxCustom.Text = Regex.Replace(exceptionWordsBoxCustom.Text.Trim(' '), @"\s{2,}", " ");
            CustomComboBoxLeave(exceptionWordsBoxCustom);
        }

        private void exceptionCharsBox_Leave(object sender, EventArgs e)
        {
            exceptionCharsBoxCustom.Text = Regex.Replace(exceptionCharsBoxCustom.Text.Trim(' '), @"\s{2,}", " ");
            CustomComboBoxLeave(exceptionCharsBoxCustom);
        }

        private void wordSeparatorsBox_Leave(object sender, EventArgs e)
        {
            wordSeparatorsBoxCustom.Text = Regex.Replace(wordSeparatorsBoxCustom.Text.Trim(' '), @"\s{2,}", " ");
            CustomComboBoxLeave(wordSeparatorsBoxCustom);
        }

        internal static string[] GetCharsInString(string text)
        {
            if (text == null)
                return null;

            //Let's count the number of chars except for spaces
            int count = 0;
            foreach (char ch in text)
                if (ch != ' ')
                    count++;

            string[] chars = new string[count];
            count = 0;
            foreach (char ch in text)
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

            string[] leftItems = GetCharsInString(text1);
            string[] rightItems = GetCharsInString(text2);

            if (leftItems.Length != rightItems.Length)
                return false;

            return true;
        }

        private void leftExceptionCharsBox_Leave(object sender, EventArgs e)
        {
            leftExceptionCharsBoxCustom.Text = Regex.Replace(leftExceptionCharsBoxCustom.Text.Trim(' '), @"\s{2,}", " ");

            rightExceptionCharsBoxCustom.Focus();
            CustomComboBoxLeave(leftExceptionCharsBoxCustom);
        }

        private void rightExceptionCharsBox_Leave(object sender, EventArgs e)
        {
            rightExceptionCharsBoxCustom.Text = Regex.Replace(rightExceptionCharsBoxCustom.Text.Trim(' '), @"\s{2,}", " ");

            if (!CheckIfTheSameNumberOfCharsInStrings(leftExceptionCharsBoxCustom.Text, rightExceptionCharsBoxCustom.Text))
            {
                MessageBox.Show(MbForm, MsgTheNumberOfOpeningExceptionCharactersMustBe,
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);

                rightExceptionCharsBoxCustom.Focus();
            }

            CustomComboBoxLeave(rightExceptionCharsBoxCustom);
        }

        private void casingRuleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sentenceCaseRadioButton.Checked)
                wordSeparatorsCheckBoxLabel.Text = wordSeparatorsSentenceLabel;
            else
                wordSeparatorsCheckBoxLabel.Text = wordSeparatorsLabel;
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
            for (int i = 0; i < exceptionWordsBoxCustom.Items.Count; i++)
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
            SavedSettings.leftExceptionCharsAsr = leftExceptionCharsBoxCustom.Text;
            SavedSettings.rightExceptionCharsAsr = rightExceptionCharsBoxCustom.Text;
        }

        private void buttonAsrWordSeparators_Click(object sender, EventArgs e)
        {
            SavedSettings.wordSeparatorsAsr = wordSeparatorsBoxCustom.Text;
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            QuickSettings settings = new QuickSettings(TagToolsPlugin);
            Display(settings, true);
        }

        private void alwaysCapitalize1stWordCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!alwaysCapitalize1stWordCheckBox.IsEnabled())
                return;

            alwaysCapitalize1stWordCheckBox.Checked = !alwaysCapitalize1stWordCheckBox.Checked;
        }

        private void alwaysCapitalizeLastWordCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!alwaysCapitalizeLastWordCheckBox.IsEnabled())
                return;

            alwaysCapitalizeLastWordCheckBox.Checked = !alwaysCapitalizeLastWordCheckBox.Checked;
        }

        private void ChangeCase_Load(object sender, EventArgs e)
        {
            (int, int, int, int, int, int, int) value = loadWindowLayout();

            if (value.Item1 != 0)
            {
                previewTable.Columns[2].Width = (int)Math.Round(value.Item1 * hDpiFontScaling);
                previewTable.Columns[4].Width = (int)Math.Round(value.Item2 * hDpiFontScaling);
                previewTable.Columns[6].Width = (int)Math.Round(value.Item3 * hDpiFontScaling);
            }
            else
            {
                previewTable.Columns[2].Width = (int)Math.Round(previewTable.Columns[2].Width * hDpiFontScaling);
                previewTable.Columns[4].Width = (int)Math.Round(previewTable.Columns[4].Width * hDpiFontScaling);
                previewTable.Columns[6].Width = (int)Math.Round(previewTable.Columns[6].Width * hDpiFontScaling);
            }

        }

        private void ChangeCase_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveWindowLayout(previewTable.Columns[2].Width, previewTable.Columns[4].Width, previewTable.Columns[6].Width);
        }
    }
}
