using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data.SqlClient;
using ReactTestApi.Models;
using Newtonsoft.Json;

namespace ReactTestApi.Controllers
{
    public class SaveRecordController : ApiController
    {
        [HttpPost]
        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "POST")]
        public string Get()
        {

            string sendstring = Request.Content.ReadAsStringAsync().Result.ToString();

            BookRecord record = JsonConvert.DeserializeObject<BookRecord>(sendstring);

            string conn = "Server=TR\\SQLEXPRESS;Database=Product;uid=ange;pwd=ange0909;Trusted_Connection=True;MultipleActiveResultSets=True";

            SqlConnection mycon = new SqlConnection(conn);
            /*訂購記錄欄位:買家，訂購資訊，日期*/
            string sendRecord = "insert into Record (customer,bookItems,bookDate,bookTotal) values (@customer,@bookItems,@bookDate,@bookTotal)";

            SqlCommand cmd = new SqlCommand(sendRecord, mycon);
            cmd.Parameters.Add("@customer", System.Data.SqlDbType.VarChar).Value = record.customer;
            cmd.Parameters.Add("@bookItems", System.Data.SqlDbType.VarChar).Value = record.bookItems;
            cmd.Parameters.Add("@bookDate", System.Data.SqlDbType.Date).Value = DateTime.Now;
            cmd.Parameters.Add("@bookTotal", System.Data.SqlDbType.Int).Value = record.costTotal;

            mycon.Open();
            cmd.ExecuteNonQuery();

            return "OK";
        }


        [HttpPost]
        [Route("api/SaveRecord/Send")]
        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "POST")]
        public string Send()
        {
            /*分辨調閱出來的使用者ID為何*/
            string sendstring = Request.Content.ReadAsStringAsync().Result.ToString();

            /*連線字串*/
            string conn = "Server=TR\\SQLEXPRESS;Database=Product;uid=ange;pwd=ange0909;Trusted_Connection=True;MultipleActiveResultSets=True";

            SqlConnection mycon = new SqlConnection(conn);

            string select = "select * from Record where customer=@customer";

            SqlCommand cmd = new SqlCommand(select, mycon);

            cmd.Parameters.Add("@customer", System.Data.SqlDbType.VarChar).Value = sendstring;
            mycon.Open();
            SqlDataReader mydr = cmd.ExecuteReader();

            List<SendBookRecord> list1 = new List<SendBookRecord>();

            while (mydr.HasRows)
            {
                while (mydr.Read())
                {
                    
                    string customer = mydr.GetString(1);
                    string bookItemsList = mydr.GetString(2);
                    string date = mydr.GetString(3);
                    int total = mydr.GetInt32(4);
                    DateTime Now = DateTime.Now;

                    /*計算該紀錄距離現在差了幾天 如果小於三十天則存入list*/
                    if ((Now-DateTime.Parse(date)).Days<=30)
                        list1.Add(new SendBookRecord(customer, bookItemsList,DateTime.Parse(date).ToShortDateString(),total));

                }
                mydr.NextResult();
            }

            string resultstring = JsonConvert.SerializeObject(list1);

            return resultstring;
        }
    }
}
