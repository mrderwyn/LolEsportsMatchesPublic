using LolEsportsMatches.DataAccess;
using LolEsportsMatches.DataAccess.Games;
using LolEsportsMatches.DataAccess.Leagues;
using LolEsportsMatches.Services.GameResults;
using LolEsportsMatches.Services.LolesportsApiAccess;
using LolEsportsMatches.Services.Teams;
using Microsoft.Extensions.Logging;
using PagedEnumerables;

namespace LolEsportsMatches.Services.LifeTimeUpdated
{
    /// <summary>
    /// Provides access to the League of Legends game results stored in the <see cref="LolEsportsMatchesDataAccessFactory"/> repository,
    /// which are constantly updated from the <see cref="LolEsportsApiAccess"/> Life LoL games API.
    /// </summary>
    /// <seealso cref="LolEsportsMatches.Services.GameResults.IGameResultsService" />
    public class GamesResultService : IGameResultsService
    {
        private readonly LolEsportsMatchesDataAccessFactory factory;
        private readonly ILogger<GamesResultService>? logger;
        private readonly LolEsportsApiAccess apiAccess;
        private readonly ITeamInfoService teamService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GamesResultService"/> class.
        /// </summary>
        /// <param name="factory">The data access factory.</param>
        /// <param name="teamService">The team service.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="api">The League of Legends Life Game API.</param>
        public GamesResultService(LolEsportsMatchesDataAccessFactory factory, ITeamInfoService teamService, ILogger<GamesResultService> logger, LolEsportsApiAccess api)
        {
            this.factory = factory;
            this.logger = logger;
            this.apiAccess = api;
            this.teamService = teamService;
        }

        /// <summary>
        /// Shows the games based on filters.
        /// </summary>
        /// <remarks>Once called, the service checks for new game results and saves them.
        /// Thus, on subsequent calls, data about the latest games will be provided from the repository,
        /// and not downloaded from third-party resources.</remarks>
        /// <param name="leagueKey">The league key - can be either id or slug name of the league.</param>
        /// <param name="teamKey">The team key - can be either id or slug name of the team.</param>
        /// <param name="champId">The champ identifier.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns>
        /// <see cref="T:PagedEnumerables.IPagedEnumerable`1" /> with found games, or Null - if
        /// games cannot be found due to wrong filters keys.
        /// </returns>
        public async Task<IPagedEnumerable<GameResult>?> ShowGames(
            string? leagueKey,
            string? teamKey,
            string? champId,
            int offset,
            int count)
        {
            this.logger?.LogInformation("Loading games with #{leagueId} league, #{teamId} team, {champId} champ fitlers",
                leagueKey, teamKey, champId);

            if (!string.IsNullOrWhiteSpace(leagueKey))
            {
                ILeagueDataAccessObject leagueDao = this.factory.GetLeagueDataAccessObject();
                try
                {
                    LeagueTransferObject? league = await leagueDao.FindLeague(leagueKey);
                    IEnumerable<string>? completedMatches = await this.apiAccess.GetAllCompletedMatchesIdByLeagueId(league.Id);
                    IEnumerable<string>? toUpdate = await leagueDao.NotStored(league.Id, completedMatches);
                    await UpdateGames(league.Id, toUpdate);
                }
                catch (LeagueNotFoundException)
                {
                    return null;
                }
            }

            IGameDataAccessObject gameDao = this.factory.GetGameDataAccessObject();
            IQueryable<GameTransferObject> games = await gameDao.SelectGamesWithFilter(leagueKey, teamKey, champId);
            return new PagedQueryable<GameResult>(games.Select(dto => ObjectsMapper.MapGame(dto)), offset, count);
        }

