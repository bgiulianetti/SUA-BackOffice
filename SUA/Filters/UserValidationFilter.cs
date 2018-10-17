using SUA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SUA.Filters
{
    public class UserValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToString();
            var action = filterContext.ActionDescriptor.ActionName.ToString();

            var user = System.Web.HttpContext.Current.Session["user"] as UserModel;
            if (user == null)
            {
                if(controller != "Home" && action != "Login")
                    filterContext.Result = new RedirectResult("/login");
            }


            if(controller == "Home" && action=="Login")
            {
                filterContext.Result = new RedirectResult("/inicio");
            }
            else if(controller == "Feecha" && action == "Fecha")
            {
                if(user.Fechas == "Lectura")
                {
                    filterContext.Result = new RedirectResult("/inicio");
                }
            }
        }
    }
}