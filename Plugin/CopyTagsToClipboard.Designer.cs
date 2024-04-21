
using System.Windows.Forms;

namespace MusicBeePlugin
{
    partial class CopyTagsToClipboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CopyTagsToClipboard));
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.sourceTagList = new System.Windows.Forms.ListBox();
            this.checkedSourceTagList = new System.Windows.Forms.ListBox();
            this.uncheckedSourceTagLabel = new System.Windows.Forms.Label();
            this.checkedSourceTagLabel = new System.Windows.Forms.Label();
            this.tagSetComboBox = new System.Windows.Forms.ComboBox();
            this.checkUncheckAllCheckBox = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.checkUncheckAllCheckBoxLabel = new System.Windows.Forms.Label();
            this.controlsPanel = new System.Windows.Forms.Panel();
            this.infoLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.controlsPanel.SuspendLayout();
            this.SuspendLayout();

            //MusicBee
            this.checkedSourceTagList = new CustomListBox(Plugin.SavedSettings.dontUseSkinColors);
            this.sourceTagList = new CustomListBox(Plugin.SavedSettings.dontUseSkinColors);
            //~MusicBee

            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Tag = "#buttonCancel&controlsPanel";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Tag = "#controlsPanel&controlsPanel";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.sourceTagList, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.checkedSourceTagList, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.uncheckedSourceTagLabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.checkedSourceTagLabel, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Tag = "#CopyTagsToClipboard&controlsPanel";
            // 
            // sourceTagList
            // 
            resources.ApplyResources(this.sourceTagList, "sourceTagList");
            this.sourceTagList.MultiColumn = true;
            this.sourceTagList.Name = "sourceTagList";
            this.sourceTagList.Sorted = true;
            this.sourceTagList.SelectedIndexChanged += new System.EventHandler(this.sourceTagList_SelectedIndexChanged);
            // 
            // checkedSourceTagList
            // 
            resources.ApplyResources(this.checkedSourceTagList, "checkedSourceTagList");
            this.checkedSourceTagList.MultiColumn = true;
            this.checkedSourceTagList.Name = "checkedSourceTagList";
            this.checkedSourceTagList.Sorted = true;
            this.checkedSourceTagList.SelectedIndexChanged += new System.EventHandler(this.checkedSourceTagList_SelectedIndexChanged);
            // 
            // uncheckedSourceTagLabel
            // 
            resources.ApplyResources(this.uncheckedSourceTagLabel, "uncheckedSourceTagLabel");
            this.uncheckedSourceTagLabel.Name = "uncheckedSourceTagLabel";
            // 
            // checkedSourceTagLabel
            // 
            resources.ApplyResources(this.checkedSourceTagLabel, "checkedSourceTagLabel");
            this.checkedSourceTagLabel.Name = "checkedSourceTagLabel";
            // 
            // tagSetComboBox
            // 
            resources.ApplyResources(this.tagSetComboBox, "tagSetComboBox");
            this.tagSetComboBox.FormattingEnabled = true;
            this.tagSetComboBox.Name = "tagSetComboBox";
            this.tagSetComboBox.Tag = "#CopyTagsToClipboard&tableLayoutPanel1";
            this.toolTip1.SetToolTip(this.tagSetComboBox, resources.GetString("tagSetComboBox.ToolTip"));
            this.tagSetComboBox.DropDownClosed += new System.EventHandler(this.tagSetComboBox_DropDownClosed);
            this.tagSetComboBox.TextChanged += new System.EventHandler(this.tagSetComboBox_TextChanged);
            // 
            // checkUncheckAllCheckBox
            // 
            resources.ApplyResources(this.checkUncheckAllCheckBox, "checkUncheckAllCheckBox");
            this.checkUncheckAllCheckBox.Name = "checkUncheckAllCheckBox";
            this.checkUncheckAllCheckBox.Tag = "#checkUncheckAllCheckBoxLabel@pinned-to-parent-x";
            this.checkUncheckAllCheckBox.CheckedChanged += new System.EventHandler(this.checkUncheckAllCheckBox_CheckedChanged);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 15000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.label1.Tag = "@pinned-to-parent-x";
            // 
            // checkUncheckAllCheckBoxLabel
            // 
            resources.ApplyResources(this.checkUncheckAllCheckBoxLabel, "checkUncheckAllCheckBoxLabel");
            this.checkUncheckAllCheckBoxLabel.Name = "checkUncheckAllCheckBoxLabel";
            this.checkUncheckAllCheckBoxLabel.Tag = "#&controlsPanel";
            this.checkUncheckAllCheckBoxLabel.Click += new System.EventHandler(this.checkUncheckAllCheckBoxLabel_Click);
            // 
            // controlsPanel
            // 
            resources.ApplyResources(this.controlsPanel, "controlsPanel");
            this.controlsPanel.Controls.Add(this.buttonCancel);
            this.controlsPanel.Controls.Add(this.buttonOK);
            this.controlsPanel.Controls.Add(this.checkUncheckAllCheckBoxLabel);
            this.controlsPanel.Controls.Add(this.checkUncheckAllCheckBox);
            this.controlsPanel.Controls.Add(this.label1);
            this.controlsPanel.Name = "controlsPanel";
            this.controlsPanel.Tag = "#&CopyTagsToClipboard";
            // 
            // infoLabel
            // 
            resources.ApplyResources(this.infoLabel, "infoLabel");
            this.infoLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Tag = "#CopyTagsToClipboard&tagSetComboBox@pinned-to-parent-y";
            // 
            // CopyTagsToClipboard
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.controlsPanel);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.tagSetComboBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CopyTagsToClipboard";
            this.Tag = "";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CopyTagsToClipboard_FormClosed);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.controlsPanel.ResumeLayout(false);
            this.controlsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox tagSetComboBox;
        private System.Windows.Forms.CheckBox checkUncheckAllCheckBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label checkUncheckAllCheckBoxLabel;
        private Panel controlsPanel;
        private Label infoLabel;
        private ListBox checkedSourceTagList;
        private Label checkedSourceTagLabel;
        private Label uncheckedSourceTagLabel;
        private ListBox sourceTagList;
    }
}