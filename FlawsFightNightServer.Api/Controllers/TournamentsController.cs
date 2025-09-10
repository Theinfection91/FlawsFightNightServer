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
            var tournament = _tournamentManager.GetById(id);
            if (tournament == null)
                return NotFound();

            return Ok(tournament);
        }

        // POST: api/tournaments/create
        [HttpPost("create")]
        public IActionResult CreateTournament([FromBody] Tournament newTournament)
        {
            // Check if given ID is already in use
            if (_tournamentManager.IsIdInDatabase(newTournament.Id))
            {
                return Conflict(new { message = $"Tournament ID ({newTournament.Id}) already exists." });
            }

            // Add the new tournament
            _tournamentManager.AddTournament(newTournament);

            return CreatedAtAction(nameof(GetTournament), new { id = newTournament.Id }, newTournament);

        }
    }
}
