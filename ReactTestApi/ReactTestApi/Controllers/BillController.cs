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
    public class BillController : ApiController
    {
        [HttpPost]
        
        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "POST")]
        public string Get()
        {
            string sendstring = Request.Content.ReadAsStringAsync().Result.ToString();
            billRecord_Write data=JsonConvert.DeserializeObject<billRecord_Write>(sendstring);/*解讀完傳過來的資料*/

            List<billRecord_Child> list2 = JsonConvert.DeserializeObject<List<billRecord_Child>>(data.itemList);

            //string resultstring = "";
            string conn = "Server=TR\\SQLEXPRESS;Database=tani_storage;uid=ange;pwd=ange0909;Trusted_Connection=True;MultipleActiveResultSets=True";


            SqlConnection mycon = new SqlConnection(conn);

            /*會循環執行*/
            string add_detail = "insert into billList(bill_code,customer_code,booked_date,total) values(@bill_code,@customer_code,@booked_date,@total)";

            SqlCommand cmd2 = new SqlCommand(add_detail, mycon);

            cmd2.Parameters.Add("@bill_code", System.Data.SqlDbType.VarChar).Value = data.billcode;
            cmd2.Parameters.Add("@customer_code", System.Data.SqlDbType.Int).Value = data.customerCode;
            cmd2.Parameters.Add("@booked_date", System.Data.SqlDbType.Date).Value = DateTime.Today;
            cmd2.Parameters.Add("@total", System.Data.SqlDbType.Int).Value = data.total;
            /*添加明細(不包括貨物詳情)*/
            mycon.Open();
            cmd2.ExecuteNonQuery();

           


            /*添加貨物訂購詳情資訊*/
            foreach (var item in list2)
            {
                string insert = "insert into bookList (bill_code,product_id,counts) values (@bill_code,@product_id,@counts)";
                SqlCommand cmd = new SqlCommand(insert, mycon);

                cmd.Parameters.Add("@bill_code", System.Data.SqlDbType.VarChar).Value = data.billcode;
                cmd.Parameters.Add("@product_id", System.Data.SqlDbType.VarChar).Value = item.id;
                //cmd.Parameters.Add("@customer_code", System.Data.SqlDbType.Int).Value = data.customerCode;
                cmd.Parameters.Add("@counts", System.Data.SqlDbType.Int).Value = item.count;
                //cmd.Parameters.Add("@booked_date", System.Data.SqlDbType.DateTime).Value = DateTime.Today;

                cmd.ExecuteNonQuery();
            }
            mycon.Close();

            return "OK";
        }

        [HttpPost]
        [Route("api/Bill/Get")]
        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "POST")]
        public string GetStorage()
        {
            string conn = "Server=TR\\SQLEXPRESS;Database=tani_storage;uid=ange;pwd=ange0909;Trusted_Connection=True;MultipleActiveResultSets=True";
            SqlConnection mycon = new SqlConnection(conn);

            string search= "SELECT product.id,CONCAT(model_category.names,product_category.names) as productNames,product.price,product.count FROM product " +
                            "inner join model_category ON product.model_id = model_category.id " +
                            "inner Join product_category ON product_category.id = product.product_id";

            SqlCommand cmd = new SqlCommand(search,mycon);
            mycon.Open();

            SqlDataReader mydr = cmd.ExecuteReader();

            List<Product2> list1 = new List<Product2>();
            
            if (mydr.HasRows)
            {
                
                while (mydr.Read())
                {
                    int id = mydr.GetInt32(0);
                    string names = mydr.GetString(1); /*完整產品名稱(機種+產品名稱)*/
                    int price = mydr.GetInt32(2);
                    int count = mydr.GetInt32(3);

                    list1.Add(new Product2(id,names, price, count));

                }
                mydr.NextResult();
            }

            mycon.Close();

            string resultString = JsonConvert.SerializeObject(list1);

            return resultString;
        }

        [HttpPost]
        [Route("api/Bill/GetBill")]
        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "POST")]
        public string GetBill()
        {
            string conn = "Server=TR\\SQLEXPRESS;Database=tani_storage;uid=ange;pwd=ange0909;Trusted_Connection=True;MultipleActiveResultSets=True";
            SqlConnection mycon = new SqlConnection(conn);

            string sendstring = Request.Content.ReadAsStringAsync().Result.ToString();
            int customerCode = int.Parse(sendstring);/*獲取客戶編號*/

            /*如果選擇一開始全抓，雖然比較方便，但安全性很低*/
            /*如果選擇選擇能跟使用者的設定而變動，雖然麻煩但安全性會高很多*/

            /*要先抓到哪些帳單是屬於該客戶的，再來就是依次找到該客戶訂購詳細資料*/
            string select = "select * from billList where customer_code=@customer_code";

            SqlCommand cmd = new SqlCommand(select, mycon);

            cmd.Parameters.Add("@customer_code", System.Data.SqlDbType.Int).Value = customerCode;

            mycon.Open();

            SqlDataReader mydr = cmd.ExecuteReader();

            List<ReturnRecord> Rlist = new List<ReturnRecord>();
            List<billDetails> Blist = new List<billDetails>();

            if (mydr.HasRows)
            {
                while (mydr.Read())
                {
                    string get_billcode = mydr.GetString(0);

                    int get_customercode = mydr.GetInt32(1);

                    string getDate = mydr.GetDateTime(2).Date.ToString("yyyy-MM-dd");

                    int getTotal = mydr.GetInt32(3);


                    Rlist.Add(new ReturnRecord(get_billcode, get_customercode, getDate, getTotal));
                }
                mydr.NextResult();
            }

            /*接著找尋詳細資料*/
            foreach(var bill in Rlist)
            {
                string searchDetails = "select model_category.names,product_category.names,product.price,bookList.counts " +
                                       "from model_category, product_category, product, bookList " +
                                        "where(bookList.product_id = product.id and product.product_id = product_category.id and product.model_id = model_category.id and bookList.bill_code = '524297222427')";
                SqlCommand cmd2 = new SqlCommand(searchDetails, mycon);

                //cmd2.Parameters.Add("@billcode", System.Data.SqlDbType.VarChar).Value = bill.bill_code;

                SqlDataReader mydr2 = cmd2.ExecuteReader();

                if (mydr2.HasRows)
                {
                    while (mydr2.Read())
                    {
                        string get_productNames = mydr2.GetString(0) + mydr2.GetString(1);

                        int get_price = mydr2.GetInt32(2);

                        int get_count = mydr2.GetInt32(3);

                        billDetails newDetails = new billDetails(get_productNames, get_price, get_count);

                        bill.addDetails(newDetails);
                    }
                    mydr2.NextResult();
                }
            }

            mycon.Close();

            string resultString = JsonConvert.SerializeObject(Rlist);
            

            return resultString;
        }
    }
}
