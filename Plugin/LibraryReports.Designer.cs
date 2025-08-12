using System.Windows.Forms;

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

            base.Dispose(disposing);

            if (disposing)
            {
                periodicCacheClearingTimer?.Dispose();
                source?.Dispose();

                totalsFont?.Dispose();

                warningWide?.Dispose();
                warning?.Dispose();

                columnTemplate?.Dispose();
                artworkColumnTemplate?.Dispose();

                clearArtworks();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LibraryReports));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.clearUseAnotherPresetButton = new System.Windows.Forms.Button();
            this.hidePreviewCheckBoxLabel = new System.Windows.Forms.Label();
            this.hidePreviewCheckBox = new System.Windows.Forms.CheckBox();
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
            this.formatComboBox = new System.Windows.Forms.ComboBox();
            this.labelFormat = new System.Windows.Forms.Label();
            this.useAnotherPresetAsSourceComboBox = new System.Windows.Forms.ComboBox();
            this.useAnotherPresetAsSourceLabel = new System.Windows.Forms.Label();
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
            this.TextColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImageColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.totalsCheckBoxLabel = new System.Windows.Forms.Label();
            this.totalsCheckBox = new System.Windows.Forms.CheckBox();
            this.columnNameTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.expressionsDataGridView = new System.Windows.Forms.DataGridView();
            this.expressionsDataGridViewCheckedColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.expressionsDataGridViewExpressionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.expressionsDataGridViewNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagsDataGridView = new System.Windows.Forms.DataGridView();
            this.tagsDataGridViewCheckedColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tagsDataGridViewFunctionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagsDataGridViewTagColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagsDataGridViewInfoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagsDataGridViewTotalsCheckedColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.buttonAddFunction = new System.Windows.Forms.Button();
            this.buttonUpdateFunction = new System.Windows.Forms.Button();
            this.multipleItemsSplitterLabel = new System.Windows.Forms.Label();
            this.multipleItemsSplitterTrimCheckBoxLabel = new System.Windows.Forms.Label();
            this.expressionTextBox = new System.Windows.Forms.TextBox();
            this.multipleItemsSplitterTrimCheckBox = new System.Windows.Forms.CheckBox();
            this.multipleItemsSplitterComboBox = new System.Windows.Forms.ComboBox();
            this.expressionLabel = new System.Windows.Forms.Label();
            this.buttonClearExpression = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.parameter2Label = new System.Windows.Forms.Label();
            this.sourceTagList = new System.Windows.Forms.ComboBox();
            this.labelFunction = new System.Windows.Forms.Label();
            this.forTagLabel = new System.Windows.Forms.Label();
            this.parameter2ComboBox = new System.Windows.Forms.ComboBox();
            this.functionComboBox = new System.Windows.Forms.ComboBox();
            this.autoApplyPresetsLabel = new System.Windows.Forms.Label();
            this.dirtyErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();

            //MusicBee
            if (useSkinColors)
            {
                this.presetTabControl.Dispose();
                this.presetTabControl = new FlatTabControl();
            }

            this.presetList.Dispose();
            this.presetNameTextBox.Dispose();
            this.appendTextBox.Dispose();
            this.idTextBox.Dispose();
            this.expressionTextBox.Dispose();

            this.presetList = new CustomCheckedListBox(Plugin.SavedSettings.dontUseSkinColors);
            this.presetNameTextBox = ControlsTools.CreateMusicBeeTextBox();
            this.appendTextBox = ControlsTools.CreateMusicBeeTextBox();
            this.idTextBox = ControlsTools.CreateMusicBeeTextBox();
            this.expressionTextBox = ControlsTools.CreateMusicBeeTextBox();
            //~MusicBee

            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.openReportCheckBoxPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.newArtworkSizeUpDown)).BeginInit();
            this.listBoxTableLayoutPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.presetTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.expressionsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).BeginInit();
            this.SuspendLayout();
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
            this.dirtyErrorProvider.SetIconAlignment(this.tableLayoutPanel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tableLayoutPanel1.IconAlignment"))));
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.clearUseAnotherPresetButton);
            this.panel1.Controls.Add(this.hidePreviewCheckBoxLabel);
            this.panel1.Controls.Add(this.hidePreviewCheckBox);
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
            this.panel1.Controls.Add(this.formatComboBox);
            this.panel1.Controls.Add(this.labelFormat);
            this.panel1.Controls.Add(this.useAnotherPresetAsSourceComboBox);
            this.panel1.Controls.Add(this.useAnotherPresetAsSourceLabel);
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
            // clearUseAnotherPresetButton
            // 
            resources.ApplyResources(this.clearUseAnotherPresetButton, "clearUseAnotherPresetButton");
            this.clearUseAnotherPresetButton.Name = "clearUseAnotherPresetButton";
            this.clearUseAnotherPresetButton.Tag = "#labelFormat";
            this.toolTip1.SetToolTip(this.clearUseAnotherPresetButton, resources.GetString("clearUseAnotherPresetButton.ToolTip"));
            this.clearUseAnotherPresetButton.Click += new System.EventHandler(this.clearUseAnotherPresetButton_Click);
            // 
            // hidePreviewCheckBoxLabel
            // 
            resources.ApplyResources(this.hidePreviewCheckBoxLabel, "hidePreviewCheckBoxLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.hidePreviewCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("hidePreviewCheckBoxLabel.IconAlignment"))));
            this.hidePreviewCheckBoxLabel.Name = "hidePreviewCheckBoxLabel";
            this.hidePreviewCheckBoxLabel.Tag = "";
            this.toolTip1.SetToolTip(this.hidePreviewCheckBoxLabel, resources.GetString("hidePreviewCheckBoxLabel.ToolTip"));
            this.hidePreviewCheckBoxLabel.Click += new System.EventHandler(this.hidePreviewCheckBoxLabel_Click);
            // 
            // hidePreviewCheckBox
            // 
            resources.ApplyResources(this.hidePreviewCheckBox, "hidePreviewCheckBox");
            this.dirtyErrorProvider.SetIconAlignment(this.hidePreviewCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("hidePreviewCheckBox.IconAlignment"))));
            this.hidePreviewCheckBox.Name = "hidePreviewCheckBox";
            this.hidePreviewCheckBox.Tag = "#hidePreviewCheckBoxLabel";
            this.toolTip1.SetToolTip(this.hidePreviewCheckBox, resources.GetString("hidePreviewCheckBox.ToolTip"));
            this.hidePreviewCheckBox.CheckedChanged += new System.EventHandler(this.hidePreviewCheckBox_CheckedChanged);
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
            this.useAnotherPresetAsSourceComboBox.Tag = "#clearUseAnotherPresetButton";
            this.toolTip1.SetToolTip(this.useAnotherPresetAsSourceComboBox, resources.GetString("useAnotherPresetAsSourceComboBox.ToolTip"));
            this.useAnotherPresetAsSourceComboBox.SelectedIndexChanged += new System.EventHandler(this.useAnotherPresetAsSourceComboBox_SelectedIndexChanged);
            // 
            // useAnotherPresetAsSourceLabel
            // 
            resources.ApplyResources(this.useAnotherPresetAsSourceLabel, "useAnotherPresetAsSourceLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.useAnotherPresetAsSourceLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("useAnotherPresetAsSourceLabel.IconAlignment"))));
            this.useAnotherPresetAsSourceLabel.Image = global::MusicBeePlugin.Properties.Resources.warning_15;
            this.useAnotherPresetAsSourceLabel.Name = "useAnotherPresetAsSourceLabel";
            this.useAnotherPresetAsSourceLabel.Tag = "#useAnotherPresetAsSourceComboBox";
            this.toolTip1.SetToolTip(this.useAnotherPresetAsSourceLabel, resources.GetString("useAnotherPresetAsSourceLabel.ToolTip"));
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
            this.buttonSettings.Tag = "#buttonPreview@non-defaultable@square-button";
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
            this.presetNameTextBox.Tag = "#hidePreviewCheckBox";
            this.toolTip1.SetToolTip(this.presetNameTextBox, resources.GetString("presetNameTextBox.ToolTip"));
            this.presetNameTextBox.Leave += new System.EventHandler(this.presetNameTextBox_Leave);
            // 
            // listBoxTableLayoutPanel
            // 
            resources.ApplyResources(this.listBoxTableLayoutPanel, "listBoxTableLayoutPanel");
            this.listBoxTableLayoutPanel.Controls.Add(this.presetList, 0, 0);
            this.dirtyErrorProvider.SetIconAlignment(this.listBoxTableLayoutPanel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("listBoxTableLayoutPanel.IconAlignment"))));
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
            this.dirtyErrorProvider.SetIconAlignment(this.panel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("panel2.IconAlignment"))));
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
            this.previewTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.previewTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.previewTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.previewTable.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.previewTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TextColumn,
            this.ImageColumn});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.previewTable.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.previewTable, "previewTable");
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
            // TextColumn
            // 
            this.TextColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.TextColumn.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.TextColumn, "TextColumn");
            this.TextColumn.Name = "TextColumn";
            this.TextColumn.ReadOnly = true;
            // 
            // ImageColumn
            // 
            this.ImageColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle2.NullValue")));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ImageColumn.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.ImageColumn, "ImageColumn");
            this.ImageColumn.Name = "ImageColumn";
            this.ImageColumn.ReadOnly = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.buttonUpdateFunction);
            this.tabPage2.Controls.Add(this.buttonAddFunction);
            this.tabPage2.Controls.Add(this.columnNameTextBox);
            this.tabPage2.Controls.Add(this.buttonHelp);
            this.tabPage2.Controls.Add(this.buttonClearExpression);
            this.tabPage2.Controls.Add(this.expressionTextBox);
            this.tabPage2.Controls.Add(this.expressionLabel);
            this.tabPage2.Controls.Add(this.multipleItemsSplitterTrimCheckBoxLabel);
            this.tabPage2.Controls.Add(this.multipleItemsSplitterTrimCheckBox);
            this.tabPage2.Controls.Add(this.multipleItemsSplitterComboBox);
            this.tabPage2.Controls.Add(this.multipleItemsSplitterLabel);
            this.tabPage2.Controls.Add(this.totalsCheckBoxLabel);
            this.tabPage2.Controls.Add(this.totalsCheckBox);
            this.tabPage2.Controls.Add(this.sourceTagList);
            this.tabPage2.Controls.Add(this.forTagLabel);
            this.tabPage2.Controls.Add(this.functionComboBox);
            this.tabPage2.Controls.Add(this.labelFunction);
            this.tabPage2.Controls.Add(this.tableLayoutPanel2);
            this.tabPage2.Controls.Add(this.parameter2Label);
            this.tabPage2.Controls.Add(this.parameter2ComboBox);
            this.dirtyErrorProvider.SetIconAlignment(this.tabPage2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tabPage2.IconAlignment"))));
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            // 
            // totalsCheckBoxLabel
            // 
            resources.ApplyResources(this.totalsCheckBoxLabel, "totalsCheckBoxLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.totalsCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("totalsCheckBoxLabel.IconAlignment"))));
            this.totalsCheckBoxLabel.Name = "totalsCheckBoxLabel";
            this.totalsCheckBoxLabel.Tag = "#multipleItemsSplitterLabel";
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
            // columnNameTextBox
            // 
            resources.ApplyResources(this.columnNameTextBox, "columnNameTextBox");
            this.dirtyErrorProvider.SetIconAlignment(this.columnNameTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("columnNameTextBox.IconAlignment"))));
            this.columnNameTextBox.Name = "columnNameTextBox";
            this.columnNameTextBox.Tag = "#buttonAddFunction";
            this.toolTip1.SetToolTip(this.columnNameTextBox, resources.GetString("columnNameTextBox.ToolTip"));
            this.columnNameTextBox.TextChanged += new System.EventHandler(this.columnNameTextBox_TextChanged);
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.tagsDataGridView, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.expressionsDataGridView, 1, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Tag = "#tabPage2&tabPage2";
            // 
            // expressionsDataGridView
            // 
            this.expressionsDataGridView.AllowUserToAddRows = false;
            this.expressionsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.expressionsDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.expressionsDataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.expressionsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.expressionsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.expressionsDataGridViewCheckedColumn,
            this.expressionsDataGridViewExpressionColumn,
            this.expressionsDataGridViewNameColumn});
            resources.ApplyResources(this.expressionsDataGridView, "expressionsDataGridView");
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
            this.expressionsDataGridViewCheckedColumn.IndeterminateValue = "";
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
            // expressionsDataGridViewNameColumn
            // 
            this.expressionsDataGridViewNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.expressionsDataGridViewNameColumn.FillWeight = 50F;
            resources.ApplyResources(this.expressionsDataGridViewNameColumn, "expressionsDataGridViewNameColumn");
            this.expressionsDataGridViewNameColumn.Name = "expressionsDataGridViewNameColumn";
            // 
            // tagsDataGridView
            // 
            this.tagsDataGridView.AllowUserToAddRows = false;
            this.tagsDataGridView.AllowUserToDeleteRows = false;
            this.tagsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tagsDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.tagsDataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.tagsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tagsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tagsDataGridViewCheckedColumn,
            this.tagsDataGridViewFunctionColumn,
            this.tagsDataGridViewTagColumn,
            this.tagsDataGridViewInfoColumn,
            this.tagsDataGridViewTotalsCheckedColumn});
            resources.ApplyResources(this.tagsDataGridView, "tagsDataGridView");
            this.tagsDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dirtyErrorProvider.SetIconAlignment(this.tagsDataGridView, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tagsDataGridView.IconAlignment"))));
            this.tagsDataGridView.MultiSelect = false;
            this.tagsDataGridView.Name = "tagsDataGridView";
            this.tagsDataGridView.RowHeadersVisible = false;
            this.tagsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.toolTip1.SetToolTip(this.tagsDataGridView, resources.GetString("tagsDataGridView.ToolTip"));
            this.tagsDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tagsDataGridView_CellClick);
            this.tagsDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tagsDataGridView_CellContentClick);
            // 
            // tagsDataGridViewCheckedColumn
            // 
            this.tagsDataGridViewCheckedColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.tagsDataGridViewCheckedColumn.FalseValue = "F";
            resources.ApplyResources(this.tagsDataGridViewCheckedColumn, "tagsDataGridViewCheckedColumn");
            this.tagsDataGridViewCheckedColumn.IndeterminateValue = "";
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
            // tagsDataGridViewTotalsCheckedColumn
            // 
            this.tagsDataGridViewTotalsCheckedColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.tagsDataGridViewTotalsCheckedColumn.FalseValue = "F";
            resources.ApplyResources(this.tagsDataGridViewTotalsCheckedColumn, "tagsDataGridViewTotalsCheckedColumn");
            this.tagsDataGridViewTotalsCheckedColumn.Name = "tagsDataGridViewTotalsCheckedColumn";
            this.tagsDataGridViewTotalsCheckedColumn.ReadOnly = true;
            this.tagsDataGridViewTotalsCheckedColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.tagsDataGridViewTotalsCheckedColumn.TrueValue = "T";
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
            // buttonUpdateFunction
            // 
            resources.ApplyResources(this.buttonUpdateFunction, "buttonUpdateFunction");
            this.dirtyErrorProvider.SetIconAlignment(this.buttonUpdateFunction, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonUpdateFunction.IconAlignment"))));
            this.buttonUpdateFunction.Image = global::MusicBeePlugin.Properties.Resources.warning_wide_15;
            this.buttonUpdateFunction.Name = "buttonUpdateFunction";
            this.buttonUpdateFunction.Tag = "#tabPage2@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonUpdateFunction, resources.GetString("buttonUpdateFunction.ToolTip"));
            this.buttonUpdateFunction.Click += new System.EventHandler(this.buttonUpdateFunction_Click);
            // 
            // multipleItemsSplitterLabel
            // 
            resources.ApplyResources(this.multipleItemsSplitterLabel, "multipleItemsSplitterLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.multipleItemsSplitterLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("multipleItemsSplitterLabel.IconAlignment"))));
            this.multipleItemsSplitterLabel.Name = "multipleItemsSplitterLabel";
            this.multipleItemsSplitterLabel.Tag = "#multipleItemsSplitterComboBox";
            this.toolTip1.SetToolTip(this.multipleItemsSplitterLabel, resources.GetString("multipleItemsSplitterLabel.ToolTip"));
            // 
            // multipleItemsSplitterTrimCheckBoxLabel
            // 
            resources.ApplyResources(this.multipleItemsSplitterTrimCheckBoxLabel, "multipleItemsSplitterTrimCheckBoxLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.multipleItemsSplitterTrimCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("multipleItemsSplitterTrimCheckBoxLabel.IconAlignment"))));
            this.multipleItemsSplitterTrimCheckBoxLabel.Name = "multipleItemsSplitterTrimCheckBoxLabel";
            this.toolTip1.SetToolTip(this.multipleItemsSplitterTrimCheckBoxLabel, resources.GetString("multipleItemsSplitterTrimCheckBoxLabel.ToolTip"));
            this.multipleItemsSplitterTrimCheckBoxLabel.Click += new System.EventHandler(this.multipleItemsSplitterTrimCheckBoxLabel_Click);
            // 
            // expressionTextBox
            // 
            this.dirtyErrorProvider.SetIconAlignment(this.expressionTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("expressionTextBox.IconAlignment"))));
            resources.ApplyResources(this.expressionTextBox, "expressionTextBox");
            this.expressionTextBox.Name = "expressionTextBox";
            this.expressionTextBox.Tag = "#buttonClearExpression";
            this.toolTip1.SetToolTip(this.expressionTextBox, resources.GetString("expressionTextBox.ToolTip"));
            this.expressionTextBox.TextChanged += new System.EventHandler(this.expressionTextBox_TextChanged);
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
            // multipleItemsSplitterComboBox
            // 
            this.multipleItemsSplitterComboBox.DropDownWidth = 500;
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
            this.multipleItemsSplitterComboBox.DropDownClosed += new System.EventHandler(this.multipleItemsSplitterComboBox_DropDownClosed);
            // 
            // expressionLabel
            // 
            resources.ApplyResources(this.expressionLabel, "expressionLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.expressionLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("expressionLabel.IconAlignment"))));
            this.expressionLabel.Name = "expressionLabel";
            this.expressionLabel.Tag = "#expressionTextBox";
            this.toolTip1.SetToolTip(this.expressionLabel, resources.GetString("expressionLabel.ToolTip"));
            // 
            // buttonClearExpression
            // 
            this.dirtyErrorProvider.SetIconAlignment(this.buttonClearExpression, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonClearExpression.IconAlignment"))));
            resources.ApplyResources(this.buttonClearExpression, "buttonClearExpression");
            this.buttonClearExpression.Name = "buttonClearExpression";
            this.buttonClearExpression.Tag = "#buttonHelp@non-defaultable@square-button";
            this.toolTip1.SetToolTip(this.buttonClearExpression, resources.GetString("buttonClearExpression.ToolTip"));
            this.buttonClearExpression.Click += new System.EventHandler(this.buttonClearExpression_Click);
            // 
            // buttonHelp
            // 
            this.dirtyErrorProvider.SetIconAlignment(this.buttonHelp, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonHelp.IconAlignment"))));
            resources.ApplyResources(this.buttonHelp, "buttonHelp");
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Tag = "#columnNameTextBox@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonHelp, resources.GetString("buttonHelp.ToolTip"));
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // parameter2Label
            // 
            resources.ApplyResources(this.parameter2Label, "parameter2Label");
            this.dirtyErrorProvider.SetIconAlignment(this.parameter2Label, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("parameter2Label.IconAlignment"))));
            this.parameter2Label.Name = "parameter2Label";
            this.parameter2Label.Tag = "@parameter2ComboBox";
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
            // labelFunction
            // 
            resources.ApplyResources(this.labelFunction, "labelFunction");
            this.dirtyErrorProvider.SetIconAlignment(this.labelFunction, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelFunction.IconAlignment"))));
            this.labelFunction.Name = "labelFunction";
            this.labelFunction.Tag = "#functionComboBox";
            // 
            // forTagLabel
            // 
            resources.ApplyResources(this.forTagLabel, "forTagLabel");
            this.dirtyErrorProvider.SetIconAlignment(this.forTagLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("forTagLabel.IconAlignment"))));
            this.forTagLabel.Name = "forTagLabel";
            this.forTagLabel.Tag = "#sourceTagList";
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
            resources.ApplyResources(this.dataGridViewCheckBoxColumn1, "dataGridViewCheckBoxColumn1");
            this.dataGridViewCheckBoxColumn1.IndeterminateValue = "I";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
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
            resources.ApplyResources(this.dataGridViewCheckBoxColumn2, "dataGridViewCheckBoxColumn2");
            this.dataGridViewCheckBoxColumn2.IndeterminateValue = "I";
            this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
            this.dataGridViewCheckBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
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
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.autoApplyPresetsLabel);
            this.DoubleBuffered = true;
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
            ((System.ComponentModel.ISupportInitialize)(this.openReportCheckBoxPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.newArtworkSizeUpDown)).EndInit();
            this.listBoxTableLayoutPanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.presetTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.previewTable)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.expressionsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tagsDataGridView)).EndInit();
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
        private System.Windows.Forms.TabControl presetTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.Button buttonUpdateFunction;
        private System.Windows.Forms.Button buttonAddFunction;
        private System.Windows.Forms.Button buttonClearExpression;
        private System.Windows.Forms.Label assignHotkeyCheckBoxLabel;
        private System.Windows.Forms.Label useHotkeyForSelectedTracksCheckBoxLabel;
        private System.Windows.Forms.Label useAnotherPresetAsSourceLabel;
        private System.Windows.Forms.CheckBox useHotkeyForSelectedTracksCheckBox;
        private System.Windows.Forms.Label resizeArtworkCheckBoxLabel;
        private System.Windows.Forms.Label openReportCheckBoxLabel;
        private System.Windows.Forms.Label conditionCheckBoxLabel;
        private System.Windows.Forms.Label smartOperationCheckBoxLabel;
        private System.Windows.Forms.CheckBox smartOperationCheckBox;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.PictureBox openReportCheckBoxPicture;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel listBoxTableLayoutPanel;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private Label hidePreviewCheckBoxLabel;
        private CheckBox hidePreviewCheckBox;
        private DataGridView expressionsDataGridView;
        private DataGridView tagsDataGridView;
        private TextBox expressionTextBox;
        private Label expressionLabel;
        private Label parameter2Label;
        private ComboBox sourceTagList;
        private Label forTagLabel;
        private ComboBox functionComboBox;
        private Label labelFunction;
        private Label multipleItemsSplitterTrimCheckBoxLabel;
        private CheckBox multipleItemsSplitterTrimCheckBox;
        private ComboBox multipleItemsSplitterComboBox;
        private Label multipleItemsSplitterLabel;
        private ComboBox parameter2ComboBox;
        private TableLayoutPanel tableLayoutPanel2;
        private DataGridViewTextBoxColumn TextColumn;
        private DataGridViewImageColumn ImageColumn;
        private Button clearUseAnotherPresetButton;
        private DataGridViewCheckBoxColumn expressionsDataGridViewCheckedColumn;
        private DataGridViewTextBoxColumn expressionsDataGridViewExpressionColumn;
        private DataGridViewTextBoxColumn expressionsDataGridViewNameColumn;
        private TextBox columnNameTextBox;
        private Label totalsCheckBoxLabel;
        private CheckBox totalsCheckBox;
        private DataGridViewCheckBoxColumn tagsDataGridViewCheckedColumn;
        private DataGridViewTextBoxColumn tagsDataGridViewFunctionColumn;
        private DataGridViewTextBoxColumn tagsDataGridViewTagColumn;
        private DataGridViewTextBoxColumn tagsDataGridViewInfoColumn;
        private DataGridViewCheckBoxColumn tagsDataGridViewTotalsCheckedColumn;
    }
}