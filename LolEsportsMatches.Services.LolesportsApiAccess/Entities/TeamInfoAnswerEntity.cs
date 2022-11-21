namespace LolEsportsMatches.Services.LolesportsApiAccess.Entities
{
    internal class TeamInfoAnswerEntity
    {
        public TeamInfoDataEntity Data { get; set; } = null!;
    }

    internal class TeamInfoDataEntity
    {
        public List<FullInfoEntity> Teams { get; set; } = null!;
    }

    internal class FullInfoEntity
    {
        public string Id { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string Image { get; set; } = null!;

        public HomeLeagueEntity HomeLeague { get; set; } = null!;

        public List<TeamPlayerEntity> Players { get; set; } = null!;
    }

    internal class HomeLeagueEntity
    {
        public string Name { get; set; } = null!;

        public string Region { get; set; } = null!;
    }

    internal class TeamPlayerEntity
    {
        public string SummonerName { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Image { get; set; } = null!;

        public string Role { get; set; } = null!;
    }
}
