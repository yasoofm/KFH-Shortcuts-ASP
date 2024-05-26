using FrontKFHShortcuts.Models;
using FrontKFHShortcuts.Models.Catalog;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FrontKFHShortcuts.Controllers
{
    public class CategoryController : Controller
    {
        private readonly GlobalAppState MyState;

        public CategoryController(GlobalAppState state)
        {
            MyState = state;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            var client = MyState.createClient();
            var response = await client.GetAsync("Admin/GetCategory");
            if (response.IsSuccessStatusCode)
            {
                var categories = await response.Content.ReadFromJsonAsync<List<CategoryResponse>>();
                return View(categories);
            }
            return View(null);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryRequest category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var client = MyState.createClient();
                    var response = await client.PostAsJsonAsync("Admin/AddCategory", category);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                return View(category);
            }
            catch
            {
                return View(category);
            }
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(CategoryResponse category)
        {
                var categoryRequest = new CategoryRequest
                {
                    Name = category.Name
                };
                ViewBag.CategoryId = category.Id; // Store the ID to use in the form
                return View(categoryRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryRequest category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var client = MyState.createClient();
                    var response = await client.PatchAsJsonAsync($"Admin/EditCategory?Id={id}", category);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }





        // GET: Category/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(CategoryResponse category)
        {
            return View(category);
        }


        // POST: Category/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var client = MyState.createClient();
                var response = await client.DeleteAsync($"Admin/RemoveCategory?Id={id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}
