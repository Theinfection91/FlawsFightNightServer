using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FlawsFightNightServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
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

        // GET: api/tournaments/{id}
        [HttpGet("{id}")]
        public IActionResult GetTournament(int id)
        {
            // Just returning dummy data
            var tournament = new { Id = id, Name = $"Flaws Fight Night #{id}", Teams = 8 };
            return Ok(tournament);
        }

        // POST: api/tournaments
        [HttpPost]
        public IActionResult CreateTournament([FromBody] dynamic newTournament)
        {
            // For now, just echo back what was sent
            return CreatedAtAction(nameof(GetTournament), new { id = 999 }, newTournament);
        }
    }
}
