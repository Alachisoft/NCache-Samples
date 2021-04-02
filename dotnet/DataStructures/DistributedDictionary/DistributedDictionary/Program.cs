using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alachisoft.NCache.Samples
{
    class Program
    {
        static void Main (string[] args)
        {
            try
            {
                Alachisoft.NCache.Samples.DistributedDictionary.Run();

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);

            }
        }
    }
}
