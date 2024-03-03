using ExtensionMethods;
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
        HtmlDocumentCdBooklet = 7
    }
    #endregion

    #region Main module
    public partial class Plugin
    {
        #region Members
        internal static bool DeveloperMode = false;
        private static bool Uninstalled = false;

        internal static MusicBeeApiInterface MbApiInterface;
        private static readonly PluginInfo About = new PluginInfo();

        internal static string PluginVersion;

        internal static ToolStripMenuItem OpenedFormsSubmenu;

        //Skinning
        internal static bool PluginClosing = false;//***

        internal static Color FormForeColor;
        internal static Color FormBackColor;
        internal static Color FormBorderColor;

        internal static Color ControlHighlightForeColor;
        internal static Color ControlHighlightBackColor;

        internal static Color ButtonFocusedBorderColor;
        internal static Color ButtonBorderColor;

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

        internal static Color InputControlDimmedForeColor;
        internal static Color InputControlDimmedBackColor;

        internal static Color AccentColor;
        internal static Color AccentSelectedColor;
        internal static Color DimmedAccentColor;
        internal static Color DeepDimmedAccentColor;

        internal static Color DimmedHighlight;

        internal static Color SplitterColor;


        internal static readonly Color NoColor = Color.FromArgb(-1);

        internal static Color ScrollBarBackColor = NoColor;
        internal static Color ScrollBarBorderColor = NoColor;
        internal static Color NarrowScrollBarBackColor = NoColor;

        internal static Color ScrollBarThumbAndSpansForeColor = NoColor;
        internal static Color ScrollBarThumbAndSpansBackColor = NoColor;
        internal static Color ScrollBarThumbAndSpansBorderColor = NoColor;


        //Themed colors for ASR/LR
        internal static Color HighlightColor;

        internal static Color UntickedColor;
        internal static Color TickedColor;


        //Themed bitmaps

        //Dictionary below is intended for disposing unused ones 
        private static Dictionary<Form, List<Bitmap>> FormsThemedBitmaps = new Dictionary<Form, List<Bitmap>>();
        private static Dictionary<Bitmap, int> ThemedBitmapCounts = new Dictionary<Bitmap, int>();

        internal static Bitmap ThemedBitmapAddRef(Form form, Image oldImage, Bitmap newBitmap)
        {
            if (newBitmap == null)
                return (Bitmap)oldImage;


            List<Bitmap> bitmapList = null;
            if (newBitmap != Resources.transparent_15)
            {
                if (FormsThemedBitmaps.TryGetValue(form, out bitmapList))
                {
                    bitmapList.Add(newBitmap);
                    FormsThemedBitmaps.AddReplace(form, bitmapList);
                }
                else
                {
                    bitmapList = new List<Bitmap>();
                    bitmapList.Add(newBitmap);

                    FormsThemedBitmaps.Add(form, bitmapList);
                }


                ThemedBitmapCounts.TryGetValue(newBitmap, out var count);
                ThemedBitmapCounts.AddReplace(newBitmap, ++count);
            }

            if (oldImage != null && oldImage != Resources.transparent_15)
            {
                if (bitmapList != null || FormsThemedBitmaps.TryGetValue(form, out bitmapList))
                    FormThemedBitmapRelease(bitmapList, (Bitmap)oldImage);
            }

            return newBitmap;
        }

        internal static void FormThemedBitmapRelease(List<Bitmap> bitmapList, Bitmap bitmap)
        {
            if (bitmapList.Contains(bitmap))
            {
                ThemedBitmapCounts.TryGetValue(bitmap, out var count);
                if (--count == 0)
                {
                    ThemedBitmapCounts.Remove(bitmap);
                    bitmap.Dispose();
                }
                else
                {
                    ThemedBitmapCounts.AddReplace(bitmap, count);
                }
            }
        }

        internal static void FormThemedBitmapsRelease(Form form)
        {
            if (!FormsThemedBitmaps.TryGetValue(form, out var bitmapList))
                //"form" is one of setting windows. This function is called artificially on saving settings, not from PluginWindowTemplate_FormClosed() handler.
                return;


            foreach (var bitmap in bitmapList)
                FormThemedBitmapRelease(bitmapList, bitmap);

            FormsThemedBitmaps.Remove(form);
        }


        internal const float ScrollBarWidth = 10.6f;//Not yet DPI scaled
        internal static int SbBorderWidth = 1; // _scaledPx;


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


        internal static Bitmap ButtonRemoveImage;
        internal static Bitmap ButtonSetImage;

        internal static Bitmap CheckedState;
        internal static Bitmap UncheckedState;

        internal static Bitmap WarningWide;
        internal static Bitmap Warning;

        internal static Bitmap Search;
        internal static Bitmap Window;

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



        internal const int MaximumNumberOfASRHotkeys = 20;
        internal const int MaximumNumberOfLRHotkeys = 20;
        internal const int PredefinedReportPresetCount = 4;

        internal static readonly char LocalizedDecimalPoint = (0.5).ToString()[1];
        internal static Bitmap MissingArtwork;
        private static readonly Random RandomGenerator = new Random();

        internal static Form MbForm;
        internal static List<PluginWindowTemplate> OpenedForms = new List<PluginWindowTemplate>();
        internal static int NumberOfNativeMbBackgroundTasks = 0;

        private static bool PrioritySet = false; //MusicBeePlugin's background thread priority must be set to "lower"

        internal static int NumberOfTagChanges = 0;

        internal static bool DisablePlaySoundOnce = false;

        internal static bool BackupIsAlwaysNeeded = true;
        internal static SortedDictionary<int, bool> TracksNeedsToBeBackedUp = new SortedDictionary<int, bool>();
        internal static SortedDictionary<int, bool> TempTracksNeedsToBeBackedUp = new SortedDictionary<int, bool>();
        internal static int UpdatedTracksForBackupCount = 0;
        internal const int MaxUpdatedTracksCount = 5000;
        internal static string MusicName;

        private static string LastMessage = string.Empty;

        internal static SortedDictionary<string, MetaDataType> TagNamesIds = new SortedDictionary<string, MetaDataType>();
        internal static Dictionary<MetaDataType, string> TagIdsNames = new Dictionary<MetaDataType, string>();

        internal static SortedDictionary<string, FilePropertyType> PropNamesIds = new SortedDictionary<string, FilePropertyType>();
        internal static Dictionary<FilePropertyType, string> PropIdsNames = new Dictionary<FilePropertyType, string>();

        private delegate void AutoApplyDelegate(object currentFileObj, object tagToolsPluginObj);
        private readonly AutoApplyDelegate autoApplyDelegate = AsrAutoApplyPresets;

        private static readonly List<string> FilesUpdatedByPlugin = new List<string>();
        private static readonly List<string> ChangedFiles = new List<string>();

        private static System.Threading.Timer PeriodicUI_RefreshTimer = null;
        private static System.Threading.Timer DelayedStatusbarTextClearingTimer = null;
        private static TimerCallback DelayedStatusbarTextClearingDelegate;
        private static bool UiRefreshingIsNeeded = false;
        private static TimeSpan RefreshUI_Delay = new TimeSpan(0, 0, 5);
        internal static DateTime LastUI_Refresh = DateTime.MinValue;
        private static readonly object LastUI_RefreshLocker = new object();

        internal static System.Threading.Timer PeriodicAutobackupTimer = null;
        internal static object AutobackupLocker = new object();

        internal static Button EmptyButton;
        internal static Form EmptyForm;

        internal static string[] ReadonlyTagsNames;

        internal static string Language;
        internal static string PluginsPath;

        private static string PluginSettingsFileName = "mb_TagTools.settings.xml";
        private static string PluginSettingsFilePath;

        private static string PluginHelpFileName = "Additional-Tagging-Tools";
        private static string PluginHelpFilePath;

        internal const string BackupIndexFileName = ".Master Tag Index.mbi";
        internal static string BackupDefaultPrefix = "Tag Backup ";
        internal static BackupIndex BackupIndex;

        internal const int StatusbarTextUpdateInterval = 50;
        internal static char[] FilesSeparators = { '\0' };

        internal static List<Preset> AsrAutoAppliedPresets = new List<Preset>();
        internal const string AsrPresetsDirectory = "ASR Presets";
        internal const string ASRPresetExtension = ".asr-preset.xml";
        internal static string ASRPresetNaming = "ASR preset";
        internal static Preset[] AsrPresetsWithHotkeys = new Preset[MaximumNumberOfASRHotkeys];
        internal static int ASRPresetsWithHotkeysCount = 0;

        internal static SortedDictionary<string, Preset> IdsAsrPresets = new SortedDictionary<string, Preset>();
        internal static Preset MSR;

        internal static LibraryReports LibraryReportsCommandForAutoApplying = new LibraryReports();

        internal static LibraryReports LibraryReportsCommandForHotkeys = new LibraryReports();
        internal static ReportPreset[] ReportPresetsWithHotkeys = new ReportPreset[MaximumNumberOfLRHotkeys];
        internal static int LRPresetsWithHotkeysCount = 0;
        internal static LibraryReports LibraryReportsCommandForFunctionIds = new LibraryReports();
        internal static SortedDictionary<string, ReportPreset> IdsReportPresets = new SortedDictionary<string, ReportPreset>();
        internal static bool ReportPresetIdsAreInitialized = false;

        internal const char MultipleItemsSplitterId = '\0';
        internal const char GuestId = '\x01';
        internal const char PerformerId = '\x02';
        internal const char RemixerId = '\x04';
        internal const char EndOfStringId = '\x08';

        internal const string TotalsString = "\u0001";

        internal static string LastCommandSbText;
        private static bool LastPreview;
        private static int LastFileCounter;
        private static int LastFileCounterThreshold;
        private static int LastFileCounterTotal;

        internal static ToolStripMenuItem TagToolsSubmenu;
        internal static ToolStripMenuItem TagToolsContextSubmenu;

        internal static ToolStripMenuItem CopyTagsToClipboardUsingMenuItem = null;
        internal static ToolStripMenuItem CopyTagsToClipboardUsingContextMenuItem = null;
        internal static ToolStripMenuItem ASRPresetsMenuItem = null;
        internal static ToolStripMenuItem ASRPresetsContextMenuItem = null;
        internal static ToolStripMenuItem LibraryReportsPresetsMenuItem = null;
        internal static ToolStripMenuItem LRPresetsContextMenuItem = null;

        private int InitialSkipCount;
        #endregion

        #region Localized strings
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

        public class SavedSettingsType
        {
            public bool not1stTimeUsage;

            public bool allowAsrLrPresetAutoexecution;
            public bool allowCommandExecutionWithoutPreview;

            public bool dontShowContextMenu;

            public bool dontShowCopyTag;
            public bool dontShowSwapTags;
            public bool dontShowChangeCase;
            public bool dontShowReencodeTag;
            public bool dontShowLibraryReports;
            public bool dontShowAutorate;
            public bool dontShowASR;
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
            public string usedEncodingName;
            public string reencodeTagSourceTagName;

            public bool onlyIfDestinationIsEmpty;
            public bool onlyIfSourceNotEmpty;
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

            public ReportPreset[] reportsPresets;

            public int autoAppliedReportPresetsCount;
            public bool recalculateOnNumberOfTagsChanges;
            public decimal numberOfTagsToRecalculate;

            public string unitK;
            public string unitM;
            public string unitG;

            public string multipleItemsSplitterChar1;
            public string multipleItemsSplitterChar2;

            public string exportedLastFolder;

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

            public List<WindowSettingsType> windowsSettings;

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
            public bool dontAutoSelectDisplayedTags;

            public string[] lastSelectedFolders;

            public bool backupArtworks;
            public bool dontTryToGuessLibraryName;
            public bool dontSkipAutobackupsIfOnlyPlayCountsChanged;

            public List<Guid> autoAppliedAsrPresetGuids = new List<Guid>();
            public Guid[] asrPresetsWithHotkeysGuids = new Guid[MaximumNumberOfASRHotkeys];

            public bool dontShowPredefinedPresetsCantBeChangedMessage;
            public string defaultAsrPresetsExportFolder;

            public MessageBoxDefaultButton unsavedChangesConfirmationLastAnswer = MessageBoxDefaultButton.Button1;

            public int lastSkippedTagId;
            public int lastSkippedDateFormat;
        }

        internal static SavedSettingsType SavedSettings;
        #endregion


        #region Other localized strings
        //Localizable strings

        //Supported exported file formats
        internal static string ExportedFormats;
        internal static string ExportedTrackList;

        //MusicBeePlugin localizable strings
        internal static string PluginName;
        internal static string PluginMenuGroupName;
        private static string PluginDescription;

        private static string PluginHelpString;
        private static string PluginVersionString;

        private static string OpenWindowsMenuSectionName;
        private static string TagToolsMenuSectionName;
        private static string BackupRestoreMenuSectionName;

        internal static string CopyTagCommandName;
        internal static string SwapTagsCommandName;
        internal static string ChangeCaseCommandName;
        internal static string ReencodeTagCommandName;
        internal static string ReencodeTagsCommandName;
        internal static string LibraryReportsCommandName;
        internal static string AutoRateCommandName;
        internal static string AsrCommandName;
        internal static string CarCommandName;
        internal static string CtCommandName;
        internal static string CopyTagsToClipboardCommandName;
        internal static string PasteTagsFromClipboardCommandName;
        internal static string MsrCommandName;
        internal static string ShowHiddenCommandName;

        private static string TagToolsHotkeyDescription;
        private static string CopyTagCommandDescription;
        private static string SwapTagsCommandDescription;
        private static string ChangeCaseCommandDescription;
        private static string ReencodeTagCommandDescription;
        private static string ReencodeTagsCommandDescription;
        private static string LibraryReportsCommandDescription;
        private static string AutoRateCommandDescription;
        private static string AsrCommandDescription;
        internal static string AsrHotkeyDescription;
        internal static string ReportPresetHotkeyDescription;
        private static string CarCommandDescription;
        private static string CtCommandDescription;
        private static string CopyTagsToClipboardCommandDescription;
        private static string PasteTagsFromClipboardCommandDescription;
        private static string CopyTagsToClipboardUsingMenuDescription;
        private static string MsrCommandDescription;
        internal static string ShowHiddenCommandDescription;

        internal static string BackupTagsCommandName;
        internal static string RestoreTagsCommandName;
        internal static string RestoreTagsForEntireLibraryCommandName;
        internal static string RenameMoveBackupCommandName;
        internal static string MoveBackupsCommandName;
        internal static string CreateNewBaselineCommandName;
        internal static string DeleteBackupsCommandName;
        internal static string AutoBackupSettingsCommandName;

        internal static string TagHistoryCommandName;

        private static string BackupTagsCommandDescription;
        private static string RestoreTagsCommandDescription;
        private static string RestoreTagsForEntireLibraryCommandDescription;
        private static string RenameMoveBackupCommandDescription;
        private static string MoveBackupsCommandDescription;
        private static string CreateNewBaselineCommandDescription;
        private static string DeleteBackupsCommandDescription;
        private static string AutoBackupSettingsCommandDescription;

        private static string TagHistoryCommandDescription;

        internal static string CopyTagCommandSbText;
        internal static string SwapTagsCommandSbText;
        internal static string ChangeCaseCommandSbText;
        internal static string ReencodeTagCommandSbText;
        internal static string LibraryReportsCommandSbText;
        internal static string LibraryReportsGeneratingPreviewCommandSbText;
        internal static string ApplyingLibraryReportSbText;
        internal static string AutoRateCommandSbText;
        internal static string AsrCommandSbText;
        internal static string CarCommandSbText;
        internal static string MsrCommandSbText;

        internal static string AnotherLrPresetIsRunningSbText;

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

        internal static string DefaultASRPresetName;


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
        internal static string MsgNoAggregateFunctionNothingToSave;//*** NOT USED ANYMORE
        internal static string MsgPleaseUseGroupingFunction;
        internal static string MsgAllTags;
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

        internal static string MsgLrDoYouWantToSaveChangesBeforeClosingTheWindow;
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

        internal static string MsgYouMustImportStandardAsrPresetsFirst;

        internal static string CtlDirtyError1sf;
        internal static string CtlDirtyError1mf;
        internal static string CtlDirtyError2sf;
        internal static string CtlDirtyError2mf;


        internal static string MsgMasterBackupIndexIsCorrupted;
        internal static string MsgBackupIsCorrupted;
        internal static string MsgFolderDoesntExists;
        internal static string MsgSelectOneTrackOnly;
        internal static string MsgSelectTrack;
        internal static string MsgSelectAtLeast2Tracks;
        internal static string MsgCreateBaselineWarning;
        internal static string MsgBackupBaselineFileDoesntExist;
        internal static string MsgThisIsTheBackupOfDifferentLibrary;
        internal static string MsgGiveNameToAsrPreset;
        internal static string MsgAreYouSureYouWantToSaveAsrPreset;
        internal static string MsgAreYouSureYouWantToOverwriteAsrPreset;
        internal static string MsgAreYouSureYouWantToOverwriteRenameAsrPreset;
        internal static string MsgAreYouSureYouWantToDeleteAsrPreset;
        internal static string MsgPredefinedPresetsCantBeChanged;
        internal static string MsgSavePreset;
        internal static string MsgDeletePreset;
        internal static string MsgRefreshUi;
        internal static string MsgIncorrectReportPresetId;
        internal static string CtlNewAsrPreset;

        internal static string CtlAutoLrPresetName;

        internal static string CtlAsrCellTooTip;
        internal static string CtlAsrAllTagsCellTooTip;
        internal static string MsgAsrPresetsUsingAllTagsPseudoTagNameCannotBeAutoApplied;

        internal static string SbAutobackuping;
        internal static string SbMovingBackupsToNewFolder;
        internal static string SbMakingTagBackup;
        internal static string SbRestoringTagsFromBackup;
        internal static string SbRenamingMovingBackup;
        internal static string SbMovingBackups;
        internal static string SbDeletingBackups;
        internal static string SbTagAutobackupSkipped;
        internal static string SbCompairingTags;

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
        internal static string CtlMixedValues;
        internal static string CtlMixedValuesSameAsInLibrary;
        internal static string CtlMixedValuesDifferentFromLibrary;

        internal static string MsgNotAllowedSymbols;
        internal static string MsgPresetExists;

        internal static string CtlWholeCuesheetWillBeReencoded;

        internal static string CtlMSR;

        internal static string CtlUnknown;

        internal static string CtlMixedFilters;

        internal static string MsgFirstSelectWhichFieldYouWantToAssignFunctionIdTo;
        #endregion


        #region Common methods/functions
        public struct SwappedTags
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

        public struct ConvertStringsResult
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

            internal ConvertStringsResult(ResultType resultTypeParam, DataType dataTypeParam)
            {
                dataType = dataTypeParam;
                resultType = resultTypeParam;

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
                if (items1.Count != 0) //Its "average count" function, i.e. double
                {
                    double value = (double)items1.Count / items.Count;

                    double comparedValue;
                    if (comparedResult.items.Count == 0 && comparedResult.items1.Count == 0)
                        comparedValue = 0;
                    else
                        comparedValue = (double)comparedResult.items1.Count / comparedResult.items.Count;

                    if (value == comparedValue)
                        return 0;
                    else if (value < comparedValue)
                        return -1;
                    else //if (value > comparedValue)
                        return +1;
                }
                else if (resultType == ResultType.UnknownOrString || resultType == ResultType.UseOtherResults) //**** use other results to get type????
                {
                    string value = resultS;
                    string comparedValue = comparedResult.resultS;

                    return value.CompareTo(comparedValue);
                }
                else if (resultType == ResultType.ItemCount) //Items count
                {
                    double value = items.Count;
                    double comparedValue = comparedResult.items.Count;

                    if (value == comparedValue)
                        return 0;
                    else if (value < comparedValue)
                        return -1;
                    else //if (value > comparedValue)
                        return +1;
                }
                else if (resultType >= ResultType.AutoDouble && resultType < ResultType.UnknownOrString) //(Auto)double, year or date-time (milliseconds from 1900-01-01), timespan (milliseconds)
                {
                    double value = resultD;
                    if (items != null && items.Count != 0) //Its "average" function
                        value /= items.Count;

                    double comparedValue = comparedResult.resultD;
                    if (items != null && items.Count != 0) //Its "average" function
                        comparedValue /= comparedResult.items.Count;

                    if (value == comparedValue)
                        return 0;
                    else if (value < comparedValue)
                        return -1;
                    else //if (value > comparedValue)
                        return +1;
                }


                throw new Exception("Unsupported result type: " + resultType + "!"); //This must never happen
            }

            internal double getResult() //Returns double for any numeric (number/duration/date-time) result or NegativeInfinity for any not numeric one
            {
                if (items1.Count != 0) //Its "Average count" function
                {
                    return (double)items1.Count / items.Count;
                }
                //else if (resultType >= ResultType.UnknownOrString || resultType == ResultType.UseOtherResults) //**** must never happen
                //{
                //    if (items.Count == 0)
                //        return double.NegativeInfinity;
                //    else //Its "average" function
                //        return resultD / items.Count;
                //}
                else if (resultType == ResultType.ItemCount) //Items count
                {
                    return items.Count;
                }
                else if (resultType == ResultType.Double || resultType == ResultType.AutoDouble) //Double
                {
                    if (items.Count == 0)
                        return resultD;
                    else //Its "average" function
                        return resultD / items.Count;
                }
                else if (resultType == ResultType.TimeSpan) //Timespan
                {
                    if (items.Count == 0)
                        return resultD;
                    else //Its "average" function
                        return resultD / items.Count;
                }
                else if (resultType == ResultType.DateTime) //Datetime
                {
                    if (items.Count == 0)
                        return resultD;
                    else //Its "average" function
                        return resultD / items.Count;
                }
                else if (resultType == ResultType.Year) //Datetime (year only)
                {
                    if (items.Count == 0)
                        return resultD;
                    else //Its "average" function
                        return resultD / items.Count;
                }
                //else if (resultType == ResultType.ParsingError) //Parsing error //**** must never happen
                //{
                //    return double.NegativeInfinity;
                //}
                else
                {
                    throw new Exception("Not implemented result type: " + resultType + "!");
                }
            }

            internal string getFormattedResult(int operation, string mulDivFactorRepr, string precisionDigitsRepr, string appendedText)
            {
                if (resultType == ResultType.UnknownOrString || resultType == ResultType.UseOtherResults) //**** Use other results???
                {
                    return resultS;
                }
                else if (resultType == ResultType.ParsingError)
                {
                    return "#PARSING ERROR!";
                }


                double result = getResult();

                if (result == double.NegativeInfinity)
                {
                    resultType = ResultType.ParsingError;

                    return "#INVALID VALUE!";
                }
                else if (resultType == ResultType.ItemCount || resultType == ResultType.Double || resultType == ResultType.AutoDouble) //Item count or double. It's numeric result. Let's format it.
                {
                    int mulDivFactor = GetMulDivFactor(mulDivFactorRepr);
                    if (mulDivFactor != 1 && operation == 0)
                        result /= mulDivFactor;
                    else if (mulDivFactor != 1 && operation == 1)
                        result *= mulDivFactor;

                    int precisionDigits = GetPrecisionDigits(precisionDigitsRepr);
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

            string fractionalpart = Regex.Replace(input, @"^(\D*?)(\d+)(\.|\,)?(\d*?)(\s*)" + units + @"(\D.*)$", "$4", RegexOptions.IgnoreCase);
            if (fractionalpart == string.Empty || fractionalpart == input)
                stringnumber = Regex.Replace(input, @"^(\D*?)(\d+)(\.|\,)?(\d*?)(\s*)" + units + @"(\D.*)$", "$2", RegexOptions.IgnoreCase);
            else
                stringnumber = Regex.Replace(input, @"^(\D*?)(\d+)(\.|\,)?(\d*?)(\s*)" + units + @"(\D.*)$", "$2" + LocalizedDecimalPoint + "$4", RegexOptions.IgnoreCase);

            if (stringnumber == input)
                return (double.NegativeInfinity, null, null, null);

            string prefix = Regex.Replace(input, @"^(\D*?)(\d+)(\.|\,)?(\d*?)(\s*)" + units + @"(\D.*)$", "$1", RegexOptions.IgnoreCase);
            if (prefix == input)
                prefix = string.Empty;

            string space = Regex.Replace(input, @"^(\D*?)(\d+)(\.|\,)?(\d*?)(\s*)" + units + @"(\D.*)$", "$5", RegexOptions.IgnoreCase);
            if (space == input)
                space = string.Empty;

            string postfix = Regex.Replace(input, @"^(\D*?)(\d+)(\.|\,)?(\d*?)(\s*)" + units + @"(\D.*)$", "$7", RegexOptions.IgnoreCase);
            if (postfix == input)
                postfix = string.Empty;

            if (double.TryParse(stringnumber, out double number))
                return ((number * multiplicator), prefix, space, postfix);
            else
                return (double.NegativeInfinity, null, null, null);
        }

        internal static (double, string, string, string) ParseNumber(string input) //Returns normalized number & string postfix
        {
            string stringnumber;

            string fractionalpart = Regex.Replace(input, @"^(\D*?)(\d+)(\.|\,)?(\d*?)(\s*)(\D*)$", "$4", RegexOptions.IgnoreCase);
            if (fractionalpart == string.Empty || fractionalpart == input)
                stringnumber = Regex.Replace(input, @"^(\D*?)(\d+)(\.|\,)?(\d*?)(\s*)(\D*)$", "$2", RegexOptions.IgnoreCase);
            else
                stringnumber = Regex.Replace(input, @"^(\D*?)(\d+)(\.|\,)?(\d*?)(\s*)(\D*)$", "$2" + LocalizedDecimalPoint + "$4", RegexOptions.IgnoreCase);

            string prefix = Regex.Replace(input, @"^(\D*?)(\d+)(\.|\,)?(\d*?)(\s*)(\D*)$", "$1", RegexOptions.IgnoreCase);

            if (!string.IsNullOrWhiteSpace(prefix)) // Probably prefixed string must not br treated as number at all //***
                return (double.NegativeInfinity, null, null, null);
            if (prefix == input)
                prefix = string.Empty;

            string space = Regex.Replace(input, @"^(\D*?)(\d+)(\.|\,)?(\d*?)(\s*)(\D*)$", "$5", RegexOptions.IgnoreCase);
            if (space == input)
                space = string.Empty;

            string postfix = Regex.Replace(input, @"^(\D*?)(\d+)(\.|\,)?(\d*?)(\s*)(\D*)$", "$6", RegexOptions.IgnoreCase);
            if (postfix == input)
                postfix = string.Empty;

            if (double.TryParse(stringnumber, out double number))
                return (number, prefix, space, postfix);
            else
                return (double.NegativeInfinity, null, null, null);
        }

        internal static ConvertStringsResult ConvertStrings(string arg, ResultType resultType, DataType dataType, bool replacements = false)
        {
            ConvertStringsResult result = new ConvertStringsResult(resultType, dataType);

            result.resultS = arg;

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
                string unitsK = "(k|к";
                string unitsM = "(m|м";
                string unitsG = "(g|г";

                if (!string.IsNullOrWhiteSpace(SavedSettings.unitK))
                    unitsK = "|" + SavedSettings.unitK;

                if (!string.IsNullOrWhiteSpace(SavedSettings.unitM))
                    unitsM = "|" + SavedSettings.unitM;

                if (!string.IsNullOrWhiteSpace(SavedSettings.unitG))
                    unitsG = "|" + SavedSettings.unitG;

                unitsK += ")";
                unitsM += ")";
                unitsG += ")";

                double number = double.NegativeInfinity;

                string numberprefix = null;
                string numberspace = null;
                string numberpostfix = null;

                double numbertemp;
                string numberprefixtemp;
                string numberspacetemp;
                string numberpostfixtemp;


                (numbertemp, numberprefixtemp, numberspacetemp, numberpostfixtemp) = ParseNumberAndMeasurementUnits(arg, unitsG, 1_099_511_627_776);
                if (numbertemp != double.NegativeInfinity)
                {
                    number = numbertemp;
                    numberprefix = numberprefixtemp;
                    numberspace = numberspacetemp;
                    numberpostfix = numberpostfixtemp;
                }
                else
                {
                    (numbertemp, numberprefixtemp, numberspacetemp, numberpostfixtemp) = ParseNumberAndMeasurementUnits(arg, unitsM, 1_048_576);
                    if (numbertemp != double.NegativeInfinity)
                    {
                        number = numbertemp;
                        numberprefix = numberprefixtemp;
                        numberspace = numberspacetemp;
                        numberpostfix = numberpostfixtemp;
                    }
                    else
                    {
                        (numbertemp, numberprefixtemp, numberspacetemp, numberpostfixtemp) = ParseNumberAndMeasurementUnits(arg, unitsK, 1024);
                        if (numbertemp != double.NegativeInfinity)
                        {
                            number = numbertemp;
                            numberprefix = numberprefixtemp;
                            numberspace = numberspacetemp;
                            numberpostfix = numberpostfixtemp;
                        }
                        else
                        {
                            if (number == double.NegativeInfinity)
                            {
                                (numbertemp, numberprefixtemp, numberspacetemp, numberpostfixtemp) = ParseNumber(arg);
                                if (numbertemp != double.NegativeInfinity)
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


                if (number != double.NegativeInfinity)
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
                if (arg.Length < 5 && !arg.Contains(":") && !arg.Contains("/") && !arg.Contains("-") && !arg.Contains("."))
                {
                    result.resultD = (new DateTime(int.Parse(arg), 1, 1) - ConvertStringsResult.ReferenceDt - ConvertStringsResult.ReferenceTsOffset).TotalMilliseconds;
                    result.resultType = ResultType.Year;

                    return result;
                }
                else if (arg.Contains(":") && !arg.Contains("/") && !arg.Contains("-") && !arg.Contains("."))
                {
                    return ConvertStrings(arg, ResultType.TimeSpan, DataType.String);
                }
                else if (DateTime.TryParse(arg, out DateTime datetime))
                {
                    if (datetime.Year < 1900)
                    {
                        result.resultD = TimeSpan.Parse(arg).TotalMilliseconds;
                        result.resultType = ResultType.TimeSpan;
                    }
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

                try //Let's try to parse as short duration (without date)
                {
                    if (arg.Contains(":") && !arg.Contains("/") && !arg.Contains("-") && !arg.Contains("."))
                    {
                        string[] ts1 = arg.Split(':');
                        TimeSpan time;

                        time = TimeSpan.FromSeconds(Convert.ToInt32(ts1[ts1.Length - 1]));
                        time += TimeSpan.FromMinutes(Convert.ToInt32(ts1[ts1.Length - 2]));
                        if (ts1.Length > 2)
                            time += TimeSpan.FromHours(Convert.ToInt32(ts1[ts1.Length - 3]));

                        result.resultD = time.TotalMilliseconds;
                        result.resultType = ResultType.TimeSpan;

                        return result;
                    }
                    else
                    {
                        throw new Exception(); //Go to parsing as date/time
                    }
                }
                catch
                {
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
                        try //Let's try to parse as timespan again (as full timespan format including date)
                        {
                            result.resultD = TimeSpan.Parse(arg).TotalSeconds;
                            result.resultType = ResultType.TimeSpan;

                            return result;
                        }
                        catch
                        {
                            //Let's try to parse as number if there were no replacements
                            return ConvertStrings(arg, ResultType.AutoDouble, DataType.String, true);
                        }
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

                    if (result1.resultD > result2.resultD)
                        return 1;
                    else if (result1.resultD < result2.resultD)
                        return -1;
                    else
                        return 0;

                case ResultType.TimeSpan:

                    if (result1.resultD > result2.resultD)
                        return 1;
                    else if (result1.resultD < result2.resultD)
                        return -1;
                    else
                        return 0;

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

        public class DataGridViewCellComparer : System.Collections.IComparer
        {
            internal ResultType[] resultTypes = null;
            internal int comparedColumnIndex = -1;

            public int Compare(object x, object y)
            {
                if (resultTypes == null)
                    throw new Exception("Undefined table sorting type!");

                if (comparedColumnIndex != -1)
                {
                    int comparison = CompareStrings((x as DataGridViewRow).Cells[comparedColumnIndex].Value as string, (y as DataGridViewRow).Cells[comparedColumnIndex].Value as string);
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
            internal int tagCounterIndex = -1;

            public int Compare(string[] x, string[] y)
            {
                if (tagCounterIndex != -1)
                {
                    int comparation;

                    for (int i = 0; i < tagCounterIndex; i++)
                    {
                        comparation = CompareStrings(x[i], y[i], ResultType.Double, DataType.Number);

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
            string id = GetPersistentTrackId(trackUrl);
            string nodeName = MbApiInterface.Library_GetFileTag(trackUrl, (MetaDataType)42);

            if (nodeName != MusicName)
                return;


            int trackId = int.Parse(id);


            if (UpdatedTracksForBackupCount < MaxUpdatedTracksCount)
            {
                if (!TracksNeedsToBeBackedUp.Contains(trackId))
                {
                    TracksNeedsToBeBackedUp.Add(trackId, true);
                    UpdatedTracksForBackupCount++;
                }
            }
            else
            {
                TracksNeedsToBeBackedUp.Clear();
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
            string trackRepresentation = string.Empty;
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
            trackRepresentation += (trackRepresentation == string.Empty) ? (trackNo) : ("-" + trackNo);
            trackRepresentation += (trackRepresentation == string.Empty) ? string.Empty : ". ";
            trackRepresentation += (trackRepresentation == string.Empty) ? (displayedArtist) : (". " + displayedArtist);
            trackRepresentation += (trackRepresentation == string.Empty) ? (album) : (" - " + album);
            trackRepresentation += (trackRepresentation == string.Empty) ? (title) : (" - " + title);

            return trackRepresentation;
        }

        internal static string GetTrackRepresentation(string[] tags, string[] tags2, string[] tagNames, bool previewSortTags)
        {
            string trackRepresentation = string.Empty;

            trackRepresentation += tags[12]; //12 - track number
            trackRepresentation += (trackRepresentation == string.Empty) ? tags[28] : ("-" + tags[28]); //28 - disk number
            trackRepresentation += (trackRepresentation == string.Empty) ? string.Empty : ". ";

            trackRepresentation += tags[5]; //5 - Album artist or artist
            trackRepresentation += (trackRepresentation == string.Empty) ? tags[3] : (" - " + tags[3]); //3 - album name
            trackRepresentation += (trackRepresentation == string.Empty) ? tags[2] : (" - " + tags[2]); //2 - track tiltle

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

            trackRepresentation = Regex.Replace(trackRepresentation, "\0", "; "); //Remove multiple artist/composer splitters
            trackRepresentation = Regex.Replace(trackRepresentation, "\x01", string.Empty); //Here and below: remove performer/remixer/guest ids
            trackRepresentation = Regex.Replace(trackRepresentation, "\x02", string.Empty);
            trackRepresentation = Regex.Replace(trackRepresentation, "\x03", string.Empty);

            return trackRepresentation;
        }

        internal static SwappedTags SwapTags(string sourceTagValue, string destinationTagValue, MetaDataType sourceTagId, MetaDataType destinationTagId,
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

        internal static void CustomComboBoxLeave(CustomComboBox comboBox, string newValue = null)
        {
            string comboBoxText = (newValue ?? comboBox.Text);

            if (comboBoxText == string.Empty)
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
            string comboBoxText = (newValue ?? comboBox.Text);

            if (comboBoxText == string.Empty)
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
        //    bool itemIsFound = false;

        //    for (int i = 0; i < listBox.Items.Count; i++)
        //    {
        //        if ((listBox.Items[i] as ReportPreset).guid == preset.guid)
        //        {
        //            listBox.Items.RemoveAt(i);
        //            itemIsFound = true;
        //            break;
        //        }
        //    }

        //    if (!itemIsFound)
        //        listBox.Items.RemoveAt(listBox.Items.Count - 1);

        //    listBox.Items.Insert(NumberOfPredefinedPresets, preset);
        //    listBox.SelectedItem = preset;
        //}

        internal static string GetFileTag(string sourceFileUrl, MetaDataType tagId, bool normalizeTrackRatingTo0_100Range = false)
        {
            string tag = string.Empty;
            string rawArtist = string.Empty;
            string multiArtist = string.Empty;
            string rawComposer = string.Empty;
            string multiComposer = string.Empty;


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
                        SetStatusbarText(ex.Message, false);
                        return string.Empty;
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

                    if (multiArtist != string.Empty)
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


            if (tag == null)
                tag = string.Empty;


            return tag;
        }

        internal static string[] GetFileTags(string sourceFileUrl, List<MetaDataType> tagIds)
        {
            MetaDataType[] tagIds2 = tagIds.ToArray();

            for (int i = 0; i < tagIds2.Length; i++)
            {
                MetaDataType tagId = tagIds2[i];

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



            if (!MbApiInterface.Library_GetFileTags(sourceFileUrl, tagIds2, out string[] tags))
                return null;


            for (int i = 0; i < tagIds.Count; i++)
            {
                MetaDataType tagId = tagIds[i];

                switch (tagId)
                {
                    case 0:
                        tags[i] = string.Empty;
                        break;

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
            string multiArtist = string.Empty;
            string multiComposer = string.Empty;
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
                        Bitmap bitmapImage = (tc.ConvertFrom(Convert.FromBase64String(value)) as Bitmap);

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

        internal static bool CommitTagsToFile(string sourceFileUrl, bool ignoreTagsChanged = false, bool updateOnlyChangedTags = false)
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
                bool refresh = false;

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

        internal static void SetStatusbarText(string newMessage, bool autoClear)
        {
            if (autoClear)
            {
                if (LastMessage != newMessage)
                    MbApiInterface.MB_SetBackgroundTaskMessage(newMessage);

                DelayedStatusbarTextClearingTimer = new System.Threading.Timer(DelayedStatusbarTextClearingDelegate, null, (int)RefreshUI_Delay.TotalMilliseconds * 2, 0);
            }
            else if (LastMessage != newMessage)
            {
                MbApiInterface.MB_SetBackgroundTaskMessage(newMessage);
                LastMessage = newMessage;
            }
        }

        private void DelayedStatusbarTextClearing(object state)
        {
            MbApiInterface.MB_SetBackgroundTaskMessage(string.Empty);
            DelayedStatusbarTextClearingTimer.Dispose();
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

        internal static string GetTagName(MetaDataType tagId, string allTagsTagName = null)
        {
            if (tagId == DateCreatedTagId)
                return DateCreatedTagName;

            if (tagId == FolderTagId)
                return FolderTagName;

            if (tagId == FileNameTagId)
                return FileNameTagName;

            if (tagId == FileNameTagId)
                return FilePathWoExtTagName;

            if (tagId == FilePathTagId)
                return FilePathTagName;

            if (tagId == FilePathWoExtTagId)
                return FilePathWoExtTagName;

            if (tagId == AllTagsPseudoTagId)
            {
                if (allTagsTagName == null)
                    return AllTagsPseudoTagName;
                else
                    return allTagsTagName;
            }


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
                return string.Empty;
            }
        }

        internal static string GetPropPrefix(string propName)
        {
            FilePropertyType check; //To throw an exception if property name doesn't exist
            if (propName != null)
                check = PropNamesIds[propName];

            return "❏ ";
        }

        internal static string GetTagPrefix(string tagName)
        {
            string prefix;

            if (!TagNamesIds.TryGetValue(tagName, out MetaDataType tagId))
                return GetPropPrefix(null);


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
            bool addNullAlso = true, bool addTagPrefixes = false, bool addAllTagsPseudoTagAlso = false, bool addDateCreatedAlso = true)
        {
            foreach (string tagName in TagNamesIds.Keys)
            {
                string prefix = string.Empty;

                if (addTagPrefixes)
                {
                    prefix = GetTagPrefix(tagName);
                }

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
                else if (addReadOnlyTagsAlso || !ChangeCase.IsItemContainedInList(tagName, ReadonlyTagsNames))
                {
                    list.Add(prefix + tagName);
                }
            }
        }

        internal static FilePropertyType GetPropId(string propName)
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

        internal static string GetPropName(FilePropertyType propId)
        {
            if (PropIdsNames.TryGetValue(propId, out string propName))
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
            string marker = string.Empty;

            if (addTagMarker)
                marker = GetPropPrefix(null);

            foreach (string propName in PropNamesIds.Keys)
            {
                list.Add(marker + propName);
            };
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
                SetStatusbarText(string.Empty, false);
                return;
            }
            else if (LastCommandSbText == "<CUESHEET>")
            {
                System.Media.SystemSounds.Exclamation.Play();
                SetStatusbarText(CtlWholeCuesheetWillBeReencoded, false);
                return;
            }
            else if (finalStatus == null)
            {
                string sbText;

                if (LastPreview)
                    sbText = LastCommandSbText + ": " + "100% (" + LastFileCounter + " " + SbItemNames + ") " + SbRead;
                else
                    sbText = LastCommandSbText + ": " + "100% (" + LastFileCounter + " " + SbItemNames + ") " + SbUpdated;

                //if (lastPreview)
                //    sbText = LastCommandSbText + ": 100% " + sbRead;
                //else
                //    sbText = LastCommandSbText + ": 100% " + sbUpdated;

                SetStatusbarText(sbText, autoClear);
            }
            else
            {
                SetStatusbarText(GenegateStatusbarTextForFileOperations(LastCommandSbText, LastPreview, LastFileCounter, LastFileCounterTotal, finalStatus), autoClear);
            }


            if (!sbSetFilesAsItems)
                SbItemNames = SbFiles;
        }

        internal static string GenegateStatusbarTextForFileOperations(string commandSbText, bool preview, int fileCounter1Based, int filesTotal, string currentFile = null)
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

        internal static void SetStatusbarTextForFileOperations(string commandSbText, bool preview, int fileCounter0Based, int filesTotal, string currentFile = null, int updatePercentage = 10)
        {
            LastCommandSbText = commandSbText;
            LastPreview = preview;
            LastFileCounter = ++fileCounter0Based; //Let's make fileCounter0Based 1-BASED!
            LastFileCounterTotal = filesTotal;

            if (fileCounter0Based < 2)
                LastFileCounterThreshold = 0;

            bool update = false;
            if (filesTotal == 0 || (fileCounter0Based - LastFileCounterThreshold) / (float)filesTotal >= updatePercentage / 100f)
            {
                update = true;
                LastFileCounterThreshold = fileCounter0Based;
            }

            if (updatePercentage == 0 || update || fileCounter0Based % StatusbarTextUpdateInterval == 0)
            {
                SetStatusbarText(GenegateStatusbarTextForFileOperations(commandSbText, preview, fileCounter0Based, filesTotal, currentFile), false);
            }
        }

        private static void regularUI_Refresh(object state)
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

        internal void regularAutobackup(object state)
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
                if (backupCache == null)
                    return;

                if (backupCache.isAutocreated && backupCache.libraryName == currentLibraryName)
                {
                    int negativeDate = -((backupCache.creationDate.Year * 10000 + backupCache.creationDate.Month * 100 + backupCache.creationDate.Day) * 1000000 +
                        backupCache.creationDate.Hour * 10000 + backupCache.creationDate.Minute * 100 + backupCache.creationDate.Second);

                    filesWithNegativeDates.Add(negativeDate, backupFilenameWithoutExtension);
                    fileCreationDates.Add(backupFilenameWithoutExtension, backupCache.creationDate);
                }
            }


            List<string> backupsToDelete = new List<string>();
            DateTime now = DateTime.UtcNow;

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
            CopyTag tagToolsForm = new CopyTag(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void swapTagsEventHandler(object sender, EventArgs e)
        {
            SwapTags tagToolsForm = new SwapTags(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void changeCaseEventHandler(object sender, EventArgs e)
        {
            ChangeCase tagToolsForm = new ChangeCase(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void reencodeTagEventHandler(object sender, EventArgs e)
        {
            ReencodeTag tagToolsForm = new ReencodeTag(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void reencodeTagsEventHandler(object sender, EventArgs e)
        {
            ReencodeTags tagToolsForm = new ReencodeTags(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void libraryReportsEventHandler(object sender, EventArgs e)
        {
            LibraryReports tagToolsForm = new LibraryReports(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void autoRateEventHandler(object sender, EventArgs e)
        {
            AutoRate tagToolsForm = new AutoRate(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void asrEventHandler(object sender, EventArgs e)
        {
            AdvancedSearchAndReplace tagToolsForm = new AdvancedSearchAndReplace(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void carEventHandler(object sender, EventArgs e)
        {
            CalculateAverageAlbumRating tagToolsForm = new CalculateAverageAlbumRating(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void copyTagsToClipboardEventHandler(object sender, EventArgs e)
        {
            CopyTagsToClipboard tagToolsForm = new CopyTagsToClipboard(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void pasteTagsFromClipboardEventHandler(object sender, EventArgs e)
        {
            PasteTagsFromClipboard(this);
        }

        internal void multipleSearchReplaceEventHandler(object sender, EventArgs e)
        {
            MultipleSearchAndReplace tagToolsForm = new MultipleSearchAndReplace(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void showHiddenEventHandler(object sender, EventArgs e)
        {
            lock (OpenedForms)
            {
                foreach (PluginWindowTemplate form in OpenedForms)
                {
                    if (!form.Visible || form.WindowState == FormWindowState.Minimized)
                        PluginWindowTemplate.Display(form);
                }
            }
        }

        internal void backupTagsEventHandler(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Title = CtlSaveBackupTitle,
                Filter = CtlMusicBeeBackupType,
                InitialDirectory = GetAutobackupDirectory(SavedSettings.autobackupDirectory),
                FileName = GetDefaultBackupFilename(BackupDefaultPrefix)
            };

            if (dialog.ShowDialog(MbForm) == DialogResult.Cancel) return;

            MbApiInterface.MB_SetBackgroundTaskMessage(SbMakingTagBackup);

            if (File.Exists(dialog.FileName))
                File.Delete(dialog.FileName);
            if (File.Exists(GetBackupFilenameWithoutExtension(dialog.FileName) + ".mbc"))
                File.Delete(GetBackupFilenameWithoutExtension(dialog.FileName) + ".mbc");

            MbApiInterface.MB_CreateParameterisedBackgroundTask(BackupIndex.saveBackupAsync, new object[] { GetBackupFilenameWithoutExtension(dialog.FileName), SbMakingTagBackup, false }, MbForm);
            dialog.Dispose();
        }

        internal void restoreTagsEventHandler(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = CtlRestoreBackupTitle,
                Filter = CtlMusicBeeBackupType,
                InitialDirectory = GetAutobackupDirectory(SavedSettings.autobackupDirectory)
            };

            if (dialog.ShowDialog(MbForm) == DialogResult.Cancel) return;

            MbApiInterface.MB_SetBackgroundTaskMessage(SbRestoringTagsFromBackup);

            MbApiInterface.MB_CreateParameterisedBackgroundTask(BackupIndex.LoadBackupAsync, new object[] { GetBackupFilenameWithoutExtension(dialog.FileName), SbRestoringTagsFromBackup, false, this }, MbForm);
            dialog.Dispose();
        }

        internal void restoreTagsForEntireLibraryEventHandler(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = CtlRestoreBackupTitle,
                Filter = CtlMusicBeeBackupType,
                InitialDirectory = GetAutobackupDirectory(SavedSettings.autobackupDirectory)
            };

            if (dialog.ShowDialog(MbForm) == DialogResult.Cancel) return;

            MbApiInterface.MB_SetBackgroundTaskMessage(SbRestoringTagsFromBackup);

            MbApiInterface.MB_CreateParameterisedBackgroundTask(BackupIndex.LoadBackupAsync, new object[] { GetBackupFilenameWithoutExtension(dialog.FileName), SbRestoringTagsFromBackup, true, this }, MbForm);
            dialog.Dispose();
        }

        internal void renameMoveBackupEventHandler(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Title = CtlRenameSelectBackupTitle,
                Filter = CtlMusicBeeBackupType,
                InitialDirectory = GetAutobackupDirectory(SavedSettings.autobackupDirectory)
            };

            if (openDialog.ShowDialog(MbForm) == DialogResult.Cancel) return;

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Title = CtlRenameSaveBackupTitle,
                Filter = CtlMusicBeeBackupType,
                InitialDirectory = GetAutobackupDirectory(SavedSettings.autobackupDirectory),
                FileName = openDialog.SafeFileName
            };

            if (saveDialog.ShowDialog(MbForm) == DialogResult.Cancel) return;

            if (openDialog.FileName == saveDialog.FileName) return;

            MbApiInterface.MB_SetBackgroundTaskMessage(SbRenamingMovingBackup);

            if (File.Exists(saveDialog.FileName))
                File.Delete(saveDialog.FileName);
            if (File.Exists(GetBackupFilenameWithoutExtension(saveDialog.FileName) + ".mbc"))
                File.Delete(GetBackupFilenameWithoutExtension(saveDialog.FileName) + ".mbc");

            File.Move(openDialog.FileName, saveDialog.FileName);
            File.Move(GetBackupFilenameWithoutExtension(openDialog.FileName) + ".mbc", GetBackupFilenameWithoutExtension(saveDialog.FileName) + ".mbc");

            MbApiInterface.MB_SetBackgroundTaskMessage(string.Empty);
            openDialog.Dispose();
            saveDialog.Dispose();
        }

        internal void moveBackupsEventHandler(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Title = CtlMoveSelectBackupsTitle,
                Filter = CtlMusicBeeBackupType,
                InitialDirectory = GetAutobackupDirectory(SavedSettings.autobackupDirectory),
                Multiselect = true
            };

            if (openDialog.ShowDialog(MbForm) == DialogResult.Cancel) return;

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Title = CtlMoveSaveBackupsTitle,
                Filter = "|*.destinationfolder",
                //saveDialog.InitialDirectory = GetAutobackupDirectory(SavedSettings.autobackupDirectory);
                FileName = CtlSelectThisFolder
            };

            if (saveDialog.ShowDialog(MbForm) == DialogResult.Cancel) return;

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

            MbApiInterface.MB_SetBackgroundTaskMessage(string.Empty);
            openDialog.Dispose();
            saveDialog.Dispose();
        }

        internal void createNewBaselineEventHandler(object sender, EventArgs e)
        {
            MessageBox.Show(MbForm, MsgCreateBaselineWarning, CtlWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);


            SaveFileDialog dialog = new SaveFileDialog
            {
                Title = CtlSaveBackupTitle,
                Filter = CtlMusicBeeBackupType,
                InitialDirectory = GetAutobackupDirectory(SavedSettings.autobackupDirectory),
                FileName = GetDefaultBackupFilename(BackupDefaultPrefix)
            };

            if (dialog.ShowDialog(MbForm) == DialogResult.Cancel) return;


            lock (AutobackupLocker)
            {
                //Lets delete all incremental backups of current library
                string currentLibraryName = GetLibraryName();
                string[] files = Directory.GetFiles(GetAutobackupDirectory(SavedSettings.autobackupDirectory), "*.mbc", SearchOption.TopDirectoryOnly);

                BackupCacheType backupCache = new BackupCacheType();

                for (int i = 0; i < files.Length; i++)
                {
                    string backupFilenameWithoutExtension = GetBackupFilenameWithoutExtension(files[i]);

                    backupCache = BackupCacheType.Load(backupFilenameWithoutExtension, ".xml", true);

                    if (backupCache == null || backupCache.libraryName == currentLibraryName)
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

        internal void deleteBackupsEventHandler(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = CtlDeleteBackupsTitle,
                Filter = CtlMusicBeeBackupType,
                InitialDirectory = GetAutobackupDirectory(SavedSettings.autobackupDirectory),
                Multiselect = true
            };

            if (dialog.ShowDialog(MbForm) == DialogResult.Cancel) return;

            MbApiInterface.MB_SetBackgroundTaskMessage(SbDeletingBackups);

            BackupCacheType backupCache = new BackupCacheType();
            foreach (var filename in dialog.FileNames)
            {
                backupCache = BackupCacheType.Load(GetBackupFilenameWithoutExtension(filename));

                if (backupCache != null)
                    BackupIndex.deleteBackup(backupCache);

                File.Delete(filename);
                File.Delete(GetBackupFilenameWithoutExtension(filename) + ".mbc");
            }

            MbApiInterface.MB_SetBackgroundTaskMessage(string.Empty);
            dialog.Dispose();
        }

        internal void autoBackupSettingsEventHandler(object sender, EventArgs e)
        {
            AutoBackupSettings tagToolsForm = new AutoBackupSettings(this);
            PluginWindowTemplate.Display(tagToolsForm);
        }

        internal void tagHistoryEventHandler(object sender, EventArgs e)
        {
            if (MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out string[] files))
            {
                //if (files.Length > 1)
                //{
                //    MessageBox.Show(MusicBeePlugin.MbForm, msgSelectOneTrackOnly);
                //    return;
                //}

                if (files.Length == 0)
                {
                    MessageBox.Show(MbForm, MsgSelectTrack, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                int[] trackIds = new int[files.Length];
                for (int i = 0; i < files.Length; i++)
                    trackIds[i] = int.Parse(GetPersistentTrackId(files[i]));


                TagHistory tagToolsForm = new TagHistory(this, files, trackIds);
                PluginWindowTemplate.Display(tagToolsForm, true);
            }
        }

        internal void compareTracksEventHandler(object sender, EventArgs e)
        {

            if (MbApiInterface.Library_QueryFilesEx("domain=SelectedFiles", out string[] files))
            {
                if (files.Length < 2)
                {
                    MessageBox.Show(MbForm, MsgSelectAtLeast2Tracks, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }


                CompareTracks tagToolsForm = new CompareTracks(this, files);
                PluginWindowTemplate.Display(tagToolsForm, true);
            }
        }

        internal void helpEventHandler(object sender, EventArgs e)
        {
            Process.Start(PluginHelpFilePath);
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

            PluginHelpString = "Help...";//********
            PluginVersionString = "Version: ";

            OpenWindowsMenuSectionName = "OPEN WINDOWS";
            TagToolsMenuSectionName = "TAGGING && REPORTING";
            BackupRestoreMenuSectionName = "BACKUP && RESTORE";

            CopyTagCommandName = "Copy Tag...";
            SwapTagsCommandName = "Swap Tags...";
            ChangeCaseCommandName = "Change Case...";
            ReencodeTagCommandName = "Reencode Tag...";
            ReencodeTagsCommandName = "Reencode Tags...";
            LibraryReportsCommandName = "Library Reports...";
            AutoRateCommandName = "Auto Rate Tracks...";
            AsrCommandName = "Advanced Search && Replace...";
            CarCommandName = "Calculate Average Album Rating...";
            CtCommandName = "Compare Tracks...";
            CopyTagsToClipboardCommandName = "Copy Tags to Clipboard...";
            PasteTagsFromClipboardCommandName = "Paste Tags from Clipboard";
            MsrCommandName = "Multiple Search && Replace...";
            ShowHiddenCommandName = "Show hidden/restore minimized plugin windows";

            TagToolsHotkeyDescription = "Tagging Tools: ";
            CopyTagCommandDescription = TagToolsHotkeyDescription + "Copy Tag";
            SwapTagsCommandDescription = TagToolsHotkeyDescription + "Swap Tags";
            ChangeCaseCommandDescription = TagToolsHotkeyDescription + "Change Case";
            ReencodeTagCommandDescription = TagToolsHotkeyDescription + "Reencode Tag";
            ReencodeTagsCommandDescription = TagToolsHotkeyDescription + "Reencode Tags";
            LibraryReportsCommandDescription = TagToolsHotkeyDescription + "Library Reports";
            ReportPresetHotkeyDescription = TagToolsHotkeyDescription;
            AutoRateCommandDescription = TagToolsHotkeyDescription + "Auto Rate Tracks";
            AsrCommandDescription = TagToolsHotkeyDescription + "Advanced Search & Replace";
            AsrHotkeyDescription = TagToolsHotkeyDescription;
            CarCommandDescription = TagToolsHotkeyDescription + "Calculate Average Album Rating";
            CtCommandDescription = TagToolsHotkeyDescription + "Compare Tracks";
            CopyTagsToClipboardCommandDescription = TagToolsHotkeyDescription + "Copy Tags to Clipboard";
            PasteTagsFromClipboardCommandDescription = TagToolsHotkeyDescription + "Paste Tags from Clipboard";
            CopyTagsToClipboardUsingMenuDescription = "Copy Tags to Clipboard Using Tag Set";
            MsrCommandDescription = TagToolsHotkeyDescription + "Multiple Search & Replace";
            ShowHiddenCommandDescription = TagToolsHotkeyDescription + "Show Hidden MusicBeePlugin Windows";

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
            LibraryReportsGeneratingPreviewCommandSbText = "Generating preview";
            ApplyingLibraryReportSbText = "Applying LR preset";
            AutoRateCommandSbText = "Auto rating tracks";
            AsrCommandSbText = "Advanced searching and replacing";
            CarCommandSbText = "Calculating average album rating";
            MsrCommandSbText = "Multiple searching and replacing";

            AnotherLrPresetIsRunningSbText = "Another library reports preset is running. Can't proceed!";

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

            EmptyPresetName = "<Empty preset>";


            //Let's determine casing rules for genre category as an example
            ChangeCase.ChangeCaseOptions changeCaseMode = ChangeCase.ChangeCaseOptions.SentenceCase;
            string[] genreCategory = MbApiInterface.Setting_GetFieldName(MetaDataType.GenreCategory).Split(' ');
            if (char.ToUpper(genreCategory[genreCategory.Length - 1][0]) == genreCategory[genreCategory.Length - 1][0])
                changeCaseMode = ChangeCase.ChangeCaseOptions.TitleCase;

            ArtistArtistsName = MbApiInterface.Setting_GetFieldName(MetaDataType.Artist);
            ComposerComposersName = MbApiInterface.Setting_GetFieldName(MetaDataType.Composer);

            DisplayedArtistName = MbApiInterface.MB_GetLocalisation("aSplit.msg.diar", "display {0}:").Replace("{0}", ArtistArtistsName);
            DisplayedArtistName = ChangeCase.ChangeWordsCase(DisplayedArtistName, changeCaseMode,
                null, false, null, null);
            DisplayedArtistName = DisplayedArtistName.Remove(DisplayedArtistName.Length - 1);

            DisplayedComposerName = MbApiInterface.MB_GetLocalisation("aSplit.msg.diar", "display {0}:").Replace("{0}", ComposerComposersName);
            DisplayedComposerName = ChangeCase.ChangeWordsCase(DisplayedComposerName, changeCaseMode,
                null, false, null, null);
            DisplayedComposerName = DisplayedComposerName.Remove(DisplayedComposerName.Length - 1);

            string localizedDisplayedAlbumArtist = MbApiInterface.MB_GetLocalisation("aSplit.msg.diar", "display {0}:");
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
            ExportedFormats = "HTML Document (grouped by albums)|*.htm|HTML Document|*.htm|Simple HTML table|*.htm|Tab delimited text|*.txt|M3U Playlist|*.m3u|CSV file|*.csv|HTML Document (CD Booklet)|*.htm";
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

            SbBrokenPresetRetrievalChain = "Broken preset retrieval chain for LR preset: %%PRESETNAME%%! Check out the preset chain!";

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

            DefaultASRPresetName = "Preset #";

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
            MsgNoAggregateFunctionNothingToSave = "No aggregate function in the table. Nothing to save.";
            MsgPleaseUseGroupingFunction = "Please use <Grouping> function for tags \"" + ArtworkName + "\" and \"" + SequenceNumberName + "\"!";
            MsgAllTags = "ALL TAGS";
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
                "so its possible to automatically update auto rating if playing track is changed. \n" +
                "Also its possible to update auto rating manually for selected tracks or automatically for entire library \n" +
                "on MusicBee startup. ";
            MsgAutoCalculationOfThresholdsDescription = "Auto calculation of thresholds is another way to set up auto ratings. This option allows you to set desirable weights \n" +
                "of auto rating values in your library. Actual weights may differ from desirable values because its not always possible \n" +
                "to satisfy desirable weights (suppose all your tracks have the same \"plays per day\" value, its obvious that there is no way to split \n" +
                "your library into several parts on the basis of \"plays per day\" value). Actual weights are displayed next to desired weights after calculation is made. \n" +
                "Because calculation of thresholds is time consuming operation it cannot be done automatically on any event except for MusicBee startup. ";

            MsgNumberOfPlayedTracks = "Ever played tracks in your library: ";
            MsgIncorrectSumOfWeights = "The sum of all weights must be equal or less than 100%!";
            MsgSum = "Sum: ";
            MsgNumberOfNotRatedTracks = "% of tracks rated as no stars)";
            MsgTracks = " tracks)";
            MsgActualPercent = "% / Act.: ";
            MsgIncorrectPresetName = "Incorrect preset name or duplicated preset names.";

            MsgLrDoYouWantToSaveChangesBeforeClosingTheWindow = "There are unsaved changes. Do you want to save changes before closing the window?";

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

            MsgFirstThreeGroupingFieldsInPreviewTableShouldBe = "First three grouping fields in preview table should be \"" + DisplayedAlbumArtistName + "\", \"" + AlbumTagName + "\" and \"" + ArtworkName + "\" to export to HTML Document (grouped by album)";
            MsgFirstSixGroupingFieldsInPreviewTableShouldBe = "First six grouping fields in preview table should be '" + SequenceNumberName
                + "\", \"" + DisplayedAlbumArtistName + "\", \"" + AlbumTagName + "\", \"" + ArtworkName + "\", \""
                + MbApiInterface.Setting_GetFieldName(MetaDataType.TrackTitle) + "\" and \""
                + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Duration)
                + "' to export to HTML Document (CD Booklet)";

            MsgYouMustImportStandardAsrPresetsFirst = "You must import standard ASR presets first!";

            SbSorting = "Sorting table...";
            SbUpdating = "updating";
            SbReading = "reading";
            SbUpdated = "updated";
            SbRead = "read";
            SbFiles = "file(s)";
            SbItems = "record(s)";
            SbItemNames = SbFiles;

            SbAsrPresetIsApplied = "ASR preset is applied: %%PRESETNAME%%";
            SbAsrPresetsAreApplied = "ASR presets are applied: %%PRESETCOUNT%%";

            SbLrHotkeysAreAssignedIncorrectly = "LR hotkey is assigned incorrectly!";
            SbIncorrectLrFunctionId = "Incorrect LR function ID!";

            CtlDirtyError1sf = string.Empty;
            CtlDirtyError1mf = string.Empty;
            CtlDirtyError2sf = " background file updating operation is running/scheduled. \n" +
                "Preview results may be not accurate. ";
            CtlDirtyError2mf = " background file updating operations is running/scheduled. \n" +
                "Preview results may be not accurate. ";


            MsgMasterBackupIndexIsCorrupted = "Master tag backup index is corrupted! All existing at the moment backups are not available any more in \"Tag history\" command.";
            MsgBackupIsCorrupted = "Backup \"%%FILENAME%%\" is corrupted or is not valid MusicBee backup!";
            MsgFolderDoesntExists = "Folder doesn't exist!";
            MsgSelectOneTrackOnly = "Select one track only!";
            MsgSelectTrack = "Select a track!";
            MsgSelectAtLeast2Tracks = "Select at least 2 tracks to compare!";
            MsgBackupBaselineFileDoesntExist = "Backup baseline file\"%%FILENAME%%\" doesn't exist! Backup tags manually first!";
            MsgThisIsTheBackupOfDifferentLibrary = "This is the backup of different library!";
            MsgCreateBaselineWarning = "When you create the first backup of given library, a backup baseline is created. " +
                "All further backups are incremental relative to baseline. " +
                "If you have changed very much tags incremental backups may become too large. This command will delete ALL incremental backups " +
                "of CURRENT library and will create new backup baseline if you continue. ";

            MsgGiveNameToAsrPreset = "Give a name to preset!";
            MsgAreYouSureYouWantToSaveAsrPreset = "Do you want to save ASR preset named \"%%PRESETNAME%%\"?";
            MsgAreYouSureYouWantToOverwriteAsrPreset = "Do you want to overwrite ASR preset \"%%PRESETNAME%%\"?";
            MsgAreYouSureYouWantToOverwriteRenameAsrPreset = "Do you want to overwrite ASR preset \"%%PRESETNAME%%\", and rename it to \"%%NEWPRESETNAME%%\"?";
            MsgAreYouSureYouWantToDeleteAsrPreset = "Do you want to delete ASR preset \"%%PRESETNAME%%\"?";
            MsgPredefinedPresetsCantBeChanged = "Predefined presets can't be changed. Preset editor will open in read-only mode.\n\n"
                + "Do you want to disable this warning?";
            MsgSavePreset = "Save Preset";
            MsgDeletePreset = "Delete Preset";
            MsgRefreshUi = "Refresh UI!";
            MsgIncorrectReportPresetId = "Incorrect $LR() ID!";

            CtlNewAsrPreset = "<New ASR Preset>";

            CtlAutoLrPresetName = "(Auto preset name)";

            CtlAsrCellTooTip = "Unchecked rows won't be processed\n" +
                "\n" +
                "Shift-click checks/unchecks all rows containing the same tags as in this row";
            CtlAsrAllTagsCellTooTip = "Unchecked rows won't be processed\n" +
                "\n" +
                "Shift-click checks/unchecks all rows containing the same tag\n" +
                "\n" +
                "Ctrl-click adds/removes the tag referred in the row to/from proceeded or preserved tags";
            MsgAsrPresetsUsingAllTagsPseudoTagNameCannotBeAutoApplied = "Can't execute preset %%PRESETNAME%%! ASR presets using %%AllTagsPseudoTagName%% cannot " +
                "be auto-executed, nor can be applied via hotkey!";


            SbAutobackuping = "Autosaving tag backup...";
            SbMovingBackupsToNewFolder = "Moving backups to new folder...";
            SbMakingTagBackup = "Making tag backup...";
            SbRestoringTagsFromBackup = "Restoring tags from backup...";
            SbRenamingMovingBackup = "Renaming/moving backup...";
            SbMovingBackups = "Moving backups...";
            SbDeletingBackups = "Deleting backup(s)...";
            SbTagAutobackupSkipped = "Tag autobackup skipped (no changes since last tag backup)";
            SbCompairingTags = "Comparing tags with baseline... ";

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
            CtlMixedValues = "(Mixed values)";
            CtlMixedValuesSameAsInLibrary = "Same as in library";
            CtlMixedValuesDifferentFromLibrary = "Different from library";

            MsgNotAllowedSymbols = "Only ASCII letters, numbers and symbols - : _ . are allowed!";
            MsgPresetExists = "Preset with ID %%ID%% already exists!";

            CtlWholeCuesheetWillBeReencoded = "IMPORTANT NOTE: ONE OR SEVERAL TRACKS BELONG TO CUESHEET. THE ALL CUESHEET TRACKS WILL BE REENCODED!";

            CtlMSR = "MSR: ";

            CtlUnknown = MbApiInterface.MB_GetLocalisation("dSum.msg.Unknown", "Unknown");

            CtlMixedFilters = "(Mixed filters)";

            MsgFirstSelectWhichFieldYouWantToAssignFunctionIdTo = "First select, which field you want to assign function ID to (leftmost combo-box on function ID line)!";

            //Defaults for controls
            SavedSettings = new SavedSettingsType
            {

                //Lets set initial defaults
                reportsPresets = null,

                smartOperation = true,
                appendSource = false,

                changeCaseFlag = 1,
                useExceptionWords = false,

                useExceptionChars = false,
                useWordSplitters = false,
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
                MessageBox.Show(MbForm, e.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            DontShowShowHiddenWindows = SavedSettings.dontShowShowHiddenWindows;

            if (SavedSettings.windowsSettings == null)
                SavedSettings.windowsSettings = new List<WindowSettingsType>();

            if (SavedSettings.exceptionWords == null || SavedSettings.exceptionWords.Length < 10)
            {
                SavedSettings.exceptionWords = new string[10];
                SavedSettings.exceptionWords[0] = "the a an and or not";
                SavedSettings.exceptionWords[1] = "a al an and as at but by de for in la le mix nor of on or remix the to vs. y ze feat.";
                SavedSettings.exceptionWords[2] = "U2 UB40";
                SavedSettings.exceptionWords[3] = string.Empty;
                SavedSettings.exceptionWords[4] = string.Empty;
                SavedSettings.exceptionWords[5] = string.Empty;
                SavedSettings.exceptionWords[6] = string.Empty;
                SavedSettings.exceptionWords[7] = string.Empty;
                SavedSettings.exceptionWords[8] = string.Empty;
                SavedSettings.exceptionWords[9] = string.Empty;
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
                SavedSettings.exceptionCharsASR = string.Empty;
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
                SavedSettings.appendedText = new string[10];
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
                SavedSettings.addedText = new string[10];
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

            if (SavedSettings.autobackupDirectory == null)
                SavedSettings.autobackupDirectory = "Tag Backups";
            if (!Directory.Exists(GetAutobackupDirectory(SavedSettings.autobackupDirectory)))
                Directory.CreateDirectory(GetAutobackupDirectory(SavedSettings.autobackupDirectory));

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
                //Lets redefine localizable strings

                //MusicBeePlugin localizable strings
                PluginName = "Дополнительные инструменты";
                PluginMenuGroupName = "Дополнительные инструменты";
                PluginDescription = "Плагин добавляет дополнительные инструменты для работы с тегами";

                PluginHelpString = "Справка...";//********
                PluginVersionString = "Версия: ";

                OpenWindowsMenuSectionName = "ОТКРЫТЫЕ ОКНА";
                TagToolsMenuSectionName = "ДОПОЛНИТЕЛЬНЫЕ ИНСТРУМЕНТЫ";
                BackupRestoreMenuSectionName = "АРХИВАЦИЯ И ВОССТАНОВЛЕНИЕ";

                CopyTagCommandName = "Копировать тег...";
                SwapTagsCommandName = "Поменять местами два тега...";
                ChangeCaseCommandName = "Изменить регистр тега...";
                ReencodeTagCommandName = "Изменить кодировку тега...";
                ReencodeTagsCommandName = "Изменить кодировку тегов...";
                LibraryReportsCommandName = "Отчеты по библиотеке...";
                AutoRateCommandName = "Автоматически установить рейтинги...";
                AsrCommandName = "Дополнительный поиск и замена...";
                CarCommandName = "Рассчитать средний рейтинг альбомов...";
                CtCommandName = "Сравнить треки...";
                CopyTagsToClipboardCommandName = "Копировать теги в буфер обмена...";
                PasteTagsFromClipboardCommandName = "Вставить теги из буфера обмена";
                MsrCommandName = "Множественный поиск и замена...";
                ShowHiddenCommandName = "Показать скрытые/восстановить свернутые окна плагина";

                TagToolsHotkeyDescription = "Дополнительные инструменты: ";
                CopyTagCommandDescription = TagToolsHotkeyDescription + "Копировать тег";
                SwapTagsCommandDescription = TagToolsHotkeyDescription + "Поменять местами два тега";
                ChangeCaseCommandDescription = TagToolsHotkeyDescription + "Изменить регистр тега";
                ReencodeTagCommandDescription = TagToolsHotkeyDescription + "Изменить кодировку тега";
                ReencodeTagsCommandDescription = TagToolsHotkeyDescription + "Изменить кодировку тегов";
                LibraryReportsCommandDescription = TagToolsHotkeyDescription + "Отчеты по библиотеке";
                ReportPresetHotkeyDescription = TagToolsHotkeyDescription;
                AutoRateCommandDescription = TagToolsHotkeyDescription + "Автоматически установить рейтинги";
                AsrCommandDescription = TagToolsHotkeyDescription + "Дополнительный поиск и замена";
                AsrHotkeyDescription = TagToolsHotkeyDescription;
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
                LibraryReportsGeneratingPreviewCommandSbText = "Формирование предварительного просмотра";
                ApplyingLibraryReportSbText = "Применение пресета ОБ";
                AutoRateCommandSbText = "Автоматическая установка рейтингов";
                AsrCommandSbText = "Дополнительный поиск и замена";
                CarCommandSbText = "Расчет среднего рейтинга альбомов";
                MsrCommandSbText = "Множественный поиск и замена";

                AnotherLrPresetIsRunningSbText = "Другой отчет ОБ уже запущен. Невозможно применить отчет!";

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

                EmptyPresetName = "<Пустой пресет>";

                AllTagsPseudoTagName = "<ВСЕ ТЕГИ>";

                GenericTagSetName = "Набор тегов";


                SequenceNumberName = "№ п/п";


                //Supported exported file formats
                ExportedFormats = "Документ HTML (по альбомам)|*.htm|Документ HTML|*.htm|Простая таблица HTML|*.htm|Текст, разделенный табуляциями|*.txt|Плейлист M3U|*.m3u|Файл CSV|*.csv|Документ HTML (буклет компакт-диска)|*.htm";
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

                SbBrokenPresetRetrievalChain = "Нарушенная цепочка пресетов для получения списка треков для для пресета ОБ: %%PRESETNAME%%! Проверьте цепочку пресетов!";

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
                MsgNoAggregateFunctionNothingToSave = "В таблице нет ни одной агрегатной функции. Нечего сохранять.";
                MsgPleaseUseGroupingFunction = "Пожалуйста, используйте функцию <Группировка> для тегов \"" + ArtworkName + "\" и \"" + SequenceNumberName + "\"!";
                MsgAllTags = "ВСЕ ТЕГИ";
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

                MsgLrDoYouWantToSaveChangesBeforeClosingTheWindow = "Есть несохраненные изменения. Сохранить изменения, прежде чем закрыть окно?";

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
                    + " чтения файл{а;ов;ов} или {его;их;их} неверного формата.";

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
                    + " чтения файл{а;ов;ов} или {его;их;их} неверного формата.";

                MsgPresetsNotFound = "Не найдены пресеты для установки в ожидаемом каталоге!";

                MsgDeletingConfirmation = "Удалить все стандартные пресеты?";
                MsgNoPresetsDeleted = "Пресеты не были удалены.";
                MsgPresetsWereDeleted = " пресет{;а;ов} был{;и;и} удален{;ы;ы}.";


                MsgNumberOfTagsInTextFileDoesntCorrespondToNumberOfSelectedTracks = "Количество тегов в текстовом файле (%%TEXT-TAG-FILES-COUNT%%)" +
                    " не соответствует количеству выбранных треков (%%SELECTED-FILES-COUNT%%)!";

                MsgClipboardDoesntContainText = "Буфер обмена не содержит текст!";

                MsgClipboardDoesntContainTags = "Буфер обмена не содержит теги!";

                MsgUnknownTagNameInClipboard = "Неизвестное имя тега в буфере обмена: %%TAG-NAME%%!";

                MsgNumberOfTagsInClipboardDoesntCorrespondToNumberOfSelectedTracks = "Количество тегов треков в буфере обмена (%%FILE-TAGS-LENGTH%%)" +
                    " не соответствует количеству выбранных треков (%%SELECTED-FILES-COUNT%%)!";
                MsgDoYouWantToPasteTagsAnyway = " Вставить теги все равно?";
                MsgWrongNumberOfCopiedTags = "Неверное количество скопированных в буфер обмена тегов (%%CLIPBOARD-TAGS-COUNT%%)" +
                    " в строке %%CLIPBOARD-LINE%% текста буфера обмена!";
                MsgMatchTracksByPathQuestion = "Буфер обмена содержит псевдо-тег %%MATCH-TAG-NAME%%. Вставлять теги в соответствии с этим тегом" +
                    " (в противном случае треки для тегов будут выбираться по их порядку отображения)?";
                MsgTracksSelectedMatchedNotMatched = "Выбрано треков: %%SELECTED-TRACKS%%\n" +
                    "Тегов треков в буфере обмена: %%CLIPBOARD-TRACKS%%\n" +
                    "Найдено подходящих треков: %%MATCHED-TRACKS%%\n" +
                    "Не найдено подходящих треков: %%NOT-MATCHED-TRACKS%%\n\n" + 
                    "Вставить теги?";


                MsgFirstThreeGroupingFieldsInPreviewTableShouldBe = "Первые три поля группировок в таблице должны быть \"" + DisplayedAlbumArtistName + "\", '"
                    + AlbumTagName + "\" и \"" + ArtworkName + "\" для того, чтобы экспортировать теги в формат \"Документ HTML (по альбомам)'";
                MsgFirstSixGroupingFieldsInPreviewTableShouldBe = "Первые шесть полей группировок в таблице должны быть \"" + SequenceNumberName
                    + "\", \"" + DisplayedAlbumArtistName + "\", \"" + AlbumTagName + "\", \"" + ArtworkName + "\", \""
                    + MbApiInterface.Setting_GetFieldName(MetaDataType.TrackTitle) + "\" and \""
                    + MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Duration)
                    + "\" для того, чтобы экспортировать теги в формат \"Документ HTML (буклет компакт-диска)\"";

                MsgYouMustImportStandardAsrPresetsFirst = "Сначала надо импортировать стандартные пресеты дополнительного поиска и замены!";

                SbSorting = "Сортировка таблицы...";
                SbUpdating = "запись";
                SbReading = "чтение";
                SbUpdated = "записан(о)";
                SbRead = "прочитан(о)";
                SbFiles = "файл(ов)";
                SbItems = "записей";
                SbItemNames = SbFiles;

                SbAsrPresetIsApplied = "Применен пресет дополнительного поиска и замены: %%PRESETNAME%%";
                SbAsrPresetsAreApplied = "Применены пресеты дополнительного поиска и замены: %%PRESETCOUNT%%";

                SbLrHotkeysAreAssignedIncorrectly = "Комбинация клавиш пресета ОБ назначена некорректно!";
                SbIncorrectLrFunctionId = "Неверный ID функции ОБ!";

                CtlDirtyError1sf = "Работает/запланирована ";
                CtlDirtyError1mf = "Работают/запланированы ";
                CtlDirtyError2sf = " фоновая операция обновления файлов. \n" +
                    "Результаты предварительного просмотра могут быть не точны. ";
                CtlDirtyError2mf = " фоновых операций обновления файлов. \n" +
                    "Результаты предварительного просмотра могут быть не точны. ";


                MsgMasterBackupIndexIsCorrupted = "Основной индекс архива тегов поврежден! Все существующие на данный момент архивы " +
                    "будут не доступны в команде \"История тегов\".";
                MsgBackupIsCorrupted = "Архив \"%%FILENAME%%\" или поврежден, или не является архивом MusicBee!";
                MsgFolderDoesntExists = "Папка не существует!";
                MsgSelectOneTrackOnly = "Выберите только один трек!";
                MsgSelectTrack = "Выберите трек!";
                MsgSelectAtLeast2Tracks = "Выберите по меньшей мере 2 трека для сравнения!";
                MsgBackupBaselineFileDoesntExist = "Файл первоначального опорного архива \"%%FILENAME%%\" не существует! Сначала создайте архив тегов вручную!";
                MsgThisIsTheBackupOfDifferentLibrary = "Это архив другой библиотеки!";
                MsgCreateBaselineWarning = "Когда создается первый архив любой библиотеки, сначала создается полный опорный архив. " +
                    "Все последующие архивы являются разницей с опорным архивом. " +
                    "Если было изменено очень много тегов, то разностные архивы могут стать очень большими. Эта команда удалит " +
                    "ВСЕ разностные архивы ТЕКУЩЕЙ библиотеки и создаст новый опорный архив. ";

                MsgGiveNameToAsrPreset = "Задайте название пресета!";
                MsgAreYouSureYouWantToSaveAsrPreset = "Сохранить пресет дополнительного поиска и замены под названием \"%%PRESETNAME%%\"?";
                MsgAreYouSureYouWantToOverwriteAsrPreset = "Перезаписать пресет дополнительного поиска и замены \"%%PRESETNAME%%\"?";
                MsgAreYouSureYouWantToOverwriteRenameAsrPreset = "Перезаписать пресет дополнительного поиска и замены \"%%PRESETNAME%%\", переименовав его в \"%%NEWPRESETNAME%%\"?";
                MsgAreYouSureYouWantToDeleteAsrPreset = "Удалить пресет дополнительного поиска и замены \"%%PRESETNAME%%\"?";
                MsgPredefinedPresetsCantBeChanged = "Стандартные пресеты нельзя изменять. Редактор пресетов будет открыт в режиме просмотра.\n\n"
                    + "Отключить показ этого предупреждения?";
                MsgSavePreset = "Сохранить пресет";
                MsgDeletePreset = "Удалить пресет";
                MsgRefreshUi = "Обновите панель!";
                MsgIncorrectReportPresetId = "Неверный ID $LR()!";

                CtlNewAsrPreset = "<Новый пресет ДПЗ>";

                CtlAutoLrPresetName = "(Автоматическое имя)";


                CtlAsrCellTooTip = "Неотмеченные строки не будут обработаны\n" +
                    "\n" +
                    "Щелчок при нажатой клавише Shift отметит/снимет отметку во всех строках, содержащих те же теги, что и в этой строке";
                CtlAsrAllTagsCellTooTip = "Неотмеченные строки не будут обработаны\n" +
                    "\n" +
                    "Щелчок при нажатой клавише Shift отметит/снимет отметку во всех строках, содержащих тот же тег\n" +
                    "\n" +
                    "Щелчок при нажатой клавише Ctrl добавит/удалит тег, указанный в строке, к списку/из списка обрабатываемых/исключенных тегов";
                MsgAsrPresetsUsingAllTagsPseudoTagNameCannotBeAutoApplied = "Невозможно применить пресет %%PRESETNAME%%! Пресеты ДПЗ, использующие псевдо-тег %%AllTagsPseudoTagName%%, " +
                    "не могут применяться ни автоматически, ни при помощи комбинации клавиш!";


                SbAutobackuping = "Автосохранение архива тегов...";
                SbMovingBackupsToNewFolder = "Идет перемещение архивов в новую папку...";
                SbMakingTagBackup = "Сохранение архива тегов...";
                SbRestoringTagsFromBackup = "Идет восстановление тегов из архива...";
                SbRenamingMovingBackup = "Идет переименование/перемещение архива...";
                SbMovingBackups = "Идет перемещение архивов...";
                SbDeletingBackups = "Идет удаление архивов...";
                SbTagAutobackupSkipped = "Автоархивирование тегов пропущено (не было изменений с момента последней архивации)";
                SbCompairingTags = "Идет сравнение тегов с основным архивом... ";

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
                CtlMixedValues = "(Разные значения)";
                CtlMixedValuesSameAsInLibrary = "Совпадает с библиотекой";
                CtlMixedValuesDifferentFromLibrary = "Отличается от библиотеки";

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
            PluginHelpFilePath = Path.Combine(PluginsPath, PluginHelpFileName) + "." + Language + ".chm";//********

            if (SavedSettings.autobackupPrefix == null)
                SavedSettings.autobackupPrefix = "Tag Autobackup ";


            if (SavedSettings.reportsPresets == null)
                SavedSettings.reportsPresets = new ReportPreset[0];

            // Let's remove all predefined presets and recreate them from scratch
            int existingPredefinedCount = 0;
            for (int i = 0; i < SavedSettings.reportsPresets.Length; i++)
            {
                if (!SavedSettings.reportsPresets[i].userPreset)
                    existingPredefinedCount++;
            }

            int presetCounter = 0;
            var reportsPresets = new ReportPreset[SavedSettings.reportsPresets.Length - existingPredefinedCount + PredefinedReportPresetCount];
            foreach (var reportPreset in SavedSettings.reportsPresets)
            {
                if (reportPreset.userPreset)
                    reportsPresets[presetCounter++] = reportPreset;
            }


            ReportPreset tempLibraryReportsPreset;

            PresetColumnAttributes[] tempGroupings = new PresetColumnAttributes[3];
            PresetColumnAttributes[] tempFunctions = new PresetColumnAttributes[9];

            string[] tempDestinationTags;
            string[] tempFunctionIds;


            //Library Totals
            tempGroupings[0] = new PresetColumnAttributes(LrFunctionType.Grouping, new string[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.Genre), null, null, false);
            tempGroupings[1] = new PresetColumnAttributes(LrFunctionType.Grouping, new string[] { string.Empty }, DisplayedAlbumArtistName, null, null, false);
            tempGroupings[2] = new PresetColumnAttributes(LrFunctionType.Grouping, new string[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.Album), null, null, false);

            for (int f = 0; f < tempGroupings.Length; f++)
                tempGroupings[f].columnIndices = new int[] { f };


            tempFunctions[0] = new PresetColumnAttributes(LrFunctionType.Count, new string[] { string.Empty }, DisplayedAlbumArtistName, null, null, false);
            tempFunctions[1] = new PresetColumnAttributes(LrFunctionType.Count, new string[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.Genre), null, null, false);
            tempFunctions[2] = new PresetColumnAttributes(LrFunctionType.Count, new string[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.Year), null, null, false);
            tempFunctions[3] = new PresetColumnAttributes(LrFunctionType.Count, new string[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.Album), null, null, false);
            tempFunctions[4] = new PresetColumnAttributes(LrFunctionType.Count, new string[] { string.Empty }, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url), null, null, false);
            tempFunctions[5] = new PresetColumnAttributes(LrFunctionType.Sum, new string[] { string.Empty }, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Duration), null, null, false);
            tempFunctions[6] = new PresetColumnAttributes(LrFunctionType.Sum, new string[] { string.Empty }, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Size), null, null, false);
            tempFunctions[7] = new PresetColumnAttributes(LrFunctionType.Sum, new string[] { string.Empty }, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.PlayCount), null, null, false);
            tempFunctions[8] = new PresetColumnAttributes(LrFunctionType.Sum, new string[] { string.Empty }, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.SkipCount), null, null, false);

            for (int f = 0; f < tempFunctions.Length; f++)
                tempFunctions[f].columnIndices = new int[] { tempGroupings.Length + f };


            tempFunctionIds = new string[tempFunctions.Length];

            tempDestinationTags = new string[tempFunctions.Length];
            for (int i = 0; i < tempDestinationTags.Length; i++)
                tempDestinationTags[i] = NullTagName;

            tempLibraryReportsPreset = new ReportPreset
            {
                groupings = tempGroupings,
                functions = tempFunctions,
                totals = true,
                name = LibraryTotalsPresetName.ToUpper(),
                userPreset = false,

                destinationTags = tempDestinationTags,
                functionIds = tempFunctionIds,
                conditionField = tempGroupings[0].parameterName,
                comparison = Comparison.IsGreaterOrEqual,

                exportedTrackListName = ExportedTrackList,
                fileFormatIndex = LrReportFormat.HtmlDocument
            };

            tempLibraryReportsPreset.sourceTags = new string[tempFunctions.Length];
            for (int i = 0; i < tempFunctions.Length; i++)
                tempLibraryReportsPreset.sourceTags[i] = GetColumnName(tempFunctions[i].parameterName, tempFunctions[i].parameter2Name,
                    tempFunctions[i].functionType, null, false, null, false, false, true);

            reportsPresets[presetCounter++] = tempLibraryReportsPreset;


            //Library Averages
            tempGroupings = new PresetColumnAttributes[3];
            tempFunctions = new PresetColumnAttributes[10];

            tempGroupings[0] = new PresetColumnAttributes(LrFunctionType.Grouping, new string[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.Genre), null, null, false);
            tempGroupings[1] = new PresetColumnAttributes(LrFunctionType.Grouping, new string[] { string.Empty }, DisplayedAlbumArtistName, null, null, false);
            tempGroupings[2] = new PresetColumnAttributes(LrFunctionType.Grouping, new string[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.Album), null, null, false);

            for (int f = 0; f < tempGroupings.Length; f++)
                tempGroupings[f].columnIndices = new int[] { f };


            tempFunctions[0] = new PresetColumnAttributes(LrFunctionType.AverageCount, new string[] { string.Empty },
                MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url), MbApiInterface.Setting_GetFieldName(MetaDataType.Artist), null, false);
            tempFunctions[1] = new PresetColumnAttributes(LrFunctionType.AverageCount, new string[] { string.Empty },
                MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url), MbApiInterface.Setting_GetFieldName(MetaDataType.Album), null, false);
            tempFunctions[2] = new PresetColumnAttributes(LrFunctionType.AverageCount, new string[] { string.Empty },
                MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url), MbApiInterface.Setting_GetFieldName(MetaDataType.Genre), null, false);
            tempFunctions[3] = new PresetColumnAttributes(LrFunctionType.AverageCount, new string[] { string.Empty },
                MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url), MbApiInterface.Setting_GetFieldName(MetaDataType.Year), null, false);
            tempFunctions[4] = new PresetColumnAttributes(LrFunctionType.Average, new string[] { string.Empty },
                MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Bitrate), MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url), null, false);
            tempFunctions[5] = new PresetColumnAttributes(LrFunctionType.Average, new string[] { string.Empty },
                MbApiInterface.Setting_GetFieldName(MetaDataType.Rating), MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url), null, false);
            tempFunctions[6] = new PresetColumnAttributes(LrFunctionType.Average, new string[] { string.Empty },
                MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Duration), MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url), null, false);
            tempFunctions[7] = new PresetColumnAttributes(LrFunctionType.Average, new string[] { string.Empty },
                MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Size), MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url), null, false);
            tempFunctions[8] = new PresetColumnAttributes(LrFunctionType.Average, new string[] { string.Empty },
                MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.PlayCount), MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url), null, false);
            tempFunctions[9] = new PresetColumnAttributes(LrFunctionType.Average, new string[] { string.Empty },
                MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.SkipCount), MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Url), null, false);

            for (int f = 0; f < tempFunctions.Length; f++)
                tempFunctions[f].columnIndices = new int[] { tempGroupings.Length + f };


            tempFunctionIds = new string[tempFunctions.Length];

            tempDestinationTags = new string[tempFunctions.Length];
            for (int i = 0; i < tempDestinationTags.Length; i++)
                tempDestinationTags[i] = NullTagName;

            tempLibraryReportsPreset = new ReportPreset
            {
                groupings = tempGroupings,
                functions = tempFunctions,
                totals = true,
                name = LibraryAveragesPresetName.ToUpper(),
                userPreset = false,

                destinationTags = tempDestinationTags,
                functionIds = tempFunctionIds,
                conditionField = tempGroupings[0].parameterName,
                comparison = Comparison.IsGreaterOrEqual,

                exportedTrackListName = ExportedTrackList,
                fileFormatIndex = LrReportFormat.HtmlDocument
            };

            tempLibraryReportsPreset.sourceTags = new string[tempFunctions.Length];
            for (int i = 0; i < tempFunctions.Length; i++)
                tempLibraryReportsPreset.sourceTags[i] = GetColumnName(tempFunctions[i].parameterName, tempFunctions[i].parameter2Name,
                    tempFunctions[i].functionType, null, false, null, false, false, true);

            reportsPresets[presetCounter++] = tempLibraryReportsPreset;


            //CD Booklet
            tempGroupings = new PresetColumnAttributes[6];
            tempFunctions = new PresetColumnAttributes[0];

            tempGroupings[0] = new PresetColumnAttributes(LrFunctionType.Grouping, new string[] { null }, SequenceNumberName, null, null, false);
            tempGroupings[1] = new PresetColumnAttributes(LrFunctionType.Grouping, new string[] { string.Empty }, DisplayedAlbumArtistName, null, null, false);
            tempGroupings[2] = new PresetColumnAttributes(LrFunctionType.Grouping, new string[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.Album), null, null, false);
            tempGroupings[3] = new PresetColumnAttributes(LrFunctionType.Grouping, new string[] { null }, MbApiInterface.Setting_GetFieldName(MetaDataType.Artwork), null, null, false);
            tempGroupings[4] = new PresetColumnAttributes(LrFunctionType.Grouping, new string[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.TrackTitle), null, null, false);
            tempGroupings[5] = new PresetColumnAttributes(LrFunctionType.Grouping, new string[] { string.Empty }, MbApiInterface.Setting_GetFieldName((MetaDataType)FilePropertyType.Duration), null, null, false);

            for (int f = 0; f < tempGroupings.Length; f++)
                tempGroupings[f].columnIndices = new int[] { f };


            tempFunctionIds = new string[tempFunctions.Length];

            tempDestinationTags = new string[tempFunctions.Length];
            for (int i = 0; i < tempDestinationTags.Length; i++)
                tempDestinationTags[i] = NullTagName;

            tempLibraryReportsPreset = new ReportPreset
            {
                groupings = tempGroupings,
                functions = tempFunctions,
                totals = false,
                name = CDBookletPresetName.ToUpper(),
                userPreset = false,

                sourceTags = new string[0],
                destinationTags = tempDestinationTags,
                functionIds = tempFunctionIds,
                conditionField = tempGroupings[0].parameterName,
                comparison = Comparison.IsGreaterOrEqual,

                exportedTrackListName = ExportedTrackList,
                fileFormatIndex = LrReportFormat.HtmlDocumentCdBooklet
            };

            reportsPresets[presetCounter++] = tempLibraryReportsPreset;


            //Albums and Tracks
            tempGroupings = new PresetColumnAttributes[5];
            tempFunctions = new PresetColumnAttributes[0];

            tempGroupings[0] = new PresetColumnAttributes(LrFunctionType.Grouping, new string[] { string.Empty }, DisplayedAlbumArtistName, null, null, false);
            tempGroupings[1] = new PresetColumnAttributes(LrFunctionType.Grouping, new string[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.Album), null, null, false);
            tempGroupings[2] = new PresetColumnAttributes(LrFunctionType.Grouping, new string[] { null }, MbApiInterface.Setting_GetFieldName(MetaDataType.Artwork), null, null, false);
            tempGroupings[3] = new PresetColumnAttributes(LrFunctionType.Grouping, new string[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.TrackNo), null, null, false);
            tempGroupings[4] = new PresetColumnAttributes(LrFunctionType.Grouping, new string[] { string.Empty }, MbApiInterface.Setting_GetFieldName(MetaDataType.TrackTitle), null, null, false);

            for (int f = 0; f < tempGroupings.Length; f++)
                tempGroupings[f].columnIndices = new int[] { f };


            tempFunctionIds = new string[tempFunctions.Length];

            tempDestinationTags = new string[tempFunctions.Length];
            for (int i = 0; i < tempDestinationTags.Length; i++)
                tempDestinationTags[i] = NullTagName;

            tempLibraryReportsPreset = new ReportPreset
            {
                groupings = tempGroupings,
                functions = tempFunctions,
                totals = false,
                name = AlbumsAndTracksPresetName.ToUpper(),
                userPreset = false,

                sourceTags = new string[0],
                destinationTags = tempDestinationTags,
                functionIds = tempFunctionIds,
                conditionField = tempGroupings[0].parameterName,
                comparison = Comparison.IsGreaterOrEqual,

                exportedTrackListName = ExportedTrackList,
                fileFormatIndex = LrReportFormat.HtmlDocumentByAlbums
            };

            reportsPresets[presetCounter++] = tempLibraryReportsPreset;


            foreach (var reportPreset in reportsPresets) //******* remove later !!!
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

            SavedSettings.reportsPresets = reportsPresets;


            //Lets reset invalid defaults for controls
            if (SavedSettings.closeShowHiddenWindows == 0)
                SavedSettings.closeShowHiddenWindows = 1;

            if (SavedSettings.changeCaseFlag == 0) SavedSettings.changeCaseFlag = 1;

            if (SavedSettings.defaultRating == 0) SavedSettings.defaultRating = 5;

            if (SavedSettings.numberOfTagsToRecalculate == 0) SavedSettings.numberOfTagsToRecalculate = 100;


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
                List<string> tagNameList = new List<string>();
                FillListByTagNames(tagNameList, false, false, false, false, false, false);
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

            EmptyButton = new Button();
            EmptyForm = new Form(); //For themed bitmaps disposing control

            MusicName = MbApiInterface.MB_GetLocalisation("Main.tree.Music", "Music");

            TagToolsSubmenu = (ToolStripMenuItem)MbApiInterface.MB_AddMenuItem("mnuTools/[1]" + PluginMenuGroupName, null, null);
            if (!SavedSettings.dontShowContextMenu)
                TagToolsContextSubmenu = (ToolStripMenuItem)MbApiInterface.MB_AddMenuItem("context.Main/" + PluginMenuGroupName, null, null);


            About.PluginInfoVersion = PluginInfoVersion;
            About.Name = PluginName;
            About.Description = PluginDescription;
            About.Author = "boroda";
            About.TargetApplication = string.Empty;   // current only applies to artwork, lyrics or instant messenger name that appears in the provider drop down selector or target Instant Messenger
            About.Type = PluginType.General;
            About.VersionMajor = (short)System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major;
            About.VersionMinor = (short)System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor;
            About.Revision = (short)System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build; // Autogenerated, number of days since 2000-01-01 at build time
            About.MinInterfaceVersion = 39;
            About.MinApiRevision = 51;
            About.ReceiveNotifications = (ReceiveNotificationFlags.PlayerEvents | ReceiveNotificationFlags.TagEvents);
            About.ConfigurationPanelHeight = 0;   // height in pixels that musicbee should reserve in a panel for config settings. When set, a handle to an empty panel will be passed to the Configure function

            int pluginBuildTime = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision;

            PluginVersion = PluginVersionString + About.VersionMajor + "." + About.VersionMinor + "." + About.Revision + "." + pluginBuildTime;
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

            PluginSettings tagToolsForm = new PluginSettings(this);
            PluginWindowTemplate.Display(tagToolsForm, true);

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
            PeriodicUI_RefreshTimer?.Dispose();
            PeriodicUI_RefreshTimer = null;

            if (PeriodicAutobackupTimer != null)
            {
                PeriodicAutobackupTimer.Dispose();
                PeriodicAutobackupTimer = null;
            }



            lock (OpenedForms)
            {
                PluginClosing = true;

                for (int i = OpenedForms.Count - 1; i >= 0; i--)
                {
                    OpenedForms[i].backgroundTaskIsCanceled = true;
                    OpenedForms[i].Close();
                }
            }


            //Let's dispose all unused bitmaps
            FormThemedBitmapsRelease(EmptyForm);


            LibraryReportsCommandForAutoApplying?.Dispose();
            LibraryReportsCommandForHotkeys?.Dispose();
            LibraryReportsCommandForFunctionIds?.Dispose();

            EmptyButton?.Dispose();
            EmptyForm?.Dispose();

            if (!Uninstalled)
                SaveSettings();
        }

        // uninstall this plugin - clean up any persisted files
        public void Uninstall()
        {
            //Lets delete backups
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


            //Lets delete settings file
            if (File.Exists(PluginSettingsFilePath))
            {
                File.Delete(PluginSettingsFilePath);
            }


            //Lets delete working presets files
            if (Directory.Exists(PresetsPath))
            {
                Directory.Delete(PresetsPath, true);
            }


            //Lets try to delete predefined presets files (this can't be done by plugin if it's
            // installed to "C:\Program Files (x86)\MusicBee\Plugins" folder)
            string presetsPath = Path.Combine(PluginsPath, AsrPresetsDirectory);

            try
            {
                if (Directory.Exists(presetsPath))
                {
                    Directory.Delete(presetsPath, true);
                }
            }
            catch { };

            Uninstalled = true;
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
                    // perform startup initialization

                    //LR & ASR init
                    InitLR();
                    InitASR();


                    //Let's create plugin main and context menu items
                    MbForm.Invoke((MethodInvoker)delegate { addPluginContextMenuItems(); addPluginMenuItems(); });


                    PeriodicUI_RefreshTimer = new System.Threading.Timer(regularUI_Refresh, null, RefreshUI_Delay, RefreshUI_Delay);

                    DelayedStatusbarTextClearingDelegate = DelayedStatusbarTextClearing;

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
                                MessageBox.Show(MbForm, MsgMasterBackupIndexIsCorrupted, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                        using (AutoRate tagToolsForm = new AutoRate(this))
                        {
                            tagToolsForm.switchOperation(tagToolsForm.onStartup, EmptyButton, EmptyButton, EmptyButton, EmptyButton, true, null);
                        }
                    }
                    if (SavedSettings.calculateAlbumRatingAtStartUp)
                    {
                        using (CalculateAverageAlbumRating tagToolsForm = new CalculateAverageAlbumRating(this))
                        {
                            tagToolsForm.calculateAlbumRatingForAllTracks();
                        }
                    }

                    MissingArtwork = Resources.missing_artwork;
                    DefaultArtwork = MissingArtwork;


                    //Let's prepare themed bitmaps for controls
                    prepareThemedBitmapsAndColors();


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
                            AsrAutoApplyPresets(sourceFileUrl, this);
                    }

                    if (SavedSettings.autoRateOnTrackProperties)
                    {
                        AutoRate.AutoRateLive(this, sourceFileUrl);
                    }

                    NumberOfTagChanges++;


                    if (!SavedSettings.dontShowBackupRestore && SavedSettings.dontSkipAutobackupsIfOnlyPlayCountsChanged)
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
                            AsrAutoApplyPresets(sourceFileUrl, this);
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
                            AsrAutoApplyPresets(sourceFileUrl, this);
                    }

                    if (!SavedSettings.dontShowCAR)
                    {
                        if (SavedSettings.calculateAlbumRatingAtTagsChanged)
                            CalculateAverageAlbumRating.CalculateAlbumRatingForAlbum(this, sourceFileUrl);
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
                            AsrAutoApplyPresets(sourceFileUrl, this);
                    }

                    NumberOfTagChanges++;


                    if (!SavedSettings.dontShowBackupRestore)
                    {
                        UpdateTrackForBackup(sourceFileUrl);
                    }

                    break;
            }


            //Auto-execute library reports
            if (NumberOfTagChanges >= SavedSettings.numberOfTagsToRecalculate)
                NumberOfTagChanges = -1;

            if (SavedSettings.recalculateOnNumberOfTagsChanges && NumberOfTagChanges == -1)
                AutoApplyReportPresets();
        }
        #endregion

        #region Plugin helper methods
        internal void prepareThemedBitmapsAndColors()
        {
            //Skin controls
            const float AccentBackWeight = 0.70f;
            const float LightDimmedWeight = 0.90f;
            const float DimmedWeight = 0.65f;
            const float DeepDimmedWeight = 0.32f;
            const float ScrollBarsForeWeight = 0.75f;//***

            InputPanelForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanel, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));
            InputPanelBackColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanel, ElementState.ElementStateDefault, ElementComponent.ComponentBackground));
            InputPanelBorderColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanel, ElementState.ElementStateDefault, ElementComponent.ComponentBorder));

            InputControlForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));
            InputControlBackColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentBackground));
            InputControlBorderColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentBorder));

            if (!SavedSettings.dontUseSkinColors)
            {
                AccentColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanel, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));

                DimmedAccentColor = GetWeightedColor(AccentColor, InputPanelBackColor, DimmedWeight);
                DeepDimmedAccentColor = GetWeightedColor(AccentColor, InputPanelBackColor, DeepDimmedWeight);

                FormForeColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanel, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));
                FormBackColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanel, ElementState.ElementStateDefault, ElementComponent.ComponentBackground));
                FormBorderColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanel, ElementState.ElementStateDefault, ElementComponent.ComponentBorder));

                InputControlDimmedForeColor = GetWeightedColor(InputControlForeColor, InputPanelBackColor, DimmedWeight);
                InputControlDimmedBackColor = GetWeightedColor(InputPanelBackColor, FormBackColor, DimmedWeight);


                //SKINNING BUTTONS (ESPECIALLY DISABLED BUTTONS)
                //Below: (SkinElement)2 - BUTTON??? enum code //****

                //Back colors
                int buttonBackCode = MbApiInterface.Setting_GetSkinElementColour((SkinElement)2, ElementState.ElementStateDefault, ElementComponent.ComponentBackground);
                Color buttonBackColor;

                //const float ButtonBackgroundWeight = LightDimmedWeight;
                const float ButtonBackgroundWeight = 1;
                if (buttonBackCode == 0) //Unsupported by older API
                    //buttonBackColor = GetWeightedColor(Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinTrackAndArtistPanel, ElementState.ElementStateDefault, ElementComponent.ComponentBackground)), AccentColor, ButtonBackgroundWeight);
                    buttonBackColor = GetWeightedColor(FormBackColor, AccentColor, ButtonBackgroundWeight);
                else if (buttonBackCode == -1) //Windows color scheme
                    buttonBackColor = SystemColors.Control;
                else
                    buttonBackColor = Color.FromArgb(buttonBackCode);

                Color buttonBackLightDimmedAccentColor = GetWeightedColor(buttonBackColor, AccentColor, LightDimmedWeight);
                Color buttonBackDimmedAccentColor = GetWeightedColor(buttonBackColor, AccentColor, DimmedWeight);
                Color buttonBackDeepDimmedAccentColor = GetWeightedColor(buttonBackColor, AccentColor, DeepDimmedWeight);


                ButtonBackColor = buttonBackColor;
                ButtonDisabledBackColor = ButtonBackColor;

                const float MouseOverButtonBackColor = 0.535f;
                ButtonMouseOverBackColor = GetWeightedColor(ButtonBackColor, FormBackColor, MouseOverButtonBackColor);

                //***
                //float avgForeBrightness = GetAverageBrightness(_buttonBackColor);
                //float avgBackBrightness = GetAverageBrightness(_buttonBackColor);
                //if (Math.Abs(avgForeBrightness - avgBackBrightness) < 0.5f)
                //{
                //    if (avgBackBrightness < 0.5f)
                //        _buttonDisabledBackColor = GetWeightedColor(_buttonBackColor, Color.Black, 0.6f);
                //    else
                //        _buttonDisabledBackColor = GetWeightedColor(_buttonBackColor, Color.White, 0.6f);
                //}
                //***


                //Fore colors
                int buttonForeCode = MbApiInterface.Setting_GetSkinElementColour((SkinElement)2, ElementState.ElementStateDefault, ElementComponent.ComponentForeground);
                Color buttonForeColor;

                if (buttonForeCode == 0) //Unsupported by older API
                    buttonForeColor = AccentColor;
                else if (buttonForeCode == -1) //Windows color scheme
                    buttonForeColor = SystemColors.ControlText;
                else
                    buttonForeColor = Color.FromArgb(buttonForeCode);

                Color buttonLightForeDimmedColor = GetWeightedColor(buttonForeColor, ButtonBackColor, LightDimmedWeight);
                Color buttonForeDimmedColor = GetWeightedColor(buttonForeColor, ButtonBackColor, DimmedWeight);
                Color buttonForeDeepDimmedColor = GetWeightedColor(buttonForeColor, ButtonBackColor, DeepDimmedWeight);

                ButtonForeColor = buttonForeColor;
                ButtonDisabledForeColor = GetWeightedColor(ButtonForeColor, ButtonDisabledBackColor, 0.5f);


                //Border colors
                int buttonBorderCode = MbApiInterface.Setting_GetSkinElementColour((SkinElement)2, ElementState.ElementStateDefault, ElementComponent.ComponentBorder);
                Color buttonBorderColor;

                if (buttonBorderCode == 0) //Unsupported by older API
                    buttonBorderColor = Color.FromArgb(MbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanel, ElementState.ElementStateDefault, ElementComponent.ComponentBorder));
                else if (buttonBorderCode == -1) //Windows color scheme
                    buttonBorderColor = SystemColors.ControlText;
                else
                    buttonBorderColor = Color.FromArgb(buttonBorderCode);

                ButtonBorderColor = buttonBorderColor;
                ButtonFocusedBorderColor = GetWeightedColor(ButtonBorderColor, ButtonForeColor);


                ControlHighlightBackColor = buttonBackLightDimmedAccentColor;
                ControlHighlightForeColor = buttonForeColor;

                float avgForeBrightness = GetAverageBrightness(ControlHighlightForeColor);
                float avgBackBrightness = GetAverageBrightness(ControlHighlightBackColor);
                if (Math.Abs(avgForeBrightness - avgBackBrightness) < 0.35f)
                {
                    if (avgForeBrightness < 0.5f)
                        ControlHighlightForeColor = GetWeightedColor(ControlHighlightForeColor, Color.Black, 0.4f);
                    else
                        ControlHighlightForeColor = GetWeightedColor(ControlHighlightForeColor, Color.White, 0.4f);
                }
            }
            else
            {
                //Color windowsAccentColor = GetWindowColorizationColor(true);

                Color backColorNotSkinned = SystemColors.Control;
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
            }



            ScrollBarBackColor = InputControlBackColor;
            ScrollBarBorderColor = DeepDimmedAccentColor;
            NarrowScrollBarBackColor = InputControlBackColor;
            ScrollBarThumbAndSpansForeColor = GetWeightedColor(AccentColor, InputPanelBackColor, ScrollBarsForeWeight);//***
            //ScrollBarThumbAndSpansBackColor = InputPanelBackColor;
            //ScrollBarThumbAndSpansBorderColor = InputPanelBackColor;


            Color sampleColor = SystemColors.Highlight;
            DimmedHighlight = GetHighlightColor(sampleColor, AccentColor, FormBackColor);


            //Setting default & making themed colors
            HighlightColor = Color.Red;
            UntickedColor = AccentColor;

            TickedColor = GetHighlightColor(HighlightColor, sampleColor, FormBackColor, 0.5f);


            //Making themed bitmaps
            var scalingSampleForm = new PluginSampleWindow(this);
            scalingSampleForm.dontShowForm = true;
            scalingSampleForm.Show();
            float dpiScaling = scalingSampleForm.dpiScaling;
            float buttonHeightDpiFontScaling = scalingSampleForm.getButtonHeightDpiFontScaling();
            scalingSampleForm.Dispose();
            scalingSampleForm = null;


            //Splitter is invisible by default. Lets draw it.
            SplitterColor = GetWeightedColor(SystemColors.Desktop, AccentColor, 0.8f);//***

            Bitmap oldBitmap;

            if (!SavedSettings.dontUseSkinColors) //It's in case if skinned & not skinned buttons use different flat styles
            {
                int scaledPx = (int)Math.Round(dpiScaling);
                SbBorderWidth = 1; //Units: px (not yet DPI scaled, scaling will done below)

                int scrollBarImagesWidth = ControlsTools.GetCustomScrollBarInitialWidth(dpiScaling, 0);
                int comboBoxDownArrowSize = (int)Math.Round(dpiScaling * 1.238f * Plugin.ScrollBarWidth);

                SbBorderWidth *= scaledPx; //HERE IS DPI scaling of SbBorderWidth

                int size = (int)Math.Round(17f * buttonHeightDpiFontScaling);
                int wideHeight = (int)Math.Round(15f * buttonHeightDpiFontScaling);
                int wideWidth = (int)Math.Round(30f * buttonHeightDpiFontScaling);
                int smallSize = (int)Math.Round(15f * buttonHeightDpiFontScaling);

                oldBitmap = DownArrowComboBoxImage;
                DownArrowComboBoxImage = GetSolidImageByBitmapMask(ScrollBarThumbAndSpansForeColor, Resources.down_arrow_combobox_b,
                    comboBoxDownArrowSize, comboBoxDownArrowSize);
                ThemedBitmapAddRef(EmptyForm, oldBitmap, DownArrowComboBoxImage);

                oldBitmap = UpArrowImage;
                UpArrowImage = GetSolidImageByBitmapMask(ScrollBarThumbAndSpansForeColor, Resources.up_arrow_b,
                    scrollBarImagesWidth, scrollBarImagesWidth);
                ThemedBitmapAddRef(EmptyForm, oldBitmap, UpArrowImage);
                oldBitmap = DownArrowImage;
                DownArrowImage = MirrorBitmap(UpArrowImage, true);
                ThemedBitmapAddRef(EmptyForm, oldBitmap, DownArrowImage);

                //EITHER:
                oldBitmap = ThumbMiddleVerticalImage;
                ThumbMiddleVerticalImage = GetSolidImageByBitmapMask(ScrollBarThumbAndSpansForeColor, Resources.thumb_middle_vertical_c,
                    scrollBarImagesWidth - 3 * scaledPx, Resources.thumb_middle_vertical_c.Height, //Here middle thumb image is without right transparent part
                    true, InterpolationMode.NearestNeighbor);
                ThemedBitmapAddRef(EmptyForm, oldBitmap, ThumbMiddleVerticalImage);


                //OR:
                //ThumbTopImage = GetSolidImageByBitmapMask(_scrollBarThumbAndSpansForeColor, Resources.thumb_top_b,
                //    scrollBarImagesWidth, (int)Math.Round(_dpiScaling * scrollBarImagesWidth * Resources.thumb_top_b.Height / Resources.thumb_top_b.Width));
                //ThemedBitmapAddRef(EmptyForm, ThumbTopImage);

                //ThumbMiddleImage = GetSolidImageByBitmapMask(_scrollBarThumbAndSpansForeColor, Resources.thumb_middle_b,
                //    scrollBarImagesWidth, (int)Math.Round(_dpiScaling * scrollBarImagesWidth * Resources.thumb_middle_b.Height / Resources.thumb_middle_b.Width));
                //ThemedBitmapAddRef(EmptyForm, ThumbMiddleImage);

                //ThumbBottomImage = MirrorBitmap(ThumbTopImage, true);
                //ThemedBitmapAddRef(EmptyForm, ThumbBottomImage);


                oldBitmap = LeftArrowImage;
                LeftArrowImage = GetSolidImageByBitmapMask(ScrollBarThumbAndSpansForeColor, Resources.left_arrow_b, scrollBarImagesWidth, scrollBarImagesWidth);
                ThemedBitmapAddRef(EmptyForm, oldBitmap, LeftArrowImage);
                oldBitmap = RightArrowImage;
                RightArrowImage = MirrorBitmap(LeftArrowImage, false);
                ThemedBitmapAddRef(EmptyForm, oldBitmap, RightArrowImage);

                //EITHER:
                oldBitmap = ThumbMiddleHorizontalImage;
                ThumbMiddleHorizontalImage = GetSolidImageByBitmapMask(ScrollBarThumbAndSpansForeColor, Resources.thumb_middle_horizontal_c,
                    Resources.thumb_middle_horizontal_c.Width, scrollBarImagesWidth - 3 * scaledPx, //Here middle thumb image is without bottom transparent part
                    true, InterpolationMode.NearestNeighbor);
                ThemedBitmapAddRef(EmptyForm, oldBitmap, ThumbMiddleHorizontalImage);


                //OR:
                //ThumbLeftImage = GetSolidImageByBitmapMask(_scrollBarThumbAndSpansForeColor, Resources.thumb_left_b,
                //    scrollBarImagesWidth, (int)Math.Round(_dpiScaling * scrollBarImagesWidth * Resources.thumb_left_b.Height / Resources.thumb_left_b.Width));
                //ThemedBitmapAddRef(EmptyForm, ThumbLeftImage);

                //ThumbMiddleImage = GetSolidImageByBitmapMask(_scrollBarThumbAndSpansForeColor, Resources.thumb_middle_b,
                //    scrollBarImagesWidth, (int)Math.Round(_dpiScaling * scrollBarImagesWidth * Resources.thumb_middle_b.Height / Resources.thumb_middle_b.Width));
                //ThemedBitmapAddRef(EmptyForm, ThumbMiddleImage);

                //ThumbRightImage = MirrorBitmap(ThumbLeftImage, true);
                //ThemedBitmapAddRef(EmptyForm, ThumbRightImage);


                oldBitmap = ButtonRemoveImage;
                ButtonRemoveImage = GetSolidImageByBitmapMask(ButtonForeColor, Resources.uncheck_mark,
                    size, size);
                ThemedBitmapAddRef(EmptyForm, oldBitmap, ButtonRemoveImage);
                oldBitmap = ButtonSetImage;
                ButtonSetImage = GetSolidImageByBitmapMask(ButtonForeColor, Resources.check_mark,
                    size, size);
                ThemedBitmapAddRef(EmptyForm, oldBitmap, ButtonSetImage);
                oldBitmap = WarningWide;
                WarningWide = ScaleBitmap(Resources.warning_wide, PixelFormat.Format32bppArgb, InterpolationMode.HighQualityBicubic,
                    wideWidth, wideHeight);
                ThemedBitmapAddRef(EmptyForm, oldBitmap, WarningWide);
                oldBitmap = Warning;
                Warning = ScaleBitmap(Resources.warning, PixelFormat.Format32bppArgb, InterpolationMode.HighQualityBicubic,
                    smallSize, smallSize);
                ThemedBitmapAddRef(EmptyForm, oldBitmap, Warning);
                oldBitmap = Gear;
                Gear = GetSolidImageByBitmapMask(ButtonForeColor, Resources.gear, size, size);
                ThemedBitmapAddRef(EmptyForm, oldBitmap, Gear);
            }
            else
            {
                SbBorderWidth = 0; //Units: px; scroll bars not skinned

                int size = (int)Math.Round(20f * ((buttonHeightDpiFontScaling - 1) * 1.4f + 1));
                int wideHeight = (int)Math.Round(15f * buttonHeightDpiFontScaling);
                int wideWidth = (int)Math.Round(30f * buttonHeightDpiFontScaling);
                int smallSize = (int)Math.Round(15f * buttonHeightDpiFontScaling);


                UpArrowImage = null;
                DownArrowImage = null;

                ThumbMiddleVerticalImage = null;


                oldBitmap = ButtonRemoveImage;
                ButtonRemoveImage = GetSolidImageByBitmapMask(ButtonForeColor, Resources.uncheck_mark,
                    size, size);
                ThemedBitmapAddRef(EmptyForm, oldBitmap, ButtonRemoveImage);
                oldBitmap = ButtonSetImage;
                ButtonSetImage = GetSolidImageByBitmapMask(ButtonForeColor, Resources.check_mark,
                    size, size);
                ThemedBitmapAddRef(EmptyForm, oldBitmap, ButtonSetImage);
                oldBitmap = WarningWide;
                WarningWide = ScaleBitmap(Resources.warning_wide, PixelFormat.Format32bppArgb, InterpolationMode.HighQualityBicubic,
                    wideWidth, wideHeight);
                ThemedBitmapAddRef(EmptyForm, oldBitmap, WarningWide);
                oldBitmap = Warning;
                Warning = ScaleBitmap(Resources.warning, PixelFormat.Format32bppArgb, InterpolationMode.HighQualityBicubic,
                    smallSize, smallSize);
                ThemedBitmapAddRef(EmptyForm, oldBitmap, Warning);
                oldBitmap = Gear;
                Gear = GetSolidImageByBitmapMask(ButtonForeColor, Resources.gear,
                    size, size);
                ThemedBitmapAddRef(EmptyForm, oldBitmap, Gear);
            }


            //ASR
            int markSize = (int)Math.Round(15f * buttonHeightDpiFontScaling);

            oldBitmap = CheckedState;
            CheckedState = GetSolidImageByBitmapMask(AccentColor, Resources.check_mark, markSize, markSize);
            ThemedBitmapAddRef(EmptyForm, oldBitmap, CheckedState);
            oldBitmap = UncheckedState;
            UncheckedState = GetSolidImageByBitmapMask(GetWeightedColor(AccentColor, FormBackColor, AccentBackWeight), Resources.uncheck_mark, markSize, markSize);
            ThemedBitmapAddRef(EmptyForm, oldBitmap, UncheckedState);

            int pictureSize = (int)Math.Round(19f * buttonHeightDpiFontScaling);

            oldBitmap = Search;
            Search = GetSolidImageByBitmapMask(AccentColor, Resources.search, pictureSize, pictureSize);
            ThemedBitmapAddRef(EmptyForm, oldBitmap, Search);


            oldBitmap = AutoAppliedPresetsAccent;
            AutoAppliedPresetsAccent = GetSolidImageByBitmapMask(AccentColor, Resources.auto_applied_presets, pictureSize, pictureSize);
            ThemedBitmapAddRef(EmptyForm, oldBitmap, AutoAppliedPresetsAccent);
            oldBitmap = AutoAppliedPresetsDimmed;
            AutoAppliedPresetsDimmed = GetSolidImageByBitmapMask(GetWeightedColor(AccentColor, FormBackColor, DeepDimmedWeight), Resources.auto_applied_presets, pictureSize, pictureSize);
            ThemedBitmapAddRef(EmptyForm, oldBitmap, AutoAppliedPresetsDimmed);

            oldBitmap = PredefinedPresetsAccent;
            PredefinedPresetsAccent = GetSolidImageByBitmapMask(AccentColor, Resources.predefined_presets, pictureSize, pictureSize);
            ThemedBitmapAddRef(EmptyForm, oldBitmap, PredefinedPresetsAccent);
            oldBitmap = PredefinedPresetsDimmed;
            PredefinedPresetsDimmed = GetSolidImageByBitmapMask(GetWeightedColor(AccentColor, FormBackColor, DeepDimmedWeight), Resources.predefined_presets, pictureSize, pictureSize);
            ThemedBitmapAddRef(EmptyForm, oldBitmap, PredefinedPresetsDimmed);

            oldBitmap = CustomizedPresetsAccent;
            CustomizedPresetsAccent = GetSolidImageByBitmapMask(AccentColor, Resources.customized_presets, pictureSize, pictureSize);
            ThemedBitmapAddRef(EmptyForm, oldBitmap, CustomizedPresetsAccent);
            oldBitmap = CustomizedPresetsDimmed;
            CustomizedPresetsDimmed = GetSolidImageByBitmapMask(GetWeightedColor(AccentColor, FormBackColor, DeepDimmedWeight), Resources.customized_presets, pictureSize, pictureSize);
            ThemedBitmapAddRef(EmptyForm, oldBitmap, CustomizedPresetsDimmed);

            oldBitmap = UserPresetsAccent;
            UserPresetsAccent = GetSolidImageByBitmapMask(AccentColor, Resources.user_presets, pictureSize, pictureSize);
            ThemedBitmapAddRef(EmptyForm, oldBitmap, UserPresetsAccent);
            oldBitmap = UserPresetsDimmed;
            UserPresetsDimmed = GetSolidImageByBitmapMask(GetWeightedColor(AccentColor, FormBackColor, DeepDimmedWeight), Resources.user_presets, pictureSize, pictureSize);
            ThemedBitmapAddRef(EmptyForm, oldBitmap, UserPresetsDimmed);

            oldBitmap = PlaylistPresetsAccent;
            PlaylistPresetsAccent = GetSolidImageByBitmapMask(AccentColor, Resources.playlist_presets, pictureSize, pictureSize);
            ThemedBitmapAddRef(EmptyForm, oldBitmap, PlaylistPresetsAccent);
            oldBitmap = PlaylistPresetsDimmed;
            PlaylistPresetsDimmed = GetSolidImageByBitmapMask(GetWeightedColor(AccentColor, FormBackColor, DeepDimmedWeight), Resources.playlist_presets, pictureSize, pictureSize);
            ThemedBitmapAddRef(EmptyForm, oldBitmap, PlaylistPresetsDimmed);

            oldBitmap = FunctionIdPresetsAccent;
            FunctionIdPresetsAccent = GetSolidImageByBitmapMask(AccentColor, Resources.function_id_presets, pictureSize, pictureSize);
            ThemedBitmapAddRef(EmptyForm, oldBitmap, FunctionIdPresetsAccent);
            oldBitmap = FunctionIdPresetsDimmed;
            FunctionIdPresetsDimmed = GetSolidImageByBitmapMask(GetWeightedColor(AccentColor, FormBackColor, DeepDimmedWeight), Resources.function_id_presets, pictureSize, pictureSize);
            ThemedBitmapAddRef(EmptyForm, oldBitmap, FunctionIdPresetsDimmed);

            oldBitmap = HotkeyPresetsAccent;
            HotkeyPresetsAccent = GetSolidImageByBitmapMask(AccentColor, Resources.hotkey_presets, pictureSize, pictureSize);
            ThemedBitmapAddRef(EmptyForm, oldBitmap, HotkeyPresetsAccent);
            oldBitmap = HotkeyPresetsDimmed;
            HotkeyPresetsDimmed = GetSolidImageByBitmapMask(GetWeightedColor(AccentColor, FormBackColor, DeepDimmedWeight), Resources.hotkey_presets, pictureSize, pictureSize);
            ThemedBitmapAddRef(EmptyForm, oldBitmap, HotkeyPresetsDimmed);

            oldBitmap = UncheckAllFiltersAccent;
            UncheckAllFiltersAccent = GetSolidImageByBitmapMask(AccentColor, Resources.uncheck_all_preset_filters, pictureSize, pictureSize);
            ThemedBitmapAddRef(EmptyForm, oldBitmap, UncheckAllFiltersAccent);
            oldBitmap = UncheckAllFiltersDimmed;
            UncheckAllFiltersDimmed = GetSolidImageByBitmapMask(GetWeightedColor(AccentColor, FormBackColor, DeepDimmedWeight), Resources.uncheck_all_preset_filters, pictureSize, pictureSize);
            ThemedBitmapAddRef(EmptyForm, oldBitmap, UncheckAllFiltersDimmed);


            //LR
            int pictogramSize = (int)Math.Round(23f * buttonHeightDpiFontScaling);

            oldBitmap = Window;
            Window = ScaleBitmap(Resources.window, pictogramSize, pictogramSize);
            ThemedBitmapAddRef(EmptyForm, oldBitmap, Window);


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

            Color ChangedForeColor = Color.FromKnownColor(KnownColor.Red);
            Color PreservedTagsForeColor = Color.FromKnownColor(KnownColor.Blue);
            Color PreservedTagValuesForeColor = Color.FromKnownColor(KnownColor.Green);


            //CHANGED STYLE
            ChangedCellStyle.BackColor = UnchangedCellStyle.BackColor;
            ChangedCellStyle.SelectionBackColor = UnchangedCellStyle.SelectionBackColor;

            float scale = 1.5f;//***
            float shift = 50f;//***
            float threshold = 175f;

            Color backColor = ChangedCellStyle.BackColor;
            float br = backColor.R;
            float bg = backColor.G;
            float bb = backColor.B;

            float bbrt = (br + bg + bb) / 3f;

            Color foreColor = ChangedForeColor;
            float r = foreColor.R;
            float g = foreColor.G;
            float b = foreColor.B;

            float brt = (r + g + b) / 3f;

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
                r = r / scale;
                g = g / scale;
                b = b / scale;

            }
            else
            {
                r = r * scale;
                g = g * scale;
                b = b * scale;

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
                r = r / scale;
                g = g / scale;
                b = b / scale;

            }
            else
            {
                r = r * scale;
                g = g * scale;
                b = b * scale;

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
        }

        internal static ToolStripMenuItem AddMenuItem(ToolStripMenuItem menuItemGroup, string itemName, string hotkeyDescription, EventHandler handler, bool enabled = true, Form form = null)
        {
            if (hotkeyDescription != null)
                MbApiInterface.MB_RegisterCommand(hotkeyDescription, handler);

            ToolStripItem menuItem = menuItemGroup.DropDown.Items.Add(itemName, null, handler);
            menuItem.Enabled = enabled;
            menuItem.Tag = form;

            if (itemName == "-")
                return null;
            else
                return (ToolStripMenuItem)menuItem;
        }

        internal void addPluginMenuItems() //Must be called AFTER InitLR() and InitASR(), addPluginContextMenuItems()!
        {
            TagToolsSubmenu.DropDown.Items.Clear();

            OpenedFormsSubmenu = AddMenuItem(TagToolsSubmenu, OpenWindowsMenuSectionName, null, null);
            AddMenuItem(TagToolsSubmenu, "-", null, null);
            AddMenuItem(TagToolsSubmenu, TagToolsMenuSectionName, null, null, false);

            if (!SavedSettings.dontShowCopyTag) AddMenuItem(TagToolsSubmenu, CopyTagCommandName, CopyTagCommandDescription, copyTagEventHandler);
            if (!SavedSettings.dontShowSwapTags) AddMenuItem(TagToolsSubmenu, SwapTagsCommandName, SwapTagsCommandDescription, swapTagsEventHandler);
            if (!SavedSettings.dontShowChangeCase) AddMenuItem(TagToolsSubmenu, ChangeCaseCommandName, ChangeCaseCommandDescription, changeCaseEventHandler);
            if (!SavedSettings.dontShowReencodeTag)
            {
                AddMenuItem(TagToolsSubmenu, ReencodeTagCommandName, ReencodeTagCommandDescription, reencodeTagEventHandler);
                AddMenuItem(TagToolsSubmenu, ReencodeTagsCommandName, ReencodeTagsCommandDescription, reencodeTagsEventHandler);
            }
            if (!SavedSettings.dontShowLibraryReports)
            {
                AddMenuItem(TagToolsSubmenu, LibraryReportsCommandName, LibraryReportsCommandDescription, libraryReportsEventHandler);
                if (LRPresetsWithHotkeysCount > 0)
                {
                    LibraryReportsPresetsMenuItem = AddMenuItem(TagToolsSubmenu, LibraryReportsCommandName.Replace("...", string.Empty), null, null);
                    RegisterLRPresetsHotkeysAndMenuItems(this);
                }
            }
            if (!SavedSettings.dontShowAutorate) AddMenuItem(TagToolsSubmenu, AutoRateCommandName, AutoRateCommandDescription, autoRateEventHandler);
            if (!SavedSettings.dontShowASR)
            {
                AddMenuItem(TagToolsSubmenu, AsrCommandName, AsrCommandDescription, asrEventHandler);
                if (ASRPresetsWithHotkeysCount > 0)
                {
                    ASRPresetsMenuItem = AddMenuItem(TagToolsSubmenu, AsrCommandName.Replace("...", string.Empty), null, null);
                    RegisterASRPresetsHotkeysAndMenuItems(this);
                }
                AddMenuItem(TagToolsSubmenu, MsrCommandName, MsrCommandDescription, multipleSearchReplaceEventHandler);
            }
            if (!SavedSettings.dontShowCAR) AddMenuItem(TagToolsSubmenu, CarCommandName, CarCommandDescription, carEventHandler);
            if (!SavedSettings.dontShowCT) AddMenuItem(TagToolsSubmenu, CtCommandName, CtCommandDescription, compareTracksEventHandler);


            AddMenuItem(TagToolsSubmenu, "-", null, null);
            AddMenuItem(TagToolsSubmenu, CopyTagsToClipboardCommandName, CopyTagsToClipboardCommandDescription, copyTagsToClipboardEventHandler);
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

            AddMenuItem(TagToolsSubmenu, PasteTagsFromClipboardCommandName, PasteTagsFromClipboardCommandDescription, pasteTagsFromClipboardEventHandler);


            if (!SavedSettings.dontShowBackupRestore)
            {
                AddMenuItem(TagToolsSubmenu, "-", null, null);
                AddMenuItem(TagToolsSubmenu, BackupRestoreMenuSectionName, null, null, false);
                AddMenuItem(TagToolsSubmenu, TagHistoryCommandName, TagHistoryCommandDescription, tagHistoryEventHandler);
                AddMenuItem(TagToolsSubmenu, "-", null, null);
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

            AddMenuItem(TagToolsSubmenu, "-", null, null);
            AddMenuItem(TagToolsSubmenu, PluginHelpString, null, helpEventHandler);//********
            AddMenuItem(TagToolsSubmenu, PluginVersion, null, null, false);
        }

        internal void addPluginContextMenuItems() //Must be called AFTER InitLR() and InitASR(), and BEFORE addPluginMenuItems()!
        {
            if (TagToolsContextSubmenu == null)
                return;


            TagToolsContextSubmenu.DropDown.Items.Clear();

            if (!SavedSettings.dontShowBackupRestore) AddMenuItem(TagToolsContextSubmenu, TagToolsMenuSectionName, null, null, false);

            if (!SavedSettings.dontShowCopyTag) AddMenuItem(TagToolsContextSubmenu, CopyTagCommandName, null, copyTagEventHandler);
            if (!SavedSettings.dontShowSwapTags) AddMenuItem(TagToolsContextSubmenu, SwapTagsCommandName, null, swapTagsEventHandler);
            if (!SavedSettings.dontShowChangeCase) AddMenuItem(TagToolsContextSubmenu, ChangeCaseCommandName, null, changeCaseEventHandler);
            if (!SavedSettings.dontShowLibraryReports)
            {
                AddMenuItem(TagToolsSubmenu, LibraryReportsCommandName, LibraryReportsCommandDescription, libraryReportsEventHandler);
                if (LRPresetsWithHotkeysCount > 0)
                    LRPresetsContextMenuItem = AddMenuItem(TagToolsContextSubmenu, LibraryReportsCommandName.Replace("...", string.Empty), null, null);
            }
            if (!SavedSettings.dontShowASR)
            {
                //AddMenuItem(TagToolsContextSubmenu, AsrCommandName, null, asrEventHandler);
                if (ASRPresetsWithHotkeysCount > 0)
                    ASRPresetsContextMenuItem = AddMenuItem(TagToolsContextSubmenu, AsrCommandName.Replace("...", string.Empty), null, null);
                AddMenuItem(TagToolsContextSubmenu, MsrCommandName, null, multipleSearchReplaceEventHandler);
            }
            if (!SavedSettings.dontShowCT) AddMenuItem(TagToolsContextSubmenu, CtCommandName, null, compareTracksEventHandler);
            AddMenuItem(TagToolsContextSubmenu, "-", null, null);
            AddMenuItem(TagToolsContextSubmenu, CopyTagsToClipboardCommandName, null, copyTagsToClipboardEventHandler);
            CopyTagsToClipboardUsingContextMenuItem = AddMenuItem(TagToolsContextSubmenu, CopyTagsToClipboardUsingMenuDescription, null, null);
            addCopyTagsToClipboardUsingContextMenuItems();
            AddMenuItem(TagToolsContextSubmenu, PasteTagsFromClipboardCommandName, null, pasteTagsFromClipboardEventHandler);

            if (!SavedSettings.dontShowBackupRestore)
            {
                AddMenuItem(TagToolsContextSubmenu, "-", null, null);
                AddMenuItem(TagToolsContextSubmenu, BackupRestoreMenuSectionName, null, null, false);
                AddMenuItem(TagToolsContextSubmenu, TagHistoryCommandName, null, tagHistoryEventHandler);
            }


            if (!SavedSettings.dontShowShowHiddenWindows)
            {
                MbApiInterface.MB_RegisterCommand(ShowHiddenCommandName, showHiddenEventHandler);
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
            if (presetId == string.Empty)
                return string.Empty;

            return GetLastReplacedTag(url, IdsAsrPresets[presetId]) ?? string.Empty;
        }

        public string CustomFunc_LR(string url, string functionId)
        {
            if (functionId == string.Empty)
                return string.Empty;

            return AutoCalculateReportPresetFunction(url, functionId) ?? string.Empty;
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

        public string CustomFunc_Log(string number)
        {
            number = number.Replace('.', LocalizedDecimalPoint);

            return Math.Log10(double.Parse(number)).ToString();
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
            string[] wordSplittersASR = SavedSettings.wordSplittersASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] exceptionCharsASR = SavedSettings.exceptionCharsASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            input = ChangeCase.ChangeWordsCase(input, ChangeCase.ChangeCaseOptions.LowerCase, null, false,
                null, wordSplittersASR);
            string result = ChangeCase.ChangeWordsCase(input, ChangeCase.ChangeCaseOptions.TitleCase, exceptionWords, false,
                exceptionCharsASR, wordSplittersASR, true, true);

            return result;
        }

        public string CustomFunc_TitleCase(string input, string exceptionWordsString)
        {
            string[] exceptionWords = exceptionWordsString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] wordSplittersASR = SavedSettings.wordSplittersASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] exceptionCharsASR = SavedSettings.exceptionCharsASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            input = ChangeCase.ChangeWordsCase(input, ChangeCase.ChangeCaseOptions.LowerCase, null, false,
                null, wordSplittersASR);
            string result = ChangeCase.ChangeWordsCase(input, ChangeCase.ChangeCaseOptions.TitleCase, exceptionWords, false,
                exceptionCharsASR, wordSplittersASR, true, true);

            return result;
        }

        public string CustomFunc_SentenceCase(string input)
        {
            string[] exceptionWords = SavedSettings.exceptionWordsASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] wordSplittersASR = SavedSettings.wordSplittersASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] exceptionCharsASR = SavedSettings.exceptionCharsASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            input = ChangeCase.ChangeWordsCase(input, ChangeCase.ChangeCaseOptions.LowerCase, null, false,
                null, wordSplittersASR);
            string result = ChangeCase.ChangeWordsCase(input, ChangeCase.ChangeCaseOptions.SentenceCase, exceptionWords, false,
                exceptionCharsASR, wordSplittersASR, true, true);

            return result;
        }

        public string CustomFunc_SentenceCase(string input, string exceptionWordsString)
        {
            string[] exceptionWords = exceptionWordsString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] wordSplittersASR = SavedSettings.wordSplittersASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] exceptionCharsASR = SavedSettings.exceptionCharsASR.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            input = ChangeCase.ChangeWordsCase(input, ChangeCase.ChangeCaseOptions.LowerCase, null, false,
                null, wordSplittersASR);
            string result = ChangeCase.ChangeWordsCase(input, ChangeCase.ChangeCaseOptions.SentenceCase, exceptionWords, false,
                exceptionCharsASR, wordSplittersASR, true, true);

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
                return string.Empty;
        }

        public string CustomFunc_Char(string code)
        {
            ushort charcode = ushort.Parse(code, System.Globalization.NumberStyles.HexNumber);
            return ((char)charcode).ToString();
        }

        public string CustomFunc_CharN(string code, string timesString)
        {
            ushort charcode = ushort.Parse(code, System.Globalization.NumberStyles.HexNumber);
            string character = ((char)charcode).ToString();

            string sequence = string.Empty;

            int times = int.Parse(timesString);
            for (int i = 0; i < times; i++)
                sequence += character;

            return sequence;
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
