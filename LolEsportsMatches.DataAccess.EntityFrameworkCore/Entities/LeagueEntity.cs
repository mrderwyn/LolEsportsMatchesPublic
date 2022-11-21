using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LolEsportsMatches.DataAccess.EntityFrameworkCore.Entities
{
    public class LeagueEntity
    {
        [Key]
        [Column("league_id")]
        public string Id { get; set; } = null!;

        [Column("slug")]
        public string Slug { get; set; } = null!;

        [Column("name")]
        public string Name { get; set; } = null!;

        [Column("image")]
        public string Image { get; set; } = null!;

        [Column("last_stored_match_id")]
        public string? LastStoredMatchId { get; set; }
    }
}
