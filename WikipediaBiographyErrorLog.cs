using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiAccess;

namespace WikidataBioValidation 
{
    class WikipediaBiographyErrorLog : ErrorLog
    {
        public string Module {get {return "G";}}
        public List<ErrorMessage> Errors { get; set; }

        public WikipediaBiographyErrorLog()
        {
            Errors = new List<ErrorMessage>();
//            Errors.Add(new ErrorMessage(Module, 0, "WikipediaBiography module"));
        }

        public void CannotRetrieveData()
        {
            Errors.Add(new ErrorMessage(Module,1,"Unable to retrieve data"));
        }
    }
}
