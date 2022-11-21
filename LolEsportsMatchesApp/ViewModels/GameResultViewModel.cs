using LolEsportsMatchesApp.Models;

namespace LolEsportsMatchesApp.ViewModels
{
    public class GameResultViewModel : GameResultModel
    {
        public string LeagueName { get; set; } = null!;

        public TeamModel BlueTeam { get; set; } = null!;

        public TeamModel RedTeam { get; set; } = null!;

        public string LeagueSlug { get; set; } = null!;

        public string[] ChampionsBlueImage { get; set; } = null!;

        public string[] ChampionsRedImage { get; set; } = null!;
    }
}
