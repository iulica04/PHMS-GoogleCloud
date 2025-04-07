using Domain.Common;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PHMS.Controllers
{
    public interface IAuthorizationManager
    {
        public static Result<Unit> EnsureProperAuthorization(string requestHeadersAuthorization, string secretKey, Guid requestId, List<string>? allowedRoles = null)
        {
            if (string.IsNullOrEmpty(requestHeadersAuthorization))
            {
                return Result<Unit>.Failure("Authorization header is missing");
            }

            var token = requestHeadersAuthorization.Replace("Bearer ", "");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            var requesterId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var requesterRole = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (requesterId is null || requesterRole is null)
            {
                return Result<Unit>.Failure("Invalid or expired token");
            }

            if (requesterId != requestId.ToString() && (allowedRoles is null || !allowedRoles!.Contains(requesterRole!)))
            {
                return Result<Unit>.Failure("You are not authorized to perform this action");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
