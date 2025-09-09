using FlawsFightNightServer.Core.Enumerators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlawsFightNightServer.Core.Models
{
    public class Tournament
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TournamentType Type { get; set; }
        public int TeamSize { get; set; }
        public string TeamSizeFormat => $"{TeamSize}v{TeamSize}";
        public List<Team> Teams = [];
    }
}
