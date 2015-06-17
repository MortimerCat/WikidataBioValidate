using System.Collections.Generic;
using WikiAccess;

namespace WikidataBioValidation
{
    class BirthDeathCategoriesErrorLog : ErrorLog
    {
        public string Module { get { return "C"; } }
        public List<ErrorMessage> Errors { get; set; }

        public BirthDeathCategoriesErrorLog()
        {
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
            Errors.Add(new ErrorMessage(Module,2,"No birth categories in article"));
        }

        public void LivingandDeathCat()
        {
            Errors.Add(new ErrorMessage(Module,3,"Living people and death categories found"));
        }

        public void NoLivingorDeathCat()
        {
            Errors.Add(new ErrorMessage(Module,4,"No living people or death category"));
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


    }
}

