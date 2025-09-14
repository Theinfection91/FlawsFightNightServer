using FlawsFightNightServer.Api.DTOs.Tournaments;
using FlawsFightNightServer.Core.Managers;
using FlawsFightNightServer.Core.Models;
using FlawsFightNightServer.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlawsFightNightServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private TournamentManager _tournamentManager;
        public TournamentsController(AppDbContext dbContext, TournamentManager tournamentManager)
        {
            _dbContext = dbContext;
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
        public async Task<IActionResult> CreateTournament([FromBody] CreateTournamentRequest createTournamentRequest)
        {
            try
            {
                var guild = await _dbContext.Guilds.FindAsync(createTournamentRequest.GuildId);
                if (guild == null)
                {
                    guild = new Guild
                    {
                        Id = createTournamentRequest.GuildId,
                        Name = $"Guild_{createTournamentRequest.GuildId}" // placeholder name, or fetch from bot
                    };
                    _dbContext.Guilds.Add(guild);
                    await _dbContext.SaveChangesAsync(); // commit guild first
                }

                Tournament newTournament = _tournamentManager.CreateNewTournament(
                    createTournamentRequest.TournamentName,
                    createTournamentRequest.TournamentType,
                    createTournamentRequest.TeamSize,
                    createTournamentRequest.GuildId,
                    guild
                );
                if (newTournament == null)
                {
                    return BadRequest("Failed to create a new tournament.");
                }
                // Add the new tournament (OLD WAY)
                //_tournamentManager.AddTournament(createTournamentRequest.GuildId, newTournament);

                // Save changes to the database
                _dbContext.Tournaments.Add(newTournament);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetTournament),
                    new { guildId = createTournamentRequest.GuildId, tournamentId = newTournament.Id }, // must match param names
                    new
                    {
                        message = "Tournament created successfully.",
                        tournamentId = newTournament.Id,
                        tournamentName = newTournament.Name,
                        tournamentType = newTournament.Type,
                        teamSizeFormat = newTournament.TeamSizeFormat
                    }
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
