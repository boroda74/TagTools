namespace MusicBeePlugin
{
    partial class SelectTagsPlugin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectTagsPlugin));
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.uncheckedTagsList = new System.Windows.Forms.CheckedListBox();
            this.checkedTagsList = new System.Windows.Forms.CheckedListBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.uncheckedTagsList, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.checkedTagsList, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // uncheckedTagsList
            // 
            resources.ApplyResources(this.uncheckedTagsList, "uncheckedTagsList");
            this.uncheckedTagsList.FormattingEnabled = true;
            this.uncheckedTagsList.MultiColumn = true;
            this.uncheckedTagsList.Name = "uncheckedTagsList";
            this.uncheckedTagsList.Sorted = true;
            this.uncheckedTagsList.SelectedIndexChanged += new System.EventHandler(this.uncheckedTagsList_SelectedIndexChanged);
            // 
            // checkedTagsList
            // 
            resources.ApplyResources(this.checkedTagsList, "checkedTagsList");
            this.checkedTagsList.FormattingEnabled = true;
            this.checkedTagsList.MultiColumn = true;
            this.checkedTagsList.Name = "checkedTagsList";
            this.checkedTagsList.Sorted = true;
            this.checkedTagsList.SelectedIndexChanged += new System.EventHandler(this.checkedTagsList_SelectedIndexChanged);
            // 
            // SelectTagsPlugin
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectTagsPlugin";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckedListBox uncheckedTagsList;
        private System.Windows.Forms.CheckedListBox checkedTagsList;
    }
}