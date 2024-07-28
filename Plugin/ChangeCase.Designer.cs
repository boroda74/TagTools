namespace MusicBeePlugin
{
    partial class ChangeCase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangeCase));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.forSelectedTracksLabel = new System.Windows.Forms.Label();
            this.sourceTagList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.sentenceCaseRadioButton = new System.Windows.Forms.RadioButton();
            this.lowerCaseRadioButton = new System.Windows.Forms.RadioButton();
            this.upperCaseRadioButton = new System.Windows.Forms.RadioButton();
            this.titleCaseRadioButton = new System.Windows.Forms.RadioButton();
            this.toggleCaseRadioButton = new System.Windows.Forms.RadioButton();
            this.exceptionCharsBox = new System.Windows.Forms.ComboBox();
            this.wordSeparatorsBox = new System.Windows.Forms.ComboBox();
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
            this.wordSeparatorsCheckBox = new System.Windows.Forms.CheckBox();
            this.onlyWordsCheckBox = new System.Windows.Forms.CheckBox();
            this.dirtyErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.buttonSettings = new System.Windows.Forms.Button();
            this.exceptionWordsBox = new System.Windows.Forms.ComboBox();
            this.alwaysCapitalize1stWordCheckBox = new System.Windows.Forms.CheckBox();
            this.alwaysCapitalizeLastWordCheckBox = new System.Windows.Forms.CheckBox();
            this.removeExceptionButton = new System.Windows.Forms.Button();
            this.buttonAsrExceptedWords = new System.Windows.Forms.Button();
            this.buttonAsrExceptWordsAfterSymbols = new System.Windows.Forms.Button();
            this.buttonAsrWordSeparators = new System.Windows.Forms.Button();
            this.sentenceCaseRadioButtonLabel = new System.Windows.Forms.Label();
            this.lowerCaseRadioButtonLabel = new System.Windows.Forms.Label();
            this.upperCaseRadioButtonLabel = new System.Windows.Forms.Label();
            this.titleCaseRadioButtonLabel = new System.Windows.Forms.Label();
            this.toggleCaseRadioButtonLabel = new System.Windows.Forms.Label();
            this.exceptionWordsCheckBoxLabel = new System.Windows.Forms.Label();
            this.onlyWordsCheckBoxLabel = new System.Windows.Forms.Label();
            this.exceptionCharsCheckBoxLabel = new System.Windows.Forms.Label();
            this.wordSeparatorsCheckBoxLabel = new System.Windows.Forms.Label();
            this.alwaysCapitalize1stWordCheckBoxLabel = new System.Windows.Forms.Label();
            this.alwaysCapitalizeLastWordCheckBoxLabel = new System.Windows.Forms.Label();
            this.fieldsPanel = new System.Windows.Forms.Panel();
            this.buttonAsrExceptWordsBetweenSymbols = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rightExceptionCharsBox = new System.Windows.Forms.ComboBox();
            this.leftExceptionCharsBox = new System.Windows.Forms.ComboBox();
            this.exceptionCharPairsCheckBoxLabel = new System.Windows.Forms.Label();
            this.exceptionCharPairsCheckBox = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).BeginInit();
            this.fieldsPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dirtyErrorProvider.SetError(this.buttonCancel, resources.GetString("buttonCancel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonCancel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonCancel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonCancel, ((int)(resources.GetObject("buttonCancel.IconPadding"))));
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Tag = "#fieldsPanel@pinned-to-parent-x@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonCancel, resources.GetString("buttonCancel.ToolTip"));
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.dirtyErrorProvider.SetError(this.buttonOK, resources.GetString("buttonOK.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonOK, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonOK.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonOK, ((int)(resources.GetObject("buttonOK.IconPadding"))));
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Tag = "#buttonCancel";
            this.toolTip1.SetToolTip(this.buttonOK, resources.GetString("buttonOK.ToolTip"));
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // forSelectedTracksLabel
            // 
            resources.ApplyResources(this.forSelectedTracksLabel, "forSelectedTracksLabel");
            this.dirtyErrorProvider.SetError(this.forSelectedTracksLabel, resources.GetString("forSelectedTracksLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.forSelectedTracksLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("forSelectedTracksLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.forSelectedTracksLabel, ((int)(resources.GetObject("forSelectedTracksLabel.IconPadding"))));
            this.forSelectedTracksLabel.Name = "forSelectedTracksLabel";
            this.toolTip1.SetToolTip(this.forSelectedTracksLabel, resources.GetString("forSelectedTracksLabel.ToolTip"));
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
            this.sourceTagList.Tag = "#forSelectedTracksLabel";
            this.toolTip1.SetToolTip(this.sourceTagList, resources.GetString("sourceTagList.ToolTip"));
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.dirtyErrorProvider.SetError(this.label1, resources.GetString("label1.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            this.label1.Tag = "#sourceTagList@pinned-to-parent-x";
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
            this.sentenceCaseRadioButton.Tag = "#sentenceCaseRadioButtonLabel@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.sentenceCaseRadioButton, resources.GetString("sentenceCaseRadioButton.ToolTip"));
            this.sentenceCaseRadioButton.CheckedChanged += new System.EventHandler(this.casingRuleRadioButton_CheckedChanged);
            // 
            // lowerCaseRadioButton
            // 
            resources.ApplyResources(this.lowerCaseRadioButton, "lowerCaseRadioButton");
            this.dirtyErrorProvider.SetError(this.lowerCaseRadioButton, resources.GetString("lowerCaseRadioButton.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.lowerCaseRadioButton, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lowerCaseRadioButton.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.lowerCaseRadioButton, ((int)(resources.GetObject("lowerCaseRadioButton.IconPadding"))));
            this.lowerCaseRadioButton.Name = "lowerCaseRadioButton";
            this.lowerCaseRadioButton.Tag = "#lowerCaseRadioButtonLabel@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.lowerCaseRadioButton, resources.GetString("lowerCaseRadioButton.ToolTip"));
            this.lowerCaseRadioButton.CheckedChanged += new System.EventHandler(this.casingRuleRadioButton_CheckedChanged);
            // 
            // upperCaseRadioButton
            // 
            resources.ApplyResources(this.upperCaseRadioButton, "upperCaseRadioButton");
            this.dirtyErrorProvider.SetError(this.upperCaseRadioButton, resources.GetString("upperCaseRadioButton.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.upperCaseRadioButton, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("upperCaseRadioButton.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.upperCaseRadioButton, ((int)(resources.GetObject("upperCaseRadioButton.IconPadding"))));
            this.upperCaseRadioButton.Name = "upperCaseRadioButton";
            this.upperCaseRadioButton.Tag = "#upperCaseRadioButtonLabel@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.upperCaseRadioButton, resources.GetString("upperCaseRadioButton.ToolTip"));
            this.upperCaseRadioButton.CheckedChanged += new System.EventHandler(this.casingRuleRadioButton_CheckedChanged);
            // 
            // titleCaseRadioButton
            // 
            resources.ApplyResources(this.titleCaseRadioButton, "titleCaseRadioButton");
            this.dirtyErrorProvider.SetError(this.titleCaseRadioButton, resources.GetString("titleCaseRadioButton.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.titleCaseRadioButton, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("titleCaseRadioButton.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.titleCaseRadioButton, ((int)(resources.GetObject("titleCaseRadioButton.IconPadding"))));
            this.titleCaseRadioButton.Name = "titleCaseRadioButton";
            this.titleCaseRadioButton.Tag = "#titleCaseRadioButtonLabel@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.titleCaseRadioButton, resources.GetString("titleCaseRadioButton.ToolTip"));
            this.titleCaseRadioButton.CheckedChanged += new System.EventHandler(this.casingRuleRadioButton_CheckedChanged);
            // 
            // toggleCaseRadioButton
            // 
            resources.ApplyResources(this.toggleCaseRadioButton, "toggleCaseRadioButton");
            this.dirtyErrorProvider.SetError(this.toggleCaseRadioButton, resources.GetString("toggleCaseRadioButton.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.toggleCaseRadioButton, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("toggleCaseRadioButton.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.toggleCaseRadioButton, ((int)(resources.GetObject("toggleCaseRadioButton.IconPadding"))));
            this.toggleCaseRadioButton.Name = "toggleCaseRadioButton";
            this.toggleCaseRadioButton.Tag = "#toggleCaseRadioButtonLabel@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.toggleCaseRadioButton, resources.GetString("toggleCaseRadioButton.ToolTip"));
            this.toggleCaseRadioButton.CheckedChanged += new System.EventHandler(this.casingRuleRadioButton_CheckedChanged);
            // 
            // exceptionCharsBox
            // 
            resources.ApplyResources(this.exceptionCharsBox, "exceptionCharsBox");
            this.exceptionCharsBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.exceptionCharsBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.dirtyErrorProvider.SetError(this.exceptionCharsBox, resources.GetString("exceptionCharsBox.Error"));
            this.exceptionCharsBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.exceptionCharsBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("exceptionCharsBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.exceptionCharsBox, ((int)(resources.GetObject("exceptionCharsBox.IconPadding"))));
            this.exceptionCharsBox.Name = "exceptionCharsBox";
            this.exceptionCharsBox.Tag = "#buttonAsrExceptWordsAfterSymbols@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.exceptionCharsBox, resources.GetString("exceptionCharsBox.ToolTip"));
            this.exceptionCharsBox.Leave += new System.EventHandler(this.exceptionCharsBox_Leave);
            // 
            // wordSeparatorsBox
            // 
            resources.ApplyResources(this.wordSeparatorsBox, "wordSeparatorsBox");
            this.wordSeparatorsBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.wordSeparatorsBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.dirtyErrorProvider.SetError(this.wordSeparatorsBox, resources.GetString("wordSeparatorsBox.Error"));
            this.wordSeparatorsBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.wordSeparatorsBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("wordSeparatorsBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.wordSeparatorsBox, ((int)(resources.GetObject("wordSeparatorsBox.IconPadding"))));
            this.wordSeparatorsBox.Name = "wordSeparatorsBox";
            this.wordSeparatorsBox.Tag = "#buttonAsrWordSeparators@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.wordSeparatorsBox, resources.GetString("wordSeparatorsBox.ToolTip"));
            this.wordSeparatorsBox.Leave += new System.EventHandler(this.wordSeparatorsBox_Leave);
            // 
            // buttonPreview
            // 
            resources.ApplyResources(this.buttonPreview, "buttonPreview");
            this.dirtyErrorProvider.SetError(this.buttonPreview, resources.GetString("buttonPreview.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonPreview, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonPreview.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonPreview, ((int)(resources.GetObject("buttonPreview.IconPadding"))));
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Tag = "#buttonOK";
            this.toolTip1.SetToolTip(this.buttonPreview, resources.GetString("buttonPreview.ToolTip"));
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
            this.previewTable.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.previewTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.File,
            this.Track,
            this.OriginalTag,
            this.OriginalTagT,
            this.NewTag,
            this.NewTagT});
            this.dirtyErrorProvider.SetError(this.previewTable, resources.GetString("previewTable.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.previewTable, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("previewTable.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.previewTable, ((int)(resources.GetObject("previewTable.IconPadding"))));
            this.previewTable.MultiSelect = false;
            this.previewTable.Name = "previewTable";
            this.previewTable.RowHeadersVisible = false;
            this.previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.previewTable.Tag = "#ChangeCase&ChangeCase@pinned-to-parent-x";
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
            this.Track.ReadOnly = true;
            // 
            // OriginalTag
            // 
            this.OriginalTag.FillWeight = 1F;
            resources.ApplyResources(this.OriginalTag, "OriginalTag");
            this.OriginalTag.Name = "OriginalTag";
            this.OriginalTag.ReadOnly = true;
            // 
            // OriginalTagT
            // 
            this.OriginalTagT.FillWeight = 25F;
            resources.ApplyResources(this.OriginalTagT, "OriginalTagT");
            this.OriginalTagT.Name = "OriginalTagT";
            this.OriginalTagT.ReadOnly = true;
            // 
            // NewTag
            // 
            this.NewTag.FillWeight = 1F;
            resources.ApplyResources(this.NewTag, "NewTag");
            this.NewTag.Name = "NewTag";
            this.NewTag.ReadOnly = true;
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
            this.buttonReapply.Tag = "#buttonSettings@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.buttonReapply, resources.GetString("buttonReapply.ToolTip"));
            this.buttonReapply.Click += new System.EventHandler(this.buttonReapply_Click);
            // 
            // exceptionWordsCheckBox
            // 
            resources.ApplyResources(this.exceptionWordsCheckBox, "exceptionWordsCheckBox");
            this.dirtyErrorProvider.SetError(this.exceptionWordsCheckBox, resources.GetString("exceptionWordsCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.exceptionWordsCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("exceptionWordsCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.exceptionWordsCheckBox, ((int)(resources.GetObject("exceptionWordsCheckBox.IconPadding"))));
            this.exceptionWordsCheckBox.Name = "exceptionWordsCheckBox";
            this.exceptionWordsCheckBox.Tag = "#exceptionWordsCheckBoxLabel";
            this.toolTip1.SetToolTip(this.exceptionWordsCheckBox, resources.GetString("exceptionWordsCheckBox.ToolTip"));
            this.exceptionWordsCheckBox.CheckedChanged += new System.EventHandler(this.exceptWordsCheckBox_CheckedChanged);
            // 
            // exceptionCharsCheckBox
            // 
            resources.ApplyResources(this.exceptionCharsCheckBox, "exceptionCharsCheckBox");
            this.dirtyErrorProvider.SetError(this.exceptionCharsCheckBox, resources.GetString("exceptionCharsCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.exceptionCharsCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("exceptionCharsCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.exceptionCharsCheckBox, ((int)(resources.GetObject("exceptionCharsCheckBox.IconPadding"))));
            this.exceptionCharsCheckBox.Name = "exceptionCharsCheckBox";
            this.exceptionCharsCheckBox.Tag = "#exceptionCharsCheckBoxLabel";
            this.toolTip1.SetToolTip(this.exceptionCharsCheckBox, resources.GetString("exceptionCharsCheckBox.ToolTip"));
            this.exceptionCharsCheckBox.CheckedChanged += new System.EventHandler(this.exceptCharsCheckBox_CheckedChanged);
            // 
            // wordSeparatorsCheckBox
            // 
            resources.ApplyResources(this.wordSeparatorsCheckBox, "wordSeparatorsCheckBox");
            this.dirtyErrorProvider.SetError(this.wordSeparatorsCheckBox, resources.GetString("wordSeparatorsCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.wordSeparatorsCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("wordSeparatorsCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.wordSeparatorsCheckBox, ((int)(resources.GetObject("wordSeparatorsCheckBox.IconPadding"))));
            this.wordSeparatorsCheckBox.Name = "wordSeparatorsCheckBox";
            this.wordSeparatorsCheckBox.Tag = "#wordSeparatorsCheckBoxLabel";
            this.toolTip1.SetToolTip(this.wordSeparatorsCheckBox, resources.GetString("wordSeparatorsCheckBox.ToolTip"));
            this.wordSeparatorsCheckBox.CheckedChanged += new System.EventHandler(this.wordSeparatorsCheckBox_CheckedChanged);
            // 
            // onlyWordsCheckBox
            // 
            resources.ApplyResources(this.onlyWordsCheckBox, "onlyWordsCheckBox");
            this.dirtyErrorProvider.SetError(this.onlyWordsCheckBox, resources.GetString("onlyWordsCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.onlyWordsCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("onlyWordsCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.onlyWordsCheckBox, ((int)(resources.GetObject("onlyWordsCheckBox.IconPadding"))));
            this.onlyWordsCheckBox.Name = "onlyWordsCheckBox";
            this.onlyWordsCheckBox.Tag = "#onlyWordsCheckBoxLabel";
            this.toolTip1.SetToolTip(this.onlyWordsCheckBox, resources.GetString("onlyWordsCheckBox.ToolTip"));
            this.onlyWordsCheckBox.CheckedChanged += new System.EventHandler(this.onlyWordsCheckBox_CheckedChanged);
            // 
            // dirtyErrorProvider
            // 
            this.dirtyErrorProvider.BlinkRate = 1000;
            this.dirtyErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
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
            this.buttonSettings.Tag = "@non-defaultable@square-button";
            this.toolTip1.SetToolTip(this.buttonSettings, resources.GetString("buttonSettings.ToolTip"));
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
            this.exceptionWordsBox.Tag = "#removeExceptionButton@pinned-to-parent-x";
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
            this.alwaysCapitalize1stWordCheckBox.Tag = "#alwaysCapitalize1stWordCheckBoxLabel";
            this.alwaysCapitalize1stWordCheckBox.ThreeState = true;
            this.toolTip1.SetToolTip(this.alwaysCapitalize1stWordCheckBox, resources.GetString("alwaysCapitalize1stWordCheckBox.ToolTip"));
            // 
            // alwaysCapitalizeLastWordCheckBox
            // 
            resources.ApplyResources(this.alwaysCapitalizeLastWordCheckBox, "alwaysCapitalizeLastWordCheckBox");
            this.dirtyErrorProvider.SetError(this.alwaysCapitalizeLastWordCheckBox, resources.GetString("alwaysCapitalizeLastWordCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.alwaysCapitalizeLastWordCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("alwaysCapitalizeLastWordCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.alwaysCapitalizeLastWordCheckBox, ((int)(resources.GetObject("alwaysCapitalizeLastWordCheckBox.IconPadding"))));
            this.alwaysCapitalizeLastWordCheckBox.Name = "alwaysCapitalizeLastWordCheckBox";
            this.alwaysCapitalizeLastWordCheckBox.Tag = "#alwaysCapitalizeLastWordCheckBoxLabel";
            this.alwaysCapitalizeLastWordCheckBox.ThreeState = true;
            this.toolTip1.SetToolTip(this.alwaysCapitalizeLastWordCheckBox, resources.GetString("alwaysCapitalizeLastWordCheckBox.ToolTip"));
            // 
            // removeExceptionButton
            // 
            resources.ApplyResources(this.removeExceptionButton, "removeExceptionButton");
            this.dirtyErrorProvider.SetError(this.removeExceptionButton, resources.GetString("removeExceptionButton.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.removeExceptionButton, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("removeExceptionButton.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.removeExceptionButton, ((int)(resources.GetObject("removeExceptionButton.IconPadding"))));
            this.removeExceptionButton.Name = "removeExceptionButton";
            this.removeExceptionButton.Tag = "#fieldsPanel@pinned-to-parent-x@non-defaultable";
            this.toolTip1.SetToolTip(this.removeExceptionButton, resources.GetString("removeExceptionButton.ToolTip"));
            this.removeExceptionButton.Click += new System.EventHandler(this.removeExceptionButton_Click);
            // 
            // buttonAsrExceptedWords
            // 
            resources.ApplyResources(this.buttonAsrExceptedWords, "buttonAsrExceptedWords");
            this.dirtyErrorProvider.SetError(this.buttonAsrExceptedWords, resources.GetString("buttonAsrExceptedWords.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonAsrExceptedWords, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonAsrExceptedWords.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonAsrExceptedWords, ((int)(resources.GetObject("buttonAsrExceptedWords.IconPadding"))));
            this.buttonAsrExceptedWords.Name = "buttonAsrExceptedWords";
            this.buttonAsrExceptedWords.Tag = "#fieldsPanel@pinned-to-parent-x@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonAsrExceptedWords, resources.GetString("buttonAsrExceptedWords.ToolTip"));
            this.buttonAsrExceptedWords.Click += new System.EventHandler(this.buttonAsr_Click);
            // 
            // buttonAsrExceptWordsAfterSymbols
            // 
            resources.ApplyResources(this.buttonAsrExceptWordsAfterSymbols, "buttonAsrExceptWordsAfterSymbols");
            this.dirtyErrorProvider.SetError(this.buttonAsrExceptWordsAfterSymbols, resources.GetString("buttonAsrExceptWordsAfterSymbols.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonAsrExceptWordsAfterSymbols, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonAsrExceptWordsAfterSymbols.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonAsrExceptWordsAfterSymbols, ((int)(resources.GetObject("buttonAsrExceptWordsAfterSymbols.IconPadding"))));
            this.buttonAsrExceptWordsAfterSymbols.Name = "buttonAsrExceptWordsAfterSymbols";
            this.buttonAsrExceptWordsAfterSymbols.Tag = "#fieldsPanel@pinned-to-parentl@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonAsrExceptWordsAfterSymbols, resources.GetString("buttonAsrExceptWordsAfterSymbols.ToolTip"));
            this.buttonAsrExceptWordsAfterSymbols.Click += new System.EventHandler(this.buttonAsrExceptWordsAfterSymbols_Click);
            // 
            // buttonAsrWordSeparators
            // 
            resources.ApplyResources(this.buttonAsrWordSeparators, "buttonAsrWordSeparators");
            this.dirtyErrorProvider.SetError(this.buttonAsrWordSeparators, resources.GetString("buttonAsrWordSeparators.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonAsrWordSeparators, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonAsrWordSeparators.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonAsrWordSeparators, ((int)(resources.GetObject("buttonAsrWordSeparators.IconPadding"))));
            this.buttonAsrWordSeparators.Name = "buttonAsrWordSeparators";
            this.buttonAsrWordSeparators.Tag = "#fieldsPanel@pinned-to-parent-x@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonAsrWordSeparators, resources.GetString("buttonAsrWordSeparators.ToolTip"));
            this.buttonAsrWordSeparators.Click += new System.EventHandler(this.buttonAsrWordSeparators_Click);
            // 
            // sentenceCaseRadioButtonLabel
            // 
            resources.ApplyResources(this.sentenceCaseRadioButtonLabel, "sentenceCaseRadioButtonLabel");
            this.dirtyErrorProvider.SetError(this.sentenceCaseRadioButtonLabel, resources.GetString("sentenceCaseRadioButtonLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.sentenceCaseRadioButtonLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("sentenceCaseRadioButtonLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.sentenceCaseRadioButtonLabel, ((int)(resources.GetObject("sentenceCaseRadioButtonLabel.IconPadding"))));
            this.sentenceCaseRadioButtonLabel.Name = "sentenceCaseRadioButtonLabel";
            this.toolTip1.SetToolTip(this.sentenceCaseRadioButtonLabel, resources.GetString("sentenceCaseRadioButtonLabel.ToolTip"));
            this.sentenceCaseRadioButtonLabel.Click += new System.EventHandler(this.sentenceCaseRadioButtonLabel_Click);
            // 
            // lowerCaseRadioButtonLabel
            // 
            resources.ApplyResources(this.lowerCaseRadioButtonLabel, "lowerCaseRadioButtonLabel");
            this.dirtyErrorProvider.SetError(this.lowerCaseRadioButtonLabel, resources.GetString("lowerCaseRadioButtonLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.lowerCaseRadioButtonLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lowerCaseRadioButtonLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.lowerCaseRadioButtonLabel, ((int)(resources.GetObject("lowerCaseRadioButtonLabel.IconPadding"))));
            this.lowerCaseRadioButtonLabel.Name = "lowerCaseRadioButtonLabel";
            this.toolTip1.SetToolTip(this.lowerCaseRadioButtonLabel, resources.GetString("lowerCaseRadioButtonLabel.ToolTip"));
            this.lowerCaseRadioButtonLabel.Click += new System.EventHandler(this.lowerCaseRadioButtonLabel_Click);
            // 
            // upperCaseRadioButtonLabel
            // 
            resources.ApplyResources(this.upperCaseRadioButtonLabel, "upperCaseRadioButtonLabel");
            this.dirtyErrorProvider.SetError(this.upperCaseRadioButtonLabel, resources.GetString("upperCaseRadioButtonLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.upperCaseRadioButtonLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("upperCaseRadioButtonLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.upperCaseRadioButtonLabel, ((int)(resources.GetObject("upperCaseRadioButtonLabel.IconPadding"))));
            this.upperCaseRadioButtonLabel.Name = "upperCaseRadioButtonLabel";
            this.toolTip1.SetToolTip(this.upperCaseRadioButtonLabel, resources.GetString("upperCaseRadioButtonLabel.ToolTip"));
            this.upperCaseRadioButtonLabel.Click += new System.EventHandler(this.upperCaseRadioButtonLabel_Click);
            // 
            // titleCaseRadioButtonLabel
            // 
            resources.ApplyResources(this.titleCaseRadioButtonLabel, "titleCaseRadioButtonLabel");
            this.dirtyErrorProvider.SetError(this.titleCaseRadioButtonLabel, resources.GetString("titleCaseRadioButtonLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.titleCaseRadioButtonLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("titleCaseRadioButtonLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.titleCaseRadioButtonLabel, ((int)(resources.GetObject("titleCaseRadioButtonLabel.IconPadding"))));
            this.titleCaseRadioButtonLabel.Name = "titleCaseRadioButtonLabel";
            this.toolTip1.SetToolTip(this.titleCaseRadioButtonLabel, resources.GetString("titleCaseRadioButtonLabel.ToolTip"));
            this.titleCaseRadioButtonLabel.Click += new System.EventHandler(this.titleCaseRadioButtonLabel_Click);
            // 
            // toggleCaseRadioButtonLabel
            // 
            resources.ApplyResources(this.toggleCaseRadioButtonLabel, "toggleCaseRadioButtonLabel");
            this.dirtyErrorProvider.SetError(this.toggleCaseRadioButtonLabel, resources.GetString("toggleCaseRadioButtonLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.toggleCaseRadioButtonLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("toggleCaseRadioButtonLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.toggleCaseRadioButtonLabel, ((int)(resources.GetObject("toggleCaseRadioButtonLabel.IconPadding"))));
            this.toggleCaseRadioButtonLabel.Name = "toggleCaseRadioButtonLabel";
            this.toggleCaseRadioButtonLabel.Tag = "#&previewTable";
            this.toolTip1.SetToolTip(this.toggleCaseRadioButtonLabel, resources.GetString("toggleCaseRadioButtonLabel.ToolTip"));
            this.toggleCaseRadioButtonLabel.Click += new System.EventHandler(this.toggleCaseRadioButtonLabel_Click);
            // 
            // exceptionWordsCheckBoxLabel
            // 
            resources.ApplyResources(this.exceptionWordsCheckBoxLabel, "exceptionWordsCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.exceptionWordsCheckBoxLabel, resources.GetString("exceptionWordsCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.exceptionWordsCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("exceptionWordsCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.exceptionWordsCheckBoxLabel, ((int)(resources.GetObject("exceptionWordsCheckBoxLabel.IconPadding"))));
            this.exceptionWordsCheckBoxLabel.Name = "exceptionWordsCheckBoxLabel";
            this.exceptionWordsCheckBoxLabel.Tag = "#onlyWordsCheckBox";
            this.toolTip1.SetToolTip(this.exceptionWordsCheckBoxLabel, resources.GetString("exceptionWordsCheckBoxLabel.ToolTip"));
            this.exceptionWordsCheckBoxLabel.Click += new System.EventHandler(this.exceptionWordsCheckBoxLabel_Click);
            // 
            // onlyWordsCheckBoxLabel
            // 
            resources.ApplyResources(this.onlyWordsCheckBoxLabel, "onlyWordsCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.onlyWordsCheckBoxLabel, resources.GetString("onlyWordsCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.onlyWordsCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("onlyWordsCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.onlyWordsCheckBoxLabel, ((int)(resources.GetObject("onlyWordsCheckBoxLabel.IconPadding"))));
            this.onlyWordsCheckBoxLabel.Name = "onlyWordsCheckBoxLabel";
            this.onlyWordsCheckBoxLabel.Tag = "";
            this.toolTip1.SetToolTip(this.onlyWordsCheckBoxLabel, resources.GetString("onlyWordsCheckBoxLabel.ToolTip"));
            this.onlyWordsCheckBoxLabel.Click += new System.EventHandler(this.onlyWordsCheckBoxLabel_Click);
            // 
            // exceptionCharsCheckBoxLabel
            // 
            resources.ApplyResources(this.exceptionCharsCheckBoxLabel, "exceptionCharsCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.exceptionCharsCheckBoxLabel, resources.GetString("exceptionCharsCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.exceptionCharsCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("exceptionCharsCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.exceptionCharsCheckBoxLabel, ((int)(resources.GetObject("exceptionCharsCheckBoxLabel.IconPadding"))));
            this.exceptionCharsCheckBoxLabel.Name = "exceptionCharsCheckBoxLabel";
            this.toolTip1.SetToolTip(this.exceptionCharsCheckBoxLabel, resources.GetString("exceptionCharsCheckBoxLabel.ToolTip"));
            this.exceptionCharsCheckBoxLabel.Click += new System.EventHandler(this.exceptionCharsCheckBoxLabel_Click);
            // 
            // wordSeparatorsCheckBoxLabel
            // 
            resources.ApplyResources(this.wordSeparatorsCheckBoxLabel, "wordSeparatorsCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.wordSeparatorsCheckBoxLabel, resources.GetString("wordSeparatorsCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.wordSeparatorsCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("wordSeparatorsCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.wordSeparatorsCheckBoxLabel, ((int)(resources.GetObject("wordSeparatorsCheckBoxLabel.IconPadding"))));
            this.wordSeparatorsCheckBoxLabel.Name = "wordSeparatorsCheckBoxLabel";
            this.wordSeparatorsCheckBoxLabel.Tag = "#fieldsPanel";
            this.toolTip1.SetToolTip(this.wordSeparatorsCheckBoxLabel, resources.GetString("wordSeparatorsCheckBoxLabel.ToolTip"));
            this.wordSeparatorsCheckBoxLabel.Click += new System.EventHandler(this.wordSeparatorsCheckBoxLabel_Click);
            // 
            // alwaysCapitalize1stWordCheckBoxLabel
            // 
            resources.ApplyResources(this.alwaysCapitalize1stWordCheckBoxLabel, "alwaysCapitalize1stWordCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.alwaysCapitalize1stWordCheckBoxLabel, resources.GetString("alwaysCapitalize1stWordCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.alwaysCapitalize1stWordCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("alwaysCapitalize1stWordCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.alwaysCapitalize1stWordCheckBoxLabel, ((int)(resources.GetObject("alwaysCapitalize1stWordCheckBoxLabel.IconPadding"))));
            this.alwaysCapitalize1stWordCheckBoxLabel.Name = "alwaysCapitalize1stWordCheckBoxLabel";
            this.alwaysCapitalize1stWordCheckBoxLabel.Tag = "#alwaysCapitalizeLastWordCheckBox";
            this.toolTip1.SetToolTip(this.alwaysCapitalize1stWordCheckBoxLabel, resources.GetString("alwaysCapitalize1stWordCheckBoxLabel.ToolTip"));
            this.alwaysCapitalize1stWordCheckBoxLabel.Click += new System.EventHandler(this.alwaysCapitalize1stWordCheckBoxLabel_Click);
            // 
            // alwaysCapitalizeLastWordCheckBoxLabel
            // 
            resources.ApplyResources(this.alwaysCapitalizeLastWordCheckBoxLabel, "alwaysCapitalizeLastWordCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.alwaysCapitalizeLastWordCheckBoxLabel, resources.GetString("alwaysCapitalizeLastWordCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.alwaysCapitalizeLastWordCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("alwaysCapitalizeLastWordCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.alwaysCapitalizeLastWordCheckBoxLabel, ((int)(resources.GetObject("alwaysCapitalizeLastWordCheckBoxLabel.IconPadding"))));
            this.alwaysCapitalizeLastWordCheckBoxLabel.Name = "alwaysCapitalizeLastWordCheckBoxLabel";
            this.toolTip1.SetToolTip(this.alwaysCapitalizeLastWordCheckBoxLabel, resources.GetString("alwaysCapitalizeLastWordCheckBoxLabel.ToolTip"));
            this.alwaysCapitalizeLastWordCheckBoxLabel.Click += new System.EventHandler(this.alwaysCapitalizeLastWordCheckBoxLabel_Click);
            // 
            // fieldsPanel
            // 
            resources.ApplyResources(this.fieldsPanel, "fieldsPanel");
            this.fieldsPanel.Controls.Add(this.buttonAsrWordSeparators);
            this.fieldsPanel.Controls.Add(this.wordSeparatorsBox);
            this.fieldsPanel.Controls.Add(this.buttonAsrExceptWordsBetweenSymbols);
            this.fieldsPanel.Controls.Add(this.tableLayoutPanel1);
            this.fieldsPanel.Controls.Add(this.buttonAsrExceptWordsAfterSymbols);
            this.fieldsPanel.Controls.Add(this.exceptionCharsBox);
            this.fieldsPanel.Controls.Add(this.removeExceptionButton);
            this.fieldsPanel.Controls.Add(this.exceptionWordsBox);
            this.fieldsPanel.Controls.Add(this.buttonCancel);
            this.fieldsPanel.Controls.Add(this.buttonAsrExceptedWords);
            this.fieldsPanel.Controls.Add(this.buttonOK);
            this.fieldsPanel.Controls.Add(this.buttonPreview);
            this.fieldsPanel.Controls.Add(this.buttonSettings);
            this.fieldsPanel.Controls.Add(this.buttonReapply);
            this.dirtyErrorProvider.SetError(this.fieldsPanel, resources.GetString("fieldsPanel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.fieldsPanel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("fieldsPanel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.fieldsPanel, ((int)(resources.GetObject("fieldsPanel.IconPadding"))));
            this.fieldsPanel.Name = "fieldsPanel";
            this.fieldsPanel.Tag = "#ChangeCase";
            this.toolTip1.SetToolTip(this.fieldsPanel, resources.GetString("fieldsPanel.ToolTip"));
            // 
            // buttonAsrExceptWordsBetweenSymbols
            // 
            resources.ApplyResources(this.buttonAsrExceptWordsBetweenSymbols, "buttonAsrExceptWordsBetweenSymbols");
            this.dirtyErrorProvider.SetError(this.buttonAsrExceptWordsBetweenSymbols, resources.GetString("buttonAsrExceptWordsBetweenSymbols.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonAsrExceptWordsBetweenSymbols, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonAsrExceptWordsBetweenSymbols.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonAsrExceptWordsBetweenSymbols, ((int)(resources.GetObject("buttonAsrExceptWordsBetweenSymbols.IconPadding"))));
            this.buttonAsrExceptWordsBetweenSymbols.Name = "buttonAsrExceptWordsBetweenSymbols";
            this.buttonAsrExceptWordsBetweenSymbols.Tag = "#fieldsPanel@pinned-to-parentl@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonAsrExceptWordsBetweenSymbols, resources.GetString("buttonAsrExceptWordsBetweenSymbols.ToolTip"));
            this.buttonAsrExceptWordsBetweenSymbols.Click += new System.EventHandler(this.buttonAsrExceptWordsBetweenSymbols_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.rightExceptionCharsBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.leftExceptionCharsBox, 0, 0);
            this.dirtyErrorProvider.SetError(this.tableLayoutPanel1, resources.GetString("tableLayoutPanel1.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.tableLayoutPanel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tableLayoutPanel1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.tableLayoutPanel1, ((int)(resources.GetObject("tableLayoutPanel1.IconPadding"))));
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Tag = "#buttonAsrExceptWordsBetweenSymbols";
            this.toolTip1.SetToolTip(this.tableLayoutPanel1, resources.GetString("tableLayoutPanel1.ToolTip"));
            // 
            // rightExceptionCharsBox
            // 
            resources.ApplyResources(this.rightExceptionCharsBox, "rightExceptionCharsBox");
            this.dirtyErrorProvider.SetError(this.rightExceptionCharsBox, resources.GetString("rightExceptionCharsBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.rightExceptionCharsBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("rightExceptionCharsBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.rightExceptionCharsBox, ((int)(resources.GetObject("rightExceptionCharsBox.IconPadding"))));
            this.rightExceptionCharsBox.Name = "rightExceptionCharsBox";
            this.rightExceptionCharsBox.Tag = "";
            this.toolTip1.SetToolTip(this.rightExceptionCharsBox, resources.GetString("rightExceptionCharsBox.ToolTip"));
            this.rightExceptionCharsBox.Leave += new System.EventHandler(this.rightExceptionCharsBox_Leave);
            // 
            // leftExceptionCharsBox
            // 
            resources.ApplyResources(this.leftExceptionCharsBox, "leftExceptionCharsBox");
            this.dirtyErrorProvider.SetError(this.leftExceptionCharsBox, resources.GetString("leftExceptionCharsBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.leftExceptionCharsBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("leftExceptionCharsBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.leftExceptionCharsBox, ((int)(resources.GetObject("leftExceptionCharsBox.IconPadding"))));
            this.leftExceptionCharsBox.Name = "leftExceptionCharsBox";
            this.leftExceptionCharsBox.Tag = "";
            this.toolTip1.SetToolTip(this.leftExceptionCharsBox, resources.GetString("leftExceptionCharsBox.ToolTip"));
            this.leftExceptionCharsBox.Leave += new System.EventHandler(this.leftExceptionCharsBox_Leave);
            // 
            // exceptionCharPairsCheckBoxLabel
            // 
            resources.ApplyResources(this.exceptionCharPairsCheckBoxLabel, "exceptionCharPairsCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.exceptionCharPairsCheckBoxLabel, resources.GetString("exceptionCharPairsCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.exceptionCharPairsCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("exceptionCharPairsCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.exceptionCharPairsCheckBoxLabel, ((int)(resources.GetObject("exceptionCharPairsCheckBoxLabel.IconPadding"))));
            this.exceptionCharPairsCheckBoxLabel.Name = "exceptionCharPairsCheckBoxLabel";
            this.toolTip1.SetToolTip(this.exceptionCharPairsCheckBoxLabel, resources.GetString("exceptionCharPairsCheckBoxLabel.ToolTip"));
            this.exceptionCharPairsCheckBoxLabel.Click += new System.EventHandler(this.exceptionCharPairsCheckBoxLabel_Click);
            // 
            // exceptionCharPairsCheckBox
            // 
            resources.ApplyResources(this.exceptionCharPairsCheckBox, "exceptionCharPairsCheckBox");
            this.dirtyErrorProvider.SetError(this.exceptionCharPairsCheckBox, resources.GetString("exceptionCharPairsCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.exceptionCharPairsCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("exceptionCharPairsCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.exceptionCharPairsCheckBox, ((int)(resources.GetObject("exceptionCharPairsCheckBox.IconPadding"))));
            this.exceptionCharPairsCheckBox.Name = "exceptionCharPairsCheckBox";
            this.exceptionCharPairsCheckBox.Tag = "#exceptionCharPairsCheckBoxLabel";
            this.toolTip1.SetToolTip(this.exceptionCharPairsCheckBox, resources.GetString("exceptionCharPairsCheckBox.ToolTip"));
            this.exceptionCharPairsCheckBox.CheckedChanged += new System.EventHandler(this.exceptionCharPairsCheckBox_CheckedChanged);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 1500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // ChangeCase
            // 
            this.AcceptButton = this.buttonPreview;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.previewTable);
            this.Controls.Add(this.fieldsPanel);
            this.Controls.Add(this.alwaysCapitalizeLastWordCheckBoxLabel);
            this.Controls.Add(this.alwaysCapitalizeLastWordCheckBox);
            this.Controls.Add(this.alwaysCapitalize1stWordCheckBoxLabel);
            this.Controls.Add(this.alwaysCapitalize1stWordCheckBox);
            this.Controls.Add(this.wordSeparatorsCheckBoxLabel);
            this.Controls.Add(this.wordSeparatorsCheckBox);
            this.Controls.Add(this.exceptionCharPairsCheckBoxLabel);
            this.Controls.Add(this.exceptionCharPairsCheckBox);
            this.Controls.Add(this.exceptionCharsCheckBoxLabel);
            this.Controls.Add(this.exceptionCharsCheckBox);
            this.Controls.Add(this.onlyWordsCheckBoxLabel);
            this.Controls.Add(this.onlyWordsCheckBox);
            this.Controls.Add(this.exceptionWordsCheckBoxLabel);
            this.Controls.Add(this.exceptionWordsCheckBox);
            this.Controls.Add(this.toggleCaseRadioButtonLabel);
            this.Controls.Add(this.toggleCaseRadioButton);
            this.Controls.Add(this.titleCaseRadioButtonLabel);
            this.Controls.Add(this.titleCaseRadioButton);
            this.Controls.Add(this.upperCaseRadioButtonLabel);
            this.Controls.Add(this.upperCaseRadioButton);
            this.Controls.Add(this.lowerCaseRadioButtonLabel);
            this.Controls.Add(this.lowerCaseRadioButton);
            this.Controls.Add(this.sentenceCaseRadioButtonLabel);
            this.Controls.Add(this.sentenceCaseRadioButton);
            this.Controls.Add(this.forSelectedTracksLabel);
            this.Controls.Add(this.sourceTagList);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Name = "ChangeCase";
            this.Tag = "";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChangeCase_FormClosing);
            this.Load += new System.EventHandler(this.ChangeCase_Load);
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).EndInit();
            this.fieldsPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label forSelectedTracksLabel;
        private System.Windows.Forms.ComboBox sourceTagList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton sentenceCaseRadioButton;
        private System.Windows.Forms.RadioButton lowerCaseRadioButton;
        private System.Windows.Forms.RadioButton upperCaseRadioButton;
        private System.Windows.Forms.RadioButton titleCaseRadioButton;
        private System.Windows.Forms.RadioButton toggleCaseRadioButton;
        private System.Windows.Forms.ComboBox exceptionCharsBox;
        private System.Windows.Forms.ComboBox wordSeparatorsBox;
        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.DataGridView previewTable;
        private System.Windows.Forms.Button buttonReapply;
        private System.Windows.Forms.CheckBox exceptionWordsCheckBox;
        private System.Windows.Forms.CheckBox exceptionCharsCheckBox;
        private System.Windows.Forms.CheckBox wordSeparatorsCheckBox;
        private System.Windows.Forms.CheckBox onlyWordsCheckBox;
        private System.Windows.Forms.ErrorProvider dirtyErrorProvider;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox exceptionWordsBox;
        private System.Windows.Forms.CheckBox alwaysCapitalizeLastWordCheckBox;
        private System.Windows.Forms.CheckBox alwaysCapitalize1stWordCheckBox;
        private System.Windows.Forms.Button removeExceptionButton;
        private System.Windows.Forms.Button buttonAsrExceptedWords;
        private System.Windows.Forms.Button buttonAsrExceptWordsAfterSymbols;
        private System.Windows.Forms.Button buttonAsrWordSeparators;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.Label upperCaseRadioButtonLabel;
        private System.Windows.Forms.Label lowerCaseRadioButtonLabel;
        private System.Windows.Forms.Label sentenceCaseRadioButtonLabel;
        private System.Windows.Forms.Label toggleCaseRadioButtonLabel;
        private System.Windows.Forms.Label titleCaseRadioButtonLabel;
        private System.Windows.Forms.Label exceptionWordsCheckBoxLabel;
        private System.Windows.Forms.Label onlyWordsCheckBoxLabel;
        private System.Windows.Forms.Label exceptionCharsCheckBoxLabel;
        private System.Windows.Forms.Label wordSeparatorsCheckBoxLabel;
        private System.Windows.Forms.Label alwaysCapitalize1stWordCheckBoxLabel;
        private System.Windows.Forms.Label alwaysCapitalizeLastWordCheckBoxLabel;
        private System.Windows.Forms.Panel fieldsPanel;
        private System.Windows.Forms.DataGridViewTextBoxColumn File;
        private System.Windows.Forms.DataGridViewTextBoxColumn Track;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTag;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTagT;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTag;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTagT;
        private System.Windows.Forms.Label exceptionCharPairsCheckBoxLabel;
        private System.Windows.Forms.CheckBox exceptionCharPairsCheckBox;
        private System.Windows.Forms.ComboBox leftExceptionCharsBox;
        private System.Windows.Forms.Button buttonAsrExceptWordsBetweenSymbols;
        private System.Windows.Forms.ComboBox rightExceptionCharsBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}