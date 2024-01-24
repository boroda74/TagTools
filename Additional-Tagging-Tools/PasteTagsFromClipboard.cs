using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class Plugin
    {
        //If files is NOT null then paste tags matching by paths automatically (and fileTags must be NOT null also)
        public static bool PasteTagsFromClipboard(Plugin tagToolsPluginParam, string[] files = null, string[] fileTags = null)
        {
            bool autoPaste = false;
            if (files != null)
                autoPaste = true;

            List<string> destinationTagNames = new List<string>();

            if (!autoPaste)
            {
                if (!MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out files))
                    files = new string[0];
            }


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

            if (!autoPaste)
                fileTags = System.Windows.Clipboard.GetText().Split(new char[] { '\n' }, StringSplitOptions.None);

            if (fileTags.Length < 2) //1st row must be tag names, 2nd row and further are tag values
            {
                MessageBox.Show(MbForm, MsgClipboardDoesntContainTags, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }


            string allTagNames = fileTags[0].Trim('\r');
            string[] tagNames = allTagNames.Split(new char[] { '\t' }, StringSplitOptions.None);
            int[] tagIds = new int[tagNames.Length];
            for (int k = 0; k < tagNames.Length; k++)
            {
                string tagName = tagNames[k];

                tagIds[k] = (int)GetTagId(tagName);

                if (tagIds[k] == 0)
                {
                    MessageBox.Show(MbForm, MsgUnknownTagNameInClipboard.Replace("%%TAG-NAME%%", tagName), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }


            int matchTagIndex = -1;
            string matchTagName = null;
            for (int k = 0; k < tagIds.Length; k++)
            {
                if (tagIds[k] == (int)FilePropertyType.Url)
                {
                    matchTagIndex = k;
                    matchTagName = GetTagName((MetaDataType)FilePropertyType.Url);
                    break;
                }
                else if (tagIds[k] == (int)FilePathWoExtTagId)
                {
                    matchTagIndex = k;
                    matchTagName = FilePathWoExtTagName;
                }
            }


            if (matchTagIndex > -1 && !autoPaste)
            {
                DialogResult matchTracksResult = MessageBox.Show(MbForm, MsgMatchTracksByPathQuestion.Replace("%%MATCH-TAG-NAME%%", matchTagName),
                    string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (matchTracksResult == DialogResult.No)
                    matchTagIndex = -1;
            }


            bool multiplePasting = false;
            if (!autoPaste)
            {
                if ((fileTags.Length == 2 && files.Length > 1) || (fileTags.Length - 1 != files.Length && matchTagIndex > -1))
                {
                    DialogResult pasteAnywayResult = MessageBox.Show(MbForm, MsgNumberOfTagsInClipboardDoesntCorrespondToNumberOfSelectedTracks
                            .Replace("%%FILE-TAGS-LENGTH%%", (fileTags.Length - 1).ToString())
                            .Replace("%%SELECTED-FILES-COUNT%%", files.Length.ToString()) +
                            MsgDoYouWantToPasteTagsAnyway,
                        string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                    if (pasteAnywayResult == DialogResult.Yes)
                        multiplePasting = true;
                    else
                        return false;
                }
                else if (fileTags.Length - 1 != files.Length)
                {
                    MessageBox.Show(MbForm, MsgNumberOfTagsInClipboardDoesntCorrespondToNumberOfSelectedTracks
                            .Replace("%%FILE-TAGS-LENGTH%%", (fileTags.Length - 1).ToString())
                            .Replace("%%SELECTED-FILES-COUNT%%", files.Length.ToString()),
                        string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }


            int matchedTracks = 0;
            int notMatchedTracks = 0;
            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];

                bool trackMatched = false;
                string[] tags = null;
                if (matchTagIndex == -1)
                {
                    tags = fileTags[multiplePasting ? 1 : i + 1].Split(new char[] { '\t' }, StringSplitOptions.None);

                    if (tagIds.Length != tags.Length)
                    {
                        MessageBox.Show(MbForm, MsgWrongNumberOfCopiedTags
                            .Replace("%%CLIPBOARD-TAGS-COUNT%%", tags.Length.ToString())
                            .Replace("%%CLIPBOARD-LINE%%", (multiplePasting ? 1 : i + 1).ToString()),
                            string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                else
                {
                    string fileMatchTag = GetFileTag(file, (MetaDataType)tagIds[matchTagIndex]);

                    for (int l = 1; l < fileTags.Length; l++)
                    {
                        tags = fileTags[l].Split(new char[] { '\t' }, StringSplitOptions.None);

                        if (tagIds.Length != tags.Length)
                        {
                            MessageBox.Show(MbForm, MsgWrongNumberOfCopiedTags
                                .Replace("%%CLIPBOARD-TAGS-COUNT%%", tags.Length.ToString())
                                .Replace("%%CLIPBOARD-LINE%%", l.ToString()), 
                                string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }

                        string matchTag = tags[matchTagIndex];

                        if (matchTag == fileMatchTag)
                        {
                            trackMatched = true;
                            break;
                        }
                    }
                }


                if (matchTagIndex > -1 && !trackMatched) //Track not matched by path among selected tracks
                {
                    notMatchedTracks++;
                    continue;
                }

                matchedTracks++;

                if (matchTagIndex == -1 || autoPaste)
                {
                    for (int j = 0; j < tagIds.Length; j++)
                    {
                        tags[j] = tags[j].Trim('\r');
                        string tag = tags[j].Replace('\u0006', '\u0000').Replace('\u0007', '\u000D').Replace('\u0008', '\u000A');
                        SetFileTag(file, (MetaDataType)tagIds[j], tag);
                    }

                    CommitTagsToFile(file);
                }
            }

            if (matchTagIndex == -1 || autoPaste)
                MbApiInterface.MB_RefreshPanels();

            DialogResult continuePasteResult = DialogResult.Yes;
            if (!autoPaste && matchTagIndex > -1 && (matchedTracks != files.Length || matchedTracks != fileTags.Length - 1))
            {
                continuePasteResult = MessageBox.Show(MbForm, MsgTracksSelectedMatchedNotMatched
                    .Replace("%%SELECTED-TRACKS%%", files.Length.ToString())
                    .Replace("%%CLIPBOARD-TRACKS%%", (fileTags.Length - 1).ToString())
                    .Replace("%%MATCHED-TRACKS%%", matchedTracks.ToString())
                    .Replace("%%NOT-MATCHED-TRACKS%%", notMatchedTracks.ToString()),
                    string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }

            if (matchTagIndex > -1 && !autoPaste && continuePasteResult == DialogResult.Yes)
                return PasteTagsFromClipboard(tagToolsPluginParam, files, fileTags);
            else if (matchTagIndex > -1 && !autoPaste)
                return false;
            

            return true;
        }
    }
}
