namespace MusicBeePlugin
{
    partial class ReEncodeTag
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReEncodeTag));
            this.buttonPreview = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.forSelectedTracksLabel = new System.Windows.Forms.Label();
            this.sourceTagList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.initialEncodingList = new System.Windows.Forms.ComboBox();
            previewTable = new System.Windows.Forms.DataGridView();
            this.File = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Track = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalTag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewTag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usedEncodingList = new System.Windows.Forms.ComboBox();
            this.incorrectlyUsedEncodingLabel = new System.Windows.Forms.Label();
            this.dirtyErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.buttonSettings = new System.Windows.Forms.Button();
            this.controlsPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(previewTable)).BeginInit();
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
            // buttonClose
            // 
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Tag = "#controlsPanel";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Tag = "#buttonClose";
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
            // initialEncodingList
            // 
            this.initialEncodingList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.initialEncodingList.DropDownWidth = 250;
            this.initialEncodingList.FormattingEnabled = true;
            resources.ApplyResources(this.initialEncodingList, "initialEncodingList");
            this.initialEncodingList.Name = "initialEncodingList";
            this.initialEncodingList.Tag = "#incorrectlyUsedEncodingLabel";
            // 
            // previewTable
            // 
            previewTable.AllowUserToAddRows = false;
            previewTable.AllowUserToDeleteRows = false;
            resources.ApplyResources(previewTable, "previewTable");
            previewTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            previewTable.BackgroundColor = System.Drawing.SystemColors.Window;
            previewTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            previewTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.File,
            this.Track,
            this.OriginalTag,
            this.NewTag});
            previewTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            previewTable.MultiSelect = false;
            previewTable.Name = "previewTable";
            previewTable.RowHeadersVisible = false;
            previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            previewTable.Tag = "#ReEncodeTag&ReEncodeTag@pinned-to-parent-x";
            previewTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.previewTable_CellContentClick);
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
            // usedEncodingList
            // 
            this.usedEncodingList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.usedEncodingList.DropDownWidth = 250;
            this.usedEncodingList.FormattingEnabled = true;
            resources.ApplyResources(this.usedEncodingList, "usedEncodingList");
            this.usedEncodingList.Name = "usedEncodingList";
            // 
            // incorrectlyUsedEncodingLabel
            // 
            resources.ApplyResources(this.incorrectlyUsedEncodingLabel, "incorrectlyUsedEncodingLabel");
            this.incorrectlyUsedEncodingLabel.Name = "incorrectlyUsedEncodingLabel";
            this.incorrectlyUsedEncodingLabel.Tag = "#usedEncodingList";
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
            this.buttonSettings.Tag = "#buttonPreview@non-defaultable@square-button";
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // controlsPanel
            // 
            resources.ApplyResources(this.controlsPanel, "controlsPanel");
            this.controlsPanel.Controls.Add(this.buttonSettings);
            this.controlsPanel.Controls.Add(this.usedEncodingList);
            this.controlsPanel.Controls.Add(this.incorrectlyUsedEncodingLabel);
            this.controlsPanel.Controls.Add(this.initialEncodingList);
            this.controlsPanel.Controls.Add(this.label2);
            this.controlsPanel.Controls.Add(this.buttonClose);
            this.controlsPanel.Controls.Add(this.buttonOK);
            this.controlsPanel.Controls.Add(this.buttonPreview);
            this.controlsPanel.Controls.Add(this.forSelectedTracksLabel);
            this.controlsPanel.Controls.Add(this.sourceTagList);
            this.controlsPanel.Controls.Add(this.label1);
            this.controlsPanel.Name = "controlsPanel";
            this.controlsPanel.Tag = "#ReEncodeTag&previewTable@pinned-to-parent-x";
            // 
            // ReEncodeTag
            // 
            this.AcceptButton = this.buttonPreview;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.Controls.Add(previewTable);
            this.Controls.Add(this.controlsPanel);
            this.DoubleBuffered = true;
            this.Name = "ReEncodeTag";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReEncodeTagPlugin_FormClosing);
            this.Load += new System.EventHandler(this.ReEncodeTagPlugin_Load);
            ((System.ComponentModel.ISupportInitialize)(previewTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).EndInit();
            this.controlsPanel.ResumeLayout(false);
            this.controlsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label forSelectedTracksLabel;
        private System.Windows.Forms.ComboBox sourceTagList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox initialEncodingList;
        private System.Windows.Forms.DataGridView previewTable;
        private System.Windows.Forms.ComboBox usedEncodingList;
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