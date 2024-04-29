# LR EXPRESSIONS

LR expressions are mostly intended to be used for regexes, though they are not limited to them. LR expressions can save you the number of virtual tags if you need some expressions to be used in LR only, but the main purpose of LR expressions is to use them together with multiple item splitters for grouping tags.

If several virtual tags sourced from a single tag are split into multiple values (e.g., separated by ; ) for a given track, then all combinations of these values will be included in the LR report. Contrary to this, different expressions of one split grouping tag for a given track produce a single combination for every split tag value.

&nbsp;

Two virtual tags sourced from a single "Custom2" tag are split into multiple values:

![Image](<lib/LR-vt.png>)

&nbsp;

Two expressions of one split into multiple values grouping "Custom2" tag:

![Image](<lib/LR-expr.png>)

&nbsp;

You can refer to any tag in an LR expression using the MusicBee generic \<Tag Name\> construction, or refer to the current grouping/function tag (which can already be one of the split tag values if splitter is defined for this tag) as \\@

***
_Created with the Personal Edition of HelpNDoc: [Effortlessly Create Encrypted, Password-Protected PDFs](<https://www.helpndoc.com/step-by-step-guides/how-to-generate-an-encrypted-password-protected-pdf-document/>)_
