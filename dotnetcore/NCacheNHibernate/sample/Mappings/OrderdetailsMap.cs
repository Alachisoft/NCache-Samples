using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using Sample.CustomerService.Domain;


namespace Sample.CustomerService.Maps {
    
    
    public class OrderdetailsMap : ClassMapping<Orderdetails> {
        
        public OrderdetailsMap() {
			Table("`Order Details`");
			Lazy(true);
			ComposedId(compId =>
				{
					compId.ManyToOne(x => x.Orders, map =>
					{
						map.Column("`OrderID`");
						map.Cascade(Cascade.None);
					});

					compId.ManyToOne(x => x.Products, map =>
					{
						map.Column("`ProductID`");
						map.Cascade(Cascade.None);
					});
				});
			Property(x => x.UnitPrice, map =>
			{
				map.Column("`UnitPrice`");
				map.NotNullable(true);
			});
			Property(x => x.Quantity, map =>
			{
				map.Column("`Quantity`");
				map.NotNullable(true);
			});
			Property(x => x.Discount, map =>
			{
				map.Column("`Discount`");
				map.NotNullable(true);
			});

			Cache(mapping =>
			{
				mapping.Region("region3");
				mapping.Usage(CacheUsage.NonstrictReadWrite);
			});

		}
    }
}
