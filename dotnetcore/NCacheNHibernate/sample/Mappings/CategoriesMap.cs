using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using Sample.CustomerService.Domain;


namespace Sample.CustomerService.Maps 
{
    
    
    public class CategoriesMap : ClassMapping<Categories> 
    {
        
        public CategoriesMap() {
            Table("`Categories`");
			Lazy(true);
			Id(x => x.Id, map =>
            {
                map.Generator(Generators.Identity);
                map.Column("`CategoryID`");
            });
			Property(x => x.CategoryName, map =>
            {
                map.Column("`CategoryName`");
                map.NotNullable(true);
            });
			Property(x => x.Description, map =>
            {
                map.Column("`Description`");
            });
			Property(x => x.Picture, map =>
            {
                map.Column("`Picture`");
            });
			Set(x => x.Products, colmap =>  { colmap.Key(x => x.Column("`CategoryID`")); colmap.Inverse(true); }, map => { map.OneToMany(); });

            Cache(mapping =>
            {
                mapping.Region("region3");
                mapping.Usage(CacheUsage.NonstrictReadWrite);
            });
        }
    }
}
