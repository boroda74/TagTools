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
                emptyArtwork.Dispose();

                columnTemplate.Dispose();
                artworkCellTemplate.Dispose();
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
            this.buttonCancel = new System.Windows.Forms.Button();
            previewTable = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.searchFolderTextBox = new System.Windows.Forms.ComboBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.forLastLabel = new System.Windows.Forms.Label();
            this.numberOfBackupsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.backupsLabel = new System.Windows.Forms.Label();
            this.rereadButton = new System.Windows.Forms.Button();
            this.restoreSelectedButton = new System.Windows.Forms.Button();
            this.selectTagsButton = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.rememberColumnAsDefaultWidthCheckBox = new System.Windows.Forms.CheckBox();
            this.undoButton = new System.Windows.Forms.Button();
            this.autoSelectTagsCheckBox = new System.Windows.Forms.CheckBox();
            this.rememberColumnAsDefaultWidthCheckBoxLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.trackListComboBox = new System.Windows.Forms.ComboBox();
            this.optionsPanel = new System.Windows.Forms.Panel();
            this.controlsPanel = new System.Windows.Forms.Panel();
            this.placeholderLabel3 = new System.Windows.Forms.Label();
            this.Library = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImageCellTemplate = new System.Windows.Forms.DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)(previewTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfBackupsNumericUpDown)).BeginInit();
            this.optionsPanel.SuspendLayout();
            this.controlsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Tag = "#buttonCancel";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Tag = "#controlsPanel@non-defaultable";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // previewTable
            // 
            previewTable.AllowUserToAddRows = false;
            previewTable.AllowUserToDeleteRows = false;
            resources.ApplyResources(previewTable, "previewTable");
            previewTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            previewTable.BackgroundColor = System.Drawing.SystemColors.Window;
            previewTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            previewTable.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            previewTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            previewTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Library,
            this.Column1,
            this.ImageCellTemplate});
            previewTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            previewTable.Name = "previewTable";
            previewTable.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            previewTable.Tag = "#TagHistory&controlsPanel";
            previewTable.RowHeadersWidthChanged += new System.EventHandler(this.previewTable_RowHeadersWidthChanged);
            previewTable.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.previewTable_CellEndEdit);
            previewTable.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.previewTable_CellFormatting);
            previewTable.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.previewTable_ColumnHeaderMouseClick);
            previewTable.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.previewTable_ColumnWidthChanged);
            previewTable.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.previewTable_DataError);
            previewTable.SelectionChanged += new System.EventHandler(this.previewTable_SelectionChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            this.label3.Tag = "#optionsPanel@pinned-to-parent-x";
            // 
            // searchFolderTextBox
            // 
            resources.ApplyResources(this.searchFolderTextBox, "searchFolderTextBox");
            this.searchFolderTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.searchFolderTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.searchFolderTextBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.searchFolderTextBox.FormattingEnabled = true;
            this.searchFolderTextBox.Name = "searchFolderTextBox";
            this.searchFolderTextBox.Tag = "#browseButton";
            this.searchFolderTextBox.Leave += new System.EventHandler(this.searchFolderTextBox_Leave);
            // 
            // browseButton
            // 
            resources.ApplyResources(this.browseButton, "browseButton");
            this.browseButton.Name = "browseButton";
            this.browseButton.Tag = "#forLastLabel@non-defaultable";
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
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
            // restoreSelectedButton
            // 
            resources.ApplyResources(this.restoreSelectedButton, "restoreSelectedButton");
            this.restoreSelectedButton.Name = "restoreSelectedButton";
            this.restoreSelectedButton.Tag = "#undoButton";
            this.toolTip1.SetToolTip(this.restoreSelectedButton, resources.GetString("restoreSelectedButton.ToolTip"));
            this.restoreSelectedButton.Click += new System.EventHandler(this.restoreSelectedButton_Click);
            // 
            // selectTagsButton
            // 
            resources.ApplyResources(this.selectTagsButton, "selectTagsButton");
            this.selectTagsButton.Name = "selectTagsButton";
            this.selectTagsButton.Tag = "@non-defaultable";
            this.toolTip1.SetToolTip(this.selectTagsButton, resources.GetString("selectTagsButton.ToolTip"));
            this.selectTagsButton.Click += new System.EventHandler(this.selectTagsButton_Click);
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
            // undoButton
            // 
            resources.ApplyResources(this.undoButton, "undoButton");
            this.undoButton.Name = "undoButton";
            this.toolTip1.SetToolTip(this.undoButton, resources.GetString("undoButton.ToolTip"));
            this.undoButton.Click += new System.EventHandler(this.undoButton_Click);
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
            this.trackListComboBox.DropDownClosed += new System.EventHandler(this.trackListComboBox_DropDownClosed);
            // 
            // optionsPanel
            // 
            resources.ApplyResources(this.optionsPanel, "optionsPanel");
            this.optionsPanel.Controls.Add(this.rereadButton);
            this.optionsPanel.Controls.Add(this.backupsLabel);
            this.optionsPanel.Controls.Add(this.numberOfBackupsNumericUpDown);
            this.optionsPanel.Controls.Add(this.forLastLabel);
            this.optionsPanel.Controls.Add(this.browseButton);
            this.optionsPanel.Controls.Add(this.searchFolderTextBox);
            this.optionsPanel.Controls.Add(this.trackListComboBox);
            this.optionsPanel.Name = "optionsPanel";
            this.optionsPanel.Tag = "#TagHistory&previewTable";
            // 
            // controlsPanel
            // 
            resources.ApplyResources(this.controlsPanel, "controlsPanel");
            this.controlsPanel.Controls.Add(this.buttonCancel);
            this.controlsPanel.Controls.Add(this.buttonOK);
            this.controlsPanel.Controls.Add(this.rememberColumnAsDefaultWidthCheckBoxLabel);
            this.controlsPanel.Controls.Add(this.rememberColumnAsDefaultWidthCheckBox);
            this.controlsPanel.Controls.Add(this.selectTagsButton);
            this.controlsPanel.Controls.Add(this.autoSelectTagsCheckBox);
            this.controlsPanel.Controls.Add(this.undoButton);
            this.controlsPanel.Controls.Add(this.restoreSelectedButton);
            this.controlsPanel.Controls.Add(this.placeholderLabel3);
            this.controlsPanel.Name = "controlsPanel";
            this.controlsPanel.Tag = "#TagHistory&TagHistory";
            // 
            // placeholderLabel3
            // 
            resources.ApplyResources(this.placeholderLabel3, "placeholderLabel3");
            this.placeholderLabel3.Name = "placeholderLabel3";
            this.placeholderLabel3.Tag = "";
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
            // TagHistory
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.controlsPanel);
            this.Controls.Add(previewTable);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.optionsPanel);
            this.DoubleBuffered = true;
            this.Name = "TagHistory";
            this.Load += new System.EventHandler(this.TagHistory_Load);
            this.Shown += new System.EventHandler(this.TagHistoryPlugin_Shown);
            ((System.ComponentModel.ISupportInitialize)(previewTable)).EndInit();
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
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.DataGridView previewTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox searchFolderTextBox;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.Label forLastLabel;
        private System.Windows.Forms.NumericUpDown numberOfBackupsNumericUpDown;
        private System.Windows.Forms.Label backupsLabel;
        private System.Windows.Forms.Button rereadButton;
        private System.Windows.Forms.Button restoreSelectedButton;
        private System.Windows.Forms.Button selectTagsButton;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox rememberColumnAsDefaultWidthCheckBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox trackListComboBox;
        private System.Windows.Forms.Button undoButton;
        private System.Windows.Forms.CheckBox autoSelectTagsCheckBox;
        private System.Windows.Forms.Label rememberColumnAsDefaultWidthCheckBoxLabel;
        private System.Windows.Forms.Panel optionsPanel;
        private System.Windows.Forms.Panel controlsPanel;
        private System.Windows.Forms.Label placeholderLabel3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Library;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewImageColumn ImageCellTemplate;
    }
}