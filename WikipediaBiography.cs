using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiAccess;

namespace WikidataBioValidation
{
    /// <summary>
    /// This class represents an article on Wikipedia
    /// </summary>
    class WikipediaBiography
    {
        private string Article;
        public List<string> Templates;
        public List<string> Categories;
        private List<ErrorLog> IOErrors { get; set; }
        public WikipediaBiographyErrorLog GErrors { get; set; }

        public WikipediaBiography(string wikilink, string language = "enwiki")
        {
            Templates = new List<string>();
            Categories = new List<string>();
            GErrors = new WikipediaBiographyErrorLog();

            WikipediaIO WIO = new WikipediaIO();

            WIO.Action = "query";
            WIO.Export = "Yes";
            WIO.ExportNoWrap = "Yes";
            WIO.Format = "xml";
            WIO.Redirects = "yes";
            WIO.Titles = wikilink;

            if (WIO.GetData())
            {
                Article = WIO.Article;
                Templates = WIO.Templates;
                Categories = WIO.Categories;
                IOErrors = WIO.GetErrors();
            }
        }

        private string GetDefaultSort()
        {
            int startpoint = Article.ToLower().IndexOf("{{defaultsort:", StringComparison.Ordinal);
            if (startpoint == -1)
            {
                // Error noDEFAULTSORT
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
                    // Error invalidDEFAULTSORT
                    return "";
                }
            }
        }

        public List<ErrorLog> GetErrors()
        {
            List<ErrorLog> Errors = IOErrors;
         
                Errors.Add(GErrors);
            return Errors;
        }
    }
}
