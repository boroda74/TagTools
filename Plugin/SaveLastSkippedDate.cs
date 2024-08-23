using System;

using ExtensionMethods;

using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class SaveLastSkippedDate : PluginWindowTemplate
    {
        private CustomComboBox lastSkippedTagListCustom;
        private CustomComboBox lastSkippedDateFormatTagListCustom;


        internal SaveLastSkippedDate(Plugin plugin) : base(plugin)
        {
            InitializeComponent();
        }

        protected override void initializeForm()
        {
            base.initializeForm();

            lastSkippedTagListCustom = namesComboBoxes["lastSkippedTagList"];
            lastSkippedDateFormatTagListCustom = namesComboBoxes["lastSkippedDateFormatTagList"];


            var sampleDateTime = new DateTime(2022, 12, 31, 14, 30, 15);
            lastSkippedDateFormatTagListCustom.Items.Add(sampleDateTime.ToString("d"));
            lastSkippedDateFormatTagListCustom.Items.Add(sampleDateTime.ToString("g"));
            lastSkippedDateFormatTagListCustom.Items.Add(sampleDateTime.ToString("G"));
            lastSkippedDateFormatTagListCustom.SelectedIndex = SavedSettings.lastSkippedDateFormat;

            FillListByTagNames(lastSkippedTagListCustom.Items);
            if (SavedSettings.lastSkippedTagId == 0)
            {
                lastSkippedTagListCustom.Text = GetTagName(MetaDataType.Custom1);
                saveLastSkippedCheckBox.Checked = false;
            }
            else
            {
                lastSkippedTagListCustom.Text = GetTagName((MetaDataType)SavedSettings.lastSkippedTagId);
                saveLastSkippedCheckBox.Checked = true;
            }
        }

        private void saveSettings()
        {
            SavedSettings.lastSkippedDateFormat = lastSkippedDateFormatTagListCustom.SelectedIndex;

            if (saveLastSkippedCheckBox.Checked)
                SavedSettings.lastSkippedTagId = (int)GetTagId(lastSkippedTagListCustom.Text);
            else
                SavedSettings.lastSkippedTagId = 0;

            TagToolsPlugin.SaveSettings();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            saveSettings();
            Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveLastSkippedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            lastSkippedTagListCustom.Enable(saveLastSkippedCheckBox.Checked);
        }

        private void saveLastSkippedCheckBoxLabel_Click(object sender, EventArgs e)
        {
            saveLastSkippedCheckBox.Checked = !saveLastSkippedCheckBox.Checked;
        }

        private void SaveLastSkippedDate_Load(object sender, EventArgs e)
        {
            placeholderCheckBox.Visible = false;
        }
    }
}
