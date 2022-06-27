using GCard.DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GCard.RazorPagesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataTableController : Controller
    {
        private readonly IRepositoryService _repoService;
        private readonly IWebHostEnvironment _hostEnvironment;
        public DataTableController(IRepositoryService repoService, IWebHostEnvironment hostEnvironment)
        {
            _repoService = repoService;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet("getAllProducts")]
        public IActionResult GetAllProductItems()
        {
            var productList = _repoService.ProductItemRepository.GetAll(includeProp: "ItemType,Occasion");
            return Json(new { data = productList });
        }

        [HttpGet("getAllTypes")]
        public IActionResult GetAllItemTypes()
        {
            var itemTypeList = _repoService.ItemTypeRepository.GetAll();
            return Json(new { data = itemTypeList });
        }

        [HttpGet("getAllOccasions")]
        public IActionResult GetAllOccasions()
        {
            var occasionsList = _repoService.OccasionRepository.GetAll();
            return Json(new { data = occasionsList });
        }

        [HttpDelete("deleteProduct/{id}")]
        public IActionResult DeleteProductItem(int? id)
        {
            var productItem = _repoService.ProductItemRepository.GetWithCondition(i => i.Id == id);
            if (productItem == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            if (productItem.Image != null)
            {
                var oldImgFile = Path.Combine(_hostEnvironment.WebRootPath, productItem.Image.TrimStart('\\'));
                if (System.IO.File.Exists(oldImgFile))
                {
                    System.IO.File.Delete(oldImgFile);
                }
            }

            _repoService.ProductItemRepository.Delete(productItem);
            return Json(new { success = true, message = "Deleted successfully" });
        }

        [HttpDelete("deleteItemType/{id}")]
        public IActionResult DeleteItemType(int? id)
        {
            var itemType = _repoService.ItemTypeRepository.GetWithCondition(i => i.Id == id);
            if (itemType == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _repoService.ItemTypeRepository.Delete(itemType);

            return Json(new { success = true, message = "Deleted successfully" });
        }

        [HttpDelete("deleteOccasion/{id}")]
        public IActionResult DeleteOccasion(int? id)
        {
            var occasion = _repoService.OccasionRepository.GetWithCondition(i => i.Id == id);
            if (occasion == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _repoService.OccasionRepository.Delete(occasion);
            return Json(new { success = true, message = "Deleted successfully" });
        }
    }
}
