using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlawsFightNightServer.Core.Models
{
    public class Member
    {
        public ulong DiscordId { get; set; }
        public string? DisplayName { get; set; }
    }
}
