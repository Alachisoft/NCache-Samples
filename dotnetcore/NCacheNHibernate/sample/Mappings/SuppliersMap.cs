using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using Sample.CustomerService.Domain;


namespace Sample.CustomerService.Maps 
{
    
    
    public class SuppliersMap : ClassMapping<Suppliers> 
	{
        
        public SuppliersMap() {
			Table("`Suppliers`");
			Lazy(true);
			Id(x => x.Id, map =>
			{
				map.Generator(Generators.Identity);
				map.Column("`SupplierID`");
			});
			Property(x => x.CompanyName, map =>
			{
				map.Column("`CompanyName`");
				map.NotNullable(true);
			});
			Property(x => x.ContactName, map =>
			{
				map.Column("`ContactName`");
			});
			Property(x => x.ContactTitle, map =>
			{
				map.Column("`ContactTitle`");
			});
			Property(x => x.Address, map =>
			{
				map.Column("`Address`");
			});
			Property(x => x.City, map =>
			{
				map.Column("`City`");
			});
			Property(x => x.Region, map =>
			{
				map.Column("`Region`");
			});
			Property(x => x.PostalCode, map =>
			{
				map.Column("`PostalCode`");
			});
			Property(x => x.Country, map =>
			{
				map.Column("`Country`");
			});
			Property(x => x.Phone, map =>
			{
				map.Column("`Phone`");
			});
			Property(x => x.Fax, map =>
			{
				map.Column("`Fax`");
			});
			Property(x => x.HomePage, map =>
			{
				map.Column("`HomePage`");
			});
			Set(x => x.Products, colmap =>  { colmap.Key(x => x.Column("`SupplierID`")); colmap.Inverse(true); }, map => { map.OneToMany(); });

			Cache(mapping =>
			{
				mapping.Region("region3");
				mapping.Usage(CacheUsage.NonstrictReadWrite);
			});
		}
    }
}
