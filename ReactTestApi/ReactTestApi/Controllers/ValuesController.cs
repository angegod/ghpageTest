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
    public class ValuesController : ApiController
    {
        // GET api/values
        /*public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }*/

        // GET api/values/5
        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
        public string Get(string searchid)
        {
            string returnstring = "";
            string conn = "Server=TR\\SQLEXPRESS;Database=TestDb;uid=ange;pwd=ange0909;Trusted_Connection=True;MultipleActiveResultSets=True";

            SqlConnection mycon = new SqlConnection(conn);

            string select = "select * from TestTable where names=@name";

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

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
