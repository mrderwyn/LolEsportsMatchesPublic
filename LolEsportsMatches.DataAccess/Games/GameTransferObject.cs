using LolEsportsMatches.DataAccess.Leagues;
using LolEsportsMatches.DataAccess.Teams;

namespace LolEsportsMatches.DataAccess.Games
{
    public class GameTransferObject
    {
        public string Id { get; set; } = null!;

        public LeagueTransferObject League { get; set; } = null!;

        public TeamTransferObject TeamBlue { get; set; } = null!;

        public TeamTransferObject TeamRed { get; set; } = null!;

        public string[] ChampionsBlue { get; set; } = null!;

        public string[] ChampionsRed { get; set; } = null!;

        public short KillsBlue { get; set; }

        public short KillsRed { get; set; }

        public DateTime GameDate { get; set; }
    }
}
