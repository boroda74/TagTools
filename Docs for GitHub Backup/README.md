#ADDITIONAL TAGGING & REPORTING TOOLS PLUGIN

Plugin requires MusicBee version **3.5** or higher.
<br/>

*Note:*
If you are familiar with the plugin quick update process, you can always download *latest plugin version [here][4]*. Copy the file "mb_TagTools.dll" from the archive "mb_TagTools_latest.zip" to MusicBee "Plugins" folder, and overwrite the existing file.
<br/>

*Note:*
Pay attention to tool tips over buttons/fields in plugin windows.

###Plugin adding/updating/removing

Extract content of the downloaded archive to a temporary folder of your choice. See the "README FIRST!" file inside this folder for instructions on adding/updating/removing the plugin.
<br/>

----

##TAGGING & REPORTING TOOLS

###Copy Tag

Allows you to copy one tag to another for selected files. It’s possible to append one tag to the end of another tag or add one tag to the beginning of another tag, placing custom text between them. Also, it’s possible to use &lt;Null&gt; pseudo tag as the source to append static text to the destination tag.

*Note:*
- It is possible to copy a number of source pseudo tags (like &lt;Date Created&gt;, which indicates the file creation date) to, for example, custom tags.

###Swap Tags

Swaps any two tags for selected files.

