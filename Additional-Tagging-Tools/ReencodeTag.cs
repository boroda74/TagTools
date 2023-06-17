using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class ReencodeTagPlugin : PluginWindowTemplate
    {
        private delegate void AddRowToTable(string[] row);
        private delegate void ProcessRowOfTable(int row);
        private AddRowToTable addRowToTable;
        private ProcessRowOfTable processRowOfTable;

        private string[] files = new string[0];
        private List<string[]> tags = new List<string[]>();
        private Plugin.MetaDataType sourceTagId;

        private Encoding defaultEncoding;
        private Encoding originalEncoding;
        private Encoding usedEncoding;

        public ReencodeTagPlugin()
        {
            InitializeComponent();
        }

        public ReencodeTagPlugin(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            Plugin.FillListByTagNames(sourceTagList.Items, false, false, false, false);
            sourceTagList.Text = Plugin.SavedSettings.reencodeTagSourceTagName;

            defaultEncoding = Encoding.Default;
            EncodingInfo[] encodings = Encoding.GetEncodings();

            for (int i = 1; i < encodings.Length; i++)
            {
                initialEncodingsList.Items.Add(encodings[i].Name);
                usedEncodingsList.Items.Add(encodings[i].Name);
            }

            initialEncodingsList.Text = Plugin.SavedSettings.initialEncodingName;
            //usedEncodingsList.Text = Plugin.savedSettings.usedEncodingName;

            if (initialEncodingsList.Text == "")
                initialEncodingsList.Text = defaultEncoding.WebName;
            if (usedEncodingsList.Text == "")
                usedEncodingsList.Text = defaultEncoding.WebName;

            DatagridViewCheckBoxHeaderCell cbHeader = new DatagridViewCheckBoxHeaderCell();
            cbHeader.setState(true);
            cbHeader.OnCheckBoxClicked += new CheckBoxClickedHandler(cbHeader_OnCheckBoxClicked);

            DataGridViewCheckBoxColumn colCB = new DataGridViewCheckBoxColumn();
            colCB.HeaderCell = cbHeader;
            colCB.ThreeState = true;

            colCB.FalseValue = "False";
            colCB.TrueValue = "True";
            colCB.IndeterminateValue = "";
            colCB.Width = 25;
            colCB.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            previewTable.Columns.Insert(0, colCB);

            previewTable.Columns[2].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[3].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            previewTable.Columns[4].HeaderCell.Style.WrapMode = DataGridViewTriState.True;

            previewTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            addRowToTable = previewList_AddRowToTable;
            processRowOfTable = previewList_ProcessRowOfTable;

            (int, int, int, int, int, int, int) value = loadWindowLayout();

            if (value.Item1 != 0)
            {
                previewTable.Columns[1].Width = value.Item1;
                previewTable.Columns[2].Width = value.Item2;
                previewTable.Columns[3].Width = value.Item3;
            }
        }

        private void previewList_AddRowToTable(string[] row)
        {
            previewTable.Rows.Add(row);
        }

        private void previewList_ProcessRowOfTable(int row)
        {
            previewTable.Rows[row].Cells[0].Value = "";
        }

        private string reencode(string source)
        {
            try
            {
                byte[] usedBytes = usedEncoding.GetBytes(source);
                return originalEncoding.GetString(usedBytes);
            }
            catch
            {
                return Plugin.TableCellError;
            }
        }

        private bool prepareBackgroundPreview()
        {
            tags.Clear();
            previewTable.Rows.Clear();
            previewIsGenerated = false;
            ((DatagridViewCheckBoxHeaderCell)previewTable.Columns[0].HeaderCell).setState(true);

            if (backgroundTaskIsWorking())
                return true;

            sourceTagId = Plugin.GetTagId(sourceTagList.Text);
            originalEncoding = Encoding.GetEncoding(initialEncodingsList.Text);
            usedEncoding = Encoding.GetEncoding(usedEncodingsList.Text);


            files = null;
            if (!Plugin.MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files))
                files = new string[0];

            if (files.Length == 0)
            {
                MessageBox.Show(this, Plugin.MsgNoFilesSelected, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool prepareBackgroundTask()
        {
            if (previewTable.Rows.Count == 0)
            {
                return prepareBackgroundPreview();
            }
            else
            {
                string[] tag;

                tags.Clear();

                for (int fileCounter = 0; fileCounter < previewTable.Rows.Count; fileCounter++)
                {
                    tag = new string[3];

                    tag[0] = (string)previewTable.Rows[fileCounter].Cells[0].Value;
                    tag[1] = (string)previewTable.Rows[fileCounter].Cells[1].Value;
                    tag[2] = (string)previewTable.Rows[fileCounter].Cells[4].Value;

                    tags.Add(tag);
                }

                return true;
            }
        }

        private void previewChanges()
        {
            string currentFile;
            string sourceTagValue;
            string newTagValue;

            string isChecked;

            string track;
            string[] row = { "Checked", "File", "Track", "OriginalTag", "NewTag" };
            string[] tag = { "Checked", "file", "newTag" };

            for (int fileCounter = 0; fileCounter < files.Length; fileCounter++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                currentFile = files[fileCounter];

                Plugin.SetStatusbarTextForFileOperations(Plugin.ReencodeTagCommandSbText, true, fileCounter, files.Length, currentFile);

                sourceTagValue = Plugin.GetFileTag(currentFile, sourceTagId);
                newTagValue = reencode(sourceTagValue);

                track = Plugin.GetTrackRepresentation(currentFile);


                if (sourceTagValue == newTagValue)
                    isChecked = "False";
                else
                    isChecked = "True";

                tag = new string[3];

                tag[0] = isChecked;
                tag[1] = currentFile;
                tag[2] = newTagValue;


                row = new string[5];

                row[0] = isChecked;
                row[1] = currentFile;
                row[2] = track;
                row[3] = sourceTagValue;
                row[4] = newTagValue;

                Invoke(addRowToTable, new object[] { row });

                tags.Add(tag);

                previewIsGenerated = true;
            }

            Plugin.SetResultingSbText();
        }

        private void applyChanges()
        {
            string currentFile;
            string newTag;
            string isChecked;

            if (tags.Count == 0)
                previewChanges();

            for (int i = 0; i < tags.Count; i++)
            {
                if (backgroundTaskIsCanceled)
                    return;

                isChecked = tags[i][0];

                if (isChecked == "True")
                {
                    currentFile = tags[i][1];
                    newTag = tags[i][2];

                    tags[i][0] = "";

                    Invoke(processRowOfTable, new object[] { i });

                    Plugin.SetStatusbarTextForFileOperations(Plugin.ReencodeTagCommandSbText, false, i, tags.Count, currentFile);

                    Plugin.SetFileTag(currentFile, sourceTagId, newTag);
                    Plugin.CommitTagsToFile(currentFile);
                }
            }

            Plugin.RefreshPanels(true);

            Plugin.SetResultingSbText();
        }

        private void saveSettings()
        {
            saveWindowLayout(previewTable.Columns[1].Width, previewTable.Columns[2].Width, previewTable.Columns[3].Width);

            Plugin.SavedSettings.reencodeTagSourceTagName = sourceTagList.Text;
            Plugin.SavedSettings.initialEncodingName = initialEncodingsList.Text;
            //Plugin.savedSettings.usedEncodingName = usedEncodingsList.Text;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            saveSettings();
            if (prepareBackgroundTask())
                switchOperation(applyChanges, (Button)sender, buttonOK, buttonPreview, buttonCancel, true, null);
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            clickOnPreviewButton(previewTable, prepareBackgroundPreview, previewChanges, (Button)sender, buttonOK, buttonCancel);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbHeader_OnCheckBoxClicked(bool state)
        {
            foreach (DataGridViewRow row in previewTable.Rows)
            {
                if ((string)row.Cells[0].Value == "")
                    continue;

                if (state)
                    row.Cells[0].Value = "False";
                else
                    row.Cells[0].Value = "True";

                DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(0, row.Index);
                previewList_CellContentClick(null, e);
            }
        }

        private void previewList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string sourceTagValue;
                string newTagValue;

                string isChecked = (string)previewTable.Rows[e.RowIndex].Cells[0].Value;

                if (isChecked == "True")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "False";

                    sourceTagValue = (string)previewTable.Rows[e.RowIndex].Cells[3].Value;

                    previewTable.Rows[e.RowIndex].Cells[4].Value = sourceTagValue;
                }
                else if (isChecked == "False")
                {
                    previewTable.Rows[e.RowIndex].Cells[0].Value = "True";

                    sourceTagValue = (string)previewTable.Rows[e.RowIndex].Cells[3].Value;

                    newTagValue = reencode(sourceTagValue);

                    previewTable.Rows[e.RowIndex].Cells[4].Value = newTagValue;
                }
            }
        }

        private void previewTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((string)previewTable.Rows[e.RowIndex].Cells[3].Value == (string)previewTable.Rows[e.RowIndex].Cells[4].Value)
                previewTable.Rows[e.RowIndex].Cells[4].Style = Plugin.UnchangedStyle;
            else
                previewTable.Rows[e.RowIndex].Cells[4].Style = Plugin.ChangedStyle;
        }

        public override void enableDisablePreviewOptionControls(bool enable)
        {
            if (enable && previewIsGenerated)
                return;

            sourceTagList.Enabled = enable;
            initialEncodingsList.Enabled = enable;
            usedEncodingsList.Enabled = enable;
        }

        public override void enableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonPreview, string.Empty);
        }

        public override void disableQueryingButtons()
        {
            dirtyErrorProvider.SetError(buttonPreview, " ");
            dirtyErrorProvider.SetError(buttonPreview, getBackgroundTasksWarning());
        }

        public override void enableQueryingOrUpdatingButtons()
        {
            buttonOK.Enabled = true;
            buttonPreview.Enabled = true;
        }

        public override void disableQueryingOrUpdatingButtons()
        {
            buttonOK.Enabled = false;
            buttonPreview.Enabled = false;
        }
    }
}
