using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikidataBioValidation
{
    /// <summary>
    /// Test and demo program that looks up a Wikidata item, finds the corresponding wikipedia article, finally comparing the two.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            int Qcode = 83235;

            WikidataBiography WDperson = new WikidataBiography(Qcode);
                        
            Console.WriteLine(WDperson.Qcode);
            Console.WriteLine(WDperson.Name);
            Console.WriteLine(WDperson.Gender);
            Console.WriteLine(WDperson.Description);
            Console.WriteLine(WDperson.InstanceOf);
            Console.WriteLine(WDperson.CitizenOf);
            Console.WriteLine(WDperson.DateOfBirth.thisDate);
            Console.WriteLine(WDperson.DateOfBirth.thisPrecision);
            Console.WriteLine(WDperson.DateOfDeath.thisDate);
            Console.WriteLine(WDperson.DateOfDeath.thisPrecision);
            Console.WriteLine(WDperson.Wikilink);

            if (WDperson.ErrorMessage != null)
            {
                foreach (string errormessage in WDperson.ErrorMessage)
                {
                    Console.WriteLine(errormessage);
                }
            }


            WikipediaBiography WPperson = new WikipediaBiography(WDperson.Wikilink);

            if (WPperson.ErrorMessage != null)
            {
                foreach (string errormessage in WPperson.ErrorMessage)
                {
                    Console.WriteLine(errormessage);
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
