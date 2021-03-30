using System.Collections.Generic;

namespace Sample.CustomerService.Domain 
{
    public class Shippers :Entity<int>
    {
        public Shippers() { }
        public virtual string CompanyName { get; set; }
        public virtual string Phone { get; set; }

        public virtual ISet<Orders> Orders { get; set; }

        public virtual void AddOrder(Orders orders)
        {
            Orders.Add(orders);
            orders.Shippers = this;
        }
    }
}
