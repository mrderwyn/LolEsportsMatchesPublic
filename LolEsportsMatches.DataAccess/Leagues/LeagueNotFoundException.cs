using System.Globalization;
using System.Runtime.Serialization;

namespace LolEsportsMatches.DataAccess.Leagues
{
    [Serializable]
    public class LeagueNotFoundException : Exception
    {
        public LeagueNotFoundException(string id)
            : base(string.Format(CultureInfo.InvariantCulture, $"A league with identifier = {id}."))
        {
            this.LeagueId = id;
        }

        protected LeagueNotFoundException(SerializationInfo info, StreamingContext context)
              : base(info, context)
        {
        }

        public string LeagueId { get; } = string.Empty;
    }
}
