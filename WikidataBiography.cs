using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiAccess;

namespace WikidataBioValidation
{
    /// <summary>
    /// Class to hold a Biographical Wikidata item, just the item I personally need!
    /// </summary>
    class WikidataBiography
    {
        private readonly string[] CLAIMSREQUIRED = { "P21", "P27", "P31", "P569", "P570" };
        public int Qcode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Wikilink { get; set; }
        public List<Wikidate> DateOfBirth { get; set; }
        public List<Wikidate> DateOfDeath { get; set; }
        public string Gender { get; set; }
        public string CitizenOf { get; set; }
        public string InstanceOf { get; set; }
        public WikidataBiographyErrorLog WikidataBioErrors { get; set; }
        public bool Found { get; set; }
        public bool Valid { get; set; }
        private List<ErrorLog> IOErrors { get; set; }
        private bool isHuman = false;

        public WikidataBiography(int qcode)
        {
            WikidataBioErrors = new WikidataBiographyErrorLog();
            Qcode = qcode;

            WikidataIO WIO = new WikidataIO();
            WIO.Action = "wbgetentities";
            WIO.Format = "json";
            WIO.Sites = "";
            WIO.Ids = qcode;
            WIO.Props = "claims|descriptions|labels|sitelinks";
            WIO.Languages = "";
            WIO.ClaimsRequired = CLAIMSREQUIRED;

            WikidataFields Fields = WIO.GetData();
            IOErrors = WIO.GetErrors();

            if (Fields == null)
            {
                Found = false;
                WikidataBioErrors.CannotRetrieveData();
            }
            else
            {
                Found = true;
                ExtractFields(Fields);
            }
        }

        private void ExtractFields(WikidataFields wikidataFields)
        {
            DateOfBirth = new List<Wikidate>();
            DateOfDeath = new List<Wikidate>();

            string ThisName;
            if (!wikidataFields.Labels.TryGetValue("en-gb", out ThisName))
                if (!wikidataFields.Labels.TryGetValue("en", out ThisName))
                    WikidataBioErrors.NoEnglishName();

            Name = ThisName;

            string ThisDescription = "";
            if (!wikidataFields.Description.TryGetValue("en-gb", out ThisDescription))
                if (!wikidataFields.Description.TryGetValue("en", out ThisDescription))
                    WikidataBioErrors.NoEnglishDescription();
            Description = ThisDescription;

            string ThisWikilink = "";
            if (!wikidataFields.WikipediaLinks.TryGetValue("enwiki", out ThisWikilink))
                WikidataBioErrors.NoENwiki();
            Wikilink = ThisWikilink;

            foreach (string thisClaim in CLAIMSREQUIRED)
            {
                IEnumerable<WikidataClaim> Claims = from val in wikidataFields.Claims where val.Key == Convert.ToInt32(thisClaim.Substring(1)) select val.Value;

                foreach (WikidataClaim Claim in Claims)
                {
                    switch (thisClaim)
                    {
                        case "P21":
                            ValidateP21(Claim);
                            break;

                        case "P27":
                            ValidateP27(Claim);
                            break;

                        case "P31":
                            ValidateP31(Claim);
                            break;

                        case "P569":
                            ValidateP569(Claim);
                            break;

                        case "P570":
                            ValidateP570(Claim);
                            break;
                    }
                }
            }
            FinalValidation();
        }

        private void FinalValidation()
        {
            if (string.IsNullOrWhiteSpace(Gender)) WikidataBioErrors.NoGender();
            if (string.IsNullOrWhiteSpace(InstanceOf)) WikidataBioErrors.NoInstance();
            if (string.IsNullOrWhiteSpace(CitizenOf)) WikidataBioErrors.NoCitizenship();
            if (!isHuman) WikidataBioErrors.NotHuman();
            if (DateOfBirth.Count == 0) WikidataBioErrors.NoBirth();

            if (DateOfBirth.Count != 0)
            {
                if (DateOfDeath.Count == 0)
                    ValidateLiving();
                else
                    ValidateDeceased();
            }
         }

