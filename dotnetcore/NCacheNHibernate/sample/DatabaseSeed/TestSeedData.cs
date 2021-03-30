using Bogus;
using NHibernate.Linq;
using Sample.CustomerService.Domain;
using SampleApp.NHibernateHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using static Bogus.DataSets.Name;

namespace SampleApp.DatabaseSeed
{
    public static class TestSeedData
    {
        public static void Initialize(NHibernateHelper nHibernateHelper)
        {
            Randomizer.Seed = new Random(8675309);

            var dummies = SeedPollingTable()
                .Generate(1)
                .First();

            using (var session = nHibernateHelper.GetSessionFactory().OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    session.Save(dummies);
                    tx.Commit();
                }
            }
        }

        public static void Initialize2(NHibernateHelper nHibernateHelper)
        {
            Randomizer.Seed = new Random(8675309);

            var dummies = SeedCustomers()
                .Generate(1)
                .First();

            using (var session = nHibernateHelper.GetSessionFactory().OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    session.Save(dummies);
                    tx.Commit();
                }
            }
        }

        public static void Initialize3(NHibernateHelper nHibernateHelper)
        {
            Randomizer.Seed = new Random(8675309);

            var t1 = SeedSuppliers()
                .Generate(1)
                .First();

            var t2 = SeedCategories()
                .Generate(1)
                .First();

            var dummies = SeedProducts()
                .Generate(150)
                .ToList();

            foreach (var dummy in dummies)
            {
                t1.AddProduct(dummy);
                t2.AddProduct(dummy); 
            }

            using (var session = nHibernateHelper.GetSessionFactory().OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    session.Save(t1);
                    session.Save(t2);
                    foreach (var dummy in dummies)
                    {
                        session.Save(dummy); 
                    }
                    tx.Commit();
                }
            }

            using (var session = nHibernateHelper.GetSessionFactory().OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    session.Query<Products>()
                        .Where(x => true)
                        .WithOptions(options =>
                        {
                            options.SetCacheable(true);
                        })
                        .ToList();

                    tx.Commit();
                }
            }
        }


        private static Faker<NCachePollingTable> SeedPollingTable()
        {
            return new Faker<NCachePollingTable>()
                .RuleFor(x => x.cache_id, faker => faker.Random.String2(5,5))
                .RuleFor(x => x.cache_key, faker => faker.Random.String2(5, 5))
                .RuleFor(x => x.modified, faker => false)
                .RuleFor(x => x.work_in_progress, faker => true);
        }

        private static Faker<Region> SeedRegions()
        {
            return new Faker<Region>()
                .RuleFor(x => x.RegionDescription, faker => faker.Lorem.Sentence(15, 10))
                .RuleFor(x => x.Territories, faker => new HashSet<Territories>());
        }


        private static Faker<Customers> SeedCustomers()
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

        private static Faker<Products> SeedProducts()
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

        private static Faker<Categories> SeedCategories()
        {
            return new Faker<Categories>()
                .RuleFor(x => x.CategoryName, faker => RandomWord(faker))
                .RuleFor(x => x.Description, faker => RandomSentence(faker, 10, 5))
                .RuleFor(x => x.Picture, faker => new byte[] { 255 })
                .RuleFor(x => x.Products, faker => new HashSet<Products>());
        }

        private static Faker<Suppliers> SeedSuppliers()
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
