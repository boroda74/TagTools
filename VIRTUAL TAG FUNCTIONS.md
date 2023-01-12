**VIRTUAL TAG FUNCTIONS**

This plugin introduces several new virtual tag functions (they also can be used in various filename templates, e.g. in the file organizer) like 
$Char(hexadecimal code), which returns Unicode character with given hexadecimal code, e.g. $Char(a7) returns Unicode character "§" (U+00A7)

*Note:*

Use these functions like native MusicBee functions (i.e. without any initial setup required by $ALR() and $ASR() functions). 
Everywhere you can use either literals or MusicBee tags of appropriate types as function parameters, e.g. $NumberOfDays(&lt;Year&gt;,&lt;Original Year&gt;) or $SubDateTime(&lt;Date Added&gt;,01/01/2000). 

<pre>
$Random(max_number): random integer number between 0 
&nbsp;&nbsp;&nbsp;&nbsp;and max_number (including them)

$Now(): returns current date/time. Date/time format depends on your Windows 
&nbsp;&nbsp;&nbsp;&nbsp;regional settings, something like 11/01/2022 02:30:00 pm (for US regional 
&nbsp;&nbsp;&nbsp;&nbsp;settings) or 01.11.2022 14:30:00 (for European regional settings)

$AddDuration(duration1,duration2): duration format is similar to date/time format 
&nbsp;&nbsp;&nbsp;&nbsp;without "am/pm" suffix, but allowed values are different, 
&nbsp;&nbsp;&nbsp;&nbsp;e.g. 00/01/0000 00:00:00 is valid duration, but not valid date/time. Either 
&nbsp;&nbsp;&nbsp;&nbsp;date part or time part can be omitted, it will be defaulted to zero value

$SubDuration(duration1,duration2)

$MulDuration(duration,number): multiplies duration by floating point or integer 
&nbsp;&nbsp;&nbsp;&nbsp;number, e.g. $MulDuration(&lt;Time&gt;,&lt;Play Count&gt;)

$SubDateTime(datetime1,datetime2): returns duration

$NumberOfDays(datetime1,datetime2): returns number of days between datetime1 
&nbsp;&nbsp;&nbsp;&nbsp;and datetime2

$AddDurationToDateTime(datetime,duration)

$SubDurationFromDateTime(datetime,duration)

$TitleCase(string)

$SentenceCase(string)

$Sqrt(number): square root. May be useful for gathering some library 
&nbsp;&nbsp;&nbsp;&nbsp;statistics (in conjunction with ALR functions)

$eq(number1,number2): compares 2 integer or fractional numbers, determines if 
&nbsp;&nbsp;&nbsp;&nbsp;number1 is **eq**ual to number2, e.g. $eq(1.0,1) returns "T"

$ne(number1,number2): determines if number1 is **n**ot **e**qual to number2

$gt(number1,number2): determines if number1 is **g**reater **t**han number2

$lt(number1,number2): determines if number1 is **l**ess **t**han to number2

$ge(number1,number2): determines if number1 is **g**reater than or **e**qual to to number2

$le(number1,number2): determines if number1 is **l**ess than or **e**qual to number2

$Round(number,number_of_digits_after_decimal_point): $Round(4.28,1) 
&nbsp;&nbsp;&nbsp;&nbsp;returns 4.3, $Round(5.2,0) returns 5

$RoundUp(number,number_of_digits_after_decimal_point): $RoundUp(5.2,0) 
&nbsp;&nbsp;&nbsp;&nbsp;returns 6

$RoundDown(number,number_of_digits_after_decimal_point): $RoundDown(4.28,1) 
&nbsp;&nbsp;&nbsp;&nbsp;returns 4.2

$Name(&lt;URL&gt;): returns file name without extension and path to file. 
&nbsp;&nbsp;&nbsp;&nbsp;Type &lt;URL&gt; exactly like this, don't use other function argument value

$DateCreated(&lt;URL&gt;): returns creation date/time of music file (not last 
&nbsp;&nbsp;&nbsp;&nbsp;modification date/time)

$Char(hexadecimal code): returns Unicode character with given hexadecimal 
&nbsp;&nbsp;&nbsp;&nbsp;code, e.g. $Char(a7) returns "§" (U+00A7)

$TagContainsAnyString(&lt;URL&gt;,tag_name,string1|string2|etc.): returns "T" if tag 
&nbsp;&nbsp;&nbsp;&nbsp;contains any of the strings separated by |, otherwise returns "F". 
&nbsp;&nbsp;&nbsp;&nbsp;tag_name must be written without angle brackets, 
&nbsp;&nbsp;&nbsp;&nbsp;e.g. $TagContainsAnyString(&lt;URL&gt;,Lyrics,water|river)

$TagContainsAllStrings(&lt;URL&gt;,tag_name,string1|string2|etc.): returns "T" if tag 
&nbsp;&nbsp;&nbsp;&nbsp;contains all strings separated by |, otherwise returns "F". 
</pre>
