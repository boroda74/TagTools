namespace MusicBeePlugin
{
    partial class PluginWindowTemplate
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
            try
            {
                base.Dispose(disposing);
            }
            catch { }
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginWindowTemplate));
            this.SuspendLayout();
            // 
            // PluginWindowTemplate
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "PluginWindowTemplate";
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ToolsPluginTemplate_FormClosing);
            this.Load += new System.EventHandler(this.PluginWindowTemplate_Load);
            this.VisibleChanged += new System.EventHandler(this.ToolsPluginTemplate_VisibleChanged);
            this.Resize += new System.EventHandler(this.ToolsPluginTemplate_Resize);
            this.ResumeLayout(false);

        }

        #endregion

    }
}