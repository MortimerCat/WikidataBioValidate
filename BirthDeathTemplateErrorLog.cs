using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiAccess;

namespace WikidataBioValidation
{
    class BirthDeathTemplateErrorLog : ErrorLog
    {
        public string Module {get {return "T";}}
        public List<ErrorMessage> Errors { get; set; }

        public BirthDeathTemplateErrorLog()
        {
            Errors = new List<ErrorMessage>();
#if DEBUG
            Errors.Add(new ErrorMessage(Module, 0, "BirthDeathTemplate module"));
#endif
        }

        public void NoTemplates()
        {
            Errors.Add(new ErrorMessage(Module, 1, "No templates"));
        }

    }
}
