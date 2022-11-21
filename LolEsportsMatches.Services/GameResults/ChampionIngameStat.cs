namespace LolEsportsMatches.Services.GameResults
{
    public class ChampionIngameStat
    {
        public int Level { get; set; }

        public int Kills { get; set; }

        public int Deaths { get; set; }

        public int Assists { get; set; }

        public int Gold { get; set; }

        public List<int> ItemsId { get; set; } = new();

        public int FirstMainPerkId { get; set; }

        public int SubPerkId { get; set; }

        public List<char> Abilities { get; set; } = new();
    }
}
