using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiEx2.Models;

namespace WebApiEx2.Controllers
{
    public class CustomersController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<CustomerJson> Get()
        {
            return GetCustomers();
        }

        // GET api/<controller>/5
        public CustomerJson Get(string name)
        {
            return GetCustomer(name);
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        // Get Customers from json
        private List<CustomerJson> GetCustomers()
        {
             return JsonConvert.DeserializeObject<List<CustomerJson>>(File.ReadAllText(@"C:\Users\otti\source\repos\WebApiEx2\WebApiEx2\Content\DB\customers.json"));
        }
        private CustomerJson GetCustomer(string name)
        {
            return GetCustomers().Where(x => x.Customer.firstName.Equals(name)).FirstOrDefault();
        }
    }
}