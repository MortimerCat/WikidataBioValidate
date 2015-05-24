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
        private WikidataBiography WDperson;
        private WikipediaBiography WPperson;

        public WikiValidate(WikidataBiography WDperson, WikipediaBiography WPperson)
        {
            // TODO: Complete member initialisation
            this.WDperson = WDperson;
            this.WPperson = WPperson;
        }

        public IEnumerable<string> ErrorMessage { get; set; }
    }
}
