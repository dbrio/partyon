using PartyOn.model.porfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyOn.model.profile
{
    public class PerfilEverArg
    {
         public IList<modelProfile> ResultsPerfil { get; private set; }

         public PerfilEverArg(IList<modelProfile> results)
        {
            this.ResultsPerfil = results;
        }
    }
}
