using CleanArchitecture.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace CleanArchitecture.WebApi.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        IHttpContextAccessor _httpContext;
        public CurrentUserService(IHttpContextAccessor httpContext) =>
            this._httpContext = httpContext;

        public Guid UserId 
        {
            get 
            {
                var id = _httpContext.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                return string.IsNullOrWhiteSpace(id) ? Guid.Empty : Guid.NewGuid();
            }
        }
    }
}
