﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;
using ExtensionMethods;

namespace MusicBeePlugin
{
    public partial class AutoRateCommand : PluginWindowTemplate
    {
        private string[] files = new string[0];

        private static int NumberOfFiles;
        private decimal actSumOfPercentage;

        public AutoRateCommand(Plugin tagToolsPluginParam) : base(tagToolsPluginParam)
        {
            InitializeComponent();
            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            toolTip1.SetToolTip(this.autoRateAtStartUpCheckBox, MsgThresholdsDescription);
            toolTip1.SetToolTip(this.autoRateAtStartUpCheckBoxLabel, MsgThresholdsDescription);
            toolTip1.SetToolTip(this.autoRateOnTrackPropertiesCheckBox, MsgThresholdsDescription);
            toolTip1.SetToolTip(this.threshold5Box, MsgThresholdsDescription);
            toolTip1.SetToolTip(this.threshold45Box, MsgThresholdsDescription);
            toolTip1.SetToolTip(this.threshold4Box, MsgThresholdsDescription);
            toolTip1.SetToolTip(this.threshold35Box, MsgThresholdsDescription);
            toolTip1.SetToolTip(this.threshold3Box, MsgThresholdsDescription);
            toolTip1.SetToolTip(this.threshold25Box, MsgThresholdsDescription);
            toolTip1.SetToolTip(this.threshold2Box, MsgThresholdsDescription);
            toolTip1.SetToolTip(this.threshold15Box, MsgThresholdsDescription);
            toolTip1.SetToolTip(this.threshold1Box, MsgThresholdsDescription);
            toolTip1.SetToolTip(this.threshold05Box, MsgThresholdsDescription);
            toolTip1.SetToolTip(this.buttonOK, MsgThresholdsDescription);

            toolTip1.SetToolTip(this.calculateThresholdsAtStartUpCheckBox, MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(this.holdsAtStartUpCheckBoxLabel, MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(this.perCent5UpDown, MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(this.perCent45UpDown, MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(this.perCent4UpDown, MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(this.perCent35UpDown, MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(this.perCent3UpDown, MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(this.perCent25UpDown, MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(this.perCent2UpDown, MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(this.perCent15UpDown, MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(this.perCent1UpDown, MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(this.perCent05UpDown, MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(this.buttonCalculate, MsgAutoCalculationOfThresholdsDescription);


            autoRatingTagList.Items.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Rating));
            autoRatingTagList.Items.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.RatingAlbum));

            autoRatingTagList.Items.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom1));
            autoRatingTagList.Items.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom2));
            autoRatingTagList.Items.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom3));
            autoRatingTagList.Items.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom4));
            autoRatingTagList.Items.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom5));
            autoRatingTagList.Items.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom6));
            autoRatingTagList.Items.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom7));
            autoRatingTagList.Items.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom8));
            autoRatingTagList.Items.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom9));
            autoRatingTagList.Text = GetTagName(SavedSettings.autoRateTagId);

            playsPerDayTagList.Items.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom1));
            playsPerDayTagList.Items.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom2));
            playsPerDayTagList.Items.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom3));
            playsPerDayTagList.Items.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom4));
            playsPerDayTagList.Items.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom5));
            playsPerDayTagList.Items.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom6));
            playsPerDayTagList.Items.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom7));
            playsPerDayTagList.Items.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom8));
            playsPerDayTagList.Items.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom9));
            playsPerDayTagList.Text = GetTagName(SavedSettings.playsPerDayTagId);

            storePlaysPerDayCheckBox.Checked = SavedSettings.storePlaysPerDay;

            autoRateAtStartUpCheckBox.Checked = SavedSettings.autoRateAtStartUp;
            notifyWhenAutoratingCompletedCheckBox.Checked = SavedSettings.notifyWhenAutoratingCompleted;
            calculateThresholdsAtStartUpCheckBox.Checked = SavedSettings.calculateThresholdsAtStartUp;
            autoRateOnTrackPropertiesCheckBox.Checked = SavedSettings.autoRateOnTrackProperties;
            baseRatingTrackBar.Value = SavedSettings.defaultRating;

            sinceAddedCheckBox.Checked = SavedSettings.sinceAdded;

            checkBox5.Checked = SavedSettings.checkBox5;
            threshold5Box.Text = ConvertDoubleToString(SavedSettings.threshold5);
            checkBox45.Checked = SavedSettings.checkBox45;
            threshold45Box.Text = ConvertDoubleToString(SavedSettings.threshold45);
            checkBox4.Checked = SavedSettings.checkBox4;
            threshold4Box.Text = ConvertDoubleToString(SavedSettings.threshold4);
            checkBox35.Checked = SavedSettings.checkBox35;
            threshold35Box.Text = ConvertDoubleToString(SavedSettings.threshold35);
            checkBox3.Checked = SavedSettings.checkBox3;
            threshold3Box.Text = ConvertDoubleToString(SavedSettings.threshold3);
            checkBox25.Checked = SavedSettings.checkBox25;
            threshold25Box.Text = ConvertDoubleToString(SavedSettings.threshold25);
            checkBox2.Checked = SavedSettings.checkBox2;
            threshold2Box.Text = ConvertDoubleToString(SavedSettings.threshold2);
            checkBox15.Checked = SavedSettings.checkBox15;
            threshold15Box.Text = ConvertDoubleToString(SavedSettings.threshold15);
            checkBox1.Checked = SavedSettings.checkBox1;
            threshold1Box.Text = ConvertDoubleToString(SavedSettings.threshold1);
            checkBox05.Checked = SavedSettings.checkBox05;
            threshold05Box.Text = ConvertDoubleToString(SavedSettings.threshold05);

            perCent5UpDown.Value = SavedSettings.perCent5;
            perCent45UpDown.Value = SavedSettings.perCent45;
            perCent4UpDown.Value = SavedSettings.perCent4;
            perCent35UpDown.Value = SavedSettings.perCent35;
            perCent3UpDown.Value = SavedSettings.perCent3;
            perCent25UpDown.Value = SavedSettings.perCent25;
            perCent2UpDown.Value = SavedSettings.perCent2;
            perCent15UpDown.Value = SavedSettings.perCent15;
            perCent1UpDown.Value = SavedSettings.perCent1;
            perCent05UpDown.Value = SavedSettings.perCent05;

            getStatistics();
            calcActSumOfPercentage();

            perCentN_ValueChanged(perCent5UpDown, checkBox5, perCentLabel5, SavedSettings.actualPerCent5);
            perCentN_ValueChanged(perCent45UpDown, checkBox45, perCentLabel45, SavedSettings.actualPerCent45);
            perCentN_ValueChanged(perCent4UpDown, checkBox4, perCentLabel4, SavedSettings.actualPerCent4);
            perCentN_ValueChanged(perCent35UpDown, checkBox35, perCentLabel35, SavedSettings.actualPerCent35);
            perCentN_ValueChanged(perCent3UpDown, checkBox3, perCentLabel3, SavedSettings.actualPerCent3);
            perCentN_ValueChanged(perCent25UpDown, checkBox25, perCentLabel25, SavedSettings.actualPerCent25);
            perCentN_ValueChanged(perCent2UpDown, checkBox2, perCentLabel2, SavedSettings.actualPerCent2);
            perCentN_ValueChanged(perCent15UpDown, checkBox15, perCentLabel15, SavedSettings.actualPerCent15);
            perCentN_ValueChanged(perCent1UpDown, checkBox1, perCentLabel1, SavedSettings.actualPerCent1);
            perCentN_ValueChanged(perCent05UpDown, checkBox05, perCentLabel05, SavedSettings.actualPerCent05);
        }

        private decimal sumOfPercentage()
        {
            decimal sumOfPercentageVariable = 0;

            sumOfPercentageVariable += perCent5UpDown.Value;
            sumOfPercentageVariable += perCent45UpDown.Value;
            sumOfPercentageVariable += perCent4UpDown.Value;
            sumOfPercentageVariable += perCent35UpDown.Value;
            sumOfPercentageVariable += perCent3UpDown.Value;
            sumOfPercentageVariable += perCent25UpDown.Value;
            sumOfPercentageVariable += perCent2UpDown.Value;
            sumOfPercentageVariable += perCent15UpDown.Value;
            sumOfPercentageVariable += perCent1UpDown.Value;
            sumOfPercentageVariable += perCent05UpDown.Value;

            return sumOfPercentageVariable;
        }

        private void calcActSumOfPercentage()
        {
            actSumOfPercentage = -1;

            if (SavedSettings.actualPerCent5 != -1)
            {
                if (actSumOfPercentage == -1)
                    actSumOfPercentage = SavedSettings.actualPerCent5;
                else
                    actSumOfPercentage += SavedSettings.actualPerCent5;
            }

            if (SavedSettings.actualPerCent45 != -1)
            {
                if (actSumOfPercentage == -1)
                    actSumOfPercentage = SavedSettings.actualPerCent45;
                else
                    actSumOfPercentage += SavedSettings.actualPerCent45;
            }

            if (SavedSettings.actualPerCent4 != -1)
            {
                if (actSumOfPercentage == -1)
                    actSumOfPercentage = SavedSettings.actualPerCent4;
                else
                    actSumOfPercentage += SavedSettings.actualPerCent4;
            }

            if (SavedSettings.actualPerCent35 != -1)
            {
                if (actSumOfPercentage == -1)
                    actSumOfPercentage = SavedSettings.actualPerCent35;
                else
                    actSumOfPercentage += SavedSettings.actualPerCent35;
            }

            if (SavedSettings.actualPerCent3 != -1)
            {
                if (actSumOfPercentage == -1)
                    actSumOfPercentage = SavedSettings.actualPerCent3;
                else
                    actSumOfPercentage += SavedSettings.actualPerCent3;
            }

            if (SavedSettings.actualPerCent25 != -1)
            {
                if (actSumOfPercentage == -1)
                    actSumOfPercentage = SavedSettings.actualPerCent25;
                else
                    actSumOfPercentage += SavedSettings.actualPerCent25;
            }

            if (SavedSettings.actualPerCent2 != -1)
            {
                if (actSumOfPercentage == -1)
                    actSumOfPercentage = SavedSettings.actualPerCent2;
                else
                    actSumOfPercentage += SavedSettings.actualPerCent2;
            }

            if (SavedSettings.actualPerCent15 != -1)
            {
                if (actSumOfPercentage == -1)
                    actSumOfPercentage = SavedSettings.actualPerCent15;
                else
                    actSumOfPercentage += SavedSettings.actualPerCent15;
            }

            if (SavedSettings.actualPerCent1 != -1)
            {
                if (actSumOfPercentage == -1)
                    actSumOfPercentage = SavedSettings.actualPerCent1;
                else
                    actSumOfPercentage += SavedSettings.actualPerCent1;
            }

            if (SavedSettings.actualPerCent05 != -1)
            {
                if (actSumOfPercentage == -1)
                    actSumOfPercentage = SavedSettings.actualPerCent05;
                else
                    actSumOfPercentage += SavedSettings.actualPerCent05;
            }

        }

        private static double RoundPlaysPerDay(double value)
        {
            return Math.Round(value, 12);
        }

        private static double RoundPlaysPerDay(string value)
        {
            double temp;

            try
            {
                temp = Convert.ToDouble(value);
            }
            catch (FormatException)
            {
                temp = 0;
            }

            return RoundPlaysPerDay(temp);
        }

        private static string ConvertDoubleToString(double value)
        {
            return RoundPlaysPerDay(value).ToString();
        }

        public static void AutoRateLive(Plugin tagToolsPluginParam, string currentFile)
        {
            MetaDataType autoRateTagId = SavedSettings.autoRateTagId;
            MetaDataType playsPerDayTagId = SavedSettings.playsPerDayTagId;

            int autoRating;
            double playsPerDay = GetPlaysPerDay(tagToolsPluginParam, currentFile);

            if (playsPerDay == -1)
                autoRating = SavedSettings.defaultRating;
            else
            {
                if (playsPerDay >= SavedSettings.threshold5 && SavedSettings.checkBox5)
                    autoRating = 10;
                else if (playsPerDay >= SavedSettings.threshold45 && SavedSettings.checkBox45)
                    autoRating = 9;
                else if (playsPerDay >= SavedSettings.threshold4 && SavedSettings.checkBox4)
                    autoRating = 8;
                else if (playsPerDay >= SavedSettings.threshold35 && SavedSettings.checkBox35)
                    autoRating = 7;
                else if (playsPerDay >= SavedSettings.threshold3 && SavedSettings.checkBox3)
                    autoRating = 6;
                else if (playsPerDay >= SavedSettings.threshold25 && SavedSettings.checkBox25)
                    autoRating = 5;
                else if (playsPerDay >= SavedSettings.threshold2 && SavedSettings.checkBox2)
                    autoRating = 4;
                else if (playsPerDay >= SavedSettings.threshold15 && SavedSettings.checkBox15)
                    autoRating = 3;
                else if (playsPerDay >= SavedSettings.threshold1 && SavedSettings.checkBox1)
                    autoRating = 2;
                else if (playsPerDay >= SavedSettings.threshold05 && SavedSettings.checkBox05)
                    autoRating = 1;
                else
                    autoRating = 0;
            }

            SetFileTag(currentFile, autoRateTagId, (10 * autoRating).ToString(), true);

            if (SavedSettings.storePlaysPerDay)
                SetFileTag(currentFile, playsPerDayTagId, ConvertDoubleToString(playsPerDay));

            CommitTagsToFile(currentFile, false, true);

            RefreshPanels(true);
        }

        public void autoRateOnStartup()
        {
            string currentFile;

            files = null;
            if (!MbApiInterface.Library_QueryFilesEx("domain=Library", out files))
                files = new string[0];

            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                currentFile = files[fileCounter];

                SetStatusbarTextForFileOperations(AutoRateCommandSbText, false, fileCounter, files.Length, currentFile);

                AutoRateLive(TagToolsPlugin, currentFile);
            }

            RefreshPanels(true);

            SetResultingSbText();

            if (SavedSettings.notifyWhenAutoratingCompleted) MessageBox.Show(this, MsgBackgroundTaskIsCompleted, "", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool prepareBackgroundTask()
        {
            if (backgroundTaskIsWorking())
                return true;

            files = null;
            if (!MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files))
                files = new string[0];

            if (files.Length == 0)
            {
                MessageBox.Show(this, MsgNoFilesSelected, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void autoRateNow()
        {
            string currentFile;

            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                currentFile = files[fileCounter];

                SetStatusbarTextForFileOperations(AutoRateCommandSbText, false, fileCounter, files.Length, currentFile);

                AutoRateLive(TagToolsPlugin, currentFile);
            }

            SetResultingSbText();
        }

        private static double GetPlaysPerDay(Plugin tagToolsPlugin, string currentFile)
        {
            double daysSinceAdded = 0;
            double daysSinceLastPlayed = 0;

            int played = Convert.ToInt32(MbApiInterface.Library_GetFileProperty(currentFile, FilePropertyType.PlayCount));
            int skipped = Convert.ToInt32(MbApiInterface.Library_GetFileProperty(currentFile, FilePropertyType.SkipCount));
            double derivedPlayed = played - skipped;

            if (derivedPlayed < 0)
                derivedPlayed = 0;

            try { daysSinceAdded = (DateTime.Parse("" + MbApiInterface.Library_GetFileProperty(currentFile, FilePropertyType.DateAdded)) - DateTime.Now).TotalDays; }
            catch (FormatException) { daysSinceAdded = 0; }

            try
            {
                if (SavedSettings.sinceAdded)
                    daysSinceLastPlayed = 0;
                else
                    daysSinceLastPlayed = (DateTime.Parse("" + MbApiInterface.Library_GetFileProperty(currentFile, FilePropertyType.LastPlayed)) - DateTime.Now).TotalDays;
            }
            catch (FormatException) { daysSinceLastPlayed = 0; }

            double daysPlayed = daysSinceLastPlayed - daysSinceAdded;

            if (daysPlayed < 0)
                daysPlayed = 0;

            if (daysPlayed != 0 && played != 0)
                return RoundPlaysPerDay(derivedPlayed / daysPlayed);
            else
                return -1;
        }

        private void getStatistics()
        {
            string currentFile;

            double totalPlaysPerDay = 0;
            NumberOfFiles = 0;

            double maxPlaysPerDay = 0;
            double avgPlaysPerDay = 0;

            files = null;
            if (!MbApiInterface.Library_QueryFilesEx("domain=Library", out files))
                files = new string[0];

            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                currentFile = files[fileCounter];

                double playsPerDay = GetPlaysPerDay(TagToolsPlugin, currentFile);

                if (playsPerDay != -1)
                {
                    NumberOfFiles++;
                    totalPlaysPerDay += playsPerDay;

                    if (maxPlaysPerDay < playsPerDay)
                        maxPlaysPerDay = playsPerDay;
                }
            }

            if (NumberOfFiles != 0)
            {
                avgPlaysPerDay = totalPlaysPerDay / NumberOfFiles;
            }

            maxPlaysPerDayBox.Text = ConvertDoubleToString(maxPlaysPerDay);
            avgPlaysPerDayBox.Text = ConvertDoubleToString(avgPlaysPerDay);
            labelTotalTracks.Text = MsgNumberOfPlayedTracks + NumberOfFiles;
        }

        public void onStartup()
        {
            if (SavedSettings.calculateThresholdsAtStartUp) calculateThresholds();
            if (SavedSettings.autoRateAtStartUp) autoRateOnStartup();
        }

        public void calculateThresholds()
        {
            string currentFile;

            NumberOfFiles = 0;
            SortedDictionary<double, int> playsPerDayStatistics = new SortedDictionary<double, int>();

            files = null;
            if (!MbApiInterface.Library_QueryFilesEx("domain=Library", out files))
                files = new string[0];

            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                currentFile = files[fileCounter];

                double playsPerDay = GetPlaysPerDay(TagToolsPlugin, currentFile);

                if (playsPerDay != -1)
                {
                    NumberOfFiles++;

                    int statistics;
                    playsPerDayStatistics.TryGetValue(-playsPerDay, out statistics);

                    if (statistics != 0)
                        playsPerDayStatistics.Remove(-playsPerDay);

                    playsPerDayStatistics.Add(-playsPerDay, ++statistics);
                }
            }

            SavedSettings.threshold5 = 0;
            SavedSettings.threshold45 = 0;
            SavedSettings.threshold4 = 0;
            SavedSettings.threshold35 = 0;
            SavedSettings.threshold3 = 0;
            SavedSettings.threshold25 = 0;
            SavedSettings.threshold2 = 0;
            SavedSettings.threshold15 = 0;
            SavedSettings.threshold1 = 0;
            SavedSettings.threshold05 = 0;

            SavedSettings.actualPerCent5 = -1;
            SavedSettings.actualPerCent45 = -1;
            SavedSettings.actualPerCent4 = -1;
            SavedSettings.actualPerCent35 = -1;
            SavedSettings.actualPerCent3 = -1;
            SavedSettings.actualPerCent25 = -1;
            SavedSettings.actualPerCent2 = -1;
            SavedSettings.actualPerCent15 = -1;
            SavedSettings.actualPerCent1 = -1;
            SavedSettings.actualPerCent05 = -1;

            int statisticsSum = 0;
            int assignedFilesNumber = 0;

            foreach (double playsPerDay in playsPerDayStatistics.Keys)
            {
                if (backgroundTaskIsCanceled)
                    return;

                int statistics;
                playsPerDayStatistics.TryGetValue(playsPerDay, out statistics);
                statisticsSum += statistics;

                if (SavedSettings.perCent5 != 0 && SavedSettings.threshold5 == 0)
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= SavedSettings.perCent5)
                    {
                        SavedSettings.actualPerCent5 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        SavedSettings.threshold5 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (SavedSettings.perCent45 != 0 && SavedSettings.threshold45 == 0)
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= SavedSettings.perCent45)
                    {
                        SavedSettings.actualPerCent45 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        SavedSettings.threshold45 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (SavedSettings.perCent4 != 0 && SavedSettings.threshold4 == 0)
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= SavedSettings.perCent4)
                    {
                        SavedSettings.actualPerCent4 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        SavedSettings.threshold4 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (SavedSettings.perCent35 != 0 && SavedSettings.threshold35 == 0)
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= SavedSettings.perCent35)
                    {
                        SavedSettings.actualPerCent35 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        SavedSettings.threshold35 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (SavedSettings.perCent3 != 0 && SavedSettings.threshold3 == 0)
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= SavedSettings.perCent3)
                    {
                        SavedSettings.actualPerCent3 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        SavedSettings.threshold3 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (SavedSettings.perCent25 != 0 && SavedSettings.threshold25 == 0)
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= SavedSettings.perCent25)
                    {
                        SavedSettings.actualPerCent25 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        SavedSettings.threshold25 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (SavedSettings.perCent2 != 0 && SavedSettings.threshold2 == 0)
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= SavedSettings.perCent2)
                    {
                        SavedSettings.actualPerCent2 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        SavedSettings.threshold2 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (SavedSettings.perCent15 != 0 && SavedSettings.threshold15 == 0)
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= SavedSettings.perCent15)
                    {
                        SavedSettings.actualPerCent15 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        SavedSettings.threshold15 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (SavedSettings.perCent1 != 0 && SavedSettings.threshold1 == 0)
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= SavedSettings.perCent1)
                    {
                        SavedSettings.actualPerCent1 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        SavedSettings.threshold1 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (SavedSettings.perCent05 != 0 && SavedSettings.threshold05 == 0)
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= SavedSettings.perCent05)
                    {
                        SavedSettings.actualPerCent05 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        SavedSettings.threshold05 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
            }

            if (NumberOfFiles > assignedFilesNumber)
            {
                if (SavedSettings.perCent5 != 0 && SavedSettings.threshold5 == 0)
                {
                    SavedSettings.actualPerCent5 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //Plugin.savedSettings.threshold5 = 0;
                }
                else if (SavedSettings.perCent45 != 0 && SavedSettings.threshold45 == 0)
                {
                    SavedSettings.actualPerCent45 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //Plugin.savedSettings.threshold45 = 0;
                }
                else if (SavedSettings.perCent4 != 0 && SavedSettings.threshold4 == 0)
                {
                    SavedSettings.actualPerCent4 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //Plugin.savedSettings.threshold4 = 0;
                }
                else if (SavedSettings.perCent35 != 0 && SavedSettings.threshold35 == 0)
                {
                    SavedSettings.actualPerCent35 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //Plugin.savedSettings.threshold35 = 0;
                }
                else if (SavedSettings.perCent3 != 0 && SavedSettings.threshold3 == 0)
                {
                    SavedSettings.actualPerCent3 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //Plugin.savedSettings.threshold3 = 0;
                }
                else if (SavedSettings.perCent25 != 0 && SavedSettings.threshold25 == 0)
                {
                    SavedSettings.actualPerCent25 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //Plugin.savedSettings.threshold25 = 0;
                }
                else if (SavedSettings.perCent2 != 0 && SavedSettings.threshold2 == 0)
                {
                    SavedSettings.actualPerCent2 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //Plugin.savedSettings.threshold2 = 0;
                }
                else if (SavedSettings.perCent15 != 0 && SavedSettings.threshold15 == 0)
                {
                    SavedSettings.actualPerCent15 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //Plugin.savedSettings.threshold15 = 0;
                }
                else if (SavedSettings.perCent1 != 0 && SavedSettings.threshold1 == 0)
                {
                    SavedSettings.actualPerCent1 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //Plugin.savedSettings.threshold1 = 0;
                }
                else if (SavedSettings.perCent05 != 0 && SavedSettings.threshold05 == 0)
                {
                    SavedSettings.actualPerCent05 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //Plugin.savedSettings.threshold05 = 0;
                }
            }
        }

        private bool checkSettings()
        {
            if (sumOfPercentage() > 100)
            {
                MessageBox.Show(this, MsgIncorrectSumOfWeights, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        private bool saveSettings()
        {
            if (!checkSettings())
                return false;

            SavedSettings.autoRateTagId = GetTagId(autoRatingTagList.Text);
            SavedSettings.storePlaysPerDay = storePlaysPerDayCheckBox.Checked;
            SavedSettings.playsPerDayTagId = GetTagId(playsPerDayTagList.Text);

            SavedSettings.autoRateAtStartUp = autoRateAtStartUpCheckBox.Checked;
            SavedSettings.notifyWhenAutoratingCompleted = notifyWhenAutoratingCompletedCheckBox.Checked;
            SavedSettings.calculateThresholdsAtStartUp = calculateThresholdsAtStartUpCheckBox.Checked;
            SavedSettings.autoRateOnTrackProperties = autoRateOnTrackPropertiesCheckBox.Checked;
            SavedSettings.defaultRating = baseRatingTrackBar.Value;

            SavedSettings.sinceAdded = sinceAddedCheckBox.Checked;

            SavedSettings.checkBox5 = checkBox5.Checked;
            SavedSettings.threshold5 = RoundPlaysPerDay(threshold5Box.Text);
            SavedSettings.checkBox45 = checkBox45.Checked;
            SavedSettings.threshold45 = RoundPlaysPerDay(threshold45Box.Text);
            SavedSettings.checkBox4 = checkBox4.Checked;
            SavedSettings.threshold4 = RoundPlaysPerDay(threshold4Box.Text);
            SavedSettings.checkBox35 = checkBox35.Checked;
            SavedSettings.threshold35 = RoundPlaysPerDay(threshold35Box.Text);
            SavedSettings.checkBox3 = checkBox3.Checked;
            SavedSettings.threshold3 = RoundPlaysPerDay(threshold3Box.Text);
            SavedSettings.checkBox25 = checkBox25.Checked;
            SavedSettings.threshold25 = RoundPlaysPerDay(threshold25Box.Text);
            SavedSettings.checkBox2 = checkBox2.Checked;
            SavedSettings.threshold2 = RoundPlaysPerDay(threshold2Box.Text);
            SavedSettings.checkBox15 = checkBox15.Checked;
            SavedSettings.threshold15 = RoundPlaysPerDay(threshold15Box.Text);
            SavedSettings.checkBox1 = checkBox1.Checked;
            SavedSettings.threshold1 = RoundPlaysPerDay(threshold1Box.Text);
            SavedSettings.checkBox05 = checkBox05.Checked;
            SavedSettings.threshold05 = RoundPlaysPerDay(threshold05Box.Text);

            SavedSettings.perCent5 = perCent5UpDown.Value;
            SavedSettings.perCent45 = perCent45UpDown.Value;
            SavedSettings.perCent4 = perCent4UpDown.Value;
            SavedSettings.perCent35 = perCent35UpDown.Value;
            SavedSettings.perCent3 = perCent3UpDown.Value;
            SavedSettings.perCent25 = perCent25UpDown.Value;
            SavedSettings.perCent2 = perCent2UpDown.Value;
            SavedSettings.perCent15 = perCent15UpDown.Value;
            SavedSettings.perCent1 = perCent1UpDown.Value;
            SavedSettings.perCent05 = perCent05UpDown.Value;

            return true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (checkSettings())
            {
                if (prepareBackgroundTask())
                    switchOperation(autoRateNow, (Button)sender, EmptyButton, EmptyButton, buttonCancel, true, null);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveSettings();
        }

        private void checkBoxN_CheckedChanged(TextBox threshold, CheckBox checkBox, NumericUpDown perCent)
        {
            threshold.Enable(checkBox.Checked);
            if (checkBox.Checked)
            {
                if (perCent.Value == 0)
                {
                    perCent.Value = 1;
                }
            }
            else
            {
                perCent.Value = 0;
            }
        }

        private void checkBoxFive_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold5Box, checkBox5, perCent5UpDown);
        }

        private void checkBox5Label_Click(object sender, EventArgs e)
        {
            checkBox5.Checked = !checkBox5.Checked;
            checkBoxFive_CheckedChanged(null, null);
        }

        private void checkBoxFourAndHalf_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold45Box, checkBox45, perCent45UpDown);
        }

        private void checkBox45Label_Click(object sender, EventArgs e)
        {
            checkBox45.Checked = !checkBox45.Checked;
            checkBoxFourAndHalf_CheckedChanged(null, null);
        }

        private void checkBoxFour_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold4Box, checkBox4, perCent4UpDown);
        }

        private void checkBox4Label_Click(object sender, EventArgs e)
        {
            checkBox4.Checked = !checkBox4.Checked;
            checkBoxFour_CheckedChanged(null, null);
        }

        private void checkBoxThreeAndHalf_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold35Box, checkBox35, perCent35UpDown);
        }

        private void checkBox35Label_Click(object sender, EventArgs e)
        {
            checkBox35.Checked = !checkBox35.Checked;
            checkBoxThreeAndHalf_CheckedChanged(null, null);
        }

        private void checkBoxThree_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold3Box, checkBox3, perCent3UpDown);
        }

        private void checkBox3Label_Click(object sender, EventArgs e)
        {
            checkBox3.Checked = !checkBox3.Checked;
            checkBoxThree_CheckedChanged(null, null);
        }

        private void checkBoxTwoAndHalf_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold25Box, checkBox25, perCent25UpDown);
        }

        private void checkBox25Label_Click(object sender, EventArgs e)
        {
            checkBox25.Checked = !checkBox25.Checked;
            checkBoxTwoAndHalf_CheckedChanged(null, null);
        }

        private void checkBoxTwo_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold2Box, checkBox2, perCent2UpDown);
        }

        private void checkBox2Label_Click(object sender, EventArgs e)
        {
            checkBox2.Checked = !checkBox2.Checked;
            checkBoxTwo_CheckedChanged(null, null);
        }

        private void checkBoxOneAndHalf_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold15Box, checkBox15, perCent15UpDown);
        }

        private void checkBox15Label_Click(object sender, EventArgs e)
        {
            checkBox15.Checked = !checkBox15.Checked;
            checkBoxOneAndHalf_CheckedChanged(null , null);
        }

        private void checkBoxOne_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold1Box, checkBox1, perCent1UpDown);
        }

        private void checkBox1Label_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = !checkBox1.Checked;
            checkBoxOne_CheckedChanged(null, null);
        }

        private void checkBoxHalf_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold05Box, checkBox05, perCent05UpDown);
        }

        private void checkBox05Label_Click(object sender, EventArgs e)
        {
            checkBox05.Checked = !checkBox05.Checked;
            checkBoxHalf_CheckedChanged(null, null);
        }

        private void perCentN_ValueChanged(NumericUpDown perCent, CheckBox checkBox, Label perCentLabel, decimal actualPerCent)
        {
            if (perCent.Value == 0)
                checkBox.Checked = false;
            else
                checkBox.Checked = true;

            if (actSumOfPercentage == -1)
            {
                labelSum.Text = MsgSum + sumOfPercentage() + "% (" + (100 - sumOfPercentage()) + MsgNumberOfNotRatedTracks;
            }
            else
            {
                labelSum.Text = MsgSum + sumOfPercentage() + MsgActualPercent + actSumOfPercentage + "% (" + (100 - actSumOfPercentage) + MsgNumberOfNotRatedTracks;
            }

            if (actualPerCent == -1)
            {
                perCentLabel.Text = "% (~" + Math.Round(NumberOfFiles * perCent.Value / 100, 0) + MsgTracks;
                Font font = new Font(perCentLabel.Font, FontStyle.Regular);
                perCentLabel.Font = font;
            }
            else
            {
                perCentLabel.Text = MsgActualPercent + actualPerCent + "% (~" + Math.Round(NumberOfFiles * actualPerCent / 100, 0) + MsgTracks;
                Font font = new Font(perCentLabel.Font, FontStyle.Bold);
                perCentLabel.Font = font;
            }
        }

        private void perCent5_ValueChanged(object sender, EventArgs e)
        {
            perCentN_ValueChanged(perCent5UpDown, checkBox5, perCentLabel5, SavedSettings.actualPerCent5);
        }

        private void perCent45_ValueChanged(object sender, EventArgs e)
        {
            perCentN_ValueChanged(perCent45UpDown, checkBox45, perCentLabel45, SavedSettings.actualPerCent45);
        }

        private void perCent4_ValueChanged(object sender, EventArgs e)
        {
            perCentN_ValueChanged(perCent4UpDown, checkBox4, perCentLabel4, SavedSettings.actualPerCent4);
        }

        private void perCent35_ValueChanged(object sender, EventArgs e)
        {
            perCentN_ValueChanged(perCent35UpDown, checkBox35, perCentLabel35, SavedSettings.actualPerCent35);
        }

        private void perCent3_ValueChanged(object sender, EventArgs e)
        {
            perCentN_ValueChanged(perCent3UpDown, checkBox3, perCentLabel3, SavedSettings.actualPerCent3);
        }

        private void perCent25_ValueChanged(object sender, EventArgs e)
        {
            perCentN_ValueChanged(perCent25UpDown, checkBox25, perCentLabel25, SavedSettings.actualPerCent25);
        }

        private void perCent2_ValueChanged(object sender, EventArgs e)
        {
            perCentN_ValueChanged(perCent2UpDown, checkBox2, perCentLabel2, SavedSettings.actualPerCent2);
        }

        private void perCent15_ValueChanged(object sender, EventArgs e)
        {
            perCentN_ValueChanged(perCent15UpDown, checkBox15, perCentLabel15, SavedSettings.actualPerCent15);
        }

        private void perCent1_ValueChanged(object sender, EventArgs e)
        {
            perCentN_ValueChanged(perCent1UpDown, checkBox1, perCentLabel1, SavedSettings.actualPerCent1);
        }

        private void perCent05_ValueChanged(object sender, EventArgs e)
        {
            perCentN_ValueChanged(perCent05UpDown, checkBox05, perCentLabel05, SavedSettings.actualPerCent05);
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            if (!saveSettings())
                return;

            calculateThresholds();
            calcActSumOfPercentage();

            threshold5Box.Text = ConvertDoubleToString(SavedSettings.threshold5);
            threshold45Box.Text = ConvertDoubleToString(SavedSettings.threshold45);
            threshold4Box.Text = ConvertDoubleToString(SavedSettings.threshold4);
            threshold35Box.Text = ConvertDoubleToString(SavedSettings.threshold35);
            threshold3Box.Text = ConvertDoubleToString(SavedSettings.threshold3);
            threshold25Box.Text = ConvertDoubleToString(SavedSettings.threshold25);
            threshold2Box.Text = ConvertDoubleToString(SavedSettings.threshold2);
            threshold15Box.Text = ConvertDoubleToString(SavedSettings.threshold15);
            threshold1Box.Text = ConvertDoubleToString(SavedSettings.threshold1);
            threshold05Box.Text = ConvertDoubleToString(SavedSettings.threshold05);

            perCentN_ValueChanged(perCent5UpDown, checkBox5, perCentLabel5, SavedSettings.actualPerCent5);
            perCentN_ValueChanged(perCent45UpDown, checkBox45, perCentLabel45, SavedSettings.actualPerCent45);
            perCentN_ValueChanged(perCent4UpDown, checkBox4, perCentLabel4, SavedSettings.actualPerCent4);
            perCentN_ValueChanged(perCent35UpDown, checkBox35, perCentLabel35, SavedSettings.actualPerCent35);
            perCentN_ValueChanged(perCent3UpDown, checkBox3, perCentLabel3, SavedSettings.actualPerCent3);
            perCentN_ValueChanged(perCent25UpDown, checkBox25, perCentLabel25, SavedSettings.actualPerCent25);
            perCentN_ValueChanged(perCent2UpDown, checkBox2, perCentLabel2, SavedSettings.actualPerCent2);
            perCentN_ValueChanged(perCent15UpDown, checkBox15, perCentLabel15, SavedSettings.actualPerCent15);
            perCentN_ValueChanged(perCent1UpDown, checkBox1, perCentLabel1, SavedSettings.actualPerCent1);
            perCentN_ValueChanged(perCent05UpDown, checkBox05, perCentLabel05, SavedSettings.actualPerCent05);
        }

        private void threshold_TextChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).Text = ConvertDoubleToString(RoundPlaysPerDay(((TextBox)sender).Text));
        }

        private void autoRateAtStartUp_CheckedChanged(object sender, EventArgs e)
        {
            notifyWhenAutoratingCompletedCheckBox.Enable(autoRateAtStartUpCheckBox.Checked);
        }

        private void autoRateAtStartUpCheckBoxLabel_Click(object sender, EventArgs e)
        {
            autoRateAtStartUpCheckBox.Checked = !autoRateAtStartUpCheckBox.Checked;
            autoRateAtStartUp_CheckedChanged(null, null);
        }

        private void storePlaysPerDay_CheckedChanged(object sender, EventArgs e)
        {
            playsPerDayTagList.Enable(storePlaysPerDayCheckBox.Checked);
        }

        private void storePlaysPerDayCheckBoxLabel_Click(object sender, EventArgs e)
        {
            storePlaysPerDayCheckBox.Checked = !storePlaysPerDayCheckBox.Checked;
            storePlaysPerDay_CheckedChanged(null, null);
        }

        public override void enableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonCalculate, " ");
            dirtyErrorProvider.SetError(buttonCalculate, string.Empty);
        }

        public override void disableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonCalculate, getBackgroundTasksWarning());
        }

        public override void enableQueryingOrUpdatingButtons()
        {
            buttonOK.Enable(true);
            buttonCalculate.Enable(true);
        }

        public override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enable(false);
            buttonCalculate.Enable(false);
        }

        private void holdsAtStartUpCheckBoxLabel_Click(object sender, EventArgs e)
        {
            calculateThresholdsAtStartUpCheckBox.Checked = !calculateThresholdsAtStartUpCheckBox.Checked;
        }

        private void autoRateOnTrackPropertiesCheckBoxLabel_Click(object sender, EventArgs e)
        {
            autoRateOnTrackPropertiesCheckBox.Checked = !autoRateOnTrackPropertiesCheckBox.Checked;
        }

        private void sinceAddedCheckBoxLabel_Click(object sender, EventArgs e)
        {
            sinceAddedCheckBox.Checked = !sinceAddedCheckBox.Checked;
        }

        private void notifyWhenAutoratingCompletedCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!notifyWhenAutoratingCompletedCheckBox.Enabled)
                return;

            notifyWhenAutoratingCompletedCheckBox.Checked = !notifyWhenAutoratingCompletedCheckBox.Checked;
        }
    }
}
