**ASR SPECIAL FUNCTIONS**

You can use special functions in substitution fields of "Advanced Search & Replace" and "Multiple Search & Replace" commands:

<pre>
\@null[[]] : returns "null" Unicode character

\@char[[hexadecimal code]] : returns Unicode character with given hexadecimal code, e.g. \@char[[2f]] returns "/"

\@tc[[string;;excepted words]] : returns Title Cased string except for given words separated by spaces, e.g \@tc[[$1;;a the an>]] will return title cased (except for words "a", "the", "an") 1st captured in search pattern string, and \@tc[[$1]] will return title cased string without any excepted words
Excepted words will be unchanged, not lowercased! To lowercase them, use: \@tc[[\@lc[[string]];;excepted words]]
\@lc[[string;;excepted words]] : returns lower cased string except for given words

\@uc[[string;;excepted words]] : returns UPPER CASED string except for given words

\@sc[[string;;excepted words]] : returns Sentence cased string except for given words

\@eval[[virtual tag expression]] : returns result of calculation of virtual tag expression, e.g. \@eval[[$Sub(<Play Count>,<Skip Count>)]]

\@repunct[[string]] : changes Unicode punctuation marks to ASCII analogs, e.g. « to <<
</pre>