using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RecipeApi.Authentication;
using RecipeApi.Database;

namespace RecipeApi.Tools
{
    public interface ITokenHelper
    {
        TokenData GenerateJwtToken(ApplicationUser user, List<string> roles, SignInManager<ApplicationUser> signInManager);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
    public class TokenHelper: ITokenHelper
    {

        private readonly AuthenticationSettings _authenticationSettings;

        public TokenHelper(AuthenticationSettings authenticationSettings)
        {
            _authenticationSettings = authenticationSettings;
        }

        public TokenData GenerateJwtToken(
            ApplicationUser user, List<string> roles, SignInManager<ApplicationUser> signInManager)
        {
            string newRefreshToken = GenerateRefreshToken();

            var claims = new List<Claim>
            {
                // new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                // new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("refreshToken", newRefreshToken),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token");
            claimsIdentity.AddClaims(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            ApplyClaimsForContextUser(claims, signInManager);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.ToUniversalTime().AddMinutes(Convert.ToDouble(_authenticationSettings.JwtExpireMinutes));

            var token = new JwtSecurityToken(
                _authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims: claimsIdentity.Claims,
                expires: expires,
                signingCredentials: creds
            );

            string result = (new JwtSecurityTokenHandler()).WriteToken(token);

            return new TokenData()
            {
                Token = result,
                RefreshToken = newRefreshToken
            };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = _authenticationSettings.JwtIssuer,
                ValidAudience = _authenticationSettings.JwtIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey)),
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        private void ApplyClaimsForContextUser(List<Claim> claims, SignInManager<ApplicationUser> signInManager)
        {
            var identity = new ClaimsIdentity(claims);
            signInManager.Context.User = new ClaimsPrincipal(identity);
        }
    }
}