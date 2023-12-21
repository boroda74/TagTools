using MusicBeePlugin.Properties;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using static MusicBeePlugin.AdvancedSearchAndReplaceCommand;
using static MusicBeePlugin.Plugin;
using ExtensionMethods;

namespace MusicBeePlugin
{
    public partial class ASRPresetEditor : PluginWindowTemplate
    { 
        private Preset preset;
        private bool settingsSaved = false;
        private string currentLanguage;
        private static string WritableAllTagsLocalizedItem;

        public ASRPresetEditor(Plugin tagToolsPluginParam) : base(tagToolsPluginParam)
        {
            InitializeComponent();
            WritableAllTagsLocalizedItem = parameterTagTypeList.Items[3].ToString();
        }

        protected override void initializeForm()
        {
            base.initializeForm();

            if (UseMusicBeeFontSkinColors)
            {
                linkLabel1.DisabledLinkColor = DimmedAccentColor;
                linkLabel1.LinkColor = TickedColor;
                linkLabel1.ActiveLinkColor = DimmedHighlight;
            }

            if (Language == "ru")
            {
                tableLayoutPanel1.ColumnStyles[2].Width = (int)Math.Round(180 * hDpiFontScaling);
                tableLayoutPanel1.ColumnStyles[7].Width = (int)Math.Round(180 * hDpiFontScaling);
            }
            else
            {
                tableLayoutPanel1.ColumnStyles[2].Width = (int)Math.Round(135 * hDpiFontScaling);
                tableLayoutPanel1.ColumnStyles[7].Width = (int)Math.Round(135 * hDpiFontScaling);
            }


            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        private void makeReadonly(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is Button)
                {
                    (control as Button).Enable(false);
                }
                else if (control.GetType().IsSubclassOf(typeof(TextBox)) || control is TextBox)
                {
                    (control as TextBox).ReadOnly = true;
                }
                else if (control.GetType().IsSubclassOf(typeof(ComboBox)) || control is ComboBox)
                {
                    (control as ComboBox).Enable(false);
                }
                else if (control.GetType().IsSubclassOf(typeof(ListBox)) || control is ListBox)
                {
                    (control as ListBox).Enable(false);
                }
                else if (control is GroupBox)
                {
                    //(control as GroupBox).Enable(false);
                }
                else
                {
                    control.Enable(false);
                }

                makeReadonly(control);
            }
        }


        private CheckState getThreeStateChecked(bool? state)
        {
            if (state == true)
                return CheckState.Checked;
            else if (state == false)
                return CheckState.Unchecked;
            else
                return CheckState.Indeterminate;
        }

        private bool? getThreeStateBool(CheckState state)
        {
            if (state == CheckState.Checked)
                return true;
            else if (state == CheckState.Unchecked)
                return false;
            else
                return null;
        }

