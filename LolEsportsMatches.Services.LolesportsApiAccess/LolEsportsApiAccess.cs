using Microsoft.Extensions.Logging;
using RestSharp;
using System.Text.Json;

namespace LolEsportsMatches.Services.LolesportsApiAccess
{
    /// <summary>
    /// Provides access to Lol Live Esport API.
    /// </summary>
    /// <remarks>
    /// Information about leagues, teams and matches is provided from <c>esports-api.lolesports.com</c>,
    /// and about the results of games - from <c>feed.lolesports.com</c>.
    /// </remarks>
    public class LolEsportsApiAccess
    {
        private readonly ILogger<LolEsportsApiAccess>? logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LolEsportsApiAccess"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public LolEsportsApiAccess(ILogger<LolEsportsApiAccess> logger) =>
            this.logger = logger;

        /// <summary>
        /// Gets the team transfer object by identifier.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <returns><see cref="Entities.TeamInfoTransferObject"/> if the team API request is successful and parsed,
        /// otherwise - null.</returns>
        public async Task<Entities.TeamInfoTransferObject?> GetTeamTransferObjectById(string teamId)
        {
            Entities.TeamInfoAnswerEntity? root = await GetResponseFromLolEsports<Entities.TeamInfoAnswerEntity>("getTeams", $"id={teamId}");
            return
                root is null ? null :
                (from team in root.Data.Teams
                 where team.Id == teamId
                 select new Entities.TeamInfoTransferObject
                 {
                     Id = teamId,
                     Slug = team.Slug,
                     Name = team.Name,
                     Code = team.Code,
                     Image = team.Image,
                     HomeLeague = team.HomeLeague.Name,
                     Region = team.HomeLeague.Region,
                     Players = team.Players.Select(p => new Entities.PlayerInfoTransferObject
                     {
                         SummonerName = p.SummonerName,
                         FirstName = p.FirstName,
                         LastName = p.LastName,
                         Image = p.Image,
                         Role = p.Role,
                     }).ToList(),
                 }).FirstOrDefault();
        }

        /// <summary>
        /// Gets all completed matches identifier by league identifier.
        /// </summary>
        /// <param name="leagueId">The league identifier.</param>
        /// <returns><see cref="IEnumerable{string}"/> if the completed matches API request is successful and parsed,
        /// otherwise - null.</returns>
        public async Task<IEnumerable<string>?> GetAllCompletedMatchesIdByLeagueId(string leagueId)
        {
            Entities.ScheduleAnswerEntity? root = await GetResponseFromLolEsports<Entities.ScheduleAnswerEntity>("getSchedule", $"leagueId={leagueId}");
            return
                root is null ? null :
                (from entity in root.Data.Schedule.Events
                 where entity.State == "completed"
                 select entity.Match.Id);
        }

        /// <summary>
        /// Gets all completed games identifier by match identifier.
        /// </summary>
        /// <param name="matchId">The match identifier.</param>
        /// <returns><see cref="IEnumerable{string}"/> if the all completed games API request is successful and parsed,
        /// otherwise - null.</returns>
        public async Task<IEnumerable<string>?> GetAllCompletedGamesIdByMatchId(string matchId)
        {
            Entities.MatchInfoAnswerEntity? root = await GetResponseFromLolEsports<Entities.MatchInfoAnswerEntity>("getEventDetails", $"id={matchId}");
            return
                root is null ? null :
                (from entity in root.Data.Event.Match.Games
                 where entity.State.Equals("completed", StringComparison.InvariantCultureIgnoreCase)
                 select entity.Id);
        }

        /// <summary>
        /// Gets the game detailed information by game identifier.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <returns><see cref="Entities.GameDetailedStatTransferObject"/> if the game details API request is successful and parsed,
        /// otherwise - null.</returns>
        public async Task<Entities.GameDetailedStatTransferObject?> GetGameDetailedInfoByGameId(string gameId)
        {
            Entities.GamePlayerBuildsEntity? root = await GetResponseFromFeedEsports<Entities.GamePlayerBuildsEntity>($"details/{gameId}", $"startingTime={CurrentTime()}");
            if (root is null)
            {
                return null;
            }

            Entities.BuildsFrame? frame = root.Frames[^1];

            Entities.GameDetailedStatTransferObject? dto = new()
            {
                ChampsDetails = frame.Participants.Select(f => new Entities.ChampsStatTransferObject
                {
                    Level = f.Level,
                    Kills = f.Kills,
                    Deaths = f.Deaths,
                    Assists = f.Assists,
                    TotalGoldEarned = f.TotalGoldEarned,
                    Items = f.Items,
                    FirstMainPerkId = f.PerkMetadata.Perks.FirstOrDefault(),
                    SubPerkId = f.PerkMetadata.SubStyleId,
                    Abilities = f.Abilities,
                }).ToList(),
            };

            return dto;
        }

