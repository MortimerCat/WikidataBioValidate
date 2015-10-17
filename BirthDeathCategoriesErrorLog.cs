using System.Collections.Generic;
using WikiAccess;

namespace WikidataBioValidation
{
    class BirthDeathCategoriesErrorLog : ErrorLog
    {
        public string Module
        {
            get
            {
                if (WhereData.ToLower() == "wikipedia")
                {
                    return "C";
                }
                else
                {
                    return "Y";
                }
            }
        }
        public List<ErrorMessage> Errors { get; set; }
        private string WhereData;

        public BirthDeathCategoriesErrorLog(string wheredata)
        {
            WhereData = wheredata;
            Errors = new List<ErrorMessage>();
#if DEBUG
            Errors.Add(new ErrorMessage(Module, 0, "BirthDeathCategories module"));
#endif
        }

        public void NoCategories()
        {
            Errors.Add(new ErrorMessage(Module, 1, "No categories"));
        }

        public void NoBirthCategories()
        {
            Errors.Add(new ErrorMessage(Module, 2, "No birth categories in article"));
        }

        public void LivingandDeathCat()
        {
            Errors.Add(new ErrorMessage(Module, 3, "Living people and death categories found"));
        }

        public void NoLivingorDeathCat()
        {
            Errors.Add(new ErrorMessage(Module, 4, "No living people or death category"));
        }

        public void BirthYearButNotKnown()
        {
            Errors.Add(new ErrorMessage(Module, 5, "Birth year category but year not known"));
        }

        public void DeathYearButNotKnown()
        {
            Errors.Add(new ErrorMessage(Module, 6, "Death year category but year not known"));
        }

        public void LivingNotWithLivingVariation()
        {
            Errors.Add(new ErrorMessage(Module, 7, "Living person using dead missing template"));
        }

        public void DeadWithLivingVariation()
        {
            Errors.Add(new ErrorMessage(Module, 8, "No Living people template using live missing template"));
        }

        public void BirthDateNotMatchBirthCat(string whichdates)
        {
            Errors.Add(new ErrorMessage(Module, 9, "Year of birth on " + whichdates + " does not match birth year category"));
        }

        public void BirthDateKnownNoCategory(string whichdates)
        {
            Errors.Add(new ErrorMessage(Module, 10, "Year of birth is known on " + whichdates + " but there is no birth year category"));
        }

        public void NoBirthDateCategoryExists(string whichdates)
        {
            Errors.Add(new ErrorMessage(Module, 11, "Year of birth not known on " + whichdates + " but there is a birth year category"));
        }

        public void BirthDateNotKnownNoExplanation(string whichdates)
        {
            Errors.Add(new ErrorMessage(Module, 12, "Year of birth not known on " + whichdates + " but no missing date templates"));
        }

        public void BirthDateKnownDateMissingCategory(string whichdates)
        {
            Errors.Add(new ErrorMessage(Module, 13, "Year of birth is known on " + whichdates + " but in date missing category"));
        }

        public void BirthDateNotKnownNotDateMissingCategory(string whichdates)
        {
            Errors.Add(new ErrorMessage(Module, 14, "Year of birth is not known on " + whichdates + " and not in date missing category"));
        }

        public void DeathDateNotMatchDeathCat(string whichdates)
        {
            Errors.Add(new ErrorMessage(Module, 15, "Year of death on " + whichdates + " does not match death year category"));
        }

        public void DeathDateKnownNoCategory(string whichdates)
        {
            Errors.Add(new ErrorMessage(Module, 16, "Year of death is known on " + whichdates + " but there is no death year category"));
        }

        public void NoDeathDateCategoryExists(string whichdates)
        {
            Errors.Add(new ErrorMessage(Module, 17, "Year of death not known on " + whichdates + " but there is a death year category"));
        }

        public void DeathDateNotKnownNoExplanation(string whichdates)
        {
            Errors.Add(new ErrorMessage(Module, 18, "Year of death not known on " + whichdates + " but no missing date templates"));
        }

        public void DeathDateKnownDateMissingCategory(string whichdates)
        {
            Errors.Add(new ErrorMessage(Module, 19, "Year of death is known on " + whichdates + " but in date missing category"));
        }

        public void DeathDateNotKnownNotDateMissingCategory(string whichdates)
        {
            Errors.Add(new ErrorMessage(Module, 20, "Year of death is not known on " + whichdates + " and not in date missing category"));
        }

        public void Over100NotCentenarianCategory(string whichdates,int age)
        {
            Errors.Add(new ErrorMessage(Module, 21, "Age of " + age.ToString() + " on " + whichdates + " not in Centenarian category"));
        }

        public void Over110NotCentenarianCategory(string whichdates, int age)
        {
            Errors.Add(new ErrorMessage(Module, 22, "Age of " + age.ToString() + " on " + whichdates + " not in Super-centenarian category"));
        }

        public void Under100CentenarianCategory(string whichdates, int age)
        {
            Errors.Add(new ErrorMessage(Module, 22, "Age of " + age.ToString() + " on " + whichdates + " and in Centenarian category"));
        }

        public void Under110CentenarianCategory(string whichdates, int age)
        {
            Errors.Add(new ErrorMessage(Module, 23, "Age of " + age.ToString() + " on " + whichdates + " and in Super-centenarian category"));
        }

        public void AgeOfZero()
        {
            Errors.Add(new ErrorMessage(Module,24,"Age calculates to zero"));
        }
    }
}
