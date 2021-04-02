using NCacheResponseCaching.Data;
using NCacheResponseCaching.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResponseCaching.Models
{
    /// <summary>
    /// Methods / Operations allowed 
    /// </summary>
   public interface IProductsRepository
    {
        ProductModel GetAllProductsFromDatabase(int? price);
        ProductModel GetProductById(int id);
        bool UpdateProductInDatabase(Product product);
    }
}
