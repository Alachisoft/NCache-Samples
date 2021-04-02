using System;
using System.Collections.Generic;

namespace Sample.CustomerService.Domain 
{
    public class Orders :Entity<int>
    {
        public Orders() { }
        public virtual Customers Customers { get; set; }
        public virtual Employees Employees { get; set; }
        public virtual Shippers Shippers { get; set; }
        public virtual DateTime? OrderDate { get; set; }
        public virtual DateTime? RequiredDate { get; set; }
        public virtual DateTime? ShippedDate { get; set; }
        public virtual decimal? Freight { get; set; }
        public virtual string ShipName { get; set; }
        public virtual string ShipAddress { get; set; }
        public virtual string ShipCity { get; set; }
        public virtual string ShipRegion { get; set; }
        public virtual string ShipPostalCode { get; set; }
        public virtual string ShipCountry { get; set; }

        public virtual ISet<Orderdetails> Orderdetails { get; set; }

        public virtual void AddOrderDetail(Orderdetails orderdetails)
        {
            Orderdetails.Add(orderdetails);
            orderdetails.Orders = this;
        }
    }
}
