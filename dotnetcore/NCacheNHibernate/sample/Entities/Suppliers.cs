using System.Collections.Generic;

namespace Sample.CustomerService.Domain
{
    public class Suppliers :Entity<int>
    {
        public Suppliers() { }
        public virtual string CompanyName { get; set; }
        public virtual string ContactName { get; set; }
        public virtual string ContactTitle { get; set; }
        public virtual string Address { get; set; }
        public virtual string City { get; set; }
        public virtual string Region { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string Country { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Fax { get; set; }
        public virtual string HomePage { get; set; }

        public virtual ISet<Products> Products { get; set; }

        public virtual void AddProduct(Products products)
        {
            Products.Add(products);
            products.Suppliers = this;
        }

    }
}
