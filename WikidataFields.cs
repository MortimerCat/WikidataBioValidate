using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikidataBioValidation
{
    /// <summary>
    /// Container to hold a single item grabbed from Wikidata
    /// </summary>
    class WikidataFields
    {
        public int ID { get; set; }
        public Dictionary<string, string> WikipediaLinks { get; set; }
        public Dictionary<string, string> Labels { get; set; }
        public Dictionary<string, string> Description { get; set; }
        public Dictionary<int, WikidataClaim> Claims { get; set; }

        public WikidataFields()
        {
            WikipediaLinks = new Dictionary<string, string>();
            Labels = new Dictionary<string, string>();
            Description = new Dictionary<string, string>();
            Claims = new Dictionary<int, WikidataClaim>();
        }
    }
}
