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
        public BookRecord(string get_User, string get_Items, int get_total)
        {
            customer = get_User;
            bookItems = get_Items;
            costTotal = get_total;
        }



        public string customer { get; set; }

        public string bookItems { get; set; }

        public int costTotal { get; set; }



    }

    public class SendBookRecord
    {
        public SendBookRecord(string get_User, string get_Items, string get_Date, int get_total)
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

    public class billRecord_Write
    {
        public billRecord_Write(string get_bcode,int get_code,string get_list)
        {
            billcode = get_bcode;
            customerCode = get_code;
            itemList = get_list;
        }

        public string billcode { get; set; }
        public int customerCode { get; set; }

        public string itemList { get; set; }

        public int total { get; set; }

    }

    public class billRecord_Child//二次解讀需要用到這個
    {
        public billRecord_Child(int get_id,int get_price,int get_count)
        {
            id = get_id;
            price = get_price;
            count = get_count;
        }

        public int id { get; set; }

        public int price { get; set; }

        public int count { get; set; }
    }

    public class billRecord_Read{

        public billRecord_Read(int get_CCode,string get_Pname,int get_count,DateTime get_date)
        {
            customer_code = get_CCode;
            product_name = get_Pname;
            count = get_count;
            bookdate = get_date;
        }

        public int customer_code { get; set; }

        public string product_name { get; set; }

        public int count { get; set; }

        public DateTime bookdate { get; set; }


    }

}