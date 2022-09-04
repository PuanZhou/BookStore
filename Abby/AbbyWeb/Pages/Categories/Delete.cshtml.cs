using AbbyWeb.Data;
using AbbyWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        //[BindProperty] //綁定UI中asp-for的物件
        public Category Category { get; set; }

        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int id)
        {
            Category = _db.Category.Find(id);
        }

        public async Task<IActionResult> OnPost() //Key word On the Http word that this handler will do 
        {

            var categoryFromdb = _db.Category.Find(Category.Id);
            if (categoryFromdb != null)
            {
                _db.Category.Remove(categoryFromdb);
                await _db.SaveChangesAsync();
                TempData["success"] = "Category 刪除成功";
                return RedirectToPage("Index");
            }

            return Page();
        }
    }
}
