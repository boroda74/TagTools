namespace MusicBeePlugin
{
    partial class ChangeCasePresetNaming
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangeCasePresetNaming));
            this.label1 = new System.Windows.Forms.Label();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.descriptionBox = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.languages = new System.Windows.Forms.ComboBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.textBoxTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();

            //MusicBee
            this.nameBox = ControlsTools.CreateMusicBeeTextBox();
            this.descriptionBox = ControlsTools.CreateMusicBeeTextBox();
            //~MusicBee

            this.textBoxTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.label1.Tag = "@pinned-to-parent-x";
            // 
            // nameBox
            // 
            resources.ApplyResources(this.nameBox, "nameBox");
            this.nameBox.Name = "nameBox";
            this.nameBox.Tag = "#ChangeCasePresetNaming";
            this.nameBox.TextChanged += new System.EventHandler(this.nameBox_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            this.label2.Tag = "@pinned-to-parent-x";
            // 
            // descriptionBox
            // 
            resources.ApplyResources(this.descriptionBox, "descriptionBox");
            this.descriptionBox.Name = "descriptionBox";
            this.descriptionBox.Tag = "";
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Tag = "#buttonClose";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonClose
            // 
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Tag = "#ChangeCasePresetNaming@non-defaultable";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // languages
            // 
            this.languages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.languages.FormattingEnabled = true;
            resources.ApplyResources(this.languages, "languages");
            this.languages.Name = "languages";
            this.languages.Tag = "@pinned-to-parent-x";
            this.languages.SelectedIndexChanged += new System.EventHandler(this.languages_SelectedIndexChanged);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 1500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // textBoxTableLayoutPanel
            // 
            resources.ApplyResources(this.textBoxTableLayoutPanel, "textBoxTableLayoutPanel");
            this.textBoxTableLayoutPanel.Controls.Add(this.descriptionBox, 0, 0);
            this.textBoxTableLayoutPanel.Name = "textBoxTableLayoutPanel";
            this.textBoxTableLayoutPanel.Tag = "#ChangeCasePresetNaming";
            // 
            // ChangeCasePresetNaming
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxTableLayoutPanel);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.languages);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonOK);
            this.DoubleBuffered = true;
            this.Name = "ChangeCasePresetNaming";
            this.Tag = "@min-max-width-same@min-max-height-same";
            this.Load += new System.EventHandler(this.ChangeCasePresetNaming_Load);
            this.textBoxTableLayoutPanel.ResumeLayout(false);
            this.textBoxTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox descriptionBox;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.ComboBox languages;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TableLayoutPanel textBoxTableLayoutPanel;
    }
}