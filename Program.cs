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

            Console.WriteLine(WDperson.thisWikidata.ID);
            Console.WriteLine(WDperson.thisWikidata.Name);
            Console.WriteLine(WDperson.thisWikidata.Gender);
            Console.WriteLine(WDperson.thisWikidata.Description);
            Console.WriteLine(WDperson.thisWikidata.InstanceOf);
            Console.WriteLine(WDperson.thisWikidata.CitizenOf);
            Console.WriteLine(WDperson.thisWikidata.DateOfBirth.thisDate);
            Console.WriteLine(WDperson.thisWikidata.DateOfBirth.thisPrecision);
            Console.WriteLine(WDperson.thisWikidata.DateOfDeath.thisDate);
            Console.WriteLine(WDperson.thisWikidata.DateOfDeath.thisPrecision);
            Console.WriteLine(WDperson.thisWikidata.WikipediaLink);


            if (WDperson.ErrorMessage != null)
            {
                foreach (string errormessage in WDperson.ErrorMessage)
                {
                    Console.WriteLine(errormessage);
                }
            }

            WikipediaBioArticle WPperson = new WikipediaBioArticle(WDperson.thisWikidata.WikipediaLink);

            if (WPperson.ErrorMessage != null)
            {
                foreach (string errormessage in WPperson.ErrorMessage)
                {
                    Console.WriteLine(errormessage);
                }
            }

            WikiValidate Vperson = new WikiValidate(WDperson, WPperson);

            if (Vperson.ErrorMessage != null)
            {
                foreach (string errormessage in WPperson.ErrorMessage)
                {
                    Console.WriteLine(errormessage);
                }
            }
        }
    }
}
