using FlawsFightNightServer.Core.Models;
using FlawsFightNightServer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlawsFightNightServer.Core.Managers
{
    public class TournamentManager : BaseDataDriven
    {
        public TournamentManager(DataManager dataManager) : base("TournamentManager", dataManager)
        {
            
        }

        public bool IsIdInDatabase(string id)
        {
            return _dataManager.TournamentsDatabaseFile.Tournaments.Any(t => t.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        public Tournament? GetById(string id)
        {
            return _dataManager.TournamentsDatabaseFile.Tournaments.FirstOrDefault(t => t.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        public void AddTournament(Tournament tournament)
        {
            _dataManager.TournamentsDatabaseFile.Tournaments.Add(tournament);
            _dataManager.SaveAndReloadTournamentsDatabaseFile().Wait();
        }
    }  
}
