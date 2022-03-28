using GlobalAPI.Models;
using GlobalAPI.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GlobalAPI.Controllers.Authentication
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        static string connectionStr = Properties.Settings.Default.connectionString;

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization != null)
            {
                var authToken = actionContext.Request.Headers.Authorization.Parameter;

                //Format to 'Username:Password'
                var decodeauthToken = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authToken));

                //Split 'Username' from 'Password'
                var arrUserNameandPassword = decodeauthToken.Split(':');

                //0th postion of array we get username and at 1st we get password  
                if (IsAuthorizedUser(arrUserNameandPassword[0], arrUserNameandPassword[1]))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(arrUserNameandPassword[0]), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }

        private bool IsAuthorizedUser(string username, string password)
        {  
            List<User> users = null;
            users = GetUsers();

            if (users.Count < 1)
            {
                return false;
            }
            else
            {
                foreach (User u in users)
                {
                    if (u.username == username && u.passwordHash == HashPassword.toHash(password))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private List<User> GetUsers()
        {
            List<User> list = new List<User>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionStr);
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM USERS", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    User u = new User
                    {
                        username = (string)reader["username"],
                        name = (string)reader["name"],
                        email = (string)reader["email"],
                        passwordHash = (string)reader["passwordHash"]
                    };
                    list.Add(u);
                }

                reader.Close();
                conn.Close();
                return list;
            }
            catch (Exception e)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }

                return new List<User>();
            }
        }
    }
}