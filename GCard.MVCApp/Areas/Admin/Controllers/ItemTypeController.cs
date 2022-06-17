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
            IEnumerable<ItemType> itemTypeList = _repoService.ItemTypeRepository.GetAll();
            return View(itemTypeList);
        }
        //Get
        public IActionResult Create()
        {
            return View();
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public IActionResult Create(ItemType itemType)
        {
            if (ModelState.IsValid)
            {
                if (itemType.Name.Length < 3)
                {
                    ModelState.AddModelError(string.Empty, "less than 3");
                    return View(itemType);
                }
                _repoService.ItemTypeRepository.Add(itemType);
                _repoService.Save();
                //TempData["success"] = "";
                return RedirectToAction("Index");
            }
            return View(itemType); //stay on that view
        }
    }
}
