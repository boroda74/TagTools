﻿using ExtensionMethods;
using MusicBeePlugin.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using static MusicBeePlugin.AdvancedSearchAndReplace;
using static MusicBeePlugin.LibraryReports;


namespace MusicBeePlugin
{
    #region Custom types
    public class WindowSettingsType
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

        public int splitterDistance;

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

    public enum LrFunctionType
    {
        Grouping = 0,
        Count,
        Sum,
        Minimum,
        Maximum,
        Average,
        AverageCount
    }

    public enum LrReportFormat
    {
        HtmlDocumentByAlbums = 1,
        HtmlDocument = 2,
        HtmlTable = 3,
        TabSeparatedText = 4,
        M3u = 5,
        Csv = 6,
        HtmlDocumentCdBooklet = 7,
        HtmlDocumentAlbumGrid = 8
    }
    #endregion

    #region Main module
    partial class Plugin
    {
        #region Members
        internal static bool DeveloperMode;
        private static bool Uninstalled;

        internal static MusicBeeApiInterface MbApiInterface;
        private static readonly PluginInfo About = new PluginInfo();

        internal static string PluginVersion;

        internal static ToolStripMenuItem OpenedFormsSubmenu;

        //Skinning
        internal static bool PluginClosing;//***

        internal static float ButtonHeightDpiFontScaling = 1;
        internal static float TextBoxHeightDpiFontScaling = 1;
        internal static float DpiScaling = 1;

        internal static bool SizesColorsChanged = true;

        internal static Color FormForeColor;
        internal static Color FormBackColor;
        internal static Color FormBorderColor;

        internal static Color ControlHighlightForeColor;
        internal static Color ControlHighlightBackColor;

        internal static bool UseNativeButtonPaint = true;

        internal static Color ButtonFocusedBorderColor;
        internal static Color ButtonBorderColor;
        internal static Color ButtonDisabledBorderColor;

        internal static Color ButtonForeColor;
        internal static Color ButtonBackColor;

        internal static Color ButtonMouseOverBackColor;

        internal static Color ButtonDisabledForeColor;
        internal static Color ButtonDisabledBackColor;

        internal static Color InputPanelForeColor;
        internal static Color InputPanelBackColor;
        internal static Color InputPanelBorderColor;

        internal static Color InputControlForeColor;
        internal static Color InputControlBackColor;
        internal static Color InputControlBorderColor;
        internal static Color InputControlFocusedBorderColor;

        internal static Color InputControlDimmedForeColor;
        internal static Color InputControlDimmedBackColor;

        internal static Color AccentColor;
        internal static Color AccentSelectedColor;
        internal static Color DimmedAccentColor;
        internal static Color DeepDimmedAccentColor;

        internal static Color DimmedHighlight;

        internal static Color SplitterColor;


        internal static readonly Color NoColor = Color.FromArgb(-2); //****

        internal static Color ScrollBarBackColor = NoColor;
        internal static Color ScrollBarBorderColor = NoColor;
        internal static Color NarrowScrollBarBackColor = NoColor;

        internal static Color ScrollBarThumbAndSpansForeColor = NoColor;
        internal static Color ScrollBarThumbAndSpansBackColor = NoColor;
        internal static Color ScrollBarThumbAndSpansBorderColor = NoColor;

        internal static Color ScrollBarFocusedBorderColor = NoColor;


        //Themed colors for ASR/LR
        internal static Color HighlightColor;

        internal static Color UntickedColor;
        internal static Color TickedColor;


        internal const float ScrollBarWidth = 10.6f;//Not yet DPI scaled
        internal static int SbBorderWidth = 1; //scaledPx;


        internal static Bitmap DisabledDownArrowComboBoxImage;
        internal static Bitmap DownArrowComboBoxImage;

        internal static Bitmap UpArrowImage;
        internal static Bitmap DownArrowImage;

        internal static Bitmap ThumbTopImage;
        internal static Bitmap ThumbMiddleVerticalImage;
        internal static Bitmap ThumbBottomImage;


        internal static Bitmap LeftArrowImage;
        internal static Bitmap RightArrowImage;

        internal static Bitmap ThumbLeftImage;
        internal static Bitmap ThumbMiddleHorizontalImage;
        internal static Bitmap ThumbRightImage;


        //internal static Bitmap ButtonSetImage;
        //internal static Bitmap DisabledButtonSetImage;

        internal static Bitmap CheckedState;
        internal static Bitmap UncheckedState;

        internal static Bitmap WarningWide;

        internal static Bitmap ErrorWide;
        internal static Bitmap FatalErrorWide;
        internal static Bitmap ErrorFatalErrorWide;


        internal static Bitmap Warning;

        internal static Bitmap Error;
        internal static Bitmap FatalError;
        internal static Bitmap ErrorFatalError;


        internal static Bitmap Search;
        internal static Bitmap Window;

        internal static Bitmap ClearField;
        internal static Bitmap Gear;

        internal static Bitmap AutoAppliedPresetsAccent;
        internal static Bitmap AutoAppliedPresetsDimmed;

        internal static Bitmap PredefinedPresetsAccent;
        internal static Bitmap PredefinedPresetsDimmed;

        internal static Bitmap CustomizedPresetsAccent;
        internal static Bitmap CustomizedPresetsDimmed;

        internal static Bitmap UserPresetsAccent;
        internal static Bitmap UserPresetsDimmed;

        internal static Bitmap PlaylistPresetsAccent;
        internal static Bitmap PlaylistPresetsDimmed;

        internal static Bitmap FunctionIdPresetsAccent;
        internal static Bitmap FunctionIdPresetsDimmed;

        internal static Bitmap HotkeyPresetsAccent;
        internal static Bitmap HotkeyPresetsDimmed;

        internal static Bitmap UncheckAllFiltersAccent;
        internal static Bitmap UncheckAllFiltersDimmed;


        internal static string EnabledState = "+";
        internal static string DisabledState = "-";


        internal const int ActionRetryDelay = 500; //milliseconds.

        internal static string LrCachedFunctionResultPresetSeparator = "\ufffc"; //Object replacement character, it will be hardly used in tags
        internal static string LrCachedFunctionResultLibraryPathHashSeparator = "\ufffd";//Replacement character, it will be hardly used in tags
        internal static string LrCurrentLibraryPathHash = null;
        internal static int LrLastAskedCacheFillLibraryPathHash = -1;

        internal static object LrPresetExecutionLocker = new object();


        internal const int MaximumNumberOfAsrHotkeys = 20;
        internal const int MaximumNumberOfLrHotkeys = 20;
        internal const int PredefinedReportPresetCount = 5;

        internal static readonly char LocalizedDecimalPoint = (0.5).ToString()[1];
        internal static Bitmap MissingArtwork;
        private static readonly Random RandomGenerator = new Random();

        internal static Form MbForm;
        internal static List<PluginWindowTemplate> OpenedForms = new List<PluginWindowTemplate>();
        internal static int NumberOfNativeMbBackgroundTasks = 0;

        private static bool PrioritySet; //MusicBeePlugin's background thread priority must be set to "lower"

        internal static bool DisablePlaySoundOnce = false;

        internal static bool BackupIsAlwaysNeeded = true;
        internal static SortedDictionary<int, bool> TracksNeededToBeBackedUp = new SortedDictionary<int, bool>();
        internal static SortedDictionary<int, bool> TempTracksNeededToBeBackedUp = new SortedDictionary<int, bool>();
        internal static int UpdatedTracksForBackupCount;
        internal const int MaxUpdatedTracksCount = 5000;

        internal static string MusicName;

        private static string LastMessage = string.Empty;

        internal static SortedDictionary<string, MetaDataType> TagNamesIds = new SortedDictionary<string, MetaDataType>();
        internal static Dictionary<MetaDataType, string> TagIdsNames = new Dictionary<MetaDataType, string>();

        internal static SortedDictionary<string, FilePropertyType> PropNamesIds = new SortedDictionary<string, FilePropertyType>();
        internal static Dictionary<FilePropertyType, string> PropIdsNames = new Dictionary<FilePropertyType, string>();

        internal static readonly List<string> FilesUpdatedByPlugin = new List<string>();
        internal static readonly List<string> ChangedFiles = new List<string>();

        private static System.Threading.Timer PeriodicUiRefreshTimer;
        private static System.Threading.Timer DelayedStatusBarTextClearingTimer;
        private static bool UiRefreshingIsNeeded;
        private static TimeSpan RefreshUI_Delay = new TimeSpan(0, 0, 5);
        internal static DateTime LastUI_Refresh = DateTime.MinValue;
        private static readonly object LastUI_RefreshLocker = new object();

        internal static System.Threading.Timer PeriodicAutoBackupTimer;

        internal static Button EmptyButton;
        internal static Form EmptyForm;

        internal static string[] ReadonlyTagsNames;
        internal static string[] CustomTagNames;

        internal static string Language;
        internal static string PluginsPath;

        private static readonly string PluginSettingsFileName = "mb_TagTools.settings.xml";
        private static string PluginSettingsFilePath;

        private static readonly string PluginHelpFileName = "Additional-Tagging-Tools";
        private static string PluginHelpFilePath;

        private static readonly string PluginWebPage = "https://getmusicbee.com/addons/plugins/49/additional-tagging-amp-reporting-tools/";

        internal const string BackupIndexFileName = ".Master Tag Index.mbi";
        internal static string BackupDefaultPrefix = "Tag Backup ";
        internal static BackupIndex BackupIndex;

        internal const int StatusBarTextUpdateInterval = 50;

        internal static List<Preset> AsrAutoAppliedPresets = new List<Preset>();
        internal const string AsrPresetsDirectory = "ASR Presets";
        internal const string AsrPresetExtension = ".asr-preset.xml";
        internal static string AsrPresetNaming = "ASR preset";
        internal static Preset[] AsrPresetsWithHotkeys = new Preset[MaximumNumberOfAsrHotkeys];
        internal static int AsrPresetsWithHotkeysCount = 0;

        internal static SortedDictionary<string, Preset> IdsAsrPresets = new SortedDictionary<string, Preset>();
        internal static Preset MSR;


        internal static SortedDictionary<Guid, bool> AllLrPresetGuidsInUse = new SortedDictionary<Guid, bool>(); //<guid, false>: NOT permanentGuid!
        internal static SortedDictionary<int, string> LrTrackCacheNeededToBeUpdated = new SortedDictionary<int, string>(); //<Track persistent id, URL>
        internal static SortedDictionary<Guid, SortedDictionary<int, SortedDictionary<string, bool>[]>> PresetChangingGroupingTagsRaw //Dictionaries of tags values,
                                                                                                                                      //arrays of groupings per track id
                                                                                                                                      //per preset
            = new SortedDictionary<Guid, SortedDictionary<int, SortedDictionary<string, bool>[]>>();
        internal static SortedDictionary<Guid, SortedDictionary<int, SortedDictionary<string, bool>[]>> PresetChangingActualGroupingTags
            = new SortedDictionary<Guid, SortedDictionary<int, SortedDictionary<string, bool>[]>>();
        internal static SortedDictionary<Guid, SortedDictionary<int, SortedDictionary<string, bool>[]>> PresetChangingActualGroupingTagsRaw
            = new SortedDictionary<Guid, SortedDictionary<int, SortedDictionary<string, bool>[]>>();

        internal static string[] LrTrackCacheNeededToBeUpdatedWorkingCopy = new string[0]; //URL[]
        internal static SortedDictionary<Guid, SortedDictionary<string, bool>[]> ChangingGroupingTagsRawWorkingCopy  //Dictionaries of tags values,
            = new SortedDictionary<Guid, SortedDictionary<string, bool>[]>();                                        //arrays of groupings per preset
        internal static SortedDictionary<Guid, SortedDictionary<string, bool>[]> ChangingActualGroupingTagsWorkingCopy
            = new SortedDictionary<Guid, SortedDictionary<string, bool>[]>();
        internal static SortedDictionary<Guid, SortedDictionary<string, bool>[]> ChangingActualGroupingTagsRawWorkingCopy
            = new SortedDictionary<Guid, SortedDictionary<string, bool>[]>();

        internal static System.Threading.Timer UpdateFunctionCacheTimer = null;
        internal static readonly TimeSpan FunctionCacheUpdateDelay = new TimeSpan(0, 0, 5); //Every 5 secs. //****
        internal static readonly TimeSpan FunctionCacheClearingDelay = new TimeSpan(0, 1, 0); //Every 1 min. //****
        internal static readonly int PresetCacheCountThreshold = 10;
        internal static readonly int PresetCacheCountCriticalThreshold = 100;
        internal static readonly int TagsCacheClearingGroupingsCountThreshold = 20_000;


        internal static LibraryReports LibraryReportsCommandForHotkeys;
        internal static ReportPreset[] ReportPresetsWithHotkeys = new ReportPreset[MaximumNumberOfLrHotkeys];
        internal static int LrPresetsWithHotkeysCount = 0;

        internal static LibraryReports LibraryReportsCommandForAutoApplying;
        internal static List<ReportPreset> ReportPresetsForAutoApplying = new List<ReportPreset>();

        internal static LibraryReports LibraryReportsCommandForFunctionIds;
        internal static SortedDictionary<string, ReportPreset> IdsReportPresets = new SortedDictionary<string, ReportPreset>();
        internal static bool ReportPresetIdsAreInitialized = false;


        internal const char MultipleItemsSplitterId = '\u0000';
        internal const char GuestId = '\u0001';
        internal const char PerformerId = '\u0002';
        internal const char RemixerId = '\u0004';
        internal const char EndOfStringId = '\u0008';

        internal const string TotalsString = "\u0001";

        internal static string LastCommandSbText;
        private static bool LastPreview;
        private static int LastFileCounter;
        private static int LastFileCounterThreshold;
        private static int LastFileCounterTotal;

        internal static ToolStripMenuItem TagToolsSubmenu;
        internal static ToolStripMenuItem TagToolsContextSubmenu;

        internal static ToolStripMenuItem CopyTagsToClipboardUsingMenuItem;
        internal static ToolStripMenuItem CopyTagsToClipboardUsingContextMenuItem;
        internal static ToolStripMenuItem AsrPresetsMenuItem;
        internal static ToolStripMenuItem AsrPresetsContextMenuItem;
        internal static ToolStripMenuItem LrPresetsMenuItem;
        internal static ToolStripMenuItem LrPresetsContextMenuItem;

        private int InitialSkipCount;
        #endregion

        #region Service tags and their localized names
        //Some workarounds
        internal const MetaDataType DisplayedArtistId = (MetaDataType)(-1);
        internal const MetaDataType ArtistArtistsId = (MetaDataType)(-2);
        internal const MetaDataType DisplayedComposerId = (MetaDataType)(-3);
        internal const MetaDataType ComposerComposersId = (MetaDataType)(-4);
        internal const MetaDataType LyricsId = (MetaDataType)(-5);
        internal const MetaDataType SynchronisedLyricsId = (MetaDataType)(-6);
        internal const MetaDataType UnsynchronisedLyricsId = (MetaDataType)(-7);

        internal const MetaDataType AllTagsPseudoTagId = (MetaDataType)(-22);

        internal const MetaDataType NullTagId = (MetaDataType)(-20);
        internal const MetaDataType DateCreatedTagId = (MetaDataType)(-21);

        internal const MetaDataType ClipboardTagId = (MetaDataType)(-150);
        internal const MetaDataType FolderTagId = (MetaDataType)(-151);
        internal const MetaDataType FileNameTagId = (MetaDataType)(-152);
        internal const MetaDataType FilePathTagId = (MetaDataType)(-153);
        internal const MetaDataType FilePathWoExtTagId = (MetaDataType)(-154);

        internal static string DisplayedArtistName;
        internal static string ArtistArtistsName;
        internal static string DisplayedComposerName;
        internal static string ComposerComposersName;
        internal static string DisplayedAlbumArtistName;
        internal static string ArtworkName;
        internal static string LyricsName;
        internal static string LyricsNamePostfix;
        internal static string SynchronisedLyricsName;
        internal static string UnsynchronisedLyricsName;

        internal static string NullTagName;
        internal static string AllTagsPseudoTagName;

        internal static string FolderTagName;
        internal static string FileNameTagName;
        internal static string FilePathTagName;
        internal static string FilePathWoExtTagName;
        internal static string AlbumUniqueIdName;

        internal static string GenericTagSetName;
        #endregion


        #region Defaults for controls
        //Defaults for controls
        internal static DataGridViewCellStyle HeaderCellStyle = new DataGridViewHeaderCell().Style;

        internal static DataGridViewCellStyle UnchangedCellStyle = new DataGridViewCellStyle();
        internal static DataGridViewCellStyle ChangedCellStyle = new DataGridViewCellStyle();
        internal static DataGridViewCellStyle DimmedCellStyle = new DataGridViewCellStyle();
        internal static DataGridViewCellStyle PreservedTagCellStyle = new DataGridViewCellStyle();
        internal static DataGridViewCellStyle PreservedTagValueCellStyle = new DataGridViewCellStyle();
        #endregion

        #region Settings
        //Cached settings until MusicBee restart
        internal static bool DontShowShowHiddenWindows;

        internal static bool UseCustomTrackIdTag;
        internal static int CustomTrackIdTag;


        [Serializable]
        public class PluginSettings : SavedSettingsType //******* remove SavedSettingsType later!!!!!!
        {

        }

        [XmlInclude(typeof(PluginSettings))]
        [Serializable]
        public class SavedSettingsType
        {
            public bool not1stTimeUsage;

            public bool allowAsrLrPresetAutoExecution;
            public bool allowCommandExecutionWithoutPreview;

            public bool dontShowContextMenu;

            public bool dontShowCopyTag;
            public bool dontShowSwapTags;
            public bool dontShowChangeCase;
            public bool dontShowReEncodeTag;
            public bool dontShowLibraryReports;
            public bool dontShowAutoRate;
            public bool dontShowAsr;
            public bool dontShowCAR;
            public bool dontShowCT;
            public bool dontShowShowHiddenWindows;
            public bool dontShowBackupRestore;

            public bool minimizePluginWindows;

            public bool dontUseSkinColors;

            public bool useMusicBeeFont;

            public bool useCustomFont;
            public string pluginFontFamilyName;
            public float pluginFontSize;
            public FontStyle pluginFontStyle;

            public bool dontHighlightChangedTags;

            public bool dontIncludeInPreviewLinesWithoutChangedTags;
            public bool dontIncludeInPreviewLinesWithPreservedTagsAsr;
            public bool dontIncludeInPreviewLinesWithPreservedTagValuesAsr;

            public int closeShowHiddenWindows;

            public bool dontPlayCompletedSound;
            public bool playStartedSound;
            public bool playCanceledSound;
            public bool dontPlayTickedAutoApplyingAsrLrPresetSound;

            public string copySourceTagName;
            public string changeCaseSourceTagName;
            public string swapTagsSourceTagName;

            public string copyDestinationTagName;
            public string swapTagsDestinationTagName;

            public string initialEncodingName;
            public string reencodeTagSourceTagName;

            public bool onlyIfDestinationIsEmpty;
            public bool onlyIfSourceNotEmpty;
            public bool smartOperation;
            public bool appendSource;
            public bool addSource;
            public object[] customText;
            public object[] appendedText;
            public object[] addedText;


            public int changeCaseFlag;

            public bool? useExceptionWords; //false: don't use; true: don't change case of excepted words; null: change case only of excepted words
            public object[] exceptedWords;
            public string exceptionWordsAsr;

            public bool useExceptionChars;
            public object[] exceptionChars;
            public string exceptionCharsAsr;

            public bool useExceptionCharPairs;
            public object[] openingExceptionChars;
            public string openingExceptionCharsAsr;
            public object[] closingExceptionChars;
            public string closingExceptionCharsAsr;

            public bool useSentenceSeparators;
            public object[] sentenceSeparators;
            public string sentenceSeparatorsAsr;

            public bool? alwaysCapitalize1stWord = true;
            public bool? alwaysCapitalizeLastWord = false;
            public bool ignoreSingleLetterExceptedWords;


            public ReportPreset[] reportsPresets; //***** delete it later!!!!
            public ReportPreset[] reportPresets;

            public string unitK;
            public string unitM;
            public string unitG;

            public string multipleItemsSplitterChar1;
            public string multipleItemsSplitterChar2;

            public string exportedLastFolder;
            public bool hideLrPreview;

            public MetaDataType autoRateTagId;
            public bool storePlaysPerDay;
            public MetaDataType playsPerDayTagId;

            public bool autoRateAtStartUp;
            public bool notifyWhenAutoRatingCompleted;
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

            public List<WindowSettingsType> windowsSettings;

            public int lastInteractiveTagSet = 0;
            public int lastTagSet = 0;
            public TagSetType[] copyTagsTagSets;

            public string autoBackupDirectory;
            public string autoBackupPrefix;
            public decimal autoBackupInterval;
            public decimal autoDeleteKeepNumberOfDays;
            public decimal autoDeleteKeepNumberOfFiles;
            public bool useCustomTrackIdTag;
            public int customTrackIdTag;

            public int rowHeadersWidth;
            public int defaultColumnWidth;
            public decimal defaultTagHistoryNumberOfBackups;
            public int[] displayedTags;
            public bool dontAutoSelectDisplayedTags;

            public object[] lastSelectedFolders;

            public bool backupArtworks;
            public bool dontTryToGuessLibraryName;
            public bool dontSkipAutoBackupsIfOnlyPlayCountsChanged;

            public List<Guid> autoAppliedAsrPresetGuids = new List<Guid>();
            public Guid[] asrPresetsWithHotkeysGuids = new Guid[MaximumNumberOfAsrHotkeys];

            public bool dontShowPredefinedPresetsCantBeChangedMessage;
            public string defaultAsrPresetsExportFolder;

            public MessageBoxDefaultButton unsavedChangesConfirmationLastAnswer = MessageBoxDefaultButton.Button1;

            public int lastSkippedTagId;
            public int lastSkippedDateFormat;
        }

        internal static PluginSettings SavedSettings;
        #endregion


        #region Localized strings
        //Localizable strings

        //Supported exported file formats
        internal static string ExportedFormats;
        internal static string ExportedTrackList;

        //MusicBeePlugin localizable strings
        internal static string PluginName;
        internal static string PluginMenuGroupName;
        private static string PluginDescription;

        private static string PluginHelpString;
        private static string PluginWebPageString;
        private static string PluginWebPageToolTip;
        private static string PluginVersionString;

        private static string OpenWindowsMenuSectionName;
        private static string TagToolsMenuSectionName;
        private static string BackupRestoreMenuSectionName;

        internal static string CopyTagName;
        internal static string SwapTagsName;
        internal static string ChangeCaseName;
        internal static string ReEncodeTagName;
        internal static string ReEncodeTagsName;
        internal static string LibraryReportsName;
        internal static string AutoRateName;
        internal static string AsrName;
        internal static string CarName;
        internal static string CompareTracksName;
        internal static string CopyTagsToClipboardName;
        internal static string PasteTagsFromClipboardName;
        internal static string MsrName;
        internal static string ShowHiddenWindowsName;

        private static string TagToolsHotkeyDescription;
        private static string CopyTagDescription;
        private static string SwapTagsDescription;
        private static string ChangeCaseDescription;
        private static string ReEncodeTagDescription;
        private static string ReEncodeTagsDescription;
        private static string LibraryReportsDescription;
        private static string AutoRateDescription;
        private static string AsrDescription;
        internal static string AsrPresetHotkeyDescription;
        internal static string LrPresetHotkeyDescription;
        private static string CarDescription;
        private static string CompareTracksDescription;
        private static string CopyTagsToClipboardDescription;
        private static string PasteTagsFromClipboardDescription;
        private static string CopyTagsToClipboardUsingMenuDescription;
        private static string MsrCommandDescription;
        internal static string ShowHiddenWindowsDescription;

        internal static string BackupTagsName;
        internal static string RestoreTagsName;
        internal static string RestoreTagsForEntireLibraryName;
        internal static string RenameMoveBackupName;
        internal static string MoveBackupsName;
        internal static string CreateNewBaselineName;
        internal static string createEmptyBaselineRestoreTagsForEntireLibraryName;
        internal static string DeleteBackupsName;
        internal static string AutoBackupSettingsName;

        internal static string TagHistoryName;

        private static string BackupTagsDescription;
        private static string RestoreTagsDescription;
        private static string RestoreTagsForEntireLibraryDescription;
        private static string RenameMoveBackupDescription;
        private static string MoveBackupsDescription;
        private static string CreateNewBaselineDescription;
        private static string createEmptyBaselineRestoreTagsForEntireLibraryDescription;
        private static string DeleteBackupsDescription;
        private static string AutoBackupSettingsDescription;

        private static string TagHistoryDescription;

        internal static string CopyTagSbText;
        internal static string SwapTagsSbText;
        internal static string ChangeCaseSbText;
        internal static string CompareTracksSbText;
        internal static string ReEncodeTagSbText;
        internal static string LibraryReportsSbText;
        internal static string LibraryReportsGeneratingPreviewSbText;
        internal static string ApplyingLrPresetSbText;
        internal static string AutoRateSbText;
        internal static string AsrSbText;
        internal static string CarSbText;
        internal static string MsrSbText;
        internal static string TagHistorySbText;

        internal static string AnotherLrPresetIsRunningSbText;
        internal static string CtlAnotherLrPresetIsRunning;
        internal static string CtlWaitUntilPresetIsCompleted;
        internal static string CtlLrElapsed;

        //Other localizable strings
        internal static string AlbumTagName;
        internal static string Custom9TagName;
        internal static string UrlTagName;
        internal static string GenreCategoryName;

        internal static string[] LrFunctionNames = new string[7];

        internal static string LibraryReport;

        internal static string DateCreatedTagName;
        internal static string EmptyValueTagName;
        internal static string ClipboardTagName;
        internal static string TextFileTagName;
        internal static string SequenceNumberName;

        internal static string ParameterTagName;
        internal static string TempTagName;

        internal static string LibraryTotalsPresetName;
        internal static string LibraryAveragesPresetName;
        internal static string CDBookletPresetName;
        internal static string AlbumsAndTracksPresetName;
        internal static string AlbumGridPresetName;

        internal static string EmptyPresetName;

        //Displayed text
        internal static string ListItemConditionIs;
        internal static string ListItemConditionIsNot;
        internal static string ListItemConditionIsGreater;
        internal static string ListItemConditionIsGreaterOrEqual;//***
        internal static string ListItemConditionIsLess;
        internal static string ListItemConditionIsLessOrEqual;

        internal static string SelectTagsWindowTitle;
        internal static string SelectDisplayedTagsWindowTitle;
        internal static string SelectButtonName;

        internal static string AsrProcessTagsButtonName;
        internal static string AsrPreserveTagsButtonName;

        internal static string LrButtonFilterResultsShowAllText;
        internal static string LrButtonFilterResultsShowAllToolTip;

        internal static string LrCellToolTip;

        internal static string SbBrokenPresetRetrievalChain;

        internal static string OKButtonName;
        internal static string StopButtonName;
        internal static string CancelButtonName;
        internal static string HideButtonName;
        internal static string PreviewButtonName;
        internal static string ClearButtonName;
        internal static string FindButtonName;
        internal static string SelectFoundButtonName;
        internal static string OriginalTagHeaderTextTagValue;
        internal static string OriginalTagHeaderText;

        internal static string TableCellError;

        internal static string DefaultAsrPresetName;


        internal static string SbSorting;
        internal static string SbUpdating;
        internal static string SbReading;
        internal static string SbUpdated;
        internal static string SbRead;
        internal static string SbFiles;
        internal static string SbItems;
        internal static string SbItemNames;

        internal static string SbAsrPresetIsApplied;
        internal static string SbAsrPresetsAreApplied;

        internal static string SbLrHotkeysAreAssignedIncorrectly;
        internal static string SbIncorrectLrFunctionId;
        internal static string SbLrEmptyTrackListToBeApplied;
        internal static string SbLrNot1TrackPassedToLrFunctionId;
        internal static string SbLrSenselessToSaveSpitGroupingsTo1File;

        internal static string SbBackupRestoreCantDeleteBackupFile;

        internal static string Msg1stTimeUsage;

        internal static string MsgFileNotFound;
        internal static string MsgNoTracksSelected;
        internal static string MsgNoTracksDisplayed;
        internal static string MsgSourceAndDestinationTagsAreTheSame;
        internal static string MsgSwapTagsSourceAndDestinationTagsAreTheSame;
        internal static string MsgNoTagsSelected;
        internal static string MsgNoTracksInCurrentView;
        internal static string MsgTrackListIsEmpty;
        internal static string MsgPreviewIsNotGeneratedNothingToSave;
        internal static string MsgPreviewIsNotGeneratedNothingToChange;
        internal static string MsgPleaseUseGroupingFunction;
        internal static string CtlAllTags;
        internal static string MsgNoUrlColumnUnableToSave;
        internal static string MsgEmptyURL;
        internal static string MsgUnableToSave;
        internal static string MsgUsingEmptyValueAsSourceTagMayBeUsefulOnlyForAppendingStaticTextToDestinationTag;
        internal static string MsgBackgroundTaskIsCompleted;
        internal static string MsgThresholdsDescription;
        internal static string MsgAutoCalculationOfThresholdsDescription;

        internal static string MsgNumberOfPlayedTracks;
        internal static string MsgIncorrectSumOfWeights;
        internal static string MsgSum;
        internal static string MsgNumberOfNotRatedTracks;
        internal static string MsgTracks;
        internal static string MsgActualPercent;

        internal static string MsgCsTheNumberOfOpeningExceptionCharactersMustBe;

        internal static string MsgLrDoYouWantToSaveChangesBeforeClosingTheWindow;
        internal static string MsgLrDoYouWantToCloseTheWindowWithoutSavingChanges;
        internal static string MsgAsrDoYouWantToSaveChangesBeforeClosingTheWindow;

        internal static string MsgIncorrectPresetName;
        internal static string MsgDeletePresetConfirmation;
        internal static string MsgInstallingConfirmation;
        internal static string MsgDoYouWantToResetYourCustomizedPredefinedPresets;
        internal static string MsgDoYouWantToRemovePredefinedPresets;
        internal static string MsgDoYouWantToImportExistingPresetsAsCopies;

        internal static string MsgPresetsWereImported;
        internal static string MsgMsrPresetWasImported;
        internal static string MsgPresetsWereImportedAsCopies;
        internal static string MsgPresetsFailedToImport;

        internal static string MsgPresetsWereInstalled;
        internal static string MsgMsrPresetWasInstalled;
        internal static string MsgPresetsCustomizedWereReinstalled;
        internal static string MsgPresetsWereReinstalled;
        internal static string MsgPresetsWereUpdated;
        internal static string MsgPresetsCustomizedWereUpdated;
        internal static string MsgPresetsWereNotChanged;
        internal static string MsgPresetsRemoved;
        internal static string MsgPresetsFailedToUpdate;

        internal static string MsgPresetsNotFound;

        internal static string MsgDeletingConfirmation;

        internal static string MsgNoPresetsDeleted;
        internal static string MsgPresetsWereDeleted;

        internal static string MsgLrCachedPresetsChanged;
        internal static string MsgLrCachedPresetsNotApplied;


        internal static string MsgNumberOfTagsInTextFileDoesntCorrespondToNumberOfSelectedTracks;

        internal static string MsgClipboardDoesntContainText;
        internal static string MsgClipboardDoesntContainTags;
        internal static string MsgUnknownTagNameInClipboard;

        internal static string MsgNumberOfTagsInClipboardDoesntCorrespondToNumberOfSelectedTracks;
        internal static string MsgDoYouWantToPasteTagsAnyway;
        internal static string MsgWrongNumberOfCopiedTags;
        internal static string MsgMatchTracksByPathQuestion;
        internal static string MsgTracksSelectedMatchedNotMatched;

        internal static string MsgFirstThreeGroupingFieldsInPreviewTableShouldBe;
        internal static string MsgFirstSixGroupingFieldsInPreviewTableShouldBe;
        internal static string MsgFirstThreeGroupingFieldsInPreviewTableShouldBe2;
        internal static string MsgResizingArtworksRequired;

        internal static string MsgYouMustImportStandardAsrPresetsFirst;

        internal static string CtlDirtyError1sf;
        internal static string CtlDirtyError1mf;
        internal static string CtlDirtyError2sf;
        internal static string CtlDirtyError2mf;

        internal static string MsgSelectTracks;

        internal static string MsgCtSelectAtLeast2Tracks;

        internal static string MsgAsrPredefinedPresetsCantBeChanged;

        internal static string MsgMsrGiveNameToAsrPreset;
        internal static string MsgMsrAreYouSureYouWantToSaveAsrPreset;
        internal static string MsgMsrAreYouSureYouWantToOverwriteAsrPreset;
        internal static string MsgMsrAreYouSureYouWantToOverwriteRenameAsrPreset;
        internal static string MsgMsrAreYouSureYouWantToDeleteAsrPreset;
        internal static string MsgMsrSavePreset;
        internal static string MsgMsrDeletePreset;

        internal static string MsgBrMasterBackupIndexIsCorrupted;
        internal static string MsgBrBackupIsCorrupted;
        internal static string MsgBrFolderDoesntExists;
        internal static string MsgBrCreateBaselineWarning;
        internal static string MsgBrBackupBaselineFileDoesntExist;
        internal static string MsgBrThisIsTheBackupOfDifferentLibrary;
        internal static string MsgBrCreateNewBaselineBackupOrRestoreTagsFromAnotherLibraryBeforeMusicBeeRestart;
        internal static string MsgBrCreateNewBaseline;

