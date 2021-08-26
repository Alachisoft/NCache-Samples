using System;

namespace Alachisoft.NCache.Samples
{
    class Program
    {
        static void Main(string[] args)
        {

                try
                {
                                    Alachisoft.NCache.Samples.LoaderAndRefresherUsage.Run();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
        }
    }
}
