# ASR & MSR SPECIAL FUNCTIONS

You can use special functions in substitution fields of "Advanced Search \& Replace" and "Multiple Search \& Replace" commands:

<pre>
<b>\@null[[]]</b> : returns "null" Unicode character. The same as <b><i>\@char[[0]]</i></b>

<b>\@char[[hexadecimal_code]]</b> : returns Unicode character with given <b><i>hexadecimal_code</i></b>, 
    e.g. <b><i>\@char[[2f]]</i></b> returns "/"

<b>\@tc[[string;;ignored_words]]</b> : returns Title Cased string except for <b><i>ignored_words</i></b> separated 
    by spaces, e.g <b><i>\@tc[[$1;;a the an&gt;]]</i></b> will return title cased (except for words "a", "the", "an") first 
    captured in search pattern string, and <b><i>\@tc[[$1]]</i></b> will return title cased string, not using 
    any <b><i>ignored_words</i></b>. <b><i>ignored_words</i></b> will be lowercase. The first and the last words will be title cased always

<b>\@lc[[string;;ignored_words]]</b> : returns lower cased string except for <b><i>ignored_words</i></b>

<b>\@uc[[string;;ignored_words]]</b> : returns UPPER CASED string except for <b><i>ignored_words</i></b>

<b>\@sc[[string;;ignored_words]]</b> : returns Sentence cased string except for <b><i>ignored_words</i></b>

<b>\@eval[[virtual_tag_expression]]</b> : returns result of calculation of <b><i>virtual_tag_expression</i></b>, 
    e.g. <b><i>\@eval[[$Sub(&lt;Play Count&gt;,&lt;Skip Count&gt;)]]</i></b>

<b>\@repunct[[string]]</b> : changes Unicode punctuation marks to ASCII analogs, e.g. « to &lt;&lt;
</pre>