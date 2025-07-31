using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SD_Sinema.Web.Filters
{
    public class AuthenticationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Check if the action or controller has AllowAnonymous attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata
                .Any(em => em.GetType() == typeof(AllowAnonymousAttribute));
            
            if (allowAnonymous)
            {
                return; // Allow access without authentication
            }
            
            // Check if user is authenticated
            var userId = context.HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                // Redirect to login page if not authenticated
                context.Result = new RedirectToActionResult("Login", "Users", null);
                return;
            }
        }
    }
} 