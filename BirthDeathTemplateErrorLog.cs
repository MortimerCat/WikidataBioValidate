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

        public void UnrecognisedTemplate(string template)
        {
            Errors.Add(new ErrorMessage(Module, 2, "Unrecognised template " + template));
        }

        public void ExceptionThrown(string template,string systemMessage)
        {
            Errors.Add(new ErrorMessage(Module, 3, "Exception thrown extracting date " + template, systemMessage));
        }

        public void SecondBirth(string template)
        {
            Errors.Add(new ErrorMessage(Module, 4, "Alternative date of birth found in " + template));
        }

        public void SecondDeath(string template)
        {
            Errors.Add(new ErrorMessage(Module, 5, "Alternative date of death found in " + template));
        }

        public void NoBirthDeathTemplate()
        {
            Errors.Add(new ErrorMessage(Module,6,"No Birth/Death template in article"));
        }
    }
}
