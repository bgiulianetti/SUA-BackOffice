using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SUA.CalendarEvents
{
    public class Program
    {
        static string[] Scopes = { "https://www.googleapis.com/auth/calendar"};
        static string ApplicationName = "Google Calendar";

        public void CreateEvent()
        {

            var start = new DateTime(2019, 01, 06, 22, 00, 00);//DateTime.Parse(args[1]);
            var end = new DateTime(2019, 01, 06, 23, 59, 59); //DateTime.Parse(args[2]);
            var body = "body";//args[3];
            var location = "Pinamar";// args[4];
            var titulo = "Sanata en Pinamar";// args[5];
            var calendarId = "09ptb764ha2oood2ighc8udfik@group.calendar.google.com"; //args[6];
            var eventId = DateTime.Now.ToString("yyyyMMddHHmmss");// args[7];

            UserCredential credential;

            using (var stream = new FileStream(@"C:\Users\fcacho\Documents\Calendar\credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = @"C:\Users\fcacho\Documents\Calendar\token.json";
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
        }
    }
}
