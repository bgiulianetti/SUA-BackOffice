﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using SUA.Models;
using SUA.Servicios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CalendarQuickstart
{
    public static class Program
    {
        static string[] Scopes = {
            "https://www.googleapis.com/auth/calendar",
        };
        static string ApplicationName = "Google Calendar API .NET Quickstart";

        static void Main(string[] args)
        {
            while (true)
            {
                CreateEvent();
                Thread.Sleep(60000);
            }
            
            //Console.Read();
        }

        public static void CreateEvent()
        {
            UserCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });

            var serviceFecha = new SUA.Servicios.FechaService();
            var fechas = serviceFecha.GetFechaForGoogleCalendarAction();
            if (fechas != null && fechas.Count > 0)
            {
                foreach (var fecha in fechas)
                {
                    var _event = CreateEvent(fecha);
                    if (fecha.GoogleCalendarState == "post")
                    {
                        try
                        {
                            //var eventRequest = service.Events.Insert(_event, "09ptb764ha2oood2ighc8udfik@group.calendar.google.com");
                            var eventRequest = service.Events.Insert(_event, fecha.Show.GoogleCalendarId);
                            eventRequest.SendNotifications = true;
                            eventRequest.Execute();
                        }
                        catch (Exception ex)
                        {
                            var mensaje = ex.Message;
                        }
                    }
                    else if (fecha.GoogleCalendarState == "put")
                    {
                        try
                        {
                            //var eventRequest = service.Events.Update(_event, "09ptb764ha2oood2ighc8udfik@group.calendar.google.com", fecha.UniqueId);
                            var eventRequest = service.Events.Update(_event, fecha.Show.GoogleCalendarId, fecha.UniqueId);
                            eventRequest.SendNotifications = true;
                            eventRequest.Execute();
                        }
                        catch (Exception ex)
                        {
                            var mensaje = ex.Message;
                        }
                    }
                    else if(fecha.GoogleCalendarState == "delete")
                    {
                        try
                        {
                            //var eventRequest = service.Events.Delete("09ptb764ha2oood2ighc8udfik@group.calendar.google.com", fecha.UniqueId);
                            var eventRequest = service.Events.Delete(fecha.Show.GoogleCalendarId, fecha.UniqueId);
                            eventRequest.SendNotifications = true;
                            eventRequest.Execute();
                        }
                        catch (Exception ex)
                        {
                            var mensaje = ex.Message;
                        }
                    }
                    fecha.GoogleCalendarState = "ok";
                    serviceFecha.UpdateFecha(fecha);
                }
            }
        }

        public static Event CreateEvent(Fecha fecha)
        {
            var _event =  new Event
            {
                Id = fecha.UniqueId,
                Start = new EventDateTime { DateTime = fecha.FechaHorario, DateTimeRaw = fecha.FechaHorario.ToString("yyyy-MM-ddTHH:mm:ss-03:00") },
                End = new EventDateTime { DateTime = fecha.FechaHorario, DateTimeRaw = fecha.FechaHorario.ToString("yyyy-MM-ddTHH:mm:ss-03:00") },
                Location = fecha.Sala.Direccion.Direccion + ", " + fecha.Sala.Direccion.Ciudad,
                Description = CreateDescription(fecha),
                Summary = fecha.Sala.Direccion.Ciudad + " " + fecha.Show.SiglaBordereaux,
                Source = new Event.SourceData { Title = "BO-SUA", Url = "http://c721.cloud.wiroos.net" },
            };

            var attendees = new List<EventAttendee>();
            attendees.Add(new EventAttendee() { Email = fecha.Productor.Email, DisplayName = fecha.Productor.Nombre + " " + fecha.Productor.Apellido });
            foreach (var integrante  in fecha.Show.Integrantes)
            {
                var atendee = new EventAttendee()
                {
                    Email = integrante.Email,
                    DisplayName = integrante.Nombre + " " + integrante.Apellido,
                };
                attendees.Add(atendee);
            }
            attendees.Add(new EventAttendee() { Email = "standupargentina@gmail.com", DisplayName = "SUA" });
            _event.Attendees = attendees.ToArray();
            return _event;
        }


        public static string CreateDescription(Fecha fecha)
        {
            var description = fecha.Sala.ToString();
            /*
            if (!string.IsNullOrEmpty(fecha.Observaciones))
            {
                description += "\n\nObservaciones: " + fecha.Observaciones;
            }
            */
            return description;
        }

    }
}