using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlawsFightNightServer.Core.Entities
{
    public class Tournament
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<Team> Teams = new List<Team>();
    }
}
