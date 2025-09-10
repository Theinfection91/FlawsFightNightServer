using FlawsFightNightServer.Data.Handlers;
using FlawsFightNightServer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlawsFightNightServer.Core.Managers
{
    public class DataManager
    {
        public string Name => "DataManager";

        // Tournaments Database
        public TournamentsDatabaseFile TournamentsDatabaseFile { get; private set; }
        private readonly TournamentsDatabaseHandler _tournamentsDataHandler;

        public DataManager(TournamentsDatabaseHandler tournamentsDatabaseHandler)
        {
            _tournamentsDataHandler = tournamentsDatabaseHandler;
            LoadTournamentsDatabaseFile().Wait();
        }

        #region Tournaments Database
        public async Task LoadTournamentsDatabaseFile()
        {
            TournamentsDatabaseFile = await Task.Run(() => _tournamentsDataHandler.Load());
        }

        public async Task SaveTournamentsDatabaseFile()
        {
            await Task.Run(() => _tournamentsDataHandler.Save(TournamentsDatabaseFile));
        }

        public async Task SaveAndReloadTournamentsDatabaseFile()
        {
            await SaveTournamentsDatabaseFile();
            await LoadTournamentsDatabaseFile();
        }
        #endregion
    }
}
