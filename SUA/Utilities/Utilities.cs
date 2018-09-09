using System;
using System.Collections.Generic;
using System.Linq;
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
                "Neuquén",
                "Río Negro",
                "Salta",
                "San Juan",
                "Santa Cruz",
                "Santa Fe",
                "Santiago del Estero",
                "Tierra del Fuego",
                "Tucumán"
            };
            return ConverListToSelectListItem(provincias);
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
                "Venezuela​"
            };
            return ConverListToSelectListItem(paises);
        }

        public static List<SelectListItem> GetBancos()
        {
            var bancos = new List<string> {
                "BANCO DE LA NACION ARGENTINA",
                "BANCO DE LA PROVINCIA DE BUENOS AIRES",
                "INDUSTRIAL AND COMMERCIAL BANK OF CHINA",
                "CITIBANK",
                "BBVA BANCO FRANCES",
                "THE BANK OF TOKYO-MITSUBISHI UFJ",
                "BANCO DE LA PROVINCIA DE CORDOBA",
                "BANCO SUPERVIELLE",
                "BANCO DE LA CIUDAD DE BUENOS AIRES",
                "BANCO PATAGONIA"
                ,"BANCO HIPOTECARIO",
                "BANCO DE SAN JUAN",
                "BANCO DEL TUCUMAN",
                "BANCO MUNICIPAL DE ROSARIO",
                "BANCO SANTANDER RIO",
                "BANCO DEL CHUBUT",
                "BANCO DE SANTA CRUZ",
                "BANCO DE LA PAMPA SOCIEDAD DE ECONOMÍA M",
                "BANCO DE CORRIENTES",
                "BANCO PROVINCIA DEL NEUQUÉN",
                "BANCO INTERFINANZAS",
                "HSBC BANK ARGENTINA",
                "JPMORGAN CHASE BANK",
                "BANCO CREDICOOP",
                "BANCO DE VALORES",
                "BANCO ROELA",
                "BANCO MARIVA",
                "BANCO ITAU ARGENTINA",
                "BANK OF AMERICA",
                "BNP PARIBAS",
                "BANCO PROVINCIA DE TIERRA DEL FUEGO",
                "BANCO DE LA REPUBLICA ORIENTAL DEL URUGUAY",
                "BANCO SAENZ",
                "BANCO MERIDIAN",
                "BANCO MACRO",
                "BANCO COMAFI",
                "BANCO DE INVERSION Y COMERCIO EXTERIOR S",
                "BANCO PIANO",
                "BANCO FINANSUR",
                "BANCO JULIO SOCIEDAD ANONIMA",
                "BANCO RIOJA SOCIEDAD ANONIMA UNIPERSONAL",
                "BANCO DEL SOL",
                "NUEVO BANCO DEL CHACO",
                "BANCO VOII",
                "BANCO DE FORMOSA",
                "BANCO CMF",
                "BANCO DE SANTIAGO DEL ESTERO","BANCO INDUSTRIAL",
                "NUEVO BANCO DE SANTA FE",
                "BANCO CETELEM ARGENTINA",
                "BANCO DE SERVICIOS FINANCIEROS",
                "BANCO BRADESCO ARGENTINA",
                "BANCO DE SERVICIOS Y TRANSACCIONES","RCI BANQUE",
                "BACS BANCO DE CREDITO Y SECURITIZACION S",
                "BANCO MASVENTAS","NUEVO BANCO DE ENTRE RÍOS",
                "BANCO COLUMBIA",
                "BANCO BICA",
                "BANCO COINAG",
                "BANCO DE COMERCIO",
                "FORD CREDIT COMPAÑIA FINANCIERA",
                "COMPAÑIA FINANCIERA ARGENTINA",
                "VOLKWAGEN FINANCIAL SERVICES CIA.FIN",
                "CORDIAL COMPAÑÍA FINANCIERA",
                "FCA COMPAÑIA FINANCIERA",
                "GPAT COMPAÑIA FINANCIERA",
                "MERCEDES-BENZ COMPAÑÍA FINANCIERA ARGENTINA",
                "ROMBO COMPAÑÍA FINANCIERA",
                "JOHN DEERE CREDIT COMPAÑÍA FINANCIERA",
                "PSA FINANCE ARGENTINA COMPAÑÍA FINANCIER",
                "TOYOTA COMPAÑÍA FINANCIERA DE ARGENTINA",
                "FINANDINO COMPAÑIA FINANCIERA",
                "MONTEMAR COMPAÑIA FINANCIERA",
                "MULTIFINANZAS COMPAÑIA FINANCIERA",
                "CAJA DE CREDITO \"CUENCA\" COOPERATIVA",
            };
            return ConverListToSelectListItem(bancos);
        }

        public static List<SelectListItem> ConverListToSelectListItem(List<string> list)
        {
            var selectList = new List<SelectListItem>();
            foreach (var item in list)
                selectList.Add(new SelectListItem { Text = item.Replace(" ", ""), Value = item });

            return selectList;
        }

    }
}