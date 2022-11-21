using LolEsportsMatches.DataAccess.EntityFrameworkCore.Context;
using LolEsportsMatches.DataAccess.Leagues;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LolEsportsMatches.DataAccess.EntityFrameworkCore
{
    /// <summary>
    /// Provide access to stored leagues using EF Core (<see cref="MatchHistoryContext"/> context).
    /// </summary>
    /// <seealso cref="LolEsportsMatches.DataAccess.Leagues.ILeagueDataAccessObject" />
    public class LeagueEntityFrameworkDataAccessObject : ILeagueDataAccessObject
    {
        private readonly MatchHistoryContext context;
        private readonly ILogger<LeagueEntityFrameworkDataAccessObject>? logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LeagueEntityFrameworkDataAccessObject"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        public LeagueEntityFrameworkDataAccessObject(MatchHistoryContext context, ILogger<LeagueEntityFrameworkDataAccessObject>? logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger;
        }

        /// <summary>
        /// Finds the league.
        /// </summary>
        /// <param name="key">The key - can be either id or slug name of the league.</param>
        /// <returns>
        ///   <see cref="T:LolEsportsMatches.DataAccess.Leagues.LeagueTransferObject" /> of found league or throw <see cref="LeagueNotFoundException"/> exception, if league is not found.
        /// </returns>
        /// <exception cref="LolEsportsMatches.DataAccess.Leagues.LeagueNotFoundException">if league is not found.</exception>
        public async Task<LeagueTransferObject> FindLeague(string key)
        {
            Entities.LeagueEntity? entity = await this.context.Leagues.FirstOrDefaultAsync(e => e.Id == key || e.Slug == key);
            if (entity is null)
            {
                throw new LeagueNotFoundException(key);
            }

            return ObjectsMapper.MapLeague(entity);
        }

        /// <summary>
        /// Returns ids of all not stored matches for the specified league.
        /// </summary>
        /// <param name="leagueId">The league identifier.</param>
        /// <param name="matchesIds">The matches ids.</param>
        /// <returns>
        ///   <see cref="IEnumerable{string}" />ids of all not stored matches.
        /// </returns>
        /// <exception cref="LolEsportsMatches.DataAccess.Leagues.LeagueNotFoundException">if league with specified identifier is not found.</exception>
        public async Task<IEnumerable<string>> NotStored(string leagueId, IEnumerable<string>? matchesIds)
        {
            Entities.LeagueEntity? entity = await this.context.Leagues.FirstOrDefaultAsync(e => e.Id == leagueId);
            if (entity is null)
            {
                throw new LeagueNotFoundException(leagueId);
            }

            if (entity.LastStoredMatchId is null)
            {
                return matchesIds ?? Array.Empty<string>();
            }

            if (matchesIds is null || matchesIds.LastOrDefault() == entity.LastStoredMatchId)
            {
                return Array.Empty<string>();
            }

            IEnumerable<string>? afterLoaded = matchesIds.SkipWhile(id => id != entity.LastStoredMatchId);
            if (afterLoaded.Count() > 1)
            {
                return afterLoaded.Skip(1);
            }

            return matchesIds;
        }

        /// <summary>
        /// Inserts the league.
        /// </summary>
        /// <param name="league">The league.</param>
        /// <returns>
        /// true if the league has been inserted, otherwise - false.
        /// </returns>
        public async Task<bool> InsertLeague(LeagueTransferObject league)
        {
            try
            {
                await this.context.Leagues.AddAsync(ObjectsMapper.MapLeague(league));
                await this.context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Removes the league.
        /// </summary>
        /// <param name="leagueId">The league identifier.</param>
        /// <returns>
        /// true if the league has been removed, otherwise - false.
        /// </returns>
        public async Task<bool> RemoveLeague(string leagueId)
        {
            try
            {
                Entities.LeagueEntity? entity = await this.context.Leagues.FindAsync(leagueId);
                if (entity is null)
                {
                    return false;
                }

                this.context.Leagues.Remove(entity);
                await this.context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Selects all leagues.
        /// </summary>
        /// <returns>
        ///   <see cref="IEnumerable{LeagueTransferObject}" /> query with all stored leagues.
        /// </returns>
        public async Task<IEnumerable<LeagueTransferObject>> SelectLeagues()
        {
            IEnumerable<Entities.LeagueEntity>? leagues = this.context.Leagues.AsEnumerable();
            return await Task.Run(() => leagues.Select(e => ObjectsMapper.MapLeague(e)));
        }

        /// <summary>
        /// Updates the league.
        /// </summary>
        /// <remarks>if the league cannot be updated - a critical error is logged</remarks>
        /// <param name="league">The league.</param>
        /// <returns>
        /// true if the league has been updated, otherwise - false.
        /// </returns>
        /// <exception cref="LolEsportsMatches.DataAccess.Leagues.LeagueNotFoundException">if league is not found.</exception>
        public async Task<bool> UpdateLeague(LeagueTransferObject league)
        {
            try
            {
                Entities.LeagueEntity? entity = await this.context.Leagues.FindAsync(league.Id);
                if (entity is null)
                {
                    throw new LeagueNotFoundException(league.Id);
                }

                entity.Slug = league.Slug;
                entity.Name = league.Name;
                entity.Image = league.Image;

                if (league.LastLoadedMatchId is not null)
                {
                    entity.LastStoredMatchId = league.LastLoadedMatchId;
                }

                await this.context.SaveChangesAsync();
                return true;
            }
            catch (LeagueNotFoundException)
            {
                throw;
            }
            catch
            {
                this.logger?.LogCritical("League {leagueId}! Last match (#{matchId}) info wasnt updated. Fix this!!", league.Id, league.LastLoadedMatchId);
                return false;
            }
        }
    }
}
