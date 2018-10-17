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
                filterContext.Result = new RedirectResult("/login");
            else
            {
                if (user.UserMaster == "no")
                {
                    if (controller == "Home" && action == "Login")
                    {
                        filterContext.Result = new RedirectResult("/inicio");
                    }
                    else if (controller == "Fecha" && action == "Fecha")
                    {
                        if (user.Fechas == "Lectura")
                        {
                            filterContext.Result = new RedirectResult("/inicio");
                        }
                    }
                    else if (controller == "Fecha" && action == "Bordereaux")
                    {
                        if (user.Bordereaux == "Lectura")
                        {
                            filterContext.Result = new RedirectResult("/fechas");
                        }
                    }
                    else if (controller == "Fecha" && action == "DeleteFecha")
                    {
                        if (user.Fechas == "Lectura")
                        {
                            filterContext.Result = new RedirectResult("/fechas");
                        }
                    }
                    else if (controller == "Productor" && action == "Productor")
                    {
                        if (user.Productores == "Lectura")
                        {
                            filterContext.Result = new RedirectResult("/inicio");
                        }
                    }
                    else if (controller == "Productor" && action == "DeleteProductor")
                    {
                        if (user.Productores == "Lectura")
                        {
                            filterContext.Result = new RedirectResult("/productores");
                        }
                    }
                    else if (controller == "Sala" && action == "Sala")
                    {
                        if (user.Salas == "Lectura")
                        {
                            filterContext.Result = new RedirectResult("/inicio");
                        }
                    }
                    else if (controller == "Sala" && action == "DeleteSala")
                    {
                        if (user.Salas == "Lectura")
                        {
                            filterContext.Result = new RedirectResult("/salas");
                        }
                    }
                    else if (controller == "Show" && action == "Show")
                    {
                        if (user.Shows == "Lectura")
                        {
                            filterContext.Result = new RedirectResult("/inicio");
                        }
                    }
                    else if (controller == "Show" && action == "DeleteShow")
                    {
                        if (user.Shows == "Lectura")
                        {
                            filterContext.Result = new RedirectResult("/shows");
                        }
                    }
                    else if (controller == "Standupero" && action == "Standupero")
                    {
                        if (user.Shows == "Lectura")
                        {
                            filterContext.Result = new RedirectResult("/inicio");
                        }
                    }
                    else if (controller == "Standupero" && action == "DeleteStandupero")
                    {
                        if (user.Shows == "Lectura")
                        {
                            filterContext.Result = new RedirectResult("/standuperos");
                        }
                    }
                    else if (controller == "User" && action == "Usuario")
                    {
                        if (user.Shows == "Lectura")
                        {
                            filterContext.Result = new RedirectResult("/inicio");
                        }
                    }
                    else if (controller == "User" && action == "Usuarios")
                    {
                        filterContext.Result = new RedirectResult("/inicio");
                    }
                }
            }

        }
    }
}