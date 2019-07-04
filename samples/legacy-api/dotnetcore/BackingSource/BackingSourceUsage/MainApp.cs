using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Alachisoft.NCache.Sample.Data;
using Alachisoft.NCache.Web.Caching;

namespace Alachisoft.NCache.Samples
{
    class MainApp
    {
        private Cache _cache;
        private string _customerID;
        private string _contactName;
        private string _companyName;
        private string _address;
        private string _city;
        private string _country;
        private string _postalCode;
        private string _phone;
        private string _fax;

        public string CustomerID
        {
            get;
            set;
        }
        public string ContactName
        {
            get;
            set;
        }
        public string CompanyName
        {
            get;
            set;
        }
        public string Addresss
        {
            get;
            set;
        }
        public string City
        {
            get;
            set;
        }
        public string Country
        {
            get;
            set;
        }
        public string PostalCode
        {
            get;
            set;
        }
        public string Phone
        {
            get;
            set;
        }
        public string Fax
        {
            get;
            set;
        }
        public void Run()
        {
            InitializeCache();
            Console.WriteLine("Select the Backing Source Provider (number) you want to use");
            int userChoice = GetUserChoice();
            if (userChoice != 0 || userChoice != 3)
            {
                Console.WriteLine("Enter customerId");
                CustomerID = Convert.ToString(Console.ReadLine());
                Customer customer = new Customer();
                if (userChoice == 1)
                {
                    customer=_cache.Get(CustomerID, "SqlReadThruProvider", DSReadOption.ReadThru) as Customer;
                }
                else
                {
                    customer=(Customer)_cache.Get(CustomerID.ToUpper(), "XMLReadThruProvider", DSReadOption.ReadThru);
                }
                if (customer == null)
                {
                    Console.WriteLine("Customer information does not exist");
                    return;
                }
                GetDataFromUser();
                int userUpdationChoice = GetUserUpdationChoice();
                UpdateData(userUpdationChoice);
            }

        }

        public int GetUserUpdationChoice()
        {
            Console.WriteLine("");
            Console.WriteLine(" 1- SQL-WriteThroughProvider");
            Console.WriteLine(" 2- XML-WriteThroughProvider");
            Console.WriteLine(" 3- Exit");
            Console.WriteLine("");
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice >= 1 && choice <= 3)
                    return choice;
                else
                    return 0;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public int GetUserChoice()
        {
            Console.WriteLine("");
            Console.WriteLine(" 1- SQL-ReadThroughProvider");
            Console.WriteLine(" 2- XML-ReadThroughProvider");
            Console.WriteLine(" 3- Exit");
            Console.WriteLine("");
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice >= 1 && choice <= 3)
                    return choice;
                else
                    return 0;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        
        public void GetDataFromUser()
        {
            Console.WriteLine("Enter the following information");
            Console.WriteLine("Contact Name ");
            ContactName = Console.ReadLine();
            Console.WriteLine("Company Name");
            CompanyName = Console.ReadLine();
            Console.WriteLine("Address");
            Addresss = Console.ReadLine();
            Console.WriteLine("City");
            City = Console.ReadLine();
            Console.WriteLine("Country");
            Country = Console.ReadLine();
            Console.WriteLine("Postal Code");
            PostalCode = Console.ReadLine();
            Console.WriteLine("Phone Number");
            Phone = Console.ReadLine();
            Console.WriteLine("Fax Number");
            Fax = Console.ReadLine();
        }
        public void UpdateData(int userchoice)
        {
            Customer customer = new Customer();
            customer.CustomerID = CustomerID;
            customer.ContactName = ContactName;
            customer.CompanyName = CompanyName;
            customer.Address = Addresss;
            customer.City = City;
            customer.Country = Country;
            customer.PostalCode = PostalCode;
            customer.ContactNo = Phone;
            customer.Fax = Fax;


            try
            {
                if (userchoice == 1)
                    _cache.Insert(customer.CustomerID.ToUpper(), new CacheItem(customer), DSWriteOption.WriteThru, "SqlWriteThruProvider", null);
                else
                    _cache.Insert(customer.CustomerID.ToUpper(), new CacheItem(customer), DSWriteOption.WriteThru, "XmlWriteThruProvider", null);

                Console.WriteLine("Customer information updated successfuly");

            }
            catch (Exception ex)
            {

            }
          

        }

        private void InitializeCache()
        {
            string cacheId = System.Configuration.ConfigurationManager.AppSettings["CacheID"];

            if (String.IsNullOrEmpty(cacheId))
            {
                Console.WriteLine("The CacheID cannot be null or empty.");
                return;
            }

            // Initialize an instance of the cache to begin performing operations:
            _cache = NCache.Web.Caching.NCache.InitializeCache(cacheId);

            // Print output on console
            Console.WriteLine(string.Format("\nCache '{0}' is initialized.", cacheId));
        }

    }
}
