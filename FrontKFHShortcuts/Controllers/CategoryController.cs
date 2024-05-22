using FrontKFHShortcuts.Models.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace FrontKFHShortcuts.Controllers
{
        public class CategoryController : Controller
        {
            private static List<CategoryResponse> categories = new List<CategoryResponse>
        {
            new CategoryResponse { Id = 1, Name = "Organic Crown" },
            new CategoryResponse { Id = 2, Name = "Rain Umbrella" },
            new CategoryResponse { Id = 3, Name = "Serva Bottle" },
            new CategoryResponse { Id = 4, Name = "Coffee Beans" },
            new CategoryResponse { Id = 5, Name = "Book Shelves" },
            new CategoryResponse { Id = 6, Name = "Dinner Set" },
            new CategoryResponse { Id = 7, Name = "Nike Shoes" },
            new CategoryResponse { Id = 8, Name = "Computer Glasses" },
            new CategoryResponse { Id = 9, Name = "Alloy Jewel Set" }
        };

            // GET: Category
            public IActionResult Index()
            {
                return View(categories);
            }

            // GET: Category/Create
            public IActionResult Create()
            {
                return View();
            }

            // POST: Category/Create
            [HttpPost]
            public IActionResult Create(CategoryResponse category)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        category.Id = categories.Max(c => c.Id) + 1;
                        categories.Add(category);
                        return RedirectToAction("Index");
                    }

                    return View(category);
                }
                catch
                {
                    return View();
                }
            }

            // GET: Category/Edit/5
            public IActionResult Edit(int id)
            {
                var category = categories.FirstOrDefault(c => c.Id == id);
                return View(category);
            }

            // POST: Category/Edit/5
            [HttpPost]
            public IActionResult Edit(int id, CategoryResponse category)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var existingCategory = categories.FirstOrDefault(c => c.Id == id);
                        existingCategory.Name = category.Name;
                        return RedirectToAction("Index");
                    }

                    return View(category);
                }
                catch
                {
                    return View();
                }
            }

        // GET: Category/Delete/5
        public ActionResult Delete(int id)
        {
            var category = categories.FirstOrDefault(c => c.Id == id);
            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var category = categories.FirstOrDefault(c => c.Id == id);
                categories.Remove(category);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

