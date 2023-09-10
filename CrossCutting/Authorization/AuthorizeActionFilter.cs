using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossCutting.Authorization
{
    public class AuthorizeActionFilter : IAsyncAuthorizationFilter
    {
        private readonly string[] _permissions;
        private readonly IConfiguration _configuration;

        public AuthorizeActionFilter(
            string[] permissions,
            IConfiguration configuration
            )
        {
            _permissions = permissions;
            _configuration = configuration;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            string? userName = null;

            userName = "JD319984";

            //if (authInfo.IsAuthorized)
            //{
            //    RoleActionResponse roles = await _cerberus.GetRolesActionsAsync(_configuration["AppConfig:AppId"], userName);

            //    foreach (Role role in roles.Roles)
            //    {
            //        if (role.Actions.Select(A => A.Name.ToUpper().Trim()).Intersect(_permissions.Select(A => A.ToUpper().Trim())).Any())
            //        {
            //            return;
            //        }
            //    }
            //}

            //context.Result = new UnauthorizedResult();
            return;
        }
    }
}
