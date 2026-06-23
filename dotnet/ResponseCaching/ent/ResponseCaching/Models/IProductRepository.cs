
namespace ResponseCaching.Models
{
    /// <summary>
    /// Methods / Operations allowed 
    /// </summary>
    public interface IProductsRepository
    {
        ProductModel GetAllProductsFromDatabase(int? price);
        ProductModel GetProductById(int ProductID);
        bool UpdateProductInDatabase(Product product);
    }
}
