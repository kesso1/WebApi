using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiEx2.Models
{
    public class CustomerJson
    {
        public Customer Customer { get; set; }
    }
    public class Customer
    {
        [JsonProperty("firstname")]
        public string firstName { get; set; }
        [JsonProperty("lastname")]
        public string lastName { get; set; }
        [JsonProperty("address")]
        public string address { get; set; }
    }

}