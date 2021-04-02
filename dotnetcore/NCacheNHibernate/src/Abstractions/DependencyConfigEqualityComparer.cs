using System.Collections.Generic;

namespace NHibernate.Caches.NCache
{
    public class DependecyConfigEqualityComparer :
            IEqualityComparer<DependencyConfig>
    {
        public bool Equals(DependencyConfig x, DependencyConfig y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(DependencyConfig obj)
        {
            if (obj == null)
            {
                return -1;
            }

            return obj.GetHashCode();
        }
    }
}
