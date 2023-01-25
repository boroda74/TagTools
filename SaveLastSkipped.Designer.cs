namespace MusicBeePlugin
{
    partial class SaveLastSkippedCommand
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveLastSkippedCommand));
            this.lastSkippedTagList = new System.Windows.Forms.ComboBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.saveLastSkippedCheckBox = new System.Windows.Forms.CheckBox();
            this.dirtyErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.lastSkippedDateFormatTagList = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // lastSkippedTagList
            // 
            this.lastSkippedTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lastSkippedTagList.DropDownWidth = 250;
            resources.ApplyResources(this.lastSkippedTagList, "lastSkippedTagList");
            this.lastSkippedTagList.FormattingEnabled = true;
            this.lastSkippedTagList.Name = "lastSkippedTagList";
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
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
            // saveLastSkippedCheckBox
            // 
            resources.ApplyResources(this.saveLastSkippedCheckBox, "saveLastSkippedCheckBox");
            this.saveLastSkippedCheckBox.Name = "saveLastSkippedCheckBox";
            this.saveLastSkippedCheckBox.UseVisualStyleBackColor = true;
            this.saveLastSkippedCheckBox.CheckedChanged += new System.EventHandler(this.saveLastSkippedCheckBox_CheckedChanged);
            // 
            // dirtyErrorProvider
            // 
            this.dirtyErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.dirtyErrorProvider.ContainerControl = this;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lastSkippedDateFormatTagList
            // 
            this.lastSkippedDateFormatTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lastSkippedDateFormatTagList.DropDownWidth = 250;
            this.lastSkippedDateFormatTagList.FormattingEnabled = true;
            resources.ApplyResources(this.lastSkippedDateFormatTagList, "lastSkippedDateFormatTagList");
            this.lastSkippedDateFormatTagList.Name = "lastSkippedDateFormatTagList";
            // 
            // SaveLastSkippedCommand
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.lastSkippedDateFormatTagList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.saveLastSkippedCheckBox);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.lastSkippedTagList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "SaveLastSkippedCommand";
            ((System.ComponentModel.ISupportInitialize)(this.dirtyErrorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox lastSkippedTagList;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.CheckBox saveLastSkippedCheckBox;
        private System.Windows.Forms.ErrorProvider dirtyErrorProvider;
        private System.Windows.Forms.ComboBox lastSkippedDateFormatTagList;
        private System.Windows.Forms.Label label1;
    }
}