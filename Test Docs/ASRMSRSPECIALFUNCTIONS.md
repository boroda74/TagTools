# ASR & MSR SPECIAL FUNCTIONS

You can use special functions in substitution fields of "Advanced Search \& Replace" and "Multiple Search \& Replace" commands:

**\\@null\[\[\]\]** : returns "null" Unicode character. The same as ***\\@char\[\[0\]\]***\
&nbsp;\
**\\@char\[\[hexadecimal code\]\]** : returns Unicode character with given hexadecimal code, e.g. ***\\@char\[\[2f\]\]*** returns "/" \
&nbsp;\
**\\@tc\[\[string;;excepted words\]\]** : returns Title Cased string except for given words separated by spaces, e.g ***\\@tc\[\[$1;;a the an\>\]\]*** will return title cased (except for words "a", "the", "an") 1st captured in search pattern string, and ***\\@tc\[\[$1\]\]*** will return title cased string, not using any excepted words. Excepted words will be unchanged, **not** become *lowercase*\! To lowercase them, use: ***\\@tc\[\[@lc\[\[string\]\];;excepted words\]\]*** \
&nbsp;\
**\\@lc\[\[string;;excepted words\]\]** : returns lower cased string except for the given words \
&nbsp;\
**\\@uc\[\[string;;excepted words\]\]** : returns UPPER CASED string except for the given words \
&nbsp;\
**\\@sc\[\[string;;excepted words\]\]** : returns Sentence cased string except for the given words \
&nbsp;\
**\\@eval\[\[virtual tag expression\]\]** : returns result of calculation of virtual tag expression, e.g. ***\\@eval\[\[$Sub(\<Play Count\>,\<Skip Count\>)\]\]***\
\
**\\@repunct\[\[string\]\]** : changes Unicode punctuation marks to ASCII analogs, e.g. « to \<\<

***
_Created with the Personal Edition of HelpNDoc: [Keep Your Sensitive PDFs Safe with These Easy Security Measures](<https://www.helpndoc.com/step-by-step-guides/how-to-generate-an-encrypted-password-protected-pdf-document/>)_
