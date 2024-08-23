namespace MusicBeePlugin
{
    partial class MultipleSearchAndReplace
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
            if (disposing)
            {
                templateNameTextBox.Parent?.Dispose();
            }

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultipleSearchAndReplace));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.templateTable = new System.Windows.Forms.DataGridView();
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
            this.buttonClose = new System.Windows.Forms.Button();
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
            this.buttonSettings = new System.Windows.Forms.Button();
            this.buttonDeleteSaved = new System.Windows.Forms.Button();
            this.smartOperationCheckBox = new System.Windows.Forms.CheckBox();
            this.autoApplyCheckBox = new System.Windows.Forms.CheckBox();
            this.autoApplyPictureBox = new System.Windows.Forms.PictureBox();
            this.controlsPanel = new System.Windows.Forms.Panel();
            this.smartOperationCheckBoxLabel = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dataGridViewComboBoxColumn1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();

            //MusicBee
            this.templateNameTextBox = ControlsTools.CreateMusicBeeTextBox();
            //~MusicBee            

            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.templateTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.autoApplyPictureBox)).BeginInit();
            this.controlsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.BackColor = System.Drawing.SystemColors.GrayText;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.Panel1.Controls.Add(this.templateTable);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.Panel2.Controls.Add(this.previewTable);
            this.splitContainer1.Tag = "#MultipleSearchAndReplace&MultipleSearchAndReplace@pinned-to-parent-x";
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // templateTable
            // 
            this.templateTable.AllowUserToAddRows = false;
            this.templateTable.AllowUserToDeleteRows = false;
            this.templateTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.templateTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.templateTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.templateTable, "templateTable");
            this.templateTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RegEx,
            this.CaseSensitive,
            this.SearchFor,
            this.ReplaceWith,
            this.Position});
            this.dirtyErrorProvider.SetIconAlignment(this.templateTable, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("templateTable.IconAlignment"))));
            this.templateTable.MultiSelect = false;
            this.templateTable.Name = "templateTable";
            this.templateTable.RowHeadersVisible = false;
            this.templateTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // RegEx
            // 
            this.RegEx.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.RegEx.FalseValue = "F";
            this.RegEx.FillWeight = 1F;
            resources.ApplyResources(this.RegEx, "RegEx");
            this.RegEx.Name = "RegEx";
            this.RegEx.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.RegEx.TrueValue = "T";
            // 
            // CaseSensitive
            // 
            this.CaseSensitive.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.CaseSensitive.FalseValue = "F";
            this.CaseSensitive.FillWeight = 1F;
            resources.ApplyResources(this.CaseSensitive, "CaseSensitive");
            this.CaseSensitive.Name = "CaseSensitive";
            this.CaseSensitive.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CaseSensitive.TrueValue = "T";
            // 
            // SearchFor
            // 
            this.SearchFor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.SearchFor, "SearchFor");
            this.SearchFor.Name = "SearchFor";
            this.SearchFor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ReplaceWith
            // 
            this.ReplaceWith.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.ReplaceWith, "ReplaceWith");
            this.ReplaceWith.Name = "ReplaceWith";
            this.ReplaceWith.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Position
            // 
            this.Position.FillWeight = 1F;
            resources.ApplyResources(this.Position, "Position");
            this.Position.Name = "Position";
            this.Position.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Position.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // previewTable
            // 
            this.previewTable.AllowUserToAddRows = false;
            this.previewTable.AllowUserToDeleteRows = false;
            this.previewTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.previewTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.previewTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.previewTable, "previewTable");
            this.previewTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Track,
            this.OriginalTag,
            this.NewTag,
            this.FileColumn});
            this.dirtyErrorProvider.SetIconAlignment(this.previewTable, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("previewTable.IconAlignment"))));
            this.previewTable.MultiSelect = false;
            this.previewTable.Name = "previewTable";
            this.previewTable.RowHeadersVisible = false;
            this.previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // Track
            // 
            this.Track.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Track.FillWeight = 40F;
            resources.ApplyResources(this.Track, "Track");
            this.Track.Name = "Track";
            this.Track.ReadOnly = true;
            // 
            // OriginalTag
            // 
            this.OriginalTag.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.OriginalTag.FillWeight = 25F;
            resources.ApplyResources(this.OriginalTag, "OriginalTag");
            this.OriginalTag.Name = "OriginalTag";
            this.OriginalTag.ReadOnly = true;
            // 
            // NewTag
            // 
            this.NewTag.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NewTag.FillWeight = 25F;
            resources.ApplyResources(this.NewTag, "NewTag");
            this.NewTag.Name = "NewTag";
            // 
            // FileColumn
            // 
            this.FileColumn.FillWeight = 1F;
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
            this.dirtyErrorProvider.SetIconAlignment(this.buttonOK, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonOK.IconAlignment"))));
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Tag = "#buttonClose";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonClose
            // 
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dirtyErrorProvider.SetIconAlignment(this.buttonClose, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonClose.IconAlignment"))));
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Tag = "#controlsPanel@non-defaultable";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonPreview
            // 
            resources.ApplyResources(this.buttonPreview, "buttonPreview");
            this.dirtyErrorProvider.SetError(this.buttonPreview, resources.GetString("buttonPreview.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonPreview, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonPreview.IconAlignment"))));
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Tag = "#buttonOK";
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // presetLabel
            // 
            resources.ApplyResources(this.presetLabel, "presetLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.presetLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("presetLabel.IconAlignment"))));
            this.presetLabel.Name = "presetLabel";
            this.presetLabel.Tag = "#templateNameTextBox";
            this.toolTip1.SetToolTip(this.presetLabel, resources.GetString("presetLabel.ToolTip"));
            // 
            // buttonSave
            // 
            resources.ApplyResources(this.buttonSave, "buttonSave");
            this.buttonSave.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dirtyErrorProvider.SetIconAlignment(this.buttonSave, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonSave.IconAlignment"))));
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Tag = "#loadComboBox@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonSave, resources.GetString("buttonSave.ToolTip"));
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // templateNameTextBox
            // 
            resources.ApplyResources(this.templateNameTextBox, "templateNameTextBox");
            this.dirtyErrorProvider.SetIconAlignment(this.templateNameTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("templateNameTextBox.IconAlignment"))));
            this.templateNameTextBox.Name = "templateNameTextBox";
            this.templateNameTextBox.Tag = "#autoApplyCheckBox";
            this.toolTip1.SetToolTip(this.templateNameTextBox, resources.GetString("templateNameTextBox.ToolTip"));
            this.templateNameTextBox.TextChanged += new System.EventHandler(this.templateNameTextBox_TextChanged);
            // 
            // fromTagLabel
            // 
            resources.ApplyResources(this.fromTagLabel, "fromTagLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.fromTagLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("fromTagLabel.IconAlignment"))));
            this.fromTagLabel.Name = "fromTagLabel";
            this.fromTagLabel.Tag = "#sourceTagList@pinned-to-parent-x";
            // 
            // sourceTagList
            // 
            this.sourceTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceTagList.DropDownWidth = 250;
            this.sourceTagList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.sourceTagList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("sourceTagList.IconAlignment"))));
            resources.ApplyResources(this.sourceTagList, "sourceTagList");
            this.sourceTagList.Name = "sourceTagList";
            this.sourceTagList.Tag = "#autoDestinationTagCheckBox";
            this.sourceTagList.SelectedIndexChanged += new System.EventHandler(this.sourceTagList_SelectedIndexChanged);
            // 
            // toTagLabel
            // 
            resources.ApplyResources(this.toTagLabel, "toTagLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.toTagLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("toTagLabel.IconAlignment"))));
            this.toTagLabel.Name = "toTagLabel";
            this.toTagLabel.Tag = "#destinationTagList";
            this.toTagLabel.Click += new System.EventHandler(this.label2_Click);
            // 
            // destinationTagList
            // 
            this.destinationTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.destinationTagList.DropDownWidth = 250;
            this.destinationTagList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.destinationTagList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("destinationTagList.IconAlignment"))));
            resources.ApplyResources(this.destinationTagList, "destinationTagList");
            this.destinationTagList.Name = "destinationTagList";
            this.destinationTagList.Tag = "#smartOperationCheckBox";
            // 
            // buttonAdd
            // 
            this.dirtyErrorProvider.SetIconAlignment(this.buttonAdd, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonAdd.IconAlignment"))));
            resources.ApplyResources(this.buttonAdd, "buttonAdd");
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Tag = "#buttonDelete@non-defaultable";
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonDelete
            // 
            this.dirtyErrorProvider.SetIconAlignment(this.buttonDelete, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonDelete.IconAlignment"))));
            resources.ApplyResources(this.buttonDelete, "buttonDelete");
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Tag = "#presetLabel@non-defaultable";
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonUp
            // 
            resources.ApplyResources(this.buttonUp, "buttonUp");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonUp, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonUp.IconAlignment"))));
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Tag = "#buttonDown@non-defaultable@square-button@pinned-to-parent-x";
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonDown
            // 
            resources.ApplyResources(this.buttonDown, "buttonDown");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonDown, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonDown.IconAlignment"))));
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Tag = "#buttonAdd@non-defaultable@square-button";
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // loadComboBox
            // 
            resources.ApplyResources(this.loadComboBox, "loadComboBox");
            this.loadComboBox.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.loadComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.loadComboBox.DropDownWidth = 250;
            this.loadComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.loadComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("loadComboBox.IconAlignment"))));
            this.loadComboBox.Name = "loadComboBox";
            this.loadComboBox.Sorted = true;
            this.loadComboBox.Tag = "#buttonDeleteSaved";
            this.toolTip1.SetToolTip(this.loadComboBox, resources.GetString("loadComboBox.ToolTip"));
            this.loadComboBox.SelectedIndexChanged += new System.EventHandler(this.loadComboBox_SelectedIndexChanged);
            // 
            // autoDestinationTagCheckBox
            // 
            resources.ApplyResources(this.autoDestinationTagCheckBox, "autoDestinationTagCheckBox");
            this.dirtyErrorProvider.SetIconAlignment(this.autoDestinationTagCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("autoDestinationTagCheckBox.IconAlignment"))));
            this.autoDestinationTagCheckBox.Name = "autoDestinationTagCheckBox";
            this.autoDestinationTagCheckBox.Tag = "#toTagLabel";
            this.toolTip1.SetToolTip(this.autoDestinationTagCheckBox, resources.GetString("autoDestinationTagCheckBox.ToolTip"));
            this.autoDestinationTagCheckBox.CheckedChanged += new System.EventHandler(this.autoDestinationTagCheckBox_CheckedChanged);
            // 
            // buttonSettings
            // 
            resources.ApplyResources(this.buttonSettings, "buttonSettings");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonSettings, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonSettings.IconAlignment"))));
            this.buttonSettings.Image = global::MusicBeePlugin.Properties.Resources.gear_15;
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Tag = "#buttonPreview@square-button";
            this.toolTip1.SetToolTip(this.buttonSettings, resources.GetString("buttonSettings.ToolTip"));
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // buttonDeleteSaved
            // 
            resources.ApplyResources(this.buttonDeleteSaved, "buttonDeleteSaved");
            this.buttonDeleteSaved.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dirtyErrorProvider.SetIconAlignment(this.buttonDeleteSaved, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonDeleteSaved.IconAlignment"))));
            this.buttonDeleteSaved.Name = "buttonDeleteSaved";
            this.buttonDeleteSaved.Tag = "#controlsPanel@non-defaultable@square-button";
            this.toolTip1.SetToolTip(this.buttonDeleteSaved, resources.GetString("buttonDeleteSaved.ToolTip"));
            this.buttonDeleteSaved.Click += new System.EventHandler(this.buttonDeleteSaved_Click);
            // 
            // smartOperationCheckBox
            // 
            resources.ApplyResources(this.smartOperationCheckBox, "smartOperationCheckBox");
            this.dirtyErrorProvider.SetIconAlignment(this.smartOperationCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("smartOperationCheckBox.IconAlignment"))));
            this.smartOperationCheckBox.Name = "smartOperationCheckBox";
            this.smartOperationCheckBox.Tag = "#smartOperationCheckBoxLabel";
            this.toolTip1.SetToolTip(this.smartOperationCheckBox, resources.GetString("smartOperationCheckBox.ToolTip"));
            // 
            // autoApplyCheckBox
            // 
            resources.ApplyResources(this.autoApplyCheckBox, "autoApplyCheckBox");
            this.autoApplyCheckBox.Name = "autoApplyCheckBox";
            this.autoApplyCheckBox.Tag = "#buttonSave";
            this.toolTip1.SetToolTip(this.autoApplyCheckBox, resources.GetString("autoApplyCheckBox.ToolTip"));
            // 
            // autoApplyPictureBox
            // 
            resources.ApplyResources(this.autoApplyPictureBox, "autoApplyPictureBox");
            this.autoApplyPictureBox.Image = global::MusicBeePlugin.Properties.Resources.auto_applied_presets;
            this.autoApplyPictureBox.Name = "autoApplyPictureBox";
            this.autoApplyPictureBox.TabStop = false;
            this.autoApplyPictureBox.Tag = "#buttonSave@square-control";
            this.toolTip1.SetToolTip(this.autoApplyPictureBox, resources.GetString("autoApplyPictureBox.ToolTip"));
            this.autoApplyPictureBox.Click += new System.EventHandler(this.autoApplyPictureBox_Click);
            // 
            // controlsPanel
            // 
            resources.ApplyResources(this.controlsPanel, "controlsPanel");
            this.controlsPanel.Controls.Add(this.smartOperationCheckBoxLabel);
            this.controlsPanel.Controls.Add(this.smartOperationCheckBox);
            this.controlsPanel.Controls.Add(this.buttonDeleteSaved);
            this.controlsPanel.Controls.Add(this.loadComboBox);
            this.controlsPanel.Controls.Add(this.buttonSave);
            this.controlsPanel.Controls.Add(this.autoApplyPictureBox);
            this.controlsPanel.Controls.Add(this.autoApplyCheckBox);
            this.controlsPanel.Controls.Add(this.templateNameTextBox);
            this.controlsPanel.Controls.Add(this.presetLabel);
            this.controlsPanel.Controls.Add(this.buttonDelete);
            this.controlsPanel.Controls.Add(this.buttonAdd);
            this.controlsPanel.Controls.Add(this.buttonDown);
            this.controlsPanel.Controls.Add(this.buttonUp);
            this.controlsPanel.Controls.Add(this.buttonClose);
            this.controlsPanel.Controls.Add(this.buttonOK);
            this.controlsPanel.Controls.Add(this.buttonPreview);
            this.controlsPanel.Controls.Add(this.buttonSettings);
            this.controlsPanel.Controls.Add(this.destinationTagList);
            this.controlsPanel.Controls.Add(this.toTagLabel);
            this.controlsPanel.Controls.Add(this.autoDestinationTagCheckBox);
            this.controlsPanel.Controls.Add(this.sourceTagList);
            this.controlsPanel.Controls.Add(this.fromTagLabel);
            this.controlsPanel.Name = "controlsPanel";
            this.controlsPanel.Tag = "#MultipleSearchAndReplace&splitContainer1@pinned-to-parent-x";
            // 
            // smartOperationCheckBoxLabel
            // 
            resources.ApplyResources(this.smartOperationCheckBoxLabel, "smartOperationCheckBoxLabel");
            this.smartOperationCheckBoxLabel.Name = "smartOperationCheckBoxLabel";
            this.toolTip1.SetToolTip(this.smartOperationCheckBoxLabel, resources.GetString("smartOperationCheckBoxLabel.ToolTip"));
            this.smartOperationCheckBoxLabel.Click += new System.EventHandler(this.smartOperationCheckBoxLabel_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 900;
            this.toolTip1.ReshowDelay = 1000;
            // 
            // dataGridViewComboBoxColumn1
            // 
            this.dataGridViewComboBoxColumn1.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.dataGridViewComboBoxColumn1.FillWeight = 60F;
            resources.ApplyResources(this.dataGridViewComboBoxColumn1, "dataGridViewComboBoxColumn1");
            this.dataGridViewComboBoxColumn1.MaxDropDownItems = 15;
            this.dataGridViewComboBoxColumn1.Name = "dataGridViewComboBoxColumn1";
            this.dataGridViewComboBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewCheckBoxColumn1.FalseValue = "F";
            this.dataGridViewCheckBoxColumn1.FillWeight = 15F;
            resources.ApplyResources(this.dataGridViewCheckBoxColumn1, "dataGridViewCheckBoxColumn1");
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumn1.TrueValue = "T";
            // 
            // dataGridViewCheckBoxColumn2
            // 
            this.dataGridViewCheckBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewCheckBoxColumn2.FalseValue = "F";
            this.dataGridViewCheckBoxColumn2.FillWeight = 15F;
            resources.ApplyResources(this.dataGridViewCheckBoxColumn2, "dataGridViewCheckBoxColumn2");
            this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
            this.dataGridViewCheckBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumn2.TrueValue = "T";
            // 
            // dataGridViewTextBoxColumn1
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn3
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.FillWeight = 40F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.FillWeight = 25F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.FillWeight = 25F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn7, "dataGridViewTextBoxColumn7");
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // MultipleSearchAndReplace
            // 
            this.AcceptButton = this.buttonPreview;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.controlsPanel);
            this.DoubleBuffered = true;
            this.HelpButton = true;
            this.Name = "MultipleSearchAndReplace";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MultipleSearchAndReplace_FormClosing);
            this.Load += new System.EventHandler(this.MultipleSearchAndReplace_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.templateTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.autoApplyPictureBox)).EndInit();
            this.controlsPanel.ResumeLayout(false);
            this.controlsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ErrorProvider dirtyErrorProvider;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView templateTable;
        private System.Windows.Forms.DataGridView previewTable;
        private System.Windows.Forms.PictureBox autoApplyPictureBox;
        private System.Windows.Forms.CheckBox autoApplyCheckBox;
        private System.Windows.Forms.Button buttonDeleteSaved;
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
        private System.Windows.Forms.Panel controlsPanel;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Track;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTag;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTag;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileColumn;
        private System.Windows.Forms.Label smartOperationCheckBoxLabel;
        private System.Windows.Forms.CheckBox smartOperationCheckBox;
        private System.Windows.Forms.DataGridViewCheckBoxColumn RegEx;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CaseSensitive;
        private System.Windows.Forms.DataGridViewTextBoxColumn SearchFor;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReplaceWith;
        private System.Windows.Forms.DataGridViewTextBoxColumn Position;
    }
}