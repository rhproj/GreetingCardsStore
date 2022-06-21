using GCard.DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GCard.RazorPagesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemTypeController : Controller
    {
        private readonly IRepositoryService _repoService;
        public ItemTypeController(IRepositoryService repoService)
        {
            _repoService = repoService;
        }

        #region API
        [HttpGet]
        public IActionResult GetAll()
        {
            var itemTypeList = _repoService.ItemTypeRepository.GetAll();
            return Json(new { data = itemTypeList });
        }
        #endregion


        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    var mIfromDb = _repoWrapper.MenuItemRepo.GetWithCondition(m => m.Id == id);
        //    //удаляя запись картинка к нему больше не нужна - удаляем:
        //    var oldImgFile = Path.Combine(_hostEnvironment.WebRootPath, mIfromDb.Image.TrimStart('\\'));
        //    if (System.IO.File.Exists(oldImgFile))
        //    {
        //        System.IO.File.Delete(oldImgFile);
        //    }

        //    _repoWrapper.MenuItemRepo.Delete(mIfromDb);
        //    _repoWrapper.Save();

        //    return Json(new { success = true, message = "Успешно удалено" }); //анонимный класс?
        //}
    }
}
