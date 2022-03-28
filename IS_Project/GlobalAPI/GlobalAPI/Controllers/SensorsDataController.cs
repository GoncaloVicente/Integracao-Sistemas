using GlobalAPI.Controllers.Authentication;
using GlobalAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Http;

namespace GlobalAPI.Controllers
{
    /*
    -> Adicionar novos dados do sensor
    -> Invalidar dados do sensor
    -> Obter dados do sensor por localicação com ou sem intervalos de tempo
    */
    public class SensorsDataController : ApiController
    {
        static string connectionStr = Properties.Settings.Default.connectionString;

        // GET: api/sensors/data
        // Obter lista de todos os dados de todos os sensores
        [BasicAuthentication]
        [Route("api/sensors/data")]
        public IHttpActionResult GetAllSensorsData()
        {
            List<SensorData> list = new List<SensorData>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionStr);
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM DATA WHERE valid=1", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    SensorData d = new SensorData
                    {
                        id = (int)reader["id"],
                        id_sensor = (int)reader["id_sensor"],
                        temperature = float.Parse(reader["temperature"].ToString()),
                        humidity = float.Parse(reader["humidity"].ToString()),
                        battery = (int)reader["battery"],
                        timestamp = long.Parse(reader["timestamp"].ToString())
                    };
                    list.Add(d);
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

