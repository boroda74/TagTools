namespace MusicBeePlugin
{
    partial class AutoBackupSettings
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoBackupSettings));
            this.autobackupFolderTextBox = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.autobackupFolderLabel = new System.Windows.Forms.Label();
            this.autobackupPrefixLabel = new System.Windows.Forms.Label();
            this.autobackupPrefixTextBox = new System.Windows.Forms.TextBox();
            this.autobackupCheckBox = new System.Windows.Forms.CheckBox();
            this.autobackupNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.autobackupPeriodLabel = new System.Windows.Forms.Label();
            this.autodeleteOldCheckBox = new System.Windows.Forms.CheckBox();
            this.numberOfDaysNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.autodeleteManyCheckBox = new System.Windows.Forms.CheckBox();
            this.numberOfFilesNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.autodeleteOldLabel = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.backupArtworksCheckBox = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dontTryToGuessLibraryNameCheckBox = new System.Windows.Forms.CheckBox();
            this.autobackupCheckBoxLabel = new System.Windows.Forms.Label();
            this.autodeleteOldCheckBoxLabel = new System.Windows.Forms.Label();
            this.autodeleteManyCheckBoxLabel = new System.Windows.Forms.Label();
            this.dontSkipAutobackupsIfPlayCountsChangedCheckBoxLabel = new System.Windows.Forms.Label();
            this.dontTryToGuessLibraryNameCheckBoxLabel = new System.Windows.Forms.Label();
            this.backupArtworksCheckBoxLabel = new System.Windows.Forms.Label();
            this.dontSkipAutobackupsIfPlayCountsChangedCheckBox = new System.Windows.Forms.CheckBox();
            this.dontSkipAutobackupsIfPlayCountsChangedLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.autobackupNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfDaysNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfFilesNumericUpDown)).BeginInit();
            this.SuspendLayout();

            //MusicBee
            this.autobackupFolderTextBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.autobackupPrefixTextBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            //~MusicBee

            // 
            // autobackupFolderTextBox
            // 
            resources.ApplyResources(this.autobackupFolderTextBox, "autobackupFolderTextBox");
            this.autobackupFolderTextBox.Name = "autobackupFolderTextBox";
            this.autobackupFolderTextBox.ReadOnly = true;
            this.autobackupFolderTextBox.Tag = "#browseButton";
            // 
            // browseButton
            // 
            resources.ApplyResources(this.browseButton, "browseButton");
            this.browseButton.Name = "browseButton";
            this.browseButton.Tag = "#AutoBackupSettings@non-defaultable";
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // autobackupFolderLabel
            // 
            resources.ApplyResources(this.autobackupFolderLabel, "autobackupFolderLabel");
            this.autobackupFolderLabel.Name = "autobackupFolderLabel";
            this.autobackupFolderLabel.Tag = "";
            // 
            // autobackupPrefixLabel
            // 
            resources.ApplyResources(this.autobackupPrefixLabel, "autobackupPrefixLabel");
            this.autobackupPrefixLabel.Name = "autobackupPrefixLabel";
            this.autobackupPrefixLabel.Tag = "#autobackupPrefixTextBox";
            // 
            // autobackupPrefixTextBox
            // 
            resources.ApplyResources(this.autobackupPrefixTextBox, "autobackupPrefixTextBox");
            this.autobackupPrefixTextBox.Name = "autobackupPrefixTextBox";
            this.autobackupPrefixTextBox.Tag = "#AutoBackupSettings";
            // 
            // autobackupCheckBox
            // 
            resources.ApplyResources(this.autobackupCheckBox, "autobackupCheckBox");
            this.autobackupCheckBox.Name = "autobackupCheckBox";
            this.autobackupCheckBox.Tag = "#autobackupCheckBoxLabel";
            this.toolTip1.SetToolTip(this.autobackupCheckBox, resources.GetString("autobackupCheckBox.ToolTip"));
            this.autobackupCheckBox.CheckedChanged += new System.EventHandler(this.autobackupCheckBox_CheckedChanged);
            // 
            // autobackupNumericUpDown
            // 
            resources.ApplyResources(this.autobackupNumericUpDown, "autobackupNumericUpDown");
            this.autobackupNumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.autobackupNumericUpDown.Name = "autobackupNumericUpDown";
            this.autobackupNumericUpDown.Tag = "#autobackupPeriodLabel";
            // 
            // autobackupPeriodLabel
            // 
            resources.ApplyResources(this.autobackupPeriodLabel, "autobackupPeriodLabel");
            this.autobackupPeriodLabel.Name = "autobackupPeriodLabel";
            this.autobackupPeriodLabel.Tag = "";
            // 
            // autodeleteOldCheckBox
            // 
            resources.ApplyResources(this.autodeleteOldCheckBox, "autodeleteOldCheckBox");
            this.autodeleteOldCheckBox.Name = "autodeleteOldCheckBox";
            this.autodeleteOldCheckBox.Tag = "#autodeleteOldCheckBoxLabel";
            this.toolTip1.SetToolTip(this.autodeleteOldCheckBox, resources.GetString("autodeleteOldCheckBox.ToolTip"));
            this.autodeleteOldCheckBox.CheckedChanged += new System.EventHandler(this.autodeleteOldCheckBox_CheckedChanged);
            // 
            // numberOfDaysNumericUpDown
            // 
            resources.ApplyResources(this.numberOfDaysNumericUpDown, "numberOfDaysNumericUpDown");
            this.numberOfDaysNumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numberOfDaysNumericUpDown.Name = "numberOfDaysNumericUpDown";
            this.numberOfDaysNumericUpDown.Tag = "#autodeleteOldLabel";
            // 
            // autodeleteManyCheckBox
            // 
            resources.ApplyResources(this.autodeleteManyCheckBox, "autodeleteManyCheckBox");
            this.autodeleteManyCheckBox.Name = "autodeleteManyCheckBox";
            this.autodeleteManyCheckBox.Tag = "#autodeleteManyCheckBoxLabel";
            this.toolTip1.SetToolTip(this.autodeleteManyCheckBox, resources.GetString("autodeleteManyCheckBox.ToolTip"));
            this.autodeleteManyCheckBox.CheckedChanged += new System.EventHandler(this.autodeleteManyCheckBox_CheckedChanged);
            // 
            // numberOfFilesNumericUpDown
            // 
            resources.ApplyResources(this.numberOfFilesNumericUpDown, "numberOfFilesNumericUpDown");
            this.numberOfFilesNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numberOfFilesNumericUpDown.Name = "numberOfFilesNumericUpDown";
            // 
            // autodeleteOldLabel
            // 
            resources.ApplyResources(this.autodeleteOldLabel, "autodeleteOldLabel");
            this.autodeleteOldLabel.Name = "autodeleteOldLabel";
            // 
            // buttonClose
            // 
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Tag = "#AutoBackupSettings@non-defaultable@pinned-to-parent-y";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Tag = "#buttonClose@pinned-to-parent-y";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // backupArtworksCheckBox
            // 
            resources.ApplyResources(this.backupArtworksCheckBox, "backupArtworksCheckBox");
            this.backupArtworksCheckBox.Name = "backupArtworksCheckBox";
            this.backupArtworksCheckBox.Tag = "#backupArtworksCheckBoxLabel";
            this.toolTip1.SetToolTip(this.backupArtworksCheckBox, resources.GetString("backupArtworksCheckBox.ToolTip"));
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 15000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // dontTryToGuessLibraryNameCheckBox
            // 
            resources.ApplyResources(this.dontTryToGuessLibraryNameCheckBox, "dontTryToGuessLibraryNameCheckBox");
            this.dontTryToGuessLibraryNameCheckBox.Name = "dontTryToGuessLibraryNameCheckBox";
            this.dontTryToGuessLibraryNameCheckBox.Tag = "#dontTryToGuessLibraryNameCheckBoxLabel";
            this.toolTip1.SetToolTip(this.dontTryToGuessLibraryNameCheckBox, resources.GetString("dontTryToGuessLibraryNameCheckBox.ToolTip"));
            // 
            // autobackupCheckBoxLabel
            // 
            resources.ApplyResources(this.autobackupCheckBoxLabel, "autobackupCheckBoxLabel");
            this.autobackupCheckBoxLabel.Name = "autobackupCheckBoxLabel";
            this.autobackupCheckBoxLabel.Tag = "#autobackupNumericUpDown";
            this.toolTip1.SetToolTip(this.autobackupCheckBoxLabel, resources.GetString("autobackupCheckBoxLabel.ToolTip"));
            this.autobackupCheckBoxLabel.Click += new System.EventHandler(this.autobackupCheckBoxLabel_Click);
            // 
            // autodeleteOldCheckBoxLabel
            // 
            resources.ApplyResources(this.autodeleteOldCheckBoxLabel, "autodeleteOldCheckBoxLabel");
            this.autodeleteOldCheckBoxLabel.Name = "autodeleteOldCheckBoxLabel";
            this.autodeleteOldCheckBoxLabel.Tag = "#numberOfDaysNumericUpDown";
            this.toolTip1.SetToolTip(this.autodeleteOldCheckBoxLabel, resources.GetString("autodeleteOldCheckBoxLabel.ToolTip"));
            this.autodeleteOldCheckBoxLabel.Click += new System.EventHandler(this.autodeleteOldCheckBoxLabel_Click);
            // 
            // autodeleteManyCheckBoxLabel
            // 
            resources.ApplyResources(this.autodeleteManyCheckBoxLabel, "autodeleteManyCheckBoxLabel");
            this.autodeleteManyCheckBoxLabel.Name = "autodeleteManyCheckBoxLabel";
            this.autodeleteManyCheckBoxLabel.Tag = "#numberOfFilesNumericUpDown";
            this.toolTip1.SetToolTip(this.autodeleteManyCheckBoxLabel, resources.GetString("autodeleteManyCheckBoxLabel.ToolTip"));
            this.autodeleteManyCheckBoxLabel.Click += new System.EventHandler(this.autodeleteManyCheckBoxLabel_Click);
            // 
            // dontSkipAutobackupsIfPlayCountsChangedCheckBoxLabel
            // 
            resources.ApplyResources(this.dontSkipAutobackupsIfPlayCountsChangedCheckBoxLabel, "dontSkipAutobackupsIfPlayCountsChangedCheckBoxLabel");
            this.dontSkipAutobackupsIfPlayCountsChangedCheckBoxLabel.Name = "dontSkipAutobackupsIfPlayCountsChangedCheckBoxLabel";
            this.dontSkipAutobackupsIfPlayCountsChangedCheckBoxLabel.Tag = "dontSkipAutobackupsIfPlayCountsChangedCheckBoxLabelLabel";
            this.toolTip1.SetToolTip(this.dontSkipAutobackupsIfPlayCountsChangedCheckBoxLabel, resources.GetString("dontSkipAutobackupsIfPlayCountsChangedCheckBoxLabel.ToolTip"));
            // 
            // dontTryToGuessLibraryNameCheckBoxLabel
            // 
            resources.ApplyResources(this.dontTryToGuessLibraryNameCheckBoxLabel, "dontTryToGuessLibraryNameCheckBoxLabel");
            this.dontTryToGuessLibraryNameCheckBoxLabel.Name = "dontTryToGuessLibraryNameCheckBoxLabel";
            this.toolTip1.SetToolTip(this.dontTryToGuessLibraryNameCheckBoxLabel, resources.GetString("dontTryToGuessLibraryNameCheckBoxLabel.ToolTip"));
            this.dontTryToGuessLibraryNameCheckBoxLabel.Click += new System.EventHandler(this.dontTryToGuessLibraryNameCheckBoxLabel_Click);
            // 
            // backupArtworksCheckBoxLabel
            // 
            resources.ApplyResources(this.backupArtworksCheckBoxLabel, "backupArtworksCheckBoxLabel");
            this.backupArtworksCheckBoxLabel.Name = "backupArtworksCheckBoxLabel";
            this.toolTip1.SetToolTip(this.backupArtworksCheckBoxLabel, resources.GetString("backupArtworksCheckBoxLabel.ToolTip"));
            this.backupArtworksCheckBoxLabel.Click += new System.EventHandler(this.backupArtworksCheckBoxLabel_Click);
            // 
            // dontSkipAutobackupsIfPlayCountsChangedCheckBox
            // 
            resources.ApplyResources(this.dontSkipAutobackupsIfPlayCountsChangedCheckBox, "dontSkipAutobackupsIfPlayCountsChangedCheckBox");
            this.dontSkipAutobackupsIfPlayCountsChangedCheckBox.Name = "dontSkipAutobackupsIfPlayCountsChangedCheckBox";
            this.dontSkipAutobackupsIfPlayCountsChangedCheckBox.Tag = "#dontSkipAutobackupsIfPlayCountsChangedLabel";
            // 
            // dontSkipAutobackupsIfPlayCountsChangedLabel
            // 
            resources.ApplyResources(this.dontSkipAutobackupsIfPlayCountsChangedLabel, "dontSkipAutobackupsIfPlayCountsChangedLabel");
            this.dontSkipAutobackupsIfPlayCountsChangedLabel.Name = "dontSkipAutobackupsIfPlayCountsChangedLabel";
            this.dontSkipAutobackupsIfPlayCountsChangedLabel.Tag = "";
            this.dontSkipAutobackupsIfPlayCountsChangedLabel.Click += new System.EventHandler(this.dontSkipAutobackupsIfPlayCountsChangedLabel_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // AutoBackupSettings
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.backupArtworksCheckBoxLabel);
            this.Controls.Add(this.backupArtworksCheckBox);
            this.Controls.Add(this.dontTryToGuessLibraryNameCheckBoxLabel);
            this.Controls.Add(this.dontTryToGuessLibraryNameCheckBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dontSkipAutobackupsIfPlayCountsChangedLabel);
            this.Controls.Add(this.dontSkipAutobackupsIfPlayCountsChangedCheckBox);
            this.Controls.Add(this.numberOfFilesNumericUpDown);
            this.Controls.Add(this.autodeleteManyCheckBoxLabel);
            this.Controls.Add(this.autodeleteManyCheckBox);
            this.Controls.Add(this.autodeleteOldLabel);
            this.Controls.Add(this.numberOfDaysNumericUpDown);
            this.Controls.Add(this.autodeleteOldCheckBoxLabel);
            this.Controls.Add(this.autodeleteOldCheckBox);
            this.Controls.Add(this.autobackupPrefixTextBox);
            this.Controls.Add(this.autobackupPrefixLabel);
            this.Controls.Add(this.autobackupPeriodLabel);
            this.Controls.Add(this.autobackupNumericUpDown);
            this.Controls.Add(this.autobackupCheckBoxLabel);
            this.Controls.Add(this.autobackupCheckBox);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.autobackupFolderTextBox);
            this.Controls.Add(this.autobackupFolderLabel);
            this.Controls.Add(this.dontSkipAutobackupsIfPlayCountsChangedCheckBoxLabel);
            this.MaximizeBox = false;
            this.Name = "AutoBackupSettings";
            this.Tag = "@min-max-height-same";
            ((System.ComponentModel.ISupportInitialize)(this.autobackupNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfDaysNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfFilesNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label autobackupFolderLabel;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.TextBox autobackupFolderTextBox;
        private System.Windows.Forms.Label autobackupPrefixLabel;
        private System.Windows.Forms.TextBox autobackupPrefixTextBox;
        private System.Windows.Forms.CheckBox autobackupCheckBox;
        private System.Windows.Forms.NumericUpDown autobackupNumericUpDown;
        private System.Windows.Forms.Label autobackupPeriodLabel;
        private System.Windows.Forms.CheckBox autodeleteOldCheckBox;
        private System.Windows.Forms.NumericUpDown numberOfDaysNumericUpDown;
        private System.Windows.Forms.CheckBox autodeleteManyCheckBox;
        private System.Windows.Forms.NumericUpDown numberOfFilesNumericUpDown;
        private System.Windows.Forms.Label autodeleteOldLabel;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.CheckBox backupArtworksCheckBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox dontTryToGuessLibraryNameCheckBox;
        private System.Windows.Forms.CheckBox dontSkipAutobackupsIfPlayCountsChangedCheckBox;
        private System.Windows.Forms.Label autobackupCheckBoxLabel;
        private System.Windows.Forms.Label autodeleteOldCheckBoxLabel;
        private System.Windows.Forms.Label autodeleteManyCheckBoxLabel;
        private System.Windows.Forms.Label dontSkipAutobackupsIfPlayCountsChangedCheckBoxLabel;
        private System.Windows.Forms.Label dontSkipAutobackupsIfPlayCountsChangedLabel;
        private System.Windows.Forms.Label dontTryToGuessLibraryNameCheckBoxLabel;
        private System.Windows.Forms.Label backupArtworksCheckBoxLabel;
    }
}