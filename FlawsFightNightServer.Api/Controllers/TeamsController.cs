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

        [HttpGet("{id}")]
        public IActionResult GetTeam(string id)
        {
            return Ok(new { message = "This is a placeholder response from TeamsController." });
        }

        [HttpPost("register")]
        public IActionResult RegisterTeam([FromBody] RegisterTeamRequest registerTeamRequest)
        {
            try
            {
                if (!_teamManager.IsTeamNameUnique(registerTeamRequest.TeamName))
                {
                    return Conflict("Team name is already taken.");
                }

                if (!_tournamentManager.IsIdInDatabase(registerTeamRequest.TournamentId))
                {
                    return NotFound("Tournament not found.");
                }

                Tournament tournament = _tournamentManager.GetTournamentById(registerTeamRequest.TournamentId);

                Team newTeam = _teamManager.CreateNewTeam(
                    registerTeamRequest.TeamName,
                    registerTeamRequest.Members
                );
                if (newTeam == null)
                {
                    return BadRequest("Failed to create a new team.");
                }
                
                // Add the new team to the specified tournament
                tournament.AddTeam(newTeam);

                // Save changes to the database
                _tournamentManager.SaveAndReloadTournaments();

                return Ok(new { message = "Team registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            // Place holder logic here
            
        }
    }
}
