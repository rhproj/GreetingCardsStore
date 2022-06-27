using GCard.DataAccess.Repository;
using GCard.Model;
using GCard.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCard.RazorPagesApp.Pages.Admin.ProductItemPage
{
    public class UpsertModel : PageModel
    {
        private readonly IRepositoryService _repoService;
        private readonly IWebHostEnvironment _hostEnvironment;
        [BindProperty]
        public ProductItemVM ProductItemVM { get; set; }
        public UpsertModel(IRepositoryService repoService, IWebHostEnvironment hostEnvironment)
        {
            _repoService = repoService;
            _hostEnvironment = hostEnvironment;
            ProductItemVM = new()
            {
                ProductItem = new(),
                ItemTypeList = _repoService.ItemTypeRepository.GetAll().Select(i => new SelectListItem() { Text = i.Name, Value = i.Id.ToString() }),
                OccasionList = _repoService.OccasionRepository.GetAll().Select(i => new SelectListItem() { Text = i.Name, Value = i.Id.ToString() }),
            };
        }

        public void OnGet(int? id) //for edit
        {
            if (id != null)
            {
                ProductItemVM.ProductItem = _repoService.ProductItemRepository.GetWithCondition(i => i.Id == id);
            }
        }

        public IActionResult OnPost() //IFormFile? file) //var files = HttpContext.Request.Form.Files;
        {
            var webRootPath = _hostEnvironment.WebRootPath; 
            var files = HttpContext.Request.Form.Files;

            if (files.Count > 0) //only if we pick an img
            {
                var fileNameNew = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webRootPath, @"img\productItems");
                var extension = Path.GetExtension(files[0].FileName); //renaming keeping same .ext

                if (ProductItemVM.ProductItem.Image != null) //delet if already exists
                {
                    var oldImgFile = Path.Combine(webRootPath, ProductItemVM.ProductItem.Image.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImgFile))
                    {
                        System.IO.File.Delete(oldImgFile);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(uploads, fileNameNew + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                ProductItemVM.ProductItem.Image = @"\img\productItems\" + fileNameNew + extension; //update url
            }

            if (ProductItemVM.ProductItem.Id == 0)
            {
                _repoService.ProductItemRepository.Add(ProductItemVM.ProductItem);
                TempData["success"] = "New Product added successfully";
            }
            else //редактируем сущ-ий
            {
                _repoService.ProductItemRepository.Update(ProductItemVM.ProductItem);
                TempData["success"] = "New Product updated successfully";
            }

            return RedirectToPage("./Index");


            //if (ProductItemVM.ProductItem.Id == 0) //т.е создаем новый
            //{
            //    if (files.Count > 0)
            //    {
            //        var fileNameNew = Guid.NewGuid().ToString();
            //        var uploads = Path.Combine(webRootPath, @"img\productItems");
            //        var extension = Path.GetExtension(files[0].FileName);
            //        using (var fileStream = new FileStream(Path.Combine(uploads, fileNameNew + extension), FileMode.Create))
            //        {
            //            files[0].CopyTo(fileStream);
            //        } //полю дадим значения пути до файла
            //        ProductItemVM.ProductItem.Image = @"img\productItems\" + fileNameNew + extension;
            //    }

            //    _repoService.ProductItemRepository.Add(ProductItemVM.ProductItem);
            //}
            //else //редактируем сущ-ий
            //{
            //    var mIfromDb = _repoService.ProductItemRepository.GetWithCondition(m => m.Id == ProductItemVM.ProductItem.Id);
            //    #region Change Image
            //    if (files.Count > 0) //значит мы выбрали картинку, т.е хотим поменять картинку (ну если она вообще была)
            //    {
            //        string fileNameNew = Guid.NewGuid().ToString();
            //        var uploads = Path.Combine(webRootPath, @"img\productItems");
            //        var extension = Path.GetExtension(files[0].FileName);

            //        if (mIfromDb.Image != null)
            //        {
            //            var oldImgFile = Path.Combine(webRootPath, mIfromDb.Image.TrimStart('\\'));
            //            if (System.IO.File.Exists(oldImgFile)) //если несущ-ет, значит пропускаем и сразу картинку ставим
            //            {
            //                System.IO.File.Delete(oldImgFile);
            //            }
            //        }

            //        using (var fileStream = new FileStream(Path.Combine(uploads, fileNameNew + extension), FileMode.Create))
            //        {
            //            files[0].CopyTo(fileStream);
            //        }
            //        ProductItemVM.ProductItem.Image = @"img\productItems\" + fileNameNew + extension;
            //    }
            //    #endregion
            //    else  //значит оставляем старую картинку (это можно не писать, но мы подстрах-сь)
            //    {
            //        ProductItemVM.ProductItem.Image = mIfromDb.Image;
            //    }
            //    _repoService.ProductItemRepository.Update(ProductItemVM.ProductItem);
            //}
            //files = null;
            //return RedirectToPage("./Index");
        }
    }
}
