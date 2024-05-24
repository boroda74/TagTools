# VIRTUAL TAG FUNCTIONS

Plugin introduces several new virtual tag functions (they also can be used in various filename templates, e.g. in the file organizer).

**&nbsp;

*Note:*



**Use these functions like native MusicBee functions (i.e. without any initial setup required by $LR() and $ASR() functions).

**Everywhere you can use either literals or MusicBee tags of appropriate types as function parameters, e.g. $NumberOfDays(\<Year\>,\<Original Year\>) or $SubDateTime(\<Date Added\>,01/01/2000).

**$Random(max\_number)** : random integer number between 0 and *max\_number* (including them)

**$Now()** : returns current date/time. Date/time format depends on your Windows regional settings, something like **11/01/2022 02:30:00 pm** (for US regional settings) or **01.11.2022 14:30:00** (for European regional settings)

**$AddDuration(duration1,duration2)** : adds *duration2* to *duration1*. Duration format is similar to date/time format without "**am/pm**" suffix, but allowed values are different, e.g. **00/01/0000 00:00:00** is valid duration, but is not a valid date/time. Either date part or time part can be omitted. It will be defaulted to zero value

**$SubDuration(duration1,duration2)** : subtracts *duration2* from *duration1*

**$MulDuration(duration,number)** : multiplies *duration* by floating point or integer *number*, e.g. *$MulDuration(\<Time\>,\<Play Count\>)*

**$SubDateTime(datetime1,datetime2)** : subtracts *datetime2* from *datetime1*. Returns duration

**$NumberOfDays(datetime1,datetime2)** : returns the number of days between *datetime1* and *datetime2*

**$AddDurationToDateTime(datetime,duration)** : adds *duration* to *datetime*

**$SubDurationFromDateTime(datetime,duration)** : subtracts *duration* from *datetime*

**$SentenceCase(string,sentence\_separators)** : where ***sentence\_separators*** is the list of characters after which the words will become capitalized. Characters in the ***sentence\_separators*** list may be separated by spaces or not separated at all. For example, these two character lists mean the same: "**. , -**" and "**.,-**". A dot, followed by a space, is always treated as a sentence separator. You can omit the last parameter simply by putting the closing parenthesis earlier, i.e., *$SentenceCase(string)*

**$TitleCase(string,lower\_case\_words,word\_separators,lower\_case\_between\_chars\_opening\_list,lower\_case\_between\_chars\_closing\_list,lower\_case\_after\_chars)** : where *lower\_case\_words* is the list of words separated by spaces, which will become lowercase (*always except for the first word* and *except for the last words if not lowercase by other parameters*). *word\_separators* is the list of characters after which the words will be capitalized. *lower\_case\_between\_chars\_opening\_list* and *lower\_case\_between\_chars\_closing\_list* are the lists of characters *between* which the words become lowercase. The number of characters in both lists must be the same; the opening and closing characters are paired by their position in the lists (the lists may be, e.g., "**( \[ {**" and "**) \] }**"). *lower\_case\_after\_chars* consists of a list of characters, *after* which the words become lowercase (e.g., **'** for something like "Someone#8202;**'**&#8202;s"). *The first word* will *always* be capitalized. It’s recommended to enclose the last four lists into quotes (e.g., **"\& . -"**). Characters in all character lists may be separated by spaces or not separated at all. For example, these two character lists mean the same: "**. , -**" and "**.,-**". You can omit any parameter except for the first one, using a single character **\`** *not* surrounded by spaces to pass empty parameter. You can *safely use* character **\`** in its literal meaning if it’s typed in the list *along with other characters*. To use *the single character* **\`** in its literal meaning (*not* as an omitted parameter) type "**\\\`**", and to use *the single character* **\\** in its literal meaning type "**\\\\**". You can omit any number of end parameters simply by putting closing parenthesis earlier, e.g., *$TitleCase(string,lower\_case\_words,word\_separators)*

**$Sqrt(number)** : square root. May be useful for collecting some library statistics (in conjunction with LR functions)

**$eq(number1,number2)** : compares two integer or fractional numbers, determines if *number1* is **eq**ual to *number2*, e.g. *$eq(1.0,1)* returns "**T**"

**$ne(number1,number2)** : returns "**T**" if *number1* is **n**ot **e**qual to *number2*, otherwise returns "**F**"

**$gt(number1,number2)** : returns "**T**" if *number1* is **g**reater **t**han *number2*, otherwise returns "**F**"

**$lt(number1,number2)** : returns "**T**" if *number1* is **l**ess **t**han to *number2*, otherwise returns "**F**"

**$ge(number1,number2)** : returns "**T**" if *number1* is **g**reater than or **e**qual to to *number2*, otherwise returns "**F**"

**$le(number1,number2)** : returns "**T**" if *number1* is **l**ess than or **e**qual to *number2*, otherwise returns "**F**"

**$Round(number,number\_of\_digits\_after\_decimal\_point)** : *$Round(4.28,1)* returns **4.3**, and *$Round(5.2,0)* returns **5**

**$RoundUp(number,number\_of\_digits\_after\_decimal\_point)** : *$RoundUp(5.2,0)* returns **6**

**$RoundDown(number,number\_of\_digits\_after\_decimal\_point)** : *$RoundDown(4.28,1)* returns **4.2**

**$Name(\<URL\>)** : returns file name without extension and path to file. Type *\<URL\>* exactly like this, don't use other function argument value

**$DateCreated(\<URL\>)** : returns creation date/time of music file (*not* last modification date/time)

**$Char(hexadecimal\_code)** : returns Unicode character with given *hexadecimal\_code*, e.g. *$Char(a7)* returns "**§**" (U+00A7)

**$CharN(hexadecimal\_code,decimal\_number\_of\_times)** : returns Unicode character with given *hexadecimal\_code* repeated the given number of times, e.g. *$CharN(a7,3)* returns "**§§§**" (U+00A7 repeated 3 times)

**$TagContainsAnyString(\<URL\>,tag\_name,string1\|string2\|etc.)** : returns "**T**" if tag contains any of the strings separated by \|, otherwise returns "**F**". *tag\_name* must be written without angle brackets, e.g. *$TagContainsAnyString(\<URL\>,Lyrics,water\|river)*

**$TagContainsAllStrings(\<URL\>,tag\_name,string1\|string2\|etc.)** : returns "**T**" if tag contains all strings separated by \|, otherwise returns "**F**"
