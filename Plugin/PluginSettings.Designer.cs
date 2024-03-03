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

            if (disposing)
            {
                customFont.Dispose();
            }
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
            this.playTickedAsrPresetSoundCheckBoxLabel = new System.Windows.Forms.Label();
            this.playTickedAsrPresetSoundCheckBox = new System.Windows.Forms.CheckBox();
            this.playStoppedSoundCheckBoxLabel = new System.Windows.Forms.Label();
            this.playStoppedSoundCheckBox = new System.Windows.Forms.CheckBox();
            this.playStartedSoundCheckBoxLabel = new System.Windows.Forms.Label();
            this.playStartedSoundCheckBox = new System.Windows.Forms.CheckBox();
            this.playCompletedSoundCheckBoxLabel = new System.Windows.Forms.Label();
            this.playCompletedSoundCheckBox = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.unitsPanel = new System.Windows.Forms.Panel();
            this.unitGBox = new System.Windows.Forms.TextBox();
            this.unitMBox = new System.Windows.Forms.TextBox();
            this.unitKBox = new System.Windows.Forms.TextBox();
            this.saveLastSkippedButton = new System.Windows.Forms.Button();
            this.useSkinColorsCheckBox = new System.Windows.Forms.CheckBox();
            this.closeHiddenCommandWindowsRadioButton = new System.Windows.Forms.RadioButton();
            this.showHiddenCommandWindowsRadioButton = new System.Windows.Forms.RadioButton();
            this.highlightChangedTagsCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.legendGroupBox = new System.Windows.Forms.GroupBox();
            this.preservedTagValuesLegendTextBox = new System.Windows.Forms.TextBox();
            this.preservedTagsLegendTextBox = new System.Windows.Forms.TextBox();
            this.changedLegendTextBox = new System.Windows.Forms.TextBox();
            this.legendLabel = new System.Windows.Forms.Label();
            this.highlightChangedTagsCheckBoxLabel = new System.Windows.Forms.Label();
            this.showHiddenCommandWindowsRadioButtonLabel = new System.Windows.Forms.Label();
            this.closeHiddenCommandWindowsRadioButtonLabel = new System.Windows.Forms.Label();
            this.minimizePluginWindowsCheckBoxLabel = new System.Windows.Forms.Label();
            this.minimizePluginWindowsCheckBox = new System.Windows.Forms.CheckBox();
            this.customFontButton = new System.Windows.Forms.Button();
            this.customFontTextBox = new System.Windows.Forms.TextBox();
            this.useCustomFontCheckBoxLabel = new System.Windows.Forms.Label();
            this.useCustomFontCheckBox = new System.Windows.Forms.CheckBox();
            this.useMusicBeeFontCheckBoxLabel = new System.Windows.Forms.Label();
            this.useMusicBeeFontCheckBox = new System.Windows.Forms.CheckBox();
            this.useSkinColorsCheckBoxLabel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.showReencodeTagCheckBox = new System.Windows.Forms.CheckBox();
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
            this.contextMenuCheckBoxLabel = new System.Windows.Forms.Label();
            this.allowCommandExecutionWithoutPreviewCheckBoxLabel = new System.Windows.Forms.Label();
            this.allowCommandExecutionWithoutPreviewCheckBox = new System.Windows.Forms.CheckBox();
            this.allowAsrLrPresetAutoexecutionCheckBoxLabel = new System.Windows.Forms.Label();
            this.allowAsrLrPresetAutoexecutionCheckBox = new System.Windows.Forms.CheckBox();
            this.showShowHiddenWindowsCheckBoxLabel = new System.Windows.Forms.Label();
            this.showBackupRestoreCheckBoxLabel = new System.Windows.Forms.Label();
            this.showCTCheckBoxLabel = new System.Windows.Forms.Label();
            this.showCARCheckBoxLabel = new System.Windows.Forms.Label();
            this.showASRCheckBoxLabel = new System.Windows.Forms.Label();
            this.showAutorateCheckBoxLabel = new System.Windows.Forms.Label();
            this.showLibraryReportsCheckBoxLabel = new System.Windows.Forms.Label();
            this.showReencodeTagCheckBoxLabel = new System.Windows.Forms.Label();
            this.showChangeCaseCheckBoxLabel = new System.Windows.Forms.Label();
            this.showSwapTagsCheckBoxLabel = new System.Windows.Forms.Label();
            this.showCopyTagCheckBoxLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.includePreservedTagValuesCheckBoxLabel = new System.Windows.Forms.Label();
            this.includePreservedTagValuesCheckBox = new System.Windows.Forms.CheckBox();
            this.includePreservedTagsCheckBoxLabel = new System.Windows.Forms.Label();
            this.includePreservedTagsCheckBox = new System.Windows.Forms.CheckBox();
            this.includeNotChangedTagsCheckBoxLabel = new System.Windows.Forms.Label();
            this.includeNotChangedTagsCheckBox = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonsPanel = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.unitsPanel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.legendGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.buttonsPanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();

            //MusicBee
            this.unitKBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.unitMBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.unitGBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.customFontTextBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.preservedTagValuesLegendTextBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.preservedTagsLegendTextBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.changedLegendTextBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            //~MusicBee

            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Tag = "@pinned-to-parent-x@pinned-to-parent-y@non-defaultable";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Tag = "#buttonCancel@pinned-to-parent-y";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            this.label6.Tag = "@pinned-to-parent-x";
            // 
            // versionLabel
            // 
            resources.ApplyResources(this.versionLabel, "versionLabel");
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Tag = "@pinned-to-parent-x@pinned-to-parent-y";
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.playTickedAsrPresetSoundCheckBoxLabel);
            this.groupBox3.Controls.Add(this.playTickedAsrPresetSoundCheckBox);
            this.groupBox3.Controls.Add(this.playStoppedSoundCheckBoxLabel);
            this.groupBox3.Controls.Add(this.playStoppedSoundCheckBox);
            this.groupBox3.Controls.Add(this.playStartedSoundCheckBoxLabel);
            this.groupBox3.Controls.Add(this.playStartedSoundCheckBox);
            this.groupBox3.Controls.Add(this.playCompletedSoundCheckBoxLabel);
            this.groupBox3.Controls.Add(this.playCompletedSoundCheckBox);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            this.groupBox3.Tag = "#tabPage1@pinned-to-parent-x";
            // 
            // playTickedAsrPresetSoundCheckBoxLabel
            // 
            resources.ApplyResources(this.playTickedAsrPresetSoundCheckBoxLabel, "playTickedAsrPresetSoundCheckBoxLabel");
            this.playTickedAsrPresetSoundCheckBoxLabel.Name = "playTickedAsrPresetSoundCheckBoxLabel";
            this.playTickedAsrPresetSoundCheckBoxLabel.Click += new System.EventHandler(this.playTickedAsrPresetSoundCheckBoxLabel_Click);
            // 
            // playTickedAsrPresetSoundCheckBox
            // 
            resources.ApplyResources(this.playTickedAsrPresetSoundCheckBox, "playTickedAsrPresetSoundCheckBox");
            this.playTickedAsrPresetSoundCheckBox.Name = "playTickedAsrPresetSoundCheckBox";
            this.playTickedAsrPresetSoundCheckBox.Tag = "#playTickedAsrPresetSoundCheckBoxLabel";
            // 
            // playStoppedSoundCheckBoxLabel
            // 
            resources.ApplyResources(this.playStoppedSoundCheckBoxLabel, "playStoppedSoundCheckBoxLabel");
            this.playStoppedSoundCheckBoxLabel.Name = "playStoppedSoundCheckBoxLabel";
            this.playStoppedSoundCheckBoxLabel.Click += new System.EventHandler(this.playStoppedSoundCheckBoxLabel_Click);
            // 
            // playStoppedSoundCheckBox
            // 
            resources.ApplyResources(this.playStoppedSoundCheckBox, "playStoppedSoundCheckBox");
            this.playStoppedSoundCheckBox.Name = "playStoppedSoundCheckBox";
            this.playStoppedSoundCheckBox.Tag = "#playStoppedSoundCheckBoxLabel";
            // 
            // playStartedSoundCheckBoxLabel
            // 
            resources.ApplyResources(this.playStartedSoundCheckBoxLabel, "playStartedSoundCheckBoxLabel");
            this.playStartedSoundCheckBoxLabel.Name = "playStartedSoundCheckBoxLabel";
            this.playStartedSoundCheckBoxLabel.Click += new System.EventHandler(this.playStartedSoundCheckBoxLabel_Click);
            // 
            // playStartedSoundCheckBox
            // 
            resources.ApplyResources(this.playStartedSoundCheckBox, "playStartedSoundCheckBox");
            this.playStartedSoundCheckBox.Name = "playStartedSoundCheckBox";
            this.playStartedSoundCheckBox.Tag = "#playStartedSoundCheckBoxLabel";
            // 
            // playCompletedSoundCheckBoxLabel
            // 
            resources.ApplyResources(this.playCompletedSoundCheckBoxLabel, "playCompletedSoundCheckBoxLabel");
            this.playCompletedSoundCheckBoxLabel.Name = "playCompletedSoundCheckBoxLabel";
            this.playCompletedSoundCheckBoxLabel.Click += new System.EventHandler(this.playCompletedSoundCheckBoxLabel_Click);
            // 
            // playCompletedSoundCheckBox
            // 
            resources.ApplyResources(this.playCompletedSoundCheckBox, "playCompletedSoundCheckBox");
            this.playCompletedSoundCheckBox.Name = "playCompletedSoundCheckBox";
            this.playCompletedSoundCheckBox.Tag = "#playCompletedSoundCheckBoxLabel";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.unitsPanel);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            this.groupBox4.Tag = "#tabPage2@pinned-to-parent-x";
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
            this.label3.Tag = "#unitsPanel";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // unitsPanel
            // 
            this.unitsPanel.Controls.Add(this.unitGBox);
            this.unitsPanel.Controls.Add(this.unitMBox);
            this.unitsPanel.Controls.Add(this.unitKBox);
            resources.ApplyResources(this.unitsPanel, "unitsPanel");
            this.unitsPanel.Name = "unitsPanel";
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
            // saveLastSkippedButton
            // 
            resources.ApplyResources(this.saveLastSkippedButton, "saveLastSkippedButton");
            this.saveLastSkippedButton.Name = "saveLastSkippedButton";
            this.saveLastSkippedButton.Tag = "#buttonOK@pinned-to-parent-y";
            this.saveLastSkippedButton.Click += new System.EventHandler(this.saveLastSkippedButton_Click);
            // 
            // useSkinColorsCheckBox
            // 
            resources.ApplyResources(this.useSkinColorsCheckBox, "useSkinColorsCheckBox");
            this.useSkinColorsCheckBox.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.useSkinColorsCheckBox.Name = "useSkinColorsCheckBox";
            this.useSkinColorsCheckBox.Tag = "#useSkinColorsCheckBoxLabel";
            // 
            // closeHiddenCommandWindowsRadioButton
            // 
            resources.ApplyResources(this.closeHiddenCommandWindowsRadioButton, "closeHiddenCommandWindowsRadioButton");
            this.closeHiddenCommandWindowsRadioButton.Name = "closeHiddenCommandWindowsRadioButton";
            this.closeHiddenCommandWindowsRadioButton.TabStop = true;
            this.closeHiddenCommandWindowsRadioButton.Tag = "#closeHiddenCommandWindowsRadioButtonLabel";
            // 
            // showHiddenCommandWindowsRadioButton
            // 
            resources.ApplyResources(this.showHiddenCommandWindowsRadioButton, "showHiddenCommandWindowsRadioButton");
            this.showHiddenCommandWindowsRadioButton.Name = "showHiddenCommandWindowsRadioButton";
            this.showHiddenCommandWindowsRadioButton.TabStop = true;
            this.showHiddenCommandWindowsRadioButton.Tag = "#showHiddenCommandWindowsRadioButtonLabel";
            // 
            // highlightChangedTagsCheckBox
            // 
            resources.ApplyResources(this.highlightChangedTagsCheckBox, "highlightChangedTagsCheckBox");
            this.highlightChangedTagsCheckBox.Name = "highlightChangedTagsCheckBox";
            this.highlightChangedTagsCheckBox.Tag = "#highlightChangedTagsCheckBoxLabel";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.legendGroupBox);
            this.groupBox2.Controls.Add(this.highlightChangedTagsCheckBoxLabel);
            this.groupBox2.Controls.Add(this.highlightChangedTagsCheckBox);
            this.groupBox2.Controls.Add(this.showHiddenCommandWindowsRadioButtonLabel);
            this.groupBox2.Controls.Add(this.showHiddenCommandWindowsRadioButton);
            this.groupBox2.Controls.Add(this.closeHiddenCommandWindowsRadioButtonLabel);
            this.groupBox2.Controls.Add(this.closeHiddenCommandWindowsRadioButton);
            this.groupBox2.Controls.Add(this.minimizePluginWindowsCheckBoxLabel);
            this.groupBox2.Controls.Add(this.minimizePluginWindowsCheckBox);
            this.groupBox2.Controls.Add(this.customFontButton);
            this.groupBox2.Controls.Add(this.customFontTextBox);
            this.groupBox2.Controls.Add(this.useCustomFontCheckBoxLabel);
            this.groupBox2.Controls.Add(this.useCustomFontCheckBox);
            this.groupBox2.Controls.Add(this.useMusicBeeFontCheckBoxLabel);
            this.groupBox2.Controls.Add(this.useMusicBeeFontCheckBox);
            this.groupBox2.Controls.Add(this.useSkinColorsCheckBoxLabel);
            this.groupBox2.Controls.Add(this.useSkinColorsCheckBox);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            this.groupBox2.Tag = "#tabPage1@pinned-to-parent-x";
            // 
            // legendGroupBox
            // 
            resources.ApplyResources(this.legendGroupBox, "legendGroupBox");
            this.legendGroupBox.Controls.Add(this.preservedTagValuesLegendTextBox);
            this.legendGroupBox.Controls.Add(this.preservedTagsLegendTextBox);
            this.legendGroupBox.Controls.Add(this.changedLegendTextBox);
            this.legendGroupBox.Controls.Add(this.legendLabel);
            this.legendGroupBox.Name = "legendGroupBox";
            this.legendGroupBox.TabStop = false;
            this.legendGroupBox.Tag = "#groupBox2";
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
            // legendLabel
            // 
            resources.ApplyResources(this.legendLabel, "legendLabel");
            this.legendLabel.Name = "legendLabel";
            // 
            // highlightChangedTagsCheckBoxLabel
            // 
            resources.ApplyResources(this.highlightChangedTagsCheckBoxLabel, "highlightChangedTagsCheckBoxLabel");
            this.highlightChangedTagsCheckBoxLabel.Name = "highlightChangedTagsCheckBoxLabel";
            this.highlightChangedTagsCheckBoxLabel.Click += new System.EventHandler(this.highlightChangedTagsCheckBoxLabel_Click);
            // 
            // showHiddenCommandWindowsRadioButtonLabel
            // 
            resources.ApplyResources(this.showHiddenCommandWindowsRadioButtonLabel, "showHiddenCommandWindowsRadioButtonLabel");
            this.showHiddenCommandWindowsRadioButtonLabel.Name = "showHiddenCommandWindowsRadioButtonLabel";
            this.showHiddenCommandWindowsRadioButtonLabel.Click += new System.EventHandler(this.showHiddenCommandWindowsRadioButtonLabel_Click);
            // 
            // closeHiddenCommandWindowsRadioButtonLabel
            // 
            resources.ApplyResources(this.closeHiddenCommandWindowsRadioButtonLabel, "closeHiddenCommandWindowsRadioButtonLabel");
            this.closeHiddenCommandWindowsRadioButtonLabel.Name = "closeHiddenCommandWindowsRadioButtonLabel";
            this.closeHiddenCommandWindowsRadioButtonLabel.Click += new System.EventHandler(this.closeHiddenCommandWindowsRadioButtonLabel_Click);
            // 
            // minimizePluginWindowsCheckBoxLabel
            // 
            resources.ApplyResources(this.minimizePluginWindowsCheckBoxLabel, "minimizePluginWindowsCheckBoxLabel");
            this.minimizePluginWindowsCheckBoxLabel.Name = "minimizePluginWindowsCheckBoxLabel";
            this.minimizePluginWindowsCheckBoxLabel.Click += new System.EventHandler(this.minimizePluginWindowsCheckBoxLabel_Click);
            // 
            // minimizePluginWindowsCheckBox
            // 
            resources.ApplyResources(this.minimizePluginWindowsCheckBox, "minimizePluginWindowsCheckBox");
            this.minimizePluginWindowsCheckBox.Name = "minimizePluginWindowsCheckBox";
            this.minimizePluginWindowsCheckBox.Tag = "#minimizePluginWindowsCheckBoxLabel";
            // 
            // customFontButton
            // 
            resources.ApplyResources(this.customFontButton, "customFontButton");
            this.customFontButton.Name = "customFontButton";
            this.customFontButton.Tag = "#legendGroupBox@non-defaultable";
            this.customFontButton.UseVisualStyleBackColor = true;
            this.customFontButton.Click += new System.EventHandler(this.customFontButton_Click);
            // 
            // customFontTextBox
            // 
            resources.ApplyResources(this.customFontTextBox, "customFontTextBox");
            this.customFontTextBox.Name = "customFontTextBox";
            this.customFontTextBox.ReadOnly = true;
            this.customFontTextBox.Tag = "#customFontButton";
            // 
            // useCustomFontCheckBoxLabel
            // 
            resources.ApplyResources(this.useCustomFontCheckBoxLabel, "useCustomFontCheckBoxLabel");
            this.useCustomFontCheckBoxLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.useCustomFontCheckBoxLabel.Name = "useCustomFontCheckBoxLabel";
            this.useCustomFontCheckBoxLabel.Tag = "#customFontTextBox";
            this.useCustomFontCheckBoxLabel.Click += new System.EventHandler(this.useCustomFontCheckBoxLabel_Click);
            // 
            // useCustomFontCheckBox
            // 
            resources.ApplyResources(this.useCustomFontCheckBox, "useCustomFontCheckBox");
            this.useCustomFontCheckBox.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.useCustomFontCheckBox.Name = "useCustomFontCheckBox";
            this.useCustomFontCheckBox.Tag = "#useCustomFontCheckBoxLabel";
            this.useCustomFontCheckBox.CheckedChanged += new System.EventHandler(this.useCustomFontCheckBox_CheckedChanged);
            // 
            // useMusicBeeFontCheckBoxLabel
            // 
            resources.ApplyResources(this.useMusicBeeFontCheckBoxLabel, "useMusicBeeFontCheckBoxLabel");
            this.useMusicBeeFontCheckBoxLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.useMusicBeeFontCheckBoxLabel.Name = "useMusicBeeFontCheckBoxLabel";
            this.useMusicBeeFontCheckBoxLabel.Tag = "#useCustomFontCheckBox";
            this.useMusicBeeFontCheckBoxLabel.Click += new System.EventHandler(this.useMusicBeeFontCheckBoxLabel_Click);
            // 
            // useMusicBeeFontCheckBox
            // 
            resources.ApplyResources(this.useMusicBeeFontCheckBox, "useMusicBeeFontCheckBox");
            this.useMusicBeeFontCheckBox.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.useMusicBeeFontCheckBox.Name = "useMusicBeeFontCheckBox";
            this.useMusicBeeFontCheckBox.Tag = "#useMusicBeeFontCheckBoxLabel";
            this.useMusicBeeFontCheckBox.CheckedChanged += new System.EventHandler(this.useMusicBeeFontCheckBox_CheckedChanged);
            // 
            // useSkinColorsCheckBoxLabel
            // 
            resources.ApplyResources(this.useSkinColorsCheckBoxLabel, "useSkinColorsCheckBoxLabel");
            this.useSkinColorsCheckBoxLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.useSkinColorsCheckBoxLabel.Name = "useSkinColorsCheckBoxLabel";
            this.useSkinColorsCheckBoxLabel.Click += new System.EventHandler(this.useSkinColorsCheckBoxLabel_Click);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // showReencodeTagCheckBox
            // 
            resources.ApplyResources(this.showReencodeTagCheckBox, "showReencodeTagCheckBox");
            this.showReencodeTagCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showReencodeTagCheckBox.Name = "showReencodeTagCheckBox";
            this.showReencodeTagCheckBox.Tag = "#showReencodeTagCheckBoxLabel";
            // 
            // showChangeCaseCheckBox
            // 
            resources.ApplyResources(this.showChangeCaseCheckBox, "showChangeCaseCheckBox");
            this.showChangeCaseCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showChangeCaseCheckBox.Name = "showChangeCaseCheckBox";
            this.showChangeCaseCheckBox.Tag = "#showChangeCaseCheckBoxLabel";
            // 
            // showLibraryReportsCheckBox
            // 
            resources.ApplyResources(this.showLibraryReportsCheckBox, "showLibraryReportsCheckBox");
            this.showLibraryReportsCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showLibraryReportsCheckBox.Name = "showLibraryReportsCheckBox";
            this.showLibraryReportsCheckBox.Tag = "#showLibraryReportsCheckBoxLabel";
            // 
            // showCopyTagCheckBox
            // 
            resources.ApplyResources(this.showCopyTagCheckBox, "showCopyTagCheckBox");
            this.showCopyTagCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showCopyTagCheckBox.Name = "showCopyTagCheckBox";
            this.showCopyTagCheckBox.Tag = "#showCopyTagCheckBoxLabel";
            // 
            // showAutorateCheckBox
            // 
            resources.ApplyResources(this.showAutorateCheckBox, "showAutorateCheckBox");
            this.showAutorateCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showAutorateCheckBox.Name = "showAutorateCheckBox";
            this.showAutorateCheckBox.Tag = "#showAutorateCheckBoxLabel";
            // 
            // showSwapTagsCheckBox
            // 
            resources.ApplyResources(this.showSwapTagsCheckBox, "showSwapTagsCheckBox");
            this.showSwapTagsCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showSwapTagsCheckBox.Name = "showSwapTagsCheckBox";
            this.showSwapTagsCheckBox.Tag = "#showSwapTagsCheckBoxLabel";
            // 
            // showASRCheckBox
            // 
            resources.ApplyResources(this.showASRCheckBox, "showASRCheckBox");
            this.showASRCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showASRCheckBox.Name = "showASRCheckBox";
            this.showASRCheckBox.Tag = "#showASRCheckBoxLabel";
            // 
            // showShowHiddenWindowsCheckBox
            // 
            resources.ApplyResources(this.showShowHiddenWindowsCheckBox, "showShowHiddenWindowsCheckBox");
            this.showShowHiddenWindowsCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showShowHiddenWindowsCheckBox.Name = "showShowHiddenWindowsCheckBox";
            this.showShowHiddenWindowsCheckBox.Tag = "#showShowHiddenWindowsCheckBoxLabel";
            // 
            // showCARCheckBox
            // 
            resources.ApplyResources(this.showCARCheckBox, "showCARCheckBox");
            this.showCARCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showCARCheckBox.Name = "showCARCheckBox";
            this.showCARCheckBox.Tag = "#showCARCheckBoxLabel";
            // 
            // showBackupRestoreCheckBox
            // 
            resources.ApplyResources(this.showBackupRestoreCheckBox, "showBackupRestoreCheckBox");
            this.showBackupRestoreCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showBackupRestoreCheckBox.Name = "showBackupRestoreCheckBox";
            this.showBackupRestoreCheckBox.Tag = "#showBackupRestoreCheckBoxLabel";
            // 
            // contextMenuCheckBox
            // 
            resources.ApplyResources(this.contextMenuCheckBox, "contextMenuCheckBox");
            this.contextMenuCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.contextMenuCheckBox.Name = "contextMenuCheckBox";
            this.contextMenuCheckBox.Tag = "#contextMenuCheckBoxLabel";
            // 
            // showCTCheckBox
            // 
            resources.ApplyResources(this.showCTCheckBox, "showCTCheckBox");
            this.showCTCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showCTCheckBox.Name = "showCTCheckBox";
            this.showCTCheckBox.Tag = "#showCTCheckBoxLabel";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.contextMenuCheckBoxLabel);
            this.groupBox1.Controls.Add(this.contextMenuCheckBox);
            this.groupBox1.Controls.Add(this.allowCommandExecutionWithoutPreviewCheckBoxLabel);
            this.groupBox1.Controls.Add(this.allowCommandExecutionWithoutPreviewCheckBox);
            this.groupBox1.Controls.Add(this.allowAsrLrPresetAutoexecutionCheckBoxLabel);
            this.groupBox1.Controls.Add(this.allowAsrLrPresetAutoexecutionCheckBox);
            this.groupBox1.Controls.Add(this.showShowHiddenWindowsCheckBoxLabel);
            this.groupBox1.Controls.Add(this.showShowHiddenWindowsCheckBox);
            this.groupBox1.Controls.Add(this.showBackupRestoreCheckBoxLabel);
            this.groupBox1.Controls.Add(this.showBackupRestoreCheckBox);
            this.groupBox1.Controls.Add(this.showCTCheckBoxLabel);
            this.groupBox1.Controls.Add(this.showCTCheckBox);
            this.groupBox1.Controls.Add(this.showCARCheckBoxLabel);
            this.groupBox1.Controls.Add(this.showCARCheckBox);
            this.groupBox1.Controls.Add(this.showASRCheckBoxLabel);
            this.groupBox1.Controls.Add(this.showASRCheckBox);
            this.groupBox1.Controls.Add(this.showAutorateCheckBoxLabel);
            this.groupBox1.Controls.Add(this.showAutorateCheckBox);
            this.groupBox1.Controls.Add(this.showLibraryReportsCheckBoxLabel);
            this.groupBox1.Controls.Add(this.showLibraryReportsCheckBox);
            this.groupBox1.Controls.Add(this.showReencodeTagCheckBoxLabel);
            this.groupBox1.Controls.Add(this.showReencodeTagCheckBox);
            this.groupBox1.Controls.Add(this.showChangeCaseCheckBoxLabel);
            this.groupBox1.Controls.Add(this.showChangeCaseCheckBox);
            this.groupBox1.Controls.Add(this.showSwapTagsCheckBoxLabel);
            this.groupBox1.Controls.Add(this.showSwapTagsCheckBox);
            this.groupBox1.Controls.Add(this.showCopyTagCheckBoxLabel);
            this.groupBox1.Controls.Add(this.showCopyTagCheckBox);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            this.groupBox1.Tag = "#tabPage1@pinned-to-parent-x";
            // 
            // contextMenuCheckBoxLabel
            // 
            resources.ApplyResources(this.contextMenuCheckBoxLabel, "contextMenuCheckBoxLabel");
            this.contextMenuCheckBoxLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.contextMenuCheckBoxLabel.Name = "contextMenuCheckBoxLabel";
            this.contextMenuCheckBoxLabel.Tag = "";
            this.contextMenuCheckBoxLabel.Click += new System.EventHandler(this.contextMenuCheckBoxLabel_Click);
            // 
            // allowCommandExecutionWithoutPreviewCheckBoxLabel
            // 
            resources.ApplyResources(this.allowCommandExecutionWithoutPreviewCheckBoxLabel, "allowCommandExecutionWithoutPreviewCheckBoxLabel");
            this.allowCommandExecutionWithoutPreviewCheckBoxLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.allowCommandExecutionWithoutPreviewCheckBoxLabel.Name = "allowCommandExecutionWithoutPreviewCheckBoxLabel";
            this.allowCommandExecutionWithoutPreviewCheckBoxLabel.Tag = "";
            this.allowCommandExecutionWithoutPreviewCheckBoxLabel.Click += new System.EventHandler(this.allowCommandExecutionWithoutPreviewCheckBoxLabel_Click);
            // 
            // allowCommandExecutionWithoutPreviewCheckBox
            // 
            resources.ApplyResources(this.allowCommandExecutionWithoutPreviewCheckBox, "allowCommandExecutionWithoutPreviewCheckBox");
            this.allowCommandExecutionWithoutPreviewCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.allowCommandExecutionWithoutPreviewCheckBox.Name = "allowCommandExecutionWithoutPreviewCheckBox";
            this.allowCommandExecutionWithoutPreviewCheckBox.Tag = "#allowCommandExecutionWithoutPreviewCheckBoxLabel";
            // 
            // allowAsrLrPresetAutoexecutionCheckBoxLabel
            // 
            resources.ApplyResources(this.allowAsrLrPresetAutoexecutionCheckBoxLabel, "allowAsrLrPresetAutoexecutionCheckBoxLabel");
            this.allowAsrLrPresetAutoexecutionCheckBoxLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.allowAsrLrPresetAutoexecutionCheckBoxLabel.Name = "allowAsrLrPresetAutoexecutionCheckBoxLabel";
            this.allowAsrLrPresetAutoexecutionCheckBoxLabel.Tag = "";
            this.allowAsrLrPresetAutoexecutionCheckBoxLabel.Click += new System.EventHandler(this.allowAsrLrPresetAutoexecutionCheckBoxLabel_Click);
            // 
            // allowAsrLrPresetAutoexecutionCheckBox
            // 
            resources.ApplyResources(this.allowAsrLrPresetAutoexecutionCheckBox, "allowAsrLrPresetAutoexecutionCheckBox");
            this.allowAsrLrPresetAutoexecutionCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.allowAsrLrPresetAutoexecutionCheckBox.Name = "allowAsrLrPresetAutoexecutionCheckBox";
            this.allowAsrLrPresetAutoexecutionCheckBox.Tag = "#allowAsrLrPresetAutoexecutionCheckBoxLabel";
            // 
            // showShowHiddenWindowsCheckBoxLabel
            // 
            resources.ApplyResources(this.showShowHiddenWindowsCheckBoxLabel, "showShowHiddenWindowsCheckBoxLabel");
            this.showShowHiddenWindowsCheckBoxLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showShowHiddenWindowsCheckBoxLabel.Name = "showShowHiddenWindowsCheckBoxLabel";
            this.showShowHiddenWindowsCheckBoxLabel.Tag = "";
            this.showShowHiddenWindowsCheckBoxLabel.Click += new System.EventHandler(this.showShowHiddenWindowsCheckBoxLabel_Click);
            // 
            // showBackupRestoreCheckBoxLabel
            // 
            resources.ApplyResources(this.showBackupRestoreCheckBoxLabel, "showBackupRestoreCheckBoxLabel");
            this.showBackupRestoreCheckBoxLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showBackupRestoreCheckBoxLabel.Name = "showBackupRestoreCheckBoxLabel";
            this.showBackupRestoreCheckBoxLabel.Click += new System.EventHandler(this.showBackupRestoreCheckBoxLabel_Click);
            // 
            // showCTCheckBoxLabel
            // 
            resources.ApplyResources(this.showCTCheckBoxLabel, "showCTCheckBoxLabel");
            this.showCTCheckBoxLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showCTCheckBoxLabel.Name = "showCTCheckBoxLabel";
            this.showCTCheckBoxLabel.Click += new System.EventHandler(this.showCTCheckBoxLabel_Click);
            // 
            // showCARCheckBoxLabel
            // 
            resources.ApplyResources(this.showCARCheckBoxLabel, "showCARCheckBoxLabel");
            this.showCARCheckBoxLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showCARCheckBoxLabel.Name = "showCARCheckBoxLabel";
            this.showCARCheckBoxLabel.Click += new System.EventHandler(this.showCARCheckBoxLabel_Click);
            // 
            // showASRCheckBoxLabel
            // 
            resources.ApplyResources(this.showASRCheckBoxLabel, "showASRCheckBoxLabel");
            this.showASRCheckBoxLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showASRCheckBoxLabel.Name = "showASRCheckBoxLabel";
            this.showASRCheckBoxLabel.Click += new System.EventHandler(this.showASRCheckBoxLabel_Click);
            // 
            // showAutorateCheckBoxLabel
            // 
            resources.ApplyResources(this.showAutorateCheckBoxLabel, "showAutorateCheckBoxLabel");
            this.showAutorateCheckBoxLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showAutorateCheckBoxLabel.Name = "showAutorateCheckBoxLabel";
            this.showAutorateCheckBoxLabel.Click += new System.EventHandler(this.showAutorateCheckBoxLabel_Click);
            // 
            // showLibraryReportsCheckBoxLabel
            // 
            resources.ApplyResources(this.showLibraryReportsCheckBoxLabel, "showLibraryReportsCheckBoxLabel");
            this.showLibraryReportsCheckBoxLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showLibraryReportsCheckBoxLabel.Name = "showLibraryReportsCheckBoxLabel";
            this.showLibraryReportsCheckBoxLabel.Click += new System.EventHandler(this.showLibraryReportsCheckBoxLabel_Click);
            // 
            // showReencodeTagCheckBoxLabel
            // 
            resources.ApplyResources(this.showReencodeTagCheckBoxLabel, "showReencodeTagCheckBoxLabel");
            this.showReencodeTagCheckBoxLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showReencodeTagCheckBoxLabel.Name = "showReencodeTagCheckBoxLabel";
            this.showReencodeTagCheckBoxLabel.Click += new System.EventHandler(this.showReencodeTagCheckBoxLabel_Click);
            // 
            // showChangeCaseCheckBoxLabel
            // 
            resources.ApplyResources(this.showChangeCaseCheckBoxLabel, "showChangeCaseCheckBoxLabel");
            this.showChangeCaseCheckBoxLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showChangeCaseCheckBoxLabel.Name = "showChangeCaseCheckBoxLabel";
            this.showChangeCaseCheckBoxLabel.Click += new System.EventHandler(this.showChangeCaseCheckBoxLabel_Click);
            // 
            // showSwapTagsCheckBoxLabel
            // 
            resources.ApplyResources(this.showSwapTagsCheckBoxLabel, "showSwapTagsCheckBoxLabel");
            this.showSwapTagsCheckBoxLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showSwapTagsCheckBoxLabel.Name = "showSwapTagsCheckBoxLabel";
            this.showSwapTagsCheckBoxLabel.Click += new System.EventHandler(this.showSwapTagsCheckBoxLabel_Click);
            // 
            // showCopyTagCheckBoxLabel
            // 
            resources.ApplyResources(this.showCopyTagCheckBoxLabel, "showCopyTagCheckBoxLabel");
            this.showCopyTagCheckBoxLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showCopyTagCheckBoxLabel.Name = "showCopyTagCheckBoxLabel";
            this.showCopyTagCheckBoxLabel.Click += new System.EventHandler(this.showCopyTagCheckBoxLabel_Click);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // groupBox5
            // 
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Controls.Add(this.includePreservedTagValuesCheckBoxLabel);
            this.groupBox5.Controls.Add(this.includePreservedTagValuesCheckBox);
            this.groupBox5.Controls.Add(this.includePreservedTagsCheckBoxLabel);
            this.groupBox5.Controls.Add(this.includePreservedTagsCheckBox);
            this.groupBox5.Controls.Add(this.includeNotChangedTagsCheckBoxLabel);
            this.groupBox5.Controls.Add(this.includeNotChangedTagsCheckBox);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            this.groupBox5.Tag = "#tabPage2@pinned-to-parent-x";
            // 
            // includePreservedTagValuesCheckBoxLabel
            // 
            resources.ApplyResources(this.includePreservedTagValuesCheckBoxLabel, "includePreservedTagValuesCheckBoxLabel");
            this.includePreservedTagValuesCheckBoxLabel.Name = "includePreservedTagValuesCheckBoxLabel";
            this.includePreservedTagValuesCheckBoxLabel.Click += new System.EventHandler(this.includePreservedTagValuesCheckBoxLabel_Click);
            // 
            // includePreservedTagValuesCheckBox
            // 
            resources.ApplyResources(this.includePreservedTagValuesCheckBox, "includePreservedTagValuesCheckBox");
            this.includePreservedTagValuesCheckBox.Name = "includePreservedTagValuesCheckBox";
            this.includePreservedTagValuesCheckBox.Tag = "#includePreservedTagValuesCheckBoxLabel";
            // 
            // includePreservedTagsCheckBoxLabel
            // 
            resources.ApplyResources(this.includePreservedTagsCheckBoxLabel, "includePreservedTagsCheckBoxLabel");
            this.includePreservedTagsCheckBoxLabel.Name = "includePreservedTagsCheckBoxLabel";
            this.includePreservedTagsCheckBoxLabel.Click += new System.EventHandler(this.includePreservedTagsCheckBoxLabel_Click);
            // 
            // includePreservedTagsCheckBox
            // 
            resources.ApplyResources(this.includePreservedTagsCheckBox, "includePreservedTagsCheckBox");
            this.includePreservedTagsCheckBox.Name = "includePreservedTagsCheckBox";
            this.includePreservedTagsCheckBox.Tag = "#includePreservedTagsCheckBoxLabel";
            // 
            // includeNotChangedTagsCheckBoxLabel
            // 
            resources.ApplyResources(this.includeNotChangedTagsCheckBoxLabel, "includeNotChangedTagsCheckBoxLabel");
            this.includeNotChangedTagsCheckBoxLabel.Name = "includeNotChangedTagsCheckBoxLabel";
            this.includeNotChangedTagsCheckBoxLabel.Click += new System.EventHandler(this.includeNotChangedTagsCheckBoxLabel_Click);
            // 
            // includeNotChangedTagsCheckBox
            // 
            resources.ApplyResources(this.includeNotChangedTagsCheckBox, "includeNotChangedTagsCheckBox");
            this.includeNotChangedTagsCheckBox.Name = "includeNotChangedTagsCheckBox";
            this.includeNotChangedTagsCheckBox.Tag = "#includeNotChangedTagsCheckBoxLabel";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Name = "label1";
            this.label1.Tag = "@pinned-to-parent-x";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label5.Name = "label5";
            this.label5.Tag = "@pinned-to-parent-x";
            // 
            // buttonsPanel
            // 
            resources.ApplyResources(this.buttonsPanel, "buttonsPanel");
            this.buttonsPanel.Controls.Add(this.buttonCancel);
            this.buttonsPanel.Controls.Add(this.buttonOK);
            this.buttonsPanel.Controls.Add(this.saveLastSkippedButton);
            this.buttonsPanel.Controls.Add(this.versionLabel);
            this.buttonsPanel.Name = "buttonsPanel";
            this.buttonsPanel.Tag = "#PluginSettings&PluginSettings@pinned-to-parent-x";
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Tag = "#PluginSettings@pinned-to-parent-x";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox5);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            // 
            // PluginSettings
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.buttonsPanel);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label6);
            this.Name = "PluginSettings";
            this.Tag = "@min-max-width-same@min-max-height-same";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.unitsPanel.ResumeLayout(false);
            this.unitsPanel.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.legendGroupBox.ResumeLayout(false);
            this.legendGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.buttonsPanel.ResumeLayout(false);
            this.buttonsPanel.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
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
        private System.Windows.Forms.CheckBox showReencodeTagCheckBox;
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
        private System.Windows.Forms.GroupBox legendGroupBox;
        private System.Windows.Forms.TextBox preservedTagValuesLegendTextBox;
        private System.Windows.Forms.TextBox preservedTagsLegendTextBox;
        private System.Windows.Forms.TextBox changedLegendTextBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox minimizePluginWindowsCheckBox;
        private System.Windows.Forms.Label showSwapTagsCheckBoxLabel;
        private System.Windows.Forms.Label showCopyTagCheckBoxLabel;
        private System.Windows.Forms.Label showLibraryReportsCheckBoxLabel;
        private System.Windows.Forms.Label showReencodeTagCheckBoxLabel;
        private System.Windows.Forms.Label showChangeCaseCheckBoxLabel;
        private System.Windows.Forms.Label showCARCheckBoxLabel;
        private System.Windows.Forms.Label showASRCheckBoxLabel;
        private System.Windows.Forms.Label showAutorateCheckBoxLabel;
        private System.Windows.Forms.Label contextMenuCheckBoxLabel;
        private System.Windows.Forms.Label showBackupRestoreCheckBoxLabel;
        private System.Windows.Forms.Label showShowHiddenWindowsCheckBoxLabel;
        private System.Windows.Forms.Label showCTCheckBoxLabel;
        private System.Windows.Forms.Label minimizePluginWindowsCheckBoxLabel;
        private System.Windows.Forms.Label highlightChangedTagsCheckBoxLabel;
        private System.Windows.Forms.Label useSkinColorsCheckBoxLabel;
        private System.Windows.Forms.Label showHiddenCommandWindowsRadioButtonLabel;
        private System.Windows.Forms.Label closeHiddenCommandWindowsRadioButtonLabel;
        private System.Windows.Forms.Label playStartedSoundCheckBoxLabel;
        private System.Windows.Forms.Label playCompletedSoundCheckBoxLabel;
        private System.Windows.Forms.Label playTickedAsrPresetSoundCheckBoxLabel;
        private System.Windows.Forms.Label playStoppedSoundCheckBoxLabel;
        private System.Windows.Forms.Label includePreservedTagValuesCheckBoxLabel;
        private System.Windows.Forms.Label includePreservedTagsCheckBoxLabel;
        private System.Windows.Forms.Label includeNotChangedTagsCheckBoxLabel;
        private System.Windows.Forms.Label allowCommandExecutionWithoutPreviewCheckBoxLabel;
        private System.Windows.Forms.CheckBox allowCommandExecutionWithoutPreviewCheckBox;
        private System.Windows.Forms.Label legendLabel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label allowAsrLrPresetAutoexecutionCheckBoxLabel;
        private System.Windows.Forms.CheckBox allowAsrLrPresetAutoexecutionCheckBox;
        private System.Windows.Forms.Panel buttonsPanel;
        private System.Windows.Forms.Panel unitsPanel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label useMusicBeeFontCheckBoxLabel;
        private System.Windows.Forms.CheckBox useMusicBeeFontCheckBox;
        private System.Windows.Forms.TextBox customFontTextBox;
        private System.Windows.Forms.Label useCustomFontCheckBoxLabel;
        private System.Windows.Forms.CheckBox useCustomFontCheckBox;
        private System.Windows.Forms.Button customFontButton;
    }
}