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

namespace CalendarQuickstart
{
    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/calendar-dotnet-quickstart.json
        static string[] Scopes = {
            "https://www.googleapis.com/auth/calendar",        
        };
        static string ApplicationName = "Google Calendar API .NET Quickstart";

        static void Main(string[] args)
        {
            var accion = args[0];
            if(args[0] == "post")
            {
                var start = DateTime.Parse(args[1]);// new DateTime(2019, 01, 03, 22, 00, 00);
                var end = DateTime.Parse(args[2]);// new DateTime(2019, 01, 03, 23, 59, 59);
                var body = args[3];
                var location = args[4];
                var titulo = args[5];
                var calendarId = args[6]; //"09ptb764ha2oood2ighc8udfik@group.calendar.google.com";

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
                    ApplicationName = ApplicationName,
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
            else if(args[0] == "put")
            {

            }
            else if (args[0] == "delete")
            {

            }
            Console.Read();
        }
        private void CreateEvent(string calendarId, string body, DateTime start, DateTime end)
        {

        }
    }
}