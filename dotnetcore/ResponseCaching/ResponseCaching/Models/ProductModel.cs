using Microsoft.Extensions.Configuration;
using NCacheResponseCaching.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace NCacheResponseCaching.Models
{
    /// <summary>
    /// Model Class that perform db operation
    /// </summary>
    public class ProductModel
    {
        public ProductModel()
        {
            Products = new List<Product>();
        }

        public List<Product> Products { get; set; }
        public int Id { get; set; }
        public string ProductName { get ; set ; }
        public decimal UnitPrice { get ; set ; }
        public string QuantityPerUnit { get; set; }
    }
}
