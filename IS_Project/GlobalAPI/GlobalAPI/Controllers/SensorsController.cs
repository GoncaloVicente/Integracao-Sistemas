using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GlobalAPI.Models;
using System.Data.SqlClient;
using GlobalAPI.Controllers.Authentication;
using System.Threading;

namespace GlobalAPI.Controllers
{
    /*
    -> Registar sensor pessoal
    -> Obter lista de todos os sensores
    -> Só é possível obter informação se for um utilizador qualificado (Autenticação)
    */
    public class SensorsController : ApiController
    {
        static string connectionStr = Properties.Settings.Default.connectionString;

        // GET: api/sensors
        // Obter lista de todos os sensores
        [BasicAuthentication]
        [Route("api/sensors")]
        public IHttpActionResult GetAllSensors()
        {
            List<Sensor> list = new List<Sensor>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionStr);
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM SENSORS", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Sensor s = new Sensor
                    {
                        id = (int)reader["id"],
                        name = (string)reader["name"],
                        localization = (string)(reader["localization"] == DBNull.Value ?  "" : reader["localization"]),
                        username = (string)(reader["username"] == DBNull.Value ? "" : reader["username"])
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

        // GET: api/sensors/1
        // Obter um sensor por id
        [BasicAuthentication]
        [Route("api/sensors/{id:int}")]
        public IHttpActionResult GetSensorById(int id)
        {
            Sensor sensor = null;
            SqlConnection conn = null;
            bool ok = true;

            try
            {
                conn = new SqlConnection(connectionStr);
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM SENSORS WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    sensor = new Sensor
                    {
                        id = (int)reader["id"],
                        name = (string)reader["name"],
                        localization = (string)(reader["localization"] == DBNull.Value ? "" : reader["localization"]),
                        username = (string)(reader["username"] == DBNull.Value ? "" : reader["username"])
                    };
                }
                else
                {
                    ok = false;
                }

                reader.Close();
                conn.Close();

                if (ok == false)
                {
                    return NotFound();
                }
                return Ok(sensor);
            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }

                return NotFound();
            }
        }

        // POST: api/sensors
        // Registar sensor que pertence à rede de sensores
        [BasicAuthentication]
        [Route("api/sensors")]
        public IHttpActionResult PostSensors([FromBody]Sensor sensor)
        {
            try
            {
                if (sensor.id < 1 || sensor.id > 5000)
                {
                    return BadRequest("ID is out of the defined range for the sensors network");
                }

                using (SqlConnection conn = new SqlConnection(connectionStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO SENSORS (id,name,localization) VALUES (@id,@name,@localization)", conn);
                    cmd.Parameters.AddWithValue("@id", sensor.id);
                    cmd.Parameters.AddWithValue("@name", sensor.name);
                    cmd.Parameters.AddWithValue("@localization", sensor.localization);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        return Ok();
                    }
                    return NotFound();
                }
            }
            catch (SqlException sql) {
                return Conflict();
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        // POST: api/sensors/personal
        // Registar sensor pessoal
        [BasicAuthentication]
        [Route("api/sensors/personal")]
        public IHttpActionResult PostPersonalSensors([FromBody]Sensor sensor)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionStr))
                {
                    conn.Open();
                    int id = 0;

                    //Verificar Id
                    SqlCommand cmd = new SqlCommand("SELECT id FROM SENSORS WHERE username is not null", conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.Read())
                    {
                        id = 5000;
                    }
                    else
                    {
                        reader.Close();
                        cmd = new SqlCommand("SELECT max(id) as id FROM SENSORS WHERE username is not null", conn);
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            id = (int)reader["id"];
                            id++;
                        }
                    }

                    cmd = new SqlCommand("INSERT INTO SENSORS (id,name,username) VALUES (@id,@name,@username)", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", sensor.name);
                    cmd.Parameters.AddWithValue("@username", sensor.username);

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

        // GET: api/sensors/personal
        // Ver apenas os meus sensores pessoais
        [BasicAuthentication]
        [Route("api/sensors/personal")]
        public IHttpActionResult GetAllPersonalSensors()
        {
            List<Sensor> list = new List<Sensor>();
            SqlConnection conn = null;

            try
            {
                string username = Thread.CurrentPrincipal.Identity.Name;

                conn = new SqlConnection(connectionStr);
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM SENSORS WHERE username=@username", conn);
                cmd.Parameters.AddWithValue("@username", username);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Sensor s = new Sensor
                    {
                        id = (int)reader["id"],
                        name = (string)reader["name"],
                        localization = (string)(reader["localization"] == DBNull.Value ? "" : reader["localization"]),
                        username = (string)(reader["username"] == DBNull.Value ? "" : reader["username"])
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
    }
}
