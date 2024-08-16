using System;
using System.Threading;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    partial class Plugin
    {
        internal static void PasteTagsFromClipboard()
        {
            if (!Clipboard.ContainsText())
            {
                MessageBox.Show(MbForm, MsgClipboardDoesntContainText, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out var files);


            if (files == null || files.Length == 0)
            {
                MessageBox.Show(MbForm, MsgNoTracksSelected, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            var fileTags = System.Windows.Clipboard.GetText().Split(new[] { '\n' }, StringSplitOptions.None);

            if (fileTags.Length < 2) //1st row must be tag names, 2nd row and further are tag values
            {
                MessageBox.Show(MbForm, MsgClipboardDoesntContainTags, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            Thread workingTread = new Thread(PasteTagsFromClipboardParameterizedThreadStart);
            workingTread.Start(new string[][] { files, fileTags });
        }

        //If files is NOT null then paste tags matching by paths automatically (and fileTags must be NOT null also)
        internal static void PasteTagsFromClipboardParameterizedThreadStart(object data)
        {
            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            string[] files = (data as string[][])[0];
            string[] fileTags = (data as string[][])[1];
            PasteTagsFromClipboardInternal(files, fileTags, false);
        }

        internal static void PasteTagsFromClipboardInternal(string[] files, string[] fileTags, bool autoPaste)
        {
            var allTagNames = fileTags[0].Trim('\r');
            var tagNames = allTagNames.Split(new[] { '\t' }, StringSplitOptions.None);
            var tagIds = new int[tagNames.Length];
            for (var k = 0; k < tagNames.Length; k++)
            {
                var tagName = tagNames[k];

                tagIds[k] = (int)GetTagId(tagName);

                if (tagIds[k] == 0)
                {
                    MbForm.Invoke(new Action(() =>
                    {
                        MessageBox.Show(MbForm, MsgUnknownTagNameInClipboard.Replace("%%TAG-NAME%%", tagName), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }));

                    return;
                }
            }


            var matchTagIndex = -1;
            string matchTagName = null;
            for (var k = 0; k < tagIds.Length; k++)
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
                var matchTracksResult = DialogResult.Yes;

                MbForm.Invoke(new Action(() =>
                {
                    matchTracksResult = MessageBox.Show(MbForm, MsgMatchTracksByPathQuestion.Replace("%%MATCH-TAG-NAME%%", matchTagName),
                        string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                }));


                if (matchTracksResult == DialogResult.No)
                    matchTagIndex = -1;
            }


            var multiplePasting = false;
            if (!autoPaste)
            {
                if ((fileTags.Length == 2 && files.Length > 1) || (fileTags.Length - 1 != files.Length && matchTagIndex > -1))
                {
                    var pasteAnywayResult = DialogResult.No;

                    MbForm.Invoke(new Action(() =>
                    {
                        pasteAnywayResult = MessageBox.Show(MbForm, MsgNumberOfTagsInClipboardDoesntCorrespondToNumberOfSelectedTracks
                                                                            .Replace("%%FILE-TAGS-LENGTH%%", (fileTags.Length - 1).ToString())
                                                                            .Replace("%%SELECTED-FILES-COUNT%%", files.Length.ToString()) +
                                                                        MsgDoYouWantToPasteTagsAnyway,
                            string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    }));


                    if (pasteAnywayResult == DialogResult.Yes)
                        multiplePasting = true;
                    else
                        return;
                }
                else if (fileTags.Length - 1 != files.Length)
                {
                    MbForm.Invoke(new Action(() =>
                    {
                        MessageBox.Show(MbForm, MsgNumberOfTagsInClipboardDoesntCorrespondToNumberOfSelectedTracks
                                .Replace("%%FILE-TAGS-LENGTH%%", (fileTags.Length - 1).ToString())
                                .Replace("%%SELECTED-FILES-COUNT%%", files.Length.ToString()),
                            string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }));

                    return;
                }
            }


            var matchedTracks = 0;
            var notMatchedTracks = 0;
            for (var i = 0; i < files.Length; i++)
            {
                if (PluginClosing)
                    return;

                while (MbForm.Disposing)
                    Thread.Sleep(ActionRetryDelay);

                if (MbForm.IsDisposed)
                    MbForm = Control.FromHandle(MbApiInterface.MB_GetWindowHandle()) as Form;

                var file = files[i];


                if ((i & 0x1f) == 0)
                    SetStatusBarTextForFileOperations(PasteTagsSbText, false, i, files.Length, file);


                var trackMatched = false;
                string[] tags = null;
                if (matchTagIndex == -1)
                {
                    tags = fileTags[multiplePasting ? 1 : i + 1].Split(new[] { '\t' }, StringSplitOptions.None);

                    if (tagIds.Length != tags.Length)
                    {
                        MbForm.Invoke(new Action(() =>
                        {
                            MessageBox.Show(MbForm, MsgWrongNumberOfCopiedTags
                                    .Replace("%%CLIPBOARD-TAGS-COUNT%%", tags.Length.ToString())
                                    .Replace("%%CLIPBOARD-LINE%%", (multiplePasting ? 1 : i + 1).ToString()),
                                string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }));

                        return;
                    }
                }
                else
                {
                    var fileMatchTag = GetFileTag(file, (MetaDataType)tagIds[matchTagIndex]);

                    for (var l = 1; l < fileTags.Length; l++)
                    {
                        tags = fileTags[l].Split(new[] { '\t' }, StringSplitOptions.None);

                        if (tagIds.Length != tags.Length)
                        {
                            MbForm.Invoke(new Action(() =>
                            {
                                MessageBox.Show(MbForm, MsgWrongNumberOfCopiedTags
                                        .Replace("%%CLIPBOARD-TAGS-COUNT%%", tags.Length.ToString())
                                        .Replace("%%CLIPBOARD-LINE%%", l.ToString()),
                                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }));

                            return;
                        }

                        var matchTag = tags[matchTagIndex];

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
                    for (var j = 0; j < tagIds.Length; j++)
                    {
                        tags[j] = tags[j].Trim('\r');
                        var tag = tags[j].Replace('\u0006', '\u0000').Replace('\u0007', '\u000D').Replace('\u0008', '\u000A');
                        SetFileTag(file, (MetaDataType)tagIds[j], tag);
                    }

                    CommitTagsToFile(file);
                }
            }


            SetResultingSbText();


            if (matchTagIndex == -1 || autoPaste)
                RefreshPanels(true);

            var continuePasteResult = DialogResult.Yes;
            if (!autoPaste && matchTagIndex > -1)
            {
                MbForm.Invoke(new Action(() =>
                {
                    continuePasteResult = MessageBox.Show(MbForm, MsgTracksSelectedMatchedNotMatched
                            .Replace("%%SELECTED-TRACKS%%", files.Length.ToString())
                            .Replace("%%CLIPBOARD-TRACKS%%", (fileTags.Length - 1).ToString())
                            .Replace("%%MATCHED-TRACKS%%", matchedTracks.ToString())
                            .Replace("%%NOT-MATCHED-TRACKS%%", notMatchedTracks.ToString()),
                        string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }));
            }

            if (!autoPaste && matchTagIndex > -1 && continuePasteResult == DialogResult.Yes)
                PasteTagsFromClipboardInternal(files, fileTags, true);
        }
    }
}
