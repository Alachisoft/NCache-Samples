using System.Collections.Generic;

namespace Sample.CustomerService.Domain
{
    public class Categories:Entity<int>
    {
        public Categories() { }
        public virtual string CategoryName { get; set; }
        public virtual string Description { get; set; }
        public virtual byte[] Picture { get; set; }

        public virtual ISet<Products> Products { get; set; }

        public virtual void AddProduct(Products product)
        {
            Products.Add(product);
            product.Categories = this;
        }
    }
}
