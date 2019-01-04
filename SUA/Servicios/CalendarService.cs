using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web;

namespace SUA.Servicios
{
    public class GoogleCalendarService
    {
        public Uri ApiUrl { get; set; }
        public HttpClient Client { get; set; }
        public string[] Scopes { get; set; }

        public GoogleCalendarService()
        {
            ApiUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings.Get("Calendar.BaseUrl"));
            Client = new HttpClient();
            Client.BaseAddress = ApiUrl;
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Scopes = new string[] {
                "https://www.googleapis.com/auth/calendar",
            };
        }

        public string GetCalendarKey()
        {
            var response = Client.GetAsync(System.Configuration.ConfigurationManager.AppSettings.Get("Calendar.Key")).Result;
            var responseJson = response.Content.ReadAsStringAsync().Result;
            var calendarInfo = JsonConvert.DeserializeObject<CalendarKey>(responseJson);
            return calendarInfo.apiKey;
        }

        public List<CalendarId> GetCalendars()
        {
            var response = Client.GetAsync(System.Configuration.ConfigurationManager.AppSettings.Get("Calendar.IDs")).Result;
            var responseJson = response.Content.ReadAsStringAsync().Result;
            var calendars = JsonConvert.DeserializeObject<CalendarResponse>(responseJson);
            return calendars.calendars.ToList();
        }

        public ClientSecrets GetCredentials()
        {
            var response = Client.GetAsync(System.Configuration.ConfigurationManager.AppSettings.Get("Calendar.Credentials")).Result;
            var responseJson = response.Content.ReadAsStringAsync().Result;
            var credentials = JsonConvert.DeserializeObject<ClientSecrets>(responseJson);
            return credentials;
        }

        public void CreateEvent(string path, string accion, DateTime start, DateTime end, string salaInfo, string direccion, string titulo, string calendarId, string eventId)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = path;
            var arguments = accion + " " +
                            " \"" + start.ToString("yyyy-MM-dd hh:mm:ss") + "\" " +
                            " \"" + end.ToString("yyyy-MM-dd hh:mm:ss") + "\" " +
                            " \"" + salaInfo + "\" " +
                            " \"" + direccion + "\" " +
                            " \" " + titulo + "\" " +
                            "09ptb764ha2oood2ighc8udfik@group.calendar.google.com" + " " + 
                            eventId;
            process.StartInfo.Arguments = arguments;

            process.Start();
        }
    }

    public class CalendarKey
    {
        public string apiKey { get; set; }
    }

    public class CalendarId
    {
        public string show { get; set; }
        public string id { get; set; }
    }

    public class CalendarResponse
    {
        public CalendarId[] calendars { get; set; }
    }

}