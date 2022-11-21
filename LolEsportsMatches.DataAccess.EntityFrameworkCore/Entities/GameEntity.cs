using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LolEsportsMatches.DataAccess.EntityFrameworkCore.Entities
{
    public class GameEntity
    {
        [Key]
        [Column("game_id")]
        public string Id { get; set; } = null!;

        [Column("league_id")]
        public string LeagueId { get; set; } = null!;

        [Column("game_date")]
        public DateTime GameDate { get; set; }

        [Column("team_blue_id")]
        public string TeamBlueId { get; set; } = null!;

        [Column("team_red_id")]
        public string TeamRedId { get; set; } = null!;

        [Column("team_ blue_kills")]
        public short TeamBlueKills { get; set; }

        [Column("team_red_kills")]
        public short TeamRedKills { get; set; }

        [Column("team_red_champion_1")]
        public short TeamRedChampion1 { get; set; }
        [Column("team_red_champion_2")]
        public short TeamRedChampion2 { get; set; }
        [Column("team_red_champion_3")]
        public short TeamRedChampion3 { get; set; }
        [Column("team_red_champion_4")]
        public short TeamRedChampion4 { get; set; }
        [Column("team_red_champion_5")]
        public short TeamRedChampion5 { get; set; }
        [Column("team_blue_champion_1")]
        public short TeamBlueChampion1 { get; set; }
        [Column("team_blue_champion_2")]
        public short TeamBlueChampion2 { get; set; }
        [Column("team_blue_champion_3")]
        public short TeamBlueChampion3 { get; set; }
        [Column("team_blue_champion_4")]
        public short TeamBlueChampion4 { get; set; }
        [Column("team_blue_champion_5")]
        public short TeamBlueChampion5 { get; set; }

        [ForeignKey("LeagueId")]
        public LeagueEntity League { get; set; } = null!;

        [ForeignKey("TeamBlueId")]
        public TeamEntity TeamBlue { get; set; } = null!;

        [ForeignKey("TeamRedId")]
        public TeamEntity TeamRed { get; set; } = null!;
    }
}
