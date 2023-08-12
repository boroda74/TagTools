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
            this.dirtyErrorProvider.SetError(this.buttonCancel, resources.GetString("buttonCancel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonCancel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonCancel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonCancel, ((int)(resources.GetObject("buttonCancel.IconPadding"))));
            this.buttonCancel.Name = "buttonCancel";
            this.toolTip1.SetToolTip(this.buttonCancel, resources.GetString("buttonCancel.ToolTip"));
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.dirtyErrorProvider.SetError(this.buttonOK, resources.GetString("buttonOK.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonOK, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonOK.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonOK, ((int)(resources.GetObject("buttonOK.IconPadding"))));
            this.buttonOK.Name = "buttonOK";
            this.toolTip1.SetToolTip(this.buttonOK, resources.GetString("buttonOK.ToolTip"));
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // mainLabel
            // 
            resources.ApplyResources(this.mainLabel, "mainLabel");
            this.dirtyErrorProvider.SetError(this.mainLabel, resources.GetString("mainLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.mainLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("mainLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.mainLabel, ((int)(resources.GetObject("mainLabel.IconPadding"))));
            this.mainLabel.Name = "mainLabel";
            this.toolTip1.SetToolTip(this.mainLabel, resources.GetString("mainLabel.ToolTip"));
            // 
            // sourceTagList
            // 
            resources.ApplyResources(this.sourceTagList, "sourceTagList");
            this.sourceTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceTagList.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.sourceTagList, resources.GetString("sourceTagList.Error"));
            this.sourceTagList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.sourceTagList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("sourceTagList.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.sourceTagList, ((int)(resources.GetObject("sourceTagList.IconPadding"))));
            this.sourceTagList.Name = "sourceTagList";
            this.toolTip1.SetToolTip(this.sourceTagList, resources.GetString("sourceTagList.ToolTip"));
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.dirtyErrorProvider.SetError(this.label1, resources.GetString("label1.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            this.toolTip1.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // sentenceCaseRadioButton
            // 
            resources.ApplyResources(this.sentenceCaseRadioButton, "sentenceCaseRadioButton");
            this.dirtyErrorProvider.SetError(this.sentenceCaseRadioButton, resources.GetString("sentenceCaseRadioButton.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.sentenceCaseRadioButton, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("sentenceCaseRadioButton.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.sentenceCaseRadioButton, ((int)(resources.GetObject("sentenceCaseRadioButton.IconPadding"))));
            this.sentenceCaseRadioButton.Name = "sentenceCaseRadioButton";
            this.sentenceCaseRadioButton.TabStop = true;
            this.toolTip1.SetToolTip(this.sentenceCaseRadioButton, resources.GetString("sentenceCaseRadioButton.ToolTip"));
            this.sentenceCaseRadioButton.UseVisualStyleBackColor = true;
            this.sentenceCaseRadioButton.CheckedChanged += new System.EventHandler(this.casingRuleRadioButton_CheckedChanged);
            // 
            // lowerCaseRadioButton
            // 
            resources.ApplyResources(this.lowerCaseRadioButton, "lowerCaseRadioButton");
            this.dirtyErrorProvider.SetError(this.lowerCaseRadioButton, resources.GetString("lowerCaseRadioButton.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.lowerCaseRadioButton, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lowerCaseRadioButton.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.lowerCaseRadioButton, ((int)(resources.GetObject("lowerCaseRadioButton.IconPadding"))));
            this.lowerCaseRadioButton.Name = "lowerCaseRadioButton";
            this.toolTip1.SetToolTip(this.lowerCaseRadioButton, resources.GetString("lowerCaseRadioButton.ToolTip"));
            this.lowerCaseRadioButton.UseVisualStyleBackColor = true;
            this.lowerCaseRadioButton.CheckedChanged += new System.EventHandler(this.casingRuleRadioButton_CheckedChanged);
            // 
            // upperCaseRadioButton
            // 
            resources.ApplyResources(this.upperCaseRadioButton, "upperCaseRadioButton");
            this.dirtyErrorProvider.SetError(this.upperCaseRadioButton, resources.GetString("upperCaseRadioButton.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.upperCaseRadioButton, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("upperCaseRadioButton.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.upperCaseRadioButton, ((int)(resources.GetObject("upperCaseRadioButton.IconPadding"))));
            this.upperCaseRadioButton.Name = "upperCaseRadioButton";
            this.toolTip1.SetToolTip(this.upperCaseRadioButton, resources.GetString("upperCaseRadioButton.ToolTip"));
            this.upperCaseRadioButton.UseVisualStyleBackColor = true;
            this.upperCaseRadioButton.CheckedChanged += new System.EventHandler(this.casingRuleRadioButton_CheckedChanged);
            // 
            // titleCaseRadioButton
            // 
            resources.ApplyResources(this.titleCaseRadioButton, "titleCaseRadioButton");
            this.dirtyErrorProvider.SetError(this.titleCaseRadioButton, resources.GetString("titleCaseRadioButton.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.titleCaseRadioButton, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("titleCaseRadioButton.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.titleCaseRadioButton, ((int)(resources.GetObject("titleCaseRadioButton.IconPadding"))));
            this.titleCaseRadioButton.Name = "titleCaseRadioButton";
            this.toolTip1.SetToolTip(this.titleCaseRadioButton, resources.GetString("titleCaseRadioButton.ToolTip"));
            this.titleCaseRadioButton.UseVisualStyleBackColor = true;
            this.titleCaseRadioButton.CheckedChanged += new System.EventHandler(this.casingRuleRadioButton_CheckedChanged);
            // 
            // toggleCaseRadioButton
            // 
            resources.ApplyResources(this.toggleCaseRadioButton, "toggleCaseRadioButton");
            this.dirtyErrorProvider.SetError(this.toggleCaseRadioButton, resources.GetString("toggleCaseRadioButton.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.toggleCaseRadioButton, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("toggleCaseRadioButton.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.toggleCaseRadioButton, ((int)(resources.GetObject("toggleCaseRadioButton.IconPadding"))));
            this.toggleCaseRadioButton.Name = "toggleCaseRadioButton";
            this.toolTip1.SetToolTip(this.toggleCaseRadioButton, resources.GetString("toggleCaseRadioButton.ToolTip"));
            this.toggleCaseRadioButton.UseVisualStyleBackColor = true;
            this.toggleCaseRadioButton.CheckedChanged += new System.EventHandler(this.casingRuleRadioButton_CheckedChanged);
            // 
            // exceptionCharsBox
            // 
            resources.ApplyResources(this.exceptionCharsBox, "exceptionCharsBox");
            this.dirtyErrorProvider.SetError(this.exceptionCharsBox, resources.GetString("exceptionCharsBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.exceptionCharsBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("exceptionCharsBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.exceptionCharsBox, ((int)(resources.GetObject("exceptionCharsBox.IconPadding"))));
            this.exceptionCharsBox.Name = "exceptionCharsBox";
            this.toolTip1.SetToolTip(this.exceptionCharsBox, resources.GetString("exceptionCharsBox.ToolTip"));
            // 
            // wordSplittersBox
            // 
            resources.ApplyResources(this.wordSplittersBox, "wordSplittersBox");
            this.dirtyErrorProvider.SetError(this.wordSplittersBox, resources.GetString("wordSplittersBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.wordSplittersBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("wordSplittersBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.wordSplittersBox, ((int)(resources.GetObject("wordSplittersBox.IconPadding"))));
            this.wordSplittersBox.Name = "wordSplittersBox";
            this.toolTip1.SetToolTip(this.wordSplittersBox, resources.GetString("wordSplittersBox.ToolTip"));
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.dirtyErrorProvider.SetError(this.label6, resources.GetString("label6.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label6.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label6, ((int)(resources.GetObject("label6.IconPadding"))));
            this.label6.Name = "label6";
            this.toolTip1.SetToolTip(this.label6, resources.GetString("label6.ToolTip"));
            // 
            // buttonPreview
            // 
            resources.ApplyResources(this.buttonPreview, "buttonPreview");
            this.dirtyErrorProvider.SetError(this.buttonPreview, resources.GetString("buttonPreview.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonPreview, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonPreview.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonPreview, ((int)(resources.GetObject("buttonPreview.IconPadding"))));
            this.buttonPreview.Name = "buttonPreview";
            this.toolTip1.SetToolTip(this.buttonPreview, resources.GetString("buttonPreview.ToolTip"));
            this.buttonPreview.UseVisualStyleBackColor = true;
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // previewTable
            // 
            resources.ApplyResources(this.previewTable, "previewTable");
            this.previewTable.AllowUserToAddRows = false;
            this.previewTable.AllowUserToDeleteRows = false;
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
            this.dirtyErrorProvider.SetError(this.previewTable, resources.GetString("previewTable.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.previewTable, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("previewTable.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.previewTable, ((int)(resources.GetObject("previewTable.IconPadding"))));
            this.previewTable.MultiSelect = false;
            this.previewTable.Name = "previewTable";
            this.previewTable.RowHeadersVisible = false;
            this.previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.toolTip1.SetToolTip(this.previewTable, resources.GetString("previewTable.ToolTip"));
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
            this.dirtyErrorProvider.SetError(this.buttonReapply, resources.GetString("buttonReapply.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonReapply, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonReapply.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonReapply, ((int)(resources.GetObject("buttonReapply.IconPadding"))));
            this.buttonReapply.Name = "buttonReapply";
            this.toolTip1.SetToolTip(this.buttonReapply, resources.GetString("buttonReapply.ToolTip"));
            this.buttonReapply.UseVisualStyleBackColor = true;
            this.buttonReapply.Click += new System.EventHandler(this.buttonReapply_Click);
            // 
            // exceptionWordsCheckBox
            // 
            resources.ApplyResources(this.exceptionWordsCheckBox, "exceptionWordsCheckBox");
            this.dirtyErrorProvider.SetError(this.exceptionWordsCheckBox, resources.GetString("exceptionWordsCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.exceptionWordsCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("exceptionWordsCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.exceptionWordsCheckBox, ((int)(resources.GetObject("exceptionWordsCheckBox.IconPadding"))));
            this.exceptionWordsCheckBox.Name = "exceptionWordsCheckBox";
            this.toolTip1.SetToolTip(this.exceptionWordsCheckBox, resources.GetString("exceptionWordsCheckBox.ToolTip"));
            this.exceptionWordsCheckBox.UseVisualStyleBackColor = true;
            this.exceptionWordsCheckBox.CheckedChanged += new System.EventHandler(this.exceptWordsCheckBox_CheckedChanged);
            // 
            // exceptionCharsCheckBox
            // 
            resources.ApplyResources(this.exceptionCharsCheckBox, "exceptionCharsCheckBox");
            this.dirtyErrorProvider.SetError(this.exceptionCharsCheckBox, resources.GetString("exceptionCharsCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.exceptionCharsCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("exceptionCharsCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.exceptionCharsCheckBox, ((int)(resources.GetObject("exceptionCharsCheckBox.IconPadding"))));
            this.exceptionCharsCheckBox.Name = "exceptionCharsCheckBox";
            this.toolTip1.SetToolTip(this.exceptionCharsCheckBox, resources.GetString("exceptionCharsCheckBox.ToolTip"));
            this.exceptionCharsCheckBox.UseVisualStyleBackColor = true;
            this.exceptionCharsCheckBox.CheckedChanged += new System.EventHandler(this.exceptCharsCheckBox_CheckedChanged);
            // 
            // wordSplittersCheckBox
            // 
            resources.ApplyResources(this.wordSplittersCheckBox, "wordSplittersCheckBox");
            this.dirtyErrorProvider.SetError(this.wordSplittersCheckBox, resources.GetString("wordSplittersCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.wordSplittersCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("wordSplittersCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.wordSplittersCheckBox, ((int)(resources.GetObject("wordSplittersCheckBox.IconPadding"))));
            this.wordSplittersCheckBox.Name = "wordSplittersCheckBox";
            this.toolTip1.SetToolTip(this.wordSplittersCheckBox, resources.GetString("wordSplittersCheckBox.ToolTip"));
            this.wordSplittersCheckBox.UseVisualStyleBackColor = true;
            this.wordSplittersCheckBox.CheckedChanged += new System.EventHandler(this.wordSplittersCheckBox_CheckedChanged);
            // 
            // onlyWordsCheckBox
            // 
            resources.ApplyResources(this.onlyWordsCheckBox, "onlyWordsCheckBox");
            this.dirtyErrorProvider.SetError(this.onlyWordsCheckBox, resources.GetString("onlyWordsCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.onlyWordsCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("onlyWordsCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.onlyWordsCheckBox, ((int)(resources.GetObject("onlyWordsCheckBox.IconPadding"))));
            this.onlyWordsCheckBox.Name = "onlyWordsCheckBox";
            this.toolTip1.SetToolTip(this.onlyWordsCheckBox, resources.GetString("onlyWordsCheckBox.ToolTip"));
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
            this.dirtyErrorProvider.SetError(this.buttonSettings, resources.GetString("buttonSettings.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonSettings, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonSettings.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonSettings, ((int)(resources.GetObject("buttonSettings.IconPadding"))));
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
            this.dirtyErrorProvider.SetError(this.exceptionWordsBox, resources.GetString("exceptionWordsBox.Error"));
            this.exceptionWordsBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.exceptionWordsBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("exceptionWordsBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.exceptionWordsBox, ((int)(resources.GetObject("exceptionWordsBox.IconPadding"))));
            this.exceptionWordsBox.Name = "exceptionWordsBox";
            this.toolTip1.SetToolTip(this.exceptionWordsBox, resources.GetString("exceptionWordsBox.ToolTip"));
            this.exceptionWordsBox.Leave += new System.EventHandler(this.exceptionWordsBox_Leave);
            // 
            // alwaysCapitalize1stWordCheckBox
            // 
            resources.ApplyResources(this.alwaysCapitalize1stWordCheckBox, "alwaysCapitalize1stWordCheckBox");
            this.dirtyErrorProvider.SetError(this.alwaysCapitalize1stWordCheckBox, resources.GetString("alwaysCapitalize1stWordCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.alwaysCapitalize1stWordCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("alwaysCapitalize1stWordCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.alwaysCapitalize1stWordCheckBox, ((int)(resources.GetObject("alwaysCapitalize1stWordCheckBox.IconPadding"))));
            this.alwaysCapitalize1stWordCheckBox.Name = "alwaysCapitalize1stWordCheckBox";
            this.toolTip1.SetToolTip(this.alwaysCapitalize1stWordCheckBox, resources.GetString("alwaysCapitalize1stWordCheckBox.ToolTip"));
            this.alwaysCapitalize1stWordCheckBox.UseVisualStyleBackColor = true;
            // 
            // alwaysCapitalizeLastWordCheckBox
            // 
            resources.ApplyResources(this.alwaysCapitalizeLastWordCheckBox, "alwaysCapitalizeLastWordCheckBox");
            this.dirtyErrorProvider.SetError(this.alwaysCapitalizeLastWordCheckBox, resources.GetString("alwaysCapitalizeLastWordCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.alwaysCapitalizeLastWordCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("alwaysCapitalizeLastWordCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.alwaysCapitalizeLastWordCheckBox, ((int)(resources.GetObject("alwaysCapitalizeLastWordCheckBox.IconPadding"))));
            this.alwaysCapitalizeLastWordCheckBox.Name = "alwaysCapitalizeLastWordCheckBox";
            this.toolTip1.SetToolTip(this.alwaysCapitalizeLastWordCheckBox, resources.GetString("alwaysCapitalizeLastWordCheckBox.ToolTip"));
            this.alwaysCapitalizeLastWordCheckBox.UseVisualStyleBackColor = true;
            // 
            // removeExceptionButton
            // 
            resources.ApplyResources(this.removeExceptionButton, "removeExceptionButton");
            this.dirtyErrorProvider.SetError(this.removeExceptionButton, resources.GetString("removeExceptionButton.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.removeExceptionButton, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("removeExceptionButton.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.removeExceptionButton, ((int)(resources.GetObject("removeExceptionButton.IconPadding"))));
            this.removeExceptionButton.Image = global::MusicBeePlugin.Properties.Resources.clear_button_15;
            this.removeExceptionButton.Name = "removeExceptionButton";
            this.toolTip1.SetToolTip(this.removeExceptionButton, resources.GetString("removeExceptionButton.ToolTip"));
            this.removeExceptionButton.UseVisualStyleBackColor = true;
            this.removeExceptionButton.Click += new System.EventHandler(this.removeExceptionButton_Click);
            // 
            // buttonASRExceptedWords
            // 
            resources.ApplyResources(this.buttonASRExceptedWords, "buttonASRExceptedWords");
            this.dirtyErrorProvider.SetError(this.buttonASRExceptedWords, resources.GetString("buttonASRExceptedWords.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonASRExceptedWords, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonASRExceptedWords.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonASRExceptedWords, ((int)(resources.GetObject("buttonASRExceptedWords.IconPadding"))));
            this.buttonASRExceptedWords.Name = "buttonASRExceptedWords";
            this.toolTip1.SetToolTip(this.buttonASRExceptedWords, resources.GetString("buttonASRExceptedWords.ToolTip"));
            this.buttonASRExceptedWords.UseVisualStyleBackColor = true;
            this.buttonASRExceptedWords.Click += new System.EventHandler(this.buttonASR_Click);
            // 
            // buttonASRExceptWordsAfterSymbols
            // 
            resources.ApplyResources(this.buttonASRExceptWordsAfterSymbols, "buttonASRExceptWordsAfterSymbols");
            this.dirtyErrorProvider.SetError(this.buttonASRExceptWordsAfterSymbols, resources.GetString("buttonASRExceptWordsAfterSymbols.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonASRExceptWordsAfterSymbols, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonASRExceptWordsAfterSymbols.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonASRExceptWordsAfterSymbols, ((int)(resources.GetObject("buttonASRExceptWordsAfterSymbols.IconPadding"))));
            this.buttonASRExceptWordsAfterSymbols.Name = "buttonASRExceptWordsAfterSymbols";
            this.toolTip1.SetToolTip(this.buttonASRExceptWordsAfterSymbols, resources.GetString("buttonASRExceptWordsAfterSymbols.ToolTip"));
            this.buttonASRExceptWordsAfterSymbols.UseVisualStyleBackColor = true;
            this.buttonASRExceptWordsAfterSymbols.Click += new System.EventHandler(this.buttonASRExceptWordsAfterSymbols_Click);
            // 
            // buttonASRWordSplitters
            // 
            resources.ApplyResources(this.buttonASRWordSplitters, "buttonASRWordSplitters");
            this.dirtyErrorProvider.SetError(this.buttonASRWordSplitters, resources.GetString("buttonASRWordSplitters.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonASRWordSplitters, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonASRWordSplitters.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonASRWordSplitters, ((int)(resources.GetObject("buttonASRWordSplitters.IconPadding"))));
            this.buttonASRWordSplitters.Name = "buttonASRWordSplitters";
            this.toolTip1.SetToolTip(this.buttonASRWordSplitters, resources.GetString("buttonASRWordSplitters.ToolTip"));
            this.buttonASRWordSplitters.UseVisualStyleBackColor = true;
            this.buttonASRWordSplitters.Click += new System.EventHandler(this.buttonASRWordSplitters_Click);
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
    }
}