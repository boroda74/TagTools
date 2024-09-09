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

                source?.Dispose();

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.previewTable = new System.Windows.Forms.DataGridView();
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
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel4);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Tag = "#AdvancedSearchAndReplace&AdvancedSearchAndReplace";
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.listBoxTableLayoutPanel, 0, 0);
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
            this.panel1.Controls.Add(this.descriptionGroupBox);
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
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // preserveTagValuesTextBox
            // 
            resources.ApplyResources(this.preserveTagValuesTextBox, "preserveTagValuesTextBox");
            this.preserveTagValuesTextBox.Name = "preserveTagValuesTextBox";
            this.toolTip1.SetToolTip(this.preserveTagValuesTextBox, resources.GetString("preserveTagValuesTextBox.ToolTip"));
            this.preserveTagValuesTextBox.TextChanged += new System.EventHandler(this.preserveTagValuesTextBox_TextChanged);
            // 
            // labelPreserveTagValues
            // 
            resources.ApplyResources(this.labelPreserveTagValues, "labelPreserveTagValues");
            this.labelPreserveTagValues.Name = "labelPreserveTagValues";
            this.toolTip1.SetToolTip(this.labelPreserveTagValues, resources.GetString("labelPreserveTagValues.ToolTip"));
            // 
            // buttonSelectPreservedTags
            // 
            resources.ApplyResources(this.buttonSelectPreservedTags, "buttonSelectPreservedTags");
            this.buttonSelectPreservedTags.Name = "buttonSelectPreservedTags";
            this.buttonSelectPreservedTags.Tag = "@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonSelectPreservedTags, resources.GetString("buttonSelectPreservedTags.ToolTip"));
            this.buttonSelectPreservedTags.Click += new System.EventHandler(this.buttonSelectPreservedTags_Click);
            // 
            // processPreserveTagsTextBox
            // 
            resources.ApplyResources(this.processPreserveTagsTextBox, "processPreserveTagsTextBox");
            this.processPreserveTagsTextBox.Name = "processPreserveTagsTextBox";
            this.toolTip1.SetToolTip(this.processPreserveTagsTextBox, resources.GetString("processPreserveTagsTextBox.ToolTip"));
            this.processPreserveTagsTextBox.TextChanged += new System.EventHandler(this.processPreserveTagsTextBox_TextChanged);
            // 
            // buttonProcessPreserveTags
            // 
            resources.ApplyResources(this.buttonProcessPreserveTags, "buttonProcessPreserveTags");
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
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // customText4Box
            // 
            resources.ApplyResources(this.customText4Box, "customText4Box");
            this.customText4Box.Name = "customText4Box";
            this.customText4Box.TextChanged += new System.EventHandler(this.customText4Box_TextChanged);
            // 
            // customText4Label
            // 
            resources.ApplyResources(this.customText4Label, "customText4Label");
            this.customText4Label.Name = "customText4Label";
            // 
            // customText3Box
            // 
            resources.ApplyResources(this.customText3Box, "customText3Box");
            this.customText3Box.Name = "customText3Box";
            this.customText3Box.TextChanged += new System.EventHandler(this.customText3Box_TextChanged);
            // 
            // customText3Label
            // 
            resources.ApplyResources(this.customText3Label, "customText3Label");
            this.customText3Label.Name = "customText3Label";
            // 
            // customText2Box
            // 
            resources.ApplyResources(this.customText2Box, "customText2Box");
            this.customText2Box.Name = "customText2Box";
            this.customText2Box.TextChanged += new System.EventHandler(this.customText2Box_TextChanged);
            // 
            // customText2Label
            // 
            resources.ApplyResources(this.customText2Label, "customText2Label");
            this.customText2Label.Name = "customText2Label";
            // 
            // customTextBox
            // 
            resources.ApplyResources(this.customTextBox, "customTextBox");
            this.customTextBox.Name = "customTextBox";
            this.customTextBox.TextChanged += new System.EventHandler(this.customTextBox_TextChanged);
            // 
            // customTextLabel
            // 
            resources.ApplyResources(this.customTextLabel, "customTextLabel");
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
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // parameterTag6List
            // 
            resources.ApplyResources(this.parameterTag6List, "parameterTag6List");
            this.parameterTag6List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterTag6List.DropDownWidth = 250;
            this.parameterTag6List.FormattingEnabled = true;
            this.parameterTag6List.Name = "parameterTag6List";
            this.parameterTag6List.SelectedIndexChanged += new System.EventHandler(this.parameterTag6_SelectedIndexChanged);
            // 
            // labelTag6
            // 
            resources.ApplyResources(this.labelTag6, "labelTag6");
            this.labelTag6.Name = "labelTag6";
            // 
            // pictureTag6
            // 
            resources.ApplyResources(this.pictureTag6, "pictureTag6");
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
            this.parameterTag5List.Name = "parameterTag5List";
            this.parameterTag5List.SelectedIndexChanged += new System.EventHandler(this.parameterTag5_SelectedIndexChanged);
            // 
            // labelTag5
            // 
            resources.ApplyResources(this.labelTag5, "labelTag5");
            this.labelTag5.Name = "labelTag5";
            // 
            // pictureTag5
            // 
            resources.ApplyResources(this.pictureTag5, "pictureTag5");
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
            this.parameterTag4List.Name = "parameterTag4List";
            this.parameterTag4List.SelectedIndexChanged += new System.EventHandler(this.parameterTag4_SelectedIndexChanged);
            // 
            // labelTag4
            // 
            resources.ApplyResources(this.labelTag4, "labelTag4");
            this.labelTag4.Name = "labelTag4";
            // 
            // pictureTag4
            // 
            resources.ApplyResources(this.pictureTag4, "pictureTag4");
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
            this.parameterTag3List.Name = "parameterTag3List";
            this.parameterTag3List.SelectedIndexChanged += new System.EventHandler(this.parameterTag3_SelectedIndexChanged);
            // 
            // labelTag3
            // 
            resources.ApplyResources(this.labelTag3, "labelTag3");
            this.labelTag3.Name = "labelTag3";
            // 
            // pictureTag3
            // 
            resources.ApplyResources(this.pictureTag3, "pictureTag3");
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
            this.parameterTag2List.Name = "parameterTag2List";
            this.parameterTag2List.SelectedIndexChanged += new System.EventHandler(this.parameterTag2_SelectedIndexChanged);
            // 
            // labelTag2
            // 
            resources.ApplyResources(this.labelTag2, "labelTag2");
            this.labelTag2.Name = "labelTag2";
            // 
            // pictureTag2
            // 
            resources.ApplyResources(this.pictureTag2, "pictureTag2");
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
            this.parameterTagList.Name = "parameterTagList";
            this.parameterTagList.SelectedIndexChanged += new System.EventHandler(this.parameterTag_SelectedIndexChanged);
            // 
            // labelTag
            // 
            resources.ApplyResources(this.labelTag, "labelTag");
            this.labelTag.Name = "labelTag";
            this.toolTip1.SetToolTip(this.labelTag, resources.GetString("labelTag.ToolTip"));
            // 
            // pictureTag
            // 
            resources.ApplyResources(this.pictureTag, "pictureTag");
            this.pictureTag.Image = global::MusicBeePlugin.Properties.Resources.warning_15;
            this.pictureTag.Name = "pictureTag";
            this.pictureTag.TabStop = false;
            // 
            // userPresetLabel
            // 
            resources.ApplyResources(this.userPresetLabel, "userPresetLabel");
            this.userPresetLabel.Name = "userPresetLabel";
            this.userPresetLabel.Tag = "#splitContainer1@pinned-to-parent-x";
            // 
            // userPresetPictureBox
            // 
            resources.ApplyResources(this.userPresetPictureBox, "userPresetPictureBox");
            this.userPresetPictureBox.Image = global::MusicBeePlugin.Properties.Resources.user_presets;
            this.userPresetPictureBox.Name = "userPresetPictureBox";
            this.userPresetPictureBox.TabStop = false;
            this.userPresetPictureBox.Tag = "#userPresetLabel@square-control@small-picture";
            // 
            // customizedPresetLabel
            // 
            resources.ApplyResources(this.customizedPresetLabel, "customizedPresetLabel");
            this.customizedPresetLabel.Name = "customizedPresetLabel";
            this.customizedPresetLabel.Tag = "#userPresetPictureBox";
            // 
            // customizedPresetPictureBox
            // 
            resources.ApplyResources(this.customizedPresetPictureBox, "customizedPresetPictureBox");
            this.customizedPresetPictureBox.Image = global::MusicBeePlugin.Properties.Resources.customized_presets;
            this.customizedPresetPictureBox.Name = "customizedPresetPictureBox";
            this.customizedPresetPictureBox.TabStop = false;
            this.customizedPresetPictureBox.Tag = "#customizedPresetLabel@square-control@small-picture";
            // 
            // favoriteCheckBoxLabel
            // 
            resources.ApplyResources(this.favoriteCheckBoxLabel, "favoriteCheckBoxLabel");
            this.favoriteCheckBoxLabel.Name = "favoriteCheckBoxLabel";
            this.favoriteCheckBoxLabel.Tag = "";
            this.favoriteCheckBoxLabel.Click += new System.EventHandler(this.favoriteCheckBoxLabel_Click);
            // 
            // favoriteCheckBox
            // 
            resources.ApplyResources(this.favoriteCheckBox, "favoriteCheckBox");
            this.favoriteCheckBox.Name = "favoriteCheckBox";
            this.favoriteCheckBox.Tag = "#favoriteCheckBoxLabel";
            this.favoriteCheckBox.CheckedChanged += new System.EventHandler(this.favoriteCheckBox_CheckedChanged);
            // 
            // applyToPlayingTrackCheckBoxLabel
            // 
            resources.ApplyResources(this.applyToPlayingTrackCheckBoxLabel, "applyToPlayingTrackCheckBoxLabel");
            this.applyToPlayingTrackCheckBoxLabel.Name = "applyToPlayingTrackCheckBoxLabel";
            this.applyToPlayingTrackCheckBoxLabel.Tag = "#favoriteCheckBox";
            this.toolTip1.SetToolTip(this.applyToPlayingTrackCheckBoxLabel, resources.GetString("applyToPlayingTrackCheckBoxLabel.ToolTip"));
            this.applyToPlayingTrackCheckBoxLabel.Click += new System.EventHandler(this.applyToPlayingTrackCheckBoxLabel_Click);
            // 
            // applyToPlayingTrackCheckBox
            // 
            resources.ApplyResources(this.applyToPlayingTrackCheckBox, "applyToPlayingTrackCheckBox");
            this.applyToPlayingTrackCheckBox.Name = "applyToPlayingTrackCheckBox";
            this.applyToPlayingTrackCheckBox.Tag = "#applyToPlayingTrackCheckBoxLabel";
            this.toolTip1.SetToolTip(this.applyToPlayingTrackCheckBox, resources.GetString("applyToPlayingTrackCheckBox.ToolTip"));
            this.applyToPlayingTrackCheckBox.CheckedChanged += new System.EventHandler(this.applyToPlayingTrackCheckBox_CheckedChanged);
            // 
            // assignHotkeyCheckBoxLabel
            // 
            resources.ApplyResources(this.assignHotkeyCheckBoxLabel, "assignHotkeyCheckBoxLabel");
            this.assignHotkeyCheckBoxLabel.Name = "assignHotkeyCheckBoxLabel";
            this.assignHotkeyCheckBoxLabel.Tag = "#applyToPlayingTrackCheckBox";
            this.toolTip1.SetToolTip(this.assignHotkeyCheckBoxLabel, resources.GetString("assignHotkeyCheckBoxLabel.ToolTip"));
            this.assignHotkeyCheckBoxLabel.Click += new System.EventHandler(this.assignHotkeyCheckBoxLabel_Click);
            // 
            // assignHotkeyCheckBox
            // 
            resources.ApplyResources(this.assignHotkeyCheckBox, "assignHotkeyCheckBox");
            this.assignHotkeyCheckBox.Name = "assignHotkeyCheckBox";
            this.assignHotkeyCheckBox.Tag = "#assignHotkeyCheckBoxLabel";
            this.toolTip1.SetToolTip(this.assignHotkeyCheckBox, resources.GetString("assignHotkeyCheckBox.ToolTip"));
            this.assignHotkeyCheckBox.CheckedChanged += new System.EventHandler(this.assignHotkeyCheckBox_CheckedChanged);
            // 
            // clearIdButton
            // 
            resources.ApplyResources(this.clearIdButton, "clearIdButton");
            this.clearIdButton.Name = "clearIdButton";
            this.clearIdButton.Tag = "@pinned-to-parent-x@non-defaultable@square-control";
            this.toolTip1.SetToolTip(this.clearIdButton, resources.GetString("clearIdButton.ToolTip"));
            this.clearIdButton.Click += new System.EventHandler(this.clearIdButton_Click);
            // 
            // idTextBox
            // 
            resources.ApplyResources(this.idTextBox, "idTextBox");
            this.idTextBox.Name = "idTextBox";
            this.idTextBox.Tag = "#clearIdButton";
            this.toolTip1.SetToolTip(this.idTextBox, resources.GetString("idTextBox.ToolTip"));
            this.idTextBox.Leave += new System.EventHandler(this.idTextBox_Leave);
            // 
            // idLabel
            // 
            resources.ApplyResources(this.idLabel, "idLabel");
            this.idLabel.Name = "idLabel";
            this.idLabel.Tag = "#idTextBox";
            // 
            // playlistComboBox
            // 
            resources.ApplyResources(this.playlistComboBox, "playlistComboBox");
            this.playlistComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playlistComboBox.DropDownWidth = 250;
            this.playlistComboBox.FormattingEnabled = true;
            this.playlistComboBox.Name = "playlistComboBox";
            this.playlistComboBox.Tag = "#idLabel";
            this.playlistComboBox.SelectedIndexChanged += new System.EventHandler(this.playlistComboBox_SelectedIndexChanged);
            // 
            // conditionCheckBoxLabel
            // 
            resources.ApplyResources(this.conditionCheckBoxLabel, "conditionCheckBoxLabel");
            this.conditionCheckBoxLabel.Name = "conditionCheckBoxLabel";
            this.conditionCheckBoxLabel.Tag = "#playlistComboBox";
            this.conditionCheckBoxLabel.Click += new System.EventHandler(this.conditionCheckBoxLabel_Click);
            // 
            // conditionCheckBox
            // 
            resources.ApplyResources(this.conditionCheckBox, "conditionCheckBox");
            this.conditionCheckBox.Name = "conditionCheckBox";
            this.conditionCheckBox.Tag = "#conditionCheckBoxLabel";
            this.conditionCheckBox.CheckedChanged += new System.EventHandler(this.conditionCheckBox_CheckedChanged);
            // 
            // descriptionGroupBox
            // 
            resources.ApplyResources(this.descriptionGroupBox, "descriptionGroupBox");
            this.descriptionGroupBox.Controls.Add(this.textBoxTableLayoutPanel);
            this.descriptionGroupBox.Controls.Add(this.descriptionGroupBoxLabel);
            this.descriptionGroupBox.Name = "descriptionGroupBox";
            this.descriptionGroupBox.TabStop = false;
            // 
            // textBoxTableLayoutPanel
            // 
            resources.ApplyResources(this.textBoxTableLayoutPanel, "textBoxTableLayoutPanel");
            this.textBoxTableLayoutPanel.Controls.Add(this.descriptionBox, 0, 0);
            this.textBoxTableLayoutPanel.Name = "textBoxTableLayoutPanel";
            this.textBoxTableLayoutPanel.Tag = "";
            // 
            // descriptionBox
            // 
            resources.ApplyResources(this.descriptionBox, "descriptionBox");
            this.descriptionBox.Name = "descriptionBox";
            this.descriptionBox.ReadOnly = true;
            // 
            // descriptionGroupBoxLabel
            // 
            resources.ApplyResources(this.descriptionGroupBoxLabel, "descriptionGroupBoxLabel");
            this.descriptionGroupBoxLabel.Name = "descriptionGroupBoxLabel";
            // 
            // listBoxTableLayoutPanel
            // 
            resources.ApplyResources(this.listBoxTableLayoutPanel, "listBoxTableLayoutPanel");
            this.listBoxTableLayoutPanel.Controls.Add(this.presetList, 0, 0);
            this.listBoxTableLayoutPanel.Name = "listBoxTableLayoutPanel";
            // 
            // presetList
            // 
            resources.ApplyResources(this.presetList, "presetList");
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
            this.previewTable.MultiSelect = false;
            this.previewTable.Name = "previewTable";
            this.previewTable.RowHeadersVisible = false;
            this.previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.previewTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.previewTable_CellContentClick);
            this.previewTable.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.previewTable_ColumnWidthChanged);
            this.previewTable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PreviewTable_MouseDown);
            this.previewTable.MouseLeave += new System.EventHandler(this.PreviewTable_MouseLeave);
            this.previewTable.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PreviewTable_MouseUp);
            // 
            // settingsProcessingGroupBox
            // 
            resources.ApplyResources(this.settingsProcessingGroupBox, "settingsProcessingGroupBox");
            this.settingsProcessingGroupBox.Controls.Add(this.buttonOK);
            this.settingsProcessingGroupBox.Controls.Add(this.buttonPreview);
            this.settingsProcessingGroupBox.Controls.Add(this.buttonClose);
            this.settingsProcessingGroupBox.Controls.Add(this.buttonSaveClose);
            this.settingsProcessingGroupBox.Controls.Add(this.settingsProcessingLabel);
            this.settingsProcessingGroupBox.Name = "settingsProcessingGroupBox";
            this.settingsProcessingGroupBox.TabStop = false;
            this.settingsProcessingGroupBox.Tag = "";
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
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
            this.buttonSaveClose.Name = "buttonSaveClose";
            this.buttonSaveClose.Tag = "";
            this.toolTip1.SetToolTip(this.buttonSaveClose, resources.GetString("buttonSaveClose.ToolTip"));
            this.buttonSaveClose.Click += new System.EventHandler(this.buttonSaveClose_Click);
            // 
            // settingsProcessingLabel
            // 
            resources.ApplyResources(this.settingsProcessingLabel, "settingsProcessingLabel");
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
            resources.ApplyResources(this.presetManagementGroupBox, "presetManagementGroupBox");
            this.presetManagementGroupBox.Name = "presetManagementGroupBox";
            this.presetManagementGroupBox.TabStop = false;
            this.presetManagementGroupBox.Tag = "";
            // 
            // buttonDelete
            // 
            resources.ApplyResources(this.buttonDelete, "buttonDelete");
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Tag = "";
            this.toolTip1.SetToolTip(this.buttonDelete, resources.GetString("buttonDelete.ToolTip"));
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonEdit
            // 
            resources.ApplyResources(this.buttonEdit, "buttonEdit");
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Tag = "";
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonCopy
            // 
            resources.ApplyResources(this.buttonCopy, "buttonCopy");
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Tag = "";
            this.toolTip1.SetToolTip(this.buttonCopy, resources.GetString("buttonCopy.ToolTip"));
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // buttonCreate
            // 
            resources.ApplyResources(this.buttonCreate, "buttonCreate");
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Tag = "";
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // buttonExportCustom
            // 
            resources.ApplyResources(this.buttonExportCustom, "buttonExportCustom");
            this.buttonExportCustom.Name = "buttonExportCustom";
            this.buttonExportCustom.Tag = "";
            this.toolTip1.SetToolTip(this.buttonExportCustom, resources.GetString("buttonExportCustom.ToolTip"));
            this.buttonExportCustom.Click += new System.EventHandler(this.buttonExportUser_Click);
            // 
            // buttonDeleteAll
            // 
            resources.ApplyResources(this.buttonDeleteAll, "buttonDeleteAll");
            this.buttonDeleteAll.Name = "buttonDeleteAll";
            this.buttonDeleteAll.Tag = "";
            this.toolTip1.SetToolTip(this.buttonDeleteAll, resources.GetString("buttonDeleteAll.ToolTip"));
            this.buttonDeleteAll.Click += new System.EventHandler(this.buttonDeleteAll_Click);
            // 
            // buttonImportAll
            // 
            resources.ApplyResources(this.buttonImportAll, "buttonImportAll");
            this.buttonImportAll.Name = "buttonImportAll";
            this.buttonImportAll.Tag = "";
            this.toolTip1.SetToolTip(this.buttonImportAll, resources.GetString("buttonImportAll.ToolTip"));
            this.buttonImportAll.Click += new System.EventHandler(this.buttonInstallAll_Click);
            // 
            // buttonImportNew
            // 
            resources.ApplyResources(this.buttonImportNew, "buttonImportNew");
            this.buttonImportNew.Name = "buttonImportNew";
            this.buttonImportNew.Tag = "";
            this.toolTip1.SetToolTip(this.buttonImportNew, resources.GetString("buttonImportNew.ToolTip"));
            this.buttonImportNew.Click += new System.EventHandler(this.buttonInstallNew_Click);
            // 
            // buttonImport
            // 
            resources.ApplyResources(this.buttonImport, "buttonImport");
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Tag = "";
            this.toolTip1.SetToolTip(this.buttonImport, resources.GetString("buttonImport.ToolTip"));
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // buttonExport
            // 
            resources.ApplyResources(this.buttonExport, "buttonExport");
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Tag = "";
            this.toolTip1.SetToolTip(this.buttonExport, resources.GetString("buttonExport.ToolTip"));
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // presetManagementLabel
            // 
            resources.ApplyResources(this.presetManagementLabel, "presetManagementLabel");
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
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Tag = "#clearSearchButton";
            this.toolTip1.SetToolTip(this.searchTextBox, resources.GetString("searchTextBox.ToolTip"));
            this.searchTextBox.TextChanged += new System.EventHandler(this.searchTextBox_TextChanged);
            // 
            // buttonSettings
            // 
            resources.ApplyResources(this.buttonSettings, "buttonSettings");
            this.buttonSettings.Image = global::MusicBeePlugin.Properties.Resources.gear_15;
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Tag = "@non-defaultable@square-control";
            this.toolTip1.SetToolTip(this.buttonSettings, resources.GetString("buttonSettings.ToolTip"));
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // uncheckAllFiltersPictureBox
            // 
            resources.ApplyResources(this.uncheckAllFiltersPictureBox, "uncheckAllFiltersPictureBox");
            this.uncheckAllFiltersPictureBox.Name = "uncheckAllFiltersPictureBox";
            this.uncheckAllFiltersPictureBox.TabStop = false;
            this.uncheckAllFiltersPictureBox.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.uncheckAllFiltersPictureBox, resources.GetString("uncheckAllFiltersPictureBox.ToolTip"));
            this.uncheckAllFiltersPictureBox.Click += new System.EventHandler(this.uncheckAllFiltersPictureBox_Click);
            // 
            // hotkeyPictureBox
            // 
            resources.ApplyResources(this.hotkeyPictureBox, "hotkeyPictureBox");
            this.hotkeyPictureBox.Name = "hotkeyPictureBox";
            this.hotkeyPictureBox.TabStop = false;
            this.hotkeyPictureBox.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.hotkeyPictureBox, resources.GetString("hotkeyPictureBox.ToolTip"));
            this.hotkeyPictureBox.Click += new System.EventHandler(this.hotkeyPictureBox_Click);
            // 
            // functionIdPictureBox
            // 
            resources.ApplyResources(this.functionIdPictureBox, "functionIdPictureBox");
            this.functionIdPictureBox.Name = "functionIdPictureBox";
            this.functionIdPictureBox.TabStop = false;
            this.functionIdPictureBox.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.functionIdPictureBox, resources.GetString("functionIdPictureBox.ToolTip"));
            this.functionIdPictureBox.Click += new System.EventHandler(this.functionIdPictureBox_Click);
            // 
            // playlistPictureBox
            // 
            resources.ApplyResources(this.playlistPictureBox, "playlistPictureBox");
            this.playlistPictureBox.Name = "playlistPictureBox";
            this.playlistPictureBox.TabStop = false;
            this.playlistPictureBox.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.playlistPictureBox, resources.GetString("playlistPictureBox.ToolTip"));
            this.playlistPictureBox.Click += new System.EventHandler(this.playlistPictureBox_Click);
            // 
            // userPictureBox
            // 
            resources.ApplyResources(this.userPictureBox, "userPictureBox");
            this.userPictureBox.Name = "userPictureBox";
            this.userPictureBox.TabStop = false;
            this.userPictureBox.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.userPictureBox, resources.GetString("userPictureBox.ToolTip"));
            this.userPictureBox.Click += new System.EventHandler(this.userPictureBox_Click);
            // 
            // customizedPictureBox
            // 
            resources.ApplyResources(this.customizedPictureBox, "customizedPictureBox");
            this.customizedPictureBox.Name = "customizedPictureBox";
            this.customizedPictureBox.TabStop = false;
            this.customizedPictureBox.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.customizedPictureBox, resources.GetString("customizedPictureBox.ToolTip"));
            this.customizedPictureBox.Click += new System.EventHandler(this.customizedPictureBox_Click);
            // 
            // predefinedPictureBox
            // 
            resources.ApplyResources(this.predefinedPictureBox, "predefinedPictureBox");
            this.predefinedPictureBox.Name = "predefinedPictureBox";
            this.predefinedPictureBox.TabStop = false;
            this.predefinedPictureBox.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.predefinedPictureBox, resources.GetString("predefinedPictureBox.ToolTip"));
            this.predefinedPictureBox.Click += new System.EventHandler(this.predefinedPictureBox_Click);
            // 
            // tickedOnlyPictureBox
            // 
            resources.ApplyResources(this.tickedOnlyPictureBox, "tickedOnlyPictureBox");
            this.tickedOnlyPictureBox.Name = "tickedOnlyPictureBox";
            this.tickedOnlyPictureBox.TabStop = false;
            this.tickedOnlyPictureBox.Tag = "@square-control";
            this.toolTip1.SetToolTip(this.tickedOnlyPictureBox, resources.GetString("tickedOnlyPictureBox.ToolTip"));
            this.tickedOnlyPictureBox.Click += new System.EventHandler(this.tickedOnlyPictureBox_Click);
            // 
            // clearSearchButton
            // 
            resources.ApplyResources(this.clearSearchButton, "clearSearchButton");
            this.clearSearchButton.Name = "clearSearchButton";
            this.clearSearchButton.Tag = "@non-defaultable@square-control";
            this.toolTip1.SetToolTip(this.clearSearchButton, resources.GetString("clearSearchButton.ToolTip"));
            this.clearSearchButton.Click += new System.EventHandler(this.clearSearchButton_Click);
            // 
            // autoApplyPresetsLabel
            // 
            resources.ApplyResources(this.autoApplyPresetsLabel, "autoApplyPresetsLabel");
            this.autoApplyPresetsLabel.AutoEllipsis = true;
            this.autoApplyPresetsLabel.Name = "autoApplyPresetsLabel";
            this.autoApplyPresetsLabel.Tag = "#AdvancedSearchAndReplace&filtersPanel";
            this.autoApplyPresetsLabel.Click += new System.EventHandler(this.label5_Click);
            // 
            // searchPictureBox
            // 
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
            this.filtersPanel.Name = "filtersPanel";
            this.filtersPanel.Tag = "#AdvancedSearchAndReplace&splitContainer1";
            // 
            // dirtyErrorProvider
            // 
            this.dirtyErrorProvider.BlinkRate = 1000;
            this.dirtyErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
            this.dirtyErrorProvider.ContainerControl = this;
            resources.ApplyResources(this.dirtyErrorProvider, "dirtyErrorProvider");
            // 
            // PresetGuid
            // 
            this.PresetGuid.DataPropertyName = "PresetGuid";
            this.PresetGuid.FillWeight = 1F;
            resources.ApplyResources(this.PresetGuid, "PresetGuid");
            this.PresetGuid.Name = "PresetGuid";
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
            this.Track.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Track.DataPropertyName = "Track";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Track.DefaultCellStyle = dataGridViewCellStyle1;
            this.Track.FillWeight = 60F;
            resources.ApplyResources(this.Track, "Track");
            this.Track.Name = "Track";
            // 
            // TagName1
            // 
            this.TagName1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TagName1.DataPropertyName = "TagName1";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.TagName1.DefaultCellStyle = dataGridViewCellStyle2;
            this.TagName1.FillWeight = 24.52376F;
            resources.ApplyResources(this.TagName1, "TagName1");
            this.TagName1.Name = "TagName1";
            // 
            // OriginalTag1
            // 
            this.OriginalTag1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.OriginalTag1.DataPropertyName = "OriginalTag1";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.OriginalTag1.DefaultCellStyle = dataGridViewCellStyle3;
            this.OriginalTag1.FillWeight = 24.52376F;
            resources.ApplyResources(this.OriginalTag1, "OriginalTag1");
            this.OriginalTag1.Name = "OriginalTag1";
            // 
            // NewTag1
            // 
            this.NewTag1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NewTag1.DataPropertyName = "NewTag1";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.NewTag1.DefaultCellStyle = dataGridViewCellStyle4;
            this.NewTag1.FillWeight = 24.52376F;
            resources.ApplyResources(this.NewTag1, "NewTag1");
            this.NewTag1.Name = "NewTag1";
            // 
            // TagName2
            // 
            this.TagName2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TagName2.DataPropertyName = "TagName2";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.TagName2.DefaultCellStyle = dataGridViewCellStyle5;
            this.TagName2.FillWeight = 24.52376F;
            resources.ApplyResources(this.TagName2, "TagName2");
            this.TagName2.Name = "TagName2";
            // 
            // OriginalTag2
            // 
            this.OriginalTag2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.OriginalTag2.DataPropertyName = "OriginalTag2";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.OriginalTag2.DefaultCellStyle = dataGridViewCellStyle6;
            this.OriginalTag2.FillWeight = 24.52376F;
            resources.ApplyResources(this.OriginalTag2, "OriginalTag2");
            this.OriginalTag2.Name = "OriginalTag2";
            // 
            // NewTag2
            // 
            this.NewTag2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NewTag2.DataPropertyName = "NewTag2";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.NewTag2.DefaultCellStyle = dataGridViewCellStyle7;
            this.NewTag2.FillWeight = 24.52376F;
            resources.ApplyResources(this.NewTag2, "NewTag2");
            this.NewTag2.Name = "NewTag2";
            // 
            // TagName3
            // 
            this.TagName3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TagName3.DataPropertyName = "TagName3";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.TagName3.DefaultCellStyle = dataGridViewCellStyle8;
            this.TagName3.FillWeight = 24.52376F;
            resources.ApplyResources(this.TagName3, "TagName3");
            this.TagName3.Name = "TagName3";
            // 
            // OriginalTag3
            // 
            this.OriginalTag3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.OriginalTag3.DataPropertyName = "OriginalTag3";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.OriginalTag3.DefaultCellStyle = dataGridViewCellStyle9;
            this.OriginalTag3.FillWeight = 24.52376F;
            resources.ApplyResources(this.OriginalTag3, "OriginalTag3");
            this.OriginalTag3.Name = "OriginalTag3";
            // 
            // NewTag3
            // 
            this.NewTag3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NewTag3.DataPropertyName = "NewTag3";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.NewTag3.DefaultCellStyle = dataGridViewCellStyle10;
            this.NewTag3.FillWeight = 24.52376F;
            resources.ApplyResources(this.NewTag3, "NewTag3");
            this.NewTag3.Name = "NewTag3";
            // 
            // TagName4
            // 
            this.TagName4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TagName4.DataPropertyName = "TagName4";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.TagName4.DefaultCellStyle = dataGridViewCellStyle11;
            this.TagName4.FillWeight = 24.52376F;
            resources.ApplyResources(this.TagName4, "TagName4");
            this.TagName4.Name = "TagName4";
            // 
            // OriginalTag4
            // 
            this.OriginalTag4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.OriginalTag4.DataPropertyName = "OriginalTag4";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.OriginalTag4.DefaultCellStyle = dataGridViewCellStyle12;
            this.OriginalTag4.FillWeight = 24.52376F;
            resources.ApplyResources(this.OriginalTag4, "OriginalTag4");
            this.OriginalTag4.Name = "OriginalTag4";
            // 
            // NewTag4
            // 
            this.NewTag4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NewTag4.DataPropertyName = "NewTag4";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.NewTag4.DefaultCellStyle = dataGridViewCellStyle13;
            this.NewTag4.FillWeight = 24.52376F;
            resources.ApplyResources(this.NewTag4, "NewTag4");
            this.NewTag4.Name = "NewTag4";
            // 
            // TagName5
            // 
            this.TagName5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TagName5.DataPropertyName = "TagName5";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.TagName5.DefaultCellStyle = dataGridViewCellStyle14;
            this.TagName5.FillWeight = 24.52376F;
            resources.ApplyResources(this.TagName5, "TagName5");
            this.TagName5.Name = "TagName5";
            // 
            // OriginalTag5
            // 
            this.OriginalTag5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.OriginalTag5.DataPropertyName = "OriginalTag5";
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.OriginalTag5.DefaultCellStyle = dataGridViewCellStyle15;
            this.OriginalTag5.FillWeight = 24.52376F;
            resources.ApplyResources(this.OriginalTag5, "OriginalTag5");
            this.OriginalTag5.Name = "OriginalTag5";
            // 
            // NewTag5
            // 
            this.NewTag5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NewTag5.DataPropertyName = "NewTag5";
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.NewTag5.DefaultCellStyle = dataGridViewCellStyle16;
            this.NewTag5.FillWeight = 24.52376F;
            resources.ApplyResources(this.NewTag5, "NewTag5");
            this.NewTag5.Name = "NewTag5";
            // 
            // OddEven
            // 
            this.OddEven.DataPropertyName = "OddEven";
            this.OddEven.FillWeight = 1F;
            resources.ApplyResources(this.OddEven, "OddEven");
            this.OddEven.Name = "OddEven";
            // 
            // AdvancedSearchAndReplace
            // 
            this.AcceptButton = this.buttonPreview;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.filtersPanel);
            this.Controls.Add(this.autoApplyPresetsLabel);
            this.DoubleBuffered = true;
            this.Name = "AdvancedSearchAndReplace";
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
        private DataGridViewTextBoxColumn PresetGuid;
        private DataGridViewTextBoxColumn File;
        private DataGridViewTextBoxColumn Track;
        private DataGridViewTextBoxColumn TagName1;
        private DataGridViewTextBoxColumn OriginalTag1;
        private DataGridViewTextBoxColumn NewTag1;
        private DataGridViewTextBoxColumn TagName2;
        private DataGridViewTextBoxColumn OriginalTag2;
        private DataGridViewTextBoxColumn NewTag2;
        private DataGridViewTextBoxColumn TagName3;
        private DataGridViewTextBoxColumn OriginalTag3;
        private DataGridViewTextBoxColumn NewTag3;
        private DataGridViewTextBoxColumn TagName4;
        private DataGridViewTextBoxColumn OriginalTag4;
        private DataGridViewTextBoxColumn NewTag4;
        private DataGridViewTextBoxColumn TagName5;
        private DataGridViewTextBoxColumn OriginalTag5;
        private DataGridViewTextBoxColumn NewTag5;
        private DataGridViewTextBoxColumn OddEven;
    }
}