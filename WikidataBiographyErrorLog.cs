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
//            Errors.Add(new ErrorMessage(Module, 0, "WikidataBiography module"));
        }

        public void CannotRetrieveData()
        {
            Errors.Add(new ErrorMessage(Module,1,"Unable to rerieve data"));
        }
 
    }
}
