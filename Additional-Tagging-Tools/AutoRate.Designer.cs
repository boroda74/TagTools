using System.Windows.Forms;

namespace MusicBeePlugin
{
    partial class AutoRateCommand
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoRateCommand));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.autoRatingTagList = new System.Windows.Forms.ComboBox();
            this.autoRateAtStartUpCheckBox = new System.Windows.Forms.CheckBox();
            this.autoRateOnTrackPropertiesCheckBox = new System.Windows.Forms.CheckBox();
            this.baseRatingTrackBar = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.threshold05Box = new System.Windows.Forms.TextBox();
            this.threshold1Box = new System.Windows.Forms.TextBox();
            this.threshold15Box = new System.Windows.Forms.TextBox();
            this.threshold2Box = new System.Windows.Forms.TextBox();
            this.threshold25Box = new System.Windows.Forms.TextBox();
            this.threshold3Box = new System.Windows.Forms.TextBox();
            this.threshold35Box = new System.Windows.Forms.TextBox();
            this.threshold4Box = new System.Windows.Forms.TextBox();
            this.threshold45Box = new System.Windows.Forms.TextBox();
            this.threshold5Box = new System.Windows.Forms.TextBox();
            this.LabelFive = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.maxPlaysPerDayBox = new System.Windows.Forms.TextBox();
            this.avgPlaysPerDayBox = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox45 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox35 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox25 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox15 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox05 = new System.Windows.Forms.CheckBox();
            this.perCent5UpDown = new System.Windows.Forms.NumericUpDown();
            this.perCentLabel5 = new System.Windows.Forms.Label();
            this.perCentLabel45 = new System.Windows.Forms.Label();
            this.perCent45UpDown = new System.Windows.Forms.NumericUpDown();
            this.perCentLabel4 = new System.Windows.Forms.Label();
            this.perCent4UpDown = new System.Windows.Forms.NumericUpDown();
            this.perCentLabel35 = new System.Windows.Forms.Label();
            this.perCent35UpDown = new System.Windows.Forms.NumericUpDown();
            this.perCentLabel3 = new System.Windows.Forms.Label();
            this.perCent3UpDown = new System.Windows.Forms.NumericUpDown();
            this.perCentLabel25 = new System.Windows.Forms.Label();
            this.perCent25UpDown = new System.Windows.Forms.NumericUpDown();
            this.perCentLabel2 = new System.Windows.Forms.Label();
            this.perCent2UpDown = new System.Windows.Forms.NumericUpDown();
            this.perCentLabel15 = new System.Windows.Forms.Label();
            this.perCent15UpDown = new System.Windows.Forms.NumericUpDown();
            this.perCentLabel1 = new System.Windows.Forms.Label();
            this.perCent1UpDown = new System.Windows.Forms.NumericUpDown();
            this.perCentLabel05 = new System.Windows.Forms.Label();
            this.perCent05UpDown = new System.Windows.Forms.NumericUpDown();
            this.buttonCalculate = new System.Windows.Forms.Button();
            this.labelSum = new System.Windows.Forms.Label();
            this.calculateThresholdsAtStartUpCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelTotalTracks = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox05Label = new System.Windows.Forms.Label();
            this.checkBox1Label = new System.Windows.Forms.Label();
            this.checkBox15Label = new System.Windows.Forms.Label();
            this.checkBox2Label = new System.Windows.Forms.Label();
            this.checkBox25Label = new System.Windows.Forms.Label();
            this.checkBox3Label = new System.Windows.Forms.Label();
            this.checkBox35Label = new System.Windows.Forms.Label();
            this.checkBox4Label = new System.Windows.Forms.Label();
            this.checkBox45Label = new System.Windows.Forms.Label();
            this.checkBox5Label = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.sinceAddedCheckBox = new System.Windows.Forms.CheckBox();
            this.sinceAddedCheckBoxLabel = new System.Windows.Forms.Label();
            this.notifyWhenAutoratingCompletedCheckBox = new System.Windows.Forms.CheckBox();
            this.playsPerDayTagList = new System.Windows.Forms.ComboBox();
            this.storePlaysPerDayCheckBox = new System.Windows.Forms.CheckBox();
            this.storePlaysPerDayCheckBoxLabel = new System.Windows.Forms.Label();
            this.autoRateAtStartUpCheckBoxLabel = new System.Windows.Forms.Label();
            this.notifyWhenAutoratingCompletedCheckBoxLabel = new System.Windows.Forms.Label();
            this.autoRateOnTrackPropertiesCheckBoxLabel = new System.Windows.Forms.Label();
            this.holdsAtStartUpCheckBoxLabel = new System.Windows.Forms.Label();
            this.dirtyErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.baseRatingTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.perCent5UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.perCent45UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.perCent4UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.perCent35UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.perCent3UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.perCent25UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.perCent2UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.perCent15UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.perCent1UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.perCent05UpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dirtyErrorProvider.SetError(this.buttonCancel, resources.GetString("buttonCancel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonCancel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonCancel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonCancel, ((int)(resources.GetObject("buttonCancel.IconPadding"))));
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Tag = "#AutoRateCommand@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonCancel, resources.GetString("buttonCancel.ToolTip"));
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.dirtyErrorProvider.SetError(this.buttonOK, resources.GetString("buttonOK.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonOK, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonOK.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonOK, ((int)(resources.GetObject("buttonOK.IconPadding"))));
            this.buttonOK.Name = "buttonOK";
            this.toolTip1.SetToolTip(this.buttonOK, resources.GetString("buttonOK.ToolTip"));
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
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
            // autoRatingTagList
            // 
            resources.ApplyResources(this.autoRatingTagList, "autoRatingTagList");
            this.autoRatingTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.autoRatingTagList.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.autoRatingTagList, resources.GetString("autoRatingTagList.Error"));
            this.autoRatingTagList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.autoRatingTagList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("autoRatingTagList.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.autoRatingTagList, ((int)(resources.GetObject("autoRatingTagList.IconPadding"))));
            this.autoRatingTagList.Name = "autoRatingTagList";
            this.toolTip1.SetToolTip(this.autoRatingTagList, resources.GetString("autoRatingTagList.ToolTip"));
            // 
            // autoRateAtStartUpCheckBox
            // 
            resources.ApplyResources(this.autoRateAtStartUpCheckBox, "autoRateAtStartUpCheckBox");
            this.autoRateAtStartUpCheckBox.Checked = true;
            this.autoRateAtStartUpCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dirtyErrorProvider.SetError(this.autoRateAtStartUpCheckBox, resources.GetString("autoRateAtStartUpCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.autoRateAtStartUpCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("autoRateAtStartUpCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.autoRateAtStartUpCheckBox, ((int)(resources.GetObject("autoRateAtStartUpCheckBox.IconPadding"))));
            this.autoRateAtStartUpCheckBox.Name = "autoRateAtStartUpCheckBox";
            this.autoRateAtStartUpCheckBox.Tag = "#autoRateAtStartUpCheckBoxLabel";
            this.toolTip1.SetToolTip(this.autoRateAtStartUpCheckBox, resources.GetString("autoRateAtStartUpCheckBox.ToolTip"));
            this.autoRateAtStartUpCheckBox.CheckedChanged += new System.EventHandler(this.autoRateAtStartUp_CheckedChanged);
            // 
            // autoRateOnTrackPropertiesCheckBox
            // 
            resources.ApplyResources(this.autoRateOnTrackPropertiesCheckBox, "autoRateOnTrackPropertiesCheckBox");
            this.dirtyErrorProvider.SetError(this.autoRateOnTrackPropertiesCheckBox, resources.GetString("autoRateOnTrackPropertiesCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.autoRateOnTrackPropertiesCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("autoRateOnTrackPropertiesCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.autoRateOnTrackPropertiesCheckBox, ((int)(resources.GetObject("autoRateOnTrackPropertiesCheckBox.IconPadding"))));
            this.autoRateOnTrackPropertiesCheckBox.Name = "autoRateOnTrackPropertiesCheckBox";
            this.autoRateOnTrackPropertiesCheckBox.Tag = "#autoRateOnTrackPropertiesCheckBoxLabel";
            this.toolTip1.SetToolTip(this.autoRateOnTrackPropertiesCheckBox, resources.GetString("autoRateOnTrackPropertiesCheckBox.ToolTip"));
            // 
            // baseRatingTrackBar
            // 
            resources.ApplyResources(this.baseRatingTrackBar, "baseRatingTrackBar");
            this.dirtyErrorProvider.SetError(this.baseRatingTrackBar, resources.GetString("baseRatingTrackBar.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.baseRatingTrackBar, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("baseRatingTrackBar.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.baseRatingTrackBar, ((int)(resources.GetObject("baseRatingTrackBar.IconPadding"))));
            this.baseRatingTrackBar.Name = "baseRatingTrackBar";
            this.toolTip1.SetToolTip(this.baseRatingTrackBar, resources.GetString("baseRatingTrackBar.ToolTip"));
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.dirtyErrorProvider.SetError(this.label5, resources.GetString("label5.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label5.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label5, ((int)(resources.GetObject("label5.IconPadding"))));
            this.label5.Name = "label5";
            this.toolTip1.SetToolTip(this.label5, resources.GetString("label5.ToolTip"));
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
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.dirtyErrorProvider.SetError(this.label7, resources.GetString("label7.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label7, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label7.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label7, ((int)(resources.GetObject("label7.IconPadding"))));
            this.label7.Name = "label7";
            this.toolTip1.SetToolTip(this.label7, resources.GetString("label7.ToolTip"));
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.dirtyErrorProvider.SetError(this.label8, resources.GetString("label8.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label8, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label8.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label8, ((int)(resources.GetObject("label8.IconPadding"))));
            this.label8.Name = "label8";
            this.toolTip1.SetToolTip(this.label8, resources.GetString("label8.ToolTip"));
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.dirtyErrorProvider.SetError(this.label9, resources.GetString("label9.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label9, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label9.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label9, ((int)(resources.GetObject("label9.IconPadding"))));
            this.label9.Name = "label9";
            this.toolTip1.SetToolTip(this.label9, resources.GetString("label9.ToolTip"));
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.dirtyErrorProvider.SetError(this.label10, resources.GetString("label10.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label10, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label10.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label10, ((int)(resources.GetObject("label10.IconPadding"))));
            this.label10.Name = "label10";
            this.toolTip1.SetToolTip(this.label10, resources.GetString("label10.ToolTip"));
            // 
            // buttonSave
            // 
            resources.ApplyResources(this.buttonSave, "buttonSave");
            this.dirtyErrorProvider.SetError(this.buttonSave, resources.GetString("buttonSave.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonSave, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonSave.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonSave, ((int)(resources.GetObject("buttonSave.IconPadding"))));
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Tag = "@non-defaultable";
            this.toolTip1.SetToolTip(this.buttonSave, resources.GetString("buttonSave.ToolTip"));
            this.buttonSave.Click += new System.EventHandler(this.button1_Click);
            // 
            // threshold05Box
            // 
            resources.ApplyResources(this.threshold05Box, "threshold05Box");
            this.dirtyErrorProvider.SetError(this.threshold05Box, resources.GetString("threshold05Box.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.threshold05Box, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("threshold05Box.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.threshold05Box, ((int)(resources.GetObject("threshold05Box.IconPadding"))));
            this.threshold05Box.Name = "threshold05Box";
            this.threshold05Box.Tag = "#label28";
            this.toolTip1.SetToolTip(this.threshold05Box, resources.GetString("threshold05Box.ToolTip"));
            this.threshold05Box.Leave += new System.EventHandler(this.threshold_TextChanged);
            // 
            // threshold1Box
            // 
            resources.ApplyResources(this.threshold1Box, "threshold1Box");
            this.dirtyErrorProvider.SetError(this.threshold1Box, resources.GetString("threshold1Box.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.threshold1Box, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("threshold1Box.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.threshold1Box, ((int)(resources.GetObject("threshold1Box.IconPadding"))));
            this.threshold1Box.Name = "threshold1Box";
            this.threshold1Box.Tag = "#label27";
            this.toolTip1.SetToolTip(this.threshold1Box, resources.GetString("threshold1Box.ToolTip"));
            this.threshold1Box.Leave += new System.EventHandler(this.threshold_TextChanged);
            // 
            // threshold15Box
            // 
            resources.ApplyResources(this.threshold15Box, "threshold15Box");
            this.dirtyErrorProvider.SetError(this.threshold15Box, resources.GetString("threshold15Box.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.threshold15Box, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("threshold15Box.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.threshold15Box, ((int)(resources.GetObject("threshold15Box.IconPadding"))));
            this.threshold15Box.Name = "threshold15Box";
            this.threshold15Box.Tag = "#label26";
            this.toolTip1.SetToolTip(this.threshold15Box, resources.GetString("threshold15Box.ToolTip"));
            this.threshold15Box.Leave += new System.EventHandler(this.threshold_TextChanged);
            // 
            // threshold2Box
            // 
            resources.ApplyResources(this.threshold2Box, "threshold2Box");
            this.dirtyErrorProvider.SetError(this.threshold2Box, resources.GetString("threshold2Box.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.threshold2Box, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("threshold2Box.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.threshold2Box, ((int)(resources.GetObject("threshold2Box.IconPadding"))));
            this.threshold2Box.Name = "threshold2Box";
            this.threshold2Box.Tag = "#label25";
            this.toolTip1.SetToolTip(this.threshold2Box, resources.GetString("threshold2Box.ToolTip"));
            this.threshold2Box.Leave += new System.EventHandler(this.threshold_TextChanged);
            // 
            // threshold25Box
            // 
            resources.ApplyResources(this.threshold25Box, "threshold25Box");
            this.dirtyErrorProvider.SetError(this.threshold25Box, resources.GetString("threshold25Box.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.threshold25Box, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("threshold25Box.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.threshold25Box, ((int)(resources.GetObject("threshold25Box.IconPadding"))));
            this.threshold25Box.Name = "threshold25Box";
            this.threshold25Box.Tag = "#label24";
            this.toolTip1.SetToolTip(this.threshold25Box, resources.GetString("threshold25Box.ToolTip"));
            this.threshold25Box.Leave += new System.EventHandler(this.threshold_TextChanged);
            // 
            // threshold3Box
            // 
            resources.ApplyResources(this.threshold3Box, "threshold3Box");
            this.dirtyErrorProvider.SetError(this.threshold3Box, resources.GetString("threshold3Box.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.threshold3Box, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("threshold3Box.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.threshold3Box, ((int)(resources.GetObject("threshold3Box.IconPadding"))));
            this.threshold3Box.Name = "threshold3Box";
            this.threshold3Box.Tag = "#label23";
            this.toolTip1.SetToolTip(this.threshold3Box, resources.GetString("threshold3Box.ToolTip"));
            this.threshold3Box.Leave += new System.EventHandler(this.threshold_TextChanged);
            // 
            // threshold35Box
            // 
            resources.ApplyResources(this.threshold35Box, "threshold35Box");
            this.dirtyErrorProvider.SetError(this.threshold35Box, resources.GetString("threshold35Box.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.threshold35Box, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("threshold35Box.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.threshold35Box, ((int)(resources.GetObject("threshold35Box.IconPadding"))));
            this.threshold35Box.Name = "threshold35Box";
            this.threshold35Box.Tag = "#label22";
            this.toolTip1.SetToolTip(this.threshold35Box, resources.GetString("threshold35Box.ToolTip"));
            this.threshold35Box.Leave += new System.EventHandler(this.threshold_TextChanged);
            // 
            // threshold4Box
            // 
            resources.ApplyResources(this.threshold4Box, "threshold4Box");
            this.dirtyErrorProvider.SetError(this.threshold4Box, resources.GetString("threshold4Box.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.threshold4Box, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("threshold4Box.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.threshold4Box, ((int)(resources.GetObject("threshold4Box.IconPadding"))));
            this.threshold4Box.Name = "threshold4Box";
            this.threshold4Box.Tag = "#label21";
            this.toolTip1.SetToolTip(this.threshold4Box, resources.GetString("threshold4Box.ToolTip"));
            this.threshold4Box.Leave += new System.EventHandler(this.threshold_TextChanged);
            // 
            // threshold45Box
            // 
            resources.ApplyResources(this.threshold45Box, "threshold45Box");
            this.dirtyErrorProvider.SetError(this.threshold45Box, resources.GetString("threshold45Box.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.threshold45Box, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("threshold45Box.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.threshold45Box, ((int)(resources.GetObject("threshold45Box.IconPadding"))));
            this.threshold45Box.Name = "threshold45Box";
            this.threshold45Box.Tag = "#label20";
            this.toolTip1.SetToolTip(this.threshold45Box, resources.GetString("threshold45Box.ToolTip"));
            this.threshold45Box.Leave += new System.EventHandler(this.threshold_TextChanged);
            // 
            // threshold5Box
            // 
            resources.ApplyResources(this.threshold5Box, "threshold5Box");
            this.dirtyErrorProvider.SetError(this.threshold5Box, resources.GetString("threshold5Box.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.threshold5Box, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("threshold5Box.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.threshold5Box, ((int)(resources.GetObject("threshold5Box.IconPadding"))));
            this.threshold5Box.Name = "threshold5Box";
            this.threshold5Box.Tag = "#LabelFive";
            this.toolTip1.SetToolTip(this.threshold5Box, resources.GetString("threshold5Box.ToolTip"));
            this.threshold5Box.Leave += new System.EventHandler(this.threshold_TextChanged);
            // 
            // LabelFive
            // 
            resources.ApplyResources(this.LabelFive, "LabelFive");
            this.dirtyErrorProvider.SetError(this.LabelFive, resources.GetString("LabelFive.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.LabelFive, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("LabelFive.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.LabelFive, ((int)(resources.GetObject("LabelFive.IconPadding"))));
            this.LabelFive.Name = "LabelFive";
            this.LabelFive.Tag = "#groupBox2";
            this.toolTip1.SetToolTip(this.LabelFive, resources.GetString("LabelFive.ToolTip"));
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.dirtyErrorProvider.SetError(this.label19, resources.GetString("label19.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label19, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label19.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label19, ((int)(resources.GetObject("label19.IconPadding"))));
            this.label19.Name = "label19";
            this.toolTip1.SetToolTip(this.label19, resources.GetString("label19.ToolTip"));
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.dirtyErrorProvider.SetError(this.label20, resources.GetString("label20.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label20, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label20.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label20, ((int)(resources.GetObject("label20.IconPadding"))));
            this.label20.Name = "label20";
            this.label20.Tag = "#groupBox2";
            this.toolTip1.SetToolTip(this.label20, resources.GetString("label20.ToolTip"));
            // 
            // label21
            // 
            resources.ApplyResources(this.label21, "label21");
            this.dirtyErrorProvider.SetError(this.label21, resources.GetString("label21.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label21, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label21.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label21, ((int)(resources.GetObject("label21.IconPadding"))));
            this.label21.Name = "label21";
            this.label21.Tag = "#groupBox2";
            this.toolTip1.SetToolTip(this.label21, resources.GetString("label21.ToolTip"));
            // 
            // label22
            // 
            resources.ApplyResources(this.label22, "label22");
            this.dirtyErrorProvider.SetError(this.label22, resources.GetString("label22.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label22, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label22.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label22, ((int)(resources.GetObject("label22.IconPadding"))));
            this.label22.Name = "label22";
            this.label22.Tag = "#groupBox2";
            this.toolTip1.SetToolTip(this.label22, resources.GetString("label22.ToolTip"));
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.dirtyErrorProvider.SetError(this.label23, resources.GetString("label23.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label23, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label23.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label23, ((int)(resources.GetObject("label23.IconPadding"))));
            this.label23.Name = "label23";
            this.label23.Tag = "#groupBox2";
            this.toolTip1.SetToolTip(this.label23, resources.GetString("label23.ToolTip"));
            // 
            // label24
            // 
            resources.ApplyResources(this.label24, "label24");
            this.dirtyErrorProvider.SetError(this.label24, resources.GetString("label24.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label24, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label24.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label24, ((int)(resources.GetObject("label24.IconPadding"))));
            this.label24.Name = "label24";
            this.label24.Tag = "#groupBox2";
            this.toolTip1.SetToolTip(this.label24, resources.GetString("label24.ToolTip"));
            // 
            // label25
            // 
            resources.ApplyResources(this.label25, "label25");
            this.dirtyErrorProvider.SetError(this.label25, resources.GetString("label25.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label25, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label25.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label25, ((int)(resources.GetObject("label25.IconPadding"))));
            this.label25.Name = "label25";
            this.label25.Tag = "#groupBox2";
            this.toolTip1.SetToolTip(this.label25, resources.GetString("label25.ToolTip"));
            // 
            // label26
            // 
            resources.ApplyResources(this.label26, "label26");
            this.dirtyErrorProvider.SetError(this.label26, resources.GetString("label26.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label26, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label26.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label26, ((int)(resources.GetObject("label26.IconPadding"))));
            this.label26.Name = "label26";
            this.label26.Tag = "#groupBox2";
            this.toolTip1.SetToolTip(this.label26, resources.GetString("label26.ToolTip"));
            // 
            // label27
            // 
            resources.ApplyResources(this.label27, "label27");
            this.dirtyErrorProvider.SetError(this.label27, resources.GetString("label27.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label27, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label27.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label27, ((int)(resources.GetObject("label27.IconPadding"))));
            this.label27.Name = "label27";
            this.label27.Tag = "#groupBox2";
            this.toolTip1.SetToolTip(this.label27, resources.GetString("label27.ToolTip"));
            // 
            // label28
            // 
            resources.ApplyResources(this.label28, "label28");
            this.dirtyErrorProvider.SetError(this.label28, resources.GetString("label28.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label28, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label28.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label28, ((int)(resources.GetObject("label28.IconPadding"))));
            this.label28.Name = "label28";
            this.label28.Tag = "#groupBox2";
            this.toolTip1.SetToolTip(this.label28, resources.GetString("label28.ToolTip"));
            // 
            // label29
            // 
            resources.ApplyResources(this.label29, "label29");
            this.dirtyErrorProvider.SetError(this.label29, resources.GetString("label29.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label29, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label29.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label29, ((int)(resources.GetObject("label29.IconPadding"))));
            this.label29.Name = "label29";
            this.toolTip1.SetToolTip(this.label29, resources.GetString("label29.ToolTip"));
            // 
            // maxPlaysPerDayBox
            // 
            resources.ApplyResources(this.maxPlaysPerDayBox, "maxPlaysPerDayBox");
            this.dirtyErrorProvider.SetError(this.maxPlaysPerDayBox, resources.GetString("maxPlaysPerDayBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.maxPlaysPerDayBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("maxPlaysPerDayBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.maxPlaysPerDayBox, ((int)(resources.GetObject("maxPlaysPerDayBox.IconPadding"))));
            this.maxPlaysPerDayBox.Name = "maxPlaysPerDayBox";
            this.maxPlaysPerDayBox.ReadOnly = true;
            this.maxPlaysPerDayBox.Tag = "#label1";
            this.toolTip1.SetToolTip(this.maxPlaysPerDayBox, resources.GetString("maxPlaysPerDayBox.ToolTip"));
            // 
            // avgPlaysPerDayBox
            // 
            resources.ApplyResources(this.avgPlaysPerDayBox, "avgPlaysPerDayBox");
            this.dirtyErrorProvider.SetError(this.avgPlaysPerDayBox, resources.GetString("avgPlaysPerDayBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.avgPlaysPerDayBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("avgPlaysPerDayBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.avgPlaysPerDayBox, ((int)(resources.GetObject("avgPlaysPerDayBox.IconPadding"))));
            this.avgPlaysPerDayBox.Name = "avgPlaysPerDayBox";
            this.avgPlaysPerDayBox.ReadOnly = true;
            this.avgPlaysPerDayBox.Tag = "#label2";
            this.toolTip1.SetToolTip(this.avgPlaysPerDayBox, resources.GetString("avgPlaysPerDayBox.ToolTip"));
            // 
            // label30
            // 
            resources.ApplyResources(this.label30, "label30");
            this.dirtyErrorProvider.SetError(this.label30, resources.GetString("label30.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.label30, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label30.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label30, ((int)(resources.GetObject("label30.IconPadding"))));
            this.label30.Name = "label30";
            this.toolTip1.SetToolTip(this.label30, resources.GetString("label30.ToolTip"));
            // 
            // checkBox5
            // 
            resources.ApplyResources(this.checkBox5, "checkBox5");
            this.checkBox5.Checked = true;
            this.checkBox5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dirtyErrorProvider.SetError(this.checkBox5, resources.GetString("checkBox5.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.checkBox5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkBox5.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.checkBox5, ((int)(resources.GetObject("checkBox5.IconPadding"))));
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Tag = "#checkBox5Label";
            this.toolTip1.SetToolTip(this.checkBox5, resources.GetString("checkBox5.ToolTip"));
            this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBoxFive_CheckedChanged);
            // 
            // checkBox45
            // 
            resources.ApplyResources(this.checkBox45, "checkBox45");
            this.checkBox45.Checked = true;
            this.checkBox45.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dirtyErrorProvider.SetError(this.checkBox45, resources.GetString("checkBox45.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.checkBox45, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkBox45.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.checkBox45, ((int)(resources.GetObject("checkBox45.IconPadding"))));
            this.checkBox45.Name = "checkBox45";
            this.checkBox45.Tag = "#checkBox45Label";
            this.toolTip1.SetToolTip(this.checkBox45, resources.GetString("checkBox45.ToolTip"));
            this.checkBox45.CheckedChanged += new System.EventHandler(this.checkBoxFourAndHalf_CheckedChanged);
            // 
            // checkBox4
            // 
            resources.ApplyResources(this.checkBox4, "checkBox4");
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dirtyErrorProvider.SetError(this.checkBox4, resources.GetString("checkBox4.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.checkBox4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkBox4.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.checkBox4, ((int)(resources.GetObject("checkBox4.IconPadding"))));
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Tag = "#checkBox4Label";
            this.toolTip1.SetToolTip(this.checkBox4, resources.GetString("checkBox4.ToolTip"));
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBoxFour_CheckedChanged);
            // 
            // checkBox35
            // 
            resources.ApplyResources(this.checkBox35, "checkBox35");
            this.checkBox35.Checked = true;
            this.checkBox35.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dirtyErrorProvider.SetError(this.checkBox35, resources.GetString("checkBox35.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.checkBox35, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkBox35.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.checkBox35, ((int)(resources.GetObject("checkBox35.IconPadding"))));
            this.checkBox35.Name = "checkBox35";
            this.checkBox35.Tag = "#checkBox35Label";
            this.toolTip1.SetToolTip(this.checkBox35, resources.GetString("checkBox35.ToolTip"));
            this.checkBox35.CheckedChanged += new System.EventHandler(this.checkBoxThreeAndHalf_CheckedChanged);
            // 
            // checkBox3
            // 
            resources.ApplyResources(this.checkBox3, "checkBox3");
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dirtyErrorProvider.SetError(this.checkBox3, resources.GetString("checkBox3.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.checkBox3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkBox3.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.checkBox3, ((int)(resources.GetObject("checkBox3.IconPadding"))));
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Tag = "#checkBox3Label";
            this.toolTip1.SetToolTip(this.checkBox3, resources.GetString("checkBox3.ToolTip"));
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBoxThree_CheckedChanged);
            // 
            // checkBox25
            // 
            resources.ApplyResources(this.checkBox25, "checkBox25");
            this.checkBox25.Checked = true;
            this.checkBox25.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dirtyErrorProvider.SetError(this.checkBox25, resources.GetString("checkBox25.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.checkBox25, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkBox25.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.checkBox25, ((int)(resources.GetObject("checkBox25.IconPadding"))));
            this.checkBox25.Name = "checkBox25";
            this.checkBox25.Tag = "#checkBox25Label";
            this.toolTip1.SetToolTip(this.checkBox25, resources.GetString("checkBox25.ToolTip"));
            this.checkBox25.CheckedChanged += new System.EventHandler(this.checkBoxTwoAndHalf_CheckedChanged);
            // 
            // checkBox2
            // 
            resources.ApplyResources(this.checkBox2, "checkBox2");
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dirtyErrorProvider.SetError(this.checkBox2, resources.GetString("checkBox2.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.checkBox2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkBox2.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.checkBox2, ((int)(resources.GetObject("checkBox2.IconPadding"))));
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Tag = "#checkBox2Label";
            this.toolTip1.SetToolTip(this.checkBox2, resources.GetString("checkBox2.ToolTip"));
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBoxTwo_CheckedChanged);
            // 
            // checkBox15
            // 
            resources.ApplyResources(this.checkBox15, "checkBox15");
            this.checkBox15.Checked = true;
            this.checkBox15.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dirtyErrorProvider.SetError(this.checkBox15, resources.GetString("checkBox15.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.checkBox15, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkBox15.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.checkBox15, ((int)(resources.GetObject("checkBox15.IconPadding"))));
            this.checkBox15.Name = "checkBox15";
            this.checkBox15.Tag = "#checkBox15Label";
            this.toolTip1.SetToolTip(this.checkBox15, resources.GetString("checkBox15.ToolTip"));
            this.checkBox15.CheckedChanged += new System.EventHandler(this.checkBoxOneAndHalf_CheckedChanged);
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dirtyErrorProvider.SetError(this.checkBox1, resources.GetString("checkBox1.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.checkBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkBox1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.checkBox1, ((int)(resources.GetObject("checkBox1.IconPadding"))));
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Tag = "#checkBox1Label";
            this.toolTip1.SetToolTip(this.checkBox1, resources.GetString("checkBox1.ToolTip"));
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBoxOne_CheckedChanged);
            // 
            // checkBox05
            // 
            resources.ApplyResources(this.checkBox05, "checkBox05");
            this.checkBox05.Checked = true;
            this.checkBox05.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dirtyErrorProvider.SetError(this.checkBox05, resources.GetString("checkBox05.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.checkBox05, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkBox05.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.checkBox05, ((int)(resources.GetObject("checkBox05.IconPadding"))));
            this.checkBox05.Name = "checkBox05";
            this.checkBox05.Tag = "#checkBox05Label";
            this.toolTip1.SetToolTip(this.checkBox05, resources.GetString("checkBox05.ToolTip"));
            this.checkBox05.CheckedChanged += new System.EventHandler(this.checkBoxHalf_CheckedChanged);
            // 
            // perCent5UpDown
            // 
            resources.ApplyResources(this.perCent5UpDown, "perCent5UpDown");
            this.dirtyErrorProvider.SetError(this.perCent5UpDown, resources.GetString("perCent5UpDown.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.perCent5UpDown, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("perCent5UpDown.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.perCent5UpDown, ((int)(resources.GetObject("perCent5UpDown.IconPadding"))));
            this.perCent5UpDown.Name = "perCent5UpDown";
            this.toolTip1.SetToolTip(this.perCent5UpDown, resources.GetString("perCent5UpDown.ToolTip"));
            this.perCent5UpDown.ValueChanged += new System.EventHandler(this.perCent5_ValueChanged);
            this.perCent5UpDown.Leave += new System.EventHandler(this.perCent5_ValueChanged);
            // 
            // perCentLabel5
            // 
            resources.ApplyResources(this.perCentLabel5, "perCentLabel5");
            this.dirtyErrorProvider.SetError(this.perCentLabel5, resources.GetString("perCentLabel5.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.perCentLabel5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("perCentLabel5.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.perCentLabel5, ((int)(resources.GetObject("perCentLabel5.IconPadding"))));
            this.perCentLabel5.Name = "perCentLabel5";
            this.toolTip1.SetToolTip(this.perCentLabel5, resources.GetString("perCentLabel5.ToolTip"));
            // 
            // perCentLabel45
            // 
            resources.ApplyResources(this.perCentLabel45, "perCentLabel45");
            this.dirtyErrorProvider.SetError(this.perCentLabel45, resources.GetString("perCentLabel45.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.perCentLabel45, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("perCentLabel45.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.perCentLabel45, ((int)(resources.GetObject("perCentLabel45.IconPadding"))));
            this.perCentLabel45.Name = "perCentLabel45";
            this.toolTip1.SetToolTip(this.perCentLabel45, resources.GetString("perCentLabel45.ToolTip"));
            // 
            // perCent45UpDown
            // 
            resources.ApplyResources(this.perCent45UpDown, "perCent45UpDown");
            this.dirtyErrorProvider.SetError(this.perCent45UpDown, resources.GetString("perCent45UpDown.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.perCent45UpDown, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("perCent45UpDown.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.perCent45UpDown, ((int)(resources.GetObject("perCent45UpDown.IconPadding"))));
            this.perCent45UpDown.Name = "perCent45UpDown";
            this.toolTip1.SetToolTip(this.perCent45UpDown, resources.GetString("perCent45UpDown.ToolTip"));
            this.perCent45UpDown.ValueChanged += new System.EventHandler(this.perCent45_ValueChanged);
            this.perCent45UpDown.Leave += new System.EventHandler(this.perCent45_ValueChanged);
            // 
            // perCentLabel4
            // 
            resources.ApplyResources(this.perCentLabel4, "perCentLabel4");
            this.dirtyErrorProvider.SetError(this.perCentLabel4, resources.GetString("perCentLabel4.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.perCentLabel4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("perCentLabel4.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.perCentLabel4, ((int)(resources.GetObject("perCentLabel4.IconPadding"))));
            this.perCentLabel4.Name = "perCentLabel4";
            this.toolTip1.SetToolTip(this.perCentLabel4, resources.GetString("perCentLabel4.ToolTip"));
            // 
            // perCent4UpDown
            // 
            resources.ApplyResources(this.perCent4UpDown, "perCent4UpDown");
            this.dirtyErrorProvider.SetError(this.perCent4UpDown, resources.GetString("perCent4UpDown.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.perCent4UpDown, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("perCent4UpDown.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.perCent4UpDown, ((int)(resources.GetObject("perCent4UpDown.IconPadding"))));
            this.perCent4UpDown.Name = "perCent4UpDown";
            this.toolTip1.SetToolTip(this.perCent4UpDown, resources.GetString("perCent4UpDown.ToolTip"));
            this.perCent4UpDown.ValueChanged += new System.EventHandler(this.perCent4_ValueChanged);
            this.perCent4UpDown.Leave += new System.EventHandler(this.perCent4_ValueChanged);
            // 
            // perCentLabel35
            // 
            resources.ApplyResources(this.perCentLabel35, "perCentLabel35");
            this.dirtyErrorProvider.SetError(this.perCentLabel35, resources.GetString("perCentLabel35.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.perCentLabel35, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("perCentLabel35.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.perCentLabel35, ((int)(resources.GetObject("perCentLabel35.IconPadding"))));
            this.perCentLabel35.Name = "perCentLabel35";
            this.toolTip1.SetToolTip(this.perCentLabel35, resources.GetString("perCentLabel35.ToolTip"));
            // 
            // perCent35UpDown
            // 
            resources.ApplyResources(this.perCent35UpDown, "perCent35UpDown");
            this.dirtyErrorProvider.SetError(this.perCent35UpDown, resources.GetString("perCent35UpDown.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.perCent35UpDown, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("perCent35UpDown.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.perCent35UpDown, ((int)(resources.GetObject("perCent35UpDown.IconPadding"))));
            this.perCent35UpDown.Name = "perCent35UpDown";
            this.toolTip1.SetToolTip(this.perCent35UpDown, resources.GetString("perCent35UpDown.ToolTip"));
            this.perCent35UpDown.ValueChanged += new System.EventHandler(this.perCent35_ValueChanged);
            this.perCent35UpDown.Leave += new System.EventHandler(this.perCent35_ValueChanged);
            // 
            // perCentLabel3
            // 
            resources.ApplyResources(this.perCentLabel3, "perCentLabel3");
            this.dirtyErrorProvider.SetError(this.perCentLabel3, resources.GetString("perCentLabel3.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.perCentLabel3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("perCentLabel3.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.perCentLabel3, ((int)(resources.GetObject("perCentLabel3.IconPadding"))));
            this.perCentLabel3.Name = "perCentLabel3";
            this.toolTip1.SetToolTip(this.perCentLabel3, resources.GetString("perCentLabel3.ToolTip"));
            // 
            // perCent3UpDown
            // 
            resources.ApplyResources(this.perCent3UpDown, "perCent3UpDown");
            this.dirtyErrorProvider.SetError(this.perCent3UpDown, resources.GetString("perCent3UpDown.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.perCent3UpDown, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("perCent3UpDown.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.perCent3UpDown, ((int)(resources.GetObject("perCent3UpDown.IconPadding"))));
            this.perCent3UpDown.Name = "perCent3UpDown";
            this.toolTip1.SetToolTip(this.perCent3UpDown, resources.GetString("perCent3UpDown.ToolTip"));
            this.perCent3UpDown.ValueChanged += new System.EventHandler(this.perCent3_ValueChanged);
            this.perCent3UpDown.Leave += new System.EventHandler(this.perCent3_ValueChanged);
            // 
            // perCentLabel25
            // 
            resources.ApplyResources(this.perCentLabel25, "perCentLabel25");
            this.dirtyErrorProvider.SetError(this.perCentLabel25, resources.GetString("perCentLabel25.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.perCentLabel25, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("perCentLabel25.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.perCentLabel25, ((int)(resources.GetObject("perCentLabel25.IconPadding"))));
            this.perCentLabel25.Name = "perCentLabel25";
            this.toolTip1.SetToolTip(this.perCentLabel25, resources.GetString("perCentLabel25.ToolTip"));
            // 
            // perCent25UpDown
            // 
            resources.ApplyResources(this.perCent25UpDown, "perCent25UpDown");
            this.dirtyErrorProvider.SetError(this.perCent25UpDown, resources.GetString("perCent25UpDown.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.perCent25UpDown, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("perCent25UpDown.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.perCent25UpDown, ((int)(resources.GetObject("perCent25UpDown.IconPadding"))));
            this.perCent25UpDown.Name = "perCent25UpDown";
            this.toolTip1.SetToolTip(this.perCent25UpDown, resources.GetString("perCent25UpDown.ToolTip"));
            this.perCent25UpDown.ValueChanged += new System.EventHandler(this.perCent25_ValueChanged);
            this.perCent25UpDown.Leave += new System.EventHandler(this.perCent25_ValueChanged);
            // 
            // perCentLabel2
            // 
            resources.ApplyResources(this.perCentLabel2, "perCentLabel2");
            this.dirtyErrorProvider.SetError(this.perCentLabel2, resources.GetString("perCentLabel2.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.perCentLabel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("perCentLabel2.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.perCentLabel2, ((int)(resources.GetObject("perCentLabel2.IconPadding"))));
            this.perCentLabel2.Name = "perCentLabel2";
            this.toolTip1.SetToolTip(this.perCentLabel2, resources.GetString("perCentLabel2.ToolTip"));
            // 
            // perCent2UpDown
            // 
            resources.ApplyResources(this.perCent2UpDown, "perCent2UpDown");
            this.dirtyErrorProvider.SetError(this.perCent2UpDown, resources.GetString("perCent2UpDown.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.perCent2UpDown, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("perCent2UpDown.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.perCent2UpDown, ((int)(resources.GetObject("perCent2UpDown.IconPadding"))));
            this.perCent2UpDown.Name = "perCent2UpDown";
            this.toolTip1.SetToolTip(this.perCent2UpDown, resources.GetString("perCent2UpDown.ToolTip"));
            this.perCent2UpDown.ValueChanged += new System.EventHandler(this.perCent2_ValueChanged);
            this.perCent2UpDown.Leave += new System.EventHandler(this.perCent2_ValueChanged);
            // 
            // perCentLabel15
            // 
            resources.ApplyResources(this.perCentLabel15, "perCentLabel15");
            this.dirtyErrorProvider.SetError(this.perCentLabel15, resources.GetString("perCentLabel15.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.perCentLabel15, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("perCentLabel15.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.perCentLabel15, ((int)(resources.GetObject("perCentLabel15.IconPadding"))));
            this.perCentLabel15.Name = "perCentLabel15";
            this.toolTip1.SetToolTip(this.perCentLabel15, resources.GetString("perCentLabel15.ToolTip"));
            // 
            // perCent15UpDown
            // 
            resources.ApplyResources(this.perCent15UpDown, "perCent15UpDown");
            this.dirtyErrorProvider.SetError(this.perCent15UpDown, resources.GetString("perCent15UpDown.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.perCent15UpDown, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("perCent15UpDown.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.perCent15UpDown, ((int)(resources.GetObject("perCent15UpDown.IconPadding"))));
            this.perCent15UpDown.Name = "perCent15UpDown";
            this.toolTip1.SetToolTip(this.perCent15UpDown, resources.GetString("perCent15UpDown.ToolTip"));
            this.perCent15UpDown.ValueChanged += new System.EventHandler(this.perCent15_ValueChanged);
            this.perCent15UpDown.Leave += new System.EventHandler(this.perCent15_ValueChanged);
            // 
            // perCentLabel1
            // 
            resources.ApplyResources(this.perCentLabel1, "perCentLabel1");
            this.dirtyErrorProvider.SetError(this.perCentLabel1, resources.GetString("perCentLabel1.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.perCentLabel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("perCentLabel1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.perCentLabel1, ((int)(resources.GetObject("perCentLabel1.IconPadding"))));
            this.perCentLabel1.Name = "perCentLabel1";
            this.toolTip1.SetToolTip(this.perCentLabel1, resources.GetString("perCentLabel1.ToolTip"));
            // 
            // perCent1UpDown
            // 
            resources.ApplyResources(this.perCent1UpDown, "perCent1UpDown");
            this.dirtyErrorProvider.SetError(this.perCent1UpDown, resources.GetString("perCent1UpDown.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.perCent1UpDown, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("perCent1UpDown.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.perCent1UpDown, ((int)(resources.GetObject("perCent1UpDown.IconPadding"))));
            this.perCent1UpDown.Name = "perCent1UpDown";
            this.toolTip1.SetToolTip(this.perCent1UpDown, resources.GetString("perCent1UpDown.ToolTip"));
            this.perCent1UpDown.ValueChanged += new System.EventHandler(this.perCent1_ValueChanged);
            this.perCent1UpDown.Leave += new System.EventHandler(this.perCent1_ValueChanged);
            // 
            // perCentLabel05
            // 
            resources.ApplyResources(this.perCentLabel05, "perCentLabel05");
            this.dirtyErrorProvider.SetError(this.perCentLabel05, resources.GetString("perCentLabel05.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.perCentLabel05, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("perCentLabel05.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.perCentLabel05, ((int)(resources.GetObject("perCentLabel05.IconPadding"))));
            this.perCentLabel05.Name = "perCentLabel05";
            this.toolTip1.SetToolTip(this.perCentLabel05, resources.GetString("perCentLabel05.ToolTip"));
            // 
            // perCent05UpDown
            // 
            resources.ApplyResources(this.perCent05UpDown, "perCent05UpDown");
            this.dirtyErrorProvider.SetError(this.perCent05UpDown, resources.GetString("perCent05UpDown.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.perCent05UpDown, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("perCent05UpDown.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.perCent05UpDown, ((int)(resources.GetObject("perCent05UpDown.IconPadding"))));
            this.perCent05UpDown.Name = "perCent05UpDown";
            this.toolTip1.SetToolTip(this.perCent05UpDown, resources.GetString("perCent05UpDown.ToolTip"));
            this.perCent05UpDown.ValueChanged += new System.EventHandler(this.perCent05_ValueChanged);
            this.perCent05UpDown.Leave += new System.EventHandler(this.perCent05_ValueChanged);
            // 
            // buttonCalculate
            // 
            resources.ApplyResources(this.buttonCalculate, "buttonCalculate");
            this.dirtyErrorProvider.SetError(this.buttonCalculate, resources.GetString("buttonCalculate.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.buttonCalculate, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("buttonCalculate.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.buttonCalculate, ((int)(resources.GetObject("buttonCalculate.IconPadding"))));
            this.buttonCalculate.Name = "buttonCalculate";
            this.toolTip1.SetToolTip(this.buttonCalculate, resources.GetString("buttonCalculate.ToolTip"));
            this.buttonCalculate.Click += new System.EventHandler(this.buttonCalculate_Click);
            // 
            // labelSum
            // 
            resources.ApplyResources(this.labelSum, "labelSum");
            this.dirtyErrorProvider.SetError(this.labelSum, resources.GetString("labelSum.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.labelSum, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelSum.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelSum, ((int)(resources.GetObject("labelSum.IconPadding"))));
            this.labelSum.Name = "labelSum";
            this.toolTip1.SetToolTip(this.labelSum, resources.GetString("labelSum.ToolTip"));
            // 
            // calculateThresholdsAtStartUpCheckBox
            // 
            resources.ApplyResources(this.calculateThresholdsAtStartUpCheckBox, "calculateThresholdsAtStartUpCheckBox");
            this.dirtyErrorProvider.SetError(this.calculateThresholdsAtStartUpCheckBox, resources.GetString("calculateThresholdsAtStartUpCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.calculateThresholdsAtStartUpCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("calculateThresholdsAtStartUpCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.calculateThresholdsAtStartUpCheckBox, ((int)(resources.GetObject("calculateThresholdsAtStartUpCheckBox.IconPadding"))));
            this.calculateThresholdsAtStartUpCheckBox.Name = "calculateThresholdsAtStartUpCheckBox";
            this.calculateThresholdsAtStartUpCheckBox.Tag = "#holdsAtStartUpCheckBoxLabel";
            this.toolTip1.SetToolTip(this.calculateThresholdsAtStartUpCheckBox, resources.GetString("calculateThresholdsAtStartUpCheckBox.ToolTip"));
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.buttonCalculate);
            this.groupBox1.Controls.Add(this.labelTotalTracks);
            this.groupBox1.Controls.Add(this.labelSum);
            this.groupBox1.Controls.Add(this.perCentLabel05);
            this.groupBox1.Controls.Add(this.perCent05UpDown);
            this.groupBox1.Controls.Add(this.perCentLabel1);
            this.groupBox1.Controls.Add(this.perCent1UpDown);
            this.groupBox1.Controls.Add(this.perCentLabel15);
            this.groupBox1.Controls.Add(this.perCent15UpDown);
            this.groupBox1.Controls.Add(this.perCentLabel2);
            this.groupBox1.Controls.Add(this.perCent2UpDown);
            this.groupBox1.Controls.Add(this.perCentLabel25);
            this.groupBox1.Controls.Add(this.perCent25UpDown);
            this.groupBox1.Controls.Add(this.perCentLabel3);
            this.groupBox1.Controls.Add(this.perCent3UpDown);
            this.groupBox1.Controls.Add(this.perCentLabel35);
            this.groupBox1.Controls.Add(this.perCent35UpDown);
            this.groupBox1.Controls.Add(this.perCentLabel4);
            this.groupBox1.Controls.Add(this.perCent4UpDown);
            this.groupBox1.Controls.Add(this.perCentLabel45);
            this.groupBox1.Controls.Add(this.perCent45UpDown);
            this.groupBox1.Controls.Add(this.perCentLabel5);
            this.groupBox1.Controls.Add(this.perCent5UpDown);
            this.dirtyErrorProvider.SetError(this.groupBox1, resources.GetString("groupBox1.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.groupBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.groupBox1, ((int)(resources.GetObject("groupBox1.IconPadding"))));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            this.groupBox1.Tag = "#AutoRateCommand";
            this.toolTip1.SetToolTip(this.groupBox1, resources.GetString("groupBox1.ToolTip"));
            // 
            // labelTotalTracks
            // 
            resources.ApplyResources(this.labelTotalTracks, "labelTotalTracks");
            this.dirtyErrorProvider.SetError(this.labelTotalTracks, resources.GetString("labelTotalTracks.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.labelTotalTracks, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelTotalTracks.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.labelTotalTracks, ((int)(resources.GetObject("labelTotalTracks.IconPadding"))));
            this.labelTotalTracks.Name = "labelTotalTracks";
            this.toolTip1.SetToolTip(this.labelTotalTracks, resources.GetString("labelTotalTracks.ToolTip"));
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.buttonOK);
            this.groupBox2.Controls.Add(this.avgPlaysPerDayBox);
            this.groupBox2.Controls.Add(this.label30);
            this.groupBox2.Controls.Add(this.maxPlaysPerDayBox);
            this.groupBox2.Controls.Add(this.label29);
            this.groupBox2.Controls.Add(this.label28);
            this.groupBox2.Controls.Add(this.threshold05Box);
            this.groupBox2.Controls.Add(this.checkBox05Label);
            this.groupBox2.Controls.Add(this.checkBox05);
            this.groupBox2.Controls.Add(this.label27);
            this.groupBox2.Controls.Add(this.threshold1Box);
            this.groupBox2.Controls.Add(this.checkBox1Label);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.label26);
            this.groupBox2.Controls.Add(this.threshold15Box);
            this.groupBox2.Controls.Add(this.checkBox15Label);
            this.groupBox2.Controls.Add(this.checkBox15);
            this.groupBox2.Controls.Add(this.label25);
            this.groupBox2.Controls.Add(this.threshold2Box);
            this.groupBox2.Controls.Add(this.checkBox2Label);
            this.groupBox2.Controls.Add(this.checkBox2);
            this.groupBox2.Controls.Add(this.label24);
            this.groupBox2.Controls.Add(this.threshold25Box);
            this.groupBox2.Controls.Add(this.checkBox25Label);
            this.groupBox2.Controls.Add(this.checkBox25);
            this.groupBox2.Controls.Add(this.label23);
            this.groupBox2.Controls.Add(this.threshold3Box);
            this.groupBox2.Controls.Add(this.checkBox3Label);
            this.groupBox2.Controls.Add(this.checkBox3);
            this.groupBox2.Controls.Add(this.label22);
            this.groupBox2.Controls.Add(this.threshold35Box);
            this.groupBox2.Controls.Add(this.checkBox35Label);
            this.groupBox2.Controls.Add(this.checkBox35);
            this.groupBox2.Controls.Add(this.label21);
            this.groupBox2.Controls.Add(this.threshold4Box);
            this.groupBox2.Controls.Add(this.checkBox4Label);
            this.groupBox2.Controls.Add(this.checkBox4);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.threshold45Box);
            this.groupBox2.Controls.Add(this.checkBox45Label);
            this.groupBox2.Controls.Add(this.checkBox45);
            this.groupBox2.Controls.Add(this.LabelFive);
            this.groupBox2.Controls.Add(this.threshold5Box);
            this.groupBox2.Controls.Add(this.checkBox5Label);
            this.groupBox2.Controls.Add(this.checkBox5);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.dirtyErrorProvider.SetError(this.groupBox2, resources.GetString("groupBox2.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.groupBox2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox2.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.groupBox2, ((int)(resources.GetObject("groupBox2.IconPadding"))));
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            this.groupBox2.Tag = "#AutoRateCommand";
            this.toolTip1.SetToolTip(this.groupBox2, resources.GetString("groupBox2.ToolTip"));
            // 
            // checkBox05Label
            // 
            resources.ApplyResources(this.checkBox05Label, "checkBox05Label");
            this.dirtyErrorProvider.SetError(this.checkBox05Label, resources.GetString("checkBox05Label.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.checkBox05Label, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkBox05Label.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.checkBox05Label, ((int)(resources.GetObject("checkBox05Label.IconPadding"))));
            this.checkBox05Label.Name = "checkBox05Label";
            this.toolTip1.SetToolTip(this.checkBox05Label, resources.GetString("checkBox05Label.ToolTip"));
            this.checkBox05Label.Click += new System.EventHandler(this.checkBox05Label_Click);
            // 
            // checkBox1Label
            // 
            resources.ApplyResources(this.checkBox1Label, "checkBox1Label");
            this.dirtyErrorProvider.SetError(this.checkBox1Label, resources.GetString("checkBox1Label.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.checkBox1Label, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkBox1Label.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.checkBox1Label, ((int)(resources.GetObject("checkBox1Label.IconPadding"))));
            this.checkBox1Label.Name = "checkBox1Label";
            this.toolTip1.SetToolTip(this.checkBox1Label, resources.GetString("checkBox1Label.ToolTip"));
            this.checkBox1Label.Click += new System.EventHandler(this.checkBox1Label_Click);
            // 
            // checkBox15Label
            // 
            resources.ApplyResources(this.checkBox15Label, "checkBox15Label");
            this.dirtyErrorProvider.SetError(this.checkBox15Label, resources.GetString("checkBox15Label.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.checkBox15Label, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkBox15Label.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.checkBox15Label, ((int)(resources.GetObject("checkBox15Label.IconPadding"))));
            this.checkBox15Label.Name = "checkBox15Label";
            this.toolTip1.SetToolTip(this.checkBox15Label, resources.GetString("checkBox15Label.ToolTip"));
            this.checkBox15Label.Click += new System.EventHandler(this.checkBox15Label_Click);
            // 
            // checkBox2Label
            // 
            resources.ApplyResources(this.checkBox2Label, "checkBox2Label");
            this.dirtyErrorProvider.SetError(this.checkBox2Label, resources.GetString("checkBox2Label.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.checkBox2Label, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkBox2Label.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.checkBox2Label, ((int)(resources.GetObject("checkBox2Label.IconPadding"))));
            this.checkBox2Label.Name = "checkBox2Label";
            this.toolTip1.SetToolTip(this.checkBox2Label, resources.GetString("checkBox2Label.ToolTip"));
            this.checkBox2Label.Click += new System.EventHandler(this.checkBox2Label_Click);
            // 
            // checkBox25Label
            // 
            resources.ApplyResources(this.checkBox25Label, "checkBox25Label");
            this.dirtyErrorProvider.SetError(this.checkBox25Label, resources.GetString("checkBox25Label.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.checkBox25Label, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkBox25Label.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.checkBox25Label, ((int)(resources.GetObject("checkBox25Label.IconPadding"))));
            this.checkBox25Label.Name = "checkBox25Label";
            this.toolTip1.SetToolTip(this.checkBox25Label, resources.GetString("checkBox25Label.ToolTip"));
            this.checkBox25Label.Click += new System.EventHandler(this.checkBox25Label_Click);
            // 
            // checkBox3Label
            // 
            resources.ApplyResources(this.checkBox3Label, "checkBox3Label");
            this.dirtyErrorProvider.SetError(this.checkBox3Label, resources.GetString("checkBox3Label.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.checkBox3Label, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkBox3Label.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.checkBox3Label, ((int)(resources.GetObject("checkBox3Label.IconPadding"))));
            this.checkBox3Label.Name = "checkBox3Label";
            this.toolTip1.SetToolTip(this.checkBox3Label, resources.GetString("checkBox3Label.ToolTip"));
            this.checkBox3Label.Click += new System.EventHandler(this.checkBox3Label_Click);
            // 
            // checkBox35Label
            // 
            resources.ApplyResources(this.checkBox35Label, "checkBox35Label");
            this.dirtyErrorProvider.SetError(this.checkBox35Label, resources.GetString("checkBox35Label.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.checkBox35Label, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkBox35Label.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.checkBox35Label, ((int)(resources.GetObject("checkBox35Label.IconPadding"))));
            this.checkBox35Label.Name = "checkBox35Label";
            this.toolTip1.SetToolTip(this.checkBox35Label, resources.GetString("checkBox35Label.ToolTip"));
            this.checkBox35Label.Click += new System.EventHandler(this.checkBox35Label_Click);
            // 
            // checkBox4Label
            // 
            resources.ApplyResources(this.checkBox4Label, "checkBox4Label");
            this.dirtyErrorProvider.SetError(this.checkBox4Label, resources.GetString("checkBox4Label.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.checkBox4Label, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkBox4Label.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.checkBox4Label, ((int)(resources.GetObject("checkBox4Label.IconPadding"))));
            this.checkBox4Label.Name = "checkBox4Label";
            this.toolTip1.SetToolTip(this.checkBox4Label, resources.GetString("checkBox4Label.ToolTip"));
            this.checkBox4Label.Click += new System.EventHandler(this.checkBox4Label_Click);
            // 
            // checkBox45Label
            // 
            resources.ApplyResources(this.checkBox45Label, "checkBox45Label");
            this.dirtyErrorProvider.SetError(this.checkBox45Label, resources.GetString("checkBox45Label.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.checkBox45Label, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkBox45Label.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.checkBox45Label, ((int)(resources.GetObject("checkBox45Label.IconPadding"))));
            this.checkBox45Label.Name = "checkBox45Label";
            this.toolTip1.SetToolTip(this.checkBox45Label, resources.GetString("checkBox45Label.ToolTip"));
            this.checkBox45Label.Click += new System.EventHandler(this.checkBox45Label_Click);
            // 
            // checkBox5Label
            // 
            resources.ApplyResources(this.checkBox5Label, "checkBox5Label");
            this.dirtyErrorProvider.SetError(this.checkBox5Label, resources.GetString("checkBox5Label.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.checkBox5Label, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkBox5Label.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.checkBox5Label, ((int)(resources.GetObject("checkBox5Label.IconPadding"))));
            this.checkBox5Label.Name = "checkBox5Label";
            this.toolTip1.SetToolTip(this.checkBox5Label, resources.GetString("checkBox5Label.ToolTip"));
            this.checkBox5Label.Click += new System.EventHandler(this.checkBox5Label_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.dirtyErrorProvider.SetError(this.label2, resources.GetString("label2.Error"));
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.dirtyErrorProvider.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label2, ((int)(resources.GetObject("label2.IconPadding"))));
            this.label2.Name = "label2";
            this.label2.Tag = "#groupBox2";
            this.toolTip1.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.dirtyErrorProvider.SetError(this.label1, resources.GetString("label1.Error"));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.dirtyErrorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            this.label1.Tag = "#groupBox2";
            this.toolTip1.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 20000;
            this.toolTip1.InitialDelay = 1000;
            this.toolTip1.ReshowDelay = 100;
            // 
            // sinceAddedCheckBox
            // 
            resources.ApplyResources(this.sinceAddedCheckBox, "sinceAddedCheckBox");
            this.dirtyErrorProvider.SetError(this.sinceAddedCheckBox, resources.GetString("sinceAddedCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.sinceAddedCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("sinceAddedCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.sinceAddedCheckBox, ((int)(resources.GetObject("sinceAddedCheckBox.IconPadding"))));
            this.sinceAddedCheckBox.Name = "sinceAddedCheckBox";
            this.sinceAddedCheckBox.Tag = "#sinceAddedCheckBoxLabel";
            this.toolTip1.SetToolTip(this.sinceAddedCheckBox, resources.GetString("sinceAddedCheckBox.ToolTip"));
            // 
            // sinceAddedCheckBoxLabel
            // 
            resources.ApplyResources(this.sinceAddedCheckBoxLabel, "sinceAddedCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.sinceAddedCheckBoxLabel, resources.GetString("sinceAddedCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.sinceAddedCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("sinceAddedCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.sinceAddedCheckBoxLabel, ((int)(resources.GetObject("sinceAddedCheckBoxLabel.IconPadding"))));
            this.sinceAddedCheckBoxLabel.Name = "sinceAddedCheckBoxLabel";
            this.toolTip1.SetToolTip(this.sinceAddedCheckBoxLabel, resources.GetString("sinceAddedCheckBoxLabel.ToolTip"));
            this.sinceAddedCheckBoxLabel.Click += new System.EventHandler(this.sinceAddedCheckBoxLabel_Click);
            // 
            // notifyWhenAutoratingCompletedCheckBox
            // 
            resources.ApplyResources(this.notifyWhenAutoratingCompletedCheckBox, "notifyWhenAutoratingCompletedCheckBox");
            this.dirtyErrorProvider.SetError(this.notifyWhenAutoratingCompletedCheckBox, resources.GetString("notifyWhenAutoratingCompletedCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.notifyWhenAutoratingCompletedCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("notifyWhenAutoratingCompletedCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.notifyWhenAutoratingCompletedCheckBox, ((int)(resources.GetObject("notifyWhenAutoratingCompletedCheckBox.IconPadding"))));
            this.notifyWhenAutoratingCompletedCheckBox.Name = "notifyWhenAutoratingCompletedCheckBox";
            this.notifyWhenAutoratingCompletedCheckBox.Tag = "#notifyWhenAutoratingCompletedCheckBoxLabel";
            this.toolTip1.SetToolTip(this.notifyWhenAutoratingCompletedCheckBox, resources.GetString("notifyWhenAutoratingCompletedCheckBox.ToolTip"));
            // 
            // playsPerDayTagList
            // 
            resources.ApplyResources(this.playsPerDayTagList, "playsPerDayTagList");
            this.playsPerDayTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playsPerDayTagList.DropDownWidth = 250;
            this.dirtyErrorProvider.SetError(this.playsPerDayTagList, resources.GetString("playsPerDayTagList.Error"));
            this.playsPerDayTagList.FormattingEnabled = true;
            this.dirtyErrorProvider.SetIconAlignment(this.playsPerDayTagList, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("playsPerDayTagList.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.playsPerDayTagList, ((int)(resources.GetObject("playsPerDayTagList.IconPadding"))));
            this.playsPerDayTagList.Name = "playsPerDayTagList";
            this.toolTip1.SetToolTip(this.playsPerDayTagList, resources.GetString("playsPerDayTagList.ToolTip"));
            // 
            // storePlaysPerDayCheckBox
            // 
            resources.ApplyResources(this.storePlaysPerDayCheckBox, "storePlaysPerDayCheckBox");
            this.storePlaysPerDayCheckBox.Checked = true;
            this.storePlaysPerDayCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dirtyErrorProvider.SetError(this.storePlaysPerDayCheckBox, resources.GetString("storePlaysPerDayCheckBox.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.storePlaysPerDayCheckBox, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("storePlaysPerDayCheckBox.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.storePlaysPerDayCheckBox, ((int)(resources.GetObject("storePlaysPerDayCheckBox.IconPadding"))));
            this.storePlaysPerDayCheckBox.Name = "storePlaysPerDayCheckBox";
            this.storePlaysPerDayCheckBox.Tag = "#storePlaysPerDayCheckBoxLabel";
            this.toolTip1.SetToolTip(this.storePlaysPerDayCheckBox, resources.GetString("storePlaysPerDayCheckBox.ToolTip"));
            this.storePlaysPerDayCheckBox.CheckedChanged += new System.EventHandler(this.storePlaysPerDay_CheckedChanged);
            // 
            // storePlaysPerDayCheckBoxLabel
            // 
            resources.ApplyResources(this.storePlaysPerDayCheckBoxLabel, "storePlaysPerDayCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.storePlaysPerDayCheckBoxLabel, resources.GetString("storePlaysPerDayCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.storePlaysPerDayCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("storePlaysPerDayCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.storePlaysPerDayCheckBoxLabel, ((int)(resources.GetObject("storePlaysPerDayCheckBoxLabel.IconPadding"))));
            this.storePlaysPerDayCheckBoxLabel.Name = "storePlaysPerDayCheckBoxLabel";
            this.toolTip1.SetToolTip(this.storePlaysPerDayCheckBoxLabel, resources.GetString("storePlaysPerDayCheckBoxLabel.ToolTip"));
            this.storePlaysPerDayCheckBoxLabel.Click += new System.EventHandler(this.storePlaysPerDayCheckBoxLabel_Click);
            // 
            // autoRateAtStartUpCheckBoxLabel
            // 
            resources.ApplyResources(this.autoRateAtStartUpCheckBoxLabel, "autoRateAtStartUpCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.autoRateAtStartUpCheckBoxLabel, resources.GetString("autoRateAtStartUpCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.autoRateAtStartUpCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("autoRateAtStartUpCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.autoRateAtStartUpCheckBoxLabel, ((int)(resources.GetObject("autoRateAtStartUpCheckBoxLabel.IconPadding"))));
            this.autoRateAtStartUpCheckBoxLabel.Name = "autoRateAtStartUpCheckBoxLabel";
            this.autoRateAtStartUpCheckBoxLabel.Tag = "#notifyWhenAutoratingCompletedCheckBox";
            this.toolTip1.SetToolTip(this.autoRateAtStartUpCheckBoxLabel, resources.GetString("autoRateAtStartUpCheckBoxLabel.ToolTip"));
            this.autoRateAtStartUpCheckBoxLabel.Click += new System.EventHandler(this.autoRateAtStartUpCheckBoxLabel_Click);
            // 
            // notifyWhenAutoratingCompletedCheckBoxLabel
            // 
            resources.ApplyResources(this.notifyWhenAutoratingCompletedCheckBoxLabel, "notifyWhenAutoratingCompletedCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.notifyWhenAutoratingCompletedCheckBoxLabel, resources.GetString("notifyWhenAutoratingCompletedCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.notifyWhenAutoratingCompletedCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("notifyWhenAutoratingCompletedCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.notifyWhenAutoratingCompletedCheckBoxLabel, ((int)(resources.GetObject("notifyWhenAutoratingCompletedCheckBoxLabel.IconPadding"))));
            this.notifyWhenAutoratingCompletedCheckBoxLabel.Name = "notifyWhenAutoratingCompletedCheckBoxLabel";
            this.toolTip1.SetToolTip(this.notifyWhenAutoratingCompletedCheckBoxLabel, resources.GetString("notifyWhenAutoratingCompletedCheckBoxLabel.ToolTip"));
            this.notifyWhenAutoratingCompletedCheckBoxLabel.Click += new System.EventHandler(this.notifyWhenAutoratingCompletedCheckBoxLabel_Click);
            // 
            // autoRateOnTrackPropertiesCheckBoxLabel
            // 
            resources.ApplyResources(this.autoRateOnTrackPropertiesCheckBoxLabel, "autoRateOnTrackPropertiesCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.autoRateOnTrackPropertiesCheckBoxLabel, resources.GetString("autoRateOnTrackPropertiesCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.autoRateOnTrackPropertiesCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("autoRateOnTrackPropertiesCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.autoRateOnTrackPropertiesCheckBoxLabel, ((int)(resources.GetObject("autoRateOnTrackPropertiesCheckBoxLabel.IconPadding"))));
            this.autoRateOnTrackPropertiesCheckBoxLabel.Name = "autoRateOnTrackPropertiesCheckBoxLabel";
            this.toolTip1.SetToolTip(this.autoRateOnTrackPropertiesCheckBoxLabel, resources.GetString("autoRateOnTrackPropertiesCheckBoxLabel.ToolTip"));
            this.autoRateOnTrackPropertiesCheckBoxLabel.Click += new System.EventHandler(this.autoRateOnTrackPropertiesCheckBoxLabel_Click);
            // 
            // holdsAtStartUpCheckBoxLabel
            // 
            resources.ApplyResources(this.holdsAtStartUpCheckBoxLabel, "holdsAtStartUpCheckBoxLabel");
            this.dirtyErrorProvider.SetError(this.holdsAtStartUpCheckBoxLabel, resources.GetString("holdsAtStartUpCheckBoxLabel.Error"));
            this.dirtyErrorProvider.SetIconAlignment(this.holdsAtStartUpCheckBoxLabel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("holdsAtStartUpCheckBoxLabel.IconAlignment"))));
            this.dirtyErrorProvider.SetIconPadding(this.holdsAtStartUpCheckBoxLabel, ((int)(resources.GetObject("holdsAtStartUpCheckBoxLabel.IconPadding"))));
            this.holdsAtStartUpCheckBoxLabel.Name = "holdsAtStartUpCheckBoxLabel";
            this.toolTip1.SetToolTip(this.holdsAtStartUpCheckBoxLabel, resources.GetString("holdsAtStartUpCheckBoxLabel.ToolTip"));
            this.holdsAtStartUpCheckBoxLabel.Click += new System.EventHandler(this.holdsAtStartUpCheckBoxLabel_Click);
            // 
            // dirtyErrorProvider
            // 
            this.dirtyErrorProvider.BlinkRate = 1000;
            this.dirtyErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
            this.dirtyErrorProvider.ContainerControl = this;
            resources.ApplyResources(this.dirtyErrorProvider, "dirtyErrorProvider");
            // 
            // AutoRateCommand
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.sinceAddedCheckBoxLabel);
            this.Controls.Add(this.sinceAddedCheckBox);
            this.Controls.Add(this.autoRateOnTrackPropertiesCheckBoxLabel);
            this.Controls.Add(this.autoRateOnTrackPropertiesCheckBox);
            this.Controls.Add(this.notifyWhenAutoratingCompletedCheckBoxLabel);
            this.Controls.Add(this.notifyWhenAutoratingCompletedCheckBox);
            this.Controls.Add(this.autoRateAtStartUpCheckBoxLabel);
            this.Controls.Add(this.autoRateAtStartUpCheckBox);
            this.Controls.Add(this.holdsAtStartUpCheckBoxLabel);
            this.Controls.Add(this.calculateThresholdsAtStartUpCheckBox);
            this.Controls.Add(this.playsPerDayTagList);
            this.Controls.Add(this.storePlaysPerDayCheckBoxLabel);
            this.Controls.Add(this.storePlaysPerDayCheckBox);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.autoRatingTagList);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.baseRatingTrackBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AutoRateCommand";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            ((System.ComponentModel.ISupportInitialize)(this.baseRatingTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.perCent5UpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.perCent45UpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.perCent4UpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.perCent35UpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.perCent3UpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.perCent25UpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.perCent2UpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.perCent15UpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.perCent1UpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.perCent05UpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox autoRatingTagList;
        private System.Windows.Forms.CheckBox autoRateAtStartUpCheckBox;
        private System.Windows.Forms.CheckBox autoRateOnTrackPropertiesCheckBox;
        private System.Windows.Forms.TrackBar baseRatingTrackBar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TextBox threshold05Box;
        private System.Windows.Forms.TextBox threshold1Box;
        private System.Windows.Forms.TextBox threshold15Box;
        private System.Windows.Forms.TextBox threshold2Box;
        private System.Windows.Forms.TextBox threshold25Box;
        private System.Windows.Forms.TextBox threshold3Box;
        private System.Windows.Forms.TextBox threshold35Box;
        private System.Windows.Forms.TextBox threshold4Box;
        private System.Windows.Forms.TextBox threshold45Box;
        private System.Windows.Forms.TextBox threshold5Box;
        private System.Windows.Forms.Label LabelFive;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox maxPlaysPerDayBox;
        private System.Windows.Forms.TextBox avgPlaysPerDayBox;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox45;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox35;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox25;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox15;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox05;
        private System.Windows.Forms.NumericUpDown perCent5UpDown;
        private System.Windows.Forms.Label perCentLabel5;
        private System.Windows.Forms.Label perCentLabel45;
        private System.Windows.Forms.NumericUpDown perCent45UpDown;
        private System.Windows.Forms.Label perCentLabel4;
        private System.Windows.Forms.NumericUpDown perCent4UpDown;
        private System.Windows.Forms.Label perCentLabel35;
        private System.Windows.Forms.NumericUpDown perCent35UpDown;
        private System.Windows.Forms.Label perCentLabel3;
        private System.Windows.Forms.NumericUpDown perCent3UpDown;
        private System.Windows.Forms.Label perCentLabel25;
        private System.Windows.Forms.NumericUpDown perCent25UpDown;
        private System.Windows.Forms.Label perCentLabel2;
        private System.Windows.Forms.NumericUpDown perCent2UpDown;
        private System.Windows.Forms.Label perCentLabel15;
        private System.Windows.Forms.NumericUpDown perCent15UpDown;
        private System.Windows.Forms.Label perCentLabel1;
        private System.Windows.Forms.NumericUpDown perCent1UpDown;
        private System.Windows.Forms.Label perCentLabel05;
        private System.Windows.Forms.NumericUpDown perCent05UpDown;
        private System.Windows.Forms.Button buttonCalculate;
        private System.Windows.Forms.Label labelSum;
        private System.Windows.Forms.CheckBox calculateThresholdsAtStartUpCheckBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelTotalTracks;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox notifyWhenAutoratingCompletedCheckBox;
        private System.Windows.Forms.ComboBox playsPerDayTagList;
        private System.Windows.Forms.CheckBox storePlaysPerDayCheckBox;
        private System.Windows.Forms.CheckBox sinceAddedCheckBox;
        private System.Windows.Forms.ErrorProvider dirtyErrorProvider;
        private Label autoRateAtStartUpCheckBoxLabel;
        private Label storePlaysPerDayCheckBoxLabel;
        private Label notifyWhenAutoratingCompletedCheckBoxLabel;
        private Label autoRateOnTrackPropertiesCheckBoxLabel;
        private Label sinceAddedCheckBoxLabel;
        private Label checkBox5Label;
        private Label checkBox45Label;
        private Label checkBox4Label;
        private Label checkBox35Label;
        private Label checkBox3Label;
        private Label checkBox25Label;
        private Label checkBox2Label;
        private Label checkBox15Label;
        private Label checkBox1Label;
        private Label checkBox05Label;
        private Label holdsAtStartUpCheckBoxLabel;
        private Label label2;
        private Label label1;
    }
}