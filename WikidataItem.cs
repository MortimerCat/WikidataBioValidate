using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikidataBioValidation
{
    /// <summary>
    /// This class represents an item on Wikidata.org 
    /// </summary>
    class WikidataItem
    {
        public  WikidataFields thisWikidata;

        public WikidataItem(string wikidataItemID)
        {
            WikidataIO WIO = new WikidataIO();

            WIO.Action = "wbgetentities";
            WIO.Format = "json";
            WIO.Sites = "enwiki";
            WIO.Ids = wikidataItemID;
            WIO.Props  = "claims|descriptions|labels|sitelinks";
            WIO.Languages = "en";

            thisWikidata = WIO.GetData();
        }

        public IEnumerable<string> ErrorMessage { get; set; }
    }
}
