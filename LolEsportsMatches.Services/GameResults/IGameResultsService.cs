using PagedEnumerables;

namespace LolEsportsMatches.Services.GameResults
{
    /// <summary>
    /// Exposes game results service, which provides functionality for
    /// searching and managing game results.
    /// </summary>
    public interface IGameResultsService
    {
        /// <summary>
        /// Shows the games based on filters.
        /// </summary>
        /// <param name="leagueKey">The league key - can be either id or slug name of the league.</param>
        /// <param name="teamKey">The team key - can be either id or slug name of the team.</param>
        /// <param name="champId">The champ identifier.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns><see cref="IPagedEnumerable{GameResult}"/> with found games, or Null - if 
        /// games cannot be found due to wrong filters keys.</returns>
        Task<IPagedEnumerable<GameResult>?> ShowGames(
            string? leagueKey,
            string? teamKey,
            string? champId,
            int offset,
            int count);

        /// <summary>
        /// Updates the game.
        /// </summary>
        /// <param name="game">The game to update.</param>
        /// <returns>true if the game has been updated, otherwise - false.</returns>
        Task<bool> UpdateGame(GameResult game);

        /// <summary>
        /// Creates the game.
        /// </summary>
        /// <param name="game">The game to create.</param>
        /// <returns>true if the game has been created, otherwise - false.</returns>
        Task<bool> CreateGame(GameResult game);

        /// <summary>
        /// Deletes the game.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <returns>true if the game has been deleted, otherwise - false.</returns>
        Task<bool> DeleteGame(string gameId);

        /// <summary>
        /// Gets the specified game.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <returns><see cref="GameResult"/> if the specified game exists, otherwise - null.</returns>
        Task<GameResult?> GetGame(string gameId);

        /// <summary>
        /// Gets the game details.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <returns><see cref="GameDetailedResult"/> if the specified game exists, otherwise - null.</returns>
        Task<GameDetailedResult?> GetGameDetails(string gameId);
    }
}
