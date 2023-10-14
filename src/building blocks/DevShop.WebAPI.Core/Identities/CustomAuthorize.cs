using System.Security.Claims;
using DevShop.Core.DomainObjects.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DevShop.WebAPI.Core.Identities;

public class CustomAuthorization
{
    public static bool ValidarClaimsUsuario(HttpContext context, string claimName, string claimValue)
    {
        return context.User.Identity.IsAuthenticated &&
               context.User.Claims.Any(c => c.Type == claimName && c.Value.Contains(claimValue));
    }

    public static bool ValidarRoleUsuario(HttpContext context, string role)
    {
        return context.User.Identity.IsAuthenticated &&
               context.User.IsInRole(role);
    }
}

public class ClaimsAuthorizeAttribute : TypeFilterAttribute
{
    public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequisitoClaimFilter))
    {
        Arguments = new object[] { new Claim(claimName, claimValue) };
    }
}


public class RequisitoClaimFilter : IAuthorizationFilter
{
    private readonly Claim _claim;

    public RequisitoClaimFilter(Claim claim) => _claim = claim;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        //Verifica se o usuário está autenticado.
        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            throw new UnauthorizedException(string.Empty);
            //context.Result = new StatusCodeResult(401);
            //return;
        }

        //Verifica se o usuário está autenticação e se ele tem permissão.
        //Ex: Está logado, mas não tem claim solicidada => [ClaimsAuthorize("Catalogo","Ler")]
        if (!CustomAuthorization.ValidarClaimsUsuario(context.HttpContext, _claim.Type, _claim.Value))
        {
            // context.Result = new StatusCodeResult(403);

            throw new ForbiddenException(string.Empty);
        }


    }
}