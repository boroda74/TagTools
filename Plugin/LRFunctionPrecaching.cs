using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using ExtensionMethods;

using static MusicBeePlugin.LibraryReports;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class LrFunctionPrecaching : PluginWindowTemplate
    {
        internal protected Dictionary<ReportPreset, int> allCurrentPresets;
        internal protected ReportPreset[] newOrChangedCachedPresets;

        internal protected string exceptionText;
        internal protected string progressInfo;

        public LrFunctionPrecaching(Plugin plugin) : base(plugin)
        {
            TagToolsPlugin = plugin;

            InitializeComponent();

            WindowMenuIcon = LrMenuIcon;
            WindowIcon = LrIcon;
            WindowIconInactive = LrIconInactive;
            TitleBarText = this.Text;
        }

        internal protected override void initializeForm()
        {
            base.initializeForm();

            progressInfo = progressInfoLabel.Text;
            button_GotFocus(AcceptButton, null); //Let's mark active button
        }

        private void anotherLrPresetCompleteTimer_Tick(object sender, EventArgs e)
        {
            if (BackgroundTaskIsInProgress)
                return;

            BackgroundTaskIsInProgress = true;

            if (anotherLrPresetCompleteTimer.Interval > 1 || !LibraryReportsCommandForFunctionIds.backgroundTaskIsScheduled)
            {
                anotherLrPresetCompleteTimer.Stop();
                anotherLrPresetCompleteTimer.Dispose();
            }

            var workingThread = new Thread(calculatePresets);
            workingThread.Start();
        }

        internal bool precachePresets(ReportPreset[] currentPresets, List<ReportPreset> newOrChangedPresets)
        {
            lock (currentPresets)
            {
                allCurrentPresets = new Dictionary<ReportPreset, int>();
                for (var i = 0; i < currentPresets.Length; i++)
                    allCurrentPresets.Add(new ReportPreset(currentPresets[i], true), i);
            }

            newOrChangedCachedPresets = new ReportPreset[newOrChangedPresets.Count];
            newOrChangedPresets.CopyTo(newOrChangedCachedPresets, 0);

            Display(this, null, true);

            LibraryReportsCommandForFunctionIds.backgroundTaskIsScheduled = false;

            if (exceptionText != null)
            {
                MessageBox.Show(this, exceptionText, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (LibraryReportsCommandForFunctionIds.backgroundTaskIsStopping)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void calculatePresets()
        {
            LibraryReportsCommandForFunctionIds.backgroundTaskIsScheduled = true;
            LibraryReportsCommandForFunctionIds.backgroundTaskIsStopping = false;
            //progressInfoLabel.Text = progressInfo;

            var processedPresets = new List<ReportPreset>();

            var startTime = DateTime.UtcNow;
            Thread.CurrentThread.Priority = ThreadPriority.Normal;

            var reportPresetsBackup = LibraryReportsCommandForFunctionIds.reportPresets;
            var unitedReportPresets = LibraryReportsCommandForFunctionIds.reportPresets.Union(newOrChangedCachedPresets);

            LibraryReportsCommandForFunctionIds.reportPresets = unitedReportPresets.ToArray();


            for (var i = 0; i < newOrChangedCachedPresets.Length; i++)
            {
                if (LibraryReportsCommandForFunctionIds.backgroundTaskIsStopping)
                {
                    if (DisablePlaySoundOnce)
                        DisablePlaySoundOnce = false;
                    else if (SavedSettings.playCanceledSound)
                        System.Media.SystemSounds.Hand.Play();

                    break;
                }

                try
                {
                    var now = DateTime.UtcNow;
                    var elapsed = now - startTime;

                    var status = progressInfo + "\n\n" + newOrChangedCachedPresets[i].getName() + "\n\n" + (i + 1) + "/" + newOrChangedCachedPresets.Length
                                 + CtlLrElapsed + elapsed.Hours + ":" + elapsed.Minutes + ":" + elapsed.Seconds;

                    Invoke(new Action(() => { progressInfoLabel.Text = status; }));

                    lock (LrPresetExecutionLocker)
                    {
                        LibraryReportsCommandForFunctionIds.appliedPreset = newOrChangedCachedPresets[i];
                        LibraryReportsCommandForFunctionIds.backgroundTaskIsUpdatingTags = true;

                        LibraryReportsCommandForFunctionIds.executePreset(this, null, false, true, null, false, true);
                    }
                }
                catch (ThreadAbortException)
                {
                    LibraryReportsCommandForFunctionIds.backgroundTaskIsStopping = true;

                    //Let's just stop the thread...
                    break;
                }
                catch (Exception ex)
                {
                    LibraryReportsCommandForFunctionIds.backgroundTaskIsStopping = true;

                    exceptionText = ex.Message;
                    break;
                }

                processedPresets.Add(newOrChangedCachedPresets[i]);
            }


            LibraryReportsCommandForFunctionIds.reportPresets = reportPresetsBackup;

            BackgroundTaskIsInProgress = false;

            var doNotChangeSavedPresets = new List<ReportPreset>();
            var savePresets = new List<ReportPreset>();

            foreach (var preset in allCurrentPresets.Keys)
            {
                bool savePreset = !(!newOrChangedCachedPresets.Contains(preset) && !processedPresets.Contains(preset));

                if (savePreset)
                {
                    savePresets.Add(preset);
                }
                else
                {
                    lock (SavedSettings.reportPresets)
                    {
                        foreach (var savedPreset in SavedSettings.reportPresets)
                        {
                            if (savedPreset.permanentGuid == preset.permanentGuid) //Can't be saved, but there is saved copy (it's not new preset). Let's resave this copy.
                                doNotChangeSavedPresets.Add(savedPreset);
                        }
                    }
                }
            }


            lock (SavedSettings.reportPresets)
            {
                SavedSettings.reportPresets = new ReportPreset[savePresets.Count + doNotChangeSavedPresets.Count];

                for (var i = 0; i < savePresets.Count; i++)
                    SavedSettings.reportPresets[i] = new ReportPreset(savePresets[i], true);

                for (var i = 0; i < doNotChangeSavedPresets.Count; i++)
                    SavedSettings.reportPresets[savePresets.Count + i] = doNotChangeSavedPresets[i];
            }

            LibraryReportsCommandForFunctionIds.backgroundTaskIsScheduled = false;

            RefreshPanels(true);
            SetResultingSbText(this);
            Invoke(new Action(Close));
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            LibraryReportsCommandForFunctionIds.backgroundTaskIsStopping = true;
            buttonClose.Enable(false);

            progressInfo += CtlWaitUntilPresetIsCompleted;
            progressInfoLabel.Text += progressInfo;

            anotherLrPresetCompleteTimer.Stop();
            anotherLrPresetCompleteTimer.Dispose();

            while (LibraryReportsCommandForFunctionIds.backgroundTaskIsScheduled)
                Thread.Sleep(ActionRetryDelay);

            Close();
        }

        private void LrFunctionPrecaching_Shown(object sender, EventArgs e)
        {
            if (BackgroundTaskIsInProgress)
            {
                progressInfoLabel.Text = CtlAnotherLrPresetIsRunning;

                anotherLrPresetCompleteTimer.Tick += anotherLrPresetCompleteTimer_Tick;
                anotherLrPresetCompleteTimer.Interval = 500;
                anotherLrPresetCompleteTimer.Start();
            }
            else
            {
                anotherLrPresetCompleteTimer.Interval = 1;
                anotherLrPresetCompleteTimer_Tick(null, null);
            }
        }
    }
}