        public bool editPreset(Preset presetParam, bool readOnly)
        {
            preset = presetParam;

            if (readOnly)
            {
                makeReadonly(this);
                buttonCancel.Enable(true);
                buttonOK.Enable(true);
                linkLabel1.Enable(true);
            }

            guidBox.Text = preset.guid.ToString();
            modifiedBox.Text = preset.modifiedUtc.ToLocalTime().ToString();
            userPresetCheckBox.Checked = preset.userPreset;
            userPresetCheckBox.Enable(DeveloperMode);
            removePresetCheckBox.Checked = preset.removePreset;
            removePresetCheckBox.Visible = DeveloperMode;
            customizedByUserCheckBox.Checked = preset.customizedByUser;
            customizedByUserCheckBox.Enable(DeveloperMode);


            currentLanguage = null;

            bool englishIsAvailable = false;
            bool nativeLanguageIsAvailable = false;

            foreach (string language in preset.names.Keys)
            {
                languages.Items.Add(language);

                if (language == Language)
                    nativeLanguageIsAvailable = true;

                if (language == "en")
                    englishIsAvailable = true;
            }

            if (!nativeLanguageIsAvailable)
                languages.Items.Add(Language);

            if (!englishIsAvailable)
                languages.Items.Add("en");

            if (nativeLanguageIsAvailable)
                currentLanguage = Language;
            else if (englishIsAvailable)
                currentLanguage = "en";
            else
                currentLanguage = (string)languages.Items[0];

            nameBox.Text = GetDictValue(preset.names, currentLanguage);
            descriptionBox.Text = GetDictValue(preset.descriptions, currentLanguage);

            languages.SelectedItem = currentLanguage;


            parameterTagTypeList.SelectedIndex = (int)preset.parameterTagTypeNew;
            parameterTag2TypeList.SelectedIndex = (int)preset.parameterTag2TypeNew;
            parameterTag3TypeList.SelectedIndex = (int)preset.parameterTag3TypeNew;
            parameterTag4TypeList.SelectedIndex = (int)preset.parameterTag4TypeNew;
            parameterTag5TypeList.SelectedIndex = (int)preset.parameterTag5TypeNew;
            parameterTag6TypeList.SelectedIndex = (int)preset.parameterTag6TypeNew;


            parameterTagList.Text = AsrGetTagName(preset.parameterTagId);
            parameterTag2List.Text = AsrGetTagName(preset.parameterTag2Id);
            parameterTag3List.Text = AsrGetTagName(preset.parameterTag3Id);
            parameterTag4List.Text = AsrGetTagName(preset.parameterTag4Id);
            parameterTag5List.Text = AsrGetTagName(preset.parameterTag5Id);
            parameterTag6List.Text = AsrGetTagName(preset.parameterTag6Id);


            customTextBox.Text = preset.customText;
            customTextCheckBox.Checked = preset.customTextChecked;
            customText2Box.Text = preset.customText2;
            customText2CheckBox.Checked = preset.customText2Checked;
            customText3Box.Text = preset.customText3;
            customText3CheckBox.Checked = preset.customText3Checked;
            customText4Box.Text = preset.customText4;
            customText4CheckBox.Checked = preset.customText4Checked;


            ignoreCaseCheckBox.Checked = preset.ignoreCase;


            searchedPatternBox.Text = preset.searchedPattern;
            searchedPattern2Box.Text = preset.searchedPattern2;
            searchedPattern3Box.Text = preset.searchedPattern3;
            searchedPattern4Box.Text = preset.searchedPattern4;
            searchedPattern5Box.Text = preset.searchedPattern5;

            replacedPatternBox.Text = preset.replacedPattern;
            replacedPattern2Box.Text = preset.replacedPattern2;
            replacedPattern3Box.Text = preset.replacedPattern3;
            replacedPattern4Box.Text = preset.replacedPattern4;
            replacedPattern5Box.Text = preset.replacedPattern5;


            searchedTagList.Text = AsrGetTagName(preset.searchedTagId);
            searchedTag2List.Text = AsrGetTagName(preset.searchedTag2Id);
            searchedTag3List.Text = AsrGetTagName(preset.searchedTag3Id);
            searchedTag4List.Text = AsrGetTagName(preset.searchedTag4Id);
            searchedTag5List.Text = AsrGetTagName(preset.searchedTag5Id);


            replacedTagList.Text = AsrGetTagName(preset.replacedTagId);
            replacedTag2List.Text = AsrGetTagName(preset.replacedTag2Id);
            replacedTag3List.Text = AsrGetTagName(preset.replacedTag3Id);
            replacedTag4List.Text = AsrGetTagName(preset.replacedTag4Id);
            replacedTag5List.Text = AsrGetTagName(preset.replacedTag5Id);

            appendCheckBox.Checked = preset.append;
            append2CheckBox.Checked = preset.append2;
            append3CheckBox.Checked = preset.append3;
            append4CheckBox.Checked = preset.append4;
            append5CheckBox.Checked = preset.append5;

            condition1CheckBox.CheckState = getThreeStateChecked(preset.limitation1);
            condition2CheckBox.CheckState = getThreeStateChecked(preset.limitation2);
            condition3CheckBox.CheckState = getThreeStateChecked(preset.limitation3);
            condition4CheckBox.CheckState = getThreeStateChecked(preset.limitation4);
            condition5CheckBox.CheckState = getThreeStateChecked(preset.limitation5);

            languages.Text = Language;

            if (readOnly)
            {
                languages.Enable(true);
                parameterTagTypeList.Enable(false);
                parameterTag2TypeList.Enable(false);
                parameterTag3TypeList.Enable(false);
                parameterTag4TypeList.Enable(false);
                parameterTag5TypeList.Enable(false);
                parameterTag6TypeList.Enable(false);

                customTextCheckBox.Enable(false);
                customText2CheckBox.Enable(false);
                customText3CheckBox.Enable(false);
                customText4CheckBox.Enable(false);

                customTextBox.ReadOnly = !customTextCheckBox.Checked;
                customText2Box.ReadOnly = !customText2CheckBox.Checked;
                customText3Box.ReadOnly = !customText3CheckBox.Checked;
                customText4Box.ReadOnly = !customText4CheckBox.Checked;
            }

            Display(this, true);

            return settingsSaved;
        }

