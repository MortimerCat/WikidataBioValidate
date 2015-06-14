using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiAccess;

namespace WikidataBioValidation
{
    class WikidataBiographyErrorLog : ErrorLog
    {
        public string Module {get {return "B";}}
        public List<ErrorMessage> Errors { get; set; }

        public WikidataBiographyErrorLog()
        {
            Errors = new List<ErrorMessage>();
//            Errors.Add(new ErrorMessage(Module, 0, "WikidataBiography module"));
        }

        public void CannotRetrieveData()
        {
            Errors.Add(new ErrorMessage(Module, 1, "Can not retrieve data from Wikidata"));
        }

        public void MultipleGender()
        {
            Errors.Add(new ErrorMessage(Module, 2, "Multiple genders defined"));
        }

         public void MultipleCitizenship()
        {
            Errors.Add(new ErrorMessage(Module, 3, "Multiple citizenships defined"));
        }

        public void MultipleInstance()
        {
            Errors.Add(new ErrorMessage(Module, 4, "Multiple instances defined"));
        }

        public void NoEnglishName()
        {
            Errors.Add(new ErrorMessage(Module, 5, "No English names defined"));
        }

        public void NoEnglishDescription()
        {
            Errors.Add(new ErrorMessage(Module, 6, "No English descriptions defined"));
        }

        public void NoENwiki()
        {
            Errors.Add(new ErrorMessage(Module, 7, "No English Wikipedia link"));
        }

        public void UnrecognisedGender(int qcode,string description)
        {
            Errors.Add(new ErrorMessage(Module, 8, "Unrecognised gender Q" + qcode.ToString() + " " + description));
        }

        public void UnrecognisedInstance(int qcode, string description)
        {
            Errors.Add(new ErrorMessage(Module, 9, "Unrecognised instance Q" + qcode.ToString() + " " + description));
        }

        public void NoGender()
        {
            Errors.Add(new ErrorMessage(Module, 10, "No gender defined"));
        }

        public void NoInstance()
        {
            Errors.Add(new ErrorMessage(Module, 11, "No instance defined"));
        }

        public void NoCitizenship()
        {
            Errors.Add(new ErrorMessage(Module, 12, "No citizenship defined"));
        }

        public void NotHuman()
        {
            Errors.Add(new ErrorMessage(Module,13,"Human instance not defined"));
        }

        public void MultipleBirth()
        {
            Errors.Add(new ErrorMessage(Module,14,"Multiple birth dates defined"));
        }
        
        public void MultipleDeath()
        {
            Errors.Add(new ErrorMessage(Module,15,"Multiple death dates defined"));
        }

        public void FutureBirth()
        {
            Errors.Add(new ErrorMessage(Module, 16, "Birth date in future"));
        }

        public void FutureDeath()
        {
            Errors.Add(new ErrorMessage(Module, 17, "Death date in future"));
        }

        public void NoBirth()
        {
            Errors.Add(new ErrorMessage(Module, 18, "No birth date defined"));
        }

        public void NoDeath()
        {
            Errors.Add(new ErrorMessage(Module, 19, "Person too old for no death date"));
        }

        public void TooYoung()
        {
            Errors.Add(new ErrorMessage(Module, 20, "Birth too recent for notabilty"));
        }

        public void DiedTooOld()
        {
            Errors.Add(new ErrorMessage(Module, 21, "Age at death too high"));
        }

        public void BirthAfterDeath()
        {
            Errors.Add(new ErrorMessage(Module, 22, "Person appears to have died before birth"));
        }

        public void DiedTooYoung()
        {
            Errors.Add(new ErrorMessage(Module, 23, "Died too young for notability"));
        }










    }
}
