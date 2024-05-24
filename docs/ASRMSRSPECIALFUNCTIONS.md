# ASR & MSR SPECIAL FUNCTIONS

You can use special functions in substitution fields of "Advanced Search \& Replace" and "Multiple Search \& Replace" commands:

**\\@null\[\[\]\]** : returns "null" Unicode character. The same as ***\\@char\[\[0\]\]***

**\\@char\[\[hexadecimal\_code\]\]** : returns Unicode character with given ***hexadecimal\_code***, e.g. ***\\@char\[\[2f\]\]*** returns "/"

**\\@tc\[\[string;;ignored\_words\]\]** : returns Title Cased string except for ***ignored\_words*** separated by spaces, e.g ***\\@tc\[\[$1;;a the an\>\]\]*** will return title cased (except for words "a", "the", "an") first captured in search pattern string, and ***\\@tc\[\[$1\]\]*** will return title cased string, not using any ***ignored\_words***. ***ignored\_words*** will be unchanged, *not become lowercase*\! To lowercase them, use: ***\\@tc\[\[@lc\[\[string\]\];;ignored\_words\]\]***. The first and the last words will be title cased always

**\\@lc\[\[string;;ignored\_words\]\]** : returns lower cased string except for ***ignored\_words***

**\\@uc\[\[string;;ignored\_words\]\]** : returns UPPER CASED string except for ***ignored\_words***

**\\@sc\[\[string;;ignored\_words\]\]** : returns Sentence cased string except for ***ignored\_words***

**\\@eval\[\[virtual\_tag\_expression\]\]** : returns result of calculation of ***virtual\_tag\_expression***, e.g. ***\\@eval\[\[$Sub(\<Play Count\>,\<Skip Count\>)\]\]***

**\\@repunct\[\[string\]\]** : changes Unicode punctuation marks to ASCII analogs, e.g. « to \<\<
