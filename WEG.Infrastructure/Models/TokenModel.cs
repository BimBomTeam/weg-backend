using System.IdentityModel.Tokens.Jwt;

namespace WEG.Infrastructure.Models
{
    public class TokenModel
    {
        public JwtSecurityToken? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
