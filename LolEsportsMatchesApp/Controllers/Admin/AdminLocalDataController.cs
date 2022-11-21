using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LolEsportsMatchesApp.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("admin/local")]
    public class AdminLocalDataController : Controller
    {
        private readonly ErrorStorage storage;
        private readonly LolDataServicesFactory lolDataFactory;

        public AdminLocalDataController(ErrorStorage storage, LolDataServicesFactory factory)
        {
            this.storage = storage;
            this.lolDataFactory = factory;
        }

        public IActionResult Index()
        {
            List<ErrorInfo> allErrors = this.storage.GetAllErrors();
            return View(allErrors);
        }

        [HttpPost("solve")]
        [ValidateAntiForgeryToken]
        public IActionResult Solve([FromForm] int? id)
        {
            if (id is not null)
            {
                this.storage.RemoveError(id.Value);
            }

            return RedirectToAction("Index");
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateData()
        {
            await this.lolDataFactory.Update();
            return RedirectToAction("Index");
        }
    }
}
