using LolEsportsMatches.DataAccess;
using LolEsportsMatches.DataAccess.Teams;
using LolEsportsMatches.Services.Teams;
using PagedEnumerables;

namespace LolEsportsMatches.Services.LifeTimeUpdated
{
    /// <summary>
    /// Provides access to the League of Legends teams information stored in the <see cref="LolEsportsMatchesDataAccessFactory"/> repository,
    /// which are constantly updated from the <see cref="LolEsportsApiAccess"/> Life LoL teams API.
    /// </summary>
    /// <seealso cref="LolEsportsMatches.Services.Teams.ITeamInfoService" />
    public class TeamsInfoService : ITeamInfoService
    {
        private readonly LolEsportsMatchesDataAccessFactory factory;
        private readonly LolesportsApiAccess.LolEsportsApiAccess apiAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamsInfoService"/> class.
        /// </summary>
        /// <param name="factory">The data access factory.</param>
        /// <param name="api">The Leauge of Legends Life Game API.</param>
        public TeamsInfoService(LolEsportsMatchesDataAccessFactory factory, LolesportsApiAccess.LolEsportsApiAccess api)
        {
            this.factory = factory;
            this.apiAccess = api;
        }

        /// <summary>
        /// Gets the team.
        /// </summary>
        /// <remarks>
        /// Шf this team is not stored in the repository, information about it is saved from third-party services.
        /// </remarks>
        /// <param name="key">The key - can be either id or slug name of the team.</param>
        /// <returns>
        ///   <see cref="T:LolEsportsMatches.Services.Teams.TeamInfo" /> if the specified team exists, otherwise - null.
        /// </returns>
        public async Task<TeamInfo?> GetTeam(string key)
        {
            ITeamDataAccessObject dao = this.factory.GetTeamDataAccessObject();
            TeamTransferObject dto;
            try
            {
                dto = await dao.FindTeam(key);
            }
            catch (TeamNotFoundException)
            {
                LolesportsApiAccess.Entities.TeamInfoTransferObject? lifeTeamInfo = await this.apiAccess.GetTeamTransferObjectById(key);
                if (lifeTeamInfo is null)
                {
                    return null;
                }

                dto = new TeamTransferObject
                {
                    Id = key,
                    Slug = lifeTeamInfo.Slug,
                    Name = lifeTeamInfo.Name,
                    Code = lifeTeamInfo.Code,
                    Image = lifeTeamInfo.Image,
                    Region = lifeTeamInfo.Region,
                };

                await dao.InsertTeam(dto);
            }

            return ObjectsMapper.MapTeam(dto);
        }

        /// <summary>
        /// Updates the team.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>
        /// true if the team has been updates, otherwise - false.
        /// </returns>
        public async Task<bool> UpdateTeam(TeamInfo team)
        {
            try
            {
                ITeamDataAccessObject dao = this.factory.GetTeamDataAccessObject();
                await dao.UpdateTeam(ObjectsMapper.MapTeam(team));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Creates the team.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>
        /// true if the team has been created, otherwise - false.
        /// </returns>
        public async Task<bool> CreateTeam(TeamInfo team)
        {
            ITeamDataAccessObject dao = this.factory.GetTeamDataAccessObject();
            return await dao.InsertTeam(ObjectsMapper.MapTeam(team));
        }

        /// <summary>
        /// Deletes the team.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <returns>
        /// true if the team has been deleted, otherwise - false.
        /// </returns>
        public async Task<bool> DeleteTeam(string teamId)
        { 
            ITeamDataAccessObject dao = this.factory.GetTeamDataAccessObject();
            return await dao.RemoveTeam(teamId);
        }

        /// <summary>
        /// Shows all teams.
        /// </summary>
        /// <returns>
        ///   <see cref="T:System.Collections.Generic.IEnumerable`1" /> with all teams.
        /// </returns>
        public async Task<IEnumerable<TeamInfo>> ShowTeams()
        {
            ITeamDataAccessObject dao = this.factory.GetTeamDataAccessObject();
            IQueryable<TeamTransferObject>? teams = await dao.SelectTeams();
            return teams.Select(dto => ObjectsMapper.MapTeam(dto)).AsEnumerable();
        }

        /// <summary>
        /// Shows the teams based on filters.
        /// </summary>
        /// <param name="region">The region name.</param>
        /// <param name="name">The part of team name or team code.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns>
        ///   <see cref="T:PagedEnumerables.IPagedEnumerable`1" /> with found teams.
        /// </returns>
        public async Task<IPagedEnumerable<TeamInfo>> ShowTeams(string? region, string? name, int offset, int count)
        {
            ITeamDataAccessObject dao = this.factory.GetTeamDataAccessObject();
            IQueryable<TeamTransferObject>? teams = await dao.SelectTeamsWithFilter(region, name);
            return
                new PagedQueryable<TeamInfo>(teams.Select(dto => ObjectsMapper.MapTeam(dto)), offset, count);
        }

        /// <summary>
        /// Gets the team details by identifier.
        /// </summary>
        /// <param name="key">The key - can be either id or slug name of the team.</param>
        /// <returns>
        ///   <see cref="T:LolEsportsMatches.Services.Teams.TeamDetailedInfo" /> if the specified team exists, otherwise - null.
        /// </returns>
        public async Task<TeamDetailedInfo?> GetTeamDetailsById(string key)
        {
            ITeamDataAccessObject dao = this.factory.GetTeamDataAccessObject();
            try
            {
                TeamTransferObject? teamFromRepository = await dao.FindTeam(key);
                LolesportsApiAccess.Entities.TeamInfoTransferObject? lifeTeamInfo = await this.apiAccess.GetTeamTransferObjectById(teamFromRepository.Id);
                return lifeTeamInfo is not null
                    ? ObjectsMapper.MapTeam(lifeTeamInfo)
                    : new TeamDetailedInfo
                    {
                        Id = teamFromRepository.Id,
                        Slug = teamFromRepository.Slug,
                        Name = teamFromRepository.Name,
                        Code = teamFromRepository.Code,
                        Image = teamFromRepository.Image,
                        Region = teamFromRepository.Region,
                        HomeLeague = "",
                        Players = new(),
                    };
            }
            catch (TeamNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// Shows the regions, sorted alphabetically.
        /// </summary>
        /// <returns>
        ///   <see cref="IEnumerable{string}" /> with all existed regions.
        /// </returns>
        public async Task<IEnumerable<string>> ShowRegions()
        {
            ITeamDataAccessObject dao = this.factory.GetTeamDataAccessObject();
            var query = await dao.SelectTeams();
            return query.Select(dto => dto.Region).Distinct().OrderBy(r => r);
        }
    }
}
