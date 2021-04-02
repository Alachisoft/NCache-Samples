using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using Sample.CustomerService.Domain;


namespace Sample.CustomerService.Maps 
{
    
    
    public class CustomerdemographicsMap : ClassMapping<Customerdemographics> 
	{
        
        public CustomerdemographicsMap() {
			Table("`CustomerDemographics`");
			Lazy(true);
			Id(x => x.Id, map =>
			{
				map.Generator(Generators.Assigned);
				map.Column("`CustomerTypeID`");
			});
			Property(x => x.Customerdesc, map =>
			{
				map.Column("`CustomerDesc`");
			});
			Set(x => x.Customercustomerdemo, colmap =>  { colmap.Key(x => x.Column("`CustomerTypeID`")); colmap.Inverse(true); }, map => { map.OneToMany(); });

			Cache(mapping =>
			{
				mapping.Region("region3");
				mapping.Usage(CacheUsage.NonstrictReadWrite);
			});
		}
    }
}
