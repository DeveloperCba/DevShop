using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace DevShop.WebAPI.Core.Identities;

public interface IAspNetUser
{
    string Name { get; }
    Guid GetUserId();
    bool UserInformedEhLogged(string userId);
    string GetUserEmail();
    string GetUserToken();
    string GetUserRefreshToken();
    bool IsAuthenticated();
    bool HasRole(string role);
    IEnumerable<Claim> GetClaims();
    HttpContext GetHttpContext();
}