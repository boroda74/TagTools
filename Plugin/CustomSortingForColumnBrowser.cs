using System;
using System.Collections.Generic;

using ExtensionMethods;

using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class CustomSortingForColumnBrowser : PluginWindowTemplate
    {
        public const string EmptyPrefix = "  ";
        public const string MarkedPrefix = "⯀ ";

        public class CustomSortingTag
        {
            public bool mark;
            public string tagName = string.Empty;
            internal CustomSortingSet parent;

            public override string ToString()
            {
                return (mark ? MarkedPrefix : EmptyPrefix) + tagName;
            }

            public override bool Equals(object compared)
            {
                if (compared == null)
                    return false;

                return parent == ((CustomSortingTag)compared).parent && tagName == ((CustomSortingTag)compared).tagName;
            }
        }

        public class CustomSortingSet
        {
            public CustomSortingTag sourceTag;
            public CustomSortingTag tag;

            public void Init()
            {
                sourceTag.parent = this;
                tag.parent = this;
            }

            public override bool Equals(object compared)
            {
                if (compared == null)
                    return false;

                return ((CustomSortingSet)compared).sourceTag == sourceTag && ((CustomSortingSet)compared).tag == tag;
            }
        }


        private CustomComboBox tagComboBoxCustom;
        private CustomComboBox sourceTagComboBoxCustom;


        private SortedDictionary<string, string> newPureTagValues = new SortedDictionary<string, string>();
        private SortedDictionary<string, string> originalPureTagValues = new SortedDictionary<string, string>();

        private bool ignoreAutoCopyCheckBoxChanged = false;

        public CustomSortingForColumnBrowser(Plugin plugin) : base(plugin)
        {
            InitializeComponent();
        }

        internal protected override void initializeForm()
        {
            base.initializeForm();

            sourceTagComboBoxCustom = namesComboBoxes["sourceTagComboBox"];
            tagComboBoxCustom = namesComboBoxes["tagComboBox"];


            int DisplayArtistIndex = 0;
            int Custom1Index = 0;

            List<string> sourceTags = new List<string>();
            FillListByTagNames(sourceTags, true, false, false, false, false, true, false);

            foreach (string tag in sourceTags)
            {
                sourceTagComboBoxCustom.Add(new CustomSortingTag { mark = false, tagName = tag });

                if (tag == DisplayedArtistName)
                    DisplayArtistIndex = sourceTagComboBoxCustom.Items.Count - 1;
            }


            List<string> tags = new List<string>();
            FillListByTagNames(tags, false, false, false, false, false, false, false);

            foreach (string tag in tags)
            {
                tagComboBoxCustom.Add(new CustomSortingTag { mark = false, tagName = tag });

                if (tag == GetTagName(MetaDataType.Custom1))
                    Custom1Index = tagComboBoxCustom.Items.Count - 1;
            }


            foreach (var customSortingSet in SavedSettings.customSortingSets)
            {
                for (int i = 0; i < sourceTagComboBoxCustom.Items.Count - 1; i++)
                {
                    if ((sourceTagComboBoxCustom.Items[i] as CustomSortingTag).tagName == customSortingSet.sourceTag.tagName)
                    {
                        (sourceTagComboBoxCustom.Items[i] as CustomSortingTag).mark = customSortingSet.sourceTag.mark;
                        (sourceTagComboBoxCustom.Items[i] as CustomSortingTag).parent = customSortingSet.sourceTag.parent;
                    }
                }

                for (int i = 0; i < tagComboBoxCustom.Items.Count - 1; i++)
                {
                    if ((tagComboBoxCustom.Items[i] as CustomSortingTag).tagName == customSortingSet.tag.tagName)
                    {
                        (tagComboBoxCustom.Items[i] as CustomSortingTag).mark = customSortingSet.tag.mark;
                        (tagComboBoxCustom.Items[i] as CustomSortingTag).parent = customSortingSet.tag.parent;
                    }
                }
            }


            if (SavedSettings.customSortingSets.Count > 0)
            {
                sourceTagComboBoxCustom.SelectedItem = SavedSettings.customSortingSets[0].sourceTag;
                tagComboBoxCustom.SelectedItem = SavedSettings.customSortingSets[0].tag;
            }
            else
            {
                sourceTagComboBoxCustom.SelectedIndex = DisplayArtistIndex;
                tagComboBoxCustom.SelectedIndex = Custom1Index;
            }


            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        public static void InitCscb()
        {
            if (SavedSettings.dontShowCustomSortingForColumnBrowser || SavedSettings.customSortingSets.Count == 0)
                return;

            foreach (var customSortingSet in SavedSettings.customSortingSets)
                customSortingSet.Init();
        }

        private void buttonCopy_Clicked(object sender, EventArgs e)
        {
            tagList.Items.Clear();


            MetaDataType tagId = GetTagId((sourceTagComboBoxCustom.SelectedItem as CustomSortingTag).tagName); //-V3080

            MbApiInterface.Library_QueryFilesEx("domain=Library", out var files);


            originalPureTagValues.Clear();
            foreach (var file in files)
            {
                string tagValue = GetFileTag(file, tagId);
                originalPureTagValues.AddSkip(tagValue, tagValue);
            }


            int totalCount = originalPureTagValues.Count;
            newPureTagValues.Clear();
            foreach (var tags in originalPureTagValues)
                newPureTagValues.AddSkip(getSpaces(totalCount--) + tags.Value, tags.Key);


            foreach (var tags in newPureTagValues)
                tagList.Items.Add(tags.Key);
        }

        private void buttonOverwrite_Click(object sender, EventArgs e)
        {
            MetaDataType tagId1 = GetTagId((sourceTagComboBoxCustom.SelectedItem as CustomSortingTag).tagName); //-V3080
            MetaDataType tagId2 = GetTagId((tagComboBoxCustom.SelectedItem as CustomSortingTag).tagName); //-V3080

            MbApiInterface.Library_QueryFilesEx("domain=Library", out var files);

            foreach (var file in files)
            {
                string originalTagValue = GetFileTag(file, tagId1);

                foreach (var tags in newPureTagValues)
                {
                    if (tags.Value == originalTagValue)
                    {
                        string newValue = tags.Key;

                        SetFileTag(file, tagId2, newValue);
                        CommitTagsToFile(file);

                        break;
                    }
                }
            }

            RefreshPanels(true);
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            tagList.Items.Clear();


            MetaDataType tagId = GetTagId((tagComboBoxCustom.SelectedItem as CustomSortingTag).tagName); //-V3080

            MbApiInterface.Library_QueryFilesEx("domain=Library", out var files);


            originalPureTagValues.Clear();
            foreach (var file in files)
            {
                string tagValue = GetFileTag(file, tagId);
                originalPureTagValues.AddSkip(tagValue, tagValue.TrimStart('\u200b'));
            }


            newPureTagValues.Clear();
            foreach (var tags in originalPureTagValues)
                newPureTagValues.AddSkip(tags.Key, tags.Value);


            foreach (var tags in newPureTagValues)
                tagList.Items.Add(tags.Key);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            MetaDataType tagId = GetTagId((tagComboBoxCustom.SelectedItem as CustomSortingTag).tagName); //-V3080

            MbApiInterface.Library_QueryFilesEx("domain=Library", out var files);

            foreach (var file in files)
            {
                string tagValue = GetFileTag(file, tagId);

                foreach (var tags in originalPureTagValues)
                {
                    if (tags.Key == tagValue)
                    {
                        string pureValue = tags.Value;

                        foreach (var tags2 in newPureTagValues)
                        {
                            if (pureValue == tags2.Value)
                            {
                                SetFileTag(file, tagId, tags2.Key);
                                CommitTagsToFile(file);

                                break;
                            }
                        }
                    }
                }
            }

            RefreshPanels(true);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private string getSpaces(int times)
        {
            var sequence = string.Empty;

            for (var i = 0; i < times; i++)
                sequence += "\u200b";

            return sequence;
        }

        private int getSpacesCount(string tagValue)
        {
            int count = 0;
            for (count = 0; count < tagValue.Length; count++)
                if (tagValue[count] != '\u200b')
                    break;

            return count;
        }

        private void buttonUpMore_Click(object sender, EventArgs e)
        {
            moveUp(15);
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            moveUp(1);
        }

        private void moveUp(int count)
        {
            if (tagList.SelectedIndex <= 0)
                return;

            int upperIndex = tagList.SelectedIndex - count;
            if (upperIndex < 0)
                upperIndex = 0;


            string upperTagValueOrig = tagList.Items[upperIndex] as string;
            string currentTagValueOrig = tagList.Items[tagList.SelectedIndex] as string;

            int upperSpacesCount = getSpacesCount(upperTagValueOrig);
            int currentSpacesCount = getSpacesCount(currentTagValueOrig);

            string upperTagValue = upperTagValueOrig.Substring(upperSpacesCount);
            string currentTagValue = currentTagValueOrig.Substring(currentSpacesCount);


            string pureCurrentTagValue = newPureTagValues[currentTagValueOrig];
            string pureUpperTagValue = newPureTagValues[upperTagValueOrig];

            newPureTagValues.Remove(currentTagValueOrig);
            newPureTagValues.Remove(upperTagValueOrig);


            currentTagValue = getSpaces(upperSpacesCount) + currentTagValue;
            upperTagValue = getSpaces(currentSpacesCount) + upperTagValue;

            newPureTagValues.Add(currentTagValue, pureCurrentTagValue);
            newPureTagValues.Add(upperTagValue, pureUpperTagValue);


            tagList.Items.Remove(upperTagValueOrig);
            tagList.Items.Add(upperTagValue);

            tagList.Items.Remove(currentTagValueOrig);
            tagList.Items.Add(currentTagValue);

            tagList.SelectedIndex = upperIndex;
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            moveDown(1);
        }

        private void buttonDownMore_Click(object sender, EventArgs e)
        {
            moveDown(15);
        }

        private void moveDown(int count)
        {
            if (tagList.SelectedIndex == tagList.Items.Count - 1)
                return;

            int lowerIndex = tagList.SelectedIndex + count;
            if (lowerIndex > tagList.Items.Count - 1)
                lowerIndex = tagList.Items.Count - 1;


            string lowerTagValueOrig = tagList.Items[lowerIndex] as string;
            string currentTagValueOrig = tagList.Items[tagList.SelectedIndex] as string;

            int lowerSpacesCount = getSpacesCount(lowerTagValueOrig);
            int currentSpacesCount = getSpacesCount(currentTagValueOrig);

            string lowerTagValue = lowerTagValueOrig.Substring(lowerSpacesCount);
            string currentTagValue = currentTagValueOrig.Substring(currentSpacesCount);


            string pureCurrentTagValue = newPureTagValues[currentTagValueOrig];
            string pureLowerTagValue = newPureTagValues[lowerTagValueOrig];

            newPureTagValues.Remove(currentTagValueOrig);
            newPureTagValues.Remove(lowerTagValueOrig);



            currentTagValue = getSpaces(lowerSpacesCount) + currentTagValue;
            lowerTagValue = getSpaces(currentSpacesCount) + lowerTagValue;

            newPureTagValues.Add(currentTagValue, pureCurrentTagValue);
            newPureTagValues.Add(lowerTagValue, pureLowerTagValue);


            tagList.Items.Remove(lowerTagValueOrig);
            tagList.Items.Add(lowerTagValue);

            tagList.Items.Remove(currentTagValueOrig);
            tagList.Items.Add(currentTagValue);

            tagList.SelectedIndex = lowerIndex;
        }

        private void autoCopyCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!autoCopyCheckBox.IsEnabled())
                return;

            autoCopyCheckBox.Checked = !autoCopyCheckBox.Checked;
        }

        private void autoCopyCheckBox_CheckedChanged(object sender, EventArgs e)//*******************
        {
            if (ignoreAutoCopyCheckBoxChanged)
                return;


            if (autoCopyCheckBox.Checked)
            {
                for (int i = SavedSettings.customSortingSets.Count - 1; i >= 0; i--)
                {
                    if (SavedSettings.customSortingSets[i].sourceTag.tagName == (sourceTagComboBoxCustom.SelectedItem as CustomSortingTag).tagName) //-V3080
                        SavedSettings.customSortingSets.RemoveAt(i);
                    else if (SavedSettings.customSortingSets[i].tag.tagName == (tagComboBoxCustom.SelectedItem as CustomSortingTag).tagName) //-V3080
                        SavedSettings.customSortingSets.RemoveAt(i);
                }

                (sourceTagComboBoxCustom.SelectedItem as CustomSortingTag).mark = true; //-V3080
                (tagComboBoxCustom.SelectedItem as CustomSortingTag).mark = true; //-V3080

                var newSet = new CustomSortingSet
                {
                    sourceTag = sourceTagComboBoxCustom.SelectedItem as CustomSortingTag,
                    tag = tagComboBoxCustom.SelectedItem as CustomSortingTag
                };

                (sourceTagComboBoxCustom.SelectedItem as CustomSortingTag).parent = newSet; //-V3080
                (tagComboBoxCustom.SelectedItem as CustomSortingTag).parent = newSet; //-V3080

                SavedSettings.customSortingSets.Add(newSet);
            }
            else
            {
                var currentSet = (sourceTagComboBoxCustom.SelectedItem as CustomSortingTag).parent; //-V3080

                (sourceTagComboBoxCustom.SelectedItem as CustomSortingTag).mark = false; //-V3080
                (sourceTagComboBoxCustom.SelectedItem as CustomSortingTag).parent = null; //-V3080
                (tagComboBoxCustom.SelectedItem as CustomSortingTag).mark = false; //-V3080
                (tagComboBoxCustom.SelectedItem as CustomSortingTag).parent = null; //-V3080

                SavedSettings.customSortingSets.Remove(currentSet);
            }

            sourceTagComboBoxCustom.Refresh();
            tagComboBoxCustom.Refresh();
        }

        private void sourceTagComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ignoreAutoCopyCheckBoxChanged = true;

            if ((sourceTagComboBoxCustom.SelectedItem as CustomSortingTag).mark) //-V3080
            {
                tagComboBoxCustom.SelectedItem = (sourceTagComboBoxCustom.SelectedItem as CustomSortingTag).parent.tag; //-V3080
                autoCopyCheckBox.Checked = true;
            }
            else
            {
                autoCopyCheckBox.Checked = false;
            }

            ignoreAutoCopyCheckBoxChanged = false;
        }

        private void tagComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ignoreAutoCopyCheckBoxChanged = true;

            if ((tagComboBoxCustom.SelectedItem as CustomSortingTag).mark) //-V3080
            {
                if ((tagComboBoxCustom.SelectedItem as CustomSortingTag).parent.sourceTag == sourceTagComboBoxCustom.SelectedItem) //-V3080
                {
                    autoCopyCheckBox.Checked = true;
                    ignoreAutoCopyCheckBoxChanged = false;
                    return;
                }
            }

            autoCopyCheckBox.Checked = false;

            ignoreAutoCopyCheckBoxChanged = false;
        }
    }
}
