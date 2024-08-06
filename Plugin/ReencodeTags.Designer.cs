﻿namespace MusicBeePlugin
{
    partial class ReEncodeTags
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReEncodeTags));
            this.buttonPreview = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.initialEncodingList = new System.Windows.Forms.ComboBox();
            this.previewTable = new System.Windows.Forms.DataGridView();
            this.File = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Track = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewTrack = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usedEncodingList = new System.Windows.Forms.ComboBox();
            this.usedEncodingLabel = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dirtyErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.buttonSettings = new System.Windows.Forms.Button();
            this.previewSortTagsСheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.previewSortTagsСheckBoxLabel = new System.Windows.Forms.Label();
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
            this.dirtyErrorProvider.SetIconPadding(this.buttonPreview, ((int)(resources.GetObject("buttonPreview.IconPadding"))));
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Tag = "#buttonOK";
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // buttonClose
            // 
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dirtyErrorProvider.SetError(this.buttonClose, resources.GetString("buttonClose.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonClose, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonClose.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonClose, ((int)(resources.GetObject("buttonClose.IconPadding"))));
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Tag = "#controlsPanel@non-defaultable";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.dirtyErrorProvider.SetError(this.buttonOK, resources.GetString("buttonOK.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonOK, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonOK.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonOK, ((int)(resources.GetObject("buttonOK.IconPadding"))));
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Tag = "#buttonClose";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.dirtyErrorProvider.SetError(this.label2, resources.GetString("label2.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label2, ((int)(resources.GetObject("label2.IconPadding"))));
            this.label2.Name = "label2";
            this.label2.Tag = "#initialEncodingList@pinned-to-parent-x";
            // 
            // initialEncodingList
            // 
            resources.ApplyResources(this.initialEncodingList, "initialEncodingList");
            this.initialEncodingList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.initialEncodingList.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.initialEncodingList, resources.GetString("initialEncodingList.Error"));
            this.initialEncodingList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.initialEncodingList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("initialEncodingList.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.initialEncodingList, ((int)(resources.GetObject("initialEncodingList.IconPadding"))));
            this.initialEncodingList.Name = "initialEncodingList";
            this.initialEncodingList.Tag = "#usedEncodingLabel";
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
            this.File,
            this.Track,
            this.NewTrack});
            this.previewTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dirtyErrorProvider.SetError(this.previewTable, resources.GetString("previewTable.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.previewTable, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("previewTable.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.previewTable, ((int)(resources.GetObject("previewTable.IconPadding"))));
            this.previewTable.MultiSelect = false;
            this.previewTable.Name = "previewTable";
            this.previewTable.RowHeadersVisible = false;
            this.previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.previewTable.Tag = "#ReEncodeTags&ReEncodeTags@pinned-to-parent-x";
            this.previewTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.previewTable_CellContentClick);
            this.previewTable.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.previewTable_CellContentDoubleClick);
            // 
            // File
            // 
            this.File.FillWeight = 1F;
            resources.ApplyResources(this.File, "File");
            this.File.Name = "File";
            // 
            // Track
            // 
            this.Track.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Track.FillWeight = 50F;
            resources.ApplyResources(this.Track, "Track");
            this.Track.Name = "Track";
            // 
            // NewTrack
            // 
            this.NewTrack.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NewTrack.FillWeight = 50F;
            resources.ApplyResources(this.NewTrack, "NewTrack");
            this.NewTrack.Name = "NewTrack";
            // 
            // usedEncodingList
            // 
            resources.ApplyResources(this.usedEncodingList, "usedEncodingList");
            this.usedEncodingList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.usedEncodingList.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.usedEncodingList, resources.GetString("usedEncodingList.Error"));
            this.usedEncodingList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.usedEncodingList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("usedEncodingList.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.usedEncodingList, ((int)(resources.GetObject("usedEncodingList.IconPadding"))));
            this.usedEncodingList.Name = "usedEncodingList";
            // 
            // usedEncodingLabel
            // 
            resources.ApplyResources(this.usedEncodingLabel, "usedEncodingLabel");
            this.dirtyErrorProvider.SetError(this.usedEncodingLabel, resources.GetString("usedEncodingLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.usedEncodingLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("usedEncodingLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.usedEncodingLabel, ((int)(resources.GetObject("usedEncodingLabel.IconPadding"))));
            this.usedEncodingLabel.Name = "usedEncodingLabel";
            this.usedEncodingLabel.Tag = "#usedEncodingList";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.FillWeight = 1F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 75F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.FillWeight = 25F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.FillWeight = 25F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
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
            this.dirtyErrorProvider.SetError(this.buttonSettings, resources.GetString("buttonSettings.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonSettings, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonSettings.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonSettings, ((int)(resources.GetObject("buttonSettings.IconPadding"))));
            this.buttonSettings.Image = global::MusicBeePlugin.Properties.Resources.gear_15;
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Tag = "#buttonPreview@non-defaultable@square-button";
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // previewSortTagsСheckBox
            // 
            resources.ApplyResources(this.previewSortTagsСheckBox, "previewSortTagsСheckBox");
            this.dirtyErrorProvider.SetError(this.previewSortTagsСheckBox, resources.GetString("previewSortTagsСheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.previewSortTagsСheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("previewSortTagsСheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.previewSortTagsСheckBox, ((int)(resources.GetObject("previewSortTagsСheckBox.IconPadding"))));
            this.previewSortTagsСheckBox.Name = "previewSortTagsСheckBox";
            this.previewSortTagsСheckBox.Tag = "#previewSortTagsСheckBoxLabel";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.dirtyErrorProvider.SetError(this.label1, resources.GetString("label1.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            this.label1.Tag = "@pinned-to-parent-x";
            // 
            // previewSortTagsСheckBoxLabel
            // 
            resources.ApplyResources(this.previewSortTagsСheckBoxLabel, "previewSortTagsСheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.previewSortTagsСheckBoxLabel, resources.GetString("previewSortTagsСheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.previewSortTagsСheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("previewSortTagsСheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.previewSortTagsСheckBoxLabel, ((int)(resources.GetObject("previewSortTagsСheckBoxLabel.IconPadding"))));
            this.previewSortTagsСheckBoxLabel.Name = "previewSortTagsСheckBoxLabel";
            // 
            // controlsPanel
            // 
            resources.ApplyResources(this.controlsPanel, "controlsPanel");
            this.controlsPanel.Controls.Add(this.buttonSettings);
            this.controlsPanel.Controls.Add(this.previewSortTagsСheckBoxLabel);
            this.controlsPanel.Controls.Add(this.previewSortTagsСheckBox);
            this.controlsPanel.Controls.Add(this.label1);
            this.controlsPanel.Controls.Add(this.buttonClose);
            this.controlsPanel.Controls.Add(this.buttonOK);
            this.controlsPanel.Controls.Add(this.buttonPreview);
            this.controlsPanel.Controls.Add(this.usedEncodingList);
            this.controlsPanel.Controls.Add(this.usedEncodingLabel);
            this.controlsPanel.Controls.Add(this.initialEncodingList);
            this.controlsPanel.Controls.Add(this.label2);
            this.dirtyErrorProvider.SetError(this.controlsPanel, resources.GetString("controlsPanel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.controlsPanel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("controlsPanel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.controlsPanel, ((int)(resources.GetObject("controlsPanel.IconPadding"))));
            this.controlsPanel.Name = "controlsPanel";
            this.controlsPanel.Tag = "#ReEncodeTags&previewTable@pinned-to-parent-x";
            // 
            // ReEncodeTags
            // 
            this.AcceptButton = this.buttonPreview;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.Controls.Add(this.previewTable);
            this.Controls.Add(this.controlsPanel);
            this.DoubleBuffered = true;
            this.Name = "ReEncodeTags";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReEncodeTags_FormClosing);
            this.Load += new System.EventHandler(this.ReEncodeTags_Load);
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).EndInit();
            this.controlsPanel.ResumeLayout(false);
            this.controlsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox initialEncodingList;
        private System.Windows.Forms.DataGridView previewTable;
        private System.Windows.Forms.ComboBox usedEncodingList;
        private System.Windows.Forms.Label usedEncodingLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.ErrorProvider dirtyErrorProvider;
        private System.Windows.Forms.DataGridViewTextBoxColumn File;
        private System.Windows.Forms.DataGridViewTextBoxColumn Track;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTrack;
        private System.Windows.Forms.CheckBox previewSortTagsСheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label previewSortTagsСheckBoxLabel;
        private System.Windows.Forms.Panel controlsPanel;
        private System.Windows.Forms.Button buttonSettings;
    }
}