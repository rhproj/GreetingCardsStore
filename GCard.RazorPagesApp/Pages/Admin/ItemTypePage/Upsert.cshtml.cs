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
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid) 
            {
                _repoService.ItemTypeRepository.Add(ItemType); 
                _repoService.Save(); 
                //TempData["success"] = "";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
