using System.Collections.Generic;

namespace Sample.CustomerService.Domain 
{
    
    
    public class Customers : Entity<string>
    {
        public Customers() { }
        public virtual string CompanyName { get; set; }
        public virtual string ContactName { get; set; }
        public virtual string ContactTitle { get; set; }
        public virtual string Address { get; set; }
        public virtual string City { get; set; }
        public virtual string Region { get; set; }
        public virtual string Postalcode { get; set; }
        public virtual string Country { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Fax { get; set; }

        public virtual ISet<Orders> Orders { get; set; }
        public virtual ISet<Customercustomerdemo> Customercustomerdemo { get; set; }
        public virtual void AddOrder(Orders order)
        {
            Orders.Add(order);
            order.Customers = this;
        }
        public virtual void AddCustomerDemo(Customercustomerdemo customerCustomerDemo)
        {
            Customercustomerdemo.Add(customerCustomerDemo);
            customerCustomerDemo.Customers = this;
        }

    }
}
