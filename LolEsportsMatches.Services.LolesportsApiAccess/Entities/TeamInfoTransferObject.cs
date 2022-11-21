namespace LolEsportsMatches.Services.LolesportsApiAccess.Entities
{
    public class TeamInfoTransferObject
    {
        public string Id { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string Image { get; set; } = null!;

        public string HomeLeague { get; set; } = null!;

        public string Region { get; set; } = null!;

        public List<PlayerInfoTransferObject> Players { get; set; } = null!;
    }
}
