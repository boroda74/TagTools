namespace MusicBeePlugin
{
    partial class AutoBackupSettings
    {
        ///<summary>
        ///Требуется переменная конструктора.
        ///</summary>
        private System.ComponentModel.IContainer components = null;

        ///<summary>
        ///Освободить все используемые ресурсы.
        ///</summary>
        ///<param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        ///<summary>
        ///Обязательный метод для поддержки конструктора - не изменяйте
        ///содержимое данного метода при помощи редактора кода.
        ///</summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoBackupSettings));
            this.autoBackupFolderTextBox = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.autoBackupFolderLabel = new System.Windows.Forms.Label();
            this.autoBackupPrefixLabel = new System.Windows.Forms.Label();
            this.autoBackupPrefixTextBox = new System.Windows.Forms.TextBox();
            this.autoBackupCheckBox = new System.Windows.Forms.CheckBox();
            this.autoBackupNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.autoBackupPeriodLabel = new System.Windows.Forms.Label();
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
            this.autoBackupCheckBoxLabel = new System.Windows.Forms.Label();
            this.autodeleteOldCheckBoxLabel = new System.Windows.Forms.Label();
            this.autodeleteManyCheckBoxLabel = new System.Windows.Forms.Label();
            this.dontSkipAutoBackupsIfPlayCountsChangedCheckBoxLabel = new System.Windows.Forms.Label();
            this.dontTryToGuessLibraryNameCheckBoxLabel = new System.Windows.Forms.Label();
            this.backupArtworksCheckBoxLabel = new System.Windows.Forms.Label();
            this.storeTrackIdsInCustomTagCheckBoxLabel = new System.Windows.Forms.Label();
            this.storeTrackIdsInCustomTagCheckBox = new System.Windows.Forms.CheckBox();
            this.trackIdTagList = new System.Windows.Forms.ComboBox();
            this.dontSkipAutoBackupsIfPlayCountsChangedCheckBox = new System.Windows.Forms.CheckBox();
            this.dontSkipAutoBackupsIfPlayCountsChangedLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.autoBackupNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfDaysNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfFilesNumericUpDown)).BeginInit();
            this.SuspendLayout();

            //MusicBee
            this.autoBackupFolderTextBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.autoBackupPrefixTextBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            //~MusicBee

            // 
            // autoBackupFolderTextBox
            // 
            resources.ApplyResources(this.autoBackupFolderTextBox, "autoBackupFolderTextBox");
            this.autoBackupFolderTextBox.Name = "autoBackupFolderTextBox";
            this.autoBackupFolderTextBox.ReadOnly = true;
            this.autoBackupFolderTextBox.Tag = "#browseButton";
            this.toolTip1.SetToolTip(this.autoBackupFolderTextBox, resources.GetString("autoBackupFolderTextBox.ToolTip"));
            // 
            // browseButton
            // 
            resources.ApplyResources(this.browseButton, "browseButton");
            this.browseButton.Name = "browseButton";
            this.browseButton.Tag = "#AutoBackupSettings@non-defaultable";
            this.toolTip1.SetToolTip(this.browseButton, resources.GetString("browseButton.ToolTip"));
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // autoBackupFolderLabel
            // 
            resources.ApplyResources(this.autoBackupFolderLabel, "autoBackupFolderLabel");
            this.autoBackupFolderLabel.Name = "autoBackupFolderLabel";
            this.autoBackupFolderLabel.Tag = "";
            this.toolTip1.SetToolTip(this.autoBackupFolderLabel, resources.GetString("autoBackupFolderLabel.ToolTip"));
            // 
            // autoBackupPrefixLabel
            // 
            resources.ApplyResources(this.autoBackupPrefixLabel, "autoBackupPrefixLabel");
            this.autoBackupPrefixLabel.Name = "autoBackupPrefixLabel";
            this.autoBackupPrefixLabel.Tag = "#autoBackupPrefixTextBox";
            this.toolTip1.SetToolTip(this.autoBackupPrefixLabel, resources.GetString("autoBackupPrefixLabel.ToolTip"));
            // 
            // autoBackupPrefixTextBox
            // 
            resources.ApplyResources(this.autoBackupPrefixTextBox, "autoBackupPrefixTextBox");
            this.autoBackupPrefixTextBox.Name = "autoBackupPrefixTextBox";
            this.autoBackupPrefixTextBox.Tag = "#AutoBackupSettings";
            this.toolTip1.SetToolTip(this.autoBackupPrefixTextBox, resources.GetString("autoBackupPrefixTextBox.ToolTip"));
            // 
            // autoBackupCheckBox
            // 
            resources.ApplyResources(this.autoBackupCheckBox, "autoBackupCheckBox");
            this.autoBackupCheckBox.Name = "autoBackupCheckBox";
            this.autoBackupCheckBox.Tag = "#autoBackupCheckBoxLabel";
            this.toolTip1.SetToolTip(this.autoBackupCheckBox, resources.GetString("autoBackupCheckBox.ToolTip"));
            this.autoBackupCheckBox.CheckedChanged += new System.EventHandler(this.autoBackupCheckBox_CheckedChanged);
            // 
            // autoBackupNumericUpDown
            // 
            resources.ApplyResources(this.autoBackupNumericUpDown, "autoBackupNumericUpDown");
            this.autoBackupNumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.autoBackupNumericUpDown.Name = "autoBackupNumericUpDown";
            this.autoBackupNumericUpDown.Tag = "#autoBackupPeriodLabel";
            this.toolTip1.SetToolTip(this.autoBackupNumericUpDown, resources.GetString("autoBackupNumericUpDown.ToolTip"));
            // 
            // autoBackupPeriodLabel
            // 
            resources.ApplyResources(this.autoBackupPeriodLabel, "autoBackupPeriodLabel");
            this.autoBackupPeriodLabel.Name = "autoBackupPeriodLabel";
            this.autoBackupPeriodLabel.Tag = "";
            this.toolTip1.SetToolTip(this.autoBackupPeriodLabel, resources.GetString("autoBackupPeriodLabel.ToolTip"));
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
            this.toolTip1.SetToolTip(this.numberOfDaysNumericUpDown, resources.GetString("numberOfDaysNumericUpDown.ToolTip"));
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
            this.toolTip1.SetToolTip(this.numberOfFilesNumericUpDown, resources.GetString("numberOfFilesNumericUpDown.ToolTip"));
            // 
            // autodeleteOldLabel
            // 
            resources.ApplyResources(this.autodeleteOldLabel, "autodeleteOldLabel");
            this.autodeleteOldLabel.Name = "autodeleteOldLabel";
            this.toolTip1.SetToolTip(this.autodeleteOldLabel, resources.GetString("autodeleteOldLabel.ToolTip"));
            // 
            // buttonClose
            // 
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Tag = "#AutoBackupSettings@non-defaultable@pinned-to-parent-y";
            this.toolTip1.SetToolTip(this.buttonClose, resources.GetString("buttonClose.ToolTip"));
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Tag = "#buttonClose@pinned-to-parent-y";
            this.toolTip1.SetToolTip(this.buttonOK, resources.GetString("buttonOK.ToolTip"));
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
            // autoBackupCheckBoxLabel
            // 
            resources.ApplyResources(this.autoBackupCheckBoxLabel, "autoBackupCheckBoxLabel");
            this.autoBackupCheckBoxLabel.Name = "autoBackupCheckBoxLabel";
            this.autoBackupCheckBoxLabel.Tag = "#autoBackupNumericUpDown";
            this.toolTip1.SetToolTip(this.autoBackupCheckBoxLabel, resources.GetString("autoBackupCheckBoxLabel.ToolTip"));
            this.autoBackupCheckBoxLabel.Click += new System.EventHandler(this.autoBackupCheckBoxLabel_Click);
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
            // dontSkipAutoBackupsIfPlayCountsChangedCheckBoxLabel
            // 
            resources.ApplyResources(this.dontSkipAutoBackupsIfPlayCountsChangedCheckBoxLabel, "dontSkipAutoBackupsIfPlayCountsChangedCheckBoxLabel");
            this.dontSkipAutoBackupsIfPlayCountsChangedCheckBoxLabel.Name = "dontSkipAutoBackupsIfPlayCountsChangedCheckBoxLabel";
            this.toolTip1.SetToolTip(this.dontSkipAutoBackupsIfPlayCountsChangedCheckBoxLabel, resources.GetString("dontSkipAutoBackupsIfPlayCountsChangedCheckBoxLabel.ToolTip"));
            // 
            // dontTryToGuessLibraryNameCheckBoxLabel
            // 
            resources.ApplyResources(this.dontTryToGuessLibraryNameCheckBoxLabel, "dontTryToGuessLibraryNameCheckBoxLabel");
            this.dontTryToGuessLibraryNameCheckBoxLabel.Name = "dontTryToGuessLibraryNameCheckBoxLabel";
            this.dontTryToGuessLibraryNameCheckBoxLabel.Tag = "#storeTrackIdsInCustomTagCheckBox";
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
            // storeTrackIdsInCustomTagCheckBoxLabel
            // 
            resources.ApplyResources(this.storeTrackIdsInCustomTagCheckBoxLabel, "storeTrackIdsInCustomTagCheckBoxLabel");
            this.storeTrackIdsInCustomTagCheckBoxLabel.Name = "storeTrackIdsInCustomTagCheckBoxLabel";
            this.storeTrackIdsInCustomTagCheckBoxLabel.Tag = "#trackIdTagList";
            this.toolTip1.SetToolTip(this.storeTrackIdsInCustomTagCheckBoxLabel, resources.GetString("storeTrackIdsInCustomTagCheckBoxLabel.ToolTip"));
            this.storeTrackIdsInCustomTagCheckBoxLabel.Click += new System.EventHandler(this.storeTrackIdsInCustomTagCheckBoxLabel_Click);
            // 
            // storeTrackIdsInCustomTagCheckBox
            // 
            resources.ApplyResources(this.storeTrackIdsInCustomTagCheckBox, "storeTrackIdsInCustomTagCheckBox");
            this.storeTrackIdsInCustomTagCheckBox.Name = "storeTrackIdsInCustomTagCheckBox";
            this.storeTrackIdsInCustomTagCheckBox.Tag = "#storeTrackIdsInCustomTagCheckBoxLabel";
            this.toolTip1.SetToolTip(this.storeTrackIdsInCustomTagCheckBox, resources.GetString("storeTrackIdsInCustomTagCheckBox.ToolTip"));
            this.storeTrackIdsInCustomTagCheckBox.CheckedChanged += new System.EventHandler(this.storeTrackIdsInCustomTagCheckBox_CheckedChanged);
            // 
            // trackIdTagList
            // 
            resources.ApplyResources(this.trackIdTagList, "trackIdTagList");
            this.trackIdTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.trackIdTagList.DropDownWidth = 250;
            this.trackIdTagList.FormattingEnabled = true;
            this.trackIdTagList.Name = "trackIdTagList";
            this.trackIdTagList.Tag = "";
            this.toolTip1.SetToolTip(this.trackIdTagList, resources.GetString("trackIdTagList.ToolTip"));
            this.trackIdTagList.SelectedIndexChanged += new System.EventHandler(this.trackIdTagList_SelectedIndexChanged);
            // 
            // dontSkipAutoBackupsIfPlayCountsChangedCheckBox
            // 
            resources.ApplyResources(this.dontSkipAutoBackupsIfPlayCountsChangedCheckBox, "dontSkipAutoBackupsIfPlayCountsChangedCheckBox");
            this.dontSkipAutoBackupsIfPlayCountsChangedCheckBox.Name = "dontSkipAutoBackupsIfPlayCountsChangedCheckBox";
            this.dontSkipAutoBackupsIfPlayCountsChangedCheckBox.Tag = "#dontSkipAutoBackupsIfPlayCountsChangedLabel";
            this.toolTip1.SetToolTip(this.dontSkipAutoBackupsIfPlayCountsChangedCheckBox, resources.GetString("dontSkipAutoBackupsIfPlayCountsChangedCheckBox.ToolTip"));
            // 
            // dontSkipAutoBackupsIfPlayCountsChangedLabel
            // 
            resources.ApplyResources(this.dontSkipAutoBackupsIfPlayCountsChangedLabel, "dontSkipAutoBackupsIfPlayCountsChangedLabel");
            this.dontSkipAutoBackupsIfPlayCountsChangedLabel.Name = "dontSkipAutoBackupsIfPlayCountsChangedLabel";
            this.dontSkipAutoBackupsIfPlayCountsChangedLabel.Tag = "";
            this.toolTip1.SetToolTip(this.dontSkipAutoBackupsIfPlayCountsChangedLabel, resources.GetString("dontSkipAutoBackupsIfPlayCountsChangedLabel.ToolTip"));
            this.dontSkipAutoBackupsIfPlayCountsChangedLabel.Click += new System.EventHandler(this.dontSkipAutoBackupsIfPlayCountsChangedLabel_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox1, resources.GetString("groupBox1.ToolTip"));
            // 
            // AutoBackupSettings
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.Controls.Add(this.trackIdTagList);
            this.Controls.Add(this.storeTrackIdsInCustomTagCheckBoxLabel);
            this.Controls.Add(this.storeTrackIdsInCustomTagCheckBox);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.backupArtworksCheckBoxLabel);
            this.Controls.Add(this.backupArtworksCheckBox);
            this.Controls.Add(this.dontTryToGuessLibraryNameCheckBoxLabel);
            this.Controls.Add(this.dontTryToGuessLibraryNameCheckBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dontSkipAutoBackupsIfPlayCountsChangedLabel);
            this.Controls.Add(this.dontSkipAutoBackupsIfPlayCountsChangedCheckBox);
            this.Controls.Add(this.numberOfFilesNumericUpDown);
            this.Controls.Add(this.autodeleteManyCheckBoxLabel);
            this.Controls.Add(this.autodeleteManyCheckBox);
            this.Controls.Add(this.autodeleteOldLabel);
            this.Controls.Add(this.numberOfDaysNumericUpDown);
            this.Controls.Add(this.autodeleteOldCheckBoxLabel);
            this.Controls.Add(this.autodeleteOldCheckBox);
            this.Controls.Add(this.autoBackupPrefixTextBox);
            this.Controls.Add(this.autoBackupPrefixLabel);
            this.Controls.Add(this.autoBackupPeriodLabel);
            this.Controls.Add(this.autoBackupNumericUpDown);
            this.Controls.Add(this.autoBackupCheckBoxLabel);
            this.Controls.Add(this.autoBackupCheckBox);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.autoBackupFolderTextBox);
            this.Controls.Add(this.autoBackupFolderLabel);
            this.Controls.Add(this.dontSkipAutoBackupsIfPlayCountsChangedCheckBoxLabel);
            this.Name = "AutoBackupSettings";
            this.Tag = "@min-max-height-same";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            ((System.ComponentModel.ISupportInitialize)(this.autoBackupNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfDaysNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfFilesNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label autoBackupFolderLabel;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.TextBox autoBackupFolderTextBox;
        private System.Windows.Forms.Label autoBackupPrefixLabel;
        private System.Windows.Forms.TextBox autoBackupPrefixTextBox;
        private System.Windows.Forms.CheckBox autoBackupCheckBox;
        private System.Windows.Forms.NumericUpDown autoBackupNumericUpDown;
        private System.Windows.Forms.Label autoBackupPeriodLabel;
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
        private System.Windows.Forms.CheckBox dontSkipAutoBackupsIfPlayCountsChangedCheckBox;
        private System.Windows.Forms.Label autoBackupCheckBoxLabel;
        private System.Windows.Forms.Label autodeleteOldCheckBoxLabel;
        private System.Windows.Forms.Label autodeleteManyCheckBoxLabel;
        private System.Windows.Forms.Label dontSkipAutoBackupsIfPlayCountsChangedCheckBoxLabel;
        private System.Windows.Forms.Label dontSkipAutoBackupsIfPlayCountsChangedLabel;
        private System.Windows.Forms.Label dontTryToGuessLibraryNameCheckBoxLabel;
        private System.Windows.Forms.Label backupArtworksCheckBoxLabel;
        private System.Windows.Forms.Label storeTrackIdsInCustomTagCheckBoxLabel;
        private System.Windows.Forms.CheckBox storeTrackIdsInCustomTagCheckBox;
        private System.Windows.Forms.ComboBox trackIdTagList;
    }
}