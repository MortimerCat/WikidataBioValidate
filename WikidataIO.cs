using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikidataBioValidation
{
    /// <summary>
    /// Interface to Wikidata
    /// </summary>
    class WikidataIO : WikimediaApi
    {
        protected override string APIurl { get { return @"http://www.wikidata.org/w/api.php?"; } }

        public WikidataFields Data { get; private set; }

        public WikidataIO(string wikidataItemID)
        {
            Data = new WikidataFields()
            {
                ID = wikidataItemID
            };

            RetrieveData();
        }

        private void RetrieveData()
        {
            throw new NotImplementedException();
        }




    }
}
