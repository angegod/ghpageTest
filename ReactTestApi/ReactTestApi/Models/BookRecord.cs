using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.Json;
using Newtonsoft.Json;

namespace ReactTestApi.Models
{
    public class BookRecord
    {
        public BookRecord(string get_User,string get_Items,int get_total)
        {
            customer = get_User;
            bookItems = get_Items;
            costTotal = get_total;
        }


        
        public  string customer { get; set; }
        
        public string bookItems { get; set; }

        public int costTotal { get; set; }

        

    }

    public class SendBookRecord
    {
        public SendBookRecord(string get_User, string get_Items,string get_Date,int get_total)
        {
            customer = get_User;
            bookItems = get_Items;
            date = get_Date;
            total = get_total;
        }



        public string customer { get; set; }

        public string bookItems { get; set; }

        public string date { get; set; }

        public int total { get; set; }
    }
}