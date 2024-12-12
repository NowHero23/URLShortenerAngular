using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace URLShortenerAngular.Server.Services
{
    public class JwtService
    {
        private string _securekey = "d17cc3117176d87c3aa8d0f2843fd8faa0652300c3da3390a9e61edffbb4562a";// "This_is_super_secretKey"
        public string Generate(int id)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securekey));
            var credantials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credantials);

            var peyload = new JwtPayload(id.ToString(), null, null, null, DateTime.Today.AddDays(1)); //1day
            var securityToken = new JwtSecurityToken(header, peyload);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
        public JwtSecurityToken Varify(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_securekey);
            tokenHandler.ValidateToken(jwt, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false
            },
            out SecurityToken validatedToken);

            return (JwtSecurityToken)validatedToken;
        }
    }
}
