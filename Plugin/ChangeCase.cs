using ExtensionMethods;
using System;
using System.Collections.Generic;
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


        private DataGridViewCellStyle unchangedCellStyle = new DataGridViewCellStyle(UnchangedCellStyle);
        private DataGridViewCellStyle changedCellStyle = new DataGridViewCellStyle(ChangedCellStyle);
        private DataGridViewCellStyle dimmedCellStyle = new DataGridViewCellStyle(DimmedCellStyle);

        private delegate void AddRowToTable(string[] row);
        private delegate void ProcessRowOfTable(int row);
        private AddRowToTable addRowToTable;
        private ProcessRowOfTable processRowOfTable;

        private delegate void UpdateCustomScrollBarsDelegate(DataGridView dataGridView);
        private UpdateCustomScrollBarsDelegate updateCustomScrollBars;

        private string[] files = new string[0];
        private List<string[]> tags = new List<string[]>();
        private MetaDataType sourceTagId;

        private int changeCaseFlag;
        private bool useWhiteList;
        private string[] exceptionWords;
        private string[] exceptionChars;
        private string[] wordSplitters;
        private bool alwaysCapitalize1stWord;
        private bool alwaysCapitalizeLastWord;

        internal ChangeCase(Plugin tagToolsPluginParam) : base(tagToolsPluginParam)
        {
            InitializeComponent();
        }

        protected override void initializeForm()
        {
            base.initializeForm();

            sourceTagListCustom = namesComboBoxes["sourceTagList"];
            exceptionWordsBoxCustom = namesComboBoxes["exceptionWordsBox"];


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
            exceptionCharsBox.Text = SavedSettings.exceptionChars;
            wordSplittersCheckBox.Checked = SavedSettings.useWordSplitters;
            wordSplittersBox.Text = SavedSettings.wordSplitters;
            alwaysCapitalize1stWordCheckBox.Checked = SavedSettings.alwaysCapitalize1stWord;
            alwaysCapitalizeLastWordCheckBox.Checked = SavedSettings.alwaysCapitalizeLastWord;

            exceptWordsCheckBox_CheckedChanged(null, null);
            exceptCharsCheckBox_CheckedChanged(null, null);
            wordSplittersCheckBox_CheckedChanged(null, null);
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
                            newSubstring = newSubstring + (string.Empty + currentChar).ToUpper();
                            isTheFirstChar = false;
                        }
                        else
                            newSubstring = newSubstring + (string.Empty + currentChar).ToLower();
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
                            newSubstring = newSubstring + (string.Empty + currentChar).ToLower();
                        else //Char is lowercased
                            newSubstring = newSubstring + (string.Empty + currentChar).ToUpper();
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

        internal static bool IsItemContainedInList(string item, string[] list)
        {
            item = string.Empty + item; //It converts null to string

            foreach (string currentItem in list)
                if (item.ToLower() == currentItem.ToLower()) return true;

            return false;
        }

        internal static bool IsItemContainedInList(char item, string[] list)
        {
            return IsItemContainedInList(string.Empty + item, list);
        }

        internal static string ChangeWordsCase(string source, ChangeCaseOptions changeCaseOption, string[] exceptionWords = null, bool useWhiteList = false, string[] exceptionChars = null, string[] wordSplitters = null, bool alwaysCapitalize1stWord = false, bool alwaysCapitalizeLastWord = false)
        {
            string newString = string.Empty;
            string currentWord = string.Empty;
            char prevChar = '\0';
            bool wasCharException = false;
            bool isTheFirstWord = true;

            if (exceptionWords == null)
                exceptionWords = new string[0];

            if (exceptionChars == null)
                exceptionChars = new string[0];

            if (wordSplitters == null)
                wordSplitters = new string[0];

            foreach (char currentChar in source)
            {
                if (IsItemContainedInList(currentChar, wordSplitters) || currentChar == ' ' || currentChar == MultipleItemsSplitterId) //Possible end of word
                {
                    if (!IsItemContainedInList(prevChar, wordSplitters) && prevChar != ' ' && prevChar != MultipleItemsSplitterId) //End of word
                    {
                        if (alwaysCapitalize1stWord && isTheFirstWord) //Always Capitalize 1st word in tag if this option is checked
                        {
                            newString = newString + ChangeSubstringCase(currentWord, ChangeCaseOptions.TitleCase, true) + currentChar;
                        }
                        else if (wasCharException || (IsItemContainedInList(currentWord, exceptionWords) && !useWhiteList) || (!IsItemContainedInList(currentWord, exceptionWords) && useWhiteList)) //Ignore changing case
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
                        newString = newString + currentChar;
                    }
                }
                else //Not the end of word
                {
                    if (currentWord == string.Empty && IsCharCaseSensitive(currentChar)) //Beginning of new word
                    {
                        if (IsItemContainedInList(currentChar, exceptionChars)) //Ignore changing case later
                            wasCharException = true;

                        currentWord = currentWord + currentChar;
                    }
                    else if (currentWord == string.Empty) //Several repeating symbols between words
                    {
                        if (IsItemContainedInList(currentChar, exceptionChars)) //Ignore changing case later
                            wasCharException = true;

                        newString = newString + currentChar;
                    }
                    else //Letter or symbol in the middle of the word
                    {
                        currentWord = currentWord + currentChar;
                    }
                }

                prevChar = currentChar;
            }

            //String is ended, so last currentWord IS a word
            if (alwaysCapitalizeLastWord && !wasCharException) //Always Capitalize last word if this option is checked, but there was no character exception
                newString = newString + ChangeSubstringCase(currentWord, ChangeCaseOptions.TitleCase, true);
            else if (wasCharException || (IsItemContainedInList(currentWord, exceptionWords) && !useWhiteList) || (!IsItemContainedInList(currentWord, exceptionWords) && useWhiteList)) //Ignore changing case
                newString = newString + currentWord;
            else //Change case
                newString = newString + ChangeSubstringCase(currentWord, changeCaseOption, isTheFirstWord);

            return newString;
        }

        internal static string ChangeSentenceCase(string source, string[] exceptionWords = null, bool useWhiteList = false, string[] exceptionChars = null, string[] wordSplitters = null)
        {
            string newString = string.Empty;
            string currentSentence = string.Empty;
            char prevChar = '\0';

            foreach (char currentChar in source)
            {
                if ((prevChar == '.' && currentChar == ' ') || currentChar == MultipleItemsSplitterId) //Beginning of new sentence
                {
                    newString = newString + ChangeWordsCase(currentSentence, ChangeCaseOptions.SentenceCase, exceptionWords, useWhiteList, exceptionChars, wordSplitters);
                    currentSentence = string.Empty + currentChar;
                }
                else //Not the beginning of new sentence
                {
                    currentSentence = currentSentence + currentChar;
                }

                prevChar = currentChar;
            }

            //String is ended, so last currentSentence IS a sentence
            newString = newString + ChangeWordsCase(currentSentence, ChangeCaseOptions.SentenceCase, exceptionWords, useWhiteList, exceptionChars, wordSplitters);

            return newString;
        }

        private string changeCase(string source, int changeCaseOptions, string[] exceptionWords = null, bool useWhiteList = false, string[] exceptionChars = null, string[] wordSplitters = null, bool alwaysCapitalize1stWord = false, bool alwaysCapitalizeLastWord = false)
        {
            if (changeCaseOptions == 1) //Splitting to sentences
            {
                return ChangeSentenceCase(source, exceptionWords, useWhiteList, exceptionChars, wordSplitters);
            }
            else //Splitting to words
            {
                if (changeCaseOptions == 2)
                    return ChangeWordsCase(source, ChangeCaseOptions.LowerCase, exceptionWords, useWhiteList, exceptionChars, wordSplitters, alwaysCapitalize1stWord, alwaysCapitalizeLastWord);
                else if (changeCaseOptions == 3)
                    return ChangeWordsCase(source, ChangeCaseOptions.UpperCase, exceptionWords, useWhiteList, exceptionChars, wordSplitters, alwaysCapitalize1stWord, alwaysCapitalizeLastWord);
                else if (changeCaseOptions == 4)
                    return ChangeWordsCase(source, ChangeCaseOptions.TitleCase, exceptionWords, useWhiteList, exceptionChars, wordSplitters, alwaysCapitalize1stWord, alwaysCapitalizeLastWord);
                else //if (changeCaseOptions == 5)
                    return ChangeWordsCase(source, ChangeCaseOptions.ToggleCase, exceptionWords, useWhiteList, exceptionChars, wordSplitters, alwaysCapitalize1stWord, alwaysCapitalizeLastWord);
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
            wordSplitters = null;
            alwaysCapitalize1stWord = alwaysCapitalize1stWordCheckBox.Checked;
            alwaysCapitalizeLastWord = alwaysCapitalizeLastWordCheckBox.Checked;

            if (exceptionWordsCheckBox.IsEnabled() && exceptionWordsCheckBox.Checked)
                exceptionWords = exceptionWordsBoxCustom.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (onlyWordsCheckBox.IsEnabled() && onlyWordsCheckBox.Checked)
                exceptionWords = exceptionWordsBoxCustom.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (exceptionCharsCheckBox.IsEnabled() && exceptionCharsCheckBox.Checked)
                exceptionChars = exceptionCharsBox.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (wordSplittersCheckBox.IsEnabled() && wordSplittersCheckBox.Checked)
                wordSplitters = wordSplittersBox.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

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
                newTagValue = changeCase(sourceTagValue, changeCaseFlag, exceptionWords, useWhiteList, exceptionChars, wordSplitters, alwaysCapitalize1stWord, alwaysCapitalizeLastWord);
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
            wordSplitters = null;

            if (exceptionWordsCheckBox.IsEnabled() && exceptionWordsCheckBox.Checked)
                exceptionWords = exceptionWordsBoxCustom.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (exceptionWordsCheckBox.IsEnabled() && onlyWordsCheckBox.Checked)
                exceptionWords = exceptionWordsBoxCustom.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (exceptionCharsCheckBox.IsEnabled() && exceptionCharsCheckBox.Checked)
                exceptionChars = exceptionCharsBox.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (wordSplittersCheckBox.IsEnabled() && wordSplittersCheckBox.Checked)
                wordSplitters = wordSplittersBox.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < previewTable.Rows.Count; i++)
            {
                if ((string)previewTable.Rows[i].Cells[0].Value == "T")
                {
                    newTagValue = changeCase((string)previewTable.Rows[i].Cells[5].Value, changeCaseFlag, exceptionWords, useWhiteList, exceptionChars, wordSplitters);
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
            SavedSettings.exceptionChars = exceptionCharsBox.Text;
            SavedSettings.useWordSplitters = wordSplittersCheckBox.Checked;
            SavedSettings.wordSplitters = wordSplittersBox.Text;
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
            exceptWordsCheckBox_CheckedChanged(null, null);
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
            onlyWordsCheckBox_CheckedChanged(null, null);
        }

        private void exceptCharsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            exceptionCharsBox.Enable(exceptionCharsCheckBox.Checked);
            buttonASRExceptWordsAfterSymbols.Enable(exceptionCharsCheckBox.Checked);
        }

        private void exceptionCharsCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!exceptionCharsCheckBox.IsEnabled())
                return;

            exceptionCharsCheckBox.Checked = !exceptionCharsCheckBox.Checked;
            exceptCharsCheckBox_CheckedChanged(null, null);
        }

        private void wordSplittersCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            wordSplittersBox.Enable(wordSplittersCheckBox.Checked);
            buttonASRWordSplitters.Enable(wordSplittersCheckBox.Checked);
        }

        private void wordSplittersCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!wordSplittersCheckBox.IsEnabled())
                return;

            wordSplittersCheckBox.Checked = !wordSplittersCheckBox.Checked;
            wordSplittersCheckBox_CheckedChanged(null, null);
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
                    wordSplitters = null;

                    if (exceptionWordsCheckBox.IsEnabled() && exceptionWordsCheckBox.Checked)
                        exceptionWords = exceptionWordsBoxCustom.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (exceptionWordsCheckBox.IsEnabled() && onlyWordsCheckBox.Checked)
                        exceptionWords = exceptionWordsBoxCustom.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (exceptionCharsCheckBox.IsEnabled() && exceptionCharsCheckBox.Checked)
                        exceptionChars = exceptionCharsBox.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (wordSplittersCheckBox.IsEnabled() && wordSplittersCheckBox.Checked)
                        wordSplitters = wordSplittersBox.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    previewTable.Rows[e.RowIndex].Cells[0].Value = "T";

                    sourceTagValue = (string)previewTable.Rows[e.RowIndex].Cells[3].Value;
                    sourceTagTValue = (string)previewTable.Rows[e.RowIndex].Cells[4].Value;

                    newTagValue = changeCase(sourceTagValue, changeCaseFlag, exceptionWords, useWhiteList, exceptionChars, wordSplitters);
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
            CustomComboBoxLeave(exceptionWordsBoxCustom);
        }

        private void casingRuleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (titleCaseRadioButton.Checked || toggleCaseRadioButton.Checked)
            {
                alwaysCapitalize1stWordCheckBox.Enable(true);
                alwaysCapitalizeLastWordCheckBox.Enable(true);
            }
            else
            {
                alwaysCapitalize1stWordCheckBox.Enable(false);
                alwaysCapitalizeLastWordCheckBox.Enable(false);
            }
        }

        private void sentenceCaseRadioButtonLabel_Click(object sender, EventArgs e)
        {
            if (!sentenceCaseRadioButton.IsEnabled())
                return;

            sentenceCaseRadioButton.Checked = true;
            casingRuleRadioButton_CheckedChanged(null, null);
        }

        private void lowerCaseRadioButtonLabel_Click(object sender, EventArgs e)
        {
            if (!lowerCaseRadioButton.IsEnabled())
                return;

            lowerCaseRadioButton.Checked = true;
            casingRuleRadioButton_CheckedChanged(null, null);
        }

        private void upperCaseRadioButtonLabel_Click(object sender, EventArgs e)
        {
            if (!upperCaseRadioButton.IsEnabled())
                return;

            upperCaseRadioButton.Checked = true;
            casingRuleRadioButton_CheckedChanged(null, null);
        }

        private void titleCaseRadioButtonLabel_Click(object sender, EventArgs e)
        {
            if (!titleCaseRadioButton.IsEnabled())
                return;

            titleCaseRadioButton.Checked = true;
            casingRuleRadioButton_CheckedChanged(null, null);
        }

        private void toggleCaseRadioButtonLabel_Click(object sender, EventArgs e)
        {
            if (!toggleCaseRadioButton.IsEnabled())
                return;

            toggleCaseRadioButton.Checked = true;
            casingRuleRadioButton_CheckedChanged(null, null);
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

        private void buttonASR_Click(object sender, EventArgs e)
        {
            SavedSettings.exceptionWordsASR = exceptionWordsBoxCustom.Text;
        }

        private void buttonASRExceptWordsAfterSymbols_Click(object sender, EventArgs e)
        {
            SavedSettings.exceptionCharsASR = exceptionCharsBox.Text;
        }

        private void buttonASRWordSplitters_Click(object sender, EventArgs e)
        {
            SavedSettings.wordSplittersASR = buttonASRWordSplitters.Text;
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            PluginQuickSettings settings = new PluginQuickSettings(TagToolsPlugin);
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
