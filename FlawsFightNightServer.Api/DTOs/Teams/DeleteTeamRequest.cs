namespace FlawsFightNightServer.Api.DTOs.Teams
{
    public class DeleteTeamRequest
    {
        public string TeamId { get; set; }
        public string TournamentId { get; set; }
        public ulong GuildId { get; set; }
    }
}
