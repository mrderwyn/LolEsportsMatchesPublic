namespace LolEsportsMatches.DataAccess.Leagues
{
    /// <summary>
    /// Provides access to stored leagues.
    /// </summary>
    public interface ILeagueDataAccessObject
    {
        /// <summary>
        /// Returns ids of all not stored matches for the specified league.
        /// </summary>
        /// <param name="leagueId">The league identifier.</param>
        /// <param name="matchesIds">The matches ids.</param>
        /// <returns><see cref="IEnumerable{string}"/>ids of all not stored matches.</returns>
        Task<IEnumerable<string>> NotStored(string leagueId, IEnumerable<string>? matchesIds);

        /// <summary>
        /// Finds the league.
        /// </summary>
        /// <param name="key">The key - can be either id or slug name of the league.</param>
        /// <returns><see cref="LeagueTransferObject"/> of found league.</returns>
        Task<LeagueTransferObject> FindLeague(string key);

        /// <summary>
        /// Updates the league.
        /// </summary>
        /// <param name="league">The league.</param>
        /// <returns>true if the league has been updated, otherwise - false.</returns>
        Task<bool> UpdateLeague(LeagueTransferObject league);

        /// <summary>
        /// Selects all leagues.
        /// </summary>
        /// <returns><see cref="IEnumerable{LeagueTransferObject}"/> query with all stored leagues.</returns>
        Task<IEnumerable<LeagueTransferObject>> SelectLeagues();

        /// <summary>
        /// Removes the league.
        /// </summary>
        /// <param name="leagueId">The league identifier.</param>
        /// <returns>true if the league has been removed, otherwise - false.</returns>
        Task<bool> RemoveLeague(string leagueId);

        /// <summary>
        /// Inserts the league.
        /// </summary>
        /// <param name="league">The league.</param>
        /// <returns>true if the league has been inserted, otherwise - false.</returns>
        Task<bool> InsertLeague(LeagueTransferObject league);
    }
}
