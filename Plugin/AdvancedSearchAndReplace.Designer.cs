using ExtensionMethods;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    partial class AdvancedSearchAndReplace
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
            {
                if (allTagsWarningTimer != null)
                    allTagsWarningTimer.Dispose();

                warning?.Dispose();
                warningWide?.Dispose();
                checkedState?.Dispose();
                uncheckedState?.Dispose();

                tagNameFont?.Dispose();
            }
        }

        #region Код, автоматически созданный конструктором форм Windows

        ///<summary>
        ///Обязательный метод для поддержки конструктора - не изменяйте
        ///содержимое данного метода при помощи редактора кода.
        ///</summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedSearchAndReplace));
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
            this.descriptionGroupBox = new System.Windows.Forms.GroupBox();
            this.textBoxTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.descriptionBox = new System.Windows.Forms.TextBox();
            this.descriptionGroupBoxLabel = new System.Windows.Forms.Label();
            this.listBoxTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.presetList = new System.Windows.Forms.CheckedListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            previewTable = new System.Windows.Forms.DataGridView();
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
            this.descriptionGroupBox.SuspendLayout();
            this.textBoxTableLayoutPanel.SuspendLayout();
            this.listBoxTableLayoutPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(previewTable)).BeginInit();
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
            this.presetList = new CustomCheckedListBox(Plugin.SavedSettings.dontUseSkinColors);
            this.searchTextBox = ControlsTools.CreateMusicBeeTextBox();
            this.customTextBox = ControlsTools.CreateMusicBeeTextBox();
            this.customText2Box = ControlsTools.CreateMusicBeeTextBox();
            this.customText3Box = ControlsTools.CreateMusicBeeTextBox();
            this.customText4Box = ControlsTools.CreateMusicBeeTextBox();
            this.idTextBox = ControlsTools.CreateMusicBeeTextBox();
            this.preserveTagValuesTextBox = ControlsTools.CreateMusicBeeTextBox();
            this.processPreserveTagsTextBox = ControlsTools.CreateMusicBeeTextBox();
            this.descriptionBox = ControlsTools.CreateMusicBeeTextBox();


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
            this.dirtyErrorProvider.SetError(this.splitContainer1, resources.GetString("splitContainer1.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.splitContainer1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.splitContainer1, ((int)(resources.GetObject("splitContainer1.IconPadding"))));
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel4);
            this.dirtyErrorProvider.SetError(this.splitContainer1.Panel1, resources.GetString("splitContainer1.Panel1.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.splitContainer1.Panel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer1.Panel1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.splitContainer1.Panel1, ((int)(resources.GetObject("splitContainer1.Panel1.IconPadding"))));
            this.toolTip1.SetToolTip(this.splitContainer1.Panel1, resources.GetString("splitContainer1.Panel1.ToolTip"));
            // 
            // splitContainer1.Panel2
            // 
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.dirtyErrorProvider.SetError(this.splitContainer1.Panel2, resources.GetString("splitContainer1.Panel2.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.splitContainer1.Panel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer1.Panel2.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.splitContainer1.Panel2, ((int)(resources.GetObject("splitContainer1.Panel2.IconPadding"))));
            this.toolTip1.SetToolTip(this.splitContainer1.Panel2, resources.GetString("splitContainer1.Panel2.ToolTip"));
            this.splitContainer1.Tag = "#AdvancedSearchAndReplace&AdvancedSearchAndReplace";
            this.toolTip1.SetToolTip(this.splitContainer1, resources.GetString("splitContainer1.ToolTip"));
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.listBoxTableLayoutPanel, 0, 0);
            this.dirtyErrorProvider.SetError(this.tableLayoutPanel4, resources.GetString("tableLayoutPanel4.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.tableLayoutPanel4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tableLayoutPanel4.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.tableLayoutPanel4, ((int)(resources.GetObject("tableLayoutPanel4.IconPadding"))));
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.toolTip1.SetToolTip(this.tableLayoutPanel4, resources.GetString("tableLayoutPanel4.ToolTip"));
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
            this.panel1.Controls.Add(this.descriptionGroupBox);
            this.dirtyErrorProvider.SetError(this.panel1, resources.GetString("panel1.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.panel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("panel1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.panel1, ((int)(resources.GetObject("panel1.IconPadding"))));
            this.panel1.Name = "panel1";
            this.panel1.Tag = "";
            this.toolTip1.SetToolTip(this.panel1, resources.GetString("panel1.ToolTip"));
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.preserveTagValuesTextBox, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.labelPreserveTagValues, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonSelectPreservedTags, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.processPreserveTagsTextBox, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonProcessPreserveTags, 0, 0);
            this.dirtyErrorProvider.SetError(this.tableLayoutPanel3, resources.GetString("tableLayoutPanel3.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.tableLayoutPanel3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tableLayoutPanel3.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.tableLayoutPanel3, ((int)(resources.GetObject("tableLayoutPanel3.IconPadding"))));
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.toolTip1.SetToolTip(this.tableLayoutPanel3, resources.GetString("tableLayoutPanel3.ToolTip"));
            // 
            // preserveTagValuesTextBox
            // 
            resources.ApplyResources(this.preserveTagValuesTextBox, "preserveTagValuesTextBox");
            this.dirtyErrorProvider.SetError(this.preserveTagValuesTextBox, resources.GetString("preserveTagValuesTextBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.preserveTagValuesTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("preserveTagValuesTextBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.preserveTagValuesTextBox, ((int)(resources.GetObject("preserveTagValuesTextBox.IconPadding"))));
            this.preserveTagValuesTextBox.Name = "preserveTagValuesTextBox";
            this.toolTip1.SetToolTip(this.preserveTagValuesTextBox, resources.GetString("preserveTagValuesTextBox.ToolTip"));
            this.preserveTagValuesTextBox.TextChanged += new System.EventHandler(this.preserveTagValuesTextBox_TextChanged);
            // 
            // labelPreserveTagValues
            // 
            resources.ApplyResources(this.labelPreserveTagValues, "labelPreserveTagValues");
            this.dirtyErrorProvider.SetError(this.labelPreserveTagValues, resources.GetString("labelPreserveTagValues.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.labelPreserveTagValues, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelPreserveTagValues.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelPreserveTagValues, ((int)(resources.GetObject("labelPreserveTagValues.IconPadding"))));
            this.labelPreserveTagValues.Name = "labelPreserveTagValues";
            this.toolTip1.SetToolTip(this.labelPreserveTagValues, resources.GetString("labelPreserveTagValues.ToolTip"));
            // 
            // buttonSelectPreservedTags
            // 
            resources.ApplyResources(this.buttonSelectPreservedTags, "buttonSelectPreservedTags");
            this.dirtyErrorProvider.SetError(this.buttonSelectPreservedTags, resources.GetString("buttonSelectPreservedTags.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonSelectPreservedTags, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonSelectPreservedTags.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonSelectPreservedTags, ((int)(resources.GetObject("buttonSelectPreservedTags.IconPadding"))));
            this.buttonSelectPreservedTags.Name = "buttonSelectPreservedTags";
            this.buttonSelectPreservedTags.Tag = "@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonSelectPreservedTags, resources.GetString("buttonSelectPreservedTags.ToolTip"));
            this.buttonSelectPreservedTags.Click += new System.EventHandler(this.buttonSelectPreservedTags_Click);
            // 
            // processPreserveTagsTextBox
            // 
            resources.ApplyResources(this.processPreserveTagsTextBox, "processPreserveTagsTextBox");
            this.dirtyErrorProvider.SetError(this.processPreserveTagsTextBox, resources.GetString("processPreserveTagsTextBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.processPreserveTagsTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("processPreserveTagsTextBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.processPreserveTagsTextBox, ((int)(resources.GetObject("processPreserveTagsTextBox.IconPadding"))));
            this.processPreserveTagsTextBox.Name = "processPreserveTagsTextBox";
            this.toolTip1.SetToolTip(this.processPreserveTagsTextBox, resources.GetString("processPreserveTagsTextBox.ToolTip"));
            this.processPreserveTagsTextBox.TextChanged += new System.EventHandler(this.processPreserveTagsTextBox_TextChanged);
            // 
            // buttonProcessPreserveTags
            // 
            resources.ApplyResources(this.buttonProcessPreserveTags, "buttonProcessPreserveTags");
            this.dirtyErrorProvider.SetError(this.buttonProcessPreserveTags, resources.GetString("buttonProcessPreserveTags.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonProcessPreserveTags, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonProcessPreserveTags.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonProcessPreserveTags, ((int)(resources.GetObject("buttonProcessPreserveTags.IconPadding"))));
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
            this.dirtyErrorProvider.SetError(this.tableLayoutPanel2, resources.GetString("tableLayoutPanel2.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.tableLayoutPanel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tableLayoutPanel2.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.tableLayoutPanel2, ((int)(resources.GetObject("tableLayoutPanel2.IconPadding"))));
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.toolTip1.SetToolTip(this.tableLayoutPanel2, resources.GetString("tableLayoutPanel2.ToolTip"));
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
            // customText4Label
            // 
            resources.ApplyResources(this.customText4Label, "customText4Label");
            this.dirtyErrorProvider.SetError(this.customText4Label, resources.GetString("customText4Label.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.customText4Label, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customText4Label.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.customText4Label, ((int)(resources.GetObject("customText4Label.IconPadding"))));
            this.customText4Label.Name = "customText4Label";
            this.toolTip1.SetToolTip(this.customText4Label, resources.GetString("customText4Label.ToolTip"));
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
            // customText3Label
            // 
            resources.ApplyResources(this.customText3Label, "customText3Label");
            this.dirtyErrorProvider.SetError(this.customText3Label, resources.GetString("customText3Label.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.customText3Label, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customText3Label.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.customText3Label, ((int)(resources.GetObject("customText3Label.IconPadding"))));
            this.customText3Label.Name = "customText3Label";
            this.toolTip1.SetToolTip(this.customText3Label, resources.GetString("customText3Label.ToolTip"));
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
            // customText2Label
            // 
            resources.ApplyResources(this.customText2Label, "customText2Label");
            this.dirtyErrorProvider.SetError(this.customText2Label, resources.GetString("customText2Label.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.customText2Label, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customText2Label.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.customText2Label, ((int)(resources.GetObject("customText2Label.IconPadding"))));
            this.customText2Label.Name = "customText2Label";
            this.toolTip1.SetToolTip(this.customText2Label, resources.GetString("customText2Label.ToolTip"));
            // 
            // customTextBox
            // 
            resources.ApplyResources(this.customTextBox, "customTextBox");
            this.dirtyErrorProvider.SetError(this.customTextBox, resources.GetString("customTextBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.customTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customTextBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.customTextBox, ((int)(resources.GetObject("customTextBox.IconPadding"))));
            this.customTextBox.Name = "customTextBox";
            this.toolTip1.SetToolTip(this.customTextBox, resources.GetString("customTextBox.ToolTip"));
            this.customTextBox.TextChanged += new System.EventHandler(this.customTextBox_TextChanged);
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
            this.dirtyErrorProvider.SetError(this.tableLayoutPanel1, resources.GetString("tableLayoutPanel1.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.tableLayoutPanel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tableLayoutPanel1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.tableLayoutPanel1, ((int)(resources.GetObject("tableLayoutPanel1.IconPadding"))));
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.toolTip1.SetToolTip(this.tableLayoutPanel1, resources.GetString("tableLayoutPanel1.ToolTip"));
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
            this.parameterTag6List.SelectedIndexChanged += new System.EventHandler(this.parameterTag6_SelectedIndexChanged);
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
            // pictureTag6
            // 
            resources.ApplyResources(this.pictureTag6, "pictureTag6");
            this.dirtyErrorProvider.SetError(this.pictureTag6, resources.GetString("pictureTag6.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.pictureTag6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("pictureTag6.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.pictureTag6, ((int)(resources.GetObject("pictureTag6.IconPadding"))));
            this.pictureTag6.Image = global::MusicBeePlugin.Properties.Resources.warning_15;
            this.pictureTag6.Name = "pictureTag6";
            this.pictureTag6.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureTag6, resources.GetString("pictureTag6.ToolTip"));
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
            this.parameterTag5List.SelectedIndexChanged += new System.EventHandler(this.parameterTag5_SelectedIndexChanged);
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
            // pictureTag5
            // 
            resources.ApplyResources(this.pictureTag5, "pictureTag5");
            this.dirtyErrorProvider.SetError(this.pictureTag5, resources.GetString("pictureTag5.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.pictureTag5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("pictureTag5.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.pictureTag5, ((int)(resources.GetObject("pictureTag5.IconPadding"))));
            this.pictureTag5.Image = global::MusicBeePlugin.Properties.Resources.warning_15;
            this.pictureTag5.Name = "pictureTag5";
            this.pictureTag5.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureTag5, resources.GetString("pictureTag5.ToolTip"));
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
            this.parameterTag4List.SelectedIndexChanged += new System.EventHandler(this.parameterTag4_SelectedIndexChanged);
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
            // pictureTag4
            // 
            resources.ApplyResources(this.pictureTag4, "pictureTag4");
            this.dirtyErrorProvider.SetError(this.pictureTag4, resources.GetString("pictureTag4.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.pictureTag4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("pictureTag4.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.pictureTag4, ((int)(resources.GetObject("pictureTag4.IconPadding"))));
            this.pictureTag4.Image = global::MusicBeePlugin.Properties.Resources.warning_15;
            this.pictureTag4.Name = "pictureTag4";
            this.pictureTag4.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureTag4, resources.GetString("pictureTag4.ToolTip"));
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
            // labelTag3
            // 
            resources.ApplyResources(this.labelTag3, "labelTag3");
            this.dirtyErrorProvider.SetError(this.labelTag3, resources.GetString("labelTag3.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.labelTag3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelTag3.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelTag3, ((int)(resources.GetObject("labelTag3.IconPadding"))));
            this.labelTag3.Name = "labelTag3";
            this.toolTip1.SetToolTip(this.labelTag3, resources.GetString("labelTag3.ToolTip"));
            // 
            // pictureTag3
            // 
            resources.ApplyResources(this.pictureTag3, "pictureTag3");
            this.dirtyErrorProvider.SetError(this.pictureTag3, resources.GetString("pictureTag3.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.pictureTag3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("pictureTag3.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.pictureTag3, ((int)(resources.GetObject("pictureTag3.IconPadding"))));
            this.pictureTag3.Image = global::MusicBeePlugin.Properties.Resources.warning_15;
            this.pictureTag3.Name = "pictureTag3";
            this.pictureTag3.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureTag3, resources.GetString("pictureTag3.ToolTip"));
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
            // labelTag2
            // 
            resources.ApplyResources(this.labelTag2, "labelTag2");
            this.dirtyErrorProvider.SetError(this.labelTag2, resources.GetString("labelTag2.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.labelTag2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelTag2.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelTag2, ((int)(resources.GetObject("labelTag2.IconPadding"))));
            this.labelTag2.Name = "labelTag2";
            this.toolTip1.SetToolTip(this.labelTag2, resources.GetString("labelTag2.ToolTip"));
            // 
            // pictureTag2
            // 
            resources.ApplyResources(this.pictureTag2, "pictureTag2");
            this.dirtyErrorProvider.SetError(this.pictureTag2, resources.GetString("pictureTag2.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.pictureTag2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("pictureTag2.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.pictureTag2, ((int)(resources.GetObject("pictureTag2.IconPadding"))));
            this.pictureTag2.Image = global::MusicBeePlugin.Properties.Resources.warning_15;
            this.pictureTag2.Name = "pictureTag2";
            this.pictureTag2.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureTag2, resources.GetString("pictureTag2.ToolTip"));
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
            // labelTag
            // 
            resources.ApplyResources(this.labelTag, "labelTag");
            this.dirtyErrorProvider.SetError(this.labelTag, resources.GetString("labelTag.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.labelTag, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelTag.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelTag, ((int)(resources.GetObject("labelTag.IconPadding"))));
            this.labelTag.Name = "labelTag";
            this.toolTip1.SetToolTip(this.labelTag, resources.GetString("labelTag.ToolTip"));
            // 
            // pictureTag
            // 
            resources.ApplyResources(this.pictureTag, "pictureTag");
            this.dirtyErrorProvider.SetError(this.pictureTag, resources.GetString("pictureTag.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.pictureTag, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("pictureTag.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.pictureTag, ((int)(resources.GetObject("pictureTag.IconPadding"))));
            this.pictureTag.Image = global::MusicBeePlugin.Properties.Resources.warning_15;
            this.pictureTag.Name = "pictureTag";
            this.pictureTag.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureTag, resources.GetString("pictureTag.ToolTip"));
            // 
            // userPresetLabel
            // 
            resources.ApplyResources(this.userPresetLabel, "userPresetLabel");
            this.dirtyErrorProvider.SetError(this.userPresetLabel, resources.GetString("userPresetLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.userPresetLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("userPresetLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.userPresetLabel, ((int)(resources.GetObject("userPresetLabel.IconPadding"))));
            this.userPresetLabel.Name = "userPresetLabel";
            this.userPresetLabel.Tag = "#splitContainer1@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.userPresetLabel, resources.GetString("userPresetLabel.ToolTip"));
            // 
            // userPresetPictureBox
            // 
            resources.ApplyResources(this.userPresetPictureBox, "userPresetPictureBox");
            this.dirtyErrorProvider.SetError(this.userPresetPictureBox, resources.GetString("userPresetPictureBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.userPresetPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("userPresetPictureBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.userPresetPictureBox, ((int)(resources.GetObject("userPresetPictureBox.IconPadding"))));
            this.userPresetPictureBox.Image = global::MusicBeePlugin.Properties.Resources.user_presets;
            this.userPresetPictureBox.Name = "userPresetPictureBox";
            this.userPresetPictureBox.TabStop = false;
            this.userPresetPictureBox.Tag = "#userPresetLabel@square-control@small-picture";
            this.toolTip1.SetToolTip(this.userPresetPictureBox, resources.GetString("userPresetPictureBox.ToolTip"));
            // 
            // customizedPresetLabel
            // 
            resources.ApplyResources(this.customizedPresetLabel, "customizedPresetLabel");
            this.dirtyErrorProvider.SetError(this.customizedPresetLabel, resources.GetString("customizedPresetLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.customizedPresetLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customizedPresetLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.customizedPresetLabel, ((int)(resources.GetObject("customizedPresetLabel.IconPadding"))));
            this.customizedPresetLabel.Name = "customizedPresetLabel";
            this.customizedPresetLabel.Tag = "#userPresetPictureBox";
            this.toolTip1.SetToolTip(this.customizedPresetLabel, resources.GetString("customizedPresetLabel.ToolTip"));
            // 
            // customizedPresetPictureBox
            // 
            resources.ApplyResources(this.customizedPresetPictureBox, "customizedPresetPictureBox");
            this.dirtyErrorProvider.SetError(this.customizedPresetPictureBox, resources.GetString("customizedPresetPictureBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.customizedPresetPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customizedPresetPictureBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.customizedPresetPictureBox, ((int)(resources.GetObject("customizedPresetPictureBox.IconPadding"))));
            this.customizedPresetPictureBox.Image = global::MusicBeePlugin.Properties.Resources.customized_presets;
            this.customizedPresetPictureBox.Name = "customizedPresetPictureBox";
            this.customizedPresetPictureBox.TabStop = false;
            this.customizedPresetPictureBox.Tag = "#customizedPresetLabel@square-control@small-picture";
            this.toolTip1.SetToolTip(this.customizedPresetPictureBox, resources.GetString("customizedPresetPictureBox.ToolTip"));
            // 
            // favoriteCheckBoxLabel
            // 
            resources.ApplyResources(this.favoriteCheckBoxLabel, "favoriteCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.favoriteCheckBoxLabel, resources.GetString("favoriteCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.favoriteCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("favoriteCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.favoriteCheckBoxLabel, ((int)(resources.GetObject("favoriteCheckBoxLabel.IconPadding"))));
            this.favoriteCheckBoxLabel.Name = "favoriteCheckBoxLabel";
            this.favoriteCheckBoxLabel.Tag = "";
            this.toolTip1.SetToolTip(this.favoriteCheckBoxLabel, resources.GetString("favoriteCheckBoxLabel.ToolTip"));
            this.favoriteCheckBoxLabel.Click += new System.EventHandler(this.favoriteCheckBoxLabel_Click);
            // 
            // favoriteCheckBox
            // 
            resources.ApplyResources(this.favoriteCheckBox, "favoriteCheckBox");
            this.dirtyErrorProvider.SetError(this.favoriteCheckBox, resources.GetString("favoriteCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.favoriteCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("favoriteCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.favoriteCheckBox, ((int)(resources.GetObject("favoriteCheckBox.IconPadding"))));
            this.favoriteCheckBox.Name = "favoriteCheckBox";
            this.favoriteCheckBox.Tag = "#favoriteCheckBoxLabel";
            this.toolTip1.SetToolTip(this.favoriteCheckBox, resources.GetString("favoriteCheckBox.ToolTip"));
            this.favoriteCheckBox.CheckedChanged += new System.EventHandler(this.favoriteCheckBox_CheckedChanged);
            // 
            // applyToPlayingTrackCheckBoxLabel
            // 
            resources.ApplyResources(this.applyToPlayingTrackCheckBoxLabel, "applyToPlayingTrackCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.applyToPlayingTrackCheckBoxLabel, resources.GetString("applyToPlayingTrackCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.applyToPlayingTrackCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("applyToPlayingTrackCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.applyToPlayingTrackCheckBoxLabel, ((int)(resources.GetObject("applyToPlayingTrackCheckBoxLabel.IconPadding"))));
            this.applyToPlayingTrackCheckBoxLabel.Name = "applyToPlayingTrackCheckBoxLabel";
            this.applyToPlayingTrackCheckBoxLabel.Tag = "#favoriteCheckBox";
            this.toolTip1.SetToolTip(this.applyToPlayingTrackCheckBoxLabel, resources.GetString("applyToPlayingTrackCheckBoxLabel.ToolTip"));
            this.applyToPlayingTrackCheckBoxLabel.Click += new System.EventHandler(this.applyToPlayingTrackCheckBoxLabel_Click);
            // 
            // applyToPlayingTrackCheckBox
            // 
            resources.ApplyResources(this.applyToPlayingTrackCheckBox, "applyToPlayingTrackCheckBox");
            this.dirtyErrorProvider.SetError(this.applyToPlayingTrackCheckBox, resources.GetString("applyToPlayingTrackCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.applyToPlayingTrackCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("applyToPlayingTrackCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.applyToPlayingTrackCheckBox, ((int)(resources.GetObject("applyToPlayingTrackCheckBox.IconPadding"))));
            this.applyToPlayingTrackCheckBox.Name = "applyToPlayingTrackCheckBox";
            this.applyToPlayingTrackCheckBox.Tag = "#applyToPlayingTrackCheckBoxLabel";
            this.toolTip1.SetToolTip(this.applyToPlayingTrackCheckBox, resources.GetString("applyToPlayingTrackCheckBox.ToolTip"));
            this.applyToPlayingTrackCheckBox.CheckedChanged += new System.EventHandler(this.applyToPlayingTrackCheckBox_CheckedChanged);
            // 
            // assignHotkeyCheckBoxLabel
            // 
            resources.ApplyResources(this.assignHotkeyCheckBoxLabel, "assignHotkeyCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.assignHotkeyCheckBoxLabel, resources.GetString("assignHotkeyCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.assignHotkeyCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("assignHotkeyCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.assignHotkeyCheckBoxLabel, ((int)(resources.GetObject("assignHotkeyCheckBoxLabel.IconPadding"))));
            this.assignHotkeyCheckBoxLabel.Name = "assignHotkeyCheckBoxLabel";
            this.assignHotkeyCheckBoxLabel.Tag = "#applyToPlayingTrackCheckBox";
            this.toolTip1.SetToolTip(this.assignHotkeyCheckBoxLabel, resources.GetString("assignHotkeyCheckBoxLabel.ToolTip"));
            this.assignHotkeyCheckBoxLabel.Click += new System.EventHandler(this.assignHotkeyCheckBoxLabel_Click);
            // 
            // assignHotkeyCheckBox
            // 
            resources.ApplyResources(this.assignHotkeyCheckBox, "assignHotkeyCheckBox");
            this.dirtyErrorProvider.SetError(this.assignHotkeyCheckBox, resources.GetString("assignHotkeyCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.assignHotkeyCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("assignHotkeyCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.assignHotkeyCheckBox, ((int)(resources.GetObject("assignHotkeyCheckBox.IconPadding"))));
            this.assignHotkeyCheckBox.Name = "assignHotkeyCheckBox";
            this.assignHotkeyCheckBox.Tag = "#assignHotkeyCheckBoxLabel";
            this.toolTip1.SetToolTip(this.assignHotkeyCheckBox, resources.GetString("assignHotkeyCheckBox.ToolTip"));
            this.assignHotkeyCheckBox.CheckedChanged += new System.EventHandler(this.assignHotkeyCheckBox_CheckedChanged);
            // 
            // clearIdButton
            // 
            resources.ApplyResources(this.clearIdButton, "clearIdButton");
            this.dirtyErrorProvider.SetError(this.clearIdButton, resources.GetString("clearIdButton.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.clearIdButton, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("clearIdButton.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.clearIdButton, ((int)(resources.GetObject("clearIdButton.IconPadding"))));
            this.clearIdButton.Name = "clearIdButton";
            this.clearIdButton.Tag = "@pinned-to-parent-x@non-defaultable@square-control";
            this.toolTip1.SetToolTip(this.clearIdButton, resources.GetString("clearIdButton.ToolTip"));
            this.clearIdButton.Click += new System.EventHandler(this.clearIdButton_Click);
            // 
            // idTextBox
            // 
            resources.ApplyResources(this.idTextBox, "idTextBox");
            this.dirtyErrorProvider.SetError(this.idTextBox, resources.GetString("idTextBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.idTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("idTextBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.idTextBox, ((int)(resources.GetObject("idTextBox.IconPadding"))));
            this.idTextBox.Name = "idTextBox";
            this.idTextBox.Tag = "#clearIdButton";
            this.toolTip1.SetToolTip(this.idTextBox, resources.GetString("idTextBox.ToolTip"));
            this.idTextBox.Leave += new System.EventHandler(this.idTextBox_Leave);
            // 
            // idLabel
            // 
            resources.ApplyResources(this.idLabel, "idLabel");
            this.dirtyErrorProvider.SetError(this.idLabel, resources.GetString("idLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.idLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("idLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.idLabel, ((int)(resources.GetObject("idLabel.IconPadding"))));
            this.idLabel.Name = "idLabel";
            this.idLabel.Tag = "#idTextBox";
            this.toolTip1.SetToolTip(this.idLabel, resources.GetString("idLabel.ToolTip"));
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
            this.playlistComboBox.Tag = "#idLabel";
            this.toolTip1.SetToolTip(this.playlistComboBox, resources.GetString("playlistComboBox.ToolTip"));
            this.playlistComboBox.SelectedIndexChanged += new System.EventHandler(this.playlistComboBox_SelectedIndexChanged);
            // 
            // conditionCheckBoxLabel
            // 
            resources.ApplyResources(this.conditionCheckBoxLabel, "conditionCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.conditionCheckBoxLabel, resources.GetString("conditionCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.conditionCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("conditionCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.conditionCheckBoxLabel, ((int)(resources.GetObject("conditionCheckBoxLabel.IconPadding"))));
            this.conditionCheckBoxLabel.Name = "conditionCheckBoxLabel";
            this.conditionCheckBoxLabel.Tag = "#playlistComboBox";
            this.toolTip1.SetToolTip(this.conditionCheckBoxLabel, resources.GetString("conditionCheckBoxLabel.ToolTip"));
            this.conditionCheckBoxLabel.Click += new System.EventHandler(this.conditionCheckBoxLabel_Click);
            // 
            // conditionCheckBox
            // 
            resources.ApplyResources(this.conditionCheckBox, "conditionCheckBox");
            this.dirtyErrorProvider.SetError(this.conditionCheckBox, resources.GetString("conditionCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.conditionCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("conditionCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.conditionCheckBox, ((int)(resources.GetObject("conditionCheckBox.IconPadding"))));
            this.conditionCheckBox.Name = "conditionCheckBox";
            this.conditionCheckBox.Tag = "#conditionCheckBoxLabel";
            this.toolTip1.SetToolTip(this.conditionCheckBox, resources.GetString("conditionCheckBox.ToolTip"));
            this.conditionCheckBox.CheckedChanged += new System.EventHandler(this.conditionCheckBox_CheckedChanged);
            // 
            // descriptionGroupBox
            // 
            resources.ApplyResources(this.descriptionGroupBox, "descriptionGroupBox");
            this.descriptionGroupBox.Controls.Add(this.textBoxTableLayoutPanel);
            this.descriptionGroupBox.Controls.Add(this.descriptionGroupBoxLabel);
            this.dirtyErrorProvider.SetError(this.descriptionGroupBox, resources.GetString("descriptionGroupBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.descriptionGroupBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("descriptionGroupBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.descriptionGroupBox, ((int)(resources.GetObject("descriptionGroupBox.IconPadding"))));
            this.descriptionGroupBox.Name = "descriptionGroupBox";
            this.descriptionGroupBox.TabStop = false;
            this.toolTip1.SetToolTip(this.descriptionGroupBox, resources.GetString("descriptionGroupBox.ToolTip"));
            // 
            // textBoxTableLayoutPanel
            // 
            resources.ApplyResources(this.textBoxTableLayoutPanel, "textBoxTableLayoutPanel");
            this.textBoxTableLayoutPanel.Controls.Add(this.descriptionBox, 0, 0);
            this.dirtyErrorProvider.SetError(this.textBoxTableLayoutPanel, resources.GetString("textBoxTableLayoutPanel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.textBoxTableLayoutPanel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("textBoxTableLayoutPanel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.textBoxTableLayoutPanel, ((int)(resources.GetObject("textBoxTableLayoutPanel.IconPadding"))));
            this.textBoxTableLayoutPanel.Name = "textBoxTableLayoutPanel";
            this.textBoxTableLayoutPanel.Tag = "";
            this.toolTip1.SetToolTip(this.textBoxTableLayoutPanel, resources.GetString("textBoxTableLayoutPanel.ToolTip"));
            // 
            // descriptionBox
            // 
            resources.ApplyResources(this.descriptionBox, "descriptionBox");
            this.dirtyErrorProvider.SetError(this.descriptionBox, resources.GetString("descriptionBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.descriptionBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("descriptionBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.descriptionBox, ((int)(resources.GetObject("descriptionBox.IconPadding"))));
            this.descriptionBox.Name = "descriptionBox";
            this.descriptionBox.ReadOnly = true;
            this.toolTip1.SetToolTip(this.descriptionBox, resources.GetString("descriptionBox.ToolTip"));
            // 
            // descriptionGroupBoxLabel
            // 
            resources.ApplyResources(this.descriptionGroupBoxLabel, "descriptionGroupBoxLabel");
            this.dirtyErrorProvider.SetError(this.descriptionGroupBoxLabel, resources.GetString("descriptionGroupBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.descriptionGroupBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("descriptionGroupBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.descriptionGroupBoxLabel, ((int)(resources.GetObject("descriptionGroupBoxLabel.IconPadding"))));
            this.descriptionGroupBoxLabel.Name = "descriptionGroupBoxLabel";
            this.toolTip1.SetToolTip(this.descriptionGroupBoxLabel, resources.GetString("descriptionGroupBoxLabel.ToolTip"));
            // 
            // listBoxTableLayoutPanel
            // 
            resources.ApplyResources(this.listBoxTableLayoutPanel, "listBoxTableLayoutPanel");
            this.listBoxTableLayoutPanel.Controls.Add(this.presetList, 0, 0);
            this.dirtyErrorProvider.SetError(this.listBoxTableLayoutPanel, resources.GetString("listBoxTableLayoutPanel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.listBoxTableLayoutPanel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("listBoxTableLayoutPanel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.listBoxTableLayoutPanel, ((int)(resources.GetObject("listBoxTableLayoutPanel.IconPadding"))));
            this.listBoxTableLayoutPanel.Name = "listBoxTableLayoutPanel";
            this.toolTip1.SetToolTip(this.listBoxTableLayoutPanel, resources.GetString("listBoxTableLayoutPanel.ToolTip"));
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
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(previewTable);
            this.panel2.Controls.Add(this.settingsProcessingGroupBox);
            this.panel2.Controls.Add(this.presetManagementGroupBox);
            this.dirtyErrorProvider.SetError(this.panel2, resources.GetString("panel2.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.panel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("panel2.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.panel2, ((int)(resources.GetObject("panel2.IconPadding"))));
            this.panel2.Name = "panel2";
            this.panel2.Tag = "";
            this.toolTip1.SetToolTip(this.panel2, resources.GetString("panel2.ToolTip"));
            // 
            // previewTable
            // 
            resources.ApplyResources(previewTable, "previewTable");
            previewTable.AllowUserToAddRows = false;
            previewTable.AllowUserToDeleteRows = false;
            previewTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            previewTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            previewTable.BackgroundColor = System.Drawing.SystemColors.Window;
            previewTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            previewTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
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
            previewTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dirtyErrorProvider.SetError(previewTable, resources.GetString("previewTable.Error"));
            this.dirtyErrorProvider.SetIconAlignment(previewTable, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("previewTable.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(previewTable, ((int)(resources.GetObject("previewTable.IconPadding"))));
            previewTable.MultiSelect = false;
            previewTable.Name = "previewTable";
            previewTable.RowHeadersVisible = false;
            previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.toolTip1.SetToolTip(previewTable, resources.GetString("previewTable.ToolTip"));
            previewTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.previewTable_CellContentClick);
            previewTable.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(previewTable_ColumnWidthChanged);
            previewTable.MouseLeave += new System.EventHandler(PreviewTable_MouseLeave);
            previewTable.MouseDown += new System.Windows.Forms.MouseEventHandler(PreviewTable_MouseDown);
            previewTable.MouseUp += new System.Windows.Forms.MouseEventHandler(PreviewTable_MouseUp);
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
            this.Track.FillWeight = 60F;
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
            this.dirtyErrorProvider.SetError(this.settingsProcessingGroupBox, resources.GetString("settingsProcessingGroupBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.settingsProcessingGroupBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("settingsProcessingGroupBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.settingsProcessingGroupBox, ((int)(resources.GetObject("settingsProcessingGroupBox.IconPadding"))));
            this.settingsProcessingGroupBox.Name = "settingsProcessingGroupBox";
            this.settingsProcessingGroupBox.TabStop = false;
            this.settingsProcessingGroupBox.Tag = "";
            this.toolTip1.SetToolTip(this.settingsProcessingGroupBox, resources.GetString("settingsProcessingGroupBox.ToolTip"));
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.dirtyErrorProvider.SetError(this.buttonOK, resources.GetString("buttonOK.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonOK, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonOK.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonOK, ((int)(resources.GetObject("buttonOK.IconPadding"))));
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
            this.dirtyErrorProvider.SetIconPadding(this.buttonPreview, ((int)(resources.GetObject("buttonPreview.IconPadding"))));
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Tag = "";
            this.toolTip1.SetToolTip(this.buttonPreview, resources.GetString("buttonPreview.ToolTip"));
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // buttonClose
            // 
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dirtyErrorProvider.SetError(this.buttonClose, resources.GetString("buttonClose.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonClose, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonClose.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonClose, ((int)(resources.GetObject("buttonClose.IconPadding"))));
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
            this.dirtyErrorProvider.SetError(this.buttonSaveClose, resources.GetString("buttonSaveClose.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonSaveClose, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonSaveClose.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonSaveClose, ((int)(resources.GetObject("buttonSaveClose.IconPadding"))));
            this.buttonSaveClose.Name = "buttonSaveClose";
            this.buttonSaveClose.Tag = "";
            this.toolTip1.SetToolTip(this.buttonSaveClose, resources.GetString("buttonSaveClose.ToolTip"));
            this.buttonSaveClose.Click += new System.EventHandler(this.buttonSaveClose_Click);
            // 
            // settingsProcessingLabel
            // 
            resources.ApplyResources(this.settingsProcessingLabel, "settingsProcessingLabel");
            this.dirtyErrorProvider.SetError(this.settingsProcessingLabel, resources.GetString("settingsProcessingLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.settingsProcessingLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("settingsProcessingLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.settingsProcessingLabel, ((int)(resources.GetObject("settingsProcessingLabel.IconPadding"))));
            this.settingsProcessingLabel.Name = "settingsProcessingLabel";
            this.settingsProcessingLabel.Tag = "";
            this.toolTip1.SetToolTip(this.settingsProcessingLabel, resources.GetString("settingsProcessingLabel.ToolTip"));
            // 
            // presetManagementGroupBox
            // 
            resources.ApplyResources(this.presetManagementGroupBox, "presetManagementGroupBox");
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
            this.dirtyErrorProvider.SetError(this.presetManagementGroupBox, resources.GetString("presetManagementGroupBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.presetManagementGroupBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("presetManagementGroupBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.presetManagementGroupBox, ((int)(resources.GetObject("presetManagementGroupBox.IconPadding"))));
            this.presetManagementGroupBox.Name = "presetManagementGroupBox";
            this.presetManagementGroupBox.TabStop = false;
            this.presetManagementGroupBox.Tag = "";
            this.toolTip1.SetToolTip(this.presetManagementGroupBox, resources.GetString("presetManagementGroupBox.ToolTip"));
            // 
            // buttonDelete
            // 
            resources.ApplyResources(this.buttonDelete, "buttonDelete");
            this.dirtyErrorProvider.SetError(this.buttonDelete, resources.GetString("buttonDelete.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonDelete, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonDelete.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonDelete, ((int)(resources.GetObject("buttonDelete.IconPadding"))));
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Tag = "";
            this.toolTip1.SetToolTip(this.buttonDelete, resources.GetString("buttonDelete.ToolTip"));
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonEdit
            // 
            resources.ApplyResources(this.buttonEdit, "buttonEdit");
            this.dirtyErrorProvider.SetError(this.buttonEdit, resources.GetString("buttonEdit.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonEdit, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonEdit.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonEdit, ((int)(resources.GetObject("buttonEdit.IconPadding"))));
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Tag = "";
            this.toolTip1.SetToolTip(this.buttonEdit, resources.GetString("buttonEdit.ToolTip"));
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonCopy
            // 
            resources.ApplyResources(this.buttonCopy, "buttonCopy");
            this.dirtyErrorProvider.SetError(this.buttonCopy, resources.GetString("buttonCopy.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonCopy, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonCopy.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonCopy, ((int)(resources.GetObject("buttonCopy.IconPadding"))));
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Tag = "";
            this.toolTip1.SetToolTip(this.buttonCopy, resources.GetString("buttonCopy.ToolTip"));
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // buttonCreate
            // 
            resources.ApplyResources(this.buttonCreate, "buttonCreate");
            this.dirtyErrorProvider.SetError(this.buttonCreate, resources.GetString("buttonCreate.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonCreate, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonCreate.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonCreate, ((int)(resources.GetObject("buttonCreate.IconPadding"))));
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Tag = "";
            this.toolTip1.SetToolTip(this.buttonCreate, resources.GetString("buttonCreate.ToolTip"));
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // buttonExportCustom
            // 
            resources.ApplyResources(this.buttonExportCustom, "buttonExportCustom");
            this.dirtyErrorProvider.SetError(this.buttonExportCustom, resources.GetString("buttonExportCustom.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonExportCustom, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonExportCustom.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonExportCustom, ((int)(resources.GetObject("buttonExportCustom.IconPadding"))));
            this.buttonExportCustom.Name = "buttonExportCustom";
            this.buttonExportCustom.Tag = "";
            this.toolTip1.SetToolTip(this.buttonExportCustom, resources.GetString("buttonExportCustom.ToolTip"));
            this.buttonExportCustom.Click += new System.EventHandler(this.buttonExportUser_Click);
            // 
            // buttonDeleteAll
            // 
            resources.ApplyResources(this.buttonDeleteAll, "buttonDeleteAll");
            this.dirtyErrorProvider.SetError(this.buttonDeleteAll, resources.GetString("buttonDeleteAll.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonDeleteAll, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonDeleteAll.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonDeleteAll, ((int)(resources.GetObject("buttonDeleteAll.IconPadding"))));
            this.buttonDeleteAll.Name = "buttonDeleteAll";
            this.buttonDeleteAll.Tag = "";
            this.toolTip1.SetToolTip(this.buttonDeleteAll, resources.GetString("buttonDeleteAll.ToolTip"));
            this.buttonDeleteAll.Click += new System.EventHandler(this.buttonDeleteAll_Click);
            // 
            // buttonImportAll
            // 
            resources.ApplyResources(this.buttonImportAll, "buttonImportAll");
            this.dirtyErrorProvider.SetError(this.buttonImportAll, resources.GetString("buttonImportAll.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonImportAll, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonImportAll.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonImportAll, ((int)(resources.GetObject("buttonImportAll.IconPadding"))));
            this.buttonImportAll.Name = "buttonImportAll";
            this.buttonImportAll.Tag = "";
            this.toolTip1.SetToolTip(this.buttonImportAll, resources.GetString("buttonImportAll.ToolTip"));
            this.buttonImportAll.Click += new System.EventHandler(this.buttonInstallAll_Click);
            // 
            // buttonImportNew
            // 
            resources.ApplyResources(this.buttonImportNew, "buttonImportNew");
            this.dirtyErrorProvider.SetError(this.buttonImportNew, resources.GetString("buttonImportNew.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonImportNew, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonImportNew.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonImportNew, ((int)(resources.GetObject("buttonImportNew.IconPadding"))));
            this.buttonImportNew.Name = "buttonImportNew";
            this.buttonImportNew.Tag = "";
            this.toolTip1.SetToolTip(this.buttonImportNew, resources.GetString("buttonImportNew.ToolTip"));
            this.buttonImportNew.Click += new System.EventHandler(this.buttonInstallNew_Click);
            // 
            // buttonImport
            // 
            resources.ApplyResources(this.buttonImport, "buttonImport");
            this.dirtyErrorProvider.SetError(this.buttonImport, resources.GetString("buttonImport.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonImport, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonImport.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonImport, ((int)(resources.GetObject("buttonImport.IconPadding"))));
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Tag = "";
            this.toolTip1.SetToolTip(this.buttonImport, resources.GetString("buttonImport.ToolTip"));
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // buttonExport
            // 
            resources.ApplyResources(this.buttonExport, "buttonExport");
            this.dirtyErrorProvider.SetError(this.buttonExport, resources.GetString("buttonExport.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonExport, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonExport.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonExport, ((int)(resources.GetObject("buttonExport.IconPadding"))));
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Tag = "";
            this.toolTip1.SetToolTip(this.buttonExport, resources.GetString("buttonExport.ToolTip"));
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // presetManagementLabel
            // 
            resources.ApplyResources(this.presetManagementLabel, "presetManagementLabel");
            this.dirtyErrorProvider.SetError(this.presetManagementLabel, resources.GetString("presetManagementLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.presetManagementLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("presetManagementLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.presetManagementLabel, ((int)(resources.GetObject("presetManagementLabel.IconPadding"))));
            this.presetManagementLabel.Name = "presetManagementLabel";
            this.presetManagementLabel.Tag = "";
            this.toolTip1.SetToolTip(this.presetManagementLabel, resources.GetString("presetManagementLabel.ToolTip"));
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
            this.filterComboBox.Tag = "#filtersPanel";
            this.toolTip1.SetToolTip(this.filterComboBox, resources.GetString("filterComboBox.ToolTip"));
            this.filterComboBox.SelectedIndexChanged += new System.EventHandler(this.filterComboBox_SelectedIndexChanged);
            // 
            // searchTextBox
            // 
            resources.ApplyResources(this.searchTextBox, "searchTextBox");
            this.dirtyErrorProvider.SetError(this.searchTextBox, resources.GetString("searchTextBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.searchTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("searchTextBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.searchTextBox, ((int)(resources.GetObject("searchTextBox.IconPadding"))));
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Tag = "#clearSearchButton";
            this.toolTip1.SetToolTip(this.searchTextBox, resources.GetString("searchTextBox.ToolTip"));
            this.searchTextBox.TextChanged += new System.EventHandler(this.searchTextBox_TextChanged);
            // 
            // buttonSettings
            // 
            resources.ApplyResources(this.buttonSettings, "buttonSettings");
            this.dirtyErrorProvider.SetError(this.buttonSettings, resources.GetString("buttonSettings.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonSettings, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonSettings.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonSettings, ((int)(resources.GetObject("buttonSettings.IconPadding"))));
            this.buttonSettings.Image = global::MusicBeePlugin.Properties.Resources.gear_15;
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Tag = "@non-defaultable@square-control";
            this.toolTip1.SetToolTip(this.buttonSettings, resources.GetString("buttonSettings.ToolTip"));
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // uncheckAllFiltersPictureBox
            // 
            resources.ApplyResources(this.uncheckAllFiltersPictureBox, "uncheckAllFiltersPictureBox");
            this.dirtyErrorProvider.SetError(this.uncheckAllFiltersPictureBox, resources.GetString("uncheckAllFiltersPictureBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.uncheckAllFiltersPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("uncheckAllFiltersPictureBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.uncheckAllFiltersPictureBox, ((int)(resources.GetObject("uncheckAllFiltersPictureBox.IconPadding"))));
            this.uncheckAllFiltersPictureBox.Name = "uncheckAllFiltersPictureBox";
            this.uncheckAllFiltersPictureBox.TabStop = false;
            this.uncheckAllFiltersPictureBox.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.uncheckAllFiltersPictureBox, resources.GetString("uncheckAllFiltersPictureBox.ToolTip"));
            this.uncheckAllFiltersPictureBox.Click += new System.EventHandler(this.uncheckAllFiltersPictureBox_Click);
            // 
            // hotkeyPictureBox
            // 
            resources.ApplyResources(this.hotkeyPictureBox, "hotkeyPictureBox");
            this.dirtyErrorProvider.SetError(this.hotkeyPictureBox, resources.GetString("hotkeyPictureBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.hotkeyPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("hotkeyPictureBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.hotkeyPictureBox, ((int)(resources.GetObject("hotkeyPictureBox.IconPadding"))));
            this.hotkeyPictureBox.Name = "hotkeyPictureBox";
            this.hotkeyPictureBox.TabStop = false;
            this.hotkeyPictureBox.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.hotkeyPictureBox, resources.GetString("hotkeyPictureBox.ToolTip"));
            this.hotkeyPictureBox.Click += new System.EventHandler(this.hotkeyPictureBox_Click);
            // 
            // functionIdPictureBox
            // 
            resources.ApplyResources(this.functionIdPictureBox, "functionIdPictureBox");
            this.dirtyErrorProvider.SetError(this.functionIdPictureBox, resources.GetString("functionIdPictureBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.functionIdPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("functionIdPictureBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.functionIdPictureBox, ((int)(resources.GetObject("functionIdPictureBox.IconPadding"))));
            this.functionIdPictureBox.Name = "functionIdPictureBox";
            this.functionIdPictureBox.TabStop = false;
            this.functionIdPictureBox.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.functionIdPictureBox, resources.GetString("functionIdPictureBox.ToolTip"));
            this.functionIdPictureBox.Click += new System.EventHandler(this.functionIdPictureBox_Click);
            // 
            // playlistPictureBox
            // 
            resources.ApplyResources(this.playlistPictureBox, "playlistPictureBox");
            this.dirtyErrorProvider.SetError(this.playlistPictureBox, resources.GetString("playlistPictureBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.playlistPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("playlistPictureBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.playlistPictureBox, ((int)(resources.GetObject("playlistPictureBox.IconPadding"))));
            this.playlistPictureBox.Name = "playlistPictureBox";
            this.playlistPictureBox.TabStop = false;
            this.playlistPictureBox.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.playlistPictureBox, resources.GetString("playlistPictureBox.ToolTip"));
            this.playlistPictureBox.Click += new System.EventHandler(this.playlistPictureBox_Click);
            // 
            // userPictureBox
            // 
            resources.ApplyResources(this.userPictureBox, "userPictureBox");
            this.dirtyErrorProvider.SetError(this.userPictureBox, resources.GetString("userPictureBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.userPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("userPictureBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.userPictureBox, ((int)(resources.GetObject("userPictureBox.IconPadding"))));
            this.userPictureBox.Name = "userPictureBox";
            this.userPictureBox.TabStop = false;
            this.userPictureBox.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.userPictureBox, resources.GetString("userPictureBox.ToolTip"));
            this.userPictureBox.Click += new System.EventHandler(this.userPictureBox_Click);
            // 
            // customizedPictureBox
            // 
            resources.ApplyResources(this.customizedPictureBox, "customizedPictureBox");
            this.dirtyErrorProvider.SetError(this.customizedPictureBox, resources.GetString("customizedPictureBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.customizedPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("customizedPictureBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.customizedPictureBox, ((int)(resources.GetObject("customizedPictureBox.IconPadding"))));
            this.customizedPictureBox.Name = "customizedPictureBox";
            this.customizedPictureBox.TabStop = false;
            this.customizedPictureBox.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.customizedPictureBox, resources.GetString("customizedPictureBox.ToolTip"));
            this.customizedPictureBox.Click += new System.EventHandler(this.customizedPictureBox_Click);
            // 
            // predefinedPictureBox
            // 
            resources.ApplyResources(this.predefinedPictureBox, "predefinedPictureBox");
            this.dirtyErrorProvider.SetError(this.predefinedPictureBox, resources.GetString("predefinedPictureBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.predefinedPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("predefinedPictureBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.predefinedPictureBox, ((int)(resources.GetObject("predefinedPictureBox.IconPadding"))));
            this.predefinedPictureBox.Name = "predefinedPictureBox";
            this.predefinedPictureBox.TabStop = false;
            this.predefinedPictureBox.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.predefinedPictureBox, resources.GetString("predefinedPictureBox.ToolTip"));
            this.predefinedPictureBox.Click += new System.EventHandler(this.predefinedPictureBox_Click);
            // 
            // tickedOnlyPictureBox
            // 
            resources.ApplyResources(this.tickedOnlyPictureBox, "tickedOnlyPictureBox");
            this.dirtyErrorProvider.SetError(this.tickedOnlyPictureBox, resources.GetString("tickedOnlyPictureBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.tickedOnlyPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tickedOnlyPictureBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.tickedOnlyPictureBox, ((int)(resources.GetObject("tickedOnlyPictureBox.IconPadding"))));
            this.tickedOnlyPictureBox.Name = "tickedOnlyPictureBox";
            this.tickedOnlyPictureBox.TabStop = false;
            this.tickedOnlyPictureBox.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.tickedOnlyPictureBox, resources.GetString("tickedOnlyPictureBox.ToolTip"));
            this.tickedOnlyPictureBox.Click += new System.EventHandler(this.tickedOnlyPictureBox_Click);
            // 
            // clearSearchButton
            // 
            resources.ApplyResources(this.clearSearchButton, "clearSearchButton");
            this.dirtyErrorProvider.SetError(this.clearSearchButton, resources.GetString("clearSearchButton.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.clearSearchButton, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("clearSearchButton.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.clearSearchButton, ((int)(resources.GetObject("clearSearchButton.IconPadding"))));
            this.clearSearchButton.Name = "clearSearchButton";
            this.clearSearchButton.Tag = "@non-defaultable@square-control";
            this.toolTip1.SetToolTip(this.clearSearchButton, resources.GetString("clearSearchButton.ToolTip"));
            this.clearSearchButton.Click += new System.EventHandler(this.clearSearchButton_Click);
            // 
            // autoApplyPresetsLabel
            // 
            resources.ApplyResources(this.autoApplyPresetsLabel, "autoApplyPresetsLabel");
            this.autoApplyPresetsLabel.AutoEllipsis = true;
            this.dirtyErrorProvider.SetError(this.autoApplyPresetsLabel, resources.GetString("autoApplyPresetsLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.autoApplyPresetsLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("autoApplyPresetsLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.autoApplyPresetsLabel, ((int)(resources.GetObject("autoApplyPresetsLabel.IconPadding"))));
            this.autoApplyPresetsLabel.Name = "autoApplyPresetsLabel";
            this.autoApplyPresetsLabel.Tag = "#AdvancedSearchAndReplace&filtersPanel";
            this.toolTip1.SetToolTip(this.autoApplyPresetsLabel, resources.GetString("autoApplyPresetsLabel.ToolTip"));
            this.autoApplyPresetsLabel.Click += new System.EventHandler(this.label5_Click);
            // 
            // searchPictureBox
            // 
            resources.ApplyResources(this.searchPictureBox, "searchPictureBox");
            this.dirtyErrorProvider.SetError(this.searchPictureBox, resources.GetString("searchPictureBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.searchPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("searchPictureBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.searchPictureBox, ((int)(resources.GetObject("searchPictureBox.IconPadding"))));
            this.searchPictureBox.Image = global::MusicBeePlugin.Properties.Resources.search;
            this.searchPictureBox.Name = "searchPictureBox";
            this.searchPictureBox.TabStop = false;
            this.searchPictureBox.Tag = "#searchTextBox@square-control";
            this.toolTip1.SetToolTip(this.searchPictureBox, resources.GetString("searchPictureBox.ToolTip"));
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
            this.dirtyErrorProvider.SetError(this.filtersPanel, resources.GetString("filtersPanel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.filtersPanel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("filtersPanel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.filtersPanel, ((int)(resources.GetObject("filtersPanel.IconPadding"))));
            this.filtersPanel.Name = "filtersPanel";
            this.filtersPanel.Tag = "#AdvancedSearchAndReplace&splitContainer1";
            this.toolTip1.SetToolTip(this.filtersPanel, resources.GetString("filtersPanel.ToolTip"));
            // 
            // dirtyErrorProvider
            // 
            this.dirtyErrorProvider.BlinkRate = 1000;
            this.dirtyErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
            this.dirtyErrorProvider.ContainerControl = this;
            resources.ApplyResources(this.dirtyErrorProvider, "dirtyErrorProvider");
            // 
            // AdvancedSearchAndReplace
            // 
            this.AcceptButton = this.buttonPreview;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.autoApplyPresetsLabel);
            this.Controls.Add(this.filtersPanel);
            this.DoubleBuffered = true;
            this.Name = "AdvancedSearchAndReplace";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AdvancedSearchAndReplace_FormClosing);
            this.Load += new System.EventHandler(this.AdvancedSearchAndReplace_Load);
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
            this.descriptionGroupBox.ResumeLayout(false);
            this.descriptionGroupBox.PerformLayout();
            this.textBoxTableLayoutPanel.ResumeLayout(false);
            this.textBoxTableLayoutPanel.PerformLayout();
            this.listBoxTableLayoutPanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(previewTable)).EndInit();
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

            //MusicBee
            new TextBoxBorder(this.searchTextBox);
            new TextBoxBorder(this.customTextBox);
            new TextBoxBorder(this.customText2Box);
            new TextBoxBorder(this.customText3Box);
            new TextBoxBorder(this.customText4Box);
            new TextBoxBorder(this.idTextBox);
            new TextBoxBorder(this.preserveTagValuesTextBox);
            new TextBoxBorder(this.processPreserveTagsTextBox);
            new TextBoxBorder(this.descriptionBox);
            //~MusicBee
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
        private System.Windows.Forms.GroupBox descriptionGroupBox;
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
        private System.Windows.Forms.Label descriptionGroupBoxLabel;
        private System.Windows.Forms.Panel filtersPanel;
        private System.Windows.Forms.TableLayoutPanel listBoxTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel textBoxTableLayoutPanel;
    }
}