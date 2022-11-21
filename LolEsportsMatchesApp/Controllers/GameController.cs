using LolEsportsMatchesApp.Models;
using LolEsportsMatchesApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LolEsportsMatchesApp.Controllers
{
    [AllowAnonymous]
    [Route("game")]
    public class GameController : Controller
    {
        private readonly IGameResultsService gameResultService;
        private readonly ILolDataChampionsService championsService;
        private readonly ILolDataItemsService itemsService;
        private readonly ILolDataRunesService runesService;

        public GameController(
            IGameResultsService gameService,
            ILolDataChampionsService championsService,
            ILolDataItemsService itemsService,
            ILolDataRunesService runesService)
        {
            this.gameResultService = gameService ?? throw new ArgumentNullException(nameof(gameService));
            this.championsService = championsService ?? throw new ArgumentNullException(nameof(championsService));
            this.itemsService = itemsService ?? throw new ArgumentNullException(nameof(itemsService));
            this.runesService = runesService ?? throw new ArgumentNullException(nameof(runesService));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Index(string id)
        {
            GameDetailedResult? g = await this.gameResultService.GetGameDetails(id);
            if (g is null)
            {
                return this.NotFound();
            }

            GameDetailedViewModel model = new()
            {
                Id = g.Id,
                LeagueId = g.League.Id,
                LeagueSlug = g.League.Slug,
                LeagueName = g.League.Name,
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
                ChampsBlueStat = g.ChampionsBlueStat
                    .Select(s => ChampStatModelFromObj(s))
                    .ToArray(),
                ChampsRedStat = g.ChampionsRedStat
                    .Select(s => ChampStatModelFromObj(s))
                    .ToArray(),
            };

            return View(model);
        }

        private ChampionIngameStatModel ChampStatModelFromObj(ChampionIngameStat stat)
        {
            List<int> removeAllItems = new()
            {
                3363,
                2138,
                2139,
                2140,
                3364,
                3340,
            };

            List<int> letOnlyOneItem = new()
            {
                2055,
            };

            List<int> itemsIds = stat.ItemsId
                .Except(removeAllItems)
                .GroupBy(i => i)
                .SelectMany(g => letOnlyOneItem.Contains(g.Key)
                    ? g.Take(1)
                    : g)
                .ToList();

            if (itemsIds.Count < 6)
            {
                itemsIds.AddRange(Enumerable.Repeat<int>(0, 6 - itemsIds.Count));
            }

            if (itemsIds.Count > 6)
            {
                string? debugLine = string.Join(',', itemsIds);
                Console.WriteLine($"\t\t!!items more than 6 = {debugLine}");
            }

            Rune mainRune = this.runesService.GetRune(stat.FirstMainPerkId);
            Rune subRune = this.runesService.GetRune(stat.SubPerkId);
            IEnumerable<Item> itemsList = itemsIds.Select(id => this.itemsService.GetItem(id));

            ChampionIngameStatModel result = new()
            {
                Level = stat.Level,
                Kills = stat.Kills,
                Deaths = stat.Deaths,
                Assists = stat.Assists,
                Gold = stat.Gold,
                Abilities = string.Join(string.Empty, stat.Abilities.Take(8)),
                ItemsImage = itemsList.Select(item => this.itemsService.GetItemImage(item)).ToArray(),
                ItemsName = itemsList.Select(item => item.Name).ToArray(),
                MainPerk = (mainRune.Name, this.runesService.GetRuneImage(mainRune)),
                SubPerk = (subRune.Name, this.runesService.GetRuneImage(subRune)),
            };

            return result;
        }
    }
}
