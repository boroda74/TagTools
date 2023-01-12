using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class ASRPresetEditor : PluginWindowTemplate
    {
        private AdvancedSearchAndReplaceCommand.Preset preset;
        private bool settingsSaved = false;
        private string currentLanguage = "";
        private AdvancedSearchAndReplaceCommand asrPlugin;

        public ASRPresetEditor()
        {
            InitializeComponent();
        }

        public ASRPresetEditor(Plugin tagToolsPluginParam, AdvancedSearchAndReplaceCommand asrPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;
            asrPlugin = asrPluginParam;

            initializeForm();
        }

        private void makeReadonly(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control.GetType() == typeof(Button))
                {
                    ((Button)control).Enabled = false;
                }
                else if (control.GetType().IsSubclassOf(typeof(TextBox)) || control.GetType() == typeof(TextBox))
                {
                    ((TextBox)control).ReadOnly = true;
                }
                else if (control.GetType().IsSubclassOf(typeof(ComboBox)) || control.GetType() == typeof(ComboBox))
                {
                    ((ComboBox)control).Enabled = false;
                }
                else if (control.GetType().IsSubclassOf(typeof(ListBox)) || control.GetType() == typeof(ListBox))
                {
                    ((ListBox)control).Enabled = false;
                }
                else if (control.GetType() == typeof(GroupBox))
                {
                    //((GroupBox)control).Enabled = false;
                }
                else
                {
                    control.Enabled = false;
                }

                makeReadonly(control);
            }
        }

        public bool editPreset(AdvancedSearchAndReplaceCommand.Preset presetParam, bool readOnly)
        {
            preset = presetParam;

            if (readOnly)
            {
                makeReadonly(this);
                buttonCancel.Enabled = true;
                buttonApply.Enabled = true;
                linkLabel1.Enabled = true;
            }

            guidBox.Text = preset.guid.ToString();
            modifiedBox.Text = preset.modifiedUtc.ToLocalTime().ToString();
            userPresetCheckBox.Checked = preset.userPreset;
            userPresetCheckBox.Enabled = Plugin.DeveloperMode;
            customizedByUserCheckBox.Checked = preset.customizedByUser;
            customizedByUserCheckBox.Enabled = Plugin.DeveloperMode;

            bool englishIsAvailable = false;
            bool nativeLanguageIsAvailable = false;

            foreach (string language in preset.names.Keys)
            {
                languages.Items.Add(language);

                if (language == Plugin.Language)
                    nativeLanguageIsAvailable = true;

                if (language == "en")
                    englishIsAvailable = true;
            }

            if (!englishIsAvailable)
            {
                languages.Items.Add("en");

                if (Plugin.Language == "en")
                    nativeLanguageIsAvailable = true;
            }

            if (!nativeLanguageIsAvailable)
                languages.Items.Add(Plugin.Language);


            parameterTagTypeList.SelectedIndex = preset.parameterTagType;
            parameterTag2TypeList.SelectedIndex = preset.parameterTag2Type;
            parameterTag3TypeList.SelectedIndex = preset.parameterTag3Type;
            parameterTag4TypeList.SelectedIndex = preset.parameterTag4Type;
            parameterTag5TypeList.SelectedIndex = preset.parameterTag5Type;
            parameterTag6TypeList.SelectedIndex = preset.parameterTag6Type;


            parameterTagList.Text = asrPlugin.getTagName(preset.parameterTagId);
            parameterTag2List.Text = asrPlugin.getTagName(preset.parameterTag2Id);
            parameterTag3List.Text = asrPlugin.getTagName(preset.parameterTag3Id);
            parameterTag4List.Text = asrPlugin.getTagName(preset.parameterTag4Id);
            parameterTag5List.Text = asrPlugin.getTagName(preset.parameterTag5Id);
            parameterTag6List.Text = asrPlugin.getTagName(preset.parameterTag6Id);


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


            searchedTagList.Text = asrPlugin.getTagName(preset.searchedTagId);
            searchedTag2List.Text = asrPlugin.getTagName(preset.searchedTag2Id);
            searchedTag3List.Text = asrPlugin.getTagName(preset.searchedTag3Id);
            searchedTag4List.Text = asrPlugin.getTagName(preset.searchedTag4Id);
            searchedTag5List.Text = asrPlugin.getTagName(preset.searchedTag5Id);


            replacedTagList.Text = asrPlugin.getTagName(preset.replacedTagId);
            replacedTag2List.Text = asrPlugin.getTagName(preset.replacedTag2Id);
            replacedTag3List.Text = asrPlugin.getTagName(preset.replacedTag3Id);
            replacedTag4List.Text = asrPlugin.getTagName(preset.replacedTag4Id);
            replacedTag5List.Text = asrPlugin.getTagName(preset.replacedTag5Id);

            appendCheckBox.Checked = preset.append;
            append2CheckBox.Checked = preset.append2;
            append3CheckBox.Checked = preset.append3;
            append4CheckBox.Checked = preset.append4;
            append5CheckBox.Checked = preset.append5;

            languages.Text = Plugin.Language;

            if (readOnly)
            {
                languages.Enabled = true;
                parameterTagTypeList.Enabled = false;
                parameterTag2TypeList.Enabled = false;
                parameterTag3TypeList.Enabled = false;
                parameterTag4TypeList.Enabled = false;
                parameterTag5TypeList.Enabled = false;
                parameterTag6TypeList.Enabled = false;

                customTextCheckBox.Enabled = false;
                customText2CheckBox.Enabled = false;
                customText3CheckBox.Enabled = false;
                customText4CheckBox.Enabled = false;

                customTextBox.ReadOnly = !customTextCheckBox.Checked;
                customText2Box.ReadOnly = !customText2CheckBox.Checked;
                customText3Box.ReadOnly = !customText3CheckBox.Checked;
                customText4Box.ReadOnly = !customText4CheckBox.Checked;
            }

            ShowDialog();
            return settingsSaved;
        }

        private bool saveSettings()
        {
            AdvancedSearchAndReplaceCommand.SetDictValue(preset.names, currentLanguage, nameBox.Text);
            AdvancedSearchAndReplaceCommand.SetDictValue(preset.descriptions, currentLanguage, descriptionBox.Text);

            preset.userPreset = userPresetCheckBox.Checked;
            preset.customizedByUser = customizedByUserCheckBox.Checked;
            preset.ignoreCase = ignoreCaseCheckBox.Checked;


            preset.searchedPattern = searchedPatternBox.Text;
            preset.searchedPattern2 = searchedPattern2Box.Text;
            preset.searchedPattern3 = searchedPattern3Box.Text;
            preset.searchedPattern4 = searchedPattern4Box.Text;
            preset.searchedPattern5 = searchedPattern5Box.Text;

            preset.searchedTagId = asrPlugin.getTagId(searchedTagList.Text);
            preset.searchedTag2Id = asrPlugin.getTagId(searchedTag2List.Text);
            preset.searchedTag3Id = asrPlugin.getTagId(searchedTag3List.Text);
            preset.searchedTag4Id = asrPlugin.getTagId(searchedTag4List.Text);
            preset.searchedTag5Id = asrPlugin.getTagId(searchedTag5List.Text);


            preset.replacedPattern = replacedPatternBox.Text;
            preset.replacedPattern2 = replacedPattern2Box.Text;
            preset.replacedPattern3 = replacedPattern3Box.Text;
            preset.replacedPattern4 = replacedPattern4Box.Text;
            preset.replacedPattern5 = replacedPattern5Box.Text;

            preset.replacedTagId = asrPlugin.getTagId(replacedTagList.Text);
            preset.replacedTag2Id = asrPlugin.getTagId(replacedTag2List.Text);
            preset.replacedTag3Id = asrPlugin.getTagId(replacedTag3List.Text);
            preset.replacedTag4Id = asrPlugin.getTagId(replacedTag4List.Text);
            preset.replacedTag5Id = asrPlugin.getTagId(replacedTag5List.Text);


            preset.parameterTagType = parameterTagTypeList.SelectedIndex;
            preset.parameterTag2Type = parameterTag2TypeList.SelectedIndex;
            preset.parameterTag3Type = parameterTag3TypeList.SelectedIndex;
            preset.parameterTag4Type = parameterTag4TypeList.SelectedIndex;
            preset.parameterTag5Type = parameterTag5TypeList.SelectedIndex;
            preset.parameterTag6Type = parameterTag6TypeList.SelectedIndex;


            preset.parameterTagId = asrPlugin.getTagId(parameterTagList.Text);
            preset.parameterTag2Id = asrPlugin.getTagId(parameterTag2List.Text);
            preset.parameterTag3Id = asrPlugin.getTagId(parameterTag3List.Text);
            preset.parameterTag4Id = asrPlugin.getTagId(parameterTag4List.Text);
            preset.parameterTag5Id = asrPlugin.getTagId(parameterTag5List.Text);
            preset.parameterTag6Id = asrPlugin.getTagId(parameterTag6List.Text);


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
            string prevName = "";
            string prevDescription = "";

            if (currentLanguage != "")
            {
                prevName = nameBox.Text;
                prevDescription = descriptionBox.Text;

                AdvancedSearchAndReplaceCommand.SetDictValue(preset.names, currentLanguage, prevName);
                AdvancedSearchAndReplaceCommand.SetDictValue(preset.descriptions, currentLanguage, prevDescription);
            }


            currentLanguage = languages.Text;
            nameBox.Text = AdvancedSearchAndReplaceCommand.GetDictValue(preset.names, languages.Text);
            descriptionBox.Text = AdvancedSearchAndReplaceCommand.GetDictValue(preset.descriptions, languages.Text);

            if (nameBox.Text == "")
                nameBox.Text = prevName;

            if (descriptionBox.Text == "")
                descriptionBox.Text = prevDescription;
        }

        private void fillTagList(ComboBox tagList, bool tagType, int parameterType, int parameter2Type, int parameter3Type, int parameter4Type, int parameter5Type, int parameter6Type)
        {
            string tagName = tagList.Text;

            tagList.Items.Clear();

            tagList.Items.Add("<" + Plugin.TempTagName + " 1>");
            tagList.Items.Add("<" + Plugin.TempTagName + " 2>");
            tagList.Items.Add("<" + Plugin.TempTagName + " 3>");
            tagList.Items.Add("<" + Plugin.TempTagName + " 4>");

            Plugin.FillList(tagList.Items, tagType); // tagType: true - read only, false - writable
            if (tagType)
                Plugin.FillListWithProps(tagList.Items);

            tagList.Items.Add(Plugin.ClipboardTagName);

            if (parameterType == 1 || (parameterType == 2 && tagType))
                tagList.Items.Add("<" + Plugin.ParameterTagName + " 1>");

            if (parameter2Type == 1 || (parameter2Type == 2 && tagType))
                tagList.Items.Add("<" + Plugin.ParameterTagName + " 2>");

            if (parameter3Type == 1 || (parameter3Type == 2 && tagType))
                tagList.Items.Add("<" + Plugin.ParameterTagName + " 3>");

            if (parameter4Type == 1 || (parameter4Type == 2 && tagType))
                tagList.Items.Add("<" + Plugin.ParameterTagName + " 4>");

            if (parameter5Type == 1 || (parameter5Type == 2 && tagType))
                tagList.Items.Add("<" + Plugin.ParameterTagName + " 5>");

            if (parameter6Type == 1 || (parameter6Type == 2 && tagType))
                tagList.Items.Add("<" + Plugin.ParameterTagName + " 6>");

            tagList.Text = tagName;

            if (tagList.Items.Contains(tagName))
                tagList.Text = tagName;
            else
                tagList.SelectedIndex = 0;
        }

        private void parameterTagTypeChanged(ComboBox parameterTagTypeListParam, ComboBox parameterTagListParam, Label label, ComboBox parameterTagTypeListParam2 = null)
        {
            string parameterTagName = parameterTagListParam.Text;

            asrPlugin.fillParameterTagList(parameterTagTypeListParam.SelectedIndex, parameterTagListParam, label);

            if (parameterTagListParam.Items.Contains(parameterTagName))
                parameterTagListParam.Text = parameterTagName;
            else if (parameterTagListParam.Items.Count > 0)
                parameterTagListParam.SelectedIndex = 0;
            else
                parameterTagListParam.SelectedIndex = -1;


            if (parameterTagTypeListParam2 != null)
            {
                if (parameterTagTypeListParam.SelectedIndex == 0) //Not used
                {
                    parameterTagTypeListParam2.SelectedIndex = 0;
                    parameterTagTypeListParam2.Enabled = false;
                }
                else
                {
                    parameterTagTypeListParam2.Enabled = true;
                }
            }


            fillTagList(searchedTagList, true, parameterTagTypeList.SelectedIndex, parameterTag2TypeList.SelectedIndex, parameterTag3TypeList.SelectedIndex, parameterTag4TypeList.SelectedIndex, parameterTag5TypeList.SelectedIndex, parameterTag6TypeList.SelectedIndex);
            fillTagList(searchedTag2List, true, parameterTagTypeList.SelectedIndex, parameterTag2TypeList.SelectedIndex, parameterTag3TypeList.SelectedIndex, parameterTag4TypeList.SelectedIndex, parameterTag5TypeList.SelectedIndex, parameterTag6TypeList.SelectedIndex);
            fillTagList(searchedTag3List, true, parameterTagTypeList.SelectedIndex, parameterTag2TypeList.SelectedIndex, parameterTag3TypeList.SelectedIndex, parameterTag4TypeList.SelectedIndex, parameterTag5TypeList.SelectedIndex, parameterTag6TypeList.SelectedIndex);
            fillTagList(searchedTag4List, true, parameterTagTypeList.SelectedIndex, parameterTag2TypeList.SelectedIndex, parameterTag3TypeList.SelectedIndex, parameterTag4TypeList.SelectedIndex, parameterTag5TypeList.SelectedIndex, parameterTag6TypeList.SelectedIndex);
            fillTagList(searchedTag5List, true, parameterTagTypeList.SelectedIndex, parameterTag2TypeList.SelectedIndex, parameterTag3TypeList.SelectedIndex, parameterTag4TypeList.SelectedIndex, parameterTag5TypeList.SelectedIndex, parameterTag6TypeList.SelectedIndex);

            fillTagList(replacedTagList, false, parameterTagTypeList.SelectedIndex, parameterTag2TypeList.SelectedIndex, parameterTag3TypeList.SelectedIndex, parameterTag4TypeList.SelectedIndex, parameterTag5TypeList.SelectedIndex, parameterTag6TypeList.SelectedIndex);
            fillTagList(replacedTag2List, false, parameterTagTypeList.SelectedIndex, parameterTag2TypeList.SelectedIndex, parameterTag3TypeList.SelectedIndex, parameterTag4TypeList.SelectedIndex, parameterTag5TypeList.SelectedIndex, parameterTag6TypeList.SelectedIndex);
            fillTagList(replacedTag3List, false, parameterTagTypeList.SelectedIndex, parameterTag2TypeList.SelectedIndex, parameterTag3TypeList.SelectedIndex, parameterTag4TypeList.SelectedIndex, parameterTag5TypeList.SelectedIndex, parameterTag6TypeList.SelectedIndex);
            fillTagList(replacedTag4List, false, parameterTagTypeList.SelectedIndex, parameterTag2TypeList.SelectedIndex, parameterTag3TypeList.SelectedIndex, parameterTag4TypeList.SelectedIndex, parameterTag5TypeList.SelectedIndex, parameterTag6TypeList.SelectedIndex);
            fillTagList(replacedTag5List, false, parameterTagTypeList.SelectedIndex, parameterTag2TypeList.SelectedIndex, parameterTag3TypeList.SelectedIndex, parameterTag4TypeList.SelectedIndex, parameterTag5TypeList.SelectedIndex, parameterTag6TypeList.SelectedIndex);
        }

        private void parameterTagTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            parameterTagTypeChanged(parameterTagTypeList, parameterTagList, label21, parameterTag2TypeList);
        }

        private void parameterTag2TypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            parameterTagTypeChanged(parameterTag2TypeList, parameterTag2List, label22, parameterTag3TypeList);
        }

        private void parameterTag3TypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            parameterTagTypeChanged(parameterTag3TypeList, parameterTag3List, label23, parameterTag4TypeList);
        }

        private void parameterTag4TypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            parameterTagTypeChanged(parameterTag4TypeList, parameterTag4List, label24, parameterTag5TypeList);
        }

        private void parameterTag5TypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            parameterTagTypeChanged(parameterTag5TypeList, parameterTag5List, label25, parameterTag6TypeList);
        }

        private void parameterTag6TypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            parameterTagTypeChanged(parameterTag6TypeList, parameterTag6List, label20);
        }

        private void customTextChecked_CheckedChanged(object sender, EventArgs e)
        {
            customTextBox.Enabled = customTextCheckBox.Checked;
            if (!customTextCheckBox.Checked)
            {
                customTextBox.Text = "";
                customText2CheckBox.Checked = false;
            }
            customText2CheckBox.Enabled = customTextCheckBox.Checked;
        }

        private void customText2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            customText2Box.Enabled = customText2CheckBox.Checked;
            if (!customText2CheckBox.Checked)
            {
                customText2Box.Text = "";
                customText3CheckBox.Checked = false;
            }
            customText3CheckBox.Enabled = customText2CheckBox.Checked;
        }

        private void customText3CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            customText3Box.Enabled = customText3CheckBox.Checked;
            if (!customText3CheckBox.Checked)
            {
                customText3Box.Text = "";
                customText4CheckBox.Checked = false;
            }
            customText4CheckBox.Enabled = customText3CheckBox.Checked;
        }

        private void customText4CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            customText4Box.Enabled = customText4CheckBox.Checked;
            if (!customText4CheckBox.Checked)
            {
                customText4Box.Text = "";
            }
        }
    }
}
