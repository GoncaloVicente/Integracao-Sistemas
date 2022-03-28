using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalAPI.Models
{
    public class Sensor
    {
        public int id { get; set; }
        public string name { get; set; }
        public string localization { get; set; }
        public string username { get; set; }
    }
}