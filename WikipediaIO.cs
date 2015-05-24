using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.IO;

namespace WikidataBioValidation
{
    class WikipediaIO : WikimediaApi
    {
        protected override string APIurl { get { return @"http://en.wikipedia.org/w/api.php?"; } }
        protected override string Parameters
        {
            get
            {
                string Param = "action=" + Action;
                if (Titles != "") Param += "&titles=" + HttpUtility.UrlEncode(Titles);
                if (Format != "") Param += "&format=" + Format;
                if (Redirects != "") Param += "&redirects";
                if (Export != "") Param += "&export";
                if (ExportNoWrap != "") Param += "&exportnowrap";

                return Param;
            }
        }

        private List<string> Templates = new List<string>();
        private List<string> Categories = new List<string>();

        private string _PageContent;
        public string Action { get; set; }
        public string Titles { get; set; }
        public string Format { get; set; }
        public string Redirects { get; set; }
        public string Export { get; set; }
        public string ExportNoWrap { get; set; }
        public string PageTitle { get; set; }
        public bool WikipediaAvailable { get; set; }
        public bool WikipediaArticleExists { get; set; }
        public bool UnbalancedHTMLcomments { get; set; }


        public string GetData()
        {
            GrabPage();
            ExtractXML();
            return _PageContent;
        }

        private string ExtractXML()
        {
            WikipediaAvailable = false;
            WikipediaArticleExists = false;

            using (XmlReader Reader = XmlReader.Create(new StringReader(Content)))
            {

                string[] thisElementName = new string[5];
                while (Reader.Read())
                {
                    switch (Reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            thisElementName[Reader.Depth] = Reader.Name;

                            if (thisElementName[0] == "mediawiki")
                            {
                                if (thisElementName[1] == "page")
                                {
                                    WikipediaAvailable = true;
                                }
                            }

                            break;

                        case XmlNodeType.Text:
                            if (thisElementName[0] == "mediawiki")
                            {
                                if (thisElementName[1] == "page")
                                {
                                    if (thisElementName[2] == "title")
                                    {
                                        PageTitle = Reader.Value;
                                    }

                                    if (thisElementName[2] == "revision")
                                    {
                                        if (thisElementName[3] == "text")
                                        {
                                            WikipediaArticleExists = true;
                                            UnbalancedHTMLcomments =  RemoveHTMLcomments(ReplaceDash(RemoveTerminators(Reader.Value)), out _PageContent);
                                        }
                                    }

                                }
                            }
                            break;
                    }

                }

                return _PageContent;

            }
        }

        private bool RemoveHTMLcomments(string OriginalText, out string RevisedText)
        {
            string ThisText = OriginalText;
            int CommentStart = 0;

            do
            {
                CommentStart = ThisText.IndexOf("<!--", StringComparison.Ordinal);
                if (CommentStart != -1)
                {
                    int CommentEnd = ThisText.IndexOf("-->", CommentStart + 3, StringComparison.Ordinal);

                    if (CommentEnd == -1 || CommentEnd < CommentStart)
                    {
                        RevisedText = ThisText;
                        return false;
                    }
                    else
                    {
                        string Comment = ThisText.Substring(CommentStart, CommentEnd - CommentStart + 3);
                        ThisText = ThisText.Replace(Comment, "");
                    }
                }

            } while (CommentStart != -1);

            RevisedText = ThisText;
            return true;
        }

        private string RemoveTerminators(string originalText)
        {
            string newText = originalText;
            newText = newText.Replace("\u000a", string.Empty);
            newText = newText.Replace("\u000b", string.Empty);
            newText = newText.Replace("\u000c", string.Empty);
            newText = newText.Replace("\u000d", string.Empty);
            newText = newText.Replace("\u0085", string.Empty);
            newText = newText.Replace("\u2028", string.Empty);
            newText = newText.Replace("\u2029", string.Empty);
            return newText;
        }


