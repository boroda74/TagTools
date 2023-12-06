namespace MusicBeePlugin
{
    partial class MultipleSearchAndReplaceCommand
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultipleSearchAndReplaceCommand));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.templateTable = new System.Windows.Forms.DataGridView();
            this.SearchTag = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.RegEx = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CaseSensitive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.SearchFor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReplaceWith = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Position = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.previewTable = new System.Windows.Forms.DataGridView();
            this.Track = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalTag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewTag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dirtyErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.presetLabel = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.templateNameTextBox = new System.Windows.Forms.TextBox();
            this.fromTagLabel = new System.Windows.Forms.Label();
            this.sourceTagList = new System.Windows.Forms.ComboBox();
            this.toTagLabel = new System.Windows.Forms.Label();
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
            this.templateTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.templateTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.templateTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.templateTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SearchTag,
            this.RegEx,
            this.CaseSensitive,
            this.SearchFor,
            this.ReplaceWith,
            this.Position});
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
            // RegEx
            // 
            this.RegEx.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.RegEx.FalseValue = "F";
            this.RegEx.FillWeight = 15F;
            resources.ApplyResources(this.RegEx, "RegEx");
            this.RegEx.Name = "RegEx";
            this.RegEx.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.RegEx.TrueValue = "T";
            // 
            // CaseSensitive
            // 
            this.CaseSensitive.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.CaseSensitive.FalseValue = "F";
            this.CaseSensitive.FillWeight = 15F;
            resources.ApplyResources(this.CaseSensitive, "CaseSensitive");
            this.CaseSensitive.Name = "CaseSensitive";
            this.CaseSensitive.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CaseSensitive.TrueValue = "T";
            // 
            // SearchFor
            // 
            resources.ApplyResources(this.SearchFor, "SearchFor");
            this.SearchFor.Name = "SearchFor";
            this.SearchFor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ReplaceWith
            // 
            resources.ApplyResources(this.ReplaceWith, "ReplaceWith");
            this.ReplaceWith.Name = "ReplaceWith";
            this.ReplaceWith.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Position
            // 
            resources.ApplyResources(this.Position, "Position");
            this.Position.Name = "Position";
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
            this.Track,
            this.OriginalTag,
            this.NewTag,
            this.FileColumn});
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
            // Track
            // 
            this.Track.FillWeight = 40F;
            resources.ApplyResources(this.Track, "Track");
            this.Track.Name = "Track";
            this.Track.ReadOnly = true;
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
            // FileColumn
            // 
            resources.ApplyResources(this.FileColumn, "FileColumn");
            this.FileColumn.Name = "FileColumn";
            // 
            // dirtyErrorProvider
            // 
            this.dirtyErrorProvider.BlinkRate = 1000;
            this.dirtyErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
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
            this.buttonOK.Tag = "#buttonCancel";
            this.toolTip1.SetToolTip(this.buttonOK, resources.GetString("buttonOK.ToolTip"));
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
            this.buttonCancel.Tag = "#MultipleSearchAndReplaceCommand@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonCancel, resources.GetString("buttonCancel.ToolTip"));
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonPreview
            // 
            resources.ApplyResources(this.buttonPreview, "buttonPreview");
            this.dirtyErrorProvider.SetError(this.buttonPreview, resources.GetString("buttonPreview.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonPreview, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonPreview.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonPreview, ((int)(resources.GetObject("buttonPreview.IconPadding"))));
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Tag = "#buttonOK";
            this.toolTip1.SetToolTip(this.buttonPreview, resources.GetString("buttonPreview.ToolTip"));
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // presetLabel
            // 
            resources.ApplyResources(this.presetLabel, "presetLabel");
            this.dirtyErrorProvider.SetError(this.presetLabel, resources.GetString("presetLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.presetLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("presetLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.presetLabel, ((int)(resources.GetObject("presetLabel.IconPadding"))));
            this.presetLabel.Name = "presetLabel";
            this.presetLabel.Tag = "#templateNameTextBox";
            this.toolTip1.SetToolTip(this.presetLabel, resources.GetString("presetLabel.ToolTip"));
            // 
            // buttonSave
            // 
            resources.ApplyResources(this.buttonSave, "buttonSave");
            this.buttonSave.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dirtyErrorProvider.SetError(this.buttonSave, resources.GetString("buttonSave.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonSave, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonSave.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonSave, ((int)(resources.GetObject("buttonSave.IconPadding"))));
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Tag = "#loadComboBox@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonSave, resources.GetString("buttonSave.ToolTip"));
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // templateNameTextBox
            // 
            resources.ApplyResources(this.templateNameTextBox, "templateNameTextBox");
            this.dirtyErrorProvider.SetError(this.templateNameTextBox, resources.GetString("templateNameTextBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.templateNameTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("templateNameTextBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.templateNameTextBox, ((int)(resources.GetObject("templateNameTextBox.IconPadding"))));
            this.templateNameTextBox.Name = "templateNameTextBox";
            this.templateNameTextBox.Tag = "#autoApplyCheckBox";
            this.toolTip1.SetToolTip(this.templateNameTextBox, resources.GetString("templateNameTextBox.ToolTip"));
            this.templateNameTextBox.TextChanged += new System.EventHandler(this.templateNameTextBox_TextChanged);
            // 
            // fromTagLabel
            // 
            resources.ApplyResources(this.fromTagLabel, "fromTagLabel");
            this.dirtyErrorProvider.SetError(this.fromTagLabel, resources.GetString("fromTagLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.fromTagLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("fromTagLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.fromTagLabel, ((int)(resources.GetObject("fromTagLabel.IconPadding"))));
            this.fromTagLabel.Name = "fromTagLabel";
            this.fromTagLabel.Tag = "#sourceTagList";
            this.toolTip1.SetToolTip(this.fromTagLabel, resources.GetString("fromTagLabel.ToolTip"));
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
            this.sourceTagList.Tag = "#autoDestinationTagCheckBox";
            this.toolTip1.SetToolTip(this.sourceTagList, resources.GetString("sourceTagList.ToolTip"));
            this.sourceTagList.SelectedIndexChanged += new System.EventHandler(this.sourceTagList_SelectedIndexChanged);
            // 
            // toTagLabel
            // 
            resources.ApplyResources(this.toTagLabel, "toTagLabel");
            this.dirtyErrorProvider.SetError(this.toTagLabel, resources.GetString("toTagLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.toTagLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("toTagLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.toTagLabel, ((int)(resources.GetObject("toTagLabel.IconPadding"))));
            this.toTagLabel.Name = "toTagLabel";
            this.toTagLabel.Tag = "#destinationTagList";
            this.toolTip1.SetToolTip(this.toTagLabel, resources.GetString("toTagLabel.ToolTip"));
            this.toTagLabel.Click += new System.EventHandler(this.label2_Click);
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
            this.buttonAdd.Tag = "#buttonDelete@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonAdd, resources.GetString("buttonAdd.ToolTip"));
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonDelete
            // 
            resources.ApplyResources(this.buttonDelete, "buttonDelete");
            this.dirtyErrorProvider.SetError(this.buttonDelete, resources.GetString("buttonDelete.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonDelete, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonDelete.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonDelete, ((int)(resources.GetObject("buttonDelete.IconPadding"))));
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Tag = "@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonDelete, resources.GetString("buttonDelete.ToolTip"));
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonUp
            // 
            resources.ApplyResources(this.buttonUp, "buttonUp");
            this.dirtyErrorProvider.SetError(this.buttonUp, resources.GetString("buttonUp.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonUp, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonUp.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonUp, ((int)(resources.GetObject("buttonUp.IconPadding"))));
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Tag = "#buttonDown@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonUp, resources.GetString("buttonUp.ToolTip"));
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonDown
            // 
            resources.ApplyResources(this.buttonDown, "buttonDown");
            this.dirtyErrorProvider.SetError(this.buttonDown, resources.GetString("buttonDown.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonDown, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonDown.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonDown, ((int)(resources.GetObject("buttonDown.IconPadding"))));
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Tag = "#buttonAdd@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonDown, resources.GetString("buttonDown.ToolTip"));
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
            this.loadComboBox.Tag = "#buttonDeleteSaved";
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
            this.autoDestinationTagCheckBox.Tag = "#toTagLlabel";
            this.toolTip1.SetToolTip(this.autoDestinationTagCheckBox, resources.GetString("autoDestinationTagCheckBox.ToolTip"));
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
            this.buttonDeleteSaved.Tag = "#MultipleSearchAndReplaceCommand@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonDeleteSaved, resources.GetString("buttonDeleteSaved.ToolTip"));
            this.buttonDeleteSaved.Click += new System.EventHandler(this.buttonDeleteSaved_Click);
            // 
            // autoApplyCheckBox
            // 
            resources.ApplyResources(this.autoApplyCheckBox, "autoApplyCheckBox");
            this.dirtyErrorProvider.SetError(this.autoApplyCheckBox, resources.GetString("autoApplyCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.autoApplyCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("autoApplyCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.autoApplyCheckBox, ((int)(resources.GetObject("autoApplyCheckBox.IconPadding"))));
            this.autoApplyCheckBox.Name = "autoApplyCheckBox";
            this.autoApplyCheckBox.Tag = "#autoApplyPictureBox";
            this.toolTip1.SetToolTip(this.autoApplyCheckBox, resources.GetString("autoApplyCheckBox.ToolTip"));
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
            this.autoApplyPictureBox.Tag = "";
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
            this.AcceptButton = this.buttonPreview;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.buttonDeleteSaved);
            this.Controls.Add(this.loadComboBox);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.autoApplyPictureBox);
            this.Controls.Add(this.autoApplyCheckBox);
            this.Controls.Add(this.templateNameTextBox);
            this.Controls.Add(this.presetLabel);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.buttonDown);
            this.Controls.Add(this.buttonUp);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonPreview);
            this.Controls.Add(this.buttonSettings);
            this.Controls.Add(this.searchOnlyCheckBox);
            this.Controls.Add(this.destinationTagList);
            this.Controls.Add(this.toTagLabel);
            this.Controls.Add(this.autoDestinationTagCheckBox);
            this.Controls.Add(this.sourceTagList);
            this.Controls.Add(this.fromTagLabel);
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
        private System.Windows.Forms.DataGridView previewTable;
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
        private System.Windows.Forms.Label toTagLabel;
        private System.Windows.Forms.ComboBox sourceTagList;
        private System.Windows.Forms.Label fromTagLabel;
        private System.Windows.Forms.TextBox templateNameTextBox;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label presetLabel;
        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.DataGridViewComboBoxColumn SearchTag;
        private System.Windows.Forms.DataGridViewCheckBoxColumn RegEx;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CaseSensitive;
        private System.Windows.Forms.DataGridViewTextBoxColumn SearchFor;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReplaceWith;
        private System.Windows.Forms.DataGridViewTextBoxColumn Position;
        private System.Windows.Forms.DataGridViewTextBoxColumn Track;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTag;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTag;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileColumn;
    }
}