using System;
using System.Windows.Forms;

using ExtensionMethods;

using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class SwapTags : PluginWindowTemplate
    {
        private CustomComboBox sourceTagListCustom;
        private CustomComboBox destinationTagListCustom;


        private MetaDataType sourceTagId;
        private MetaDataType destinationTagId;
        private bool smartOperation;
        private string[] files = Array.Empty<string>();

        public SwapTags(Plugin plugin) : base(plugin)
        {
            InitializeComponent();

            WindowMenuIcon = SwapTagsMenuIcon;
            WindowIcon = SwapTagsIcon;
            WindowIconInactive = SwapTagsIconInactive;
            TitleBarText = this.Text;
        }

        internal protected override void initializeForm()
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
                return false;

            sourceTagId = GetTagId(sourceTagListCustom.Text);
            destinationTagId = GetTagId(destinationTagListCustom.Text);
            smartOperation = smartOperationCheckBox.Checked;

            MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files);
            if (files == null || files.Length == 0)
            {
                MessageBox.Show(this, MsgNoTracksSelected, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool applyingChangesStopped()
        {
            backgroundTaskIsScheduled = false;
            backgroundTaskIsStopping = false;
            backgroundTaskIsStoppedOrCancelled = false;
            closeFormOnStopping = false;

            SetResultingSbText(null);

            if (closeFormOnStopping)
            {
                ignoreClosingForm = false;
                Close();
            }

            ignoreClosingForm = false;

            return true;
        }

        private void swapTags()
        {
            for (var fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (checkStoppingStatus())
                {
                    Invoke(new Action(() => { stopButtonClickedMethod(applyingChangesStopped); }));
                    return;
                }


                var currentFile = files[fileCounter];

                SetStatusBarTextForFileOperations(null, SwapTagsSbText, false, fileCounter, files.Length, currentFile);

                var sourceTagValue = GetFileTag(currentFile, sourceTagId);
                var destinationTagValue = GetFileTag(currentFile, destinationTagId);

                var swappedTags = SwapTags(sourceTagValue, destinationTagValue, sourceTagId, destinationTagId, smartOperation);

                if (sourceTagId != destinationTagId)
                    SetFileTag(currentFile, destinationTagId, swappedTags.newDestinationNormalizedTagValue);

                SetFileTag(currentFile, sourceTagId, swappedTags.newSourceNormalizedTagValue);
                CommitTagsToFile(currentFile);
            }

            Invoke(new Action(() => { applyingChangesStopped(); }));

            RefreshPanels(true);
            SetResultingSbText(null);
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
            {
                if (!smartOperationCheckBox.Checked || !(GetTagId(sourceTagListCustom.Text) == ArtistArtistsId || GetTagId(sourceTagListCustom.Text) == ComposerComposersId))
                {
                    MessageBox.Show(this, MsgSwapTagsSourceAndDestinationTagsAreTheSame,
                        null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            ignoreClosingForm = true;

            saveSettings();
            if (prepareBackgroundTask())
                switchOperation(swapTags, sender as Button, buttonOK, EmptyButton, buttonClose, true, null);
        }

        private void buttonClose_Click(object sender, EventArgs e)
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

        private void SwapTags_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Nothing for this command...
        }
    }
}
