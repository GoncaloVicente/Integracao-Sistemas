using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Grahps
{
    public class Sensor
    {
        public Sensor(int id, float temperature, float humidity, int battery, DateTime timestamp)
        {
            Id = id;
            Temperature = temperature;
            Humidity = humidity;
            Battery = battery;
            Timestamp = timestamp;
        }

        public int Id { get; set; }

        public float Temperature { get; set; }

        public float Humidity { get; set; }

        public int Battery { get; set; }
        public DateTime Timestamp { get; set; }

    }
}
