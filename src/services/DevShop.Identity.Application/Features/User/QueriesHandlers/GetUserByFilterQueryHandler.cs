using System.Linq.Expressions;
using AutoMapper;
using DevShop.Core.Datas.Enumerators;
using DevShop.Core.Datas.Interfaces;
using DevShop.Core.DomainObjects;
using DevShop.Identity.Application.Enumerators;
using DevShop.Identity.Application.Features.User.Dtos;
using DevShop.Identity.Application.Features.User.Queries;
using DevShop.Identity.Domain.Interfaces;
using DevShop.Identity.Domain.Models;
using DevShop.WebAPI.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Identity.Application.Features.User.QueriesHandlers;

public class GetUserByFilterQueryHandler : BaseService,
    IRequestHandler<GetUserByEmailQuery, UserDto>,
    IRequestHandler<GetUserByIdQuery, UserDto>,
    IRequestHandler<GetUserByUserNameQuery, UserDto>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IRedisRepository _redisRepository;
    public GetUserByFilterQueryHandler(
        INotify notification,
        IUserRepository userRepository,
        IMapper mapper,
        IRedisRepository redisRepository) : base(notification)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _redisRepository = redisRepository;
    }

    public async Task<UserDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var key = $"identity:users:byemail:{request.Email}";
        return await GetUser(key, request.Email, UserFilterEnum.Email);
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var key = $"identity:users:byuserid:{request.UserId}";
        return await GetUser(key, request.UserId, UserFilterEnum.Id);
    }

    public async Task<UserDto> Handle(GetUserByUserNameQuery request, CancellationToken cancellationToken)
    {
        var key = $"identity:users:byusername:{request.UserName}";
        return await GetUser(key, request.UserName, UserFilterEnum.UserName);
    }

    private async Task<UserDto> GetUser(string key, string value, UserFilterEnum type)
    {

        var modelCache = await _redisRepository.GetCache<UserDto>(key);
        if (modelCache == null)
        {
            Expression<Func<ApplicationUser, bool>> filter = default!;
            var user = new ApplicationUser();
            switch (type)
            {
                case UserFilterEnum.Id:
                    filter = x => x.Id == value;
                    break;
                case UserFilterEnum.Email:
                    filter = x => x.Email == value;
                    break;
                case UserFilterEnum.UserName:
                    filter = x => x.UserName == value;
                    break;
                default:
                    user = default!;
                    break;
            }
            user = await _userRepository.GetByFilterAsync(filter,
                include: source => source
                    .Include(x => x.Claims)
                    .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
            );
            if (user == null)
            {
                NotificationEvent("Usuário não encontrado.");
                return default!;
            }
            modelCache = _mapper.Map<UserDto>(user);
            await _redisRepository.SetCache(key, modelCache, (int)CacheExpiration.OneMinute);
        }
        return modelCache;
    }
}