using System;
using System.Text;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class Plugin
    {
        internal void fillTagNames()
        {
            ReadonlyTagsNames = new string[48];

            ReadonlyTagsNames[0] = GenreCategoryName;
            ReadonlyTagsNames[1] = SynchronisedLyricsName;
            ReadonlyTagsNames[2] = UnsynchronisedLyricsName;
            ReadonlyTagsNames[3] = DisplayedAlbumArtistName;
            ReadonlyTagsNames[4] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual1);
            ReadonlyTagsNames[5] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual2);
            ReadonlyTagsNames[6] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual3);
            ReadonlyTagsNames[7] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual4);
            ReadonlyTagsNames[8] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual5);
            ReadonlyTagsNames[9] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual6);
            ReadonlyTagsNames[10] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual7);
            ReadonlyTagsNames[11] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual8);
            ReadonlyTagsNames[12] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual9);

            ReadonlyTagsNames[13] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual10);
            ReadonlyTagsNames[14] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual11);
            ReadonlyTagsNames[15] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual12);
            ReadonlyTagsNames[16] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual13);
            ReadonlyTagsNames[17] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual14);
            ReadonlyTagsNames[18] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual15);
            ReadonlyTagsNames[19] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual16);

            ReadonlyTagsNames[20] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual17);
            ReadonlyTagsNames[21] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual18);
            ReadonlyTagsNames[22] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual19);
            ReadonlyTagsNames[23] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual20);
            ReadonlyTagsNames[24] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual21);
            ReadonlyTagsNames[25] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual22);
            ReadonlyTagsNames[26] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual23);
            ReadonlyTagsNames[27] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual24);
            ReadonlyTagsNames[28] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual25);
            ReadonlyTagsNames[29] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual26);
            ReadonlyTagsNames[30] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual27);
            ReadonlyTagsNames[31] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual28);
            ReadonlyTagsNames[32] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual29);
            ReadonlyTagsNames[33] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual30);
            ReadonlyTagsNames[34] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual31);
            ReadonlyTagsNames[35] = MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual32);

            ReadonlyTagsNames[36] = MbApiInterface.Setting_GetFieldName(MetaDataType.Genres);
            ReadonlyTagsNames[37] = MbApiInterface.Setting_GetFieldName(MetaDataType.Artists);
            ReadonlyTagsNames[38] = MbApiInterface.Setting_GetFieldName(MetaDataType.ArtistsWithArtistRole);
            ReadonlyTagsNames[39] = MbApiInterface.Setting_GetFieldName(MetaDataType.ArtistsWithPerformerRole);
            ReadonlyTagsNames[40] = MbApiInterface.Setting_GetFieldName(MetaDataType.ArtistsWithGuestRole);
            ReadonlyTagsNames[41] = MbApiInterface.Setting_GetFieldName(MetaDataType.ArtistsWithRemixerRole);

            ReadonlyTagsNames[42] = MbApiInterface.Setting_GetFieldName(MetaDataType.HasLyrics);

            ReadonlyTagsNames[43] = FolderTagName;
            ReadonlyTagsNames[44] = FileNameTagName;
            ReadonlyTagsNames[45] = FilePathTagName;
            ReadonlyTagsNames[46] = FilePathWoExtTagName;
            ReadonlyTagsNames[47] = AlbumUniqueIdName;


            //Tags
            bool wereErrors = false;
            Encoding unicode1 = Encoding.UTF8;
            System.IO.FileStream stream1 = System.IO.File.Open(System.IO.Path.Combine(MbApiInterface.Setting_GetPersistentStoragePath(), "TagTools.TagNamesErrorLog.txt"), System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
            System.IO.StreamWriter file1 = new System.IO.StreamWriter(stream1, unicode1);

            TagNamesIds.Clear();
            TagIdsNames.Clear();

            try
            {
                file1.WriteLine("Adding " + AllTagsPseudoTagName + " / " + AllTagsPseudoTagId);
                TagNamesIds.Add(AllTagsPseudoTagName, AllTagsPseudoTagId);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + AllTagsPseudoTagName + " / " + AllTagsPseudoTagId);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.TrackTitle) + " / " + MetaDataType.TrackTitle);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.TrackTitle), MetaDataType.TrackTitle);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.TrackTitle) + " / " + MetaDataType.TrackTitle);
            }
            try
            {
                file1.WriteLine("Adding " + AlbumTagName + " / " + MetaDataType.Album);
                TagNamesIds.Add(AlbumTagName, MetaDataType.Album);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + AlbumTagName + " / " + MetaDataType.Album);
            }
            try
            {
                file1.WriteLine("Adding " + DisplayedAlbumArtistName + " / " + MetaDataType.AlbumArtist);
                TagNamesIds.Add(DisplayedAlbumArtistName, MetaDataType.AlbumArtist);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + DisplayedAlbumArtistName + " / " + MetaDataType.AlbumArtist);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.AlbumArtist) + " / " + MetaDataType.AlbumArtistRaw);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.AlbumArtist), MetaDataType.AlbumArtistRaw);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.AlbumArtist) + " / " + MetaDataType.AlbumArtistRaw);
            }
            try
            {
                file1.WriteLine("Adding " + DisplayedArtistName + " / " + DisplayedArtistId);
                TagNamesIds.Add(DisplayedArtistName, DisplayedArtistId);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + DisplayedArtistName + " / " + DisplayedArtistId);
            }
            try
            {
                file1.WriteLine("Adding " + ArtistArtistsName + " / " + ArtistArtistsId);
                TagNamesIds.Add(ArtistArtistsName, ArtistArtistsId);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + ArtistArtistsName + " / " + ArtistArtistsId);
            }
            try
            {
                file1.WriteLine("Adding " + ArtworkName + " / " + MetaDataType.Artwork);
                TagNamesIds.Add(ArtworkName, MetaDataType.Artwork);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + ArtworkName + " / " + MetaDataType.Artwork);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.BeatsPerMin) + " / " + MetaDataType.BeatsPerMin);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.BeatsPerMin), MetaDataType.BeatsPerMin);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.BeatsPerMin) + " / " + MetaDataType.BeatsPerMin);
            }
            try
            {
                file1.WriteLine("Adding " + DisplayedComposerName + " / " + DisplayedComposerId);
                TagNamesIds.Add(DisplayedComposerName, DisplayedComposerId);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + DisplayedComposerName + " / " + DisplayedComposerId);
            }
            try
            {
                file1.WriteLine("Adding " + ComposerComposersName + " / " + ComposerComposersId);
                TagNamesIds.Add(ComposerComposersName, ComposerComposersId);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + ComposerComposersName + " / " + ComposerComposersId);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Comment) + " / " + MetaDataType.Comment);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Comment), MetaDataType.Comment);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Comment) + " / " + MetaDataType.Comment);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Conductor) + " / " + MetaDataType.Conductor);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Conductor), MetaDataType.Conductor);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Conductor) + " / " + MetaDataType.Conductor);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.DiscNo) + " / " + MetaDataType.DiscNo);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.DiscNo), MetaDataType.DiscNo);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.DiscNo) + " / " + MetaDataType.DiscNo);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.DiscCount) + " / " + MetaDataType.DiscCount);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.DiscCount), MetaDataType.DiscCount);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.DiscCount) + " / " + MetaDataType.DiscCount);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Encoder) + " / " + MetaDataType.Encoder);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Encoder), MetaDataType.Encoder);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Encoder) + " / " + MetaDataType.Encoder);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Genre) + " / " + MetaDataType.Genre);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Genre), MetaDataType.Genre);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Genre) + " / " + MetaDataType.Genre);
            }
            try
            {
                file1.WriteLine("Adding " + GenreCategoryName + " / " + MetaDataType.GenreCategory);
                TagNamesIds.Add(GenreCategoryName, MetaDataType.GenreCategory);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + GenreCategoryName + " / " + MetaDataType.GenreCategory);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Grouping) + " / " + MetaDataType.Grouping);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Grouping), MetaDataType.Grouping);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Grouping) + " / " + MetaDataType.Grouping);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Keywords) + " / " + MetaDataType.Keywords);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Keywords), MetaDataType.Keywords);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Keywords) + " / " + MetaDataType.Keywords);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Lyricist) + " / " + MetaDataType.Lyricist);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Lyricist), MetaDataType.Lyricist);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Lyricist) + " / " + MetaDataType.Lyricist);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Mood) + " / " + MetaDataType.Mood);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Mood), MetaDataType.Mood);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Mood) + " / " + MetaDataType.Mood);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Occasion) + " / " + MetaDataType.Occasion);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Occasion), MetaDataType.Occasion);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Occasion) + " / " + MetaDataType.Occasion);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Origin) + " / " + MetaDataType.Origin);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Origin), MetaDataType.Origin);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Origin) + " / " + MetaDataType.Origin);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Publisher) + " / " + MetaDataType.Publisher);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Publisher), MetaDataType.Publisher);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Publisher) + " / " + MetaDataType.Publisher);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Quality) + " / " + MetaDataType.Quality);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Quality), MetaDataType.Quality);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Quality) + " / " + MetaDataType.Quality);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Rating) + " / " + MetaDataType.Rating);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Rating), MetaDataType.Rating);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Rating) + " / " + MetaDataType.Rating);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.RatingAlbum) + " / " + MetaDataType.RatingAlbum);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.RatingAlbum), MetaDataType.RatingAlbum);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.RatingAlbum) + " / " + MetaDataType.RatingAlbum);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.RatingLove) + " / " + MetaDataType.RatingLove);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.RatingLove), MetaDataType.RatingLove);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.RatingLove) + " / " + MetaDataType.RatingLove);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Tempo) + " / " + MetaDataType.Tempo);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Tempo), MetaDataType.Tempo);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Tempo) + " / " + MetaDataType.Tempo);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.TrackNo) + " / " + MetaDataType.TrackNo);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.TrackNo), MetaDataType.TrackNo);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.TrackNo) + " / " + MetaDataType.TrackNo);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.TrackCount) + " / " + MetaDataType.TrackCount);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.TrackCount), MetaDataType.TrackCount);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.TrackCount) + " / " + MetaDataType.TrackCount);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Year) + " / " + MetaDataType.Year);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Year), MetaDataType.Year);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Year) + " / " + MetaDataType.Year);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.HasLyrics) + " / " + MetaDataType.HasLyrics);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.HasLyrics), MetaDataType.HasLyrics);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.HasLyrics) + " / " + MetaDataType.HasLyrics);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual1) + " / " + MetaDataType.Virtual1);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual1), MetaDataType.Virtual1);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual1) + " / " + MetaDataType.Virtual1);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual2) + " / " + MetaDataType.Virtual2);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual2), MetaDataType.Virtual2);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual2) + " / " + MetaDataType.Virtual2);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual3) + " / " + MetaDataType.Virtual3);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual3), MetaDataType.Virtual3);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual3) + " / " + MetaDataType.Virtual3);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual4) + " / " + MetaDataType.Virtual4);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual4), MetaDataType.Virtual4);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual4) + " / " + MetaDataType.Virtual4);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual5) + " / " + MetaDataType.Virtual5);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual5), MetaDataType.Virtual5);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual5) + " / " + MetaDataType.Virtual5);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual6) + " / " + MetaDataType.Virtual6);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual6), MetaDataType.Virtual6);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual6) + " / " + MetaDataType.Virtual6);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual7) + " / " + MetaDataType.Virtual7);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual7), MetaDataType.Virtual7);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual7) + " / " + MetaDataType.Virtual7);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual8) + " / " + MetaDataType.Virtual8);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual8), MetaDataType.Virtual8);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual8) + " / " + MetaDataType.Virtual8);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual9) + " / " + MetaDataType.Virtual9);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual9), MetaDataType.Virtual9);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual9) + " / " + MetaDataType.Virtual9);
            }

            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual10) + " / " + MetaDataType.Virtual10);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual10), MetaDataType.Virtual10);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual10) + " / " + MetaDataType.Virtual10);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual11) + " / " + MetaDataType.Virtual11);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual11), MetaDataType.Virtual11);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual11) + " / " + MetaDataType.Virtual11);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual12) + " / " + MetaDataType.Virtual12);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual12), MetaDataType.Virtual12);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual12) + " / " + MetaDataType.Virtual12);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual13) + " / " + MetaDataType.Virtual13);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual13), MetaDataType.Virtual13);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual13) + " / " + MetaDataType.Virtual13);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual14) + " / " + MetaDataType.Virtual14);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual14), MetaDataType.Virtual14);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual14) + " / " + MetaDataType.Virtual14);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual15) + " / " + MetaDataType.Virtual15);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual15), MetaDataType.Virtual15);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual15) + " / " + MetaDataType.Virtual15);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual16) + " / " + MetaDataType.Virtual16);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual16), MetaDataType.Virtual16);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual16) + " / " + MetaDataType.Virtual16);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual17) + " / " + MetaDataType.Virtual17);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual17), MetaDataType.Virtual17);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual17) + " / " + MetaDataType.Virtual17);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual18) + " / " + MetaDataType.Virtual18);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual18), MetaDataType.Virtual18);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual18) + " / " + MetaDataType.Virtual18);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual19) + " / " + MetaDataType.Virtual19);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual19), MetaDataType.Virtual19);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual19) + " / " + MetaDataType.Virtual19);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual20) + " / " + MetaDataType.Virtual20);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual20), MetaDataType.Virtual20);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual20) + " / " + MetaDataType.Virtual20);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual21) + " / " + MetaDataType.Virtual21);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual21), MetaDataType.Virtual21);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual21) + " / " + MetaDataType.Virtual21);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual22) + " / " + MetaDataType.Virtual22);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual22), MetaDataType.Virtual22);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual22) + " / " + MetaDataType.Virtual22);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual23) + " / " + MetaDataType.Virtual23);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual23), MetaDataType.Virtual23);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual23) + " / " + MetaDataType.Virtual23);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual24) + " / " + MetaDataType.Virtual24);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual24), MetaDataType.Virtual24);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual24) + " / " + MetaDataType.Virtual24);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual25) + " / " + MetaDataType.Virtual25);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual25), MetaDataType.Virtual25);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual25) + " / " + MetaDataType.Virtual25);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual26) + " / " + MetaDataType.Virtual26);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual26), MetaDataType.Virtual26);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual26) + " / " + MetaDataType.Virtual26);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual27) + " / " + MetaDataType.Virtual27);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual27), MetaDataType.Virtual27);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual27) + " / " + MetaDataType.Virtual27);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual28) + " / " + MetaDataType.Virtual28);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual28), MetaDataType.Virtual28);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual28) + " / " + MetaDataType.Virtual28);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual29) + " / " + MetaDataType.Virtual29);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual29), MetaDataType.Virtual29);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual29) + " / " + MetaDataType.Virtual29);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual30) + " / " + MetaDataType.Virtual30);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual30), MetaDataType.Virtual30);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual30) + " / " + MetaDataType.Virtual30);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual31) + " / " + MetaDataType.Virtual31);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual31), MetaDataType.Virtual31);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual31) + " / " + MetaDataType.Virtual31);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual32) + " / " + MetaDataType.Virtual32);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual32), MetaDataType.Virtual32);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual32) + " / " + MetaDataType.Virtual32);
            }


            try
            {
                file1.WriteLine("Adding " + LyricsName + " / " + LyricsId);
                TagNamesIds.Add(LyricsName, LyricsId);
            }
            catch (ArgumentException)
            {
                file1.WriteLine("Cant add " + LyricsName + " / " + LyricsId);

                try
                {
                    file1.WriteLine("Retry: adding " + LyricsName + LyricsNamePostfix + " / " + LyricsId);
                    TagNamesIds.Add(LyricsName + LyricsNamePostfix, LyricsId);
                }
                catch (ArgumentException)
                {
                    wereErrors = true;
                    file1.WriteLine("Retry: cant add " + LyricsName + LyricsNamePostfix + " / " + LyricsId);
                }
            }
            try
            {
                file1.WriteLine("Adding " + SynchronisedLyricsName + " / " + SynchronisedLyricsId);
                TagNamesIds.Add(SynchronisedLyricsName, SynchronisedLyricsId);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + SynchronisedLyricsName + " / " + SynchronisedLyricsId);
            }
            try
            {
                file1.WriteLine("Adding " + UnsynchronisedLyricsName + " / " + UnsynchronisedLyricsId);
                TagNamesIds.Add(UnsynchronisedLyricsName, UnsynchronisedLyricsId);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + UnsynchronisedLyricsName + " / " + UnsynchronisedLyricsId);
            }
            try
            {
                file1.WriteLine("Adding " + NullTagName + " / " + NullTagId);
                TagNamesIds.Add(NullTagName, NullTagId);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + NullTagName + " / " + NullTagId);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom1) + " / " + MetaDataType.Custom1);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom1), MetaDataType.Custom1);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom1) + " / " + MetaDataType.Custom1);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom2) + " / " + MetaDataType.Custom2);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom2), MetaDataType.Custom2);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom2) + " / " + MetaDataType.Custom2);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom3) + " / " + MetaDataType.Custom3);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom3), MetaDataType.Custom3);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom3) + " / " + MetaDataType.Custom3);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom4) + " / " + MetaDataType.Custom4);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom4), MetaDataType.Custom4);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom4) + " / " + MetaDataType.Custom4);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom5) + " / " + MetaDataType.Custom5);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom5), MetaDataType.Custom5);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom5) + " / " + MetaDataType.Custom5);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom6) + " / " + MetaDataType.Custom6);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom6), MetaDataType.Custom6);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom6) + " / " + MetaDataType.Custom6);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom7) + " / " + MetaDataType.Custom7);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom7), MetaDataType.Custom7);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom7) + " / " + MetaDataType.Custom7);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom8) + " / " + MetaDataType.Custom8);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom8), MetaDataType.Custom8);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom8) + " / " + MetaDataType.Custom8);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom9) + " / " + MetaDataType.Custom9);
                TagNamesIds.Add(Custom9TagName, MetaDataType.Custom9);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom9) + " / " + MetaDataType.Custom9);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom10) + " / " + MetaDataType.Custom10);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom10), MetaDataType.Custom10);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom10) + " / " + MetaDataType.Custom10);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom11) + " / " + MetaDataType.Custom11);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom11), MetaDataType.Custom11);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom11) + " / " + MetaDataType.Custom11);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom12) + " / " + MetaDataType.Custom12);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom12), MetaDataType.Custom12);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom12) + " / " + MetaDataType.Custom12);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom13) + " / " + MetaDataType.Custom13);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom13), MetaDataType.Custom13);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom13) + " / " + MetaDataType.Custom13);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom14) + " / " + MetaDataType.Custom14);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom14), MetaDataType.Custom14);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom14) + " / " + MetaDataType.Custom14);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom15) + " / " + MetaDataType.Custom15);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom15), MetaDataType.Custom15);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom15) + " / " + MetaDataType.Custom15);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom16) + " / " + MetaDataType.Custom16);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Custom16), MetaDataType.Custom16);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Custom16) + " / " + MetaDataType.Custom16);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Genres) + " / " + MetaDataType.Genres);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Genres), MetaDataType.Genres);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Genres) + " / " + MetaDataType.Genres);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Artists) + " / " + MetaDataType.Artists);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Artists), MetaDataType.Artists);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Artists) + " / " + MetaDataType.Artists);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.ArtistsWithArtistRole) + " / " + MetaDataType.ArtistsWithArtistRole);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.ArtistsWithArtistRole), MetaDataType.ArtistsWithArtistRole);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.ArtistsWithArtistRole) + " / " + MetaDataType.ArtistsWithArtistRole);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.ArtistsWithPerformerRole) + " / " + MetaDataType.ArtistsWithPerformerRole);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.ArtistsWithPerformerRole), MetaDataType.ArtistsWithPerformerRole);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.ArtistsWithPerformerRole) + " / " + MetaDataType.ArtistsWithPerformerRole);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.ArtistsWithGuestRole) + " / " + MetaDataType.ArtistsWithGuestRole);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.ArtistsWithGuestRole), MetaDataType.ArtistsWithGuestRole);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.ArtistsWithGuestRole) + " / " + MetaDataType.ArtistsWithGuestRole);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.ArtistsWithRemixerRole) + " / " + MetaDataType.ArtistsWithRemixerRole);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.ArtistsWithRemixerRole), MetaDataType.ArtistsWithRemixerRole);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.ArtistsWithRemixerRole) + " / " + MetaDataType.ArtistsWithRemixerRole);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.LastPlayed) + " / " + FilePropertyType.LastPlayed);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.LastPlayed), (MetaDataType)FilePropertyType.LastPlayed);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.LastPlayed) + " / " + FilePropertyType.LastPlayed);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.PlayCount) + " / " + FilePropertyType.PlayCount);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.PlayCount), (MetaDataType)FilePropertyType.PlayCount);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.PlayCount) + " / " + FilePropertyType.PlayCount);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.SkipCount) + " / " + FilePropertyType.SkipCount);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.SkipCount), (MetaDataType)FilePropertyType.SkipCount);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.SkipCount) + " / " + FilePropertyType.SkipCount);
            }

            //Tags below are added manually by me
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.SortTitle) + " / " + MetaDataType.SortTitle);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.SortTitle), MetaDataType.SortTitle);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.SortTitle) + " / " + MetaDataType.SortTitle);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.SortAlbum) + " / " + MetaDataType.SortAlbum);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.SortAlbum), MetaDataType.SortAlbum);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.SortAlbum) + " / " + MetaDataType.SortAlbum);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.SortAlbumArtist) + " / " + MetaDataType.SortAlbumArtist);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.SortAlbumArtist), MetaDataType.SortAlbumArtist);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.SortAlbumArtist) + " / " + MetaDataType.SortAlbumArtist);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.SortArtist) + " / " + MetaDataType.SortArtist);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.SortArtist), MetaDataType.SortArtist);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.SortArtist) + " / " + MetaDataType.SortArtist);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.SortComposer) + " / " + MetaDataType.SortComposer);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.SortComposer), MetaDataType.SortComposer);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.SortComposer) + " / " + MetaDataType.SortComposer);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Work) + " / " + MetaDataType.Work);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Work), MetaDataType.Work);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Work) + " / " + MetaDataType.Work);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.MovementName) + " / " + MetaDataType.MovementName);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.MovementName), MetaDataType.MovementName);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.MovementName) + " / " + MetaDataType.MovementName);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.MovementNo) + " / " + MetaDataType.MovementNo);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.MovementNo), MetaDataType.MovementNo);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.MovementNo) + " / " + MetaDataType.MovementNo);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.MovementCount) + " / " + MetaDataType.MovementCount);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.MovementCount), MetaDataType.MovementCount);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.MovementCount) + " / " + MetaDataType.MovementCount);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.ShowMovement) + " / " + MetaDataType.ShowMovement);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.ShowMovement), MetaDataType.ShowMovement);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.ShowMovement) + " / " + MetaDataType.ShowMovement);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.Language) + " / " + MetaDataType.Language);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.Language), MetaDataType.Language);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.Language) + " / " + MetaDataType.Language);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.OriginalArtist) + " / " + MetaDataType.OriginalArtist);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.OriginalArtist), MetaDataType.OriginalArtist);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.OriginalArtist) + " / " + MetaDataType.OriginalArtist);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.OriginalYear) + " / " + MetaDataType.OriginalYear);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.OriginalYear), MetaDataType.OriginalYear);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.OriginalYear) + " / " + MetaDataType.OriginalYear);
            }
            try
            {
                file1.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName(MetaDataType.OriginalTitle) + " / " + MetaDataType.OriginalTitle);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName(MetaDataType.OriginalTitle), MetaDataType.OriginalTitle);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName(MetaDataType.OriginalTitle) + " / " + MetaDataType.OriginalTitle);
            }
            try
            {
                file1.WriteLine("Adding " + FolderTagName + " / " + FolderTagId);
                TagNamesIds.Add(FolderTagName, FolderTagId);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + FolderTagName + " / " + FolderTagId);
            }
            try
            {
                file1.WriteLine("Adding " + FileNameTagName + " / " + FileNameTagId);
                TagNamesIds.Add(FileNameTagName, FileNameTagId);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + FileNameTagName + " / " + FileNameTagId);
            }
            try
            {
                file1.WriteLine("Adding " + FilePathTagName + " / " + FilePathTagId);
                TagNamesIds.Add(FilePathTagName, FilePathTagId);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + FilePathTagName + " / " + FilePathTagId);
            }
            try
            {
                file1.WriteLine("Adding " + FilePathWoExtTagName + " / " + FilePathWoExtTagId);
                TagNamesIds.Add(FilePathWoExtTagName, FilePathWoExtTagId);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + FilePathWoExtTagName + " / " + FilePathWoExtTagId);
            }
            try
            {
                file1.WriteLine("Adding " + AlbumUniqueIdName + " / " + MetaDataType.AlbumUniqueId);
                TagNamesIds.Add(AlbumUniqueIdName, MetaDataType.AlbumUniqueId);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + AlbumUniqueIdName + " / " + MetaDataType.AlbumUniqueId);
            }
            try
            {
                file1.WriteLine("Adding " + DateCreatedTagName + " / " + DateCreatedTagId);
                TagNamesIds.Add(DateCreatedTagName, DateCreatedTagId);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file1.WriteLine("Cant add " + DateCreatedTagName + " / " + DateCreatedTagId);
            }

            if (wereErrors)
            {
                MessageBox.Show(MbForm, "Some tag names are duplicated. See \"" +
                    System.IO.Path.Combine(MbApiInterface.Setting_GetPersistentStoragePath(), "TagTools.TagNamesErrorLog.txt")
                    + "\" file for details. MusicBeePlugin is not properly initialized.",
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            file1.Close();


            TagIdsNames.Add(MetaDataType.TrackTitle, MbApiInterface.Setting_GetFieldName(MetaDataType.TrackTitle));
            TagIdsNames.Add(MetaDataType.Album, AlbumTagName);
            TagIdsNames.Add(MetaDataType.AlbumArtist, DisplayedAlbumArtistName);
            TagIdsNames.Add(MetaDataType.AlbumArtistRaw, MbApiInterface.Setting_GetFieldName(MetaDataType.AlbumArtist));
            TagIdsNames.Add(DisplayedArtistId, DisplayedArtistName);
            TagIdsNames.Add(ArtistArtistsId, ArtistArtistsName);
            TagIdsNames.Add(MetaDataType.Artwork, ArtworkName);
            TagIdsNames.Add(MetaDataType.BeatsPerMin, MbApiInterface.Setting_GetFieldName(MetaDataType.BeatsPerMin));
            TagIdsNames.Add(DisplayedComposerId, DisplayedComposerName);
            TagIdsNames.Add(ComposerComposersId, ComposerComposersName);
            TagIdsNames.Add(MetaDataType.Comment, MbApiInterface.Setting_GetFieldName(MetaDataType.Comment));
            TagIdsNames.Add(MetaDataType.Conductor, MbApiInterface.Setting_GetFieldName(MetaDataType.Conductor));
            TagIdsNames.Add(MetaDataType.DiscNo, MbApiInterface.Setting_GetFieldName(MetaDataType.DiscNo));
            TagIdsNames.Add(MetaDataType.DiscCount, MbApiInterface.Setting_GetFieldName(MetaDataType.DiscCount));
            TagIdsNames.Add(MetaDataType.Encoder, MbApiInterface.Setting_GetFieldName(MetaDataType.Encoder));
            TagIdsNames.Add(MetaDataType.Genre, MbApiInterface.Setting_GetFieldName(MetaDataType.Genre));
            TagIdsNames.Add(MetaDataType.GenreCategory, GenreCategoryName);
            TagIdsNames.Add(MetaDataType.Grouping, MbApiInterface.Setting_GetFieldName(MetaDataType.Grouping));
            TagIdsNames.Add(MetaDataType.Keywords, MbApiInterface.Setting_GetFieldName(MetaDataType.Keywords));
            TagIdsNames.Add(MetaDataType.Lyricist, MbApiInterface.Setting_GetFieldName(MetaDataType.Lyricist));
            TagIdsNames.Add(MetaDataType.Mood, MbApiInterface.Setting_GetFieldName(MetaDataType.Mood));
            TagIdsNames.Add(MetaDataType.Occasion, MbApiInterface.Setting_GetFieldName(MetaDataType.Occasion));
            TagIdsNames.Add(MetaDataType.Origin, MbApiInterface.Setting_GetFieldName(MetaDataType.Origin));
            TagIdsNames.Add(MetaDataType.Publisher, MbApiInterface.Setting_GetFieldName(MetaDataType.Publisher));
            TagIdsNames.Add(MetaDataType.Quality, MbApiInterface.Setting_GetFieldName(MetaDataType.Quality));
            TagIdsNames.Add(MetaDataType.Rating, MbApiInterface.Setting_GetFieldName(MetaDataType.Rating));
            TagIdsNames.Add(MetaDataType.RatingAlbum, MbApiInterface.Setting_GetFieldName(MetaDataType.RatingAlbum));
            TagIdsNames.Add(MetaDataType.RatingLove, MbApiInterface.Setting_GetFieldName(MetaDataType.RatingLove));
            TagIdsNames.Add(MetaDataType.Tempo, MbApiInterface.Setting_GetFieldName(MetaDataType.Tempo));
            TagIdsNames.Add(MetaDataType.TrackNo, MbApiInterface.Setting_GetFieldName(MetaDataType.TrackNo));
            TagIdsNames.Add(MetaDataType.TrackCount, MbApiInterface.Setting_GetFieldName(MetaDataType.TrackCount));
            TagIdsNames.Add(MetaDataType.Year, MbApiInterface.Setting_GetFieldName(MetaDataType.Year));
            TagIdsNames.Add(MetaDataType.HasLyrics, MbApiInterface.Setting_GetFieldName(MetaDataType.HasLyrics));
            TagIdsNames.Add(MetaDataType.Virtual1, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual1));
            TagIdsNames.Add(MetaDataType.Virtual2, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual2));
            TagIdsNames.Add(MetaDataType.Virtual3, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual3));
            TagIdsNames.Add(MetaDataType.Virtual4, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual4));
            TagIdsNames.Add(MetaDataType.Virtual5, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual5));
            TagIdsNames.Add(MetaDataType.Virtual6, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual6));
            TagIdsNames.Add(MetaDataType.Virtual7, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual7));
            TagIdsNames.Add(MetaDataType.Virtual8, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual8));
            TagIdsNames.Add(MetaDataType.Virtual9, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual9));
            TagIdsNames.Add(MetaDataType.Virtual10, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual10));
            TagIdsNames.Add(MetaDataType.Virtual11, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual11));
            TagIdsNames.Add(MetaDataType.Virtual12, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual12));
            TagIdsNames.Add(MetaDataType.Virtual13, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual13));
            TagIdsNames.Add(MetaDataType.Virtual14, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual14));
            TagIdsNames.Add(MetaDataType.Virtual15, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual15));
            TagIdsNames.Add(MetaDataType.Virtual16, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual16));
            TagIdsNames.Add(MetaDataType.Virtual17, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual17));
            TagIdsNames.Add(MetaDataType.Virtual18, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual18));
            TagIdsNames.Add(MetaDataType.Virtual19, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual19));
            TagIdsNames.Add(MetaDataType.Virtual20, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual20));
            TagIdsNames.Add(MetaDataType.Virtual21, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual21));
            TagIdsNames.Add(MetaDataType.Virtual22, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual22));
            TagIdsNames.Add(MetaDataType.Virtual23, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual23));
            TagIdsNames.Add(MetaDataType.Virtual24, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual24));
            TagIdsNames.Add(MetaDataType.Virtual25, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual25));
            TagIdsNames.Add(MetaDataType.Virtual26, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual26));
            TagIdsNames.Add(MetaDataType.Virtual27, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual27));
            TagIdsNames.Add(MetaDataType.Virtual28, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual28));
            TagIdsNames.Add(MetaDataType.Virtual29, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual29));
            TagIdsNames.Add(MetaDataType.Virtual30, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual30));
            TagIdsNames.Add(MetaDataType.Virtual31, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual31));
            TagIdsNames.Add(MetaDataType.Virtual32, MbApiInterface.Setting_GetFieldName(MetaDataType.Virtual32));

            TagIdsNames.Add(LyricsId, LyricsName);
            TagIdsNames.Add(SynchronisedLyricsId, SynchronisedLyricsName);
            TagIdsNames.Add(UnsynchronisedLyricsId, UnsynchronisedLyricsName);

            TagIdsNames.Add(AllTagsPseudoTagId, AllTagsPseudoTagName);
            TagIdsNames.Add(NullTagId, NullTagName);

            TagIdsNames.Add(MetaDataType.Custom1, MbApiInterface.Setting_GetFieldName(MetaDataType.Custom1));
            TagIdsNames.Add(MetaDataType.Custom2, MbApiInterface.Setting_GetFieldName(MetaDataType.Custom2));
            TagIdsNames.Add(MetaDataType.Custom3, MbApiInterface.Setting_GetFieldName(MetaDataType.Custom3));
            TagIdsNames.Add(MetaDataType.Custom4, MbApiInterface.Setting_GetFieldName(MetaDataType.Custom4));
            TagIdsNames.Add(MetaDataType.Custom5, MbApiInterface.Setting_GetFieldName(MetaDataType.Custom5));
            TagIdsNames.Add(MetaDataType.Custom6, MbApiInterface.Setting_GetFieldName(MetaDataType.Custom6));
            TagIdsNames.Add(MetaDataType.Custom7, MbApiInterface.Setting_GetFieldName(MetaDataType.Custom7));
            TagIdsNames.Add(MetaDataType.Custom8, MbApiInterface.Setting_GetFieldName(MetaDataType.Custom8));
            TagIdsNames.Add(MetaDataType.Custom9, Custom9TagName);
            TagIdsNames.Add(MetaDataType.Custom10, MbApiInterface.Setting_GetFieldName(MetaDataType.Custom10));
            TagIdsNames.Add(MetaDataType.Custom11, MbApiInterface.Setting_GetFieldName(MetaDataType.Custom11));
            TagIdsNames.Add(MetaDataType.Custom12, MbApiInterface.Setting_GetFieldName(MetaDataType.Custom12));
            TagIdsNames.Add(MetaDataType.Custom13, MbApiInterface.Setting_GetFieldName(MetaDataType.Custom13));
            TagIdsNames.Add(MetaDataType.Custom14, MbApiInterface.Setting_GetFieldName(MetaDataType.Custom14));
            TagIdsNames.Add(MetaDataType.Custom15, MbApiInterface.Setting_GetFieldName(MetaDataType.Custom15));
            TagIdsNames.Add(MetaDataType.Custom16, MbApiInterface.Setting_GetFieldName(MetaDataType.Custom16));


            TagIdsNames.Add(MetaDataType.Genres, MbApiInterface.Setting_GetFieldName(MetaDataType.Genres));
            TagIdsNames.Add(MetaDataType.Artists, MbApiInterface.Setting_GetFieldName(MetaDataType.Artists));
            TagIdsNames.Add(MetaDataType.ArtistsWithArtistRole, MbApiInterface.Setting_GetFieldName(MetaDataType.ArtistsWithArtistRole));
            TagIdsNames.Add(MetaDataType.ArtistsWithPerformerRole, MbApiInterface.Setting_GetFieldName(MetaDataType.ArtistsWithPerformerRole));
            TagIdsNames.Add(MetaDataType.ArtistsWithGuestRole, MbApiInterface.Setting_GetFieldName(MetaDataType.ArtistsWithGuestRole));
            TagIdsNames.Add(MetaDataType.ArtistsWithRemixerRole, MbApiInterface.Setting_GetFieldName(MetaDataType.ArtistsWithRemixerRole));

            TagIdsNames.Add((MetaDataType)FilePropertyType.LastPlayed, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.LastPlayed));
            TagIdsNames.Add((MetaDataType)FilePropertyType.PlayCount, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.PlayCount));
            TagIdsNames.Add((MetaDataType)FilePropertyType.SkipCount, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.SkipCount));

            //Tags below are added manually by me
            TagIdsNames.Add(MetaDataType.SortTitle, MbApiInterface.Setting_GetFieldName(MetaDataType.SortTitle));
            TagIdsNames.Add(MetaDataType.SortAlbum, MbApiInterface.Setting_GetFieldName(MetaDataType.SortAlbum));
            TagIdsNames.Add(MetaDataType.SortAlbumArtist, MbApiInterface.Setting_GetFieldName(MetaDataType.SortAlbumArtist));
            TagIdsNames.Add(MetaDataType.SortArtist, MbApiInterface.Setting_GetFieldName(MetaDataType.SortArtist));
            TagIdsNames.Add(MetaDataType.SortComposer, MbApiInterface.Setting_GetFieldName(MetaDataType.SortComposer));
            TagIdsNames.Add(MetaDataType.Work, MbApiInterface.Setting_GetFieldName(MetaDataType.Work));
            TagIdsNames.Add(MetaDataType.MovementName, MbApiInterface.Setting_GetFieldName(MetaDataType.MovementName));
            TagIdsNames.Add(MetaDataType.MovementNo, MbApiInterface.Setting_GetFieldName(MetaDataType.MovementNo));
            TagIdsNames.Add(MetaDataType.MovementCount, MbApiInterface.Setting_GetFieldName(MetaDataType.MovementCount));
            TagIdsNames.Add(MetaDataType.ShowMovement, MbApiInterface.Setting_GetFieldName(MetaDataType.ShowMovement));
            TagIdsNames.Add(MetaDataType.Language, MbApiInterface.Setting_GetFieldName(MetaDataType.Language));
            TagIdsNames.Add(MetaDataType.OriginalArtist, MbApiInterface.Setting_GetFieldName(MetaDataType.OriginalArtist));
            TagIdsNames.Add(MetaDataType.OriginalYear, MbApiInterface.Setting_GetFieldName(MetaDataType.OriginalYear));
            TagIdsNames.Add(MetaDataType.OriginalTitle, MbApiInterface.Setting_GetFieldName(MetaDataType.OriginalTitle));

            TagIdsNames.Add(FolderTagId, FolderTagName);
            TagIdsNames.Add(FileNameTagId, FileNameTagName);
            TagIdsNames.Add(FilePathTagId, FilePathTagName);
            TagIdsNames.Add(FilePathWoExtTagId, FilePathWoExtTagName);
            TagIdsNames.Add(MetaDataType.AlbumUniqueId, AlbumUniqueIdName);
            TagIdsNames.Add(DateCreatedTagId, DateCreatedTagName);

            wereErrors = false;
            Encoding unicode2 = Encoding.UTF8;
            System.IO.FileStream stream2 = System.IO.File.Open(System.IO.Path.Combine(MbApiInterface.Setting_GetPersistentStoragePath(), "TagTools.PropNamesErrorLog.txt"), System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
            System.IO.StreamWriter file2 = new System.IO.StreamWriter(stream2, unicode2);

            PropNamesIds.Clear();
            PropIdsNames.Clear();

            try
            {
                file2.WriteLine("Adding " + UrlTagName + " / " + FilePropertyType.Url);
                PropNamesIds.Add(UrlTagName, FilePropertyType.Url);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + UrlTagName + " / " + FilePropertyType.Url);
            }
            try
            {
                file2.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Kind) + " / " + FilePropertyType.Kind);
                PropNamesIds.Add(MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Kind), FilePropertyType.Kind);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Kind) + " / " + FilePropertyType.Kind);
            }
            try
            {
                file2.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Format) + " / " + FilePropertyType.Format);
                PropNamesIds.Add(MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Format), FilePropertyType.Format);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Format) + " / " + FilePropertyType.Format);
            }
            try
            {
                file2.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Size) + " / " + FilePropertyType.Size);
                PropNamesIds.Add(MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Size), FilePropertyType.Size);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Size) + " / " + FilePropertyType.Size);
            }
            try
            {
                file2.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Channels) + " / " + FilePropertyType.Channels);
                PropNamesIds.Add(MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Channels), FilePropertyType.Channels);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Channels) + " / " + FilePropertyType.Channels);
            }
            try
            {
                file2.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.SampleRate) + " / " + FilePropertyType.SampleRate);
                PropNamesIds.Add(MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.SampleRate), FilePropertyType.SampleRate);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.SampleRate) + " / " + FilePropertyType.SampleRate);
            }
            try
            {
                file2.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Bitrate) + " / " + FilePropertyType.Bitrate);
                PropNamesIds.Add(MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Bitrate), FilePropertyType.Bitrate);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Bitrate) + " / " + FilePropertyType.Bitrate);
            }
            try
            {
                file2.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Bitdepth) + " / " + FilePropertyType.Bitdepth);
                PropNamesIds.Add(MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Bitdepth), FilePropertyType.Bitdepth);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Bitdepth) + " / " + FilePropertyType.Bitdepth);
            }
            try
            {
                file2.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.DateModified) + " / " + FilePropertyType.DateModified);
                PropNamesIds.Add(MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.DateModified), FilePropertyType.DateModified);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.DateModified) + " / " + FilePropertyType.DateModified);
            }
            try //"Date added" is now writable
            {
                file2.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.DateAdded) + " / " + FilePropertyType.DateAdded);
                TagNamesIds.Add(MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.DateAdded), (MetaDataType)(int)FilePropertyType.DateAdded);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.DateAdded) + " / " + FilePropertyType.DateAdded);
            }
            try
            {
                file2.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Duration) + " / " + FilePropertyType.Duration);
                PropNamesIds.Add(MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Duration), FilePropertyType.Duration);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Duration) + " / " + FilePropertyType.Duration);
            }
            try
            {
                file2.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.ReplayGainTrack) + " / " + FilePropertyType.ReplayGainTrack);
                PropNamesIds.Add(MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.ReplayGainTrack), FilePropertyType.ReplayGainTrack);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.ReplayGainTrack) + " / " + FilePropertyType.ReplayGainTrack);
            }
            try
            {
                file2.WriteLine("Adding " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.ReplayGainAlbum) + " / " + FilePropertyType.ReplayGainAlbum);
                PropNamesIds.Add(MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.ReplayGainAlbum), FilePropertyType.ReplayGainAlbum);
            }
            catch (ArgumentException)
            {
                wereErrors = true;
                file2.WriteLine("Cant add " + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.ReplayGainAlbum) + " / " + FilePropertyType.ReplayGainAlbum);
            }

            if (wereErrors)
            {
                MessageBox.Show(MbForm, "Some track property names are duplicated. See \"" +
                    System.IO.Path.Combine(MbApiInterface.Setting_GetPersistentStoragePath(), "TagTools.PropNamesErrorLog.txt")
                    + "\" file for details. MusicBeePlugin is not properly initialized.",
                    string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            file2.Close();


            PropIdsNames.Add(FilePropertyType.Url, UrlTagName);
            PropIdsNames.Add(FilePropertyType.Kind, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Kind));
            PropIdsNames.Add(FilePropertyType.Format, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Format));
            PropIdsNames.Add(FilePropertyType.Size, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Size));
            PropIdsNames.Add(FilePropertyType.Channels, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Channels));
            PropIdsNames.Add(FilePropertyType.SampleRate, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.SampleRate));
            PropIdsNames.Add(FilePropertyType.Bitrate, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Bitrate));
            PropIdsNames.Add(FilePropertyType.Bitdepth, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Bitdepth));
            PropIdsNames.Add(FilePropertyType.DateModified, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.DateModified));
            TagIdsNames.Add((MetaDataType)(int)FilePropertyType.DateAdded, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.DateAdded)); //"Date Added" is now writable
            PropIdsNames.Add(FilePropertyType.Duration, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Duration));
            PropIdsNames.Add(FilePropertyType.ReplayGainTrack, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.ReplayGainTrack));
            PropIdsNames.Add(FilePropertyType.ReplayGainAlbum, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.ReplayGainAlbum));
        }
    }
}
