using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReactTestApi.Models
{
    public class People
    {
        public People(string get_name,string get_identity)
        {
            name = get_name;
            identity = get_identity;
        }

        public string name { get; set; }

        public string identity { get; set; }
    }
}