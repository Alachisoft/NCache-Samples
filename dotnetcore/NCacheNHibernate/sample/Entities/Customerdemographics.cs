using System.Collections.Generic;

namespace Sample.CustomerService.Domain 
{
    public class Customerdemographics:Entity<string> 
    {
        public Customerdemographics() { }
        public virtual string Customerdesc { get; set; }

        public virtual ISet<Customercustomerdemo> Customercustomerdemo { get; set; }

        public virtual void AddCustomerDemo(Customercustomerdemo customerCustomerDemo)
        {
            Customercustomerdemo.Add(customerCustomerDemo);
            customerCustomerDemo.Customerdemographics = this;
        }
    }
}
