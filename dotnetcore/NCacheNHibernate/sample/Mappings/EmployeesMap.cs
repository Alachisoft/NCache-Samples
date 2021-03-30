using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using Sample.CustomerService.Domain;


namespace Sample.CustomerService.Maps 
{
    
    
    public class EmployeesMap : ClassMapping<Employees> 
	{
        
        public EmployeesMap() {
			Table("`Employees`");
			Lazy(true);
			Id(x => x.Id, map =>
			{
				map.Generator(Generators.Identity);
				map.Column("`EmployeeID`");
			});
			Property(x => x.LastName, map =>
			{
				map.Column("`LastName`");
				map.NotNullable(true);
			});
			Property(x => x.FirstName, map =>
			{
				map.Column("`FirstName`");
				map.NotNullable(true);
			});
			Property(x => x.Title, map =>
			{
				map.Column("`Title`");
			});
			Property(x => x.TitleOfCourtesy, map =>
			{
				map.Column("`TitleOfCourtesy`");
			});
			Property(x => x.BirthDate, map =>
			{
				map.Column("`BirthDate`");
			});
			Property(x => x.HireDate, map =>
			{
				map.Column("`HireDate`");
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
			Property(x => x.Postalcode, map =>
			{
				map.Column("`Postalcode`");
			});
			Property(x => x.Country, map =>
			{
				map.Column("`Country`");
			});
			Property(x => x.Homephone, map =>
			{
				map.Column("`HomePhone`");
			});
			Property(x => x.Extension, map =>
			{
				map.Column("`Extension`");
			});
			Property(x => x.Photo, map =>
			{
				map.Column("`Photo`");
			});
			Property(x => x.Notes, map =>
			{
				map.Column("`Notes`");
			});
			Property(x => x.Photopath, map =>
			{
				map.Column("`PhotoPath`");
			});
			ManyToOne(x => x.EmployeesVal, map =>
			{
				map.Column("`ReportsTo`");
				map.NotNullable(false);
				map.Cascade(Cascade.None);
			});

			Set(x => x.Employeeterritories, colmap =>  { colmap.Key(x => x.Column("`EmployeeID`")); colmap.Inverse(true); }, map => { map.OneToMany(); });
			Set(x => x.Orders, colmap =>  { colmap.Key(x => x.Column("`EmployeeID`")); colmap.Inverse(true); }, map => { map.OneToMany(); });

			Cache(mapping =>
			{
				mapping.Region("region2");
				mapping.Usage(CacheUsage.NonstrictReadWrite);
			});
		}
    }
}