        private void ValidateLiving()
        {
            Wikidate thisBirth = DateOfBirth.First();
            if (Wikidate.isCalculable(thisBirth.thisPrecision))
            {
               TimeSpan Span = DateTime.Now - thisBirth.thisDate;
               int Age = (int)Math.Ceiling((double)(Span.Days / 365.25));

               if (Age < 16)
               {
                       if (!ChildException()) WikidataBioErrors.TooYoung();
               }
               if (Age > 120)
                   WikidataBioErrors.NoDeath();
            }

        }

        private void ValidateDeceased()
        {
            Wikidate thisBirth = DateOfBirth.First();
            Wikidate thisDeath = DateOfDeath.First();

            if (Wikidate.isCalculable(thisBirth.thisPrecision) && Wikidate.isCalculable(thisDeath.thisPrecision))
            {
                TimeSpan Span = thisDeath.thisDate - thisBirth.thisDate;
                int Age = (int)Math.Ceiling((double)(Span.Days / 365.25));

                if (Age < 0)
                    WikidataBioErrors.BirthAfterDeath();
                else if (Age < 16)
                {
                    if (!ChildException()) WikidataBioErrors.DiedTooYoung();
                }
                if (Age > 120)
                    WikidataBioErrors.DiedTooOld();
            }

        }

        private bool ChildException()
        {
            if (Description.ToLower().IndexOf("child") == -1
                && Description.ToLower().IndexOf("royal") == -1
                && Description.ToLower().IndexOf("prince") == -1
                )
                return false;
            else
                return true;
        }

        private void ValidateP570(WikidataClaim Claim)
        {
            Wikidate thisDate= Claim.ValueAsDateTime;
            if (DateOfDeath.Count == 1) WikidataBioErrors.MultipleDeath();
            DateOfDeath.Add(thisDate);

            if (thisDate.thisDate > DateTime.Now) WikidataBioErrors.FutureDeath();

        }

        private void ValidateP569(WikidataClaim Claim)
        {
            Wikidate thisDate = Claim.ValueAsDateTime;
            if (DateOfBirth.Count == 1) WikidataBioErrors.MultipleBirth();
            DateOfBirth.Add(Claim.ValueAsDateTime);

            if (thisDate.thisDate > DateTime.Now) WikidataBioErrors.FutureBirth();
        }

        private void ValidateP31(WikidataClaim Claim)
        {
            if (!string.IsNullOrWhiteSpace(InstanceOf))
            {
                WikidataBioErrors.MultipleInstance();
                InstanceOf += " & ";
            }

            InstanceOf += Claim.ValueAsString;

            if (Claim.Qcode == 5)
            {
                isHuman = true;
                return;
            }

            // Brief list of valid alternatives to keep down error messages
            if (Claim.Qcode == 216866) return; // cojoined twins
            if (Claim.Qcode == 5883980) return; // holocaust victim
            if (Claim.Qcode == 159979) return; // twin
            if (Claim.Qcode == 2985549) return; // mononymous
            if (Claim.Qcode == 484188) return; // Serial killer

            WikidataBioErrors.UnrecognisedInstance(Claim.Qcode, Claim.ToString());
        }

        private void ValidateP27(WikidataClaim Claim)
        {
            if (!string.IsNullOrWhiteSpace(CitizenOf))
            {
                WikidataBioErrors.MultipleCitizenship();
                CitizenOf += " & ";
            }
            CitizenOf += Claim.ValueAsString;
        }

        private void ValidateP21(WikidataClaim Claim)
        {
            if (!string.IsNullOrWhiteSpace(Gender))
            {
                WikidataBioErrors.MultipleGender();
                Gender += " & ";
            }
            Gender += Claim.ValueAsString;

            if (Claim.Qcode == 6581072) return;  // male
            if (Claim.Qcode == 6581097) return;  // female
            if (Claim.Qcode == 2449503) return;  // transgender male
            if (Claim.Qcode == 1052281) return;  // transgender female

            WikidataBioErrors.UnrecognisedGender(Claim.Qcode, Claim.ToString());
        }



        public List<ErrorLog> GetErrors()
        {
            List<ErrorLog> Errors = IOErrors;
            Errors.Add(WikidataBioErrors);
            return Errors;
        }
    }
}
