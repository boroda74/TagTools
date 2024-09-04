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

            if (disposing)
                source?.Dispose();
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
            this.dirtyErrorProvider.SetIconAlignment(this.copyTagLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("copyTagLabel.IconAlignment"))));
            this.copyTagLabel.Name = "copyTagLabel";
            this.copyTagLabel.Tag = "#sourceTagList";
            // 
            // sourceTagList
            // 
            this.sourceTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceTagList.DropDownWidth = 250;
            this.sourceTagList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.sourceTagList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("sourceTagList.IconAlignment"))));
            resources.ApplyResources(this.sourceTagList, "sourceTagList");
            this.sourceTagList.Name = "sourceTagList";
            this.sourceTagList.Tag = "#forTagLabel";
            this.sourceTagList.SelectedIndexChanged += new System.EventHandler(this.sourceTagList_SelectedIndexChanged);
            // 
            // forTagLabel
            // 
            resources.ApplyResources(this.forTagLabel, "forTagLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.forTagLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("forTagLabel.IconAlignment"))));
            this.forTagLabel.Name = "forTagLabel";
            this.forTagLabel.Tag = "#destinationTagList";
            // 
            // destinationTagList
            // 
            this.destinationTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.destinationTagList.DropDownWidth = 250;
            this.destinationTagList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.destinationTagList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("destinationTagList.IconAlignment"))));
            resources.ApplyResources(this.destinationTagList, "destinationTagList");
            this.destinationTagList.Name = "destinationTagList";
            this.destinationTagList.Tag = "#forSelectedTracksLabel";
            this.destinationTagList.SelectedIndexChanged += new System.EventHandler(this.destinationTagList_SelectedIndexChanged);
            // 
            // forSelectedTracksLabel
            // 
            resources.ApplyResources(this.forSelectedTracksLabel, "forSelectedTracksLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.forSelectedTracksLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("forSelectedTracksLabel.IconAlignment"))));
            this.forSelectedTracksLabel.Name = "forSelectedTracksLabel";
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
            this.buttonClose.Tag = "#fieldsPanel@non-defaultable";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // appendCheckBox
            // 
            resources.ApplyResources(this.appendCheckBox, "appendCheckBox");
            this.dirtyErrorProvider.SetIconAlignment(this.appendCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("appendCheckBox.IconAlignment"))));
            this.appendCheckBox.Name = "appendCheckBox";
            this.appendCheckBox.Tag = "#appendCheckBoxLabel";
            this.appendCheckBox.CheckedChanged += new System.EventHandler(this.appendCheckBox_CheckedChanged);
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
            // previewTable
            // 
            this.previewTable.AllowUserToAddRows = false;
            this.previewTable.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.previewTable, "previewTable");
            this.previewTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
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
            this.dirtyErrorProvider.SetIconAlignment(this.previewTable, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("previewTable.IconAlignment"))));
            this.previewTable.MultiSelect = false;
            this.previewTable.Name = "previewTable";
            this.previewTable.RowHeadersVisible = false;
            this.previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.previewTable.Tag = "#CopyTag&CopyTag@pinned-to-parent-x";
            this.previewTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.previewTable_CellContentClick);
            // 
            // File
            // 
            this.File.DataPropertyName = "File";
            this.File.FillWeight = 1F;
            resources.ApplyResources(this.File, "File");
            this.File.Name = "File";
            // 
            // Track
            // 
            this.Track.DataPropertyName = "Track";
            this.Track.FillWeight = 200F;
            resources.ApplyResources(this.Track, "Track");
            this.Track.Name = "Track";
            // 
            // SupposedDestinationTagValue
            // 
            this.SupposedDestinationTagValue.DataPropertyName = "SupposedDestinationTagValue";
            this.SupposedDestinationTagValue.FillWeight = 1F;
            resources.ApplyResources(this.SupposedDestinationTagValue, "SupposedDestinationTagValue");
            this.SupposedDestinationTagValue.Name = "SupposedDestinationTagValue";
            // 
            // SupposedDestinationTagValueNormalized
            // 
            this.SupposedDestinationTagValueNormalized.DataPropertyName = "SupposedDestinationTagValueNormalized";
            this.SupposedDestinationTagValueNormalized.FillWeight = 1F;
            resources.ApplyResources(this.SupposedDestinationTagValueNormalized, "SupposedDestinationTagValueNormalized");
            this.SupposedDestinationTagValueNormalized.Name = "SupposedDestinationTagValueNormalized";
            // 
            // OriginalDestinationTagValue
            // 
            this.OriginalDestinationTagValue.DataPropertyName = "OriginalDestinationTagValue";
            this.OriginalDestinationTagValue.FillWeight = 1F;
            resources.ApplyResources(this.OriginalDestinationTagValue, "OriginalDestinationTagValue");
            this.OriginalDestinationTagValue.Name = "OriginalDestinationTagValue";
            // 
            // OriginalDestinationTagValueNormalized
            // 
            this.OriginalDestinationTagValueNormalized.DataPropertyName = "OriginalDestinationTagValueNormalized";
            resources.ApplyResources(this.OriginalDestinationTagValueNormalized, "OriginalDestinationTagValueNormalized");
            this.OriginalDestinationTagValueNormalized.Name = "OriginalDestinationTagValueNormalized";
            // 
            // NewDestinationTagValue
            // 
            this.NewDestinationTagValue.DataPropertyName = "NewDestinationTagValue";
            this.NewDestinationTagValue.FillWeight = 1F;
            resources.ApplyResources(this.NewDestinationTagValue, "NewDestinationTagValue");
            this.NewDestinationTagValue.Name = "NewDestinationTagValue";
            // 
            // NewDestinationTagValueNormalized
            // 
            this.NewDestinationTagValueNormalized.DataPropertyName = "NewDestinationTagValueNormalized";
            resources.ApplyResources(this.NewDestinationTagValueNormalized, "NewDestinationTagValueNormalized");
            this.NewDestinationTagValueNormalized.Name = "NewDestinationTagValueNormalized";
            // 
            // smartOperationCheckBox
            // 
            resources.ApplyResources(this.smartOperationCheckBox, "smartOperationCheckBox");
            this.dirtyErrorProvider.SetIconAlignment(this.smartOperationCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("smartOperationCheckBox.IconAlignment"))));
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
            this.dirtyErrorProvider.SetIconAlignment(this.onlyIfDestinationEmptyCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("onlyIfDestinationEmptyCheckBox.IconAlignment"))));
            this.onlyIfDestinationEmptyCheckBox.Name = "onlyIfDestinationEmptyCheckBox";
            this.onlyIfDestinationEmptyCheckBox.Tag = "#onlyIfDestinationEmptyCheckBoxLabel";
            // 
            // addCheckBox
            // 
            resources.ApplyResources(this.addCheckBox, "addCheckBox");
            this.dirtyErrorProvider.SetIconAlignment(this.addCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("addCheckBox.IconAlignment"))));
            this.addCheckBox.Name = "addCheckBox";
            this.addCheckBox.Tag = "#addCheckBoxLabel";
            this.addCheckBox.CheckedChanged += new System.EventHandler(this.addCheckBox_CheckedChanged);
            // 
            // fileNameLabel
            // 
            resources.ApplyResources(this.fileNameLabel, "fileNameLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.fileNameLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("fileNameLabel.IconAlignment"))));
            this.fileNameLabel.Name = "fileNameLabel";
            this.fileNameLabel.Tag = "#fileNameTextBox";
            // 
            // fileNameTextBox
            // 
            resources.ApplyResources(this.fileNameTextBox, "fileNameTextBox");
            this.fileNameTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.fileNameTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.fileNameTextBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.fileNameTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("fileNameTextBox.IconAlignment"))));
            this.fileNameTextBox.Name = "fileNameTextBox";
            this.fileNameTextBox.Tag = "#buttonBrowse";
            this.fileNameTextBox.Leave += new System.EventHandler(this.filenameTextBox_Leave);
            // 
            // appendedTextBox
            // 
            resources.ApplyResources(this.appendedTextBox, "appendedTextBox");
            this.appendedTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.appendedTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.appendedTextBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.appendedTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("appendedTextBox.IconAlignment"))));
            this.appendedTextBox.Name = "appendedTextBox";
            this.appendedTextBox.Tag = "#fieldsPanel";
            this.appendedTextBox.Leave += new System.EventHandler(this.appendedTextBox_Leave);
            // 
            // addedTextBox
            // 
            resources.ApplyResources(this.addedTextBox, "addedTextBox");
            this.addedTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.addedTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.addedTextBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.addedTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("addedTextBox.IconAlignment"))));
            this.addedTextBox.Name = "addedTextBox";
            this.addedTextBox.Tag = "#fieldsPanel";
            this.addedTextBox.Leave += new System.EventHandler(this.addedTextBox_Leave);
            // 
            // buttonBrowse
            // 
            resources.ApplyResources(this.buttonBrowse, "buttonBrowse");
            this.buttonBrowse.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dirtyErrorProvider.SetIconAlignment(this.buttonBrowse, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonBrowse.IconAlignment"))));
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Tag = "#fieldsPanel@non-defaultable";
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // buttonSettings
            // 
            resources.ApplyResources(this.buttonSettings, "buttonSettings");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonSettings, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonSettings.IconAlignment"))));
            this.buttonSettings.Image = global::MusicBeePlugin.Properties.Resources.gear_15;
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Tag = "@non-defaultable@square-button";
            this.toolTip1.SetToolTip(this.buttonSettings, resources.GetString("buttonSettings.ToolTip"));
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // onlyIfSourceNotEmptyCheckBox
            // 
            resources.ApplyResources(this.onlyIfSourceNotEmptyCheckBox, "onlyIfSourceNotEmptyCheckBox");
            this.dirtyErrorProvider.SetIconAlignment(this.onlyIfSourceNotEmptyCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("onlyIfSourceNotEmptyCheckBox.IconAlignment"))));
            this.onlyIfSourceNotEmptyCheckBox.Name = "onlyIfSourceNotEmptyCheckBox";
            this.onlyIfSourceNotEmptyCheckBox.Tag = "#onlyIfSourceNotEmptyCheckBoxLabel";
            // 
            // smartOperationCheckBoxLabel
            // 
            resources.ApplyResources(this.smartOperationCheckBoxLabel, "smartOperationCheckBoxLabel");
            this.smartOperationCheckBoxLabel.Name = "smartOperationCheckBoxLabel";
            this.toolTip1.SetToolTip(this.smartOperationCheckBoxLabel, resources.GetString("smartOperationCheckBoxLabel.ToolTip"));
            this.smartOperationCheckBoxLabel.Click += new System.EventHandler(this.smartOperationCheckBoxLabel_Click);
            // 
            // onlyIfDestinationEmptyCheckBoxLabel
            // 
            resources.ApplyResources(this.onlyIfDestinationEmptyCheckBoxLabel, "onlyIfDestinationEmptyCheckBoxLabel");
            this.onlyIfDestinationEmptyCheckBoxLabel.Name = "onlyIfDestinationEmptyCheckBoxLabel";
            this.onlyIfDestinationEmptyCheckBoxLabel.Tag = "#onlyIfSourceNotEmptyCheckBox";
            this.onlyIfDestinationEmptyCheckBoxLabel.Click += new System.EventHandler(this.onlyIfDestinationEmptyCheckBoxLabel_Click);
            // 
            // onlyIfSourceNotEmptyCheckBoxLabel
            // 
            resources.ApplyResources(this.onlyIfSourceNotEmptyCheckBoxLabel, "onlyIfSourceNotEmptyCheckBoxLabel");
            this.onlyIfSourceNotEmptyCheckBoxLabel.Name = "onlyIfSourceNotEmptyCheckBoxLabel";
            this.onlyIfSourceNotEmptyCheckBoxLabel.Tag = "";
            this.onlyIfSourceNotEmptyCheckBoxLabel.Click += new System.EventHandler(this.onlyIfSourceNotEmptyCheckBoxLabel_Click);
            // 
            // appendCheckBoxLabel
            // 
            resources.ApplyResources(this.appendCheckBoxLabel, "appendCheckBoxLabel");
            this.appendCheckBoxLabel.Name = "appendCheckBoxLabel";
            this.appendCheckBoxLabel.Click += new System.EventHandler(this.appendCheckBoxLabel_Click);
            // 
            // addCheckBoxLabel
            // 
            resources.ApplyResources(this.addCheckBoxLabel, "addCheckBoxLabel");
            this.addCheckBoxLabel.Name = "addCheckBoxLabel";
            this.addCheckBoxLabel.Tag = "#fieldsPanel";
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
            this.fieldsPanel.Name = "fieldsPanel";
            this.fieldsPanel.Tag = "#CopyTag&previewTable@pinned-to-parent-x";
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