// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using System;
using System.Collections;
using Alachisoft.NCache.Sample.Data;
using System.Configuration;
using Alachisoft.NCache.Client;
using System.Collections.Generic;

namespace Alachisoft.NCache.Samples
{
    /// <summary>
    /// Class that demonstrates the usage of groups in NCache
    /// </summary>
    public class Groups
    {
        private static ICache _cache;

        public static void Run()
        {
            // Initialize the cache
            InitializeCache();

            //Adding item in same group        
            AddItems();

            // Getting group data
            // Will return nine items
            GetItemsByGroup();

            // getGroupKeys is yet another function to retrive group data.
            // It however requires multiple iterations to retrive actual data
            // 1) To get List of Keys and 2) TO get items for the return List of Keys

            // Updating items in groups
            // Item is updated at the specified group
            UpdateItem();

            // Remove group data
            RemoveGroupData();

            // Dispose cache once done
            _cache.Dispose();
        }

        /// <summary>
        /// This method initializes the cache.
        /// </summary>
        private static void InitializeCache()
        {
            string cache = ConfigurationManager.AppSettings["CacheId"];

            if (String.IsNullOrEmpty(cache))
            {
                Console.WriteLine("The Cache Name cannot be null or empty.");
                return;
            }

            // Initialize an instance of the cache to begin performing operations:
            _cache = NCache.Client.CacheManager.GetCache(cache);
            _cache.Clear();
            Console.WriteLine("Cache initialized successfully");
        }

        /// <summary>
        /// This method add items in the cache.
        /// </summary>
        private static void AddItems()
        {
            // Mobile Items

            AddGroupedDatatoCache(1, "PhoneSamsung", "Mobiles");
            AddGroupedDatatoCache(2, "HTCPhone", "Mobiles");
            AddGroupedDatatoCache(3, "NokiaPhone", "Mobiles");

            // Laptop Items

            AddGroupedDatatoCache(4, "AcerLaptop", "Laptops");
            AddGroupedDatatoCache(5, "HPLaptop", "Laptops");
            AddGroupedDatatoCache(6, "DellLaptop", "Laptops");

            // small electronics items

            AddGroupedDatatoCache(7, "HairDryer", "SmallElectronics");
            AddGroupedDatatoCache(8, "VaccumCleaner", "SmallElectronics");
            AddGroupedDatatoCache(9, "Iron", "SmallElectronics");



            Console.WriteLine("Items added in cache.");
        }

        private static void AddGroupedDatatoCache(int productId, string productName, string group, string className = "Elec", string category = "Elec")
        {
            Product product = new Product() { Id = productId, Name = productName, ClassName = className, Category = category };

            // create object key
            string key = "product:" + product.Id.ToString();

            // create CacheItem with your desired object
            CacheItem item = new CacheItem(product);

            // assign group to CacheItem object
            item.Group = group;

            // add CacheItem object to cache
            _cache.Add(key, item);
        }



        /// <summary>
        /// This method fetches items from the cache using groups
        /// </summary>
        private static void GetItemsByGroup()
        {
            IDictionary<string, Product> items = _cache.SearchService.GetGroupData<Product>("Mobiles");
            if (items.Count > 0)
            {
                Console.WriteLine("Item count: " + items.Count);
                Console.WriteLine("Following Products are found in group 'Mobiles'");
                IEnumerator itor = items.Values.GetEnumerator();
                while (itor.MoveNext())
                {
                    Console.WriteLine(((Product)itor.Current).Name);
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// This mehtod updates items in the cache.
        /// </summary>
        private static void UpdateItem()
        {
            Product product = new Product() { Id = 8, Name = "PanaSonicIron" };

            CacheItem panasonicIron = new CacheItem(product);
            panasonicIron.Group = "SmallElectronics";

            string key = "product:" + product.Id.ToString();
            _cache.Insert(key, panasonicIron);
        }

        /// <summary>
        /// This method removes items from the cache that belong to a specific group
        /// </summary>
        private static void RemoveGroupData()
        {
            Console.WriteLine("Item count: " + _cache.Count); // Itemcount = 6
            _cache.SearchService.RemoveGroupData("Laptops"); // Will remove all items from cache based on group Laptops

            Console.WriteLine("Item count: " + _cache.Count); // Itemcount = 0
        }
    }
}