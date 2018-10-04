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

        public GoogleCalendarService()
        {
            ApiUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings.Get("Calendar.BaseUrl"));
            Client = new HttpClient();
            Client.BaseAddress = ApiUrl;
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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