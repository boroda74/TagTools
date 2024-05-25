# VIRTUAL TAG FUNCTIONS

Plugin introduces several new virtual tag functions (they also can be used in various filename templates, e.g. in the file organizer).

&nbsp;

*Note:*

Use these functions like native MusicBee functions (i.e. without any initial setup required by $LR() and $ASR() functions).

Everywhere you can use either literals or MusicBee tags of appropriate types as function parameters, 
    e.g. $NumberOfDays(\<Year\>,\<Original Year\>) or $SubDateTime(\<Date Added\>,01/01/2000).

<pre>
<b>$Random(max\_number)</b> : random integer number between 0 and <i>max\_number</i> (including them)

<b>$Now()</b> : returns current date/time. Date/time format depends on your Windows regional settings, 
    something like <b>11/01/2022 02:30:00 pm</b> (for US regional settings) 
    or <b>01.11.2022 14:30:00</b> (for European regional settings)

<b>$AddDuration(duration1,duration2)</b> : adds <i>duration2</i> to <i>duration1</i>. Duration format is similar to date/time format 
    without "<b>am/pm</b>" suffix, but allowed values are different, e.g. <b>00/01/0000 00:00:00</b> is valid duration, but is not 
    a valid date/time. Either date part or time part can be omitted. It will be defaulted to zero value

<b>$SubDuration(duration1,duration2)</b> : subtracts <i>duration2</i> from <i>duration1</i>

<b>$MulDuration(duration,number)</b> : multiplies <i>duration</i> by floating point or integer <i>number</i>, e.g. <i>$MulDuration(\<Time\>,\<Play Count\>)</i>

<b>$SubDateTime(datetime1,datetime2)</b> : subtracts <i>datetime2</i> from <i>datetime1</i>. Returns duration

<b>$NumberOfDays(datetime1,datetime2)</b> : returns the number of days between <i>datetime1</i> and <i>datetime2</i>

<b>$AddDurationToDateTime(datetime,duration)</b> : adds <i>duration</i> to <i>datetime</i>

<b>$SubDurationFromDateTime(datetime,duration)</b> : subtracts <i>duration</i> from <i>datetime</i>

<b>$SentenceCase(string,sentence\_separators)</b> : where <b><i>sentence\_separators</i></b> is the list of characters after which 
    the words will become capitalized. Characters in the <b><i>sentence\_separators</i></b> list may be separated by spaces or 
    not separated at all. For example, these two character lists mean the same: "<b>. , -</b>" and "<b>.,-</b>". A dot, followed by 
    a space, is always treated as a sentence separator. You can omit the last parameter simply by putting the closing 
    parenthesis earlier, i.e., <i>$SentenceCase(string)</i>

