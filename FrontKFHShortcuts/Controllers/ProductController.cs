using FrontKFHShortcuts.Models.Catalog;
using FrontKFHShortcuts.Models.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace FrontKFHShortcuts.Controllers
{
    public class ProductController : Controller
    {
        private static List<CategoryResponse> categories = new List<CategoryResponse>
        {
            new CategoryResponse { Id = 1, Name = "Beauty" },
            new CategoryResponse { Id = 2, Name = "Grocery" },
            new CategoryResponse { Id = 3, Name = "Food" },
            new CategoryResponse { Id = 4, Name = "Furniture" },
            new CategoryResponse { Id = 5, Name = "Shoes" },
            new CategoryResponse { Id = 6, Name = "Frames" },
            new CategoryResponse { Id = 7, Name = "Jewellery" }
        };

        private static List<ProductResponse> products = new List<ProductResponse>
        {
            new ProductResponse { Id = 1, Name = "Organic Cream", Image = "path/to/image1.jpg", Shariah = "Compliant", TargetAudience = "Adults", Description = "Organic cream for beauty", CategoryName = "Beauty" , AwardedPoints=100},
            new ProductResponse { Id = 2, Name = "Rain Umbrella", Image = "path/to/image2.jpg", Shariah = "Non-compliant", TargetAudience = "All", Description = "Umbrella for rainy days", CategoryName = "Grocery", AwardedPoints=50},
            // Add more products as needed
        };

        // GET: Products
        public ActionResult Index()
        {
           ViewBag.Categories = categories;
            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.Categories = new SelectList(categories, "Name", "Name");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        public ActionResult Create(ProductRequest request)
        {
            if (ModelState.IsValid)
            {
                var newProduct = new ProductResponse
                {
                    Id = products.Count + 1,
                    Name = request.Name,
                    Image = request.Image,
                    Shariah = request.Shariah ?? "Unknown",
                    TargetAudience = request.TargetAudience ?? "Unknown",
                    Description = request.Description ?? "No description",
                    CategoryName = request.CategoryName,
                    AwardedPoints = request.AwardedPoints
                };

                products.Add(newProduct);
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(categories, "Name", "Name");
            return View(request);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int id)
        {
            var product = products.Find(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var request = new ProductRequest
            {
                Name = product.Name,
                Image = product.Image,
                Shariah = product.Shariah,
                TargetAudience = product.TargetAudience,
                Description = product.Description,
                CategoryName = product.CategoryName,
                AwardedPoints = product.AwardedPoints
            };

            ViewBag.Categories = new SelectList(categories, "Name", "Name", product.CategoryName);
            return View(request);
        }

        // POST: Products/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ProductRequest request)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = products.Find(p => p.Id == id);
                if (existingProduct != null)
                {
                    existingProduct.Name = request.Name;
                    existingProduct.Image = request.Image;
                    existingProduct.Shariah = request.Shariah;
                    existingProduct.TargetAudience = request.TargetAudience;
                    existingProduct.Description = request.Description;
                    existingProduct.CategoryName = request.CategoryName;
                    existingProduct.AwardedPoints = request.AwardedPoints;
                }
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(categories, "Name", "Name", request.CategoryName);
            return View(request);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int id)
        {
            var product = products.Find(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = products.Find(p => p.Id == id);
            if (product != null)
            {
                products.Remove(product);
            }
            return RedirectToAction("Index");
        }


        public ActionResult EditProduct(int id)
        {
            var product = products.Find(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var request = new ProductRequest
            {
                Name = product.Name,
                Image = product.Image,
                Shariah = product.Shariah,
                TargetAudience = product.TargetAudience,
                Description = product.Description,
                CategoryName = product.CategoryName,
                AwardedPoints =product.AwardedPoints
            };

            ViewBag.Categories = new SelectList(categories, "Name", "Name", product.CategoryName);
            return View(request);
        }

        // POST: Products/Edit/5
        [HttpPost]
        public ActionResult EditProduct(int id, ProductRequest request)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = products.Find(p => p.Id == id);
                if (existingProduct != null)
                {
                    existingProduct.Name = request.Name;
                    existingProduct.Image = request.Image;
                    existingProduct.Shariah = request.Shariah;
                    existingProduct.TargetAudience = request.TargetAudience;
                    existingProduct.Description = request.Description;
                    existingProduct.CategoryName = request.CategoryName;
                    existingProduct.AwardedPoints = request.AwardedPoints;
                }
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(categories, "Name", "Name", request.CategoryName);
            return View(request);
        }


        public ActionResult Details(int id)
        {

            var product = products.Find(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
                return View(product);
        }
    }
}