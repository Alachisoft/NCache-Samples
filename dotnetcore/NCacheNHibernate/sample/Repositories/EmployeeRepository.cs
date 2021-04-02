using NHibernate.Linq;
using Sample.CustomerService.Domain;
using SampleApp.NHibernateHelpers;
using System.Collections.Generic;
using System.Linq;

namespace SampleApp.Repositories
{
    class EmployeeRepository
    {
        private readonly NHibernateHelper _nhibernateHelper;

        public EmployeeRepository(
            NHibernateHelper nhibernateHelper)
        {
            _nhibernateHelper = nhibernateHelper;
        }

        public IEnumerable<Employees> GetEmployees()
        {
            using (var sess = _nhibernateHelper.GetSessionFactory().OpenSession())
            {
                using (var tx = sess.BeginTransaction())
                {

                    var employees = sess.Query<Employees>()
                                    .Where(x => true)
                                    .WithOptions(options =>
                                    {
                                        options.SetCacheable(true);
                                        options.SetCacheRegion("region2");
                                    })
                                    .ToList();
                    tx.Commit();

                    return employees;
                }
            }
        }

        public bool SaveUpdateEmployee(Employees employee)
        {
            if (employee == null)
            {
                return false;
            }


            using (var sess = _nhibernateHelper.GetSessionFactory().OpenSession())
            {
                using (var tx = sess.BeginTransaction())
                {
                    sess.Save(employee);

                    tx.Commit();

                    return true;
                }
            }
        }
    }
}
