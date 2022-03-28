using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Database
{
    class Program
    {
        private static string connectionString = Properties.Settings.Default.ConnStr;
        private static string sqlQuery = "";

        public static string XmlFilePath { get; set; }

        static void Main(string[] args)
        {
            MqttClient mClient = new MqttClient("test.mosquitto.org");
            string[] topics = { "alerts", "info" };

            mClient.Connect(Guid.NewGuid().ToString());

            if (!mClient.IsConnected)
            {
                Console.WriteLine("Error connecting to message broker...");
                return;
            }

            byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };

            mClient.Subscribe(topics, qosLevels);
            Console.WriteLine("Waiting for data...");
            mClient.MqttMsgPublishReceived += MClient_MqttMsgPublishReceived;
        }

        private static void MClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            if (e.Topic.Equals("info"))
                infoMsg(Encoding.UTF8.GetString(e.Message));
            else
                alertsMsg(Encoding.UTF8.GetString(e.Message));
        }

        private static void infoMsg(String message)
        {
            // Console.WriteLine(message);
            // Console.Read();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(message);

            
            XmlNodeList list = doc.GetElementsByTagName("sensor");
            int sensor = int.Parse(list[0].Attributes[0].Value);

            list = doc.GetElementsByTagName("temperature");
            float temperature = float.Parse(list[0].InnerText);

            list = doc.GetElementsByTagName("humidity");
            float humidity = float.Parse(list[0].InnerText);

            list = doc.GetElementsByTagName("battery");
            int battery = int.Parse(list[0].InnerText);

            list = doc.GetElementsByTagName("timestamp");
            long timestamp = long.Parse(list[0].InnerText);


            sqlQuery = "INSERT INTO Data ([Id_Sensor], [Temperature], [Humidity], [Battery], [Timestamp], [VALID]) VALUES (@sensor,@temperature,@humidity,@battery,@timestamp,1);";
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                    {
                        cmd.Parameters.Add("@sensor", SqlDbType.Int).Value = sensor;
                        cmd.Parameters.Add("@temperature", SqlDbType.Float).Value = temperature;
                        cmd.Parameters.Add("@humidity", SqlDbType.Float).Value = humidity;
                        cmd.Parameters.Add("@battery", SqlDbType.Int).Value = battery;
                        cmd.Parameters.Add("@timestamp", SqlDbType.BigInt).Value = timestamp;

                        int rowsAdded = cmd.ExecuteNonQuery();
                        if (rowsAdded > 0)
                        {
                            Console.WriteLine("Data inserted!!");
                        }
                        else
                            Console.WriteLine("No data inserted...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR: " + ex.Message);
                }
            }
        }

        private static void alertsMsg(String message)
        {
            // Console.WriteLine(message);
            // Console.Read();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(message);


            XmlNodeList list = doc.GetElementsByTagName("sensor");
            int sensor = int.Parse(list[0].Attributes[0].Value);

            list = doc.GetElementsByTagName("temperature");
            float temperature = float.Parse(list[0].InnerText);
            
            list = doc.GetElementsByTagName("humidity");
            float humidity = float.Parse(list[0].InnerText);

            list = doc.GetElementsByTagName("battery");
            int battery = int.Parse(list[0].InnerText);

            list = doc.GetElementsByTagName("timestamp");
            long timestamp = long.Parse(list[0].InnerText);

            list = doc.GetElementsByTagName("alert");
            string alert = list[0].InnerText.ToString();


            sqlQuery = "UPDATE Data SET Alert=@alert WHERE [Id_Sensor]=@sensor AND [Temperature]=@temperature AND [Humidity]=@humidity AND [Timestamp]=@timestamp;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                    {
                        cmd.Parameters.Add("@sensor", SqlDbType.Int).Value = sensor;
                        cmd.Parameters.Add("@temperature", SqlDbType.Float).Value = temperature;
                        cmd.Parameters.Add("@humidity", SqlDbType.Float).Value = humidity;
                        cmd.Parameters.Add("@battery", SqlDbType.Int).Value = battery;
                        cmd.Parameters.Add("@timestamp", SqlDbType.BigInt).Value = timestamp;
                        cmd.Parameters.Add("@alert", SqlDbType.NVarChar).Value = alert;

                        int rowsAdded = cmd.ExecuteNonQuery();
                        if (rowsAdded > 0)
                        {
                            Console.WriteLine("Alert inserted!!");
                        }
                        else
                            Console.WriteLine("No alert inserted");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR: " + ex.Message);
                }
            }

        }
    }
}