        internal static string MsgUnsupportedMusicBeeFontType;

        internal static string CtlNewAsrPreset;
        internal static string CtlAsrSyntaxError;

        internal static string CtlAsrPresetEditorPleaseWait;

        internal static string CtlLrPresetAutoName;
        internal static string CtlLrInvalidPresetFormatAutoName;
        internal static string CtlLrError;
        internal static string CtlLrParsingError;
        internal static string CtlLrInvalidValue;
        internal static string CtlLrRefreshUi;
        internal static string CtlLrIncorrectReportPresetId;

        internal static string CtlCopyTagFilter;

        internal static string ExProhibitedParameterCombination;
        internal static string ExIncorrectLrHotkeySlot;
        internal static string ExThisFieldAlreadyDefinedInPreset;


        internal static string ExInvalidDropdownStyle;
        internal static string ExImpossibleToStretchThumb;
        internal static string ExMaskAndImageMustBeTheSameSize;


        internal static string CtlAutoRateCalculating;

        internal static string CtlAsrCellTooTip;
        internal static string CtlAsrAllTagsCellTooTip;
        internal static string MsgAsrPresetsUsingAllTagsPseudoTagNameCannotBeAutoApplied;

        internal static string SbAutoBackingUp;
        internal static string SbMovingBackupsToNewFolder;
        internal static string SbMakingTagBackup;
        internal static string SbRestoringTagsFromBackup;
        internal static string SbRenamingMovingBackup;
        internal static string SbMovingBackups;
        internal static string SbDeletingBackups;
        internal static string SbTagAutoBackupSkipped;
        internal static string SbComparingTags;

        internal static string CtlWarning;
        internal static string CtlMusicBeeBackupType;
        internal static string CtlSaveBackupTitle;
        internal static string CtlRestoreBackupTitle;
        internal static string CtlRenameSelectBackupTitle;
        internal static string CtlRenameSaveBackupTitle;
        internal static string CtlMoveSelectBackupsTitle;
        internal static string CtlMoveSaveBackupsTitle;
        internal static string CtlDeleteBackupsTitle;
        internal static string CtlSelectThisFolder;
        internal static string CtlNoBackupData;
        internal static string CtlNoDifferences;
        internal static string CtlNoPreview;
        internal static string CtlMixedValues;
        internal static string CtlMixedValuesSameAsInLibrary;
        internal static string CtlMixedValuesDifferentFromLibrary;

        internal static string CtlTags;

        internal static string MsgNotAllowedSymbols;
        internal static string MsgPresetExists;

        internal static string CtlWholeCuesheetWillBeReencoded;

        internal static string CtlMSR;

        internal static string CtlUnknown;

        internal static string CtlMixedFilters;

        internal static string MsgFirstSelectWhichFieldYouWantToAssignFunctionIdTo;
        #endregion


        #region Common methods/functions
        internal static CheckState GetNextCheckState(CheckState state)
        {
            switch (state)
            {
                case CheckState.Checked:
                    return CheckState.Indeterminate;
                case CheckState.Indeterminate:
                    return CheckState.Unchecked;
                default:
                    return CheckState.Checked;
            }
        }

        internal static CheckState GetCheckState(bool? state)
        {
            switch (state)
            {
                case true:
                    return CheckState.Checked;
                case false:
                    return CheckState.Unchecked;
                default:
                    return CheckState.Indeterminate;
            }
        }

        internal static bool? GetNullableBoolFromCheckState(CheckState state)
        {
            switch (state)
            {
                case CheckState.Checked:
                    return true;
                case CheckState.Unchecked:
                    return false;
                default:
                    return null;
            }
        }

        internal static string GetPersistentTrackId(string currentFile, bool useCustomTrackIdTag = false)
        {
            if (useCustomTrackIdTag && SavedSettings.useCustomTrackIdTag && SavedSettings.customTrackIdTag > 0)
                return GetFileTag(currentFile, (MetaDataType)SavedSettings.customTrackIdTag);
            else
                return MbApiInterface.Library_GetDevicePersistentId(currentFile, (DeviceIdType)0) ?? "-1";
        }

        internal static int GetPersistentTrackIdInt(string currentFile, bool useCustomTrackIdTag = false)
        {
            return int.Parse(GetPersistentTrackId(currentFile, useCustomTrackIdTag));
        }

        internal static string AddLibraryNameToTrackId(string libraryName, string trackId)
        {
            return libraryName + "%%%%" + trackId;
        }

        internal static string GetCurrentLibraryPath()
        {
            string libraryPath = null;

            if (MbApiInterface.Playlist_QueryPlaylists())
            {
                var playlist = MbApiInterface.Playlist_QueryGetNextPlaylist();

                if (!string.IsNullOrEmpty(playlist))
                    libraryPath = Regex.Replace(playlist, @"^(.*)\\Playlists\\.*", "$1");
            }

            if (string.IsNullOrWhiteSpace(libraryPath))
                libraryPath = string.Empty;

            return libraryPath;
        }

        internal static string GetStringHash(string input)
        {
            //TypeConverter tc = TypeDescriptor.GetConverter(typeof(string));
            //System.Security.Cryptography.MD5Cng md5 = new System.Security.Cryptography.MD5Cng();
            //byte[] hash = md5.ComputeHash(tc.ConvertTo(input, typeof(byte[])) as byte[]);

            //return Convert.ToBase64String(hash);

            return input.GetHashCode().ToString("X");
        }

        internal static string GetPluralForm(string sentence, int number)
        {
            int form;
            var remainder = number % 10;

            if (number == 0) //Here may be special form like "No files" instead of "0 files"
                form = 5;
            else if (number >= 5 && number <= 20)
                form = 5;
            else if (remainder == 0)
                form = 5;
            else if (remainder == 1)
                form = 1;
            else if (remainder >= 2 && remainder <= 4)
                form = 2;
            else
                form = 5;

            switch (form)
            {
                case 1:
                    sentence = Regex.Replace(sentence, @"\{(.*?);(.*?);(.*?)\}", "$1");
                    break;
                case 2:
                    sentence = Regex.Replace(sentence, @"\{(.*?);(.*?);(.*?)\}", "$2");
                    break;
                case 5:
                    sentence = Regex.Replace(sentence, @"\{(.*?);(.*?);(.*?)\}", "$3");
                    break;
            }

            return sentence;
        }

        internal static string AddLeadingSpaces(int number, int spacesCount, int zerosCount = 1)
        {
            var leadingZerosNumber = number.ToString("D" + spacesCount);
            var leadingSpaces = string.Empty;
            int maxZerosIndex;
            for (maxZerosIndex = 0; maxZerosIndex < spacesCount - zerosCount; maxZerosIndex++)
            {
                if (leadingZerosNumber[maxZerosIndex] == '0')
                    leadingSpaces += '\u2007';
                else
                    break;
            }

            return leadingSpaces + leadingZerosNumber.Substring(maxZerosIndex);
        }

        internal struct SwappedTags
        {
            internal string newDestinationTagValue;
            internal string newDestinationTagTValue;
            internal string destinationTagTValue;
            internal string newSourceTagValue;
            internal string newSourceTagTValue;
            internal string sourceTagTValue;
        }

        internal static int GetMulDivFactor(string representation)
        {
            switch (representation)
            {
                case null:
                case "1 (ignore)":
                case "1 (игнорировать)":
                    return 1;
                case "1000 (K)":
                    return 1000;
                case "1000000 (M)":
                    return 1000000;
                case "1024 (K)":
                    return 1024;
                case "1048576 (M)":
                    return 1048576;
                default:
                    return int.Parse(representation);
            }
        }

        internal static int GetPrecisionDigits(string representation)
        {
            switch (representation)
            {
                case null:
                case "(don't round)":
                case "(не округлять)":
                    return -1;
                default:
                    return int.Parse(representation);
            }

        }

        public enum ResultType
        {
            UseOtherResults = -2,
            AutoDouble = -1,
            Double = 1,
            Year = 2,
            DateTime = 3,
            TimeSpan = 4,

            ItemCount = 50,
            UnknownOrString = 51,

            ParsingError = 100
        }

        internal struct ConvertStringsResult
        {
            internal static readonly DateTime ReferenceDt = new DateTime(0001, 1, 1);
            internal static readonly TimeSpan ReferenceTsOffset = new TimeSpan(0001, 0, 0);

            internal DataType dataType;
            internal ResultType resultType;

            internal double resultD;
            internal string resultDPrefix;
            internal string resultDSpace;
            internal string resultDPostfix;

            internal string resultS;

            internal SortedDictionary<string, bool> urls;
            internal SortedDictionary<string, bool> items;
            internal SortedDictionary<string, bool> items1;

            internal ConvertStringsResult(ResultType resultType, DataType dataType)
            {
                this.dataType = dataType;
                this.resultType = resultType;

                resultD = 0;
                resultDPrefix = null;
                resultDSpace = null;
                resultDPostfix = null;

                resultS = string.Empty;

                urls = new SortedDictionary<string, bool>();
                items = new SortedDictionary<string, bool>();
                items1 = new SortedDictionary<string, bool>();
            }

            internal int compare(ConvertStringsResult comparedResult) //-1: less than comparedResult, 0: equal to comparedResult, +1: greater than comparedResult
            {
                var value = getResult();
                var comparedValue = comparedResult.getResult();

                if (double.IsNegativeInfinity(value) || double.IsNegativeInfinity(comparedValue)) //Let's compare as strings
                {
                    return resultS.CompareTo(comparedResult.resultS);
                }
                else
                {
                    if (value == comparedValue)
                        return 0;
                    else if (value < comparedValue)
                        return -1;
                    else //if (value > comparedValue)
                        return +1;
                }
            }

            internal double getResult() //Returns double for any numeric (number/duration/date-time) result or NegativeInfinity for any not numeric one
            {
                if (items1.Count != 0) //It's "Average count" function
                {
                    return (double)items1.Count / items.Count;
                }
                //Must never happen? //****
                else if ((resultType >= ResultType.UnknownOrString && resultType < ResultType.ParsingError) || resultType == ResultType.UseOtherResults)
                {
                    if (items.Count == 0)
                        return double.NegativeInfinity;
                    else //It's "average" function
                        return resultD / items.Count;
                }
                else if (resultType == ResultType.ItemCount) //Items count
                {
                    return items.Count;
                }
                else if (resultType == ResultType.Double || resultType == ResultType.AutoDouble) //Double
                {
                    if (items.Count == 0)
                        return resultD;
                    else //It's "average" function
                        return resultD / items.Count;
                }
                else if (resultType == ResultType.TimeSpan) //Timespan
                {
                    if (items.Count == 0)
                        return resultD;
                    else //It's "average" function
                        return resultD / items.Count;
                }
                else if (resultType == ResultType.DateTime) //Datetime
                {
                    if (items.Count == 0)
                        return resultD;
                    else //It's "average" function
                        return resultD / items.Count;
                }
                else if (resultType == ResultType.Year) //Datetime (year only)
                {
                    if (items.Count == 0)
                        return resultD;
                    else //It's "average" function
                        return resultD / items.Count;
                }
                //(Auto)double, year or date-time (milliseconds from 1900-01-01), timespan (milliseconds)
                //else if (resultType >= ResultType.AutoDouble && resultType < ResultType.UnknownOrString)
                //{
                //    double value = resultD;
                //    if (items != null && items.Count != 0) //It's "average" function
                //        value /= items.Count;

                //    return value;
                //}
                else if (resultType == ResultType.ParsingError) //Parsing error. Must never happen
                {
                    return double.PositiveInfinity; //Exception!
                }
                else
                {
                    return double.NegativeInfinity; //Treat as string
                }
            }

            internal string getFormattedResult(int operation, string mulDivFactorRepr, string precisionDigitsRepr, string appendedText)
            {
                if (resultType == ResultType.UnknownOrString || resultType == ResultType.UseOtherResults)
                {
                    return resultS;
                }
                else if (resultType == ResultType.ParsingError)
                {
                    return CtlLrParsingError;
                }


                var result = getResult();

                if (double.IsNegativeInfinity(result))
                {
                    resultType = ResultType.ParsingError;

                    return CtlLrInvalidValue;
                }
                else if (resultType == ResultType.ItemCount || resultType == ResultType.Double || resultType == ResultType.AutoDouble) //Item count or double. It's numeric result. Let's format it.
                {
                    var mulDivFactor = GetMulDivFactor(mulDivFactorRepr);
                    if (mulDivFactor != 1 && operation == 0)
                        result /= mulDivFactor;
                    else if (mulDivFactor != 1 && operation == 1)
                        result *= mulDivFactor;

                    var precisionDigits = GetPrecisionDigits(precisionDigitsRepr);
                    if (precisionDigits >= 0)
                        return resultDPrefix + Math.Round((decimal)result, precisionDigits).ToString("F" + precisionDigits) + resultDSpace + appendedText + resultDPostfix;
                    else
                        return resultDPrefix + result.ToString() + resultDSpace + appendedText + resultDPostfix;
                }
                else if (resultType == ResultType.TimeSpan)
                {
                    return TimeSpan.FromMilliseconds(result).ToString("g");//***
                }
                else if (resultType == ResultType.DateTime)
                {
                    return (ReferenceDt + ReferenceTsOffset + TimeSpan.FromMilliseconds(result)).ToString("g");
                }
                else if (resultType == ResultType.Year)
                {
                    return (ReferenceDt + ReferenceTsOffset + TimeSpan.FromMilliseconds(result)).Year.ToString("D4");
                }
                else
                {
                    throw new Exception("Not implemented result type: " + resultType + "!");
                }
            }
        }

        internal static (double, string, string, string) ParseNumberAndMeasurementUnits(string input, string units, double multiplicator) //Returns normalized number & string postfix
        {
            string stringnumber;

            var fractionalpart = Regex.Replace(input, @"^(\D*?)(\d+)(\.|\,)?(\d*?)(\s*)" + units + @"(\D.*)$", "$4", RegexOptions.IgnoreCase);
            if (fractionalpart == string.Empty || fractionalpart == input)
                stringnumber = Regex.Replace(input, @"^(\D*?)(\d+)(\.|\,)?(\d*?)(\s*)" + units + @"(\D.*)$", "$2", RegexOptions.IgnoreCase);
            else
                stringnumber = Regex.Replace(input, @"^(\D*?)(\d+)(\.|\,)?(\d*?)(\s*)" + units + @"(\D.*)$", "$2" + LocalizedDecimalPoint + "$4", RegexOptions.IgnoreCase);

            if (stringnumber == input)
                return (double.NegativeInfinity, null, null, null);

            var prefix = Regex.Replace(input, @"^(\D*?)(\d+)(\.|\,)?(\d*?)(\s*)" + units + @"(\D.*)$", "$1", RegexOptions.IgnoreCase);
            if (prefix == input)
                prefix = string.Empty;

            var space = Regex.Replace(input, @"^(\D*?)(\d+)(\.|\,)?(\d*?)(\s*)" + units + @"(\D.*)$", "$5", RegexOptions.IgnoreCase);
            if (space == input)
                space = string.Empty;

            var postfix = Regex.Replace(input, @"^(\D*?)(\d+)(\.|\,)?(\d*?)(\s*)" + units + @"(\D.*)$", "$7", RegexOptions.IgnoreCase);
            if (postfix == input)
                postfix = string.Empty;

            if (double.TryParse(stringnumber, out var number))
                return ((number * multiplicator), prefix, space, postfix);
            else
                return (double.NegativeInfinity, null, null, null);
        }

        internal static (double, string, string, string) ParseNumber(string input) //Returns normalized number & string postfix
        {
            string stringnumber;

            var fractionalpart = Regex.Replace(input, @"^(\D*?)(\d+)(\.|\,)?(\d*?)(\s*)(\D*)$", "$4", RegexOptions.IgnoreCase);
            if (fractionalpart == string.Empty || fractionalpart == input)
                stringnumber = Regex.Replace(input, @"^(\D*?)(\d+)(\.|\,)?(\d*?)(\s*)(\D*)$", "$2", RegexOptions.IgnoreCase);
            else
                stringnumber = Regex.Replace(input, @"^(\D*?)(\d+)(\.|\,)?(\d*?)(\s*)(\D*)$", "$2" + LocalizedDecimalPoint + "$4", RegexOptions.IgnoreCase);

            var prefix = Regex.Replace(input, @"^(\D*?)(\d+)(\.|\,)?(\d*?)(\s*)(\D*)$", "$1", RegexOptions.IgnoreCase);

            if (!string.IsNullOrWhiteSpace(prefix)) //Probably prefixed string must not br treated as number at all //***
                return (double.NegativeInfinity, null, null, null);
            if (prefix == input)
                prefix = string.Empty;

            var space = Regex.Replace(input, @"^(\D*?)(\d+)(\.|\,)?(\d*?)(\s*)(\D*)$", "$5", RegexOptions.IgnoreCase);
            if (space == input)
                space = string.Empty;

            var postfix = Regex.Replace(input, @"^(\D*?)(\d+)(\.|\,)?(\d*?)(\s*)(\D*)$", "$6", RegexOptions.IgnoreCase);
            if (postfix == input)
                postfix = string.Empty;

            if (double.TryParse(stringnumber, out var number))
                return (number, prefix, space, postfix);
            else
                return (double.NegativeInfinity, null, null, null);
        }

        internal static ConvertStringsResult ConvertStrings(string arg, ResultType resultType, DataType dataType, bool replacements = false)
        {
            var result = new ConvertStringsResult(resultType, dataType)
            {
                resultS = arg
            };

            if (arg == string.Empty)
            {
                result.resultD = 0;
                result.resultS = string.Empty;

                result.dataType = dataType;
                result.resultType = ResultType.UseOtherResults; //Use other results to get type

                return result;
            }
            else if (arg == CtlUnknown)
            {
                result.resultD = 0;
                result.resultS = CtlUnknown;

                result.dataType = dataType;
                result.resultType = ResultType.UseOtherResults; //Use other results to get type

                return result;
            }


            if (replacements)
            {
                var unitsK = "(k|к";
                var unitsM = "(m|м";
                var unitsG = "(g|г";

                if (!string.IsNullOrWhiteSpace(SavedSettings.unitK))
                    unitsK = "|" + SavedSettings.unitK;

                if (!string.IsNullOrWhiteSpace(SavedSettings.unitM))
                    unitsM = "|" + SavedSettings.unitM;

                if (!string.IsNullOrWhiteSpace(SavedSettings.unitG))
                    unitsG = "|" + SavedSettings.unitG;

                unitsK += ")";
                unitsM += ")";
                unitsG += ")";

                var number = double.NegativeInfinity;

                string numberprefix = null;
                string numberspace = null;
                string numberpostfix = null;

                double numbertemp;
                string numberprefixtemp;
                string numberspacetemp;
                string numberpostfixtemp;


                (numbertemp, numberprefixtemp, numberspacetemp, numberpostfixtemp) = ParseNumberAndMeasurementUnits(arg, unitsG, 1_099_511_627_776);
                if (!double.IsNegativeInfinity(numbertemp))
                {
                    number = numbertemp;
                    numberprefix = numberprefixtemp;
                    numberspace = numberspacetemp;
                    numberpostfix = numberpostfixtemp;
                }
                else
                {
                    (numbertemp, numberprefixtemp, numberspacetemp, numberpostfixtemp) = ParseNumberAndMeasurementUnits(arg, unitsM, 1_048_576);
                    if (!double.IsNegativeInfinity(numbertemp))
                    {
                        number = numbertemp;
                        numberprefix = numberprefixtemp;
                        numberspace = numberspacetemp;
                        numberpostfix = numberpostfixtemp;
                    }
                    else
                    {
                        (numbertemp, numberprefixtemp, numberspacetemp, numberpostfixtemp) = ParseNumberAndMeasurementUnits(arg, unitsK, 1024);
                        if (!double.IsNegativeInfinity(numbertemp))
                        {
                            number = numbertemp;
                            numberprefix = numberprefixtemp;
                            numberspace = numberspacetemp;
                            numberpostfix = numberpostfixtemp;
                        }
                        else
                        {
                            if (double.IsNegativeInfinity(number))
                            {
                                (numbertemp, numberprefixtemp, numberspacetemp, numberpostfixtemp) = ParseNumber(arg);
                                if (!double.IsNegativeInfinity(numbertemp))
                                {
                                    number = numbertemp;
                                    numberprefix = numberprefixtemp;
                                    numberspace = numberspacetemp;
                                    numberpostfix = numberpostfixtemp;
                                }
                            }
                        }
                    }
                }


                if (!double.IsNegativeInfinity(number))
                {
                    result.resultD = number;
                    result.resultDPrefix = numberprefix;
                    result.resultDSpace = numberspace;
                    result.resultDPostfix = numberpostfix;
                    result.dataType = dataType;

                    if (dataType == DataType.Number)
                        result.resultType = ResultType.Double;
                    else
                        result.resultType = ResultType.AutoDouble;
                }
                else if (dataType != DataType.Number) //Auto-type
                {
                    result.resultD = 0;
                    result.resultDPrefix = string.Empty;
                    result.resultDSpace = string.Empty;
                    result.resultDPostfix = string.Empty;
                    result.dataType = dataType;
                    result.resultType = ResultType.UnknownOrString;
                }
                else //Error parsing number type
                {
                    result.resultD = 0;
                    result.resultDPrefix = string.Empty;
                    result.resultDSpace = string.Empty;
                    result.resultDPostfix = string.Empty;
                    result.dataType = dataType;
                    result.resultType = ResultType.ParsingError;
                }

                return result;
            }


            if (dataType == DataType.Number)
            {
                if (!double.TryParse(arg, out result.resultD))
                    return ConvertStrings(arg, ResultType.Double, DataType.Number, true);

                return result;
            }
            else if (dataType == DataType.Rating)
            {
                if (!double.TryParse(arg, out result.resultD))
                {
                    result.resultD = 0;
                    result.resultType = ResultType.ParsingError;
                }

                return result;
            }
            else if (dataType == DataType.DateTime)
            {
                //Let's try to parse as year
                if (arg.Length == 4 && !arg.Contains(":") && !arg.Contains("/") && !arg.Contains("-") && !arg.Contains("."))
                {
                    result.resultD = (new DateTime(int.Parse(arg), 1, 1) - ConvertStringsResult.ReferenceDt - ConvertStringsResult.ReferenceTsOffset).TotalMilliseconds;
                    result.resultType = ResultType.Year;

                    return result;
                }
                //Let's try to parse as short duration (without date)
                else if (arg.Length < 9 && arg.Contains(":") && !arg.Contains("/") && !arg.Contains("-") && !arg.Contains("."))
                {
                    var ts1 = arg.Split(':');

                    var time = TimeSpan.FromSeconds(Convert.ToInt32(ts1[ts1.Length - 1]));
                    time += TimeSpan.FromMinutes(Convert.ToInt32(ts1[ts1.Length - 2]));
                    if (ts1.Length > 2)
                        time += TimeSpan.FromHours(Convert.ToInt32(ts1[ts1.Length - 3]));

                    result.resultD = time.TotalMilliseconds;
                    result.resultType = ResultType.TimeSpan;

                    return result;
                }
                else if (DateTime.TryParse(arg, out var datetime))
                {
                    //Let's try to parse as long duration (with date)
                    if (datetime.Year < 1900)
                    {
                        result.resultD = TimeSpan.Parse(arg).TotalMilliseconds;
                        result.resultType = ResultType.TimeSpan;
                    }
                    //Let's try to parse as date/time
                    else
                    {
                        result.resultD = (datetime - ConvertStringsResult.ReferenceDt - ConvertStringsResult.ReferenceTsOffset).TotalMilliseconds;
                        result.resultType = ResultType.DateTime;
                    }
                }
                else
                {
                    result.resultD = 0;
                    result.resultType = ResultType.ParsingError;
                }

                return result;
            }
            else
            {
                if (dataType != DataType.String)
                    throw new Exception("Unsupported data type: " + dataType + "!");

                try //Let's try to parse as date/time
                {
                    var datetime = DateTime.Parse(arg);
                    if (datetime.Year < 1900)
                        throw new Exception(); //Let's try to parse as timespan again (as full timespan format including date)

                    result.resultD = (datetime - ConvertStringsResult.ReferenceDt - ConvertStringsResult.ReferenceTsOffset).TotalMilliseconds;
                    result.resultType = ResultType.DateTime;

                    return result;
                }
                catch
                {
                    try
                    {
                        //Let's try to parse as timespan again (as full timespan format including date)
                        if (arg.Length > 5 && (arg.IndexOf('/') != arg.LastIndexOf('/')
                            || arg.IndexOf('-') != arg.LastIndexOf('-') || arg.IndexOf('.') != arg.LastIndexOf('.')))
                        {
                            result.resultD = TimeSpan.Parse(arg).TotalMilliseconds;
                            result.resultType = ResultType.TimeSpan;

                            return result;
                        }
                        else
                        {
                            throw new Exception(); //Go to parsing as number
                        }
                    }
                    catch
                    {
                        //Let's try to parse as number if there were no replacements
                        return ConvertStrings(arg, ResultType.AutoDouble, DataType.String, true);
                    }
                }
            }
        }

        //Returns: +1 - string1 > string2, 0 - string1 = string2, -1 - string1 < string2
        internal static int CompareStrings(string string1, string string2, ResultType type = ResultType.UseOtherResults, DataType datatype = DataType.String)
        {
            ConvertStringsResult result1 = default;
            ConvertStringsResult result2 = default;

            if (type < ResultType.UnknownOrString)
            {
                result1 = ConvertStrings(string1, type, datatype);
                result2 = ConvertStrings(string2, type, datatype);

                if (result1.resultType > result2.resultType)
                    type = result1.resultType;
                else
                    type = result2.resultType;
            }

            switch (type)
            {
                case ResultType.ItemCount:

                    if (result1.items.Count > result2.items.Count)
                        return 1;
                    else if (result1.items.Count < result2.items.Count)
                        return -1;
                    else
                        return 0;

                case ResultType.AutoDouble:
                case ResultType.Double:
                case ResultType.TimeSpan:
                case ResultType.DateTime:

                    if (result1.resultD > result2.resultD)
                        return 1;
                    else if (result1.resultD < result2.resultD)
                        return -1;
                    else
                        return 0;

                case ResultType.ParsingError:

                    return -1; //Let's sort parsing errors 1st for ascending order

                default:

                    return string.Compare(string1, string2);
            }
        }

        internal class DataGridViewCellComparer : System.Collections.IComparer
        {
            internal ResultType[] resultTypes = null;
            internal int comparedColumnIndex = -1;

            public int Compare(object x, object y)
            {
                if (resultTypes == null)
                    throw new Exception("Undefined table sorting type!");

                if (comparedColumnIndex != -1)
                {
                    var comparison = CompareStrings((x as DataGridViewRow).Cells[comparedColumnIndex].Value as string, (y as DataGridViewRow).Cells[comparedColumnIndex].Value as string); //-V3156
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


        internal class TextTableComparer : IComparer<string[]>
        {
            internal int tagCounterIndex = -1;

            public int Compare(string[] x, string[] y)
            {
                if (tagCounterIndex != -1)
                {
                    for (var i = 0; i < tagCounterIndex; i++)
                    {
                        var comparation = CompareStrings(x[i], y[i], ResultType.Double, DataType.Number);

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

        internal static void UpdateTrackForBackup(string trackUrl)
        {
            var nodeName = MbApiInterface.Library_GetFileTag(trackUrl, (MetaDataType)42);

            if (nodeName != MusicName)
                return;


            var trackId = GetPersistentTrackIdInt(trackUrl);

            if (UpdatedTracksForBackupCount < MaxUpdatedTracksCount)
            {
                if (!TracksNeededToBeBackedUp.AddSkip(trackId))
                    UpdatedTracksForBackupCount++;
            }
            else
            {
                TracksNeededToBeBackedUp.Clear();
                UpdatedTracksForBackupCount = 0;
                BackupIsAlwaysNeeded = true;
            }
        }

        internal static string GetTagRepresentation(string tag)
        {
            return ReplaceMSIds(RemoveMSIdAtTheEndOfString(RemoveRoleIds(tag)));
        }

        internal static string GetTrackRepresentation(string currentFile)
        {
            var trackRepresentation = string.Empty;

            var displayedArtist = GetFileTag(currentFile, DisplayedArtistId);
            var album = GetFileTag(currentFile, MetaDataType.Album);
            var title = GetFileTag(currentFile, MetaDataType.TrackTitle);
            var diskNo = GetFileTag(currentFile, MetaDataType.DiscNo);
            var trackNo = GetFileTag(currentFile, MetaDataType.TrackNo);

            trackRepresentation += diskNo;
            trackRepresentation += (trackRepresentation == string.Empty) ? (trackNo) : ("-" + trackNo);
            trackRepresentation += (trackRepresentation == string.Empty) ? string.Empty : ". ";
            trackRepresentation += (trackRepresentation == string.Empty) ? (displayedArtist) : (". " + displayedArtist);
            trackRepresentation += (trackRepresentation == string.Empty) ? (album) : (" - " + album);
            trackRepresentation += (trackRepresentation == string.Empty) ? (title) : (" - " + title);

            return trackRepresentation;
        }

        internal static string GetTrackRepresentation(string[] tags, string[] tags2, string[] tagNames, bool previewSortTags)
        {
            var trackRepresentation = string.Empty;

            trackRepresentation += tags[12]; //12 - track number
            trackRepresentation += (trackRepresentation == string.Empty) ? tags[28] : ("-" + tags[28]); //28 - disk number
            trackRepresentation += (trackRepresentation == string.Empty) ? string.Empty : ". ";

            trackRepresentation += tags[5]; //5 - Album artist or artist
            trackRepresentation += (trackRepresentation == string.Empty) ? tags[3] : (" - " + tags[3]); //3 - album name
            trackRepresentation += (trackRepresentation == string.Empty) ? tags[2] : (" - " + tags[2]); //2 - track tiltle

            trackRepresentation += " (";

            for (var i = 0; i < tags.Length; i++)
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

            trackRepresentation = Regex.Replace(trackRepresentation, "\u0000", "; "); //Remove multiple artist/composer splitters
            trackRepresentation = Regex.Replace(trackRepresentation, "\u0001", string.Empty); //Here and below: remove performer/remixer/guest ids
            trackRepresentation = Regex.Replace(trackRepresentation, "\u0002", string.Empty);
            trackRepresentation = Regex.Replace(trackRepresentation, "\u0003", string.Empty);

            return trackRepresentation;
        }

        internal static SwappedTags SwapTags(string sourceTagValue, string destinationTagValue, MetaDataType sourceTagId, MetaDataType destinationTagId,
            bool smartOperation, bool appendSourceToDestination = false, string appendedText = "", bool addSourceToDestination = false, string addedText = "")
        {
            var swappedTags = new SwappedTags();
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
                    if (ReplaceMSIds(sourceTagValue) == sourceTagValue) //No MS ids, it's a single item
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

        internal static void CustomComboBoxLeave(CustomComboBox comboBox, string newValue = null)
        {
            var comboBoxText = (newValue ?? comboBox.Text);

            if (string.IsNullOrWhiteSpace(comboBoxText))
                return;

            if (comboBox.Items.Contains(comboBoxText))
                comboBox.Items.Remove(comboBoxText);
            else
                comboBox.Items.RemoveAt(9);

            comboBox.Items.Insert(0, comboBoxText);

            comboBox.Text = comboBoxText;
        }

        internal static void ComboBoxLeave(ComboBox comboBox, string newValue = null)
        {
            var comboBoxText = (newValue ?? comboBox.Text);

            if (string.IsNullOrWhiteSpace(comboBoxText))
                return;

            if (comboBox.Items.Contains(comboBoxText))
                comboBox.Items.Remove(comboBoxText);
            else
                comboBox.Items.RemoveAt(9);

            comboBox.Items.Insert(0, comboBoxText);

            comboBox.Text = comboBoxText;
        }

        //internal static void SetIReportPreset1stInListBoxByGuid(ListBox listBox, ReportPreset preset)
        //{
        //   bool itemIsFound = false;

        //   for (int i = 0; i < listBox.Items.Count; i++)
        //   {
        //       if ((listBox.Items[i] as ReportPreset).guid == preset.guid)
        //       {
        //           listBox.Items.RemoveAt(i);
        //           itemIsFound = true;
        //           break;
        //       }
        //   }

        //   if (!itemIsFound)
        //       listBox.Items.RemoveAt(listBox.Items.Count - 1);

        //   listBox.Items.Insert(NumberOfPredefinedPresets, preset);
        //   listBox.SelectedItem = preset;
        //}

        internal static string GetFileTag(string sourceFileUrl, MetaDataType tagId, bool normalizeTrackRatingTo0_100Range = false)
        {
            string tag = string.Empty;

            switch (tagId)
            {
                case 0:
                    break;

                case DateCreatedTagId:
                    try
                    {
                        var fileInfo = new FileInfo(sourceFileUrl);
                        tag = fileInfo.CreationTime.ToString("G");
                    }
                    catch (Exception ex)
                    {
                        SetStatusBarText(ex.Message, false);
                        return string.Empty;
                    }

                    break;

                case NullTagId:
                    break;

                case DisplayedArtistId:
                    tag = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Artist);
                    break;

                case ArtistArtistsId:
                    var rawArtist = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Artist);
                    var multiArtist = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.MultiArtist);

                    if (multiArtist != string.Empty)
                        tag = multiArtist;
                    else
                        tag = rawArtist;
                    break;

                case DisplayedComposerId:
                    tag = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Composer);
                    break;

                case ComposerComposersId:
                    var rawComposer = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Composer);
                    var multiComposer = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.MultiComposer);

                    if (multiComposer != string.Empty)
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
                        if (tag != string.Empty)
                        {
                            if (!decimal.TryParse(tag, out var rating))
                                rating = 0;

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

                case FilePathWoExtTagId:
                    tag = Regex.Replace(sourceFileUrl, @"^(.*)\.(.*)", "$1");
                    break;

                default:
                    tag = MbApiInterface.Library_GetFileTag(sourceFileUrl, tagId);
                    break;
            }


            return tag ?? string.Empty;
        }

        internal static string[] GetFileTags(string sourceFileUrl, List<MetaDataType> tagIds)
        {
            var tagIds2 = tagIds.ToArray();

            for (var i = 0; i < tagIds2.Length; i++)
            {
                var tagId = tagIds2[i];

                switch (tagId)
                {
                    case NullTagId:
                        tagIds2[i] = 0;
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
                        tagIds2[i] = 0;
                        break;

                    case LyricsId:
                        tagIds2[i] = 0;
                        break;

                    case SynchronisedLyricsId:
                        tagIds2[i] = 0;
                        break;

                    case UnsynchronisedLyricsId:
                        tagIds2[i] = 0;
                        break;

                    case FolderTagId:
                        tagIds2[i] = 0;
                        break;

                    case FileNameTagId:
                        tagIds2[i] = 0;
                        break;

                    case FilePathTagId:
                        tagIds2[i] = 0;
                        break;

                    case FilePathWoExtTagId:
                        tagIds2[i] = 0;
                        break;
                }
            }



            if (!MbApiInterface.Library_GetFileTags(sourceFileUrl, tagIds2, out var tags))
                return Array.Empty<string>();


            for (var i = 0; i < tagIds.Count; i++)
            {
                var tagId = tagIds[i];

                switch (tagId)
                {
                    case 0:
                    case NullTagId:
                        tags[i] = string.Empty;
                        break;

                    case ArtistArtistsId:
                        tags[i] = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Artist);
                        break;

                    case ComposerComposersId:
                        if (tags[i] == string.Empty)
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

                    case FilePathWoExtTagId:
                        tags[i] = Regex.Replace(sourceFileUrl, @"^(.*)\.(.*)", "$1");
                        break;
                }


                if (tags[i] == null)
                    tags[i] = string.Empty;
            }

            return tags;
        }

        //Method returns "T" if tag was really changed and "F" otherwise
        internal static bool SetFileTag(string sourceFileUrl, MetaDataType tagId, string value, bool updateOnlyChangedTags = false)
        {
            bool result1;
            bool result2;


            if (tagId == (MetaDataType)(-201) || tagId == (MetaDataType)(-202) || tagId == (MetaDataType)(-203) || tagId == (MetaDataType)(-204))
                return false;

            if (tagId == DateCreatedTagId)
            {
                try
                {
                    var fileInfo = new FileInfo(sourceFileUrl)
                    {
                        CreationTime = DateTime.Parse(value)
                    };
                }
                catch (Exception ex)
                {
                    SetStatusBarText(ex.Message, false);
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
                        ChangedFiles.AddUnique(sourceFileUrl);
                }
            }
            else
            {
                lock (ChangedFiles)
                    ChangedFiles.AddUnique(sourceFileUrl);
            }

            switch (tagId)
            {
                case NullTagId:
                    return true;

                case DisplayedArtistId:
                    var multiArtist = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.MultiArtist);
                    result1 = MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.Artist, value);
                    result2 = MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.MultiArtist, multiArtist);
                    return result1 && result2;

