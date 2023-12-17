namespace MusicBeePlugin
{
    partial class SaveLastSkippedCommand
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveLastSkippedCommand));
            this.lastSkippedTagList = new System.Windows.Forms.ComboBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.saveLastSkippedCheckBox = new System.Windows.Forms.CheckBox();
            this.dateTimeFormatLabel = new System.Windows.Forms.Label();
            this.lastSkippedDateFormatTagList = new System.Windows.Forms.ComboBox();
            this.saveLastSkippedCheckBoxLabel = new System.Windows.Forms.Label();
            this.placeholderCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lastSkippedTagList
            // 
            resources.ApplyResources(this.lastSkippedTagList, "lastSkippedTagList");
            this.lastSkippedTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lastSkippedTagList.DropDownWidth = 250;
            this.lastSkippedTagList.FormattingEnabled = true;
            this.lastSkippedTagList.Name = "lastSkippedTagList";
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Tag = "@non-defaultable";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // saveLastSkippedCheckBox
            // 
            resources.ApplyResources(this.saveLastSkippedCheckBox, "saveLastSkippedCheckBox");
            this.saveLastSkippedCheckBox.Name = "saveLastSkippedCheckBox";
            this.saveLastSkippedCheckBox.Tag = "#saveLastSkippedCheckBoxLabel";
            this.saveLastSkippedCheckBox.CheckedChanged += new System.EventHandler(this.saveLastSkippedCheckBox_CheckedChanged);
            // 
            // dateTimeFormatLabel
            // 
            resources.ApplyResources(this.dateTimeFormatLabel, "dateTimeFormatLabel");
            this.dateTimeFormatLabel.Name = "dateTimeFormatLabel";
            // 
            // lastSkippedDateFormatTagList
            // 
            resources.ApplyResources(this.lastSkippedDateFormatTagList, "lastSkippedDateFormatTagList");
            this.lastSkippedDateFormatTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lastSkippedDateFormatTagList.DropDownWidth = 250;
            this.lastSkippedDateFormatTagList.FormattingEnabled = true;
            this.lastSkippedDateFormatTagList.Name = "lastSkippedDateFormatTagList";
            // 
            // saveLastSkippedCheckBoxLabel
            // 
            resources.ApplyResources(this.saveLastSkippedCheckBoxLabel, "saveLastSkippedCheckBoxLabel");
            this.saveLastSkippedCheckBoxLabel.Name = "saveLastSkippedCheckBoxLabel";
            this.saveLastSkippedCheckBoxLabel.Click += new System.EventHandler(this.saveLastSkippedCheckBoxLabel_Click);
            // 
            // placeholderCheckBox
            // 
            resources.ApplyResources(this.placeholderCheckBox, "placeholderCheckBox");
            this.placeholderCheckBox.Name = "placeholderCheckBox";
            this.placeholderCheckBox.Tag = "#dateTimeFormatLabel";
            // 
            // SaveLastSkippedCommand
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.lastSkippedDateFormatTagList);
            this.Controls.Add(this.dateTimeFormatLabel);
            this.Controls.Add(this.placeholderCheckBox);
            this.Controls.Add(this.lastSkippedTagList);
            this.Controls.Add(this.saveLastSkippedCheckBoxLabel);
            this.Controls.Add(this.saveLastSkippedCheckBox);
            this.MaximizeBox = false;
            this.Name = "SaveLastSkippedCommand";
            this.Tag = "@min-max-width-same@min-max-height-same";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox lastSkippedTagList;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.CheckBox saveLastSkippedCheckBox;
        private System.Windows.Forms.ComboBox lastSkippedDateFormatTagList;
        private System.Windows.Forms.Label dateTimeFormatLabel;
        private System.Windows.Forms.Label saveLastSkippedCheckBoxLabel;
        private System.Windows.Forms.CheckBox placeholderCheckBox;
    }
}