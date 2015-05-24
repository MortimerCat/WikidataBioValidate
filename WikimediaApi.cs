using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;

namespace WikidataBioValidation
{
    /// <summary>
    /// Abstract class that does the actual call to the Wiki websites.
    /// Must set Target Framework to .NET framework 4 (not client profile)
    /// if you are using this in your own project, please change BOTNAME and CONTACT below
    /// </summary>
    public abstract class WikimediaApi
    {
        private int _Second = 0;
        private const string BOTNAME = "Perigliobot";
        private const string CONTACT = "Wikidata@lynxmail.co.uk";

        protected string Content { get; private set; }

        protected abstract string APIurl { get; }
        protected abstract string Parameters { get; }

        /// <summary>
        /// Make sure we wait a second between calls.
        /// This simple method only throttles fast running scripts allowing slower ones to run at full speed.
        /// </summary>
        private void ThrottleWikiAccess()
        {
            if (DateTime.Now.Second == _Second)
            {
                Thread.Sleep(1000);
            }
            _Second = DateTime.Now.Second;
        }

        /// <summary>
        /// Method used to grab page from Wiki website, and store into Content property.
        /// </summary>
        /// <returns></returns>
        protected void GrabPage()
        {
            ThrottleWikiAccess();
            LoadPage(DownloadPage());
        }

        /// <summary>
        /// Read page from download, store in Content property
        /// </summary>
        /// <param name="tempfile"></param>
        private void LoadPage(string tempfile)
        {
            Content = File.ReadAllText(tempfile);
            File.Delete(tempfile);
        }

        /// <summary>
        /// Download page from Wiki web site into temp file
        /// </summary>
        /// <returns>Temp file name</returns>
        private string DownloadPage()
        {
            string Tempfile = Path.GetTempFileName();

            WebClient wikiClient = new WebClient();
            wikiClient.Headers.Add("user-agent", BOTNAME + " Contact: " + CONTACT + ")");

            try
            {
                string FullURL = APIurl + Parameters;
                wikiClient.DownloadFile(FullURL, Tempfile);
            }
            catch
            {
                Tempfile = null;
            }

            return Tempfile;
        }
    }
}
