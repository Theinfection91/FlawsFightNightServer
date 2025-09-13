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



        public string? GenerateTournamentId(ulong guildId)
        {
            bool isUnique = false;
            string uniqueId;

            while (!isUnique)
            {
                Random random = new();
                int randomInt = random.Next(100, 1000);
                uniqueId = $"T{randomInt}";

                // Check if the generated ID is unique
                if (!IsTournamentIdInDatabase(uniqueId, guildId))
                {
                    isUnique = true;
                    return uniqueId;
                }
            }
            return null;
        }

        public bool IsTournamentIdInDatabase(string tournamentId, ulong guildId)
        {
            if (_dataManager.TournamentsDatabaseFile.TournamentsByGuild.TryGetValue(guildId, out var tournaments))
            {
                foreach (var tournament in tournaments)
                {
                    if (tournament.Id.Equals(tournamentId, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public Tournament? GetTournamentById(string tournamentId, ulong guildId)
        {
            if (_dataManager.TournamentsDatabaseFile.TournamentsByGuild.TryGetValue(guildId, out var tournaments))
            {
                foreach (var tournament in tournaments)
                {
                    if (tournament.Id.Equals(tournamentId, StringComparison.OrdinalIgnoreCase))
                    {
                        return tournament;
                    }
                }
            }
            return null;
        }

        public List<Tournament> GetAllTournamentsForGuild(ulong guildId)
        {
            if (_dataManager.TournamentsDatabaseFile.TournamentsByGuild.TryGetValue(guildId, out var tournaments))
            {
                return tournaments;
            }
            return new List<Tournament>();
        }

        public List<Tournament> GetAllTournaments()
        {
            var allTournaments = new List<Tournament>();
            foreach (var guildTournaments in _dataManager.TournamentsDatabaseFile.TournamentsByGuild.Values)
            {
                allTournaments.AddRange(guildTournaments);
            }
            return allTournaments;
        }

        public Tournament CreateNewTournament(string name, string type, int teamSize, ulong guildId)
        {
            var newTournament = new Tournament
            {
                Id = GenerateTournamentId(guildId) ?? throw new Exception("Failed to generate a unique Tournament ID."),
                Name = name,
                Type = TournamentTypeResolver(type),
                TeamSize = teamSize,
                Teams = new List<Team>(),
                IsTeamsLocked = false
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

        public void AddTournament(ulong guildId, Tournament tournament)
        {
            // Ensure the guild entry exists
            if (!_dataManager.TournamentsDatabaseFile.TournamentsByGuild.ContainsKey(guildId))
            {
                _dataManager.TournamentsDatabaseFile.TournamentsByGuild[guildId] = new List<Tournament>();
            }
            // Add the tournament to the guild's list
            _dataManager.TournamentsDatabaseFile.TournamentsByGuild[guildId].Add(tournament);

            // Save changes and reload the database file
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
