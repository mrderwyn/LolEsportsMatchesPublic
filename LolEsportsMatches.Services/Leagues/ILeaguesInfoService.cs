namespace LolEsportsMatches.Services.Leagues
{
    /// <summary>
    /// Exposes leagues information service, which provides functionality for
    /// searching and managing leagues.
    /// </summary>
    public interface ILeaguesInfoService
    {
        /// <summary>
        /// Shows the leagues.
        /// </summary>
        /// <returns><see cref="IEnumerable{LeagueInfo}"/> with all leagues.</returns>
        Task<IEnumerable<LeagueInfo>> ShowLeagues();

        /// <summary>
        /// Updates the league.
        /// </summary>
        /// <param name="league">The league.</param>
        /// <returns>true if the league has been updated, otherwise - false.</returns>
        Task<bool> UpdateLeague(LeagueInfo league);

        /// <summary>
        /// Creates the league.
        /// </summary>
        /// <param name="league">The league.</param>
        /// <returns>true if the league has been created, otherwise - false.</returns>
        Task<bool> CreateLeague(LeagueInfo league);

        /// <summary>
        /// Deletes the league.
        /// </summary>
        /// <param name="leagueId">The league identifier.</param>
        /// <returns>true if the league has been deleted, otherwise - false.</returns>
        Task<bool> DeleteLeague(string leagueId);

        /// <summary>
        /// Gets the league.
        /// </summary>
        /// <param name="key">The league key - can be either id or slug name of the league.</param>
        /// <returns><see cref="LeagueInfo"/> if the specified league exists, otherwise - null.</returns>
        Task<LeagueInfo?> GetLeague(string key);
    }
}
