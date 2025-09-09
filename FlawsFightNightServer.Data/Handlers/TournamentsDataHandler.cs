using FlawsFightNightServer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlawsFightNightServer.Data.Handlers
{
    public class TournamentsDataHandler : BaseDataHandler<List<Tournament>>
    {
        public TournamentsDataHandler() : base("tournaments.json", "Data")
        {

        }
    }
}
