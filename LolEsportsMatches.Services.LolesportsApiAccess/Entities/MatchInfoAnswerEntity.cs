namespace LolEsportsMatches.Services.LolesportsApiAccess.Entities
{
    internal class MatchInfoAnswerEntity
    {
        public MatchDataEntity Data { get; set; } = null!;
    }

    internal class MatchDataEntity
    {
        public MatchInfoEventEntity Event { get; set; } = null!;
    }

    internal class MatchInfoEventEntity
    {
        public MatchInfoMatchEntity Match { get; set; } = null!;
    }

    internal class MatchInfoMatchEntity
    {
        public List<MatchInfoTeamEntity> Teams { get; set; } = null!;

        public List<MatchInfoGameEntity> Games { get; set; } = null!;
    }

    internal class MatchInfoTeamEntity
    {
        public string Id { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string Image { get; set; } = null!;
    }

    internal class MatchInfoGameEntity
    {
        public int Number { get; set; }

        public string Id { get; set; } = null!;

        public string State { get; set; } = null!;

        public List<MatchInfoGameTeamEntity> Teams { get; set; } = null!;
    }

    internal class MatchInfoGameTeamEntity
    {
        public string Id { get; set; } = null!;

        public string Side { get; set; } = null!;
    }
}
