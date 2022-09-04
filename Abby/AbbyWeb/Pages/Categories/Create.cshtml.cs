using AbbyWeb.Data;
using AbbyWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Categories
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
       
        //[BindProperty] //綁定UI中asp-for的物件
        public Category Category { get; set; }

        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(/*Category category*/) //Key word On the Http word that this handler will do 
        {
            if (Category.Name == Category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Category.Name", "名稱和次序不能相同");//自訂驗證錯誤 第一個參數為Key Name會與UI的asp-validation-for欄位做綁定 第二個為錯誤訊息
            }
            if (ModelState.IsValid)
            {
                await _db.Category.AddAsync(Category);
                await _db.SaveChangesAsync();
                TempData["success"] = "Category 新建成功";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
