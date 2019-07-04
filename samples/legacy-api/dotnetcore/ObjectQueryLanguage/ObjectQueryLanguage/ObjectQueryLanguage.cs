// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// NCache Object Query Language sample
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using System;
using System.Collections;
using System.Configuration;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Web.Caching;
using Alachisoft.NCache.Sample.Data;

namespace Alachisoft.NCache.Samples
{
    /// <summary>
    /// Class that provides the functionality of the object query language sample.
    /// </summary>
    public class ObjectQueryLanguage
    {
        private static Cache _cache;

        /// <summary>
        /// Executing this method will perform the operations fo the sample using object query language.
        /// </summary>
        public static void Run()
        {
            // Initialize Cache 
            InitializeCache();

            // Querying an item in NCache requires either NamedTags or
            // Indexes defined for objects or both.

            // Generate a new instance of Product
            Product product = GenerateProduct(1);

            // Insert Items in cache with Named Tags
            InsertItemsWithNamedTags();

            // Querying a simple cache item via named Tags
            QueryItemsUsingNamedTags();

            // Insert Item on which Index is defined using NCache Manager
            InsertItemsWithPreDefinedIndex(product);

            // Query Items with pre-defined index
            QueryItemsUsingPreDefinedIndex();

            // Insert Items with both Named tags and predefined index
            InsertItemsWithNamedTagsAndIndex(product);

            // Query Items with both named tags and predefined index
            QueryItemsUsingNamedTagsAndIndex();

            // Dispose the cache once done
            _cache.Dispose();
        }

        /// <summary>
        /// This method initializes the cache
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
            _cache = NCache.Web.Caching.NCache.InitializeCache(cache);

            Console.WriteLine("Cache initialized successfully");
        }

        /// <summary>
        /// This method inserts items in cache with named tags.
        /// </summary>
        private static void InsertItemsWithNamedTags()
        {
            NamedTagsDictionary nameTagDict = new NamedTagsDictionary();

            nameTagDict.Add("Employee", "DavidBrown");
            nameTagDict.Add("Department", "Mathematics");

            _cache.Add("Employee:DavidBrown", "Department:Mathematics", nameTagDict);

            Console.WriteLine("Items added in cache.");
        }

        /// <summary>
        /// This method queries Items from NCache using named tags.
        /// </summary>
        private static void QueryItemsUsingNamedTags()
        {
            // Defining Searching criteria
            Hashtable criteria = new Hashtable();
            criteria["Employee"] = "DavidBrown";

            string query = "SELECT System.String WHERE this.Employee = ?";

            ICacheReader result = _cache.ExecuteReader(query, criteria);

            if (!result.IsClosed)
            {
                while (result.Read())
                {
                    Console.WriteLine(result[0].ToString());
                }
            }

            Console.WriteLine();
        }

        /// <summary>
        /// This method inserts items in the cache on which index was defined using NCache Manager.
        /// </summary>
        private static void InsertItemsWithPreDefinedIndex(Product product)
        {
            // New product object whose index is definex in NCache Manager
            _cache.Add("Product:CellularPhoneHTC", product);

            Console.WriteLine("Item Inserted in cache.");
        }

        /// <summary>
        /// This method queries items in the cache on which index was defined using NCache Manager.
        /// </summary>
        private static void QueryItemsUsingPreDefinedIndex()
        {
            //  Query can only be applied to C# Primitive data types:	
            //  and for those non primitive data types whose indexes are defined in NCache manager

            string query = "SELECT Alachisoft.NCache.Sample.Data.Product Where this.Id = ?";
            var criteria = new Hashtable();
            criteria["Id"] = 1;

            ICacheReader result = _cache.ExecuteReader(query, criteria);

            if (!result.IsClosed)
            {
                while (result.Read())
                {
                    Product productFound = (Product)result[1];
                    PrintProductDetails(productFound);
                }
            }

            Console.WriteLine();
        }

        /// <summary>
        /// This method inserts items in the cache with named tags and a predefined index using NCache Manager.
        /// </summary>
        private static void InsertItemsWithNamedTagsAndIndex(Product product)
        {
            //Querying indexed object via NamedTags

            var nameTagDict = new NamedTagsDictionary();
            nameTagDict.Add("ProductName", "HTCPhone");
            nameTagDict.Add("Class", "Electronics");

            _cache.Insert("Product:CellularPhoneHTC", product, nameTagDict);
        }

        /// <summary>
        /// This method queries items from the cache using both named tags and pre-defined index.
        /// </summary>
        private static void QueryItemsUsingNamedTagsAndIndex()
        {
            string query = "SELECT Alachisoft.NCache.Sample.Data.Product Where this.ProductName = ?";

            var criteria = new Hashtable();
            criteria["ProductName"] = "HTCPhone";

            ICacheReader result = _cache.ExecuteReader(query, criteria);

            if (!result.IsClosed)
            {
                while (result.Read())
                {
                    Product productFound = (Product)result[1];
                    PrintProductDetails(productFound);
                }
            }

            Console.WriteLine();
        }

        /// <summary>
        /// This method generates a new instance of product with the specified key.
        /// </summary>
        /// <param name="key"> Key that will be set as productId. </param>
        /// <returns> returns a new instance of Product. </returns>
        private static Product GenerateProduct(int key)
        {
            return new Product(){Id = key, Name = "HTCPhone", ClassName = "Electronics", Category = "QCPassed"} ;
        }

        /// <summary>
        /// Method for printing details of product type.
        /// </summary>
        /// <param name="product"> Product whos details will be printed</param>
        internal static void PrintProductDetails(Product product)
        {
            Console.WriteLine("Id:       " + product.Id);
            Console.WriteLine("Name:     " + product.Name);
            Console.WriteLine("Class:    " + product.ClassName);
            Console.WriteLine("Category: " + product.Category);
            Console.WriteLine();
        }
    }

}