using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalAPI.Models
{
    public class Alert
    {
        public int id { get; set; }
        public int id_sensor_data { get; set; }
        public string alert_msg { get; set; }
        public long timestamp { get; set; }
    }
}