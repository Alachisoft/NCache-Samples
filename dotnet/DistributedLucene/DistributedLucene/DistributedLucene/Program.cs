using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alachisoft.NCache.Samples
{
    /// ******************************************************************************
    /// <summary>
    /// A sample program that shows the usage of Distributed Lucene with  NCache.
    /// 
    /// Requirements:
    ///     1. A running NCache cache
    ///     2. Cache ID in app.config
    ///     3. Lucene Indexes enabled from NCache Web Manager, Full Test Search Index tab. 
    ///     4. Client Activity Notifications enabled on cache. 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new Alachisoft.NCache.Samples.LuceneService().Run();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Console.Read();
            }
        }
    }
}
