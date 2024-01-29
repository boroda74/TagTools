using ExtensionMethods;
using System.Drawing.Drawing2D;

namespace MusicBeePlugin
{
    partial class AdvancedSearchAndReplaceCommand
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedSearchAndReplaceCommand));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.preserveTagValuesTextBox = new System.Windows.Forms.TextBox();
            this.labelPreserveTagValues = new System.Windows.Forms.Label();
            this.buttonSelectPreservedTags = new System.Windows.Forms.Button();
            this.processPreserveTagsTextBox = new System.Windows.Forms.TextBox();
            this.buttonProcessPreserveTags = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.customText4Box = new System.Windows.Forms.TextBox();
            this.customText4Label = new System.Windows.Forms.Label();
            this.customText3Box = new System.Windows.Forms.TextBox();
            this.customText3Label = new System.Windows.Forms.Label();
            this.customText2Box = new System.Windows.Forms.TextBox();
            this.customText2Label = new System.Windows.Forms.Label();
            this.customTextBox = new System.Windows.Forms.TextBox();
            this.customTextLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.parameterTag6List = new System.Windows.Forms.ComboBox();
            this.labelTag6 = new System.Windows.Forms.Label();
            this.pictureTag6 = new System.Windows.Forms.PictureBox();
            this.parameterTag5List = new System.Windows.Forms.ComboBox();
            this.labelTag5 = new System.Windows.Forms.Label();
            this.pictureTag5 = new System.Windows.Forms.PictureBox();
            this.parameterTag4List = new System.Windows.Forms.ComboBox();
            this.labelTag4 = new System.Windows.Forms.Label();
            this.pictureTag4 = new System.Windows.Forms.PictureBox();
            this.parameterTag3List = new System.Windows.Forms.ComboBox();
            this.labelTag3 = new System.Windows.Forms.Label();
            this.pictureTag3 = new System.Windows.Forms.PictureBox();
            this.parameterTag2List = new System.Windows.Forms.ComboBox();
            this.labelTag2 = new System.Windows.Forms.Label();
            this.pictureTag2 = new System.Windows.Forms.PictureBox();
            this.parameterTagList = new System.Windows.Forms.ComboBox();
            this.labelTag = new System.Windows.Forms.Label();
            this.pictureTag = new System.Windows.Forms.PictureBox();
            this.userPresetLabel = new System.Windows.Forms.Label();
            this.userPresetPictureBox = new System.Windows.Forms.PictureBox();
            this.customizedPresetLabel = new System.Windows.Forms.Label();
            this.customizedPresetPictureBox = new System.Windows.Forms.PictureBox();
            this.favoriteCheckBoxLabel = new System.Windows.Forms.Label();
            this.favoriteCheckBox = new System.Windows.Forms.CheckBox();
            this.applyToPlayingTrackCheckBoxLabel = new System.Windows.Forms.Label();
            this.applyToPlayingTrackCheckBox = new System.Windows.Forms.CheckBox();
            this.assignHotkeyCheckBoxLabel = new System.Windows.Forms.Label();
            this.assignHotkeyCheckBox = new System.Windows.Forms.CheckBox();
            this.clearIdButton = new System.Windows.Forms.Button();
            this.idTextBox = new System.Windows.Forms.TextBox();
            this.idLabel = new System.Windows.Forms.Label();
            this.playlistComboBox = new System.Windows.Forms.ComboBox();
            this.conditionCheckBoxLabel = new System.Windows.Forms.Label();
            this.conditionCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.descriptionBox = new System.Windows.Forms.TextBox();
            this.groupBox1Label = new System.Windows.Forms.Label();
            this.presetList = new System.Windows.Forms.CheckedListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.previewTable = new System.Windows.Forms.DataGridView();
            this.PresetGuid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.File = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Track = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TagName1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalTag1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewTag1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TagName2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalTag2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewTag2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TagName3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalTag3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewTag3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TagName4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalTag4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewTag4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TagName5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OriginalTag5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewTag5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OddEven = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.settingsProcessingGroupBox = new System.Windows.Forms.GroupBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonSaveClose = new System.Windows.Forms.Button();
            this.settingsProcessingLabel = new System.Windows.Forms.Label();
            this.presetManagementGroupBox = new System.Windows.Forms.GroupBox();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.buttonExportCustom = new System.Windows.Forms.Button();
            this.buttonDeleteAll = new System.Windows.Forms.Button();
            this.buttonImportAll = new System.Windows.Forms.Button();
            this.buttonImportNew = new System.Windows.Forms.Button();
            this.buttonImport = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.presetManagementLabel = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.filterComboBox = new System.Windows.Forms.ComboBox();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.uncheckAllFiltersPictureBox = new System.Windows.Forms.PictureBox();
            this.hotkeyPictureBox = new System.Windows.Forms.PictureBox();
            this.functionIdPictureBox = new System.Windows.Forms.PictureBox();
            this.playlistPictureBox = new System.Windows.Forms.PictureBox();
            this.userPictureBox = new System.Windows.Forms.PictureBox();
            this.customizedPictureBox = new System.Windows.Forms.PictureBox();
            this.predefinedPictureBox = new System.Windows.Forms.PictureBox();
            this.tickedOnlyPictureBox = new System.Windows.Forms.PictureBox();
            this.clearSearchButton = new System.Windows.Forms.Button();
            this.autoApplyPresetsLabel = new System.Windows.Forms.Label();
            this.searchPictureBox = new System.Windows.Forms.PictureBox();
            this.filtersPanel = new System.Windows.Forms.Panel();
            this.dirtyErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTag6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTag5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTag4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTag3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTag2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userPresetPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customizedPresetPictureBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).BeginInit();
            this.settingsProcessingGroupBox.SuspendLayout();
            this.presetManagementGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uncheckAllFiltersPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hotkeyPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.functionIdPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playlistPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customizedPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.predefinedPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tickedOnlyPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchPictureBox)).BeginInit();
            this.filtersPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).BeginInit();
            this.SuspendLayout();

            //MusicBee
            this.searchTextBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.customTextBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.customText2Box = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.customText3Box = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.customText4Box = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.idTextBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.preserveTagValuesTextBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);


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
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.BackColor = System.Drawing.SystemColors.GrayText;
            this.dirtyErrorProvider.SetIconAlignment(this.splitContainer1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer1.IconAlignment"))));
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel4);
            this.dirtyErrorProvider.SetIconAlignment(this.splitContainer1.Panel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer1.Panel1.IconAlignment"))));
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.dirtyErrorProvider.SetIconAlignment(this.splitContainer1.Panel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer1.Panel2.IconAlignment"))));
            this.splitContainer1.Tag = "#AdvancedSearchAndReplaceCommand&AdvancedSearchAndReplaceCommand@pinned-to-parent" +
    "-x";
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.presetList, 0, 0);
            this.dirtyErrorProvider.SetIconAlignment(this.tableLayoutPanel4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tableLayoutPanel4.IconAlignment"))));
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.tableLayoutPanel3);
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Controls.Add(this.userPresetLabel);
            this.panel1.Controls.Add(this.userPresetPictureBox);
            this.panel1.Controls.Add(this.customizedPresetLabel);
            this.panel1.Controls.Add(this.customizedPresetPictureBox);
            this.panel1.Controls.Add(this.favoriteCheckBoxLabel);
            this.panel1.Controls.Add(this.favoriteCheckBox);
            this.panel1.Controls.Add(this.applyToPlayingTrackCheckBoxLabel);
            this.panel1.Controls.Add(this.applyToPlayingTrackCheckBox);
            this.panel1.Controls.Add(this.assignHotkeyCheckBoxLabel);
            this.panel1.Controls.Add(this.assignHotkeyCheckBox);
            this.panel1.Controls.Add(this.clearIdButton);
            this.panel1.Controls.Add(this.idTextBox);
            this.panel1.Controls.Add(this.idLabel);
            this.panel1.Controls.Add(this.playlistComboBox);
            this.panel1.Controls.Add(this.conditionCheckBoxLabel);
            this.panel1.Controls.Add(this.conditionCheckBox);
            this.panel1.Controls.Add(this.groupBox1);
            this.dirtyErrorProvider.SetIconAlignment(this.panel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("panel1.IconAlignment"))));
            this.panel1.Name = "panel1";
            this.panel1.Tag = "";
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.preserveTagValuesTextBox, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.labelPreserveTagValues, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonSelectPreservedTags, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.processPreserveTagsTextBox, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonProcessPreserveTags, 0, 0);
            this.dirtyErrorProvider.SetIconAlignment(this.tableLayoutPanel3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tableLayoutPanel3.IconAlignment"))));
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // preserveTagValuesTextBox
            // 
            resources.ApplyResources(this.preserveTagValuesTextBox, "preserveTagValuesTextBox");
            this.dirtyErrorProvider.SetIconAlignment(this.preserveTagValuesTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("preserveTagValuesTextBox.IconAlignment"))));
            this.preserveTagValuesTextBox.Name = "preserveTagValuesTextBox";
            this.toolTip1.SetToolTip(this.preserveTagValuesTextBox, resources.GetString("preserveTagValuesTextBox.ToolTip"));
            this.preserveTagValuesTextBox.TextChanged += new System.EventHandler(this.preserveTagValuesTextBox_TextChanged);
            // 
            // labelPreserveTagValues
            // 
            resources.ApplyResources(this.labelPreserveTagValues, "labelPreserveTagValues");
            this.dirtyErrorProvider.SetIconAlignment(this.labelPreserveTagValues, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelPreserveTagValues.IconAlignment"))));
            this.labelPreserveTagValues.Name = "labelPreserveTagValues";
            this.toolTip1.SetToolTip(this.labelPreserveTagValues, resources.GetString("labelPreserveTagValues.ToolTip"));
            // 
            // buttonSelectPreservedTags
            // 
            resources.ApplyResources(this.buttonSelectPreservedTags, "buttonSelectPreservedTags");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonSelectPreservedTags, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonSelectPreservedTags.IconAlignment"))));
            this.buttonSelectPreservedTags.Name = "buttonSelectPreservedTags";
            this.buttonSelectPreservedTags.Tag = "@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonSelectPreservedTags, resources.GetString("buttonSelectPreservedTags.ToolTip"));
            this.buttonSelectPreservedTags.Click += new System.EventHandler(this.buttonSelectPreservedTags_Click);
            // 
            // processPreserveTagsTextBox
            // 
            resources.ApplyResources(this.processPreserveTagsTextBox, "processPreserveTagsTextBox");
            this.dirtyErrorProvider.SetIconAlignment(this.processPreserveTagsTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("processPreserveTagsTextBox.IconAlignment"))));
            this.processPreserveTagsTextBox.Name = "processPreserveTagsTextBox";
            this.toolTip1.SetToolTip(this.processPreserveTagsTextBox, resources.GetString("processPreserveTagsTextBox.ToolTip"));
            this.processPreserveTagsTextBox.TextChanged += new System.EventHandler(this.processPreserveTagsTextBox_TextChanged);
            // 
            // buttonProcessPreserveTags
            // 
            resources.ApplyResources(this.buttonProcessPreserveTags, "buttonProcessPreserveTags");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonProcessPreserveTags, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonProcessPreserveTags.IconAlignment"))));
            this.buttonProcessPreserveTags.Name = "buttonProcessPreserveTags";
            this.buttonProcessPreserveTags.Tag = "@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonProcessPreserveTags, resources.GetString("buttonProcessPreserveTags.ToolTip"));
            this.buttonProcessPreserveTags.Click += new System.EventHandler(this.buttonProcessPreserveTags_Click);
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.customText4Box, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.customText4Label, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.customText3Box, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.customText3Label, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.customText2Box, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.customText2Label, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.customTextBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.customTextLabel, 0, 0);
            this.dirtyErrorProvider.SetIconAlignment(this.tableLayoutPanel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tableLayoutPanel2.IconAlignment"))));
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // customText4Box
            // 
            resources.ApplyResources(this.customText4Box, "customText4Box");
            this.dirtyErrorProvider.SetIconAlignment(this.customText4Box, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customText4Box.IconAlignment"))));
            this.customText4Box.Name = "customText4Box";
            this.customText4Box.TextChanged += new System.EventHandler(this.customText4Box_TextChanged);
            // 
            // customText4Label
            // 
            resources.ApplyResources(this.customText4Label, "customText4Label");
            this.dirtyErrorProvider.SetIconAlignment(this.customText4Label, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customText4Label.IconAlignment"))));
            this.customText4Label.Name = "customText4Label";
            // 
            // customText3Box
            // 
            resources.ApplyResources(this.customText3Box, "customText3Box");
            this.dirtyErrorProvider.SetIconAlignment(this.customText3Box, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customText3Box.IconAlignment"))));
            this.customText3Box.Name = "customText3Box";
            this.customText3Box.TextChanged += new System.EventHandler(this.customText3Box_TextChanged);
            // 
            // customText3Label
            // 
            resources.ApplyResources(this.customText3Label, "customText3Label");
            this.dirtyErrorProvider.SetIconAlignment(this.customText3Label, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customText3Label.IconAlignment"))));
            this.customText3Label.Name = "customText3Label";
            // 
            // customText2Box
            // 
            resources.ApplyResources(this.customText2Box, "customText2Box");
            this.dirtyErrorProvider.SetIconAlignment(this.customText2Box, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customText2Box.IconAlignment"))));
            this.customText2Box.Name = "customText2Box";
            this.customText2Box.TextChanged += new System.EventHandler(this.customText2Box_TextChanged);
            // 
            // customText2Label
            // 
            resources.ApplyResources(this.customText2Label, "customText2Label");
            this.dirtyErrorProvider.SetIconAlignment(this.customText2Label, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customText2Label.IconAlignment"))));
            this.customText2Label.Name = "customText2Label";
            // 
            // customTextBox
            // 
            resources.ApplyResources(this.customTextBox, "customTextBox");
            this.dirtyErrorProvider.SetIconAlignment(this.customTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customTextBox.IconAlignment"))));
            this.customTextBox.Name = "customTextBox";
            this.customTextBox.TextChanged += new System.EventHandler(this.customTextBox_TextChanged);
            // 
            // customTextLabel
            // 
            resources.ApplyResources(this.customTextLabel, "customTextLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.customTextLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customTextLabel.IconAlignment"))));
            this.customTextLabel.Name = "customTextLabel";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.parameterTag6List, 8, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelTag6, 7, 1);
            this.tableLayoutPanel1.Controls.Add(this.pictureTag6, 6, 1);
            this.tableLayoutPanel1.Controls.Add(this.parameterTag5List, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelTag5, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.pictureTag5, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.parameterTag4List, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelTag4, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.pictureTag4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.parameterTag3List, 8, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelTag3, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureTag3, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.parameterTag2List, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelTag2, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureTag2, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.parameterTagList, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelTag, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureTag, 0, 0);
            this.dirtyErrorProvider.SetIconAlignment(this.tableLayoutPanel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tableLayoutPanel1.IconAlignment"))));
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // parameterTag6List
            // 
            resources.ApplyResources(this.parameterTag6List, "parameterTag6List");
            this.parameterTag6List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterTag6List.DropDownWidth = 250;
            this.parameterTag6List.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.parameterTag6List, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("parameterTag6List.IconAlignment"))));
            this.parameterTag6List.Name = "parameterTag6List";
            this.parameterTag6List.SelectedIndexChanged += new System.EventHandler(this.parameterTag6_SelectedIndexChanged);
            // 
            // labelTag6
            // 
            resources.ApplyResources(this.labelTag6, "labelTag6");
            this.dirtyErrorProvider.SetIconAlignment(this.labelTag6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelTag6.IconAlignment"))));
            this.labelTag6.Name = "labelTag6";
            // 
            // pictureTag6
            // 
            resources.ApplyResources(this.pictureTag6, "pictureTag6");
            this.dirtyErrorProvider.SetIconAlignment(this.pictureTag6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("pictureTag6.IconAlignment"))));
            this.pictureTag6.Image = global::MusicBeePlugin.Properties.Resources.warning_15;
            this.pictureTag6.Name = "pictureTag6";
            this.pictureTag6.TabStop = false;
            // 
            // parameterTag5List
            // 
            resources.ApplyResources(this.parameterTag5List, "parameterTag5List");
            this.parameterTag5List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterTag5List.DropDownWidth = 250;
            this.parameterTag5List.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.parameterTag5List, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("parameterTag5List.IconAlignment"))));
            this.parameterTag5List.Name = "parameterTag5List";
            this.parameterTag5List.SelectedIndexChanged += new System.EventHandler(this.parameterTag5_SelectedIndexChanged);
            // 
            // labelTag5
            // 
            resources.ApplyResources(this.labelTag5, "labelTag5");
            this.dirtyErrorProvider.SetIconAlignment(this.labelTag5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelTag5.IconAlignment"))));
            this.labelTag5.Name = "labelTag5";
            // 
            // pictureTag5
            // 
            resources.ApplyResources(this.pictureTag5, "pictureTag5");
            this.dirtyErrorProvider.SetIconAlignment(this.pictureTag5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("pictureTag5.IconAlignment"))));
            this.pictureTag5.Image = global::MusicBeePlugin.Properties.Resources.warning_15;
            this.pictureTag5.Name = "pictureTag5";
            this.pictureTag5.TabStop = false;
            // 
            // parameterTag4List
            // 
            resources.ApplyResources(this.parameterTag4List, "parameterTag4List");
            this.parameterTag4List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterTag4List.DropDownWidth = 250;
            this.parameterTag4List.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.parameterTag4List, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("parameterTag4List.IconAlignment"))));
            this.parameterTag4List.Name = "parameterTag4List";
            this.parameterTag4List.SelectedIndexChanged += new System.EventHandler(this.parameterTag4_SelectedIndexChanged);
            // 
            // labelTag4
            // 
            resources.ApplyResources(this.labelTag4, "labelTag4");
            this.dirtyErrorProvider.SetIconAlignment(this.labelTag4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelTag4.IconAlignment"))));
            this.labelTag4.Name = "labelTag4";
            // 
            // pictureTag4
            // 
            resources.ApplyResources(this.pictureTag4, "pictureTag4");
            this.dirtyErrorProvider.SetIconAlignment(this.pictureTag4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("pictureTag4.IconAlignment"))));
            this.pictureTag4.Image = global::MusicBeePlugin.Properties.Resources.warning_15;
            this.pictureTag4.Name = "pictureTag4";
            this.pictureTag4.TabStop = false;
            // 
            // parameterTag3List
            // 
            resources.ApplyResources(this.parameterTag3List, "parameterTag3List");
            this.parameterTag3List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterTag3List.DropDownWidth = 250;
            this.parameterTag3List.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.parameterTag3List, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("parameterTag3List.IconAlignment"))));
            this.parameterTag3List.Name = "parameterTag3List";
            this.parameterTag3List.SelectedIndexChanged += new System.EventHandler(this.parameterTag3_SelectedIndexChanged);
            // 
            // labelTag3
            // 
            resources.ApplyResources(this.labelTag3, "labelTag3");
            this.dirtyErrorProvider.SetIconAlignment(this.labelTag3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelTag3.IconAlignment"))));
            this.labelTag3.Name = "labelTag3";
            // 
            // pictureTag3
            // 
            resources.ApplyResources(this.pictureTag3, "pictureTag3");
            this.dirtyErrorProvider.SetIconAlignment(this.pictureTag3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("pictureTag3.IconAlignment"))));
            this.pictureTag3.Image = global::MusicBeePlugin.Properties.Resources.warning_15;
            this.pictureTag3.Name = "pictureTag3";
            this.pictureTag3.TabStop = false;
            // 
            // parameterTag2List
            // 
            resources.ApplyResources(this.parameterTag2List, "parameterTag2List");
            this.parameterTag2List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterTag2List.DropDownWidth = 250;
            this.parameterTag2List.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.parameterTag2List, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("parameterTag2List.IconAlignment"))));
            this.parameterTag2List.Name = "parameterTag2List";
            this.parameterTag2List.SelectedIndexChanged += new System.EventHandler(this.parameterTag2_SelectedIndexChanged);
            // 
            // labelTag2
            // 
            resources.ApplyResources(this.labelTag2, "labelTag2");
            this.dirtyErrorProvider.SetIconAlignment(this.labelTag2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelTag2.IconAlignment"))));
            this.labelTag2.Name = "labelTag2";
            // 
            // pictureTag2
            // 
            resources.ApplyResources(this.pictureTag2, "pictureTag2");
            this.dirtyErrorProvider.SetIconAlignment(this.pictureTag2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("pictureTag2.IconAlignment"))));
            this.pictureTag2.Image = global::MusicBeePlugin.Properties.Resources.warning_15;
            this.pictureTag2.Name = "pictureTag2";
            this.pictureTag2.TabStop = false;
            // 
            // parameterTagList
            // 
            resources.ApplyResources(this.parameterTagList, "parameterTagList");
            this.parameterTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterTagList.DropDownWidth = 250;
            this.parameterTagList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.parameterTagList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("parameterTagList.IconAlignment"))));
            this.parameterTagList.Name = "parameterTagList";
            this.parameterTagList.SelectedIndexChanged += new System.EventHandler(this.parameterTag_SelectedIndexChanged);
            // 
            // labelTag
            // 
            resources.ApplyResources(this.labelTag, "labelTag");
            this.dirtyErrorProvider.SetIconAlignment(this.labelTag, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelTag.IconAlignment"))));
            this.labelTag.Name = "labelTag";
            this.toolTip1.SetToolTip(this.labelTag, resources.GetString("labelTag.ToolTip"));
            // 
            // pictureTag
            // 
            resources.ApplyResources(this.pictureTag, "pictureTag");
            this.dirtyErrorProvider.SetIconAlignment(this.pictureTag, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("pictureTag.IconAlignment"))));
            this.pictureTag.Image = global::MusicBeePlugin.Properties.Resources.warning_15;
            this.pictureTag.Name = "pictureTag";
            this.pictureTag.TabStop = false;
            // 
            // userPresetLabel
            // 
            resources.ApplyResources(this.userPresetLabel, "userPresetLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.userPresetLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("userPresetLabel.IconAlignment"))));
            this.userPresetLabel.Name = "userPresetLabel";
            this.userPresetLabel.Tag = "#splitContainer1@pinned-to-parent-x";
            // 
            // userPresetPictureBox
            // 
            resources.ApplyResources(this.userPresetPictureBox, "userPresetPictureBox");
            this.dirtyErrorProvider.SetIconAlignment(this.userPresetPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("userPresetPictureBox.IconAlignment"))));
            this.userPresetPictureBox.Image = global::MusicBeePlugin.Properties.Resources.clear_button_15;
            this.userPresetPictureBox.Name = "userPresetPictureBox";
            this.userPresetPictureBox.TabStop = false;
            this.userPresetPictureBox.Tag = "#userPresetLabel@square-control@small-picture";
            // 
            // customizedPresetLabel
            // 
            resources.ApplyResources(this.customizedPresetLabel, "customizedPresetLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.customizedPresetLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customizedPresetLabel.IconAlignment"))));
            this.customizedPresetLabel.Name = "customizedPresetLabel";
            this.customizedPresetLabel.Tag = "#userPresetPictureBox";
            // 
            // customizedPresetPictureBox
            // 
            resources.ApplyResources(this.customizedPresetPictureBox, "customizedPresetPictureBox");
            this.dirtyErrorProvider.SetIconAlignment(this.customizedPresetPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customizedPresetPictureBox.IconAlignment"))));
            this.customizedPresetPictureBox.Image = global::MusicBeePlugin.Properties.Resources.clear_button_15;
            this.customizedPresetPictureBox.Name = "customizedPresetPictureBox";
            this.customizedPresetPictureBox.TabStop = false;
            this.customizedPresetPictureBox.Tag = "#customizedPresetLabel@square-control@small-picture";
            // 
            // favoriteCheckBoxLabel
            // 
            resources.ApplyResources(this.favoriteCheckBoxLabel, "favoriteCheckBoxLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.favoriteCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("favoriteCheckBoxLabel.IconAlignment"))));
            this.favoriteCheckBoxLabel.Name = "favoriteCheckBoxLabel";
            this.favoriteCheckBoxLabel.Tag = "";
            this.favoriteCheckBoxLabel.Click += new System.EventHandler(this.favoriteCheckBoxLabel_Click);
            // 
            // favoriteCheckBox
            // 
            resources.ApplyResources(this.favoriteCheckBox, "favoriteCheckBox");
            this.dirtyErrorProvider.SetIconAlignment(this.favoriteCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("favoriteCheckBox.IconAlignment"))));
            this.favoriteCheckBox.Name = "favoriteCheckBox";
            this.favoriteCheckBox.Tag = "#favoriteCheckBoxLabel";
            this.favoriteCheckBox.CheckedChanged += new System.EventHandler(this.favoriteCheckBox_CheckedChanged);
            // 
            // applyToPlayingTrackCheckBoxLabel
            // 
            resources.ApplyResources(this.applyToPlayingTrackCheckBoxLabel, "applyToPlayingTrackCheckBoxLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.applyToPlayingTrackCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("applyToPlayingTrackCheckBoxLabel.IconAlignment"))));
            this.applyToPlayingTrackCheckBoxLabel.Name = "applyToPlayingTrackCheckBoxLabel";
            this.applyToPlayingTrackCheckBoxLabel.Tag = "#favoriteCheckBox";
            this.toolTip1.SetToolTip(this.applyToPlayingTrackCheckBoxLabel, resources.GetString("applyToPlayingTrackCheckBoxLabel.ToolTip"));
            this.applyToPlayingTrackCheckBoxLabel.Click += new System.EventHandler(this.applyToPlayingTrackCheckBoxLabel_Click);
            // 
            // applyToPlayingTrackCheckBox
            // 
            resources.ApplyResources(this.applyToPlayingTrackCheckBox, "applyToPlayingTrackCheckBox");
            this.dirtyErrorProvider.SetIconAlignment(this.applyToPlayingTrackCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("applyToPlayingTrackCheckBox.IconAlignment"))));
            this.applyToPlayingTrackCheckBox.Name = "applyToPlayingTrackCheckBox";
            this.applyToPlayingTrackCheckBox.Tag = "#applyToPlayingTrackCheckBoxLabel";
            this.toolTip1.SetToolTip(this.applyToPlayingTrackCheckBox, resources.GetString("applyToPlayingTrackCheckBox.ToolTip"));
            this.applyToPlayingTrackCheckBox.CheckedChanged += new System.EventHandler(this.applyToPlayingTrackCheckBox_CheckedChanged);
            // 
            // assignHotkeyCheckBoxLabel
            // 
            resources.ApplyResources(this.assignHotkeyCheckBoxLabel, "assignHotkeyCheckBoxLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.assignHotkeyCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("assignHotkeyCheckBoxLabel.IconAlignment"))));
            this.assignHotkeyCheckBoxLabel.Name = "assignHotkeyCheckBoxLabel";
            this.assignHotkeyCheckBoxLabel.Tag = "#applyToPlayingTrackCheckBox";
            this.toolTip1.SetToolTip(this.assignHotkeyCheckBoxLabel, resources.GetString("assignHotkeyCheckBoxLabel.ToolTip"));
            this.assignHotkeyCheckBoxLabel.Click += new System.EventHandler(this.assignHotkeyCheckBoxLabel_Click);
            // 
            // assignHotkeyCheckBox
            // 
            resources.ApplyResources(this.assignHotkeyCheckBox, "assignHotkeyCheckBox");
            this.dirtyErrorProvider.SetIconAlignment(this.assignHotkeyCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("assignHotkeyCheckBox.IconAlignment"))));
            this.assignHotkeyCheckBox.Name = "assignHotkeyCheckBox";
            this.assignHotkeyCheckBox.Tag = "#assignHotkeyCheckBoxLabel";
            this.toolTip1.SetToolTip(this.assignHotkeyCheckBox, resources.GetString("assignHotkeyCheckBox.ToolTip"));
            this.assignHotkeyCheckBox.CheckedChanged += new System.EventHandler(this.assignHotkeyCheckBox_CheckedChanged);
            // 
            // clearIdButton
            // 
            resources.ApplyResources(this.clearIdButton, "clearIdButton");
            this.dirtyErrorProvider.SetIconAlignment(this.clearIdButton, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("clearIdButton.IconAlignment"))));
            this.clearIdButton.Image = global::MusicBeePlugin.Properties.Resources.clear_button_15;
            this.clearIdButton.Name = "clearIdButton";
            this.clearIdButton.Tag = "@pinned-to-parent-x@non-defaultable@square-control";
            this.toolTip1.SetToolTip(this.clearIdButton, resources.GetString("clearIdButton.ToolTip"));
            this.clearIdButton.Click += new System.EventHandler(this.clearIdButton_Click);
            // 
            // idTextBox
            // 
            resources.ApplyResources(this.idTextBox, "idTextBox");
            this.dirtyErrorProvider.SetIconAlignment(this.idTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("idTextBox.IconAlignment"))));
            this.idTextBox.Name = "idTextBox";
            this.idTextBox.Tag = "#clearIdButton";
            this.toolTip1.SetToolTip(this.idTextBox, resources.GetString("idTextBox.ToolTip"));
            this.idTextBox.Leave += new System.EventHandler(this.idTextBox_Leave);
            // 
            // idLabel
            // 
            resources.ApplyResources(this.idLabel, "idLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.idLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("idLabel.IconAlignment"))));
            this.idLabel.Name = "idLabel";
            this.idLabel.Tag = "#idTextBox";
            // 
            // playlistComboBox
            // 
            resources.ApplyResources(this.playlistComboBox, "playlistComboBox");
            this.playlistComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playlistComboBox.DropDownWidth = 250;
            this.playlistComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.playlistComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("playlistComboBox.IconAlignment"))));
            this.playlistComboBox.Name = "playlistComboBox";
            this.playlistComboBox.Tag = "#idLabel";
            this.playlistComboBox.SelectedIndexChanged += new System.EventHandler(this.playlistComboBox_SelectedIndexChanged);
            // 
            // conditionCheckBoxLabel
            // 
            resources.ApplyResources(this.conditionCheckBoxLabel, "conditionCheckBoxLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.conditionCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("conditionCheckBoxLabel.IconAlignment"))));
            this.conditionCheckBoxLabel.Name = "conditionCheckBoxLabel";
            this.conditionCheckBoxLabel.Tag = "#playlistComboBox";
            this.conditionCheckBoxLabel.Click += new System.EventHandler(this.conditionCheckBoxLabel_Click);
            // 
            // conditionCheckBox
            // 
            resources.ApplyResources(this.conditionCheckBox, "conditionCheckBox");
            this.dirtyErrorProvider.SetIconAlignment(this.conditionCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("conditionCheckBox.IconAlignment"))));
            this.conditionCheckBox.Name = "conditionCheckBox";
            this.conditionCheckBox.Tag = "#conditionCheckBoxLabel";
            this.conditionCheckBox.CheckedChanged += new System.EventHandler(this.conditionCheckBox_CheckedChanged);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.descriptionBox);
            this.groupBox1.Controls.Add(this.groupBox1Label);
            this.dirtyErrorProvider.SetIconAlignment(this.groupBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox1.IconAlignment"))));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // descriptionBox
            // 
            resources.ApplyResources(this.descriptionBox, "descriptionBox");
            this.dirtyErrorProvider.SetIconAlignment(this.descriptionBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("descriptionBox.IconAlignment"))));
            this.descriptionBox.Name = "descriptionBox";
            this.descriptionBox.ReadOnly = true;
            // 
            // groupBox1Label
            // 
            resources.ApplyResources(this.groupBox1Label, "groupBox1Label");
            this.dirtyErrorProvider.SetIconAlignment(this.groupBox1Label, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox1Label.IconAlignment"))));
            this.groupBox1Label.Name = "groupBox1Label";
            // 
            // presetList
            // 
            resources.ApplyResources(this.presetList, "presetList");
            this.dirtyErrorProvider.SetIconAlignment(this.presetList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("presetList.IconAlignment"))));
            this.presetList.Name = "presetList";
            this.presetList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.presetList_ItemCheck);
            this.presetList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.presetList_MouseClick);
            this.presetList.SelectedIndexChanged += new System.EventHandler(this.presetList_SelectedIndexChanged);
            this.presetList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.presetList_MouseDoubleClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.previewTable);
            this.panel2.Controls.Add(this.settingsProcessingGroupBox);
            this.panel2.Controls.Add(this.presetManagementGroupBox);
            resources.ApplyResources(this.panel2, "panel2");
            this.dirtyErrorProvider.SetIconAlignment(this.panel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("panel2.IconAlignment"))));
            this.panel2.Name = "panel2";
            this.panel2.Tag = "";
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
            this.PresetGuid,
            this.File,
            this.Track,
            this.TagName1,
            this.OriginalTag1,
            this.NewTag1,
            this.TagName2,
            this.OriginalTag2,
            this.NewTag2,
            this.TagName3,
            this.OriginalTag3,
            this.NewTag3,
            this.TagName4,
            this.OriginalTag4,
            this.NewTag4,
            this.TagName5,
            this.OriginalTag5,
            this.NewTag5,
            this.OddEven});
            this.previewTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dirtyErrorProvider.SetIconAlignment(this.previewTable, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("previewTable.IconAlignment"))));
            this.previewTable.MultiSelect = false;
            this.previewTable.Name = "previewTable";
            this.previewTable.RowHeadersVisible = false;
            this.previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.previewTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.previewTable_CellContentClick);
            // 
            // PresetGuid
            // 
            resources.ApplyResources(this.PresetGuid, "PresetGuid");
            this.PresetGuid.Name = "PresetGuid";
            // 
            // File
            // 
            this.File.FillWeight = 7.191176F;
            resources.ApplyResources(this.File, "File");
            this.File.Name = "File";
            // 
            // Track
            // 
            this.Track.FillWeight = 168.2848F;
            resources.ApplyResources(this.Track, "Track");
            this.Track.Name = "Track";
            // 
            // TagName1
            // 
            this.TagName1.FillWeight = 24.52376F;
            resources.ApplyResources(this.TagName1, "TagName1");
            this.TagName1.Name = "TagName1";
            // 
            // OriginalTag1
            // 
            this.OriginalTag1.FillWeight = 24.52376F;
            resources.ApplyResources(this.OriginalTag1, "OriginalTag1");
            this.OriginalTag1.Name = "OriginalTag1";
            // 
            // NewTag1
            // 
            this.NewTag1.FillWeight = 24.52376F;
            resources.ApplyResources(this.NewTag1, "NewTag1");
            this.NewTag1.Name = "NewTag1";
            // 
            // TagName2
            // 
            this.TagName2.FillWeight = 24.52376F;
            resources.ApplyResources(this.TagName2, "TagName2");
            this.TagName2.Name = "TagName2";
            // 
            // OriginalTag2
            // 
            this.OriginalTag2.FillWeight = 24.52376F;
            resources.ApplyResources(this.OriginalTag2, "OriginalTag2");
            this.OriginalTag2.Name = "OriginalTag2";
            // 
            // NewTag2
            // 
            this.NewTag2.FillWeight = 24.52376F;
            resources.ApplyResources(this.NewTag2, "NewTag2");
            this.NewTag2.Name = "NewTag2";
            // 
            // TagName3
            // 
            this.TagName3.FillWeight = 24.52376F;
            resources.ApplyResources(this.TagName3, "TagName3");
            this.TagName3.Name = "TagName3";
            // 
            // OriginalTag3
            // 
            this.OriginalTag3.FillWeight = 24.52376F;
            resources.ApplyResources(this.OriginalTag3, "OriginalTag3");
            this.OriginalTag3.Name = "OriginalTag3";
            // 
            // NewTag3
            // 
            this.NewTag3.FillWeight = 24.52376F;
            resources.ApplyResources(this.NewTag3, "NewTag3");
            this.NewTag3.Name = "NewTag3";
            // 
            // TagName4
            // 
            this.TagName4.FillWeight = 24.52376F;
            resources.ApplyResources(this.TagName4, "TagName4");
            this.TagName4.Name = "TagName4";
            // 
            // OriginalTag4
            // 
            this.OriginalTag4.FillWeight = 24.52376F;
            resources.ApplyResources(this.OriginalTag4, "OriginalTag4");
            this.OriginalTag4.Name = "OriginalTag4";
            // 
            // NewTag4
            // 
            this.NewTag4.FillWeight = 24.52376F;
            resources.ApplyResources(this.NewTag4, "NewTag4");
            this.NewTag4.Name = "NewTag4";
            // 
            // TagName5
            // 
            this.TagName5.FillWeight = 24.52376F;
            resources.ApplyResources(this.TagName5, "TagName5");
            this.TagName5.Name = "TagName5";
            // 
            // OriginalTag5
            // 
            this.OriginalTag5.FillWeight = 24.52376F;
            resources.ApplyResources(this.OriginalTag5, "OriginalTag5");
            this.OriginalTag5.Name = "OriginalTag5";
            // 
            // NewTag5
            // 
            this.NewTag5.FillWeight = 24.52376F;
            resources.ApplyResources(this.NewTag5, "NewTag5");
            this.NewTag5.Name = "NewTag5";
            // 
            // OddEven
            // 
            this.OddEven.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.OddEven.FillWeight = 5.286435F;
            resources.ApplyResources(this.OddEven, "OddEven");
            this.OddEven.Name = "OddEven";
            // 
            // settingsProcessingGroupBox
            // 
            resources.ApplyResources(this.settingsProcessingGroupBox, "settingsProcessingGroupBox");
            this.settingsProcessingGroupBox.Controls.Add(this.buttonOK);
            this.settingsProcessingGroupBox.Controls.Add(this.buttonPreview);
            this.settingsProcessingGroupBox.Controls.Add(this.buttonClose);
            this.settingsProcessingGroupBox.Controls.Add(this.buttonSaveClose);
            this.settingsProcessingGroupBox.Controls.Add(this.settingsProcessingLabel);
            this.dirtyErrorProvider.SetIconAlignment(this.settingsProcessingGroupBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("settingsProcessingGroupBox.IconAlignment"))));
            this.settingsProcessingGroupBox.Name = "settingsProcessingGroupBox";
            this.settingsProcessingGroupBox.TabStop = false;
            this.settingsProcessingGroupBox.Tag = "";
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonOK, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonOK.IconAlignment"))));
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Tag = "";
            this.toolTip1.SetToolTip(this.buttonOK, resources.GetString("buttonOK.ToolTip"));
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonPreview
            // 
            resources.ApplyResources(this.buttonPreview, "buttonPreview");
            this.dirtyErrorProvider.SetError(this.buttonPreview, resources.GetString("buttonPreview.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonPreview, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonPreview.IconAlignment"))));
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Tag = "";
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // buttonClose
            // 
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dirtyErrorProvider.SetIconAlignment(this.buttonClose, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonClose.IconAlignment"))));
            this.buttonClose.Image = global::MusicBeePlugin.Properties.Resources.warning_wide_15;
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Tag = "@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonClose, resources.GetString("buttonClose.ToolTip"));
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonSaveClose
            // 
            resources.ApplyResources(this.buttonSaveClose, "buttonSaveClose");
            this.buttonSaveClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dirtyErrorProvider.SetIconAlignment(this.buttonSaveClose, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonSaveClose.IconAlignment"))));
            this.buttonSaveClose.Name = "buttonSaveClose";
            this.buttonSaveClose.Tag = "";
            this.toolTip1.SetToolTip(this.buttonSaveClose, resources.GetString("buttonSaveClose.ToolTip"));
            this.buttonSaveClose.Click += new System.EventHandler(this.buttonSaveClose_Click);
            // 
            // settingsProcessingLabel
            // 
            resources.ApplyResources(this.settingsProcessingLabel, "settingsProcessingLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.settingsProcessingLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("settingsProcessingLabel.IconAlignment"))));
            this.settingsProcessingLabel.Name = "settingsProcessingLabel";
            this.settingsProcessingLabel.Tag = "";
            // 
            // presetManagementGroupBox
            // 
            this.presetManagementGroupBox.Controls.Add(this.buttonDelete);
            this.presetManagementGroupBox.Controls.Add(this.buttonEdit);
            this.presetManagementGroupBox.Controls.Add(this.buttonCopy);
            this.presetManagementGroupBox.Controls.Add(this.buttonCreate);
            this.presetManagementGroupBox.Controls.Add(this.buttonExportCustom);
            this.presetManagementGroupBox.Controls.Add(this.buttonDeleteAll);
            this.presetManagementGroupBox.Controls.Add(this.buttonImportAll);
            this.presetManagementGroupBox.Controls.Add(this.buttonImportNew);
            this.presetManagementGroupBox.Controls.Add(this.buttonImport);
            this.presetManagementGroupBox.Controls.Add(this.buttonExport);
            this.presetManagementGroupBox.Controls.Add(this.presetManagementLabel);
            this.dirtyErrorProvider.SetIconAlignment(this.presetManagementGroupBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("presetManagementGroupBox.IconAlignment"))));
            resources.ApplyResources(this.presetManagementGroupBox, "presetManagementGroupBox");
            this.presetManagementGroupBox.Name = "presetManagementGroupBox";
            this.presetManagementGroupBox.TabStop = false;
            this.presetManagementGroupBox.Tag = "";
            // 
            // buttonDelete
            // 
            resources.ApplyResources(this.buttonDelete, "buttonDelete");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonDelete, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonDelete.IconAlignment"))));
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Tag = "";
            this.toolTip1.SetToolTip(this.buttonDelete, resources.GetString("buttonDelete.ToolTip"));
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonEdit
            // 
            resources.ApplyResources(this.buttonEdit, "buttonEdit");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonEdit, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonEdit.IconAlignment"))));
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Tag = "";
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonCopy
            // 
            resources.ApplyResources(this.buttonCopy, "buttonCopy");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonCopy, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonCopy.IconAlignment"))));
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Tag = "";
            this.toolTip1.SetToolTip(this.buttonCopy, resources.GetString("buttonCopy.ToolTip"));
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // buttonCreate
            // 
            resources.ApplyResources(this.buttonCreate, "buttonCreate");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonCreate, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonCreate.IconAlignment"))));
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Tag = "";
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // buttonExportCustom
            // 
            resources.ApplyResources(this.buttonExportCustom, "buttonExportCustom");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonExportCustom, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonExportCustom.IconAlignment"))));
            this.buttonExportCustom.Name = "buttonExportCustom";
            this.buttonExportCustom.Tag = "";
            this.toolTip1.SetToolTip(this.buttonExportCustom, resources.GetString("buttonExportCustom.ToolTip"));
            this.buttonExportCustom.Click += new System.EventHandler(this.buttonExportUser_Click);
            // 
            // buttonDeleteAll
            // 
            resources.ApplyResources(this.buttonDeleteAll, "buttonDeleteAll");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonDeleteAll, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonDeleteAll.IconAlignment"))));
            this.buttonDeleteAll.Name = "buttonDeleteAll";
            this.buttonDeleteAll.Tag = "";
            this.toolTip1.SetToolTip(this.buttonDeleteAll, resources.GetString("buttonDeleteAll.ToolTip"));
            this.buttonDeleteAll.Click += new System.EventHandler(this.buttonDeleteAll_Click);
            // 
            // buttonImportAll
            // 
            resources.ApplyResources(this.buttonImportAll, "buttonImportAll");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonImportAll, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonImportAll.IconAlignment"))));
            this.buttonImportAll.Name = "buttonImportAll";
            this.buttonImportAll.Tag = "";
            this.toolTip1.SetToolTip(this.buttonImportAll, resources.GetString("buttonImportAll.ToolTip"));
            this.buttonImportAll.Click += new System.EventHandler(this.buttonInstallAll_Click);
            // 
            // buttonImportNew
            // 
            resources.ApplyResources(this.buttonImportNew, "buttonImportNew");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonImportNew, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonImportNew.IconAlignment"))));
            this.buttonImportNew.Name = "buttonImportNew";
            this.buttonImportNew.Tag = "";
            this.toolTip1.SetToolTip(this.buttonImportNew, resources.GetString("buttonImportNew.ToolTip"));
            this.buttonImportNew.Click += new System.EventHandler(this.buttonInstallNew_Click);
            // 
            // buttonImport
            // 
            resources.ApplyResources(this.buttonImport, "buttonImport");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonImport, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonImport.IconAlignment"))));
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Tag = "";
            this.toolTip1.SetToolTip(this.buttonImport, resources.GetString("buttonImport.ToolTip"));
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // buttonExport
            // 
            resources.ApplyResources(this.buttonExport, "buttonExport");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonExport, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonExport.IconAlignment"))));
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Tag = "";
            this.toolTip1.SetToolTip(this.buttonExport, resources.GetString("buttonExport.ToolTip"));
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // presetManagementLabel
            // 
            resources.ApplyResources(this.presetManagementLabel, "presetManagementLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.presetManagementLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("presetManagementLabel.IconAlignment"))));
            this.presetManagementLabel.Name = "presetManagementLabel";
            this.presetManagementLabel.Tag = "";
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 900;
            this.toolTip1.ReshowDelay = 1000;
            // 
            // filterComboBox
            // 
            resources.ApplyResources(this.filterComboBox, "filterComboBox");
            this.filterComboBox.CausesValidation = false;
            this.filterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.filterComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("filterComboBox.IconAlignment"))));
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
            this.filterComboBox.Tag = "#filtersPanel";
            this.toolTip1.SetToolTip(this.filterComboBox, resources.GetString("filterComboBox.ToolTip"));
            this.filterComboBox.SelectedIndexChanged += new System.EventHandler(this.filterComboBox_SelectedIndexChanged);
            // 
            // searchTextBox
            // 
            resources.ApplyResources(this.searchTextBox, "searchTextBox");
            this.dirtyErrorProvider.SetIconAlignment(this.searchTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("searchTextBox.IconAlignment"))));
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Tag = "#clearSearchButton";
            this.toolTip1.SetToolTip(this.searchTextBox, resources.GetString("searchTextBox.ToolTip"));
            this.searchTextBox.TextChanged += new System.EventHandler(this.searchTextBox_TextChanged);
            // 
            // buttonSettings
            // 
            resources.ApplyResources(this.buttonSettings, "buttonSettings");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonSettings, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonSettings.IconAlignment"))));
            this.buttonSettings.Image = global::MusicBeePlugin.Properties.Resources.gear_15;
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Tag = "@non-defaultable@square-control";
            this.toolTip1.SetToolTip(this.buttonSettings, resources.GetString("buttonSettings.ToolTip"));
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // uncheckAllFiltersPictureBox
            // 
            resources.ApplyResources(this.uncheckAllFiltersPictureBox, "uncheckAllFiltersPictureBox");
            this.dirtyErrorProvider.SetIconAlignment(this.uncheckAllFiltersPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("uncheckAllFiltersPictureBox.IconAlignment"))));
            this.uncheckAllFiltersPictureBox.Name = "uncheckAllFiltersPictureBox";
            this.uncheckAllFiltersPictureBox.TabStop = false;
            this.uncheckAllFiltersPictureBox.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.uncheckAllFiltersPictureBox, resources.GetString("uncheckAllFiltersPictureBox.ToolTip"));
            this.uncheckAllFiltersPictureBox.Click += new System.EventHandler(this.uncheckAllFiltersPictureBox_Click);
            // 
            // hotkeyPictureBox
            // 
            resources.ApplyResources(this.hotkeyPictureBox, "hotkeyPictureBox");
            this.dirtyErrorProvider.SetIconAlignment(this.hotkeyPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("hotkeyPictureBox.IconAlignment"))));
            this.hotkeyPictureBox.Name = "hotkeyPictureBox";
            this.hotkeyPictureBox.TabStop = false;
            this.hotkeyPictureBox.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.hotkeyPictureBox, resources.GetString("hotkeyPictureBox.ToolTip"));
            this.hotkeyPictureBox.Click += new System.EventHandler(this.hotkeyPictureBox_Click);
            // 
            // functionIdPictureBox
            // 
            resources.ApplyResources(this.functionIdPictureBox, "functionIdPictureBox");
            this.dirtyErrorProvider.SetIconAlignment(this.functionIdPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("functionIdPictureBox.IconAlignment"))));
            this.functionIdPictureBox.Name = "functionIdPictureBox";
            this.functionIdPictureBox.TabStop = false;
            this.functionIdPictureBox.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.functionIdPictureBox, resources.GetString("functionIdPictureBox.ToolTip"));
            this.functionIdPictureBox.Click += new System.EventHandler(this.functionIdPictureBox_Click);
            // 
            // playlistPictureBox
            // 
            resources.ApplyResources(this.playlistPictureBox, "playlistPictureBox");
            this.dirtyErrorProvider.SetIconAlignment(this.playlistPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("playlistPictureBox.IconAlignment"))));
            this.playlistPictureBox.Name = "playlistPictureBox";
            this.playlistPictureBox.TabStop = false;
            this.playlistPictureBox.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.playlistPictureBox, resources.GetString("playlistPictureBox.ToolTip"));
            this.playlistPictureBox.Click += new System.EventHandler(this.playlistPictureBox_Click);
            // 
            // userPictureBox
            // 
            resources.ApplyResources(this.userPictureBox, "userPictureBox");
            this.dirtyErrorProvider.SetIconAlignment(this.userPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("userPictureBox.IconAlignment"))));
            this.userPictureBox.Name = "userPictureBox";
            this.userPictureBox.TabStop = false;
            this.userPictureBox.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.userPictureBox, resources.GetString("userPictureBox.ToolTip"));
            this.userPictureBox.Click += new System.EventHandler(this.userPictureBox_Click);
            // 
            // customizedPictureBox
            // 
            resources.ApplyResources(this.customizedPictureBox, "customizedPictureBox");
            this.dirtyErrorProvider.SetIconAlignment(this.customizedPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customizedPictureBox.IconAlignment"))));
            this.customizedPictureBox.Name = "customizedPictureBox";
            this.customizedPictureBox.TabStop = false;
            this.customizedPictureBox.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.customizedPictureBox, resources.GetString("customizedPictureBox.ToolTip"));
            this.customizedPictureBox.Click += new System.EventHandler(this.customizedPictureBox_Click);
            // 
            // predefinedPictureBox
            // 
            resources.ApplyResources(this.predefinedPictureBox, "predefinedPictureBox");
            this.dirtyErrorProvider.SetIconAlignment(this.predefinedPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("predefinedPictureBox.IconAlignment"))));
            this.predefinedPictureBox.Name = "predefinedPictureBox";
            this.predefinedPictureBox.TabStop = false;
            this.predefinedPictureBox.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.predefinedPictureBox, resources.GetString("predefinedPictureBox.ToolTip"));
            this.predefinedPictureBox.Click += new System.EventHandler(this.predefinedPictureBox_Click);
            // 
            // tickedOnlyPictureBox
            // 
            resources.ApplyResources(this.tickedOnlyPictureBox, "tickedOnlyPictureBox");
            this.dirtyErrorProvider.SetIconAlignment(this.tickedOnlyPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tickedOnlyPictureBox.IconAlignment"))));
            this.tickedOnlyPictureBox.Name = "tickedOnlyPictureBox";
            this.tickedOnlyPictureBox.TabStop = false;
            this.tickedOnlyPictureBox.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.tickedOnlyPictureBox, resources.GetString("tickedOnlyPictureBox.ToolTip"));
            this.tickedOnlyPictureBox.Click += new System.EventHandler(this.tickedOnlyPictureBox_Click);
            // 
            // clearSearchButton
            // 
            resources.ApplyResources(this.clearSearchButton, "clearSearchButton");
            this.dirtyErrorProvider.SetIconAlignment(this.clearSearchButton, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("clearSearchButton.IconAlignment"))));
            this.clearSearchButton.Image = global::MusicBeePlugin.Properties.Resources.clear_button_15;
            this.clearSearchButton.Name = "clearSearchButton";
            this.clearSearchButton.Tag = "@non-defaultable@square-control";
            this.toolTip1.SetToolTip(this.clearSearchButton, resources.GetString("clearSearchButton.ToolTip"));
            this.clearSearchButton.Click += new System.EventHandler(this.clearSearchButton_Click);
            // 
            // autoApplyPresetsLabel
            // 
            resources.ApplyResources(this.autoApplyPresetsLabel, "autoApplyPresetsLabel");
            this.autoApplyPresetsLabel.AutoEllipsis = true;
            this.dirtyErrorProvider.SetIconAlignment(this.autoApplyPresetsLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("autoApplyPresetsLabel.IconAlignment"))));
            this.autoApplyPresetsLabel.Name = "autoApplyPresetsLabel";
            this.autoApplyPresetsLabel.Tag = "#AdvancedSearchAndReplaceCommand&filtersPanel@pinned-to-parent-x";
            this.autoApplyPresetsLabel.Click += new System.EventHandler(this.label5_Click);
            // 
            // searchPictureBox
            // 
            this.dirtyErrorProvider.SetIconAlignment(this.searchPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("searchPictureBox.IconAlignment"))));
            this.searchPictureBox.Image = global::MusicBeePlugin.Properties.Resources.search;
            resources.ApplyResources(this.searchPictureBox, "searchPictureBox");
            this.searchPictureBox.Name = "searchPictureBox";
            this.searchPictureBox.TabStop = false;
            this.searchPictureBox.Tag = "#searchTextBox@square-control";
            // 
            // filtersPanel
            // 
            resources.ApplyResources(this.filtersPanel, "filtersPanel");
            this.filtersPanel.Controls.Add(this.filterComboBox);
            this.filtersPanel.Controls.Add(this.uncheckAllFiltersPictureBox);
            this.filtersPanel.Controls.Add(this.hotkeyPictureBox);
            this.filtersPanel.Controls.Add(this.functionIdPictureBox);
            this.filtersPanel.Controls.Add(this.playlistPictureBox);
            this.filtersPanel.Controls.Add(this.userPictureBox);
            this.filtersPanel.Controls.Add(this.customizedPictureBox);
            this.filtersPanel.Controls.Add(this.predefinedPictureBox);
            this.filtersPanel.Controls.Add(this.tickedOnlyPictureBox);
            this.filtersPanel.Controls.Add(this.buttonSettings);
            this.filtersPanel.Controls.Add(this.clearSearchButton);
            this.filtersPanel.Controls.Add(this.searchTextBox);
            this.filtersPanel.Controls.Add(this.searchPictureBox);
            this.dirtyErrorProvider.SetIconAlignment(this.filtersPanel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("filtersPanel.IconAlignment"))));
            this.filtersPanel.Name = "filtersPanel";
            this.filtersPanel.Tag = "#AdvancedSearchAndReplaceCommand&splitContainer1@pinned-to-parent-x";
            // 
            // dirtyErrorProvider
            // 
            this.dirtyErrorProvider.BlinkRate = 1000;
            this.dirtyErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
            this.dirtyErrorProvider.ContainerControl = this;
            resources.ApplyResources(this.dirtyErrorProvider, "dirtyErrorProvider");
            // 
            // AdvancedSearchAndReplaceCommand
            // 
            this.AcceptButton = this.buttonPreview;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.autoApplyPresetsLabel);
            this.Controls.Add(this.filtersPanel);
            this.Name = "AdvancedSearchAndReplaceCommand";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AdvancedSearchAndReplaceCommand_FormClosing);
            this.Load += new System.EventHandler(this.AdvancedSearchAndReplaceCommand_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTag6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTag5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTag4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTag3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTag2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userPresetPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customizedPresetPictureBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).EndInit();
            this.settingsProcessingGroupBox.ResumeLayout(false);
            this.settingsProcessingGroupBox.PerformLayout();
            this.presetManagementGroupBox.ResumeLayout(false);
            this.presetManagementGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uncheckAllFiltersPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hotkeyPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.functionIdPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playlistPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customizedPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.predefinedPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tickedOnlyPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchPictureBox)).EndInit();
            this.filtersPanel.ResumeLayout(false);
            this.filtersPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ErrorProvider dirtyErrorProvider;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox uncheckAllFiltersPictureBox;
        private System.Windows.Forms.PictureBox hotkeyPictureBox;
        private System.Windows.Forms.PictureBox functionIdPictureBox;
        private System.Windows.Forms.PictureBox playlistPictureBox;
        private System.Windows.Forms.PictureBox userPictureBox;
        private System.Windows.Forms.PictureBox customizedPictureBox;
        private System.Windows.Forms.PictureBox predefinedPictureBox;
        private System.Windows.Forms.PictureBox tickedOnlyPictureBox;
        private System.Windows.Forms.ComboBox filterComboBox;
        private System.Windows.Forms.Button clearSearchButton;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.PictureBox searchPictureBox;
        private System.Windows.Forms.Label autoApplyPresetsLabel;
        private System.Windows.Forms.PictureBox userPresetPictureBox;
        private System.Windows.Forms.PictureBox customizedPresetPictureBox;
        private System.Windows.Forms.Label customizedPresetLabel;
        private System.Windows.Forms.Label userPresetLabel;
        private System.Windows.Forms.CheckBox applyToPlayingTrackCheckBox;
        private System.Windows.Forms.Button clearIdButton;
        private System.Windows.Forms.TextBox idTextBox;
        private System.Windows.Forms.Label idLabel;
        private System.Windows.Forms.CheckBox assignHotkeyCheckBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox descriptionBox;
        private System.Windows.Forms.ComboBox playlistComboBox;
        private System.Windows.Forms.CheckBox conditionCheckBox;
        private System.Windows.Forms.CheckedListBox presetList;
        private System.Windows.Forms.GroupBox settingsProcessingGroupBox;
        private System.Windows.Forms.Label settingsProcessingLabel;
        private System.Windows.Forms.Button buttonSaveClose;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.GroupBox presetManagementGroupBox;
        private System.Windows.Forms.Label presetManagementLabel;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Button buttonDeleteAll;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.Button buttonImportNew;
        private System.Windows.Forms.Button buttonImportAll;
        private System.Windows.Forms.Button buttonExportCustom;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.DataGridView previewTable;
        private System.Windows.Forms.CheckBox favoriteCheckBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn PresetGuid;
        private System.Windows.Forms.DataGridViewTextBoxColumn File;
        private System.Windows.Forms.DataGridViewTextBoxColumn Track;
        private System.Windows.Forms.DataGridViewTextBoxColumn TagName1;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTag1;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTag1;
        private System.Windows.Forms.DataGridViewTextBoxColumn TagName2;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTag2;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTag2;
        private System.Windows.Forms.DataGridViewTextBoxColumn TagName3;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTag3;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTag3;
        private System.Windows.Forms.DataGridViewTextBoxColumn TagName4;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTag4;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTag4;
        private System.Windows.Forms.DataGridViewTextBoxColumn TagName5;
        private System.Windows.Forms.DataGridViewTextBoxColumn OriginalTag5;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewTag5;
        private System.Windows.Forms.DataGridViewTextBoxColumn OddEven;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.Label conditionCheckBoxLabel;
        private System.Windows.Forms.Label applyToPlayingTrackCheckBoxLabel;
        private System.Windows.Forms.Label assignHotkeyCheckBoxLabel;
        private System.Windows.Forms.Label favoriteCheckBoxLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label customTextLabel;
        private System.Windows.Forms.TextBox customTextBox;
        private System.Windows.Forms.Label customText2Label;
        private System.Windows.Forms.TextBox customText2Box;
        private System.Windows.Forms.Label customText3Label;
        private System.Windows.Forms.TextBox customText3Box;
        private System.Windows.Forms.Label customText4Label;
        private System.Windows.Forms.TextBox customText4Box;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureTag6;
        private System.Windows.Forms.PictureBox pictureTag5;
        private System.Windows.Forms.PictureBox pictureTag4;
        private System.Windows.Forms.PictureBox pictureTag3;
        private System.Windows.Forms.PictureBox pictureTag2;
        private System.Windows.Forms.PictureBox pictureTag;
        private System.Windows.Forms.ComboBox parameterTag6List;
        private System.Windows.Forms.ComboBox parameterTag5List;
        private System.Windows.Forms.ComboBox parameterTag4List;
        private System.Windows.Forms.ComboBox parameterTag3List;
        private System.Windows.Forms.ComboBox parameterTag2List;
        private System.Windows.Forms.ComboBox parameterTagList;
        private System.Windows.Forms.Label labelTag;
        private System.Windows.Forms.Label labelTag2;
        private System.Windows.Forms.Label labelTag3;
        private System.Windows.Forms.Label labelTag4;
        private System.Windows.Forms.Label labelTag5;
        private System.Windows.Forms.Label labelTag6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button buttonSelectPreservedTags;
        private System.Windows.Forms.Label labelPreserveTagValues;
        private System.Windows.Forms.Button buttonProcessPreserveTags;
        private System.Windows.Forms.TextBox processPreserveTagsTextBox;
        private System.Windows.Forms.TextBox preserveTagValuesTextBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label groupBox1Label;
        private System.Windows.Forms.Panel filtersPanel;
    }
}