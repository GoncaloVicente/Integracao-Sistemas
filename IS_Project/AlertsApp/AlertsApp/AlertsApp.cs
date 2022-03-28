using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace AlertsApp
{
    public partial class AlertsApp : Form
    {

        string FILENAME = Application.StartupPath + @"\App_Data\alert_conditions.xml";

        List<string> parameters = new List<string> { "Temperature", "Humidity", "Battery" };
        List<string> types = new List<string> { ">", "<", "=", "between" };
        List<AlertCondition> alertConditions = new List<AlertCondition> { };

        //Broker
        MqttClient mqttClient = null;
        string[] topics = { "info", "alerts"};

        bool connectBroker = false;

        public AlertsApp()
        {
            InitializeComponent();
        }

        private void AlertsApp_Load(object sender, EventArgs e)
        {
            cbParameter.DataSource = parameters;
            cbType.DataSource = types;
            lblAnd.Enabled = false;
            txtValue2.Enabled = false;
            btnUpdateAlert.Enabled = false;

            connectBroker = connect();

            if (!connectBroker)
            {
                btnBroker.Visible = true;
            }

            readFileAlertConditions();
            updateTable();
        }

        private void MqttClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                if (e.Topic == "info")
                {
                    string msg = readMessages(Encoding.UTF8.GetString(e.Message));
                    if (msg != null)
                    {
                        msg = constructMsgAlert(Encoding.UTF8.GetString(e.Message), msg);
                        mqttClient.Publish("alerts", Encoding.UTF8.GetBytes(msg));
                    }
                }
            });
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbType.SelectedItem.ToString() == "between")
            {
                txtValue2.Enabled = true;
                lblAnd.Enabled = true;
            }
            else
            {
                txtValue2.Clear();
                txtValue2.Enabled = false;
                lblAnd.Enabled = false;
            }
        }

        private void btnCreateAlert_Click(object sender, EventArgs e)
        {
            if (validation(txtValue.Text, txtValue2.Text))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(FILENAME);

                XmlNode node = doc.SelectSingleNode("/alertConditions");

                XmlElement condition = doc.CreateElement("condition");
                string type = cbType.SelectedItem.ToString();
                if (type == ">")
                {
                    type = "bigger";
                }
                else if (type == "<")
                {
                    type = "smaller";
                }
                else if (type == "=")
                {
                    type = "equal";
                }
                condition.SetAttribute("type", type);
                condition.SetAttribute("parameter", cbParameter.SelectedItem.ToString());

                XmlElement value1 = doc.CreateElement("value1");
                value1.InnerText = txtValue.Text.Replace('.', ',');
                XmlElement enable = doc.CreateElement("enable");
                enable.InnerText = cbEnable.Checked.ToString();

                condition.AppendChild(value1);
                condition.AppendChild(enable);

                String msg;
                if (txtMessage.Text.Trim().Length > 0)
                {
                    XmlElement message = doc.CreateElement("message");
                    message.InnerText = txtMessage.Text;
                    msg = txtMessage.Text;
                    condition.AppendChild(message);
                }
                else
                {
                    XmlElement message = doc.CreateElement("message");
                    message.InnerText = "Alert relative to " + cbParameter.SelectedItem.ToString();
                    msg = message.InnerText;
                    condition.AppendChild(message);
                }

                if (txtValue2.Text.Trim().Length > 0)
                {
                    XmlElement value2 = doc.CreateElement("value2");
                    value2.InnerText = txtValue2.Text.Replace('.', ',');
                    condition.AppendChild(value2);
                }

                node.AppendChild(condition);

                AlertCondition newAlertCondition;
                if (txtValue2.Text.Trim().Length < 1)
                {
                    newAlertCondition = new AlertCondition(cbParameter.SelectedItem.ToString(), type, float.Parse(txtValue.Text.Replace('.', ',')), cbEnable.Checked, msg);
                }
                else
                {
                    newAlertCondition = new AlertCondition(cbParameter.SelectedItem.ToString(), type, float.Parse(txtValue.Text.Replace('.', ',')), float.Parse(txtValue2.Text.Replace('.', ',')), cbEnable.Checked, msg);
                }

                if (exists(newAlertCondition))
                {
                    MessageBox.Show("Already exists an alert condiction with same parameter, type and values");
                    return;
                }

                doc.Save(FILENAME);
                lbAlertConditions.Items.Add(newAlertCondition);
                alertConditions.Add(newAlertCondition);
            }
        }

        private bool validation(string value1, string value2)
        {
            value1 = value1.Replace('.', ',');
            //Between -> Both values are required
            if (cbType.SelectedItem.ToString() == "between")
            {
                if (value1.Trim().Length < 1 || value2.Trim().Length < 1)
                {
                    MessageBox.Show("Both values are required");
                    return false;
                }

                value2 = value2.Replace('.', ',');
                if (!isFloat(value1) || !isFloat(value2))
                {
                    MessageBox.Show("Values cannot contain letters");
                    return false;
                }
                if (float.Parse(value2) <= float.Parse(value1))
                {
                    MessageBox.Show("The second value must be bigger than the first one");
                    return false;
                }

                return true;
            }

            //< or > or = -> First value is required
            if (value1.Trim().Length < 1)
            {
                MessageBox.Show("The first value is required");
                return false;
            }
            if (!isFloat(value1))
            {
                MessageBox.Show("Value cannot contain letters");
                return false;
            }

            return true;
        }

        private bool isFloat(string value)
        {
            try
            {
                float.Parse(value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void readFileAlertConditions()
        {
            AlertCondition alertCondition = null;
            XmlTextReader reader = new XmlTextReader(FILENAME);

            string parameter = null;
            string type = null;
            float value1 = float.NaN;
            float value2 = float.NaN;
            bool enable = false;
            string message = null;

            int cont = 0;
            bool isBetween = false;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        if (reader.Name == "condition")
                        {
                            parameter = reader.Name;
                            type = reader.GetAttribute("type");
                            parameter = reader.GetAttribute("parameter");
                            if (type == "between")
                            {
                                isBetween = true;
                            }
                        }
                        else if (reader.Name != "alertConditions")
                        {
                            cont++;
                        }
                        break;
                    case XmlNodeType.Text:
                        if (cont == 1)
                        {
                            value1 = float.Parse(reader.Value);
                        }
                        else if (cont == 2)
                        {
                            enable = reader.Value == "True" ? true : false;
                        }
                        else if (cont == 3)
                        {
                            message = reader.Value;
                        }
                        else if (cont == 4 && isBetween)
                        {
                            value2 = float.Parse(reader.Value);
                        }
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        if (reader.Name == "condition")
                        {
                            if (float.IsNaN(value2))
                            {
                                alertCondition = new AlertCondition(parameter, type, value1, enable, message);
                            }
                            else
                            {
                                alertCondition = new AlertCondition(parameter, type, value1, value2, enable, message);
                            }
                            alertConditions.Add(alertCondition);
                            cont = 0;
                            isBetween = false;
                            value2 = float.NaN;
                        }
                        break;
                }
            }
            reader.Close();
        }

        private void updateTable()
        {
            lbAlertConditions.Items.Clear();
            for (int i = 0; i < alertConditions.Count; i++)
            {
                lbAlertConditions.Items.Add(alertConditions.ElementAt(i));
            }
        }

        private void lbAlertConditions_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnUpdateAlert.Enabled = true;
            btnRemoveAlert.Enabled = true;
            if (lbAlertConditions.SelectedIndex < 0)
            {
                MessageBox.Show("Error in selection");
                return;
            }
            AlertCondition alertCondition = alertConditions.ElementAt(lbAlertConditions.SelectedIndex);
            cbParameter.SelectedItem = alertCondition.parameter;
            if (alertCondition.type == "bigger")
            {
                cbType.SelectedItem = ">";
            }
            else if (alertCondition.type == "smaller")
            {
                cbType.SelectedItem = "<";
            }
            else if (alertCondition.type == "equal")
            {
                cbType.SelectedItem = "=";
            }
            else if (alertCondition.type == "between")
            {
                cbType.SelectedItem = "between";
            }
            txtValue.Text = alertCondition.value1.ToString();
            cbEnable.Checked = alertCondition.enable;
            if (!float.IsNaN(alertCondition.value2))
            {
                txtValue2.Text = alertCondition.value2.ToString();
            }
            txtMessage.Text = alertCondition.message;
        }

        private void btnUpdateAlert_Click(object sender, EventArgs e)
        {
            if (lbAlertConditions.SelectedIndex < 0)
            {
                MessageBox.Show("Please select one alert condiction");
                return;
            }
            AlertCondition alertCondition = alertConditions.ElementAt(lbAlertConditions.SelectedIndex);
            if (validation(txtValue.Text, txtValue2.Text))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(FILENAME);

                XmlNode node = doc.SelectSingleNode("/alertConditions/condition[position()="+(lbAlertConditions.SelectedIndex + 1)+"]");

                string type = cbType.SelectedItem.ToString();
                if (type == ">")
                {
                    type = "bigger";
                }
                else if (type == "<")
                {
                    type = "smaller";
                }
                else if (type == "=")
                {
                    type = "equal";
                }
                alertCondition.type = type;
                node.Attributes[0].Value = type;
                alertCondition.parameter = cbParameter.SelectedItem.ToString();
                node.Attributes[1].Value = cbParameter.SelectedItem.ToString();

                alertCondition.value1 = float.Parse(txtValue.Text.Replace('.', ','));
                node.ChildNodes[0].InnerText = txtValue.Text.Replace('.', ',');
                alertCondition.enable = cbEnable.Checked;
                node.ChildNodes[1].InnerText = cbEnable.Checked.ToString();

                String msg;
                if (txtMessage.Text.Trim().Length > 0)
                {
                    alertCondition.message = txtMessage.Text;
                    node.ChildNodes[2].InnerText = txtMessage.Text;
                }
                else
                {
                    alertCondition.message = "Alert relative to " + cbParameter.SelectedItem.ToString();
                    node.ChildNodes[2].InnerText = alertCondition.message;
                }

                if (txtValue2.Text.Trim().Length > 0)
                {
                    alertCondition.value2 = float.Parse(txtValue2.Text.Replace('.', ','));
                    if (node.ChildNodes.Count == 3)
                    {
                        XmlElement value2 = doc.CreateElement("value2");
                        value2.InnerText = txtValue2.Text.Replace('.', ',');
                        node.AppendChild(value2);
                    }
                    else
                    {
                        node.LastChild.InnerText = txtValue2.Text.Replace('.', ',');
                    }
                }
                else
                {
                    if (node.LastChild.Name == "value2")
                    {
                        alertCondition.value2 = float.NaN;
                        node.RemoveChild(node.LastChild);
                    }
                }

                doc.Save(FILENAME);
                updateTable();
            }
        }

        private bool exists(AlertCondition alertCondition)
        {
            for (int i=0;i<alertConditions.Count;i++)
            {
                if (alertConditions.ElementAt(i).isEqual(alertCondition))
                {
                    return true;
                }
            }
            return false;
        }

        private void btnRemoveAlert_Click(object sender, EventArgs e)
        {
            if (lbAlertConditions.SelectedIndex < 0)
            {
                MessageBox.Show("Please select one alert condiction");
                return;
            }
            DialogResult confirm = MessageBox.Show("Are you sure that you want to remove this alert condiction?", "Delete Alert Condiction", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (confirm.ToString().ToUpper() == "YES") {
                alertConditions.RemoveAt(lbAlertConditions.SelectedIndex);

                XmlDocument doc = new XmlDocument();
                doc.Load(FILENAME);

                XmlNode node = doc.SelectSingleNode("/alertConditions/condition[position()="+(lbAlertConditions.SelectedIndex+1)+"]");
                node.ParentNode.RemoveChild(node);

                doc.Save(FILENAME);
                updateTable();
            }
        }

        private string readMessages(string info)
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

            string msg = verifyAlert(temperature, humidity, battery);
            if (msg != null)
            {
                txtAlertsLog.Text += Environment.NewLine + "(Sensor "+id+")Alert Triggered - "+msg+" at "+Date.getDate(timestamp);
            }

            return msg;
        }

        private string verifyAlert(float temperature, float humidity, int battery)
        {
            for (int i=0;i<alertConditions.Count;i++)
            {
                AlertCondition alertCondition = alertConditions.ElementAt(i);
                if (alertCondition.enable == false)
                {
                    continue;
                }
                if (alertCondition.parameter == "Temperature")
                {
                    if (alertCondition.type == "smaller")
                    {
                        if (temperature < alertCondition.value1)
                        {
                            return alertCondition.message;
                        }
                    }else if (alertCondition.type == "bigger")
                    {
                        if (temperature > alertCondition.value1)
                        {
                            return alertCondition.message;
                        }

                    }
                    else if (alertCondition.type == "equal")
                    {
                        if (temperature == alertCondition.value1)
                        {
                            return alertCondition.message;
                        }
                    }
                    else if (alertCondition.type == "between")
                    {
                        if (temperature > alertCondition.value1 && temperature < alertCondition.value2)
                        {
                            return alertCondition.message;
                        }
                    }
                }
                else if (alertCondition.parameter == "Humidity")
                {
                    if (alertCondition.type == "smaller")
                    {
                        if (humidity < alertCondition.value1)
                        {
                            return alertCondition.message;
                        }
                    }
                    else if (alertCondition.type == "bigger")
                    {
                        if (humidity > alertCondition.value1)
                        {
                            return alertCondition.message;
                        }

                    }
                    else if (alertCondition.type == "equal")
                    {
                        if (humidity == alertCondition.value1)
                        {
                            return alertCondition.message;
                        }
                    }
                    else if (alertCondition.type == "between")
                    {
                        if (humidity > alertCondition.value1 && humidity < alertCondition.value2)
                        {
                            return alertCondition.message;
                        }
                    }
                }
                else if (alertCondition.parameter == "Battery")
                {
                    if (alertCondition.type == "smaller")
                    {
                        if (battery < alertCondition.value1)
                        {
                            return alertCondition.message;
                        }
                    }
                    else if (alertCondition.type == "bigger")
                    {
                        if (battery > alertCondition.value1)
                        {
                            return alertCondition.message;
                        }

                    }
                    else if (alertCondition.type == "equal")
                    {
                        if (battery == alertCondition.value1)
                        {
                            return alertCondition.message;
                        }
                    }
                    else if (alertCondition.type == "between")
                    {
                        if (battery > alertCondition.value1 && temperature < alertCondition.value2)
                        {
                            return alertCondition.message;
                        }
                    }
                }
            }
            return null;
        }

        private string constructMsgAlert(string info, string msg)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(info);

            XmlElement alertElement = xdoc.CreateElement("alert");
            alertElement.InnerText = msg;

            xdoc.GetElementsByTagName("sensor")[0].AppendChild(alertElement);

            return xdoc.InnerXml;
        }

        private bool connect()
        {
            //Broker
            try
            {
                mqttClient = new MqttClient("test.mosquitto.org");
                mqttClient.Connect(Guid.NewGuid().ToString());
                if (!mqttClient.IsConnected)
                {
                    MessageBox.Show("Error connecting to message broker");
                    return false;
                }
                byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };
                mqttClient.Subscribe(topics, qosLevels);

                mqttClient.MqttMsgPublishReceived += MqttClient_MqttMsgPublishReceived;

                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to connect to broker", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnBroker_Click(object sender, EventArgs e)
        {
            connectBroker = connect();

            if (!connectBroker)
            {
                btnBroker.Visible = true;
            }
            else
            {
                btnBroker.Visible = false;
                MessageBox.Show("Connect to broker");
            }
        }

        private void AlertsApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mqttClient.IsConnected)
            {
                mqttClient.Disconnect();
            }
        }
    }
}
