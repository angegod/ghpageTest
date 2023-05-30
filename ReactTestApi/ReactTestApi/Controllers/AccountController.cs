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
    public class AccountController : ApiController
    {
        [HttpPost]
        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "POST")]
        public string Get()
        {
            string sendstring = Request.Content.ReadAsStringAsync().Result.ToString();

            Account a1 = JsonConvert.DeserializeObject<Account>(sendstring);

            string resultstring = "";

            string conn = "Server=TR\\SQLEXPRESS;Database=Product;uid=ange;pwd=ange0909;Trusted_Connection=True;MultipleActiveResultSets=True";

            SqlConnection mycon = new SqlConnection(conn);

            string checkLogin = "select * from Account where AccName=@AccName and Pwd=@Pwd";
            SqlCommand cmd = new SqlCommand(checkLogin, mycon);

            cmd.Parameters.Add("@AccName", System.Data.SqlDbType.VarChar).Value = a1.AccountName;
            cmd.Parameters.Add("@Pwd", System.Data.SqlDbType.VarChar).Value = a1.Password;

            mycon.Open();

            SqlDataReader mydr = cmd.ExecuteReader();


            /*登入帳號判別程序*/
            if (mydr.HasRows)
                resultstring = "OK";
            else
                resultstring = "Failed";
            /*記得將連線給關掉*/
            mycon.Close();

            /*回傳判讀結果*/

            return resultstring;

        }
    }
}
