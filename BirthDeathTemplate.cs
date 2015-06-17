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

        private List<string[]> Templates;
        public BirthDeathTemplateErrorLog TemplateErrors { get; set; }

        public BirthDeathTemplate(List<string[]> templates)
        {
            Templates = templates;
            TemplateErrors = new BirthDeathTemplateErrorLog();
            DOB = new Wikidate();
            DOD = new Wikidate();

            if (Templates.Count == 0) TemplateErrors.NoTemplates();
            ExtractDates();
        }


        private void ExtractDates()
        {
            bool FoundOne = false;
            foreach (string[] Template in Templates)
            {
                string TemplateName = Template[0];
                string FullTemplate = Template[1];

                if (IgnoreTemplate(TemplateName)) continue;

                if (TemplateName.IndexOf("birth") != -1 || TemplateName.IndexOf("death") != -1)
                    if (ExtractBirthDeathDates(TemplateName, FullTemplate)) FoundOne = true;
            }

            if (FoundOne == false) TemplateErrors.NoBirthDeathTemplate();
        }

        private bool ExtractBirthDeathDates(string TemplateName, string FullTemplate)
        {
            bool FoundOne = true;
            string[] TemplateParams = FullTemplate.Split("|".ToCharArray());
            int StartPos = 1;
            DateTime t_birth = DateTime.MinValue;
            DateTime t_death = DateTime.MinValue;
            DatePrecision t_birthAccuracy = DatePrecision.Day;
            DatePrecision t_deathAccuracy = DatePrecision.Day;
            
            try
            {
                if (TemplateParams[1].Substring(0, 2) == "mf" || TemplateParams[1].Substring(0, 2) == "df")
                    StartPos = 2;

                switch (TemplateName.Replace("_", " "))
                {
                    case "birth date and age":
                    case "birthdate and age":
                    case "birth date":
                        if (TemplateParams[StartPos + 1] == "0" || TemplateParams[StartPos + 2] == "0")
                        {
                            t_birth = new DateTime(Convert.ToInt32(TemplateParams[StartPos]), 1, 1);
                            t_birthAccuracy = DatePrecision.Year;
                        }
                        else
                            t_birth = new DateTime(Convert.ToInt32(TemplateParams[StartPos]), Convert.ToInt32(TemplateParams[StartPos + 1]), Convert.ToInt32(TemplateParams[StartPos + 2]));
                        break;

                    case "birth-date and age":
                    case "birth-date":
                        DateTime.TryParse(TemplateParams[StartPos], out t_birth);
                        break;

                    case "death-date and age":
                        DateTime.TryParse(TemplateParams[StartPos], out t_death);
                        DateTime.TryParse(TemplateParams[StartPos + 1], out t_birth);
                        break;

                    case "death-date":
                        DateTime.TryParse(TemplateParams[StartPos], out t_death);
                        break;

                    case "birth year and age":
                    case "birth year":
                        t_birth = new DateTime(Convert.ToInt32(TemplateParams[StartPos]), 1, 1);
                        t_birthAccuracy = DatePrecision.Year;
                        break;

                    case "death year and age":
                        t_death = new DateTime(Convert.ToInt32(TemplateParams[StartPos]), 1, 1);
                        t_deathAccuracy = DatePrecision.Year;
                        t_birth = new DateTime(Convert.ToInt32(TemplateParams[StartPos + 1]), 1, 1);
                        t_birthAccuracy = DatePrecision.Year;
                        break;

                    case "death year":
                        t_death = new DateTime(Convert.ToInt32(TemplateParams[StartPos]), 1, 1);
                        t_deathAccuracy = DatePrecision.Year;
                        break;

                    case "death date":
                    case "date of death":
                        t_death = new DateTime(Convert.ToInt32(TemplateParams[StartPos]), Convert.ToInt32(TemplateParams[StartPos + 1]), Convert.ToInt32(TemplateParams[StartPos + 2]));
                        break;

                    case "death date and age":
                        if (TemplateParams[StartPos + 1] == "0" || TemplateParams[StartPos + 2] == "0")
                        {
                            t_death = new DateTime(Convert.ToInt32(TemplateParams[StartPos]), 1, 1);
                            t_deathAccuracy = DatePrecision.Year;
                        }
                        else
                            t_death = new DateTime(Convert.ToInt32(TemplateParams[StartPos]), Convert.ToInt32(TemplateParams[StartPos + 1]), Convert.ToInt32(TemplateParams[StartPos + 2]));


                        if (TemplateParams[StartPos + 4] == "0" || TemplateParams[StartPos + 5] == "0")
                        {
                            t_birth = new DateTime(Convert.ToInt32(TemplateParams[StartPos + 3]), 1, 1);
                            t_birthAccuracy = DatePrecision.Year;
                        }
                        else
                            t_birth = new DateTime(Convert.ToInt32(TemplateParams[StartPos + 3]), Convert.ToInt32(TemplateParams[StartPos + 4]), Convert.ToInt32(TemplateParams[StartPos + 5]));
                        break;


                    default:
                        FoundOne = false;
                        TemplateErrors.UnrecognisedTemplate(TemplateName);
                        break;
                }
            }
            catch (Exception exception)
            {
                TemplateErrors.ExceptionThrown(TemplateName, exception.Message);
                return FoundOne;
            }

            if (DOB.thisDate != DateTime.MinValue && t_birth != null)
            {
                if (DOB.thisDate != t_birth)
                    TemplateErrors.SecondBirth(TemplateName);
            }
            else
            {
                DOB.thisDate = t_birth;
                DOB.thisPrecision = t_birthAccuracy;
            }

            if (DOD.thisDate != DateTime.MinValue && t_death != null)
            {
                if (DOD.thisDate != t_death)
                    TemplateErrors.SecondDeath(TemplateName);
            }
            else
            {
                DOD.thisDate = t_death;
                DOD.thisPrecision = t_deathAccuracy;
            }
            return FoundOne;
        }

        private bool IgnoreTemplate(string template)
        {
            string ThisTemplate = template.Replace("_", " ").ToLower();
            if (ThisTemplate == "recent death") return true;
            if (ThisTemplate == "the birthday party") return true;
            return false;
        }
    }
}
