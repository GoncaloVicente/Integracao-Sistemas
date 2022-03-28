using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace BinaryTranslator {
    public partial class Form1 : Form {
        string FILENAME = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\data.bin";
        //Broker
        MqttClient mqttClient = null;
        string[] topics = { "info"};
        public Form1() {
            InitializeComponent();
        }

        private void sendData_Click(object sender, EventArgs e) {
            mqttClient = new MqttClient("test.mosquitto.org");
            mqttClient.Connect(Guid.NewGuid().ToString());
            if (!mqttClient.IsConnected) {
                MessageBox.Show("Error connecting to message broker");
                return;
            }

            using (FileStream fs = new FileStream(FILENAME, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)) {
                byte[] b = new byte[1024];
                do {
                    ReadDataFromFile(fs);
                    if (keepSending.Checked) {
                        System.Threading.Thread.Sleep(10000);
                    }                    
                } while (fs.ReadByte() > 0 || keepSending.Checked);
            }
        }

        private void ReadDataFromFile(FileStream fs) {
            String hex, discard = "", aux = "";
            float temp, hum;
            int bat, timestamp;

            hex = "<sensor id=\"" + string.Format("{0:X2}", fs.ReadByte()) + "\">\n";
            for (int i = 0; i < 3; i++) {
                discard += fs.ReadByte();
            }
            discard = "";
            for (int i = 0; i < 4; i++) {
                if (i == 0) {
                    discard += string.Format("{0:X2}", fs.ReadByte());
                } else {
                    aux = discard;
                    discard = "";
                    discard += string.Format("{0:X2}", fs.ReadByte());
                    discard += aux;
                    aux = "";
                }
            }
            aux = "0x";
            aux += discard;
            byte[] bytes = null;
            try
            {
                bytes = BitConverter.GetBytes(Convert.ToInt64(aux, 16));
            }
            catch (Exception e)
            {
                keepSending.Checked = false;
                return;
            }
            temp = BitConverter.ToSingle(bytes, 0);
            discard = aux = "";
            hex += "<temperature> " + temp.ToString() + " </temperature>\n";

            for (int i = 0; i < 4; i++) {
                if (i == 0) {
                    discard += string.Format("{0:X2}", fs.ReadByte());
                } else {
                    aux = discard;
                    discard = "";
                    discard += string.Format("{0:X2}", fs.ReadByte());
                    discard += aux;
                    aux = "";
                }
            }
            aux = "0x";
            aux += discard;
            bytes = BitConverter.GetBytes(Convert.ToInt32(aux, 16));
            hum = BitConverter.ToSingle(bytes, 0);
            discard = aux = "";
            hex += "<humidity> " + hum.ToString() + " </humidity>\n";

            bat = fs.ReadByte();
            hex += "<battery> " + bat.ToString() + " </battery>\n";
            for (int i = 0; i < 3; i++) {
                discard += fs.ReadByte();
            }
            discard = "";

            for (int i = 0; i < 4; i++) {
                if (i == 0) {
                    discard += string.Format("{0:X2}", fs.ReadByte());
                } else {
                    aux = discard;
                    discard = "";
                    discard += string.Format("{0:X2}", fs.ReadByte());
                    discard += aux;
                    aux = "";
                }
            }
            aux = "0x";
            aux += discard;
            bytes = BitConverter.GetBytes(Convert.ToInt32(aux, 16));
            timestamp = BitConverter.ToInt32(bytes, 0);
            discard = aux = "";
            hex += "<timestamp> " + timestamp.ToString() + " </timestamp>\n</sensor>";
            for (int i = 0; i < 3; i++) {
                discard += fs.ReadByte();
            }
            discard = "";
            Console.WriteLine(hex);
            mqttClient.Publish("info", Encoding.UTF8.GetBytes(hex));
        }
    }
}
