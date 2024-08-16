namespace MusicBeePlugin
{
    partial class TagHistory
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
                emptyArtwork?.Dispose();

                columnTemplate?.Dispose();
                artworkCellTemplate?.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TagHistory));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.previewTable = new System.Windows.Forms.DataGridView();
            this.Library = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImageCellTemplate = new System.Windows.Forms.DataGridViewImageColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.searchFolderComboBox = new System.Windows.Forms.ComboBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.forLastLabel = new System.Windows.Forms.Label();
            this.numberOfBackupsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.backupsLabel = new System.Windows.Forms.Label();
            this.rereadButton = new System.Windows.Forms.Button();
            this.buttonRestoreSelected = new System.Windows.Forms.Button();
            this.buttonSelectTags = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.rememberColumnAsDefaultWidthCheckBox = new System.Windows.Forms.CheckBox();
            this.buttonUndo = new System.Windows.Forms.Button();
            this.autoSelectTagsCheckBox = new System.Windows.Forms.CheckBox();
            this.rememberColumnAsDefaultWidthCheckBoxLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.trackListComboBox = new System.Windows.Forms.ComboBox();
            this.optionsPanel = new System.Windows.Forms.Panel();
            this.controlsPanel = new System.Windows.Forms.Panel();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.placeholderLabel3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfBackupsNumericUpDown)).BeginInit();
            this.optionsPanel.SuspendLayout();
            this.controlsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Tag = "#buttonClose";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonClose
            // 
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Tag = "#controlsPanel@non-defaultable";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // previewTable
            // 
            this.previewTable.AllowUserToAddRows = false;
            this.previewTable.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.previewTable, "previewTable");
            this.previewTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.previewTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.previewTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.previewTable.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.previewTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.previewTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Library,
            this.Column1,
            this.ImageCellTemplate});
            this.previewTable.Name = "previewTable";
            this.previewTable.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.previewTable.Tag = "#TagHistory&controlsPanel";
            this.previewTable.RowHeadersWidthChanged += new System.EventHandler(this.previewTable_RowHeadersWidthChanged);
            this.previewTable.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.previewTable_CellEndEdit);
            this.previewTable.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.previewTable_CellFormatting);
            this.previewTable.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.previewTable_ColumnHeaderMouseClick);
            this.previewTable.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.previewTable_ColumnWidthChanged);
            this.previewTable.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.previewTable_DataError);
            this.previewTable.SelectionChanged += new System.EventHandler(this.previewTable_SelectionChanged);
            // 
            // Library
            // 
            this.Library.FillWeight = 1F;
            this.Library.Frozen = true;
            resources.ApplyResources(this.Library, "Library");
            this.Library.Name = "Library";
            this.Library.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column1
            // 
            this.Column1.FillWeight = 1F;
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // ImageCellTemplate
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle2.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle2.NullValue")));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ImageCellTemplate.DefaultCellStyle = dataGridViewCellStyle2;
            this.ImageCellTemplate.FillWeight = 1F;
            resources.ApplyResources(this.ImageCellTemplate, "ImageCellTemplate");
            this.ImageCellTemplate.Image = global::MusicBeePlugin.Properties.Resources.search;
            this.ImageCellTemplate.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.ImageCellTemplate.Name = "ImageCellTemplate";
            this.ImageCellTemplate.ReadOnly = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            this.label3.Tag = "#optionsPanel@pinned-to-parent-x";
            // 
            // searchFolderComboBox
            // 
            resources.ApplyResources(this.searchFolderComboBox, "searchFolderComboBox");
            this.searchFolderComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.searchFolderComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.searchFolderComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.searchFolderComboBox.FormattingEnabled = true;
            this.searchFolderComboBox.Name = "searchFolderComboBox";
            this.searchFolderComboBox.Tag = "#buttonBrowse";
            this.searchFolderComboBox.SelectedIndexChanged += new System.EventHandler(this.searchFolderComboBox_SelectedIndexChanged);
            // 
            // buttonBrowse
            // 
            resources.ApplyResources(this.buttonBrowse, "buttonBrowse");
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Tag = "#forLastLabel@non-defaultable";
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // forLastLabel
            // 
            resources.ApplyResources(this.forLastLabel, "forLastLabel");
            this.forLastLabel.Name = "forLastLabel";
            this.forLastLabel.Tag = "#numberOfBackupsNumericUpDown";
            // 
            // numberOfBackupsNumericUpDown
            // 
            resources.ApplyResources(this.numberOfBackupsNumericUpDown, "numberOfBackupsNumericUpDown");
            this.numberOfBackupsNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numberOfBackupsNumericUpDown.Name = "numberOfBackupsNumericUpDown";
            this.numberOfBackupsNumericUpDown.Tag = "#backupsLabel";
            this.numberOfBackupsNumericUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numberOfBackupsNumericUpDown.ValueChanged += new System.EventHandler(this.numberOfBackupsNumericUpDown_ValueChanged);
            // 
            // backupsLabel
            // 
            resources.ApplyResources(this.backupsLabel, "backupsLabel");
            this.backupsLabel.Name = "backupsLabel";
            this.backupsLabel.Tag = "#rereadButton";
            // 
            // rereadButton
            // 
            resources.ApplyResources(this.rereadButton, "rereadButton");
            this.rereadButton.Name = "rereadButton";
            this.rereadButton.Tag = "#optionsPanel@non-defaultable";
            this.rereadButton.Click += new System.EventHandler(this.rereadButton_Click);
            // 
            // buttonRestoreSelected
            // 
            resources.ApplyResources(this.buttonRestoreSelected, "buttonRestoreSelected");
            this.buttonRestoreSelected.Name = "buttonRestoreSelected";
            this.buttonRestoreSelected.Tag = "#buttonUndo";
            this.toolTip1.SetToolTip(this.buttonRestoreSelected, resources.GetString("buttonRestoreSelected.ToolTip"));
            this.buttonRestoreSelected.Click += new System.EventHandler(this.restoreSelectedButton_Click);
            // 
            // buttonSelectTags
            // 
            resources.ApplyResources(this.buttonSelectTags, "buttonSelectTags");
            this.buttonSelectTags.Name = "buttonSelectTags";
            this.buttonSelectTags.Tag = "@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonSelectTags, resources.GetString("buttonSelectTags.ToolTip"));
            this.buttonSelectTags.Click += new System.EventHandler(this.buttonSelectTags_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // rememberColumnAsDefaultWidthCheckBox
            // 
            resources.ApplyResources(this.rememberColumnAsDefaultWidthCheckBox, "rememberColumnAsDefaultWidthCheckBox");
            this.rememberColumnAsDefaultWidthCheckBox.Name = "rememberColumnAsDefaultWidthCheckBox";
            this.rememberColumnAsDefaultWidthCheckBox.Tag = "#rememberColumnAsDefaultWidthCheckBoxLabel";
            this.toolTip1.SetToolTip(this.rememberColumnAsDefaultWidthCheckBox, resources.GetString("rememberColumnAsDefaultWidthCheckBox.ToolTip"));
            // 
            // buttonUndo
            // 
            resources.ApplyResources(this.buttonUndo, "buttonUndo");
            this.buttonUndo.Name = "buttonUndo";
            this.toolTip1.SetToolTip(this.buttonUndo, resources.GetString("buttonUndo.ToolTip"));
            this.buttonUndo.Click += new System.EventHandler(this.undoButton_Click);
            // 
            // autoSelectTagsCheckBox
            // 
            resources.ApplyResources(this.autoSelectTagsCheckBox, "autoSelectTagsCheckBox");
            this.autoSelectTagsCheckBox.Name = "autoSelectTagsCheckBox";
            this.autoSelectTagsCheckBox.Tag = "#placeholderLabel3";
            this.toolTip1.SetToolTip(this.autoSelectTagsCheckBox, resources.GetString("autoSelectTagsCheckBox.ToolTip"));
            this.autoSelectTagsCheckBox.CheckedChanged += new System.EventHandler(this.autoSelectTagsCheckBox_CheckedChanged);
            // 
            // rememberColumnAsDefaultWidthCheckBoxLabel
            // 
            resources.ApplyResources(this.rememberColumnAsDefaultWidthCheckBoxLabel, "rememberColumnAsDefaultWidthCheckBoxLabel");
            this.rememberColumnAsDefaultWidthCheckBoxLabel.Name = "rememberColumnAsDefaultWidthCheckBoxLabel";
            this.toolTip1.SetToolTip(this.rememberColumnAsDefaultWidthCheckBoxLabel, resources.GetString("rememberColumnAsDefaultWidthCheckBoxLabel.ToolTip"));
            this.rememberColumnAsDefaultWidthCheckBoxLabel.Click += new System.EventHandler(this.rememberColumnAsDefaultWidthCheckBoxLabel_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            this.label4.Tag = "@pinned-to-parent-x";
            // 
            // trackListComboBox
            // 
            resources.ApplyResources(this.trackListComboBox, "trackListComboBox");
            this.trackListComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.trackListComboBox.FormattingEnabled = true;
            this.trackListComboBox.Items.AddRange(new object[] {
            resources.GetString("trackListComboBox.Items")});
            this.trackListComboBox.Name = "trackListComboBox";
            this.trackListComboBox.Tag = "#optionsPanel";
            this.trackListComboBox.SelectedIndexChanged += new System.EventHandler(this.trackListComboBox_SelectedIndexChanged);
            // 
            // optionsPanel
            // 
            resources.ApplyResources(this.optionsPanel, "optionsPanel");
            this.optionsPanel.Controls.Add(this.rereadButton);
            this.optionsPanel.Controls.Add(this.backupsLabel);
            this.optionsPanel.Controls.Add(this.numberOfBackupsNumericUpDown);
            this.optionsPanel.Controls.Add(this.forLastLabel);
            this.optionsPanel.Controls.Add(this.buttonBrowse);
            this.optionsPanel.Controls.Add(this.searchFolderComboBox);
            this.optionsPanel.Controls.Add(this.trackListComboBox);
            this.optionsPanel.Name = "optionsPanel";
            this.optionsPanel.Tag = "#TagHistory&previewTable";
            // 
            // controlsPanel
            // 
            resources.ApplyResources(this.controlsPanel, "controlsPanel");
            this.controlsPanel.Controls.Add(this.buttonPreview);
            this.controlsPanel.Controls.Add(this.buttonClose);
            this.controlsPanel.Controls.Add(this.buttonOK);
            this.controlsPanel.Controls.Add(this.rememberColumnAsDefaultWidthCheckBoxLabel);
            this.controlsPanel.Controls.Add(this.rememberColumnAsDefaultWidthCheckBox);
            this.controlsPanel.Controls.Add(this.buttonSelectTags);
            this.controlsPanel.Controls.Add(this.autoSelectTagsCheckBox);
            this.controlsPanel.Controls.Add(this.buttonUndo);
            this.controlsPanel.Controls.Add(this.buttonRestoreSelected);
            this.controlsPanel.Controls.Add(this.placeholderLabel3);
            this.controlsPanel.Name = "controlsPanel";
            this.controlsPanel.Tag = "#TagHistory&TagHistory";
            // 
            // buttonPreview
            // 
            resources.ApplyResources(this.buttonPreview, "buttonPreview");
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Tag = "#buttonOK";
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // placeholderLabel3
            // 
            resources.ApplyResources(this.placeholderLabel3, "placeholderLabel3");
            this.placeholderLabel3.Name = "placeholderLabel3";
            this.placeholderLabel3.Tag = "";
            // 
            // TagHistory
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.Controls.Add(this.controlsPanel);
            this.Controls.Add(this.previewTable);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.optionsPanel);
            this.DoubleBuffered = true;
            this.Name = "TagHistory";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TagHistory_FormClosing);
            this.Load += new System.EventHandler(this.TagHistory_Load);
            this.Shown += new System.EventHandler(this.TagHistory_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfBackupsNumericUpDown)).EndInit();
            this.optionsPanel.ResumeLayout(false);
            this.optionsPanel.PerformLayout();
            this.controlsPanel.ResumeLayout(false);
            this.controlsPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.DataGridView previewTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox searchFolderComboBox;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Label forLastLabel;
        private System.Windows.Forms.NumericUpDown numberOfBackupsNumericUpDown;
        private System.Windows.Forms.Label backupsLabel;
        private System.Windows.Forms.Button rereadButton;
        private System.Windows.Forms.Button buttonRestoreSelected;
        private System.Windows.Forms.Button buttonSelectTags;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox rememberColumnAsDefaultWidthCheckBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox trackListComboBox;
        private System.Windows.Forms.Button buttonUndo;
        private System.Windows.Forms.CheckBox autoSelectTagsCheckBox;
        private System.Windows.Forms.Label rememberColumnAsDefaultWidthCheckBoxLabel;
        private System.Windows.Forms.Panel optionsPanel;
        private System.Windows.Forms.Panel controlsPanel;
        private System.Windows.Forms.Label placeholderLabel3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Library;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewImageColumn ImageCellTemplate;
        private System.Windows.Forms.Button buttonPreview;
    }
}