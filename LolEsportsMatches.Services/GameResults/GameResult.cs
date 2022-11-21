namespace LolEsportsMatches.Services.GameResults
{
    public class GameResult
    {
        public string Id { get; set; } = null!;

        public Leagues.LeagueInfo League { get; set; } = null!;

        public Teams.TeamInfo BlueTeam { get; set; } = null!;

        public Teams.TeamInfo RedTeam { get; set; } = null!;

        public string[] ChampionsBlue { get; set; } = null!;

        public string[] ChampionsRed { get; set; } = null!;

        public int KillsBlue { get; set; }

        public int KillsRed { get; set; }

        public DateTime GameDate { get; set; }
    }
}