        private bool saveSettings()
        {
            SetDictValue(preset.names, currentLanguage, nameBox.Text);
            SetDictValue(preset.descriptions, currentLanguage, descriptionBox.Text);

            preset.userPreset = userPresetCheckBox.Checked;
            preset.removePreset = removePresetCheckBox.Checked;
            preset.customizedByUser = customizedByUserCheckBox.Checked;
            preset.ignoreCase = ignoreCaseCheckBox.Checked;


            preset.searchedPattern = searchedPatternBox.Text;
            preset.searchedPattern2 = searchedPattern2Box.Text;
            preset.searchedPattern3 = searchedPattern3Box.Text;
            preset.searchedPattern4 = searchedPattern4Box.Text;
            preset.searchedPattern5 = searchedPattern5Box.Text;

            preset.searchedTagId = AsrGetTagId(searchedTagList.Text);
            preset.searchedTag2Id = AsrGetTagId(searchedTag2List.Text);
            preset.searchedTag3Id = AsrGetTagId(searchedTag3List.Text);
            preset.searchedTag4Id = AsrGetTagId(searchedTag4List.Text);
            preset.searchedTag5Id = AsrGetTagId(searchedTag5List.Text);


            preset.replacedPattern = replacedPatternBox.Text;
            preset.replacedPattern2 = replacedPattern2Box.Text;
            preset.replacedPattern3 = replacedPattern3Box.Text;
            preset.replacedPattern4 = replacedPattern4Box.Text;
            preset.replacedPattern5 = replacedPattern5Box.Text;

            preset.replacedTagId = AsrGetTagId(replacedTagList.Text);
            preset.replacedTag2Id = AsrGetTagId(replacedTag2List.Text);
            preset.replacedTag3Id = AsrGetTagId(replacedTag3List.Text);
            preset.replacedTag4Id = AsrGetTagId(replacedTag4List.Text);
            preset.replacedTag5Id = AsrGetTagId(replacedTag5List.Text);


            preset.parameterTagTypeNew = (TagType)parameterTagTypeList.SelectedIndex;
            preset.parameterTag2TypeNew = (TagType)parameterTag2TypeList.SelectedIndex;
            preset.parameterTag3TypeNew = (TagType)parameterTag3TypeList.SelectedIndex;
            preset.parameterTag4TypeNew = (TagType)parameterTag4TypeList.SelectedIndex;
            preset.parameterTag5TypeNew = (TagType)parameterTag5TypeList.SelectedIndex;
            preset.parameterTag6TypeNew = (TagType)parameterTag6TypeList.SelectedIndex;


            preset.parameterTagId = AsrGetTagId(parameterTagList.Text);
            preset.parameterTag2Id = AsrGetTagId(parameterTag2List.Text);
            preset.parameterTag3Id = AsrGetTagId(parameterTag3List.Text);
            preset.parameterTag4Id = AsrGetTagId(parameterTag4List.Text);
            preset.parameterTag5Id = AsrGetTagId(parameterTag5List.Text);
            preset.parameterTag6Id = AsrGetTagId(parameterTag6List.Text);


            preset.customText = customTextBox.Text;
            preset.customTextChecked = customTextCheckBox.Checked;
            preset.customText2 = customText2Box.Text;
            preset.customText2Checked = customText2CheckBox.Checked;
            preset.customText3 = customText3Box.Text;
            preset.customText3Checked = customText3CheckBox.Checked;
            preset.customText4 = customText4Box.Text;
            preset.customText4Checked = customText4CheckBox.Checked;

            preset.append = appendCheckBox.Checked;
            preset.append2 = append2CheckBox.Checked;
            preset.append3 = append3CheckBox.Checked;
            preset.append4 = append4CheckBox.Checked;
            preset.append5 = append5CheckBox.Checked;

            preset.limitation1 = getThreeStateBool(condition1CheckBox.CheckState);
            preset.limitation2 = getThreeStateBool(condition2CheckBox.CheckState);
            preset.limitation3 = getThreeStateBool(condition3CheckBox.CheckState);
            preset.limitation4 = getThreeStateBool(condition4CheckBox.CheckState);
            preset.limitation5 = getThreeStateBool(condition5CheckBox.CheckState);

            return true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            settingsSaved = saveSettings();
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            settingsSaved = false;
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://msdn.microsoft.com/en-us/library/az24scfc.aspx");
        }

