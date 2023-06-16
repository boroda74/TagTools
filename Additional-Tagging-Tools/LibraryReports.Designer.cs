namespace MusicBeePlugin
{
    partial class LibraryReportsCommand
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LibraryReportsCommand));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.appendLabel = new System.Windows.Forms.Label();
            this.appendTextBox = new System.Windows.Forms.TextBox();
            this.digitsLabel = new System.Windows.Forms.Label();
            this.roundToLabel = new System.Windows.Forms.Label();
            this.precisionDigitsComboBox = new System.Windows.Forms.ComboBox();
            this.mulDivFactorComboBox = new System.Windows.Forms.ComboBox();
            this.operationComboBox = new System.Windows.Forms.ComboBox();
            this.buttonAddPreset = new System.Windows.Forms.Button();
            this.clearIdButton = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.idTextBox = new System.Windows.Forms.TextBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.useHotkeyForSelectedTracksCheckBox = new System.Windows.Forms.CheckBox();
            this.presetNameTextBox = new System.Windows.Forms.TextBox();
            this.assignHotkeyCheckBox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonDeletePreset = new System.Windows.Forms.Button();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.buttonCopyPreset = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonCheckAll = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonUncheckAll = new System.Windows.Forms.Button();
            this.conditionCheckBox = new System.Windows.Forms.CheckBox();
            this.conditionFieldList = new System.Windows.Forms.ComboBox();
            this.conditionList = new System.Windows.Forms.ComboBox();
            this.labelSaveField = new System.Windows.Forms.Label();
            this.destinationTagList = new System.Windows.Forms.ComboBox();
            this.comparedFieldList = new System.Windows.Forms.ComboBox();
            this.presetsBox = new System.Windows.Forms.CheckedListBox();
            this.labelSaveToTag = new System.Windows.Forms.Label();
            this.sourceFieldComboBox = new System.Windows.Forms.ComboBox();
            this.labelFunction = new System.Windows.Forms.Label();
            this.functionComboBox = new System.Windows.Forms.ComboBox();
            this.labelFormat = new System.Windows.Forms.Label();
            this.totalsCheckBox = new System.Windows.Forms.CheckBox();
            this.formatComboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.openReportCheckBox = new System.Windows.Forms.CheckBox();
            this.parameter2ComboBox = new System.Windows.Forms.ComboBox();
            this.labelPx = new System.Windows.Forms.Label();
            this.resizeArtworkCheckBox = new System.Windows.Forms.CheckBox();
            this.yArworkSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.xArworkSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.labelXxY = new System.Windows.Forms.Label();
            this.previewTable = new System.Windows.Forms.DataGridView();
            this.sourceTagList = new System.Windows.Forms.CheckedListBox();
            this.autoApplyInfoLabel = new System.Windows.Forms.Label();
            this.dirtyErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.numberOfTagsToRecalculateNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.recalculateOnNumberOfTagsChangesCheckBox = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.yArworkSizeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xArworkSizeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfTagsToRecalculateNumericUpDown)).BeginInit();
            this.SuspendLayout();
            //MusicBee
            this.presetNameTextBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.comparedFieldList = (System.Windows.Forms.ComboBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.ComboBox);
            this.idTextBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            //~MusicBee
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.dirtyErrorProvider.SetError(this.splitContainer1, resources.GetString("splitContainer1.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.splitContainer1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.splitContainer1, ((int)(resources.GetObject("splitContainer1.IconPadding"))));
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.Controls.Add(this.appendLabel);
            this.splitContainer1.Panel1.Controls.Add(this.appendTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.digitsLabel);
            this.splitContainer1.Panel1.Controls.Add(this.roundToLabel);
            this.splitContainer1.Panel1.Controls.Add(this.precisionDigitsComboBox);
            this.splitContainer1.Panel1.Controls.Add(this.mulDivFactorComboBox);
            this.splitContainer1.Panel1.Controls.Add(this.operationComboBox);
            this.splitContainer1.Panel1.Controls.Add(this.buttonAddPreset);
            this.splitContainer1.Panel1.Controls.Add(this.clearIdButton);
            this.splitContainer1.Panel1.Controls.Add(this.buttonExport);
            this.splitContainer1.Panel1.Controls.Add(this.idTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.buttonClose);
            this.splitContainer1.Panel1.Controls.Add(this.useHotkeyForSelectedTracksCheckBox);
            this.splitContainer1.Panel1.Controls.Add(this.presetNameTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.assignHotkeyCheckBox);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.buttonDeletePreset);
            this.splitContainer1.Panel1.Controls.Add(this.buttonPreview);
            this.splitContainer1.Panel1.Controls.Add(this.buttonCopyPreset);
            this.splitContainer1.Panel1.Controls.Add(this.buttonApply);
            this.splitContainer1.Panel1.Controls.Add(this.buttonCheckAll);
            this.splitContainer1.Panel1.Controls.Add(this.buttonSave);
            this.splitContainer1.Panel1.Controls.Add(this.buttonUncheckAll);
            this.splitContainer1.Panel1.Controls.Add(this.conditionCheckBox);
            this.splitContainer1.Panel1.Controls.Add(this.conditionFieldList);
            this.splitContainer1.Panel1.Controls.Add(this.conditionList);
            this.splitContainer1.Panel1.Controls.Add(this.labelSaveField);
            this.splitContainer1.Panel1.Controls.Add(this.destinationTagList);
            this.splitContainer1.Panel1.Controls.Add(this.comparedFieldList);
            this.splitContainer1.Panel1.Controls.Add(this.presetsBox);
            this.splitContainer1.Panel1.Controls.Add(this.labelSaveToTag);
            this.splitContainer1.Panel1.Controls.Add(this.sourceFieldComboBox);
            this.splitContainer1.Panel1.Controls.Add(this.labelFunction);
            this.splitContainer1.Panel1.Controls.Add(this.functionComboBox);
            this.splitContainer1.Panel1.Controls.Add(this.labelFormat);
            this.splitContainer1.Panel1.Controls.Add(this.totalsCheckBox);
            this.splitContainer1.Panel1.Controls.Add(this.formatComboBox);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.openReportCheckBox);
            this.splitContainer1.Panel1.Controls.Add(this.parameter2ComboBox);
            this.splitContainer1.Panel1.Controls.Add(this.labelPx);
            this.splitContainer1.Panel1.Controls.Add(this.resizeArtworkCheckBox);
            this.splitContainer1.Panel1.Controls.Add(this.yArworkSizeUpDown);
            this.splitContainer1.Panel1.Controls.Add(this.xArworkSizeUpDown);
            this.splitContainer1.Panel1.Controls.Add(this.labelXxY);
            this.dirtyErrorProvider.SetError(this.splitContainer1.Panel1, resources.GetString("splitContainer1.Panel1.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.splitContainer1.Panel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer1.Panel1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.splitContainer1.Panel1, ((int)(resources.GetObject("splitContainer1.Panel1.IconPadding"))));
            this.toolTip1.SetToolTip(this.splitContainer1.Panel1, resources.GetString("splitContainer1.Panel1.ToolTip"));
            // 
            // splitContainer1.Panel2
            // 
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.Controls.Add(this.previewTable);
            this.splitContainer1.Panel2.Controls.Add(this.sourceTagList);
            this.dirtyErrorProvider.SetError(this.splitContainer1.Panel2, resources.GetString("splitContainer1.Panel2.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.splitContainer1.Panel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer1.Panel2.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.splitContainer1.Panel2, ((int)(resources.GetObject("splitContainer1.Panel2.IconPadding"))));
            this.toolTip1.SetToolTip(this.splitContainer1.Panel2, resources.GetString("splitContainer1.Panel2.ToolTip"));
            this.toolTip1.SetToolTip(this.splitContainer1, resources.GetString("splitContainer1.ToolTip"));
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // appendLabel
            // 
            resources.ApplyResources(this.appendLabel, "appendLabel");
            this.dirtyErrorProvider.SetError(this.appendLabel, resources.GetString("appendLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.appendLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("appendLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.appendLabel, ((int)(resources.GetObject("appendLabel.IconPadding"))));
            this.appendLabel.Name = "appendLabel";
            this.toolTip1.SetToolTip(this.appendLabel, resources.GetString("appendLabel.ToolTip"));
            // 
            // appendTextBox
            // 
            resources.ApplyResources(this.appendTextBox, "appendTextBox");
            this.dirtyErrorProvider.SetError(this.appendTextBox, resources.GetString("appendTextBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.appendTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("appendTextBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.appendTextBox, ((int)(resources.GetObject("appendTextBox.IconPadding"))));
            this.appendTextBox.Name = "appendTextBox";
            this.toolTip1.SetToolTip(this.appendTextBox, resources.GetString("appendTextBox.ToolTip"));
            this.appendTextBox.TextChanged += new System.EventHandler(this.appendTextBox_TextChanged);
            // 
            // digitsLabel
            // 
            resources.ApplyResources(this.digitsLabel, "digitsLabel");
            this.dirtyErrorProvider.SetError(this.digitsLabel, resources.GetString("digitsLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.digitsLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("digitsLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.digitsLabel, ((int)(resources.GetObject("digitsLabel.IconPadding"))));
            this.digitsLabel.Name = "digitsLabel";
            this.toolTip1.SetToolTip(this.digitsLabel, resources.GetString("digitsLabel.ToolTip"));
            // 
            // roundToLabel
            // 
            resources.ApplyResources(this.roundToLabel, "roundToLabel");
            this.dirtyErrorProvider.SetError(this.roundToLabel, resources.GetString("roundToLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.roundToLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("roundToLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.roundToLabel, ((int)(resources.GetObject("roundToLabel.IconPadding"))));
            this.roundToLabel.Name = "roundToLabel";
            this.toolTip1.SetToolTip(this.roundToLabel, resources.GetString("roundToLabel.ToolTip"));
            // 
            // precisionDigitsComboBox
            // 
            resources.ApplyResources(this.precisionDigitsComboBox, "precisionDigitsComboBox");
            this.dirtyErrorProvider.SetError(this.precisionDigitsComboBox, resources.GetString("precisionDigitsComboBox.Error"));
            this.precisionDigitsComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.precisionDigitsComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("precisionDigitsComboBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.precisionDigitsComboBox, ((int)(resources.GetObject("precisionDigitsComboBox.IconPadding"))));
            this.precisionDigitsComboBox.Items.AddRange(new object[] {
            resources.GetString("precisionDigitsComboBox.Items"),
            resources.GetString("precisionDigitsComboBox.Items1"),
            resources.GetString("precisionDigitsComboBox.Items2"),
            resources.GetString("precisionDigitsComboBox.Items3"),
            resources.GetString("precisionDigitsComboBox.Items4"),
            resources.GetString("precisionDigitsComboBox.Items5")});
            this.precisionDigitsComboBox.Name = "precisionDigitsComboBox";
            this.toolTip1.SetToolTip(this.precisionDigitsComboBox, resources.GetString("precisionDigitsComboBox.ToolTip"));
            this.precisionDigitsComboBox.TextChanged += new System.EventHandler(this.precisionDigitsComboBox_TextChanged);
            // 
            // mulDivFactorComboBox
            // 
            resources.ApplyResources(this.mulDivFactorComboBox, "mulDivFactorComboBox");
            this.dirtyErrorProvider.SetError(this.mulDivFactorComboBox, resources.GetString("mulDivFactorComboBox.Error"));
            this.mulDivFactorComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.mulDivFactorComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("mulDivFactorComboBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.mulDivFactorComboBox, ((int)(resources.GetObject("mulDivFactorComboBox.IconPadding"))));
            this.mulDivFactorComboBox.Items.AddRange(new object[] {
            resources.GetString("mulDivFactorComboBox.Items"),
            resources.GetString("mulDivFactorComboBox.Items1"),
            resources.GetString("mulDivFactorComboBox.Items2"),
            resources.GetString("mulDivFactorComboBox.Items3"),
            resources.GetString("mulDivFactorComboBox.Items4"),
            resources.GetString("mulDivFactorComboBox.Items5"),
            resources.GetString("mulDivFactorComboBox.Items6")});
            this.mulDivFactorComboBox.Name = "mulDivFactorComboBox";
            this.toolTip1.SetToolTip(this.mulDivFactorComboBox, resources.GetString("mulDivFactorComboBox.ToolTip"));
            this.mulDivFactorComboBox.TextChanged += new System.EventHandler(this.mulDivFactorComboBox_TextChanged);
            // 
            // operationComboBox
            // 
            resources.ApplyResources(this.operationComboBox, "operationComboBox");
            this.operationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dirtyErrorProvider.SetError(this.operationComboBox, resources.GetString("operationComboBox.Error"));
            this.operationComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.operationComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("operationComboBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.operationComboBox, ((int)(resources.GetObject("operationComboBox.IconPadding"))));
            this.operationComboBox.Items.AddRange(new object[] {
            resources.GetString("operationComboBox.Items"),
            resources.GetString("operationComboBox.Items1")});
            this.operationComboBox.Name = "operationComboBox";
            this.toolTip1.SetToolTip(this.operationComboBox, resources.GetString("operationComboBox.ToolTip"));
            this.operationComboBox.SelectedIndexChanged += new System.EventHandler(this.operationComboBox_SelectedIndexChanged);
            // 
            // buttonAddPreset
            // 
            resources.ApplyResources(this.buttonAddPreset, "buttonAddPreset");
            this.dirtyErrorProvider.SetError(this.buttonAddPreset, resources.GetString("buttonAddPreset.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonAddPreset, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonAddPreset.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonAddPreset, ((int)(resources.GetObject("buttonAddPreset.IconPadding"))));
            this.buttonAddPreset.Name = "buttonAddPreset";
            this.toolTip1.SetToolTip(this.buttonAddPreset, resources.GetString("buttonAddPreset.ToolTip"));
            this.buttonAddPreset.UseVisualStyleBackColor = true;
            this.buttonAddPreset.Click += new System.EventHandler(this.buttonAddPreset_Click);
            // 
            // clearIdButton
            // 
            resources.ApplyResources(this.clearIdButton, "clearIdButton");
            this.dirtyErrorProvider.SetError(this.clearIdButton, resources.GetString("clearIdButton.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.clearIdButton, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("clearIdButton.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.clearIdButton, ((int)(resources.GetObject("clearIdButton.IconPadding"))));
            this.clearIdButton.Image = global::MusicBeePlugin.Properties.Resources.clear_button_15;
            this.clearIdButton.Name = "clearIdButton";
            this.toolTip1.SetToolTip(this.clearIdButton, resources.GetString("clearIdButton.ToolTip"));
            this.clearIdButton.UseVisualStyleBackColor = true;
            this.clearIdButton.Click += new System.EventHandler(this.clearIdButton_Click);
            // 
            // buttonExport
            // 
            resources.ApplyResources(this.buttonExport, "buttonExport");
            this.dirtyErrorProvider.SetError(this.buttonExport, resources.GetString("buttonExport.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonExport, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonExport.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonExport, ((int)(resources.GetObject("buttonExport.IconPadding"))));
            this.buttonExport.Name = "buttonExport";
            this.toolTip1.SetToolTip(this.buttonExport, resources.GetString("buttonExport.ToolTip"));
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // idTextBox
            // 
            resources.ApplyResources(this.idTextBox, "idTextBox");
            this.dirtyErrorProvider.SetError(this.idTextBox, resources.GetString("idTextBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.idTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("idTextBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.idTextBox, ((int)(resources.GetObject("idTextBox.IconPadding"))));
            this.idTextBox.Name = "idTextBox";
            this.toolTip1.SetToolTip(this.idTextBox, resources.GetString("idTextBox.ToolTip"));
            this.idTextBox.Leave += new System.EventHandler(this.idTextBox_Leave);
            // 
            // buttonClose
            // 
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dirtyErrorProvider.SetError(this.buttonClose, resources.GetString("buttonClose.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonClose, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonClose.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonClose, ((int)(resources.GetObject("buttonClose.IconPadding"))));
            this.buttonClose.Image = global::MusicBeePlugin.Properties.Resources.Warning_15;
            this.buttonClose.Name = "buttonClose";
            this.toolTip1.SetToolTip(this.buttonClose, resources.GetString("buttonClose.ToolTip"));
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // useHotkeyForSelectedTracksCheckBox
            // 
            resources.ApplyResources(this.useHotkeyForSelectedTracksCheckBox, "useHotkeyForSelectedTracksCheckBox");
            this.dirtyErrorProvider.SetError(this.useHotkeyForSelectedTracksCheckBox, resources.GetString("useHotkeyForSelectedTracksCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.useHotkeyForSelectedTracksCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("useHotkeyForSelectedTracksCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.useHotkeyForSelectedTracksCheckBox, ((int)(resources.GetObject("useHotkeyForSelectedTracksCheckBox.IconPadding"))));
            this.useHotkeyForSelectedTracksCheckBox.Name = "useHotkeyForSelectedTracksCheckBox";
            this.toolTip1.SetToolTip(this.useHotkeyForSelectedTracksCheckBox, resources.GetString("useHotkeyForSelectedTracksCheckBox.ToolTip"));
            this.useHotkeyForSelectedTracksCheckBox.UseVisualStyleBackColor = false;
            this.useHotkeyForSelectedTracksCheckBox.CheckedChanged += new System.EventHandler(this.useHotkeyForSelectedTracksCheckBox_CheckedChanged);
            // 
            // presetNameTextBox
            // 
            resources.ApplyResources(this.presetNameTextBox, "presetNameTextBox");
            this.dirtyErrorProvider.SetError(this.presetNameTextBox, resources.GetString("presetNameTextBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.presetNameTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("presetNameTextBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.presetNameTextBox, ((int)(resources.GetObject("presetNameTextBox.IconPadding"))));
            this.presetNameTextBox.Name = "presetNameTextBox";
            this.toolTip1.SetToolTip(this.presetNameTextBox, resources.GetString("presetNameTextBox.ToolTip"));
            this.presetNameTextBox.Leave += new System.EventHandler(this.presetNameTextBox_Leave);
            // 
            // assignHotkeyCheckBox
            // 
            resources.ApplyResources(this.assignHotkeyCheckBox, "assignHotkeyCheckBox");
            this.dirtyErrorProvider.SetError(this.assignHotkeyCheckBox, resources.GetString("assignHotkeyCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.assignHotkeyCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("assignHotkeyCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.assignHotkeyCheckBox, ((int)(resources.GetObject("assignHotkeyCheckBox.IconPadding"))));
            this.assignHotkeyCheckBox.Name = "assignHotkeyCheckBox";
            this.toolTip1.SetToolTip(this.assignHotkeyCheckBox, resources.GetString("assignHotkeyCheckBox.ToolTip"));
            this.assignHotkeyCheckBox.UseVisualStyleBackColor = true;
            this.assignHotkeyCheckBox.CheckedChanged += new System.EventHandler(this.assignHotkeyCheckBox_CheckedChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.dirtyErrorProvider.SetError(this.label3, resources.GetString("label3.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label3.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label3, ((int)(resources.GetObject("label3.IconPadding"))));
            this.label3.Name = "label3";
            this.toolTip1.SetToolTip(this.label3, resources.GetString("label3.ToolTip"));
            // 
            // buttonDeletePreset
            // 
            resources.ApplyResources(this.buttonDeletePreset, "buttonDeletePreset");
            this.dirtyErrorProvider.SetError(this.buttonDeletePreset, resources.GetString("buttonDeletePreset.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonDeletePreset, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonDeletePreset.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonDeletePreset, ((int)(resources.GetObject("buttonDeletePreset.IconPadding"))));
            this.buttonDeletePreset.Name = "buttonDeletePreset";
            this.toolTip1.SetToolTip(this.buttonDeletePreset, resources.GetString("buttonDeletePreset.ToolTip"));
            this.buttonDeletePreset.UseVisualStyleBackColor = true;
            this.buttonDeletePreset.Click += new System.EventHandler(this.buttonDeletePreset_Click);
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
            // buttonCopyPreset
            // 
            resources.ApplyResources(this.buttonCopyPreset, "buttonCopyPreset");
            this.dirtyErrorProvider.SetError(this.buttonCopyPreset, resources.GetString("buttonCopyPreset.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonCopyPreset, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonCopyPreset.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonCopyPreset, ((int)(resources.GetObject("buttonCopyPreset.IconPadding"))));
            this.buttonCopyPreset.Name = "buttonCopyPreset";
            this.toolTip1.SetToolTip(this.buttonCopyPreset, resources.GetString("buttonCopyPreset.ToolTip"));
            this.buttonCopyPreset.UseVisualStyleBackColor = true;
            this.buttonCopyPreset.Click += new System.EventHandler(this.buttonCopyPreset_Click);
            // 
            // buttonApply
            // 
            resources.ApplyResources(this.buttonApply, "buttonApply");
            this.dirtyErrorProvider.SetError(this.buttonApply, resources.GetString("buttonApply.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonApply, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonApply.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonApply, ((int)(resources.GetObject("buttonApply.IconPadding"))));
            this.buttonApply.Name = "buttonApply";
            this.toolTip1.SetToolTip(this.buttonApply, resources.GetString("buttonApply.ToolTip"));
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonCheckAll
            // 
            resources.ApplyResources(this.buttonCheckAll, "buttonCheckAll");
            this.dirtyErrorProvider.SetError(this.buttonCheckAll, resources.GetString("buttonCheckAll.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonCheckAll, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonCheckAll.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonCheckAll, ((int)(resources.GetObject("buttonCheckAll.IconPadding"))));
            this.buttonCheckAll.Name = "buttonCheckAll";
            this.toolTip1.SetToolTip(this.buttonCheckAll, resources.GetString("buttonCheckAll.ToolTip"));
            this.buttonCheckAll.UseVisualStyleBackColor = true;
            this.buttonCheckAll.Click += new System.EventHandler(this.buttonCheckAll_Click);
            // 
            // buttonSave
            // 
            resources.ApplyResources(this.buttonSave, "buttonSave");
            this.buttonSave.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dirtyErrorProvider.SetError(this.buttonSave, resources.GetString("buttonSave.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonSave, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonSave.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonSave, ((int)(resources.GetObject("buttonSave.IconPadding"))));
            this.buttonSave.Name = "buttonSave";
            this.toolTip1.SetToolTip(this.buttonSave, resources.GetString("buttonSave.ToolTip"));
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonUncheckAll
            // 
            resources.ApplyResources(this.buttonUncheckAll, "buttonUncheckAll");
            this.dirtyErrorProvider.SetError(this.buttonUncheckAll, resources.GetString("buttonUncheckAll.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonUncheckAll, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonUncheckAll.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonUncheckAll, ((int)(resources.GetObject("buttonUncheckAll.IconPadding"))));
            this.buttonUncheckAll.Name = "buttonUncheckAll";
            this.toolTip1.SetToolTip(this.buttonUncheckAll, resources.GetString("buttonUncheckAll.ToolTip"));
            this.buttonUncheckAll.UseVisualStyleBackColor = true;
            this.buttonUncheckAll.Click += new System.EventHandler(this.buttonUncheckAll_Click);
            // 
            // conditionCheckBox
            // 
            resources.ApplyResources(this.conditionCheckBox, "conditionCheckBox");
            this.dirtyErrorProvider.SetError(this.conditionCheckBox, resources.GetString("conditionCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.conditionCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("conditionCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.conditionCheckBox, ((int)(resources.GetObject("conditionCheckBox.IconPadding"))));
            this.conditionCheckBox.Name = "conditionCheckBox";
            this.toolTip1.SetToolTip(this.conditionCheckBox, resources.GetString("conditionCheckBox.ToolTip"));
            this.conditionCheckBox.UseVisualStyleBackColor = true;
            this.conditionCheckBox.CheckedChanged += new System.EventHandler(this.conditionCheckBox_CheckedChanged);
            // 
            // conditionFieldList
            // 
            resources.ApplyResources(this.conditionFieldList, "conditionFieldList");
            this.conditionFieldList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.conditionFieldList.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.conditionFieldList, resources.GetString("conditionFieldList.Error"));
            this.conditionFieldList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.conditionFieldList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("conditionFieldList.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.conditionFieldList, ((int)(resources.GetObject("conditionFieldList.IconPadding"))));
            this.conditionFieldList.Name = "conditionFieldList";
            this.toolTip1.SetToolTip(this.conditionFieldList, resources.GetString("conditionFieldList.ToolTip"));
            this.conditionFieldList.SelectedIndexChanged += new System.EventHandler(this.conditionFieldList_SelectedIndexChanged);
            // 
            // conditionList
            // 
            resources.ApplyResources(this.conditionList, "conditionList");
            this.conditionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dirtyErrorProvider.SetError(this.conditionList, resources.GetString("conditionList.Error"));
            this.conditionList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.conditionList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("conditionList.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.conditionList, ((int)(resources.GetObject("conditionList.IconPadding"))));
            this.conditionList.Name = "conditionList";
            this.toolTip1.SetToolTip(this.conditionList, resources.GetString("conditionList.ToolTip"));
            this.conditionList.SelectedIndexChanged += new System.EventHandler(this.conditionList_SelectedIndexChanged);
            // 
            // labelSaveField
            // 
            resources.ApplyResources(this.labelSaveField, "labelSaveField");
            this.dirtyErrorProvider.SetError(this.labelSaveField, resources.GetString("labelSaveField.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.labelSaveField, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelSaveField.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelSaveField, ((int)(resources.GetObject("labelSaveField.IconPadding"))));
            this.labelSaveField.Name = "labelSaveField";
            this.toolTip1.SetToolTip(this.labelSaveField, resources.GetString("labelSaveField.ToolTip"));
            // 
            // destinationTagList
            // 
            resources.ApplyResources(this.destinationTagList, "destinationTagList");
            this.destinationTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.destinationTagList.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.destinationTagList, resources.GetString("destinationTagList.Error"));
            this.destinationTagList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.destinationTagList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("destinationTagList.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.destinationTagList, ((int)(resources.GetObject("destinationTagList.IconPadding"))));
            this.destinationTagList.Name = "destinationTagList";
            this.toolTip1.SetToolTip(this.destinationTagList, resources.GetString("destinationTagList.ToolTip"));
            this.destinationTagList.SelectedIndexChanged += new System.EventHandler(this.destinationTagList_SelectedIndexChanged);
            // 
            // comparedFieldList
            // 
            resources.ApplyResources(this.comparedFieldList, "comparedFieldList");
            this.comparedFieldList.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.comparedFieldList, resources.GetString("comparedFieldList.Error"));
            this.comparedFieldList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.comparedFieldList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("comparedFieldList.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.comparedFieldList, ((int)(resources.GetObject("comparedFieldList.IconPadding"))));
            this.comparedFieldList.Name = "comparedFieldList";
            this.toolTip1.SetToolTip(this.comparedFieldList, resources.GetString("comparedFieldList.ToolTip"));
            this.comparedFieldList.TextUpdate += new System.EventHandler(this.comparedFieldList_TextUpdate);
            // 
            // presetsBox
            // 
            resources.ApplyResources(this.presetsBox, "presetsBox");
            this.dirtyErrorProvider.SetError(this.presetsBox, resources.GetString("presetsBox.Error"));
            this.presetsBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.presetsBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("presetsBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.presetsBox, ((int)(resources.GetObject("presetsBox.IconPadding"))));
            this.presetsBox.Name = "presetsBox";
            this.toolTip1.SetToolTip(this.presetsBox, resources.GetString("presetsBox.ToolTip"));
            this.presetsBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.presetsBox_ItemCheck);
            this.presetsBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.presetsBox_MouseClick);
            this.presetsBox.SelectedIndexChanged += new System.EventHandler(this.presetsBox_SelectedIndexChanged);
            // 
            // labelSaveToTag
            // 
            resources.ApplyResources(this.labelSaveToTag, "labelSaveToTag");
            this.dirtyErrorProvider.SetError(this.labelSaveToTag, resources.GetString("labelSaveToTag.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.labelSaveToTag, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelSaveToTag.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelSaveToTag, ((int)(resources.GetObject("labelSaveToTag.IconPadding"))));
            this.labelSaveToTag.Name = "labelSaveToTag";
            this.toolTip1.SetToolTip(this.labelSaveToTag, resources.GetString("labelSaveToTag.ToolTip"));
            // 
            // sourceFieldComboBox
            // 
            resources.ApplyResources(this.sourceFieldComboBox, "sourceFieldComboBox");
            this.sourceFieldComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceFieldComboBox.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.sourceFieldComboBox, resources.GetString("sourceFieldComboBox.Error"));
            this.sourceFieldComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.sourceFieldComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("sourceFieldComboBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.sourceFieldComboBox, ((int)(resources.GetObject("sourceFieldComboBox.IconPadding"))));
            this.sourceFieldComboBox.Name = "sourceFieldComboBox";
            this.toolTip1.SetToolTip(this.sourceFieldComboBox, resources.GetString("sourceFieldComboBox.ToolTip"));
            this.sourceFieldComboBox.SelectedIndexChanged += new System.EventHandler(this.sourceFieldComboBox_SelectedIndexChanged);
            // 
            // labelFunction
            // 
            resources.ApplyResources(this.labelFunction, "labelFunction");
            this.dirtyErrorProvider.SetError(this.labelFunction, resources.GetString("labelFunction.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.labelFunction, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelFunction.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelFunction, ((int)(resources.GetObject("labelFunction.IconPadding"))));
            this.labelFunction.Name = "labelFunction";
            this.toolTip1.SetToolTip(this.labelFunction, resources.GetString("labelFunction.ToolTip"));
            // 
            // functionComboBox
            // 
            resources.ApplyResources(this.functionComboBox, "functionComboBox");
            this.functionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.functionComboBox.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.functionComboBox, resources.GetString("functionComboBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.functionComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("functionComboBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.functionComboBox, ((int)(resources.GetObject("functionComboBox.IconPadding"))));
            this.functionComboBox.Name = "functionComboBox";
            this.toolTip1.SetToolTip(this.functionComboBox, resources.GetString("functionComboBox.ToolTip"));
            this.functionComboBox.SelectedIndexChanged += new System.EventHandler(this.functionComboBox_SelectedIndexChanged);
            // 
            // labelFormat
            // 
            resources.ApplyResources(this.labelFormat, "labelFormat");
            this.dirtyErrorProvider.SetError(this.labelFormat, resources.GetString("labelFormat.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.labelFormat, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelFormat.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelFormat, ((int)(resources.GetObject("labelFormat.IconPadding"))));
            this.labelFormat.Name = "labelFormat";
            this.toolTip1.SetToolTip(this.labelFormat, resources.GetString("labelFormat.ToolTip"));
            // 
            // totalsCheckBox
            // 
            resources.ApplyResources(this.totalsCheckBox, "totalsCheckBox");
            this.dirtyErrorProvider.SetError(this.totalsCheckBox, resources.GetString("totalsCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.totalsCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("totalsCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.totalsCheckBox, ((int)(resources.GetObject("totalsCheckBox.IconPadding"))));
            this.totalsCheckBox.Name = "totalsCheckBox";
            this.toolTip1.SetToolTip(this.totalsCheckBox, resources.GetString("totalsCheckBox.ToolTip"));
            this.totalsCheckBox.UseVisualStyleBackColor = true;
            this.totalsCheckBox.CheckedChanged += new System.EventHandler(this.totalsCheckBox_CheckedChanged);
            // 
            // formatComboBox
            // 
            resources.ApplyResources(this.formatComboBox, "formatComboBox");
            this.formatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dirtyErrorProvider.SetError(this.formatComboBox, resources.GetString("formatComboBox.Error"));
            this.formatComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.formatComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("formatComboBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.formatComboBox, ((int)(resources.GetObject("formatComboBox.IconPadding"))));
            this.formatComboBox.Name = "formatComboBox";
            this.toolTip1.SetToolTip(this.formatComboBox, resources.GetString("formatComboBox.ToolTip"));
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
            // openReportCheckBox
            // 
            resources.ApplyResources(this.openReportCheckBox, "openReportCheckBox");
            this.dirtyErrorProvider.SetError(this.openReportCheckBox, resources.GetString("openReportCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.openReportCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("openReportCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.openReportCheckBox, ((int)(resources.GetObject("openReportCheckBox.IconPadding"))));
            this.openReportCheckBox.Image = global::MusicBeePlugin.Properties.Resources.Window;
            this.openReportCheckBox.Name = "openReportCheckBox";
            this.toolTip1.SetToolTip(this.openReportCheckBox, resources.GetString("openReportCheckBox.ToolTip"));
            this.openReportCheckBox.UseVisualStyleBackColor = true;
            // 
            // parameter2ComboBox
            // 
            resources.ApplyResources(this.parameter2ComboBox, "parameter2ComboBox");
            this.parameter2ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dirtyErrorProvider.SetError(this.parameter2ComboBox, resources.GetString("parameter2ComboBox.Error"));
            this.parameter2ComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.parameter2ComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("parameter2ComboBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.parameter2ComboBox, ((int)(resources.GetObject("parameter2ComboBox.IconPadding"))));
            this.parameter2ComboBox.Name = "parameter2ComboBox";
            this.toolTip1.SetToolTip(this.parameter2ComboBox, resources.GetString("parameter2ComboBox.ToolTip"));
            this.parameter2ComboBox.SelectedIndexChanged += new System.EventHandler(this.functionComboBox_SelectedIndexChanged);
            // 
            // labelPx
            // 
            resources.ApplyResources(this.labelPx, "labelPx");
            this.dirtyErrorProvider.SetError(this.labelPx, resources.GetString("labelPx.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.labelPx, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelPx.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelPx, ((int)(resources.GetObject("labelPx.IconPadding"))));
            this.labelPx.Name = "labelPx";
            this.toolTip1.SetToolTip(this.labelPx, resources.GetString("labelPx.ToolTip"));
            // 
            // resizeArtworkCheckBox
            // 
            resources.ApplyResources(this.resizeArtworkCheckBox, "resizeArtworkCheckBox");
            this.dirtyErrorProvider.SetError(this.resizeArtworkCheckBox, resources.GetString("resizeArtworkCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.resizeArtworkCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("resizeArtworkCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.resizeArtworkCheckBox, ((int)(resources.GetObject("resizeArtworkCheckBox.IconPadding"))));
            this.resizeArtworkCheckBox.Name = "resizeArtworkCheckBox";
            this.toolTip1.SetToolTip(this.resizeArtworkCheckBox, resources.GetString("resizeArtworkCheckBox.ToolTip"));
            this.resizeArtworkCheckBox.UseVisualStyleBackColor = true;
            this.resizeArtworkCheckBox.CheckedChanged += new System.EventHandler(this.resizeArtworkCheckBox_CheckedChanged);
            // 
            // yArworkSizeUpDown
            // 
            resources.ApplyResources(this.yArworkSizeUpDown, "yArworkSizeUpDown");
            this.dirtyErrorProvider.SetError(this.yArworkSizeUpDown, resources.GetString("yArworkSizeUpDown.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.yArworkSizeUpDown, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("yArworkSizeUpDown.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.yArworkSizeUpDown, ((int)(resources.GetObject("yArworkSizeUpDown.IconPadding"))));
            this.yArworkSizeUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.yArworkSizeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.yArworkSizeUpDown.Name = "yArworkSizeUpDown";
            this.toolTip1.SetToolTip(this.yArworkSizeUpDown, resources.GetString("yArworkSizeUpDown.ToolTip"));
            this.yArworkSizeUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.yArworkSizeUpDown.ValueChanged += new System.EventHandler(this.yArworkSizeUpDown_ValueChanged);
            // 
            // xArworkSizeUpDown
            // 
            resources.ApplyResources(this.xArworkSizeUpDown, "xArworkSizeUpDown");
            this.dirtyErrorProvider.SetError(this.xArworkSizeUpDown, resources.GetString("xArworkSizeUpDown.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.xArworkSizeUpDown, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("xArworkSizeUpDown.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.xArworkSizeUpDown, ((int)(resources.GetObject("xArworkSizeUpDown.IconPadding"))));
            this.xArworkSizeUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.xArworkSizeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.xArworkSizeUpDown.Name = "xArworkSizeUpDown";
            this.toolTip1.SetToolTip(this.xArworkSizeUpDown, resources.GetString("xArworkSizeUpDown.ToolTip"));
            this.xArworkSizeUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.xArworkSizeUpDown.ValueChanged += new System.EventHandler(this.xArworkSizeUpDown_ValueChanged);
            // 
            // labelXxY
            // 
            resources.ApplyResources(this.labelXxY, "labelXxY");
            this.dirtyErrorProvider.SetError(this.labelXxY, resources.GetString("labelXxY.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.labelXxY, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelXxY.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelXxY, ((int)(resources.GetObject("labelXxY.IconPadding"))));
            this.labelXxY.Name = "labelXxY";
            this.toolTip1.SetToolTip(this.labelXxY, resources.GetString("labelXxY.ToolTip"));
            // 
            // previewTable
            // 
            resources.ApplyResources(this.previewTable, "previewTable");
            this.previewTable.AllowUserToAddRows = false;
            this.previewTable.AllowUserToDeleteRows = false;
            this.previewTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.previewTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.previewTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.previewTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.previewTable.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.previewTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dirtyErrorProvider.SetError(this.previewTable, resources.GetString("previewTable.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.previewTable, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("previewTable.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.previewTable, ((int)(resources.GetObject("previewTable.IconPadding"))));
            this.previewTable.MultiSelect = false;
            this.previewTable.Name = "previewTable";
            this.previewTable.RowHeadersVisible = false;
            this.previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.toolTip1.SetToolTip(this.previewTable, resources.GetString("previewTable.ToolTip"));
            this.previewTable.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.previewTable_ColumnHeaderMouseClick);
            this.previewTable.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.previewList_DataError);
            // 
            // sourceTagList
            // 
            resources.ApplyResources(this.sourceTagList, "sourceTagList");
            this.sourceTagList.CheckOnClick = true;
            this.dirtyErrorProvider.SetError(this.sourceTagList, resources.GetString("sourceTagList.Error"));
            this.sourceTagList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.sourceTagList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("sourceTagList.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.sourceTagList, ((int)(resources.GetObject("sourceTagList.IconPadding"))));
            this.sourceTagList.Name = "sourceTagList";
            this.toolTip1.SetToolTip(this.sourceTagList, resources.GetString("sourceTagList.ToolTip"));
            this.sourceTagList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.sourceTagList_ItemCheck);
            // 
            // autoApplyInfoLabel
            // 
            resources.ApplyResources(this.autoApplyInfoLabel, "autoApplyInfoLabel");
            this.autoApplyInfoLabel.AutoEllipsis = true;
            this.dirtyErrorProvider.SetError(this.autoApplyInfoLabel, resources.GetString("autoApplyInfoLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.autoApplyInfoLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("autoApplyInfoLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.autoApplyInfoLabel, ((int)(resources.GetObject("autoApplyInfoLabel.IconPadding"))));
            this.autoApplyInfoLabel.Name = "autoApplyInfoLabel";
            this.toolTip1.SetToolTip(this.autoApplyInfoLabel, resources.GetString("autoApplyInfoLabel.ToolTip"));
            // 
            // dirtyErrorProvider
            // 
            this.dirtyErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.dirtyErrorProvider.ContainerControl = this;
            resources.ApplyResources(this.dirtyErrorProvider, "dirtyErrorProvider");
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
            // numberOfTagsToRecalculateNumericUpDown
            // 
            resources.ApplyResources(this.numberOfTagsToRecalculateNumericUpDown, "numberOfTagsToRecalculateNumericUpDown");
            this.dirtyErrorProvider.SetError(this.numberOfTagsToRecalculateNumericUpDown, resources.GetString("numberOfTagsToRecalculateNumericUpDown.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.numberOfTagsToRecalculateNumericUpDown, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("numberOfTagsToRecalculateNumericUpDown.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.numberOfTagsToRecalculateNumericUpDown, ((int)(resources.GetObject("numberOfTagsToRecalculateNumericUpDown.IconPadding"))));
            this.numberOfTagsToRecalculateNumericUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numberOfTagsToRecalculateNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numberOfTagsToRecalculateNumericUpDown.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numberOfTagsToRecalculateNumericUpDown.Name = "numberOfTagsToRecalculateNumericUpDown";
            this.toolTip1.SetToolTip(this.numberOfTagsToRecalculateNumericUpDown, resources.GetString("numberOfTagsToRecalculateNumericUpDown.ToolTip"));
            this.numberOfTagsToRecalculateNumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numberOfTagsToRecalculateNumericUpDown.ValueChanged += new System.EventHandler(this.numberOfTagsToRecalculateNumericUpDown_ValueChanged);
            // 
            // recalculateOnNumberOfTagsChangesCheckBox
            // 
            resources.ApplyResources(this.recalculateOnNumberOfTagsChangesCheckBox, "recalculateOnNumberOfTagsChangesCheckBox");
            this.dirtyErrorProvider.SetError(this.recalculateOnNumberOfTagsChangesCheckBox, resources.GetString("recalculateOnNumberOfTagsChangesCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.recalculateOnNumberOfTagsChangesCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("recalculateOnNumberOfTagsChangesCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.recalculateOnNumberOfTagsChangesCheckBox, ((int)(resources.GetObject("recalculateOnNumberOfTagsChangesCheckBox.IconPadding"))));
            this.recalculateOnNumberOfTagsChangesCheckBox.Name = "recalculateOnNumberOfTagsChangesCheckBox";
            this.toolTip1.SetToolTip(this.recalculateOnNumberOfTagsChangesCheckBox, resources.GetString("recalculateOnNumberOfTagsChangesCheckBox.ToolTip"));
            this.recalculateOnNumberOfTagsChangesCheckBox.UseVisualStyleBackColor = true;
            this.recalculateOnNumberOfTagsChangesCheckBox.CheckedChanged += new System.EventHandler(this.recalculateOnNumberOfTagsChangesCheckBox_CheckedChanged);
            // 
            // LibraryReportsCommand
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numberOfTagsToRecalculateNumericUpDown);
            this.Controls.Add(this.recalculateOnNumberOfTagsChangesCheckBox);
            this.Controls.Add(this.autoApplyInfoLabel);
            this.Name = "LibraryReportsCommand";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LibraryReportsCommand_FormClosing);
            this.Load += new System.EventHandler(this.LibraryReportsCommand_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.yArworkSizeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xArworkSizeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfTagsToRecalculateNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Label autoApplyInfoLabel;
        private System.Windows.Forms.Button buttonCheckAll;
        private System.Windows.Forms.Button buttonUncheckAll;
        private System.Windows.Forms.CheckBox conditionCheckBox;
        private System.Windows.Forms.ComboBox conditionFieldList;
        private System.Windows.Forms.ComboBox conditionList;
        private System.Windows.Forms.Label labelSaveField;
        private System.Windows.Forms.ComboBox destinationTagList;
        private System.Windows.Forms.ErrorProvider dirtyErrorProvider;
        private System.Windows.Forms.ComboBox comparedFieldList;
        private System.Windows.Forms.ComboBox sourceFieldComboBox;
        private System.Windows.Forms.Label labelSaveToTag;
        private System.Windows.Forms.ComboBox functionComboBox;
        private System.Windows.Forms.Label labelFunction;
        private System.Windows.Forms.CheckBox totalsCheckBox;
        private System.Windows.Forms.ComboBox parameter2ComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelPx;
        private System.Windows.Forms.NumericUpDown yArworkSizeUpDown;
        private System.Windows.Forms.Label labelXxY;
        private System.Windows.Forms.NumericUpDown xArworkSizeUpDown;
        private System.Windows.Forms.CheckBox resizeArtworkCheckBox;
        private System.Windows.Forms.CheckBox openReportCheckBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox formatComboBox;
        private System.Windows.Forms.Label labelFormat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numberOfTagsToRecalculateNumericUpDown;
        private System.Windows.Forms.CheckBox recalculateOnNumberOfTagsChangesCheckBox;
        private System.Windows.Forms.CheckedListBox presetsBox;
        private System.Windows.Forms.Button buttonCopyPreset;
        private System.Windows.Forms.TextBox presetNameTextBox;
        private System.Windows.Forms.Button buttonDeletePreset;
        private System.Windows.Forms.Button buttonAddPreset;
        private System.Windows.Forms.CheckBox useHotkeyForSelectedTracksCheckBox;
        private System.Windows.Forms.CheckBox assignHotkeyCheckBox;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button clearIdButton;
        private System.Windows.Forms.TextBox idTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckedListBox sourceTagList;
        private System.Windows.Forms.DataGridView previewTable;
        private System.Windows.Forms.ComboBox operationComboBox;
        private System.Windows.Forms.ComboBox mulDivFactorComboBox;
        private System.Windows.Forms.Label roundToLabel;
        private System.Windows.Forms.ComboBox precisionDigitsComboBox;
        private System.Windows.Forms.Label digitsLabel;
        private System.Windows.Forms.TextBox appendTextBox;
        private System.Windows.Forms.Label appendLabel;
    }
}