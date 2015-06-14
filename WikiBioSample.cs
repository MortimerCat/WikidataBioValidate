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
            int Qcode = 219640;

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
                    Console.WriteLine(WDperson.DateOfBirth[0].ToString());
                }
                if (WDperson.DateOfDeath.Count > 0)
                {
                    Console.WriteLine(WDperson.DateOfDeath[0].ToString());
                }
                Console.WriteLine(WDperson.Wikilink);
                Console.WriteLine("-----  End of Wikidata info -----");

                Console.WriteLine("--- Wikipedia article " + WDperson.Wikilink + " ---");
                WikipediaBiography WPperson = new WikipediaBiography(WDperson.Wikilink);

                Console.WriteLine(WPperson.Categories.Count.ToString() + " categories in article");
                Console.WriteLine(WPperson.Templates.Count.ToString() + " templates in article");
                Console.WriteLine("DEFAULTSORT " + WPperson.DefaultSort);
                Console.WriteLine("DOB : " + WPperson.BirthDate.ToString());
                Console.WriteLine("DOD : " + WPperson.DeathDate.ToString());

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
                Console.WriteLine("--- End of Wikipedia article ---");

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
