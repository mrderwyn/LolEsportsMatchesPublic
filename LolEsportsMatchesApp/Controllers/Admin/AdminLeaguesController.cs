using LolEsportsMatchesApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LolEsportsMatchesApp.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("admin/leagues")]
    public class AdminLeaguesController : Controller
    {
        private readonly ILeaguesInfoService leagueService;

        public AdminLeaguesController(ILeaguesInfoService service)
        {
            leagueService = service;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<LeagueInfo> leagues = await leagueService.ShowLeagues();
            IEnumerable<LeagueModel> model = leagues
                .Select(l => new LeagueModel
                {
                    Id = l.Id,
                    Slug = l.Slug,
                    Name = l.Name,
                    Image = l.Image,
                });

            return View(model);
        }

        [HttpPost("{id}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            _ = await leagueService.DeleteLeague(id);
            return RedirectToAction("Index");
        }

        [HttpGet("new")]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost("new")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(LeagueModel league)
        {
            if (ModelState.IsValid)
            {
                bool result = await leagueService.CreateLeague(new LeagueInfo
                {
                    Id = league.Id,
                    Slug = league.Slug,
                    Name = league.Name,
                    Image = league.Image,
                });

                if (result)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", $"Cannot create league with {league.Id} id");
            }

            return View();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Update(string id)
        {
            LeagueInfo? league = await leagueService.GetLeague(id);
            if (league is null)
            {
                return NotFound();
            }

            return View(new LeagueModel
            {
                Id = league.Id,
                Slug = league.Slug,
                Name = league.Name,
                Image = league.Image,
            });
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(LeagueModel league)
        {
            if (ModelState.IsValid)
            {
                bool result = await leagueService.UpdateLeague(new LeagueInfo
                {
                    Id = league.Id,
                    Slug = league.Slug,
                    Name = league.Name,
                    Image = league.Image,
                });

                if (result)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Cannot update legue.");
            }

            return View(league);
        }
    }
}