        /// <summary>
        /// Updates the game.
        /// </summary>
        /// <param name="game">The game to update.</param>
        /// <returns>
        /// true if the game has been updated, otherwise - false.
        /// </returns>
        public async Task<bool> UpdateGame(GameResult game)
        {
            try
            {
                IGameDataAccessObject dao = this.factory.GetGameDataAccessObject();
                await dao.UpdateGame(ObjectsMapper.MapGame(game));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Creates the game.
        /// </summary>
        /// <param name="game">The game to create.</param>
        /// <returns>
        /// true if the game has been created, otherwise - false.
        /// </returns>
        public async Task<bool> CreateGame(GameResult game)
        {
            IGameDataAccessObject dao = this.factory.GetGameDataAccessObject();
            return (await dao.InsertGames(new[] { ObjectsMapper.MapGame(game) })) == 1;
        }

        /// <summary>
        /// Deletes the game.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <returns>
        /// true if the game has been deleted, otherwise - false.
        /// </returns>
        public async Task<bool> DeleteGame(string gameId)
        {
            IGameDataAccessObject dao = this.factory.GetGameDataAccessObject();
            return await dao.RemoveGame(gameId);
        }

        /// <summary>
        /// Gets the specified game.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <returns>
        ///   <see cref="T:LolEsportsMatches.Services.GameResults.GameResult" /> if the specified game exists, otherwise - null.
        /// </returns>
        public async Task<GameResult?> GetGame(string gameId)
        {
            IGameDataAccessObject dao = this.factory.GetGameDataAccessObject();
            try
            {
                return ObjectsMapper.MapGame(await dao.FindGame(gameId));
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the game details.
        /// </summary>
        /// <remarks>data is combined from the game results from the repository
        /// and the results of each player, loaded from third-party services.</remarks>
        /// <param name="gameId">The game identifier.</param>
        /// <returns>
        ///   <see cref="T:LolEsportsMatches.Services.GameResults.GameDetailedResult" /> if the specified game exists, otherwise - null.
        /// </returns>
        public async Task<GameDetailedResult?> GetGameDetails(string gameId)
        {
            GameResult? gameMain = await this.GetGame(gameId);
            LolesportsApiAccess.Entities.GameDetailedStatTransferObject? gameDetails = await this.apiAccess.GetGameDetailedInfoByGameId(gameId);
            if (gameMain is null)
            {
                return null;
            }

            return ObjectsMapper.MapGame(gameMain, gameDetails);
        }

        private async Task UpdateGames(string leagueId, IEnumerable<string>? matchesIds)
        {
            if (matchesIds is null || !matchesIds.Any())
            {
                return;
            }

            int loadedMatchesCount = 0;
            List<GameTransferObject> gamesToInsert = new();

            foreach (string? matchId in matchesIds)
            {
                loadedMatchesCount++;
                IEnumerable<string>? gamesIds = await this.apiAccess.GetAllCompletedGamesIdByMatchId(matchId);
                if (gamesIds is null)
                {
                    this.logger?.LogError("Match #{id} not loaded.", matchId);
                    continue;
                }

                foreach (string? gameId in gamesIds)
                {
                    LolesportsApiAccess.Entities.GameResultTransferObject? gameLifeResult = await this.apiAccess.GetGameResultTransferObjectByGameId(gameId);
                    if (gameLifeResult is null)
                    {
                        this.logger?.LogError("Game #{id} (match id #{matchId}) in {leagueId} league wasn't parsed. Please parse again.", gameId, matchId, leagueId);
                    }
                    else
                    {
                        if ((await this.teamService.GetTeam(gameLifeResult.TeamBlueId)) is null || (await this.teamService.GetTeam(gameLifeResult.TeamRedId)) is null)
                        {
                            this.logger?.LogError("Game #{id} (match id #{matchId} in {leagueId} cannot be inserted,"
                                + " one of teams id (#{blueId} or #{redId}) doesnt exist in database. Please input unexisted"
                                + "team manually, before adding game.", gameId, matchId, gameLifeResult.LeagueId, gameLifeResult.TeamBlueId, gameLifeResult.TeamRedId);
                        }
                        else
                        {
                            gamesToInsert.Add(ObjectsMapper.MapGame(gameLifeResult, leagueId));
                        }
                    }
                }
            }

            IGameDataAccessObject gameDao = this.factory.GetGameDataAccessObject();
            int inserted = await gameDao.InsertGames(gamesToInsert);
            this.logger?.LogInformation("Inserted {count} games.", inserted);

            string? lastMatch = matchesIds.LastOrDefault();
            if (string.IsNullOrWhiteSpace(lastMatch))
            {
                this.logger?.LogCritical("League #{leagueId} ! Games information was updated, but last match info wasnt. Fix this!!", leagueId);
                return;
            }

            ILeagueDataAccessObject leagueDao = this.factory.GetLeagueDataAccessObject();
            LeagueTransferObject? league = await leagueDao.FindLeague(leagueId);
            string? prevLastLoadedMatchId = league.LastLoadedMatchId;
            league.LastLoadedMatchId = lastMatch;
            await leagueDao.UpdateLeague(league);
            this.logger?.LogInformation("League #{leagueId} was updated with lastmatch #{lastMatch} (was #{last})", leagueId, lastMatch, prevLastLoadedMatchId);
        }
    }
}
