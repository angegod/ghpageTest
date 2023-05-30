using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using System.Data.SqlClient;
using ReactTestApi.Models;

namespace ReactTestApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
        public string Get(string searchid)
        {
            string returnstring = "";
            string conn = "Server=TR\\SQLEXPRESS;Database=Imgtext;uid=ange;pwd=ange0909;Trusted_Connection=True;MultipleActiveResultSets=True";

            SqlConnection mycon = new SqlConnection(conn);

            string select = "select * from storage where itemName=@itemName";

            SqlCommand cmd = new SqlCommand(select, mycon);
            cmd.Parameters.Add("@name", System.Data.SqlDbType.VarChar).Value = searchid;
            mycon.Open();

            SqlDataReader mydr = cmd.ExecuteReader();
            while (mydr.HasRows)
            {
                while (mydr.Read())
                {
                    string name = mydr.GetString(1);
                    string identity = mydr.GetString(2);

                    People p1 = new People(name, identity);

                    returnstring = JsonConvert.SerializeObject(p1);
                }
                mydr.NextResult();
            }

            return returnstring;
        }
    }
}
