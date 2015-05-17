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
        private string WikidataItemID;

        public WikidataItem(string wikidataItemID)
        {

        }

        public string WikipediaLink { get; set; }

        public IEnumerable<string> ErrorMessage { get; set; }
    }
}
