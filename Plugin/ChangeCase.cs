using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using ExtensionMethods;

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
        private CustomComboBox openingExceptionCharsBoxCustom;
        private CustomComboBox closingExceptionCharsBoxCustom;
        private CustomComboBox sentenceSeparatorsBoxCustom;

        private bool closingExceptionCharsBoxLeaving;

        private readonly DataGridViewCellStyle unchangedCellStyle = new DataGridViewCellStyle(UnchangedCellStyle);
        private readonly DataGridViewCellStyle changedCellStyle = new DataGridViewCellStyle(ChangedCellStyle);
        private readonly DataGridViewCellStyle dimmedCellStyle = new DataGridViewCellStyle(DimmedCellStyle);

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

        private string changeOnlyWordsText;
        private string exceptForWordsText;

        internal ChangeCase(Plugin plugin) : base(plugin)
        {
            InitializeComponent();
        }

        protected override void initializeForm()
        {
            base.initializeForm();

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


            buttonLabels[removeExceptionButton] = string.Empty;
            removeExceptionButton.Text = string.Empty;
            removeExceptionButton.Image = ReplaceBitmap(removeExceptionButton.Image, ClearField);

            buttonSettings.Image = ReplaceBitmap(buttonSettings.Image, Gear);


            changeOnlyWordsText = toolTip1.GetToolTip(exceptionWordsCheckBoxLabel);
            exceptForWordsText = exceptionWordsCheckBoxLabel.Text;
            toolTip1.SetToolTip(exceptionWordsCheckBoxLabel, toolTip1.GetToolTip(exceptionWordsCheckBox));


            FillListByTagNames(sourceTagListCustom.Items);
            sourceTagListCustom.Text = SavedSettings.changeCaseSourceTagName;

            setChangeCaseOptionsRadioButtons(SavedSettings.changeCaseFlag);

            exceptionWordsCheckBox.CheckState = GetCheckState(SavedSettings.useExceptionWords);
            exceptionWordsBoxCustom.AddRange(SavedSettings.exceptedWords);
            exceptionWordsBoxCustom.Text = SavedSettings.exceptedWords[0] as string;

            exceptionCharsCheckBox.Checked = SavedSettings.useExceptionChars;
            exceptionCharsBoxCustom.AddRange(SavedSettings.exceptionChars);
            exceptionCharsBoxCustom.Text = SavedSettings.exceptionChars[0] as string;

            exceptionCharPairsCheckBox.Checked = SavedSettings.useExceptionCharPairs;
            openingExceptionCharsBoxCustom.AddRange(SavedSettings.openingExceptionChars);
            openingExceptionCharsBoxCustom.Text = SavedSettings.openingExceptionChars[0] as string;
            closingExceptionCharsBoxCustom.AddRange(SavedSettings.closingExceptionChars);
            closingExceptionCharsBoxCustom.Text = SavedSettings.closingExceptionChars[0] as string;

            sentenceSeparatorsCheckBox.Checked = SavedSettings.useSentenceSeparators;
            sentenceSeparatorsBoxCustom.AddRange(SavedSettings.sentenceSeparators);
            sentenceSeparatorsBoxCustom.Text = SavedSettings.sentenceSeparators[0] as string;

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
            cbHeader.OnCheckBoxClicked += cbHeader_OnCheckBoxClicked;

            var colCB = new DataGridViewCheckBoxColumn
            {
                HeaderCell = cbHeader,
                ThreeState = true,
                FalseValue = "F",
                TrueValue = "T",
                IndeterminateValue = string.Empty,
                Width = 25,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };

            previewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            previewTable.Columns.Insert(0, colCB);
            previewTable.Columns[2].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[4].HeaderCell.Style = headerCellStyle;
            previewTable.Columns[6].HeaderCell.Style = headerCellStyle;

            previewTable.Columns[2].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[4].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[6].HeaderCell.Style.WrapMode = DataGridViewTriState.True;

            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;


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
            if (previewIsGenerated)
            {
                previewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                previewTable.AllowUserToResizeColumns = true;
                previewTable.AllowUserToResizeRows = true;
                foreach (DataGridViewColumn column in previewTable.Columns)
                    column.SortMode = DataGridViewColumnSortMode.Automatic;
            }
            else
            {
                previewTable.AllowUserToResizeColumns = false;
                previewTable.AllowUserToResizeRows = false;
                foreach (DataGridViewColumn column in previewTable.Columns)
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }


            backgroundTaskIsStopping = false;
            backgroundTaskIsStoppedOrCancelled = false;

            tags.Clear();
            previewTable.RowCount = 0;
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
            previewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            previewTable.AllowUserToResizeColumns = true;
            previewTable.AllowUserToResizeRows = true;
            foreach (DataGridViewColumn column in previewTable.Columns)
                column.SortMode = DataGridViewColumnSortMode.Automatic;


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
            }

            ignoreClosingForm = false;
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

        internal static string ChangeWordsCase(string source, ChangeCaseOptions changeCaseOption, ref List<char> encounteredLeftExceptionChars, ref bool wasCharException, string[] exceptedWords = null, bool useWhiteList = false,
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
            resetPreviewData();

            if (backgroundTaskIsStopping)
                backgroundTaskIsStoppedOrCancelled = true;

            if (previewIsGenerated)
            {
                previewIsGenerated = false;
                enableDisablePreviewOptionControls(true);
                return true;
            }

            previewIsGenerated = false;

            if (backgroundTaskIsWorking())
                return true;



            previewTable.AllowUserToResizeColumns = false;
            previewTable.AllowUserToResizeRows = false;
            foreach (DataGridViewColumn column in previewTable.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;


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
                previewTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
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
                tags.Clear();

                for (var fileCounter = 0; fileCounter < previewTable.Rows.Count; fileCounter++)
                {
                    string[] tag = { "Checked", "File", "NewTag" };

                    tag[0] = previewTable.Rows[fileCounter].Cells[0].Value as string;
                    tag[1] = previewTable.Rows[fileCounter].Cells[1].Value as string;
                    tag[2] = previewTable.Rows[fileCounter].Cells[5].Value as string;

                    tags.Add(tag);
                }

                ignoreClosingForm = true;

                return true;
            }
        }

        private void previewChanges()
        {
            previewIsGenerated = true;

            List<string[]> rows = new List<string[]>();
            var stripNotChangedLines = SavedSettings.dontIncludeInPreviewLinesWithoutChangedTags;

            for (var fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (checkStoppingStatus())
                {
                    Invoke(new Action(() => { stopButtonClickedMethod(prepareBackgroundPreview); }));
                    return;
                }

                // ReSharper disable once RedundantAssignment
                string[] tag = { "Checked", "File", "NewTag" };
                // ReSharper disable once RedundantAssignment
                string[] row = { "Checked", "File", "Track", "OriginalTag", "OriginalTagT", "NewTag", "NewTagT" };

                var currentFile = files[fileCounter];

                SetStatusBarTextForFileOperations(ChangeCaseSbText, true, fileCounter, files.Length, currentFile);

                var sourceTagValue = GetFileTag(currentFile, sourceTagId);
                var sourceTagTValue = GetTagRepresentation(sourceTagValue);
                var newTagValue = changeCase(sourceTagValue, changeCaseFlag, exceptedWords, useWhiteList, exceptionChars,
                    openingExceptionChars, closingExceptionChars, sentenceSeparators, alwaysCapitalize1stWord, alwaysCapitalizeLastWord, ignoreSingleLetterExceptedWords);
                var newTagTValue = GetTagRepresentation(newTagValue);

                var track = GetTrackRepresentation(currentFile);


                string isChecked;
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


                rows.Add(row);

                int rowCountToFormat1 = 0;
                Invoke(new Action(() => { rowCountToFormat1 = AddRowsToTable(this, previewTable, rows, false, true); }));
                Invoke(new Action(() => { FormatChangedTags(this, previewTable, rowCountToFormat1, previewTableFormatRow); }));
            }

            int rowCountToFormat2 = 0;
            Invoke(new Action(() => { rowCountToFormat2 = AddRowsToTable(this, previewTable, rows, true, true); }));
            Invoke(new Action(() => { FormatChangedTags(this, previewTable, rowCountToFormat2, previewTableFormatRow); checkStoppedStatus(); resetFormToGeneratedPreview(); }));
        }

        private void reapplyRules()
        {
            if (previewTable.Rows.Count == 0)
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

            for (var i = 0; i < previewTable.Rows.Count; i++)
            {
                if (previewTable.Rows[i].Cells[0].Value as string == "T")
                {
                    var newTagValue = changeCase(previewTable.Rows[i].Cells[5].Value as string, changeCaseFlag, exceptedWords, useWhiteList, exceptionChars,
                        openingExceptionChars, closingExceptionChars, sentenceSeparators, alwaysCapitalize1stWord, alwaysCapitalizeLastWord, ignoreSingleLetterExceptedWords);
                    var newTagTValue = GetTagRepresentation(newTagValue);

                    previewTable.Rows[i].Cells[5].Value = newTagValue;
                    previewTable.Rows[i].Cells[6].Value = newTagTValue;
                }
            }
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

                if (isChecked == "T")
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
            exceptionWordsBoxCustom.Items.CopyTo(SavedSettings.exceptedWords, 0);

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
            ignoreClosingForm = clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewChanges, buttonPreview, buttonOK, buttonClose);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonReapply_Click(object sender, EventArgs e)
        {
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

        private void previewTableFormatRow(DataGridView dataGridView, int rowIndex)
        {
            if (SavedSettings.dontHighlightChangedTags)
                return;

            if (dataGridView.Rows[rowIndex].Cells[0].Value as string != "T")
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

                if (isChecked == "T")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "F";

                    sourceTagValue = previewTable.Rows[e.RowIndex].Cells[3].Value as string;
                    var sourceTagTValue = previewTable.Rows[e.RowIndex].Cells[4].Value as string;

                    previewTable.Rows[e.RowIndex].Cells[5].Value = sourceTagValue;
                    previewTable.Rows[e.RowIndex].Cells[6].Value = sourceTagTValue;
                }
                else if (isChecked == "F")
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

                    previewTable.Rows[e.RowIndex].Cells[0].Value = "T";

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
            buttonOK.Enable((previewIsGenerated || SavedSettings.allowCommandExecutionWithoutPreview) && !backgroundTaskIsStopping && (!backgroundTaskIsWorking() || backgroundTaskIsUpdatingTags));
            buttonPreview.Enable(true);
            buttonReapply.Enable(previewIsGenerated && !backgroundTaskIsWorking());
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

        private void sentenceSeparatorsBox_Leave(object sender, EventArgs e)
        {
            sentenceSeparatorsBoxCustom.Text = Regex.Replace(sentenceSeparatorsBoxCustom.Text.Trim(' '), @"\s{2,}", " ");
            CustomComboBoxLeave(sentenceSeparatorsBoxCustom);
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

        private void openingExceptionCharsBox_Leave(object sender, EventArgs e)
        {
            openingExceptionCharsBoxCustom.Text = Regex.Replace(openingExceptionCharsBoxCustom.Text.Trim(' '), @"\s{2,}", " ");

            if (ActiveControl != closingExceptionCharsBoxCustom && !CheckIfTheSameNumberOfCharsInStrings(openingExceptionCharsBoxCustom.Text, closingExceptionCharsBoxCustom.Text))
            {
                closingExceptionCharsBoxCustom.Focus();
                closingExceptionCharsBoxCustom.SelectionStart = closingExceptionCharsBoxCustom.Text.Length + 1;
                closingExceptionCharsBoxCustom.SelectionLength = 0;
            }

            CustomComboBoxLeave(openingExceptionCharsBoxCustom);
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

            CustomComboBoxLeave(closingExceptionCharsBoxCustom);
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
                }

                backgroundTaskIsStopping = true;
                SetStatusBarText(ChangeCaseSbText + SbTextStoppingCurrentOperation, false);

                e.Cancel = true;
            }
            else
            {
                saveWindowLayout(previewTable.Columns[2].Width, previewTable.Columns[4].Width, previewTable.Columns[6].Width);
            }
        }
    }
}
