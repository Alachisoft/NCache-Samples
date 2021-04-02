using System.Collections.Generic;

namespace NHibernate.Caches.NCache
{
    public class QueryDependencyConfigComparer :
                    IEqualityComparer<QueryDependencyConfiguration>
    {
        public bool Equals(
                        QueryDependencyConfiguration x,
                        QueryDependencyConfiguration y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(
                        QueryDependencyConfiguration obj)
        {
            if (obj == null)
            {
                return -1;
            }

            return obj.GetHashCode();
        }
    }
}
