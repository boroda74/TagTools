using MusicBeePlugin.Properties;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class ChangeCaseCommand : PluginWindowTemplate
    {
        public enum ChangeCaseOptions
        {
            sentenceCase = -1,
            lowerCase = 0,
            upperCase = 1,
            titleCase = 2,
            toggleCase = 3,
        }

        private delegate void AddRowToTable(string[] row);
        private delegate void ProcessRowOfTable(int row);
        private AddRowToTable addRowToTable;
        private ProcessRowOfTable processRowOfTable;

        private string[] files = new string[0];
        private List<string[]> tags = new List<string[]>();
        private Plugin.MetaDataType sourceTagId;

        private int changeCaseFlag;
        private bool useWhiteList;
        private string[] exceptionWords;
        private string[] exceptionChars;
        private string[] wordSplitters;
        private bool alwaysCapitalize1stWord;
        private bool alwaysCapitalizeLastWord;

        public ChangeCaseCommand()
        {
            InitializeComponent();
        }

        public ChangeCaseCommand(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            removeExceptionButton.Image = Plugin.ButtonRemoveImage;

            Plugin.FillListByTagNames(sourceTagList.Items);
            sourceTagList.Text = Plugin.SavedSettings.changeCaseSourceTagName;

            setChangeCaseOptionsRadioButtons(Plugin.SavedSettings.changeCaseFlag);
            exceptionWordsCheckBox.Checked = Plugin.SavedSettings.useExceptionWords;
            onlyWordsCheckBox.Checked = Plugin.SavedSettings.useOnlyWords;
            exceptionWordsBox.Items.AddRange(Plugin.SavedSettings.exceptionWords);
            exceptionWordsBox.Text = Plugin.SavedSettings.exceptionWords[0];
            exceptionCharsCheckBox.Checked = Plugin.SavedSettings.useExceptionChars;
            exceptionCharsBox.Text = Plugin.SavedSettings.exceptionChars;
            wordSplittersCheckBox.Checked = Plugin.SavedSettings.useWordSplitters;
            wordSplittersBox.Text = Plugin.SavedSettings.wordSplitters;
            alwaysCapitalize1stWordCheckBox.Checked = Plugin.SavedSettings.alwaysCapitalize1stWord;
            alwaysCapitalizeLastWordCheckBox.Checked = Plugin.SavedSettings.alwaysCapitalizeLastWord;

            exceptWordsCheckBox_CheckedChanged(null, null);
            exceptCharsCheckBox_CheckedChanged(null, null);
            wordSplittersCheckBox_CheckedChanged(null, null);
            sentenceCaseRadioButton_CheckedChanged(null, null);

            DatagridViewCheckBoxHeaderCell cbHeader = new DatagridViewCheckBoxHeaderCell();
            cbHeader.setState(true);
            cbHeader.OnCheckBoxClicked += new CheckBoxClickedHandler(cbHeader_OnCheckBoxClicked);

            DataGridViewCheckBoxColumn colCB = new DataGridViewCheckBoxColumn();
            colCB.HeaderCell = cbHeader;
            colCB.ThreeState = true;
            colCB.FalseValue = "False";
            colCB.TrueValue = "True";
            colCB.IndeterminateValue = "";
            colCB.Width = 25;
            colCB.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            previewTable.Columns.Insert(0, colCB);

            previewTable.Columns[2].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[4].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[6].HeaderCell.Style.WrapMode = DataGridViewTriState.True;

            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            addRowToTable = previewList_AddRowToTable;
            processRowOfTable = previewList_ProcessRowOfTable;

            int column1Width;
            int column2Width;
            int column3Width;

            int table2column1Width;
            int table2column2Width;
            int table2column3Width;

            loadWindowSizesPositions(true, out column1Width, out column2Width, out column3Width, out table2column1Width, out table2column2Width, out table2column3Width);

            if (column1Width > 0)
            {
                previewTable.Columns[1].Width = column1Width;
                previewTable.Columns[2].Width = column2Width;
                previewTable.Columns[3].Width = column3Width;
            }
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

        private void previewList_AddRowToTable(string[] row)
        {
            previewTable.Rows.Add(row);
        }

        private void previewList_ProcessRowOfTable(int row)
        {
            previewTable.Rows[row].Cells[0].Value = "";
        }

        public static string ChangeSubstringCase(string substring, ChangeCaseOptions changeCaseOption, bool isTheFirstWord)
        {
            string newSubstring = "";

            switch (changeCaseOption)
            {
                case ChangeCaseOptions.sentenceCase:
                    if (isTheFirstWord)
                    {
                        newSubstring = ChangeSubstringCase(substring, ChangeCaseOptions.titleCase, isTheFirstWord);
                    }
                    else
                    {
                        newSubstring = ChangeSubstringCase(substring, ChangeCaseOptions.lowerCase, isTheFirstWord);
                    }
                    break;
                case ChangeCaseOptions.titleCase:
                    bool isTheFirstChar = true;

                    foreach (char currentChar in substring)
                    {
                        if (isTheFirstChar)
                        {
                            newSubstring = newSubstring + ("" + currentChar).ToUpper();
                            isTheFirstChar = false;
                        }
                        else
                            newSubstring = newSubstring + ("" + currentChar).ToLower();
                    }
                    break;
                case ChangeCaseOptions.lowerCase:
                    newSubstring = substring.ToLower();
                    break;
                case ChangeCaseOptions.upperCase:
                    newSubstring = substring.ToUpper();
                    break;
                case ChangeCaseOptions.toggleCase:
                    foreach (char currentChar in substring)
                    {
                        if (("" + currentChar).ToUpper() == ("" + currentChar)) //Char is uppercased
                        {
                            newSubstring = newSubstring + ("" + currentChar).ToLower();
                        }
                        else //Char is lowercased
                        {
                            newSubstring = newSubstring + ("" + currentChar).ToUpper();
                        }
                    }
                    break;
            }

            return newSubstring;
        }

        public static bool CharIsCaseSensitive(char item)
        {
            if (("" + item).ToLower() == ("" + item).ToUpper())
                return false;
            else
                return true;
        }

        public static bool IsItemContainedInList(string item, string[] list)
        {
            item = "" + item; //It converts null to string

            foreach (string currentItem in list)
                if (item.ToLower() == currentItem.ToLower()) return true;

            return false;
        }

        public static bool IsItemContainedInList(char item, string[] list)
        {
            return IsItemContainedInList("" + item, list);
        }

        public static string ChangeWordsCase(string source, ChangeCaseOptions changeCaseOption, string[] exceptionWords = null, bool useWhiteList = false, string[] exceptionChars = null, string[] wordSplitters = null, bool alwaysCapitalize1stWord = false, bool alwaysCapitalizeLastWord = false)
        {
            string newString = "";
            string currentWord = "";
            char prevChar = '\0';
            bool wasCharException = false;
            bool isTheFirstWord = true;

            if (exceptionWords == null)
            {
                exceptionWords = new string[1];
                exceptionWords[0] = "";
            }

            if (exceptionChars == null)
            {
                exceptionChars = new string[1];
                exceptionChars[0] = "";
            }

            if (wordSplitters == null)
            {
                wordSplitters = new string[1];
                wordSplitters[0] = "";
            }

            foreach (char currentChar in source)
            {
                if (IsItemContainedInList(currentChar, wordSplitters) || currentChar == ' ' || currentChar == Plugin.MultipleItemsSplitterId) //Possible end of word
                {
                    if (!IsItemContainedInList(prevChar, wordSplitters) && prevChar != ' ' && prevChar != Plugin.MultipleItemsSplitterId) //End of word
                    {
                        if (alwaysCapitalize1stWord && isTheFirstWord) //Always Capitalize 1st word in tag if this option is checked
                        {
                            newString = newString + ChangeSubstringCase(currentWord, ChangeCaseOptions.titleCase, true) + currentChar;
                        }
                        else if (wasCharException || (IsItemContainedInList(currentWord, exceptionWords) && !useWhiteList) || (!IsItemContainedInList(currentWord, exceptionWords) && useWhiteList)) //Ignore changing case
                        {
                            if (currentWord.Length == 1 && IsItemContainedInList(currentChar, wordSplitters))
                                currentWord = currentWord.ToUpper();

                            newString = newString + currentWord + currentChar;
                        }
                        else //Change case
                        {
                            if (currentWord.Length == 1 && IsItemContainedInList(currentChar, wordSplitters))
                            {
                                currentWord = currentWord.ToUpper();
                                newString = newString + currentWord + currentChar;
                            }
                            else
                            {
                                newString = newString + ChangeSubstringCase(currentWord, changeCaseOption, isTheFirstWord) + currentChar;
                            }
                        }


                        currentWord = ""; //Beginning of new word
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
                    if (currentWord == "" && CharIsCaseSensitive(currentChar)) //Beginning of new word
                    {
                        if (IsItemContainedInList(currentChar, exceptionChars)) //Ignore changing case later
                            wasCharException = true;

                        currentWord = currentWord + currentChar;
                    }
                    else if (currentWord == "") //Several repeating symbols between words
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
            if (alwaysCapitalizeLastWord) //Always Capitalize last word if this option is checked
                newString = newString + ChangeSubstringCase(currentWord, ChangeCaseOptions.titleCase, true);
            else if (wasCharException || (IsItemContainedInList(currentWord, exceptionWords) && !useWhiteList) || (!IsItemContainedInList(currentWord, exceptionWords) && useWhiteList)) //Ignore changing case
                newString = newString + currentWord;
            else //Change case
                newString = newString + ChangeSubstringCase(currentWord, changeCaseOption, isTheFirstWord);

            return newString;
        }

        public static string ChangeSentenceCase(string source, string[] exceptionWords = null, bool useWhiteList = false, string[] exceptionChars = null, string[] wordSplitters = null)
        {
            string newString = "";
            string currentSentence = "";
            char prevChar = '\0';

            foreach (char currentChar in source)
            {
                if ((prevChar == '.' && currentChar == ' ') || currentChar == Plugin.MultipleItemsSplitterId) //Beginning of new sentence
                {
                    newString = newString + ChangeWordsCase(currentSentence, ChangeCaseOptions.sentenceCase, exceptionWords, useWhiteList, exceptionChars, wordSplitters);
                    currentSentence = "" + currentChar;
                }
                else //Not the beginning of new sentence
                {
                    currentSentence = currentSentence + currentChar;
                }

                prevChar = currentChar;
            }

            //String is ended, so last currentSentence IS a sentence
            newString = newString + ChangeWordsCase(currentSentence, ChangeCaseOptions.sentenceCase, exceptionWords, useWhiteList, exceptionChars, wordSplitters);

            return newString;
        }

        private string changeCase(string source, int changeCaseOptions, string[] exceptionWords = null, bool useWhiteList = false, string[] exceptionChars = null, string[] wordSplitters = null, bool alwaysCapitalize1stWord = false, bool alwaysCapitalizeLastWord = false)
        {
            if (changeCaseOptions == 1) //Splitting to sentences
                return ChangeSentenceCase(source, exceptionWords, useWhiteList, exceptionChars, wordSplitters);
            else //Splitting to words
            {
                if (changeCaseOptions == 2)
                    return ChangeWordsCase(source, ChangeCaseOptions.lowerCase, exceptionWords, useWhiteList, exceptionChars, wordSplitters, alwaysCapitalize1stWord, alwaysCapitalizeLastWord);
                else if (changeCaseOptions == 3)
                    return ChangeWordsCase(source, ChangeCaseOptions.upperCase, exceptionWords, useWhiteList, exceptionChars, wordSplitters, alwaysCapitalize1stWord, alwaysCapitalizeLastWord);
                else if (changeCaseOptions == 4)
                    return ChangeWordsCase(source, ChangeCaseOptions.titleCase, exceptionWords, useWhiteList, exceptionChars, wordSplitters, alwaysCapitalize1stWord, alwaysCapitalizeLastWord);
                else //if (changeCaseOptions == 5)
                    return ChangeWordsCase(source, ChangeCaseOptions.toggleCase, exceptionWords, useWhiteList, exceptionChars, wordSplitters, alwaysCapitalize1stWord, alwaysCapitalizeLastWord);
            }
        }

        private bool prepareBackgroundPreview()
        {
            tags.Clear();
            previewTable.Rows.Clear();
            ((DatagridViewCheckBoxHeaderCell)previewTable.Columns[0].HeaderCell).setState(true);

            if (backgroundTaskIsWorking())
                return true;

            changeCaseFlag = getChangeCaseOptionsRadioButtons();

            useWhiteList = onlyWordsCheckBox.Checked;
            exceptionWords = null;
            exceptionChars = null;
            wordSplitters = null;
            alwaysCapitalize1stWord = alwaysCapitalize1stWordCheckBox.Checked;
            alwaysCapitalizeLastWord = alwaysCapitalizeLastWordCheckBox.Checked;

            if (exceptionWordsCheckBox.Enabled && exceptionWordsCheckBox.Checked)
                exceptionWords = exceptionWordsBox.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (exceptionWordsCheckBox.Enabled && onlyWordsCheckBox.Checked)
                exceptionWords = exceptionWordsBox.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (exceptionCharsCheckBox.Enabled && exceptionCharsCheckBox.Checked)
                exceptionChars = exceptionCharsBox.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (wordSplittersCheckBox.Enabled && wordSplittersCheckBox.Checked)
                wordSplitters = wordSplittersBox.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            sourceTagId = Plugin.GetTagId(sourceTagList.Text);

            files = null;
            if (!Plugin.MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files))
                files = new string[0];

            if (files.Length == 0)
            {
                MessageBox.Show(this, Plugin.MsgNoFilesSelected, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

            string track;
            string[] row = { "Checked", "File", "Track", "OriginalTag", "OriginalTagT", "NewTag", "NewTagT" };
            string[] tag = { "Checked", "file", "newTag" };

            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                currentFile = files[fileCounter];

                Plugin.SetStatusbarTextForFileOperations(Plugin.ChangeCaseCommandSbText, true, fileCounter, files.Length, currentFile);

                sourceTagValue = Plugin.GetFileTag(currentFile, sourceTagId);
                sourceTagTValue = Plugin.GetTagRepresentation(sourceTagValue);
                newTagValue = changeCase(sourceTagValue, changeCaseFlag, exceptionWords, useWhiteList, exceptionChars, wordSplitters, alwaysCapitalize1stWord, alwaysCapitalizeLastWord);
                newTagTValue = Plugin.GetTagRepresentation(newTagValue);

                track = Plugin.GetTrackRepresentation(currentFile);


                if (sourceTagValue == newTagValue)
                    isChecked = "False";
                else
                    isChecked = "True";

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

            Plugin.SetResultingSbText();
        }

        private void reapplyRules()
        {
            string newTagValue;
            string newTagTValue;

            if (previewTable.Rows.Count == 0)
            {
                MessageBox.Show(this, Plugin.MsgPreviewIsNotGeneratedNothingToChange, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            changeCaseFlag = getChangeCaseOptionsRadioButtons();

            useWhiteList = onlyWordsCheckBox.Checked;
            exceptionWords = null;
            exceptionChars = null;
            wordSplitters = null;

            if (exceptionWordsCheckBox.Enabled && exceptionWordsCheckBox.Checked)
                exceptionWords = exceptionWordsBox.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (exceptionWordsCheckBox.Enabled && onlyWordsCheckBox.Checked)
                exceptionWords = exceptionWordsBox.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (exceptionCharsCheckBox.Enabled && exceptionCharsCheckBox.Checked)
                exceptionChars = exceptionCharsBox.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (wordSplittersCheckBox.Enabled && wordSplittersCheckBox.Checked)
                wordSplitters = wordSplittersBox.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < previewTable.Rows.Count; i++)
            {
                if ((string)previewTable.Rows[i].Cells[0].Value == "True")
                {
                    newTagValue = changeCase((string)previewTable.Rows[i].Cells[5].Value, changeCaseFlag, exceptionWords, useWhiteList, exceptionChars, wordSplitters);
                    newTagTValue = Plugin.GetTagRepresentation(newTagValue);

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

                if (isChecked == "True")
                {
                    currentFile = tags[i][1];
                    newTagValue = tags[i][2];

                    tags[i][0] = "";

                    Invoke(processRowOfTable, new object[] { i });

                    Plugin.SetStatusbarTextForFileOperations(Plugin.ChangeCaseCommandSbText, false, i, tags.Count, currentFile);

                    Plugin.SetFileTag(currentFile, sourceTagId, newTagValue);
                    Plugin.CommitTagsToFile(currentFile);
                }
            }

            Plugin.RefreshPanels(true);

            Plugin.SetResultingSbText();
        }

        private void saveSettings()
        {
            saveWindowSizesPositions(previewTable.Columns[1].Width, previewTable.Columns[2].Width, previewTable.Columns[3].Width);

            Plugin.SavedSettings.changeCaseSourceTagName = sourceTagList.Text;
            Plugin.SavedSettings.changeCaseFlag = getChangeCaseOptionsRadioButtons();
            Plugin.SavedSettings.useExceptionWords = exceptionWordsCheckBox.Checked;
            Plugin.SavedSettings.useOnlyWords = onlyWordsCheckBox.Checked;
            exceptionWordsBox.Items.CopyTo(Plugin.SavedSettings.exceptionWords, 0);
            Plugin.SavedSettings.useExceptionChars = exceptionCharsCheckBox.Checked;
            Plugin.SavedSettings.exceptionChars = exceptionCharsBox.Text;
            Plugin.SavedSettings.useWordSplitters = wordSplittersCheckBox.Checked;
            Plugin.SavedSettings.wordSplitters = wordSplittersBox.Text;
            Plugin.SavedSettings.alwaysCapitalize1stWord = alwaysCapitalize1stWordCheckBox.Checked;
            Plugin.SavedSettings.alwaysCapitalizeLastWord = alwaysCapitalizeLastWordCheckBox.Checked;

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
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            exceptionWordsBox.Items.CopyTo(Plugin.SavedSettings.exceptionWords, 0);
            Close();
        }

        private void buttonReapply_Click(object sender, EventArgs e)
        {
            reapplyRules();
        }

        private void exceptWordsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            exceptionWordsBox.Enabled = exceptionWordsCheckBox.Checked || onlyWordsCheckBox.Checked;
            if (exceptionWordsCheckBox.Checked)
                onlyWordsCheckBox.Checked = false;
        }

        private void onlyWordsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            exceptionWordsBox.Enabled = exceptionWordsCheckBox.Checked || onlyWordsCheckBox.Checked;
            if (onlyWordsCheckBox.Checked)
                exceptionWordsCheckBox.Checked = false;
        }

        private void exceptCharsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            exceptionCharsBox.Enabled = exceptionCharsCheckBox.Checked;
        }

        private void wordSplittersCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            wordSplittersBox.Enabled = wordSplittersCheckBox.Checked;
        }

        private void cbHeader_OnCheckBoxClicked(bool state)
        {
            foreach (DataGridViewRow row in previewTable.Rows)
            {
                if ((string)row.Cells[0].Value == "")
                    continue;

                if (state)
                    row.Cells[0].Value = "False";
                else
                    row.Cells[0].Value = "True";

                DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(0, row.Index);
                previewList_CellContentClick(null, e);
            }
        }

        private void previewList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((string)previewTable.Rows[e.RowIndex].Cells[3].Value == (string)previewTable.Rows[e.RowIndex].Cells[5].Value)
                previewTable.Rows[e.RowIndex].Cells[6].Style = Plugin.UnchangedStyle;
            else
                previewTable.Rows[e.RowIndex].Cells[6].Style = Plugin.ChangedStyle;
        }

        private void previewList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string sourceTagValue;
                string sourceTagTValue;
                string newTagValue;
                string newTagTValue;

                string isChecked = (string)previewTable.Rows[e.RowIndex].Cells[0].Value;

                if (isChecked == "True")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "False";

                    sourceTagValue = (string)previewTable.Rows[e.RowIndex].Cells[3].Value;
                    sourceTagTValue = (string)previewTable.Rows[e.RowIndex].Cells[4].Value;

                    previewTable.Rows[e.RowIndex].Cells[5].Value = sourceTagValue;
                    previewTable.Rows[e.RowIndex].Cells[6].Value = sourceTagTValue;
                }
                else if (isChecked == "False")
                {
                    changeCaseFlag = getChangeCaseOptionsRadioButtons();

                    useWhiteList = onlyWordsCheckBox.Checked;
                    exceptionWords = null;
                    exceptionChars = null;
                    wordSplitters = null;

                    if (exceptionWordsCheckBox.Enabled && exceptionWordsCheckBox.Checked)
                        exceptionWords = exceptionWordsBox.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (exceptionWordsCheckBox.Enabled && onlyWordsCheckBox.Checked)
                        exceptionWords = exceptionWordsBox.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (exceptionCharsCheckBox.Enabled && exceptionCharsCheckBox.Checked)
                        exceptionChars = exceptionCharsBox.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (wordSplittersCheckBox.Enabled && wordSplittersCheckBox.Checked)
                        wordSplitters = wordSplittersBox.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    previewTable.Rows[e.RowIndex].Cells[0].Value = "True";

                    sourceTagValue = (string)previewTable.Rows[e.RowIndex].Cells[3].Value;
                    sourceTagTValue = (string)previewTable.Rows[e.RowIndex].Cells[4].Value;

                    newTagValue = changeCase(sourceTagValue, changeCaseFlag, exceptionWords, useWhiteList, exceptionChars, wordSplitters);
                    newTagTValue = Plugin.GetTagRepresentation(newTagValue);

                    previewTable.Rows[e.RowIndex].Cells[5].Value = newTagValue;
                    previewTable.Rows[e.RowIndex].Cells[6].Value = newTagTValue;
                }
            }
        }

        public override void enableDisablePreviewOptionControls(bool enable)
        {
            if (enable && previewIsGenerated)
                return;

            sourceTagList.Enabled = enable;
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
            buttonOK.Enabled = true;
            buttonPreview.Enabled = true;
            buttonReapply.Enabled = true;
        }

        public override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enabled = false;
            buttonPreview.Enabled = false;
            buttonReapply.Enabled = false;
        }

        private void exceptionWordsBox_Leave(object sender, EventArgs e)
        {
            Plugin.ComboBoxLeave(exceptionWordsBox);
        }

        private void sentenceCaseRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sentenceCaseRadioButton.Checked)
            {
                alwaysCapitalize1stWordCheckBox.Enabled = false;
                alwaysCapitalizeLastWordCheckBox.Enabled = false;
            }
            else
            {
                alwaysCapitalize1stWordCheckBox.Enabled = true;
                alwaysCapitalizeLastWordCheckBox.Enabled = true;
            }
        }

        private void removeExceptionButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < exceptionWordsBox.Items.Count; i++)
            {
                if (exceptionWordsBox.Items[i].ToString() == exceptionWordsBox.Text)
                {
                    exceptionWordsBox.Items.Remove(exceptionWordsBox.Text);
                    exceptionWordsBox.Items.Add("");

                    exceptionWordsBox.Text = "";

                    break;
                }
            }
        }

        private void buttonASR_Click(object sender, EventArgs e)
        {
            Plugin.SavedSettings.exceptionWordsASR = exceptionWordsBox.Text;
        }

        private void buttonASRExceptWordsAfterSymbols_Click(object sender, EventArgs e)
        {
            Plugin.SavedSettings.exceptionCharsASR = exceptionCharsBox.Text;
        }

        private void buttonASRWordSplitters_Click(object sender, EventArgs e)
        {
            Plugin.SavedSettings.wordSplittersASR = buttonASRWordSplitters.Text;
        }
    }
}
