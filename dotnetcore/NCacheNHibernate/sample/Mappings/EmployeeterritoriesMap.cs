using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using Sample.CustomerService.Domain;


namespace Sample.CustomerService.Maps {
    
    
    public class EmployeeterritoriesMap : ClassMapping<Employeeterritories> {
        
        public EmployeeterritoriesMap() {
			Table("`EmployeeTerritories`");
			Lazy(true);
			ComposedId(compId =>
				{
					compId.ManyToOne(x => x.Employees, map =>
					{
						map.Column("`EmployeeID`");
						map.Cascade(Cascade.None);
					});
					compId.ManyToOne(x => x.Territories, map =>
					{
						map.Column("`TerritoryID`");
						map.Cascade(Cascade.None);
					});
				});

			Cache(mapping =>
			{
				mapping.Region("region2");
				mapping.Usage(CacheUsage.NonstrictReadWrite);
			});

		}
    }
}
