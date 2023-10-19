using DevShop.Identity.Application.Features.User.Dtos;
using DevShop.Identity.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace DevShop.Identity.Application.Contracts;

public interface IAutenticationService
{
    Task<UserResponseLoginDto> GenerateJwt(string email);
    Task<RefreshToken> GetRefreshToken(Guid refreshToken);
    SignInManager<ApplicationUser> SignInManager { get; set; }
    UserManager<ApplicationUser> UserManager { get; set; }
}