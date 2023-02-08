using System;
using System.Collections.Generic;


namespace MusicBeePlugin
{
    public partial class SelectTagsPlugin : PluginWindowTemplate
    {
        public int[] displayedTags;

        public SelectTagsPlugin()
        {
            InitializeComponent();
        }

        public SelectTagsPlugin(Plugin tagToolsPluginParam, int[] displayedTagsParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;
            displayedTags = displayedTagsParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            Plugin.FillListByTagNames(uncheckedTagsList.Items, false, true, false);

            for (int i = 0; i < displayedTags.Length; i++)
            {
                for (int j = 0; j < uncheckedTagsList.Items.Count; j++)
                {
                    string tagName = Plugin.GetTagName((Plugin.MetaDataType)displayedTags[i]);

                    if ((string)uncheckedTagsList.Items[j] == tagName)
                    {
                        uncheckedTagsList.SetItemChecked(j, true);
                        break;
                    }
                }
            }


            for (int i = 0; i < uncheckedTagsList.Items.Count;)
                if (uncheckedTagsList.GetItemChecked(i))
                    uncheckedTagsList.SelectedIndex = i;
                else
                    i++;
        }

        private void saveSettings()
        {
            List<int> checkedIds = new List<int>();

            for (int i = 0; i < checkedTagsList.Items.Count; i++)
            {
                int id = (int)Plugin.GetTagId((string)checkedTagsList.Items[i]);

                checkedIds.Add(id);
            }

            displayedTags = new int[checkedIds.Count];
            checkedIds.CopyTo(displayedTags, 0);
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

        private void checkedTagsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedTagsList.SelectedIndex != -1)
            {
                uncheckedTagsList.Items.Add(checkedTagsList.Items[checkedTagsList.SelectedIndex]);
                checkedTagsList.Items.RemoveAt(checkedTagsList.SelectedIndex);
            }

            if (checkedTagsList.Items.Count == 0)
                buttonOK.Enabled = false;
        }

        private void uncheckedTagsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (uncheckedTagsList.SelectedIndex != -1)
            {
                checkedTagsList.Items.Add(uncheckedTagsList.Items[uncheckedTagsList.SelectedIndex]);

                for (int i = 0; i < checkedTagsList.Items.Count; i++)
                    checkedTagsList.SetItemChecked(i, true);

                uncheckedTagsList.Items.RemoveAt(uncheckedTagsList.SelectedIndex);
            }

            if (checkedTagsList.Items.Count > 0)
                buttonOK.Enabled = true;
        }
    }
}
