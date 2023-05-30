using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReactTestApi.Models
{
    public class Product
    {
        public Product(int get_id,string get_name,int get_price,int get_count,string get_imgLink)
        {
            id = get_id;
            name = get_name;
            price = get_price;
            count = get_count;
            imgLink = get_imgLink;
         }

        public int id { get; set; }

        public string name { get; set; }

        public int count { get; set; }

        public int price { get; set; }

        public string imgLink { get; set; }

    }
}