<b>$TitleCase(string,lower\_case\_words,word\_separators,lower\_case\_between\_chars\_opening\_list,lower\_case\_between\_chars\_closing\_list,lower\_case\_after\_chars)</b> : 
    where <i>lower\_case\_words</i> is the list of words separated by spaces, which will become lowercase 
    (<i>always except for the first word</i> and <i>except for the last words if not lowercase by other parameters</i>). 
    <i>word\_separators</i> is the list of characters after which the words will be 
    capitalized. <i>lower\_case\_between\_chars\_opening\_list</i> and <i>lower\_case\_between\_chars\_closing\_list</i> are 
    the lists of characters <i>between</i> which the words become lowercase. The number of characters in both lists must be 
    the same; the opening and closing characters are paired by their position in the lists (the lists may 
    be, e.g., "<b>( \[ {</b>" and "<b>) \] }</b>"). <i>lower\_case\_after\_chars</i> consists of a list of characters, <i>after</i> which the words 
    become lowercase (e.g., <b>'</b> for something like "Someone#8202;<b>'</b>&#8202;s"). <i>The first word</i> will <i>always</i> be capitalized. 
    It’s recommended to enclose the last four lists into quotes (e.g., <b>"\& . -"</b>). Characters in all character lists may be separated 
    by spaces or not separated at all. For example, these two character lists mean the same: "<b>. , -</b>" and "<b>.,-</b>". You can omit 
    any parameter except for the first one, using a single character <b>\`</b> <i>not</i> surrounded by spaces to pass empty parameter. 
    You can <i>safely use</i> character <b>\`</b> in its literal meaning if it’s typed in the list <i>along with other characters</i>. 
    To use <i>the single character</i> <b>\`</b> in its literal meaning (<i>not</i> as an omitted parameter) type "<b>\\\`</b>", and to use 
    <i>the single character</i> <b>\\</b> in its literal meaning type "<b>\\\\</b>". You can omit any number of end parameters simply 
    by putting closing parenthesis earlier, e.g., <i>$TitleCase(string,lower\_case\_words,word\_separators)</i>

<b>$Sqrt(number)</b> : square root. May be useful for collecting some library statistics (in conjunction with LR functions)

<b>$eq(number1,number2)</b> : compares two integer or fractional numbers, determines if <i>number1</i> is <b>eq</b>ual to <i>number2</i>, 
    e.g. <i>$eq(1.0,1)</i> returns "<b>T</b>"

<b>$ne(number1,number2)</b> : returns "<b>T</b>" if <i>number1</i> is <b>n</b>ot <b>e</b>qual to <i>number2</i>, otherwise returns "<b>F</b>"

<b>$gt(number1,number2)</b> : returns "<b>T</b>" if <i>number1</i> is <b>g</b>reater <b>t</b>han <i>number2</i>, otherwise returns "<b>F</b>"

<b>$lt(number1,number2)</b> : returns "<b>T</b>" if <i>number1</i> is <b>l</b>ess <b>t</b>han to <i>number2</i>, otherwise returns "<b>F</b>"

<b>$ge(number1,number2)</b> : returns "<b>T</b>" if <i>number1</i> is <b>g</b>reater than or <b>e</b>qual to to <i>number2</i>, otherwise returns "<b>F</b>"

<b>$le(number1,number2)</b> : returns "<b>T</b>" if <i>number1</i> is <b>l</b>ess than or <b>e</b>qual to <i>number2</i>, otherwise returns "<b>F</b>"

<b>$Round(number,number\_of\_digits\_after\_decimal\_point)</b> : <i>$Round(4.28,1)</i> returns <b>4.3</b>, and <i>$Round(5.2,0)</i> returns <b>5</b>

<b>$RoundUp(number,number\_of\_digits\_after\_decimal\_point)</b> : <i>$RoundUp(5.2,0)</i> returns <b>6</b>

<b>$RoundDown(number,number\_of\_digits\_after\_decimal\_point)</b> : <i>$RoundDown(4.28,1)</i> returns <b>4.2</b>

<b>$Name(\<URL\>)</b> : returns file name without extension and path to file. Type <i>\<URL\></i> exactly like this, don't use other function 
    argument value

<b>$DateCreated(\<URL\>)</b> : returns creation date/time of music file (<i>not</i> last modification date/time)

<b>$Char(hexadecimal\_code)</b> : returns Unicode character with given <i>hexadecimal\_code</i>, e.g. <i>$Char(a7)</i> returns "<b>§</b>" (U+00A7)

<b>$CharN(hexadecimal\_code,decimal\_number\_of\_times)</b> : returns Unicode character with given <i>hexadecimal\_code</i> repeated 
    the given number of times, e.g. <i>$CharN(a7,3)</i> returns "<b>§§§</b>" (U+00A7 repeated 3 times)

<b>$TagContainsAnyString(\<URL\>,tag\_name,string1\|string2\|etc.)</b> : returns "<b>T</b>" if tag contains any of the strings separated by \|, otherwise 
    returns "<b>F</b>". <i>tag\_name</i> must be written without angle brackets, e.g. <i>$TagContainsAnyString(\<URL\>,Lyrics,water\|river)</i>

<b>$TagContainsAllStrings(\<URL\>,tag\_name,string1\|string2\|etc.)</b> : returns "<b>T</b>" if tag contains all strings separated by \|, otherwise returns "<b>F</b>"
</pre>