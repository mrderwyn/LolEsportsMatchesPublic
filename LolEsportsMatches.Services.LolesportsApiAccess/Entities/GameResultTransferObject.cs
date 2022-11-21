namespace LolEsportsMatches.Services.LolesportsApiAccess.Entities
{
    public class GameResultTransferObject
    {
        public string Id { get; set; } = null!;

        public string LeagueId { get; set; } = null!;

        public string TeamBlueId { get; set; } = null!;

        public string TeamRedId { get; set; } = null!;

        public short TeamBlueKills { get; set; }

        public short TeamRedKills { get; set; }

        public string[] TeamBlueChampions { get; set; } = null!;

        public string[] TeamRedChampions { get; set; } = null!;

        public DateTime GameDate { get; set; }
    }
}
