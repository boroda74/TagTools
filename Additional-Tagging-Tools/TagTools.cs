using MusicBeePlugin.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using static MusicBeePlugin.AdvancedSearchAndReplaceCommand;

namespace MusicBeePlugin
{
    #region Custom types
    public class SizePositionType
    {
        public string className;
        public int x;
        public int y;
        public int w;
        public int h;
        public bool max;

        public int column1Width;
        public int column2Width;
        public int column3Width;

        public int table2column1Width;
        public int table2column2Width;
        public int table2column3Width;
    }

    public class TagSetType
    {
        public string setName;
        public int[] tagIds;
        public int setPosition;

        public string getPrefix()
        {
            return "[" + setPosition.ToString("D2") + "] ";
        }

        public override string ToString()
        {
            return getPrefix() + setName;
        }
    }

    public enum FunctionType
    {
        Grouping = 0,
        Count,
        Sum,
        Minimum,
        Maximum,
        Average,
        AverageCount
    }
    #endregion

    #region Main module
    public partial class Plugin
    {
        #region Members
        public static bool DeveloperMode = false;
        private static bool Uninstalled = false;

        public static MusicBeeApiInterface MbApiInterface;
        private static readonly PluginInfo About = new PluginInfo();

        public const int MaximumNumberOfASRHotkeys = 20;

        public static char LocalizedDecimalPoint = (0.5).ToString()[1];
        public static Bitmap MissingArtwork;
        private static readonly Random RandomGenerator = new Random();

        public static Form MbForm;
        public static List<PluginWindowTemplate> OpenedForms = new List<PluginWindowTemplate>();
        public static int NumberOfNativeMbBackgroundTasks = 0;

        private static bool PrioritySet = false; //Plugin's background thread priority must be set to 'lower'

        public static int NumberOfTagChanges = 0;

        public static bool BackupIsAlwaysNeeded = true;
        public static SortedDictionary<int, bool> TracksNeedsToBeBackuped = new SortedDictionary<int, bool>();
        public static SortedDictionary<int, bool> TempTracksNeedsToBeBackuped = new SortedDictionary<int, bool>();
        public static int UpdatedTracksForBackupCount = 0;
        public const int MaxUpdatedTracksCount = 5000;
        public static string MusicName;

        private static string LastMessage = "";

        public static SortedDictionary<string, MetaDataType> TagNamesIds = new SortedDictionary<string, MetaDataType>();
        public static Dictionary<MetaDataType, string> TagIdsNames = new Dictionary<MetaDataType, string>();

        public static SortedDictionary<string, FilePropertyType> PropNamesIds = new SortedDictionary<string, FilePropertyType>();
        public static Dictionary<FilePropertyType, string> PropIdsNames = new Dictionary<FilePropertyType, string>();

        private delegate void AutoApplyDelegate(object currentFileObj, object tagToolsPluginObj);
        private readonly AutoApplyDelegate autoApplyDelegate = AutoApply;

        private static readonly List<string> FilesUpdatedByPlugin = new List<string>();
        private static readonly List<string> ChangedFiles = new List<string>();

        private static System.Threading.Timer PeriodicUI_RefreshTimer = null;
        private static bool UiRefreshingIsNeeded = false;
        private static TimeSpan RefreshUI_Delay = new TimeSpan(0, 0, 5);
        public static DateTime LastUI_Refresh = DateTime.MinValue;
        private static readonly object LastUI_RefreshLocker = new object();

        public static System.Threading.Timer PeriodicAutobackupTimer = null;
        public static object AutobackupLocker = new object();

        public static Button EmptyButton = new Button();

        public static string[] ReadonlyTagsNames;
        private static string[] Conditions;

        public static string Language;
        private static string PluginSettingsFileName = "mb_TagTools.settings.xml";
        private static string PluginSettingsFilePath;
        public const string BackupIndexFileName = ".Master Tag Index.mbi";
        public static string BackupDefaultPrefix = "Tag Backup ";
        public static BackupIndex BackupIndex;

        public const int StatusbarTextUpdateInterval = 50;
        public static char[] FilesSeparators = { '\0' };

        public static List<Preset> AutoAppliedPresets = new List<Preset>();
        public const string AsrPresetsDirectory = "ASR Presets";
        public static string PluginsPath;
        public const string OldASRPresetExtension = ".ASR Preset.xml";//***
        public const string ASRPresetExtension = ".asr-preset.xml";
        public static string ASRPresetNaming = "ASR preset";
        public static Preset[] AsrPresetsWithHotkeys = new Preset[MaximumNumberOfASRHotkeys];
        public static int ASRPresetsWithHotkeysCount = 0;

        public static SortedDictionary<string, Preset> AsrIdsPresets = new SortedDictionary<string, Preset>();
        public static Preset MSR;

        public const char MultipleItemsSplitterId = '\0';
        public const char GuestId = '\x01';
        public const char PerformerId = '\x02';
        public const char RemixerId = '\x04';
        public const char EndOfStringId = '\x08';

        public const string TotalsString = "\u0001";
        public const int NumberOfPredefinedPresets = 5;

        public static string LastCommandSbText;
        private static bool LastPreview;
        private static int LastFileCounter;

        public static LibraryReportsCommand.LibraryReportsPreset LibraryReportsPreset = new LibraryReportsCommand.LibraryReportsPreset();

        public static ToolStripMenuItem TagToolsSubmenu;
        public static ToolStripMenuItem TagToolsContextSubmenu;

        public static ToolStripMenuItem CopyTagsToClipboardUsingMenuItem = null;
        public static ToolStripMenuItem CopyTagsToClipboardUsingContextMenuItem = null;
        public static ToolStripMenuItem ASRPresetsMenuItem = null;
        public static ToolStripMenuItem ASRPresetsContextMenuItem = null;

        private int InitialSkipCount;
        #endregion

        #region Localized strings
        //Some workarounds
        public const MetaDataType DisplayedArtistId = (MetaDataType)(-1);
        public const MetaDataType ArtistArtistsId = (MetaDataType)(-2);
        public const MetaDataType DisplayedComposerId = (MetaDataType)(-3);
        public const MetaDataType ComposerComposersId = (MetaDataType)(-4);
        public const MetaDataType LyricsId = (MetaDataType)(-5);
        public const MetaDataType SynchronisedLyricsId = (MetaDataType)(-6);
        public const MetaDataType UnsynchronisedLyricsId = (MetaDataType)(-7);

        public const MetaDataType NullTagId = (MetaDataType)(-20);
        public const MetaDataType DateCreatedTagId = (MetaDataType)(-21);
        public const MetaDataType ClipboardTagId = (MetaDataType)(-150);

        public const MetaDataType FolderTagId = (MetaDataType)(-151);
        public const MetaDataType FileNameTagId = (MetaDataType)(-152);
        public const MetaDataType FilePathTagId = (MetaDataType)(-153);

        public static string DisplayedArtistName;
        public static string ArtistArtistsName;
        public static string DisplayedComposerName;
        public static string ComposerComposersName;
        public static string DisplayedAlbumArtsistName;
        public static string ArtworkName;
        public static string LyricsName;
        public static string LyricsNamePostfix;
        public static string SynchronisedLyricsName;
        public static string UnsynchronisedLyricsName;

        public static string NullTagName;

        public static string FolderTagName;
        public static string FileNameTagName;
        public static string FilePathTagName;
        public static string AlbumUniqueIdName;

        public static string GenericTagSetName;
        #endregion


        #region Defaults for controls
        //Defaults for controls
        public static DataGridViewCellStyle UnchangedStyle = new DataGridViewCellStyle();
        public static DataGridViewCellStyle ChangedStyle = new DataGridViewCellStyle();
        #endregion

        #region Settings
        public class SavedSettingsType
        {
            public bool dontShowContextMenu;

            public bool dontShowCopyTag;
            public bool dontShowSwapTags;
            public bool dontShowChangeCase;
            public bool dontShowRencodeTag;
            public bool dontShowLibraryReports;
            public bool dontShowAutorate;
            public bool dontShowASR;
            public bool dontShowCAR;
            public bool dontShowCT;
            public bool dontShowShowHiddenWindows;
            public bool dontShowBackupRestore;

            public bool useSkinColors;
            public bool dontHighlightChangedTags;
            public int closeShowHiddenWindows;

            public bool dontPlayCompletedSound;
            public bool playStartedSound;
            public bool playCanceledSound;
            public bool dontPlayTickedAsrPresetSound;

            public string copySourceTagName;
            public string changeCaseSourceTagName;
            public string swapTagsSourceTagName;

            public string copyDestinationTagName;
            public string swapTagsDestinationTagName;

            public string initialEncodingName;
            public string usedEncodingName;
            public string reencodeTagSourceTagName;

            public bool onlyIfDestinationIsEmpty;
            public bool smartOperation;
            public bool appendSource;
            public bool addSource;
            public string[] customText;
            public string[] appendedText;
            public string[] addedText;

            public int changeCaseFlag;
            public bool useExceptionWords;
            public bool useOnlyWords;
            public string[] exceptionWords;
            public string exceptionWordsASR;
            public bool useExceptionChars;
            public string exceptionChars;
            public string exceptionCharsASR;
            public bool useWordSplitters;
            public string wordSplitters;
            public string wordSplittersASR;
            public bool alwaysCapitalize1stWord;
            public bool alwaysCapitalizeLastWord;

            public LibraryReportsCommand.LibraryReportsPreset[] libraryReportsPresets;
            public int libraryReportsPresetNumber;
            public string savedFieldName;
            public string destinationTagOfSavedFieldName;
            public bool conditionIsChecked;
            public string conditionTagName;
            public string condition;
            public string comparedField;
            public int filterIndex;

            public AutoLibraryReportCommand.AutoLibraryReportsPreset[] autoLibraryReportsPresets;
            public bool recalculateOnNumberOfTagsChanges;
            public decimal numberOfTagsToRecalculate;

            public string unitK;
            public string unitM;
            public string unitG;

            public string multipleItemsSplitterChar1;
            public string multipleItemsSplitterChar2;

            public string exportedTrackListName;

            public MetaDataType autoRateTagId;
            public bool storePlaysPerDay;
            public MetaDataType playsPerDayTagId;

            public bool autoRateAtStartUp;
            public bool notifyWhenAutoratingCompleted;
            public bool calculateThresholdsAtStartUp;
            public bool autoRateOnTrackProperties;
            public bool sinceAdded;

            public bool calculateAlbumRatingAtStartUp;
            public bool calculateAlbumRatingAtTagsChanged;
            public bool notifyWhenCalculationCompleted;
            public bool considerUnrated;
            public int defaultRating;
            public string trackRatingTagName;
            public string albumRatingTagName;

            public bool resizeArtwork;
            public ushort xArtworkSize;
            public ushort yArtworkSize;
            public bool openReportAfterExporting;

            public bool checkBox5;
            public double threshold5;
            public bool checkBox45;
            public double threshold45;
            public bool checkBox4;
            public double threshold4;
            public bool checkBox35;
            public double threshold35;
            public bool checkBox3;
            public double threshold3;
            public bool checkBox25;
            public double threshold25;
            public bool checkBox2;
            public double threshold2;
            public bool checkBox15;
            public double threshold15;
            public bool checkBox1;
            public double threshold1;
            public bool checkBox05;
            public double threshold05;

            public decimal perCent5;
            public decimal perCent45;
            public decimal perCent4;
            public decimal perCent35;
            public decimal perCent3;
            public decimal perCent25;
            public decimal perCent2;
            public decimal perCent15;
            public decimal perCent1;
            public decimal perCent05;

            public decimal actualPerCent5;
            public decimal actualPerCent45;
            public decimal actualPerCent4;
            public decimal actualPerCent35;
            public decimal actualPerCent3;
            public decimal actualPerCent25;
            public decimal actualPerCent2;
            public decimal actualPerCent15;
            public decimal actualPerCent1;
            public decimal actualPerCent05;

            public DateTime lastAsrImportDateUtc;

            public List<SizePositionType> commandWindows;

            public int lastInteractiveTagSet = 0;
            public int lastTagSet = 0;
            public TagSetType[] copyTagsTagSets;

            public string autobackupDirectory;
            public string autobackupPrefix;
            public decimal autobackupInterval;
            public decimal autodeleteKeepNumberOfDays;
            public decimal autodeleteKeepNumberOfFiles;

            public int rowHeadersWidth;
            public int defaultColumnWidth;
            public decimal defaultTagHistoryNumberOfBackups;
            public int[] displayedTags;
            public bool DontAutoSelectDisplayedTags;

            public string[] lastSelectedFolders;

            public bool backupArtworks;
            public bool dontTryToGuessLibraryName;
            public bool dontSkipAutobackupsIfPlayCountsChanged;

            public List<Guid> autoAppliedAsrPresetGuids = new List<Guid>();
            public Guid[] asrPresetsWithHotkeysGuids = new Guid[MaximumNumberOfASRHotkeys];

            public bool dontShowPredefinedPresetsCantBeChangedMessage;

            public int lastSkippedTagId;
            public int lastSkippedDateFormat;
        }

        public static SavedSettingsType SavedSettings;
        #endregion


        #region Other localized strings
        //Localizable strings

        //Supported exported file formats
        public static string ExportedFormats;

        //Plugin localizable strings
        public static string PluginName;
        public static string PluginMenuGroup;
        private static string Description;

        private static string TagToolsMenuSectionName;
        private static string BackupRestoreMenuSectionName;

        public static string CopyTagCommandName;
        public static string SwapTagsCommandName;
        public static string ChangeCaseCommandName;
        public static string ReencodeTagCommandName;
        public static string ReencodeTagsCommandName;
        public static string LibraryReportsCommandName;
        public static string AutoLibraryReportsCommandName;
        public static string AutoRateCommandName;
        public static string AsrCommandName;
        public static string CarCommandName;
        public static string CtCommandName;
        public static string CopyTagsToClipboardCommandName;
        public static string PasteTagsFromClipboardCommandName;
        public static string MsrCommandName;
        public static string ShowHiddenCommandName;

        private static string TagToolsHotkeyDescription;
        private static string CopyTagCommandDescription;
        private static string SwapTagsCommandDescription;
        private static string ChangeCaseCommandDescription;
        private static string ReencodeTagCommandDescription;
        private static string ReencodeTagsCommandDescription;
        private static string LibraryReportsCommandDescription;
        private static string AutoLibraryReportsCommandDescription;
        private static string AutoRateCommandDescription;
        private static string AsrCommandDescription;
        public static string AsrHotkeyDescription;
        private static string CarCommandDescription;
        private static string CtCommandDescription;
        private static string CopyTagsToClipboardCommandDescription;
        private static string PasteTagsFromClipboardCommandDescription;
        private static string CopyTagsToClipboardUsingMenuDescription;
        private static string MsrCommandDescription;
        private static string ShowHiddenCommandDescription;

        public static string BackupTagsCommandName;
        public static string RestoreTagsCommandName;
        public static string RestoreTagsForEntireLibraryCommandName;
        public static string RenameMoveBackupCommandName;
        public static string MoveBackupsCommandName;
        public static string CreateNewBaselineCommandName;
        public static string DeleteBackupsCommandName;
        public static string AutoBackupSettingsCommandName;

        public static string TagHistoryCommandName;

        private static string BackupTagsCommandDescription;
        private static string RestoreTagsCommandDescription;
        private static string RestoreTagsForEntireLibraryCommandDescription;
        private static string RenameMoveBackupCommandDescription;
        private static string MoveBackupsCommandDescription;
        private static string CreateNewBaselineCommandDescription;
        private static string DeleteBackupsCommandDescription;
        private static string AutoBackupSettingsCommandDescription;

        private static string TagHistoryCommandDescription;

        public static string CopyTagCommandSbText;
        public static string SwapTagsCommandSbText;
        public static string ChangeCaseCommandSbText;
        public static string ReencodeTagCommandSbText;
        public static string LibraryReportsCommandSbText;
        public static string LibraryReportsGneratingPreviewCommandSbText;
        public static string AutoRateCommandSbText;
        public static string AsrCommandSbText;
        public static string CarCommandSbText;
        public static string MsrCommandSbText;

        //Other localizable strings
        public static string AlbumTagName;
        public static string Custom9TagName;
        public static string UrlTagName;
        public static string GenreCategoryName;

        public static string GroupingName;
        public static string CountName;
        public static string SumName;
        public static string MinimumName;
        public static string MaximumName;
        public static string AverageName;
        public static string AverageCountName;

        public static string LibraryReport;

        public static string DateCreatedTagName;
        public static string EmptyValueTagName;
        public static string ClipboardTagName;
        public static string TextFileTagName;
        public static string SequenceNumberName;

        public static string ParameterTagName;
        public static string TempTagName;

        public static string CustomTagsPresetName;
        public static string LibraryTotalsPresetName;
        public static string LibraryAveragesPresetName;
        public static string CDBookletPresetName;
        public static string AlbumsAndTracksPresetName;

        public static string EmptyPresetName;

        //Displayed text
        public static string ListItemConditionIs;
        public static string ListItemConditionIsNot;
        public static string ListItemConditionIsGreater;
        public static string ListItemConditionIsLess;

        public static string OKButtonName;
        public static string StopButtonName;
        public static string CancelButtonName;
        public static string HideButtonName;
        public static string PreviewButtonName;
        public static string ClearButtonName;
        public static string FindButtonName;
        public static string SelectFoundButtonName;
        public static string OriginalTagHeaderTextTagValue;
        public static string OriginalTagHeaderText;

        public static string TableCellError;
        
        public static string DefaultASRPresetName;


        public static string SbSorting;
        public static string SbUpdating;
        public static string SbReading;
        public static string SbUpdated;
        public static string SbRead;
        public static string SbFiles;
        public static string SbAsrPresetIsApplied;
        public static string SbAsrPresetsAreApplied;

        public static string MsgFileNotFound;
        public static string MsgNoFilesSelected;
        public static string MsgNoFilesDisplayed;
        public static string MsgSourceAndDestinationTagsAreTheSame;
        public static string MsgSwapTagsSourceAndDestinationTagsAreTheSame;
        public static string MsgNoTagsSelected;
        public static string MsgNoFilesInCurrentView;
        public static string MsgTracklistIsEmpty;
        public static string MsgForExportingPlaylistsURLfieldMustBeIncludedInTagList;
        public static string MsgPreviewIsNotGeneratedNothingToSave;
        public static string MsgPreviewIsNotGeneratedNothingToChange;
        public static string MsgNoAggregateFunctionNothingToSave;
        public static string MsgPleaseUseGroupingFunctionForArtworkTag;
        public static string MsgAllTags;
        public static string MsgNoURLcolumnUnableToSave;
        public static string MsgEmptyURL;
        public static string MsgUnableToSave;
        public static string MsgUsingEmptyValueAsSourceTagMayBeUsefulOnlyForAppendingStaticTextToDestinationtag;
        public static string MsgBackgroundTaskIsCompleted;
        public static string MsgThresholdsDescription;
        public static string MsgAutoCalculationOfThresholdsDescription;

        public static string MsgNumberOfPlayedTracks;
        public static string MsgIncorrectSumOfWeights;
        public static string MsgSum;
        public static string MsgNumberOfNotRatedTracks;
        public static string MsgTracks;
        public static string MsgActualPercent;

        public static string MsgIncorrectPresetName;
        public static string MsgDeletePresetConfirmation;
        public static string MsgImportingConfirmation;
        public static string MsgDoYouWantToUpdateYourCustomizedPredefinedPresets;
        public static string MsgDoYouWantToImportExistingPresetsAsCopies;
        public static string MsgPresetsWereImported;
        public static string MsgPresetsWereImportedAsCopies;
        public static string MsgPresetsFailedToImport;
        public static string MsgPresetsWereInstalled;
        public static string MsgPresetsWereUpdated;
        public static string MsgPresetsCustomizedWereUpdated;
        public static string MsgPresetsWereNotChanged;
        public static string MsgPresetsWereSkipped;
        public static string MsgPresetsFailedToUpdate;
        public static string MsgPresetsNotFound;
        public static string MsgDeletingConfirmation;
        public static string MsgNoPresetsDeleted;
        public static string MsgPresetsWereDeleted;
        public static string MsgFailedToDelete;

        public static string MsgNumberOfTagsInTextFile;
        public static string MsgDoesntCorrespondToNumberOfSelectedTracks;
        public static string MsgMessageEnd;

        public static string MsgClipboardDesntContainText;

        public static string MsgNumberOfTagsInClipboard;
        public static string MsgNumberOfTracksInClipboard;
        public static string MsgDoesntCorrespondToNumberOfSelectedTracksC;
        public static string MsgDoesntCorrespondToNumberOfCopiedTagsC;
        public static string MsgMessageEndC;
        public static string MsgDoYouWantToPasteAnyway;

        public static string MsgFirstThreeGroupingFieldsInPreviewTableShouldBe;
        public static string MsgFirstSixGroupingFieldsInPreviewTableShouldBe;

        public static string MsgBackgroundAutoLibraryReportIsExecuted;

        public static string MsgYouMustImportStandardASRPresetsFirst;

        public static string CtlDirtyError1sf;
        public static string CtlDirtyError1mf;
        public static string CtlDirtyError2sf;
        public static string CtlDirtyError2mf;


        public static string MsgMasterBackupIndexIsCorrupted;
        public static string MsgBackupIsCorrupted;
        public static string MsgFolderDoesntExists;
        public static string MsgSelectOneTrackOnly;
        public static string MsgSelectTrack;
        public static string MsgSelectAtLeast2Tracks;
        public static string MsgCreateBaselineWarning;
        public static string MsgBackupBaselineFileDoesntExist1;
        public static string MsgBackupBaselineFileDoesntExist2;
        public static string MsgThisIsTheBackupOfDifferentLibrary;
        public static string MsgGiveNameToASRpreset;
        public static string MsgAreYouSureYouWantToSaveASRpreset;
        public static string MsgAreYouSureYouWantToOverwriteASRpreset;
        public static string MsgAreYouSureYouWantToOverwriteRenameASRpreset;
        public static string MsgAreYouSureYouWantToDeleteASRpreset;
        public static string MsgPredefinedPresetsCantBeChanged;
        public static string MsgSavePreset;
        public static string MsgDeletePreset;

        public static string CtlNewASRPreset;

        public static string SbAutobackuping;
        public static string SbMovingBackupsToNewFolder;
        public static string SbMakingTagBackup;
        public static string SbRestoringTagsFromBackup;
        public static string SbRenamingMovingBackup;
        public static string SbMovingBackups;
        public static string SbDeletingBackups;
        public static string SbTagAutobackupSkipped;
        public static string SbCompairingTags;

        public static string CtlWarning;
        public static string CtlMusicBeeBackupType;
        public static string CtlSaveBackupTitle;
        public static string CtlRestoreBackupTitle;
        public static string CtlRenameSelectBackupTitle;
        public static string CtlRenameSaveBackupTitle;
        public static string CtlMoveSelectBackupsTitle;
        public static string CtlMoveSaveBackupsTitle;
        public static string CtlDeleteBackupsTitle;
        public static string CtlSelectThisFolder;
        public static string CtlNoBackupData;
        public static string CtlNoDifferences;
        public static string MsgNotAllowedSymbols;
        public static string MsgPresetExists;

        public static string CtlWholeCuesheetWillBeReencoded;

        public static string CtlMSR;

        public static string CtlUnknown;

