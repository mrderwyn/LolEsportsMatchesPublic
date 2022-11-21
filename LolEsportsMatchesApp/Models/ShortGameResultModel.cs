using System.ComponentModel.DataAnnotations;

namespace LolEsportsMatchesApp.Models
{
    public class ShortGameResultModel
    {
        public string Id { get; set; } = null!;

        public TeamModel Opponent { get; set; } = null!;

        public string[] SelectedTeamChampionsImage { get; set; } = null!;

        public string[] SelectedTeamChampions { get; set; } = null!;

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yy HH:mm}")]
        public DateTime GameDate { get; set; }
    }
}
