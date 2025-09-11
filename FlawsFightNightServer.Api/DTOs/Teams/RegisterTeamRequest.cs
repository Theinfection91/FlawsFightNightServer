namespace FlawsFightNightServer.Api.DTOs.Teams
{
    // TODO Create DTOs (Data Transfer Objects) for commands
    public class RegisterTeamRequest
    {
        public string TournamentId { get; set; }
        public string TeamName { get; set; }
        public Dictionary<ulong, string>? Members { get; set; } // Key: Discord User ID, Value: In-game Name
    }
}
