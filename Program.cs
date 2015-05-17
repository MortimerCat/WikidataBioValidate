using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikidataBioValidation
{
    /// <summary>
    /// Test and demo program that looks up a wikidata item, finds the corresponding wikipedia article, finally comparing the two.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string WikidataItemID = "Q11662";

            WikidataItem WDperson = new WikidataItem(WikidataItemID);

            foreach(string errormessage in WDperson.ErrorMessage)
            {
                Console.WriteLine(errormessage);
            }
            
            WikipediaBioArticle WPperson = new WikipediaBioArticle(WDperson.thisWikidata.WikipediaLink);

            foreach (string errormessage in WPperson.ErrorMessage)
            {
                Console.WriteLine(errormessage);
            }

            WikiValidate Vperson = new WikiValidate(WDperson,WPperson);

            foreach (string errormessage in WPperson.ErrorMessage)
            {
                Console.WriteLine(errormessage);
            }
        }
    }
}
