using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;

namespace PartyOn.model.userData
{
    class userDataDbDataContext : DataContext
    {
        public userDataDbDataContext(string conectionString)
            : base(conectionString)
        {

        }

        public Table<userDataDB> userDataDB2
        {
            get
            {
                return this.GetTable<userDataDB>();
            }
        }
    }
}
