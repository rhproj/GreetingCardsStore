using GCard.DataAccess.Repository;
using GCard.Model;
using Microsoft.AspNetCore.Mvc;

namespace GCard.MVCApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ItemTypeController : Controller
    {
        private readonly IRepositoryService _repoService;
        public ItemTypeController(IRepositoryService repoService)
        {
            _repoService = repoService;
        }

        public IActionResult Index()
        {
            //IEnumerable<ItemType> itemTypeList = _repoService.ItemTypeRepository.GetAll();
            //return View(itemTypeList);
            return View();
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            ItemType itemType = new();
            if (id == null || id == 0)
            {
                return View(itemType);//NotFound();
            }
            else //update
            {
                itemType = _repoService.ItemTypeRepository.GetWithCondition(i => i.Id == id);    //ProductRepo.GetWithCondition(p => p.Id == id); ;
                return View(itemType);
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ItemType itemType)
        {
            if (itemType.Id == 0)
            {
                _repoService.ItemTypeRepository.Add(itemType);
                TempData["success"] = "New Type successfully added";
            }
            else
            {
                _repoService.ItemTypeRepository.Update(itemType);
                TempData["success"] = "Type successfully updated";
            }
            return RedirectToAction("Index");
        }

        #region API
        [HttpGet]
        public IActionResult GetAll()
        {
            var itemTypeList = _repoService.ItemTypeRepository.GetAll();
            return Json(new { data = itemTypeList });
        }
        #endregion
    }
}
