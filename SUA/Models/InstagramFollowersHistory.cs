using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Models
{
    public class InstagramUser
    {
        public string Username { get; set; }
        public string ProfilePicture { get; set; }
        public int Posts { get; set; }
        public int Following { get; set; }
        public List<InstragramUserFollowersHistory> FollowersLegacy { get; set; }
        public List<InstragramUserFollowersHistory> Followers { get; set; }
        public string Status { get; set; }
    }

    public class InstragramUserFollowersHistory
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
        public int Difference { get; set; }
    }
}