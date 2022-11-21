namespace LolEsportsMatches.Services.LolesportsApiAccess.Entities
{
    internal class GamePlayerBuildsEntity
    {
        public List<BuildsFrame> Frames { get; set; } = null!;
    }

    internal class BuildsFrame
    {
        public List<ParticipantEntity> Participants { get; set; } = null!;
    }

    internal class ParticipantEntity
    {
        public int Level { get; set; }

        public int Kills { get; set; }

        public int Deaths { get; set; }

        public int Assists { get; set; }

        public int TotalGoldEarned { get; set; }

        public List<int> Items { get; set; } = null!;

        public PerkMetadataEntity PerkMetadata { get; set; } = null!;

        public List<char> Abilities { get; set; } = null!;
    }

    internal class PerkMetadataEntity
    {
        public int StyleId { get; set; }

        public int SubStyleId { get; set; }

        public List<int> Perks { get; set; } = null!;
    }
}
