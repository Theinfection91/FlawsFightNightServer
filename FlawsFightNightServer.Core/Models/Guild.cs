using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlawsFightNightServer.Core.Models
{
    public class Guild
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public List<Tournament> Tournaments { get; set; } = new();
        public Guild() { }
        public void AddTournament(Tournament tournament)
        {
            Tournaments.Add(tournament);
        }
    }
}
