using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlawsFightNightServer.Core.Models
{
    public class Member
    {
        public ulong Id { get; set; }
        public string? Name { get; set; }

        public string TeamId { get; set; }
        public Team Team { get; set; }
    }
}
