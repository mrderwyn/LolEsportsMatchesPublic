using LolEsportsMatchesApp.Models;
using LolEsportsMatchesApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LolEsportsMatchesApp.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILeaguesInfoService leaguesService;

        public HomeController(ILeaguesInfoService leaguesService)
        {
            this.leaguesService = leaguesService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<LeagueInfo> leagues = await this.leaguesService.ShowLeagues();
            IEnumerable<LeagueModel> leaguesModel =
                from l in leagues
                select new LeagueModel
                {
                    Id = l.Id,
                    Slug = l.Slug,
                    Name = l.Name,
                    Image = l.Image,
                };

            return View(leaguesModel);
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? code = null)
        {
            if (code.HasValue)
            {
                return View("Code", code.Value);
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}