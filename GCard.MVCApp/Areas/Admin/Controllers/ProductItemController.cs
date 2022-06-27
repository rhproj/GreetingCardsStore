using GCard.DataAccess.Repository;
using GCard.Model;
using GCard.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCard.MVCApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductItemController : Controller
    {
        private readonly IRepositoryService _repoService;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductItemController(IRepositoryService repoService, IWebHostEnvironment hostEnvironment)
        {
            _repoService = repoService;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            ProductItemVM productItemVM = new()
            {
                ProductItem = new(),
                ItemTypeList = _repoService.ItemTypeRepository.GetAll().Select(i => new SelectListItem() { Text = i.Name, Value = i.Id.ToString() }),
                OccasionList = _repoService.OccasionRepository.GetAll().Select(i => new SelectListItem() { Text = i.Name, Value = i.Id.ToString() }),
            };

            if (id == null || id == 0)
            {
                return View(productItemVM);
            }
            else //update
            {
                productItemVM.ProductItem = _repoService.ProductItemRepository.GetWithCondition(i=>i.Id == id);
                return View(productItemVM);
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductItemVM productItemVM)
        {
            var webRootPath = _hostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            if (files.Count>0)
            {
                var fileNameNew = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webRootPath, @"img\productItems");
                var extension = Path.GetExtension(files[0].FileName);

                if (productItemVM.ProductItem.Image != null)
                {
                    var oldImgFile = Path.Combine(webRootPath, productItemVM.ProductItem.Image.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImgFile))
                    {
                        System.IO.File.Delete(oldImgFile);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(uploads, fileNameNew + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                productItemVM.ProductItem.Image = @"\img\productItems\" + fileNameNew + extension; //update url
            }

            if (productItemVM.ProductItem.Id == 0)
            {
                _repoService.ProductItemRepository.Add(productItemVM.ProductItem);
                TempData["success"] = "New Product added successfully";
            }
            else 
            {
                _repoService.ProductItemRepository.Update(productItemVM.ProductItem);
                TempData["success"] = "New Product updated successfully";
            }

            return RedirectToAction("Index");
        }

        #region API
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _repoService.ProductItemRepository.GetAll(includeProp: "ItemType,Occasion");
            return Json(new { data = productList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
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
        #endregion
    }
}
