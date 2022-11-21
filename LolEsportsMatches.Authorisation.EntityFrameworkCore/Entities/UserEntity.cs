using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LolEsportsMatches.Authorisation.EntityFrameworkCore.Entities
{
    public class UserEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("email")]
        public string Email { get; set; } = null!;

        [Column("password")]
        public string Password { get; set; } = null!;

        [Column("role")]
        public string Role { get; set; } = null!;
    }
}
