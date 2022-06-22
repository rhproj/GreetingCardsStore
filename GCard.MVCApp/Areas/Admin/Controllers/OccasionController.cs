using GCard.DataAccess.Repository;
using GCard.Model;
using Microsoft.AspNetCore.Mvc;

namespace GCard.MVCApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OccasionController : Controller
    {
        private readonly IRepositoryService _repoService;
        public OccasionController(IRepositoryService repoService)
        {
            _repoService = repoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            Occasion occasion = new();
            if (id == null || id == 0)
            {
                return View(occasion);
            }
            else
            {
                occasion = _repoService.OccasionRepository.GetWithCondition(i => i.Id == id);
                return View(occasion);
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Occasion occasion)
        {
            if (occasion.Id == 0)
            {
                _repoService.OccasionRepository.Add(occasion);
                TempData["success"] = "New Occasion successfully added";
            }
            else
            {
                _repoService.OccasionRepository.Update(occasion);
                TempData["success"] = "Occasion successfully updated";
            }
            return RedirectToAction("Index");
        }

        #region API
        [HttpGet]
        public IActionResult GetAll()
        {
            var occasionsList = _repoService.OccasionRepository.GetAll();
            return Json(new { data = occasionsList });
        }
        #endregion
    }
}
