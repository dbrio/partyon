using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyOn.model
{
    class UserActivityEventArgs : EventArgs
    {
        public IList<modelActivity> ActivityResults { get; private set;  }

        public UserActivityEventArgs(IList<modelActivity> results)
        {
            this.ActivityResults = results;
        }
    }
}
