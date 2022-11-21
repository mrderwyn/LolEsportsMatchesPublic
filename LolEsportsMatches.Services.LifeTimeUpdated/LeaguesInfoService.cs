using LolEsportsMatches.DataAccess;
using LolEsportsMatches.DataAccess.Leagues;
using LolEsportsMatches.Services.Leagues;

namespace LolEsportsMatches.Services.LifeTimeUpdated
{
    /// <summary>
    /// Provides access to the League of Legends leagues information stored in the <see cref="LolEsportsMatchesDataAccessFactory"/> repository
    /// </summary>
    /// <seealso cref="LolEsportsMatches.Services.Leagues.ILeaguesInfoService" />
    public class LeaguesInfoService : ILeaguesInfoService
    {
        private readonly LolEsportsMatchesDataAccessFactory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="LeaguesInfoService"/> class.
        /// </summary>
        /// <param name="factory">The data access factory.</param>
        public LeaguesInfoService(LolEsportsMatchesDataAccessFactory factory)
        {
            this.factory = factory;
        }

        /// <summary>
        /// Shows the leagues.
        /// </summary>
        /// <returns>
        ///   <see cref="T:System.Collections.Generic.IEnumerable`1" /> with all leagues.
        /// </returns>
        public async Task<IEnumerable<LeagueInfo>> ShowLeagues()
        {
            ILeagueDataAccessObject dao = this.factory.GetLeagueDataAccessObject();
            return (await dao.SelectLeagues()).Select(dto => ObjectsMapper.MapLeague(dto));
        }

        /// <summary>
        /// Updates the league.
        /// </summary>
        /// <param name="league">The league.</param>
        /// <returns>
        /// true if the league has been updated, otherwise - false.
        /// </returns>
        public async Task<bool> UpdateLeague(LeagueInfo league)
        {
            try
            {
                ILeagueDataAccessObject dao = this.factory.GetLeagueDataAccessObject();
                await dao.UpdateLeague(ObjectsMapper.MapLeague(league));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Creates the league.
        /// </summary>
        /// <param name="league">The league.</param>
        /// <returns>
        /// true if the league has been created, otherwise - false.
        /// </returns>
        public async Task<bool> CreateLeague(LeagueInfo league)
        {
            ILeagueDataAccessObject dao = this.factory.GetLeagueDataAccessObject();
            return await dao.InsertLeague(ObjectsMapper.MapLeague(league));
        }

        /// <summary>
        /// Deletes the league.
        /// </summary>
        /// <param name="leagueId">The league identifier.</param>
        /// <returns>
        /// true if the league has been deleted, otherwise - false.
        /// </returns>
        public async Task<bool> DeleteLeague(string leagueId)
        {
            ILeagueDataAccessObject dao = this.factory.GetLeagueDataAccessObject();
            return await dao.RemoveLeague(leagueId);
        }

        /// <summary>
        /// Gets the league.
        /// </summary>
        /// <param name="key">The league key - can be either id or slug name of the league.</param>
        /// <returns>
        ///   <see cref="T:LolEsportsMatches.Services.Leagues.LeagueInfo" /> if the specified league exists, otherwise - null.
        /// </returns>
        public async Task<LeagueInfo?> GetLeague(string key)
        {
            try
            {
                ILeagueDataAccessObject dao = this.factory.GetLeagueDataAccessObject();
                return ObjectsMapper.MapLeague(await dao.FindLeague(key));
            }
            catch (LeagueNotFoundException)
            {
                return null;
            }
        }
    }
}