        /// <summary>
        /// Gets the game result transfer object by game identifier.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <returns><see cref="Entities.GameResultTransferObject"/> if the game result API request is successful and parsed,
        /// otherwise - null.</returns>
        public async Task<Entities.GameResultTransferObject?> GetGameResultTransferObjectByGameId(string gameId)
        {
            Entities.GameAnswerEntity? firstRoot = await GetResponseFromFeedEsports<Entities.GameAnswerEntity>(
                $"window/{gameId}",
                $"startingTime={CurrentTime()}");

            if (firstRoot is null)
            {
                return null;
            }

            Entities.FrameEntity? lastFrame = firstRoot.Frames.LastOrDefault();
            Entities.GameResultTransferObject? dto = new()
            {
                Id = gameId,
                TeamBlueId = firstRoot.GameMetadata.BlueTeamMetadata.EsportsTeamId,
                TeamRedId = firstRoot.GameMetadata.RedTeamMetadata.EsportsTeamId,
                TeamBlueChampions =
                    (from champ in firstRoot.GameMetadata.BlueTeamMetadata.ParticipantMetadata
                     select champ.ChampionId).ToArray(),
                TeamRedChampions =
                    (from champ in firstRoot.GameMetadata.RedTeamMetadata.ParticipantMetadata
                     select champ.ChampionId).ToArray(),
                TeamBlueKills = (short)(lastFrame is not null ? lastFrame.BlueTeam.TotalKills : 0),
                TeamRedKills = (short)(lastFrame is not null ? lastFrame.RedTeam.TotalKills : 0),
            };

            Entities.GameAnswerEntity? secondRoot = await GetResponseFromFeedEsports<Entities.GameAnswerEntity>($"window/{gameId}", string.Empty);
            Entities.FrameEntity? firstFrame;
            if (secondRoot is null || (firstFrame = secondRoot.Frames.FirstOrDefault()) is null)
            {
                return null;
            }

            string? timeString = firstFrame.Rfc460Timestamp.Replace('T', ' ').Remove(16);
            DateTime date = DateTime.ParseExact(timeString, "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            dto.GameDate = date;
            return dto;
        }

        private async Task<T?> GetResponseFromLolEsports<T>(string path, string query) where T : class
        {
            string? basePath = @$"https://esports-api.lolesports.com/persisted/gw/";
            string? fullQuery = "hl=en-GB&" + query;
            RestResponse? response = await GetResponse(basePath, path, fullQuery, "x-api-key", "0TvQnueqKa5mxJntVWt0w4LpLfEkrV1Ta8rQBb9Z");
            if (response is null || !response.IsSuccessful || string.IsNullOrWhiteSpace(response.Content))
            {
                return null;
            }

            JsonSerializerOptions? options = new()
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<T>(response.Content, options);
        }

        private async Task<T?> GetResponseFromFeedEsports<T>(string path, string query) where T : class
        {
            string? basePath = @$"https://feed.lolesports.com/livestats/v1/";
            RestResponse? response = await GetResponse(basePath, path, query, null, null);
            if (response is null || !response.IsSuccessful || string.IsNullOrWhiteSpace(response.Content))
            {
                return null;
            }

            JsonSerializerOptions? options = new()
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<T>(response.Content, options);
        }

        private async Task<RestResponse?> GetResponse(string basePath, string requestPath, string query, string? headerKey, string? headerValue)
        {
            RestClient? client = new(basePath);
            RestRequest? request = new(requestPath + "?" + query, Method.Get);
            if (headerKey is not null && headerValue is not null)
            {
                request.AddHeader(headerKey, headerValue);
            }

            this.logger?.LogInformation("execute request: path={path}{requestPath} query={query}", basePath, requestPath, query);
            RestResponse? response = await client.ExecuteAsync(request);
            if (response is null)
            {
                this.logger?.LogError("response is null");
                return null;
            }

            if (!response.IsSuccessful)
            {
                this.logger?.LogError("response is failed. {answer}", response.Content);
                return null;
            }

            return response;
        }

        private static string CurrentTime()
        {
            DateTime time = DateTime.UtcNow.AddMinutes(-1);
            char separator = 'T';
            return time.ToString($"yyyy-MM-dd{separator}HH:mm:00.000Z");
        }
    }
}
