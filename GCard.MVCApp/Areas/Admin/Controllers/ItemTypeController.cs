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
                itemType = _repoService.ItemTypeRepository.GetWithCondition(i=>i.Id == id);    //ProductRepo.GetWithCondition(p => p.Id == id); ;
                return View(itemType);
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ItemType itemType)
        {
            _repoService.ItemTypeRepository.Add(itemType);
            _repoService.Save();
            TempData["success"] = "Item type added succesfully";
            return RedirectToAction("Index");

            //if (ModelState.IsValid)
            //{

            //}
            //return View(itemType);
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
