namespace LolEsportsMatches.DataAccess.Games
{
    /// <summary>
    /// Provides access to stored games.
    /// </summary>
    public interface IGameDataAccessObject
    {
        /// <summary>
        /// Inserts the games.
        /// </summary>
        /// <param name="games">The games collection.</param>
        /// <returns>Number of inserded games.</returns>
        Task<int> InsertGames(IEnumerable<GameTransferObject> games);

        /// <summary>
        /// Finds the game.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <returns><see cref="GameTransferObject"/> of found game.</returns>
        Task<GameTransferObject> FindGame(string gameId);

        /// <summary>
        /// Updates the game.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <returns>true if the game has been updated, otherwise - false.</returns>
        Task<bool> UpdateGame(GameTransferObject game);

        /// <summary>
        /// Removes the game.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <returns>true if the game has been removed, otherwise - false.</returns>
        Task<bool> RemoveGame(string gameId);

        /// <summary>
        /// Selects all games.
        /// </summary>
        /// <returns><see cref="IQueryable{GameTransferObject}"/> query with all stored games.</returns>
        Task<IQueryable<GameTransferObject>> SelectGames();

        /// <summary>
        /// Selects the games with filter.
        /// </summary>
        /// <param name="leagueKey">The league key - can be either id or slug name of the league. If value is null or empty - no league filter is applied.</param>
        /// <param name="teamKey">The team key - can be either id or slug name of the team. If value is null or empty - no team filter is applied.</param>
        /// <param name="champName">Name of the champ. If value is null or empty - no champ filter is applied.</param>
        /// <returns><see cref="IQueryable{GameTransferObject}"/> query with all games matching the filter.</returns>
        Task<IQueryable<GameTransferObject>> SelectGamesWithFilter(string? leagueKey, string? teamKey, string? champName);
    }
}
