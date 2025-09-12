using FlawsFightNightServer.Core.Models;
using FlawsFightNightServer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlawsFightNightServer.Core.Managers
{
    public class TeamManager : BaseDataDriven
    {
        public TeamManager(DataManager dataManager) : base("TeamManager", dataManager)
        {

        }

        public string? GenerateTeamId()
        {
            bool isUnique = false;
            string uniqueId;

            while (!isUnique)
            {
                Random random = new();
                int randomInt = random.Next(100, 1000);
                uniqueId = $"S{randomInt}";

                // Check if the generated ID is unique
                if (!IsTeamIdInDatabase(uniqueId))
                {
                    isUnique = true;
                    return uniqueId;
                }
            }
            return null;
        }

        public bool IsTeamIdInDatabase(string id)
        {
            return _dataManager.TournamentsDatabaseFile.Tournaments.Any(t => t.Teams.Any(team => team.Id.Equals(id, StringComparison.OrdinalIgnoreCase)));
        }

        public bool IsTeamNameUnique(string teamName)
        {
            foreach (var tournament in _dataManager.TournamentsDatabaseFile.Tournaments)
            {
                if (tournament.Teams.Any(t => t.Name.Equals(teamName, StringComparison.OrdinalIgnoreCase)))
                {
                    return false;
                }
            }
            return true;
        }

        public Team? GetTeamById(string id)
        {
            foreach (var tournament in _dataManager.TournamentsDatabaseFile.Tournaments)
            {
                var team = tournament.Teams.FirstOrDefault(t => t.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
                if (team != null)
                {
                    return team;
                }
            }
            return null;
        }

        public Team CreateNewTeam(string teamName, Dictionary<ulong, string> members)
        {
            var newTeam = new Team
            {
                Id = GenerateTeamId() ?? throw new Exception("Failed to generate a unique team ID."),
                Name = teamName,
                Members = members
            };
            return newTeam;
        }
    }
}
