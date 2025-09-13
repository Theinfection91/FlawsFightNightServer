namespace FlawsFightNightServer.Api.DTOs.Matches
{
    public class ReportWinRequest
    {
        public string WinningTeamId { get; set; }
        public int WinnngTeamScore { get; set; }
        public int LosingTeamScore { get; set; }
        public ulong GuildId { get; set; }
    }
}
