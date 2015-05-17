using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WikidataBioValidation
{
    public abstract class WikimediaApi
    {
        private int _Second = 0;
        protected abstract string APIurl { get; }

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
    }


}
