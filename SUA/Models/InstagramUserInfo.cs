using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Models
{
    public class InstagramUserInfo
    {
        public int Publicaciones { get; set; }
        public int Followers { get; set; }
        public string User { get; set; }
        public string Followed { get; set; }
    }
}