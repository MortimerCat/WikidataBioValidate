using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WikidataBioValidation
{

    class WikidataCache
    {
        Dictionary<string, string> _Cache = new Dictionary<string,string>();
        private readonly string LABELCACHE = Path.GetTempPath() + "wikidataLabelCache";

        public WikidataCache()
        {
            if (!File.Exists(LABELCACHE))
                File.Create(LABELCACHE).Close();

            if (_Cache.Count == 0)
            {
                using (StreamReader Sr = new StreamReader(LABELCACHE))
                {
                    string property;
                    while ((property = Sr.ReadLine()) != null)
                    {
                        string Description = Sr.ReadLine();
                        _Cache.Add(property, Description);
                    }
                }
            }

        }

        public string RetrieveData(string itemid)
        {
            string Qcode = itemid;
            string Description = null;
            if (_Cache.TryGetValue(itemid, out Description))
            {
                return Description;
            }
            else
            {
                return Lookup(Qcode);
            }
        }

        public string Lookup(string qcode)
        {
            WikidataIO IO = new WikidataIO();
            IO.Action = "wbgetentities";
            IO.Format = "json";
            IO.Ids = "Q" + qcode;
            IO.Props = "labels";
            IO.Languages = "en";
            WikidataFields Fields = new WikidataFields();

            Fields = IO.GetData();

            using (StreamWriter Sw = File.AppendText(LABELCACHE))
            {
                Sw.WriteLine(qcode);
                Sw.WriteLine(Fields.Name);
            }

            _Cache.Add(qcode, Fields.Name);

            return Fields.Name;
        }

    }
}
