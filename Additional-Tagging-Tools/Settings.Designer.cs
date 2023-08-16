namespace MusicBeePlugin
{
    partial class PluginSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginSettings));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.playTickedAsrPresetSoundCheckBox = new System.Windows.Forms.CheckBox();
            this.playStoppedSoundCheckBox = new System.Windows.Forms.CheckBox();
            this.playStartedSoundCheckBox = new System.Windows.Forms.CheckBox();
            this.playCompletedSoundCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.unitGBox = new System.Windows.Forms.TextBox();
            this.unitMBox = new System.Windows.Forms.TextBox();
            this.unitKBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.saveLastSkippedButton = new System.Windows.Forms.Button();
            this.useSkinColorsCheckBox = new System.Windows.Forms.CheckBox();
            this.closeHiddenCommandWindowsRadioButton = new System.Windows.Forms.RadioButton();
            this.showHiddenCommandWindowsRadioButton = new System.Windows.Forms.RadioButton();
            this.highlightChangedTagsCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.minimizePluginWindowsCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.preservedTagValuesLegendTextBox = new System.Windows.Forms.TextBox();
            this.preservedTagsLegendTextBox = new System.Windows.Forms.TextBox();
            this.changedLegendTextBox = new System.Windows.Forms.TextBox();
            this.showRencodeTagCheckBox = new System.Windows.Forms.CheckBox();
            this.showChangeCaseCheckBox = new System.Windows.Forms.CheckBox();
            this.showLibraryReportsCheckBox = new System.Windows.Forms.CheckBox();
            this.showCopyTagCheckBox = new System.Windows.Forms.CheckBox();
            this.showAutorateCheckBox = new System.Windows.Forms.CheckBox();
            this.showSwapTagsCheckBox = new System.Windows.Forms.CheckBox();
            this.showASRCheckBox = new System.Windows.Forms.CheckBox();
            this.showShowHiddenWindowsCheckBox = new System.Windows.Forms.CheckBox();
            this.showCARCheckBox = new System.Windows.Forms.CheckBox();
            this.showBackupRestoreCheckBox = new System.Windows.Forms.CheckBox();
            this.contextMenuCheckBox = new System.Windows.Forms.CheckBox();
            this.showCTCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.includePreservedTagValuesCheckBox = new System.Windows.Forms.CheckBox();
            this.includePreservedTagsCheckBox = new System.Windows.Forms.CheckBox();
            this.includeNotChangedTagsCheckBox = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            //MusicBee
            this.unitKBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.unitMBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.unitGBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            //~MusicBee
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // versionLabel
            // 
            resources.ApplyResources(this.versionLabel, "versionLabel");
            this.versionLabel.Name = "versionLabel";
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.playTickedAsrPresetSoundCheckBox);
            this.groupBox3.Controls.Add(this.playStoppedSoundCheckBox);
            this.groupBox3.Controls.Add(this.playStartedSoundCheckBox);
            this.groupBox3.Controls.Add(this.playCompletedSoundCheckBox);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // playTickedAsrPresetSoundCheckBox
            // 
            resources.ApplyResources(this.playTickedAsrPresetSoundCheckBox, "playTickedAsrPresetSoundCheckBox");
            this.playTickedAsrPresetSoundCheckBox.Name = "playTickedAsrPresetSoundCheckBox";
            this.playTickedAsrPresetSoundCheckBox.UseVisualStyleBackColor = true;
            // 
            // playStoppedSoundCheckBox
            // 
            resources.ApplyResources(this.playStoppedSoundCheckBox, "playStoppedSoundCheckBox");
            this.playStoppedSoundCheckBox.Name = "playStoppedSoundCheckBox";
            this.playStoppedSoundCheckBox.UseVisualStyleBackColor = true;
            // 
            // playStartedSoundCheckBox
            // 
            resources.ApplyResources(this.playStartedSoundCheckBox, "playStartedSoundCheckBox");
            this.playStartedSoundCheckBox.Name = "playStartedSoundCheckBox";
            this.playStartedSoundCheckBox.UseVisualStyleBackColor = true;
            // 
            // playCompletedSoundCheckBox
            // 
            resources.ApplyResources(this.playCompletedSoundCheckBox, "playCompletedSoundCheckBox");
            this.playCompletedSoundCheckBox.Name = "playCompletedSoundCheckBox";
            this.playCompletedSoundCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.unitGBox);
            this.groupBox4.Controls.Add(this.unitMBox);
            this.groupBox4.Controls.Add(this.unitKBox);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // unitGBox
            // 
            resources.ApplyResources(this.unitGBox, "unitGBox");
            this.unitGBox.Name = "unitGBox";
            // 
            // unitMBox
            // 
            resources.ApplyResources(this.unitMBox, "unitMBox");
            this.unitMBox.Name = "unitMBox";
            // 
            // unitKBox
            // 
            resources.ApplyResources(this.unitKBox, "unitKBox");
            this.unitKBox.Name = "unitKBox";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // saveLastSkippedButton
            // 
            resources.ApplyResources(this.saveLastSkippedButton, "saveLastSkippedButton");
            this.saveLastSkippedButton.Name = "saveLastSkippedButton";
            this.saveLastSkippedButton.UseVisualStyleBackColor = true;
            this.saveLastSkippedButton.Click += new System.EventHandler(this.saveLastSkippedButton_Click);
            // 
            // useSkinColorsCheckBox
            // 
            resources.ApplyResources(this.useSkinColorsCheckBox, "useSkinColorsCheckBox");
            this.useSkinColorsCheckBox.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.useSkinColorsCheckBox.Name = "useSkinColorsCheckBox";
            this.useSkinColorsCheckBox.UseVisualStyleBackColor = true;
            // 
            // closeHiddenCommandWindowsRadioButton
            // 
            resources.ApplyResources(this.closeHiddenCommandWindowsRadioButton, "closeHiddenCommandWindowsRadioButton");
            this.closeHiddenCommandWindowsRadioButton.Name = "closeHiddenCommandWindowsRadioButton";
            this.closeHiddenCommandWindowsRadioButton.TabStop = true;
            this.closeHiddenCommandWindowsRadioButton.UseVisualStyleBackColor = true;
            // 
            // showHiddenCommandWindowsRadioButton
            // 
            resources.ApplyResources(this.showHiddenCommandWindowsRadioButton, "showHiddenCommandWindowsRadioButton");
            this.showHiddenCommandWindowsRadioButton.Name = "showHiddenCommandWindowsRadioButton";
            this.showHiddenCommandWindowsRadioButton.TabStop = true;
            this.showHiddenCommandWindowsRadioButton.UseVisualStyleBackColor = true;
            // 
            // highlightChangedTagsCheckBox
            // 
            resources.ApplyResources(this.highlightChangedTagsCheckBox, "highlightChangedTagsCheckBox");
            this.highlightChangedTagsCheckBox.Name = "highlightChangedTagsCheckBox";
            this.highlightChangedTagsCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.minimizePluginWindowsCheckBox);
            this.groupBox2.Controls.Add(this.groupBox6);
            this.groupBox2.Controls.Add(this.highlightChangedTagsCheckBox);
            this.groupBox2.Controls.Add(this.showHiddenCommandWindowsRadioButton);
            this.groupBox2.Controls.Add(this.closeHiddenCommandWindowsRadioButton);
            this.groupBox2.Controls.Add(this.useSkinColorsCheckBox);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // minimizePluginWindowsCheckBox
            // 
            resources.ApplyResources(this.minimizePluginWindowsCheckBox, "minimizePluginWindowsCheckBox");
            this.minimizePluginWindowsCheckBox.Name = "minimizePluginWindowsCheckBox";
            this.minimizePluginWindowsCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Controls.Add(this.preservedTagValuesLegendTextBox);
            this.groupBox6.Controls.Add(this.preservedTagsLegendTextBox);
            this.groupBox6.Controls.Add(this.changedLegendTextBox);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // preservedTagValuesLegendTextBox
            // 
            resources.ApplyResources(this.preservedTagValuesLegendTextBox, "preservedTagValuesLegendTextBox");
            this.preservedTagValuesLegendTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.preservedTagValuesLegendTextBox.Name = "preservedTagValuesLegendTextBox";
            this.preservedTagValuesLegendTextBox.ReadOnly = true;
            this.preservedTagValuesLegendTextBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.preservedTagValuesLegendTextBox_MouseClick);
            // 
            // preservedTagsLegendTextBox
            // 
            resources.ApplyResources(this.preservedTagsLegendTextBox, "preservedTagsLegendTextBox");
            this.preservedTagsLegendTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.preservedTagsLegendTextBox.Name = "preservedTagsLegendTextBox";
            this.preservedTagsLegendTextBox.ReadOnly = true;
            this.preservedTagsLegendTextBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.preservedTagsLegendTextBox_MouseClick);
            // 
            // changedLegendTextBox
            // 
            resources.ApplyResources(this.changedLegendTextBox, "changedLegendTextBox");
            this.changedLegendTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.changedLegendTextBox.Name = "changedLegendTextBox";
            this.changedLegendTextBox.ReadOnly = true;
            this.toolTip1.SetToolTip(this.changedLegendTextBox, resources.GetString("changedLegendTextBox.ToolTip"));
            this.changedLegendTextBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.changedLegendTextBox_MouseClick);
            // 
            // showRencodeTagCheckBox
            // 
            resources.ApplyResources(this.showRencodeTagCheckBox, "showRencodeTagCheckBox");
            this.showRencodeTagCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showRencodeTagCheckBox.Name = "showRencodeTagCheckBox";
            this.showRencodeTagCheckBox.UseVisualStyleBackColor = true;
            // 
            // showChangeCaseCheckBox
            // 
            resources.ApplyResources(this.showChangeCaseCheckBox, "showChangeCaseCheckBox");
            this.showChangeCaseCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showChangeCaseCheckBox.Name = "showChangeCaseCheckBox";
            this.showChangeCaseCheckBox.UseVisualStyleBackColor = true;
            // 
            // showLibraryReportsCheckBox
            // 
            resources.ApplyResources(this.showLibraryReportsCheckBox, "showLibraryReportsCheckBox");
            this.showLibraryReportsCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showLibraryReportsCheckBox.Name = "showLibraryReportsCheckBox";
            this.showLibraryReportsCheckBox.UseVisualStyleBackColor = true;
            // 
            // showCopyTagCheckBox
            // 
            resources.ApplyResources(this.showCopyTagCheckBox, "showCopyTagCheckBox");
            this.showCopyTagCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showCopyTagCheckBox.Name = "showCopyTagCheckBox";
            this.showCopyTagCheckBox.UseVisualStyleBackColor = true;
            // 
            // showAutorateCheckBox
            // 
            resources.ApplyResources(this.showAutorateCheckBox, "showAutorateCheckBox");
            this.showAutorateCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showAutorateCheckBox.Name = "showAutorateCheckBox";
            this.showAutorateCheckBox.UseVisualStyleBackColor = true;
            // 
            // showSwapTagsCheckBox
            // 
            resources.ApplyResources(this.showSwapTagsCheckBox, "showSwapTagsCheckBox");
            this.showSwapTagsCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showSwapTagsCheckBox.Name = "showSwapTagsCheckBox";
            this.showSwapTagsCheckBox.UseVisualStyleBackColor = true;
            // 
            // showASRCheckBox
            // 
            resources.ApplyResources(this.showASRCheckBox, "showASRCheckBox");
            this.showASRCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showASRCheckBox.Name = "showASRCheckBox";
            this.showASRCheckBox.UseVisualStyleBackColor = true;
            // 
            // showShowHiddenWindowsCheckBox
            // 
            resources.ApplyResources(this.showShowHiddenWindowsCheckBox, "showShowHiddenWindowsCheckBox");
            this.showShowHiddenWindowsCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showShowHiddenWindowsCheckBox.Name = "showShowHiddenWindowsCheckBox";
            this.showShowHiddenWindowsCheckBox.UseVisualStyleBackColor = true;
            // 
            // showCARCheckBox
            // 
            resources.ApplyResources(this.showCARCheckBox, "showCARCheckBox");
            this.showCARCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showCARCheckBox.Name = "showCARCheckBox";
            this.showCARCheckBox.UseVisualStyleBackColor = true;
            // 
            // showBackupRestoreCheckBox
            // 
            resources.ApplyResources(this.showBackupRestoreCheckBox, "showBackupRestoreCheckBox");
            this.showBackupRestoreCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showBackupRestoreCheckBox.Name = "showBackupRestoreCheckBox";
            this.showBackupRestoreCheckBox.UseVisualStyleBackColor = true;
            // 
            // contextMenuCheckBox
            // 
            resources.ApplyResources(this.contextMenuCheckBox, "contextMenuCheckBox");
            this.contextMenuCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.contextMenuCheckBox.Name = "contextMenuCheckBox";
            this.contextMenuCheckBox.UseVisualStyleBackColor = true;
            // 
            // showCTCheckBox
            // 
            resources.ApplyResources(this.showCTCheckBox, "showCTCheckBox");
            this.showCTCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showCTCheckBox.Name = "showCTCheckBox";
            this.showCTCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.showCTCheckBox);
            this.groupBox1.Controls.Add(this.contextMenuCheckBox);
            this.groupBox1.Controls.Add(this.showBackupRestoreCheckBox);
            this.groupBox1.Controls.Add(this.showCARCheckBox);
            this.groupBox1.Controls.Add(this.showShowHiddenWindowsCheckBox);
            this.groupBox1.Controls.Add(this.showASRCheckBox);
            this.groupBox1.Controls.Add(this.showSwapTagsCheckBox);
            this.groupBox1.Controls.Add(this.showAutorateCheckBox);
            this.groupBox1.Controls.Add(this.showCopyTagCheckBox);
            this.groupBox1.Controls.Add(this.showLibraryReportsCheckBox);
            this.groupBox1.Controls.Add(this.showChangeCaseCheckBox);
            this.groupBox1.Controls.Add(this.showRencodeTagCheckBox);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox5
            // 
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Controls.Add(this.includePreservedTagValuesCheckBox);
            this.groupBox5.Controls.Add(this.includePreservedTagsCheckBox);
            this.groupBox5.Controls.Add(this.includeNotChangedTagsCheckBox);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // includePreservedTagValuesCheckBox
            // 
            resources.ApplyResources(this.includePreservedTagValuesCheckBox, "includePreservedTagValuesCheckBox");
            this.includePreservedTagValuesCheckBox.Name = "includePreservedTagValuesCheckBox";
            this.includePreservedTagValuesCheckBox.UseVisualStyleBackColor = true;
            // 
            // includePreservedTagsCheckBox
            // 
            resources.ApplyResources(this.includePreservedTagsCheckBox, "includePreservedTagsCheckBox");
            this.includePreservedTagsCheckBox.Name = "includePreservedTagsCheckBox";
            this.includePreservedTagsCheckBox.UseVisualStyleBackColor = true;
            // 
            // includeNotChangedTagsCheckBox
            // 
            resources.ApplyResources(this.includeNotChangedTagsCheckBox, "includeNotChangedTagsCheckBox");
            this.includeNotChangedTagsCheckBox.Name = "includeNotChangedTagsCheckBox";
            this.includeNotChangedTagsCheckBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Name = "label1";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label5.Name = "label5";
            // 
            // PluginSettings
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.saveLastSkippedButton);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PluginSettings";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox playStoppedSoundCheckBox;
        private System.Windows.Forms.CheckBox playStartedSoundCheckBox;
        private System.Windows.Forms.CheckBox playCompletedSoundCheckBox;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox unitGBox;
        private System.Windows.Forms.TextBox unitMBox;
        private System.Windows.Forms.TextBox unitKBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button saveLastSkippedButton;
        private System.Windows.Forms.CheckBox playTickedAsrPresetSoundCheckBox;
        private System.Windows.Forms.CheckBox useSkinColorsCheckBox;
        private System.Windows.Forms.RadioButton closeHiddenCommandWindowsRadioButton;
        private System.Windows.Forms.RadioButton showHiddenCommandWindowsRadioButton;
        private System.Windows.Forms.CheckBox highlightChangedTagsCheckBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox showRencodeTagCheckBox;
        private System.Windows.Forms.CheckBox showChangeCaseCheckBox;
        private System.Windows.Forms.CheckBox showLibraryReportsCheckBox;
        private System.Windows.Forms.CheckBox showCopyTagCheckBox;
        private System.Windows.Forms.CheckBox showAutorateCheckBox;
        private System.Windows.Forms.CheckBox showSwapTagsCheckBox;
        private System.Windows.Forms.CheckBox showASRCheckBox;
        private System.Windows.Forms.CheckBox showShowHiddenWindowsCheckBox;
        private System.Windows.Forms.CheckBox showCARCheckBox;
        private System.Windows.Forms.CheckBox showBackupRestoreCheckBox;
        private System.Windows.Forms.CheckBox contextMenuCheckBox;
        private System.Windows.Forms.CheckBox showCTCheckBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox includeNotChangedTagsCheckBox;
        private System.Windows.Forms.CheckBox includePreservedTagsCheckBox;
        private System.Windows.Forms.CheckBox includePreservedTagValuesCheckBox;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox preservedTagValuesLegendTextBox;
        private System.Windows.Forms.TextBox preservedTagsLegendTextBox;
        private System.Windows.Forms.TextBox changedLegendTextBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox minimizePluginWindowsCheckBox;
    }
}