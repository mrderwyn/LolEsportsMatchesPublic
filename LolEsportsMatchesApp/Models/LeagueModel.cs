using System.ComponentModel.DataAnnotations;

namespace LolEsportsMatchesApp.Models
{
    public class LeagueModel
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public string Slug { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        [DataType(DataType.ImageUrl)]
        [Required]
        public string Image { get; set; } = null!;
    }
}
