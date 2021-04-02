using System.Collections.Generic;

namespace Sample.CustomerService.Domain 
{
    
    public class Territories :Entity<string>
    {
        public Territories() { }
        public virtual Region Region { get; set; }
        public virtual string Territorydescription { get; set; }

        public virtual ISet<Employeeterritories> Employeeterritories { get; set; }

        public virtual void AddEmployeeTerritory(Employeeterritories employeeterritories)
        {
            Employeeterritories.Add(employeeterritories);
            employeeterritories.Territories = this;
        }
    }
}
