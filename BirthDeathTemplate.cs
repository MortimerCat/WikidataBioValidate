using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiAccess;

namespace WikidataBioValidation
{
    class BirthDeathTemplate
    {
        public Wikidate DOB { get; set; }
        public Wikidate DOD { get; set; }

        private List<string> Templates;
        public BirthDeathTemplateErrorLog TemplateErrors { get; set; }

        public BirthDeathTemplate(List<string> templates)
        {
            Templates = templates;
            TemplateErrors = new BirthDeathTemplateErrorLog();
            DOB = new Wikidate();
            DOD = new Wikidate();

            if (Templates.Count == 0) TemplateErrors.NoTemplates();
        }
    }
}
