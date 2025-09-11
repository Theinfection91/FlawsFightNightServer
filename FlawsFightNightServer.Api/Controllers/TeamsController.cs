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
        public TeamsController(TeamManager teamManager)
        {
            _teamManager = teamManager;
        }

        [HttpGet("{id}")]
        public IActionResult GetTeam(string id)
        {
            return Ok(new { message = "This is a placeholder response from TeamsController." });
        }

        public IActionResult RegisterTeam([FromBody] Team newTeam)
        {
            // Place holder logic here
            return CreatedAtAction(nameof(GetTeam), new { id = newTeam.Name }, newTeam);
        }
    }
}
