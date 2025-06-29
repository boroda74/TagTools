# VIRTUAL TAG FUNCTIONS

Plugin introduces several new virtual tag functions (they also can be used in various filename templates, e.g. in the file organizer).

<b><i>Note:</i></b>

Use these functions like native MusicBee functions (i.e. without any initial setup required by $LR() and $ASR() functions). 
Everywhere you can use either literals or MusicBee tags of appropriate types as function parameters, 
e.g. $NumberOfDays(&lt;Year&gt;,&lt;Original Year&gt;) or $SubDateTime(&lt;Date Added&gt;,01/01/2000).

<pre>
<b>$Random(max_number)</b> : random integer number between <b>0</b> and <b><i>max_number</i></b> (including them)
<b>$Now()</b> : returns current date/time. Date/time format depends on your Windows regional settings, something 
    like <b>11/01/2022 02:30:00 pm</b> (for US regional settings) or <b>01.11.2022 14:30:00</b> (for European regional 
    settings)

<b>$AddDuration(duration1,duration2)</b> : adds <b><i>duration2</i></b> to <b><i>duration1</i></b>. Duration format is <b><i>dddddd.hh:mm:ss</i></b>, 
    where <b><i>dddddd</i></b> is the number of days (without leading zeros) and <b><i>hh</i></b>, <b><i>mm</i></b>, <b><i>ss</i></b> are hours, minutes and 
    seconds (with leading zeros except for the most left zeros). If some most left parts of duration are zero, 
    they are omitted (e.g., <b>260.02:20:05</b> or <b>5:25</b>)

<b>$SubDuration(duration1,duration2)</b> : subtracts <b><i>duration2</i></b> from <b><i>duration1</i></b>

<b>$MulDuration(duration,number)</b> : multiplies <b><i>duration</i></b> by floating point or integer <b><i>number</i></b>, 
    e.g. <b><i>$MulDuration(&lt;Time&gt;,&lt;Play Count&gt;)</i></b>

<b>$SubDateTime(datetime1,datetime2)</b> : subtracts <b><i>datetime2</i></b> from <b><i>datetime1</i></b>. Returns duration

<b>$NumberOfDays(datetime1,datetime2)</b> : returns the number of days between <b><i>datetime1</i></b> and <b><i>datetime2</i></b>

<b>$AddDurationToDateTime(datetime,duration)</b> : adds <b><i>duration</i></b> to <b><i>datetime</i></b>

<b>$SubDurationFromDateTime(datetime,duration)</b> : subtracts <b><i>duration</i></b> from <b><i>datetime</i></b>

<b>$SentenceCase(string,upper_case_words,sentence_separators)</b> : <b><i>upper_case_words</i></b> will become uppercase. 
    You can use special pseudo-word "<b>#RN</b>" in <b><i>upper_case_words</i></b> list to uppercase 
    <i>Roman numerals</i> (e.g., <b>III</b>, <b>VI</b>, <b>X</b>). <b><i>sentence_separators</i></b> is the list of characters, after which the words will 
    become capitalized. Characters in the <b><i>sentence_separators</i></b> list may be separated by spaces or not 
    separated at all. For example, these two character lists mean the same: "<b>. , -</b>" and "<b>.,-</b>". A dot, followed by 
    a space, is always treated as a sentence separator. You can omit any parameter except for the first one, 
    using a single character <b>`</b> <b><i>not</i></b> surrounded by spaces to pass empty parameter. You can <b><i>safely use</i></b> 
    character <b><i>`</i></b> in its literal meaning if it’s typed in the list <b><i>along with other characters</i></b> or using a single 
    character <b>`</b> <b><i>surrounded</i></b> by spaces. Also, you can omit any parameter(s) in the middle of the parameter list 
    by simply typing two (or more, for more omitted parameters) commas in a row if you are using 
    <i>Musicbee virtual tag editor</i>, e.g. <b><i>$SentenceCase(&lt;Titlegt;>,,:)</i></b>. <i>Musicbee virtual
    tag</i> 
    <i>editor</i> will auto-add empty quotes for you (i.e. <b><i>$SentenceCase&lt;Title&gt;,"",":")</i></b>), and this is acceptable

    If you are using the plugin with MusicBee 3.6 or later, then you can omit any number of the 
    end parameters simply by putting closing parenthesis earlier, e.g., $SentenceCase(string) 
    or $SentenceCase(string,upper_case_words)

<b>$TitleCase(string,lower_case_words,upper_case_words,lower_case_words_between_brackets,sentence_separators)</b>: 
    where <b><i>lower_case_words</i></b> is the list of words separated by spaces, which will become 
    lowercase (<b><i>always except for the first word</i></b> and <b><i>except for the last words</i></b> if not lowercase by <b><i>other parameters</i></b>). 
    <b><i>upper_case_words</i></b> will become uppercase. This rule <i>overrides all other rules</i>. 
    <b><i>lower_case_words_between_brackets</i></b> is the list of words separated by spaces, which will become lowercase 
    between round, square and curly brackets, i.e., "<b>( [ {</b>" and "<b>) ] }</b>"). You can use special pseudo-word "<b>III</b>, <b>VI</b>, <b>X</b>" in 
    <b><i>lower_case_words</i></b>, <b><i>upper_case_words</i></b> and <b><i>lower_case_words_between_brackets</i></b> lists to include 
    <i>Roman numerals</i> (e.g., <b>III</b>, <b>VI</b>, <b>X</b>) in the corresponding list. <b><i>sentence_separators</i></b>, is the list of characters after 
    which the words will become capitalized (switching off all exceptions, e.g. <b><i>lower_case_words</i></b>, besides the words 
    from the <b><i>upper_case_words</i></b>). <b><i>The first word</i></b> will <b><i>always</i></b> be capitalized (besides the words from the <b><i>upper_case_words</i></b>). 
    It’s recommended to enclose the last four lists into quotes (e.g., "<b>\& . -</b>"). Characters in all character lists may 
    be separated by spaces or not separated at all. For example, these two character lists mean the same: "<b>. , -</b>" and "<b>.,-</b>". 
    You can omit any parameter except for the first one, using a single character <b>`</b>  <b><i>not</i></b> surrounded by spaces 
    to pass empty parameter. You can <b><i>safely use</i></b> character <b>`</b> in its literal meaning if it’s typed in the list 
    <b><i>along with other characters</i></b> or using a single character <b>`</b> <b><i>surrounded</i></b> by spaces. Also, you can omit any parameter(s) 
    in the middle of the parameter list by simply typing two (or more, for more omitted parameters) commas in a row 
    if you are using <i>Musicbee virtual tag editor</i>, e.g. <b><i>$TitleCase(&lt;Title&gt;,,,,:)</i></b>). <i>Musicbee virtual tag editor</i> will 
    auto-add empty quotes for you (i.e. <b><i>$TitleCase(&lt;Title&gt;,,,,:)</i></b>), and this is acceptable

    If you are using the plugin with MusicBee 3.6 or later, then you can omit any 
    number of the end parameters simply by putting closing parenthesis earlier, 
    e.g., $TitleCase(string,lower_case_words,upper_case_words)

