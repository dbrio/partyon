using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyOn.model.userData
{
    [Table]
    public class userDataDB
    {
        [Column(IsDbGenerated = true, IsPrimaryKey = true)]
        public int ID { get; set; }
        [Column]
        public string username { get; set; }
        [Column]
        public int UserID { get; set; }
    }
}
