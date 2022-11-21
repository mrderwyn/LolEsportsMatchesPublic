using LolEsportsMatches.DataAccess.EntityFrameworkCore.Context;
using LolEsportsMatches.DataAccess.Teams;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DelegateDecompiler.EntityFrameworkCore;

namespace LolEsportsMatches.DataAccess.EntityFrameworkCore
{
    /// <summary>
    /// Provide access to stored teams using EF Core (<see cref="MatchHistoryContext"/> context).
    /// </summary>
    /// <seealso cref="LolEsportsMatches.DataAccess.Teams.ITeamDataAccessObject" />
    public class TeamEntityFrameworkDataAccessObject : ITeamDataAccessObject
    {
        private readonly MatchHistoryContext context;
        private readonly ILogger<TeamEntityFrameworkDataAccessObject>? logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamEntityFrameworkDataAccessObject"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        public TeamEntityFrameworkDataAccessObject(MatchHistoryContext context, ILogger<TeamEntityFrameworkDataAccessObject>? logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger;
        }

        /// <summary>
        /// Finds the team.
        /// </summary>
        /// <param name="key">The key - can be either id or slug name of the team.</param>
        /// <returns>
        ///   <see cref="T:LolEsportsMatches.DataAccess.Teams.TeamTransferObject" /> of found team or throw <see cref="TeamNotFoundException"/> exception, if team is not found.
        /// </returns>
        /// <exception cref="LolEsportsMatches.DataAccess.Teams.TeamNotFoundException">if team is not found.</exception>
        public async Task<TeamTransferObject> FindTeam(string key)
        {
            Entities.TeamEntity? entity = await this.context.Teams.FirstOrDefaultAsync(e => e.Id == key || e.Slug == key);
            if (entity is null)
            {
                throw new TeamNotFoundException(key);
            }

            return ObjectsMapper.MapTeam(entity);
        }

        /// <summary>
        /// Inserts the team.
        /// </summary>
        /// <remarks>if the team cannot be inserted - an error is logged</remarks>
        /// <param name="team">The team.</param>
        /// <returns>
        /// true if the team has been inserted, otherwise - false.
        /// </returns>
        public async Task<bool> InsertTeam(TeamTransferObject team)
        {
            try
            {
                Entities.TeamEntity? entity = ObjectsMapper.MapTeam(team);
                await this.context.Teams.AddAsync(entity);
                await this.context.SaveChangesAsync();
                return true;
            }
            catch
            {
                this.logger?.LogError("Cannot insert team (id = #{id})", team.Id);
                return false;
            }
        }

        /// <summary>
        /// Removes the team.
        /// </summary>
        /// <remarks>if the team cannot be removed - an error is logged</remarks>
        /// <param name="teamId">The team identifier.</param>
        /// <returns>
        /// true if the team has been removed, otherwise - false.
        /// </returns>
        public async Task<bool> RemoveTeam(string teamId)
        {
            try
            {
                Entities.TeamEntity? entity = await this.context.Teams.FindAsync(teamId);
                if (entity is null)
                {
                    return false;
                }

                this.context.Teams.Remove(entity);
                await this.context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Selects all teams.
        /// </summary>
        /// <returns>
        ///   <see cref="IQueryable{TeamTransferObject}" /> query with all stored teams.
        /// </returns>
        public async Task<IQueryable<TeamTransferObject>> SelectTeams()
        {
            return await Task.Run(() => this.context.Teams.DecompileAsync().Select(e => e.MapTeam()));
        }

        /// <summary>
        /// Selects the teams with filter.
        /// </summary>
        /// <param name="region">The region name. If value is null or empty - no region filter is applied.</param>
        /// <param name="name">The part of team name or team code. If value is null or empty - no name filter is applied.</param>
        /// <returns>
        ///   <see cref="IQueryable{TeamTransferObject}" /> query with all teams matching the filter.
        /// </returns>
        public async Task<IQueryable<TeamTransferObject>> SelectTeamsWithFilter(string? region, string? name)
        {
            IQueryable<Entities.TeamEntity>? teams = this.context.Teams.DecompileAsync();

            if (!string.IsNullOrWhiteSpace(region))
            {
                teams = teams.Where(t => t.Region == region);
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.ToLower();
                teams = teams.Where(t => EF.Functions.Like(t.Code.ToLower(), $"%{name}%")
                                        || EF.Functions.Like(t.Name.ToLower(), $"%{name}%"));
            }

            return await Task.Run(() => teams.Select(e => e.MapTeam()));
        }

        /// <summary>
        /// Updates the team.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>
        /// true if the team has been updated, otherwise - false.
        /// </returns>
        public async Task<bool> UpdateTeam(TeamTransferObject team)
        {
            try
            {
                Entities.TeamEntity? entity = ObjectsMapper.MapTeam(team);
                this.context.Teams.Update(entity);

                await this.context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
