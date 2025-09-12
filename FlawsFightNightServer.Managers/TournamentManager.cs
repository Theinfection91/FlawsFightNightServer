using FlawsFightNightServer.Core.Enumerators;
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



        public string? GenerateTournamentId()
        {
            bool isUnique = false;
            string uniqueId;

            while (!isUnique)
            {
                Random random = new();
                int randomInt = random.Next(100, 1000);
                uniqueId = $"T{randomInt}";

                // Check if the generated ID is unique
                if (!IsTournamentIdInDatabase(uniqueId))
                {
                    isUnique = true;
                    return uniqueId;
                }
            }
            return null;
        }

        public bool IsTournamentIdInDatabase(string id)
        {
            return _dataManager.TournamentsDatabaseFile.Tournaments.Any(t => t.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        public Tournament? GetTournamentById(string id)
        {
            return _dataManager.TournamentsDatabaseFile.Tournaments.FirstOrDefault(t => t.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        public Tournament CreateNewTournament(string name, string type, int teamSize)
        {
            var newTournament = new Tournament
            {
                Id = GenerateTournamentId() ?? throw new Exception("Failed to generate a unique Tournament ID."),
                Name = name,
                Type = TournamentTypeResolver(type),
                TeamSize = teamSize,
                Teams = new List<Team>()
            };
            return newTournament;
        }

        public TournamentType TournamentTypeResolver(string type)
        {
            return type.ToLower() switch
            {
                "ladder" => TournamentType.Ladder,
                "single_elimination" => TournamentType.SingleElimination,
                "double_elimination" => TournamentType.DoubleElimination,
                "roundrobin" => TournamentType.RoundRobin,
                _ => throw new ArgumentException($"Invalid tournament type: {type}")
            };
        }

        public void AddTournament(Tournament tournament)
        {
            _dataManager.TournamentsDatabaseFile.Tournaments.Add(tournament);
            _dataManager.SaveAndReloadTournamentsDatabaseFile().Wait();
        }

        public void SaveTournaments()
        {
            _dataManager.SaveAndReloadTournamentsDatabaseFile().Wait();
        }

        public void LoadTournaments()
        {
            _dataManager.LoadTournamentsDatabaseFile().Wait();
        }

        public void SaveAndReloadTournaments()
        {
            _dataManager.SaveAndReloadTournamentsDatabaseFile().Wait();
        }
    }  
}