        // GET: api/sensors/1/data
        // Obter lista de todos os dados de um sensor
        [BasicAuthentication]
        [Route("api/sensors/{id:int}/data")]
        public IHttpActionResult GetSensorDataBySensorId(int id)
        {
            List<SensorData> list = new List<SensorData>();
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
                cmd = new SqlCommand("SELECT * FROM DATA WHERE valid=1 and id_sensor=@sensor", conn);
                cmd.Parameters.AddWithValue("@sensor", id);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    SensorData d = new SensorData
                    {
                        id = (int)reader["id"],
                        id_sensor = (int)reader["id_sensor"],
                        temperature = float.Parse(reader["temperature"].ToString()),
                        humidity = float.Parse(reader["humidity"].ToString()),
                        battery = (int)reader["battery"],
                        timestamp = long.Parse(reader["timestamp"].ToString())
                    };
                    list.Add(d);
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

        // GET: api/sensors/data/1
        // Obter o dados por id
        [BasicAuthentication]
        [Route("api/sensors/data/{id:int}")]
        public IHttpActionResult GetSensorDataById(int id)
        {
            List<SensorData> list = new List<SensorData>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionStr);
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM DATA WHERE valid=1 and id=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    SensorData d = new SensorData
                    {
                        id = (int)reader["id"],
                        id_sensor = (int)reader["id_sensor"],
                        temperature = float.Parse(reader["temperature"].ToString()),
                        humidity = float.Parse(reader["humidity"].ToString()),
                        battery = (int)reader["battery"],
                        timestamp = long.Parse(reader["timestamp"].ToString())
                    };
                    list.Add(d);
                }
                else
                {
                    reader.Close();
                    conn.Close();
                    return NotFound();
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

        // GET: api/sensors/bar/data
        // Obter lista de todos os dados de todos os sensores de uma dada localização
        [BasicAuthentication]
        [Route("api/sensors/{localization}/data")]
        public IHttpActionResult GetSensorDataByLocalization(string localization)
        {
            List<SensorData> list = new List<SensorData>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionStr);
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM DATA d join SENSORS s ON d.id_sensor=s.id WHERE valid=1 and upper(localization)=@localization", conn);
                cmd.Parameters.AddWithValue("@localization", localization.ToUpper());
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    SensorData d = new SensorData
                    {
                        id = (int)reader["id"],
                        id_sensor = (int)reader["id_sensor"],
                        temperature = float.Parse(reader["temperature"].ToString()),
                        humidity = float.Parse(reader["humidity"].ToString()),
                        battery = (int)reader["battery"],
                        timestamp = long.Parse(reader["timestamp"].ToString())
                    };
                    list.Add(d);
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

        // GET: api/sensors/data/small/10122019
        // Obter lista de todos os dados dentro de um intervalo de tempo
        // Timestamp: Long
        [BasicAuthentication]
        [Route("api/sensors/data/{condiction}/{date1}/{date2?}")]
        public IHttpActionResult GetSensorDataWithTemporalInterval(string condiction = "", string date1 = "", string date2 = "")
        {
            condiction = condiction.ToUpper();
            if (!checkCondiction(condiction))
            {
                return BadRequest("Invalid condiction");
            }
            if (!checkDates(date1,date2))
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

            DateTime firstDateTime = new DateTime(int.Parse(date1.Substring(4,4)), int.Parse(date1.Substring(2,2)), int.Parse(date1.Substring(0, 2)));
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

            List<SensorData> list = new List<SensorData>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionStr);
                conn.Open();

                SqlCommand cmd;
                SqlDataReader reader;

                if (condiction == "SMALL")
                {

                    cmd = new SqlCommand("SELECT * FROM DATA WHERE valid=1 and timestamp<@date", conn);
                    cmd.Parameters.AddWithValue("@date", firstDate);
                    reader = cmd.ExecuteReader();

                }else if (condiction == "BIG")
                {

                    cmd = new SqlCommand("SELECT * FROM DATA WHERE valid=1 and timestamp>@date", conn);
                    cmd.Parameters.AddWithValue("@date", firstDate);
                    reader = cmd.ExecuteReader();

                }
                else
                {

                    cmd = new SqlCommand("SELECT * FROM DATA WHERE valid=1 and timestamp>@date1 and timestamp<@date2", conn);
                    cmd.Parameters.AddWithValue("@date1", firstDate);
                    cmd.Parameters.AddWithValue("@date2", secondDate);
                    reader = cmd.ExecuteReader();

                }

                while (reader.Read())
                {
                    SensorData s = new SensorData
                    {
                        id = (int)reader["id"],
                        id_sensor = (int)reader["id_sensor"],
                        temperature = float.Parse(reader["temperature"].ToString()),
                        humidity = float.Parse(reader["humidity"].ToString()),
                        battery = (int)reader["battery"],
                        timestamp = long.Parse(reader["timestamp"].ToString())
                    };
                    list.Add(s);
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

        // GET: api/sensors/bar/data/small/10122019
        // Obter lista de todos os dados dentro de um intervalo de tempo e localização
        // Timestamp: Long
        [BasicAuthentication]
        [Route("api/sensors/{localization}/data/{condiction}/{date1}/{date2?}")]
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

            List<SensorData> list = new List<SensorData>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionStr);
                conn.Open();

                SqlCommand cmd;
                SqlDataReader reader;

                if (condiction == "SMALL")
                {

                    cmd = new SqlCommand("SELECT * FROM DATA d join SENSORS s ON d.id_sensor = s.id WHERE valid=1 and timestamp<@date and upper(localization)= @localization", conn);
                    cmd.Parameters.AddWithValue("@date", firstDate);
                    cmd.Parameters.AddWithValue("@localization", localization.ToUpper());
                    reader = cmd.ExecuteReader();

                }
                else if (condiction == "BIG")
                {

                    cmd = new SqlCommand("SELECT * FROM DATA d join SENSORS s ON d.id_sensor = s.id WHERE valid=1 and timestamp>@date and upper(localization)= @localization", conn);
                    cmd.Parameters.AddWithValue("@date", firstDate);
                    cmd.Parameters.AddWithValue("@localization", localization.ToUpper());
                    reader = cmd.ExecuteReader();

                }
                else
                {

                    cmd = new SqlCommand("SELECT * FROM DATA d join SENSORS s ON d.id_sensor = s.id WHERE valid=1 and timestamp>@date1 and timestamp<@date2 and upper(localization)= @localization", conn);
                    cmd.Parameters.AddWithValue("@date1", firstDate);
                    cmd.Parameters.AddWithValue("@date2", secondDate);
                    cmd.Parameters.AddWithValue("@localization", localization.ToUpper());
                    reader = cmd.ExecuteReader();

                }

                while (reader.Read())
                {
                    SensorData s = new SensorData
                    {
                        id = (int)reader["id"],
                        id_sensor = (int)reader["id_sensor"],
                        temperature = float.Parse(reader["temperature"].ToString()),
                        humidity = float.Parse(reader["humidity"].ToString()),
                        battery = (int)reader["battery"],
                        timestamp = long.Parse(reader["timestamp"].ToString())
                    };
                    list.Add(s);
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

        // POST: api/sensors/5000/data
        // Registar dados de um sensor pessoal
        // Timestamp: Long
        [BasicAuthentication]
        [Route("api/sensors/{id:int}/data")]
        public IHttpActionResult PostSensorData(int id, [FromBody]SensorData data)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionStr))
                {
                    conn.Open();

                    string username = Thread.CurrentPrincipal.Identity.Name;

                    //Verificar Id (se é um sensor pessoal)
                    SqlCommand cmd = new SqlCommand("SELECT * FROM SENSORS WHERE username is not null and username=@username and id=@id", conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.Read())
                    {
                        return NotFound();
                    }

                    cmd = new SqlCommand("INSERT INTO DATA (id_sensor,temperature,humidity,battery,timestamp,valid) VALUES (@sensor,@temperature,@humidity,@battery,@timestamp,@valid)", conn);
                    cmd.Parameters.AddWithValue("@sensor", id);
                    cmd.Parameters.AddWithValue("@temperature", data.temperature);
                    cmd.Parameters.AddWithValue("@humidity", data.humidity);
                    cmd.Parameters.AddWithValue("@battery", data.battery);
                    cmd.Parameters.AddWithValue("@timestamp", data.timestamp);
                    cmd.Parameters.AddWithValue("@valid", 1);

                    reader.Close();
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        return Ok();
                    }
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        // DELETE: api/sensors/data/5000
        // Invalidar dados de um sensor pessoal
        // Timestamp: Long
        [BasicAuthentication]
        [Route("api/sensors/data/{id:int}")]
        public IHttpActionResult DeleteSensorData(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionStr))
                {
                    conn.Open();

                    string username = Thread.CurrentPrincipal.Identity.Name;

                    //Verificar se os dados pertencem a um sensor pessoal
                    SqlCommand cmd = new SqlCommand("SELECT * FROM DATA d join SENSORS s on (d.id_sensor=s.id) WHERE d.id=@id and valid=1 and username is not null and username=@username", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@username", username);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.Read())
                    {
                        return NotFound();
                    }

                    cmd = new SqlCommand("UPDATE DATA SET valid=0 WHERE id=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    reader.Close();
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        return Ok();
                    }
                    return NotFound();
                }
            }
            catch (Exception e)
            {
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
