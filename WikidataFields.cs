using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikidataBioValidation
{
    /// <summary>
    /// Container class to hold a persons information
    /// Note that is not a complete list, just the ones needed to validate.
    /// </summary>
    class WikidataFields
    {
        public string ID { set; get; }
        public string WikipediaLink { set;  get; }
        public Wikidate DateOfBirth { set; get; }
        public Wikidate DateOfDeath { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Gender { set; get; }
        public string CitizenOf { set; get; }
        public string InstanceOf { set; get; }

        public WikidataFields()
        {
            DateOfBirth = new Wikidate();
            DateOfDeath = new Wikidate();
        }

    }

}
