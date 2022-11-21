using PagedEnumerables;

namespace LolEsportsMatches.Services.Teams
{
    /// <summary>
    /// Exposes teams information service, which provides functionality for
    /// searching and managing teams.
    /// </summary>
    public interface ITeamInfoService
    {
        /// <summary>
        /// Gets the team.
        /// </summary>
        /// <param name="key">The key - can be either id or slug name of the team.</param>
        /// <returns><see cref="TeamInfo"/> if the specified team exists, otherwise - null.</returns>
        Task<TeamInfo?> GetTeam(string key);

        /// <summary>
        /// Gets the team details by identifier.
        /// </summary>
        /// <param name="key">The key - can be either id or slug name of the team.</param>
        /// <returns><see cref="TeamDetailedInfo"/> if the specified team exists, otherwise - null.</returns>
        Task<TeamDetailedInfo?> GetTeamDetailsById(string key);

        /// <summary>
        /// Updates the team.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>true if the team has been updates, otherwise - false.</returns>
        Task<bool> UpdateTeam(TeamInfo team);

        /// <summary>
        /// Creates the team.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>true if the team has been created, otherwise - false.</returns>
        Task<bool> CreateTeam(TeamInfo team);

        /// <summary>
        /// Deletes the team.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <returns>true if the team has been deleted, otherwise - false.</returns>
        Task<bool> DeleteTeam(string teamId);

        /// <summary>
        /// Shows the teams based on filters.
        /// </summary>
        /// <param name="region">The region name.</param>
        /// <param name="name">The part of team name or team code.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns><see cref="IPagedEnumerable{TeamInfo}"/> with found teams.</returns>
        Task<IPagedEnumerable<TeamInfo>> ShowTeams(string? region, string? name, int offset, int count);

        /// <summary>
        /// Shows all teams.
        /// </summary>
        /// <returns><see cref="IEnumerable{TeamInfo}"/> with all teams.</returns>
        Task<IEnumerable<TeamInfo>> ShowTeams();

        /// <summary>
        /// Shows the regions.
        /// </summary>
        /// <returns><see cref="IEnumerable{String}"/> with all existed regions.</returns>
        Task<IEnumerable<string>> ShowRegions();
    }
}
