using ExtensionMethods;
using System;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class SwapTags : PluginWindowTemplate
    {
        private CustomComboBox sourceTagListCustom;
        private CustomComboBox destinationTagListCustom;


        private MetaDataType sourceTagId;
        private MetaDataType destinationTagId;
        private string[] files = new string[0];

        internal SwapTags(Plugin plugin) : base(plugin)
        {
            InitializeComponent();
        }

        protected override void initializeForm()
        {
            base.initializeForm();

            sourceTagListCustom = namesComboBoxes["sourceTagList"];
            destinationTagListCustom = namesComboBoxes["destinationTagList"];


            FillListByTagNames(sourceTagListCustom.Items);
            sourceTagListCustom.Text = SavedSettings.swapTagsSourceTagName;

            FillListByTagNames(destinationTagListCustom.Items);
            destinationTagListCustom.Text = SavedSettings.swapTagsDestinationTagName;

            smartOperationCheckBox.Checked = SavedSettings.smartOperation;

            enableQueryingOrUpdatingButtons();

            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        private bool prepareBackgroundTask()
        {
            if (backgroundTaskIsWorking())
                return true;

            sourceTagId = GetTagId(sourceTagListCustom.Text);
            destinationTagId = GetTagId(destinationTagListCustom.Text);

            files = null;
            if (!MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files))
                files = new string[0];

            if (files.Length == 0)
            {
                MessageBox.Show(this, MsgNoTracksSelected, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

                SetStatusbarTextForFileOperations(SwapTagsSbText, false, fileCounter, files.Length, currentFile);

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
            SavedSettings.swapTagsSourceTagName = sourceTagListCustom.Text;
            SavedSettings.swapTagsDestinationTagName = destinationTagListCustom.Text;
            SavedSettings.smartOperation = smartOperationCheckBox.Checked;

            TagToolsPlugin.SaveSettings();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (sourceTagListCustom.Text == destinationTagListCustom.Text)
                if (!smartOperationCheckBox.Checked || !(GetTagId(sourceTagListCustom.Text) == ArtistArtistsId || GetTagId(sourceTagListCustom.Text) == ComposerComposersId))
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

        internal override void enableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonOK, " ");
            dirtyErrorProvider.SetError(buttonOK, string.Empty);
        }

        internal override void disableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonOK, getBackgroundTasksWarning());
        }

        internal override void enableQueryingOrUpdatingButtons()
        {
            buttonOK.Enable(true);
        }

        internal override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enable(false);
        }

        private void smartOperationCheckBoxLabel_Click(object sender, EventArgs e)
        {
            smartOperationCheckBox.Checked = !smartOperationCheckBox.Checked;
        }
    }
}
