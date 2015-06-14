using System;
using System.Collections.Generic;
using WikiAccess;

namespace WikidataBioValidation
{
    /// <summary>
    /// Test and demo program that looks up a Wikidata item, finds the corresponding wikipedia article, finally comparing the two.
    /// </summary>
    class WikiBioSample
    {
        static void Main(string[] args)
        {
            int Qcode = 6214027;

            WikidataBiography WDperson = new WikidataBiography(Qcode);
            List<ErrorLog> Errors = WDperson.GetErrors();

            Console.WriteLine("-----  Wikidata errors-----");
            foreach (ErrorLog thisLog in Errors)
            {
                if (thisLog != null)
                {
                    foreach (ErrorMessage Error in thisLog.Errors)
                    {
                        Console.WriteLine(Error.ToString());
                    }
                }
            }
            Console.WriteLine("-----  End of Wikidata errors-----");

            if (WDperson.Found == true)
            {
                Console.WriteLine("-----  Wikidata info-----");
                Console.WriteLine(WDperson.Qcode);
                Console.WriteLine(WDperson.Name);
                Console.WriteLine(WDperson.Gender);
                Console.WriteLine(WDperson.Description);
                Console.WriteLine(WDperson.InstanceOf);
                Console.WriteLine(WDperson.CitizenOf);
                if (WDperson.DateOfBirth.Count > 0)
                {
                    Console.WriteLine(WDperson.DateOfBirth[0].thisDate);
                    Console.WriteLine(WDperson.DateOfBirth[0].thisPrecision);
                }
                if (WDperson.DateOfDeath.Count > 0)
                {
                    Console.WriteLine(WDperson.DateOfDeath[0].thisDate);
                    Console.WriteLine(WDperson.DateOfDeath[0].thisPrecision);
                }
                Console.WriteLine(WDperson.Wikilink);
                Console.WriteLine("-----  End of Wikidata info-----");


                WikipediaBiography WPperson = new WikipediaBiography(WDperson.Wikilink);

                Console.WriteLine(WPperson.Categories.Count.ToString() + " categories in article");
                Console.WriteLine(WPperson.Templates.Count.ToString() + " templates in article");

                foreach (ErrorLog thisLog in WPperson.GetErrors())
                {
                    if (thisLog != null)
                    {
                        foreach (ErrorMessage Error in thisLog.Errors)
                        {
                            Console.WriteLine(Error.ToString());
                        }
                    }
                }
            }
            /*

                        WikiValidate Vperson = new WikiValidate(WDperson, WPperson);

                        if (Vperson.ErrorMessage != null)
                        {
                            foreach (string errormessage in WPperson.ErrorMessage)
                            {
                                Console.WriteLine(errormessage);
                            }
                        }
             */
        }
    }
}
