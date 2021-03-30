using System.Collections.Generic;

namespace Sample.CustomerService.Domain 
{
    public class Products:Entity<int> 
    {
        public Products() { }
        public virtual Suppliers Suppliers { get; set; }
        public virtual Categories Categories { get; set; }
        public virtual string ProductName { get; set; }
        public virtual string QuantityPerUnit { get; set; }
        public virtual decimal? UnitPrice { get; set; }
        public virtual short? UnitsInStock { get; set; }
        public virtual short? UnitsOnOrder { get; set; }
        public virtual short? ReorderLevel { get; set; }
        public virtual bool Discontinued { get; set; }

        public virtual ISet<Orderdetails> Orderdetails { get; set; }

        public virtual void AddOrderDetail(Orderdetails orderdetails)
        {
            Orderdetails.Add(orderdetails);
            orderdetails.Products = this;
        }
    }
}
