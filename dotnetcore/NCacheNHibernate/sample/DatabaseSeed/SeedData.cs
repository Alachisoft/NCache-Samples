using Bogus;
using Sample.CustomerService.Domain;
using SampleApp.NHibernateHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using static Bogus.DataSets.Name;

namespace SampleApp.DatabaseSeed
{
    public static class SeedData
    {
        static SeedData()
        {
            Randomizer.Seed = new Random(8675309);
        }
        public static void Initialize(NHibernateHelper nHibernateHelper)
        {
            var categories = SeedCategories()
                .Generate(1)
                .ToList();

            var suppliers = SeedSuppliers()
                .Generate(1)
                .ToList();

            var products = new List<Products>();

            foreach (var category in categories)
            {
                foreach (var supplier in suppliers)
                {
                    var products1 = SeedProducts()
                        .Generate(5)
                        .ToList();

                    foreach (var product in products1)
                    {
                        category.AddProduct(product);
                        supplier.AddProduct(product);
                        products.Add(product);
                    }
                }
            }

            var customers = SeedCustomers()
                .Generate(5)
                .ToList();

            var customerDemographics = SeedCustomerDemographics()
                .Generate(1)
                .ToList();

            var employees = SeedEmployees()
                .Generate(4)
                .ToList();

            var shippers = SeedShippers()
                .Generate(1)
                .ToList();

            var orders = new List<Orders>();

            foreach (var customer in customers)
            {
                foreach (var employee in employees)
                {
                    foreach (var shipper in shippers)
                    {
                        var order = SeedOrders()
                            .Generate(1)
                            .First();

                        customer.AddOrder(order);
                        employee.AddOrder(order);
                        shipper.AddOrder(order);
                        orders.Add(order);
                    }
                }
            }

            var customerCustomerDemos = new List<Customercustomerdemo>();

            foreach (var customer in customers)
            {
                foreach (var customerDemo in customerDemographics)
                {
                    var customerCustomerDemo = new Customercustomerdemo();
                    customer.AddCustomerDemo(customerCustomerDemo);
                    customerDemo.AddCustomerDemo(customerCustomerDemo);
                    customerCustomerDemos.Add(customerCustomerDemo);
                }
            }

            List<Orderdetails> orderDetails = new List<Orderdetails>();

            foreach (var order in orders)
            {
                foreach (var product in products)
                {
                    var orderDetail = SeedOrderDetails()
                        .Generate(1)
                        .First();

                    order.AddOrderDetail(orderDetail);
                    product.AddOrderDetail(orderDetail);
                    orderDetails.Add(orderDetail);
                }
            }

            var regions = SeedRegions()
                .Generate(1)
                .ToList();

            var territories = new List<Territories>();

            foreach (var region in regions)
            {
                var territories1 = SeedTerritories()
                    .Generate(2)
                    .ToList();

                foreach (var territory in territories1)
                {
                    region.AddTerritory(territory);
                    territories.Add(territory);
                }
            }

            var employeeTerritories = new List<Employeeterritories>();

            foreach (var employee in employees)
            {
                foreach (var territory in territories)
                {
                    var employeeTerritory = new Employeeterritories();
                    employee.AddEmployeeTerritory(employeeTerritory);
                    territory.AddEmployeeTerritory(employeeTerritory);
                    employeeTerritories.Add(employeeTerritory);
                }
            }

            foreach (var region in regions)
            {
                using (var session = nHibernateHelper.GetSessionFactory().OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        session.Save(region);
                        tx.Commit();
                    }
                }
            }

            foreach (var territory in territories)
            {
                using (var session = nHibernateHelper.GetSessionFactory().OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        session.Save(territory);
                        tx.Commit();
                    }
                }
            }

