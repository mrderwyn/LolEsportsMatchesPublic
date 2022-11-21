using LolEsportsMatchesApp.Models;
using LolEsportsMatchesApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LolEsportsMatchesApp.Controllers
{
    [AllowAnonymous]
    [Route("leagues")]
    public class LeaguesController : Controller
    {
        private readonly IGameResultsService gameResultService;
        private readonly ILolDataChampionsService championsService;

        private readonly int ElementsOnPageLimit = 20;

        public LeaguesController(
            IGameResultsService gameService,
            ILolDataChampionsService championsService)
        {
            this.gameResultService = gameService ?? throw new ArgumentNullException(nameof(gameService));
            this.championsService = championsService ?? throw new ArgumentNullException(nameof(championsService));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Index(string id, [FromQuery] int page = 1, [FromQuery] string? teamId = "", [FromQuery] string? champId = "")
        {
            ViewData["champsList"] = this.championsService.GetChampions();
            ViewData["selectedChamp"] = champId;
            ViewData["selectedTeam"] = teamId;

            PagedEnumerables.PageSettings pageSettings = new(page, this.ElementsOnPageLimit);

            PagedEnumerables.IPagedEnumerable<GameResult>? games = await this.gameResultService.ShowGames(id, teamId, champId, pageSettings.Offset, pageSettings.Count);
            if (games is null)
            {
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<GameResultViewModel> gamesModel = games
                .Select(g =>
                {
                    return new GameResultViewModel
                    {
                        Id = g.Id,
                        BlueTeam = new TeamModel
                        {
                            Id = g.BlueTeam.Id,
                            Name = g.BlueTeam.Name,
                            Code = g.BlueTeam.Code,
                            Image = g.BlueTeam.Image,
                            Slug = g.BlueTeam.Slug,
                        },
                        RedTeam = new TeamModel
                        {
                            Id = g.RedTeam.Id,
                            Name = g.RedTeam.Name,
                            Code = g.RedTeam.Code,
                            Image = g.RedTeam.Image,
                            Slug = g.RedTeam.Slug,
                        },
                        ChampionsBlue = g.ChampionsBlue,
                        ChampionsRed = g.ChampionsRed,
                        ChampionsBlueImage = g.ChampionsBlue.Select(name => this.championsService.GetChampionImage(name)).ToArray(),
                        ChampionsRedImage = g.ChampionsRed.Select(name => this.championsService.GetChampionImage(name)).ToArray(),
                        KillsBlue = g.KillsBlue,
                        KillsRed = g.KillsRed,
                        GameDate = g.GameDate,
                    };
                });

            return View(PagedModel<GameResultViewModel>.CreatePagedModel(gamesModel, id, pageSettings, games.TotalCount));
        }
    }
}
