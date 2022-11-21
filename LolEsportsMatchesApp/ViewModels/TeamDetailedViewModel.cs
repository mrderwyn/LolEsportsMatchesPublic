using LolEsportsMatchesApp.Models;

namespace LolEsportsMatchesApp.ViewModels
{
    public class TeamDetailedViewModel : TeamModel
    {
        public string HomeLeagueName { get; set; } = null!;

        public List<PlayerModel> Players { get; set; } = null!;

        public PagedModel<ShortGameResultModel> PagedGames { get; set; } = null!;
    }
}
