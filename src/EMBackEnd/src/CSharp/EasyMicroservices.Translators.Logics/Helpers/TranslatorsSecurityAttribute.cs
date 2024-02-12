using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EasyMicroservices.TranslatorsMicroservice.Helpers;
public class TranslatorsSecurityAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    string _Role;
    public TranslatorsSecurityAttribute(string role)
    {
        _Role = role;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Customize your authorization logic here
        var user = context.HttpContext.User;
        if (!user.Identity.IsAuthenticated)
        {
            // Not logged in
            context.Result = new UnauthorizedResult();
            return;
        }

        // Check for the specific permission

        if (!user.IsInRole(_Role))
        {
            // User does not have the required permission
            context.Result = new ForbidResult();
        }
    }
}