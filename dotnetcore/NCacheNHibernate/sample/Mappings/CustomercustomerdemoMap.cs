using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using Sample.CustomerService.Domain;


namespace Sample.CustomerService.Maps 
{
    
    
    public class CustomercustomerdemoMap : ClassMapping<Customercustomerdemo> 
	{
        
        public CustomercustomerdemoMap() {
			Table("`CustomerCustomerDemo`");
			Lazy(true);
			ComposedId(compId =>
				{
					compId.ManyToOne(x => x.Customers, map =>
					{
						map.Column("`CustomerID`");
						map.Cascade(Cascade.None);
					});

					compId.ManyToOne(x => x.Customerdemographics, map =>
					{
						map.Column("`CustomerTypeID`");
						map.Cascade(Cascade.None);
					});
				});

			Cache(mapping =>
			{
				mapping.Region("region3");
				mapping.Usage(CacheUsage.NonstrictReadWrite);
			});
		}
    }
}
