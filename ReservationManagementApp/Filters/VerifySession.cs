
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using ReservationManagementApp.Controllers;


namespace SolucionMonolitica.Filters
{

    public class VerifySession : ActionFilterAttribute
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public VerifySession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Session.GetString("User")))
            {
                if (filterContext.Controller is AccountController == false && filterContext.Controller is HomeController == false)
                    filterContext.HttpContext.Response.Redirect("/Account/Login");
            }
        }
    }
}