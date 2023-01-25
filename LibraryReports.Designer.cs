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
            this.sourceTagList = new System.Windows.Forms.CheckedListBox();
            this.buttonSave = new System.Windows.Forms.Button();
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comparedFieldList = new System.Windows.Forms.ComboBox();
            this.presetsBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.fieldComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.functionComboBox = new System.Windows.Forms.ComboBox();
            this.totalsCheckBox = new System.Windows.Forms.CheckBox();
            this.dirtyErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.parameter2ComboBox = new System.Windows.Forms.ComboBox();
            this.resizeArtworkCheckBox = new System.Windows.Forms.CheckBox();
            this.xArworkSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.yArworkSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.openReportCheckBox = new System.Windows.Forms.CheckBox();
            this.formatComboBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xArworkSizeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yArworkSizeUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // sourceTagList
            // 
            resources.ApplyResources(this.sourceTagList, "sourceTagList");
            this.sourceTagList.CheckOnClick = true;
            this.sourceTagList.FormattingEnabled = true;
            this.sourceTagList.Name = "sourceTagList";
            this.sourceTagList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.sourceTagList_ItemCheck);
            // 
            // buttonSave
            // 
            resources.ApplyResources(this.buttonSave, "buttonSave");
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // destinationTagList
            // 
            this.destinationTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.destinationTagList.DropDownWidth = 250;
            this.destinationTagList.FormattingEnabled = true;
            resources.ApplyResources(this.destinationTagList, "destinationTagList");
            this.destinationTagList.Name = "destinationTagList";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // conditionList
            // 
            resources.ApplyResources(this.conditionList, "conditionList");
            this.conditionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.conditionList.FormattingEnabled = true;
            this.conditionList.Name = "conditionList";
            // 
            // conditionFieldList
            // 
            resources.ApplyResources(this.conditionFieldList, "conditionFieldList");
            this.conditionFieldList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.conditionFieldList.DropDownWidth = 250;
            this.conditionFieldList.FormattingEnabled = true;
            this.conditionFieldList.Name = "conditionFieldList";
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
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // comparedFieldList
            // 
            resources.ApplyResources(this.comparedFieldList, "comparedFieldList");
            this.comparedFieldList.DropDownWidth = 250;
            this.comparedFieldList.FormattingEnabled = true;
            this.comparedFieldList.Name = "comparedFieldList";
            // 
            // presetsBox
            // 
            resources.ApplyResources(this.presetsBox, "presetsBox");
            this.presetsBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.presetsBox.DropDownWidth = 1000;
            this.presetsBox.FormattingEnabled = true;
            this.presetsBox.Name = "presetsBox";
            this.presetsBox.SelectedValueChanged += new System.EventHandler(this.presetsBox_SelectedValueChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // fieldComboBox
            // 
            this.fieldComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fieldComboBox.DropDownWidth = 250;
            this.fieldComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.fieldComboBox, "fieldComboBox");
            this.fieldComboBox.Name = "fieldComboBox";
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
            // totalsCheckBox
            // 
            resources.ApplyResources(this.totalsCheckBox, "totalsCheckBox");
            this.totalsCheckBox.Name = "totalsCheckBox";
            this.totalsCheckBox.UseVisualStyleBackColor = true;
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
            // resizeArtworkCheckBox
            // 
            resources.ApplyResources(this.resizeArtworkCheckBox, "resizeArtworkCheckBox");
            this.resizeArtworkCheckBox.Name = "resizeArtworkCheckBox";
            this.resizeArtworkCheckBox.UseVisualStyleBackColor = true;
            this.resizeArtworkCheckBox.CheckedChanged += new System.EventHandler(this.resizeArtworkCheckBox_CheckedChanged);
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
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
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
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // openReportCheckBox
            // 
            resources.ApplyResources(this.openReportCheckBox, "openReportCheckBox");
            this.openReportCheckBox.Image = global::MusicBeePlugin.Properties.Resources.Window;
            this.openReportCheckBox.Name = "openReportCheckBox";
            this.toolTip1.SetToolTip(this.openReportCheckBox, resources.GetString("openReportCheckBox.ToolTip"));
            this.openReportCheckBox.UseVisualStyleBackColor = true;
            // 
            // formatComboBox
            // 
            resources.ApplyResources(this.formatComboBox, "formatComboBox");
            this.formatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.formatComboBox.FormattingEnabled = true;
            this.formatComboBox.Name = "formatComboBox";
            this.toolTip1.SetToolTip(this.formatComboBox, resources.GetString("formatComboBox.ToolTip"));
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // LibraryReportsCommand
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.label9);
            this.Controls.Add(this.formatComboBox);
            this.Controls.Add(this.openReportCheckBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.yArworkSizeUpDown);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.xArworkSizeUpDown);
            this.Controls.Add(this.resizeArtworkCheckBox);
            this.Controls.Add(this.parameter2ComboBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.totalsCheckBox);
            this.Controls.Add(this.functionComboBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.fieldComboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.presetsBox);
            this.Controls.Add(this.comparedFieldList);
            this.Controls.Add(this.buttonSave);
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
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.label1);
            this.Name = "LibraryReportsCommand";
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xArworkSizeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yArworkSizeUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView previewTable;
        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCheckAll;
        private System.Windows.Forms.CheckedListBox sourceTagList;
        private System.Windows.Forms.Button buttonUncheckAll;
        private System.Windows.Forms.CheckBox conditionCheckBox;
        private System.Windows.Forms.ComboBox conditionFieldList;
        private System.Windows.Forms.ComboBox conditionList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox destinationTagList;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.ErrorProvider dirtyErrorProvider;
        private System.Windows.Forms.ComboBox comparedFieldList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox presetsBox;
        private System.Windows.Forms.ComboBox fieldComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox functionComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox totalsCheckBox;
        private System.Windows.Forms.ComboBox parameter2ComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown yArworkSizeUpDown;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown xArworkSizeUpDown;
        private System.Windows.Forms.CheckBox resizeArtworkCheckBox;
        private System.Windows.Forms.CheckBox openReportCheckBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox formatComboBox;
        private System.Windows.Forms.Label label9;
    }
}