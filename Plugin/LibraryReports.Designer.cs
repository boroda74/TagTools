﻿using System.Windows.Forms;

namespace MusicBeePlugin
{
    partial class LibraryReports
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

            if (disposing)
            {
                if (periodicCacheClearingTimer != null)
                    periodicCacheClearingTimer.Dispose();

                if (totalsFont != null)
                    totalsFont.Dispose();

                if (warningWide != null)
                    warningWide.Dispose();

                if (errorWide != null)
                    errorWide.Dispose();

                if (fatalErrorWide != null)
                    fatalErrorWide.Dispose();

                if (errorFatalErrorWide != null)
                    errorFatalErrorWide.Dispose();

                if (warning != null)
                    warning.Dispose();

                if (error != null)
                    error.Dispose();

                if (fatalError != null)
                    fatalError.Dispose();

                if (errorFatalError != null)
                    errorFatalError.Dispose();

                if (DefaultArtwork != null)
                    DefaultArtwork.Dispose();

                if (artwork != null)
                    artwork.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LibraryReports));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.presetChainIsWrongPictureBox = new System.Windows.Forms.PictureBox();
            this.smartOperationCheckBoxLabel = new System.Windows.Forms.Label();
            this.smartOperationCheckBox = new System.Windows.Forms.CheckBox();
            this.appendTextBox = new System.Windows.Forms.TextBox();
            this.appendLabel = new System.Windows.Forms.Label();
            this.digitsLabel = new System.Windows.Forms.Label();
            this.precisionDigitsComboBox = new System.Windows.Forms.ComboBox();
            this.roundToLabel = new System.Windows.Forms.Label();
            this.mulDivFactorComboBox = new System.Windows.Forms.ComboBox();
            this.operationComboBox = new System.Windows.Forms.ComboBox();
            this.clearIdButton = new System.Windows.Forms.Button();
            this.idTextBox = new System.Windows.Forms.TextBox();
            this.labelFunctionId = new System.Windows.Forms.Label();
            this.destinationTagList = new System.Windows.Forms.ComboBox();
            this.labelSaveToTag = new System.Windows.Forms.Label();
            this.sourceFieldComboBox = new System.Windows.Forms.ComboBox();
            this.labelSaveField = new System.Windows.Forms.Label();
            this.buttonFilterResults = new System.Windows.Forms.Button();
            this.comparedFieldList = new System.Windows.Forms.ComboBox();
            this.conditionList = new System.Windows.Forms.ComboBox();
            this.conditionFieldList = new System.Windows.Forms.ComboBox();
            this.conditionCheckBoxLabel = new System.Windows.Forms.Label();
            this.conditionCheckBox = new System.Windows.Forms.CheckBox();
            this.buttonExport = new System.Windows.Forms.Button();
            this.openReportCheckBoxPicture = new System.Windows.Forms.PictureBox();
            this.openReportCheckBoxLabel = new System.Windows.Forms.Label();
            this.openReportCheckBox = new System.Windows.Forms.CheckBox();
            this.newArtworkSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.labelRemark = new System.Windows.Forms.Label();
            this.resizeArtworkCheckBoxLabel = new System.Windows.Forms.Label();
            this.resizeArtworkCheckBox = new System.Windows.Forms.CheckBox();
            this.totalsCheckBoxLabel = new System.Windows.Forms.Label();
            this.totalsCheckBox = new System.Windows.Forms.CheckBox();
            this.formatComboBox = new System.Windows.Forms.ComboBox();
            this.labelFormat = new System.Windows.Forms.Label();
            this.useAnotherPresetAsSourceComboBox = new System.Windows.Forms.ComboBox();
            this.useAnotherPresetAsSourceCheckBoxLabel = new System.Windows.Forms.Label();
            this.useAnotherPresetAsSourceCheckBox = new System.Windows.Forms.CheckBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonSaveClose = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.buttonDeletePreset = new System.Windows.Forms.Button();
            this.buttonCopyPreset = new System.Windows.Forms.Button();
            this.buttonAddPreset = new System.Windows.Forms.Button();
            this.useHotkeyForSelectedTracksCheckBoxLabel = new System.Windows.Forms.Label();
            this.useHotkeyForSelectedTracksCheckBox = new System.Windows.Forms.CheckBox();
            this.assignHotkeyCheckBoxLabel = new System.Windows.Forms.Label();
            this.assignHotkeyCheckBox = new System.Windows.Forms.CheckBox();
            this.presetNameTextBox = new System.Windows.Forms.TextBox();
            this.listBoxTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.presetList = new System.Windows.Forms.CheckedListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.presetTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.previewTable = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.multipleItemsSplitterTrimCheckBoxLabel = new System.Windows.Forms.Label();
            this.multipleItemsSplitterTrimCheckBox = new System.Windows.Forms.CheckBox();
            this.expressionsDataGridView = new System.Windows.Forms.DataGridView();
            this.expressionsDataGridViewCheckedColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.expressionsDataGridViewExpressionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagsDataGridView = new System.Windows.Forms.DataGridView();
            this.tagsDataGridViewCheckedColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tagsDataGridViewFunctionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagsDataGridViewTagColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagsDataGridViewInfoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.expressionTextBox = new System.Windows.Forms.TextBox();
            this.expressionLabel = new System.Windows.Forms.Label();
            this.multipleItemsSplitterComboBox = new System.Windows.Forms.ComboBox();
            this.multipleItemsSplitterLabel = new System.Windows.Forms.Label();
            this.parameter2ComboBox = new System.Windows.Forms.ComboBox();
            this.parameter2Label = new System.Windows.Forms.Label();
            this.sourceTagList = new System.Windows.Forms.ComboBox();
            this.forTagLabel = new System.Windows.Forms.Label();
            this.functionComboBox = new System.Windows.Forms.ComboBox();
            this.labelFunction = new System.Windows.Forms.Label();
            this.buttonsPanel = new System.Windows.Forms.Panel();
            this.buttonUpdateFunction = new System.Windows.Forms.Button();
            this.buttonAddFunction = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.buttonClearExpression = new System.Windows.Forms.Button();
            this.autoApplyPresetsLabel = new System.Windows.Forms.Label();
            this.dirtyErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.presetChainIsWrongPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.openReportCheckBoxPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.newArtworkSizeUpDown)).BeginInit();
            this.listBoxTableLayoutPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.presetTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.expressionsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagsDataGridView)).BeginInit();
            this.buttonsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).BeginInit();
            this.SuspendLayout();

            //MusicBee
            this.presetList = new CustomCheckedListBox(Plugin.SavedSettings.dontUseSkinColors);
            this.presetNameTextBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.appendTextBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.idTextBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
            this.expressionTextBox = (System.Windows.Forms.TextBox)Plugin.MbApiInterface.MB_AddPanel(null, Plugin.PluginPanelDock.TextBox);
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
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            this.dirtyErrorProvider.SetIconAlignment(this.splitContainer1.Panel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer1.Panel1.IconAlignment"))));
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.dirtyErrorProvider.SetIconAlignment(this.splitContainer1.Panel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer1.Panel2.IconAlignment"))));
            this.splitContainer1.Tag = "#LibraryReports&LibraryReports@pinned-to-parent-x";
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.listBoxTableLayoutPanel, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.presetChainIsWrongPictureBox);
            this.panel1.Controls.Add(this.smartOperationCheckBoxLabel);
            this.panel1.Controls.Add(this.smartOperationCheckBox);
            this.panel1.Controls.Add(this.appendTextBox);
            this.panel1.Controls.Add(this.appendLabel);
            this.panel1.Controls.Add(this.digitsLabel);
            this.panel1.Controls.Add(this.precisionDigitsComboBox);
            this.panel1.Controls.Add(this.roundToLabel);
            this.panel1.Controls.Add(this.mulDivFactorComboBox);
            this.panel1.Controls.Add(this.operationComboBox);
            this.panel1.Controls.Add(this.clearIdButton);
            this.panel1.Controls.Add(this.idTextBox);
            this.panel1.Controls.Add(this.labelFunctionId);
            this.panel1.Controls.Add(this.destinationTagList);
            this.panel1.Controls.Add(this.labelSaveToTag);
            this.panel1.Controls.Add(this.sourceFieldComboBox);
            this.panel1.Controls.Add(this.labelSaveField);
            this.panel1.Controls.Add(this.buttonFilterResults);
            this.panel1.Controls.Add(this.comparedFieldList);
            this.panel1.Controls.Add(this.conditionList);
            this.panel1.Controls.Add(this.conditionFieldList);
            this.panel1.Controls.Add(this.conditionCheckBoxLabel);
            this.panel1.Controls.Add(this.conditionCheckBox);
            this.panel1.Controls.Add(this.buttonExport);
            this.panel1.Controls.Add(this.openReportCheckBoxPicture);
            this.panel1.Controls.Add(this.openReportCheckBoxLabel);
            this.panel1.Controls.Add(this.openReportCheckBox);
            this.panel1.Controls.Add(this.newArtworkSizeUpDown);
            this.panel1.Controls.Add(this.labelRemark);
            this.panel1.Controls.Add(this.resizeArtworkCheckBoxLabel);
            this.panel1.Controls.Add(this.resizeArtworkCheckBox);
            this.panel1.Controls.Add(this.totalsCheckBoxLabel);
            this.panel1.Controls.Add(this.totalsCheckBox);
            this.panel1.Controls.Add(this.formatComboBox);
            this.panel1.Controls.Add(this.labelFormat);
            this.panel1.Controls.Add(this.useAnotherPresetAsSourceComboBox);
            this.panel1.Controls.Add(this.useAnotherPresetAsSourceCheckBoxLabel);
            this.panel1.Controls.Add(this.useAnotherPresetAsSourceCheckBox);
            this.panel1.Controls.Add(this.buttonClose);
            this.panel1.Controls.Add(this.buttonSaveClose);
            this.panel1.Controls.Add(this.buttonOK);
            this.panel1.Controls.Add(this.buttonPreview);
            this.panel1.Controls.Add(this.buttonSettings);
            this.panel1.Controls.Add(this.buttonDeletePreset);
            this.panel1.Controls.Add(this.buttonCopyPreset);
            this.panel1.Controls.Add(this.buttonAddPreset);
            this.panel1.Controls.Add(this.useHotkeyForSelectedTracksCheckBoxLabel);
            this.panel1.Controls.Add(this.useHotkeyForSelectedTracksCheckBox);
            this.panel1.Controls.Add(this.assignHotkeyCheckBoxLabel);
            this.panel1.Controls.Add(this.assignHotkeyCheckBox);
            this.panel1.Controls.Add(this.presetNameTextBox);
            this.dirtyErrorProvider.SetIconAlignment(this.panel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("panel1.IconAlignment"))));
            this.panel1.Name = "panel1";
            // 
            // presetChainIsWrongPictureBox
            // 
            resources.ApplyResources(this.presetChainIsWrongPictureBox, "presetChainIsWrongPictureBox");
            this.presetChainIsWrongPictureBox.Name = "presetChainIsWrongPictureBox";
            this.presetChainIsWrongPictureBox.TabStop = false;
            this.presetChainIsWrongPictureBox.Tag = "#labelFormat";
            // 
            // smartOperationCheckBoxLabel
            // 
            resources.ApplyResources(this.smartOperationCheckBoxLabel, "smartOperationCheckBoxLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.smartOperationCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("smartOperationCheckBoxLabel.IconAlignment"))));
            this.smartOperationCheckBoxLabel.Name = "smartOperationCheckBoxLabel";
            this.smartOperationCheckBoxLabel.Tag = "@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.smartOperationCheckBoxLabel, resources.GetString("smartOperationCheckBoxLabel.ToolTip"));
            this.smartOperationCheckBoxLabel.Click += new System.EventHandler(this.smartOperationCheckBoxLabel_Click);
            // 
            // smartOperationCheckBox
            // 
            resources.ApplyResources(this.smartOperationCheckBox, "smartOperationCheckBox");
            this.dirtyErrorProvider.SetIconAlignment(this.smartOperationCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("smartOperationCheckBox.IconAlignment"))));
            this.smartOperationCheckBox.Name = "smartOperationCheckBox";
            this.smartOperationCheckBox.Tag = "#smartOperationCheckBoxLabel";
            this.toolTip1.SetToolTip(this.smartOperationCheckBox, resources.GetString("smartOperationCheckBox.ToolTip"));
            // 
            // appendTextBox
            // 
            resources.ApplyResources(this.appendTextBox, "appendTextBox");
            this.dirtyErrorProvider.SetIconAlignment(this.appendTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("appendTextBox.IconAlignment"))));
            this.appendTextBox.Name = "appendTextBox";
            this.appendTextBox.Tag = "#smartOperationCheckBox";
            this.appendTextBox.TextChanged += new System.EventHandler(this.appendTextBox_TextChanged);
            // 
            // appendLabel
            // 
            resources.ApplyResources(this.appendLabel, "appendLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.appendLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("appendLabel.IconAlignment"))));
            this.appendLabel.Name = "appendLabel";
            this.appendLabel.Tag = "#appendTextBox";
            this.toolTip1.SetToolTip(this.appendLabel, resources.GetString("appendLabel.ToolTip"));
            // 
            // digitsLabel
            // 
            resources.ApplyResources(this.digitsLabel, "digitsLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.digitsLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("digitsLabel.IconAlignment"))));
            this.digitsLabel.Name = "digitsLabel";
            this.digitsLabel.Tag = "#appendLabel";
            // 
            // precisionDigitsComboBox
            // 
            this.precisionDigitsComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.precisionDigitsComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("precisionDigitsComboBox.IconAlignment"))));
            this.precisionDigitsComboBox.Items.AddRange(new object[] {
            resources.GetString("precisionDigitsComboBox.Items"),
            resources.GetString("precisionDigitsComboBox.Items1"),
            resources.GetString("precisionDigitsComboBox.Items2"),
            resources.GetString("precisionDigitsComboBox.Items3"),
            resources.GetString("precisionDigitsComboBox.Items4"),
            resources.GetString("precisionDigitsComboBox.Items5")});
            resources.ApplyResources(this.precisionDigitsComboBox, "precisionDigitsComboBox");
            this.precisionDigitsComboBox.Name = "precisionDigitsComboBox";
            this.precisionDigitsComboBox.Tag = "#digitsLabel";
            this.precisionDigitsComboBox.TextChanged += new System.EventHandler(this.precisionDigitsComboBox_TextChanged);
            // 
            // roundToLabel
            // 
            resources.ApplyResources(this.roundToLabel, "roundToLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.roundToLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("roundToLabel.IconAlignment"))));
            this.roundToLabel.Name = "roundToLabel";
            this.roundToLabel.Tag = "#precisionDigitsComboBox";
            // 
            // mulDivFactorComboBox
            // 
            this.mulDivFactorComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.mulDivFactorComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("mulDivFactorComboBox.IconAlignment"))));
            this.mulDivFactorComboBox.Items.AddRange(new object[] {
            resources.GetString("mulDivFactorComboBox.Items"),
            resources.GetString("mulDivFactorComboBox.Items1"),
            resources.GetString("mulDivFactorComboBox.Items2"),
            resources.GetString("mulDivFactorComboBox.Items3"),
            resources.GetString("mulDivFactorComboBox.Items4"),
            resources.GetString("mulDivFactorComboBox.Items5"),
            resources.GetString("mulDivFactorComboBox.Items6")});
            resources.ApplyResources(this.mulDivFactorComboBox, "mulDivFactorComboBox");
            this.mulDivFactorComboBox.Name = "mulDivFactorComboBox";
            this.mulDivFactorComboBox.Tag = "#roundToLabel";
            this.mulDivFactorComboBox.TextChanged += new System.EventHandler(this.mulDivFactorComboBox_TextChanged);
            // 
            // operationComboBox
            // 
            this.operationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.operationComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.operationComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("operationComboBox.IconAlignment"))));
            this.operationComboBox.Items.AddRange(new object[] {
            resources.GetString("operationComboBox.Items"),
            resources.GetString("operationComboBox.Items1")});
            resources.ApplyResources(this.operationComboBox, "operationComboBox");
            this.operationComboBox.Name = "operationComboBox";
            this.operationComboBox.Tag = "#mulDivFactorComboBox@pinned-to-parent-x";
            this.operationComboBox.SelectedIndexChanged += new System.EventHandler(this.operationComboBox_SelectedIndexChanged);
            // 
            // clearIdButton
            // 
            resources.ApplyResources(this.clearIdButton, "clearIdButton");
            this.dirtyErrorProvider.SetIconAlignment(this.clearIdButton, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("clearIdButton.IconAlignment"))));
            this.clearIdButton.Image = global::MusicBeePlugin.Properties.Resources.clear_button_15;
            this.clearIdButton.Name = "clearIdButton";
            this.clearIdButton.Tag = "#splitContainer1@non-defaultable@square-control";
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
            // labelFunctionId
            // 
            resources.ApplyResources(this.labelFunctionId, "labelFunctionId");
            this.dirtyErrorProvider.SetIconAlignment(this.labelFunctionId, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelFunctionId.IconAlignment"))));
            this.labelFunctionId.Name = "labelFunctionId";
            this.labelFunctionId.Tag = "#idTextBox";
            // 
            // destinationTagList
            // 
            this.destinationTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.destinationTagList.DropDownWidth = 250;
            this.destinationTagList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.destinationTagList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("destinationTagList.IconAlignment"))));
            resources.ApplyResources(this.destinationTagList, "destinationTagList");
            this.destinationTagList.Name = "destinationTagList";
            this.destinationTagList.SelectedIndexChanged += new System.EventHandler(this.destinationTagList_SelectedIndexChanged);
            // 
            // labelSaveToTag
            // 
            resources.ApplyResources(this.labelSaveToTag, "labelSaveToTag");
            this.dirtyErrorProvider.SetIconAlignment(this.labelSaveToTag, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelSaveToTag.IconAlignment"))));
            this.labelSaveToTag.Name = "labelSaveToTag";
            this.labelSaveToTag.Tag = "#destinationTagList";
            // 
            // sourceFieldComboBox
            // 
            this.sourceFieldComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceFieldComboBox.DropDownWidth = 600;
            this.sourceFieldComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.sourceFieldComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("sourceFieldComboBox.IconAlignment"))));
            resources.ApplyResources(this.sourceFieldComboBox, "sourceFieldComboBox");
            this.sourceFieldComboBox.Name = "sourceFieldComboBox";
            this.sourceFieldComboBox.Tag = "#labelSaveToTag";
            this.sourceFieldComboBox.SelectedIndexChanged += new System.EventHandler(this.sourceFieldComboBox_SelectedIndexChanged);
            // 
            // labelSaveField
            // 
            resources.ApplyResources(this.labelSaveField, "labelSaveField");
            this.dirtyErrorProvider.SetIconAlignment(this.labelSaveField, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelSaveField.IconAlignment"))));
            this.labelSaveField.Name = "labelSaveField";
            this.labelSaveField.Tag = "#sourceFieldComboBox";
            // 
            // buttonFilterResults
            // 
            resources.ApplyResources(this.buttonFilterResults, "buttonFilterResults");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonFilterResults, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonFilterResults.IconAlignment"))));
            this.buttonFilterResults.Name = "buttonFilterResults";
            this.buttonFilterResults.Tag = "#splitContainer1@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonFilterResults, resources.GetString("buttonFilterResults.ToolTip"));
            this.buttonFilterResults.Click += new System.EventHandler(this.buttonFilterResults_Click);
            // 
            // comparedFieldList
            // 
            resources.ApplyResources(this.comparedFieldList, "comparedFieldList");
            this.comparedFieldList.DropDownWidth = 600;
            this.comparedFieldList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.comparedFieldList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("comparedFieldList.IconAlignment"))));
            this.comparedFieldList.Name = "comparedFieldList";
            this.comparedFieldList.Tag = "#buttonFilterResults";
            this.comparedFieldList.TextChanged += new System.EventHandler(this.comparedFieldList_TextChanged);
            // 
            // conditionList
            // 
            this.conditionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.conditionList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.conditionList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("conditionList.IconAlignment"))));
            resources.ApplyResources(this.conditionList, "conditionList");
            this.conditionList.Name = "conditionList";
            this.conditionList.Tag = "#comparedFieldList";
            this.conditionList.SelectedIndexChanged += new System.EventHandler(this.conditionList_SelectedIndexChanged);
            // 
            // conditionFieldList
            // 
            this.conditionFieldList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.conditionFieldList.DropDownWidth = 600;
            this.conditionFieldList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.conditionFieldList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("conditionFieldList.IconAlignment"))));
            resources.ApplyResources(this.conditionFieldList, "conditionFieldList");
            this.conditionFieldList.Name = "conditionFieldList";
            this.conditionFieldList.Tag = "#conditionList";
            this.conditionFieldList.SelectedIndexChanged += new System.EventHandler(this.conditionFieldList_SelectedIndexChanged);
            // 
            // conditionCheckBoxLabel
            // 
            resources.ApplyResources(this.conditionCheckBoxLabel, "conditionCheckBoxLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.conditionCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("conditionCheckBoxLabel.IconAlignment"))));
            this.conditionCheckBoxLabel.Name = "conditionCheckBoxLabel";
            this.conditionCheckBoxLabel.Tag = "#conditionFieldList";
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
            // buttonExport
            // 
            resources.ApplyResources(this.buttonExport, "buttonExport");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonExport, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonExport.IconAlignment"))));
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Tag = "#splitContainer1@non-defaultable";
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // openReportCheckBoxPicture
            // 
            resources.ApplyResources(this.openReportCheckBoxPicture, "openReportCheckBoxPicture");
            this.dirtyErrorProvider.SetIconAlignment(this.openReportCheckBoxPicture, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("openReportCheckBoxPicture.IconAlignment"))));
            this.openReportCheckBoxPicture.Image = global::MusicBeePlugin.Properties.Resources.window_23;
            this.openReportCheckBoxPicture.Name = "openReportCheckBoxPicture";
            this.openReportCheckBoxPicture.TabStop = false;
            this.openReportCheckBoxPicture.Tag = "@square-control@small-picture";
            this.toolTip1.SetToolTip(this.openReportCheckBoxPicture, resources.GetString("openReportCheckBoxPicture.ToolTip"));
            this.openReportCheckBoxPicture.Click += new System.EventHandler(this.openReportCheckBoxLabel_Click);
            // 
            // openReportCheckBoxLabel
            // 
            resources.ApplyResources(this.openReportCheckBoxLabel, "openReportCheckBoxLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.openReportCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("openReportCheckBoxLabel.IconAlignment"))));
            this.openReportCheckBoxLabel.Name = "openReportCheckBoxLabel";
            this.openReportCheckBoxLabel.Tag = "&openReportCheckBoxPicture";
            this.toolTip1.SetToolTip(this.openReportCheckBoxLabel, resources.GetString("openReportCheckBoxLabel.ToolTip"));
            this.openReportCheckBoxLabel.Click += new System.EventHandler(this.openReportCheckBoxLabel_Click);
            // 
            // openReportCheckBox
            // 
            resources.ApplyResources(this.openReportCheckBox, "openReportCheckBox");
            this.dirtyErrorProvider.SetIconAlignment(this.openReportCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("openReportCheckBox.IconAlignment"))));
            this.openReportCheckBox.Name = "openReportCheckBox";
            this.openReportCheckBox.Tag = "#openReportCheckBoxLabel|openReportCheckBoxPicture";
            this.toolTip1.SetToolTip(this.openReportCheckBox, resources.GetString("openReportCheckBox.ToolTip"));
            // 
            // newArtworkSizeUpDown
            // 
            this.dirtyErrorProvider.SetIconAlignment(this.newArtworkSizeUpDown, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("newArtworkSizeUpDown.IconAlignment"))));
            resources.ApplyResources(this.newArtworkSizeUpDown, "newArtworkSizeUpDown");
            this.newArtworkSizeUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.newArtworkSizeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.newArtworkSizeUpDown.Name = "newArtworkSizeUpDown";
            this.newArtworkSizeUpDown.Tag = "#labelRemark";
            this.newArtworkSizeUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.newArtworkSizeUpDown.ValueChanged += new System.EventHandler(this.newArtworkSizeUpDown_ValueChanged);
            // 
            // labelRemark
            // 
            resources.ApplyResources(this.labelRemark, "labelRemark");
            this.dirtyErrorProvider.SetIconAlignment(this.labelRemark, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelRemark.IconAlignment"))));
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Tag = "";
            // 
            // resizeArtworkCheckBoxLabel
            // 
            resources.ApplyResources(this.resizeArtworkCheckBoxLabel, "resizeArtworkCheckBoxLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.resizeArtworkCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("resizeArtworkCheckBoxLabel.IconAlignment"))));
            this.resizeArtworkCheckBoxLabel.Name = "resizeArtworkCheckBoxLabel";
            this.resizeArtworkCheckBoxLabel.Tag = "#newArtworkSizeUpDown";
            this.resizeArtworkCheckBoxLabel.Click += new System.EventHandler(this.resizeArtworkCheckBoxLabel_Click);
            // 
            // resizeArtworkCheckBox
            // 
            resources.ApplyResources(this.resizeArtworkCheckBox, "resizeArtworkCheckBox");
            this.dirtyErrorProvider.SetIconAlignment(this.resizeArtworkCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("resizeArtworkCheckBox.IconAlignment"))));
            this.resizeArtworkCheckBox.Name = "resizeArtworkCheckBox";
            this.resizeArtworkCheckBox.Tag = "#resizeArtworkCheckBoxLabel";
            this.resizeArtworkCheckBox.CheckedChanged += new System.EventHandler(this.resizeArtworkCheckBox_CheckedChanged);
            // 
            // totalsCheckBoxLabel
            // 
            resources.ApplyResources(this.totalsCheckBoxLabel, "totalsCheckBoxLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.totalsCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("totalsCheckBoxLabel.IconAlignment"))));
            this.totalsCheckBoxLabel.Name = "totalsCheckBoxLabel";
            this.totalsCheckBoxLabel.Tag = "#resizeArtworkCheckBox";
            this.totalsCheckBoxLabel.Click += new System.EventHandler(this.totalsCheckBoxLabel_Click);
            // 
            // totalsCheckBox
            // 
            resources.ApplyResources(this.totalsCheckBox, "totalsCheckBox");
            this.dirtyErrorProvider.SetIconAlignment(this.totalsCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("totalsCheckBox.IconAlignment"))));
            this.totalsCheckBox.Name = "totalsCheckBox";
            this.totalsCheckBox.Tag = "#totalsCheckBoxLabel";
            this.totalsCheckBox.CheckedChanged += new System.EventHandler(this.totalsCheckBox_CheckedChanged);
            // 
            // formatComboBox
            // 
            resources.ApplyResources(this.formatComboBox, "formatComboBox");
            this.formatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.formatComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.formatComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("formatComboBox.IconAlignment"))));
            this.formatComboBox.Name = "formatComboBox";
            this.formatComboBox.Tag = "#splitContainer1";
            this.toolTip1.SetToolTip(this.formatComboBox, resources.GetString("formatComboBox.ToolTip"));
            this.formatComboBox.SelectedIndexChanged += new System.EventHandler(this.formatComboBox_SelectedIndexChanged);
            // 
            // labelFormat
            // 
            resources.ApplyResources(this.labelFormat, "labelFormat");
            this.dirtyErrorProvider.SetIconAlignment(this.labelFormat, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelFormat.IconAlignment"))));
            this.labelFormat.Name = "labelFormat";
            this.labelFormat.Tag = "#formatComboBox";
            // 
            // useAnotherPresetAsSourceComboBox
            // 
            resources.ApplyResources(this.useAnotherPresetAsSourceComboBox, "useAnotherPresetAsSourceComboBox");
            this.useAnotherPresetAsSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.useAnotherPresetAsSourceComboBox.DropDownWidth = 600;
            this.useAnotherPresetAsSourceComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.useAnotherPresetAsSourceComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("useAnotherPresetAsSourceComboBox.IconAlignment"))));
            this.useAnotherPresetAsSourceComboBox.Name = "useAnotherPresetAsSourceComboBox";
            this.useAnotherPresetAsSourceComboBox.Tag = "#presetChainIsWrongPictureBox";
            this.toolTip1.SetToolTip(this.useAnotherPresetAsSourceComboBox, resources.GetString("useAnotherPresetAsSourceComboBox.ToolTip"));
            this.useAnotherPresetAsSourceComboBox.SelectedIndexChanged += new System.EventHandler(this.useAnotherPresetAsSourceComboBox_SelectedIndexChanged);
            // 
            // useAnotherPresetAsSourceCheckBoxLabel
            // 
            resources.ApplyResources(this.useAnotherPresetAsSourceCheckBoxLabel, "useAnotherPresetAsSourceCheckBoxLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.useAnotherPresetAsSourceCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("useAnotherPresetAsSourceCheckBoxLabel.IconAlignment"))));
            this.useAnotherPresetAsSourceCheckBoxLabel.Name = "useAnotherPresetAsSourceCheckBoxLabel";
            this.useAnotherPresetAsSourceCheckBoxLabel.Tag = "#useAnotherPresetAsSourceComboBox";
            this.useAnotherPresetAsSourceCheckBoxLabel.Click += new System.EventHandler(this.useAnotherPresetAsSourceCheckBoxLabel_Click);
            // 
            // useAnotherPresetAsSourceCheckBox
            // 
            resources.ApplyResources(this.useAnotherPresetAsSourceCheckBox, "useAnotherPresetAsSourceCheckBox");
            this.dirtyErrorProvider.SetIconAlignment(this.useAnotherPresetAsSourceCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("useAnotherPresetAsSourceCheckBox.IconAlignment"))));
            this.useAnotherPresetAsSourceCheckBox.Name = "useAnotherPresetAsSourceCheckBox";
            this.useAnotherPresetAsSourceCheckBox.Tag = "#useAnotherPresetAsSourceCheckBoxLabel";
            this.toolTip1.SetToolTip(this.useAnotherPresetAsSourceCheckBox, resources.GetString("useAnotherPresetAsSourceCheckBox.ToolTip"));
            this.useAnotherPresetAsSourceCheckBox.CheckedChanged += new System.EventHandler(this.useAnotherPresetAsSourceCheckBox_CheckedChanged);
            // 
            // buttonClose
            // 
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dirtyErrorProvider.SetIconAlignment(this.buttonClose, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonClose.IconAlignment"))));
            this.buttonClose.Image = global::MusicBeePlugin.Properties.Resources.warning_wide_15;
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Tag = "#splitContainer1@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonClose, resources.GetString("buttonClose.ToolTip"));
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonSaveClose
            // 
            resources.ApplyResources(this.buttonSaveClose, "buttonSaveClose");
            this.buttonSaveClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dirtyErrorProvider.SetIconAlignment(this.buttonSaveClose, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonSaveClose.IconAlignment"))));
            this.buttonSaveClose.Name = "buttonSaveClose";
            this.buttonSaveClose.Tag = "#buttonClose";
            this.toolTip1.SetToolTip(this.buttonSaveClose, resources.GetString("buttonSaveClose.ToolTip"));
            this.buttonSaveClose.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonOK, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonOK.IconAlignment"))));
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Tag = "#buttonSaveClose";
            this.toolTip1.SetToolTip(this.buttonOK, resources.GetString("buttonOK.ToolTip"));
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonPreview
            // 
            resources.ApplyResources(this.buttonPreview, "buttonPreview");
            this.dirtyErrorProvider.SetError(this.buttonPreview, resources.GetString("buttonPreview.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonPreview, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonPreview.IconAlignment"))));
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Tag = "#buttonOK";
            this.toolTip1.SetToolTip(this.buttonPreview, resources.GetString("buttonPreview.ToolTip"));
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
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
            // buttonDeletePreset
            // 
            this.dirtyErrorProvider.SetIconAlignment(this.buttonDeletePreset, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonDeletePreset.IconAlignment"))));
            resources.ApplyResources(this.buttonDeletePreset, "buttonDeletePreset");
            this.buttonDeletePreset.Name = "buttonDeletePreset";
            this.buttonDeletePreset.Click += new System.EventHandler(this.buttonDeletePreset_Click);
            // 
            // buttonCopyPreset
            // 
            this.dirtyErrorProvider.SetIconAlignment(this.buttonCopyPreset, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonCopyPreset.IconAlignment"))));
            resources.ApplyResources(this.buttonCopyPreset, "buttonCopyPreset");
            this.buttonCopyPreset.Name = "buttonCopyPreset";
            this.buttonCopyPreset.Tag = "#buttonDeletePreset";
            this.buttonCopyPreset.Click += new System.EventHandler(this.buttonCopyPreset_Click);
            // 
            // buttonAddPreset
            // 
            this.dirtyErrorProvider.SetIconAlignment(this.buttonAddPreset, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonAddPreset.IconAlignment"))));
            resources.ApplyResources(this.buttonAddPreset, "buttonAddPreset");
            this.buttonAddPreset.Name = "buttonAddPreset";
            this.buttonAddPreset.Tag = "#buttonCopyPreset@pinned-to-parent-x@non-defaultable";
            this.buttonAddPreset.Click += new System.EventHandler(this.buttonAddPreset_Click);
            // 
            // useHotkeyForSelectedTracksCheckBoxLabel
            // 
            resources.ApplyResources(this.useHotkeyForSelectedTracksCheckBoxLabel, "useHotkeyForSelectedTracksCheckBoxLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.useHotkeyForSelectedTracksCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("useHotkeyForSelectedTracksCheckBoxLabel.IconAlignment"))));
            this.useHotkeyForSelectedTracksCheckBoxLabel.Name = "useHotkeyForSelectedTracksCheckBoxLabel";
            this.useHotkeyForSelectedTracksCheckBoxLabel.Tag = "@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.useHotkeyForSelectedTracksCheckBoxLabel, resources.GetString("useHotkeyForSelectedTracksCheckBoxLabel.ToolTip"));
            this.useHotkeyForSelectedTracksCheckBoxLabel.Click += new System.EventHandler(this.useHotkeyForSelectedTracksCheckBoxLabel_Click);
            // 
            // useHotkeyForSelectedTracksCheckBox
            // 
            resources.ApplyResources(this.useHotkeyForSelectedTracksCheckBox, "useHotkeyForSelectedTracksCheckBox");
            this.dirtyErrorProvider.SetIconAlignment(this.useHotkeyForSelectedTracksCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("useHotkeyForSelectedTracksCheckBox.IconAlignment"))));
            this.useHotkeyForSelectedTracksCheckBox.Name = "useHotkeyForSelectedTracksCheckBox";
            this.useHotkeyForSelectedTracksCheckBox.Tag = "#useHotkeyForSelectedTracksCheckBoxLabel";
            this.toolTip1.SetToolTip(this.useHotkeyForSelectedTracksCheckBox, resources.GetString("useHotkeyForSelectedTracksCheckBox.ToolTip"));
            this.useHotkeyForSelectedTracksCheckBox.CheckedChanged += new System.EventHandler(this.useHotkeyForSelectedTracksCheckBox_CheckedChanged);
            // 
            // assignHotkeyCheckBoxLabel
            // 
            resources.ApplyResources(this.assignHotkeyCheckBoxLabel, "assignHotkeyCheckBoxLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.assignHotkeyCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("assignHotkeyCheckBoxLabel.IconAlignment"))));
            this.assignHotkeyCheckBoxLabel.Name = "assignHotkeyCheckBoxLabel";
            this.assignHotkeyCheckBoxLabel.Tag = "#useHotkeyForSelectedTracksCheckBox";
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
            // presetNameTextBox
            // 
            this.dirtyErrorProvider.SetIconAlignment(this.presetNameTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("presetNameTextBox.IconAlignment"))));
            resources.ApplyResources(this.presetNameTextBox, "presetNameTextBox");
            this.presetNameTextBox.Name = "presetNameTextBox";
            this.toolTip1.SetToolTip(this.presetNameTextBox, resources.GetString("presetNameTextBox.ToolTip"));
            this.presetNameTextBox.Leave += new System.EventHandler(this.presetNameTextBox_Leave);
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
            this.presetList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.presetList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("presetList.IconAlignment"))));
            this.presetList.Name = "presetList";
            this.presetList.Tag = "";
            this.presetList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.presetList_ItemCheck);
            this.presetList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.presetList_MouseClick);
            this.presetList.SelectedIndexChanged += new System.EventHandler(this.presetList_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.presetTabControl);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // presetTabControl
            // 
            resources.ApplyResources(this.presetTabControl, "presetTabControl");
            this.presetTabControl.Controls.Add(this.tabPage1);
            this.presetTabControl.Controls.Add(this.tabPage2);
            this.dirtyErrorProvider.SetIconAlignment(this.presetTabControl, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("presetTabControl.IconAlignment"))));
            this.presetTabControl.Name = "presetTabControl";
            this.presetTabControl.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.previewTable);
            this.dirtyErrorProvider.SetIconAlignment(this.tabPage1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tabPage1.IconAlignment"))));
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            // 
            // previewTable
            // 
            this.previewTable.AllowUserToAddRows = false;
            this.previewTable.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.previewTable, "previewTable");
            this.previewTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.previewTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.previewTable.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.previewTable.DefaultCellStyle = dataGridViewCellStyle1;
            this.previewTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dirtyErrorProvider.SetIconAlignment(this.previewTable, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("previewTable.IconAlignment"))));
            this.previewTable.MultiSelect = false;
            this.previewTable.Name = "previewTable";
            this.previewTable.RowHeadersVisible = false;
            this.previewTable.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.previewTable.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.previewTable_CellClick);
            this.previewTable.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.previewTable_ColumnHeaderMouseClick);
            this.previewTable.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.previewTable_ColumnWidthChanged);
            this.previewTable.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.previewTable_DataError);
            this.previewTable.RowHeightChanged += new System.Windows.Forms.DataGridViewRowEventHandler(this.previewTable_RowHeightChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.multipleItemsSplitterTrimCheckBoxLabel);
            this.tabPage2.Controls.Add(this.multipleItemsSplitterTrimCheckBox);
            this.tabPage2.Controls.Add(this.expressionsDataGridView);
            this.tabPage2.Controls.Add(this.tagsDataGridView);
            this.tabPage2.Controls.Add(this.expressionTextBox);
            this.tabPage2.Controls.Add(this.expressionLabel);
            this.tabPage2.Controls.Add(this.multipleItemsSplitterComboBox);
            this.tabPage2.Controls.Add(this.multipleItemsSplitterLabel);
            this.tabPage2.Controls.Add(this.parameter2ComboBox);
            this.tabPage2.Controls.Add(this.parameter2Label);
            this.tabPage2.Controls.Add(this.sourceTagList);
            this.tabPage2.Controls.Add(this.forTagLabel);
            this.tabPage2.Controls.Add(this.functionComboBox);
            this.tabPage2.Controls.Add(this.labelFunction);
            this.tabPage2.Controls.Add(this.buttonsPanel);
            this.dirtyErrorProvider.SetIconAlignment(this.tabPage2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tabPage2.IconAlignment"))));
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            // 
            // multipleItemsSplitterTrimCheckBoxLabel
            // 
            resources.ApplyResources(this.multipleItemsSplitterTrimCheckBoxLabel, "multipleItemsSplitterTrimCheckBoxLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.multipleItemsSplitterTrimCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("multipleItemsSplitterTrimCheckBoxLabel.IconAlignment"))));
            this.multipleItemsSplitterTrimCheckBoxLabel.Name = "multipleItemsSplitterTrimCheckBoxLabel";
            this.toolTip1.SetToolTip(this.multipleItemsSplitterTrimCheckBoxLabel, resources.GetString("multipleItemsSplitterTrimCheckBoxLabel.ToolTip"));
            this.multipleItemsSplitterTrimCheckBoxLabel.Click += new System.EventHandler(this.multipleItemsSplitterTrimCheckBoxLabel_Click);
            // 
            // multipleItemsSplitterTrimCheckBox
            // 
            resources.ApplyResources(this.multipleItemsSplitterTrimCheckBox, "multipleItemsSplitterTrimCheckBox");
            this.dirtyErrorProvider.SetIconAlignment(this.multipleItemsSplitterTrimCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("multipleItemsSplitterTrimCheckBox.IconAlignment"))));
            this.multipleItemsSplitterTrimCheckBox.Name = "multipleItemsSplitterTrimCheckBox";
            this.multipleItemsSplitterTrimCheckBox.Tag = "#multipleItemsSplitterTrimCheckBoxLabel";
            this.toolTip1.SetToolTip(this.multipleItemsSplitterTrimCheckBox, resources.GetString("multipleItemsSplitterTrimCheckBox.ToolTip"));
            this.multipleItemsSplitterTrimCheckBox.UseVisualStyleBackColor = false;
            this.multipleItemsSplitterTrimCheckBox.CheckedChanged += new System.EventHandler(this.multipleItemsSplitterTrimCheckBox_CheckedChanged);
            // 
            // expressionsDataGridView
            // 
            this.expressionsDataGridView.AllowUserToAddRows = false;
            resources.ApplyResources(this.expressionsDataGridView, "expressionsDataGridView");
            this.expressionsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.expressionsDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.expressionsDataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.expressionsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.expressionsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.expressionsDataGridViewCheckedColumn,
            this.expressionsDataGridViewExpressionColumn});
            this.expressionsDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dirtyErrorProvider.SetIconAlignment(this.expressionsDataGridView, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("expressionsDataGridView.IconAlignment"))));
            this.expressionsDataGridView.MultiSelect = false;
            this.expressionsDataGridView.Name = "expressionsDataGridView";
            this.expressionsDataGridView.RowHeadersVisible = false;
            this.expressionsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.expressionsDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.expressionsDataGridView_CellClick);
            this.expressionsDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.expressionsDataGridView_CellContentClick);
            // 
            // expressionsDataGridViewCheckedColumn
            // 
            this.expressionsDataGridViewCheckedColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.expressionsDataGridViewCheckedColumn.FalseValue = "F";
            resources.ApplyResources(this.expressionsDataGridViewCheckedColumn, "expressionsDataGridViewCheckedColumn");
            this.expressionsDataGridViewCheckedColumn.IndeterminateValue = "I";
            this.expressionsDataGridViewCheckedColumn.Name = "expressionsDataGridViewCheckedColumn";
            this.expressionsDataGridViewCheckedColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.expressionsDataGridViewCheckedColumn.TrueValue = "T";
            // 
            // expressionsDataGridViewExpressionColumn
            // 
            this.expressionsDataGridViewExpressionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.expressionsDataGridViewExpressionColumn, "expressionsDataGridViewExpressionColumn");
            this.expressionsDataGridViewExpressionColumn.Name = "expressionsDataGridViewExpressionColumn";
            // 
            // tagsDataGridView
            // 
            this.tagsDataGridView.AllowUserToAddRows = false;
            this.tagsDataGridView.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.tagsDataGridView, "tagsDataGridView");
            this.tagsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tagsDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.tagsDataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.tagsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tagsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tagsDataGridViewCheckedColumn,
            this.tagsDataGridViewFunctionColumn,
            this.tagsDataGridViewTagColumn,
            this.tagsDataGridViewInfoColumn});
            this.tagsDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dirtyErrorProvider.SetIconAlignment(this.tagsDataGridView, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tagsDataGridView.IconAlignment"))));
            this.tagsDataGridView.MultiSelect = false;
            this.tagsDataGridView.Name = "tagsDataGridView";
            this.tagsDataGridView.RowHeadersVisible = false;
            this.tagsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tagsDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tagsDataGridView_CellClick);
            this.tagsDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tagsDataGridView_CellContentClick);
            // 
            // tagsDataGridViewCheckedColumn
            // 
            this.tagsDataGridViewCheckedColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.tagsDataGridViewCheckedColumn.FalseValue = "F";
            resources.ApplyResources(this.tagsDataGridViewCheckedColumn, "tagsDataGridViewCheckedColumn");
            this.tagsDataGridViewCheckedColumn.IndeterminateValue = "I";
            this.tagsDataGridViewCheckedColumn.Name = "tagsDataGridViewCheckedColumn";
            this.tagsDataGridViewCheckedColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.tagsDataGridViewCheckedColumn.TrueValue = "T";
            // 
            // tagsDataGridViewFunctionColumn
            // 
            this.tagsDataGridViewFunctionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.tagsDataGridViewFunctionColumn.FillWeight = 50F;
            resources.ApplyResources(this.tagsDataGridViewFunctionColumn, "tagsDataGridViewFunctionColumn");
            this.tagsDataGridViewFunctionColumn.Name = "tagsDataGridViewFunctionColumn";
            this.tagsDataGridViewFunctionColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.tagsDataGridViewFunctionColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tagsDataGridViewTagColumn
            // 
            this.tagsDataGridViewTagColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.tagsDataGridViewTagColumn.FillWeight = 150F;
            resources.ApplyResources(this.tagsDataGridViewTagColumn, "tagsDataGridViewTagColumn");
            this.tagsDataGridViewTagColumn.Name = "tagsDataGridViewTagColumn";
            this.tagsDataGridViewTagColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tagsDataGridViewInfoColumn
            // 
            this.tagsDataGridViewInfoColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.tagsDataGridViewInfoColumn.FillWeight = 70F;
            resources.ApplyResources(this.tagsDataGridViewInfoColumn, "tagsDataGridViewInfoColumn");
            this.tagsDataGridViewInfoColumn.Name = "tagsDataGridViewInfoColumn";
            this.tagsDataGridViewInfoColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // expressionTextBox
            // 
            resources.ApplyResources(this.expressionTextBox, "expressionTextBox");
            this.dirtyErrorProvider.SetIconAlignment(this.expressionTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("expressionTextBox.IconAlignment"))));
            this.expressionTextBox.Name = "expressionTextBox";
            this.expressionTextBox.Tag = "#buttonsPanel";
            this.toolTip1.SetToolTip(this.expressionTextBox, resources.GetString("expressionTextBox.ToolTip"));
            this.expressionTextBox.TextChanged += new System.EventHandler(this.expressionTextBox_TextChanged);
            // 
            // expressionLabel
            // 
            resources.ApplyResources(this.expressionLabel, "expressionLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.expressionLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("expressionLabel.IconAlignment"))));
            this.expressionLabel.Name = "expressionLabel";
            this.expressionLabel.Tag = "#expressionTextBox";
            this.toolTip1.SetToolTip(this.expressionLabel, resources.GetString("expressionLabel.ToolTip"));
            // 
            // multipleItemsSplitterComboBox
            // 
            this.multipleItemsSplitterComboBox.DropDownWidth = 390;
            this.dirtyErrorProvider.SetIconAlignment(this.multipleItemsSplitterComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("multipleItemsSplitterComboBox.IconAlignment"))));
            this.multipleItemsSplitterComboBox.Items.AddRange(new object[] {
            resources.GetString("multipleItemsSplitterComboBox.Items"),
            resources.GetString("multipleItemsSplitterComboBox.Items1"),
            resources.GetString("multipleItemsSplitterComboBox.Items2"),
            resources.GetString("multipleItemsSplitterComboBox.Items3")});
            resources.ApplyResources(this.multipleItemsSplitterComboBox, "multipleItemsSplitterComboBox");
            this.multipleItemsSplitterComboBox.Name = "multipleItemsSplitterComboBox";
            this.multipleItemsSplitterComboBox.Tag = "#multipleItemsSplitterTrimCheckBox";
            this.toolTip1.SetToolTip(this.multipleItemsSplitterComboBox, resources.GetString("multipleItemsSplitterComboBox.ToolTip"));
            this.multipleItemsSplitterComboBox.SelectedIndexChanged += new System.EventHandler(this.multipleItemsSplitterComboBox_SelectedIndexChanged);
            this.multipleItemsSplitterComboBox.TextUpdate += new System.EventHandler(this.multipleItemsSplitterComboBox_TextUpdate);
            // 
            // multipleItemsSplitterLabel
            // 
            resources.ApplyResources(this.multipleItemsSplitterLabel, "multipleItemsSplitterLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.multipleItemsSplitterLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("multipleItemsSplitterLabel.IconAlignment"))));
            this.multipleItemsSplitterLabel.Name = "multipleItemsSplitterLabel";
            this.multipleItemsSplitterLabel.Tag = "#multipleItemsSplitterComboBox";
            this.toolTip1.SetToolTip(this.multipleItemsSplitterLabel, resources.GetString("multipleItemsSplitterLabel.ToolTip"));
            // 
            // parameter2ComboBox
            // 
            this.parameter2ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameter2ComboBox.DropDownWidth = 250;
            this.parameter2ComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.parameter2ComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("parameter2ComboBox.IconAlignment"))));
            resources.ApplyResources(this.parameter2ComboBox, "parameter2ComboBox");
            this.parameter2ComboBox.Name = "parameter2ComboBox";
            this.parameter2ComboBox.SelectedIndexChanged += new System.EventHandler(this.parameter2ComboBox_SelectedIndexChanged);
            // 
            // parameter2Label
            // 
            resources.ApplyResources(this.parameter2Label, "parameter2Label");
            this.dirtyErrorProvider.SetIconAlignment(this.parameter2Label, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("parameter2Label.IconAlignment"))));
            this.parameter2Label.Name = "parameter2Label";
            this.parameter2Label.Tag = "#parameter2ComboBox";
            // 
            // sourceTagList
            // 
            this.sourceTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceTagList.DropDownWidth = 250;
            this.sourceTagList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.sourceTagList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("sourceTagList.IconAlignment"))));
            resources.ApplyResources(this.sourceTagList, "sourceTagList");
            this.sourceTagList.Name = "sourceTagList";
            this.sourceTagList.Tag = "#parameter2Label";
            this.sourceTagList.SelectedIndexChanged += new System.EventHandler(this.sourceTagList_SelectedIndexChanged);
            // 
            // forTagLabel
            // 
            resources.ApplyResources(this.forTagLabel, "forTagLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.forTagLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("forTagLabel.IconAlignment"))));
            this.forTagLabel.Name = "forTagLabel";
            this.forTagLabel.Tag = "#sourceTagList";
            // 
            // functionComboBox
            // 
            this.functionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.functionComboBox.DropDownWidth = 110;
            this.dirtyErrorProvider.SetIconAlignment(this.functionComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("functionComboBox.IconAlignment"))));
            resources.ApplyResources(this.functionComboBox, "functionComboBox");
            this.functionComboBox.Name = "functionComboBox";
            this.functionComboBox.Tag = "#forTagLabel";
            this.functionComboBox.SelectedIndexChanged += new System.EventHandler(this.functionComboBox_SelectedIndexChanged);
            // 
            // labelFunction
            // 
            resources.ApplyResources(this.labelFunction, "labelFunction");
            this.dirtyErrorProvider.SetIconAlignment(this.labelFunction, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelFunction.IconAlignment"))));
            this.labelFunction.Name = "labelFunction";
            this.labelFunction.Tag = "#functionComboBox";
            // 
            // buttonsPanel
            // 
            resources.ApplyResources(this.buttonsPanel, "buttonsPanel");
            this.buttonsPanel.Controls.Add(this.buttonUpdateFunction);
            this.buttonsPanel.Controls.Add(this.buttonAddFunction);
            this.buttonsPanel.Controls.Add(this.buttonHelp);
            this.buttonsPanel.Controls.Add(this.buttonClearExpression);
            this.buttonsPanel.Name = "buttonsPanel";
            this.buttonsPanel.Tag = "#tabPage2";
            // 
            // buttonUpdateFunction
            // 
            resources.ApplyResources(this.buttonUpdateFunction, "buttonUpdateFunction");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonUpdateFunction, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonUpdateFunction.IconAlignment"))));
            this.buttonUpdateFunction.Image = global::MusicBeePlugin.Properties.Resources.warning_wide_15;
            this.buttonUpdateFunction.Name = "buttonUpdateFunction";
            this.buttonUpdateFunction.Tag = "#buttonsPanel@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonUpdateFunction, resources.GetString("buttonUpdateFunction.ToolTip"));
            this.buttonUpdateFunction.Click += new System.EventHandler(this.buttonUpdateFunction_Click);
            // 
            // buttonAddFunction
            // 
            resources.ApplyResources(this.buttonAddFunction, "buttonAddFunction");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonAddFunction, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonAddFunction.IconAlignment"))));
            this.buttonAddFunction.Name = "buttonAddFunction";
            this.buttonAddFunction.Tag = "#buttonUpdateFunction@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonAddFunction, resources.GetString("buttonAddFunction.ToolTip"));
            this.buttonAddFunction.Click += new System.EventHandler(this.buttonAddFunction_Click);
            // 
            // buttonHelp
            // 
            this.dirtyErrorProvider.SetIconAlignment(this.buttonHelp, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonHelp.IconAlignment"))));
            resources.ApplyResources(this.buttonHelp, "buttonHelp");
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Tag = "@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonHelp, resources.GetString("buttonHelp.ToolTip"));
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // buttonClearExpression
            // 
            this.dirtyErrorProvider.SetIconAlignment(this.buttonClearExpression, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonClearExpression.IconAlignment"))));
            this.buttonClearExpression.Image = global::MusicBeePlugin.Properties.Resources.clear_button_15;
            resources.ApplyResources(this.buttonClearExpression, "buttonClearExpression");
            this.buttonClearExpression.Name = "buttonClearExpression";
            this.buttonClearExpression.Tag = "@pinned-to-parent-x@non-defaultable@square-control";
            this.toolTip1.SetToolTip(this.buttonClearExpression, resources.GetString("buttonClearExpression.ToolTip"));
            this.buttonClearExpression.Click += new System.EventHandler(this.buttonClearExpression_Click);
            // 
            // autoApplyPresetsLabel
            // 
            resources.ApplyResources(this.autoApplyPresetsLabel, "autoApplyPresetsLabel");
            this.autoApplyPresetsLabel.AutoEllipsis = true;
            this.dirtyErrorProvider.SetIconAlignment(this.autoApplyPresetsLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("autoApplyPresetsLabel.IconAlignment"))));
            this.autoApplyPresetsLabel.Name = "autoApplyPresetsLabel";
            this.autoApplyPresetsLabel.Tag = "#LibraryReports&splitContainer1@pinned-to-parent-x";
            // 
            // dirtyErrorProvider
            // 
            this.dirtyErrorProvider.BlinkRate = 1000;
            this.dirtyErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
            this.dirtyErrorProvider.ContainerControl = this;
            resources.ApplyResources(this.dirtyErrorProvider, "dirtyErrorProvider");
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 7000;
            this.toolTip1.InitialDelay = 340;
            this.toolTip1.ReshowDelay = 68;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewCheckBoxColumn1.FalseValue = "F";
            resources.ApplyResources(this.dataGridViewCheckBoxColumn1, "dataGridViewCheckBoxColumn1");
            this.dataGridViewCheckBoxColumn1.IndeterminateValue = "I";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumn1.TrueValue = "T";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewCheckBoxColumn2
            // 
            this.dataGridViewCheckBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewCheckBoxColumn2.FalseValue = "F";
            resources.ApplyResources(this.dataGridViewCheckBoxColumn2, "dataGridViewCheckBoxColumn2");
            this.dataGridViewCheckBoxColumn2.IndeterminateValue = "I";
            this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
            this.dataGridViewCheckBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumn2.TrueValue = "T";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn2.FillWeight = 50F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.FillWeight = 150F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.FillWeight = 70F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // LibraryReports
            // 
            this.AcceptButton = this.buttonPreview;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.autoApplyPresetsLabel);
            this.Name = "LibraryReports";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LibraryReports_FormClosing);
            this.Load += new System.EventHandler(this.LibraryReports_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.presetChainIsWrongPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.openReportCheckBoxPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.newArtworkSizeUpDown)).EndInit();
            this.listBoxTableLayoutPanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.presetTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.expressionsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagsDataGridView)).EndInit();
            this.buttonsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Label autoApplyPresetsLabel;
        private System.Windows.Forms.CheckBox conditionCheckBox;
        private System.Windows.Forms.ComboBox conditionFieldList;
        private System.Windows.Forms.ComboBox conditionList;
        private System.Windows.Forms.Label labelSaveField;
        private System.Windows.Forms.ComboBox destinationTagList;
        private System.Windows.Forms.ErrorProvider dirtyErrorProvider;
        private System.Windows.Forms.ComboBox comparedFieldList;
        private System.Windows.Forms.ComboBox sourceFieldComboBox;
        private System.Windows.Forms.Label labelSaveToTag;
        private System.Windows.Forms.ComboBox functionComboBox;
        private System.Windows.Forms.Label labelFunction;
        private System.Windows.Forms.ComboBox parameter2ComboBox;
        private System.Windows.Forms.Label labelRemark;
        private System.Windows.Forms.NumericUpDown newArtworkSizeUpDown;
        private System.Windows.Forms.CheckBox resizeArtworkCheckBox;
        private System.Windows.Forms.CheckBox openReportCheckBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox formatComboBox;
        private System.Windows.Forms.Label labelFormat;
        private System.Windows.Forms.CheckedListBox presetList;
        private System.Windows.Forms.Button buttonCopyPreset;
        private System.Windows.Forms.TextBox presetNameTextBox;
        private System.Windows.Forms.Button buttonDeletePreset;
        private System.Windows.Forms.Button buttonAddPreset;
        private System.Windows.Forms.CheckBox assignHotkeyCheckBox;
        private System.Windows.Forms.Button buttonSaveClose;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button clearIdButton;
        private System.Windows.Forms.TextBox idTextBox;
        private System.Windows.Forms.Label labelFunctionId;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView previewTable;
        private System.Windows.Forms.ComboBox operationComboBox;
        private System.Windows.Forms.ComboBox mulDivFactorComboBox;
        private System.Windows.Forms.Label roundToLabel;
        private System.Windows.Forms.ComboBox precisionDigitsComboBox;
        private System.Windows.Forms.Label digitsLabel;
        private System.Windows.Forms.TextBox appendTextBox;
        private System.Windows.Forms.Label appendLabel;
        private System.Windows.Forms.Button buttonFilterResults;
        private System.Windows.Forms.ComboBox useAnotherPresetAsSourceComboBox;
        private System.Windows.Forms.CheckBox useAnotherPresetAsSourceCheckBox;
        private System.Windows.Forms.Label parameter2Label;
        private System.Windows.Forms.Label multipleItemsSplitterLabel;
        private System.Windows.Forms.ComboBox multipleItemsSplitterComboBox;
        private System.Windows.Forms.Label expressionLabel;
        private System.Windows.Forms.CheckBox multipleItemsSplitterTrimCheckBox;
        private System.Windows.Forms.TabControl presetTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ComboBox sourceTagList;
        private System.Windows.Forms.Label forTagLabel;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.Button buttonUpdateFunction;
        private System.Windows.Forms.Button buttonAddFunction;
        private System.Windows.Forms.DataGridView tagsDataGridView;
        private System.Windows.Forms.DataGridView expressionsDataGridView;
        private System.Windows.Forms.TextBox expressionTextBox;
        private System.Windows.Forms.Button buttonClearExpression;
        private System.Windows.Forms.Label assignHotkeyCheckBoxLabel;
        private System.Windows.Forms.Label useHotkeyForSelectedTracksCheckBoxLabel;
        private System.Windows.Forms.Label useAnotherPresetAsSourceCheckBoxLabel;
        private System.Windows.Forms.CheckBox useHotkeyForSelectedTracksCheckBox;
        private System.Windows.Forms.Label resizeArtworkCheckBoxLabel;
        private System.Windows.Forms.Label totalsCheckBoxLabel;
        private System.Windows.Forms.Label openReportCheckBoxLabel;
        private System.Windows.Forms.CheckBox totalsCheckBox;
        private System.Windows.Forms.Label conditionCheckBoxLabel;
        private System.Windows.Forms.Label smartOperationCheckBoxLabel;
        private System.Windows.Forms.CheckBox smartOperationCheckBox;
        private System.Windows.Forms.DataGridViewCheckBoxColumn expressionsDataGridViewCheckedColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn expressionsDataGridViewExpressionColumn;
        private System.Windows.Forms.Label multipleItemsSplitterTrimCheckBoxLabel;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.PictureBox openReportCheckBoxPicture;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel buttonsPanel;
        private System.Windows.Forms.DataGridViewCheckBoxColumn tagsDataGridViewCheckedColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagsDataGridViewFunctionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagsDataGridViewTagColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagsDataGridViewInfoColumn;
        private System.Windows.Forms.TableLayoutPanel listBoxTableLayoutPanel;
        private System.Windows.Forms.PictureBox presetChainIsWrongPictureBox;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    }
}