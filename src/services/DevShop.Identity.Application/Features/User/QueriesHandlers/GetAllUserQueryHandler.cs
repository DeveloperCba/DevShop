using System.Linq.Expressions;
using AutoMapper;
using DevShop.Core.Datas.Enumerators;
using DevShop.Core.Datas.Extensions;
using DevShop.Core.Datas.Interfaces;
using DevShop.Core.DomainObjects;
using DevShop.Identity.Application.Features.User.Dtos;
using DevShop.Identity.Application.Features.User.Queries;
using DevShop.Identity.Domain.Interfaces;
using DevShop.Identity.Domain.Models;
using DevShop.WebAPI.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Identity.Application.Features.User.QueriesHandlers;

public class GetAllUserQueryHandler : BaseService,
    IRequestHandler<GetAllUserQuery, List<UserDto>>,
    IRequestHandler<GetAllUserPaginationQuery, List<UserDto>>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IRedisRepository _redisRepository;
    public GetAllUserQueryHandler(
        INotify notification,
        IUserRepository userRepository,
        IMapper mapper,
        IRedisRepository redisRepository) : base(notification)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _redisRepository = redisRepository;
    }

    public async Task<List<UserDto>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        var key = $"identity:users:filter:{(string.IsNullOrEmpty(request.Filter) ? "all" : $"all:{request.Filter}")}";
        var modelCache = await _redisRepository.GetCache<List<UserDto>>(key);
        if (modelCache == null)
        {
            Expression<Func<ApplicationUser, bool>> filter = x => !string.IsNullOrEmpty(x.Id);
            if (!string.IsNullOrEmpty(request.Filter))
                filter = GetFilterUser(request.Filter);

            var users = (await _userRepository.GetAllAsync(filter,
                include: source => source
                    .Include(x => x.Claims)
                    .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
            )).ToList();

            if (users.Count > 0 && users != null)
            {
                modelCache = _mapper.Map<List<UserDto>>(users);
                await _redisRepository.SetCache(key, modelCache, (int)CacheExpiration.OneMinute);
            }
        }


        return modelCache ?? default!;
    }

    public async Task<List<UserDto>> Handle(GetAllUserPaginationQuery request, CancellationToken cancellationToken)
    {
        var key = $"identity:users:filter:{(string.IsNullOrEmpty(request.Filter) ? "all" : $"{request.Filter}")}:pagenumber:{request.PageNumber}:pagesize:{request.PageSize}";
        var modelCache = await _redisRepository.GetCache<List<UserDto>>(key);
        if (modelCache == null)
        {
            Expression<Func<ApplicationUser, bool>> filter = x => !string.IsNullOrEmpty(x.Id);
            if (!string.IsNullOrEmpty(request.Filter))
                filter = GetFilterUser(request.Filter);

            var users = (await _userRepository
                    .GetAllPaginationAsync(filter, pageNumber: request.PageNumber, pageSize: request.PageSize, isTracking: false,
                        include: source => source
                            .Include(x => x.Claims)
                            .Include(x => x.UserRoles)
                            .ThenInclude(x => x.Role)
                    ))
                .ToList();

            if (users.Count > 0)
            {
                modelCache = _mapper.Map<List<UserDto>>(users);
                await _redisRepository.SetCache(key, modelCache, (int)CacheExpiration.OneMinute);
            }
        }
        return modelCache ?? default!;
    }

    private Expression<Func<ApplicationUser, bool>> GetFilterUser(string search)
    {
        Expression<Func<ApplicationUser, bool>> filter = x => !string.IsNullOrEmpty(x.Id);
        filter = filter.And(x => x.Name.Contains(search));
        filter = filter.Or(x => x.UserName.Contains(search));
        filter = filter.Or(x => x.Document == search);
        filter = filter.Or(x => x.Email == search);

        //if (search.IsInteger())
        //    filter = filter.Or(x => x.id == Convert.ToInt32(search));

        return filter;

    }
}