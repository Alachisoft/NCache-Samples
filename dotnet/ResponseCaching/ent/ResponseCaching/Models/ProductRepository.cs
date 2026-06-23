// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

namespace ResponseCaching.Models
{
    /// <summary>
    /// Handles all database operations related to products such as 
    /// fetching and updating product records.
    /// </summary>
    public class ProductRepository : IProductsRepository
    {
        private readonly AppDbContext context;

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Initializes the ProductRepository with the specified database context.
        /// </summary>
        /// <param name="context">The database context used to access product data.</param>
        public ProductRepository(AppDbContext context) 
        {
            this.context = context;
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Method to fetch all products from database 
        /// </summary>
        /// <param name="price"></param>
        /// <returns>object containing a list of products</returns>
        public ProductModel GetAllProductsFromDatabase(int? price)
        {
            price = price ?? 1000;
            var products= context.Products;
            List<Product> productsList = products.Where(x => x.UnitPrice <= price).ToList();
            ProductModel productModel = new ProductModel();
            productModel.Products = productsList;
            return productModel;
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Method to fetch product from database based on the provided ProductID
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns>object containing the details of the product </returns>
        public ProductModel GetProductById(int ProductID)
        {
            var result= context.Products.Find(ProductID);
            ProductModel productModel = new ProductModel()
            {
                ProductID = result.ProductId,
                ProductName = result.ProductName,
                QuantityPerUnit = result.QuantityPerUnit,
                UnitPrice = result.UnitPrice
            };
            return productModel;
        }

        /// -------------------------------------------------------------------------------
        /// <summary>
        /// Method that allows to update the Product in database.
        /// </summary>
        /// <param name="updatedProduct"></param>
        /// <returns>/// A boolean value indicating whether the product was successfully updated in the database.</returns>
        public bool UpdateProductInDatabase(Product updatedProduct)
        {
            var product = context.Products.Attach(updatedProduct);
            product.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return true;
        }
    }
}
