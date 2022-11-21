namespace LolEsportsMatches.Services.Teams
{
    public class TeamDetailedInfo : TeamInfo
    {
        public string HomeLeague { get; set; } = null!;

        public List<PlayerInfo> Players { get; set; } = null!;
    }
}
