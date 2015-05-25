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


Wiki Interface
==============
For a general interface to Wikidata and Wikipedia, the following files are required. One day I might split these off into a project of their own.

WikimediaApi.cs  This is the class that handles the Internet activity to grab the page. It is a base class for the following IO classes.

WikidataIO.cs  This class retrieves an item from Wikidata. It doesn't do much except store the data within a class
  WikidataFields.cs  Container class to store a Wikidata item
  WikidataClaim.cs Container class to store a "Claim"
  WikidataCache.cs Local cache of property labels
  WikidataExtract.cs Class which interprets the JSON exported by Wikidata (requires Newtonsoft)
 

WikipediaIO.cs This class retrieves a complete article from Wikipedia. This retrieves the text in its raw state, as seen when editing a Wikipedia page. There are methods to allow extraction of templates and categories.


Wikidate.cs - Class to store a date and its precision.