*Notes:*
- "Smart swap"/"Smart copy" option is important only for working with multiple artists/composers (it will convert the list of artists/composers to ";" delimited string if destination tag doesn't support multiple values). Unchecking this option will preserve each byte of the source tag, but it may only be useful if you use the destination tag for temporary storage.
- It’s possible to use the same source and destination tags for "Artist"/"Composer" tags in "Swap tags" command with "Smart swap" option enabled to convert ";" delimited tag to multiple artists/composers and vice versa.

###Change Case

Changes letter case of a tag according to rather sophisticated rules for selected tracks.

*Notes:*
- Excluded words *remain unchanged*, **not** become *lowercase*!
- You can make two or more steps by changing casing rules and clicking "Reapply rules to new tag" button without saving changes to tags.

###Library Reports

Allows to export some library statistics for currently displayed tracks to external file: HTML document (as a table), M3U playlist (it’s only possible to export file paths to playlists, but considering filtering capability of command this option may be useful), CSV file, HTML document as CD booklet, etc. You can re-import LR report file into a spreadsheet editor (including HTML table - it is the easiest way to import the track list to MS Excel) for printing statistics or the content of your library, filtered library content, or your playlist(s).

*Notes:*
- Command supports two types of exported fields: grouping tags and aggregated functions. Plugin calculates every aggregated function for every unique combination of grouping tags. "Count" function counts the number of *different* values of a given tag. Other functions are obvious, and you can apply them only to numeric or duration tags (e.g. "Time", "File size", etc.).
- Any aggregated function can be saved to physically stored custom tag (of course this custom tag won’t be updated automatically), so that MusicBee could use this aggregated function for sorting/filtering.
- For all aggregated functions except for "Count" function please make sure that all tag values used by function have the same measurement units (KB, MB, etc.) or adjust units in plugin settings (go to MusicBee menu> Edit> Preferences> Plugins> Configure, not required if both MusicBee and Windows localizations are English or Russian).
- If you want only to export some tags, then define only grouping fields in preview table (don't use aggregated functions and uncheck "Calculate sub-grouping totals" checkbox, which is senseless in this case).

Also, "Library Reports" allows to calculate one or more presets and to save results to (custom) tags at startup, after a given number of tag changes or manually. This may be useful for *auto-saving* or manual saving such values as "Summary play counts of albums" or "Number of tracks for each artist", etc. to (custom) tags, i.e. it's some analog of virtual tags with the ability to operate not only on tags of current track, but on tags of all tracks.

"Library Reports" command supports multiple item splitters for grouping tags. For example, you can define splitter ";" for "Artist" or "Genre" tags to split multiple artists or genres. If several tags are split (i.e., several tags for one track may have several values) then *all combinations of split values for every tag* will be included in the LR report.

"Library Reports" supports virtual tag expressions (any expressions valid for virtual tags). 
<br/>

[Read more about "Library Reports" virtual tag expressions][6]
<br/>

"Library Reports" command adds a new virtual tag function:

<pre>$LR(&lt;URL&gt;,function_id)</pre>

To use this function open library reports window, create one or several presets, each with one or more aggregated functions and assign a function id (any string containing a-z chars, numbers and symbols . : - _ ) to functions. Probably you will want to write all aggregate functions to &lt;Null&gt; tag in this case (i.e. do not write them to any tags at all if you use these functions only for virtual tags).

"Library Reports" virtual tag functions allow to gather and display in the main panel some library statistics for currently displayed tracks.

*Note:*
- All $LR virtual tag functions are calculated, updated and displayed dynamically.

*Example 1*
If you want to get the number of albums of artist of displayed track, i.e. the number of albums (function "Count" of (parameter) tag "Album") per artist (tag "Album Artist"), then add grouping tag "Album Artist" in the table and required function "Count(Album)".

*Example 2*
If you want to get the number of releases of artist (of displayed track) per year, i.e. the number of albums (function "Count" of (parameter) tag "Album") per year (tag "Year") per artist (tag "Album Artist"), then add the grouping tags (in order) "Album Artist" and "Year" in the table and required function "Count(Album)".

*Example 3*
If you want to get the total duration of all tracks of artist of displayed track, i.e. the sum of track durations (function "Sum" of tag "Time") per artist (tag "Album Artist"), then add grouping tag "Album Artist" and function "Sum(Time)".

See two predefined presets "LIBRARY TOTALS" and "LIBRARY AVERAGES" of "Library Reports" command for more examples.

*LR virtual tag functions may be even more useful if they are used in the grouping header of "Albums and Tracks" view.*
Make sure that you haven't *accidentally* checked some "Library Reports" presets for auto-execution. You will see a warning message at the top of "Library Reports" window if any presets are marked as auto-executed:
<br/>

[Auto-executed preset view example][2]

###Auto Rate

Calculates auto rating based on the number of plays per day for selected tracks. Also, it’s possible to auto rate all tracks of your library on MusicBee startup or update auto rating if currently played track is changed.

Another option is to calculate auto ratings defining the percentage of tracks of your library that should be assigned a certain rating level (e.g. 1 star, 2 starts, etc.)

###Re-encode Tag/Re-encode Tags

Even ID3V2 tags are usually stored not using UTF encoding (which is independent of the language), most other tagging systems frequently use national code pages too. Here, reading tags is correct only if tags use the same encoding (code page) as your windows default encoding (locale, default code page). This command allows you to convert any incorrectly interpreted, not UTF tags, to UTF tags. In this command, "Initial encoding" is the suggested actual encoding of the tag(s) and the "Used encoding" is incorrectly interpreted encoding of the tag(s). Usually it's your default windows encoding. The only sense to change "Used encoding" is if you got music file from another computer with different regional settings. "Re-encode Tag**s**" re-encodes all tags of selected tracks at ones. Cue sheets are supported.

###Advanced Search & Replace

- "Advanced Search and Replace" works only with regular expressions.
- "Advanced Search and Replace" has savable/customizable presets.
- You can choose which "Advanced Search and Replace" presets to automatically execute if any tag is changed and/or if new tracks are added to the library or inbox.
- "Advanced Search and Replace" can search in one tag and save a replacement in another tag.
- Single "Advanced Search and Replace" preset can make up to 5 subsequent replacements.

*Notes:*
- You can install/update predefined ASR presets by clicking "Install All"/"Install New" buttons.
- You cannot *edit* predefined presets, but you can *copy them* and *edit the copy* and/or *delete* them.

"Advanced Search & Replace" command adds a new virtual tag function:

<pre>$ASR(&lt;URL&gt;,preset_id)</pre>

To use this function go to main ASR window, select preset and define preset id (any string containing a-z chars, numbers and symbols . : - _ ). Then use function in virtual tags, file organization templates, etc., where preset_id is the id entered in ASR window. ASR preset function will return the last value written to tag in the preset. To see the last written tag, click "Edit/Rename" button in the ASR window or double-click preset (if "Edit/Rename" button is disabled). *Most preset functions* will return exactly what *you expect*.

*Note:*
- All $ASR virtual tag functions are calculated, updated and displayed dynamically.

Make sure that you haven't *accidentally* checked some "Advanced Search & Replace" presets for auto-execution. You will see a warning message at the top of the ASR window if any presets are marked as auto-executed:
<br/>

[Auto-executed preset view example][3]

###Multiple Search & Replace

Multi-step search and replace. You can make an unlimited number of replacements in the same source tag and write result to a given destination tag. MSR presets can be saved as special ASR presets and can be ticked in ASR for auto-execution if any tags are changed or if new tracks are added to the library. 

###Calculate Average Album Ratings

Command averages all the ratings of the individual tracks on the album, writing the result to "Album rating". Any tag can be used as track rating, and any writable tag as album rating. This may be useful for (auto)calculating the average album rating, and saving it to (custom) tag.

###Compare Tracks

Command provides an easy way to compare tags of 2 (or more) tracks.

###Copy Tags to Clipboard
Copies specified tags from selected files to clipboard.

###Paste Tags from Clipboard

Pastes copied to clipboard tags to selected files from clipboard.

*Notes:*
- It's possible to copy tags from one file and paste them to more than one file.
- It's impossible to copy tags from several files and paste them to another number of files.
- It's possible to copy tags in MusicBee and paste them to another application like Microsoft Excel or Notepad++.
- Usually tags are pasted to tracks in the track display order, but you can also copy <Full path w/o ext.> (full track path including filename **without** extension) or URL (full track path including filename **and** extension) pseudo-tags (along with other tags) to clipboard. Here, the plugin will prompt you to match tracks according to these tags or use the track displayed order as usual. It's not recommended to copy both path pseudo-tags to clipboard to avoid confusion, but just in case the URL pseudo-tag takes precedence. If you choose to match tracks by path, the number of tracks from which the tags have been copied may not be equal to the number of tracks to which the tags are pasted.

###Last Skipped Date/Time

Plugin has the option to store "last skipped date/time" in any writable tag (e.g. some custom tag). Go to MusicBee menu> Edit> Preferences> Plugins> Additional Tagging Tools> Configure. Click the button "Save last skipped date..." at the bottom of the plugin settings window.

Plugin strictly follows MusicBee convention of what is "skipped track".

To define date type of custom tag, open MusicBee menu> Edit> Preferences> Tags (1)> Define New Tags> Configure Fields. Find "Custom1" tag (or some other custom tag), and change its type to "Date".
<br/>

[See here screenshot of this setting.][5]

----

##BACKUP & RESTORE

These commands allow to (auto)backup tags of all tracks in your library and restore them. First, create full tag backup manually. You can do manual or auto incremental backups after this.

*Notes:*
- The plugin backs up tags using tack unique IDs, which are stored in the MusicBee library (database) only. So, it's some kind of "Time machine" only. THE PLUGIN WON'T BE ABLE TO RESTORE TAGS IF YOU HAVE CREATED A NEW MUSICBEE LIBRARY FROM SCRATCH, E.G. ON A NEW PC (EVEN IF YOU HAVE ADDED THE SAME TRACKS TO THE NEW LIBRARY)!

----

##VIRTUAL TAG FUNCTIONS

Plugin introduces several new virtual tag functions (they also can be used in various filename templates, e.g. in the file organizer).
<br/>

*Note:*
Use these functions like native MusicBee functions (i.e. without any initial setup required by $LR() and $ASR() functions). 
Everywhere you can use either literals or MusicBee tags of appropriate types as function parameters, e.g. $NumberOfDays(&lt;Year&gt;,&lt;Original Year&gt;) or $SubDateTime(&lt;Date Added&gt;,01/01/2000). 

<pre>
$Random(max_number): random integer number between 0 
    and max_number (including them)

$Now(): returns current date/time. Date/time format depends on your Windows 
    regional settings, something like 11/01/2022 02:30:00 pm (for US regional 
    settings) or 01.11.2022 14:30:00 (for European regional settings)

$AddDuration(duration1,duration2): duration format is similar to date/time format 
    without "am/pm" suffix, but allowed values are different, 
    e.g. 00/01/0000 00:00:00 is valid duration, but is not a valid date/time. Either 
    date part or time part can be omitted. It will be defaulted to zero value

$SubDuration(duration1,duration2)

$MulDuration(duration,number): multiplies duration by floating point or integer 
    number, e.g. $MulDuration(&lt;Time&gt;,&lt;Play Count&gt;)

$SubDateTime(datetime1,datetime2): returns duration

$NumberOfDays(datetime1,datetime2): returns the number of days between datetime1 
    and datetime2

$AddDurationToDateTime(datetime,duration)

$SubDurationFromDateTime(datetime,duration)

$TitleCase(string)

$SentenceCase(string)

$Sqrt(number): square root. May be useful for gathering some library 
    statistics (in conjunction with LR functions)

$eq(number1,number2): compares 2 integer or fractional numbers, determines if 
    number1 is **eq**ual to number2, e.g. $eq(1.0,1) returns "T"

$ne(number1,number2): determines if number1 is **n**ot **e**qual to number2

$gt(number1,number2): determines if number1 is **g**reater **t**han number2

$lt(number1,number2): determines if number1 is **l**ess **t**han to number2

$ge(number1,number2): determines if number1 is **g**reater than or **e**qual to to number2

$le(number1,number2): determines if number1 is **l**ess than or **e**qual to number2

$Round(number,number_of_digits_after_decimal_point): $Round(4.28,1) 
    returns 4.3, and $Round(5.2,0) returns 5

$RoundUp(number,number_of_digits_after_decimal_point): $RoundUp(5.2,0) 
    returns 6

$RoundDown(number,number_of_digits_after_decimal_point): $RoundDown(4.28,1) 
    returns 4.2

$Name(&lt;URL&gt;): returns file name without extension and path to file. 
    Type &lt;URL&gt; exactly like this, don't use other function argument value

$DateCreated(&lt;URL&gt;): returns creation date/time of music file (not last 
    modification date/time)

$Char(hexadecimal code): returns Unicode character with given hexadecimal 
    code, e.g. $Char(a7) returns "§" (U+00A7)

$CharN(hexadecimal code,decimal number of times): returns Unicode character with given hexadecimal 
    code repeated the given number of times, e.g. $CharN(a7,3) returns "§§§" (U+00A7 repeated 3 times)

$TagContainsAnyString(&lt;URL&gt;,tag_name,string1|string2|etc.): returns "T" if tag 
    contains any of the strings separated by |, otherwise returns "F". 
    tag_name must be written without angle brackets, 
    e.g. $TagContainsAnyString(&lt;URL&gt;,Lyrics,water|river)

$TagContainsAllStrings(&lt;URL&gt;,tag_name,string1|string2|etc.): returns "T" if tag 
    contains all strings separated by |, otherwise returns "F". 
</pre>

----

##ASR SPECIAL FUNCTIONS

You can use special functions in substitution fields of "Advanced Search & Replace" and "Multiple Search & Replace" commands:

<pre>
\@null[[]] : returns "null" Unicode character

\@char[[hexadecimal code]] : returns Unicode character with given hexadecimal code, 
    e.g. \@char[[2f]] returns "/"

\@tc[[string;;excepted words]] : returns Title Cased string except for given words 
    separated by spaces, e.g \@tc[[$1;;a the an>]] will return title cased (except for 
    words "a", "the", "an") 1st captured in search pattern string, and \@tc[[$1]] will 
    return title cased string, not using any excepted words

Excepted words *will be unchanged*, **not** become *lowercased*! To lowercase them, 
    use: \@tc[[\@lc[[string]];;excepted words]]

\@lc[[string;;excepted words]] : returns lower cased string except for the given words

\@uc[[string;;excepted words]] : returns UPPER CASED string except for the given words

\@sc[[string;;excepted words]] : returns Sentence cased string except for the given words

\@eval[[virtual tag expression]] : returns result of calculation of virtual tag expression, 
    e.g. \@eval[[$Sub(<Play Count>,<Skip Count>)]]

\@repunct[[string]] : changes Unicode punctuation marks to ASCII analogs, e.g. « to <<
</pre>

----

##APPENDIX

###License

Do all you want with plugin binary and source code at your own risk.

###Backup download

[Google Drive, all my plugins and their sources][1]

## SAST Tools

This project has been checked by PVS-Studio.
[PVS-Studio](https://pvs-studio.ru/ru/pvs-studio/?utm_source=website&utm_medium=github&utm_campaign=open_source) - static analyzer for C, C++, C###, and Java code.

  [1]: https://drive.google.com/drive/folders/0B-D1MoIY406HMzlJVWprYXF1Q0k?usp=sharing
  [2]: https://github.com/boroda74/TagTools/blob/master/Docs/LR-AUTO-EXECUTION-FILTERING.md
  [3]: https://github.com/boroda74/TagTools/blob/master/Docs/ASR-AUTO-EXECUTION-FILTERING.md
  [4]: https://www.mediafire.com/file/h2t08o9562efboi/mb_TagTools_latest.zip/file
  [5]: https://i.imgur.com/C1gPwaK.png
  [6]: https://github.com/boroda74/TagTools/blob/master/Docs/LR-EXPRESSIONS.md
