using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalAPI.Models
{
    public class User
    {
        public string username { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string passwordHash { get; set; }
    }
}