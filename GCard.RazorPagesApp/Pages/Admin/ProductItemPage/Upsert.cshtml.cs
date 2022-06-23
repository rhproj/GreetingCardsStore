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
                //ProductItemVM = _repoService.ProductItemRepository.GetWithCondition(i => i.Id == id);
            }
        }

        public IActionResult OnPost(IFormFile file)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            if (ProductItemVM.ProductItem.Id == 0)
            {
                var fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"img\productItems");
                var extension = Path.GetExtension(files[0].FileName);

                //_repoService.ProductItemRepository.Add(ProductItemVM);
                TempData["success"] = "New Type successfully added";
            }
            else
            {
                //_repoService.ProductItemRepository.Update(ProductItemVM);
                TempData["success"] = "Type successfully updated";
            }

            return RedirectToPage("./Index");
        }
    }
}
