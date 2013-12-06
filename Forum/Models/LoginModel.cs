using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Models
{
    public class LoginModel
    {
        public LoginModel()
        {
            User = new Database.User();
        }

        public Database.User User { get; set; }
        public string Password { get; set; }
        public bool KeepConnected { get; set; }
        public string Error { get; set; }
    }
}