using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace WikidataBioValidation
{
    /// <summary>
    /// Class to extract data from Wikidata
    /// </summary>
    class WikidataExtract
    {
        public WikidataFields Fields { get; set; }
        private string Content { get; set; }
        private string[] ClaimsRequired = new string[5] { "P31", "P27", "P21", "P569", "P570" };
        private WikidataCache Cache = new WikidataCache();

        public WikidataExtract(string content)
        {
            Fields = new WikidataFields();
            Content = content;
            ExtractJSON();
        }

        private bool ExtractJSON()
        {
            //Interpret the JSON - Basically read in a level at a time.
            var DataFromWiki = JObject.Parse(Content);
            var Entities = (JObject)DataFromWiki["entities"];

            var Entity = Entities.Properties().First();   // Name is variable, so grab data by using first method
            string EntityKey = Entity.Name;

            var EntityData = (JObject)Entity.Value;

            if (EntityKey == "-1")
            {
                // QCode does not exist
                return false;
            }

            Fields.ID = (string)EntityData["id"];
            string EntityType = (string)EntityData["type"];

            if (EntityType == null)
            {
                return false;
            }

            var Descriptions = (JObject)EntityData["descriptions"];
            var Labels = (JObject)EntityData["labels"];
            var SiteLinks = (JObject)EntityData["sitelinks"];

            if (Labels != null)
            {
                var LabelObject = (JObject)Labels["en"];
                if (LabelObject != null)
                    Fields.Name = (string)LabelObject["value"]; ;

                LabelObject = (JObject)Labels["en-gb"];
                if (LabelObject != null)
                    Fields.Name = (string)LabelObject["value"];
            }
            else
                Fields.Name = "";

            if (Descriptions != null)
            {
                var DescriptionObject = (JObject)Descriptions["en"];
                if (DescriptionObject != null)
                    Fields.Description = (string)DescriptionObject["value"]; ;

                DescriptionObject = (JObject)Descriptions["en-gb"];
                if (DescriptionObject != null)
                    Fields.Description = (string)DescriptionObject["value"];
            }
            else
                Fields.Description = "";

            if (SiteLinks != null)
            {
                var SiteLink = (JObject)SiteLinks["enwiki"];
                if (SiteLink != null)
                    Fields.WikipediaLink = (string)SiteLink["title"];
            }
            else
                Fields.WikipediaLink = "";

            var Claims = (JObject)EntityData["claims"];
            if (Claims != null)
            {
                //Now we get to loop through each claim property for that article
                foreach (var Claim in Claims.Properties())
                {
                    string ClaimKey = Claim.Name;

                    if (Array.IndexOf(ClaimsRequired, ClaimKey) == -1) continue;


                    var ClaimData = (JArray)Claim.Value;

                    for (int ThisClaim = 0; ThisClaim < ClaimData.Count(); ThisClaim++)
                    {

                        //claimData is an array - another loop

                        string thisValueString = "";
                        var MainSnak = (JObject)ClaimData[ThisClaim]["mainsnak"];
                        string SnakType = (string)MainSnak["snaktype"];
                        string SnakDataType = (string)MainSnak["datatype"];
                        var SnakDataValue = (JObject)MainSnak["datavalue"];

                        Wikidate thisValueDateTime = new Wikidate();

                        if (SnakType == "novalue" || SnakType == "somevalue")
                        {
                            thisValueString = SnakType;
                        }
                        else
                        {
                            if (SnakDataType == "string" || SnakDataType == "commonsMedia" || SnakDataType == "url")
                            {
                                thisValueString = (string)SnakDataValue["value"];
                            }
                            else if (SnakDataType == "wikibase-item")
                            {
                                var ObjectValue = (JObject)SnakDataValue["value"];
                                thisValueString = Cache.Lookup((string)ObjectValue["numeric-id"]);
                            }
                            else if (SnakDataType == "time")
                            {
                                var ObjectValue = (JObject)SnakDataValue["value"];

                                string ValueTime = (string)ObjectValue["time"];


                                string ValueTimePrecision = (string)ObjectValue["precision"];
                                string ValueTimeCalendarModel = (string)ObjectValue["calendarmodel"];

                                bool Julian = false;
                                bool Gregorian = false;

                                if (ValueTimeCalendarModel != "http://www.wikidata.org/entity/Q1985727")
                                    Gregorian = true;
                                if (ValueTimeCalendarModel == "http://www.wikidata.org/entity/Q1985786")
                                    Julian = true;

                                if (ValueTimePrecision == "11" || ValueTimePrecision == "10" || ValueTimePrecision == "9"
                                                               || ValueTimePrecision == "8" || ValueTimePrecision == "7" || ValueTimePrecision == "6")
                                {
                                    int DateStart = ValueTime.IndexOf("-", 2) - 4;

                                    string ThisDateString = (ValueTime.Substring(DateStart, 10));
                                    ThisDateString = ThisDateString.Replace("-00", "-01");  // Occasionally get 1901-00-00 ?

                                    bool ValidDate = true;
                                    DateTime thisDate;
                                    try
                                    {
                                        thisDate = DateTime.Parse(ThisDateString, null, DateTimeStyles.RoundtripKind);
                                    }
                                    catch
                                    {
                                        thisDate = DateTime.MinValue;
                                        ValidDate = false;
                                    }
                                    if (Julian == true && ValueTimePrecision == "11")
                                    {
                                        // All dates will be Gregorian
                                        // Julian flag tells us to display Julian date.
                                        // JulianCalendar JulCal = new JulianCalendar();
                                        // DateTime dta = JulCal.ToDateTime(thisDate.Year, thisDate.Month, thisDate.Day, 0, 0, 0, 0);
                                        // thisDate = dta;
                                    }

                                    DatePrecision Precision = DatePrecision.Null;


                                    if (ValidDate == false)
                                    {
                                        Precision = DatePrecision.Invalid;
                                    }
                                    else if (ValueTime.Substring(0, 1) == "+")
                                    {
                                        switch (ValueTimePrecision)
                                        {
                                            case "11":
                                                Precision = DatePrecision.Day;
                                                break;
                                            case "10":
                                                Precision = DatePrecision.Month;
                                                break;
                                            case "9":
                                                Precision = DatePrecision.Year;
                                                break;
                                            case "8":
                                                Precision = DatePrecision.Decade;
                                                break;
                                            case "7":
                                                Precision = DatePrecision.Century;
                                                break;
                                            case "6":
                                                Precision = DatePrecision.Millenium;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        Precision = DatePrecision.BCE;
                                    }
                                    thisValueDateTime.thisPrecision = Precision;
                                    thisValueDateTime.thisDate = thisDate;
                                }
                            }
                            else if (SnakDataType == "monolingualtext")
                            {
                                var ObjectValue = (JObject)SnakDataValue["value"];
                                string ValueText = (string)ObjectValue["text"];
                                string ValueLanguage = (string)ObjectValue["language"];
                                thisValueString = ValueText + "(" + ValueLanguage + ")";
                            }
                            else if (SnakDataType == "quantity")
                            {
                                var ObjectValue = (JObject)SnakDataValue["value"];
                                string ValueAmount = (string)ObjectValue["amount"];
                                string ValueUnit = (string)ObjectValue["unit"];
                                string ValueUpper = (string)ObjectValue["upperBound"];
                                string ValueLower = (string)ObjectValue["lowerBound"];

                                thisValueString = "(" + ValueLower + " to " + ValueUpper + ") Unit " + ValueUnit;
                            }

                        }
                        switch (ClaimKey)
                        {
                            case "P31":
                                Fields.InstanceOf = thisValueString;
                                break;
                            case "P21":
                                Fields.Gender = thisValueString;
                                break;
                            case "P27":
                                Fields.CitizenOf = thisValueString;
                                break;
                            case "P569":
                                Fields.DateOfBirth = thisValueDateTime;
                                break;
                            case "P570":
                                Fields.DateOfDeath = thisValueDateTime;
                                break;
                        }

                    }

                }
            }


            return true;
        }

    }
}
