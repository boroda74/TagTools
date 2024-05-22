using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static MusicBeePlugin.LibraryReports;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class LrFunctionPrecaching : PluginWindowTemplate
    {
        protected Dictionary<ReportPreset, int> allCurrentPresets;
        protected ReportPreset[] newOrChangedCachedPresets;

        protected string exceptionText = null;
        protected string progressInfo;

        internal LrFunctionPrecaching(Plugin plugin) : base(plugin)
        {
            TagToolsPlugin = plugin;

            InitializeComponent();
        }

        protected override void initializeForm()
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

            Thread workingThread = new Thread(calculatePresets);
            workingThread.Start();
        }

        internal bool precachePresets(ReportPreset[] allCurrentPresets, List<ReportPreset> newOrChangedCachedPresets)
        {
            lock (allCurrentPresets)
            {
                this.allCurrentPresets = new Dictionary<ReportPreset, int>();
                for (int i = 0; i < allCurrentPresets.Length; i++)
                    this.allCurrentPresets.Add(new ReportPreset(allCurrentPresets[i], true), i);
            }

            this.newOrChangedCachedPresets = new ReportPreset[newOrChangedCachedPresets.Count];
            newOrChangedCachedPresets.CopyTo(this.newOrChangedCachedPresets, 0);

            Display(this, true);

            LibraryReportsCommandForFunctionIds.backgroundTaskIsScheduled = false;

            if (exceptionText != null)
            {
                LibraryReportsCommandForFunctionIds.backgroundTaskIsCanceled = false;
                MessageBox.Show(this, exceptionText, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (LibraryReportsCommandForFunctionIds.backgroundTaskIsCanceled)
            {
                LibraryReportsCommandForFunctionIds.backgroundTaskIsCanceled = false;
                return false;
            }
            else
            {
                LibraryReportsCommandForFunctionIds.backgroundTaskIsCanceled = false;
                return true;
            }
        }

        private void calculatePresets()
        {
            LibraryReportsCommandForFunctionIds.backgroundTaskIsScheduled = true;
            LibraryReportsCommandForFunctionIds.backgroundTaskIsCanceled = false;
            //progressInfoLabel.Text = progressInfo;

            List<ReportPreset> processedPresets = new List<ReportPreset>();

            DateTime startTime = DateTime.UtcNow;
            Thread.CurrentThread.Priority = ThreadPriority.Normal;

            ReportPreset[] reportPresetsBackup = LibraryReportsCommandForFunctionIds.reportPresets;
            var unitedReportPresets = LibraryReportsCommandForFunctionIds.reportPresets.Union(newOrChangedCachedPresets);

            LibraryReportsCommandForFunctionIds.reportPresets = unitedReportPresets.ToArray();


            for (int i = 0; i < newOrChangedCachedPresets.Length; i++)
            {
                if (LibraryReportsCommandForFunctionIds.backgroundTaskIsCanceled)
                {
                    if (DisablePlaySoundOnce)
                        DisablePlaySoundOnce = false;
                    else if (SavedSettings.playCanceledSound)
                        System.Media.SystemSounds.Hand.Play();

                    break;
                }

                try
                {
                    DateTime now = DateTime.UtcNow;
                    TimeSpan elapsed = now - startTime;

                    string status = progressInfo + "\n\n" + newOrChangedCachedPresets[i].getName() + "\n\n" + (i + 1) + "/" + newOrChangedCachedPresets.Length
                        + CtlLrElapsed + elapsed.Hours + ":" + elapsed.Minutes + ":" + elapsed.Seconds;

                    Invoke(new Action(() => { progressInfoLabel.Text = status; }));

                    lock (LrPresetExecutionLocker)
                    {
                        LibraryReportsCommandForFunctionIds.appliedPreset = newOrChangedCachedPresets[i];
                        LibraryReportsCommandForFunctionIds.executePreset(null, false, true, null, false, true);
                    }
                }
                catch (ThreadAbortException)
                {
                    LibraryReportsCommandForFunctionIds.backgroundTaskIsCanceled = true;

                    //Let's just stop the thread...
                    break;
                }
                catch (Exception ex)
                {
                    LibraryReportsCommandForFunctionIds.backgroundTaskIsCanceled = true;

                    exceptionText = ex.Message;
                    break;
                }

                processedPresets.Add(newOrChangedCachedPresets[i]);
            }


            LibraryReportsCommandForFunctionIds.reportPresets = reportPresetsBackup;

            BackgroundTaskIsInProgress = false;

            List<ReportPreset> doNotChangeSavedPresets = new List<ReportPreset>();
            List<ReportPreset> savePresets = new List<ReportPreset>();

            int j = -1;
            foreach (var preset in this.allCurrentPresets.Keys)
            {
                j++;
                bool savePreset = true;

                if (!newOrChangedCachedPresets.Contains(preset) && !processedPresets.Contains(preset))
                    savePreset = false;

                if (savePreset)
                {
                    savePresets.Add(preset);
                }
                else
                {
                    foreach (var savedPreset in SavedSettings.reportPresets)
                    {
                        if (savedPreset.permanentGuid == preset.permanentGuid) //Can't be saved, but there is saved copy (it's not new preset). Let's resave this copy.
                            doNotChangeSavedPresets.Add(savedPreset);
                    }
                }
            }


            lock (SavedSettings.reportPresets)
            {
                SavedSettings.reportPresets = new ReportPreset[savePresets.Count + doNotChangeSavedPresets.Count];

                for (int i = 0; i < savePresets.Count; i++)
                    SavedSettings.reportPresets[i] = new ReportPreset(savePresets[i], true);

                for (int i = 0; i < doNotChangeSavedPresets.Count; i++)
                    SavedSettings.reportPresets[savePresets.Count + i] = doNotChangeSavedPresets[i];
            }

            LibraryReportsCommandForFunctionIds.backgroundTaskIsScheduled = false;

            RefreshPanels(true);
            SetResultingSbText();
            Invoke(new Action(() => { Close(); }));
        }

        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            LibraryReportsCommandForFunctionIds.backgroundTaskIsCanceled = true;
            buttonCancel.Enable(false);

            progressInfo += CtlWaitUntilPresetIsCompleted;
            progressInfoLabel.Text += progressInfo;

            anotherLrPresetCompleteTimer.Stop();
            anotherLrPresetCompleteTimer.Dispose();

            while (LibraryReportsCommandForFunctionIds.backgroundTaskIsScheduled)
                Thread.Sleep(500);

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

