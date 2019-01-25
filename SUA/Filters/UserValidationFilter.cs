using SUA.Models;
using SUA.Servicios;
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

            var session = filterContext.HttpContext.Request.Cookies.Get("session");
            if (session == null)
            {
                if (action != "Login")
                    filterContext.Result = new RedirectResult("/login");
            }
            else
            {
                var service = new UserService();
                var user = service.GetUserByNombre(session.Value);
                if (user == null && controller != "Home" && action != "Login")
                {
                    filterContext.Result = new RedirectResult("/login");
                }
                else
                {
                    if (user.UserMaster == "no")
                    {
                        //////////////////////Database BackUp //////////////////////////

                        if (controller == "DataBase" && action == "Backup")
                        {
                            filterContext.Result = new RedirectResult("/inicio");
                        }


                        ////////////////////// LOGIN //////////////////////////
                        if (controller == "Home" && action == "Login")
                        {
                            filterContext.Result = new RedirectResult("/inicio");
                        }

                        //////////////////////////////////FECHA/////////////////////////////////////////////

                        //CREAR - EDITAR
                        else if (controller == "Fecha" && action == "Fecha")
                        {
                            if (user.Fechas == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                            else if (user.Fechas == "Lectura")
                            {
                                filterContext.Result = new RedirectResult("/fechas");
                            }
                        }
                        //LISTAR FECHAS
                        else if (controller == "Fecha" && action == "Fechas")
                        {
                            if (user.Fechas == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                        }
                        //BORRAR FECHA
                        else if (controller == "Fecha" && action == "DeleteFecha")
                        {
                            if (user.Fechas == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                            else if (user.Fechas == "Lectura")
                            {
                                filterContext.Result = new RedirectResult("/fechas");
                            }
                        }
                        //CREAR - EDITAR BX
                        else if (controller == "Fecha" && action == "Bordereaux")
                        {
                            if(user.Bordereaux == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                            else if (user.Bordereaux == "Lectura")
                            {
                                filterContext.Result = new RedirectResult("/fechas");
                            }
                        }
                        //CREAR - IMPRIMIR BX
                        else if (controller == "Fecha" && action == "PrintBordereaux")
                        {
                            if (user.Bordereaux == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                        }

                        //FECHAS CERRADAS
                        else if (controller == "Fecha" && action == "FechasCerradas")
                        {
                            if (user.Reportes == "no")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                        }




                        ///////////////////////////////PRODUCTORES////////////////////////////////////////////

                        //CREAR - EDITAR
                        else if (controller == "Productor" && action == "Productor")
                        {
                            if(user.Productores == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                            else if (user.Productores == "Lectura")
                            {
                                filterContext.Result = new RedirectResult("/productores");
                            }
                        }
                        //LISTAR
                        else if (controller == "Productor" && action == "Productores")
                        {
                            if (user.Productores == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                        }
                        //BORRAR
                        else if (controller == "Productor" && action == "DeleteProductor")
                        {
                            if (user.Productores == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                            else if (user.Productores == "Lectura")
                            {
                                filterContext.Result = new RedirectResult("/productores");
                            }
                        }

                        ///////////////////////////////////////SALAS///////////////////////////////////////////////


                        // CREAR - EDITAR
                        else if (controller == "Sala" && action == "Sala")
                        {
                            if(user.Salas == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                            else if (user.Salas == "Lectura")
                            {
                                filterContext.Result = new RedirectResult("/salas");
                            }
                        }

                        //LISTAR
                        else if (controller == "Sala" && action == "Salas")
                        {
                            if (user.Salas == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                        }

                        //BORRAR
                        else if (controller == "Sala" && action == "DeleteSala")
                        {
                            if (user.Salas == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                            else if (user.Salas == "Lectura")
                            {
                                filterContext.Result = new RedirectResult("/salas");
                            }
                        }


                        //////////////////////////////SHOW///////////////////////////////////////////

                        //CREAR - EDITAR
                        else if (controller == "Show" && action == "Show")
                        {
                            if (user.Shows == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                            if (user.Shows == "Lectura")
                            {
                                filterContext.Result = new RedirectResult("/shows");
                            }
                        }

                        //LISTAR
                        else if (controller == "Show" && action == "Shows")
                        {
                            if (user.Shows == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                        }

                        //BORRAR
                        else if (controller == "Show" && action == "DeleteShow")
                        {
                            if (user.Shows == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                            if (user.Shows == "Lectura")
                            {
                                filterContext.Result = new RedirectResult("/shows");
                            }
                        }


                        ////////////////////////////STANDUPERO///////////////////////////////////////////

                        //CREAR - EDITAR
                        else if (controller == "Standupero" && action == "Standupero")
                        {
                            if (user.Standuperos == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                            if (user.Standuperos == "Lectura")
                            {
                                filterContext.Result = new RedirectResult("/standuperos");
                            }
                        }

                        //LISTAR
                        else if (controller == "Standupero" && action == "Standuperos")
                        {
                            if (user.Standuperos == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                        }

                        //BORRAR
                        else if (controller == "Standupero" && action == "DeleteStandupero")
                        {
                            if (user.Standuperos == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                            if (user.Standuperos == "Lectura")
                            {
                                filterContext.Result = new RedirectResult("/standuperos");
                            }
                        }

                        ////////////////////////////USUARIOS////////////////////////////////////

                        else if (controller == "User" && action == "Usuario")
                        {
                            filterContext.Result = new RedirectResult("/inicio");
                        }
                        else if (controller == "User" && action == "Usuarios")
                        {
                            filterContext.Result = new RedirectResult("/inicio");
                        }
                        else if (controller == "Historial" && action == "Historial")
                        {
                            filterContext.Result = new RedirectResult("/inicio");
                        }


                        //////////////////////////////HOTELES/////////////////////////////////////////

                        //CREAR - EDITAR
                        else if (controller == "Hotel" && action == "Hotel")
                        {
                            if (user.Hoteles == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                            if (user.Hoteles == "Lectura")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                        }

                        //LISTAR
                        else if (controller == "Hotel" && action == "Hotels")
                        {
                            if (user.Hoteles == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                        }
                        //BORRAR
                        else if (controller == "Hotel" && action == "DeleteHotel")
                        {
                            if (user.Hoteles == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                            if (user.Hoteles == "Lectura")
                            {
                                filterContext.Result = new RedirectResult("/hoteles");
                            }
                        }

                        ////////////////////////////////////RESTAURANTES/////////////////////////////////

                        
                        //CREAR - EDITAR
                        else if (controller == "Restaurante" && action == "Restaurante")
                        {
                            if(user.Restaurantes == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                            else if (user.Restaurantes == "Lectura")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                        }

                        //LISTAR
                        else if (controller == "Restaurante" && action == "Restaurantes")
                        {
                            if (user.Restaurantes == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                        }
                        //BORRAR
                        else if (controller == "Restaurante" && action == "DeleteRestaurante")
                        {
                            if (user.Restaurantes == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                            if (user.Restaurantes == "Lectura")
                            {
                                filterContext.Result = new RedirectResult("/restaurantes");
                            }
                        }


                        ////////////////////////////////////PROVEEDORES/////////////////////////////////


                        //CREAR - EDITAR
                        else if (controller == "Proveedor" && action == "Proveedor")
                        {
                            if (user.Proveedores == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                            else if (user.Proveedores == "Lectura")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                        }

                        //LISTAR
                        else if (controller == "Proveedor" && action == "Proveedores")
                        {
                            if (user.Proveedores == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                        }
                        //BORRAR
                        else if (controller == "Proveedor" && action == "DeleteProveedor")
                        {
                            if (user.Proveedores == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                            if (user.Proveedores == "Lectura")
                            {
                                filterContext.Result = new RedirectResult("/proveedores");
                            }
                        }



                        ////////////////////////////////////PRENSA/////////////////////////////////


                        //CREAR - EDITAR
                        else if (controller == "Prensa" && action == "Prensa")
                        {
                            if (user.Prensa == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                            else if (user.Prensa == "Lectura")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                        }

                        //LISTAR
                        else if (controller == "Prensa" && action == "Prensas")
                        {
                            if (user.Prensa == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                        }
                        //BORRAR
                        else if (controller == "Prensa" && action == "DeletePrensa")
                        {
                            if (user.Prensa == "Prohibido")
                            {
                                filterContext.Result = new RedirectResult("/inicio");
                            }
                            if (user.Prensa == "Lectura")
                            {
                                filterContext.Result = new RedirectResult("/prensas");
                            }
                        }
                    }
                }

            }
        }
    }
}