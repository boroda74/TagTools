namespace MusicBeePlugin
{
    partial class CompareTracks
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompareTracks));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.previewTable = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImageCellTemplate = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.selectTagsButton = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.rememberColumnAsDefaultWidthCheckBox = new System.Windows.Forms.CheckBox();
            this.autoSelectTagsCheckBox = new System.Windows.Forms.CheckBox();
            this.rememberColumnAsDefaultWidthCheckBoxLabel = new System.Windows.Forms.Label();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.buttonPaste = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.toolsPanel = new System.Windows.Forms.Panel();
            this.placeholderLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).BeginInit();
            this.toolsPanel.SuspendLayout();
            this.SuspendLayout();
            //
            //buttonOK
            //
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Tag = "#buttonCancel";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            //
            //buttonCancel
            //
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Tag = "#toolsPanel@non-defaultable@pinned-to-parent-x";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            //
            //previewTable
            //
            this.previewTable.AllowUserToAddRows = false;
            this.previewTable.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.previewTable, "previewTable");
            this.previewTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.previewTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.previewTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.previewTable.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.previewTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.previewTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.ImageCellTemplate});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.previewTable.DefaultCellStyle = dataGridViewCellStyle3;
            this.previewTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.previewTable.Name = "previewTable";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.previewTable.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.previewTable.RowHeadersVisible = false;
            this.previewTable.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.previewTable.Tag = "#CompareTracks&CompareTracks@pinned-to-parent-x@pinned-to-parent-y";
            this.previewTable.RowHeadersWidthChanged += new System.EventHandler(this.previewTable_RowHeadersWidthChanged);
            this.previewTable.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.previewTable_ColumnHeaderMouseClick);
            this.previewTable.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.previewTable_ColumnWidthChanged);
            this.previewTable.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.previewTable_DataError);
            this.previewTable.SelectionChanged += new System.EventHandler(this.previewTable_SelectionChanged);
            //
            //Column1
            //
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Name = "Column1";
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            //
            //ImageCellTemplate
            //
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle2.NullValue")));
            this.ImageCellTemplate.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.ImageCellTemplate, "ImageCellTemplate");
            this.ImageCellTemplate.Image = global::MusicBeePlugin.Properties.Resources.search;
            this.ImageCellTemplate.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.ImageCellTemplate.Name = "ImageCellTemplate";
            this.ImageCellTemplate.ReadOnly = true;
            //
            //dataGridViewTextBoxColumn1
            //
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            //
            //dataGridViewTextBoxColumn2
            //
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            //
            //dataGridViewTextBoxColumn3
            //
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            //
            //selectTagsButton
            //
            resources.ApplyResources(this.selectTagsButton, "selectTagsButton");
            this.selectTagsButton.Name = "selectTagsButton";
            this.selectTagsButton.Tag = "#rememberColumnAsDefaultWidthCheckBox@non-defaultable";
            this.toolTip1.SetToolTip(this.selectTagsButton, resources.GetString("selectTagsButton.ToolTip"));
            this.selectTagsButton.Click += new System.EventHandler(this.selectTagsButton_Click);
            //
            //toolTip1
            //
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            //
            //rememberColumnAsDefaultWidthCheckBox
            //
            resources.ApplyResources(this.rememberColumnAsDefaultWidthCheckBox, "rememberColumnAsDefaultWidthCheckBox");
            this.rememberColumnAsDefaultWidthCheckBox.Name = "rememberColumnAsDefaultWidthCheckBox";
            this.rememberColumnAsDefaultWidthCheckBox.Tag = "#rememberColumnAsDefaultWidthCheckBoxLabel";
            this.toolTip1.SetToolTip(this.rememberColumnAsDefaultWidthCheckBox, resources.GetString("rememberColumnAsDefaultWidthCheckBox.ToolTip"));
            //
            //autoSelectTagsCheckBox
            //
            resources.ApplyResources(this.autoSelectTagsCheckBox, "autoSelectTagsCheckBox");
            this.autoSelectTagsCheckBox.Name = "autoSelectTagsCheckBox";
            this.autoSelectTagsCheckBox.Tag = "#placeholderLabel@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.autoSelectTagsCheckBox, resources.GetString("autoSelectTagsCheckBox.ToolTip"));
            this.autoSelectTagsCheckBox.CheckedChanged += new System.EventHandler(this.autoSelectTagsCheckBox_CheckedChanged);
            //
            //rememberColumnAsDefaultWidthCheckBoxLabel
            //
            resources.ApplyResources(this.rememberColumnAsDefaultWidthCheckBoxLabel, "rememberColumnAsDefaultWidthCheckBoxLabel");
            this.rememberColumnAsDefaultWidthCheckBoxLabel.Name = "rememberColumnAsDefaultWidthCheckBoxLabel";
            this.toolTip1.SetToolTip(this.rememberColumnAsDefaultWidthCheckBoxLabel, resources.GetString("rememberColumnAsDefaultWidthCheckBoxLabel.ToolTip"));
            this.rememberColumnAsDefaultWidthCheckBoxLabel.Click += new System.EventHandler(this.rememberColumnAsDefaultWidthCheckBoxLabel_Click);
            //
            //buttonCopy
            //
            resources.ApplyResources(this.buttonCopy, "buttonCopy");
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.buttonCopy, resources.GetString("buttonCopy.ToolTip"));
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            //
            //buttonPaste
            //
            resources.ApplyResources(this.buttonPaste, "buttonPaste");
            this.buttonPaste.Name = "buttonPaste";
            this.buttonPaste.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.buttonPaste, resources.GetString("buttonPaste.ToolTip"));
            this.buttonPaste.Click += new System.EventHandler(this.buttonPaste_Click);
            //
            //buttonClear
            //
            resources.ApplyResources(this.buttonClear, "buttonClear");
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.buttonClear, resources.GetString("buttonClear.ToolTip"));
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            //
            //toolsPanel
            //
            resources.ApplyResources(this.toolsPanel, "toolsPanel");
            this.toolsPanel.Controls.Add(this.buttonCancel);
            this.toolsPanel.Controls.Add(this.buttonOK);
            this.toolsPanel.Controls.Add(this.buttonClear);
            this.toolsPanel.Controls.Add(this.buttonPaste);
            this.toolsPanel.Controls.Add(this.buttonCopy);
            this.toolsPanel.Controls.Add(this.rememberColumnAsDefaultWidthCheckBoxLabel);
            this.toolsPanel.Controls.Add(this.rememberColumnAsDefaultWidthCheckBox);
            this.toolsPanel.Controls.Add(this.selectTagsButton);
            this.toolsPanel.Controls.Add(this.autoSelectTagsCheckBox);
            this.toolsPanel.Controls.Add(this.placeholderLabel);
            this.toolsPanel.Name = "toolsPanel";
            this.toolsPanel.Tag = "#CompareTracks&CompareTracks@pinned-to-parent-x@pinned-to-parent-y";
            //
            //placeholderLabel
            //
            resources.ApplyResources(this.placeholderLabel, "placeholderLabel");
            this.placeholderLabel.Name = "placeholderLabel";
            //
            //CompareTracks
            //
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.previewTable);
            this.Controls.Add(this.toolsPanel);
            this.Name = "CompareTracks";
            this.Tag = "";
            this.Load += new System.EventHandler(this.CompareTracks_Load);
            this.Shown += new System.EventHandler(this.CompareTracks_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).EndInit();
            this.toolsPanel.ResumeLayout(false);
            this.toolsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.DataGridView previewTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.Button selectTagsButton;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox rememberColumnAsDefaultWidthCheckBox;
        private System.Windows.Forms.CheckBox autoSelectTagsCheckBox;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.Button buttonPaste;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Label rememberColumnAsDefaultWidthCheckBoxLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewImageColumn ImageCellTemplate;
        private System.Windows.Forms.Panel toolsPanel;
        private System.Windows.Forms.Label placeholderLabel;
    }
}