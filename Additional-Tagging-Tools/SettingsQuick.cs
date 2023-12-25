﻿using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class PluginQuickSettings : PluginWindowTemplate
    {
        private bool selectedLineColors;
        private string changedLegendText;
        private string selectedChangedLegendText;

        protected void setCloseShowWindowsRadioButtons(int pos)
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

        protected int getCloseShowWindowsRadioButtons()
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

        public PluginQuickSettings(Plugin TagToolsPluginParam) : base(TagToolsPluginParam)
        {
            InitializeComponent();
        }

        protected override void initializeForm()
        {
            base.initializeForm();

            versionLabel.Text = PluginVersion;

            selectedChangedLegendText = Regex.Replace(toolTip1.GetToolTip(changedLegendTextBox), @"^(.*)\:(.*)", "$2");
            changedLegendText = Regex.Replace(toolTip1.GetToolTip(changedLegendTextBox), @"^(.*)\:(.*)", "$1");

            selectedLineColors = true;
            reSkinLegend();

            allowAsrLrPresetAutoexecutionCheckBox.Checked = SavedSettings.allowAsrLrPresetAutoexecution;
            allowCommandExecutionWithoutPreviewCheckBox.Checked = SavedSettings.allowCommandExecutionWithoutPreview;

            minimizePluginWindowsCheckBox.Checked = SavedSettings.minimizePluginWindows;

            useMusicBeeFontSkinColorsCheckBox.Checked = SavedSettings.useMusicBeeFontSkinColors;
            highlightChangedTagsCheckBox.Checked = !SavedSettings.dontHighlightChangedTags;

            includeNotChangedTagsCheckBox.Checked = !SavedSettings.dontIncludeInPreviewLinesWithoutChangedTags;
            includePreservedTagsCheckBox.Checked = !SavedSettings.dontIncludeInPreviewLinesWithPreservedTagsAsr;
            includePreservedTagValuesCheckBox.Checked = !SavedSettings.dontIncludeInPreviewLinesWithPreservedTagValuesAsr;

            setCloseShowWindowsRadioButtons(SavedSettings.closeShowHiddenWindows);

            playCompletedSoundCheckBox.Checked = !SavedSettings.dontPlayCompletedSound;
            playStartedSoundCheckBox.Checked = SavedSettings.playStartedSound;
            playStoppedSoundCheckBox.Checked = SavedSettings.playCanceledSound;
            playTickedAsrPresetSoundCheckBox.Checked = !SavedSettings.dontPlayTickedAutoApplyingAsrLrPresetSound;
        }

        private void saveSettings()
        {
            bool previousUseSkinColors = SavedSettings.useMusicBeeFontSkinColors;

            SavedSettings.allowAsrLrPresetAutoexecution = allowAsrLrPresetAutoexecutionCheckBox.Checked;
            SavedSettings.allowCommandExecutionWithoutPreview = allowCommandExecutionWithoutPreviewCheckBox.Checked;

            SavedSettings.minimizePluginWindows = minimizePluginWindowsCheckBox.Checked;

            SavedSettings.useMusicBeeFontSkinColors = useMusicBeeFontSkinColorsCheckBox.Checked;
            SavedSettings.dontHighlightChangedTags = !highlightChangedTagsCheckBox.Checked;

            SavedSettings.dontIncludeInPreviewLinesWithoutChangedTags = !includeNotChangedTagsCheckBox.Checked;
            SavedSettings.dontIncludeInPreviewLinesWithPreservedTagsAsr = !includePreservedTagsCheckBox.Checked;
            SavedSettings.dontIncludeInPreviewLinesWithPreservedTagValuesAsr = !includePreservedTagValuesCheckBox.Checked;

            SavedSettings.closeShowHiddenWindows = getCloseShowWindowsRadioButtons();

            SavedSettings.dontPlayCompletedSound = !playCompletedSoundCheckBox.Checked;
            SavedSettings.playStartedSound = playStartedSoundCheckBox.Checked;
            SavedSettings.playCanceledSound = playStoppedSoundCheckBox.Checked;
            SavedSettings.dontPlayTickedAutoApplyingAsrLrPresetSound = !playTickedAsrPresetSoundCheckBox.Checked;

            TagToolsPlugin.SaveSettings();

            TagToolsPlugin.addPluginMenuItems();
            TagToolsPlugin.addPluginContextMenuItems();

            if (previousUseSkinColors != SavedSettings.useMusicBeeFontSkinColors)
            {
                //Let's dispose all unused themed bitmaps
                FormsThemedBitmapsRelease(EmptyForm);

                //Let's recreate themed bitmaps using new colors
                TagToolsPlugin.prepareThemedBitmapsAndColors();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            saveSettings();
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveLastSkippedButton_Click(object sender, EventArgs e)
        {
            SaveLastSkippedCommand tagToolsForm = new SaveLastSkippedCommand(TagToolsPlugin);
            Display(tagToolsForm, true);
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

        private void useMusicBeeFontSkinColorsCheckBoxLabel_Click(object sender, EventArgs e)
        {
            useMusicBeeFontSkinColorsCheckBox.Checked = !useMusicBeeFontSkinColorsCheckBox.Checked;
        }

        private void highlightChangedTagsCheckBoxLabel_Click(object sender, EventArgs e)
        {
            highlightChangedTagsCheckBox.Checked = !highlightChangedTagsCheckBox.Checked;
        }

        private void minimizePluginWindowsCheckBoxLabel_Click(object sender, EventArgs e)
        {
            minimizePluginWindowsCheckBox.Checked = !minimizePluginWindowsCheckBox.Checked;
        }

        private void closeHiddenCommandWindowsRadioButtonLabel_Click(object sender, EventArgs e)
        {
            closeHiddenCommandWindowsRadioButton.Checked = true;
        }

        private void showHiddenCommandWindowsRadioButtonLabel_Click(object sender, EventArgs e)
        {
            showHiddenCommandWindowsRadioButton.Checked = true;
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

        private void allowAsrLrPresetAutoexecutionCheckBoxLabel_Click(object sender, EventArgs e)
        {
            allowAsrLrPresetAutoexecutionCheckBox.Checked = !allowAsrLrPresetAutoexecutionCheckBox.Checked;
        }
    }
}
