using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.CacheLoader;
using Alachisoft.NCache.Sample.Data;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RefresherSample
{
    public class Refresher : ICacheLoader
    {
        private string _connectionString;
        private static ICache _cache;

        public void Init(IDictionary<string, string> parameters, string cacheName)
        {

            if (parameters != null && parameters.Count > 0)
                _connectionString = parameters.ContainsKey("ConnectionString")
                    ? parameters["ConnectionString"] as string : string.Empty;
            if (string.IsNullOrEmpty(cacheName))
                throw new ArgumentException("Cache name cannot be null or empty");

            _cache = CacheManager.GetCache(cacheName);
        }

        public object LoadDatasetOnStartup(string dataset)
        {
            IList<object> loadDatasetAtStartup;

            if (string.IsNullOrEmpty(dataset))
                throw new InvalidOperationException("Invalid dataset.");

            switch (dataset.ToLower())
            {
                case "products":
                    loadDatasetAtStartup = FetchProductsFromDataSouce();
                    break;
                case "Orders":
                    loadDatasetAtStartup = FetchOrdersFromDataSouce();
                    break;
                default:
                    throw new InvalidOperationException("Invalid Dataset.");
            }

            string[] keys = GetKeys(loadDatasetAtStartup);
            IDictionary<string, CacheItem> cacheData = GetCacheItemDictionary(keys, loadDatasetAtStartup);
            _cache.InsertBulk(cacheData);

            object userContext = DateTime.Now;
            return userContext;
        }

        public object RefreshDataset(string dataSet, object userContext)
        {
            if (string.IsNullOrEmpty(dataSet))
                throw new InvalidOperationException("Invalid dataset.");

            DateTime? lastRefreshTime;

            switch (dataSet.ToLower())
            {
                case "products":
                    lastRefreshTime = userContext as DateTime?;
                    IList<Product> productsNeedToRefresh = FetchUpdatedProducts(lastRefreshTime) as IList<Product>;
                    foreach(var product in productsNeedToRefresh)
                    {
                        string key = $"ProductID:{product.Id}";
                        CacheItem cacheItem = new CacheItem(product);
                        _cache.Insert(key, cacheItem);
                    }
                    break;
                case "Orders":
                    lastRefreshTime = userContext as DateTime?;
                    IList<Order> OrdersNeedToRefresh = FetchUpdatedOrders(lastRefreshTime) as IList<Order>;
                    foreach (var order in OrdersNeedToRefresh)
                    {
                        string key = $"OrderID:{order.OrderID}";
                        CacheItem cacheItem = new CacheItem(order);
                        _cache.Insert(key, cacheItem);
                    }
                    
                    break;
                default:
                    throw new InvalidOperationException("Invalid Dataset.");
            }

            userContext = DateTime.Now;
            return userContext;
        }

        public IDictionary<string, RefreshPreference> GetDatasetsToRefresh(IDictionary<string, object> userContexts)
        {
            IDictionary<string, RefreshPreference> DatasetsNeedToRefresh = new Dictionary<string, RefreshPreference>();
            DateTime? lastRefreshTime;
            bool datasetHasUpdated;

            foreach (var dataSet in userContexts.Keys)
            {
                switch (dataSet.ToLower())
                {
                    case "products":
                        lastRefreshTime = userContexts[dataSet] as DateTime?;
                        datasetHasUpdated = HasProductDatasetUpdated(dataSet, lastRefreshTime);
                        if (datasetHasUpdated)
                        {
                            DatasetsNeedToRefresh.Add(dataSet, RefreshPreference.RefreshNow);
                        }
                        break;
                    case "Orders":
                        lastRefreshTime = userContexts[dataSet] as DateTime?;
                        datasetHasUpdated = HasOrdersDatasetUpdated(dataSet, lastRefreshTime);
                        if (datasetHasUpdated)
                        {
                            DatasetsNeedToRefresh.Add(dataSet, RefreshPreference.RefreshOnNextTimeOfDay);
                        }
                        break;
                    default:
                        throw new InvalidOperationException("Invalid Dataset.");
                }               
            }

            return DatasetsNeedToRefresh;
        }

        public void Dispose()
        {
            // clean your unmanaged resources
        }

        public string[] GetKeys(IList<object> objects)
        {
            string[] keys = new string[objects.Count];
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = objects[i].GetType() == typeof(Product) ? $"ProductId:{(objects[i] as Product).Id}" : $"OrderID:{(objects[i] as Order).OrderID}";
            }

            return keys;
        }

        public static IDictionary<string, CacheItem> GetCacheItemDictionary(string[] keys, IList<Object> value)
        {
            IDictionary<string, CacheItem> items = new Dictionary<string, CacheItem>();
            CacheItem cacheItem = null;

            for (int i = 0; i < value.Count - 1; i++)
            {
                cacheItem = new CacheItem(value[i]);
                items.Add(keys[i], cacheItem);
            }

            return items;
        }

        private IList<object> FetchProductsFromDataSouce()
        {
            string Query = "select * from Products where UnitsInStock > 0";
            return ExecuteQuery(Query, "products");

        }

        private IList<object> FetchOrdersFromDataSouce()
        {
            string Query = "select * from dbo.Orders";
            return ExecuteQuery(Query, "Orders");

        }

        private bool HasProductDatasetUpdated(string dataSet, object dateTime)
        {
            bool result = false;
            string query = $"select count(*) from Products where CreationTime > '{dateTime as DateTime?}' and LastModify > '{dateTime as DateTime?}'";
            result = ExecuteAggregateQuery(query) > 0;
            return result;
        }

        private bool HasOrdersDatasetUpdated(string dataSet, object dateTime)
        {
            bool result = false;
            string query = $"select count(*) from Orders where OrderDate > '{dateTime as DateTime?}' and RequiredDate > '{dateTime as DateTime?}'";
            result = ExecuteAggregateQuery(query) > 0;
            return result;
        }
       
        private IList<object> FetchUpdatedProducts(object dateTime)
        {
            string Query = $"select * from Products where CreationTime > '{dateTime as DateTime?}' and LastModify > '{dateTime as DateTime?}'";
            return ExecuteQuery(Query, "products");

        }

        private IList<object> FetchUpdatedOrders(object dateTime)
        {           
            string Query = $"select * from Products where CreationTime > '{dateTime as DateTime?}' and LastModify > '{dateTime as DateTime?}'";
            return ExecuteQuery(Query, "products");

        }     
        
        private int ExecuteAggregateQuery(string Query)
        {
            int result = 0;
            using (SqlConnection myConnection = new SqlConnection(_connectionString))
            {
                SqlCommand oCmd = new SqlCommand(Query, myConnection);

                myConnection.Open();
                using (var reader = oCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = Convert.ToInt32(reader[0]);
                    }                   
                }
                myConnection.Close();
            }
            return result;
        }
        
        private IList<object> ExecuteQuery(string Query, string dataSet)
        {
            IList<object> Data;
            
            using (SqlConnection myConnection = new SqlConnection(_connectionString))
            {                
                SqlCommand oCmd = new SqlCommand(Query, myConnection);

                myConnection.Open();

                using (var reader = oCmd.ExecuteReader())
                {
                    Data = GetData(reader, dataSet);
                }
                    
                myConnection.Close();
            }            
            return Data ;
        }

        private IList<object>  GetData(SqlDataReader sqlDataReader, string dataSet)
        {
            IList<object> dataList = new List<object>();
            while (sqlDataReader.Read())
            {
                if (string.Compare(dataSet, "Orders", true) == 0)
                {
                    Order orders = new Order()
                    {
                        OrderID = Convert.ToInt32(sqlDataReader["OrderID"]),
                        ShipName = sqlDataReader["ShipName"].ToString(),
                        ShipAddress = sqlDataReader["ShipAddress"].ToString(),
                        ShipCity = sqlDataReader["ShipCity"].ToString()
                    };
                    dataList.Add(orders);

                }
                if (string.Compare(dataSet, "Products", true) == 0)
                {
                    Product product = new Product()
                    {
                        Name = sqlDataReader["ProductName"].ToString(),
                        Id = Convert.ToInt32(sqlDataReader["ProductID"]),
                        UnitsAvailable = Convert.ToInt32(sqlDataReader["UnitsInStock"]),
                        UnitPrice = Convert.ToInt32(sqlDataReader["UnitPrice"])
                    };
                    dataList.Add(product);
                    
                }
            }
            return dataList;
        }
        
    }
}
