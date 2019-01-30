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
            Client = new HttpClient()
            {
                BaseAddress = ApiUrl
            };
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public InstagramUserData GetUserBy(string username)
        {
            var response = Client.GetAsync(username).Result;
            var responseJson = response.Content.ReadAsStringAsync().Result;

            var following = "";
            try
            {
                following = responseJson.Split(new string[] { " Following" }, StringSplitOptions.None)[0].Split(new string[] { ", " }, StringSplitOptions.None).Last().Trim().Replace(",", "").Replace(".", "");
            }
            catch
            {
                following = "0";
            }

            var followers = "";
            try
            {
                followers = responseJson.Split(new string[] { "userInteractionCount\":\"" }, StringSplitOptions.None)[1].Split('"')[0];
            }
            catch
            {
                followers = "0";
            }

            var foto = "";
            try
            {
                foto = responseJson.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None)[1].Split('"')[0];
            }
            catch
            {
                foto = "https://www.yourbusinessfreedom.com.au/wp-content/uploads/2017/02/unknown-profile-1.jpg";
            }


            var posts = ""; 
            try
            {
                posts = responseJson.Split(new string[] { " Posts" }, StringSplitOptions.None)[0].Split(new string[] { ", " }, StringSplitOptions.None).Last().Trim().Replace(",", "").Replace(".", "");
            }
            catch
            {
                posts = "0";
            }

            var user = new InstagramUserData()
            {
                Followers = Int32.Parse(followers),
                Picture = new Uri(foto),
                InstagramUser = username,
                Following = Int32.Parse(following),
                Posts = Int32.Parse(posts)
            };

            return user;
        }

        public List<InstagramUserData> GetUsers()
        {
            var standuperoService = new StanduperoService();
            var standuperos = standuperoService.GetStanduperos();
            var lista = new List<InstagramUserData>();
            foreach (var standupero in standuperos)
            {
                lista.Add(GetUserBy(standupero.InstagramUser.Replace("@", "").Trim().ToLower()));
            }
            return lista;
        }


    }
}