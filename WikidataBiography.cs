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
        private readonly string[] CLAIMSREQUIRED = { "P31", "P27", "P21", "P569", "P570" };
        public int Qcode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Wikilink { get; set; }
        public List<Wikidate> DateOfBirth { get; set; }
        public List<Wikidate> DateOfDeath { get; set; }
        public string Gender { get; set; }
        public string CitizenOf { get; set; }
        public string InstanceOf { get; set; }
            
        public WikidataBiography(int qcode)
        {
            BError = new WikidataBiographyErrorLog();
            Qcode = qcode;

            WikidataIO WIO = new WikidataIO();
            WIO.Action = "wbgetentities";
            WIO.Format = "json";
            WIO.Sites = "";
            WIO.Ids = qcode;
            WIO.Props = "claims|descriptions|labels|sitelinks";
            WIO.Languages = "";
            WIO.ClaimsRequired = CLAIMSREQUIRED;

            ExtractFields(WIO.GetData());
        }



        private void ExtractFields(WikidataFields wikidataFields)
        {
            DateOfBirth = new List<Wikidate>();
            DateOfDeath = new List<Wikidate>();

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

            foreach (string thisClaim in CLAIMSREQUIRED)
            {
                IEnumerable<WikidataClaim> Claims = from val in wikidataFields.Claims where val.Key == Convert.ToInt32(thisClaim.Substring(1)) select val.Value;

                foreach (WikidataClaim Claim in Claims)
                {
                    switch (thisClaim)
                    {
                        case "P21":
                            if (!string.IsNullOrWhiteSpace(Gender)) Gender += " & ";
                            Gender += Claim.ValueAsString;
                            break;

                        case "P27":
                            if (!string.IsNullOrWhiteSpace(CitizenOf)) CitizenOf += " & ";
                            CitizenOf += Claim.ValueAsString;
                            break;

                        case "P31":
                            if (!string.IsNullOrWhiteSpace(InstanceOf)) InstanceOf += " & ";
                            InstanceOf += Claim.ValueAsString;
                            break;

                        case "P569":
                            DateOfBirth.Add(Claim.ValueAsDateTime);
                            break;

                        case "P570":
                            DateOfDeath.Add(Claim.ValueAsDateTime);
                            break;
                    }
                }
            }
        }
    }
}
