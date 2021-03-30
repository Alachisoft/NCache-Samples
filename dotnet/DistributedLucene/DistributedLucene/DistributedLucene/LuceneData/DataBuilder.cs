using DistributedLucene.LuceneData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedLucene.LuceneData
{
    class DataBuilder
    {
        readonly string[] ProductNames = new string[] { "", ""};

        /// <summary>
        /// Populates and returns the Products.
        /// </summary>
        /// <param name="count">Number of product objects to fetch.</param>
        /// <returns></returns>
        public static IEnumerable<Product> FetchProducts(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Product product = new Product();

                product.Name = ProductNameData.ProductNames[i];

                if (i < 15)
                {
                    product.Category = $" {product.ToString()} Beverages";
                    product.Description = "This category contains products like Soft Drinks, Coffees, Teas, Beers and Ales. Price ranges vary from product to product. Other products lie in different Product Categories. ";
                }
                   
                else if (i >= 15 && i < 30)
                {
                    product.Category = $" {product.ToString()} Seafood";
                    product.Description = "This category contains products like Seaweed and Fish. Price ranges vary from product to product. Other products lie in different Product Categories. ";
                }
                 
                else if (i >= 30 && i < 45)
                {
                    product.Category = $" {product.ToString()} Meat";
                    product.Description = "This category contains products like Chicken, Lamb and Beef.  Price ranges vary from product to product. Other products lie in different Product Categories. ";
                }

                else
                {
                    product.Category = $" {product.ToString()} Other Category";
                    product.Description = "This category contains products like Dried Fruits and Packed Foods. Price ranges vary from product to product. Other products lie in different Product Categories. "; 
                }

                yield return product;
            }
        }
    }
}
