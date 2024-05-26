using FrontKFHShortcuts.Models.LogIn;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace FrontKFHShortcuts.Models
{
    public class GlobalAppState
    {
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public void SaveToken(LoginResponse userInfo)
        {
            Token = userInfo.Token;
            FirstName = userInfo.FirstName;
            LastName = userInfo.LastName;
        }

        public void RemoveToken()
        {
            Token = "";
        }

        public HttpClient createClient()
        {
            var client = new HttpClient();
            if (!string.IsNullOrEmpty(Token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            }
            client.BaseAddress = new Uri("https://backkfhshortcuts20240522175847.azurewebsites.net");
            return client;
        }
    }
}