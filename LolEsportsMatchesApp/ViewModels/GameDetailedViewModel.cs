using LolEsportsMatchesApp.Models;

namespace LolEsportsMatchesApp.ViewModels
{
    public class GameDetailedViewModel : GameResultViewModel
    {
        public ChampionIngameStatModel[] ChampsBlueStat { get; set; } = null!;

        public ChampionIngameStatModel[] ChampsRedStat { get; set; } = null!;
    }
}
