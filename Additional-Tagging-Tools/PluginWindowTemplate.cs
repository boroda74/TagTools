using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

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


        private bool modal;
        private FormWindowState windowState;
        private int left;
        private int top;
        private int width;
        private int height;

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
        }

        public PluginWindowTemplate(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;
        }

        public static Control SkinControl(Control control)
        {
            if (!SavedSettings.useSkinColors)
                return control;


            if (control.GetType() == typeof(Button))
            {
                control.BackColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentBackground));
                control.ForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));

                ((Button)control).FlatStyle = FlatStyle.Flat;
                ((Button)control).FlatAppearance.BorderSize = 1;
                ((Button)control).FlatAppearance.BorderColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentBorder));
            }
            else if (control.GetType().IsSubclassOf(typeof(TextBox)) || control.GetType() == typeof(TextBox))
            {
                control.BackColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentBackground));
                control.ForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));

                //((TextBox)control).BorderStyle = BorderStyle.FixedSingle;
            }
            else if (control.GetType().IsSubclassOf(typeof(ComboBox)) || control.GetType() == typeof(ComboBox))
            {
                control.BackColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentBackground));
                control.ForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));

                ((ComboBox)control).FlatStyle = FlatStyle.Flat;
            }
            else if (control.GetType().IsSubclassOf(typeof(ListBox)) || control.GetType() == typeof(ListBox))
            {
                control.BackColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentBackground));
                control.ForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));
            }
            else if (control.GetType() == typeof(DataGridView))
            {
                control.BackColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentBackground));
                control.ForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));

                ((DataGridView)control).BackgroundColor = control.BackColor;
                ((DataGridView)control).DefaultCellStyle.BackColor = control.BackColor;
                //((DataGridView)control).ColumnHeadersDefaultCellStyle.BackColor = control.BackColor;
            }
            else
            {
                control.BackColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanel, ElementState.ElementStateDefault, ElementComponent.ComponentBackground));
                control.ForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanel, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));
            }

            return control;
        }

        public static Control SkinChildrenControls(Control parentControl)
        {
            if (!SavedSettings.useSkinColors)
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
            if (SavedSettings.useSkinColors)
            {
                form.BackColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanel, ElementState.ElementStateDefault, ElementComponent.ComponentBackground));
                form.ForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanel, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));

                SkinChildrenControls(form);
            }

            return form;
        }

        protected void setInitialFormMaximizedBounds()
        {
            if (modal && MaximumSize.Height != 0)
            {
                Height = MinimumSize.Height;
            }

            setFormMaximizedBounds();
        }

        protected void setFormMaximizedBounds()
        {
            int maximizedHeight = MaximumSize.Height;

            if (maximizedHeight != 0)
            {
                if (modal)
                {
                    maximizedHeight = Height;
                }

                if (maximizedHeight > Screen.FromControl(this).WorkingArea.Height)
                    maximizedHeight = Screen.FromControl(this).WorkingArea.Height;

                MaximizedBounds = new Rectangle(Screen.FromControl(this).WorkingArea.Left, Screen.FromControl(this).WorkingArea.Top,
                    Screen.FromControl(this).WorkingArea.Width, maximizedHeight);
            }
        }

        protected void initializeForm()
        {
            MbForm = MbForm.IsDisposed ? (Form)FromHandle(MbApiInterface.MB_GetWindowHandle()) : MbForm;
            MbForm.AddOwnedForm(this);

            clickedButton = EmptyButton;

            lock (OpenedForms)
            {
                if (NumberOfNativeMbBackgroundTasks > 0)
                    disableQueryingButtons();
                else
                    enableQueryingButtons();
            }

            stopButtonClicked = stopButtonClickedMethod;
            taskStarted = taskStartedMethod;

            TagToolsPlugin.fillTagNames();


            //return; //For debugging

            SkinForm(this);
        }

        public static void Display(PluginWindowTemplate newForm, bool modalForm = false)
        {
            lock (OpenedForms)
            {
                foreach (PluginWindowTemplate form in OpenedForms)
                {
                    if (newForm == null || form.GetType() == newForm.GetType())
                    {
                        newForm?.Dispose();

                        if (form.Visible && form.WindowState != FormWindowState.Minimized)
                        {
                            form.Activate();
                        }
                        else
                        {
                            form.Left = form.left;
                            form.Top = form.top;
                            form.Width = form.width;
                            form.Height = form.height;
                            form.WindowState = form.windowState;

                            if (newForm != null && modalForm)
                            {
                                form.modal = true;
                                form.setInitialFormMaximizedBounds();
                                form.ShowDialog();
                            }
                            else if (newForm != null)
                            {
                                form.modal = false;
                                form.setInitialFormMaximizedBounds();
                                form.Show();
                            }
                            else if (form.modal)
                            {
                                form.ShowDialog();
                            }
                            else //if (!form.modal)
                            {
                                form.Show();
                            }
                        }

                        return;
                    }
                }

                OpenedForms.Add(newForm);
            }

            if (modalForm)
            {
                newForm.modal = true;
                newForm.setInitialFormMaximizedBounds();
                newForm.ShowDialog();
                newForm.Dispose();
            }
            else
            {
                newForm.modal = false;
                newForm.setInitialFormMaximizedBounds();
                newForm.Show();
            }
        }

        private void serializedOperation()
        {
            bool taskWasStarted = false;

            InitializeSbText();

            try
            {
                if (backgroundTaskIsCanceled)
                {
                    if (DisablePlaySoundOnce)
                        DisablePlaySoundOnce = false;
                    else if (SavedSettings.playCanceledSound)
                        System.Media.SystemSounds.Hand.Play();
                }
                else
                {
                    if (DisablePlaySoundOnce)
                        DisablePlaySoundOnce = false;
                    else if (SavedSettings.playStartedSound)
                        System.Media.SystemSounds.Exclamation.Play();

                    backgroundThread = Thread.CurrentThread;
                    backgroundThread.Priority = ThreadPriority.BelowNormal;


                    if (clickedButton != EmptyButton)
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
                    if (DisablePlaySoundOnce)
                        DisablePlaySoundOnce = false;
                    else if (!SavedSettings.dontPlayCompletedSound)
                        System.Media.SystemSounds.Asterisk.Play();

                    backgroundTaskIsScheduled = false;
                    backgroundTaskIsCanceled = false;

                    if (backgroundTaskIsNativeMB && taskWasStarted)
                    {
                        NumberOfNativeMbBackgroundTasks--;
                    }
                }

                if (clickedButton != EmptyButton)
                    Invoke(stopButtonClicked, new object[] { this, null });

                RefreshPanels(true);

                SetResultingSbText();

                if (!Visible)
                {
                    if (SavedSettings.closeShowHiddenWindows == 1)
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

                    lock (OpenedForms)
                    {
                        if (backgroundTaskIsNativeMB)
                        {
                            NumberOfNativeMbBackgroundTasks++;
                        }
                    }

                    queryingOrUpdatingButtonClick(this);
                }
                else
                {
                    backgroundTaskIsCanceled = true;

                    lock (OpenedForms)
                    {
                        if (backgroundTaskIsNativeMB)
                        {
                            NumberOfNativeMbBackgroundTasks--;
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
                    lock (OpenedForms)
                    {
                        NumberOfNativeMbBackgroundTasks++;
                    }

                    MbApiInterface.MB_CreateBackgroundTask(serializedOperation, this);
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
            if (NumberOfNativeMbBackgroundTasks == 1)
                return CtlDirtyError1sf + NumberOfNativeMbBackgroundTasks + CtlDirtyError2sf;
            else if (NumberOfNativeMbBackgroundTasks > 1)
                return CtlDirtyError1mf + NumberOfNativeMbBackgroundTasks + CtlDirtyError2mf;
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
            lock (OpenedForms)
            {
                foreach (PluginWindowTemplate form in OpenedForms)
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
                    clickedButton.Text = CancelButtonName;
                    clickedButton.Enabled = true;

                    closeButtonText = closeButton.Text;
                    closeButton.Text = HideButtonName;
                    closeButton.Enabled = true;
                }
                else //Querying operation
                {
                    clickedForm.disableQueryingOrUpdatingButtons();

                    clickedButton.Text = StopButtonName;
                    clickedButton.Enabled = true;

                    closeButton.Enabled = false;
                }

                enableDisablePreviewOptionControls(false);
            }
        }

        public void stopButtonClickedMethod(PluginWindowTemplate clickedForm, PrepareOperation prepareOperation)
        {
            lock (OpenedForms)
            {
                foreach (PluginWindowTemplate form in OpenedForms)
                {
                    if (backgroundTaskIsNativeMB && form != clickedForm) //Updating operation
                    {
                        if (NumberOfNativeMbBackgroundTasks > 0 && !(form.backgroundTaskIsNativeMB && form.backgroundTaskIsScheduled && !form.backgroundTaskIsCanceled))
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
                        previewButton.Text = ClearButtonName;
                    else
                        previewButton.Text = PreviewButtonName;

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
                        clickedButton.Text = PreviewButtonName;
                        previewIsStopped = true;
                    }
                    else
                    {
                        backgroundTaskIsCanceled = true;

                        if (previewIsGenerated && !previewIsStopped)
                        {
                            clickedButton.Text = ClearButtonName;
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
            lock (OpenedForms)
            {
                if (backgroundTaskIsNativeMB) //Updating operation
                {
                    enableQueryingButtons();

                    clickedButton.Text = StopButtonName;
                }
            }
        }

        // clearPreview = 0: 
        protected void clickOnPreviewButton(DataGridView previewTable, PrepareOperation prepareOperation, ThreadStart operation, 
            Button clickedButtonParam, Button okButtonParam, Button closeButtonParam, int clearPreview = 0)
        {
            if ((previewTable.Rows.Count == 0 || backgroundTaskIsWorking() || clearPreview == 2) && clearPreview != 1)
            {
                if (prepareOperation())
                    switchOperation(operation, clickedButtonParam, okButtonParam, clickedButtonParam, closeButtonParam, false, prepareOperation);
            }
            else
            {
                previewIsGenerated = false;
                prepareOperation();
                clickedButtonParam.Text = PreviewButtonName;
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

                lock (OpenedForms)
                {
                    OpenedForms.Remove(this);
                }
            }
        }

        private void ToolsPluginTemplate_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                left = RestoreBounds.Left;
                top = RestoreBounds.Top;
                width = RestoreBounds.Width;
                height = RestoreBounds.Height;

                if (!SavedSettings.minimizePluginWindows)
                    Hide();
            }
            else
            {
                if (WindowState == FormWindowState.Maximized)
                {
                    left = RestoreBounds.Left;
                    top = RestoreBounds.Top;
                    width = RestoreBounds.Width;
                    height = RestoreBounds.Height;
                    windowState = WindowState;
                }
                else
                {
                    setFormMaximizedBounds();

                    left = Left;
                    top = Top;
                    width = Width;
                    height = Height;
                    windowState = WindowState;
                }
            }
        }

        private void ToolsPluginTemplate_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible && width != 0)
            {
                WindowState = windowState;
                Left = left;
                Top = top;
                Width = width;
                Height = height;

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
                Left = windowSettings.x;
                left = windowSettings.x;

                Top = windowSettings.y;
                top = windowSettings.y;

                if (FormBorderStyle != FormBorderStyle.FixedDialog)
                {
                    Width = windowSettings.w;
                    Height = windowSettings.h;

                    if (windowSettings.max)
                    {
                        WindowState = FormWindowState.Maximized;
                        windowState = FormWindowState.Maximized;
                    }
                    else
                    {
                        WindowState = FormWindowState.Normal;
                        windowState = FormWindowState.Normal;
                    }
                }
            }
        }

        protected WindowSettingsType findCreateSavedWindowSettings(bool createIfAbsent)
        {
            string fullName = GetType().FullName;
            WindowSettingsType currentWindowSettings = null;

            foreach (var windowSettings in SavedSettings.windowsSettings)
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

                SavedSettings.windowsSettings.Add(currentWindowSettings);
            }

            return currentWindowSettings;
        }

        protected void saveWindowSizesPositions()
        {
            var currentWindowSettings = findCreateSavedWindowSettings(true);

            if (!Visible || WindowState == FormWindowState.Minimized)
            {
                currentWindowSettings.x = left;
                currentWindowSettings.y = top;
                currentWindowSettings.w = width;
                currentWindowSettings.h = height;
                currentWindowSettings.max = (windowState == FormWindowState.Maximized ? true : false);
            }
            else if (windowState == FormWindowState.Maximized)
            {
                currentWindowSettings.x = left;
                currentWindowSettings.y = top;
                currentWindowSettings.w = width;
                currentWindowSettings.h = height;
                currentWindowSettings.max = true;
            }
            else
            {
                currentWindowSettings.x = Left;
                currentWindowSettings.y = Top;
                currentWindowSettings.w = width;
                currentWindowSettings.h = height;
                currentWindowSettings.max = false;
            }
        }

        protected void saveWindowLayout(int column1Width = 0, int column2Width = 0, int column3Width = 0, int splitterDistance = 0, int table2column1Width = 0, int table2column2Width = 0, int table2column3Width = 0)
        {
            var currentWindowSettings = findCreateSavedWindowSettings(true);

            if (column1Width != 0)
            {
                currentWindowSettings.column1Width = column1Width;
                currentWindowSettings.column2Width = column2Width;
                currentWindowSettings.column3Width = column3Width;
            }

            if (table2column1Width != 0)
            {
                currentWindowSettings.table2column1Width = table2column1Width;
                currentWindowSettings.table2column2Width = table2column2Width;
                currentWindowSettings.table2column3Width = table2column3Width;
            }

            if (splitterDistance != 0)
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
