using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LolEsportsMatches.DataAccess.EntityFrameworkCore.Entities
{
    public class TeamEntity
    {
        [Key]
        [Column("team_id")]
        public string Id { get; set; } = null!;

        [Column("slug")]
        public string Slug { get; set; } = null!;

        [Column("name")]
        public string Name { get; set; } = null!;

        [Column("code")]
        public string Code { get; set; } = null!;

        [Column("image")]
        public string Image { get; set; } = null!;

        [Column("region")]
        public string Region { get; set; } = null!;
    }
}
