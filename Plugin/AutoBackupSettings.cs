using ExtensionMethods;
using System;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class AutoBackupSettings : PluginWindowTemplate
    {
        private decimal initialAutoBackupInterval;
        private string initialAutoBackupDirectory;

        internal AutoBackupSettings(Plugin tagToolsPluginParam) : base(tagToolsPluginParam)
        {
            InitializeComponent();
        }

        protected override void initializeForm()
        {
            base.initializeForm();

            initialAutoBackupInterval = SavedSettings.autobackupInterval;
            initialAutoBackupDirectory = SavedSettings.autobackupDirectory;

            autobackupFolderTextBox.Text = GetAutobackupDirectory(SavedSettings.autobackupDirectory);
            autobackupPrefixTextBox.Text = SavedSettings.autobackupPrefix;

            autobackupNumericUpDown.Value = SavedSettings.autobackupInterval;
            numberOfDaysNumericUpDown.Value = SavedSettings.autodeleteKeepNumberOfDays;
            numberOfFilesNumericUpDown.Value = SavedSettings.autodeleteKeepNumberOfFiles;

            if (SavedSettings.autobackupInterval != 0)
                autobackupCheckBox.Checked = true;

            if (SavedSettings.autodeleteKeepNumberOfDays != 0)
                autodeleteOldCheckBox.Checked = true;

            if (SavedSettings.autodeleteKeepNumberOfFiles != 0)
                autodeleteManyCheckBox.Checked = true;

            dontSkipAutobackupsIfPlayCountsChangedCheckBox.Checked = SavedSettings.dontSkipAutobackupsIfOnlyPlayCountsChanged;

            backupArtworksCheckBox.Checked = SavedSettings.backupArtworks;
            dontTryToGuessLibraryNameCheckBox.Checked = SavedSettings.dontTryToGuessLibraryName;


            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        private void autobackupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (autobackupCheckBox.Checked)
                autobackupNumericUpDown.Enable(true);
            else
                autobackupNumericUpDown.Enable(false);
        }

        private void autobackupCheckBoxLabel_Click(object sender, EventArgs e)
        {
            autobackupCheckBox.Checked = !autobackupCheckBox.Checked;
            autobackupCheckBox_CheckedChanged(null, null);
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
            autodeleteOldCheckBox_CheckedChanged(null, null);
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
            autodeleteManyCheckBox_CheckedChanged(null, null);
        }

        private void dontSkipAutobackupsIfPlayCountsChangedLabel_Click(object sender, EventArgs e)
        {
            dontSkipAutobackupsIfPlayCountsChangedCheckBox.Checked = !dontSkipAutobackupsIfPlayCountsChangedCheckBox.Checked;
        }

        private void dontTryToGuessLibraryNameCheckBoxLabel_Click(object sender, EventArgs e)
        {
            dontTryToGuessLibraryNameCheckBox.Checked = !dontTryToGuessLibraryNameCheckBox.Checked;
        }

        private void backupArtworksCheckBoxLabel_Click(object sender, EventArgs e)
        {
            backupArtworksCheckBox.Checked = !backupArtworksCheckBox.Checked;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                SelectedPath = autobackupFolderTextBox.Text
            };

            if (dialog.ShowDialog(this) == DialogResult.Cancel) return;

            autobackupFolderTextBox.Text = dialog.SelectedPath;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string driveLetter = MbApiInterface.Setting_GetPersistentStoragePath().Substring(0, 2);

            SavedSettings.autobackupDirectory = autobackupFolderTextBox.Text.Replace(MbApiInterface.Setting_GetPersistentStoragePath(), string.Empty).Replace(driveLetter, string.Empty);
            SavedSettings.autobackupPrefix = autobackupPrefixTextBox.Text;

            if (autobackupCheckBox.Checked)
                SavedSettings.autobackupInterval = autobackupNumericUpDown.Value;
            else
                SavedSettings.autobackupInterval = 0;

            if (autodeleteOldCheckBox.Checked)
                SavedSettings.autodeleteKeepNumberOfDays = numberOfDaysNumericUpDown.Value;
            else
                SavedSettings.autodeleteKeepNumberOfDays = 0;

            if (autodeleteManyCheckBox.Checked)
                SavedSettings.autodeleteKeepNumberOfFiles = numberOfFilesNumericUpDown.Value;
            else
                SavedSettings.autodeleteKeepNumberOfFiles = 0;

            SavedSettings.dontSkipAutobackupsIfOnlyPlayCountsChanged = dontSkipAutobackupsIfPlayCountsChangedCheckBox.Checked;


            PeriodicAutobackupTimer?.Dispose();

            PeriodicAutobackupTimer = null;

            if (initialAutoBackupDirectory != SavedSettings.autobackupDirectory)
            {
                MbApiInterface.MB_SetBackgroundTaskMessage(SbMovingBackupsToNewFolder);

                lock (AutobackupLocker)
                {
                    if (!System.IO.Directory.Exists(GetAutobackupDirectory(SavedSettings.autobackupDirectory)))
                        System.IO.Directory.CreateDirectory(GetAutobackupDirectory(SavedSettings.autobackupDirectory));

                    string[] files = System.IO.Directory.GetFileSystemEntries(GetAutobackupDirectory(initialAutoBackupDirectory));
                    for (int i = 0; i < files.Length; i++)
                        try
                        {
                            System.IO.Directory.Move(files[i], GetAutobackupDirectory(SavedSettings.autobackupDirectory) + @"\" + GetBackupSafeFilename(files[i]));
                        }
                        catch { };


                    try
                    {
                        System.IO.Directory.Delete(GetAutobackupDirectory(initialAutoBackupDirectory));
                    }
                    catch { };
                }

                MbApiInterface.MB_SetBackgroundTaskMessage(string.Empty);
            }

            SavedSettings.backupArtworks = backupArtworksCheckBox.Checked;
            SavedSettings.dontTryToGuessLibraryName = dontTryToGuessLibraryNameCheckBox.Checked;

            if (initialAutoBackupInterval != SavedSettings.autobackupInterval)
                TagToolsPlugin.InitBackupRestore();


            Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
