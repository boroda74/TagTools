using System;
using System.Windows.Forms;


namespace MusicBeePlugin
{
    public partial class AutoBackupSettings : PluginWindowTemplate
    {
        private decimal initialAutobackupInterval;
        private string initialAutobackupDirectory;

        public AutoBackupSettings()
        {
            InitializeComponent();
        }

        public AutoBackupSettings(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            initialAutobackupInterval = Plugin.SavedSettings.autobackupInterval;
            initialAutobackupDirectory = Plugin.SavedSettings.autobackupDirectory;

            autobackupFolderTextBox.Text = Plugin.GetAutobackupDirectory(Plugin.SavedSettings.autobackupDirectory);
            autobackupPrefixTextBox.Text = Plugin.SavedSettings.autobackupPrefix;

            autobackupNumericUpDown.Value = Plugin.SavedSettings.autobackupInterval;
            numberOfDaysNumericUpDown.Value = Plugin.SavedSettings.autodeleteKeepNumberOfDays;
            numberOfFilesNumericUpDown.Value = Plugin.SavedSettings.autodeleteKeepNumberOfFiles;

            if (Plugin.SavedSettings.autobackupInterval != 0)
                autobackupCheckBox.Checked = true;

            if (Plugin.SavedSettings.autodeleteKeepNumberOfDays != 0)
                autodeleteOldCheckBox.Checked = true;

            if (Plugin.SavedSettings.autodeleteKeepNumberOfFiles != 0)
                autodeleteManyCheckBox.Checked = true;

            DontSkipAutobackupsIfPlayCountsChangedCheckBox.Checked = Plugin.SavedSettings.dontSkipAutobackupsIfPlayCountsChanged;

            backupArtworksCheckBox.Checked = Plugin.SavedSettings.backupArtworks;
            dontTryToGuessLibraryNameCheckBox.Checked = Plugin.SavedSettings.dontTryToGuessLibraryName;
        }

        private void autobackupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (autobackupCheckBox.Checked)
                autobackupNumericUpDown.Enabled = true;
            else
                autobackupNumericUpDown.Enabled = false;
        }

        private void autodeleteOldCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (autodeleteOldCheckBox.Checked)
                numberOfDaysNumericUpDown.Enabled = true;
            else
                numberOfDaysNumericUpDown.Enabled = false;
        }

        private void autodeleteManyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (autodeleteManyCheckBox.Checked)
                numberOfFilesNumericUpDown.Enabled = true;
            else
                numberOfFilesNumericUpDown.Enabled = false;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = autobackupFolderTextBox.Text;

            if (dialog.ShowDialog(this) == DialogResult.Cancel) return;

            autobackupFolderTextBox.Text = dialog.SelectedPath;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string driveLetter = Plugin.MbApiInterface.Setting_GetPersistentStoragePath().Substring(0, 2);

            Plugin.SavedSettings.autobackupDirectory = autobackupFolderTextBox.Text.Replace(Plugin.MbApiInterface.Setting_GetPersistentStoragePath(), "").Replace(driveLetter, "");
            Plugin.SavedSettings.autobackupPrefix = autobackupPrefixTextBox.Text;

            if (autobackupCheckBox.Checked)
                Plugin.SavedSettings.autobackupInterval = autobackupNumericUpDown.Value;
            else
                Plugin.SavedSettings.autobackupInterval = 0;

            if (autodeleteOldCheckBox.Checked)
                Plugin.SavedSettings.autodeleteKeepNumberOfDays = numberOfDaysNumericUpDown.Value;
            else
                Plugin.SavedSettings.autodeleteKeepNumberOfDays = 0;

            if (autodeleteManyCheckBox.Checked)
                Plugin.SavedSettings.autodeleteKeepNumberOfFiles = numberOfFilesNumericUpDown.Value;
            else
                Plugin.SavedSettings.autodeleteKeepNumberOfFiles = 0;

            Plugin.SavedSettings.dontSkipAutobackupsIfPlayCountsChanged = DontSkipAutobackupsIfPlayCountsChangedCheckBox.Checked;


            Plugin.PeriodicAutobackupTimer?.Dispose();

            Plugin.PeriodicAutobackupTimer = null;

            if (initialAutobackupDirectory != Plugin.SavedSettings.autobackupDirectory)
            {
                Plugin.MbApiInterface.MB_SetBackgroundTaskMessage(Plugin.SbMovingBackupsToNewFolder);

                lock (Plugin.AutobackupLocker)
                {
                    if (!System.IO.Directory.Exists(Plugin.GetAutobackupDirectory(Plugin.SavedSettings.autobackupDirectory)))
                        System.IO.Directory.CreateDirectory(Plugin.GetAutobackupDirectory(Plugin.SavedSettings.autobackupDirectory));

                    string[] files = System.IO.Directory.GetFileSystemEntries(Plugin.GetAutobackupDirectory(initialAutobackupDirectory));
                    for (int i = 0; i < files.Length; i++)
                        try
                        {
                            System.IO.Directory.Move(files[i], Plugin.GetAutobackupDirectory(Plugin.SavedSettings.autobackupDirectory) + @"\" + Plugin.GetBackupSafeFilename(files[i]));
                        }
                        catch { };


                    try
                    {
                        System.IO.Directory.Delete(Plugin.GetAutobackupDirectory(initialAutobackupDirectory));
                    }
                    catch { };
                }

                Plugin.MbApiInterface.MB_SetBackgroundTaskMessage("");
            }

            if (initialAutobackupInterval != Plugin.SavedSettings.autobackupInterval && Plugin.SavedSettings.autobackupInterval != 0)
            {
                Plugin.PeriodicAutobackupTimer = new System.Threading.Timer(TagToolsPlugin.regularAutobackup, null, new TimeSpan(0, 0, (int)Plugin.SavedSettings.autobackupInterval * 60), new TimeSpan(0, 0, (int)Plugin.SavedSettings.autobackupInterval * 60));
            }

            Plugin.SavedSettings.backupArtworks = backupArtworksCheckBox.Checked;
            Plugin.SavedSettings.dontTryToGuessLibraryName = dontTryToGuessLibraryNameCheckBox.Checked;


            Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
