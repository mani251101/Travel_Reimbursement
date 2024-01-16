using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
#nullable disable

namespace authfilters
{
    public class Authorizefilters : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                // User is not authenticated, redirect to login page
                context.Result = new RedirectResult("Error");
                return;
            }
            if (!context.HttpContext.User.IsInRole("Admin"))
            {
                // User does not have the required role, deny access
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}