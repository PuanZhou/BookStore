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
       
        //[BindProperty] //�j�wUI��asp-for������
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
                ModelState.AddModelError("Category.Name", "�W�٩M���Ǥ���ۦP");//�ۭq���ҿ��~ �Ĥ@�ӰѼƬ�Key Name�|�PUI��asp-validation-for��찵�j�w �ĤG�Ӭ����~�T��
            }
            if (ModelState.IsValid)
            {
                await _db.Category.AddAsync(Category);
                await _db.SaveChangesAsync();
                TempData["success"] = "Category �s�ئ��\";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
