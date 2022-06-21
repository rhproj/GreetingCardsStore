using GCard.DataAccess.Repository;
using GCard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GCard.RazorPagesApp.Pages.Admin.ItemTypePage
{
    public class UpsertModel : PageModel
    {
        private readonly IRepositoryService _repoService;
        [BindProperty]
        public ItemType ItemType { get; set; }
        public UpsertModel(IRepositoryService repoService)
        {
            _repoService = repoService;
            ItemType = new();
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            //ItemType itemType = new();
            if (ItemType.Id == 0)
            {
                _repoService.ItemTypeRepository.Add(ItemType);
                _repoService.Save();
                TempData["success"] = "Item type added succesfully";
                return RedirectToAction("Index");
            }
            else //update
            {
                ItemType = _repoService.ItemTypeRepository.GetWithCondition(i => i.Id == ItemType.Id);    //ProductRepo.GetWithCondition(p => p.Id == id); ;
                _repoService.ItemTypeRepository.Update(ItemType);
                _repoService.Save();
                TempData["success"] = "Item type updated succesfully";
                return RedirectToAction("Index");
            }
            //if (ModelState.IsValid)
            //{
            //    _repoService.ItemTypeRepository.Add(ItemType);
            //    _repoService.Save();
            //    TempData["success"] = "Added successfully";
            //    return RedirectToPage("Index");
            //}
            return Page();
        }
    }
}
