﻿namespace MusicBeePlugin
{
    partial class LrFunctionPrecaching
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
        }

        #region Код, автоматически созданный конструктором форм Windows

        ///<summary>
        ///Обязательный метод для поддержки конструктора - не изменяйте
        ///содержимое данного метода при помощи редактора кода.
        ///</summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LrFunctionPrecaching));
            this.progressInfoLabel = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.anotherLrPresetCompleteTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // progressInfoLabel
            // 
            resources.ApplyResources(this.progressInfoLabel, "progressInfoLabel");
            this.progressInfoLabel.Name = "progressInfoLabel";
            this.progressInfoLabel.Tag = "#LrFunctionPrecaching&LrFunctionPrecaching@pinned-to-parent-x@pinned-to-parent-y";
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Tag = "#LrFunctionPrecaching&LrFunctionPrecaching";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // LrFunctionPrecaching
            // 
            this.AcceptButton = this.buttonCancel;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.progressInfoLabel);
            this.DoubleBuffered = true;
            this.Name = "LrFunctionPrecaching";
            this.Tag = "@min-max-width-same@min-max-height-same";
            this.Shown += new System.EventHandler(this.LrFunctionPrecaching_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label progressInfoLabel;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Timer anotherLrPresetCompleteTimer;
    }
}