using System.Globalization;
using System.Runtime.Serialization;

namespace LolEsportsMatches.DataAccess.Games
{
    [Serializable]
    public class GameNotFoundException : Exception
    {
        public GameNotFoundException(string id)
            : base(string.Format(CultureInfo.InvariantCulture, $"A game with identifier = {id}."))
        {
            this.GameId = id;
        }

        protected GameNotFoundException(SerializationInfo info, StreamingContext context)
              : base(info, context)
        {
        }

        public string GameId { get; } = string.Empty;
    }
}
