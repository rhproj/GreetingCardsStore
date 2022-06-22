using GCard.DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GCard.RazorPagesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataTableController : Controller
    {
        private readonly IRepositoryService _repoService;
        public DataTableController(IRepositoryService repoService)
        {
            _repoService = repoService;
        }

        [HttpGet("getAllProducts")]
        public IActionResult GetAllOccasions()
        {
            var occasionsList = _repoService.OccasionRepository.GetAll();
            return Json(new { data = occasionsList });
        }

        [HttpGet]
        public IActionResult GetAll()
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
