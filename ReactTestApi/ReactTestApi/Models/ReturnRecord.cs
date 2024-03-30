using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReactTestApi.Models
{
    public class ReturnRecord/*負責回傳帳單部分*/
    {
        public ReturnRecord(string get_Bcode,int get_Ccode,string get_Date,int get_total)
        {
            bill_code = get_Bcode;
            customer_code = get_Ccode;
            booked_date = get_Date;
            bill_total = get_total;
            list = new List<billDetails>();
        }

        public void addDetails(billDetails newDetails)
        {
            list.Add(newDetails);
        }


        public string bill_code { get; set; }

        public int customer_code { get; set; }

        public string booked_date { get; set; }

        public int bill_total { get; set; }

        public List<billDetails> list { get; set; }
    }

    public class billDetails 
    {
        public billDetails(string getName,int getPrice,int getCount)
        {
            productNames = getName;
            price = getPrice;
            count = getCount;
        }

        public string productNames { get; set; }

        public int price { get; set; }

        public int count { get; set; }
    }
}