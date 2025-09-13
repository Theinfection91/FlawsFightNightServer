namespace FlawsFightNightServer.Api.DTOs.Tournaments
{
    public class UnlockTeamsRequest
    {
        public string TournamentId { get; set; }
        public ulong GuildId { get; set; }
    }
}
