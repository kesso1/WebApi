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
            CustomerJson customer = GetCustomer(name);
            if (customer != null) return GetCustomer(name);
            var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(string.Format("No customer with name = {0}", name)),
                ReasonPhrase = "Customer name Not Found"
            };
            throw new HttpResponseException(resp);
        }

        // POST api/<controller>
        public void Post(Transaction transaction)
        {
            CustomerJson sender = GetCustomers().Where(x => x.Customer.firstName.Equals(transaction.sender.firstName) && x.Customer.lastName.Equals(transaction.sender.lastName)).FirstOrDefault();
            CustomerJson recipient = GetCustomers().Where(x => x.Customer.firstName.Equals(transaction.recipient.firstName) && x.Customer.lastName.Equals(transaction.recipient.lastName)).FirstOrDefault();
            if (sender != null && recipient != null)
            {
                sender.Customer.balance -= transaction.amount;
                recipient.Customer.balance += transaction.amount;
            }
            saveCustomer(sender);
            saveCustomer(recipient);
        }

        // PUT api/<controller>/5
        public void Put(CustomerJson customer)
        {
            try
            {
                List<CustomerJson> allCustomers = GetCustomers();
                //CustomerJson newCustomer = JsonConvert.DeserializeObject<CustomerJson>(value);
                allCustomers.Add(customer);
                File.WriteAllText(@"C:\Users\otti\source\repos\WebApiEx2\WebApiEx2\Content\DB\customers.json", JsonConvert.SerializeObject(allCustomers));
            }
            catch
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("Unable to create customer = {0}", customer)),
                    ReasonPhrase = "Probably malformed content"
                };
                throw new HttpResponseException(resp);
            }
        }
        
        // DELETE api/<controller>/5
        public void Delete(string name)
        {
            try
            {
                CustomerJson rmCustomer = GetCustomers().Where(x => x.Customer.firstName.Equals(name)).FirstOrDefault();
                removeCustomer(rmCustomer);
            }
            catch
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("No customer with name = {0}", name)),
                    ReasonPhrase = "Customer name Not Found"
                };
                throw new HttpResponseException(resp);
            }
        }
        // Get Customers from Json
        private List<CustomerJson> GetCustomers()
        {
             return JsonConvert.DeserializeObject<List<CustomerJson>>(File.ReadAllText(@"C:\Users\otti\source\repos\WebApiEx2\WebApiEx2\Content\DB\customers.json"));
        }
        private CustomerJson GetCustomer(string name)
        {
            return GetCustomers().Where(x => x.Customer.firstName.Equals(name)).FirstOrDefault();
        }
        private void removeCustomer(CustomerJson customer)
        {
            List<CustomerJson> allCustomers = GetCustomers();
            allCustomers.Remove(customer);
            File.WriteAllText(@"C:\Users\otti\source\repos\WebApiEx2\WebApiEx2\Content\DB\customers.json", JsonConvert.SerializeObject(allCustomers));
        }
        private void saveCustomer(CustomerJson customer)
        {
            List<CustomerJson> allCustomers = GetCustomers();
            CustomerJson oldCostumer = allCustomers.Where(x => x.Customer.firstName.Equals(customer.Customer.firstName)).FirstOrDefault();
            allCustomers.Remove(oldCostumer);
            allCustomers.Add(customer);
            File.WriteAllText(@"C:\Users\otti\source\repos\WebApiEx2\WebApiEx2\Content\DB\customers.json", JsonConvert.SerializeObject(allCustomers));
        }
    }
}