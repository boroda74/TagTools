﻿namespace MusicBeePlugin
{
    partial class PluginSampleWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginSampleWindow));
            this.squareButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // squareButton
            // 
            resources.ApplyResources(this.squareButton, "squareButton");
            this.squareButton.Name = "squareButton";
            this.squareButton.Tag = "@square-control";
            this.squareButton.UseVisualStyleBackColor = true;
            // 
            // PluginSampleWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.squareButton);
            this.Name = "PluginSampleWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button squareButton;
    }
}