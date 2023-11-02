namespace MusicBeePlugin
{
    partial class CalculateAverageAlbumRatingCommand
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalculateAverageAlbumRatingCommand));
            this.notifyWhenCalculationCompletedCheckBox = new System.Windows.Forms.CheckBox();
            this.calculateAlbumRatingAtStartUpCheckBox = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.considerUnratedCheckBox = new System.Windows.Forms.CheckBox();
            this.calculateAlbumRatingAtTagsChangedCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.trackRatingTagList = new System.Windows.Forms.ComboBox();
            this.albumRatingTagList = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.calculateAlbumRatingAtStartUpCheckBoxLabel = new System.Windows.Forms.Label();
            this.considerUnratedCheckBoxLabel = new System.Windows.Forms.Label();
            this.calculateAlbumRatingAtTagsChangedCheckBoxLabel = new System.Windows.Forms.Label();
            this.notifyWhenCalculationCompletedCheckBoxLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // notifyWhenCalculationCompletedCheckBox
            // 
            resources.ApplyResources(this.notifyWhenCalculationCompletedCheckBox, "notifyWhenCalculationCompletedCheckBox");
            this.notifyWhenCalculationCompletedCheckBox.Name = "notifyWhenCalculationCompletedCheckBox";
            this.notifyWhenCalculationCompletedCheckBox.Tag = "notifyWhenCalculationCompletedCheckBoxLabel";
            this.notifyWhenCalculationCompletedCheckBox.UseVisualStyleBackColor = true;
            // 
            // calculateAlbumRatingAtStartUpCheckBox
            // 
            resources.ApplyResources(this.calculateAlbumRatingAtStartUpCheckBox, "calculateAlbumRatingAtStartUpCheckBox");
            this.calculateAlbumRatingAtStartUpCheckBox.Name = "calculateAlbumRatingAtStartUpCheckBox";
            this.calculateAlbumRatingAtStartUpCheckBox.Tag = "calculateAlbumRatingAtStartUpCheckBoxLabel";
            this.calculateAlbumRatingAtStartUpCheckBox.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // considerUnratedCheckBox
            // 
            resources.ApplyResources(this.considerUnratedCheckBox, "considerUnratedCheckBox");
            this.considerUnratedCheckBox.Name = "considerUnratedCheckBox";
            this.considerUnratedCheckBox.Tag = "considerUnratedCheckBoxLabel";
            this.considerUnratedCheckBox.UseVisualStyleBackColor = true;
            // 
            // calculateAlbumRatingAtTagsChangedCheckBox
            // 
            resources.ApplyResources(this.calculateAlbumRatingAtTagsChangedCheckBox, "calculateAlbumRatingAtTagsChangedCheckBox");
            this.calculateAlbumRatingAtTagsChangedCheckBox.Name = "calculateAlbumRatingAtTagsChangedCheckBox";
            this.calculateAlbumRatingAtTagsChangedCheckBox.Tag = "calculateAlbumRatingAtTagsChangedCheckBoxLabel";
            this.calculateAlbumRatingAtTagsChangedCheckBox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // trackRatingTagList
            // 
            resources.ApplyResources(this.trackRatingTagList, "trackRatingTagList");
            this.trackRatingTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.trackRatingTagList.DropDownWidth = 250;
            this.trackRatingTagList.FormattingEnabled = true;
            this.trackRatingTagList.Name = "trackRatingTagList";
            // 
            // albumRatingTagList
            // 
            resources.ApplyResources(this.albumRatingTagList, "albumRatingTagList");
            this.albumRatingTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.albumRatingTagList.DropDownWidth = 250;
            this.albumRatingTagList.FormattingEnabled = true;
            this.albumRatingTagList.Name = "albumRatingTagList";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // calculateAlbumRatingAtStartUpCheckBoxLabel
            // 
            resources.ApplyResources(this.calculateAlbumRatingAtStartUpCheckBoxLabel, "calculateAlbumRatingAtStartUpCheckBoxLabel");
            this.calculateAlbumRatingAtStartUpCheckBoxLabel.Name = "calculateAlbumRatingAtStartUpCheckBoxLabel";
            this.calculateAlbumRatingAtStartUpCheckBoxLabel.Click += new System.EventHandler(this.calculateAlbumRatingAtStartUpCheckBoxLabel_Click);
            // 
            // considerUnratedCheckBoxLabel
            // 
            resources.ApplyResources(this.considerUnratedCheckBoxLabel, "considerUnratedCheckBoxLabel");
            this.considerUnratedCheckBoxLabel.Name = "considerUnratedCheckBoxLabel";
            this.considerUnratedCheckBoxLabel.Click += new System.EventHandler(this.considerUnratedCheckBoxLabel_Click);
            // 
            // calculateAlbumRatingAtTagsChangedCheckBoxLabel
            // 
            resources.ApplyResources(this.calculateAlbumRatingAtTagsChangedCheckBoxLabel, "calculateAlbumRatingAtTagsChangedCheckBoxLabel");
            this.calculateAlbumRatingAtTagsChangedCheckBoxLabel.Name = "calculateAlbumRatingAtTagsChangedCheckBoxLabel";
            this.calculateAlbumRatingAtTagsChangedCheckBoxLabel.Click += new System.EventHandler(this.calculateAlbumRatingAtTagsChangedCheckBoxLabel_Click);
            // 
            // notifyWhenCalculationCompletedCheckBoxLabel
            // 
            resources.ApplyResources(this.notifyWhenCalculationCompletedCheckBoxLabel, "notifyWhenCalculationCompletedCheckBoxLabel");
            this.notifyWhenCalculationCompletedCheckBoxLabel.Name = "notifyWhenCalculationCompletedCheckBoxLabel";
            this.notifyWhenCalculationCompletedCheckBoxLabel.Click += new System.EventHandler(this.notifyWhenCalculationCompletedCheckBoxLabel_Click);
            // 
            // CalculateAverageAlbumRatingCommand
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.notifyWhenCalculationCompletedCheckBoxLabel);
            this.Controls.Add(this.calculateAlbumRatingAtTagsChangedCheckBoxLabel);
            this.Controls.Add(this.considerUnratedCheckBoxLabel);
            this.Controls.Add(this.calculateAlbumRatingAtStartUpCheckBoxLabel);
            this.Controls.Add(this.albumRatingTagList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.trackRatingTagList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.calculateAlbumRatingAtTagsChangedCheckBox);
            this.Controls.Add(this.considerUnratedCheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.notifyWhenCalculationCompletedCheckBox);
            this.Controls.Add(this.calculateAlbumRatingAtStartUpCheckBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CalculateAverageAlbumRatingCommand";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox notifyWhenCalculationCompletedCheckBox;
        private System.Windows.Forms.CheckBox calculateAlbumRatingAtStartUpCheckBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox considerUnratedCheckBox;
        private System.Windows.Forms.CheckBox calculateAlbumRatingAtTagsChangedCheckBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox trackRatingTagList;
        private System.Windows.Forms.ComboBox albumRatingTagList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label calculateAlbumRatingAtStartUpCheckBoxLabel;
        private System.Windows.Forms.Label considerUnratedCheckBoxLabel;
        private System.Windows.Forms.Label calculateAlbumRatingAtTagsChangedCheckBoxLabel;
        private System.Windows.Forms.Label notifyWhenCalculationCompletedCheckBoxLabel;
    }
}