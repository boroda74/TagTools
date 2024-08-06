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
            if (disposing)
            {
                presetNameTextBox?.Parent?.Dispose();
                appendTextBox?.Parent?.Dispose();
                idTextBox?.Parent?.Dispose();
                expressionTextBox?.Parent?.Dispose();
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);

            if (disposing)
            {
                periodicCacheClearingTimer?.Dispose();

                totalsFont?.Dispose();

                warningWide?.Dispose();
                errorWide?.Dispose();
                fatalErrorWide?.Dispose();
                errorFatalErrorWide?.Dispose();

                warning?.Dispose();
                error?.Dispose();
                fatalError?.Dispose();
                errorFatalError?.Dispose();

                DefaultArtwork?.Dispose();
                artwork?.Dispose();
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.hidePreviewCheckBoxLabel = new System.Windows.Forms.Label();
            this.hidePreviewCheckBox = new System.Windows.Forms.CheckBox();
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
            this.presetNameTextBox = ControlsTools.CreateMusicBeeTextBox();
            this.appendTextBox = ControlsTools.CreateMusicBeeTextBox();
            this.idTextBox = ControlsTools.CreateMusicBeeTextBox();
            this.expressionTextBox = ControlsTools.CreateMusicBeeTextBox();
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
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
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
            this.splitContainer1.Tag = "#LibraryReports&LibraryReports@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.splitContainer1, resources.GetString("splitContainer1.ToolTip"));
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.listBoxTableLayoutPanel, 0, 0);
            this.dirtyErrorProvider.SetError(this.tableLayoutPanel1, resources.GetString("tableLayoutPanel1.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.tableLayoutPanel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tableLayoutPanel1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.tableLayoutPanel1, ((int)(resources.GetObject("tableLayoutPanel1.IconPadding"))));
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.toolTip1.SetToolTip(this.tableLayoutPanel1, resources.GetString("tableLayoutPanel1.ToolTip"));
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.hidePreviewCheckBoxLabel);
            this.panel1.Controls.Add(this.hidePreviewCheckBox);
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
            this.dirtyErrorProvider.SetError(this.panel1, resources.GetString("panel1.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.panel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("panel1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.panel1, ((int)(resources.GetObject("panel1.IconPadding"))));
            this.panel1.Name = "panel1";
            this.toolTip1.SetToolTip(this.panel1, resources.GetString("panel1.ToolTip"));
            // 
            // hidePreviewCheckBoxLabel
            // 
            resources.ApplyResources(this.hidePreviewCheckBoxLabel, "hidePreviewCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.hidePreviewCheckBoxLabel, resources.GetString("hidePreviewCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.hidePreviewCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("hidePreviewCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.hidePreviewCheckBoxLabel, ((int)(resources.GetObject("hidePreviewCheckBoxLabel.IconPadding"))));
            this.hidePreviewCheckBoxLabel.Name = "hidePreviewCheckBoxLabel";
            this.hidePreviewCheckBoxLabel.Tag = "";
            this.toolTip1.SetToolTip(this.hidePreviewCheckBoxLabel, resources.GetString("hidePreviewCheckBoxLabel.ToolTip"));
            this.hidePreviewCheckBoxLabel.Click += new System.EventHandler(this.hidePreviewCheckBoxLabel_Click);
            // 
            // hidePreviewCheckBox
            // 
            resources.ApplyResources(this.hidePreviewCheckBox, "hidePreviewCheckBox");
            this.dirtyErrorProvider.SetError(this.hidePreviewCheckBox, resources.GetString("hidePreviewCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.hidePreviewCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("hidePreviewCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.hidePreviewCheckBox, ((int)(resources.GetObject("hidePreviewCheckBox.IconPadding"))));
            this.hidePreviewCheckBox.Name = "hidePreviewCheckBox";
            this.hidePreviewCheckBox.Tag = "#hidePreviewCheckBoxLabel";
            this.toolTip1.SetToolTip(this.hidePreviewCheckBox, resources.GetString("hidePreviewCheckBox.ToolTip"));
            this.hidePreviewCheckBox.CheckedChanged += new System.EventHandler(this.hidePreviewCheckBox_CheckedChanged);
            // 
            // presetChainIsWrongPictureBox
            // 
            resources.ApplyResources(this.presetChainIsWrongPictureBox, "presetChainIsWrongPictureBox");
            this.dirtyErrorProvider.SetError(this.presetChainIsWrongPictureBox, resources.GetString("presetChainIsWrongPictureBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.presetChainIsWrongPictureBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("presetChainIsWrongPictureBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.presetChainIsWrongPictureBox, ((int)(resources.GetObject("presetChainIsWrongPictureBox.IconPadding"))));
            this.presetChainIsWrongPictureBox.Name = "presetChainIsWrongPictureBox";
            this.presetChainIsWrongPictureBox.TabStop = false;
            this.presetChainIsWrongPictureBox.Tag = "#labelFormat";
            this.toolTip1.SetToolTip(this.presetChainIsWrongPictureBox, resources.GetString("presetChainIsWrongPictureBox.ToolTip"));
            // 
            // smartOperationCheckBoxLabel
            // 
            resources.ApplyResources(this.smartOperationCheckBoxLabel, "smartOperationCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.smartOperationCheckBoxLabel, resources.GetString("smartOperationCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.smartOperationCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("smartOperationCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.smartOperationCheckBoxLabel, ((int)(resources.GetObject("smartOperationCheckBoxLabel.IconPadding"))));
            this.smartOperationCheckBoxLabel.Name = "smartOperationCheckBoxLabel";
            this.smartOperationCheckBoxLabel.Tag = "@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.smartOperationCheckBoxLabel, resources.GetString("smartOperationCheckBoxLabel.ToolTip"));
            this.smartOperationCheckBoxLabel.Click += new System.EventHandler(this.smartOperationCheckBoxLabel_Click);
            // 
            // smartOperationCheckBox
            // 
            resources.ApplyResources(this.smartOperationCheckBox, "smartOperationCheckBox");
            this.dirtyErrorProvider.SetError(this.smartOperationCheckBox, resources.GetString("smartOperationCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.smartOperationCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("smartOperationCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.smartOperationCheckBox, ((int)(resources.GetObject("smartOperationCheckBox.IconPadding"))));
            this.smartOperationCheckBox.Name = "smartOperationCheckBox";
            this.smartOperationCheckBox.Tag = "#smartOperationCheckBoxLabel";
            this.toolTip1.SetToolTip(this.smartOperationCheckBox, resources.GetString("smartOperationCheckBox.ToolTip"));
            // 
            // appendTextBox
            // 
            resources.ApplyResources(this.appendTextBox, "appendTextBox");
            this.dirtyErrorProvider.SetError(this.appendTextBox, resources.GetString("appendTextBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.appendTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("appendTextBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.appendTextBox, ((int)(resources.GetObject("appendTextBox.IconPadding"))));
            this.appendTextBox.Name = "appendTextBox";
            this.appendTextBox.Tag = "#smartOperationCheckBox";
            this.toolTip1.SetToolTip(this.appendTextBox, resources.GetString("appendTextBox.ToolTip"));
            this.appendTextBox.TextChanged += new System.EventHandler(this.appendTextBox_TextChanged);
            // 
            // appendLabel
            // 
            resources.ApplyResources(this.appendLabel, "appendLabel");
            this.dirtyErrorProvider.SetError(this.appendLabel, resources.GetString("appendLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.appendLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("appendLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.appendLabel, ((int)(resources.GetObject("appendLabel.IconPadding"))));
            this.appendLabel.Name = "appendLabel";
            this.appendLabel.Tag = "#appendTextBox";
            this.toolTip1.SetToolTip(this.appendLabel, resources.GetString("appendLabel.ToolTip"));
            // 
            // digitsLabel
            // 
            resources.ApplyResources(this.digitsLabel, "digitsLabel");
            this.dirtyErrorProvider.SetError(this.digitsLabel, resources.GetString("digitsLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.digitsLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("digitsLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.digitsLabel, ((int)(resources.GetObject("digitsLabel.IconPadding"))));
            this.digitsLabel.Name = "digitsLabel";
            this.digitsLabel.Tag = "#appendLabel";
            this.toolTip1.SetToolTip(this.digitsLabel, resources.GetString("digitsLabel.ToolTip"));
            // 
            // precisionDigitsComboBox
            // 
            resources.ApplyResources(this.precisionDigitsComboBox, "precisionDigitsComboBox");
            this.dirtyErrorProvider.SetError(this.precisionDigitsComboBox, resources.GetString("precisionDigitsComboBox.Error"));
            this.precisionDigitsComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.precisionDigitsComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("precisionDigitsComboBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.precisionDigitsComboBox, ((int)(resources.GetObject("precisionDigitsComboBox.IconPadding"))));
            this.precisionDigitsComboBox.Items.AddRange(new object[] {
            resources.GetString("precisionDigitsComboBox.Items"),
            resources.GetString("precisionDigitsComboBox.Items1"),
            resources.GetString("precisionDigitsComboBox.Items2"),
            resources.GetString("precisionDigitsComboBox.Items3"),
            resources.GetString("precisionDigitsComboBox.Items4"),
            resources.GetString("precisionDigitsComboBox.Items5")});
            this.precisionDigitsComboBox.Name = "precisionDigitsComboBox";
            this.precisionDigitsComboBox.Tag = "#digitsLabel";
            this.toolTip1.SetToolTip(this.precisionDigitsComboBox, resources.GetString("precisionDigitsComboBox.ToolTip"));
            this.precisionDigitsComboBox.TextChanged += new System.EventHandler(this.precisionDigitsComboBox_TextChanged);
            // 
            // roundToLabel
            // 
            resources.ApplyResources(this.roundToLabel, "roundToLabel");
            this.dirtyErrorProvider.SetError(this.roundToLabel, resources.GetString("roundToLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.roundToLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("roundToLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.roundToLabel, ((int)(resources.GetObject("roundToLabel.IconPadding"))));
            this.roundToLabel.Name = "roundToLabel";
            this.roundToLabel.Tag = "#precisionDigitsComboBox";
            this.toolTip1.SetToolTip(this.roundToLabel, resources.GetString("roundToLabel.ToolTip"));
            // 
            // mulDivFactorComboBox
            // 
            resources.ApplyResources(this.mulDivFactorComboBox, "mulDivFactorComboBox");
            this.dirtyErrorProvider.SetError(this.mulDivFactorComboBox, resources.GetString("mulDivFactorComboBox.Error"));
            this.mulDivFactorComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.mulDivFactorComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("mulDivFactorComboBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.mulDivFactorComboBox, ((int)(resources.GetObject("mulDivFactorComboBox.IconPadding"))));
            this.mulDivFactorComboBox.Items.AddRange(new object[] {
            resources.GetString("mulDivFactorComboBox.Items"),
            resources.GetString("mulDivFactorComboBox.Items1"),
            resources.GetString("mulDivFactorComboBox.Items2"),
            resources.GetString("mulDivFactorComboBox.Items3"),
            resources.GetString("mulDivFactorComboBox.Items4"),
            resources.GetString("mulDivFactorComboBox.Items5"),
            resources.GetString("mulDivFactorComboBox.Items6")});
            this.mulDivFactorComboBox.Name = "mulDivFactorComboBox";
            this.mulDivFactorComboBox.Tag = "#roundToLabel";
            this.toolTip1.SetToolTip(this.mulDivFactorComboBox, resources.GetString("mulDivFactorComboBox.ToolTip"));
            this.mulDivFactorComboBox.TextChanged += new System.EventHandler(this.mulDivFactorComboBox_TextChanged);
            // 
            // operationComboBox
            // 
            resources.ApplyResources(this.operationComboBox, "operationComboBox");
            this.operationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dirtyErrorProvider.SetError(this.operationComboBox, resources.GetString("operationComboBox.Error"));
            this.operationComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.operationComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("operationComboBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.operationComboBox, ((int)(resources.GetObject("operationComboBox.IconPadding"))));
            this.operationComboBox.Items.AddRange(new object[] {
            resources.GetString("operationComboBox.Items"),
            resources.GetString("operationComboBox.Items1")});
            this.operationComboBox.Name = "operationComboBox";
            this.operationComboBox.Tag = "#mulDivFactorComboBox@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.operationComboBox, resources.GetString("operationComboBox.ToolTip"));
            this.operationComboBox.SelectedIndexChanged += new System.EventHandler(this.operationComboBox_SelectedIndexChanged);
            // 
            // clearIdButton
            // 
            resources.ApplyResources(this.clearIdButton, "clearIdButton");
            this.dirtyErrorProvider.SetError(this.clearIdButton, resources.GetString("clearIdButton.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.clearIdButton, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("clearIdButton.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.clearIdButton, ((int)(resources.GetObject("clearIdButton.IconPadding"))));
            this.clearIdButton.Name = "clearIdButton";
            this.clearIdButton.Tag = "#splitContainer1@non-defaultable@square-control";
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
            // labelFunctionId
            // 
            resources.ApplyResources(this.labelFunctionId, "labelFunctionId");
            this.dirtyErrorProvider.SetError(this.labelFunctionId, resources.GetString("labelFunctionId.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.labelFunctionId, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelFunctionId.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelFunctionId, ((int)(resources.GetObject("labelFunctionId.IconPadding"))));
            this.labelFunctionId.Name = "labelFunctionId";
            this.labelFunctionId.Tag = "#idTextBox";
            this.toolTip1.SetToolTip(this.labelFunctionId, resources.GetString("labelFunctionId.ToolTip"));
            // 
            // destinationTagList
            // 
            resources.ApplyResources(this.destinationTagList, "destinationTagList");
            this.destinationTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.destinationTagList.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.destinationTagList, resources.GetString("destinationTagList.Error"));
            this.destinationTagList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.destinationTagList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("destinationTagList.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.destinationTagList, ((int)(resources.GetObject("destinationTagList.IconPadding"))));
            this.destinationTagList.Name = "destinationTagList";
            this.toolTip1.SetToolTip(this.destinationTagList, resources.GetString("destinationTagList.ToolTip"));
            this.destinationTagList.SelectedIndexChanged += new System.EventHandler(this.destinationTagList_SelectedIndexChanged);
            // 
            // labelSaveToTag
            // 
            resources.ApplyResources(this.labelSaveToTag, "labelSaveToTag");
            this.dirtyErrorProvider.SetError(this.labelSaveToTag, resources.GetString("labelSaveToTag.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.labelSaveToTag, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelSaveToTag.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelSaveToTag, ((int)(resources.GetObject("labelSaveToTag.IconPadding"))));
            this.labelSaveToTag.Name = "labelSaveToTag";
            this.labelSaveToTag.Tag = "#destinationTagList";
            this.toolTip1.SetToolTip(this.labelSaveToTag, resources.GetString("labelSaveToTag.ToolTip"));
            // 
            // sourceFieldComboBox
            // 
            resources.ApplyResources(this.sourceFieldComboBox, "sourceFieldComboBox");
            this.sourceFieldComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceFieldComboBox.DropDownWidth = 600;
            this.dirtyErrorProvider.SetError(this.sourceFieldComboBox, resources.GetString("sourceFieldComboBox.Error"));
            this.sourceFieldComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.sourceFieldComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("sourceFieldComboBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.sourceFieldComboBox, ((int)(resources.GetObject("sourceFieldComboBox.IconPadding"))));
            this.sourceFieldComboBox.Name = "sourceFieldComboBox";
            this.sourceFieldComboBox.Tag = "#labelSaveToTag";
            this.toolTip1.SetToolTip(this.sourceFieldComboBox, resources.GetString("sourceFieldComboBox.ToolTip"));
            this.sourceFieldComboBox.SelectedIndexChanged += new System.EventHandler(this.sourceFieldComboBox_SelectedIndexChanged);
            // 
            // labelSaveField
            // 
            resources.ApplyResources(this.labelSaveField, "labelSaveField");
            this.dirtyErrorProvider.SetError(this.labelSaveField, resources.GetString("labelSaveField.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.labelSaveField, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelSaveField.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelSaveField, ((int)(resources.GetObject("labelSaveField.IconPadding"))));
            this.labelSaveField.Name = "labelSaveField";
            this.labelSaveField.Tag = "#sourceFieldComboBox";
            this.toolTip1.SetToolTip(this.labelSaveField, resources.GetString("labelSaveField.ToolTip"));
            // 
            // buttonFilterResults
            // 
            resources.ApplyResources(this.buttonFilterResults, "buttonFilterResults");
            this.dirtyErrorProvider.SetError(this.buttonFilterResults, resources.GetString("buttonFilterResults.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonFilterResults, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonFilterResults.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonFilterResults, ((int)(resources.GetObject("buttonFilterResults.IconPadding"))));
            this.buttonFilterResults.Name = "buttonFilterResults";
            this.buttonFilterResults.Tag = "#splitContainer1@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonFilterResults, resources.GetString("buttonFilterResults.ToolTip"));
            this.buttonFilterResults.Click += new System.EventHandler(this.buttonFilterResults_Click);
            // 
            // comparedFieldList
            // 
            resources.ApplyResources(this.comparedFieldList, "comparedFieldList");
            this.comparedFieldList.DropDownWidth = 600;
            this.dirtyErrorProvider.SetError(this.comparedFieldList, resources.GetString("comparedFieldList.Error"));
            this.comparedFieldList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.comparedFieldList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("comparedFieldList.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.comparedFieldList, ((int)(resources.GetObject("comparedFieldList.IconPadding"))));
            this.comparedFieldList.Name = "comparedFieldList";
            this.comparedFieldList.Tag = "#buttonFilterResults";
            this.toolTip1.SetToolTip(this.comparedFieldList, resources.GetString("comparedFieldList.ToolTip"));
            this.comparedFieldList.TextChanged += new System.EventHandler(this.comparedFieldList_TextChanged);
            // 
            // conditionList
            // 
            resources.ApplyResources(this.conditionList, "conditionList");
            this.conditionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dirtyErrorProvider.SetError(this.conditionList, resources.GetString("conditionList.Error"));
            this.conditionList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.conditionList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("conditionList.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.conditionList, ((int)(resources.GetObject("conditionList.IconPadding"))));
            this.conditionList.Name = "conditionList";
            this.conditionList.Tag = "#comparedFieldList";
            this.toolTip1.SetToolTip(this.conditionList, resources.GetString("conditionList.ToolTip"));
            this.conditionList.SelectedIndexChanged += new System.EventHandler(this.conditionList_SelectedIndexChanged);
            // 
            // conditionFieldList
            // 
            resources.ApplyResources(this.conditionFieldList, "conditionFieldList");
            this.conditionFieldList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.conditionFieldList.DropDownWidth = 600;
            this.dirtyErrorProvider.SetError(this.conditionFieldList, resources.GetString("conditionFieldList.Error"));
            this.conditionFieldList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.conditionFieldList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("conditionFieldList.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.conditionFieldList, ((int)(resources.GetObject("conditionFieldList.IconPadding"))));
            this.conditionFieldList.Name = "conditionFieldList";
            this.conditionFieldList.Tag = "#conditionList";
            this.toolTip1.SetToolTip(this.conditionFieldList, resources.GetString("conditionFieldList.ToolTip"));
            this.conditionFieldList.SelectedIndexChanged += new System.EventHandler(this.conditionFieldList_SelectedIndexChanged);
            // 
            // conditionCheckBoxLabel
            // 
            resources.ApplyResources(this.conditionCheckBoxLabel, "conditionCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.conditionCheckBoxLabel, resources.GetString("conditionCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.conditionCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("conditionCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.conditionCheckBoxLabel, ((int)(resources.GetObject("conditionCheckBoxLabel.IconPadding"))));
            this.conditionCheckBoxLabel.Name = "conditionCheckBoxLabel";
            this.conditionCheckBoxLabel.Tag = "#conditionFieldList";
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
            // buttonExport
            // 
            resources.ApplyResources(this.buttonExport, "buttonExport");
            this.dirtyErrorProvider.SetError(this.buttonExport, resources.GetString("buttonExport.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonExport, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonExport.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonExport, ((int)(resources.GetObject("buttonExport.IconPadding"))));
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Tag = "#splitContainer1@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonExport, resources.GetString("buttonExport.ToolTip"));
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // openReportCheckBoxPicture
            // 
            resources.ApplyResources(this.openReportCheckBoxPicture, "openReportCheckBoxPicture");
            this.dirtyErrorProvider.SetError(this.openReportCheckBoxPicture, resources.GetString("openReportCheckBoxPicture.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.openReportCheckBoxPicture, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("openReportCheckBoxPicture.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.openReportCheckBoxPicture, ((int)(resources.GetObject("openReportCheckBoxPicture.IconPadding"))));
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
            this.dirtyErrorProvider.SetError(this.openReportCheckBoxLabel, resources.GetString("openReportCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.openReportCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("openReportCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.openReportCheckBoxLabel, ((int)(resources.GetObject("openReportCheckBoxLabel.IconPadding"))));
            this.openReportCheckBoxLabel.Name = "openReportCheckBoxLabel";
            this.openReportCheckBoxLabel.Tag = "&openReportCheckBoxPicture";
            this.toolTip1.SetToolTip(this.openReportCheckBoxLabel, resources.GetString("openReportCheckBoxLabel.ToolTip"));
            this.openReportCheckBoxLabel.Click += new System.EventHandler(this.openReportCheckBoxLabel_Click);
            // 
            // openReportCheckBox
            // 
            resources.ApplyResources(this.openReportCheckBox, "openReportCheckBox");
            this.dirtyErrorProvider.SetError(this.openReportCheckBox, resources.GetString("openReportCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.openReportCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("openReportCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.openReportCheckBox, ((int)(resources.GetObject("openReportCheckBox.IconPadding"))));
            this.openReportCheckBox.Name = "openReportCheckBox";
            this.openReportCheckBox.Tag = "#openReportCheckBoxLabel|openReportCheckBoxPicture";
            this.toolTip1.SetToolTip(this.openReportCheckBox, resources.GetString("openReportCheckBox.ToolTip"));
            // 
            // newArtworkSizeUpDown
            // 
            resources.ApplyResources(this.newArtworkSizeUpDown, "newArtworkSizeUpDown");
            this.dirtyErrorProvider.SetError(this.newArtworkSizeUpDown, resources.GetString("newArtworkSizeUpDown.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.newArtworkSizeUpDown, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("newArtworkSizeUpDown.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.newArtworkSizeUpDown, ((int)(resources.GetObject("newArtworkSizeUpDown.IconPadding"))));
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
            this.toolTip1.SetToolTip(this.newArtworkSizeUpDown, resources.GetString("newArtworkSizeUpDown.ToolTip"));
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
            this.dirtyErrorProvider.SetError(this.labelRemark, resources.GetString("labelRemark.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.labelRemark, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelRemark.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelRemark, ((int)(resources.GetObject("labelRemark.IconPadding"))));
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Tag = "";
            this.toolTip1.SetToolTip(this.labelRemark, resources.GetString("labelRemark.ToolTip"));
            // 
            // resizeArtworkCheckBoxLabel
            // 
            resources.ApplyResources(this.resizeArtworkCheckBoxLabel, "resizeArtworkCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.resizeArtworkCheckBoxLabel, resources.GetString("resizeArtworkCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.resizeArtworkCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("resizeArtworkCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.resizeArtworkCheckBoxLabel, ((int)(resources.GetObject("resizeArtworkCheckBoxLabel.IconPadding"))));
            this.resizeArtworkCheckBoxLabel.Name = "resizeArtworkCheckBoxLabel";
            this.resizeArtworkCheckBoxLabel.Tag = "#newArtworkSizeUpDown";
            this.toolTip1.SetToolTip(this.resizeArtworkCheckBoxLabel, resources.GetString("resizeArtworkCheckBoxLabel.ToolTip"));
            this.resizeArtworkCheckBoxLabel.Click += new System.EventHandler(this.resizeArtworkCheckBoxLabel_Click);
            // 
            // resizeArtworkCheckBox
            // 
            resources.ApplyResources(this.resizeArtworkCheckBox, "resizeArtworkCheckBox");
            this.dirtyErrorProvider.SetError(this.resizeArtworkCheckBox, resources.GetString("resizeArtworkCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.resizeArtworkCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("resizeArtworkCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.resizeArtworkCheckBox, ((int)(resources.GetObject("resizeArtworkCheckBox.IconPadding"))));
            this.resizeArtworkCheckBox.Name = "resizeArtworkCheckBox";
            this.resizeArtworkCheckBox.Tag = "#resizeArtworkCheckBoxLabel";
            this.toolTip1.SetToolTip(this.resizeArtworkCheckBox, resources.GetString("resizeArtworkCheckBox.ToolTip"));
            this.resizeArtworkCheckBox.CheckedChanged += new System.EventHandler(this.resizeArtworkCheckBox_CheckedChanged);
            // 
            // totalsCheckBoxLabel
            // 
            resources.ApplyResources(this.totalsCheckBoxLabel, "totalsCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.totalsCheckBoxLabel, resources.GetString("totalsCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.totalsCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("totalsCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.totalsCheckBoxLabel, ((int)(resources.GetObject("totalsCheckBoxLabel.IconPadding"))));
            this.totalsCheckBoxLabel.Name = "totalsCheckBoxLabel";
            this.totalsCheckBoxLabel.Tag = "#resizeArtworkCheckBox";
            this.toolTip1.SetToolTip(this.totalsCheckBoxLabel, resources.GetString("totalsCheckBoxLabel.ToolTip"));
            this.totalsCheckBoxLabel.Click += new System.EventHandler(this.totalsCheckBoxLabel_Click);
            // 
            // totalsCheckBox
            // 
            resources.ApplyResources(this.totalsCheckBox, "totalsCheckBox");
            this.dirtyErrorProvider.SetError(this.totalsCheckBox, resources.GetString("totalsCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.totalsCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("totalsCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.totalsCheckBox, ((int)(resources.GetObject("totalsCheckBox.IconPadding"))));
            this.totalsCheckBox.Name = "totalsCheckBox";
            this.totalsCheckBox.Tag = "#totalsCheckBoxLabel";
            this.toolTip1.SetToolTip(this.totalsCheckBox, resources.GetString("totalsCheckBox.ToolTip"));
            this.totalsCheckBox.CheckedChanged += new System.EventHandler(this.totalsCheckBox_CheckedChanged);
            // 
            // formatComboBox
            // 
            resources.ApplyResources(this.formatComboBox, "formatComboBox");
            this.formatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dirtyErrorProvider.SetError(this.formatComboBox, resources.GetString("formatComboBox.Error"));
            this.formatComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.formatComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("formatComboBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.formatComboBox, ((int)(resources.GetObject("formatComboBox.IconPadding"))));
            this.formatComboBox.Name = "formatComboBox";
            this.formatComboBox.Tag = "#splitContainer1";
            this.toolTip1.SetToolTip(this.formatComboBox, resources.GetString("formatComboBox.ToolTip"));
            this.formatComboBox.SelectedIndexChanged += new System.EventHandler(this.formatComboBox_SelectedIndexChanged);
            // 
            // labelFormat
            // 
            resources.ApplyResources(this.labelFormat, "labelFormat");
            this.dirtyErrorProvider.SetError(this.labelFormat, resources.GetString("labelFormat.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.labelFormat, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelFormat.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelFormat, ((int)(resources.GetObject("labelFormat.IconPadding"))));
            this.labelFormat.Name = "labelFormat";
            this.labelFormat.Tag = "#formatComboBox";
            this.toolTip1.SetToolTip(this.labelFormat, resources.GetString("labelFormat.ToolTip"));
            // 
            // useAnotherPresetAsSourceComboBox
            // 
            resources.ApplyResources(this.useAnotherPresetAsSourceComboBox, "useAnotherPresetAsSourceComboBox");
            this.useAnotherPresetAsSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.useAnotherPresetAsSourceComboBox.DropDownWidth = 600;
            this.dirtyErrorProvider.SetError(this.useAnotherPresetAsSourceComboBox, resources.GetString("useAnotherPresetAsSourceComboBox.Error"));
            this.useAnotherPresetAsSourceComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.useAnotherPresetAsSourceComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("useAnotherPresetAsSourceComboBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.useAnotherPresetAsSourceComboBox, ((int)(resources.GetObject("useAnotherPresetAsSourceComboBox.IconPadding"))));
            this.useAnotherPresetAsSourceComboBox.Name = "useAnotherPresetAsSourceComboBox";
            this.useAnotherPresetAsSourceComboBox.Tag = "#presetChainIsWrongPictureBox";
            this.toolTip1.SetToolTip(this.useAnotherPresetAsSourceComboBox, resources.GetString("useAnotherPresetAsSourceComboBox.ToolTip"));
            this.useAnotherPresetAsSourceComboBox.SelectedIndexChanged += new System.EventHandler(this.useAnotherPresetAsSourceComboBox_SelectedIndexChanged);
            // 
            // useAnotherPresetAsSourceCheckBoxLabel
            // 
            resources.ApplyResources(this.useAnotherPresetAsSourceCheckBoxLabel, "useAnotherPresetAsSourceCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.useAnotherPresetAsSourceCheckBoxLabel, resources.GetString("useAnotherPresetAsSourceCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.useAnotherPresetAsSourceCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("useAnotherPresetAsSourceCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.useAnotherPresetAsSourceCheckBoxLabel, ((int)(resources.GetObject("useAnotherPresetAsSourceCheckBoxLabel.IconPadding"))));
            this.useAnotherPresetAsSourceCheckBoxLabel.Name = "useAnotherPresetAsSourceCheckBoxLabel";
            this.useAnotherPresetAsSourceCheckBoxLabel.Tag = "#useAnotherPresetAsSourceComboBox";
            this.toolTip1.SetToolTip(this.useAnotherPresetAsSourceCheckBoxLabel, resources.GetString("useAnotherPresetAsSourceCheckBoxLabel.ToolTip"));
            this.useAnotherPresetAsSourceCheckBoxLabel.Click += new System.EventHandler(this.useAnotherPresetAsSourceCheckBoxLabel_Click);
            // 
            // useAnotherPresetAsSourceCheckBox
            // 
            resources.ApplyResources(this.useAnotherPresetAsSourceCheckBox, "useAnotherPresetAsSourceCheckBox");
            this.dirtyErrorProvider.SetError(this.useAnotherPresetAsSourceCheckBox, resources.GetString("useAnotherPresetAsSourceCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.useAnotherPresetAsSourceCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("useAnotherPresetAsSourceCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.useAnotherPresetAsSourceCheckBox, ((int)(resources.GetObject("useAnotherPresetAsSourceCheckBox.IconPadding"))));
            this.useAnotherPresetAsSourceCheckBox.Name = "useAnotherPresetAsSourceCheckBox";
            this.useAnotherPresetAsSourceCheckBox.Tag = "#useAnotherPresetAsSourceCheckBoxLabel";
            this.toolTip1.SetToolTip(this.useAnotherPresetAsSourceCheckBox, resources.GetString("useAnotherPresetAsSourceCheckBox.ToolTip"));
            this.useAnotherPresetAsSourceCheckBox.CheckedChanged += new System.EventHandler(this.useAnotherPresetAsSourceCheckBox_CheckedChanged);
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
            this.buttonClose.Tag = "#splitContainer1@non-defaultable";
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
            this.buttonSaveClose.Tag = "#buttonClose";
            this.toolTip1.SetToolTip(this.buttonSaveClose, resources.GetString("buttonSaveClose.ToolTip"));
            this.buttonSaveClose.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.dirtyErrorProvider.SetError(this.buttonOK, resources.GetString("buttonOK.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonOK, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonOK.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonOK, ((int)(resources.GetObject("buttonOK.IconPadding"))));
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
            this.dirtyErrorProvider.SetIconPadding(this.buttonPreview, ((int)(resources.GetObject("buttonPreview.IconPadding"))));
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Tag = "#buttonOK";
            this.toolTip1.SetToolTip(this.buttonPreview, resources.GetString("buttonPreview.ToolTip"));
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
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
            this.toolTip1.SetToolTip(this.buttonSettings, resources.GetString("buttonSettings.ToolTip"));
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // buttonDeletePreset
            // 
            resources.ApplyResources(this.buttonDeletePreset, "buttonDeletePreset");
            this.dirtyErrorProvider.SetError(this.buttonDeletePreset, resources.GetString("buttonDeletePreset.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonDeletePreset, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonDeletePreset.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonDeletePreset, ((int)(resources.GetObject("buttonDeletePreset.IconPadding"))));
            this.buttonDeletePreset.Name = "buttonDeletePreset";
            this.toolTip1.SetToolTip(this.buttonDeletePreset, resources.GetString("buttonDeletePreset.ToolTip"));
            this.buttonDeletePreset.Click += new System.EventHandler(this.buttonDeletePreset_Click);
            // 
            // buttonCopyPreset
            // 
            resources.ApplyResources(this.buttonCopyPreset, "buttonCopyPreset");
            this.dirtyErrorProvider.SetError(this.buttonCopyPreset, resources.GetString("buttonCopyPreset.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonCopyPreset, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonCopyPreset.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonCopyPreset, ((int)(resources.GetObject("buttonCopyPreset.IconPadding"))));
            this.buttonCopyPreset.Name = "buttonCopyPreset";
            this.buttonCopyPreset.Tag = "#buttonDeletePreset";
            this.toolTip1.SetToolTip(this.buttonCopyPreset, resources.GetString("buttonCopyPreset.ToolTip"));
            this.buttonCopyPreset.Click += new System.EventHandler(this.buttonCopyPreset_Click);
            // 
            // buttonAddPreset
            // 
            resources.ApplyResources(this.buttonAddPreset, "buttonAddPreset");
            this.dirtyErrorProvider.SetError(this.buttonAddPreset, resources.GetString("buttonAddPreset.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonAddPreset, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonAddPreset.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonAddPreset, ((int)(resources.GetObject("buttonAddPreset.IconPadding"))));
            this.buttonAddPreset.Name = "buttonAddPreset";
            this.buttonAddPreset.Tag = "#buttonCopyPreset@pinned-to-parent-x@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonAddPreset, resources.GetString("buttonAddPreset.ToolTip"));
            this.buttonAddPreset.Click += new System.EventHandler(this.buttonAddPreset_Click);
            // 
            // useHotkeyForSelectedTracksCheckBoxLabel
            // 
            resources.ApplyResources(this.useHotkeyForSelectedTracksCheckBoxLabel, "useHotkeyForSelectedTracksCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.useHotkeyForSelectedTracksCheckBoxLabel, resources.GetString("useHotkeyForSelectedTracksCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.useHotkeyForSelectedTracksCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("useHotkeyForSelectedTracksCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.useHotkeyForSelectedTracksCheckBoxLabel, ((int)(resources.GetObject("useHotkeyForSelectedTracksCheckBoxLabel.IconPadding"))));
            this.useHotkeyForSelectedTracksCheckBoxLabel.Name = "useHotkeyForSelectedTracksCheckBoxLabel";
            this.useHotkeyForSelectedTracksCheckBoxLabel.Tag = "@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.useHotkeyForSelectedTracksCheckBoxLabel, resources.GetString("useHotkeyForSelectedTracksCheckBoxLabel.ToolTip"));
            this.useHotkeyForSelectedTracksCheckBoxLabel.Click += new System.EventHandler(this.useHotkeyForSelectedTracksCheckBoxLabel_Click);
            // 
            // useHotkeyForSelectedTracksCheckBox
            // 
            resources.ApplyResources(this.useHotkeyForSelectedTracksCheckBox, "useHotkeyForSelectedTracksCheckBox");
            this.dirtyErrorProvider.SetError(this.useHotkeyForSelectedTracksCheckBox, resources.GetString("useHotkeyForSelectedTracksCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.useHotkeyForSelectedTracksCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("useHotkeyForSelectedTracksCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.useHotkeyForSelectedTracksCheckBox, ((int)(resources.GetObject("useHotkeyForSelectedTracksCheckBox.IconPadding"))));
            this.useHotkeyForSelectedTracksCheckBox.Name = "useHotkeyForSelectedTracksCheckBox";
            this.useHotkeyForSelectedTracksCheckBox.Tag = "#useHotkeyForSelectedTracksCheckBoxLabel";
            this.toolTip1.SetToolTip(this.useHotkeyForSelectedTracksCheckBox, resources.GetString("useHotkeyForSelectedTracksCheckBox.ToolTip"));
            this.useHotkeyForSelectedTracksCheckBox.CheckedChanged += new System.EventHandler(this.useHotkeyForSelectedTracksCheckBox_CheckedChanged);
            // 
            // assignHotkeyCheckBoxLabel
            // 
            resources.ApplyResources(this.assignHotkeyCheckBoxLabel, "assignHotkeyCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.assignHotkeyCheckBoxLabel, resources.GetString("assignHotkeyCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.assignHotkeyCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("assignHotkeyCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.assignHotkeyCheckBoxLabel, ((int)(resources.GetObject("assignHotkeyCheckBoxLabel.IconPadding"))));
            this.assignHotkeyCheckBoxLabel.Name = "assignHotkeyCheckBoxLabel";
            this.assignHotkeyCheckBoxLabel.Tag = "#useHotkeyForSelectedTracksCheckBox";
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
            // presetNameTextBox
            // 
            resources.ApplyResources(this.presetNameTextBox, "presetNameTextBox");
            this.dirtyErrorProvider.SetError(this.presetNameTextBox, resources.GetString("presetNameTextBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.presetNameTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("presetNameTextBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.presetNameTextBox, ((int)(resources.GetObject("presetNameTextBox.IconPadding"))));
            this.presetNameTextBox.Name = "presetNameTextBox";
            this.presetNameTextBox.Tag = "#hidePreviewCheckBox";
            this.toolTip1.SetToolTip(this.presetNameTextBox, resources.GetString("presetNameTextBox.ToolTip"));
            this.presetNameTextBox.Leave += new System.EventHandler(this.presetNameTextBox_Leave);
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
            this.presetList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.presetList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("presetList.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.presetList, ((int)(resources.GetObject("presetList.IconPadding"))));
            this.presetList.Name = "presetList";
            this.presetList.Tag = "";
            this.toolTip1.SetToolTip(this.presetList, resources.GetString("presetList.ToolTip"));
            this.presetList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.presetList_ItemCheck);
            this.presetList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.presetList_MouseClick);
            this.presetList.SelectedIndexChanged += new System.EventHandler(this.presetList_SelectedIndexChanged);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.presetTabControl);
            this.dirtyErrorProvider.SetError(this.panel2, resources.GetString("panel2.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.panel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("panel2.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.panel2, ((int)(resources.GetObject("panel2.IconPadding"))));
            this.panel2.Name = "panel2";
            this.toolTip1.SetToolTip(this.panel2, resources.GetString("panel2.ToolTip"));
            // 
            // presetTabControl
            // 
            resources.ApplyResources(this.presetTabControl, "presetTabControl");
            this.presetTabControl.Controls.Add(this.tabPage1);
            this.presetTabControl.Controls.Add(this.tabPage2);
            this.dirtyErrorProvider.SetError(this.presetTabControl, resources.GetString("presetTabControl.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.presetTabControl, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("presetTabControl.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.presetTabControl, ((int)(resources.GetObject("presetTabControl.IconPadding"))));
            this.presetTabControl.Name = "presetTabControl";
            this.presetTabControl.SelectedIndex = 0;
            this.toolTip1.SetToolTip(this.presetTabControl, resources.GetString("presetTabControl.ToolTip"));
            // 
            // tabPage1
            // 
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Controls.Add(this.previewTable);
            this.dirtyErrorProvider.SetError(this.tabPage1, resources.GetString("tabPage1.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.tabPage1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tabPage1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.tabPage1, ((int)(resources.GetObject("tabPage1.IconPadding"))));
            this.tabPage1.Name = "tabPage1";
            this.toolTip1.SetToolTip(this.tabPage1, resources.GetString("tabPage1.ToolTip"));
            // 
            // previewTable
            // 
            resources.ApplyResources(this.previewTable, "previewTable");
            this.previewTable.AllowUserToAddRows = false;
            this.previewTable.AllowUserToDeleteRows = false;
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
            this.dirtyErrorProvider.SetError(this.previewTable, resources.GetString("previewTable.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.previewTable, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("previewTable.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.previewTable, ((int)(resources.GetObject("previewTable.IconPadding"))));
            this.previewTable.MultiSelect = false;
            this.previewTable.Name = "previewTable";
            this.previewTable.RowHeadersVisible = false;
            this.previewTable.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.previewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.toolTip1.SetToolTip(this.previewTable, resources.GetString("previewTable.ToolTip"));
            this.previewTable.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.previewTable_CellClick);
            this.previewTable.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.previewTable_ColumnHeaderMouseClick);
            this.previewTable.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.previewTable_ColumnWidthChanged);
            this.previewTable.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.previewTable_DataError);
            this.previewTable.RowHeightChanged += new System.Windows.Forms.DataGridViewRowEventHandler(this.previewTable_RowHeightChanged);
            // 
            // tabPage2
            // 
            resources.ApplyResources(this.tabPage2, "tabPage2");
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
            this.dirtyErrorProvider.SetError(this.tabPage2, resources.GetString("tabPage2.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.tabPage2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tabPage2.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.tabPage2, ((int)(resources.GetObject("tabPage2.IconPadding"))));
            this.tabPage2.Name = "tabPage2";
            this.toolTip1.SetToolTip(this.tabPage2, resources.GetString("tabPage2.ToolTip"));
            // 
            // multipleItemsSplitterTrimCheckBoxLabel
            // 
            resources.ApplyResources(this.multipleItemsSplitterTrimCheckBoxLabel, "multipleItemsSplitterTrimCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.multipleItemsSplitterTrimCheckBoxLabel, resources.GetString("multipleItemsSplitterTrimCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.multipleItemsSplitterTrimCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("multipleItemsSplitterTrimCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.multipleItemsSplitterTrimCheckBoxLabel, ((int)(resources.GetObject("multipleItemsSplitterTrimCheckBoxLabel.IconPadding"))));
            this.multipleItemsSplitterTrimCheckBoxLabel.Name = "multipleItemsSplitterTrimCheckBoxLabel";
            this.toolTip1.SetToolTip(this.multipleItemsSplitterTrimCheckBoxLabel, resources.GetString("multipleItemsSplitterTrimCheckBoxLabel.ToolTip"));
            this.multipleItemsSplitterTrimCheckBoxLabel.Click += new System.EventHandler(this.multipleItemsSplitterTrimCheckBoxLabel_Click);
            // 
            // multipleItemsSplitterTrimCheckBox
            // 
            resources.ApplyResources(this.multipleItemsSplitterTrimCheckBox, "multipleItemsSplitterTrimCheckBox");
            this.dirtyErrorProvider.SetError(this.multipleItemsSplitterTrimCheckBox, resources.GetString("multipleItemsSplitterTrimCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.multipleItemsSplitterTrimCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("multipleItemsSplitterTrimCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.multipleItemsSplitterTrimCheckBox, ((int)(resources.GetObject("multipleItemsSplitterTrimCheckBox.IconPadding"))));
            this.multipleItemsSplitterTrimCheckBox.Name = "multipleItemsSplitterTrimCheckBox";
            this.multipleItemsSplitterTrimCheckBox.Tag = "#multipleItemsSplitterTrimCheckBoxLabel";
            this.toolTip1.SetToolTip(this.multipleItemsSplitterTrimCheckBox, resources.GetString("multipleItemsSplitterTrimCheckBox.ToolTip"));
            this.multipleItemsSplitterTrimCheckBox.UseVisualStyleBackColor = false;
            this.multipleItemsSplitterTrimCheckBox.CheckedChanged += new System.EventHandler(this.multipleItemsSplitterTrimCheckBox_CheckedChanged);
            // 
            // expressionsDataGridView
            // 
            resources.ApplyResources(this.expressionsDataGridView, "expressionsDataGridView");
            this.expressionsDataGridView.AllowUserToAddRows = false;
            this.expressionsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.expressionsDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.expressionsDataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.expressionsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.expressionsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.expressionsDataGridViewCheckedColumn,
            this.expressionsDataGridViewExpressionColumn});
            this.expressionsDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dirtyErrorProvider.SetError(this.expressionsDataGridView, resources.GetString("expressionsDataGridView.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.expressionsDataGridView, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("expressionsDataGridView.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.expressionsDataGridView, ((int)(resources.GetObject("expressionsDataGridView.IconPadding"))));
            this.expressionsDataGridView.MultiSelect = false;
            this.expressionsDataGridView.Name = "expressionsDataGridView";
            this.expressionsDataGridView.RowHeadersVisible = false;
            this.expressionsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.toolTip1.SetToolTip(this.expressionsDataGridView, resources.GetString("expressionsDataGridView.ToolTip"));
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
            resources.ApplyResources(this.tagsDataGridView, "tagsDataGridView");
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
            this.tagsDataGridViewInfoColumn});
            this.tagsDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dirtyErrorProvider.SetError(this.tagsDataGridView, resources.GetString("tagsDataGridView.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.tagsDataGridView, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tagsDataGridView.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.tagsDataGridView, ((int)(resources.GetObject("tagsDataGridView.IconPadding"))));
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
            this.dirtyErrorProvider.SetError(this.expressionTextBox, resources.GetString("expressionTextBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.expressionTextBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("expressionTextBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.expressionTextBox, ((int)(resources.GetObject("expressionTextBox.IconPadding"))));
            this.expressionTextBox.Name = "expressionTextBox";
            this.expressionTextBox.Tag = "#buttonsPanel";
            this.toolTip1.SetToolTip(this.expressionTextBox, resources.GetString("expressionTextBox.ToolTip"));
            this.expressionTextBox.TextChanged += new System.EventHandler(this.expressionTextBox_TextChanged);
            // 
            // expressionLabel
            // 
            resources.ApplyResources(this.expressionLabel, "expressionLabel");
            this.dirtyErrorProvider.SetError(this.expressionLabel, resources.GetString("expressionLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.expressionLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("expressionLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.expressionLabel, ((int)(resources.GetObject("expressionLabel.IconPadding"))));
            this.expressionLabel.Name = "expressionLabel";
            this.expressionLabel.Tag = "#expressionTextBox";
            this.toolTip1.SetToolTip(this.expressionLabel, resources.GetString("expressionLabel.ToolTip"));
            // 
            // multipleItemsSplitterComboBox
            // 
            resources.ApplyResources(this.multipleItemsSplitterComboBox, "multipleItemsSplitterComboBox");
            this.multipleItemsSplitterComboBox.DropDownWidth = 390;
            this.dirtyErrorProvider.SetError(this.multipleItemsSplitterComboBox, resources.GetString("multipleItemsSplitterComboBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.multipleItemsSplitterComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("multipleItemsSplitterComboBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.multipleItemsSplitterComboBox, ((int)(resources.GetObject("multipleItemsSplitterComboBox.IconPadding"))));
            this.multipleItemsSplitterComboBox.Items.AddRange(new object[] {
            resources.GetString("multipleItemsSplitterComboBox.Items"),
            resources.GetString("multipleItemsSplitterComboBox.Items1"),
            resources.GetString("multipleItemsSplitterComboBox.Items2"),
            resources.GetString("multipleItemsSplitterComboBox.Items3")});
            this.multipleItemsSplitterComboBox.Name = "multipleItemsSplitterComboBox";
            this.multipleItemsSplitterComboBox.Tag = "#multipleItemsSplitterTrimCheckBox";
            this.toolTip1.SetToolTip(this.multipleItemsSplitterComboBox, resources.GetString("multipleItemsSplitterComboBox.ToolTip"));
            this.multipleItemsSplitterComboBox.SelectedIndexChanged += new System.EventHandler(this.multipleItemsSplitterComboBox_SelectedIndexChanged);
            this.multipleItemsSplitterComboBox.TextUpdate += new System.EventHandler(this.multipleItemsSplitterComboBox_TextUpdate);
            // 
            // multipleItemsSplitterLabel
            // 
            resources.ApplyResources(this.multipleItemsSplitterLabel, "multipleItemsSplitterLabel");
            this.dirtyErrorProvider.SetError(this.multipleItemsSplitterLabel, resources.GetString("multipleItemsSplitterLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.multipleItemsSplitterLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("multipleItemsSplitterLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.multipleItemsSplitterLabel, ((int)(resources.GetObject("multipleItemsSplitterLabel.IconPadding"))));
            this.multipleItemsSplitterLabel.Name = "multipleItemsSplitterLabel";
            this.multipleItemsSplitterLabel.Tag = "#multipleItemsSplitterComboBox";
            this.toolTip1.SetToolTip(this.multipleItemsSplitterLabel, resources.GetString("multipleItemsSplitterLabel.ToolTip"));
            // 
            // parameter2ComboBox
            // 
            resources.ApplyResources(this.parameter2ComboBox, "parameter2ComboBox");
            this.parameter2ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameter2ComboBox.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.parameter2ComboBox, resources.GetString("parameter2ComboBox.Error"));
            this.parameter2ComboBox.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.parameter2ComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("parameter2ComboBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.parameter2ComboBox, ((int)(resources.GetObject("parameter2ComboBox.IconPadding"))));
            this.parameter2ComboBox.Name = "parameter2ComboBox";
            this.toolTip1.SetToolTip(this.parameter2ComboBox, resources.GetString("parameter2ComboBox.ToolTip"));
            this.parameter2ComboBox.SelectedIndexChanged += new System.EventHandler(this.parameter2ComboBox_SelectedIndexChanged);
            // 
            // parameter2Label
            // 
            resources.ApplyResources(this.parameter2Label, "parameter2Label");
            this.dirtyErrorProvider.SetError(this.parameter2Label, resources.GetString("parameter2Label.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.parameter2Label, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("parameter2Label.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.parameter2Label, ((int)(resources.GetObject("parameter2Label.IconPadding"))));
            this.parameter2Label.Name = "parameter2Label";
            this.parameter2Label.Tag = "#parameter2ComboBox";
            this.toolTip1.SetToolTip(this.parameter2Label, resources.GetString("parameter2Label.ToolTip"));
            // 
            // sourceTagList
            // 
            resources.ApplyResources(this.sourceTagList, "sourceTagList");
            this.sourceTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceTagList.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.sourceTagList, resources.GetString("sourceTagList.Error"));
            this.sourceTagList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.sourceTagList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("sourceTagList.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.sourceTagList, ((int)(resources.GetObject("sourceTagList.IconPadding"))));
            this.sourceTagList.Name = "sourceTagList";
            this.sourceTagList.Tag = "#parameter2Label";
            this.toolTip1.SetToolTip(this.sourceTagList, resources.GetString("sourceTagList.ToolTip"));
            this.sourceTagList.SelectedIndexChanged += new System.EventHandler(this.sourceTagList_SelectedIndexChanged);
            // 
            // forTagLabel
            // 
            resources.ApplyResources(this.forTagLabel, "forTagLabel");
            this.dirtyErrorProvider.SetError(this.forTagLabel, resources.GetString("forTagLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.forTagLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("forTagLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.forTagLabel, ((int)(resources.GetObject("forTagLabel.IconPadding"))));
            this.forTagLabel.Name = "forTagLabel";
            this.forTagLabel.Tag = "#sourceTagList";
            this.toolTip1.SetToolTip(this.forTagLabel, resources.GetString("forTagLabel.ToolTip"));
            // 
            // functionComboBox
            // 
            resources.ApplyResources(this.functionComboBox, "functionComboBox");
            this.functionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.functionComboBox.DropDownWidth = 110;
            this.dirtyErrorProvider.SetError(this.functionComboBox, resources.GetString("functionComboBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.functionComboBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("functionComboBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.functionComboBox, ((int)(resources.GetObject("functionComboBox.IconPadding"))));
            this.functionComboBox.Name = "functionComboBox";
            this.functionComboBox.Tag = "#forTagLabel";
            this.toolTip1.SetToolTip(this.functionComboBox, resources.GetString("functionComboBox.ToolTip"));
            this.functionComboBox.SelectedIndexChanged += new System.EventHandler(this.functionComboBox_SelectedIndexChanged);
            // 
            // labelFunction
            // 
            resources.ApplyResources(this.labelFunction, "labelFunction");
            this.dirtyErrorProvider.SetError(this.labelFunction, resources.GetString("labelFunction.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.labelFunction, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelFunction.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelFunction, ((int)(resources.GetObject("labelFunction.IconPadding"))));
            this.labelFunction.Name = "labelFunction";
            this.labelFunction.Tag = "#functionComboBox";
            this.toolTip1.SetToolTip(this.labelFunction, resources.GetString("labelFunction.ToolTip"));
            // 
            // buttonsPanel
            // 
            resources.ApplyResources(this.buttonsPanel, "buttonsPanel");
            this.buttonsPanel.Controls.Add(this.buttonUpdateFunction);
            this.buttonsPanel.Controls.Add(this.buttonAddFunction);
            this.buttonsPanel.Controls.Add(this.buttonHelp);
            this.buttonsPanel.Controls.Add(this.buttonClearExpression);
            this.dirtyErrorProvider.SetError(this.buttonsPanel, resources.GetString("buttonsPanel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonsPanel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonsPanel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonsPanel, ((int)(resources.GetObject("buttonsPanel.IconPadding"))));
            this.buttonsPanel.Name = "buttonsPanel";
            this.buttonsPanel.Tag = "#tabPage2";
            this.toolTip1.SetToolTip(this.buttonsPanel, resources.GetString("buttonsPanel.ToolTip"));
            // 
            // buttonUpdateFunction
            // 
            resources.ApplyResources(this.buttonUpdateFunction, "buttonUpdateFunction");
            this.dirtyErrorProvider.SetError(this.buttonUpdateFunction, resources.GetString("buttonUpdateFunction.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonUpdateFunction, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonUpdateFunction.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonUpdateFunction, ((int)(resources.GetObject("buttonUpdateFunction.IconPadding"))));
            this.buttonUpdateFunction.Image = global::MusicBeePlugin.Properties.Resources.warning_wide_15;
            this.buttonUpdateFunction.Name = "buttonUpdateFunction";
            this.buttonUpdateFunction.Tag = "#buttonsPanel@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonUpdateFunction, resources.GetString("buttonUpdateFunction.ToolTip"));
            this.buttonUpdateFunction.Click += new System.EventHandler(this.buttonUpdateFunction_Click);
            // 
            // buttonAddFunction
            // 
            resources.ApplyResources(this.buttonAddFunction, "buttonAddFunction");
            this.dirtyErrorProvider.SetError(this.buttonAddFunction, resources.GetString("buttonAddFunction.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonAddFunction, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonAddFunction.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonAddFunction, ((int)(resources.GetObject("buttonAddFunction.IconPadding"))));
            this.buttonAddFunction.Name = "buttonAddFunction";
            this.buttonAddFunction.Tag = "#buttonUpdateFunction@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonAddFunction, resources.GetString("buttonAddFunction.ToolTip"));
            this.buttonAddFunction.Click += new System.EventHandler(this.buttonAddFunction_Click);
            // 
            // buttonHelp
            // 
            resources.ApplyResources(this.buttonHelp, "buttonHelp");
            this.dirtyErrorProvider.SetError(this.buttonHelp, resources.GetString("buttonHelp.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonHelp, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonHelp.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonHelp, ((int)(resources.GetObject("buttonHelp.IconPadding"))));
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Tag = "@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonHelp, resources.GetString("buttonHelp.ToolTip"));
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // buttonClearExpression
            // 
            resources.ApplyResources(this.buttonClearExpression, "buttonClearExpression");
            this.dirtyErrorProvider.SetError(this.buttonClearExpression, resources.GetString("buttonClearExpression.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonClearExpression, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonClearExpression.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonClearExpression, ((int)(resources.GetObject("buttonClearExpression.IconPadding"))));
            this.buttonClearExpression.Name = "buttonClearExpression";
            this.buttonClearExpression.Tag = "@pinned-to-parent-x@non-defaultable@square-button";
            this.toolTip1.SetToolTip(this.buttonClearExpression, resources.GetString("buttonClearExpression.ToolTip"));
            this.buttonClearExpression.Click += new System.EventHandler(this.buttonClearExpression_Click);
            // 
            // autoApplyPresetsLabel
            // 
            resources.ApplyResources(this.autoApplyPresetsLabel, "autoApplyPresetsLabel");
            this.autoApplyPresetsLabel.AutoEllipsis = true;
            this.dirtyErrorProvider.SetError(this.autoApplyPresetsLabel, resources.GetString("autoApplyPresetsLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.autoApplyPresetsLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("autoApplyPresetsLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.autoApplyPresetsLabel, ((int)(resources.GetObject("autoApplyPresetsLabel.IconPadding"))));
            this.autoApplyPresetsLabel.Name = "autoApplyPresetsLabel";
            this.autoApplyPresetsLabel.Tag = "#LibraryReports&splitContainer1@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.autoApplyPresetsLabel, resources.GetString("autoApplyPresetsLabel.ToolTip"));
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
            this.DoubleBuffered = true;
            this.Name = "LibraryReports";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
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
        private Label hidePreviewCheckBoxLabel;
        private CheckBox hidePreviewCheckBox;
    }
}