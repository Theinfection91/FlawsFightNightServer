using FlawsFightNightServer.Api.DTOs.Teams;
using FlawsFightNightServer.Core.Managers;
using FlawsFightNightServer.Core.Models;
using FlawsFightNightServer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FlawsFightNightServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private AppDbContext _dbContext;
        private TeamManager _teamManager;
        private TournamentManager _tournamentManager;
        public TeamsController(AppDbContext dbContext, TeamManager teamManager, TournamentManager tournamentManager)
        {
            _dbContext = dbContext;
            _teamManager = teamManager;
            _tournamentManager = tournamentManager;
        }

        [HttpGet("{guildId}/{teamId}")]
        public async Task<IActionResult> GetTeam(string teamId, ulong guildId)
        {
            var team = await _dbContext.Teams
        .Include(t => t.Members)
        .FirstOrDefaultAsync(t => t.Id == teamId && t.Tournament.GuildId == guildId);

            if (team == null) return NotFound();

            return Ok(team);
        }

        [HttpGet("{guildId}/{tournamentId}/all")]
        public IActionResult GetAllTeamsForTournament(string tournamentId, ulong guildId)
        {
            var tournament = _tournamentManager.GetTournamentById(tournamentId, guildId);
            if (tournament == null)
                return NotFound("Tournament not found.");
            return Ok(tournament.Teams);
        }

        [HttpGet("{guildId}/all")]
        public IActionResult GetAllTeamsForGuild(ulong guildId)
        {
            var teams = _teamManager.GetAllTeamsForGuild(guildId);
            if (teams == null || teams.Count == 0)
                return NotFound("No teams found for this guild.");
            return Ok(teams);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterTeam([FromBody] RegisterTeamRequest registerTeamRequest)
        {
            try
            {
                //if (!_teamManager.IsTeamNameUnique(registerTeamRequest.TeamName, registerTeamRequest.GuildId))
                //{
                //    return Conflict("Team name is already taken.");
                //}

                //if (!_tournamentManager.IsTournamentIdInDatabase(registerTeamRequest.TournamentId, registerTeamRequest.GuildId))
                //{
                //    return NotFound("Tournament not found.");
                //}
                Tournament tournament = _dbContext.Tournaments
                    .Include(t => t.Teams)
                    .FirstOrDefault(t => t.Id == registerTeamRequest.TournamentId && t.GuildId == registerTeamRequest.GuildId);
                //Tournament? tournament = _tournamentManager.GetTournamentById(registerTeamRequest.TournamentId, registerTeamRequest.GuildId);

                if (tournament == null)
                {
                    return NotFound("Tournament not found.");
                }

                Team newTeam = _teamManager.CreateNewTeam(
                    registerTeamRequest.TeamName,
                    registerTeamRequest.Members,
                    registerTeamRequest.GuildId,
                    tournament.Id,
                    tournament
                );
                if (newTeam == null)
                {
                    return BadRequest("Failed to create a new team.");
                }

                // Add the new team to the specified tournament
                //tournament.AddTeam(newTeam);
                _dbContext.Teams.Add(newTeam);
                await _dbContext.SaveChangesAsync();

                // Save changes to the database
                //_tournamentManager.SaveAndReloadTournaments();

                return CreatedAtAction(
                    nameof(GetTeam),
                    routeValues: new
                    {
                        guildId = registerTeamRequest.GuildId,
                        teamId = newTeam.Id
                    },
                    value: new
                    {
                        message = "Team registered successfully.",
                        teamId = newTeam.Id,
                        teamName = newTeam.Name,
                        members = newTeam.Members
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("delete")]
        public IActionResult DeleteTeam([FromBody] DeleteTeamRequest removeTeamRequest)
        {
            try
            {
                if (!_tournamentManager.IsTournamentIdInDatabase(removeTeamRequest.TournamentId, removeTeamRequest.GuildId))
                {
                    return NotFound("Tournament not found.");
                }
                Tournament tournament = _tournamentManager.GetTournamentById(removeTeamRequest.TournamentId, removeTeamRequest.GuildId);
                Team team = _teamManager.GetTeamById(removeTeamRequest.TeamId, removeTeamRequest.GuildId);
                if (team == null)
                {
                    return NotFound("Team not found.");
                }
                // Remove the team from the specified tournament
                tournament.Teams.RemoveAll(t => t.Id.Equals(removeTeamRequest.TeamId, StringComparison.OrdinalIgnoreCase));
                // Save changes to the database
                _tournamentManager.SaveAndReloadTournaments();
                return Ok(new
                {
                    message = "Team removed successfully.",
                    teamId = team.Id,
                    teamName = team.Name
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
