using FlawsFightNightServer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlawsFightNightServer.Data.Handlers
{
    public class TournamentsDatabaseHandler : BaseDataHandler<TournamentsDatabaseFile>
    {
        public TournamentsDatabaseHandler() : base("tournaments.json", "Data")
        {

        }
    }
}
