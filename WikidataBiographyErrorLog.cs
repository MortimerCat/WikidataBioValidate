using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiAccess;

namespace WikidataBioValidation
{
    class WikidataBiographyErrorLog : ErrorLog
    {
        public string Module {get {return "B";}}
        public List<ErrorMessage> Errors { get; set; }

        public WikidataBiographyErrorLog()
        {
            Errors = new List<ErrorMessage>();
        }
 
    }
}