        private string ReplaceDash(string original)
        {
            string Output = original;

            Output = Output.Replace("\u058A", "-");
            Output = Output.Replace("\u05BE", "-");
            Output = Output.Replace("\u1400", "-");
            Output = Output.Replace("\u1806", "-");
            Output = Output.Replace("\u2010", "-");

            Output = Output.Replace("\u2011", "-");
            Output = Output.Replace("\u2012", "-");
            Output = Output.Replace("\u2013", "-");
            Output = Output.Replace("\u2014", "-");
            Output = Output.Replace("\u2015", "-");

            Output = Output.Replace("\u2E17", "-");
            Output = Output.Replace("\u2E1A", "-");
            Output = Output.Replace("\u2E3A", "-");
            Output = Output.Replace("\u2E3B", "-");
            Output = Output.Replace("\u301C", "-");

            Output = Output.Replace("\u3030", "-");
            Output = Output.Replace("\u30A0", "-");
            Output = Output.Replace("\uFE31", "-");
            Output = Output.Replace("\uFE32", "-");
            Output = Output.Replace("\uFE58", "-");

            Output = Output.Replace("\uFE63", "-");
            Output = Output.Replace("\uFF0D", "-");

            return Output;
        }

        public List<string> ExtractCategories()
        {

            int catStart = _PageContent.IndexOf("[[Category:", StringComparison.Ordinal);

            while (catStart > 0)
            {
                int catNextPipe = _PageContent.IndexOf("|", catStart, StringComparison.Ordinal);
                int catNextClose = _PageContent.IndexOf("]]", catStart, StringComparison.Ordinal);
                int catFinish = 0;

                if (catNextPipe < catNextClose && catNextPipe > 0)
                {
                    catFinish = catNextPipe;
                }
                else
                {
                    catFinish = catNextClose;
                }

                if (catStart != -1 && catFinish != -1 && catFinish > catStart)
                {
                    Categories.Add(_PageContent.Substring(catStart + 11, catFinish - catStart - 11).ToLower().Trim());
                    catStart = _PageContent.IndexOf("[[Category:", catFinish, StringComparison.Ordinal);
                }
                else
                {
                    //Error unbalancedCategoryBrackets;
                    catStart = -99;
                }
            }

            return Categories;

        }


        public List<string> ExtractTemplates()
        {

            int tplStart = _PageContent.IndexOf("{{", StringComparison.Ordinal);

            while (tplStart >= 0)
            {
                int tplNextPipe = _PageContent.IndexOf("|", tplStart, StringComparison.Ordinal);
                int tplNextClose = _PageContent.IndexOf("}}", tplStart, StringComparison.Ordinal);
                int tplFinish = 0;

                if (tplNextPipe < tplNextClose && tplNextPipe > 0)
                {
                    tplFinish = tplNextPipe;
                }
                else
                {
                    tplFinish = tplNextClose;
                }

                if (tplStart != -1 && tplFinish != -1 && tplFinish > tplStart)
                {
                    Templates.Add(_PageContent.Substring(tplStart + 2, tplFinish - tplStart - 2).ToLower().Trim());
                    tplStart = _PageContent.IndexOf("{{", tplFinish, StringComparison.Ordinal);
                }
                else
                {
                    // Error unbalancedTemplateBrackets
                    tplStart = -99;
                }
            }

            return Templates;
        }

        public static bool DelinkText(string OriginalText, out string RevisedText)
        {
            string thisText = OriginalText;

            int linkStart = 0;

            // Look for first [[ and ]], replace them with text
            // repeat until no more [[ or ]]
            do
            {
                linkStart = thisText.IndexOf("[[", StringComparison.Ordinal);
                if (linkStart != -1)
                {
                    //Found start
                    int linkEnd = thisText.IndexOf("]]", StringComparison.Ordinal);

                    if (linkEnd == -1 || linkEnd < linkStart)
                    {
                        //Did not find close
                        RevisedText = OriginalText;
                        return false;
                    }
                    else
                    {
                        // Found link, extract text
                        string link = thisText.Substring(linkStart, linkEnd - linkStart + 2);
                        string newlink = link.Substring(2, link.Length - 4);

                        // If its piped, remove left side
                        int pipe = newlink.IndexOf("|", StringComparison.Ordinal);
                        if (pipe != -1)
                        {
                            newlink = newlink.Substring(pipe + 1);
                        }
                        // Replace [[link]] with newlink
                        thisText = thisText.Replace(link, newlink);
                    }
                }
                else
                {
                    //Did not find a [[, probably finished
                    //but first check there are no closing ]]
                    int linkClose = thisText.IndexOf("]]", StringComparison.Ordinal);
                    if (linkClose != -1)
                    {
                        RevisedText = OriginalText;
                        return false;
                    }
                }

            } while (linkStart != -1);

            RevisedText = thisText;
            return true;
        }

    }
}
