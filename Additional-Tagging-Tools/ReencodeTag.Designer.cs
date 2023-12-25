namespace MusicBeePlugin
{
    partial class ReencodeTagCommand
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReencodeTagCommand));
            this.buttonPreview = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.forSelectedTracksLabel = new System.Windows.Forms.Label();
            this.sourceTagList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.initialEncodingsList = new System.Windows.Forms.ComboBox();
            this.previewTable = new System.Windows.Forms.DataGridView();
            this.File = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Track = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalTag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewTag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usedEncodingsList = new System.Windows.Forms.ComboBox();
            this.incorrectlyUsedEncodingLabel = new System.Windows.Forms.Label();
            this.dirtyErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.buttonSettings = new System.Windows.Forms.Button();
            this.controlsPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).BeginInit();
            this.controlsPanel.SuspendLayout();
            this.SuspendLayout();
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
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Tag = "#controlsPanel";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Tag = "#buttonCancel";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // forSelectedTracksLabel
            // 
            resources.ApplyResources(this.forSelectedTracksLabel, "forSelectedTracksLabel");
            this.forSelectedTracksLabel.Name = "forSelectedTracksLabel";
            // 
            // sourceTagList
            // 
            this.sourceTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceTagList.DropDownWidth = 250;
            this.sourceTagList.FormattingEnabled = true;
            resources.ApplyResources(this.sourceTagList, "sourceTagList");
            this.sourceTagList.Name = "sourceTagList";
            this.sourceTagList.Tag = "#forSelectedTracksLabel";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.label1.Tag = "@pinned-to-parent-x";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            this.label2.Tag = "@pinned-to-parent-x";
            // 
            // initialEncodingsList
            // 
            this.initialEncodingsList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.initialEncodingsList.DropDownWidth = 250;
            this.initialEncodingsList.FormattingEnabled = true;
            resources.ApplyResources(this.initialEncodingsList, "initialEncodingsList");
            this.initialEncodingsList.Name = "initialEncodingsList";
            this.initialEncodingsList.Tag = "#incorrectlyUsedEncodingLabel";
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
            this.OriginalTag,
            this.NewTag});
            this.previewTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.previewTable.MultiSelect = false;
            this.previewTable.Name = "previewTable";
            this.previewTable.RowHeadersVisible = false;
            this.previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.previewTable.Tag = "#ReencodeTagCommand&ReencodeTagCommand@pinned-to-parent-x";
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
            this.Track.FillWeight = 75F;
            resources.ApplyResources(this.Track, "Track");
            this.Track.Name = "Track";
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
            // usedEncodingsList
            // 
            this.usedEncodingsList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.usedEncodingsList.DropDownWidth = 250;
            this.usedEncodingsList.FormattingEnabled = true;
            resources.ApplyResources(this.usedEncodingsList, "usedEncodingsList");
            this.usedEncodingsList.Name = "usedEncodingsList";
            // 
            // incorrectlyUsedEncodingLabel
            // 
            resources.ApplyResources(this.incorrectlyUsedEncodingLabel, "incorrectlyUsedEncodingLabel");
            this.incorrectlyUsedEncodingLabel.Name = "incorrectlyUsedEncodingLabel";
            this.incorrectlyUsedEncodingLabel.Tag = "#usedEncodingsList";
            // 
            // dirtyErrorProvider
            // 
            this.dirtyErrorProvider.BlinkRate = 1000;
            this.dirtyErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
            this.dirtyErrorProvider.ContainerControl = this;
            resources.ApplyResources(this.dirtyErrorProvider, "dirtyErrorProvider");
            // 
            // buttonSettings
            // 
            resources.ApplyResources(this.buttonSettings, "buttonSettings");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonSettings, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonSettings.IconAlignment"))));
            this.buttonSettings.Image = global::MusicBeePlugin.Properties.Resources.gear_15;
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Tag = "#buttonPreview@non-defaultable@square-control";
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // controlsPanel
            // 
            resources.ApplyResources(this.controlsPanel, "controlsPanel");
            this.controlsPanel.Controls.Add(this.buttonSettings);
            this.controlsPanel.Controls.Add(this.usedEncodingsList);
            this.controlsPanel.Controls.Add(this.incorrectlyUsedEncodingLabel);
            this.controlsPanel.Controls.Add(this.initialEncodingsList);
            this.controlsPanel.Controls.Add(this.label2);
            this.controlsPanel.Controls.Add(this.buttonCancel);
            this.controlsPanel.Controls.Add(this.buttonOK);
            this.controlsPanel.Controls.Add(this.buttonPreview);
            this.controlsPanel.Controls.Add(this.forSelectedTracksLabel);
            this.controlsPanel.Controls.Add(this.sourceTagList);
            this.controlsPanel.Controls.Add(this.label1);
            this.controlsPanel.Name = "controlsPanel";
            this.controlsPanel.Tag = "#ReencodeTagCommand&previewTable@pinned-to-parent-x";
            // 
            // ReencodeTagCommand
            // 
            this.AcceptButton = this.buttonPreview;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.previewTable);
            this.Controls.Add(this.controlsPanel);
            this.Name = "ReencodeTagCommand";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReencodeTagPlugin_FormClosing);
            this.Load += new System.EventHandler(this.ReencodeTagPlugin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).EndInit();
            this.controlsPanel.ResumeLayout(false);
            this.controlsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label forSelectedTracksLabel;
        private System.Windows.Forms.ComboBox sourceTagList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox initialEncodingsList;
        private System.Windows.Forms.DataGridView previewTable;
        private System.Windows.Forms.ComboBox usedEncodingsList;
        private System.Windows.Forms.Label incorrectlyUsedEncodingLabel;
        private System.Windows.Forms.ErrorProvider dirtyErrorProvider;
        private System.Windows.Forms.DataGridViewTextBoxColumn File;
        private System.Windows.Forms.DataGridViewTextBoxColumn Track;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTag;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTag;
        private System.Windows.Forms.Panel controlsPanel;
        private System.Windows.Forms.Button buttonSettings;
    }
}