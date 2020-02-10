using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            //LOS CALENDARIOS DEBEN ESTAR CREADOS CON LA CUENTA: standupargentina@gmail.com 
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



        public void CreateEvent(/*string calendarId, string body, DateTime start, DateTime end*/)
        {
            string[] Scopes = {
            "https://www.googleapis.com/auth/calendar",
        };
            string ApplicationName = "Google Calendar API .NET Quickstart";


            //var accion = args[0];
            //if(args[0] == "post")
            //{
            var start = new DateTime(2019, 01, 06, 22, 00, 00);//DateTime.Parse(args[1]);
            var end = new DateTime(2019, 01, 06, 23, 59, 59); //DateTime.Parse(args[2]);
            var body = "body";//args[3];
            var location = "Pinamar";// args[4];
            var titulo = "Sanata en Pinamar";// args[5];
            var calendarId = "09ptb764ha2oood2ighc8udfik@group.calendar.google.com"; //args[6];
            var eventId = DateTime.Now.ToString("yyyyMMddHHmmss");// args[7];

            UserCredential credential;

            using (var stream = new FileStream("D:/Git/SUA-BackOffice/SUA.CalendarService/bin/Debug/credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "D:/Git/SUA-BackOffice/SUA.CalendarService/bin/Debug/token.json";
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
                ApplicationName = ApplicationName,
            });

            //Event
            var _event = new Event
            {
                Id = eventId,
                Start = new EventDateTime { DateTime = start },
                End = new EventDateTime { DateTime = end },
                Location = location,
                Description = body,
                Summary = titulo,
                Source = new Event.SourceData { Title = "BO-SUA", Url = "http://c721.cloud.wiroos.net" },
            };
            var newEventRequest = service.Events.Insert(_event, calendarId);
            var eventResult = newEventRequest.Execute();
            /* }
             else if(args[0] == "put")
             {

             }
             else if (args[0] == "delete")
             {

             }*/
        }


        public static void CreateEventViejo(/*string path, string accion, DateTime start, DateTime end, string salaInfo, string direccion, string titulo, string calendarId, string eventId*/)
        {
            var proc = new Process();
            proc.StartInfo.FileName = @"C:\Users\fcacho\Documents\calendarService\SUA.CalendarService.exe ";

            //proc.StartInfo.UseShellExecute = false;
            //proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            //string outPut = proc.StandardOutput.ReadToEnd();

            proc.WaitForExit();
            //var exitCode = proc.ExitCode;
            //proc.Close();
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