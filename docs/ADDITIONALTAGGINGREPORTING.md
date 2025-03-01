# ADDITIONAL TAGGING & REPORTING

## List of plugin's commands

### Copy Tag

Allows you to copy one tag to another for selected files. It’s possible to append one tag to the end of another tag or add one tag to the beginning of another tag, placing custom text between them. Also, it’s possible to use \<Null\pseudo tag as the source to append static text to the destination tag.

*Notes:*

* "Smart copy" option is important only for working with multiple artists/composers (it will convert the list of artists/composers to ";" delimited string if destination tag doesn't support multiple values). Unchecking this option will preserve each byte of the source tag (including service invisible characters), but it may only be useful if you use the destination tag for temporary storage.

- It is possible to copy a number of source pseudo tags (like \<Date Created\>, which indicates the file creation date) to, for example, custom tags.

### Swap Tags

Swaps any two tags for selected files.

*Notes:*

* "Smart swap" option is important only for working with multiple artists/composers (it will convert the list of artists/composers to ";" delimited string if destination tag doesn't support multiple values). Unchecking this option will preserve each byte of the source tag (including service invisible characters), but it may only be useful if you use the destination tag for temporary storage.
* It’s possible to use the same source and destination tags for "Artist"/"Composer" tags in "Swap tags" command with "Smart swap" option enabled to convert ";" delimited tag to multiple artists/composers and vice versa.

### Change Case

Changes letter case of a tag according to rather sophisticated rules for selected tracks.

*Notes:*

* Excluded words *remain unchanged*, **not** become *lowercase*\!
* You can make two or more steps by changing casing rules (and exeptions) and clicking "Reapply rules to new tag" button without saving changes to tags.
* "Change Case" supports presets for multistep case changing. There are two predefined presets, "Sentence case" and "Title Case", which are identical by functionality to corresponding VT functions. Click "Description" button for details. Also, you can record your own presets if needed.

### Re-encode Tag/Re-encode Tags

Even ID3V2 tags are usually stored not using UTF encoding (which is independent of the language), most other tagging systems frequently use national code pages too. Here, reading tags is correct only if tags use the same encoding (code page) as your Windows default encoding (regional settings). This command allows you to convert any incorrectly interpreted not UTF-encoded tags to UTF-encoded tags. In this command, "Initial encoding" is the suggested actual encoding of the tag(s) and the "Used encoding" is incorrectly interpreted encoding of the tag(s). Usually it's your default Windows encoding. The only sense to change "Used encoding" is if you got music file from another computer with different regional settings. "Re-encode Tag**s**" re-encodes all tags of selected tracks at ones. Cuesheets are supported.

### Library Reports

Allows to export some library statistics for currently displayed tracks to external file: HTML document (as a table), M3U playlist (it’s only possible to export file paths to playlists, but considering filtering capability of command this option may be useful), CSV file, HTML document as CD booklet, etc. You can re-import LR report file into a spreadsheet editor (including HTML table - it is the easiest way to import the tags to Microsoft Excel) for printing statistics or the content of your library, filtered library content, or your playlist(s).

*Notes:*

* Command supports two types of exported fields: grouping tags and aggregated functions. Plugin calculates every aggregated function for every unique combination of grouping tags. "Count" function counts the number of *different* values of a given tag. Other functions are obvious, and you can apply them only to numeric or duration tags (e.g. "Time", "File size", etc.).
* Any aggregated function can be saved to physically stored (e.g., custom) tag, so that MusicBee could use this aggregated function for sorting/filtering or in auto-playlist rules.
* For all aggregated functions except for "Count" function please make sure that all tag values used by function have the same measurement units (KB, MB, etc.) or adjust units in plugin settings (go to MusicBee menu\Edit\Preferences\Plugins\Additional Tagging Tools\Configure (not required if both MusicBee and Windows localizations are English or Russian).
* If you want only to export some tags, then define only grouping fields in preview table (don't use aggregated functions and uncheck "Calculate sub-grouping totals" checkbox, which is senseless in this case).
* LR has the option "Hide preview". You won't see report preview if you check this option, but you still will be able to export generated report to a file. This option is mostly intended for generating very large reports to avoid "Out of memory" exceptions. Checking this option will save a lot of memory and substantially speed up generating reports (dozens of times faster).

Also, "Library Reports" allows to calculate aggregated functions of one or more presets and to save results to (custom or generic) tags at startup or manually. This may be useful for *auto-saving* or manual saving such values as "Summary play counts of albums" or "Number of tracks for each artist", etc. to (custom) tags, i.e. it's some analog of virtual tags with the ability to operate not only on tags of the current track, but on tags of all tracks.

[View some LR report examples](LREXAMPLES.md)

Also, see two predefined presets "LIBRARY TOTALS" and "LIBRARY AVERAGES" of "Library Reports" command as examples.

"Library Reports" command supports multiple item splitters for grouping tags. For example, you can define splitter ";" for "Artist" or "Genre" tags to split multiple artists or genres. If several tags are split (i.e., several tags for one track may have several values) then *all combinations of split values for every tag* will be included in the LR report.

"Library Reports" supports virtual tag expressions (any expressions valid for virtual tags).
[Read more about "Library Reports" virtual tag expressions](LREXPRESSIONS.md)

"Library Reports" command adds a new virtual tag function:

$LR(\<URL\>,function\_id)

To use this function open library reports window, create one or more presets, each with one or more aggregated functions and assign a function id (any string containing **a-z** chars, **numbers** and symbols **. : - \_** ) to the functions. You can write all aggregate function results to \<Null\pseudo-tag in this case (i.e. do not write them to any tags at all if you use these functions only for virtual tags). All $LR virtual tag functions are calculated, updated and displayed *dynamically*. Also, you can at the same time *assign an id* to the function **and** choose *to store function results in (custom) tag*. In this case, LR will use a stored tag as a persistent function result cache. If you change some tags or add new tracks to the library, this cache will *dynamically update*. However, **it’s highly recommended** first to fill this cache for all existing tracks/current tags in the current library to avoid UI slowdowns and freezes when using MusicBee regularly. You will be automatically prompted to execute all affected presets and fill the cache for the current library if you define such LR presets.
"Library Reports" virtual tag functions allow you to collect and display some library statistics for currently displayed tracks in the main window. Either you can use virtual tags that use $LR functions in auto-playlist filter criteria. However, in this case it's **strongly recommended** to cache the results of the LR functions in tags.

*LR virtual tag functions may be even more useful if they are used in the grouping header of "Albums and Tracks" view.*

**Make sure that you haven't accidentally checked some "Library Reports" presets for auto-execution.** You will see a warning message at the top of "Library Reports" window if any presets are marked as auto-executed:

[View auto-executed preset example](LRAUTO-EXECUTIONFILTERING.md)

### Auto Rate

Calculates auto rating based on the number of plays per day for selected tracks. Also, it’s possible to auto rate all tracks of your library on MusicBee startup or update auto rating if currently played track is changed.

Another option is to calculate auto ratings defining the percentage of tracks of your library that should be assigned a certain rating level (e.g. 1 star, 2 stars, etc.)

### Advanced Search \& Replace

*Description:*

* "Advanced Search and Replace" works only with regular expressions.
* "Advanced Search and Replace" has savable/customizable presets.
* You can choose which "Advanced Search and Replace" presets to automatically execute if any tag is changed and/or if new tracks are added to the library or inbox.
* "Advanced Search and Replace" can search in one tag and save a replacement in another tag.
* Single "Advanced Search and Replace" preset can make up to 5 subsequent replacements.

*Notes:*

* You can install/update predefined ASR presets by clicking "Install All"/"Install New" buttons.
* You cannot *edit* predefined presets, but you can *copy them* and *edit the copy* and/or *delete* them.

"Advanced Search \& Replace" command adds a new virtual tag function:

$ASR(\<URL\>,preset\_id)

To use this function open ASR window, select preset and define preset id (any string containing a-z chars, numbers and symbols . : - \_ ). Then use function in virtual tags, file organization templates, etc., where preset\_id is the id entered in ASR window. ASR preset function will return the last value written to tag in the preset. To see the last written tag, click "Edit/Rename" button in the ASR window or double-click preset (if "Edit/Rename" button is disabled). *Most preset functions* will return exactly what *you expect*.

*Notes:*

* All $ASR virtual tag functions are calculated, updated and displayed dynamically.
* You can use [special functions](ASRMSRSPECIALFUNCTIONS.md) in substitution fields of "Advanced Search \& Replace" command.

**Make sure that you haven't accidentally checked some "Advanced Search \& Replace" presets for auto-execution.** You will see a warning message at the top of the ASR window if any presets are marked as auto-executed:

[View auto-executed preset example](ASRAUTO-EXECUTIONFILTERING.md)

### Multiple Search \& Replace

Multi-step search and replace. You can make an unlimited number of replacements in the same source tag and write result to a given destination tag. MSR presets can be saved as special ASR presets and can be ticked in ASR for auto-execution if any tags are changed or if new tracks are added to the library.

*Note:*

* You can use [special functions](ASRMSRSPECIALFUNCTIONS.md) in substitution fields of "Multiple Search \& Replace" command.

### Calculate Average Album Ratings

Command averages all the ratings of the individual tracks on the album, writing the result to album rating tag. Any tag can be used as track rating, and any writable tag as album rating. This may be useful for (auto)calculating the average album rating, and saving it to (custom) tag.

### Compare Tracks

Command provides an easy way to compare tags of 2 (or more) tracks.

### Copy Tags to Clipboard

Copies specified tags from selected files to clipboard.

### Paste Tags from Clipboard

Pastes tags to selected files from clipboard.

*Notes:*

* It's possible to copy tags from one track and paste them to more than one track.
* It's impossible to copy tags from several tracks and paste them to another number of tracks.
* It's possible to copy tags in MusicBee and paste them to another application like Microsoft Excel or Notepad++.
* Usually tags are pasted to tracks in the track display order, but you can also copy \<Full path w/o ext.\(full track path including filename **without** extension) or URL (full track path including filename **and** extension) pseudo-tags along with other tags to clipboard. Here, the plugin will prompt you to match tracks according to these tags or use the track displayed order as usual. It's not recommended to copy both these pseudo-tags to clipboard to avoid confusion, but just in case the URL pseudo-tag takes precedence. If you choose to match tracks by path, the number of tracks from which the tags have been copied may not be equal to the number of tracks to which the tags are pasted.
* If you paste tags from external application (e.g. from Microsoft Excel or Notepad++) make sure that *the first copied row* contains *tag names* (exactly as *the tags are named in MusicBee*).

### Last Skipped Date/Time

Plugin has the option to save track last skipped date/time to any writable tag (e.g. some custom tag). Go to MusicBee menu\Edit\Preferences\Plugins\Additional Tagging Tools\Configure. Click the button "Save last skipped date..." at the bottom of the plugin settings window.

Plugin strictly follows MusicBee convention of what is "skipped track".

To define date type of custom tag, open MusicBee menu\Edit\Preferences\Tags (1)\Define New Tags\Configure Fields. Find "Custom1" tag (or some other custom tag), and change its type to "Date".

[See here screenshot of this setting](LASTSKIPPEDDATETIME.md)

### Custom Sorting for Column Browser

It inserts a different number of invisible zero-width spaces at the beginning of tags for arbitrary sorting. Of course, it can be used not only for column browser, but it seems that MusicBee native custom sorting can be used in all other places.

[See here screenshot of this command](CUSTOMSORTINGCOLUMNBROWSER.md)

***

Copyright © boroda 2012-2025. Help version 9.2.250301