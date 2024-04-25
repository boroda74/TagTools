# VIRTUAL TAG FUNCTIONS

Plugin introduces several new virtual tag functions (they also can be used in various filename templates, e.g. in the file organizer).\
&nbsp;

*Note:*

\
Use these functions like native MusicBee functions (i.e. without any initial setup required by $LR() and $ASR() functions). \
Everywhere you can use either literals or MusicBee tags of appropriate types as function parameters, e.g. $NumberOfDays(\<Year\>,\<Original Year\>) or $SubDateTime(\<Date Added\>,01/01/2000).

**$Random(max\_number)** : random integer number between 0 and max\_number (including them)\
&nbsp;\
**$Now()** : returns current date/time. Date/time format depends on your Windows regional settings, something like 11/01/2022 02:30:00 pm (for US regional settings) or 01.11.2022 14:30:00 (for European regional settings)\
&nbsp;\
**$AddDuration(duration1,duration2)** : adds ***duration2*** to ***duration1***.&nbsp; Duration format is similar to date/time format without "am/pm" suffix, but allowed values are different, e.g. 00/01/0000 00:00:00 is valid duration, but is not a valid date/time. Either date part or time part can be omitted. It will be defaulted to zero value\
&nbsp;\
**$SubDuration(duration1,duration2)** : subtracts ***duration2*** from ***duration1***.\
&nbsp;\
**$MulDuration(duration,number)** : multiplies ***duration*** by floating point or integer ***number***, e.g. ***$MulDuration(\<Time\>,\<Play Count\>)*** \
&nbsp;\
**$SubDateTime(datetime1,datetime2)** : subtracts ***datetime2*** from ***datetime1***. Returns duration\
&nbsp;\
**$NumberOfDays(datetime1,datetime2)** : returns the number of days between ***datetime1*** and ***datetime2*** \
&nbsp;\
**$AddDurationToDateTime(datetime,duration)** : adds ***duration*** to ***datetime***\
&nbsp;\
**$SubDurationFromDateTime(datetime,duration)** : subtracts ***duration*** from ***datetime***\
&nbsp;\
**$TitleCase(string,exceptionWords,wordSplitters,exceptionChars)** : excepted words become lowercase, they must be separated by spaces. ***wordSplitters*** is the list of characters separated by spaces, so that the words after them will be capitalized (e.g. \& or . ). ***exceptionChars*** will do the reverse action: the words after them will be lowercase (e.g. " ' \[ { ). You can omit any parameter. It will be ignored in this case. Just put the closing bracket earlier if you leave out one or more last parameters:\
&nbsp; &nbsp; ● $TitleCase(string,,,' \[ {)\
&nbsp; &nbsp; ● $TitleCase(string,the a an)\
&nbsp; &nbsp; ● $TitleCase(string)\
&nbsp;\
**$SentenceCase(string,wordSplitters)** : ***wordSplitters*** is the list of characters separated by spaces, which begins the new sentence, e.g. dot. You can omit the last parameter, then the entire input string will be treated as a single sentence:\
&nbsp; &nbsp; ● $SentenceCase(string,.)\
&nbsp; &nbsp; ● $SentenceCase(string)\
&nbsp;\
**$Sqrt(number)** : square root. May be useful for gathering some library statistics (in conjunction with LR functions) \
&nbsp;\
**$eq(number1,number2)** : compares 2 integer or fractional numbers, determines if number1 is **eq**ual to number2, e.g. ***$eq(1.0,1)*** returns "T" \
&nbsp;\
**$ne(number1,number2)** : determines if number1 is **n**ot **e**qual to number2 \
&nbsp;\
**$gt(number1,number2)** : determines if number1 is **g**reater **t**han number2 \
&nbsp;\
**$lt(number1,number2)** : determines if number1 is **l**ess **t**han to number2 \
&nbsp;\
**$ge(number1,number2)** : determines if number1 is **g**reater than or **e**qual to to number2 \
&nbsp;\
**$le(number1,number2)** : determines if number1 is **l**ess than or **e**qual to number2 \
&nbsp;\
**$Round(number,number\_of\_digits\_after\_decimal\_point)** : ***$Round(4.28,1)*** returns 4.3, and ***$Round(5.2,0)*** returns 5 \
&nbsp;\
**$RoundUp(number,number\_of\_digits\_after\_decimal\_point)** : ***$RoundUp(5.2,0)*** returns 6 \
&nbsp;\
**$RoundDown(number,number\_of\_digits\_after\_decimal\_point)** : ***$RoundDown(4.28,1)*** returns 4.2 \
&nbsp;\
**$Name(\<URL\>)** : returns file name without extension and path to file. Type \<URL\> exactly like this, don't use other function argument value\
&nbsp;\
**$DateCreated(\<URL\>)** : returns creation date/time of music file (***not*** last modification date/time) \
&nbsp;\
**$Char(hexadecimal code)** : returns Unicode character with given hexadecimal code, e.g. ***$Char(a7)*** returns "§" (U+00A7) \
&nbsp;\
**$CharN(hexadecimal code,decimal number of times)** : returns Unicode character with given hexadecimal code repeated the given number of times, e.g. ***$CharN(a7,3)*** returns "§§§" (U+00A7 repeated 3 times) \
&nbsp;\
**$TagContainsAnyString(\<URL\>,tag\_name,string1\|string2\|etc.)** : returns "T" if tag contains any of the strings separated by \|, otherwise returns "F". tag\_name must be written without angle brackets, e.g. ***$TagContainsAnyString(\<URL\>,Lyrics,water\|river)*** \
&nbsp;\
**$TagContainsAllStrings(\<URL\>,tag\_name,string1\|string2\|etc.)** : returns "T" if tag contains all strings separated by \|, otherwise returns "F".
***
_Created with the Personal Edition of HelpNDoc: [Effortlessly Spot and Fix Problems in Your Documentation with HelpNDoc's Project Analyzer](<https://www.helpndoc.com/feature-tour/advanced-project-analyzer/>)_
