namespace MusicBeePlugin
{
    partial class MultipleSearchAndReplaceCommand
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultipleSearchAndReplaceCommand));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.templateTable = new System.Windows.Forms.DataGridView();
            this.SearchTag = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.previewTable = new System.Windows.Forms.DataGridView();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalTag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewTag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dirtyErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.templateNameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.sourceTagList = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.destinationTagList = new System.Windows.Forms.ComboBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            this.loadComboBox = new System.Windows.Forms.ComboBox();
            this.autoDestinationTagCheckBox = new System.Windows.Forms.CheckBox();
            this.searchOnlyCheckBox = new System.Windows.Forms.CheckBox();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.buttonDeleteSaved = new System.Windows.Forms.Button();
            this.autoApplyCheckBox = new System.Windows.Forms.CheckBox();
            this.autoApplyPictureBox = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.templateTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.autoApplyPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.BackColor = System.Drawing.SystemColors.GrayText;
            this.dirtyErrorProvider.SetError(this.splitContainer1, resources.GetString("splitContainer1.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.splitContainer1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.splitContainer1, ((int)(resources.GetObject("splitContainer1.IconPadding"))));
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.Panel1.Controls.Add(this.templateTable);
            this.dirtyErrorProvider.SetError(this.splitContainer1.Panel1, resources.GetString("splitContainer1.Panel1.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.splitContainer1.Panel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer1.Panel1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.splitContainer1.Panel1, ((int)(resources.GetObject("splitContainer1.Panel1.IconPadding"))));
            this.toolTip1.SetToolTip(this.splitContainer1.Panel1, resources.GetString("splitContainer1.Panel1.ToolTip"));
            // 
            // splitContainer1.Panel2
            // 
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.Panel2.Controls.Add(this.previewTable);
            this.dirtyErrorProvider.SetError(this.splitContainer1.Panel2, resources.GetString("splitContainer1.Panel2.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.splitContainer1.Panel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer1.Panel2.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.splitContainer1.Panel2, ((int)(resources.GetObject("splitContainer1.Panel2.IconPadding"))));
            this.toolTip1.SetToolTip(this.splitContainer1.Panel2, resources.GetString("splitContainer1.Panel2.ToolTip"));
            this.toolTip1.SetToolTip(this.splitContainer1, resources.GetString("splitContainer1.ToolTip"));
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // templateTable
            // 
            resources.ApplyResources(this.templateTable, "templateTable");
            this.templateTable.AllowUserToAddRows = false;
            this.templateTable.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.templateTable.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.templateTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.templateTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.templateTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.templateTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SearchTag,
            this.Column1,
            this.Column2,
            this.dataGridViewTextBoxColumn14,
            this.dataGridViewTextBoxColumn16,
            this.Column5});
            this.dirtyErrorProvider.SetError(this.templateTable, resources.GetString("templateTable.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.templateTable, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("templateTable.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.templateTable, ((int)(resources.GetObject("templateTable.IconPadding"))));
            this.templateTable.MultiSelect = false;
            this.templateTable.Name = "templateTable";
            this.templateTable.RowHeadersVisible = false;
            this.templateTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.toolTip1.SetToolTip(this.templateTable, resources.GetString("templateTable.ToolTip"));
            // 
            // SearchTag
            // 
            this.SearchTag.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.SearchTag.FillWeight = 60F;
            resources.ApplyResources(this.SearchTag, "SearchTag");
            this.SearchTag.MaxDropDownItems = 15;
            this.SearchTag.Name = "SearchTag";
            this.SearchTag.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SearchTag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column1.FalseValue = "F";
            this.Column1.FillWeight = 15F;
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Name = "Column1";
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.TrueValue = "T";
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column2.FalseValue = "F";
            this.Column2.FillWeight = 15F;
            resources.ApplyResources(this.Column2, "Column2");
            this.Column2.Name = "Column2";
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column2.TrueValue = "T";
            // 
            // dataGridViewTextBoxColumn14
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn14, "dataGridViewTextBoxColumn14");
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn16
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn16, "dataGridViewTextBoxColumn16");
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            this.dataGridViewTextBoxColumn16.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column5
            // 
            resources.ApplyResources(this.Column5, "Column5");
            this.Column5.Name = "Column5";
            // 
            // previewTable
            // 
            resources.ApplyResources(this.previewTable, "previewTable");
            this.previewTable.AllowUserToAddRows = false;
            this.previewTable.AllowUserToDeleteRows = false;
            this.previewTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.previewTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.previewTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.previewTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column3,
            this.OriginalTag,
            this.NewTag,
            this.Column4});
            this.previewTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dirtyErrorProvider.SetError(this.previewTable, resources.GetString("previewTable.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.previewTable, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("previewTable.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.previewTable, ((int)(resources.GetObject("previewTable.IconPadding"))));
            this.previewTable.MultiSelect = false;
            this.previewTable.Name = "previewTable";
            this.previewTable.RowHeadersVisible = false;
            this.previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.toolTip1.SetToolTip(this.previewTable, resources.GetString("previewTable.ToolTip"));
            // 
            // Column3
            // 
            this.Column3.FillWeight = 40F;
            resources.ApplyResources(this.Column3, "Column3");
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // OriginalTag
            // 
            this.OriginalTag.FillWeight = 25F;
            resources.ApplyResources(this.OriginalTag, "OriginalTag");
            this.OriginalTag.Name = "OriginalTag";
            // 
            // NewTag
            // 
            this.NewTag.FillWeight = 25F;
            resources.ApplyResources(this.NewTag, "NewTag");
            this.NewTag.Name = "NewTag";
            // 
            // Column4
            // 
            resources.ApplyResources(this.Column4, "Column4");
            this.Column4.Name = "Column4";
            // 
            // dirtyErrorProvider
            // 
            this.dirtyErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.dirtyErrorProvider.ContainerControl = this;
            resources.ApplyResources(this.dirtyErrorProvider, "dirtyErrorProvider");
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.dirtyErrorProvider.SetError(this.buttonOK, resources.GetString("buttonOK.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonOK, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonOK.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonOK, ((int)(resources.GetObject("buttonOK.IconPadding"))));
            this.buttonOK.Name = "buttonOK";
            this.toolTip1.SetToolTip(this.buttonOK, resources.GetString("buttonOK.ToolTip"));
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dirtyErrorProvider.SetError(this.buttonCancel, resources.GetString("buttonCancel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonCancel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonCancel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonCancel, ((int)(resources.GetObject("buttonCancel.IconPadding"))));
            this.buttonCancel.Name = "buttonCancel";
            this.toolTip1.SetToolTip(this.buttonCancel, resources.GetString("buttonCancel.ToolTip"));
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
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
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.dirtyErrorProvider.SetError(this.label3, resources.GetString("label3.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label3.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label3, ((int)(resources.GetObject("label3.IconPadding"))));
            this.label3.Name = "label3";
            this.toolTip1.SetToolTip(this.label3, resources.GetString("label3.ToolTip"));
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
            // templateNameTextBox
            // 
            resources.ApplyResources(this.templateNameTextBox, "templateNameTextBox");
            this.dirtyErrorProvider.SetError(this.templateNameTextBox, resources.GetString("templateNameTextBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.templateNameTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("templateNameTextBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.templateNameTextBox, ((int)(resources.GetObject("templateNameTextBox.IconPadding"))));
            this.templateNameTextBox.Name = "templateNameTextBox";
            this.toolTip1.SetToolTip(this.templateNameTextBox, resources.GetString("templateNameTextBox.ToolTip"));
            this.templateNameTextBox.TextChanged += new System.EventHandler(this.templateNameTextBox_TextChanged);
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
            // sourceTagList
            // 
            resources.ApplyResources(this.sourceTagList, "sourceTagList");
            this.sourceTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceTagList.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.sourceTagList, resources.GetString("sourceTagList.Error"));
            this.sourceTagList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.sourceTagList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("sourceTagList.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.sourceTagList, ((int)(resources.GetObject("sourceTagList.IconPadding"))));
            this.sourceTagList.Name = "sourceTagList";
            this.toolTip1.SetToolTip(this.sourceTagList, resources.GetString("sourceTagList.ToolTip"));
            this.sourceTagList.SelectedIndexChanged += new System.EventHandler(this.sourceTagList_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.dirtyErrorProvider.SetError(this.label2, resources.GetString("label2.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label2, ((int)(resources.GetObject("label2.IconPadding"))));
            this.label2.Name = "label2";
            this.toolTip1.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
            this.label2.Click += new System.EventHandler(this.label2_Click);
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
            // 
            // buttonAdd
            // 
            resources.ApplyResources(this.buttonAdd, "buttonAdd");
            this.dirtyErrorProvider.SetError(this.buttonAdd, resources.GetString("buttonAdd.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonAdd, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonAdd.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonAdd, ((int)(resources.GetObject("buttonAdd.IconPadding"))));
            this.buttonAdd.Name = "buttonAdd";
            this.toolTip1.SetToolTip(this.buttonAdd, resources.GetString("buttonAdd.ToolTip"));
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonDelete
            // 
            resources.ApplyResources(this.buttonDelete, "buttonDelete");
            this.dirtyErrorProvider.SetError(this.buttonDelete, resources.GetString("buttonDelete.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonDelete, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonDelete.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonDelete, ((int)(resources.GetObject("buttonDelete.IconPadding"))));
            this.buttonDelete.Name = "buttonDelete";
            this.toolTip1.SetToolTip(this.buttonDelete, resources.GetString("buttonDelete.ToolTip"));
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonUp
            // 
            resources.ApplyResources(this.buttonUp, "buttonUp");
            this.dirtyErrorProvider.SetError(this.buttonUp, resources.GetString("buttonUp.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonUp, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonUp.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonUp, ((int)(resources.GetObject("buttonUp.IconPadding"))));
            this.buttonUp.Name = "buttonUp";
            this.toolTip1.SetToolTip(this.buttonUp, resources.GetString("buttonUp.ToolTip"));
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonDown
            // 
            resources.ApplyResources(this.buttonDown, "buttonDown");
            this.dirtyErrorProvider.SetError(this.buttonDown, resources.GetString("buttonDown.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonDown, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonDown.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonDown, ((int)(resources.GetObject("buttonDown.IconPadding"))));
            this.buttonDown.Name = "buttonDown";
            this.toolTip1.SetToolTip(this.buttonDown, resources.GetString("buttonDown.ToolTip"));
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // loadComboBox
            // 
            resources.ApplyResources(this.loadComboBox, "loadComboBox");
            this.loadComboBox.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.loadComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.loadComboBox.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.loadComboBox, resources.GetString("loadComboBox.Error"));
            this.loadComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.loadComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("loadComboBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.loadComboBox, ((int)(resources.GetObject("loadComboBox.IconPadding"))));
            this.loadComboBox.Name = "loadComboBox";
            this.loadComboBox.Sorted = true;
            this.toolTip1.SetToolTip(this.loadComboBox, resources.GetString("loadComboBox.ToolTip"));
            this.loadComboBox.SelectedIndexChanged += new System.EventHandler(this.loadComboBox_SelectedIndexChanged);
            // 
            // autoDestinationTagCheckBox
            // 
            resources.ApplyResources(this.autoDestinationTagCheckBox, "autoDestinationTagCheckBox");
            this.dirtyErrorProvider.SetError(this.autoDestinationTagCheckBox, resources.GetString("autoDestinationTagCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.autoDestinationTagCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("autoDestinationTagCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.autoDestinationTagCheckBox, ((int)(resources.GetObject("autoDestinationTagCheckBox.IconPadding"))));
            this.autoDestinationTagCheckBox.Name = "autoDestinationTagCheckBox";
            this.toolTip1.SetToolTip(this.autoDestinationTagCheckBox, resources.GetString("autoDestinationTagCheckBox.ToolTip"));
            this.autoDestinationTagCheckBox.UseVisualStyleBackColor = true;
            this.autoDestinationTagCheckBox.CheckedChanged += new System.EventHandler(this.autoDestinationTagCheckBox_CheckedChanged);
            // 
            // searchOnlyCheckBox
            // 
            resources.ApplyResources(this.searchOnlyCheckBox, "searchOnlyCheckBox");
            this.dirtyErrorProvider.SetError(this.searchOnlyCheckBox, resources.GetString("searchOnlyCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.searchOnlyCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("searchOnlyCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.searchOnlyCheckBox, ((int)(resources.GetObject("searchOnlyCheckBox.IconPadding"))));
            this.searchOnlyCheckBox.Name = "searchOnlyCheckBox";
            this.toolTip1.SetToolTip(this.searchOnlyCheckBox, resources.GetString("searchOnlyCheckBox.ToolTip"));
            this.searchOnlyCheckBox.UseVisualStyleBackColor = true;
            this.searchOnlyCheckBox.CheckedChanged += new System.EventHandler(this.SearchOnlyCheckBox_CheckedChanged);
            // 
            // buttonSettings
            // 
            resources.ApplyResources(this.buttonSettings, "buttonSettings");
            this.dirtyErrorProvider.SetError(this.buttonSettings, resources.GetString("buttonSettings.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonSettings, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonSettings.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonSettings, ((int)(resources.GetObject("buttonSettings.IconPadding"))));
            this.buttonSettings.Image = global::MusicBeePlugin.Properties.Resources.gear_15;
            this.buttonSettings.Name = "buttonSettings";
            this.toolTip1.SetToolTip(this.buttonSettings, resources.GetString("buttonSettings.ToolTip"));
            this.buttonSettings.UseVisualStyleBackColor = true;
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // buttonDeleteSaved
            // 
            resources.ApplyResources(this.buttonDeleteSaved, "buttonDeleteSaved");
            this.buttonDeleteSaved.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dirtyErrorProvider.SetError(this.buttonDeleteSaved, resources.GetString("buttonDeleteSaved.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonDeleteSaved, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonDeleteSaved.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonDeleteSaved, ((int)(resources.GetObject("buttonDeleteSaved.IconPadding"))));
            this.buttonDeleteSaved.Image = global::MusicBeePlugin.Properties.Resources.clear_button_15;
            this.buttonDeleteSaved.Name = "buttonDeleteSaved";
            this.toolTip1.SetToolTip(this.buttonDeleteSaved, resources.GetString("buttonDeleteSaved.ToolTip"));
            this.buttonDeleteSaved.UseVisualStyleBackColor = true;
            this.buttonDeleteSaved.Click += new System.EventHandler(this.buttonDeleteSaved_Click);
            // 
            // autoApplyCheckBox
            // 
            resources.ApplyResources(this.autoApplyCheckBox, "autoApplyCheckBox");
            this.dirtyErrorProvider.SetError(this.autoApplyCheckBox, resources.GetString("autoApplyCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.autoApplyCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("autoApplyCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.autoApplyCheckBox, ((int)(resources.GetObject("autoApplyCheckBox.IconPadding"))));
            this.autoApplyCheckBox.Name = "autoApplyCheckBox";
            this.toolTip1.SetToolTip(this.autoApplyCheckBox, resources.GetString("autoApplyCheckBox.ToolTip"));
            this.autoApplyCheckBox.UseVisualStyleBackColor = false;
            // 
            // autoApplyPictureBox
            // 
            resources.ApplyResources(this.autoApplyPictureBox, "autoApplyPictureBox");
            this.dirtyErrorProvider.SetError(this.autoApplyPictureBox, resources.GetString("autoApplyPictureBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.autoApplyPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("autoApplyPictureBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.autoApplyPictureBox, ((int)(resources.GetObject("autoApplyPictureBox.IconPadding"))));
            this.autoApplyPictureBox.Image = global::MusicBeePlugin.Properties.Resources.auto_applied_presets;
            this.autoApplyPictureBox.Name = "autoApplyPictureBox";
            this.autoApplyPictureBox.TabStop = false;
            this.toolTip1.SetToolTip(this.autoApplyPictureBox, resources.GetString("autoApplyPictureBox.ToolTip"));
            this.autoApplyPictureBox.Click += new System.EventHandler(this.autoApplyPictureBox_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 900;
            this.toolTip1.ReshowDelay = 1000;
            // 
            // MultipleSearchAndReplaceCommand
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonPreview);
            this.Controls.Add(this.buttonSettings);
            this.Controls.Add(this.destinationTagList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.autoDestinationTagCheckBox);
            this.Controls.Add(this.sourceTagList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.autoApplyPictureBox);
            this.Controls.Add(this.autoApplyCheckBox);
            this.Controls.Add(this.buttonDeleteSaved);
            this.Controls.Add(this.searchOnlyCheckBox);
            this.Controls.Add(this.loadComboBox);
            this.Controls.Add(this.buttonDown);
            this.Controls.Add(this.buttonUp);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.templateNameTextBox);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.label3);
            this.HelpButton = true;
            this.Name = "MultipleSearchAndReplaceCommand";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MultipleSearchAndReplaceCommand_FormClosing);
            this.Load += new System.EventHandler(this.MultipleSearchAndReplaceCommand_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.templateTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.autoApplyPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ErrorProvider dirtyErrorProvider;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView templateTable;
        private System.Windows.Forms.DataGridViewComboBoxColumn SearchTag;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridView previewTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTag;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTag;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.PictureBox autoApplyPictureBox;
        private System.Windows.Forms.CheckBox autoApplyCheckBox;
        private System.Windows.Forms.Button buttonDeleteSaved;
        private System.Windows.Forms.CheckBox searchOnlyCheckBox;
        private System.Windows.Forms.CheckBox autoDestinationTagCheckBox;
        private System.Windows.Forms.ComboBox loadComboBox;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.ComboBox destinationTagList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox sourceTagList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox templateNameTextBox;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.Button buttonSettings;
    }
}