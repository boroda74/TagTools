using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace MusicBeePlugin
{
    public partial class PluginSettings : PluginWindowTemplate
    {
        protected Plugin.PluginInfo about;

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

        public PluginSettings()
        {
            InitializeComponent();
        }

        public PluginSettings(Plugin TagToolsPluginParam, Plugin.PluginInfo aboutParam)
        {
            InitializeComponent();

            TagToolsPlugin = TagToolsPluginParam;
            about = aboutParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            versionLabel.Text += about.VersionMajor + "." + about.VersionMinor + "." + about.Revision;

            contextMenuCheckBox.Checked = !Plugin.SavedSettings.dontShowContextMenu;

            showCopyTagCheckBox.Checked = !Plugin.SavedSettings.dontShowCopyTag;
            showSwapTagsCheckBox.Checked = !Plugin.SavedSettings.dontShowSwapTags;
            showChangeCaseCheckBox.Checked = !Plugin.SavedSettings.dontShowChangeCase;
            showRencodeTagCheckBox.Checked = !Plugin.SavedSettings.dontShowRencodeTag;
            showLibraryReportsCheckBox.Checked = !Plugin.SavedSettings.dontShowLibraryReports;
            showAutorateCheckBox.Checked = !Plugin.SavedSettings.dontShowAutorate;
            showASRCheckBox.Checked = !Plugin.SavedSettings.dontShowASR;
            showCARCheckBox.Checked = !Plugin.SavedSettings.dontShowCAR;
            showCTCheckBox.Checked = !Plugin.SavedSettings.dontShowCT;
            showShowHiddenWindowsCheckBox.Checked = !Plugin.SavedSettings.dontShowShowHiddenWindows;

            showBackupRestoreCheckBox.Checked = !Plugin.SavedSettings.dontShowBackupRestore;

            useSkinColorsCheckBox.Checked = Plugin.SavedSettings.useSkinColors;
            highlightChangedTagsCheckBox.Checked = !Plugin.SavedSettings.dontHighlightChangedTags;

            setCloseShowWindowsRadioButtons(Plugin.SavedSettings.closeShowHiddenWindows);

            playCompletedSoundCheckBox.Checked = !Plugin.SavedSettings.dontPlayCompletedSound;
            playStartedSoundCheckBox.Checked = Plugin.SavedSettings.playStartedSound;
            playStoppedSoundCheckBox.Checked = Plugin.SavedSettings.playCanceledSound;
            playTickedAsrPresetSoundCheckBox.Checked = !Plugin.SavedSettings.dontPlayTickedAsrPresetSound;

            unitKBox.Text = Plugin.SavedSettings.unitK;
            unitMBox.Text = Plugin.SavedSettings.unitM;
            unitGBox.Text = Plugin.SavedSettings.unitG;
        }

        private void saveSettings()
        {
            Plugin.SavedSettings.dontShowContextMenu = !contextMenuCheckBox.Checked;

            Plugin.SavedSettings.dontShowCopyTag = !showCopyTagCheckBox.Checked;
            Plugin.SavedSettings.dontShowSwapTags = !showSwapTagsCheckBox.Checked;
            Plugin.SavedSettings.dontShowChangeCase = !showChangeCaseCheckBox.Checked;
            Plugin.SavedSettings.dontShowRencodeTag = !showRencodeTagCheckBox.Checked;
            Plugin.SavedSettings.dontShowLibraryReports = !showLibraryReportsCheckBox.Checked;
            Plugin.SavedSettings.dontShowAutorate = !showAutorateCheckBox.Checked;
            Plugin.SavedSettings.dontShowASR = !showASRCheckBox.Checked;
            Plugin.SavedSettings.dontShowCAR = !showCARCheckBox.Checked;
            Plugin.SavedSettings.dontShowCT = !showCTCheckBox.Checked;
            Plugin.SavedSettings.dontShowShowHiddenWindows = !showShowHiddenWindowsCheckBox.Checked;

            Plugin.SavedSettings.dontShowBackupRestore = !showBackupRestoreCheckBox.Checked;

            Plugin.SavedSettings.useSkinColors = useSkinColorsCheckBox.Checked;
            Plugin.SavedSettings.dontHighlightChangedTags = !highlightChangedTagsCheckBox.Checked;
            Plugin.SavedSettings.closeShowHiddenWindows = getCloseShowWindowsRadioButtons();

            Plugin.SavedSettings.dontPlayCompletedSound = !playCompletedSoundCheckBox.Checked;
            Plugin.SavedSettings.playStartedSound = playStartedSoundCheckBox.Checked;
            Plugin.SavedSettings.playCanceledSound = playStoppedSoundCheckBox.Checked;
            Plugin.SavedSettings.dontPlayTickedAsrPresetSound = !playTickedAsrPresetSoundCheckBox.Checked;

            Plugin.SavedSettings.unitK = unitKBox.Text;
            Plugin.SavedSettings.unitM = unitMBox.Text;
            Plugin.SavedSettings.unitG = unitGBox.Text;

            TagToolsPlugin.addPluginMenuItems();
            TagToolsPlugin.addPluginContextMenuItems();
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
    }
}
