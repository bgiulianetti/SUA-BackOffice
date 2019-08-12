using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Models
{
    public class InstagramUserRankingPeriod
    {
        public string Username { get; set; }
        public string ProfilePicture { get; set; }
        public int Weekly { get; set; }
        public int WeeklyPercentage { get; set; }
        public int Monthly { get; set; }
        public int MonthlyPercentage { get; set; }
        public int SemiAnnually { get; set; }
        public int SemiAnnuallyPercentage { get; set; }
    }
}