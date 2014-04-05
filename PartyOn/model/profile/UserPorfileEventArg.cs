using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyOn.model.porfile
{
   public class UserPorfileEventArg:EventArgs
    {
         public IList<modelProfile> ResultsProfile { get; private set; }

         public UserPorfileEventArg(IList<modelProfile> results)
        {
            this.ResultsProfile = results;
        }
    }
}
