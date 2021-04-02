using System.Collections.Generic;

namespace Sample.CustomerService.Domain 
{
    public class Region :Entity<int>
    {
        public Region() { }
        public virtual string RegionDescription { get; set; }

        public virtual ISet<Territories> Territories { get; set; }

        public virtual void AddTerritory(Territories territories)
        {
            Territories.Add(territories);
            territories.Region = this;
        }
    }
}
