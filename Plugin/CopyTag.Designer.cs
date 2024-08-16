namespace MusicBeePlugin
{
    partial class CopyTag
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CopyTag));
            this.copyTagLabel = new System.Windows.Forms.Label();
            this.sourceTagList = new System.Windows.Forms.ComboBox();
            this.forTagLabel = new System.Windows.Forms.Label();
            this.destinationTagList = new System.Windows.Forms.ComboBox();
            this.forSelectedTracksLabel = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.appendCheckBox = new System.Windows.Forms.CheckBox();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.previewTable = new System.Windows.Forms.DataGridView();
            this.File = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Track = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SupposedDestinationTagValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SupposedDestinationTagValueNormalized = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalDestinationTagValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalDestinationTagValueNormalized = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewDestinationTagValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewDestinationTagValueNormalized = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.smartOperationCheckBox = new System.Windows.Forms.CheckBox();
            this.dirtyErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.onlyIfDestinationEmptyCheckBox = new System.Windows.Forms.CheckBox();
            this.addCheckBox = new System.Windows.Forms.CheckBox();
            this.fileNameLabel = new System.Windows.Forms.Label();
            this.fileNameTextBox = new System.Windows.Forms.ComboBox();
            this.appendedTextBox = new System.Windows.Forms.ComboBox();
            this.addedTextBox = new System.Windows.Forms.ComboBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.onlyIfSourceNotEmptyCheckBox = new System.Windows.Forms.CheckBox();
            this.smartOperationCheckBoxLabel = new System.Windows.Forms.Label();
            this.onlyIfDestinationEmptyCheckBoxLabel = new System.Windows.Forms.Label();
            this.onlyIfSourceNotEmptyCheckBoxLabel = new System.Windows.Forms.Label();
            this.appendCheckBoxLabel = new System.Windows.Forms.Label();
            this.addCheckBoxLabel = new System.Windows.Forms.Label();
            this.fieldsPanel = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).BeginInit();
            this.fieldsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // copyTagLabel
            // 
            resources.ApplyResources(this.copyTagLabel, "copyTagLabel");
            this.dirtyErrorProvider.SetError(this.copyTagLabel, resources.GetString("copyTagLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.copyTagLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("copyTagLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.copyTagLabel, ((int)(resources.GetObject("copyTagLabel.IconPadding"))));
            this.copyTagLabel.Name = "copyTagLabel";
            this.copyTagLabel.Tag = "#sourceTagList";
            this.toolTip1.SetToolTip(this.copyTagLabel, resources.GetString("copyTagLabel.ToolTip"));
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
            this.sourceTagList.Tag = "#forTagLabel";
            this.toolTip1.SetToolTip(this.sourceTagList, resources.GetString("sourceTagList.ToolTip"));
            this.sourceTagList.SelectedIndexChanged += new System.EventHandler(this.sourceTagList_SelectedIndexChanged);
            // 
            // forTagLabel
            // 
            resources.ApplyResources(this.forTagLabel, "forTagLabel");
            this.dirtyErrorProvider.SetError(this.forTagLabel, resources.GetString("forTagLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.forTagLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("forTagLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.forTagLabel, ((int)(resources.GetObject("forTagLabel.IconPadding"))));
            this.forTagLabel.Name = "forTagLabel";
            this.forTagLabel.Tag = "#destinationTagList";
            this.toolTip1.SetToolTip(this.forTagLabel, resources.GetString("forTagLabel.ToolTip"));
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
            this.destinationTagList.Tag = "#forSelectedTracksLabel";
            this.toolTip1.SetToolTip(this.destinationTagList, resources.GetString("destinationTagList.ToolTip"));
            this.destinationTagList.SelectedIndexChanged += new System.EventHandler(this.destinationTagList_SelectedIndexChanged);
            // 
            // forSelectedTracksLabel
            // 
            resources.ApplyResources(this.forSelectedTracksLabel, "forSelectedTracksLabel");
            this.dirtyErrorProvider.SetError(this.forSelectedTracksLabel, resources.GetString("forSelectedTracksLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.forSelectedTracksLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("forSelectedTracksLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.forSelectedTracksLabel, ((int)(resources.GetObject("forSelectedTracksLabel.IconPadding"))));
            this.forSelectedTracksLabel.Name = "forSelectedTracksLabel";
            this.toolTip1.SetToolTip(this.forSelectedTracksLabel, resources.GetString("forSelectedTracksLabel.ToolTip"));
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.dirtyErrorProvider.SetError(this.buttonOK, resources.GetString("buttonOK.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonOK, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonOK.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonOK, ((int)(resources.GetObject("buttonOK.IconPadding"))));
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Tag = "#buttonClose";
            this.toolTip1.SetToolTip(this.buttonOK, resources.GetString("buttonOK.ToolTip"));
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonClose
            // 
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dirtyErrorProvider.SetError(this.buttonClose, resources.GetString("buttonClose.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonClose, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonClose.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonClose, ((int)(resources.GetObject("buttonClose.IconPadding"))));
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Tag = "#fieldsPanel@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonClose, resources.GetString("buttonClose.ToolTip"));
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // appendCheckBox
            // 
            resources.ApplyResources(this.appendCheckBox, "appendCheckBox");
            this.dirtyErrorProvider.SetError(this.appendCheckBox, resources.GetString("appendCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.appendCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("appendCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.appendCheckBox, ((int)(resources.GetObject("appendCheckBox.IconPadding"))));
            this.appendCheckBox.Name = "appendCheckBox";
            this.appendCheckBox.Tag = "#appendCheckBoxLabel";
            this.toolTip1.SetToolTip(this.appendCheckBox, resources.GetString("appendCheckBox.ToolTip"));
            this.appendCheckBox.CheckedChanged += new System.EventHandler(this.appendCheckBox_CheckedChanged);
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
            // previewTable
            // 
            resources.ApplyResources(this.previewTable, "previewTable");
            this.previewTable.AllowUserToAddRows = false;
            this.previewTable.AllowUserToDeleteRows = false;
            this.previewTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.previewTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.previewTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.previewTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.previewTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.File,
            this.Track,
            this.SupposedDestinationTagValue,
            this.SupposedDestinationTagValueNormalized,
            this.OriginalDestinationTagValue,
            this.OriginalDestinationTagValueNormalized,
            this.NewDestinationTagValue,
            this.NewDestinationTagValueNormalized});
            this.previewTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dirtyErrorProvider.SetError(this.previewTable, resources.GetString("previewTable.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.previewTable, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("previewTable.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.previewTable, ((int)(resources.GetObject("previewTable.IconPadding"))));
            this.previewTable.MultiSelect = false;
            this.previewTable.Name = "previewTable";
            this.previewTable.RowHeadersVisible = false;
            this.previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.previewTable.Tag = "#CopyTag&CopyTag@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.previewTable, resources.GetString("previewTable.ToolTip"));
            this.previewTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.previewTable_CellContentClick);
            // 
            // File
            // 
            this.File.FillWeight = 1F;
            resources.ApplyResources(this.File, "File");
            this.File.Name = "File";
            // 
            // Track
            // 
            this.Track.FillWeight = 200F;
            resources.ApplyResources(this.Track, "Track");
            this.Track.Name = "Track";
            // 
            // SupposedDestinationTagValue
            // 
            this.SupposedDestinationTagValue.FillWeight = 1F;
            resources.ApplyResources(this.SupposedDestinationTagValue, "SupposedDestinationTagValue");
            this.SupposedDestinationTagValue.Name = "SupposedDestinationTagValue";
            // 
            // SupposedDestinationTagValueNormalized
            // 
            this.SupposedDestinationTagValueNormalized.FillWeight = 1F;
            resources.ApplyResources(this.SupposedDestinationTagValueNormalized, "SupposedDestinationTagValueNormalized");
            this.SupposedDestinationTagValueNormalized.Name = "SupposedDestinationTagValueNormalized";
            // 
            // OriginalDestinationTagValue
            // 
            this.OriginalDestinationTagValue.FillWeight = 1F;
            resources.ApplyResources(this.OriginalDestinationTagValue, "OriginalDestinationTagValue");
            this.OriginalDestinationTagValue.Name = "OriginalDestinationTagValue";
            // 
            // OriginalDestinationTagValueNormalized
            // 
            resources.ApplyResources(this.OriginalDestinationTagValueNormalized, "OriginalDestinationTagValueNormalized");
            this.OriginalDestinationTagValueNormalized.Name = "OriginalDestinationTagValueNormalized";
            // 
            // NewDestinationTagValue
            // 
            this.NewDestinationTagValue.FillWeight = 1F;
            resources.ApplyResources(this.NewDestinationTagValue, "NewDestinationTagValue");
            this.NewDestinationTagValue.Name = "NewDestinationTagValue";
            // 
            // NewDestinationTagValueNormalized
            // 
            resources.ApplyResources(this.NewDestinationTagValueNormalized, "NewDestinationTagValueNormalized");
            this.NewDestinationTagValueNormalized.Name = "NewDestinationTagValueNormalized";
            // 
            // smartOperationCheckBox
            // 
            resources.ApplyResources(this.smartOperationCheckBox, "smartOperationCheckBox");
            this.dirtyErrorProvider.SetError(this.smartOperationCheckBox, resources.GetString("smartOperationCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.smartOperationCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("smartOperationCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.smartOperationCheckBox, ((int)(resources.GetObject("smartOperationCheckBox.IconPadding"))));
            this.smartOperationCheckBox.Name = "smartOperationCheckBox";
            this.smartOperationCheckBox.Tag = "#smartOperationCheckBoxLabel";
            this.toolTip1.SetToolTip(this.smartOperationCheckBox, resources.GetString("smartOperationCheckBox.ToolTip"));
            // 
            // dirtyErrorProvider
            // 
            this.dirtyErrorProvider.BlinkRate = 1000;
            this.dirtyErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
            this.dirtyErrorProvider.ContainerControl = this;
            resources.ApplyResources(this.dirtyErrorProvider, "dirtyErrorProvider");
            // 
            // onlyIfDestinationEmptyCheckBox
            // 
            resources.ApplyResources(this.onlyIfDestinationEmptyCheckBox, "onlyIfDestinationEmptyCheckBox");
            this.dirtyErrorProvider.SetError(this.onlyIfDestinationEmptyCheckBox, resources.GetString("onlyIfDestinationEmptyCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.onlyIfDestinationEmptyCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("onlyIfDestinationEmptyCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.onlyIfDestinationEmptyCheckBox, ((int)(resources.GetObject("onlyIfDestinationEmptyCheckBox.IconPadding"))));
            this.onlyIfDestinationEmptyCheckBox.Name = "onlyIfDestinationEmptyCheckBox";
            this.onlyIfDestinationEmptyCheckBox.Tag = "#onlyIfDestinationEmptyCheckBoxLabel";
            this.toolTip1.SetToolTip(this.onlyIfDestinationEmptyCheckBox, resources.GetString("onlyIfDestinationEmptyCheckBox.ToolTip"));
            // 
            // addCheckBox
            // 
            resources.ApplyResources(this.addCheckBox, "addCheckBox");
            this.dirtyErrorProvider.SetError(this.addCheckBox, resources.GetString("addCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.addCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("addCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.addCheckBox, ((int)(resources.GetObject("addCheckBox.IconPadding"))));
            this.addCheckBox.Name = "addCheckBox";
            this.addCheckBox.Tag = "#addCheckBoxLabel";
            this.toolTip1.SetToolTip(this.addCheckBox, resources.GetString("addCheckBox.ToolTip"));
            this.addCheckBox.CheckedChanged += new System.EventHandler(this.addCheckBox_CheckedChanged);
            // 
            // fileNameLabel
            // 
            resources.ApplyResources(this.fileNameLabel, "fileNameLabel");
            this.dirtyErrorProvider.SetError(this.fileNameLabel, resources.GetString("fileNameLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.fileNameLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("fileNameLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.fileNameLabel, ((int)(resources.GetObject("fileNameLabel.IconPadding"))));
            this.fileNameLabel.Name = "fileNameLabel";
            this.fileNameLabel.Tag = "#fileNameTextBox";
            this.toolTip1.SetToolTip(this.fileNameLabel, resources.GetString("fileNameLabel.ToolTip"));
            // 
            // fileNameTextBox
            // 
            resources.ApplyResources(this.fileNameTextBox, "fileNameTextBox");
            this.fileNameTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.fileNameTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.dirtyErrorProvider.SetError(this.fileNameTextBox, resources.GetString("fileNameTextBox.Error"));
            this.fileNameTextBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.fileNameTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("fileNameTextBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.fileNameTextBox, ((int)(resources.GetObject("fileNameTextBox.IconPadding"))));
            this.fileNameTextBox.Name = "fileNameTextBox";
            this.fileNameTextBox.Tag = "#buttonBrowse";
            this.toolTip1.SetToolTip(this.fileNameTextBox, resources.GetString("fileNameTextBox.ToolTip"));
            this.fileNameTextBox.Leave += new System.EventHandler(this.filenameTextBox_Leave);
            // 
            // appendedTextBox
            // 
            resources.ApplyResources(this.appendedTextBox, "appendedTextBox");
            this.appendedTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.appendedTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.dirtyErrorProvider.SetError(this.appendedTextBox, resources.GetString("appendedTextBox.Error"));
            this.appendedTextBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.appendedTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("appendedTextBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.appendedTextBox, ((int)(resources.GetObject("appendedTextBox.IconPadding"))));
            this.appendedTextBox.Name = "appendedTextBox";
            this.appendedTextBox.Tag = "#fieldsPanel";
            this.toolTip1.SetToolTip(this.appendedTextBox, resources.GetString("appendedTextBox.ToolTip"));
            this.appendedTextBox.Leave += new System.EventHandler(this.appendedTextBox_Leave);
            // 
            // addedTextBox
            // 
            resources.ApplyResources(this.addedTextBox, "addedTextBox");
            this.addedTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.addedTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.dirtyErrorProvider.SetError(this.addedTextBox, resources.GetString("addedTextBox.Error"));
            this.addedTextBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.addedTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("addedTextBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.addedTextBox, ((int)(resources.GetObject("addedTextBox.IconPadding"))));
            this.addedTextBox.Name = "addedTextBox";
            this.addedTextBox.Tag = "#fieldsPanel";
            this.toolTip1.SetToolTip(this.addedTextBox, resources.GetString("addedTextBox.ToolTip"));
            this.addedTextBox.Leave += new System.EventHandler(this.addedTextBox_Leave);
            // 
            // buttonBrowse
            // 
            resources.ApplyResources(this.buttonBrowse, "buttonBrowse");
            this.buttonBrowse.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dirtyErrorProvider.SetError(this.buttonBrowse, resources.GetString("buttonBrowse.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonBrowse, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonBrowse.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonBrowse, ((int)(resources.GetObject("buttonBrowse.IconPadding"))));
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Tag = "#fieldsPanel@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonBrowse, resources.GetString("buttonBrowse.ToolTip"));
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // buttonSettings
            // 
            resources.ApplyResources(this.buttonSettings, "buttonSettings");
            this.dirtyErrorProvider.SetError(this.buttonSettings, resources.GetString("buttonSettings.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonSettings, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonSettings.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonSettings, ((int)(resources.GetObject("buttonSettings.IconPadding"))));
            this.buttonSettings.Image = global::MusicBeePlugin.Properties.Resources.gear_15;
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Tag = "@non-defaultable@square-button";
            this.toolTip1.SetToolTip(this.buttonSettings, resources.GetString("buttonSettings.ToolTip"));
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // onlyIfSourceNotEmptyCheckBox
            // 
            resources.ApplyResources(this.onlyIfSourceNotEmptyCheckBox, "onlyIfSourceNotEmptyCheckBox");
            this.dirtyErrorProvider.SetError(this.onlyIfSourceNotEmptyCheckBox, resources.GetString("onlyIfSourceNotEmptyCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.onlyIfSourceNotEmptyCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("onlyIfSourceNotEmptyCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.onlyIfSourceNotEmptyCheckBox, ((int)(resources.GetObject("onlyIfSourceNotEmptyCheckBox.IconPadding"))));
            this.onlyIfSourceNotEmptyCheckBox.Name = "onlyIfSourceNotEmptyCheckBox";
            this.onlyIfSourceNotEmptyCheckBox.Tag = "#onlyIfSourceNotEmptyCheckBoxLabel";
            this.toolTip1.SetToolTip(this.onlyIfSourceNotEmptyCheckBox, resources.GetString("onlyIfSourceNotEmptyCheckBox.ToolTip"));
            // 
            // smartOperationCheckBoxLabel
            // 
            resources.ApplyResources(this.smartOperationCheckBoxLabel, "smartOperationCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.smartOperationCheckBoxLabel, resources.GetString("smartOperationCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.smartOperationCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("smartOperationCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.smartOperationCheckBoxLabel, ((int)(resources.GetObject("smartOperationCheckBoxLabel.IconPadding"))));
            this.smartOperationCheckBoxLabel.Name = "smartOperationCheckBoxLabel";
            this.toolTip1.SetToolTip(this.smartOperationCheckBoxLabel, resources.GetString("smartOperationCheckBoxLabel.ToolTip"));
            this.smartOperationCheckBoxLabel.Click += new System.EventHandler(this.smartOperationCheckBoxLabel_Click);
            // 
            // onlyIfDestinationEmptyCheckBoxLabel
            // 
            resources.ApplyResources(this.onlyIfDestinationEmptyCheckBoxLabel, "onlyIfDestinationEmptyCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.onlyIfDestinationEmptyCheckBoxLabel, resources.GetString("onlyIfDestinationEmptyCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.onlyIfDestinationEmptyCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("onlyIfDestinationEmptyCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.onlyIfDestinationEmptyCheckBoxLabel, ((int)(resources.GetObject("onlyIfDestinationEmptyCheckBoxLabel.IconPadding"))));
            this.onlyIfDestinationEmptyCheckBoxLabel.Name = "onlyIfDestinationEmptyCheckBoxLabel";
            this.onlyIfDestinationEmptyCheckBoxLabel.Tag = "#onlyIfSourceNotEmptyCheckBox";
            this.toolTip1.SetToolTip(this.onlyIfDestinationEmptyCheckBoxLabel, resources.GetString("onlyIfDestinationEmptyCheckBoxLabel.ToolTip"));
            this.onlyIfDestinationEmptyCheckBoxLabel.Click += new System.EventHandler(this.onlyIfDestinationEmptyCheckBoxLabel_Click);
            // 
            // onlyIfSourceNotEmptyCheckBoxLabel
            // 
            resources.ApplyResources(this.onlyIfSourceNotEmptyCheckBoxLabel, "onlyIfSourceNotEmptyCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.onlyIfSourceNotEmptyCheckBoxLabel, resources.GetString("onlyIfSourceNotEmptyCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.onlyIfSourceNotEmptyCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("onlyIfSourceNotEmptyCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.onlyIfSourceNotEmptyCheckBoxLabel, ((int)(resources.GetObject("onlyIfSourceNotEmptyCheckBoxLabel.IconPadding"))));
            this.onlyIfSourceNotEmptyCheckBoxLabel.Name = "onlyIfSourceNotEmptyCheckBoxLabel";
            this.onlyIfSourceNotEmptyCheckBoxLabel.Tag = "";
            this.toolTip1.SetToolTip(this.onlyIfSourceNotEmptyCheckBoxLabel, resources.GetString("onlyIfSourceNotEmptyCheckBoxLabel.ToolTip"));
            this.onlyIfSourceNotEmptyCheckBoxLabel.Click += new System.EventHandler(this.onlyIfSourceNotEmptyCheckBoxLabel_Click);
            // 
            // appendCheckBoxLabel
            // 
            resources.ApplyResources(this.appendCheckBoxLabel, "appendCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.appendCheckBoxLabel, resources.GetString("appendCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.appendCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("appendCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.appendCheckBoxLabel, ((int)(resources.GetObject("appendCheckBoxLabel.IconPadding"))));
            this.appendCheckBoxLabel.Name = "appendCheckBoxLabel";
            this.toolTip1.SetToolTip(this.appendCheckBoxLabel, resources.GetString("appendCheckBoxLabel.ToolTip"));
            this.appendCheckBoxLabel.Click += new System.EventHandler(this.appendCheckBoxLabel_Click);
            // 
            // addCheckBoxLabel
            // 
            resources.ApplyResources(this.addCheckBoxLabel, "addCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.addCheckBoxLabel, resources.GetString("addCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.addCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("addCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.addCheckBoxLabel, ((int)(resources.GetObject("addCheckBoxLabel.IconPadding"))));
            this.addCheckBoxLabel.Name = "addCheckBoxLabel";
            this.addCheckBoxLabel.Tag = "#fieldsPanel";
            this.toolTip1.SetToolTip(this.addCheckBoxLabel, resources.GetString("addCheckBoxLabel.ToolTip"));
            this.addCheckBoxLabel.Click += new System.EventHandler(this.addCheckBoxLabel_Click);
            // 
            // fieldsPanel
            // 
            resources.ApplyResources(this.fieldsPanel, "fieldsPanel");
            this.fieldsPanel.Controls.Add(this.addedTextBox);
            this.fieldsPanel.Controls.Add(this.appendedTextBox);
            this.fieldsPanel.Controls.Add(this.buttonBrowse);
            this.fieldsPanel.Controls.Add(this.fileNameTextBox);
            this.fieldsPanel.Controls.Add(this.fileNameLabel);
            this.fieldsPanel.Controls.Add(this.buttonClose);
            this.fieldsPanel.Controls.Add(this.buttonOK);
            this.fieldsPanel.Controls.Add(this.buttonPreview);
            this.fieldsPanel.Controls.Add(this.buttonSettings);
            this.dirtyErrorProvider.SetError(this.fieldsPanel, resources.GetString("fieldsPanel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.fieldsPanel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("fieldsPanel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.fieldsPanel, ((int)(resources.GetObject("fieldsPanel.IconPadding"))));
            this.fieldsPanel.Name = "fieldsPanel";
            this.fieldsPanel.Tag = "#CopyTag&previewTable@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.fieldsPanel, resources.GetString("fieldsPanel.ToolTip"));
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.FillWeight = 1F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 50F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.FillWeight = 1F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.FillWeight = 1F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.FillWeight = 1F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.FillWeight = 25F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.FillWeight = 1F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn7, "dataGridViewTextBoxColumn7");
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.FillWeight = 25F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn8, "dataGridViewTextBoxColumn8");
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // CopyTag
            // 
            this.AcceptButton = this.buttonPreview;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.Controls.Add(this.previewTable);
            this.Controls.Add(this.addCheckBoxLabel);
            this.Controls.Add(this.addCheckBox);
            this.Controls.Add(this.appendCheckBoxLabel);
            this.Controls.Add(this.appendCheckBox);
            this.Controls.Add(this.onlyIfSourceNotEmptyCheckBoxLabel);
            this.Controls.Add(this.onlyIfSourceNotEmptyCheckBox);
            this.Controls.Add(this.onlyIfDestinationEmptyCheckBoxLabel);
            this.Controls.Add(this.onlyIfDestinationEmptyCheckBox);
            this.Controls.Add(this.smartOperationCheckBoxLabel);
            this.Controls.Add(this.smartOperationCheckBox);
            this.Controls.Add(this.forSelectedTracksLabel);
            this.Controls.Add(this.destinationTagList);
            this.Controls.Add(this.forTagLabel);
            this.Controls.Add(this.sourceTagList);
            this.Controls.Add(this.copyTagLabel);
            this.Controls.Add(this.fieldsPanel);
            this.DoubleBuffered = true;
            this.Name = "CopyTag";
            this.Tag = "";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CopyTag_FormClosing);
            this.Load += new System.EventHandler(this.CopyTag_Load);
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).EndInit();
            this.fieldsPanel.ResumeLayout(false);
            this.fieldsPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label copyTagLabel;
        private System.Windows.Forms.ComboBox sourceTagList;
        private System.Windows.Forms.Label forTagLabel;
        private System.Windows.Forms.ComboBox destinationTagList;
        private System.Windows.Forms.Label forSelectedTracksLabel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.CheckBox appendCheckBox;
        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.DataGridView previewTable;
        private System.Windows.Forms.CheckBox smartOperationCheckBox;
        private System.Windows.Forms.ErrorProvider dirtyErrorProvider;
        private System.Windows.Forms.CheckBox onlyIfDestinationEmptyCheckBox;
        private System.Windows.Forms.CheckBox addCheckBox;
        private System.Windows.Forms.Label fileNameLabel;
        private System.Windows.Forms.ComboBox addedTextBox;
        private System.Windows.Forms.ComboBox appendedTextBox;
        private System.Windows.Forms.ComboBox fileNameTextBox;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox onlyIfSourceNotEmptyCheckBox;
        private System.Windows.Forms.Label smartOperationCheckBoxLabel;
        private System.Windows.Forms.Label onlyIfDestinationEmptyCheckBoxLabel;
        private System.Windows.Forms.Label onlyIfSourceNotEmptyCheckBoxLabel;
        private System.Windows.Forms.Label appendCheckBoxLabel;
        private System.Windows.Forms.Label addCheckBoxLabel;
        private System.Windows.Forms.Panel fieldsPanel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn File;
        private System.Windows.Forms.DataGridViewTextBoxColumn Track;
        private System.Windows.Forms.DataGridViewTextBoxColumn SupposedDestinationTagValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn SupposedDestinationTagValueNormalized;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalDestinationTagValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalDestinationTagValueNormalized;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewDestinationTagValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewDestinationTagValueNormalized;
    }
}