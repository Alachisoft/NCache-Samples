using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Dependencies;
using Models;
using System;
using System.Configuration;
using System.Threading;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;
using System.Net.Http;

namespace Alachisoft.NCache.Samples
{
    public class NotifyExtensibleDependencyUsage
    {
        private static ICache _cache;
        private static string _connectionString;
        private static DocumentClient _client = null;

        // In case accessing database from a remote client through network or even from a container on the same host, 
        // SSL certification verification is required
        // IMPORTANT: For development purposes, the SSL authentication stage has been circumvented. HOWEVER, in customerion,
        // the ServerCertificateCustomValidationCallback should be implemented with concrete authentication logic
        static readonly HttpClientHandler cosmosDbHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (a, b, c, d) => true
        };

        // Cosmos DB connection policy settings
        static readonly ConnectionPolicy cosmosDBConnectionPolicy = new ConnectionPolicy
        {
            EnableEndpointDiscovery = false
        };

        private static string _endPoint;
        private static string _authKey;
        private static string _monitoredCollection;
        private static string _leaseCollection; 
        private static string _databaseName;

        /// <summary> 
        /// Executing this method will perform all the operations of the sample
        /// </summary>
        public static void Run()
        {
            // Initialize cache 
            InitializeCache();

            _endPoint = ConfigurationManager.AppSettings["EndPoint"];
            _authKey = ConfigurationManager.AppSettings["AuthKey"];
            _monitoredCollection = ConfigurationManager.AppSettings["MonitoredCollection"];
            _leaseCollection = ConfigurationManager.AppSettings["LeaseCollection"];
            _databaseName = ConfigurationManager.AppSettings["DatabaseName"];

            string customerId = "1";
            _cache.Clear();
            Console.WriteLine("Enter Customer id :");
            customerId = Console.ReadLine();
            //var xx = ChangeProcessorManager.Instance;
            // Fetch a sampple customer from the database 
            Customer customer = LoadCustomerFromDatabase(customerId);


            // Add customer to the cache with cosmosdb Dependency
            AddCustomerToCacheWithDependency(customer);

            // Fetching cached item to verify successfull insertion
            customer = _cache.Get<Customer>(customer.Id);

            if (customer != null)
            {
                Console.WriteLine("customer fetched from cache");
            }
            else
            {
                Console.WriteLine("customer not found in cache");
                return;
            }

            // Let's modiefy the customer
            customer.ContactName += 1;
            // Update customer in DemoDatabase db to trigger cosmosdb server dependency
            UpdateCustomersInDatabase(customer);

            // Wait for CosmosDb dependency to trigger
            Thread.Sleep(10000);

            // Get objects from cache
            GetObjectsFromCache(customer.Id);
        }

        /// <summary>
        /// This method updates customer into the database.
        /// </summary>
        /// <param name="customer"> customer that will be updated. </param>
        /// <returns> Returns the count of affected rows in the database. </returns>
        private static int UpdateCustomersInDatabase(Customer customer)
        {
            var documentURI = UriFactory.CreateDocumentUri(_databaseName, _monitoredCollection, customer.Id);
            var partitionKey = new PartitionKey(customer.Id);
            var requestOptions = new RequestOptions
            {
                PartitionKey = partitionKey
            };

            var response = _client.ReplaceDocumentAsync(
                documentUri: UriFactory.CreateDocumentUri(_databaseName, _monitoredCollection, customer.Id),
                document: customer,
                options: requestOptions)
                .Result;

            return 1;
        }

        /// <summary>
        /// This method initializes the cache
        /// </summary>
        private static void InitializeCache()
        {
            string cache = ConfigurationManager.AppSettings["CacheID"];
            

            // Initialize an instance of the cache to begin performing operations:
            _cache = Alachisoft.NCache.Client.CacheManager.GetCache(cache);

            // Print output on console
            Console.WriteLine(string.Format("\nCache '{0}' is initialized.", cache));
        }

        /// <summary>
        /// This method fetches instance of the customer from the database.
        /// </summary>
        /// <param name="customerId"> customer Id that will be used to fetch data from the database. </param>
        /// <returns> Returns the instance of customer that was fetched. </returns>
        private static Customer LoadCustomerFromDatabase(string customerId)
        {
            Customer customer = null;
            

            var documentURI = UriFactory.CreateDocumentUri(_databaseName, _monitoredCollection, customerId);
            var partitionKey = new PartitionKey(customerId);
            var requestOptions = new RequestOptions
            {
                PartitionKey = partitionKey
            };

            _client = new DocumentClient(
            serviceEndpoint: new Uri(_endPoint),
            authKeyOrResourceToken: _authKey,
            handler: cosmosDbHandler,
            connectionPolicy: cosmosDBConnectionPolicy
        );

            // The following code is for retrieving the required item to the Cosmos DB database.
            // In case of successful read, the response object HttpStatusCode would be set to OK.
            // Otherwise, a DocumentClientException will be thrown with the HttpStatusCode property
            // detailing the cause of the exception
            ResourceResponse<Document> response = _client.ReadDocumentAsync(
                documentUri: documentURI,
                options: requestOptions)
                .Result;

            // In case of successful database GET operation, the code flow will reach here at which point we can safely
            // add the corresponding information in the cache without worrying about cache-database inconsistency

            customer = (Customer)(dynamic)response.Resource;

            return customer;
        }

        /// <summary>
        /// This method adds an instance of Produc in the cache with notification based Dependency.
        /// </summary>
        /// <param name="customer"> customer that is to be added. </param>
        private static void AddCustomerToCacheWithDependency(Customer customer)
        {
            string endPoint = ConfigurationManager.AppSettings["EndPoint"];
            string authKey = ConfigurationManager.AppSettings["AuthKey"];
            string monitoredCollection = ConfigurationManager.AppSettings["MonitoredCollection"];
            string leaseCollection = ConfigurationManager.AppSettings["LeaseCollection"];
            string databaseName = ConfigurationManager.AppSettings["DatabaseName"];
            

            // Let's create cosmosdb depdenency
            CacheDependency cosmosDbDependency = new CosmosDbNotificationDependency<Customer>(customer.Id,
                endPoint,
                authKey,
                databaseName,
                monitoredCollection,
                endPoint,
                authKey,
                databaseName,
                leaseCollection);

            CacheItem cacheItem = new CacheItem(customer);
            cacheItem.Dependency = cosmosDbDependency;

            // Inserting Loaded customer into cache with key: [item:1]
            _cache.Add(customer.Id, cacheItem);
        }

        /// <summary>
        /// This methods generates a key for the specified customer.
        /// </summary>
        /// <param name="customer"> customer whos key will be generated. </param>
        /// <returns> Returns key of the customer specified. </returns>
        private static string GenerateCacheKey(Customer customer)
        {
            string cacheKey = "Customer#" + customer.Id;
            return cacheKey;
        }

        /// <summary>
        /// This method gets objects from the cache
        /// </summary>
        /// <param name="key"> String keys to get objects from cache </param>
        private static void GetObjectsFromCache(params string[] keys)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                string value = _cache.Get<object>(keys[i])?.ToString();
                if (value == null)
                {
                    Console.WriteLine(string.Format("Item with key {0} is removed from cache ", keys[i]));
                }
                else
                {
                    Console.WriteLine(string.Format("Item with key {0} is still in cache ", keys[i]));
                }
            }
        }
    }
}
