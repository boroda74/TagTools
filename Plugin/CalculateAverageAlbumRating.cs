﻿using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class CalculateAverageAlbumRating : PluginWindowTemplate
    {
        private CustomComboBox trackRatingTagListCustom;
        private CustomComboBox albumRatingTagListCustom;


        private string[] files = new string[0];

        internal CalculateAverageAlbumRating(Plugin tagToolsPluginParam) : base(tagToolsPluginParam)
        {
            InitializeComponent();
        }

        protected override void initializeForm()
        {
            base.initializeForm();

            trackRatingTagListCustom = namesComboBoxes["trackRatingTagList"];
            albumRatingTagListCustom = namesComboBoxes["albumRatingTagList"];


            buttonSettings.Image = ThemedBitmapAddRef(this, buttonSettings.Image, Gear);

            calculateAlbumRatingAtStartUpCheckBox.Checked = SavedSettings.calculateAlbumRatingAtStartUp;
            calculateAlbumRatingAtTagsChangedCheckBox.Checked = SavedSettings.calculateAlbumRatingAtTagsChanged;
            notifyWhenCalculationCompletedCheckBox.Checked = SavedSettings.notifyWhenCalculationCompleted;
            considerUnratedCheckBox.Checked = SavedSettings.considerUnrated;

            FillListByTagNames(trackRatingTagListCustom.Items);
            trackRatingTagListCustom.Text = SavedSettings.trackRatingTagName;

            FillListByTagNames(albumRatingTagListCustom.Items);
            albumRatingTagListCustom.Text = SavedSettings.albumRatingTagName;


            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        internal void calculateAlbumRating()
        {
            List<string[]> tags = new List<string[]>();
            string[] row;
            string currentFile;

            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                currentFile = files[fileCounter];

                SetStatusbarTextForFileOperations(CarCommandSbText, true, fileCounter, files.Length, currentFile);

                row = new string[4];

                row[0] = GetFileTag(currentFile, MetaDataType.AlbumArtist);
                row[1] = GetFileTag(currentFile, MetaDataType.Album);
                row[2] = GetFileTag(currentFile, GetTagId(SavedSettings.trackRatingTagName), true);
                row[3] = currentFile;

                tags.Add(row);
            }

            SetStatusbarText(CarCommandSbText + " (" + SbSorting + ")", false);

            TextTableComparer textTableComparator = new TextTableComparer
            {
                tagCounterIndex = 2
            };
            tags.Sort(textTableComparator);

            string currentAlbumArtsist;
            string currentAlbum;

            string prevAlbumArtsist = string.Empty;
            string prevAlbum = string.Empty;
            int prevRow = 0;

            double sumRating;
            int numberOfTracks;
            double avgRating;

            for (int i = 0; i < tags.Count; i++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                currentAlbumArtsist = tags[i][0];
                currentAlbum = tags[i][1];

                if (i == 0)
                {
                    prevAlbumArtsist = currentAlbumArtsist;
                    prevAlbum = currentAlbum;
                }

                if (prevAlbumArtsist != currentAlbumArtsist || prevAlbum != currentAlbum)
                {
                    sumRating = 0;
                    numberOfTracks = 0;

                    for (int j = prevRow; j < i; j++)
                    {
                        if (string.Empty + tags[j][2] != string.Empty || SavedSettings.considerUnrated)
                        {
                            sumRating += ConvertStrings(tags[j][2], ResultType.Double, DataType.Rating).resultD;
                            numberOfTracks++;
                        }
                    }

                    if (numberOfTracks == 0)
                        avgRating = 0;
                    else
                        avgRating = Math.Round(sumRating / 10 / numberOfTracks) * 10;

                    for (int j = prevRow; j < i; j++)
                    {
                        currentFile = tags[j][3];

                        SetFileTag(currentFile, GetTagId(SavedSettings.albumRatingTagName), avgRating.ToString(), true);
                        CommitTagsToFile(currentFile, false, true);

                        SetStatusbarTextForFileOperations(CarCommandSbText, false, j, tags.Count, currentFile);
                    }

                    prevAlbumArtsist = currentAlbumArtsist;
                    prevAlbum = currentAlbum;
                    prevRow = i;
                }
            }

            sumRating = 0;
            numberOfTracks = 0;

            for (int j = prevRow; j < tags.Count; j++)
            {
                if (string.Empty + tags[j][2] != string.Empty || SavedSettings.considerUnrated)
                {
                    sumRating += ConvertStrings(tags[j][2], ResultType.Double, DataType.Rating).resultD;
                    numberOfTracks++;
                }
            }

            if (numberOfTracks == 0)
                avgRating = 0;
            else
                avgRating = Math.Round(sumRating / 10 / numberOfTracks) * 10;

            for (int j = prevRow; j < tags.Count; j++)
            {
                currentFile = tags[j][3];

                SetFileTag(currentFile, GetTagId(SavedSettings.albumRatingTagName), avgRating.ToString(), true);
                CommitTagsToFile(currentFile, false, true);

                SetStatusbarTextForFileOperations(CarCommandSbText, false, j, tags.Count, currentFile);
            }

            RefreshPanels(true);

            SetResultingSbText();

            if (SavedSettings.notifyWhenCalculationCompleted) MessageBox.Show(this, MsgBackgroundTaskIsCompleted,
                string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        internal void calculateAlbumRatingForDisplayedTracks()
        {
            files = null;
            if (!MbApiInterface.Library_QueryFilesEx("domain=DisplayedFiles", out files))
                files = new string[0];

            switchOperation(calculateAlbumRating, buttonOK, EmptyButton, EmptyButton, buttonCancel, true, null);
        }

        internal void calculateAlbumRatingForAllTracks()
        {
            files = null;
            if (!MbApiInterface.Library_QueryFilesEx("domain=Library", out files))
                files = new string[0];

            switchOperation(calculateAlbumRating, buttonOK, EmptyButton, EmptyButton, EmptyButton, true, null);
        }

        internal static void CalculateAlbumRatingForAlbum(Plugin tagToolsPluginParam, string currentFile)
        {
            string[] localFiles = null;

            if (!MbApiInterface.Library_QueryFilesEx("domain=Library", out localFiles))
                localFiles = new string[0];

            string currentAlbumArtist = GetFileTag(currentFile, MetaDataType.AlbumArtist);
            string currentAlbum = GetFileTag(currentFile, MetaDataType.Album);

            List<string[]> tags = new List<string[]>();
            string[] row;
            string file;

            for (int fileCounter = 0; fileCounter < localFiles.Length; fileCounter++)
            {
                file = localFiles[fileCounter];

                string albumArtist = GetFileTag(file, MetaDataType.AlbumArtist);
                string album = GetFileTag(file, MetaDataType.Album);
                string rating = GetFileTag(file, GetTagId(SavedSettings.trackRatingTagName), true);

                if (currentAlbumArtist == albumArtist && currentAlbum == album)
                {
                    row = new string[4];

                    row[0] = albumArtist;
                    row[1] = album;
                    row[2] = rating;
                    row[3] = file;

                    tags.Add(row);
                }
            }


            double sumRating;
            int numberOfTracks;
            double avgRating;

            sumRating = 0;
            numberOfTracks = 0;

            for (int j = 0; j < tags.Count; j++)
            {
                if (string.Empty + tags[j][2] != string.Empty || SavedSettings.considerUnrated)
                {
                    sumRating += ConvertStrings(tags[j][2], ResultType.Double, DataType.Rating).resultD;
                    numberOfTracks++;
                }
            }

            if (numberOfTracks == 0)
                avgRating = 0;
            else
                avgRating = Math.Round(sumRating / 10 / numberOfTracks) * 10;

            for (int j = 0; j < tags.Count; j++)
            {
                file = tags[j][3];

                SetFileTag(file, GetTagId(SavedSettings.albumRatingTagName), avgRating.ToString(), true);
                CommitTagsToFile(file, false, true);
            }

            RefreshPanels(true);
        }

        internal override void enableDisablePreviewOptionControls(bool enable, bool dontChangeDisabled = false)
        {
            foreach (var control in allControls)
            {
                if (control != buttonOK && control != buttonCancel)
                    control.Enable(enable);
            }
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

        private void saveSettings()
        {
            SavedSettings.calculateAlbumRatingAtStartUp = calculateAlbumRatingAtStartUpCheckBox.Checked;
            SavedSettings.calculateAlbumRatingAtTagsChanged = calculateAlbumRatingAtTagsChangedCheckBox.Checked;
            SavedSettings.notifyWhenCalculationCompleted = notifyWhenCalculationCompletedCheckBox.Checked;
            SavedSettings.considerUnrated = considerUnratedCheckBox.Checked;

            SavedSettings.trackRatingTagName = trackRatingTagListCustom.Text;
            SavedSettings.albumRatingTagName = albumRatingTagListCustom.Text;

            TagToolsPlugin.SaveSettings();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            saveSettings();
            calculateAlbumRatingForDisplayedTracks();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveSettings();
        }

        private void calculateAlbumRatingAtStartUpCheckBoxLabel_Click(object sender, EventArgs e)
        {
            calculateAlbumRatingAtStartUpCheckBox.Checked = !calculateAlbumRatingAtStartUpCheckBox.Checked;
        }

        private void considerUnratedCheckBoxLabel_Click(object sender, EventArgs e)
        {
            considerUnratedCheckBox.Checked = !considerUnratedCheckBox.Checked;
        }

        private void calculateAlbumRatingAtTagsChangedCheckBoxLabel_Click(object sender, EventArgs e)
        {
            calculateAlbumRatingAtTagsChangedCheckBox.Checked = !calculateAlbumRatingAtTagsChangedCheckBox.Checked;
        }

        private void notifyWhenCalculationCompletedCheckBoxLabel_Click(object sender, EventArgs e)
        {
            notifyWhenCalculationCompletedCheckBox.Checked = !notifyWhenCalculationCompletedCheckBox.Checked;
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            PluginQuickSettings settings = new PluginQuickSettings(TagToolsPlugin);
            Display(settings, true);
        }
    }
}
