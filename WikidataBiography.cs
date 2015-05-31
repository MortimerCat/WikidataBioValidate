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
        public int Qcode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Wikilink { get; set; }
        public Wikidate DateOfBirth { get; set; }
        public Wikidate DateOfDeath { get; set; }
        public string Gender { get; set; }
        public string CitizenOf { get; set; }
        public string InstanceOf { get; set; }

        public WikidataBiography(int qcode)
        {
            Qcode = qcode;

            WikidataIO WIO = new WikidataIO();
            WIO.Action = "wbgetentities";
            WIO.Format = "json";
            WIO.Sites = "";
            WIO.Ids = qcode;
            WIO.Props = "claims|descriptions|labels|sitelinks";
            WIO.Languages = "";
            WIO.ClaimsRequired = new string[5] { "P31", "P27", "P21", "P569", "P570" };

            ExtractFields(WIO.GetData());

        }

        private void ExtractFields(WikidataFields wikidataFields)
        {
            DateOfBirth = new Wikidate();
            DateOfDeath = new Wikidate();

            string ThisName;
            if (!wikidataFields.Labels.TryGetValue("en-gb", out ThisName))
                wikidataFields.Labels.TryGetValue("en", out ThisName);
            Name = ThisName;

            string ThisDescription;
            if (!wikidataFields.Description.TryGetValue("en-gb", out ThisDescription))
                wikidataFields.Description.TryGetValue("en", out ThisDescription);
            Description = ThisDescription;

            string ThisWikilink;
            wikidataFields.WikipediaLinks.TryGetValue("enwiki", out ThisWikilink);
            Wikilink = ThisWikilink;

            IEnumerable<WikidataClaim> Claims = from val in wikidataFields.Claims where val.Key == 27 select val.Value;
            CitizenOf = "";
            foreach (WikidataClaim Claim in Claims)
            {
                if (CitizenOf != "") CitizenOf += " & ";
                CitizenOf += Claim.ValueAsString; 
            }


            /*
            wikidataFields.Claims.TryGetValue(31, out Claim);
            if (Claim != null) InstanceOf = Claim.ValueAsString;
            wikidataFields.Claims.TryGetValue(21, out Claim);
            if (Claim != null) Gender = Claim.ValueAsString;

            wikidataFields.Claims.TryGetValue(569, out Claim);
            if (Claim != null) DateOfBirth = Claim.ValueAsDateTime;
            wikidataFields.Claims.TryGetValue(570, out Claim);
            if (Claim != null) DateOfDeath = Claim.ValueAsDateTime;
            */
        }

        public IEnumerable<string> ErrorMessage { get; set; }
    }
}
