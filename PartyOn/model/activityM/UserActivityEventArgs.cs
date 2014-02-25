using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyOn.model
{
    public class UserActivityEventArgs : EventArgs
    {
        public IList<modelActivity> Results { get; private set; }

        public UserActivityEventArgs(IList<modelActivity> results)
        {
            this.Results = results;
        }
    }

}
