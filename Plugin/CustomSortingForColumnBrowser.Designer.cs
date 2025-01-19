
using System.Windows.Forms;

namespace MusicBeePlugin
{
    partial class CustomSortingForColumnBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomSortingForColumnBrowser));
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.tagComboBox = new System.Windows.Forms.ComboBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.buttonCopyLabel = new System.Windows.Forms.Label();
            this.autoCopyCheckBoxLabel = new System.Windows.Forms.Label();
            this.autoCopyCheckBox = new System.Windows.Forms.CheckBox();
            this.buttonOverwrite = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.controlsPanel = new System.Windows.Forms.Panel();
            this.buttonDownMore = new System.Windows.Forms.Button();
            this.buttonUpMore = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.tagList = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.sourceTagComboBox = new System.Windows.Forms.ComboBox();
            this.buttonCopy = new System.Windows.Forms.Button();

            //MusicBee
            this.tagList.Dispose();

            this.tagList = new CustomListBox(Plugin.SavedSettings.dontUseSkinColors);
            //~MusicBee

            this.controlsPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Tag = "#buttonClose&controlsPanel";
            this.toolTip1.SetToolTip(this.buttonOK, resources.GetString("buttonOK.ToolTip"));
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonClose
            // 
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Tag = "#controlsPanel&controlsPanel";
            this.toolTip1.SetToolTip(this.buttonClose, resources.GetString("buttonClose.ToolTip"));
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // tagComboBox
            // 
            resources.ApplyResources(this.tagComboBox, "tagComboBox");
            this.tagComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tagComboBox.FormattingEnabled = true;
            this.tagComboBox.Name = "tagComboBox";
            this.tagComboBox.Tag = "";
            this.toolTip1.SetToolTip(this.tagComboBox, resources.GetString("tagComboBox.ToolTip"));
            this.tagComboBox.SelectedIndexChanged += new System.EventHandler(this.tagComboBox_SelectedIndexChanged);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 15000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // buttonCopyLabel
            // 
            resources.ApplyResources(this.buttonCopyLabel, "buttonCopyLabel");
            this.buttonCopyLabel.Name = "buttonCopyLabel";
            this.buttonCopyLabel.Tag = "#buttonCopy";
            this.toolTip1.SetToolTip(this.buttonCopyLabel, resources.GetString("buttonCopyLabel.ToolTip"));
            // 
            // autoCopyCheckBoxLabel
            // 
            resources.ApplyResources(this.autoCopyCheckBoxLabel, "autoCopyCheckBoxLabel");
            this.autoCopyCheckBoxLabel.Name = "autoCopyCheckBoxLabel";
            this.autoCopyCheckBoxLabel.Tag = "#buttonOverwrite";
            this.toolTip1.SetToolTip(this.autoCopyCheckBoxLabel, resources.GetString("autoCopyCheckBoxLabel.ToolTip"));
            this.autoCopyCheckBoxLabel.Click += new System.EventHandler(this.autoCopyCheckBoxLabel_Click);
            // 
            // autoCopyCheckBox
            // 
            resources.ApplyResources(this.autoCopyCheckBox, "autoCopyCheckBox");
            this.autoCopyCheckBox.Name = "autoCopyCheckBox";
            this.autoCopyCheckBox.Tag = "#autoCopyCheckBoxLabel";
            this.toolTip1.SetToolTip(this.autoCopyCheckBox, resources.GetString("autoCopyCheckBox.ToolTip"));
            this.autoCopyCheckBox.UseVisualStyleBackColor = true;
            this.autoCopyCheckBox.CheckedChanged += new System.EventHandler(this.autoCopyCheckBox_CheckedChanged);
            // 
            // buttonOverwrite
            // 
            resources.ApplyResources(this.buttonOverwrite, "buttonOverwrite");
            this.buttonOverwrite.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonOverwrite.Name = "buttonOverwrite";
            this.buttonOverwrite.Tag = "";
            this.toolTip1.SetToolTip(this.buttonOverwrite, resources.GetString("buttonOverwrite.ToolTip"));
            this.buttonOverwrite.Click += new System.EventHandler(this.buttonOverwrite_Click);
            // 
            // buttonLoad
            // 
            resources.ApplyResources(this.buttonLoad, "buttonLoad");
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Tag = "#buttonOK&controlsPanel";
            this.toolTip1.SetToolTip(this.buttonLoad, resources.GetString("buttonLoad.ToolTip"));
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // controlsPanel
            // 
            resources.ApplyResources(this.controlsPanel, "controlsPanel");
            this.controlsPanel.Controls.Add(this.buttonLoad);
            this.controlsPanel.Controls.Add(this.buttonDownMore);
            this.controlsPanel.Controls.Add(this.buttonUpMore);
            this.controlsPanel.Controls.Add(this.buttonDown);
            this.controlsPanel.Controls.Add(this.buttonUp);
            this.controlsPanel.Controls.Add(this.buttonClose);
            this.controlsPanel.Controls.Add(this.buttonOK);
            this.controlsPanel.Name = "controlsPanel";
            this.controlsPanel.Tag = "#CustomSortingForColumnBrowser&CustomSortingForColumnBrowser";
            this.toolTip1.SetToolTip(this.controlsPanel, resources.GetString("controlsPanel.ToolTip"));
            // 
            // buttonDownMore
            // 
            resources.ApplyResources(this.buttonDownMore, "buttonDownMore");
            this.buttonDownMore.Name = "buttonDownMore";
            this.buttonDownMore.Tag = "&controlsPanel@non-defaultable@square-button";
            this.toolTip1.SetToolTip(this.buttonDownMore, resources.GetString("buttonDownMore.ToolTip"));
            this.buttonDownMore.Click += new System.EventHandler(this.buttonDownMore_Click);
            // 
            // buttonUpMore
            // 
            resources.ApplyResources(this.buttonUpMore, "buttonUpMore");
            this.buttonUpMore.Name = "buttonUpMore";
            this.buttonUpMore.Tag = "#buttonUp&controlsPanel@non-defaultable@square-button@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.buttonUpMore, resources.GetString("buttonUpMore.ToolTip"));
            this.buttonUpMore.Click += new System.EventHandler(this.buttonUpMore_Click);
            // 
            // buttonDown
            // 
            resources.ApplyResources(this.buttonDown, "buttonDown");
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Tag = "#buttonDownMore&controlsPanel@non-defaultable@square-button";
            this.toolTip1.SetToolTip(this.buttonDown, resources.GetString("buttonDown.ToolTip"));
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // buttonUp
            // 
            resources.ApplyResources(this.buttonUp, "buttonUp");
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Tag = "#buttonDown&controlsPanel@non-defaultable@square-button@pinned-to-parent-x";
            this.toolTip1.SetToolTip(this.buttonUp, resources.GetString("buttonUp.ToolTip"));
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // infoLabel
            // 
            resources.ApplyResources(this.infoLabel, "infoLabel");
            this.infoLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Tag = "";
            this.toolTip1.SetToolTip(this.infoLabel, resources.GetString("infoLabel.ToolTip"));
            // 
            // tagList
            // 
            resources.ApplyResources(this.tagList, "tagList");
            this.tagList.Name = "tagList";
            this.tagList.Sorted = true;
            this.toolTip1.SetToolTip(this.tagList, resources.GetString("tagList.ToolTip"));
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.tagList, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Tag = "#CustomSortingForColumnBrowser&controlsPanel";
            this.toolTip1.SetToolTip(this.tableLayoutPanel1, resources.GetString("tableLayoutPanel1.ToolTip"));
            // 
            // sourceTagComboBox
            // 
            resources.ApplyResources(this.sourceTagComboBox, "sourceTagComboBox");
            this.sourceTagComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceTagComboBox.FormattingEnabled = true;
            this.sourceTagComboBox.Name = "sourceTagComboBox";
            this.sourceTagComboBox.Tag = "";
            this.toolTip1.SetToolTip(this.sourceTagComboBox, resources.GetString("sourceTagComboBox.ToolTip"));
            this.sourceTagComboBox.SelectedIndexChanged += new System.EventHandler(this.sourceTagComboBox_SelectedIndexChanged);
            // 
            // buttonCopy
            // 
            resources.ApplyResources(this.buttonCopy, "buttonCopy");
            this.buttonCopy.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Tag = "#autoCopyCheckBox";
            this.toolTip1.SetToolTip(this.buttonCopy, resources.GetString("buttonCopy.ToolTip"));
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Clicked);
            // 
            // CustomSortingForColumnBrowser
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.Controls.Add(this.buttonOverwrite);
            this.Controls.Add(this.autoCopyCheckBoxLabel);
            this.Controls.Add(this.autoCopyCheckBox);
            this.Controls.Add(this.buttonCopy);
            this.Controls.Add(this.sourceTagComboBox);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.buttonCopyLabel);
            this.Controls.Add(this.controlsPanel);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.tagComboBox);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomSortingForColumnBrowser";
            this.Tag = "";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.controlsPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.ComboBox tagComboBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private Panel controlsPanel;
        private Label infoLabel;
        private Label buttonCopyLabel;
        private ListBox tagList;
        private TableLayoutPanel tableLayoutPanel1;
        private Button buttonDown;
        private Button buttonUp;
        private ComboBox sourceTagComboBox;
        private Button buttonCopy;
        private Button buttonDownMore;
        private Button buttonUpMore;
        private CheckBox autoCopyCheckBox;
        private Label autoCopyCheckBoxLabel;
        private Button buttonOverwrite;
        private Button buttonLoad;
    }
}