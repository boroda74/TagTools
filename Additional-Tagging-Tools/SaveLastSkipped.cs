using System;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;
using ExtensionMethods;

namespace MusicBeePlugin
{
    public partial class SaveLastSkippedCommand : PluginWindowTemplate
    {
        public SaveLastSkippedCommand(Plugin tagToolsPluginParam) : base(tagToolsPluginParam)
        {
            InitializeComponent();
            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            DateTime sampleDateTime = new DateTime(2022, 12, 31, 14, 30, 15);
            lastSkippedDateFormatTagList.Items.Add(sampleDateTime.ToString("d"));
            lastSkippedDateFormatTagList.Items.Add(sampleDateTime.ToString("g"));
            lastSkippedDateFormatTagList.Items.Add(sampleDateTime.ToString("G"));
            lastSkippedDateFormatTagList.SelectedIndex = SavedSettings.lastSkippedDateFormat;

            FillListByTagNames(lastSkippedTagList.Items);
            if (SavedSettings.lastSkippedTagId == 0)
            {
                lastSkippedTagList.Text = GetTagName(MetaDataType.Custom1);
                saveLastSkippedCheckBox.Checked = false;
            }
            else
            {
                lastSkippedTagList.Text = GetTagName((MetaDataType)SavedSettings.lastSkippedTagId);
                saveLastSkippedCheckBox.Checked = true;
            }
        }

        private void saveSettings()
        {
            SavedSettings.lastSkippedDateFormat = lastSkippedDateFormatTagList.SelectedIndex;

            if (saveLastSkippedCheckBox.Checked)
                SavedSettings.lastSkippedTagId = (int)GetTagId(lastSkippedTagList.Text);
            else
                SavedSettings.lastSkippedTagId = 0;

            TagToolsPlugin.SaveSettings();
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

        private void saveLastSkippedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            lastSkippedTagList.Enable(saveLastSkippedCheckBox.Checked);
        }
    }
}
