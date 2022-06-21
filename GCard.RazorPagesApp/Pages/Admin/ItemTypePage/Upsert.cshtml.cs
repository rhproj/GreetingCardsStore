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

        public void OnGet(int? id) 
        {
            if (id != null) 
            {
                ItemType = _repoService.ItemTypeRepository.GetWithCondition(i => i.Id == id);
            }
        }

        public IActionResult OnPost()
        {
            if (ItemType.Id == 0)
            {
                _repoService.ItemTypeRepository.Add(ItemType);
                TempData["success"] = "New Type successfully added";
            }
            else
            {
                _repoService.ItemTypeRepository.Update(ItemType);
                TempData["success"] = "Type successfully updated";
            }

            return RedirectToPage("./Index");
        }
    }
}
