using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class Plugin
    {
        public static bool PasteTagsFromClipboard(Plugin tagToolsPluginParam)
        {
            List<string> destinationTagNames = new List<string>();
            string[] files = null;

            if (!MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files))
                files = new string[0];


            if (files.Length == 0)
            {
                MessageBox.Show(MbForm, MsgNoTracksSelected, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }


            if (!Clipboard.ContainsText())
            {
                MessageBox.Show(MbForm, MsgClipboardDoesntContainText, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            string[] fileTags = System.Windows.Clipboard.GetText().Split(new char[] { '\n' }, StringSplitOptions.None);

            bool multiplePasting = false;
            if (fileTags.Length == 1 && files.Length > 1)
            {
                DialogResult result = MessageBox.Show(MbForm, MsgNumberOfTagsInClipboardDoesntCorrespondToNumberOfSelectedTracks
                        .Replace("%%FILE-TAGS-LENGTH%%", fileTags.Length.ToString())
                        .Replace("%%SELECTED-FILES-COUNT%%", files.Length.ToString()) +
                        MsgDoYouWantToPasteTagsAnyway,
                    string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                    multiplePasting = true;
                else
                    return false;

            }
            else if (fileTags.Length != files.Length)
            {
                MessageBox.Show(MbForm, MsgNumberOfTagsInClipboardDoesntCorrespondToNumberOfSelectedTracks
                        .Replace("%%FILE-TAGS-LENGTH%%", fileTags.Length.ToString())
                        .Replace("%%SELECTED-FILES-COUNT%%", files.Length.ToString()),
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }


            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
                string[] tags = fileTags[multiplePasting ? 0 : i].Split(new char[] { '\t' }, StringSplitOptions.None);

                if (tags.Length != SavedSettings.copyTagsTagSets[SavedSettings.lastTagSet].tagIds.Length)
                {
                    MessageBox.Show(MbForm, MsgNumberOfTagsInClipboardDoesntCorrespondToNumberOfCopiedTags
                        .Replace("%%CLIPBOARD-TAGS-COUNT%%", tags.Length.ToString())
                        .Replace("%%LAST-USED-TAG-SET-TAGS-COUNT%%", SavedSettings.copyTagsTagSets[SavedSettings.lastTagSet].tagIds.Length.ToString()),
                        string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                for (int j = 0; j < SavedSettings.copyTagsTagSets[SavedSettings.lastTagSet].tagIds.Length; j++)
                {
                    if (tags[j].Length > 0 && tags[j][tags[j].Length - 1] == '\r')
                        tags[j] = tags[j].Remove(tags[j].Length - 1);

                    string tag = tags[j].Replace("\u0006", "\u0000").Replace("\u0007", "\u000D").Replace("\u0008", "\u000A");
                    SetFileTag(file, (MetaDataType)SavedSettings.copyTagsTagSets[SavedSettings.lastTagSet].tagIds[j], tag);
                }
                CommitTagsToFile(file);
            }

            MbApiInterface.MB_RefreshPanels();

            return true;
        }
    }
}
