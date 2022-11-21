namespace LolEsportsMatches.DataAccess.Leagues
{
    public class LeagueTransferObject
    {
        public string Id { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Image { get; set; } = null!;

        public string? LastLoadedMatchId { get; set; }
    }
}
