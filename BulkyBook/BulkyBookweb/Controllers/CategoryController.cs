using BulkyBook.Models;
using BulkyBookweb.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookweb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }//DI注入 
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryLis = _db.Categories;
            return View(objCategoryLis);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name","The DisplayOrder can't exactly match the Name");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Category created successfully"; // TempData傳一次性的訊息到前端頁面
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if(id==null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = _db.Categories.Find(id);
            //var categoryFromDbFirst = _db.Categories.FirstOrDefault(c => c.id == id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(c => c.id == id);

            if(categoryFromDb== null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken] //非必要 但在Post Action前加入防止(Cross-Site Request Forgery) 
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The DisplayOrder can't exactly match the Name");//自訂義驗證錯誤訊息 前面的參數是要顯示給頁面的欄位
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //Get
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            
            var categoryFromDb = _db.Categories.Find(id);
            
            if(categoryFromDb== null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }
        //Post

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var categoryFromdb = _db.Categories.Find(id);

            if(categoryFromdb== null)
            {
                return NotFound();
            }
            _db.Categories.Remove(categoryFromdb);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
