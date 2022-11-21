using LolEsportsMatches.DataAccess.EntityFrameworkCore.Context;
using LolEsportsMatches.DataAccess.EntityFrameworkCore.Entities;
using LolEsportsMatches.DataAccess.Games;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LolEsportsMatches.DataAccess.EntityFrameworkCore
{
    /// <summary>
    /// Provide access to stored games using EF Core (<see cref="MatchHistoryContext"/> context).
    /// </summary>
    /// <seealso cref="LolEsportsMatches.DataAccess.Games.IGameDataAccessObject" />
    public class GameEntityFrameworkDataAccessObject : IGameDataAccessObject
    {
        private readonly MatchHistoryContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameEntityFrameworkDataAccessObject"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        public GameEntityFrameworkDataAccessObject(MatchHistoryContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Finds the game.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <returns>
        ///   <see cref="T:LolEsportsMatches.DataAccess.Games.GameTransferObject" /> of found game or throw <see cref="GameNotFoundException"/> exception, if game is not found.
        /// </returns>
        /// <exception cref="LolEsportsMatches.DataAccess.Games.GameNotFoundException">if game is not found.</exception>
        public async Task<GameTransferObject> FindGame(string gameId)
        {
            GameEntity? entity = await this.IncludedGames.SingleOrDefaultAsync(e => e.Id == gameId);
            if (entity is null)
            {
                throw new GameNotFoundException(gameId);
            }

            return ObjectsMapper.MapGame(entity);
        }

        /// <summary>
        /// Inserts the games.
        /// </summary>
        /// <param name="games">The games collection.</param>
        /// <returns>
        /// Number of inserded games.
        /// </returns>
        public async Task<int> InsertGames(IEnumerable<GameTransferObject> games)
        {
            try
            {
                int gamesCount = 0;

                foreach (GameTransferObject? game in games)
                {
                    GameEntity? entity = ObjectsMapper.MapGame(game);
                    await this.context.Games.AddAsync(entity);
                    gamesCount++;
                }

                await this.context.SaveChangesAsync();
                return gamesCount;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Removes the game.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <returns>
        /// true if the game has been removed, otherwise - false.
        /// </returns>
        public async Task<bool> RemoveGame(string gameId)
        {
            try
            {
                GameEntity? entity = await this.context.Games.FindAsync(gameId);
                if (entity is null)
                {
                    return false;
                }

                this.context.Games.Remove(entity);
                await this.context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Selects all games.
        /// </summary>
        /// <returns>
        ///   <see cref="IQueryable{GameTransferObject}" /> query with all stored games.
        /// </returns>
        public async Task<IQueryable<GameTransferObject>> SelectGames()
        {
            return await Task.Run(() => this.IncludedGames
                .OrderByDescending(e => e.GameDate)
                .Select(e => ObjectsMapper.MapGame(e)));
        }

        /// <summary>
        /// Selects the games with filter.
        /// </summary>
        /// <param name="leagueKey">The league key - can be either id or slug name of the league. If value is null or empty - no league filter is applied.</param>
        /// <param name="teamKey">The team key - can be either id or slug name of the team. If value is null or empty - no team filter is applied.</param>
        /// <param name="champName">Name of the champ. If value is null or empty - no champ filter is applied.</param>
        /// <returns>
        ///   <see cref="IQueryable{GameTransferObject}" /> query with all games matching the filter.
        /// </returns>
        public async Task<IQueryable<GameTransferObject>> SelectGamesWithFilter(string? leagueKey, string? teamKey, string? champName)
        {
            IQueryable<GameEntity>? query = this.IncludedGames;

            if (!string.IsNullOrWhiteSpace(leagueKey))
            {
                LeagueEntity? l = await this.context.Leagues.FirstOrDefaultAsync(e => e.Slug == leagueKey);
                if (l is not null)
                {
                    leagueKey = l.Id;
                }

                query = query.Where(e => e.LeagueId == leagueKey);
            }

            if (!string.IsNullOrWhiteSpace(teamKey))
            {
                TeamEntity? t = await this.context.Teams.FirstOrDefaultAsync(e => e.Slug == teamKey);
                if (t is not null)
                {
                    teamKey = t.Id;
                }

                query = query.Where(e => e.TeamBlueId == teamKey || e.TeamRedId == teamKey);
            }

            if (!string.IsNullOrWhiteSpace(champName))
            {
                short champId = ObjectsMapper.GetChampIndex(champName);
                query = WhereChamp(champId);
            }

            return
                query
                .OrderByDescending(e => e.GameDate)
                .Select(e => ObjectsMapper.MapGame(e));

            IQueryable<GameEntity> WhereChamp(short champId)
            {
                return query.Where(e =>
                        e.TeamBlueChampion1 == champId
                        || e.TeamBlueChampion2 == champId
                        || e.TeamBlueChampion3 == champId
                        || e.TeamBlueChampion4 == champId
                        || e.TeamBlueChampion5 == champId
                        || e.TeamRedChampion1 == champId
                        || e.TeamRedChampion2 == champId
                        || e.TeamRedChampion3 == champId
                        || e.TeamRedChampion4 == champId
                        || e.TeamRedChampion5 == champId);
            }
        }

        /// <summary>
        /// Updates the game.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <returns>
        /// true if the game has been updated, otherwise - false.
        /// </returns>
        public async Task<bool> UpdateGame(GameTransferObject game)
        {
            try
            {
                GameEntity? entity = ObjectsMapper.MapGame(game);
                this.context.Games.Update(entity);

                await this.context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private IQueryable<GameEntity> IncludedGames => this.context.Games
            .Include(g => g.League)
            .Include(g => g.TeamBlue)
            .Include(g => g.TeamRed);
    }
}
