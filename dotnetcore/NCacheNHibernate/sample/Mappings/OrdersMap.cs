using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using Sample.CustomerService.Domain;


namespace Sample.CustomerService.Maps
{
    
    
    public class OrdersMap : ClassMapping<Orders>
	{
        
        public OrdersMap() {
			Table("`Orders`");
			Lazy(true);
			Id(x => x.Id, map =>
			{
				map.Generator(Generators.Identity);
				map.Column("`OrderID`");
			}) ;
			Property(x => x.OrderDate, map =>
			{
				map.Column("`OrderDate`");
			});
			Property(x => x.RequiredDate, map =>
			{
				map.Column("`RequiredDate`");
			});
			Property(x => x.ShippedDate, map =>
			{
				map.Column("`ShippedDate`");
			});
			Property(x => x.Freight, map =>
			{
				map.Column("`Freight`");
			});
			Property(x => x.ShipName, map =>
			{
				map.Column("`ShipName`");
			});
			Property(x => x.ShipAddress, map =>
			{
				map.Column("`ShipAddress`");
			});
			Property(x => x.ShipCity, map =>
			{
				map.Column("`ShipCity`");
			});
			Property(x => x.ShipRegion, map =>
			{
				map.Column("`ShipRegion`");
			});
			Property(x => x.ShipPostalCode, map =>
			{
				map.Column("`ShipPostalCode`");
			});
			Property(x => x.ShipCountry, map =>
			{
				map.Column("`ShipCountry`");
			});
			ManyToOne(x => x.Customers, map => 
			{
				map.Column("`CustomerID`");
				map.NotNullable(true);
				map.Cascade(Cascade.None);
			});

			ManyToOne(x => x.Employees, map => 
			{
				map.Column("`EmployeeID`");
				map.NotNullable(true);
				map.Cascade(Cascade.None);
			});

			ManyToOne(x => x.Shippers, map => 
			{
				map.Column("`ShipVia`");
				map.NotNullable(true);
				map.Cascade(Cascade.None);
			});

			Set(x => x.Orderdetails, colmap =>  { colmap.Key(x => x.Column("`OrderID`")); colmap.Inverse(true); }, map => { map.OneToMany(); });

			Cache(mapping =>
			{
				mapping.Region("region3");
				mapping.Usage(CacheUsage.NonstrictReadWrite);
			});
		}
    }
}
