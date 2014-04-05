using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyOn.model.song
{
   public class UserSongEverArg
    {
       public IList<modelSong> ResultsSong { get; private set; }

        public UserSongEverArg(IList<modelSong>results)
        {
            this.ResultsSong = results;
        }
    }
}

