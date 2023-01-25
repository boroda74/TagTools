using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace MusicBeePlugin
{
    public partial class PasteTagsFromClipboardCommand : PluginWindowTemplate
    {
        private List<string> destinationTagNames = new List<string>();

        public PasteTagsFromClipboardCommand()
        {
            InitializeComponent();
        }

        public PasteTagsFromClipboardCommand(Plugin tagToolsPluginParam)
        {
            InitializeComponent();

            TagToolsPlugin = tagToolsPluginParam;

            initializeForm();
        }

        protected new void initializeForm()
        {
            base.initializeForm();

            Plugin.FillList(destinationTagNames, false, false, false);
        }

        private bool pasteTagsFromClipboard()
        {
            string[] files = null;

            if (!Plugin.MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files))
                files = new string[0];


            if (files.Length == 0)
            {
                MessageBox.Show(Plugin.MbForm, Plugin.MsgNoFilesSelected, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }


            if (!Clipboard.ContainsText())
            {
                MessageBox.Show(Plugin.MbForm, Plugin.MsgClipboardDesntContainText, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            string[] fileTags = System.Windows.Clipboard.GetText().Split(new char[] { '\n' }, StringSplitOptions.None);
            //string[] fileTags = NativeWindowsClipboard.GetText().Split(new char[] { '\n' }, StringSplitOptions.None);//***

            bool multiplePasting = false;
            if (fileTags.Length == 1 && files.Length > 1)
            {
                MultiplePastingQuestion question = new MultiplePastingQuestion(TagToolsPlugin, fileTags.Length, files.Length);
                question.ShowDialog();

                if (question.PasteAnyway)
                    multiplePasting = true;
                else
                    return false;

            }
            else if (fileTags.Length != files.Length)
            {
                MessageBox.Show(Plugin.MbForm, Plugin.MsgNumberOfTracksInClipboard + fileTags.Length + Plugin.MsgDoesntCorrespondToNumberOfSelectedTracksC + files.Length + Plugin.MsgMessageEndC, 
                    null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }


            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
                string[] tags = fileTags[multiplePasting ? 0 : i].Split(new char[] { '\t' }, StringSplitOptions.None);

                if (tags.Length != Plugin.SavedSettings.copyTagsTagSets[Plugin.SavedSettings.lastTagSet].tagIds.Length)
                {
                    MessageBox.Show(Plugin.MbForm, Plugin.MsgNumberOfTagsInClipboard + tags.Length + Plugin.MsgDoesntCorrespondToNumberOfCopiedTagsC 
                        + Plugin.SavedSettings.copyTagsTagSets[Plugin.SavedSettings.lastTagSet].tagIds.Length + Plugin.MsgMessageEndC, 
                        null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                for (int j = 0; j < Plugin.SavedSettings.copyTagsTagSets[Plugin.SavedSettings.lastTagSet].tagIds.Length; j++)
                {
                    if (tags[j].Length > 0 && tags[j][tags[j].Length - 1] == '\r')
                        tags[j] = tags[j].Remove(tags[j].Length - 1);
                    
                    string tag = tags[j].Replace("\u0006", "\u0000").Replace("\u0007", "\u000D").Replace("\u0008", "\u000A");
                    Plugin.SetFileTag(file, (Plugin.MetaDataType)Plugin.SavedSettings.copyTagsTagSets[Plugin.SavedSettings.lastTagSet].tagIds[j], tag);
                }
                Plugin.CommitTagsToFile(file);
            }

            Plugin.MbApiInterface.MB_RefreshPanels();

            return true;
        }

        private void PasteTagsFromClipboardPlugin_Shown(object sender, EventArgs e)
        {
            pasteTagsFromClipboard();
            Close();
        }
    }
}
