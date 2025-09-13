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

        public string? GenerateTeamId(ulong guildId)
        {
            bool isUnique = false;
            string uniqueId;

            while (!isUnique)
            {
                Random random = new();
                int randomInt = random.Next(100, 1000);
                uniqueId = $"S{randomInt}";

                // Check if the generated ID is unique
                if (!IsTeamIdInDatabase(uniqueId, guildId))
                {
                    isUnique = true;
                    return uniqueId;
                }
            }
            return null;
        }

        public bool IsTeamIdInDatabase(string teamId, ulong guildId)
        {
            if (_dataManager.TournamentsDatabaseFile.TournamentsByGuild.TryGetValue(guildId, out var tournaments))
            {
                foreach (var tournament in tournaments)
                {
                    if (tournament.Teams.Any(t => t.Id.Equals(teamId, StringComparison.OrdinalIgnoreCase)))
                    {
                        return true;
                    }
                }
                return false;
            }
            return false;
        }

        public bool IsTeamNameUnique(string teamName, ulong guildId)
        {
            if (_dataManager.TournamentsDatabaseFile.TournamentsByGuild.TryGetValue(guildId, out var tournaments))
            {
                foreach (var tournament in tournaments)
                {
                    if (tournament.Teams.Any(t => t.Name.Equals(teamName, StringComparison.OrdinalIgnoreCase)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public Team? GetTeamById(string teamId, ulong guildId)
        {
            if (_dataManager.TournamentsDatabaseFile.TournamentsByGuild.TryGetValue(guildId, out var tournaments))
            {
                foreach (var tournament in tournaments)
                {
                    var team = tournament.Teams.FirstOrDefault(t => t.Id.Equals(teamId, StringComparison.OrdinalIgnoreCase));
                    if (team != null)
                    {
                        return team;
                    }
                }
            }
            return null;
        }

        public List<Team> GetAllTeamsForGuild(ulong guildId)
        {
            List<Team> allTeams = new();
            if (_dataManager.TournamentsDatabaseFile.TournamentsByGuild.TryGetValue(guildId, out var tournaments))
            {
                foreach (var tournament in tournaments)
                {
                    allTeams.AddRange(tournament.Teams);
                }
            }
            return allTeams;
        }

        public Team CreateNewTeam(string teamName, Dictionary<ulong, string> members, ulong guildId)
        {
            // Convert the members dictionary to a list of Member objects
            List<Member> membersList = new();
            foreach (var member in members)
            {
                membersList.Add(new Member { DiscordId = member.Key, DisplayName = member.Value });
            }

            // Create the new team
            var newTeam = new Team
            {
                Id = GenerateTeamId(guildId) ?? throw new Exception("Failed to generate a unique team ID."),
                Name = teamName,
                Members = membersList
            };
            return newTeam;
        }
    }
}
