using LolData.Services.Champions;
using LolEsportsMatches.DataAccess.EntityFrameworkCore.Context;
using LolEsportsMatches.DataAccess.Games;
using LolEsportsMatches.DataAccess.Leagues;
using LolEsportsMatches.DataAccess.Teams;
using Microsoft.Extensions.Logging;

namespace LolEsportsMatches.DataAccess.EntityFrameworkCore
{
    /// <summary>
    /// Factory for <see cref="GameEntityFrameworkDataAccessObject"/>, <see cref="TeamEntityFrameworkDataAccessObject"/> and <see cref="LeagueEntityFrameworkDataAccessObject"/> objects.
    /// Use Entity Framework (<see cref="MatchHistoryContext"/> context).
    /// </summary>
    /// <seealso cref="LolEsportsMatches.DataAccess.LolEsportsMatchesDataAccessFactory" />
    public class EntityFrameworkDataAccessFactory : LolEsportsMatchesDataAccessFactory
    {
        private readonly MatchHistoryContext context;
        private readonly ILogger<LeagueEntityFrameworkDataAccessObject>? leagueLogger;
        private readonly ILogger<TeamEntityFrameworkDataAccessObject>? teamLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkDataAccessFactory"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="champService">The champ service to resolve the unique identifier of champions - cannot be null.</param>
        /// <param name="leagueLogger">The league logger.</param>
        /// <param name="teamLogger">The team logger.</param>
        /// <exception cref="System.ArgumentNullException">
        /// context
        /// or
        /// champService
        /// </exception>
        public EntityFrameworkDataAccessFactory(
            MatchHistoryContext context,
            ILolDataChampionsService champService,
            ILogger<LeagueEntityFrameworkDataAccessObject>? leagueLogger,
            ILogger<TeamEntityFrameworkDataAccessObject>? teamLogger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.leagueLogger = leagueLogger;
            this.teamLogger = teamLogger;
            ObjectsMapper.champService = champService ?? throw new ArgumentNullException(nameof(champService));
        }

        /// <summary>
        /// Gets the data access object for games.
        /// </summary>
        /// <returns>
        ///   <see cref="T:LolEsportsMatches.DataAccess.Games.IGameDataAccessObject" /> games data access object.
        /// </returns>
        public override IGameDataAccessObject GetGameDataAccessObject()
        {
            return new GameEntityFrameworkDataAccessObject(this.context);
        }

        /// <summary>
        /// Gets the data access object for leagues.
        /// </summary>
        /// <returns>
        ///   <see cref="T:LolEsportsMatches.DataAccess.Leagues.ILeagueDataAccessObject" /> leagues data access object.
        /// </returns>
        public override ILeagueDataAccessObject GetLeagueDataAccessObject()
        {
            return new LeagueEntityFrameworkDataAccessObject(this.context, this.leagueLogger);
        }

        /// <summary>
        /// Gets the data access object for teams.
        /// </summary>
        /// <returns>
        ///   <see cref="T:LolEsportsMatches.DataAccess.Teams.ITeamDataAccessObject" /> teams data access object.
        /// </returns>
        public override ITeamDataAccessObject GetTeamDataAccessObject()
        {
            return new TeamEntityFrameworkDataAccessObject(this.context, this.teamLogger);
        }
    }
}
