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
    public class ProductController : ApiController
    {
        [HttpPost]
        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "POST")]
        public string Get()
        {
            string resultstring = "";

            string conn = "Server=TR\\SQLEXPRESS;Database=Product;uid=ange;pwd=ange0909;Trusted_Connection=True;MultipleActiveResultSets=True";

            SqlConnection mycon = new SqlConnection(conn);
            List<Product> list1 = new List<Product>();

            string select = "select * from products";

            SqlCommand cmd = new SqlCommand(select, mycon);
            //cmd.Parameters.Add("@name", System.Data.SqlDbType.VarChar).Value = searchName;
            mycon.Open();

            SqlDataReader mydr = cmd.ExecuteReader();
            while (mydr.HasRows)
            {
                while (mydr.Read())
                {
                    int id = mydr.GetInt32(0);
                    string name = mydr.GetString(1);
                    int price  = mydr.GetInt32(2);
                    string link = mydr.GetString(3);

                    Product p1 = new Product(id, name, price,0,link);

                    list1.Add(p1);

                    
                }
                mydr.NextResult();
            }
            resultstring = JsonConvert.SerializeObject(list1);
            return resultstring;

        }
        //[EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]

    }

}
