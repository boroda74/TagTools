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
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.autoBackupFolderLabel = new System.Windows.Forms.Label();
            this.autoBackupPrefixLabel = new System.Windows.Forms.Label();
            this.autoBackupPrefixTextBox = new System.Windows.Forms.TextBox();
            this.autoBackupCheckBox = new System.Windows.Forms.CheckBox();
            this.autoBackupNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.autoBackupPeriodLabel = new System.Windows.Forms.Label();
            this.autoDeleteOldCheckBox = new System.Windows.Forms.CheckBox();
            this.numberOfDaysNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.autoDeleteManyCheckBox = new System.Windows.Forms.CheckBox();
            this.numberOfFilesNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.autoDeleteOldLabel = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.backupArtworksCheckBox = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dontTryToGuessLibraryNameCheckBox = new System.Windows.Forms.CheckBox();
            this.autoBackupCheckBoxLabel = new System.Windows.Forms.Label();
            this.autoDeleteOldCheckBoxLabel = new System.Windows.Forms.Label();
            this.autoDeleteManyCheckBoxLabel = new System.Windows.Forms.Label();
            this.dontSkipAutoBackupsIfPlayCountsChangedCheckBoxLabel = new System.Windows.Forms.Label();
            this.dontTryToGuessLibraryNameCheckBoxLabel = new System.Windows.Forms.Label();
            this.backupArtworksCheckBoxLabel = new System.Windows.Forms.Label();
            this.storeTrackIdsInCustomTagCheckBoxLabel = new System.Windows.Forms.Label();
            this.storeTrackIdsInCustomTagCheckBox = new System.Windows.Forms.CheckBox();
            this.trackIdTagList = new System.Windows.Forms.ComboBox();
            this.dontSkipAutoBackupsIfPlayCountsChangedCheckBox = new System.Windows.Forms.CheckBox();
            this.dontSkipAutoBackupsIfPlayCountsChangedLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.emptyLabel = new System.Windows.Forms.Label();

            //MusicBee
            this.autoBackupFolderTextBox = ControlsTools.CreateMusicBeeTextBox();
            this.autoBackupPrefixTextBox = ControlsTools.CreateMusicBeeTextBox();
            //~MusicBee

            ((System.ComponentModel.ISupportInitialize)(this.autoBackupNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfDaysNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfFilesNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // autoBackupFolderTextBox
            // 
            resources.ApplyResources(this.autoBackupFolderTextBox, "autoBackupFolderTextBox");
            this.autoBackupFolderTextBox.Name = "autoBackupFolderTextBox";
            this.autoBackupFolderTextBox.ReadOnly = true;
            this.autoBackupFolderTextBox.Tag = "#buttonBrowse";
            this.toolTip1.SetToolTip(this.autoBackupFolderTextBox, resources.GetString("autoBackupFolderTextBox.ToolTip"));
            // 
            // buttonBrowse
            // 
            resources.ApplyResources(this.buttonBrowse, "buttonBrowse");
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Tag = "#AutoBackupSettings@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonBrowse, resources.GetString("buttonBrowse.ToolTip"));
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
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
            // autoDeleteOldCheckBox
            // 
            resources.ApplyResources(this.autoDeleteOldCheckBox, "autoDeleteOldCheckBox");
            this.autoDeleteOldCheckBox.Name = "autoDeleteOldCheckBox";
            this.autoDeleteOldCheckBox.Tag = "#autoDeleteOldCheckBoxLabel";
            this.toolTip1.SetToolTip(this.autoDeleteOldCheckBox, resources.GetString("autoDeleteOldCheckBox.ToolTip"));
            this.autoDeleteOldCheckBox.CheckedChanged += new System.EventHandler(this.autoDeleteOldCheckBox_CheckedChanged);
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
            this.numberOfDaysNumericUpDown.Tag = "#autoDeleteOldLabel";
            this.toolTip1.SetToolTip(this.numberOfDaysNumericUpDown, resources.GetString("numberOfDaysNumericUpDown.ToolTip"));
            // 
            // autoDeleteManyCheckBox
            // 
            resources.ApplyResources(this.autoDeleteManyCheckBox, "autoDeleteManyCheckBox");
            this.autoDeleteManyCheckBox.Name = "autoDeleteManyCheckBox";
            this.autoDeleteManyCheckBox.Tag = "#autoDeleteManyCheckBoxLabel";
            this.toolTip1.SetToolTip(this.autoDeleteManyCheckBox, resources.GetString("autoDeleteManyCheckBox.ToolTip"));
            this.autoDeleteManyCheckBox.CheckedChanged += new System.EventHandler(this.autoDeleteManyCheckBox_CheckedChanged);
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
            // autoDeleteOldLabel
            // 
            resources.ApplyResources(this.autoDeleteOldLabel, "autoDeleteOldLabel");
            this.autoDeleteOldLabel.Name = "autoDeleteOldLabel";
            this.toolTip1.SetToolTip(this.autoDeleteOldLabel, resources.GetString("autoDeleteOldLabel.ToolTip"));
            // 
            // buttonClose
            // 
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Tag = "#AutoBackupSettings@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonClose, resources.GetString("buttonClose.ToolTip"));
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Tag = "#buttonClose";
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
            // autoDeleteOldCheckBoxLabel
            // 
            resources.ApplyResources(this.autoDeleteOldCheckBoxLabel, "autoDeleteOldCheckBoxLabel");
            this.autoDeleteOldCheckBoxLabel.Name = "autoDeleteOldCheckBoxLabel";
            this.autoDeleteOldCheckBoxLabel.Tag = "#numberOfDaysNumericUpDown";
            this.toolTip1.SetToolTip(this.autoDeleteOldCheckBoxLabel, resources.GetString("autoDeleteOldCheckBoxLabel.ToolTip"));
            this.autoDeleteOldCheckBoxLabel.Click += new System.EventHandler(this.autoDeleteOldCheckBoxLabel_Click);
            // 
            // autoDeleteManyCheckBoxLabel
            // 
            resources.ApplyResources(this.autoDeleteManyCheckBoxLabel, "autoDeleteManyCheckBoxLabel");
            this.autoDeleteManyCheckBoxLabel.Name = "autoDeleteManyCheckBoxLabel";
            this.autoDeleteManyCheckBoxLabel.Tag = "#numberOfFilesNumericUpDown";
            this.toolTip1.SetToolTip(this.autoDeleteManyCheckBoxLabel, resources.GetString("autoDeleteManyCheckBoxLabel.ToolTip"));
            this.autoDeleteManyCheckBoxLabel.Click += new System.EventHandler(this.autoDeleteManyCheckBoxLabel_Click);
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
            this.dontTryToGuessLibraryNameCheckBoxLabel.Tag = "";
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
            this.trackIdTagList.Tag = "#AutoBackupSettings";
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
            // emptyLabel
            // 
            resources.ApplyResources(this.emptyLabel, "emptyLabel");
            this.emptyLabel.Name = "emptyLabel";
            this.toolTip1.SetToolTip(this.emptyLabel, resources.GetString("emptyLabel.ToolTip"));
            // 
            // AutoBackupSettings
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.emptyLabel);
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
            this.Controls.Add(this.autoDeleteManyCheckBoxLabel);
            this.Controls.Add(this.autoDeleteManyCheckBox);
            this.Controls.Add(this.autoDeleteOldLabel);
            this.Controls.Add(this.numberOfDaysNumericUpDown);
            this.Controls.Add(this.autoDeleteOldCheckBoxLabel);
            this.Controls.Add(this.autoDeleteOldCheckBox);
            this.Controls.Add(this.autoBackupPrefixTextBox);
            this.Controls.Add(this.autoBackupPrefixLabel);
            this.Controls.Add(this.autoBackupPeriodLabel);
            this.Controls.Add(this.autoBackupNumericUpDown);
            this.Controls.Add(this.autoBackupCheckBoxLabel);
            this.Controls.Add(this.autoBackupCheckBox);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.autoBackupFolderTextBox);
            this.Controls.Add(this.autoBackupFolderLabel);
            this.Controls.Add(this.dontSkipAutoBackupsIfPlayCountsChangedCheckBoxLabel);
            this.DoubleBuffered = true;
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
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.TextBox autoBackupFolderTextBox;
        private System.Windows.Forms.Label autoBackupPrefixLabel;
        private System.Windows.Forms.TextBox autoBackupPrefixTextBox;
        private System.Windows.Forms.CheckBox autoBackupCheckBox;
        private System.Windows.Forms.NumericUpDown autoBackupNumericUpDown;
        private System.Windows.Forms.Label autoBackupPeriodLabel;
        private System.Windows.Forms.CheckBox autoDeleteOldCheckBox;
        private System.Windows.Forms.NumericUpDown numberOfDaysNumericUpDown;
        private System.Windows.Forms.CheckBox autoDeleteManyCheckBox;
        private System.Windows.Forms.NumericUpDown numberOfFilesNumericUpDown;
        private System.Windows.Forms.Label autoDeleteOldLabel;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.CheckBox backupArtworksCheckBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox dontTryToGuessLibraryNameCheckBox;
        private System.Windows.Forms.CheckBox dontSkipAutoBackupsIfPlayCountsChangedCheckBox;
        private System.Windows.Forms.Label autoBackupCheckBoxLabel;
        private System.Windows.Forms.Label autoDeleteOldCheckBoxLabel;
        private System.Windows.Forms.Label autoDeleteManyCheckBoxLabel;
        private System.Windows.Forms.Label dontSkipAutoBackupsIfPlayCountsChangedCheckBoxLabel;
        private System.Windows.Forms.Label dontSkipAutoBackupsIfPlayCountsChangedLabel;
        private System.Windows.Forms.Label dontTryToGuessLibraryNameCheckBoxLabel;
        private System.Windows.Forms.Label backupArtworksCheckBoxLabel;
        private System.Windows.Forms.Label storeTrackIdsInCustomTagCheckBoxLabel;
        private System.Windows.Forms.CheckBox storeTrackIdsInCustomTagCheckBox;
        private System.Windows.Forms.ComboBox trackIdTagList;
        private System.Windows.Forms.Label emptyLabel;
    }
}