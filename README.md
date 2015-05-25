# WikidataBioValidate
https://www.wikidata.org/wiki/User:Periglio

History
=======

I have a website with a celebrity birth/death trivia generator. "who is 50 years old today?" My database is an extract
of Wikidata (a sister project of Wikipedia). I was finding lots of problems within the data, so I started on this project
to try to clear up some of the mess. I am now converting my code to an open format and have started this repository.

Description
===========

Basically, the software grabs a biographical entry from Wikidata and performs sanity checks on the dates. Next, it
finds the associate Wikipedia article and checks for discrepancies between templates and categories. Finally, it compares
the data between wikidata and wikipedia.

The front end here is just a single lookup. My production version downloads a batch of numbers from my own database, and loops
through these one at a time.
