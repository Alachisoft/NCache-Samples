using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCacheResponseCaching.Data
{
    /// <summary>
    /// Product Class
    /// </summary>
    public class Product
    {
        int productID;
        string productName;
        decimal unitPrice;
        string quantityPerUnit;
      

        public Product(int productID, string productName, decimal unitPrice, string quantityPerUnit )
        {
            this.ProductID = productID;
            this.ProductName = productName;
            this.UnitPrice = unitPrice;
            this.QuantityPerUnit = quantityPerUnit;
        }

       public Product() { }

        public int ProductID { get => productID; set => productID = value; }
        public string ProductName { get => productName; set => productName = value; }
        public decimal UnitPrice { get => unitPrice; set => unitPrice = value; }
        public string QuantityPerUnit { get => quantityPerUnit; set => quantityPerUnit = value; }
        
    }
}
