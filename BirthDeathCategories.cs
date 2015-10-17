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
            CategoryErrors = new BirthDeathCategoriesErrorLog(WhereDates);
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
            CheckBirthCategory();
            CheckDeathCategory();
            CheckAges();
        }

        private void CheckAges()
        {
            int Age = CalculateAge();

            if (Age == 0) CategoryErrors.AgeOfZero();
            if (Age >0) CheckCentenarian(Age);
            if (Age >0 && Age < 14) CheckChild(Age);
        }

        private int CalculateAge()
        {
            if (!Wikidate.isCalculable(DOB.thisPrecision)) return -1;
            DateTime EndTime;
            if (DOD.thisPrecision == DatePrecision.Null)
                EndTime = DateTime.Now;
            else
            {
                if (!Wikidate.isCalculable(DOD.thisPrecision)) return -1;
                EndTime = DOD.thisDate;
            }

            TimeSpan AgeInDays = DOD.thisDate - DOB.thisDate;
            int AgeInYears = (int)Math.Ceiling((double)(AgeInDays.Days / 365.25));
            return AgeInYears;
        }

        private void CheckCentenarian(int age)
        {
            //throw new NotImplementedException();
        }

        private void CheckChild(int age)
        {
            //throw new NotImplementedException();
        }

        private void CheckBirthCategory()
        {
            string BirthCategory = "";

            if (DOB.thisPrecision == DatePrecision.Day || DOB.thisPrecision == DatePrecision.Month || DOB.thisPrecision == DatePrecision.Year)
            {
                BirthCategory = DOB.Year.ToString() + " births";
                if (DOB.thisPrecision == DatePrecision.Day && BirthDateNotAccurateCat == true)
                    CategoryErrors.BirthDateKnownDateMissingCategory(WhereDates);
                else if (DOB.thisPrecision != DatePrecision.Day && BirthDateNotAccurateCat == false)
                    CategoryErrors.BirthDateNotKnownNotDateMissingCategory(WhereDates);
            }
            else if (DOB.thisPrecision == DatePrecision.Decade)
            {
                BirthCategory = DOB.Year.ToString() + "s births";
            }
            else
            {
                if (BirthYearCat == true || BirthDecadeCat == true)
                    CategoryErrors.NoBirthDateCategoryExists(WhereDates);
                else

                    if (BirthYearNotKnownCat == false)
                        CategoryErrors.BirthDateNotKnownNoExplanation(WhereDates);
                return;
            }

            if (Categories.IndexOf(BirthCategory) == -1)
            {
                if (BirthYearCat == true || BirthDecadeCat == true)
                    CategoryErrors.BirthDateNotMatchBirthCat(WhereDates);
                else
                    CategoryErrors.BirthDateKnownNoCategory(WhereDates);
            }

        }

        private void CheckDeathCategory()
        {
            string DeathCategory = "";

            if (DOD.thisPrecision == DatePrecision.Day || DOD.thisPrecision == DatePrecision.Month || DOD.thisPrecision == DatePrecision.Year)
            {
                DeathCategory = DOD.Year.ToString() + " deaths";
                if (DOD.thisPrecision == DatePrecision.Day && DeathDateNotAccurateCat == true)
                    CategoryErrors.DeathDateKnownDateMissingCategory(WhereDates);
                else if (DOD.thisPrecision != DatePrecision.Day && DeathDateNotAccurateCat == false)
                    CategoryErrors.DeathDateNotKnownNotDateMissingCategory(WhereDates);
            }
            else if (DOD.thisPrecision == DatePrecision.Decade)
            {
                DeathCategory = DOD.Year.ToString() + "s deaths";
            }
            else
            {
                if (DeathYearCat == true || DeathDecadeCat == true)
                    CategoryErrors.NoDeathDateCategoryExists(WhereDates);
                else
                    if (LivingPeopleCat == false)
                    {
                        if (DeathYearNotKnownCat == false)
                            CategoryErrors.DeathDateNotKnownNoExplanation(WhereDates);
                    }
                return;
            }

            if (Categories.IndexOf(DeathCategory) == -1)
            {
                if (DeathYearCat == true || DeathDecadeCat == true)
                    CategoryErrors.DeathDateNotMatchDeathCat(WhereDates);
                else
                    CategoryErrors.DeathDateKnownNoCategory(WhereDates);
            }
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
