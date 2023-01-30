﻿using System.Drawing.Drawing2D;

namespace MusicBeePlugin
{
    partial class AdvancedSearchAndReplaceCommand
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedSearchAndReplaceCommand));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.buttonDeleteAll = new System.Windows.Forms.Button();
            this.buttonImportNew = new System.Windows.Forms.Button();
            this.buttonImportAll = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.buttonImport = new System.Windows.Forms.Button();
            this.buttonExportCustom = new System.Windows.Forms.Button();
            this.assignHotkeyCheckBox = new System.Windows.Forms.CheckBox();
            this.idTextBox = new System.Windows.Forms.TextBox();
            this.applyToPlayingTrackCheckBox = new System.Windows.Forms.CheckBox();
            this.preserveValuesTextBox = new System.Windows.Forms.TextBox();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.filterComboBox = new System.Windows.Forms.ComboBox();
            this.hotkeyPictureBox = new System.Windows.Forms.PictureBox();
            this.playlistPictureBox = new System.Windows.Forms.PictureBox();
            this.userPictureBox = new System.Windows.Forms.PictureBox();
            this.customizedPictureBox = new System.Windows.Forms.PictureBox();
            this.predefinedPictureBox = new System.Windows.Forms.PictureBox();
            this.tickedOnlyPictureBox = new System.Windows.Forms.PictureBox();
            this.clearIdButton = new System.Windows.Forms.Button();
            this.clearSearchButton = new System.Windows.Forms.Button();
            this.uncheckAllFiltersPictureBox = new System.Windows.Forms.PictureBox();
            this.functionIdPictureBox = new System.Windows.Forms.PictureBox();
            this.presetList = new System.Windows.Forms.CheckedListBox();
            this.previewTable = new System.Windows.Forms.DataGridView();
            this.File = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Track = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalTag1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewTag1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalTag2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewTag2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalTag3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewTag3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalTag4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewTag4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalTag5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewTag5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.userPresetPictureBox = new System.Windows.Forms.PictureBox();
            this.customizedPresetPictureBox = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.descriptionBox = new System.Windows.Forms.TextBox();
            this.customText4Label = new System.Windows.Forms.Label();
            this.customText2Label = new System.Windows.Forms.Label();
            this.customText4Box = new System.Windows.Forms.TextBox();
            this.customText2Box = new System.Windows.Forms.TextBox();
            this.customText3Label = new System.Windows.Forms.Label();
            this.labelTag6 = new System.Windows.Forms.Label();
            this.customText3Box = new System.Windows.Forms.TextBox();
            this.parameterTag6List = new System.Windows.Forms.ComboBox();
            this.labelTag5 = new System.Windows.Forms.Label();
            this.parameterTag5List = new System.Windows.Forms.ComboBox();
            this.labelTag4 = new System.Windows.Forms.Label();
            this.parameterTag4List = new System.Windows.Forms.ComboBox();
            this.customTextLabel = new System.Windows.Forms.Label();
            this.customTextBox = new System.Windows.Forms.TextBox();
            this.labelTag3 = new System.Windows.Forms.Label();
            this.parameterTag3List = new System.Windows.Forms.ComboBox();
            this.labelTag2 = new System.Windows.Forms.Label();
            this.parameterTag2List = new System.Windows.Forms.ComboBox();
            this.labelTag = new System.Windows.Forms.Label();
            this.parameterTagList = new System.Windows.Forms.ComboBox();
            this.playlistComboBox = new System.Windows.Forms.ComboBox();
            this.conditionCheckBox = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonSaveClose = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dirtyErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.hotkeyPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playlistPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customizedPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.predefinedPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tickedOnlyPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uncheckAllFiltersPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.functionIdPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userPresetPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customizedPresetPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).BeginInit();
            this.SuspendLayout();

            //MusicBee
            this.searchTextBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.customTextBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.customText2Box = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.customText3Box = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.customText4Box = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.idTextBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.preserveValuesTextBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);

            InterpolationMode defaultInterpolationMode = InterpolationMode.HighQualityBicubic;


            this.customizedPresetPictureBox = new InterpolatedBox();
            ((InterpolatedBox)customizedPresetPictureBox).Interpolation = defaultInterpolationMode;

            this.userPresetPictureBox = new InterpolatedBox();
            ((InterpolatedBox)userPresetPictureBox).Interpolation = defaultInterpolationMode;


            this.tickedOnlyPictureBox = new InterpolatedBox();
            ((InterpolatedBox)tickedOnlyPictureBox).Interpolation = defaultInterpolationMode;

            this.predefinedPictureBox = new InterpolatedBox();
            ((InterpolatedBox)predefinedPictureBox).Interpolation = defaultInterpolationMode;

            this.customizedPictureBox = new InterpolatedBox();
            ((InterpolatedBox)customizedPictureBox).Interpolation = defaultInterpolationMode;

            this.userPictureBox = new InterpolatedBox();
            ((InterpolatedBox)userPictureBox).Interpolation = defaultInterpolationMode;

            this.playlistPictureBox = new InterpolatedBox();
            ((InterpolatedBox)playlistPictureBox).Interpolation = defaultInterpolationMode;

            this.functionIdPictureBox = new InterpolatedBox();
            ((InterpolatedBox)functionIdPictureBox).Interpolation = defaultInterpolationMode;

            this.hotkeyPictureBox = new InterpolatedBox();
            ((InterpolatedBox)hotkeyPictureBox).Interpolation = defaultInterpolationMode;

            this.uncheckAllFiltersPictureBox = new InterpolatedBox();
            ((InterpolatedBox)uncheckAllFiltersPictureBox).Interpolation = defaultInterpolationMode;
            //~MusicBee

            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 900;
            this.toolTip1.ReshowDelay = 1000;
            // 
            // buttonDeleteAll
            // 
            resources.ApplyResources(this.buttonDeleteAll, "buttonDeleteAll");
            this.dirtyErrorProvider.SetError(this.buttonDeleteAll, resources.GetString("buttonDeleteAll.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonDeleteAll, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonDeleteAll.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonDeleteAll, ((int)(resources.GetObject("buttonDeleteAll.IconPadding"))));
            this.buttonDeleteAll.Name = "buttonDeleteAll";
            this.toolTip1.SetToolTip(this.buttonDeleteAll, resources.GetString("buttonDeleteAll.ToolTip"));
            this.buttonDeleteAll.UseVisualStyleBackColor = true;
            this.buttonDeleteAll.Click += new System.EventHandler(this.buttonDeleteAll_Click);
            // 
            // buttonImportNew
            // 
            resources.ApplyResources(this.buttonImportNew, "buttonImportNew");
            this.dirtyErrorProvider.SetError(this.buttonImportNew, resources.GetString("buttonImportNew.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonImportNew, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonImportNew.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonImportNew, ((int)(resources.GetObject("buttonImportNew.IconPadding"))));
            this.buttonImportNew.Name = "buttonImportNew";
            this.toolTip1.SetToolTip(this.buttonImportNew, resources.GetString("buttonImportNew.ToolTip"));
            this.buttonImportNew.UseVisualStyleBackColor = true;
            this.buttonImportNew.Click += new System.EventHandler(this.buttonImportNew_Click);
            // 
            // buttonImportAll
            // 
            resources.ApplyResources(this.buttonImportAll, "buttonImportAll");
            this.dirtyErrorProvider.SetError(this.buttonImportAll, resources.GetString("buttonImportAll.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonImportAll, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonImportAll.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonImportAll, ((int)(resources.GetObject("buttonImportAll.IconPadding"))));
            this.buttonImportAll.Name = "buttonImportAll";
            this.toolTip1.SetToolTip(this.buttonImportAll, resources.GetString("buttonImportAll.ToolTip"));
            this.buttonImportAll.UseVisualStyleBackColor = true;
            this.buttonImportAll.Click += new System.EventHandler(this.buttonImportAll_Click);
            // 
            // buttonExport
            // 
            resources.ApplyResources(this.buttonExport, "buttonExport");
            this.dirtyErrorProvider.SetError(this.buttonExport, resources.GetString("buttonExport.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonExport, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonExport.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonExport, ((int)(resources.GetObject("buttonExport.IconPadding"))));
            this.buttonExport.Name = "buttonExport";
            this.toolTip1.SetToolTip(this.buttonExport, resources.GetString("buttonExport.ToolTip"));
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // buttonImport
            // 
            resources.ApplyResources(this.buttonImport, "buttonImport");
            this.dirtyErrorProvider.SetError(this.buttonImport, resources.GetString("buttonImport.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonImport, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonImport.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonImport, ((int)(resources.GetObject("buttonImport.IconPadding"))));
            this.buttonImport.Name = "buttonImport";
            this.toolTip1.SetToolTip(this.buttonImport, resources.GetString("buttonImport.ToolTip"));
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // buttonExportCustom
            // 
            resources.ApplyResources(this.buttonExportCustom, "buttonExportCustom");
            this.dirtyErrorProvider.SetError(this.buttonExportCustom, resources.GetString("buttonExportCustom.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonExportCustom, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonExportCustom.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonExportCustom, ((int)(resources.GetObject("buttonExportCustom.IconPadding"))));
            this.buttonExportCustom.Name = "buttonExportCustom";
            this.toolTip1.SetToolTip(this.buttonExportCustom, resources.GetString("buttonExportCustom.ToolTip"));
            this.buttonExportCustom.UseVisualStyleBackColor = true;
            this.buttonExportCustom.Click += new System.EventHandler(this.buttonExportCustom_Click);
            // 
            // assignHotkeyCheckBox
            // 
            resources.ApplyResources(this.assignHotkeyCheckBox, "assignHotkeyCheckBox");
            this.dirtyErrorProvider.SetError(this.assignHotkeyCheckBox, resources.GetString("assignHotkeyCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.assignHotkeyCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("assignHotkeyCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.assignHotkeyCheckBox, ((int)(resources.GetObject("assignHotkeyCheckBox.IconPadding"))));
            this.assignHotkeyCheckBox.Name = "assignHotkeyCheckBox";
            this.toolTip1.SetToolTip(this.assignHotkeyCheckBox, resources.GetString("assignHotkeyCheckBox.ToolTip"));
            this.assignHotkeyCheckBox.UseVisualStyleBackColor = true;
            this.assignHotkeyCheckBox.CheckedChanged += new System.EventHandler(this.assignHotkeyCheckBox_CheckedChanged);
            // 
            // idTextBox
            // 
            resources.ApplyResources(this.idTextBox, "idTextBox");
            this.dirtyErrorProvider.SetError(this.idTextBox, resources.GetString("idTextBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.idTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("idTextBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.idTextBox, ((int)(resources.GetObject("idTextBox.IconPadding"))));
            this.idTextBox.Name = "idTextBox";
            this.toolTip1.SetToolTip(this.idTextBox, resources.GetString("idTextBox.ToolTip"));
            this.idTextBox.Leave += new System.EventHandler(this.idTextBox_Leave);
            // 
            // applyToPlayingTrackCheckBox
            // 
            resources.ApplyResources(this.applyToPlayingTrackCheckBox, "applyToPlayingTrackCheckBox");
            this.dirtyErrorProvider.SetError(this.applyToPlayingTrackCheckBox, resources.GetString("applyToPlayingTrackCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.applyToPlayingTrackCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("applyToPlayingTrackCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.applyToPlayingTrackCheckBox, ((int)(resources.GetObject("applyToPlayingTrackCheckBox.IconPadding"))));
            this.applyToPlayingTrackCheckBox.Name = "applyToPlayingTrackCheckBox";
            this.toolTip1.SetToolTip(this.applyToPlayingTrackCheckBox, resources.GetString("applyToPlayingTrackCheckBox.ToolTip"));
            this.applyToPlayingTrackCheckBox.UseVisualStyleBackColor = true;
            this.applyToPlayingTrackCheckBox.CheckedChanged += new System.EventHandler(this.applyToPlayingTrackCheckBox_CheckedChanged);
            // 
            // preserveValuesTextBox
            // 
            resources.ApplyResources(this.preserveValuesTextBox, "preserveValuesTextBox");
            this.dirtyErrorProvider.SetError(this.preserveValuesTextBox, resources.GetString("preserveValuesTextBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.preserveValuesTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("preserveValuesTextBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.preserveValuesTextBox, ((int)(resources.GetObject("preserveValuesTextBox.IconPadding"))));
            this.preserveValuesTextBox.Name = "preserveValuesTextBox";
            this.toolTip1.SetToolTip(this.preserveValuesTextBox, resources.GetString("preserveValuesTextBox.ToolTip"));
            this.preserveValuesTextBox.TextChanged += new System.EventHandler(this.preserveValuesTextBox_TextChanged);
            // 
            // buttonCopy
            // 
            resources.ApplyResources(this.buttonCopy, "buttonCopy");
            this.dirtyErrorProvider.SetError(this.buttonCopy, resources.GetString("buttonCopy.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonCopy, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonCopy.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonCopy, ((int)(resources.GetObject("buttonCopy.IconPadding"))));
            this.buttonCopy.Name = "buttonCopy";
            this.toolTip1.SetToolTip(this.buttonCopy, resources.GetString("buttonCopy.ToolTip"));
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
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
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.dirtyErrorProvider.SetError(this.label2, resources.GetString("label2.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label2, ((int)(resources.GetObject("label2.IconPadding"))));
            this.label2.Name = "label2";
            this.toolTip1.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
            // 
            // searchTextBox
            // 
            resources.ApplyResources(this.searchTextBox, "searchTextBox");
            this.dirtyErrorProvider.SetError(this.searchTextBox, resources.GetString("searchTextBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.searchTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("searchTextBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.searchTextBox, ((int)(resources.GetObject("searchTextBox.IconPadding"))));
            this.searchTextBox.Name = "searchTextBox";
            this.toolTip1.SetToolTip(this.searchTextBox, resources.GetString("searchTextBox.ToolTip"));
            this.searchTextBox.TextChanged += new System.EventHandler(this.searchTextBox_TextChanged);
            // 
            // buttonClose
            // 
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dirtyErrorProvider.SetError(this.buttonClose, resources.GetString("buttonClose.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonClose, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonClose.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonClose, ((int)(resources.GetObject("buttonClose.IconPadding"))));
            this.buttonClose.Name = "buttonClose";
            this.toolTip1.SetToolTip(this.buttonClose, resources.GetString("buttonClose.ToolTip"));
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // filterComboBox
            // 
            resources.ApplyResources(this.filterComboBox, "filterComboBox");
            this.filterComboBox.CausesValidation = false;
            this.filterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dirtyErrorProvider.SetError(this.filterComboBox, resources.GetString("filterComboBox.Error"));
            this.filterComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.filterComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("filterComboBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.filterComboBox, ((int)(resources.GetObject("filterComboBox.IconPadding"))));
            this.filterComboBox.Items.AddRange(new object[] {
            resources.GetString("filterComboBox.Items"),
            resources.GetString("filterComboBox.Items1"),
            resources.GetString("filterComboBox.Items2"),
            resources.GetString("filterComboBox.Items3"),
            resources.GetString("filterComboBox.Items4"),
            resources.GetString("filterComboBox.Items5"),
            resources.GetString("filterComboBox.Items6"),
            resources.GetString("filterComboBox.Items7")});
            this.filterComboBox.Name = "filterComboBox";
            this.toolTip1.SetToolTip(this.filterComboBox, resources.GetString("filterComboBox.ToolTip"));
            this.filterComboBox.SelectedIndexChanged += new System.EventHandler(this.filterComboBox_SelectedIndexChanged);
            // 
            // hotkeyPictureBox
            // 
            resources.ApplyResources(this.hotkeyPictureBox, "hotkeyPictureBox");
            this.hotkeyPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.dirtyErrorProvider.SetError(this.hotkeyPictureBox, resources.GetString("hotkeyPictureBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.hotkeyPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("hotkeyPictureBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.hotkeyPictureBox, ((int)(resources.GetObject("hotkeyPictureBox.IconPadding"))));
            this.hotkeyPictureBox.Image = global::MusicBeePlugin.Properties.Resources.hotkey_presets;
            this.hotkeyPictureBox.Name = "hotkeyPictureBox";
            this.hotkeyPictureBox.TabStop = false;
            this.toolTip1.SetToolTip(this.hotkeyPictureBox, resources.GetString("hotkeyPictureBox.ToolTip"));
            this.hotkeyPictureBox.Click += new System.EventHandler(this.hotkeyPictureBox_Click);
            // 
            // playlistPictureBox
            // 
            resources.ApplyResources(this.playlistPictureBox, "playlistPictureBox");
            this.playlistPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.dirtyErrorProvider.SetError(this.playlistPictureBox, resources.GetString("playlistPictureBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.playlistPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("playlistPictureBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.playlistPictureBox, ((int)(resources.GetObject("playlistPictureBox.IconPadding"))));
            this.playlistPictureBox.Image = global::MusicBeePlugin.Properties.Resources.playlist_presets;
            this.playlistPictureBox.Name = "playlistPictureBox";
            this.playlistPictureBox.TabStop = false;
            this.toolTip1.SetToolTip(this.playlistPictureBox, resources.GetString("playlistPictureBox.ToolTip"));
            this.playlistPictureBox.Click += new System.EventHandler(this.playlistPictureBox_Click);
            // 
            // userPictureBox
            // 
            resources.ApplyResources(this.userPictureBox, "userPictureBox");
            this.userPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.dirtyErrorProvider.SetError(this.userPictureBox, resources.GetString("userPictureBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.userPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("userPictureBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.userPictureBox, ((int)(resources.GetObject("userPictureBox.IconPadding"))));
            this.userPictureBox.Image = global::MusicBeePlugin.Properties.Resources.user_presets;
            this.userPictureBox.Name = "userPictureBox";
            this.userPictureBox.TabStop = false;
            this.toolTip1.SetToolTip(this.userPictureBox, resources.GetString("userPictureBox.ToolTip"));
            this.userPictureBox.Click += new System.EventHandler(this.userPictureBox_Click);
            // 
            // customizedPictureBox
            // 
            resources.ApplyResources(this.customizedPictureBox, "customizedPictureBox");
            this.customizedPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.dirtyErrorProvider.SetError(this.customizedPictureBox, resources.GetString("customizedPictureBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.customizedPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customizedPictureBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.customizedPictureBox, ((int)(resources.GetObject("customizedPictureBox.IconPadding"))));
            this.customizedPictureBox.Image = global::MusicBeePlugin.Properties.Resources.customized_presets;
            this.customizedPictureBox.Name = "customizedPictureBox";
            this.customizedPictureBox.TabStop = false;
            this.toolTip1.SetToolTip(this.customizedPictureBox, resources.GetString("customizedPictureBox.ToolTip"));
            this.customizedPictureBox.Click += new System.EventHandler(this.customizedPictureBox_Click);
            // 
            // predefinedPictureBox
            // 
            resources.ApplyResources(this.predefinedPictureBox, "predefinedPictureBox");
            this.predefinedPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.dirtyErrorProvider.SetError(this.predefinedPictureBox, resources.GetString("predefinedPictureBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.predefinedPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("predefinedPictureBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.predefinedPictureBox, ((int)(resources.GetObject("predefinedPictureBox.IconPadding"))));
            this.predefinedPictureBox.Image = global::MusicBeePlugin.Properties.Resources.predefined_presets;
            this.predefinedPictureBox.Name = "predefinedPictureBox";
            this.predefinedPictureBox.TabStop = false;
            this.toolTip1.SetToolTip(this.predefinedPictureBox, resources.GetString("predefinedPictureBox.ToolTip"));
            this.predefinedPictureBox.Click += new System.EventHandler(this.predefinedPictureBox_Click);
            // 
            // tickedOnlyPictureBox
            // 
            resources.ApplyResources(this.tickedOnlyPictureBox, "tickedOnlyPictureBox");
            this.tickedOnlyPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.dirtyErrorProvider.SetError(this.tickedOnlyPictureBox, resources.GetString("tickedOnlyPictureBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.tickedOnlyPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tickedOnlyPictureBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.tickedOnlyPictureBox, ((int)(resources.GetObject("tickedOnlyPictureBox.IconPadding"))));
            this.tickedOnlyPictureBox.Image = global::MusicBeePlugin.Properties.Resources.auto_applied_presets;
            this.tickedOnlyPictureBox.Name = "tickedOnlyPictureBox";
            this.tickedOnlyPictureBox.TabStop = false;
            this.toolTip1.SetToolTip(this.tickedOnlyPictureBox, resources.GetString("tickedOnlyPictureBox.ToolTip"));
            this.tickedOnlyPictureBox.Click += new System.EventHandler(this.tickedOnlyPictureBox_Click);
            // 
            // clearIdButton
            // 
            resources.ApplyResources(this.clearIdButton, "clearIdButton");
            this.dirtyErrorProvider.SetError(this.clearIdButton, resources.GetString("clearIdButton.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.clearIdButton, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("clearIdButton.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.clearIdButton, ((int)(resources.GetObject("clearIdButton.IconPadding"))));
            this.clearIdButton.Image = global::MusicBeePlugin.Properties.Resources.clear_button;
            this.clearIdButton.Name = "clearIdButton";
            this.toolTip1.SetToolTip(this.clearIdButton, resources.GetString("clearIdButton.ToolTip"));
            this.clearIdButton.UseVisualStyleBackColor = true;
            this.clearIdButton.Click += new System.EventHandler(this.clearIdButton_Click);
            // 
            // clearSearchButton
            // 
            resources.ApplyResources(this.clearSearchButton, "clearSearchButton");
            this.dirtyErrorProvider.SetError(this.clearSearchButton, resources.GetString("clearSearchButton.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.clearSearchButton, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("clearSearchButton.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.clearSearchButton, ((int)(resources.GetObject("clearSearchButton.IconPadding"))));
            this.clearSearchButton.Image = global::MusicBeePlugin.Properties.Resources.clear_button;
            this.clearSearchButton.Name = "clearSearchButton";
            this.toolTip1.SetToolTip(this.clearSearchButton, resources.GetString("clearSearchButton.ToolTip"));
            this.clearSearchButton.UseVisualStyleBackColor = true;
            this.clearSearchButton.Click += new System.EventHandler(this.clearSearchButton_Click);
            // 
            // uncheckAllFiltersPictureBox
            // 
            resources.ApplyResources(this.uncheckAllFiltersPictureBox, "uncheckAllFiltersPictureBox");
            this.uncheckAllFiltersPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.dirtyErrorProvider.SetError(this.uncheckAllFiltersPictureBox, resources.GetString("uncheckAllFiltersPictureBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.uncheckAllFiltersPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("uncheckAllFiltersPictureBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.uncheckAllFiltersPictureBox, ((int)(resources.GetObject("uncheckAllFiltersPictureBox.IconPadding"))));
            this.uncheckAllFiltersPictureBox.Image = global::MusicBeePlugin.Properties.Resources.uncheck_all_preset_filters;
            this.uncheckAllFiltersPictureBox.Name = "uncheckAllFiltersPictureBox";
            this.uncheckAllFiltersPictureBox.TabStop = false;
            this.toolTip1.SetToolTip(this.uncheckAllFiltersPictureBox, resources.GetString("uncheckAllFiltersPictureBox.ToolTip"));
            this.uncheckAllFiltersPictureBox.Click += new System.EventHandler(this.uncheckAllFiltersPictureBox_Click);
            // 
            // functionIdPictureBox
            // 
            resources.ApplyResources(this.functionIdPictureBox, "functionIdPictureBox");
            this.functionIdPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.dirtyErrorProvider.SetError(this.functionIdPictureBox, resources.GetString("functionIdPictureBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.functionIdPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("functionIdPictureBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.functionIdPictureBox, ((int)(resources.GetObject("functionIdPictureBox.IconPadding"))));
            this.functionIdPictureBox.Image = global::MusicBeePlugin.Properties.Resources.function_id_presets;
            this.functionIdPictureBox.Name = "functionIdPictureBox";
            this.functionIdPictureBox.TabStop = false;
            this.toolTip1.SetToolTip(this.functionIdPictureBox, resources.GetString("functionIdPictureBox.ToolTip"));
            this.functionIdPictureBox.Click += new System.EventHandler(this.functionIdPictureBox_Click);
            // 
            // presetList
            // 
            resources.ApplyResources(this.presetList, "presetList");
            this.dirtyErrorProvider.SetError(this.presetList, resources.GetString("presetList.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.presetList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("presetList.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.presetList, ((int)(resources.GetObject("presetList.IconPadding"))));
            this.presetList.Name = "presetList";
            this.toolTip1.SetToolTip(this.presetList, resources.GetString("presetList.ToolTip"));
            this.presetList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.presetList_ItemCheck);
            this.presetList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.presetList_MouseClick);
            this.presetList.SelectedIndexChanged += new System.EventHandler(this.presetList_SelectedIndexChanged);
            this.presetList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.presetList_MouseDoubleClick);
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
            this.OriginalTag1,
            this.NewTag1,
            this.OriginalTag2,
            this.NewTag2,
            this.OriginalTag3,
            this.NewTag3,
            this.OriginalTag4,
            this.NewTag4,
            this.OriginalTag5,
            this.NewTag5});
            this.previewTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dirtyErrorProvider.SetError(this.previewTable, resources.GetString("previewTable.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.previewTable, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("previewTable.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.previewTable, ((int)(resources.GetObject("previewTable.IconPadding"))));
            this.previewTable.MultiSelect = false;
            this.previewTable.Name = "previewTable";
            this.previewTable.RowHeadersVisible = false;
            this.previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.toolTip1.SetToolTip(this.previewTable, resources.GetString("previewTable.ToolTip"));
            this.previewTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.previewTable_CellContentClick);
            this.previewTable.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.previewTable_CellFormatting);
            this.previewTable.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.previewTable_ColumnWidthChanged);
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
            // OriginalTag1
            // 
            this.OriginalTag1.FillWeight = 25F;
            resources.ApplyResources(this.OriginalTag1, "OriginalTag1");
            this.OriginalTag1.Name = "OriginalTag1";
            // 
            // NewTag1
            // 
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.NewTag1.DefaultCellStyle = dataGridViewCellStyle1;
            this.NewTag1.FillWeight = 25F;
            resources.ApplyResources(this.NewTag1, "NewTag1");
            this.NewTag1.Name = "NewTag1";
            // 
            // OriginalTag2
            // 
            this.OriginalTag2.FillWeight = 25F;
            resources.ApplyResources(this.OriginalTag2, "OriginalTag2");
            this.OriginalTag2.Name = "OriginalTag2";
            // 
            // NewTag2
            // 
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.NewTag2.DefaultCellStyle = dataGridViewCellStyle2;
            this.NewTag2.FillWeight = 25F;
            resources.ApplyResources(this.NewTag2, "NewTag2");
            this.NewTag2.Name = "NewTag2";
            // 
            // OriginalTag3
            // 
            this.OriginalTag3.FillWeight = 25F;
            resources.ApplyResources(this.OriginalTag3, "OriginalTag3");
            this.OriginalTag3.Name = "OriginalTag3";
            // 
            // NewTag3
            // 
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.NewTag3.DefaultCellStyle = dataGridViewCellStyle3;
            this.NewTag3.FillWeight = 25F;
            resources.ApplyResources(this.NewTag3, "NewTag3");
            this.NewTag3.Name = "NewTag3";
            // 
            // OriginalTag4
            // 
            this.OriginalTag4.FillWeight = 25F;
            resources.ApplyResources(this.OriginalTag4, "OriginalTag4");
            this.OriginalTag4.Name = "OriginalTag4";
            // 
            // NewTag4
            // 
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.NewTag4.DefaultCellStyle = dataGridViewCellStyle4;
            this.NewTag4.FillWeight = 25F;
            resources.ApplyResources(this.NewTag4, "NewTag4");
            this.NewTag4.Name = "NewTag4";
            // 
            // OriginalTag5
            // 
            this.OriginalTag5.FillWeight = 25F;
            resources.ApplyResources(this.OriginalTag5, "OriginalTag5");
            this.OriginalTag5.Name = "OriginalTag5";
            // 
            // NewTag5
            // 
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.NewTag5.DefaultCellStyle = dataGridViewCellStyle5;
            this.NewTag5.FillWeight = 25F;
            resources.ApplyResources(this.NewTag5, "NewTag5");
            this.NewTag5.Name = "NewTag5";
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
            // buttonCreate
            // 
            resources.ApplyResources(this.buttonCreate, "buttonCreate");
            this.dirtyErrorProvider.SetError(this.buttonCreate, resources.GetString("buttonCreate.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonCreate, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonCreate.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonCreate, ((int)(resources.GetObject("buttonCreate.IconPadding"))));
            this.buttonCreate.Name = "buttonCreate";
            this.toolTip1.SetToolTip(this.buttonCreate, resources.GetString("buttonCreate.ToolTip"));
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // buttonEdit
            // 
            resources.ApplyResources(this.buttonEdit, "buttonEdit");
            this.dirtyErrorProvider.SetError(this.buttonEdit, resources.GetString("buttonEdit.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonEdit, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonEdit.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonEdit, ((int)(resources.GetObject("buttonEdit.IconPadding"))));
            this.buttonEdit.Name = "buttonEdit";
            this.toolTip1.SetToolTip(this.buttonEdit, resources.GetString("buttonEdit.ToolTip"));
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.userPresetPictureBox);
            this.panel1.Controls.Add(this.customizedPresetPictureBox);
            this.panel1.Controls.Add(this.uncheckAllFiltersPictureBox);
            this.panel1.Controls.Add(this.hotkeyPictureBox);
            this.panel1.Controls.Add(this.functionIdPictureBox);
            this.panel1.Controls.Add(this.playlistPictureBox);
            this.panel1.Controls.Add(this.userPictureBox);
            this.panel1.Controls.Add(this.customizedPictureBox);
            this.panel1.Controls.Add(this.predefinedPictureBox);
            this.panel1.Controls.Add(this.tickedOnlyPictureBox);
            this.panel1.Controls.Add(this.filterComboBox);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.preserveValuesTextBox);
            this.panel1.Controls.Add(this.applyToPlayingTrackCheckBox);
            this.panel1.Controls.Add(this.clearIdButton);
            this.panel1.Controls.Add(this.idTextBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.assignHotkeyCheckBox);
            this.panel1.Controls.Add(this.clearSearchButton);
            this.panel1.Controls.Add(this.searchTextBox);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.customText4Label);
            this.panel1.Controls.Add(this.customText2Label);
            this.panel1.Controls.Add(this.customText4Box);
            this.panel1.Controls.Add(this.customText2Box);
            this.panel1.Controls.Add(this.customText3Label);
            this.panel1.Controls.Add(this.labelTag6);
            this.panel1.Controls.Add(this.customText3Box);
            this.panel1.Controls.Add(this.parameterTag6List);
            this.panel1.Controls.Add(this.labelTag5);
            this.panel1.Controls.Add(this.parameterTag5List);
            this.panel1.Controls.Add(this.labelTag4);
            this.panel1.Controls.Add(this.parameterTag4List);
            this.panel1.Controls.Add(this.customTextLabel);
            this.panel1.Controls.Add(this.customTextBox);
            this.panel1.Controls.Add(this.labelTag3);
            this.panel1.Controls.Add(this.parameterTag3List);
            this.panel1.Controls.Add(this.labelTag2);
            this.panel1.Controls.Add(this.parameterTag2List);
            this.panel1.Controls.Add(this.labelTag);
            this.panel1.Controls.Add(this.parameterTagList);
            this.panel1.Controls.Add(this.playlistComboBox);
            this.panel1.Controls.Add(this.conditionCheckBox);
            this.panel1.Controls.Add(this.presetList);
            this.panel1.Controls.Add(this.label5);
            this.dirtyErrorProvider.SetError(this.panel1, resources.GetString("panel1.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.panel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("panel1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.panel1, ((int)(resources.GetObject("panel1.IconPadding"))));
            this.panel1.Name = "panel1";
            this.toolTip1.SetToolTip(this.panel1, resources.GetString("panel1.ToolTip"));
            // 
            // userPresetPictureBox
            // 
            resources.ApplyResources(this.userPresetPictureBox, "userPresetPictureBox");
            this.userPresetPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.dirtyErrorProvider.SetError(this.userPresetPictureBox, resources.GetString("userPresetPictureBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.userPresetPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("userPresetPictureBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.userPresetPictureBox, ((int)(resources.GetObject("userPresetPictureBox.IconPadding"))));
            this.userPresetPictureBox.Image = global::MusicBeePlugin.Properties.Resources.uncheck_mark;
            this.userPresetPictureBox.Name = "userPresetPictureBox";
            this.userPresetPictureBox.TabStop = false;
            this.toolTip1.SetToolTip(this.userPresetPictureBox, resources.GetString("userPresetPictureBox.ToolTip"));
            // 
            // customizedPresetPictureBox
            // 
            resources.ApplyResources(this.customizedPresetPictureBox, "customizedPresetPictureBox");
            this.customizedPresetPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.dirtyErrorProvider.SetError(this.customizedPresetPictureBox, resources.GetString("customizedPresetPictureBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.customizedPresetPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customizedPresetPictureBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.customizedPresetPictureBox, ((int)(resources.GetObject("customizedPresetPictureBox.IconPadding"))));
            this.customizedPresetPictureBox.Image = global::MusicBeePlugin.Properties.Resources.uncheck_mark;
            this.customizedPresetPictureBox.Name = "customizedPresetPictureBox";
            this.customizedPresetPictureBox.TabStop = false;
            this.toolTip1.SetToolTip(this.customizedPresetPictureBox, resources.GetString("customizedPresetPictureBox.ToolTip"));
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.dirtyErrorProvider.SetError(this.label4, resources.GetString("label4.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label4.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label4, ((int)(resources.GetObject("label4.IconPadding"))));
            this.label4.Name = "label4";
            this.toolTip1.SetToolTip(this.label4, resources.GetString("label4.ToolTip"));
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
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.dirtyErrorProvider.SetError(this.label1, resources.GetString("label1.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            this.toolTip1.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.dirtyErrorProvider.SetError(this.pictureBox1, resources.GetString("pictureBox1.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.pictureBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("pictureBox1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.pictureBox1, ((int)(resources.GetObject("pictureBox1.IconPadding"))));
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox1, resources.GetString("pictureBox1.ToolTip"));
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.descriptionBox);
            this.dirtyErrorProvider.SetError(this.groupBox1, resources.GetString("groupBox1.Error"));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.dirtyErrorProvider.SetIconAlignment(this.groupBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.groupBox1, ((int)(resources.GetObject("groupBox1.IconPadding"))));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox1, resources.GetString("groupBox1.ToolTip"));
            // 
            // descriptionBox
            // 
            resources.ApplyResources(this.descriptionBox, "descriptionBox");
            this.descriptionBox.BackColor = System.Drawing.SystemColors.Control;
            this.dirtyErrorProvider.SetError(this.descriptionBox, resources.GetString("descriptionBox.Error"));
            this.descriptionBox.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.dirtyErrorProvider.SetIconAlignment(this.descriptionBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("descriptionBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.descriptionBox, ((int)(resources.GetObject("descriptionBox.IconPadding"))));
            this.descriptionBox.Name = "descriptionBox";
            this.descriptionBox.ReadOnly = true;
            this.toolTip1.SetToolTip(this.descriptionBox, resources.GetString("descriptionBox.ToolTip"));
            // 
            // customText4Label
            // 
            resources.ApplyResources(this.customText4Label, "customText4Label");
            this.dirtyErrorProvider.SetError(this.customText4Label, resources.GetString("customText4Label.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.customText4Label, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customText4Label.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.customText4Label, ((int)(resources.GetObject("customText4Label.IconPadding"))));
            this.customText4Label.Name = "customText4Label";
            this.toolTip1.SetToolTip(this.customText4Label, resources.GetString("customText4Label.ToolTip"));
            // 
            // customText2Label
            // 
            resources.ApplyResources(this.customText2Label, "customText2Label");
            this.dirtyErrorProvider.SetError(this.customText2Label, resources.GetString("customText2Label.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.customText2Label, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customText2Label.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.customText2Label, ((int)(resources.GetObject("customText2Label.IconPadding"))));
            this.customText2Label.Name = "customText2Label";
            this.toolTip1.SetToolTip(this.customText2Label, resources.GetString("customText2Label.ToolTip"));
            // 
            // customText4Box
            // 
            resources.ApplyResources(this.customText4Box, "customText4Box");
            this.dirtyErrorProvider.SetError(this.customText4Box, resources.GetString("customText4Box.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.customText4Box, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customText4Box.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.customText4Box, ((int)(resources.GetObject("customText4Box.IconPadding"))));
            this.customText4Box.Name = "customText4Box";
            this.toolTip1.SetToolTip(this.customText4Box, resources.GetString("customText4Box.ToolTip"));
            this.customText4Box.TextChanged += new System.EventHandler(this.customText4Box_TextChanged);
            // 
            // customText2Box
            // 
            resources.ApplyResources(this.customText2Box, "customText2Box");
            this.dirtyErrorProvider.SetError(this.customText2Box, resources.GetString("customText2Box.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.customText2Box, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customText2Box.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.customText2Box, ((int)(resources.GetObject("customText2Box.IconPadding"))));
            this.customText2Box.Name = "customText2Box";
            this.toolTip1.SetToolTip(this.customText2Box, resources.GetString("customText2Box.ToolTip"));
            this.customText2Box.TextChanged += new System.EventHandler(this.customText2Box_TextChanged);
            // 
            // customText3Label
            // 
            resources.ApplyResources(this.customText3Label, "customText3Label");
            this.dirtyErrorProvider.SetError(this.customText3Label, resources.GetString("customText3Label.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.customText3Label, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customText3Label.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.customText3Label, ((int)(resources.GetObject("customText3Label.IconPadding"))));
            this.customText3Label.Name = "customText3Label";
            this.toolTip1.SetToolTip(this.customText3Label, resources.GetString("customText3Label.ToolTip"));
            // 
            // labelTag6
            // 
            resources.ApplyResources(this.labelTag6, "labelTag6");
            this.dirtyErrorProvider.SetError(this.labelTag6, resources.GetString("labelTag6.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.labelTag6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelTag6.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelTag6, ((int)(resources.GetObject("labelTag6.IconPadding"))));
            this.labelTag6.Name = "labelTag6";
            this.toolTip1.SetToolTip(this.labelTag6, resources.GetString("labelTag6.ToolTip"));
            // 
            // customText3Box
            // 
            resources.ApplyResources(this.customText3Box, "customText3Box");
            this.dirtyErrorProvider.SetError(this.customText3Box, resources.GetString("customText3Box.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.customText3Box, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customText3Box.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.customText3Box, ((int)(resources.GetObject("customText3Box.IconPadding"))));
            this.customText3Box.Name = "customText3Box";
            this.toolTip1.SetToolTip(this.customText3Box, resources.GetString("customText3Box.ToolTip"));
            this.customText3Box.TextChanged += new System.EventHandler(this.customText3Box_TextChanged);
            // 
            // parameterTag6List
            // 
            resources.ApplyResources(this.parameterTag6List, "parameterTag6List");
            this.parameterTag6List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterTag6List.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.parameterTag6List, resources.GetString("parameterTag6List.Error"));
            this.parameterTag6List.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.parameterTag6List, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("parameterTag6List.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.parameterTag6List, ((int)(resources.GetObject("parameterTag6List.IconPadding"))));
            this.parameterTag6List.Name = "parameterTag6List";
            this.toolTip1.SetToolTip(this.parameterTag6List, resources.GetString("parameterTag6List.ToolTip"));
            this.parameterTag6List.SelectedIndexChanged += new System.EventHandler(this.parameterTag6List_SelectedIndexChanged);
            // 
            // labelTag5
            // 
            resources.ApplyResources(this.labelTag5, "labelTag5");
            this.dirtyErrorProvider.SetError(this.labelTag5, resources.GetString("labelTag5.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.labelTag5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelTag5.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelTag5, ((int)(resources.GetObject("labelTag5.IconPadding"))));
            this.labelTag5.Name = "labelTag5";
            this.toolTip1.SetToolTip(this.labelTag5, resources.GetString("labelTag5.ToolTip"));
            // 
            // parameterTag5List
            // 
            resources.ApplyResources(this.parameterTag5List, "parameterTag5List");
            this.parameterTag5List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterTag5List.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.parameterTag5List, resources.GetString("parameterTag5List.Error"));
            this.parameterTag5List.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.parameterTag5List, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("parameterTag5List.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.parameterTag5List, ((int)(resources.GetObject("parameterTag5List.IconPadding"))));
            this.parameterTag5List.Name = "parameterTag5List";
            this.toolTip1.SetToolTip(this.parameterTag5List, resources.GetString("parameterTag5List.ToolTip"));
            this.parameterTag5List.SelectedIndexChanged += new System.EventHandler(this.parameterTag5List_SelectedIndexChanged);
            // 
            // labelTag4
            // 
            resources.ApplyResources(this.labelTag4, "labelTag4");
            this.dirtyErrorProvider.SetError(this.labelTag4, resources.GetString("labelTag4.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.labelTag4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelTag4.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelTag4, ((int)(resources.GetObject("labelTag4.IconPadding"))));
            this.labelTag4.Name = "labelTag4";
            this.toolTip1.SetToolTip(this.labelTag4, resources.GetString("labelTag4.ToolTip"));
            // 
            // parameterTag4List
            // 
            resources.ApplyResources(this.parameterTag4List, "parameterTag4List");
            this.parameterTag4List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterTag4List.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.parameterTag4List, resources.GetString("parameterTag4List.Error"));
            this.parameterTag4List.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.parameterTag4List, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("parameterTag4List.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.parameterTag4List, ((int)(resources.GetObject("parameterTag4List.IconPadding"))));
            this.parameterTag4List.Name = "parameterTag4List";
            this.toolTip1.SetToolTip(this.parameterTag4List, resources.GetString("parameterTag4List.ToolTip"));
            this.parameterTag4List.SelectedIndexChanged += new System.EventHandler(this.parameterTag4List_SelectedIndexChanged);
            // 
            // customTextLabel
            // 
            resources.ApplyResources(this.customTextLabel, "customTextLabel");
            this.dirtyErrorProvider.SetError(this.customTextLabel, resources.GetString("customTextLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.customTextLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customTextLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.customTextLabel, ((int)(resources.GetObject("customTextLabel.IconPadding"))));
            this.customTextLabel.Name = "customTextLabel";
            this.toolTip1.SetToolTip(this.customTextLabel, resources.GetString("customTextLabel.ToolTip"));
            // 
            // customTextBox
            // 
            resources.ApplyResources(this.customTextBox, "customTextBox");
            this.dirtyErrorProvider.SetError(this.customTextBox, resources.GetString("customTextBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.customTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customTextBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.customTextBox, ((int)(resources.GetObject("customTextBox.IconPadding"))));
            this.customTextBox.Name = "customTextBox";
            this.toolTip1.SetToolTip(this.customTextBox, resources.GetString("customTextBox.ToolTip"));
            this.customTextBox.TextChanged += new System.EventHandler(this.customText_TextChanged);
            // 
            // labelTag3
            // 
            resources.ApplyResources(this.labelTag3, "labelTag3");
            this.dirtyErrorProvider.SetError(this.labelTag3, resources.GetString("labelTag3.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.labelTag3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelTag3.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelTag3, ((int)(resources.GetObject("labelTag3.IconPadding"))));
            this.labelTag3.Name = "labelTag3";
            this.toolTip1.SetToolTip(this.labelTag3, resources.GetString("labelTag3.ToolTip"));
            // 
            // parameterTag3List
            // 
            resources.ApplyResources(this.parameterTag3List, "parameterTag3List");
            this.parameterTag3List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterTag3List.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.parameterTag3List, resources.GetString("parameterTag3List.Error"));
            this.parameterTag3List.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.parameterTag3List, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("parameterTag3List.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.parameterTag3List, ((int)(resources.GetObject("parameterTag3List.IconPadding"))));
            this.parameterTag3List.Name = "parameterTag3List";
            this.toolTip1.SetToolTip(this.parameterTag3List, resources.GetString("parameterTag3List.ToolTip"));
            this.parameterTag3List.SelectedIndexChanged += new System.EventHandler(this.parameterTag3_SelectedIndexChanged);
            // 
            // labelTag2
            // 
            resources.ApplyResources(this.labelTag2, "labelTag2");
            this.dirtyErrorProvider.SetError(this.labelTag2, resources.GetString("labelTag2.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.labelTag2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelTag2.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelTag2, ((int)(resources.GetObject("labelTag2.IconPadding"))));
            this.labelTag2.Name = "labelTag2";
            this.toolTip1.SetToolTip(this.labelTag2, resources.GetString("labelTag2.ToolTip"));
            // 
            // parameterTag2List
            // 
            resources.ApplyResources(this.parameterTag2List, "parameterTag2List");
            this.parameterTag2List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterTag2List.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.parameterTag2List, resources.GetString("parameterTag2List.Error"));
            this.parameterTag2List.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.parameterTag2List, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("parameterTag2List.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.parameterTag2List, ((int)(resources.GetObject("parameterTag2List.IconPadding"))));
            this.parameterTag2List.Name = "parameterTag2List";
            this.toolTip1.SetToolTip(this.parameterTag2List, resources.GetString("parameterTag2List.ToolTip"));
            this.parameterTag2List.SelectedIndexChanged += new System.EventHandler(this.parameterTag2_SelectedIndexChanged);
            // 
            // labelTag
            // 
            resources.ApplyResources(this.labelTag, "labelTag");
            this.dirtyErrorProvider.SetError(this.labelTag, resources.GetString("labelTag.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.labelTag, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelTag.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelTag, ((int)(resources.GetObject("labelTag.IconPadding"))));
            this.labelTag.Name = "labelTag";
            this.toolTip1.SetToolTip(this.labelTag, resources.GetString("labelTag.ToolTip"));
            // 
            // parameterTagList
            // 
            resources.ApplyResources(this.parameterTagList, "parameterTagList");
            this.parameterTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterTagList.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.parameterTagList, resources.GetString("parameterTagList.Error"));
            this.parameterTagList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.parameterTagList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("parameterTagList.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.parameterTagList, ((int)(resources.GetObject("parameterTagList.IconPadding"))));
            this.parameterTagList.Name = "parameterTagList";
            this.toolTip1.SetToolTip(this.parameterTagList, resources.GetString("parameterTagList.ToolTip"));
            this.parameterTagList.SelectedIndexChanged += new System.EventHandler(this.parameterTag_SelectedIndexChanged);
            // 
            // playlistComboBox
            // 
            resources.ApplyResources(this.playlistComboBox, "playlistComboBox");
            this.playlistComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playlistComboBox.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.playlistComboBox, resources.GetString("playlistComboBox.Error"));
            this.playlistComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.playlistComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("playlistComboBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.playlistComboBox, ((int)(resources.GetObject("playlistComboBox.IconPadding"))));
            this.playlistComboBox.Name = "playlistComboBox";
            this.toolTip1.SetToolTip(this.playlistComboBox, resources.GetString("playlistComboBox.ToolTip"));
            this.playlistComboBox.SelectedIndexChanged += new System.EventHandler(this.playlistComboBox_SelectedIndexChanged);
            // 
            // conditionCheckBox
            // 
            resources.ApplyResources(this.conditionCheckBox, "conditionCheckBox");
            this.dirtyErrorProvider.SetError(this.conditionCheckBox, resources.GetString("conditionCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.conditionCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("conditionCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.conditionCheckBox, ((int)(resources.GetObject("conditionCheckBox.IconPadding"))));
            this.conditionCheckBox.Name = "conditionCheckBox";
            this.toolTip1.SetToolTip(this.conditionCheckBox, resources.GetString("conditionCheckBox.ToolTip"));
            this.conditionCheckBox.UseVisualStyleBackColor = true;
            this.conditionCheckBox.CheckedChanged += new System.EventHandler(this.conditionCheckBox_CheckedChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.AutoEllipsis = true;
            this.dirtyErrorProvider.SetError(this.label5, resources.GetString("label5.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label5.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label5, ((int)(resources.GetObject("label5.IconPadding"))));
            this.label5.Name = "label5";
            this.toolTip1.SetToolTip(this.label5, resources.GetString("label5.ToolTip"));
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.buttonSaveClose);
            this.groupBox3.Controls.Add(this.buttonOK);
            this.groupBox3.Controls.Add(this.buttonPreview);
            this.groupBox3.Controls.Add(this.buttonClose);
            this.dirtyErrorProvider.SetError(this.groupBox3, resources.GetString("groupBox3.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.groupBox3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox3.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.groupBox3, ((int)(resources.GetObject("groupBox3.IconPadding"))));
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox3, resources.GetString("groupBox3.ToolTip"));
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.dirtyErrorProvider.SetError(this.label7, resources.GetString("label7.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label7, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label7.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label7, ((int)(resources.GetObject("label7.IconPadding"))));
            this.label7.Name = "label7";
            this.toolTip1.SetToolTip(this.label7, resources.GetString("label7.ToolTip"));
            // 
            // buttonSaveClose
            // 
            resources.ApplyResources(this.buttonSaveClose, "buttonSaveClose");
            this.buttonSaveClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dirtyErrorProvider.SetError(this.buttonSaveClose, resources.GetString("buttonSaveClose.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonSaveClose, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonSaveClose.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonSaveClose, ((int)(resources.GetObject("buttonSaveClose.IconPadding"))));
            this.buttonSaveClose.Name = "buttonSaveClose";
            this.toolTip1.SetToolTip(this.buttonSaveClose, resources.GetString("buttonSaveClose.ToolTip"));
            this.buttonSaveClose.UseVisualStyleBackColor = true;
            this.buttonSaveClose.Click += new System.EventHandler(this.buttonSaveClose_Click);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.previewTable);
            this.dirtyErrorProvider.SetError(this.panel2, resources.GetString("panel2.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.panel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("panel2.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.panel2, ((int)(resources.GetObject("panel2.IconPadding"))));
            this.panel2.Name = "panel2";
            this.toolTip1.SetToolTip(this.panel2, resources.GetString("panel2.ToolTip"));
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.buttonExport);
            this.groupBox2.Controls.Add(this.buttonDeleteAll);
            this.groupBox2.Controls.Add(this.buttonDelete);
            this.groupBox2.Controls.Add(this.buttonEdit);
            this.groupBox2.Controls.Add(this.buttonCreate);
            this.groupBox2.Controls.Add(this.buttonImport);
            this.groupBox2.Controls.Add(this.buttonImportNew);
            this.groupBox2.Controls.Add(this.buttonImportAll);
            this.groupBox2.Controls.Add(this.buttonExportCustom);
            this.groupBox2.Controls.Add(this.buttonCopy);
            this.dirtyErrorProvider.SetError(this.groupBox2, resources.GetString("groupBox2.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.groupBox2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox2.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.groupBox2, ((int)(resources.GetObject("groupBox2.IconPadding"))));
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox2, resources.GetString("groupBox2.ToolTip"));
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.dirtyErrorProvider.SetError(this.label6, resources.GetString("label6.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label6.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label6, ((int)(resources.GetObject("label6.IconPadding"))));
            this.label6.Name = "label6";
            this.toolTip1.SetToolTip(this.label6, resources.GetString("label6.ToolTip"));
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.dirtyErrorProvider.SetError(this.tableLayoutPanel1, resources.GetString("tableLayoutPanel1.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.tableLayoutPanel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tableLayoutPanel1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.tableLayoutPanel1, ((int)(resources.GetObject("tableLayoutPanel1.IconPadding"))));
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.toolTip1.SetToolTip(this.tableLayoutPanel1, resources.GetString("tableLayoutPanel1.ToolTip"));
            // 
            // dirtyErrorProvider
            // 
            this.dirtyErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.dirtyErrorProvider.ContainerControl = this;
            resources.ApplyResources(this.dirtyErrorProvider, "dirtyErrorProvider");
            // 
            // AdvancedSearchAndReplaceCommand
            // 
            this.AcceptButton = this.buttonPreview;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "AdvancedSearchAndReplaceCommand";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AdvancedSearchAndReplaceCommand_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.hotkeyPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playlistPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customizedPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.predefinedPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tickedOnlyPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uncheckAllFiltersPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.functionIdPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userPresetPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customizedPresetPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button buttonDeleteAll;
        private System.Windows.Forms.Button buttonImportNew;
        private System.Windows.Forms.Button buttonImportAll;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.Button buttonExportCustom;
        private System.Windows.Forms.DataGridView previewTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn File;
        private System.Windows.Forms.DataGridViewTextBoxColumn Track;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTag1;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTag1;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTag2;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTag2;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTag3;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTag3;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTag4;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTag4;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTag5;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTag5;
        private System.Windows.Forms.ErrorProvider dirtyErrorProvider;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox descriptionBox;
        private System.Windows.Forms.Label customText4Label;
        private System.Windows.Forms.Label customText2Label;
        private System.Windows.Forms.TextBox customText4Box;
        private System.Windows.Forms.TextBox customText2Box;
        private System.Windows.Forms.Label customText3Label;
        private System.Windows.Forms.Label labelTag6;
        private System.Windows.Forms.TextBox customText3Box;
        private System.Windows.Forms.ComboBox parameterTag6List;
        private System.Windows.Forms.Label labelTag5;
        private System.Windows.Forms.ComboBox parameterTag5List;
        private System.Windows.Forms.Label labelTag4;
        private System.Windows.Forms.ComboBox parameterTag4List;
        private System.Windows.Forms.Label customTextLabel;
        private System.Windows.Forms.TextBox customTextBox;
        private System.Windows.Forms.Label labelTag3;
        private System.Windows.Forms.ComboBox parameterTag3List;
        private System.Windows.Forms.Label labelTag2;
        private System.Windows.Forms.ComboBox parameterTag2List;
        private System.Windows.Forms.Label labelTag;
        private System.Windows.Forms.ComboBox parameterTagList;
        private System.Windows.Forms.ComboBox playlistComboBox;
        private System.Windows.Forms.CheckBox conditionCheckBox;
        private System.Windows.Forms.CheckedListBox presetList;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button clearSearchButton;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.CheckBox assignHotkeyCheckBox;
        private System.Windows.Forms.Button clearIdButton;
        private System.Windows.Forms.TextBox idTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox applyToPlayingTrackCheckBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox preserveValuesTextBox;
        private System.Windows.Forms.Button buttonSaveClose;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox filterComboBox;
        private System.Windows.Forms.PictureBox tickedOnlyPictureBox;
        private System.Windows.Forms.PictureBox predefinedPictureBox;
        private System.Windows.Forms.PictureBox customizedPictureBox;
        private System.Windows.Forms.PictureBox userPictureBox;
        private System.Windows.Forms.PictureBox playlistPictureBox;
        private System.Windows.Forms.PictureBox functionIdPictureBox;
        private System.Windows.Forms.PictureBox hotkeyPictureBox;
        private System.Windows.Forms.PictureBox uncheckAllFiltersPictureBox;
        private System.Windows.Forms.PictureBox userPresetPictureBox;
        private System.Windows.Forms.PictureBox customizedPresetPictureBox;
    }
}