            foreach (var customer in customers)
            {
                using (var session = nHibernateHelper.GetSessionFactory().OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        session.Save(customer);
                        tx.Commit();
                    }
                }
            }

            foreach (var customerDemo in customerDemographics)
            {
                using (var session = nHibernateHelper.GetSessionFactory().OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        session.Save(customerDemo);
                        tx.Commit();
                    }
                }
            }

            foreach (var customerCustomerDemo in customerCustomerDemos)
            {
                using (var session = nHibernateHelper.GetSessionFactory().OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        session.Save(customerCustomerDemo);
                        tx.Commit();
                    }
                }
            }

            foreach (var category in categories)
            {
                using (var session = nHibernateHelper.GetSessionFactory().OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        session.Save(category);
                        tx.Commit();
                    }
                }
            }

            foreach (var supplier in suppliers)
            {
                using (var session = nHibernateHelper.GetSessionFactory().OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        session.Save(supplier);
                        tx.Commit();
                    }
                }
            }

            foreach (var product in products)
            {
                using (var session = nHibernateHelper.GetSessionFactory().OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        session.Save(product);
                        tx.Commit();
                    }
                }
            }

            foreach (var employee in employees)
            {
                using (var session = nHibernateHelper.GetSessionFactory().OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        session.Save(employee);
                        tx.Commit();
                    }
                }
            }

            foreach (var employeeTerritory in employeeTerritories)
            {
                using (var session = nHibernateHelper.GetSessionFactory().OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        session.Save(employeeTerritory);
                        tx.Commit();
                    }
                }
            }

            foreach (var shipper in shippers)
            {
                using (var session = nHibernateHelper.GetSessionFactory().OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        session.Save(shipper);
                        tx.Commit();
                    }
                }
            }

            foreach (var order in orders)
            {
                using (var session = nHibernateHelper.GetSessionFactory().OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        session.Save(order);
                        tx.Commit();
                    }
                }
            }

            foreach (var orderDetail in orderDetails)
            {
                using (var session = nHibernateHelper.GetSessionFactory().OpenSession())
                {
                    using (var tx = session.BeginTransaction())
                    {
                        session.Save(orderDetail);
                        tx.Commit();
                    }
                }
            }

        }

        public static Faker<Categories> SeedCategories()
        {
            return new Faker<Categories>()
                .RuleFor(x => x.CategoryName, faker => RandomWord(faker))
                .RuleFor(x => x.Description, faker => RandomSentence(faker, 10, 5))
                .RuleFor(x => x.Picture, faker => new byte[] { 255 })
                .RuleFor(x => x.Products, faker => new HashSet<Products>());
        }

        public static Faker<Suppliers> SeedSuppliers()
        {
            return new Faker<Suppliers>()
                .RuleFor(x => x.Address, faker => RandomStreetAddress(faker))
                .RuleFor(x => x.City, faker => RandomCity(faker))
                .RuleFor(x => x.ContactName, faker => RandomFullName(faker))
                .RuleFor(x => x.ContactTitle, faker => RandomJobTitle(faker))
                .RuleFor(x => x.CompanyName, faker => RandomCompanyName(faker))
                .RuleFor(x => x.PostalCode, faker => RandomZipCode(faker))
                .RuleFor(x => x.Region, faker => RandomState(faker))
                .RuleFor(x => x.Country, faker => RandomCountry(faker))
                .RuleFor(x => x.Phone, faker => RandomPhoneFax(faker))
                .RuleFor(x => x.Fax, faker => RandomPhoneFax(faker))
                .RuleFor(x => x.HomePage, faker => RandomWebSite(faker))
                .RuleFor(x => x.Products, faker => new HashSet<Products>());
        }

        public static Faker<Shippers> SeedShippers()
        {
            return new Faker<Shippers>()
                .RuleFor(x => x.CompanyName, faker => RandomCompanyName(faker))
                .RuleFor(x => x.Phone, faker => RandomPhoneFax(faker))
                .RuleFor(x => x.Orders, faker => new HashSet<Orders>());
        }

        public static Faker<Region> SeedRegions()
        {
            return new Faker<Region>()
                .RuleFor(x => x.RegionDescription, faker => RandomSentence(faker, 15, 10))
                .RuleFor(x => x.Territories, faker => new HashSet<Territories>());
        }

        public static Faker<Customers> SeedCustomers()
        {
            return new Faker<Customers>()
                .RuleFor(x => x.Address, faker => RandomStreetAddress(faker))
                .RuleFor(x => x.City, faker => RandomCity(faker))
                .RuleFor(x => x.ContactName, faker => RandomFullName(faker))
                .RuleFor(x => x.ContactTitle, faker => RandomJobTitle(faker))
                .RuleFor(x => x.CompanyName, faker => RandomCompanyName(faker))
                .RuleFor(x => x.Country, faker => RandomCountry(faker))
                .RuleFor(x => x.Region, faker => RandomState(faker))
                .RuleFor(x => x.Phone, faker => RandomPhoneFax(faker))
                .RuleFor(x => x.Postalcode, faker => RandomZipCode(faker))
                .RuleFor(x => x.Fax, faker => RandomPhoneFax(faker))
                .RuleFor(x => x.Id, faker => RandomID(faker, 5, 5))
                .RuleFor(x => x.Orders, faker => new HashSet<Orders>())
                .RuleFor(x => x.Customercustomerdemo, faker => new HashSet<Customercustomerdemo>());
        }

        public static Faker<Products> SeedProducts()
        {
            return new Faker<Products>()
                .RuleFor(x => x.ProductName, faker => RandomProductName(faker))
                .RuleFor(x => x.QuantityPerUnit, faker => "" + RandomInt(faker))
                .RuleFor(x => x.UnitPrice, faker => RandomDecimal(faker))
                .RuleFor(x => x.UnitsInStock, faker => RandomShort(faker, 0, 100))
                .RuleFor(x => x.UnitsOnOrder, faker => RandomShort(faker, 0, 50))
                .RuleFor(x => x.ReorderLevel, faker => RandomShort(faker, 1, 10))
                .RuleFor(x => x.Discontinued, faker => RandomBool(faker))
                .RuleFor(x => x.Suppliers, faker => null)
                .RuleFor(x => x.Categories, faker => null)
                .RuleFor(x => x.Orderdetails, faker => new HashSet<Orderdetails>());
        }

        public static Faker<Orders> SeedOrders()
        {
            return new Faker<Orders>()
                .RuleFor(x => x.ShipAddress, faker => RandomStreetAddress(faker))
                .RuleFor(x => x.ShipCity, faker => RandomCity(faker))
                .RuleFor(x => x.ShipCountry, faker => RandomCountry(faker))
                .RuleFor(x => x.ShipName, faker => RandomShipName(faker))
                .RuleFor(x => x.ShipPostalCode, faker => RandomZipCode(faker))
                .RuleFor(x => x.ShipRegion, faker => RandomState(faker))
                .RuleFor(x => x.OrderDate, faker => RandomDateTime(faker, 100, 70))
                .RuleFor(x => x.ShippedDate, faker => RandomDateTime(faker, 60, 30))
                .RuleFor(x => x.RequiredDate, faker => RandomDateTime(faker, 20, 10))
                .RuleFor(x => x.Customers, faker => null)
                .RuleFor(x => x.Employees, faker => null)
                .RuleFor(x => x.Shippers, faker => null)
                .RuleFor(x => x.Orderdetails, faker => new HashSet<Orderdetails>());
        }

        public static Faker<Customerdemographics> SeedCustomerDemographics()
        {
            return new Faker<Customerdemographics>()
                .RuleFor(x => x.Id, faker => RandomID(faker, 5, 5))
                .RuleFor(x => x.Customerdesc, faker => RandomParagraph(faker))
                .RuleFor(x => x.Customercustomerdemo, faker => new HashSet<Customercustomerdemo>());
        }

        public static Faker<Employees> SeedEmployees()
        {
            return new Faker<Employees>()
                .RuleFor(x => x.Address, faker => RandomStreetAddress(faker))
                .RuleFor(x => x.BirthDate, faker => RandomDateTime(faker, 60 * 365, 18 * 365))
                .RuleFor(x => x.City, faker => RandomCity(faker))
                .RuleFor(x => x.Country, faker => RandomCountry(faker))
                .RuleFor(x => x.FirstName, faker => RandomFirstName(faker))
                .RuleFor(x => x.LastName, faker => RandomLastName(faker))
                .RuleFor(x => x.Title, faker => RandomJobTitle(faker))
                .RuleFor(x => x.TitleOfCourtesy, faker => RandomTitleOfCourtest(faker))
                .RuleFor(x => x.HireDate, faker => RandomDateTime(faker, 5 * 365, 2 * 365))
                .RuleFor(x => x.Photo, faker => new byte[] { 255 })
                .RuleFor(x => x.Photopath, faker => RandomWebSite(faker))
                .RuleFor(x => x.Region, faker => RandomState(faker))
                .RuleFor(x => x.Postalcode, faker => RandomZipCode(faker))
                .RuleFor(x => x.Homephone, faker => RandomPhoneFax(faker))
                .RuleFor(x => x.Extension, faker => "" + RandomInt(faker, 100))
                .RuleFor(x => x.Notes, faker => RandomParagraph(faker))
                .RuleFor(x => x.Orders, faker => new HashSet<Orders>())
                .RuleFor(x => x.Employeeterritories, faker => new HashSet<Employeeterritories>())
                .RuleFor(x => x.EmployeesVal, faker => null);
        }

        public static Faker<Orderdetails> SeedOrderDetails()
        {
            return new Faker<Orderdetails>()
                .RuleFor(x => x.Discount, faker => RandomFloat(faker, 5f))
                .RuleFor(x => x.UnitPrice, faker => RandomDecimal(faker))
                .RuleFor(x => x.Quantity, faker => RandomShort(faker, 1, 3))
                .RuleFor(x => x.Orders, faker => null)
                .RuleFor(x => x.Products, faker => null);
        }

        public static Faker<Territories> SeedTerritories()
        {
            return new Faker<Territories>()
                .RuleFor(x => x.Id, faker => RandomID(faker, 5, 5))
                .RuleFor(x => x.Territorydescription, faker => RandomParagraph(faker))
                .RuleFor(x => x.Region, faker => null)
                .RuleFor(x => x.Employeeterritories, faker => new HashSet<Employeeterritories>());
        }


        private static string RandomWord(Faker faker)
        {
            return faker.Lorem.Word();
        }

        private static string RandomSentence(Faker faker, int i, int j)
        {
            return faker.Lorem.Sentence(i, j);
        }

        private static string RandomStreetAddress(Faker faker)
        {
            return faker.Address.StreetAddress();
        }

        private static string RandomCity(Faker faker)
        {
            return faker.Address.City();
        }

        private static string RandomFullName(Faker faker)
        {
            return faker.Person.FullName;
        }

        public static string RandomFirstName(Faker faker)
        {
            return faker.Name.FirstName();
        }

        private static string RandomLastName(Faker faker)
        {
            return faker.Name.LastName();
        }

        private static string RandomJobTitle(Faker faker)
        {
            return faker.Name.JobTitle();
        }

        private static string RandomTitleOfCourtest(Faker faker)
        {
            return faker.PickRandom<string>(new List<string> { "Mr.", "Ms." });
        }

        private static string RandomCompanyName(Faker faker)
        {
            return faker.Company.CompanyName();
        }

        private static string RandomZipCode(Faker faker)
        {
            return faker.Address.ZipCode();
        }

        private static string RandomState(Faker faker)
        {
            return faker.Address.State();
        }

        private static string RandomCountry(Faker faker)
        {
            return faker.Address.Country();
        }

        private static string RandomPhoneFax(Faker faker)
        {
            return faker.Phone.PhoneNumber();
        }

        private static string RandomWebSite(Faker faker)
        {
            return faker.Person.Website;
        }

        private static string RandomID(Faker faker, int i, int j)
        {
            return faker.Random.String2(i, j).ToUpperInvariant();
        }

        private static string RandomProductName(Faker faker)
        {
            return faker.Commerce.ProductName();
        }

        private static int RandomInt(Faker faker, int j = 10)
        {
            return faker.Random.Int(1, j);
        }

        private static decimal RandomDecimal(Faker faker)
        {
            return faker.Random.Decimal(0.4m) * 100m;
        }

        private static short RandomShort(Faker faker, short i, short j)
        {
            return faker.Random.Short(i, j);
        }

        private static bool RandomBool(Faker faker)
        {
            return faker.Random.Bool(0.05f);
        }

        private static float RandomFloat(Faker faker, float j)
        {
            return faker.Random.Float() * j;
        }

        private static string RandomParagraph(Faker faker)
        {
            return faker.Lorem.Paragraph(3).Substring(0, 25);
        }

        private static string RandomShipName(Faker faker)
        {
            return faker.Name.FirstName(Gender.Female);
        }

        private static DateTime RandomDateTime(Faker faker, double i, double j = 0)
        {
            return faker.Date.Between(
                DateTime.UtcNow - TimeSpan.FromDays(i),
                DateTime.UtcNow - TimeSpan.FromDays(j));
        }

    }
}
