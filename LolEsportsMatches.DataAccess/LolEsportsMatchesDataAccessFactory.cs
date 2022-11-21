using LolEsportsMatches.DataAccess.Games;
using LolEsportsMatches.DataAccess.Leagues;
using LolEsportsMatches.DataAccess.Teams;

namespace LolEsportsMatches.DataAccess
{
    /// <summary>
    /// Abstract factory for <see cref="IGameDataAccessObject"/>, <see cref="ILeagueDataAccessObject"/> 
    /// and <see cref="ITeamDataAccessObject"/>.
    /// </summary>
    public abstract class LolEsportsMatchesDataAccessFactory
    {
        /// <summary>
        /// Gets the data access object for games.
        /// </summary>
        /// <returns><see cref="IGameDataAccessObject"/> games data access object.</returns>
        public abstract IGameDataAccessObject GetGameDataAccessObject();

        /// <summary>
        /// Gets the data access object for leagues.
        /// </summary>
        /// <returns><see cref="ILeagueDataAccessObject"/> leagues data access object.</returns>
        public abstract ILeagueDataAccessObject GetLeagueDataAccessObject();

        /// <summary>
        /// Gets the data access object for teams.
        /// </summary>
        /// <returns><see cref="ITeamDataAccessObject"/> teams data access object.</returns>
        public abstract ITeamDataAccessObject GetTeamDataAccessObject();
    }
}