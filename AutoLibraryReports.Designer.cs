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
            this.buttonIdOK = new System.Windows.Forms.Button();
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
            this.sourceTagList.FormattingEnabled = true;
            this.sourceTagList.Name = "sourceTagList";
            this.sourceTagList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.sourceTagList_ItemCheck);
            // 
            // destinationTagList
            // 
            this.destinationTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.destinationTagList.DropDownWidth = 250;
            this.destinationTagList.FormattingEnabled = true;
            resources.ApplyResources(this.destinationTagList, "destinationTagList");
            this.destinationTagList.Name = "destinationTagList";
            this.toolTip.SetToolTip(this.destinationTagList, resources.GetString("destinationTagList.ToolTip"));
            this.destinationTagList.SelectedIndexChanged += new System.EventHandler(this.destinationTagList_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // conditionList
            // 
            this.conditionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.conditionList, "conditionList");
            this.conditionList.FormattingEnabled = true;
            this.conditionList.Name = "conditionList";
            this.conditionList.SelectedIndexChanged += new System.EventHandler(this.conditionList_SelectedIndexChanged);
            // 
            // conditionFieldList
            // 
            this.conditionFieldList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.conditionFieldList.DropDownWidth = 250;
            resources.ApplyResources(this.conditionFieldList, "conditionFieldList");
            this.conditionFieldList.FormattingEnabled = true;
            this.conditionFieldList.Name = "conditionFieldList";
            this.conditionFieldList.SelectedIndexChanged += new System.EventHandler(this.conditionFieldList_SelectedIndexChanged);
            // 
            // conditionCheckBox
            // 
            resources.ApplyResources(this.conditionCheckBox, "conditionCheckBox");
            this.conditionCheckBox.Name = "conditionCheckBox";
            this.conditionCheckBox.UseVisualStyleBackColor = true;
            this.conditionCheckBox.CheckedChanged += new System.EventHandler(this.checkBoxCondition_CheckedChanged);
            // 
            // buttonUncheckAll
            // 
            resources.ApplyResources(this.buttonUncheckAll, "buttonUncheckAll");
            this.buttonUncheckAll.Name = "buttonUncheckAll";
            this.buttonUncheckAll.UseVisualStyleBackColor = true;
            this.buttonUncheckAll.Click += new System.EventHandler(this.buttonUncheckAll_Click);
            // 
            // buttonCheckAll
            // 
            resources.ApplyResources(this.buttonCheckAll, "buttonCheckAll");
            this.buttonCheckAll.Name = "buttonCheckAll";
            this.buttonCheckAll.UseVisualStyleBackColor = true;
            this.buttonCheckAll.Click += new System.EventHandler(this.buttonCheckAll_Click);
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
            // buttonPreview
            // 
            resources.ApplyResources(this.buttonPreview, "buttonPreview");
            this.dirtyErrorProvider.SetError(this.buttonPreview, resources.GetString("buttonPreview.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonPreview, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonPreview.IconAlignment"))));
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.UseVisualStyleBackColor = true;
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // comparedFieldList
            // 
            this.comparedFieldList.DropDownWidth = 250;
            resources.ApplyResources(this.comparedFieldList, "comparedFieldList");
            this.comparedFieldList.FormattingEnabled = true;
            this.comparedFieldList.Name = "comparedFieldList";
            this.comparedFieldList.TextUpdate += new System.EventHandler(this.comparedFieldList_TextUpdate);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // sourceFieldComboBox
            // 
            this.sourceFieldComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceFieldComboBox.DropDownWidth = 250;
            this.sourceFieldComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.sourceFieldComboBox, "sourceFieldComboBox");
            this.sourceFieldComboBox.Name = "sourceFieldComboBox";
            this.sourceFieldComboBox.SelectedIndexChanged += new System.EventHandler(this.fieldComboBox_SelectedIndexChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // functionComboBox
            // 
            this.functionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.functionComboBox.DropDownWidth = 250;
            resources.ApplyResources(this.functionComboBox, "functionComboBox");
            this.functionComboBox.Name = "functionComboBox";
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
            this.label6.Name = "label6";
            // 
            // parameter2ComboBox
            // 
            this.parameter2ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameter2ComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.parameter2ComboBox, "parameter2ComboBox");
            this.parameter2ComboBox.Name = "parameter2ComboBox";
            this.parameter2ComboBox.SelectedIndexChanged += new System.EventHandler(this.functionComboBox_SelectedIndexChanged);
            // 
            // presetsBox
            // 
            resources.ApplyResources(this.presetsBox, "presetsBox");
            this.presetsBox.FormattingEnabled = true;
            this.presetsBox.Name = "presetsBox";
            this.presetsBox.SelectedIndexChanged += new System.EventHandler(this.presetsBox_SelectedIndexChanged);
            // 
            // recalculateOnNumberOfTagsChangesCheckBox
            // 
            resources.ApplyResources(this.recalculateOnNumberOfTagsChangesCheckBox, "recalculateOnNumberOfTagsChangesCheckBox");
            this.recalculateOnNumberOfTagsChangesCheckBox.Name = "recalculateOnNumberOfTagsChangesCheckBox";
            this.recalculateOnNumberOfTagsChangesCheckBox.UseVisualStyleBackColor = true;
            this.recalculateOnNumberOfTagsChangesCheckBox.CheckedChanged += new System.EventHandler(this.recalculateOnNumberOfTagsChangesCheckBox_CheckedChanged);
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
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // buttonAddPreset
            // 
            resources.ApplyResources(this.buttonAddPreset, "buttonAddPreset");
            this.buttonAddPreset.Name = "buttonAddPreset";
            this.buttonAddPreset.UseVisualStyleBackColor = true;
            this.buttonAddPreset.Click += new System.EventHandler(this.buttonAddPreset_Click);
            // 
            // buttonUpdatePreset
            // 
            resources.ApplyResources(this.buttonUpdatePreset, "buttonUpdatePreset");
            this.buttonUpdatePreset.Name = "buttonUpdatePreset";
            this.buttonUpdatePreset.UseVisualStyleBackColor = true;
            this.buttonUpdatePreset.Click += new System.EventHandler(this.buttonUpdatePreset_Click);
            // 
            // buttonDeletePreset
            // 
            resources.ApplyResources(this.buttonDeletePreset, "buttonDeletePreset");
            this.buttonDeletePreset.Name = "buttonDeletePreset";
            this.buttonDeletePreset.UseVisualStyleBackColor = true;
            this.buttonDeletePreset.Click += new System.EventHandler(this.buttonDeletePreset_Click);
            // 
            // labelNotSaved
            // 
            resources.ApplyResources(this.labelNotSaved, "labelNotSaved");
            this.labelNotSaved.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.labelNotSaved.Name = "labelNotSaved";
            // 
            // buttonIdOK
            // 
            this.toolTip.SetToolTip(this.buttonIdOK, resources.GetString("buttonIdOK.ToolTip"));
            resources.ApplyResources(this.buttonIdOK, "buttonIdOK");
            this.buttonIdOK.Name = "buttonIdOK";
            this.buttonIdOK.UseVisualStyleBackColor = true;
            // 
            // clearIdButton
            // 
            this.toolTip.SetToolTip(this.clearIdButton, resources.GetString("clearIdButton.ToolTip"));
            resources.ApplyResources(this.clearIdButton, "clearIdButton");
            this.clearIdButton.Name = "clearIdButton";
            this.clearIdButton.UseVisualStyleBackColor = true;
            this.clearIdButton.Click += new System.EventHandler(this.clearIdButton_Click);
            // 
            // idTextBox
            // 
            resources.ApplyResources(this.idTextBox, "idTextBox");
            this.idTextBox.Name = "idTextBox";
            this.toolTip.SetToolTip(this.idTextBox, resources.GetString("idTextBox.ToolTip"));
            this.idTextBox.Leave += new System.EventHandler(this.idTextBox_Leave);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // AutoLibraryReportCommand
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.buttonIdOK);
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
        private System.Windows.Forms.Button buttonIdOK;
        private System.Windows.Forms.Button clearIdButton;
        private System.Windows.Forms.TextBox idTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ToolTip toolTip;
    }
}