using SUA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SUA.Utilities
{
    public static class UtilitiesAndStuff
    {
        public static List<SelectListItem> GetProvincias()
        {
            var provincias = new List<string>
            {
                "Buenos Aires",
                "Catamarca",
                "Chaco",
                "Chubut",
                "Córdoba",
                "Corrientes",
                "Entre Ríos",
                "Formosa",
                "Jujuy",
                "La Pampa",
                "La Rioja",
                "Mendoza",
                "Misiones",
                "Montevideo",
                "Neuquén",
                "Río Negro",
                "Salta",
                "San Juan",
                "San Luis",
                "Santa Cruz",
                "Santa Fe",
                "Santiago del Estero",
                "Tierra del Fuego",
                "Tucumán"
            };
            return ConverProvinciasToSelectListItem(provincias);
        }

        public static List<SelectListItem> GetPaises()
        {
            var paises = new List<string>
            {
                "Argentina",
                "Bolivia​",
                "Chile",
                "Colombia",
                "Ecuador",
                "España",
                "Perú",
                "Uruguay",
                "Venezuela​"
            };
            return ConverListToSelectListItem(paises);
        }

        public static List<SelectListItem> GetBancos()
        {
            var bancos = new List<string> {
                "BANCO GALICIA",
                "BANCO DE LA NACION ARGENTINA",
                "BANCO DE LA PROVINCIA DE BUENOS AIRES",
                "CITIBANK",
                "BBVA BANCO FRANCES",
                "BANCO SUPERVIELLE",
                "BANCO DE LA CIUDAD DE BUENOS AIRES",
                "BANCO PATAGONIA",
                "BANCO HIPOTECARIO",
                "BANCO DE SAN JUAN",
                "BANCO DEL TUCUMAN",
                "BANCO MUNICIPAL DE ROSARIO",
                "BANCO SANTANDER RIO",
                "HSBC BANK ARGENTINA",
                "JPMORGAN CHASE BANK",
                "BANCO CREDICOOP",
                "BANCO MARIVA",
                "BANCO ITAU ARGENTINA",
                "BANCO MACRO",
                "BANCO COMAFI",
                "BANCO FINANSUR",
                "BANCO COLUMBIA"
            };

            bancos = bancos.OrderBy(q => q).ToList();
            return ConverListToSelectListItem(bancos);
        }

        public static List<SelectListItem> ConverListToSelectListItem(List<string> list)
        {
            var selectList = new List<SelectListItem>();
            foreach (var item in list)
                selectList.Add(new SelectListItem { Text = item.Replace(" ", ""), Value = item });

            return selectList;
        }

        public static List<SelectListItem> ConverProvinciasToSelectListItem(List<string> list)
        {
            var selectList = new List<SelectListItem>();
            foreach (var item in list)
                selectList.Add(new SelectListItem { Text = item.Replace(" ", "@"), Value = item });

            return selectList;
        }

        public static string GenerateUniqueId()
        {
            return DateTime.Now.ToString("yyyyMMddHHssfff");
        }

        public static List<SelectListItem> GetImpuestos()
        {
            var impuestos = new List<string>
            {
                "AADET",
                "AADICAPIF",
                "Agadu",
                "Alquiler Teatro",
                "Argentores",
                "COFONTE",
                "Comisión por Tarjeta​",
                "IMM",
                "Impuesto Municipal",
                "Luces y Sonido",
                "Otro",
                "SADAIC",
                "Ticketing"
            };
            return ConverListToSelectListItem(impuestos);
        }

        public static List<SelectListItem> GetEntradas()
        {
            var entradas = new List<string>
            {
                "2x1",
                "Aplicacion",
                "Boleteria",
                "Entradas Teatro 1",
                "Entradas Teatro 2",
                "Entradas Teatro 3",
                "Entradas Teatro 4",
                "Entradas Teatro 5",
                "Entradas Teatro 6",
                "Entradas Ticketera 1",
                "Entradas Ticketera 2",
                "Entradas Ticketera 3",
                "Entradas Ticketera 4",
                "Entradas Ticketera 5",
                "Entradas Ticketera 6",
                "Free",
                "Invitaciones",
                "Invitaciones 2",
                "Menor",
                "Otras",
                "Wix",
                "Wix 50% off",
                "Wix 25% off"

            };
            return ConverListToSelectListItem(entradas);
        }

        public static List<SelectListItem> GetGastosCompany()
        {
            var gastos = new List<string>
            {
                "Ads Facebook",
                "Buque",
                "Camarin",
                "Cartel Puerta",
                "Carteles Calle",
                "Combustible",
                "Direccion",
                "Estacionamiento",
                "Hospedaje",
                "Otro",
                "Pasajes avión",
                "Peajes",
                "Propina",
                "Propina Sonido",
                "Publicidad",
                "Sonido",
                "Taxis"
            };
            return ConverListToSelectListItem(gastos);
        }

        public static int CalcularVencimiento(DateTime fecha, int repeticionEnDias)
        {
            var fechaVencimiento = fecha.AddDays(repeticionEnDias);
            double diasVencido = (DateTime.Now - fechaVencimiento).TotalDays;
            var diasVencidoRedondeado = Math.Round(diasVencido, MidpointRounding.AwayFromZero);
            return Convert.ToInt32(diasVencidoRedondeado);
        }

    }
}