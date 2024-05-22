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

        // GET: /Admin/GetCategory
        [ProducesResponseType(typeof(Category), 200)]
        [HttpGet("GetCategory")]
        public async Task<ActionResult<IEnumerable<GetCategoryResponse>>> getCategories()
        {
            using (var context = _context)
            {
                return Ok(await context.Categories.Select(x => new GetCategoryResponse { Id = x.Id, Name = x.Name}).ToListAsync());
            }   
        }

        // GET: /Admin/GetProduct?category=...
        [ProducesResponseType(typeof(Product), 200)]
        [HttpGet("GetProduct")]
        public async Task<ActionResult<IEnumerable<Product>>> getProducts(string category)
        {
            using (var context = _context)
            {
                return Ok(await context.Products
                    .Include(x => x.Category)
                    .Where(x => x.Category.Name == category)
                    .Select(x => new GetProductResponse
                {
                        Id = x.Id,
                        Name = x.Name,
                        Image = x.Image,
                        Shariah = x.Sharia,
                        TargetAudience = x.TargetAudience,
                        Description = x.Description,
                        CategoryName = x.Category.Name,
                }).ToListAsync());
            }
        }

        // POST: /Admin/AddCategory
        [HttpPost("AddCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> AddCategory(AddCategoryRequest request)
        {
            using(var context = _context)
            {
                await context.Categories.AddAsync(new Category { Name = request.Name, });
                context.SaveChanges();
                return Ok();
            }
        }

        // POST: /Admin/AddProduct
        [HttpPost("AddProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AddProduct(AddProductRequest request)
        {
            using (var context = _context)
            {
                var category = await context.Categories.Where(x => x.Name == request.CategoryName).SingleOrDefaultAsync();
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
                context.SaveChanges();
                return Ok();
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
