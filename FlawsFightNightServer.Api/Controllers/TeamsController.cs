using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlawsFightNightServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetTeams(string tournamentId)
        {

        }
    }
}
