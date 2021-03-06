﻿using SUA.Models;
using SUA.Repositorios;
using SUA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Servicios
{
    public class FechaService
    {
        public ESRepositorio Repository { get; set; }

        public FechaService()
        {
            var node = new UriBuilder("localhost")
            {
                Port = 9200
            };
            var settings = new ESSettings(node);
            Repository = new ESRepositorio(settings, ESRepositorio.ContentType.fecha.ToString());
        }

        public List<Fecha> GetFechas(string all, bool isIndexRequesting = false)
        {
            var fechas = Repository.GetFechas().OrderByDescending(f => f.FechaHorario).ToList();
            if (all == "")
                fechas = fechas.Take(50).ToList();
            else if (all == "bx")
                fechas = fechas.Where(f => DateTime.Now.AddHours(3) >= f.FechaHorario && f.Borederaux == null).ToList();
            else if (all == "gasto")
                fechas = fechas.Where(f => f.Gastos == null || f.Gastos.Count == 0).ToList();

            var session = HttpContext.Current.Request.Cookies.Get("session");
            var service = new UserService();
            var user = service.GetUserByNombre(session.Value);
            if (user.UserMaster == "no")
            {
                if(user.ShowsAsignados.Count > 0)
                    return fechas.Where(f => user.ShowsAsignados.Contains(f.Show)).ToList();
                else
                    return new List<Fecha>();
            }
            else
            {
                return fechas;
            }

        }
        public List<Fecha> GetFechasForBackUp()
        {
            return Repository.GetFechas();
        }
        public Fecha GetFechaById(string id)
        {
            return Repository.GetFechaById(id);
        }
        public List<Fecha> GetFechasByShow(string nombreShow)
        {
            return Repository.GetFechasByShow(nombreShow);
        }
        public List<Fecha> GetFechasByShowId(string id)
        {
            return Repository.GetFechasByShowId(id);
        }
        public Fecha GetUltimaFechaByShowId(string id)
        {
            var fechas = Repository.GetFechasByShowId(id);
            if (fechas.Count == 0)
                return null;

            var fechasOrdenadas = fechas.OrderBy(f => f.FechaHorario);
            return fechasOrdenadas.Last();
        }
        public List<Fecha> GetFechasByProvincia(string nombreProvincia)
        {
            return Repository.GetFechasByProvincia(nombreProvincia);
        }
        public List<Fecha> GetFechasBySala(string nombreSala)
        {
            return Repository.GetFechasBySala(nombreSala);
        }
        public Fecha GetFechaBySalaAndFechaAndHorario(string idSala, DateTime fechaYHorario)
        {
            return Repository.GetFechaBySalaAndFechaAndHorario(idSala, fechaYHorario);
        }
        public void AddFecha(Fecha fecha)
        {
            Repository.AddFecha(fecha);
        }
        public void AddBulkFecha(List<Fecha> fechas)
        {
            Repository.AddBulkFecha(fechas);
        }
        public void UpdateFecha(Fecha fecha)
        {
            Repository.UpdateFecha(fecha);
        }
        public string GetFechaInnerIdById(string id)
        {
            return Repository.GetFechaInnerIdById(id);
        }
        public void DeleteFecha(string id)
        {
            Repository.DeleteFecha(id);
        }
        public Fecha GetUltimaFechaBySalaId(string idSala)
        {
            var fechas = Repository.GetFechasByIdSala(idSala);
            if (fechas.Count > 0)
            {
                fechas.OrderBy(f => f.FechaHorario);
                return fechas.Last();
            }
            return null;
        }
        public List<Fecha> GetFechasByCiudadAndRangoFecha(string ciudad, DateTime desde, DateTime hasta)
        {
            var fechas = Repository.GetFechasByCiudad(ciudad, desde, hasta);
            if (fechas.Count > 0)
            {
                fechas.OrderBy(f => f.FechaHorario);
            }
            return fechas;
        }
        public Fecha GetUltimaFechaBySalaAndShow(string idSala, string idShow)
        {
            var fechas = Repository.GetFechasByIdSalaAndIdShow(idSala, idShow);
            if(fechas != null && fechas.Count > 0)
            {
                fechas = fechas.OrderBy(f => f.FechaHorario).ToList();
                return fechas.Last();
            }
            return null;

        }
        public List<Fecha> GetFechasConBordereaux()
        {
            var fechasConBordereaux = new List<Fecha>();
            var fechas = Repository.GetFechas();
            foreach (var item in fechas)
            {
                if (item.Borederaux != null)
                    fechasConBordereaux.Add(item);
            }

            //filtro las fechas por los shows que tienen asignados los usuarios
            var session = HttpContext.Current.Request.Cookies.Get("session");
            var service = new UserService();
            var user = service.GetUserByNombre(session.Value);
            if (user.UserMaster == "no")
            {
                if (user.ShowsAsignados.Count > 0)
                {
                    var fechasFiltradas = new List<Fecha>();
                    foreach (var item in fechasConBordereaux)
                    {
                        if (user.ShowsAsignados.Contains(item.Show))
                            fechasFiltradas.Add(item);
                    }
                    return fechasFiltradas;
                }
                else
                {
                    return new List<Fecha>();
                }
            }
            else
            {
                return fechasConBordereaux;
            }

        }
        public List<Fecha> GetFechasByCiudadAndIdShow(string ciudad, string idShow)
        {
            var fechas = Repository.GetFechasByCiudadAndIdShow(ciudad, idShow);
            return fechas;
        }

        public List<Fecha> GetFechaForGoogleCalendarAction()
        {
            return Repository.GetFechaForGoogleCalendarAction();
        }

        public List<InfoPlazasParaRepeticion> GetRepeticionPlazasByShowAndDate(string idShow, DateTime date)
        {
            var list = new List<InfoPlazasParaRepeticion>();

            var salaService = new SalaService();
            var ciudades = salaService.GetCiudadesInSalas();

            var showService = new ShowService();
            var repeticiones = showService.GetShowById(idShow).Repeticion;
            if(repeticiones == null)
            {
                foreach (var ciudad in ciudades)
                {
                    var salas = salaService.GetSalasByCiudad(ciudad);
                    list.Add(new InfoPlazasParaRepeticion
                    {
                        Ciudad = ciudad,
                        Repeticion = 10000000,
                        Salas = ConverSalaListToSalaSimpleList(salas)
                    });
                }
            }
            else
            {
                foreach (var ciudad in ciudades)
                {
                    RepeticionPlazas repeticion = null;
                    repeticion = repeticiones.Where(f => f.Ciudad == ciudad).FirstOrDefault();
                    if (repeticion != null)
                    {
                        var fechaService = new FechaService();
                        var ultimaFechaByCiudadAndShow = fechaService.GetFechasByCiudadAndIdShow(ciudad, idShow).OrderByDescending(f => f.FechaHorario).ToList().FirstOrDefault();
                        if (ultimaFechaByCiudadAndShow == null)
                        {
                            var porcentajeDiferencia = UtilitiesAndStuff.CalcularRepeticion(new DateTime(2018, 01, 01), date, repeticion.Dias);
                            var salasPorCiudad = salaService.GetSalasByCiudad(ciudad);
                            list.Add(new InfoPlazasParaRepeticion
                            {
                                Ciudad = ciudad,
                                Repeticion = porcentajeDiferencia,
                                Salas = ConverSalaListToSalaSimpleList(salasPorCiudad)
                            });
                        }
                        else
                        {
                            var porcentajeDiferencia = UtilitiesAndStuff.CalcularRepeticion(ultimaFechaByCiudadAndShow.FechaHorario, date, repeticion.Dias);

                            var salas = salaService.GetSalasByCiudad(ciudad);
                            list.Add(new InfoPlazasParaRepeticion
                            {
                                Ciudad = ciudad,
                                Repeticion = porcentajeDiferencia,
                                Salas = ConverSalaListToSalaSimpleList(salas)
                            });
                        }
                    }
                    else
                    {
                        var salas = salaService.GetSalasByCiudad(ciudad);
                        list.Add(new InfoPlazasParaRepeticion
                        {
                            Ciudad = ciudad,
                            Repeticion = 10000000,
                            Salas = ConverSalaListToSalaSimpleList(salas)
                        });
                    }
                }
            }
            
            var listOrdenada = list.OrderByDescending(o => o.Repeticion).ToList();
            var ciudadesSinEspecificacion = listOrdenada.Where(f => f.Repeticion == 10000000).ToList();
            listOrdenada.RemoveAll(f => f.Repeticion == 10000000);
            listOrdenada.AddRange(ciudadesSinEspecificacion);
            return listOrdenada;
        }

        private List<SalaSimple> ConverSalaListToSalaSimpleList(List<Sala> salas)
        {
            var list = new List<SalaSimple>();
            foreach (var sala in salas)
            {
                list.Add(new SalaSimple {
                    IdSala = sala.UniqueId,
                    Nombre = sala.Nombre
                });
            }
            return list;
        }

        public List<ChartInfoContract> GetFechasByYear(int year)
        {
            var fechasInYear = Repository.GetFechas().Where(f => f.FechaHorario >= new DateTime(year, 01, 01) && f.FechaHorario <= new DateTime(year, 12, 31)).ToList();
            var fechas = new List<ChartInfoContract>();

            var showService = new ShowService();
            var shows = showService.GetShows();
            foreach (var show in shows)
            {
                var count = fechasInYear.Where(f => f.Show.UniqueId == show.UniqueId).Count();
                fechas.Add(new ChartInfoContract { y = count, label = show._Show });
            }
            return fechas.OrderByDescending(f=>f.y).ToList();
        }

    }
}