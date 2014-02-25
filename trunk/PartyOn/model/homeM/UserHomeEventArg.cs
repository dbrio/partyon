using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyOn.model.homeM
{
   public class UserHomeEventArg: EventArgs
    {
        public IList<modelHome> ResultsHome { get; private set; }

        public UserHomeEventArg(IList<modelHome>results)
        {
            this.ResultsHome = results;
        }
    }
}