        public static string MsgFirstSelectWhichFieldYouWantToAssignFunctionIdTo = "First select, which field you want to assign function id to (leftmost combobox on function id line)!";
        #endregion


        #region Common methods/functions
        public struct SwappedTags
        {
            public string newDestinationTagValue;
            public string newDestinationTagTValue;
            public string destinationTagTValue;
            public string newSourceTagValue;
            public string newSourceTagTValue;
            public string sourceTagTValue;
        }

        public struct ConvertStringsResults
        {
            public int type; // 4 - date-time, 3 - timespan, 2 - double, 1 - items count, 0 - unknown/string

            public double result1f;
            public double result2f;
            public string result1s;
            public string result2s;

            public SortedDictionary<string, object> items;
            public SortedDictionary<string, object> items1;

            public ConvertStringsResults(int typeParam)
            {
                type = typeParam;
                result1f = 0;
                result2f = 0;
                result1s = null;
                result2s = null;
                items = new SortedDictionary<string, object>();
                items1 = new SortedDictionary<string, object>();
            }

            public string getResult()
            {
                if (items1.Count != 0) //Its 'average count' function
                    return "" + Math.Round((double)items1.Count / items.Count, 2);
                else if (type == 0)
                {
                    if (items.Count == 0)
                        return "" + result1s;
                    else //Its 'average' function
                        return "" + Math.Round(result1f / items.Count, 2);
                }
                else if (type == 1)
                {
                    return "" + items.Count;
                }
                else if (type == 2)
                {
                    if (items.Count == 0)
                        return "" + result1f;
                    else //Its 'average' function
                        return "" + Math.Round(result1f / items.Count, 2);
                }
                else if (type == 3)
                {
                    if (items.Count == 0)
                        return "" + TimeSpan.FromSeconds(result1f);
                    else //Its 'average' function
                        return "" + TimeSpan.FromSeconds(result1f / items.Count);
                }
                else //if (type == 4)
                {
                    if (items.Count == 0)
                        return "" + (DateTime.MinValue + TimeSpan.FromSeconds(result1f));
                    else //Its 'average' function
                        return "" + (DateTime.MinValue + TimeSpan.FromSeconds(result1f / items.Count));
                }
            }
        }

        public static ConvertStringsResults ConvertStrings(string xstring, string ystring = null, bool replacements = false)
        {
            ConvertStringsResults results = new ConvertStringsResults();

            if (ystring == null)
                ystring = xstring;


            if (xstring == CtlUnknown)
            {
                results.result1f = 0;
                results.result2f = 0;
                results.type = 4;

                return results;
            }


            if (replacements)
            {
                string additionalUnitK = "";
                string additionalUnitM = "";
                string additionalUnitG = "";

                if (!string.IsNullOrWhiteSpace(SavedSettings.unitK))
                    additionalUnitK = "|" + SavedSettings.unitK;

                if (!string.IsNullOrWhiteSpace(SavedSettings.unitM))
                    additionalUnitM = "|" + SavedSettings.unitM;

                if (!string.IsNullOrWhiteSpace(SavedSettings.unitG))
                    additionalUnitG = "|" + SavedSettings.unitG;

                xstring = Regex.Replace(xstring, @"^(\d+)(\.|\,)?(\d)?(\d)?(\d)?.*?(g|г" + additionalUnitG + ").*$", "$1`$3$4$5~000000000", RegexOptions.IgnoreCase);
                ystring = Regex.Replace(ystring, @"^(\d+)(\.|\,)?(\d)?(\d)?(\d)?.*?(g|г" + additionalUnitG + ").*$", "$1`$3$4$5~000000000", RegexOptions.IgnoreCase);

                xstring = Regex.Replace(xstring, @"~", "", RegexOptions.IgnoreCase);
                ystring = Regex.Replace(ystring, @"~", "", RegexOptions.IgnoreCase);

                xstring = Regex.Replace(xstring, @"^(.*?)`(.{9}).*$", "$1$2", RegexOptions.IgnoreCase);
                ystring = Regex.Replace(ystring, @"^(.*?)`(.{9}).*$", "$1$2", RegexOptions.IgnoreCase);


                xstring = Regex.Replace(xstring, @"^(\d+)(\.|\,)?(\d)?(\d)?(\d)?.*?(m|м" + additionalUnitM + ").*$", "$1`$3$4$5~000000", RegexOptions.IgnoreCase);
                ystring = Regex.Replace(ystring, @"^(\d+)(\.|\,)?(\d)?(\d)?(\d)?.*?(m|м" + additionalUnitM + ").*$", "$1`$3$4$5~000000", RegexOptions.IgnoreCase);

                xstring = Regex.Replace(xstring, @"~", "", RegexOptions.IgnoreCase);
                ystring = Regex.Replace(ystring, @"~", "", RegexOptions.IgnoreCase);

                xstring = Regex.Replace(xstring, @"^(.*?)`(.{6}).*$", "$1$2", RegexOptions.IgnoreCase);
                ystring = Regex.Replace(ystring, @"^(.*?)`(.{6}).*$", "$1$2", RegexOptions.IgnoreCase);


                xstring = Regex.Replace(xstring, @"^(\d+)(\.|\,)?(\d)?(\d)?(\d)?.*?(k|к" + additionalUnitK + ").*$", "$1`$3$4$5~000", RegexOptions.IgnoreCase);
                ystring = Regex.Replace(ystring, @"^(\d+)(\.|\,)?(\d)?(\d)?(\d)?.*?(k|к" + additionalUnitK + ").*$", "$1`$3$4$5~000", RegexOptions.IgnoreCase);

                xstring = Regex.Replace(xstring, @"~", "", RegexOptions.IgnoreCase);
                ystring = Regex.Replace(ystring, @"~", "", RegexOptions.IgnoreCase);

                xstring = Regex.Replace(xstring, @"^(.*?)`(.{3}).*$", "$1$2", RegexOptions.IgnoreCase);
                ystring = Regex.Replace(ystring, @"^(.*?)`(.{3}).*$", "$1$2", RegexOptions.IgnoreCase);


                xstring = Regex.Replace(xstring, @"^(\d+).*$", "$1", RegexOptions.IgnoreCase);
                ystring = Regex.Replace(ystring, @"^(\d+).*$", "$1", RegexOptions.IgnoreCase);
            }


            try
            {
                results.result1f = double.Parse(xstring);
                results.result2f = double.Parse(ystring);
                results.type = 2;

                return results;
            }
            catch
            {
                try
                {
                    string[] ts1 = xstring.Split(':');
                    TimeSpan time;

                    time = TimeSpan.FromSeconds(Convert.ToInt32(ts1[ts1.Length - 1]));
                    time += TimeSpan.FromMinutes(Convert.ToInt32(ts1[ts1.Length - 2]));
                    if (ts1.Length > 2)
                        time += TimeSpan.FromHours(Convert.ToInt32(ts1[ts1.Length - 3]));

                    results.result1f = time.TotalSeconds;


                    string[] ts2 = ystring.Split(':');

                    time = TimeSpan.FromSeconds(Convert.ToInt32(ts2[ts2.Length - 1]));
                    time += TimeSpan.FromMinutes(Convert.ToInt32(ts2[ts2.Length - 2]));
                    if (ts2.Length > 2)
                        time += TimeSpan.FromHours(Convert.ToInt32(ts2[ts2.Length - 3]));

                    results.result2f = time.TotalSeconds;


                    results.type = 3;

                    return results;
                }
                catch
                {
                    try
                    {
                        results.result1f = (DateTime.Parse(xstring) - DateTime.MinValue).TotalSeconds;
                        results.result2f = (DateTime.Parse(ystring) - DateTime.MinValue).TotalSeconds;
                        results.type = 4;

                        return results;
                    }
                    catch
                    {
                        if (!replacements)
                        {
                            return ConvertStrings(xstring, ystring, true);
                        }
                        else
                        {
                            results.result1s = xstring;
                            results.result2s = ystring;
                            results.type = 0;

                            return results;
                        }
                    }
                }
            }
        }

        public static int CompareStrings(string xstring, string ystring)
        {
            ConvertStringsResults results;

            results = ConvertStrings(xstring, ystring);

            switch (results.type)
            {
                case 1:
                    if (results.result1f > results.result2f)
                        return 1;
                    else if (results.result1f < results.result2f)
                        return -1;
                    else
                        return 0;

                case 2:
                    if (results.result1f > results.result2f)
                        return 1;
                    else if (results.result1f < results.result2f)
                        return -1;
                    else
                        return 0;

                case 3:
                    if (results.result1f > results.result2f)
                        return 1;
                    else if (results.result1f < results.result2f)
                        return -1;
                    else
                        return 0;

                case 4:
                    if (results.result1f > results.result2f)
                        return 1;
                    else if (results.result1f < results.result2f)
                        return -1;
                    else
                        return 0;

                default:
                    return String.Compare(xstring, ystring);
            }
        }

        public class DataGridViewCellComparer : System.Collections.IComparer
        {
            public int comparedColumnIndex = -1;

            public int Compare(object x, object y)
            {
                if (comparedColumnIndex != -1)
                {
                    int comparison = CompareStrings((string)(((DataGridViewRow)x).Cells[comparedColumnIndex].Value), (string)(((DataGridViewRow)y).Cells[comparedColumnIndex].Value));
                    if (comparison > 0)
                        return 1;
                    else if (comparison < 0)
                        return -1;
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
        }


        public class TextTableComparer : IComparer<string[]>
        {
            public int tagCounterIndex = -1;

            public int Compare(string[] x, string[] y)
            {
                if (tagCounterIndex != -1)
                {
                    int comparation;

                    for (int i = 0; i < tagCounterIndex; i++)
                    {
                        comparation = CompareStrings(x[i], y[i]);

                        if (comparation > 0)
                            return 1;
                        else if (comparation < 0)
                            return -1;
                    }

                    return 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        public static void UpdateTrackForBackup(string trackUrl)
        {
            string id = GetPersistentTrackId(trackUrl);

            if (id == null)
                return;

            string nodeName = MbApiInterface.Library_GetFileTag(trackUrl, (MetaDataType)42);

            if (nodeName != MusicName)
                return;


            int trackId = int.Parse(id);


            if (UpdatedTracksForBackupCount < MaxUpdatedTracksCount)
            {
                if (!TracksNeedsToBeBackuped.TryGetValue(trackId, out _))
                {
                    TracksNeedsToBeBackuped.Add(trackId, true);
                    UpdatedTracksForBackupCount++;
                }
            }
            else
            {
                TracksNeedsToBeBackuped.Clear();
                UpdatedTracksForBackupCount = 0;
                BackupIsAlwaysNeeded = true;
            }
        }

        public static string GetTagRepresentation(string tag)
        {
            return ReplaceMSIds(RemoveMSIdAtTheEndOfString(RemoveRoleIds(tag)));
        }

        public static string GetTrackRepresentation(string currentFile)
        {
            string trackRepresentation = "";
            string displayedArtist;
            string album;
            string title;
            string diskNo;
            string trackNo;

            displayedArtist = GetFileTag(currentFile, DisplayedArtistId);
            album = GetFileTag(currentFile, MetaDataType.Album);
            title = GetFileTag(currentFile, MetaDataType.TrackTitle);
            diskNo = GetFileTag(currentFile, MetaDataType.DiscNo);
            trackNo = GetFileTag(currentFile, MetaDataType.TrackNo);

            trackRepresentation += diskNo;
            trackRepresentation += (trackRepresentation == "") ? (trackNo) : ("-" + trackNo);
            trackRepresentation += (trackRepresentation == "") ? "" : ". ";
            trackRepresentation += (trackRepresentation == "") ? (displayedArtist) : (". " + displayedArtist);
            trackRepresentation += (trackRepresentation == "") ? (album) : (" - " + album);
            trackRepresentation += (trackRepresentation == "") ? (title) : (" - " + title);

            return trackRepresentation;
        }

        public static string GetTrackRepresentation(string[] tags, string[] tags2, string[] tagNames, bool previewSortTags)
        {
            string trackRepresentation = "";

            trackRepresentation += tags[12]; //12 - track number
            trackRepresentation += (trackRepresentation == "") ? tags[28] : ("-" + tags[28]); //28 - disk number
            trackRepresentation += (trackRepresentation == "") ? "" : ". ";

            trackRepresentation += tags[5]; //5 - Album artist or artist
            trackRepresentation += (trackRepresentation == "") ? tags[3] : (" - " + tags[3]); //3 - album name
            trackRepresentation += (trackRepresentation == "") ? tags[2] : (" - " + tags[2]); //2 - track tiltle

            trackRepresentation += " (";

            for (int i = 0; i < tags.Length; i++)
            {
                if (i == 12)
                    continue;
                else if (i == 28)
                    continue;
                else if (i == 4)
                    continue;
                else if (i == 3)
                    continue;
                else if (i == 2)
                    continue;

                if (!previewSortTags)
                {
                    if (i == 52 || i == 53 || i == 54 || i == 55 || i == 56)
                        continue;
                }

                if (tags[i] != tags2[i])
                    trackRepresentation += tagNames[i] + ": " + tags[i] + "; ";
            }

            if (trackRepresentation[trackRepresentation.Length - 2] == ';')
                trackRepresentation = trackRepresentation.Remove(trackRepresentation.Length - 2) + ")";

            if (trackRepresentation[trackRepresentation.Length - 1] == '(')
                trackRepresentation = trackRepresentation.Remove(trackRepresentation.Length - 2);

            trackRepresentation = Regex.Replace(trackRepresentation, "\0", "; "); //Remove multile artist/composer splitters
            trackRepresentation = Regex.Replace(trackRepresentation, "\x01", ""); //Here and below: remove performer/remixer/guest ids
            trackRepresentation = Regex.Replace(trackRepresentation, "\x02", "");
            trackRepresentation = Regex.Replace(trackRepresentation, "\x03", "");

            return trackRepresentation;
        }

        public static SwappedTags SwapTags(string sourceTagValue, string destinationTagValue, MetaDataType sourceTagId, MetaDataType destinationTagId,
            bool smartOperation, bool appendSourceToDestination = false, string appendedText = "", bool addSourceToDestination = false, string addedText = "")
        {
            SwappedTags swappedTags = new SwappedTags();
            string appendedValue;

            if (smartOperation)
            {
                if (appendSourceToDestination)
                {
                    if (destinationTagId == ArtistArtistsId)
                        appendedValue = ReplaceMSChars(appendedText) + RemoveMSIdAtTheEndOfString(sourceTagValue);
                    else if (destinationTagId == ComposerComposersId)
                        appendedValue = ReplaceMSChars(appendedText) + RemoveMSIdAtTheEndOfString(RemoveRoleIds(sourceTagValue));
                    else
                        appendedValue = appendedText + GetTagRepresentation(sourceTagValue);

                    swappedTags.newDestinationTagValue = RemoveMSIdAtTheEndOfString(destinationTagValue) + appendedValue;
                }
                else if (addSourceToDestination)
                {
                    if (destinationTagId == ArtistArtistsId)
                        appendedValue = RemoveMSIdAtTheEndOfString(sourceTagValue) + ReplaceMSChars(addedText);
                    else if (destinationTagId == ComposerComposersId)
                        appendedValue = RemoveMSIdAtTheEndOfString(RemoveRoleIds(sourceTagValue)) + ReplaceMSChars(addedText);
                    else
                        appendedValue = GetTagRepresentation(sourceTagValue) + addedText;

                    swappedTags.newDestinationTagValue = appendedValue + RemoveMSIdAtTheEndOfString(destinationTagValue);
                }
                else
                {
                    if (destinationTagId == ArtistArtistsId)
                        appendedValue = RemoveMSIdAtTheEndOfString(sourceTagValue);
                    else if (destinationTagId == ComposerComposersId)
                        appendedValue = RemoveMSIdAtTheEndOfString(RemoveRoleIds(sourceTagValue));
                    else
                        appendedValue = GetTagRepresentation(sourceTagValue);

                    swappedTags.newDestinationTagValue = appendedValue;
                }
                swappedTags.newDestinationTagTValue = ReplaceMSIds(RemoveRoleIds(swappedTags.newDestinationTagValue));
                swappedTags.destinationTagTValue = GetTagRepresentation(destinationTagValue);

                if (sourceTagId == destinationTagId) //Smart conversion of multiple items of one tag
                {
                    if (ReplaceMSIds(sourceTagValue) == sourceTagValue) //No MS ids, its a single item
                    {
                        appendedValue = ReplaceMSChars(sourceTagValue);
                    }
                    else //Multiple items
                    {
                        appendedValue = GetTagRepresentation(sourceTagValue);
                    }
                }
                else //Normal swapping
                {
                    if (sourceTagId == ArtistArtistsId)
                        appendedValue = RemoveMSIdAtTheEndOfString(destinationTagValue);
                    else if (sourceTagId == ComposerComposersId)
                        appendedValue = RemoveMSIdAtTheEndOfString(RemoveRoleIds(destinationTagValue));
                    else
                        appendedValue = GetTagRepresentation(destinationTagValue);
                }

                swappedTags.newSourceTagValue = appendedValue;
                swappedTags.newSourceTagTValue = ReplaceMSIds(RemoveRoleIds(swappedTags.newSourceTagValue));
                swappedTags.sourceTagTValue = GetTagRepresentation(sourceTagValue);
            }
            else
            {
                if (appendSourceToDestination)
                {
                    appendedValue = appendedText + sourceTagValue;
                    swappedTags.newDestinationTagValue = destinationTagValue + appendedValue;
                }
                else if (addSourceToDestination)
                {
                    appendedValue = sourceTagValue + addedText;
                    swappedTags.newDestinationTagValue = appendedValue + destinationTagValue;
                }
                else
                {
                    appendedValue = sourceTagValue;
                    swappedTags.newDestinationTagValue = appendedValue;
                }
                swappedTags.newDestinationTagTValue = swappedTags.newDestinationTagValue;
                swappedTags.destinationTagTValue = destinationTagValue;


                appendedValue = destinationTagValue;
                swappedTags.newSourceTagValue = appendedValue;
                swappedTags.newSourceTagTValue = swappedTags.newSourceTagValue;
                swappedTags.sourceTagTValue = sourceTagValue;
            }


            return swappedTags;
        }

        public static void ComboBoxLeave(ComboBox comboBox, string newValue = null)
        {
            string comboBoxText = (newValue ?? comboBox.Text);

            if (comboBoxText == "")
                return;

            if (comboBox.Items.Contains(comboBoxText))
                comboBox.Items.Remove(comboBoxText);
            else
                comboBox.Items.RemoveAt(9);

            comboBox.Items.Insert(0, comboBoxText);

            comboBox.Text = comboBoxText;
        }

        public static void SetItemInComboBox(ComboBox comboBox, object item)
        {
            bool itemIsFound = false;

            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if (("" + comboBox.Items[i]) == ("" + item))
                {
                    comboBox.Items.RemoveAt(i);
                    itemIsFound = true;
                    break;
                }
            }

            if (!itemIsFound)
                comboBox.Items.RemoveAt(9);

            comboBox.Items.Insert(NumberOfPredefinedPresets, item);
            comboBox.SelectedItem = item;
        }

        public static string GetFileTag(string sourceFileUrl, MetaDataType tagId, bool autoAlbumArtist = false, bool normalizeTrackRatingTo0_100Range = false)
        {
            string tag = "";
            string rawArtist = "";
            string multiArtist = "";
            string rawComposer = "";
            string multiComposer = "";


            switch (tagId)
            {
                case (MetaDataType)0:
                    break;

                case DateCreatedTagId:
                    try
                    {
                        var fileInfo = new FileInfo(sourceFileUrl);
                        tag = fileInfo.CreationTime.ToString("G");
                    }
                    catch (Exception ex)
                    {
                        SetStatusbarText(ex.Message, false);
                        return "";
                    }

                    break;

                case NullTagId:
                    break;

                case DisplayedArtistId:
                    tag = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Artist);
                    break;

                case ArtistArtistsId:
                    rawArtist = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Artist);
                    multiArtist = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.MultiArtist);

                    if (multiArtist != "")
                        tag = multiArtist;
                    else
                        tag = rawArtist;
                    break;

                case DisplayedComposerId:
                    tag = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Composer);
                    break;

                case ComposerComposersId:
                    rawComposer = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Composer);
                    multiComposer = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.MultiComposer);

                    if (multiComposer != "")
                        tag = multiComposer;
                    else
                        tag = rawComposer;
                    break;

                case MetaDataType.Artwork:
                    tag = MbApiInterface.Library_GetArtwork(sourceFileUrl, 0);
                    break;

                case LyricsId:
                    tag = MbApiInterface.Library_GetLyrics(sourceFileUrl, LyricsType.NotSpecified);
                    break;

                case SynchronisedLyricsId:
                    tag = MbApiInterface.Library_GetLyrics(sourceFileUrl, LyricsType.Synchronised);
                    break;

                case UnsynchronisedLyricsId:
                    tag = MbApiInterface.Library_GetLyrics(sourceFileUrl, LyricsType.UnSynchronised);
                    break;

                case MetaDataType.Rating:
                    tag = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Rating);

                    if (normalizeTrackRatingTo0_100Range)
                    {
                        if (tag != "")
                        {
                            double rating = ConvertStrings(tag).result1f;

                            if (rating <= 5)
                                rating *= 20;

                            tag = rating.ToString();
                        }
                    }

                    break;

                case FolderTagId:
                    tag = Regex.Replace(sourceFileUrl, @"^(.*\\)?(.*)\\(.*)", "$2");
                    break;

                case FileNameTagId:
                    tag = Regex.Replace(sourceFileUrl, @"^(.*\\)?(.*)\\(.*)", "$3");
                    break;

                case FilePathTagId:
                    tag = Regex.Replace(sourceFileUrl, @"^(.*)\\(.*)", "$1");
                    break;

