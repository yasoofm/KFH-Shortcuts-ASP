using BackKFHShortcuts.Models;
using BackKFHShortcuts.Models.Entities;
using BackKFHShortcuts.Models.Request;
using BackKFHShortcuts.Models.Responses;
using Microsoft.AspNetCore.Authorization;
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
                return Ok(await context.Categories.Where(x => x.IsDeleted == false).Select(x => new GetCategoryResponse { Id = x.Id, Name = x.Name }).ToListAsync());
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
                    .Where(x => x.Category.Name == category && x.IsDeleted == false)
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

        [HttpPost("CreateProductRequest")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> CreateProductRequest(RequestProductRequest request)
        {
            using (var context = _context)
            {
                var userClaim = User.FindFirst("userId");
                if (userClaim == null)
                {
                    throw new InvalidOperationException();
                }
                var userId = int.Parse(userClaim.Value);

                var product = await context.Products.FindAsync(request.ProductId);
                if(product == null)
                {
                    return NotFound();
                }

                var user = await context.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                _ = await context.ProductRequests.AddAsync(new ProductRequest
                {
                    ClientName = request.ClientName,
                    ClientNumber = request.ClientNumber,
                    Product = product,
                    User = user
                });
                await context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}
