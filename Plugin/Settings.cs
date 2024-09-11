using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using ExtensionMethods;

using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class Settings : PluginWindowTemplate
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

        public Settings(Plugin plugin) : base(plugin)
        {
            InitializeComponent();

            new ControlBorder(this.unitKBox);
            new ControlBorder(this.unitMBox);
            new ControlBorder(this.unitGBox);
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

            contextMenuCheckBox.Checked = !SavedSettings.dontShowContextMenu;

            showCopyTagCheckBox.Checked = !SavedSettings.dontShowCopyTag;
            showSwapTagsCheckBox.Checked = !SavedSettings.dontShowSwapTags;
            showChangeCaseCheckBox.Checked = !SavedSettings.dontShowChangeCase;
            showReEncodeTagCheckBox.Checked = !SavedSettings.dontShowReEncodeTag;
            showLibraryReportsCheckBox.Checked = !SavedSettings.dontShowLibraryReports;
            showAutoRateCheckBox.Checked = !SavedSettings.dontShowAutoRate;
            showAsrCheckBox.Checked = !SavedSettings.dontShowAsr;
            showCARCheckBox.Checked = !SavedSettings.dontShowCAR;
            showCTCheckBox.Checked = !SavedSettings.dontShowCT;
            showShowHiddenWindowsCheckBox.Checked = !SavedSettings.dontShowShowHiddenWindows;

            showBackupRestoreCheckBox.Checked = !SavedSettings.dontShowBackupRestore;

            minimizePluginWindowsCheckBox.Checked = SavedSettings.minimizePluginWindows;

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

            unitKBox.Text = SavedSettings.unitK;
            unitMBox.Text = SavedSettings.unitM;
            unitGBox.Text = SavedSettings.unitG;
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

            SavedSettings.dontShowContextMenu = !contextMenuCheckBox.Checked;

            SavedSettings.dontShowCopyTag = !showCopyTagCheckBox.Checked;
            SavedSettings.dontShowSwapTags = !showSwapTagsCheckBox.Checked;
            SavedSettings.dontShowChangeCase = !showChangeCaseCheckBox.Checked;
            SavedSettings.dontShowReEncodeTag = !showReEncodeTagCheckBox.Checked;
            SavedSettings.dontShowLibraryReports = !showLibraryReportsCheckBox.Checked;
            SavedSettings.dontShowAutoRate = !showAutoRateCheckBox.Checked;
            SavedSettings.dontShowAsr = !showAsrCheckBox.Checked;
            SavedSettings.dontShowCAR = !showCARCheckBox.Checked;
            SavedSettings.dontShowCT = !showCTCheckBox.Checked;
            SavedSettings.dontShowShowHiddenWindows = !showShowHiddenWindowsCheckBox.Checked;

            SavedSettings.dontShowBackupRestore = !showBackupRestoreCheckBox.Checked;

            SavedSettings.minimizePluginWindows = minimizePluginWindowsCheckBox.Checked;

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

            SavedSettings.unitK = unitKBox.Text;
            SavedSettings.unitM = unitMBox.Text;
            SavedSettings.unitG = unitGBox.Text;

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

        private void showCopyTagCheckBoxLabel_Click(object sender, EventArgs e)
        {
            showCopyTagCheckBox.Checked = !showCopyTagCheckBox.Checked;
        }

        private void showSwapTagsCheckBoxLabel_Click(object sender, EventArgs e)
        {
            showSwapTagsCheckBox.Checked = !showSwapTagsCheckBox.Checked;
        }

        private void showChangeCaseCheckBoxLabel_Click(object sender, EventArgs e)
        {
            showChangeCaseCheckBox.Checked = !showChangeCaseCheckBox.Checked;
        }

        private void showReEncodeTagCheckBoxLabel_Click(object sender, EventArgs e)
        {
            showReEncodeTagCheckBox.Checked = !showReEncodeTagCheckBox.Checked;
        }

        private void showLibraryReportsCheckBoxLabel_Click(object sender, EventArgs e)
        {
            showLibraryReportsCheckBox.Checked = !showLibraryReportsCheckBox.Checked;
        }

        private void showAutoRateCheckBoxLabel_Click(object sender, EventArgs e)
        {
            showAutoRateCheckBox.Checked = !showAutoRateCheckBox.Checked;
        }

        private void showAsrCheckBoxLabel_Click(object sender, EventArgs e)
        {
            showAsrCheckBox.Checked = !showAsrCheckBox.Checked;
        }

        private void showCARCheckBoxLabel_Click(object sender, EventArgs e)
        {
            showCARCheckBox.Checked = !showCARCheckBox.Checked;
        }

        private void showCTCheckBoxLabel_Click(object sender, EventArgs e)
        {
            showCTCheckBox.Checked = !showCTCheckBox.Checked;
        }

        private void showShowHiddenWindowsCheckBoxLabel_Click(object sender, EventArgs e)
        {
            showShowHiddenWindowsCheckBox.Checked = !showShowHiddenWindowsCheckBox.Checked;
        }

        private void showBackupRestoreCheckBoxLabel_Click(object sender, EventArgs e)
        {
            showBackupRestoreCheckBox.Checked = !showBackupRestoreCheckBox.Checked;
        }

        private void contextMenuCheckBoxLabel_Click(object sender, EventArgs e)
        {
            contextMenuCheckBox.Checked = !contextMenuCheckBox.Checked;
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
            useMusicBeeFontCheckBox_CheckedChanged(null, null);
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
            useCustomFontCheckBox_CheckedChanged(null, null);
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

        private void allowAsrLrPresetAutoExecutionCheckBoxLabel_Click(object sender, EventArgs e)
        {
            allowAsrLrPresetAutoExecutionCheckBox.Checked = !allowAsrLrPresetAutoExecutionCheckBox.Checked;
        }
    }
}