                default:
                    tag = MbApiInterface.Library_GetFileTag(sourceFileUrl, tagId);
                    break;
            }


            if (tag == null)
                tag = "";


            return tag;
        }

        public static string[] GetFileTags(string sourceFileUrl, List<MetaDataType> tagIds, bool autoAlbumArtist = false)
        {
            MetaDataType[] tagIds2 = tagIds.ToArray();

            for (int i = 0; i < tagIds2.Length; i++)
            {
                MetaDataType tagId = tagIds2[i];

                switch (tagId)
                {
                    case NullTagId:
                        tagIds2[i] = (MetaDataType)0;
                        break;

                    case DisplayedArtistId:
                        tagIds2[i] = MetaDataType.Artist;
                        break;

                    case ArtistArtistsId:
                        tagIds2[i] = MetaDataType.MultiArtist;
                        break;

                    case DisplayedComposerId:
                        tagIds2[i] = MetaDataType.Composer;
                        break;

                    case ComposerComposersId:
                        tagIds2[i] = MetaDataType.MultiComposer;
                        break;

                    case MetaDataType.Artwork:
                        tagIds2[i] = (MetaDataType)0;
                        break;

                    case LyricsId:
                        tagIds2[i] = (MetaDataType)0;
                        break;

                    case SynchronisedLyricsId:
                        tagIds2[i] = (MetaDataType)0;
                        break;

                    case UnsynchronisedLyricsId:
                        tagIds2[i] = (MetaDataType)0;
                        break;

                    case FolderTagId:
                        tagIds2[i] = (MetaDataType)0;
                        break;

                    case FileNameTagId:
                        tagIds2[i] = (MetaDataType)0;
                        break;

                    case FilePathTagId:
                        tagIds2[i] = (MetaDataType)0;
                        break;
                }
            }



            if (!MbApiInterface.Library_GetFileTags(sourceFileUrl, tagIds2, out string[] tags))
                return null;


            for (int i = 0; i < tagIds.Count; i++)
            {
                MetaDataType tagId = tagIds[i];

                switch (tagId)
                {
                    case (MetaDataType)0:
                        tags[i] = "";
                        break;

                    case NullTagId:
                        tags[i] = "";
                        break;

                    case ArtistArtistsId:
                        if (tags[i] == "")
                            tags[i] = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Artist);
                        break;

                    case ComposerComposersId:
                        if (tags[i] == "")
                            tags[i] = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Composer);
                        break;

                    case MetaDataType.Artwork:
                        tags[i] = MbApiInterface.Library_GetArtwork(sourceFileUrl, 0);
                        break;

                    case LyricsId:
                        tags[i] = MbApiInterface.Library_GetLyrics(sourceFileUrl, LyricsType.NotSpecified);
                        break;

                    case SynchronisedLyricsId:
                        tags[i] = MbApiInterface.Library_GetLyrics(sourceFileUrl, LyricsType.Synchronised);
                        break;

                    case UnsynchronisedLyricsId:
                        tags[i] = MbApiInterface.Library_GetLyrics(sourceFileUrl, LyricsType.UnSynchronised);
                        break;

                    case FolderTagId:
                        tags[i] = Regex.Replace(sourceFileUrl, @"^(.*\\)?(.*)\\(.*)", "$2");
                        break;

                    case FileNameTagId:
                        tags[i] = Regex.Replace(sourceFileUrl, @"^(.*\\)?(.*)\\(.*)", "$3");
                        break;

                    case FilePathTagId:
                        tags[i] = Regex.Replace(sourceFileUrl, @"^(.*)\\(.*)", "$1");
                        break;
                }


                if (tags[i] == null)
                    tags[i] = "";
            }

            return tags;
        }

        //Method returns 'true' if tag was really changed and 'false' otherwise
        public static bool SetFileTag(string sourceFileUrl, MetaDataType tagId, string value, bool updateOnlyChangedTags = false)
        {
            string multiArtist = "";
            string multiComposer = "";
            bool result1;
            bool result2;


            if (tagId == (MetaDataType)(-201) || tagId == (MetaDataType)(-202) || tagId == (MetaDataType)(-203) || tagId == (MetaDataType)(-204))
                return false;

            if (tagId == DateCreatedTagId)
            {
                try
                {
                    var fileInfo = new FileInfo(sourceFileUrl);
                    fileInfo.CreationTime = DateTime.Parse(value);
                }
                catch (Exception ex)
                {
                    SetStatusbarText(ex.Message, false);
                    return false;
                }

                return true;
            }

            if (updateOnlyChangedTags)
            {
                if (GetFileTag(sourceFileUrl, tagId, true) == value)
                {
                    return false;
                }
                else
                {
                    lock (ChangedFiles)
                    {
                        if (!ChangedFiles.Contains(sourceFileUrl))
                            ChangedFiles.Add(sourceFileUrl);
                    }
                }
            }
            else
            {
                lock (ChangedFiles)
                {
                    if (!ChangedFiles.Contains(sourceFileUrl))
                        ChangedFiles.Add(sourceFileUrl);
                }
            }

            switch (tagId)
            {
                case NullTagId:
                    return true;

                case DisplayedArtistId:
                    multiArtist = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.MultiArtist);
                    result1 = MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.Artist, value);
                    result2 = MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.MultiArtist, multiArtist);
                    return result1 && result2;

                case ArtistArtistsId:
                    if (ReplaceMSIds(value) == value) //No MS Ids, single artist
                        return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.Artist, value);
                    else
                        return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.MultiArtist, value);

                case DisplayedComposerId:
                    multiComposer = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.MultiComposer);
                    result1 = MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.Composer, value);
                    result2 = MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.MultiComposer, multiComposer);
                    return result1 && result2;

                case ComposerComposersId:
                    if (ReplaceMSIds(value) == value) //No MS Ids, single composer
                        return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.Composer, value);
                    else
                        return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.MultiComposer, value);

                case MetaDataType.Artwork:
                    byte[] imageData = null;

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Bitmap));
                        Bitmap bitmapImage = (Bitmap)tc.ConvertFrom(Convert.FromBase64String(value));

                        MemoryStream tempJpeg = new MemoryStream();
                        bitmapImage.Save(tempJpeg, System.Drawing.Imaging.ImageFormat.Png);

                        imageData = tempJpeg.ToArray();
                        tempJpeg.Close();
                    }

                    return MbApiInterface.Library_SetArtworkEx(sourceFileUrl, 0, imageData);

                case MetaDataType.AlbumArtistRaw:
                    return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.AlbumArtist, value);

                case LyricsId:
                    return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.Lyrics, value);

                case MetaDataType.Rating:
                    if (value.ToLower() == "no rating")
                        return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.Rating, "");
                    else
                        return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.Rating, value);

                case MetaDataType.RatingAlbum:
                    if (value.ToLower() == "no rating")
                        return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.RatingAlbum, "");
                    else
                        return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.RatingAlbum, value);

                default:
                    return MbApiInterface.Library_SetFileTag(sourceFileUrl, tagId, value);
            }
        }

        public static bool CommitTagsToFile(string sourceFileUrl, bool ignoreTagsChanged = false, bool updateOnlyChangedTags = false)
        {
            bool result = false;

            if (updateOnlyChangedTags)
            {
                lock (ChangedFiles)
                {
                    if (!ChangedFiles.Contains(sourceFileUrl))
                        result = true;
                }

                if (result)
                    return true;
            }

            if (ignoreTagsChanged)
            {
                lock (FilesUpdatedByPlugin)
                {
                    FilesUpdatedByPlugin.Add(sourceFileUrl);
                }
            }

            result = MbApiInterface.Library_CommitTagsToFile(sourceFileUrl);

            RefreshPanels();

            lock (ChangedFiles)
            {
                if (ChangedFiles.Contains(sourceFileUrl))
                    ChangedFiles.Remove(sourceFileUrl);
            }

            return result;
        }

        public static void RefreshPanels(bool immediateRefresh = false)
        {
            if (immediateRefresh)
            {
                if (UiRefreshingIsNeeded)
                {
                    UiRefreshingIsNeeded = false;
                    lock (LastUI_RefreshLocker)
                    {
                        LastUI_Refresh = DateTime.Now;
                    }

                    MbApiInterface.MB_RefreshPanels();
                    MbApiInterface.MB_SetBackgroundTaskMessage(LastMessage);
                }
            }
            else
            {
                bool refresh = false;

                lock (LastUI_RefreshLocker)
                {
                    if (DateTime.Now - LastUI_Refresh >= RefreshUI_Delay)
                    {
                        LastUI_Refresh = DateTime.Now;
                        refresh = true;
                    }
                }


                if (refresh)
                {
                    UiRefreshingIsNeeded = false;
                    MbApiInterface.MB_RefreshPanels();
                    MbApiInterface.MB_SetBackgroundTaskMessage(LastMessage);
                }
                else
                {
                    UiRefreshingIsNeeded = true;
                }
            }
        }

        public static void SetStatusbarText(string newMessage, bool autoClear)
        {
            if (LastMessage != newMessage)
            {
                MbApiInterface.MB_SetBackgroundTaskMessage(newMessage);

                if (autoClear)
                {
                    LastMessage = "";
                    new System.Threading.Timer(DelayedClearingStatusbarText, null, (int)RefreshUI_Delay.TotalMilliseconds * 2, 0);
                }
                else
                {
                    LastMessage = newMessage;
                }
            }
        }

        private static void DelayedClearingStatusbarText(object state)
        {
            if (LastMessage == "")
                MbApiInterface.MB_SetBackgroundTaskMessage(LastMessage);
        }

        public static string RemoveMSIdAtTheEndOfString(string value)
        {
            value += EndOfStringId;
            value = value.Replace("" + MultipleItemsSplitterId + EndOfStringId, "");
            value = value.Replace("" + EndOfStringId, "");

            return value;
        }

        public static string ReplaceMSChars(string value)
        {
            value = value.Replace(SavedSettings.multipleItemsSplitterChar2, "" + MultipleItemsSplitterId);
            value = value.Replace(SavedSettings.multipleItemsSplitterChar1, "" + MultipleItemsSplitterId);

            return value;
        }

        public static string ReplaceMSIds(string value)
        {
            value = value.Replace("" + MultipleItemsSplitterId, SavedSettings.multipleItemsSplitterChar2);

            return value;
        }

        public static string RemoveRoleIds(string value)
        {
            value = value.Replace("" + GuestId, "");
            value = value.Replace("" + PerformerId, "");
            value = value.Replace("" + RemixerId, "");

            return value;
        }

        public static MetaDataType GetTagId(string tagName)
        {
            if (tagName == null)
                tagName = "";

            if (tagName.Length > 1 && (tagName[0] == '★' || tagName[0] == '☆' || tagName[0] == '♺' || tagName[0] == '❏'))
                tagName = tagName.Substring(2);

            if (tagName == DateCreatedTagName)
                return DateCreatedTagId;

            if (tagName == FolderTagName)
                return FolderTagId;

            if (tagName == FileNameTagName)
                return FileNameTagId;

            if (tagName == FilePathTagName)
                return FilePathTagId;

            if (TagNamesIds.TryGetValue(tagName, out MetaDataType tagId))
            {
                return tagId;
            }
            else
            {
                if (PropNamesIds.TryGetValue(tagName, out FilePropertyType propId))
                {
                    return (MetaDataType)propId;
                }
                else
                {
                    return 0;
                }
            }
        }

        public static string GetTagName(MetaDataType tagId)
        {
            if (tagId == DateCreatedTagId)
                return DateCreatedTagName;

            if (tagId == FolderTagId)
                return FolderTagName;

            if (tagId == FileNameTagId)
                return FileNameTagName;

            if (tagId == FilePathTagId)
                return FilePathTagName;

            if (TagIdsNames.TryGetValue(tagId, out string tagName))
            {
                return tagName;
            }
            else if (PropIdsNames.TryGetValue((FilePropertyType)tagId, out tagName))
            {
                return tagName;
            }
            else
            {
                return "";
            }
        }

        public static void FillList(System.Collections.IList list, bool addReadOnlyTagsAlso = false, bool addArtworkAlso = false, bool addNullAlso = true, bool addTagMarkers = false)
        {
            foreach (string element in TagNamesIds.Keys)
            {
                string marker = "";

                if (addTagMarkers)
                {
                    MetaDataType id = TagNamesIds[element];

                    switch (id)
                    {
                        case MetaDataType.Custom1:
                        case MetaDataType.Custom2:
                        case MetaDataType.Custom3:
                        case MetaDataType.Custom4:
                        case MetaDataType.Custom5:
                        case MetaDataType.Custom6:
                        case MetaDataType.Custom7:
                        case MetaDataType.Custom8:
                        case MetaDataType.Custom9:
                        case MetaDataType.Custom10:
                        case MetaDataType.Custom11:
                        case MetaDataType.Custom12:
                        case MetaDataType.Custom13:
                        case MetaDataType.Custom14:
                        case MetaDataType.Custom15:
                        case MetaDataType.Custom16:

                            marker = "★";
                            break;

                        case MetaDataType.Virtual1:
                        case MetaDataType.Virtual2:
                        case MetaDataType.Virtual3:
                        case MetaDataType.Virtual4:
                        case MetaDataType.Virtual5:
                        case MetaDataType.Virtual6:
                        case MetaDataType.Virtual7:
                        case MetaDataType.Virtual8:
                        case MetaDataType.Virtual9:
                        case MetaDataType.Virtual10:
                        case MetaDataType.Virtual11:
                        case MetaDataType.Virtual12:
                        case MetaDataType.Virtual13:
                        case MetaDataType.Virtual14:
                        case MetaDataType.Virtual15:
                        case MetaDataType.Virtual16:
                        case MetaDataType.Virtual17:
                        case MetaDataType.Virtual18:
                        case MetaDataType.Virtual19:
                        case MetaDataType.Virtual20:
                        case MetaDataType.Virtual21:
                        case MetaDataType.Virtual22:
                        case MetaDataType.Virtual23:
                        case MetaDataType.Virtual24:
                        case MetaDataType.Virtual25:
                        case MetaDataType.Virtual26:
                        case MetaDataType.Virtual27:
                        case MetaDataType.Virtual28:
                        case MetaDataType.Virtual29:
                        case MetaDataType.Virtual30:
                        case MetaDataType.Virtual31:
                        case MetaDataType.Virtual32:

                            marker = "♺";
                            break;

                        default:

                            if (ChangeCaseCommand.IsItemContainedInList(element, ReadonlyTagsNames))
                                marker = "✪";
                            else
                                marker = "☆";
                            break;
                    }
                    
                    marker = "" + marker + " ";
                }

                if (element == ArtworkName)
                {
                    if (addArtworkAlso)
                        list.Add(marker + element);
                }
                else
                {
                    if (addNullAlso || element != NullTagName)
                        if (addReadOnlyTagsAlso || !ChangeCaseCommand.IsItemContainedInList(element, ReadonlyTagsNames))
                            list.Add(marker + element);
                }
            }
        }

        public static FilePropertyType GetPropId(string propName)
        {
            if (PropNamesIds.TryGetValue(propName, out FilePropertyType propId))
            {
                return propId;
            }
            else
            {
                return 0;
            }
        }

        public static string GetPropName(FilePropertyType propId)
        {
            if (PropIdsNames.TryGetValue(propId, out string propName))
            {
                return propName;
            }
            else
            {
                return "";
            }
        }

        public static void FillListWithProps(System.Collections.IList list, bool addTagMarker = false)
        {
            string marker = "";

            if (addTagMarker)
                marker = "❏ ";

            foreach (string element in PropNamesIds.Keys)
            {
                list.Add(marker + element);
            };
        }

        public static void InitializeSbText()
        {
            LastCommandSbText = "";
            LastPreview = true;
            LastFileCounter = 0;

        }

        public static void SetResultingSbText()
        {
            if (LastCommandSbText == "")
            {
                SetStatusbarText("", false);
                return;
            }
            else if (LastCommandSbText == "<CUESHEET>")
            {
                System.Media.SystemSounds.Exclamation.Play();
                SetStatusbarText(CtlWholeCuesheetWillBeReencoded, false);
                return;
            }

            string sbText;

            if (LastPreview)
                sbText = LastCommandSbText + ": " + "100% (" + LastFileCounter + " " + SbFiles + ") " + SbRead;
            else
                sbText = LastCommandSbText + ": " + "100% (" + LastFileCounter + " " + SbFiles + ") " + SbUpdated;

            //if (lastPreview)
            //    sbText = LastCommandSbText + ": 100% " + sbRead;
            //else
            //    sbText = LastCommandSbText + ": 100% " + sbUpdated;

            SetStatusbarText(sbText, false);
        }

        public static void SetStatusbarTextForFileOperations(string commandSbText, bool preview, int fileCounter, int filesTotal, string currentFile = null, bool immediateDisplaying = false)
        {
            string sbText;

            LastCommandSbText = commandSbText;
            LastPreview = preview;
            LastFileCounter = fileCounter + 1;

            fileCounter++;

            if (immediateDisplaying || fileCounter % StatusbarTextUpdateInterval == 0)
            {
                if (preview)
                    sbText = commandSbText + " (" + SbReading + "): " + Math.Round((double)100 * LastFileCounter / filesTotal, 0) + "%";
                else
                    sbText = commandSbText + " (" + SbUpdating + "): " + Math.Round((double)100 * LastFileCounter / filesTotal, 0) + "%";

                if (currentFile != null)
                    sbText += " (" + currentFile + ")";

                SetStatusbarText(sbText, false);
            }
        }

        private static void regularUI_Refresh(object state)
        {
            if (UiRefreshingIsNeeded)
            {
                lock (LastUI_RefreshLocker)
                {
                    LastUI_Refresh = DateTime.Now;
                }

                UiRefreshingIsNeeded = false;
                MbApiInterface.MB_RefreshPanels();
                MbApiInterface.MB_SetBackgroundTaskMessage(LastMessage);
            }
        }

        public void regularAutobackup(object state)
        {
            MbApiInterface.MB_SetBackgroundTaskMessage(SbAutobackuping);
            BackupIndex.saveBackup(GetAutobackupDirectory(SavedSettings.autobackupDirectory) + @"\" + GetDefaultBackupFilename(SavedSettings.autobackupPrefix), SbAutobackuping, true);


            decimal autodeleteKeepNumberOfDays = SavedSettings.autodeleteKeepNumberOfDays > 0 ? SavedSettings.autodeleteKeepNumberOfDays : 1000000;
            decimal autodeleteKeepNumberOfFiles = SavedSettings.autodeleteKeepNumberOfFiles > 0 ? SavedSettings.autodeleteKeepNumberOfFiles : 1000000;

            string[] files = Directory.GetFiles(GetAutobackupDirectory(SavedSettings.autobackupDirectory), "*.mbc", SearchOption.TopDirectoryOnly);
            SortedDictionary<int, string> filesWithNegativeDates = new SortedDictionary<int, string>();
            SortedDictionary<string, DateTime> fileCreationDates = new SortedDictionary<string, DateTime>();

            BackupCacheType backupCache = new BackupCacheType();
            string currentLibraryName = GetLibraryName();

            for (int i = 0; i < files.Length; i++)
            {
                string backupFilenameWithoutExtension = GetBackupFilenameWithoutExtension(files[i]);

                backupCache = BackupCacheType.Load(backupFilenameWithoutExtension);

                if (backupCache.isAutocreated && backupCache.libraryName == currentLibraryName)
                {
                    int negativeDate = -((backupCache.creationDate.Year * 10000 + backupCache.creationDate.Month * 100 + backupCache.creationDate.Day) * 1000000 +
                        backupCache.creationDate.Hour * 10000 + backupCache.creationDate.Minute * 100 + backupCache.creationDate.Second);

                    filesWithNegativeDates.Add(negativeDate, backupFilenameWithoutExtension);
                    fileCreationDates.Add(backupFilenameWithoutExtension, backupCache.creationDate);
                }
            }


            List<string> backupsToDelete = new List<string>();
            DateTime now = DateTime.Now;

            foreach (var fileWithNegativeDate in filesWithNegativeDates)
            {
                if (now - fileCreationDates[fileWithNegativeDate.Value] > new TimeSpan((int)autodeleteKeepNumberOfDays, 0, 0, 0))
                    backupsToDelete.Add(fileWithNegativeDate.Value);
            }


            int keptCount = 1;
            foreach (var fileWithNegativeDate in filesWithNegativeDates)
            {
                if (keptCount > autodeleteKeepNumberOfFiles && !backupsToDelete.Contains(fileWithNegativeDate.Value))
                    backupsToDelete.Add(fileWithNegativeDate.Value);
                else
                    keptCount++;
            }


            foreach (string backup in backupsToDelete)
            {
                backupCache = BackupCacheType.Load(backup);

                BackupIndex.deleteBackup(backupCache);

                File.Delete(backup + ".xml");
                File.Delete(backup + ".mbc");
            }

            MbApiInterface.MB_SetBackgroundTaskMessage("");
        }
        #endregion

        #region Menu handlers
        public void copyTagEventHandler(object sender, EventArgs e)
        {
            CopyTagCommand tagToolsForm = new CopyTagCommand(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        public void swapTagsEventHandler(object sender, EventArgs e)
        {
            SwapTagsCommand tagToolsForm = new SwapTagsCommand(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        public void changeCaseEventHandler(object sender, EventArgs e)
        {
            ChangeCaseCommand tagToolsForm = new ChangeCaseCommand(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        public void reencodeTagEventHandler(object sender, EventArgs e)
        {
            ReencodeTagPlugin tagToolsForm = new ReencodeTagPlugin(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        public void reencodeTagsEventHandler(object sender, EventArgs e)
        {
            ReencodeTagsCommand tagToolsForm = new ReencodeTagsCommand(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        public void libraryReportsEventHandler(object sender, EventArgs e)
        {
            LibraryReportsCommand tagToolsForm = new LibraryReportsCommand(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        public void autoLibraryReportsEventHandler(object sender, EventArgs e)
        {
            AutoLibraryReportCommand tagToolsForm = new AutoLibraryReportCommand(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        public void autoRateEventHandler(object sender, EventArgs e)
        {
            AutoRateCommand tagToolsForm = new AutoRateCommand(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        public void asrEventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplaceCommand tagToolsForm = new AdvancedSearchAndReplaceCommand(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        public void carEventHandler(object sender, EventArgs e)
        {
            CalculateAverageAlbumRatingCommand tagToolsForm = new CalculateAverageAlbumRatingCommand(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        public void copyTagsToClipboardEventHandler(object sender, EventArgs e)
        {
            CopyTagsToClipboardCommand tagToolsForm = new CopyTagsToClipboardCommand(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        public void pasteTagsFromClipboardEventHandler(object sender, EventArgs e)
        {
            PasteTagsFromClipboardCommand tagToolsForm = new PasteTagsFromClipboardCommand(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        public void multipleSearchReplaceEventHandler(object sender, EventArgs e)
        {
            MultipleSearchAndReplaceCommand tagToolsForm = new MultipleSearchAndReplaceCommand(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        public void showHiddenEventHandler(object sender, EventArgs e)
        {
            lock (OpenedForms)
            {
                foreach (PluginWindowTemplate form in OpenedForms)
                {
                    if (!form.Visible)
                        form.Visible = true;
                }
            }
        }

        public void backupTagsEventHandler(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Title = CtlSaveBackupTitle,
                Filter = CtlMusicBeeBackupType,
                InitialDirectory = GetAutobackupDirectory(SavedSettings.autobackupDirectory),
                FileName = GetDefaultBackupFilename(BackupDefaultPrefix)
            };

            if (dialog.ShowDialog() == DialogResult.Cancel) return;

            MbApiInterface.MB_SetBackgroundTaskMessage(SbMakingTagBackup);

            if (File.Exists(dialog.FileName))
                File.Delete(dialog.FileName);
            if (File.Exists(GetBackupFilenameWithoutExtension(dialog.FileName) + ".mbc"))
                File.Delete(GetBackupFilenameWithoutExtension(dialog.FileName) + ".mbc");

            MbApiInterface.MB_CreateParameterisedBackgroundTask(BackupIndex.saveBackupAsync, new object[] { GetBackupFilenameWithoutExtension(dialog.FileName), SbMakingTagBackup, false }, MbForm);
            dialog.Dispose();
        }

        public void restoreTagsEventHandler(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = CtlRestoreBackupTitle,
                Filter = CtlMusicBeeBackupType,
                InitialDirectory = GetAutobackupDirectory(SavedSettings.autobackupDirectory)
            };

            if (dialog.ShowDialog() == DialogResult.Cancel) return;

            MbApiInterface.MB_SetBackgroundTaskMessage(SbRestoringTagsFromBackup);

            MbApiInterface.MB_CreateParameterisedBackgroundTask(BackupIndex.LoadBackupAsync, new object[] { GetBackupFilenameWithoutExtension(dialog.FileName), SbRestoringTagsFromBackup, false, this }, MbForm);
            dialog.Dispose();
        }

        public void restoreTagsForEntireLibraryEventHandler(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = CtlRestoreBackupTitle,
                Filter = CtlMusicBeeBackupType,
                InitialDirectory = GetAutobackupDirectory(SavedSettings.autobackupDirectory)
            };

            if (dialog.ShowDialog() == DialogResult.Cancel) return;

            MbApiInterface.MB_SetBackgroundTaskMessage(SbRestoringTagsFromBackup);

            MbApiInterface.MB_CreateParameterisedBackgroundTask(BackupIndex.LoadBackupAsync, new object[] { GetBackupFilenameWithoutExtension(dialog.FileName), SbRestoringTagsFromBackup, true, this }, MbForm);
            dialog.Dispose();
        }

        public void renameMoveBackupEventHandler(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Title = CtlRenameSelectBackupTitle,
                Filter = CtlMusicBeeBackupType,
                InitialDirectory = GetAutobackupDirectory(SavedSettings.autobackupDirectory)
            };

            if (openDialog.ShowDialog() == DialogResult.Cancel) return;

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Title = CtlRenameSaveBackupTitle,
                Filter = CtlMusicBeeBackupType,
                InitialDirectory = GetAutobackupDirectory(SavedSettings.autobackupDirectory),
                FileName = openDialog.SafeFileName
            };

            if (saveDialog.ShowDialog() == DialogResult.Cancel) return;

            if (openDialog.FileName == saveDialog.FileName) return;

            MbApiInterface.MB_SetBackgroundTaskMessage(SbRenamingMovingBackup);

            if (File.Exists(saveDialog.FileName))
                File.Delete(saveDialog.FileName);
            if (File.Exists(GetBackupFilenameWithoutExtension(saveDialog.FileName) + ".mbc"))
                File.Delete(GetBackupFilenameWithoutExtension(saveDialog.FileName) + ".mbc");

            File.Move(openDialog.FileName, saveDialog.FileName);
            File.Move(GetBackupFilenameWithoutExtension(openDialog.FileName) + ".mbc", GetBackupFilenameWithoutExtension(saveDialog.FileName) + ".mbc");

            MbApiInterface.MB_SetBackgroundTaskMessage("");
            openDialog.Dispose();
            saveDialog.Dispose();
        }

        public void moveBackupsEventHandler(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Title = CtlMoveSelectBackupsTitle,
                Filter = CtlMusicBeeBackupType,
                InitialDirectory = GetAutobackupDirectory(SavedSettings.autobackupDirectory),
                Multiselect = true
            };

            if (openDialog.ShowDialog() == DialogResult.Cancel) return;

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Title = CtlMoveSaveBackupsTitle,
                Filter = "|*.destinationfolder",
                //saveDialog.InitialDirectory = GetAutobackupDirectory(SavedSettings.autobackupDirectory);
                FileName = CtlSelectThisFolder
            };

            if (saveDialog.ShowDialog() == DialogResult.Cancel) return;

            string sourceFolder = Regex.Replace(openDialog.FileNames[0], @"(.*)\\.*", "$1");
            string destinationFolder = Regex.Replace(saveDialog.FileName, @"(.*)\\.*", "$1");

            if (sourceFolder == destinationFolder) return;

            MbApiInterface.MB_SetBackgroundTaskMessage(SbMovingBackups);

            foreach (var filename in openDialog.SafeFileNames)
            {
                if (File.Exists(destinationFolder + @"\" + filename))
                    File.Delete(destinationFolder + @"\" + filename);
                if (File.Exists(GetBackupFilenameWithoutExtension(destinationFolder + @"\" + filename) + ".mbc"))
                    File.Delete(GetBackupFilenameWithoutExtension(destinationFolder + @"\" + filename) + ".mbc");

                File.Move(sourceFolder + @"\" + filename, destinationFolder + @"\" + filename);
                File.Move(GetBackupFilenameWithoutExtension(sourceFolder + @"\" + filename) + ".mbc", GetBackupFilenameWithoutExtension(destinationFolder + @"\" + filename) + ".mbc");
            }

            MbApiInterface.MB_SetBackgroundTaskMessage("");
            openDialog.Dispose();
            saveDialog.Dispose();
        }

        public void createNewBaselineEventHandler(object sender, EventArgs e)
        {
            MessageBox.Show(MbForm, MsgCreateBaselineWarning, CtlWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);


            SaveFileDialog dialog = new SaveFileDialog
            {
                Title = CtlSaveBackupTitle,
                Filter = CtlMusicBeeBackupType,
                InitialDirectory = GetAutobackupDirectory(SavedSettings.autobackupDirectory),
                FileName = GetDefaultBackupFilename(BackupDefaultPrefix)
            };

            if (dialog.ShowDialog() == DialogResult.Cancel) return;


            lock (AutobackupLocker)
            {
                //Lets delete all incremental backups of current library
                string currentLibraryName = GetLibraryName();
                string[] files = Directory.GetFiles(GetAutobackupDirectory(SavedSettings.autobackupDirectory), "*.mbc", SearchOption.TopDirectoryOnly);

                BackupCacheType backupCache = new BackupCacheType();

                for (int i = 0; i < files.Length; i++)
                {
                    string backupFilenameWithoutExtension = GetBackupFilenameWithoutExtension(files[i]);

                    backupCache = BackupCacheType.Load(backupFilenameWithoutExtension);

                    if (backupCache.libraryName == currentLibraryName)
                    {
                        BackupIndex.deleteBackup(backupCache);

                        File.Delete(backupFilenameWithoutExtension + ".mbc");
                        File.Delete(backupFilenameWithoutExtension + ".xml");
                    }
                }

                //Lets delete baseline
                File.Delete(GetBackupBaselineFilename());
            }


            //Now lets create new baseline
            MbApiInterface.MB_SetBackgroundTaskMessage(SbMakingTagBackup);

            if (File.Exists(dialog.FileName))
                File.Delete(dialog.FileName);
            if (File.Exists(GetBackupFilenameWithoutExtension(dialog.FileName) + ".mbc"))
                File.Delete(GetBackupFilenameWithoutExtension(dialog.FileName) + ".mbc");

            MbApiInterface.MB_CreateParameterisedBackgroundTask(BackupIndex.saveBackupAsync, new object[] { GetBackupFilenameWithoutExtension(dialog.FileName), SbMakingTagBackup, false }, MbForm);
            dialog.Dispose();
        }

        public void deleteBackupsEventHandler(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = CtlDeleteBackupsTitle,
                Filter = CtlMusicBeeBackupType,
                InitialDirectory = GetAutobackupDirectory(SavedSettings.autobackupDirectory),
                Multiselect = true
            };

            if (dialog.ShowDialog() == DialogResult.Cancel) return;

            MbApiInterface.MB_SetBackgroundTaskMessage(SbDeletingBackups);

            BackupCacheType backupCache = new BackupCacheType();
            foreach (var filename in dialog.FileNames)
            {
                backupCache = BackupCacheType.Load(GetBackupFilenameWithoutExtension(filename));

                BackupIndex.deleteBackup(backupCache);

                File.Delete(filename);
                File.Delete(GetBackupFilenameWithoutExtension(filename) + ".mbc");
            }

            MbApiInterface.MB_SetBackgroundTaskMessage("");
            dialog.Dispose();
        }

        public void autoBackupSettingsEventHandler(object sender, EventArgs e)
        {
            AutoBackupSettings tagToolsForm = new AutoBackupSettings(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        public void tagHistoryEventHandler(object sender, EventArgs e)
        {
            if (MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out string[] files))
            {
                //if (files.Length > 1)
                //{
                //    MessageBox.Show(Plugin.MbForm, msgSelectOneTrackOnly);
                //    return;
                //}

                if (files.Length == 0)
                {
                    MessageBox.Show(MbForm, MsgSelectTrack, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                int[] trackIds = new int[files.Length];
                for (int i = 0; i < files.Length; i++)
                    trackIds[i] = int.Parse(GetPersistentTrackId(files[i]));


                TagHistoryCommand tagToolsForm = new TagHistoryCommand(this, files, trackIds);
                PluginWindowTemplate.Display(tagToolsForm, true);
            }
        }

        public void compareTracksEventHandler(object sender, EventArgs e)
        {

            if (MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out string[] files))
            {
                if (files.Length < 2)
                {
                    MessageBox.Show(MbForm, MsgSelectAtLeast2Tracks, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }


                CompareTracksCommand tagToolsForm = new CompareTracksCommand(this, files);
                PluginWindowTemplate.Display(tagToolsForm, true);
            }
        }
        #endregion

        #region Main plugin initialization
        public PluginInfo Initialise(IntPtr apiInterfacePtr)
        {
            #region General initialization
            MbApiInterface = new MusicBeeApiInterface();
            MbApiInterface.Initialise(apiInterfacePtr);
            #endregion

            #region English localization
            //Localizable strings

            //Plugin localizable strings
            PluginName = "Additional Tagging & Reporting Tools";
            PluginMenuGroup = "Additional Tagging && Reporting Tools";
            Description = "Adds some tagging & reporting tools to MusicBee";

            TagToolsMenuSectionName = "TAGGING && REPORTING";
            BackupRestoreMenuSectionName = "BACKUP && RESTORE";

            CopyTagCommandName = "Copy Tag...";
            SwapTagsCommandName = "Swap Tags...";
            ChangeCaseCommandName = "Change Case...";
            ReencodeTagCommandName = "Reencode Tag...";
            ReencodeTagsCommandName = "Reencode Tags...";
            LibraryReportsCommandName = "Library Reports...";
            AutoLibraryReportsCommandName = "Auto Library Reports...";
            AutoRateCommandName = "Auto Rate Tracks...";
            AsrCommandName = "Advanced Search && Replace...";
            CarCommandName = "Calculate Average Album Rating...";
            CtCommandName = "Compare Tracks...";
            CopyTagsToClipboardCommandName = "Copy Tags to Clipboard...";
            PasteTagsFromClipboardCommandName = "Paste Tags from Clipboard";
            MsrCommandName = "Multiple Search && Replace...";
            ShowHiddenCommandName = "Show hidden plugin windows";

            TagToolsHotkeyDescription = "Tagging Tools: ";
            CopyTagCommandDescription = TagToolsHotkeyDescription + "Copy Tag";
            SwapTagsCommandDescription = TagToolsHotkeyDescription + "Swap Tags";
            ChangeCaseCommandDescription = TagToolsHotkeyDescription + "Change Case";
            ReencodeTagCommandDescription = TagToolsHotkeyDescription + "Reencode Tag";
            ReencodeTagsCommandDescription = TagToolsHotkeyDescription + "Reencode Tags";
            LibraryReportsCommandDescription = TagToolsHotkeyDescription + "Library Reports";
            AutoLibraryReportsCommandDescription = TagToolsHotkeyDescription + "Auto Library Reports";
            AutoRateCommandDescription = TagToolsHotkeyDescription + "Auto Rate Tracks";
            AsrCommandDescription = TagToolsHotkeyDescription + "Advanced Search & Replace";
            AsrHotkeyDescription = TagToolsHotkeyDescription + "⌕: ";
            CarCommandDescription = TagToolsHotkeyDescription + "Calculate Average Album Rating";
            CtCommandDescription = TagToolsHotkeyDescription + "Compare Tracks";
            CopyTagsToClipboardCommandDescription = TagToolsHotkeyDescription + "Copy Tags to Clipboard";
            PasteTagsFromClipboardCommandDescription = TagToolsHotkeyDescription + "Paste Tags from Clipboard";
            CopyTagsToClipboardUsingMenuDescription = "Copy Tags to Clipboard Using Tag Set";
            MsrCommandDescription = TagToolsHotkeyDescription + "Multiple Search & Replace";
            ShowHiddenCommandDescription = TagToolsHotkeyDescription + "Show Hidden Plugin Windows";

            BackupTagsCommandName = "Backup Tags for All Tracks...";
            RestoreTagsCommandName = "Restore Tags for Selected Tracks...";
            RestoreTagsForEntireLibraryCommandName = "Restore Tags for All Tracks...";
            RenameMoveBackupCommandName = "Rename or Move Backup...";
            MoveBackupsCommandName = "Move Backup(s)...";
            CreateNewBaselineCommandName = "Create New Baseline of Current Library...";
            DeleteBackupsCommandName = "Delete Backup(s)...";
            AutoBackupSettingsCommandName = "(Auto)backup Settings...";

            TagHistoryCommandName = "Tag History....";

            BackupTagsCommandDescription = TagToolsHotkeyDescription + "Backup Tags for All Tracks";
            RestoreTagsCommandDescription = TagToolsHotkeyDescription + "Restore Tags for Selected Tracks";
            RestoreTagsForEntireLibraryCommandDescription = TagToolsHotkeyDescription + "Restore Tags for All Tracks";
            RenameMoveBackupCommandDescription = TagToolsHotkeyDescription + "Rename or Move Backup";
            MoveBackupsCommandDescription = TagToolsHotkeyDescription + "Move Backup(s)";
            CreateNewBaselineCommandDescription = TagToolsHotkeyDescription + "Create New Baseline of Current Library";
            DeleteBackupsCommandDescription = TagToolsHotkeyDescription + "Delete Backup(s)";
            AutoBackupSettingsCommandDescription = TagToolsHotkeyDescription + "(Auto)backup Settings";

            TagHistoryCommandDescription = TagToolsHotkeyDescription + "Tag History";

            CopyTagCommandSbText = "Copying tag";
            SwapTagsCommandSbText = "Swapping tags";
            ChangeCaseCommandSbText = "Changing case";
            ReencodeTagCommandSbText = "Reencoding tags";
            LibraryReportsCommandSbText = "Generating report";
            LibraryReportsGneratingPreviewCommandSbText = "Generating preview";
            AutoRateCommandSbText = "Auto rating tracks";
            AsrCommandSbText = "Advanced searching and replacing";
            CarCommandSbText = "Calculating average album rating";
            MsrCommandSbText = "Multiple searching and replacing";

            //Other localizable strings
            AlbumTagName = MbApiInterface.Setting_GetFieldName(MetaDataType.Album);
            Custom9TagName = MbApiInterface.Setting_GetFieldName(MetaDataType.Custom9);
            UrlTagName = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);
            GenreCategoryName = MbApiInterface.Setting_GetFieldName(MetaDataType.GenreCategory);

            GroupingName = "<Grouping>";
            CountName = "Count";
            SumName = "Sum";
            MinimumName = "Minimum";
            MaximumName = "Maximum";
            AverageName = "Average value";
            AverageCountName = "Average count";

            LibraryReport = "Library report";


            DateCreatedTagName = "<Date Created>";
            EmptyValueTagName = "<Empty Value>";
            ClipboardTagName = "<Clipboard>";
            TextFileTagName = "<Text File>";

            FolderTagName = "<Folder>";
            FileNameTagName = "<Filename>";
            FilePathTagName = "<File Path>";
            AlbumUniqueIdName = "<Album Unique Id>";

            ParameterTagName = "Tag";
            TempTagName = "Temp";

            CustomTagsPresetName = "<Unsaved tags>";
            LibraryTotalsPresetName = "Library totals";
            LibraryAveragesPresetName = "Library averages";
            CDBookletPresetName = "CD Booklet";
            AlbumsAndTracksPresetName = "Albums & Tracks";

            EmptyPresetName = "<Empty preset>";


            //Let's determine casing rules for genre category as an example
            ChangeCaseCommand.ChangeCaseOptions changeCaseMode = ChangeCaseCommand.ChangeCaseOptions.sentenceCase;
            string[] genreCategory = MbApiInterface.Setting_GetFieldName(MetaDataType.GenreCategory).Split(' ');
            if (char.ToUpper(genreCategory[genreCategory.Length - 1][0]) == genreCategory[genreCategory.Length - 1][0])
                changeCaseMode = ChangeCaseCommand.ChangeCaseOptions.titleCase;

            ArtistArtistsName = MbApiInterface.Setting_GetFieldName(MetaDataType.Artist);
            ComposerComposersName = MbApiInterface.Setting_GetFieldName(MetaDataType.Composer);

            DisplayedArtistName = MbApiInterface.MB_GetLocalisation("aSplit.msg.diar", "display {0}:").Replace("{0}", ArtistArtistsName);
            DisplayedArtistName = ChangeCaseCommand.ChangeWordsCase(DisplayedArtistName, changeCaseMode, 
                null, false, null, null);
            DisplayedArtistName = DisplayedArtistName.Remove(DisplayedArtistName.Length - 1);

            DisplayedComposerName = MbApiInterface.MB_GetLocalisation("aSplit.msg.diar", "display {0}:").Replace("{0}", ComposerComposersName);
            DisplayedComposerName = ChangeCaseCommand.ChangeWordsCase(DisplayedComposerName, changeCaseMode, 
                null, false, null, null);
            DisplayedComposerName = DisplayedComposerName.Remove(DisplayedComposerName.Length - 1);

            DisplayedAlbumArtsistName = MbApiInterface.MB_GetLocalisation("aSplit.msg.diar", "display {0}:").Replace("{0}", MbApiInterface.Setting_GetFieldName(MetaDataType.AlbumArtist));
            DisplayedAlbumArtsistName = ChangeCaseCommand.ChangeWordsCase(DisplayedAlbumArtsistName, changeCaseMode, 
                null, false, null, null);
            DisplayedAlbumArtsistName = DisplayedAlbumArtsistName.Remove(DisplayedAlbumArtsistName.Length - 1);


            ArtworkName = MbApiInterface.Setting_GetFieldName(MetaDataType.Artwork);

            LyricsName = MbApiInterface.Setting_GetFieldName(MetaDataType.Lyrics);
            LyricsNamePostfix = MbApiInterface.MB_GetLocalisation("Main.msg.Y[s", "Y [synched/unsynched]").Substring(1);
            SynchronisedLyricsName = LyricsName + MbApiInterface.MB_GetLocalisation("Main.msg.Y[s.2", "Y [synched]").Substring(1);
            UnsynchronisedLyricsName = LyricsName + MbApiInterface.MB_GetLocalisation("Main.msg.Y[u", "Y [unsynched]").Substring(1);


            NullTagName = "<Null>";

            GenericTagSetName = "Tag set";

            //Supported exported file formats
            ExportedFormats = "HTML Document (grouped by albums)|*.htm|HTML Document|*.htm|Simple HTML table|*.htm|Tab delimited text|*.txt|M3U Playlist|*.m3u|CSV file|*.csv|HTML Document (CD Booklet)|*.htm";
            string exportedTrackList = "Exported Track List";


            //Displayed text
            SequenceNumberName = "#";

            ListItemConditionIs = "is";
            ListItemConditionIsNot = "is not";
            ListItemConditionIsGreater = "is greater than";
            ListItemConditionIsLess = "is less than";

            OKButtonName = "Procceed";
            StopButtonName = "Stop";
            CancelButtonName = "Cancel";
            HideButtonName = "Hide";
            PreviewButtonName = "Preview";
            ClearButtonName = "Clear";
            FindButtonName = "Find";
            SelectFoundButtonName = "Select found";
            OriginalTagHeaderTextTagValue = "Tag value";
            OriginalTagHeaderText = "Original tag";
            
            TableCellError = "#Error!";

            DefaultASRPresetName = "Preset #";

            MsgFileNotFound = "File not found!";
            MsgNoFilesSelected = "No files selected.";
            MsgNoFilesDisplayed = "No files displayed.";
            MsgSourceAndDestinationTagsAreTheSame = "Both tags are the same. Nothing done.";
            MsgSwapTagsSourceAndDestinationTagsAreTheSame = "Using the same " +
                "source and destination tags may be useful only for 'Artist'/'Composer' tags for conversion of ';' delimited tag to the list of artists/composers and vice versa. Nothing done.";
            MsgNoTagsSelected = "No tags selected. Nothing to preview.";
            MsgNoFilesInCurrentView = "No files in current view.";
            MsgTracklistIsEmpty = "Track list is empty. Nothing to export. Click 'Preview' first.";
            MsgForExportingPlaylistsURLfieldMustBeIncludedInTagList = "For exporting playlists '" + UrlTagName + "' field must be included in tag list.";
            MsgPreviewIsNotGeneratedNothingToSave = "Preview is not generated. Nothing to save.";
            MsgPreviewIsNotGeneratedNothingToChange = "Preview is not generated. Nothing to change.";
            MsgNoAggregateFunctionNothingToSave = "No aggregate function in the table. Nothing to save.";
            MsgPleaseUseGroupingFunctionForArtworkTag = "Please use <Grouping> function for artwork tag!";
            MsgAllTags = "ALL TAGS";
            MsgNoURLcolumnUnableToSave = "No '" + UrlTagName + "' tag in the table. Unable to save.";
            MsgEmptyURL = "Empty '" + UrlTagName + "' in row ";
            MsgUnableToSave = "Unable to save. ";
            MsgUsingEmptyValueAsSourceTagMayBeUsefulOnlyForAppendingStaticTextToDestinationtag = "Using '" + EmptyValueTagName +
                "' as source tag is useful only with option 'Append source tag to the end of destination tag' for adding some static text to the destination tag. Nothing done.";
            MsgBackgroundTaskIsCompleted = "Additional tagging tools background task is completed.";
            MsgThresholdsDescription = "Auto ratings are based on the average number of times track is played every day \n" +
                "or 'plays per day' virtual tag. You should set 'plays per day' threshold values for every rating number \n" +
                "you want to be assigned to tracks. Thresholds are evaluated for track in descending order: from 5 star rating \n" +
                "to 0.5 star rating. 'Plays per day' virtual tag can be evaluated for every track independent of rest of library, \n" +
                "so its possible to automatically update auto rating if playing track is changed. \n" +
                "Also its possible to update auto rating manually for selected files or automatically for entire library \n" +
                "on MusicBee startup. ";
            MsgAutoCalculationOfThresholdsDescription = "Auto calculation of thresholds is another way to set up auto ratings. This option allows you to set desirable weights \n" +
                "of auto rating values in your library. Actual weights may differ from desirable values because its not always possible \n" +
                "to satisfy desirable weights (suppose all your tracks have the same 'plays per day' value, its obvious that there is no way to split \n" +
                "your library into several parts on the basis of 'plays per day' value). Actual weights are displayed next to desired weights after calculation is made. \n" +
                "Because calculation of thresholds is time consuming operation it cannot be done automatically on any event except for MusicBee startup. ";

            MsgNumberOfPlayedTracks = "Ever played tracks in your library: ";
            MsgIncorrectSumOfWeights = "The sum of all weights must be equal or less than 100%!";
            MsgSum = "Sum: ";
            MsgNumberOfNotRatedTracks = "% of tracks rated as no stars)";
            MsgTracks = " tracks)";
            MsgActualPercent = "% / Act.: ";
            MsgIncorrectPresetName = "Incorrect preset name or duplicated preset names.";

            MsgDeletePresetConfirmation = "Do you want to delete selected preset?";
            MsgImportingConfirmation = "Do you want to install predefined presets?";
            MsgDoYouWantToUpdateYourCustomizedPredefinedPresets = "One or more predefined presets customized by you have been changed by developer.\n"
                + "Do you want to update your customized predefined presets by new versions from developer?\n\n"
                + "ALL YOUR CUSTOMIZATIONS WILL BE LOST!";
            MsgDoYouWantToImportExistingPresetsAsCopies = "One or more imported presets already exist.\n"
                + "Do you want to import them as new presets, and keep your current presets?\n\n"
                + "OTHERWISE, EXISTING PRESETS WILL BE OVERWRITTEN!";

            MsgPresetsWereImported = " preset{;s;s} {was;were;were} successfully imported.\n";
            MsgPresetsWereImportedAsCopies = " preset{;s;s} {was;were;were} successfully imported as new preset{;s;s}.\n";
            MsgPresetsFailedToImport = " preset{;s;s} failed to import due to file\n" + AddLeadingSpaces(0, 4, 0)  + " read error{;s;s} or wrong format.";

            MsgPresetsWereInstalled = " preset{;s;s} {was;were;were} successfully installed.\n";
            MsgPresetsWereUpdated = " presets {was;were;were} successfully updated.\n\n";
            MsgPresetsCustomizedWereUpdated = " preset{;s;s} customized by you {was;were;were} updated.\n";
            MsgPresetsWereNotChanged = " preset{;s;s} {was;were;were} not changed since\n" + AddLeadingSpaces(0, 4, 0)  + " last update, and skipped.\n\n";
            MsgPresetsWereSkipped = " preset{;s;s} {was;were;were} customized by you, and skipped.\n";
            MsgPresetsFailedToUpdate = " preset{;s;s} failed to install due to file\n" + AddLeadingSpaces(0, 4, 0)  + " read error{;s;s} or wrong format.";
            MsgPresetsNotFound = "No presets for installing found in expected directory!";

            MsgDeletingConfirmation = "Do you want to delete all predefined presets?";
            MsgNoPresetsDeleted = "No presets were deleted. ";
            MsgPresetsWereDeleted = " preset{;s;s} {was;were;were} successfully deleted.";
            MsgFailedToDelete = " preset{;s;s} failed to delete.";

            MsgNumberOfTagsInTextFile = "Number of tags in text file (";
            MsgDoesntCorrespondToNumberOfSelectedTracks = ") doesn't correspond to number of selected tracks (";
            MsgMessageEnd = ")!";

            MsgClipboardDesntContainText = "Clipboard doesn't contain text!";

            MsgNumberOfTagsInClipboard = "The number of tags in clipboard (";
            MsgNumberOfTracksInClipboard = "The number of tracks in clipboard (";
            MsgDoesntCorrespondToNumberOfSelectedTracksC = ") doesn't correspond to the number of selected tracks (";
            MsgDoesntCorrespondToNumberOfCopiedTagsC = ") doesn't correspond to the number of copied tags (";
            MsgMessageEndC = ")!";
            MsgDoYouWantToPasteAnyway = " Do you want to paste tags anyway?";

            MsgFirstThreeGroupingFieldsInPreviewTableShouldBe = "First three grouping fields in preview table should be '" + DisplayedAlbumArtsistName + "', '" + AlbumTagName + "' and '" + ArtworkName + "' to export to HTML Document (grouped by album)";
            MsgFirstSixGroupingFieldsInPreviewTableShouldBe = "First six grouping fields in preview table should be '" + SequenceNumberName 
                + "', '" + DisplayedAlbumArtsistName + "', '" + AlbumTagName + "', '" + ArtworkName + "', '" 
                + MbApiInterface.Setting_GetFieldName(MetaDataType.TrackTitle) + "' and '" 
                + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Duration) 
                + "' to export to HTML Document (CD Booklet)";

            MsgBackgroundAutoLibraryReportIsExecuted = "Background auto library report is executed! Please wait until it is finished!";

            MsgYouMustImportStandardASRPresetsFirst = "You must import standard ASR presets first!";

            SbSorting = "Sorting table...";
            SbUpdating = "updating";
            SbReading = "reading";
            SbUpdated = "updated";
            SbRead = "read";
            SbFiles = "file(s)";
            SbAsrPresetIsApplied = "ASR preset is applied: %PRESETNAME%";
            SbAsrPresetsAreApplied = "ASR presets are applied: %PRESETCOUNT%";

            CtlDirtyError1sf = "";
            CtlDirtyError1mf = "";
            CtlDirtyError2sf = " background file updating operation is running/scheduled. \n" +
                "Preview results may be not accurate. ";
            CtlDirtyError2mf = " background file updating operations is running/scheduled. \n" +
                "Preview results may be not accurate. ";


            MsgMasterBackupIndexIsCorrupted = "Master tag backup index is corrupted! All existing at the moment backups are not available any more in 'Tag history' command.";
            MsgBackupIsCorrupted = "Backup '%%FILENAME%%' is corrupted or is not valid MusicBee backup!";
            MsgFolderDoesntExists = "Folder doesn't exist!";
            MsgSelectOneTrackOnly = "Select one track only!";
            MsgSelectTrack = "Select a track!";
            MsgSelectAtLeast2Tracks = "Select at least 2 tracks to compare!";
            MsgBackupBaselineFileDoesntExist1 = "Backup baseline file '";
            MsgBackupBaselineFileDoesntExist2 = "' doesn't exist!";
            MsgThisIsTheBackupOfDifferentLibrary = "This is the backup of different library!";
            MsgCreateBaselineWarning = "When you create the first backup of given library, a backup baseline is created. All further backups are incremental relative to baseline. " +
                "If you have changed very much tags incremental backups may become too large. This command will delete ALL incremental backups of CURRENT library and will create new backup baseline if you continue. ";

            MsgGiveNameToASRpreset = "Give a name to preset!";
            MsgAreYouSureYouWantToSaveASRpreset = "Do you want to save ASR preset named \"%PRESETNAME%\"?";
            MsgAreYouSureYouWantToOverwriteASRpreset = "Do you want to overwrite ASR preset \"%PRESETNAME%\"?";
            MsgAreYouSureYouWantToOverwriteRenameASRpreset = "Do you want to overwrite ASR preset \"%PRESETNAME%\", and rename it to \"%NEWPRESETNAME%\"?";
            MsgAreYouSureYouWantToDeleteASRpreset = "Do you want to delete ASR preset \"%PRESETNAME%\"?";
            MsgPredefinedPresetsCantBeChanged = "Predefined presets can't be changed. Preset editor will open in read-only mode.\n\n"
                + "Do you want to disable this warning?";
            MsgSavePreset = "Save Preset";
            MsgDeletePreset = "Delete Preset";

            CtlNewASRPreset = "<New ASR Preset>";


            SbAutobackuping = "Autosaving tag backup...";
            SbMovingBackupsToNewFolder = "Moving backups to new folder...";
            SbMakingTagBackup = "Making tag backup...";
            SbRestoringTagsFromBackup = "Restoring tags from backup...";
            SbRenamingMovingBackup = "Renaming/moving backup...";
            SbMovingBackups = "Moving backups...";
            SbDeletingBackups = "Deleting backup(s)...";
            SbTagAutobackupSkipped = "Tag autobackup skipped (no changes since last tag backup)";
            SbCompairingTags = "Compairing tags with baseline... ";

            CtlWarning = "WARNING!";
            CtlMusicBeeBackupType = "MusicBee Tag Backup|*.xml";
            CtlSaveBackupTitle = "NAVIGATE TO DESIRED FOLDER, TYPE BACKUP NAME AT THE BOTTOM OF THE WINDOW AND CLICK 'SAVE'";
            CtlRestoreBackupTitle = "NAVIGATE TO DESIRED FOLDER, SELECT BACKUP AND CLICK 'OPEN'";
            CtlRenameSelectBackupTitle = "NAVIGATE TO DESIRED FOLDER, SELECT BACKUP TO BE RENAMED/MOVED AND CLICK 'OPEN'";
            CtlRenameSaveBackupTitle = "NAVIGATE TO DESIRED FOLDER, TYPE NEW BACKUP NAME AT THE BOTTOM OF THE WINDOW AND CLICK 'SAVE'";
            CtlMoveSelectBackupsTitle = "NAVIGATE TO DESIRED FOLDER, SELECT BACKUPS TO BE MOVED AND CLICK 'OPEN'";
            CtlMoveSaveBackupsTitle = "NAVIGATE TO DESTINATION FOLDER AND CLICK 'SAVE'";
            CtlDeleteBackupsTitle = "NAVIGATE TO DESIRED FOLDER, SELECT BACKUP(S) TO DELETE AND CLICK 'OPEN'";
            CtlSelectThisFolder = "[SELECT THIS FOLDER]";
            CtlNoBackupData = "<No backup data>";
            CtlNoDifferences = "<No differences>";

            MsgNotAllowedSymbols = "Only ASCII letters, numbers and symbols - : _ . are allowed!";
            MsgPresetExists = "Preset with id %ID% already exists!";

            CtlWholeCuesheetWillBeReencoded = "IMPORTANT NOTE: ONE OR SEVERAL TRACKS BELONG TO CUESHEET. THE ALL CUESHEET TRACKS WILL BE REENCODED!";

            CtlMSR = "MSR: ";

            CtlUnknown = MbApiInterface.MB_GetLocalisation("dSum.msg.Unknown", "Unknown");

            MsgFirstSelectWhichFieldYouWantToAssignFunctionIdTo = "First select, which field you want to assign function id to (leftmost combobox on function id line)!";

            //Defaults for controls
            UnchangedStyle.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
            UnchangedStyle.SelectionForeColor = Color.FromKnownColor(KnownColor.HighlightText);

            SavedSettings = new SavedSettingsType
            {

                //Lets set initial defaults
                libraryReportsPresets = null,

                smartOperation = true,
                appendSource = false,

                changeCaseFlag = 1,
                useExceptionWords = false,

                useExceptionChars = false,
                useWordSplitters = false,

                comparedField = "1"
            };
            #endregion

            #region Reading settings
            //Lets try to read defaults for controls from settings file
            PluginSettingsFilePath = Path.Combine(MbApiInterface.Setting_GetPersistentStoragePath(), PluginSettingsFileName);

            Encoding unicode = Encoding.UTF8;
            FileStream stream = File.Open(PluginSettingsFilePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
            StreamReader file = new StreamReader(stream, unicode);

            System.Xml.Serialization.XmlSerializer controlsDefaultsSerializer = null;
            try
            {
                controlsDefaultsSerializer = new System.Xml.Serialization.XmlSerializer(typeof(SavedSettingsType));
            }
            catch (Exception e)
            {
                MessageBox.Show(MbForm, e.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                SavedSettings = (SavedSettingsType)controlsDefaultsSerializer.Deserialize(file);
            }
            catch
            {
                //Ignore...
            };

            file.Close();
            #endregion


            #region Resetting invalid/absent settings
            if (SavedSettings == null)
                SavedSettings = new SavedSettingsType();

            if (SavedSettings.commandWindows == null)
                SavedSettings.commandWindows = new List<SizePositionType>();

            if (SavedSettings.exceptionWords == null || SavedSettings.exceptionWords.Length < 10)
            {
                SavedSettings.exceptionWords = new string[10];
                SavedSettings.exceptionWords[0] = "the a an and or not";
                SavedSettings.exceptionWords[1] = "a al an and as at but by de for in la le mix nor of on or remix the to vs. y ze feat.";
                SavedSettings.exceptionWords[2] = "U2 UB40";
                SavedSettings.exceptionWords[3] = "";
                SavedSettings.exceptionWords[4] = "";
                SavedSettings.exceptionWords[5] = "";
                SavedSettings.exceptionWords[6] = "";
                SavedSettings.exceptionWords[7] = "";
                SavedSettings.exceptionWords[8] = "";
                SavedSettings.exceptionWords[9] = "";
            }

            if (SavedSettings.exceptionWordsASR == null)
            {
                SavedSettings.exceptionWordsASR = "a al an and as at but by de for in la le mix nor of on or remix the to vs. y ze feat.";
            }

            if (SavedSettings.exceptionChars == null)
            {
                SavedSettings.exceptionChars = "\" ' ( { [ /";
            }

            if (SavedSettings.exceptionCharsASR == null)
            {
                SavedSettings.exceptionCharsASR = "";
            }

            if (SavedSettings.wordSplitters == null)
            {
                SavedSettings.wordSplitters = "& .";
            }

            if (SavedSettings.wordSplittersASR == null)
            {
                SavedSettings.wordSplittersASR = ".";
            }

            if (SavedSettings.customText == null || SavedSettings.customText.Length < 10)
            {
                SavedSettings.customText = new string[10];
                SavedSettings.customText[0] = "";
                SavedSettings.customText[1] = "";
                SavedSettings.customText[2] = "";
                SavedSettings.customText[3] = "";
                SavedSettings.customText[4] = "";
                SavedSettings.customText[5] = "";
                SavedSettings.customText[6] = "";
                SavedSettings.customText[7] = "";
                SavedSettings.customText[8] = "";
                SavedSettings.customText[9] = "";
            }

            if (SavedSettings.appendedText == null || SavedSettings.appendedText.Length < 10)
            {
                SavedSettings.appendedText = new string[10];
                SavedSettings.appendedText[0] = "";
                SavedSettings.appendedText[1] = "";
                SavedSettings.appendedText[2] = "";
                SavedSettings.appendedText[3] = "";
                SavedSettings.appendedText[4] = "";
                SavedSettings.appendedText[5] = "";
                SavedSettings.appendedText[6] = "";
                SavedSettings.appendedText[7] = "";
                SavedSettings.appendedText[8] = "";
                SavedSettings.appendedText[9] = "";
            }

            if (SavedSettings.addedText == null || SavedSettings.addedText.Length < 10)
            {
                SavedSettings.addedText = new string[10];
                SavedSettings.addedText[0] = "";
                SavedSettings.addedText[1] = "";
                SavedSettings.addedText[2] = "";
                SavedSettings.addedText[3] = "";
                SavedSettings.addedText[4] = "";
                SavedSettings.addedText[5] = "";
                SavedSettings.addedText[6] = "";
                SavedSettings.addedText[7] = "";
                SavedSettings.addedText[8] = "";
                SavedSettings.addedText[9] = "";
            }


            SavedSettings.actualPerCent5 = -1;
            SavedSettings.actualPerCent45 = -1;
            SavedSettings.actualPerCent4 = -1;
            SavedSettings.actualPerCent35 = -1;
            SavedSettings.actualPerCent3 = -1;
            SavedSettings.actualPerCent25 = -1;
            SavedSettings.actualPerCent2 = -1;
            SavedSettings.actualPerCent15 = -1;
            SavedSettings.actualPerCent1 = -1;
            SavedSettings.actualPerCent05 = -1;

            if (SavedSettings.autobackupDirectory == null)
                SavedSettings.autobackupDirectory = "Tag Backups";
            if (!Directory.Exists(GetAutobackupDirectory(SavedSettings.autobackupDirectory)))
                Directory.CreateDirectory(GetAutobackupDirectory(SavedSettings.autobackupDirectory));

            if (SavedSettings.defaultTagHistoryNumberOfBackups < 1)
                SavedSettings.defaultTagHistoryNumberOfBackups = 10;


            if (SavedSettings.dontHighlightChangedTags)
            {
                ChangedStyle.ForeColor = UnchangedStyle.ForeColor;
                ChangedStyle.SelectionForeColor = UnchangedStyle.SelectionForeColor;
            }
            else
            {
                ChangedStyle.ForeColor = Color.FromKnownColor(KnownColor.Highlight);

                int r = UnchangedStyle.SelectionForeColor.R;
                int g = UnchangedStyle.SelectionForeColor.G;
                int b = UnchangedStyle.SelectionForeColor.B;

                if (r < 128) r = r * 7 / 6; else r = r * 6 / 7;
                if (g < 128) g = g * 7 / 6; else g = g * 6 / 7;
                if (b < 128) b = b * 7 / 6; else b = b * 6 / 7;

                ChangedStyle.SelectionForeColor = Color.FromArgb(r, g, b);
            }
            #endregion


            #region Localizations
            Language = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

            PluginsPath = Path.Combine(Application.StartupPath, "Plugins");
            if (!File.Exists(Path.Combine(PluginsPath, "mb_TagTools.dll")))
                PluginsPath = Path.Combine(MbApiInterface.Setting_GetPersistentStoragePath(), "Plugins");

            if (File.Exists(Application.StartupPath + @"\Plugins\DevMode.txt"))
                DeveloperMode = true;

            if (!File.Exists(Path.Combine(PluginsPath, "ru", "mb_TagTools.resources.dll"))) Language = "en"; //For testing

            if (Language == "ru")
            {
                //Lets redefine localizable strings

                //Plugin localizable strings
                PluginName = "Дополнительные инструменты";
                PluginMenuGroup = "Дополнительные инструменты";
                Description = "Плагин добавляет дополнительные инструменты для работы с тегами";

                TagToolsMenuSectionName = "ДОПОЛНИТЕЛЬНЫЕ ИНСТРУМЕНТЫ";
                BackupRestoreMenuSectionName = "АРХИВАЦИЯ И ВОССТАНОВЛЕНИЕ";

                CopyTagCommandName = "Копировать тег...";
                SwapTagsCommandName = "Поменять местами два тега...";
                ChangeCaseCommandName = "Изменить регистр тега...";
                ReencodeTagCommandName = "Изменить кодировку тега...";
                ReencodeTagsCommandName = "Изменить кодировку тегов...";
                LibraryReportsCommandName = "Отчеты по библиотеке...";
                AutoLibraryReportsCommandName = "Автоматические отчеты по библиотеке...";
                AutoRateCommandName = "Автоматически установить рейтинги...";
                AsrCommandName = "Дополнительный поиск и замена...";
                CarCommandName = "Рассчитать средний рейтинг альбомов...";
                CtCommandName = "Сравнить треки...";
                CopyTagsToClipboardCommandName = "Копировать теги в буфер обмена...";
                PasteTagsFromClipboardCommandName = "Вставить теги из буфера обмена";
                MsrCommandName = "Множественный поиск и замена...";
                ShowHiddenCommandName = "Показать скрытые окна плагина";

                TagToolsHotkeyDescription = "Дополнительные инструменты: ";
                CopyTagCommandDescription = TagToolsHotkeyDescription + "Копировать тег";
                SwapTagsCommandDescription = TagToolsHotkeyDescription + "Поменять местами два тега";
                ChangeCaseCommandDescription = TagToolsHotkeyDescription + "Изменить регистр тега";
                ReencodeTagCommandDescription = TagToolsHotkeyDescription + "Изменить кодировку тега";
                ReencodeTagsCommandDescription = TagToolsHotkeyDescription + "Изменить кодировку тегов";
                LibraryReportsCommandDescription = TagToolsHotkeyDescription + "Отчеты по библиотеке";
                AutoLibraryReportsCommandDescription = TagToolsHotkeyDescription + "Автоматические отчеты по библиотеке";
                AutoRateCommandDescription = TagToolsHotkeyDescription + "Автоматически установить рейтинги";
                AsrCommandDescription = TagToolsHotkeyDescription + "Дополнительный поиск и замена";
                AsrHotkeyDescription = TagToolsHotkeyDescription + "⌕: ";
                CarCommandDescription = TagToolsHotkeyDescription + "Рассчитать средний рейтинг альбомов";
                CtCommandDescription = TagToolsHotkeyDescription + "Сравнить треки";
                CopyTagsToClipboardCommandDescription = TagToolsHotkeyDescription + "Копировать теги в буфер обмена";
                PasteTagsFromClipboardCommandDescription = TagToolsHotkeyDescription + "Вставить теги из буфера обмена";
                CopyTagsToClipboardUsingMenuDescription = "Копировать теги в буфер обмена, используя набор";
                MsrCommandDescription = TagToolsHotkeyDescription + "Множественный поиск и замена";
                ShowHiddenCommandDescription = TagToolsHotkeyDescription + "Показать скрытые окна плагина";

                BackupTagsCommandName = "Архивировать теги для всех треков...";
                RestoreTagsCommandName = "Восстановить теги для выбранных треков...";
                RestoreTagsForEntireLibraryCommandName = "Восстановить теги для всех треков...";
                RenameMoveBackupCommandName = "Переименовать или переместить архив...";
                MoveBackupsCommandName = "Переместить архив(ы)...";
                CreateNewBaselineCommandName = "Создать новый опорный архив текущей библиотеки...";
                DeleteBackupsCommandName = "Удалить архив(ы)...";
                AutoBackupSettingsCommandName = "Настройки (авто)архивации...";

                TagHistoryCommandName = "История тегов...";

                BackupTagsCommandDescription = TagToolsHotkeyDescription + "Архивировать теги для всех треков";
                RestoreTagsCommandDescription = TagToolsHotkeyDescription + "Восстановить теги для выбранных треков";
                RestoreTagsForEntireLibraryCommandDescription = TagToolsHotkeyDescription + "Восстановить теги для всех треков";
                RenameMoveBackupCommandDescription = TagToolsHotkeyDescription + "Переименовать или переместить архив";
                MoveBackupsCommandDescription = TagToolsHotkeyDescription + "Переместить архив(ы)";
                CreateNewBaselineCommandDescription = TagToolsHotkeyDescription + "Создать новый опорный архив текущей библиотеки";
                DeleteBackupsCommandDescription = TagToolsHotkeyDescription + "Удалить архив(ы)";
                AutoBackupSettingsCommandDescription = TagToolsHotkeyDescription + "Настройки (авто)архивации";

                TagHistoryCommandDescription = TagToolsHotkeyDescription + "История тегов";

                CopyTagCommandSbText = "Копирование тегов";
                SwapTagsCommandSbText = "Обмен тегов местами";
                ChangeCaseCommandSbText = "Изменение регистра тега";
                ReencodeTagCommandSbText = "Изменение кодировки тегов";
                LibraryReportsCommandSbText = "Формирование отчета";
                LibraryReportsGneratingPreviewCommandSbText = "Формирование предварительного просмотра";
                AutoRateCommandSbText = "Автоматическая установка рейтингов";
                AsrCommandSbText = "Дополнительный поиск и замена";
                CarCommandSbText = "Расчет среднего рейтинга альбомов";
                MsrCommandSbText = "Множественный поиск и замена";

                GroupingName = "<Группировка>";
                CountName = "Количество";
                SumName = "Сумма";
                MinimumName = "Минимум";
                MaximumName = "Максимум";
                AverageName = "Среднее значение";
                AverageCountName = "Среднее количество";

                LibraryReport = "Отчет по библиотеке";


                //Other localizable strings
                DateCreatedTagName = "<Создано>";
                EmptyValueTagName = "<Пустое значение>";
                ClipboardTagName = "<Буфер обмена>";
                TextFileTagName = "<Текстовый файл>";

                FolderTagName = "<Папка>";
                FileNameTagName = "<Имя файла>";
                FilePathTagName = "<Путь к файлу>";
                AlbumUniqueIdName = "<Уникальный Id альбома>";

                ParameterTagName = "Тег";
                //tempTagName = "Врем";

                CustomTagsPresetName = "<Несохраненные теги>";
                LibraryTotalsPresetName = "Итоги по библиотеке";
                LibraryAveragesPresetName = "В среднем по библиотеке";
                CDBookletPresetName = "Буклет компакт-диска";
                AlbumsAndTracksPresetName = "Альбомы и треки";

                EmptyPresetName = "<Пустой пресет>";

                GenericTagSetName = "Набор тегов";


                SequenceNumberName = "№ п/п";


                //Supported exported file formats
                ExportedFormats = "Документ HTML (по альбомам)|*.htm|Документ HTML|*.htm|Простая таблица HTML|*.htm|Текст, разделенный табуляциями|*.txt|Плейлист M3U|*.m3u|Файл CSV|*.csv|Документ HTML (буклет компакт-диска)|*.htm";
                exportedTrackList = "Список экпортированных треков";

                //Displayed text
                ListItemConditionIs = "равен";
                ListItemConditionIsNot = "не равен";
                ListItemConditionIsGreater = "больше чем";
                ListItemConditionIsLess = "меньше чем";

                OKButtonName = "Применить";
                StopButtonName = "Остановить";
                CancelButtonName = "Отменить";
                HideButtonName = "Скрыть";
                PreviewButtonName = "Просмотр";
                ClearButtonName = "Очистить";
                FindButtonName = "Найти";
                SelectFoundButtonName = "Выбрать найденное";
                OriginalTagHeaderTextTagValue = "Значение тега";
                OriginalTagHeaderText = "Исходное значение";

                TableCellError = "#Ошибка!";

                DefaultASRPresetName = "Пресет №";

                MsgFileNotFound = "Файл не найден!";
                MsgNoFilesSelected = "Файлы не выбраны.";
                MsgNoFilesDisplayed = "Нет отображаемых фалйов.";
                MsgSourceAndDestinationTagsAreTheSame = "Оба тега одинаковые. Обработка не выполнена.";
                MsgSwapTagsSourceAndDestinationTagsAreTheSame = "Использовать один и тот же " +
                    "тег-источник и тег-получатель имеет смысл только для тегов 'Исполнитель'/'Композитор' для преобразования строки, разделенной символами ';', в список исполнителей/композиторов и " +
                    "наоборот. Обработка не выполнена.";
                MsgNoTagsSelected = "Теги не выбраны. Обработка не выполнена.";
                MsgNoFilesInCurrentView = "Нет файлов в текущем режиме отображения.";
                MsgTracklistIsEmpty = "Список треков пуст. Сначала нажмите кнопку 'Просмотр'.";
                MsgForExportingPlaylistsURLfieldMustBeIncludedInTagList = "Для экпортирования плейлистов тег '" + UrlTagName + "' должен быть обязательно включен в таблицу.";
                MsgPreviewIsNotGeneratedNothingToSave = "Таблица не сгенерирована. Нечего сохранять.";
                MsgPreviewIsNotGeneratedNothingToChange = "Таблица не сгенерирована. Нечего изменять.";
                MsgNoAggregateFunctionNothingToSave = "В таблице нет ни одной агрегатной функции. Нечего сохранять.";
                MsgPleaseUseGroupingFunctionForArtworkTag = "Пожалуйста, используйте функцию <Группировка> для тега 'Обложка'!";
                MsgAllTags = "ВСЕ ТЕГИ";
                MsgNoURLcolumnUnableToSave = "В таблице нет тега '" + UrlTagName + "'. Невозможно сохранить результаты.";
                MsgEmptyURL = "Пустой тег '" + UrlTagName + "' в строке ";
                MsgUnableToSave = "Невозможно сохранить результаты. ";
                MsgUsingEmptyValueAsSourceTagMayBeUsefulOnlyForAppendingStaticTextToDestinationtag = "Использование псевдотега '" + EmptyValueTagName +
                    "' в качестве тега-источника имеет смысл только при включенной опции 'Добавить тег источника в конец тега получателя' для добавления какого-то " +
                    "текста  к тегу-получателю. Обработка не выполнена.";
                MsgBackgroundTaskIsCompleted = "Фоновая задача плагина 'Дополнительные инструменты' завершена.";
                MsgThresholdsDescription = "Автоматическая установка рейтингов основана на среднем числе воспроизведений композиции в день \n" +
                    "(на виртуальном теге 'число воспроизведений в день'). Вам следует установить пороговые значения 'числа воспроизведений в день' для каждого значения рейтинга, \n" +
                    "которое может быть назаначено композициям. Пороговые значения оцениваются в убывающем порядке: от рейтинга 5 звездочек \n" +
                    "до рейтинга 0.5 звездочки. Тег 'число воспроизведений в день' может быть рассчитан для каждой композиции независимо от остальной библиотеки, \n" +
                    "так что возможно автоматическое обновление рейтинга при смене композиции. Также возможно рассчитать рейтинги для выбранных композиций \n" +
                    "коммандой 'Установить рейтинги' или для всех композиций библиотеки при запуске MusicBee. ";
                MsgAutoCalculationOfThresholdsDescription = "Автоматический расчет пороговых значений 'числа воспроизведений в день' - это еще один способ установки авто-рейтингов. \n" +
                    "Эта опция позволяет вам установить желаемый процент каждого значения рейтинга в библиотеке. Действительные проценты значений рейтингов могут отличаться \n" +
                    "от желаемых, поскольку не всегда можно разбить композиции библиотеки на несколько групп (предположим все композиции библитеки имеют одинаковое \n" +
                    "значение 'числа воспроизведений в день', очевидно не существует способа разбить все композиции на группы, исходя из значений 'числа воспроизведений в день'. \n" +
                    "Действительные проценты отображаются справа от желаемых процентов после вычисления пороговых значений. Поскольку вычисление пороговых значений требует заметного \n" +
                    "времени, то оно не может производиться автоматически, кроме как при запуске MusicBee. ";

                MsgNumberOfPlayedTracks = "Число когда либо воспроизводившихся композиций: ";
                MsgIncorrectSumOfWeights = "Сумма всех процентов должна быть равна 100% или меньше!";
                MsgSum = "Сумма: ";
                MsgNumberOfNotRatedTracks = "% композиций с нулевым рейтингом)";
                MsgTracks = " композиций)";
                MsgActualPercent = "% / Действ.: ";
                MsgIncorrectPresetName = "Некорректное название пресета или пресет с таким названием уже существует.";

                MsgDeletePresetConfirmation = "Вы действительно хотите удалить выбранный пресет?";
                MsgImportingConfirmation = "Вы действительно хотите установить стандартные пресеты?";
                MsgDoYouWantToUpdateYourCustomizedPredefinedPresets = "Некоторые стандартные пресеты, которые вы настроили, "
                    + "были изменены разработчиком.\n"
                    + "Вы хотите обновить настроенные стандартные пресеты новыми версиями от разработчика?\n\n"
                    + "ВСЕ ВАШИ НАСТРОЙКИ БУДУТ УТЕРЯНЫ!";
                MsgDoYouWantToImportExistingPresetsAsCopies = "Некоторые импортируемые пресеты уже существуют.\n"
                    + "Импортировать их как новые пресеты, сохранив существующие?\n\n"
                    + "В ПРОТИВНОМ СЛУЧАЕ СУЩЕСТВУЮЩИЕ ПРЕСЕТЫ БУДУТ ПЕРЕЗАПИСАНЫ!\n";

                MsgPresetsWereImported = " пресет{;а;ов} был{;и;и} импортирован{;ы;ы}.\n";
                MsgPresetsWereImportedAsCopies = " пресет{;а;ов} был{;и;и} импортирован{;ы;ы} как новы{й;ые;ые} пресет{;ы;ы}.\n";
                MsgPresetsFailedToImport = " пресет{;а;ов} не удалось импортировать из-за ошиб{ки;ок;ок}\n" + AddLeadingSpaces(0, 4, 0)  + " чтения файл{а;ов;ов} или {его;их;их} неверного формата.";

                MsgPresetsWereInstalled = " пресет{;а;ов} был{;и;и} устновлен{;ы;ы}.\n\n";
                MsgPresetsWereUpdated = " пресет{;а;ов} был{;и;и} обновлен{;ы;ы}.\n\n";
                MsgPresetsCustomizedWereUpdated = " пресет{;а;ов}, настроенн{ый;ых;ых} вами, был{;и;и} обновлен{;ы;ы}.\n\n";
                MsgPresetsWereNotChanged = " пресет{;а;ов} не изменил{ся;ись;ись} с последнего \n" + AddLeadingSpaces(0, 4, 0)  + " обновления и был{;и;и} пропущен{;ы;ы}.\n\n";
                MsgPresetsWereSkipped = " пресет{;а;ов} был{;и;и} настроен{;ы;ы} вами\n" + AddLeadingSpaces(0, 4, 0)  + " и был{;и;и} пропущен{;ы;ы}.\n\n";
                MsgPresetsFailedToUpdate = " пресет{;а;ов} не удалось обновить из-за ошиб{ки;ок;ок}\n" + AddLeadingSpaces(0, 4, 0)  + " чтения файл{а;ов;ов} или {его;их;их} неверного формата.";
                MsgPresetsNotFound = "Не найдены пресеты для установки в ожидаемом каталоге!";

                MsgDeletingConfirmation = "Вы действительно хотите удалить все стандартные пресеты?";
                MsgNoPresetsDeleted = "Пресет не были удалены.";
                MsgPresetsWereDeleted = " пресет{;а;ов} был{;и;и} удален{;ы;ы}.";
                MsgFailedToDelete = " пресет{;а;ов} не удалось удалить.";

                MsgNumberOfTagsInTextFile = "Количество тегов в текстовом файле (";
                MsgDoesntCorrespondToNumberOfSelectedTracks = ") не соответствует количеству выбранных треков (";
                MsgMessageEnd = ")!";

                MsgClipboardDesntContainText = "Буфер обмена не содержит текст!";

                MsgNumberOfTagsInClipboard = "Количество тегов в буфере обмена (";
                MsgNumberOfTracksInClipboard = "Количество треков в буфере обмена (";
                MsgDoesntCorrespondToNumberOfSelectedTracksC = ") не соответствует количеству выбранных треков (";
                MsgDoesntCorrespondToNumberOfCopiedTagsC = ") не соответствует количеству скопированных тегов (";
                MsgMessageEndC = ")!";
                MsgDoYouWantToPasteAnyway = " Вставить теги все равно?";

                MsgFirstThreeGroupingFieldsInPreviewTableShouldBe = "Первые три поля группировок в таблице должны быть '" + DisplayedAlbumArtsistName + "', '" 
                    + AlbumTagName + "' и '" + ArtworkName + "' для того, чтобы экспортировать теги в формат 'Документ HTML (по альбомам)'";
                MsgFirstSixGroupingFieldsInPreviewTableShouldBe = "Первые шесть полей группировок в таблице должны быть '" + SequenceNumberName
                    + "', '" + DisplayedAlbumArtsistName + "', '" + AlbumTagName + "', '" + ArtworkName + "', '"
                    + MbApiInterface.Setting_GetFieldName(MetaDataType.TrackTitle) + "' and '"
                    + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Duration)
                    + "' для того, чтобы экспортировать теги в формат 'Документ HTML (буклет компакт-диска)'";

                MsgBackgroundAutoLibraryReportIsExecuted = "Исполняется фоновая задача автоматических отчетов по библиотеке! Подождите пока она завершится!";

                MsgYouMustImportStandardASRPresetsFirst = "Сначала надо импортировать стандартные пресеты дополнительного поиска и замены!";

                SbSorting = "Сортировка таблицы...";
                SbUpdating = "запись";
                SbReading = "чтение";
                SbUpdated = "записан(о)";
                SbRead = "прочитан(о)";
                SbFiles = "файл(ов)";
                SbAsrPresetIsApplied = "Применен пресет дополнительного поиска и замены: %PRESETNAME%";
                SbAsrPresetsAreApplied = "Применены пресеты дополнительного поиска и замены: %PRESETCOUNT%";

                CtlDirtyError1sf = "Работает/запланирована ";
                CtlDirtyError1mf = "Работают/запланированы ";
                CtlDirtyError2sf = " фоновая операция обновления файлов. \n" +
                    "Результаты предварительного просмотра могут быть не точны. ";
                CtlDirtyError2mf = " фоновых операций обновления файлов. \n" +
                    "Результаты предварительного просмотра могут быть не точны. ";


                MsgMasterBackupIndexIsCorrupted = "Основной индекс архива тегов поврежден! Все существующие на данный момент архивы будут не доступны в команде 'История тегов'.";
                MsgBackupIsCorrupted = "Архив '%%FILENAME%%' или поврежден, или не является архивом MusicBee!";
                MsgFolderDoesntExists = "Папка не существует!";
                MsgSelectOneTrackOnly = "Выберите только один трек!";
                MsgSelectTrack = "Выберите трек!";
                MsgSelectAtLeast2Tracks = "Выберите по меньшей мере 2 трека для сравнения!";
                MsgBackupBaselineFileDoesntExist1 = "Файл первоначального опорного архива '";
                MsgBackupBaselineFileDoesntExist2 = "' не существует!";
                MsgThisIsTheBackupOfDifferentLibrary = "Это архив другой библиотеки!";
                MsgCreateBaselineWarning = "Когда создается первый архив любой библиотеки, сначала создается полный опорный архив. Все последующие архивы являются разницей с опорным архивом. " +
                    "Если было изменено очень много тегов, то разностные архивы могут стать очень большими. Эта команда удалит ВСЕ разностные архивы ТЕКУЩЕЙ библиотеки и создаст новый опорный архив. ";

                MsgGiveNameToASRpreset = "Задайте название пресета!";
                MsgAreYouSureYouWantToSaveASRpreset = "Cохранить пресет дополнительного поиска и замены под названием \"%PRESETNAME%\"?";
                MsgAreYouSureYouWantToOverwriteASRpreset = "Перезаписать пресет дополнительного поиска и замены \"%PRESETNAME%\"?";
                MsgAreYouSureYouWantToOverwriteRenameASRpreset = "Перезаписать пресет дополнительного поиска и замены \"%PRESETNAME%\", переименовав его в \"%NEWPRESETNAME%\"?";
                MsgAreYouSureYouWantToDeleteASRpreset = "Удалить пресет дополнительного поиска и замены \"%PRESETNAME%\"?";
                MsgPredefinedPresetsCantBeChanged = "Стандартные пресеты нельзя изменять. Редактор пресетов будет открыт в режиме просмотра.\n\n"
                    + "Отключить показ этого предупреждения?";
                MsgSavePreset = "Сохранить пресет";
                MsgDeletePreset = "Удалить пресет";

                CtlNewASRPreset = "<Новый пресет ДПЗ>";

                SbAutobackuping = "Автосохранение архива тегов...";
                SbMovingBackupsToNewFolder = "Идет перемещение архивов в новую папку...";
                SbMakingTagBackup = "Сохранение архива тегов...";
                SbRestoringTagsFromBackup = "Идет восстановление тегов из архива...";
                SbRenamingMovingBackup = "Идет переименование/перемещение архива...";
                SbMovingBackups = "Идет перемещение архивов...";
                SbDeletingBackups = "Идет удаление архивов...";
                SbTagAutobackupSkipped = "Автоархивирование тегов пропущено (не было изменеий с момента последней архивации)";
                SbCompairingTags = "Идет сравнение тегов с основным архивом... ";

                CtlWarning = "ПРЕДУПРЕЖДЕНИЕ!";
                CtlMusicBeeBackupType = "Архив тегов MusicBee|*.xml";
                CtlSaveBackupTitle = "ПЕРЕЙДИТЕ В НУЖНУЮ ПАПКУ, НАПИШИТЕ НАЗВАНИЕ АРХИВА ВНИЗУ ОКНА И НАЖМИТЕ 'СОХРАНИТЬ'";
                CtlRestoreBackupTitle = "ПЕРЕЙДИТЕ В НУЖНУЮ ПАПКУ, ВЫБЕРИТЕ АРХИВ И НАЖМИТЕ 'ОТКРЫТЬ'";
                CtlRenameSelectBackupTitle = "ПЕРЕЙДИТЕ В НУЖНУЮ ПАПКУ, ВЫБЕРИТЕ АРХИВ, КОТОРЫЙ ХОТИТЕ ПЕРЕИМЕНОВАТЬ/ПЕРЕМЕСТИТЬ И НАЖМИТЕ 'ОТКРЫТЬ'";
                CtlRenameSaveBackupTitle = "ПЕРЕЙДИТЕ В НУЖНУЮ ПАПКУ, НАПИШИТЕ НОВОЕ НАЗВАНИЕ АРХИВА ВНИЗУ ОКНА И НАЖМИТЕ 'СОХРАНИТЬ'";
                CtlMoveSelectBackupsTitle = "ПЕРЕЙДИТЕ В НУЖНУЮ ПАПКУ, ВЫБЕРИТЕ АРХИВЫ, КОТОРЫЕ ХОТИТЕ ПЕРЕМЕСТИТЬ И НАЖМИТЕ 'ОТКРЫТЬ'";
                CtlMoveSaveBackupsTitle = "ПЕРЕЙДИТЕ В ПАПКУ, В КОТОРУЮ ХОТИТЕ ПЕРЕМЕСТИТЬ АРХИВЫ И НАЖМИТЕ 'СОХРАНИТЬ'";
                CtlDeleteBackupsTitle = "ПЕРЕЙДИТЕ В НУЖНУЮ ПАПКУ, ВЫБЕРИТЕ АРХИВ(Ы) ДЛЯ УДАЛЕНИЯ И НАЖМИТЕ 'ОТКРЫТЬ'";
                CtlSelectThisFolder = "[ВЫБРАТЬ ЭТУ ПАПКУ]";
                CtlNoBackupData = "<Нет архивных данных>";
                CtlNoDifferences = "<Нет отличий>";

                MsgNotAllowedSymbols = "Разрешены только буквы ASCII, числа и символы - : _ .!";
                MsgPresetExists = "Пресет с id-ом %ID% уже существует!";

                CtlWholeCuesheetWillBeReencoded = "ВНИМАНИЕ: ОДИН ИЛИ НЕСКЛЬКО ТРЕКОВ ОТНОСЯТСЯ К ФАЙЛУ РАЗМЕТКИ. КОДИРОВКА БУДЕТ ИЗМЕНЕНА ДЛЯ ВСЕГО ФАЙЛА РАЗМЕТКИ!";

                CtlMSR = "МПЗ: ";

                MsgFirstSelectWhichFieldYouWantToAssignFunctionIdTo = "Сначала выберите, какому полю вы хотите назначить id функции вирт. тегов (самый левый выпадающий список на линии idа функции)!";

                ////Defaults for controls
                //charBox = "";

                //exceptions = "the a an and or not";
                //exceptionChars = "\" ' ( { [ /";
                //wordSplitters = "&";

                BackupDefaultPrefix = "Архив тегов ";
                ASRPresetNaming = "Пресет ДПЗ";

                if (SavedSettings.autobackupPrefix == null) SavedSettings.autobackupPrefix = "Автоматический архив тегов ";
            }
            else
            {
                Language = "en";
            }
            #endregion


            #region Resetting invalid/absent settings again
            if (SavedSettings.autobackupPrefix == null)
                SavedSettings.autobackupPrefix = "Tag Autobackup ";


            string[] noTags = new string[0];
            FunctionType[] noTypes = new FunctionType[0];
            LibraryReportsCommand.LibraryReportsPreset tempLibraryReportsPreset;

            if (SavedSettings.libraryReportsPresets == null || SavedSettings.libraryReportsPresets.Length < 10)
            {
                SavedSettings.libraryReportsPresets = new LibraryReportsCommand.LibraryReportsPreset[10];



                tempLibraryReportsPreset = new LibraryReportsCommand.LibraryReportsPreset
                {
                    groupingNames = noTags,
                    functionNames = noTags,
                    functionTypes = noTypes,
                    parameterNames = noTags
                };



                SavedSettings.libraryReportsPresets[5] = tempLibraryReportsPreset;
                SavedSettings.libraryReportsPresets[6] = tempLibraryReportsPreset;
                SavedSettings.libraryReportsPresets[7] = tempLibraryReportsPreset;
                SavedSettings.libraryReportsPresets[8] = tempLibraryReportsPreset;
                SavedSettings.libraryReportsPresets[9] = tempLibraryReportsPreset;
            }



            LibraryReportsPreset = new LibraryReportsCommand.LibraryReportsPreset
            {
                groupingNames = noTags,
                functionNames = noTags,
                functionTypes = noTypes,
                parameterNames = noTags,
                name = CustomTagsPresetName.ToUpper()
            };

            SavedSettings.libraryReportsPresets[0] = LibraryReportsPreset;


            string[] tempGroupings = new string[3];
            string[] tempFunctions = new string[9];
            FunctionType[] tempFunctionTypes = new FunctionType[9];
            string[] tempParameters = new string[9];
            string[] tempParameters2 = new string[9];

            tempGroupings[0] = MbApiInterface.Setting_GetFieldName(MetaDataType.Genre);
            tempGroupings[1] = DisplayedAlbumArtsistName;
            tempGroupings[2] = MbApiInterface.Setting_GetFieldName(MetaDataType.Album);

            tempParameters[0] = DisplayedAlbumArtsistName;
            tempParameters[1] = MbApiInterface.Setting_GetFieldName(MetaDataType.Genre);
            tempParameters[2] = MbApiInterface.Setting_GetFieldName(MetaDataType.Year);
            tempParameters[3] = MbApiInterface.Setting_GetFieldName(MetaDataType.Album);
            tempParameters[4] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);
            tempParameters[5] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Duration);
            tempParameters[6] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Size);
            tempParameters[7] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.PlayCount);
            tempParameters[8] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.SkipCount);

            tempFunctionTypes[0] = FunctionType.Count;
            tempFunctionTypes[1] = FunctionType.Count;
            tempFunctionTypes[2] = FunctionType.Count;
            tempFunctionTypes[3] = FunctionType.Count;
            tempFunctionTypes[4] = FunctionType.Count;
            tempFunctionTypes[5] = FunctionType.Sum;
            tempFunctionTypes[6] = FunctionType.Sum;
            tempFunctionTypes[7] = FunctionType.Sum;
            tempFunctionTypes[8] = FunctionType.Sum;

            for (int i = 0; i < tempParameters.Length; i++)
                tempFunctions[i] = LibraryReportsCommand.GetColumnName(tempParameters[i], null, tempFunctionTypes[i]);

            tempLibraryReportsPreset = new LibraryReportsCommand.LibraryReportsPreset
            {
                groupingNames = tempGroupings,
                functionNames = tempFunctions,
                functionTypes = tempFunctionTypes,
                parameterNames = tempParameters,
                parameter2Names = tempParameters2,
                totals = true,
                name = LibraryTotalsPresetName.ToUpper()
            };

            SavedSettings.libraryReportsPresets[1] = tempLibraryReportsPreset;



            tempGroupings = new string[3];
            tempFunctions = new string[10];
            tempFunctionTypes = new FunctionType[10];
            tempParameters = new string[10];
            tempParameters2 = new string[10];

            tempGroupings[0] = MbApiInterface.Setting_GetFieldName(MetaDataType.Genre);
            tempGroupings[1] = DisplayedAlbumArtsistName;
            tempGroupings[2] = MbApiInterface.Setting_GetFieldName(MetaDataType.Album);

            tempParameters[0] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);
            tempParameters[1] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);
            tempParameters[2] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);
            tempParameters[3] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);
            tempParameters[4] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Bitrate);
            tempParameters[5] = MbApiInterface.Setting_GetFieldName(MetaDataType.Rating);
            tempParameters[6] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Duration);
            tempParameters[7] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Size);
            tempParameters[8] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.PlayCount);
            tempParameters[9] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.SkipCount);

            tempParameters2[0] = MbApiInterface.Setting_GetFieldName(MetaDataType.Artist);
            tempParameters2[1] = MbApiInterface.Setting_GetFieldName(MetaDataType.Album);
            tempParameters2[2] = MbApiInterface.Setting_GetFieldName(MetaDataType.Genre);
            tempParameters2[3] = MbApiInterface.Setting_GetFieldName(MetaDataType.Year);
            tempParameters2[4] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);
            tempParameters2[5] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);
            tempParameters2[6] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);
            tempParameters2[7] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);
            tempParameters2[8] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);
            tempParameters2[9] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);

            tempFunctionTypes[0] = FunctionType.AverageCount;
            tempFunctionTypes[1] = FunctionType.AverageCount;
            tempFunctionTypes[2] = FunctionType.AverageCount;
            tempFunctionTypes[3] = FunctionType.AverageCount;
            tempFunctionTypes[4] = FunctionType.Average;
            tempFunctionTypes[5] = FunctionType.Average;
            tempFunctionTypes[6] = FunctionType.Average;
            tempFunctionTypes[7] = FunctionType.Average;
            tempFunctionTypes[8] = FunctionType.Average;
            tempFunctionTypes[9] = FunctionType.Average;

            for (int i = 0; i < tempParameters.Length; i++)
                tempFunctions[i] = LibraryReportsCommand.GetColumnName(tempParameters[i], tempParameters2[i], tempFunctionTypes[i]);

            tempLibraryReportsPreset = new LibraryReportsCommand.LibraryReportsPreset
            {
                groupingNames = tempGroupings,
                functionNames = tempFunctions,
                functionTypes = tempFunctionTypes,
                parameterNames = tempParameters,
                parameter2Names = tempParameters2,
                totals = true,
                name = LibraryAveragesPresetName.ToUpper()
            };

            SavedSettings.libraryReportsPresets[2] = tempLibraryReportsPreset;



            tempGroupings = new string[6];
            tempFunctions = new string[0];
            tempFunctionTypes = new FunctionType[0];
            tempParameters = new string[0];
            tempParameters2 = new string[0];

            tempGroupings[0] = SequenceNumberName;
            tempGroupings[1] = DisplayedAlbumArtsistName;
            tempGroupings[2] = MbApiInterface.Setting_GetFieldName(MetaDataType.Album);
            tempGroupings[3] = MbApiInterface.Setting_GetFieldName(MetaDataType.Artwork);
            tempGroupings[4] = MbApiInterface.Setting_GetFieldName(MetaDataType.TrackTitle);
            tempGroupings[5] = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Duration);

            tempLibraryReportsPreset = new LibraryReportsCommand.LibraryReportsPreset
            {
                groupingNames = tempGroupings,
                functionNames = tempFunctions,
                functionTypes = tempFunctionTypes,
                parameterNames = tempParameters,
                parameter2Names = tempParameters2,
                totals = false,
                name = CDBookletPresetName.ToUpper()
            };

            SavedSettings.libraryReportsPresets[3] = tempLibraryReportsPreset;



            tempGroupings = new string[4];
            tempFunctions = new string[0];
            tempFunctionTypes = new FunctionType[0];
            tempParameters = new string[0];
            tempParameters2 = new string[0];

            tempGroupings[0] = DisplayedAlbumArtsistName;
            tempGroupings[1] = MbApiInterface.Setting_GetFieldName(MetaDataType.Album);
            tempGroupings[2] = MbApiInterface.Setting_GetFieldName(MetaDataType.Artwork);
            tempGroupings[3] = MbApiInterface.Setting_GetFieldName(MetaDataType.TrackTitle);

            tempLibraryReportsPreset = new LibraryReportsCommand.LibraryReportsPreset
            {
                groupingNames = tempGroupings,
                functionNames = tempFunctions,
                functionTypes = tempFunctionTypes,
                parameterNames = tempParameters,
                parameter2Names = tempParameters2,
                totals = false,
                name = AlbumsAndTracksPresetName.ToUpper()
            };

            SavedSettings.libraryReportsPresets[4] = tempLibraryReportsPreset;



            Conditions = new string[4];
            Conditions[0] = ListItemConditionIs;
            Conditions[1] = ListItemConditionIsNot;
            Conditions[2] = ListItemConditionIsGreater;
            Conditions[3] = ListItemConditionIsLess;

            //Lets reset invalid defaults for controls
            if (SavedSettings.closeShowHiddenWindows == 0)
                SavedSettings.closeShowHiddenWindows = 1;

            if (SavedSettings.changeCaseFlag == 0) SavedSettings.changeCaseFlag = 1;

            if (SavedSettings.defaultRating == 0) SavedSettings.defaultRating = 5;

            if (SavedSettings.numberOfTagsToRecalculate == 0) SavedSettings.numberOfTagsToRecalculate = 100;
            if (SavedSettings.autoLibraryReportsPresets == null) SavedSettings.autoLibraryReportsPresets = new AutoLibraryReportCommand.AutoLibraryReportsPreset[0];

            fillTagNames();

            //Again lets reset invalid defaults for controls
            if (GetTagId(SavedSettings.copySourceTagName) == 0)
                SavedSettings.copySourceTagName = ArtistArtistsName;
            if (GetTagId(SavedSettings.changeCaseSourceTagName) == 0)
                SavedSettings.changeCaseSourceTagName = ArtistArtistsName;
            if (GetTagId(SavedSettings.swapTagsSourceTagName) == 0)
                SavedSettings.swapTagsSourceTagName = ArtistArtistsName;

            //if (getTagId(SavedSettings.conditionTagName) == 0) SavedSettings.conditionTagName = tagCounterTagName;
            if (!ChangeCaseCommand.IsItemContainedInList(SavedSettings.condition, Conditions))
                SavedSettings.condition = ListItemConditionIsGreater;
            if (GetTagId(SavedSettings.destinationTagOfSavedFieldName) == 0)
                SavedSettings.destinationTagOfSavedFieldName = Custom9TagName;
            if (SavedSettings.filterIndex == 0)
                SavedSettings.filterIndex = 1;
            if ("" + SavedSettings.multipleItemsSplitterChar2 == "")
            {
                SavedSettings.multipleItemsSplitterChar1 = ";";
                SavedSettings.multipleItemsSplitterChar2 = "; ";
            }
            SavedSettings.multipleItemsSplitterChar1 = "" + SavedSettings.multipleItemsSplitterChar1;
            if ("" + SavedSettings.exportedTrackListName == "")
                SavedSettings.exportedTrackListName = exportedTrackList;

            if (GetTagId(SavedSettings.copyDestinationTagName) == 0)
                SavedSettings.copyDestinationTagName = GetTagName(MetaDataType.TrackTitle);
            if (GetTagId(SavedSettings.swapTagsDestinationTagName) == 0)
                SavedSettings.swapTagsDestinationTagName = GetTagName(MetaDataType.TrackTitle);

            if (SavedSettings.autoRateTagId == 0)
                SavedSettings.autoRateTagId = MetaDataType.Custom9;
            if (SavedSettings.playsPerDayTagId == 0)
                SavedSettings.playsPerDayTagId = MetaDataType.Custom8;
            if (!(SavedSettings.checkBox5 || SavedSettings.checkBox45 || SavedSettings.checkBox4 || SavedSettings.checkBox35 || SavedSettings.checkBox3
                || SavedSettings.checkBox25 || SavedSettings.checkBox2 || SavedSettings.checkBox15 || SavedSettings.checkBox1 || SavedSettings.checkBox05))
            {
                SavedSettings.checkBox5 = true;
                SavedSettings.perCent5 = 100;
            }

            if (GetTagId(SavedSettings.trackRatingTagName) == 0)
                SavedSettings.trackRatingTagName = GetTagName(MetaDataType.Rating);
            if (GetTagId(SavedSettings.albumRatingTagName) == 0)
                SavedSettings.albumRatingTagName = GetTagName(MetaDataType.RatingAlbum);


            if (SavedSettings.copyTagsTagSets == null)
            {
                List<string> tagNameList = new List<string>();
                FillList(tagNameList, false, true, false);
                //FillListWithProps(tagNameList);

                if (tagNameList.Contains("Sort Artist"))
                    tagNameList.Remove("Sort Artist");

                if (tagNameList.Contains("Sort Album Artist"))
                    tagNameList.Remove("Sort Album Artist");

                List<int> tagIdList = new List<int>();
                foreach (var tagName in tagNameList)
                    tagIdList.Add((int)GetTagId(tagName));


                SavedSettings.copyTagsTagSets = new TagSetType[10];

                SavedSettings.copyTagsTagSets[0] = new TagSetType
                {
                    setName = "[01] " + GenericTagSetName + " 1",
                    tagIds = new int[tagNameList.Count],
                    setPosition = 1
                };
                tagIdList.CopyTo(SavedSettings.copyTagsTagSets[0].tagIds);

                SavedSettings.copyTagsTagSets[1] = new TagSetType
                {
                    setName = "[02] " + GenericTagSetName + " 2",
                    tagIds = new int[tagNameList.Count],
                    setPosition = 2
                };
                tagIdList.CopyTo(SavedSettings.copyTagsTagSets[1].tagIds);

                SavedSettings.copyTagsTagSets[2] = new TagSetType
                {
                    setName = "[03] " + GenericTagSetName + " 3",
                    tagIds = new int[tagNameList.Count],
                    setPosition = 3
                };
                tagIdList.CopyTo(SavedSettings.copyTagsTagSets[2].tagIds);

                SavedSettings.copyTagsTagSets[3] = new TagSetType
                {
                    setName = "[04] " + GenericTagSetName + " 4",
                    tagIds = new int[tagNameList.Count],
                    setPosition = 4
                };
                tagIdList.CopyTo(SavedSettings.copyTagsTagSets[3].tagIds);

                SavedSettings.copyTagsTagSets[4] = new TagSetType
                {
                    setName = "[05] " + GenericTagSetName + " 5",
                    tagIds = new int[tagNameList.Count],
                    setPosition = 5
                };
                tagIdList.CopyTo(SavedSettings.copyTagsTagSets[4].tagIds);

                SavedSettings.copyTagsTagSets[5] = new TagSetType
                {
                    setName = "[06] " + GenericTagSetName + " 6",
                    tagIds = new int[tagNameList.Count],
                    setPosition = 6
                };
                tagIdList.CopyTo(SavedSettings.copyTagsTagSets[5].tagIds);

                SavedSettings.copyTagsTagSets[6] = new TagSetType
                {
                    setName = "[07] " + GenericTagSetName + " 7",
                    tagIds = new int[tagNameList.Count],
                    setPosition = 7
                };
                tagIdList.CopyTo(SavedSettings.copyTagsTagSets[6].tagIds);

                SavedSettings.copyTagsTagSets[7] = new TagSetType
                {
                    setName = "[08] " + GenericTagSetName + " 8",
                    tagIds = new int[tagNameList.Count],
                    setPosition = 8
                };
                tagIdList.CopyTo(SavedSettings.copyTagsTagSets[7].tagIds);

                SavedSettings.copyTagsTagSets[8] = new TagSetType
                {
                    setName = "[09] " + GenericTagSetName + " 9",
                    tagIds = new int[tagNameList.Count],
                    setPosition = 9
                };
                tagIdList.CopyTo(SavedSettings.copyTagsTagSets[8].tagIds);

                SavedSettings.copyTagsTagSets[9] = new TagSetType
                {
                    setName = "[10] " + GenericTagSetName + " 10",
                    tagIds = new int[tagNameList.Count],
                    setPosition = 10
                };
                tagIdList.CopyTo(SavedSettings.copyTagsTagSets[9].tagIds);
            }
            else
            {
                for (int i = 0; i < SavedSettings.copyTagsTagSets.Length; i++)
                    SavedSettings.copyTagsTagSets[i].setPosition = i + 1;
            }
            #endregion

            #region Final initialization
            //Final initialization
            MbForm = (Form)Control.FromHandle(MbApiInterface.MB_GetWindowHandle());
            MusicName = MbApiInterface.MB_GetLocalisation("Main.tree.Music", "Music");

            TagToolsSubmenu = (ToolStripMenuItem)MbApiInterface.MB_AddMenuItem("mnuTools/[1]" + PluginMenuGroup, null, null);
            if (!SavedSettings.dontShowContextMenu)
                TagToolsContextSubmenu = (ToolStripMenuItem)MbApiInterface.MB_AddMenuItem("context.Main/" + PluginMenuGroup, null, null);

            About.PluginInfoVersion = PluginInfoVersion;
            About.Name = PluginName;
            About.Description = Description;
            About.Author = "boroda";
            About.TargetApplication = "";   // current only applies to artwork, lyrics or instant messenger name that appears in the provider drop down selector or target Instant Messenger
            About.Type = PluginType.General;
            About.VersionMajor = (short)System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major;
            About.VersionMinor = (short)System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor;
            About.Revision = (short)System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build; // number of days since 2000-01-01 at build time
            About.MinInterfaceVersion = 39;
            About.MinApiRevision = 51;
            About.ReceiveNotifications = (ReceiveNotificationFlags.PlayerEvents | ReceiveNotificationFlags.TagEvents);
            About.ConfigurationPanelHeight = 0;   // height in pixels that musicbee should reserve in a panel for config settings. When set, a handle to an empty panel will be passed to the Configure function
            #endregion

            return About;
        }
        #endregion

        #region Other plugin interface methods
        public bool Configure(IntPtr panelHandle)
        {
            // save any persistent settings in a sub-folder of this path
            //string dataPath = mbApiInterface.Setting_GetPersistentStoragePath();
            // panelHandle will only be set if you set about.ConfigurationPanelHeight to a non-zero value
            // keep in mind the panel width is scaled according to the font the user has selected
            // if about.ConfigurationPanelHeight is set to 0, you can display your own popup window

            PluginSettings tagToolsForm = new PluginSettings(this, About);
            tagToolsForm.ShowDialog();

            SaveSettings();

            return true;
        }

        // called by MusicBee when the user clicks Apply or Save in the MusicBee Preferences screen.
        // its up to you to figure out whether anything has changed and needs updating
        public void SaveSettings()
        {
            Encoding unicode = Encoding.UTF8;
            FileStream stream = File.Open(PluginSettingsFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
            StreamWriter file = new StreamWriter(stream, unicode);

            System.Xml.Serialization.XmlSerializer controlsDefaultsSerializer = new System.Xml.Serialization.XmlSerializer(typeof(SavedSettingsType));

            controlsDefaultsSerializer.Serialize(file, SavedSettings);

            file.Close();
        }

        // MusicBee is closing the plugin (plugin is being disabled by user or MusicBee is shutting down)
        public void Close(PluginCloseReason reason)
        {
            PeriodicUI_RefreshTimer.Dispose();
            PeriodicUI_RefreshTimer = null;

            if (PeriodicAutobackupTimer != null)
            {
                PeriodicAutobackupTimer.Dispose();
                PeriodicAutobackupTimer = null;
            }

            EmptyButton.Dispose();


            lock (OpenedForms)
            {
                foreach (PluginWindowTemplate form in OpenedForms)
                {
                    form.backgroundTaskIsCanceled = true;
                }
            }

            if (!Uninstalled)
                SaveSettings();
        }

        // uninstall this plugin - clean up any persisted files
        public void Uninstall()
        {
            //Delete backups
            if (Directory.Exists(GetAutobackupDirectory(SavedSettings.autobackupDirectory)))
            {
                if (PeriodicAutobackupTimer != null)
                {
                    PeriodicAutobackupTimer.Dispose();
                    PeriodicAutobackupTimer = null;
                }

                lock (AutobackupLocker)
                {
                    Directory.Delete(GetAutobackupDirectory(SavedSettings.autobackupDirectory), true);
                }
            }

            //Delete settings file
            if (File.Exists(PluginSettingsFilePath))
            {
                File.Delete(PluginSettingsFilePath);
            }

            //Delete presets files
            string presetsPath = Path.Combine(MbApiInterface.Setting_GetPersistentStoragePath(), AsrPresetsDirectory);

            if (Directory.Exists(presetsPath))
            {
                Directory.Delete(presetsPath, true);
            }

            Uninstalled = true;
        }

        public static ToolStripMenuItem AddMenuItem(ToolStripMenuItem menuItemGroup, string itemName, string hotkeyDescription, EventHandler handler, bool enabled = true)
        {
            if (hotkeyDescription != null)
                MbApiInterface.MB_RegisterCommand(hotkeyDescription, handler);

            ToolStripItem menuItem = menuItemGroup.DropDown.Items.Add(itemName, null, handler);
            menuItem.Enabled = enabled;

            if (itemName == "-")
                return null;
            else
                return (ToolStripMenuItem)menuItem;
        }

        // receive event notifications from MusicBee
        // you need to set about.ReceiveNotificationFlags = PlayerEvents to receive all notifications, and not just the startup event
        public void ReceiveNotification(string sourceFileUrl, NotificationType type)
        {
            if (!PrioritySet)
            {
                PrioritySet = true;
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;
            }

            // perform some action depending on the notification type
            switch (type)
            {
                case NotificationType.PluginStartup:
                    // perform startup initialisation

                    //ASR init
                    Init();


                    //Let's create plugin main and context menu items
                    MbForm.Invoke((MethodInvoker)delegate { addPluginMenuItems(); });
                    MbForm.Invoke((MethodInvoker)delegate { addPluginContextMenuItems(); });


                    //Let's register ASR presets hotkeys and quick menu items 
                    MbForm.Invoke((MethodInvoker)delegate { RegisterASRPresetsHotkeysAndMenuItems(this); });

                    PeriodicUI_RefreshTimer = new System.Threading.Timer(regularUI_Refresh, null, RefreshUI_Delay, RefreshUI_Delay);

                    //(Auto)backup init
                    if (!SavedSettings.dontShowBackupRestore)
                    {
                        if (File.Exists(GetAutobackupDirectory(SavedSettings.autobackupDirectory) + @"\" + BackupIndexFileName))
                        {
                            FileStream stream = File.Open(GetAutobackupDirectory(SavedSettings.autobackupDirectory) + @"\" + BackupIndexFileName, FileMode.Open, FileAccess.Read, FileShare.None);
                            StreamReader file = new StreamReader(stream, Encoding.UTF8);

                            System.Xml.Serialization.XmlSerializer backupSerializer = new System.Xml.Serialization.XmlSerializer(typeof(BackupIndex));

                            try
                            {
                                BackupIndex = (BackupIndex)backupSerializer.Deserialize(file);
                            }
                            catch
                            {
                                MessageBox.Show(MbForm, MsgMasterBackupIndexIsCorrupted, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                BackupIndex = new BackupIndex();
                            }

                            file.Close();
                        }
                        else
                        {
                            BackupIndex = new BackupIndex();
                        }

                        if (!SavedSettings.dontShowBackupRestore && SavedSettings.autobackupInterval != 0)
                        {
                            PeriodicAutobackupTimer = new System.Threading.Timer(regularAutobackup, null, new TimeSpan(0, 0, (int)SavedSettings.autobackupInterval * 60), new TimeSpan(0, 0, (int)SavedSettings.autobackupInterval * 60));
                        }
                    }

                    //Auto rate at startup
                    if (SavedSettings.calculateThresholdsAtStartUp || SavedSettings.autoRateAtStartUp)
                    {
                        using (AutoRateCommand tagToolsForm = new AutoRateCommand(this))
                        {
                            tagToolsForm.switchOperation(tagToolsForm.onStartup, EmptyButton, EmptyButton, EmptyButton, EmptyButton, true, null);
                        }
                    }
                    if (SavedSettings.calculateAlbumRatingAtStartUp)
                    {
                        using (CalculateAverageAlbumRatingCommand tagToolsForm = new CalculateAverageAlbumRatingCommand(this))
                        {
                            tagToolsForm.calculateAlbumRatingForAllTracks();
                        }
                    }

                    ResourceManager resourceManager = Resources.ResourceManager;
                    MissingArtwork = (Bitmap)resourceManager.GetObject("MissingArtwork");


                    //Auto library reports on startup
                    AutoLibraryReportCommand.AutoCalculate(this);


                    //Let's refresh UI
                    MbApiInterface.MB_InvokeCommand((Command)4, null);

                    break;
                case NotificationType.TrackChanged:
                    InitialSkipCount = Convert.ToInt32(MbApiInterface.Library_GetFileProperty(sourceFileUrl, FilePropertyType.SkipCount));

                    break;
                case NotificationType.PlayCountersChanged:
                    //Play count should be changed, but track is not changed yet

                    if (SavedSettings.lastSkippedTagId != 0)
                    {
                        int newSkipCount = Convert.ToInt32(MbApiInterface.Library_GetFileProperty(sourceFileUrl, FilePropertyType.SkipCount));
                        if (newSkipCount != InitialSkipCount)
                        {
                            string lastSkippedDate;
                            DateTime now = DateTime.Now;

                            if (SavedSettings.lastSkippedDateFormat == 0)
                                lastSkippedDate = now.ToString("d");
                            else if (SavedSettings.lastSkippedDateFormat == 1)
                                lastSkippedDate = now.ToString("g");
                            else //if (SavedSettings.lastSkippedDateFormat == 2)
                                lastSkippedDate = now.ToString("G");

                            SetFileTag(sourceFileUrl, (MetaDataType)SavedSettings.lastSkippedTagId, lastSkippedDate, true);
                            CommitTagsToFile(sourceFileUrl, false, true);
                        }
                    }

                    if (!SavedSettings.dontShowASR)
                    {
                        bool autoApply = false;

                        lock (FilesUpdatedByPlugin)
                        {
                            if (FilesUpdatedByPlugin.Contains(sourceFileUrl))
                                FilesUpdatedByPlugin.Remove(sourceFileUrl);
                            else
                                autoApply = true;
                        }

                        if (autoApply)
                            AutoApply(sourceFileUrl, this);
                    }

                    if (SavedSettings.autoRateOnTrackProperties)
                    {
                        AutoRateCommand.AutoRateLive(this, sourceFileUrl);
                    }

                    NumberOfTagChanges++;


                    if (!SavedSettings.dontShowBackupRestore && SavedSettings.dontSkipAutobackupsIfPlayCountsChanged)
                    {
                        UpdateTrackForBackup(sourceFileUrl);
                    }

                    break;
                case NotificationType.TagsChanged:
                case NotificationType.FileAddedToInbox:
                case NotificationType.FileAddedToLibrary:
                    if (!SavedSettings.dontShowASR)
                    {
                        bool autoApply = false;

                        lock (FilesUpdatedByPlugin)
                        {
                            if (FilesUpdatedByPlugin.Contains(sourceFileUrl))
                                FilesUpdatedByPlugin.Remove(sourceFileUrl);
                            else
                                autoApply = true;
                        }

                        if (autoApply)
                            AutoApply(sourceFileUrl, this);
                    }

                    NumberOfTagChanges++;


                    if (!SavedSettings.dontShowBackupRestore)
                    {
                        UpdateTrackForBackup(sourceFileUrl);
                    }

                    break;
                case NotificationType.RatingChanged:
                    if (!SavedSettings.dontShowASR)
                    {
                        bool autoApply = false;

                        lock (FilesUpdatedByPlugin)
                        {
                            if (FilesUpdatedByPlugin.Contains(sourceFileUrl))
                                FilesUpdatedByPlugin.Remove(sourceFileUrl);
                            else
                                autoApply = true;
                        }

                        if (autoApply)
                            AutoApply(sourceFileUrl, this);
                    }

                    if (!SavedSettings.dontShowCAR)
                    {
                        if (SavedSettings.calculateAlbumRatingAtTagsChanged)
                            CalculateAverageAlbumRatingCommand.CalculateAlbumRatingForAlbum(this, sourceFileUrl);
                    }

                    NumberOfTagChanges++;


                    if (!SavedSettings.dontShowBackupRestore)
                    {
                        UpdateTrackForBackup(sourceFileUrl);
                    }

                    break;
                case NotificationType.ReplayGainChanged:
                    if (!SavedSettings.dontShowASR)
                    {
                        bool autoApply = false;

                        lock (FilesUpdatedByPlugin)
                        {
                            if (FilesUpdatedByPlugin.Contains(sourceFileUrl))
                                FilesUpdatedByPlugin.Remove(sourceFileUrl);
                            else
                                autoApply = true;
                        }

                        if (autoApply)
                            AutoApply(sourceFileUrl, this);
                    }

                    NumberOfTagChanges++;


                    if (!SavedSettings.dontShowBackupRestore)
                    {
                        UpdateTrackForBackup(sourceFileUrl);
                    }

                    break;
            }


            //Auto library reports
            if (NumberOfTagChanges >= SavedSettings.numberOfTagsToRecalculate)
                NumberOfTagChanges = -1;

            if (SavedSettings.recalculateOnNumberOfTagsChanges && NumberOfTagChanges == -1)
                AutoLibraryReportCommand.AutoCalculate(this);
        }

        public void addPluginMenuItems()
        {
            TagToolsSubmenu.DropDown.Items.Clear();

            if (!SavedSettings.dontShowBackupRestore) AddMenuItem(TagToolsSubmenu, TagToolsMenuSectionName, null, null, false);

            if (!SavedSettings.dontShowCopyTag) AddMenuItem(TagToolsSubmenu, CopyTagCommandName, CopyTagCommandDescription, copyTagEventHandler);
            if (!SavedSettings.dontShowSwapTags) AddMenuItem(TagToolsSubmenu, SwapTagsCommandName, SwapTagsCommandDescription, swapTagsEventHandler);
            if (!SavedSettings.dontShowChangeCase) AddMenuItem(TagToolsSubmenu, ChangeCaseCommandName, ChangeCaseCommandDescription, changeCaseEventHandler);
            if (!SavedSettings.dontShowRencodeTag)
            {
                AddMenuItem(TagToolsSubmenu, ReencodeTagCommandName, ReencodeTagCommandDescription, reencodeTagEventHandler);
                AddMenuItem(TagToolsSubmenu, ReencodeTagsCommandName, ReencodeTagsCommandDescription, reencodeTagsEventHandler);
            }
            if (!SavedSettings.dontShowLibraryReports) AddMenuItem(TagToolsSubmenu, LibraryReportsCommandName, LibraryReportsCommandDescription, libraryReportsEventHandler);
            if (!SavedSettings.dontShowLibraryReports) AddMenuItem(TagToolsSubmenu, AutoLibraryReportsCommandName, AutoLibraryReportsCommandDescription, autoLibraryReportsEventHandler);
            if (!SavedSettings.dontShowAutorate) AddMenuItem(TagToolsSubmenu, AutoRateCommandName, AutoRateCommandDescription, autoRateEventHandler);
            if (!SavedSettings.dontShowASR)
            {
                AddMenuItem(TagToolsSubmenu, AsrCommandName, AsrCommandDescription, asrEventHandler);
                if (ASRPresetsWithHotkeysCount > 0)
                    ASRPresetsMenuItem = AddMenuItem(TagToolsSubmenu, AsrCommandName.Replace("...", ""), null, null);
                AddMenuItem(TagToolsSubmenu, MsrCommandName, MsrCommandDescription, multipleSearchReplaceEventHandler);
            }
            if (!SavedSettings.dontShowCAR) AddMenuItem(TagToolsSubmenu, CarCommandName, CarCommandDescription, carEventHandler);
            if (!SavedSettings.dontShowCT) AddMenuItem(TagToolsSubmenu, CtCommandName, CtCommandDescription, compareTracksEventHandler);


            AddMenuItem(TagToolsSubmenu, "-", null, null);
            AddMenuItem(TagToolsSubmenu, CopyTagsToClipboardCommandName, CopyTagsToClipboardCommandDescription, copyTagsToClipboardEventHandler);
            CopyTagsToClipboardUsingMenuItem = AddMenuItem(TagToolsSubmenu, CopyTagsToClipboardUsingMenuDescription, null, null);
            MbForm.Invoke((MethodInvoker)delegate { addCopyTagsToClipboardUsingMenuItems(); });

            MbApiInterface.MB_RegisterCommand(TagToolsHotkeyDescription + CopyTagsToClipboardUsingMenuDescription + " [01]", copyTagsUsingTagSet1EventHandler);
            MbApiInterface.MB_RegisterCommand(TagToolsHotkeyDescription + CopyTagsToClipboardUsingMenuDescription + " [02]", copyTagsUsingTagSet2EventHandler);
            MbApiInterface.MB_RegisterCommand(TagToolsHotkeyDescription + CopyTagsToClipboardUsingMenuDescription + " [03]", copyTagsUsingTagSet3EventHandler);
            MbApiInterface.MB_RegisterCommand(TagToolsHotkeyDescription + CopyTagsToClipboardUsingMenuDescription + " [04]", copyTagsUsingTagSet4EventHandler);
            MbApiInterface.MB_RegisterCommand(TagToolsHotkeyDescription + CopyTagsToClipboardUsingMenuDescription + " [05]", copyTagsUsingTagSet5EventHandler);
            MbApiInterface.MB_RegisterCommand(TagToolsHotkeyDescription + CopyTagsToClipboardUsingMenuDescription + " [06]", copyTagsUsingTagSet6EventHandler);
            MbApiInterface.MB_RegisterCommand(TagToolsHotkeyDescription + CopyTagsToClipboardUsingMenuDescription + " [07]", copyTagsUsingTagSet7EventHandler);
            MbApiInterface.MB_RegisterCommand(TagToolsHotkeyDescription + CopyTagsToClipboardUsingMenuDescription + " [08]", copyTagsUsingTagSet8EventHandler);
            MbApiInterface.MB_RegisterCommand(TagToolsHotkeyDescription + CopyTagsToClipboardUsingMenuDescription + " [09]", copyTagsUsingTagSet9EventHandler);
            MbApiInterface.MB_RegisterCommand(TagToolsHotkeyDescription + CopyTagsToClipboardUsingMenuDescription + " [10]", copyTagsUsingTagSet10EventHandler);

            AddMenuItem(TagToolsSubmenu, PasteTagsFromClipboardCommandName, PasteTagsFromClipboardCommandDescription, pasteTagsFromClipboardEventHandler);


            if (!SavedSettings.dontShowBackupRestore)
            {
                AddMenuItem(TagToolsSubmenu, "-", null, null);
                AddMenuItem(TagToolsSubmenu, BackupRestoreMenuSectionName, null, null, false);
                AddMenuItem(TagToolsSubmenu, BackupTagsCommandName, BackupTagsCommandDescription, backupTagsEventHandler);
                AddMenuItem(TagToolsSubmenu, RestoreTagsCommandName, RestoreTagsCommandDescription, restoreTagsEventHandler);
                AddMenuItem(TagToolsSubmenu, RestoreTagsForEntireLibraryCommandName, RestoreTagsForEntireLibraryCommandDescription, restoreTagsForEntireLibraryEventHandler);
                AddMenuItem(TagToolsSubmenu, RenameMoveBackupCommandName, RenameMoveBackupCommandDescription, renameMoveBackupEventHandler);
                AddMenuItem(TagToolsSubmenu, MoveBackupsCommandName, MoveBackupsCommandDescription, moveBackupsEventHandler);
                AddMenuItem(TagToolsSubmenu, CreateNewBaselineCommandName, CreateNewBaselineCommandDescription, createNewBaselineEventHandler);
                AddMenuItem(TagToolsSubmenu, DeleteBackupsCommandName, DeleteBackupsCommandDescription, deleteBackupsEventHandler);
                AddMenuItem(TagToolsSubmenu, "-", null, null);
                AddMenuItem(TagToolsSubmenu, AutoBackupSettingsCommandName, AutoBackupSettingsCommandDescription, autoBackupSettingsEventHandler);
            }

            if (!SavedSettings.dontShowShowHiddenWindows)
            {
                AddMenuItem(TagToolsSubmenu, "-", null, null);
                AddMenuItem(TagToolsSubmenu, ShowHiddenCommandName, ShowHiddenCommandDescription, showHiddenEventHandler);
            }
        }

        public void addPluginContextMenuItems()
        {
            if (TagToolsContextSubmenu == null)
                return;


            TagToolsContextSubmenu.DropDown.Items.Clear();

            if (!SavedSettings.dontShowBackupRestore) AddMenuItem(TagToolsContextSubmenu, TagToolsMenuSectionName, null, null, false);

            if (!SavedSettings.dontShowCopyTag) AddMenuItem(TagToolsContextSubmenu, CopyTagCommandName, null, copyTagEventHandler);
            if (!SavedSettings.dontShowSwapTags) AddMenuItem(TagToolsContextSubmenu, SwapTagsCommandName, null, swapTagsEventHandler);
            if (!SavedSettings.dontShowChangeCase) AddMenuItem(TagToolsContextSubmenu, ChangeCaseCommandName, null, changeCaseEventHandler);
            if (!SavedSettings.dontShowASR)
            {
                AddMenuItem(TagToolsContextSubmenu, AsrCommandName, null, asrEventHandler);
                if (ASRPresetsWithHotkeysCount > 0)
                    ASRPresetsContextMenuItem = AddMenuItem(TagToolsContextSubmenu, AsrCommandName.Replace("...", ""), null, null);
                AddMenuItem(TagToolsContextSubmenu, MsrCommandName, null, multipleSearchReplaceEventHandler);
            }
            if (!SavedSettings.dontShowCT) AddMenuItem(TagToolsContextSubmenu, CtCommandName, null, compareTracksEventHandler);
            AddMenuItem(TagToolsContextSubmenu, "-", null, null);
            AddMenuItem(TagToolsContextSubmenu, CopyTagsToClipboardCommandName, null, copyTagsToClipboardEventHandler);
            CopyTagsToClipboardUsingContextMenuItem = AddMenuItem(TagToolsContextSubmenu, CopyTagsToClipboardUsingMenuDescription, null, null);
            MbForm.Invoke((MethodInvoker)delegate { addCopyTagsToClipboardUsingContextMenuItems(); });
            AddMenuItem(TagToolsContextSubmenu, PasteTagsFromClipboardCommandName, null, pasteTagsFromClipboardEventHandler);

            if (!SavedSettings.dontShowBackupRestore)
            {
                AddMenuItem(TagToolsContextSubmenu, "-", null, null);
                AddMenuItem(TagToolsContextSubmenu, BackupRestoreMenuSectionName, null, null, false);

                MbApiInterface.MB_RegisterCommand(TagHistoryCommandDescription, tagHistoryEventHandler);
                AddMenuItem(TagToolsContextSubmenu, TagHistoryCommandName, null, tagHistoryEventHandler);
            }

            if (!SavedSettings.dontShowShowHiddenWindows)
            {
                AddMenuItem(TagToolsContextSubmenu, "-", null, null);
                AddMenuItem(TagToolsContextSubmenu, ShowHiddenCommandName, null, showHiddenEventHandler);
            }
        }

        public void addCopyTagsToClipboardUsingMenuItems()
        {
            CopyTagsToClipboardUsingMenuItem.DropDown.Items.Clear();

            CopyTagsToClipboardUsingMenuItem.DropDown.Items.Add(SavedSettings.copyTagsTagSets[0].setName, null, copyTagsUsingTagSet1EventHandler);
            CopyTagsToClipboardUsingMenuItem.DropDown.Items.Add(SavedSettings.copyTagsTagSets[1].setName, null, copyTagsUsingTagSet2EventHandler);
            CopyTagsToClipboardUsingMenuItem.DropDown.Items.Add(SavedSettings.copyTagsTagSets[2].setName, null, copyTagsUsingTagSet3EventHandler);
            CopyTagsToClipboardUsingMenuItem.DropDown.Items.Add(SavedSettings.copyTagsTagSets[3].setName, null, copyTagsUsingTagSet4EventHandler);
            CopyTagsToClipboardUsingMenuItem.DropDown.Items.Add(SavedSettings.copyTagsTagSets[4].setName, null, copyTagsUsingTagSet5EventHandler);
            CopyTagsToClipboardUsingMenuItem.DropDown.Items.Add(SavedSettings.copyTagsTagSets[5].setName, null, copyTagsUsingTagSet6EventHandler);
            CopyTagsToClipboardUsingMenuItem.DropDown.Items.Add(SavedSettings.copyTagsTagSets[6].setName, null, copyTagsUsingTagSet7EventHandler);
            CopyTagsToClipboardUsingMenuItem.DropDown.Items.Add(SavedSettings.copyTagsTagSets[7].setName, null, copyTagsUsingTagSet8EventHandler);
            CopyTagsToClipboardUsingMenuItem.DropDown.Items.Add(SavedSettings.copyTagsTagSets[8].setName, null, copyTagsUsingTagSet9EventHandler);
            CopyTagsToClipboardUsingMenuItem.DropDown.Items.Add(SavedSettings.copyTagsTagSets[9].setName, null, copyTagsUsingTagSet10EventHandler);
        }

        public void addCopyTagsToClipboardUsingContextMenuItems()
        {
            if (CopyTagsToClipboardUsingContextMenuItem != null)
            {
                CopyTagsToClipboardUsingContextMenuItem.DropDown.Items.Clear();

                CopyTagsToClipboardUsingContextMenuItem.DropDown.Items.Add(SavedSettings.copyTagsTagSets[0].setName, null, copyTagsUsingTagSet1EventHandler);
                CopyTagsToClipboardUsingContextMenuItem.DropDown.Items.Add(SavedSettings.copyTagsTagSets[1].setName, null, copyTagsUsingTagSet2EventHandler);
                CopyTagsToClipboardUsingContextMenuItem.DropDown.Items.Add(SavedSettings.copyTagsTagSets[2].setName, null, copyTagsUsingTagSet3EventHandler);
                CopyTagsToClipboardUsingContextMenuItem.DropDown.Items.Add(SavedSettings.copyTagsTagSets[3].setName, null, copyTagsUsingTagSet4EventHandler);
                CopyTagsToClipboardUsingContextMenuItem.DropDown.Items.Add(SavedSettings.copyTagsTagSets[4].setName, null, copyTagsUsingTagSet5EventHandler);
                CopyTagsToClipboardUsingContextMenuItem.DropDown.Items.Add(SavedSettings.copyTagsTagSets[5].setName, null, copyTagsUsingTagSet6EventHandler);
                CopyTagsToClipboardUsingContextMenuItem.DropDown.Items.Add(SavedSettings.copyTagsTagSets[6].setName, null, copyTagsUsingTagSet7EventHandler);
                CopyTagsToClipboardUsingContextMenuItem.DropDown.Items.Add(SavedSettings.copyTagsTagSets[7].setName, null, copyTagsUsingTagSet8EventHandler);
                CopyTagsToClipboardUsingContextMenuItem.DropDown.Items.Add(SavedSettings.copyTagsTagSets[8].setName, null, copyTagsUsingTagSet9EventHandler);
                CopyTagsToClipboardUsingContextMenuItem.DropDown.Items.Add(SavedSettings.copyTagsTagSets[9].setName, null, copyTagsUsingTagSet10EventHandler);
            }
        }

        public string CustomFunc_ASR(string url, string presetId)
        {
            if (presetId == "")
                return "";

            return GetLastReplacedTag(url, AsrIdsPresets[presetId]) ?? "";
        }

        public string CustomFunc_ALR(string url, string functionId)
        {
            if (functionId == "")
                return "";

            return AutoLibraryReportCommand.AutoCalculatePresetFunction(this, url, functionId) ?? "";
        }

        public string CustomFunc_Random(string max_number)
        {
            return RandomGenerator.Next(int.Parse(max_number) + 1).ToString("D" + Math.Ceiling((decimal)(Math.Log10(float.Parse(max_number) + 1))));
        }

         public string CustomFunc_Round(string number, string number_of_digits)
        {
            number = number.Replace('.', LocalizedDecimalPoint);
            number_of_digits = number_of_digits.Replace('.', LocalizedDecimalPoint);

            if (int.Parse(number_of_digits) > 0)
                return Math.Round(decimal.Parse(number), int.Parse(number_of_digits)).ToString("F" + number_of_digits);
            else
                return Math.Round(decimal.Parse(number), int.Parse(number_of_digits)).ToString();
        }

        public string CustomFunc_RoundUp(string number, string number_of_digits)
        {
            number = number.Replace('.', LocalizedDecimalPoint);
            number_of_digits = number_of_digits.Replace('.', LocalizedDecimalPoint);

            if (int.Parse(number_of_digits) > 0)
                return (Math.Ceiling(decimal.Parse(number) * (decimal)Math.Pow(10, int.Parse(number_of_digits))) / (decimal)Math.Pow(10, int.Parse(number_of_digits))).ToString();
            else
                return Math.Ceiling(decimal.Parse(number)).ToString();
        }

        public string CustomFunc_RoundDown(string number, string number_of_digits)
        {
            number = number.Replace('.', LocalizedDecimalPoint);
            number_of_digits = number_of_digits.Replace('.', LocalizedDecimalPoint);

            if (int.Parse(number_of_digits) > 0)
                return (Math.Floor(decimal.Parse(number) * (decimal)Math.Pow(10, int.Parse(number_of_digits))) / (decimal)Math.Pow(10, int.Parse(number_of_digits))).ToString();
            else
                return Math.Floor(decimal.Parse(number)).ToString();
        }

        public string CustomFunc_Sqrt(string number)
        {
            number = number.Replace('.', LocalizedDecimalPoint);

            return Math.Sqrt(float.Parse(number)).ToString();
        }

        public string CustomFunc_eq(string number1, string number2)
        {
            number1 = number1.Replace('.', LocalizedDecimalPoint);
            number2 = number2.Replace('.', LocalizedDecimalPoint);

            if (decimal.Parse(number1) == decimal.Parse(number2))
                return "T";
            else
                return "F";
        }

        public string CustomFunc_ne(string number1, string number2)
        {
            number1 = number1.Replace('.', LocalizedDecimalPoint);
            number2 = number2.Replace('.', LocalizedDecimalPoint);

            if (decimal.Parse(number1) != decimal.Parse(number2))
                return "T";
            else
                return "F";
        }

        public string CustomFunc_gt(string number1, string number2)
        {
            number1 = number1.Replace('.', LocalizedDecimalPoint);
            number2 = number2.Replace('.', LocalizedDecimalPoint);

            if (decimal.Parse(number1) > decimal.Parse(number2))
                return "T";
            else
                return "F";
        }

        public string CustomFunc_lt(string number1, string number2)
        {
            number1 = number1.Replace('.', LocalizedDecimalPoint);
            number2 = number2.Replace('.', LocalizedDecimalPoint);

            if (decimal.Parse(number1) < decimal.Parse(number2))
                return "T";
            else
                return "F";
        }

        public string CustomFunc_ge(string number1, string number2)
        {
            number1 = number1.Replace('.', LocalizedDecimalPoint);
            number2 = number2.Replace('.', LocalizedDecimalPoint);

            if (decimal.Parse(number1) >= decimal.Parse(number2))
                return "T";
            else
                return "F";
        }

        public string CustomFunc_le(string number1, string number2)
        {
            number1 = number1.Replace('.', LocalizedDecimalPoint);
            number2 = number2.Replace('.', LocalizedDecimalPoint);

            if (decimal.Parse(number1) <= decimal.Parse(number2))
                return "T";
            else
                return "F";
        }

        public string CustomFunc_AddDuration(string duration1, string duration2)
        {
            return (TimeSpan.Parse(duration1) + TimeSpan.Parse(duration2)).ToString();
        }

        public string CustomFunc_SubDuration(string duration1, string duration2)
        {
            return (TimeSpan.Parse(duration1) - TimeSpan.Parse(duration2)).ToString();
        }

        public string CustomFunc_MulDuration(string duration, string number)
        {
            return TimeSpan.FromSeconds(Math.Round(double.Parse(number) * TimeSpan.Parse(duration).TotalSeconds)).ToString();
        }

        public string CustomFunc_SubDateTime(string datetime1, string datetime2)
        {
            return (DateTime.Parse(datetime1) - DateTime.Parse(datetime2)).ToString();
        }

        public string CustomFunc_NumberOfDays(string datetime1, string datetime2)
        {
            if (datetime2.Length == 4)
            {
                datetime2 = (new DateTime(int.Parse(datetime2), 01, 01)).ToString();
            }

            return Math.Floor((DateTime.Parse(datetime1) - DateTime.Parse(datetime2)).TotalDays).ToString();
        }

        public string CustomFunc_AddDurationToDateTime(string datetime, string duration)
        {
            return (DateTime.Parse(datetime) + TimeSpan.Parse(duration)).ToString();
        }

        public string CustomFunc_SubDurationFromDateTime(string datetime, string duration)
        {
            return (DateTime.Parse(datetime) - TimeSpan.Parse(duration)).ToString();
        }

        public string CustomFunc_Now()
        {
            return DateTime.Now.ToString();
        }

        public string CustomFunc_TitleCase(string input)
        {
            string[] exceptionWords = SavedSettings.exceptionWordsASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            input = ChangeCaseCommand.ChangeWordsCase(input, ChangeCaseCommand.ChangeCaseOptions.lowerCase, null, false,
                null, SavedSettings.wordSplittersASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            string result = ChangeCaseCommand.ChangeWordsCase(input, ChangeCaseCommand.ChangeCaseOptions.titleCase, exceptionWords, false,
                SavedSettings.exceptionCharsASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), SavedSettings.wordSplittersASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), true, true);

            return result;
        }

        public string CustomFunc_SentenceCase(string input)
        {
            string[] exceptionWords = SavedSettings.exceptionWordsASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            input = ChangeCaseCommand.ChangeWordsCase(input, ChangeCaseCommand.ChangeCaseOptions.lowerCase, null, false,
                null, SavedSettings.wordSplittersASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            string result = ChangeCaseCommand.ChangeWordsCase(input, ChangeCaseCommand.ChangeCaseOptions.sentenceCase, exceptionWords, false,
                SavedSettings.exceptionCharsASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), SavedSettings.wordSplittersASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), true, false);

            return result;
        }

        public string CustomFunc_Name(string parameter)
        {
            return Regex.Replace(parameter, @"^(.*\\)?(.*)\..*", "$2");
        }

        public string CustomFunc_DateCreated(string url)
        {
            FileInfo fileInfo = new FileInfo(url);
            if (fileInfo.Exists)
                return fileInfo.CreationTime.ToString("d");
            else
                return "";
        }

        public string CustomFunc_Char(string code)
        {
            ushort charcode = ushort.Parse(code, System.Globalization.NumberStyles.HexNumber);
            return ((char)charcode).ToString();
        }

        public string CustomFunc_TagContainsAnyString(string url, string tagName, string text)
        {
            MetaDataType tagId = GetTagId(tagName);

            string tagValue = GetFileTag(url, tagId);
            string[] textParts = text.Split('|');

            foreach (string textPart in textParts)
            {
                if (Regex.IsMatch(tagValue, textPart, RegexOptions.IgnoreCase))
                    return "T";
            }

            return "F";
        }

        public string CustomFunc_TagContainsAllStrings(string url, string tagName, string text)
        {
            MetaDataType tagId = GetTagId(tagName);

            string tagValue = GetFileTag(url, tagId);
            string[] textParts = text.Split('|');

            string result = "T";
            foreach (string textPart in textParts)
            {
                if (!Regex.IsMatch(tagValue, textPart, RegexOptions.IgnoreCase))
                {
                    result = "F";
                    break;
                }
            }

            return result;
        }
        #endregion
    }
    #endregion
}
