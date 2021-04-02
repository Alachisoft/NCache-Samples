using NHibernate;

namespace SampleApp.NHibernateHelpers
{
    public abstract class NHibernateHelper
    {
        public abstract ISessionFactory GetSessionFactory();

        public abstract void CloseSessionFactory();
    }
}
