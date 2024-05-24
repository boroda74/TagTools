using ExtensionMethods;
using System;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class AutoBackupSettings : PluginWindowTemplate
    {
        private CustomComboBox trackIdTagListCustom;

        private decimal initialAutoBackupInterval;
        private string initialAutoBackupDirectory;

        private bool customTrackIdTagWarningShown = false;

        internal AutoBackupSettings(Plugin plugin) : base(plugin)
        {
            InitializeComponent();
        }

        protected override void initializeForm()
        {
            base.initializeForm();

            trackIdTagListCustom = namesComboBoxes["trackIdTagList"];


            customTrackIdTagWarningShown = true;


            if (CustomTrackIdTag == 0)
            {
                UseCustomTrackIdTag = SavedSettings.useCustomTrackIdTag;
                CustomTrackIdTag = SavedSettings.customTrackIdTag;
            }

            storeTrackIdsInCustomTagCheckBox.Checked = UseCustomTrackIdTag;

            FillListByTagNames(trackIdTagListCustom.Items, false, false, false, false, false, false, true);
            trackIdTagListCustom.Text = GetTagName((MetaDataType)CustomTrackIdTag);
            if (trackIdTagListCustom.SelectedIndex == -1)
                trackIdTagListCustom.SelectedIndex = 0;

            customTrackIdTagWarningShown = false;


            initialAutoBackupInterval = SavedSettings.autoBackupInterval;
            initialAutoBackupDirectory = SavedSettings.autoBackupDirectory;

            autoBackupFolderTextBox.Text = BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory);
            autoBackupPrefixTextBox.Text = SavedSettings.autoBackupPrefix;

            autoBackupNumericUpDown.Value = SavedSettings.autoBackupInterval;
            numberOfDaysNumericUpDown.Value = SavedSettings.autodeleteKeepNumberOfDays;
            numberOfFilesNumericUpDown.Value = SavedSettings.autodeleteKeepNumberOfFiles;

            if (SavedSettings.autoBackupInterval != 0)
                autoBackupCheckBox.Checked = true;

            if (SavedSettings.autodeleteKeepNumberOfDays != 0)
                autodeleteOldCheckBox.Checked = true;

            if (SavedSettings.autodeleteKeepNumberOfFiles != 0)
                autodeleteManyCheckBox.Checked = true;

            dontSkipAutoBackupsIfPlayCountsChangedCheckBox.Checked = SavedSettings.dontSkipAutoBackupsIfOnlyPlayCountsChanged;

            backupArtworksCheckBox.Checked = SavedSettings.backupArtworks;
            dontTryToGuessLibraryNameCheckBox.Checked = SavedSettings.dontTryToGuessLibraryName;


            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        private void autoBackupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (autoBackupCheckBox.Checked)
                autoBackupNumericUpDown.Enable(true);
            else
                autoBackupNumericUpDown.Enable(false);
        }

        private void autoBackupCheckBoxLabel_Click(object sender, EventArgs e)
        {
            autoBackupCheckBox.Checked = !autoBackupCheckBox.Checked;
        }

        private void autodeleteOldCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (autodeleteOldCheckBox.Checked)
                numberOfDaysNumericUpDown.Enable(true);
            else
                numberOfDaysNumericUpDown.Enable(false);
        }

        private void autodeleteOldCheckBoxLabel_Click(object sender, EventArgs e)
        {
            autodeleteOldCheckBox.Checked = !autodeleteOldCheckBox.Checked;
        }

        private void autodeleteManyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (autodeleteManyCheckBox.Checked)
                numberOfFilesNumericUpDown.Enable(true);
            else
                numberOfFilesNumericUpDown.Enable(false);
        }

        private void autodeleteManyCheckBoxLabel_Click(object sender, EventArgs e)
        {
            autodeleteManyCheckBox.Checked = !autodeleteManyCheckBox.Checked;
        }

        private void dontSkipAutoBackupsIfPlayCountsChangedLabel_Click(object sender, EventArgs e)
        {
            dontSkipAutoBackupsIfPlayCountsChangedCheckBox.Checked = !dontSkipAutoBackupsIfPlayCountsChangedCheckBox.Checked;
        }

        private void dontTryToGuessLibraryNameCheckBoxLabel_Click(object sender, EventArgs e)
        {
            dontTryToGuessLibraryNameCheckBox.Checked = !dontTryToGuessLibraryNameCheckBox.Checked;
        }


        private void storeTrackIdsInCustomTagCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            trackIdTagListCustom.Enable(storeTrackIdsInCustomTagCheckBox.Checked);
            UseCustomTrackIdTag = storeTrackIdsInCustomTagCheckBox.Checked;

            if (!customTrackIdTagWarningShown && UseCustomTrackIdTag != SavedSettings.useCustomTrackIdTag)
            {
                customTrackIdTagWarningShown = true;
                MessageBox.Show(this, MsgBrCreateNewBaselineBackupBeforeMusicBeeRestart, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void storeTrackIdsInCustomTagCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (storeTrackIdsInCustomTagCheckBox.IsEnabled())
                storeTrackIdsInCustomTagCheckBox.Checked = !storeTrackIdsInCustomTagCheckBox.Checked;
        }

        private void trackIdTagList_SelectedIndexChanged(object sender, EventArgs e)
        {
            CustomTrackIdTag = (int)GetTagId(trackIdTagListCustom.Text);


            if (!customTrackIdTagWarningShown && UseCustomTrackIdTag && CustomTrackIdTag != SavedSettings.customTrackIdTag)
            {
                customTrackIdTagWarningShown = true;
                MessageBox.Show(this, MsgBrCreateNewBaselineBackupBeforeMusicBeeRestart, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void backupArtworksCheckBoxLabel_Click(object sender, EventArgs e)
        {
            backupArtworksCheckBox.Checked = !backupArtworksCheckBox.Checked;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                SelectedPath = autoBackupFolderTextBox.Text
            };

            if (dialog.ShowDialog(this) == DialogResult.Cancel) return;

            autoBackupFolderTextBox.Text = dialog.SelectedPath;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string driveLetter = MbApiInterface.Setting_GetPersistentStoragePath().Substring(0, 2);

            SavedSettings.autoBackupDirectory = autoBackupFolderTextBox.Text.Replace(MbApiInterface.Setting_GetPersistentStoragePath(), string.Empty).Replace(driveLetter, string.Empty);
            SavedSettings.autoBackupPrefix = autoBackupPrefixTextBox.Text;

            if (autoBackupCheckBox.Checked)
                SavedSettings.autoBackupInterval = autoBackupNumericUpDown.Value;
            else
                SavedSettings.autoBackupInterval = 0;

            if (autodeleteOldCheckBox.Checked)
                SavedSettings.autodeleteKeepNumberOfDays = numberOfDaysNumericUpDown.Value;
            else
                SavedSettings.autodeleteKeepNumberOfDays = 0;

            if (autodeleteManyCheckBox.Checked)
                SavedSettings.autodeleteKeepNumberOfFiles = numberOfFilesNumericUpDown.Value;
            else
                SavedSettings.autodeleteKeepNumberOfFiles = 0;

            SavedSettings.dontSkipAutoBackupsIfOnlyPlayCountsChanged = dontSkipAutoBackupsIfPlayCountsChangedCheckBox.Checked;


            PeriodicAutoBackupTimer?.Dispose();

            PeriodicAutoBackupTimer = null;

            if (initialAutoBackupDirectory != SavedSettings.autoBackupDirectory)
            {
                MbApiInterface.MB_SetBackgroundTaskMessage(SbMovingBackupsToNewFolder);

                lock (TracksNeededToBeBackedUp)
                {
                    if (!System.IO.Directory.Exists(BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory)))
                        System.IO.Directory.CreateDirectory(BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory));

                    string[] files = System.IO.Directory.GetFileSystemEntries(BrGetAutoBackupDirectory(initialAutoBackupDirectory));
                    for (int i = 0; i < files.Length; i++)
                        try
                        {
                            System.IO.Directory.Move(files[i], BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory) + @"\" + BrGetBackupSafeFilename(files[i]));
                        }
                        catch { };


                    try
                    {
                        System.IO.Directory.Delete(BrGetAutoBackupDirectory(initialAutoBackupDirectory));
                    }
                    catch { };
                }

                MbApiInterface.MB_SetBackgroundTaskMessage(string.Empty);
            }

            SavedSettings.backupArtworks = backupArtworksCheckBox.Checked;
            SavedSettings.dontTryToGuessLibraryName = dontTryToGuessLibraryNameCheckBox.Checked;

            if (initialAutoBackupInterval != SavedSettings.autoBackupInterval)
                TagToolsPlugin.InitBackupRestore();


            Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
