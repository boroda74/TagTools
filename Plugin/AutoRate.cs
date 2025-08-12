using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using ExtensionMethods;

using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class AutoRate : PluginWindowTemplate
    {
        private CustomComboBox autoRatingTagListCustom;
        private CustomComboBox playsPerDayTagListCustom;


        bool formIsOpening;

        private string[] files = Array.Empty<string>();

        private static int NumberOfFiles;
        private decimal actualSumOfPercentages;

        private bool calculateActualSumOfPercentageOnCalculatingThresholds = false;

        private System.Threading.Timer statisticsTimer;

        public AutoRate(Plugin plugin) : base(plugin)
        {
            InitializeComponent();

            WindowMenuIcon = AutorateMenuIcon;
            WindowIcon = AutorateIcon;
            WindowIconInactive = AutorateIconInactive;
            TitleBarText = this.Text;

            new ControlBorder(this.threshold05Box);
            new ControlBorder(this.threshold1Box);
            new ControlBorder(this.threshold15Box);
            new ControlBorder(this.threshold2Box);
            new ControlBorder(this.threshold25Box);
            new ControlBorder(this.threshold3Box);
            new ControlBorder(this.threshold35Box);
            new ControlBorder(this.threshold4Box);
            new ControlBorder(this.threshold45Box);
            new ControlBorder(this.threshold5Box);
            new ControlBorder(this.maxPlaysPerDayBox);
            new ControlBorder(this.avgPlaysPerDayBox);

            new ControlBorder(this.perCent05UpDown);
            new ControlBorder(this.perCent1UpDown);
            new ControlBorder(this.perCent15UpDown);
            new ControlBorder(this.perCent2UpDown);
            new ControlBorder(this.perCent25UpDown);
            new ControlBorder(this.perCent3UpDown);
            new ControlBorder(this.perCent35UpDown);
            new ControlBorder(this.perCent4UpDown);
            new ControlBorder(this.perCent45UpDown);
            new ControlBorder(this.perCent5UpDown);
        }

        internal static void AutoRateOnStartup(Plugin plugin)
        {
            if (SavedSettings.calculateThresholdsAtStartUp || SavedSettings.autoRateAtStartUp)
            {
                using (var tagToolsForm = new AutoRate(plugin))
                    tagToolsForm.switchOperation(tagToolsForm.onStartup, EmptyButton, EmptyButton, EmptyButton, EmptyButton, true, null);
            }

            if (SavedSettings.calculateAlbumRatingAtStartUp)
            {
                using (var tagToolsForm = new CalculateAverageAlbumRating(plugin))
                    tagToolsForm.calculateAlbumRatingForAllTracks();
            }
        }

        internal protected override void initializeForm()
        {
            base.initializeForm();

            autoRatingTagListCustom = namesComboBoxes["autoRatingTagList"];
            playsPerDayTagListCustom = namesComboBoxes["playsPerDayTagList"];


            ReplaceButtonBitmap(buttonSettings, Gear);


            toolTip1.SetToolTip(autoRateAtStartUpCheckBox, MsgThresholdsDescription);
            toolTip1.SetToolTip(autoRateAtStartUpCheckBoxLabel, MsgThresholdsDescription);
            toolTip1.SetToolTip(autoRateOnTrackPropertiesCheckBox, MsgThresholdsDescription);
            toolTip1.SetToolTip(threshold5Box, MsgThresholdsDescription);
            toolTip1.SetToolTip(threshold45Box, MsgThresholdsDescription);
            toolTip1.SetToolTip(threshold4Box, MsgThresholdsDescription);
            toolTip1.SetToolTip(threshold35Box, MsgThresholdsDescription);
            toolTip1.SetToolTip(threshold3Box, MsgThresholdsDescription);
            toolTip1.SetToolTip(threshold25Box, MsgThresholdsDescription);
            toolTip1.SetToolTip(threshold2Box, MsgThresholdsDescription);
            toolTip1.SetToolTip(threshold15Box, MsgThresholdsDescription);
            toolTip1.SetToolTip(threshold1Box, MsgThresholdsDescription);
            toolTip1.SetToolTip(threshold05Box, MsgThresholdsDescription);
            toolTip1.SetToolTip(buttonOK, MsgThresholdsDescription);

            toolTip1.SetToolTip(calculateThresholdsAtStartUpCheckBox, MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(holdsAtStartUpCheckBoxLabel, MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(perCent5UpDown, MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(perCent45UpDown, MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(perCent4UpDown, MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(perCent35UpDown, MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(perCent3UpDown, MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(perCent25UpDown, MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(perCent2UpDown, MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(perCent15UpDown, MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(perCent1UpDown, MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(perCent05UpDown, MsgAutoCalculationOfThresholdsDescription);
            toolTip1.SetToolTip(buttonCalculate, MsgAutoCalculationOfThresholdsDescription);


            FillListByTagNames(autoRatingTagListCustom.Items, false, false, false, false, false, false, true);
            autoRatingTagListCustom.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Rating));
            autoRatingTagListCustom.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.RatingAlbum));
            autoRatingTagListCustom.Text = GetTagName(SavedSettings.autoRateTagId);


            FillListByTagNames(playsPerDayTagListCustom.Items, false, false, false, false, false, false, true);
            playsPerDayTagListCustom.Text = GetTagName(SavedSettings.playsPerDayTagId);


            formIsOpening = true;


            storePlaysPerDayCheckBox.Checked = SavedSettings.storePlaysPerDay;
            autoRateAtStartUpCheckBox.Checked = SavedSettings.autoRateAtStartUp;
            notifyWhenAutoRatingCompletedCheckBox.Checked = SavedSettings.notifyWhenAutoRatingCompleted;
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

            maxPlaysPerDayBox.Text = CtlAutoRateCalculating;
            avgPlaysPerDayBox.Text = CtlAutoRateCalculating;
            labelTotalTracks.Text = MsgNumberOfPlayedTracks + CtlAutoRateCalculating.ToLower();


            formIsOpening = false;

            decimal fullPercentageSum = sumOfPercentages();

            if (fullPercentageSum == 100)
            {
                calculateThresholdsAtStartUpCheckBox.Enable(true);
                buttonCalculate.Enable(true);
                buttonNormalizePercentages.Enable(false);
            }
            else
            {
                calculateThresholdsAtStartUpCheckBox.Enable(false);
            }


            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        internal protected bool checkStoppingCollectingStatisticsStatus()
        {
            if (PluginClosing)
                return true;
            else if (backgroundTaskIsStopping || backgroundTaskIsStoppedOrCancelled)
                return true;
            else
                return false;
        }

        private decimal sumOfPercentages()
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

        private void calculateActualSumOfPercentages()
        {
            actualSumOfPercentages = -1;

            if (SavedSettings.actualPerCent5 != -1)
            {
                if (actualSumOfPercentages == -1)
                    actualSumOfPercentages = SavedSettings.actualPerCent5;
                else
                    actualSumOfPercentages += SavedSettings.actualPerCent5;
            }

            if (SavedSettings.actualPerCent45 != -1)
            {
                if (actualSumOfPercentages == -1)
                    actualSumOfPercentages = SavedSettings.actualPerCent45;
                else
                    actualSumOfPercentages += SavedSettings.actualPerCent45;
            }

            if (SavedSettings.actualPerCent4 != -1)
            {
                if (actualSumOfPercentages == -1)
                    actualSumOfPercentages = SavedSettings.actualPerCent4;
                else
                    actualSumOfPercentages += SavedSettings.actualPerCent4;
            }

            if (SavedSettings.actualPerCent35 != -1)
            {
                if (actualSumOfPercentages == -1)
                    actualSumOfPercentages = SavedSettings.actualPerCent35;
                else
                    actualSumOfPercentages += SavedSettings.actualPerCent35;
            }

            if (SavedSettings.actualPerCent3 != -1)
            {
                if (actualSumOfPercentages == -1)
                    actualSumOfPercentages = SavedSettings.actualPerCent3;
                else
                    actualSumOfPercentages += SavedSettings.actualPerCent3;
            }

            if (SavedSettings.actualPerCent25 != -1)
            {
                if (actualSumOfPercentages == -1)
                    actualSumOfPercentages = SavedSettings.actualPerCent25;
                else
                    actualSumOfPercentages += SavedSettings.actualPerCent25;
            }

            if (SavedSettings.actualPerCent2 != -1)
            {
                if (actualSumOfPercentages == -1)
                    actualSumOfPercentages = SavedSettings.actualPerCent2;
                else
                    actualSumOfPercentages += SavedSettings.actualPerCent2;
            }

            if (SavedSettings.actualPerCent15 != -1)
            {
                if (actualSumOfPercentages == -1)
                    actualSumOfPercentages = SavedSettings.actualPerCent15;
                else
                    actualSumOfPercentages += SavedSettings.actualPerCent15;
            }

            if (SavedSettings.actualPerCent1 != -1)
            {
                if (actualSumOfPercentages == -1)
                    actualSumOfPercentages = SavedSettings.actualPerCent1;
                else
                    actualSumOfPercentages += SavedSettings.actualPerCent1;
            }

            if (SavedSettings.actualPerCent05 != -1)
            {
                if (actualSumOfPercentages == -1)
                    actualSumOfPercentages = SavedSettings.actualPerCent05;
                else
                    actualSumOfPercentages += SavedSettings.actualPerCent05;
            }


            if (calculateActualSumOfPercentageOnCalculatingThresholds && InvokeRequired)
                MbForm.Invoke(new Action(() => { fillThresholdsPercentagesUi(); }));
            else if (calculateActualSumOfPercentageOnCalculatingThresholds)
                fillThresholdsPercentagesUi();
        }

        private static double RoundPlaysPerDay(double value)
        {
            return Math.Round(value, 12);
        }

        private static double RoundPlaysPerDay(string value)
        {
            double number;

            try
            {
                number = Convert.ToDouble(value);
            }
            catch (FormatException)
            {
                number = 0;
            }

            return RoundPlaysPerDay(number);
        }

        private static string ConvertDoubleToString(double value)
        {
            double ppd = RoundPlaysPerDay(value);
            string ppdString = ppd.ToString();

            //string intString = Math.Floor(ppd).ToString();
            //int max = 5;
            //for (int i = 5; i > intString.Length; i--)
            //    ppdString = " " + ppdString;

            return ppdString;
        }

        internal static void AutoRateLive(string currentFile)
        {
            var autoRateTagId = SavedSettings.autoRateTagId;
            var playsPerDayTagId = SavedSettings.playsPerDayTagId;

            int autoRating;
            var playsPerDay = GetPlaysPerDay(currentFile);

            if (playsPerDay == -1) //-V3024
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

        internal void autoRateOnStartup()
        {
            if (!MbApiInterface.Library_QueryFilesEx("domain=Library", out files))
                files = Array.Empty<string>();

            for (var fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (checkStoppingStatus())
                {
                    SetStatusBarText(null, null, true);
                    backgroundTaskIsStoppedOrCancelled = true;
                    return;
                }


                var currentFile = files[fileCounter];

                SetStatusBarTextForFileOperations(null, AutoRateSbText, false, fileCounter, files.Length, currentFile);

                AutoRateLive(currentFile);
            }

            RefreshPanels(true);
            SetResultingSbText(null);

            if (SavedSettings.notifyWhenAutoRatingCompleted) MessageBox.Show(this, MsgBackgroundTaskIsCompleted, string.Empty,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool applyingChangesStopped()
        {
            backgroundTaskIsScheduled = false;
            backgroundTaskIsStopping = false;
            backgroundTaskIsStoppedOrCancelled = false;
            closeFormOnStopping = false;
            ignoreClosingForm = false;

            SetResultingSbText(this);

            return true;
        }

        private void closeFormOnOperationStoppingCompletionIfRequired()
        {
            backgroundTaskIsScheduled = false;
            SetStatusBarText(this, null, true);


            if (Disposing || IsDisposed || !IsHandleCreated)
                return;

            if (closeFormOnStopping)
            {
                ignoreClosingForm = false;
                Close();
                return;
            }

            enableDisablePreviewOptionControls(true);
            ignoreClosingForm = false;
        }

        private bool prepareBackgroundTask()
        {
            if (backgroundTaskIsWorking())
                return false;

            MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files);
            if (files == null || files.Length == 0)
            {
                MessageBox.Show(this, MsgNoTracksSelected, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            ignoreClosingForm = true;

            return true;
        }

        private void autoRateNow()
        {
            for (var fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (checkStoppingStatus())
                {
                    MbForm.Invoke(new Action(() => { stopButtonClickedMethod(applyingChangesStopped); }));
                    return;
                }


                var currentFile = files[fileCounter];

                SetStatusBarTextForFileOperations(this, AutoRateSbText, false, fileCounter, files.Length, currentFile);

                AutoRateLive(currentFile);
            }

            RefreshPanels(true);
            SetResultingSbText(this);

            MbForm.Invoke(new Action(() => { closeFormOnOperationStoppingCompletionIfRequired(); }));
        }

        private static double GetPlaysPerDay(string currentFile)
        {
            double daysSinceAdded;
            double daysSinceLastPlayed;

            var played = Convert.ToInt32(MbApiInterface.Library_GetFileProperty(currentFile, FilePropertyType.PlayCount));
            var skipped = Convert.ToInt32(MbApiInterface.Library_GetFileProperty(currentFile, FilePropertyType.SkipCount));
            double derivedPlayed = played - skipped;

            if (derivedPlayed < 0)
                derivedPlayed = 0;

            try
            {
                daysSinceAdded =
                    (DateTime.Parse(string.Empty +
                                    MbApiInterface.Library_GetFileProperty(currentFile, FilePropertyType.DateAdded)) -
                     DateTime.Now).TotalDays;
            }
            catch (FormatException)
            {
                daysSinceAdded = 0;
            }

            try
            {
                if (SavedSettings.sinceAdded)
                    daysSinceLastPlayed = 0;
                else
                    daysSinceLastPlayed = (DateTime.Parse(string.Empty + MbApiInterface.Library_GetFileProperty(currentFile, FilePropertyType.LastPlayed)) - DateTime.Now).TotalDays;
            }
            catch (FormatException) { daysSinceLastPlayed = 0; }

            var daysPlayed = daysSinceLastPlayed - daysSinceAdded;

            if (daysPlayed < 0)
                daysPlayed = 0;

            if (daysPlayed != 0 && played != 0) //-V3024
                return RoundPlaysPerDay(derivedPlayed / daysPlayed);
            else
                return -1;
        }

        private void getStatistics(object state)
        {
            statisticsTimer?.Dispose();
            statisticsTimer = null;

            Thread.CurrentThread.Priority = ThreadPriority.Lowest;


            double totalPlaysPerDay = 0;
            NumberOfFiles = 0;

            double maxPlaysPerDay = 0;
            double avgPlaysPerDay = 0;

            if (!MbApiInterface.Library_QueryFilesEx("domain=Library", out files))
                files = Array.Empty<string>();

            for (var fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (checkStoppingCollectingStatisticsStatus())
                {
                    backgroundTaskIsStoppedOrCancelled = true;
                    MbForm.Invoke(new Action(() => { closeFormOnOperationStoppingCompletionIfRequired(); }));
                    return;
                }

                if ((fileCounter & 0x1f) == 0)
                    MbForm.Invoke(new Action(() => { labelTotalTracks.Text = MsgNumberOfPlayedTracks + CtlAutoRateCalculating.ToLower() + " (" + (100 * (fileCounter + 1) / files.Length) + "%)"; }));


                var currentFile = files[fileCounter];

                var playsPerDay = GetPlaysPerDay(currentFile);

                if (playsPerDay != -1) //-V3024
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



            MbForm.Invoke(new Action(() =>
            {
                maxPlaysPerDayBox.Text = ConvertDoubleToString(maxPlaysPerDay);
                avgPlaysPerDayBox.Text = ConvertDoubleToString(avgPlaysPerDay);
                labelTotalTracks.Text = MsgNumberOfPlayedTracks + NumberOfFiles;

                closeFormOnOperationStoppingCompletionIfRequired();
            }));

            if (!SavedSettings.dontPlayCompletedSound)
                System.Media.SystemSounds.Asterisk.Play();
        }

        internal void onStartup()
        {
            if (SavedSettings.calculateThresholdsAtStartUp) calculateThresholds();
            if (SavedSettings.autoRateAtStartUp) autoRateOnStartup();
        }

        internal void calculateThresholds()
        {
            ignoreClosingForm = true;

            decimal fullPercentageSum = sumOfPercentages();

            if (fullPercentageSum != 100)
                SavedSettings.threshold5 += (double)(100 - fullPercentageSum);

            calculateActualSumOfPercentages();

            NumberOfFiles = 0;
            var playsPerDayStatistics = new SortedDictionary<double, int>();

            if (!MbApiInterface.Library_QueryFilesEx("domain=Library", out files))
                files = Array.Empty<string>();

            for (var fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (checkStoppingStatus())
                {
                    backgroundTaskIsStoppedOrCancelled = true;
                    MbForm.Invoke(new Action(() => { closeFormOnOperationStoppingCompletionIfRequired(); }));
                    return;
                }


                var currentFile = files[fileCounter];
                var playsPerDay = GetPlaysPerDay(currentFile);

                SetStatusBarText(this, AutoRateSbText + AutoRateSbTextCalculatingThresholds + (fileCounter + 1) + "/" + files.Length, false);

                if (playsPerDay != -1) //-V3024
                {
                    NumberOfFiles++;

                    if (playsPerDayStatistics.TryGetValue(-playsPerDay, out var statistics))
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

            var statisticsSum = 0;
            var assignedFilesNumber = 0;

            SetStatusBarText(this, AutoRateSbText + AutoRateSbTextCalculatingActualPercentagesCalculatingThresholds, false);

            foreach (var playsPerDay in playsPerDayStatistics.Keys)
            {
                if (checkStoppingStatus())
                {
                    backgroundTaskIsStoppedOrCancelled = true;
                    MbForm.Invoke(new Action(() => { closeFormOnOperationStoppingCompletionIfRequired(); }));
                    return;
                }


                playsPerDayStatistics.TryGetValue(playsPerDay, out var statistics);
                statisticsSum += statistics;

                if (SavedSettings.perCent5 != 0 && SavedSettings.threshold5 == 0) //-V3024
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= SavedSettings.perCent5)
                    {
                        SavedSettings.actualPerCent5 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        SavedSettings.threshold5 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (SavedSettings.perCent45 != 0 && SavedSettings.threshold45 == 0) //-V3024
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= SavedSettings.perCent45)
                    {
                        SavedSettings.actualPerCent45 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        SavedSettings.threshold45 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (SavedSettings.perCent4 != 0 && SavedSettings.threshold4 == 0) //-V3024
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= SavedSettings.perCent4)
                    {
                        SavedSettings.actualPerCent4 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        SavedSettings.threshold4 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (SavedSettings.perCent35 != 0 && SavedSettings.threshold35 == 0) //-V3024
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= SavedSettings.perCent35)
                    {
                        SavedSettings.actualPerCent35 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        SavedSettings.threshold35 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (SavedSettings.perCent3 != 0 && SavedSettings.threshold3 == 0) //-V3024
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= SavedSettings.perCent3)
                    {
                        SavedSettings.actualPerCent3 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        SavedSettings.threshold3 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (SavedSettings.perCent25 != 0 && SavedSettings.threshold25 == 0) //-V3024
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= SavedSettings.perCent25)
                    {
                        SavedSettings.actualPerCent25 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        SavedSettings.threshold25 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (SavedSettings.perCent2 != 0 && SavedSettings.threshold2 == 0) //-V3024
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= SavedSettings.perCent2)
                    {
                        SavedSettings.actualPerCent2 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        SavedSettings.threshold2 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (SavedSettings.perCent15 != 0 && SavedSettings.threshold15 == 0) //-V3024
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= SavedSettings.perCent15)
                    {
                        SavedSettings.actualPerCent15 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        SavedSettings.threshold15 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (SavedSettings.perCent1 != 0 && SavedSettings.threshold1 == 0) //-V3024
                {
                    if (((decimal)statisticsSum) / NumberOfFiles * 100 >= SavedSettings.perCent1)
                    {
                        SavedSettings.actualPerCent1 = Math.Round(((decimal)statisticsSum) / NumberOfFiles * 100, 0);
                        SavedSettings.threshold1 = -playsPerDay;
                        assignedFilesNumber += statisticsSum;
                        statisticsSum = 0;
                    }
                }
                else if (SavedSettings.perCent05 != 0 && SavedSettings.threshold05 == 0) //-V3024
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
                if (SavedSettings.perCent5 != 0 && SavedSettings.threshold5 == 0) //-V3024
                {
                    SavedSettings.actualPerCent5 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //MusicBeePlugin.savedSettings.threshold5 = 0;
                }
                else if (SavedSettings.perCent45 != 0 && SavedSettings.threshold45 == 0) //-V3024
                {
                    SavedSettings.actualPerCent45 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //MusicBeePlugin.savedSettings.threshold45 = 0;
                }
                else if (SavedSettings.perCent4 != 0 && SavedSettings.threshold4 == 0) //-V3024
                {
                    SavedSettings.actualPerCent4 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //MusicBeePlugin.savedSettings.threshold4 = 0;
                }
                else if (SavedSettings.perCent35 != 0 && SavedSettings.threshold35 == 0) //-V3024
                {
                    SavedSettings.actualPerCent35 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //MusicBeePlugin.savedSettings.threshold35 = 0;
                }
                else if (SavedSettings.perCent3 != 0 && SavedSettings.threshold3 == 0) //-V3024
                {
                    SavedSettings.actualPerCent3 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //MusicBeePlugin.savedSettings.threshold3 = 0;
                }
                else if (SavedSettings.perCent25 != 0 && SavedSettings.threshold25 == 0) //-V3024
                {
                    SavedSettings.actualPerCent25 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //MusicBeePlugin.savedSettings.threshold25 = 0;
                }
                else if (SavedSettings.perCent2 != 0 && SavedSettings.threshold2 == 0) //-V3024
                {
                    SavedSettings.actualPerCent2 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //MusicBeePlugin.savedSettings.threshold2 = 0;
                }
                else if (SavedSettings.perCent15 != 0 && SavedSettings.threshold15 == 0) //-V3024
                {
                    SavedSettings.actualPerCent15 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //MusicBeePlugin.savedSettings.threshold15 = 0;
                }
                else if (SavedSettings.perCent1 != 0 && SavedSettings.threshold1 == 0) //-V3024
                {
                    SavedSettings.actualPerCent1 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //MusicBeePlugin.savedSettings.threshold1 = 0;
                }
                else if (SavedSettings.perCent05 != 0 && SavedSettings.threshold05 == 0) //-V3024
                {
                    SavedSettings.actualPerCent05 = Math.Round(((decimal)(NumberOfFiles - assignedFilesNumber)) / NumberOfFiles * 100, 0);
                    //MusicBeePlugin.savedSettings.threshold05 = 0;
                }
            }


            SetStatusBarText(this, null, true);

            if (calculateActualSumOfPercentageOnCalculatingThresholds)
                calculateActualSumOfPercentages();

            MbForm.Invoke(new Action(() => { closeFormOnOperationStoppingCompletionIfRequired(); }));
        }

        private bool checkSettings()
        {
            if (sumOfPercentages() != 100)
            {
                MessageBox.Show(this, MsgIncorrectSumOfWeights, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        private bool saveSettings()
        {
            if (!checkSettings())
                return false;


            SavedSettings.autoRateTagId = GetTagId(autoRatingTagListCustom.Text);
            SavedSettings.storePlaysPerDay = storePlaysPerDayCheckBox.Checked;
            SavedSettings.playsPerDayTagId = GetTagId(playsPerDayTagListCustom.Text);

            SavedSettings.autoRateAtStartUp = autoRateAtStartUpCheckBox.Checked;
            SavedSettings.notifyWhenAutoRatingCompleted = notifyWhenAutoRatingCompletedCheckBox.Checked;
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
                    switchOperation(autoRateNow, buttonOK, buttonOK, EmptyButton, buttonClose, true, null);
            }
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            ignoreClosingForm = true;
            calculateActualSumOfPercentageOnCalculatingThresholds = true;
            switchOperation(calculateThresholds, buttonCalculate, buttonOK, buttonCalculate, buttonClose, false, null);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            saveSettings();
        }

        private void checkBoxN_CheckedChanged(NumericUpDown threshold, CheckBox checkBox, NumericUpDown perCent, bool enable = true)
        {
            threshold.Enable(checkBox.Checked && enable);
            if (checkBox.Checked)
            {
                if (perCent.Value == 0)
                    perCent.Value = 1;
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
        }

        private void checkBoxFourAndHalf_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold45Box, checkBox45, perCent45UpDown);
        }

        private void checkBox45Label_Click(object sender, EventArgs e)
        {
            checkBox45.Checked = !checkBox45.Checked;
        }

        private void checkBoxFour_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold4Box, checkBox4, perCent4UpDown);
        }

        private void checkBox4Label_Click(object sender, EventArgs e)
        {
            checkBox4.Checked = !checkBox4.Checked;
        }

        private void checkBoxThreeAndHalf_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold35Box, checkBox35, perCent35UpDown);
        }

        private void checkBox35Label_Click(object sender, EventArgs e)
        {
            checkBox35.Checked = !checkBox35.Checked;
        }

        private void checkBoxThree_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold3Box, checkBox3, perCent3UpDown);
        }

        private void checkBox3Label_Click(object sender, EventArgs e)
        {
            checkBox3.Checked = !checkBox3.Checked;
        }

        private void checkBoxTwoAndHalf_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold25Box, checkBox25, perCent25UpDown);
        }

        private void checkBox25Label_Click(object sender, EventArgs e)
        {
            checkBox25.Checked = !checkBox25.Checked;
        }

        private void checkBoxTwo_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold2Box, checkBox2, perCent2UpDown);
        }

        private void checkBox2Label_Click(object sender, EventArgs e)
        {
            checkBox2.Checked = !checkBox2.Checked;
        }

        private void checkBoxOneAndHalf_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold15Box, checkBox15, perCent15UpDown);
        }

        private void checkBox15Label_Click(object sender, EventArgs e)
        {
            checkBox15.Checked = !checkBox15.Checked;
        }

        private void checkBoxOne_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold1Box, checkBox1, perCent1UpDown);
        }

        private void checkBox1Label_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = !checkBox1.Checked;
        }

        private void checkBoxHalf_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxN_CheckedChanged(threshold05Box, checkBox05, perCent05UpDown);
        }

        private void checkBox05Label_Click(object sender, EventArgs e)
        {
            checkBox05.Checked = !checkBox05.Checked;
        }

        private void perCentN_ValueChanged(NumericUpDown perCent, CheckBox checkBox, Label perCentLabel, decimal actualPerCent)
        {
            if (!formIsOpening)
            {
                calculateThresholdsAtStartUpCheckBox.Checked = false;
                calculateThresholdsAtStartUpCheckBox.Enable(false);
                buttonCalculate.Enable(false);
                buttonNormalizePercentages.Enable(true);
            }

            if (perCent.Value == 0)
                checkBox.Checked = false;
            else
                checkBox.Checked = true;

            if (actualSumOfPercentages == -1)
                labelSum.Text = MsgSum + sumOfPercentages() + "% (" + (100 - sumOfPercentages()) + MsgNumberOfNotRatedTracks;
            else
                labelSum.Text = MsgSum + sumOfPercentages() + MsgActualPercent + actualSumOfPercentages + "% (" + (100 - actualSumOfPercentages) + MsgNumberOfNotRatedTracks;


            if (actualPerCent == -1)
            {
                perCentLabel.Text = "% (~" + Math.Round(NumberOfFiles * perCent.Value / 100, 0) + MsgTracks;
                var font = new Font(perCentLabel.Font, FontStyle.Regular);
                perCentLabel.Font = font;
            }
            else
            {
                perCentLabel.Text = MsgActualPercent + actualPerCent + "% (~" + Math.Round(NumberOfFiles * actualPerCent / 100, 0) + MsgTracks;
                var font = new Font(perCentLabel.Font, FontStyle.Bold);
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

        private void fillThresholdsPercentagesUi()
        {
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
            (sender as NumericUpDown).Text = ConvertDoubleToString(RoundPlaysPerDay((sender as NumericUpDown).Text));
        }

        private void autoRateAtStartUp_CheckedChanged(object sender, EventArgs e)
        {
            notifyWhenAutoRatingCompletedCheckBox.Enable(autoRateAtStartUpCheckBox.Checked);
        }

        private void autoRateAtStartUpCheckBoxLabel_Click(object sender, EventArgs e)
        {
            autoRateAtStartUpCheckBox.Checked = !autoRateAtStartUpCheckBox.Checked;
        }

        private void storePlaysPerDay_CheckedChanged(object sender, EventArgs e)
        {
            playsPerDayTagListCustom.Enable(storePlaysPerDayCheckBox.Checked);
        }

        private void storePlaysPerDayCheckBoxLabel_Click(object sender, EventArgs e)
        {
            storePlaysPerDayCheckBox.Checked = !storePlaysPerDayCheckBox.Checked;
        }

        internal override void enableDisablePreviewOptionControls(bool enable, bool dontChangeDisabled = false)
        {
            foreach (var control in allControls)
            {
                if (control != buttonSettings && control != buttonClose)
                    control.Enable(enable);
            }

            if (enable)
            {
                decimal fullPercentageSum = sumOfPercentages();

                if (fullPercentageSum == 100)
                {
                    calculateThresholdsAtStartUpCheckBox.Enable(true);
                    buttonNormalizePercentages.Enable(false);
                    buttonCalculate.Enable(true);
                }
                else
                {
                    calculateThresholdsAtStartUpCheckBox.Enable(false);
                    buttonNormalizePercentages.Enable(true);
                    buttonCalculate.Enable(false);
                }
            }

            checkBoxN_CheckedChanged(threshold5Box, checkBox5, perCent5UpDown, enable);
            checkBoxN_CheckedChanged(threshold45Box, checkBox45, perCent45UpDown, enable);
            checkBoxN_CheckedChanged(threshold4Box, checkBox4, perCent4UpDown, enable);
            checkBoxN_CheckedChanged(threshold35Box, checkBox35, perCent35UpDown, enable);
            checkBoxN_CheckedChanged(threshold3Box, checkBox3, perCent3UpDown, enable);
            checkBoxN_CheckedChanged(threshold25Box, checkBox25, perCent25UpDown, enable);
            checkBoxN_CheckedChanged(threshold2Box, checkBox2, perCent2UpDown, enable);
            checkBoxN_CheckedChanged(threshold15Box, checkBox15, perCent15UpDown, enable);
            checkBoxN_CheckedChanged(threshold1Box, checkBox1, perCent1UpDown, enable);
            checkBoxN_CheckedChanged(threshold05Box, checkBox05, perCent05UpDown, enable);
        }

        internal override void enableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonCalculate, " ");
            dirtyErrorProvider.SetError(buttonCalculate, string.Empty);
        }

        internal override void disableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonCalculate, getBackgroundTasksWarning());
        }

        internal override void enableQueryingOrUpdatingButtons()
        {
            buttonOK.Enable(true && (backgroundTaskIsUpdatingTags || !backgroundTaskIsWorking()));
        }

        internal override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enable(false);
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

        private void notifyWhenAutoRatingCompletedCheckBoxLabel_Click(object sender, EventArgs e)
        {
            if (!notifyWhenAutoRatingCompletedCheckBox.IsEnabled())
                return;

            notifyWhenAutoRatingCompletedCheckBox.Checked = !notifyWhenAutoRatingCompletedCheckBox.Checked;
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            var settings = new QuickSettings(TagToolsPlugin);
            Display(settings, this, true);
        }

        private void buttonNormalizePercentages_Click(object sender, EventArgs e)
        {
            decimal fullPercentageSum = sumOfPercentages();

            if (fullPercentageSum == 0)
            {
                perCent05UpDown.Value = 0;
                perCent1UpDown.Value = 0;
                perCent15UpDown.Value = 0;
                perCent2UpDown.Value = 0;
                perCent25UpDown.Value = 0;
                perCent3UpDown.Value = 0;
                perCent35UpDown.Value = 0;
                perCent4UpDown.Value = 0;
                perCent45UpDown.Value = 0;
                perCent5UpDown.Value = 0;
            }
            else
            {
                perCent05UpDown.Value = Math.Round(perCent05UpDown.Value * 100 / fullPercentageSum);
                perCent1UpDown.Value = Math.Round(perCent1UpDown.Value * 100 / fullPercentageSum);
                perCent15UpDown.Value = Math.Round(perCent15UpDown.Value * 100 / fullPercentageSum);
                perCent2UpDown.Value = Math.Round(perCent2UpDown.Value * 100 / fullPercentageSum);
                perCent25UpDown.Value = Math.Round(perCent25UpDown.Value * 100 / fullPercentageSum);
                perCent3UpDown.Value = Math.Round(perCent3UpDown.Value * 100 / fullPercentageSum);
                perCent35UpDown.Value = Math.Round(perCent35UpDown.Value * 100 / fullPercentageSum);
                perCent4UpDown.Value = Math.Round(perCent4UpDown.Value * 100 / fullPercentageSum);
                perCent45UpDown.Value = Math.Round(perCent45UpDown.Value * 100 / fullPercentageSum);
                perCent5UpDown.Value = Math.Round(perCent5UpDown.Value * 100 / fullPercentageSum);
            }

            fullPercentageSum = sumOfPercentages();
            if (fullPercentageSum != 100)
                perCent5UpDown.Value += 100 - fullPercentageSum;

            fullPercentageSum = sumOfPercentages();
            if (fullPercentageSum == 100)
            {
                buttonNormalizePercentages.Enable(false);
                calculateThresholdsAtStartUpCheckBox.Enable(true);
                buttonCalculate.Enable(true);
            }


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

            calculateActualSumOfPercentages();
        }

        internal protected override void childClassFormShown()
        {
            ignoreClosingForm = true;
            statisticsTimer = new System.Threading.Timer(getStatistics, null, 1000, Timeout.Infinite);
        }

        private void AutoRate_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ignoreClosingForm)
            {
                if (!backgroundTaskIsUpdatingTags)
                {
                    backgroundTaskIsStopping = true;
                    closeFormOnStopping = true;
                    buttonClose.Enable(false);

                    backgroundTaskIsStopping = true;
                    SetStatusBarText(this, AutoRateSbText + SbTextStoppingCurrentOperation, false);

                    e.Cancel = true;
                }
            }
        }
    }
}
