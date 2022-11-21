namespace LolEsportsMatches.Services.LolesportsApiAccess.Entities
{
    internal class ScheduleAnswerEntity
    {
        public DataEntity Data { get; set; } = null!;
    }

    internal class DataEntity
    {
        public ScheduleEntity Schedule { get; set; } = null!;
    }

    internal class ScheduleEntity
    {
        public List<EventEntity> Events { get; set; } = null!;
    }

    internal class EventEntity
    {
        public string State { get; set; } = null!;

        public MatchEntity Match { get; set; } = null!;
    }

    internal class MatchEntity
    {
        public string Id { get; set; } = null!;
    }
}
