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

        public void CreateEvent()
        {
            var start = new DateTime(2019, 01, 09, 22, 00, 00);
            var end = new DateTime(2019, 01, 09, 23, 59, 59);
            var body = "Descripcion \n Descrición \n Hola tarola";
            var location = "San Martin 980 Quilmes";
            var titulo = "Titulo Test";
            var calendarId = "09ptb764ha2oood2ighc8udfik@group.calendar.google.com";

            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "SUA - BackOffice",
            });

            //Event
            var _event = new Event
            {
                Id = System.Guid.NewGuid().ToString().Replace("-", ""),
                Start = new EventDateTime { DateTime = start },
                End = new EventDateTime { DateTime = end },
                Location = location,
                Description = body,
                Summary = titulo,
                Source = new Event.SourceData { Title = "BO-SUA", Url = "http://c721.cloud.wiroos.net" }
            };
            var newEventRequest = service.Events.Insert(_event, calendarId);
            var eventResult = newEventRequest.Execute();
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