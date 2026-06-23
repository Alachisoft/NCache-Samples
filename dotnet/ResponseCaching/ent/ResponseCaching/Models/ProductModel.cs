
namespace ResponseCaching.Models
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
        public int ProductID { get; set; }
        public string ProductName { get ; set ; }
        public decimal? UnitPrice { get ; set ; }
        public string QuantityPerUnit { get; set; }
    }
}
