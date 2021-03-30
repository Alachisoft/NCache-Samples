using NHibernate.Linq;
using Sample.CustomerService.Domain;
using SampleApp.NHibernateHelpers;
using System.Collections.Generic;
using System.Linq;

namespace SampleApp.Repositories
{
    public class ProductRepository
    {
        private readonly NHibernateHelper _nhibernateHelper;

        public ProductRepository(
            NHibernateHelper nhibernateHelper)
        {
            _nhibernateHelper = nhibernateHelper;
        }

        public IEnumerable<Products> GetProducts()
        {
            using (var sess = _nhibernateHelper.GetSessionFactory().OpenSession())
            {
                using (var tx = sess.BeginTransaction())
                {

                    var products = sess.Query<Products>()
                                    .Where(x => true)
                                    .WithOptions(options =>
                                    {
                                        options.SetCacheable(true);
                                        options.SetCacheRegion("region3");
                                    })
                                    .ToList();
                    tx.Commit();

                    return products;
                }
            }
        }
    }
}
