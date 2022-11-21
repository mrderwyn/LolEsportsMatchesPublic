namespace LolEsportsMatches.Services.LolesportsApiAccess.Entities
{
    internal class GameAnswerEntity
    {
        public GameMetadataEntity GameMetadata { get; set; } = null!;

        public List<FrameEntity> Frames { get; set; } = null!;
    }

    internal class GameMetadataEntity
    {
        public TeamMetadataEntity BlueTeamMetadata { get; set; } = null!;

        public TeamMetadataEntity RedTeamMetadata { get; set; } = null!;
    }

    internal class TeamMetadataEntity
    {
        public string EsportsTeamId { get; set; } = null!;

        public List<SummonerMetadataEntity> ParticipantMetadata { get; set; } = null!;
    }

    internal class SummonerMetadataEntity
    {
        public string ChampionId { get; set; } = null!;
    }

    internal class FrameEntity
    {
        public string Rfc460Timestamp { get; set; } = null!;

        public TeamResultEntity BlueTeam { get; set; } = null!;

        public TeamResultEntity RedTeam { get; set; } = null!;
    }

    internal class TeamResultEntity
    {
        public int TotalKills { get; set; }
    }
}
