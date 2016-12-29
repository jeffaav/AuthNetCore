using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthNetCore.Filters
{
    public class UserAuthenticatedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext?.User;

            if (!(user is null) && user.Identity.IsAuthenticated)
                context.Result = new RedirectToRouteResult("default");

            base.OnActionExecuting(context);
        }
    }
}
