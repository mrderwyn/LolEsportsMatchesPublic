namespace LolEsportsMatches.DataAccess.Teams
{
    public class TeamTransferObject
    {
        public string Id { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string Image { get; set; } = null!;

        public string Region { get; set; } = null!;
    }
}
