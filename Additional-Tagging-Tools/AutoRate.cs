using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class AutoRateCommand : PluginWindowTemplate
    {
        private string[] files = new string[0];

        private static int NumberOfFiles;
        private decimal actSumOfPercentage;

        public AutoRateCommand()
        {
            InitializeComponent();
        }

        public AutoRateCommand(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            toolTip1.SetToolTip(this.autoRateAtStartUpCheckBox, Plugin.MsgThresholdsDescription);
            toolTip1.SetToolTip(this.autoRateOnTrackPropertiesCheckBox, Plugin.MsgThresholdsDescription);
            toolTip1.SetToolTip(this.threshold5Box, Plugin.MsgThresholdsDescription);
            toolTip1.SetToolTip(this.threshold45Box, Plugin.MsgThresholdsDescription);
            toolTip1.SetToolTip(this.threshold4Box, Plugin.MsgThresholdsDescription);
            toolTip1.SetToolTip(this.threshold35Box, Plugin.MsgThresholdsDescription);
            toolTip1.SetToolTip(this.threshold3Box, Plugin.MsgThresholdsDescription);
            toolTip1.SetToolTip(this.threshold25Box, Plugin.MsgThresholdsDescription);
            toolTip1.SetToolTip(this.threshold2Box, Plugin.MsgThresholdsDescription);
            toolTip1.SetToolTip(this.threshold15Box, Plugin.MsgThresholdsDescription);
            toolTip1.SetToolTip(this.threshold1Box, Plugin.MsgThresholdsDescription);
            toolTip1.SetToolTip(this.threshold05Box, Plugin.MsgThresholdsDescription);
            toolTip1.SetToolTip(this.buttonOK, Plugin.MsgThresholdsDescription);

            toolTip1.SetToolTip(this.calculateThresholdsAtStartUpCheckBox, Plugin.MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(this.perCent5UpDown, Plugin.MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(this.perCent45UpDown, Plugin.MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(this.perCent4UpDown, Plugin.MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(this.perCent35UpDown, Plugin.MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(this.perCent3UpDown, Plugin.MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(this.perCent25UpDown, Plugin.MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(this.perCent2UpDown, Plugin.MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(this.perCent15UpDown, Plugin.MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(this.perCent1UpDown, Plugin.MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(this.perCent05UpDown, Plugin.MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(this.buttonCalculate, Plugin.MsgAutoCalculationOfThresholdsDescription);


            autoRatingTagList.Items.Add(Plugin.MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Rating));
            autoRatingTagList.Items.Add(Plugin.MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.RatingAlbum));

            autoRatingTagList.Items.Add(Plugin.MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom1));
            autoRatingTagList.Items.Add(Plugin.MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom2));
            autoRatingTagList.Items.Add(Plugin.MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom3));
            autoRatingTagList.Items.Add(Plugin.MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom4));
            autoRatingTagList.Items.Add(Plugin.MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom5));
            autoRatingTagList.Items.Add(Plugin.MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom6));
            autoRatingTagList.Items.Add(Plugin.MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom7));
            autoRatingTagList.Items.Add(Plugin.MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom8));
            autoRatingTagList.Items.Add(Plugin.MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom9));
            autoRatingTagList.Text = Plugin.GetTagName(Plugin.SavedSettings.autoRateTagId);

            playsPerDayTagList.Items.Add(Plugin.MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom1));
            playsPerDayTagList.Items.Add(Plugin.MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom2));
            playsPerDayTagList.Items.Add(Plugin.MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom3));
            playsPerDayTagList.Items.Add(Plugin.MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom4));
            playsPerDayTagList.Items.Add(Plugin.MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom5));
            playsPerDayTagList.Items.Add(Plugin.MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom6));
            playsPerDayTagList.Items.Add(Plugin.MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom7));
            playsPerDayTagList.Items.Add(Plugin.MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom8));
            playsPerDayTagList.Items.Add(Plugin.MbApiInterface.Setting_GetFieldName(Plugin.MetaDataType.Custom9));
            playsPerDayTagList.Text = Plugin.GetTagName(Plugin.SavedSettings.playsPerDayTagId);

            storePlaysPerDayCheckBox.Checked = Plugin.SavedSettings.storePlaysPerDay;

            autoRateAtStartUpCheckBox.Checked = Plugin.SavedSettings.autoRateAtStartUp;
            notifyWhenAutoratingCompletedCheckBox.Checked = Plugin.SavedSettings.notifyWhenAutoratingCompleted;
            calculateThresholdsAtStartUpCheckBox.Checked = Plugin.SavedSettings.calculateThresholdsAtStartUp;
            autoRateOnTrackPropertiesCheckBox.Checked = Plugin.SavedSettings.autoRateOnTrackProperties;
            baseRatingTrackBar.Value = Plugin.SavedSettings.defaultRating;

            sinceAddedCheckBox.Checked = Plugin.SavedSettings.sinceAdded;

            checkBox5.Checked = Plugin.SavedSettings.checkBox5;
            threshold5Box.Text = ConvertDoubleToString(Plugin.SavedSettings.threshold5);
            checkBox45.Checked = Plugin.SavedSettings.checkBox45;
            threshold45Box.Text = ConvertDoubleToString(Plugin.SavedSettings.threshold45);
            checkBox4.Checked = Plugin.SavedSettings.checkBox4;
            threshold4Box.Text = ConvertDoubleToString(Plugin.SavedSettings.threshold4);
            checkBox35.Checked = Plugin.SavedSettings.checkBox35;
            threshold35Box.Text = ConvertDoubleToString(Plugin.SavedSettings.threshold35);
            checkBox3.Checked = Plugin.SavedSettings.checkBox3;
            threshold3Box.Text = ConvertDoubleToString(Plugin.SavedSettings.threshold3);
            checkBox25.Checked = Plugin.SavedSettings.checkBox25;
            threshold25Box.Text = ConvertDoubleToString(Plugin.SavedSettings.threshold25);
            checkBox2.Checked = Plugin.SavedSettings.checkBox2;
            threshold2Box.Text = ConvertDoubleToString(Plugin.SavedSettings.threshold2);
            checkBox15.Checked = Plugin.SavedSettings.checkBox15;
            threshold15Box.Text = ConvertDoubleToString(Plugin.SavedSettings.threshold15);
            checkBox1.Checked = Plugin.SavedSettings.checkBox1;
            threshold1Box.Text = ConvertDoubleToString(Plugin.SavedSettings.threshold1);
            checkBox05.Checked = Plugin.SavedSettings.checkBox05;
            threshold05Box.Text = ConvertDoubleToString(Plugin.SavedSettings.threshold05);

            perCent5UpDown.Value = Plugin.SavedSettings.perCent5;
            perCent45UpDown.Value = Plugin.SavedSettings.perCent45;
            perCent4UpDown.Value = Plugin.SavedSettings.perCent4;
            perCent35UpDown.Value = Plugin.SavedSettings.perCent35;
            perCent3UpDown.Value = Plugin.SavedSettings.perCent3;
            perCent25UpDown.Value = Plugin.SavedSettings.perCent25;
            perCent2UpDown.Value = Plugin.SavedSettings.perCent2;
            perCent15UpDown.Value = Plugin.SavedSettings.perCent15;
            perCent1UpDown.Value = Plugin.SavedSettings.perCent1;
            perCent05UpDown.Value = Plugin.SavedSettings.perCent05;

            getStatistics();
            calcActSumOfPercentage();

            perCentN_ValueChanged(perCent5UpDown, checkBox5, perCentLabel5, Plugin.SavedSettings.actualPerCent5);
            perCentN_ValueChanged(perCent45UpDown, checkBox45, perCentLabel45, Plugin.SavedSettings.actualPerCent45);
            perCentN_ValueChanged(perCent4UpDown, checkBox4, perCentLabel4, Plugin.SavedSettings.actualPerCent4);
            perCentN_ValueChanged(perCent35UpDown, checkBox35, perCentLabel35, Plugin.SavedSettings.actualPerCent35);
            perCentN_ValueChanged(perCent3UpDown, checkBox3, perCentLabel3, Plugin.SavedSettings.actualPerCent3);
            perCentN_ValueChanged(perCent25UpDown, checkBox25, perCentLabel25, Plugin.SavedSettings.actualPerCent25);
            perCentN_ValueChanged(perCent2UpDown, checkBox2, perCentLabel2, Plugin.SavedSettings.actualPerCent2);
            perCentN_ValueChanged(perCent15UpDown, checkBox15, perCentLabel15, Plugin.SavedSettings.actualPerCent15);
            perCentN_ValueChanged(perCent1UpDown, checkBox1, perCentLabel1, Plugin.SavedSettings.actualPerCent1);
            perCentN_ValueChanged(perCent05UpDown, checkBox05, perCentLabel05, Plugin.SavedSettings.actualPerCent05);
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

            if (Plugin.SavedSettings.actualPerCent5 != -1)
            {
                if (actSumOfPercentage == -1)
                    actSumOfPercentage = Plugin.SavedSettings.actualPerCent5;
                else
                    actSumOfPercentage += Plugin.SavedSettings.actualPerCent5;
            }

            if (Plugin.SavedSettings.actualPerCent45 != -1)
            {
                if (actSumOfPercentage == -1)
                    actSumOfPercentage = Plugin.SavedSettings.actualPerCent45;
                else
                    actSumOfPercentage += Plugin.SavedSettings.actualPerCent45;
            }

            if (Plugin.SavedSettings.actualPerCent4 != -1)
            {
                if (actSumOfPercentage == -1)
                    actSumOfPercentage = Plugin.SavedSettings.actualPerCent4;
                else
                    actSumOfPercentage += Plugin.SavedSettings.actualPerCent4;
            }

            if (Plugin.SavedSettings.actualPerCent35 != -1)
            {
                if (actSumOfPercentage == -1)
                    actSumOfPercentage = Plugin.SavedSettings.actualPerCent35;
                else
                    actSumOfPercentage += Plugin.SavedSettings.actualPerCent35;
            }

            if (Plugin.SavedSettings.actualPerCent3 != -1)
            {
                if (actSumOfPercentage == -1)
                    actSumOfPercentage = Plugin.SavedSettings.actualPerCent3;
                else
                    actSumOfPercentage += Plugin.SavedSettings.actualPerCent3;
            }

            if (Plugin.SavedSettings.actualPerCent25 != -1)
            {
                if (actSumOfPercentage == -1)
                    actSumOfPercentage = Plugin.SavedSettings.actualPerCent25;
                else
                    actSumOfPercentage += Plugin.SavedSettings.actualPerCent25;
            }

            if (Plugin.SavedSettings.actualPerCent2 != -1)
            {
                if (actSumOfPercentage == -1)
                    actSumOfPercentage = Plugin.SavedSettings.actualPerCent2;
                else
                    actSumOfPercentage += Plugin.SavedSettings.actualPerCent2;
            }

            if (Plugin.SavedSettings.actualPerCent15 != -1)
            {
                if (actSumOfPercentage == -1)
                    actSumOfPercentage = Plugin.SavedSettings.actualPerCent15;
                else
                    actSumOfPercentage += Plugin.SavedSettings.actualPerCent15;
            }

            if (Plugin.SavedSettings.actualPerCent1 != -1)
            {
                if (actSumOfPercentage == -1)
                    actSumOfPercentage = Plugin.SavedSettings.actualPerCent1;
                else
                    actSumOfPercentage += Plugin.SavedSettings.actualPerCent1;
            }

            if (Plugin.SavedSettings.actualPerCent05 != -1)
            {
                if (actSumOfPercentage == -1)
                    actSumOfPercentage = Plugin.SavedSettings.actualPerCent05;
                else
                    actSumOfPercentage += Plugin.SavedSettings.actualPerCent05;
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
            Plugin.MetaDataType autoRateTagId = Plugin.SavedSettings.autoRateTagId;
            Plugin.MetaDataType playsPerDayTagId = Plugin.SavedSettings.playsPerDayTagId;

            int autoRating;
            double playsPerDay = GetPlaysPerDay(tagToolsPluginParam, currentFile);

            if (playsPerDay == -1)
                autoRating = Plugin.SavedSettings.defaultRating;
            else
            {
                if (playsPerDay >= Plugin.SavedSettings.threshold5 && Plugin.SavedSettings.checkBox5)
                    autoRating = 10;
                else if (playsPerDay >= Plugin.SavedSettings.threshold45 && Plugin.SavedSettings.checkBox45)
                    autoRating = 9;
                else if (playsPerDay >= Plugin.SavedSettings.threshold4 && Plugin.SavedSettings.checkBox4)
                    autoRating = 8;
                else if (playsPerDay >= Plugin.SavedSettings.threshold35 && Plugin.SavedSettings.checkBox35)
                    autoRating = 7;
                else if (playsPerDay >= Plugin.SavedSettings.threshold3 && Plugin.SavedSettings.checkBox3)
                    autoRating = 6;
                else if (playsPerDay >= Plugin.SavedSettings.threshold25 && Plugin.SavedSettings.checkBox25)
                    autoRating = 5;
                else if (playsPerDay >= Plugin.SavedSettings.threshold2 && Plugin.SavedSettings.checkBox2)
                    autoRating = 4;
                else if (playsPerDay >= Plugin.SavedSettings.threshold15 && Plugin.SavedSettings.checkBox15)
                    autoRating = 3;
                else if (playsPerDay >= Plugin.SavedSettings.threshold1 && Plugin.SavedSettings.checkBox1)
                    autoRating = 2;
                else if (playsPerDay >= Plugin.SavedSettings.threshold05 && Plugin.SavedSettings.checkBox05)
                    autoRating = 1;
                else
                    autoRating = 0;
            }

            Plugin.SetFileTag(currentFile, autoRateTagId, (10 * autoRating).ToString(), true);

            if (Plugin.SavedSettings.storePlaysPerDay)
                Plugin.SetFileTag(currentFile, playsPerDayTagId, ConvertDoubleToString(playsPerDay));

            Plugin.CommitTagsToFile(currentFile, false, true);

            Plugin.RefreshPanels(true);
        }

        public void autoRateOnStartup()
        {
            string currentFile;

            files = null;
            if (!Plugin.MbApiInterface.Library_QueryFilesEx("domain=Library", out files))
                files = new string[0];

            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                currentFile = files[fileCounter];

                Plugin.SetStatusbarTextForFileOperations(Plugin.AutoRateCommandSbText, false, fileCounter, files.Length, currentFile);

                AutoRateLive(TagToolsPlugin, currentFile);
            }

            Plugin.RefreshPanels(true);

            Plugin.SetStatusbarTextForFileOperations(Plugin.AutoRateCommandSbText, false, files.Length - 1, files.Length, null, true);

            if (Plugin.SavedSettings.notifyWhenAutoratingCompleted) MessageBox.Show(this, Plugin.MsgBackgroundTaskIsCompleted, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool prepareBackgroundTask()
        {
            if (backgroundTaskIsWorking())
                return true;

            files = null;
            if (!Plugin.MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files))
                files = new string[0];

            if (files.Length == 0)
            {
                MessageBox.Show(this, Plugin.MsgNoFilesSelected, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

                Plugin.SetStatusbarTextForFileOperations(Plugin.AutoRateCommandSbText, false, fileCounter, files.Length, currentFile);

                AutoRateLive(TagToolsPlugin, currentFile);
            }

            Plugin.SetStatusbarTextForFileOperations(Plugin.AutoRateCommandSbText, false, files.Length - 1, files.Length, null, true);
        }

        private static double GetPlaysPerDay(Plugin tagToolsPlugin, string currentFile)
        {
            double daysSinceAdded = 0;
            double daysSinceLastPlayed = 0;

            int played = Convert.ToInt32(Plugin.MbApiInterface.Library_GetFileProperty(currentFile, Plugin.FilePropertyType.PlayCount));
            int skipped = Convert.ToInt32(Plugin.MbApiInterface.Library_GetFileProperty(currentFile, Plugin.FilePropertyType.SkipCount));
            double derivedPlayed = played - skipped;

            if (derivedPlayed < 0)
                derivedPlayed = 0;

            try { daysSinceAdded = (DateTime.Parse("" + Plugin.MbApiInterface.Library_GetFileProperty(currentFile, Plugin.FilePropertyType.DateAdded)) - DateTime.Now).TotalDays; }
            catch (FormatException) { daysSinceAdded = 0; }

            try
            {
                if (Plugin.SavedSettings.sinceAdded)
                    daysSinceLastPlayed = 0;
                else
                    daysSinceLastPlayed = (DateTime.Parse("" + Plugin.MbApiInterface.Library_GetFileProperty(currentFile, Plugin.FilePropertyType.LastPlayed)) - DateTime.Now).TotalDays;
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
            if (!Plugin.MbApiInterface.Library_QueryFilesEx("domain=Library", out files))
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
            labelTotalTracks.Text = Plugin.MsgNumberOfPlayedTracks + NumberOfFiles;
        }

        public void onStartup()
        {
            if (Plugin.SavedSettings.calculateThresholdsAtStartUp) calculateThresholds();
            if (Plugin.SavedSettings.autoRateAtStartUp) autoRateOnStartup();
        }

        public void calculateThresholds()
        {
            string currentFile;

            NumberOfFiles = 0;
            SortedDictionary<double, int> playsPerDayStatistics = new SortedDictionary<double, int>();

            files = null;
            if (!Plugin.MbApiInterface.Library_QueryFilesEx("domain=Library", out files))
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

            Plugin.SavedSettings.threshold5 = 0;
            Plugin.SavedSettings.threshold45 = 0;
            Plugin.SavedSettings.threshold4 = 0;
            Plugin.SavedSettings.threshold35 = 0;
            Plugin.SavedSettings.threshold3 = 0;
            Plugin.SavedSettings.threshold25 = 0;
            Plugin.SavedSettings.threshold2 = 0;
            Plugin.SavedSettings.threshold15 = 0;
            Plugin.SavedSettings.threshold1 = 0;
            Plugin.SavedSettings.threshold05 = 0;

            Plugin.SavedSettings.actualPerCent5 = -1;
            Plugin.SavedSettings.actualPerCent45 = -1;
            Plugin.SavedSettings.actualPerCent4 = -1;
            Plugin.SavedSettings.actualPerCent35 = -1;
            Plugin.SavedSettings.actualPerCent3 = -1;
            Plugin.SavedSettings.actualPerCent25 = -1;
            Plugin.SavedSettings.actualPerCent2 = -1;
            Plugin.SavedSettings.actualPerCent15 = -1;
            Plugin.SavedSettings.actualPerCent1 = -1;
            Plugin.SavedSettings.actualPerCent05 = -1;

            int statisticsSum = 0;
            int assignedFilesNumber = 0;

            foreach (double playsPerDay in playsPerDayStatistics.Keys)
            {
                if (backgroundTaskIsCanceled)
                    return;

                int statistics;
                playsPerDayStatistics.TryGetValue(playsPerDay, out statistics);
                statisticsSum += statistics;

                if (Plugin.SavedSettings.perCent5 != 0 && Plugin.SavedSettings.threshold5 == 0)
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= Plugin.SavedSettings.perCent5)
                    {
                        Plugin.SavedSettings.actualPerCent5 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        Plugin.SavedSettings.threshold5 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (Plugin.SavedSettings.perCent45 != 0 && Plugin.SavedSettings.threshold45 == 0)
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= Plugin.SavedSettings.perCent45)
                    {
                        Plugin.SavedSettings.actualPerCent45 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        Plugin.SavedSettings.threshold45 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (Plugin.SavedSettings.perCent4 != 0 && Plugin.SavedSettings.threshold4 == 0)
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= Plugin.SavedSettings.perCent4)
                    {
                        Plugin.SavedSettings.actualPerCent4 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        Plugin.SavedSettings.threshold4 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (Plugin.SavedSettings.perCent35 != 0 && Plugin.SavedSettings.threshold35 == 0)
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= Plugin.SavedSettings.perCent35)
                    {
                        Plugin.SavedSettings.actualPerCent35 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        Plugin.SavedSettings.threshold35 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (Plugin.SavedSettings.perCent3 != 0 && Plugin.SavedSettings.threshold3 == 0)
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= Plugin.SavedSettings.perCent3)
                    {
                        Plugin.SavedSettings.actualPerCent3 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        Plugin.SavedSettings.threshold3 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (Plugin.SavedSettings.perCent25 != 0 && Plugin.SavedSettings.threshold25 == 0)
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= Plugin.SavedSettings.perCent25)
                    {
                        Plugin.SavedSettings.actualPerCent25 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        Plugin.SavedSettings.threshold25 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (Plugin.SavedSettings.perCent2 != 0 && Plugin.SavedSettings.threshold2 == 0)
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= Plugin.SavedSettings.perCent2)
                    {
                        Plugin.SavedSettings.actualPerCent2 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        Plugin.SavedSettings.threshold2 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (Plugin.SavedSettings.perCent15 != 0 && Plugin.SavedSettings.threshold15 == 0)
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= Plugin.SavedSettings.perCent15)
                    {
                        Plugin.SavedSettings.actualPerCent15 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        Plugin.SavedSettings.threshold15 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (Plugin.SavedSettings.perCent1 != 0 && Plugin.SavedSettings.threshold1 == 0)
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= Plugin.SavedSettings.perCent1)
                    {
                        Plugin.SavedSettings.actualPerCent1 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        Plugin.SavedSettings.threshold1 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (Plugin.SavedSettings.perCent05 != 0 && Plugin.SavedSettings.threshold05 == 0)
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= Plugin.SavedSettings.perCent05)
                    {
                        Plugin.SavedSettings.actualPerCent05 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        Plugin.SavedSettings.threshold05 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
            }

            if (NumberOfFiles > assignedFilesNumber)
            {
                if (Plugin.SavedSettings.perCent5 != 0 && Plugin.SavedSettings.threshold5 == 0)
                {
                    Plugin.SavedSettings.actualPerCent5 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //Plugin.savedSettings.threshold5 = 0;
                }
                else if (Plugin.SavedSettings.perCent45 != 0 && Plugin.SavedSettings.threshold45 == 0)
                {
                    Plugin.SavedSettings.actualPerCent45 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //Plugin.savedSettings.threshold45 = 0;
                }
                else if (Plugin.SavedSettings.perCent4 != 0 && Plugin.SavedSettings.threshold4 == 0)
                {
                    Plugin.SavedSettings.actualPerCent4 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //Plugin.savedSettings.threshold4 = 0;
                }
                else if (Plugin.SavedSettings.perCent35 != 0 && Plugin.SavedSettings.threshold35 == 0)
                {
                    Plugin.SavedSettings.actualPerCent35 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //Plugin.savedSettings.threshold35 = 0;
                }
                else if (Plugin.SavedSettings.perCent3 != 0 && Plugin.SavedSettings.threshold3 == 0)
                {
                    Plugin.SavedSettings.actualPerCent3 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //Plugin.savedSettings.threshold3 = 0;
                }
                else if (Plugin.SavedSettings.perCent25 != 0 && Plugin.SavedSettings.threshold25 == 0)
                {
                    Plugin.SavedSettings.actualPerCent25 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //Plugin.savedSettings.threshold25 = 0;
                }
                else if (Plugin.SavedSettings.perCent2 != 0 && Plugin.SavedSettings.threshold2 == 0)
                {
                    Plugin.SavedSettings.actualPerCent2 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //Plugin.savedSettings.threshold2 = 0;
                }
                else if (Plugin.SavedSettings.perCent15 != 0 && Plugin.SavedSettings.threshold15 == 0)
                {
                    Plugin.SavedSettings.actualPerCent15 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //Plugin.savedSettings.threshold15 = 0;
                }
                else if (Plugin.SavedSettings.perCent1 != 0 && Plugin.SavedSettings.threshold1 == 0)
                {
                    Plugin.SavedSettings.actualPerCent1 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //Plugin.savedSettings.threshold1 = 0;
                }
                else if (Plugin.SavedSettings.perCent05 != 0 && Plugin.SavedSettings.threshold05 == 0)
                {
                    Plugin.SavedSettings.actualPerCent05 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //Plugin.savedSettings.threshold05 = 0;
                }
            }
        }

        private bool checkSettings()
        {
            if (sumOfPercentage() > 100)
            {
                MessageBox.Show(this, Plugin.MsgIncorrectSumOfWeights, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        private bool saveSettings()
        {
            if (!checkSettings())
                return false;

            Plugin.SavedSettings.autoRateTagId = Plugin.GetTagId(autoRatingTagList.Text);
            Plugin.SavedSettings.storePlaysPerDay = storePlaysPerDayCheckBox.Checked;
            Plugin.SavedSettings.playsPerDayTagId = Plugin.GetTagId(playsPerDayTagList.Text);

            Plugin.SavedSettings.autoRateAtStartUp = autoRateAtStartUpCheckBox.Checked;
            Plugin.SavedSettings.notifyWhenAutoratingCompleted = notifyWhenAutoratingCompletedCheckBox.Checked;
            Plugin.SavedSettings.calculateThresholdsAtStartUp = calculateThresholdsAtStartUpCheckBox.Checked;
            Plugin.SavedSettings.autoRateOnTrackProperties = autoRateOnTrackPropertiesCheckBox.Checked;
            Plugin.SavedSettings.defaultRating = baseRatingTrackBar.Value;

            Plugin.SavedSettings.sinceAdded = sinceAddedCheckBox.Checked;

            Plugin.SavedSettings.checkBox5 = checkBox5.Checked;
            Plugin.SavedSettings.threshold5 = RoundPlaysPerDay(threshold5Box.Text);
            Plugin.SavedSettings.checkBox45 = checkBox45.Checked;
            Plugin.SavedSettings.threshold45 = RoundPlaysPerDay(threshold45Box.Text);
            Plugin.SavedSettings.checkBox4 = checkBox4.Checked;
            Plugin.SavedSettings.threshold4 = RoundPlaysPerDay(threshold4Box.Text);
            Plugin.SavedSettings.checkBox35 = checkBox35.Checked;
            Plugin.SavedSettings.threshold35 = RoundPlaysPerDay(threshold35Box.Text);
            Plugin.SavedSettings.checkBox3 = checkBox3.Checked;
            Plugin.SavedSettings.threshold3 = RoundPlaysPerDay(threshold3Box.Text);
            Plugin.SavedSettings.checkBox25 = checkBox25.Checked;
            Plugin.SavedSettings.threshold25 = RoundPlaysPerDay(threshold25Box.Text);
            Plugin.SavedSettings.checkBox2 = checkBox2.Checked;
            Plugin.SavedSettings.threshold2 = RoundPlaysPerDay(threshold2Box.Text);
            Plugin.SavedSettings.checkBox15 = checkBox15.Checked;
            Plugin.SavedSettings.threshold15 = RoundPlaysPerDay(threshold15Box.Text);
            Plugin.SavedSettings.checkBox1 = checkBox1.Checked;
            Plugin.SavedSettings.threshold1 = RoundPlaysPerDay(threshold1Box.Text);
            Plugin.SavedSettings.checkBox05 = checkBox05.Checked;
            Plugin.SavedSettings.threshold05 = RoundPlaysPerDay(threshold05Box.Text);

            Plugin.SavedSettings.perCent5 = perCent5UpDown.Value;
            Plugin.SavedSettings.perCent45 = perCent45UpDown.Value;
            Plugin.SavedSettings.perCent4 = perCent4UpDown.Value;
            Plugin.SavedSettings.perCent35 = perCent35UpDown.Value;
            Plugin.SavedSettings.perCent3 = perCent3UpDown.Value;
            Plugin.SavedSettings.perCent25 = perCent25UpDown.Value;
            Plugin.SavedSettings.perCent2 = perCent2UpDown.Value;
            Plugin.SavedSettings.perCent15 = perCent15UpDown.Value;
            Plugin.SavedSettings.perCent1 = perCent1UpDown.Value;
            Plugin.SavedSettings.perCent05 = perCent05UpDown.Value;

            return true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (checkSettings())
            {
                if (prepareBackgroundTask())
                    switchOperation(autoRateNow, (Button)sender, Plugin.EmptyButton, Plugin.EmptyButton, buttonCancel, true, null);
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
            threshold.Enabled = checkBox.Checked;
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

        private void checkBoxFourAndHalf_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold45Box, checkBox45, perCent45UpDown);
        }

        private void checkBoxFour_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold4Box, checkBox4, perCent4UpDown);
        }

        private void checkBoxThreeAndHalf_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold35Box, checkBox35, perCent35UpDown);
        }

        private void checkBoxThree_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold3Box, checkBox3, perCent3UpDown);
        }

        private void checkBoxTwoAndHalf_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold25Box, checkBox25, perCent25UpDown);
        }

        private void checkBoxTwo_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold2Box, checkBox2, perCent2UpDown);
        }

        private void checkBoxOneAndHalf_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold15Box, checkBox15, perCent15UpDown);
        }

        private void checkBoxOne_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold1Box, checkBox1, perCent1UpDown);
        }

        private void checkBoxHalf_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold05Box, checkBox05, perCent05UpDown);
        }

        private void perCentN_ValueChanged(NumericUpDown perCent, CheckBox checkBox, Label perCentLabel, decimal actualPerCent)
        {
            if (perCent.Value == 0)
                checkBox.Checked = false;
            else
                checkBox.Checked = true;

            if (actSumOfPercentage == -1)
            {
                labelSum.Text = Plugin.MsgSum + sumOfPercentage() + "% (" + (100 - sumOfPercentage()) + Plugin.MsgNumberOfNotRatedTracks;
            }
            else
            {
                labelSum.Text = Plugin.MsgSum + sumOfPercentage() + Plugin.MsgActualPercent + actSumOfPercentage + "% (" + (100 - actSumOfPercentage) + Plugin.MsgNumberOfNotRatedTracks;
            }

            if (actualPerCent == -1)
            {
                perCentLabel.Text = "% (~" + Math.Round(NumberOfFiles * perCent.Value / 100, 0) + Plugin.MsgTracks;
                Font font = new Font(perCentLabel.Font, FontStyle.Regular);
                perCentLabel.Font = font;
            }
            else
            {
                perCentLabel.Text = Plugin.MsgActualPercent + actualPerCent + "% (~" + Math.Round(NumberOfFiles * actualPerCent / 100, 0) + Plugin.MsgTracks;
                Font font = new Font(perCentLabel.Font, FontStyle.Bold);
                perCentLabel.Font = font;
            }
        }

        private void perCent5_ValueChanged(object sender, EventArgs e)
        {
            perCentN_ValueChanged(perCent5UpDown, checkBox5, perCentLabel5, Plugin.SavedSettings.actualPerCent5);
        }

        private void perCent45_ValueChanged(object sender, EventArgs e)
        {
            perCentN_ValueChanged(perCent45UpDown, checkBox45, perCentLabel45, Plugin.SavedSettings.actualPerCent45);
        }

        private void perCent4_ValueChanged(object sender, EventArgs e)
        {
            perCentN_ValueChanged(perCent4UpDown, checkBox4, perCentLabel4, Plugin.SavedSettings.actualPerCent4);
        }

        private void perCent35_ValueChanged(object sender, EventArgs e)
        {
            perCentN_ValueChanged(perCent35UpDown, checkBox35, perCentLabel35, Plugin.SavedSettings.actualPerCent35);
        }

        private void perCent3_ValueChanged(object sender, EventArgs e)
        {
            perCentN_ValueChanged(perCent3UpDown, checkBox3, perCentLabel3, Plugin.SavedSettings.actualPerCent3);
        }

        private void perCent25_ValueChanged(object sender, EventArgs e)
        {
            perCentN_ValueChanged(perCent25UpDown, checkBox25, perCentLabel25, Plugin.SavedSettings.actualPerCent25);
        }

        private void perCent2_ValueChanged(object sender, EventArgs e)
        {
            perCentN_ValueChanged(perCent2UpDown, checkBox2, perCentLabel2, Plugin.SavedSettings.actualPerCent2);
        }

        private void perCent15_ValueChanged(object sender, EventArgs e)
        {
            perCentN_ValueChanged(perCent15UpDown, checkBox15, perCentLabel15, Plugin.SavedSettings.actualPerCent15);
        }

        private void perCent1_ValueChanged(object sender, EventArgs e)
        {
            perCentN_ValueChanged(perCent1UpDown, checkBox1, perCentLabel1, Plugin.SavedSettings.actualPerCent1);
        }

        private void perCent05_ValueChanged(object sender, EventArgs e)
        {
            perCentN_ValueChanged(perCent05UpDown, checkBox05, perCentLabel05, Plugin.SavedSettings.actualPerCent05);
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            if (!saveSettings())
                return;

            calculateThresholds();
            calcActSumOfPercentage();

            threshold5Box.Text = ConvertDoubleToString(Plugin.SavedSettings.threshold5);
            threshold45Box.Text = ConvertDoubleToString(Plugin.SavedSettings.threshold45);
            threshold4Box.Text = ConvertDoubleToString(Plugin.SavedSettings.threshold4);
            threshold35Box.Text = ConvertDoubleToString(Plugin.SavedSettings.threshold35);
            threshold3Box.Text = ConvertDoubleToString(Plugin.SavedSettings.threshold3);
            threshold25Box.Text = ConvertDoubleToString(Plugin.SavedSettings.threshold25);
            threshold2Box.Text = ConvertDoubleToString(Plugin.SavedSettings.threshold2);
            threshold15Box.Text = ConvertDoubleToString(Plugin.SavedSettings.threshold15);
            threshold1Box.Text = ConvertDoubleToString(Plugin.SavedSettings.threshold1);
            threshold05Box.Text = ConvertDoubleToString(Plugin.SavedSettings.threshold05);

            perCentN_ValueChanged(perCent5UpDown, checkBox5, perCentLabel5, Plugin.SavedSettings.actualPerCent5);
            perCentN_ValueChanged(perCent45UpDown, checkBox45, perCentLabel45, Plugin.SavedSettings.actualPerCent45);
            perCentN_ValueChanged(perCent4UpDown, checkBox4, perCentLabel4, Plugin.SavedSettings.actualPerCent4);
            perCentN_ValueChanged(perCent35UpDown, checkBox35, perCentLabel35, Plugin.SavedSettings.actualPerCent35);
            perCentN_ValueChanged(perCent3UpDown, checkBox3, perCentLabel3, Plugin.SavedSettings.actualPerCent3);
            perCentN_ValueChanged(perCent25UpDown, checkBox25, perCentLabel25, Plugin.SavedSettings.actualPerCent25);
            perCentN_ValueChanged(perCent2UpDown, checkBox2, perCentLabel2, Plugin.SavedSettings.actualPerCent2);
            perCentN_ValueChanged(perCent15UpDown, checkBox15, perCentLabel15, Plugin.SavedSettings.actualPerCent15);
            perCentN_ValueChanged(perCent1UpDown, checkBox1, perCentLabel1, Plugin.SavedSettings.actualPerCent1);
            perCentN_ValueChanged(perCent05UpDown, checkBox05, perCentLabel05, Plugin.SavedSettings.actualPerCent05);
        }

        private void threshold_TextChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).Text = ConvertDoubleToString(RoundPlaysPerDay(((TextBox)sender).Text));
        }

        private void autoRateAtStartUp_CheckedChanged(object sender, EventArgs e)
        {
            notifyWhenAutoratingCompletedCheckBox.Enabled = autoRateAtStartUpCheckBox.Checked;
        }

        private void storePlaysPerDay_CheckedChanged(object sender, EventArgs e)
        {
            playsPerDayTagList.Enabled = storePlaysPerDayCheckBox.Checked;
        }

        public override void enableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonCalculate, " ");
            dirtyErrorProvider.SetError(buttonCalculate, String.Empty);
        }

        public override void disableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonCalculate, getBackgroundTasksWarning());
        }

        public override void enableQueryingOrUpdatingButtons()
        {
            buttonOK.Enabled = true;
            buttonCalculate.Enabled = true;
        }

        public override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enabled = false;
            buttonCalculate.Enabled = false;
        }
    }
}
