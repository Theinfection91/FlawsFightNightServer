using FlawsFightNightServer.Api.DTOs.Teams;
using FlawsFightNightServer.Core.Managers;
using FlawsFightNightServer.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlawsFightNightServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private TeamManager _teamManager;
        private TournamentManager _tournamentManager;
        public TeamsController(TeamManager teamManager, TournamentManager tournamentManager)
        {
            _teamManager = teamManager;
            _tournamentManager = tournamentManager;
        }

        [HttpGet("{guildId}/{teamId}")]
        public IActionResult GetTeam(string teamId, ulong guildId)
        {
            var team = _teamManager.GetTeamById(teamId, guildId);
            if (team == null)
                return NotFound();

            return Ok(team);
        }

        [HttpGet("{guildId}/{tournamentId}/all")]
        public IActionResult GetAllTeams(string tournamentId, ulong guildId)
        {
            var tournament = _tournamentManager.GetTournamentById(tournamentId, guildId);
            if (tournament == null)
                return NotFound("Tournament not found.");
            return Ok(tournament.Teams);
        }

        [HttpPost("register")]
        public IActionResult RegisterTeam([FromBody] RegisterTeamRequest registerTeamRequest)
        {
            try
            {
                if (!_teamManager.IsTeamNameUnique(registerTeamRequest.TeamName, registerTeamRequest.GuildId))
                {
                    return Conflict("Team name is already taken.");
                }

                if (!_tournamentManager.IsTournamentIdInDatabase(registerTeamRequest.TournamentId, registerTeamRequest.GuildId))
                {
                    return NotFound("Tournament not found.");
                }

                Tournament? tournament = _tournamentManager.GetTournamentById(registerTeamRequest.TournamentId, registerTeamRequest.GuildId);

                if (tournament == null)
                {
                    return NotFound("Tournament not found.");
                }

                Team newTeam = _teamManager.CreateNewTeam(
                    registerTeamRequest.TeamName,
                    registerTeamRequest.Members,
                    registerTeamRequest.GuildId
                );
                if (newTeam == null)
                {
                    return BadRequest("Failed to create a new team.");
                }

                // Add the new team to the specified tournament
                tournament.AddTeam(newTeam);

                // Save changes to the database
                _tournamentManager.SaveAndReloadTournaments();

                return CreatedAtAction(
                    nameof(GetTeam),
                    new { id = newTeam.Id },
                    new
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
