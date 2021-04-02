using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using Sample.CustomerService.Domain;


namespace Sample.CustomerService.Maps 
{
    
    
    public class ShippersMap : ClassMapping<Shippers> 
	{
        
        public ShippersMap() {
			Table("`Shippers`");
			Lazy(true);
			Id(x => x.Id, map =>
			{
				map.Generator(Generators.Identity);
				map.Column("`ShipperID`");
			});
			Property(x => x.CompanyName, map =>
			{
				map.Column("`CompanyName`");
				map.NotNullable(true);
			});
			Property(x => x.Phone, map =>
			{
				map.Column("`Phone`");
			});
			Set(x => x.Orders, colmap =>  { colmap.Key(x => x.Column("`ShipVia`")); colmap.Inverse(true); }, map => { map.OneToMany(); });

			Cache(mapping =>
			{
				mapping.Region("region1");
				mapping.Usage(CacheUsage.NonstrictReadWrite);
			});
		}
    }
}
