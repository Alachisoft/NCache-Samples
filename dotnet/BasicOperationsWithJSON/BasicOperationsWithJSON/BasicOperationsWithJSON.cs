// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using Alachisoft.NCache.Runtime;
using Alachisoft.NCache.Sample.Data;
using Alachisoft.NCache.Client;
using System;
using System.Configuration;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Runtime.JSON;
using Newtonsoft.Json;

namespace Alachisoft.NCache.Samples
{
    /// <summary>
    /// Class that provides the functionality of the sample
    /// </summary>
    public class BasicOperationsWithJSON
    {
        private static ICache _cache;

        /// <summary>
        /// Executing this method will perform all the operations of the sample
        /// </summary>
        public static void Run()
        {
            // Initialize cache
            InitializeCache();

            // Create a simple customer object
            Customer customer = CreateNewCustomer();
            JsonObject jsonObject = PopulateJSONObjectFromCustomer(customer);

            string key = GetKey(customer);

            // Adding item synchronously
            AddJsonObjectToCache(key, jsonObject);

            // Get the object from cache
            jsonObject = GetJsonObjectFromCache(key);

            // Modify the object and update in cache
            UpdateJsonObjectInCache(key, jsonObject);

            // Remove the existing object from cache
            RemoveJsonObjectFromCache(key);

            // Dispose the cache once done
            _cache.Dispose();
        }

        /// <summary>
        /// This method initializes the cache
        /// </summary>
        private static void InitializeCache()
        {
            string cache = ConfigurationManager.AppSettings["CacheID"];

            if (String.IsNullOrEmpty(cache))
            {
                Console.WriteLine("The CacheID cannot be null or empty.");
                return;
            }

            // Initialize an instance of the cache to begin performing operations:
            _cache = CacheManager.GetCache(cache);

            // Print output on console
            Console.WriteLine(string.Format("\nCache '{0}' is initialized.", cache));
        }

        /// <summary>
        /// This method adds json object in the cache using synchronous api
        /// </summary>
        /// <param name="key"> String key to be added in cache </param>
        /// <param name="jsonObject"> Instance of JsonObject that will be added to cache </param>
        private static void AddJsonObjectToCache(string key, JsonObject jsonObject)
        {
            TimeSpan expirationInterval = new TimeSpan(0, 1, 0);

            Expiration expiration = new Expiration(ExpirationType.Absolute);
            expiration.ExpireAfter = expirationInterval;

            //Populating cache item
            CacheItem item = new CacheItem(jsonObject);
            item.Expiration = expiration;

            // Adding cacheitem to cache with an absolute expiration of 1 minute
            _cache.Add(key, item);

            // Print output on console
            Console.WriteLine("\nJSON Object is added to cache.");
        }

        /// <summary>
        /// This method gets a json object from the cache using synchronous api
        /// </summary>
        /// <param name="key"> String key to get Json object from cache</param>
        /// <returns> returns instance of JsonObject retrieved from cache</returns>
        private static JsonObject GetJsonObjectFromCache(string key)
        {
            JsonObject cachedJsonObject = _cache.Get<JsonObject>(key);

            // Print output on console
            Console.WriteLine("\nJSON Object is fetched from cache");

            PrintJsonObjectDetails(cachedJsonObject);

            return cachedJsonObject;
        }

        /// <summary>
        /// This method updates json object in the cache using synchronous api
        /// </summary>
        /// <param name="key"> String key to be updated in cache</param>
        /// <param name="jsonObject"> Instance of JsonObject that will be updated in the cache</param>
        private static void UpdateJsonObjectInCache(string key, JsonObject jsonObject)
        {
            // Update item with a sliding expiration of 30 seconds
            jsonObject["CompanyName"] = (JsonValue)"Gourmet Lanchonetes" ;

            TimeSpan expirationInterval = new TimeSpan(0, 0, 30);

            Expiration expiration = new Expiration(ExpirationType.Sliding);
            expiration.ExpireAfter = expirationInterval;

            CacheItem item = new CacheItem(jsonObject);
            item.Expiration = expiration;

            _cache.Insert(key, jsonObject);

            // Print output on console
            Console.WriteLine("\nJSON Object is updated in cache.");
        }

        /// <summary>
        /// Remove a json object in the cache using synchronous api
        /// </summary>
        /// <param name="key"> String key to be deleted from cache</param>
        private static void RemoveJsonObjectFromCache(string key)
        {
            // Remove the existing json object
            _cache.Remove(key);

            // Print output on console
            Console.WriteLine("\nJSON Object is removed from cache.");
        }

        /// <summary>
        /// Generates instance of Customer to be used in this sample
        /// </summary>
        /// <returns> returns instance of Customer </returns>
        private static Customer CreateNewCustomer()
        {
            return new Customer
            {
                CustomerID = "DAVJO",
                ContactName = "David Johnes",
                CompanyName = "Lonesome Pine Restaurant",
                ContactNo = "12345-6789",
                Address = "Silicon Valley, Santa Clara, California",
            };
        }

        /// <summary>
        /// Generates a string key for specified customer
        /// </summary>
        /// <param name="customer"> Instance of Customer to generate a key</param>
        /// <returns> returns a key </returns>
        private static string GetKey(Customer customer)
        {
            return string.Format("Customer:{0}", customer.CustomerID);
        }

        /// <summary>
        /// This method prints detials of customer type.
        /// </summary>
        /// <param name="jsonObject"></param>
        private static void PrintJsonObjectDetails(JsonObject jsonObject)
        {
            if (jsonObject == null) return;

            var ContactName = jsonObject["ContactName"] as JsonValue;
            var CompanyName = jsonObject["CompanyName"] as JsonValue;
            var ContactNo = jsonObject["ContactNo"] as JsonValue;
            var Address = jsonObject["Address"] as JsonValue;


            Console.WriteLine();
            Console.WriteLine("Customer Details are as follows: ");
            Console.WriteLine("ContactName: " + ContactName.ToStringValue());
            Console.WriteLine("CompanyName: " + CompanyName.ToStringValue());
            Console.WriteLine("Contact No: " + ContactNo.ToStringValue());
            Console.WriteLine("Address: " + Address.ToStringValue());
            Console.WriteLine();
        }

        private static JsonObject PopulateJSONObjectFromCustomer(Customer customer)
        {
            // Method 1
            string jsonString = JsonConvert.SerializeObject(customer);

            return new JsonObject(jsonString);

            // Method 2

            // var jsonObject = new JsonObject();

            // jsonObject["CustomerID"] = (JsonValue)customer.CustomerID;
            // jsonObject["ContactName"] = (JsonValue)customer.ContactName;
            // jsonObject["CompanyName"] = (JsonValue)customer.CompanyName;
            // jsonObject["ContactNo"] = (JsonValue)customer.ContactNo;
            // jsonObject["Address"] = (JsonValue)customer.Address;
            // jsonObject["City"] = (JsonValue)customer.City;
            // jsonObject["Country"] = (JsonValue)customer.Country;
            // jsonObject["PostalCode"] = (JsonValue)customer.PostalCode;
            // jsonObject["Fax"] = (JsonValue)customer.Fax;

            //  jsonObject;
        }
    }
}
