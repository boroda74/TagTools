using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class PluginSettings : PluginWindowTemplate
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
                toolTip1.SetToolTip(groupBox6, selectedChangedLegendText);

                toolTip1.SetToolTip(changedLegendTextBox, selectedChangedLegendText);
                toolTip1.SetToolTip(preservedTagsLegendTextBox, selectedChangedLegendText);
                toolTip1.SetToolTip(preservedTagValuesLegendTextBox, selectedChangedLegendText);
            }
            else
            {
                toolTip1.SetToolTip(groupBox6, changedLegendText);

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

        public PluginSettings(Plugin TagToolsPluginParam) : base(TagToolsPluginParam)
        {
            InitializeComponent();
            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            versionLabel.Text = PluginVersion;

            selectedChangedLegendText = Regex.Replace(toolTip1.GetToolTip(changedLegendTextBox), @"^(.*)\:(.*)", "$2");
            changedLegendText = Regex.Replace(toolTip1.GetToolTip(changedLegendTextBox), @"^(.*)\:(.*)", "$1");

            selectedLineColors = true;
            reSkinLegend();

            contextMenuCheckBox.Checked = !SavedSettings.dontShowContextMenu;

            showCopyTagCheckBox.Checked = !SavedSettings.dontShowCopyTag;
            showSwapTagsCheckBox.Checked = !SavedSettings.dontShowSwapTags;
            showChangeCaseCheckBox.Checked = !SavedSettings.dontShowChangeCase;
            showRencodeTagCheckBox.Checked = !SavedSettings.dontShowRencodeTag;
            showLibraryReportsCheckBox.Checked = !SavedSettings.dontShowLibraryReports;
            showAutorateCheckBox.Checked = !SavedSettings.dontShowAutorate;
            showASRCheckBox.Checked = !SavedSettings.dontShowASR;
            showCARCheckBox.Checked = !SavedSettings.dontShowCAR;
            showCTCheckBox.Checked = !SavedSettings.dontShowCT;
            showShowHiddenWindowsCheckBox.Checked = !SavedSettings.dontShowShowHiddenWindows;

            showBackupRestoreCheckBox.Checked = !SavedSettings.dontShowBackupRestore;

            minimizePluginWindowsCheckBox.Checked = SavedSettings.minimizePluginWindows;

            useSkinColorsCheckBox.Checked = SavedSettings.useSkinColors;
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
            bool previousUseSkinColors = SavedSettings.useSkinColors;

            SavedSettings.dontShowContextMenu = !contextMenuCheckBox.Checked;

            SavedSettings.dontShowCopyTag = !showCopyTagCheckBox.Checked;
            SavedSettings.dontShowSwapTags = !showSwapTagsCheckBox.Checked;
            SavedSettings.dontShowChangeCase = !showChangeCaseCheckBox.Checked;
            SavedSettings.dontShowRencodeTag = !showRencodeTagCheckBox.Checked;
            SavedSettings.dontShowLibraryReports = !showLibraryReportsCheckBox.Checked;
            SavedSettings.dontShowAutorate = !showAutorateCheckBox.Checked;
            SavedSettings.dontShowASR = !showASRCheckBox.Checked;
            SavedSettings.dontShowCAR = !showCARCheckBox.Checked;
            SavedSettings.dontShowCT = !showCTCheckBox.Checked;
            SavedSettings.dontShowShowHiddenWindows = !showShowHiddenWindowsCheckBox.Checked;

            SavedSettings.dontShowBackupRestore = !showBackupRestoreCheckBox.Checked;

            SavedSettings.minimizePluginWindows = minimizePluginWindowsCheckBox.Checked;

            SavedSettings.useSkinColors = useSkinColorsCheckBox.Checked;
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

            TagToolsPlugin.SaveSettings();

            TagToolsPlugin.addPluginMenuItems();
            TagToolsPlugin.addPluginContextMenuItems();

            if (previousUseSkinColors != SavedSettings.useSkinColors)
                PrepareThemedBitmapsAndColors();
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

        private void showCopyTagCheckBoxLabel_Click(object sender, EventArgs e)
        {
            showCopyTagCheckBox.Checked = !showCopyTagCheckBox.Checked;
        }

        private void label7_Click(object sender, EventArgs e)
        {
            showSwapTagsCheckBox.Checked = !showSwapTagsCheckBox.Checked;
        }

        private void label8_Click(object sender, EventArgs e)
        {
            showChangeCaseCheckBox.Checked = !showChangeCaseCheckBox.Checked;
        }

        private void label9_Click(object sender, EventArgs e)
        {
            showRencodeTagCheckBox.Checked = !showRencodeTagCheckBox.Checked;
        }

        private void label10_Click(object sender, EventArgs e)
        {
            showLibraryReportsCheckBox.Checked = !showLibraryReportsCheckBox.Checked;
        }

        private void label11_Click(object sender, EventArgs e)
        {
            showAutorateCheckBox.Checked = !showAutorateCheckBox.Checked;
        }

        private void label12_Click(object sender, EventArgs e)
        {
            showASRCheckBox.Checked = !showASRCheckBox.Checked;
        }

        private void label13_Click(object sender, EventArgs e)
        {
            showCARCheckBox.Checked = !showCARCheckBox.Checked;
        }

        private void label14_Click(object sender, EventArgs e)
        {
            showCTCheckBox.Checked = !showCTCheckBox.Checked;
        }

        private void label15_Click(object sender, EventArgs e)
        {
            showShowHiddenWindowsCheckBox.Checked = !showShowHiddenWindowsCheckBox.Checked;
        }

        private void label16_Click(object sender, EventArgs e)
        {
            showBackupRestoreCheckBox.Checked = !showBackupRestoreCheckBox.Checked;
        }

        private void label17_Click(object sender, EventArgs e)
        {
            contextMenuCheckBox.Checked = !contextMenuCheckBox.Checked;
        }

        private void label18_Click(object sender, EventArgs e)
        {
            useSkinColorsCheckBox.Checked = !useSkinColorsCheckBox.Checked;
        }

        private void label19_Click(object sender, EventArgs e)
        {
            highlightChangedTagsCheckBox.Checked = !highlightChangedTagsCheckBox.Checked;
        }

        private void label20_Click(object sender, EventArgs e)
        {
            minimizePluginWindowsCheckBox.Checked = !minimizePluginWindowsCheckBox.Checked;
        }

        private void label21_Click(object sender, EventArgs e)
        {
            closeHiddenCommandWindowsRadioButton.Checked = true;
        }

        private void label22_Click(object sender, EventArgs e)
        {
            showHiddenCommandWindowsRadioButton.Checked = true;
        }

        private void label23_Click(object sender, EventArgs e)
        {
            playCompletedSoundCheckBox.Checked = !playCompletedSoundCheckBox.Checked;
        }

        private void label24_Click(object sender, EventArgs e)
        {
            playStartedSoundCheckBox.Checked = !playStartedSoundCheckBox.Checked;
        }

        private void label25_Click(object sender, EventArgs e)
        {
            playStoppedSoundCheckBox.Checked = !playStoppedSoundCheckBox.Checked;
        }

        private void label26_Click(object sender, EventArgs e)
        {
            playTickedAsrPresetSoundCheckBox.Checked = !playTickedAsrPresetSoundCheckBox.Checked;
        }

        private void label27_Click(object sender, EventArgs e)
        {
            includeNotChangedTagsCheckBox.Checked = !includeNotChangedTagsCheckBox.Checked;
        }

        private void label28_Click(object sender, EventArgs e)
        {
            includePreservedTagsCheckBox.Checked = !includePreservedTagsCheckBox.Checked;
        }

        private void label29_Click(object sender, EventArgs e)
        {
            includePreservedTagValuesCheckBox.Checked = !includePreservedTagValuesCheckBox.Checked;
        }
    }
}
