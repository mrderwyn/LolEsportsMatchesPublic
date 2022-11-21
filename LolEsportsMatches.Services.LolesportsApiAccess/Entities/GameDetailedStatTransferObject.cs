namespace LolEsportsMatches.Services.LolesportsApiAccess.Entities
{
    public class GameDetailedStatTransferObject
    {
        public List<ChampsStatTransferObject> ChampsDetails { get; set; } = null!;
    }

    public class ChampsStatTransferObject
    {
        public int Level { get; set; }

        public int Kills { get; set; }

        public int Deaths { get; set; }

        public int Assists { get; set; }

        public int TotalGoldEarned { get; set; }

        public List<int> Items { get; set; } = null!;

        public int FirstMainPerkId { get; set; }

        public int SubPerkId { get; set; }

        public List<char> Abilities { get; set; } = null!;
    }
}
