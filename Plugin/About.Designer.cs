namespace MusicBeePlugin
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.buttonClose = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.buttonsPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Tag = "@pinned-to-parent-x@pinned-to-parent-y";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            this.label6.Tag = "@pinned-to-parent-x";
            // 
            // versionLabel
            // 
            resources.ApplyResources(this.versionLabel, "versionLabel");
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Tag = "@pinned-to-parent-x@pinned-to-parent-y";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Name = "label1";
            this.label1.Tag = "@pinned-to-parent-x";
            // 
            // buttonsPanel
            // 
            resources.ApplyResources(this.buttonsPanel, "buttonsPanel");
            this.buttonsPanel.Controls.Add(this.buttonClose);
            this.buttonsPanel.Controls.Add(this.versionLabel);
            this.buttonsPanel.Name = "buttonsPanel";
            this.buttonsPanel.Tag = "#About&About@pinned-to-parent-x";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            this.label2.Tag = "@pinned-to-parent-x";
            // 
            // About
            // 
            this.AcceptButton = this.buttonClose;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonsPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label6);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.Tag = "@min-max-width-same@min-max-height-same";
            this.buttonsPanel.ResumeLayout(false);
            this.buttonsPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel buttonsPanel;
        private System.Windows.Forms.Label label2;
    }
}