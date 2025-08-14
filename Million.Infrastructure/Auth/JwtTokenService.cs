using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Million.Application.Ports.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Million.Infrastructure.Auth
{
    /// <summary>Opciones de JWT (bound desde appsettings)</summary>
    public sealed class JwtOptions
    {
        public string Issuer { get; set; } = "MillionState";
        public string Audience { get; set; } = "MillionState.Api";
        public string SigningKey { get; set; } = "REPLACE_WITH_A_LONG_RANDOM_SECRET";
        public int ExpMinutes { get; set; } = 60;
    }

    /// <summary>Servicio de generación de tokens (Singleton)</summary>
    public sealed class JwtTokenService : IJwtTokenService
    {
        private readonly JwtOptions _options;
        private readonly SigningCredentials _creds;

        public JwtTokenService(IOptions<JwtOptions> options)
        {
            _options = options.Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
            _creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }

        public string GenerateToken(string username, IEnumerable<string> roles, TimeSpan? lifetime = null)
        {
            var now = DateTime.UtcNow;
            var expires = now.Add(lifetime ?? TimeSpan.FromMinutes(_options.ExpMinutes));

            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Name, username)
        };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: _creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}