using AutoMapper;
using DevShop.Identity.Application.Features.Auth.Commands;
using DevShop.Identity.Application.Features.User.Commands;
using DevShop.Identity.Application.Features.User.Dtos;
using DevShop.Identity.Application.Models.Requesties;
using DevShop.Identity.Domain.Models;

namespace DevShop.Identity.Application.Mappings;

public class AutoMapperConfiguration : Profile
{
    public AutoMapperConfiguration()
    {
        #region USER

        //Command
        CreateMap<LoginRequest, LoginCommand>().ReverseMap();
        CreateMap<UserRequest, CreateUserCommand>().ReverseMap();
        CreateMap<UpdateRequest, UpdateUserCommand>().ReverseMap();
        CreateMap<UserRemoveRequest, RemoveUserCommand>().ReverseMap();
        CreateMap<ResetPasswordRequest, ResetPasswordCommand>().ReverseMap();
        CreateMap<ResetPasswordConfirmationRequest, ResetPasswordConfirmationCommand>().ReverseMap();
        CreateMap<ConfirmEmailRequest, ConfirmEmailCommand>().ReverseMap();
        CreateMap<SendTokenConfirmEmailRequest, SendTokenConfirmEmailCommand>().ReverseMap();
        CreateMap<LockUserRequest, LockUserCommand>().ReverseMap();
        CreateMap<UnlockUserRequest, UnlockUserCommand>().ReverseMap();

        CreateMap<ApplicationUser, UserDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles))
            .ForMember(dest => dest.Claims, opt => opt.MapFrom(src => src.Claims))
            ;

        CreateMap<ApplicationUserClaim, UserClaimDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.ClaimType))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.ClaimValue))
            ;

        CreateMap<ApplicationUserRole, UserRoleDto>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name))
            ;

        #endregion

    }

    public class DtoToEntity : Profile
    {
        public DtoToEntity()
        {
            CreateMap<UserRoleDto, ApplicationUserRole>()
                .ConstructUsing(c => new ApplicationUserRole());
        }
    }
}