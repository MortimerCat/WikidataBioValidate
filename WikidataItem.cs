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
            WikidataIO WIO = new WikidataIO(wikidataItemID);
            thisWikidata = WIO.Data;
        }

        public IEnumerable<string> ErrorMessage { get; set; }
    }
}
