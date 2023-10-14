﻿using DevShop.Identity.Application.Models.Dtos;
using MediatR;
namespace DevShop.Identity.Application.Features.Auth.Commands;

public class ConfirmEmailCommand : IRequest<MessageDto>
{
    public string UserId { get; set; }
    public string Token { get; set; }
}