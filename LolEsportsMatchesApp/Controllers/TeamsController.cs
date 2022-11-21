using LolEsportsMatchesApp.Models;
using LolEsportsMatchesApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LolEsportsMatchesApp.Controllers
{
    [AllowAnonymous]
    [Route("teams")]
    public class TeamsController : Controller
    {
        private readonly IGameResultsService gameResultService;
        private readonly ITeamInfoService teamInfoService;
        private readonly ILolDataChampionsService championsService;

        private readonly int ElementsOnPageLimit = 25;

        public TeamsController(
            IGameResultsService gameService,
            ITeamInfoService teamInfoService,
            ILolDataChampionsService championsService)
        {
            this.gameResultService = gameService ?? throw new ArgumentNullException(nameof(gameService));
            this.teamInfoService = teamInfoService ?? throw new ArgumentNullException(nameof(teamInfoService));
            this.championsService = championsService ?? throw new ArgumentNullException(nameof(championsService));
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] string region = "", [FromQuery] string name = "")
        {
            ViewData["selectedRegion"] = region;
            ViewData["selectedName"] = name;
            //ViewData["regionsList"] = Regions;

            ViewData["regionsList"] = (await this.teamInfoService.ShowRegions()).ToList();

            PagedEnumerables.PageSettings pageSettings = new(page, this.ElementsOnPageLimit);

            PagedEnumerables.IPagedEnumerable<TeamInfo>? teams = await this.teamInfoService.ShowTeams(region, name, pageSettings.Offset, pageSettings.Count);
            if (teams is null)
            {
                return NotFound();
            }

            IEnumerable<TeamModel> teamsModel = teams
                .Select(t => new TeamModel
                {
                    Id = t.Id,
                    Slug = t.Slug,
                    Name = t.Name,
                    Image = t.Image,
                });

            return View(PagedModel<TeamModel>.CreatePagedModel(teamsModel, string.Empty, pageSettings, teams.TotalCount));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DisplayTeam(string id, [FromQuery] int page = 1)
        {
            TeamDetailedInfo? team = await this.teamInfoService.GetTeamDetailsById(id);

            if (team is null)
            {
                return NotFound();
            }

            PagedEnumerables.PageSettings pageSettings = new(page, this.ElementsOnPageLimit);
            PagedEnumerables.IPagedEnumerable<GameResult>? games = await this.gameResultService.ShowGames(null, id, null, pageSettings.Offset, pageSettings.Count);

            if (games is null)
            {
                return RedirectToAction("Index", "Teams");
            }

            IEnumerable<ShortGameResultModel> gamesModel = games
                .Select(g =>
                {
                    string[] selectedTeamChamps;
                    TeamInfo opponentTeam;
                    (selectedTeamChamps, opponentTeam) = g.BlueTeam.Id == team.Id
                        ? (g.ChampionsBlue, g.RedTeam)
                        : (g.ChampionsRed, g.BlueTeam);

                    return new ShortGameResultModel
                    {
                        Id = g.Id,
                        Opponent = new TeamModel
                        {
                            Id = opponentTeam.Id,
                            Slug = opponentTeam.Slug,
                            Code = opponentTeam.Code,
                            Image = opponentTeam.Image,
                        },
                        SelectedTeamChampions = selectedTeamChamps,
                        SelectedTeamChampionsImage = selectedTeamChamps.Select(name => this.championsService.GetChampionImage(name)).ToArray(),
                        GameDate = g.GameDate,
                    };
                });

            TeamDetailedViewModel teamDetailedModel = new()
            {
                Id = team.Id,
                Slug = team.Slug,
                Name = team.Name,
                Code = team.Code,
                Image = team.Image,
                HomeLeagueName = team.HomeLeague,
                Region = team.Region,
                Players = team.Players.Select(p => new PlayerModel
                {
                    SummonerName = p.SummonerName,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Image = p.Image,
                    Role = p.Role,
                }).ToList(),
                PagedGames = PagedModel<ShortGameResultModel>.CreatePagedModel(
                    gamesModel,
                    id,
                    pageSettings,
                    games.TotalCount),
            };

            return View(teamDetailedModel);
        }

        private static readonly List<string> Regions = new()
        {
            "LATIN AMERICA",
            "BRAZIL",
            "CHINA",
            "EUROPE",
            "COMMONWEALTH OF INDEPENDENT STATES",
            "NORTH AMERICA",
            "KOREA",
            "HONG KONG, MACAU, TAIWAN",
            "OCEANIA",
            "TURKEY",
            "JAPAN",
            "VIETNAM",
        };
    }
}
