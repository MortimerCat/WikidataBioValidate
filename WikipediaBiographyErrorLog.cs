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
#if DBEUG
            Errors.Add(new ErrorMessage(Module, 0, "WikipediaBiography module"));
#endif
        }

        public void CannotRetrieveData()
        {
            Errors.Add(new ErrorMessage(Module,1,"Unable to retrieve wikipedia article"));
        }

        public void NoDefaultSort()
        {
            Errors.Add(new ErrorMessage(Module, 2, "No DEFAULTSORT in article"));
        }

        public void InvalidDefaultSort()
        {
            Errors.Add(new ErrorMessage(Module, 3, "Invalid DEFAULTSORT"));
        }
    }
}
