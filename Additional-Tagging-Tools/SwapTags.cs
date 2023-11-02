using System;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;
using ExtensionMethods;

namespace MusicBeePlugin
{
    public partial class SwapTagsCommand : PluginWindowTemplate
    {
        private MetaDataType sourceTagId;
        private MetaDataType destinationTagId;
        private string[] files = new string[0];

        public SwapTagsCommand(Plugin tagToolsPluginParam) : base(tagToolsPluginParam)
        {
            InitializeComponent();
            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            FillListByTagNames(sourceTagList.Items);
            sourceTagList.Text = SavedSettings.swapTagsSourceTagName;

            FillListByTagNames(destinationTagList.Items);
            destinationTagList.Text = SavedSettings.swapTagsDestinationTagName;

            smartOperationCheckBox.Checked = SavedSettings.smartOperation;
        }

        private bool prepareBackgroundTask()
        {
            if (backgroundTaskIsWorking())
                return true;

            sourceTagId = GetTagId(sourceTagList.Text);
            destinationTagId = GetTagId(destinationTagList.Text);

            files = null;
            if (!MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files))
                files = new string[0];

            if (files.Length == 0)
            {
                MessageBox.Show(this, MsgNoFilesSelected, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            SwappedTags swappedTags;

            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                currentFile = files[fileCounter];

                SetStatusbarTextForFileOperations(SwapTagsCommandSbText, false, fileCounter, files.Length, currentFile);

                sourceTagValue = GetFileTag(currentFile, sourceTagId);
                destinationTagValue = GetFileTag(currentFile, destinationTagId);

                swappedTags = SwapTags(sourceTagValue, destinationTagValue, sourceTagId, destinationTagId, smartOperationCheckBox.Checked);

                if (sourceTagId != destinationTagId)
                    SetFileTag(currentFile, destinationTagId, swappedTags.newDestinationTagValue);

                SetFileTag(currentFile, sourceTagId, swappedTags.newSourceTagValue);
                CommitTagsToFile(currentFile);
            }

            RefreshPanels(true);

            SetResultingSbText();
        }

        private void saveSettings()
        {
            SavedSettings.swapTagsSourceTagName = sourceTagList.Text;
            SavedSettings.swapTagsDestinationTagName = destinationTagList.Text;
            SavedSettings.smartOperation = smartOperationCheckBox.Checked;

            TagToolsPlugin.SaveSettings();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (sourceTagList.Text == destinationTagList.Text)
                if (!smartOperationCheckBox.Checked || !(GetTagId(sourceTagList.Text) == ArtistArtistsId || GetTagId(sourceTagList.Text) == ComposerComposersId))
                {
                    MessageBox.Show(this, MsgSwapTagsSourceAndDestinationTagsAreTheSame, 
                        null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

            saveSettings();
            if (prepareBackgroundTask())
                switchOperation(swapTags, (Button)sender, buttonOK, EmptyButton, buttonCancel, true, null);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        public override void enableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonOK, " ");
            dirtyErrorProvider.SetError(buttonOK, string.Empty);
        }

        public override void enableQueryingOrUpdatingButtons()
        {
            buttonOK.Enable(true);
        }

        public override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enable(false);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            smartOperationCheckBox.Checked = !smartOperationCheckBox.Checked;
        }
    }
}
