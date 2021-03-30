using NHibernate;
using NHibernate.Linq;
using Sample.CustomerService.Domain;
using SampleApp.NHibernateHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleApp.Repositories
{
    public class CustomerRepository
    {
        private readonly NHibernateHelper _nhibernateHelper;

        public CustomerRepository(
            NHibernateHelper nhibernateHelper)
        {
            _nhibernateHelper = nhibernateHelper;
        }

        public IEnumerable<string> GetCustomers()
        {
            using (var sess = _nhibernateHelper.GetSessionFactory().OpenSession())
            {
                using (var tx = sess.BeginTransaction())
                {

                    var customers = sess.Query<Customers>()
                                    .Select(x => x.Id)
                                    .Where(x => true)
                                    .WithOptions(options =>
                                    {
                                        options.SetCacheable(true);
                                        options.SetCacheRegion("region3");
                                    })
                                    .ToList();
                    tx.Commit();

                    return customers;
                }
            }
        }

        public Customers GetCustomer(string customerID)
        {
            if (string.IsNullOrWhiteSpace(customerID))
            {
                throw new ArgumentNullException(nameof(customerID));
            }

            using (var sess = _nhibernateHelper.GetSessionFactory().OpenSession())
            {
                using (var tx = sess.BeginTransaction())
                {
                    var customer = sess.Query<Customers>()
                                    .Where(x => x.Id == customerID)
                                    .WithOptions(options =>
                                    {
                                        options.SetCacheable(true);
                                        options.SetCacheRegion("region3");
                                    })
                                    .FirstOrDefault();

                    tx.Commit();

                    return customer;
                }
            }
        }

        public IEnumerable<Customers> GetCustomerByCountry(string country)
        {
            using (var sess = _nhibernateHelper.GetSessionFactory().OpenSession())
            {
                using (var tx = sess.BeginTransaction())
                {

                    var customers = sess.Query<Customers>()
                                    .Where(x => x.Country == country)
                                    .WithOptions(options =>
                                    {
                                        options.SetCacheable(true);
                                        options.SetCacheRegion("region3");
                                    })
                                    .ToList();
                    tx.Commit();

                    return customers;
                }
            }
        }

        public IEnumerable<Orders> GetCustomerOrders(string customerID)
        {
            if (string.IsNullOrWhiteSpace(customerID))
            {
                throw new ArgumentNullException(nameof(customerID));
            }


            using (var sess = _nhibernateHelper.GetSessionFactory().OpenSession())
            {
                using (var tx = sess.BeginTransaction())
                {
                    var orders = sess.Query<Orders>()
                                    .Fetch(o => o.Customers)
                                    .Where(x => x.Customers.Id == customerID)
                                    .WithOptions(options =>
                                    {
                                        options.SetCacheable(true);
                                        options.SetCacheRegion("region3");
                                    })
                                    .ToList();

                    tx.Commit();

                    return orders;
                }
            }
        }

        public bool SaveUpdateCustomer(Customers customer)
        {
            if (customer == null)
            {
                return false;
            }


            using (var sess = _nhibernateHelper.GetSessionFactory().OpenSession())
            {
                using (var tx = sess.BeginTransaction())
                {
                    sess.Save(customer);

                    tx.Commit();

                    return true;
                }
            }
        }

        public bool RemoveCustomer(Customers customer)
        {
            if (customer == null)
            {
                return false;
            }

            using (var sess = _nhibernateHelper.GetSessionFactory().OpenSession())
            {
                using (var tx = sess.BeginTransaction())
                {
                    sess.Delete(customer);

                    tx.Commit();

                    return true;
                }
            }
        }
    }
}
