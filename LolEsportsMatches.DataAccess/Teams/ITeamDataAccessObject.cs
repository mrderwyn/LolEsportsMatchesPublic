namespace LolEsportsMatches.DataAccess.Teams
{
    /// <summary>
    /// Provide access to stored teams.
    /// </summary>
    public interface ITeamDataAccessObject
    {
        /// <summary>
        /// Inserts the team.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>true if the team has been inserted, otherwise - false.</returns>
        Task<bool> InsertTeam(TeamTransferObject team);

        /// <summary>
        /// Finds the team.
        /// </summary>
        /// <param name="key">The key - can be either id or slug name of the team.</param>
        /// <returns><see cref="TeamTransferObject"/> of found team.</returns>
        Task<TeamTransferObject> FindTeam(string key);

        /// <summary>
        /// Updates the team.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>true if the team has been updated, otherwise - false.</returns>
        Task<bool> UpdateTeam(TeamTransferObject team);

        /// <summary>
        /// Removes the team.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <returns>true if the team has been removed, otherwise - false.</returns>
        Task<bool> RemoveTeam(string teamId);

        /// <summary>
        /// Selects all teams.
        /// </summary>
        /// <returns><see cref="IQueryable{TeamTransferObject}"/> query with all stored teams.</returns>
        Task<IQueryable<TeamTransferObject>> SelectTeams();

        /// <summary>
        /// Selects the teams with filter.
        /// </summary>
        /// <param name="region">The region name. If value is null or empty - no region filter is applied.</param>
        /// <param name="name">The part of team name or team code. If value is null or empty - no name filter is applied.</param>
        /// <returns><see cref="IQueryable{TeamTransferObject}"/> query with all teams matching the filter.</returns>
        Task<IQueryable<TeamTransferObject>> SelectTeamsWithFilter(string? region, string? name);
    }
}
