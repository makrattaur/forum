using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Database;

namespace Forum.Models
{
    public class RegisterModel
    {
        public RegisterModel()
        {
            User = new User();
        }

        public User User { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public string Error { get; set; }
    }
}