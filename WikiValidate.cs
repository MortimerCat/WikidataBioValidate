using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikidataBioValidation
{
    /// <summary>
    /// This class compares the data between Wikidata and Wikipedia
    /// </summary>
    class WikiValidate
    {
        private WikidataItem WDperson;
        private WikipediaBioArticle WPperson;

        public WikiValidate(WikidataItem WDperson, WikipediaBioArticle WPperson)
        {
            // TODO: Complete member initialisation
            this.WDperson = WDperson;
            this.WPperson = WPperson;
        }
    }
}
