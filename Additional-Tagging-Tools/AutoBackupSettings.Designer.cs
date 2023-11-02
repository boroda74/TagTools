namespace MusicBeePlugin
{
    partial class AutoBackupSettings
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.autobackupPrefixTextBox = new System.Windows.Forms.TextBox();
            this.autobackupCheckBox = new System.Windows.Forms.CheckBox();
            this.autobackupNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.autodeleteOldCheckBox = new System.Windows.Forms.CheckBox();
            this.numberOfDaysNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.autodeleteManyCheckBox = new System.Windows.Forms.CheckBox();
            this.numberOfFilesNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.backupArtworksCheckBox = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dontTryToGuessLibraryNameCheckBox = new System.Windows.Forms.CheckBox();
            this.autobackupCheckBoxLabel = new System.Windows.Forms.Label();
            this.autodeleteOldCheckBoxLabel = new System.Windows.Forms.Label();
            this.autodeleteManyCheckBoxLabel = new System.Windows.Forms.Label();
            this.DontSkipAutobackupsIfPlayCountsChangedCheckBoxLabel = new System.Windows.Forms.Label();
            this.dontTryToGuessLibraryNameCheckBoxLabel = new System.Windows.Forms.Label();
            this.backupArtworksCheckBoxLabel = new System.Windows.Forms.Label();
            this.DontSkipAutobackupsIfPlayCountsChangedCheckBox = new System.Windows.Forms.CheckBox();
            this.DontSkipAutobackupsIfPlayCountsChangedCheckBoxLabelLabel = new System.Windows.Forms.Label();
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
            // 
            // browseButton
            // 
            resources.ApplyResources(this.browseButton, "browseButton");
            this.browseButton.Name = "browseButton";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // autobackupPrefixTextBox
            // 
            resources.ApplyResources(this.autobackupPrefixTextBox, "autobackupPrefixTextBox");
            this.autobackupPrefixTextBox.Name = "autobackupPrefixTextBox";
            // 
            // autobackupCheckBox
            // 
            resources.ApplyResources(this.autobackupCheckBox, "autobackupCheckBox");
            this.autobackupCheckBox.Name = "autobackupCheckBox";
            this.autobackupCheckBox.Tag = "autobackupCheckBoxLabel";
            this.toolTip1.SetToolTip(this.autobackupCheckBox, resources.GetString("autobackupCheckBox.ToolTip"));
            this.autobackupCheckBox.UseVisualStyleBackColor = true;
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
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // autodeleteOldCheckBox
            // 
            resources.ApplyResources(this.autodeleteOldCheckBox, "autodeleteOldCheckBox");
            this.autodeleteOldCheckBox.Name = "autodeleteOldCheckBox";
            this.autodeleteOldCheckBox.Tag = "autodeleteOldCheckBoxLabel";
            this.toolTip1.SetToolTip(this.autodeleteOldCheckBox, resources.GetString("autodeleteOldCheckBox.ToolTip"));
            this.autodeleteOldCheckBox.UseVisualStyleBackColor = true;
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
            // 
            // autodeleteManyCheckBox
            // 
            resources.ApplyResources(this.autodeleteManyCheckBox, "autodeleteManyCheckBox");
            this.autodeleteManyCheckBox.Name = "autodeleteManyCheckBox";
            this.autodeleteManyCheckBox.Tag = "autodeleteManyCheckBoxLabel";
            this.toolTip1.SetToolTip(this.autodeleteManyCheckBox, resources.GetString("autodeleteManyCheckBox.ToolTip"));
            this.autodeleteManyCheckBox.UseVisualStyleBackColor = true;
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
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // buttonClose
            // 
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // backupArtworksCheckBox
            // 
            resources.ApplyResources(this.backupArtworksCheckBox, "backupArtworksCheckBox");
            this.backupArtworksCheckBox.Name = "backupArtworksCheckBox";
            this.backupArtworksCheckBox.Tag = "backupArtworksCheckBoxLabel";
            this.toolTip1.SetToolTip(this.backupArtworksCheckBox, resources.GetString("backupArtworksCheckBox.ToolTip"));
            this.backupArtworksCheckBox.UseVisualStyleBackColor = true;
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
            this.dontTryToGuessLibraryNameCheckBox.Tag = "dontTryToGuessLibraryNameCheckBoxLabel";
            this.toolTip1.SetToolTip(this.dontTryToGuessLibraryNameCheckBox, resources.GetString("dontTryToGuessLibraryNameCheckBox.ToolTip"));
            this.dontTryToGuessLibraryNameCheckBox.UseVisualStyleBackColor = true;
            // 
            // autobackupCheckBoxLabel
            // 
            resources.ApplyResources(this.autobackupCheckBoxLabel, "autobackupCheckBoxLabel");
            this.autobackupCheckBoxLabel.Name = "autobackupCheckBoxLabel";
            this.toolTip1.SetToolTip(this.autobackupCheckBoxLabel, resources.GetString("autobackupCheckBoxLabel.ToolTip"));
            this.autobackupCheckBoxLabel.Click += new System.EventHandler(this.autobackupCheckBoxLabel_Click);
            // 
            // autodeleteOldCheckBoxLabel
            // 
            resources.ApplyResources(this.autodeleteOldCheckBoxLabel, "autodeleteOldCheckBoxLabel");
            this.autodeleteOldCheckBoxLabel.Name = "autodeleteOldCheckBoxLabel";
            this.toolTip1.SetToolTip(this.autodeleteOldCheckBoxLabel, resources.GetString("autodeleteOldCheckBoxLabel.ToolTip"));
            this.autodeleteOldCheckBoxLabel.Click += new System.EventHandler(this.autodeleteOldCheckBoxLabel_Click);
            // 
            // autodeleteManyCheckBoxLabel
            // 
            resources.ApplyResources(this.autodeleteManyCheckBoxLabel, "autodeleteManyCheckBoxLabel");
            this.autodeleteManyCheckBoxLabel.Name = "autodeleteManyCheckBoxLabel";
            this.toolTip1.SetToolTip(this.autodeleteManyCheckBoxLabel, resources.GetString("autodeleteManyCheckBoxLabel.ToolTip"));
            this.autodeleteManyCheckBoxLabel.Click += new System.EventHandler(this.autodeleteManyCheckBoxLabel_Click);
            // 
            // DontSkipAutobackupsIfPlayCountsChangedCheckBoxLabel
            // 
            resources.ApplyResources(this.DontSkipAutobackupsIfPlayCountsChangedCheckBoxLabel, "DontSkipAutobackupsIfPlayCountsChangedCheckBoxLabel");
            this.DontSkipAutobackupsIfPlayCountsChangedCheckBoxLabel.Name = "DontSkipAutobackupsIfPlayCountsChangedCheckBoxLabel";
            this.DontSkipAutobackupsIfPlayCountsChangedCheckBoxLabel.Tag = "DontSkipAutobackupsIfPlayCountsChangedCheckBoxLabelLabel";
            this.toolTip1.SetToolTip(this.DontSkipAutobackupsIfPlayCountsChangedCheckBoxLabel, resources.GetString("DontSkipAutobackupsIfPlayCountsChangedCheckBoxLabel.ToolTip"));
            // 
            // dontTryToGuessLibraryNameCheckBoxLabel
            // 
            resources.ApplyResources(this.dontTryToGuessLibraryNameCheckBoxLabel, "dontTryToGuessLibraryNameCheckBoxLabel");
            this.dontTryToGuessLibraryNameCheckBoxLabel.Name = "dontTryToGuessLibraryNameCheckBoxLabel";
            this.toolTip1.SetToolTip(this.dontTryToGuessLibraryNameCheckBoxLabel, resources.GetString("dontTryToGuessLibraryNameCheckBoxLabel.ToolTip"));
            // 
            // backupArtworksCheckBoxLabel
            // 
            resources.ApplyResources(this.backupArtworksCheckBoxLabel, "backupArtworksCheckBoxLabel");
            this.backupArtworksCheckBoxLabel.Name = "backupArtworksCheckBoxLabel";
            this.toolTip1.SetToolTip(this.backupArtworksCheckBoxLabel, resources.GetString("backupArtworksCheckBoxLabel.ToolTip"));
            // 
            // DontSkipAutobackupsIfPlayCountsChangedCheckBox
            // 
            resources.ApplyResources(this.DontSkipAutobackupsIfPlayCountsChangedCheckBox, "DontSkipAutobackupsIfPlayCountsChangedCheckBox");
            this.DontSkipAutobackupsIfPlayCountsChangedCheckBox.Name = "DontSkipAutobackupsIfPlayCountsChangedCheckBox";
            this.DontSkipAutobackupsIfPlayCountsChangedCheckBox.Tag = "DontSkipAutobackupsIfPlayCountsChangedCheckBoxLabel";
            this.DontSkipAutobackupsIfPlayCountsChangedCheckBox.UseVisualStyleBackColor = true;
            // 
            // DontSkipAutobackupsIfPlayCountsChangedCheckBoxLabelLabel
            // 
            resources.ApplyResources(this.DontSkipAutobackupsIfPlayCountsChangedCheckBoxLabelLabel, "DontSkipAutobackupsIfPlayCountsChangedCheckBoxLabelLabel");
            this.DontSkipAutobackupsIfPlayCountsChangedCheckBoxLabelLabel.Name = "DontSkipAutobackupsIfPlayCountsChangedCheckBoxLabelLabel";
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
            this.Controls.Add(this.backupArtworksCheckBoxLabel);
            this.Controls.Add(this.dontTryToGuessLibraryNameCheckBoxLabel);
            this.Controls.Add(this.DontSkipAutobackupsIfPlayCountsChangedCheckBoxLabelLabel);
            this.Controls.Add(this.autodeleteManyCheckBoxLabel);
            this.Controls.Add(this.autodeleteOldCheckBoxLabel);
            this.Controls.Add(this.autobackupCheckBoxLabel);
            this.Controls.Add(this.DontSkipAutobackupsIfPlayCountsChangedCheckBoxLabel);
            this.Controls.Add(this.DontSkipAutobackupsIfPlayCountsChangedCheckBox);
            this.Controls.Add(this.dontTryToGuessLibraryNameCheckBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.backupArtworksCheckBox);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numberOfFilesNumericUpDown);
            this.Controls.Add(this.autodeleteManyCheckBox);
            this.Controls.Add(this.numberOfDaysNumericUpDown);
            this.Controls.Add(this.autodeleteOldCheckBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.autobackupNumericUpDown);
            this.Controls.Add(this.autobackupCheckBox);
            this.Controls.Add(this.autobackupPrefixTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.autobackupFolderTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AutoBackupSettings";
            ((System.ComponentModel.ISupportInitialize)(this.autobackupNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfDaysNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfFilesNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.TextBox autobackupFolderTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox autobackupPrefixTextBox;
        private System.Windows.Forms.CheckBox autobackupCheckBox;
        private System.Windows.Forms.NumericUpDown autobackupNumericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox autodeleteOldCheckBox;
        private System.Windows.Forms.NumericUpDown numberOfDaysNumericUpDown;
        private System.Windows.Forms.CheckBox autodeleteManyCheckBox;
        private System.Windows.Forms.NumericUpDown numberOfFilesNumericUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.CheckBox backupArtworksCheckBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox dontTryToGuessLibraryNameCheckBox;
        private System.Windows.Forms.CheckBox DontSkipAutobackupsIfPlayCountsChangedCheckBox;
        private System.Windows.Forms.Label autobackupCheckBoxLabel;
        private System.Windows.Forms.Label autodeleteOldCheckBoxLabel;
        private System.Windows.Forms.Label autodeleteManyCheckBoxLabel;
        private System.Windows.Forms.Label DontSkipAutobackupsIfPlayCountsChangedCheckBoxLabel;
        private System.Windows.Forms.Label DontSkipAutobackupsIfPlayCountsChangedCheckBoxLabelLabel;
        private System.Windows.Forms.Label dontTryToGuessLibraryNameCheckBoxLabel;
        private System.Windows.Forms.Label backupArtworksCheckBoxLabel;
    }
}