                case ArtistArtistsId:
                    if (ReplaceMSIds(value) == value) //No MS Ids, single artist
                        return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.Artist, value);
                    else
                        return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.MultiArtist, value);

                case DisplayedComposerId:
                    var multiComposer = MbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.MultiComposer);
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
                        var tc = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Bitmap));
                        var bitmapImage = tc.ConvertFrom(Convert.FromBase64String(value)) as Bitmap;

                        var tempJpeg = new MemoryStream();
                        bitmapImage.Save(tempJpeg, ImageFormat.Png);

                        imageData = tempJpeg.ToArray();
                        tempJpeg.Close();
                        bitmapImage.Dispose();
                    }

                    return MbApiInterface.Library_SetArtworkEx(sourceFileUrl, 0, imageData);

                case MetaDataType.AlbumArtistRaw:
                    return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.AlbumArtist, value);

                case LyricsId:
                    return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.Lyrics, value);

                case MetaDataType.Rating:
                    if (value.ToLower() == "no rating")
                        return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.Rating, string.Empty);
                    else
                        return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.Rating, value);

                case MetaDataType.RatingAlbum:
                    if (value.ToLower() == "no rating")
                        return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.RatingAlbum, string.Empty);
                    else
                        return MbApiInterface.Library_SetFileTag(sourceFileUrl, MetaDataType.RatingAlbum, value);

                default:
                    return MbApiInterface.Library_SetFileTag(sourceFileUrl, tagId, value);
            }
        }

        internal static bool CommitTagsToFile(string sourceFileUrl, bool ignoreFutureTagsChangedEvent = false, bool updateOnlyChangedTags = false)
        {
            var result = false;

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

            if (ignoreFutureTagsChangedEvent)
            {
                lock (FilesUpdatedByPlugin)
                {
                    FilesUpdatedByPlugin.Add(sourceFileUrl);
                }
            }

            result = MbApiInterface.Library_CommitTagsToFile(sourceFileUrl);

            RefreshPanels();

            lock (ChangedFiles)
                ChangedFiles.RemoveExisting(sourceFileUrl);

            return result;
        }

        internal static void RefreshPanels(bool immediateRefresh = false, bool quickRefresh = false, bool forceRefresh = false)
        {
            if (immediateRefresh)
            {
                if (UiRefreshingIsNeeded || forceRefresh)
                {
                    UiRefreshingIsNeeded = false;
                    lock (LastUI_RefreshLocker)
                    {
                        LastUI_Refresh = DateTime.UtcNow;
                    }

                    if (quickRefresh)
                        MbApiInterface.MB_InvokeCommand((Command)4, null);
                    else
                        MbApiInterface.MB_RefreshPanels();

                    MbApiInterface.MB_SetBackgroundTaskMessage(LastMessage);
                }
            }
            else
            {
                var refresh = false;

                lock (LastUI_RefreshLocker)
                {
                    if (DateTime.UtcNow - LastUI_Refresh >= RefreshUI_Delay)
                    {
                        LastUI_Refresh = DateTime.UtcNow;
                        refresh = true;
                    }
                }


                if (refresh)
                {
                    UiRefreshingIsNeeded = false;

                    if (quickRefresh)
                        MbApiInterface.MB_InvokeCommand((Command)4, null);
                    else
                        MbApiInterface.MB_RefreshPanels();

                    MbApiInterface.MB_SetBackgroundTaskMessage(LastMessage);
                }
                else
                {
                    UiRefreshingIsNeeded = true;
                }
            }
        }

        internal static void SetStatusBarText(string newMessage, bool autoClear)
        {
            if (autoClear)
            {
                if (LastMessage != newMessage)
                    MbApiInterface.MB_SetBackgroundTaskMessage(newMessage);

                DelayedStatusBarTextClearingTimer = new System.Threading.Timer(delayedStatusBarTextClearing, null, (int)RefreshUI_Delay.TotalMilliseconds * 2, 0);
            }
            else if (LastMessage != newMessage)
            {
                MbApiInterface.MB_SetBackgroundTaskMessage(newMessage);
                LastMessage = newMessage;
            }
        }

        private static void delayedStatusBarTextClearing(object state)
        {
            MbApiInterface.MB_SetBackgroundTaskMessage(string.Empty);
            DelayedStatusBarTextClearingTimer.Dispose();
        }

        internal static string RemoveMSIdAtTheEndOfString(string value)
        {
            value += EndOfStringId;
            value = value.Replace(string.Empty + MultipleItemsSplitterId + EndOfStringId, string.Empty);
            value = value.Replace(string.Empty + EndOfStringId, string.Empty);

            return value;
        }

        internal static string ReplaceMSChars(string value)
        {
            value = value.Replace(SavedSettings.multipleItemsSplitterChar2, string.Empty + MultipleItemsSplitterId);
            value = value.Replace(SavedSettings.multipleItemsSplitterChar1, string.Empty + MultipleItemsSplitterId);

            return value;
        }

        internal static string ReplaceMSIds(string value)
        {
            value = value.Replace(string.Empty + MultipleItemsSplitterId, SavedSettings.multipleItemsSplitterChar2);

            return value;
        }

        internal static string RemoveRoleIds(string value)
        {
            value = value.Replace(string.Empty + GuestId, string.Empty);
            value = value.Replace(string.Empty + PerformerId, string.Empty);
            value = value.Replace(string.Empty + RemixerId, string.Empty);

            return value;
        }

        internal static MetaDataType GetTagId(string tagName)
        {
            if (tagName == null)
                tagName = string.Empty;

            if (tagName.Length > 1 && (tagName[0] == '★' || tagName[0] == '☆' || tagName[0] == '✪' || tagName[0] == '♺' || tagName[0] == '❏'))
                tagName = tagName.Substring(2);

            if (tagName == DateCreatedTagName)
                return DateCreatedTagId;

            if (tagName == FolderTagName)
                return FolderTagId;

            if (tagName == FileNameTagName)
                return FileNameTagId;

            if (tagName == FilePathTagName)
                return FilePathTagId;

            if (tagName == FilePathWoExtTagName)
                return FilePathWoExtTagId;

            if (tagName == SequenceNumberName) //TagId not used for this pseudo-tag, but it must be not 0 for compatibility with LR 
                return (MetaDataType)10000;

            if (TagNamesIds.TryGetValue(tagName, out var tagId))
            {
                return tagId;
            }
            else
            {
                if (PropNamesIds.TryGetValue(tagName, out var propId))
                {
                    return (MetaDataType)propId;
                }
                else
                {
                    return 0;
                }
            }
        }

        internal static string GetTagName(MetaDataType tagId, string allTagsTagName = null)
        {
            if (tagId == DateCreatedTagId)
                return DateCreatedTagName;

            if (tagId == FolderTagId)
                return FolderTagName;

            if (tagId == FileNameTagId)
                return FileNameTagName;

            if (tagId == FilePathWoExtTagId)
                return FilePathWoExtTagName;

            if (tagId == FilePathTagId)
                return FilePathTagName;

            if (tagId == AllTagsPseudoTagId)
            {
                if (allTagsTagName == null)
                    return AllTagsPseudoTagName;
                else
                    return allTagsTagName;
            }


            if (TagIdsNames.TryGetValue(tagId, out var tagName))
            {
                return tagName;
            }
            else if (PropIdsNames.TryGetValue((FilePropertyType)tagId, out tagName))
            {
                return tagName;
            }
            else
            {
                return string.Empty;
            }
        }

        internal static string GetPropPrefix(string propName)
        {
            if (PropNamesIds.ContainsKey(propName))
                return "❏ ";

            return "⁈ ";
        }

        internal static string GetTagPrefix(string tagName)
        {
            string prefix;

            if (!TagNamesIds.TryGetValue(tagName, out var tagId))
                return GetPropPrefix(tagName);


            switch (tagId)
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

                    prefix = "★";
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

                    prefix = "♺";
                    break;

                default:

                    if (ReadonlyTagsNames.Contains(tagName))
                        prefix = "✪";
                    else
                        prefix = "☆";
                    break;
            }

            prefix += " ";

            return prefix;
        }

        internal static void FillListByTagNames(System.Collections.IList list, bool addReadOnlyTagsAlso = false, bool addArtworkAlso = false,
            bool addNullAlso = true, bool addTagPrefixes = false, bool addAllTagsPseudoTagAlso = false, bool addDateCreatedAlso = true, bool addCustomTagsOnly = false)
        {
            foreach (var tagName in TagNamesIds.Keys)
            {
                if (addCustomTagsOnly && !CustomTagNames.Contains(tagName))
                    continue;


                var prefix = string.Empty;

                if (addTagPrefixes)
                    prefix = GetTagPrefix(tagName);

                if (tagName == ArtworkName)
                {
                    if (addArtworkAlso)
                        list.Add(prefix + tagName);
                }
                else if (tagName == DateCreatedTagName)
                {
                    if (addDateCreatedAlso)
                        list.Add(prefix + tagName);
                }
                else if (tagName == AllTagsPseudoTagName)
                {
                    if (addAllTagsPseudoTagAlso)
                        list.Add(prefix + tagName);
                }
                else if (tagName == NullTagName)
                {
                    if (addNullAlso)
                        list.Add(prefix + tagName);
                }
                else if (addReadOnlyTagsAlso || !ChangeCase.IsItemContainedInArray(tagName, ReadonlyTagsNames))
                {
                    list.Add(prefix + tagName);
                }
            }
        }

        internal static FilePropertyType GetPropId(string propName)
        {
            if (PropNamesIds.TryGetValue(propName, out var propId))
            {
                return propId;
            }
            else
            {
                return 0;
            }
        }

        internal static string GetPropName(FilePropertyType propId)
        {
            if (PropIdsNames.TryGetValue(propId, out var propName))
            {
                return propName;
            }
            else
            {
                return string.Empty;
            }
        }

        internal static void FillListByPropNames(System.Collections.IList list, bool addTagMarker = false)
        {
            var marker = string.Empty;

            if (addTagMarker)
                marker = GetPropPrefix(null);

            foreach (var propName in PropNamesIds.Keys)
                list.Add(marker + propName);
        }

        internal static void InitializeSbText()
        {
            LastCommandSbText = string.Empty;
            LastPreview = true;
            LastFileCounter = 0;
            LastFileCounterTotal = 0;

        }

        internal static void SetResultingSbText(string finalStatus = null, bool autoClear = true, bool sbSetFilesAsItems = false)
        {
            if (sbSetFilesAsItems)
                SbItemNames = SbItems;

            if (LastCommandSbText == string.Empty)
            {
                SetStatusBarText(string.Empty, false);
                return;
            }
            else if (LastCommandSbText == "<CUESHEET>")
            {
                System.Media.SystemSounds.Exclamation.Play();
                SetStatusBarText(CtlWholeCuesheetWillBeReencoded, false);
                return;
            }
            else if (finalStatus == null)
            {
                string sbText;

                if (LastPreview)
                    sbText = LastCommandSbText + ": " + "100% (" + LastFileCounter  + " " + SbItemNames + ") " + SbRead;
                else
                    sbText = LastCommandSbText + ": " + "100% (" + LastFileCounter  + " " + SbItemNames + ") " + SbUpdated;

                //if (lastPreview)
                //   sbText = LastCommandSbText + ": 100% " + sbRead;
                //else
                //   sbText = LastCommandSbText + ": 100% " + sbUpdated;

                SetStatusBarText(sbText, autoClear);
            }
            else
            {
                SetStatusBarText(GenegateStatusBarTextForFileOperations(LastCommandSbText, LastPreview, LastFileCounter, LastFileCounterTotal, finalStatus), autoClear);
            }


            if (!sbSetFilesAsItems)
                SbItemNames = SbFiles;
        }

        internal static string GenegateStatusBarTextForFileOperations(string commandSbText, bool preview, int fileCounter1Based, int filesTotal, string currentFile = null)
        {
            string sbText;

            if (filesTotal == 0)
            {
                if (preview)
                    sbText = commandSbText + " (" + SbReading + "): " + fileCounter1Based + "/" + filesTotal + " ???";
                else
                    sbText = commandSbText + " (" + SbUpdating + "): " + fileCounter1Based + "/" + filesTotal + " ???";
            }
            else
            {
                if (preview)
                    sbText = commandSbText + " (" + SbReading + "): " + Math.Round(100d * fileCounter1Based / filesTotal, 0) + "%";
                else
                    sbText = commandSbText + " (" + SbUpdating + "): " + Math.Round(100d * fileCounter1Based / filesTotal, 0) + "%";
            }

            if (currentFile != null)
                sbText += " (" + currentFile + ")";

            return sbText;
        }

        internal static void SetStatusBarTextForFileOperations(string commandSbText, bool preview, int fileCounter0Based, int filesTotal, string currentFile = null, int updatePercentage = 10)
        {
            LastCommandSbText = commandSbText;
            LastPreview = preview;
            LastFileCounter = ++fileCounter0Based; //Let's make fileCounter0Based 1-BASED!
            LastFileCounterTotal = filesTotal;

            if (fileCounter0Based < 2)
                LastFileCounterThreshold = 0;

            var update = false;
            if (filesTotal == 0 || (fileCounter0Based - LastFileCounterThreshold) / (float)filesTotal >= updatePercentage / 100f)
            {
                update = true;
                LastFileCounterThreshold = fileCounter0Based;
            }

            if (updatePercentage == 0 || update || fileCounter0Based % StatusBarTextUpdateInterval == 0)
            {
                SetStatusBarText(GenegateStatusBarTextForFileOperations(commandSbText, preview, fileCounter0Based, filesTotal, currentFile), false);
            }
        }

        private static void RegularUiRefresh(object state)
        {
            if (UiRefreshingIsNeeded)
            {
                lock (LastUI_RefreshLocker)
                {
                    LastUI_Refresh = DateTime.UtcNow;
                }

                UiRefreshingIsNeeded = false;
                MbApiInterface.MB_RefreshPanels();
                MbApiInterface.MB_SetBackgroundTaskMessage(LastMessage);
            }
        }

        internal static void RegularAutoBackup(object state)
        {
            MbApiInterface.MB_SetBackgroundTaskMessage(SbAutoBackingUp);
            BackupIndex.saveBackup(BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory) + @"\" + BrGetDefaultBackupFilename(SavedSettings.autoBackupPrefix), SbAutoBackingUp, true, false);


            var autoDeleteKeepNumberOfDays = SavedSettings.autoDeleteKeepNumberOfDays > 0 ? SavedSettings.autoDeleteKeepNumberOfDays : 1000000;
            var autoDeleteKeepNumberOfFiles = SavedSettings.autoDeleteKeepNumberOfFiles > 0 ? SavedSettings.autoDeleteKeepNumberOfFiles : 1000000;

            var files = Directory.GetFiles(BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory), "*.mbc", SearchOption.TopDirectoryOnly);
            var filesWithNegativeDates = new SortedDictionary<int, string>();
            var fileCreationDates = new SortedDictionary<string, DateTime>();

            BackupCache backupCache;
            var currentLibraryName = BrGetCurrentLibraryName();

            for (var i = 0; i < files.Length; i++)
            {
                var backupFilenameWithoutExtension = BrGetBackupFilenameWithoutExtension(files[i]);

                backupCache = BackupCache.Load(backupFilenameWithoutExtension);
                if (backupCache == null)
                    return;

                if (backupCache.isAutoCreated && backupCache.libraryName == currentLibraryName)
                {
                    var negativeDate = -((backupCache.creationDate.Year * 10000 + backupCache.creationDate.Month * 100 + backupCache.creationDate.Day) * 1000000 +
                                         backupCache.creationDate.Hour * 10000 + backupCache.creationDate.Minute * 100 + backupCache.creationDate.Second);

                    filesWithNegativeDates.Add(negativeDate, backupFilenameWithoutExtension);
                    fileCreationDates.Add(backupFilenameWithoutExtension, backupCache.creationDate);
                }
            }


            var backupsToDelete = new List<string>();
            var now = DateTime.UtcNow;

            foreach (var fileWithNegativeDate in filesWithNegativeDates)
            {
                if (now - fileCreationDates[fileWithNegativeDate.Value] > new TimeSpan((int)autoDeleteKeepNumberOfDays, 0, 0, 0))
                    backupsToDelete.Add(fileWithNegativeDate.Value);
            }


            var keptCount = 1;
            foreach (var fileWithNegativeDate in filesWithNegativeDates)
            {
                if (keptCount > autoDeleteKeepNumberOfFiles)
                    backupsToDelete.AddUnique(fileWithNegativeDate.Value);
                else
                    keptCount++;
            }


            foreach (var backup in backupsToDelete)
            {
                backupCache = BackupCache.Load(backup);
                if (backupCache == null)
                {
                    SetStatusBarText(SbBackupRestoreCantDeleteBackupFile, false);

                    if (SavedSettings.playCanceledSound)
                        System.Media.SystemSounds.Hand.Play();

                    return;
                }

                BackupIndex.deleteBackup(backupCache);

                File.Delete(backup + ".xml");
                File.Delete(backup + ".mbc");
            }

            MbApiInterface.MB_SetBackgroundTaskMessage(string.Empty);
        }
        #endregion

        #region Menu handlers
        internal void openWindowActivationEventHandler(object sender, EventArgs e)
        {
            var tagToolsForm = ((sender as ToolStripMenuItem).Tag as PluginWindowTemplate);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void copyTagEventHandler(object sender, EventArgs e)
        {
            var tagToolsForm = new CopyTag(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void swapTagsEventHandler(object sender, EventArgs e)
        {
            var tagToolsForm = new SwapTags(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void changeCaseEventHandler(object sender, EventArgs e)
        {
            var tagToolsForm = new ChangeCase(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void reencodeTagEventHandler(object sender, EventArgs e)
        {
            var tagToolsForm = new ReEncodeTag(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void reencodeTagsEventHandler(object sender, EventArgs e)
        {
            var tagToolsForm = new ReEncodeTags(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void libraryReportsEventHandler(object sender, EventArgs e)
        {
            var tagToolsForm = new LibraryReports(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void autoRateEventHandler(object sender, EventArgs e)
        {
            var tagToolsForm = new AutoRate(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void asrEventHandler(object sender, EventArgs e)
        {
            var tagToolsForm = new AdvancedSearchAndReplace(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void carEventHandler(object sender, EventArgs e)
        {
            var tagToolsForm = new CalculateAverageAlbumRating(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void copyTagsToClipboardEventHandler(object sender, EventArgs e)
        {
            var tagToolsForm = new CopyTagsToClipboard(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void pasteTagsFromClipboardEventHandler(object sender, EventArgs e)
        {
            PasteTagsFromClipboard();
        }

        internal void multipleSearchReplaceEventHandler(object sender, EventArgs e)
        {
            var tagToolsForm = new MultipleSearchAndReplace(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void showHiddenEventHandler(object sender, EventArgs e)
        {
            lock (OpenedForms)
            {
                foreach (var form in OpenedForms)
                {
                    if (!form.Visible || form.WindowState == FormWindowState.Minimized)
                        PluginWindowTemplate.Display(form);
                }
            }
        }

        internal void backupTagsEventHandler(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Title = CtlSaveBackupTitle,
                Filter = CtlMusicBeeBackupType,
                InitialDirectory = BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory),
                FileName = BrGetDefaultBackupFilename(BackupDefaultPrefix)
            };

            if (dialog.ShowDialog(MbForm) == DialogResult.Cancel) return;

            MbApiInterface.MB_SetBackgroundTaskMessage(SbMakingTagBackup);

            if (File.Exists(dialog.FileName))
                File.Delete(dialog.FileName);
            if (File.Exists(BrGetBackupFilenameWithoutExtension(dialog.FileName) + ".mbc"))
                File.Delete(BrGetBackupFilenameWithoutExtension(dialog.FileName) + ".mbc");

            MbApiInterface.MB_CreateParameterisedBackgroundTask(BackupIndex.saveBackupAsync, new object[] { BrGetBackupFilenameWithoutExtension(dialog.FileName), SbMakingTagBackup, false, false }, MbForm);
            
            dialog.Dispose();
        }

        internal void restoreTagsEventHandler(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = CtlRestoreBackupTitle,
                Filter = CtlMusicBeeBackupType,
                InitialDirectory = BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory)
            };

            if (dialog.ShowDialog(MbForm) == DialogResult.Cancel) return;

            MbApiInterface.MB_SetBackgroundTaskMessage(SbRestoringTagsFromBackup);
            MbApiInterface.MB_CreateParameterisedBackgroundTask(BackupIndex.LoadBackupAsync, new object[] { BrGetBackupFilenameWithoutExtension(dialog.FileName), SbRestoringTagsFromBackup, false, false }, MbForm);

            dialog.Dispose();
        }

        //EventArgs e == null means "restore from another lbrary, don't ask confirmation"
        internal void restoreTagsForEntireLibraryEventHandler(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = CtlRestoreBackupTitle,
                Filter = CtlMusicBeeBackupType,
                InitialDirectory = BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory)
            };

            if (dialog.ShowDialog(MbForm) == DialogResult.Cancel) return;

            MbApiInterface.MB_SetBackgroundTaskMessage(SbRestoringTagsFromBackup);
            MbApiInterface.MB_CreateParameterisedBackgroundTask(BackupIndex.LoadBackupAsync, new object[] { BrGetBackupFilenameWithoutExtension(dialog.FileName), SbRestoringTagsFromBackup, true, e == null }, MbForm);

            dialog.Dispose();
        }

        internal void renameMoveBackupEventHandler(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog
            {
                Title = CtlRenameSelectBackupTitle,
                Filter = CtlMusicBeeBackupType,
                InitialDirectory = BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory)
            };

            if (openDialog.ShowDialog(MbForm) == DialogResult.Cancel) return;

            var saveDialog = new SaveFileDialog
            {
                Title = CtlRenameSaveBackupTitle,
                Filter = CtlMusicBeeBackupType,
                InitialDirectory = BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory),
                FileName = openDialog.SafeFileName
            };

            if (saveDialog.ShowDialog(MbForm) == DialogResult.Cancel) return;

            if (openDialog.FileName == saveDialog.FileName) return;

            MbApiInterface.MB_SetBackgroundTaskMessage(SbRenamingMovingBackup);

            if (File.Exists(saveDialog.FileName))
                File.Delete(saveDialog.FileName);
            if (File.Exists(BrGetBackupFilenameWithoutExtension(saveDialog.FileName) + ".mbc"))
                File.Delete(BrGetBackupFilenameWithoutExtension(saveDialog.FileName) + ".mbc");

            File.Move(openDialog.FileName, saveDialog.FileName);
            File.Move(BrGetBackupFilenameWithoutExtension(openDialog.FileName) + ".mbc", BrGetBackupFilenameWithoutExtension(saveDialog.FileName) + ".mbc");

            MbApiInterface.MB_SetBackgroundTaskMessage(string.Empty);
            
            openDialog.Dispose();
            saveDialog.Dispose();
        }

        internal void moveBackupsEventHandler(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog
            {
                Title = CtlMoveSelectBackupsTitle,
                Filter = CtlMusicBeeBackupType,
                InitialDirectory = BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory),
                Multiselect = true
            };

            if (openDialog.ShowDialog(MbForm) == DialogResult.Cancel) return;

            var saveDialog = new SaveFileDialog
            {
                Title = CtlMoveSaveBackupsTitle,
                Filter = "|*.destinationfolder",
                //saveDialog.InitialDirectory = GetAutoBackupDirectory(SavedSettings.autoBackupDirectory);
                FileName = CtlSelectThisFolder
            };

            if (saveDialog.ShowDialog(MbForm) == DialogResult.Cancel) return;

            var sourceFolder = Regex.Replace(openDialog.FileNames[0], @"(.*)\\.*", "$1");
            var destinationFolder = Regex.Replace(saveDialog.FileName, @"(.*)\\.*", "$1");

            if (sourceFolder == destinationFolder) return;

            MbApiInterface.MB_SetBackgroundTaskMessage(SbMovingBackups);

            foreach (var filename in openDialog.SafeFileNames)
            {
                if (File.Exists(destinationFolder + @"\" + filename))
                    File.Delete(destinationFolder + @"\" + filename);
                if (File.Exists(BrGetBackupFilenameWithoutExtension(destinationFolder + @"\" + filename) + ".mbc"))
                    File.Delete(BrGetBackupFilenameWithoutExtension(destinationFolder + @"\" + filename) + ".mbc");

                File.Move(sourceFolder + @"\" + filename, destinationFolder + @"\" + filename);
                File.Move(BrGetBackupFilenameWithoutExtension(sourceFolder + @"\" + filename) + ".mbc", BrGetBackupFilenameWithoutExtension(destinationFolder + @"\" + filename) + ".mbc");
            }

            MbApiInterface.MB_SetBackgroundTaskMessage(string.Empty);
            
            openDialog.Dispose();
            saveDialog.Dispose();
        }


        //EventArgs e == null means "create empty baseline backup"
        internal void createNewBaselineEventHandler(object sender, EventArgs e)
        {
            if (e != null)
                MessageBox.Show(MbForm, MsgBrCreateBaselineWarning, CtlWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);


            var dialog = new SaveFileDialog
            {
                Title = CtlSaveBackupTitle,
                Filter = CtlMusicBeeBackupType,
                InitialDirectory = BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory),
                FileName = BrGetDefaultBackupFilename(BackupDefaultPrefix)
            };

            if (dialog.ShowDialog(MbForm) == DialogResult.Cancel) return;


            lock (TracksNeededToBeBackedUp)
            {
                //Let's delete all incremental backups of current library
                var currentLibraryName = BrGetCurrentLibraryName();
                var files = Directory.GetFiles(BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory), "*.mbc", SearchOption.TopDirectoryOnly);

                for (var i = 0; i < files.Length; i++)
                {
                    var backupFilenameWithoutExtension = BrGetBackupFilenameWithoutExtension(files[i]);

                    var backupCache = BackupCache.Load(backupFilenameWithoutExtension);

                    if (backupCache == null || backupCache.libraryName == currentLibraryName)
                    {
                        if (backupCache != null)
                            BackupIndex.deleteBackup(backupCache);

                        File.Delete(backupFilenameWithoutExtension + ".mbc");
                        File.Delete(backupFilenameWithoutExtension + ".xml");
                    }
                }

                //Let's delete baseline
                File.Delete(BrGetBackupBaselineFilename());
            }


            //Now let's create new baseline
            MbApiInterface.MB_SetBackgroundTaskMessage(SbMakingTagBackup);

            if (File.Exists(dialog.FileName))
                File.Delete(dialog.FileName);

            if (File.Exists(BrGetBackupFilenameWithoutExtension(dialog.FileName) + ".mbc"))
                File.Delete(BrGetBackupFilenameWithoutExtension(dialog.FileName) + ".mbc");

            if (UseCustomTrackIdTag)
            {
                var useCustomTrackIdTag = UseCustomTrackIdTag;
                var customTrackIdTag = CustomTrackIdTag;

                //Let's cache previous value in case of baseline backup creation failure (which is handled inside (BackupIndex.saveBackupAsync())
                UseCustomTrackIdTag = SavedSettings.useCustomTrackIdTag;
                CustomTrackIdTag = SavedSettings.customTrackIdTag;

                SavedSettings.useCustomTrackIdTag = useCustomTrackIdTag;
                SavedSettings.customTrackIdTag = customTrackIdTag;
            }

            MbApiInterface.MB_CreateParameterisedBackgroundTask(BackupIndex.saveBackupAsync, new object[] { BrGetBackupFilenameWithoutExtension(dialog.FileName), SbMakingTagBackup, false, e == null }, MbForm);
            
            dialog.Dispose();
        }

        internal void createEmptyBaselineRestoreTagsForEntireLibraryEventHandler(object sender, EventArgs e)
        {
            //createNewBaselineEventHandler(sender, NULL) means "create empty baseline backup"
            createNewBaselineEventHandler(sender, null);
            //restoreTagsForEntireLibraryEventHandler(sender, NULL) means "restore from another lbrary, don't ask confirmation"
            restoreTagsForEntireLibraryEventHandler(sender, null);

            MessageBox.Show(MbForm, MsgBrCreateNewBaseline, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        internal void deleteBackupsEventHandler(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = CtlDeleteBackupsTitle,
                Filter = CtlMusicBeeBackupType,
                InitialDirectory = BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory),
                Multiselect = true
            };

            if (dialog.ShowDialog(MbForm) == DialogResult.Cancel) return;

            MbApiInterface.MB_SetBackgroundTaskMessage(SbDeletingBackups);

            foreach (var filename in dialog.FileNames)
            {
                var backupCache = BackupCache.Load(BrGetBackupFilenameWithoutExtension(filename));

                if (backupCache != null)
                    BackupIndex.deleteBackup(backupCache);

                File.Delete(filename);
                File.Delete(BrGetBackupFilenameWithoutExtension(filename) + ".mbc");
            }

            MbApiInterface.MB_SetBackgroundTaskMessage(string.Empty);
            
            dialog.Dispose();
        }

        internal void autoBackupSettingsEventHandler(object sender, EventArgs e)
        {
            var tagToolsForm = new AutoBackupSettings(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void tagHistoryEventHandler(object sender, EventArgs e)
        {
            if (MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out var files))
            {
                var trackIds = new int[files.Length];
                for (var i = 0; i < files.Length; i++)
                    trackIds[i] = GetPersistentTrackIdInt(files[i], true);


                var tagToolsForm = new TagHistory(this, files, trackIds);
                PluginWindowTemplate.Display(tagToolsForm);
            }
        }

        internal void compareTracksEventHandler(object sender, EventArgs e)
        {

            if (MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out var files))
            {
                if (files.Length < 2)
                {
                    MessageBox.Show(MbForm, MsgCtSelectAtLeast2Tracks, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }


                var tagToolsForm = new CompareTracks(this, files);
                PluginWindowTemplate.Display(tagToolsForm);
            }
        }

        internal void helpEventHandler(object sender, EventArgs e)
        {
            Process.Start(PluginHelpFilePath);
        }

        internal void webPageEventHandler(object sender, EventArgs e)
        {
            Process.Start(PluginWebPage);
        }
        #endregion

        #region Main plugin initialization
        public PluginInfo Initialise(IntPtr apiInterfacePtr)
        {
            #region General initialization
            Application.EnableVisualStyles();

            MbApiInterface = new MusicBeeApiInterface();
            MbApiInterface.Initialise(apiInterfacePtr);
            #endregion

            #region English localization
            //Localizable strings

            //MusicBeePlugin localizable strings
            PluginName = "Additional Tagging & Reporting Tools";
            PluginMenuGroupName = "Additional Tagging && Reporting Tools";
            PluginDescription = "Adds some tagging & reporting tools to MusicBee";

            PluginHelpString = "Help...";
            PluginWebPageString = "Plugin Web Page..."; ;
            PluginWebPageToolTip = "Open Plugin Web Page (to check/download the latest version";
            PluginVersionString = "Version: ";

            OpenWindowsMenuSectionName = "OPEN WINDOWS";
            TagToolsMenuSectionName = "TAGGING && REPORTING";
            BackupRestoreMenuSectionName = "BACKUP && RESTORE";

            CopyTagName = "Copy Tag...";
            SwapTagsName = "Swap Tags...";
            ChangeCaseName = "Change Case...";
            ReEncodeTagName = "Reencode Tag...";
            ReEncodeTagsName = "Reencode Tags...";
            LibraryReportsName = "Library Reports...";
            AutoRateName = "Auto Rate Tracks...";
            AsrName = "Advanced Search && Replace...";
            CarName = "Calculate Average Album Rating...";
            CompareTracksName = "Compare Tracks...";
            CopyTagsToClipboardName = "Copy Tags to Clipboard...";
            PasteTagsFromClipboardName = "Paste Tags from Clipboard";
            MsrName = "Multiple Search && Replace...";
            ShowHiddenWindowsName = "Show hidden/restore minimized plugin windows";

            TagToolsHotkeyDescription = "Tagging Tools: ";
            CopyTagDescription = TagToolsHotkeyDescription + "Copy Tag";
            SwapTagsDescription = TagToolsHotkeyDescription + "Swap Tags";
            ChangeCaseDescription = TagToolsHotkeyDescription + "Change Case";
            ReEncodeTagDescription = TagToolsHotkeyDescription + "Reencode Tag";
            ReEncodeTagsDescription = TagToolsHotkeyDescription + "Reencode Tags";
            LibraryReportsDescription = TagToolsHotkeyDescription + "Library Reports";
            LrPresetHotkeyDescription = TagToolsHotkeyDescription;
            AutoRateDescription = TagToolsHotkeyDescription + "Auto Rate Tracks";
            AsrDescription = TagToolsHotkeyDescription + "Advanced Search & Replace";
            AsrPresetHotkeyDescription = TagToolsHotkeyDescription;
            CarDescription = TagToolsHotkeyDescription + "Calculate Average Album Rating";
            CompareTracksDescription = TagToolsHotkeyDescription + "Compare Tracks";
            CopyTagsToClipboardDescription = TagToolsHotkeyDescription + "Copy Tags to Clipboard";
            PasteTagsFromClipboardDescription = TagToolsHotkeyDescription + "Paste Tags from Clipboard";
            CopyTagsToClipboardUsingMenuDescription = "Copy Tags to Clipboard Using Tag Set";
            MsrCommandDescription = TagToolsHotkeyDescription + "Multiple Search & Replace";
            ShowHiddenWindowsDescription = TagToolsHotkeyDescription + "Show hidden/restore minimized plugin windows";

            BackupTagsName = "Backup Tags for All Tracks...";
            RestoreTagsName = "Restore Tags for Selected Tracks...";
            RestoreTagsForEntireLibraryName = "Restore Tags for All Tracks...";
            RenameMoveBackupName = "Rename or Move Backup...";
            MoveBackupsName = "Move Backup(s)...";
            CreateNewBaselineName = "Create New Baseline of Current Library...";
            createEmptyBaselineRestoreTagsForEntireLibraryName = "Create Empty Baseline and Restore Tags from Another Library...";
            DeleteBackupsName = "Delete Backup(s)...";
            AutoBackupSettingsName = "(Auto)backup Settings...";

            TagHistoryName = "Tag History....";

            BackupTagsDescription = TagToolsHotkeyDescription + "Backup Tags for All Tracks";
            RestoreTagsDescription = TagToolsHotkeyDescription + "Restore Tags for Selected Tracks";
            RestoreTagsForEntireLibraryDescription = TagToolsHotkeyDescription + "Restore Tags for All Tracks";
            RenameMoveBackupDescription = TagToolsHotkeyDescription + "Rename or Move Backup";
            MoveBackupsDescription = TagToolsHotkeyDescription + "Move Backup(s)";
            CreateNewBaselineDescription = TagToolsHotkeyDescription + "Create New Baseline of Current Library";
            createEmptyBaselineRestoreTagsForEntireLibraryDescription = TagToolsHotkeyDescription + "Create Empty Baseline and Restore Tags from Another Library";
            DeleteBackupsDescription = TagToolsHotkeyDescription + "Delete Backup(s)";
            AutoBackupSettingsDescription = TagToolsHotkeyDescription + "(Auto)backup Settings";

            TagHistoryDescription = TagToolsHotkeyDescription + "Tag History";

            CopyTagSbText = "Copying tag";
            SwapTagsSbText = "Swapping tags";
            ChangeCaseSbText = "Changing case";
            CompareTracksSbText = "Comparing tracks";
            ReEncodeTagSbText = "Reencoding tags";
            LibraryReportsSbText = "Generating report";
            LibraryReportsGeneratingPreviewSbText = "Generating preview";
            ApplyingLrPresetSbText = "Applying LR preset";
            AutoRateSbText = "Auto rating tracks";
            AsrSbText = "Advanced searching and replacing";
            CarSbText = "Calculating average album rating";
            MsrSbText = "Multiple searching and replacing";
            TagHistorySbText = "Tag history";

            AnotherLrPresetIsRunningSbText = "Another library reports preset is running. Can't proceed!";
            CtlAnotherLrPresetIsRunning = "Another library reports preset is running. Please wait until it is completed...";
            CtlWaitUntilPresetIsCompleted = "\r\n\r\nStopping execution of presets, but currently running library reports preset must be completed. Please wait...";
            CtlLrElapsed = ", elapsed: ";

            //Other localizable strings
            AlbumTagName = MbApiInterface.Setting_GetFieldName(MetaDataType.Album);
            Custom9TagName = MbApiInterface.Setting_GetFieldName(MetaDataType.Custom9);
            UrlTagName = MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url);
            GenreCategoryName = MbApiInterface.Setting_GetFieldName(MetaDataType.GenreCategory);

            LrFunctionNames[0] = "<Grouping>";
            LrFunctionNames[1] = "Count";
            LrFunctionNames[2] = "Sum";
            LrFunctionNames[3] = "Minimum";
            LrFunctionNames[4] = "Maximum";
            LrFunctionNames[5] = "Average value";
            LrFunctionNames[6] = "Average count";

            LibraryReport = "Library report";


            DateCreatedTagName = "<Date Created>";
            EmptyValueTagName = "<Empty Value>";
            ClipboardTagName = "<Clipboard>";
            TextFileTagName = "<Text File>";

            FolderTagName = "<Folder>";
            FileNameTagName = "<Filename>";
            FilePathTagName = "<File Path>";
            FilePathWoExtTagName = "<Full Path w/o Ext.>";
            AlbumUniqueIdName = "<Album Unique Id>";

            ParameterTagName = "Tag";
            TempTagName = "Temp";

            LibraryTotalsPresetName = "Library totals";
            LibraryAveragesPresetName = "Library averages";
            CDBookletPresetName = "CD Booklet(s)";
            AlbumsAndTracksPresetName = "Albums & Tracks";
            AlbumGridPresetName = "Album Grid (album list)";

            EmptyPresetName = "<Empty preset>";


            //Let's determine casing rules for genre category as an example
            var changeCaseMode = ChangeCase.ChangeCaseOptions.SentenceCase;
            var genreCategory = MbApiInterface.Setting_GetFieldName(MetaDataType.GenreCategory).Split(' ');
            if (char.ToUpper(genreCategory[genreCategory.Length - 1][0]) == genreCategory[genreCategory.Length - 1][0])
                changeCaseMode = ChangeCase.ChangeCaseOptions.TitleCase;

            ArtistArtistsName = MbApiInterface.Setting_GetFieldName(MetaDataType.Artist);
            ComposerComposersName = MbApiInterface.Setting_GetFieldName(MetaDataType.Composer);

            DisplayedArtistName = MbApiInterface.MB_GetLocalisation("aSplit.msg.diar", "display {0}:").Replace("{0}", ArtistArtistsName);
            DisplayedArtistName = ChangeCase.ChangeWordsCase(DisplayedArtistName, changeCaseMode);
            DisplayedArtistName = DisplayedArtistName.Remove(DisplayedArtistName.Length - 1);

            DisplayedComposerName = MbApiInterface.MB_GetLocalisation("aSplit.msg.diar", "display {0}:").Replace("{0}", ComposerComposersName);
            DisplayedComposerName = ChangeCase.ChangeWordsCase(DisplayedComposerName, changeCaseMode);
            DisplayedComposerName = DisplayedComposerName.Remove(DisplayedComposerName.Length - 1);

            var localizedDisplayedAlbumArtist = MbApiInterface.MB_GetLocalisation("aSplit.msg.diar", "display {0}:");
            localizedDisplayedAlbumArtist = Regex.Replace(localizedDisplayedAlbumArtist, @"^(.*?)\s?\{0\}\:?\s?(.*)", " ($1$2)");
            DisplayedAlbumArtistName = MbApiInterface.Setting_GetFieldName(MetaDataType.AlbumArtist) + localizedDisplayedAlbumArtist;


            ArtworkName = MbApiInterface.Setting_GetFieldName(MetaDataType.Artwork);

            LyricsName = MbApiInterface.Setting_GetFieldName(MetaDataType.Lyrics);
            LyricsNamePostfix = MbApiInterface.MB_GetLocalisation("Main.msg.Y[s", "Y [synched/unsynched]").Substring(1);
            SynchronisedLyricsName = LyricsName + MbApiInterface.MB_GetLocalisation("Main.msg.Y[s.2", "Y [synched]").Substring(1);
            UnsynchronisedLyricsName = LyricsName + MbApiInterface.MB_GetLocalisation("Main.msg.Y[u", "Y [unsynched]").Substring(1);


            NullTagName = "<Null>";
            AllTagsPseudoTagName = "<ALL TAGS>";

            GenericTagSetName = "Tag set";

            //Supported exported file formats
            ExportedFormats = "HTML Document (grouped by albums)|*.htm|HTML Document|*.htm|Simple HTML table|*.htm|Tab delimited text|*.txt|M3U Playlist|*.m3u|" +
                "CSV file|*.csv|HTML Document (CD Booklet)|*.htm|HTML Document (Album Grid)|*.htm";
            ExportedTrackList = "LR Track List";


            //Displayed text
            SequenceNumberName = "#";

            //ListItemConditionIs = "is equal to";
            //ListItemConditionIsNot = "is not equal to";
            //ListItemConditionIsGreater = "is greater than";
            //ListItemConditionIsGreaterOrEqual = "is greater than or equal to";
            //ListItemConditionIsLess = "is less than";
            //ListItemConditionIsLessOrEqual = "is less than or equal to";

            ListItemConditionIs = "="; //***
            ListItemConditionIsNot = "!=";
            ListItemConditionIsGreater = ">";
            ListItemConditionIsGreaterOrEqual = ">=";
            ListItemConditionIsLess = "<";
            ListItemConditionIsLessOrEqual = "<=";

            SelectTagsWindowTitle = "Select Tags";
            SelectDisplayedTagsWindowTitle = "Select Displayed Tags";
            SelectButtonName = "Select";

            AsrProcessTagsButtonName = "Process tags";
            AsrPreserveTagsButtonName = "Preserve tags";

            LrButtonFilterResultsShowAllText = "Show All";
            LrButtonFilterResultsShowAllToolTip = "Show full preview";

            LrCellToolTip = "Ctrl-click to copy cell content to conditional export/saving compared field. \n" +
                "Double-click row border to auto-size row. \n" +
                "Shift-double-click row border to auto-size all rows. \n" +
                "Alt-resize the row to change the height of all rows. ";

            SbBrokenPresetRetrievalChain = "Broken preset retrieval chain for LR preset: %%PRESET-NAME%%! Check out the preset chain!";

            OKButtonName = "Proceed";
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

            DefaultAsrPresetName = "Preset #";

            Msg1stTimeUsage = "This is the first time you are using AT&RT plugin. Please read some notes before you continue. This message will not appear again.\r\n\r\n" +
                "Plugin's advanced features are very sophisticated and can be confusing, though basic ones are pretty simple to use.\r\n\r\n" +
                "But you can disable almost any part of plugin functionality (of course, you can reenable any part later). " +
                "Disabling any plugin menu item in settings hides not only the menu item itself " +
                "but also turns off the entire corresponding plugin functionality. " +
                "For example, disabling item \"Show 'Advanced Search & Replace' / 'Multiple Search & Replace' commands in menu\" will stop " +
                "ASR initialization and ASR preset auto-execution.\r\n\r\n" +
                "To open plugin settings manually, go to " +
                "MusicBee menu> Edit> Preferences> Plugins> Additional Tagging & Reporting Tools> Configure.\r\n\r\n" +
                "Do you want to open plugin settings now to adjust its functionality?";

            MsgFileNotFound = "File not found!";
            MsgNoTracksSelected = "No tracks selected.";
            MsgNoTracksDisplayed = "No tracks displayed.";
            MsgSourceAndDestinationTagsAreTheSame = "Both tags are the same. Nothing done.";
            MsgSwapTagsSourceAndDestinationTagsAreTheSame = "Using the same " +
                "source and destination tags may be useful only for \"Artist\"/\"Composer\" tags for conversion of \";\" delimited tag to the list of artists/composers and vice versa. Nothing done.";
            MsgNoTagsSelected = "No tags selected. Nothing to preview.";
            MsgNoTracksInCurrentView = "No tracks in current view.";
            MsgTrackListIsEmpty = "Track list is empty. Nothing to export. Click \"Preview\" first.";
            MsgPreviewIsNotGeneratedNothingToSave = "Preview is not generated. Nothing to save.";
            MsgPreviewIsNotGeneratedNothingToChange = "Preview is not generated. Nothing to change.";
            MsgPleaseUseGroupingFunction = "Please use <Grouping> function for tags \"" + ArtworkName + "\" and \"" + SequenceNumberName + "\"!";
            CtlAllTags = "ALL TAGS";
            MsgNoUrlColumnUnableToSave = "No \"" + UrlTagName + "\" tag in the table. Unable to save.";
            MsgEmptyURL = "Empty \"" + UrlTagName + "\" in row ";
            MsgUnableToSave = "Unable to save. ";
            MsgUsingEmptyValueAsSourceTagMayBeUsefulOnlyForAppendingStaticTextToDestinationTag = "Using '" + EmptyValueTagName +
                "\" as source tag is useful only with option \"Append source tag to the end of destination tag' for adding some static text to the destination tag. Nothing done.";
            MsgBackgroundTaskIsCompleted = "Additional tagging tools background task is completed.";
            MsgThresholdsDescription = "Auto ratings are based on the average number of times track is played every day \n" +
                "or \"plays per day\" virtual tag. You should set \"plays per day\" threshold values for every rating number \n" +
                "you want to be assigned to tracks. Thresholds are evaluated for track in descending order: from 5 star rating \n" +
                "to 0.5 star rating. \"Plays per day\" virtual tag can be evaluated for every track independent of rest of library, \n" +
                "so it's possible to automatically update auto rating if playing track is changed. \n" +
                "Also it's possible to update auto rating manually for selected tracks or automatically for entire library \n" +
                "on MusicBee startup. ";
            MsgAutoCalculationOfThresholdsDescription = "Auto calculation of thresholds is another way to set up auto ratings. This option allows you to set desirable weights \n" +
                "of auto rating values in your library. Actual weights may differ from desirable values because it's not always possible \n" +
                "to satisfy desirable weights (suppose all your tracks have the same \"plays per day\" value, it's obvious that there is no way to split \n" +
                "your library into several parts on the basis of \"plays per day\" value). Actual weights are displayed next to desired weights after calculation is made. \n" +
                "Because calculation of thresholds is time consuming operation it cannot be done automatically on any event except for MusicBee startup. ";

            MsgNumberOfPlayedTracks = "Ever played tracks in your library: ";
            MsgIncorrectSumOfWeights = "The sum of all weights must be equal or less than 100%!";
            MsgSum = "Sum: ";
            MsgNumberOfNotRatedTracks = "% of tracks rated as no stars)";
            MsgTracks = " tracks)";
            MsgActualPercent = "% / Act.: ";
            MsgIncorrectPresetName = "Incorrect preset name or duplicated preset names.";

            MsgCsTheNumberOfOpeningExceptionCharactersMustBe = "The number of opening exception characters must be the same as the number of closing exception characters!";

            MsgLrDoYouWantToSaveChangesBeforeClosingTheWindow = "There are unsaved changes. Do you want to save changes before closing the window?";
            MsgLrDoYouWantToCloseTheWindowWithoutSavingChanges = "You can’t save presets now. Would you like to discard any changes when closing the LR window?";

            MsgAsrDoYouWantToSaveChangesBeforeClosingTheWindow = "One or more presets have been customized or changed. Do you want to save changes before closing the window?";

            MsgDeletePresetConfirmation = "Do you want to delete selected preset?";
            MsgInstallingConfirmation = "Do you want to install predefined presets?";
            MsgDoYouWantToImportExistingPresetsAsCopies = "One or more imported presets already exist.\n"
                + "Do you want to import them as new presets, and keep your current presets?\n\n"
                + "OTHERWISE, EXISTING PRESETS WILL BE OVERWRITTEN!";
            MsgDoYouWantToResetYourCustomizedPredefinedPresets = "You have customized one or more predefined presets.\n"
                + "Do you want to reinstall your customized predefined presets, and replace them by latest versions from developer?\n\n"
                + "ALL YOUR CUSTOMIZATIONS WILL BE LOST!\n\n"
                + "Otherwise, customized presets will be updated to latest versions, but all their customizations will be preserved.";
            MsgDoYouWantToRemovePredefinedPresets = "Some predefined presets are obsolete and have been removed from the plugin package.\n\n"
                + "Do you want to delete these presets?";

            MsgPresetsWereImported = " preset{;s;s} {was;were;were} imported.\n";
            MsgMsrPresetWasImported = " service hidden preset was imported.\n";
            MsgPresetsWereImportedAsCopies = " preset{;s;s} {was;were;were} imported as new preset{;s;s}.\n";
            MsgPresetsFailedToImport = " preset{;s;s} failed to import due to file\n" + AddLeadingSpaces(0, 4, 0) +
                " read error{;s;s} or wrong format.";

            MsgPresetsWereInstalled = " preset{;s;s} {was;were;were} installed.\n";
            MsgMsrPresetWasInstalled = " service hidden preset was installed.\n";
            MsgPresetsCustomizedWereReinstalled = " preset{;s;s} customized by you {was;were;were} reinstalled,\n" + AddLeadingSpaces(0, 4, 0)
                + " but {its;their;their} customizations were preserved.\n";
            MsgPresetsWereReinstalled = " preset{;s;s} {was;were;were} reinstalled.\n";
            MsgPresetsWereUpdated = " presets {was;were;were} updated.\n";
            MsgPresetsCustomizedWereUpdated = " preset{;s;s} customized by you {was;were;were} updated,\n" + AddLeadingSpaces(0, 4, 0)
                + " but {its;their;their} customizations were preserved.\n";
            MsgPresetsWereNotChanged = " preset{;s;s} {was;were;were} not changed since\n" + AddLeadingSpaces(0, 4, 0)
                + " last update, and skipped.\n\n";
            MsgPresetsRemoved = " predefined preset{;s;s} {was;were;were} deleted.\n";
            MsgPresetsFailedToUpdate = " preset{;s;s} failed to install due to file\n" + AddLeadingSpaces(0, 4, 0)
                + " read error{;s;s} or wrong format.";

            MsgPresetsNotFound = "No presets for installing found in expected directory!";

            MsgDeletingConfirmation = "Do you want to delete all predefined presets?";
            MsgNoPresetsDeleted = "No presets were deleted. ";
            MsgPresetsWereDeleted = " preset{;s;s} {was;were;were} deleted.";


            var msgLrCachedPresetsInitialFilling = "These functions will use these tags as the cache. If you change some tags or add new tracks " +
                                                   "to the library, this cache will dynamically update. However, it’s HIGHLY RECOMMENDED first to fill this cache for all existing " +
                                                   "tracks/current tags in the current library to avoid UI slowdowns and freezes when using MusicBee regularly. \n\n" +
                                                   "Do you want to execute all affected presets automatically now? " +
                                                   "This may take a while.";

            MsgLrCachedPresetsChanged = "You have created or changed %%CHANGED-PRESET-COUNT%% LR preset{;s;s}, which use{s;;} LR functions " +
                "with assigned IDs AND saved to tags. " + msgLrCachedPresetsInitialFilling;

            MsgLrCachedPresetsNotApplied = "You have some LR presets which use LR functions " +
                "with assigned IDs AND saved to tags. " + msgLrCachedPresetsInitialFilling;


            MsgNumberOfTagsInTextFileDoesntCorrespondToNumberOfSelectedTracks = "Number of tags in text file (%%TEXT-TAG-FILES-COUNT%%)" +
                " doesn't correspond to number of selected tracks (%%SELECTED-FILES-COUNT%%)!";

            MsgClipboardDoesntContainText = "Clipboard doesn't contain text!";

            MsgClipboardDoesntContainTags = "Clipboard doesn't contain tags!";

            MsgUnknownTagNameInClipboard = "Unknown tag name in clipboard: %%TAG-NAME%%!";

            MsgNumberOfTagsInClipboardDoesntCorrespondToNumberOfSelectedTracks = "The number of tags of tracks in clipboard (%%FILE-TAGS-LENGTH%%)" +
                " doesn't correspond to the number of selected tracks (%%SELECTED-FILES-COUNT%%)!";
            MsgDoYouWantToPasteTagsAnyway = " Do you want to paste tags anyway?";
            MsgWrongNumberOfCopiedTags = "Wrong number of copied tags in clipboard (%%CLIPBOARD-TAGS-COUNT%%)" +
                " at line %%CLIPBOARD-LINE%% of clipboard text!";
            MsgMatchTracksByPathQuestion = "Clipboard contains %%MATCH-TAG-NAME%% pseudo-tag. Do you want to match tracks" +
                " for pasted tags according to this tag (otherwise tracks for pasted tags will be selected according to their displayed order)?";
            MsgTracksSelectedMatchedNotMatched = "Selected tracks: %%SELECTED-TRACKS%%\n" +
                "Tags of tracks in clipboard: %%CLIPBOARD-TRACKS%%\n" +
                "Matched tracks: %%MATCHED-TRACKS%%\n" +
                "Not matched tracks: %%NOT-MATCHED-TRACKS%%\n\n" +
                "Paste tags?";

            MsgFirstThreeGroupingFieldsInPreviewTableShouldBe = "First three grouping fields in preview table should be \"" + DisplayedAlbumArtistName + "\", \"" + AlbumTagName + "\" and \"" + ArtworkName + "\" to export" +
                " to \"HTML Document (grouped by album)\"!";
            MsgFirstSixGroupingFieldsInPreviewTableShouldBe = "First six grouping fields in preview table should be '" + SequenceNumberName
                + "\", \"" + DisplayedAlbumArtistName + "\", \"" + AlbumTagName + "\", \"" + ArtworkName + "\", \""
                + MbApiInterface.Setting_GetFieldName(MetaDataType.TrackTitle) + "\" and \""
                + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Duration)
                + "' to export to \"HTML Document (CD Booklet)\"!";
            MsgFirstThreeGroupingFieldsInPreviewTableShouldBe2 = "First three grouping fields in preview table should be \"" + AlbumTagName + "\", \"" + DisplayedAlbumArtistName + "\" and \"" + ArtworkName + "\" to export " +
                "to \"HTML Document (Album List)\"!";
            MsgResizingArtworksRequired = "Artwork resizing (more than 60px) must be turned on to use this export format!";

            MsgYouMustImportStandardAsrPresetsFirst = "You must import standard ASR presets first!";

            SbSorting = "Sorting table...";
            SbUpdating = "updating";
            SbReading = "reading";
            SbUpdated = "updated";
            SbRead = "read";
            SbFiles = "file(s)";
            SbItems = "record(s)";
            SbItemNames = SbFiles;

            SbAsrPresetIsApplied = "ASR preset is applied: %%PRESET-NAME%%";
            SbAsrPresetsAreApplied = "ASR presets are applied: %%PRESET-COUNT%%";

            SbLrHotkeysAreAssignedIncorrectly = "LR hotkey is assigned incorrectly!";
            SbIncorrectLrFunctionId = "Incorrect LR function ID: ";
            SbLrEmptyTrackListToBeApplied = "Empty track list passed to LR preset execution!";
            SbLrNot1TrackPassedToLrFunctionId = "The number of tracks passed to $LR() function is not 1!";
            SbLrSenselessToSaveSpitGroupingsTo1File = "It's senseless to save spit groupings to 1 file!";

            SbBackupRestoreCantDeleteBackupFile = "ERROR: Can't delete backup file!";

            CtlDirtyError1sf = string.Empty;
            CtlDirtyError1mf = string.Empty;
            CtlDirtyError2sf = " background file updating operation is running/scheduled. \n" +
                "Preview results may be not accurate. ";
            CtlDirtyError2mf = " background file updating operations is running/scheduled. \n" +
                "Preview results may be not accurate. ";

            MsgSelectTracks = "Select a tracks!";

            MsgBrMasterBackupIndexIsCorrupted = "Master tag backup index is corrupted! All existing at the moment backups are not available any more in \"Tag history\" command.";
            MsgBrBackupIsCorrupted = "Backup \"%%FILENAME%%\" is corrupted or is not valid MusicBee backup!";
            MsgBrFolderDoesntExists = "Folder doesn't exist!";
            MsgCtSelectAtLeast2Tracks = "Select at least 2 tracks to compare!";
            MsgBrBackupBaselineFileDoesntExist = "Backup baseline file\"%%FILENAME%%\" doesn't exist! Backup tags manually first!";
            MsgBrThisIsTheBackupOfDifferentLibrary = "This is the backup of different library! Do you still want to try to restore the tags from this backup?";
            MsgBrCreateBaselineWarning = "When you create the first backup of given library, a backup baseline is created. " +
                "All further backups are incremental relative to baseline. " +
                "If you have changed very much tags incremental backups may become too large. This command will delete ALL incremental backups " +
                "of CURRENT library and will create new backup baseline if you continue. ";
            MsgBrCreateNewBaselineBackupOrRestoreTagsFromAnotherLibraryBeforeMusicBeeRestart = "You must create new baseline backup or restore tags from ANOTHER library " +
                "BEFORE MUSICBEE RESTART for the change of this option to take effect!";
            MsgBrCreateNewBaseline = "It is recommended to create a new baseline after putting the tags in order!";

            MsgMsrGiveNameToAsrPreset = "Give a name to preset!";
            MsgMsrAreYouSureYouWantToSaveAsrPreset = "Do you want to save ASR preset named \"%%PRESET-NAME%%\"?";
            MsgMsrAreYouSureYouWantToOverwriteAsrPreset = "Do you want to overwrite ASR preset \"%%PRESET-NAME%%\"?";
            MsgMsrAreYouSureYouWantToOverwriteRenameAsrPreset = "Do you want to overwrite ASR preset \"%%PRESET-NAME%%\", and rename it to \"%%NEWPRESETNAME%%\"?";
            MsgMsrAreYouSureYouWantToDeleteAsrPreset = "Do you want to delete ASR preset \"%%PRESET-NAME%%\"?";
            MsgAsrPredefinedPresetsCantBeChanged = "Predefined presets can't be changed. Preset editor will open in read-only mode.\n\n"
                + "Do you want to disable this warning?";
            MsgMsrSavePreset = "Save Preset";
            MsgMsrDeletePreset = "Delete Preset";

            MsgUnsupportedMusicBeeFontType = "Unsupported MusicBee font type!\n" +
                "Either choose different font in MusicBee preferences or disable using MusicBee font in plugin settings.";

            CtlNewAsrPreset = "<New ASR Preset>";
            CtlAsrSyntaxError = "SYNTAX ERROR!";

            CtlAsrPresetEditorPleaseWait = " PLEASE WAIT! ";

            CtlLrPresetAutoName = "(Auto preset name)";
            CtlLrInvalidPresetFormatAutoName = "Invalid preset format!";
            CtlLrError = "ERROR!";
            CtlLrParsingError = "#PARSING ERROR!";
            CtlLrInvalidValue = "#INVALID VALUE!";
            CtlLrRefreshUi = "Refresh UI!";
            CtlLrIncorrectReportPresetId = "Incorrect $LR() ID!";

            CtlCopyTagFilter = "Text files|*.txt";

            ExProhibitedParameterCombination = "Prohibited parameter combination: interactive = %%interactive%%, filterResults = %%filterResults%%";
            ExIncorrectLrHotkeySlot = "Incorrect LR hotkey slot: %%SLOT%%!";
            ExThisFieldAlreadyDefinedInPreset = "This field already exists in preset!";


            ExInvalidDropdownStyle = "Invalid dropdown style!";
            ExImpossibleToStretchThumb = "Impossible to stretch thumb. Middle thumb image is null.";
            ExMaskAndImageMustBeTheSameSize = "Mask and image must be the same size!";


            CtlAutoRateCalculating = "Calculating...";

            CtlAsrCellTooTip = "Unchecked rows won't be processed\n" +
                "\n" +
                "Shift-click checks/unchecks all rows containing the same tags as in this row";
            CtlAsrAllTagsCellTooTip = "Unchecked rows won't be processed\n" +
                "\n" +
                "Shift-click checks/unchecks all rows containing the same tag\n" +
                "\n" +
                "Ctrl-click adds/removes the tag referred in the row to/from proceeded or preserved tags";
            MsgAsrPresetsUsingAllTagsPseudoTagNameCannotBeAutoApplied = "Can't execute preset %%PRESET-NAME%%! ASR presets using %%AllTagsPseudoTagName%% cannot " +
                "be auto-executed, nor can be applied via hotkey!";


            SbAutoBackingUp = "Autosaving tag backup...";
            SbMovingBackupsToNewFolder = "Moving backups to new folder...";
            SbMakingTagBackup = "Making tag backup...";
            SbRestoringTagsFromBackup = "Restoring tags from backup...";
            SbRenamingMovingBackup = "Renaming/moving backup...";
            SbMovingBackups = "Moving backups...";
            SbDeletingBackups = "Deleting backup(s)...";
            SbTagAutoBackupSkipped = "Tag auto-backup skipped (no changes since last tag backup)";
            SbComparingTags = "Comparing tags with baseline... ";

            CtlWarning = "WARNING!";
            CtlMusicBeeBackupType = "MusicBee Tag Backup|*.xml";
            CtlSaveBackupTitle = "NAVIGATE TO DESIRED FOLDER, TYPE BACKUP NAME AT THE BOTTOM OF THE WINDOW AND CLICK \"SAVE\"";
            CtlRestoreBackupTitle = "NAVIGATE TO DESIRED FOLDER, SELECT BACKUP AND CLICK \"OPEN\"";
            CtlRenameSelectBackupTitle = "NAVIGATE TO DESIRED FOLDER, SELECT BACKUP TO BE RENAMED/MOVED AND CLICK \"OPEN\"";
            CtlRenameSaveBackupTitle = "NAVIGATE TO DESIRED FOLDER, TYPE NEW BACKUP NAME AT THE BOTTOM OF THE WINDOW AND CLICK \"SAVE\"";
            CtlMoveSelectBackupsTitle = "NAVIGATE TO DESIRED FOLDER, SELECT BACKUPS TO BE MOVED AND CLICK \"OPEN\"";
            CtlMoveSaveBackupsTitle = "NAVIGATE TO DESTINATION FOLDER AND CLICK \"SAVE\"";
            CtlDeleteBackupsTitle = "NAVIGATE TO DESIRED FOLDER, SELECT BACKUP(S) TO DELETE AND CLICK \"OPEN\"";
            CtlSelectThisFolder = "[SELECT THIS FOLDER]";
            CtlNoBackupData = "<No backup data>";
            CtlNoDifferences = "<No differences>";
            CtlNoPreview = "<No preview>";
            CtlMixedValues = "(Mixed values)";
            CtlMixedValuesSameAsInLibrary = "Same as in library";
            CtlMixedValuesDifferentFromLibrary = "Different from library";

            CtlTags = "Tags";

            MsgNotAllowedSymbols = "Only ASCII letters, numbers and symbols - : _ . are allowed!";
            MsgPresetExists = "Preset with ID %%ID%% already exists!";

            CtlWholeCuesheetWillBeReencoded = "IMPORTANT NOTE: ONE OR SEVERAL TRACKS BELONG TO CUESHEET. THE ALL CUESHEET TRACKS WILL BE REENCODED!";

            CtlMSR = "MSR: ";

            CtlUnknown = MbApiInterface.MB_GetLocalisation("dSum.msg.Unknown", "Unknown");

            CtlMixedFilters = "(Mixed filters)";

            MsgFirstSelectWhichFieldYouWantToAssignFunctionIdTo = "First select, which field you want to assign function ID to (leftmost combo-box on function ID line)!";

            //Defaults for controls
            SavedSettings = new PluginSettings
            {

                //Let's set initial defaults
                reportPresets = null,

                smartOperation = true,
                appendSource = false,

                changeCaseFlag = 1,
                useExceptionWords = false,

                useExceptionChars = false,
                useSentenceSeparators = false,
            };
            #endregion

            #region Reading settings
            //Let's try to read defaults for controls from settings file
            PluginSettingsFilePath = Path.Combine(MbApiInterface.Setting_GetPersistentStoragePath(), PluginSettingsFileName);

            var unicode = Encoding.UTF8;

            if (File.Exists(PluginSettingsFilePath + ".new-format")) //********* Simplify later!!!!!!
            {
                var stream = File.Open(PluginSettingsFilePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
                var file = new StreamReader(stream, unicode);

                XmlSerializer controlsDefaultsSerializer = null;
                try
                {
                    controlsDefaultsSerializer = new XmlSerializer(typeof(PluginSettings));
                }
                catch (Exception e)
                {
                    MessageBox.Show(MbForm, e.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                try
                {
                    SavedSettings = (PluginSettings)controlsDefaultsSerializer.Deserialize(file);
                }
                catch
                {
                    //Ignore...
                }

                file.Close();
            }
            else //******* remove later!!!!
            {
                var stream = File.Open(PluginSettingsFilePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
                var file = new StreamReader(stream, unicode);

                XmlSerializer controlsDefaultsSerializer = null;
                try
                {
                    controlsDefaultsSerializer = new XmlSerializer(typeof(SavedSettingsType));
                }
                catch (Exception e)
                {
                    MessageBox.Show(MbForm, e.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                try
                {
                    var oldSavedSettings = (SavedSettingsType)controlsDefaultsSerializer.Deserialize(file);
                    oldSavedSettings.CopyPropsTo(ref SavedSettings);
                }
                catch
                {
                    //Ignore...
                }

                file.Close();
            }
            #endregion

            #region Resetting invalid/absent settings
            if (SavedSettings == null)
                SavedSettings = new PluginSettings();

            DontShowShowHiddenWindows = SavedSettings.dontShowShowHiddenWindows;

            if (SavedSettings.windowsSettings == null)
                SavedSettings.windowsSettings = new List<WindowSettingsType>();

            if (SavedSettings.exceptedWords == null || SavedSettings.exceptedWords.Length < 10)
            {
                SavedSettings.exceptedWords = new object[10];
                SavedSettings.exceptedWords[0] = "the a an and or not";
                SavedSettings.exceptedWords[1] = "a al an and as at but by de for in la le mix nor of on or remix the to vs. y ze feat.";
                SavedSettings.exceptedWords[2] = "U2 UB40";
                SavedSettings.exceptedWords[3] = string.Empty;
                SavedSettings.exceptedWords[4] = string.Empty;
                SavedSettings.exceptedWords[5] = string.Empty;
                SavedSettings.exceptedWords[6] = string.Empty;
                SavedSettings.exceptedWords[7] = string.Empty;
                SavedSettings.exceptedWords[8] = string.Empty;
                SavedSettings.exceptedWords[9] = string.Empty;
            }

            if (SavedSettings.exceptionWordsAsr == null)
                SavedSettings.exceptionWordsAsr = "a al an and as at but by de for in la le mix nor of on or remix the to vs. y ze feat.";

            if (SavedSettings.exceptionChars == null || SavedSettings.exceptionChars.Length < 10)
            {
                SavedSettings.exceptionChars = new object[10];
                SavedSettings.exceptionChars[0] = "\" ' ( [ { /";
                SavedSettings.exceptionChars[1] = string.Empty;
                SavedSettings.exceptionChars[2] = string.Empty;
                SavedSettings.exceptionChars[3] = string.Empty;
                SavedSettings.exceptionChars[4] = string.Empty;
                SavedSettings.exceptionChars[5] = string.Empty;
                SavedSettings.exceptionChars[6] = string.Empty;
                SavedSettings.exceptionChars[7] = string.Empty;
                SavedSettings.exceptionChars[8] = string.Empty;
                SavedSettings.exceptionChars[9] = string.Empty;
            }

            if (SavedSettings.openingExceptionChars == null || SavedSettings.closingExceptionChars == null
                || SavedSettings.openingExceptionChars.Length < 10 || SavedSettings.closingExceptionChars.Length < 10)
            {
                SavedSettings.openingExceptionChars = new object[10];
                SavedSettings.openingExceptionChars[0] = "( [ {";
                SavedSettings.openingExceptionChars[1] = string.Empty;
                SavedSettings.openingExceptionChars[2] = string.Empty;
                SavedSettings.openingExceptionChars[3] = string.Empty;
                SavedSettings.openingExceptionChars[4] = string.Empty;
                SavedSettings.openingExceptionChars[5] = string.Empty;
                SavedSettings.openingExceptionChars[6] = string.Empty;
                SavedSettings.openingExceptionChars[7] = string.Empty;
                SavedSettings.openingExceptionChars[8] = string.Empty;
                SavedSettings.openingExceptionChars[9] = string.Empty;

                SavedSettings.closingExceptionChars = new object[10];
                SavedSettings.closingExceptionChars[0] = ") ] }";
                SavedSettings.closingExceptionChars[1] = string.Empty;
                SavedSettings.closingExceptionChars[2] = string.Empty;
                SavedSettings.closingExceptionChars[3] = string.Empty;
                SavedSettings.closingExceptionChars[4] = string.Empty;
                SavedSettings.closingExceptionChars[5] = string.Empty;
                SavedSettings.closingExceptionChars[6] = string.Empty;
                SavedSettings.closingExceptionChars[7] = string.Empty;
                SavedSettings.closingExceptionChars[8] = string.Empty;
                SavedSettings.closingExceptionChars[9] = string.Empty;
            }

            if (SavedSettings.exceptionCharsAsr == null)
                SavedSettings.exceptionCharsAsr = string.Empty;

            if (SavedSettings.sentenceSeparators == null || SavedSettings.sentenceSeparators.Length < 10)
            {
                SavedSettings.sentenceSeparators = new object[10];
                SavedSettings.sentenceSeparators[0] = ".";
                SavedSettings.sentenceSeparators[1] = string.Empty;
                SavedSettings.sentenceSeparators[2] = string.Empty;
                SavedSettings.sentenceSeparators[3] = string.Empty;
                SavedSettings.sentenceSeparators[4] = string.Empty;
                SavedSettings.sentenceSeparators[5] = string.Empty;
                SavedSettings.sentenceSeparators[6] = string.Empty;
                SavedSettings.sentenceSeparators[7] = string.Empty;
                SavedSettings.sentenceSeparators[8] = string.Empty;
                SavedSettings.sentenceSeparators[9] = string.Empty;
            }

            if (SavedSettings.sentenceSeparatorsAsr == null)
                SavedSettings.sentenceSeparatorsAsr = ".";

            if (SavedSettings.customText == null || SavedSettings.customText.Length < 10)
            {
                SavedSettings.customText = new object[10];
                SavedSettings.customText[0] = string.Empty;
                SavedSettings.customText[1] = string.Empty;
                SavedSettings.customText[2] = string.Empty;
                SavedSettings.customText[3] = string.Empty;
                SavedSettings.customText[4] = string.Empty;
                SavedSettings.customText[5] = string.Empty;
                SavedSettings.customText[6] = string.Empty;
                SavedSettings.customText[7] = string.Empty;
                SavedSettings.customText[8] = string.Empty;
                SavedSettings.customText[9] = string.Empty;
            }

            if (SavedSettings.appendedText == null || SavedSettings.appendedText.Length < 10)
            {
                SavedSettings.appendedText = new object[10];
                SavedSettings.appendedText[0] = string.Empty;
                SavedSettings.appendedText[1] = string.Empty;
                SavedSettings.appendedText[2] = string.Empty;
                SavedSettings.appendedText[3] = string.Empty;
                SavedSettings.appendedText[4] = string.Empty;
                SavedSettings.appendedText[5] = string.Empty;
                SavedSettings.appendedText[6] = string.Empty;
                SavedSettings.appendedText[7] = string.Empty;
                SavedSettings.appendedText[8] = string.Empty;
                SavedSettings.appendedText[9] = string.Empty;
            }

            if (SavedSettings.addedText == null || SavedSettings.addedText.Length < 10)
            {
                SavedSettings.addedText = new object[10];
                SavedSettings.addedText[0] = string.Empty;
                SavedSettings.addedText[1] = string.Empty;
                SavedSettings.addedText[2] = string.Empty;
                SavedSettings.addedText[3] = string.Empty;
                SavedSettings.addedText[4] = string.Empty;
                SavedSettings.addedText[5] = string.Empty;
                SavedSettings.addedText[6] = string.Empty;
                SavedSettings.addedText[7] = string.Empty;
                SavedSettings.addedText[8] = string.Empty;
                SavedSettings.addedText[9] = string.Empty;
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

            if (SavedSettings.autoBackupDirectory == null)
                SavedSettings.autoBackupDirectory = "Tag Backups";
            if (!Directory.Exists(BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory)))
                Directory.CreateDirectory(BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory));

            if (SavedSettings.defaultTagHistoryNumberOfBackups < 1)
                SavedSettings.defaultTagHistoryNumberOfBackups = 10;

            if (SavedSettings.defaultAsrPresetsExportFolder == null)
                SavedSettings.defaultAsrPresetsExportFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
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
                //Let's redefine localizable strings

                //MusicBeePlugin localizable strings
                PluginName = "Дополнительные инструменты";
                PluginMenuGroupName = "Дополнительные инструменты";
                PluginDescription = "Плагин добавляет дополнительные инструменты для работы с тегами";

                PluginHelpString = "Справка...";
                PluginWebPageString = "Веб-страница плагина..."; ;
                PluginWebPageToolTip = "Открыть веб-страницу плагина (для проверки/загрузки последней версии)";
                PluginVersionString = "Версия: ";

                OpenWindowsMenuSectionName = "ОТКРЫТЫЕ ОКНА";
                TagToolsMenuSectionName = "ДОПОЛНИТЕЛЬНЫЕ ИНСТРУМЕНТЫ";
                BackupRestoreMenuSectionName = "АРХИВАЦИЯ И ВОССТАНОВЛЕНИЕ";

                CopyTagName = "Копировать тег...";
                SwapTagsName = "Поменять местами два тега...";
                ChangeCaseName = "Изменить регистр тега...";
                ReEncodeTagName = "Изменить кодировку тега...";
                ReEncodeTagsName = "Изменить кодировку тегов...";
                LibraryReportsName = "Отчеты по библиотеке...";
                AutoRateName = "Автоматически установить рейтинги...";
                AsrName = "Дополнительный поиск и замена...";
                CarName = "Рассчитать средний рейтинг альбомов...";
                CompareTracksName = "Сравнить треки...";
                CopyTagsToClipboardName = "Копировать теги в буфер обмена...";
                PasteTagsFromClipboardName = "Вставить теги из буфера обмена";
                MsrName = "Множественный поиск и замена...";
                ShowHiddenWindowsName = "Показать скрытые/восстановить свернутые окна плагина";

                TagToolsHotkeyDescription = "Дополнительные инструменты: ";
                CopyTagDescription = TagToolsHotkeyDescription + "Копировать тег";
                SwapTagsDescription = TagToolsHotkeyDescription + "Поменять местами два тега";
                ChangeCaseDescription = TagToolsHotkeyDescription + "Изменить регистр тега";
                ReEncodeTagDescription = TagToolsHotkeyDescription + "Изменить кодировку тега";
                ReEncodeTagsDescription = TagToolsHotkeyDescription + "Изменить кодировку тегов";
                LibraryReportsDescription = TagToolsHotkeyDescription + "Отчеты по библиотеке";
                LrPresetHotkeyDescription = TagToolsHotkeyDescription;
                AutoRateDescription = TagToolsHotkeyDescription + "Автоматически установить рейтинги";
                AsrDescription = TagToolsHotkeyDescription + "Дополнительный поиск и замена";
                AsrPresetHotkeyDescription = TagToolsHotkeyDescription;
                CarDescription = TagToolsHotkeyDescription + "Рассчитать средний рейтинг альбомов";
                CompareTracksDescription = TagToolsHotkeyDescription + "Сравнить треки";
                CopyTagsToClipboardDescription = TagToolsHotkeyDescription + "Копировать теги в буфер обмена";
                PasteTagsFromClipboardDescription = TagToolsHotkeyDescription + "Вставить теги из буфера обмена";
                CopyTagsToClipboardUsingMenuDescription = "Копировать теги в буфер обмена, используя набор";
                MsrCommandDescription = TagToolsHotkeyDescription + "Множественный поиск и замена";
                ShowHiddenWindowsDescription = TagToolsHotkeyDescription + "Показать скрытые окна плагина";

                BackupTagsName = "Архивировать теги для всех треков...";
                RestoreTagsName = "Восстановить теги для выбранных треков...";
                RestoreTagsForEntireLibraryName = "Восстановить теги для всех треков...";
                RenameMoveBackupName = "Переименовать или переместить архив...";
                MoveBackupsName = "Переместить архив(ы)...";
                CreateNewBaselineName = "Создать новый опорный архив текущей библиотеки...";
                createEmptyBaselineRestoreTagsForEntireLibraryName = "Создать пустой опорный архив и восстановить теги из другой библиотеки...";
                DeleteBackupsName = "Удалить архив(ы)...";
                AutoBackupSettingsName = "Настройки (авто)архивации...";

                TagHistoryName = "История тегов...";

                BackupTagsDescription = TagToolsHotkeyDescription + "Архивировать теги для всех треков";
                RestoreTagsDescription = TagToolsHotkeyDescription + "Восстановить теги для выбранных треков";
                RestoreTagsForEntireLibraryDescription = TagToolsHotkeyDescription + "Восстановить теги для всех треков";
                RenameMoveBackupDescription = TagToolsHotkeyDescription + "Переименовать или переместить архив";
                MoveBackupsDescription = TagToolsHotkeyDescription + "Переместить архив(ы)";
                CreateNewBaselineDescription = TagToolsHotkeyDescription + "Создать новый опорный архив текущей библиотеки";
                createEmptyBaselineRestoreTagsForEntireLibraryDescription = TagToolsHotkeyDescription + "Создать пустой опорный архив и восстановить теги из другой библиотеки";
                DeleteBackupsDescription = TagToolsHotkeyDescription + "Удалить архив(ы)";
                AutoBackupSettingsDescription = TagToolsHotkeyDescription + "Настройки (авто)архивации";

                TagHistoryDescription = TagToolsHotkeyDescription + "История тегов";

                CopyTagSbText = "Копирование тегов";
                SwapTagsSbText = "Обмен тегов местами";
                ChangeCaseSbText = "Изменение регистра тега";
                CompareTracksSbText = "Сравнение треков";
                ReEncodeTagSbText = "Изменение кодировки тегов";
                LibraryReportsSbText = "Формирование отчета";
                LibraryReportsGeneratingPreviewSbText = "Формирование предварительного просмотра";
                ApplyingLrPresetSbText = "Применение пресета ОБ";
                AutoRateSbText = "Автоматическая установка рейтингов";
                AsrSbText = "Дополнительный поиск и замена";
                CarSbText = "Расчет среднего рейтинга альбомов";
                MsrSbText = "Множественный поиск и замена";
                TagHistorySbText = "История тегов";

                AnotherLrPresetIsRunningSbText = "Другой отчет ОБ уже запущен. Невозможно применить отчет!";
                CtlAnotherLrPresetIsRunning = "Другой отчет ОБ уже запущен. Подождите завершения его работы...";
                CtlWaitUntilPresetIsCompleted = "\r\n\r\nИдет остановка применения пресетов, но применение текущего пресета должно быть завершено. Пожалуйста, подождите...";
                CtlLrElapsed = ", прошло: ";

                LrFunctionNames[0] = "<Группировка>";
                LrFunctionNames[1] = "Количество";
                LrFunctionNames[2] = "Сумма";
                LrFunctionNames[3] = "Минимум";
                LrFunctionNames[4] = "Максимум";
                LrFunctionNames[5] = "Среднее значение";
                LrFunctionNames[6] = "Среднее количество";

                LibraryReport = "Отчет по библиотеке";


                //Other localizable strings
                DateCreatedTagName = "<Создано>";
                EmptyValueTagName = "<Пустое значение>";
                ClipboardTagName = "<Буфер обмена>";
                TextFileTagName = "<Текстовый файл>";

                FolderTagName = "<Папка>";
                FileNameTagName = "<Имя файла>";
                FilePathTagName = "<Путь к файлу>";
                FilePathWoExtTagName = "<Полный путь без расш.>";
                AlbumUniqueIdName = "<Уникальный Id альбома>";

                ParameterTagName = "Тег";
                //tempTagName = "Врем.";

                LibraryTotalsPresetName = "Итоги по библиотеке";
                LibraryAveragesPresetName = "В среднем по библиотеке";
                CDBookletPresetName = "Буклет компакт-диска";
                AlbumsAndTracksPresetName = "Альбомы и треки";
                AlbumGridPresetName = "Сетка альбомов (список альбомов)";

                EmptyPresetName = "<Пустой пресет>";

                AllTagsPseudoTagName = "<ВСЕ ТЕГИ>";

                GenericTagSetName = "Набор тегов";


                SequenceNumberName = "№ п/п";


                //Supported exported file formats
                ExportedFormats = "Документ HTML (по альбомам)|*.htm|Документ HTML|*.htm|Простая таблица HTML|*.htm|Текст, разделенный табуляциями|*.txt|Плейлист M3U|*.m3u|" +
                    "Файл CSV|*.csv|Документ HTML (буклет компакт-диска)|*.htm|Документ HTML (сетка альбомов)|*.htm";
                ExportedTrackList = "Список треков ОБ";

                //Displayed text
                //ListItemConditionIs = "равен";
                //ListItemConditionIsNot = "не равен";
                //ListItemConditionIsGreater = "больше";
                //ListItemConditionIsGreaterOrEqual = "больше или равен";
                //ListItemConditionIsLess = "меньше";
                //ListItemConditionIsLessOrEqual = "меньше или равен";

                SelectTagsWindowTitle = "Выберите теги";
                SelectDisplayedTagsWindowTitle = "Выберите отображаемые теги";
                SelectButtonName = "Выбрать";

                AsrProcessTagsButtonName = "Обрабатывать теги";
                AsrPreserveTagsButtonName = "Не изменять теги";

                LrButtonFilterResultsShowAllText = "Показать все";
                LrButtonFilterResultsShowAllToolTip = "Показать все результаты";

                LrCellToolTip = "Щелчок с нажатой клавишей Ctrl копирует содержимое ячейки в поле сравнения условного экспорта/записи. \n" +
                    "Двойной щелчок по границе строки устанавливает авто-размер строки. \n" +
                    "Двойной щелчок с нажатой клавишей Shift по границе строки устанавливает авто-размер всех строк. \n" +
                    "Изменение размера строки с нажатой клавишей Alt изменит размер всех строк. ";

                SbBrokenPresetRetrievalChain = "Нарушенная цепочка пресетов для получения списка треков для для пресета ОБ: %%PRESET-NAME%%! Проверьте цепочку пресетов!";

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

                DefaultAsrPresetName = "Пресет №";

                Msg1stTimeUsage = "Вы впервые используете плагин \"Дополнительные инструменты\". Пожалуйста, прочитайте некоторые замечания, прежде чем продолжить. " +
                    "Это сообщение больше не появится.\r\n\r\nРасширенные функции плагина очень сложны и могут запутать, хотя базовые довольно просты в использовании.\r\n\r\n" +
                    "Но вы можете отключить практически любую часть функциональности плагина (конечно, вы можете включить любую часть позже). " +
                    "Отключение любого пункта меню плагина в настройках скрывает не только сам пункт меню, но и отключает весь соответствующий функционал плагина. " +
                    "Например, при отключении пункта \"Показывать в меню команду 'Дополнительный поиск и замена' / 'Множественный поиск и замена'\" " +
                    "будут прекращены инициализация ДПЗ и автоматическое выполнение пресетов ДПЗ.\r\n\r\n" +
                    "Чтобы открыть настройки плагина вручную, перейдите в меню MusicBee> Правка> Настройки> Плагины> Дополнительные инструменты> Настройка.\r\n\r\n" +
                    "Хотите открыть настройки плагина сейчас, чтобы настроить его функциональность?";

                MsgFileNotFound = "Файл не найден!";
                MsgNoTracksSelected = "Треки не выбраны.";
                MsgNoTracksDisplayed = "Нет отображаемых треков.";
                MsgSourceAndDestinationTagsAreTheSame = "Оба тега одинаковые. Обработка не выполнена.";
                MsgSwapTagsSourceAndDestinationTagsAreTheSame = "Использовать один и тот же " +
                    "тег-источник и тег-получатель имеет смысл только для тегов \"Исполнитель\"/\"Композитор\" для преобразования строки, разделенной символами \";\", в список исполнителей/композиторов и " +
                    "наоборот. Обработка не выполнена.";
                MsgNoTagsSelected = "Теги не выбраны. Обработка не выполнена.";
                MsgNoTracksInCurrentView = "Нет треков в текущем режиме отображения.";
                MsgTrackListIsEmpty = "Список треков пуст. Сначала нажмите кнопку \"Просмотр\".";
                MsgPreviewIsNotGeneratedNothingToSave = "Таблица не сгенерирована. Нечего сохранять.";
                MsgPreviewIsNotGeneratedNothingToChange = "Таблица не сгенерирована. Нечего изменять.";
                MsgPleaseUseGroupingFunction = "Пожалуйста, используйте функцию <Группировка> для тегов \"" + ArtworkName + "\" и \"" + SequenceNumberName + "\"!";
                CtlAllTags = "ВСЕ ТЕГИ";
                MsgNoUrlColumnUnableToSave = "В таблице нет тега \"" + UrlTagName + "\". Невозможно сохранить результаты.";
                MsgEmptyURL = "Пустой тег \"" + UrlTagName + "\" в строке ";
                MsgUnableToSave = "Невозможно сохранить результаты. ";
                MsgUsingEmptyValueAsSourceTagMayBeUsefulOnlyForAppendingStaticTextToDestinationTag = "Использование псевдо-тега '" + EmptyValueTagName +
                    "\" в качестве тега-источника имеет смысл только при включенной опции \"Добавить тег источника в конец тега получателя' для добавления какого-то " +
                    "текста  к тегу-получателю. Обработка не выполнена.";
                MsgBackgroundTaskIsCompleted = "Фоновая задача плагина \"Дополнительные инструменты\" завершена.";
                MsgThresholdsDescription = "Автоматическая установка рейтингов основана на среднем числе воспроизведений композиции в день \n" +
                    "(на виртуальном теге \"число воспроизведений в день\"). Вам следует установить пороговые значения \"числа воспроизведений в день\" для каждого значения рейтинга, \n" +
                    "которое может быть назначено композициям. Пороговые значения оцениваются в убывающем порядке: от рейтинга 5 звездочек \n" +
                    "до рейтинга 0.5 звездочки. Тег \"число воспроизведений в день\" может быть рассчитан для каждой композиции независимо от остальной библиотеки, \n" +
                    "так что возможно автоматическое обновление рейтинга при смене композиции. Также возможно рассчитать рейтинги для выбранных композиций \n" +
                    "командой \"Установить рейтинги\" или для всех композиций библиотеки при запуске MusicBee. ";
                MsgAutoCalculationOfThresholdsDescription = "Автоматический расчет пороговых значений \"числа воспроизведений в день\" - это еще один способ установки авто-рейтингов. \n" +
                    "Эта опция позволяет вам установить желаемый процент каждого значения рейтинга в библиотеке. Действительные проценты значений рейтингов могут отличаться \n" +
                    "от желаемых, поскольку не всегда можно разбить композиции библиотеки на несколько групп (предположим все композиции библиотеки имеют одинаковое \n" +
                    "значение \"числа воспроизведений в день\", очевидно не существует способа разбить все композиции на группы, исходя из значений \"числа воспроизведений в день\". \n" +
                    "Действительные проценты отображаются справа от желаемых процентов после вычисления пороговых значений. Поскольку вычисление пороговых значений требует заметного \n" +
                    "времени, то оно не может производиться автоматически, кроме как при запуске MusicBee. ";

                MsgNumberOfPlayedTracks = "Число когда либо воспроизводившихся композиций: ";
                MsgIncorrectSumOfWeights = "Сумма всех процентов должна быть равна 100% или меньше!";
                MsgSum = "Сумма: ";
                MsgNumberOfNotRatedTracks = "% композиций с нулевым рейтингом)";
                MsgTracks = " композиций)";
                MsgActualPercent = "% / Действ.: ";
                MsgIncorrectPresetName = "Некорректное название пресета или пресет с таким названием уже существует.";

                MsgCsTheNumberOfOpeningExceptionCharactersMustBe = "Число открывающих символов исключения должно быть таким же как и число закрывающих символов!";

                MsgLrDoYouWantToSaveChangesBeforeClosingTheWindow = "Есть несохраненные изменения. Сохранить изменения, прежде чем закрыть окно?";
                MsgLrDoYouWantToCloseTheWindowWithoutSavingChanges = "Сейчас вы не можете сохранить пресеты. Отменить все изменения при закрытии окна \"Отчетов по библиотеке\"?";

                MsgAsrDoYouWantToSaveChangesBeforeClosingTheWindow = "Один или несколько пресетов были настроены или изменены. Сохранить изменения, прежде чем закрыть окно?";

                MsgDeletePresetConfirmation = "Удалить выбранный пресет?";
                MsgInstallingConfirmation = "Установить стандартные пресеты?";
                MsgDoYouWantToImportExistingPresetsAsCopies = "Некоторые импортируемые пресеты уже существуют.\n"
                    + "Импортировать их как новые пресеты, сохранив существующие?\n\n"
                    + "В ПРОТИВНОМ СЛУЧАЕ СУЩЕСТВУЮЩИЕ ПРЕСЕТЫ БУДУТ ПЕРЕЗАПИСАНЫ!\n";
                MsgDoYouWantToResetYourCustomizedPredefinedPresets = "Вы настроили некоторые стандартные пресеты.\n"
                    + "Вы хотите переустановить настроенные стандартные пресеты, заменив их последними версиями от разработчика?\n\n"
                    + "ВСЕ ВАШИ НАСТРОЙКИ БУДУТ УТЕРЯНЫ!\n\n"
                    + "В противном случае настроенные пресеты будут обновлены до последних версий, но все их настройки будут сохранены.";
                MsgDoYouWantToRemovePredefinedPresets = "Некоторые стандартные пресеты устарели и были удалены из поставки плагина.\n\n"
                    + "Вы хотите удалить эти пресеты?";


                MsgPresetsWereImported = " пресет{;а;ов} был{;и;и} импортирован{;ы;ы}.\n";
                MsgMsrPresetWasImported = " служебный скрытый пресет был импортирован.\n";
                MsgPresetsWereImportedAsCopies = " пресет{;а;ов} был{;и;и} импортирован{;ы;ы} как новы{й;ые;ые} пресет{;ы;ы}.\n";
                MsgPresetsFailedToImport = " пресет{;а;ов} не удалось импортировать из-за ошиб{ки;ок;ок}\n" + AddLeadingSpaces(0, 4, 0)
                    + " чтения файл{а;ов;ов} или {его;их;их} некорректного формата.";

                MsgPresetsWereInstalled = " пресет{;а;ов} был{;и;и} установлен{;ы;ы}.\n\n";
                MsgMsrPresetWasInstalled = " служебный скрытый пресет был установлен.\n";
                MsgPresetsCustomizedWereReinstalled = " пресет{;а;ов}, настроенн{ый;ых;ых} вами, был{;и;и}\n" + AddLeadingSpaces(0, 4, 0)
                    + " переустановлен{;ы;ы}, но {его;их;их} настройки были сохранены.\n\n";
                MsgPresetsWereReinstalled = " пресет{;а;ов} был{;и;и} переустановлен{;ы;ы}.\n\n";
                MsgPresetsWereUpdated = " пресет{;а;ов} был{;и;и} обновлен{;ы;ы}.\n\n";
                MsgPresetsCustomizedWereUpdated = " пресет{;а;ов}, настроенн{ый;ых;ых} вами, был{;и;и} обновлен{;ы;ы},\n" + AddLeadingSpaces(0, 4, 0)
                    + " но {его;их;их} настройки были сохранены.\n\n";
                MsgPresetsWereNotChanged = " пресет{;а;ов} не изменил{ся;ись;ись} с последнего \n" + AddLeadingSpaces(0, 4, 0)
                    + " обновления и был{;и;и} пропущен{;ы;ы}.\n\n";
                MsgPresetsRemoved = " пресет{;а;ов} был{;и;и} удален{;ы;ы}.\n\n";
                MsgPresetsFailedToUpdate = " пресет{;а;ов} не удалось обновить из-за ошиб{ки;ок;ок}\n" + AddLeadingSpaces(0, 4, 0)
                    + " чтения файл{а;ов;ов} или {его;их;их} некорректного формата.";

                MsgPresetsNotFound = "Не найдены пресеты для установки в ожидаемом каталоге!";

                MsgDeletingConfirmation = "Удалить все стандартные пресеты?";
                MsgNoPresetsDeleted = "Пресеты не были удалены.";
                MsgPresetsWereDeleted = " пресет{;а;ов} был{;и;и} удален{;ы;ы}.";


                msgLrCachedPresetsInitialFilling = "Эти функции будут использовать теги в качестве кеша. Если вы измените " +
                    "какие-то теги или добавите в библиотеку новые треки, этот кеш будет динамически обновляться. Но КРАЙНЕ РЕКОМЕНДУЕТСЯ сначала заполнить " +
                    "этот кеш для всех существующих треков/текущих тегов текущей библиотеки, чтобы избежать замедления и зависания интерфейса " +
                    "при обычном использовании MusicBee. Вы хотите применить все необходимые пресеты сейчас автоматически? Это может занять некоторое время.";

                MsgLrCachedPresetsChanged = "Вы создали или изменили %%CHANGED-PRESET-COUNT%% пресет{;а;ов} ОБ, которы{й;е;е} использу{е;ю;ю}т функции ОБ " +
                    "с назначенными идентификаторами И с сохранением результатов в теги. " + msgLrCachedPresetsInitialFilling;

                MsgLrCachedPresetsNotApplied = "Вы создали или изменили один или несколько пресетов ОБ, которые используют функции ОБ " +
                    "с назначенными идентификаторами И с сохранением результатов в теги. " + msgLrCachedPresetsInitialFilling;


                MsgNumberOfTagsInTextFileDoesntCorrespondToNumberOfSelectedTracks = "Количество тегов в текстовом файле (%%TEXT-TAG-FILES-COUNT%%)" +
                    " не соответствует количеству выбранных треков (%%SELECTED-FILES-COUNT%%)!";

                MsgClipboardDoesntContainText = "Буфер обмена не содержит текст!";

                MsgClipboardDoesntContainTags = "Буфер обмена не содержит теги!";

                MsgUnknownTagNameInClipboard = "Неизвестное имя тега в буфере обмена: %%TAG-NAME%%!";

                MsgNumberOfTagsInClipboardDoesntCorrespondToNumberOfSelectedTracks = "Количество тегов треков в буфере обмена (%%FILE-TAGS-LENGTH%%)" +
                    " не соответствует количеству выбранных треков (%%SELECTED-FILES-COUNT%%)!";
                MsgDoYouWantToPasteTagsAnyway = " Вставить теги все равно?";
                MsgWrongNumberOfCopiedTags = "Некорректное количество скопированных в буфер обмена тегов (%%CLIPBOARD-TAGS-COUNT%%)" +
                    " в строке %%CLIPBOARD-LINE%% текста буфера обмена!";
                MsgMatchTracksByPathQuestion = "Буфер обмена содержит псевдо-тег %%MATCH-TAG-NAME%%. Вставлять теги в соответствии с этим тегом" +
                    " (в противном случае треки для тегов будут выбираться по их порядку отображения)?";
                MsgTracksSelectedMatchedNotMatched = "Выбрано треков: %%SELECTED-TRACKS%%\n" +
                    "Тегов треков в буфере обмена: %%CLIPBOARD-TRACKS%%\n" +
                    "Найдено подходящих треков: %%MATCHED-TRACKS%%\n" +
                    "Не найдено подходящих треков: %%NOT-MATCHED-TRACKS%%\n\n" +
                    "Вставить теги?";


                MsgFirstThreeGroupingFieldsInPreviewTableShouldBe = "Первые три поля группировок в таблице должны быть \"" + DisplayedAlbumArtistName + "\", '"
                    + AlbumTagName + "\" и \"" + ArtworkName + "\" для того, чтобы экспортировать теги в формат \"Документ HTML (по альбомам)\"!";
                MsgFirstSixGroupingFieldsInPreviewTableShouldBe = "Первые шесть полей группировок в таблице должны быть \"" + SequenceNumberName
                    + "\", \"" + DisplayedAlbumArtistName + "\", \"" + AlbumTagName + "\", \"" + ArtworkName + "\", \""
                    + MbApiInterface.Setting_GetFieldName(MetaDataType.TrackTitle) + "\" and \""
                    + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Duration)
                    + "\" для того, чтобы экспортировать теги в формат \"Документ HTML (буклет компакт-диска)\"!";
                MsgFirstThreeGroupingFieldsInPreviewTableShouldBe2 = "Первые три поля группировок в таблице должны быть \"" + AlbumTagName + "\", \"" + DisplayedAlbumArtistName + "\" и \"" + ArtworkName
                    + "\" для того, чтобы экспортировать теги в формат \"Документ HTML (список альбомов)\"!";
                MsgResizingArtworksRequired = "Требуется включить изменение размеров обложек (не меньше 60 пикс.) для использования этого формата экспорта!";

                MsgYouMustImportStandardAsrPresetsFirst = "Сначала надо импортировать стандартные пресеты дополнительного поиска и замены!";

                SbSorting = "Сортировка таблицы...";
                SbUpdating = "запись";
                SbReading = "чтение";
                SbUpdated = "записан(о)";
                SbRead = "прочитан(о)";
                SbFiles = "файл(ов)";
                SbItems = "записей";
                SbItemNames = SbFiles;

                SbAsrPresetIsApplied = "Применен пресет дополнительного поиска и замены: %%PRESET-NAME%%";
                SbAsrPresetsAreApplied = "Применены пресеты дополнительного поиска и замены: %%PRESET-COUNT%%";

                SbLrHotkeysAreAssignedIncorrectly = "Комбинация клавиш пресета ОБ назначена некорректно!";
                SbIncorrectLrFunctionId = "Некорректный ID функции ОБ: ";
                SbLrEmptyTrackListToBeApplied = "Пустой список треков передан пресету ОБ для применения!";
                SbLrNot1TrackPassedToLrFunctionId = "Число треков, переданных функции $LR() не равно 1!";
                SbLrSenselessToSaveSpitGroupingsTo1File = "Не имеет смысла записывать разделенные группировки в 1 файл!";

                SbBackupRestoreCantDeleteBackupFile = "ОШИБКА: Невозможно удалить файл архива!";

                CtlDirtyError1sf = "Работает/запланирована ";
                CtlDirtyError1mf = "Работают/запланированы ";
                CtlDirtyError2sf = " фоновая операция обновления файлов. \n" +
                    "Результаты предварительного просмотра могут быть не точны. ";
                CtlDirtyError2mf = " фоновых операций обновления файлов. \n" +
                    "Результаты предварительного просмотра могут быть не точны. ";

                MsgSelectTracks = "Выберите треки!";

                MsgBrMasterBackupIndexIsCorrupted = "Основной индекс архива тегов поврежден! Все существующие на данный момент архивы " +
                    "будут не доступны в команде \"История тегов\".";
                MsgBrBackupIsCorrupted = "Архив \"%%FILENAME%%\" или поврежден, или не является архивом MusicBee!";
                MsgBrFolderDoesntExists = "Папка не существует!";
                MsgCtSelectAtLeast2Tracks = "Выберите по меньшей мере 2 трека для сравнения!";
                MsgBrBackupBaselineFileDoesntExist = "Файл первоначального опорного архива \"%%FILENAME%%\" не существует! Сначала создайте архив тегов вручную!";
                MsgBrThisIsTheBackupOfDifferentLibrary = "Это архив другой библиотеки! Все равно попытаться восстановить теги из этой резервной копии?";
                MsgBrCreateBaselineWarning = "Когда создается первый архив любой библиотеки, сначала создается полный опорный архив. " +
                    "Все последующие архивы являются разницей с опорным архивом. " +
                    "Если было изменено очень много тегов, то разностные архивы могут стать очень большими. Эта команда удалит " +
                    "ВСЕ разностные архивы ТЕКУЩЕЙ библиотеки и создаст новый опорный архив. ";
                MsgBrCreateNewBaselineBackupOrRestoreTagsFromAnotherLibraryBeforeMusicBeeRestart = "Требуется создать новый опорный архив или восстановить теги из ДРУГОЙ библиотеки " +
                    "ДО ПЕРЕЗАПУСКА MUSICBEE, чтобы изменение этой настройки вступило в силу!";
                MsgBrCreateNewBaseline = "Рекомендуется создать новый опорный архив после наведения порядка в тегах!";

                MsgMsrGiveNameToAsrPreset = "Задайте название пресета!";
                MsgMsrAreYouSureYouWantToSaveAsrPreset = "Сохранить пресет дополнительного поиска и замены под названием \"%%PRESET-NAME%%\"?";
                MsgMsrAreYouSureYouWantToOverwriteAsrPreset = "Перезаписать пресет дополнительного поиска и замены \"%%PRESET-NAME%%\"?";
                MsgMsrAreYouSureYouWantToOverwriteRenameAsrPreset = "Перезаписать пресет дополнительного поиска и замены \"%%PRESET-NAME%%\", переименовав его в \"%%NEWPRESETNAME%%\"?";
                MsgMsrAreYouSureYouWantToDeleteAsrPreset = "Удалить пресет дополнительного поиска и замены \"%%PRESET-NAME%%\"?";
                MsgAsrPredefinedPresetsCantBeChanged = "Стандартные пресеты нельзя изменять. Редактор пресетов будет открыт в режиме просмотра.\n\n"
                    + "Отключить показ этого предупреждения?";
                MsgMsrSavePreset = "Сохранить пресет";
                MsgMsrDeletePreset = "Удалить пресет";

                MsgUnsupportedMusicBeeFontType = "Тип шрифта MusicBee не поддерживается плагином!\n" +
                    "Или выберите другой шрифт в настройках MusicBee, или отключите использование шрифта MusicBee в настройках плагина.";

                CtlNewAsrPreset = "<Новый пресет ДПЗ>";
                CtlAsrSyntaxError = "СИНТАКСИЧЕСКАЯ ОШИБКА!";

                CtlAsrPresetEditorPleaseWait = " НЕМНОГО ПОДОЖТИТЕ! ";

                CtlLrPresetAutoName = "(Автоматическое имя)";
                CtlLrInvalidPresetFormatAutoName = "Некорректный формат пресета!";
                CtlLrError = "ОШИБКА!";
                CtlLrParsingError = "#ОШИБКА РАЗБОРА РЕЗУЛЬТАТА!";
                CtlLrInvalidValue = "#НЕКОРРЕКТНОЕ ЗНАЧЕНИЕ РЕЗУЛЬТАТА!";
                CtlLrRefreshUi = "Обновите панель!";
                CtlLrIncorrectReportPresetId = "Некорректный ID $LR()!";

                CtlCopyTagFilter = "Текстовые файлы|*.txt";

                ExProhibitedParameterCombination = "Запрещенная комбинация параметров: interactive = %%interactive%%, filterResults = %%filterResults%%";
                ExIncorrectLrHotkeySlot = "Некорректный слот горячей клавиши ОБ: %%SLOT%%!";
                ExThisFieldAlreadyDefinedInPreset = "Это поле уже определено в пресете!";


                ExInvalidDropdownStyle = "Некорректный стиль выпадающего списка!";
                ExImpossibleToStretchThumb = "Невозможно масштабировать ползунок полосы прокрутки. Пустая ссылка на изображение ползунка.";
                ExMaskAndImageMustBeTheSameSize = "Маска и изображение должны быть одинакового размера!";

                
                CtlAutoRateCalculating = "Идет расчет...";

                CtlAsrCellTooTip = "Неотмеченные строки не будут обработаны\n" +
                    "\n" +
                    "Щелчок при нажатой клавише Shift отметит/снимет отметку во всех строках, содержащих те же теги, что и в этой строке";
                CtlAsrAllTagsCellTooTip = "Неотмеченные строки не будут обработаны\n" +
                    "\n" +
                    "Щелчок при нажатой клавише Shift отметит/снимет отметку во всех строках, содержащих тот же тег\n" +
                    "\n" +
                    "Щелчок при нажатой клавише Ctrl добавит/удалит тег, указанный в строке, к списку/из списка обрабатываемых/исключенных тегов";
                MsgAsrPresetsUsingAllTagsPseudoTagNameCannotBeAutoApplied = "Невозможно применить пресет %%PRESET-NAME%%! Пресеты ДПЗ, использующие псевдо-тег %%AllTagsPseudoTagName%%, " +
                    "не могут применяться ни автоматически, ни при помощи комбинации клавиш!";


                SbAutoBackingUp = "Автосохранение архива тегов...";
                SbMovingBackupsToNewFolder = "Идет перемещение архивов в новую папку...";
                SbMakingTagBackup = "Сохранение архива тегов...";
                SbRestoringTagsFromBackup = "Идет восстановление тегов из архива...";
                SbRenamingMovingBackup = "Идет переименование/перемещение архива...";
                SbMovingBackups = "Идет перемещение архивов...";
                SbDeletingBackups = "Идет удаление архивов...";
                SbTagAutoBackupSkipped = "Авто-архивирование тегов пропущено (не было изменений с момента последней архивации)";
                SbComparingTags = "Идет сравнение тегов с основным архивом... ";

                CtlWarning = "ПРЕДУПРЕЖДЕНИЕ!";
                CtlMusicBeeBackupType = "Архив тегов MusicBee|*.xml";
                CtlSaveBackupTitle = "ПЕРЕЙДИТЕ В НУЖНУЮ ПАПКУ, НАПИШИТЕ НАЗВАНИЕ АРХИВА ВНИЗУ ОКНА И НАЖМИТЕ \"СОХРАНИТЬ\"";
                CtlRestoreBackupTitle = "ПЕРЕЙДИТЕ В НУЖНУЮ ПАПКУ, ВЫБЕРИТЕ АРХИВ И НАЖМИТЕ \"ОТКРЫТЬ\"";
                CtlRenameSelectBackupTitle = "ПЕРЕЙДИТЕ В НУЖНУЮ ПАПКУ, ВЫБЕРИТЕ АРХИВ, КОТОРЫЙ ХОТИТЕ ПЕРЕИМЕНОВАТЬ/ПЕРЕМЕСТИТЬ И НАЖМИТЕ \"ОТКРЫТЬ\"";
                CtlRenameSaveBackupTitle = "ПЕРЕЙДИТЕ В НУЖНУЮ ПАПКУ, НАПИШИТЕ НОВОЕ НАЗВАНИЕ АРХИВА ВНИЗУ ОКНА И НАЖМИТЕ \"СОХРАНИТЬ\"";
                CtlMoveSelectBackupsTitle = "ПЕРЕЙДИТЕ В НУЖНУЮ ПАПКУ, ВЫБЕРИТЕ АРХИВЫ, КОТОРЫЕ ХОТИТЕ ПЕРЕМЕСТИТЬ И НАЖМИТЕ \"ОТКРЫТЬ\"";
                CtlMoveSaveBackupsTitle = "ПЕРЕЙДИТЕ В ПАПКУ, В КОТОРУЮ ХОТИТЕ ПЕРЕМЕСТИТЬ АРХИВЫ И НАЖМИТЕ \"СОХРАНИТЬ\"";
                CtlDeleteBackupsTitle = "ПЕРЕЙДИТЕ В НУЖНУЮ ПАПКУ, ВЫБЕРИТЕ АРХИВ(Ы) ДЛЯ УДАЛЕНИЯ И НАЖМИТЕ \"ОТКРЫТЬ\"";
                CtlSelectThisFolder = "[ВЫБРАТЬ ЭТУ ПАПКУ]";
                CtlNoBackupData = "<Нет архивных данных>";
                CtlNoDifferences = "<Нет отличий>";
                CtlNoPreview = "<Предварительный просмотр не сформирован>";
                CtlMixedValues = "(Разные значения)";
                CtlMixedValuesSameAsInLibrary = "Совпадает с библиотекой";
                CtlMixedValuesDifferentFromLibrary = "Отличается от библиотеки";

                CtlTags = "Теги";

                MsgNotAllowedSymbols = "Разрешены только буквы ASCII, числа и символы - : _ .!";
                MsgPresetExists = "Пресет с ID-ом %%ID%% уже существует!";

                CtlWholeCuesheetWillBeReencoded = "ВНИМАНИЕ: ОДИН ИЛИ НЕСКЛЬКО ТРЕКОВ ОТНОСЯТСЯ К ФАЙЛУ РАЗМЕТКИ. КОДИРОВКА БУДЕТ ИЗМЕНЕНА ДЛЯ ВСЕГО ФАЙЛА РАЗМЕТКИ!";

                CtlMSR = "МПЗ: ";

                CtlMixedFilters = "(Несколько фильтров)";

                MsgFirstSelectWhichFieldYouWantToAssignFunctionIdTo = "Сначала выберите, какому полю вы хотите назначить ID функции вирт. тегов (самый левый выпадающий список на линии ID-а функции)!";

                ////Defaults for controls
                //charBox = string.Empty;

                //exceptions = "the a an and or not";
                //exceptionChars = "\" ' ( { [ /";
                //sentenceSeparators = "&";

                BackupDefaultPrefix = "Архив тегов ";
                AsrPresetNaming = "Пресет ДПЗ";

                if (SavedSettings.autoBackupPrefix == null) SavedSettings.autoBackupPrefix = "Автоматический архив тегов ";
            }
            else
            {
                Language = "en";
            }
            #endregion

            #region Resetting invalid/absent settings again
            PluginHelpFilePath = Path.Combine(PluginsPath, PluginHelpFileName) + "." + Language + ".chm";

            if (SavedSettings.autoBackupPrefix == null)
                SavedSettings.autoBackupPrefix = "Tag Auto-Backup ";


            if (SavedSettings.reportPresets == null && SavedSettings.reportsPresets == null)
            {
                SavedSettings.reportPresets = Array.Empty<ReportPreset>();
            }
            else if (SavedSettings.reportPresets == null) //******** remove later!!!!!
            {
                SavedSettings.reportPresets = new ReportPreset[SavedSettings.reportsPresets.Length];
                SavedSettings.reportsPresets.CopyTo(SavedSettings.reportPresets, 0);
            }

            //Let's remove all predefined presets and recreate them from scratch (except for allowed user customizations)
            var existingPredefinedPresets = new SortedDictionary<Guid, ReportPreset>(); //<permanentGuid, ReportPreset>

            var existingPredefinedPresetCount = 0;
            for (var i = 0; i < SavedSettings.reportPresets.Length; i++)
            {
                if (!SavedSettings.reportPresets[i].userPreset)
                {
                    existingPredefinedPresets.Add(SavedSettings.reportPresets[i].permanentGuid, SavedSettings.reportPresets[i]);
                    existingPredefinedPresetCount++;
                }
            }

            var presetCounter = 0;
            var reportPresets = new ReportPreset[SavedSettings.reportPresets.Length - existingPredefinedPresetCount + PredefinedReportPresetCount];
            foreach (var reportPreset in SavedSettings.reportPresets)
            {
                if (reportPreset.userPreset)
                    reportPresets[presetCounter++] = reportPreset;
            }


            var groupings = new PresetColumnAttributes[3];
            var functions = new PresetColumnAttributes[9];


            //Library Totals
            groupings[0] = new PresetColumnAttributes(LrFunctionType.Grouping, new[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.Genre), null, null, false);
            groupings[1] = new PresetColumnAttributes(LrFunctionType.Grouping, new[] { string.Empty }, DisplayedAlbumArtistName, null, null, false);
            groupings[2] = new PresetColumnAttributes(LrFunctionType.Grouping, new[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.Album), null, null, false);

            for (var f = 0; f < groupings.Length; f++)
                groupings[f].columnIndices = new[] { f };


            functions[0] = new PresetColumnAttributes(LrFunctionType.Count, new[] { string.Empty }, DisplayedAlbumArtistName, null, null, false);
            functions[1] = new PresetColumnAttributes(LrFunctionType.Count, new[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.Genre), null, null, false);
            functions[2] = new PresetColumnAttributes(LrFunctionType.Count, new[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.Year), null, null, false);
            functions[3] = new PresetColumnAttributes(LrFunctionType.Count, new[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.Album), null, null, false);
            functions[4] = new PresetColumnAttributes(LrFunctionType.Count, new[] { string.Empty }, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url), null, null, false);
            functions[5] = new PresetColumnAttributes(LrFunctionType.Sum, new[] { string.Empty }, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Duration), null, null, false);
            functions[6] = new PresetColumnAttributes(LrFunctionType.Sum, new[] { string.Empty }, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Size), null, null, false);
            functions[7] = new PresetColumnAttributes(LrFunctionType.Sum, new[] { string.Empty }, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.PlayCount), null, null, false);
            functions[8] = new PresetColumnAttributes(LrFunctionType.Sum, new[] { string.Empty }, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.SkipCount), null, null, false);

            for (var f = 0; f < functions.Length; f++)
                functions[f].columnIndices = new[] { groupings.Length + f };


            var functionIds = new string[functions.Length];

            var destinationTags = new string[functions.Length];
            for (var i = 0; i < destinationTags.Length; i++)
                destinationTags[i] = NullTagName;

            //Let's copy allowed user customizations from existing predefined preset (if it exists)
            var presetPermanentGuid = Guid.Parse("450A95FE-E660-44B7-B34C-1169C9466493");
            var libraryReportsPreset = GetCreatePredefinedPreset(presetPermanentGuid, LibraryTotalsPresetName, existingPredefinedPresets,
                groupings, functions, destinationTags, functionIds);

            reportPresets[presetCounter++] = libraryReportsPreset;


            //Library Averages
            groupings = new PresetColumnAttributes[3];
            functions = new PresetColumnAttributes[10];

            groupings[0] = new PresetColumnAttributes(LrFunctionType.Grouping, new[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.Genre), null, null, false);
            groupings[1] = new PresetColumnAttributes(LrFunctionType.Grouping, new[] { string.Empty }, DisplayedAlbumArtistName, null, null, false);
            groupings[2] = new PresetColumnAttributes(LrFunctionType.Grouping, new[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.Album), null, null, false);

            for (var f = 0; f < groupings.Length; f++)
                groupings[f].columnIndices = new[] { f };


            functions[0] = new PresetColumnAttributes(LrFunctionType.AverageCount, new[] { string.Empty },
                MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url), MbApiInterface.Setting_GetFieldName(MetaDataType.Artist), null, false);
            functions[1] = new PresetColumnAttributes(LrFunctionType.AverageCount, new[] { string.Empty },
                MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url), MbApiInterface.Setting_GetFieldName(MetaDataType.Album), null, false);
            functions[2] = new PresetColumnAttributes(LrFunctionType.AverageCount, new[] { string.Empty },
                MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url), MbApiInterface.Setting_GetFieldName(MetaDataType.Genre), null, false);
            functions[3] = new PresetColumnAttributes(LrFunctionType.AverageCount, new[] { string.Empty },
                MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url), MbApiInterface.Setting_GetFieldName(MetaDataType.Year), null, false);
            functions[4] = new PresetColumnAttributes(LrFunctionType.Average, new[] { string.Empty },
                MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Bitrate), MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url), null, false);
            functions[5] = new PresetColumnAttributes(LrFunctionType.Average, new[] { string.Empty },
                MbApiInterface.Setting_GetFieldName(MetaDataType.Rating), MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url), null, false);
            functions[6] = new PresetColumnAttributes(LrFunctionType.Average, new[] { string.Empty },
                MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Duration), MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url), null, false);
            functions[7] = new PresetColumnAttributes(LrFunctionType.Average, new[] { string.Empty },
                MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Size), MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url), null, false);
            functions[8] = new PresetColumnAttributes(LrFunctionType.Average, new[] { string.Empty },
                MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.PlayCount), MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url), null, false);
            functions[9] = new PresetColumnAttributes(LrFunctionType.Average, new[] { string.Empty },
                MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.SkipCount), MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url), null, false);

            for (var f = 0; f < functions.Length; f++)
                functions[f].columnIndices = new[] { groupings.Length + f };


            functionIds = new string[functions.Length];

            destinationTags = new string[functions.Length];
            for (var i = 0; i < destinationTags.Length; i++)
                destinationTags[i] = NullTagName;

            //Let's copy allowed user customizations from existing predefined preset (if it exists)
            presetPermanentGuid = Guid.Parse("2759C09A-B982-4FC5-9872-FBD27A4D8F5E");
            libraryReportsPreset = GetCreatePredefinedPreset(presetPermanentGuid, LibraryAveragesPresetName, existingPredefinedPresets,
                groupings, functions, destinationTags, functionIds);

            reportPresets[presetCounter++] = libraryReportsPreset;


            //CD Booklet
            groupings = new PresetColumnAttributes[6];
            functions = Array.Empty<PresetColumnAttributes>();

            groupings[0] = new PresetColumnAttributes(LrFunctionType.Grouping, new string[] { null }, SequenceNumberName, null, null, false);
            groupings[1] = new PresetColumnAttributes(LrFunctionType.Grouping, new[] { string.Empty }, DisplayedAlbumArtistName, null, null, false);
            groupings[2] = new PresetColumnAttributes(LrFunctionType.Grouping, new[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.Album), null, null, false);
            groupings[3] = new PresetColumnAttributes(LrFunctionType.Grouping, new string[] { null }, MbApiInterface.Setting_GetFieldName(MetaDataType.Artwork), null, null, false);
            groupings[4] = new PresetColumnAttributes(LrFunctionType.Grouping, new[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.TrackTitle), null, null, false);
            groupings[5] = new PresetColumnAttributes(LrFunctionType.Grouping, new[] { string.Empty }, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Duration), null, null, false);

            for (var f = 0; f < groupings.Length; f++)
                groupings[f].columnIndices = new[] { f };


            functionIds = new string[functions.Length];

            destinationTags = new string[functions.Length];
            for (var i = 0; i < destinationTags.Length; i++) //-V3116
                destinationTags[i] = NullTagName;

            //Let's copy allowed user customizations from existing predefined preset (if it exists)
            presetPermanentGuid = Guid.Parse("C7EACE32-B70F-4E5E-BEF1-2D10BE3B74E5");
            libraryReportsPreset = GetCreatePredefinedPreset(presetPermanentGuid, CDBookletPresetName, existingPredefinedPresets,
                groupings, functions, destinationTags, functionIds);

            reportPresets[presetCounter++] = libraryReportsPreset;


            //Albums and Tracks
            groupings = new PresetColumnAttributes[5];
            functions = Array.Empty<PresetColumnAttributes>();

            groupings[0] = new PresetColumnAttributes(LrFunctionType.Grouping, new[] { string.Empty }, DisplayedAlbumArtistName, null, null, false);
            groupings[1] = new PresetColumnAttributes(LrFunctionType.Grouping, new[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.Album), null, null, false);
            groupings[2] = new PresetColumnAttributes(LrFunctionType.Grouping, new string[] { null }, MbApiInterface.Setting_GetFieldName(MetaDataType.Artwork), null, null, false);
            groupings[3] = new PresetColumnAttributes(LrFunctionType.Grouping, new[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.TrackNo), null, null, false);
            groupings[4] = new PresetColumnAttributes(LrFunctionType.Grouping, new[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.TrackTitle), null, null, false);

            for (var f = 0; f < groupings.Length; f++)
                groupings[f].columnIndices = new[] { f };


            functionIds = new string[functions.Length];

            destinationTags = new string[functions.Length];
            for (var i = 0; i < destinationTags.Length; i++) //-V3116
                destinationTags[i] = NullTagName;

            //Let's copy allowed user customizations from existing predefined preset (if it exists)
            presetPermanentGuid = Guid.Parse("F14133BF-7D9E-403F-B2F2-B3A2BE669BC8");
            libraryReportsPreset = GetCreatePredefinedPreset(presetPermanentGuid, AlbumsAndTracksPresetName, existingPredefinedPresets,
                groupings, functions, destinationTags, functionIds);

            reportPresets[presetCounter++] = libraryReportsPreset;


            //Album Grid
            groupings = new PresetColumnAttributes[3];
            functions = Array.Empty<PresetColumnAttributes>();

            groupings[0] = new PresetColumnAttributes(LrFunctionType.Grouping, new[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.Album), null, null, false);
            groupings[1] = new PresetColumnAttributes(LrFunctionType.Grouping, new[] { string.Empty }, DisplayedAlbumArtistName, null, null, false);
            groupings[2] = new PresetColumnAttributes(LrFunctionType.Grouping, new string[] { null }, MbApiInterface.Setting_GetFieldName(MetaDataType.Artwork), null, null, false);

            for (var f = 0; f < groupings.Length; f++)
                groupings[f].columnIndices = new[] { f };


            functionIds = new string[functions.Length];

            destinationTags = new string[functions.Length];
            for (var i = 0; i < destinationTags.Length; i++) //-V3116
                destinationTags[i] = NullTagName;

            //Let's copy allowed user customizations from existing predefined preset (if it exists)
            presetPermanentGuid = Guid.Parse("FA3D3B21-9B80-4C6C-AC67-1D8FC2A3CEBA");
            libraryReportsPreset = GetCreatePredefinedPreset(presetPermanentGuid, AlbumGridPresetName, existingPredefinedPresets,
                groupings, functions, destinationTags, functionIds);

            reportPresets[presetCounter++] = libraryReportsPreset;


            foreach (var reportPreset in reportPresets) //******* remove later !!!
            {
                if (reportPreset != null && reportPreset.exportedTrackListName == null)
                    reportPreset.exportedTrackListName = ExportedTrackList;


                if (reportPreset != null && (reportPreset.operations == null || reportPreset.operations.Length < reportPreset.functions.Length))
                    reportPreset.operations = new int[reportPreset.functions.Length];

                if (reportPreset != null && (reportPreset.mulDivFactors == null || reportPreset.mulDivFactors.Length < reportPreset.functions.Length))
                    reportPreset.mulDivFactors = new string[reportPreset.functions.Length];

                if (reportPreset != null && (reportPreset.precisionDigits == null || reportPreset.precisionDigits.Length < reportPreset.functions.Length))
                    reportPreset.precisionDigits = new string[reportPreset.functions.Length];

                if (reportPreset != null && (reportPreset.appendTexts == null || reportPreset.appendTexts.Length < reportPreset.functions.Length))
                    reportPreset.appendTexts = new string[reportPreset.functions.Length];
            }

            SavedSettings.reportPresets = reportPresets;


            //Let's reset invalid defaults for controls
            if (SavedSettings.closeShowHiddenWindows == 0)
                SavedSettings.closeShowHiddenWindows = 1;

            if (SavedSettings.changeCaseFlag == 0) SavedSettings.changeCaseFlag = 1;

            if (SavedSettings.defaultRating == 0) SavedSettings.defaultRating = 5;


            fillTagNames();

            //Again let's reset invalid defaults for controls
            if (GetTagId(SavedSettings.copySourceTagName) == 0)
                SavedSettings.copySourceTagName = ArtistArtistsName;
            if (GetTagId(SavedSettings.changeCaseSourceTagName) == 0)
                SavedSettings.changeCaseSourceTagName = ArtistArtistsName;
            if (GetTagId(SavedSettings.swapTagsSourceTagName) == 0)
                SavedSettings.swapTagsSourceTagName = ArtistArtistsName;

            if (string.IsNullOrEmpty(SavedSettings.multipleItemsSplitterChar2))
            {
                SavedSettings.multipleItemsSplitterChar1 = ";";
                SavedSettings.multipleItemsSplitterChar2 = "; ";
            }

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
                var tagNameList = new List<string>();
                FillListByTagNames(tagNameList, false, false, false, false, false, false);
                //FillListWithProps(tagNameList);

                tagNameList.RemoveExisting("Sort Artist");
                tagNameList.RemoveExisting("Sort Album Artist");

                var tagIdList = new List<int>();
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
                for (var i = 0; i < SavedSettings.copyTagsTagSets.Length; i++)
                    SavedSettings.copyTagsTagSets[i].setPosition = i + 1;
            }
            #endregion

            #region Final initialization
            //Final initialization
            MbForm = Control.FromHandle(MbApiInterface.MB_GetWindowHandle()) as Form;

            EmptyButton = new Button();
            EmptyForm = new Form(); //For themed bitmaps disposing control

            MusicName = MbApiInterface.MB_GetLocalisation("Main.tree.Music", "Music");

            TagToolsSubmenu = (ToolStripMenuItem)MbApiInterface.MB_AddMenuItem("mnuTools/[1]" + PluginMenuGroupName, null, null);
            if (!SavedSettings.dontShowContextMenu)
                TagToolsContextSubmenu = MbApiInterface.MB_AddMenuItem("context.Main/" + PluginMenuGroupName, null, null) as ToolStripMenuItem;


            About.PluginInfoVersion = PluginInfoVersion;
            About.Name = PluginName;
            About.Description = PluginDescription;
            About.Author = "boroda";
            About.TargetApplication = string.Empty;   //current only applies to artwork, lyrics or instant messenger name that appears in the provider drop down selector or target Instant Messenger
            About.Type = PluginType.General;
            About.VersionMajor = (short)System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major;
            About.VersionMinor = (short)System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor;
            About.Revision = (short)System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build; //Autogenerated, number of days since 2000-01-01 at build time
            About.MinInterfaceVersion = 39;
            About.MinApiRevision = 51;
            About.ReceiveNotifications = (ReceiveNotificationFlags.PlayerEvents | ReceiveNotificationFlags.TagEvents);
            About.ConfigurationPanelHeight = 0;   //height in pixels that musicbee should reserve in a panel for config settings. When set, a handle to an empty panel will be passed to the Configure function

            var pluginBuildTime = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision;

            PluginVersion = PluginVersionString + About.VersionMajor + "." + About.VersionMinor + "." + About.Revision + "." + pluginBuildTime;
            #endregion

            return About;
        }
        #endregion

        #region Other plugin interface methods
        public bool Configure(IntPtr panelHandle)
        {
            //save any persistent settings in a sub-folder of this path
            //string dataPath = mbApiInterface.Setting_GetPersistentStoragePath();
            //panelHandle will only be set if you set about.ConfigurationPanelHeight to a non-zero value
            //keep in mind the panel width is scaled according to the font the user has selected
            //if about.ConfigurationPanelHeight is set to 0, you can display your own popup window

            var tagToolsForm = new Settings(this);
            PluginWindowTemplate.Display(tagToolsForm, true);

            SaveSettings();

            return true;
        }

        //called by MusicBee when the user clicks Apply or Save in the MusicBee Preferences screen.
        //It's up to you to figure out whether anything has changed and needs updating
        public void SaveSettings()
        {
            var unicode = Encoding.UTF8;

            var stream = File.Open(PluginSettingsFilePath + ".new-format", FileMode.Create, FileAccess.Write, FileShare.None); //********* remove later!!!
            var file = new StreamWriter(stream, unicode);

            file.Close();


            stream = File.Open(PluginSettingsFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
            file = new StreamWriter(stream, unicode);

            var controlsDefaultsSerializer = new XmlSerializer(typeof(PluginSettings));
            controlsDefaultsSerializer.Serialize(file, SavedSettings);

            file.Close();
        }

        //MusicBee is closing the plugin (plugin is being disabled by user or MusicBee is shutting down)
        public void Close(PluginCloseReason reason)
        {
            PeriodicUiRefreshTimer?.Dispose();
            PeriodicUiRefreshTimer = null;

            PeriodicAutoBackupTimer?.Dispose();
            PeriodicAutoBackupTimer = null;


            lock (OpenedForms)
            {
                PluginClosing = true;

                for (var i = OpenedForms.Count - 1; i >= 0; i--)
                {
                    OpenedForms[i].backgroundTaskIsCanceled = true;
                    OpenedForms[i].Close();
                }
            }


            //Let's dispose all global bitmaps
            DisposePluginBitmaps();


            LibraryReportsCommandForAutoApplying?.Dispose();
            LibraryReportsCommandForAutoApplying = null;
            LibraryReportsCommandForHotkeys?.Dispose();
            LibraryReportsCommandForHotkeys = null;
            LibraryReportsCommandForFunctionIds?.Dispose();
            LibraryReportsCommandForFunctionIds = null;

            EmptyButton?.Dispose();
            EmptyForm?.Dispose();

            if (!Uninstalled)
                SaveSettings();
        }

        //uninstall this plugin - clean up any persisted files
        public void Uninstall()
        {
            //Let's delete backups
            if (Directory.Exists(BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory)))
            {
                if (PeriodicAutoBackupTimer != null)
                {
                    PeriodicAutoBackupTimer.Dispose();
                    PeriodicAutoBackupTimer = null;
                }

                lock (TracksNeededToBeBackedUp)
                    Directory.Delete(BrGetAutoBackupDirectory(SavedSettings.autoBackupDirectory), true);
            }


            //Let's delete settings file
            if (File.Exists(PluginSettingsFilePath))
            {
                File.Delete(PluginSettingsFilePath);
            }


            //Let's delete working presets files
            if (Directory.Exists(PresetsPath))
            {
                Directory.Delete(PresetsPath, true);
            }


            //Let's try to delete predefined presets files (this can't be done by plugin if it's
            //installed to "C:\Program Files (x86)\MusicBee\Plugins" folder)
            var presetsPath = Path.Combine(PluginsPath, AsrPresetsDirectory);

            try
            {
                if (Directory.Exists(presetsPath))
                {
                    Directory.Delete(presetsPath, true);
                }
            }
            catch
            {
                //Ignored...
            }

            Uninstalled = true;
        }

        private void autoApplyAsrUpdateLrCache(string newChangedFileUrl, string changingFile = null)
        {
            if (!SavedSettings.dontShowAsr || !SavedSettings.dontShowLibraryReports)
            {
                var autoApplyAsrUpdateLrCache = false;

                if (newChangedFileUrl != null)
                {
                    lock (FilesUpdatedByPlugin)
                    {
                        if (!FilesUpdatedByPlugin.RemoveExisting(newChangedFileUrl))
                            autoApplyAsrUpdateLrCache = true;
                    }

                    if (autoApplyAsrUpdateLrCache && !SavedSettings.dontShowAsr)
                        AsrAutoApplyPresets(newChangedFileUrl);
                }

                if (changingFile != null)
                {
                    if (!FilesUpdatedByPlugin.Contains(changingFile))
                        autoApplyAsrUpdateLrCache = true;
                }


                if (autoApplyAsrUpdateLrCache && !SavedSettings.dontShowLibraryReports)
                    LrNotifyFileTagsChanged(newChangedFileUrl, changingFile);
            }
        }

        //receive event notifications from MusicBee
        //you need to set about.ReceiveNotificationFlags = PlayerEvents to receive all notifications, and not just the startup event
        public void ReceiveNotification(string sourceFileUrl, NotificationType type)
        {
            if (!PrioritySet)
            {
                PrioritySet = true;
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;
            }

            //perform some action depending on the notification type
            switch (type)
            {
                case NotificationType.PluginStartup:
                    //perform startup initialization

                    MissingArtwork = Resources.missing_artwork;
                    DefaultArtwork = MissingArtwork;


                    //ASR & LR init
                    InitAsr();
                    InitLr();

                    //(Auto)backup init
                    InitBackupRestore();


                    //Let's create plugin main and context menu items
                    MbForm.Invoke((MethodInvoker)delegate 
                    {
                        getButtonTextBoxDpiFontScaling(); 
                        prepareThemedBitmapsAndColors(); 

                        addPluginContextMenuItems(); 
                        addPluginMenuItems(); 
                    });


                    //Let's refresh MusicBee UI every 5 sec if there are any tags changed by plugin since last refresh
                    PeriodicUiRefreshTimer = new System.Threading.Timer(RegularUiRefresh, null, RefreshUI_Delay, RefreshUI_Delay);


                    //Auto rate at startup
                    AutoRate.AutoRateOnStartup(this);


                    //Execute library reports on startup
                    AutoApplyReportPresets();


                    //Let's refresh UI
                    RefreshPanels(true, true, true);

                    break;
                case NotificationType.TrackChanged:
                    InitialSkipCount = Convert.ToInt32(MbApiInterface.Library_GetFileProperty(sourceFileUrl, FilePropertyType.SkipCount));

                    break;
                case NotificationType.PlayCountersChanged:
                    //Play count should be changed, but track is not changed yet

                    if (SavedSettings.lastSkippedTagId != 0)
                    {
                        var newSkipCount = Convert.ToInt32(MbApiInterface.Library_GetFileProperty(sourceFileUrl, FilePropertyType.SkipCount));
                        if (newSkipCount != InitialSkipCount)
                        {
                            string lastSkippedDate;
                            var now = DateTime.Now;

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

                    autoApplyAsrUpdateLrCache(sourceFileUrl);

                    if (SavedSettings.autoRateOnTrackProperties)
                        AutoRate.AutoRateLive(sourceFileUrl);


                    if (!SavedSettings.dontShowBackupRestore && SavedSettings.dontSkipAutoBackupsIfOnlyPlayCountsChanged)
                        UpdateTrackForBackup(sourceFileUrl);

                    break;
                case NotificationType.TagsChanging:
                    autoApplyAsrUpdateLrCache(null, sourceFileUrl);

                    break;
                case NotificationType.FileRemovedFromLibrary:
                    autoApplyAsrUpdateLrCache(sourceFileUrl);

                    break;
                case NotificationType.FileRenamed:
                    if (LibraryReportsCommandForFunctionIds != null)
                        LibraryReportsCommandForFunctionIds.updateLrCacheForRenamedTrack(sourceFileUrl);

                    break;
                case NotificationType.TagsChanged:
                case NotificationType.ReplayGainChanged:
                case NotificationType.FileAddedToLibrary:
                    autoApplyAsrUpdateLrCache(sourceFileUrl);


                    if (!SavedSettings.dontShowBackupRestore)
                        UpdateTrackForBackup(sourceFileUrl);

                    break;
                case NotificationType.RatingChanged:
                    autoApplyAsrUpdateLrCache(sourceFileUrl);

                    if (!SavedSettings.dontShowCAR)
                    {
                        if (SavedSettings.calculateAlbumRatingAtTagsChanged)
                            CalculateAverageAlbumRating.CalculateAlbumRatingForAlbum(sourceFileUrl);
                    }


                    if (!SavedSettings.dontShowBackupRestore)
                        UpdateTrackForBackup(sourceFileUrl);

                    break;
            }
        }
        #endregion

        #region Plugin helper methods
        //Themed bitmaps
        internal static Bitmap ReplaceBitmap(Image oldImage, Bitmap newBitmap)
        {
            if (newBitmap == null)
                return (Bitmap)oldImage;

            oldImage?.Dispose();

            return new Bitmap(newBitmap);
        }

        internal static Bitmap CopyBitmap(Bitmap newBitmap)
        {
            if (newBitmap == null)
                return null;
            else
                return new Bitmap(newBitmap);
        }

        internal static void DisposePluginBitmaps()
        {
            DisabledDownArrowComboBoxImage?.Dispose();
            DownArrowComboBoxImage?.Dispose();

            UpArrowImage?.Dispose();
            DownArrowImage?.Dispose();

            ThumbTopImage?.Dispose();
            ThumbMiddleVerticalImage?.Dispose();
            ThumbBottomImage?.Dispose();


            LeftArrowImage?.Dispose();
            RightArrowImage?.Dispose();

            ThumbLeftImage?.Dispose();
            ThumbMiddleHorizontalImage?.Dispose();
            ThumbRightImage?.Dispose();


            CheckedState?.Dispose();
            UncheckedState?.Dispose();


            WarningWide?.Dispose();

            ErrorWide?.Dispose();
            FatalErrorWide?.Dispose();
            ErrorFatalErrorWide?.Dispose();


            Warning?.Dispose();

            Error?.Dispose();
            FatalError?.Dispose();
            ErrorFatalError?.Dispose();


            Search?.Dispose();
            Window?.Dispose();

            Gear?.Dispose();

            AutoAppliedPresetsAccent?.Dispose();
            AutoAppliedPresetsDimmed?.Dispose();

            PredefinedPresetsAccent?.Dispose();
            PredefinedPresetsDimmed?.Dispose();

            CustomizedPresetsAccent?.Dispose();
            CustomizedPresetsDimmed?.Dispose();

            UserPresetsAccent?.Dispose();
            UserPresetsDimmed?.Dispose();

            PlaylistPresetsAccent?.Dispose();
            PlaylistPresetsDimmed?.Dispose();

            FunctionIdPresetsAccent?.Dispose();
            FunctionIdPresetsDimmed?.Dispose();

            HotkeyPresetsAccent?.Dispose();
            HotkeyPresetsDimmed?.Dispose();

            UncheckAllFiltersAccent?.Dispose();
            UncheckAllFiltersDimmed?.Dispose();

            MissingArtwork?.Dispose();
        }

        internal static Color GetButtonBackColor()
        {
            var buttonBackCode = MbApiInterface.Setting_GetSkinElementColour((SkinElement)2, ElementState.ElementStateDefault, ElementComponent.ComponentBackground);
            Color buttonBackColor;

            //const float ButtonBackgroundWeight = LightDimmedWeight;
            const float ButtonBackgroundWeight = 1;
            if (buttonBackCode == 0) //Unsupported by older API
                //buttonBackColor = GetWeightedColor(Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinTrackAndArtistPanel, ElementState.ElementStateDefault, ElementComponent.ComponentBackground)), AccentColor, ButtonBackgroundWeight);
                buttonBackColor = GetWeightedColor(FormBackColor, AccentColor, ButtonBackgroundWeight);
            else if (buttonBackCode == -1) //Windows color scheme
                buttonBackColor = InputPanelBackColor;
            else
                buttonBackColor = Color.FromArgb(buttonBackCode);


            return buttonBackColor;
        }


        internal void getButtonTextBoxDpiFontScaling()
        {
            var scalingSampleForm = new PluginSampleWindow(this);
            scalingSampleForm.dontShowForm = true;
            scalingSampleForm.Show();
            DpiScaling = scalingSampleForm.dpiScaling;
            (ButtonHeightDpiFontScaling, TextBoxHeightDpiFontScaling) = scalingSampleForm.getButtonHeightDpiFontScaling();
            scalingSampleForm.Close();
        }

        internal static void WaitPreparedThemedBitmapsAndColors()
        {
            while (SizesColorsChanged)
                Thread.Sleep(ActionRetryDelay);
        }

        internal void prepareThemedBitmapsAndColors()
        {
            if (!SizesColorsChanged)
                return;


            //Skin controls
            const float AccentBackWeight = 0.70f;
            const float LightDimmedWeight = 0.90f;
            const float DimmedWeight = 0.65f;
            const float DeepDimmedWeight = 0.32f;
            const float ScrollBarsForeWeight = 0.75f;//***

            InputPanelForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanel, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));
            InputPanelBackColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanel, ElementState.ElementStateDefault, ElementComponent.ComponentBackground));
            InputPanelBorderColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanel, ElementState.ElementStateDefault, ElementComponent.ComponentBorder));

            if (!SavedSettings.dontUseSkinColors)
            {
                InputControlForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));
                InputControlBackColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentBackground));
                InputControlBorderColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentBorder));
                InputControlFocusedBorderColor = GetWeightedColor(InputControlBorderColor, InputControlForeColor);


                AccentColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanel, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));
                AccentSelectedColor = NoColor; //****

                DimmedAccentColor = GetWeightedColor(AccentColor, InputPanelBackColor, DimmedWeight);
                DeepDimmedAccentColor = GetWeightedColor(AccentColor, InputPanelBackColor, DeepDimmedWeight);

                FormForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanel, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));
                FormBackColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanel, ElementState.ElementStateDefault, ElementComponent.ComponentBackground));
                FormBorderColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanel, ElementState.ElementStateDefault, ElementComponent.ComponentBorder));

                InputControlDimmedForeColor = GetWeightedColor(InputControlForeColor, FormBackColor, DimmedWeight);
                InputControlDimmedBackColor = GetWeightedColor(InputControlBackColor, FormBackColor, DimmedWeight);


                //SKINNING BUTTONS (ESPECIALLY DISABLED BUTTONS)
                //Below: (SkinElement)2 - BUTTON??? enum code //****

                //Back colors
                Color buttonBackColor = GetButtonBackColor();

                var buttonBackLightDimmedAccentColor = GetWeightedColor(buttonBackColor, AccentColor, LightDimmedWeight);
                var buttonBackDimmedAccentColor = GetWeightedColor(buttonBackColor, AccentColor, DimmedWeight);
                var buttonBackDeepDimmedAccentColor = GetWeightedColor(buttonBackColor, AccentColor, DeepDimmedWeight);


                ButtonBackColor = buttonBackColor;
                ButtonDisabledBackColor = ButtonBackColor;

                const float MouseOverButtonBackColor = 0.535f;
                ButtonMouseOverBackColor = GetWeightedColor(ButtonBackColor, FormBackColor, MouseOverButtonBackColor);

                //***
                //float avgForeBrightness = GetAverageBrightness(_buttonBackColor);
                //float avgBackBrightness = GetAverageBrightness(_buttonBackColor);
                //if (Math.Abs(avgForeBrightness - avgBackBrightness) < 0.5f)
                //{
                //   if (avgBackBrightness < 0.5f)
                //       _buttonDisabledBackColor = GetWeightedColor(_buttonBackColor, Color.Black, 0.6f);
                //   else
                //       _buttonDisabledBackColor = GetWeightedColor(_buttonBackColor, Color.White, 0.6f);
                //}
                //***


                //Fore colors
                var buttonForeCode = MbApiInterface.Setting_GetSkinElementColour((SkinElement)2, ElementState.ElementStateDefault, ElementComponent.ComponentForeground);
                if (buttonForeCode != -1)
                    UseNativeButtonPaint = false;

                Color buttonForeColor;

                if (buttonForeCode == 0) //Unsupported by older API
                    buttonForeColor = AccentColor;
                else if (buttonForeCode == -1) //Windows color scheme
                    buttonForeColor = SystemColors.ControlText;
                else
                    buttonForeColor = Color.FromArgb(buttonForeCode);

                var buttonLightForeDimmedColor = GetWeightedColor(buttonForeColor, ButtonBackColor, LightDimmedWeight);
                var buttonForeDimmedColor = GetWeightedColor(buttonForeColor, ButtonBackColor, DimmedWeight);
                var buttonForeDeepDimmedColor = GetWeightedColor(buttonForeColor, ButtonBackColor, DeepDimmedWeight);

                ButtonForeColor = buttonForeColor;
                ButtonDisabledForeColor = GetWeightedColor(ButtonForeColor, ButtonDisabledBackColor);


                //Border colors
                var buttonBorderCode = MbApiInterface.Setting_GetSkinElementColour((SkinElement)2, ElementState.ElementStateDefault, ElementComponent.ComponentBorder);
                Color buttonBorderColor;

                if (buttonBorderCode == 0) //Unsupported by older API
                    buttonBorderColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanel, ElementState.ElementStateDefault, ElementComponent.ComponentBorder));
                else if (buttonBorderCode == -1) //Windows color scheme
                    buttonBorderColor = SystemColors.ControlText;
                else
                    buttonBorderColor = Color.FromArgb(buttonBorderCode);

                ButtonBorderColor = buttonBorderColor;
                ButtonFocusedBorderColor = GetWeightedColor(ButtonBorderColor, ButtonForeColor);
                ButtonDisabledBorderColor = GetWeightedColor(ButtonBorderColor, ButtonForeColor, 0.25f);


                ControlHighlightBackColor = buttonBackColor; //*** buttonBackLightDimmedAccentColor;
                ControlHighlightForeColor = buttonForeColor;

                var avgForeBrightness = GetAverageBrightness(ControlHighlightForeColor);
                var avgBackBrightness = GetAverageBrightness(ControlHighlightBackColor);
                if (Math.Abs(avgForeBrightness - avgBackBrightness) < 0.35f)
                {
                    if (avgForeBrightness < 0.5f)
                        ControlHighlightForeColor = GetWeightedColor(ControlHighlightForeColor, Color.Black, 0.4f);
                    else
                        ControlHighlightForeColor = GetWeightedColor(ControlHighlightForeColor, Color.White, 0.4f);
                }


                ScrollBarBackColor = InputControlBackColor;
                ScrollBarBorderColor = DeepDimmedAccentColor;
                NarrowScrollBarBackColor = InputControlBackColor;
                ScrollBarThumbAndSpansForeColor = GetWeightedColor(AccentColor, InputPanelBackColor, ScrollBarsForeWeight);//***
                ScrollBarThumbAndSpansBackColor = InputControlBackColor;
                ScrollBarThumbAndSpansBorderColor = InputControlBackColor;

                ScrollBarFocusedBorderColor = GetWeightedColor(ScrollBarBorderColor, ScrollBarThumbAndSpansForeColor);
            }
            else
            {
                InputControlForeColor = SystemColors.ControlText;
                InputControlBackColor = SystemColors.Window;
                InputControlBorderColor = SystemColors.ControlDark;
                InputControlFocusedBorderColor = SystemColors.ControlDark;


                //Color windowsAccentColor = GetWindowColorizationColor(true);

                var backColorNotSkinned = SystemColors.Control;
                AccentColor = SystemColors.ControlText;

                DimmedAccentColor = GetWeightedColor(AccentColor, backColorNotSkinned, DimmedWeight); //This is not used at the moment
                DeepDimmedAccentColor = GetWeightedColor(AccentColor, backColorNotSkinned, DeepDimmedWeight); //This is not used at the moment

                FormBackColor = SystemColors.Control;
                FormForeColor = SystemColors.ControlText; //This is not used at the moment

                //Skinning buttons (especially disabled buttons)
                //ButtonFocusedBorderColor = windowsAccentColor;
                ButtonFocusedBorderColor = ButtonBorderColor; //This is not used at the moment
                ButtonBorderColor = SystemColors.ActiveBorder; //This is not used at the moment

                ButtonBackColor = SystemColors.ButtonFace; //This is not used at the moment
                ButtonForeColor = SystemColors.ControlText; //This is not used at the moment

                ButtonDisabledBackColor = SystemColors.ActiveBorder; //This is not used at the moment
                ButtonDisabledForeColor = SystemColors.GrayText; //This is not used at the moment


                ControlHighlightBackColor = SystemColors.ButtonFace; //This is not used at the moment
                ControlHighlightForeColor = SystemColors.ControlText; //This is not used at the moment


                ScrollBarBackColor = SystemColors.ControlText;
                ScrollBarBorderColor = SystemColors.ControlText;
                NarrowScrollBarBackColor = SystemColors.Window;
                ScrollBarThumbAndSpansForeColor = SystemColors.ButtonFace;//***
                ScrollBarThumbAndSpansBackColor = SystemColors.Window;
                ScrollBarThumbAndSpansBorderColor = SystemColors.ControlText;

                ScrollBarFocusedBorderColor = SystemColors.ControlText;
            }



            var sampleColor = SystemColors.Highlight;
            DimmedHighlight = GetHighlightColor(sampleColor, AccentColor, FormBackColor);


            //Setting default & making themed colors
            HighlightColor = Color.Red;
            UntickedColor = AccentColor;

            TickedColor = GetHighlightColor(HighlightColor, sampleColor, FormBackColor, 0.5f);

            //Splitter is invisible by default. Let's draw it.
            SplitterColor = GetWeightedColor(SystemColors.Desktop, AccentColor, 0.8f);//***


            //Making themed bitmaps
            if (!SavedSettings.dontUseSkinColors) //It's in case if skinned & not skinned buttons use different flat styles
            {
                var scaledPx = (int)Math.Round(DpiScaling);
                SbBorderWidth = scaledPx; //HERE IS DPI scaling of SbBorderWidth

                var size = (int)Math.Round(17f * ButtonHeightDpiFontScaling);

                var wideHeight = (int)Math.Round(15f * ButtonHeightDpiFontScaling);
                var wideWidth = (int)Math.Round(30f * ButtonHeightDpiFontScaling);

                var midHeight = (int)Math.Round(15f * ButtonHeightDpiFontScaling);
                var midWidth = (int)Math.Round(15f * ButtonHeightDpiFontScaling);
                var midWideWidth = (int)Math.Round(30f * ButtonHeightDpiFontScaling);
                var midWiderWidth = (int)Math.Round(32f * ButtonHeightDpiFontScaling);

                var midWidestWidth = (int)Math.Round(43f * ButtonHeightDpiFontScaling);

                var smallSize = (int)Math.Round(15f * ButtonHeightDpiFontScaling);

                var scrollBarImagesWidth = ControlsTools.GetCustomScrollBarInitialWidth(DpiScaling, 0); //Second arg. must be: SbBorderWidth /= scaledPx//*****
                var comboBoxDownArrowSize = (int)Math.Round(DpiScaling * 1.238f * ScrollBarWidth);


                DisabledDownArrowComboBoxImage?.Dispose();
                if (GetAverageBrightness(ButtonBackColor) >= 0.5)
                    DisabledDownArrowComboBoxImage = GetSolidImageByBitmapMask(Color.Black,
                        Resources.down_arrow_combobox_b, comboBoxDownArrowSize, comboBoxDownArrowSize);
                else
                    DisabledDownArrowComboBoxImage = GetSolidImageByBitmapMask(Color.White,
                        Resources.down_arrow_combobox_b, comboBoxDownArrowSize, comboBoxDownArrowSize);


                DownArrowComboBoxImage?.Dispose();
                DownArrowComboBoxImage = GetSolidImageByBitmapMask(ScrollBarThumbAndSpansForeColor,
                    Resources.down_arrow_combobox_b, comboBoxDownArrowSize, comboBoxDownArrowSize);

                UpArrowImage?.Dispose();
                UpArrowImage = GetSolidImageByBitmapMask(ScrollBarThumbAndSpansForeColor,
                    Resources.up_arrow_b, scrollBarImagesWidth, scrollBarImagesWidth);

                DownArrowImage?.Dispose();
                DownArrowImage = MirrorBitmap(UpArrowImage, true);

                //EITHER:
                ThumbMiddleVerticalImage?.Dispose();
                ThumbMiddleVerticalImage = GetSolidImageByBitmapMask(ScrollBarThumbAndSpansForeColor,
                    Resources.thumb_middle_vertical_c,
                    scrollBarImagesWidth - 3 * scaledPx, Resources.thumb_middle_vertical_c.Height, //Here middle thumb image is without right transparent part
                    true, InterpolationMode.NearestNeighbor);


                //OR:
                //ThumbTopImage = GetSolidImageByBitmapMask(_scrollBarThumbAndSpansForeColor, Resources.thumb_top_b,
                //   scrollBarImagesWidth, (int)Math.Round(_dpiScaling * scrollBarImagesWidth * Resources.thumb_top_b.Height / Resources.thumb_top_b.Width));
                //ThemedBitmapAddRef(EmptyForm, ThumbTopImage);

                //ThumbMiddleImage = GetSolidImageByBitmapMask(_scrollBarThumbAndSpansForeColor, Resources.thumb_middle_b,
                //   scrollBarImagesWidth, (int)Math.Round(_dpiScaling * scrollBarImagesWidth * Resources.thumb_middle_b.Height / Resources.thumb_middle_b.Width));
                //ThemedBitmapAddRef(EmptyForm, ThumbMiddleImage);

                //ThumbBottomImage = MirrorBitmap(ThumbTopImage, true);
                //ThemedBitmapAddRef(EmptyForm, ThumbBottomImage);


                LeftArrowImage?.Dispose();
                LeftArrowImage = GetSolidImageByBitmapMask(ScrollBarThumbAndSpansForeColor,
                    Resources.left_arrow_b, scrollBarImagesWidth, scrollBarImagesWidth);

                RightArrowImage?.Dispose();
                RightArrowImage = MirrorBitmap(LeftArrowImage, false);

                //EITHER:
                ThumbMiddleHorizontalImage?.Dispose();
                ThumbMiddleHorizontalImage = GetSolidImageByBitmapMask(ScrollBarThumbAndSpansForeColor,
                    Resources.thumb_middle_horizontal_c,
                    Resources.thumb_middle_horizontal_c.Width, scrollBarImagesWidth - 3 * scaledPx, //Here middle thumb image is without bottom transparent part
                    true, InterpolationMode.NearestNeighbor);

                //OR:
                //ThumbLeftImage = GetSolidImageByBitmapMask(_scrollBarThumbAndSpansForeColor, Resources.thumb_left_b,
                //   scrollBarImagesWidth, (int)Math.Round(_dpiScaling * scrollBarImagesWidth * Resources.thumb_left_b.Height / Resources.thumb_left_b.Width));
                //ThemedBitmapAddRef(EmptyForm, ThumbLeftImage);

                //ThumbMiddleImage = GetSolidImageByBitmapMask(_scrollBarThumbAndSpansForeColor, Resources.thumb_middle_b,
                //   scrollBarImagesWidth, (int)Math.Round(_dpiScaling * scrollBarImagesWidth * Resources.thumb_middle_b.Height / Resources.thumb_middle_b.Width));
                //ThemedBitmapAddRef(EmptyForm, ThumbMiddleImage);

                //ThumbRightImage = MirrorBitmap(ThumbLeftImage, true);
                //ThemedBitmapAddRef(EmptyForm, ThumbRightImage);


                WarningWide?.Dispose();
                WarningWide = ScaleBitmap(Resources.warning_wide, PixelFormat.Format32bppArgb, InterpolationMode.HighQualityBicubic,
                    wideWidth, wideHeight);

                ErrorWide?.Dispose();
                ErrorWide = ScaleBitmap(Resources.error_wide, PixelFormat.Format32bppArgb, InterpolationMode.HighQualityBicubic,
                    midWideWidth, midHeight);

                FatalErrorWide?.Dispose();
                FatalErrorWide = ScaleBitmap(Resources.fatal_error_wide, PixelFormat.Format32bppArgb, InterpolationMode.HighQualityBicubic,
                    midWideWidth, midHeight);

                ErrorFatalErrorWide?.Dispose();
                ErrorFatalErrorWide = ScaleBitmap(Resources.error_fatal_error_wide, PixelFormat.Format32bppArgb, InterpolationMode.HighQualityBicubic,
                    midWidestWidth, midHeight);


                Warning?.Dispose();
                Warning = ScaleBitmap(Resources.warning, PixelFormat.Format32bppArgb, InterpolationMode.HighQualityBicubic,
                    smallSize, smallSize);

                Error?.Dispose();
                Error = ScaleBitmap(Resources.error, PixelFormat.Format32bppArgb, InterpolationMode.HighQualityBicubic,
                    midWidth, midHeight);

                FatalError?.Dispose();
                FatalError = ScaleBitmap(Resources.fatal_error, PixelFormat.Format32bppArgb, InterpolationMode.HighQualityBicubic,
                    midWidth, midHeight);

                ErrorFatalError?.Dispose();
                ErrorFatalError = ScaleBitmap(Resources.error_fatal_error, PixelFormat.Format32bppArgb, InterpolationMode.HighQualityBicubic,
                    midWiderWidth, midHeight);


                Gear?.Dispose();
                Gear = GetSolidImageByBitmapMask(ButtonForeColor, Resources.gear, size, size);

                ClearField?.Dispose();
                ClearField = GetSolidImageByBitmapMask(ButtonForeColor, Resources.uncheck_mark, size, size);
            }
            else
            {
                SbBorderWidth = 0; //Units: px; scroll bars not skinned

                var size = (int)Math.Round(19f * ButtonHeightDpiFontScaling);
                var wideHeight = (int)Math.Round(15f * ButtonHeightDpiFontScaling);
                var wideWidth = (int)Math.Round(30f * ButtonHeightDpiFontScaling);
                var smallSize = (int)Math.Round(15f * ButtonHeightDpiFontScaling);


                DownArrowComboBoxImage?.Dispose();
                DownArrowComboBoxImage = null;

                UpArrowImage?.Dispose();
                UpArrowImage = null;
                DownArrowImage?.Dispose();
                DownArrowImage = null;
                ThumbMiddleVerticalImage?.Dispose();
                ThumbMiddleVerticalImage = null;

                LeftArrowImage?.Dispose();
                LeftArrowImage = null;
                RightArrowImage?.Dispose();
                RightArrowImage = null;
                ThumbMiddleHorizontalImage?.Dispose();
                ThumbMiddleHorizontalImage = null;

                WarningWide?.Dispose();
                WarningWide = ScaleBitmap(Resources.warning_wide, PixelFormat.Format32bppArgb, InterpolationMode.HighQualityBicubic, wideWidth, wideHeight);

                Warning?.Dispose();
                Warning = ScaleBitmap(Resources.warning, PixelFormat.Format32bppArgb, InterpolationMode.HighQualityBicubic, smallSize, smallSize);

                Gear?.Dispose();
                Gear = GetSolidImageByBitmapMask(ButtonForeColor, Resources.gear, size, size);

                ClearField?.Dispose();
                ClearField = GetSolidImageByBitmapMask(ButtonForeColor, Resources.uncheck_mark, size, size);
            }


            //ASR
            var markSize = (int)Math.Round(15f * ButtonHeightDpiFontScaling);

            CheckedState?.Dispose();
            CheckedState = GetSolidImageByBitmapMask(AccentColor, Resources.check_mark, markSize, markSize);
            
            UncheckedState?.Dispose();
            UncheckedState = GetSolidImageByBitmapMask(GetWeightedColor(AccentColor, FormBackColor, AccentBackWeight), 
                Resources.uncheck_mark, markSize, markSize);
            
            var pictureSize = (int)Math.Round(19f * ButtonHeightDpiFontScaling);

            Search?.Dispose();
            Search = GetSolidImageByBitmapMask(AccentColor, Resources.search, pictureSize, pictureSize);
            

            AutoAppliedPresetsAccent?.Dispose();
            AutoAppliedPresetsAccent = GetSolidImageByBitmapMask(AccentColor, Resources.auto_applied_presets, pictureSize, pictureSize);
            
            AutoAppliedPresetsDimmed?.Dispose();
            AutoAppliedPresetsDimmed = GetSolidImageByBitmapMask(GetWeightedColor(AccentColor, FormBackColor, DeepDimmedWeight), 
                Resources.auto_applied_presets, pictureSize, pictureSize);
            
            PredefinedPresetsAccent?.Dispose();
            PredefinedPresetsAccent = GetSolidImageByBitmapMask(AccentColor, Resources.predefined_presets, pictureSize, pictureSize);
            
            PredefinedPresetsDimmed?.Dispose();
            PredefinedPresetsDimmed = GetSolidImageByBitmapMask(GetWeightedColor(AccentColor, FormBackColor, DeepDimmedWeight), 
                Resources.predefined_presets, pictureSize, pictureSize);
            
            CustomizedPresetsAccent?.Dispose();
            CustomizedPresetsAccent = GetSolidImageByBitmapMask(AccentColor, Resources.customized_presets, pictureSize, pictureSize);
            
            CustomizedPresetsDimmed?.Dispose();
            CustomizedPresetsDimmed = GetSolidImageByBitmapMask(GetWeightedColor(AccentColor, FormBackColor, DeepDimmedWeight), 
                Resources.customized_presets, pictureSize, pictureSize);
            
            UserPresetsAccent?.Dispose();
            UserPresetsAccent = GetSolidImageByBitmapMask(AccentColor, Resources.user_presets, pictureSize, pictureSize);
            
            UserPresetsDimmed?.Dispose();
            UserPresetsDimmed = GetSolidImageByBitmapMask(GetWeightedColor(AccentColor, FormBackColor, DeepDimmedWeight), 
                Resources.user_presets, pictureSize, pictureSize);
            
            PlaylistPresetsAccent?.Dispose();
            PlaylistPresetsAccent = GetSolidImageByBitmapMask(AccentColor, Resources.playlist_presets, pictureSize, pictureSize);
            
            PlaylistPresetsDimmed?.Dispose();
            PlaylistPresetsDimmed = GetSolidImageByBitmapMask(GetWeightedColor(AccentColor, FormBackColor, DeepDimmedWeight), 
                Resources.playlist_presets, pictureSize, pictureSize);
            
            FunctionIdPresetsAccent?.Dispose();
            FunctionIdPresetsAccent = GetSolidImageByBitmapMask(AccentColor, Resources.function_id_presets, pictureSize, pictureSize);
            
            FunctionIdPresetsDimmed?.Dispose();
            FunctionIdPresetsDimmed = GetSolidImageByBitmapMask(GetWeightedColor(AccentColor, FormBackColor, DeepDimmedWeight), 
                Resources.function_id_presets, pictureSize, pictureSize);
            
            HotkeyPresetsAccent?.Dispose();
            HotkeyPresetsAccent = GetSolidImageByBitmapMask(AccentColor, Resources.hotkey_presets, pictureSize, pictureSize);
            
            HotkeyPresetsDimmed?.Dispose();
            HotkeyPresetsDimmed = GetSolidImageByBitmapMask(GetWeightedColor(AccentColor, FormBackColor, DeepDimmedWeight), 
                Resources.hotkey_presets, pictureSize, pictureSize);
            
            UncheckAllFiltersAccent?.Dispose();
            UncheckAllFiltersAccent = GetSolidImageByBitmapMask(AccentColor, Resources.uncheck_all_preset_filters, pictureSize, pictureSize);
            
            UncheckAllFiltersDimmed?.Dispose();
            UncheckAllFiltersDimmed = GetSolidImageByBitmapMask(GetWeightedColor(AccentColor, FormBackColor, DeepDimmedWeight), 
                Resources.uncheck_all_preset_filters, pictureSize, pictureSize);
            

            //LR
            var pictogramSize = (int)Math.Round(23f * ButtonHeightDpiFontScaling);

            Window?.Dispose();
            Window = ScaleBitmap(Resources.window, PixelFormat.Format32bppArgb, InterpolationMode.HighQualityBicubic, pictogramSize, pictogramSize);
            

            HeaderCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            //DATAGRIDVIEW COLOR DEFINITIONS
            if (!SavedSettings.dontUseSkinColors)
            {
                HeaderCellStyle.ForeColor = ButtonForeColor;
                HeaderCellStyle.BackColor = DeepDimmedAccentColor;
                HeaderCellStyle.SelectionForeColor = ButtonForeColor;
                HeaderCellStyle.SelectionBackColor = DimmedAccentColor;

                UnchangedCellStyle.ForeColor = InputControlForeColor;
                UnchangedCellStyle.BackColor = InputControlBackColor;
                UnchangedCellStyle.SelectionForeColor = ButtonForeColor;
                UnchangedCellStyle.SelectionBackColor = ButtonBackColor;

                if (GetBrightnessDifference(UnchangedCellStyle.SelectionBackColor, UnchangedCellStyle.BackColor) < 0.25f)
                    UnchangedCellStyle.SelectionBackColor = DeepDimmedAccentColor;

                if (GetBrightnessDifference(HeaderCellStyle.ForeColor, HeaderCellStyle.BackColor) < 0.3f)
                    HeaderCellStyle.ForeColor = InvertAverageBrightness(HeaderCellStyle.ForeColor);

                if (GetBrightnessDifference(HeaderCellStyle.SelectionForeColor, HeaderCellStyle.SelectionBackColor) < 0.3f)
                    HeaderCellStyle.SelectionForeColor = InvertAverageBrightness(HeaderCellStyle.SelectionForeColor);
            }
            else
            {
                UnchangedCellStyle.ForeColor = SystemColors.WindowText;
                UnchangedCellStyle.BackColor = SystemColors.Window;
                UnchangedCellStyle.SelectionForeColor = SystemColors.HighlightText;
                UnchangedCellStyle.SelectionBackColor = SystemColors.Highlight;
            }

            var ChangedForeColor = Color.FromKnownColor(KnownColor.Red);
            var PreservedTagsForeColor = Color.FromKnownColor(KnownColor.Blue);
            var PreservedTagValuesForeColor = Color.FromKnownColor(KnownColor.Green);


            //CHANGED STYLE
            ChangedCellStyle.BackColor = UnchangedCellStyle.BackColor;
            ChangedCellStyle.SelectionBackColor = UnchangedCellStyle.SelectionBackColor;

            var scale = 1.5f;//***
            var shift = 50f;//***
            var threshold = 175f;

            var backColor = ChangedCellStyle.BackColor;
            float br = backColor.R;
            float bg = backColor.G;
            float bb = backColor.B;

            var bbrt = (br + bg + bb) / 3f;

            var foreColor = ChangedForeColor;
            float r = foreColor.R;
            float g = foreColor.G;
            float b = foreColor.B;

            var brt = (r + g + b) / 3f;

            if (Math.Abs(brt - bbrt) < threshold)
            {
                if (brt < 127)
                    r = r * (brt * scale) + shift;
                else
                    r = r / (brt * scale) - shift;

                if (brt < 127)
                    g = g * (brt * scale) + shift;
                else
                    g = g / (brt * scale) - shift;

                if (brt < 127)
                    b = b * (brt * scale) + shift;
                else
                    b = b / (brt * scale) - shift;

                r = r > 255 ? 255 : r;
                r = r < 0 ? 0 : r;

                g = g > 255 ? 255 : g;
                g = g < 0 ? 0 : g;

                b = b > 255 ? 255 : b;
                b = b < 0 ? 0 : b;
            }

            ChangedCellStyle.ForeColor = Color.FromArgb(255, (int)Math.Round(r), (int)Math.Round(g), (int)Math.Round(b));


            //SELECTION CHANGED STYLE
            backColor = ChangedCellStyle.SelectionBackColor;
            br = backColor.R;
            bg = backColor.G;
            bb = backColor.B;

            bbrt = (br + bg + bb) / 3f;

            foreColor = ChangedForeColor;
            r = foreColor.R;
            g = foreColor.G;
            b = foreColor.B;

            brt = (r + g + b) / 3f;

            if (Math.Abs(brt - bbrt) < threshold)
            {
                if (brt < 127)
                    r = r * (brt * scale) + shift;
                else
                    r = r / (brt * scale) - shift;

                if (brt < 127)
                    g = g * (brt * scale) + shift;
                else
                    g = g / (brt * scale) - shift;

                if (brt < 127)
                    b = b * (brt * scale) + shift;
                else
                    b = b / (brt * scale) - shift;

                r = r > 255 ? 255 : r;
                r = r < 0 ? 0 : r;

                g = g > 255 ? 255 : g;
                g = g < 0 ? 0 : g;

                b = b > 255 ? 255 : b;
                b = b < 0 ? 0 : b;
            }

            ChangedCellStyle.SelectionForeColor = Color.FromArgb(255, (int)Math.Round(r), (int)Math.Round(g), (int)Math.Round(b));


            //DIMMED STYLE
            DimmedCellStyle.BackColor = UnchangedCellStyle.BackColor;
            DimmedCellStyle.SelectionBackColor = UnchangedCellStyle.SelectionBackColor;

            scale = 0.2f; //Dimmed text brightness scale //***

            foreColor = UnchangedCellStyle.ForeColor;
            r = foreColor.R;
            g = foreColor.G;
            b = foreColor.B;

            brt = (r + g + b) / 3f;

            if (brt < 127)
            {
                r /= scale;
                g /= scale;
                b /= scale;

            }
            else
            {
                r *= scale;
                g *= scale;
                b *= scale;

            }

            r = r > 255 ? 255 : r;
            r = r < 0 ? 0 : r;

            g = g > 255 ? 255 : g;
            g = g < 0 ? 0 : g;

            b = b > 255 ? 255 : b;
            b = b < 0 ? 0 : b;

            DimmedCellStyle.ForeColor = Color.FromArgb(255, (int)Math.Round(r), (int)Math.Round(g), (int)Math.Round(b));


            //SELECTION DIMMED STYLE
            foreColor = UnchangedCellStyle.SelectionForeColor;
            r = foreColor.R;
            g = foreColor.G;
            b = foreColor.B;

            brt = (r + g + b) / 3f;

            if (brt < 127)
            {
                r /= scale;
                g /= scale;
                b /= scale;

            }
            else
            {
                r *= scale;
                g *= scale;
                b *= scale;

            }

            r = r > 255 ? 255 : r;
            r = r < 0 ? 0 : r;

            g = g > 255 ? 255 : g;
            g = g < 0 ? 0 : g;

            b = b > 255 ? 255 : b;
            b = b < 0 ? 0 : b;

            DimmedCellStyle.SelectionForeColor = Color.FromArgb(255, (int)Math.Round(r), (int)Math.Round(g), (int)Math.Round(b));


            //PRESERVED TAG STYLE
            PreservedTagCellStyle.BackColor = UnchangedCellStyle.BackColor;
            PreservedTagCellStyle.SelectionBackColor = UnchangedCellStyle.SelectionBackColor;

            scale = 1.5f;//***

            backColor = UnchangedCellStyle.BackColor;
            br = backColor.R;
            bg = backColor.G;
            bb = backColor.B;

            bbrt = (br + bg + bb) / 3f;

            foreColor = PreservedTagsForeColor;
            r = foreColor.R;
            g = foreColor.G;
            b = foreColor.B;

            brt = (r + g + b) / 3f;

            if (Math.Abs(brt - bbrt) < threshold)
            {
                if (brt < 127)
                    r = r * (brt * scale) + shift;
                else
                    r = r / (brt * scale) - shift;

                if (brt < 127)
                    g = g * (brt * scale) + shift;
                else
                    g = g / (brt * scale) - shift;

                if (brt < 127)
                    b = b * (brt * scale) + shift;
                else
                    b = b / (brt * scale) - shift;

                r = r > 255 ? 255 : r;
                r = r < 0 ? 0 : r;

                g = g > 255 ? 255 : g;
                g = g < 0 ? 0 : g;

                b = b > 255 ? 255 : b;
                b = b < 0 ? 0 : b;
            }

            PreservedTagCellStyle.ForeColor = Color.FromArgb(255, (int)Math.Round(r), (int)Math.Round(g), (int)Math.Round(b));


            //SELECTION PRESERVED TAG STYLE
            backColor = UnchangedCellStyle.SelectionBackColor;
            br = backColor.R;
            bg = backColor.G;
            bb = backColor.B;

            bbrt = (br + bg + bb) / 3f;

            foreColor = PreservedTagsForeColor;
            r = foreColor.R;
            g = foreColor.G;
            b = foreColor.B;

            brt = (r + g + b) / 3f;

            if (Math.Abs(brt - bbrt) < threshold)
            {
                if (brt < 127)
                    r = r * (brt * scale) + shift;
                else
                    r = r / (brt * scale) - shift;

                if (brt < 127)
                    g = g * (brt * scale) + shift;
                else
                    g = g / (brt * scale) - shift;

                if (brt < 127)
                    b = b * (brt * scale) + shift;
                else
                    b = b / (brt * scale) - shift;

                r = r > 255 ? 255 : r;
                r = r < 0 ? 0 : r;

                g = g > 255 ? 255 : g;
                g = g < 0 ? 0 : g;

                b = b > 255 ? 255 : b;
                b = b < 0 ? 0 : b;
            }

            PreservedTagCellStyle.SelectionForeColor = Color.FromArgb(255, (int)Math.Round(r), (int)Math.Round(g), (int)Math.Round(b));


            //PRESERVED TAG VALUE STYLE
            PreservedTagValueCellStyle.BackColor = UnchangedCellStyle.BackColor;
            PreservedTagValueCellStyle.SelectionBackColor = UnchangedCellStyle.SelectionBackColor;

            scale = 1.5f;//***

            backColor = UnchangedCellStyle.BackColor;
            br = backColor.R;
            bg = backColor.G;
            bb = backColor.B;

            bbrt = (br + bg + bb) / 3f;

            foreColor = PreservedTagValuesForeColor;
            r = foreColor.R;
            g = foreColor.G;
            b = foreColor.B;

            brt = (r + g + b) / 3f;

            if (Math.Abs(brt - bbrt) < threshold)
            {
                if (brt < 127)
                    r = r * (brt * scale) + shift;
                else
                    r = r / (brt * scale) - shift;

                if (brt < 127)
                    g = g * (brt * scale) + shift;
                else
                    g = g / (brt * scale) - shift;

                if (brt < 127)
                    b = b * (brt * scale) + shift;
                else
                    b = b / (brt * scale) - shift;

                r = r > 255 ? 255 : r;
                r = r < 0 ? 0 : r;

                g = g > 255 ? 255 : g;
                g = g < 0 ? 0 : g;

                b = b > 255 ? 255 : b;
                b = b < 0 ? 0 : b;
            }

            PreservedTagValueCellStyle.ForeColor = Color.FromArgb(255, (int)Math.Round(r), (int)Math.Round(g), (int)Math.Round(b));


            //SELECTION PRESERVED TAG STYLE
            backColor = UnchangedCellStyle.SelectionBackColor;
            br = backColor.R;
            bg = backColor.G;
            bb = backColor.B;

            bbrt = (br + bg + bb) / 3f;

            foreColor = PreservedTagValuesForeColor;
            r = foreColor.R;
            g = foreColor.G;
            b = foreColor.B;

            brt = (r + g + b) / 3f;

            if (Math.Abs(brt - bbrt) < threshold)
            {
                if (brt < 127)
                    r = r * (brt * scale) + shift;
                else
                    r = r / (brt * scale) - shift;

                if (brt < 127)
                    g = g * (brt * scale) + shift;
                else
                    g = g / (brt * scale) - shift;

                if (brt < 127)
                    b = b * (brt * scale) + shift;
                else
                    b = b / (brt * scale) - shift;

                r = r > 255 ? 255 : r;
                r = r < 0 ? 0 : r;

                g = g > 255 ? 255 : g;
                g = g < 0 ? 0 : g;

                b = b > 255 ? 255 : b;
                b = b < 0 ? 0 : b;
            }

            PreservedTagValueCellStyle.SelectionForeColor = Color.FromArgb(255, (int)Math.Round(r), (int)Math.Round(g), (int)Math.Round(b));


            SizesColorsChanged = false;
        }

        internal static ToolStripMenuItem AddMenuItem(ToolStripMenuItem menuItemGroup, string itemName, string hotkeyDescription, EventHandler handler, bool enabled = true, Form form = null)
        {
            if (hotkeyDescription != null)
                MbApiInterface.MB_RegisterCommand(hotkeyDescription, handler);

            var menuItem = menuItemGroup.DropDown.Items.Add(itemName, null, handler);
            menuItem.Enabled = enabled;
            menuItem.Tag = form;

            if (itemName == "-")
                return null;
            else
                return (ToolStripMenuItem)menuItem;
        }

        internal void addPluginMenuItems() //Must be called AFTER InitLr() and InitAsr(), addPluginContextMenuItems()!
        {
            TagToolsSubmenu.DropDown.Items.Clear();
            TagToolsSubmenu.DropDown.ShowItemToolTips = true;

            OpenedFormsSubmenu = AddMenuItem(TagToolsSubmenu, OpenWindowsMenuSectionName, null, null);
            MbApiInterface.MB_RegisterCommand(ShowHiddenWindowsDescription, showHiddenEventHandler);

            AddMenuItem(TagToolsSubmenu, "-", null, null);
            AddMenuItem(TagToolsSubmenu, TagToolsMenuSectionName, null, null, false);

            if (!SavedSettings.dontShowCopyTag) AddMenuItem(TagToolsSubmenu, CopyTagName, CopyTagDescription, copyTagEventHandler);
            if (!SavedSettings.dontShowSwapTags) AddMenuItem(TagToolsSubmenu, SwapTagsName, SwapTagsDescription, swapTagsEventHandler);
            if (!SavedSettings.dontShowChangeCase) AddMenuItem(TagToolsSubmenu, ChangeCaseName, ChangeCaseDescription, changeCaseEventHandler);
            if (!SavedSettings.dontShowReEncodeTag)
            {
                AddMenuItem(TagToolsSubmenu, ReEncodeTagName, ReEncodeTagDescription, reencodeTagEventHandler);
                AddMenuItem(TagToolsSubmenu, ReEncodeTagsName, ReEncodeTagsDescription, reencodeTagsEventHandler);
            }
            if (!SavedSettings.dontShowLibraryReports)
            {
                AddMenuItem(TagToolsSubmenu, LibraryReportsName, LibraryReportsDescription, libraryReportsEventHandler);
                if (LrPresetsWithHotkeysCount > 0)
                {
                    LrPresetsMenuItem = AddMenuItem(TagToolsSubmenu, LibraryReportsName.Replace("...", string.Empty), null, null);
                    RegisterLrPresetsHotkeysAndMenuItems(this);
                }
            }
            if (!SavedSettings.dontShowAutoRate) AddMenuItem(TagToolsSubmenu, AutoRateName, AutoRateDescription, autoRateEventHandler);
            if (!SavedSettings.dontShowAsr)
            {
                AddMenuItem(TagToolsSubmenu, AsrName, AsrDescription, asrEventHandler);
                if (AsrPresetsWithHotkeysCount > 0)
                {
                    AsrPresetsMenuItem = AddMenuItem(TagToolsSubmenu, AsrName.Replace("...", string.Empty), null, null);
                    RegisterAsrPresetsHotkeysAndMenuItems(this);
                }
                AddMenuItem(TagToolsSubmenu, MsrName, MsrCommandDescription, multipleSearchReplaceEventHandler);
            }
            if (!SavedSettings.dontShowCAR) AddMenuItem(TagToolsSubmenu, CarName, CarDescription, carEventHandler);
            if (!SavedSettings.dontShowCT) AddMenuItem(TagToolsSubmenu, CompareTracksName, CompareTracksDescription, compareTracksEventHandler);


            AddMenuItem(TagToolsSubmenu, "-", null, null);
            AddMenuItem(TagToolsSubmenu, CopyTagsToClipboardName, CopyTagsToClipboardDescription, copyTagsToClipboardEventHandler);
            CopyTagsToClipboardUsingMenuItem = AddMenuItem(TagToolsSubmenu, CopyTagsToClipboardUsingMenuDescription, null, null);
            addCopyTagsToClipboardUsingMenuItems();

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

            AddMenuItem(TagToolsSubmenu, PasteTagsFromClipboardName, PasteTagsFromClipboardDescription, pasteTagsFromClipboardEventHandler);


            if (!SavedSettings.dontShowBackupRestore)
            {
                AddMenuItem(TagToolsSubmenu, "-", null, null);

                var backupRestoreSubenu = AddMenuItem(TagToolsSubmenu, BackupRestoreMenuSectionName, null, null);
                AddMenuItem(backupRestoreSubenu, TagHistoryName, TagHistoryDescription, tagHistoryEventHandler); //-V3080
                AddMenuItem(backupRestoreSubenu, "-", null, null);
                AddMenuItem(backupRestoreSubenu, BackupTagsName, BackupTagsDescription, backupTagsEventHandler);
                AddMenuItem(backupRestoreSubenu, RestoreTagsName, RestoreTagsDescription, restoreTagsEventHandler);
                AddMenuItem(backupRestoreSubenu, RestoreTagsForEntireLibraryName, RestoreTagsForEntireLibraryDescription, restoreTagsForEntireLibraryEventHandler);
                AddMenuItem(backupRestoreSubenu, RenameMoveBackupName, RenameMoveBackupDescription, renameMoveBackupEventHandler);
                AddMenuItem(backupRestoreSubenu, MoveBackupsName, MoveBackupsDescription, moveBackupsEventHandler);
                AddMenuItem(backupRestoreSubenu, CreateNewBaselineName, CreateNewBaselineDescription, createNewBaselineEventHandler);
                AddMenuItem(backupRestoreSubenu, createEmptyBaselineRestoreTagsForEntireLibraryName, createEmptyBaselineRestoreTagsForEntireLibraryDescription, createEmptyBaselineRestoreTagsForEntireLibraryEventHandler);
                AddMenuItem(backupRestoreSubenu, DeleteBackupsName, DeleteBackupsDescription, deleteBackupsEventHandler);
                AddMenuItem(backupRestoreSubenu, "-", null, null);
                AddMenuItem(backupRestoreSubenu, AutoBackupSettingsName, AutoBackupSettingsDescription, autoBackupSettingsEventHandler);
            }

            AddMenuItem(TagToolsSubmenu, "-", null, null);
            AddMenuItem(TagToolsSubmenu, PluginHelpString, null, helpEventHandler);
            AddMenuItem(TagToolsSubmenu, PluginWebPageString, null, webPageEventHandler).ToolTipText = PluginWebPageToolTip;
            AddMenuItem(TagToolsSubmenu, PluginVersion, null, null, false);
        }

        internal void addPluginContextMenuItems() //Must be called AFTER InitLr() and InitAsr(), and BEFORE addPluginMenuItems()!
        {
            if (TagToolsContextSubmenu == null)
                return;


            TagToolsContextSubmenu.DropDown.Items.Clear();

            if (!SavedSettings.dontShowBackupRestore) AddMenuItem(TagToolsContextSubmenu, TagToolsMenuSectionName, null, null, false);

            if (!SavedSettings.dontShowCopyTag) AddMenuItem(TagToolsContextSubmenu, CopyTagName, null, copyTagEventHandler);
            if (!SavedSettings.dontShowSwapTags) AddMenuItem(TagToolsContextSubmenu, SwapTagsName, null, swapTagsEventHandler);
            if (!SavedSettings.dontShowChangeCase) AddMenuItem(TagToolsContextSubmenu, ChangeCaseName, null, changeCaseEventHandler);
            if (!SavedSettings.dontShowLibraryReports)
            {
                AddMenuItem(TagToolsSubmenu, LibraryReportsName, LibraryReportsDescription, libraryReportsEventHandler);
                if (LrPresetsWithHotkeysCount > 0)
                    LrPresetsContextMenuItem = AddMenuItem(TagToolsContextSubmenu, LibraryReportsName.Replace("...", string.Empty), null, null);
            }
            if (!SavedSettings.dontShowAsr)
            {
                //AddMenuItem(TagToolsContextSubmenu, AsrCommandName, null, asrEventHandler);
                if (AsrPresetsWithHotkeysCount > 0)
                    AsrPresetsContextMenuItem = AddMenuItem(TagToolsContextSubmenu, AsrName.Replace("...", string.Empty), null, null);
                AddMenuItem(TagToolsContextSubmenu, MsrName, null, multipleSearchReplaceEventHandler);
            }
            if (!SavedSettings.dontShowCT) AddMenuItem(TagToolsContextSubmenu, CompareTracksName, null, compareTracksEventHandler);
            AddMenuItem(TagToolsContextSubmenu, "-", null, null);
            AddMenuItem(TagToolsContextSubmenu, CopyTagsToClipboardName, null, copyTagsToClipboardEventHandler);
            CopyTagsToClipboardUsingContextMenuItem = AddMenuItem(TagToolsContextSubmenu, CopyTagsToClipboardUsingMenuDescription, null, null);
            addCopyTagsToClipboardUsingContextMenuItems();
            AddMenuItem(TagToolsContextSubmenu, PasteTagsFromClipboardName, null, pasteTagsFromClipboardEventHandler);

            if (!SavedSettings.dontShowBackupRestore)
            {
                AddMenuItem(TagToolsContextSubmenu, "-", null, null);
                AddMenuItem(TagToolsContextSubmenu, BackupRestoreMenuSectionName, null, null, false);
                AddMenuItem(TagToolsContextSubmenu, TagHistoryName, null, tagHistoryEventHandler);
            }


            if (!SavedSettings.dontShowShowHiddenWindows)
            {
                MbApiInterface.MB_RegisterCommand(ShowHiddenWindowsName, showHiddenEventHandler);
            }
        }

        internal void addCopyTagsToClipboardUsingMenuItems()
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

        internal void addCopyTagsToClipboardUsingContextMenuItems()
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
        #endregion

        #region Plugin's additional virtual tag functions
        public string CustomFunc_ASR(string url, string presetId)
        {
            if (SavedSettings.dontShowAsr)
                return "ASR is disabled!";

            if (presetId == string.Empty)
                return string.Empty;

            return GetLastReplacedTag(url, IdsAsrPresets[presetId]) ?? string.Empty;
        }

        public string CustomFunc_LR(string url, string functionId)
        {
            if (SavedSettings.dontShowLibraryReports)
                return "LR is disabled!";

            return AutoCalculateReportPresetFunction(url, functionId) ?? "<null>";
        }

        public string CustomFunc_Random(string max_number)
        {
            try
            {
                return RandomGenerator.Next(int.Parse(max_number) + 1).ToString("D" + Math.Ceiling((decimal)Math.Log10(float.Parse(max_number) + 1)));
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CustomFunc_Round(string number, string number_of_digits)
        {
            try
            { 
                number = number.Replace('.', LocalizedDecimalPoint);
                number_of_digits = number_of_digits.Replace('.', LocalizedDecimalPoint);

                if (int.Parse(number_of_digits) > 0)
                    return Math.Round(decimal.Parse(number), int.Parse(number_of_digits)).ToString("F" + number_of_digits);
                else
                    return Math.Round(decimal.Parse(number), int.Parse(number_of_digits)).ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CustomFunc_RoundUp(string number, string number_of_digits)
        {
            try
            {
                number = number.Replace('.', LocalizedDecimalPoint);
                number_of_digits = number_of_digits.Replace('.', LocalizedDecimalPoint);

                if (int.Parse(number_of_digits) > 0)
                    return (Math.Ceiling(decimal.Parse(number) * (decimal)Math.Pow(10, int.Parse(number_of_digits))) / (decimal)Math.Pow(10, int.Parse(number_of_digits))).ToString();
                else
                    return Math.Ceiling(decimal.Parse(number)).ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CustomFunc_RoundDown(string number, string number_of_digits)
        {
            try
            {
                number = number.Replace('.', LocalizedDecimalPoint);
                number_of_digits = number_of_digits.Replace('.', LocalizedDecimalPoint);

                if (int.Parse(number_of_digits) > 0)
                    return (Math.Floor(decimal.Parse(number) * (decimal)Math.Pow(10, int.Parse(number_of_digits))) / (decimal)Math.Pow(10, int.Parse(number_of_digits))).ToString();
                else
                    return Math.Floor(decimal.Parse(number)).ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CustomFunc_Sqrt(string number)
        {
            try
            {
                number = number.Replace('.', LocalizedDecimalPoint);

                return Math.Sqrt(float.Parse(number)).ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CustomFunc_Log(string number)
        {
            try
            {
                number = number.Replace('.', LocalizedDecimalPoint);

                return Math.Log10(double.Parse(number)).ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CustomFunc_eq(string number1, string number2)
        {
            try
            {
                number1 = number1.Replace('.', LocalizedDecimalPoint);
                number2 = number2.Replace('.', LocalizedDecimalPoint);

                if (decimal.Parse(number1) == decimal.Parse(number2))
                    return "T";
                else
                    return "F";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CustomFunc_ne(string number1, string number2)
        {
            try
            {
                number1 = number1.Replace('.', LocalizedDecimalPoint);
                number2 = number2.Replace('.', LocalizedDecimalPoint);

                if (decimal.Parse(number1) != decimal.Parse(number2))
                    return "T";
                else
                    return "F";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public string CustomFunc_gt(string number1, string number2)
        {
            try
            {
                number1 = number1.Replace('.', LocalizedDecimalPoint);
                number2 = number2.Replace('.', LocalizedDecimalPoint);

                if (decimal.Parse(number1) > decimal.Parse(number2))
                    return "T";
                else
                    return "F";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CustomFunc_lt(string number1, string number2)
        {
            try
            {
                number1 = number1.Replace('.', LocalizedDecimalPoint);
                number2 = number2.Replace('.', LocalizedDecimalPoint);

                if (decimal.Parse(number1) < decimal.Parse(number2))
                    return "T";
                else
                    return "F";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CustomFunc_ge(string number1, string number2)
        {
            try
            {
                number1 = number1.Replace('.', LocalizedDecimalPoint);
                number2 = number2.Replace('.', LocalizedDecimalPoint);

                if (decimal.Parse(number1) >= decimal.Parse(number2))
                    return "T";
                else
                    return "F";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CustomFunc_le(string number1, string number2)
        {
            try
            {
                number1 = number1.Replace('.', LocalizedDecimalPoint);
                number2 = number2.Replace('.', LocalizedDecimalPoint);

                if (decimal.Parse(number1) <= decimal.Parse(number2))
                    return "T";
                else
                    return "F";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private string removeLeadingZerosFromDuration(string duration)
        {
            if (duration.Contains('.'))
                return duration;

            if (duration.Length > 6) //e.g. > :01:22
                duration = duration.TrimStart('0');

            if (duration.StartsWith(":"))
                duration = duration.TrimStart(':');
            else
                return duration;

            duration = duration.TrimStart('0');

            if (duration.StartsWith(":")) //e.g. :12
                return "0" + duration;
            else
                return duration;
        }

        public string CustomFunc_AddDuration(string duration1, string duration2)
        {
            try
            {
                return removeLeadingZerosFromDuration((TimeSpan.Parse(duration1) + TimeSpan.Parse(duration2)).ToString());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CustomFunc_SubDuration(string duration1, string duration2)
        {
            try
            {
                return removeLeadingZerosFromDuration((TimeSpan.Parse(duration1) - TimeSpan.Parse(duration2)).ToString());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CustomFunc_MulDuration(string duration, string number)
        {
            try
            {
                return removeLeadingZerosFromDuration(TimeSpan.FromMilliseconds(Math.Round(double.Parse(number) * TimeSpan.Parse(duration).TotalMilliseconds)).ToString());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CustomFunc_SubDateTime(string datetime1, string datetime2)
        {
            try
            {
                return removeLeadingZerosFromDuration((DateTime.Parse(datetime1) - DateTime.Parse(datetime2)).ToString());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CustomFunc_NumberOfDays(string datetime1, string datetime2)
        {
            try
            {
                if (datetime2.Length == 4)
                    datetime2 = (new DateTime(int.Parse(datetime2), 01, 01)).ToString();

                return Math.Floor((DateTime.Parse(datetime1) - DateTime.Parse(datetime2)).TotalDays).ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CustomFunc_AddDurationToDateTime(string datetime, string duration)
        {
            try
            {
                return (DateTime.Parse(datetime) + TimeSpan.Parse(duration)).ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CustomFunc_SubDurationFromDateTime(string datetime, string duration)
        {
            try
            {
                return (DateTime.Parse(datetime) - TimeSpan.Parse(duration)).ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CustomFunc_Seconds(string duration)
        {
            try
            {
                duration = Regex.Replace(duration, @"^(\d+)$", "00:00:$1");
                duration = Regex.Replace(duration, @"^(\d+:\d+)$", "00:$1");
                return TimeSpan.Parse(duration).TotalSeconds.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CustomFunc_DurationFromSeconds(string seconds)
        {
            try
            {
                string duration = TimeSpan.FromSeconds(double.Parse(seconds)).ToString();
                return removeLeadingZerosFromDuration(duration);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CustomFunc_Now()
        {
            return DateTime.Now.ToString();
        }

        public string CustomFunc_TitleCase(string input, string lowerCaseWordsString, string upperCaseWordsString, //******* abbreviations!!!!
            string openingExceptionCharsString, string closingExceptionCharsString, string exceptionCharsString)
        {
            try
            {
                string[] lowerCaseWords = null;
                string[] upperCaseWords = null;
                string[] openingExceptionChars = null;
                string[] closingExceptionChars = null;
                string[] exceptionChars = null;


                if (lowerCaseWordsString == "`")
                    lowerCaseWordsString = null;

                if (upperCaseWordsString == "`")
                    upperCaseWordsString = null;

                if (openingExceptionCharsString == "`")
                    openingExceptionCharsString = null;

                if (closingExceptionCharsString == "`")
                    closingExceptionCharsString = null;


                if (!ChangeCase.CheckIfTheSameNumberOfCharsInStrings(openingExceptionCharsString, closingExceptionCharsString))
                    return CtlLrError;


                if (!string.IsNullOrWhiteSpace(lowerCaseWordsString))
                    lowerCaseWords = lowerCaseWordsString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (!string.IsNullOrWhiteSpace(upperCaseWordsString))
                    upperCaseWords = upperCaseWordsString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (!string.IsNullOrWhiteSpace(openingExceptionCharsString))
                    openingExceptionChars = ChangeCase.GetCharsInString(openingExceptionCharsString);

                if (!string.IsNullOrWhiteSpace(closingExceptionCharsString))
                    closingExceptionChars = ChangeCase.GetCharsInString(closingExceptionCharsString);

                if (!string.IsNullOrWhiteSpace(exceptionCharsString))
                    exceptionChars = ChangeCase.GetCharsInString(exceptionCharsString);


                string[] exceptedWords = null;

                if (lowerCaseWords != null && upperCaseWords == null)
                    exceptedWords = lowerCaseWords;
                else if (lowerCaseWords == null && upperCaseWords != null)
                    exceptedWords = upperCaseWords;
                else if (lowerCaseWords != null && upperCaseWords != null)
                    exceptedWords = lowerCaseWords.Union(upperCaseWords).ToArray();


                input = ChangeCase.ChangeWordsCase(input, ChangeCase.ChangeCaseOptions.LowerCase);

                input = ChangeCase.ChangeSentenceCase(input, ChangeCase.ChangeCaseOptions.TitleCase, null, false,
                    null, openingExceptionChars, closingExceptionChars, exceptionChars, true, true);

                input = ChangeCase.ChangeSentenceCase(input, ChangeCase.ChangeCaseOptions.LowerCase, lowerCaseWords, true,
                    null, openingExceptionChars, closingExceptionChars, exceptionChars, null, null, true);


                List<char> encounteredLeftExceptionChars = null;
                bool wasCharException = false;

                var result = ChangeCase.ChangeWordsCase(input, ChangeCase.ChangeCaseOptions.UpperCase, ref encounteredLeftExceptionChars, ref wasCharException, upperCaseWords, true,
                    null, null, null, null, null);

                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CustomFunc_SentenceCase(string input, string upperCaseWordsString, string sentenceSeparatorsString) //******* abbreviations!!!!
        {
            try
            {
                string[] upperCaseWords = null;
                string[] sentenceSeparators = null;

                if (upperCaseWordsString == "`")
                    upperCaseWordsString = null;

                if (!string.IsNullOrWhiteSpace(upperCaseWordsString))
                    upperCaseWords = upperCaseWordsString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (!string.IsNullOrWhiteSpace(sentenceSeparatorsString))
                    sentenceSeparators = ChangeCase.GetCharsInString(sentenceSeparatorsString);

                input = ChangeCase.ChangeWordsCase(input, ChangeCase.ChangeCaseOptions.UpperCase);

                List<char> encounteredLeftExceptionChars = null;
                bool wasCharException = false;

                input = ChangeCase.ChangeWordsCase(input, ChangeCase.ChangeCaseOptions.LowerCase, ref encounteredLeftExceptionChars, ref wasCharException, upperCaseWords);

                var result = ChangeCase.ChangeSentenceCase(input, ChangeCase.ChangeCaseOptions.SentenceCase, upperCaseWords, false, null, null, null, sentenceSeparators,
                    true, false, true);

                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CustomFunc_Name(string parameter)
        {
            return Regex.Replace(parameter, @"^(.*\\)?(.*)\..*", "$2");
        }

        public string CustomFunc_DateCreated(string url)
        {
            var fileInfo = new FileInfo(url);
            if (fileInfo.Exists)
                return fileInfo.CreationTime.ToString("d");
            else
                return string.Empty;
        }

        public string CustomFunc_Char(string code)
        {
            try
            {
                var charcode = ushort.Parse(code, System.Globalization.NumberStyles.HexNumber);
                return ((char)charcode).ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CustomFunc_CharN(string code, string timesString)
        {
            try
            {
                var charcode = ushort.Parse(code, System.Globalization.NumberStyles.HexNumber);
                var character = ((char)charcode).ToString();

                var sequence = string.Empty;

                var times = int.Parse(timesString);
                for (var i = 0; i < times; i++)
                    sequence += character;

                return sequence;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CustomFunc_TagContainsAnyString(string url, string tagName, string text)
        {
            var tagId = GetTagId(tagName);

            var tagValue = GetFileTag(url, tagId);
            var textParts = text.Split('|');

            foreach (var textPart in textParts)
            {
                var escTextPart = Regex.Escape(textPart);

                if (Regex.IsMatch(tagValue, escTextPart, RegexOptions.IgnoreCase))
                    return "T";
            }

            return "F";
        }

        public string CustomFunc_TagContainsAllStrings(string url, string tagName, string text)
        {
            var tagId = GetTagId(tagName);

            var tagValue = GetFileTag(url, tagId);
            var textParts = text.Split('|');

            var result = "T";
            foreach (var textPart in textParts)
            {
                var escTextPart = Regex.Escape(textPart);

                if (!Regex.IsMatch(tagValue, escTextPart, RegexOptions.IgnoreCase))
                    return "F";
            }

            return "T";
        }
        #endregion
    }
    #endregion
}
