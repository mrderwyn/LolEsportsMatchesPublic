using System.Globalization;
using System.Runtime.Serialization;

namespace LolEsportsMatches.DataAccess.Teams
{
    [Serializable]
    public class TeamNotFoundException : Exception
    {
        public TeamNotFoundException(string id)
            : base(string.Format(CultureInfo.InvariantCulture, $"A team with identifier = {id}."))
        {
            this.TeamId = id;
        }

        protected TeamNotFoundException(SerializationInfo info, StreamingContext context)
              : base(info, context)
        {
        }

        public string TeamId { get; } = string.Empty;
    }
}
