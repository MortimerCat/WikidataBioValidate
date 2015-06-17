using System;
using System.Collections.Generic;
using WikiAccess;

namespace WikidataBioValidation
{
    /// <summary>
    /// This class represents an article on Wikipedia
    /// </summary>
    class WikipediaBiography
    {
        private string Article;
        public List<string[]> Templates;
        public List<string> Categories;
        public string DefaultSort;
        private List<ErrorLog> IOErrors { get; set; }
        public WikipediaBiographyErrorLog WikipediaBioErrors { get; set; }
        private ErrorLog TemplateErrors { get; set; }
        private ErrorLog CategoryErrors { get; set; }
        public Wikidate BirthDate;
        public Wikidate DeathDate;

        public WikipediaBiography(string wikilink, string language = "enwiki")
        {
            Templates = new List<string[]>();
            Categories = new List<string>();
            BirthDate = new Wikidate();
            DeathDate = new Wikidate();

            WikipediaBioErrors = new WikipediaBiographyErrorLog();

            WikipediaIO WIO = new WikipediaIO();

            WIO.Action = "query";
            WIO.Export = "Yes";
            WIO.ExportNoWrap = "Yes";
            WIO.Format = "xml";
            WIO.Redirects = "yes";
            WIO.Titles = wikilink;

            bool Result = WIO.GetData();
            IOErrors = WIO.GetErrors();

            if (!Result) return;

            Article = WIO.Article;
            Templates = WIO.TemplatesUsed;
            Categories = WIO.CategoriesUsed;
            DefaultSort = GetDefaultSort();

            BirthDeathTemplate DateExtract = new BirthDeathTemplate(Templates);
            BirthDate = DateExtract.DOB;
            DeathDate = DateExtract.DOD;
            TemplateErrors = DateExtract.TemplateErrors;

            BirthDeathCategories CatAnalysis = new BirthDeathCategories("Wikipedia", Categories, BirthDate, DeathDate);
            CatAnalysis.AnalyseArticle();
            CatAnalysis.CompareDates();
            CategoryErrors = CatAnalysis.CategoryErrors;
        }

        private string GetDefaultSort()
        {
            int startpoint = Article.ToLower().IndexOf("{{defaultsort:", StringComparison.Ordinal);
            if (startpoint == -1)
            {
                WikipediaBioErrors.NoDefaultSort();
                return "";
            }
            else
            {
                int endpoint = Article.IndexOf("}}", startpoint, StringComparison.Ordinal);

                if (endpoint > startpoint)
                {
                    return Article.Substring(startpoint + 14, endpoint - startpoint - 14).Trim();
                }
                else
                {
                    WikipediaBioErrors.InvalidDefaultSort();
                    return "";
                }
            }
        }

        public List<ErrorLog> GetErrors()
        {
            List<ErrorLog> Errors = IOErrors;
            Errors.Add(WikipediaBioErrors);
            Errors.Add(TemplateErrors);
            Errors.Add(CategoryErrors);
            return Errors;
        }
    }
}
