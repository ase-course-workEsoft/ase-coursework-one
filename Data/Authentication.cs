using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FuelIn.Data
{
    public class Authentication : ActionFilterAttribute
    {
        public string requiredPrivilegeType { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.GetString("userName") == null || 
                filterContext.HttpContext.Session.GetString("privilegeType") != requiredPrivilegeType)
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary {
                                { "Controller", "Auth" },
                                { "Action", "Index" }
                            });
            }
        }
    }
}
