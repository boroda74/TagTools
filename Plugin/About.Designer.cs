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
            this.creditLabel = new System.Windows.Forms.Label();
            this.creditLinkLabel = new System.Windows.Forms.LinkLabel();
            this.iconSetLabel = new System.Windows.Forms.Label();
            this.iconSetLinkLabel = new System.Windows.Forms.LinkLabel();
            this.toolbarButtonsLabel = new System.Windows.Forms.Label();
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
            // creditLabel
            // 
            resources.ApplyResources(this.creditLabel, "creditLabel");
            this.creditLabel.Name = "creditLabel";
            this.creditLabel.Tag = "#creditLinkLabel@pinned-to-parent-x";
            // 
            // creditLinkLabel
            // 
            this.creditLinkLabel.ActiveLinkColor = System.Drawing.SystemColors.MenuHighlight;
            resources.ApplyResources(this.creditLinkLabel, "creditLinkLabel");
            this.creditLinkLabel.LinkColor = System.Drawing.SystemColors.HotTrack;
            this.creditLinkLabel.Name = "creditLinkLabel";
            this.creditLinkLabel.TabStop = true;
            this.creditLinkLabel.Tag = "#iconSetLabel";
            this.creditLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.creditLinkLabel_LinkClicked);
            // 
            // iconSetLabel
            // 
            resources.ApplyResources(this.iconSetLabel, "iconSetLabel");
            this.iconSetLabel.Name = "iconSetLabel";
            this.iconSetLabel.Tag = "#iconSetLinkLabel";
            // 
            // iconSetLinkLabel
            // 
            this.iconSetLinkLabel.ActiveLinkColor = System.Drawing.SystemColors.MenuHighlight;
            resources.ApplyResources(this.iconSetLinkLabel, "iconSetLinkLabel");
            this.iconSetLinkLabel.LinkColor = System.Drawing.SystemColors.HotTrack;
            this.iconSetLinkLabel.Name = "iconSetLinkLabel";
            this.iconSetLinkLabel.TabStop = true;
            this.iconSetLinkLabel.Tag = "#toolbarButtonsLabel";
            this.iconSetLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.iconSetLinkLabel_LinkClicked);
            // 
            // toolbarButtonsLabel
            // 
            resources.ApplyResources(this.toolbarButtonsLabel, "toolbarButtonsLabel");
            this.toolbarButtonsLabel.Name = "toolbarButtonsLabel";
            this.toolbarButtonsLabel.Tag = "";
            // 
            // About
            // 
            this.AcceptButton = this.buttonClose;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.Controls.Add(this.toolbarButtonsLabel);
            this.Controls.Add(this.iconSetLinkLabel);
            this.Controls.Add(this.iconSetLabel);
            this.Controls.Add(this.creditLinkLabel);
            this.Controls.Add(this.creditLabel);
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
        private System.Windows.Forms.Label creditLabel;
        private System.Windows.Forms.LinkLabel creditLinkLabel;
        private System.Windows.Forms.Label iconSetLabel;
        private System.Windows.Forms.LinkLabel iconSetLinkLabel;
        private System.Windows.Forms.Label toolbarButtonsLabel;
    }
}