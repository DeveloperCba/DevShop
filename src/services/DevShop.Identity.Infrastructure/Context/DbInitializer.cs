using System.Security.Claims;
using DevShop.Core.Datas.Interfaces;
using DevShop.Identity.Domain;
using DevShop.Identity.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace DevShop.Identity.Infrastructure.Context;

public class DbInitializer : IDbInitializer
{
    private readonly ApplicationIdentityDbContext _context;
    private readonly LogDbContext _logContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IHostEnvironment _hostEnvironment;

    public DbInitializer(
        ApplicationIdentityDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IHostEnvironment hostEnvironment
        ,
        LogDbContext logContext)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
        _hostEnvironment = hostEnvironment;
        _logContext = logContext;
    }

    public void Initialize()
    {
        if (_context.Database.GetPendingMigrations().Any())
        {
            _context.Database.Migrate();

            if (_hostEnvironment.IsDevelopment())
                AddUser();
        }

        if (_logContext.Database.GetPendingMigrations().Any())
        {
            _logContext.Database.Migrate();
        }
    }


    private void AddUser()
    {

        if (_roleManager.FindByNameAsync(GlobalConstants.ADMIN).Result == null)
        {
            _roleManager.CreateAsync(new ApplicationRole(GlobalConstants.ADMIN)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new ApplicationRole(GlobalConstants.CUSTOMER)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new ApplicationRole(GlobalConstants.USER)).GetAwaiter().GetResult();
        }
        else { return; }

        var adminUser = new ApplicationUser
        {
            Neighborhood = "CENTRO",
            ZipCode = "78098000",
            City = "CUIABÁ",
            Complement  = "SEM QUADRA",
            Document = "68520993001",
            Email = "administrator@devshop.io",
            UserName = "administrator@devshop.io",
            Name = "ADMINISTRADOR DO SISTEMA",
            Street = "RUA TESTE",
            Number = "345",
            PhoneNumber = "9999999999",
            State = "MT",
        };

        _userManager.CreateAsync(adminUser,"Teste@123").GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(adminUser, GlobalConstants.ADMIN).GetAwaiter().GetResult();
        _ = _userManager.AddClaimsAsync(adminUser, new Claim[] {
            new Claim("Create","True"),
            new Claim("Delete","True"),
            new Claim("Update","True"),
            new Claim("Read","True"),
        }).Result;

        var customerUser = new ApplicationUser
        {
            Neighborhood = "CENTRO",
            ZipCode = "78098000",
            City = "CUIABÁ",
            Complement = "SEM QUADRA",
            Document = "52669843093",
            Email = "support@devshop.io",
            UserName = "support@devshop.io",
            Name = "USUÁRIO DO SISTEMA",
            Street = "RUA TESTE",
            Number = "345",
            PhoneNumber = "9999999999",
            State = "MT",
        };

        _userManager.CreateAsync(customerUser, "Teste@123").GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(customerUser, GlobalConstants.ADMIN).GetAwaiter().GetResult();
        _ = _userManager.AddClaimsAsync(customerUser, new Claim[] {
            new Claim("Create","True"),
            new Claim("Delete","False"),
            new Claim("Update","True"),
            new Claim("Read","True"),
        }).Result;

    }

}