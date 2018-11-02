using SUA.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SUA.Utilities
{
    public static class UtilitiesAndStuff
    {
        public static List<SelectListItem> GetEstados()
        {
            var estados = new List<string>
            {
                "si",
                "no"
            };
            return ConverListToSelectListItem(estados);
        }

        public static List<SelectListItem> GetPermisos()
        {
            var permisos = new List<string>
            {
                "Prohibido",
                "Lectura",
                "Escritura"
            };
            return ConverListToSelectListItem(permisos);
        }

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

        public static int CalcularRepeticion(DateTime fechaUltimoShow, DateTime fechaProximoShow, int repeticionEnDias)
        {
            var FechaRepeticion = fechaUltimoShow.AddDays(repeticionEnDias);
            var distanciaEnDias = (fechaProximoShow - FechaRepeticion).TotalDays;
            var distanciaPorcentaje = distanciaEnDias * 100 / repeticionEnDias;
            return Convert.ToInt32(distanciaPorcentaje);
        }

        public static List<SelectListItem> GetColores()
        {
            var colores = new List<string>
            {
                "black",
                "blue",
                "brown",
                "gray",
                "green",
                "lightblue",
                "lightgray",
                "orange",
                "pink",
                "purple",
                "white",
                "yellow"
            };
            return ConverListToSelectListItem(colores);
        }

        public static List<Ciudad> GetCiudades()
        {
            var ciudades = new List<Ciudad>();
            ciudades.Add(new Ciudad("Azul", "Buenos Aires", "Argentina", "53054"));
            ciudades.Add(new Ciudad("Bahía Blanca", "Buenos Aires", "Argentina", "299101"));
            ciudades.Add(new Ciudad("Banda del Río Salí", "Tucumán", "Argentina", "64591"));
            ciudades.Add(new Ciudad("Banfield", "Buenos Aires", "Argentina", "223898"));
            ciudades.Add(new Ciudad("Barranqueras", "Chaco", "Argentina", "50738"));
            ciudades.Add(new Ciudad("Béccar", "Buenos Aires", "Argentina", "58811"));
            ciudades.Add(new Ciudad("Belén de Escobar", "Buenos Aires", "Argentina", "55054"));
            ciudades.Add(new Ciudad("Bella Vista", "Buenos Aires", "Argentina", "67936"));
            ciudades.Add(new Ciudad("Berazategui", "Buenos Aires", "Argentina", "167555"));
            ciudades.Add(new Ciudad("Berisso", "Buenos Aires", "Argentina", "54406"));
            ciudades.Add(new Ciudad("Bernal", "Buenos Aires", "Argentina", "109914"));
            ciudades.Add(new Ciudad("Bosques", "Buenos Aires", "Argentina", "51663"));
            ciudades.Add(new Ciudad("Boulogne Sur Mer", "Buenos Aires", "Argentina", "73496"));
            ciudades.Add(new Ciudad("Buenos Aires", "Ciudad Autónoma de Buenos Aires", "Argentina", "2890151"));
            ciudades.Add(new Ciudad("Burzaco", "Buenos Aires", "Argentina", "86113"));
            ciudades.Add(new Ciudad("Campana", "Buenos Aires", "Argentina", "94461"));
            ciudades.Add(new Ciudad("Caseros", "Buenos Aires", "Argentina", "95785"));
            ciudades.Add(new Ciudad("Castelar", "Buenos Aires", "Argentina", "104019"));
            ciudades.Add(new Ciudad("Chimbas", "San Juan", "Argentina", "73210"));
            ciudades.Add(new Ciudad("Chivilcoy", "Buenos Aires", "Argentina", "87510"));
            ciudades.Add(new Ciudad("Cipolletti", "Río Negro", "Argentina", "66472"));
            ciudades.Add(new Ciudad("Ciudad de Corrientes", "Corrientes", "Argentina", "314546"));
            ciudades.Add(new Ciudad("Ciudad de Formosa", "Formosa", "Argentina", "222226"));
            ciudades.Add(new Ciudad("Ciudad de La Rioja", "La Rioja", "Argentina", "178872"));
            ciudades.Add(new Ciudad("Ciudad de Mendoza", "Mendoza", "Argentina", "110993"));
            ciudades.Add(new Ciudad("Ciudad de Neuquén", "Neuquén", "Argentina", "201868"));
            ciudades.Add(new Ciudad("Ciudad de Río Cuarto", "Córdoba", "Argentina", "157010"));
            ciudades.Add(new Ciudad("Ciudad de Salta", "Salta", "Argentina", "535303"));
            ciudades.Add(new Ciudad("Ciudad de San Juan", "San Juan", "Argentina", "112778"));
            ciudades.Add(new Ciudad("Ciudad de San Luis", "San Luis", "Argentina", "250957"));
            ciudades.Add(new Ciudad("Ciudad de Santa Fe", "Santa Fe", "Argentina", "500000"));
            ciudades.Add(new Ciudad("Ciudad de Santiago del Estero", "Santiago del Estero", "Argentina", "230614"));
            ciudades.Add(new Ciudad("Ciudad Evita", "Buenos Aires", "Argentina", "68650"));
            ciudades.Add(new Ciudad("Ciudad Jardín El Libertador", "Buenos Aires", "Argentina", "61780"));
            ciudades.Add(new Ciudad("Ciudadela", "Buenos Aires", "Argentina", "73155"));
            ciudades.Add(new Ciudad("Clorinda", "Formosa", "Argentina", "46884"));
            ciudades.Add(new Ciudad("Comodoro Rivadavia", "Chubut", "Argentina", "212410"));
            ciudades.Add(new Ciudad("Concepción", "Tucumán", "Argentina", "46194"));
            ciudades.Add(new Ciudad("Concepción del Uruguay", "Entre Ríos", "Argentina", "64538"));
            ciudades.Add(new Ciudad("Concordia", "Entre Ríos", "Argentina", "147046"));
            ciudades.Add(new Ciudad("Córdoba", "Córdoba", "Argentina", "1429604"));
            ciudades.Add(new Ciudad("Don Torcuato", "Buenos Aires", "Argentina", "64867"));
            ciudades.Add(new Ciudad("El Jagüel", "Buenos Aires", "Argentina", "48781"));
            ciudades.Add(new Ciudad("El Palomar", "Buenos Aires", "Argentina", "57146"));
            ciudades.Add(new Ciudad("Eldorado", "Misiones", "Argentina", "47794"));
            ciudades.Add(new Ciudad("Esperanza", "Santa Fe", "Argentina", "42082"));
            ciudades.Add(new Ciudad("Ezeiza", "Buenos Aires", "Argentina", "93247"));
            ciudades.Add(new Ciudad("Ezpeleta", "Buenos Aires", "Argentina", "72557"));
            ciudades.Add(new Ciudad("Florencio Varela", "Buenos Aires", "Argentina", "120678"));
            ciudades.Add(new Ciudad("Florida (no es ciudad sino barrio)", "Buenos Aires", "Argentina", "75891"));
            ciudades.Add(new Ciudad("General Pico", "La Pampa", "Argentina", "52414"));
            ciudades.Add(new Ciudad("General Roca", "Río Negro", "Argentina", "69602"));
            ciudades.Add(new Ciudad("General Rodríguez", "Buenos Aires", "Argentina", "63317"));
            ciudades.Add(new Ciudad("Glew", "Buenos Aires", "Argentina", "57878"));
            ciudades.Add(new Ciudad("Gobernador Julio A Costa", "Buenos Aires", "Argentina", "49291"));
            ciudades.Add(new Ciudad("Godoy Cruz", "Mendoza", "Argentina", "182563"));
            ciudades.Add(new Ciudad("González Catán", "Buenos Aires", "Argentina", "165206"));
            ciudades.Add(new Ciudad("Goya", "Corrientes", "Argentina", "66462"));
            ciudades.Add(new Ciudad("Grand Bourg", "Buenos Aires", "Argentina", "85159"));
            ciudades.Add(new Ciudad("Gregorio de Laferrere", "Buenos Aires", "Argentina", "175670"));
            ciudades.Add(new Ciudad("Gualeguaychú", "Entre Ríos", "Argentina", "74681"));
            ciudades.Add(new Ciudad("Guaymallén", "Mendoza", "Argentina", "223365"));
            ciudades.Add(new Ciudad("Isidro Casanova", "Buenos Aires", "Argentina", "136091"));
            ciudades.Add(new Ciudad("Ituzaingó", "Buenos Aires", "Argentina", "126631"));
            ciudades.Add(new Ciudad("José C. Paz", "Buenos Aires", "Argentina", "216637"));
            ciudades.Add(new Ciudad("Junín", "Buenos Aires", "Argentina", "87509"));
            ciudades.Add(new Ciudad("La Banda", "Santiago del Estero", "Argentina", "95142"));
            ciudades.Add(new Ciudad("La Plata", "Buenos Aires", "Argentina", "699523"));
            ciudades.Add(new Ciudad("La Tablada", "Buenos Aires", "Argentina", "80389"));
            ciudades.Add(new Ciudad("Lanús", "Buenos Aires", "Argentina", "459263"));
            ciudades.Add(new Ciudad("Las Heras", "Mendoza", "Argentina", "182563"));
            ciudades.Add(new Ciudad("Libertad", "Buenos Aires", "Argentina", "100476"));
            ciudades.Add(new Ciudad("Lomas de Zamora", "Buenos Aires", "Argentina", "111897"));
            ciudades.Add(new Ciudad("Lomas del Mirador", "Buenos Aires", "Argentina", "52971"));
            ciudades.Add(new Ciudad("Longchamps", "Buenos Aires", "Argentina", "47622"));
            ciudades.Add(new Ciudad("Los Polvorines", "Buenos Aires", "Argentina", "53354"));
            ciudades.Add(new Ciudad("Luján", "Buenos Aires", "Argentina", "67266"));
            ciudades.Add(new Ciudad("Luján de Cuyo", "Mendoza", "Argentina", "73058"));
            ciudades.Add(new Ciudad("Maipú", "Mendoza", "Argentina", "89433"));
            ciudades.Add(new Ciudad("Mar del Plata", "Buenos Aires", "Argentina", "664892"));
            ciudades.Add(new Ciudad("Mariano Acosta", "Buenos Aires", "Argentina", "54081"));
            ciudades.Add(new Ciudad("Martínez", "Buenos Aires", "Argentina", "65859"));
            ciudades.Add(new Ciudad("Mercedes", "Buenos Aires", "Argentina", "51967"));
            ciudades.Add(new Ciudad("Merlo", "Buenos Aires", "Argentina", "244168"));
            ciudades.Add(new Ciudad("Monte Chingolo", "Buenos Aires", "Argentina", "85060"));
            ciudades.Add(new Ciudad("Monte Grande", "Buenos Aires", "Argentina", "110241"));
            ciudades.Add(new Ciudad("Moreno", "Buenos Aires", "Argentina", "149317"));
            ciudades.Add(new Ciudad("Morón", "Buenos Aires", "Argentina", "92725"));
            ciudades.Add(new Ciudad("Necochea", "Buenos Aires", "Argentina", "65459"));
            ciudades.Add(new Ciudad("Oberá", "Misiones", "Argentina", "51681"));
            ciudades.Add(new Ciudad("Olavarría", "Buenos Aires", "Argentina", "83738"));
            ciudades.Add(new Ciudad("Olivos (no es ciudad sino barrio)", "Buenos Aires", "Argentina", "75527"));
            ciudades.Add(new Ciudad("Palpalá", "Jujuy", "Argentina", "45077"));
            ciudades.Add(new Ciudad("Paraná", "Entre Ríos", "Argentina", "247863"));
            ciudades.Add(new Ciudad("Pergamino", "Buenos Aires", "Argentina", "104985"));
            ciudades.Add(new Ciudad("Pilar", "Buenos Aires", "Argentina", "226517"));
            ciudades.Add(new Ciudad("Posadas", "Misiones", "Argentina", "354719"));
            ciudades.Add(new Ciudad("Presidencia Roque Sáenz Peña", "Chaco", "Argentina", "76377"));
            ciudades.Add(new Ciudad("Presidente Perón", "Buenos Aires", "Argentina", "52529"));
            ciudades.Add(new Ciudad("Puerto Madryn", "Chubut", "Argentina", "93995"));
            ciudades.Add(new Ciudad("Punta Alta", "Buenos Aires", "Argentina", "57296"));
            ciudades.Add(new Ciudad("Quilmes", "Buenos Aires", "Argentina", "230810"));
            ciudades.Add(new Ciudad("Rafael Calzada", "Buenos Aires", "Argentina", "56419"));
            ciudades.Add(new Ciudad("Rafael Castillo", "Buenos Aires", "Argentina", "103992"));
            ciudades.Add(new Ciudad("Rafaela", "Santa Fe", "Argentina", "82530"));
            ciudades.Add(new Ciudad("Ramos Mejía", "Buenos Aires", "Argentina", "98547"));
            ciudades.Add(new Ciudad("Rawson", "Chubut", "Argentina", "31787"));
            ciudades.Add(new Ciudad("Reconquista", "Santa Fe", "Argentina", "66187"));
            ciudades.Add(new Ciudad("Remedios de Escalada (Partido de Lanús)", "Buenos Aires", "Argentina", "81465"));
            ciudades.Add(new Ciudad("Resistencia", "Chaco", "Argentina", "290723"));
            ciudades.Add(new Ciudad("Río Gallegos", "Santa Cruz", "Argentina", "95796"));
            ciudades.Add(new Ciudad("Río Grande", "Tierra del Fuego", "Argentina", "52786"));
            ciudades.Add(new Ciudad("Rivadavia", "San Juan", "Argentina", "75950"));
            ciudades.Add(new Ciudad("Rosario", "Santa Fe", "Argentina", "1400000"));
            ciudades.Add(new Ciudad("San Carlos de Bariloche", "Río Negro", "Argentina", "113299"));
            ciudades.Add(new Ciudad("San Fernando", "Buenos Aires", "Argentina", "69110"));
            ciudades.Add(new Ciudad("San Fernando del Valle de Catamarca", "Catamarca", "Argentina", "159139"));
            ciudades.Add(new Ciudad("San Francisco", "Córdoba", "Argentina", "58588"));
            ciudades.Add(new Ciudad("San Francisco Solano", "Buenos Aires", "Argentina", "53363"));
            ciudades.Add(new Ciudad("San Isidro", "Buenos Aires", "Argentina", "292878"));
            ciudades.Add(new Ciudad("San Justo", "Buenos Aires", "Argentina", "105274"));
            ciudades.Add(new Ciudad("San Martín", "Mendoza", "Argentina", "49491"));
            ciudades.Add(new Ciudad("San Miguel", "Buenos Aires", "Argentina", "157532"));
            ciudades.Add(new Ciudad("San Miguel de Tucumán", "Tucumán", "Argentina", "694327"));
            ciudades.Add(new Ciudad("San Nicolás de los Arroyos", "Buenos Aires", "Argentina", "125408"));
            ciudades.Add(new Ciudad("San Pedro de Jujuy", "Jujuy", "Argentina", "55084"));
            ciudades.Add(new Ciudad("San Rafael", "Mendoza", "Argentina", "118009"));
            ciudades.Add(new Ciudad("San Ramón de la Nueva Orán", "Salta", "Argentina", "66579"));
            ciudades.Add(new Ciudad("San Salvador de Jujuy", "Jujuy", "Argentina", "231229"));
            ciudades.Add(new Ciudad("Santa Rosa", "La Pampa", "Argentina", "101987"));
            ciudades.Add(new Ciudad("Sarandí", "Buenos Aires", "Argentina", "60725"));
            ciudades.Add(new Ciudad("Tandil", "Buenos Aires", "Argentina", "101010"));
            ciudades.Add(new Ciudad("Tartagal", "Salta", "Argentina", "55508"));
            ciudades.Add(new Ciudad("Temperley", "Buenos Aires", "Argentina", "111660"));
            ciudades.Add(new Ciudad("Trelew", "Chubut", "Argentina", "103656"));
            ciudades.Add(new Ciudad("Tres Arroyos", "Buenos Aires", "Argentina", "45986"));
            ciudades.Add(new Ciudad("Trujui", "Buenos Aires", "Argentina", "94608"));
            ciudades.Add(new Ciudad("Ushuaia", "Tierra del Fuego", "Argentina", "56825"));
            ciudades.Add(new Ciudad("Venado Tuerto", "Santa Fe", "Argentina", "68508"));
            ciudades.Add(new Ciudad("Vicente López", "Buenos Aires", "Argentina", "274082"));
            ciudades.Add(new Ciudad("Viedma", "Río Negro", "Argentina", "46767"));
            ciudades.Add(new Ciudad("Villa Carlos Paz", "Córdoba", "Argentina", "60900"));
            ciudades.Add(new Ciudad("Villa Centenario", "Buenos Aires", "Argentina", "49737"));
            ciudades.Add(new Ciudad("Villa Dolores", "Córdoba", "Argentina", "43625"));
            ciudades.Add(new Ciudad("Villa Domínico", "Buenos Aires", "Argentina", "58824"));
            ciudades.Add(new Ciudad("Villa Gobernador Gálvez", "Santa Fe", "Argentina", "74658"));
            ciudades.Add(new Ciudad("Villa Luzuriaga", "Buenos Aires", "Argentina", "73952"));
            ciudades.Add(new Ciudad("Villa Madero", "Buenos Aires", "Argentina", "75582"));
            ciudades.Add(new Ciudad("Villa María", "Córdoba", "Argentina", "72162"));
            ciudades.Add(new Ciudad("Villa Mariano Moreno-El Colmenar", "Tucumán", "Argentina", "48655"));
            ciudades.Add(new Ciudad("Villa Mercedes", "San Luis", "Argentina", "97000"));
            ciudades.Add(new Ciudad("Villa Tesei", "Buenos Aires", "Argentina", "63164"));
            ciudades.Add(new Ciudad("Virrey del Pino", "Buenos Aires", "Argentina", "90383"));
            ciudades.Add(new Ciudad("Wilde", "Buenos Aires", "Argentina", "65881"));
            ciudades.Add(new Ciudad("William Morris", "Buenos Aires", "Argentina", "48916"));
            ciudades.Add(new Ciudad("Yerba Buena/Marcos Paz", "Tucumán", "Argentina", "50057"));
            ciudades.Add(new Ciudad("Zárate", "Buenos Aires", "Argentina", "98522"));
            ciudades.Add(new Ciudad("Cochabamba", "Cochabamba", "Bolivia", "632013"));
            ciudades.Add(new Ciudad("El Alto", "La Paz", "Bolivia", "827239"));
            ciudades.Add(new Ciudad("La Paz", "La Paz", "Bolivia", "757184"));
            ciudades.Add(new Ciudad("Santa Cruz de la Sierra", "Santa Cruz", "Bolivia", "1468700"));
            ciudades.Add(new Ciudad("Brasilia", "Brasilia", "Brasil", "2789761"));
            ciudades.Add(new Ciudad("Río de Janeiro", "Río de Janeiro", "Brasil", "6476631"));
            ciudades.Add(new Ciudad("Salvador de Bahía", "Bahía", "Brasil", "2948733"));
            ciudades.Add(new Ciudad("São Paulo", "São Paulo", "Brasil", "12106920"));
            ciudades.Add(new Ciudad("Gran Concepción", "Biobío", "Chile", "848023"));
            ciudades.Add(new Ciudad("Gran La Serena", "Coquimbo", "Chile", "296253"));
            ciudades.Add(new Ciudad("Gran Santiago", "Metropolitana de Santiago", "Chile", "5631839"));
            ciudades.Add(new Ciudad("Gran Valparaíso", "Valparaíso", "Chile", "824006"));
            ciudades.Add(new Ciudad("Barranquilla", "Atlántico", "Colombia", "1228510"));
            ciudades.Add(new Ciudad("Bogotá", "Cundinamarca", "Colombia", "8164178"));
            ciudades.Add(new Ciudad("Cali", "Valle del Cauca", "Colombia", "2408653"));
            ciudades.Add(new Ciudad("Medellín", "Antioquia", "Colombia", "2501470"));
            ciudades.Add(new Ciudad("Cuenca", "Azuay", "Ecuador", "329928"));
            ciudades.Add(new Ciudad("Guayaquil", "Guayas", "Ecuador", "2278691"));
            ciudades.Add(new Ciudad("Quito", "Pichincha", "Ecuador", "1607734"));
            ciudades.Add(new Ciudad("Santo Domingo", "Santo Domingo de los Tsáchilas", "Ecuador", "270875"));
            ciudades.Add(new Ciudad("Barcelona", "Cataluña", "España", "1620809"));
            ciudades.Add(new Ciudad("Madrid", "Comunidad de Madrid", "España", "3182981"));
            ciudades.Add(new Ciudad("Sevilla", "Andalucía", "España", "689434"));
            ciudades.Add(new Ciudad("Valencia", "Comunidad Valenciana", "España", "787808"));
            ciudades.Add(new Ciudad("Ciudad de México", "Ciudad de México", "Mexico", "8851080"));
            ciudades.Add(new Ciudad("Ecatepec", "México", "Mexico", "1655015"));
            ciudades.Add(new Ciudad("Guadalajara", "Jalisco", "Mexico", "1495182"));
            ciudades.Add(new Ciudad("Puebla de Zaragoza", "Puebla", "Mexico", "1434062"));
            ciudades.Add(new Ciudad("Asunción", "Distrito Capital", "Paraguay", "523184"));
            ciudades.Add(new Ciudad("Ciudad del Este", "Alto Paraná", "Paraguay", "299255"));
            ciudades.Add(new Ciudad("Luque", "Central", "Paraguay", "272808"));
            ciudades.Add(new Ciudad("San Lorenzo", "Central", "Paraguay", "256008"));
            ciudades.Add(new Ciudad("Arequipa", "Arequipa", "Perú", "1008290"));
            ciudades.Add(new Ciudad("Callao", "Callao", "Perú", "994494"));
            ciudades.Add(new Ciudad("Lima", "Lima", "Perú", "9562280"));
            ciudades.Add(new Ciudad("Trujillo", "La Libertad", "Perú", "919899"));
            ciudades.Add(new Ciudad("MaldonadoNota 2​", "Maldonado", "Uruguay", "86782"));
            ciudades.Add(new Ciudad("Montevideo", "Montevideo", "Uruguay", "1305082"));
            ciudades.Add(new Ciudad("PaysandúNota 1​", "Paysandú", "Uruguay", "90690"));
            ciudades.Add(new Ciudad("Salto", "Salto", "Uruguay", "104028"));

            return ciudades;
        }
    }
}