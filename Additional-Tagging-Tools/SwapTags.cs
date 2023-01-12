using System;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class SwapTagsCommand : PluginWindowTemplate
    {
        private Plugin.MetaDataType sourceTagId;
        private Plugin.MetaDataType destinationTagId;
        private string[] files = new string[0];

        public SwapTagsCommand()
        {
            InitializeComponent();
        }

        public SwapTagsCommand(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            Plugin.FillList(sourceTagList.Items);
            sourceTagList.Text = Plugin.SavedSettings.swapTagsSourceTagName;

            Plugin.FillList(destinationTagList.Items);
            destinationTagList.Text = Plugin.SavedSettings.swapTagsDestinationTagName;

            smartOperationCheckBox.Checked = Plugin.SavedSettings.smartOperation;
        }

        private bool prepareBackgroundTask()
        {
            if (backgroundTaskIsWorking())
                return true;

            sourceTagId = Plugin.GetTagId(sourceTagList.Text);
            destinationTagId = Plugin.GetTagId(destinationTagList.Text);

            files = null;
            if (!Plugin.MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files))
                files = new string[0];

            if (files.Length == 0)
            {
                MessageBox.Show(this, Plugin.MsgNoFilesSelected, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void swapTags()
        {
            string currentFile;
            string sourceTagValue;
            string destinationTagValue;
            Plugin.SwappedTags swappedTags;

            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                currentFile = files[fileCounter];

                Plugin.SetStatusbarTextForFileOperations(Plugin.SwapTagsCommandSbText, false, fileCounter, files.Length, currentFile);

                sourceTagValue = Plugin.GetFileTag(currentFile, sourceTagId);
                destinationTagValue = Plugin.GetFileTag(currentFile, destinationTagId);

                swappedTags = Plugin.SwapTags(sourceTagValue, destinationTagValue, sourceTagId, destinationTagId, smartOperationCheckBox.Checked);

                if (sourceTagId != destinationTagId)
                    Plugin.SetFileTag(currentFile, destinationTagId, swappedTags.newDestinationTagValue);

                Plugin.SetFileTag(currentFile, sourceTagId, swappedTags.newSourceTagValue);
                Plugin.CommitTagsToFile(currentFile);
            }

            Plugin.RefreshPanels(true);

            Plugin.SetStatusbarTextForFileOperations(Plugin.SwapTagsCommandSbText, false, files.Length - 1, files.Length, null, true);
        }

        private void saveSettings()
        {
            Plugin.SavedSettings.swapTagsSourceTagName = sourceTagList.Text;
            Plugin.SavedSettings.swapTagsDestinationTagName = destinationTagList.Text;
            Plugin.SavedSettings.smartOperation = smartOperationCheckBox.Checked;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (sourceTagList.Text == destinationTagList.Text)
                if (!smartOperationCheckBox.Checked || !(Plugin.GetTagId(sourceTagList.Text) == Plugin.ArtistArtistsId || Plugin.GetTagId(sourceTagList.Text) == Plugin.ComposerComposersId))
                {
                    MessageBox.Show(this, Plugin.MsgSwapTagsSourceAndDestinationTagsAreTheSame, 
                        null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

            saveSettings();
            if (prepareBackgroundTask())
                switchOperation(swapTags, (Button)sender, buttonOK, Plugin.EmptyButton, buttonCancel, true, null);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        public override void enableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonOK, " ");
            dirtyErrorProvider.SetError(buttonOK, String.Empty);
        }

        public override void enableQueryingOrUpdatingButtons()
        {
            buttonOK.Enabled = true;
        }

        public override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enabled = false;
        }
    }
}
