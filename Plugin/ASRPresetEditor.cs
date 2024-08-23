using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using ExtensionMethods;

using MusicBeePlugin.Properties;

using static MusicBeePlugin.AdvancedSearchAndReplace;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class AsrPresetEditor : PluginWindowTemplate
    {
        private CustomComboBox replacedTag5ListCustom;
        private CustomComboBox searchedTag5ListCustom;
        private CustomComboBox replacedTag4ListCustom;
        private CustomComboBox searchedTag4ListCustom;
        private CustomComboBox replacedTag3ListCustom;
        private CustomComboBox searchedTag3ListCustom;
        private CustomComboBox replacedTag2ListCustom;
        private CustomComboBox searchedTag2ListCustom;
        private CustomComboBox replacedTagListCustom;
        private CustomComboBox searchedTagListCustom;

        private CustomComboBox parameterTag6TypeListCustom;
        private CustomComboBox parameterTag6ListCustom;
        private CustomComboBox parameterTag5TypeListCustom;
        private CustomComboBox parameterTag5ListCustom;
        private CustomComboBox parameterTag4TypeListCustom;
        private CustomComboBox parameterTag4ListCustom;
        private CustomComboBox parameterTag3TypeListCustom;
        private CustomComboBox parameterTag3ListCustom;
        private CustomComboBox parameterTag2TypeListCustom;
        private CustomComboBox parameterTag2ListCustom;
        private CustomComboBox parameterTagTypeListCustom;
        private CustomComboBox parameterTagListCustom;

        private CustomComboBox languagesCustom;


        private Preset preset;
        private bool readOnly;
        private bool settingsSaved;
        private string currentLanguage;

        private bool fillTagComboBoxes;
        private string FormTitleBarText = null;
        private int SearchedReplacedTagsFillingStepNumber = 0;

        private static string WritableAllTagsLocalizedItem;
        private Bitmap warning;

        internal AsrPresetEditor(Plugin plugin) : base(plugin)
        {
            InitializeComponent();

            new ControlBorder(this.nameBox);
            new ControlBorder(this.descriptionBox);
            new ControlBorder(this.customTextBox);
            new ControlBorder(this.customText2Box);
            new ControlBorder(this.customText3Box);
            new ControlBorder(this.customText4Box);
            new ControlBorder(this.searchedPatternBox);
            new ControlBorder(this.replacedPatternBox);
            new ControlBorder(this.searchedPattern2Box);
            new ControlBorder(this.replacedPattern2Box);
            new ControlBorder(this.searchedPattern3Box);
            new ControlBorder(this.replacedPattern3Box);
            new ControlBorder(this.searchedPattern4Box);
            new ControlBorder(this.replacedPattern4Box);
            new ControlBorder(this.searchedPattern5Box);
            new ControlBorder(this.replacedPattern5Box);
        }

        protected override void initializeForm()
        {
            base.initializeForm();
            Enable(false, null);


            replacedTag5ListCustom = namesComboBoxes["replacedTag5List"];
            searchedTag5ListCustom = namesComboBoxes["searchedTag5List"];
            replacedTag4ListCustom = namesComboBoxes["replacedTag4List"];
            searchedTag4ListCustom = namesComboBoxes["searchedTag4List"];
            replacedTag3ListCustom = namesComboBoxes["replacedTag3List"];
            searchedTag3ListCustom = namesComboBoxes["searchedTag3List"];
            replacedTag2ListCustom = namesComboBoxes["replacedTag2List"];
            searchedTag2ListCustom = namesComboBoxes["searchedTag2List"];
            replacedTagListCustom = namesComboBoxes["replacedTagList"];
            searchedTagListCustom = namesComboBoxes["searchedTagList"];

            parameterTag6TypeListCustom = namesComboBoxes["parameterTag6TypeList"];
            parameterTag6ListCustom = namesComboBoxes["parameterTag6List"];
            parameterTag5TypeListCustom = namesComboBoxes["parameterTag5TypeList"];
            parameterTag5ListCustom = namesComboBoxes["parameterTag5List"];
            parameterTag4TypeListCustom = namesComboBoxes["parameterTag4TypeList"];
            parameterTag4ListCustom = namesComboBoxes["parameterTag4List"];
            parameterTag3TypeListCustom = namesComboBoxes["parameterTag3TypeList"];
            parameterTag3ListCustom = namesComboBoxes["parameterTag3List"];
            parameterTag2TypeListCustom = namesComboBoxes["parameterTag2TypeList"];
            parameterTag2ListCustom = namesComboBoxes["parameterTag2List"];
            parameterTagTypeListCustom = namesComboBoxes["parameterTagTypeList"];
            parameterTagListCustom = namesComboBoxes["parameterTagList"];

            languagesCustom = namesComboBoxes["languages"];


            WritableAllTagsLocalizedItem = parameterTagTypeListCustom.Items[3].ToString();

            warning = ReplaceBitmap(null, Warning);

            if (readOnly)
            {
                this.MakeReadonly(true);

                buttonClose.Enable(true);
                buttonOK.Enable(true);
                linkLabel1.Enable(true);
                tableLayoutPanel1.MakeReadonly(false);
                tableLayoutPanel2.MakeReadonly(false);
            }

            guidBox.Text = preset.guid.ToString();
            modifiedBox.Text = preset.modifiedUtc.ToLocalTime().ToString();
            userPresetCheckBox.Checked = preset.userPreset;
            userPresetCheckBox.Enable(DeveloperMode);
            removePresetCheckBox.Checked = preset.removePreset;
            removePresetCheckBoxLabel.Visible = DeveloperMode;
            removePresetCheckBox.Visible = DeveloperMode;
            customizedByUserCheckBox.Checked = preset.customizedByUser;
            customizedByUserCheckBox.Enable(DeveloperMode);


            currentLanguage = null;

            var englishIsAvailable = false;
            var nativeLanguageIsAvailable = false;

            foreach (var language in preset.names.Keys)
            {
                languagesCustom.Items.Add(language);

                if (language == Language)
                    nativeLanguageIsAvailable = true;

                if (language == "en")
                    englishIsAvailable = true;
            }

            if (!nativeLanguageIsAvailable)
                languagesCustom.Items.Add(Language);

            if (!englishIsAvailable)
                languagesCustom.Items.Add("en");

            if (nativeLanguageIsAvailable)
                currentLanguage = Language;
            else if (englishIsAvailable)
                currentLanguage = "en";
            else
                currentLanguage = languagesCustom.Items[0] as string;

            nameBox.Text = GetDictValue(preset.names, currentLanguage);
            descriptionBox.Text = GetDictValue(preset.descriptions, currentLanguage);

            languagesCustom.SelectedItem = currentLanguage;


            fillTagComboBoxes = false;

            parameterTagTypeListCustom.SelectedIndex = (int)preset.parameterTagTypeNew;
            parameterTag2TypeListCustom.SelectedIndex = (int)preset.parameterTag2TypeNew;
            parameterTag3TypeListCustom.SelectedIndex = (int)preset.parameterTag3TypeNew;
            parameterTag4TypeListCustom.SelectedIndex = (int)preset.parameterTag4TypeNew;
            parameterTag5TypeListCustom.SelectedIndex = (int)preset.parameterTag5TypeNew;
            parameterTag6TypeListCustom.SelectedIndex = (int)preset.parameterTag6TypeNew;


            parameterTagListCustom.Text = AsrGetTagName(preset.parameterTagId);
            parameterTag2ListCustom.Text = AsrGetTagName(preset.parameterTag2Id);
            parameterTag3ListCustom.Text = AsrGetTagName(preset.parameterTag3Id);
            parameterTag4ListCustom.Text = AsrGetTagName(preset.parameterTag4Id);
            parameterTag5ListCustom.Text = AsrGetTagName(preset.parameterTag5Id);
            parameterTag6ListCustom.Text = AsrGetTagName(preset.parameterTag6Id);

            fillTagComboBoxes = true;


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


            searchedTagListCustom.Text = AsrGetTagName(preset.searchedTagId);
            searchedTag2ListCustom.Text = AsrGetTagName(preset.searchedTag2Id);
            searchedTag3ListCustom.Text = AsrGetTagName(preset.searchedTag3Id);
            searchedTag4ListCustom.Text = AsrGetTagName(preset.searchedTag4Id);
            searchedTag5ListCustom.Text = AsrGetTagName(preset.searchedTag5Id);


            replacedTagListCustom.Text = AsrGetTagName(preset.replacedTagId);
            replacedTag2ListCustom.Text = AsrGetTagName(preset.replacedTag2Id);
            replacedTag3ListCustom.Text = AsrGetTagName(preset.replacedTag3Id);
            replacedTag4ListCustom.Text = AsrGetTagName(preset.replacedTag4Id);
            replacedTag5ListCustom.Text = AsrGetTagName(preset.replacedTag5Id);

            add1CheckBox.Checked = preset.add;
            add2CheckBox.Checked = preset.add2;
            add3CheckBox.Checked = preset.add3;
            add4CheckBox.Checked = preset.add4;
            add5CheckBox.Checked = preset.add5;

            append1CheckBox.Checked = preset.append;
            append2CheckBox.Checked = preset.append2;
            append3CheckBox.Checked = preset.append3;
            append4CheckBox.Checked = preset.append4;
            append5CheckBox.Checked = preset.append5;

            condition1SneCheckBox.Checked = preset.sneLimitation1;
            condition2SneCheckBox.Checked = preset.sneLimitation2;
            condition3SneCheckBox.Checked = preset.sneLimitation3;
            condition4SneCheckBox.Checked = preset.sneLimitation4;
            condition5SneCheckBox.Checked = preset.sneLimitation5;

            condition1DneCheckBox.Checked = preset.dneLimitation1;
            condition2DneCheckBox.Checked = preset.dneLimitation2;
            condition3DneCheckBox.Checked = preset.dneLimitation3;
            condition4DneCheckBox.Checked = preset.dneLimitation4;
            condition5DneCheckBox.Checked = preset.dneLimitation5;

            languagesCustom.Text = Language;

            if (readOnly)
            {
                languagesCustom.Enable(true);
                parameterTagTypeListCustom.Enable(false);
                parameterTag2TypeListCustom.Enable(false);
                parameterTag3TypeListCustom.Enable(false);
                parameterTag4TypeListCustom.Enable(false);
                parameterTag5TypeListCustom.Enable(false);
                parameterTag6TypeListCustom.Enable(false);

                customTextCheckBox.Enable(false);
                customText2CheckBox.Enable(false);
                customText3CheckBox.Enable(false);
                customText4CheckBox.Enable(false);

                customTextBox.ReadOnly = !customTextCheckBox.Checked;
                customText2Box.ReadOnly = !customText2CheckBox.Checked;
                customText3Box.ReadOnly = !customText3CheckBox.Checked;
                customText4Box.ReadOnly = !customText4CheckBox.Checked;
            }

            descriptionBox.TextChanged += descriptionBox_TextChanged;


            if (useSkinColors)
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

        internal bool editPreset(Preset preset, bool openAsReadOnly)
        {
            this.preset = preset;
            readOnly = openAsReadOnly;

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

            preset.searchedTagId = AsrGetTagId(searchedTagListCustom.Text);
            preset.searchedTag2Id = AsrGetTagId(searchedTag2ListCustom.Text);
            preset.searchedTag3Id = AsrGetTagId(searchedTag3ListCustom.Text);
            preset.searchedTag4Id = AsrGetTagId(searchedTag4ListCustom.Text);
            preset.searchedTag5Id = AsrGetTagId(searchedTag5ListCustom.Text);


            preset.replacedPattern = replacedPatternBox.Text;
            preset.replacedPattern2 = replacedPattern2Box.Text;
            preset.replacedPattern3 = replacedPattern3Box.Text;
            preset.replacedPattern4 = replacedPattern4Box.Text;
            preset.replacedPattern5 = replacedPattern5Box.Text;

            preset.replacedTagId = AsrGetTagId(replacedTagListCustom.Text);
            preset.replacedTag2Id = AsrGetTagId(replacedTag2ListCustom.Text);
            preset.replacedTag3Id = AsrGetTagId(replacedTag3ListCustom.Text);
            preset.replacedTag4Id = AsrGetTagId(replacedTag4ListCustom.Text);
            preset.replacedTag5Id = AsrGetTagId(replacedTag5ListCustom.Text);


            preset.parameterTagTypeNew = (TagType)parameterTagTypeListCustom.SelectedIndex;
            preset.parameterTag2TypeNew = (TagType)parameterTag2TypeListCustom.SelectedIndex;
            preset.parameterTag3TypeNew = (TagType)parameterTag3TypeListCustom.SelectedIndex;
            preset.parameterTag4TypeNew = (TagType)parameterTag4TypeListCustom.SelectedIndex;
            preset.parameterTag5TypeNew = (TagType)parameterTag5TypeListCustom.SelectedIndex;
            preset.parameterTag6TypeNew = (TagType)parameterTag6TypeListCustom.SelectedIndex;


            preset.parameterTagId = AsrGetTagId(parameterTagListCustom.Text);
            preset.parameterTag2Id = AsrGetTagId(parameterTag2ListCustom.Text);
            preset.parameterTag3Id = AsrGetTagId(parameterTag3ListCustom.Text);
            preset.parameterTag4Id = AsrGetTagId(parameterTag4ListCustom.Text);
            preset.parameterTag5Id = AsrGetTagId(parameterTag5ListCustom.Text);
            preset.parameterTag6Id = AsrGetTagId(parameterTag6ListCustom.Text);


            preset.customText = customTextBox.Text;
            preset.customTextChecked = customTextCheckBox.Checked;
            preset.customText2 = customText2Box.Text;
            preset.customText2Checked = customText2CheckBox.Checked;
            preset.customText3 = customText3Box.Text;
            preset.customText3Checked = customText3CheckBox.Checked;
            preset.customText4 = customText4Box.Text;
            preset.customText4Checked = customText4CheckBox.Checked;


            preset.add = add1CheckBox.Checked;
            preset.add2 = add2CheckBox.Checked;
            preset.add3 = add3CheckBox.Checked;
            preset.add4 = add4CheckBox.Checked;
            preset.add5 = add5CheckBox.Checked;

            preset.append = append1CheckBox.Checked;
            preset.append2 = append2CheckBox.Checked;
            preset.append3 = append3CheckBox.Checked;
            preset.append4 = append4CheckBox.Checked;
            preset.append5 = append5CheckBox.Checked;

            preset.sneLimitation1 = condition1SneCheckBox.Checked;
            preset.sneLimitation2 = condition2SneCheckBox.Checked;
            preset.sneLimitation3 = condition3SneCheckBox.Checked;
            preset.sneLimitation4 = condition4SneCheckBox.Checked;
            preset.sneLimitation5 = condition5SneCheckBox.Checked;

            preset.dneLimitation1 = condition1DneCheckBox.Checked;
            preset.dneLimitation2 = condition2DneCheckBox.Checked;
            preset.dneLimitation3 = condition3DneCheckBox.Checked;
            preset.dneLimitation4 = condition4DneCheckBox.Checked;
            preset.dneLimitation5 = condition5DneCheckBox.Checked;

            return true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            settingsSaved = saveSettings();
            Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
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
            if (currentLanguage == null || currentLanguage == languagesCustom.SelectedItem as string)
                return;


            var prevName = nameBox.Text;
            var prevDescription = descriptionBox.Text;

            SetDictValue(preset.names, currentLanguage, prevName);
            SetDictValue(preset.descriptions, currentLanguage, prevDescription);


            currentLanguage = languagesCustom.SelectedItem as string;
            nameBox.Text = GetDictValue(preset.names, currentLanguage);
            descriptionBox.Text = GetDictValue(preset.descriptions, currentLanguage);

            if (string.IsNullOrEmpty(nameBox.Text))
                nameBox.Text = prevName;

            if (string.IsNullOrEmpty(descriptionBox.Text))
                descriptionBox.Text = prevDescription;
        }

        private static void FillParameterTagTypeList(TagType prevTagType, CustomComboBox tagTypeList, Label tagTypeLabel)
        {
            if (tagTypeList == null || tagTypeLabel == null)
                return;


            if (prevTagType == TagType.NotUsed)
            {
                if (tagTypeList.SelectedIndex != 0)
                    tagTypeList.SelectedIndex = 0;

                if (tagTypeList.Items.Count == 4)
                    tagTypeList.Items.RemoveAt(3);

                tagTypeLabel.Enable(false);
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

                tagTypeLabel.Enable(true);
                tagTypeList.Enable(true);
            }
        }

        private void FillTagComboBox(CustomComboBox tagList, bool isSearchTag)
        {
            var searchFlag = isSearchTag ? 1 : 0;
            var tagName = tagList.Text;

            tagList.ItemsClear();


            //Let's add <Tag #> if either filled tag list is for search or <Tag #> is writable
            if (parameterTagTypeListCustom.SelectedIndex >= (int)TagType.Writable - searchFlag)
                tagList.Items.Add("<" + ParameterTagName + " 1>");

            //Let's add <Tag #> if either filled tag list is for search or <Tag #> is writable
            if (parameterTag2TypeListCustom.SelectedIndex >= (int)TagType.Writable - searchFlag)
                tagList.Items.Add("<" + ParameterTagName + " 2>");

            //Let's add <Tag #> if either filled tag list is for search or <Tag #> is writable
            if (parameterTag3TypeListCustom.SelectedIndex >= (int)TagType.Writable - searchFlag)
                tagList.Items.Add("<" + ParameterTagName + " 3>");

            //Let's add <Tag #> if either filled tag list is for search or <Tag #> is writable
            if (parameterTag4TypeListCustom.SelectedIndex >= (int)TagType.Writable - searchFlag)
                tagList.Items.Add("<" + ParameterTagName + " 4>");

            //Let's add <Tag #> if either filled tag list is for search or <Tag #> is writable
            if (parameterTag5TypeListCustom.SelectedIndex >= (int)TagType.Writable - searchFlag)
                tagList.Items.Add("<" + ParameterTagName + " 5>");

            //Let's add <Tag #> if either filled tag list is for search or <Tag #> is writable
            if (parameterTag6TypeListCustom.SelectedIndex >= (int)TagType.Writable - searchFlag)
                tagList.Items.Add("<" + ParameterTagName + " 6>");


            tagList.Items.Add("<" + TempTagName + " 1>");
            tagList.Items.Add("<" + TempTagName + " 2>");
            tagList.Items.Add("<" + TempTagName + " 3>");
            tagList.Items.Add("<" + TempTagName + " 4>");

            if (isSearchTag) //It's read-only operation
            {
                FillListByTagNames(tagList.Items, true); //true: add read-only tags too
                FillListByPropNames(tagList.Items);
            }
            else  //It's writable operation
            {
                FillListByTagNames(tagList.Items);
            }

            tagList.Items.Add(ClipboardTagName);


            if (tagList.Items.Contains(tagName))
                tagList.Text = tagName;
            else
                tagList.SelectedIndex = 0;
        }

        private void parameterTagTypeChanged(CustomComboBox parameterTagTypeComboBox, PictureBox pictureBox, CustomComboBox parameterTagComboBox,
            Label tagLabel, CustomComboBox parameterTagTypeList2 = null, Label tagTypeLabel2 = null)
        {
            if (FormTitleBarText != null)
                this.Text = FormTitleBarText + CtlAsrPresetEditorPleaseWait + "(" + SearchedReplacedTagsFillingStepNumber++ + "/10)";

            var parameterTagName = parameterTagComboBox.Text;

            var tagType = (TagType)parameterTagTypeComboBox.SelectedIndex;

            if (tagType == TagType.WritableAllowAllTags)
                pictureBox.Image = warning;
            else
                pictureBox.Image = Resources.transparent_15;


            FillParameterTagTypeList(tagType, parameterTagTypeList2, tagTypeLabel2); //-V3080

            if (tagType == TagType.WritableAllowAllTags) //Let's disable entering <ALL TAGS> pseudo-tag directly in preset editor for safety reasons !!!
                tagType = TagType.Writable;

            FillParameterTagList(tagType, parameterTagName, parameterTagComboBox, tagLabel);


            if (fillTagComboBoxes)
            {
                FillTagComboBox(searchedTagListCustom, true);
                if (FormTitleBarText != null)
                    this.Text = FormTitleBarText + CtlAsrPresetEditorPleaseWait + "(" + SearchedReplacedTagsFillingStepNumber++ + "/10)";

                FillTagComboBox(searchedTag2ListCustom, true);
                if (FormTitleBarText != null)
                    this.Text = FormTitleBarText + CtlAsrPresetEditorPleaseWait + "(" + SearchedReplacedTagsFillingStepNumber++ + "/10)";

                FillTagComboBox(searchedTag3ListCustom, true);
                if (FormTitleBarText != null)
                    this.Text = FormTitleBarText + CtlAsrPresetEditorPleaseWait + "(" + SearchedReplacedTagsFillingStepNumber++ + "/10)";

                FillTagComboBox(searchedTag4ListCustom, true);
                if (FormTitleBarText != null)
                    this.Text = FormTitleBarText + CtlAsrPresetEditorPleaseWait + "(" + SearchedReplacedTagsFillingStepNumber++ + "/10)";

                FillTagComboBox(searchedTag5ListCustom, true);
                if (FormTitleBarText != null)
                    this.Text = FormTitleBarText + CtlAsrPresetEditorPleaseWait + "(" + SearchedReplacedTagsFillingStepNumber++ + "/10)";



                FillTagComboBox(replacedTagListCustom, false);
                if (FormTitleBarText != null)
                    this.Text = FormTitleBarText + CtlAsrPresetEditorPleaseWait + "(" + SearchedReplacedTagsFillingStepNumber++ + "/10)";

                FillTagComboBox(replacedTag2ListCustom, false);
                if (FormTitleBarText != null)
                    this.Text = FormTitleBarText + CtlAsrPresetEditorPleaseWait + "(" + SearchedReplacedTagsFillingStepNumber++ + "/10)";

                FillTagComboBox(replacedTag3ListCustom, false);
                if (FormTitleBarText != null)
                    this.Text = FormTitleBarText + CtlAsrPresetEditorPleaseWait + "(" + SearchedReplacedTagsFillingStepNumber++ + "/10)";

                FillTagComboBox(replacedTag4ListCustom, false);
                if (FormTitleBarText != null)
                    this.Text = FormTitleBarText + CtlAsrPresetEditorPleaseWait + "(" + SearchedReplacedTagsFillingStepNumber++ + "/10)";

                FillTagComboBox(replacedTag5ListCustom, false);
                if (FormTitleBarText != null)
                    this.Text = FormTitleBarText + CtlAsrPresetEditorPleaseWait + "(" + SearchedReplacedTagsFillingStepNumber++ + "/10)";

            }
        }

        private void parameterTagTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            parameterTagTypeChanged(parameterTagTypeListCustom, pictureBox17, parameterTagListCustom, label21, parameterTag2TypeListCustom, label18);
        }

        private void parameterTag2TypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            parameterTagTypeChanged(parameterTag2TypeListCustom, pictureBox18, parameterTag2ListCustom, label22, parameterTag3TypeListCustom, label19);
        }

        private void parameterTag3TypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            parameterTagTypeChanged(parameterTag3TypeListCustom, pictureBox19, parameterTag3ListCustom, label23, parameterTag4TypeListCustom, label28);
        }

        private void parameterTag4TypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            parameterTagTypeChanged(parameterTag4TypeListCustom, pictureBox28, parameterTag4ListCustom, label24, parameterTag5TypeListCustom, label27);
        }

        private void parameterTag5TypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            parameterTagTypeChanged(parameterTag5TypeListCustom, pictureBox27, parameterTag5ListCustom, label25, parameterTag6TypeListCustom, label26);
        }

        private void parameterTag6TypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            parameterTagTypeChanged(parameterTag6TypeListCustom, pictureBox26, parameterTag6ListCustom, label20);
        }

        private void customTextChecked_CheckedChanged(object sender, EventArgs e)
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
            if (!customTextCheckBox.IsEnabled())
                return;

            customTextCheckBox.Checked = !customTextCheckBox.Checked;
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
            if (!customText2CheckBox.IsEnabled())
                return;

            customText2CheckBox.Checked = !customText2CheckBox.Checked;
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
            if (!customText3CheckBox.IsEnabled())
                return;

            customText3CheckBox.Checked = !customText3CheckBox.Checked;
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
            if (!customText4CheckBox.IsEnabled())
                return;

            customText4CheckBox.Checked = !customText4CheckBox.Checked;
        }

        private void ignoreCaseCheckBoxLabel_Click(object sender, EventArgs e)
        {
            ignoreCaseCheckBox.Checked = !ignoreCaseCheckBox.Checked;
        }

        private void removePresetCheckBoxLabel_Click(object sender, EventArgs e)
        {
            removePresetCheckBox.Checked = !removePresetCheckBox.Checked;
        }

        private void customizedByUserCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!customizedByUserCheckBox.IsEnabled())
                return;

            customizedByUserCheckBox.Checked = !customizedByUserCheckBox.Checked;
        }

        private void userPresetCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!userPresetCheckBox.IsEnabled())
                return;

            userPresetCheckBox.Checked = !userPresetCheckBox.Checked;
        }

        private void descriptionBox_TextChanged(object sender, EventArgs e)
        {
            updateCustomScrollBars(descriptionBox);
            descriptionBox.ScrollToCaret();
        }

        private void AsrPresetEditor_Load(object sender, EventArgs e)
        {
            updateCustomScrollBars(descriptionBox);
        }

        private void AsrPresetEditor_Shown(object sender, EventArgs e)
        {
            FormTitleBarText = this.Text;
            this.Text = FormTitleBarText + CtlAsrPresetEditorPleaseWait + "(0/9)";

            searchedTagListCustom.Enable(false);
            replacedTagListCustom.Enable(false);
            searchedTag2ListCustom.Enable(false);
            replacedTag2ListCustom.Enable(false);
            searchedTag3ListCustom.Enable(false);
            replacedTag3ListCustom.Enable(false);
            searchedTag4ListCustom.Enable(false);
            replacedTag4ListCustom.Enable(false);
            searchedTag5ListCustom.Enable(false);
            replacedTag5ListCustom.Enable(false);


            parameterTagTypeList_SelectedIndexChanged(null, null);


            searchedTagListCustom.Enable(true);
            replacedTagListCustom.Enable(true);
            searchedTag2ListCustom.Enable(true);
            replacedTag2ListCustom.Enable(true);
            searchedTag3ListCustom.Enable(true);
            replacedTag3ListCustom.Enable(true);
            searchedTag4ListCustom.Enable(true);
            replacedTag4ListCustom.Enable(true);
            searchedTag5ListCustom.Enable(true);
            replacedTag5ListCustom.Enable(true);

            Enable(true, null);
            this.Text = FormTitleBarText;

            FormTitleBarText = null;
        }
    }
}
