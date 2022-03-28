using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Grahps
{
    public partial class Form1 : Form
    {
        const String STR_CHANNEL_NAME = "info";
        MqttClient m_cClient = new MqttClient("test.mosquitto.org");
        List<Sensor> gridData;

        string[] m_strTopicsInfo = { STR_CHANNEL_NAME };

        public Form1()
        {
            InitializeComponent();
            //this.gridData = new List<Sensor>();
            this.sensorBindingSource.DataSource = new List<Sensor>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                m_cClient.Connect(Guid.NewGuid().ToString());
                if (!m_cClient.IsConnected)
                {
                    MessageBox.Show("Error connecting to message broker...");
                    return;
                }

                backgroundWorker1.RunWorkerAsync();

                //Subscribe chat channel
                m_cClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

                byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };//QoS
                m_cClient.Subscribe(m_strTopicsInfo, qosLevels);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Error connecting to message broker...");
            }

        }

        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            //Console.WriteLine("Received = " + Encoding.UTF8.GetString(e.Message) + " on topic " + e.Topic);            
            //EXTRACT FIELDS

            String strTemp = Encoding.UTF8.GetString(e.Message);
            //xmlDoc.Load(strTemp);

            //Sensor sensor;

            //var serializer = new XmlSerializer(typeof(Sensor));
            //using (TextReader reader = new StringReader(strTemp))

            Sensor sensor = getSensor(strTemp);
            

            backgroundWorker1.ReportProgress(50,sensor);



        }

        private Sensor getSensor(string info)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(info);

            int id = 0;
            float temperature = 0;
            float humidity = 0;
            int battery = 0;
            long timestamp;

            XmlNodeList nodes = xdoc.GetElementsByTagName("sensor");
            id = int.Parse(nodes[0].Attributes[0].Value);

            nodes = xdoc.GetElementsByTagName("temperature");
            temperature = float.Parse(nodes[0].InnerText);

            nodes = xdoc.GetElementsByTagName("humidity");
            humidity = float.Parse(nodes[0].InnerText);

            nodes = xdoc.GetElementsByTagName("battery");
            battery = int.Parse(nodes[0].InnerText);

            nodes = xdoc.GetElementsByTagName("timestamp");
            timestamp = long.Parse(nodes[0].InnerText);

            return new Sensor(id, temperature, humidity, battery, Date.getDate(timestamp));
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Thread.Sleep(10000); //Para poupar o processador
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            Sensor sensor = (Sensor)e.UserState; //o report progress tem uma ferramenta para passar objetos
            if (this.chart1.Series["Temperatura"].Points.Count == 20)
                this.chart1.Series["Temperatura"].Points.RemoveAt(0);

            this.chart1.Series["Temperatura"].Points.AddXY(sensor.Timestamp, sensor.Temperature);

            if (this.chart1.Series["Humidade"].Points.Count == 20)
                this.chart1.Series["Humidade"].Points.RemoveAt(0);

            this.chart1.Series["Humidade"].Points.AddXY(sensor.Timestamp, sensor.Humidity);
            this.chart1.Update();

            //tratar ordenacao
            List<Sensor> aux = (List<Sensor>) this.sensorBindingSource.List;
            aux.Add(sensor);
            List<Sensor> auxSorted = aux.OrderByDescending(o => o.Timestamp).ToList();
            this.sensorBindingSource.DataSource = auxSorted;

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_cClient.IsConnected)
            {
                m_cClient.Unsubscribe(m_strTopicsInfo); //Put this in a button to see notif!
                m_cClient.Disconnect(); //Free process and process's resources
            }
        }
    }
}
