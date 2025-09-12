using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlawsFightNightServer.Core.Models
{
    public class Team
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Member> Members { get; set; }

        public Team()
        {
            Members = new List<Member>();
        }
    }
}
