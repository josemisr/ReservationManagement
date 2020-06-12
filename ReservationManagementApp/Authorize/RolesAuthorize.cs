using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReservationManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReservationManagementApp.Authorize
{

    public class RolesAuthorizeRequirement : IAuthorizationRequirement
    {
        public RolesAuthorizeRequirement(int role)
        {
            Role = role;
        }

        public int Role { get; private set; }
    }

    public class RoleAuthorizeHandler : AuthorizationHandler<RolesAuthorizeRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RoleAuthorizeHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesAuthorizeRequirement requirement)
        {
            if (requirement.Role != 0)
            {
                if (string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Session.GetString("User")))
                {
                    context.Fail();
                }
                else
                {
                    Users user = JsonSerializer.Deserialize<Users>(_httpContextAccessor.HttpContext.Session.GetString("User"));
                    if (user.IdRole == requirement.Role)
                    {
                        context.Succeed(requirement);
                    }
                }
            }
            else
            {
                context.Succeed(requirement);
            }
            return Task.FromResult(0);
        }
    }
}

