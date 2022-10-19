using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HeartAttackApp.Filters
{
    public class AdminCheck : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (Convert.ToInt32(context.HttpContext.Session.GetString("RoleID")) == 2)
            {
                context.Result = new RedirectResult("/Account/Login");
                return;
            }
        }
    }
}
