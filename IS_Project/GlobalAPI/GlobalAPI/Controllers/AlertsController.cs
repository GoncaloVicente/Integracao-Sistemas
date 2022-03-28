using GlobalAPI.Controllers.Authentication;
using GlobalAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GlobalAPI.Controllers
{
    /*
    -> Obter dados dos alarmes
    */
    public class AlertsController : ApiController
    {
        static string connectionStr = Properties.Settings.Default.connectionString;

        // GET: api/sensors/alerts
        // Obter lista de todos os alertas
        [BasicAuthentication]
        [Route("api/sensors/alerts")]
        public IHttpActionResult GetAllAlerts()
        {
            List<Alert> list = new List<Alert>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionStr);
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM DATA WHERE valid=1 and alert is not null", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Alert a = new Alert
                    {
                        id = (int)reader["id"],
                        id_sensor_data = (int)reader["id_sensor"],
                        alert_msg = (string)reader["alert"],
                        timestamp = long.Parse(reader["timestamp"].ToString())
                    };
                    list.Add(a);
                }

                reader.Close();
                conn.Close();
                return Ok(list);
            }
            catch (Exception e)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }

                return NotFound();
            }
        }

        // GET: api/sensors/1/alerts
        // Obter lista de todos os alertas de um sensor
        [BasicAuthentication]
        [Route("api/sensors/{id:int}/alerts")]
        public IHttpActionResult GetAlertsBySensorId(int id)
        {
            List<Alert> list = new List<Alert>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionStr);
                conn.Open();

                //Verificar se o sensor existe
                SqlCommand cmd = new SqlCommand("SELECT * FROM SENSORS WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    reader.Close();
                    conn.Close();
                    return NotFound();
                }

                reader.Close();

                cmd = new SqlCommand("SELECT * FROM DATA WHERE valid=1 and alert is not null and id_sensor=@sensor", conn);
                cmd.Parameters.AddWithValue("@sensor", id);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Alert a = new Alert
                    {
                        id = (int)reader["id"],
                        id_sensor_data = (int)reader["id_sensor"],
                        alert_msg = (string)reader["alert"],
                        timestamp = long.Parse(reader["timestamp"].ToString())
                    };
                    list.Add(a);
                }

                reader.Close();
                conn.Close();
                return Ok(list);
            }
            catch (Exception e)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }

                return NotFound();
            }
        }

        // GET: api/sensors/bar/alerts
        // Obter lista de todos os alertas de todos os sensores de uma dada localização
        [BasicAuthentication]
        [Route("api/sensors/{localization}/alerts")]
        public IHttpActionResult GetAlertsByLocalization(string localization)
        {
            List<Alert> list = new List<Alert>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionStr);
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM DATA d join SENSORS s ON d.id_sensor=s.id WHERE valid=1 and upper(localization)=@localization and alert is not null", conn);
                cmd.Parameters.AddWithValue("@localization", localization.ToUpper());
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Alert a = new Alert
                    {
                        id = (int)reader["id"],
                        id_sensor_data = (int)reader["id_sensor"],
                        alert_msg = (string)reader["alert"],
                        timestamp = long.Parse(reader["timestamp"].ToString())
                    };
                    list.Add(a);
                }

                reader.Close();
                conn.Close();
                return Ok(list);
            }
            catch (Exception e)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }

                return NotFound();
            }
        }

        // GET: api/sensors/alerts/small/10122019
        // Obter lista de todos os alertas dentro de um intervalo de tempo
        // Timestamp: Long
        [BasicAuthentication]
        [Route("api/sensors/alerts/{condiction}/{date1}/{date2?}")]
        public IHttpActionResult GetAlertsWithTemporalInterval(string condiction = "", string date1 = "", string date2 = "")
        {
            condiction = condiction.ToUpper();
            if (!checkCondiction(condiction))
            {
                return BadRequest("Invalid condiction");
            }
            if (!checkDates(date1, date2))
            {
                return BadRequest("Invalid date [ddMMyyyy]");
            }
            if (condiction == "BETWEEN" && date2.Length < 1)
            {
                return BadRequest("You send 'between' condiction so you need to pass two dates");
            }
            if (condiction != "BETWEEN" && date2.Length > 0)
            {
                return BadRequest("You dont't send 'between' condiction so you need to pass just one date");
            }

            DateTime firstDateTime = new DateTime(int.Parse(date1.Substring(4, 4)), int.Parse(date1.Substring(2, 2)), int.Parse(date1.Substring(0, 2)));
            DateTime secondDateTime = new DateTime();
            long? secondDate = null;
            if (date2.Length > 0)
            {
                secondDateTime = new DateTime(int.Parse(date2.Substring(4, 4)), int.Parse(date2.Substring(2, 2)), int.Parse(date2.Substring(0, 2)));
                secondDate = Date.getUnixTime(secondDateTime);
            }

            long firstDate = Date.getUnixTime(firstDateTime);

            if (date2.Length > 0)
            {
                if (firstDate > secondDate)
                {
                    return BadRequest("First date must be earlier or equal than second date");
                }
            }

            List<Alert> list = new List<Alert>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionStr);
                conn.Open();

                SqlCommand cmd;
                SqlDataReader reader;

                if (condiction == "SMALL")
                {

                    cmd = new SqlCommand("SELECT * FROM DATA WHERE valid=1 and timestamp<@date and alert is not null", conn);
                    cmd.Parameters.AddWithValue("@date", firstDate);
                    reader = cmd.ExecuteReader();

                }
                else if (condiction == "BIG")
                {

                    cmd = new SqlCommand("SELECT * FROM DATA WHERE valid=1 and timestamp>@date and alert is not null", conn);
                    cmd.Parameters.AddWithValue("@date", firstDate);
                    reader = cmd.ExecuteReader();

                }
                else
                {

                    cmd = new SqlCommand("SELECT * FROM DATA WHERE valid=1 and timestamp>@date1 and timestamp<@date2 and alert is not null", conn);
                    cmd.Parameters.AddWithValue("@date1", firstDate);
                    cmd.Parameters.AddWithValue("@date2", secondDate);
                    reader = cmd.ExecuteReader();

                }

                while (reader.Read())
                {
                    Alert a = new Alert
                    {
                        id = (int)reader["id"],
                        id_sensor_data = (int)reader["id_sensor"],
                        alert_msg = (string)reader["alert"],
                        timestamp = long.Parse(reader["timestamp"].ToString())
                    };
                    list.Add(a);
                }

                reader.Close();
                conn.Close();
                return Ok(list);
            }
            catch (Exception e)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }

                return NotFound();
            }
        }

        // GET: api/sensors/bar/alerts/small/10122019
        // Obter lista de todos os alertas dentro de um intervalo de tempo e localização
        // Timestamp: Long
        [BasicAuthentication]
        [Route("api/sensors/{localization}/alerts/{condiction}/{date1}/{date2?}")]
        public IHttpActionResult GetSensorDataWithTemporalIntervalByLocalization(string localization = "", string condiction = "", string date1 = "", string date2 = "")
        {
            condiction = condiction.ToUpper();
            if (!checkCondiction(condiction))
            {
                return BadRequest("Invalid condiction");
            }
            if (!checkDates(date1, date2))
            {
                return BadRequest("Invalid date [ddMMyyyy]");
            }
            if (condiction == "BETWEEN" && date2.Length < 1)
            {
                return BadRequest("You send 'between' condiction so you need to pass two dates");
            }
            if (condiction != "BETWEEN" && date2.Length > 0)
            {
                return BadRequest("You dont't send 'between' condiction so you need to pass just one date");
            }

            DateTime firstDateTime = new DateTime(int.Parse(date1.Substring(4, 4)), int.Parse(date1.Substring(2, 2)), int.Parse(date1.Substring(0, 2)));
            DateTime secondDateTime = new DateTime();
            long? secondDate = null;
            if (date2.Length > 0)
            {
                secondDateTime = new DateTime(int.Parse(date2.Substring(4, 4)), int.Parse(date2.Substring(2, 2)), int.Parse(date2.Substring(0, 2)));
                secondDate = Date.getUnixTime(secondDateTime);
            }

            long firstDate = Date.getUnixTime(firstDateTime);

            if (date2.Length > 0)
            {
                if (firstDate > secondDate)
                {
                    return BadRequest("First date must be earlier or equal than second date");
                }
            }

            List<Alert> list = new List<Alert>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionStr);
                conn.Open();

                SqlCommand cmd;
                SqlDataReader reader;

                if (condiction == "SMALL")
                {

                    cmd = new SqlCommand("SELECT * FROM DATA d join SENSORS s ON d.id_sensor = s.id WHERE valid=1 and timestamp<@date and upper(localization)= @localization and alert is not null", conn);
                    cmd.Parameters.AddWithValue("@date", firstDate);
                    cmd.Parameters.AddWithValue("@localization", localization.ToUpper());
                    reader = cmd.ExecuteReader();

                }
                else if (condiction == "BIG")
                {

                    cmd = new SqlCommand("SELECT * FROM DATA d join SENSORS s ON d.id_sensor = s.id WHERE valid=1 and timestamp>@date and upper(localization)= @localization and alert is not null", conn);
                    cmd.Parameters.AddWithValue("@date", firstDate);
                    cmd.Parameters.AddWithValue("@localization", localization.ToUpper());
                    reader = cmd.ExecuteReader();

                }
                else
                {

                    cmd = new SqlCommand("SELECT * FROM DATA d join SENSORS s ON d.id_sensor = s.id WHERE valid=1 and timestamp>@date1 and timestamp<@date2 and upper(localization)= @localization and alert is not null", conn);
                    cmd.Parameters.AddWithValue("@date1", firstDate);
                    cmd.Parameters.AddWithValue("@date2", secondDate);
                    cmd.Parameters.AddWithValue("@localization", localization.ToUpper());
                    reader = cmd.ExecuteReader();

                }

                while (reader.Read())
                {
                    Alert a = new Alert
                    {
                        id = (int)reader["id"],
                        id_sensor_data = (int)reader["id_sensor"],
                        alert_msg = (string)reader["alert"],
                        timestamp = long.Parse(reader["timestamp"].ToString())
                    };
                    list.Add(a);
                }

                reader.Close();
                conn.Close();
                return Ok(list);
            }
            catch (Exception e)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }

                return NotFound();
            }
        }

        //FUNÇÕES AUXILIARES
        private bool checkCondiction(string condiction)
        {
            if (condiction != "SMALL" && condiction != "BIG" && condiction != "BETWEEN")
            {
                return false;
            }
            return true;
        }

        private bool checkDates(string date1, string date2)
        {
            try
            {
                if (date1.Length != 8)
                {
                    return false;
                }
                int date = int.Parse(date1);
                if (date2.Length > 0)
                {
                    if (date2.Length != 8)
                    {
                        return false;
                    }
                    date = int.Parse(date2);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
