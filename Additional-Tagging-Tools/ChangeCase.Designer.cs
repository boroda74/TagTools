namespace MusicBeePlugin
{
    partial class ChangeCaseCommand
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangeCaseCommand));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.mainLabel = new System.Windows.Forms.Label();
            this.sourceTagList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.sentenceCaseRadioButton = new System.Windows.Forms.RadioButton();
            this.lowerCaseRadioButton = new System.Windows.Forms.RadioButton();
            this.upperCaseRadioButton = new System.Windows.Forms.RadioButton();
            this.titleCaseRadioButton = new System.Windows.Forms.RadioButton();
            this.toggleCaseRadioButton = new System.Windows.Forms.RadioButton();
            this.exceptionCharsBox = new System.Windows.Forms.TextBox();
            this.wordSplittersBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.previewTable = new System.Windows.Forms.DataGridView();
            this.File = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Track = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalTag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalTagT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewTag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewTagT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonReapply = new System.Windows.Forms.Button();
            this.exceptionWordsCheckBox = new System.Windows.Forms.CheckBox();
            this.exceptionCharsCheckBox = new System.Windows.Forms.CheckBox();
            this.wordSplittersCheckBox = new System.Windows.Forms.CheckBox();
            this.onlyWordsCheckBox = new System.Windows.Forms.CheckBox();
            this.dirtyErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.buttonSettings = new System.Windows.Forms.Button();
            this.exceptionWordsBox = new System.Windows.Forms.ComboBox();
            this.alwaysCapitalize1stWordCheckBox = new System.Windows.Forms.CheckBox();
            this.alwaysCapitalizeLastWordCheckBox = new System.Windows.Forms.CheckBox();
            this.removeExceptionButton = new System.Windows.Forms.Button();
            this.buttonASRExceptedWords = new System.Windows.Forms.Button();
            this.buttonASRExceptWordsAfterSymbols = new System.Windows.Forms.Button();
            this.buttonASRWordSplitters = new System.Windows.Forms.Button();
            this.sentenceCaseRadioButtonLabel = new System.Windows.Forms.Label();
            this.lowerCaseRadioButtonLabel = new System.Windows.Forms.Label();
            this.upperCaseRadioButtonLabel = new System.Windows.Forms.Label();
            this.titleCaseRadioButtonLabel = new System.Windows.Forms.Label();
            this.toggleCaseRadioButtonLabel = new System.Windows.Forms.Label();
            this.exceptionWordsCheckBoxLabel = new System.Windows.Forms.Label();
            this.onlyWordsCheckBoxLabel = new System.Windows.Forms.Label();
            this.exceptionCharsCheckBoxLabel = new System.Windows.Forms.Label();
            this.wordSplittersCheckBoxLabel = new System.Windows.Forms.Label();
            this.alwaysCapitalize1stWordCheckBoxLabel = new System.Windows.Forms.Label();
            this.alwaysCapitalizeLastWordCheckBoxLabel = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).BeginInit();
            this.SuspendLayout();
            //MusicBee
            this.exceptionWordsBox = (System.Windows.Forms.ComboBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.ComboBox);
            this.exceptionCharsBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.wordSplittersBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
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
            // mainLabel
            // 
            resources.ApplyResources(this.mainLabel, "mainLabel");
            this.mainLabel.Name = "mainLabel";
            // 
            // sourceTagList
            // 
            this.sourceTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceTagList.DropDownWidth = 250;
            this.sourceTagList.FormattingEnabled = true;
            resources.ApplyResources(this.sourceTagList, "sourceTagList");
            this.sourceTagList.Name = "sourceTagList";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // sentenceCaseRadioButton
            // 
            resources.ApplyResources(this.sentenceCaseRadioButton, "sentenceCaseRadioButton");
            this.sentenceCaseRadioButton.Name = "sentenceCaseRadioButton";
            this.sentenceCaseRadioButton.TabStop = true;
            this.sentenceCaseRadioButton.Tag = "sentenceCaseRadioButtonLabel";
            this.sentenceCaseRadioButton.UseVisualStyleBackColor = true;
            this.sentenceCaseRadioButton.CheckedChanged += new System.EventHandler(this.casingRuleRadioButton_CheckedChanged);
            // 
            // lowerCaseRadioButton
            // 
            resources.ApplyResources(this.lowerCaseRadioButton, "lowerCaseRadioButton");
            this.lowerCaseRadioButton.Name = "lowerCaseRadioButton";
            this.lowerCaseRadioButton.Tag = "lowerCaseRadioButtonLabel";
            this.lowerCaseRadioButton.UseVisualStyleBackColor = true;
            this.lowerCaseRadioButton.CheckedChanged += new System.EventHandler(this.casingRuleRadioButton_CheckedChanged);
            // 
            // upperCaseRadioButton
            // 
            resources.ApplyResources(this.upperCaseRadioButton, "upperCaseRadioButton");
            this.upperCaseRadioButton.Name = "upperCaseRadioButton";
            this.upperCaseRadioButton.Tag = "upperCaseRadioButtonLabel";
            this.upperCaseRadioButton.UseVisualStyleBackColor = true;
            this.upperCaseRadioButton.CheckedChanged += new System.EventHandler(this.casingRuleRadioButton_CheckedChanged);
            // 
            // titleCaseRadioButton
            // 
            resources.ApplyResources(this.titleCaseRadioButton, "titleCaseRadioButton");
            this.titleCaseRadioButton.Name = "titleCaseRadioButton";
            this.titleCaseRadioButton.Tag = "titleCaseRadioButtonLabel";
            this.titleCaseRadioButton.UseVisualStyleBackColor = true;
            this.titleCaseRadioButton.CheckedChanged += new System.EventHandler(this.casingRuleRadioButton_CheckedChanged);
            // 
            // toggleCaseRadioButton
            // 
            resources.ApplyResources(this.toggleCaseRadioButton, "toggleCaseRadioButton");
            this.toggleCaseRadioButton.Name = "toggleCaseRadioButton";
            this.toggleCaseRadioButton.Tag = "toggleCaseRadioButtonLabel";
            this.toggleCaseRadioButton.UseVisualStyleBackColor = true;
            this.toggleCaseRadioButton.CheckedChanged += new System.EventHandler(this.casingRuleRadioButton_CheckedChanged);
            // 
            // exceptionCharsBox
            // 
            resources.ApplyResources(this.exceptionCharsBox, "exceptionCharsBox");
            this.exceptionCharsBox.Name = "exceptionCharsBox";
            // 
            // wordSplittersBox
            // 
            resources.ApplyResources(this.wordSplittersBox, "wordSplittersBox");
            this.wordSplittersBox.Name = "wordSplittersBox";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // buttonPreview
            // 
            resources.ApplyResources(this.buttonPreview, "buttonPreview");
            this.dirtyErrorProvider.SetError(this.buttonPreview, resources.GetString("buttonPreview.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonPreview, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonPreview.IconAlignment"))));
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.UseVisualStyleBackColor = true;
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // previewTable
            // 
            this.previewTable.AllowUserToAddRows = false;
            this.previewTable.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.previewTable, "previewTable");
            this.previewTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.previewTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.previewTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.previewTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.File,
            this.Track,
            this.OriginalTag,
            this.OriginalTagT,
            this.NewTag,
            this.NewTagT});
            this.previewTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.previewTable.MultiSelect = false;
            this.previewTable.Name = "previewTable";
            this.previewTable.RowHeadersVisible = false;
            this.previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.previewTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.previewTable_CellContentClick);
            // 
            // File
            // 
            this.File.FillWeight = 1F;
            resources.ApplyResources(this.File, "File");
            this.File.Name = "File";
            // 
            // Track
            // 
            this.Track.FillWeight = 75F;
            resources.ApplyResources(this.Track, "Track");
            this.Track.Name = "Track";
            // 
            // OriginalTag
            // 
            this.OriginalTag.FillWeight = 1F;
            resources.ApplyResources(this.OriginalTag, "OriginalTag");
            this.OriginalTag.Name = "OriginalTag";
            // 
            // OriginalTagT
            // 
            this.OriginalTagT.FillWeight = 25F;
            resources.ApplyResources(this.OriginalTagT, "OriginalTagT");
            this.OriginalTagT.Name = "OriginalTagT";
            // 
            // NewTag
            // 
            this.NewTag.FillWeight = 1F;
            resources.ApplyResources(this.NewTag, "NewTag");
            this.NewTag.Name = "NewTag";
            // 
            // NewTagT
            // 
            this.NewTagT.FillWeight = 25F;
            resources.ApplyResources(this.NewTagT, "NewTagT");
            this.NewTagT.Name = "NewTagT";
            // 
            // buttonReapply
            // 
            resources.ApplyResources(this.buttonReapply, "buttonReapply");
            this.buttonReapply.Name = "buttonReapply";
            this.toolTip1.SetToolTip(this.buttonReapply, resources.GetString("buttonReapply.ToolTip"));
            this.buttonReapply.UseVisualStyleBackColor = true;
            this.buttonReapply.Click += new System.EventHandler(this.buttonReapply_Click);
            // 
            // exceptionWordsCheckBox
            // 
            resources.ApplyResources(this.exceptionWordsCheckBox, "exceptionWordsCheckBox");
            this.exceptionWordsCheckBox.Name = "exceptionWordsCheckBox";
            this.exceptionWordsCheckBox.Tag = "exceptionWordsCheckBoxLabel";
            this.toolTip1.SetToolTip(this.exceptionWordsCheckBox, resources.GetString("exceptionWordsCheckBox.ToolTip"));
            this.exceptionWordsCheckBox.UseVisualStyleBackColor = true;
            this.exceptionWordsCheckBox.CheckedChanged += new System.EventHandler(this.exceptWordsCheckBox_CheckedChanged);
            // 
            // exceptionCharsCheckBox
            // 
            resources.ApplyResources(this.exceptionCharsCheckBox, "exceptionCharsCheckBox");
            this.exceptionCharsCheckBox.Name = "exceptionCharsCheckBox";
            this.exceptionCharsCheckBox.Tag = "exceptionCharsCheckBoxLabel";
            this.exceptionCharsCheckBox.UseVisualStyleBackColor = true;
            this.exceptionCharsCheckBox.CheckedChanged += new System.EventHandler(this.exceptCharsCheckBox_CheckedChanged);
            // 
            // wordSplittersCheckBox
            // 
            resources.ApplyResources(this.wordSplittersCheckBox, "wordSplittersCheckBox");
            this.wordSplittersCheckBox.Name = "wordSplittersCheckBox";
            this.wordSplittersCheckBox.Tag = "wordSplittersCheckBoxLabel";
            this.wordSplittersCheckBox.UseVisualStyleBackColor = true;
            this.wordSplittersCheckBox.CheckedChanged += new System.EventHandler(this.wordSplittersCheckBox_CheckedChanged);
            // 
            // onlyWordsCheckBox
            // 
            resources.ApplyResources(this.onlyWordsCheckBox, "onlyWordsCheckBox");
            this.onlyWordsCheckBox.Name = "onlyWordsCheckBox";
            this.onlyWordsCheckBox.Tag = "onlyWordsCheckBoxLabel";
            this.onlyWordsCheckBox.UseVisualStyleBackColor = true;
            this.onlyWordsCheckBox.CheckedChanged += new System.EventHandler(this.onlyWordsCheckBox_CheckedChanged);
            // 
            // dirtyErrorProvider
            // 
            this.dirtyErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.dirtyErrorProvider.ContainerControl = this;
            resources.ApplyResources(this.dirtyErrorProvider, "dirtyErrorProvider");
            // 
            // buttonSettings
            // 
            resources.ApplyResources(this.buttonSettings, "buttonSettings");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonSettings, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonSettings.IconAlignment"))));
            this.buttonSettings.Image = global::MusicBeePlugin.Properties.Resources.gear_15;
            this.buttonSettings.Name = "buttonSettings";
            this.toolTip1.SetToolTip(this.buttonSettings, resources.GetString("buttonSettings.ToolTip"));
            this.buttonSettings.UseVisualStyleBackColor = true;
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // exceptionWordsBox
            // 
            resources.ApplyResources(this.exceptionWordsBox, "exceptionWordsBox");
            this.exceptionWordsBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.exceptionWordsBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.exceptionWordsBox.FormattingEnabled = true;
            this.exceptionWordsBox.Name = "exceptionWordsBox";
            this.exceptionWordsBox.Leave += new System.EventHandler(this.exceptionWordsBox_Leave);
            // 
            // alwaysCapitalize1stWordCheckBox
            // 
            resources.ApplyResources(this.alwaysCapitalize1stWordCheckBox, "alwaysCapitalize1stWordCheckBox");
            this.alwaysCapitalize1stWordCheckBox.Name = "alwaysCapitalize1stWordCheckBox";
            this.alwaysCapitalize1stWordCheckBox.Tag = "alwaysCapitalize1stWordCheckBoxLabel";
            this.alwaysCapitalize1stWordCheckBox.UseVisualStyleBackColor = true;
            // 
            // alwaysCapitalizeLastWordCheckBox
            // 
            resources.ApplyResources(this.alwaysCapitalizeLastWordCheckBox, "alwaysCapitalizeLastWordCheckBox");
            this.alwaysCapitalizeLastWordCheckBox.Name = "alwaysCapitalizeLastWordCheckBox";
            this.alwaysCapitalizeLastWordCheckBox.Tag = "alwaysCapitalizeLastWordCheckBoxLabel";
            this.alwaysCapitalizeLastWordCheckBox.UseVisualStyleBackColor = true;
            // 
            // removeExceptionButton
            // 
            resources.ApplyResources(this.removeExceptionButton, "removeExceptionButton");
            this.removeExceptionButton.Image = global::MusicBeePlugin.Properties.Resources.clear_button_15;
            this.removeExceptionButton.Name = "removeExceptionButton";
            this.toolTip1.SetToolTip(this.removeExceptionButton, resources.GetString("removeExceptionButton.ToolTip"));
            this.removeExceptionButton.UseVisualStyleBackColor = true;
            this.removeExceptionButton.Click += new System.EventHandler(this.removeExceptionButton_Click);
            // 
            // buttonASRExceptedWords
            // 
            resources.ApplyResources(this.buttonASRExceptedWords, "buttonASRExceptedWords");
            this.buttonASRExceptedWords.Name = "buttonASRExceptedWords";
            this.toolTip1.SetToolTip(this.buttonASRExceptedWords, resources.GetString("buttonASRExceptedWords.ToolTip"));
            this.buttonASRExceptedWords.UseVisualStyleBackColor = true;
            this.buttonASRExceptedWords.Click += new System.EventHandler(this.buttonASR_Click);
            // 
            // buttonASRExceptWordsAfterSymbols
            // 
            resources.ApplyResources(this.buttonASRExceptWordsAfterSymbols, "buttonASRExceptWordsAfterSymbols");
            this.buttonASRExceptWordsAfterSymbols.Name = "buttonASRExceptWordsAfterSymbols";
            this.toolTip1.SetToolTip(this.buttonASRExceptWordsAfterSymbols, resources.GetString("buttonASRExceptWordsAfterSymbols.ToolTip"));
            this.buttonASRExceptWordsAfterSymbols.UseVisualStyleBackColor = true;
            this.buttonASRExceptWordsAfterSymbols.Click += new System.EventHandler(this.buttonASRExceptWordsAfterSymbols_Click);
            // 
            // buttonASRWordSplitters
            // 
            resources.ApplyResources(this.buttonASRWordSplitters, "buttonASRWordSplitters");
            this.buttonASRWordSplitters.Name = "buttonASRWordSplitters";
            this.toolTip1.SetToolTip(this.buttonASRWordSplitters, resources.GetString("buttonASRWordSplitters.ToolTip"));
            this.buttonASRWordSplitters.UseVisualStyleBackColor = true;
            this.buttonASRWordSplitters.Click += new System.EventHandler(this.buttonASRWordSplitters_Click);
            // 
            // sentenceCaseRadioButtonLabel
            // 
            resources.ApplyResources(this.sentenceCaseRadioButtonLabel, "sentenceCaseRadioButtonLabel");
            this.sentenceCaseRadioButtonLabel.Name = "sentenceCaseRadioButtonLabel";
            this.sentenceCaseRadioButtonLabel.Click += new System.EventHandler(this.sentenceCaseRadioButtonLabel_Click);
            // 
            // lowerCaseRadioButtonLabel
            // 
            resources.ApplyResources(this.lowerCaseRadioButtonLabel, "lowerCaseRadioButtonLabel");
            this.lowerCaseRadioButtonLabel.Name = "lowerCaseRadioButtonLabel";
            this.lowerCaseRadioButtonLabel.Click += new System.EventHandler(this.lowerCaseRadioButtonLabel_Click);
            // 
            // upperCaseRadioButtonLabel
            // 
            resources.ApplyResources(this.upperCaseRadioButtonLabel, "upperCaseRadioButtonLabel");
            this.upperCaseRadioButtonLabel.Name = "upperCaseRadioButtonLabel";
            this.upperCaseRadioButtonLabel.Click += new System.EventHandler(this.upperCaseRadioButtonLabel_Click);
            // 
            // titleCaseRadioButtonLabel
            // 
            resources.ApplyResources(this.titleCaseRadioButtonLabel, "titleCaseRadioButtonLabel");
            this.titleCaseRadioButtonLabel.Name = "titleCaseRadioButtonLabel";
            this.titleCaseRadioButtonLabel.Click += new System.EventHandler(this.titleCaseRadioButtonLabel_Click);
            // 
            // toggleCaseRadioButtonLabel
            // 
            resources.ApplyResources(this.toggleCaseRadioButtonLabel, "toggleCaseRadioButtonLabel");
            this.toggleCaseRadioButtonLabel.Name = "toggleCaseRadioButtonLabel";
            this.toggleCaseRadioButtonLabel.Click += new System.EventHandler(this.toggleCaseRadioButtonLabel_Click);
            // 
            // exceptionWordsCheckBoxLabel
            // 
            resources.ApplyResources(this.exceptionWordsCheckBoxLabel, "exceptionWordsCheckBoxLabel");
            this.exceptionWordsCheckBoxLabel.Name = "exceptionWordsCheckBoxLabel";
            this.toolTip1.SetToolTip(this.exceptionWordsCheckBoxLabel, resources.GetString("exceptionWordsCheckBoxLabel.ToolTip"));
            this.exceptionWordsCheckBoxLabel.Click += new System.EventHandler(this.exceptionWordsCheckBoxLabel_Click);
            // 
            // onlyWordsCheckBoxLabel
            // 
            resources.ApplyResources(this.onlyWordsCheckBoxLabel, "onlyWordsCheckBoxLabel");
            this.onlyWordsCheckBoxLabel.Name = "onlyWordsCheckBoxLabel";
            this.onlyWordsCheckBoxLabel.Click += new System.EventHandler(this.onlyWordsCheckBoxLabel_Click);
            // 
            // exceptionCharsCheckBoxLabel
            // 
            resources.ApplyResources(this.exceptionCharsCheckBoxLabel, "exceptionCharsCheckBoxLabel");
            this.exceptionCharsCheckBoxLabel.Name = "exceptionCharsCheckBoxLabel";
            this.exceptionCharsCheckBoxLabel.Click += new System.EventHandler(this.exceptionCharsCheckBoxLabel_Click);
            // 
            // wordSplittersCheckBoxLabel
            // 
            resources.ApplyResources(this.wordSplittersCheckBoxLabel, "wordSplittersCheckBoxLabel");
            this.wordSplittersCheckBoxLabel.Name = "wordSplittersCheckBoxLabel";
            this.wordSplittersCheckBoxLabel.Click += new System.EventHandler(this.wordSplittersCheckBoxLabel_Click);
            // 
            // alwaysCapitalize1stWordCheckBoxLabel
            // 
            resources.ApplyResources(this.alwaysCapitalize1stWordCheckBoxLabel, "alwaysCapitalize1stWordCheckBoxLabel");
            this.alwaysCapitalize1stWordCheckBoxLabel.Name = "alwaysCapitalize1stWordCheckBoxLabel";
            this.alwaysCapitalize1stWordCheckBoxLabel.Tag = "";
            this.alwaysCapitalize1stWordCheckBoxLabel.Click += new System.EventHandler(this.alwaysCapitalize1stWordCheckBoxLabel_Click);
            // 
            // alwaysCapitalizeLastWordCheckBoxLabel
            // 
            resources.ApplyResources(this.alwaysCapitalizeLastWordCheckBoxLabel, "alwaysCapitalizeLastWordCheckBoxLabel");
            this.alwaysCapitalizeLastWordCheckBoxLabel.Name = "alwaysCapitalizeLastWordCheckBoxLabel";
            this.alwaysCapitalizeLastWordCheckBoxLabel.Click += new System.EventHandler(this.alwaysCapitalizeLastWordCheckBoxLabel_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 1500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.FillWeight = 1F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 75F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.FillWeight = 1F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.FillWeight = 25F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.FillWeight = 1F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewTextBoxColumn6.FillWeight = 25F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // ChangeCaseCommand
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.onlyWordsCheckBoxLabel);
            this.Controls.Add(this.alwaysCapitalizeLastWordCheckBoxLabel);
            this.Controls.Add(this.alwaysCapitalize1stWordCheckBoxLabel);
            this.Controls.Add(this.wordSplittersCheckBoxLabel);
            this.Controls.Add(this.exceptionCharsCheckBoxLabel);
            this.Controls.Add(this.exceptionWordsCheckBoxLabel);
            this.Controls.Add(this.toggleCaseRadioButtonLabel);
            this.Controls.Add(this.titleCaseRadioButtonLabel);
            this.Controls.Add(this.upperCaseRadioButtonLabel);
            this.Controls.Add(this.lowerCaseRadioButtonLabel);
            this.Controls.Add(this.sentenceCaseRadioButtonLabel);
            this.Controls.Add(this.buttonSettings);
            this.Controls.Add(this.buttonASRWordSplitters);
            this.Controls.Add(this.buttonASRExceptWordsAfterSymbols);
            this.Controls.Add(this.buttonASRExceptedWords);
            this.Controls.Add(this.removeExceptionButton);
            this.Controls.Add(this.alwaysCapitalizeLastWordCheckBox);
            this.Controls.Add(this.alwaysCapitalize1stWordCheckBox);
            this.Controls.Add(this.exceptionWordsBox);
            this.Controls.Add(this.onlyWordsCheckBox);
            this.Controls.Add(this.wordSplittersCheckBox);
            this.Controls.Add(this.exceptionCharsCheckBox);
            this.Controls.Add(this.exceptionWordsCheckBox);
            this.Controls.Add(this.buttonReapply);
            this.Controls.Add(this.previewTable);
            this.Controls.Add(this.buttonPreview);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.wordSplittersBox);
            this.Controls.Add(this.exceptionCharsBox);
            this.Controls.Add(this.toggleCaseRadioButton);
            this.Controls.Add(this.titleCaseRadioButton);
            this.Controls.Add(this.upperCaseRadioButton);
            this.Controls.Add(this.lowerCaseRadioButton);
            this.Controls.Add(this.sentenceCaseRadioButton);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.mainLabel);
            this.Controls.Add(this.sourceTagList);
            this.Controls.Add(this.label1);
            this.Name = "ChangeCaseCommand";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label mainLabel;
        private System.Windows.Forms.ComboBox sourceTagList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton sentenceCaseRadioButton;
        private System.Windows.Forms.RadioButton lowerCaseRadioButton;
        private System.Windows.Forms.RadioButton upperCaseRadioButton;
        private System.Windows.Forms.RadioButton titleCaseRadioButton;
        private System.Windows.Forms.RadioButton toggleCaseRadioButton;
        private System.Windows.Forms.TextBox exceptionCharsBox;
        private System.Windows.Forms.TextBox wordSplittersBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.DataGridView previewTable;
        private System.Windows.Forms.Button buttonReapply;
        private System.Windows.Forms.CheckBox exceptionWordsCheckBox;
        private System.Windows.Forms.CheckBox exceptionCharsCheckBox;
        private System.Windows.Forms.CheckBox wordSplittersCheckBox;
        private System.Windows.Forms.CheckBox onlyWordsCheckBox;
        private System.Windows.Forms.ErrorProvider dirtyErrorProvider;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox exceptionWordsBox;
        private System.Windows.Forms.CheckBox alwaysCapitalizeLastWordCheckBox;
        private System.Windows.Forms.CheckBox alwaysCapitalize1stWordCheckBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.Button removeExceptionButton;
        private System.Windows.Forms.Button buttonASRExceptedWords;
        private System.Windows.Forms.Button buttonASRExceptWordsAfterSymbols;
        private System.Windows.Forms.Button buttonASRWordSplitters;
        private System.Windows.Forms.DataGridViewTextBoxColumn File;
        private System.Windows.Forms.DataGridViewTextBoxColumn Track;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTag;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTagT;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTag;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTagT;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.Label upperCaseRadioButtonLabel;
        private System.Windows.Forms.Label lowerCaseRadioButtonLabel;
        private System.Windows.Forms.Label sentenceCaseRadioButtonLabel;
        private System.Windows.Forms.Label toggleCaseRadioButtonLabel;
        private System.Windows.Forms.Label titleCaseRadioButtonLabel;
        private System.Windows.Forms.Label exceptionWordsCheckBoxLabel;
        private System.Windows.Forms.Label onlyWordsCheckBoxLabel;
        private System.Windows.Forms.Label exceptionCharsCheckBoxLabel;
        private System.Windows.Forms.Label wordSplittersCheckBoxLabel;
        private System.Windows.Forms.Label alwaysCapitalize1stWordCheckBoxLabel;
        private System.Windows.Forms.Label alwaysCapitalizeLastWordCheckBoxLabel;
    }
}