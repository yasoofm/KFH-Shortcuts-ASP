using BackKFHShortcuts.Models.Responses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackKFHShortcuts.Models.Authentication
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ShortcutsContext _shortcutsContext;

        public TokenService(IConfiguration configuration, ShortcutsContext shortcutsContext)
        {
            _shortcutsContext = shortcutsContext;
            _configuration = configuration;
        }

        public (bool IsValid, LoginResponse Response) GenerateToken(string email, string password)
        {
            var user = _shortcutsContext.Users.SingleOrDefault(x => x.Email == email);
            if (user == null)
            {
                return (false, new LoginResponse());
            }

            var validPassword = BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password);
            if (!validPassword)
            {
                return (false, new LoginResponse());
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("email", email),
                new Claim("userid", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);
            var generatedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return (true, new LoginResponse { Token = generatedToken, FirstName = user.FirstName, LastName = user.LastName, KFH_Id = user.KFH_Id });
        }
    }
}
