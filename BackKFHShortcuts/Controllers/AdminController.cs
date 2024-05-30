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
using Microsoft.IdentityModel.Tokens;
using Humanizer;

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
        [ProducesResponseType(typeof(List<GetCategoryResponse>), 200)]
        [HttpGet("GetCategory")]
        public async Task<ActionResult<IEnumerable<GetCategoryResponse>>> getCategories()
        {
            using (var context = _context)
            {
                return Ok(await context.Categories.Where(x => x.IsDeleted == false).Select(x => new GetCategoryResponse { Id = x.Id, Name = x.Name }).ToListAsync());
            }
        }

        // GET: Admin/GetProduct?category=...
        [ProducesResponseType(typeof(List<GetProductResponse>), 200)]
        [HttpGet("GetProduct")]
        public async Task<ActionResult<IEnumerable<GetProductResponse>>> getProducts()
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
            using (var context = _context)
            {
                await context.Categories.AddAsync(new Category { Name = request.Name, });
                await context.SaveChangesAsync();
                return Created();
            }
        }

        // POST: Admin/AddProduct
        [HttpPost("AddProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddProduct(AddProductRequest request)
        {
            using (var context = _context)
            {
                var category = await context.Categories.Where(x => x.Name == request.CategoryName && x.IsDeleted == false).SingleOrDefaultAsync();
                if (category == null)
                {
                    return NotFound();
                }
                await context.Products.AddAsync(new Product
                {
                    Name = request.Name,
                    AwardedPoints = request.AwardedPoints,
                    Category = category,
                    Description = request.Description,
                    Image = request.Image,
                    Sharia = request.Shariah,
                    TargetAudience = request.TargetAudience
                });
                await context.SaveChangesAsync();
                return NoContent();
            }
        }

        // DELETE: Admin/RemoveCategory?Id=...
        [HttpDelete("RemoveCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RemoveCategory(int Id)
        {
            using (var context = _context)
            {
                var category = await context.Categories.FindAsync(Id);
                if (category == null)
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
            using (var context = _context)
            {
                var product = await context.Products.FindAsync(Id);
                if (product == null)
                {
                    return NotFound();
                }

                if (request.Name != null)
                {
                    product.Name = request.Name;
                }
                if (request.Description != null)
                {
                    product.Description = request.Description;
                }
                if (request.AwardedPoints != null)
                {
                    product.AwardedPoints = (int)request.AwardedPoints;
                }
                if (request.TargetAudience != null)
                {
                    product.TargetAudience = request.TargetAudience;
                }
                if (request.Image != null)
                {
                    product.Image = request.Image;
                }
                if (request.Shariah != null)
                {
                    product.Sharia = request.Shariah;
                }
                if (request.CategoryName != null)
                {
                    var newCategory = await context.Categories.Where(x => x.Name == request.CategoryName).FirstOrDefaultAsync();
                    if (newCategory == null)
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

        // Get: Admin/GetProductRequest
        [ProducesResponseType(typeof(List<ProductRequestResponse>), StatusCodes.Status200OK)]
        [HttpGet("GetProductRequest")]
        public async Task<ActionResult<List<ProductRequestResponse>>> GetProductRequest()
        {
            using (var context = _context)
            {
                var productRequests = await context.ProductRequests.Include(x => x.Product).Include(x => x.User).Select(x => new ProductRequestResponse
                {
                    Id = x.Id,
                    ClientName = x.ClientName,
                    ClientNumber = x.ClientNumber,
                    NumberOfPoints = x.Product.AwardedPoints,
                    ProductId = x.Product.Id,
                    ProductTitle = x.Product.Name,
                    EmployeeId = x.User.Id,
                    EmployeeName = $"{x.User.FirstName} {x.User.LastName}",
                }).ToListAsync();
                return Ok(productRequests);
            }
        }

        // GET: Admin/GetReward
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

        // POST: Admin/AddReward
        [HttpPost("AddReward")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> AddReward(AddRewardRequest request)
        {
            using (var context = _context)
            {
                _ = await context.Rewards.AddAsync(new Reward
                {
                    Title = request.Title,
                    RequiredPoints = request.RequiredPoints,
                    Usages = request.Usages,
                    DueDate = request.DueDate
                });
                await context.SaveChangesAsync();
                return Ok();
            }
        }

        // DELETE: Admin/RemoveReward?Id=...
        [HttpDelete("RemoveReward")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> RemoveReward(int Id)
        {
            using (var context = _context)
            {
                var reward = await context.Rewards.FindAsync(Id);
                if(reward == null)
                {
                    return NotFound();
                }
                reward.IsDeleted = true;
                await context.SaveChangesAsync();
                return Ok();
            }
        }

        // PATCH: Admin/EditReward?Id=...
        [HttpPatch("EditReward")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> EditReward(int Id, EditRewardRequest request)
        {
            using (var context = _context)
            {
                var reward = await context.Rewards.FindAsync(Id);
                if (reward == null)
                {
                    return NotFound();
                }
                if(!request.Title.IsNullOrEmpty())
                {
                    reward.Title = request.Title!;
                }
                if(request.Usages != null)
                {
                    reward.Usages = (int) request.Usages;
                }
                if(request.DueDate != null)
                {
                    reward.DueDate = (DateTime) request.DueDate;
                }
                if(request.RequiredPoints != null)
                {
                    reward.RequiredPoints = (int) request.RequiredPoints;
                }
                await context.SaveChangesAsync();
                return NoContent();
            }
        }

        // GET: Admin/GetRewardRequest
        [HttpGet("GetRewardRequest")]
        [ProducesResponseType(typeof(List<RewardRequestResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<RewardRequestResponse>>> GetRewardRequest()
        {
            using (var context = _context)
            {
                return await context.RewardRequests.Include(x => x.User).Include(x => x.Reward).Select(x => new RewardRequestResponse
                {
                    ClaimedAt = x.ClaimedAt,
                    EmployeeName = $"{x.User.FirstName} {x.User.LastName}",
                    Id = x.Id,
                    RewardName = x.Reward.Title,
                }).ToListAsync();
            }
        }

        // GET: Admin/Dashboard
        [ProducesResponseType(typeof(DashboardResponse), StatusCodes.Status200OK)]
        [HttpGet("Dashboard")]
        public async Task<ActionResult<DashboardResponse>> Dashboard()
        {
            using(var context = _context)
            {
                var TotalRequests = await context.ProductRequests.CountAsync();
                var MonthlyRequests = new List<RequestPerMonth>();

                for(var i = 0; i < 12; i++)
                {
                    var Month = DateTime.Today.AddMonths(-i);
                    var MonthlyRequest = await context.ProductRequests.Where(x => x.CreateAt.Month == Month.Month).CountAsync();
                    MonthlyRequests.Add(new RequestPerMonth { Month = Month.ToString("MMM"), Requests = MonthlyRequest });
                }

                MonthlyRequests.Reverse();

                var ProductRequests = await context.Products
                    .OrderByDescending(x => x.ProductRequests.Count)
                    .Take(10)
                    .Select(x => new RequestPerProduct { Image = x.Image, ProductName = x.Name, Requests = x.ProductRequests.Count })
                    .ToListAsync();

                var TopProducts = ProductRequests.Take(5).ToList();

                var LeastProducts = await context.Products.Include(x => x.ProductRequests)
                    .OrderBy(x => x.ProductRequests.Count)
                    .Take(5)
                    .Select(x => new RequestPerProduct { Image = x.Image, ProductName = x.Name, Requests = x.ProductRequests.Count})
                    .ToListAsync();

                return Ok(new DashboardResponse { 
                    TotalRequests = TotalRequests,
                    MonthlyRequests = MonthlyRequests,
                    ProductRequests = ProductRequests,
                    LeastProducts = LeastProducts,
                    TopProducts = TopProducts
                });
            }
        }

        // GET: Admin/Product?ProductId=...
        [HttpGet("Product")]
        [ProducesResponseType(typeof(GetProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetProduct(int ProductId)
        {
            using(var context = _context)
            {
                var Product = await context.Products.Where(x => x.Id == ProductId).Include(x => x.Category).FirstOrDefaultAsync();
                if (Product == null)
                {
                    return NotFound();
                }
                return Ok(new GetProductResponse
                {
                    AwardedPoints = Product.AwardedPoints,
                    CategoryName = Product.Category.Name,
                    Id = Product.Id,
                    Name = Product.Name,
                    Description = Product.Description,
                    Image = Product.Image,
                    Shariah = Product.Sharia,
                    TargetAudience = Product.TargetAudience
                });
            }
        }
    }
}
