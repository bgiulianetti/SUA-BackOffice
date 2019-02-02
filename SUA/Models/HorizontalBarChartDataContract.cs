using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Models
{
    public class HorizontalBarChartDataContract
    {
        public string label { get; set; }
        public int y { get; set; }
        public int posts { get; set; }
        public int seguidos { get; set; }
        public string url { get; set; }
    }
}