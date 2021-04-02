using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using Sample.CustomerService.Domain;


namespace Sample.CustomerService.Maps 
{
    
    
    public class RegionMap : ClassMapping<Region> 
	{
        
        public RegionMap() 
		{
			Table("`Region`");
			Lazy(true);
			Id(x => x.Id, map =>
			{
				map.Generator(Generators.Identity);
				map.Column("`RegionID`");
			});
			Property(x => x.RegionDescription, map =>
			{
				map.Column("`RegionDescription`");
				map.NotNullable(true);
			});
			Set(x => x.Territories, colmap =>  { colmap.Key(x => x.Column("`RegionID`")); colmap.Inverse(true); }, map => { map.OneToMany(); });

			Cache(mapping =>
			{
				mapping.Region("region2");
				mapping.Usage(CacheUsage.NonstrictReadWrite);
			});
		}
    }
}
