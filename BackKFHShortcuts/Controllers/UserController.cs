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

        // GET: User/GetCategory
        [ProducesResponseType(typeof(List<GetCategoryResponse>), 200)]
        [HttpGet("GetCategory")]
        public async Task<ActionResult<IEnumerable<GetCategoryResponse>>> getCategories()
        {
            using(var context = _context)
            {
                return Ok(await context.Categories.Where(x => x.IsDeleted == false).Select(x => new GetCategoryResponse { Id = x.Id, Name = x.Name }).ToListAsync());
            }
        }

        // GET: User/GetProduct?category=...
        [ProducesResponseType(typeof(List<GetProductResponse>), 200)]
        [HttpGet("GetProduct")]
        public async Task<ActionResult<IEnumerable<GetProductResponse>>> getProducts(string category)
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

        // POST: User/CreateProductRequest
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

                user.Points += product.AwardedPoints;

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

        // POST: User/RewardRequest?Id=...
        [HttpPost("RewardRequest")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> RewardRequest(int Id)
        {
            using(var context = _context)
            {
                var userClaim = User.FindFirst("userId");
                if (userClaim == null)
                {
                    throw new InvalidOperationException();
                }

                var userId = int.Parse(userClaim.Value);
                var user = await context.Users.FindAsync(userId);
                if(user == null)
                {
                    return NotFound();
                }

                var reward = await context.Rewards.FindAsync(Id);
                if (reward == null)
                {
                    return NotFound();
                }

                if(reward.Usages <= 0)
                {
                    return BadRequest();
                }

                if(user.Points < reward.RequiredPoints)
                {
                    return BadRequest();
                }

                reward.Usages--;
                user.Points -= reward.RequiredPoints;

                _ = await context.RewardRequests.AddAsync(new RewardRequest
                {
                    ClaimedAt = DateTime.UtcNow,
                    Reward = reward,
                    User = user
                });
                await context.SaveChangesAsync();
                return NoContent();
            }
        }

        // GET: User/GetReward
        [HttpGet("GetReward")]
        [ProducesResponseType(typeof(List<GetRewardResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<GetRewardResponse>>> GetRewards()
        {
            using (var context = _context)
            {
                return Ok(await context.Rewards.Where(x => !x.IsDeleted).Select(x => new GetRewardResponse
                {
                    DueDate = x.DueDate,
                    Id = x.Id,
                    RequiredPoints = x.RequiredPoints,
                    Title = x.Title,
                    Usages = x.Usages
                }).ToListAsync());
            }
        }
    }
}
