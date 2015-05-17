using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikidataBioValidation
{
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
    }
}
