using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikidataBioValidation
{
    /// <summary>
    /// Interface to Wikidata
    /// </summary>
    class WikidataIO
    {
        private string wikidataItemID;

        public WikidataIO(string wikidataItemID)
        {
            // TODO: Complete member initialization
            this.wikidataItemID = wikidataItemID;
        }

        public WikidataFields data { get; set; }
    }
}
