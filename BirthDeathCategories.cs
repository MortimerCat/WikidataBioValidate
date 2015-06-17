using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WikiAccess;

namespace WikidataBioValidation
{
    class BirthDeathCategories
    {
        public BirthDeathCategoriesErrorLog CategoryErrors { get; set; }

        private string WhereDates;
        private List<string> Categories;
        private Wikidate DOB;
        private Wikidate DOD;
        private bool BirthYearCat;
        private bool BirthDecadeCat;
        private bool BirthYearNotKnownCat;
        private bool BirthDateNotAccurateCat;
        private bool DeathYearNotKnownCat;
        private bool DeathDateNotAccurateCat;
        private bool DeathYearCat;
        private bool DeathDecadeCat;
        private bool LivingPeopleCat;
        private bool BirthLivingPeopleCat;

        public BirthDeathCategories(string wheredates, List<string> categories, Wikidate dob, Wikidate dod)
        {
            WhereDates = wheredates;
            Categories = categories;
            DOB = dob;
            DOD = dod;
            CategoryErrors = new BirthDeathCategoriesErrorLog();
        }

        public void AnalyseArticle()
        {
            CategoryScan();
            if (!BirthYearCat && !BirthDecadeCat && !BirthYearNotKnownCat && !BirthDateNotAccurateCat) 
                CategoryErrors.NoBirthCategories();
            if (LivingPeopleCat && (DeathYearCat || DeathDecadeCat || DeathYearNotKnownCat || DeathDateNotAccurateCat)) 
                CategoryErrors.LivingandDeathCat();
            if (!LivingPeopleCat && !DeathYearCat && !DeathDecadeCat && !DeathYearNotKnownCat && !DeathDateNotAccurateCat) 
                CategoryErrors.NoLivingorDeathCat();
            if (BirthYearCat && BirthYearNotKnownCat)
                CategoryErrors.BirthYearButNotKnown();
            if (DeathYearCat && DeathYearNotKnownCat)
                CategoryErrors.DeathYearButNotKnown();
            if (LivingPeopleCat && (BirthYearNotKnownCat || BirthDateNotAccurateCat) && !BirthLivingPeopleCat)
                CategoryErrors.LivingNotWithLivingVariation();
            if (!LivingPeopleCat && BirthLivingPeopleCat)
                CategoryErrors.DeadWithLivingVariation();
        }

        public void CompareDates()
        {
            return;
        }


        private void CategoryScan()
        {
            foreach (string Category in Categories)
            {
                if (BirthYear(Category)) BirthYearCat = true;
                if (BirthDecade(Category)) BirthDecadeCat = true;
                if (BirthYearNotKnown(Category)) BirthYearNotKnownCat = true;
                if (BirthDateNotAccurate(Category)) BirthDateNotAccurateCat = true;
                if (DeathYearNotKnown(Category)) DeathYearNotKnownCat = true;
                if (DeathDateNotAccurate(Category)) DeathDateNotAccurateCat = true;
                if (DeathYear(Category)) DeathYearCat = true;
                if (DeathDecade(Category)) DeathDecadeCat = true;
                if (LivingPeople(Category)) LivingPeopleCat = true;
                if (BirthLivingPeople(Category)) BirthLivingPeopleCat = true;
            }
        }

        private bool BirthYear(string Category)
        {
            if (Regex.IsMatch(Category, "[0-9]{1,4} births")) return true;
            return false;
        }

        private bool BirthDecade(string Category)
        {
            if (Regex.IsMatch(Category, "[0-9]{1,3}0s births")) return true;
            return false;
        }

        private bool BirthYearNotKnown(string Category)
        {
            if (Category.ToLower() == "year of birth missing") return true;
            if (Category.ToLower() == "year of birth missing (living people)") return true;
            if (Category.ToLower() == "year of birth uncertain") return true;
            if (Category.ToLower() == "year of birth unknown") return true;
            return false;
        }

        private bool BirthDateNotAccurate(string Category)
        {
            if (Category.ToLower() == "date of birth missing") return true;
            if (Category.ToLower() == "date of birth missing (living people)") return true;
            if (Category.ToLower() == "date of birth uncertain") return true;
            if (Category.ToLower() == "date of birth unknown") return true;
            return false;
        }

        private bool BirthLivingPeople(string Category)
        {
            if (Category.ToLower() == "year of birth missing (living people)") return true;
            if (Category.ToLower() == "date of birth missing (living people)") return true;
            return false;
        }

        private bool DeathYearNotKnown(string Category)
        {
            if (Category.ToLower() == "year of death missing") return true;
            if (Category.ToLower() == "year of death uncertain") return true;
            if (Category.ToLower() == "year of death unknown") return true;
            return false;
        }

        private bool DeathDateNotAccurate(string Category)
        {
            if (Category.ToLower() == "date of death missing") return true;
            if (Category.ToLower() == "date of death uncertain") return true;
            if (Category.ToLower() == "date of death unknown") return true;
            return false;
        }

        private bool DeathYear(string Category)
        {
            if (Regex.IsMatch(Category, "[0-9]{1,4} deaths")) return true;
            return false;
        }

        private bool DeathDecade(string Category)
        {
            if (Regex.IsMatch(Category, "[0-9]{1,3}0s deaths")) return true;
            return false;
        }

        private bool LivingPeople(string Category)
        {
            if (Category.ToLower() == "living people") return true;
            if (Category.ToLower() == "possibly living people") return true;
            return false;
        }

    }
}
