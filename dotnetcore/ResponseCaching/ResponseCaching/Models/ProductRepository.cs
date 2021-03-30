using NCacheResponseCaching.Data;
using NCacheResponseCaching.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResponseCaching.Models
{
    public class ProductRepository : IProductsRepository
    {
        private readonly AppDbContext context;
        public ProductRepository(AppDbContext context) 
        {
            this.context = context;
        }
        /// <summary>
        /// Method to fetch all products from database 
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public ProductModel GetAllProductsFromDatabase(int? price)
        {
            price = price ?? 1000;
            var products= context.Products;
            List<Product> productsList = products.Where(x => x.UnitPrice <= price).ToList();
            ProductModel productModel = new ProductModel();
            productModel.Products = productsList;
            return productModel;
        }

        /// <summary>
        /// Method to fetch product from database based on the provided Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductModel GetProductById(int id)
        {
            var result= context.Products.Find(id);
            ProductModel productModel = new ProductModel()
            {
                Id = result.ProductID,
                ProductName = result.ProductName,
                QuantityPerUnit = result.QuantityPerUnit,
                UnitPrice = result.UnitPrice
            };
            return productModel;
        }

        /// <summary>
        /// Method that allows to update the Product in database.
        /// </summary>
        /// <param name="updatedProduct"></param>
        /// <returns></returns>
        public bool UpdateProductInDatabase(Product updatedProduct)
        {
            var product = context.Products.Attach(updatedProduct);
            product.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return true;
        }
    }
}
