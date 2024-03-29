﻿using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MyHome.Application.Common.Services;

namespace MyHome.Dashboard.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string UserId { get; }
    }
}