using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReactTestApi.Models
{
    public class Account
    {
        public Account(string get_name,string get_password)
        {
            AccountName = get_name;
            Password = get_password;
        }

        public string AccountName { get; set; }

        public string Password { get; set; }

    }
}