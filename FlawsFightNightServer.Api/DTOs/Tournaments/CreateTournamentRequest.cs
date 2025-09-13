using System.Text.Json.Serialization;

namespace FlawsFightNightServer.Api.DTOs.Tournaments
{
    public class CreateTournamentRequest
    {
        [JsonPropertyName("name")]
        public string TournamentName { get; set; }
        [JsonPropertyName("type")]
        public string TournamentType { get; set; }
        [JsonPropertyName("team_size")]
        public int TeamSize { get; set; }
        [JsonPropertyName("guild_id")]
        public ulong GuildId { get; set; }
    }
}
