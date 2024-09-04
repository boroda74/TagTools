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

            if (disposing)
                source?.Dispose();
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
            this.buttonClose = new System.Windows.Forms.Button();
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
            this.sentenceSeparatorsBox = new System.Windows.Forms.ComboBox();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.previewTable = new System.Windows.Forms.DataGridView();
            this.buttonReapply = new System.Windows.Forms.Button();
            this.exceptionWordsCheckBox = new System.Windows.Forms.CheckBox();
            this.exceptionCharsCheckBox = new System.Windows.Forms.CheckBox();
            this.sentenceSeparatorsCheckBox = new System.Windows.Forms.CheckBox();
            this.dirtyErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.buttonSettings = new System.Windows.Forms.Button();
            this.exceptionWordsBox = new System.Windows.Forms.ComboBox();
            this.alwaysCapitalize1stWordCheckBox = new System.Windows.Forms.CheckBox();
            this.alwaysCapitalizeLastWordCheckBox = new System.Windows.Forms.CheckBox();
            this.removeExceptionButton = new System.Windows.Forms.Button();
            this.buttonAsrExceptedWords = new System.Windows.Forms.Button();
            this.buttonAsrExceptWordsAfterSymbols = new System.Windows.Forms.Button();
            this.buttonAsrSentenceSeparators = new System.Windows.Forms.Button();
            this.sentenceCaseRadioButtonLabel = new System.Windows.Forms.Label();
            this.lowerCaseRadioButtonLabel = new System.Windows.Forms.Label();
            this.upperCaseRadioButtonLabel = new System.Windows.Forms.Label();
            this.titleCaseRadioButtonLabel = new System.Windows.Forms.Label();
            this.toggleCaseRadioButtonLabel = new System.Windows.Forms.Label();
            this.exceptionWordsCheckBoxLabel = new System.Windows.Forms.Label();
            this.exceptionCharsCheckBoxLabel = new System.Windows.Forms.Label();
            this.sentenceSeparatorsCheckBoxLabel = new System.Windows.Forms.Label();
            this.alwaysCapitalize1stWordCheckBoxLabel = new System.Windows.Forms.Label();
            this.alwaysCapitalizeLastWordCheckBoxLabel = new System.Windows.Forms.Label();
            this.fieldsPanel = new System.Windows.Forms.Panel();
            this.buttonAsrExceptWordsBetweenSymbols = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.closingExceptionCharsBox = new System.Windows.Forms.ComboBox();
            this.openingExceptionCharsBox = new System.Windows.Forms.ComboBox();
            this.exceptionCharPairsCheckBoxLabel = new System.Windows.Forms.Label();
            this.exceptionCharPairsCheckBox = new System.Windows.Forms.CheckBox();
            this.ignoreSingleLetterExceptedWordsCheckBoxLabel = new System.Windows.Forms.Label();
            this.ignoreSingleLetterExceptedWordsCheckBox = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.File = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Track = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalTagValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalTagValueNormalized = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewTagValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewTagValueNormalized = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).BeginInit();
            this.fieldsPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Tag = "#fieldsPanel@pinned-to-parent-x@non-defaultable";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Tag = "#buttonClose";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // forSelectedTracksLabel
            // 
            resources.ApplyResources(this.forSelectedTracksLabel, "forSelectedTracksLabel");
            this.forSelectedTracksLabel.Name = "forSelectedTracksLabel";
            // 
            // sourceTagList
            // 
            this.sourceTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceTagList.DropDownWidth = 250;
            this.sourceTagList.FormattingEnabled = true;
            resources.ApplyResources(this.sourceTagList, "sourceTagList");
            this.sourceTagList.Name = "sourceTagList";
            this.sourceTagList.Tag = "#forSelectedTracksLabel";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.label1.Tag = "#sourceTagList@pinned-to-parent-x";
            // 
            // sentenceCaseRadioButton
            // 
            resources.ApplyResources(this.sentenceCaseRadioButton, "sentenceCaseRadioButton");
            this.sentenceCaseRadioButton.Name = "sentenceCaseRadioButton";
            this.sentenceCaseRadioButton.TabStop = true;
            this.sentenceCaseRadioButton.Tag = "#sentenceCaseRadioButtonLabel@pinned-to-parent-x";
            this.sentenceCaseRadioButton.CheckedChanged += new System.EventHandler(this.casingRuleRadioButton_CheckedChanged);
            // 
            // lowerCaseRadioButton
            // 
            resources.ApplyResources(this.lowerCaseRadioButton, "lowerCaseRadioButton");
            this.lowerCaseRadioButton.Name = "lowerCaseRadioButton";
            this.lowerCaseRadioButton.Tag = "#lowerCaseRadioButtonLabel@pinned-to-parent-x";
            this.lowerCaseRadioButton.CheckedChanged += new System.EventHandler(this.casingRuleRadioButton_CheckedChanged);
            // 
            // upperCaseRadioButton
            // 
            resources.ApplyResources(this.upperCaseRadioButton, "upperCaseRadioButton");
            this.upperCaseRadioButton.Name = "upperCaseRadioButton";
            this.upperCaseRadioButton.Tag = "#upperCaseRadioButtonLabel@pinned-to-parent-x";
            this.upperCaseRadioButton.CheckedChanged += new System.EventHandler(this.casingRuleRadioButton_CheckedChanged);
            // 
            // titleCaseRadioButton
            // 
            resources.ApplyResources(this.titleCaseRadioButton, "titleCaseRadioButton");
            this.titleCaseRadioButton.Name = "titleCaseRadioButton";
            this.titleCaseRadioButton.Tag = "#titleCaseRadioButtonLabel@pinned-to-parent-x";
            this.titleCaseRadioButton.CheckedChanged += new System.EventHandler(this.casingRuleRadioButton_CheckedChanged);
            // 
            // toggleCaseRadioButton
            // 
            resources.ApplyResources(this.toggleCaseRadioButton, "toggleCaseRadioButton");
            this.toggleCaseRadioButton.Name = "toggleCaseRadioButton";
            this.toggleCaseRadioButton.Tag = "#toggleCaseRadioButtonLabel@pinned-to-parent-x";
            this.toggleCaseRadioButton.CheckedChanged += new System.EventHandler(this.casingRuleRadioButton_CheckedChanged);
            // 
            // exceptionCharsBox
            // 
            resources.ApplyResources(this.exceptionCharsBox, "exceptionCharsBox");
            this.exceptionCharsBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.exceptionCharsBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.exceptionCharsBox.FormattingEnabled = true;
            this.exceptionCharsBox.Name = "exceptionCharsBox";
            this.exceptionCharsBox.Tag = "#buttonAsrExceptWordsAfterSymbols@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.exceptionCharsBox, resources.GetString("exceptionCharsBox.ToolTip"));
            this.exceptionCharsBox.Leave += new System.EventHandler(this.exceptionCharsBox_Leave);
            // 
            // sentenceSeparatorsBox
            // 
            resources.ApplyResources(this.sentenceSeparatorsBox, "sentenceSeparatorsBox");
            this.sentenceSeparatorsBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.sentenceSeparatorsBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.sentenceSeparatorsBox.FormattingEnabled = true;
            this.sentenceSeparatorsBox.Name = "sentenceSeparatorsBox";
            this.sentenceSeparatorsBox.Tag = "#buttonAsrSentenceSeparators@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.sentenceSeparatorsBox, resources.GetString("sentenceSeparatorsBox.ToolTip"));
            this.sentenceSeparatorsBox.Leave += new System.EventHandler(this.sentenceSeparatorsBox_Leave);
            // 
            // buttonPreview
            // 
            resources.ApplyResources(this.buttonPreview, "buttonPreview");
            this.dirtyErrorProvider.SetError(this.buttonPreview, resources.GetString("buttonPreview.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonPreview, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonPreview.IconAlignment"))));
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Tag = "#buttonOK";
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
            this.previewTable.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.previewTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.File,
            this.Track,
            this.OriginalTagValue,
            this.OriginalTagValueNormalized,
            this.NewTagValue,
            this.NewTagValueNormalized});
            this.previewTable.MultiSelect = false;
            this.previewTable.Name = "previewTable";
            this.previewTable.RowHeadersVisible = false;
            this.previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.previewTable.Tag = "#ChangeCase&ChangeCase@pinned-to-parent-x";
            this.previewTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.previewTable_CellContentClick);
            // 
            // buttonReapply
            // 
            resources.ApplyResources(this.buttonReapply, "buttonReapply");
            this.buttonReapply.Name = "buttonReapply";
            this.buttonReapply.Tag = "#buttonSettings@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.buttonReapply, resources.GetString("buttonReapply.ToolTip"));
            this.buttonReapply.Click += new System.EventHandler(this.buttonReapply_Click);
            // 
            // exceptionWordsCheckBox
            // 
            resources.ApplyResources(this.exceptionWordsCheckBox, "exceptionWordsCheckBox");
            this.exceptionWordsCheckBox.Name = "exceptionWordsCheckBox";
            this.exceptionWordsCheckBox.Tag = "#exceptionWordsCheckBoxLabel";
            this.exceptionWordsCheckBox.ThreeState = true;
            this.toolTip1.SetToolTip(this.exceptionWordsCheckBox, resources.GetString("exceptionWordsCheckBox.ToolTip"));
            this.exceptionWordsCheckBox.CheckStateChanged += new System.EventHandler(this.exceptWordsCheckBox_CheckStateChanged);
            // 
            // exceptionCharsCheckBox
            // 
            resources.ApplyResources(this.exceptionCharsCheckBox, "exceptionCharsCheckBox");
            this.exceptionCharsCheckBox.Name = "exceptionCharsCheckBox";
            this.exceptionCharsCheckBox.Tag = "#exceptionCharsCheckBoxLabel";
            this.toolTip1.SetToolTip(this.exceptionCharsCheckBox, resources.GetString("exceptionCharsCheckBox.ToolTip"));
            this.exceptionCharsCheckBox.CheckedChanged += new System.EventHandler(this.exceptCharsCheckBox_CheckedChanged);
            // 
            // sentenceSeparatorsCheckBox
            // 
            resources.ApplyResources(this.sentenceSeparatorsCheckBox, "sentenceSeparatorsCheckBox");
            this.sentenceSeparatorsCheckBox.Name = "sentenceSeparatorsCheckBox";
            this.sentenceSeparatorsCheckBox.Tag = "#sentenceSeparatorsCheckBoxLabel";
            this.toolTip1.SetToolTip(this.sentenceSeparatorsCheckBox, resources.GetString("sentenceSeparatorsCheckBox.ToolTip"));
            this.sentenceSeparatorsCheckBox.CheckedChanged += new System.EventHandler(this.sentenceSeparatorsCheckBox_CheckedChanged);
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
            this.dirtyErrorProvider.SetIconAlignment(this.buttonSettings, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonSettings.IconAlignment"))));
            this.buttonSettings.Image = global::MusicBeePlugin.Properties.Resources.gear_15;
            resources.ApplyResources(this.buttonSettings, "buttonSettings");
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
            this.exceptionWordsBox.FormattingEnabled = true;
            this.exceptionWordsBox.Name = "exceptionWordsBox";
            this.exceptionWordsBox.Tag = "#removeExceptionButton@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.exceptionWordsBox, resources.GetString("exceptionWordsBox.ToolTip"));
            this.exceptionWordsBox.Leave += new System.EventHandler(this.exceptionWordsBox_Leave);
            // 
            // alwaysCapitalize1stWordCheckBox
            // 
            resources.ApplyResources(this.alwaysCapitalize1stWordCheckBox, "alwaysCapitalize1stWordCheckBox");
            this.alwaysCapitalize1stWordCheckBox.Name = "alwaysCapitalize1stWordCheckBox";
            this.alwaysCapitalize1stWordCheckBox.Tag = "#alwaysCapitalize1stWordCheckBoxLabel";
            this.alwaysCapitalize1stWordCheckBox.ThreeState = true;
            this.toolTip1.SetToolTip(this.alwaysCapitalize1stWordCheckBox, resources.GetString("alwaysCapitalize1stWordCheckBox.ToolTip"));
            // 
            // alwaysCapitalizeLastWordCheckBox
            // 
            resources.ApplyResources(this.alwaysCapitalizeLastWordCheckBox, "alwaysCapitalizeLastWordCheckBox");
            this.alwaysCapitalizeLastWordCheckBox.Name = "alwaysCapitalizeLastWordCheckBox";
            this.alwaysCapitalizeLastWordCheckBox.Tag = "#alwaysCapitalizeLastWordCheckBoxLabel";
            this.alwaysCapitalizeLastWordCheckBox.ThreeState = true;
            this.toolTip1.SetToolTip(this.alwaysCapitalizeLastWordCheckBox, resources.GetString("alwaysCapitalizeLastWordCheckBox.ToolTip"));
            // 
            // removeExceptionButton
            // 
            resources.ApplyResources(this.removeExceptionButton, "removeExceptionButton");
            this.removeExceptionButton.Name = "removeExceptionButton";
            this.removeExceptionButton.Tag = "#fieldsPanel@pinned-to-parent-x@non-defaultable";
            this.toolTip1.SetToolTip(this.removeExceptionButton, resources.GetString("removeExceptionButton.ToolTip"));
            this.removeExceptionButton.Click += new System.EventHandler(this.removeExceptionButton_Click);
            // 
            // buttonAsrExceptedWords
            // 
            resources.ApplyResources(this.buttonAsrExceptedWords, "buttonAsrExceptedWords");
            this.buttonAsrExceptedWords.Name = "buttonAsrExceptedWords";
            this.buttonAsrExceptedWords.Tag = "#fieldsPanel@pinned-to-parent-x@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonAsrExceptedWords, resources.GetString("buttonAsrExceptedWords.ToolTip"));
            this.buttonAsrExceptedWords.Click += new System.EventHandler(this.buttonAsr_Click);
            // 
            // buttonAsrExceptWordsAfterSymbols
            // 
            resources.ApplyResources(this.buttonAsrExceptWordsAfterSymbols, "buttonAsrExceptWordsAfterSymbols");
            this.buttonAsrExceptWordsAfterSymbols.Name = "buttonAsrExceptWordsAfterSymbols";
            this.buttonAsrExceptWordsAfterSymbols.Tag = "#fieldsPanel@pinned-to-parentl@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonAsrExceptWordsAfterSymbols, resources.GetString("buttonAsrExceptWordsAfterSymbols.ToolTip"));
            this.buttonAsrExceptWordsAfterSymbols.Click += new System.EventHandler(this.buttonAsrExceptWordsAfterSymbols_Click);
            // 
            // buttonAsrSentenceSeparators
            // 
            resources.ApplyResources(this.buttonAsrSentenceSeparators, "buttonAsrSentenceSeparators");
            this.buttonAsrSentenceSeparators.Name = "buttonAsrSentenceSeparators";
            this.buttonAsrSentenceSeparators.Tag = "#fieldsPanel@pinned-to-parent-x@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonAsrSentenceSeparators, resources.GetString("buttonAsrSentenceSeparators.ToolTip"));
            this.buttonAsrSentenceSeparators.Click += new System.EventHandler(this.buttonAsrSentenceSeparators_Click);
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
            this.toggleCaseRadioButtonLabel.Tag = "#&previewTable";
            this.toggleCaseRadioButtonLabel.Click += new System.EventHandler(this.toggleCaseRadioButtonLabel_Click);
            // 
            // exceptionWordsCheckBoxLabel
            // 
            resources.ApplyResources(this.exceptionWordsCheckBoxLabel, "exceptionWordsCheckBoxLabel");
            this.exceptionWordsCheckBoxLabel.Name = "exceptionWordsCheckBoxLabel";
            this.exceptionWordsCheckBoxLabel.Tag = "";
            this.toolTip1.SetToolTip(this.exceptionWordsCheckBoxLabel, resources.GetString("exceptionWordsCheckBoxLabel.ToolTip"));
            this.exceptionWordsCheckBoxLabel.Click += new System.EventHandler(this.exceptionWordsCheckBoxLabel_Click);
            // 
            // exceptionCharsCheckBoxLabel
            // 
            resources.ApplyResources(this.exceptionCharsCheckBoxLabel, "exceptionCharsCheckBoxLabel");
            this.exceptionCharsCheckBoxLabel.Name = "exceptionCharsCheckBoxLabel";
            this.toolTip1.SetToolTip(this.exceptionCharsCheckBoxLabel, resources.GetString("exceptionCharsCheckBoxLabel.ToolTip"));
            this.exceptionCharsCheckBoxLabel.Click += new System.EventHandler(this.exceptionCharsCheckBoxLabel_Click);
            // 
            // sentenceSeparatorsCheckBoxLabel
            // 
            resources.ApplyResources(this.sentenceSeparatorsCheckBoxLabel, "sentenceSeparatorsCheckBoxLabel");
            this.sentenceSeparatorsCheckBoxLabel.Name = "sentenceSeparatorsCheckBoxLabel";
            this.sentenceSeparatorsCheckBoxLabel.Tag = "#fieldsPanel";
            this.toolTip1.SetToolTip(this.sentenceSeparatorsCheckBoxLabel, resources.GetString("sentenceSeparatorsCheckBoxLabel.ToolTip"));
            this.sentenceSeparatorsCheckBoxLabel.Click += new System.EventHandler(this.sentenceSeparatorsCheckBoxLabel_Click);
            // 
            // alwaysCapitalize1stWordCheckBoxLabel
            // 
            resources.ApplyResources(this.alwaysCapitalize1stWordCheckBoxLabel, "alwaysCapitalize1stWordCheckBoxLabel");
            this.alwaysCapitalize1stWordCheckBoxLabel.Name = "alwaysCapitalize1stWordCheckBoxLabel";
            this.alwaysCapitalize1stWordCheckBoxLabel.Tag = "#alwaysCapitalizeLastWordCheckBox";
            this.toolTip1.SetToolTip(this.alwaysCapitalize1stWordCheckBoxLabel, resources.GetString("alwaysCapitalize1stWordCheckBoxLabel.ToolTip"));
            this.alwaysCapitalize1stWordCheckBoxLabel.Click += new System.EventHandler(this.alwaysCapitalize1stWordCheckBoxLabel_Click);
            // 
            // alwaysCapitalizeLastWordCheckBoxLabel
            // 
            resources.ApplyResources(this.alwaysCapitalizeLastWordCheckBoxLabel, "alwaysCapitalizeLastWordCheckBoxLabel");
            this.alwaysCapitalizeLastWordCheckBoxLabel.Name = "alwaysCapitalizeLastWordCheckBoxLabel";
            this.alwaysCapitalizeLastWordCheckBoxLabel.Tag = "#ignoreSingleLetterExceptedWordsCheckBox";
            this.toolTip1.SetToolTip(this.alwaysCapitalizeLastWordCheckBoxLabel, resources.GetString("alwaysCapitalizeLastWordCheckBoxLabel.ToolTip"));
            this.alwaysCapitalizeLastWordCheckBoxLabel.Click += new System.EventHandler(this.alwaysCapitalizeLastWordCheckBoxLabel_Click);
            // 
            // fieldsPanel
            // 
            resources.ApplyResources(this.fieldsPanel, "fieldsPanel");
            this.fieldsPanel.Controls.Add(this.buttonAsrSentenceSeparators);
            this.fieldsPanel.Controls.Add(this.sentenceSeparatorsBox);
            this.fieldsPanel.Controls.Add(this.buttonAsrExceptWordsBetweenSymbols);
            this.fieldsPanel.Controls.Add(this.tableLayoutPanel1);
            this.fieldsPanel.Controls.Add(this.buttonAsrExceptWordsAfterSymbols);
            this.fieldsPanel.Controls.Add(this.exceptionCharsBox);
            this.fieldsPanel.Controls.Add(this.removeExceptionButton);
            this.fieldsPanel.Controls.Add(this.exceptionWordsBox);
            this.fieldsPanel.Controls.Add(this.buttonClose);
            this.fieldsPanel.Controls.Add(this.buttonAsrExceptedWords);
            this.fieldsPanel.Controls.Add(this.buttonOK);
            this.fieldsPanel.Controls.Add(this.buttonPreview);
            this.fieldsPanel.Controls.Add(this.buttonSettings);
            this.fieldsPanel.Controls.Add(this.buttonReapply);
            this.fieldsPanel.Name = "fieldsPanel";
            this.fieldsPanel.Tag = "#ChangeCase";
            // 
            // buttonAsrExceptWordsBetweenSymbols
            // 
            resources.ApplyResources(this.buttonAsrExceptWordsBetweenSymbols, "buttonAsrExceptWordsBetweenSymbols");
            this.buttonAsrExceptWordsBetweenSymbols.Name = "buttonAsrExceptWordsBetweenSymbols";
            this.buttonAsrExceptWordsBetweenSymbols.Tag = "#fieldsPanel@pinned-to-parentl@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonAsrExceptWordsBetweenSymbols, resources.GetString("buttonAsrExceptWordsBetweenSymbols.ToolTip"));
            this.buttonAsrExceptWordsBetweenSymbols.Click += new System.EventHandler(this.buttonAsrExceptWordsBetweenSymbols_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.closingExceptionCharsBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.openingExceptionCharsBox, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Tag = "#buttonAsrExceptWordsBetweenSymbols@pinned-to-parent-x";
            // 
            // closingExceptionCharsBox
            // 
            resources.ApplyResources(this.closingExceptionCharsBox, "closingExceptionCharsBox");
            this.closingExceptionCharsBox.Name = "closingExceptionCharsBox";
            this.closingExceptionCharsBox.Tag = "";
            this.toolTip1.SetToolTip(this.closingExceptionCharsBox, resources.GetString("closingExceptionCharsBox.ToolTip"));
            this.closingExceptionCharsBox.Leave += new System.EventHandler(this.closingExceptionCharsBox_Leave);
            // 
            // openingExceptionCharsBox
            // 
            resources.ApplyResources(this.openingExceptionCharsBox, "openingExceptionCharsBox");
            this.openingExceptionCharsBox.Name = "openingExceptionCharsBox";
            this.openingExceptionCharsBox.Tag = "";
            this.toolTip1.SetToolTip(this.openingExceptionCharsBox, resources.GetString("openingExceptionCharsBox.ToolTip"));
            this.openingExceptionCharsBox.Leave += new System.EventHandler(this.openingExceptionCharsBox_Leave);
            // 
            // exceptionCharPairsCheckBoxLabel
            // 
            resources.ApplyResources(this.exceptionCharPairsCheckBoxLabel, "exceptionCharPairsCheckBoxLabel");
            this.exceptionCharPairsCheckBoxLabel.Name = "exceptionCharPairsCheckBoxLabel";
            this.toolTip1.SetToolTip(this.exceptionCharPairsCheckBoxLabel, resources.GetString("exceptionCharPairsCheckBoxLabel.ToolTip"));
            this.exceptionCharPairsCheckBoxLabel.Click += new System.EventHandler(this.exceptionCharPairsCheckBoxLabel_Click);
            // 
            // exceptionCharPairsCheckBox
            // 
            resources.ApplyResources(this.exceptionCharPairsCheckBox, "exceptionCharPairsCheckBox");
            this.exceptionCharPairsCheckBox.Name = "exceptionCharPairsCheckBox";
            this.exceptionCharPairsCheckBox.Tag = "#exceptionCharPairsCheckBoxLabel";
            this.toolTip1.SetToolTip(this.exceptionCharPairsCheckBox, resources.GetString("exceptionCharPairsCheckBox.ToolTip"));
            this.exceptionCharPairsCheckBox.CheckedChanged += new System.EventHandler(this.exceptionCharPairsCheckBox_CheckedChanged);
            // 
            // ignoreSingleLetterExceptedWordsCheckBoxLabel
            // 
            resources.ApplyResources(this.ignoreSingleLetterExceptedWordsCheckBoxLabel, "ignoreSingleLetterExceptedWordsCheckBoxLabel");
            this.ignoreSingleLetterExceptedWordsCheckBoxLabel.Name = "ignoreSingleLetterExceptedWordsCheckBoxLabel";
            this.toolTip1.SetToolTip(this.ignoreSingleLetterExceptedWordsCheckBoxLabel, resources.GetString("ignoreSingleLetterExceptedWordsCheckBoxLabel.ToolTip"));
            this.ignoreSingleLetterExceptedWordsCheckBoxLabel.Click += new System.EventHandler(this.ignoreSingleLetterExceptedWordsCheckBoxLabel_Click);
            // 
            // ignoreSingleLetterExceptedWordsCheckBox
            // 
            resources.ApplyResources(this.ignoreSingleLetterExceptedWordsCheckBox, "ignoreSingleLetterExceptedWordsCheckBox");
            this.ignoreSingleLetterExceptedWordsCheckBox.Name = "ignoreSingleLetterExceptedWordsCheckBox";
            this.ignoreSingleLetterExceptedWordsCheckBox.Tag = "#ignoreSingleLetterExceptedWordsCheckBoxLabel";
            this.ignoreSingleLetterExceptedWordsCheckBox.ThreeState = true;
            this.toolTip1.SetToolTip(this.ignoreSingleLetterExceptedWordsCheckBox, resources.GetString("ignoreSingleLetterExceptedWordsCheckBox.ToolTip"));
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 1500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // File
            // 
            this.File.DataPropertyName = "File";
            this.File.FillWeight = 1F;
            resources.ApplyResources(this.File, "File");
            this.File.Name = "File";
            // 
            // Track
            // 
            this.Track.DataPropertyName = "Track";
            this.Track.FillWeight = 200F;
            resources.ApplyResources(this.Track, "Track");
            this.Track.Name = "Track";
            this.Track.ReadOnly = true;
            // 
            // OriginalTagValue
            // 
            this.OriginalTagValue.DataPropertyName = "OriginalTagValue";
            this.OriginalTagValue.FillWeight = 1F;
            resources.ApplyResources(this.OriginalTagValue, "OriginalTagValue");
            this.OriginalTagValue.Name = "OriginalTagValue";
            // 
            // OriginalTagValueNormalized
            // 
            this.OriginalTagValueNormalized.DataPropertyName = "OriginalTagValueNormalized";
            resources.ApplyResources(this.OriginalTagValueNormalized, "OriginalTagValueNormalized");
            this.OriginalTagValueNormalized.Name = "OriginalTagValueNormalized";
            this.OriginalTagValueNormalized.ReadOnly = true;
            // 
            // NewTagValue
            // 
            this.NewTagValue.DataPropertyName = "NewTagValue";
            this.NewTagValue.FillWeight = 1F;
            resources.ApplyResources(this.NewTagValue, "NewTagValue");
            this.NewTagValue.Name = "NewTagValue";
            // 
            // NewTagValueNormalized
            // 
            this.NewTagValueNormalized.DataPropertyName = "NewTagValueNormalized";
            resources.ApplyResources(this.NewTagValueNormalized, "NewTagValueNormalized");
            this.NewTagValueNormalized.Name = "NewTagValueNormalized";
            // 
            // ChangeCase
            // 
            this.AcceptButton = this.buttonPreview;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ignoreSingleLetterExceptedWordsCheckBoxLabel);
            this.Controls.Add(this.ignoreSingleLetterExceptedWordsCheckBox);
            this.Controls.Add(this.previewTable);
            this.Controls.Add(this.fieldsPanel);
            this.Controls.Add(this.alwaysCapitalizeLastWordCheckBoxLabel);
            this.Controls.Add(this.alwaysCapitalizeLastWordCheckBox);
            this.Controls.Add(this.alwaysCapitalize1stWordCheckBoxLabel);
            this.Controls.Add(this.alwaysCapitalize1stWordCheckBox);
            this.Controls.Add(this.sentenceSeparatorsCheckBoxLabel);
            this.Controls.Add(this.sentenceSeparatorsCheckBox);
            this.Controls.Add(this.exceptionCharPairsCheckBoxLabel);
            this.Controls.Add(this.exceptionCharPairsCheckBox);
            this.Controls.Add(this.exceptionCharsCheckBoxLabel);
            this.Controls.Add(this.exceptionCharsCheckBox);
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

        private System.Windows.Forms.Button buttonClose;
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
        private System.Windows.Forms.ComboBox sentenceSeparatorsBox;
        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.DataGridView previewTable;
        private System.Windows.Forms.Button buttonReapply;
        private System.Windows.Forms.CheckBox exceptionWordsCheckBox;
        private System.Windows.Forms.CheckBox exceptionCharsCheckBox;
        private System.Windows.Forms.CheckBox sentenceSeparatorsCheckBox;
        private System.Windows.Forms.ErrorProvider dirtyErrorProvider;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox exceptionWordsBox;
        private System.Windows.Forms.CheckBox alwaysCapitalizeLastWordCheckBox;
        private System.Windows.Forms.CheckBox alwaysCapitalize1stWordCheckBox;
        private System.Windows.Forms.Button removeExceptionButton;
        private System.Windows.Forms.Button buttonAsrExceptedWords;
        private System.Windows.Forms.Button buttonAsrExceptWordsAfterSymbols;
        private System.Windows.Forms.Button buttonAsrSentenceSeparators;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.Label upperCaseRadioButtonLabel;
        private System.Windows.Forms.Label lowerCaseRadioButtonLabel;
        private System.Windows.Forms.Label sentenceCaseRadioButtonLabel;
        private System.Windows.Forms.Label toggleCaseRadioButtonLabel;
        private System.Windows.Forms.Label titleCaseRadioButtonLabel;
        private System.Windows.Forms.Label exceptionWordsCheckBoxLabel;
        private System.Windows.Forms.Label exceptionCharsCheckBoxLabel;
        private System.Windows.Forms.Label sentenceSeparatorsCheckBoxLabel;
        private System.Windows.Forms.Label alwaysCapitalize1stWordCheckBoxLabel;
        private System.Windows.Forms.Label alwaysCapitalizeLastWordCheckBoxLabel;
        private System.Windows.Forms.Panel fieldsPanel;
        private System.Windows.Forms.Label exceptionCharPairsCheckBoxLabel;
        private System.Windows.Forms.CheckBox exceptionCharPairsCheckBox;
        private System.Windows.Forms.ComboBox openingExceptionCharsBox;
        private System.Windows.Forms.Button buttonAsrExceptWordsBetweenSymbols;
        private System.Windows.Forms.ComboBox closingExceptionCharsBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label ignoreSingleLetterExceptedWordsCheckBoxLabel;
        private System.Windows.Forms.CheckBox ignoreSingleLetterExceptedWordsCheckBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn File;
        private System.Windows.Forms.DataGridViewTextBoxColumn Track;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTagValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTagValueNormalized;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTagValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTagValueNormalized;
    }
}