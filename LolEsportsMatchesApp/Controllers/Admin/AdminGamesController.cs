using LolEsportsMatchesApp.Models;
using LolEsportsMatchesApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LolEsportsMatchesApp.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("admin/games")]
    public class AdminGamesController : Controller
    {
        private readonly IGameResultsService gameService;
        private readonly ILeaguesInfoService leagueService;
        private readonly ITeamInfoService teamService;
        private readonly ILolDataChampionsService champService;
        private readonly int ElementsOnPageLimit = 25;

        public AdminGamesController(
            IGameResultsService gameService,
            ILeaguesInfoService leagueService,
            ITeamInfoService teamService,
            ILolDataChampionsService champs)
        {
            this.gameService = gameService;
            this.teamService = teamService;
            this.leagueService = leagueService;
            this.champService = champs;
        }

        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] string? leagueId = "", [FromQuery] string? teamId = "", [FromQuery] string? champId = "", [FromQuery] string? gameId = "")
        {
            await FillDropDownLists();
            ViewData["selectedChamp"] = champId;
            ViewData["selectedTeam"] = teamId;
            ViewData["selectedLeague"] = leagueId;
            ViewData["selectedGame"] = gameId;

            PagedEnumerables.PageSettings pageSettings = new(page, this.ElementsOnPageLimit);


            PagedEnumerables.IPagedEnumerable<GameResult>? games = string.IsNullOrWhiteSpace(gameId)
                ? await this.gameService.ShowGames(leagueId, teamId, champId, pageSettings.Offset, pageSettings.Count)
                : await TryGetGame(gameId);

            if (games is null)
            {
                return NotFound();
            }

            IEnumerable<GameResultViewModel> model = games
                .Select(game =>
                    new GameResultViewModel
                    {
                        Id = game.Id,
                        LeagueId = game.League.Id,
                        LeagueName = game.League.Name,
                        TeamBlueId = game.BlueTeam.Id,
                        BlueTeam = new TeamModel { Name = game.BlueTeam.Name, Code = game.BlueTeam.Code },
                        RedTeam = new TeamModel { Name = game.RedTeam.Name, Code = game.RedTeam.Code },
                        TeamRedId = game.RedTeam.Id,
                        ChampionsBlue = game.ChampionsBlue,
                        ChampionsRed = game.ChampionsRed,
                        KillsBlue = game.KillsBlue,
                        KillsRed = game.KillsRed,
                        GameDate = game.GameDate,
                    }
                );

            return View(PagedModel<GameResultViewModel>.CreatePagedModel(model, string.Empty, pageSettings, games.TotalCount));

            async Task<PagedEnumerables.IPagedEnumerable<GameResult>> TryGetGame(string id)
            {
                GameResult? result = await this.gameService.GetGame(id);
                return result is null
                    ? PagedEnumerables.PagedQueryable<GameResult>.Empty()
                    : PagedEnumerables.PagedQueryable<GameResult>.Single(result);
            }
        }

        [HttpPost("{id}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            _ = await gameService.DeleteGame(id);
            return RedirectToAction("Index");
        }

        [HttpGet("new")]
        public async Task<IActionResult> New()
        {
            await FillDropDownLists();
            return View();
        }

        [HttpPost("new")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(GameResultModel game)
        {
            if (ModelState.IsValid)
            {
                bool result = await gameService.CreateGame(new GameResult
                {
                    Id = game.Id,
                    League = new LeagueInfo { Id = game.LeagueId },
                    BlueTeam = new TeamInfo { Id = game.TeamBlueId },
                    RedTeam = new TeamInfo { Id = game.TeamRedId },
                    ChampionsBlue = game.ChampionsBlue,
                    ChampionsRed = game.ChampionsRed,
                    KillsBlue = game.KillsBlue,
                    KillsRed = game.KillsRed,
                    GameDate = game.GameDate,
                });

                if (result)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", $"Cannot create game with {game.Id} id.");
            }

            await FillDropDownLists();
            return View();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Update(string id)
        {
            await FillDropDownLists();
            GameResult? game = await gameService.GetGame(id);
            if (game is null)
            {
                return NotFound();
            }

            return View(new GameResultModel
            {
                Id = game.Id,
                LeagueId = game.League.Id,
                TeamBlueId = game.BlueTeam.Id,
                TeamRedId = game.RedTeam.Id,
                ChampionsBlue = game.ChampionsBlue,
                ChampionsRed = game.ChampionsRed,
                KillsBlue = game.KillsBlue,
                KillsRed = game.KillsRed,
                GameDate = game.GameDate,
            });
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(GameResultModel game)
        {
            if (ModelState.IsValid)
            {
                bool result = await gameService.UpdateGame(new GameResult
                {
                    Id = game.Id,
                    League = new LeagueInfo { Id = game.LeagueId },
                    BlueTeam = new TeamInfo { Id = game.TeamBlueId },
                    RedTeam = new TeamInfo { Id = game.TeamRedId },
                    ChampionsBlue = game.ChampionsBlue,
                    ChampionsRed = game.ChampionsRed,
                    KillsBlue = game.KillsBlue,
                    KillsRed = game.KillsRed,
                    GameDate = game.GameDate,
                });

                if (result)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Cannot update game.");
            }

            await FillDropDownLists();
            return View(game);
        }

        private async Task FillDropDownLists()
        {
            IList<Champion> champsList = this.champService.GetChampions();
            List<SelectListItem> champsDrop = champsList.Select(c => new SelectListItem { Value = c.Id, Text = c.Name }).ToList();
            ViewData["champsDrop"] = champsDrop;

            IEnumerable<LeagueInfo>? leaguesList = await this.leagueService.ShowLeagues();
            IEnumerable<TeamInfo>? teamsList = (await this.teamService.ShowTeams()).OrderBy(t => t.Code);

            List<SelectListItem> leaguesDrop = leaguesList.Select(l => new SelectListItem { Value = l.Id, Text = l.Name }).ToList();
            List<SelectListItem> teamsDrop = teamsList.Select(t => new SelectListItem { Value = t.Id, Text = $"{t.Code}({t.Name})" }).ToList();
            ViewData["leaguesDrop"] = leaguesDrop;
            ViewData["teamsDrop"] = teamsDrop;
        }
    }
}
