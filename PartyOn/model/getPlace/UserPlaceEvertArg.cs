using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyOn.model.getPlace
{
   public class UserPlaceEvertArg:EventArgs
    {
        public IList<modelPlace> ResultsPlace { get; private set; }

        public UserPlaceEvertArg(IList<modelPlace> results)
        {
            this.ResultsPlace = results;
        }

    }
}
