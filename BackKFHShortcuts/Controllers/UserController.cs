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
        private readonly IConfiguration _configuration;

        public UserController(ShortcutsContext shortcutsContext, IConfiguration config)
        {
            _configuration = config;
            _context = shortcutsContext;
        }

        // GET: User/GetCategory
        [ProducesResponseType(typeof(List<GetCategoryResponse>), 200)]
        [HttpGet("GetCategory")]
        public async Task<ActionResult<IEnumerable<GetCategoryResponse>>> getCategories()
        {
            using (var context = _context)
            {
                return Ok(await context.Categories.Where(x => x.IsDeleted == false).Select(x => new GetCategoryResponse { Id = x.Id, Name = x.Name }).ToListAsync());
            }
        }

        // GET: User/GetProduct?category=...
        [ProducesResponseType(typeof(List<GetProductResponse>), 200)]
        [HttpGet("GetProduct")]
        public async Task<ActionResult<IEnumerable<GetProductResponse>>> getProducts(string category)
        {
            using (var context = _context)
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
                if (product == null)
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
                    User = user,
                    CreateAt = DateTime.UtcNow,
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
            using (var context = _context)
            {
                var userClaim = User.FindFirst("userId");
                if (userClaim == null)
                {
                    throw new InvalidOperationException();
                }

                var userId = int.Parse(userClaim.Value);
                var user = await context.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                var reward = await context.Rewards.FindAsync(Id);
                if (reward == null)
                {
                    return NotFound();
                }

                if (reward.Usages <= 0)
                {
                    return BadRequest();
                }

                if (user.Points < reward.RequiredPoints)
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

        // GET: User/Points
        [HttpGet("Points")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult> GetPoints()
        {
            using (var context = _context)
            {
                var userClaim = User.FindFirst("userId");
                if (userClaim == null)
                {
                    throw new InvalidOperationException();
                }

                var userId = int.Parse(userClaim.Value);
                var user = await context.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(new { Points = user.Points });
            }
        }

        // GET: User/RequestHistory
        [HttpGet("RequestHistory")]
        [ProducesResponseType(typeof(List<ProductHistoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<ActionResult<List<ProductHistoryResponse>>> RewardHistory()
        {
            using(var context = _context)
            {
                var userClaim = User.FindFirst("userId");
                if (userClaim == null)
                {
                    throw new InvalidOperationException();
                }

                var userId = int.Parse(userClaim.Value);
                var user = await context.Users.Include(x => x.ProductRequests).ThenInclude(x => x.Product).Where(x => x.Id == userId).FirstOrDefaultAsync();
                if (user == null)
                {
                    return NotFound();
                }

                var History = user.ProductRequests.Select(x => new ProductHistoryResponse { Points = x.Product.AwardedPoints, ProductName = x.Product.Name });
                return Ok(History);
            }
        }

        // POST: User/Chatbot
        [HttpPost("Chatbot")]
        [ProducesResponseType(typeof(OpenAIResponse), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<ChatbotResponse>> Chatbot(ChatbotRequest request)
        {
            using(var context = _context)
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configuration["OPENAI_APIKEY"]}");

                var userClaim = User.FindFirst("userId");
                if (userClaim == null)
                {
                    throw new InvalidOperationException();
                }

                var userId = int.Parse(userClaim.Value);
                var user = await context.Users.Include(x => x.Messages).Where(x => x.Id == userId).FirstOrDefaultAsync();
                if (user == null)
                {
                    return NotFound();
                }

                user.Messages.Add(new Message { Role = "user", Content = request.Message, User = user });
                await context.SaveChangesAsync();
 
                var Messages = user.Messages.Select(x => new MessageModel { Role = x.Role, Content = x.Content }).ToList();
                var response = await client.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", new OpenAIRequest
                {
                    Messages = Messages,
                    Model = "gpt-4o"
                });

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
                    if (result != null)
                    {
                        var Choice = result.Choices.FirstOrDefault();
                        if (Choice != null)
                        {
                            user.Messages.Add(new Message { Role = Choice.Message.Role, Content = Choice.Message.Content, User = user});
                            await context.SaveChangesAsync();

                            return Ok(new ChatbotResponse { Message = Choice.Message.Content, Role = Choice.Message.Role });
                        }       
                    }
                }
                return BadRequest(response.StatusCode);
            }
        }
    }
}
