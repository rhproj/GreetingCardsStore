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

        public void OnGet(int? id) //  %^%^
        {
            if (id != null)
            {
                ProductItemVM.ProductItem = _repoService.ProductItemRepository.GetWithCondition(i => i.Id == id);
            }
        }

        public IActionResult OnPost(IFormFile? file) //var files = HttpContext.Request.Form.Files;
        {
            if (ModelState.IsValid)
            {
                var wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    var fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"img\productItems");
                    var extension = Path.GetExtension(file.FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    ProductItemVM.ProductItem.Image = @"\img\productItems\" + fileName + extension;
                }


                if (ProductItemVM.ProductItem.Id == 0)
                {
                    _repoService.ProductItemRepository.Add(ProductItemVM.ProductItem);
                    TempData["success"] = "Product added successfully";
                }
                else
                {
                    _repoService.ProductItemRepository.Update(ProductItemVM.ProductItem);
                    TempData["success"] = "Product updated successfully";
                }


                //if (ProductItemVM.ProductItem.Id == 0)
                //{

                //    //_repoService.ProductItemRepository.Add(ProductItemVM);
                //    TempData["success"] = "New Type successfully added";
                //}
                //else
                //{
                //    //_repoService.ProductItemRepository.Update(ProductItemVM);
                //    TempData["success"] = "Type successfully updated";
                //}
                return RedirectToPage("./Index");
            }

            return new PageResult(); //stay
        }
    }
}
