using System;

namespace Dictionary
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
