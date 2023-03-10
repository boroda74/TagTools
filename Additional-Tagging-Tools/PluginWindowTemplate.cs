using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class PluginWindowTemplate : Form
    {
        private delegate void StopButtonClicked(PluginWindowTemplate clickedForm, PrepareOperation prepareOperation);
        private delegate void TaskStarted();
        public delegate bool PrepareOperation();

        private StopButtonClicked stopButtonClicked;
        private TaskStarted taskStarted;

        protected static Plugin TagToolsPlugin;

        private Thread backgroundThread = null;
        protected ThreadStart job;

        public bool backgroundTaskIsScheduled = false;
        public bool backgroundTaskIsNativeMB = false;
        public bool backgroundTaskIsCanceled = false;
        public bool previewIsStopped = false;

        public bool previewIsGenerated = false;

        protected Button clickedButton;
        protected Button previewButton;
        protected Button closeButton;

        protected string okButtonText;
        protected string previewButtonText;
        protected string closeButtonText;


        private FormWindowState windowState;
        private int windowWidth;
        private int windowHeight;

        public bool backgroundTaskIsWorking()
        {
            lock (taskStarted)
            {
                if (backgroundTaskIsScheduled && !backgroundTaskIsCanceled)
                {
                    return true;
                }
            }

            return false;
        }

        public PluginWindowTemplate()
        {
            InitializeComponent();
        }

        public PluginWindowTemplate(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;

            initializeForm();
        }

        public static Control SkinControl(Control control)
        {
            if (!Plugin.SavedSettings.useSkinColors)
                return control;


            if (control.GetType() == typeof(Button))
            {
                control.BackColor = Color.FromArgb(Plugin.MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputControl, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentBackground));
                control.ForeColor = Color.FromArgb(Plugin.MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputControl, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentForeground));

                ((Button)control).FlatStyle = FlatStyle.Flat;
                ((Button)control).FlatAppearance.BorderSize = 1;
                ((Button)control).FlatAppearance.BorderColor = Color.FromArgb(Plugin.MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputControl, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentBorder));
            }
            else if (control.GetType().IsSubclassOf(typeof(TextBox)) || control.GetType() == typeof(TextBox))
            {
                control.BackColor = Color.FromArgb(Plugin.MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputControl, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentBackground));
                control.ForeColor = Color.FromArgb(Plugin.MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputControl, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentForeground));

                //((TextBox)control).BorderStyle = BorderStyle.FixedSingle;
            }
            else if (control.GetType().IsSubclassOf(typeof(ComboBox)) || control.GetType() == typeof(ComboBox))
            {
                control.BackColor = Color.FromArgb(Plugin.MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputControl, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentBackground));
                control.ForeColor = Color.FromArgb(Plugin.MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputControl, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentForeground));

                ((ComboBox)control).FlatStyle = FlatStyle.Flat;
            }
            else if (control.GetType().IsSubclassOf(typeof(ListBox)) || control.GetType() == typeof(ListBox))
            {
                control.BackColor = Color.FromArgb(Plugin.MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputControl, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentBackground));
                control.ForeColor = Color.FromArgb(Plugin.MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputControl, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentForeground));
            }
            else if (control.GetType() == typeof(DataGridView))
            {
                control.BackColor = Color.FromArgb(Plugin.MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputControl, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentBackground));
                control.ForeColor = Color.FromArgb(Plugin.MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputControl, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentForeground));

                ((DataGridView)control).BackgroundColor = control.BackColor;
                ((DataGridView)control).DefaultCellStyle.BackColor = control.BackColor;
                //((DataGridView)control).ColumnHeadersDefaultCellStyle.BackColor = control.BackColor;
            }
            else
            {
                control.BackColor = Color.FromArgb(Plugin.MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputPanel, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentBackground));
                control.ForeColor = Color.FromArgb(Plugin.MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputPanel, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentForeground));
            }

            return control;
        }

        public static Control SkinChildrenControls(Control parentControl)
        {
            if (!Plugin.SavedSettings.useSkinColors)
                return parentControl;


            foreach (Control control in parentControl.Controls)
            {

                SkinControl(control);
                SkinChildrenControls(control);
            }

            return parentControl;
        }

        public static Form SkinForm(Form form)
        {
            if (Plugin.SavedSettings.useSkinColors)
            {
                form.BackColor = Color.FromArgb(Plugin.MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputPanel, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentBackground));
                form.ForeColor = Color.FromArgb(Plugin.MbApiInterface.Setting_GetSkinElementColour(Plugin.SkinElement.SkinInputPanel, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentForeground));

                SkinChildrenControls(form);
            }

            return form;
        }

        protected void initializeForm()
        {
            Plugin.MbForm = Plugin.MbForm.IsDisposed ? (Form)FromHandle(Plugin.MbApiInterface.MB_GetWindowHandle()) : Plugin.MbForm;
            Plugin.MbForm.AddOwnedForm(this);

            clickedButton = Plugin.EmptyButton;

            lock (Plugin.OpenedForms)
            {
                if (Plugin.NumberOfNativeMbBackgroundTasks > 0)
                    disableQueryingButtons();
                else
                    enableQueryingButtons();
            }

            stopButtonClicked = stopButtonClickedMethod;
            taskStarted = taskStartedMethod;

            TagToolsPlugin.fillTagNames();


            //return; //For debbuging

            SkinForm(this);
        }

        public static void Display(PluginWindowTemplate newForm, bool modalForm = false)
        {
            lock (Plugin.OpenedForms)
            {
                foreach (PluginWindowTemplate form in Plugin.OpenedForms)
                {
                    if (form.GetType() == newForm.GetType())
                    {
                        newForm.Dispose();

                        if (form.Visible)
                        {
                            form.Activate();
                        }
                        else
                        {
                            if (modalForm)
                                form.ShowDialog();
                            else
                                form.Show();
                        }

                        return;
                    }
                }

                Plugin.OpenedForms.Add(newForm);
            }

            if (modalForm)
                newForm.ShowDialog();
            else
                newForm.Show();
        }

        private void serializedOperation()
        {
            bool taskWasStarted = false;

            Plugin.InitializeSbText();

            try
            {
                if (backgroundTaskIsCanceled)
                {
                    if (Plugin.SavedSettings.playCanceledSound)
                        System.Media.SystemSounds.Hand.Play();
                }
                else
                {
                    if (Plugin.SavedSettings.playStartedSound)
                        System.Media.SystemSounds.Exclamation.Play();

                    backgroundThread = Thread.CurrentThread;
                    backgroundThread.Priority = ThreadPriority.BelowNormal;


                    if (clickedButton != Plugin.EmptyButton)
                        Invoke(taskStarted);

                    taskWasStarted = true;
                    job();
                }
            }
            catch (ThreadAbortException)
            {
                backgroundTaskIsCanceled = true;
            }
            finally
            {
                lock (taskStarted)
                {
                    if (!Plugin.SavedSettings.dontPlayCompletedSound)
                        System.Media.SystemSounds.Asterisk.Play();

                    backgroundTaskIsScheduled = false;
                    backgroundTaskIsCanceled = false;

                    if (backgroundTaskIsNativeMB && taskWasStarted)
                    {
                        Plugin.NumberOfNativeMbBackgroundTasks--;
                    }
                }

                if (clickedButton != Plugin.EmptyButton)
                    Invoke(stopButtonClicked, new object[] { this, null });

                Plugin.RefreshPanels(true);

                Plugin.SetResultingSbText();

                if (!Visible)
                {
                    if (Plugin.SavedSettings.closeShowHiddenWindows == 1)
                        Close();
                    else
                        Visible = true;
                }
            }
        }

        public void switchOperation(ThreadStart operation, Button clickedButtonParam, Button okButtonParam, Button previewButtonParam, Button closeButtonParam, bool backgroundTaskIsNativeMbParam, PrepareOperation prepareOperation)
        {
            if (backgroundTaskIsScheduled && !backgroundTaskIsWorking())
            {
                if (backgroundTaskIsCanceled)
                {
                    backgroundTaskIsCanceled = false;

                    lock (Plugin.OpenedForms)
                    {
                        if (backgroundTaskIsNativeMB)
                        {
                            Plugin.NumberOfNativeMbBackgroundTasks++;
                        }
                    }

                    queryingOrUpdatingButtonClick(this);
                }
                else
                {
                    backgroundTaskIsCanceled = true;

                    lock (Plugin.OpenedForms)
                    {
                        if (backgroundTaskIsNativeMB)
                        {
                            Plugin.NumberOfNativeMbBackgroundTasks--;
                        }
                    }

                    stopButtonClicked(this, prepareOperation);
                }
            }
            else if (backgroundTaskIsWorking())
            {
                stopButtonClicked(this, prepareOperation);
            }
            else
            {
                backgroundTaskIsNativeMB = backgroundTaskIsNativeMbParam;

                backgroundTaskIsCanceled = false;
                backgroundTaskIsScheduled = true;
                backgroundThread = null;

                clickedButton = clickedButtonParam;
                closeButton = closeButtonParam;
                previewButton = previewButtonParam;

                job = operation;

                if (backgroundTaskIsNativeMB)
                {
                    lock (Plugin.OpenedForms)
                    {
                        Plugin.NumberOfNativeMbBackgroundTasks++;
                    }

                    Plugin.MbApiInterface.MB_CreateBackgroundTask(serializedOperation, this);
                }
                else
                {
                    Thread tempThread = new Thread(serializedOperation);
                    tempThread.Start();
                }

                okButtonText = okButtonParam.Text;
                previewButtonText = previewButtonParam.Text;
                closeButtonText = closeButtonParam.Text;

                queryingOrUpdatingButtonClick(this);
            }
        }

        protected string getBackgroundTasksWarning()
        {
            if (Plugin.NumberOfNativeMbBackgroundTasks == 1)
                return Plugin.CtlDirtyError1sf + Plugin.NumberOfNativeMbBackgroundTasks + Plugin.CtlDirtyError2sf;
            else if (Plugin.NumberOfNativeMbBackgroundTasks > 1)
                return Plugin.CtlDirtyError1mf + Plugin.NumberOfNativeMbBackgroundTasks + Plugin.CtlDirtyError2mf;
            else
                return string.Empty;
        }

        public virtual void enableDisablePreviewOptionControls(bool enable)
        {
        }

        public virtual void enableQueryingButtons()
        {
        }

        public virtual void disableQueryingButtons()
        {
        }

        public virtual void enableQueryingOrUpdatingButtons()
        {
        }

        public virtual void disableQueryingOrUpdatingButtons()
        {
        }

        public void queryingOrUpdatingButtonClick(PluginWindowTemplate clickedForm)
        {
            lock (Plugin.OpenedForms)
            {
                foreach (PluginWindowTemplate form in Plugin.OpenedForms)
                {
                    if (backgroundTaskIsNativeMB && form != clickedForm && !(form.backgroundTaskIsNativeMB && form.backgroundTaskIsScheduled && !form.backgroundTaskIsCanceled))
                    {
                        form.disableQueryingButtons();
                    }
                }

                if (backgroundTaskIsNativeMB) //Updating operation
                {
                    clickedForm.enableQueryingButtons();
                    clickedForm.disableQueryingOrUpdatingButtons();

                    okButtonText = clickedButton.Text;
                    clickedButton.Text = Plugin.CancelButtonName;
                    clickedButton.Enabled = true;

                    closeButtonText = closeButton.Text;
                    closeButton.Text = Plugin.HideButtonName;
                    closeButton.Enabled = true;
                }
                else //Querying operation
                {
                    clickedForm.disableQueryingOrUpdatingButtons();

                    clickedButton.Text = Plugin.StopButtonName;
                    clickedButton.Enabled = true;

                    closeButton.Enabled = false;
                }

                enableDisablePreviewOptionControls(false);
            }
        }

        public void stopButtonClickedMethod(PluginWindowTemplate clickedForm, PrepareOperation prepareOperation)
        {
            lock (Plugin.OpenedForms)
            {
                foreach (PluginWindowTemplate form in Plugin.OpenedForms)
                {
                    if (backgroundTaskIsNativeMB && form != clickedForm) //Updating operation
                    {
                        if (Plugin.NumberOfNativeMbBackgroundTasks > 0 && !(form.backgroundTaskIsNativeMB && form.backgroundTaskIsScheduled && !form.backgroundTaskIsCanceled))
                        {
                            form.disableQueryingButtons();
                        }
                        else
                        {
                            form.enableQueryingButtons();
                        }
                    }
                }

                clickedForm.disableQueryingButtons();
                clickedForm.enableQueryingOrUpdatingButtons();

                if (backgroundTaskIsNativeMB) //Updating operation
                {
                    clickedButton.Text = okButtonText;

                    if (previewIsGenerated)
                        previewButton.Text = Plugin.ClearButtonName;
                    else
                        previewButton.Text = Plugin.PreviewButtonName;

                    if (backgroundTaskIsScheduled)
                        previewButton.Enabled = false;
                    else
                        closeButton.Text = closeButtonText;
                }
                else //Querying operation
                {
                    if (prepareOperation != null)
                    {
                        previewIsGenerated = false;
                        prepareOperation();
                        backgroundTaskIsCanceled = true;
                        clickedButton.Text = Plugin.PreviewButtonName;
                        previewIsStopped = true;
                    }
                    else
                    {
                        backgroundTaskIsCanceled = true;

                        if (previewIsGenerated && !previewIsStopped)
                        {
                            clickedButton.Text = Plugin.ClearButtonName;
                        }
                        else
                        {
                            previewIsStopped = false;
                            clickedButton.Text = previewButtonText;
                        }

                        closeButton.Enabled = true;
                    }
                }

                enableDisablePreviewOptionControls(true);
            }
        }

        private void taskStartedMethod()
        {
            lock (Plugin.OpenedForms)
            {
                if (backgroundTaskIsNativeMB) //Updating operation
                {
                    enableQueryingButtons();

                    clickedButton.Text = Plugin.StopButtonName;
                }
            }
        }

        // clearPreview = 0: 
        protected void clickOnPreviewButton(DataGridView previewList, PrepareOperation prepareOperation, ThreadStart operation, Button clickedButtonParam, Button okButtonParam, Button closeButtonParam, int clearPreview = 0)
        {
            if ((previewList.Rows.Count == 0 || backgroundTaskIsWorking() || clearPreview == 2) && clearPreview != 1)
            {
                if (prepareOperation())
                    switchOperation(operation, clickedButtonParam, okButtonParam, clickedButtonParam, closeButtonParam, false, prepareOperation);
            }
            else
            {
                previewIsGenerated = false;
                prepareOperation();
                clickedButtonParam.Text = Plugin.PreviewButtonName;
                enableDisablePreviewOptionControls(true);
            }
        }

        private void ToolsPluginTemplate_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundTaskIsScheduled)
            {
                e.Cancel = true;
                Hide();
            }
            else
            {
                saveWindowSizesPositions();
            }
        }

        private void ToolsPluginTemplate_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                windowWidth = RestoreBounds.Width;
                windowHeight = RestoreBounds.Height;

                Hide();
            }
            else
            {
                windowState = WindowState;
            }
        }

        private void ToolsPluginTemplate_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible && windowWidth != 0)
            {
                WindowState = windowState;
                Width = windowWidth;
                Height = windowHeight;

                //windowWidth = 0;
            }
        }

        protected (int, int, int, int, int, int, int) loadWindowLayout()
        {
            int column1Width = 0;
            int column2Width = 0;
            int column3Width = 0;
            int splitterDistance = 0;
            int table2column1Width = 0;
            int table2column2Width = 0; 
            int table2column3Width = 0;

            var windowSettings = findCreateSavedWindowSettings(false);

            if (windowSettings != null)
            { 
                column1Width = windowSettings.column1Width;
                column2Width = windowSettings.column2Width;
                column3Width = windowSettings.column3Width;

                splitterDistance = windowSettings.splitterDistance;

                table2column1Width = windowSettings.table2column1Width;
                table2column2Width = windowSettings.table2column2Width;
                table2column3Width = windowSettings.table2column3Width;
            }

            return (column1Width, column2Width, column3Width, 
                splitterDistance, 
                table2column1Width, table2column2Width, table2column3Width);
        }

        protected void loadWindowSizesPositions()
        {
            var windowSettings = findCreateSavedWindowSettings(false);

            if (windowSettings != null)
            {
                DesktopLocation = new Point(windowSettings.x, windowSettings.y);

                if (FormBorderStyle != FormBorderStyle.FixedDialog)
                {
                    Size = new Size(windowSettings.w, windowSettings.h);

                    if (windowSettings.max)
                        WindowState = FormWindowState.Maximized;
                }
            }
        }

        protected WindowSettingsType findCreateSavedWindowSettings(bool createIfAbsent)
        {
            lock (Plugin.OpenedForms)
            {
                Plugin.OpenedForms.Remove(this);
            }

            string fullName = GetType().FullName;
            WindowSettingsType currentWindowSettings = null;

            foreach (var windowSettings in Plugin.SavedSettings.windowsSettings)
            {
                if (windowSettings.className == fullName)
                {
                    currentWindowSettings = windowSettings;
                    break;
                }
            }

            if (currentWindowSettings == null && createIfAbsent)
            {
                currentWindowSettings = new WindowSettingsType
                {
                    className = fullName
                };

                Plugin.SavedSettings.windowsSettings.Add(currentWindowSettings);
            }

            return currentWindowSettings;
        }

        protected void saveWindowSizesPositions()
        {
            var currentWindowSettings = findCreateSavedWindowSettings(true);

            if (windowState == FormWindowState.Maximized)
            {
                currentWindowSettings.max = true;
            }
            else if (windowState == FormWindowState.Minimized)
            {
                //Nothing changing in saved settings...
            }
            else
            {
                currentWindowSettings.x = DesktopLocation.X;
                currentWindowSettings.y = DesktopLocation.Y;
                currentWindowSettings.w = Size.Width;
                currentWindowSettings.h = Size.Height;
                currentWindowSettings.max = false;
            }
        }

        protected void saveWindowLayout(int column1Width = 0, int column2Width = 0, int column3Width = 0, int splitterDistance = 0, int table2column1Width = 0, int table2column2Width = 0, int table2column3Width = 0)
        {
            var currentWindowSettings = findCreateSavedWindowSettings(true);

            if (column1Width > 0)
            {
                currentWindowSettings.column1Width = column1Width;
                currentWindowSettings.column2Width = column2Width;
                currentWindowSettings.column3Width = column3Width;
            }

            if (table2column1Width > 0)
            {
                currentWindowSettings.table2column1Width = table2column1Width;
                currentWindowSettings.table2column2Width = table2column2Width;
                currentWindowSettings.table2column3Width = table2column3Width;
            }

            if (splitterDistance > 0)
            {
                currentWindowSettings.splitterDistance = splitterDistance;
            }
        }

        private void PluginWindowTemplate_Load(object sender, EventArgs e)
        {
            //return; //For debbuging

            loadWindowSizesPositions();
        }
    }
}
