namespace LolEsportsMatches.Services.LolesportsApiAccess.Entities
{
    public class PlayerInfoTransferObject
    {
        public string SummonerName { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Image { get; set; } = null!;

        public string Role { get; set; } = null!;
    }
}
