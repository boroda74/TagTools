namespace MusicBeePlugin
{
    partial class TagHistoryCommand
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TagHistoryCommand));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.previewTable = new System.Windows.Forms.DataGridView();
            this.Library = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImageCellTemplate = new System.Windows.Forms.DataGridViewImageColumn();
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
            this.rememberColumnasDefaulltWidthCheckBox = new System.Windows.Forms.CheckBox();
            this.undoButton = new System.Windows.Forms.Button();
            this.AutoSelectTagsCheckBox = new System.Windows.Forms.CheckBox();
            this.rememberColumnasDefaulltWidthCheckBoxLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.trackListComboBox = new System.Windows.Forms.ComboBox();
            this.optionsPanel = new System.Windows.Forms.Panel();
            this.placeholderLabel2 = new System.Windows.Forms.Label();
            this.placeholderLabel1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfBackupsNumericUpDown)).BeginInit();
            this.optionsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Tag = "#buttonCancel";
            this.toolTip1.SetToolTip(this.buttonOK, resources.GetString("buttonOK.ToolTip"));
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Tag = "#TagHistoryCommand@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonCancel, resources.GetString("buttonCancel.ToolTip"));
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // previewTable
            // 
            resources.ApplyResources(this.previewTable, "previewTable");
            this.previewTable.AllowUserToAddRows = false;
            this.previewTable.AllowUserToDeleteRows = false;
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
            this.previewTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.previewTable.Name = "previewTable";
            this.previewTable.RowHeadersVisible = false;
            this.previewTable.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.previewTable.Tag = "";
            this.toolTip1.SetToolTip(this.previewTable, resources.GetString("previewTable.ToolTip"));
            this.previewTable.RowHeadersWidthChanged += new System.EventHandler(this.previewTable_RowHeadersWidthChanged);
            this.previewTable.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.previewTable_ColumnHeaderMouseClick);
            this.previewTable.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.previewTable_ColumnWidthChanged);
            this.previewTable.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.previewTable_DataError);
            this.previewTable.SelectionChanged += new System.EventHandler(this.previewTable_SelectionChanged);
            // 
            // Library
            // 
            this.Library.Frozen = true;
            resources.ApplyResources(this.Library, "Library");
            this.Library.Name = "Library";
            this.Library.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column1
            // 
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
            this.toolTip1.SetToolTip(this.label3, resources.GetString("label3.ToolTip"));
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
            this.toolTip1.SetToolTip(this.searchFolderTextBox, resources.GetString("searchFolderTextBox.ToolTip"));
            this.searchFolderTextBox.Leave += new System.EventHandler(this.searchFolderTextBox_Leave);
            // 
            // browseButton
            // 
            resources.ApplyResources(this.browseButton, "browseButton");
            this.browseButton.Name = "browseButton";
            this.browseButton.Tag = "#forLastLabel@non-defaultable";
            this.toolTip1.SetToolTip(this.browseButton, resources.GetString("browseButton.ToolTip"));
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
            this.toolTip1.SetToolTip(this.forLastLabel, resources.GetString("forLastLabel.ToolTip"));
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
            this.toolTip1.SetToolTip(this.numberOfBackupsNumericUpDown, resources.GetString("numberOfBackupsNumericUpDown.ToolTip"));
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
            this.toolTip1.SetToolTip(this.backupsLabel, resources.GetString("backupsLabel.ToolTip"));
            // 
            // rereadButton
            // 
            resources.ApplyResources(this.rereadButton, "rereadButton");
            this.rereadButton.Name = "rereadButton";
            this.rereadButton.Tag = "#optionsPanel";
            this.toolTip1.SetToolTip(this.rereadButton, resources.GetString("rereadButton.ToolTip"));
            this.rereadButton.Click += new System.EventHandler(this.rereadButton_Click);
            // 
            // restoreSelectedButton
            // 
            resources.ApplyResources(this.restoreSelectedButton, "restoreSelectedButton");
            this.restoreSelectedButton.Name = "restoreSelectedButton";
            this.restoreSelectedButton.Tag = "@non-defaultable";
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
            // rememberColumnasDefaulltWidthCheckBox
            // 
            resources.ApplyResources(this.rememberColumnasDefaulltWidthCheckBox, "rememberColumnasDefaulltWidthCheckBox");
            this.rememberColumnasDefaulltWidthCheckBox.Name = "rememberColumnasDefaulltWidthCheckBox";
            this.rememberColumnasDefaulltWidthCheckBox.Tag = "#rememberColumnasDefaulltWidthCheckBoxLabel";
            this.toolTip1.SetToolTip(this.rememberColumnasDefaulltWidthCheckBox, resources.GetString("rememberColumnasDefaulltWidthCheckBox.ToolTip"));
            // 
            // undoButton
            // 
            resources.ApplyResources(this.undoButton, "undoButton");
            this.undoButton.Name = "undoButton";
            this.toolTip1.SetToolTip(this.undoButton, resources.GetString("undoButton.ToolTip"));
            this.undoButton.Click += new System.EventHandler(this.undoButton_Click);
            // 
            // AutoSelectTagsCheckBox
            // 
            resources.ApplyResources(this.AutoSelectTagsCheckBox, "AutoSelectTagsCheckBox");
            this.AutoSelectTagsCheckBox.Name = "AutoSelectTagsCheckBox";
            this.AutoSelectTagsCheckBox.Tag = "#buttonCancel#rememberColumnasDefaulltWidthCheckBoxLabel#scaled-moved-x";
            this.toolTip1.SetToolTip(this.AutoSelectTagsCheckBox, resources.GetString("AutoSelectTagsCheckBox.ToolTip"));
            this.AutoSelectTagsCheckBox.CheckedChanged += new System.EventHandler(this.AutoSelectTagsCheckBox_CheckedChanged);
            // 
            // rememberColumnasDefaulltWidthCheckBoxLabel
            // 
            resources.ApplyResources(this.rememberColumnasDefaulltWidthCheckBoxLabel, "rememberColumnasDefaulltWidthCheckBoxLabel");
            this.rememberColumnasDefaulltWidthCheckBoxLabel.Name = "rememberColumnasDefaulltWidthCheckBoxLabel";
            this.toolTip1.SetToolTip(this.rememberColumnasDefaulltWidthCheckBoxLabel, resources.GetString("rememberColumnasDefaulltWidthCheckBoxLabel.ToolTip"));
            this.rememberColumnasDefaulltWidthCheckBoxLabel.Click += new System.EventHandler(this.rememberColumnasDefaulltWidthCheckBoxLabel_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            this.toolTip1.SetToolTip(this.label4, resources.GetString("label4.ToolTip"));
            // 
            // trackListComboBox
            // 
            resources.ApplyResources(this.trackListComboBox, "trackListComboBox");
            this.trackListComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.trackListComboBox.FormattingEnabled = true;
            this.trackListComboBox.Name = "trackListComboBox";
            this.trackListComboBox.Tag = "#optionsPanel";
            this.toolTip1.SetToolTip(this.trackListComboBox, resources.GetString("trackListComboBox.ToolTip"));
            this.trackListComboBox.DropDownClosed += new System.EventHandler(this.trackListComboBox_DropDownClosed);
            // 
            // optionsPanel
            // 
            resources.ApplyResources(this.optionsPanel, "optionsPanel");
            this.optionsPanel.Controls.Add(this.rereadButton);
            this.optionsPanel.Controls.Add(this.backupsLabel);
            this.optionsPanel.Controls.Add(this.numberOfBackupsNumericUpDown);
            this.optionsPanel.Controls.Add(this.browseButton);
            this.optionsPanel.Controls.Add(this.forLastLabel);
            this.optionsPanel.Controls.Add(this.searchFolderTextBox);
            this.optionsPanel.Controls.Add(this.placeholderLabel2);
            this.optionsPanel.Controls.Add(this.trackListComboBox);
            this.optionsPanel.Controls.Add(this.placeholderLabel1);
            this.optionsPanel.Name = "optionsPanel";
            this.optionsPanel.Tag = "#TagHistoryCommand";
            this.toolTip1.SetToolTip(this.optionsPanel, resources.GetString("optionsPanel.ToolTip"));
            // 
            // placeholderLabel2
            // 
            resources.ApplyResources(this.placeholderLabel2, "placeholderLabel2");
            this.placeholderLabel2.Name = "placeholderLabel2";
            this.placeholderLabel2.Tag = "#searchFolderTextBox";
            this.toolTip1.SetToolTip(this.placeholderLabel2, resources.GetString("placeholderLabel2.ToolTip"));
            // 
            // placeholderLabel1
            // 
            resources.ApplyResources(this.placeholderLabel1, "placeholderLabel1");
            this.placeholderLabel1.Name = "placeholderLabel1";
            this.placeholderLabel1.Tag = "#trackListComboBox";
            this.toolTip1.SetToolTip(this.placeholderLabel1, resources.GetString("placeholderLabel1.ToolTip"));
            // 
            // TagHistoryCommand
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.rememberColumnasDefaulltWidthCheckBoxLabel);
            this.Controls.Add(this.rememberColumnasDefaulltWidthCheckBox);
            this.Controls.Add(this.selectTagsButton);
            this.Controls.Add(this.AutoSelectTagsCheckBox);
            this.Controls.Add(this.undoButton);
            this.Controls.Add(this.restoreSelectedButton);
            this.Controls.Add(this.previewTable);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.optionsPanel);
            this.Name = "TagHistoryCommand";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.Shown += new System.EventHandler(this.TagHistoryPlugin_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfBackupsNumericUpDown)).EndInit();
            this.optionsPanel.ResumeLayout(false);
            this.optionsPanel.PerformLayout();
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
        private System.Windows.Forms.CheckBox rememberColumnasDefaulltWidthCheckBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox trackListComboBox;
        private System.Windows.Forms.Button undoButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn Library;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewImageColumn ImageCellTemplate;
        private System.Windows.Forms.CheckBox AutoSelectTagsCheckBox;
        private System.Windows.Forms.Label rememberColumnasDefaulltWidthCheckBoxLabel;
        private System.Windows.Forms.Panel optionsPanel;
        private System.Windows.Forms.Label placeholderLabel1;
        private System.Windows.Forms.Label placeholderLabel2;
    }
}