using BackKFHShortcuts.Models;
using BackKFHShortcuts.Models.Entities;
using BackKFHShortcuts.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackKFHShortcuts.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ShortcutsContext _context;

        public UserController(ShortcutsContext shortcutsContext)
        {
            _context = shortcutsContext;
        }

        [ProducesResponseType(typeof(Category), 200)]
        [HttpGet("GetCategory")]
        public async Task<ActionResult<IEnumerable<Category>>> getCategories()
        {
            using(var context = _context)
            {
                return Ok(await context.Categories.Select(x => new GetCategoryResponse { Id = x.Id, Name = x.Name }).ToListAsync());
            }
        }

        [ProducesResponseType(typeof(Product), 200)]
        [HttpGet("GetProduct")]
        public async Task<ActionResult<IEnumerable<Product>>> getProducts(string category)
        {
            using( var context = _context)
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

        

    }
}
