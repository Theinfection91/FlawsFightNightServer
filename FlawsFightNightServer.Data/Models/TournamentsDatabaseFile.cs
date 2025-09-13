using FlawsFightNightServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlawsFightNightServer.Data.Models
{
    public class TournamentsDatabaseFile
    {
        public Dictionary<ulong, List<Tournament>> TournamentsByGuild { get; set; } = [];
    }
}
