namespace MusicBeePlugin
{
    partial class AutoLibraryReportCommand
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoLibraryReportCommand));
            this.sourceTagList = new System.Windows.Forms.CheckedListBox();
            this.destinationTagList = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.conditionList = new System.Windows.Forms.ComboBox();
            this.conditionFieldList = new System.Windows.Forms.ComboBox();
            this.conditionCheckBox = new System.Windows.Forms.CheckBox();
            this.buttonUncheckAll = new System.Windows.Forms.Button();
            this.buttonCheckAll = new System.Windows.Forms.Button();
            this.previewTable = new System.Windows.Forms.DataGridView();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comparedFieldList = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.sourceFieldComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.functionComboBox = new System.Windows.Forms.ComboBox();
            this.dirtyErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.parameter2ComboBox = new System.Windows.Forms.ComboBox();
            this.presetsBox = new System.Windows.Forms.CheckedListBox();
            this.recalculateOnNumberOfTagsChangesCheckBox = new System.Windows.Forms.CheckBox();
            this.numberOfTagsToRecalculateNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonAddPreset = new System.Windows.Forms.Button();
            this.buttonUpdatePreset = new System.Windows.Forms.Button();
            this.buttonDeletePreset = new System.Windows.Forms.Button();
            this.labelNotSaved = new System.Windows.Forms.Label();
            this.clearIdButton = new System.Windows.Forms.Button();
            this.idTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfTagsToRecalculateNumericUpDown)).BeginInit();
            this.SuspendLayout();
            //MusicBee
            this.comparedFieldList = (System.Windows.Forms.ComboBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.ComboBox);
            this.idTextBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            //~MusicBee
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
            this.toolTip.SetToolTip(this.sourceTagList, resources.GetString("sourceTagList.ToolTip"));
            this.sourceTagList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.sourceTagList_ItemCheck);
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
            this.toolTip.SetToolTip(this.destinationTagList, resources.GetString("destinationTagList.ToolTip"));
            this.destinationTagList.SelectedIndexChanged += new System.EventHandler(this.destinationTagList_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.dirtyErrorProvider.SetError(this.label2, resources.GetString("label2.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label2, ((int)(resources.GetObject("label2.IconPadding"))));
            this.label2.Name = "label2";
            this.toolTip.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
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
            this.toolTip.SetToolTip(this.conditionList, resources.GetString("conditionList.ToolTip"));
            this.conditionList.SelectedIndexChanged += new System.EventHandler(this.conditionList_SelectedIndexChanged);
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
            this.toolTip.SetToolTip(this.conditionFieldList, resources.GetString("conditionFieldList.ToolTip"));
            this.conditionFieldList.SelectedIndexChanged += new System.EventHandler(this.conditionFieldList_SelectedIndexChanged);
            // 
            // conditionCheckBox
            // 
            resources.ApplyResources(this.conditionCheckBox, "conditionCheckBox");
            this.dirtyErrorProvider.SetError(this.conditionCheckBox, resources.GetString("conditionCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.conditionCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("conditionCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.conditionCheckBox, ((int)(resources.GetObject("conditionCheckBox.IconPadding"))));
            this.conditionCheckBox.Name = "conditionCheckBox";
            this.toolTip.SetToolTip(this.conditionCheckBox, resources.GetString("conditionCheckBox.ToolTip"));
            this.conditionCheckBox.UseVisualStyleBackColor = true;
            this.conditionCheckBox.CheckedChanged += new System.EventHandler(this.checkBoxCondition_CheckedChanged);
            // 
            // buttonUncheckAll
            // 
            resources.ApplyResources(this.buttonUncheckAll, "buttonUncheckAll");
            this.dirtyErrorProvider.SetError(this.buttonUncheckAll, resources.GetString("buttonUncheckAll.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonUncheckAll, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonUncheckAll.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonUncheckAll, ((int)(resources.GetObject("buttonUncheckAll.IconPadding"))));
            this.buttonUncheckAll.Name = "buttonUncheckAll";
            this.toolTip.SetToolTip(this.buttonUncheckAll, resources.GetString("buttonUncheckAll.ToolTip"));
            this.buttonUncheckAll.UseVisualStyleBackColor = true;
            this.buttonUncheckAll.Click += new System.EventHandler(this.buttonUncheckAll_Click);
            // 
            // buttonCheckAll
            // 
            resources.ApplyResources(this.buttonCheckAll, "buttonCheckAll");
            this.dirtyErrorProvider.SetError(this.buttonCheckAll, resources.GetString("buttonCheckAll.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonCheckAll, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonCheckAll.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonCheckAll, ((int)(resources.GetObject("buttonCheckAll.IconPadding"))));
            this.buttonCheckAll.Name = "buttonCheckAll";
            this.toolTip.SetToolTip(this.buttonCheckAll, resources.GetString("buttonCheckAll.ToolTip"));
            this.buttonCheckAll.UseVisualStyleBackColor = true;
            this.buttonCheckAll.Click += new System.EventHandler(this.buttonCheckAll_Click);
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
            this.toolTip.SetToolTip(this.previewTable, resources.GetString("previewTable.ToolTip"));
            this.previewTable.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.previewTable_ColumnHeaderMouseClick);
            this.previewTable.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.previewList_DataError);
            // 
            // buttonPreview
            // 
            resources.ApplyResources(this.buttonPreview, "buttonPreview");
            this.dirtyErrorProvider.SetError(this.buttonPreview, resources.GetString("buttonPreview.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonPreview, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonPreview.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonPreview, ((int)(resources.GetObject("buttonPreview.IconPadding"))));
            this.buttonPreview.Name = "buttonPreview";
            this.toolTip.SetToolTip(this.buttonPreview, resources.GetString("buttonPreview.ToolTip"));
            this.buttonPreview.UseVisualStyleBackColor = true;
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dirtyErrorProvider.SetError(this.buttonCancel, resources.GetString("buttonCancel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonCancel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonCancel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonCancel, ((int)(resources.GetObject("buttonCancel.IconPadding"))));
            this.buttonCancel.Name = "buttonCancel";
            this.toolTip.SetToolTip(this.buttonCancel, resources.GetString("buttonCancel.ToolTip"));
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.dirtyErrorProvider.SetError(this.label1, resources.GetString("label1.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            this.toolTip.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
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
            this.toolTip.SetToolTip(this.comparedFieldList, resources.GetString("comparedFieldList.ToolTip"));
            this.comparedFieldList.TextUpdate += new System.EventHandler(this.comparedFieldList_TextUpdate);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.dirtyErrorProvider.SetError(this.label4, resources.GetString("label4.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label4.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label4, ((int)(resources.GetObject("label4.IconPadding"))));
            this.label4.Name = "label4";
            this.toolTip.SetToolTip(this.label4, resources.GetString("label4.ToolTip"));
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
            this.toolTip.SetToolTip(this.sourceFieldComboBox, resources.GetString("sourceFieldComboBox.ToolTip"));
            this.sourceFieldComboBox.SelectedIndexChanged += new System.EventHandler(this.sourceFieldComboBox_SelectedIndexChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.dirtyErrorProvider.SetError(this.label5, resources.GetString("label5.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label5.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label5, ((int)(resources.GetObject("label5.IconPadding"))));
            this.label5.Name = "label5";
            this.toolTip.SetToolTip(this.label5, resources.GetString("label5.ToolTip"));
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
            this.toolTip.SetToolTip(this.functionComboBox, resources.GetString("functionComboBox.ToolTip"));
            this.functionComboBox.SelectedIndexChanged += new System.EventHandler(this.functionComboBox_SelectedIndexChanged);
            // 
            // dirtyErrorProvider
            // 
            this.dirtyErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.dirtyErrorProvider.ContainerControl = this;
            resources.ApplyResources(this.dirtyErrorProvider, "dirtyErrorProvider");
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.dirtyErrorProvider.SetError(this.label6, resources.GetString("label6.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label6.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label6, ((int)(resources.GetObject("label6.IconPadding"))));
            this.label6.Name = "label6";
            this.toolTip.SetToolTip(this.label6, resources.GetString("label6.ToolTip"));
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
            this.toolTip.SetToolTip(this.parameter2ComboBox, resources.GetString("parameter2ComboBox.ToolTip"));
            this.parameter2ComboBox.SelectedIndexChanged += new System.EventHandler(this.functionComboBox_SelectedIndexChanged);
            // 
            // presetsBox
            // 
            resources.ApplyResources(this.presetsBox, "presetsBox");
            this.dirtyErrorProvider.SetError(this.presetsBox, resources.GetString("presetsBox.Error"));
            this.presetsBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.presetsBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("presetsBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.presetsBox, ((int)(resources.GetObject("presetsBox.IconPadding"))));
            this.presetsBox.Name = "presetsBox";
            this.toolTip.SetToolTip(this.presetsBox, resources.GetString("presetsBox.ToolTip"));
            this.presetsBox.SelectedIndexChanged += new System.EventHandler(this.presetsBox_SelectedIndexChanged);
            // 
            // recalculateOnNumberOfTagsChangesCheckBox
            // 
            resources.ApplyResources(this.recalculateOnNumberOfTagsChangesCheckBox, "recalculateOnNumberOfTagsChangesCheckBox");
            this.dirtyErrorProvider.SetError(this.recalculateOnNumberOfTagsChangesCheckBox, resources.GetString("recalculateOnNumberOfTagsChangesCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.recalculateOnNumberOfTagsChangesCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("recalculateOnNumberOfTagsChangesCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.recalculateOnNumberOfTagsChangesCheckBox, ((int)(resources.GetObject("recalculateOnNumberOfTagsChangesCheckBox.IconPadding"))));
            this.recalculateOnNumberOfTagsChangesCheckBox.Name = "recalculateOnNumberOfTagsChangesCheckBox";
            this.toolTip.SetToolTip(this.recalculateOnNumberOfTagsChangesCheckBox, resources.GetString("recalculateOnNumberOfTagsChangesCheckBox.ToolTip"));
            this.recalculateOnNumberOfTagsChangesCheckBox.UseVisualStyleBackColor = true;
            this.recalculateOnNumberOfTagsChangesCheckBox.CheckedChanged += new System.EventHandler(this.recalculateOnNumberOfTagsChangesCheckBox_CheckedChanged);
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
            this.toolTip.SetToolTip(this.numberOfTagsToRecalculateNumericUpDown, resources.GetString("numberOfTagsToRecalculateNumericUpDown.ToolTip"));
            this.numberOfTagsToRecalculateNumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.dirtyErrorProvider.SetError(this.label7, resources.GetString("label7.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label7, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label7.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label7, ((int)(resources.GetObject("label7.IconPadding"))));
            this.label7.Name = "label7";
            this.toolTip.SetToolTip(this.label7, resources.GetString("label7.ToolTip"));
            // 
            // buttonAddPreset
            // 
            resources.ApplyResources(this.buttonAddPreset, "buttonAddPreset");
            this.dirtyErrorProvider.SetError(this.buttonAddPreset, resources.GetString("buttonAddPreset.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonAddPreset, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonAddPreset.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonAddPreset, ((int)(resources.GetObject("buttonAddPreset.IconPadding"))));
            this.buttonAddPreset.Name = "buttonAddPreset";
            this.toolTip.SetToolTip(this.buttonAddPreset, resources.GetString("buttonAddPreset.ToolTip"));
            this.buttonAddPreset.UseVisualStyleBackColor = true;
            this.buttonAddPreset.Click += new System.EventHandler(this.buttonAddPreset_Click);
            // 
            // buttonUpdatePreset
            // 
            resources.ApplyResources(this.buttonUpdatePreset, "buttonUpdatePreset");
            this.dirtyErrorProvider.SetError(this.buttonUpdatePreset, resources.GetString("buttonUpdatePreset.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonUpdatePreset, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonUpdatePreset.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonUpdatePreset, ((int)(resources.GetObject("buttonUpdatePreset.IconPadding"))));
            this.buttonUpdatePreset.Name = "buttonUpdatePreset";
            this.toolTip.SetToolTip(this.buttonUpdatePreset, resources.GetString("buttonUpdatePreset.ToolTip"));
            this.buttonUpdatePreset.UseVisualStyleBackColor = true;
            this.buttonUpdatePreset.Click += new System.EventHandler(this.buttonUpdatePreset_Click);
            // 
            // buttonDeletePreset
            // 
            resources.ApplyResources(this.buttonDeletePreset, "buttonDeletePreset");
            this.dirtyErrorProvider.SetError(this.buttonDeletePreset, resources.GetString("buttonDeletePreset.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonDeletePreset, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonDeletePreset.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonDeletePreset, ((int)(resources.GetObject("buttonDeletePreset.IconPadding"))));
            this.buttonDeletePreset.Name = "buttonDeletePreset";
            this.toolTip.SetToolTip(this.buttonDeletePreset, resources.GetString("buttonDeletePreset.ToolTip"));
            this.buttonDeletePreset.UseVisualStyleBackColor = true;
            this.buttonDeletePreset.Click += new System.EventHandler(this.buttonDeletePreset_Click);
            // 
            // labelNotSaved
            // 
            resources.ApplyResources(this.labelNotSaved, "labelNotSaved");
            this.dirtyErrorProvider.SetError(this.labelNotSaved, resources.GetString("labelNotSaved.Error"));
            this.labelNotSaved.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.dirtyErrorProvider.SetIconAlignment(this.labelNotSaved, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelNotSaved.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelNotSaved, ((int)(resources.GetObject("labelNotSaved.IconPadding"))));
            this.labelNotSaved.Name = "labelNotSaved";
            this.toolTip.SetToolTip(this.labelNotSaved, resources.GetString("labelNotSaved.ToolTip"));
            // 
            // clearIdButton
            // 
            resources.ApplyResources(this.clearIdButton, "clearIdButton");
            this.dirtyErrorProvider.SetError(this.clearIdButton, resources.GetString("clearIdButton.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.clearIdButton, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("clearIdButton.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.clearIdButton, ((int)(resources.GetObject("clearIdButton.IconPadding"))));
            this.clearIdButton.Image = global::MusicBeePlugin.Properties.Resources.clear_button;
            this.clearIdButton.Name = "clearIdButton";
            this.toolTip.SetToolTip(this.clearIdButton, resources.GetString("clearIdButton.ToolTip"));
            this.clearIdButton.UseVisualStyleBackColor = true;
            this.clearIdButton.Click += new System.EventHandler(this.clearIdButton_Click);
            // 
            // idTextBox
            // 
            resources.ApplyResources(this.idTextBox, "idTextBox");
            this.dirtyErrorProvider.SetError(this.idTextBox, resources.GetString("idTextBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.idTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("idTextBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.idTextBox, ((int)(resources.GetObject("idTextBox.IconPadding"))));
            this.idTextBox.Name = "idTextBox";
            this.toolTip.SetToolTip(this.idTextBox, resources.GetString("idTextBox.ToolTip"));
            this.idTextBox.Leave += new System.EventHandler(this.idTextBox_Leave);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.dirtyErrorProvider.SetError(this.label8, resources.GetString("label8.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label8, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label8.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label8, ((int)(resources.GetObject("label8.IconPadding"))));
            this.label8.Name = "label8";
            this.toolTip.SetToolTip(this.label8, resources.GetString("label8.ToolTip"));
            // 
            // AutoLibraryReportCommand
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.clearIdButton);
            this.Controls.Add(this.idTextBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.labelNotSaved);
            this.Controls.Add(this.buttonDeletePreset);
            this.Controls.Add(this.buttonUpdatePreset);
            this.Controls.Add(this.buttonAddPreset);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.numberOfTagsToRecalculateNumericUpDown);
            this.Controls.Add(this.recalculateOnNumberOfTagsChangesCheckBox);
            this.Controls.Add(this.presetsBox);
            this.Controls.Add(this.parameter2ComboBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.functionComboBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.sourceFieldComboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comparedFieldList);
            this.Controls.Add(this.destinationTagList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.conditionList);
            this.Controls.Add(this.conditionFieldList);
            this.Controls.Add(this.conditionCheckBox);
            this.Controls.Add(this.buttonUncheckAll);
            this.Controls.Add(this.sourceTagList);
            this.Controls.Add(this.buttonCheckAll);
            this.Controls.Add(this.previewTable);
            this.Controls.Add(this.buttonPreview);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.label1);
            this.Name = "AutoLibraryReportCommand";
            this.toolTip.SetToolTip(this, resources.GetString("$this.ToolTip"));
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfTagsToRecalculateNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView previewTable;
        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCheckAll;
        private System.Windows.Forms.CheckedListBox sourceTagList;
        private System.Windows.Forms.Button buttonUncheckAll;
        private System.Windows.Forms.CheckBox conditionCheckBox;
        private System.Windows.Forms.ComboBox conditionFieldList;
        private System.Windows.Forms.ComboBox conditionList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox destinationTagList;
        private System.Windows.Forms.ErrorProvider dirtyErrorProvider;
        private System.Windows.Forms.ComboBox comparedFieldList;
        private System.Windows.Forms.ComboBox sourceFieldComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox functionComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox parameter2ComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckedListBox presetsBox;
        private System.Windows.Forms.CheckBox recalculateOnNumberOfTagsChangesCheckBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numberOfTagsToRecalculateNumericUpDown;
        private System.Windows.Forms.Button buttonDeletePreset;
        private System.Windows.Forms.Button buttonUpdatePreset;
        private System.Windows.Forms.Button buttonAddPreset;
        private System.Windows.Forms.Label labelNotSaved;
        private System.Windows.Forms.Button clearIdButton;
        private System.Windows.Forms.TextBox idTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ToolTip toolTip;
    }
}