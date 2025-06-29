namespace MusicBeePlugin
{
    partial class QuickSettings
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

            if (disposing)
            {
                customFont.Dispose();
            }
        }

        #region Код, автоматически созданный конструктором форм Windows

        ///<summary>
        ///Обязательный метод для поддержки конструктора - не изменяйте
        ///содержимое данного метода при помощи редактора кода.
        ///</summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuickSettings));
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
            this.useSkinColorsCheckBox = new System.Windows.Forms.CheckBox();
            this.closeHiddenCommandWindowsRadioButton = new System.Windows.Forms.RadioButton();
            this.showHiddenCommandWindowsRadioButton = new System.Windows.Forms.RadioButton();
            this.highlightChangedTagsCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.scrollPreviewToEndCheckBoxLabel = new System.Windows.Forms.Label();
            this.scrollPreviewToEndCheckBox = new System.Windows.Forms.CheckBox();
            this.legendGroupBox = new System.Windows.Forms.GroupBox();
            this.preservedTagValuesLegendTextBox = new System.Windows.Forms.TextBox();
            this.preservedTagsLegendTextBox = new System.Windows.Forms.TextBox();
            this.changedLegendTextBox = new System.Windows.Forms.TextBox();
            this.legendLabel = new System.Windows.Forms.Label();
            this.showHiddenCommandWindowsRadioButtonLabel = new System.Windows.Forms.Label();
            this.closeHiddenCommandWindowsRadioButtonLabel = new System.Windows.Forms.Label();
            this.hidePluginWindowsOnMinimizationCheckBoxLabel = new System.Windows.Forms.Label();
            this.hidePluginWindowsOnMinimizationCheckBox = new System.Windows.Forms.CheckBox();
            this.customFontButton = new System.Windows.Forms.Button();
            this.customFontTextBox = new System.Windows.Forms.TextBox();
            this.useCustomFontCheckBoxLabel = new System.Windows.Forms.Label();
            this.useCustomFontCheckBox = new System.Windows.Forms.CheckBox();
            this.useMusicBeeFontCheckBoxLabel = new System.Windows.Forms.Label();
            this.useMusicBeeFontCheckBox = new System.Windows.Forms.CheckBox();
            this.highlightChangedTagsCheckBoxLabel = new System.Windows.Forms.Label();
            this.useSkinColorsCheckBoxLabel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.allowCommandExecutionWithoutPreviewCheckBoxLabel = new System.Windows.Forms.Label();
            this.allowCommandExecutionWithoutPreviewCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.includePreservedTagValuesCheckBoxLabel = new System.Windows.Forms.Label();
            this.includePreservedTagValuesCheckBox = new System.Windows.Forms.CheckBox();
            this.includePreservedTagsCheckBoxLabel = new System.Windows.Forms.Label();
            this.includePreservedTagsCheckBox = new System.Windows.Forms.CheckBox();
            this.includeNotChangedTagsCheckBoxLabel = new System.Windows.Forms.Label();
            this.includeNotChangedTagsCheckBox = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.allowAsrLrPresetAutoExecutionCheckBoxLabel = new System.Windows.Forms.Label();
            this.allowAsrLrPresetAutoExecutionCheckBox = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonsPanel = new System.Windows.Forms.Panel();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.saveLastSkippedButton = new System.Windows.Forms.Button();
            this.versionLabel = new System.Windows.Forms.Label();

            //MusicBee
            this.customFontTextBox.Dispose();
            this.preservedTagValuesLegendTextBox.Dispose();
            this.preservedTagsLegendTextBox.Dispose();
            this.changedLegendTextBox.Dispose();

            this.customFontTextBox = ControlsTools.CreateMusicBeeTextBox();
            this.preservedTagValuesLegendTextBox = ControlsTools.CreateMusicBeeTextBox();
            this.preservedTagsLegendTextBox = ControlsTools.CreateMusicBeeTextBox();
            this.changedLegendTextBox = ControlsTools.CreateMusicBeeTextBox();
            //~MusicBee

            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.legendGroupBox.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.buttonsPanel.SuspendLayout();
            this.SuspendLayout();
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
            this.groupBox3.Tag = "#QuickSettings@pinned-to-parent-x";
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
            this.groupBox2.Controls.Add(this.scrollPreviewToEndCheckBoxLabel);
            this.groupBox2.Controls.Add(this.scrollPreviewToEndCheckBox);
            this.groupBox2.Controls.Add(this.legendGroupBox);
            this.groupBox2.Controls.Add(this.showHiddenCommandWindowsRadioButtonLabel);
            this.groupBox2.Controls.Add(this.showHiddenCommandWindowsRadioButton);
            this.groupBox2.Controls.Add(this.closeHiddenCommandWindowsRadioButtonLabel);
            this.groupBox2.Controls.Add(this.closeHiddenCommandWindowsRadioButton);
            this.groupBox2.Controls.Add(this.hidePluginWindowsOnMinimizationCheckBoxLabel);
            this.groupBox2.Controls.Add(this.hidePluginWindowsOnMinimizationCheckBox);
            this.groupBox2.Controls.Add(this.customFontButton);
            this.groupBox2.Controls.Add(this.customFontTextBox);
            this.groupBox2.Controls.Add(this.useCustomFontCheckBoxLabel);
            this.groupBox2.Controls.Add(this.useCustomFontCheckBox);
            this.groupBox2.Controls.Add(this.useMusicBeeFontCheckBoxLabel);
            this.groupBox2.Controls.Add(this.useMusicBeeFontCheckBox);
            this.groupBox2.Controls.Add(this.highlightChangedTagsCheckBoxLabel);
            this.groupBox2.Controls.Add(this.highlightChangedTagsCheckBox);
            this.groupBox2.Controls.Add(this.useSkinColorsCheckBoxLabel);
            this.groupBox2.Controls.Add(this.useSkinColorsCheckBox);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            this.groupBox2.Tag = "#QuickSettings@pinned-to-parent-x";
            // 
            // scrollPreviewToEndCheckBoxLabel
            // 
            resources.ApplyResources(this.scrollPreviewToEndCheckBoxLabel, "scrollPreviewToEndCheckBoxLabel");
            this.scrollPreviewToEndCheckBoxLabel.Name = "scrollPreviewToEndCheckBoxLabel";
            this.scrollPreviewToEndCheckBoxLabel.Click += new System.EventHandler(this.scrollPreviewToEndCheckBoxLabel_Click);
            // 
            // scrollPreviewToEndCheckBox
            // 
            resources.ApplyResources(this.scrollPreviewToEndCheckBox, "scrollPreviewToEndCheckBox");
            this.scrollPreviewToEndCheckBox.Name = "scrollPreviewToEndCheckBox";
            this.scrollPreviewToEndCheckBox.Tag = "#scrollPreviewToEndCheckBoxLabel";
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
            // hidePluginWindowsOnMinimizationCheckBoxLabel
            // 
            resources.ApplyResources(this.hidePluginWindowsOnMinimizationCheckBoxLabel, "hidePluginWindowsOnMinimizationCheckBoxLabel");
            this.hidePluginWindowsOnMinimizationCheckBoxLabel.Name = "hidePluginWindowsOnMinimizationCheckBoxLabel";
            this.hidePluginWindowsOnMinimizationCheckBoxLabel.Click += new System.EventHandler(this.hidePluginWindowsOnMinimizationCheckBoxLabel_Click);
            // 
            // hidePluginWindowsOnMinimizationCheckBox
            // 
            resources.ApplyResources(this.hidePluginWindowsOnMinimizationCheckBox, "hidePluginWindowsOnMinimizationCheckBox");
            this.hidePluginWindowsOnMinimizationCheckBox.Name = "hidePluginWindowsOnMinimizationCheckBox";
            this.hidePluginWindowsOnMinimizationCheckBox.Tag = "#hidePluginWindowsOnMinimizationCheckBoxLabel";
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
            // highlightChangedTagsCheckBoxLabel
            // 
            resources.ApplyResources(this.highlightChangedTagsCheckBoxLabel, "highlightChangedTagsCheckBoxLabel");
            this.highlightChangedTagsCheckBoxLabel.Name = "highlightChangedTagsCheckBoxLabel";
            this.highlightChangedTagsCheckBoxLabel.Click += new System.EventHandler(this.highlightChangedTagsCheckBoxLabel_Click);
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
            this.allowCommandExecutionWithoutPreviewCheckBox.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.allowCommandExecutionWithoutPreviewCheckBox.Name = "allowCommandExecutionWithoutPreviewCheckBox";
            this.allowCommandExecutionWithoutPreviewCheckBox.Tag = "#allowCommandExecutionWithoutPreviewCheckBoxLabel@pinned-to-parent-x";
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
            this.groupBox5.Tag = "#QuickSettings@pinned-to-parent-x";
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
            // allowAsrLrPresetAutoExecutionCheckBoxLabel
            // 
            resources.ApplyResources(this.allowAsrLrPresetAutoExecutionCheckBoxLabel, "allowAsrLrPresetAutoExecutionCheckBoxLabel");
            this.allowAsrLrPresetAutoExecutionCheckBoxLabel.Name = "allowAsrLrPresetAutoExecutionCheckBoxLabel";
            this.allowAsrLrPresetAutoExecutionCheckBoxLabel.Tag = "";
            this.allowAsrLrPresetAutoExecutionCheckBoxLabel.Click += new System.EventHandler(this.allowAsrLrPresetAutoExecutionCheckBoxLabel_Click);
            // 
            // allowAsrLrPresetAutoExecutionCheckBox
            // 
            resources.ApplyResources(this.allowAsrLrPresetAutoExecutionCheckBox, "allowAsrLrPresetAutoExecutionCheckBox");
            this.allowAsrLrPresetAutoExecutionCheckBox.Name = "allowAsrLrPresetAutoExecutionCheckBox";
            this.allowAsrLrPresetAutoExecutionCheckBox.Tag = "#allowAsrLrPresetAutoExecutionCheckBoxLabel@pinned-to-parent-x";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label5.Name = "label5";
            this.label5.Tag = "@pinned-to-parent-x";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Name = "label1";
            this.label1.Tag = "@pinned-to-parent-x";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            this.label6.Tag = "@pinned-to-parent-x";
            // 
            // buttonsPanel
            // 
            resources.ApplyResources(this.buttonsPanel, "buttonsPanel");
            this.buttonsPanel.Controls.Add(this.buttonClose);
            this.buttonsPanel.Controls.Add(this.buttonOK);
            this.buttonsPanel.Controls.Add(this.saveLastSkippedButton);
            this.buttonsPanel.Controls.Add(this.versionLabel);
            this.buttonsPanel.Name = "buttonsPanel";
            this.buttonsPanel.Tag = "#QuickSettings&QuickSettings@pinned-to-parent-x";
            // 
            // buttonClose
            // 
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Tag = "@pinned-to-parent-x@pinned-to-parent-y@non-defaultable";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Tag = "#buttonClose@pinned-to-parent-y";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // saveLastSkippedButton
            // 
            resources.ApplyResources(this.saveLastSkippedButton, "saveLastSkippedButton");
            this.saveLastSkippedButton.Name = "saveLastSkippedButton";
            this.saveLastSkippedButton.Tag = "#buttonOK@pinned-to-parent-y";
            this.saveLastSkippedButton.Click += new System.EventHandler(this.saveLastSkippedButton_Click);
            // 
            // versionLabel
            // 
            resources.ApplyResources(this.versionLabel, "versionLabel");
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Tag = "@pinned-to-parent-x@pinned-to-parent-y";
            // 
            // QuickSettings
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.Controls.Add(this.buttonsPanel);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.allowCommandExecutionWithoutPreviewCheckBoxLabel);
            this.Controls.Add(this.allowCommandExecutionWithoutPreviewCheckBox);
            this.Controls.Add(this.allowAsrLrPresetAutoExecutionCheckBoxLabel);
            this.Controls.Add(this.allowAsrLrPresetAutoExecutionCheckBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label6);
            this.DoubleBuffered = true;
            this.Name = "QuickSettings";
            this.Tag = "@min-max-width-same@min-max-height-same";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.legendGroupBox.ResumeLayout(false);
            this.legendGroupBox.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.buttonsPanel.ResumeLayout(false);
            this.buttonsPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox playStoppedSoundCheckBox;
        private System.Windows.Forms.CheckBox playStartedSoundCheckBox;
        private System.Windows.Forms.CheckBox playCompletedSoundCheckBox;
        private System.Windows.Forms.CheckBox playTickedAsrPresetSoundCheckBox;
        private System.Windows.Forms.CheckBox useSkinColorsCheckBox;
        private System.Windows.Forms.RadioButton closeHiddenCommandWindowsRadioButton;
        private System.Windows.Forms.RadioButton showHiddenCommandWindowsRadioButton;
        private System.Windows.Forms.CheckBox highlightChangedTagsCheckBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox includeNotChangedTagsCheckBox;
        private System.Windows.Forms.CheckBox includePreservedTagsCheckBox;
        private System.Windows.Forms.CheckBox includePreservedTagValuesCheckBox;
        private System.Windows.Forms.GroupBox legendGroupBox;
        private System.Windows.Forms.TextBox preservedTagValuesLegendTextBox;
        private System.Windows.Forms.TextBox preservedTagsLegendTextBox;
        private System.Windows.Forms.TextBox changedLegendTextBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox hidePluginWindowsOnMinimizationCheckBox;
        private System.Windows.Forms.Label hidePluginWindowsOnMinimizationCheckBoxLabel;
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
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label allowAsrLrPresetAutoExecutionCheckBoxLabel;
        private System.Windows.Forms.CheckBox allowAsrLrPresetAutoExecutionCheckBox;
        private System.Windows.Forms.Label useMusicBeeFontCheckBoxLabel;
        private System.Windows.Forms.CheckBox useMusicBeeFontCheckBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button customFontButton;
        private System.Windows.Forms.TextBox customFontTextBox;
        private System.Windows.Forms.Label useCustomFontCheckBoxLabel;
        private System.Windows.Forms.CheckBox useCustomFontCheckBox;
        private System.Windows.Forms.Panel buttonsPanel;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button saveLastSkippedButton;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label scrollPreviewToEndCheckBoxLabel;
        private System.Windows.Forms.CheckBox scrollPreviewToEndCheckBox;
    }
}