using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace DevShop.WebAPI.Core.Identities;

public class AspNetUser : IAspNetUser
{
    private readonly IHttpContextAccessor _accessor;

    public AspNetUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public string Name => _accessor.HttpContext.User.Identity.Name;

    public Guid GetUserId()
    {
        return IsAuthenticated() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;
    }

    public string GetUserEmail()
    {
        return IsAuthenticated() ? _accessor.HttpContext.User.GetUserEmail() : string.Empty;
    }

    public string GetUserToken()
    {
        return IsAuthenticated() ? _accessor.HttpContext.User.GetUserToken() : string.Empty;
    }
    public string GetUserRefreshToken()
    {
        return IsAuthenticated() ? _accessor.HttpContext.User.GetUserRefreshToken() : string.Empty;
    }
    public bool IsAuthenticated()
    {
        return _accessor.HttpContext.User.Identity.IsAuthenticated;
    }

    public bool HasRole(string role)
    {
        return _accessor.HttpContext.User.IsInRole(role);
    }

    public IEnumerable<Claim> GetClaims()
    {
        return _accessor.HttpContext.User.Claims;
    }

    public HttpContext GetHttpContext()
    {
        return _accessor.HttpContext;
    }
    public bool UserInformedEhLogged(string userId)
    {
        return GetUserId().ToString() == userId;
    }
}