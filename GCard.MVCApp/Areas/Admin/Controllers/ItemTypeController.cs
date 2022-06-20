using GCard.DataAccess.Repository;
using GCard.Model;
using Microsoft.AspNetCore.Mvc;

namespace GCard.MVCApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ItemTypeController : Controller
    {
        private readonly IRepositoryService _repoService;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ItemTypeController(IRepositoryService repoService, IWebHostEnvironment hostEnvironment)
        {
            _repoService = repoService;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<ItemType> itemTypeList = _repoService.ItemTypeRepository.GetAll();
            return View(itemTypeList);
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            ItemType itemType = new();
            //{
            //    Product = new(),
            //    CategoryList = _repoService.CategoryRepo.GetAll()
            //        .Select(i => new SelectListItem() { Text = i.Name, Value = i.Id.ToString() }),
            //    CoverTypeList = _repoService.CoverTypeRepo.GetAll()
            //        .Select(i => new SelectListItem() { Text = i.Name, Value = i.Id.ToString() })
            //};
            if (id == null || id == 0) //create
            {
                return View(itemType);//NotFound();
            }
            else //update
            {
                itemType = _repoService.ItemTypeRepository.GetWithCondition(i=>i.Id == id);    //ProductRepo.GetWithCondition(p => p.Id == id); ;
                return View(itemType);
            }
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ItemType itemType)
        {
            return NotFound();
        }
    }
}
