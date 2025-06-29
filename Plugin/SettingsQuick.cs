﻿using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using ExtensionMethods;

using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class QuickSettings : PluginWindowTemplate
    {
        private bool selectedLineColors;
        private string changedLegendText;
        private string selectedChangedLegendText;
        private System.Drawing.Font customFont;

        internal protected void setCloseShowWindowsRadioButtons(int pos)
        {
            switch (pos)
            {
                case 1:
                    closeHiddenCommandWindowsRadioButton.Checked = true;
                    break;
                default:
                    showHiddenCommandWindowsRadioButton.Checked = true;
                    break;
            }
        }

        internal protected int getCloseShowWindowsRadioButtons()
        {
            if (closeHiddenCommandWindowsRadioButton.Checked) return 1;
            else return 2;
        }

        private void reSkinLegend()
        {
            selectedLineColors = !selectedLineColors;

            if (selectedLineColors)
            {
                toolTip1.SetToolTip(legendGroupBox, selectedChangedLegendText);

                toolTip1.SetToolTip(changedLegendTextBox, selectedChangedLegendText);
                toolTip1.SetToolTip(preservedTagsLegendTextBox, selectedChangedLegendText);
                toolTip1.SetToolTip(preservedTagValuesLegendTextBox, selectedChangedLegendText);
            }
            else
            {
                toolTip1.SetToolTip(legendGroupBox, changedLegendText);

                toolTip1.SetToolTip(changedLegendTextBox, changedLegendText);
                toolTip1.SetToolTip(preservedTagsLegendTextBox, changedLegendText);
                toolTip1.SetToolTip(preservedTagValuesLegendTextBox, changedLegendText);
            }


            changedLegendTextBox.ForeColor = selectedLineColors ? ChangedCellStyle.SelectionForeColor : ChangedCellStyle.ForeColor;
            changedLegendTextBox.BackColor = selectedLineColors ? ChangedCellStyle.SelectionBackColor : ChangedCellStyle.BackColor;

            preservedTagsLegendTextBox.ForeColor = selectedLineColors ? PreservedTagCellStyle.SelectionForeColor : PreservedTagCellStyle.ForeColor;
            preservedTagsLegendTextBox.BackColor = selectedLineColors ? PreservedTagCellStyle.SelectionBackColor : PreservedTagCellStyle.BackColor;

            preservedTagValuesLegendTextBox.ForeColor = selectedLineColors ? PreservedTagValueCellStyle.SelectionForeColor : PreservedTagValueCellStyle.ForeColor;
            preservedTagValuesLegendTextBox.BackColor = selectedLineColors ? PreservedTagValueCellStyle.SelectionBackColor : PreservedTagValueCellStyle.BackColor;
        }

        public QuickSettings(Plugin plugin) : base(plugin)
        {
            InitializeComponent();

            WindowIcon = SettingsIcon;
            TitleBarText = this.Text;

            new ControlBorder(this.customFontTextBox);
            new ControlBorder(this.preservedTagValuesLegendTextBox);
            new ControlBorder(this.preservedTagsLegendTextBox);
            new ControlBorder(this.changedLegendTextBox);
        }

        internal protected override void initializeForm()
        {
            base.initializeForm();

            versionLabel.Text = PluginVersion;

            selectedChangedLegendText = Regex.Replace(toolTip1.GetToolTip(changedLegendTextBox), @"^(.*)\:(.*)", "$2");
            changedLegendText = Regex.Replace(toolTip1.GetToolTip(changedLegendTextBox), @"^(.*)\:(.*)", "$1");

            selectedLineColors = true;
            reSkinLegend();

            allowAsrLrPresetAutoExecutionCheckBox.Checked = SavedSettings.allowAsrLrPresetAutoExecution;
            allowCommandExecutionWithoutPreviewCheckBox.Checked = SavedSettings.allowCommandExecutionWithoutPreview;

            hidePluginWindowsOnMinimizationCheckBox.Checked = SavedSettings.hidePluginWindowsOnMinimization;

            scrollPreviewToEndCheckBox.Checked = SavedSettings.scrollPreviewToEnd;

            useSkinColorsCheckBox.Checked = !SavedSettings.dontUseSkinColors;
            useMusicBeeFontCheckBox.Checked = SavedSettings.useMusicBeeFont;


            useCustomFontCheckBox.Checked = SavedSettings.useCustomFont;

            if (SavedSettings.pluginFontFamilyName == null)
            {
                customFont = MbApiInterface.Setting_GetDefaultFont().Clone() as System.Drawing.Font;
            }
            else
            {
                customFont = new System.Drawing.Font(
                    SavedSettings.pluginFontFamilyName,
                    SavedSettings.pluginFontSize,
                    SavedSettings.pluginFontStyle
                    );
            }

            customFontTextBox.Text = customFont.Name + " " + customFont.Style + " " + customFont.Size + " pt.";
            customFontTextBox.Text = customFontTextBox.Text.Replace(", ", " ").Replace("bold Bold", "bold");
            customFontButton.Enable(useCustomFontCheckBox.Checked);


            highlightChangedTagsCheckBox.Checked = !SavedSettings.dontHighlightChangedTags;

            includeNotChangedTagsCheckBox.Checked = !SavedSettings.dontIncludeInPreviewLinesWithoutChangedTags;
            includePreservedTagsCheckBox.Checked = !SavedSettings.dontIncludeInPreviewLinesWithPreservedTagsAsr;
            includePreservedTagValuesCheckBox.Checked = !SavedSettings.dontIncludeInPreviewLinesWithPreservedTagValuesAsr;

            setCloseShowWindowsRadioButtons(SavedSettings.closeShowHiddenWindows);

            playCompletedSoundCheckBox.Checked = !SavedSettings.dontPlayCompletedSound;
            playStartedSoundCheckBox.Checked = SavedSettings.playStartedSound;
            playStoppedSoundCheckBox.Checked = SavedSettings.playCanceledSound;
            playTickedAsrPresetSoundCheckBox.Checked = !SavedSettings.dontPlayTickedAutoApplyingAsrLrPresetSound;

            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        private void saveSettings()
        {
            SizesColorsChanged = false;

            if (SavedSettings.dontUseSkinColors == useSkinColorsCheckBox.Checked)
                SizesColorsChanged = true;
            else if (SavedSettings.useMusicBeeFont != useMusicBeeFontCheckBox.Checked)
                SizesColorsChanged = true;
            else if (SavedSettings.useCustomFont != useCustomFontCheckBox.Checked)
                SizesColorsChanged = true;
            else if (SavedSettings.useCustomFont && SavedSettings.pluginFontFamilyName != customFont.FontFamily.Name)
                SizesColorsChanged = true;
            else if (SavedSettings.useCustomFont && Math.Abs(SavedSettings.pluginFontSize - customFont.Size) < 0.5)
                SizesColorsChanged = true;
            else if (SavedSettings.useCustomFont && SavedSettings.pluginFontStyle != customFont.Style)
                SizesColorsChanged = true;


            SavedSettings.allowAsrLrPresetAutoExecution = allowAsrLrPresetAutoExecutionCheckBox.Checked;
            SavedSettings.allowCommandExecutionWithoutPreview = allowCommandExecutionWithoutPreviewCheckBox.Checked;

            SavedSettings.hidePluginWindowsOnMinimization = hidePluginWindowsOnMinimizationCheckBox.Checked;

            SavedSettings.scrollPreviewToEnd = scrollPreviewToEndCheckBox.Checked;

            SavedSettings.dontUseSkinColors = !useSkinColorsCheckBox.Checked;
            SavedSettings.useMusicBeeFont = useMusicBeeFontCheckBox.Checked;

            SavedSettings.useCustomFont = useCustomFontCheckBox.Checked;
            SavedSettings.pluginFontFamilyName = customFont.FontFamily.Name;
            SavedSettings.pluginFontSize = customFont.Size;
            SavedSettings.pluginFontStyle = customFont.Style;

            SavedSettings.dontHighlightChangedTags = !highlightChangedTagsCheckBox.Checked;

            SavedSettings.dontIncludeInPreviewLinesWithoutChangedTags = !includeNotChangedTagsCheckBox.Checked;
            SavedSettings.dontIncludeInPreviewLinesWithPreservedTagsAsr = !includePreservedTagsCheckBox.Checked;
            SavedSettings.dontIncludeInPreviewLinesWithPreservedTagValuesAsr = !includePreservedTagValuesCheckBox.Checked;

            SavedSettings.closeShowHiddenWindows = getCloseShowWindowsRadioButtons();

            SavedSettings.dontPlayCompletedSound = !playCompletedSoundCheckBox.Checked;
            SavedSettings.playStartedSound = playStartedSoundCheckBox.Checked;
            SavedSettings.playCanceledSound = playStoppedSoundCheckBox.Checked;
            SavedSettings.dontPlayTickedAutoApplyingAsrLrPresetSound = !playTickedAsrPresetSoundCheckBox.Checked;

            TagToolsPlugin.getButtonTextBoxDpiFontScaling();
            TagToolsPlugin.prepareThemedBitmapsAndColors();

            TagToolsPlugin.SaveSettings();


            AdvancedSearchAndReplace.InitAsr();
            LibraryReports.InitLr();
            TagToolsPlugin.InitBackupRestore();

            TagToolsPlugin.addPluginContextMenuItems();
            TagToolsPlugin.addPluginMenuItems();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            saveSettings();
            Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveLastSkippedButton_Click(object sender, EventArgs e)
        {
            var tagToolsForm = new SaveLastSkippedDate(TagToolsPlugin);
            Display(tagToolsForm, this, true);
        }

        private void changedLegendTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            reSkinLegend();
        }

        private void preservedTagsLegendTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            reSkinLegend();
        }

        private void preservedTagValuesLegendTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            reSkinLegend();
        }

        private void useSkinColorsCheckBoxLabel_Click(object sender, EventArgs e)
        {
            useSkinColorsCheckBox.Checked = !useSkinColorsCheckBox.Checked;
        }

        private void useMusicBeeFontCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (useMusicBeeFontCheckBox.Checked && useCustomFontCheckBox.Checked)
                useCustomFontCheckBox.Checked = false;
        }

        private void useMusicBeeFontCheckBoxLabel_Click(object sender, EventArgs e)
        {
            useMusicBeeFontCheckBox.Checked = !useMusicBeeFontCheckBox.Checked;
        }

        private void useCustomFontCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            customFontButton.Enable(useCustomFontCheckBox.Checked);

            if (useMusicBeeFontCheckBox.Checked && useCustomFontCheckBox.Checked)
                useMusicBeeFontCheckBox.Checked = false;
        }

        private void useCustomFontCheckBoxLabel_Click(object sender, EventArgs e)
        {
            useCustomFontCheckBox.Checked = !useCustomFontCheckBox.Checked;
        }

        private void customFontButton_Click(object sender, EventArgs e)
        {
            var customFontDialog = new FontDialog();
            customFontDialog.Font = customFont;
            customFontDialog.ShowApply = false;
            customFontDialog.FontMustExist = true;
            customFontDialog.ShowColor = false;
            customFontDialog.ShowEffects = false;
            customFontDialog.AllowSimulations = false;
            customFontDialog.AllowScriptChange = false;

            var result = customFontDialog.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                customFont.Dispose();
                customFont = customFontDialog.Font.Clone() as System.Drawing.Font;
                customFontTextBox.Text = customFont.Name + " " + customFont.Style + " " + customFont.Size + " pt.";
                customFontTextBox.Text = customFontTextBox.Text.Replace(", ", " ").Replace("bold Bold", "bold");
            }

            customFontDialog.Dispose();
        }

        private void highlightChangedTagsCheckBoxLabel_Click(object sender, EventArgs e)
        {
            highlightChangedTagsCheckBox.Checked = !highlightChangedTagsCheckBox.Checked;
        }

        private void hidePluginWindowsOnMinimizationCheckBoxLabel_Click(object sender, EventArgs e)
        {
            hidePluginWindowsOnMinimizationCheckBox.Checked = !hidePluginWindowsOnMinimizationCheckBox.Checked;
        }

        private void closeHiddenCommandWindowsRadioButtonLabel_Click(object sender, EventArgs e)
        {
            closeHiddenCommandWindowsRadioButton.Checked = true;
        }

        private void showHiddenCommandWindowsRadioButtonLabel_Click(object sender, EventArgs e)
        {
            showHiddenCommandWindowsRadioButton.Checked = true;
        }

        private void scrollPreviewToEndCheckBoxLabel_Click(object sender, EventArgs e)
        {
            scrollPreviewToEndCheckBox.Checked = !scrollPreviewToEndCheckBox.Checked;
        }

        private void playCompletedSoundCheckBoxLabel_Click(object sender, EventArgs e)
        {
            playCompletedSoundCheckBox.Checked = !playCompletedSoundCheckBox.Checked;
        }

        private void playStartedSoundCheckBoxLabel_Click(object sender, EventArgs e)
        {
            playStartedSoundCheckBox.Checked = !playStartedSoundCheckBox.Checked;
        }

        private void playStoppedSoundCheckBoxLabel_Click(object sender, EventArgs e)
        {
            playStoppedSoundCheckBox.Checked = !playStoppedSoundCheckBox.Checked;
        }

        private void playTickedAsrPresetSoundCheckBoxLabel_Click(object sender, EventArgs e)
        {
            playTickedAsrPresetSoundCheckBox.Checked = !playTickedAsrPresetSoundCheckBox.Checked;
        }

        private void includeNotChangedTagsCheckBoxLabel_Click(object sender, EventArgs e)
        {
            includeNotChangedTagsCheckBox.Checked = !includeNotChangedTagsCheckBox.Checked;
        }

        private void includePreservedTagsCheckBoxLabel_Click(object sender, EventArgs e)
        {
            includePreservedTagsCheckBox.Checked = !includePreservedTagsCheckBox.Checked;
        }

        private void includePreservedTagValuesCheckBoxLabel_Click(object sender, EventArgs e)
        {
            includePreservedTagValuesCheckBox.Checked = !includePreservedTagValuesCheckBox.Checked;
        }

        private void allowCommandExecutionWithoutPreviewCheckBoxLabel_Click(object sender, EventArgs e)
        {
            allowCommandExecutionWithoutPreviewCheckBox.Checked = !allowCommandExecutionWithoutPreviewCheckBox.Checked;
        }

        private void allowAsrLrPresetAutoExecutionCheckBoxLabel_Click(object sender, EventArgs e)
        {
            allowAsrLrPresetAutoExecutionCheckBox.Checked = !allowAsrLrPresetAutoExecutionCheckBox.Checked;
        }
    }
}
