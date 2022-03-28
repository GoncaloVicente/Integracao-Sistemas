using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalAPI.Models
{
    public class SensorData
    {
        public int id { get; set; }
        public int id_sensor { get; set; }
        public float temperature { get; set; }
        public float humidity { get; set; }
        public int battery { get; set; }
        public long timestamp { get; set; }
    }
}