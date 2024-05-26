using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackKFHShortcuts.Models;
using BackKFHShortcuts.Models.Entities;
using BackKFHShortcuts.Models.Request;
using BackKFHShortcuts.Models.Responses;

namespace BackKFHShortcuts.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ShortcutsContext _context;

        public AdminController(ShortcutsContext context)
        {
            _context = context;
        }

        // GET: Admin/GetCategory
        [ProducesResponseType(typeof(Category), 200)]
        [HttpGet("GetCategory")]
        public async Task<ActionResult<IEnumerable<GetCategoryResponse>>> getCategories()
        {
            using (var context = _context)
            {
                return Ok(await context.Categories.Where(x => x.IsDeleted == false).Select(x => new GetCategoryResponse { Id = x.Id, Name = x.Name}).ToListAsync());
            }   
        }

        // GET: Admin/GetProduct?category=...
        [ProducesResponseType(typeof(Product), 200)]
        [HttpGet("GetProduct")]
        public async Task<ActionResult<IEnumerable<Product>>> getProducts()
        {
            using (var context = _context)
            {
                return Ok(await context.Products
                    .Include(x => x.Category)
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new GetProductResponse
                {
                        Id = x.Id,
                        Name = x.Name,
                        Image = x.Image,
                        Shariah = x.Sharia,
                        TargetAudience = x.TargetAudience,
                        Description = x.Description,
                        CategoryName = x.Category.Name,
                        AwardedPoints = x.AwardedPoints,
                }).ToListAsync());
            }
        }

        // POST: Admin/AddCategory
        [HttpPost("AddCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddCategory(AddCategoryRequest request)
        {
            using(var context = _context)
            {
                await context.Categories.AddAsync(new Category { Name = request.Name, });
                await context.SaveChangesAsync();
                return Created();
            }
        }

        // POST: Admin/AddProduct
        [HttpPost("AddProduct")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddProduct(AddProductRequest request)
        {
            using (var context = _context)
            {
                var category = await context.Categories.Where(x => x.Name == request.CategoryName && x.IsDeleted == false).SingleOrDefaultAsync();
                if(category == null) 
                {
                    return NotFound();
                }
                await context.Products.AddAsync(new Product 
                { 
                    Name = request.Name,
                    AwardedPoints = request.AwardedPoints,
                    Category = category, Description = request.Description,
                    Image = request.Image, Sharia = request.Shariah,
                    TargetAudience = request.TargetAudience 
                });
                await context.SaveChangesAsync();
                return Created();
            }
        }

        // DELETE: Admin/RemoveCategory?Id=...
        [HttpDelete("RemoveCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RemoveCategory(int Id)
        {
            using( var context = _context)
            {
                var category = await context.Categories.FindAsync(Id);
                if(category == null)
                {
                    return NotFound();
                }
                category.IsDeleted = true;
                await context.SaveChangesAsync();
                return Ok();
            }
        }

        // DELETE: Admin/RemoveCategory?Id=...
        [HttpDelete("RemoveProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RemoveProduct(int Id)
        {
            using (var context = _context)
            {
                var product = await context.Products.FindAsync(Id);
                if (product == null)
                {
                    return NotFound();
                }
                product.IsDeleted = true;
                await context.SaveChangesAsync();
                return Ok();
            }
        }

        // PATCH: Admin/EditProduct?Id=...
        [HttpPatch("EditProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> EditProduct(int Id, EditProductRequest request)
        {
            using(var context = _context)
            {
                var product = await context.Products.FindAsync(Id);
                if (product == null)
                {
                    return NotFound();
                }

                if(request.Name != null)
                {
                    product.Name = request.Name;
                }
                if (request.Description != null)
                {
                    product.Description = request.Description;
                }
                if(request.AwardedPoints != null)
                {
                    product.AwardedPoints = (int)request.AwardedPoints;
                }
                if(request.TargetAudience != null)
                {
                    product.TargetAudience = request.TargetAudience;
                }
                if(request.Image != null)
                {
                    product.Image = request.Image;
                }
                if(request.Shariah != null)
                {
                    product.Sharia = request.Shariah;
                }
                if(request.CategoryName != null)
                {
                    var newCategory = await context.Categories.Where(x => x.Name == request.CategoryName).FirstOrDefaultAsync();
                    if (newCategory != null)
                    {
                        return NotFound();
                    }
                    product.Category = newCategory;
                }

                await context.SaveChangesAsync();
                return Ok();
            }
        }

        // PATCH: Admin/EditCategory?Id=...
        [HttpPatch("EditCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> EditCategory(int Id, EditCategoryRequest request)
        {
            using (var context = _context)
            {
                var category = await context.Categories.FindAsync(Id);
                if (category == null)
                {
                    return NotFound();
                }

                if (request.Name != null)
                {
                    category.Name = request.Name;
                }

                await context.SaveChangesAsync();
                return Ok();
            }
        }

        [ProducesResponseType(typeof(List<ProductRequestResponse>), StatusCodes.Status200OK)]
        [HttpGet("GetProductRequest")]
        public async Task<ActionResult<List<ProductRequestResponse>>> GetProductRequest()
        {
            using (var context = _context)
            {
                var productRequests = context.ProductRequests.Include(x => x.Product).Include(x => x.User).Select(x => new ProductRequestResponse
                {
                    Id = x.Id,
                    ClientName = x.ClientName,
                    ClientNumber = x.ClientNumber,
                    NumberOfPoints = x.Product.AwardedPoints,
                    ProductId = x.Product.Id,
                    ProductTitle = x.Product.Name,
                    EmployeeId = x.User.Id,
                    EmployeeName = $"{x.User.FirstName} {x.User.LastName}",
                }).ToList();
                return Ok(productRequests);
            }
        }

        // GET: api/Admin/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }
            return category;
        }

        // PUT: api/Admin/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Admin
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        // DELETE: api/Admin/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
