using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikidataBioValidation
{
    public enum DatePrecision { Null, Day, Month, Year, Decade, Century, Unknown, NotEntered, NoProperty, BCE, Invalid, Millenium };

    class Wikidate
    {
        public DateTime thisDate { get; set; }
        public DatePrecision thisPrecision { get; set; }

        string toString()
        {
            string FormattedDate = "Invalid";

            switch (thisPrecision)
            {
                case DatePrecision.Null:
                case DatePrecision.Day:
                    FormattedDate = thisDate.ToString("d MMMM yyyy");
                    break;
                case DatePrecision.Month:
                    FormattedDate = thisDate.ToString("MMMM yyyy");
                    break;
                case DatePrecision.Year:
                    FormattedDate = thisDate.ToString("yyyy");
                    break;
                case DatePrecision.Decade:
                    FormattedDate = thisDate.ToString("yyyy").Substring(0, 3) + "0s";
                    break;
                case DatePrecision.Century:
                    int Century = Convert.ToInt32(thisDate.ToString("yyyy").Substring(0, 2));
                    FormattedDate = (Century + 1).ToString() + "th century";
                    break;
                case DatePrecision.Millenium:
                    int Millenium = Convert.ToInt32(thisDate.ToString("yyyy").Substring(0, 1));
                    FormattedDate = (Millenium + 1).ToString() + " millenium";
                    break;
                case DatePrecision.Unknown:
                    FormattedDate = "Unknown";
                    break;
                case DatePrecision.NotEntered:
                case DatePrecision.NoProperty:
                    FormattedDate = "No value";
                    break;
            }
            return FormattedDate;
        }
    }
}
