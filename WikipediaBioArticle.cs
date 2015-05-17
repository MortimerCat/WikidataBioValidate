using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikidataBioValidation
{
    /// <summary>
    /// This class represents an article on Wikipedia
    /// </summary>
    class WikipediaBioArticle
    {
        public WikipediaBioArticle(string wikilink, string language ="en")
        {
        }

        public IEnumerable<string> ErrorMessage { get; set; }
    }
}
