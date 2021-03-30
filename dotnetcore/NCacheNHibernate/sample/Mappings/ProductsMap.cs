using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using Sample.CustomerService.Domain;

namespace Sample.CustomerService.Maps 
{
    
    
    public class ProductsMap : ClassMapping<Products> 
	{
        
        public ProductsMap() {
			Table("`Products`");
			Lazy(true);
			Id(x => x.Id, map =>
			{
				map.Generator(Generators.Identity);
				map.Column("`ProductID`");
			});
			Property(x => x.ProductName, map =>
			{
				map.Column("`ProductName`");
				map.NotNullable(true);
			});
			Property(x => x.QuantityPerUnit, map =>
			{
				map.Column("`QuantityPerUnit`");
			});
			Property(x => x.UnitPrice, map =>
			{
				map.Column("`UnitPrice`");
			});
			Property(x => x.UnitsInStock, map =>
			{
				map.Column("`UnitsInStock`");
			});
			Property(x => x.UnitsOnOrder, map =>
			{
				map.Column("`UnitsOnOrder`");
			});
			Property(x => x.ReorderLevel, map =>
			{
				map.Column("`ReorderLevel`");
			});
			Property(x => x.Discontinued, map =>
			{
				map.Column("`Discontinued`");
				map.NotNullable(true);
			});
			ManyToOne(x => x.Suppliers, map => 
			{
				map.Column("`SupplierID`");
				map.NotNullable(true);
				map.Cascade(Cascade.None);
			});

			ManyToOne(x => x.Categories, map => 
			{
				map.Column("`CategoryID`");
				map.NotNullable(true);
				map.Cascade(Cascade.None);
			});

			Set(x => x.Orderdetails, colmap =>  { colmap.Key(x => x.Column("`ProductID`")); colmap.Inverse(true); }, map => { map.OneToMany(); });

			Cache(mapping =>
			{
				mapping.Region("region3");
				mapping.Usage(CacheUsage.NonstrictReadWrite);
			});
		}
    }
}
