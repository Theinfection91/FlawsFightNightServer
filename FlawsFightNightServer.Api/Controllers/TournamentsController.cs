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

        [HttpGet("all")]
        public IActionResult GetAllTournaments()
        {
            var tournaments = _tournamentManager.GetAllTournaments();
            return Ok(tournaments);
        }

        [HttpGet("{guildId}")]
        public IActionResult GetAllTournamentsForGuild(ulong guildId)
        {
            var tournaments = _tournamentManager.GetAllTournamentsForGuild(guildId);
            return Ok(tournaments);
        }

        [HttpGet("{guildId}/{tournamentId}")]
        public IActionResult GetTournament(string tournamentId, ulong guildId)
        {
            var tournament = _tournamentManager.GetTournamentById(tournamentId, guildId);
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
                    createTournamentRequest.TeamSize,
                    createTournamentRequest.GuildId
                );
                if (newTournament == null)
                {
                    return BadRequest("Failed to create a new tournament.");
                }
                // Add the new tournament
                _tournamentManager.AddTournament(createTournamentRequest.GuildId, newTournament);

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
                if (!_tournamentManager.IsTournamentIdInDatabase(lockTeamsRequest.TournamentId, lockTeamsRequest.GuildId))
                {
                    return NotFound("Tournament not found.");
                }
                Tournament tournament = _tournamentManager.GetTournamentById(lockTeamsRequest.TournamentId, lockTeamsRequest.GuildId);

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
