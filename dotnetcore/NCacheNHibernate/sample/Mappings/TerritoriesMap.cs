using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using Sample.CustomerService.Domain;


namespace Sample.CustomerService.Maps 
{
    
    
    public class TerritoriesMap : ClassMapping<Territories> 
	{
        
        public TerritoriesMap() {
			Table("`Territories`");
			Lazy(true);
			Id(x => x.Id, map =>
			{
				map.Generator(Generators.Assigned);
				map.Column("`TerritoryID`");
			});
			Property(x => x.Territorydescription, map =>
			{
				map.Column("`TerritoryDescription`");
				map.NotNullable(true);
			});
			ManyToOne(x => x.Region, map => { map.Column("`RegionID`"); map.Cascade(Cascade.None); });

			Set(x => x.Employeeterritories, colmap =>  { colmap.Key(x => x.Column("`TerritoryID`")); colmap.Inverse(true); }, map => { map.OneToMany(); });

			Cache(mapping =>
			{
				mapping.Region("region2");
				mapping.Usage(CacheUsage.NonstrictReadWrite);
			});
		}
    }
}
