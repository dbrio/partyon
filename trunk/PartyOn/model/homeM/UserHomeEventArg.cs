﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyOn.model.homeM
{
   public class UserHomeEventArg: EventArgs
    {
        public IList<modelHome> Results { get; private set; }

        public UserHomeEventArg(IList<modelHome>results)
        {
            this.Results = results;
        }
    }
}
