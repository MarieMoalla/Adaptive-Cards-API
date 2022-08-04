using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaptiveCardsFakeAPI.Entities
{
    public class User
    {
        public int id { get; set; }
        public string userName { get; set; }
        public bool selected { get; set; } = false;
    }
}
