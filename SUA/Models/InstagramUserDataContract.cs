using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Models
{
    public class InstagramUserDataContract
    {
        public string user { get; set; }
        public string pointStart { get; set; }
        public int[] data { get; set; }
    }
}