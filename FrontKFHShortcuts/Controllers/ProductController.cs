using FrontKFHShortcuts.Models;
using FrontKFHShortcuts.Models.Catalog;
using FrontKFHShortcuts.Models.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FrontKFHShortcuts.Controllers
{
    public class ProductController : Controller
    {
        private readonly GlobalAppState MyState;

        public ProductController(GlobalAppState state)
        {
            MyState = state;
        }

        private async Task<List<CategoryResponse>> GetCategories()
        {
            var client = MyState.createClient();
            var response = await client.GetAsync("Admin/GetCategory");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CategoryResponse>>();
            }
            return new List<CategoryResponse>();
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var client = MyState.createClient();
            var response = await client.GetAsync("Admin/GetProduct");
            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadFromJsonAsync<List<ProductResponse>>();
                if (products != null && products.Any())
                {
                    return View(products);
                }
            }
            // Log or debug information
            System.Diagnostics.Debug.WriteLine("No products found or API call failed.");
            return View(new List<ProductResponse>()); // Return an empty list instead of null
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            var categories = await GetCategories();
            ViewBag.Categories = new SelectList(categories, "Name", "Name");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductRequest product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var client = MyState.createClient();
                    var response = await client.PostAsJsonAsync("Admin/AddProduct", product);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                var categories = await GetCategories();
                ViewBag.Categories = new SelectList(categories, "Name", "Name");
                return View(product);
            }
            catch
            {
                var categories = await GetCategories();
                ViewBag.Categories = new SelectList(categories, "Name", "Name");
                return View(product);
            }
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> EditProduct(ProductResponse product)
        {
            var productRequest = new ProductRequest
            {
                Name = product.Name,
                Image = product.Image,
                Shariah = product.Shariah,
                TargetAudience = product.TargetAudience,
                Description = product.Description,
                CategoryName = product.CategoryName,
                AwardedPoints = product.AwardedPoints
            };
            ViewBag.ProductId = product.Id; // Store the ID to use in the form
            var client = MyState.createClient();
            var categories = await client.GetFromJsonAsync<IEnumerable<CategoryResponse>>("Admin/GetCategory");
            ViewBag.Categories = new SelectList(categories,"Name","Name",product.CategoryName); //(SelectList)
            return View(productRequest);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(int id, ProductRequest product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var client = MyState.createClient();
                    var response = await client.PutAsJsonAsync($"Admin/EditProduct?Id={id}", product);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                var categories = await GetCategories();
                ViewBag.Categories = new SelectList(categories, "Name", "Name", product.CategoryName);
                return View(product);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exceptioon occured while editing product with ID {id}: {ex.Message}");
                var categories = await GetCategories();
                ViewBag.Categories = new SelectList(categories, "Name", "Name", product.CategoryName);
                return View(product);
            }
        }

       
        // POST: Products/Delete/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var client = MyState.createClient();
                var response = await client.DeleteAsync($"Admin/RemoveProduct?Id={id}");
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

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var client = MyState.createClient();
            var response = await client.GetAsync($"Admin/GetProduct?Id={id}");
            if (response.IsSuccessStatusCode)
            {
                var product = await response.Content.ReadFromJsonAsync<ProductResponse>();
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
            return NotFound();
        }
    }
}