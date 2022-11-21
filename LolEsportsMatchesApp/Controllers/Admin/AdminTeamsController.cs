using LolEsportsMatchesApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PagedEnumerables;

namespace LolEsportsMatchesApp.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("admin/teams")]
    public class AdminTeamsController : Controller
    {
        private readonly ITeamInfoService teamService;

        private readonly int ElementsOnPageLimit = 50;

        public AdminTeamsController(ITeamInfoService service)
        {
            teamService = service;
        }

        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] string region = "", [FromQuery] string name = "", [FromQuery] string teamId = "")
        {
            ViewData["selectedRegion"] = region;
            ViewData["selectedName"] = name;
            ViewData["selectedTeam"] = teamId;
            ViewData["regionsDrop"] = await this.GetRegionsDrop();

            PageSettings pageSettings = new(page, this.ElementsOnPageLimit);

            IPagedEnumerable<TeamInfo> teams = string.IsNullOrWhiteSpace(teamId)
                ? await this.teamService.ShowTeams(region, name, pageSettings.Offset, pageSettings.Count)
                : await TryGetTeam(teamId);

            IEnumerable<TeamModel> model = teams
                .Select(t => new TeamModel
                {
                    Id = t.Id,
                    Slug = t.Slug,
                    Name = t.Name,
                    Code = t.Code,
                    Region = t.Region,
                    Image = t.Image,
                });

            return View(PagedModel<TeamModel>.CreatePagedModel(model, string.Empty, pageSettings, teams.TotalCount));

            async Task<IPagedEnumerable<TeamInfo>> TryGetTeam(string id)
            {
                TeamInfo? result = await this.teamService.GetTeam(id);
                return result is null
                    ? PagedQueryable<TeamInfo>.Empty()
                    : PagedQueryable<TeamInfo>.Single(result);
            }
        }

        [HttpPost("{id}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            _ = await teamService.DeleteTeam(id);
            return RedirectToAction("Index");
        }

        [HttpGet("new")]
        public async Task<IActionResult> New()
        {
            ViewData["regionsDrop"] = await this.GetRegionsDropWithEmptyField();
            return View();
        }

        [HttpPost("new")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(TeamModel team, [FromForm] string? newRegion)
        {
            this.TryAddNewRegion(team, newRegion);

            if (ModelState.IsValid)
            {
                bool result = await teamService.CreateTeam(new TeamInfo
                {
                    Id = team.Id,
                    Slug = team.Slug,
                    Name = team.Name,
                    Code = team.Code,
                    Region = team.Region,
                    Image = team.Image,
                });

                if (result)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", $"Cannot create team with {team.Id} id");
            }

            ViewData["regionsDrop"] = await this.GetRegionsDropWithEmptyField();
            return View();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Update(string id)
        {
            ViewData["regionsDrop"] = await this.GetRegionsDropWithEmptyField();
            TeamInfo? team = await teamService.GetTeam(id);
            if (team is null)
            {
                return NotFound();
            }

            return View(new TeamModel
            {
                Id = team.Id,
                Slug = team.Slug,
                Name = team.Name,
                Code = team.Code,
                Region = team.Region,
                Image = team.Image,
            });
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(TeamModel team, [FromForm] string? newRegion)
        {
            this.TryAddNewRegion(team, newRegion);

            if (ModelState.IsValid)
            {
                bool result = await teamService.UpdateTeam(new TeamInfo
                {
                    Id = team.Id,
                    Slug = team.Slug,
                    Name = team.Name,
                    Code = team.Code,
                    Region = team.Region,
                    Image = team.Image,
                });

                if (result)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Cannot update team.");
            }

            ViewData["regionsDrop"] = await this.GetRegionsDropWithEmptyField();
            return View(team);
        }

        private async Task<List<SelectListItem>> GetRegionsDrop()
        {
            return new List<SelectListItem>
            {
                new SelectListItem{ Text = "Region", Value = "", Disabled= true},
                new SelectListItem{ Text = "All Regions", Value = ""},
                new SelectListItem{ Text = "────────", Value = "separator", Disabled= true},
            }.Concat(await this.GetRegionsDropWithoutDefault()).ToList();
        }

        private async Task<List<SelectListItem>> GetRegionsDropWithEmptyField()
        {
            var result = await this.GetRegionsDropWithoutDefault();
            result.Add(new SelectListItem { Text = "Enter New Region", Value = "" });
            return result;    
        }

        private async Task<List<SelectListItem>> GetRegionsDropWithoutDefault()
        {
            return
                (await this.teamService.ShowRegions())
                .Select(r => new SelectListItem { Text = r, Value = r })
                .ToList();
        }

        private void TryAddNewRegion(TeamModel model, string? newRegion)
        {
            if (string.IsNullOrWhiteSpace(model.Region))
            {
                var value = string.IsNullOrWhiteSpace(newRegion) ? null! : newRegion;
                model.Region = value;
                ModelState["Region"]!.ValidationState = value is null
                    ? Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid
                    : Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            }
        }
    }
}
