using System.ComponentModel.DataAnnotations;

namespace LolEsportsMatchesApp.Models
{
    public class GameResultModel
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public string LeagueId { get; set; } = null!;

        [Required]
        public string TeamBlueId { get; set; } = null!;

        [Required]
        public string TeamRedId { get; set; } = null!;

        [Required]
        public string[] ChampionsBlue { get; set; } = new string[5];

        [Required]
        public string[] ChampionsRed { get; set; } = new string[5];

        [Required]
        public int KillsBlue { get; set; }

        [Required]
        public int KillsRed { get; set; }

        [DataType(DataType.DateTime), Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yy HH:mm}")]
        public DateTime GameDate { get; set; }
    }
}
