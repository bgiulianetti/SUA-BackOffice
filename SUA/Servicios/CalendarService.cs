using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace SUA.Servicios
{
    public class CalendarService
    {
        public Uri ApiUrl { get; set; }
        public HttpClient Client { get; set; }

        public CalendarService()
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