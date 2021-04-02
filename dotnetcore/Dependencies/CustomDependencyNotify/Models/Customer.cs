using Newtonsoft.Json;
using System;

namespace Models
{
    [Serializable]
    public class Customer
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "CompanyName")]
        public string CompanyName { get; set; }

        [JsonProperty(PropertyName = "ContactName")]
        public string ContactName { get; set; }


        [JsonProperty(PropertyName = "Address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "City")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "Country")]
        public string Country { get; set; }

        public Customer()
        { }

        public Customer(string cid, string name, string company, string address, string city, string country)
        {
            this.Id = cid;
            this.ContactName = name;
            this.CompanyName = company;
            this.Address = address;
            this.City = city;
            this.Country = country;
        }

        public override string ToString()
        {
            return $"CustomerID:\t\t{Id}" +
                    $"\nContactName:\t\t{ContactName}" +
                    $"\nCompany:\t\t{CompanyName}" +
                    $"\nAddress:\t\t{Address}" +
                    $"\nCity:\t\t\t{City}" +
                    $"\nCountry:\t\t{Country}";
        }

    }
}
