using FlawsFightNightServer.Api.DTOs.Tournaments;
using FlawsFightNightServer.Core.Managers;
using FlawsFightNightServer.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FlawsFightNightServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        private TournamentManager _tournamentManager;
        public TournamentsController(TournamentManager tournamentManager)
        {
            _tournamentManager = tournamentManager;
        }

        // GET: api/tournaments
        [HttpGet]
        public IActionResult GetTournaments()
        {
            // Hardcoded sample data
            var tournaments = new List<object>
            {
                new { Id = 1, Name = "Flaws Fight Night #1", Teams = 8 },
                new { Id = 2, Name = "Flaws Fight Night #2", Teams = 16 }
            };

            return Ok(tournaments);
        }

        [HttpGet("{id}")]
        public IActionResult GetTournament(string id)
        {
            var tournament = _tournamentManager.GetTournamentById(id);
            if (tournament == null)
                return NotFound();

            return Ok(tournament);
        }

        // POST: api/tournaments/create
        [HttpPost("create")]
        public IActionResult CreateTournament([FromBody] CreateTournamentRequest createTournamentRequest)
        {
            try
            {
                Tournament newTournament = _tournamentManager.CreateNewTournament(
                    createTournamentRequest.TournamentName,
                    createTournamentRequest.TournamentType,
                    createTournamentRequest.TeamSize
                );
                if (newTournament == null)
                {
                    return BadRequest("Failed to create a new tournament.");
                }
                // Add the new tournament
                _tournamentManager.AddTournament(newTournament);

                return CreatedAtAction(
                    nameof(GetTournament),
                    new { id = newTournament.Id }, // route values
                    new
                    {
                        message = "Tournament created successfully.",
                        tournamentId = newTournament.Id,
                        tournamentName = newTournament.Name,
                        tournamentType = newTournament.Type,
                        teamSizeFormat = newTournament.TeamSizeFormat
                    } // response body
                );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("lock-teams")]
        public IActionResult LockTeams([FromBody] LockTeamsRequest lockTeamsRequest)
        {
            try
            {
                if (!_tournamentManager.IsTournamentIdInDatabase(lockTeamsRequest.TournamentId))
                {
                    return NotFound("Tournament not found.");
                }
                Tournament tournament = _tournamentManager.GetTournamentById(lockTeamsRequest.TournamentId);

                if (tournament.IsTeamsLocked)
                {
                    return BadRequest("Teams are already locked for this tournament.");
                }

                tournament.LockTeams();
                // Save changes to the database
                _tournamentManager.SaveAndReloadTournaments();
                return Ok(new { message = "Teams locked successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
