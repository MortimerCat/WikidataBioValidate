using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikidataBioValidation
{
    /// <summary>
    /// This class represents an article on Wikipedia
    /// </summary>
    class WikipediaBiography
    {
        private string Article;
        List<string> Templates = new List<string>();
        List<string> Categories = new List<string>();

        public WikipediaBiography(string wikilink, string language ="enwiki")
        {
            WikipediaIO WIO = new WikipediaIO();

            WIO.Action = "query";
            WIO.Export = "Yes";
            WIO.ExportNoWrap = "Yes";
            WIO.Format = "xml";
            WIO.Redirects = "yes";
            WIO.Titles = wikilink;

            Article = WIO.GetData();
            Templates = WIO.ExtractTemplates();
            Categories = WIO.ExtractCategories();
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

        public IEnumerable<string> ErrorMessage { get; set; }
    }
}
