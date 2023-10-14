using System.ComponentModel.DataAnnotations;

namespace DevShop.Identity.Application.Enumerators;

public enum UserFilterEnum
{
    [Display(Name = "Id", ShortName = "ID", Description = "Id")]
    Id = 1,

    [Display(Name = "Email", ShortName = "EMAIL", Description = "Email")]
    Email = 2,

    [Display(Name = "UserName", ShortName = "UN", Description = "User Name")]
    UserName = 3,
}