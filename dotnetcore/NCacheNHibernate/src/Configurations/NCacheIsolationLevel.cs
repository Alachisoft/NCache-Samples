using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Caches.NCache
{
    public enum NCacheIsolationLevel
    {
        Default = 0,
        InProc = 1,
        Outproc = 2
    }
}
