# ASR & MSR SPECIAL FUNCTIONS

You can use special functions in substitution fields of "Advanced Search \& Replace" and "Multiple Search \& Replace" commands:

<pre>
<b>\\@null\[\[\]\]</b> : returns "null" Unicode character. The same as <b><i>\\@char\[\[0\]\]</i></b>

<b>\\@char\[\[hexadecimal\_code\]\]</b> : returns Unicode character with given <b><i>hexadecimal\_code</i></b>, 
    e.g. <b><i>\\@char\[\[2f\]\]</i></b> returns "/"

<b>\\@tc\[\[string;;ignored\_words\]\]</b> : returns Title Cased string except for <b><i>ignored\_words</i></b> separated 
    by spaces, e.g <b><i>\\@tc\[\[$1;;a the an\>\]\]</i></b> will return title cased (except for words "a", "the", "an") first 
    captured in search pattern string, and <b><i>\\@tc\[\[$1\]\]</i></b> will return title cased string, not using 
    any <b><i>ignored\_words</i></b>. <b><i>ignored\_words</i></b> will be unchanged, <i>not become lowercase</i>\! To lowercase 
    them, use: <b><i>\\@tc\[\[@lc\[\[string\]\];;ignored\_words\]\]</i></b>. The first and the last words will be title cased always

<b>\\@lc\[\[string;;ignored\_words\]\]</b> : returns lower cased string except for <b><i>ignored\_words</i></b>

<b>\\@uc\[\[string;;ignored\_words\]\]</b> : returns UPPER CASED string except for <b><i>ignored\_words</i></b>

<b>\\@sc\[\[string;;ignored\_words\]\]</b> : returns Sentence cased string except for <b><i>ignored\_words</i></b>

<b>\\@eval\[\[virtual\_tag\_expression\]\]</b> : returns result of calculation of <b><i>virtual\_tag\_expression</i></b>, 
    e.g. <b><i>\\@eval\[\[$Sub(\<Play Count\>,\<Skip Count\>)\]\]</i></b>

<b>\\@repunct\[\[string\]\]</b> : changes Unicode punctuation marks to ASCII analogs, e.g. « to \<\<
</pre>