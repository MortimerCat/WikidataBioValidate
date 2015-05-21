using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikidataBioValidation
{
    /// <summary>
    /// General interface to Wikidata
    /// </summary>
    class WikidataIO : WikimediaApi
    {
        private string Action = "wbgetentities";
        private string Format = "json";
        private string Sites = "enwiki";
        private string Ids = "";
        private string Props = "claims|descriptions|labels|sitelinks";
        private string Languages = "en";

        protected override string APIurl { get { return @"http://www.wikidata.org/w/api.php?"; } }
        protected override string Parameters
        {
            get { 
                string Param = "action=" + Action;
                Param += "&format=" + Format;
                Param += "&sites=" + Sites;
                Param += "&ids=" + Ids;
                Param += "&props=" + Props;
                Param += "&languages=" + Languages;

                return Param;
            }
        }


        public WikidataFields Data { get; private set; }

        public WikidataIO(string wikidataItemID)
        {
            Ids = wikidataItemID;
            GrabPage();

            WikidataExtract Item = new WikidataExtract(Content);
            Data = Item.Fields;


        }
    }
}