<b>$SortMultiValues(<multi_value_tag_name>,separator)</b> : alphabetically sorts multi-value tag containing 
    multiple items separated by special character (or special sequence of characters/special string), 
    e.g. <b><i>$SortMultiValues(&lt;Displayed Artist&gt;,"; ")</i></b> will return "<b>John Lennon; Paul McCartney</b>" for tag value 
    "<b>Paul McCartney; John Lennon</b>". It's recommended to include required spaces around/after separator 
    character to preserve them in the result. (e.g. "<b>; </b>" or "<b> / </b>"). 

<b>$Sqrt(number)</b> : square root. May be useful for collecting some library statistics (in conjunction with 
    LR functions)

<b>$eq(number1,number2)</b> : compares two integer or fractional numbers, determines if <b><i>number1</i></b> is 
    <b>eq</b>ual to <b><i>number2</i></b>, e.g. <b><i>$eq(1.0,1)</i></b> returns "<b>T</b>"

<b>$ne(number1,number2)</b> : returns "<b>T</b>" if <b><i>number1</i></b> is <b>n</b>ot <b>e</b>qual to <b><i>number2</i></b>, otherwise returns "<b>F</b>"

<b>$gt(number1,number2)</b> : returns "<b>T</b>" if <b><i>number1</i></b> is <b>g</b>reater <b>t</b>han <b><i>number2</i></b>, otherwise returns "<b>F</b>"

<b>$lt(number1,number2)</b> : returns "<b>T</b>" if <b><i>number1</i></b> is <b>l</b>ess <b>t</b>han to <b><i>number2</i></b>, otherwise returns "<b>F</b>"

<b>$ge(number1,number2)</b> : returns "<b>T</b>" if <b><i>number1</i></b> is <b>g</b>reater than or <b>e</b>qual to to <b><i>number2</i></b>, 
    otherwise returns "<b>F</b>"

<b>$le(number1,number2)</b> : returns "<b>T</b>" if <b><i>number1</i></b> is <b>l</b>ess than or <b>e</b>qual to <b><i>number2</i></b>, 
    otherwise returns "<b>F</b>"

<b>$Round(number,number_of_digits_after_decimal_point)</b> : <b><i>$Round(4.28,1)</i></b> returns <b>4.3</b>, 
    and <b><i>$Round(5.2,0)</i></b> returns <b>5</b>

<b>$RoundUp(number,number_of_digits_after_decimal_point)</b> : <b><i>$RoundUp(5.2,0)</i></b> returns <b>6</b>

<b>$RoundDown(number,number_of_digits_after_decimal_point)</b> : <b><i>$RoundDown(4.28,1)</i></b> returns <b>4.2</b>

<b>$Name(&lt;URL&gt;)</b> : returns file name without extension and path to file. Type <b><i>&lt;URL&gt;</i></b> exactly like this, 
    don't use other function argument value

<b>$DateCreated(&lt;URL&gt;)</b> : returns creation date/time of music file (<b><i>not</i></b> last modification date/time)

<b>$Char(hexadecimal_code)</b> : returns Unicode character with given <b><i>hexadecimal_code</i></b>, 
    e.g. <b><i>$Char(a7)</i></b> returns "<b>§</b>" (U+00A7)

<b>$CharN(hexadecimal_code,decimal_number_of_times)</b> : returns Unicode character with 
    given <b><i>hexadecimal_code</i></b> repeated the given number of times, e.g. <b><i>$CharN(a7,3)</i></b> 
    returns "<b>§§§</b>" (U+00A7 repeated 3 times)

<b>$TagContainsAnyString(&lt;URL&gt;,tag_name,string1|string2|etc.)</b> : returns "<b>T</b>" if tag contains any of 
    the strings separated by |, otherwise returns "<b>F</b>". <i>tag_name</i> must be written without angle brackets, 
    e.g. <i>$TagContainsAnyString(&lt;URL&gt;,Lyrics,water|river)</i>

<b>$TagContainsAllStrings(&lt;URL&gt;,tag_name,string1|string2|etc.)</b> : returns "<b>T</b>" if tag contains all strings 
    separated by |, otherwise returns "<b>F</b>"
</pre>

***

Copyright © boroda 2012-2025. Help version 9.3.250623