        private void languages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentLanguage == null || currentLanguage == (string)languages.SelectedItem)
                return;


            string prevName = nameBox.Text;
            string prevDescription = descriptionBox.Text;

            SetDictValue(preset.names, currentLanguage, prevName);
            SetDictValue(preset.descriptions, currentLanguage, prevDescription);


            currentLanguage = (string)languages.SelectedItem;
            nameBox.Text = GetDictValue(preset.names, currentLanguage);
            descriptionBox.Text = GetDictValue(preset.descriptions, currentLanguage);

            if (nameBox.Text == string.Empty)
                nameBox.Text = prevName;

            if (descriptionBox.Text == string.Empty)
                descriptionBox.Text = prevDescription;
        }

        private static void FillParameterTagTypeList(TagType prevTagType, ComboBox tagTypeList)
        {
            if (tagTypeList == null)
                return;


            if (prevTagType == TagType.NotUsed)
            {
                if (tagTypeList.SelectedIndex != 0)
                    tagTypeList.SelectedIndex = 0;

                if (tagTypeList.Items.Count == 4)
                    tagTypeList.Items.RemoveAt(3);

                tagTypeList.Enable(false);
            }
            else
            {
                if (prevTagType == TagType.WritableAllowAllTags)
                {
                    if (tagTypeList.Items.Count == 3)
                        tagTypeList.Items.Add(WritableAllTagsLocalizedItem);
                }
                else
                {
                    if (tagTypeList.SelectedIndex == 3)
                        tagTypeList.SelectedIndex = 2;

                    if (tagTypeList.Items.Count == 4)
                        tagTypeList.Items.RemoveAt(3);
                }

                tagTypeList.Enable(true);
            }
        }

        private void FillTagCombobox(ComboBox tagList, bool isSearchTag)
        {
            int searchFlag = isSearchTag ? 1 : 0;
            string tagName = tagList.Text;

            tagList.Items.Clear();


            // Let's add <Tag #> if either filled tag list is for search or <Tag #> is writable
            if (parameterTagTypeList.SelectedIndex >= (int)TagType.Writable - searchFlag)
                tagList.Items.Add("<" + ParameterTagName + " 1>");

            // Let's add <Tag #> if either filled tag list is for search or <Tag #> is writable
            if (parameterTag2TypeList.SelectedIndex >= (int)TagType.Writable - searchFlag)
                tagList.Items.Add("<" + ParameterTagName + " 2>");

            // Let's add <Tag #> if either filled tag list is for search or <Tag #> is writable
            if (parameterTag3TypeList.SelectedIndex >= (int)TagType.Writable - searchFlag)
                tagList.Items.Add("<" + ParameterTagName + " 3>");

            // Let's add <Tag #> if either filled tag list is for search or <Tag #> is writable
            if (parameterTag4TypeList.SelectedIndex >= (int)TagType.Writable - searchFlag)
                tagList.Items.Add("<" + ParameterTagName + " 4>");

            // Let's add <Tag #> if either filled tag list is for search or <Tag #> is writable
            if (parameterTag5TypeList.SelectedIndex >= (int)TagType.Writable - searchFlag)
                tagList.Items.Add("<" + ParameterTagName + " 5>");

            // Let's add <Tag #> if either filled tag list is for search or <Tag #> is writable
            if (parameterTag6TypeList.SelectedIndex >= (int)TagType.Writable - searchFlag)
                tagList.Items.Add("<" + ParameterTagName + " 6>");


            tagList.Items.Add("<" + TempTagName + " 1>");
            tagList.Items.Add("<" + TempTagName + " 2>");
            tagList.Items.Add("<" + TempTagName + " 3>");
            tagList.Items.Add("<" + TempTagName + " 4>");

            if (isSearchTag) // It's read-only operation
            {
                FillListByTagNames(tagList.Items, true); // true: add read-only tags too
                FillListByPropNames(tagList.Items);
            }
            else  // It's writable operation
            {
                FillListByTagNames(tagList.Items);
            }

            tagList.Items.Add(ClipboardTagName);


            if (tagList.Items.Contains(tagName))
                tagList.Text = tagName;
            else
                tagList.SelectedIndex = 0;
        }

        private void parameterTagTypeChanged(ComboBox parameterTagTypeListParam, PictureBox pictureBox, ComboBox parameterTagListParam, 
            Label label, ComboBox parameterTagTypeListParam2 = null)
        {
            string parameterTagName = parameterTagListParam.Text;

            TagType tagType = (TagType)parameterTagTypeListParam.SelectedIndex;

            if (tagType == TagType.WritableAllowAllTags)
                pictureBox.Image = ThemedBitmapAddRef(this, Warning);
            else
                pictureBox.Image = Resources.transparent_15;


            FillParameterTagTypeList(tagType, parameterTagTypeListParam2);

            if (tagType == TagType.WritableAllowAllTags) //Let's disable entering <ALL TAGS> pseudo-tag directly in preset editor for safety reasons !!!
                tagType = TagType.Writable;
            FillParameterTagList(tagType, parameterTagName, parameterTagListParam, label);


            FillTagCombobox(searchedTagList, true);
            FillTagCombobox(searchedTag2List, true);
            FillTagCombobox(searchedTag3List, true);
            FillTagCombobox(searchedTag4List, true);
            FillTagCombobox(searchedTag5List, true);

            FillTagCombobox(replacedTagList, false);
            FillTagCombobox(replacedTag2List, false);
            FillTagCombobox(replacedTag3List, false);
            FillTagCombobox(replacedTag4List, false);
            FillTagCombobox(replacedTag5List, false);
        }

        private void parameterTagTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            parameterTagTypeChanged(parameterTagTypeList, pictureBox17, parameterTagList, label21, parameterTag2TypeList);
        }

        private void parameterTag2TypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            parameterTagTypeChanged(parameterTag2TypeList, pictureBox18, parameterTag2List, label22, parameterTag3TypeList);
        }

        private void parameterTag3TypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            parameterTagTypeChanged(parameterTag3TypeList, pictureBox19, parameterTag3List, label23, parameterTag4TypeList);
        }

        private void parameterTag4TypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            parameterTagTypeChanged(parameterTag4TypeList, pictureBox28, parameterTag4List, label24, parameterTag5TypeList);
        }

        private void parameterTag5TypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            parameterTagTypeChanged(parameterTag5TypeList, pictureBox27, parameterTag5List, label25, parameterTag6TypeList);
        }

        private void parameterTag6TypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            parameterTagTypeChanged(parameterTag6TypeList, pictureBox26, parameterTag6List, label20);
        }

        private void customTextChecked_CheckedChanged(object sender, EventArgs e)//***---
        {
            customTextBox.Enable(customTextCheckBox.Checked);
            if (!customTextCheckBox.Checked)
            {
                customTextBox.Text = string.Empty;
                customText2CheckBox.Checked = false;
            }
            customText2CheckBox.Enable(customTextCheckBox.Checked);
        }

        private void customTextCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!customTextCheckBox.Enabled)
                return;

            customTextCheckBox.Checked = !customTextCheckBox.Checked;
            customTextChecked_CheckedChanged(null, null);
        }

        private void customText2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            customText2Box.Enable(customText2CheckBox.Checked);
            if (!customText2CheckBox.Checked)
            {
                customText2Box.Text = string.Empty;
                customText3CheckBox.Checked = false;
            }
            customText3CheckBox.Enable(customText2CheckBox.Checked);
        }

        private void customText2CheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!customText2CheckBox.Enabled)
                return;

            customText2CheckBox.Checked = !customText2CheckBox.Checked;
            customText2CheckBox_CheckedChanged(null, null);
        }
        
        private void customText3CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            customText3Box.Enable(customText3CheckBox.Checked);
            if (!customText3CheckBox.Checked)
            {
                customText3Box.Text = string.Empty;
                customText4CheckBox.Checked = false;
            }
            customText4CheckBox.Enable(customText3CheckBox.Checked);
        }

        private void customText3CheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!customText3CheckBox.Enabled)
                return;

            customText3CheckBox.Checked = !customText3CheckBox.Checked;
            customText3CheckBox_CheckedChanged(null, null);
        }

        private void customText4CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            customText4Box.Enable(customText4CheckBox.Checked);
            if (!customText4CheckBox.Checked)
            {
                customText4Box.Text = string.Empty;
            }
        }

        private void customText4CheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!customText4CheckBox.Enabled) 
                return;

            customText4CheckBox.Checked = !customText4CheckBox.Checked;
            customText4CheckBox_CheckedChanged(null, null);
        }

        private void ignoreCaseCheckBoxLabel_Click(object sender, EventArgs e)
        {
            ignoreCaseCheckBox.Checked = !ignoreCaseCheckBox.Checked;
        }
    }
}
