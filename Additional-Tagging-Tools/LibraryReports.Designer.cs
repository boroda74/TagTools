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
            this.buttonAddPreset = new System.Windows.Forms.Button();
            this.clearIdButton = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.idTextBox = new System.Windows.Forms.TextBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonCheckAll = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonUncheckAll = new System.Windows.Forms.Button();
            this.useHotkeyForSelectedTracksCheckBox = new System.Windows.Forms.CheckBox();
            this.conditionCheckBox = new System.Windows.Forms.CheckBox();
            this.assignHotkeyCheckBox = new System.Windows.Forms.CheckBox();
            this.conditionFieldList = new System.Windows.Forms.ComboBox();
            this.buttonCopyPreset = new System.Windows.Forms.Button();
            this.conditionList = new System.Windows.Forms.ComboBox();
            this.presetNameTextBox = new System.Windows.Forms.TextBox();
            this.labelSaveField = new System.Windows.Forms.Label();
            this.buttonDeletePreset = new System.Windows.Forms.Button();
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
            this.sourceTagList = new System.Windows.Forms.CheckedListBox();
            this.previewTable = new System.Windows.Forms.DataGridView();
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
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.buttonAddPreset);
            this.splitContainer1.Panel1.Controls.Add(this.clearIdButton);
            this.splitContainer1.Panel1.Controls.Add(this.buttonExport);
            this.splitContainer1.Panel1.Controls.Add(this.idTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.buttonClose);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.buttonPreview);
            this.splitContainer1.Panel1.Controls.Add(this.buttonApply);
            this.splitContainer1.Panel1.Controls.Add(this.buttonCheckAll);
            this.splitContainer1.Panel1.Controls.Add(this.buttonSave);
            this.splitContainer1.Panel1.Controls.Add(this.buttonUncheckAll);
            this.splitContainer1.Panel1.Controls.Add(this.useHotkeyForSelectedTracksCheckBox);
            this.splitContainer1.Panel1.Controls.Add(this.conditionCheckBox);
            this.splitContainer1.Panel1.Controls.Add(this.assignHotkeyCheckBox);
            this.splitContainer1.Panel1.Controls.Add(this.conditionFieldList);
            this.splitContainer1.Panel1.Controls.Add(this.buttonCopyPreset);
            this.splitContainer1.Panel1.Controls.Add(this.conditionList);
            this.splitContainer1.Panel1.Controls.Add(this.presetNameTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.labelSaveField);
            this.splitContainer1.Panel1.Controls.Add(this.buttonDeletePreset);
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
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.sourceTagList);
            this.splitContainer1.Panel2.Controls.Add(this.previewTable);
            // 
            // buttonAddPreset
            // 
            resources.ApplyResources(this.buttonAddPreset, "buttonAddPreset");
            this.buttonAddPreset.Name = "buttonAddPreset";
            this.buttonAddPreset.UseVisualStyleBackColor = true;
            this.buttonAddPreset.Click += new System.EventHandler(this.buttonAddPreset_Click);
            // 
            // clearIdButton
            // 
            resources.ApplyResources(this.clearIdButton, "clearIdButton");
            this.clearIdButton.Image = global::MusicBeePlugin.Properties.Resources.clear_button_15;
            this.clearIdButton.Name = "clearIdButton";
            this.toolTip1.SetToolTip(this.clearIdButton, resources.GetString("clearIdButton.ToolTip"));
            this.clearIdButton.UseVisualStyleBackColor = true;
            this.clearIdButton.Click += new System.EventHandler(this.clearIdButton_Click);
            // 
            // buttonExport
            // 
            resources.ApplyResources(this.buttonExport, "buttonExport");
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // idTextBox
            // 
            resources.ApplyResources(this.idTextBox, "idTextBox");
            this.idTextBox.Name = "idTextBox";
            this.toolTip1.SetToolTip(this.idTextBox, resources.GetString("idTextBox.ToolTip"));
            this.idTextBox.Leave += new System.EventHandler(this.idTextBox_Leave);
            // 
            // buttonClose
            // 
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Image = global::MusicBeePlugin.Properties.Resources.Warning_15;
            this.buttonClose.Name = "buttonClose";
            this.toolTip1.SetToolTip(this.buttonClose, resources.GetString("buttonClose.ToolTip"));
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // buttonPreview
            // 
            resources.ApplyResources(this.buttonPreview, "buttonPreview");
            this.dirtyErrorProvider.SetError(this.buttonPreview, resources.GetString("buttonPreview.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonPreview, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonPreview.IconAlignment"))));
            this.buttonPreview.Name = "buttonPreview";
            this.toolTip1.SetToolTip(this.buttonPreview, resources.GetString("buttonPreview.ToolTip"));
            this.buttonPreview.UseVisualStyleBackColor = true;
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // buttonApply
            // 
            resources.ApplyResources(this.buttonApply, "buttonApply");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonApply, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonApply.IconAlignment"))));
            this.buttonApply.Name = "buttonApply";
            this.toolTip1.SetToolTip(this.buttonApply, resources.GetString("buttonApply.ToolTip"));
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonCheckAll
            // 
            resources.ApplyResources(this.buttonCheckAll, "buttonCheckAll");
            this.buttonCheckAll.Name = "buttonCheckAll";
            this.buttonCheckAll.UseVisualStyleBackColor = true;
            this.buttonCheckAll.Click += new System.EventHandler(this.buttonCheckAll_Click);
            // 
            // buttonSave
            // 
            resources.ApplyResources(this.buttonSave, "buttonSave");
            this.buttonSave.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonUncheckAll
            // 
            resources.ApplyResources(this.buttonUncheckAll, "buttonUncheckAll");
            this.buttonUncheckAll.Name = "buttonUncheckAll";
            this.buttonUncheckAll.UseVisualStyleBackColor = true;
            this.buttonUncheckAll.Click += new System.EventHandler(this.buttonUncheckAll_Click);
            // 
            // useHotkeyForSelectedTracksCheckBox
            // 
            resources.ApplyResources(this.useHotkeyForSelectedTracksCheckBox, "useHotkeyForSelectedTracksCheckBox");
            this.useHotkeyForSelectedTracksCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.useHotkeyForSelectedTracksCheckBox.Name = "useHotkeyForSelectedTracksCheckBox";
            this.toolTip1.SetToolTip(this.useHotkeyForSelectedTracksCheckBox, resources.GetString("useHotkeyForSelectedTracksCheckBox.ToolTip"));
            this.useHotkeyForSelectedTracksCheckBox.UseVisualStyleBackColor = false;
            this.useHotkeyForSelectedTracksCheckBox.CheckedChanged += new System.EventHandler(this.useHotkeyForSelectedTracksCheckBox_CheckedChanged);
            // 
            // conditionCheckBox
            // 
            resources.ApplyResources(this.conditionCheckBox, "conditionCheckBox");
            this.conditionCheckBox.Name = "conditionCheckBox";
            this.conditionCheckBox.UseVisualStyleBackColor = true;
            this.conditionCheckBox.CheckedChanged += new System.EventHandler(this.conditionCheckBox_CheckedChanged);
            // 
            // assignHotkeyCheckBox
            // 
            resources.ApplyResources(this.assignHotkeyCheckBox, "assignHotkeyCheckBox");
            this.assignHotkeyCheckBox.Name = "assignHotkeyCheckBox";
            this.toolTip1.SetToolTip(this.assignHotkeyCheckBox, resources.GetString("assignHotkeyCheckBox.ToolTip"));
            this.assignHotkeyCheckBox.UseVisualStyleBackColor = true;
            this.assignHotkeyCheckBox.CheckedChanged += new System.EventHandler(this.assignHotkeyCheckBox_CheckedChanged);
            // 
            // conditionFieldList
            // 
            resources.ApplyResources(this.conditionFieldList, "conditionFieldList");
            this.conditionFieldList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.conditionFieldList.DropDownWidth = 250;
            this.conditionFieldList.FormattingEnabled = true;
            this.conditionFieldList.Name = "conditionFieldList";
            this.conditionFieldList.SelectedIndexChanged += new System.EventHandler(this.conditionFieldList_SelectedIndexChanged);
            // 
            // buttonCopyPreset
            // 
            resources.ApplyResources(this.buttonCopyPreset, "buttonCopyPreset");
            this.buttonCopyPreset.Name = "buttonCopyPreset";
            this.buttonCopyPreset.UseVisualStyleBackColor = true;
            this.buttonCopyPreset.Click += new System.EventHandler(this.buttonCopyPreset_Click);
            // 
            // conditionList
            // 
            resources.ApplyResources(this.conditionList, "conditionList");
            this.conditionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.conditionList.FormattingEnabled = true;
            this.conditionList.Name = "conditionList";
            this.conditionList.SelectedIndexChanged += new System.EventHandler(this.conditionList_SelectedIndexChanged);
            // 
            // presetNameTextBox
            // 
            resources.ApplyResources(this.presetNameTextBox, "presetNameTextBox");
            this.presetNameTextBox.Name = "presetNameTextBox";
            this.toolTip1.SetToolTip(this.presetNameTextBox, resources.GetString("presetNameTextBox.ToolTip"));
            this.presetNameTextBox.Leave += new System.EventHandler(this.presetNameTextBox_Leave);
            // 
            // labelSaveField
            // 
            resources.ApplyResources(this.labelSaveField, "labelSaveField");
            this.labelSaveField.Name = "labelSaveField";
            // 
            // buttonDeletePreset
            // 
            resources.ApplyResources(this.buttonDeletePreset, "buttonDeletePreset");
            this.buttonDeletePreset.Name = "buttonDeletePreset";
            this.buttonDeletePreset.UseVisualStyleBackColor = true;
            this.buttonDeletePreset.Click += new System.EventHandler(this.buttonDeletePreset_Click);
            // 
            // destinationTagList
            // 
            resources.ApplyResources(this.destinationTagList, "destinationTagList");
            this.destinationTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.destinationTagList.DropDownWidth = 250;
            this.destinationTagList.FormattingEnabled = true;
            this.destinationTagList.Name = "destinationTagList";
            this.destinationTagList.SelectedIndexChanged += new System.EventHandler(this.destinationTagList_SelectedIndexChanged);
            // 
            // comparedFieldList
            // 
            resources.ApplyResources(this.comparedFieldList, "comparedFieldList");
            this.comparedFieldList.DropDownWidth = 250;
            this.comparedFieldList.FormattingEnabled = true;
            this.comparedFieldList.Name = "comparedFieldList";
            this.comparedFieldList.TextUpdate += new System.EventHandler(this.comparedFieldList_TextUpdate);
            // 
            // presetsBox
            // 
            resources.ApplyResources(this.presetsBox, "presetsBox");
            this.presetsBox.FormattingEnabled = true;
            this.presetsBox.Name = "presetsBox";
            this.presetsBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.presetsBox_ItemCheck);
            this.presetsBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.presetsBox_MouseClick);
            this.presetsBox.SelectedIndexChanged += new System.EventHandler(this.presetsBox_SelectedIndexChanged);
            // 
            // labelSaveToTag
            // 
            resources.ApplyResources(this.labelSaveToTag, "labelSaveToTag");
            this.labelSaveToTag.Name = "labelSaveToTag";
            // 
            // sourceFieldComboBox
            // 
            resources.ApplyResources(this.sourceFieldComboBox, "sourceFieldComboBox");
            this.sourceFieldComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceFieldComboBox.DropDownWidth = 250;
            this.sourceFieldComboBox.FormattingEnabled = true;
            this.sourceFieldComboBox.Name = "sourceFieldComboBox";
            this.sourceFieldComboBox.SelectedIndexChanged += new System.EventHandler(this.sourceFieldComboBox_SelectedIndexChanged);
            // 
            // labelFunction
            // 
            resources.ApplyResources(this.labelFunction, "labelFunction");
            this.labelFunction.Name = "labelFunction";
            // 
            // functionComboBox
            // 
            resources.ApplyResources(this.functionComboBox, "functionComboBox");
            this.functionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.functionComboBox.DropDownWidth = 250;
            this.functionComboBox.Name = "functionComboBox";
            this.functionComboBox.SelectedIndexChanged += new System.EventHandler(this.functionComboBox_SelectedIndexChanged);
            // 
            // labelFormat
            // 
            resources.ApplyResources(this.labelFormat, "labelFormat");
            this.labelFormat.Name = "labelFormat";
            // 
            // totalsCheckBox
            // 
            resources.ApplyResources(this.totalsCheckBox, "totalsCheckBox");
            this.totalsCheckBox.Name = "totalsCheckBox";
            this.totalsCheckBox.UseVisualStyleBackColor = true;
            this.totalsCheckBox.CheckedChanged += new System.EventHandler(this.totalsCheckBox_CheckedChanged);
            // 
            // formatComboBox
            // 
            resources.ApplyResources(this.formatComboBox, "formatComboBox");
            this.formatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.formatComboBox.FormattingEnabled = true;
            this.formatComboBox.Name = "formatComboBox";
            this.toolTip1.SetToolTip(this.formatComboBox, resources.GetString("formatComboBox.ToolTip"));
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // openReportCheckBox
            // 
            resources.ApplyResources(this.openReportCheckBox, "openReportCheckBox");
            this.openReportCheckBox.Image = global::MusicBeePlugin.Properties.Resources.Window;
            this.openReportCheckBox.Name = "openReportCheckBox";
            this.toolTip1.SetToolTip(this.openReportCheckBox, resources.GetString("openReportCheckBox.ToolTip"));
            this.openReportCheckBox.UseVisualStyleBackColor = true;
            // 
            // parameter2ComboBox
            // 
            resources.ApplyResources(this.parameter2ComboBox, "parameter2ComboBox");
            this.parameter2ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameter2ComboBox.FormattingEnabled = true;
            this.parameter2ComboBox.Name = "parameter2ComboBox";
            this.parameter2ComboBox.SelectedIndexChanged += new System.EventHandler(this.functionComboBox_SelectedIndexChanged);
            // 
            // labelPx
            // 
            resources.ApplyResources(this.labelPx, "labelPx");
            this.labelPx.Name = "labelPx";
            // 
            // resizeArtworkCheckBox
            // 
            resources.ApplyResources(this.resizeArtworkCheckBox, "resizeArtworkCheckBox");
            this.resizeArtworkCheckBox.Name = "resizeArtworkCheckBox";
            this.resizeArtworkCheckBox.UseVisualStyleBackColor = true;
            this.resizeArtworkCheckBox.CheckedChanged += new System.EventHandler(this.resizeArtworkCheckBox_CheckedChanged);
            // 
            // yArworkSizeUpDown
            // 
            resources.ApplyResources(this.yArworkSizeUpDown, "yArworkSizeUpDown");
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
            this.labelXxY.Name = "labelXxY";
            // 
            // sourceTagList
            // 
            resources.ApplyResources(this.sourceTagList, "sourceTagList");
            this.sourceTagList.CheckOnClick = true;
            this.sourceTagList.FormattingEnabled = true;
            this.sourceTagList.Name = "sourceTagList";
            this.sourceTagList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.sourceTagList_ItemCheck);
            // 
            // previewTable
            // 
            this.previewTable.AllowUserToAddRows = false;
            this.previewTable.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.previewTable, "previewTable");
            this.previewTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.previewTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.previewTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.previewTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.previewTable.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.previewTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.previewTable.MultiSelect = false;
            this.previewTable.Name = "previewTable";
            this.previewTable.RowHeadersVisible = false;
            this.previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.previewTable.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.previewTable_ColumnHeaderMouseClick);
            this.previewTable.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.previewList_DataError);
            // 
            // autoApplyInfoLabel
            // 
            resources.ApplyResources(this.autoApplyInfoLabel, "autoApplyInfoLabel");
            this.autoApplyInfoLabel.AutoEllipsis = true;
            this.autoApplyInfoLabel.Name = "autoApplyInfoLabel";
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
            this.label1.Name = "label1";
            // 
            // numberOfTagsToRecalculateNumericUpDown
            // 
            resources.ApplyResources(this.numberOfTagsToRecalculateNumericUpDown, "numberOfTagsToRecalculateNumericUpDown");
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
            this.recalculateOnNumberOfTagsChangesCheckBox.Name = "recalculateOnNumberOfTagsChangesCheckBox";
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LibraryReportsCommand_FormClosing);
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
    }
}