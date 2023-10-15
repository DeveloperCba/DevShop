using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DevShop.Core.Comunications;
using DevShop.Identity.Application.Features.User.Dtos;
using DevShop.Identity.Domain.Interfaces;
using DevShop.Identity.Domain.Models;
using DevShop.Identity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Identity.Application.Services;

public interface IAutenticationService
{
    Task<UserResponseLoginDto> GenereateJwt(string email);
    Task<RefreshToken> GetRefreshToken(Guid refreshToken);
    SignInManager<ApplicationUser> SignInManager { get; set; }
    UserManager<ApplicationUser> UserManager { get; set; }
}

public class AutenticationService : IAutenticationService
{
    public SignInManager<ApplicationUser> SignInManager { get; set; }
    public UserManager<ApplicationUser> UserManager { get; set; }
    private readonly ApplicationIdentityDbContext _context;
    private readonly JwtSettings _jwtSettings;
    private readonly IUserRepository _userRepository;

    public AutenticationService(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        ApplicationIdentityDbContext context,
        IOptions<JwtSettings> jwtSettings,
        IUserRepository userRepository
    )
    {
        SignInManager = signInManager;
        UserManager = userManager;
        _context = context;
        _jwtSettings = jwtSettings.Value;
        _userRepository = userRepository;
    }

    public async Task<UserResponseLoginDto> GenereateJwt(string email)
    {
        var user = await _userRepository.GetByFilterAsync(x => x.Email == email);
        await SignInManager.SignInAsync(user, false);
        var claims = await UserManager.GetClaimsAsync(user);

        var identityClaims = await GetClaimsUser(claims, user);
        var encodedToken = EncodeToken(identityClaims);

        var refreshToken = await GenerateRefreshToken(email);

        return GetResponseToken(encodedToken, user, claims, refreshToken);
    }

    private async Task<ClaimsIdentity> GetClaimsUser(ICollection<Claim> claims, ApplicationUser user)
    {
        var userRoles = await UserManager.GetRolesAsync(user);

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

        foreach (var userRole in userRoles)
        {
            claims.Add(new Claim("role", userRole));
        }

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        return identityClaims;
    }

    private string EncodeToken(ClaimsIdentity identityClaims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.ValidOn,
            Subject = identityClaims,
            Expires = DateTime.UtcNow.AddHours((double)_jwtSettings.TokenExpiration.GetValueOrDefault() ),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

        return tokenHandler.WriteToken(token);
    }

    private UserResponseLoginDto GetResponseToken(string encodedToken, ApplicationUser user, IEnumerable<Claim> claims, RefreshToken refreshToken)
    {
        return new UserResponseLoginDto
        {
            AccessToken = encodedToken,
            RefreshToken = refreshToken.Token,
            ExpiresIn = TimeSpan.FromHours(_jwtSettings.TokenExpiration.GetValueOrDefault()).TotalSeconds,
            UsuarioToken = new UserTokenDto
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                Claims = claims.Select(c => new UserClaimDto { Type = c.Type, Value = c.Value })
            }
        };
    }

    private static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
            .TotalSeconds);

    private async Task<RefreshToken> GenerateRefreshToken(string email)
    {
        var refreshToken = new RefreshToken
        {
            UserName = email,
            ExpirationDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiration.GetValueOrDefault())
        };

        _context.RefreshTokens.RemoveRange(_context.RefreshTokens.Where(u => u.UserName == email));
        await _context.RefreshTokens.AddAsync(refreshToken);

        await _context.SaveChangesAsync();

        return refreshToken;
    }

    public async Task<RefreshToken> GetRefreshToken(Guid refreshToken)
    {
        var token = await _context.RefreshTokens.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Token == refreshToken);

        return token != null && token.ExpirationDate.ToLocalTime() > DateTime.Now
            ? token
            : default!;  
    }
}