using FrontKFHShortcuts.Models;
using FrontKFHShortcuts.Models.Catalog;
using FrontKFHShortcuts.Models.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
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
            return View(new List<ProductResponse>());
        }

        public async Task<IActionResult> Create()
        {
            var categories = await GetCategories();
            ViewBag.Categories = new SelectList(categories, "Name", "Name");
            return View();
        }

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
            ViewBag.Categories = new SelectList(categories, "Name", "Name", product.CategoryName);
            return View(productRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(int id, ProductRequest product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var client = MyState.createClient();
                    var response = await client.PatchAsJsonAsync($"Admin/EditProduct?Id={id}", product);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                var categories = await GetCategories();
                ViewBag.Categories = new SelectList(categories, "Name", "Name", product.CategoryName);
                return View(product);
            }
            catch
            {
                var categories = await GetCategories();
                ViewBag.Categories = new SelectList(categories, "Name", "Name", product.CategoryName);
                return View(product);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Delete(ProductResponse product)
        {
            return View(product);
        }

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
        public async Task<IActionResult> Details(ProductResponse product)
        {
            return View(product);
        }

        public async Task<IActionResult> ProductDetails(string productName)
        {
            var client = MyState.createClient();
            var response = await client.GetAsync($"Admin/Product?product={productName}");
            if(response.IsSuccessStatusCode) 
            {
                var product = await response.Content.ReadFromJsonAsync<ProductResponse>();
                return View("Details", product);
            }
            return RedirectToAction("Index");
        }
    }
    }
