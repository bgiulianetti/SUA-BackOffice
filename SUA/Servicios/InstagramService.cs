using Newtonsoft.Json;
using SUA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace SUA.Servicios
{
    public class InstagramService
    {
        public Uri ApiUrl { get; set; }
        public HttpClient Client { get; set; }
        public string[] Scopes { get; set; }

        public InstagramService()
        {
            ApiUrl = new Uri("https://www.instagram.com/");
            Client = new HttpClient();
            Client.BaseAddress = ApiUrl;
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public InstagramUserData GetUser(string username)
        {
            var response = Client.GetAsync(username).Result;
            var responseJson = response.Content.ReadAsStringAsync().Result;
            var foto = responseJson.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None)[1].Split('"')[0];
            var following = responseJson.Split(new string[] { " Following" }, StringSplitOptions.None)[0].Split(new string[] { ", " }, StringSplitOptions.None)[1].Trim();
            var posts = responseJson.Split(new string[] { " Posts" }, StringSplitOptions.None)[0].Split(new string[] { ", " }, StringSplitOptions.None)[1].Trim();

            var user = new InstagramUserData()
            {
                Followers = Int32.Parse(responseJson.Split(new string[] { "userInteractionCount\":\"" }, StringSplitOptions.None)[1].Split('"')[0]),
                Picture = new Uri(foto),
                InstagramUser = username
            };
            ;

            return user;
        }
    }
}