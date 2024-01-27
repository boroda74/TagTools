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
            this.groupBox1Label = new System.Windows.Forms.Label();
            this.labelTotalTracks = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox2Label = new System.Windows.Forms.Label();
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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.sinceAddedCheckBox = new System.Windows.Forms.CheckBox();
            this.sinceAddedCheckBoxLabel = new System.Windows.Forms.Label();
            this.buttonSettings = new System.Windows.Forms.Button();
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
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Tag = "#AutoRateCommand@non-defaultable";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Tag = "";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            this.label4.Tag = "#autoRatingTagList@pinned-to-parent-x";
            // 
            // autoRatingTagList
            // 
            this.autoRatingTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.autoRatingTagList.DropDownWidth = 250;
            this.autoRatingTagList.FormattingEnabled = true;
            resources.ApplyResources(this.autoRatingTagList, "autoRatingTagList");
            this.autoRatingTagList.Name = "autoRatingTagList";
            this.autoRatingTagList.Tag = "";
            // 
            // autoRateAtStartUpCheckBox
            // 
            resources.ApplyResources(this.autoRateAtStartUpCheckBox, "autoRateAtStartUpCheckBox");
            this.autoRateAtStartUpCheckBox.Checked = true;
            this.autoRateAtStartUpCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoRateAtStartUpCheckBox.Name = "autoRateAtStartUpCheckBox";
            this.autoRateAtStartUpCheckBox.Tag = "#autoRateAtStartUpCheckBoxLabel";
            this.autoRateAtStartUpCheckBox.CheckedChanged += new System.EventHandler(this.autoRateAtStartUp_CheckedChanged);
            // 
            // autoRateOnTrackPropertiesCheckBox
            // 
            resources.ApplyResources(this.autoRateOnTrackPropertiesCheckBox, "autoRateOnTrackPropertiesCheckBox");
            this.autoRateOnTrackPropertiesCheckBox.Name = "autoRateOnTrackPropertiesCheckBox";
            this.autoRateOnTrackPropertiesCheckBox.Tag = "#autoRateOnTrackPropertiesCheckBoxLabel";
            // 
            // baseRatingTrackBar
            // 
            resources.ApplyResources(this.baseRatingTrackBar, "baseRatingTrackBar");
            this.baseRatingTrackBar.Name = "baseRatingTrackBar";
            this.baseRatingTrackBar.Tag = "";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            this.label5.Tag = "";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            this.label6.Tag = "";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            this.label7.Tag = "";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            this.label8.Tag = "";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            this.label9.Tag = "";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            this.label10.Tag = "";
            // 
            // buttonSave
            // 
            resources.ApplyResources(this.buttonSave, "buttonSave");
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Tag = "@non-defaultable";
            this.buttonSave.Click += new System.EventHandler(this.button1_Click);
            // 
            // threshold05Box
            // 
            resources.ApplyResources(this.threshold05Box, "threshold05Box");
            this.threshold05Box.Name = "threshold05Box";
            this.threshold05Box.Tag = "";
            this.threshold05Box.Leave += new System.EventHandler(this.threshold_TextChanged);
            // 
            // threshold1Box
            // 
            resources.ApplyResources(this.threshold1Box, "threshold1Box");
            this.threshold1Box.Name = "threshold1Box";
            this.threshold1Box.Tag = "";
            this.threshold1Box.Leave += new System.EventHandler(this.threshold_TextChanged);
            // 
            // threshold15Box
            // 
            resources.ApplyResources(this.threshold15Box, "threshold15Box");
            this.threshold15Box.Name = "threshold15Box";
            this.threshold15Box.Tag = "";
            this.threshold15Box.Leave += new System.EventHandler(this.threshold_TextChanged);
            // 
            // threshold2Box
            // 
            resources.ApplyResources(this.threshold2Box, "threshold2Box");
            this.threshold2Box.Name = "threshold2Box";
            this.threshold2Box.Tag = "";
            this.threshold2Box.Leave += new System.EventHandler(this.threshold_TextChanged);
            // 
            // threshold25Box
            // 
            resources.ApplyResources(this.threshold25Box, "threshold25Box");
            this.threshold25Box.Name = "threshold25Box";
            this.threshold25Box.Tag = "";
            this.threshold25Box.Leave += new System.EventHandler(this.threshold_TextChanged);
            // 
            // threshold3Box
            // 
            resources.ApplyResources(this.threshold3Box, "threshold3Box");
            this.threshold3Box.Name = "threshold3Box";
            this.threshold3Box.Tag = "";
            this.threshold3Box.Leave += new System.EventHandler(this.threshold_TextChanged);
            // 
            // threshold35Box
            // 
            resources.ApplyResources(this.threshold35Box, "threshold35Box");
            this.threshold35Box.Name = "threshold35Box";
            this.threshold35Box.Tag = "";
            this.threshold35Box.Leave += new System.EventHandler(this.threshold_TextChanged);
            // 
            // threshold4Box
            // 
            resources.ApplyResources(this.threshold4Box, "threshold4Box");
            this.threshold4Box.Name = "threshold4Box";
            this.threshold4Box.Tag = "";
            this.threshold4Box.Leave += new System.EventHandler(this.threshold_TextChanged);
            // 
            // threshold45Box
            // 
            resources.ApplyResources(this.threshold45Box, "threshold45Box");
            this.threshold45Box.Name = "threshold45Box";
            this.threshold45Box.Tag = "";
            this.threshold45Box.Leave += new System.EventHandler(this.threshold_TextChanged);
            // 
            // threshold5Box
            // 
            resources.ApplyResources(this.threshold5Box, "threshold5Box");
            this.threshold5Box.Name = "threshold5Box";
            this.threshold5Box.Tag = "";
            this.threshold5Box.Leave += new System.EventHandler(this.threshold_TextChanged);
            // 
            // LabelFive
            // 
            resources.ApplyResources(this.LabelFive, "LabelFive");
            this.LabelFive.Name = "LabelFive";
            this.LabelFive.Tag = "";
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            this.label19.Tag = "";
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            this.label20.Tag = "";
            // 
            // label21
            // 
            resources.ApplyResources(this.label21, "label21");
            this.label21.Name = "label21";
            this.label21.Tag = "";
            // 
            // label22
            // 
            resources.ApplyResources(this.label22, "label22");
            this.label22.Name = "label22";
            this.label22.Tag = "";
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            this.label23.Tag = "";
            // 
            // label24
            // 
            resources.ApplyResources(this.label24, "label24");
            this.label24.Name = "label24";
            this.label24.Tag = "";
            // 
            // label25
            // 
            resources.ApplyResources(this.label25, "label25");
            this.label25.Name = "label25";
            this.label25.Tag = "";
            // 
            // label26
            // 
            resources.ApplyResources(this.label26, "label26");
            this.label26.Name = "label26";
            this.label26.Tag = "";
            // 
            // label27
            // 
            resources.ApplyResources(this.label27, "label27");
            this.label27.Name = "label27";
            this.label27.Tag = "";
            // 
            // label28
            // 
            resources.ApplyResources(this.label28, "label28");
            this.label28.Name = "label28";
            this.label28.Tag = "";
            // 
            // label29
            // 
            resources.ApplyResources(this.label29, "label29");
            this.label29.Name = "label29";
            this.label29.Tag = "";
            // 
            // maxPlaysPerDayBox
            // 
            resources.ApplyResources(this.maxPlaysPerDayBox, "maxPlaysPerDayBox");
            this.maxPlaysPerDayBox.Name = "maxPlaysPerDayBox";
            this.maxPlaysPerDayBox.ReadOnly = true;
            this.maxPlaysPerDayBox.Tag = "";
            // 
            // avgPlaysPerDayBox
            // 
            resources.ApplyResources(this.avgPlaysPerDayBox, "avgPlaysPerDayBox");
            this.avgPlaysPerDayBox.Name = "avgPlaysPerDayBox";
            this.avgPlaysPerDayBox.ReadOnly = true;
            this.avgPlaysPerDayBox.Tag = "";
            // 
            // label30
            // 
            resources.ApplyResources(this.label30, "label30");
            this.label30.Name = "label30";
            this.label30.Tag = "";
            // 
            // checkBox5
            // 
            resources.ApplyResources(this.checkBox5, "checkBox5");
            this.checkBox5.Checked = true;
            this.checkBox5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Tag = "#checkBox5Label";
            this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBoxFive_CheckedChanged);
            // 
            // checkBox45
            // 
            resources.ApplyResources(this.checkBox45, "checkBox45");
            this.checkBox45.Checked = true;
            this.checkBox45.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox45.Name = "checkBox45";
            this.checkBox45.Tag = "#checkBox45Label";
            this.checkBox45.CheckedChanged += new System.EventHandler(this.checkBoxFourAndHalf_CheckedChanged);
            // 
            // checkBox4
            // 
            resources.ApplyResources(this.checkBox4, "checkBox4");
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Tag = "#checkBox4Label";
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBoxFour_CheckedChanged);
            // 
            // checkBox35
            // 
            resources.ApplyResources(this.checkBox35, "checkBox35");
            this.checkBox35.Checked = true;
            this.checkBox35.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox35.Name = "checkBox35";
            this.checkBox35.Tag = "#checkBox35Label";
            this.checkBox35.CheckedChanged += new System.EventHandler(this.checkBoxThreeAndHalf_CheckedChanged);
            // 
            // checkBox3
            // 
            resources.ApplyResources(this.checkBox3, "checkBox3");
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Tag = "#checkBox3Label";
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBoxThree_CheckedChanged);
            // 
            // checkBox25
            // 
            resources.ApplyResources(this.checkBox25, "checkBox25");
            this.checkBox25.Checked = true;
            this.checkBox25.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox25.Name = "checkBox25";
            this.checkBox25.Tag = "#checkBox25Label";
            this.checkBox25.CheckedChanged += new System.EventHandler(this.checkBoxTwoAndHalf_CheckedChanged);
            // 
            // checkBox2
            // 
            resources.ApplyResources(this.checkBox2, "checkBox2");
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Tag = "#checkBox2Label";
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBoxTwo_CheckedChanged);
            // 
            // checkBox15
            // 
            resources.ApplyResources(this.checkBox15, "checkBox15");
            this.checkBox15.Checked = true;
            this.checkBox15.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox15.Name = "checkBox15";
            this.checkBox15.Tag = "#checkBox15Label";
            this.checkBox15.CheckedChanged += new System.EventHandler(this.checkBoxOneAndHalf_CheckedChanged);
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Tag = "#checkBox1Label";
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBoxOne_CheckedChanged);
            // 
            // checkBox05
            // 
            resources.ApplyResources(this.checkBox05, "checkBox05");
            this.checkBox05.Checked = true;
            this.checkBox05.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox05.Name = "checkBox05";
            this.checkBox05.Tag = "#checkBox05Label";
            this.checkBox05.CheckedChanged += new System.EventHandler(this.checkBoxHalf_CheckedChanged);
            // 
            // perCent5UpDown
            // 
            resources.ApplyResources(this.perCent5UpDown, "perCent5UpDown");
            this.perCent5UpDown.Name = "perCent5UpDown";
            this.perCent5UpDown.Tag = "";
            this.perCent5UpDown.ValueChanged += new System.EventHandler(this.perCent5_ValueChanged);
            this.perCent5UpDown.Leave += new System.EventHandler(this.perCent5_ValueChanged);
            // 
            // perCentLabel5
            // 
            resources.ApplyResources(this.perCentLabel5, "perCentLabel5");
            this.perCentLabel5.Name = "perCentLabel5";
            this.perCentLabel5.Tag = "";
            // 
            // perCentLabel45
            // 
            resources.ApplyResources(this.perCentLabel45, "perCentLabel45");
            this.perCentLabel45.Name = "perCentLabel45";
            this.perCentLabel45.Tag = "";
            // 
            // perCent45UpDown
            // 
            resources.ApplyResources(this.perCent45UpDown, "perCent45UpDown");
            this.perCent45UpDown.Name = "perCent45UpDown";
            this.perCent45UpDown.Tag = "";
            this.perCent45UpDown.ValueChanged += new System.EventHandler(this.perCent45_ValueChanged);
            this.perCent45UpDown.Leave += new System.EventHandler(this.perCent45_ValueChanged);
            // 
            // perCentLabel4
            // 
            resources.ApplyResources(this.perCentLabel4, "perCentLabel4");
            this.perCentLabel4.Name = "perCentLabel4";
            this.perCentLabel4.Tag = "";
            // 
            // perCent4UpDown
            // 
            resources.ApplyResources(this.perCent4UpDown, "perCent4UpDown");
            this.perCent4UpDown.Name = "perCent4UpDown";
            this.perCent4UpDown.Tag = "";
            this.perCent4UpDown.ValueChanged += new System.EventHandler(this.perCent4_ValueChanged);
            this.perCent4UpDown.Leave += new System.EventHandler(this.perCent4_ValueChanged);
            // 
            // perCentLabel35
            // 
            resources.ApplyResources(this.perCentLabel35, "perCentLabel35");
            this.perCentLabel35.Name = "perCentLabel35";
            this.perCentLabel35.Tag = "";
            // 
            // perCent35UpDown
            // 
            resources.ApplyResources(this.perCent35UpDown, "perCent35UpDown");
            this.perCent35UpDown.Name = "perCent35UpDown";
            this.perCent35UpDown.Tag = "";
            this.perCent35UpDown.ValueChanged += new System.EventHandler(this.perCent35_ValueChanged);
            this.perCent35UpDown.Leave += new System.EventHandler(this.perCent35_ValueChanged);
            // 
            // perCentLabel3
            // 
            resources.ApplyResources(this.perCentLabel3, "perCentLabel3");
            this.perCentLabel3.Name = "perCentLabel3";
            this.perCentLabel3.Tag = "";
            // 
            // perCent3UpDown
            // 
            resources.ApplyResources(this.perCent3UpDown, "perCent3UpDown");
            this.perCent3UpDown.Name = "perCent3UpDown";
            this.perCent3UpDown.Tag = "";
            this.perCent3UpDown.ValueChanged += new System.EventHandler(this.perCent3_ValueChanged);
            this.perCent3UpDown.Leave += new System.EventHandler(this.perCent3_ValueChanged);
            // 
            // perCentLabel25
            // 
            resources.ApplyResources(this.perCentLabel25, "perCentLabel25");
            this.perCentLabel25.Name = "perCentLabel25";
            this.perCentLabel25.Tag = "";
            // 
            // perCent25UpDown
            // 
            resources.ApplyResources(this.perCent25UpDown, "perCent25UpDown");
            this.perCent25UpDown.Name = "perCent25UpDown";
            this.perCent25UpDown.Tag = "";
            this.perCent25UpDown.ValueChanged += new System.EventHandler(this.perCent25_ValueChanged);
            this.perCent25UpDown.Leave += new System.EventHandler(this.perCent25_ValueChanged);
            // 
            // perCentLabel2
            // 
            resources.ApplyResources(this.perCentLabel2, "perCentLabel2");
            this.perCentLabel2.Name = "perCentLabel2";
            this.perCentLabel2.Tag = "";
            // 
            // perCent2UpDown
            // 
            resources.ApplyResources(this.perCent2UpDown, "perCent2UpDown");
            this.perCent2UpDown.Name = "perCent2UpDown";
            this.perCent2UpDown.Tag = "";
            this.perCent2UpDown.ValueChanged += new System.EventHandler(this.perCent2_ValueChanged);
            this.perCent2UpDown.Leave += new System.EventHandler(this.perCent2_ValueChanged);
            // 
            // perCentLabel15
            // 
            resources.ApplyResources(this.perCentLabel15, "perCentLabel15");
            this.perCentLabel15.Name = "perCentLabel15";
            this.perCentLabel15.Tag = "";
            // 
            // perCent15UpDown
            // 
            resources.ApplyResources(this.perCent15UpDown, "perCent15UpDown");
            this.perCent15UpDown.Name = "perCent15UpDown";
            this.perCent15UpDown.Tag = "";
            this.perCent15UpDown.ValueChanged += new System.EventHandler(this.perCent15_ValueChanged);
            this.perCent15UpDown.Leave += new System.EventHandler(this.perCent15_ValueChanged);
            // 
            // perCentLabel1
            // 
            resources.ApplyResources(this.perCentLabel1, "perCentLabel1");
            this.perCentLabel1.Name = "perCentLabel1";
            this.perCentLabel1.Tag = "";
            // 
            // perCent1UpDown
            // 
            resources.ApplyResources(this.perCent1UpDown, "perCent1UpDown");
            this.perCent1UpDown.Name = "perCent1UpDown";
            this.perCent1UpDown.Tag = "";
            this.perCent1UpDown.ValueChanged += new System.EventHandler(this.perCent1_ValueChanged);
            this.perCent1UpDown.Leave += new System.EventHandler(this.perCent1_ValueChanged);
            // 
            // perCentLabel05
            // 
            resources.ApplyResources(this.perCentLabel05, "perCentLabel05");
            this.perCentLabel05.Name = "perCentLabel05";
            this.perCentLabel05.Tag = "";
            // 
            // perCent05UpDown
            // 
            resources.ApplyResources(this.perCent05UpDown, "perCent05UpDown");
            this.perCent05UpDown.Name = "perCent05UpDown";
            this.perCent05UpDown.Tag = "";
            this.perCent05UpDown.ValueChanged += new System.EventHandler(this.perCent05_ValueChanged);
            this.perCent05UpDown.Leave += new System.EventHandler(this.perCent05_ValueChanged);
            // 
            // buttonCalculate
            // 
            this.dirtyErrorProvider.SetError(this.buttonCalculate, resources.GetString("buttonCalculate.Error"));
            resources.ApplyResources(this.buttonCalculate, "buttonCalculate");
            this.buttonCalculate.Name = "buttonCalculate";
            this.buttonCalculate.Click += new System.EventHandler(this.buttonCalculate_Click);
            // 
            // labelSum
            // 
            resources.ApplyResources(this.labelSum, "labelSum");
            this.labelSum.Name = "labelSum";
            this.labelSum.Tag = "";
            // 
            // calculateThresholdsAtStartUpCheckBox
            // 
            resources.ApplyResources(this.calculateThresholdsAtStartUpCheckBox, "calculateThresholdsAtStartUpCheckBox");
            this.calculateThresholdsAtStartUpCheckBox.Name = "calculateThresholdsAtStartUpCheckBox";
            this.calculateThresholdsAtStartUpCheckBox.Tag = "#holdsAtStartUpCheckBoxLabel";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.groupBox1Label);
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
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            this.groupBox1.Tag = "#AutoRateCommand";
            // 
            // groupBox1Label
            // 
            resources.ApplyResources(this.groupBox1Label, "groupBox1Label");
            this.groupBox1Label.Name = "groupBox1Label";
            // 
            // labelTotalTracks
            // 
            resources.ApplyResources(this.labelTotalTracks, "labelTotalTracks");
            this.labelTotalTracks.Name = "labelTotalTracks";
            this.labelTotalTracks.Tag = "";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.groupBox2Label);
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
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            this.groupBox2.Tag = "#groupBox1@pinned-to-parent-x";
            // 
            // groupBox2Label
            // 
            resources.ApplyResources(this.groupBox2Label, "groupBox2Label");
            this.groupBox2Label.Name = "groupBox2Label";
            // 
            // checkBox05Label
            // 
            resources.ApplyResources(this.checkBox05Label, "checkBox05Label");
            this.checkBox05Label.Name = "checkBox05Label";
            this.checkBox05Label.Tag = "";
            this.checkBox05Label.Click += new System.EventHandler(this.checkBox05Label_Click);
            // 
            // checkBox1Label
            // 
            resources.ApplyResources(this.checkBox1Label, "checkBox1Label");
            this.checkBox1Label.Name = "checkBox1Label";
            this.checkBox1Label.Tag = "";
            this.checkBox1Label.Click += new System.EventHandler(this.checkBox1Label_Click);
            // 
            // checkBox15Label
            // 
            resources.ApplyResources(this.checkBox15Label, "checkBox15Label");
            this.checkBox15Label.Name = "checkBox15Label";
            this.checkBox15Label.Tag = "";
            this.checkBox15Label.Click += new System.EventHandler(this.checkBox15Label_Click);
            // 
            // checkBox2Label
            // 
            resources.ApplyResources(this.checkBox2Label, "checkBox2Label");
            this.checkBox2Label.Name = "checkBox2Label";
            this.checkBox2Label.Tag = "";
            this.checkBox2Label.Click += new System.EventHandler(this.checkBox2Label_Click);
            // 
            // checkBox25Label
            // 
            resources.ApplyResources(this.checkBox25Label, "checkBox25Label");
            this.checkBox25Label.Name = "checkBox25Label";
            this.checkBox25Label.Tag = "";
            this.checkBox25Label.Click += new System.EventHandler(this.checkBox25Label_Click);
            // 
            // checkBox3Label
            // 
            resources.ApplyResources(this.checkBox3Label, "checkBox3Label");
            this.checkBox3Label.Name = "checkBox3Label";
            this.checkBox3Label.Tag = "";
            this.checkBox3Label.Click += new System.EventHandler(this.checkBox3Label_Click);
            // 
            // checkBox35Label
            // 
            resources.ApplyResources(this.checkBox35Label, "checkBox35Label");
            this.checkBox35Label.Name = "checkBox35Label";
            this.checkBox35Label.Tag = "";
            this.checkBox35Label.Click += new System.EventHandler(this.checkBox35Label_Click);
            // 
            // checkBox4Label
            // 
            resources.ApplyResources(this.checkBox4Label, "checkBox4Label");
            this.checkBox4Label.Name = "checkBox4Label";
            this.checkBox4Label.Tag = "";
            this.checkBox4Label.Click += new System.EventHandler(this.checkBox4Label_Click);
            // 
            // checkBox45Label
            // 
            resources.ApplyResources(this.checkBox45Label, "checkBox45Label");
            this.checkBox45Label.Name = "checkBox45Label";
            this.checkBox45Label.Tag = "";
            this.checkBox45Label.Click += new System.EventHandler(this.checkBox45Label_Click);
            // 
            // checkBox5Label
            // 
            resources.ApplyResources(this.checkBox5Label, "checkBox5Label");
            this.checkBox5Label.Name = "checkBox5Label";
            this.checkBox5Label.Tag = "";
            this.checkBox5Label.Click += new System.EventHandler(this.checkBox5Label_Click);
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
            this.sinceAddedCheckBox.Name = "sinceAddedCheckBox";
            this.sinceAddedCheckBox.Tag = "#sinceAddedCheckBoxLabel";
            this.toolTip1.SetToolTip(this.sinceAddedCheckBox, resources.GetString("sinceAddedCheckBox.ToolTip"));
            // 
            // sinceAddedCheckBoxLabel
            // 
            resources.ApplyResources(this.sinceAddedCheckBoxLabel, "sinceAddedCheckBoxLabel");
            this.sinceAddedCheckBoxLabel.Name = "sinceAddedCheckBoxLabel";
            this.sinceAddedCheckBoxLabel.Tag = "";
            this.toolTip1.SetToolTip(this.sinceAddedCheckBoxLabel, resources.GetString("sinceAddedCheckBoxLabel.ToolTip"));
            this.sinceAddedCheckBoxLabel.Click += new System.EventHandler(this.sinceAddedCheckBoxLabel_Click);
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
            // notifyWhenAutoratingCompletedCheckBox
            // 
            resources.ApplyResources(this.notifyWhenAutoratingCompletedCheckBox, "notifyWhenAutoratingCompletedCheckBox");
            this.notifyWhenAutoratingCompletedCheckBox.Name = "notifyWhenAutoratingCompletedCheckBox";
            this.notifyWhenAutoratingCompletedCheckBox.Tag = "#notifyWhenAutoratingCompletedCheckBoxLabel";
            // 
            // playsPerDayTagList
            // 
            this.playsPerDayTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playsPerDayTagList.DropDownWidth = 250;
            this.playsPerDayTagList.FormattingEnabled = true;
            resources.ApplyResources(this.playsPerDayTagList, "playsPerDayTagList");
            this.playsPerDayTagList.Name = "playsPerDayTagList";
            this.playsPerDayTagList.Tag = "";
            // 
            // storePlaysPerDayCheckBox
            // 
            resources.ApplyResources(this.storePlaysPerDayCheckBox, "storePlaysPerDayCheckBox");
            this.storePlaysPerDayCheckBox.Checked = true;
            this.storePlaysPerDayCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.storePlaysPerDayCheckBox.Name = "storePlaysPerDayCheckBox";
            this.storePlaysPerDayCheckBox.Tag = "#storePlaysPerDayCheckBoxLabel";
            this.storePlaysPerDayCheckBox.CheckedChanged += new System.EventHandler(this.storePlaysPerDay_CheckedChanged);
            // 
            // storePlaysPerDayCheckBoxLabel
            // 
            resources.ApplyResources(this.storePlaysPerDayCheckBoxLabel, "storePlaysPerDayCheckBoxLabel");
            this.storePlaysPerDayCheckBoxLabel.Name = "storePlaysPerDayCheckBoxLabel";
            this.storePlaysPerDayCheckBoxLabel.Tag = "#playsPerDayTagList";
            this.storePlaysPerDayCheckBoxLabel.Click += new System.EventHandler(this.storePlaysPerDayCheckBoxLabel_Click);
            // 
            // autoRateAtStartUpCheckBoxLabel
            // 
            resources.ApplyResources(this.autoRateAtStartUpCheckBoxLabel, "autoRateAtStartUpCheckBoxLabel");
            this.autoRateAtStartUpCheckBoxLabel.Name = "autoRateAtStartUpCheckBoxLabel";
            this.autoRateAtStartUpCheckBoxLabel.Tag = "#notifyWhenAutoratingCompletedCheckBox";
            this.autoRateAtStartUpCheckBoxLabel.Click += new System.EventHandler(this.autoRateAtStartUpCheckBoxLabel_Click);
            // 
            // notifyWhenAutoratingCompletedCheckBoxLabel
            // 
            resources.ApplyResources(this.notifyWhenAutoratingCompletedCheckBoxLabel, "notifyWhenAutoratingCompletedCheckBoxLabel");
            this.notifyWhenAutoratingCompletedCheckBoxLabel.Name = "notifyWhenAutoratingCompletedCheckBoxLabel";
            this.notifyWhenAutoratingCompletedCheckBoxLabel.Tag = "";
            this.notifyWhenAutoratingCompletedCheckBoxLabel.Click += new System.EventHandler(this.notifyWhenAutoratingCompletedCheckBoxLabel_Click);
            // 
            // autoRateOnTrackPropertiesCheckBoxLabel
            // 
            resources.ApplyResources(this.autoRateOnTrackPropertiesCheckBoxLabel, "autoRateOnTrackPropertiesCheckBoxLabel");
            this.autoRateOnTrackPropertiesCheckBoxLabel.Name = "autoRateOnTrackPropertiesCheckBoxLabel";
            this.autoRateOnTrackPropertiesCheckBoxLabel.Tag = "";
            this.autoRateOnTrackPropertiesCheckBoxLabel.Click += new System.EventHandler(this.autoRateOnTrackPropertiesCheckBoxLabel_Click);
            // 
            // holdsAtStartUpCheckBoxLabel
            // 
            resources.ApplyResources(this.holdsAtStartUpCheckBoxLabel, "holdsAtStartUpCheckBoxLabel");
            this.holdsAtStartUpCheckBoxLabel.Name = "holdsAtStartUpCheckBoxLabel";
            this.holdsAtStartUpCheckBoxLabel.Tag = "";
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
            this.Controls.Add(this.buttonSettings);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.baseRatingTrackBar);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.sinceAddedCheckBoxLabel);
            this.Controls.Add(this.sinceAddedCheckBox);
            this.Controls.Add(this.autoRateOnTrackPropertiesCheckBoxLabel);
            this.Controls.Add(this.autoRateOnTrackPropertiesCheckBox);
            this.Controls.Add(this.notifyWhenAutoratingCompletedCheckBoxLabel);
            this.Controls.Add(this.notifyWhenAutoratingCompletedCheckBox);
            this.Controls.Add(this.autoRateAtStartUpCheckBoxLabel);
            this.Controls.Add(this.autoRateAtStartUpCheckBox);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.playsPerDayTagList);
            this.Controls.Add(this.storePlaysPerDayCheckBoxLabel);
            this.Controls.Add(this.storePlaysPerDayCheckBox);
            this.Controls.Add(this.holdsAtStartUpCheckBoxLabel);
            this.Controls.Add(this.calculateThresholdsAtStartUpCheckBox);
            this.Controls.Add(this.autoRatingTagList);
            this.Controls.Add(this.label4);
            this.Name = "AutoRateCommand";
            this.Tag = "@min-max-width-same@min-max-height-same";
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
        private Label groupBox1Label;
        private Label groupBox2Label;
        private Button buttonSettings;
    }
}