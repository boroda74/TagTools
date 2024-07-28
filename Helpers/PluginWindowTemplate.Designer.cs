namespace MusicBeePlugin
{
    partial class PluginWindowTemplate
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

            if (disposing)
            {
                base.Dispose(disposing);

                foreach (var customCombox in namesComboBoxes.Values)
                    customCombox.Dispose();
            }
        }

        #region Код, автоматически созданный конструктором форм Windows

        ///<summary>
        ///Обязательный метод для поддержки конструктора - не изменяйте
        ///содержимое данного метода при помощи редактора кода.
        ///</summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginWindowTemplate));
            this.SuspendLayout();
            // 
            // PluginWindowTemplate
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.Name = "PluginWindowTemplate";
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PluginWindowTemplate_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PluginWindowTemplate_FormClosed);
            this.Load += new System.EventHandler(this.PluginWindowTemplate_Load);
            this.Shown += new System.EventHandler(this.PluginWindowTemplate_Shown);
            this.VisibleChanged += new System.EventHandler(this.PluginWindowTemplate_VisibleChanged);
            this.Move += new System.EventHandler(this.PluginWindowTemplate_Move);
            this.Resize += new System.EventHandler(this.PluginWindowTemplate_Resize);
            this.ResumeLayout(false);

        }

        #endregion

    }
}