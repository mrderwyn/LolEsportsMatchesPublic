namespace LolEsportsMatches.Services.GameResults
{
    public class GameDetailedResult : GameResult
    {
        public ChampionIngameStat[] ChampionsBlueStat { get; set; } = null!;

        public ChampionIngameStat[] ChampionsRedStat { get; set; } = null!;
    }
}
